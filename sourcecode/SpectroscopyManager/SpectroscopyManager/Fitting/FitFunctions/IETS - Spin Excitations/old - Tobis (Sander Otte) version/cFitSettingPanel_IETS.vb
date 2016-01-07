Public Class cFitSettingPanel_IETS
    Inherits cFitSettingPanel

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As New cFitFunction_IETS

#Region "Constructor"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_Fano_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Set the Fit-Function of the BaseClass
        MyBase._FitFunction = Me._FitFunction

        ' Initialize the Layout
        MyBase.Initialize()

        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As cFitFunction_IETS.FitParameterIdentifier

        ' apply the last settings
        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.Amplitude
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.LinearSlope
        Me.fpLinearSlope.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.D
        Me.fpIETS_D.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.E
        Me.fpIETS_E.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.Temperature
        Me.fpTemperature.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.Y0
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.XCenter
        Me.fpXCenter.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.BField
        Me.fpMagneticField.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.BAngle
        Me.fpMagneticFieldAngle.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_IETS.FitParameterIdentifier.g
        Me.fpIETS_gFactor.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.Amplitude, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.LinearSlope, Me.fpLinearSlope)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.XCenter, Me.fpXCenter)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.Y0, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.D, Me.fpIETS_D)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.E, Me.fpIETS_E)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.Temperature, Me.fpTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.BField, Me.fpMagneticField)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.BAngle, Me.fpMagneticFieldAngle)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS.FitParameterIdentifier.g, Me.fpIETS_gFactor)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_IETS.FitParameterIdentifier.Y0, 0D)
        ' END: Registerin finished
        '#############################################

        '###################################
        ' Write the Data-Type Combobox
        ' Initialize the spin combobox
        With Me.cbSpin
            For i As Integer = 1 To 20 Step 1
                .Items.Add(New KeyValuePair(Of Integer, String)(i, (i * 0.5).ToString("N1")))
            Next
            .ValueMember = "Key"
            .DisplayMember = "Value"

            ' Select the initial spin
            .SelectedIndex = Me._FitFunction.SpinInOneHalfs - 1
        End With
        ' END: Write the Data-Type Combobox
        '###################################

        '##################################
        ' Write Calculation Settings
        Me.txtIntegrationStepSize.SetValue(cFitFunction_IETS.IntegrationEnergyStep)
        ' END: Write Calculation Settings
        '##################################

    End Sub
#End Region

#Region "Parameter-Selection by Range-Selection"

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectAmplitude_Click(sender As Object, e As EventArgs) Handles btnSelectAmplitude.Click
        MyBase.RaiseYRangeSelectionRequest(AddressOf Me.ChangeAmplitudeFromYRange)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeAmplitudeFromYRange(UpperValue As Double, LowerValue As Double)
        Me.fpAmplitude.SetValue(UpperValue - LowerValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectYOffset_Click(sender As Object, e As EventArgs) Handles btnSelectYOffset.Click
        MyBase.RaiseYValueSelectionRequest(AddressOf Me.ChangeYOffsetFromYValue)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeYOffsetFromYValue(YValue As Double)
        Me.fpYOffset.SetValue(YValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectXCenter_Click(sender As Object, e As EventArgs) Handles btnSelectXCenter.Click
        MyBase.RaiseXValueSelectionRequest(AddressOf Me.ChangeXCenterFromXValue)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeXCenterFromXValue(XValue As Double)
        Me.fpXCenter.SetValue(XValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

#End Region

#Region "Additional Settings, not belonging to a certain FitParameter (e.g. Fit-Data-Type)"
    ''' <summary>
    ''' Change the selected spin.
    ''' </summary>
    Private Sub cbSpin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSpin.SelectedIndexChanged
        ' the selected spin is always one larger in the index
        Me._FitFunction.SpinInOneHalfs = Me.cbSpin.SelectedIndex + 1
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Change the intergration energy step size
    ''' </summary>
    Private Sub txtIntegrationStepSize_TextChanged(ByRef sender As NumericTextbox) Handles txtIntegrationStepSize.ValidValueChanged
        cFitFunction_IETS.IntegrationEnergyStep = Me.txtIntegrationStepSize.DecimalValue
        Me.RaiseParameterChangedEvent()
    End Sub
#End Region

End Class
