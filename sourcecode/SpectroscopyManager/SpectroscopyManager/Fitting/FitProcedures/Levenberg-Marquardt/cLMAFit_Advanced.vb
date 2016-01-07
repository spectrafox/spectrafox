Imports System.Diagnostics
Imports System.Threading
Imports System.ComponentModel

Imports MathNet.Numerics.LinearAlgebra.Double

Public Class cLMAFit_Advanced
    Implements iFitProcedure

    ' default end conditions
    Private Const DefaultMinDeltaChi2 As Double = 1.0E-30
    Private Const DefaultMaxIterations As Integer = 100
    Private Const DefaultDifferentiationStep As Double = 0.00001
    Public Const LambdaConst As Double = 0.0001

#Region "Custom Fit-Procedure Setting-Class"

    ''' <summary>
    ''' Custom implementation of the settings-class used for this fit-procedure.
    ''' </summary>
    Public Class cFitProcedureSettings
        Implements iFitProcedureSettings

        ''' <summary>
        ''' Returns the type of the fit-procedure class.
        ''' </summary>
        Public ReadOnly Property BaseClass As Type Implements iFitProcedureSettings.BaseClass
            Get
                Return GetType(cLMAFit_Advanced)
            End Get
        End Property

        ''' <summary>
        ''' Sets the minimum change in Chi^2 for which to abort the fit
        ''' </summary>
        Public Property MinDeltaChi2 As Double = DefaultMinDeltaChi2

        ''' <summary>
        ''' Sets the maximum number of iterations, after which to abort the fit.
        ''' </summary>
        Public Property MaxIteration As Integer = DefaultMaxIterations

        ''' <summary>
        ''' Sets the smallest step between numeric differentiations
        ''' </summary>
        Public Property DifferentiationStep As Double = DefaultDifferentiationStep

        ''' <summary>
        ''' Echo the Settings
        ''' </summary>
        Public Function EchoSettings() As String Implements iFitProcedureSettings.EchoSettings
            Dim SB As New System.Text.StringBuilder
            SB.AppendLine("maximum iteration count:  " & Me.MaxIteration.ToString("N0"))
            SB.AppendLine("min change in Chi^2:      " & Me.MinDeltaChi2.ToString("E3"))
            SB.Append("numeric derivative delta: " & Me.DifferentiationStep.ToString("E3"))
            Return SB.ToString
        End Function
    End Class

#End Region

#Region "Fit-Procedure-Settings"

    ''' <summary>
    ''' Fit-Procedure-Settings
    ''' </summary>
    Private ThisFitProcedureSettings As New cFitProcedureSettings

    ''' <summary>
    ''' Property to save the procedure-specific fit-settings.
    ''' </summary>
    Public Property FitProcedureSettings As iFitProcedureSettings Implements iFitProcedure.FitProcedureSettings
        Get
            Return ThisFitProcedureSettings
        End Get
        Set(value As iFitProcedureSettings)
            Me.ThisFitProcedureSettings = CType(value, cFitProcedureSettings)
        End Set
    End Property

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Public ReadOnly Property ProcedureSettingPanel As cFitProcedureSettingsPanel Implements iFitProcedure.ProcedureSettingPanel
        Get
            Dim SP As New cFitProcedureSettingsPanel_LMAAdvanced
            SP.FitProcedureSettings = Me.ThisFitProcedureSettings
            Return SP
        End Get
    End Property

#End Region

#Region "Parameters"
    ''' <summary>
    ''' interface of the function to be fitted
    ''' </summary>
    Private FitFunction As iFitFunction

    ''' <summary>
    ''' The array of fit parameters.
    ''' </summary>
    Private _FitParameters As Dictionary(Of Integer, sFitParameter)

    ''' <summary>
    ''' Count of the non-fixed FitParameters.
    ''' </summary>
    Private NonFixedFitParameterCount As Integer

    ''' <summary>
    ''' Keys of all non-fixed FitParameters.
    ''' </summary>
    Private NonFixedFitParameterKeys As List(Of Integer)

    ''' <summary>
    ''' Parameters incremented by value of lambda
    ''' </summary>
    Private IncrementedParameters As Dictionary(Of Integer, sFitParameter)

    ''' <summary>
    ''' Parameters incremented by value of lambda
    ''' </summary>
    Private IncrementedParametersBest As Dictionary(Of Integer, sFitParameter)

    ''' <summary>
    ''' Measured data points for which the model function is to be fitted.
    ''' double[0 = x, 1 = y][data point index] = data value
    ''' </summary>
    Private DataPoints As Double()()

    ''' <summary>
    ''' Weights for each data point. The merit function is: chi2 = sum[ (y_i - y(x_i;a))^2 * w_i ].
    ''' For gaussian errors in datapoints, set w_i = 1 / sigma_i.
    ''' </summary>
    Private Weights As Double()

    ''' <summary>
    ''' Hessian Matrix
    ''' </summary>
    Private alpha As DenseMatrix

    ''' <summary>
    ''' Gradient vector
    ''' </summary>
    Private beta As Double()

    ''' <summary>
    ''' FitParameter Step array
    ''' </summary>
    Private da As Double()

    ''' <summary>
    ''' temporary calculated Chi2 value
    ''' </summary>
    Private _Chi2 As Double

    ''' <summary>
    ''' Damping parameter
    ''' </summary>
    Private lambda As Double

    ''' <summary>
    ''' Step size
    ''' </summary>
    Private delta As Double
    Private IncrementedChi2 As Double
    Private IterationCount As Integer
#End Region

#Region "Stop Conditions and Fit-Time"

    ' Variable for saving the Fit-Duration
    Private _FitStartTime As Date
    Private _FitEndTime As Date

    ''' <summary>
    ''' Variable that saves the reason for the procedure to stop.
    ''' </summary>
    Private StopReason As FitStopReason
#End Region

#Region "Stop-Reasons"
    ''' <summary>
    ''' Reasons for the Fit-Procedure to end.
    ''' </summary>
    Public Enum FitStopReason
        None
        FitConverged
        MaxIterationsReached
        UserAborted
        MatrixSingular
        FunctionEvaluationContainedNaN
        JacobianContainedNaN
    End Enum

    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Public Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String Implements iFitProcedure.ConvertFitStopCodeToMessage
        Select Case FitStopCode
            Case cLMAFit.FitStopReason.FitConverged
                Return My.Resources.rLMAFit.FitStopReason_FitConverged.Replace("#d", Me.ThisFitProcedureSettings.MinDeltaChi2.ToString("E3"))
            Case cLMAFit.FitStopReason.MaxIterationsReached
                Return My.Resources.rLMAFit.FitStopReason_MaxIterationsReached.Replace("#d", Me.ThisFitProcedureSettings.MaxIteration.ToString("N0"))
            Case cLMAFit.FitStopReason.UserAborted
                Return My.Resources.rLMAFit.FitStopReason_UserAborted
            Case cLMAFit.FitStopReason.MatrixSingular
                Return My.Resources.rLMAFit.FitStopReason_MatrixSingular
            Case Else
                Return "unknown fit stop reason"
        End Select
    End Function
#End Region

#Region "Fit-Thread-Worker"
    ''' <summary>
    ''' Single Thread used for Async fitting.
    ''' </summary>
    Private WithEvents FitThreadWorker As New BackgroundWorker

    ''' <summary>
    ''' Single Thread used for Async fitting.
    ''' </summary>
    Public ReadOnly Property FitWorker As BackgroundWorker Implements iFitProcedure.FitThreadWorker
        Get
            Return Me.FitThreadWorker
        End Get
    End Property
#End Region

#Region "Events"

    ''' <summary>
    ''' Echos the current parameter set, whenever
    ''' the parameters got changed to a minimized Chi2. 
    ''' </summary>
    Public Event FitStepEcho(ByVal Parameters As Dictionary(Of Integer, sFitParameter),
                             ByVal Chi2 As Double) Implements iFitProcedure.FitStepEcho

    ''' <summary>
    ''' Echos the current parameter set, whenever
    ''' the parameters got changed to a minimized Chi2. 
    ''' </summary>
    Public Event FitEcho(ByVal Message As String) Implements iFitProcedure.FitEcho

    ''' <summary>
    ''' Echoed, if the fit procedure has ended.
    ''' </summary>
    Public Event FitFinished(ByVal FitStopReason As Integer,
                             ByVal FinalParameters As Dictionary(Of Integer, sFitParameter),
                             ByVal Chi2 As Double) Implements iFitProcedure.FitFinished

    ''' <summary>
    ''' An event to listen to, if you want to show a progress bar for single steps.
    ''' </summary>
    Public Event CalculationStepProgress(ByVal CalcItem As Integer,
                                         ByVal CalcMax As Integer,
                                         ByVal StepDescription As String) Implements iFitProcedure.CalculationStepProgress

#End Region

#Region "Constructor with the inital settings"
    ''' <summary>
    ''' Constructor of the advanced Levenberg-Marquardt-Fit (LMA)-Function
    ''' </summary>
    Public Sub New()
        ' Set Properties of the BackgroundWorker Thread
        Me.FitThreadWorker.WorkerSupportsCancellation = True
        Me.FitThreadWorker.WorkerReportsProgress = True
    End Sub
#End Region

#Region "Properties"
    ''' <summary>
    ''' Fit-Procedure-Name
    ''' </summary>
    Public ReadOnly Property Name As String Implements iFitProcedure.Name
        Get
            Return My.Resources.rLMAFit.LMA_Advanced_Name
        End Get
    End Property

    ''' <summary>
    ''' Returns the parameter set of the current Fit.
    ''' </summary>
    Public ReadOnly Property FitParameters() As Dictionary(Of Integer, sFitParameter) Implements iFitProcedure.FitParameters
        Get
            Return Me._FitParameters
        End Get
    End Property

    ''' <summary>
    ''' Returns the number of iterations used for the fit.
    ''' </summary>
    Public ReadOnly Property Iterations() As Integer Implements iFitProcedure.Iterations
        Get
            Return Me.IterationCount
        End Get
    End Property

    ''' <summary>
    ''' Returns Chi2 of the fit.
    ''' </summary>
    Public ReadOnly Property Chi2() As Double
        Get
            Return Me._Chi2
        End Get
    End Property

    ''' <summary>
    ''' Returns the current FitThread status
    ''' </summary>
    Public ReadOnly Property AsyncFitRunning As Boolean
        Get
            Return Me.FitThreadWorker.IsBusy
        End Get
    End Property

    ''' <summary>
    ''' Returns the time the Fit needed
    ''' </summary>
    Public ReadOnly Property FitDuration As TimeSpan Implements iFitProcedure.FitDuration
        Get
            If Me._FitEndTime = Nothing Then
                Return Date.Now - Me._FitStartTime
            Else
                Return Me._FitEndTime - Me._FitStartTime
            End If
        End Get
    End Property

    ''' <summary>
    ''' Time at which the Fit has been started
    ''' </summary>
    Public ReadOnly Property FitStartTime As Date Implements iFitProcedure.FitStartTime
        Get
            Return Me._FitStartTime
        End Get
    End Property

    ''' <summary>
    ''' Time at which the Fit has ended
    ''' </summary>
    Public ReadOnly Property FitEndTime As Date Implements iFitProcedure.FitEndTime
        Get
            Return Me._FitEndTime
        End Get
    End Property

    Private _FinalStatistics As cNumericalMethods.sNumericStatistics
    ''' <summary>
    ''' Statistics, that get calculated after the fit has finished
    ''' </summary>
    Public ReadOnly Property FinalStatistics As cNumericalMethods.sNumericStatistics Implements iFitProcedure.Statistics
        Get
            Return Me._FinalStatistics
        End Get
    End Property
#End Region

#Region "Fit-Start/-Stop and Reporting functions"
    ''' <summary>
    ''' The direct fit.
    ''' </summary>
    Public Sub DirectFit(ByVal ModelFitFunction As iFitFunction,
                         ByVal FitDataPoints As Double()(),
                         ByVal Weights As Double()) Implements iFitProcedure.DirectFit
        Me.StartFit(ModelFitFunction, FitDataPoints, Weights, False)
    End Sub

    ''' <summary>
    ''' Starts the fit in a background thread. If used after calling fit(lambda, minDeltaChi2, maxIterations),
    ''' uses those values. The stop condition is fetched from <code>this.stop()</code>.
    ''' Override <code>this.stop()</code> if you want to use another stop condition.
    ''' </summary>
    ''' <returns>True if new thread could be started. False, if thread was already running.</returns>
    Public Function FitAsync(ByVal ModelFitFunction As iFitFunction,
                             ByVal FitDataPoints As Double()(),
                             ByVal Weights As Double()) As Boolean Implements iFitProcedure.FitAsync
        Return Me.StartFit(ModelFitFunction, FitDataPoints, Weights, True)
    End Function

    ''' <summary>
    ''' Starts the fit in a background thread or directly, as defined. 
    ''' </summary>
    Public Function StartFit(ByRef ModelFitFunction As iFitFunction,
                             ByRef FitDataPoints As Double()(),
                             ByRef Weights As Double(),
                             ByVal StartAsync As Boolean) As Boolean
        ' Check for running Fit-Worker.
        If Me.FitThreadWorker.IsBusy And StartAsync Then
            Return False
        End If

        ' Integritiy Check of the Data
        If FitDataPoints.Length <> 2 Then
            Throw New ArgumentException("Data point array must be 2 x N")
        End If

        If FitDataPoints(0).Length <> FitDataPoints(1).Length Then
            Throw New ArgumentException("Data must have the same number of x and y points.")
        End If

        Me.FitFunction = ModelFitFunction
        Me.DataPoints = FitDataPoints
        Me.Weights = CheckWeights(Me.DataPoints(0).Length, Weights)

        ' Decide if to start async or directly
        If StartAsync Then
            ' Start Thread
            Me.FitThreadWorker.RunWorkerAsync()
        Else
            Me._Fit()
        End If

        Return True
    End Function

    ''' <summary>
    ''' Aborts the Fit-Procedure, if the Fit is running in a separate Thread.
    ''' Otherwise, if the fit is a direct fit, we can't abort it anyway.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AbortAsyncFit() Implements iFitProcedure.AbortAsyncFit
        If Me.FitThreadWorker.IsBusy Then
            Me.FitThreadWorker.CancelAsync()
        End If
    End Sub

    ''' <summary>
    ''' Tells the state of the FitThreadWorker.
    ''' </summary>
    Public Function FitIsRunning() As Boolean
        Return Me.FitThreadWorker.IsBusy
    End Function
#End Region

#Region "Stop Condition Control Function"
    ''' <summary>
    ''' The stop condition for the fit.
    ''' Override this if you want to use another stop condition.
    ''' </summary>
    Protected Overridable Function CheckStopConditions() As FitStopReason

        ' Check for convergence of the Fit.
        If System.Math.Abs(_Chi2 - IncrementedChi2) < Me.ThisFitProcedureSettings.MinDeltaChi2 Then
            Return FitStopReason.FitConverged
        End If

        ' Check for reaching the max iteration count.
        If IterationCount > Me.ThisFitProcedureSettings.MaxIteration Then
            Return FitStopReason.MaxIterationsReached
        End If

        ' Check, if a Cancellation of the Worker is pending,
        ' if we are using a worker.
        If Me.FitThreadWorker.CancellationPending Then
            Return FitStopReason.UserAborted
        End If

        Return FitStopReason.None
    End Function
#End Region

#Region "Fit Procedure"

    ''' <summary>
    ''' Update methods for the scaling matrix,
    ''' which gives the contribution of the damping parameter
    ''' lambda to a certain element of the hessian matrix.
    ''' </summary>
    Public Enum ScalingMatrixUpdateMethods
        IdentityMatrix
        DynamicallyUpdatedBasedOnJacobian
    End Enum

    Protected NumberOfFunctionCalls As Integer = 0
    Protected NumberOfJacobianCalls As Integer = 0
    Protected ParameterRejectedCount As Integer = 0
    Protected ParameterAcceptedCount As Integer = 0
    Protected BroydenUpdatesToPerform As Integer = 0
    Protected AccelerationUpdates As Integer = 0
    Protected BestFunctionEvaluationCost As Double = 0
    Protected PredictedReduction As Double
    Protected DirectionalDerivative As Double
    Protected CosAlpha As Double
    Protected Cost As Double = 0
    Protected CostBest As Double = 0
    Protected CostNew As Double = 0
    Protected JacobianUpToDate As Boolean = False
    Protected JacobianForceUpdate As Boolean = False
    Protected vParameterVector As DenseVector
    Protected vFunctionValues As DenseVector
    Protected vFunctionValuesBest As DenseVector
    Protected vFunctionValuesNew As DenseVector
    Protected vA As DenseVector
    Protected vV As DenseVector
    Protected vVOld As DenseVector
    Protected vAcceleration As DenseVector
    Protected mJacobianMatrix As DenseMatrix
    Protected mJacTJac As Matrix
    Protected mScalingMatrix As DenseMatrix
    Protected av As Double
    Protected avMax As Double

    Public Property UseCenterDifferentiationForJacobian As Boolean = False '#
    Public Property ScalingMatrixUpdateMethod As ScalingMatrixUpdateMethods = ScalingMatrixUpdateMethods.IdentityMatrix '#
    Public Property InitialFactor As Double = 1 '#


    ''' <summary>
    ''' Actual Fit-Function. If used after calling fit(lambda, minDeltaChi2, maxIterations),
    ''' uses those values. The stop condition is fetched from <code>Me.StopCondition</code>.
    ''' Override <code>Me.StopCondition</code> if you want to use another stop condition.
    ''' </summary>
    Protected Sub _Fit() Handles FitThreadWorker.DoWork

        Dim temp1, temp2 As Double

        ' Copy the fit-parameters
        Me._FitParameters = Me.FitFunction.FitParameters

        ' Set the Differentiation-Step used for calculation of the nummeric partial derivatives
        Me.PartialDerivativeDelta = Me.ThisFitProcedureSettings.DifferentiationStep

        ' Copy parameters to incremented parameter array.
        Me.IncrementedParameters = New Dictionary(Of Integer, sFitParameter)(FitParameters)
        Me.IncrementedParametersBest = New Dictionary(Of Integer, sFitParameter)(FitParameters)

        ' Count and extract all non-fixed Fit-Parameter-Keys
        Me.NonFixedFitParameterKeys = sFitParameter.GetNonFixedFitParameterKeys(FitParameters)
        Me.NonFixedFitParameterCount = Me.NonFixedFitParameterKeys.Count

        ' Set length of the Vectors
        Me.beta = New Double(NonFixedFitParameterCount - 1) {}
        Me.da = New Double(NonFixedFitParameterCount - 1) {}

        ' Set the given Hessian Matrix
        Me.alpha = New MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(NonFixedFitParameterCount, NonFixedFitParameterCount)

        ' Initialize the Jacobian Matrix
        Me.mJacobianMatrix = New DenseMatrix(NonFixedFitParameterCount, Me.DataPoints(0).Length)

        ' Set the initial damping parameter
        lambda = LambdaConst

        '##################################
        ' Reset initial parameters

        ' Set Integers and Double
        Me.IterationCount = 0
        Me.NumberOfFunctionCalls = 0
        Me.NumberOfJacobianCalls = 0
        Me.ParameterRejectedCount = 0
        Me.ParameterAcceptedCount = 0
        Me.CosAlpha = 1
        Me.av = 0
        Me.avMax = 0

        ' Set Vectors
        Me.vA = New MathNet.Numerics.LinearAlgebra.Double.DenseVector(Me.NonFixedFitParameterCount, 0)
        Me.vV = New MathNet.Numerics.LinearAlgebra.Double.DenseVector(Me.NonFixedFitParameterCount, 0)
        Me.vVOld = New MathNet.Numerics.LinearAlgebra.Double.DenseVector(Me.NonFixedFitParameterCount, 0)

        Me.vFunctionValues = New MathNet.Numerics.LinearAlgebra.Double.DenseVector(Me.DataPoints(0).Length)
        Me.vFunctionValuesBest = New MathNet.Numerics.LinearAlgebra.Double.DenseVector(Me.DataPoints(0).Length)
        Me.vFunctionValuesNew = New MathNet.Numerics.LinearAlgebra.Double.DenseVector(Me.DataPoints(0).Length)

        ' Set the start time
        Me._FitStartTime = Now

        ' Set the Stop-Reason
        Me.StopReason = FitStopReason.None

        ' Variable to tell if we have a valid result
        Dim bValidResult As Boolean = True

        ' END Initial parameters setting
        '##################################

        '###############################
        ' Pre-Interative processes

        ' Get initial function values to calculate the initial cost.
        Me.GetFunctionValues(_FitParameters, vFunctionValues)

        ' Calculate the inital cost of the function evaluation
        Me.Cost = 0.5 * vFunctionValues.DotProduct(vFunctionValues)
        Debug.WriteLine("Inital function evaluation cost: " & Cost)

        ' Check for NaNs in vFunctionValues
        If vFunctionValues.Contains(Double.NaN) Then
            bValidResult = False
            Me.StopReason = FitStopReason.FunctionEvaluationContainedNaN
        End If

        ' Save the calculated cost as best function evaluation cost
        Me.BestFunctionEvaluationCost = Cost

        ' Save the best function values found:
        vFunctionValuesBest = vFunctionValues

        ' Initialize the Jacobian Matrix
        Me.mJacobianMatrix = New DenseMatrix(NonFixedFitParameterCount, Me.DataPoints(0).Length)

        ' Use finite-differences jacobian
        If Not Me.CalculateJacobianMatrix() Then
            bValidResult = False
            Me.StopReason = FitStopReason.JacobianContainedNaN
        End If
        Me.JacobianUpToDate = True
        Me.JacobianForceUpdate = False

        ' Calculate the Transpose(Jacobian) * Jacobian
        mJacTJac = CType(mJacobianMatrix.TransposeThisAndMultiply(mJacobianMatrix), DenseMatrix)

        ' Initialize the scaling matrix
        Debug.WriteLine("Initializing scaling matrix ...")
        If Me.ScalingMatrixUpdateMethod = ScalingMatrixUpdateMethods.IdentityMatrix Then
            ' identity matrix
            Me.mScalingMatrix = DenseMatrix.Identity(Me.NonFixedFitParameterCount)
        ElseIf Me.ScalingMatrixUpdateMethod = ScalingMatrixUpdateMethods.DynamicallyUpdatedBasedOnJacobian Then
            ' dynamic update depending on jacobian
            Me.mScalingMatrix = New DenseMatrix(Me.NonFixedFitParameterCount)
            Me.mScalingMatrix.Clear()
            For i As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                Me.mScalingMatrix(i, i) = Math.Max(Me.mJacTJac(i, i), Me.mScalingMatrix(i, i))
            Next
        End If

        ' Initialize Lambda
        Debug.WriteLine("Initializing the damping parameter Lambda ...")
        ' interpret the update method as integer
        ' Those < 10 (direct adjustment) are initialized in a different manner,
        ' than those > 10, where the step-size adjustment is used.
        If Me.LambdaUpdateMethod < 10 Then
            ' direct adjustments
            '####################
            Debug.WriteLine("--> direct Lambda adjustment")
            Me.lambda = Me.mJacTJac(0, 0)
            For i As Integer = 1 To Me.NonFixedFitParameterCount - 1 Step 1
                Me.lambda = Math.Max(Me.mJacTJac(i, i), Me.lambda)
            Next
            Me.lambda *= Me.InitialFactor
        Else
            ' Initial step bound, if using trusted region method
            '####################################################
            Debug.WriteLine("--> step bound lambda adjustment")
            Me.delta = Me.InitialFactor * Math.Sqrt(Me.vParameterVector.DotProduct(Me.mScalingMatrix.LeftMultiply(Me.vParameterVector))) ' CHECK, IF ERROR!
            Me.lambda = 1.0
            If Me.delta < Me.ThisFitProcedureSettings.MinDeltaChi2 Then Me.delta = 100

            ' Only call the TrustRegion Method, if there were NO NaNs in either fvec or fjac.
            If Me.StopReason = FitStopReason.None Then
                Me.TrustRegion()
            End If
        End If

        ' END Pre-Interative processes
        '###############################

        '#################
        ' ITERATION LOOP
        Debug.WriteLine("Starting the Iteration-Loop ...")
        Debug.Indent()
        While Me.StopReason = FitStopReason.None

            '#############################
            ' Jacobian update:
            ' check, if full or only partial update is necessary
            If (Me.ParameterAcceptedCount > 0 And Me.BroydenUpdatesToPerform <= 0) Then Me.JacobianForceUpdate = True
            If (Me.ParameterAcceptedCount + Me.BroydenUpdatesToPerform) <= 0 And Not Me.JacobianUpToDate Then Me.JacobianForceUpdate = True

            ' Force Jacobian Update after to too many failed attempts
            If Me.ParameterAcceptedCount > 0 And Me.BroydenUpdatesToPerform > 0 And Not Me.JacobianForceUpdate Then
                ' Rank deficient update of jacobian matrix
                Me.RankDeficientJacobianUpdate()
                Me.JacobianUpToDate = False
            End If

            ' If the step got accepted,
            ' take the new values.
            If Me.ParameterAcceptedCount > 0 Then
                Debug.WriteLine("New parameters accepted ...")
                vFunctionValues = vFunctionValuesNew
                _FitParameters = IncrementedParameters
                vVOld = vV
                Cost = CostNew
                ' If the cost is lower, save those values
                If Cost <= CostBest Then
                    IncrementedParametersBest = _FitParameters
                    CostBest = Cost
                    vFunctionValuesBest = vFunctionValues
                End If
            End If

            ' Full rank update of the Jacobian
            If Me.JacobianForceUpdate Then
                If Not Me.CalculateJacobianMatrix() Then
                    bValidResult = False
                End If
                Me.JacobianForceUpdate = False
                Me.JacobianUpToDate = True
            End If

            ' If no NaNs in the Jacobian
            If bValidResult Then

                ' Update JtJ
                Me.mJacTJac = CType(mJacobianMatrix.TransposeThisAndMultiply(mJacobianMatrix), DenseMatrix)

                ' Update the Scaling, Lambda, if step count > 1
                If IterationCount > 1 Then

                    ' Update Scaling
                    Debug.WriteLine("Updating the Scaling-Matrix, if necessary ...")
                    If Me.ScalingMatrixUpdateMethod = ScalingMatrixUpdateMethods.DynamicallyUpdatedBasedOnJacobian Then
                        ' dynamic update depending on jacobian
                        For i As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                            Me.mScalingMatrix(i, i) = Math.Max(Me.mJacTJac(i, i), Me.mScalingMatrix(i, i))
                        Next
                    End If

                    ' Update lambda
                    Debug.WriteLine("Updating lambda ...")
                    Me.UpdateLambda()
                End If

                ' propose a new step
                ' Encapsule the Increment-Solving-Section
                ' in a Try-Catch, to catch errors from singular matrices!
                Try
                    Debug.WriteLine("Solving the linear equation system da = Beta Alpha^-1 ...")
                    Me.alpha = CType(Me.mJacTJac + Me.lambda * Me.mScalingMatrix, DenseMatrix)
                    Dim CholeskyDecomposition As New MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseCholesky(alpha)
                    Me.vV = -1 * Me.vFunctionValues * Me.mJacobianMatrix
                    Me.vV = CType(CholeskyDecomposition.Factor.Cholesky.Solve(Me.vV), DenseVector)
                Catch ex As Exception
                    ' Singular Matrix recognized
                    Me.StopReason = FitStopReason.MatrixSingular
                    Exit While
                End Try

                ' calculate the predicted reduction and the directional derivative
                ' Useful for Methods to update lambda!
                temp1 = 0.5 * Me.vV * (mJacTJac * vV) / Cost
                temp2 = 0.5 * lambda * Me.vV * (mScalingMatrix * vV) / Cost
                Me.PredictedReduction = 2 * (temp1 + temp2)
                Me.DirectionalDerivative = -1 * (temp1 + temp2)

                ' calculate CosAlpha (cos of angle between step direction (in data space) and residual vector
                Me.CosAlpha = Math.Abs(Me.vFunctionValues * (Me.mJacobianMatrix * Me.vV))
                Me.CosAlpha /= Math.Sqrt((Me.vFunctionValues * Me.vFunctionValues) * ((Me.mJacobianMatrix * Me.vV) * (Me.mJacobianMatrix * Me.vV)))

                ' If the update method is < 10, then update also the step size delta
                If Me.LambdaUpdateMethod < 10 Then Me.delta = Math.Sqrt(Me.vV * (Me.mScalingMatrix * Me.vV))

                ' Update the acceleration
                If Me.AccelerationUpdates > 0 Then

                    ' Call the update of the acceleration term.
                    ' Get as return value, if NaNs are in the
                    ' acceleration vector.
                    If True Then '''' TODO: FDAvv Function here!
                        ' If no NaNs, write the acceleration Vector
                        ' Encapsule the Increment-Solving-Section
                        ' in a Try-Catch, to catch errors from singular matrices!
                        Try
                            Debug.WriteLine("Solving the linear equation system da = Beta Alpha^-1 ...")
                            Me.alpha = CType(Me.mJacTJac + Me.lambda * Me.mScalingMatrix, DenseMatrix)
                            Dim CholeskyDecomposition As New MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseCholesky(alpha)
                            Me.vA = -1 * Me.vAcceleration * Me.mJacobianMatrix
                            Me.vA = CType(CholeskyDecomposition.Factor.Cholesky.Solve(Me.vA), DenseVector)
                        Catch ex As Exception
                            ' Singular Matrix recognized
                            Me.StopReason = FitStopReason.MatrixSingular
                            Exit While
                        End Try
                    Else
                        ' If NaNs, ignore the acceleration term
                        Me.vA.Clear()
                    End If
                End If

                ' Evaluate at proposed step
                ' --> only necessary if av <= avmax
                av = Math.Sqrt(Me.vA * (Me.mScalingMatrix * Me.vA) / (Me.vV * (Me.mScalingMatrix * Me.vV)))
                If av <= avMax Then
                    ' Set the new vector of function values
                    Me.vFunctionValuesNew = Me.vFunctionValues + Me.vV + 0.5 * Me.vA





                End If

            End If





            ' Increase Iteration Counter
            IterationCount += 1

            ' Check for Fit-Stop-Condition
            Me.StopReason = Me.CheckStopConditions
        End While
        ' Loop ended, so fit stopped!!!!
        '#################################

        '#############################
        ' Post-Processing final data

        ' Create the data using the fit-model as third column in the datapoint-array
        Dim CalculatedDataPoints(DataPoints(0).Length - 1) As Double
        For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
            CalculatedDataPoints(i) = Me.FitFunction.GetY(DataPoints(0)(i), Me.FitParameters)
        Next

        ' Calculate the final numeric statistics
        Me._FinalStatistics = cNumericalMethods.Statistics1D(CalculatedDataPoints, DataPoints(1))

        ' Save the time needed for the fit!
        Me._FitEndTime = Now

        Debug.Unindent()
        Debug.WriteLine("Fit-Process ended after " & (Me._FitEndTime - Me._FitStartTime).TotalMinutes.ToString("N0") & " minutes!")

        ' Raise the event to the listeners!
        RaiseEvent FitFinished(StopReason, _FitParameters, _Chi2)
    End Sub

    ''' <summary>
    ''' This function returns a vector of the relevant (non-fixed) 
    ''' fit-parameters, to be used in the fit-procedure with
    ''' matrix calculations.
    ''' </summary>
    Protected Function GetFitParameterVectorFromParameterList(ByRef InputParameters As Dictionary(Of Integer, sFitParameter)) As DenseVector
        Dim ReturnVector As New DenseVector(Me.NonFixedFitParameterCount)
        For NonFixedKeyCounter As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            ReturnVector(NonFixedKeyCounter) = InputParameters(Me.NonFixedFitParameterKeys(NonFixedKeyCounter)).Value
        Next
        Return ReturnVector
    End Function

    ''' <summary>
    ''' This function changes the FitParameter-Values in the given Dictionary
    ''' by the values in the given vector.
    ''' </summary>
    Protected Sub GetFitParameterDictionaryFromParameterVector(ByRef ParameterDictionaryToChange As Dictionary(Of Integer, sFitParameter),
                                                               ByRef InputParameterVector As DenseVector)
        For NonFixedKeyCounter As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            ParameterDictionaryToChange(Me.NonFixedFitParameterKeys(NonFixedKeyCounter)) =
                ParameterDictionaryToChange(Me.NonFixedFitParameterKeys(NonFixedKeyCounter)).ChangeValue(InputParameterVector(NonFixedKeyCounter))
        Next
    End Sub

    ''' <summary>
    ''' Updates parameters from incrementedParameters,
    ''' and fire the Update Event.
    ''' </summary>
    Protected Sub UpdateParameters()

        ' Copy Array
        'System.Array.Copy(IncrementedParameters, 0, _FitParameters, 0, _FitParameters.Length)
        Me._FitParameters = Me.IncrementedParameters

        ' Update-Event
        RaiseEvent FitStepEcho(_FitParameters, Chi2)
    End Sub

    ''' <summary>
    ''' Calculates value of the function for given parameter array
    ''' </summary>
    ''' <param name="InputParameters">input parameters</param>
    ''' <returns>value of the function</returns>
    Protected Function CalculateChi2(ByRef InputParameters As Dictionary(Of Integer, sFitParameter)) As Double

        ' Reduce the FitEcho to integer percentage values
        Dim FitEchoValue As Integer = CInt(DataPoints(0).Length / 50)

        Dim result As Double = 0
        Dim dy As Double
        For i As Integer = 0 To DataPoints(0).Length - 1
            ' Progress
            If Not FitThreadWorker Is Nothing And i Mod FitEchoValue = 0 Then
                RaiseEvent CalculationStepProgress(i, DataPoints(0).Length, "Calculating Chi2 ... ")
            End If

            ' Calculation
            dy = DataPoints(1)(i) - FitFunction.GetY(DataPoints(0)(i), InputParameters)
            result += Weights(i) * dy * dy
        Next
        Return result
    End Function

    ''' <summary>
    ''' Calculates values of the function for given parameter array,
    ''' and returns the values as vector.
    ''' Output vector MUST HAVE THE CORRECT SIZE!
    ''' </summary>
    ''' <param name="InputParameters">input parameters</param>
    Protected Sub GetFunctionValues(ByRef InputParameters As Dictionary(Of Integer, sFitParameter),
                                    ByRef OutputFunctionValueVector As MathNet.Numerics.LinearAlgebra.Double.DenseVector)

        ' Increment the number of function calls
        Me.NumberOfFunctionCalls += 1

        ' Reduce the FitEcho to integer percentage values
        Dim FitEchoValue As Integer = CInt(DataPoints(0).Length / 50)

        For i As Integer = 0 To DataPoints(0).Length - 1
            ' Progress
            If Not FitThreadWorker Is Nothing And i Mod FitEchoValue = 0 Then
                RaiseEvent CalculationStepProgress(i, DataPoints(0).Length, "Calculating function values ... ")
            End If

            ' Calculation
            OutputFunctionValueVector(i) = FitFunction.GetY(DataPoints(0)(i), InputParameters)
        Next
    End Sub

    ''' <summary>
    ''' Calculates function value for the current fit parameters
    ''' Does not change the value of chi2
    ''' </summary>
    ''' <returns>value of the function</returns>
    Protected Function CalculateChi2() As Double
        Return CalculateChi2(_FitParameters)
    End Function

    ''' <summary>
    ''' Calculates function value for the incremented parameters (da + a).
    ''' Does not change the value of chi2.
    ''' </summary>
    Protected Function CalculateIncrementedChi2() As Double
        Return CalculateChi2(IncrementedParameters)
    End Function

    ' Partial Derivative Calculation in the caching of the Elements
    Protected PartialDerivativeCache_LastParameterArray As New Dictionary(Of Integer, sFitParameter)
    Protected PartialDerivativeDelta As Double = 0.00001 ' 0.000000001

    ''' <summary>
    ''' Create Jacobian Matrix of all partial derivatives of the Fit-Function.
    ''' <code>UseCenterDifferentiationForJacobian</code> decides, whether center
    ''' differences will be used. They are more accurate, but require more function evaluations!
    ''' </summary>
    ''' <returns>Boolean, True, if successfull, false, if Jacobian contains NaN</returns>
    Protected Function CalculateJacobianMatrix() As Boolean

        Dim ReturnResult As Boolean = True

        ' Increment the number of jacobian evaluations
        Me.NumberOfJacobianCalls += 1

        ' Save the last used fit-parameter set
        PartialDerivativeCache_LastParameterArray.Clear()
        For Each FitParameterKV As KeyValuePair(Of Integer, sFitParameter) In Me._FitParameters
            PartialDerivativeCache_LastParameterArray.Add(FitParameterKV.Key, FitParameterKV.Value)
        Next

        ' Decide weather to use centered evaluation, or
        ' forward numeric derivation.
        If Me.UseCenterDifferentiationForJacobian Then
            '
            '    Centered Differences
            ' (more accurate than forward)
            '
            '##############################

            ' Generate the buffer arrays
            Dim PartialDerivativeCache_PosElements(DataPoints(0).Length - 1) As Double
            Dim PartialDerivativeCache_NegElements(DataPoints(0).Length - 1) As Double

            ' Create variable shortcut the currently used parameter index
            Dim CurrentParameterIndex As Integer
            For NonFixedKeyCounter As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1

                ' Register currently used parameter index
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(NonFixedKeyCounter)

                ' Save the old Parameter
                Dim TempParameter As sFitParameter = PartialDerivativeCache_LastParameterArray(CurrentParameterIndex)

                ' Reduce StepProgress-Event-spamming
                Dim StepProgressReportMod As Integer = CInt(DataPoints(0).Length * 3 / 50)

                ' Calculate the positive elements
                PartialDerivativeCache_LastParameterArray(CurrentParameterIndex) = TempParameter.ChangeValue(TempParameter.Value + PartialDerivativeDelta)
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    PartialDerivativeCache_PosElements(i) = FitFunction.GetY(DataPoints(0)(i), PartialDerivativeCache_LastParameterArray)
                    ' Progress
                    If Not FitThreadWorker Is Nothing And i Mod StepProgressReportMod = 0 Then RaiseEvent CalculationStepProgress(i, DataPoints(0).Length * 3, "Calc Jacobian (accurate centered) - (row: " & NonFixedKeyCounter & ", column: " & i & ") ... ")
                Next

                ' Calculate the negative elements
                PartialDerivativeCache_LastParameterArray(CurrentParameterIndex) = TempParameter.ChangeValue(TempParameter.Value - PartialDerivativeDelta)
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    PartialDerivativeCache_NegElements(i) = FitFunction.GetY(DataPoints(0)(i), PartialDerivativeCache_LastParameterArray)
                    ' Progress
                    If Not FitThreadWorker Is Nothing And i Mod StepProgressReportMod = 0 Then RaiseEvent CalculationStepProgress(DataPoints(0).Length + i, DataPoints(0).Length * 3, "Calc Jacobian (accurate centered) (row: " & NonFixedKeyCounter & ", column: " & i & ") ... ")
                Next

                ' Reset modified parameter in the parameter array
                PartialDerivativeCache_LastParameterArray(CurrentParameterIndex) = TempParameter

                ' Calculate the final partial derivatives and write them to the cache:
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    Me.mJacobianMatrix(NonFixedKeyCounter, i) = (PartialDerivativeCache_PosElements(i) - PartialDerivativeCache_NegElements(i)) / (2 * PartialDerivativeDelta)
                    If Double.IsNaN(Me.mJacobianMatrix(NonFixedKeyCounter, i)) Then
                        ReturnResult = False
                    End If
                    ' Progress
                    If Not FitThreadWorker Is Nothing And i Mod StepProgressReportMod = 0 Then RaiseEvent CalculationStepProgress(2 * DataPoints(0).Length + i, DataPoints(0).Length * 3, "Calc Jacobian (accurate centered) (row: " & NonFixedKeyCounter & ", column: " & i & ") ... ")
                Next
            Next
        Else
            '
            '         Forward Differences
            ' (needs half the function evaluations)
            '
            '#######################################

            ' Create variable shortcut the currently used parameter index
            Dim CurrentParameterIndex As Integer
            For NonFixedKeyCounter As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1

                ' Register currently used parameter index
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(NonFixedKeyCounter)

                ' Save the old Parameter
                Dim TempParameter As sFitParameter = PartialDerivativeCache_LastParameterArray(CurrentParameterIndex)

                ' Reduce StepProgress-Event-spamming
                Dim StepProgressReportMod As Integer = CInt(DataPoints(0).Length * 3 / 50)

                ' Calculate the positive elements
                PartialDerivativeCache_LastParameterArray(CurrentParameterIndex) = TempParameter.ChangeValue(TempParameter.Value + PartialDerivativeDelta)
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    Me.mJacobianMatrix(NonFixedKeyCounter, i) = (FitFunction.GetY(DataPoints(0)(i), PartialDerivativeCache_LastParameterArray) _
                                                                - Me.vFunctionValues(i)) / PartialDerivativeDelta
                    If Double.IsNaN(Me.mJacobianMatrix(NonFixedKeyCounter, i)) Then
                        ReturnResult = False
                    End If
                    ' Progress
                    If Not FitThreadWorker Is Nothing And i Mod StepProgressReportMod = 0 Then RaiseEvent CalculationStepProgress(i, DataPoints(0).Length, "Calc Jacobian (fast forward) (row: " & NonFixedKeyCounter & ", column: " & i & ") ... ")
                Next

                ' Reset modified parameter in the parameter array
                PartialDerivativeCache_LastParameterArray(CurrentParameterIndex) = TempParameter
            Next
        End If

        Return ReturnResult
    End Function

#Region "Initial Check of the Weights"
    ''' <summary>
    ''' Checks if the matrix of weights for each point is a 
    ''' matrix of positive elements. Otherwise it initializes
    ''' a new matrix and sets each value to 1
    ''' </summary>
    Protected Function CheckWeights(ByVal Length As Integer,
                                    ByRef Weights As Double()) As Double()
        Dim DamagedWeights As Boolean = False
        ' check for null
        If Weights Is Nothing Then
            'Trace.WriteLine("Weights were not defined.")
            DamagedWeights = True
            Weights = New Double(Length - 1) {}
        Else
            ' check if all elements are zeros or if there are negative, NaN or Infinite elements
            Dim AllZero As Boolean = True
            Dim IllegalElement As Boolean = False
            Dim i As Integer = 0
            While i < Weights.Length AndAlso Not IllegalElement
                If Weights(i) < 0 OrElse [Double].IsNaN(Weights(i)) OrElse [Double].IsInfinity(Weights(i)) Then
                    IllegalElement = True
                End If
                AllZero = (Weights(i) = 0) AndAlso AllZero
                i += 1
            End While
            DamagedWeights = AllZero OrElse IllegalElement
        End If

        If DamagedWeights Then
            Trace.WriteLine("Weights were not well defined. All elements set to 1.")
            For i As Integer = 0 To Weights.Length - 1
                Weights(i) = 1
            Next
        End If

        Return Weights
    End Function
#End Region
#End Region



    '###########################################

#Region "Lambda Updates"
    ''' <summary>
    ''' Factor with which lambda is multiplied, if the
    ''' new parameters got accepted. > 1
    ''' </summary>
    Public Property LambdaFactorAccept As Double = 3

    ''' <summary>
    ''' Factor with which lambda is divided, if the
    ''' new parameters got rejected. > 1
    ''' </summary>
    Public Property LambdaFactorReject As Double = 3

    ''' <summary>
    ''' Delta-Parameter, for indirect lambda update.
    ''' </summary>
    Private LambdaUpdateDelta As Double

    ''' <summary>
    ''' Lambda-Update-Methods
    ''' Direct methods directly update lambda,
    ''' whereas indirect methods perform the update indirectly
    ''' via the delta-parameter <code>LambdaUpdateDelta</code>.
    ''' </summary>
    Public Enum LambdaUpdateMethods
        DirectFixedFactor = 0
        DirectGainNelson = 1
        DirectUmrigarNightingale = 2
        IndirectFixedFactor = 10
        IndirectMore = 11
    End Enum

    ''' <summary>
    ''' Selector for the 
    ''' Lambda-Update-Methods
    ''' </summary>
    Private LambdaUpdateMethod As LambdaUpdateMethods

    ''' <summary>
    ''' Updates the damping parameter, depending on the selected update method.
    ''' <paramref name="ParameterRejectedCount">
    ''' Count of the rejections of the parameter set.
    ''' If = 0 then the parameters got accepted!
    ''' </paramref>
    ''' </summary>
    Public Sub UpdateLambda()

        Select Case Me.LambdaUpdateMethod
            Case LambdaUpdateMethods.DirectFixedFactor
                ' Update Lambda directly by fixed factors
                Me.LambdaUpdateFixedFactor()

            Case LambdaUpdateMethods.DirectGainNelson
                ' Update Lambda directly based on Gain-Factor Rho
                Me.LambdaUpdateNelson()

            Case LambdaUpdateMethods.DirectUmrigarNightingale
                ' Update Lambda directly using the method of Umrigar and Nightingdale (unpublished)

            Case LambdaUpdateMethods.IndirectFixedFactor
                ' TRUST REGION: Update delta by fixed factors

            Case LambdaUpdateMethods.IndirectMore
                ' TRUST REGION: Update delta as described in More'



        End Select

    End Sub

#Region "Direct Fixed Factor"

    ''' <summary>
    ''' Update lambda based on accepted / rejected step
    ''' </summary>
    Private Sub LambdaUpdateFixedFactor()
        If ParameterRejectedCount = 0 Then
            Me.lambda /= Me.LambdaFactorAccept
        Else
            Me.lambda *= Me.LambdaFactorReject
        End If
    End Sub

#End Region

#Region "Direct Gain Nelson"

    ''' <summary>
    ''' Factor for Nelson Lambda-Update
    ''' </summary>
    Public Property Rho As Double

    ''' <summary>
    ''' Update lambda method due to Nelson
    ''' </summary>
    Private Sub LambdaUpdateNelson()
        If ParameterRejectedCount = 0 Then
            Me.lambda *= Math.Max(1.0F / Me.LambdaFactorAccept, 1.0F - Math.Pow((2.0F * (Me.Rho - 0.5F)), 3))
        Else
            Dim nu As Double = Me.LambdaFactorReject
            For i As Integer = 0 To ParameterRejectedCount Step 1
                nu *= 2.0F
            Next
            Me.lambda *= nu
        End If
    End Sub

#End Region

#Region "Trusted Region Method - TODO!!!"

    ''' <summary>
    ''' Use the step size Delta to calculate the new damping parameter.
    ''' Called: Trusted Region Method.
    ''' </summary>
    Protected Sub TrustRegion()

        ' TODO!!!!!!

    End Sub


#End Region

#End Region

#Region "Acceptance of a step"

    Public Sub StepAcceptance()

    End Sub

#End Region

#Region "Rank-Deficient Jacobian Update"

    ''' <summary>
    ''' Rank-deficient Jacobian update routine.
    ''' </summary>
    Public Sub RankDeficientJacobianUpdate()

        Dim r1 As DenseVector = CType(Me.vFunctionValues + 0.5 * Me.mJacobianMatrix.LeftMultiply(Me.vV) + 0.125 * Me.vAcceleration, DenseVector)
        Dim dJac As DenseVector = CType(2.0 * (r1 - Me.vFunctionValues - 0.5 * Me.mJacobianMatrix.LeftMultiply(Me.vV)) / Me.vV.DotProduct(Me.vV), DenseVector)
        For i As Integer = 0 To Me.DataPoints(0).Length - 1 Step 1
            For j As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                Me.mJacobianMatrix(i, j) += dJac(i) * 0.5 * Me.vV(j)
            Next
        Next
        Dim v2 As DenseVector = 0.5 * (Me.vV + Me.vA)
        For i As Integer = 0 To Me.DataPoints(0).Length - 1 Step 1
            For j As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                Me.mJacobianMatrix(i, j) += dJac(i) * v2(j)
            Next
        Next

    End Sub

#End Region

End Class
