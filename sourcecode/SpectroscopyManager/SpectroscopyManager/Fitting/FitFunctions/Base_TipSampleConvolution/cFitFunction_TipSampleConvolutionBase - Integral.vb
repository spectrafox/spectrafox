Imports System.Threading.Tasks
Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports SpectroscopyManager.cNumericFunctions

#Const USEPARALLEL = 1

Public MustInherit Class cFitFunction_TipSampleConvolutionBase
    Inherits cFitFunction

#Region "initialize constants used"

    ''' <summary>
    ''' Cutoff for evaluation of exponential function:
    ''' </summary>
    Public Const MAX_EXP As Double = 100D

    ''' <summary>
    ''' Cutoff for evaluation of exponential function:
    ''' </summary>
    Public Const MIN_EXP As Double = -100D

    ''' <summary>
    ''' Cutoff for Integrals, if the value becomes irrelevant small.
    ''' </summary>
    Public Const MIN_DIFFERENTIAL As Double = 0.0001

#End Region

#Region "Fit range plausibility check"

    ''' <summary>
    ''' Check the fit-range to be not too large (<= 10mV).
    ''' Otherwise the default inititalization will need too much time.
    ''' </summary>
    Public Overrides Function FitFunctionSuggestsDifferentFitRange(ByRef FitRangeLower As Double, ByRef FitRangeUpper As Double) As Boolean
        If FitRangeLower < -0.01 Or FitRangeUpper > 0.01 Then
            FitRangeLower = -0.01
            FitRangeUpper = 0.01
            Return False
        Else
            Return True
        End If
    End Function

#End Region

#Region "Initialize Calculation Parameters"
    ''' <summary>
    ''' Energy sampling size in [eV]
    ''' Used for calculation of the current integral.
    ''' This is the step size between which the current is summed up.
    ''' (irrational for minizing numerical artifacts)
    ''' </summary>
    Public Property ConvolutionIntegrationStepSize As Double = 0.00001090169943749474241023D

    ''' <summary>
    ''' Largest relevant energy value to be considered for integration in the current integral.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property ConvolutionIntegralE_POS As Double = 0.004

    ''' <summary>
    ''' Smallest relevant energy value to be considered for integration in the current integral.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property ConvolutionIntegralE_NEG As Double = -0.004

    ''' <summary>
    ''' Calculate and cache all current values up to this bias.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property CalculateForBiasRangeUpperE As Double = 0.004

    ''' <summary>
    ''' Calculate and cache all current values from this bias.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property CalculateForBiasRangeLowerE As Double = -0.004

    ''' <summary>
    ''' Change the bias range of the calculated current.
    ''' </summary>
    Public Overrides Sub ChangeFitRangeX(LowerValue As Double, HigherValue As Double)
        'CalculateForBiasRangeUpperE = HigherValue
        'CalculateForBiasRangeLowerE = LowerValue
        'ConvolutionIntegralE_POS = HigherValue
        'ConvolutionIntegralE_NEG = LowerValue
        MyBase.ChangeFitRangeX(LowerValue, HigherValue)

        If Not LastParameterIdentifiers Is Nothing And Not LastParameterValues Is Nothing Then
            Me.PrecalculateCurrent(LastParameterIdentifiers, LastParameterValues)
        End If
    End Sub

    ''' <summary>
    ''' Forces the precalculation of the values.
    ''' </summary>
    Public Sub ForceNewPrecalculation()
        If Not LastParameterIdentifiers Is Nothing And Not LastParameterValues Is Nothing Then
            Me.PrecalculateCurrent(LastParameterIdentifiers, LastParameterValues)
        End If
    End Sub

#End Region

#Region "Model-Specific Properties"
    ''' <summary>
    ''' Type of function that should be returned by the FitFunction()
    ''' </summary>
    Public Enum FitFunctionType
        I = 0
        dIdV = 1
    End Enum

    ''' <summary>
    ''' Type of data that should be fitted.
    ''' </summary>
    Public Property FitDataType As FitFunctionType = FitFunctionType.dIdV

#End Region

#Region "InelasticChannel Definition"

    Public Const IECIdentifierOffset As Integer = 100
    Public Const OtherIdentifierOffset As Integer = 10000

    ''' <summary>
    ''' Constants added to the Fit-Parameter identifiers
    ''' </summary>
    Private Enum IECParameterIdentifier
        Energy = 0
        Probability = 1
    End Enum

    ''' <summary>
    ''' Sub-Class to define an inelastic channel with its parameters.
    ''' </summary>
    Public Class InelasticChannel
        Public Property Energy As Double
        Public Property EnergyFixed As Boolean
        Public Property Probability As Double
        Public Property ProbabilityFixed As Boolean
    End Class

    ''' <summary>
    ''' List to save all inelastic channels.
    ''' </summary>
    Private InelasticChannels As New List(Of InelasticChannel)

    ''' <summary>
    ''' Adds a new inelastic channel peak to the fit-function.
    ''' Returns the index of the internal list, at which the channel got added.
    ''' </summary>
    Public Function AddInelasticChannel(ByRef InelasticChannel As InelasticChannel) As Integer
        Me.InelasticChannels.Add(InelasticChannel)
        Return Me.InelasticChannels.Count - 1
    End Function

    ''' <summary>
    ''' Clears the list of all InelasticChannels.
    ''' </summary>
    Public Sub ClearInelasticChannels()
        Me.InelasticChannels.Clear()
    End Sub

    ''' <summary>
    ''' Returns the list of all registered inelastic channels
    ''' </summary>
    Public ReadOnly Property RegisteredInelasticChannels As List(Of InelasticChannel)
        Get
            Return Me.InelasticChannels
        End Get
    End Property

    ''' <summary>
    ''' Method that sets all initial FitParameters from the individual FitFunctions
    ''' in this multiple-function-container.
    ''' Override this, !!!BUT CALL THIS!!! to set additional properties, such as Sub-Gap-Peaks in BCS.
    ''' </summary>
    Public Overridable Sub ReInitializeFitParameters()
        ' Remove old inelastic channel fit-parameters from the Fit-Parameter-Array
        Dim FitParameterIdentifiersToRemove As New List(Of Integer)
        For Each FitParameterIdentifier As Integer In Me.FitParameters.Keys
            If FitParameterIdentifier >= IECIdentifierOffset And FitParameterIdentifier < OtherIdentifierOffset Then
                FitParameterIdentifiersToRemove.Add(FitParameterIdentifier)
            End If
        Next
        For i As Integer = 0 To FitParameterIdentifiersToRemove.Count - 1 Step 1
            Me.FitParameters.Remove(FitParameterIdentifiersToRemove(i))
        Next

        ' Modify the Fit-Parameter-Array and add all inelastic channels
        For i As Integer = 0 To Me.InelasticChannels.Count - 1 Step 1
            Dim FitParameterEnergy As sFitParameter
            Dim FitParameterProbability As sFitParameter

            Dim IEC As InelasticChannel = Me.InelasticChannels(i)
            ' create Fit-Parameter
            FitParameterEnergy = New sFitParameter("IEC " & i.ToString & " Energy", IEC.Energy, IEC.EnergyFixed, "IEC " & i.ToString & " Energy")
            FitParameterProbability = New sFitParameter("IEC " & i.ToString & " Probability", IEC.Probability, IEC.ProbabilityFixed, "IEC " & i.ToString & " Probability")

            ' add Fit-Parameters
            With Me.FitParameters
                .Add(((i + 1) * IECIdentifierOffset) + IECParameterIdentifier.Energy, FitParameterEnergy)
                .Add(((i + 1) * IECIdentifierOffset) + IECParameterIdentifier.Probability, FitParameterProbability)
            End With
        Next
    End Sub

    ''' <summary>
    ''' This function sets all parameters of the inelastic channels from the Fit-Parameter-Array
    ''' given by the fit-procedure.
    ''' Override this, !!!BUT CALL THIS!!! to set additional properties, such as Sub-Gap-Peaks in BCS.
    ''' </summary>
    Public Overridable Sub SetAdditionalProperties(ByRef Identifiers As Integer(), ByRef Values As Double())
        ' Go through all Fit-Parameters and extract those from certain InelasticChannels.
        For i As Integer = 0 To Identifiers.Length - 1 Step 1
            ' Ignore original fit-parameters
            If Identifiers(i) < IECIdentifierOffset Or Identifiers(i) >= OtherIdentifierOffset Then Continue For

            ' Get the InelasticChannel-Index by dividing the Identifier by 100 and cut the rest.
            Dim InelasticChannelCount As Integer = CInt(Identifiers(i) / IECIdentifierOffset)
            Dim InelasticChannelIndex As Integer = InelasticChannelCount - 1

            ' Set the parameter-value only
            Select Case Identifiers(i)
                Case (InelasticChannelCount * IECIdentifierOffset) + IECParameterIdentifier.Energy
                    ' IEC Energy
                    Me.InelasticChannels(InelasticChannelIndex).Energy = Values(i)
                Case (InelasticChannelCount * IECIdentifierOffset) + IECParameterIdentifier.Probability
                    ' IEC Probability
                    Me.InelasticChannels(InelasticChannelIndex).Probability = Values(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Returns the parameter-identifier for the given IEC.
    ''' </summary>
    <Cudafy>
    <DebuggerStepThrough>
    Public Shared Function GetIECParameterIdentifierFromPeakIndex(ByVal PeakIndex As Integer,
                                                                  ByVal ParameterIdentifier As Integer) As Integer
        Return (PeakIndex * IECIdentifierOffset) + ParameterIdentifier
    End Function

    ''' <summary>
    ''' This functions gets the value of a IEC parameter from the
    ''' input of two arrays of fit-parameters.
    ''' </summary>
    <Cudafy>
    <DebuggerStepThrough>
    Public Shared Function GetIECFromParameterArray(ByVal IECIndex As Integer,
                                                    ByVal ParameterIdentifiers As Integer(),
                                                    ByVal ParameterValues As Double(),
                                                    ByVal IdentifierOfParameterToExtract As Integer) As Double
        Dim InelasticChannelCount As Integer
        Dim InelasticChannelIndex As Integer

        ' Go through all Fit-Parameters and extract those from certain InelasticChannels.
        For i As Integer = 0 To ParameterIdentifiers.Length - 1 Step 1
            ' Ignore original fit-parameters and other extended parameters > 10000
            If ParameterIdentifiers(i) >= IECIdentifierOffset And ParameterIdentifiers(i) < OtherIdentifierOffset Then
                ' Get the InelasticChannel-Index by dividing the Identifier by 100 and cut the rest.
                InelasticChannelCount = CInt(ParameterIdentifiers(i) / IECIdentifierOffset)
                InelasticChannelIndex = InelasticChannelCount - 1
                If IECIndex = InelasticChannelIndex And ParameterIdentifiers(i) = GetIECParameterIdentifierFromPeakIndex(InelasticChannelCount, IdentifierOfParameterToExtract) Then
                    Return ParameterValues(i)
                End If
            End If
        Next
        Return 0
    End Function

    ''' <summary>
    ''' This functions gets the number of IEC registered from the
    ''' input of two arrays of fit-parameters.
    ''' </summary>
    <Cudafy>
    <DebuggerStepThrough>
    Public Shared Function GetIECFromParameterArray_Count(ByVal ParameterIdentifiers As Integer(), ByVal ParameterValues As Double()) As Integer
        Dim Count As Integer = 0
        ' Go through all Fit-Parameters and extract those from certain InelasticChannels.
        For i As Integer = 0 To ParameterIdentifiers.Length - 1 Step 1
            ' Ignore original fit-parameters and other extended parameters > 10000
            If ParameterIdentifiers(i) >= IECIdentifierOffset And ParameterIdentifiers(i) < OtherIdentifierOffset Then
                Count += 1
            End If
        Next
        ' return divided by 2, because each channel has two FitParameters
        Return CInt(Count / 2)
    End Function
#End Region

#Region "Register Tip and Sample-DOS"

    ''' <summary>
    ''' Different types of DOS.
    ''' </summary>
    Public Enum DOSTypes
        Sample = 0
        Tip = 1
    End Enum

    ''' <summary>
    ''' Must-overrides that have to be defined by the child functions.
    ''' Implemented as SHARED to be able to use CUDA!
    ''' 
    ''' Does nothing! Override it, and declare it for CUDAfying!!!!
    ''' </summary>
    Public Shared Function SampleDOSCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
        ' Do nothing.
        Return 0
    End Function

    ''' <summary>
    ''' Must-overrides that have to be defined by the child functions.
    ''' Implemented as SHARED to be able to use CUDA!
    ''' 
    ''' Does nothing! Override it, and declare it for CUDAfying!!!!
    ''' </summary>
    Public Shared Function TipDOSCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
        ' Do nothing
        Return 0
    End Function

    ''' <summary>
    ''' Must-overrides that have to be defined by the child functions.
    ''' Implemented as SHARED to be able to use CUDA!
    ''' 
    ''' Does nothing! Override it, and declare it for CUDAfying!!!!
    ''' </summary>
    Public MustOverride Function SampleDOS(ByRef E As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double

    ''' <summary>
    ''' Must-overrides that have to be defined by the child functions.
    ''' Implemented as SHARED to be able to use CUDA!
    ''' 
    ''' Does nothing! Override it, and declare it for CUDAfying!!!!
    ''' </summary>
    Public MustOverride Function TipDOS(ByRef E As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double

    ''' <summary>
    ''' Register both DOS-generation functions.
    ''' </summary>
    Public Overrides Sub RegisterDataGenerationFunction()
        Me.AdditionalDataGenerationFunctions.Add(My.Resources.rFitFunction_TipSampleConvolutionBase.Tip_DOS, AddressOf TipDOS)
        Me.AdditionalDataGenerationFunctions.Add(My.Resources.rFitFunction_TipSampleConvolutionBase.Sample_DOS, AddressOf SampleDOS)
    End Sub
#End Region

#Region "Cuda-Registration of other classes to compile"

    ''' <summary>
    ''' Override this function in child-classes to add additional
    ''' classes to the CUDA compile-library.
    ''' </summary>
    Public Overridable Function GetCudaCompileClasses() As List(Of Type)
        Return Nothing
    End Function

#End Region

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifier
        GlobalXOffset = 10
        GlobalYOffset = 11
        GlobalYStretch = 12
        T_tip = 13
        T_sample = 14
        SystemBroadening = 15
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(FitParameterIdentifier.GlobalXOffset, New sFitParameter("Xoffset", 0, False, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalXOffset))
        Me.FitParameters.Add(FitParameterIdentifier.GlobalYOffset, New sFitParameter("Yoffset", 0, False, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalYOffset))
        Me.FitParameters.Add(FitParameterIdentifier.GlobalYStretch, New sFitParameter("Ystretch", 1, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalYStretch))
        Me.FitParameters.Add(FitParameterIdentifier.T_tip, New sFitParameter("TipTemperature", 1.19, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_T_tip))
        Me.FitParameters.Add(FitParameterIdentifier.T_sample, New sFitParameter("SampleTemperature", 1.19, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_T_sample))
        Me.FitParameters.Add(FitParameterIdentifier.SystemBroadening, New sFitParameter("SystemBroadening", 0.000035, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_SystemBroadening))
    End Sub
#End Region

#Region "Convolution integrands (CUDA Version)"
    ''' <summary>
    ''' // *****************************************************************
    ''' // * Integrand des elastischen Kanals (EC) des Tunnelintegrals zur *
    ''' // * Berechnung des Stroms I(V):                                   *
    ''' // *                                                               *
    ''' // *    Integr_EC = DOS_t(E) * DOS_s(E+eV) * [f(E) - f(E+eV)]      *
    ''' // *****************************************************************
    ''' </summary>
    <Cudafy>
    Public Shared Function IntegrandECCUDA(ByVal E As Double,
                                           ByVal eV As Double,
                                           ByVal Identifiers As Integer(),
                                           ByVal Values As Double()) As Double
        Return (FermiF_eV(E, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values)) - FermiF_eV(E + eV, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))) * (SampleDOSCUDA(E, Identifiers, Values) * TipDOSCUDA(E + eV, Identifiers, Values))
    End Function

    '// *****************************************************************
    '// * Integrand  eines  inelastischen Kanals (IEC) des  Tunnelinte- *
    '// * grals zur Berechnung des Stroms I(V):                         *
    '// *                                                               *
    '// * Intgr_IEC = DOS_t(E+E_iec)*DOS_s(E+eV)*f(E+E_iec)*[1-f(E+eV)] *
    '// *           - DOS_t(E-E_iec)*DOS_s(E+eV)*f(E+eV)*[1-f(E-E_iec)] *
    '// *                                                               *
    '// * (siehe Aufschriebe)                                           *
    '// *****************************************************************
    <Cudafy>
    Public Shared Function IntegrandIECCUDA(ByVal EPlusEIEC As Double,
                                            ByVal EMinusEIEC As Double,
                                            ByVal EPlusBias As Double,
                                            ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
        ' // ** Berechne Fermifunktionen der Spitze f(E + E_iec) sowie f(E - E_iec) und der Probe f(E + eV); **
        Dim FermiTipEPEIEC As Double = FermiF_eV(EPlusEIEC, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiTipEMEIEC As Double = FermiF_eV(EMinusEIEC, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiSampleEPV As Double = FermiF_eV(EPlusBias, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values))

        '// ** Berechnung und Rueckgabe des Integrand; **
        '// ** Benutze Vorausberechnete Funktionen um Rechenzeit zu sparen; **
        Return SampleDOSCUDA(EPlusBias, Identifiers, Values) _
                * (TipDOSCUDA(EPlusEIEC, Identifiers, Values) * FermiTipEPEIEC * (1.0 - FermiSampleEPV) _
                - TipDOSCUDA(EMinusEIEC, Identifiers, Values) * (1.0 - FermiTipEMEIEC) * FermiSampleEPV)
    End Function
#End Region

#Region "Convolution Integrand (CPU version)"
    ''' <summary>
    ''' // *****************************************************************
    ''' // * Integrand des elastischen Kanals (EC) des Tunnelintegrals zur *
    ''' // * Berechnung des Stroms I(V):                                   *
    ''' // *                                                               *
    ''' // *    Integr_EC = DOS_t(E) * DOS_s(E+eV) * [f(E) - f(E+eV)]      *
    ''' // *****************************************************************
    ''' </summary>
    Public Function IntegrandEC(ByVal E As Double,
                                ByVal eV As Double,
                                ByRef Identifiers As Integer(),
                                ByRef Values As Double()) As Double
        Return (FermiF_eV(E, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values)) - FermiF_eV(E + eV, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))) * (SampleDOS(E, Identifiers, Values) * TipDOS(E + eV, Identifiers, Values))
    End Function

    '// *****************************************************************
    '// * Integrand  eines  inelastischen Kanals (IEC) des  Tunnelinte- *
    '// * grals zur Berechnung des Stroms I(V):                         *
    '// *                                                               *
    '// * Intgr_IEC = DOS_t(E+E_iec)*DOS_s(E+eV)*f(E+E_iec)*[1-f(E+eV)] *
    '// *           - DOS_t(E-E_iec)*DOS_s(E+eV)*f(E+eV)*[1-f(E-E_iec)] *
    '// *                                                               *
    '// * (siehe Aufschriebe)                                           *
    '// *****************************************************************
    Public Function IntegrandIEC(ByVal EPlusEIEC As Double,
                                 ByVal EMinusEIEC As Double,
                                 ByVal EPlusBias As Double,
                                 ByRef Identifiers As Integer(), ByRef Values As Double()) As Double
        ' // ** Berechne Fermifunktionen der Spitze f(E + E_iec) sowie f(E - E_iec) und der Probe f(E + eV); **
        Dim FermiTipEPEIEC As Double = FermiF_eV(EPlusEIEC, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiTipEMEIEC As Double = FermiF_eV(EMinusEIEC, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiSampleEPV As Double = FermiF_eV(EPlusBias, sFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values))

        '// ** Berechnung und Rueckgabe des Integrand; **
        '// ** Benutze Vorausberechnete Funktionen um Rechenzeit zu sparen; **
        Return SampleDOS(EPlusBias, Identifiers, Values) _
                * (TipDOS(EPlusEIEC, Identifiers, Values) * FermiTipEPEIEC * (1.0 - FermiSampleEPV) _
                - TipDOS(EMinusEIEC, Identifiers, Values) * (1.0 - FermiTipEMEIEC) * FermiSampleEPV)
    End Function


#End Region

#Region "Fit-Function Interface and caching algorithm to calculate the current"

    ''' <summary>
    ''' Current-precalculation interpolation-interval-width:
    ''' is responsible for the number of points that need to be calculated,
    ''' and therefore the speed of the calculation, and the accuracy.
    ''' </summary>
    Public Shared dE_InterpolationStepWidth As Double = 0.00001

    ''' <summary>
    ''' Cache storage for the current integral.
    ''' </summary>
    Private CurrentIntegralCache_POS As Double()
    Private CurrentIntegralCache_NEG As Double()

    ' Bias cache properties,
    ' giving the bias range of the current values
    ' stored in the arrays.
    Private dVBiasStart_Pos As Double = 0
    Private dVBiasEnd_Pos As Double = 0
    Private dVBiasStart_Neg As Double = 0
    Private dVBiasEnd_Neg As Double = 0

    ' cache variables for the last used parameters, for which the current is cached
    Private LastParameterValues As Double()
    Private LastParameterIdentifiers As Integer()

    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' Caches the Current integral, if the fit-parameters have changed.
    ''' </summary>
    Public Overrides Function GetY(ByRef x As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double

        ' Check, if the current cache has to be refreshed.
        If Not cArrayHelper.AreArraysTheSame(Values, LastParameterValues) Or
           Not cArrayHelper.AreArraysTheSame(Identifiers, LastParameterIdentifiers) Then

            Me.PrecalculateCurrent(Identifiers, Values)

        End If

        ' Finally return the cached FitFunction
        Return Me.FitFunctionPRECALC(x, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Precalculates the current cache.
    ''' </summary>
    Protected Sub PrecalculateCurrent(ByVal Identifiers As Integer(), ByVal Values As Double())

        ' save the last used parameter-set
        ReDim LastParameterValues(Values.Length - 1)
        ReDim LastParameterIdentifiers(Identifiers.Length - 1)
        Values.CopyTo(LastParameterValues, 0)
        Identifiers.CopyTo(LastParameterIdentifiers, 0)

        '#########################################
        ' Precalculate the current and broaden it
        ' for the selected interval and use the
        ' GPGPU to speed up everything.

        ' Initialize variables
        Dim iCurrentCachePoints_Pos As Integer = 0
        dVBiasStart_Pos = 0
        dVBiasEnd_Pos = 0
        Dim iCurrentCachePoints_Neg As Integer = 0
        dVBiasStart_Neg = 0
        dVBiasEnd_Neg = 0

        ' catch the case where the energy-range is 0
        If Me.CalculateForBiasRangeUpperE <= Me.CalculateForBiasRangeLowerE Then
            Return
        End If

        ' Get the range for which to calculate the current
        ' additionally consider the broadening-width.
        Dim UpperBiasEnergy As Double = Me.CalculateForBiasRangeUpperE
        Dim LowerBiasEnergy As Double = Me.CalculateForBiasRangeLowerE

        If UpperBiasEnergy > 0 Then
            UpperBiasEnergy -= 0.5 * sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        Else
            UpperBiasEnergy += 0.5 * sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        End If
        If LowerBiasEnergy > 0 Then
            LowerBiasEnergy -= 0.5 * sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        Else
            LowerBiasEnergy += 0.5 * sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        End If

        ' Not calculate the cache of the current: 
        ' for this: calculate the number of points to cache
        ' and the borders to calculate the current in between
        If UpperBiasEnergy > 0 And LowerBiasEnergy > 0 Then
            ' only positive bias values
            '###########################
            iCurrentCachePoints_Pos = CInt((UpperBiasEnergy - LowerBiasEnergy) / dE_InterpolationStepWidth)
            dVBiasEnd_Pos = UpperBiasEnergy
            dVBiasStart_Pos = LowerBiasEnergy
            Me.CurrentIntegralCache_NEG = New Double() {}
        ElseIf UpperBiasEnergy < 0 And LowerBiasEnergy < 0 Then
            ' only negative bias values
            '###########################
            iCurrentCachePoints_Neg = -CInt((LowerBiasEnergy - UpperBiasEnergy) / dE_InterpolationStepWidth)
            dVBiasEnd_Neg = LowerBiasEnergy
            dVBiasStart_Neg = UpperBiasEnergy
            Me.CurrentIntegralCache_POS = New Double() {}
        Else
            ' both, positive and negative bias values.
            '##########################################
            iCurrentCachePoints_Pos = Math.Abs(CInt(UpperBiasEnergy / dE_InterpolationStepWidth))
            iCurrentCachePoints_Neg = Math.Abs(CInt(LowerBiasEnergy / dE_InterpolationStepWidth))
            dVBiasStart_Neg = 0
            dVBiasStart_Pos = 0

            dVBiasEnd_Pos = UpperBiasEnergy
            dVBiasEnd_Neg = LowerBiasEnergy
        End If

        ' Redimension the cache arrays
        If Me.CurrentIntegralCache_POS Is Nothing And iCurrentCachePoints_Pos > 0 Then
            ReDim Me.CurrentIntegralCache_POS(iCurrentCachePoints_Pos - 1)
        ElseIf Me.CurrentIntegralCache_POS.Length <> iCurrentCachePoints_Pos And iCurrentCachePoints_Pos > 0 Then
            ReDim Me.CurrentIntegralCache_POS(iCurrentCachePoints_Pos - 1)
        End If
        If Me.CurrentIntegralCache_NEG Is Nothing And iCurrentCachePoints_Neg > 0 Then
            ReDim Me.CurrentIntegralCache_NEG(iCurrentCachePoints_Neg - 1)
        ElseIf Me.CurrentIntegralCache_NEG.Length <> iCurrentCachePoints_Neg And iCurrentCachePoints_Neg > 0 Then
            ReDim Me.CurrentIntegralCache_NEG(iCurrentCachePoints_Neg - 1)
        End If

        ' Create the XArray to calculate the current.
        Dim CurrentIntegralX_POS As Double() = New Double() {}
        Dim CurrentIntegralX_NEG As Double() = New Double() {}
        If iCurrentCachePoints_Pos > 0 Then
            ReDim CurrentIntegralX_POS(iCurrentCachePoints_Pos - 1)
        End If
        If iCurrentCachePoints_Neg > 0 Then
            ReDim CurrentIntegralX_NEG(iCurrentCachePoints_Neg - 1)
        End If

        '// fill the arrays on the CPU
        ' include the 0 in the positive array
        If iCurrentCachePoints_Pos > 0 Then
            Parallel.For(0, iCurrentCachePoints_Pos, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                   CurrentIntegralX_POS(j) = dVBiasStart_Pos + (dE_InterpolationStepWidth * j)
                                                                               End Sub)
        End If
        If iCurrentCachePoints_Neg > 0 Then
            Parallel.For(0, iCurrentCachePoints_Neg, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                   CurrentIntegralX_NEG(j) = dVBiasStart_Neg - (dE_InterpolationStepWidth * j)
                                                                               End Sub)
        End If

        '###################################
        ' Try to initialize the CUDA-Class
        If Me.UseCUDAVersion And Not Me.bCudaInizialized Then
            ' Get classes to compile for CUDA
            Dim CudaCompileClasses As List(Of Type) = Me.GetCudaCompileClasses
            If CudaCompileClasses Is Nothing Then
                CudaCompileClasses = New List(Of Type)
            End If

            ' add always this class and the numeric class.
            CudaCompileClasses.AddRange({GetType(cFitFunction_TipSampleConvolutionBase), GetType(cNumericFunctions), GetType(sFitParameter)})

            ' Try to initialize CUDA and compile the code.
            Me.InitializeCUDAOrFallBackToCPU(CudaCompileClasses.ToArray)
        End If
        '###################################

        ' Use CUDA? then copy the CPU-Array to the GPU
        If Me.UseCUDAVersion And Me.bCudaInizialized Then
            ' CUDA VERSION
            '##############
            Dim ThreadsPerBlock As Integer = cGPUComputing.GetThreadsPerBlock(CUDAGPU)

            ' Get the parameters to the GPU
            Dim dev_Identifiers As Integer() = CUDAGPU.Allocate(Of Integer)(Identifiers)
            Dim dev_Values As Double() = CUDAGPU.Allocate(Of Double)(Values)
            CUDAGPU.CopyToDevice(Identifiers, dev_Identifiers)
            CUDAGPU.CopyToDevice(Values, dev_Values)

            If iCurrentCachePoints_Pos <= 0 Then
                ReDim CurrentIntegralX_POS(1)
                ReDim CurrentIntegralCache_POS(1)
            End If
            If iCurrentCachePoints_Neg <= 0 Then
                ReDim CurrentIntegralX_NEG(1)
                ReDim CurrentIntegralCache_NEG(1)
            End If

            '// Get the storage on the CPU.
            Dim dev_CurrentIntegralX_POS As Double() = CUDAGPU.Allocate(Of Double)(CurrentIntegralX_POS)
            '// copy the array to the GPU
            CUDAGPU.CopyToDevice(CurrentIntegralX_POS, dev_CurrentIntegralX_POS)
            ' Get the output storage on the GPU
            Dim dev_CurrentIntegralCache_POS As Double() = CUDAGPU.Allocate(Of Double)(CurrentIntegralCache_POS)
            If iCurrentCachePoints_Pos > 0 Then
                '// launch Current-Integration on N threads
                CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid(iCurrentCachePoints_Pos, ThreadsPerBlock),
                               ThreadsPerBlock,
                               "CurrentCudaKernel",
                               dev_CurrentIntegralX_POS, dev_CurrentIntegralCache_POS,
                               dev_Identifiers, dev_Values,
                               Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, Me.ConvolutionIntegrationStepSize)
                ' Copy cached current arrays back to the CPU
                'CUDAGPU.CopyFromDevice(dev_CurrentIntegralCache_POS, CurrentIntegralCache_POS) ' WAS DONE BEFORE THE CONVOLUTION WITH A GAUSSIAN!
            End If

            '// Get the storage on the CPU.
            Dim dev_CurrentIntegralX_NEG As Double() = CUDAGPU.Allocate(Of Double)(CurrentIntegralX_NEG)
            '// copy the array to the GPU
            CUDAGPU.CopyToDevice(CurrentIntegralX_NEG, dev_CurrentIntegralX_NEG)
            ' Get the output storage on the GPU
            Dim dev_CurrentIntegralCache_NEG As Double() = CUDAGPU.Allocate(Of Double)(CurrentIntegralCache_NEG)

            If iCurrentCachePoints_Neg > 0 Then
                '// launch Current-Integration on N threads
                CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid(iCurrentCachePoints_Neg, ThreadsPerBlock),
                               ThreadsPerBlock,
                               "CurrentCudaKernel",
                               dev_CurrentIntegralX_NEG, dev_CurrentIntegralCache_NEG,
                               dev_Identifiers, dev_Values,
                               Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, Me.ConvolutionIntegrationStepSize)
                ' Copy cached current arrays back to the CPU
                'CUDAGPU.CopyFromDevice(dev_CurrentIntegralCache_NEG, CurrentIntegralCache_NEG) ' WAS DONE BEFORE THE CONVOLUTION WITH A GAUSSIAN!
            End If

            ' Convolute the current with a gaussian afterwards, to broaden it.
            Dim dev_CurrentConvolutionCache_POS As Double() = CUDAGPU.Allocate(Of Double)(CurrentIntegralCache_POS)
            Dim dev_CurrentConvolutionCache_NEG As Double() = CUDAGPU.Allocate(Of Double)(CurrentIntegralCache_NEG)
            CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid(Math.Max(iCurrentCachePoints_Pos, iCurrentCachePoints_Neg), ThreadsPerBlock),
                           ThreadsPerBlock,
                           "CurrentConvolutionCudaKernel",
                           dev_CurrentIntegralX_POS, dev_CurrentIntegralX_NEG,
                           dev_CurrentIntegralCache_POS, dev_CurrentIntegralCache_NEG,
                           dev_CurrentConvolutionCache_POS, dev_CurrentConvolutionCache_NEG,
                           CInt(Me.ConvolutionFunction), sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values),
                           Me.dVBiasStart_Pos, Me.dVBiasStart_Neg, Me.dVBiasEnd_Pos, Me.dVBiasEnd_Neg,
                           dE_BroadeningStepWidth, dE_InterpolationStepWidth)
            ' Copy the convoluted result back to the CPU
            CUDAGPU.CopyFromDevice(dev_CurrentConvolutionCache_POS, CurrentIntegralCache_POS)
            CUDAGPU.CopyFromDevice(dev_CurrentConvolutionCache_NEG, CurrentIntegralCache_NEG)

            ' Finally free the GPU-memory
            CUDAGPU.FreeAll()
        Else
            ' NON CUDA VERSION,
            ' but using .NET parallel library.
            '##################################
#If USEPARALLEL = 1 Then
            If iCurrentCachePoints_Pos > 0 Then
                Parallel.For(0, iCurrentCachePoints_Pos, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                       CurrentIntegralCache_POS(j) = FuncI(CurrentIntegralX_POS(j),
                                                                                                                           Identifiers,
                                                                                                                           Values,
                                                                                                                           Me.ConvolutionIntegralE_POS,
                                                                                                                           Me.ConvolutionIntegralE_NEG,
                                                                                                                           ConvolutionIntegrationStepSize)
                                                                                   End Sub)
                ' Convolute the current with a gaussian afterwards, to broaden it.
                Dim ConvolutedCurrent_POS(CurrentIntegralX_POS.Length - 1) As Double
                Parallel.For(0, iCurrentCachePoints_Pos, Me.MultiThreadingOptions, Sub(i As Integer)
                                                                                       ConvolutedCurrent_POS(i) = ConvolveFunction(CurrentIntegralX_POS(i),
                                                                                                                                   CInt(Me.ConvolutionFunction),
                                                                                                                                   sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                                                                                   End Sub)
                ConvolutedCurrent_POS.CopyTo(CurrentIntegralCache_POS, 0)
            End If
            If iCurrentCachePoints_Neg > 0 Then
                Parallel.For(0, iCurrentCachePoints_Neg, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                       CurrentIntegralCache_NEG(j) = FuncI(CurrentIntegralX_NEG(j),
                                                                                                                           Identifiers,
                                                                                                                           Values,
                                                                                                                           Me.ConvolutionIntegralE_POS,
                                                                                                                           Me.ConvolutionIntegralE_NEG,
                                                                                                                           ConvolutionIntegrationStepSize)
                                                                                   End Sub)
                ' Convolute the current with a gaussian afterwards, to broaden it.
                Dim ConvolutedCurrent_NEG(CurrentIntegralX_NEG.Length - 1) As Double
                Parallel.For(0, iCurrentCachePoints_Neg, Me.MultiThreadingOptions, Sub(i As Integer)
                                                                                       ConvolutedCurrent_NEG(i) = ConvolveFunction(CurrentIntegralX_NEG(i),
                                                                                                                                   CInt(Me.ConvolutionFunction),
                                                                                                                                   sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                                                                                   End Sub)
                ConvolutedCurrent_NEG.CopyTo(CurrentIntegralCache_NEG, 0)
            End If
#Else
            ' Convolute the current with a gaussian afterwards, to broaden it.
            If iCurrentCachePoints_Pos > 0 Then
                For j As Integer = 0 To iCurrentCachePoints_Pos - 1 Step 1
                    CurrentIntegralCache_POS(j) = FuncI(CurrentIntegralX_POS(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                Next
                ' Convolute the current with a gaussian afterwards, to broaden it.
                Dim ConvolutedCurrent_POS(CurrentIntegralX_POS.Length - 1) As Double
                For i As Integer = 0 To CurrentIntegralX_POS.Length - 1 Step 1
                    ConvolutedCurrent_POS(i) = ConvolveFunction(CurrentIntegralX_POS(i), CInt(Me.ConvolutionFunction), sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                Next
                ConvolutedCurrent_POS.CopyTo(CurrentIntegralCache_POS, 0)
            End If
            If iCurrentCachePoints_Neg > 0 Then
                For j As Integer = 0 To iCurrentCachePoints_Neg - 1 Step 1
                    CurrentIntegralCache_NEG(j) = FuncI(CurrentIntegralX_NEG(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                Next
                ' Convolute the current with a gaussian afterwards, to broaden it.
                Dim ConvolutedCurrent_NEG(CurrentIntegralX_NEG.Length - 1) As Double
                For i As Integer = 0 To CurrentIntegralX_NEG.Length - 1 Step 1
                    ConvolutedCurrent_NEG(i) = ConvolveFunction(CurrentIntegralX_NEG(i), CInt(Me.ConvolutionFunction), sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                Next
                ConvolutedCurrent_NEG.CopyTo(CurrentIntegralCache_NEG, 0)
            End If
#End If

        End If
    End Sub
#End Region

#Region "Convolution of the current"

    ''' <summary>
    ''' Energy sampling size in [eV] for the convolution with a Gauss function.
    ''' Used for calculation of broadening.
    ''' </summary>
    Public Shared dE_BroadeningStepWidth As Double = 0.0000309016994374947424102D

    ''' <summary>
    ''' Type of Broadening applied to the current.
    ''' </summary>
    Public Enum ConvolutionFunctions
        None = 0
        Gauss = 1
        Lorentz = 2
    End Enum

    ''' <summary>
    ''' Type of Broadening applied to the current.
    ''' </summary>
    Public Property ConvolutionFunction As ConvolutionFunctions

    ''' <summary>
    ''' // *****************************************************************
    ''' // *                                                               *
    ''' // *      ( f * g ) (E) = int f(F) * g( E - F ) dF                 *
    ''' // *                                                               *
    ''' // * oder mit einer Lorentz-Kurve g(E) mit derselben Breite;       *
    ''' // *                                                               *
    ''' // *****************************************************************
    ''' </summary>
    Public Function ConvolveFunction(ByVal E As Double,
                                     ByVal ConvolutionFunction As Integer,
                                     ByVal BroadeningWidth As Double) As Double
        ' Call the shared class, that is used for CUDA
        Return ConvolveFunctionCUDA(E,
                                    Me.CurrentIntegralCache_POS,
                                    Me.CurrentIntegralCache_NEG,
                                    ConvolutionFunction,
                                    BroadeningWidth,
                                    Me.dVBiasStart_Pos,
                                    Me.dVBiasStart_Neg,
                                    Me.dVBiasEnd_Pos,
                                    Me.dVBiasEnd_Neg,
                                    dE_BroadeningStepWidth,
                                    dE_InterpolationStepWidth)
    End Function

    ''' <summary>
    ''' // *****************************************************************
    ''' // *                                                               *
    ''' // *      ( f * g ) (E) = int f(F) * g( E - F ) dF                 *
    ''' // *                                                               *
    ''' // * oder mit einer Lorentz-Kurve g(E) mit derselben Breite;       *
    ''' // *                                                               *
    ''' // *****************************************************************
    ''' </summary>
    <Cudafy>
    Protected Shared Function ConvolveFunctionCUDA(ByVal E As Double,
                                                   ByVal CurrentIntegralCache_POS As Double(),
                                                   ByVal CurrentIntegralCache_NEG As Double(),
                                                   ByVal ConvolutionFunction As Integer,
                                                   ByVal BroadeningWidth As Double,
                                                   ByVal dVBiasStart_POS As Double,
                                                   ByVal dVBiasStart_NEG As Double,
                                                   ByVal dVBiasEnd_POS As Double,
                                                   ByVal dVBiasEnd_NEG As Double,
                                                   ByVal dE_BroadeningStepWidth As Double,
                                                   ByVal dE_InterpolationStepWidth As Double) As Double

        Dim ConvolutedCurrent As Double = 0

        If BroadeningWidth <= dE_BroadeningStepWidth Then
            ConvolutedCurrent = CurrentPRECALCCUDA(E, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)
        Else

            ' Get the maximum number of iterations in the energy-range.
            Dim NmaxPos As Integer = CInt((dVBiasEnd_POS * 1) / dE_BroadeningStepWidth)
            Dim NmaxNeg As Integer = CInt((-dVBiasEnd_NEG * 1) / dE_BroadeningStepWidth)

            ' Integration variable
            Dim dE As Double = 0.0
            Dim tmp As Double = 0.0
            ' Limit for small values of the broadening
            Dim MinGaussianValue As Double = 0.000000001
            Dim GaussianValue As Double = MinGaussianValue
            Dim MaxE As Double = Math.Max(cNumericFunctions.MathAbs(dVBiasStart_POS), cNumericFunctions.MathAbs(dVBiasStart_NEG))

            ' Set up a running compensation algorithm
            Dim c As Double = 0.0
            Dim y As Double

            ' Go through all energies for convolution and integrate the individual current contributions.
            Do Until dE > MaxE And GaussianValue < MinGaussianValue

                ' calculate Gaussian value
                GaussianValue = cNumericFunctions.GaussPeak_Amplitude(dE, 0, BroadeningWidth, 0, 1)

                tmp = CurrentPRECALCCUDA(E - dE, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)
                tmp += CurrentPRECALCCUDA(E + dE, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)
                tmp *= GaussianValue

                ' Error correction
                y = tmp - c
                tmp = ConvolutedCurrent + y
                c = (tmp - ConvolutedCurrent) - y

                ConvolutedCurrent = tmp

                '######################################
                ' Sum up positive energy contribution
                dE += dE_BroadeningStepWidth

            Loop


            'For j As Integer = 0 To NmaxPos Step 1
            '    '######################################
            '    ' Sum up positive energy contribution
            '    dE = j * dE_BroadeningStepWidth
            '    tmp = cNumericFunctions.GaussPeak_Amplitude(dE, E, BroadeningWidth, 0, 1)

            '    tmp *= CurrentPRECALCCUDA(dE, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)

            '    ' Error correction
            '    y = tmp - c
            '    tmp = ConvolutedCurrent + y
            '    c = (tmp - ConvolutedCurrent) - y

            '    ConvolutedCurrent = tmp
            'Next

            'c = 0.0
            'For j As Integer = 0 To NmaxNeg Step 1
            '    '######################################
            '    ' Sum up negative energy contribution
            '    dE = -dE_BroadeningStepWidth * (j + 1)
            '    tmp = CurrentPRECALCCUDA(dE, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)
            '    tmp *= cNumericFunctions.GaussPeak_Amplitude(dE, E, BroadeningWidth, 0, 1)

            '    ' Error correction
            '    y = tmp - c
            '    tmp = ConvolutedCurrent + y
            '    c = (tmp - ConvolutedCurrent) - y

            '    ConvolutedCurrent = tmp
            'Next
        End If

        ' Return the normalized current
        Return ConvolutedCurrent



        'Return 0

        Dim result, F, integ, dCurr As Double
        Dim bufPosEnergy, bufNegEnergy As Double
        Dim buf3, buf4 As Double
        Dim IntegrationBoundary As Double = MIN_DIFFERENTIAL * dE_BroadeningStepWidth

        ' save time, if the broadening is smaller than the calculation-step-size,
        ' then just use the input-array.
        If BroadeningWidth <= dE_BroadeningStepWidth Then
            result = CurrentPRECALCCUDA(E, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)
        Else
            ' Numerical Integration to convolute the current with the
            ' specified broadening peak. Convolution may take some time
            ' until the integrals converge. Especially the Lorentzian!
            result = 0.0

            If E > 0 Then
                ' integrate until the broadening converges
                F = dE_BroadeningStepWidth
                bufPosEnergy = E - F
                bufNegEnergy = E + F
                dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)
                Select Case ConvolutionFunction
                    Case ConvolutionFunctions.Gauss
                        buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
                        integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                    Case ConvolutionFunctions.Lorentz
                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                        integ = dCurr * (1.0 / buf3 + 1.0 / buf4)
                End Select

                ' Stop, if the integrand converges to small values.
                ' This needs some time for Lorentzian functions.
                While MathAbs(F) < Math.Max(MathAbs(dVBiasEnd_POS - dVBiasStart_POS), MathAbs(dVBiasEnd_NEG - dVBiasStart_NEG)) ' cNumericFunctions.MathAbs(integ) > IntegrationBoundary Or F < 1.1 * cNumericFunctions.MathAbs(E)
                    bufPosEnergy = E - F
                    bufNegEnergy = E + F
                    dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)

                    Select Case ConvolutionFunction
                        Case ConvolutionFunctions.Gauss
                            integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                        Case ConvolutionFunctions.Lorentz
                            buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                            buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                            integ = dCurr * (1 / buf3 + 1 / buf4)
                    End Select

                    result += integ
                    F += dE_BroadeningStepWidth
                End While
            Else
                ' integrate until the broadening converges
                ' Stop, if the integrand converges to small values.
                ' This needs some time for Lorentzian functions.
                F = -dE_BroadeningStepWidth
                bufPosEnergy = E - F
                bufNegEnergy = E + F
                dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)
                Select Case ConvolutionFunction
                    Case ConvolutionFunctions.Gauss
                        buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
                        integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                    Case ConvolutionFunctions.Lorentz
                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                        integ = dCurr * (1.0 / buf3 + 1.0 / buf4)
                End Select

                While MathAbs(F) < Math.Max(MathAbs(dVBiasEnd_POS - dVBiasStart_POS), MathAbs(dVBiasEnd_NEG - dVBiasStart_NEG)) ' MathAbs(integ) > IntegrationBoundary Or MathAbs(F) < 1.1 * MathAbs(E)
                    bufPosEnergy = E - F
                    bufNegEnergy = E + F
                    dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dE_InterpolationStepWidth)

                    Select Case ConvolutionFunction
                        Case ConvolutionFunctions.Gauss
                            integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                        Case ConvolutionFunctions.Lorentz
                            buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                            buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                            integ = dCurr * (1 / buf3 + 1 / buf4)
                    End Select

                    result += integ
                    F -= dE_BroadeningStepWidth
                End While
            End If

            '  // ** Vorfaktor aufmultiplizieren; **
            Select Case ConvolutionFunction
                Case ConvolutionFunctions.Gauss
                    result *= 1.0 / (Math.Sqrt(MathNet.Numerics.Constants.Pi2) * BroadeningWidth)
                Case ConvolutionFunctions.Lorentz
                    result *= 2.0 * cNumericFunctions.MathAbs(BroadeningWidth) / MathNet.Numerics.Constants.Pi
            End Select

            '  // ** Breite dE-Intervall aufmultiplizieren; **
            result *= dE_BroadeningStepWidth
        End If

        Return result
    End Function

    ''' <summary>
    ''' Calculate the Convolution of the current using the CUDAfy interface
    ''' </summary>
    <Cudafy>
    Public Shared Sub CurrentConvolutionCudaKernel(ByVal thread As Cudafy.GThread,
                                                   ByVal InArrayX_POS As Double(),
                                                   ByVal InArrayX_NEG As Double(),
                                                   ByVal CurrentIntegralCache_POS As Double(),
                                                   ByVal CurrentIntegralCache_NEG As Double(),
                                                   ByVal OutArrayY_POS As Double(),
                                                   ByVal OutArrayY_NEG As Double(),
                                                   ByVal ConvolutionFunction As Integer,
                                                   ByVal BroadeningWidth As Double,
                                                   ByVal dVBiasStart_POS As Double,
                                                   ByVal dVBiasStart_NEG As Double,
                                                   ByVal dVBiasEnd_POS As Double,
                                                   ByVal dVBiasEnd_NEG As Double,
                                                   ByVal dE_BroadeningStepWidth As Double,
                                                   ByVal dE_InterpolationStepWidth As Double)
        Dim tid As Integer = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x
        If tid < InArrayX_POS.Length Then
            OutArrayY_POS(tid) = ConvolveFunctionCUDA(InArrayX_POS(tid), CurrentIntegralCache_POS, CurrentIntegralCache_NEG,
                                                      ConvolutionFunction, BroadeningWidth, dVBiasStart_POS, dVBiasStart_NEG,
                                                      dVBiasEnd_POS, dVBiasEnd_NEG,
                                                      dE_BroadeningStepWidth, dE_InterpolationStepWidth)
        End If
        If tid < InArrayX_NEG.Length Then
            OutArrayY_NEG(tid) = ConvolveFunctionCUDA(InArrayX_NEG(tid), CurrentIntegralCache_POS, CurrentIntegralCache_NEG,
                                                      ConvolutionFunction, BroadeningWidth, dVBiasStart_POS, dVBiasStart_NEG,
                                                      dVBiasEnd_POS, dVBiasEnd_NEG,
                                                      dE_BroadeningStepWidth, dE_InterpolationStepWidth)
        End If
    End Sub
#End Region

#Region "Fit Function and CUDA Kernel for the precalculation of the current."
    ''' <summary>
    ''' Calculate the Current using the CUDAfy interface
    ''' </summary>
    <Cudafy>
    Public Shared Sub CurrentCudaKernel(ByVal thread As Cudafy.GThread,
                                        ByVal E_In As Double(),
                                        ByVal Current_Out As Double(),
                                        ByVal Identifiers As Integer(),
                                        ByVal Values As Double(),
                                        ByVal MaxEIntegrationRangePos As Double,
                                        ByVal MaxEIntegrationRangeNeg As Double,
                                        ByVal ConvolutionIntegrationStepSize As Double)
        Dim tid As Integer = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x
        If tid < E_In.Length Then
            Current_Out(tid) = FuncICUDA(E_In(tid), Identifiers, Values, MaxEIntegrationRangePos, MaxEIntegrationRangeNeg, ConvolutionIntegrationStepSize)
        End If
    End Sub

    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' PUBLIC VERSION, USING THE MEMBERS OF THE CLASS
    ''' </summary>
    Public Function CurrentPRECALC(ByVal VBias As Double) As Double
        ' Return the shared cuda version.
        Return CurrentPRECALCCUDA(VBias,
                                  Me.CurrentIntegralCache_POS, Me.CurrentIntegralCache_NEG,
                                  Me.dVBiasStart_Pos, Me.dVBiasStart_Neg, dE_InterpolationStepWidth)
    End Function

    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' CUDA VERSION, USING SEPARATE GIVEN ARRAYS
    ''' </summary>
    <Cudafy>
    Protected Shared Function CurrentPRECALCCUDA(ByVal VBias As Double,
                                                 ByVal CurrentIntegralCache_POS As Double(),
                                                 ByVal CurrentIntegralCache_NEG As Double(),
                                                 ByVal dVBiasStart_Pos As Double,
                                                 ByVal dVBiasStart_Neg As Double,
                                                 ByVal dE_InterpolationStepWidth As Double) As Double
        ' Reference to the cache arrays
        Dim CurrentCache As Double()
        Dim CurrentCacheOpposite As Double()

        ' reference to the next element
        Dim n As Double

        ' depending on the value use different arrays.
        If VBias >= 0 Then
            ' get the index n of the POS array from the entered bias
            n = (VBias - dVBiasStart_Pos) / dE_InterpolationStepWidth

            ' Assign the arrays
            CurrentCache = CurrentIntegralCache_POS
            CurrentCacheOpposite = CurrentIntegralCache_NEG
        Else
            ' get the index n of the NEG array from the entered bias
            n = -(VBias - dVBiasStart_Neg) / dE_InterpolationStepWidth

            ' Assign the arrays
            CurrentCache = CurrentIntegralCache_NEG
            CurrentCacheOpposite = CurrentIntegralCache_POS
        End If

        ' Save an interger version of the N
        Dim IntN As Integer = CInt(n)
        Dim PointsToTakeForInterpolation As Integer = 15

        ' // Now distinguish between different cases.
        ' /- Arraylength not matching
        ' /- interpolation between requested points, etc.
        If CurrentCache.Length = 0 Then
            ' Some settings are wrong. Just return 0.
            Return 0
        ElseIf CurrentCache.Length = 1 Or n < 0 Then
            ' Some settings are not good... or something went wrong!
            ' So just return the first value of the array.
            Return CurrentCache(0)
        ElseIf n > CurrentCache.Length - PointsToTakeForInterpolation - 1 Then
            ' The requested point has not been precalculated.
            ' So we have to interpolate the values we want to show.

            ' Let's take the last 5% of points for interpolation
            Dim InterpolationPoints As Integer = CInt(CurrentCache.Length * 0.05)
            If InterpolationPoints > 1 Then
                Dim InterpolationArrayX(InterpolationPoints - 1) As Double
                Dim InterpolationArrayY(InterpolationPoints - 1) As Double
                For i As Integer = InterpolationPoints To 1 Step -1
                    InterpolationArrayX(i - 1) = CurrentCache.Length - i
                    InterpolationArrayY(i - 1) = CurrentCache(CurrentCache.Length - i)
                Next

                Return cNumericalMethods.SplineInterpolationNative(InterpolationArrayX, InterpolationArrayY, n)
            Else
                ' Not enough points present, so just return the last value.
                Return CurrentCache(CurrentCache.Length - 1)
            End If
        ElseIf IntN = n Then
            ' If the value matches directly a precalculated value,
            ' then just return the precalculated value immediatelly.
            Return CurrentCache(IntN)
        Else
            ' the requested value lies between two points,
            ' so lets interpolate between these two points the values.

            ' Let's take 5 points to the right and left for interpolation in total.
            Dim InterpolationPointsRight As Integer = PointsToTakeForInterpolation
            Dim InterpolationPointsLeft As Integer = PointsToTakeForInterpolation
            Dim InterpolationPointsConnectionLeft As Integer = 0
            Dim NEffective As Double = n - IntN

            ' Check, if we need the other array? (around 0 necessary)
            If IntN < PointsToTakeForInterpolation Then
                InterpolationPointsLeft = IntN
                InterpolationPointsConnectionLeft = PointsToTakeForInterpolation - IntN

                If CurrentCacheOpposite.Length < InterpolationPointsConnectionLeft Then InterpolationPointsConnectionLeft = 0
            End If

            Dim InterpolationArrayMaxIndex As Integer = InterpolationPointsRight + InterpolationPointsLeft + InterpolationPointsConnectionLeft - 1
            Dim InterpolationArrayX(InterpolationArrayMaxIndex) As Double
            Dim InterpolationArrayY(InterpolationArrayMaxIndex) As Double

            Dim ArrayCounter As Integer = 0
            If InterpolationPointsConnectionLeft > 0 Then
                For i As Integer = 0 To InterpolationPointsConnectionLeft - 1 Step 1
                    InterpolationArrayX(ArrayCounter) = ArrayCounter
                    InterpolationArrayY(ArrayCounter) = CurrentCacheOpposite(i)

                    ArrayCounter += 1
                Next
            End If
            For i As Integer = 0 To InterpolationPointsLeft - 1 Step 1
                InterpolationArrayX(ArrayCounter) = ArrayCounter
                InterpolationArrayY(ArrayCounter) = CurrentCache(IntN - i)

                ArrayCounter += 1
            Next
            NEffective += ArrayCounter - 1
            For i As Integer = 1 To InterpolationPointsRight Step 1
                InterpolationArrayX(ArrayCounter) = ArrayCounter
                InterpolationArrayY(ArrayCounter) = CurrentCache(IntN + i)

                ArrayCounter += 1
            Next


            Return cNumericalMethods.SplineInterpolationNative(InterpolationArrayX, InterpolationArrayY, NEffective)

        End If
    End Function

    ''' <summary>
    ''' Fit-Function, depending on the Selected Fit-Data-Type
    ''' </summary>
    Public Function FitFunctionPRECALC(ByVal VBias As Double,
                                       ByRef Identifiers As Integer(),
                                       ByRef Values As Double()) As Double
        Dim VBiasEff As Double = sFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalXOffset, Identifiers, Values) + VBias
        Dim ReturnVal As Double = 0
        ' take the current or the dI/dV-value
        Select Case FitDataType
            Case FitFunctionType.I ' I(V)
                Dim I As Double = CurrentPRECALC(VBiasEff)
                ReturnVal = I
            Case FitFunctionType.dIdV ' dI/dV(V)
                Dim ILeft As Double = CurrentPRECALC((VBiasEff + dE_InterpolationStepWidth / 2))
                Dim IRight As Double = CurrentPRECALC((VBiasEff - dE_InterpolationStepWidth / 2))
                ReturnVal = (ILeft - IRight) / dE_InterpolationStepWidth
        End Select
        ' return the value shifted by the YOffset and stretched by the amplitude
        Return sFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYOffset, Identifiers, Values) + ReturnVal * sFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYStretch, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Fit-Function, depending on the Selected Fit-Data-Type
    ''' </summary>
    Public Function FitFunctionDirect(ByVal VBias As Double,
                                      ByRef Identifiers As Integer(),
                                      ByRef Values As Double()) As Double
        Dim ReturnVal As Double = 0
        Dim VBiasEff As Double = sFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalXOffset, Identifiers, Values) + VBias

        '#########################################################
        ' Sets additional properties before calculating the
        ' function value, i.e. inelastic channels
        Me.SetAdditionalProperties(Identifiers, Values)
        ' END InelasticChannels
        '#########################################################

        ' take the current or the dI/dV-value
        Select Case FitDataType
            Case FitFunctionType.I ' I(V)
                Dim I As Double = FuncI(VBiasEff, Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                ReturnVal = I
            Case FitFunctionType.dIdV ' dI/dV(V)
                Dim ILeft As Double = FuncI((VBiasEff + dE_InterpolationStepWidth / 2), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                Dim IRight As Double = FuncI((VBiasEff - dE_InterpolationStepWidth / 2), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                ReturnVal = (ILeft - IRight) / dE_InterpolationStepWidth
        End Select

        ' return the value shifted by the YOffset and stretched by the amplitude
        Return sFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYOffset, Identifiers, Values) + ReturnVal * sFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYStretch, Identifiers, Values)
    End Function

#End Region

#Region "Convolution Integral"
    ''' <summary>
    ''' Function to calculate the tunneling current I(Vbias)
    ''' </summary>
    <Cudafy>
    Public Shared Function FuncICUDA(ByVal VBias As Double,
                                     ByVal Identifiers As Integer(),
                                     ByVal Values As Double(),
                                     ByVal MaxEIntegrationRangePos As Double,
                                     ByVal MaxEIntegrationRangeNeg As Double,
                                     ByVal ConvolutionIntegrationStepSize As Double) As Double

        ' Create the current integration variable
        Dim I As Double = 0.0

        ' Get temporary variables
        Dim E As Double
        Dim IEC_E As Double
        Dim IEC_P As Double
        Dim IECCount As Integer = GetIECFromParameterArray_Count(Identifiers, Values)

        ' Get the maximum inelastic channel energy
        Dim MaxInelasticChannelEnergy As Double = 0
        For iIEC As Integer = 0 To IECCount - 1 Step 1
            IEC_E = MathAbs(GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Energy))
            If MaxInelasticChannelEnergy < IEC_E Then
                MaxInelasticChannelEnergy = IEC_E
            End If
        Next

        ' Get the maximum number of iterations in the energy-range.
        Dim NmaxPos As Integer = CInt((MaxEIntegrationRangePos + MaxInelasticChannelEnergy * 1.9) / ConvolutionIntegrationStepSize)
        Dim NmaxNeg As Integer = CInt((-MaxEIntegrationRangeNeg + MaxInelasticChannelEnergy * 1.9) / ConvolutionIntegrationStepSize)

        ' Go through all energies for convolution and integrate the individual current contributions.
        For j As Integer = 0 To NmaxPos Step 1
            '######################################
            ' Sum up positive energy contribution
            E = j * ConvolutionIntegrationStepSize
            I += IntegrandECCUDA(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                I += IEC_P * IntegrandIECCUDA(E + IEC_E,
                                              E - IEC_E,
                                              E + VBias,
                                              Identifiers, Values)
            Next
            '#####################################
        Next
        For j As Integer = 0 To NmaxNeg Step 1
            '######################################
            ' Sum up negative energy contribution
            E = -ConvolutionIntegrationStepSize * (j + 1)
            I += IntegrandECCUDA(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                I += IEC_P * IntegrandIECCUDA(E + IEC_E,
                                              E - IEC_E,
                                              E + VBias,
                                              Identifiers, Values)
            Next
            '#####################################
        Next

        ' Return the normalized current
        Return ConvolutionIntegrationStepSize * I
    End Function

    ''' <summary>
    ''' Function to calculate the tunneling current I(Vbias)
    ''' </summary>
    Public Function FuncI(ByVal VBias As Double,
                          ByRef Identifiers As Integer(),
                          ByRef Values As Double(),
                          ByVal MaxEIntegrationRangePos As Double,
                          ByVal MaxEIntegrationRangeNeg As Double,
                          ByVal ConvolutionIntegrationStepSize As Double) As Double

        'Return FuncICUDA(VBias, Identifiers, Values, MaxEIntegrationRangePos, MaxEIntegrationRangeNeg, ConvolutionIntegrationStepSize)

        ' Create the current integration variable
        Dim I As Double = 0.0

        ' Get temporary variables
        Dim E As Double
        Dim IEC_E As Double
        Dim IEC_P As Double
        Dim IECCount As Integer = GetIECFromParameterArray_Count(Identifiers, Values)

        ' Get the maximum inelastic channel energy
        Dim MaxInelasticChannelEnergy As Double = 0
        For iIEC As Integer = 0 To IECCount - 1 Step 1
            IEC_E = MathAbs(GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Energy))
            If MaxInelasticChannelEnergy < IEC_E Then
                MaxInelasticChannelEnergy = IEC_E
            End If
        Next

        ' Get the maximum number of iterations in the energy-range.
        Dim NmaxPos As Integer = CInt((MaxEIntegrationRangePos + MaxInelasticChannelEnergy * 1.9) / ConvolutionIntegrationStepSize)
        Dim NmaxNeg As Integer = CInt((-MaxEIntegrationRangeNeg + MaxInelasticChannelEnergy * 1.9) / ConvolutionIntegrationStepSize)

        ' Go through all energies for convolution and integrate the individual current contributions.
        For j As Integer = 0 To NmaxPos Step 1
            '######################################
            ' Sum up positive energy contribution
            E = j * ConvolutionIntegrationStepSize
            I += IntegrandEC(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                I += IEC_P * IntegrandIEC(E + IEC_E,
                                          E - IEC_E,
                                          E + VBias,
                                          Identifiers, Values)
            Next
            '#####################################
        Next
        For j As Integer = 0 To NmaxNeg Step 1
            '######################################
            ' Sum up negative energy contribution
            E = -ConvolutionIntegrationStepSize * (j + 1)
            I += IntegrandEC(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, cFitFunction_TipSampleConvolutionBase.IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                I += IEC_P * IntegrandIEC(E + IEC_E,
                                          E - IEC_E,
                                          E + VBias,
                                          Identifiers, Values)
            Next
            '#####################################
        Next

        ' Return the normalized current
        Return ConvolutionIntegrationStepSize * I
    End Function

#End Region

#Region "Fermi-Function - CUDA compatible"
    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet die Fermiefunktion (in eV Units)                    *
    ''' // *                                                               *
    ''' // *      f(E,T) = 1 / ( exp(E/kT) + 1 )                           *
    ''' // *****************************************************************
    ''' 
    ''' Function supports CUDA!
    ''' </summary>
    <Cudafy>
    Public Shared Function FermiF_eV(ByVal E As Double,
                                     ByVal T As Double) As Double
        '// ** Berechnungsvariablen; **
        Dim f, expo As Double

        '// ** Fallunterscheidung nach T <= 0.0 K oder T > 0.0 K; **
        '// ** (Division durch Null abfangen;) **
        If T <= 0 Then
            If E < 0D Then
                f = 1D
            Else
                f = 0D
            End If
        Else
            '  // ** Berechne Exponent der e-Funktion; **
            expo = E / (cConstants.kB_eV * T) ' * 1000) ' ?????

            '  // ** Berechne Fermi-Funktion; Fange Ueberlauf der e-Funktion ab; **
            If expo < MIN_EXP Or expo > MAX_EXP Then
                If expo < MIN_EXP Then
                    f = 1.0
                Else
                    f = 0.0
                End If
            Else
                f = 1.0 / (Math.Exp(expo) + 1.0)
            End If
        End If

        Return f
    End Function
#End Region

End Class
