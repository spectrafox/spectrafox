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

        Private _StopCondition_MinChi2Change As Double = DefaultMinDeltaChi2
        ''' <summary>
        ''' Sets the minimum change in Chi^2 for which to abort the fit
        ''' </summary>
        Public Property StopCondition_MinChi2Change As Double Implements iFitProcedureSettings.StopCondition_MinChi2Change
            Get
                Return _StopCondition_MinChi2Change
            End Get
            Set(value As Double)
                _StopCondition_MinChi2Change = value
                My.Settings.NMAFit_MinChi2 = value
                My.Settings.Save()
            End Set
        End Property

        Private _StopCondition_MaxIterations As Integer = DefaultMaxIterations
        ''' <summary>
        ''' Sets the maximum number of iterations, after which to abort the fit.
        ''' </summary>
        Public Property StopCondition_MaxIterations As Integer Implements iFitProcedureSettings.StopCondition_MaxIterations
            Get
                Return _StopCondition_MaxIterations
            End Get
            Set(value As Integer)
                _StopCondition_MaxIterations = value
                My.Settings.NMAFit_MaxIterations = value
                My.Settings.Save()
            End Set
        End Property

        ''' <summary>
        ''' Echo the Settings
        ''' </summary>
        Public Function EchoSettings() As String Implements iFitProcedureSettings.EchoSettings
            Dim SB As New System.Text.StringBuilder
            'SB.AppendLine("use adaptive NM-factors: " & Me.UseAdaptiveNelderMeadFactors)
            SB.AppendLine("try using GPU computing: " & Me.UseGPUComputingIfPossible)
            SB.AppendLine("use simulated annealing: " & Me.UseSimulatedAnnealing)
            If Me.UseSimulatedAnnealing Then
                SB.AppendLine("| simulated annealing temperature:   " & Me.SimulatedAnnealingStartTemperature.ToString("E3"))
                SB.AppendLine("| simulated annealing cooling steps: " & Me.SimulatedAnnealingSteps.ToString("N0"))
                SB.AppendLine("| simulated annealing cooling type:  " & Me.SimulatedAnnealingCoolingType.ToString)
            End If
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
            If My.Settings.NMAFit_MaxIterations > 0 Then Me._StopCondition_MaxIterations = My.Settings.NMAFit_MaxIterations
            If My.Settings.NMAFit_MinChi2 > 0 Then Me._StopCondition_MinChi2Change = My.Settings.NMAFit_MinChi2
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
    ''' Parameters incremented by value of lambda
    ''' </summary>
    Private IncrementedParameters As Dictionary(Of Integer, cFitParameter)

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
    ''' Reasons for the Fit-Procedure to end.
    ''' </summary>
    Public Enum FitStopReasons
        SimulatedAnnealingFinished
    End Enum

    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Public Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String Implements iFitProcedure.ConvertFitStopCodeToMessage
        Select Case FitStopCode
            Case cNMAFit.FitStopReasons.SimulatedAnnealingFinished
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
    Public ReadOnly Property Name As String Implements iFitProcedure.Name
        Get
            Return My.Resources.rNMAFit.NMA_Name
        End Get
    End Property
#End Region

#Region "Fit Procedure"

    ' Saves the best Chi2 reached so far!
    Protected BestChi2 As Double

    '// ** Dimension des Fits, d.h. Anzahl aktiver Parameter ermitteln; **
    Protected ndim As Integer

    '// ** Der Simplex (= die Amoebe) erzeugen; **
    ' For each fitted parameter a set of parameters is created
    Protected ParameterSimplex As Double()()

    ' Nelder-Mead Expansion, Contraction and Reflection Factors.
    Protected NMalpha As Double
    Protected NMbeta As Double
    Protected NMgamma As Double
    Protected NMdelta As Double

    ' Fit Function used for the fit
    Protected FitFunction As iFitFunction

    ' Create temporary arrays used 
    Protected OldValues As Double()

    '// ** Speicher fuer den besten jemals gefundenen Punkt und seinen Funktionswert reservieren; **
    Protected BestParameterSimplex As Double()
    Protected BestParameterSimplexChiSq As Double

    '// ** Deklaration der in der Iteration benötigten Variablen; **
    Protected ValuesForEachSimplex As Double()
    Protected CalculationDoneForEachSimplex As Boolean()

    ' Create variables to save the indices of the low- and high-points
    Protected iHighPointIndex, iSecondHighPointIndex, iLowPointIndex As Integer

    ' Create the array for calculation of the bary-center-point
    Protected BaryCenterPoint As Double()

    ' Used for testing for a better point
    Protected dTestValue As Double

    ' Array to save the thermal values of the simulated annealing
    Protected ThermalValuesForEachSimplex As Double()

    ' Create the Random generator used for simulated annealing
    Protected Rand As Random

    ' Save the start-temperature
    Protected T As Double

    ' storage for the non-fixed parameter info
    Protected ParameterCount As Integer
    Protected NonFixedFitParameterCount As Integer
    Protected NonFixedFitParameterIndices As String()

    ''' <summary>
    ''' FitInizializer, defines the initial set of variables,
    ''' e.g. adapts the length of the arrays to the datapoints, etc.
    ''' </summary>
    Protected Sub FitInitializer(ByRef ModelFitFunction As iFitFunction,
                                 ByRef FitDataPoints As Double()(),
                                 ByRef Weights As Double()) Implements iFitProcedure.FitInitializer

        ' Save the fit-function
        Me.FitFunction = ModelFitFunction

        ' Start Random Generator
        Rand = New Random(Now.Millisecond)

        ' Saves the best Chi2 reached so far!
        Me.BestChi2 = Double.MaxValue

        '// ** Dimension des Fits, d.h. Anzahl aktiver Parameter ermitteln; **
        '// ** Sofort beenden, wenn kein Fitparameter frei ist; **
        Me.NonFixedFitParameterIndices = ModelFitFunction.FitParameters.GetNonFixedInternalIdentifiers
        Me.NonFixedFitParameterCount = Me.NonFixedFitParameterIndices.Length
        Me.ndim = Me.NonFixedFitParameterCount
        Me.ParameterCount = ModelFitFunction.FitParameters.Count

        ' Create storage for the old parameter values
        ReDim OldValues(ModelFitFunction.FitParameters.Count - 1)

        ' Nelder-Mead Expansion, Contraction and Reflection Factors.
        ' Set the NM-Factors static, or depending on the dimension of the problem.
        ' http://www.webpages.uidaho.edu/~fuchang/res/ANMS.pdf
        If Me.ThisFitProcedureSettings.UseAdaptiveNelderMeadFactors And ndim > 2 Then
            Me.NMalpha = -1.0
            Me.NMbeta = 1.0 + 2.0 / ndim
            Me.NMgamma = 0.75 - 0.5 / ndim
            Me.NMdelta = 0.5 - 1.0 / ndim
        Else
            Me.NMalpha = -1.0
            Me.NMbeta = 2.0
            Me.NMgamma = 0.5
            Me.NMdelta = 0.5
        End If

        '// ** Der Simplex (= die Amoebe) erzeugen; **
        ' For each fitted parameter a set of parameters is created
        ReDim ParameterSimplex(ndim)
        For k As Integer = 0 To Me.NonFixedFitParameterCount Step 1
            'ParameterSimplex(k) = New Double(ModelFitFunction.FitParameters.Count - 1) {}
            ParameterSimplex(k) = New Double(Me.NonFixedFitParameterCount - 1) {}
        Next

        '// ** Die Startwerte des Simplex setzen; **
        '// ** Zunaechst: Jeden Eckpunkt auf denselben (Start-)Wert; **
        For k As Integer = 0 To Me.NonFixedFitParameterCount Step 1
            For j As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                ParameterSimplex(k)(j) = ModelFitFunction.FitParameters.Parameter(Me.NonFixedFitParameterIndices(j)).Value
            Next
        Next

        '// ** Bei ndim - 1 Eckpunkten je einen aktiven Parameter auslenken; **
        '// ** Inaktive Parameter bleiben bei allen gleich; **
        Dim i As Integer = 1
        For j As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            If ModelFitFunction.FitParameters.Parameter(Me.NonFixedFitParameterIndices(j)).Value <> 0 Then
                ParameterSimplex(i)(j) *= 1.1
            Else
                ParameterSimplex(i)(j) = 0.1
            End If
            i += 1
        Next

        '' Lock parameters together for all initial simplizes
        'For j As Integer = 1 To Me.NonFixedFitParameterCount Step 1
        '    cFitProcedureBase.LockParametersTogether(ParameterIdentifiers, ParameterSimplex(j), ParameterFixed, ParameterLockedTo)
        'Next

        '// ** Speicher fuer den besten jemals gefundenen Punkt und seinen Funktionswert reservieren; **
        ReDim BestParameterSimplex(Me.NonFixedFitParameterCount - 1)
        BestParameterSimplexChiSq = 0

        '// ** Setzte den besten jemals gefundenen Punkt zunaechst auf den Startpunkt; **
        ' And calculate the Chi2 for this point.
        For j As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            BestParameterSimplex(j) = ModelFitFunction.FitParameters.Parameter(Me.NonFixedFitParameterIndices(j)).Value
        Next
        BestParameterSimplexChiSq = cFitProcedureBase.CalculateChi2(FitFunction, FitDataPoints, Weights, ModelFitFunction.FitParametersGrouped)

        '// ** Deklaration der in der Iteration benötigten Variablen; **
        ReDim ValuesForEachSimplex(ndim)
        ReDim CalculationDoneForEachSimplex(ndim)
        Array.Clear(CalculationDoneForEachSimplex, 0, CalculationDoneForEachSimplex.Length)

        ' Create variables to save the indices of the low- and high-points
        iHighPointIndex = 0
        iSecondHighPointIndex = 0
        iLowPointIndex = 0

        ' Create the array for calculation of the bary-center-point
        ReDim BaryCenterPoint(Me.NonFixedFitParameterCount - 1)

        ' Used for testing for a better point
        dTestValue = 0

        ' Array to save the thermal values of the simulated annealing
        ReDim ThermalValuesForEachSimplex(ndim)

        ' Save the start-temperature
        T = Me.ThisFitProcedureSettings.SimulatedAnnealingStartTemperature

    End Sub

    ''' <summary>
    ''' FitStep. This functions gets called in the fit-loop,
    ''' until the fit converged! Has to return the calculated Chi2.
    ''' </summary>
    Protected Function FitStep(ByRef FitDataPoints As Double()(),
                               ByRef Weights As Double(),
                               ByRef InputParameters As cFitParameterGroupGroup,
                               ByRef Iteration As Integer,
                               ByRef StopReason As Integer) As Double Implements iFitProcedure.FitStep

        '// ** Funktionswerte an jedem Simplexpunkt bestimmen; **
        For p As Integer = 0 To ndim Step 1
            ' Check Simplex Value
            If Not CalculationDoneForEachSimplex(p) Then

                ' Write Changed Parameters to the parameter-set, and calculate Chi2, afterwards reset
                Me.WriteParameterSimplexToFitParameterGroup(ParameterSimplex(p), InputParameters)
                ValuesForEachSimplex(p) = cFitProcedureBase.CalculateChi2(Me.FitFunction, FitDataPoints, Weights, InputParameters)
                Me.ResetParameterGroupToOldValues(InputParameters)

                CalculationDoneForEachSimplex(p) = True

                If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
                    '// ** Wenn dies der beste jemals gefundene Wert ist: Sich als solchen merken; **
                    If ValuesForEachSimplex(p) < BestParameterSimplexChiSq Then
                        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                            BestParameterSimplex(d) = ParameterSimplex(p)(d)
                        Next

                        'cFitProcedureBase.LockParametersTogether(ParameterIdentifiers, BestParameterSimplex, ParameterFixed, ParameterLockedTo)
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

        '// ** 2. Abbruchbedingung: Simplex liegt nahezu flach da; **
        If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
            If Math.Abs(ThermalValuesForEachSimplex(iHighPointIndex) - ThermalValuesForEachSimplex(iLowPointIndex)) / (Math.Abs(ThermalValuesForEachSimplex(iHighPointIndex)) + Math.Abs(ThermalValuesForEachSimplex(iLowPointIndex)) + cConstants.Epsilon) < Me.ThisFitProcedureSettings.StopCondition_MinChi2Change Then
                StopReason = cFitProcedureBase.FitStopReasons.FitConverged
            End If
            If Iteration >= Me.ThisFitProcedureSettings.SimulatedAnnealingSteps Then
                StopReason = cNMAFit.FitStopReasons.SimulatedAnnealingFinished
            End If
        Else
            If Math.Abs(ValuesForEachSimplex(iHighPointIndex) - ValuesForEachSimplex(iLowPointIndex)) / (Math.Abs(ValuesForEachSimplex(iHighPointIndex)) + Math.Abs(ValuesForEachSimplex(iLowPointIndex)) + cConstants.Epsilon) < Me.ThisFitProcedureSettings.StopCondition_MinChi2Change Then
                StopReason = cFitProcedureBase.FitStopReasons.FitConverged
            End If
        End If

        ' Exit While!
        If StopReason <> cFitProcedureBase.FitStopReasons.None Then
            Return BestChi2
        End If

        '// ** Berechne den Schwerpunkt der Gegenfläche des Hochpunktes; **
        '// ** Schwerpunkt von D Vektoren V_i = ( sum_{i=0}^{D-1} V_i ) / D; **
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            BaryCenterPoint(d) = 0
            'CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
            For p As Integer = 0 To ndim Step 1
                If p <> iHighPointIndex Then BaryCenterPoint(d) += ParameterSimplex(p)(d) '(CurrentParameterIndex)
            Next
            BaryCenterPoint(d) /= ndim
        Next

        ' Berechne Spiegelpunkt vom Hochpunkt durch den Schwerpunkt des Gegendreiecks und tauscht die beiden
        ' Punkte, falls der Funktionswert des neuen Punktes kleiner ist als der des alten Hochpunktes;
        If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
            dTestValue = Me.SimulatedAnnealingAmotry(FitDataPoints,
                                                     Weights,
                                                     ParameterSimplex(iHighPointIndex),
                                                     ValuesForEachSimplex(iHighPointIndex),
                                                     ThermalValuesForEachSimplex(iHighPointIndex),
                                                     BestParameterSimplex,
                                                     BestParameterSimplexChiSq,
                                                     BaryCenterPoint,
                                                     NMalpha,
                                                     T,
                                                     Rand.NextDouble,
                                                     InputParameters)
        Else
            dTestValue = Me.Amotry(FitDataPoints,
                                   Weights,
                                   ParameterSimplex(iHighPointIndex),
                                   ValuesForEachSimplex(iHighPointIndex),
                                   BaryCenterPoint,
                                   NMalpha,
                                   InputParameters)
        End If


        If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
            ' SIMULATED ANNEALING -> Use Thermal values!!!
            '###############################################

            '// ** Ist der Funktionswert des neuen Punktes sogar niedriger als der des Tiefpunktes: Versuche doppelt so **
            '// ** weit in diese Richtung zu gehen; **
            If dTestValue <= ThermalValuesForEachSimplex(iLowPointIndex) Then
                Me.SimulatedAnnealingAmotry(FitDataPoints,
                                            Weights,
                                            ParameterSimplex(iHighPointIndex),
                                            ValuesForEachSimplex(iHighPointIndex),
                                            ThermalValuesForEachSimplex(iHighPointIndex),
                                            BestParameterSimplex,
                                            BestParameterSimplexChiSq,
                                            BaryCenterPoint,
                                            NMbeta,
                                            T,
                                            Rand.NextDouble,
                                            InputParameters)
            Else
                '// ** => Der neue Funktionswert ist (a) entweder immer noch der alte Hochpunkt oder **
                '// **                               (b) zwar neu, im Funktionswert aber trotzdem nicht besser als der Tiefpunkt; **

                '// ** Wenn der neue Funktionswert schlechter ist als der des zweithöchsten Punktes (d.h. der Simplex in seiner **
                '// ** Situation durch Bewegung nicht verbessert hat) nur halb so weit in Richtung des Schwerpunktes zu gehen; **
                If dTestValue >= ThermalValuesForEachSimplex(iSecondHighPointIndex) Then
                    Dim dSaveValue As Double = dTestValue
                    dTestValue = Me.SimulatedAnnealingAmotry(FitDataPoints,
                                                             Weights,
                                                             ParameterSimplex(iHighPointIndex),
                                                             ValuesForEachSimplex(iHighPointIndex),
                                                             ThermalValuesForEachSimplex(iHighPointIndex),
                                                             BestParameterSimplex,
                                                             BestParameterSimplexChiSq,
                                                             BaryCenterPoint,
                                                             NMgamma,
                                                             T,
                                                             Rand.NextDouble,
                                                             InputParameters)
                    '// ** Wenn dieser Schritt nichts verbessert: Kontrahiere alle Punkte in Richtung des Tiefpunktes, um **
                    '// ** eventuell durch eine Engstelle in der Funktionswertelandschaft schluepfen zu koennen; **
                    If dTestValue >= dSaveValue Then
                        For p As Integer = 0 To ndim Step 1
                            If p <> iLowPointIndex Then
                                For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                    ParameterSimplex(p)(d) = (ParameterSimplex(p)(d) + ParameterSimplex(iLowPointIndex)(d)) * NMdelta
                                Next
                                'cFitProcedureBase.LockParametersTogether(ParameterIdentifiers, ParameterSimplex(p), ParameterFixed, ParameterLockedTo)
                                CalculationDoneForEachSimplex(p) = False
                            End If
                        Next
                    End If
                End If
            End If

            '// ** Die Temperatur reduzieren; **
            Select Case Me.ThisFitProcedureSettings.SimulatedAnnealingCoolingType
                Case cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Linear
                    T = Me.ThisFitProcedureSettings.SimulatedAnnealingStartTemperature * (1.0 - Iteration / Me.ThisFitProcedureSettings.SimulatedAnnealingSteps)
                Case cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Exponential
                    T = Me.ThisFitProcedureSettings.SimulatedAnnealingStartTemperature * Math.Exp(-Iteration * 6 / Me.ThisFitProcedureSettings.SimulatedAnnealingSteps)
            End Select

            ' // ** Zwischenergebniss-Ausgabe; **
            If BestParameterSimplexChiSq < BestChi2 Then
                Me.WriteParameterSimplexToFitParameterGroup(BestParameterSimplex, InputParameters)
                BestChi2 = BestParameterSimplexChiSq
            End If
        Else
            ' Use simplex values without simulated annealing!!!
            '####################################################

            '// ** Ist der Funktionswert des neuen Punktes sogar niedriger als der des Tiefpunktes: Versuche doppelt so **
            '// ** weit in diese Richtung zu gehen; **
            If dTestValue <= ValuesForEachSimplex(iLowPointIndex) Then
                Me.Amotry(FitDataPoints,
                          Weights,
                          ParameterSimplex(iHighPointIndex),
                          ValuesForEachSimplex(iHighPointIndex),
                          BaryCenterPoint,
                          NMbeta,
                          InputParameters)
            Else
                '// ** => Der neue Funktionswert ist (a) entweder immer noch der alte Hochpunkt oder                              **
                '// **                               (b) zwar neu, im Funktionswert aber trotzdem nicht besser als der Tiefpunkt; **
                '// ** Wenn der neue Funktionswert schlechter ist als der des zweithöchsten Punktes (d.h. der Simplex in seiner   **
                '// ** Situation durch Bewegung nicht verbessert hat) nur halb so weit in Richtung des Schwerpunktes zu gehen;    **
                If dTestValue >= ValuesForEachSimplex(iSecondHighPointIndex) Then
                    Dim dSaveValue As Double = dTestValue
                    dTestValue = Me.Amotry(FitDataPoints,
                                           Weights,
                                           ParameterSimplex(iHighPointIndex),
                                           ValuesForEachSimplex(iHighPointIndex),
                                           BaryCenterPoint,
                                           NMgamma,
                                           InputParameters)

                    '// ** Wenn dieser Schritt nichts verbessert: Kontrahiere alle Punkte in Richtung des Tiefpunktes, um **
                    '// ** eventuell durch eine Engstelle in der Funktionswertelandschaft schluepfen zu koennen; **
                    If dTestValue >= dSaveValue Then
                        For p As Integer = 0 To ndim Step 1
                            If p <> iLowPointIndex Then

                                For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                                    ParameterSimplex(p)(d) = (ParameterSimplex(p)(d) + ParameterSimplex(iLowPointIndex)(d)) * NMdelta
                                Next
                                'cFitProcedureBase.LockParametersTogether(ParameterIdentifiers, ParameterSimplex(p), ParameterFixed, ParameterLockedTo)

                                CalculationDoneForEachSimplex(p) = False
                            End If
                        Next
                    End If
                End If
            End If

            ' // ** Zwischenergebniss-Ausgabe; **
            If ValuesForEachSimplex(iLowPointIndex) < BestChi2 Then
                Me.WriteParameterSimplexToFitParameterGroup(ParameterSimplex(iLowPointIndex), InputParameters)
                BestChi2 = ValuesForEachSimplex(iLowPointIndex)
            End If

        End If

        Return BestChi2
    End Function

    ''' <summary>
    ''' FitFinalizer, called, after the fit finished.
    ''' Releases resources, etc.
    ''' </summary>
    Protected Function FitFinalizer(ByRef FitDataPoints As Double()(),
                                    ByRef Weights As Double(),
                                    ByRef InputParameters As cFitParameterGroupGroup) As Boolean Implements iFitProcedure.FitFinalizer

        If Me.ThisFitProcedureSettings.UseSimulatedAnnealing Then
            ' Minimization finished -> use lowest point found during the annealing
            For i As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).Parameter(Me.NonFixedFitParameterIndices(i)).ChangeValue(BestParameterSimplex(i), False)
            Next
        Else
            ' Minimization finished -> use lowest point
            For i As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).Parameter(Me.NonFixedFitParameterIndices(i)).ChangeValue(ParameterSimplex(iLowPointIndex)(i), False)
            Next
        End If

        ' Clear Resources
        ParameterSimplex = Nothing
        FitFunction = Nothing
        BestParameterSimplex = Nothing
        ValuesForEachSimplex = Nothing
        CalculationDoneForEachSimplex = Nothing
        BaryCenterPoint = Nothing
        ThermalValuesForEachSimplex = Nothing
        Rand = Nothing
        NonFixedFitParameterIndices = Nothing

        Return True
    End Function

    ''' <summary>
    ''' Writes a given simplex to the given parameter-group, and locks the parameters together.
    ''' </summary>
    Private Sub WriteParameterSimplexToFitParameterGroup(ByRef ParameterSimplex As Double(), ByRef InputParameters As cFitParameterGroupGroup)

        ' Secure the old values
        Debug.WriteLine("Securing unincremented values ...")
        For j As Integer = 0 To Me.ParameterCount - 1 Step 1
            OldValues(j) = InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).ParameterByIndex(j).Value
        Next

        ' Save the new values
        Debug.WriteLine("Writing new values ...")
        For i As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).Parameter(Me.NonFixedFitParameterIndices(i)).ChangeValue(ParameterSimplex(i), False)
        Next

        ' Lock the parameters together
        cFitProcedureBase.LockParametersTogether(InputParameters)

    End Sub

    ''' <summary>
    ''' Resets the input parameters to the values saved in Me.OldValues
    ''' </summary>
    Private Sub ResetParameterGroupToOldValues(ByRef InputParameters As cFitParameterGroupGroup)
        ' Reset the parameters to the old values
        Debug.WriteLine("Resetting values ...")
        For i As Integer = 0 To Me.ParameterCount - 1 Step 1
            InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).ParameterByIndex(i).ChangeValue(OldValues(i), False)
        Next
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
    Private Function Amotry(ByRef FitDataPoints As Double()(),
                            ByRef Weights As Double(),
                            ByRef HighPointSimplex As Double(),
                            ByRef HighPointValue As Double,
                            ByRef BaryCenterPoint As Double(),
                            ByRef StretchFactor As Double,
                            ByRef InputParameters As cFitParameterGroupGroup) As Double

        '// ** Berechne den zu testenden Simplex-Punkt und seinen Funktionswert; **
        Dim TestSimplex(HighPointSimplex.Length - 1) As Double

        ' Fill the TestSimplex with the High-Point Values to set first
        ' also all fixed parameters!
        For i As Integer = 0 To HighPointSimplex.Length - 1 Step 1
            TestSimplex(i) = HighPointSimplex(i)
        Next

        ' Calculate the non-fixed parameters!
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            'CurrentParameterIndex = Me.NonFixedFitParameterIndices(d)
            Dim NewValue As Double = BaryCenterPoint(d) + StretchFactor * (HighPointSimplex(d) - BaryCenterPoint(d)) ' (CurrentParameterIndex) - BaryCenterPoint(d))
            TestSimplex(d) = NewValue
            'TestSimplex(CurrentParameterIndex) = NewValue
        Next
        'cFitProcedureBase.LockParametersTogether(InIdentifiers, TestSimplex, InFixed, InLockedTo)

        ' Calculate the value of Chi2 for this test-point.
        ' Write Changed Parameters to the parameter-set, and calculate Chi2, afterwards reset
        Me.WriteParameterSimplexToFitParameterGroup(TestSimplex, InputParameters)
        Dim TestSimplexValue As Double = cFitProcedureBase.CalculateChi2(FitFunction, FitDataPoints, Weights, InputParameters)
        Me.ResetParameterGroupToOldValues(InputParameters)

        '// ** Prüfen, ob der Testpunkt besser, d.h. sein Funktionswert kleiner als der des Hochpunktes ist; **
        If TestSimplexValue < HighPointValue Then
            '// ** => Dann tritt der Testpunkt an die Stelle des Hochpunktes; Ueberschreibe Punkt und Funktionswert; **
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
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
    Private Function SimulatedAnnealingAmotry(ByRef FitDataPoints As Double()(),
                                              ByRef Weights As Double(),
                                              ByRef HighPointSimplex As Double(),
                                              ByRef HighPointValue As Double,
                                              ByRef HighPointThermalValue As Double,
                                              ByRef BestParameterSimplex As Double(),
                                              ByRef BestParameterSimplexChiSq As Double,
                                              ByRef BaryCenterPoint As Double(),
                                              ByRef StretchFactor As Double,
                                              ByRef Temperature As Double,
                                              ByVal Rand As Double,
                                              ByRef InputParameters As cFitParameterGroupGroup) As Double

        '// ** Berechne den zu testenden Simplex-Punkt und seinen Funktionswert; **
        Dim TestSimplex(HighPointSimplex.Length - 1) As Double

        ' Fill the TestSimplex with the High-Point Values to set first
        ' also all fixed parameters!
        For i As Integer = 0 To HighPointSimplex.Length - 1 Step 1
            TestSimplex(i) = HighPointSimplex(i)
        Next

        ' Calculate the non-fixed parameters!
        For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            Dim NewValue As Double = BaryCenterPoint(d) + StretchFactor * (HighPointSimplex(d) - BaryCenterPoint(d))
            TestSimplex(d) = NewValue
        Next
        'cFitProcedureBase.LockParametersTogether(InIdentifiers, TestSimplex, InFixed, InLockedTo)

        ' Calculate the value of Chi2 for this test-point.
        ' Write Changed Parameters to the parameter-set, and calculate Chi2, afterwards reset
        Me.WriteParameterSimplexToFitParameterGroup(TestSimplex, InputParameters)
        Dim TestSimplexValue As Double = cFitProcedureBase.CalculateChi2(FitFunction, FitDataPoints, Weights, InputParameters)
        Me.ResetParameterGroupToOldValues(InputParameters)

        '// ** Einen moeglicherweise besten Funktionswert speichern; **
        If TestSimplexValue < BestParameterSimplexChiSq Then
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                BestParameterSimplex(d) = TestSimplex(d)
            Next
            BestParameterSimplexChiSq = TestSimplexValue
        End If

        '// ** Wert "thermisch verfaelscht" (siehe Numerical Recipes) speichern; **
        Dim TestSimplexThermalValue As Double = TestSimplexValue - Temperature * Math.Log((Rand + 1) / 2)

        '// ** Prüfen, ob der Testpunkt besser, d.h. sein Funktionswert kleiner als der des Hochpunktes ist; **
        If TestSimplexThermalValue < HighPointThermalValue Then
            '// ** => Dann tritt der Testpunkt an die Stelle des Hochpunktes; Ueberschreibe Punkt und Funktionswert; **
            For d As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
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
