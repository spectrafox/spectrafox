Public Class cNumericSmoothingPanel_SavitzkyGolay

    Private bReady As Boolean = False

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.nudNeighbors.Value = My.Settings.Smoothing_SavitzkyGolay_LastNeighbors

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Store the last used settings.
    ''' </summary>
    Private Sub nudNeighbors_ValueChanged(sender As Object, e As EventArgs) Handles nudNeighbors.ValueChanged
        If Not Me.bReady Then Return

        My.Settings.Smoothing_SavitzkyGolay_LastNeighbors = CInt(Me.nudNeighbors.Value)
    End Sub

End Class
