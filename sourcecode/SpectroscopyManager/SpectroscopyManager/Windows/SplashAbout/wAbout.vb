Public NotInheritable Class wAbout
    Inherits wFormBase

    ''' <summary>
    ''' Write About-Content
    ''' </summary>
    Private Sub wAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.lblVersion.Text = cProgrammDeployment.GetProgramVersionString
        'Me.txtChangeLog.Text = cProgrammDeployment.GetChangeLog 'My.Resources._ChangeLog ' My.Application.Info.Description
        Me.wbChangelog.DocumentText = cProgrammDeployment.GetChangeLog
        Me.txtEULA.Text = cProgrammDeployment.GetEULA

        ' Set color equal to banner!
        Me.BackColor = Color.FromArgb(181, 85, 8)
    End Sub

    ''' <summary>
    ''' Close window
    ''' </summary>
    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Open the SpectraFox paper.
    ''' </summary>
    Private Sub lblHint_Click(sender As Object, e As EventArgs) Handles lblHint.Click
        Try
            Dim sInfo As ProcessStartInfo = New ProcessStartInfo(My.Resources.SpectraFoxPaperURL)
            Process.Start(sInfo)
        Catch ex As Exception
            MessageBox.Show(My.Resources.rMain.Help_ErrorOpeningWebbrowser.Replace("%url", My.Resources.SpectraFoxPaperURL).Replace("%e", ex.Message),
                            My.Resources.rMain.Help_ErrorOpeningWebbrowser_Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
