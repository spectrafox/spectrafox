Public Class cFitFunction_Fano
    Inherits cFitFunction

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifier
        Y0
        Amplitude
        XCenter
        ResonantWidth
        FanoFactor
        LinearBackground
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Y0.ToString, 0, False, My.Resources.rFitFunction_Fano.Parameter_Y0))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.XCenter.ToString, 0, False, My.Resources.rFitFunction_Fano.Parameter_XCenter))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Amplitude.ToString, 1, False, My.Resources.rFitFunction_Fano.Parameter_Amplitude))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.ResonantWidth.ToString, 1, False, My.Resources.rFitFunction_Fano.Parameter_ResonantWidth))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.FanoFactor.ToString, 0, False, My.Resources.rFitFunction_Fano.Parameter_FanoFactor))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.LinearBackground.ToString, 0, False, My.Resources.rFitFunction_Fano.Parameter_LinearBackground))
    End Sub
#End Region

#Region "FitFunction"
    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' </summary>
    ''' <param name="x">x-value at which the fit function should be evaluated.</param>
    Public Overrides Function GetY(ByRef x As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

        ' Calculate the function value.
        Return FitFunction(x,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Y0.ToString).Value,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Amplitude.ToString).Value,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.ResonantWidth.ToString).Value,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.XCenter.ToString).Value,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.FanoFactor.ToString).Value,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.LinearBackground.ToString).Value)

    End Function

    ''' <summary>
    ''' Fit-Function, Lorentzian-Distribution
    ''' </summary>
    Public Shared Function FitFunction(ByVal x As Double,
                                       ByVal YOffset As Double,
                                       ByVal Amplitude As Double,
                                       ByVal ResonantWidth As Double,
                                       ByVal XCenter As Double,
                                       ByVal FanoFactor As Double,
                                       ByVal LinearBackground As Double) As Double

        'ResonantWidth = ResonantWidth / 2
        'x = x - XCenter
        'Dim d As Double = (FanoFactor * ResonantWidth + x)
        'Return YOffset + Amplitude * d * d / (ResonantWidth * ResonantWidth + x * x)

        Dim e As Double = (x - XCenter) / ResonantWidth
        Dim ReturnValue As Double = (FanoFactor + e) * (FanoFactor + e)
        ReturnValue /= (1 + FanoFactor * FanoFactor)
        ReturnValue /= (1 + e * e)

        Return ReturnValue * Amplitude + YOffset + LinearBackground * x
    End Function
#End Region

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_Fano.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_Fano.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_Fano.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_Fano.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_Fano
        End Get
    End Property
#End Region

End Class
