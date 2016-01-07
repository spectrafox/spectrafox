Public Class wProgress
    Inherits wFormBase

    Public Sub SetTitle(Title As String)
        Me.lblTitle.Text = Title
    End Sub

    Public Sub PostProgress(Percentage As Integer, Message As String)
        If Percentage > 100 Then
            Percentage = 100
        ElseIf Percentage < 0 Then
            Percentage = 0
        End If
        Me.pgbProgress.Value = Percentage
        Me.lblProgress.Text = Message
    End Sub
End Class