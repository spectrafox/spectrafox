Public Class cFitSettingPanel_IETS_SpinExcitation
    Inherits cFitSettingPanel_TipSampleConvolution

#Region "Constructor"

    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As String

        ' apply the last settings
        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.GlobalYStretch.ToString
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.LinearSlope.ToString
        Me.fpLinearSlope.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.D.ToString
        Me.fpIETS_D.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.E.ToString
        Me.fpIETS_E.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.T_tip.ToString
        Me.fpTemperatureTip.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.T_sample.ToString
        Me.fpTemperatureSample.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.SystemBroadening.ToString
        Me.fpSystemBroadening.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.GlobalYOffset.ToString
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.GlobalXOffset.ToString
        Me.fpXCenter.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BField.ToString
        Me.fpMagneticField.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAngleTheta.ToString
        Me.fpMagneticFieldAngleTheta.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAnglePhi.ToString
        Me.fpMagneticFieldAnglePhi.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.g.ToString
        Me.fpIETS_gFactor.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.GlobalYStretch.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.LinearSlope.ToString, Me.fpLinearSlope)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.GlobalXOffset.ToString, Me.fpXCenter)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.GlobalYOffset.ToString, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.SystemBroadening.ToString, Me.fpSystemBroadening)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.D.ToString, Me.fpIETS_D)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.E.ToString, Me.fpIETS_E)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.T_tip.ToString, Me.fpTemperatureTip)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.T_sample.ToString, Me.fpTemperatureSample)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BField.ToString, Me.fpMagneticField)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAngleTheta.ToString, Me.fpMagneticFieldAngleTheta)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAnglePhi.ToString, Me.fpMagneticFieldAnglePhi)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.g.ToString, Me.fpIETS_gFactor)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_IETS_SpinExcitation.FitParameterIdentifier.GlobalYOffset.ToString, 0D)
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
            Me.SelectedSpinInOneHalfs = DirectCast(Me._FitFunction, cFitFunction_IETS_SpinExcitation).SpinInOneHalfs
        End With

        ' END: Write the Data-Type Combobox
        '###################################

        Return MyBase.BindFitParameters
    End Function
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

#Region "Additional Settings, not belonging to a certain FitParameter"

    ''' <summary>
    ''' Sets and gets the selected spin in the interface combobox.
    ''' </summary>
    Public Property SelectedSpinInOneHalfs As Integer
        Get
            Return Me.cbSpin.SelectedIndex + 1
        End Get
        Set(value As Integer)
            If Me.cbSpin.Items.Count > value Then
                Me.cbSpin.SelectedIndex = value - 1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Change the selected spin.
    ''' </summary>
    Private Sub cbSpin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSpin.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' the selected spin is always one larger in the index
        DirectCast(Me._FitFunction, cFitFunction_IETS_SpinExcitation).SpinInOneHalfs = Me.SelectedSpinInOneHalfs
        Me.RaiseParameterChangedEvent()
    End Sub

#End Region

End Class
