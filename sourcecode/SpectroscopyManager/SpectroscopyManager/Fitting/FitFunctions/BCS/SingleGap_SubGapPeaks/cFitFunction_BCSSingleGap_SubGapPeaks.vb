'Imports System.Threading.Tasks
'Imports System.Threading
'Imports System.Collections.Concurrent

'Public Class cFitFunction_BCSSingleGap_SubGapPeaks
'    Inherits cFitFunction_BCSBase

'#Region "Sub-Gap Peak Definition"
'    ''' <summary>
'    ''' Type of the Sub-Gap-Peaks.
'    ''' </summary>
'    Enum SubGapPeakTypes
'        Lorentz
'        Gauss
'    End Enum

'    ''' <summary>
'    ''' Set this variable to determine the type of sub-gap-peaks used in the fit.
'    ''' </summary>
'    Public SubGapPeakType As SubGapPeakTypes = SubGapPeakTypes.Gauss

'    ''' <summary>
'    ''' Sub-Class to define a sub-gap peak.
'    ''' </summary>
'    Public Class SubGapPeak
'        Public Property XCenter As Double
'        Public Property XCenterFixed As Boolean
'        Public Property Amplitude As Double
'        Public Property AmplitudeFixed As Boolean
'        Public Property Width As Double
'        Public Property WidthFixed As Boolean
'        Public Property PosNegRatio As Double
'        Public Property PosNegRatioFixed As Boolean
'    End Class

'    ''' <summary>
'    ''' List to save all sub-gap-peaks.
'    ''' </summary>
'    Private SubGapPeaks As New List(Of SubGapPeak)

'    ''' <summary>
'    ''' Adds a new sub-gap peak to the fit-function.
'    ''' Returns the index of the internal list, at which the peak got added.
'    ''' </summary>
'    Public Function AddSubGapPeak(ByRef SubGapPeak As SubGapPeak) As Integer
'        Me.SubGapPeaks.Add(SubGapPeak)
'        Return Me.SubGapPeaks.Count - 1
'    End Function

'    ''' <summary>
'    ''' Clears the list of all sub-gap-peaks.
'    ''' </summary>
'    Public Sub ClearSubGapPeaks()
'        Me.SubGapPeaks.Clear()
'    End Sub

'    ''' <summary>
'    ''' Clears the list of all sub-gap-peaks.
'    ''' </summary>
'    Public ReadOnly Property RegisteredSubGapPeaks As List(Of SubGapPeak)
'        Get
'            Return Me.SubGapPeaks
'        End Get
'    End Property

'    ''' <summary>
'    ''' Delegate, that describes a sub-gap-peak-function value.
'    ''' Either a Lorentzian or gaussian!
'    ''' </summary>
'    Private Delegate Function SubGapPeakValue(ByVal x As Double,
'                                              ByVal XCenter As Double,
'                                              ByVal Amplitude As Double,
'                                              ByVal Width As Double,
'                                              ByVal PosNegHeightRatio As Double) As Double

'    ''' <summary>
'    ''' Represents a gaussian sub-gap-peak to be added to the sample-DOS.
'    ''' </summary>
'    Private Function SubGapPeakValue_Gauss(ByVal x As Double,
'                                           ByVal XCenter As Double,
'                                           ByVal Amplitude As Double,
'                                           ByVal Width As Double,
'                                           ByVal PosNegHeightRatio As Double) As Double
'        Return Amplitude * (GaussPeak(x, XCenter, Width) + PosNegHeightRatio * GaussPeak(x, -XCenter, Width))
'    End Function

'    ''' <summary>
'    ''' Represents a lorentzian sub-gap-peak to be added to the sample-DOS.
'    ''' </summary>
'    Private Function SubGapPeakValue_Lorentz(ByVal x As Double,
'                                             ByVal XCenter As Double,
'                                             ByVal Amplitude As Double,
'                                             ByVal Width As Double,
'                                             ByVal PosNegHeightRatio As Double) As Double
'        Return Amplitude * (LorentzPeak(x, XCenter, Width) + PosNegHeightRatio * LorentzPeak(x, -XCenter, Width))
'    End Function

'    ''' <summary>
'    ''' normalized Gaussian Peak
'    ''' </summary>
'    Private Function GaussPeak(ByVal x As Double,
'                               ByVal XCenter As Double,
'                               ByVal Width As Double) As Double
'        Dim d_zs As Double = (x - XCenter) / Width
'        Return Math.Exp(-0.5 * d_zs * d_zs)
'    End Function

'    ''' <summary>
'    ''' normalized Lorentzian Peak.
'    ''' </summary>
'    Private Function LorentzPeak(ByVal x As Double,
'                                 ByVal XCenter As Double,
'                                 ByVal Width As Double) As Double
'        Return Width / (4 * (x - XCenter) * (x - XCenter) + Width * Width) / Math.PI
'    End Function

'    ''' <summary>
'    ''' Gets the integrand contribution to the DOS created by all sub-gap peaks registered!
'    ''' </summary>
'    Private Function GetSubGapPeakContribution(ByVal x As Double) As Double
'        Dim SubGapPeakFunction As SubGapPeakValue

'        ' Determine which function to use
'        If Me.SubGapPeakType = SubGapPeakTypes.Gauss Then
'            SubGapPeakFunction = AddressOf Me.SubGapPeakValue_Gauss
'        Else
'            SubGapPeakFunction = AddressOf Me.SubGapPeakValue_Lorentz
'        End If

'        Dim SubGapPeakContribution As Double = 0

'        ' Sum up all contributions
'        For Each SubGapPeak As SubGapPeak In Me.SubGapPeaks
'            With SubGapPeak
'                SubGapPeakContribution += SubGapPeakFunction(x, .XCenter, .Amplitude, .Width, .PosNegRatio)
'            End With
'        Next

'        Return SubGapPeakContribution
'    End Function

'    ''' <summary>
'    ''' Method that sets all Initial FitParameters from the individual FitFunctions
'    ''' in this multiple-function-container
'    ''' </summary>
'    Public Sub ReInitializeFitParameters()
'        ' Remove old sub-gap peak fit-parameters from the Fit-Parameter-Array
'        Dim FitParameterIdentifiersToRemove As New List(Of Integer)
'        For Each FitParameterIdentifier As Integer In Me.FitParameters.Keys
'            If FitParameterIdentifier >= 100 Then
'                FitParameterIdentifiersToRemove.Add(FitParameterIdentifier)
'            End If
'        Next
'        For i As Integer = 0 To FitParameterIdentifiersToRemove.Count - 1 Step 1
'            Me.FitParameters.Remove(FitParameterIdentifiersToRemove(i))
'        Next

'        ' Modify the Fit-Parameter-Array and add all Sub-Gap-Peaks
'        For i As Integer = 0 To Me.SubGapPeaks.Count - 1 Step 1
'            Dim FitParameterAmplitude As sFitParameter
'            Dim FitParameterWidth As sFitParameter
'            Dim FitParameterPosNegRatio As sFitParameter
'            Dim FitParameterXCenter As sFitParameter

'            Dim SGP As SubGapPeak = Me.SubGapPeaks(i)
'            ' create Fit-Parameter
'            FitParameterAmplitude = New sFitParameter("SGP " & i.ToString & " Amplitude", SGP.Amplitude, SGP.AmplitudeFixed, "SGP " & i.ToString & " Amplitude")
'            FitParameterWidth = New sFitParameter("SGP " & i.ToString & " Width", SGP.Width, SGP.WidthFixed, "SGP " & i.ToString & " Width")
'            FitParameterPosNegRatio = New sFitParameter("SGP " & i.ToString & " +/- ratio", SGP.PosNegRatio, SGP.PosNegRatioFixed, "SGP " & i.ToString & " +/- ratio")
'            FitParameterXCenter = New sFitParameter("SGP " & i.ToString & " Xcenter", SGP.XCenter, SGP.XCenterFixed, "SGP " & i.ToString & " Xcenter")

'            ' add Fit-Parameters
'            With Me.FitParameters
'                .Add(((i + 1) * 100), FitParameterAmplitude)
'                .Add(((i + 1) * 100) + 1, FitParameterWidth)
'                .Add(((i + 1) * 100) + 2, FitParameterPosNegRatio)
'                .Add(((i + 1) * 100) + 3, FitParameterXCenter)
'            End With
'        Next
'    End Sub

'    ''' <summary>
'    ''' This function sets all parameters of the Sub-Gap-Peaks from the Fit-Parameter-Array
'    ''' given by the fit-procedure.
'    ''' </summary>
'    Public Sub SetSubGapPeaksFromFitParameters(ByRef AllFitParameters As Dictionary(Of Integer, sFitParameter))
'        ' Go through all Fit-Parameters and extract those from certain sub-gap-peaks.
'        For Each FitParameterKV As KeyValuePair(Of Integer, sFitParameter) In AllFitParameters
'            ' Ignore original fit-parameters
'            If FitParameterKV.Key < 100 Then Continue For

'            ' Get the SubGapPeak-Index by dividing the Identifier by 100 and cut the rest.
'            Dim SubGapPeakCount As Integer = CInt(FitParameterKV.Key / 100)
'            Dim SubGapPeakIndex As Integer = SubGapPeakCount - 1

'            ' Set the parameter-value only
'            Select Case FitParameterKV.Key
'                Case (SubGapPeakCount * 100)
'                    ' Amplitude
'                    Me.SubGapPeaks(SubGapPeakIndex).Amplitude = FitParameterKV.Value.Value
'                Case (SubGapPeakCount * 100) + 1
'                    ' Width
'                    Me.SubGapPeaks(SubGapPeakIndex).Width = FitParameterKV.Value.Value
'                Case (SubGapPeakCount * 100) + 2
'                    ' PosNegRatio
'                    Me.SubGapPeaks(SubGapPeakIndex).PosNegRatio = FitParameterKV.Value.Value
'                Case (SubGapPeakCount * 100) + 3
'                    ' XCenter
'                    Me.SubGapPeaks(SubGapPeakIndex).XCenter = FitParameterKV.Value.Value
'            End Select
'        Next
'    End Sub
'#End Region

'#Region "Model-Specific Properties"
'    ''' <summary>
'    ''' Type of data that should be fitted.
'    ''' </summary>
'    Public Property FitDataType As FitFunctionType = FitFunctionType.dIdV

'    ''' <summary>
'    ''' Energy-sampling size, that should be used for the numerical derivation
'    ''' of the Current-Function, to get to dIdV.
'    ''' </summary>
'    Public Property dIdVDerivatveStepWidth As Double = 0.01
'#End Region

'#Region "Initialize Fit-Parameters"

'    ''' <summary>
'    ''' Identification Indizes of the Parameters.
'    ''' </summary>
'    Public Enum FitParameterIdentifier
'        Delta_Tip
'        Delta_Sample
'        GlobalXOffset
'        GlobalYOffset
'        GlobalYStretch
'        T_tip
'        T_sample
'        BroadeningWidth
'        ImaginaryDamping
'    End Enum

'    ''' <summary>
'    ''' Method that sets all Initial FitParameters
'    ''' </summary>
'    Protected Overrides Sub InitializeFitParameters()
'        Me.FitParameters.Add(FitParameterIdentifier.Delta_Tip, New sFitParameter("TipGap", 0.00134, True, My.Resources.rFitFunction_BCSDoubleGap.Parameter_Delta_Tip))
'        Me.FitParameters.Add(FitParameterIdentifier.Delta_Sample, New sFitParameter("SampleGap1", 0.00142, False, My.Resources.rFitFunction_BCSDoubleGap.Parameter_Delta_Sample1))
'        Me.FitParameters.Add(FitParameterIdentifier.GlobalXOffset, New sFitParameter("Xoffset", 0, False, My.Resources.rFitFunction_BCSDoubleGap.Parameter_GlobalXOffset))
'        Me.FitParameters.Add(FitParameterIdentifier.GlobalYOffset, New sFitParameter("Yoffset", 0, False, My.Resources.rFitFunction_BCSDoubleGap.Parameter_GlobalYOffset))
'        Me.FitParameters.Add(FitParameterIdentifier.GlobalYStretch, New sFitParameter("Ystretch", 1, False, My.Resources.rFitFunction_BCSDoubleGap.Parameter_GlobalYStretch))
'        Me.FitParameters.Add(FitParameterIdentifier.T_tip, New sFitParameter("TipTemperature", 1.19, True, My.Resources.rFitFunction_BCSDoubleGap.Parameter_T_tip))
'        Me.FitParameters.Add(FitParameterIdentifier.T_sample, New sFitParameter("SampleTemperature", 1.19, True, My.Resources.rFitFunction_BCSDoubleGap.Parameter_T_sample))
'        Me.FitParameters.Add(FitParameterIdentifier.BroadeningWidth, New sFitParameter("BroadeningWidth", 0.000055, False, My.Resources.rFitFunction_BCSDoubleGap.Parameter_BroadeningWidth))
'        Me.FitParameters.Add(FitParameterIdentifier.ImaginaryDamping, New sFitParameter("ImaginaryDamping", 0.000015, False, My.Resources.rFitFunction_BCSDoubleGap.Parameter_ImaginaryDamping))
'    End Sub
'#End Region

'    ''' <summary>
'    ''' Returns the actual FitFunction-Value at a given X
'    ''' </summary>
'    ''' <param name="x">x-value at which the fit function should be evaluated.</param>
'    ''' <param name="FitParameters">Fit Parameters</param>
'    Public Overrides Function GetY(ByRef x As Double, ByRef FitParameters As Dictionary(Of Integer, sFitParameter)) As Double

'        'Return BCSFunc(x, FitParameters(FitParameterIdentifier.Delta_Tip).Value,
'        '               FitParameters(FitParameterIdentifier.BroadeningWidth).Value,
'        '               _BroadeningType,
'        '               FitParameters(FitParameterIdentifier.ImaginaryDamping).Value)

'        ' TEST

'        '#######################################
'        ' Extract the Sub-Gap-Peak parameters
'        ' from the Fit-Parameters-Array
'        Me.SetSubGapPeaksFromFitParameters(FitParameters)
'        ' Sub-Gap-Peaks
'        '#######################################

'        ' Check, if BCSDOS needs to be recalculated - speeds up everything
'        Me.PrecalcBCSDOS(FitParameters(FitParameterIdentifier.Delta_Tip).Value,
'                         FitParameters(FitParameterIdentifier.Delta_Sample).Value,
'                         FitParameters(FitParameterIdentifier.BroadeningWidth).Value,
'                         Me.BroadeningType,
'                         FitParameters(FitParameterIdentifier.ImaginaryDamping).Value)

'        ' Calculate the function value.
'        Return Me.FitFunction(x,
'                              FitParameters(FitParameterIdentifier.Delta_Tip).Value,
'                              FitParameters(FitParameterIdentifier.Delta_Sample).Value,
'                              FitParameters(FitParameterIdentifier.T_tip).Value,
'                              FitParameters(FitParameterIdentifier.T_sample).Value,
'                              FitParameters(FitParameterIdentifier.BroadeningWidth).Value,
'                              Me.BroadeningType,
'                              FitParameters(FitParameterIdentifier.ImaginaryDamping).Value,
'                              FitParameters(FitParameterIdentifier.GlobalXOffset).Value,
'                              FitParameters(FitParameterIdentifier.GlobalYOffset).Value,
'                              FitParameters(FitParameterIdentifier.GlobalYStretch).Value,
'                              _FitDataType,
'                              _dIdVDerivatveStepWidth)
'    End Function

'    ''' <summary>
'    ''' Delegate for an elastic tunneling channel Integrand.
'    ''' </summary>
'    Public Delegate Function IntegrandEC(ByVal E As Double,
'                                         ByVal eV As Double,
'                                         ByVal Delta_tip As Double,
'                                         ByVal Delta_sample As Double,
'                                         ByVal T_tip As Double,
'                                         ByVal T_sample As Double,
'                                         ByVal BroadWidth As Double,
'                                         ByVal Broad As Broadening,
'                                         ByVal ImaginaryDamping As Double) As Double
'    ''' <summary>
'    ''' // *****************************************************************
'    ''' // * Integrand des elastischen Kanals (EC) des Tunnelintegrals zur *
'    ''' // * Berechnung des Stroms I(V):                                   *
'    ''' // *                                                               *
'    ''' // *    Integr_EC = DOS_t(E) * DOS_s(E+eV) * [f(E) - f(E+eV)]      *
'    ''' // *****************************************************************
'    ''' </summary>
'    Public Function IntegrandECDirectCalc(ByVal E As Double,
'                                          ByVal eV As Double,
'                                          ByVal Delta_tip As Double,
'                                          ByVal Delta_sample As Double,
'                                          ByVal T_tip As Double,
'                                          ByVal T_sample As Double,
'                                          ByVal BroadWidth As Double,
'                                          ByVal Broad As Broadening,
'                                          ByVal ImaginaryDamping As Double) As Double

'        Dim FermiDiff As Double = FermiF_eV(E, T_sample) - FermiF_eV(E + eV, T_tip)
'        Dim BCSDOSOverlap As Double = (BCSFunc(E, Delta_sample, BroadWidth, Broad, ImaginaryDamping) _
'                                      + Me.GetSubGapPeakContribution(E)) _
'                                      * BCSFunc(E + eV, Delta_tip, BroadWidth, Broad, ImaginaryDamping)

'        Return FermiDiff * BCSDOSOverlap

'        '// ** Berechnung und Rueckgabe des Integrand; **
'        'Return FermiF_eV(E, T_sample) * (1 - FermiF_eV(E + eV, T_tip)) * (RatioSample1ToSample2 * BCSFunc(E, Delta_sample1, BroadWidth, Broad, ImaginaryDamping) + BCSFunc(E, Delta_sample2, BroadWidth, Broad, ImaginaryDamping)) * BCSFunc(E + eV, Delta_tip, BroadWidth, Broad, ImaginaryDamping) +
'        '       FermiF_eV(E, T_tip) * (1 - FermiF_eV(E - eV, T_sample)) * (RatioSample1ToSample2 * BCSFunc(E - eV, Delta_sample1, BroadWidth, Broad, ImaginaryDamping) + BCSFunc(E - eV, Delta_sample2, BroadWidth, Broad, ImaginaryDamping)) * BCSFunc(E, Delta_tip, BroadWidth, Broad, ImaginaryDamping)
'    End Function

'    ''' <summary>
'    ''' // *****************************************************************
'    ''' // * Integrand des elastischen Kanals (EC) des Tunnelintegrals zur *
'    ''' // * Berechnung des Stroms I(V):                                   *
'    ''' // *                                                               *
'    ''' // *    Integr_EC = DOS_t(E) * DOS_s(E+eV) * [f(E) - f(E+eV)]      *
'    ''' // *****************************************************************
'    ''' 
'    ''' USES PRECALCULATED DOS
'    ''' </summary>
'    Public Function IntegrandECPreCalc(ByVal E As Double,
'                                        ByVal eV As Double,
'                                        ByVal Delta_tip As Double,
'                                        ByVal Delta_sample As Double,
'                                        ByVal T_tip As Double,
'                                        ByVal T_sample As Double,
'                                        ByVal BroadWidth As Double,
'                                        ByVal Broad As Broadening,
'                                        ByVal ImaginaryDamping As Double) As Double
'        '// ** Berechnung und Rueckgabe des Integrand; **
'        'Dim FermiDiffForward As Double = FermiF_eV(E, T_sample) * (1 - FermiF_eV(E + eV, T_tip))
'        'Dim BCSDOSOverlapForward As Double = (RatioSample1ToSample2 * BCSFuncPrecalc(E, 1, PrecalcDOSType.Sample1) + BCSFuncPrecalc(E, 1, PrecalcDOSType.Sample2)) * BCSFuncPrecalc(E + eV, 1, PrecalcDOSType.Tip)

'        Dim FermiDiff As Double = FermiF_eV(E, T_sample) - FermiF_eV(E + eV, T_tip)
'        Dim BCSDOSOverlap As Double = (BCSFuncPrecalc(E, 1, PrecalcDOSType.Sample) _
'                                       + Me.GetSubGapPeakContribution(E)) _
'                                      * BCSFuncPrecalc(E + eV, 1, PrecalcDOSType.Tip)

'        Return FermiDiff * BCSDOSOverlap ' +
'        'FermiF_eV(E, T_tip) * (1 - FermiF_eV(E - eV, T_sample)) * (RatioSample1ToSample2 * BCSFuncPrecalc(E - eV, 1, PrecalcDOSType.Sample1) + BCSFuncPrecalc(E - eV, 1, PrecalcDOSType.Sample2)) * BCSFuncPrecalc(E, 1, PrecalcDOSType.Tip)
'    End Function

'    ''' <summary>
'    ''' Fit-Function, depending on the Selected Fit-Data-Type
'    ''' </summary>
'    Public Function FitFunction(ByVal VBias As Double,
'                                ByVal Delta_tip As Double,
'                                ByVal Delta_sample As Double,
'                                ByVal T_tip As Double,
'                                ByVal T_sample As Double,
'                                ByVal BroadWidth As Double,
'                                ByVal Broad As Broadening,
'                                ByVal ImaginaryDamping As Double,
'                                ByVal GlobalXOffset As Double,
'                                ByVal GlobalYOffset As Double,
'                                ByVal GlobalYStretch As Double,
'                                ByVal FitDataType As FitFunctionType,
'                                Optional ByVal DerivationDistance As Double = 0.001) As Double
'        Dim ReturnVal As Double = 0
'        Dim VBiasEff As Double = GlobalXOffset + VBias

'        ' take the current or the dI/dV-value
'        Select Case FitDataType
'            Case FitFunctionType.I
'                Dim I As Double = FuncI(VBiasEff, Delta_tip, Delta_sample, T_tip, T_sample, BroadWidth, Broad, GlobalXOffset, GlobalYOffset, GlobalYStretch, ImaginaryDamping, UsePrecalculatedBCSDOS)
'                ReturnVal = I
'            Case FitFunctionType.dIdV
'                Dim ILeft As Double = FuncI(VBiasEff + DerivationDistance / 2, Delta_tip, Delta_sample, T_tip, T_sample, BroadWidth, Broad, GlobalXOffset, GlobalYOffset, GlobalYStretch, ImaginaryDamping, UsePrecalculatedBCSDOS)
'                Dim IRight As Double = FuncI(VBiasEff - DerivationDistance / 2, Delta_tip, Delta_sample, T_tip, T_sample, BroadWidth, Broad, GlobalXOffset, GlobalYOffset, GlobalYStretch, ImaginaryDamping, UsePrecalculatedBCSDOS)
'                ReturnVal = (ILeft - IRight) / DerivationDistance
'        End Select

'        ' return the value shifted by the YOffset and stretched by the amplitude
'        Return GlobalYOffset + ReturnVal * GlobalYStretch
'    End Function

'    ''' <summary>
'    ''' Function to calculate the tunneling current I(Vbias)
'    ''' </summary>
'    Public Function FuncI(ByVal VBias As Double,
'                          ByVal Delta_tip As Double,
'                          ByVal Delta_sample As Double,
'                          ByVal T_tip As Double,
'                          ByVal T_sample As Double,
'                          ByVal BroadWidth As Double,
'                          ByVal Broad As Broadening,
'                          ByVal GlobalXOffset As Double,
'                          ByVal GlobalYOffset As Double,
'                          ByVal GlobalYStretch As Double,
'                          ByVal ImaginaryDamping As Double,
'                          Optional ByVal UsePrecalculatedDOS As Boolean = True) As Double

'        ' Create the current integration variable
'        Dim I As Double = 0D
'        'Dim EPV, EPEIEC, EMEIEC, EIEC, PIEC As Double

'        ' Register the elastic channel (EC) integrand Delegate,
'        ' depending on the selection of a pre- or direct-calculated DOS.
'        Dim IntegrandEC As IntegrandEC
'        If UsePrecalculatedDOS Then
'            IntegrandEC = New IntegrandEC(AddressOf IntegrandECPreCalc)
'        Else
'            IntegrandEC = New IntegrandEC(AddressOf IntegrandECDirectCalc)
'        End If

'        '###############################################
'        ' Because the calculation of the integrand is
'        ' complicated, use the new .NET 4.0 Parallel
'        ' function, to spread the load over all threads
'        ' ---> Parallel approach
'        '###############################################
'        Dim NmaxPos As Integer
'        Dim NmaxNeg As Integer

'        ' OLD range!
'        'NmaxPos = Convert.ToInt32(Math.Abs(maxE) / dECalc)
'        'NmaxNeg = Convert.ToInt32(Math.Abs(minE) / dECalc)

'        Dim IntegMaxERange As Double = (Delta_sample + Delta_tip) * 1.2

'        If Math.Abs(maxE) > IntegMaxERange Then IntegMaxERange = Math.Abs(maxE) * 1.1
'        If Math.Abs(minE) > IntegMaxERange Then IntegMaxERange = Math.Abs(minE) * 1.1

'        NmaxPos = CInt(IntegMaxERange / dECalc)
'        NmaxNeg = NmaxPos

'        Dim dIPos(NmaxPos - 1) As Double
'        'ReDim dIPos(NmaxPos - 1)
'        Dim dINeg(NmaxNeg - 1) As Double
'        'ReDim dINeg(NmaxNeg - 1)
'        ' Parallel Approach
'        If NmaxPos > 0 Then
'            Parallel.For(0, NmaxPos, Sub(j As Integer)
'                                         dIPos(j) = IntegrandEC(j * dECalc, VBias, Delta_tip, Delta_sample, T_tip, T_sample, BroadWidth, Broad, ImaginaryDamping)
'                                     End Sub)
'        End If

'        If NmaxNeg > 0 Then
'            Parallel.For(0, NmaxNeg - 1, Sub(j As Integer)
'                                             dINeg(j) = IntegrandEC(-dECalc * (j + 1), VBias, Delta_tip, Delta_sample, T_tip, T_sample, BroadWidth, Broad, ImaginaryDamping)
'                                         End Sub)
'        End If
'        ' Join Data
'        For j As Integer = 0 To NmaxPos - 1 Step 1
'            I += dIPos(j)
'        Next
'        For j As Integer = 0 To NmaxNeg - 1 Step 1
'            I += dINeg(j)
'        Next

'        ' Non-Parallel Approach
'        'Dim E As Double = 0
'        'Dim dI As Double = 0
'        'While E < maxE Or Math.Abs(dI) > MIN_DIFFERENTIAL
'        '    dI = IntegrandEC(E, VBias, Delta_tip, Delta_sample1, Delta_sample2, T_tip, T_sample, BroadWidth, Broad, RatioSample1ToSample2, ImaginaryDamping)
'        '    I += dI
'        '    E += dECalc
'        'End While

'        'E = -dECalc
'        'While E > minE Or Math.Abs(dI) > MIN_DIFFERENTIAL
'        '    dI = IntegrandEC(E, VBias, Delta_tip, Delta_sample1, Delta_sample2, T_tip, T_sample, BroadWidth, Broad, RatioSample1ToSample2, ImaginaryDamping)
'        '    I += dI
'        '    E -= dECalc
'        'End While

'        Return dECalc * I

'        '####################
'        ' old non-parallel processing
'        'While E < maxE 'Or Math.Abs(dI) > MIN_DIFFERENTIAL
'        '    '// ** Integralterm des elastischen Kanals berechnen; **
'        '    dI = IntegrandEC(E, VBias, Delta_tip, Delta_sample1, Delta_sample2, T_tip, T_sample, BroadWidth, Broad)

'        '    '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
'        '    'For iec As Integer = 0 To nIECN - 1 Step 1
'        '    '    '// ** Energie und Wahrscheinlichkeit des inelastischen Kanal; **
'        '    '    EIEC = daIECParam(iec)
'        '    '    PIEC = daIECParam(nIECN + iec)

'        '    '    '// ** Berechne E + E_iec und E - E_iec; **
'        '    '    EPEIEC = E + EIEC
'        '    '    EMEIEC = E - EIEC

'        '    '    '// ** Inelastischen Strom berechnen und aufaddieren; **
'        '    '    dI += PIEC * IntegrandIEC(EPEIEC, EMEIEC, EPV, Tt, Ts)
'        '    'Next
'        '    '// ** Gesamten Integranden (el. + inel. Terme) auf Strom aufaddieren; **
'        '    I += dI

'        '    '// ** E erhoehen; **
'        '    E += dECalc
'        'End While

'        ''// ** Integration ueber negative E; **
'        'E = -dECalc
'        'While E > minE 'Or Math.Abs(dI) > MIN_DIFFERENTIAL
'        '    '// ** Integralterm des elastischen Kanals berechnen und auf Strom aufaddieren; **
'        '    dI = IntegrandEC(E, VBias, Delta_tip, Delta_sample1, Delta_sample2, T_tip, T_sample, BroadWidth, Broad)

'        '    '// ** Alle inelastischen Kanaele berechnen und auf Strom aufaddieren; **
'        '    'For iec As Integer = 0 To nIECN - 1 Step 1
'        '    '    '// ** Energie und Wahrscheinlichkeit des inelastischen Kanal; **
'        '    '    EIEC = daIECParam(iec)
'        '    '    PIEC = daIECParam(nIECN + iec)

'        '    '    '// ** Berechne E + E_iec und E - E_iec; **
'        '    '    EPEIEC = E + EIEC
'        '    '    EMEIEC = E - EIEC

'        '    '    '// ** Inelastischen Strom berechnen und aufaddieren; **
'        '    '    dI += PIEC * IntegrandIEC(EPEIEC, EMEIEC, EPV, Tt, Ts)
'        '    'Next
'        '    '// ** Gesamten Integranden (el. + inel. Terme) auf Strom aufaddieren; **
'        '    I += dI

'        '    '// ** E verringern; **
'        '    E -= dECalc
'        'End While

'        ''// ** Rueckgabe von I; Mit Diskretisierungsintervallbreite und Streckungsparameter M multiplizieren; **
'        ''return M * dE * I;
'        'Return dECalc * I
'    End Function

'#Region "Precalculation procedure of the BCS DOS, if the parameters have changed"
'    '// Datensets, die die vorausberechnete Zustandsdichten halten (je positive und negative Werte):
'    Private DOSPosX_Tip As Double()
'    Private DOSNegX_Tip As Double()
'    Private DOSPosX_Sample As Double()
'    Private DOSNegX_Sample As Double()

'    Private DOSPosY_Tip As Double()
'    Private DOSNegY_Tip As Double()
'    Private DOSPosY_Sample As Double()
'    Private DOSNegY_Sample As Double()

'    Private LastDelta_Sample As Double = -999999998
'    Private LastDelta_Tip As Double = -999999998

'    Private LastBroadeningWidth As Double = -9999999998
'    Private LastImaginaryDamping As Double = -9999999998
'    Private LastBroadeningType As Broadening = Broadening.None

'    Private bRecalculateDOS_Tip As Boolean = True
'    Private bRecalculateDOS_Sample As Boolean = True

'    ''' <summary>
'    ''' // *****************************************************************
'    ''' // * Berechnet  die BCS-Zustandsdichten neu, falls sich  die Para- *
'    ''' // * meter geaendert haben;                                        *
'    ''' // * Funktion darf nicht veraendert werden, da sie so in bcscalc.h *
'    ''' // * aufgerufen wird;                                              *
'    ''' // *****************************************************************
'    ''' </summary>
'    ''' <remarks></remarks>
'    Public Sub PrecalcBCSDOS(ByVal Delta_Tip As Double,
'                             ByVal Delta_Sample As Double,
'                             ByVal BroadeningWidth As Double,
'                             ByVal Broadening As Broadening,
'                             ByVal ImaginaryDamping As Double)
'        ' Check, if a parameter has changed, so that a precalculation is necessary.
'        If Not bRecalculateDOS_Sample And
'            (LastDelta_Sample <> Delta_Sample Or
'             LastBroadeningWidth <> BroadeningWidth Or
'             LastBroadeningType <> Broadening Or
'             LastImaginaryDamping <> ImaginaryDamping) Then
'            bRecalculateDOS_Sample = True
'        End If
'        If Not bRecalculateDOS_Tip And
'            (LastDelta_Tip <> Delta_Tip Or
'             LastBroadeningWidth <> BroadeningWidth Or
'             LastBroadeningType <> Broadening Or
'             LastImaginaryDamping <> ImaginaryDamping) Then
'            bRecalculateDOS_Tip = True
'        End If

'        Dim NMaxPos As Integer = CInt((Math.Abs(maxE) + BroadeningWidth + dEInterpol) / dEInterpol)
'        Dim NMaxNeg As Integer = CInt((Math.Abs(minE) + BroadeningWidth + dEInterpol) / dEInterpol)

'        '// ** Pruefen, ob die Neuberechnung der Spitze durchgefuehrt werden muss; **
'        If bRecalculateDOS_Tip Then
'            '// ** => Fuehre Neuberechnung der Tip-DOS durch; **
'            '// ** In positiver Richtung; **


'            '// ** Erweitere Datensatz; **
'            ReDim DOSPosX_Tip(NMaxPos)
'            ReDim DOSPosY_Tip(NMaxPos)

'            Parallel.For(0, NMaxPos, Sub(j As Integer, LoopState As ParallelLoopState)
'                                         '// ** Setzte Wert; **
'                                         DOSPosX_Tip(j) = dEInterpol * j
'                                         DOSPosY_Tip(j) = BCSFunc(DOSPosX_Tip(j), Delta_Tip, BroadeningWidth, Broadening, ImaginaryDamping)
'                                         If DOSPosX_Tip(j) > CLOSE_TO_ONE Then
'                                             ' abort and set the others to 1
'                                             'LoopState.Break()
'                                         End If
'                                     End Sub)

'            '// ** In negativer Richtung; **

'            '// ** Erweitere Datensatz; **
'            ReDim DOSNegX_Tip(NMaxNeg)
'            ReDim DOSNegY_Tip(NMaxNeg)

'            Parallel.For(0, NMaxNeg, Sub(j As Integer, LoopState As ParallelLoopState)
'                                         '// ** Setzte Wert; **
'                                         DOSNegX_Tip(j) = -dEInterpol * j
'                                         DOSNegY_Tip(j) = BCSFunc(DOSNegX_Tip(j), Delta_Tip, BroadeningWidth, Broadening, ImaginaryDamping)
'                                         If DOSNegX_Tip(j) > CLOSE_TO_ONE Then
'                                             ' abort and set the others to 1
'                                             'LoopState.Break()
'                                         End If
'                                     End Sub)

'            '// ** Setze Flag, das Spitze berechnet wurde; **
'            bRecalculateDOS_Tip = False
'        End If

'        '// ** Pruefen, ob die Neuberechnung der Sample1 durchgefuehrt werden muss; **
'        If bRecalculateDOS_Sample Then
'            '// ** => Fuehre Neuberechnung der Tip-DOS durch; **
'            '// ** In positiver Richtung; **
'            'NMax = CInt(Math.Abs(maxE + BroadeningWidth + dEInterpol) / dEInterpol)

'            '// ** Erweitere Datensatz; **
'            ReDim DOSPosX_Sample(NMaxPos)
'            ReDim DOSPosY_Sample(NMaxPos)

'            Parallel.For(0, NMaxPos, Sub(j As Integer, LoopState As ParallelLoopState)
'                                         '// ** Setzte Wert; **
'                                         DOSPosX_Sample(j) = dEInterpol * j
'                                         DOSPosY_Sample(j) = BCSFunc(DOSPosX_Sample(j), Delta_Sample, BroadeningWidth, Broadening, ImaginaryDamping)
'                                         If DOSPosX_Sample(j) > CLOSE_TO_ONE Then
'                                             ' abort and set the others to 1
'                                             'LoopState.Break()
'                                         End If
'                                     End Sub)

'            '// ** In negativer Richtung; **
'            '--->> Same NMax as before
'            'NMax = CInt(Math.Abs(Delta_Sample1 + BroadeningWidth + dEInterpol) / dEInterpol)

'            '// ** Erweitere Datensatz; **
'            ReDim DOSNegX_Sample(NMaxNeg)
'            ReDim DOSNegY_Sample(NMaxNeg)

'            Parallel.For(0, NMaxNeg, Sub(j As Integer, LoopState As ParallelLoopState)
'                                         '// ** Setzte Wert; **
'                                         DOSNegX_Sample(j) = -dEInterpol * j
'                                         DOSNegY_Sample(j) = BCSFunc(DOSNegX_Sample(j), Delta_Sample, BroadeningWidth, Broadening, ImaginaryDamping)
'                                         If DOSNegX_Sample(j) > CLOSE_TO_ONE Then
'                                             ' abort and set the others to 1
'                                             'LoopState.Break()
'                                         End If
'                                     End Sub)

'            '// ** Setze Flag, das Spitze berechnet wurde; **
'            bRecalculateDOS_Sample = False
'        End If

'        '// ** Vermerke die Werte; **
'        LastDelta_Tip = Delta_Tip
'        LastDelta_Sample = Delta_Sample
'        LastBroadeningWidth = BroadeningWidth
'        LastBroadeningType = Broadening
'        LastImaginaryDamping = ImaginaryDamping
'    End Sub
'#End Region

'#Region "Access to Precalculated BCS-Function"
'    ''' <summary>
'    ''' DOS-Type that the precalc-function should give back.
'    ''' </summary>
'    Public Enum PrecalcDOSType
'        Tip
'        Sample
'    End Enum

'    ''' <summary>
'    ''' Gibt einen Wert fuer die Zustandsdichte der gewünschten DOS zurueck,
'    ''' wobei auf eine Tabelle zurueckgegriffen wird, die vorher mit
'    ''' PrecalcBCSDOS() erzeugt werden muss  
'    ''' Dient zur massiven Beschleunigung der Berechnung
'    ''' </summary>
'    Public Function BCSFuncPrecalc(ByVal E As Double,
'                                   ByVal pad As Double,
'                                   ByVal DOSType As PrecalcDOSType) As Double
'        Dim DOS As Double()
'        If E > 0 Then
'            Select Case DOSType
'                Case PrecalcDOSType.Tip
'                    DOS = DOSPosY_Tip
'                Case PrecalcDOSType.Sample
'                    DOS = DOSPosY_Sample
'                Case Else
'                    DOS = New Double() {}
'            End Select
'        Else
'            E = -E
'            Select Case DOSType
'                Case PrecalcDOSType.Tip
'                    DOS = DOSNegY_Tip
'                Case PrecalcDOSType.Sample
'                    DOS = DOSNegY_Sample
'                Case Else
'                    DOS = New Double() {}
'            End Select
'        End If

'        '  // ** Berechne Logische Position; **
'        Dim n As Integer = Convert.ToInt32(E / dEInterpol)

'        '  // ** Fallunterscheidung; **
'        If n >= DOS.Length - 1 Then
'            Return pad
'        Else
'            '// ** Fuehre Interpolation durch; **
'            Dim d As Double = (E - n * dEInterpol) / dEInterpol
'            Return d * (DOS(n + 1) - DOS(n)) + DOS(n)
'        End If
'    End Function
'#End Region

'#Region "Fit-Description and Formula"
'    ''' <summary>
'    ''' Formula used the fit-function.
'    ''' </summary>
'    Public Overrides Function FitFunctionFormula() As String
'        Return My.Resources.rFitFunction_BCSDoubleGap.Formula
'    End Function

'    ''' <summary>
'    ''' Name of the fit-function.
'    ''' </summary>
'    Public Overrides Function FitFunctionName() As String
'        Return My.Resources.rFitFunction_BCSDoubleGap.Name
'    End Function

'    ''' <summary>
'    ''' Description of the fit that is performed.
'    ''' </summary>
'    Public Overrides Function FitDescription() As String
'        Return My.Resources.rFitFunction_BCSDoubleGap.Description
'    End Function
'#End Region

'#Region "Fit-Settings-Panel"
'    ''' <summary>
'    ''' Returns the Settings-Panel
'    ''' </summary>
'    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
'        Get
'            Return New cFitSettingPanel_BCSSingleGap_SubGapPeaks
'        End Get
'    End Property
'#End Region

'End Class
