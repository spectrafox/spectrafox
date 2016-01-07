Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports System.ComponentModel
Imports MathNet.Numerics.LinearAlgebra.Double

''' <summary>
''' General Fit-Procedure class
''' with some shared functions.
''' </summary>
Public MustInherit Class cFitProcedure
    Implements iFitProcedure

#Region "Fit-Procedure-Settings"
    ''' <summary>
    ''' Property to save the procedure-specific fit-settings.
    ''' </summary>
    Public MustOverride Property FitProcedureSettings As iFitProcedureSettings Implements iFitProcedure.FitProcedureSettings

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Public MustOverride ReadOnly Property ProcedureSettingPanel As cFitProcedureSettingsPanel Implements iFitProcedure.ProcedureSettingPanel
#End Region

#Region "Parameters"
    ''' <summary>
    ''' interface of the function to be fitted
    ''' </summary>
    Protected FitFunction As iFitFunction

    ''' <summary>
    ''' The array of fit parameters.
    ''' </summary>
    Protected _FitParameters As Dictionary(Of Integer, sFitParameter)

    ''' <summary>
    ''' Count of the non-fixed FitParameters.
    ''' </summary>
    Protected NonFixedFitParameterCount As Integer

    ''' <summary>
    ''' Indizes in the arrays of all non-fixed FitParameters.
    ''' </summary>
    Protected NonFixedFitParameterIndices As Integer()

    ''' <summary>
    ''' Measured data points for which the model function is to be fitted.
    ''' double[0 = x, 1 = y][data point index] = data value
    ''' </summary>
    Protected DataPoints As Double()()

    ''' <summary>
    ''' Weights for each data point. The merit function is: chi2 = sum[ (y_i - y(x_i;a))^2 * w_i ].
    ''' For gaussian errors in datapoints, set w_i = 1 / sigma_i.
    ''' </summary>
    Protected Weights As Double()

    ''' <summary>
    ''' temporary calculated Chi2 value
    ''' </summary>
    Protected _Chi2 As Double

    ''' <summary>
    ''' Counts the iteration.
    ''' </summary>
    Protected IterationCount As Integer

    ' Variable for saving the Fit-Duration
    Protected _FitStartTime As Date
    Protected _FitEndTime As Date

#End Region

#Region "Property accessors"

    ''' <summary>
    ''' Fit-Procedure-Name
    ''' </summary>
    Public MustOverride ReadOnly Property Name As String Implements iFitProcedure.Name

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

    Protected _FinalStatistics As cNumericalMethods.sNumericStatistics
    ''' <summary>
    ''' Statistics, that get calculated after the fit has finished
    ''' </summary>
    Public ReadOnly Property FinalStatistics As cNumericalMethods.sNumericStatistics Implements iFitProcedure.Statistics
        Get
            Return Me._FinalStatistics
        End Get
    End Property

#End Region

#Region "Constructor with the inital settings of the backgroundworker, etc."
    ''' <summary>
    ''' Constructor of the FitProcedure
    ''' </summary>
    Public Sub New()
        ' Set Properties of the BackgroundWorker Thread
        Me.FitThreadWorker.WorkerSupportsCancellation = True
        Me.FitThreadWorker.WorkerReportsProgress = True
    End Sub
#End Region

#Region "Fit-Thread-Worker"
    ''' <summary>
    ''' Single Thread used for Async fitting.
    ''' </summary>
    Protected WithEvents FitThreadWorker As New BackgroundWorker

    ''' <summary>
    ''' Single Thread used for Async fitting.
    ''' </summary>
    Public ReadOnly Property FitWorker As BackgroundWorker Implements iFitProcedure.FitThreadWorker
        Get
            Return Me.FitThreadWorker
        End Get
    End Property
#End Region

#Region "Interface-Events"

    ''' <summary>
    ''' Echos the current parameter set, whenever
    ''' the parameters got changed to a minimized Chi2. 
    ''' </summary>
    Public Event FitStepEcho(ByVal Parameters As Dictionary(Of Integer, sFitParameter),
                             ByVal Chi2 As Double) Implements iFitProcedure.FitStepEcho

    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_FitStepEcho(ByVal Parameters As Dictionary(Of Integer, sFitParameter),
                                         ByVal Chi2 As Double)
        RaiseEvent FitStepEcho(Parameters, Chi2)
    End Sub

    ''' <summary>
    ''' Echos the current parameter set, whenever
    ''' the parameters got changed to a minimized Chi2. 
    ''' </summary>
    Public Event FitEcho(ByVal Message As String) Implements iFitProcedure.FitEcho

    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_FitEcho(ByVal Message As String)
        RaiseEvent FitEcho(Message)
    End Sub

    ''' <summary>
    ''' Echoed, if the fit procedure has ended.
    ''' </summary>
    Public Event FitFinished(ByVal FitStopReason As Integer,
                             ByVal FinalParameters As Dictionary(Of Integer, sFitParameter),
                             ByVal Chi2 As Double) Implements iFitProcedure.FitFinished

    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_FitFinished(ByVal FitStopReason As Integer,
                                                ByVal FinalParameters As Dictionary(Of Integer, sFitParameter),
                                                ByVal Chi2 As Double)
        RaiseEvent FitFinished(FitStopReason, FinalParameters, Chi2)
    End Sub

    ''' <summary>
    ''' An event to listen to, if you want to show a progress bar for single steps.
    ''' </summary>
    Public Event CalculationStepProgress(ByVal CalcItem As Integer,
                                         ByVal CalcMax As Integer,
                                         ByVal StepDescription As String) Implements iFitProcedure.CalculationStepProgress
    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_CalculationStepProgress(ByVal CalcItem As Integer,
                                                     ByVal CalcMax As Integer,
                                                     ByVal StepDescription As String)
        RaiseEvent CalculationStepProgress(CalcItem, CalcMax, StepDescription)
    End Sub

#End Region

#Region "Fit-Start/-Stop and Reporting functions"
    ''' <summary>
    ''' Initializes and starts the fit..
    ''' Override <code>Me.StopCondition</code> if you want to use another stop condition.
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
            Me._FitWrapper()
        End If

        Return True
    End Function

    ''' <summary>
    ''' Aborts the Fit-Procedure, if the Fit is running in a separate Thread.
    ''' Otherwise, if the fit is a direct fit, we can't abort it anyway.
    ''' </summary>
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

#Region "Fit-Procedure container"

    ''' <summary>
    ''' Actual Fit-Function. Must be implemented by the inherited class.
    ''' It does not have to care about saving back the fit-parameters to the base-class,
    ''' just put them back to the input-arrays.
    ''' </summary>
    Protected MustOverride Function _Fit(ByRef InValues As Double(),
                                         ByRef InIdentifiers As Integer(),
                                         ByRef InFixed As Boolean(),
                                         ByRef InLockedTo As Integer(),
                                         ByRef InUpperBoundaries As Double(),
                                         ByRef InLowerBoundaries As Double()) As Integer

    ''' <summary>
    ''' This function should be called after each iteration.
    ''' It takes care of adapting the locked parameters to each other.
    ''' </summary>
    Protected Sub LockParametersTogether(ByRef InIdentifiers As Integer(),
                                         ByRef InValues As Double(),
                                         ByRef InFixed As Boolean(),
                                         ByRef InLockedTo As Integer())

        ' Lock parameters together.
        For i As Integer = 0 To InLockedTo.Length - 1 Step 1
            If InLockedTo(i) >= 0 And InFixed(i) Then
                ' Get the other value
                For j As Integer = 0 To InIdentifiers.Length - 1 Step 1
                    If InIdentifiers(j) = InLockedTo(i) Then
                        InValues(i) = InValues(j)
                    End If
                Next
            End If
        Next
    End Sub

    ''' <summary>
    ''' Wrapper function that takes care of the initialization of the fit,
    ''' such as setting times, etc.
    ''' 
    ''' It then calls <code>_Fit</code>, that is implemented by the child-class,
    ''' and afterwards uses calculates the statistics for the fit.
    ''' </summary>
    Protected Sub _FitWrapper() Handles FitThreadWorker.DoWork

        Me._FitStartTime = Now
        IterationCount = 0

        ' Extract Fit-Parameters
        Me._FitParameters = Me.FitFunction.FitParameters

        ' get the arrays used for fitting from the parameter dictionary
        Dim InValues As Double() = Nothing
        Dim InIdentifiers As Integer() = Nothing
        Dim InFixed As Boolean() = Nothing
        Dim InLockedTo As Integer() = Nothing
        Dim InUpperBoundaries As Double() = Nothing
        Dim InLowerBoundaries As Double() = Nothing
        Dim InStandardDeviations As Double() = Nothing
        sFitParameter.GetFitParameterArrayFromDictionary(Me.FitParameters,
                                                         InIdentifiers,
                                                         InValues,
                                                         InFixed,
                                                         InLockedTo,
                                                         InUpperBoundaries,
                                                         InLowerBoundaries,
                                                         InStandardDeviations)

        ' Count and extract all non-fixed Fit-Parameter-Keys
        Me.NonFixedFitParameterIndices = sFitParameter.GetNonFixedIndizes(InFixed)
        Me.NonFixedFitParameterCount = Me.NonFixedFitParameterIndices.Count

        ' Check for enough fittable parameters
        If Me.NonFixedFitParameterCount = 0 Then
            RaiseEvent FitEcho("# Error: All fit-parameters are fixed! Nothing to fit!")
            Return
        End If

        '####################################################
        ' Call the actual fit-function from the child-class.
        Dim StopReason As Integer = Me._Fit(InValues, InIdentifiers, InFixed, InLockedTo, InUpperBoundaries, InLowerBoundaries)
        '####################################################

        ' Create the data using the fit-model as third column in the datapoint-array
        Dim CalculatedDataPoints(DataPoints(0).Length - 1) As Double
        For k As Integer = 0 To DataPoints(0).Length - 1 Step 1
            CalculatedDataPoints(k) = Me.FitFunction.GetY(DataPoints(0)(k), InIdentifiers, InValues)
        Next

        ' Calculate the final numeric statistics
        Me._FinalStatistics = cNumericalMethods.Statistics1D(CalculatedDataPoints, DataPoints(1))

        ' Calculate the standard-deviation of the Fit-Parameters using the Jacobian-Matrix
        ' It is given by the diagonal elements of (J'J)^-1.
        ' Discussed in Origin: http://originlab.com/www/helponline/Origin/en/UserGuide/The_Fit_Results.html
        For i As Integer = 0 To InStandardDeviations.Length - 1 Step 1
            InStandardDeviations(i) = 0.0
        Next
        Dim JacobianMatrix As Double(,) = Nothing
        If CalculateJacobianMatrix(Me.FitFunction,
                                   DataPoints,
                                   InIdentifiers,
                                   InValues,
                                   NonFixedFitParameterIndices,
                                   False,
                                   CalculatedDataPoints,
                                   JacobianMatrix) Then

            ' If Jacobian calculation successfull, get a matrix from it.
            Dim J As New DenseMatrix(NonFixedFitParameterCount, DataPoints(0).Length)
            ' Create variable shortcut the currently used parameter index
            For NonFixedKeyCounter As Integer = 0 To NonFixedFitParameterCount - 1 Step 1
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    J(NonFixedKeyCounter, i) = JacobianMatrix(NonFixedKeyCounter, i)
                Next
            Next
            Dim SSq As Double = (DataPoints(0).Length - InIdentifiers.Length)
            If SSq <> 0 Then
                SSq = Me._FinalStatistics.ResidualSumOfSquares / SSq
            End If
            Dim CovarianceMatrix As DenseMatrix = CType(J.TransposeAndMultiply(J).Inverse, DenseMatrix)  ' CType(J.Multiply(J.Transpose).Inverse, DenseMatrix) * SSq
            For i As Integer = 0 To Me.NonFixedFitParameterIndices.Length - 1 Step 1
                InStandardDeviations(Me.NonFixedFitParameterIndices(i)) = Math.Sqrt(CovarianceMatrix(i, i) * SSq)
            Next
        End If

        ' save best values back to parameters
        Me._FitParameters = sFitParameter.GetFitParameterDictionaryFromArrays(Me._FitParameters,
                                                                              InIdentifiers,
                                                                              InValues,
                                                                              InStandardDeviations)

        ' Loop ended, so fit finished!!!
        Me._FitEndTime = Now

        ' Raise the event to the listeners!
        RaiseEvent_FitFinished(StopReason, _FitParameters, _Chi2)
    End Sub

#End Region

#Region "Fit-Stop-Error code conversion"

    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Public MustOverride Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String Implements iFitProcedure.ConvertFitStopCodeToMessage

#End Region

#Region "Check the given weights, or set them to 1"

    ''' <summary>
    ''' Checks if the matrix of weights for each point is a 
    ''' matrix of positive elements. Otherwise it initializes
    ''' a new matrix and sets each value to 1
    ''' </summary>
    Protected Function CheckWeights(ByVal Length As Integer, ByVal Weights As Double()) As Double()
        Dim damaged As Boolean = False
        ' check for null
        If Weights Is Nothing Then
            'Trace.WriteLine("weights matrix was null")
            damaged = True
            Weights = New Double(Length - 1) {}
        Else
            ' check if all elements are zeros or if there are negative, NaN or Infinite elements
            Dim allZero As Boolean = True
            Dim illegalElement As Boolean = False
            Dim i As Integer = 0
            While i < Weights.Length AndAlso Not illegalElement
                If Weights(i) < 0 OrElse [Double].IsNaN(Weights(i)) OrElse [Double].IsInfinity(Weights(i)) Then
                    illegalElement = True
                End If
                allZero = (Weights(i) = 0) AndAlso allZero
                i += 1
            End While
            damaged = allZero OrElse illegalElement
        End If

        If damaged Then
            Trace.WriteLine("Weights were not well defined. All elements set to 1.")
            For i As Integer = 0 To Weights.Length - 1
                Weights(i) = 1
            Next
        End If

        Return Weights
    End Function

#End Region

#Region "Chi2 calculation"

    Public Const Chi2PenaltyForOutOfBoundaryParameters As Double = Double.MaxValue

    ''' <summary>
    ''' Calculates value of the function for given parameter array.
    ''' Also checks the boundaries of the parameters, and adds
    ''' a penalty factor, if the parameter value is outside the boundary.
    ''' </summary>
    ''' <returns>value of the function</returns>
    Protected Function CalculateChi2(ByRef Identifiers As Integer(),
                                     ByRef Values As Double(),
                                     ByRef UpperBoundaryIncl As Double(),
                                     ByRef LowerBoundaryIncl As Double()) As Double


        ' Check parameter boundaries in advance.
        ' If one parameter is out of range,
        ' add a high penalty factor, and abort the rest of the calculation.
        For i As Integer = 0 To Identifiers.Length - 1 Step 1
            If Values(i) > UpperBoundaryIncl(i) Or Values(i) < LowerBoundaryIncl(i) Then
                Return Chi2PenaltyForOutOfBoundaryParameters
            End If
        Next

        ' Reduce the FitEcho to integer percentage values
        Dim FitEchoValue As Integer = CInt(DataPoints(0).Length / 50)

        Dim Chi2 As Double = 0
        Dim dy As Double
        For i As Integer = 0 To DataPoints(0).Length - 1
            ' Progress
            If Not FitThreadWorker Is Nothing And i Mod FitEchoValue = 0 Then
                RaiseEvent_CalculationStepProgress(i, DataPoints(0).Length, "Calculating Chi2 ... ")
            End If

            ' Calculation
            dy = DataPoints(1)(i) - FitFunction.GetY(DataPoints(0)(i), Identifiers, Values)
            Chi2 += Weights(i) * dy * dy
        Next

        Return Chi2
    End Function

    ''' <summary>
    ''' Calculates value of the function for given parameter array.
    ''' Also checks the boundaries of the parameters, and adds
    ''' a penalty factor, if the parameter value is outside the boundary.
    ''' 
    ''' Use this for already calculated function values.
    ''' </summary>
    ''' <returns>value of the function</returns>
    Protected Function CalculateChi2(ByRef TestFunctionValueVector As Double(),
                                     ByRef Values As Double(),
                                     ByRef UpperBoundaryIncl As Double(),
                                     ByRef LowerBoundaryIncl As Double()) As Double


        ' Check parameter boundaries in advance.
        ' If one parameter is out of range,
        ' add a high penalty factor, and abort the rest of the calculation.
        For i As Integer = 0 To Values.Length - 1 Step 1
            If Values(i) > UpperBoundaryIncl(i) Or Values(i) < LowerBoundaryIncl(i) Then
                Return Chi2PenaltyForOutOfBoundaryParameters
            End If
        Next

        ' Reduce the FitEcho to integer percentage values
        Dim FitEchoValue As Integer = CInt(DataPoints(0).Length / 50)

        Dim Chi2 As Double = 0
        Dim dy As Double
        For i As Integer = 0 To DataPoints(0).Length - 1
            ' Progress
            If Not FitThreadWorker Is Nothing And i Mod FitEchoValue = 0 Then
                RaiseEvent_CalculationStepProgress(i, DataPoints(0).Length, "Calculating Chi2 ... ")
            End If

            ' Calculation
            dy = DataPoints(1)(i) - TestFunctionValueVector(i)
            Chi2 += Weights(i) * dy * dy
        Next

        Return Chi2
    End Function
#End Region

#Region "Jacobian Matrix"

    ''' <summary>
    ''' Delta between the partial derivatives used for calculating the jacobian
    ''' </summary>
    <Cudafy>
    Public Shared PartialDerivativeDelta As Double = 0.000001 ' 0.000000001

    ''' <summary>
    ''' Create Jacobian Matrix of all partial derivatives of the Fit-Function.
    ''' <code>UseCenterDifferentiationForJacobian</code> decides, whether center
    ''' differences will be used. They are more accurate, but require more function evaluations,
    ''' since for the forward differences the function evaluations already performed before are used.
    ''' 
    ''' ONLY CHECKS IF THE JACOBIAN EXISTS (IsNothing). DOES NOT CHECK THE LENGTH OF AN EXISTING ONE!!!
    ''' </summary>
    ''' <returns>Boolean, True, if successfull, false, if Jacobian contains NaN</returns>
    <Cudafy>
    Protected Shared Function CalculateJacobianMatrix(ByRef FitFunction As iFitFunction,
                                                      ByRef DataPoints As Double()(),
                                                      ByRef InIdentifiers As Integer(),
                                                      ByRef InValues As Double(),
                                                      ByRef InNonFixedParameterIndices As Integer(),
                                                      ByVal UseCenterDifferentiationForJacobian As Boolean,
                                                      ByRef FunctionValueVector As Double(),
                                                      ByRef JacobianMatrix As Double(,)) As Boolean

        ' Register Boolean result to return
        Dim ReturnResult As Boolean = True

        ' Copy the fit-parameter value set
        Dim CachedValues(InValues.Length - 1) As Double
        InValues.CopyTo(CachedValues, 0)

        ' Check the length of the Output matrix, or set it, if it does not fit.
        If JacobianMatrix Is Nothing Then
            JacobianMatrix = New Double(InNonFixedParameterIndices.Length - 1, DataPoints(0).Length - 1) {}
        End If

        ' Tmp-Variable
        Dim CurrentParameterIndex As Integer

        ' Decide weather to use centered evaluation, or
        ' forward numeric derivation.
        If UseCenterDifferentiationForJacobian Then
            '
            '    Centered Differences
            ' (more accurate than forward)
            '
            '##############################

            ' Generate the buffer arrays for the positive change in the elements
            Dim PartialDerivatives_Pos(DataPoints(0).Length - 1) As Double
            Dim PartialDerivatives_Neg(DataPoints(0).Length - 1) As Double

            ' Create variable shortcut the currently used parameter index
            For NonFixedKeyCounter As Integer = 0 To InNonFixedParameterIndices.Length - 1 Step 1
                ' Register currently used parameter index
                CurrentParameterIndex = InNonFixedParameterIndices(NonFixedKeyCounter)

                ' Calculate the positive elements
                CachedValues(CurrentParameterIndex) = InValues(CurrentParameterIndex) + PartialDerivativeDelta
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    PartialDerivatives_Pos(i) = FitFunction.GetY(DataPoints(0)(i), InIdentifiers, CachedValues)
                Next

                ' Calculate the negative elements
                CachedValues(CurrentParameterIndex) = InValues(CurrentParameterIndex) - PartialDerivativeDelta
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    PartialDerivatives_Neg(i) = FitFunction.GetY(DataPoints(0)(i), InIdentifiers, CachedValues)
                Next

                ' Calculate the final partial derivatives and write them to the cache:
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    JacobianMatrix(NonFixedKeyCounter, i) = (PartialDerivatives_Pos(i) - PartialDerivatives_Neg(i)) / (2 * PartialDerivativeDelta)
                    If Double.IsNaN(JacobianMatrix(NonFixedKeyCounter, i)) Then
                        ReturnResult = False
                    End If
                Next
            Next
        Else
            '
            '         Forward Differences
            ' (needs half the function evaluations)
            '
            '#######################################

            ' Create variable shortcut the currently used parameter index
            For NonFixedKeyCounter As Integer = 0 To InNonFixedParameterIndices.Length - 1 Step 1
                ' Register currently used parameter index
                CurrentParameterIndex = InNonFixedParameterIndices(NonFixedKeyCounter)

                ' Calculate the positive elements
                CachedValues(CurrentParameterIndex) = InValues(CurrentParameterIndex) + PartialDerivativeDelta
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    JacobianMatrix(NonFixedKeyCounter, i) = (FitFunction.GetY(DataPoints(0)(i), InIdentifiers, CachedValues) _
                                                                - FunctionValueVector(i)) / PartialDerivativeDelta
                    If Double.IsNaN(JacobianMatrix(NonFixedKeyCounter, i)) Then
                        ReturnResult = False
                    End If
                Next
            Next

        End If
        Return ReturnResult
    End Function

#End Region

End Class
