Imports System.Deployment.Application
Imports System.ComponentModel

''' <summary>
''' Form shows the Program Update Progress.
''' And additional informations, such as a ChangeLog.
''' </summary>
Public Class wProgramUpdate
    ''' <summary>
    ''' Content Constructor.
    ''' </summary>
    Private Sub wProgramUpdateProgress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Set color equal to banner!
        Me.BackColor = Color.FromArgb(181, 85, 8)

        ' Set Browser to load changelog url
        Me.wbChangeLog.Url = New Uri(My.Resources.rProgramUpdateMessages.ChangelogURL)

        ' Set Cursor to WaitCursor
        Me.Cursor = Cursors.WaitCursor

        ' Start Update-Procedure.
        If ApplicationDeployment.IsNetworkDeployed Then
            Try
                BeginUpdate()
            Catch exception As Exception
                MessageBox.Show(My.Resources.rProgramUpdateMessages.UnexpectedError & exception.Message)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

#Region "Properties"
    Public WithEvents AppDeploy As ApplicationDeployment
    Public Property ShouldAppRestart As Boolean
#End Region

    ''' <summary>
    ''' Starts the Update Procedure.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BeginUpdate()
        AppDeploy.UpdateAsync()
    End Sub

    ''' <summary>
    ''' Shows a message if the update was successfull.
    ''' </summary>
    Private Sub AppDeploy_UpdateCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles AppDeploy.UpdateCompleted
        ShouldAppRestart = False
        If e.Cancelled Then
            ' Update was cancelled.
            MessageBox.Show(My.Resources.rProgramUpdateMessages.UpdateFailedDueToCancellation, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf e.Error IsNot Nothing Then
            ' An error occured during the Update Procedure.
            MessageBox.Show(My.Resources.rProgramUpdateMessages.UpdateFailedDueToError, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        Else
            ShouldAppRestart = True
        End If
        Me.Close()
    End Sub

    ''' <summary>
    ''' Shows the Progress of the Update procedure in the progress bar and the
    ''' status text below the progress bar.
    ''' </summary>
    Private Sub AppDeploy_UpdateProgressChanged(sender As Object, e As DeploymentProgressChangedEventArgs) Handles AppDeploy.UpdateProgressChanged

        Dim progressText As String = My.Resources.rProgramUpdateMessages.UpdateDownloadProgress
        progressText = progressText.Replace("#bl", (e.BytesCompleted / 1024).ToString("N0"))
        progressText = progressText.Replace("#bt", (e.BytesTotal / 1024).ToString("N0"))
        progressText = progressText.Replace("#p", (e.ProgressPercentage).ToString("N2"))

        Me.lblUpdateProgress.Text = progressText
        Me.pgbUpdateProgress.Value = e.ProgressPercentage
    End Sub


End Class
