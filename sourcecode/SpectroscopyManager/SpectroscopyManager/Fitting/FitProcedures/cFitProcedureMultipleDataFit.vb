Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports System.ComponentModel
Imports MathNet.Numerics.LinearAlgebra.Double

''' <summary>
''' This fit-function has the purpose to fit multiple data-sets simultaneously
''' and linking parameters between the fits together.
''' </summary>
Public Class cFitProcedureMultipleDataFit

#Region "Fit-Procedure"

    ''' <summary>
    ''' Fit-Procedure used.
    ''' </summary>
    Protected FitProcedure_Set1 As iFitProcedure

    ''' <summary>
    ''' Property to save the procedure-specific fit-settings.
    ''' </summary>
    Public Property FitProcedureSettings_Set1 As iFitProcedureSettings
        Get
            If Me.FitProcedure_Set1 Is Nothing Then Return Nothing
            Return Me.FitProcedure_Set1.FitProcedureSettings
        End Get
        Set(value As iFitProcedureSettings)
            If Me.FitProcedure_Set1 Is Nothing Then Return
            Me.FitProcedure_Set1.FitProcedureSettings = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Public ReadOnly Property ProcedureSettingPanel_Set1 As cFitProcedureSettingsPanel
        Get
            Return Me.FitProcedure_Set1.ProcedureSettingPanel
        End Get
    End Property


    ''' <summary>
    ''' Fit-Procedure used.
    ''' </summary>
    Protected FitProcedure_Set2 As iFitProcedure

    ''' <summary>
    ''' Property to save the procedure-specific fit-settings.
    ''' </summary>
    Public Property FitProcedureSettings_Set2 As iFitProcedureSettings
        Get
            If Me.FitProcedure_Set2 Is Nothing Then Return Nothing
            Return Me.FitProcedure_Set2.FitProcedureSettings
        End Get
        Set(value As iFitProcedureSettings)
            If Me.FitProcedure_Set2 Is Nothing Then Return
            Me.FitProcedure_Set2.FitProcedureSettings = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Public ReadOnly Property ProcedureSettingPanel_Set2 As cFitProcedureSettingsPanel
        Get
            Return Me.FitProcedure_Set2.ProcedureSettingPanel
        End Get
    End Property

    ''' <summary>
    ''' Sets the currently used fit-procedure.
    ''' </summary>
    Public Sub SetFitProcedure(ByRef Procedure_Set1 As iFitProcedure, ByRef Procedure_Set2 As iFitProcedure)
        Me.FitProcedure_Set1 = Procedure_Set1
        Me.FitProcedure_Set2 = Procedure_Set2
    End Sub

#End Region

#Region "Parameters"
    ''' <summary>
    ''' interface of the function to be fitted for the first data-set
    ''' </summary>
    Protected FitFunction_Set1 As iFitFunction

    ''' <summary>
    ''' interface of the function to be fitted for the second data-set
    ''' </summary>
    Protected FitFunction_Set2 As iFitFunction

    ''' <summary>
    ''' The array of fit parameters for both data-set.
    ''' </summary>
    Protected _FitParameters As cFitParameterGroupGroup

    ''' <summary>
    ''' Measured data points for which the model function is to be fitted.
    ''' double[0 = x, 1 = y][data point index] = data value
    ''' Data-Set 1
    ''' </summary>
    Protected DataPoints_Set1 As Double()()

    ''' <summary>
    ''' Measured data points for which the model function is to be fitted.
    ''' double[0 = x, 1 = y][data point index] = data value
    ''' Data-Set 2
    ''' </summary>
    Protected DataPoints_Set2 As Double()()

    ''' <summary>
    ''' Weights for each data point. The merit function is: chi2 = sum[ (y_i - y(x_i;a))^2 * w_i ].
    ''' For gaussian errors in datapoints, set w_i = 1 / sigma_i.
    ''' </summary>
    Protected Weights_Set1 As Double()

    ''' <summary>
    ''' Weights for each data point. The merit function is: chi2 = sum[ (y_i - y(x_i;a))^2 * w_i ].
    ''' For gaussian errors in datapoints, set w_i = 1 / sigma_i.
    ''' </summary>
    Protected Weights_Set2 As Double()

    ''' <summary>
    ''' temporary calculated Chi2 value of the first set
    ''' </summary>
    Protected _Chi2_Set1 As Double

    ''' <summary>
    ''' temporary calculated Chi2 value of the second set
    ''' </summary>
    Protected _Chi2_Set2 As Double

    ''' <summary>
    ''' temporary calculated Chi2 value of both sets summed
    ''' </summary>
    Protected ReadOnly Property _Chi2_Total As Double
        Get
            Return Me._Chi2_Set1 + Me._Chi2_Set2
        End Get
    End Property

    ''' <summary>
    ''' Counts the iteration.
    ''' </summary>
    Protected IterationCount As Integer

    ' Variable for saving the Fit-Duration
    Protected _FitStartTime As Date
    Protected _FitEndTime As Date

    ''' <summary>
    ''' Saves the timestamp of the last fit-echo.
    ''' By This we can avoid flooding the echo-box,
    ''' by limiting the time between two echos.
    ''' </summary>
    Protected _FitEchoTime_Last As Long

    ''' <summary>
    ''' Timespan between two fit-echos in ms.
    ''' </summary>
    Protected _FitEchoMinTimeSpan As Long = Convert.ToInt64(TimeSpan.TicksPerSecond / 2)

#End Region

#Region "Property accessors"

    ''' <summary>
    ''' Fit-Procedure-Name
    ''' </summary>
    Public ReadOnly Property Name As String
        Get
            Return Me.FitProcedure_Set1.Name
        End Get
    End Property

    ''' <summary>
    ''' Returns the parameter set of the current Fit
    ''' </summary>
    Public ReadOnly Property FitParameters() As cFitParameterGroupGroup
        Get
            Return Me._FitParameters
        End Get
    End Property

    ''' <summary>
    ''' Returns the number of iterations used for the fit.
    ''' </summary>
    Public ReadOnly Property Iterations() As Integer
        Get
            Return Me.IterationCount
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
    Public ReadOnly Property FitDuration As TimeSpan
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
    Public ReadOnly Property FitStartTime As Date
        Get
            Return Me._FitStartTime
        End Get
    End Property

    ''' <summary>
    ''' Time at which the Fit has ended
    ''' </summary>
    Public ReadOnly Property FitEndTime As Date
        Get
            Return Me._FitEndTime
        End Get
    End Property

    Protected _FinalStatistics_Set1 As cNumericalMethods.sNumericStatistics
    ''' <summary>
    ''' Statistics, that get calculated after the fit has finished
    ''' </summary>
    Public ReadOnly Property FinalStatistics_Set1 As cNumericalMethods.sNumericStatistics
        Get
            Return Me._FinalStatistics_Set1
        End Get
    End Property

    Protected _FinalStatistics_Set2 As cNumericalMethods.sNumericStatistics
    ''' <summary>
    ''' Statistics, that get calculated after the fit has finished
    ''' </summary>
    Public ReadOnly Property FinalStatistics_Set2 As cNumericalMethods.sNumericStatistics
        Get
            Return Me._FinalStatistics_Set2
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
    Public ReadOnly Property FitWorker As BackgroundWorker
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
    Public Event FitStepEcho(ByVal Parameters As cFitParameterGroupGroup,
                             ByVal Chi2_Set1 As Double,
                             ByVal Chi2_Set2 As Double)

    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_FitStepEcho(ByVal Parameters As cFitParameterGroupGroup,
                                         ByVal Chi2_Set1 As Double,
                                         ByVal Chi2_Set2 As Double)
        RaiseEvent FitStepEcho(Parameters, Chi2_Set1, Chi2_Set2)
    End Sub

    ''' <summary>
    ''' Echos the current parameter set, whenever
    ''' the parameters got changed to a minimized Chi2. 
    ''' </summary>
    Public Event FitEcho(ByVal Message As String)

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
                             ByVal FinalParameters As cFitParameterGroupGroup,
                             ByVal Chi2_Set1 As Double,
                             ByVal Chi2_Set2 As Double)

    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_FitFinished(ByVal FitStopReason As Integer,
                                         ByVal FinalParameters As cFitParameterGroupGroup,
                                         ByVal Chi2_Set1 As Double,
                                         ByVal Chi2_Set2 As Double)
        RaiseEvent FitFinished(FitStopReason, FinalParameters, Chi2_Set1, Chi2_Set2)
    End Sub

    ''' <summary>
    ''' An event to listen to, if you want to show a progress bar for single steps.
    ''' </summary>
    Public Event CalculationStepProgress(ByVal CalcItem As Integer,
                                         ByVal CalcMax As Integer,
                                         ByVal StepDescription As String)
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
    ''' Initializes and starts the fit with two fit-functions.
    ''' Override <code>Me.StopCondition</code> if you want to use another stop condition.
    ''' </summary>
    Public Sub DirectFit(ByVal ModelFitFunction_Set1 As iFitFunction,
                         ByVal ModelFitFunction_Set2 As iFitFunction,
                         ByVal FitDataPoints_Set1 As Double()(),
                         ByVal FitDataPoints_Set2 As Double()(),
                         ByVal Weights_Set1 As Double(),
                         ByVal Weights_Set2 As Double())
        Me.StartFit(ModelFitFunction_Set1, ModelFitFunction_Set2,
                    FitDataPoints_Set1, FitDataPoints_Set2,
                    Weights_Set1, Weights_Set2,
                    False)
    End Sub

    ''' <summary>
    ''' Starts the fit in a background thread. If used after calling fit(lambda, minDeltaChi2, maxIterations),
    ''' uses those values. The stop condition is fetched from <code>this.stop()</code>.
    ''' Override <code>this.stop()</code> if you want to use another stop condition.
    ''' </summary>
    ''' <returns>True if new thread could be started. False, if thread was already running.</returns>
    Public Function FitAsync(ByVal ModelFitFunction_Set1 As iFitFunction,
                             ByVal ModelFitFunction_Set2 As iFitFunction,
                             ByVal FitDataPoints_Set1 As Double()(),
                             ByVal FitDataPoints_Set2 As Double()(),
                             ByVal Weights_Set1 As Double(),
                             ByVal Weights_Set2 As Double()) As Boolean
        Return Me.StartFit(ModelFitFunction_Set1, ModelFitFunction_Set2,
                           FitDataPoints_Set1, FitDataPoints_Set2,
                           Weights_Set1, Weights_Set2,
                           True)
    End Function

    ''' <summary>
    ''' Starts the fit in a background thread or directly, as defined. 
    ''' </summary>
    Public Function StartFit(ByVal ModelFitFunction_Set1 As iFitFunction,
                             ByVal ModelFitFunction_Set2 As iFitFunction,
                             ByVal FitDataPoints_Set1 As Double()(),
                             ByVal FitDataPoints_Set2 As Double()(),
                             ByVal Weights_Set1 As Double(),
                             ByVal Weights_Set2 As Double(),
                             ByVal StartAsync As Boolean) As Boolean
        ' Check for running Fit-Worker.
        If Me.FitThreadWorker.IsBusy And StartAsync Then
            Return False
        End If

        ' Integritiy Check of the Data
        If FitDataPoints_Set1.Length <> 2 Then Throw New ArgumentException("Data point array (set 1) must be 2 x N")
        If FitDataPoints_Set2.Length <> 2 Then Throw New ArgumentException("Data point array (set 2) must be 2 x N")

        If FitDataPoints_Set1(0).Length <> FitDataPoints_Set1(1).Length Then Throw New ArgumentException("Data must have the same number of x and y points. (set 1)")
        If FitDataPoints_Set2(0).Length <> FitDataPoints_Set2(1).Length Then Throw New ArgumentException("Data must have the same number of x and y points. (set 2)")

        Me.FitFunction_Set1 = ModelFitFunction_Set1
        Me.FitFunction_Set2 = ModelFitFunction_Set2
        Me.DataPoints_Set1 = FitDataPoints_Set1
        Me.DataPoints_Set2 = FitDataPoints_Set2
        Me.Weights_Set1 = CheckWeights(Me.DataPoints_Set1(0).Length, Weights_Set1)
        Me.Weights_Set2 = CheckWeights(Me.DataPoints_Set2(0).Length, Weights_Set2)

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
    Public Sub AbortAsyncFit()
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
    ''' Wrapper function that takes care of the initialization of the fit,
    ''' such as setting times, etc.
    ''' 
    ''' It calls the initializer of each fit-method before entering the fit-loop.
    ''' It then calls <code>_FitStep</code>, that is implemented by the child-class,
    ''' and afterwards calculates statistics for the fit.
    ''' </summary>
    Protected Sub _FitWrapper() Handles FitThreadWorker.DoWork

        ' Save the time when the fit was started
        Me._FitStartTime = Now

        ' Set the interation counter to 0
        IterationCount = 0

        ' Define a temporary variable
        Dim FitEchoTime_Current As Long = 0

        ' Set the best Chi2 Found
        Dim Chi2_Set1_Current As Double = Double.MaxValue
        Dim Chi2_Set2_Current As Double = Double.MaxValue
        ' Saves the best Chi2 reached so far!
        Dim Chi2_Set1_Best As Double = Double.MaxValue
        Dim Chi2_Set2_Best As Double = Double.MaxValue

        Dim Chi2_Total_Current As Double = Double.MaxValue
        Dim Chi2_Total_Best As Double = Double.MaxValue

        ' Extract Fit-Parameters
        Me._FitParameters = Me.FitFunction_Set1.FitParametersGrouped
        Me._FitParameters.Add(Me.FitFunction_Set2.FitParametersGrouped)

        ' Check for enough fittable parameters
        If Me._FitParameters.Group(Me.FitFunction_Set1.UseFitParameterGroupID).CountNonFixed = 0 Or Me._FitParameters.Group(Me.FitFunction_Set2.UseFitParameterGroupID).CountNonFixed = 0 Then
            RaiseEvent FitEcho("# Error: All fit-parameters are fixed! Nothing to fit!")
            Return
        End If

        '####################################################
        ' Call the actual fit-function from the FitProcedure

        ' Call the initial stop reason
        Dim StopReason As Integer = cFitProcedureBase.FitStopReasons.None

        ' Initialize the FitProcedure for Set 1 and Set 2
        Me.FitProcedure_Set1.FitInitializer(Me.FitFunction_Set1,
                                            Me.DataPoints_Set1,
                                            Me.Weights_Set1)
        Me.FitProcedure_Set2.FitInitializer(Me.FitFunction_Set2,
                                            Me.DataPoints_Set2,
                                            Me.Weights_Set2)

        ' Perform a the initial calculation of the Chi2
        Chi2_Set1_Current = Me.FitProcedure_Set1.FitStep(Me.DataPoints_Set1,
                                                         Me.Weights_Set1,
                                                         Me._FitParameters,
                                                         IterationCount,
                                                         StopReason)
        Chi2_Set2_Current = Me.FitProcedure_Set2.FitStep(Me.DataPoints_Set2,
                                                         Me.Weights_Set2,
                                                         Me._FitParameters,
                                                         IterationCount,
                                                         StopReason)

        '####################
        ' Start the fit-loop
        While StopReason = cFitProcedureBase.FitStopReasons.None

            ' Perform a Fit-Echo
            RaiseEvent_CalculationStepProgress(IterationCount, Me.FitProcedureSettings_Set1.StopCondition_MaxIterations, "Iteration: " & IterationCount.ToString("N0") & ", Chi2: " & (Chi2_Set1_Current + Chi2_Set2_Current).ToString("E2"))

            If Chi2_Set1_Current < Chi2_Set1_Best Or Chi2_Set2_Current < Chi2_Set2_Best Then

                ' Check for the stop condition
                If (Chi2_Set1_Best - Chi2_Set1_Current + Chi2_Set2_Best - Chi2_Set2_Current) < Me.FitProcedureSettings_Set1.StopCondition_MinChi2Change Then
                    StopReason = cFitProcedureBase.FitStopReasons.FitConverged
                End If

                ' Save the new best Chi2
                Chi2_Set1_Best = Chi2_Set1_Current
                Chi2_Set2_Best = Chi2_Set2_Current

                ' Echo the new parameter-set
                FitEchoTime_Current = Now.Ticks

                If (FitEchoTime_Current - Me._FitEchoTime_Last) >= _FitEchoMinTimeSpan Then
                    Me._FitEchoTime_Last = FitEchoTime_Current
                    'Me._FitParameters_Set1 = cFitParameter.GetFitParameterDictionaryFromArrays(Me._FitParameters_Set1, InIdentifiers_Set1, InValues_Set1)
                    'Me._FitParameters_Set2 = cFitParameter.GetFitParameterDictionaryFromArrays(Me._FitParameters_Set2, InIdentifiers_Set2, InValues_Set2)
                    RaiseEvent_FitStepEcho(Me._FitParameters, Chi2_Set1_Best, Chi2_Set2_Best) ' _Chi2)
                End If
            End If


            ' Check for a cancellation request by the user
            If Me.FitThreadWorker.CancellationPending Then
                StopReason = cFitProcedureBase.FitStopReasons.UserAborted
            End If

            ' Check for reaching the max iteration count.
            If IterationCount > Me.FitProcedureSettings_Set1.StopCondition_MaxIterations Then
                StopReason = cFitProcedureBase.FitStopReasons.MaxIterationsReached
            End If

            ' Perform the next Fit-Step
            Chi2_Set1_Current = Me.FitProcedure_Set1.FitStep(Me.DataPoints_Set1,
                                                             Me.Weights_Set1,
                                                             Me._FitParameters,
                                                             IterationCount,
                                                             StopReason)
            Chi2_Set2_Current = Me.FitProcedure_Set2.FitStep(Me.DataPoints_Set2,
                                                             Me.Weights_Set2,
                                                             Me._FitParameters,
                                                             IterationCount,
                                                             StopReason)

            ' Increase Iteration Counter
            IterationCount += 1
        End While
        '####################

        ' Call the procedure's finalizer
        Me.FitProcedure_Set1.FitFinalizer(Me.DataPoints_Set1, Me.Weights_Set1, Me._FitParameters)

        Me.FitProcedure_Set2.FitFinalizer(Me.DataPoints_Set2, Me.Weights_Set2, Me._FitParameters)

        '####################################################

        ' Create the data using the fit-model as third column in the datapoint-array
        Dim CalculatedDataPoints_Set1(DataPoints_Set1(0).Length - 1) As Double
        For k As Integer = 0 To DataPoints_Set1(0).Length - 1 Step 1
            CalculatedDataPoints_Set1(k) = Me.FitFunction_Set1.GetY(DataPoints_Set1(0)(k), Me._FitParameters)
        Next
        '
        Dim CalculatedDataPoints_Set2(DataPoints_Set2(0).Length - 1) As Double
        For k As Integer = 0 To DataPoints_Set2(0).Length - 1 Step 1
            CalculatedDataPoints_Set2(k) = Me.FitFunction_Set2.GetY(DataPoints_Set2(0)(k), Me._FitParameters)
        Next

        ' Calculate the final numeric statistics
        Me._FinalStatistics_Set1 = cNumericalMethods.Statistics1D(CalculatedDataPoints_Set1, DataPoints_Set1(1))
        Me._FinalStatistics_Set2 = cNumericalMethods.Statistics1D(CalculatedDataPoints_Set2, DataPoints_Set2(1))

        ' Calculate the standard-deviation of the Fit-Parameters using the Jacobian-Matrix
        ' It is given by the diagonal elements of (J'J)^-1.
        ' Discussed in Origin: http://originlab.com/www/helponline/Origin/en/UserGuide/The_Fit_Results.html
        For Each FPG As cFitParameterGroup In Me._FitParameters
            For Each FP As cFitParameter In FPG
                FP.StandardDeviation = 0
            Next
        Next
        For Each FPG As cFitParameterGroup In Me._FitParameters
            For Each FP As cFitParameter In FPG
                FP.StandardDeviation = 0
            Next
        Next

        Dim JacobianMatrix As Double(,)
        JacobianMatrix = Nothing
        'If cFitProcedureBase.CalculateJacobianMatrix(Me.FitFunction_Set1,
        '                                             DataPoints_Set1,
        '                                             Me._FitParameters_Set1,
        '                                             False,
        '                                             CalculatedDataPoints_Set1,
        '                                             JacobianMatrix) Then

        '    ' If Jacobian calculation successfull, get a matrix from it.
        '    Dim J As New DenseMatrix(NonFixedFitParameterCount_Set1, DataPoints_Set1(0).Length)
        '    ' Create variable shortcut the currently used parameter index
        '    For NonFixedKeyCounter As Integer = 0 To NonFixedFitParameterCount_Set1 - 1 Step 1
        '        For i As Integer = 0 To DataPoints_Set1(0).Length - 1 Step 1
        '            J(NonFixedKeyCounter, i) = JacobianMatrix(NonFixedKeyCounter, i)
        '        Next
        '    Next
        '    Dim SSq As Double = (DataPoints_Set1(0).Length - Me._FitParameters_Set1.Group(Me.FitFunction_Set1.UseFitParameterGroupID).Count)
        '    If SSq <> 0 Then
        '        SSq = Me._FinalStatistics_Set1.ResidualSumOfSquares / SSq
        '    End If
        '    Dim CovarianceMatrix As DenseMatrix = CType(J.TransposeAndMultiply(J).Inverse, DenseMatrix)  ' CType(J.Multiply(J.Transpose).Inverse, DenseMatrix) * SSq
        '    For i As Integer = 0 To Me.NonFixedFitParameterIndices_Set1.Length - 1 Step 1
        '        InStandardDeviations_Set1(Me.NonFixedFitParameterIndices_Set1(i)) = Math.Sqrt(CovarianceMatrix(i, i) * SSq)
        '    Next
        'End If

        'JacobianMatrix = Nothing
        'If cFitProcedureBase.CalculateJacobianMatrix(Me.FitFunction_Set2,
        '                                             DataPoints_Set2,
        '                                             Me._FitParameters_Set2,
        '                                             False,
        '                                             CalculatedDataPoints_Set2,
        '                                             JacobianMatrix) Then

        '    ' If Jacobian calculation successfull, get a matrix from it.
        '    Dim J As New DenseMatrix(NonFixedFitParameterCount_Set2, DataPoints_Set2(0).Length)
        '    ' Create variable shortcut the currently used parameter index
        '    For NonFixedKeyCounter As Integer = 0 To NonFixedFitParameterCount_Set2 - 1 Step 1
        '        For i As Integer = 0 To DataPoints_Set2(0).Length - 1 Step 1
        '            J(NonFixedKeyCounter, i) = JacobianMatrix(NonFixedKeyCounter, i)
        '        Next
        '    Next
        '    Dim SSq As Double = (DataPoints_Set2(0).Length - Me._FitParameters_Set2.Group(Me.FitFunction_Set2.UseFitParameterGroupID).Count)
        '    If SSq <> 0 Then
        '        SSq = Me._FinalStatistics_Set2.ResidualSumOfSquares / SSq
        '    End If
        '    Dim CovarianceMatrix As DenseMatrix = CType(J.TransposeAndMultiply(J).Inverse, DenseMatrix)  ' CType(J.Multiply(J.Transpose).Inverse, DenseMatrix) * SSq
        '    For i As Integer = 0 To Me.NonFixedFitParameterIndices_Set2.Length - 1 Step 1
        '        InStandardDeviations_Set2(Me.NonFixedFitParameterIndices_Set2(i)) = Math.Sqrt(CovarianceMatrix(i, i) * SSq)
        '    Next
        'End If

        ' Release Resources
        JacobianMatrix = Nothing

        ' Loop ended, so fit finished!!!
        Me._FitEndTime = Now

        ' Raise the event to the listeners!
        RaiseEvent_FitFinished(StopReason, _FitParameters, _Chi2_Set1, _Chi2_Set2)
    End Sub

#End Region

#Region "Fit-Stop-Error code conversion"

    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Public Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String
        Select Case FitStopCode
            Case cFitProcedureBase.FitStopReasons.FitConverged
                Return My.Resources.rFitProcedure.FitStopReason_FitConverged.Replace("#d", Me.FitProcedureSettings_Set1.StopCondition_MinChi2Change.ToString("E3"))
            Case cFitProcedureBase.FitStopReasons.MaxIterationsReached
                Return My.Resources.rFitProcedure.FitStopReason_MaxIterationsReached.Replace("#d", Me.FitProcedureSettings_Set1.StopCondition_MaxIterations.ToString("N0"))
            Case cFitProcedureBase.FitStopReasons.UserAborted
                Return My.Resources.rFitProcedure.FitStopReason_UserAborted
            Case Else
                ' Try to load the procedures internal reasons
                Return Me.FitProcedure_Set1.ConvertFitStopCodeToMessage(FitStopCode)
        End Select
    End Function

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

End Class
