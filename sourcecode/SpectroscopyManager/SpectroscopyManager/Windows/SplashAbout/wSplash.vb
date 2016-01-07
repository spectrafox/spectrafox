Public NotInheritable Class wSplash
    Private Sub wSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Verwenden Sie zum Formatieren der Versionsinformationen den Text, der zur Entwurfszeit in der Versionskontrolle festgelegt wurde, als
        '  Formatierungszeichenfolge. Dies ermöglicht ggf. eine effektive Lokalisierung.
        '  Build- und Revisionsinformationen können durch Verwendung des folgenden Codes und durch Ändern 
        '  des Entwurfszeittexts der Versionskontrolle in "Version {0}.{1:00}.{2}.{3}" oder einen ähnlichen Text eingeschlossen werden. Weitere Informationen erhalten Sie unter
        '  String.Format() in der Hilfe.
        '
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        Me.lblVersion.Text = cProgrammDeployment.GetProgramVersionString

        ' Set color equal to banner!
        Me.BackColor = Color.FromArgb(181, 85, 8)

    End Sub

    Private Delegate Sub _SetStatus(ByVal Status As String)
    ''' <summary>
    ''' Sets the status-message thread-safe.
    ''' </summary>
    Public Sub SetStatus(ByVal Status As String)
        If Me.lblStatus.InvokeRequired Then
            Me.lblStatus.Invoke(New _SetStatus(AddressOf Me.SetStatus), Status)
        Else
            Me.lblStatus.Text = Status
        End If
    End Sub


    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then My.Application.SplashScreen = Nothing
        Threading.Thread.Sleep(200)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

End Class
