Imports System.Threading.Tasks
Imports System.Threading
Imports System.Collections.Concurrent
Imports Cudafy

Public Class cFitFunction_BCSSingleGap
    Inherits cFitFunction_BCSBase

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifierBCSSingleGap
        Delta_Tip
        Delta_Sample
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSSingleGap.Delta_Tip.ToString, 0.00134, True, My.Resources.rFitFunction_BCSSingleGap.Parameter_Delta_Tip))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSSingleGap.Delta_Sample.ToString, 0.00142, False, My.Resources.rFitFunction_BCSSingleGap.Parameter_Delta_Sample))
        MyBase.InitializeFitParameters()
    End Sub
#End Region

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        ' Register the precalculatable DOS functions.
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSSingleGap.Delta_Tip)
        'MyBase.RegisterPrecalcDOS(FitParameterIdentifierBCSSingleGap.Delta_Sample)
    End Sub

    ''' <summary>
    ''' Returns the CUDA-Classes to compile
    ''' </summary>
    Public Overrides Function GetCudaCompileClasses() As List(Of Type)
        Dim ReturnList As New List(Of Type)
        ReturnList.Add(GetType(cFitFunction_BCSBase))
        ReturnList.Add(GetType(cFitFunction_BCSSingleGap))
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
    Public Function SampleDOSCUDA(ByVal E As Double, ByVal InputParameters As cFitParameterGroupGroup) As Double
        Return InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.BCSAmplitude.ToString).Value * _
               BCSFunc(E,
                       InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSSingleGap.Delta_Sample.ToString).Value,
                       0,
                       0,
                       InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value)
    End Function

    ''' <summary>
    ''' Returns the TIP-DOS
    ''' </summary>
    Public Function TipDOSCUDA(ByVal E As Double, ByVal InputParameters As cFitParameterGroupGroup) As Double
        Return BCSFunc(E,
                       InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSSingleGap.Delta_Tip.ToString).Value,
                       0,
                       0,
                       InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString).Value)
    End Function

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_BCSSingleGap.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_BCSSingleGap.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_BCSSingleGap.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_BCSSingleGap.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_BCSSingleGap
        End Get
    End Property
#End Region

End Class
