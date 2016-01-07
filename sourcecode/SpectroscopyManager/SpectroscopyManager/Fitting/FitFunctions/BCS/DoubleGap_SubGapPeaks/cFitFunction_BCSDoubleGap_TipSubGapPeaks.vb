Imports System.Threading.Tasks
Imports System.Threading
Imports System.Collections.Concurrent
Imports Cudafy

Public Class cFitFunction_BCSDoubleGap_TipSubGapPeaks
    Inherits cFitFunction_BCSBase

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifierBCSDoubleGap
        Delta_Tip
        Delta_Sample1
        Delta_Sample2
        RatioSample1ToSample2
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(FitParameterIdentifierBCSDoubleGap.Delta_Tip, New cFitParameter("TipGap", 0.00134, True, My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Parameter_Delta_Tip))
        Me.FitParameters.Add(FitParameterIdentifierBCSDoubleGap.Delta_Sample1, New cFitParameter("SampleGap1", 0.00142, False, My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Parameter_Delta_Sample1))
        Me.FitParameters.Add(FitParameterIdentifierBCSDoubleGap.Delta_Sample2, New cFitParameter("SampleGap2", 0.00132, False, My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Parameter_Delta_Sample2))
        Me.FitParameters.Add(FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2, New cFitParameter("GapRatio", 0.1, False, My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Parameter_RatioSample1ToSample2))
        MyBase.InitializeFitParameters()
    End Sub
#End Region

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        ' Register the precalculatable DOS functions.
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSDoubleGap.Delta_Tip)
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSDoubleGap.Delta_Sample1, True)
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSDoubleGap.Delta_Sample2)
    End Sub

    ''' <summary>
    ''' Returns the CUDA-Classes to compile
    ''' </summary>
    Public Overrides Function GetCudaCompileClasses() As List(Of Type)
        Dim ReturnList As New List(Of Type)
        ReturnList.Add(GetType(cFitFunction_BCSBase))
        ReturnList.Add(GetType(cFitFunction_BCSDoubleGap_TipSubGapPeaks))
        Return ReturnList
    End Function

    ''' <summary>
    ''' Returns the Sample-DOS.
    ''' </summary>
    Public Overrides Function SampleDOS(ByRef E As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double
        Return SampleDOSCUDA(E, Identifiers, Values)
    End Function

    ''' <summary>
    ''' Returns the Sample-DOS.
    ''' </summary>
    <Cudafy>
    Public Shared Shadows Function SampleDOSCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
        Return cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSBase.BCSAmplitude, Identifiers, Values) _
               * (cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2, Identifiers, Values) _
               * BCSFunc(E,
                         cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSDoubleGap.Delta_Sample1, Identifiers, Values),
                         0,
                         0,
                         cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSBase.ImaginaryDamping, Identifiers, Values)) _
               + BCSFunc(E,
                         cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSDoubleGap.Delta_Sample2, Identifiers, Values),
                         0,
                         0,
                         cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSBase.ImaginaryDamping, Identifiers, Values))) _
               + GetSubGapPeakContribution(E, Identifiers, Values, 1, cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip) ' TIP is correct HERE!!!

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
    <Cudafy>
    Public Shared Shadows Function TipDOSCUDA(ByVal E As Double, ByVal Identifiers As Integer(), ByVal Values As Double()) As Double
        Return BCSFunc(E,
                        cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSDoubleGap.Delta_Tip, Identifiers, Values),
                        0,
                        0,
                        cFitParameter.GetValueForIdentifier(FitParameterIdentifierBCSBase.ImaginaryDamping, Identifiers, Values)) _
               + GetSubGapPeakContribution(E, Identifiers, Values, 1, cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample)
    End Function

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Name_TipSGP
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Description_TipSGP
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_BCSDoubleGap_SubGapPeaks.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_BCSDoubleGap_TipSubGapPeaks
        End Get
    End Property
#End Region

End Class
