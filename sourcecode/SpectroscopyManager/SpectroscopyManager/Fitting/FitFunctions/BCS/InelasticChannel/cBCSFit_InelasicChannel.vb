Public Class cBCSFit_InelasticChannel

    ''' <summary>
    ''' Raise the parameter-changed event.
    ''' </summary>
    Public Property RaiseParameterChangedEvent As Boolean = True

#Region "Constructor"
    ''' <summary>
    ''' Initialize values
    ''' </summary>
    Private Sub cBCSFit_InelasticChannel_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set initial guess
        RaiseParameterChangedEvent = False
        Me.txtIECProbability.SetValue(0.5)
        Me.txtIECEnergy.SetValue(0.0005)
        RaiseParameterChangedEvent = True

        ' Raise the parameter-changed event one time!
        RaiseEvent ParameterValueChanged()
    End Sub
#End Region

#Region "Remove the inelastic channel"
    ''' <summary>
    ''' Event to remove the inelastic channel out of the fit-functions-DOS.
    ''' </summary>
    Public Event RequestRemovalOfInelasticChannel(ByRef PanelToRemove As cBCSFit_InelasticChannel)

    ''' <summary>
    ''' Raise the event to remove the inelastic channel.
    ''' </summary>
    Private Sub btnRemoveInelasticChannel_Click(sender As Object, e As EventArgs) Handles btnRemoveInelasticChannel.Click
        RaiseEvent RequestRemovalOfInelasticChannel(Me)
    End Sub
#End Region

#Region "Fixation Changed"

    ''' <summary>
    ''' Event, if a fixation setting changed.
    ''' </summary>
    Public Event FixationSettingChanged()

    ''' <summary>
    ''' Raise an event, if a fixation setting changed!
    ''' </summary>
    Private Sub ckbFix_CheckedChanged(sender As Object, e As EventArgs) _
        Handles ckbFix_IECProbability.CheckedChanged,
                 ckbFix_IECEnergy.CheckedChanged
        If RaiseParameterChangedEvent Then RaiseEvent FixationSettingChanged()
    End Sub
#End Region

#Region "Parameter-Value changed"
    ''' <summary>
    ''' Event, if a parameter-value changed.
    ''' </summary>
    Public Event ParameterValueChanged()

    ''' <summary>
    ''' Raise an event, if a parameter-value changes.
    ''' </summary>
    Private Sub txtNegPosRatio_TextChanged(ByRef TB As NumericTextbox) _
        Handles txtIECEnergy.ValidValueChanged,
                 txtIECProbability.ValidValueChanged
        If RaiseParameterChangedEvent Then RaiseEvent ParameterValueChanged()
    End Sub
#End Region

End Class
