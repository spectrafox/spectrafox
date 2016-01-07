Public Class cFitFunction_Lorentz
    Inherits cFitFunction

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifier
        Y0
        Area
        Width
        XCenter
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Y0.ToString, 0, False, My.Resources.rFitFunction_Lorentz.Parameter_Y0))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.XCenter.ToString, 0, False, My.Resources.rFitFunction_Lorentz.Parameter_XCenter))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Area.ToString, 1, False, My.Resources.rFitFunction_Lorentz.Parameter_Area))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Width.ToString, 1, False, My.Resources.rFitFunction_Lorentz.Parameter_Width))
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
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Area.ToString).Value,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Width.ToString).Value,
                           InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.XCenter.ToString).Value)
    End Function

    ''' <summary>
    ''' Fit-Function, Lorentzian-Distribution
    ''' </summary>
    Public Shared Function FitFunction(ByVal x As Double,
                                       ByVal YOffset As Double,
                                       ByVal Area As Double,
                                       ByVal Width As Double,
                                       ByVal XCenter As Double) As Double
        Return cNumericFunctions.LorentzPeak(x, XCenter, Width, YOffset, Area)
    End Function
#End Region

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_Lorentz.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_Lorentz.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_Lorentz.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_Lorentz.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_Lorentz
        End Get
    End Property
#End Region

End Class
