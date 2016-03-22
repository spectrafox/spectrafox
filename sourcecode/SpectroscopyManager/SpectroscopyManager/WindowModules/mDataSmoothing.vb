Public Class mDataSmoothing

    ''' <summary>
    ''' A list that contains all available smoothing methods.
    ''' </summary>
    Public SmoothingMethods As New Dictionary(Of Type, iNumericSmoothingFunction)

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Loads a list with all smoothing methods.
        Dim SmoothingMethodTypes As List(Of Type) = cNumericalMethods.GetAllLoadableSmoothingMethods

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        With Me.cbMethods
            .Items.Clear()
            .ValueMember = "Key"
            .DisplayMember = "Value"

            For Each SmoothingMethodType As Type In SmoothingMethodTypes

                ' Create a new instance of this method.
                Me.SmoothingMethods.Add(SmoothingMethodType, cNumericalMethods.GetSmoothingMethodByType(SmoothingMethodType))

                ' Add a list entry to the interface.
                .Items.Add(New KeyValuePair(Of Type, String)(SmoothingMethodType, Me.SmoothingMethods(SmoothingMethodType).Name))

            Next

            If Me.cbMethods.Items.Count > 0 Then
                .SelectedIndex = 0
            End If
        End With
    End Sub

    ''' <summary>
    ''' Change the description and the settings-panel of the smoothing method.
    ''' </summary>
    Private Sub cbMethods_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbMethods.SelectedIndexChanged

        If Me.cbMethods.SelectedItem Is Nothing Then Return

        ' Get the selected smoothing method.
        Dim SM As iNumericSmoothingFunction = Me.GetSmoothingMethod

        ' Show the description of the smoothing method:
        Me.txtDescription.Text = SM.Description

        ' Show the settings-control
        Me.tpSettings.Controls.Clear()
        Me.tpSettings.Controls.Add(SM.SmoothingOptions)

    End Sub

    ''' <summary>
    ''' Set/Get selected smoothing method.
    ''' </summary>
    Public Property SelectedSmoothingMethodType() As Type
        Get
            If Me.cbMethods.SelectedItem Is Nothing Then Return Nothing
            Return DirectCast(Me.cbMethods.SelectedItem, KeyValuePair(Of Type, String)).Key
        End Get
        Set(value As Type)
            For Each Method As KeyValuePair(Of Type, String) In Me.cbMethods.Items
                If Method.Key = value Then
                    Me.cbMethods.SelectedItem = Method
                End If
            Next
        End Set
    End Property

    ''' <summary>
    ''' Returns the currently selected smoothing method with all parameters set up.
    ''' </summary>
    Public Function GetSmoothingMethod() As iNumericSmoothingFunction
        Dim SmoothingMethodType As Type = Me.SelectedSmoothingMethodType

        If SmoothingMethodType Is Nothing Then Return Nothing
        Return Me.SmoothingMethods(SmoothingMethodType)
    End Function

    ''' <summary>
    ''' Sets the current settings-string of the smoothing options.
    ''' </summary>
    Public Sub SetSmoothingSettings(ByVal SettingsString As String)
        Me.GetSmoothingMethod.CurrentSmoothingSettings = SettingsString
    End Sub

    ''' <summary>
    ''' Sets the current settings-string of the smoothing options.
    ''' </summary>
    Public Function GetSmoothingSettings() As String
        Return Me.GetSmoothingMethod.CurrentSmoothingSettings
    End Function

End Class
