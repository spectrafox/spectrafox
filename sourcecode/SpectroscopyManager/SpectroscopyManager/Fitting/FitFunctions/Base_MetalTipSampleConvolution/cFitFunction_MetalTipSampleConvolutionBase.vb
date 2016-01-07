Imports System.Threading.Tasks
Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports SpectroscopyManager.cNumericFunctions

#Const USEPARALLEL = 1

Public MustInherit Class cFitFunction_MetalTipSampleConvolutionBase
    Inherits cFitFunction

#Region "initialize constants"

    ''' <summary>
    ''' Cutoff for evaluation of exponential function:
    ''' </summary>
    Public Const MAX_EXP As Double = 100D

    ''' <summary>
    ''' Cutoff for evaluation of exponential function.
    ''' </summary>
    Public Const MIN_EXP As Double = -100D

#End Region

#Region "initialize fit function properties determining the calculation procedure"
    ''' <summary>
    ''' Energy sampling size in [eV]
    ''' Used for calculation of the dIdV integral.
    ''' This is the step size between which the dIdV is summed up.
    ''' (irrational for minizing numerical artifacts)
    ''' </summary>
    Public Property ConvolutionIntegrationStepSize As Double = 0.000003090169943749474241023D

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
        CalculateForBiasRangeUpperE = HigherValue
        CalculateForBiasRangeLowerE = LowerValue
        ConvolutionIntegralE_POS = HigherValue
        ConvolutionIntegralE_NEG = LowerValue
        MyBase.ChangeFitRangeX(LowerValue, HigherValue)

        If Not LastParameterIdentifiers Is Nothing And Not LastParameterValues Is Nothing Then
            Me.Precalculate_dIdV(LastParameterIdentifiers, LastParameterValues)
        End If
    End Sub

    ''' <summary>
    ''' Forces the precalculation of the values.
    ''' </summary>
    Public Sub ForceNewPrecalculation()
        If Not LastParameterIdentifiers Is Nothing And Not LastParameterValues Is Nothing Then
            Me.Precalculate_dIdV(LastParameterIdentifiers, LastParameterValues)
        End If
    End Sub

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
            Dim FitParameterEnergy As cFitParameter
            Dim FitParameterProbability As cFitParameter

            Dim IEC As InelasticChannel = Me.InelasticChannels(i)
            ' create Fit-Parameter
            FitParameterEnergy = New cFitParameter("IEC " & i.ToString & " Energy", IEC.Energy, IEC.EnergyFixed, "IEC " & i.ToString & " Energy")
            FitParameterProbability = New cFitParameter("IEC " & i.ToString & " Probability", IEC.Probability, IEC.ProbabilityFixed, "IEC " & i.ToString & " Probability")

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
    Public Shared Function GetIECParameterIdentifierFromPeakIndex(ByVal PeakIndex As Integer,
                                                                  ByVal ParameterIdentifier As Integer) As Integer
        Return (PeakIndex * IECIdentifierOffset) + ParameterIdentifier
    End Function

    ''' <summary>
    ''' This functions gets the value of a IEC parameter from the
    ''' input of two arrays of fit-parameters.
    ''' </summary>
    <Cudafy>
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
        Return 1
    End Function

    ''' <summary>
    ''' Must-overrides that have to be defined by the child functions.
    ''' Implemented as SHARED to be able to use CUDA!
    ''' 
    ''' Does nothing! Override it, and declare it for CUDAfying!!!!
    ''' </summary>
    Public Shared Function TipDOSDerivativeCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
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
    ''' Must-overrides that have to be defined by the child functions.
    ''' Implemented as SHARED to be able to use CUDA!
    ''' 
    ''' Does nothing! Override it, and declare it for CUDAfying!!!!
    ''' </summary>
    Public MustOverride Function TipDOSDerivative(ByRef E As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double

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
        Me.FitParameters.Add(FitParameterIdentifier.GlobalXOffset, New cFitParameter("Xoffset", 0, False, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalXOffset))
        Me.FitParameters.Add(FitParameterIdentifier.GlobalYOffset, New cFitParameter("Yoffset", 0, False, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalYOffset))
        Me.FitParameters.Add(FitParameterIdentifier.GlobalYStretch, New cFitParameter("Ystretch", 1, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalYStretch))
        Me.FitParameters.Add(FitParameterIdentifier.T_tip, New cFitParameter("TipTemperature", 1.19, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_T_tip))
        Me.FitParameters.Add(FitParameterIdentifier.T_sample, New cFitParameter("SampleTemperature", 1.19, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_T_sample))
        Me.FitParameters.Add(FitParameterIdentifier.SystemBroadening, New cFitParameter("SystemBroadening", 0.000035, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_SystemBroadening))
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
        Dim Part1 As Double = (FermiF_eV(E, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values)) - FermiF_eV(E + eV, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))) * SampleDOSCUDA(E, Identifiers, Values) * TipDOSDerivativeCUDA(E + eV, Identifiers, Values)
        Dim Part2 As Double = FermiDerivativeF_eV(E + eV, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values)) * SampleDOSCUDA(E, Identifiers, Values) * TipDOSCUDA(E + eV, Identifiers, Values)
        Return Part1 + Part2
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
        Dim FermiTipEPEIEC As Double = FermiF_eV(EPlusEIEC, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiTipEMEIEC As Double = FermiF_eV(EMinusEIEC, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiSampleEPV As Double = FermiF_eV(EPlusBias, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values))

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
        Dim Part1 As Double = (FermiF_eV(E, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values)) - FermiF_eV(E + eV, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))) * SampleDOS(E, Identifiers, Values) * TipDOSDerivative(E + eV, Identifiers, Values)
        Dim Part2 As Double = FermiDerivativeF_eV(E + eV, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values)) * SampleDOS(E, Identifiers, Values) * TipDOSCUDA(E + eV, Identifiers, Values)
        Return Part1 + Part2
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
        Dim FermiTipEPEIEC As Double = FermiF_eV(EPlusEIEC, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiTipEMEIEC As Double = FermiF_eV(EMinusEIEC, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))
        Dim FermiSampleEPV As Double = FermiF_eV(EPlusBias, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values))

        '// ** Berechnung und Rueckgabe des Integrand; **
        '// ** Benutze Vorausberechnete Funktionen um Rechenzeit zu sparen; **
        Return SampleDOS(EPlusBias, Identifiers, Values) _
                * (TipDOS(EPlusEIEC, Identifiers, Values) * FermiTipEPEIEC * (1.0 - FermiSampleEPV) _
                - TipDOS(EMinusEIEC, Identifiers, Values) * (1.0 - FermiTipEMEIEC) * FermiSampleEPV)
    End Function


    ''' <summary>
    '''// *****************************************************************
    '''// * Abgeleiteter Integrand  eines  elastischen Kanals (EC) des  Tunnelinte- *
    '''// * grals zur Berechnung des dI(V)/dV mit metallischer Spitze:                         *
    '''// *                                                               *
    '''// * Intgr_EC_dIdV = DOS_s(E) * d[-f(E+eV)]/dV *
    '''// *                                                               *
    '''// * (siehe Aufschriebe)                                           *
    '''// *****************************************************************
    ''' </summary>
    Public Function IntegrandECdIdV(ByVal E As Double,
                               ByVal eV As Double,
                               ByRef Identifiers As Integer(),
                               ByRef Values As Double()) As Double
        Return (FermiF_eV(E, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_sample, Identifiers, Values)) - FermiF_eV(E + eV, cFitParameter.GetValueForIdentifier(FitParameterIdentifier.T_tip, Identifiers, Values))) * (SampleDOS(E, Identifiers, Values) * TipDOS(E + eV, Identifiers, Values))
    End Function
#End Region

#Region "Fit-Function Interface and caching algorithm to calculate the current"

    ''' <summary>
    ''' dIdV-precalculation interpolation-interval-width:
    ''' is responsible for the number of points that is need to be calculated,
    ''' and therefore the speed of the calculation, and the accuracy.
    ''' </summary>
    Public dEInterpolation_dIdVPrecalculation As Double = 0.000001

    ''' <summary>
    ''' Cache storage for the current integral.
    ''' </summary>
    Private dIdVIntegralCache_POS As Double()
    Private dIdVIntegralCache_NEG As Double()

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

            Me.Precalculate_dIdV(Identifiers, Values)

        End If

        ' Finally return the cached FitFunction
        Return Me.FitFunctionPRECALC(x, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Precalculates the dIdV cache.
    ''' </summary>
    Protected Sub Precalculate_dIdV(ByVal Identifiers As Integer(), ByVal Values As Double())

        ' save the last used parameter-set
        ReDim LastParameterValues(Values.Length - 1)
        ReDim LastParameterIdentifiers(Identifiers.Length - 1)
        Values.CopyTo(LastParameterValues, 0)
        Identifiers.CopyTo(LastParameterIdentifiers, 0)

        '#########################################
        ' Precalculate the dIdV and broaden it
        ' for the selected interval and use the
        ' GPGPU to speed up everything.

        ' Initialize variables
        Dim idIdVCachePoints_Pos As Integer = 0
        dVBiasStart_Pos = 0
        dVBiasEnd_Pos = 0
        Dim idIdVCachePoints_Neg As Integer = 0
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
            UpperBiasEnergy -= 0.5 * cFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        Else
            UpperBiasEnergy += 0.5 * cFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        End If
        If LowerBiasEnergy > 0 Then
            LowerBiasEnergy -= 0.5 * cFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        Else
            LowerBiasEnergy += 0.5 * cFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values)
        End If

        ' Not calculate the cache of the current: 
        ' for this: calculate the number of points to cache
        ' and the borders to calculate the current in between
        If UpperBiasEnergy > 0 And LowerBiasEnergy > 0 Then
            ' only positive bias values
            '###########################
            idIdVCachePoints_Pos = CInt((UpperBiasEnergy - LowerBiasEnergy) / dEInterpolation_dIdVPrecalculation)
            dVBiasEnd_Pos = UpperBiasEnergy
            dVBiasStart_Pos = LowerBiasEnergy
            Me.dIdVIntegralCache_NEG = New Double() {}
        ElseIf UpperBiasEnergy < 0 And LowerBiasEnergy < 0 Then
            ' only negative bias values
            '###########################
            idIdVCachePoints_Neg = -CInt((LowerBiasEnergy - UpperBiasEnergy) / dEInterpolation_dIdVPrecalculation)
            dVBiasEnd_Neg = LowerBiasEnergy
            dVBiasStart_Neg = UpperBiasEnergy
            Me.dIdVIntegralCache_POS = New Double() {}
        Else
            ' both, positive and negative bias values.
            '##########################################
            idIdVCachePoints_Pos = Math.Abs(CInt(UpperBiasEnergy / dEInterpolation_dIdVPrecalculation))
            idIdVCachePoints_Neg = Math.Abs(CInt(LowerBiasEnergy / dEInterpolation_dIdVPrecalculation))
            dVBiasStart_Neg = 0
            dVBiasStart_Pos = 0

            dVBiasEnd_Pos = UpperBiasEnergy
            dVBiasEnd_Neg = LowerBiasEnergy
        End If

        ' Redimension the cache arrays
        If Me.dIdVIntegralCache_POS Is Nothing And idIdVCachePoints_Pos > 0 Then
            ReDim Me.dIdVIntegralCache_POS(idIdVCachePoints_Pos - 1)
        ElseIf Me.dIdVIntegralCache_POS.Length <> idIdVCachePoints_Pos And idIdVCachePoints_Pos > 0 Then
            ReDim Me.dIdVIntegralCache_POS(idIdVCachePoints_Pos - 1)
        End If
        If Me.dIdVIntegralCache_NEG Is Nothing And idIdVCachePoints_Neg > 0 Then
            ReDim Me.dIdVIntegralCache_NEG(idIdVCachePoints_Neg - 1)
        ElseIf Me.dIdVIntegralCache_NEG.Length <> idIdVCachePoints_Neg And idIdVCachePoints_Neg > 0 Then
            ReDim Me.dIdVIntegralCache_NEG(idIdVCachePoints_Neg - 1)
        End If

        ' Create the XArray to calculate the current.
        Dim dIdVIntegralX_POS As Double() = New Double() {}
        Dim dIdVIntegralX_NEG As Double() = New Double() {}
        If idIdVCachePoints_Pos > 0 Then
            ReDim dIdVIntegralX_POS(idIdVCachePoints_Pos - 1)
        End If
        If idIdVCachePoints_Neg > 0 Then
            ReDim dIdVIntegralX_NEG(idIdVCachePoints_Neg - 1)
        End If

        '// fill the arrays on the CPU
        ' include the 0 in the positive array
        If idIdVCachePoints_Pos > 0 Then
            Parallel.For(0, idIdVCachePoints_Pos, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                dIdVIntegralX_POS(j) = dVBiasStart_Pos + (dEInterpolation_dIdVPrecalculation * j)
                                                                            End Sub)
        End If
        If idIdVCachePoints_Neg > 0 Then
            Parallel.For(0, idIdVCachePoints_Neg, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                dIdVIntegralX_NEG(j) = dVBiasStart_Neg - (dEInterpolation_dIdVPrecalculation * j)
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
            CudaCompileClasses.AddRange({GetType(cFitFunction_MetalTipSampleConvolutionBase), GetType(cNumericFunctions), GetType(cFitParameter)})

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

            If idIdVCachePoints_Pos <= 0 Then
                ReDim dIdVIntegralX_POS(1)
                ReDim dIdVIntegralCache_POS(1)
            End If
            If idIdVCachePoints_Neg <= 0 Then
                ReDim dIdVIntegralX_NEG(1)
                ReDim dIdVIntegralCache_NEG(1)
            End If

            '// Get the storage on the CPU.
            Dim dev_dIdVIntegralX_POS As Double() = CUDAGPU.Allocate(Of Double)(dIdVIntegralX_POS)
            '// copy the array to the GPU
            CUDAGPU.CopyToDevice(dIdVIntegralX_POS, dev_dIdVIntegralX_POS)
            ' Get the output storage on the GPU
            Dim dev_dIdVIntegralCache_POS As Double() = CUDAGPU.Allocate(Of Double)(dIdVIntegralCache_POS)
            If idIdVCachePoints_Pos > 0 Then
                '// launch dIdV-Integration on N threads
                CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid(idIdVCachePoints_Pos, ThreadsPerBlock),
                               ThreadsPerBlock,
                               "dIdVCudaKernel",
                               dev_dIdVIntegralX_POS, dev_dIdVIntegralCache_POS,
                               dev_Identifiers, dev_Values,
                               Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, Me.ConvolutionIntegrationStepSize)
                ' Copy cached current arrays back to the CPU
                'CUDAGPU.CopyFromDevice(dev_CurrentIntegralCache_POS, CurrentIntegralCache_POS) ' WAS DONE BEFORE THE CONVOLUTION WITH A GAUSSIAN!
            End If

            '// Get the storage on the CPU.
            Dim dev_dIdVIntegralX_NEG As Double() = CUDAGPU.Allocate(Of Double)(dIdVIntegralX_NEG)
            '// copy the array to the GPU
            CUDAGPU.CopyToDevice(dIdVIntegralX_NEG, dev_dIdVIntegralX_NEG)
            ' Get the output storage on the GPU
            Dim dev_dIdVIntegralCache_NEG As Double() = CUDAGPU.Allocate(Of Double)(dIdVIntegralCache_NEG)

            If idIdVCachePoints_Neg > 0 Then
                '// launch Current-Integration on N threads
                CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid(idIdVCachePoints_Neg, ThreadsPerBlock),
                               ThreadsPerBlock,
                               "dIdVCudaKernel",
                               dev_dIdVIntegralX_NEG, dev_dIdVIntegralCache_NEG,
                               dev_Identifiers, dev_Values,
                               Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, Me.ConvolutionIntegrationStepSize)
                ' Copy cached current arrays back to the CPU
                'CUDAGPU.CopyFromDevice(dev_CurrentIntegralCache_NEG, CurrentIntegralCache_NEG) ' WAS DONE BEFORE THE CONVOLUTION WITH A GAUSSIAN!
            End If

            ' Convolute the current with a gaussian afterwards, to broaden it.
            Dim dev_dIdVConvolutionCache_POS As Double() = CUDAGPU.Allocate(Of Double)(dIdVIntegralCache_POS)
            Dim dev_dIdVConvolutionCache_NEG As Double() = CUDAGPU.Allocate(Of Double)(dIdVIntegralCache_NEG)
            CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid(Math.Max(idIdVCachePoints_Pos, idIdVCachePoints_Neg), ThreadsPerBlock),
                           ThreadsPerBlock,
                           "dIdVConvolutionCudaKernel",
                           dev_dIdVIntegralX_POS, dev_dIdVIntegralX_NEG,
                           dev_dIdVIntegralCache_POS, dev_dIdVIntegralCache_NEG,
                           dev_dIdVConvolutionCache_POS, dev_dIdVConvolutionCache_NEG,
                           CInt(Me.ConvolutionFunction), cFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values),
                           Me.dVBiasStart_Pos, Me.dVBiasStart_Neg,
                           dE_BroadeningStepWidth, dEInterpolation_dIdVPrecalculation)
            ' Copy the convoluted result back to the CPU
            CUDAGPU.CopyFromDevice(dev_dIdVConvolutionCache_POS, dIdVIntegralCache_POS)
            CUDAGPU.CopyFromDevice(dev_dIdVConvolutionCache_NEG, dIdVIntegralCache_NEG)

            ' Finally free the GPU-memory
            CUDAGPU.FreeAll()
        Else
            ' NON CUDA VERSION,
            ' but using .NET parallel library.
            '##################################
#If USEPARALLEL = 1 Then
            If idIdVCachePoints_Pos > 0 Then
                Parallel.For(0, idIdVCachePoints_Pos, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                    dIdVIntegralCache_POS(j) = FuncdIdV(dIdVIntegralX_POS(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                                                                                End Sub)
                ' Convolute the dIdV with a gaussian afterwards, to broaden it.
                Dim ConvoluteddIdV_POS(dIdVIntegralX_POS.Length - 1) As Double
                Parallel.For(0, idIdVCachePoints_Pos, Me.MultiThreadingOptions, Sub(i As Integer)
                                                                                    ConvoluteddIdV_POS(i) = ConvolveFunction(dIdVIntegralX_POS(i), CInt(Me.ConvolutionFunction), cFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                                                                                End Sub)
                ConvoluteddIdV_POS.CopyTo(dIdVIntegralCache_POS, 0)
            End If
            If idIdVCachePoints_Neg > 0 Then
                Parallel.For(0, idIdVCachePoints_Neg, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                    dIdVIntegralCache_NEG(j) = FuncdIdV(dIdVIntegralX_NEG(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                                                                                End Sub)
                ' Convolute the current with a gaussian afterwards, to broaden it.
                Dim ConvoluteddIdV_NEG(dIdVIntegralX_NEG.Length - 1) As Double
                Parallel.For(0, idIdVCachePoints_Neg, Me.MultiThreadingOptions, Sub(i As Integer)
                                                                                    ConvoluteddIdV_NEG(i) = ConvolveFunction(dIdVIntegralX_NEG(i), CInt(Me.ConvolutionFunction), cFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                                                                                End Sub)
                ConvoluteddIdV_NEG.CopyTo(dIdVIntegralCache_NEG, 0)
            End If
#Else
            ' Convolute the dIdV with a gaussian afterwards, to broaden it.
            If idIdVCachePoints_Pos > 0 Then
                For j As Integer = 0 To idIdVCachePoints_Pos - 1 Step 1
                    dIdVIntegralCache_POS(j) = FuncdIdV(dIdVIntegralX_POS(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                Next
                ' Convolute the dIdV with a gaussian afterwards, to broaden it.
                Dim ConvoluteddIdV_POS(dIdVIntegralX_POS.Length - 1) As Double
                For i As Integer = 0 To dIdVIntegralX_POS.Length - 1 Step 1
                    ConvoluteddIdV_POS(i) = ConvolveFunction(dIdVIntegralX_POS(i), CInt(Me.ConvolutionFunction), sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                Next
                ConvoluteddIdV_POS.CopyTo(dIdVIntegralCache_POS, 0)
            End If
            If idIdVCachePoints_Neg > 0 Then
                For j As Integer = 0 To idIdVCachePoints_Neg - 1 Step 1
                    dIdVIntegralCache_NEG(j) = FuncdIdV(dIdVIntegralX_NEG(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                Next
                ' Convolute the current with a gaussian afterwards, to broaden it.
                Dim ConvoluteddIdV_NEG(dIdVIntegralX_NEG.Length - 1) As Double
                For i As Integer = 0 To dIdVIntegralX_NEG.Length - 1 Step 1
                    ConvoluteddIdV_NEG(i) = ConvolveFunction(dIdVIntegralX_NEG(i), CInt(Me.ConvolutionFunction), sFitParameter.GetValueForIdentifier(FitParameterIdentifier.SystemBroadening, Identifiers, Values))
                Next
                ConvoluteddIdV_NEG.CopyTo(dIdVIntegralCache_NEG, 0)
            End If
#End If

        End If
    End Sub
#End Region

#Region "Convolution of the current"

    ''' <summary>
    ''' Cutoff for Integrals, if the value becomes irrelevant small.
    ''' </summary>
    Public Const MIN_DIFFERENTIAL As Double = 0.001

    ''' <summary>
    ''' Energy sampling size in [eV] that is used to convolve the final result with a gaussian function.
    ''' Used for calculation of broadening.
    ''' </summary>
    Public Shared dE_BroadeningStepWidth As Double = 0.000003090169943749474241023D

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
                                    Me.dIdVIntegralCache_POS,
                                    Me.dIdVIntegralCache_NEG,
                                    ConvolutionFunction,
                                    BroadeningWidth,
                                    Me.dVBiasStart_Pos,
                                    Me.dVBiasStart_Neg,
                                    dE_BroadeningStepWidth,
                                    dEInterpolation_dIdVPrecalculation)
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
                                                   ByVal dE_BroadeningStepWidth As Double,
                                                   ByVal dEInterpolation_dIdVPrecalculation As Double) As Double
        Dim result, F, integ, dCurr As Double
        Dim bufPosEnergy, bufNegEnergy As Double
        Dim buf3, buf4 As Double
        Dim IntegrationBoundary As Double = MIN_DIFFERENTIAL * dE_BroadeningStepWidth

        ' save time, if the broadening is smaller than the calculation-step-size,
        ' then just use the input-array.
        If BroadeningWidth <= dE_BroadeningStepWidth Then
            result = dIdVPRECALCCUDA(E, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dEInterpolation_dIdVPrecalculation)
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
                dCurr = dIdVPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dEInterpolation_dIdVPrecalculation)
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
                While cNumericFunctions.MathAbs(integ) > IntegrationBoundary Or F < 1.1 * cNumericFunctions.MathAbs(E)
                    bufPosEnergy = E - F
                    bufNegEnergy = E + F
                    dCurr = dIdVPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dEInterpolation_dIdVPrecalculation)

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
                dCurr = dIdVPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dEInterpolation_dIdVPrecalculation)
                Select Case ConvolutionFunction
                    Case ConvolutionFunctions.Gauss
                        buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
                        integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                    Case ConvolutionFunctions.Lorentz
                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                        integ = dCurr * (1.0 / buf3 + 1.0 / buf4)
                End Select

                While MathAbs(integ) > IntegrationBoundary Or MathAbs(F) < 1.1 * MathAbs(E)
                    bufPosEnergy = E - F
                    bufNegEnergy = E + F
                    dCurr = dIdVPRECALCCUDA(F, CurrentIntegralCache_POS, CurrentIntegralCache_NEG, dVBiasStart_POS, dVBiasStart_NEG, dEInterpolation_dIdVPrecalculation)

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
    Public Shared Sub dIdVConvolutionCudaKernel(ByVal thread As Cudafy.GThread,
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
                                                ByVal dE_BroadeningStepWidth As Double,
                                                ByVal dEInterpolation_dIdVPrecalculation As Double)
        Dim tid As Integer = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x
        If tid < InArrayX_POS.Length Then
            OutArrayY_POS(tid) = ConvolveFunctionCUDA(InArrayX_POS(tid), CurrentIntegralCache_POS, CurrentIntegralCache_NEG,
                                                      ConvolutionFunction, BroadeningWidth, dVBiasStart_POS, dVBiasStart_NEG,
                                                      dE_BroadeningStepWidth, dEInterpolation_dIdVPrecalculation)
        End If
        If tid < InArrayX_NEG.Length Then
            OutArrayY_NEG(tid) = ConvolveFunctionCUDA(InArrayX_NEG(tid), CurrentIntegralCache_POS, CurrentIntegralCache_NEG,
                                                      ConvolutionFunction, BroadeningWidth, dVBiasStart_POS, dVBiasStart_NEG,
                                                      dE_BroadeningStepWidth, dEInterpolation_dIdVPrecalculation)
        End If
    End Sub
#End Region

#Region "Fit Function and CUDA Kernel for the precalculation of the dIdV."
    ''' <summary>
    ''' Calculate the dIdV using the CUDAfy interface
    ''' </summary>
    <Cudafy>
    Public Shared Sub dIdVCudaKernel(ByVal thread As Cudafy.GThread,
                                     ByVal E_In As Double(),
                                     ByVal dIdV_Out As Double(),
                                     ByVal Identifiers As Integer(),
                                     ByVal Values As Double(),
                                     ByVal MaxEIntegrationRangePos As Double,
                                     ByVal MaxEIntegrationRangeNeg As Double,
                                     ByVal ConvolutionIntegrationStepSize As Double)
        Dim tid As Integer = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x
        If tid < E_In.Length Then
            dIdV_Out(tid) = FuncdIdVCUDA(E_In(tid), Identifiers, Values, MaxEIntegrationRangePos, MaxEIntegrationRangeNeg, ConvolutionIntegrationStepSize)
        End If
    End Sub

    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' PUBLIC VERSION, USING THE MEMBERS OF THE CLASS
    ''' </summary>
    Public Function dIdVPRECALC(ByVal VBias As Double) As Double
        ' Return the shared cuda version.
        Return dIdVPRECALCCUDA(VBias,
                               Me.dIdVIntegralCache_POS, Me.dIdVIntegralCache_NEG,
                               Me.dVBiasStart_Pos, Me.dVBiasStart_Neg, dEInterpolation_dIdVPrecalculation)
    End Function

    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' CUDA VERSION, USING SEPARATE GIVEN ARRAYS
    ''' </summary>
    <Cudafy>
    Protected Shared Function dIdVPRECALCCUDA(ByVal VBias As Double,
                                              ByVal dIdVIntegralCache_POS As Double(),
                                              ByVal dIdVIntegralCache_NEG As Double(),
                                              ByVal dVBiasStart_Pos As Double,
                                              ByVal dVBiasStart_Neg As Double,
                                              ByVal dEInterpolation_dIdVPrecalculation As Double) As Double
        ' Reference to the cache array
        Dim dIdVCache As Double()

        ' reference to the next element
        Dim n As Integer
        Dim dPercentOfSlope As Double
        Dim ArrayLength As Integer

        ' depending on the value use different arrays.
        If VBias >= 0 Then
            dIdVCache = dIdVIntegralCache_POS
            ArrayLength = dIdVIntegralCache_POS.Length

            ' get the index n from the entered bias
            n = CInt((VBias - dVBiasStart_Pos) / dEInterpolation_dIdVPrecalculation)
            dPercentOfSlope = (VBias - dVBiasStart_Pos - n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
        Else
            dIdVCache = dIdVIntegralCache_NEG
            ArrayLength = dIdVIntegralCache_NEG.Length

            ' get the index n from the entered bias
            n = CInt(-(VBias - dVBiasStart_Neg) / dEInterpolation_dIdVPrecalculation)
            dPercentOfSlope = -(VBias - dVBiasStart_Neg + n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
        End If

        '  // ** Fallunterscheidung; **
        If ArrayLength = 0 Then
            Return 0
        ElseIf ArrayLength = 1 Or n < 0 Then
            Return dIdVCache(0)
        ElseIf n > ArrayLength - 2 Then
            'Return CurrentCache(CurrentCache.Length - 1)
            'Dim nO As Integer = n
            n = ArrayLength - 2
            If VBias >= 0 Then
                dPercentOfSlope = (VBias - dVBiasStart_Pos - n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
            Else
                dPercentOfSlope = -(VBias - dVBiasStart_Neg + n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
            End If
            Return (dPercentOfSlope * (dIdVCache(n + 1) - dIdVCache(n))) + dIdVCache(n)
        Else
            '// ** Fuehre Interpolation durch; **
            Return (dPercentOfSlope * (dIdVCache(n + 1) - dIdVCache(n))) + dIdVCache(n)
            'Return (CurrentCache(n + 1) - CurrentCache(n)) + CurrentCache(n)
        End If
    End Function

    ''' <summary>
    ''' Fit-Function, depending on the Selected Fit-Data-Type
    ''' </summary>
    Public Function FitFunctionPRECALC(ByVal VBias As Double,
                                       ByRef Identifiers As Integer(),
                                       ByRef Values As Double()) As Double
        Dim VBiasEff As Double = cFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalXOffset, Identifiers, Values) + VBias

        ' Get the effective bias shifted by the offset, and calculate the dIdV
        Dim ReturnVal As Double = dIdVPRECALC(VBiasEff)

        ' return the value shifted by the YOffset and stretched by the amplitude
        Return cFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYOffset, Identifiers, Values) + ReturnVal * cFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYStretch, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Fit-Function returning the dI/dV signal of a convolved
    ''' tip and sample DOS given by inheriting class.
    ''' </summary>
    Public Function FitFunctionDirect(ByVal VBias As Double,
                                      ByRef Identifiers As Integer(),
                                      ByRef Values As Double()) As Double
        Dim VBiasEff As Double = cFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalXOffset, Identifiers, Values) + VBias

        '#########################################################
        ' Sets additional properties before calculating the
        ' function value, i.e. inelastic channels
        Me.SetAdditionalProperties(Identifiers, Values)
        ' END InelasticChannels
        '#########################################################

        ' calculate the dI/dV-value
        Dim ReturnVal As Double = FuncdIdV(VBiasEff, Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)

        ' return the value shifted by the YOffset and stretched by the amplitude
        Return cFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYOffset, Identifiers, Values) + ReturnVal * cFitParameter.GetValueForIdentifier(FitParameterIdentifier.GlobalYStretch, Identifiers, Values)
    End Function

#End Region

#Region "Convolution Integral"
    ''' <summary>
    ''' Function to calculate the tunneling current I(Vbias)
    ''' </summary>
    <Cudafy>
    Public Shared Function FuncdIdVCUDA(ByVal VBias As Double,
                                        ByVal Identifiers As Integer(),
                                        ByVal Values As Double(),
                                        ByVal MaxEIntegrationRangePos As Double,
                                        ByVal MaxEIntegrationRangeNeg As Double,
                                        ByVal ConvolutionIntegrationStepSize As Double) As Double

        ' Create the current integration variable
        Dim dIdV As Double = 0.0

        ' Get temporary variables
        Dim E As Double
        Dim IEC_E As Double
        Dim IEC_P As Double
        Dim IECCount As Integer = GetIECFromParameterArray_Count(Identifiers, Values)

        ' Get the maximum inelastic channel energy
        Dim MaxInelasticChannelEnergy As Double = 0
        For iIEC As Integer = 0 To IECCount - 1 Step 1
            IEC_E = MathAbs(GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Energy))
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
            dIdV += IntegrandECCUDA(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                dIdV += IEC_P * IntegrandIECCUDA(E + IEC_E,
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
            dIdV += IntegrandECCUDA(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                dIdV += IEC_P * IntegrandIECCUDA(E + IEC_E,
                                              E - IEC_E,
                                              E + VBias,
                                              Identifiers, Values)
            Next
            '#####################################
        Next

        ' Return the normalized current
        Return ConvolutionIntegrationStepSize * dIdV
    End Function

    ''' <summary>
    ''' Function to calculate the tunneling current I(Vbias)
    ''' </summary>
    Public Function FuncdIdV(ByVal VBias As Double,
                             ByRef Identifiers As Integer(),
                             ByRef Values As Double(),
                             ByVal MaxEIntegrationRangePos As Double,
                             ByVal MaxEIntegrationRangeNeg As Double,
                             ByVal ConvolutionIntegrationStepSize As Double) As Double

        'Return FuncICUDA(VBias, Identifiers, Values, MaxEIntegrationRangePos, MaxEIntegrationRangeNeg, ConvolutionIntegrationStepSize)

        ' Create the current integration variable
        Dim dIdV As Double = 0.0

        ' Get temporary variables
        Dim E As Double
        Dim IEC_E As Double
        Dim IEC_P As Double
        Dim IECCount As Integer = GetIECFromParameterArray_Count(Identifiers, Values)

        ' Get the maximum inelastic channel energy
        Dim MaxInelasticChannelEnergy As Double = 0
        For iIEC As Integer = 0 To IECCount - 1 Step 1
            IEC_E = MathAbs(GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Energy))
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
            dIdV += IntegrandEC(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                dIdV += IEC_P * IntegrandIEC(E + IEC_E,
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
            dIdV += IntegrandEC(E, VBias, Identifiers, Values)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Probability)
                IEC_E = GetIECFromParameterArray(iIEC, Identifiers, Values, IECParameterIdentifier.Energy)
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                dIdV += IEC_P * IntegrandIEC(E + IEC_E,
                                             E - IEC_E,
                                             E + VBias,
                                             Identifiers, Values)
            Next
            '#####################################
        Next

        ' Return the normalized current
        Return dIdV * ConvolutionIntegrationStepSize
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

    ''' <summary>
    ''' Returns the derivative of the Fermi-Function.
    ''' </summary>
    <Cudafy>
    Public Shared Function FermiDerivativeF_eV(ByVal E As Double,
                                               ByVal T As Double) As Double
        Dim df, expo As Double

        '// ** Fallunterscheidung nach T <= 0.0 K oder T > 0.0 K; **
        '// ** (Division durch Null abfangen;) **
        If T <= 0 Then
            If E = 0 Then
                df = 1D
            Else
                df = 0D
            End If
        Else
            expo = E / (cConstants.kB_eV * T)
            If expo < MIN_EXP Or expo > MAX_EXP Then
                df = 0
            Else
                expo = Math.Exp(expo)
                df = expo / (expo + 1.0)
                df /= (expo + 1.0)
                df /= cConstants.kB_eV * T
            End If
        End If

        Return df
    End Function

#End Region

End Class
