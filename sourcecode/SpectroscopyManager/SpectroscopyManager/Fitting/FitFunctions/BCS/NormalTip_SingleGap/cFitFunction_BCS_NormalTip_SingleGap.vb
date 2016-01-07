Imports System.Threading.Tasks
Imports System.Threading
Imports System.Collections.Concurrent
Imports Cudafy

Public Class cFitFunction_BCS_NormalTip_SingleGap
    Inherits cFitFunction_BCSBase

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifierBCSSingleGap
        Delta_Sample
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSSingleGap.Delta_Sample.ToString, 0.00142, False, My.Resources.rFitFunction_BCSSingleGap.Parameter_Delta_Sample))
        MyBase.InitializeFitParameters()
    End Sub
#End Region

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        ' Register the precalculatable DOS functions.
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSSingleGap.Delta_Sample)
    End Sub


    ''' <summary>
    ''' Returns the CUDA-Classes to compile
    ''' </summary>
    Public Overrides Function GetCudaCompileClasses() As List(Of Type)
        Dim ReturnList As New List(Of Type)
        ReturnList.Add(GetType(cFitFunction_BCSBase))
        ReturnList.Add(GetType(cFitFunction_BCS_NormalTip_SingleGap))
        Return ReturnList
    End Function

    ''' <summary>
    ''' Returns the Sample-DOS.
    ''' </summary>
    Public Overrides Function SampleDOS(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return SampleDOSCUDA(E, InputParameters)
    End Function

    ''' <summary>
    ''' Returns the TIP-DOS
    ''' </summary>
    Public Overrides Function TipDOS(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return TipDOSCUDA(E, InputParameters)
    End Function

    ''' <summary>
    ''' Returns the Sample-DOS.
    ''' </summary>
    Public Function SampleDOSCUDA(ByVal E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return BCSFunc(E,
                       InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSSingleGap.Delta_Sample.ToString).Value,
                       0,
                       0,
                       InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value)
    End Function

    ''' <summary>
    ''' Returns the TIP-DOS
    ''' </summary>
    Public Function TipDOSCUDA(ByVal E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return 1
    End Function

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_BCS_NormalTip_SingleGap
        End Get
    End Property
#End Region

End Class


















'Imports System.Threading.Tasks
'Imports System.Threading
'Imports System.Collections.Concurrent
'Imports Cudafy
'Imports Cudafy.Host
'Imports Cudafy.Translator

'Public Class cFitFunction_BCS_NormalTip_SingleGap
'    Inherits cFitFunction_BCSBase

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
'        Me.FitParameters.Add(FitParameterIdentifier.Delta_Sample, New sFitParameter("SampleGap", 0.00142, False, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_Delta_Sample))
'        Me.FitParameters.Add(FitParameterIdentifier.GlobalXOffset, New sFitParameter("Xoffset", 0, False, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_GlobalXOffset))
'        Me.FitParameters.Add(FitParameterIdentifier.GlobalYOffset, New sFitParameter("Yoffset", 0, False, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_GlobalYOffset))
'        Me.FitParameters.Add(FitParameterIdentifier.GlobalYStretch, New sFitParameter("Ystretch", 1, False, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_GlobalYStretch))
'        Me.FitParameters.Add(FitParameterIdentifier.T_tip, New sFitParameter("TipTemperature", 1.19, True, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_T_tip))
'        Me.FitParameters.Add(FitParameterIdentifier.T_sample, New sFitParameter("SampleTemperature", 1.19, True, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_T_sample))
'        Me.FitParameters.Add(FitParameterIdentifier.BroadeningWidth, New sFitParameter("BroadeningWidth", 0.000055, False, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_BroadeningWidth))
'        Me.FitParameters.Add(FitParameterIdentifier.ImaginaryDamping, New sFitParameter("ImaginaryDamping", 0.000015, False, My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Parameter_ImaginaryDamping))
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

'        ' Check, if BCSDOS needs to be recalculated - speeds up everything
'        Me.PrecalcBCSDOS(FitParameters(FitParameterIdentifier.Delta_Sample).Value,
'                         FitParameters(FitParameterIdentifier.BroadeningWidth).Value,
'                         Me.BroadeningType,
'                         FitParameters(FitParameterIdentifier.ImaginaryDamping).Value)

'        ' Calculate the function value.
'        Return Me.FitFunction(x,
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
'                                          ByVal Delta_sample As Double,
'                                          ByVal T_tip As Double,
'                                          ByVal T_sample As Double,
'                                          ByVal BroadWidth As Double,
'                                          ByVal Broad As Broadening,
'                                          ByVal ImaginaryDamping As Double) As Double

'        Dim FermiDiff As Double = FermiF_eV(E, T_sample) - FermiF_eV(E + eV, T_tip)
'        Dim BCSDOSOverlap As Double = BCSFunc(E, Delta_sample, BroadWidth, Broad, ImaginaryDamping)

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
'    ''' </summary>
'    <Cudafy>
'    Public Shared Function IntegrandECDirectCalc_CUDA(ByVal E As Double,
'                                                 ByVal eV As Double,
'                                                 ByVal Delta_sample As Double,
'                                                 ByVal T_tip As Double,
'                                                 ByVal T_sample As Double,
'                                                 ByVal BroadWidth As Double,
'                                                 ByVal Broad As Integer,
'                                                 ByVal ImaginaryDamping As Double,
'                                                 ByVal Output As Double) As Double

'        Dim FermiSample, FermiTip As Double
'        FermiFeVCUDA(E, T_sample, FermiSample)
'        FermiFeVCUDA(E + eV, T_tip, FermiTip)
'        Dim BCSDOSOverlap As Double
'        BCSFuncCUDA(E, Delta_sample, BroadWidth, Broad, ImaginaryDamping, BCSDOSOverlap)

'        Output = (FermiSample - FermiTip) * BCSDOSOverlap
'        Return Output
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
'        Dim BCSDOSOverlap As Double = BCSFuncPrecalc(E, 1, PrecalcDOSType.Sample)

'        Return FermiDiff * BCSDOSOverlap ' +
'        'FermiF_eV(E, T_tip) * (1 - FermiF_eV(E - eV, T_sample)) * (RatioSample1ToSample2 * BCSFuncPrecalc(E - eV, 1, PrecalcDOSType.Sample1) + BCSFuncPrecalc(E - eV, 1, PrecalcDOSType.Sample2)) * BCSFuncPrecalc(E, 1, PrecalcDOSType.Tip)
'    End Function

'    ''' <summary>
'    ''' Fit-Function, depending on the Selected Fit-Data-Type
'    ''' </summary>
'    Public Function FitFunction(ByVal VBias As Double,
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
'                Dim I As Double
'                FuncI(VBiasEff, Delta_sample, T_tip, T_sample, BroadWidth, Broad, GlobalXOffset, GlobalYOffset, GlobalYStretch, ImaginaryDamping, minE, maxE, I)
'                ReturnVal = I
'            Case FitFunctionType.dIdV
'                Dim ILeft As Double
'                FuncI(VBiasEff + DerivationDistance / 2, Delta_sample, T_tip, T_sample, BroadWidth, Broad, GlobalXOffset, GlobalYOffset, GlobalYStretch, ImaginaryDamping, minE, maxE, ILeft)
'                Dim IRight As Double
'                FuncI(VBiasEff - DerivationDistance / 2, Delta_sample, T_tip, T_sample, BroadWidth, Broad, GlobalXOffset, GlobalYOffset, GlobalYStretch, ImaginaryDamping, minE, maxE, IRight)
'                ReturnVal = (ILeft - IRight) / DerivationDistance
'        End Select

'        ' return the value shifted by the YOffset and stretched by the amplitude
'        Return GlobalYOffset + ReturnVal * GlobalYStretch
'    End Function

'    ''' <summary>
'    ''' Function to calculate the tunneling current I(Vbias)
'    ''' </summary>
'    Public Shared Sub FuncI(ByVal VBias As Double,
'                            ByVal Delta_sample As Double,
'                            ByVal T_tip As Double,
'                            ByVal T_sample As Double,
'                            ByVal BroadWidth As Double,
'                            ByVal Broad As Broadening,
'                            ByVal GlobalXOffset As Double,
'                            ByVal GlobalYOffset As Double,
'                            ByVal GlobalYStretch As Double,
'                            ByVal ImaginaryDamping As Double,
'                            ByVal MinE As Double,
'                            ByVal MaxE As Double,
'                            ByRef Output As Double)

'        ' Create the current integration variable
'        Dim I As Double = 0.0


'        CudafyModes.Target = eGPUType.OpenCL
'        CudafyModes.DeviceId = 0
'        CudafyTranslator.Language = eLanguage.OpenCL
'        Dim gpu As GPGPU = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId)

'        Dim km As CudafyModule = CudafyTranslator.Cudafy(eArchitecture.OpenCL)
'        Try
'            gpu.LoadModule(km)
'        Catch ex As Exception
'            Throw New Exception(km.CompilerOutput)
'        End Try

'        '###############################################
'        ' Because the calculation of the integrand is
'        ' complicated, use the new .NET 4.0 Parallel
'        ' function, to spread the load over all threads
'        ' ---> Parallel approach
'        '###############################################

'        Dim IntegMaxERange As Double = Delta_sample * 1.2

'        If Math.Abs(maxE) > IntegMaxERange Then IntegMaxERange = Math.Abs(maxE) * 1.1
'        If Math.Abs(minE) > IntegMaxERange Then IntegMaxERange = Math.Abs(minE) * 1.1

'        Dim Nmax As Integer = CInt(IntegMaxERange / dECalc)
'        Dim NmaxTotal As Integer = 2 * Nmax - 1

'        Dim dI(NmaxTotal) As Double
'        Dim dE(NmaxTotal) As Double

'        '// allocate the memory on the GPU
'        Dim dev_dE As Double() = gpu.Allocate(Of Double)(dE)
'        Dim dev_dI As Double() = gpu.Allocate(Of Double)(dI)

'        '// fill the arrays de on the CPU
'        For j As Integer = 0 To NmaxTotal Step 1
'            dE(j) = -(Nmax * dECalc) + (j * dECalc)
'        Next

'        '// copy the arrays 'dE' to the GPU
'        gpu.CopyToDevice(dE, dev_dE)

'        '// launch Current-Integration on N threads
'        gpu.Launch(Nmax, 1, "CalcIntegrands", dev_dE, dev_dI, VBias, Delta_sample, T_tip, T_sample, BroadWidth, CInt(Broad), ImaginaryDamping)

'        '// copy the array 'dE' back from the GPU to the CPU
'        gpu.CopyFromDevice(dev_dI, dI)

'        ' Summ up the results
'        '// fill the arrays de on the CPU
'        For j As Integer = 0 To NmaxTotal Step 1
'            I += dI(j)
'        Next

'        '// free the memory allocated on the GPU
'        gpu.Free(dev_dE)
'        gpu.Free(dev_dI)

'        Output = dECalc * I
'    End Sub

'    <Cudafy>
'    Public Shared Sub CalcIntegrands(ByVal thread As GThread,
'                                     ByVal dE As Double(),
'                                     ByVal dI As Double(),
'                                    ByVal eV As Double,
'                                    ByVal Delta_sample As Double,
'                                    ByVal T_tip As Double,
'                                    ByVal T_sample As Double,
'                                    ByVal BroadWidth As Double,
'                                    ByVal Broad As Integer,
'                                    ByVal ImaginaryDamping As Double)
'        Dim tid As Integer = thread.blockIdx.x
'        If tid < dE.Length Then
'            dI(tid) = IntegrandECDirectCalc_CUDA(dE(tid), eV, Delta_sample, T_tip, T_sample, BroadWidth, Broad, ImaginaryDamping, dI(tid))
'        End If
'    End Sub


'#Region "Fermi-Function - CUDA"
'    ''' <summary>
'    ''' // *****************************************************************
'    ''' // * Berechnet die Fermiefunktion (in eV Units)                    *
'    ''' // *                                                               *
'    ''' // *      f(E,T) = 1 / ( exp(E/kT) + 1 )                           *
'    ''' // *****************************************************************
'    ''' </summary>
'    <Cudafy>
'    Public Shared Function FermiFeVCUDA(ByVal E As Double,
'                                     ByVal T As Double,
'                                     ByVal Output As Double) As Double
'        '// ** Berechnungsvariablen; **
'        Dim expo As Double

'        '// ** Fallunterscheidung nach T <= 0.0 K oder T > 0.0 K; **
'        '// ** (Division durch Null abfangen;) **
'        If T <= 0 Then
'            If E < 0D Then
'                Output = 1D
'            Else
'                Output = 0D
'            End If
'        Else
'            '  // ** Berechne Exponent der e-Funktion; **
'            expo = E / (cConstants.kB_eV * T)

'            '  // ** Berechne Fermi-Funktion; Fange Ueberlauf der e-Funktion ab; **
'            If expo < MIN_EXP Or expo > MAX_EXP Then
'                If expo < MIN_EXP Then
'                    Output = 1.0
'                Else
'                    Output = 0.0
'                End If
'            Else
'                Output = 1.0 / (Math.Exp(expo) + 1.0)
'            End If
'        End If
'        Return Output
'    End Function
'#End Region

'#Region "Actual BCS Function with included broadening convolution - CUDA VERSION"
'    ''' <summary>
'    ''' // *****************************************************************
'    ''' // * Berechnet die BCS-Zustandsdichte-Funktion                     *
'    ''' // *                                                               *
'    ''' // *    f(E) = |E|/sqrt( E^2 - D^2 )    :   |E| larger  |D|        *
'    ''' // *           0                        :   |E| smallerequal |D|   *
'    ''' // *                                                               *
'    ''' // * entweder direkt (fBroad == BROAD_OFF) oder gefaltet mit einer *
'    ''' // * normierten    Gauss-    oder    Lorentz-Funktion   (fBroad == *
'    ''' // * BROAD_GAUSS oder BROAD_LORENTZ) g(E) mit Breite w:            *
'    ''' // *                                                               *
'    ''' // *      ( f * g ) (E) = int f(F) * g( E - F ) dF                 *
'    ''' // *                                                               *
'    ''' // * oder mit einer Lorentz-Kurve g(E) mit derselben Breite;       *
'    ''' // *                                                               *
'    ''' // *****************************************************************
'    ''' </summary>
'    <Cudafy>
'    Public Shared Function BCSFuncCUDA(ByVal E As Double,
'                                   ByVal Delta As Double,
'                                   ByVal BroadeningWidth As Double,
'                                   ByVal BroadeningType As Integer,
'                                   ByVal ImaginaryDamping As Double,
'                                   ByVal Output As Double) As Double
'        Output = 0.0

'        Dim result, F, integ, dBCSDOS As Double
'        Dim bufPosEnergy, bufNegEnergy As Double
'        Dim buf3, buf4 As Double

'        ' If case Lorentz or Gauss then if w approx 0 use unbroadened BCS!
'        If BroadeningType = 3 Then
'            Output = BCSDOSCUDA(E, Delta, ImaginaryDamping, Output)
'        ElseIf BroadeningType = 0 Then
'            Output = BCSDOSCUDA(E, Delta, 0, Output)
'        ElseIf (BroadeningWidth > -0.000005 And BroadeningWidth < 0.000005) Then
'            Output = BCSDOSCUDA(E, Delta, ImaginaryDamping, Output)
'        Else
'            ' Numerical Integration to convolute the BCS DOS with the
'            ' specified broadening peak. Convolution may take some time
'            ' until the integrals converge. Especially the Lorentzian!
'            result = 0.0

'            '##############################
'            ' Convolution: OUTSIDE the gap
'            '##############################
'            ' start at a small value larger than delta
'            F = AbsCuda(Delta) + dECalc
'            bufPosEnergy = E - F
'            bufNegEnergy = E + F
'            dBCSDOS = BCSDOSCUDA(F, Delta, ImaginaryDamping, dBCSDOS)
'            Select Case BroadeningType
'                Case 1
'                    buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
'                    integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
'                Case 2
'                    buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
'                    buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
'                    integ = dBCSDOS * (1.0 / buf3 + 1.0 / buf4)
'            End Select

'            ' Stop, if the integrand converges to small values.
'            ' This needs some time for Lorentzian functions.
'            While (F < 1.1 * AbsCuda(E) Or AbsCuda(integ) > (MIN_DIFFERENTIAL * dECalc))
'                bufPosEnergy = E - F
'                bufNegEnergy = E + F
'                dBCSDOS = BCSDOSCUDA(F, Delta, ImaginaryDamping, dBCSDOS)

'                Select Case BroadeningType
'                    Case 1
'                        integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
'                    Case 2
'                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
'                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
'                        integ = dBCSDOS * (1 / buf3 + 1 / buf4)
'                End Select

'                result += integ
'                F += dECalc
'            End While

'            '#################################################
'            ' Convolution: INSIDE inside the gap
'            ' (only, if the imaginary damping factor is <> 0,
'            '  since this is the only reason why there is a
'            '  structure of the BCSDOS inside the gap)
'            '#################################################
'            If ImaginaryDamping <> 0 Then
'                ' Start slightly smaller than the gap energy.
'                F = AbsCuda(Delta) - dECalc
'                bufPosEnergy = E - F
'                bufNegEnergy = E + F
'                dBCSDOS = BCSDOSCUDA(F, Delta, ImaginaryDamping, dBCSDOS)
'                Select Case BroadeningType
'                    Case 1
'                        buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
'                        integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
'                    Case 2
'                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
'                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
'                        integ = dBCSDOS * (1.0 / buf3 + 1.0 / buf4)
'                End Select

'                ' Stop in the integrand is small enough, or at least at half of the gap
'                ' and of course, close to zero, since then the intervals would overlap.
'                While F > dECalc ' Earlier -> created shoulders due to cut-off: (F > 0.5 * Math.Abs(Delta) Or Math.Abs(integ) > (MIN_DIFFERENTIAL * dECalc)) And F > dECalc
'                    bufPosEnergy = E - F
'                    bufNegEnergy = E + F

'                    Select Case BroadeningType
'                        Case 1
'                            dBCSDOS = BCSDOSCUDA(F, Delta, ImaginaryDamping, dBCSDOS)
'                            integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
'                        Case 2
'                            buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
'                            buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
'                            integ = dBCSDOS * (1 / buf3 + 1 / buf4)
'                    End Select

'                    result += integ
'                    F -= dECalc
'                End While
'            End If

'            '  // ** Vorfaktor aufmultiplizieren; **
'            Select Case BroadeningType
'                Case 1
'                    result *= 1.0 / (Math.Sqrt(Math.PI * 2) * BroadeningWidth)
'                Case 2
'                    result *= 2.0 * AbsCuda(BroadeningWidth) / Math.PI
'            End Select

'            '  // ** Breite dE-Intervall aufmultiplizieren; **
'            result *= dECalc

'            Output = result
'            Return Output
'        End If
'    End Function

'    ''' <summary>
'    ''' Actual BCS-DOS function, that may have a small imaginary damping
'    ''' factor to smear out the peak structure by life-time effects.
'    ''' Returns similar results, as a Lorentzian convolution! Due to 
'    ''' calculation time, you should prefer the imaginary damping factor.
'    ''' </summary>
'    <Cudafy>
'    Public Shared Function BCSDOSCUDA(ByVal E As Double,
'                                  ByVal Delta As Double,
'                                  ByVal ImaginaryDamping As Double,
'                                  ByVal Output As Double) As Double
'        'If ImaginaryDamping <> 0.0 Then
'        '    ' Use complex imaginary damping factor in the energy.
'        '    Dim complESq As New Cudafy.Types.ComplexD(E, ImaginaryDamping)
'        '    complESq = Cudafy.Types.ComplexD.Multiply(complESq, complESq)
'        '    Dim complDelta As New Cudafy.Types.ComplexD(Delta, 0)

'        '    ' Calculate the Abs.Real part of the BCS-DOS, that is seen in experiment.
'        '    Output = Math.Sqrt(Cudafy.Types.ComplexD.Abs(Cudafy.Types.ComplexD.Divide(complESq, Cudafy.Types.ComplexD.Subtract(complESq, Cudafy.Types.ComplexD.Multiply(complDelta, complDelta)))))
'        'Else
'        ' Original unbroadened BCS DOS.
'        If AbsCuda(E) > AbsCuda(Delta) Then
'            Output = AbsCuda(E) / Math.Sqrt(E * E - Delta * Delta)
'        Else
'            Output = 0.0
'        End If
'        'End If
'        Return Output
'    End Function

'    <Cudafy>
'    Public Shared Function AbsCuda(ByVal d As Double) As Double
'        If d >= 0 Then
'            Return d
'        Else
'            Return -d
'        End If
'    End Function

'#End Region


'#Region "Precalculation procedure of the BCS DOS, if the parameters have changed"
'    '// Datensets, die die vorausberechnete Zustandsdichten halten (je positive und negative Werte):
'    Private DOSPosX_Sample As Double()
'    Private DOSNegX_Sample As Double()

'    Private DOSPosY_Sample As Double()
'    Private DOSNegY_Sample As Double()

'    Private LastDelta_Sample As Double = -999999998

'    Private LastBroadeningWidth As Double = -9999999998
'    Private LastImaginaryDamping As Double = -9999999998
'    Private LastBroadeningType As Broadening = Broadening.None

'    Private bRecalculateDOS_Sample As Boolean = True

'    ''' <summary>
'    ''' // *****************************************************************
'    ''' // * Berechnet  die BCS-Zustandsdichten neu, falls sich  die Para- *
'    ''' // * meter geaendert haben;                                        *
'    ''' // * Funktion darf nicht veraendert werden, da sie so in bcscalc.h *
'    ''' // * aufgerufen wird;                                              *
'    ''' // *****************************************************************
'    ''' </summary>
'    Public Sub PrecalcBCSDOS(ByVal Delta_Sample As Double,
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

'        Dim NMaxPos As Integer = CInt((Math.Abs(maxE) + BroadeningWidth + dEInterpol) / dEInterpol)
'        Dim NMaxNeg As Integer = CInt((Math.Abs(minE) + BroadeningWidth + dEInterpol) / dEInterpol)

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
'                Case PrecalcDOSType.Sample
'                    DOS = DOSPosY_Sample
'                Case Else
'                    DOS = New Double() {}
'            End Select
'        Else
'            E = -E
'            Select Case DOSType
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
'        Return My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Formula
'    End Function

'    ''' <summary>
'    ''' Name of the fit-function.
'    ''' </summary>
'    Public Overrides Function FitFunctionName() As String
'        Return My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Name
'    End Function

'    ''' <summary>
'    ''' Description of the fit that is performed.
'    ''' </summary>
'    Public Overrides Function FitDescription() As String
'        Return My.Resources.rFitFunction_BCS_NormalTip_SingleGap.Description
'    End Function
'#End Region

'#Region "Fit-Settings-Panel"
'    ''' <summary>
'    ''' Returns the Settings-Panel
'    ''' </summary>
'    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
'        Get
'            Return New cFitSettingPanel_BCS_NormalTip_SingleGap
'        End Get
'    End Property
'#End Region

'End Class