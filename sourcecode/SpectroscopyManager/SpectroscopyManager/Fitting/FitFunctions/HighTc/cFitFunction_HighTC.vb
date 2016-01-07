Imports Cudafy

Public Class cFitFunction_HighTC
    Inherits cFitFunction_MetalTipSampleConvolutionBase

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifierHighTC
        ImaginaryDamping = 21
        BCSAmplitude = 22
        Delta_Sample = 23
        LinearFactor = 24
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(FitParameterIdentifierHighTC.Delta_Sample, New cFitParameter("SampleGap", 0.045, False, My.Resources.rFitFunction_HighTc.SampleGap))
        Me.FitParameters.Add(FitParameterIdentifierHighTC.ImaginaryDamping, New cFitParameter("ImaginaryDamping", 0.000015, False, My.Resources.rFitFunction_BCSBase.Parameter_ImaginaryDamping))
        Me.FitParameters.Add(FitParameterIdentifierHighTC.BCSAmplitude, New cFitParameter("BCSAmplitude", 1, False, My.Resources.rFitFunction_BCSBase.Parameter_BCSAmplitude))
        Me.FitParameters.Add(FitParameterIdentifierHighTC.LinearFactor, New cFitParameter("LinearFactor", 0, False, My.Resources.rFitFunction_HighTc.LinearFactor))
        MyBase.InitializeFitParameters()
    End Sub
#End Region

    ''' <summary>
    ''' Theta-Integration summation
    ''' </summary>
    Private Const nDOS As Integer = 101

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        ' Register the precalculatable DOS functions.
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSSingleGap.Delta_Tip)
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSSingleGap.Delta_Sample)

        MyBase.ConvolutionIntegrationStepSize = 0.0003090169943749474241023D
        MyBase.CalculateForBiasRangeLowerE = -0.15
        MyBase.CalculateForBiasRangeUpperE = 0.15
        MyBase.ConvolutionIntegralE_NEG = -0.15
        MyBase.ConvolutionIntegralE_POS = 0.15
        cFitFunction_MetalTipSampleConvolutionBase.dE_BroadeningStepWidth = 0.0005
        MyBase.dEInterpolation_dIdVPrecalculation = 0.0005

        Me._FunctionImplementsCUDAVersion = True
    End Sub

    ''' <summary>
    ''' Returns the CUDA-Classes to compile
    ''' </summary>
    Public Overrides Function GetCudaCompileClasses() As List(Of Type)
        Dim ReturnList As New List(Of Type)
        ReturnList.Add(GetType(cFitFunction_HighTC))
        Return ReturnList
    End Function

    ''' <summary>
    ''' Returns the Sample-DOS.
    ''' </summary>
    Public Overrides Function SampleDOS(ByRef E As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double
        Return SampleDOSCUDA(E, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Returns the TIP-DOS
    ''' </summary>
    Public Overrides Function TipDOS(ByRef E As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double
        Return TipDOSCUDA(E, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Returns the TIP-DOS
    ''' </summary>
    Public Overrides Function TipDOSDerivative(ByRef E As Double, ByRef Identifiers() As Integer, ByRef Values() As Double) As Double
        Return TipDOSDerivativeCUDA(E, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Returns the Sample-DOS.
    ''' </summary>
    <Cudafy>
    Public Shared Shadows Function SampleDOSCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double

        Dim epsilon As Integer = 1
        Dim result As Double = 0
        Do While epsilon <= nDOS
            result += EnergyDependentDOSIntegrand(E,
                                                  cFitParameter.GetValueForIdentifier(FitParameterIdentifierHighTC.ImaginaryDamping, Identifiers, Values),
                                                  cFitParameter.GetValueForIdentifier(FitParameterIdentifierHighTC.Delta_Sample, Identifiers, Values),
                                                  epsilon * 2 * Math.PI / nDOS)
            epsilon += 1
        Loop
        result *= (2 * Math.PI / nDOS)
        result *= cFitParameter.GetValueForIdentifier(FitParameterIdentifierHighTC.BCSAmplitude, Identifiers, Values)

        ' changed on 04/14/2014: added linear factor
        result += cFitParameter.GetValueForIdentifier(FitParameterIdentifierHighTC.LinearFactor, Identifiers, Values) * E

        Return result
    End Function

    ''' <summary>
    ''' Returns the derivative TIP-DOS (1 since we assume a metallic tip.
    ''' </summary>
    <Cudafy>
    Public Shared Shadows Function TipDOSDerivativeCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
        Return 0
    End Function

    ''' <summary>
    ''' Returns the TIP-DOS (1 since we assume a metallic tip.
    ''' </summary>
    <Cudafy>
    Public Shared Shadows Function TipDOSCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
        Return 1
    End Function

    ''' <summary>
    ''' Density of States with energy dependent
    ''' imaginary damping factor and a gap * cos(2 * theta).
    ''' </summary>
    <Cudafy>
    Public Shared Function EnergyDependentDOSIntegrand(E As Double,
                                                       Alpha As Double,
                                                       Delta As Double,
                                                       Theta As Double) As Double
        Return BCSDOSExpanded(E, Delta * Math.Cos(2 * Theta), Alpha * E)
    End Function

    ''' <summary>
    ''' Actual BCS-DOS function, that may have a small imaginary damping
    ''' factor to smear out the peak structure by life-time effects.
    ''' Returns similar results, as a Lorentzian convolution! Due to
    ''' calculation time, you should prefer the imaginary damping factor.
    '''
    ''' Function got expanded by Mathematica to avoid the need of complex numbers.
    ''' </summary>
    <Cudafy>
    Public Shared Function BCSDOSExpanded(E As Double,
                                          Delta As Double,
                                          ImaginaryDamping As Double) As Double
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

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_HighTc.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_HighTc.FunctionName
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_HighTc.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_HighTc.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_HighTC
        End Get
    End Property
#End Region

End Class
