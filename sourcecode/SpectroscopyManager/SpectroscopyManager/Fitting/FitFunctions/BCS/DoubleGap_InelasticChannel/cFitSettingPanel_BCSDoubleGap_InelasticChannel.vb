Public Class cFitSettingPanel_BCSDoubleGap_InelasticChannel
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
        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.SystemBroadening.ToString
        Me.fpBroadeningWidth.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.Delta_Sample1.ToString
        Me.fpSampleGap1.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.Delta_Sample2.ToString
        Me.fpSampleGap2.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.Delta_Tip.ToString
        Me.fpTipGap.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.GlobalXOffset.ToString
        Me.fpXOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.GlobalYOffset.ToString
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.GlobalYStretch.ToString
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSBase.ImaginaryDamping.ToString
        Me.fpImaginaryDamping.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2.ToString
        Me.fpSampleGapRatio.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.T_sample.ToString
        Me.fpSampleTemperature.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.T_tip.ToString
        Me.fpTipTemperature.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.SystemBroadening.ToString, Me.fpBroadeningWidth)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.Delta_Sample1.ToString, Me.fpSampleGap1)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.Delta_Sample2.ToString, Me.fpSampleGap2)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.Delta_Tip.ToString, Me.fpTipGap)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.GlobalXOffset.ToString, Me.fpXOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.GlobalYOffset.ToString, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.GlobalYStretch.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSBase.ImaginaryDamping.ToString, Me.fpImaginaryDamping)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2.ToString, Me.fpSampleGapRatio)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.T_sample.ToString, Me.fpSampleTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.T_tip.ToString, Me.fpTipTemperature)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_BCSDoubleGap_InelasticChannel.FitParameterIdentifier.GlobalYOffset.ToString, 0D)
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
        Me.fpSampleGap1.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap.DecimalValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the sample gap by a selected range in the preview-image of the fit-window.
    ''' The value taken is referenced to the current tip gap!
    ''' </summary>
    Private Sub ChangeSampleGap2FromXRange(LeftValue As Double, RightValue As Double)
        Me.fpSampleGap2.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap.DecimalValue)
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

#Region "Manage Inelastic Channels"

    ''' <summary>
    ''' List with all references to inelastic channel-panels.
    ''' </summary>
    Private InelasticChannelPanels As New List(Of cBCSFit_InelasticChannel)

    ''' <summary>
    ''' Add a inelastic channel to the Fit-Function.
    ''' </summary>
    Private Sub btnAddInelasticChannel_Click(sender As Object, e As EventArgs) Handles btnAddInelasticChannel.Click
        Me.AddInelasticChannel()
    End Sub

    ''' <summary>
    ''' Add an inelastic channel to the Fit-Function.
    ''' </summary>
    Private Sub AddInelasticChannel()
        ' Create Panel
        Dim InelasticChannelPanel As New cBCSFit_InelasticChannel

        ' Add the panel to the flow-layout-panel
        Me.InelasticChannelPanels.Add(InelasticChannelPanel)
        Me.flpInelasticChannels.Controls.Add(InelasticChannelPanel)

        ' Register the remove-button
        AddHandler InelasticChannelPanel.RequestRemovalOfInelasticChannel, AddressOf RemoveInelasticChannel
        AddHandler InelasticChannelPanel.ParameterValueChanged, AddressOf InelasticChannel_ParameterValueChanged
        AddHandler InelasticChannelPanel.FixationSettingChanged, AddressOf InelasticChannel_ParameterFixationChanged

        ' Raise the event, that a parameter-changed
        Me.InelasticChannel_ParameterValueChanged()
    End Sub

    ''' <summary>
    ''' Removes an inelastic channel panel from the list
    ''' </summary>
    Private Sub RemoveInelasticChannel(ByRef PanelToRemove As cBCSFit_InelasticChannel)

        ' Remove the handlers added
        RemoveHandler PanelToRemove.RequestRemovalOfInelasticChannel, AddressOf RemoveInelasticChannel
        RemoveHandler PanelToRemove.ParameterValueChanged, AddressOf InelasticChannel_ParameterValueChanged
        RemoveHandler PanelToRemove.FixationSettingChanged, AddressOf InelasticChannel_ParameterFixationChanged

        ' Remove all the controls!
        Me.flpInelasticChannels.Controls.Remove(PanelToRemove)
        Me.InelasticChannelPanels.Remove(PanelToRemove)

        ' Raise the event, that a parameter-changed
        Me.InelasticChannel_ParameterValueChanged()
    End Sub

    ''' <summary>
    ''' Write current settings of the inelastic channel to the fit-function.
    ''' </summary>
    Private Sub InelasticChannel_ParameterValueChanged()
        ' Clear current inelastic channel settings
        Me._FitFunction.ClearInelasticChannels()

        ' Add all settings from all setting-panels
        For Each InelasticChannelPanel As cBCSFit_InelasticChannel In Me.InelasticChannelPanels

            ' Create new inelastic channel
            Dim InelasticChannel As New cFitFunction_BCSDoubleGap_InelasticChannel.InelasticChannel

            ' copy the settings of the sub-gap-peak.
            With InelasticChannel
                .Energy = InelasticChannelPanel.txtIECEnergy.DecimalValue
                .Probability = InelasticChannelPanel.txtIECProbability.DecimalValue

                .EnergyFixed = InelasticChannelPanel.ckbFix_IECEnergy.Checked
                .ProbabilityFixed = InelasticChannelPanel.ckbFix_IECProbability.Checked
            End With

            ' Add the new inelastic channel to the FitFunction
            Me._FitFunction.AddInelasticChannel(InelasticChannel)

        Next

        ' Reinitialize all the Fit-Parameters
        Me._FitFunction.ReInitializeFitParameters()

        ' Raise the parameter-changed event, to get a re-paint of the preview
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Write current settings of the inelastic channels to the fit-function.
    ''' </summary>
    Private Sub InelasticChannel_ParameterFixationChanged()
        ' Clear current sub-gap-peak settings
        Me._FitFunction.ClearInelasticChannels()

        ' Add all settings from all setting-panels
        For Each InelasticChannelPanel As cBCSFit_InelasticChannel In Me.InelasticChannelPanels

            ' Create new Sub-Gap-peak
            Dim InelasticChannel As New cFitFunction_BCSDoubleGap_InelasticChannel.InelasticChannel

            ' copy the settings of the sub-gap-peak.
            With InelasticChannel
                .Energy = InelasticChannelPanel.txtIECEnergy.DecimalValue
                .Probability = InelasticChannelPanel.txtIECProbability.DecimalValue

                .EnergyFixed = InelasticChannelPanel.ckbFix_IECEnergy.Checked
                .ProbabilityFixed = InelasticChannelPanel.ckbFix_IECProbability.Checked
            End With

            ' Add the new sub-gap peak to the FitFunction
            Me._FitFunction.AddInelasticChannel(InelasticChannel)
        Next

        ' Reinitialize all the Fit-Parameters
        Me._FitFunction.ReInitializeFitParameters()

        ' NO need to raise the parameter-changed event!
        ' The fixation is not visible in the preview image.
    End Sub

    ''' <summary>
    ''' If we get the final parameter-set after the fit,
    ''' we should write back the Fit-Parameters belonging to the inelastic channels.
    ''' The InelasticChannel-Array of the FitFunction should contain all current InelasticChannel-Settings.
    ''' So take their values.
    ''' </summary>
    Protected Overrides Sub WriteBackFitParameters(ByRef FitParameters As Dictionary(Of Integer, cFitParameter))
        If Me.InelasticChannelPanels.Count <> Me._FitFunction.RegisteredInelasticChannels.Count Then
            MsgBox("Wrong InelasticChannel count!")
            Return
        End If

        ' Add all settings from all setting-panels
        For i As Integer = 0 To Me.InelasticChannelPanels.Count - 1 Step 1
            Dim InelasticChannelPanel As cBCSFit_InelasticChannel = Me.InelasticChannelPanels(i)
            Dim InelasticChannel As cFitFunction_BCSDoubleGap_InelasticChannel.InelasticChannel = Me._FitFunction.RegisteredInelasticChannels(i)

            ' copy the settings of the sub-gap-peak.
            With InelasticChannel
                InelasticChannelPanel.txtIECEnergy.SetValue(.Energy)
                InelasticChannelPanel.txtIECProbability.SetValue(.Probability)

                InelasticChannelPanel.ckbFix_IECEnergy.Checked = .EnergyFixed
                InelasticChannelPanel.ckbFix_IECProbability.Checked = .ProbabilityFixed
            End With
        Next

        ' Call base-function to set normal parameters
        MyBase.WriteBackFitParameters(FitParameters)
    End Sub

    ''' <summary>
    ''' Check after a successfull import of a fit-parameter, if it belongs to an inelastic channel,
    ''' and adjust the settings of the corresponding peak.
    ''' </summary>
    Protected Overrides Sub Import_ParameterIdentified(FitParameterKV As KeyValuePair(Of Integer, cFitParameter))
        ' Do nothing, if we treat regular parameters
        If FitParameterKV.Key < 100 Then Return

        ' Get the InelasticChannel-Index by dividing the Identifier by 100 and cut the rest.
        Dim InelasticChannelCount As Integer = CInt(FitParameterKV.Key / 100)
        Dim InelasticChannelIndex As Integer = InelasticChannelCount - 1

        ' Check, if we have enough inelastic channels for the index to be matched,
        ' otherwise create as many as needed!
        If Me.InelasticChannelPanels.Count < InelasticChannelCount Then
            For i As Integer = Me.InelasticChannelPanels.Count To InelasticChannelCount - 1 Step 1
                Me.AddInelasticChannel()
            Next
        End If

        ' Add the Parameter-Value, depending on the actual index
        Select Case FitParameterKV.Key
            Case (InelasticChannelCount * 100)
                ' Energy
                Me.InelasticChannelPanels(InelasticChannelIndex).txtIECEnergy.SetValue(FitParameterKV.Value.Value)
                Me.InelasticChannelPanels(InelasticChannelIndex).ckbFix_IECEnergy.Checked = FitParameterKV.Value.IsFixed
            Case (InelasticChannelCount * 100) + 1
                ' Probability
                Me.InelasticChannelPanels(InelasticChannelIndex).txtIECProbability.SetValue(FitParameterKV.Value.Value)
                Me.InelasticChannelPanels(InelasticChannelIndex).ckbFix_IECProbability.Checked = FitParameterKV.Value.IsFixed
        End Select
    End Sub

    ''' <summary>
    ''' After the import, set all inelastic channel parameters again to the FitFunction.
    ''' </summary>
    Protected Overrides Sub Import_Finished()
        Me.InelasticChannel_ParameterValueChanged()
    End Sub

#End Region

End Class
