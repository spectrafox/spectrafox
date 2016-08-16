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

    ''' <summary>
    ''' Number of points to consider for the spline-interpolation
    ''' </summary>
    Public Const PointsToTakeForSplineInterpolation As Integer = 10

#End Region

#Region "Fit range plausibility check"

    ''' <summary>
    ''' Check the fit-range to be not too large (\leq 10mV).
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
    Public Overridable Property ConvolutionIntegrationStepSize As Double = 0.000010901699437494743

    ''' <summary>
    ''' Largest relevant energy value to be considered for integration in the current integral.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Overridable Property ConvolutionIntegralE_POS As Double = 0.004

    ''' <summary>
    ''' Smallest relevant energy value to be considered for integration in the current integral.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Overridable Property ConvolutionIntegralE_NEG As Double = -0.004

    ''' <summary>
    ''' Calculate and cache all current values up to this bias.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Overridable Property CalculateForBiasRangeUpperE As Double = 0.004

    ''' <summary>
    ''' Calculate and cache all current values from this bias.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Overridable Property CalculateForBiasRangeLowerE As Double = -0.004

    ''' <summary>
    ''' Change the bias range of the calculated current.
    ''' </summary>
    Public Overrides Sub ChangeFitRangeX(LowerValue As Double, HigherValue As Double)
        'CalculateForBiasRangeUpperE = HigherValue
        'CalculateForBiasRangeLowerE = LowerValue
        'ConvolutionIntegralE_POS = HigherValue
        'ConvolutionIntegralE_NEG = LowerValue
        MyBase.ChangeFitRangeX(LowerValue, HigherValue)
        Me.ForceNewPrecalculation()
    End Sub

    ''' <summary>
    ''' Forces the precalculation of the values.
    ''' </summary>
    Public Sub ForceNewPrecalculation()
        Me.PrecalculateCurrent(Me.FitParametersGrouped, True)
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

    ''' <summary>
    ''' Gets the FitFunctionType from an input integer (e.g. from loading settings, where the FitFunctionType is stored as integer).
    ''' </summary>
    Public Shared Function GetFitFunctionTypeFromInteger(ByVal Input As Integer) As FitFunctionType
        Select Case Input
            Case FitFunctionType.I
                Return FitFunctionType.I
            Case FitFunctionType.dIdV
                Return FitFunctionType.dIdV
            Case Else
                Return FitFunctionType.I
        End Select
    End Function

#End Region

#Region "InelasticChannel Definition"

    ''' <summary>
    ''' Sub-Class to define an inelastic channel with its parameters.
    ''' </summary>
    Public Class InelasticChannel

        ''' <summary>
        ''' Fit-Parameter identifiers
        ''' </summary>
        Public Enum ParameterIdentifier
            IECEnergy = 0
            IECProbability = 1
        End Enum

        ''' <summary>
        ''' Internal index of the IEC.
        ''' </summary>
        Protected _IECIndex As Integer

        ''' <summary>
        ''' Index of the IEC.
        ''' </summary>
        Public Property IECIndex As Integer
            Get
                Return Me._IECIndex
            End Get
            Set(value As Integer)
                Me.Energy.ChangeIdentifier(IECIdentifier_Energy(value))
                Me.Probability.ChangeIdentifier(IECIdentifier_Probability(value))

                Me.Energy.Description = IECDescription_Energy(value)
                Me.Probability.Description = IECDescription_Probability(value)

                Me._IECIndex = value
            End Set
        End Property

        ''' <summary>
        ''' Can be used to activate or deactivate the IEC.
        ''' </summary>
        Public SubGapPeakEnabled As Boolean = True

        ''' <summary>
        ''' Energy of the IEC
        ''' </summary>
        Public Energy As New cFitParameter(ParameterIdentifier.IECEnergy.ToString, 0, False, My.Resources.rFitFunction_TipSampleConvolutionBase.IEC_Energy)

        ''' <summary>
        ''' Probability of the IEC
        ''' </summary>
        Public Probability As New cFitParameter(ParameterIdentifier.IECProbability.ToString, 1, False, My.Resources.rFitFunction_TipSampleConvolutionBase.IEC_Probability)

        ''' <summary>
        ''' Returns a panel with all parameters locked to the parameter-selection boxes
        ''' </summary>
        Public Function GetInelasticChannelPanel(ByRef AllFitParameters As cFitParameterGroupGroup) As cFitSettingSubParameter_InelasticChannel
            Dim P As New cFitSettingSubParameter_InelasticChannel(Me,
                                                                  AllFitParameters)
            P.Identifier = Me.IECIndex
            Return P
        End Function

        ''' <summary>
        ''' Constructor
        ''' </summary>
        Public Sub New()
            AddHandler Energy.ValueChanged, AddressOf RaiseValueChangedEvent
            AddHandler Probability.ValueChanged, AddressOf RaiseValueChangedEvent
        End Sub

        ''' <summary>
        ''' Value changed of IEC
        ''' </summary>
        Public Event ValueChanged()

        ''' <summary>
        ''' Raise the Event
        ''' </summary>
        Private Sub RaiseValueChangedEvent(NewValue As Double)
            RaiseEvent ValueChanged()
        End Sub

        ''' <summary>
        ''' Returns the Nth identifier of an IEC
        ''' </summary>
        Public Shared Function IECDescription_Energy(ByVal i As Integer) As String
            Return "#" & i.ToString & " " & My.Resources.rFitFunction_TipSampleConvolutionBase.IEC_Energy
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an IEC
        ''' </summary>
        Public Shared Function IECDescription_Probability(ByVal i As Integer) As String
            Return "#" & i.ToString & " " & My.Resources.rFitFunction_TipSampleConvolutionBase.IEC_Probability
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an IEC
        ''' </summary>
        Public Shared Function IECIdentifier_Energy(ByVal i As Integer) As String
            Return ParameterIdentifier.IECEnergy.ToString & i.ToString
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an IEC
        ''' </summary>
        Public Shared Function IECIdentifier_Probability(ByVal i As Integer) As String
            Return ParameterIdentifier.IECProbability.ToString & i.ToString
        End Function

        ''' <summary>
        ''' The the IEC-Identifier from a String (last digit is number)
        ''' </summary>
        Public Shared Function IECIndexFromIdentifier(ByVal Identifier As String) As Integer
            With ParameterIdentifier.IECEnergy
                If Identifier.StartsWith(.ToString) Then Return Convert.ToInt32(Identifier.Remove(0, .ToString.Length))
            End With
            With ParameterIdentifier.IECProbability
                If Identifier.StartsWith(.ToString) Then Return Convert.ToInt32(Identifier.Remove(0, .ToString.Length))
            End With
            Return -1
        End Function

    End Class

    ''' <summary>
    ''' List to save all inelastic channels.
    ''' </summary>
    Private _InelasticChannels As New List(Of InelasticChannel)

    ''' <summary>
    ''' Adds a new inelastic channel to the fit-function.
    ''' Returns the index of the internal list, at which the channel got added.
    ''' </summary>
    Public Function AddInelasticChannel(Optional ByVal Index As Integer = -1) As Integer

        If Index < 0 Then
            Index = 0

            ' find free index for the IEC
            Dim IECIndexOccupied As Boolean = False
            For i As Integer = 0 To Me.InelasticChannels.Count Step 1
                IECIndexOccupied = False
                For Each IEC As InelasticChannel In Me.InelasticChannels
                    If IEC.IECIndex = i Then
                        IECIndexOccupied = True
                        Exit For
                    End If
                Next

                If IECIndexOccupied = False Then
                    Index = i
                    Exit For
                End If
            Next
        End If

        Dim NewIEC As InelasticChannel = New InelasticChannel()
        NewIEC.IECIndex = Index
        Me._InelasticChannels.Add(NewIEC)
        Return Index

    End Function

    ''' <summary>
    ''' Removes the inelastic channel from the list.
    ''' False, if the index was not found in the dictionary.
    ''' </summary>
    Public Function RemoveInelasticChannel(ByVal ChannelIndex As Integer) As Boolean
        With Me._InelasticChannels
            If .Count > ChannelIndex Then
                .RemoveAt(ChannelIndex)
                Return True
            Else
                Return False
            End If
        End With
    End Function

    ''' <summary>
    ''' Clears the list of all InelasticChannels.
    ''' </summary>
    Public Sub ClearInelasticChannels()
        Me._InelasticChannels.Clear()
    End Sub

    ''' <summary>
    ''' Returns the list of all registered inelastic channels
    ''' </summary>
    Public ReadOnly Property InelasticChannels As List(Of InelasticChannel)
        Get
            Return Me._InelasticChannels
        End Get
    End Property

    ''' <summary>
    ''' Method that sets all initial FitParameters from the individual FitFunctions
    ''' in this multiple-function-container.
    ''' Override this, !!!BUT CALL THIS!!! to set additional properties, such as Sub-Gap-Peaks in BCS.
    ''' </summary>
    Public Overridable Sub ReInitializeFitParameters()

        ' Remove old inelastic channel fit-parameters from the Fit-Parameter-Array
        Dim FitParameterIdentifiersToRemove As New List(Of String)
        For Each FP As cFitParameter In Me.FitParameters

            If FP.Identifier.Contains(InelasticChannel.ParameterIdentifier.IECEnergy.ToString) OrElse
               FP.Identifier.Contains(InelasticChannel.ParameterIdentifier.IECProbability.ToString) Then
                FitParameterIdentifiersToRemove.Add(FP.Identifier)
            End If

        Next

        ' Remove all Fit-Parameters
        For i As Integer = 0 To FitParameterIdentifiersToRemove.Count - 1 Step 1
            Me.FitParameters.RemoveFitParameter(FitParameterIdentifiersToRemove(i))
        Next

        ' Modify the Fit-Parameter-Array and add all inelastic channels
        For i As Integer = 0 To Me._InelasticChannels.Count - 1 Step 1
            ' add Fit-Parameters
            With Me.FitParameters
                .Add(IECIdentifier_Energy(i), Me._InelasticChannels(i).Energy)
                .Add(IECIdentifier_Probability(i), Me._InelasticChannels(i).Probability)
            End With
        Next
    End Sub

    ''' <summary>
    ''' Returns the Nth identifier of an IEC
    ''' </summary>
    Public Function IECIdentifier_Energy(ByVal i As Integer) As String
        Return InelasticChannel.ParameterIdentifier.IECEnergy.ToString & i.ToString
    End Function

    ''' <summary>
    ''' Returns the Nth identifier of an IEC
    ''' </summary>
    Public Function IECIdentifier_Probability(ByVal i As Integer) As String
        Return InelasticChannel.ParameterIdentifier.IECProbability.ToString & i.ToString
    End Function

#Region "Import / Export"

    ''' <summary>
    ''' Check after a successfull import of a fit-parameter, if it belongs to an inelastic channel.
    ''' If so, add the channel to the internal list, if not yet done, and adjust its settings.
    ''' </summary>
    Protected Overrides Sub Import_ParameterIdentified(ByVal Identifier As String, ByRef Parameter As cFitParameter)

        ' Do nothing, if we treat regular parameters
        If (Identifier.Contains(InelasticChannel.ParameterIdentifier.IECEnergy.ToString) OrElse
            Identifier.Contains(InelasticChannel.ParameterIdentifier.IECProbability.ToString)) Then

            ' Get the index form the identifier
            Dim IECIndex As Integer = InelasticChannel.IECIndexFromIdentifier(Identifier)

            ' Check, if the necessary count of inelastic channels has been added:
            Dim IECToModify As InelasticChannel = Nothing
            For Each IEC As InelasticChannel In Me.InelasticChannels
                If IEC.IECIndex = IECIndex Then
                    IECToModify = IEC
                    Exit For
                End If
            Next

            If IECToModify Is Nothing Then
                ' Create SGP
                Me.AddInelasticChannel(IECIndex)
                Me.ReInitializeFitParameters()
                IECToModify = Me.InelasticChannels(Me.InelasticChannels.Count - 1)
            End If

            ' Return, since we treated this parameter already!
            Return
        End If

        MyBase.Import_ParameterIdentified(Identifier, Parameter)
    End Sub

#End Region

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
    Public MustOverride Function SampleDOS(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

    ''' <summary>
    ''' Must-overrides that have to be defined by the child functions.
    ''' Implemented as SHARED to be able to use CUDA!
    ''' 
    ''' Does nothing! Override it, and declare it for CUDAfying!!!!
    ''' </summary>
    Public MustOverride Function TipDOS(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

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
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.GlobalXOffset.ToString, 0, False, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalXOffset))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.GlobalYOffset.ToString, 0, False, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalYOffset))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.GlobalYStretch.ToString, 1, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_GlobalYStretch))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.T_tip.ToString, 1.19, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_T_tip))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.T_sample.ToString, 1.19, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_T_sample))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.SystemBroadening.ToString, 0.000035, True, My.Resources.rFitFunction_TipSampleConvolutionBase.Parameter_SystemBroadening))
    End Sub
#End Region

#Region "Fit-Function Interface and caching algorithm to calculate the current"

    ''' <summary>
    ''' Current-precalculation interpolation-interval-width:
    ''' is responsible for the number of points that need to be calculated,
    ''' and therefore the speed of the calculation, and the accuracy.
    ''' </summary>
    Public Overridable Property dE_BiasStepWidth As Double = 0.00001

    ''' <summary>
    ''' Cache storage for the current integral.
    ''' </summary>
    Private CurrentIntegralCache As Double()
    Private CurrentIntegralCache_X As Double()

    ' Bias cache properties,
    ' giving the bias range of the current values
    ' stored in the arrays.
    Private dVBiasStart As Double = 0
    Private dVBiasEnd As Double = 0

    ' cache variables for the last used parameters, for which the current is cached
    Protected LastParameterValues() As Double

    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' Caches the Current integral, if the fit-parameters have changed.
    ''' </summary>
    Public Overrides Function GetY(ByRef x As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

        ' Launch the Precalculation
        Me.PrecalculateCurrent(InputParameters, False)

        ' Finally return the cached FitFunction
        Return Me.FitFunctionPRECALC(x, InputParameters)
    End Function

    ''' <summary>
    ''' Precalculates the current cache.
    ''' Used to save a long calculation time for each point, if the parameters have not changed.
    ''' </summary>
    Protected Overridable Sub PrecalculateCurrent(ByRef InputParameters As cFitParameterGroupGroup, ByVal Force As Boolean)

        ' Current
        Dim ParameterValues As Double() = InputParameters.Group(Me.UseFitParameterGroupID).GetParameterValues

        ' Check, if the current cache has to be refreshed.
        If Not Force AndAlso cArrayHelper.AreArraysTheSame(ParameterValues, LastParameterValues) Then Return

        ' save the last used parameter-set
        If LastParameterValues Is Nothing Then ReDim LastParameterValues(ParameterValues.Length - 1)
        If LastParameterValues.Length <> ParameterValues.Length Then ReDim LastParameterValues(ParameterValues.Length - 1)
        ParameterValues.CopyTo(LastParameterValues, 0)

        '#########################################
        ' Precalculate the current and broaden it
        ' for the selected interval and use the
        ' GPGPU to speed up everything.

        ' Initialize variables
        Dim iCurrentCachePoints As Integer = 0
        dVBiasStart = 0
        dVBiasEnd = 0

        ' catch the case where the energy-range is 0
        If Me.CalculateForBiasRangeUpperE <= Me.CalculateForBiasRangeLowerE Then
            Return
        End If

        ' Get the range for which to calculate the current
        ' additionally consider the broadening-width.
        Dim UpperBiasEnergy As Double = Me.CalculateForBiasRangeUpperE
        Dim LowerBiasEnergy As Double = Me.CalculateForBiasRangeLowerE

        If UpperBiasEnergy > 0 Then
            UpperBiasEnergy -= 0.5 * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.SystemBroadening.ToString).Value
        Else
            UpperBiasEnergy += 0.5 * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.SystemBroadening.ToString).Value
        End If
        If LowerBiasEnergy > 0 Then
            LowerBiasEnergy -= 0.5 * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.SystemBroadening.ToString).Value
        Else
            LowerBiasEnergy += 0.5 * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.SystemBroadening.ToString).Value
        End If

        ' only positive bias values
        '###########################
        iCurrentCachePoints = CInt((UpperBiasEnergy - LowerBiasEnergy) / dE_BiasStepWidth)
        dVBiasEnd = UpperBiasEnergy
        dVBiasStart = LowerBiasEnergy

        ' Redimension the cache arrays
        If Me.CurrentIntegralCache Is Nothing And iCurrentCachePoints > 0 Then
            ReDim Me.CurrentIntegralCache(iCurrentCachePoints - 1)
        ElseIf Me.CurrentIntegralCache.Length <> iCurrentCachePoints And iCurrentCachePoints > 0 Then
            ReDim Me.CurrentIntegralCache(iCurrentCachePoints - 1)
        End If

        ' Calculate new X-Array, if necessary!
        ' Not necessary, if the interpolation step width did not change.
        Dim CalculateBiasXArray As Boolean = True
        If Me.CurrentIntegralCache_X Is Nothing And iCurrentCachePoints > 0 Then
            ReDim Me.CurrentIntegralCache_X(iCurrentCachePoints - 1)
        ElseIf Me.CurrentIntegralCache_X.Length <> iCurrentCachePoints And iCurrentCachePoints > 0 Then
            ReDim Me.CurrentIntegralCache_X(iCurrentCachePoints - 1)
        Else
            CalculateBiasXArray = False
        End If

        '// fill the bias (x) array on the CPU
        If CalculateBiasXArray Then
            If iCurrentCachePoints > 0 Then
                Parallel.For(0, iCurrentCachePoints, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                   Me.CurrentIntegralCache_X(j) = dVBiasStart + (dE_BiasStepWidth * j)
                                                                               End Sub)
            End If
        End If


#If USEPARALLEL = 1 Then
        If iCurrentCachePoints > 0 Then
            Dim Parameters As cFitParameterGroupGroup = InputParameters
            Parallel.For(0, iCurrentCachePoints, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                               CurrentIntegralCache(j) = FuncI(Me.CurrentIntegralCache_X(j),
                                                                                                               Parameters,
                                                                                                               Me.ConvolutionIntegralE_POS,
                                                                                                               Me.ConvolutionIntegralE_NEG,
                                                                                                               ConvolutionIntegrationStepSize)
                                                                           End Sub)

            ' Convolute the current with a gaussian afterwards, to broaden it.
            Dim ConvolutedCurrent(Me.CurrentIntegralCache_X.Length - 1) As Double
            Parallel.For(0, iCurrentCachePoints, Me.MultiThreadingOptions, Sub(i As Integer)
                                                                               ConvolutedCurrent(i) = ConvolveFunction(Me.CurrentIntegralCache_X(i),
                                                                                                                       CInt(Me.ConvolutionFunction),
                                                                                                                       Parameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.SystemBroadening.ToString).Value)
                                                                           End Sub)
            ConvolutedCurrent.CopyTo(Me.CurrentIntegralCache, 0)
        End If
#Else
          ###### NOT DEFINED ANYMORE
#End If

    End Sub
#End Region

#Region "Convolution of the current"

    ''' <summary>
    ''' Energy sampling size in [eV] for the convolution with a Gauss function.
    ''' Used for calculation of broadening.
    ''' </summary>
    Public Overridable Property dE_BroadeningStepWidth As Double = 0.0000309016994374947424102D

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
                                    Me.CurrentIntegralCache,
                                    ConvolutionFunction,
                                    BroadeningWidth,
                                    Me.dVBiasStart,
                                    Me.dVBiasEnd,
                                    dE_BroadeningStepWidth,
                                    dE_BiasStepWidth)
    End Function

    ''' <summary>
    ''' // *****************************************************************
    ''' // *                                                               *
    ''' // *      ( f * g ) (E) = int f(F) * g( E - F ) dF                 *
    ''' // *                                                               *
    ''' // *****************************************************************
    ''' </summary>
    Protected Shared Function ConvolveFunctionCUDA(ByVal E As Double,
                                                   ByVal CurrentIntegralCache As Double(),
                                                   ByVal ConvolutionFunction As Integer,
                                                   ByVal BroadeningWidth As Double,
                                                   ByVal dVBiasStart As Double,
                                                   ByVal dVBiasEnd As Double,
                                                   ByVal dE_BroadeningStepWidth As Double,
                                                   ByVal BiasStep_dE As Double) As Double

        Dim ConvolutedCurrent As Double = 0

        If BroadeningWidth <= dE_BroadeningStepWidth Then
            ConvolutedCurrent = CurrentPRECALCCUDA(E, CurrentIntegralCache, dVBiasStart, BiasStep_dE)
        Else

            ' Get the maximum number of iterations in the energy-range.
            Dim Nmax As Integer = CInt((dVBiasEnd * 1) / dE_BroadeningStepWidth)

            ' Integration variable
            Dim dE As Double = 0.0
            Dim tmp As Double = 0.0
            ' Limit for small values of the broadening
            Dim MinGaussianValue As Double = MIN_DIFFERENTIAL
            Dim GaussianValue As Double = Double.MaxValue
            Dim MaxE As Double = Math.Max(cNumericFunctions.MathAbs(dVBiasStart), cNumericFunctions.MathAbs(dVBiasEnd))

            ' Set up a running compensation algorithm
            Dim c As Double = 0.0
            Dim y As Double

            ' Tread dE = 0 extra, to avoid double counting
            GaussianValue = cNumericFunctions.GaussPeak_Amplitude(dE, 0, BroadeningWidth, 0, 1000)
            ConvolutedCurrent = CurrentPRECALCCUDA(E, CurrentIntegralCache, dVBiasStart, BiasStep_dE) * GaussianValue
            dE += dE_BroadeningStepWidth

            ' Go through all energies for convolution and integrate the individual current contributions.
            Do Until dE > MaxE Or GaussianValue < MinGaussianValue

                ' calculate Gaussian value
                GaussianValue = cNumericFunctions.GaussPeak_Amplitude(dE, 0, BroadeningWidth, 0, 1000)

                tmp = CurrentPRECALCCUDA(E - dE, CurrentIntegralCache, dVBiasStart, BiasStep_dE)
                tmp += CurrentPRECALCCUDA(E + dE, CurrentIntegralCache, dVBiasStart, BiasStep_dE)
                tmp *= GaussianValue

                ' Error correction for small values!
                y = tmp - c
                tmp = ConvolutedCurrent + y
                c = (tmp - ConvolutedCurrent) - y

                ConvolutedCurrent = tmp

                '######################################
                ' Sum up positive energy contribution
                dE += dE_BroadeningStepWidth
            Loop

            ConvolutedCurrent *= dE_BroadeningStepWidth
        End If

        ' Return the normalized current
        Return ConvolutedCurrent

        'Dim result, F, integ, dCurr As Double
        'Dim bufPosEnergy, bufNegEnergy As Double
        'Dim buf3, buf4 As Double
        'Dim IntegrationBoundary As Double = MIN_DIFFERENTIAL * dE_BroadeningStepWidth

        '' save time, if the broadening is smaller than the calculation-step-size,
        '' then just use the input-array.
        'If BroadeningWidth <= dE_BroadeningStepWidth Then
        '    result = CurrentPRECALCCUDA(E, CurrentIntegralCache, dVBiasStart, BiasStep_dE)
        'Else
        '    ' Numerical Integration to convolute the current with the
        '    ' specified broadening peak. Convolution may take some time
        '    ' until the integrals converge. Especially the Lorentzian!
        '    result = 0.0

        '    If E > 0 Then
        '        ' integrate until the broadening converges
        '        F = dE_BroadeningStepWidth
        '        bufPosEnergy = E - F
        '        bufNegEnergy = E + F
        '        dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache, dVBiasStart, BiasStep_dE)
        '        Select Case ConvolutionFunction
        '            Case ConvolutionFunctions.Gauss
        '                buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
        '                integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
        '            Case ConvolutionFunctions.Lorentz
        '                buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
        '                buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
        '                integ = dCurr * (1.0 / buf3 + 1.0 / buf4)
        '        End Select

        '        ' Stop, if the integrand converges to small values.
        '        ' This needs some time for Lorentzian functions.
        '        While MathAbs(F) < MathAbs(dVBiasEnd - dVBiasStart) ' cNumericFunctions.MathAbs(integ) > IntegrationBoundary Or F < 1.1 * cNumericFunctions.MathAbs(E)
        '            bufPosEnergy = E - F
        '            bufNegEnergy = E + F
        '            dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache, dVBiasStart, BiasStep_dE)

        '            Select Case ConvolutionFunction
        '                Case ConvolutionFunctions.Gauss
        '                    integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
        '                Case ConvolutionFunctions.Lorentz
        '                    buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
        '                    buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
        '                    integ = dCurr * (1 / buf3 + 1 / buf4)
        '            End Select

        '            result += integ
        '            F += dE_BroadeningStepWidth
        '        End While
        '    Else
        '        ' integrate until the broadening converges
        '        ' Stop, if the integrand converges to small values.
        '        ' This needs some time for Lorentzian functions.
        '        F = -dE_BroadeningStepWidth
        '        bufPosEnergy = E - F
        '        bufNegEnergy = E + F
        '        dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache, dVBiasStart, BiasStep_dE)
        '        Select Case ConvolutionFunction
        '            Case ConvolutionFunctions.Gauss
        '                buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
        '                integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
        '            Case ConvolutionFunctions.Lorentz
        '                buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
        '                buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
        '                integ = dCurr * (1.0 / buf3 + 1.0 / buf4)
        '        End Select

        '        While MathAbs(F) < MathAbs(dVBiasEnd - dVBiasStart) ' MathAbs(integ) > IntegrationBoundary Or MathAbs(F) < 1.1 * MathAbs(E)
        '            bufPosEnergy = E - F
        '            bufNegEnergy = E + F
        '            dCurr = CurrentPRECALCCUDA(F, CurrentIntegralCache, dVBiasStart, BiasStep_dE)

        '            Select Case ConvolutionFunction
        '                Case ConvolutionFunctions.Gauss
        '                    integ = dCurr * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
        '                Case ConvolutionFunctions.Lorentz
        '                    buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
        '                    buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
        '                    integ = dCurr * (1 / buf3 + 1 / buf4)
        '            End Select

        '            result += integ
        '            F -= dE_BroadeningStepWidth
        '        End While
        '    End If

        '    '  // ** Vorfaktor aufmultiplizieren; **
        '    Select Case ConvolutionFunction
        '        Case ConvolutionFunctions.Gauss
        '            result *= 1.0 / (Math.Sqrt(MathNet.Numerics.Constants.Pi2) * BroadeningWidth)
        '        Case ConvolutionFunctions.Lorentz
        '            result *= 2.0 * cNumericFunctions.MathAbs(BroadeningWidth) / MathNet.Numerics.Constants.Pi
        '    End Select

        '    '  // ** Breite dE-Intervall aufmultiplizieren; **
        '    result *= dE_BroadeningStepWidth
        'End If

        'Return result
    End Function

#End Region

#Region "Fit Function and CUDA Kernel for the precalculation of the current."
    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' PUBLIC VERSION, USING THE MEMBERS OF THE CLASS
    ''' </summary>
    Public Function CurrentPRECALC(ByVal VBias As Double) As Double
        ' Return the shared cuda version.
        Return CurrentPRECALCCUDA(VBias,
                                  Me.CurrentIntegralCache,
                                  Me.dVBiasStart, dE_BiasStepWidth)
    End Function

    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' CUDA VERSION, USING SEPARATE GIVEN ARRAYS
    ''' </summary>
    '<Cudafy>
    Protected Shared Function CurrentPRECALCCUDA(ByVal VBias As Double,
                                                 ByVal CurrentCache As Double(),
                                                 ByVal dVBiasStart As Double,
                                                 ByVal BiasStep_dE As Double) As Double
        ' reference to the next element
        Dim n As Double = (VBias - dVBiasStart) / BiasStep_dE

        ' Save an interger version of the N
        Dim IntN As Integer = CInt(n)

        ' // Now distinguish between different cases.
        ' /- Arraylength not matching
        ' /- interpolation between requested points, etc.
        If CurrentCache.Length <= 1 Then
            ' Some settings are wrong. Just return 0.
            Return 0
        ElseIf n < 0 Then
            ' Interpolate to the left

            ' in the beginning just return the first value
            Return CurrentCache(0)

        ElseIf n >= CurrentCache.Length - 2 Then
            ' Interpolate to the right

            ' in the beginning just return the last value
            Return CurrentCache(CurrentCache.Length - 1)
        ElseIf IntN = n Then
            ' If the value matches directly a precalculated value,
            ' then just return the precalculated value immediatelly.
            Return CurrentCache(IntN)
        Else
            ' the requested value lies between two calculated points,
            ' so lets interpolate between these two points the values.

            '##############################################
            ' USING SPLINE INTERPOLATION NOW.... SEE BELOW
            ' linear interpolation
            'Dim val As Double = CurrentCache(IntN) - CurrentCache(IntN)
            'val = val * (n - IntN) / n
            'Return CurrentCache(IntN) + val

            ' Cosine Interpolation
            'Dim y1 As Double = CurrentCache(IntN)
            'Dim y2 As Double
            'Dim EvaluateAtX As Double = n - IntN

            'If IntN >= CurrentCache.Length - 2 Then
            '    y2 = CurrentCache(IntN)
            'Else
            '    y2 = CurrentCache(IntN + 1)
            'End If

            'Return cNumericalMethods.CosineInterpolationCudafy(y1, y2, EvaluateAtX)
            '##############################################

            '##############################################
            ' USING SPLINE INTERPOLATION NOW.... SEE BELOW
            '' Cubic Interpolation
            'Dim y0 As Double
            'Dim y1 As Double = CurrentCache(IntN)
            'Dim y2 As Double
            'Dim y3 As Double
            'Dim EvaluateAtX As Double = 1 + n - IntN

            'If IntN >= CurrentCache.Length - 1 Then
            '    y2 = CurrentCache(IntN)
            '    y3 = CurrentCache(IntN)
            'ElseIf IntN >= CurrentCache.Length - 2 Then
            '    y2 = CurrentCache(IntN + 1)
            '    y3 = CurrentCache(IntN + 1)
            'Else
            '    y2 = CurrentCache(IntN + 1)
            '    y3 = CurrentCache(IntN + 2)
            'End If

            'If IntN = 0 Then
            '    y0 = CurrentCache(IntN)
            'Else
            '    y0 = CurrentCache(IntN - 1)
            'End If

            'Return cNumericalMethods.CubicInterpolationCudafy(y0, y1, y2, y3, EvaluateAtX)
            '#####################################################


            ' Polynomial Interpolation
            Dim InterpolationPointsLeft As Integer = PointsToTakeForSplineInterpolation
            Dim InterpolationPointsRight As Integer = PointsToTakeForSplineInterpolation

            ' Check if we have enough points to the left and right for the interpolation
            If IntN >= CurrentCache.Length - PointsToTakeForSplineInterpolation - 1 Then
                InterpolationPointsRight = CurrentCache.Length - 1 - IntN
            End If
            If IntN < PointsToTakeForSplineInterpolation Then
                InterpolationPointsLeft = IntN
            End If

            ' Calculate the size of the interpolation-arrays
            Dim InterpolationArrayMaxIndex As Integer = InterpolationPointsRight + InterpolationPointsLeft - 1
            Dim InterpolationArrayX(InterpolationArrayMaxIndex) As Double
            Dim InterpolationArrayY(InterpolationArrayMaxIndex) As Double

            ' Fill the interpolation arrays.
            Dim ArrayCounter As Integer = 0
            For i As Integer = InterpolationPointsLeft - 1 To 1 Step -1
                InterpolationArrayX(ArrayCounter) = ArrayCounter
                InterpolationArrayY(ArrayCounter) = CurrentCache(IntN - i)

                ArrayCounter += 1
            Next

            ' Fill the point next to the evaluation point
            Dim NEffective As Double = n - IntN + ArrayCounter
            InterpolationArrayX(ArrayCounter) = ArrayCounter
            InterpolationArrayY(ArrayCounter) = CurrentCache(IntN)

            ' Fill the interpolation arrays.
            For i As Integer = 1 To InterpolationPointsRight - 1 Step 1
                ArrayCounter += 1

                InterpolationArrayX(ArrayCounter) = ArrayCounter
                InterpolationArrayY(ArrayCounter) = CurrentCache(IntN + i)
            Next

            ' Take care, that our interpolation is only accessed within the interpolation range
            If NEffective < 0 Then NEffective = 0.1
            If NEffective > ArrayCounter - 1 Then NEffective = (ArrayCounter - 1) + 0.1

            Return cNumericalMethods.SplineInterpolationNative(InterpolationArrayX, InterpolationArrayY, NEffective)
        End If
    End Function

    ''' <summary>
    ''' Fit-Function, depending on the Selected Fit-Data-Type
    ''' </summary>
    Public Overridable Function FitFunctionPRECALC(ByVal VBias As Double,
                                                   ByRef InputParameters As cFitParameterGroupGroup) As Double
        Dim VBiasEff As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.GlobalXOffset.ToString).Value + VBias
        Dim ReturnVal As Double = 0
        ' take the current or the dI/dV-value
        Select Case FitDataType
            Case FitFunctionType.I ' I(V)
                Dim I As Double = CurrentPRECALC(VBiasEff)
                ReturnVal = I
            Case FitFunctionType.dIdV ' dI/dV(V)
                Dim ILeft As Double = CurrentPRECALC((VBiasEff - dE_BiasStepWidth))
                Dim IRight As Double = CurrentPRECALC((VBiasEff + dE_BiasStepWidth))
                ReturnVal = (IRight - ILeft) / dE_BiasStepWidth
        End Select
        ' return the value shifted by the YOffset and stretched by the amplitude
        Return InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.GlobalYOffset.ToString).Value + ReturnVal * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.GlobalYStretch.ToString).Value
    End Function

    ''' <summary>
    ''' Fit-Function, depending on the Selected Fit-Data-Type
    ''' </summary>
    Public Overridable Function FitFunctionDirect(ByVal VBias As Double,
                                                  ByRef InputParameters As cFitParameterGroupGroup) As Double
        Dim ReturnVal As Double = 0
        Dim VBiasEff As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.GlobalXOffset.ToString).Value + VBias

        ' take the current or the dI/dV-value
        Select Case FitDataType
            Case FitFunctionType.I ' I(V)
                Dim I As Double = FuncI(VBiasEff, InputParameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                ReturnVal = I
            Case FitFunctionType.dIdV ' dI/dV(V)
                Dim ILeft As Double = FuncI((VBiasEff + dE_BiasStepWidth / 2), InputParameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                Dim IRight As Double = FuncI((VBiasEff - dE_BiasStepWidth / 2), InputParameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, ConvolutionIntegrationStepSize)
                ReturnVal = (ILeft - IRight) / dE_BiasStepWidth
        End Select

        ' return the value shifted by the YOffset and stretched by the amplitude
        Return InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.GlobalYOffset.ToString).Value + ReturnVal * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.GlobalYStretch.ToString).Value
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
    Public Overridable Function IntegrandEC(ByVal E As Double,
                                            ByVal eV As Double,
                                            ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return (FermiF_eV(E, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.T_sample.ToString).Value) - FermiF_eV(E + eV, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.T_tip.ToString).Value)) * (SampleDOS(E, InputParameters) * TipDOS(E + eV, InputParameters))
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
    Public Overridable Function IntegrandIEC(ByVal EPlusEIEC As Double,
                                             ByVal EMinusEIEC As Double,
                                             ByVal EPlusBias As Double,
                                             ByRef InputParameters As cFitParameterGroupGroup) As Double
        ' // ** Berechne Fermifunktionen der Spitze f(E + E_iec) sowie f(E - E_iec) und der Probe f(E + eV); **
        Dim FermiTipEPEIEC As Double = FermiF_eV(EPlusEIEC, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.T_tip.ToString).Value)
        Dim FermiTipEMEIEC As Double = FermiF_eV(EMinusEIEC, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.T_tip.ToString).Value)
        Dim FermiSampleEPV As Double = FermiF_eV(EPlusBias, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.T_sample.ToString).Value)

        '// ** Berechnung und Rueckgabe des Integrand; **
        '// ** Benutze Vorausberechnete Funktionen um Rechenzeit zu sparen; **
        Return SampleDOS(EPlusBias, InputParameters) _
                * (TipDOS(EPlusEIEC, InputParameters) * FermiTipEPEIEC * (1.0 - FermiSampleEPV) _
                - TipDOS(EMinusEIEC, InputParameters) * (1.0 - FermiTipEMEIEC) * FermiSampleEPV)
    End Function

#End Region


#Region "Convolution Integral"
    ''' <summary>
    ''' Function to calculate the tunneling current I(Vbias)
    ''' </summary>
    Public Overridable Function FuncI(ByVal VBias As Double,
                                      ByRef InputParameters As cFitParameterGroupGroup,
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
        Dim IECCount As Integer = Me._InelasticChannels.Count

        ' Get the maximum inelastic channel energy
        Dim MaxInelasticChannelEnergy As Double = 0
        For iIEC As Integer = 0 To IECCount - 1 Step 1
            IEC_E = MathAbs(InputParameters.Group(Me.UseFitParameterGroupID).Parameter(IECIdentifier_Energy(iIEC)).Value)
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
            I += IntegrandEC(E, VBias, InputParameters)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(IECIdentifier_Probability(iIEC)).Value
                IEC_E = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(IECIdentifier_Energy(iIEC)).Value
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                I += IEC_P * IntegrandIEC(E + IEC_E,
                                          E - IEC_E,
                                          E + VBias,
                                          InputParameters)
            Next
            '#####################################
        Next
        For j As Integer = 0 To NmaxNeg Step 1
            '######################################
            ' Sum up negative energy contribution
            E = -ConvolutionIntegrationStepSize * (j + 1)
            I += IntegrandEC(E, VBias, InputParameters)

            '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
            For iIEC As Integer = 0 To IECCount - 1 Step 1
                IEC_P = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(IECIdentifier_Probability(iIEC)).Value
                IEC_E = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(IECIdentifier_Energy(iIEC)).Value
                '// ** Inelastischen Strom berechnen und aufaddieren; **
                I += IEC_P * IntegrandIEC(E + IEC_E,
                                          E - IEC_E,
                                          E + VBias,
                                          InputParameters)
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

#Region "Export/Import routine for the additional function parameters (convolution step size, etc.)"

    ''' <summary>
    ''' Write additionally the parameter-settings about enabled or disabled SGPs.
    ''' </summary>
    Protected Overrides Sub Export_WriteAdditionalXMLElements(ByRef XMLWriter As Xml.XmlTextWriter)

        With XMLWriter
            ' Begin the element of the parameter description
            .WriteStartElement("CurrentIntegralConvolutionSettings") ' <CurrentIntegralConvolutionSettings>
            .WriteAttributeString("CurrentPrecalculationInterpolationStepWidth", Me.dE_BiasStepWidth.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BiasLimitUpperE", Me.CalculateForBiasRangeUpperE.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BiasLimitLowerE", Me.CalculateForBiasRangeLowerE.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ConvolutionIntegralEPos", Me.ConvolutionIntegralE_POS.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ConvolutionIntegralENeg", Me.ConvolutionIntegralE_NEG.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("IntegrationEnergyStep", Me.ConvolutionIntegrationStepSize.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BroadeningStepWidth", Me.dE_BroadeningStepWidth.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("FitDataType", Convert.ToInt32(Me.FitDataType).ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteEndElement() ' <\CurrentIntegralConvolutionSettings>
        End With

        MyBase.Export_WriteAdditionalXMLElements(XMLWriter)
    End Sub

    ''' <summary>
    ''' Read unknown XML-Elements, if needed by the import.
    ''' </summary>
    Protected Overrides Sub Import_UnknownXMLElementIdentified(ByVal XMLElementName As String, ByRef XMLReader As Xml.XmlReader)
        If XMLElementName = "CurrentIntegralConvolutionSettings" Then
            With XMLReader
                If .AttributeCount > 0 Then
                    While .MoveToNextAttribute
                        Select Case .Name
                            ' extract all the current integral settings
                            Case "CurrentPrecalculationInterpolationStepWidth"
                                Me.dE_BiasStepWidth = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "BiasLimitUpperE"
                                Me.CalculateForBiasRangeUpperE = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "BiasLimitLowerE"
                                Me.CalculateForBiasRangeLowerE = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "IntegrationEnergyStep"
                                Me.ConvolutionIntegrationStepSize = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "ConvolutionIntegralEPos"
                                Me.ConvolutionIntegralE_POS = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "ConvolutionIntegralENeg"
                                Me.ConvolutionIntegralE_NEG = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "BroadeningStepWidth"
                                Me.dE_BroadeningStepWidth = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "FitDataType"
                                Me.FitDataType = GetFitFunctionTypeFromInteger(Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture))
                        End Select
                    End While
                End If
            End With
        Else
            ' else pass back to base-class
            MyBase.Import_UnknownXMLElementIdentified(XMLElementName, XMLReader)
        End If
    End Sub

#End Region

End Class
