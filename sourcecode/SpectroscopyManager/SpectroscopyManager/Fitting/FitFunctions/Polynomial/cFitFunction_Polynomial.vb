Public Class cFitFunction_Polynomial
    Inherits cFitFunction

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifier
        Y0
        XCenter
        XAmplitude
        a
        b
        c
        d
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Y0.ToString, 0, False, My.Resources.rFitFunction_Polynomial.Parameter_Y0))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.XCenter.ToString, 0, False, My.Resources.rFitFunction_Polynomial.Parameter_XCenter))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.XAmplitude.ToString, 1, False, My.Resources.rFitFunction_Polynomial.Parameter_Amplitude))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.a.ToString, 0, False, My.Resources.rFitFunction_Polynomial.Parameter_a))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.b.ToString, 0, False, My.Resources.rFitFunction_Polynomial.Parameter_b))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.c.ToString, 0, False, My.Resources.rFitFunction_Polynomial.Parameter_c))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.d.ToString, 0, False, My.Resources.rFitFunction_Polynomial.Parameter_d))
    End Sub
#End Region

#Region "FitFunction"
    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' </summary>
    ''' <param name="x">x-value at which the fit function should be evaluated.</param>
    Public Overrides Function GetY(ByRef x As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

        ' Calculate the function value.
        Return Me.FitFunction(x,
                              InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Y0.ToString).Value,
                              InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.XCenter.ToString).Value,
                              InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.XAmplitude.ToString).Value,
                              InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.a.ToString).Value,
                              InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.b.ToString).Value,
                              InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.c.ToString).Value,
                              InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.d.ToString).Value)
    End Function

    ''' <summary>
    ''' Fit-Function, Lorentzian-Distribution
    ''' </summary>
    Public Function FitFunction(ByVal x As Double,
                                ByVal YOffset As Double,
                                ByVal XCenter As Double,
                                ByVal XAmplitude As Double,
                                ByVal a As Double,
                                ByVal b As Double,
                                ByVal c As Double,
                                ByVal d As Double) As Double
        Dim xXc As Double = (x * XAmplitude - XCenter)
        Return YOffset + a * xXc + b * (xXc * xXc) + c * (xXc * xXc * xXc) + d * (xXc * xXc * xXc * xXc)
    End Function
#End Region

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_Polynomial.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_Polynomial.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_Polynomial.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_Polynomial.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_Polynomial
        End Get
    End Property
#End Region

End Class
