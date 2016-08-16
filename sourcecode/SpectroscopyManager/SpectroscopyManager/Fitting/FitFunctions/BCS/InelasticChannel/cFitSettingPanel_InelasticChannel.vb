Public Class cFitSettingPanel_InelasticChannel
    Inherits cFitSettingPanel_TipSampleConvolution

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Protected _FitFunctionIETS As cFitFunction_TipSampleConvolutionBase

#Region "Constructor"

    ''' <summary>
    ''' Initializes the Fit-Function
    ''' </summary>
    Public Overrides Sub InitializeFitFunction(ByVal FitFunction As iFitFunction)
        ' Set the Fit-Function of this class
        Me._FitFunctionIETS = DirectCast(FitFunction, cFitFunction_TipSampleConvolutionBase)
        MyBase.InitializeFitFunction(FitFunction)
    End Sub

    ''' <summary>
    ''' Register all IECs of the fit function
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean

        For Each IEC As cFitFunction_TipSampleConvolutionBase.InelasticChannel In Me._FitFunctionIETS.InelasticChannels

            ' Reregister parameters
            Me.InelasticChannelAddedOrRemoved()

            '###############
            ' Create the interface

            ' Create the settings-panel
            Dim IECPanel As cFitSettingSubParameter_InelasticChannel = IEC.GetInelasticChannelPanel(Me.FitFunction.FitParametersGroupedWithoutCombinedGroup)

            ' Add the panel to the flow-layout-panel
            Me.flpInelasticChannels.Controls.Add(IECPanel)

            ' and add the index IEC to the panel-list, to keep
            ' track of the IEC ID and connect it to a panel index
            Me.InelasticChannelIDsToPanelIndices.Add(IEC.IECIndex)

            ' Raise the event, that a parameter-changed
            Me.InelasticChannel_ParameterValueChanged()

            ' Register the remove-button
            AddHandler IECPanel.RequestRemovalOfSubPanel, AddressOf RemoveInelasticChannel
            AddHandler IEC.ValueChanged, AddressOf InelasticChannel_ParameterValueChanged

        Next

        Me.RaiseSubParametersAddedOrRemoved()

        Return MyBase.BindFitParameters
    End Function

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_IEC_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' do nothing so far
    End Sub

    ''' <summary>
    ''' Sub that resets the FitParameterGroup to lock parameters together.
    ''' </summary>
    Public Overrides Function SetFitParameterGroups(ByRef FitParameterGroups As cFitParameterGroupGroup) As Boolean
        For Each C As Control In Me.flpInelasticChannels.Controls
            If C.GetType Is GetType(cFitSettingSubParameter_InelasticChannel) Then
                DirectCast(C, cFitSettingSubParameter_InelasticChannel).SetFitParameterGroups(FitParameterGroups)
            End If
        Next
        Return MyBase.SetFitParameterGroups(FitParameterGroups)
    End Function

#End Region

#Region "Manage Inelastic Channels"

    ''' <summary>
    ''' Add an inelastic channel to the Fit-Function.
    ''' </summary>
    Private Sub btnAddInelasticChannel_Click(sender As Object, e As EventArgs) Handles btnAddInelasticChannel.Click
        Me.AddInelasticChannel()
    End Sub

    ''' <summary>
    ''' Dictionary to keep track of all IECs with their identifiers
    ''' and the corresponding Settings-Panels
    ''' </summary>
    Private InelasticChannelIDsToPanelIndices As New List(Of Integer)

    ''' <summary>
    ''' Add an inelastic channel to the Fit-Function.
    ''' </summary>
    Private Function AddInelasticChannel() As Integer
        '################
        ' Register the peak in the FitFunction

        Me._FitFunctionIETS.AddInelasticChannel()
        Dim iIECIndex As Integer = Me._FitFunctionIETS.InelasticChannels.Count - 1

        ' Reregister parameters
        Me.InelasticChannelAddedOrRemoved()

        '###############
        ' Create the interface

        ' Create the settings-panel
        Dim IECPanel As cFitSettingSubParameter_InelasticChannel = Me._FitFunctionIETS.InelasticChannels(iIECIndex).GetInelasticChannelPanel(Me.FitFunction.FitParametersGroupedWithoutCombinedGroup)
        ' Add the panel to the flow-layout-panel
        Me.flpInelasticChannels.Controls.Add(IECPanel)

        ' and add the index SGP to the panel-list, to keep
        ' track of the SGP ID and connect it to a panel index
        Me.InelasticChannelIDsToPanelIndices.Add(Me._FitFunctionIETS.InelasticChannels(iIECIndex).IECIndex)

        ' Raise the event, that a parameter-changed
        Me.InelasticChannel_ParameterValueChanged()

        ' Register the remove-button
        AddHandler IECPanel.RequestRemovalOfSubPanel, AddressOf RemoveInelasticChannel
        AddHandler Me._FitFunctionIETS.InelasticChannels(iIECIndex).ValueChanged, AddressOf InelasticChannel_ParameterValueChanged

        ' Raises the event that parameters were added or removed
        Me.RaiseSubParametersAddedOrRemoved()

        Return iIECIndex
    End Function

    ''' <summary>
    ''' Removes an inelastic channel panel from the list
    ''' </summary>
    Private Sub RemoveInelasticChannel(ByRef PanelToRemove As cFitSettingSubParameterPanel_2)

        ' Get the index of the control, as its position should be the same as the IEC in the SGP-List
        Dim iIECIndex As Integer = Me.flpInelasticChannels.Controls.IndexOf(PanelToRemove)
        Dim iIECIdentifier As Integer = Me.InelasticChannelIDsToPanelIndices(iIECIndex)

        ' remove all the handlers
        RemoveHandler PanelToRemove.RequestRemovalOfSubPanel, AddressOf RemoveInelasticChannel
        RemoveHandler Me._FitFunctionIETS.InelasticChannels(iIECIndex).ValueChanged, AddressOf InelasticChannel_ParameterValueChanged

        ' Remove all the controls!
        Me.flpInelasticChannels.Controls.RemoveAt(iIECIndex)
        Me.InelasticChannelIDsToPanelIndices.RemoveAt(iIECIndex)
        Me._FitFunctionIETS.RemoveInelasticChannel(iIECIndex)

        ' Reregister parameters
        Me.InelasticChannelAddedOrRemoved()

        ' Raise the event, that a parameter-changed
        Me.InelasticChannel_ParameterValueChanged()
    End Sub

    ''' <summary>
    ''' If an IEC got added or removed, register it
    ''' to all other textboxes, to keep track of the
    ''' locked parameters.
    ''' </summary>
    Private Sub InelasticChannelAddedOrRemoved()

        ' Reinitialize all the Fit-Parameters
        Me._FitFunctionIETS.ReInitializeFitParameters()

        ' Register the new fit-parameter-array to all parameter-textboxes.
        For Each TextBoxKV As KeyValuePair(Of mFitParameter, String) In Me.CurrentFitParameterTextBoxes
            TextBoxKV.Key.SetFitParameterGroups(Me._FitFunctionIETS.FitParametersGroupedWithoutCombinedGroup)
        Next
        For Each C As Control In Me.flpInelasticChannels.Controls
            If C.GetType Is GetType(cFitSettingSubParameter_InelasticChannel) Then
                DirectCast(C, cFitSettingSubParameter_InelasticChannel).SetFitParameterGroups(Me.FitFunction.FitParametersGroupedWithoutCombinedGroup)
            End If
        Next

    End Sub

    ''' <summary>
    ''' Write current settings of the inelastic channels to the fit-function.
    ''' </summary>
    Private Sub InelasticChannel_ParameterValueChanged()
        ' Raise the parameter-changed event, to get a re-paint of the preview
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' If we get the final parameter-set after the fit,
    ''' we should write back the Fit-Parameters belonging to the inelastic channels.
    ''' The InelasticChannel-Array of the FitFunction should contain all current InelasticChannel-Settings.
    ''' So extract them by comparing the Fit-Parameter-Identifiers and write back the values.
    ''' </summary>
    Protected Overrides Sub WriteBackFitParameters(ByRef InputParameters As cFitParameterGroupGroup)

        ' Go through all sub-gap-peaks and extract the parameters
        For Each IEC As cFitFunction_TipSampleConvolutionBase.InelasticChannel In Me._FitFunctionIETS.InelasticChannels

            ' Go through all fit-parameters
            For Each FP As cFitParameter In InputParameters.Group(Me.FitFunction.UseFitParameterGroupID)

                Select Case FP.Identifier

                    Case cFitFunction_TipSampleConvolutionBase.InelasticChannel.IECIdentifier_Energy(IEC.IECIndex)
                        ' Found IEC energy parameter

                        ' Go to the Settingspanel and set the parameters
                        Dim IECPanelIndex As Integer = Me.InelasticChannelIDsToPanelIndices.IndexOf(IEC.IECIndex)
                        Dim IECPanel As cFitSettingSubParameter_InelasticChannel = DirectCast(Me.flpInelasticChannels.Controls(IECPanelIndex), cFitSettingSubParameter_InelasticChannel)
                        IECPanel.FitParameter1.ChangeValue(FP.Value, False)
                        IECPanel.fpFitParameter1.SetValue(FP.Value)

                    Case cFitFunction_TipSampleConvolutionBase.InelasticChannel.IECIdentifier_Probability(IEC.IECIndex)
                        ' Found IEC probability parameter

                        ' Go to the Settingspanel and set the parameters
                        Dim IECPanelIndex As Integer = Me.InelasticChannelIDsToPanelIndices.IndexOf(IEC.IECIndex)
                        Dim IECPanel As cFitSettingSubParameter_InelasticChannel = DirectCast(Me.flpInelasticChannels.Controls(IECPanelIndex), cFitSettingSubParameter_InelasticChannel)
                        IECPanel.FitParameter2.ChangeValue(FP.Value, False)
                        IECPanel.fpFitParameter2.SetValue(FP.Value)

                End Select

            Next

        Next

        ' Call base-function to set normal parameters
        MyBase.WriteBackFitParameters(InputParameters)
    End Sub

#End Region
End Class
