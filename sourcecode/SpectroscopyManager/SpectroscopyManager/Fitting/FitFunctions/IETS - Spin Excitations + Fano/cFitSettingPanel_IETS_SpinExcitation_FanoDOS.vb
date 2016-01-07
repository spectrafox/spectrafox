Public Class cFitSettingPanel_IETS_SpinExcitation_FanoDOS
    Inherits SpectroscopyManager.cFitSettingPanel_IETS_SpinExcitation

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As New cFitFunction_IETS_SpinExcitation_FanoDOS

#Region "Constructor"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    Protected Overrides Sub cFitSettingPanel_Load(sender As Object, e As EventArgs)

        ' Set the Fit-Function of the BaseClass
        MyBase._FitFunction = Me._FitFunction
        ' Tell others, that the fit function finally has been initialized!
        MyBase.OnFitFunctionInitialized()

        ' Initialize the Layout
        MyBase.Initialize()

        ' Bind all fit-parameters to the GUI
        Me.BindFitParameters()

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano

        ' apply the last settings
        CurrentIdentifier = cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.Amplitude
        Me.fpFanoAmplitude.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.FanoFactor
        Me.fpFanoQ.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.ResonantWidth
        Me.fpFanoGRes.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.XCenter
        Me.fpFanoXc.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.Y0
        Me.fpFanoY0.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)


        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.Amplitude, Me.fpFanoAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.FanoFactor, Me.fpFanoQ)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.ResonantWidth, Me.fpFanoGRes)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.XCenter, Me.fpFanoXc)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation_FanoDOS.FitParameterIdentifier_Fano.Y0, Me.fpFanoY0)
        ' END: Registerin finished
        '#############################################

        Return MyBase.BindFitParameters()
    End Function
#End Region

End Class
