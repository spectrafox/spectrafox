Imports System.Diagnostics
Imports System.Threading
Imports System.ComponentModel

''' <summary>
''' A class which implements the <i>Nelder-Mead Algorithm</i> (NMA)
''' fit for non-linear, multidimensional parameter space. 
''' 
''' The classic algorithm is extended by:
''' 1) optional simulated annealing of the parameters,
''' 2) optional adaptive Nelder-Mead expansion, reflection and contraction parameters,
'''    depending on the dimension of the problem.
'''    "Implementing the Nelder-Mead simplex algorithm with adaptive parameters"
'''    Fuchang Gao, Lixing Han, Comput Optim Appl, DOI 10.1007/s10589-010-9329-3
'''    http://www.webpages.uidaho.edu/~fuchang/res/ANMS.pdf
''' </summary>
Public Class cNMAFit
    Implements iFitProcedure

    ' default fit procedure settings
    Private Const DefaultMinDeltaChi2 As Double = 0.00000001
    Private Const DefaultMaxIterations As Integer = 3000
    Private Const DefaultAnnealingStartTemperature As Double = 0.000000001
    Private Const DefaultAnnealingSteps As Integer = 500
    Private Const DefaultUseSimulatedAnnealing As Boolean = False
    Private Const DefaultSimulatedAnnealingCoolingType As cFitProcedureSettings.SimulatedAnnealingCoolingTypes = cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Exponential
    Private Const DefaultUseAdaptiveNelderMeadFactors As Boolean = False

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
                Return GetType(cNMAFit)
            End Get
        End Property

        Private _MinDeltaChi2 As Double = DefaultMinDeltaChi2
        ''' <summary>
        ''' Sets the minimum change in Chi^2 for which to abort the fit
        ''' </summary>
        Public Property MinDeltaChi2 As Double
            Get
                Return _MinDeltaChi2
            End Get
            Set(value As Double)
                _MinDeltaChi2 = value
                My.Settings.NMAFit_MinChi2 = value
                My.Settings.Save()
            End Set
        End Property

        Private _MaxIterations As Integer = DefaultMaxIterations
        ''' <summary>
        ''' Sets the maximum number of iterations, after which to abort the fit.
        ''' </summary>
        Public Property MaxIteration As Integer
            Get
                Return _MaxIterations
            End Get
            Set(value As Integer)
                _MaxIterations = value
                My.Settings.NMAFit_MaxIterations = value
                My.Settings.Save()
            End Set
        End Property

        Private _UseSimulatedAnnealing As Boolean = DefaultUseSimulatedAnnealing
        ''' <summary>
        ''' Use the simulated annealing improvement?
        ''' </summary>
        Public Property UseSimulatedAnnealing As Boolean
            Get
                Return _UseSimulatedAnnealing
            End Get
            Set(value As Boolean)
                _UseSimulatedAnnealing = value
                My.Settings.NMAFit_UseSimulatedAnnealing = value
                My.Settings.Save()
            End Set
        End Property

        Private _SimulatedAnnealingStartTemperature As Double = DefaultAnnealingStartTemperature
        ''' <summary>
        ''' Start-Temperature used for simulated annealing correction.
        ''' </summary>
        Public Property SimulatedAnnealingStartTemperature As Double
            Get
                Return _SimulatedAnnealingStartTemperature
            End Get
            Set(value As Double)
                _SimulatedAnnealingStartTemperature = value
                My.Settings.NMAFit_AnnealingTemp = value
                My.Settings.Save()
            End Set
        End Property

        Private _SimulatedAnnealingSteps As Integer = DefaultAnnealingSteps
        ''' <summary>
        ''' Maximum steps performed to reduce the effective annealing temperature.
        ''' </summary>
        Public Property SimulatedAnnealingSteps As Integer
            Get
                Return _SimulatedAnnealingSteps
            End Get
            Set(value As Integer)
                _SimulatedAnnealingSteps = value
                My.Settings.NMAFit_AnnealingSteps = value
                My.Settings.Save()
            End Set
        End Property

        ''' <summary>
        ''' Types of temperature reduction during simulated annealing!
        ''' </summary>
        Public Enum SimulatedAnnealingCoolingTypes
            Linear
            Exponential
        End Enum

        Private _SimulatedAnnealingCoolingType As SimulatedAnnealingCoolingTypes = DefaultSimulatedAnnealingCoolingType
        ''' <summary>
        ''' Type of cooling used for the Simulated Annealing Procedure.
        ''' </summary>
        Public Property SimulatedAnnealingCoolingType As SimulatedAnnealingCoolingTypes
            Get
                Return _SimulatedAnnealingCoolingType
            End Get
            Set(value As SimulatedAnnealingCoolingTypes)
                _SimulatedAnnealingCoolingType = value
                My.Settings.NMAFit_AnnealingCoolingType = value
                My.Settings.Save()
            End Set
        End Property

        ''' <summary>
        ''' Echo the Settings
        ''' </summary>
        Public Function EchoSettings() As String Implements iFitProcedureSettings.EchoSettings
            Dim SB As New System.Text.StringBuilder
            SB.AppendLine("maximum iteration count: " & Me.MaxIteration.ToString("N0"))
            SB.AppendLine("min change in Chi^2:     " & Me.MinDeltaChi2.ToString("E3"))
            'SB.AppendLine("use adaptive NM-factors: " & Me.UseAdaptiveNelderMeadFactors)
            SB.AppendLine("use simulated annealing: " & Me.UseSimulatedAnnealing)
            SB.AppendLine("| simulated annealing temperature:   " & Me.SimulatedAnnealingStartTemperature.ToString("E3"))
            SB.AppendLine("| simulated annealing cooling steps: " & Me.SimulatedAnnealingSteps.ToString("N0"))
            SB.Append("| simulated annealing cooling type:  " & Me.SimulatedAnnealingCoolingType.ToString)
            Return SB.ToString
        End Function

        ''' <summary>
        ''' Dynamic Nelder-Mead simplex factor adaption to the dimension of the problem.
        ''' </summary>
        Public Property UseAdaptiveNelderMeadFactors As Boolean = DefaultUseAdaptiveNelderMeadFactors

        ''' <summary>
        ''' Load settings, if present!
        ''' </summary>
        Public Sub New()
            Me.UseSimulatedAnnealing = My.Settings.NMAFit_UseSimulatedAnnealing
            If My.Settings.NMAFit_AnnealingTemp >= 0 Then Me.SimulatedAnnealingStartTemperature = My.Settings.NMAFit_AnnealingTemp
            If My.Settings.NMAFit_AnnealingSteps >= 0 Then Me.SimulatedAnnealingSteps = My.Settings.NMAFit_AnnealingSteps
            If My.Settings.NMAFit_AnnealingCoolingType = SimulatedAnnealingCoolingTypes.Exponential Then Me.SimulatedAnnealingCoolingType = SimulatedAnnealingCoolingTypes.Exponential
            If My.Settings.NMAFit_AnnealingCoolingType = SimulatedAnnealingCoolingTypes.Linear Then Me.SimulatedAnnealingCoolingType = SimulatedAnnealingCoolingTypes.Linear
            If My.Settings.NMAFit_MaxIterations > 0 Then Me.MaxIteration = My.Settings.NMAFit_MaxIterations
            If My.Settings.NMAFit_MinChi2 > 0 Then Me.MinDeltaChi2 = My.Settings.NMAFit_MinChi2
        End Sub
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
            Dim SP As New cFitProcedureSettingsPanel_NMA
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
    Private alpha As MathNet.Numerics.LinearAlgebra.Generic.Matrix(Of Double)

    ''' <summary>
    ''' Gradient vector
    ''' </summary>
    Private beta As Double()

    ''' <summary>
    ''' FitParameter Step
    ''' </summary>
    Private da As Double()

    ''' <summary>
    ''' temporary calculated Chi2 value
    ''' </summary>
    Private _Chi2 As Double
    Private lambda As Double

    Private IncrementedChi2 As Double
    Private IterationCount As Integer
#End Region

#Region "Stop Conditions"

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
        SimulatedAnnealingFinished
    End Enum

    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Public Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String Implements iFitProcedure.ConvertFitStopCodeToMessage
        Select Case FitStopCode
            Case cNMAFit.FitStopReason.FitConverged
                Return My.Resources.rNMAFit.FitStopReason_FitConverged.Replace("#d", Me.ThisFitProcedureSettings.MinDeltaChi2.ToString("E3"))
            Case cNMAFit.FitStopReason.MaxIterationsReached
                Return My.Resources.rNMAFit.FitStopReason_MaxIterationsReached.Replace("#d", Me.ThisFitProcedureSettings.MaxIteration.ToString("N0"))
            Case cNMAFit.FitStopReason.UserAborted
                Return My.Resources.rNMAFit.FitStopReason_UserAborted
            Case cNMAFit.FitStopReason.SimulatedAnnealingFinished
                Return My.Resources.rNMAFit.FitStopReason_AnnealingFinished.Replace("#d", Me.ThisFitProcedureSettings.SimulatedAnnealingSteps.ToString("N0"))
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
    ''' Constructor of the Nelder-Mead Downhill-Simplex (NMA)-Algorithm
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
            Return My.Resources.rNMAFit.NMA_Name
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
    ''' <returns></returns>
    Protected Overridable Function StopCondition() As FitStopReason

        ' Check for convergence of the Fit.
        'If System.Math.Abs(_Chi2 - IncrementedChi2) < Me.ThisFitProcedureSettings.MinDeltaChi2 Then
        '    Return FitStopReason.FitConverged
        'End If

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
    ''' Actual Fit-Function.
    ''' uses those values. The stop condition is fetched from <code>this.stop()</code>.
    ''' Override <code>this.stop()</code> if you want to use another stop condition.
    ''' </summary>
    Protected Sub _Fit() Handles FitThreadWorker.DoWork
        Me._FitStartTime = Now
        IterationCount = 0

        ' Extract Fit-Parameters
        Me._FitParameters = Me.FitFunction.FitParameters

        ' Count and extract all non-fixed Fit-Parameter-Keys
        Me.NonFixedFitParameterKeys = sFitParameter.GetNonFixedFitParameterKeys(FitParameters)
        Me.NonFixedFitParameterCount = Me.NonFixedFitParameterKeys.Count

        Dim CurrentParameterIndex As Integer ' temporary variable

        ' Saves the best Chi2 reached so far!
        Dim BestChi2 As Double = Double.MaxValue

        '// ** Dimension des Fits, d.h. Anzahl aktiver Parameter ermitteln; **
        '// ** Sofort beenden, wenn kein Fitparameter frei ist; **
        Dim ndim As Integer = Me.NonFixedFitParameterCount

        ' RaiseEvent
        RaiseEvent FitEcho("Initializing ... ")

        ' Nelder-Mead Expansion, Contraction and Reflection Factors.
        Dim NMalpha As Double
        Dim NMbeta As Double
        Dim NMgamma As Double
        Dim NMdelta As Double

        ' Set the NM-Factors static, or depending on the dimension of the problem.
        ' http://www.webpages.uidaho.edu/~fuchang/res/ANMS.pdf
        If Me.ThisFitProcedureSettings.UseAdaptiveNelderMeadFactors And ndim > 2 Then
            NMalpha = -1.0
            NMbeta = 1.0 + 2.0 / ndim
            NMgamma = 0.75 - 0.5 / ndim
            NMdelta = 0.5 - 1.0 / ndim
        Else
            NMalpha = -1.0
            NMbeta = 2.0
            NMgamma = 0.5
            NMdelta = 0.5
        End If

        '// ** Der Simplex (= die Amoebe) erzeugen; **
        Dim ParameterSimplex(ndim) As Dictionary(Of Integer, sFitParameter)
        For k As Integer = 0 To Me.NonFixedFitParameterCount Step 1
            ParameterSimplex(k) = New Dictionary(Of Integer, sFitParameter)(Me._FitParameters.Count)
        Next

        '// ** Die Startwerte des Simplex setzen; **
        '// ** Zunaechst: Jeden Eckpunkt auf denselben (Start-)Wert; **
        For Each FitParameterKV As KeyValuePair(Of Integer, sFitParameter) In Me._FitParameters
            For k As Integer = 0 To ndim Step 1
                ParameterSimplex(k).Add(FitParameterKV.Key, FitParameterKV.Value)
            Next
        Next

        '// ** Bei ndim - 1 Eckpunkten je einen aktiven Parameter auslenken; **
        '// ** Inaktive Parameter bleiben bei allen gleich; **
        Dim i As Integer = 1
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
            If Me._FitParameters(CurrentParameterIndex).Value <> 0.0 Then
                ParameterSimplex(i)(CurrentParameterIndex) = ParameterSimplex(i)(CurrentParameterIndex).ChangeValue(ParameterSimplex(i)(CurrentParameterIndex).Value * 1.1)
            Else
                ParameterSimplex(i)(CurrentParameterIndex) = ParameterSimplex(i)(CurrentParameterIndex).ChangeValue(0.1)
            End If
            i += 1
        Next

        '// ** Speicher fuer den besten jemals gefundenen Punkt und seinen Funktionswert reservieren; **
        Dim BestParameterSimplex As New Dictionary(Of Integer, sFitParameter)(ParameterSimplex(0).Count)
        Dim BestParameterSimplexChiSq As Double

        '// ** Setzte den besten jemals gefundenen Punkt zunaechst auf den Startpunkt; **
        For Each FitParameterKV As KeyValuePair(Of Integer, sFitParameter) In ParameterSimplex(0)
            BestParameterSimplex.Add(FitParameterKV.Key, FitParameterKV.Value)
        Next
        BestParameterSimplexChiSq = Me.CalculateChi2(BestParameterSimplex)

        '// ** Deklaration der in der Iteration benötigten Variablen; **
        Dim ValuesForEachSimplex(ndim) As Double
        Dim CalculationDoneForEachSimplex(ndim) As Boolean
        Array.Clear(CalculationDoneForEachSimplex, 0, CalculationDoneForEachSimplex.Length)

        ' Create variables to save the indices of the low- and high-points
        Dim iHighPointIndex, iSecondHighPointIndex, iLowPointIndex As Integer

        ' Create the array for calculation of the bary-center-point
        Dim BaryCenterPoint(Me.NonFixedFitParameterCount) As Double

        ' Used for testing for a better point
        Dim dTestValue As Double

        ' Array to save the thermal values of the simulated annealing
        Dim ThermalValuesForEachSimplex(ndim) As Double

        ' Create the Random generator used for simulated annealing
        Dim Rand As New Random(Now.Millisecond)

        ' Save the start-temperature
        Dim T As Double = Me.ThisFitProcedureSettings.SimulatedAnnealingStartTemperature

        ' reset the stop event
        Me.StopReason = FitStopReason.None

        While Me.StopReason = FitStopReason.None
            '// ** Funktionswerte an jedem Simplexpunkt bestimmen; **
            For p As Integer = 0 To ndim Step 1
                If Not CalculationDoneForEachSimplex(p) Then
                    ValuesForEachSimplex(p) = Me.CalculateChi2(ParameterSimplex(p))
                    CalculationDoneForEachSimplex(p) = True

                    If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                        '// ** Wenn dies der beste jemals gefundene Wert ist: Sich als solchen merken; **
                        If ValuesForEachSimplex(p) < BestParameterSimplexChiSq Then
                            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                                BestParameterSimplex(CurrentParameterIndex) = ParameterSimplex(p)(CurrentParameterIndex)
                            Next
                            BestParameterSimplexChiSq = ValuesForEachSimplex(p)
                        End If
                    End If
                End If

                If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                    '// ** Wert "thermisch verfaelscht" (siehe Numerical Recipes) speichern; **
                    ThermalValuesForEachSimplex(p) = ValuesForEachSimplex(p) - T * Math.Log((Rand.NextDouble + 1) / 2)
                End If
            Next

            '// ** Den Hoch- und Tiefpunkt finden; **
            iHighPointIndex = 0
            iLowPointIndex = 0
            For p As Integer = 0 To ndim Step 1
                If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                    If ThermalValuesForEachSimplex(p) > ThermalValuesForEachSimplex(iHighPointIndex) Then iHighPointIndex = p
                    If ThermalValuesForEachSimplex(p) < ThermalValuesForEachSimplex(iLowPointIndex) Then iLowPointIndex = p
                Else
                    If ValuesForEachSimplex(p) > ValuesForEachSimplex(iHighPointIndex) Then iHighPointIndex = p
                    If ValuesForEachSimplex(p) < ValuesForEachSimplex(iLowPointIndex) Then iLowPointIndex = p
                End If
            Next

            '// ** Den zweithöchsten Punkt finden; **
            iSecondHighPointIndex = 0
            For p As Integer = 0 To ndim Step 1
                If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                    If p <> iHighPointIndex And ThermalValuesForEachSimplex(p) > ThermalValuesForEachSimplex(iSecondHighPointIndex) Then iSecondHighPointIndex = p
                Else
                    If p <> iHighPointIndex And ValuesForEachSimplex(p) > ValuesForEachSimplex(iSecondHighPointIndex) Then iSecondHighPointIndex = p
                End If
            Next

            '// ** 1. Abbruchbedingung: Funktionsimplementierungsabhaengig; **
            Me.StopReason = Me.StopCondition

            '// ** 2. Abbruchbedingung: Simplex liegt nahezu flach da; **
            If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                If Math.Abs(ThermalValuesForEachSimplex(iHighPointIndex) - ThermalValuesForEachSimplex(iLowPointIndex)) / (Math.Abs(ThermalValuesForEachSimplex(iHighPointIndex)) + Math.Abs(ThermalValuesForEachSimplex(iLowPointIndex)) + cConstants.Epsilon) < Me.ThisFitProcedureSettings.MinDeltaChi2 Then
                    Me.StopReason = FitStopReason.FitConverged
                End If
                If IterationCount >= Me.ThisFitProcedureSettings.SimulatedAnnealingSteps Then
                    Me.StopReason = FitStopReason.SimulatedAnnealingFinished
                End If
            Else
                If Math.Abs(ValuesForEachSimplex(iHighPointIndex) - ValuesForEachSimplex(iLowPointIndex)) / (Math.Abs(ValuesForEachSimplex(iHighPointIndex)) + Math.Abs(ValuesForEachSimplex(iLowPointIndex)) + cConstants.Epsilon) < Me.ThisFitProcedureSettings.MinDeltaChi2 Then
                    Me.StopReason = FitStopReason.FitConverged
                End If
            End If

            ' Exit While!
            If Me.StopReason <> FitStopReason.None Then
                Exit While
            End If

            '// ** Berechne den Schwerpunkt der Gegenfläche des Hochpunktes; **
            '// ** Schwerpunkt von D Vektoren V_i = ( sum_{i=0}^{D-1} V_i ) / D; **
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                BaryCenterPoint(d) = 0
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                For p As Integer = 0 To ndim Step 1
                    If p <> iHighPointIndex Then BaryCenterPoint(d) += ParameterSimplex(p)(CurrentParameterIndex).Value
                Next
                BaryCenterPoint(d) /= ndim
            Next

            ' Berechne Spiegelpunkt vom Hochpunkt durch den Schwerpunkt des Gegendreiecks und tauscht die beiden
            ' Punkte, falls der Funktionswert des neuen Punktes kleiner ist als der des alten Hochpunktes;
            If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                dTestValue = Me.SimulatedAnnealingAmotry(ParameterSimplex(iHighPointIndex),
                                                         ValuesForEachSimplex(iHighPointIndex),
                                                         ThermalValuesForEachSimplex(iHighPointIndex),
                                                         BestParameterSimplex,
                                                         BestParameterSimplexChiSq,
                                                         BaryCenterPoint,
                                                         NMalpha,
                                                         T,
                                                         Rand.NextDouble)
            Else
                dTestValue = Me.Amotry(ParameterSimplex(iHighPointIndex),
                                       ValuesForEachSimplex(iHighPointIndex),
                                       BaryCenterPoint,
                                       NMalpha)
            End If


            If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                ' SIMULATED ANNEALING -> Use Thermal values!!!
                '###############################################

                '// ** Ist der Funktionswert des neuen Punktes sogar niedriger als der des Tiefpunktes: Versuche doppelt so **
                '// ** weit in diese Richtung zu gehen; **
                If dTestValue <= ThermalValuesForEachSimplex(iLowPointIndex) Then
                    Me.SimulatedAnnealingAmotry(ParameterSimplex(iHighPointIndex),
                                                ValuesForEachSimplex(iHighPointIndex),
                                                ThermalValuesForEachSimplex(iHighPointIndex),
                                                BestParameterSimplex,
                                                BestParameterSimplexChiSq,
                                                BaryCenterPoint,
                                                NMbeta,
                                                T,
                                                Rand.NextDouble)
                Else
                    '// ** => Der neue Funktionswert ist (a) entweder immer noch der alte Hochpunkt oder **
                    '// **                               (b) zwar neu, im Funktionswert aber trotzdem nicht besser als der Tiefpunkt; **

                    '// ** Wenn der neue Funktionswert schlechter ist als der des zweithöchsten Punktes (d.h. der Simplex in seiner **
                    '// ** Situation durch Bewegung nicht verbessert hat) nur halb so weit in Richtung des Schwerpunktes zu gehen; **
                    If dTestValue >= ThermalValuesForEachSimplex(iSecondHighPointIndex) Then
                        Dim dSaveValue As Double = dTestValue
                        dTestValue = Me.SimulatedAnnealingAmotry(ParameterSimplex(iHighPointIndex),
                                                                 ValuesForEachSimplex(iHighPointIndex),
                                                                 ThermalValuesForEachSimplex(iHighPointIndex),
                                                                 BestParameterSimplex,
                                                                 BestParameterSimplexChiSq,
                                                                 BaryCenterPoint,
                                                                 NMgamma,
                                                                 T,
                                                                 Rand.NextDouble)
                        '// ** Wenn dieser Schritt nichts verbessert: Kontrahiere alle Punkte in Richtung des Tiefpunktes, um **
                        '// ** eventuell durch eine Engstelle in der Funktionswertelandschaft schluepfen zu koennen; **
                        If dTestValue >= dSaveValue Then
                            For p As Integer = 0 To ndim Step 1
                                If p <> iLowPointIndex Then
                                    For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                        CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                                        ParameterSimplex(p)(CurrentParameterIndex) = ParameterSimplex(p)(CurrentParameterIndex).ChangeValue((ParameterSimplex(p)(CurrentParameterIndex).Value + ParameterSimplex(iLowPointIndex)(CurrentParameterIndex).Value) * NMdelta)
                                    Next
                                    CalculationDoneForEachSimplex(p) = False
                                End If
                            Next
                        End If
                    End If
                End If

                '// ** Die Temperatur reduzieren; **
                Select Case Me.ThisFitProcedureSettings.SimulatedAnnealingCoolingType
                    Case cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Linear
                        T = Me.ThisFitProcedureSettings.SimulatedAnnealingStartTemperature * (1.0 - Me.Iterations / Me.ThisFitProcedureSettings.SimulatedAnnealingSteps)
                    Case cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Exponential
                        T = Me.ThisFitProcedureSettings.SimulatedAnnealingStartTemperature * Math.Exp(-Me.Iterations * 6 / Me.ThisFitProcedureSettings.SimulatedAnnealingSteps)
                End Select

                ' // ** Zwischenergebniss-Ausgabe; **
                If BestParameterSimplexChiSq < BestChi2 Then
                    RaiseEvent FitStepEcho(BestParameterSimplex, BestParameterSimplexChiSq) ' _Chi2)
                    BestChi2 = BestParameterSimplexChiSq
                End If
            Else
                ' Use simplex values without simulated annealing!!!
                '####################################################

                '// ** Ist der Funktionswert des neuen Punktes sogar niedriger als der des Tiefpunktes: Versuche doppelt so **
                '// ** weit in diese Richtung zu gehen; **
                If dTestValue <= ValuesForEachSimplex(iLowPointIndex) Then
                    Me.Amotry(ParameterSimplex(iHighPointIndex),
                              ValuesForEachSimplex(iHighPointIndex),
                              BaryCenterPoint,
                              NMbeta)
                Else
                    '// ** => Der neue Funktionswert ist (a) entweder immer noch der alte Hochpunkt oder **
                    '// **                               (b) zwar neu, im Funktionswert aber trotzdem nicht besser als der Tiefpunkt; **

                    '// ** Wenn der neue Funktionswert schlechter ist als der des zweithöchsten Punktes (d.h. der Simplex in seiner **
                    '// ** Situation durch Bewegung nicht verbessert hat) nur halb so weit in Richtung des Schwerpunktes zu gehen; **
                    If dTestValue >= ValuesForEachSimplex(iSecondHighPointIndex) Then
                        Dim dSaveValue As Double = dTestValue
                        dTestValue = Me.Amotry(ParameterSimplex(iHighPointIndex),
                                               ValuesForEachSimplex(iHighPointIndex),
                                               BaryCenterPoint, NMgamma)

                        '// ** Wenn dieser Schritt nichts verbessert: Kontrahiere alle Punkte in Richtung des Tiefpunktes, um **
                        '// ** eventuell durch eine Engstelle in der Funktionswertelandschaft schluepfen zu koennen; **
                        If dTestValue >= dSaveValue Then
                            For p As Integer = 0 To ndim Step 1
                                If p <> iLowPointIndex Then
                                    For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                        CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                                        ParameterSimplex(p)(CurrentParameterIndex) = ParameterSimplex(p)(CurrentParameterIndex).ChangeValue((ParameterSimplex(p)(CurrentParameterIndex).Value + ParameterSimplex(iLowPointIndex)(CurrentParameterIndex).Value) * NMdelta)
                                    Next
                                    CalculationDoneForEachSimplex(p) = False
                                End If
                            Next
                        End If
                    End If
                End If

                ' // ** Zwischenergebniss-Ausgabe; **
                If ValuesForEachSimplex(iLowPointIndex) < BestChi2 Then
                    RaiseEvent FitStepEcho(ParameterSimplex(iLowPointIndex), ValuesForEachSimplex(iLowPointIndex)) ' _Chi2)
                    BestChi2 = ValuesForEachSimplex(iLowPointIndex)
                End If

            End If

            ' Increase Iteration Counter
            IterationCount += 1
        End While

        If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
            '  // ** => Minimierung beendet; Uebernahme des niedrigsten Simplexpunktes als neue Parameter; **
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                Me._FitParameters(CurrentParameterIndex) = BestParameterSimplex(CurrentParameterIndex)
            Next
        Else
            '  // ** => Minimierung beendet; Uebernahme des niedrigsten Simplexpunktes als neue Parameter; **
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                Me._FitParameters(CurrentParameterIndex) = ParameterSimplex(iLowPointIndex)(CurrentParameterIndex)
            Next
        End If

        ' Create the data using the fit-model as third column in the datapoint-array
        Dim CalculatedDataPoints(DataPoints(0).Length - 1) As Double
        For k As Integer = 0 To DataPoints(0).Length - 1 Step 1
            CalculatedDataPoints(k) = Me.FitFunction.GetY(DataPoints(0)(k), Me.FitParameters)
        Next

        ' Calculate the final numeric statistics
        Me._FinalStatistics = cNumericalMethods.Statistics1D(CalculatedDataPoints, DataPoints(1))

        ' Loop ended, so fit finished!!!
        Me._FitEndTime = Now

        ' Raise the event to the listeners!
        RaiseEvent FitFinished(StopReason, _FitParameters, _Chi2)
    End Sub

    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet  den Testpunkt d_tp,  der an der Stelle  liegt, die *
    ''' // * man erhaelt, wenn man d_hip  (konkret: der Hochpunkt) an d_sp *
    ''' // * (konkret: der  Schwerpunkt  der  Gegenseite  des Hochpunktes) *
    ''' // * mit dem Faktor d_streck reflektiert, (d.h. fuer d_streck = -1 *
    ''' // * Spiegelt, fuer d_streck = 1 den Hochpunkt beibehaelt usw.).   *
    ''' // * Testet dann,  ob Funktionswert des erhaltenen Punktes kleiner *
    ''' // * ist als der des Hochpunktes DER IN p_hiVal IM ORIGINAL UEBER- *
    ''' // * GEBEN WERDEN MUSS (zur Rechenzeitoptimierung) und tauscht die *
    ''' // * beiden Punkte in diesem Fall aus.                             *
    ''' // * Zur Berechnung des Funktionswertes von d_tp muessen zudem die *
    ''' // * Variablen  p_mx, p_my, p_myErr, i_mstart, i_mend  mit  ueber- *
    ''' // * geben werden. Siehe hierzu die Beschreibung von ChiSquare().  *
    ''' // *****************************************************************
    ''' 
    ''' double CFitproject::Amotry( double* d_hip, double* p_hiVal, double* d_sp, double d_streck,
    '''                             double* p_mx, double* p_my, double* p_myErr, int i_mstart, int i_mend )
    ''' </summary>
    Private Function Amotry(ByRef HighPointSimplex As Dictionary(Of Integer, sFitParameter),
                            ByRef HighPointValue As Double,
                            ByRef BaryCenterPoint As Double(),
                            ByRef StretchFactor As Double) As Double

        '// ** Berechne den zu testenden Simplex-Punkt und seinen Funktionswert; **
        Dim TestSimplex As New Dictionary(Of Integer, sFitParameter)(HighPointSimplex.Count)

        ' Fill the TestSimplex with the High-Point Values to set first
        ' also all fixed parameters!
        For Each FitParameterKV As KeyValuePair(Of Integer, sFitParameter) In HighPointSimplex
            TestSimplex.Add(FitParameterKV.Key, FitParameterKV.Value)
        Next

        Dim CurrentParameterIndex As Integer

        ' Calculate the non-fixed parameters!
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
            Dim NewValue As Double = BaryCenterPoint(d) + StretchFactor * (HighPointSimplex(CurrentParameterIndex).Value - BaryCenterPoint(d))
            TestSimplex(CurrentParameterIndex) = TestSimplex(CurrentParameterIndex).ChangeValue(NewValue)
        Next

        ' Calculate the value of Chi2 for this test-point.
        Dim TestSimplexValue As Double = Me.CalculateChi2(TestSimplex)

        '// ** Prüfen, ob der Testpunkt besser, d.h. sein Funktionswert kleiner als der des Hochpunktes ist; **
        If TestSimplexValue < HighPointValue Then
            '// ** => Dann tritt der Testpunkt an die Stelle des Hochpunktes; Ueberschreibe Punkt und Funktionswert; **
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                HighPointSimplex(CurrentParameterIndex) = TestSimplex(CurrentParameterIndex)
            Next
            HighPointValue = TestSimplexValue
        End If

        '// ** Rückgabe: Funktionswert des neuen Punktes oder des erhaltenen (Hoch-)Punktes; **
        Return HighPointValue
    End Function

    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet  den Testpunkt d_tp,  der an der Stelle  liegt, die *
    ''' // * man erhaelt, wenn man d_hip  (konkret: der Hochpunkt) an d_sp *
    ''' // * (konkret: der  Schwerpunkt  der  Gegenseite  des Hochpunktes) *
    ''' // * mit dem Faktor d_streck reflektiert, (d.h. fuer d_streck = -1 *
    ''' // * Spiegelt, fuer d_streck = 1 den Hochpunkt beibehaelt usw.).   *
    ''' // * Testet dann,  ob Funktionswert des erhaltenen Punktes kleiner *
    ''' // * ist als der des Hochpunktes DER IN p_hiVal IM ORIGINAL UEBER- *
    ''' // * GEBEN WERDEN MUSS (zur Rechenzeitoptimierung) und tauscht die *
    ''' // * beiden Punkte in diesem Fall aus.                             *
    ''' // * Zur Berechnung des Funktionswertes von d_tp muessen zudem die *
    ''' // * Variablen  p_mx, p_my, p_myErr, i_mstart, i_mend  mit  ueber- *
    ''' // * geben werden. Siehe hierzu die Beschreibung von ChiSquare().  *
    ''' // *****************************************************************
    ''' 
    ''' double CFitproject::Amotry( double* d_hip, double* p_hiVal, double* d_sp, double d_streck,
    '''                             double* p_mx, double* p_my, double* p_myErr, int i_mstart, int i_mend )
    ''' </summary>
    Private Function SimulatedAnnealingAmotry(ByRef HighPointSimplex As Dictionary(Of Integer, sFitParameter),
                                              ByRef HighPointValue As Double,
                                              ByRef HighPointThermalValue As Double,
                                              ByRef BestParameterSimplex As Dictionary(Of Integer, sFitParameter),
                                              ByRef BestParameterSimplexChiSq As Double,
                                              ByRef BaryCenterPoint As Double(),
                                              ByRef StretchFactor As Double,
                                              ByRef Temperature As Double,
                                              ByVal Rand As Double) As Double

        '// ** Berechne den zu testenden Simplex-Punkt und seinen Funktionswert; **
        Dim TestSimplex As New Dictionary(Of Integer, sFitParameter)(HighPointSimplex.Count)

        ' Fill the TestSimplex with the High-Point Values to set first
        ' also all fixed parameters!
        For Each FitParameterKV As KeyValuePair(Of Integer, sFitParameter) In HighPointSimplex
            TestSimplex.Add(FitParameterKV.Key, FitParameterKV.Value)
        Next

        Dim CurrentParameterIndex As Integer

        ' Calculate the non-fixed parameters!
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
            Dim NewValue As Double = BaryCenterPoint(d) + StretchFactor * (HighPointSimplex(CurrentParameterIndex).Value - BaryCenterPoint(d))
            TestSimplex(CurrentParameterIndex) = TestSimplex(CurrentParameterIndex).ChangeValue(NewValue)
        Next

        ' Calculate the test-value of Chi^2 for this test-point.
        Dim TestSimplexValue As Double = Me.CalculateChi2(TestSimplex)

        '// ** Einen moeglicherweise besten Funktionswert speichern; **
        If TestSimplexValue < BestParameterSimplexChiSq Then
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                BestParameterSimplex(CurrentParameterIndex) = TestSimplex(CurrentParameterIndex)
            Next
            BestParameterSimplexChiSq = TestSimplexValue
        End If

        '// ** Wert "thermisch verfaelscht" (siehe Numerical Recipes) speichern; **
        Dim TestSimplexThermalValue As Double = TestSimplexValue - Temperature * Math.Log((Rand + 1) / 2)

        '// ** Prüfen, ob der Testpunkt besser, d.h. sein Funktionswert kleiner als der des Hochpunktes ist; **
        If TestSimplexThermalValue < HighPointThermalValue Then
            '// ** => Dann tritt der Testpunkt an die Stelle des Hochpunktes; Ueberschreibe Punkt und Funktionswert; **
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                CurrentParameterIndex = Me.NonFixedFitParameterKeys(d)
                HighPointSimplex(CurrentParameterIndex) = TestSimplex(CurrentParameterIndex)
            Next
            HighPointValue = TestSimplexValue
            HighPointThermalValue = TestSimplexThermalValue
        End If

        '// ** Rückgabe: Funktionswert des neuen Punktes oder des erhaltenen (Hoch-)Punktes; **
        Return HighPointValue
    End Function

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
    ''' <returns></returns>
    Protected Function CalculateIncrementedChi2() As Double
        Return CalculateChi2(IncrementedParameters)
    End Function

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
