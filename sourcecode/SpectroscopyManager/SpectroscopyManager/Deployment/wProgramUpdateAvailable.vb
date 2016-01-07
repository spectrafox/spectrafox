Public Class wProgramUpdateAvailable
    Inherits wFormBase

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub wProgramUpdateAvailable_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set Browser to load changelog url
        Me.wbChangeLog.Url = New Uri(My.Resources.rProgramUpdateMessages.ChangelogURL)
    End Sub
End Class