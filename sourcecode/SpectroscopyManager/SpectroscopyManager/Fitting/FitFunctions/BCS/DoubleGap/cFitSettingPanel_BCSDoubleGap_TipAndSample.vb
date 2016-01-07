Public Class cFitSettingPanel_BCSDoubleGap_TipAndSample
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
        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.SystemBroadening.ToString
        Me.fpBroadeningWidth.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Sample1.ToString
        Me.fpSampleGap1.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Sample2.ToString
        Me.fpSampleGap2.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Tip1.ToString
        Me.fpTipGap1.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Tip2.ToString
        Me.fpTipGap2.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.GlobalXOffset.ToString
        Me.fpXOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.GlobalYOffset.ToString
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSBase.BCSAmplitude.ToString
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSBase.ImaginaryDamping.ToString
        Me.fpImaginaryDamping.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2.ToString
        Me.fpSampleGapRatio.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.RatioTip1ToTip2.ToString
        Me.fpTipGapRatio.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.T_sample.ToString
        Me.fpSampleTemperature.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.T_tip.ToString
        Me.fpTipTemperature.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.SystemBroadening.ToString, Me.fpBroadeningWidth)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Sample1.ToString, Me.fpSampleGap1)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Sample2.ToString, Me.fpSampleGap2)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Tip1.ToString, Me.fpTipGap1)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.Delta_Tip2.ToString, Me.fpTipGap2)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.GlobalXOffset.ToString, Me.fpXOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.GlobalYOffset.ToString, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSBase.BCSAmplitude.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSBase.ImaginaryDamping.ToString, Me.fpImaginaryDamping)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2.ToString, Me.fpSampleGapRatio)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifierBCSDoubleGap.RatioTip1ToTip2.ToString, Me.fpTipGapRatio)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.T_sample.ToString, Me.fpSampleTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.T_tip.ToString, Me.fpTipTemperature)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_BCSDoubleGap_TipAndSample.FitParameterIdentifier.GlobalYOffset.ToString, 0D)
        ' END: Registerin finished
        '#############################################
        Return MyBase.BindFitParameters
    End Function

#End Region

#Region "Parameter-Selection by Range-Selection"

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnXRange_Click(sender As Object, e As EventArgs) _
        Handles btnSelect_BroadeningWidth.Click, btnSelect_Sample_Gap1.Click, btnSelect_Sample_Gap2.Click
        If sender Is btnSelect_BroadeningWidth Then
            MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeBroadeningWidthFromXRange)
        ElseIf sender Is btnSelect_Sample_Gap1 Then
            MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeSampleGap1FromXRange)
        ElseIf sender Is btnSelect_Sample_Gap2 Then
            MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeSampleGap2FromXRange)
        End If
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeBroadeningWidthFromXRange(LeftValue As Double, RightValue As Double)
        Me.fpBroadeningWidth.SetValue(RightValue - LeftValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the sample gap by a selected range in the preview-image of the fit-window.
    ''' The value taken is referenced to the current tip gap!
    ''' </summary>
    Private Sub ChangeSampleGap1FromXRange(LeftValue As Double, RightValue As Double)
        Me.fpSampleGap1.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap1.DecimalValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the sample gap by a selected range in the preview-image of the fit-window.
    ''' The value taken is referenced to the current tip gap!
    ''' </summary>
    Private Sub ChangeSampleGap2FromXRange(LeftValue As Double, RightValue As Double)
        Me.fpSampleGap2.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap1.DecimalValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectYOffset_Click(sender As Object, e As EventArgs) Handles btnSelect_YOffset.Click
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
    Private Sub btnSelectXCenter_Click(sender As Object, e As EventArgs) Handles btnSelect_XOffset.Click
        MyBase.RaiseXValueSelectionRequest(AddressOf Me.ChangeXCenterFromXValue)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeXCenterFromXValue(XValue As Double)
        Me.fpXOffset.SetValue(-XValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

#End Region

End Class
