Imports SpectroscopyManager

Public Class cFitFunction_IETS_SpinExcitation_FanoDOS
    Inherits cFitFunction_IETS_SpinExcitation


#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifier_Fano
        Y0 = 155400
        Amplitude
        XCenter
        ResonantWidth
        FanoFactor
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(FitParameterIdentifier_Fano.Y0, New sFitParameter("Y0_Fano", 0, False, My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Parameter_Y0))
        Me.FitParameters.Add(FitParameterIdentifier_Fano.XCenter, New sFitParameter("Xc_Fano", 0, False, My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Parameter_XCenter))
        Me.FitParameters.Add(FitParameterIdentifier_Fano.Amplitude, New sFitParameter("A_Fano", 1, False, My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Parameter_Amplitude))
        Me.FitParameters.Add(FitParameterIdentifier_Fano.ResonantWidth, New sFitParameter("GRes", 1, False, My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Parameter_ResonantWidth))
        Me.FitParameters.Add(FitParameterIdentifier_Fano.FanoFactor, New sFitParameter("q", 0, False, My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Parameter_FanoFactor))
        MyBase.InitializeFitParameters()
    End Sub
#End Region

#Region "FitFunction"

    ''' <summary>
    ''' Fit-Function, Lorentzian-Distribution
    ''' </summary>
    ''' 
    Public Function Fano(ByVal x As Double,
                                      ByVal YOffset As Double,
                                      ByVal Amplitude As Double,
                                      ByVal ResonantWidth As Double,
                                      ByVal XCenter As Double,
                                      ByVal FanoFactor As Double) As Double

        ResonantWidth = ResonantWidth / 2
        x = x - XCenter
        Dim d As Double = (FanoFactor * ResonantWidth + x)
        Return YOffset + Amplitude * d * d / (ResonantWidth * ResonantWidth + x * x)
    End Function

    Public Overrides Function SampleDOS(ByRef E As Double, ByRef Identifiers() As Integer, ByRef Values() As Double) As Double
        Return Fano(E,
                    sFitParameter.GetValueForIdentifier(FitParameterIdentifier_Fano.Y0, Identifiers, Values),
                    sFitParameter.GetValueForIdentifier(FitParameterIdentifier_Fano.Amplitude, Identifiers, Values),
                    sFitParameter.GetValueForIdentifier(FitParameterIdentifier_Fano.ResonantWidth, Identifiers, Values),
                    sFitParameter.GetValueForIdentifier(FitParameterIdentifier_Fano.XCenter, Identifiers, Values),
                    sFitParameter.GetValueForIdentifier(FitParameterIdentifier_Fano.FanoFactor, Identifiers, Values))
    End Function
#End Region

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_IETS_SpinExctitaion_FanoDOS.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_IETS_SpinExcitation_FanoDOS
        End Get
    End Property
#End Region

End Class
