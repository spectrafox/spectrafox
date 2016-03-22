Public Class cNumericSmoothingPanel_AdjacentAverageSmooth

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.nudNeighbors.Value = My.Settings.Smoothing_AdjacentAverage_LastNeighbors

    End Sub

    ''' <summary>
    ''' Store the last used settings.
    ''' </summary>
    Private Sub nudNeighbors_ValueChanged(sender As Object, e As EventArgs) Handles nudNeighbors.ValueChanged
        My.Settings.Smoothing_AdjacentAverage_LastNeighbors = CInt(Me.nudNeighbors.Value)
    End Sub
End Class
