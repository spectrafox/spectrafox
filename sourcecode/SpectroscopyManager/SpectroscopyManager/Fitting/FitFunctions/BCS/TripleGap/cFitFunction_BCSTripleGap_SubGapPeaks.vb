Imports System.Threading.Tasks
Imports System.Threading
Imports System.Collections.Concurrent
Imports Cudafy

Public Class cFitFunction_BCSTripleGap_SubGapPeaks
    Inherits cFitFunction_BCSBase

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifierBCSTripleGap
        Delta_Tip
        Delta_Sample1
        Delta_Sample2
        Delta_Sample3
        RatioSample1ToSample2
        RatioSample1ToSample3
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSTripleGap.Delta_Tip.ToString, 0.00136, True, My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Parameter_Delta_Tip))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSTripleGap.Delta_Sample1.ToString, 0.00142, False, My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Parameter_Delta_Sample1))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSTripleGap.Delta_Sample2.ToString, 0.00128, False, My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Parameter_Delta_Sample2))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSTripleGap.RatioSample1ToSample2.ToString, 0.1, False, My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Parameter_RatioSample1ToSample2))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSTripleGap.Delta_Sample3.ToString, 0.00112, False, My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Parameter_Delta_Sample3))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSTripleGap.RatioSample1ToSample3.ToString, 0.1, False, My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Parameter_RatioSample1ToSample3))
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
        ReturnList.Add(GetType(cFitFunction_BCSTripleGap_SubGapPeaks))
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
        Return InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.BCSAmplitude.ToString).Value _
               * (InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.RatioSample1ToSample2.ToString).Value _
               * BCSFunc(E,
                         InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.Delta_Sample1.ToString).Value,
                         0,
                         0,
                         InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value) _
               + BCSFunc(E,
                         InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.Delta_Sample2.ToString).Value,
                         0,
                         0,
                         InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value) _
               + InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.RatioSample1ToSample3.ToString).Value _
               * BCSFunc(E,
                         InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.Delta_Sample3.ToString).Value,
                         0,
                         0,
                         InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value)) _
               + GetSubGapPeakContribution(E, InputParameters, 1, cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample)
    End Function

    ''' <summary>
    ''' Returns the TIP-DOS
    ''' </summary>
    Public Function TipDOSCUDA(ByVal E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return BCSFunc(E,
                        InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.Delta_Tip.ToString).Value,
                        0,
                        0,
                        InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value) _
               + GetSubGapPeakContribution(E, InputParameters, 1, cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip)
    End Function

    ''' <summary>
    ''' Separated Sample-DOS for data generation functions
    ''' </summary>
    Public Function SampleDOSSeparated1(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.RatioSample1ToSample2.ToString).Value _
               * BCSFunc(E, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.Delta_Sample1.ToString).Value,
                         0, 0, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value)
    End Function

    ''' <summary>
    ''' Separated Sample-DOS for data generation functions
    ''' </summary>
    Public Function SampleDOSSeparated2(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return BCSFunc(E, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.Delta_Sample2.ToString).Value,
                         0, 0, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value)
    End Function

    ''' <summary>
    ''' Separated Sample-DOS for data generation functions
    ''' </summary>
    Public Function SampleDOSSeparated3(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return BCSFunc(E, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSTripleGap.Delta_Sample3.ToString).Value,
                         0, 0, InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value)
    End Function

    ''' <summary>
    ''' Separated SubGapPeaks for data generation functions
    ''' </summary>
    Public Function SubGapPeaksSeparatedTip(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return GetSubGapPeakContribution(E, InputParameters, 1, cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip)
    End Function

    ''' <summary>
    ''' Separated SubGapPeaks for data generation functions
    ''' </summary>
    Public Function SubGapPeaksSeparatedSample(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return GetSubGapPeakContribution(E, InputParameters, 1, cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample)
    End Function


    ''' <summary>
    ''' Register the two tip and sample DOS functions separately DOS-generation functions.
    ''' </summary>
    Public Overrides Sub RegisterDataGenerationFunction()
        MyBase.RegisterDataGenerationFunction()
        Me.AdditionalDataGenerationFunctions.Add(My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.GeneratedDataFunctionName_SampleDOS1, AddressOf SampleDOSSeparated1)
        Me.AdditionalDataGenerationFunctions.Add(My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.GeneratedDataFunctionName_SampleDOS2, AddressOf SampleDOSSeparated2)
        Me.AdditionalDataGenerationFunctions.Add(My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.GeneratedDataFunctionName_SampleDOS3, AddressOf SampleDOSSeparated3)
        Me.AdditionalDataGenerationFunctions.Add(My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.GeneratedDataFunctionName_SubGapPeaksTip, AddressOf SubGapPeaksSeparatedTip)
        Me.AdditionalDataGenerationFunctions.Add(My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.GeneratedDataFunctionName_SubGapPeaksSample, AddressOf SubGapPeaksSeparatedSample)
    End Sub

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Name_SampleSGP
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Description_SampleSGP
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_BCSTripleGap_SubGapPeaks.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_BCSTripleGap_SubGapPeaks
        End Get
    End Property
#End Region

End Class
