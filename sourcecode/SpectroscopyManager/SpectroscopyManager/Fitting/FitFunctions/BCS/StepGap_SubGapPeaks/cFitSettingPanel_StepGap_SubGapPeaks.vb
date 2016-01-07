Public Class cFitSettingPanel_StepGap_SubGapPeaks
    Inherits cFitSettingPanel_SubGapPeaks

#Region "Constructor"
  
    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As String

        ' apply the last settings
        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.SystemBroadening.ToString
        Me.fpBroadeningWidth.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSStepGap.Delta_Sample.ToString
        Me.fpSampleGap.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSStepGap.StepBroadening.ToString
        Me.fpStepBroadening.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSStepGap.Delta_Tip.ToString
        Me.fpTipGap.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.GlobalXOffset.ToString
        Me.fpXOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.GlobalYOffset.ToString
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSBase.BCSAmplitude.ToString
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSBase.ImaginaryDamping.ToString
        Me.fpImaginaryDamping.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.T_sample.ToString
        Me.fpSampleTemperature.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.T_tip.ToString
        Me.fpTipTemperature.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.SystemBroadening.ToString, Me.fpBroadeningWidth)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSStepGap.Delta_Sample.ToString, Me.fpSampleGap)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSStepGap.StepBroadening.ToString, Me.fpStepBroadening)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSStepGap.Delta_Tip.ToString, Me.fpTipGap)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.GlobalXOffset.ToString, Me.fpXOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.GlobalYOffset.ToString, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSBase.BCSAmplitude.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifierBCSBase.ImaginaryDamping.ToString, Me.fpImaginaryDamping)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.T_sample.ToString, Me.fpSampleTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.T_tip.ToString, Me.fpTipTemperature)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_StepGap_SubGapPeaks.FitParameterIdentifier.GlobalYOffset.ToString, 0D)
        ' END: Registering finished
        '#############################################
        Return MyBase.BindFitParameters
    End Function
#End Region

#Region "Parameter-Selection by Range-Selection"

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnXRange_Click(sender As Object, e As EventArgs) _
        Handles btnSelect_BroadeningWidth.Click,
                btnSelect_Sample_Gap2.Click,
                btnSelect_Sample_Gap1.Click
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
        Me.fpSampleGap.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap.DecimalValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the sample gap by a selected range in the preview-image of the fit-window.
    ''' The value taken is referenced to the current tip gap!
    ''' </summary>
    Private Sub ChangeSampleGap2FromXRange(LeftValue As Double, RightValue As Double)
        Me.fpStepBroadening.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap.DecimalValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectAmplitude_Click(sender As Object, e As EventArgs)
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
