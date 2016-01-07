Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports System.ComponentModel
Imports MathNet.Numerics.LinearAlgebra.Double

''' <summary>
''' General Fit-Procedure class
''' with some shared functions.
''' </summary>
Public Class cFitProcedureSingleDataFit

#Region "Fit-Procedure"

    ''' <summary>
    ''' Fit-Procedure used.
    ''' </summary>
    Protected FitProcedure As iFitProcedure

    ''' <summary>
    ''' Property to save the procedure-specific fit-settings.
    ''' </summary>
    Public Property FitProcedureSettings As iFitProcedureSettings
        Get
            If Me.FitProcedure Is Nothing Then Return Nothing
            Return Me.FitProcedure.FitProcedureSettings
        End Get
        Set(value As iFitProcedureSettings)
            If Me.FitProcedure Is Nothing Then Return
            Me.FitProcedure.FitProcedureSettings = FitProcedureSettings
        End Set
    End Property

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Public ReadOnly Property ProcedureSettingPanel As cFitProcedureSettingsPanel
        Get
            Return Me.FitProcedure.ProcedureSettingPanel
        End Get
    End Property

    ''' <summary>
    ''' Sets the currently used fit-procedure.
    ''' </summary>
    Public Sub SetFitProcedure(ByRef Procedure As iFitProcedure)
        Me.FitProcedure = Procedure
    End Sub

#End Region

#Region "Parameters"
    ''' <summary>
    ''' interface of the function to be fitted
    ''' </summary>
    Protected FitFunction As iFitFunction

    ''' <summary>
    ''' The array of fit parameters.
    ''' </summary>
    Protected _FitParameters As cFitParameterGroupGroup

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
            Return Me.FitProcedure.Name
        End Get
    End Property

    ''' <summary>
    ''' Returns the parameter set of the current Fit.
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

    Protected _FinalStatistics As cNumericalMethods.sNumericStatistics
    ''' <summary>
    ''' Statistics, that get calculated after the fit has finished
    ''' </summary>
    Public ReadOnly Property FinalStatistics As cNumericalMethods.sNumericStatistics
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
                             ByVal Chi2 As Double)

    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_FitStepEcho(ByVal Parameters As cFitParameterGroupGroup,
                                         ByVal Chi2 As Double)
        RaiseEvent FitStepEcho(Parameters, Chi2)
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
                             ByVal Chi2 As Double)

    ''' <summary>
    ''' Caller for inherited classes.
    ''' </summary>
    Protected Sub RaiseEvent_FitFinished(ByVal FitStopReason As Integer,
                                         ByVal FinalParameters As cFitParameterGroupGroup,
                                         ByVal Chi2 As Double)
        RaiseEvent FitFinished(FitStopReason, FinalParameters, Chi2)
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
    ''' Initializes and starts the fit..
    ''' Override <code>Me.StopCondition</code> if you want to use another stop condition.
    ''' </summary>
    Public Sub DirectFit(ByVal ModelFitFunction As iFitFunction,
                         ByVal FitDataPoints As Double()(),
                         ByVal Weights As Double())
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
                             ByVal Weights As Double()) As Boolean
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
    ''' such as setting times, etc. And calls initially the Initializer-Function
    ''' of the FitProcedure. 
    ''' 
    ''' It then calls <code>FitIteration</code> of the FitProcedure,
    ''' that is implemented by the child-class.
    ''' Afterwards it calls the FitFinalizer and calculates the statistics for the fit.
    ''' </summary>
    Protected Sub _FitWrapper() Handles FitThreadWorker.DoWork

        ' Save the time when the fit was started
        Me._FitStartTime = Now

        ' Set the interation counter to 0
        IterationCount = 0

        ' Define a temporary variable
        Dim FitEchoTime_Current As Long = 0

        ' Set the best Chi2 Found
        Dim Chi2_Current As Double = Double.MaxValue
        ' Saves the best Chi2 reached so far!
        Dim Chi2_Best As Double = Double.MaxValue

        ' Extract Fit-Parameter-Groups from the given Fit-Function
        Me._FitParameters = Me.FitFunction.FitParametersGrouped

        '####################################################
        ' Call the actual fit-function from the FitProcedure

        ' Call the initial stop reason
        Dim StopReason As Integer = cFitProcedureBase.FitStopReasons.None

        With Me.FitProcedure

            ' Initialize the FitProcedure
            .FitInitializer(Me.FitFunction, Me.DataPoints, Me.Weights)

            ' Perform a the initial calculation of the Chi2
            Chi2_Current = .FitStep(Me.DataPoints, Me.Weights, Me._FitParameters, IterationCount, StopReason)

            '####################
            ' Start the fit-loop
            While StopReason = cFitProcedureBase.FitStopReasons.None

                ' Perform a Fit-Echo
                RaiseEvent_CalculationStepProgress(IterationCount, Me.FitProcedureSettings.StopCondition_MaxIterations, "Iteration: " & IterationCount.ToString("N0") & ", Chi2: " & Chi2_Current.ToString("E2"))

                If Chi2_Current < Chi2_Best Then
                    ' Check for the stop condition
                    If (Chi2_Best - Chi2_Current) < Me.FitProcedureSettings.StopCondition_MinChi2Change Then
                        StopReason = cFitProcedureBase.FitStopReasons.FitConverged
                    End If

                    ' Save the new best Chi2
                    Chi2_Best = Chi2_Current

                    ' Echo the new parameter-set
                    FitEchoTime_Current = Now.Ticks

                    If (FitEchoTime_Current - Me._FitEchoTime_Last) >= _FitEchoMinTimeSpan Then
                        Me._FitEchoTime_Last = FitEchoTime_Current
                        'Me._FitParameters = cFitParameter.GetFitParameterDictionaryFromArrays(Me._FitParameters, InIdentifiers, InValues)
                        RaiseEvent_FitStepEcho(Me._FitParameters, Chi2_Best) ' _Chi2)
                    End If
                End If

                ' Check for a cancellation request by the user
                If Me.FitThreadWorker.CancellationPending Then
                    StopReason = cFitProcedureBase.FitStopReasons.UserAborted
                End If

                ' Check for reaching the max iteration count.
                If IterationCount > Me.FitProcedureSettings.StopCondition_MaxIterations Then
                    StopReason = cFitProcedureBase.FitStopReasons.MaxIterationsReached
                End If

                ' Perform the next Fit-Step
                Chi2_Current = .FitStep(Me.DataPoints, Me.Weights, Me._FitParameters, IterationCount, StopReason)

                ' Increase Iteration Counter
                IterationCount += 1
            End While
            '####################

            ' Call the procedure's finalizer
            .FitFinalizer(Me.DataPoints, Me.Weights, Me._FitParameters)
        End With
        '####################################################

        ' Create the data using the fit-model as third column in the datapoint-array
        Dim CalculatedDataPoints(DataPoints(0).Length - 1) As Double
        For k As Integer = 0 To DataPoints(0).Length - 1 Step 1
            CalculatedDataPoints(k) = Me.FitFunction.GetY(DataPoints(0)(k), Me._FitParameters)
        Next

        ' Calculate the final numeric statistics
        Me._FinalStatistics = cNumericalMethods.Statistics1D(CalculatedDataPoints, DataPoints(1))

        ' Calculate the standard-deviation of the Fit-Parameters using the Jacobian-Matrix
        ' It is given by the diagonal elements of (J'J)^-1.
        ' Discussed in Origin: http://originlab.com/www/helponline/Origin/en/UserGuide/The_Fit_Results.html
        For Each FPG As cFitParameterGroup In Me._FitParameters
            For Each FP As cFitParameter In FPG
                FP.StandardDeviation = 0
            Next
        Next
        Dim JacobianMatrix As Double(,) = Nothing
        'If cFitProcedureBase.CalculateJacobianMatrix(Me.FitFunction,
        '                                             DataPoints,
        '                                             Me._FitParameters,
        '                                             False,
        '                                             CalculatedDataPoints,
        '                                             JacobianMatrix) Then

        '    ' If Jacobian calculation successfull, get a matrix from it.
        '    Dim J As New DenseMatrix(NonFixedFitParameterCount, DataPoints(0).Length)
        '    ' Create variable shortcut the currently used parameter index
        '    For NonFixedKeyCounter As Integer = 0 To NonFixedFitParameterCount - 1 Step 1
        '        For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
        '            J(NonFixedKeyCounter, i) = JacobianMatrix(NonFixedKeyCounter, i)
        '        Next
        '    Next
        '    Dim SSq As Double = (DataPoints(0).Length - Me._FitParameters.Length)
        '    If SSq <> 0 Then
        '        SSq = Me._FinalStatistics.ResidualSumOfSquares / SSq
        '    End If
        '    Dim CovarianceMatrix As DenseMatrix = CType(J.TransposeAndMultiply(J).Inverse, DenseMatrix)  ' CType(J.Multiply(J.Transpose).Inverse, DenseMatrix) * SSq
        '    For i As Integer = 0 To Me.NonFixedFitParameterIndices.Length - 1 Step 1
        '        InStandardDeviations(Me.NonFixedFitParameterIndices(i)) = Math.Sqrt(CovarianceMatrix(i, i) * SSq)
        '    Next
        'End If

        ' Clear resources
        JacobianMatrix = Nothing

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
    Public Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String
        Select Case FitStopCode
            Case cFitProcedureBase.FitStopReasons.FitConverged
                Return My.Resources.rFitProcedure.FitStopReason_FitConverged.Replace("#d", Me.FitProcedureSettings.StopCondition_MinChi2Change.ToString("E3"))
            Case cFitProcedureBase.FitStopReasons.MaxIterationsReached
                Return My.Resources.rFitProcedure.FitStopReason_MaxIterationsReached.Replace("#d", Me.FitProcedureSettings.StopCondition_MaxIterations.ToString("N0"))
            Case cFitProcedureBase.FitStopReasons.UserAborted
                Return My.Resources.rFitProcedure.FitStopReason_UserAborted
            Case Else
                ' Try to load the procedures internal reasons
                Return Me.FitProcedure.ConvertFitStopCodeToMessage(FitStopCode)
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
