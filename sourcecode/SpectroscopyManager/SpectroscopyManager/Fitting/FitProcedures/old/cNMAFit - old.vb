Imports System.Diagnostics
Imports System.Threading
Imports System.ComponentModel
Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator

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
    Inherits cFitProcedure
    Implements iFitProcedure

    ' default fit procedure settings
    Private Const DefaultMinDeltaChi2 As Double = 0.00000001
    Private Const DefaultMaxIterations As Integer = 3000
    Private Const DefaultAnnealingStartTemperature As Double = 0.000000001
    Private Const DefaultAnnealingSteps As Integer = 500
    Private Const DefaultUseSimulatedAnnealing As Boolean = False
    Private Const DefaultSimulatedAnnealingCoolingType As cFitProcedureSettings.SimulatedAnnealingCoolingTypes = cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Exponential
    Private Const DefaultUseAdaptiveNelderMeadFactors As Boolean = False
    Private Const DefaultUseGPUComputingIfPossible As Boolean = False

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

        Private _UseGPUComputingIfPossible As Boolean = DefaultUseGPUComputingIfPossible
        ''' <summary>
        ''' Use the simulated annealing improvement?
        ''' </summary>
        Public Property UseGPUComputingIfPossible As Boolean
            Get
                Return _UseGPUComputingIfPossible
            End Get
            Set(value As Boolean)
                _UseGPUComputingIfPossible = value
                My.Settings.NMAFit_UseGPUComputingIfPossible = value
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
            SB.AppendLine("try using GPU computing: " & Me.UseGPUComputingIfPossible)
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
            Me.UseGPUComputingIfPossible = My.Settings.NMAFit_UseGPUComputingIfPossible
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
    Public Overrides Property FitProcedureSettings As iFitProcedureSettings
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
    Public Overrides ReadOnly Property ProcedureSettingPanel As cFitProcedureSettingsPanel
        Get
            Dim SP As New cFitProcedureSettingsPanel_NMA
            SP.FitProcedureSettings = Me.ThisFitProcedureSettings
            Return SP
        End Get
    End Property

#End Region

#Region "Parameters"
    ''' <summary>
    ''' Parameters incremented by value of lambda
    ''' </summary>
    Private IncrementedParameters As Dictionary(Of Integer, sFitParameter)

    ''' <summary>
    ''' Is CUDA initialized?
    ''' </summary>
    Protected bCudaInizialized As Boolean = False

    ''' <summary>
    ''' CUDA GPU used.
    ''' </summary>
    Protected CUDAGPU As GPGPU
#End Region

#Region "Stop-Reasons"
    ''' <summary>
    ''' Variable that saves the reason for the procedure to stop.
    ''' </summary>
    Private StopReason As FitStopReason

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
    Public Overrides Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String
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

#Region "Properties, such as name and description"
    ''' <summary>
    ''' Fit-Procedure-Name
    ''' </summary>
    Public Overrides ReadOnly Property Name As String
        Get
            Return My.Resources.rNMAFit.NMA_Name
        End Get
    End Property
#End Region

#Region "Stop Condition Control Function"
    ''' <summary>
    ''' The stop condition for the fit.
    ''' Override this if you want to use another stop condition.
    ''' </summary>
    ''' <returns></returns>
    Protected Overridable Function StopCondition() As FitStopReason

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
    Protected Overrides Function _Fit(ByRef InValues As Double(),
                                      ByRef InIdentifiers As Integer(),
                                      ByRef InFixed As Boolean(),
                                      ByRef InLockedTo As Integer(),
                                      ByRef InUpperBoundaries As Double(),
                                      ByRef InLowerBoundaries As Double()) As Integer

        ' temporary variable
        Dim CurrentParameterIndex As Integer

        ' Saves the best Chi2 reached so far!
        Dim BestChi2 As Double = Double.MaxValue

        '// ** Dimension des Fits, d.h. Anzahl aktiver Parameter ermitteln; **
        '// ** Sofort beenden, wenn kein Fitparameter frei ist; **
        Dim ndim As Integer = Me.NonFixedFitParameterCount

        ' RaiseEvent
        RaiseEvent_FitEcho("Initializing ... ")

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
        ' For each fitted parameter a set of parameters is created
        Dim ParameterSimplex(ndim)() As Double
        For k As Integer = 0 To Me.NonFixedFitParameterCount Step 1
            ParameterSimplex(k) = New Double(InIdentifiers.Count - 1) {}
        Next

        '// ** Die Startwerte des Simplex setzen; **
        '// ** Zunaechst: Jeden Eckpunkt auf denselben (Start-)Wert; **
        For k As Integer = 0 To Me.NonFixedFitParameterCount Step 1
            For j As Integer = 0 To InIdentifiers.Count - 1 Step 1
                ParameterSimplex(k)(j) = InValues(j)
            Next
        Next

        '// ** Bei ndim - 1 Eckpunkten je einen aktiven Parameter auslenken; **
        '// ** Inaktive Parameter bleiben bei allen gleich; **
        Dim i As Integer = 1
        For j As Integer = 0 To InIdentifiers.Count - 1 Step 1
            If Not InFixed(j) Then
                If InValues(j) <> 0 Then
                    ParameterSimplex(i)(j) = InValues(j) * 1.1
                Else
                    ParameterSimplex(i)(j) = 0.1
                End If
                i += 1
            End If
        Next

        ' Lock parameters together for all initial simplizes
        For j As Integer = 1 To Me.NonFixedFitParameterCount Step 1
            Me.LockParametersTogether(InIdentifiers, ParameterSimplex(j), InFixed, InLockedTo)
        Next

        '// ** Speicher fuer den besten jemals gefundenen Punkt und seinen Funktionswert reservieren; **
        Dim BestParameterSimplex(InIdentifiers.Count - 1) As Double
        Dim BestParameterSimplexChiSq As Double

        '// ** Setzte den besten jemals gefundenen Punkt zunaechst auf den Startpunkt; **
        'For Each FitParameterKV As KeyValuePair(Of Integer, sFitParameter) In ParameterSimplex(0)
        '    BestParameterSimplex.Add(FitParameterKV.Key, FitParameterKV.Value)
        'Next
        For j As Integer = 0 To InIdentifiers.Count - 1 Step 1
            BestParameterSimplex(j) = InValues(j)
        Next
        BestParameterSimplexChiSq = Me.CalculateChi2(InIdentifiers, BestParameterSimplex, InUpperBoundaries, InLowerBoundaries)

        '// ** Deklaration der in der Iteration benötigten Variablen; **
        Dim ValuesForEachSimplex(ndim) As Double
        Dim CalculationDoneForEachSimplex(ndim) As Boolean
        Array.Clear(CalculationDoneForEachSimplex, 0, CalculationDoneForEachSimplex.Length)

        ' Create variables to save the indices of the low- and high-points
        Dim iHighPointIndex, iSecondHighPointIndex, iLowPointIndex As Integer

        ' Create the array for calculation of the bary-center-point
        Dim BaryCenterPoint(InIdentifiers.Length - 1) As Double

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
                    ValuesForEachSimplex(p) = Me.CalculateChi2(InIdentifiers, ParameterSimplex(p), InUpperBoundaries, InLowerBoundaries)
                    CalculationDoneForEachSimplex(p) = True

                    If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                        '// ** Wenn dies der beste jemals gefundene Wert ist: Sich als solchen merken; **
                        If ValuesForEachSimplex(p) < BestParameterSimplexChiSq Then
                            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
                                BestParameterSimplex(CurrentParameterIndex) = ParameterSimplex(p)(CurrentParameterIndex)
                            Next
                            Me.LockParametersTogether(InIdentifiers, BestParameterSimplex, InFixed, InLockedTo)
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
                CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
                For p As Integer = 0 To ndim Step 1
                    If p <> iHighPointIndex Then BaryCenterPoint(d) += ParameterSimplex(p)(CurrentParameterIndex)
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
                                                         Rand.NextDouble,
                                                         InIdentifiers,
                                                         InUpperBoundaries,
                                                         InLowerBoundaries,
                                                         InLockedTo,
                                                         InFixed)
            Else
                dTestValue = Me.Amotry(ParameterSimplex(iHighPointIndex),
                                       ValuesForEachSimplex(iHighPointIndex),
                                       BaryCenterPoint,
                                       NMalpha,
                                       InIdentifiers,
                                       InUpperBoundaries,
                                       InLowerBoundaries,
                                       InLockedTo,
                                       InFixed)
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
                                                Rand.NextDouble,
                                                InIdentifiers,
                                                InUpperBoundaries,
                                                InLowerBoundaries,
                                                InLockedTo,
                                                InFixed)
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
                                                                 Rand.NextDouble,
                                                                 InIdentifiers,
                                                                 InUpperBoundaries,
                                                                 InLowerBoundaries,
                                                                 InLockedTo,
                                                                 InFixed)
                        '// ** Wenn dieser Schritt nichts verbessert: Kontrahiere alle Punkte in Richtung des Tiefpunktes, um **
                        '// ** eventuell durch eine Engstelle in der Funktionswertelandschaft schluepfen zu koennen; **
                        If dTestValue >= dSaveValue Then
                            For p As Integer = 0 To ndim Step 1
                                If p <> iLowPointIndex Then
                                    For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                        CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
                                        ParameterSimplex(p)(CurrentParameterIndex) = (ParameterSimplex(p)(CurrentParameterIndex) + ParameterSimplex(iLowPointIndex)(CurrentParameterIndex)) * NMdelta
                                    Next
                                    Me.LockParametersTogether(InIdentifiers, ParameterSimplex(p), InFixed, InLockedTo)
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
                    Me._FitParameters = sFitParameter.GetFitParameterDictionaryFromArrays(Me._FitParameters, InIdentifiers, BestParameterSimplex)
                    RaiseEvent_FitStepEcho(Me._FitParameters, BestParameterSimplexChiSq) ' _Chi2)
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
                              NMbeta,
                              InIdentifiers,
                              InUpperBoundaries,
                              InLowerBoundaries,
                              InLockedTo,
                              InFixed)
                Else
                    '// ** => Der neue Funktionswert ist (a) entweder immer noch der alte Hochpunkt oder **
                    '// **                               (b) zwar neu, im Funktionswert aber trotzdem nicht besser als der Tiefpunkt; **

                    '// ** Wenn der neue Funktionswert schlechter ist als der des zweithöchsten Punktes (d.h. der Simplex in seiner **
                    '// ** Situation durch Bewegung nicht verbessert hat) nur halb so weit in Richtung des Schwerpunktes zu gehen; **
                    If dTestValue >= ValuesForEachSimplex(iSecondHighPointIndex) Then
                        Dim dSaveValue As Double = dTestValue
                        dTestValue = Me.Amotry(ParameterSimplex(iHighPointIndex),
                                               ValuesForEachSimplex(iHighPointIndex),
                                               BaryCenterPoint, NMgamma,
                                               InIdentifiers,
                                               InUpperBoundaries,
                                               InLowerBoundaries,
                                               InLockedTo,
                                               InFixed)

                        '// ** Wenn dieser Schritt nichts verbessert: Kontrahiere alle Punkte in Richtung des Tiefpunktes, um **
                        '// ** eventuell durch eine Engstelle in der Funktionswertelandschaft schluepfen zu koennen; **
                        If dTestValue >= dSaveValue Then
                            For p As Integer = 0 To ndim Step 1
                                If p <> iLowPointIndex Then
                                    For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                        CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
                                        ParameterSimplex(p)(CurrentParameterIndex) = (ParameterSimplex(p)(CurrentParameterIndex) + ParameterSimplex(iLowPointIndex)(CurrentParameterIndex)) * NMdelta
                                    Next
                                    Me.LockParametersTogether(InIdentifiers, ParameterSimplex(p), InFixed, InLockedTo)
                                    CalculationDoneForEachSimplex(p) = False
                                End If
                            Next
                        End If
                    End If
                End If

                ' // ** Zwischenergebniss-Ausgabe; **
                If ValuesForEachSimplex(iLowPointIndex) < BestChi2 Then
                    Me._FitParameters = sFitParameter.GetFitParameterDictionaryFromArrays(Me._FitParameters, InIdentifiers, ParameterSimplex(iLowPointIndex))
                    RaiseEvent_FitStepEcho(Me._FitParameters, ValuesForEachSimplex(iLowPointIndex)) ' _Chi2)
                    BestChi2 = ValuesForEachSimplex(iLowPointIndex)
                End If

            End If

            ' Increase Iteration Counter
            IterationCount += 1
        End While

        If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
            '  // ** => Minimierung beendet; Uebernahme des besten jemals gefundenen Punktes als neue Parameter; **
            For d As Integer = 0 To InValues.Length - 1 Step 1
                InValues(d) = BestParameterSimplex(d)
            Next
        Else
            '  // ** => Minimierung beendet; Uebernahme des niedrigsten Simplexpunktes als neue Parameter; **
            For d As Integer = 0 To InValues.Length - 1 Step 1
                InValues(d) = ParameterSimplex(iLowPointIndex)(d)
            Next
        End If

        Return Me.StopReason
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
    Private Function Amotry(ByRef HighPointSimplex As Double(),
                            ByRef HighPointValue As Double,
                            ByRef BaryCenterPoint As Double(),
                            ByRef StretchFactor As Double,
                            ByRef InIdentifiers As Integer(),
                            ByRef InUpperBoundaries As Double(),
                            ByRef InLowerBoundaries As Double(),
                            ByRef InLockedTo As Integer(),
                            ByRef InFixed As Boolean()) As Double

        '// ** Berechne den zu testenden Simplex-Punkt und seinen Funktionswert; **
        Dim TestSimplex(HighPointSimplex.Length - 1) As Double

        ' Fill the TestSimplex with the High-Point Values to set first
        ' also all fixed parameters!
        For i As Integer = 0 To HighPointSimplex.Length - 1 Step 1
            TestSimplex(i) = HighPointSimplex(i)
        Next

        Dim CurrentParameterIndex As Integer

        ' Calculate the non-fixed parameters!
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
            Dim NewValue As Double = BaryCenterPoint(d) + StretchFactor * (HighPointSimplex(CurrentParameterIndex) - BaryCenterPoint(d))
            TestSimplex(CurrentParameterIndex) = NewValue
        Next
        Me.LockParametersTogether(InIdentifiers, TestSimplex, InFixed, InLockedTo)

        ' Calculate the value of Chi2 for this test-point.
        Dim TestSimplexValue As Double = Me.CalculateChi2(InIdentifiers, TestSimplex, InUpperBoundaries, InLowerBoundaries)

        '// ** Prüfen, ob der Testpunkt besser, d.h. sein Funktionswert kleiner als der des Hochpunktes ist; **
        If TestSimplexValue < HighPointValue Then
            '// ** => Dann tritt der Testpunkt an die Stelle des Hochpunktes; Ueberschreibe Punkt und Funktionswert; **
            For d As Integer = 0 To InIdentifiers.Length - 1 Step 1
                HighPointSimplex(d) = TestSimplex(d)
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
    Private Function SimulatedAnnealingAmotry(ByRef HighPointSimplex As Double(),
                                              ByRef HighPointValue As Double,
                                              ByRef HighPointThermalValue As Double,
                                              ByRef BestParameterSimplex As Double(),
                                              ByRef BestParameterSimplexChiSq As Double,
                                              ByRef BaryCenterPoint As Double(),
                                              ByRef StretchFactor As Double,
                                              ByRef Temperature As Double,
                                              ByVal Rand As Double,
                                              ByRef InIdentifiers As Integer(),
                                              ByRef InUpperBoundaries As Double(),
                                              ByRef InLowerBoundaries As Double(),
                                              ByRef InLockedTo As Integer(),
                                              ByRef InFixed As Boolean()) As Double

        '// ** Berechne den zu testenden Simplex-Punkt und seinen Funktionswert; **
        Dim TestSimplex(HighPointSimplex.Length - 1) As Double

        ' Fill the TestSimplex with the High-Point Values to set first
        ' also all fixed parameters!
        For i As Integer = 0 To HighPointSimplex.Length - 1 Step 1
            TestSimplex(i) = HighPointSimplex(i)
        Next

        Dim CurrentParameterIndex As Integer

        ' Calculate the non-fixed parameters!
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
            Dim NewValue As Double = BaryCenterPoint(d) + StretchFactor * (HighPointSimplex(CurrentParameterIndex) - BaryCenterPoint(d))
            TestSimplex(CurrentParameterIndex) = NewValue
        Next
        Me.LockParametersTogether(InIdentifiers, TestSimplex, InFixed, InLockedTo)

        ' Calculate the test-value of Chi^2 for this test-point.
        Dim TestSimplexValue As Double = Me.CalculateChi2(InIdentifiers, TestSimplex, InUpperBoundaries, InLowerBoundaries)

        '// ** Einen moeglicherweise besten Funktionswert speichern; **
        If TestSimplexValue < BestParameterSimplexChiSq Then
            For d As Integer = 0 To InIdentifiers.Length - 1 Step 1
                BestParameterSimplex(d) = TestSimplex(d)
            Next
            BestParameterSimplexChiSq = TestSimplexValue
        End If

        '// ** Wert "thermisch verfaelscht" (siehe Numerical Recipes) speichern; **
        Dim TestSimplexThermalValue As Double = TestSimplexValue - Temperature * Math.Log((Rand + 1) / 2)

        '// ** Prüfen, ob der Testpunkt besser, d.h. sein Funktionswert kleiner als der des Hochpunktes ist; **
        If TestSimplexThermalValue < HighPointThermalValue Then
            '// ** => Dann tritt der Testpunkt an die Stelle des Hochpunktes; Ueberschreibe Punkt und Funktionswert; **
            For d As Integer = 0 To InIdentifiers.Length - 1 Step 1
                HighPointSimplex(d) = TestSimplex(d)
            Next
            HighPointValue = TestSimplexValue
            HighPointThermalValue = TestSimplexThermalValue
        End If

        '// ** Rückgabe: Funktionswert des neuen Punktes oder des erhaltenen (Hoch-)Punktes; **
        Return HighPointValue
    End Function

#End Region


End Class
