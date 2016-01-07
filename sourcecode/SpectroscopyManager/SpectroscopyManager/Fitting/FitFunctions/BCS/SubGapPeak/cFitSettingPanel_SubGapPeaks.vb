Public Class cFitSettingPanel_SubGapPeaks
    Inherits cFitSettingPanel_TipSampleConvolution

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Protected _FitFunctionSGP As iFitFunction_SubGapPeaks

    ''' <summary>
    ''' Change the Sub-Gap-Peak-Type: Gauss or Lorentz
    ''' </summary>
    Private Sub cboSubGapPeakType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubGapPeakType.SelectedIndexChanged
        If Me._FitFunctionSGP Is Nothing Then Return
        Select Case Me.cboSubGapPeakType.SelectedIndex
            Case 0
                ' Gauss
                Me._FitFunctionSGP.SubGapPeakType = iFitFunction_SubGapPeaks.SubGapPeakTypes.Gauss
            Case 1
                ' Lorentz
                Me._FitFunctionSGP.SubGapPeakType = iFitFunction_SubGapPeaks.SubGapPeakTypes.Lorentz
        End Select
        Me.RaiseParameterChangedEvent()
    End Sub

#Region "Constructor"

    ''' <summary>
    ''' Initializes the Fit-Function
    ''' </summary>
    Public Overrides Sub InitializeFitFunction(ByVal FitFunction As iFitFunction)
        ' Set the Fit-Function of this class
        Me._FitFunctionSGP = DirectCast(FitFunction, iFitFunction_SubGapPeaks)
        MyBase.InitializeFitFunction(FitFunction)
    End Sub

    ''' <summary>
    ''' Register all SubGapPeaks present
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean

        For Each SGP As iFitFunction_SubGapPeaks.SubGapPeak In Me._FitFunctionSGP.SubGapPeaks

            ' Reregister parameters
            Me.SubGapPeakAddedOrRemoved()

            '###############
            ' Create the interface

            ' Create the settings-panel
            Dim SubGapPeakPanel As cFitSettingSubParameter_SubGapPeakPanel = SGP.GetSubGapPeakPanel(Me.FitFunction.FitParametersGroupedWithoutCombinedGroup)
            ' Add the panel to the flow-layout-panel
            Me.flpSubGapPeaks.Controls.Add(SubGapPeakPanel)

            ' and add the index SGP to the panel-list, to keep
            ' track of the SGP ID and connect it to a panel index
            Me.SubGapPeakIDsToPanelIndices.Add(SGP.SGPIndex)

            ' Raise the event, that a parameter-changed
            Me.SubGapPeak_ParameterValueChanged()

            ' Register the remove-button
            AddHandler SubGapPeakPanel.RequestRemovalOfSubGapPeak, AddressOf RemoveSubGapPeak
            AddHandler SubGapPeakPanel.PeakEnabledChanged, AddressOf SubGapPeakEnabledChanged
            AddHandler SubGapPeakPanel.ParentDOSChanged, AddressOf SubGapPeakParentDOSChanged
            AddHandler SGP.SubGapPeakValueChanged, AddressOf SubGapPeak_ParameterValueChanged

        Next

        Me.RaiseSubParametersAddedOrRemoved()

        Return MyBase.BindFitParameters
    End Function

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_SubGapPeak_Load(sender As Object, e As EventArgs) Handles Me.Load
        '###################################
        ' Write the Sub-Gap-Peak-Type Combobox
        With Me.cboSubGapPeakType
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of iFitFunction_SubGapPeaks.SubGapPeakTypes, String)(iFitFunction_SubGapPeaks.SubGapPeakTypes.Gauss, My.Resources.rBCSFit_SubGapPeak.SubGapPeakType_Gauss))
            .Items.Add(New KeyValuePair(Of iFitFunction_SubGapPeaks.SubGapPeakTypes, String)(iFitFunction_SubGapPeaks.SubGapPeakTypes.Lorentz, My.Resources.rBCSFit_SubGapPeak.SubGapPeakType_Lorentz))
            .ValueMember = "Key"
            .DisplayMember = "Value"
            If Not Me._FitFunctionSGP Is Nothing Then
                Select Case Me._FitFunctionSGP.SubGapPeakType
                    Case iFitFunction_SubGapPeaks.SubGapPeakTypes.Gauss
                        .SelectedIndex = 0
                    Case iFitFunction_SubGapPeaks.SubGapPeakTypes.Lorentz
                        .SelectedIndex = 1
                End Select
            End If
        End With
        ' END: Write the Sub-Gap-Peak-Type Combobox
        '###################################
    End Sub

    ''' <summary>
    ''' Sub that resets the FitParameterGroup to lock parameters together.
    ''' </summary>
    Public Overrides Function SetFitParameterGroups(ByRef FitParameterGroups As cFitParameterGroupGroup) As Boolean
        For Each C As Control In Me.flpSubGapPeaks.Controls
            If C.GetType Is GetType(cFitSettingSubParameter_SubGapPeakPanel) Then
                DirectCast(C, cFitSettingSubParameter_SubGapPeakPanel).SetFitParameterGroups(FitParameterGroups)
            End If
        Next
        Return MyBase.SetFitParameterGroups(FitParameterGroups)
    End Function

#End Region

#Region "Manage Sub-Gap-Peaks"

    ''' <summary>
    ''' Add a sub-gap-peak to the Fit-Function.
    ''' </summary>
    Private Sub btnAddSubGapPeak_Click(sender As Object, e As EventArgs) Handles btnAddSubGapPeak.Click
        Me.AddSubGapPeak()
    End Sub

    ''' <summary>
    ''' Dictionary to keep track of all SGP with their identifiers
    ''' and the corresponding Settings-Panels
    ''' </summary>
    Private SubGapPeakIDsToPanelIndices As New List(Of Integer)

    ''' <summary>
    ''' Add a sub-gap-peak to the Fit-Function.
    ''' </summary>
    Private Function AddSubGapPeak() As Integer
        '################
        ' Register the peak in the FitFunction

        Me._FitFunctionSGP.AddSubGapPeak()
        Dim iSGPIndex As Integer = Me._FitFunctionSGP.SubGapPeaks.Count - 1

        ' Reregister parameters
        Me.SubGapPeakAddedOrRemoved()

        '###############
        ' Create the interface

        ' Create the settings-panel
        Dim SubGapPeakPanel As cFitSettingSubParameter_SubGapPeakPanel = Me._FitFunctionSGP.SubGapPeaks(iSGPIndex).GetSubGapPeakPanel(Me.FitFunction.FitParametersGroupedWithoutCombinedGroup)
        ' Add the panel to the flow-layout-panel
        Me.flpSubGapPeaks.Controls.Add(SubGapPeakPanel)

        ' and add the index SGP to the panel-list, to keep
        ' track of the SGP ID and connect it to a panel index
        Me.SubGapPeakIDsToPanelIndices.Add(Me._FitFunctionSGP.SubGapPeaks(iSGPIndex).SGPIndex)

        ' Raise the event, that a parameter-changed
        Me.SubGapPeak_ParameterValueChanged()

        ' Register the remove-button
        AddHandler SubGapPeakPanel.RequestRemovalOfSubGapPeak, AddressOf RemoveSubGapPeak
        AddHandler SubGapPeakPanel.PeakEnabledChanged, AddressOf SubGapPeakEnabledChanged
        AddHandler SubGapPeakPanel.ParentDOSChanged, AddressOf SubGapPeakParentDOSChanged
        AddHandler Me._FitFunctionSGP.SubGapPeaks(iSGPIndex).SubGapPeakValueChanged, AddressOf SubGapPeak_ParameterValueChanged

        ' Raises the event that parameters were added or removed
        Me.RaiseSubParametersAddedOrRemoved()

        Return iSGPIndex
    End Function

    ''' <summary>
    ''' Removes a sub-gap-peak panel from the list
    ''' </summary>
    Private Sub RemoveSubGapPeak(ByRef PanelToRemove As cFitSettingSubParameterPanel_4)

        ' Get the index of the control, as its position should be the same
        ' as the SGP in the SGP-List
        Dim iSGPIndex As Integer = Me.flpSubGapPeaks.Controls.IndexOf(PanelToRemove)
        Dim iSGPIdentifier As Integer = Me.SubGapPeakIDsToPanelIndices(iSGPIndex)

        ' remove all the handlers
        RemoveHandler PanelToRemove.RequestRemovalOfSubGapPeak, AddressOf RemoveSubGapPeak
        RemoveHandler CType(PanelToRemove, cFitSettingSubParameter_SubGapPeakPanel).ParentDOSChanged, AddressOf SubGapPeakParentDOSChanged
        RemoveHandler CType(PanelToRemove, cFitSettingSubParameter_SubGapPeakPanel).PeakEnabledChanged, AddressOf SubGapPeakEnabledChanged
        RemoveHandler Me._FitFunctionSGP.SubGapPeaks(iSGPIndex).SubGapPeakValueChanged, AddressOf SubGapPeak_ParameterValueChanged

        ' Remove all the controls!
        Me.flpSubGapPeaks.Controls.RemoveAt(iSGPIndex)
        Me.SubGapPeakIDsToPanelIndices.RemoveAt(iSGPIndex)
        Me._FitFunctionSGP.RemoveSubGapPeak(iSGPIndex)
        
        ' Reregister parameters
        Me.SubGapPeakAddedOrRemoved()

        ' Raise the event, that a parameter-changed
        Me.SubGapPeak_ParameterValueChanged()
    End Sub

    ''' <summary>
    ''' If a SGP got added or removed, register it
    ''' to all other textboxes, to keep track of the
    ''' locked parameters.
    ''' </summary>
    Private Sub SubGapPeakAddedOrRemoved()
        ' Reinitialize all the Fit-Parameters
        Me._FitFunctionSGP.ReInitializeFitParameters()

        ' Register the new fit-parameter-array to all parameter-textboxes.
        For Each TextBoxKV As KeyValuePair(Of mFitParameter, String) In Me.CurrentFitParameterTextBoxes
            TextBoxKV.Key.SetFitParameterGroups(Me._FitFunctionSGP.FitParametersGroupedWithoutCombinedGroup)
        Next
        For Each C As Control In Me.flpSubGapPeaks.Controls
            If C.GetType Is GetType(cFitSettingSubParameter_SubGapPeakPanel) Then
                DirectCast(C, cFitSettingSubParameter_SubGapPeakPanel).SetFitParameterGroups(Me.FitFunction.FitParametersGroupedWithoutCombinedGroup)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Enabled state of a SubGapPeak changed
    ''' </summary>
    Public Sub SubGapPeakEnabledChanged(ByRef SubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak, ByVal Enabled As Boolean)
        Me.SubGapPeakAddedOrRemoved()
        Me.SubGapPeak_ParameterValueChanged()
    End Sub

    ''' <summary>
    ''' ParentDOS of a SubGapPeak changed
    ''' </summary>
    Public Sub SubGapPeakParentDOSChanged(ByRef SubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak, ByVal ParentDOS As cFitFunction_TipSampleConvolutionBase.DOSTypes)
        Me.SubGapPeakAddedOrRemoved()
        Me.SubGapPeak_ParameterValueChanged()
    End Sub

    ''' <summary>
    ''' Write current settings of the sub-gap peaks to the fit-function.
    ''' </summary>
    Private Sub SubGapPeak_ParameterValueChanged()
        ' Raise the parameter-changed event, to get a re-paint of the preview
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' If we get the final parameter-set after the fit,
    ''' we should write back the Fit-Parameters belonging to the Sub-Gap-Peaks.
    ''' The SubGapPeak-Array of the FitFunction should contain all current Sub-Gap-Peak-Settings.
    ''' So extract them by comparing the Fit-Parameter-Identifiers and write back the values.
    ''' </summary>
    Protected Overrides Sub WriteBackFitParameters(ByRef InputParameters As cFitParameterGroupGroup)

        ' Go through all sub-gap-peaks and extract the parameters
        For Each SGP As iFitFunction_SubGapPeaks.SubGapPeak In Me._FitFunctionSGP.SubGapPeaks

            ' Go through all fit-parameters
            For Each FP As cFitParameter In InputParameters.Group(Me.FitFunction.UseFitParameterGroupID)

                Select Case FP.Identifier

                    Case iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_XCenter(SGP.SGPIndex)
                        ' Found XCenter

                        ' Go to the Settingspanel and set the parameters
                        Dim SGPPanelIndex As Integer = Me.SubGapPeakIDsToPanelIndices.IndexOf(SGP.SGPIndex)
                        Dim SGPPanel As cFitSettingSubParameter_SubGapPeakPanel = DirectCast(Me.flpSubGapPeaks.Controls(SGPPanelIndex), cFitSettingSubParameter_SubGapPeakPanel)

                        ' XCenter
                        SGPPanel.FitParameter1.ChangeValue(FP.Value, False)
                        SGPPanel.fpFitParameter1.SetValue(FP.Value)

                    Case iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_Amplitude(SGP.SGPIndex)
                        ' Found amplitude

                        ' Go to the Settingspanel and set the parameters
                        Dim SGPPanelIndex As Integer = Me.SubGapPeakIDsToPanelIndices.IndexOf(SGP.SGPIndex)
                        Dim SGPPanel As cFitSettingSubParameter_SubGapPeakPanel = DirectCast(Me.flpSubGapPeaks.Controls(SGPPanelIndex), cFitSettingSubParameter_SubGapPeakPanel)

                        ' amplitude
                        SGPPanel.FitParameter2.ChangeValue(FP.Value, False)
                        SGPPanel.fpFitParameter2.SetValue(FP.Value)

                    Case iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_Width(SGP.SGPIndex)
                        ' Found Width

                        ' Go to the Settingspanel and set the parameters
                        Dim SGPPanelIndex As Integer = Me.SubGapPeakIDsToPanelIndices.IndexOf(SGP.SGPIndex)
                        Dim SGPPanel As cFitSettingSubParameter_SubGapPeakPanel = DirectCast(Me.flpSubGapPeaks.Controls(SGPPanelIndex), cFitSettingSubParameter_SubGapPeakPanel)

                        ' Width
                        SGPPanel.FitParameter3.ChangeValue(FP.Value, False)
                        SGPPanel.fpFitParameter3.SetValue(FP.Value)

                    Case iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_PosNegRatio(SGP.SGPIndex)
                        ' Found PosNegRatio

                        ' Go to the Settingspanel and set the parameters
                        Dim SGPPanelIndex As Integer = Me.SubGapPeakIDsToPanelIndices.IndexOf(SGP.SGPIndex)
                        Dim SGPPanel As cFitSettingSubParameter_SubGapPeakPanel = DirectCast(Me.flpSubGapPeaks.Controls(SGPPanelIndex), cFitSettingSubParameter_SubGapPeakPanel)

                        ' PosNegRatio
                        SGPPanel.FitParameter4.ChangeValue(FP.Value, False)
                        SGPPanel.fpFitParameter4.SetValue(FP.Value)

                End Select

            Next

        Next

        ' Call base-function to set normal parameters
        MyBase.WriteBackFitParameters(InputParameters)
    End Sub

#End Region

End Class
