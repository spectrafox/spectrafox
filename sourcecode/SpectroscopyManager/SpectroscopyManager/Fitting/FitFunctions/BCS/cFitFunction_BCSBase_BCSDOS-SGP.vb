Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports System.Threading.Tasks
Public MustInherit Class cFitFunction_BCSBase
    Inherits cFitFunction_TipSampleConvolutionBase
    Implements iFitFunction_SubGapPeaks

    ''' <summary>
    ''' Abbruchwert der BCS-DOS-Funktion
    ''' </summary>
    Public Const CLOSE_TO_ONE As Double = 1.001

    ''' <summary>
    ''' Cutoff for Integrals, if the value becomes irrelevant small.
    ''' </summary>
    Public Const MIN_DIFFERENTIAL As Double = 0.001

    ''' <summary>
    ''' BCS-precalculation interpolation-interval-width:
    ''' </summary>
    Protected Const dEInterpol As Double = 0.000001

    ''' <summary>
    ''' Energy sampling size in [eV]
    ''' Used for calculation of the current integral.
    ''' This is the step size between which the current is summed up.
    ''' (irrational for minizing numerical artifacts)
    ''' </summary>
    Protected Const dECalc As Double = 0.000003090169943749474241023D

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifierBCSBase
        BroadeningWidth = 20
        ImaginaryDamping = 21
        BCSAmplitude = 22
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(FitParameterIdentifierBCSBase.BroadeningWidth, New sFitParameter("BroadeningWidth", 0.000055, False, My.Resources.rFitFunction_BCSBase.Parameter_BroadeningWidth))
        Me.FitParameters.Add(FitParameterIdentifierBCSBase.ImaginaryDamping, New sFitParameter("ImaginaryDamping", 0.000015, False, My.Resources.rFitFunction_BCSBase.Parameter_ImaginaryDamping))
        Me.FitParameters.Add(FitParameterIdentifierBCSBase.BCSAmplitude, New sFitParameter("BCSAmplitude", 1, False, My.Resources.rFitFunction_BCSBase.Parameter_BCSAmplitude))
        MyBase.InitializeFitParameters()
    End Sub
#End Region

#Region "Properties such as the BroadeningType"
    ''' <summary>
    ''' Type of Broadening applied to the BCS function!
    ''' </summary>
    Public Property BroadeningType As Broadening = Broadening.ImaginaryDamping

    ''' <summary>
    ''' Changes the range in which the fit-function is defined.
    ''' Needed for convolution integrals and current integrals to estimate
    ''' the range of values to calculate the integral over.
    ''' </summary>
    Public Overrides Sub ChangeFitRangeX(LowerValue As Double, HigherValue As Double)
        ' DO NOTHING!!!!
        ' This would create artifacts for partial function fitting!
        ' Instead call the ChangeMinEMaxEValue, and set it to the full-data-range!
        'Me.minE = LowerValue
        'Me.maxE = HigherValue
    End Sub

    ''' <summary>
    ''' Changes the range in which the fit-function is defined.
    ''' Needed for convolution integrals and current integrals to estimate
    ''' the range of values to calculate the integral over.
    ''' </summary>
    Public Sub SetBCSEnergyRange(LowerValue As Double, HigherValue As Double)
        Me.ConvolutionIntegralMinE = LowerValue
        Me.ConvolutionIntegralMaxE = HigherValue
    End Sub
#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    Public Sub New()
#If DEBUG Then
        Me._FunctionImplementsCUDAVersion = True
#Else
        Me._FunctionImplementsCUDAVersion = False ' erst noch testen
#End If
    End Sub

#End Region

#Region "Type definition for Broadening, DOSType, etc."
    ''' <summary>
    ''' Type of Broadening applied to peaks.
    ''' </summary>
    Public Enum Broadening
        None
        Gauss
        Lorentz
        ImaginaryDamping
    End Enum
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
    <Cudafy.Cudafy>
    Public Shared Shadows Function FermiF_eV(ByVal E As Double,
                                             ByVal T As Double) As Double
        '// ** Berechnungsvariablen; **
        Dim f, expo As Double

        '// ** Fallunterscheidung nach T <= 0.0 K oder T > 0.0 K; **
        '// ** (Division durch Null abfangen;) **
        If T <= 0 Then
            If E < 0.0 Then
                f = 1.0
            Else
                f = 0.0
            End If
        Else
            '  // ** Berechne Exponent der e-Funktion; **
            expo = E / (cConstants.kB_eV * T)

            '  // ** Berechne Fermi-Funktion; Fange Ueberlauf der e-Funktion ab; **
            If expo < MIN_EXP Then
                f = 0.0
            ElseIf expo > MAX_EXP Then
                f = 1.0
            Else
                f = 1.0 / (Math.Exp(expo) + 1.0)
            End If
        End If

        Return f
    End Function
#End Region

#Region "Actual BCS Function with included broadening convolution - CUDA compatible"
    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet die BCS-Zustandsdichte-Funktion                     *
    ''' // *                                                               *
    ''' // *    f(E) = |E|/sqrt( E^2 - D^2 )    :   |E| larger  |D|        *
    ''' // *           0                        :   |E| smallerequal |D|   *
    ''' // *                                                               *
    ''' // * entweder direkt (fBroad == BROAD_OFF) oder gefaltet mit einer *
    ''' // * normierten    Gauss-    oder    Lorentz-Funktion   (fBroad == *
    ''' // * BROAD_GAUSS oder BROAD_LORENTZ) g(E) mit Breite w:            *
    ''' // *                                                               *
    ''' // *      ( f * g ) (E) = int f(F) * g( E - F ) dF                 *
    ''' // *                                                               *
    ''' // * oder mit einer Lorentz-Kurve g(E) mit derselben Breite;       *
    ''' // *                                                               *
    ''' // *****************************************************************
    ''' 
    ''' CUDA compatible version
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function BCSFunc(ByVal E As Double,
                                   ByVal Delta As Double,
                                   ByVal BroadeningWidth As Double,
                                   ByVal BroadeningType As Integer,
                                   ByVal ImaginaryDamping As Double) As Double

        Dim result, F, integ, dBCSDOS As Double
        Dim bufPosEnergy, bufNegEnergy As Double
        Dim buf3, buf4 As Double

        ' If case Lorentz or Gauss then if w approx 0 use unbroadened BCS!
        If BroadeningType = Broadening.ImaginaryDamping Then
            Return BCSDOSExpanded(E, Delta, ImaginaryDamping)
        ElseIf BroadeningType = Broadening.None Then
            Return BCSDOSExpanded(E, Delta, 0)
        ElseIf (BroadeningWidth > -0.000005 And BroadeningWidth < 0.000005) Then
            Return BCSDOSExpanded(E, Delta, ImaginaryDamping)
        Else
            ' Numerical Integration to convolute the BCS DOS with the
            ' specified broadening peak. Convolution may take some time
            ' until the integrals converge. Especially the Lorentzian!
            result = 0.0

            '##############################
            ' Convolution: OUTSIDE the gap
            '##############################
            ' start at a small value larger than delta
            F = cNumericFunctions.MathAbs(Delta) + dECalc
            bufPosEnergy = E - F
            bufNegEnergy = E + F
            dBCSDOS = BCSDOSExpanded(F, Delta, ImaginaryDamping)
            Select Case BroadeningType
                Case Broadening.Gauss
                    buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
                    integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                Case Broadening.Lorentz
                    buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                    buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                    integ = dBCSDOS * (1.0 / buf3 + 1.0 / buf4)
            End Select

            ' Stop, if the integrand converges to small values.
            ' This needs some time for Lorentzian functions.
            While (F < 1.1 * cNumericFunctions.MathAbs(E) Or cNumericFunctions.MathAbs(integ) > (MIN_DIFFERENTIAL * dECalc))
                bufPosEnergy = E - F
                bufNegEnergy = E + F
                dBCSDOS = BCSDOSExpanded(F, Delta, ImaginaryDamping)

                Select Case BroadeningType
                    Case Broadening.Gauss
                        integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                    Case Broadening.Lorentz
                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                        integ = dBCSDOS * (1 / buf3 + 1 / buf4)
                End Select

                result += integ
                F += dECalc
            End While

            '#################################################
            ' Convolution: INSIDE inside the gap
            ' (only, if the imaginary damping factor is <> 0,
            '  since this is the only reason why there is a
            '  structure of the BCSDOS inside the gap)
            '#################################################
            If ImaginaryDamping <> 0 Then
                ' Start slightly smaller than the gap energy.
                F = cNumericFunctions.MathAbs(Delta) - dECalc
                bufPosEnergy = E - F
                bufNegEnergy = E + F
                dBCSDOS = BCSDOSExpanded(F, Delta, ImaginaryDamping)
                Select Case BroadeningType
                    Case Broadening.Gauss
                        buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
                        integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                    Case Broadening.Lorentz
                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                        integ = dBCSDOS * (1.0 / buf3 + 1.0 / buf4)
                End Select

                ' Stop in the integrand is small enough, or at least at half of the gap
                ' and of course, close to zero, since then the intervals would overlap.
                While F > dECalc ' Earlier -> created shoulders due to cut-off: (F > 0.5 * Abs(Delta) Or Abs(integ) > (MIN_DIFFERENTIAL * dECalc)) And F > dECalc
                    bufPosEnergy = E - F
                    bufNegEnergy = E + F

                    Select Case BroadeningType
                        Case Broadening.Gauss
                            dBCSDOS = BCSDOSExpanded(F, Delta, ImaginaryDamping)
                            integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                        Case Broadening.Lorentz
                            buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                            buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                            integ = dBCSDOS * (1 / buf3 + 1 / buf4)
                    End Select

                    result += integ
                    F -= dECalc
                End While
            End If

            '  // ** Vorfaktor aufmultiplizieren; **
            Select Case BroadeningType
                Case Broadening.Gauss
                    result *= 1.0 / (Math.Sqrt(MathNet.Numerics.Constants.Pi2) * BroadeningWidth)
                Case Broadening.Lorentz
                    result *= 2.0 * cNumericFunctions.MathAbs(BroadeningWidth) / MathNet.Numerics.Constants.Pi
            End Select

            '  // ** Breite dE-Intervall aufmultiplizieren; **
            result *= dECalc
        End If

        Return result
    End Function

    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet die BCS-Zustandsdichte-Funktion                     *
    ''' // *                                                               *
    ''' // *    f(E) = |E|/sqrt( E^2 - D^2 )    :   |E| larger  |D|        *
    ''' // *           0                        :   |E| smallerequal |D|   *
    ''' // *                                                               *
    ''' // * entweder direkt (fBroad == BROAD_OFF) oder gefaltet mit einer *
    ''' // * normierten    Gauss-    oder    Lorentz-Funktion   (fBroad == *
    ''' // * BROAD_GAUSS oder BROAD_LORENTZ) g(E) mit Breite w:            *
    ''' // *                                                               *
    ''' // *      ( f * g ) (E) = int f(F) * g( E - F ) dF                 *
    ''' // *                                                               *
    ''' // * oder mit einer Lorentz-Kurve g(E) mit derselben Breite;       *
    ''' // *                                                               *
    ''' // *****************************************************************
    ''' 
    ''' including SubGapPeaks given by the arrays
    ''' CUDA compatible version
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function BCSFuncSGP(ByVal E As Double,
                                       ByVal Delta As Double,
                                       ByVal BroadeningWidth As Double,
                                       ByVal BroadeningType As Integer,
                                       ByVal ImaginaryDamping As Double,
                                       ByVal BCSAmplitude As Double,
                                       ByVal SGP_XCenters As Double(),
                                       ByVal SGP_Amplitudes As Double(),
                                       ByVal SGP_Widths As Double(),
                                       ByVal SGP_PosNegRatios As Double(),
                                       ByVal SGP_UseLorentz As Integer,
                                       ByVal AddSGPToThisDOS As Byte) As Double

        Dim result, F, integ, dBCSDOS As Double
        Dim bufPosEnergy, bufNegEnergy As Double
        Dim buf3, buf4 As Double

        ' If case Lorentz or Gauss then if w approx 0 use unbroadened BCS!
        If BroadeningType = Broadening.ImaginaryDamping Then
            Return BCSDOSExpandedSGP(E, Delta, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)
        ElseIf BroadeningType = Broadening.None Then
            Return BCSDOSExpandedSGP(E, Delta, 0, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)
        ElseIf (BroadeningWidth > -0.000005 And BroadeningWidth < 0.000005) Then
            Return BCSDOSExpandedSGP(E, Delta, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)
        Else
            ' Numerical Integration to convolute the BCS DOS with the
            ' specified broadening peak. Convolution may take some time
            ' until the integrals converge. Especially the Lorentzian!
            result = 0.0

            '##############################
            ' Convolution: OUTSIDE the gap
            '##############################
            ' start at a small value larger than delta
            F = cNumericFunctions.MathAbs(Delta) + dECalc
            bufPosEnergy = E - F
            bufNegEnergy = E + F
            dBCSDOS = BCSDOSExpandedSGP(F, Delta, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)
            Select Case BroadeningType
                Case Broadening.Gauss
                    buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
                    integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                Case Broadening.Lorentz
                    buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                    buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                    integ = dBCSDOS * (1.0 / buf3 + 1.0 / buf4)
            End Select

            ' Stop, if the integrand converges to small values.
            ' This needs some time for Lorentzian functions.
            While (F < 1.1 * cNumericFunctions.MathAbs(E) Or cNumericFunctions.MathAbs(integ) > (MIN_DIFFERENTIAL * dECalc))
                bufPosEnergy = E - F
                bufNegEnergy = E + F
                dBCSDOS = BCSDOSExpandedSGP(F, Delta, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)

                Select Case BroadeningType
                    Case Broadening.Gauss
                        integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                    Case Broadening.Lorentz
                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                        integ = dBCSDOS * (1 / buf3 + 1 / buf4)
                End Select

                result += integ
                F += dECalc
            End While

            '#################################################
            ' Convolution: INSIDE inside the gap
            ' (only, if the imaginary damping factor is <> 0,
            '  since this is the only reason why there is a
            '  structure of the BCSDOS inside the gap)
            '#################################################
            If ImaginaryDamping <> 0 Then
                ' Start slightly smaller than the gap energy.
                F = cNumericFunctions.MathAbs(Delta) - dECalc
                bufPosEnergy = E - F
                bufNegEnergy = E + F
                dBCSDOS = BCSDOSExpandedSGP(F, Delta, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)
                Select Case BroadeningType
                    Case Broadening.Gauss
                        buf3 = -1.0 / (2.0 * BroadeningWidth * BroadeningWidth)
                        integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                    Case Broadening.Lorentz
                        buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                        buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                        integ = dBCSDOS * (1.0 / buf3 + 1.0 / buf4)
                End Select

                ' Stop in the integrand is small enough, or at least at half of the gap
                ' and of course, close to zero, since then the intervals would overlap.
                While F > dECalc ' Earlier -> created shoulders due to cut-off: (F > 0.5 * Abs(Delta) Or Abs(integ) > (MIN_DIFFERENTIAL * dECalc)) And F > dECalc
                    bufPosEnergy = E - F
                    bufNegEnergy = E + F

                    Select Case BroadeningType
                        Case Broadening.Gauss
                            dBCSDOS = BCSDOSExpandedSGP(F, Delta, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)
                            integ = dBCSDOS * (Math.Exp(buf3 * bufPosEnergy * bufPosEnergy) + Math.Exp(buf3 * bufNegEnergy * bufNegEnergy))
                        Case Broadening.Lorentz
                            buf3 = BroadeningWidth * BroadeningWidth + bufPosEnergy * bufPosEnergy
                            buf4 = BroadeningWidth * BroadeningWidth + bufNegEnergy * bufNegEnergy
                            integ = dBCSDOS * (1 / buf3 + 1 / buf4)
                    End Select

                    result += integ
                    F -= dECalc
                End While
            End If

            '  // ** Vorfaktor aufmultiplizieren; **
            Select Case BroadeningType
                Case Broadening.Gauss
                    result *= 1.0 / (Math.Sqrt(MathNet.Numerics.Constants.Pi2) * BroadeningWidth)
                Case Broadening.Lorentz
                    result *= 2.0 * cNumericFunctions.MathAbs(BroadeningWidth) / MathNet.Numerics.Constants.Pi
            End Select

            '  // ** Breite dE-Intervall aufmultiplizieren; **
            result *= dECalc
        End If

        Return result
    End Function

    ''' <summary>
    ''' Actual BCS-DOS function, that may have a small imaginary damping
    ''' factor to smear out the peak structure by life-time effects.
    ''' Returns similar results, as a Lorentzian convolution! Due to 
    ''' calculation time, you should prefer the imaginary damping factor.
    ''' 
    ''' Used before, but is CUDA-incompatible and by approx. 16% slower.
    ''' </summary>
    Public Shared Function BCSDOSComplex(ByRef E As Double, ByRef Delta As Double, ByRef ImaginaryDamping As Double) As Double
        If ImaginaryDamping <> 0 Then
            ' Use complex imaginary damping factor in the energy.
            Dim complE As New Numerics.Complex(E, ImaginaryDamping)
            Dim complDelta As New Numerics.Complex(Delta, 0)

            ' Calculate the Abs.Real part of the BCS-DOS, that is seen in experiment.
            Return cNumericFunctions.MathAbs((complE / Numerics.Complex.Sqrt(complE * complE - complDelta * complDelta)).Real)
        Else
            ' Original unbroadened BCS DOS.
            If cNumericFunctions.MathAbs(E) > cNumericFunctions.MathAbs(Delta) Then
                Return cNumericFunctions.MathAbs(E) / Math.Sqrt(E * E - Delta * Delta)
            Else
                Return 0.0
            End If
        End If
    End Function

    ''' <summary>
    ''' Actual BCS-DOS function, that may have a small imaginary damping
    ''' factor to smear out the peak structure by life-time effects.
    ''' Returns similar results, as a Lorentzian convolution! Due to 
    ''' calculation time, you should prefer the imaginary damping factor.
    ''' 
    ''' Function got expanded by Mathematica to avoid the need of complex numbers.
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function BCSDOSExpanded(ByVal E As Double, ByVal Delta As Double, ByVal ImaginaryDamping As Double) As Double
        ' Calculate needed variables to save time
        Dim ESq As Double = E * E
        Dim DeltaSq As Double = Delta * Delta

        If ImaginaryDamping <> 0 Then
            Dim ImSq As Double = ImaginaryDamping * ImaginaryDamping

            Dim Arg As Double = 0.5 * Math.Atan2(2 * ImaginaryDamping * E, ESq - ImSq - DeltaSq)
            Dim S3 As Double = ImSq + DeltaSq - ESq
            Return cNumericFunctions.MathAbs((E * Math.Cos(Arg) + ImaginaryDamping * Math.Sin(Arg)) / Math.Sqrt(Math.Sqrt(4 * ImSq * ESq + S3 * S3)))
        Else
            ' Original unbroadened BCS DOS.
            If ESq > DeltaSq Then
                Return cNumericFunctions.MathAbs(E) / Math.Sqrt(ESq - DeltaSq)
            Else
                Return 0.0
            End If
        End If
    End Function

    ''' <summary>
    ''' Actual BCS-DOS function, that may have a small imaginary damping
    ''' factor to smear out the peak structure by life-time effects.
    ''' Returns similar results, as a Lorentzian convolution! Due to 
    ''' calculation time, you should prefer the imaginary damping factor.
    ''' 
    ''' Function got expanded by Mathematica to avoid the need of complex numbers.
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function BCSDOSExpandedSGP(ByVal E As Double,
                                             ByVal Delta As Double,
                                             ByVal ImaginaryDamping As Double,
                                             ByVal BCSAmplitude As Double,
                                             ByVal SGP_XCenters As Double(),
                                             ByVal SGP_Amplitudes As Double(),
                                             ByVal SGP_Widths As Double(),
                                             ByVal SGP_PosNegRatios As Double(),
                                             ByVal SGP_UseLorentz As Integer,
                                             ByVal AddSGPContribution As Byte) As Double
        If AddSGPContribution = 1 Then
            Return (BCSAmplitude * BCSDOSExpanded(E, Delta, ImaginaryDamping)) + GetSubGapPeakContribution(E, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz)
        Else
            Return BCSAmplitude * BCSDOSExpanded(E, Delta, ImaginaryDamping)
        End If
    End Function

#End Region

#Region "CUDA Kernel and initialization"

    ' ''' <summary>
    ' ''' Calculates in parallel the BCSDOS for all given E on the GPU.
    ' ''' </summary>
    '<Cudafy.Cudafy>
    'Public Shared Sub BCSDOSCudaKernel(ByVal thread As Cudafy.GThread,
    '                                   ByVal E_In As Double(),
    '                                   ByVal DOS_Out As Double(),
    '                                   ByVal Delta As Double,
    '                                   ByVal BroadeningWidth As Double,
    '                                   ByVal BroadeningType As Integer,
    '                                   ByVal ImaginaryDamping As Double)
    '    Dim tid As Integer = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x
    '    If tid < E_In.Length Then
    '        DOS_Out(tid) = BCSFunc(E_In(tid), Delta, BroadeningWidth, BroadeningType, ImaginaryDamping)
    '    End If
    '    'Dim tid As Integer = thread.blockIdx.x
    '    'If tid < E_In.Length Then
    '    '    DOS_Out(tid) = BCSFunc(E_In(tid), Delta, BroadeningWidth, BroadeningType, ImaginaryDamping)
    '    'End If
    'End Sub

    ''' <summary>
    ''' Calculates in parallel the BCSDOS for all given E on the GPU.
    ''' Includes the SubGapPeaks defined by the four arrays.
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Sub BCSDOS_SGP_CudaKernel(ByVal thread As Cudafy.GThread,
                                            ByVal E_In As Double(),
                                            ByVal DOS_Out As Double(),
                                            ByVal Delta As Double,
                                            ByVal BroadeningWidth As Double,
                                            ByVal BroadeningType As Integer,
                                            ByVal ImaginaryDamping As Double,
                                            ByVal BCSAmplitude As Double,
                                            ByVal SGP_XCenters As Double(),
                                            ByVal SGP_Amplitudes As Double(),
                                            ByVal SGP_Widths As Double(),
                                            ByVal SGP_PosNegRatios As Double(),
                                            ByVal SGP_UseLorentz As Integer,
                                            ByVal AddSGPToThisDOS As Byte)
        Dim tid As Integer = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x
        If tid < E_In.Length Then
            DOS_Out(tid) = BCSFuncSGP(E_In(tid), Delta, BroadeningWidth, BroadeningType, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPToThisDOS)
        End If
    End Sub

#End Region

#Region "Precalculate BCS DOS for acceleration."
    Private LastBroadeningWidth As Double = Double.MinValue
    Private LastImaginaryDamping As Double = Double.MinValue
    Private LastBCSAmplitude As Double = Double.MinValue
    Private LastBroadeningType As Broadening = Broadening.None

    ''' <summary>
    ''' Last SGP parameters
    ''' </summary>
    Private Last_SGP_XCenters() As Double = New Double() {}
    Private Last_SGP_Amplitudes() As Double = New Double() {}
    Private Last_SGP_Widths() As Double = New Double() {}
    Private Last_SGP_PosNegRatios() As Double = New Double() {}

    ''' <summary>
    ''' Storage for the precalculated X-array on the CPU, that gets copied to the GPU
    ''' </summary>
    Private PrecalcDOSX As Double()

    ''' <summary>
    ''' Storage for the precalculated X-array on the GPU, that gets copied from the CPU
    ''' </summary>
    Private dev_DOSX As Double()

    Private NMaxPos As Integer
    Private NMaxNeg As Integer
    Private NMaxTotal As Integer

    Private bRecalculateDOS_Tip As Boolean = True
    Private bRecalculateDOS_Sample1 As Boolean = True
    Private bRecalculateDOS_Sample2 As Boolean = True

    ''' <summary>
    ''' Saves the BCS-DOS-arrays filled in the precalculation procedure.
    ''' The Integer-Key is filled with the registered identifier.
    ''' </summary>
    Private Precalc_BCSDOSArrays As New Dictionary(Of Integer, Double())

    ''' <summary>
    ''' Saves if the BCSDOS for this identifier has to be recalculated.
    ''' </summary>
    Private Precalc_RecalculationNeeded As New Dictionary(Of Integer, Boolean)

    ''' <summary>
    ''' Saves if the SGP-Contribution should be added to this DOS.
    ''' </summary>
    Private Precalc_AddSGPContribution As New Dictionary(Of Integer, Boolean)

    ''' <summary>
    ''' Saves the last Delta-Value for the given Identifier,
    ''' to decide, if a recalculation is necessary.
    ''' </summary>
    Private Precalc_LastDelta As New Dictionary(Of Integer, Double)

    ''' <summary>
    ''' Saves the registered identifiers.
    ''' </summary>
    Private Precalc_RegisteredIdentifiers As New List(Of Integer)

    ''' <summary>
    ''' Register a DOS for precalculation.
    ''' </summary>
    Public Sub RegisterPrecalcDOS(ByVal Identifier As Integer, Optional ByVal AddSGPContributionToThis As Boolean = False)
        If Me.Precalc_RegisteredIdentifiers.Contains(Identifier) Then Throw New ArgumentException("Precalculation identifier key already exists.")

        Me.Precalc_RegisteredIdentifiers.Add(Identifier)
        Me.Precalc_BCSDOSArrays.Add(Identifier, New Double() {})
        Me.Precalc_RecalculationNeeded.Add(Identifier, True)
        Me.Precalc_LastDelta.Add(Identifier, Double.MinValue)
        Me.Precalc_AddSGPContribution.Add(Identifier, AddSGPContributionToThis)
    End Sub

    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet  die BCS-Zustandsdichten neu, falls sich  die Para- *
    ''' // * meter geaendert haben;                                        *
    ''' // * Funktion darf nicht veraendert werden, da sie so in bcscalc.h *
    ''' // * aufgerufen wird;                                              *
    ''' // *****************************************************************
    ''' </summary>
    Public Sub PrecalcBCSDOS(ByVal DeltaDictionary As Dictionary(Of Integer, Double),
                             ByVal BroadeningWidth As Double,
                             ByVal Broadening As Broadening,
                             ByVal ImaginaryDamping As Double,
                             ByVal BCSAmplitude As Double,
                             ByVal SGP_Identifiers As Integer(),
                             ByVal SGP_XCenters As Double(),
                             ByVal SGP_Amplitudes As Double(),
                             ByVal SGP_Widths As Double(),
                             ByVal SGP_PosNegRatios As Double())
        Dim iBroadening As Integer = CInt(Broadening)

        Dim bRecalculationForAllNeeded As Boolean = False
        Dim bRecalculationAtLeastForOneNeeded As Boolean = False

        ' Check, if a parameter has changed, so that a precalculation is necessary for all registered DOS.
        If LastBroadeningWidth <> BroadeningWidth Or
            LastBroadeningType <> Broadening Or
            LastImaginaryDamping <> ImaginaryDamping Or
            LastBCSAmplitude <> BCSAmplitude Then

            bRecalculationForAllNeeded = True
            bRecalculationAtLeastForOneNeeded = True
        End If

        Dim tmpIdentifier As Integer
        ' If not for all, then check the individual precalculations necessary.
        If Not bRecalculationForAllNeeded Then
            For i As Integer = 0 To Me.Precalc_RegisteredIdentifiers.Count - 1 Step 1
                tmpIdentifier = Me.Precalc_RegisteredIdentifiers(i)
                If Me.Precalc_LastDelta(tmpIdentifier) <> DeltaDictionary(tmpIdentifier) Then
                    ' save that we need to recalculate this registered DOS.
                    Me.Precalc_RecalculationNeeded(tmpIdentifier) = True

                    ' Save the delta now to the cache-array.
                    Me.Precalc_LastDelta(tmpIdentifier) = DeltaDictionary(tmpIdentifier)

                    ' Save that we need at least one recalculation
                    If Not bRecalculationAtLeastForOneNeeded Then bRecalculationAtLeastForOneNeeded = True
                End If
            Next
        Else
            For i As Integer = 0 To Me.Precalc_RegisteredIdentifiers.Count - 1 Step 1
                tmpIdentifier = Me.Precalc_RegisteredIdentifiers(i)
                ' Save the delta now to the cache-array.
                Me.Precalc_LastDelta(tmpIdentifier) = DeltaDictionary(tmpIdentifier)
            Next
        End If

        '##################################
        ' prepare Sub-Gap-Peaks, if needed
        '
        ' Caching of the SGPs in separate arrays
        Dim SGP_UseLorentz As Integer
        If Me.SubGapPeakType = iFitFunction_SubGapPeaks.SubGapPeakTypes.Lorentz Then
            SGP_UseLorentz = 1
        Else
            SGP_UseLorentz = 0
        End If

        ' get the contributions
        If Me.Precalc_AddSGPContribution.ContainsValue(True) And Not bRecalculationForAllNeeded Then
            ' Check, if something has changed
            If Not Me.AreSGPArraysTheSame(SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios,
                                          Last_SGP_XCenters, Last_SGP_Amplitudes, Last_SGP_Widths, Last_SGP_PosNegRatios) Then
                For i As Integer = 0 To Me.Precalc_RegisteredIdentifiers.Count - 1 Step 1
                    tmpIdentifier = Me.Precalc_RegisteredIdentifiers(i)
                    If Me.Precalc_AddSGPContribution(tmpIdentifier) Then
                        Me.Precalc_RecalculationNeeded(tmpIdentifier) = True
                        bRecalculationAtLeastForOneNeeded = True
                    End If
                Next
            End If
        End If
        '###############################

        ' If we need at least one recalculation, or all, initialize the calculation basis vector.
        If bRecalculationAtLeastForOneNeeded Then
            ' calculate the number of points to calculate (using maxE and minE set in advance, + an offset)
            NMaxPos = CInt((Math.Abs(Me.ConvolutionIntegralMaxE) + BroadeningWidth + dEInterpol) / dEInterpol)
            NMaxNeg = CInt((Math.Abs(Me.ConvolutionIntegralMinE) + BroadeningWidth + dEInterpol) / dEInterpol)
            NMaxTotal = NMaxNeg + NMaxPos

            If Me.UseCUDAVersion And Not Me.bCudaInizialized Then
                Me.InitializeCUDAOrFallBackToCPU({GetType(cFitFunction_BCSBase), GetType(cNumericFunctions)})
            End If

            ' Create the XArray to calculate the DOS for on the CPU.
            ReDim Me.PrecalcDOSX(NMaxTotal)

            '// fill the arrays on the CPU
            Parallel.For(0, NMaxNeg, Sub(j As Integer)
                                         Me.PrecalcDOSX(j) = -dEInterpol * (NMaxNeg - j)
                                     End Sub)
            Parallel.For(NMaxNeg, NMaxTotal, Sub(j As Integer)
                                                 Me.PrecalcDOSX(j) = dEInterpol * (j - NMaxNeg)
                                             End Sub)

            ' Use CUDA? then copy the CPU-Array to the GPU
            If Me.UseCUDAVersion And Me.bCudaInizialized Then
                '// Get the storage on the CPU.
                Me.dev_DOSX = CUDAGPU.Allocate(Of Double)(Me.PrecalcDOSX)
                '// copy the array to the GPU
                CUDAGPU.CopyToDevice(Me.PrecalcDOSX, Me.dev_DOSX)
            End If

        End If


        '######################################
        '
        ' Recalculation of the individual DOS
        '
        For i As Integer = 0 To Me.Precalc_RegisteredIdentifiers.Count - 1 Step 1
            tmpIdentifier = Me.Precalc_RegisteredIdentifiers(i)

            ' recalculate DOS?
            If bRecalculationForAllNeeded Or Me.Precalc_RecalculationNeeded(tmpIdentifier) Then

                '// Get arrays on the CPU **
                ReDim Me.Precalc_BCSDOSArrays(tmpIdentifier)(NMaxTotal)

                ' Save, if we add the SGP to this DOS.
                Dim AddSGPForThisDOS As Byte
                If Me.Precalc_AddSGPContribution(tmpIdentifier) Then
                    AddSGPForThisDOS = 1
                Else
                    AddSGPForThisDOS = 0
                End If

                '###############################################
                ' NOW: decide to use CPU or GPU for processing
                If Me.UseCUDAVersion And Me.bCudaInizialized Then
                    ' CUDA-Version
                    '##############

                    '// allocate the memory on the GPU
                    Dim dev_DOSY As Double() = CUDAGPU.Allocate(Of Double)(Me.Precalc_BCSDOSArrays(tmpIdentifier))

                    ' Separate version for DOS including the SGP, or not
                    'If Me.Precalc_AddSGPContribution(tmpIdentifier) Then
                    ' Include the SGP
                    '#################

                    ' get storage of the SGPs on the GPU
                    Dim dev_SGP_XCenters As Double() = CUDAGPU.Allocate(Of Double)(SGP_XCenters)
                    Dim dev_SGP_Amplitudes As Double() = CUDAGPU.Allocate(Of Double)(SGP_Amplitudes)
                    Dim dev_SGP_Widths As Double() = CUDAGPU.Allocate(Of Double)(SGP_Widths)
                    Dim dev_SGP_PosNegRatios As Double() = CUDAGPU.Allocate(Of Double)(SGP_PosNegRatios)
                    '// copy the array to the GPU
                    CUDAGPU.CopyToDevice(SGP_XCenters, dev_SGP_XCenters)
                    CUDAGPU.CopyToDevice(SGP_Amplitudes, dev_SGP_Amplitudes)
                    CUDAGPU.CopyToDevice(SGP_Widths, dev_SGP_Widths)
                    CUDAGPU.CopyToDevice(SGP_PosNegRatios, dev_SGP_PosNegRatios)

                    '// launch Current-Integration on N threads
                    CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid, cGPUComputing.GetThreadsPerBlock(NMaxTotal), "BCSDOS_SGP_CudaKernel", dev_DOSX, dev_DOSY, DeltaDictionary(tmpIdentifier), BroadeningWidth, iBroadening, ImaginaryDamping, BCSAmplitude, dev_SGP_XCenters, dev_SGP_Amplitudes, dev_SGP_Widths, dev_SGP_PosNegRatios, SGP_UseLorentz)

                    ' Free SGP storage on the GPU again
                    CUDAGPU.Free(dev_SGP_XCenters)
                    CUDAGPU.Free(dev_SGP_Amplitudes)
                    CUDAGPU.Free(dev_SGP_Widths)
                    CUDAGPU.Free(dev_SGP_PosNegRatios)
                    'Else
                    '    ' WITHOUT SGP
                    '    '#############
                    '    '// launch Current-Integration on N threads
                    '    'CUDAGPU.Launch(NMaxTotal, 1, "BCSDOSCudaKernel", dev_DOSX, dev_DOSY, DeltaDictionary(tmpIdentifier), BroadeningWidth, iBroadening, ImaginaryDamping)
                    '    CUDAGPU.Launch(cGPUComputing.CUDABlocksPerGrid, cGPUComputing.GetThreadsPerBlock(NMaxTotal), "BCSDOSCudaKernel", dev_DOSX, dev_DOSY, DeltaDictionary(tmpIdentifier), BroadeningWidth, iBroadening, ImaginaryDamping)
                    'End If

                    '// copy the array back from the GPU to the CPU
                    CUDAGPU.CopyFromDevice(dev_DOSY, Me.Precalc_BCSDOSArrays(tmpIdentifier))

                    ' free the device memory
                    CUDAGPU.Free(dev_DOSY)
                Else
                    ' CPU only
                    '##########
                    Parallel.For(0, NMaxTotal, Sub(j As Integer)
                                                   Me.Precalc_BCSDOSArrays(tmpIdentifier)(j) = BCSFuncSGP(Me.PrecalcDOSX(j), DeltaDictionary(tmpIdentifier), BroadeningWidth, iBroadening, ImaginaryDamping, BCSAmplitude, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios, SGP_UseLorentz, AddSGPForThisDOS)
                                               End Sub)
                    'Parallel.For(0, NMaxTotal, Sub(j As Integer)
                    '                               Me.Precalc_BCSDOSArrays(tmpIdentifier)(j) = BCSFunc(Me.PrecalcDOSX(j), DeltaDictionary(tmpIdentifier), BroadeningWidth, iBroadening, ImaginaryDamping)
                    '                           End Sub)
                End If
                '###############################################

                ' recalculation done!
                Me.Precalc_RecalculationNeeded(tmpIdentifier) = False
            End If

            Me.Precalc_LastDelta(tmpIdentifier) = DeltaDictionary(tmpIdentifier)
        Next
        '
        '
        '######################################

        ' Used CUDA? then free the GPU-array of the X-array
        If Me.UseCUDAVersion And Me.bCudaInizialized And bRecalculationAtLeastForOneNeeded Then
            CUDAGPU.Free(Me.dev_DOSX)
        End If

        '// ** Vermerke die nicht-individuellen Werte; **
        LastBroadeningWidth = BroadeningWidth
        LastBroadeningType = Broadening
        LastImaginaryDamping = ImaginaryDamping
        LastBCSAmplitude = BCSAmplitude
        Last_SGP_XCenters = SGP_XCenters
        Last_SGP_Amplitudes = SGP_Amplitudes
        Last_SGP_Widths = SGP_Widths
        Last_SGP_PosNegRatios = SGP_PosNegRatios
    End Sub

    ''' <summary>
    ''' Gibt einen Wert fuer die Zustandsdichte der gewünschten DOS zurueck,
    ''' wobei auf eine Tabelle zurueckgegriffen wird, die vorher mit
    ''' PrecalcBCSDOS() erzeugt werden muss  
    ''' Dient zur massiven Beschleunigung der Berechnung
    ''' 
    ''' DOES NOT CHECK FOR EXISTING IDENTIFIER!!! WOULD WASTE TOO MUCH TIME!
    ''' </summary>
    Public Function BCSFuncPrecalc(ByVal E As Double,
                                   ByVal DOSIdentifier As Integer) As Double


        Dim DOS As Double() = Me.Precalc_BCSDOSArrays(DOSIdentifier)

        '  // ** Berechne Logische Position; **
        Dim n As Integer = Convert.ToInt32(E / dEInterpol)
        Dim d As Double = (E - n * dEInterpol) / dEInterpol
        If E < 0 Then
            n = NMaxNeg - n
        Else
            n += NMaxNeg
        End If

        '  // ** Fallunterscheidung; **
        If n >= DOS.Length - 1 Then
            Return 1
        Else
            '// ** Fuehre Interpolation durch; **
            'Return d * (DOS(n + 1) - DOS(n)) + DOS(n)
            Return (DOS(n + 1) - DOS(n)) + DOS(n)
        End If
    End Function
#End Region

#Region "FitFunction-Implementation"

    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' </summary>
    ''' <param name="x">x-value at which the fit function should be evaluated.</param>
    Public Overrides Function GetY(ByRef x As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double
        ' Create the delta-dictionary
        Dim DeltaDictionary As New Dictionary(Of Integer, Double)
        For i As Integer = 0 To Me.Precalc_RegisteredIdentifiers.Count - 1 Step 1
            DeltaDictionary.Add(Me.Precalc_RegisteredIdentifiers(i), sFitParameter.GetValueForIdentifier(Me.Precalc_RegisteredIdentifiers(i), Identifiers, Values))
        Next

        Dim SGP_Identifiers(Me.SubGapPeaks.Count - 1) As Integer
        Dim SGP_XCenters(Me.SubGapPeaks.Count - 1) As Double
        Dim SGP_Amplitudes(Me.SubGapPeaks.Count - 1) As Double
        Dim SGP_Widths(Me.SubGapPeaks.Count - 1) As Double
        Dim SGP_PosNegRatios(Me.SubGapPeaks.Count - 1) As Double

        Me.GetSGPArraysFromFitParameterObjects(Identifiers, Values, SGP_Identifiers, SGP_XCenters, SGP_Amplitudes, SGP_Widths, SGP_PosNegRatios)

        ' Check, if BCSDOS needs to be recalculated - speeds up everything
        Me.PrecalcBCSDOS(DeltaDictionary,
                         sFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSBase.BroadeningWidth, Identifiers, Values),
                         Me.BroadeningType,
                         sFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSBase.ImaginaryDamping, Identifiers, Values),
                         sFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSBase.BCSAmplitude, Identifiers, Values),
                         SGP_Identifiers,
                         SGP_XCenters,
                         SGP_Amplitudes,
                         SGP_Widths,
                         SGP_PosNegRatios)


        ' Calculate the function value.
        'Return Me.SampleDOS(x, FitParameters)
        Return Me.FitFunction(x,
                              Identifiers, Values)
    End Function

    ''' <summary>
    ''' Returns the maximum energy range for calculation defined by the
    ''' gap-parameters registered.
    ''' </summary>
    Public Overrides Property MaxIntegrationERange As Double

    ''' <summary>
    ''' Returns the maximum energy range for calculation defined by the
    ''' gap-parameters registered.
    ''' </summary>
    Public Sub CalculateMaxIntegrationERange(ByRef DeltaDic As Dictionary(Of Integer, Double))
        Me.MaxIntegrationERange = DeltaDic.Values.Max * 2.2
    End Sub
#End Region

#Region "Step Gap Function - CUDA compatible"
    ''' <summary>
    ''' Function that creates the step gap, instead of a BCS
    ''' Uses a Sigmoidal Boltzmann distribution.
    ''' </summary>
    <Cudafy>
    Public Shared Function StepGapFunction(x As Double,
                                           Width As Double,
                                           Delta As Double) As Double
        Return 1 + cNumericFunctions.SigmoidalBoltzmann(x, Delta, 0, 1, Width) - cNumericFunctions.SigmoidalBoltzmann(x, -Delta, 0, 1, Width)
    End Function
#End Region

#Region "Sub-Gap Peak Storage and Contribution Compatible - For CUDA, use the array version of the SGP"
    ''' <summary>
    ''' Set this variable to determine the type of sub-gap-peaks used in the fit.
    ''' </summary>
    Public Property SubGapPeakType As iFitFunction_SubGapPeaks.SubGapPeakTypes = iFitFunction_SubGapPeaks.SubGapPeakTypes.Lorentz Implements iFitFunction_SubGapPeaks.SubGapPeakType

    ''' <summary>
    ''' List to save all sub-gap-peaks.
    ''' 
    ''' -> changed to dictionary to keep existing SGPs at
    ''' the same position!!!
    ''' </summary>
    Private Property SubGapPeaks As New Dictionary(Of Integer, iFitFunction_SubGapPeaks.SubGapPeak) Implements iFitFunction_SubGapPeaks.SubGapPeaks

    ''' <summary>
    ''' Returns a free index in the Dictionary.
    ''' </summary>
    Private Function GetFreeSGPIndex() As Integer
        For i As Integer = 0 To 10000 Step 1
            If Not SubGapPeaks.ContainsKey(i) Then
                Return i
            End If
        Next
        Return -1
    End Function

    ''' <summary>
    ''' Adds a new sub-gap peak to the fit-function.
    ''' Returns the index of the internal list, at which the peak got added.
    ''' </summary>
    Public Function AddSubGapPeak(ByRef SubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak) As Integer Implements iFitFunction_SubGapPeaks.AddSubGapPeak
        Dim i As Integer = Me.GetFreeSGPIndex
        Me.SubGapPeaks.Add(i, SubGapPeak)
        SubGapPeak.Identifier = i
        Return i
    End Function

    ''' <summary>
    ''' Adds a new sub-gap peak to the fit-function.
    ''' Returns the index of the internal list, at which the peak got added.
    ''' </summary>
    Public Function AddSubGapPeak(ByRef SubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak, ByVal Identifier As Integer) As Integer Implements iFitFunction_SubGapPeaks.AddSubGapPeak
        If Me.SubGapPeaks.ContainsKey(Identifier) Then
            Me.SubGapPeaks(Identifier) = SubGapPeak
        Else
            Me.SubGapPeaks.Add(Identifier, SubGapPeak)
        End If
        SubGapPeak.Identifier = Identifier
        Return Identifier
    End Function

    ''' <summary>
    ''' Removes the SGP from the list.
    ''' False, if the index was not found in the dictionary.
    ''' </summary>
    Public Function RemoveSubGapPeak(ByVal SubGapPeakIndex As Integer) As Boolean Implements iFitFunction_SubGapPeaks.RemoveSubGapPeak
        With Me.SubGapPeaks
            If .ContainsKey(SubGapPeakIndex) Then
                .Remove(SubGapPeakIndex)
                Return True
            Else
                Return False
            End If
        End With
    End Function

    ''' <summary>
    ''' Clears the list of all sub-gap-peaks.
    ''' </summary>
    Public Sub ClearSubGapPeaks() Implements iFitFunction_SubGapPeaks.ClearSubGapPeaks
        Me.SubGapPeaks.Clear()
    End Sub

    ''' <summary>
    ''' Currently registered SubGapPeaks.
    ''' </summary>
    Public ReadOnly Property RegisteredSubGapPeaks As ReadOnlyDictionary(Of Integer, iFitFunction_SubGapPeaks.SubGapPeak) Implements iFitFunction_SubGapPeaks.RegisteredSubGapPeaks
        Get
            Return New ReadOnlyDictionary(Of Integer, iFitFunction_SubGapPeaks.SubGapPeak)(Me.SubGapPeaks)
        End Get
    End Property

    ''' <summary>
    ''' Delegate, that describes a sub-gap-peak-function value.
    ''' Either a Lorentzian or gaussian!
    ''' </summary>
    Private Delegate Function SubGapPeakValue(ByVal x As Double,
                                              ByVal XCenter As Double,
                                              ByVal Amplitude As Double,
                                              ByVal Width As Double,
                                              ByVal PosNegHeightRatio As Double) As Double

    ''' <summary>
    ''' Represents a gaussian sub-gap-peak to be added to the sample-DOS.
    ''' </summary>
    <Cudafy>
    Public Shared Function SubGapPeakValue_Gauss(ByVal x As Double,
                                                 ByVal XCenter As Double,
                                                 ByVal Amplitude As Double,
                                                 ByVal Width As Double,
                                                 ByVal PosNegHeightRatio As Double) As Double
        Return Amplitude * (cNumericFunctions.GaussPeak_Amplitude_Normalized(x, XCenter, Width) _
                            + PosNegHeightRatio * cNumericFunctions.GaussPeak_Amplitude_Normalized(x, -XCenter, Width))
    End Function

    ''' <summary>
    ''' Represents a lorentzian sub-gap-peak to be added to the sample-DOS.
    ''' </summary>
    <Cudafy>
    Public Shared Function SubGapPeakValue_Lorentz(ByVal x As Double,
                                                   ByVal XCenter As Double,
                                                   ByVal Amplitude As Double,
                                                   ByVal Width As Double,
                                                   ByVal PosNegHeightRatio As Double) As Double
        Return Amplitude * (cNumericFunctions.LorentzPeak_Normalized(x, XCenter, Width) _
                            + PosNegHeightRatio * cNumericFunctions.LorentzPeak_Normalized(x, -XCenter, Width))
    End Function

    ''' <summary>
    ''' Fills given arrays with the parameters of the SGPs
    ''' saved in the fit-function. Needed for CUDAfying of the BCSDOS.
    ''' </summary>
    Protected Sub GetSGPArraysFromFitParameterObjects(ByRef Identifiers As Integer(), ByRef Values As Double(),
                                                      ByRef SGP_Identifiers As Integer(),
                                                      ByRef SGP_XCenters As Double(),
                                                      ByRef SGP_Amplitudes As Double(),
                                                      ByRef SGP_Widths As Double(),
                                                      ByRef SGP_PosNegRatios As Double())
        If SGP_Identifiers Is Nothing Then
            SGP_Identifiers = New Integer(Me.SubGapPeaks.Count - 1) {}
        Else
            If SGP_Identifiers.Length <> Me.SubGapPeaks.Count Then ReDim SGP_Identifiers(Me.SubGapPeaks.Count - 1)
        End If
        If SGP_XCenters Is Nothing Then
            SGP_XCenters = New Double(Me.SubGapPeaks.Count - 1) {}
        Else
            If SGP_XCenters.Length <> Me.SubGapPeaks.Count Then ReDim SGP_XCenters(Me.SubGapPeaks.Count - 1)
        End If
        If SGP_Amplitudes Is Nothing Then
            SGP_Amplitudes = New Double(Me.SubGapPeaks.Count - 1) {}
        Else
            If SGP_Amplitudes.Length <> Me.SubGapPeaks.Count Then ReDim SGP_Amplitudes(Me.SubGapPeaks.Count - 1)
        End If
        If SGP_Widths Is Nothing Then
            SGP_Widths = New Double(Me.SubGapPeaks.Count - 1) {}
        Else
            If SGP_Widths.Length <> Me.SubGapPeaks.Count Then ReDim SGP_Widths(Me.SubGapPeaks.Count - 1)
        End If
        If SGP_PosNegRatios Is Nothing Then
            SGP_PosNegRatios = New Double(Me.SubGapPeaks.Count - 1) {}
        Else
            If SGP_PosNegRatios.Length <> Me.SubGapPeaks.Count Then ReDim SGP_PosNegRatios(Me.SubGapPeaks.Count - 1)
        End If

        ' Sum up all contributions
        For i As Integer = 0 To Identifiers.Count - 1 Step 1
            ' Go through all Fit-Parameters and extract those from certain sub-gap-peaks.
            ' Ignore original fit-parameters and inelastic channels
            If Identifiers(i) < 10000 Or Identifiers(i) > 100000 Then Continue For

            ' Get the SubGapPeak-Index by dividing the Identifier by 1000 and cut the rest.
            Dim SubGapPeakCount As Integer = CInt(Identifiers(i) / 10000)
            Dim SubGapPeakIndex As Integer = SubGapPeakCount - 1

            ' Set the parameter-value only
            SGP_Identifiers(SubGapPeakIndex) = SubGapPeakIndex
            Select Case Identifiers(i)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.Amplitude
                    ' Amplitude
                    SGP_Amplitudes(SubGapPeakIndex) = Values(i)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.Width
                    ' Width
                    SGP_Widths(SubGapPeakIndex) = Values(i)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.PosNegRatio
                    ' PosNegRatio
                    SGP_PosNegRatios(SubGapPeakIndex) = Values(i)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.XCenter
                    ' XCenter
                    SGP_XCenters(SubGapPeakIndex) = Values(i)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Compares two SGP-arrays
    ''' If they differ in one parameter: false
    ''' </summary>
    Protected Function AreSGPArraysTheSame(ByRef SGP_1_XCenters As Double(),
                                           ByRef SGP_1_Amplitudes As Double(),
                                           ByRef SGP_1_Widths As Double(),
                                           ByRef SGP_1_PosNegRatios As Double(),
                                           ByRef SGP_2_XCenters As Double(),
                                           ByRef SGP_2_Amplitudes As Double(),
                                           ByRef SGP_2_Widths As Double(),
                                           ByRef SGP_2_PosNegRatios As Double()) As Boolean
        If SGP_1_Widths.Length <> SGP_2_Widths.Length Then Return False
        If SGP_1_Amplitudes.Length <> SGP_2_Amplitudes.Length Then Return False
        If SGP_1_XCenters.Length <> SGP_2_XCenters.Length Then Return False
        If SGP_1_PosNegRatios.Length <> SGP_2_PosNegRatios.Length Then Return False
        For i As Integer = 0 To SGP_1_Amplitudes.Length - 1 Step 1
            If SGP_1_Amplitudes(i) <> SGP_2_Amplitudes(i) Then Return False
            If SGP_1_Widths(i) <> SGP_2_Widths(i) Then Return False
            If SGP_1_PosNegRatios(i) <> SGP_2_PosNegRatios(i) Then Return False
            If SGP_1_XCenters(i) <> SGP_2_XCenters(i) Then Return False
        Next
        Return True
    End Function

    ''' <summary>
    ''' Writes the SGPs given by the SGP-arrays back to the saved FitParameter-Objects.
    ''' 
    ''' DOES NOT CHECK THE LENGTH OF THE IN-ARRAYS!!!
    ''' </summary>
    Protected Sub GetSGPDictionaryBackFromArrays(ByRef SGP_Identifiers As Integer(),
                                                 ByRef SGP_XCenters As Double(),
                                                 ByRef SGP_Amplitudes As Double(),
                                                 ByRef SGP_Widths As Double(),
                                                 ByRef SGP_PosNegRatios As Double())
        For i As Integer = 0 To SGP_Identifiers.Length - 1 Step 1
            With Me.SubGapPeaks(SGP_Identifiers(i))
                .XCenter.ChangeValue(SGP_XCenters(i), False)
                .Amplitude.ChangeValue(SGP_Amplitudes(i), False)
                .Width.ChangeValue(SGP_Widths(i), False)
                .PosNegRatio.ChangeValue(SGP_PosNegRatios(i), False)
            End With
        Next
    End Sub

    ''' <summary>
    ''' Gets the integrand contribution to the DOS created by all sub-gap peaks registered!
    ''' CUDA compatible, (uses the arrays created by <code>GetSGPArraysFromObjects</code>)
    ''' </summary>
    <Cudafy>
    Protected Shared Function GetSubGapPeakContribution(ByVal x As Double,
                                                        ByVal SGP_XCenters As Double(),
                                                        ByVal SGP_Amplitudes As Double(),
                                                        ByVal SGP_Widths As Double(),
                                                        ByVal SGP_PosNegRatios As Double(),
                                                        ByVal UseLorentz As Integer) As Double

        Dim SubGapPeakContribution As Double = 0

        ' Get the values by summing up all SGP contributions
        ' uses either Lorentz or Gauss
        If UseLorentz = 1 Then
            ' LORENTZ
            '#########
            For i As Integer = 0 To SGP_XCenters.Length - 1 Step 1
                SubGapPeakContribution += SubGapPeakValue_Lorentz(x,
                                                                  SGP_XCenters(i),
                                                                  SGP_Amplitudes(i),
                                                                  SGP_Widths(i),
                                                                  SGP_PosNegRatios(i))
            Next
        Else
            ' GAUSS
            '#######
            For i As Integer = 0 To SGP_XCenters.Length - 1 Step 1
                SubGapPeakContribution += SubGapPeakValue_Gauss(x,
                                                                SGP_XCenters(i),
                                                                SGP_Amplitudes(i),
                                                                SGP_Widths(i),
                                                                SGP_PosNegRatios(i))
            Next
        End If
        Return SubGapPeakContribution
    End Function

    ''' <summary>
    ''' Gets the integrand contribution to the DOS created by all sub-gap peaks registered!
    ''' </summary>
    Protected Function GetSubGapPeakContribution(ByVal x As Double) As Double
        Dim SubGapPeakFunction As SubGapPeakValue

        ' Determine which function to use
        If Me.SubGapPeakType = iFitFunction_SubGapPeaks.SubGapPeakTypes.Gauss Then
            SubGapPeakFunction = AddressOf SubGapPeakValue_Gauss
        Else
            SubGapPeakFunction = AddressOf SubGapPeakValue_Lorentz
        End If

        Dim SubGapPeakContribution As Double = 0

        ' Sum up all contributions
        For Each SubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak In Me.SubGapPeaks.Values
            With SubGapPeak
                SubGapPeakContribution += SubGapPeakFunction(x,
                                                             .XCenter.Value,
                                                             .Amplitude.Value,
                                                             .Width.Value,
                                                             .PosNegRatio.Value)
            End With
        Next

        Return SubGapPeakContribution
    End Function

#End Region

#Region "Register SubGapPeaks with the base fit-functions"
    ''' <summary>
    ''' Method that sets all Initial FitParameters from the individual FitFunctions
    ''' in this multiple-function-container
    ''' </summary>
    Public Overrides Sub ReInitializeFitParameters() Implements iFitFunction_SubGapPeaks.ReInitializeFitParameters
        ' Remove old sub-gap peak fit-parameters from the Fit-Parameter-Array
        Dim FitParameterIdentifiersToRemove As New List(Of Integer)
        For Each FitParameterIdentifier As Integer In Me.FitParameters.Keys
            If FitParameterIdentifier >= 10000 And FitParameterIdentifier <= 100000 Then
                FitParameterIdentifiersToRemove.Add(FitParameterIdentifier)
            End If
        Next
        For i As Integer = 0 To FitParameterIdentifiersToRemove.Count - 1 Step 1
            Me.FitParameters.Remove(FitParameterIdentifiersToRemove(i))
        Next

        ' Modify the Fit-Parameter-Array and add all Sub-Gap-Peaks
        For Each SGPKV As KeyValuePair(Of Integer, iFitFunction_SubGapPeaks.SubGapPeak) In Me.SubGapPeaks
            ' add Fit-Parameters to global array
            With Me.FitParameters
                .Add(((SGPKV.Key + 1) * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.Amplitude, SGPKV.Value.Amplitude)
                .Add(((SGPKV.Key + 1) * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.Width, SGPKV.Value.Width)
                .Add(((SGPKV.Key + 1) * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.PosNegRatio, SGPKV.Value.PosNegRatio)
                .Add(((SGPKV.Key + 1) * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.XCenter, SGPKV.Value.XCenter)
            End With
        Next

        ' Call the reinitialization for the IEC in the base-class
        MyBase.ReInitializeFitParameters()
    End Sub

    ''' <summary>
    ''' This function sets all parameters of the Sub-Gap-Peaks from the Fit-Parameter-Array
    ''' given by the fit-procedure.
    ''' </summary>
    Public Overrides Sub SetAdditionalProperties(ByRef Identifiers As Integer(), ByRef Values As Double())
        ' Go through all Fit-Parameters and extract those from certain sub-gap-peaks.
        For i As Integer = 0 To Identifiers.Length - 1 Step 1
            ' Ignore original fit-parameters and inelastic channels
            If Identifiers(i) < 10000 Or Identifiers(i) > 100000 Then Continue For

            ' Get the SubGapPeak-Index by dividing the Identifier by 1000 and cut the rest.
            Dim SubGapPeakCount As Integer = CInt(Identifiers(i) / 10000)
            Dim SubGapPeakIndex As Integer = SubGapPeakCount - 1

            ' Set the parameter-value only
            Select Case Identifiers(i)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.Amplitude
                    ' Amplitude
                    Me.SubGapPeaks(SubGapPeakIndex).Amplitude.ChangeValue(Values(i), False)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.Width
                    ' Width
                    Me.SubGapPeaks(SubGapPeakIndex).Width.ChangeValue(Values(i), False)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.PosNegRatio
                    ' PosNegRatio
                    Me.SubGapPeaks(SubGapPeakIndex).PosNegRatio.ChangeValue(Values(i), False)
                Case (SubGapPeakCount * 10000) + iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.XCenter
                    ' XCenter
                    Me.SubGapPeaks(SubGapPeakIndex).XCenter.ChangeValue(Values(i), False)
            End Select
        Next

        ' Call the initialization for the IEC in the base-class
        MyBase.SetAdditionalProperties(Identifiers, Values)
    End Sub
#End Region

End Class
