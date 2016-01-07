Imports System.Deployment.Application
Imports System.IO
Imports System.Net

''' <summary>
''' Helper Class for funtions regarding the deployment of the program.
''' </summary>
Public Class cProgrammDeployment

#Region "Program Update"
    ''' <summary>
    ''' Function, that checks if SpectraFox
    ''' needs to download an update via the click-once procedures.
    ''' 
    ''' OBSOLTE! Was used for ClickOnce deployment!
    ''' </summary>
    ''' <returns>True, if Application restarts.</returns>
    Public Shared Function CheckForUpdates() As Boolean

        ' Update-Check only possible if clickonce is active.
        If My.Application.IsNetworkDeployed Then
            Try
                ' get the reference to the current ClickOnce-Deployment
                Dim CurrentDeployment As ApplicationDeployment = ApplicationDeployment.CurrentDeployment

                ' Check for Update
                Dim Update As UpdateCheckInfo = CurrentDeployment.CheckForDetailedUpdate

                ' Update is ready
                If Update.UpdateAvailable Then
                    Dim PerformUpdate As Boolean = True
                    ' If the update is not mandatory, ask the user if to do so.
                    If Not Update.IsUpdateRequired Then
                        Dim UpdateAnnouncement As New wProgramUpdateAvailable
                        If UpdateAnnouncement.ShowDialog() = DialogResult.Cancel Then
                            PerformUpdate = False
                        End If
                    End If

                    ' Update the Program
                    If PerformUpdate Then
                        Dim DeployForm As New wProgramUpdate
                        DeployForm.AppDeploy = CurrentDeployment

                        DeployForm.ShowDialog()

                        ' If Update was successfull, restart app
                        If DeployForm.ShouldAppRestart Then
                            'Application.ExitThread()
                            Application.Exit()
                            Application.Restart()
                            End

                            Return True
                        End If
                    End If
                End If
            Catch ex As Exception
                Debug.WriteLine("Update-Check failed! Exception: " & ex.Message)
                Return False
            End Try
        End If
        Return False
    End Function

#End Region

#Region "Version String"
    ''' <summary>
    ''' Returns the version string of the program.
    ''' </summary>
    Public Shared Function GetProgramVersionString() As String

        Dim sVersion As String = My.Resources._FormBase_VersionString
        sVersion = System.String.Format(sVersion,
                                        My.Application.Info.Version.Major,
                                        My.Application.Info.Version.Minor,
                                        My.Application.Info.Version.Build,
                                        My.Application.Info.Version.Revision)
        If IntPtr.Size = 8 Then
            sVersion &= " x64"
        Else
            sVersion &= " x86"
        End If
        Return sVersion

        ' old ClickOnce Deployment
        'If My.Application.IsNetworkDeployed Then
        '    Return String.Format(My.Resources._FormBase_VersionString,
        '                         My.Application.Deployment.CurrentVersion.Major,
        '                         My.Application.Deployment.CurrentVersion.Minor,
        '                         My.Application.Deployment.CurrentVersion.Build,
        '                         My.Application.Deployment.CurrentVersion.Revision)
        'Else
        '    Return My.Resources._FormBase_VersionStringDev
        'End If

    End Function
#End Region

#Region "Changelog"
    ''' <summary>
    ''' Local ChangeLog-File
    '''  - Replaced by the online readable html file directly loaded by a browser-object.
    ''' </summary>
    Public Shared Function GetChangeLog() As String
        Dim ChangeLog As String = "Changelog"
        Try
            If My.Settings.AutoUpdateOnlyStable Then
                ChangeLog = File.ReadAllText(Application.StartupPath & Path.DirectorySeparatorChar & "changelog_stable.html")
            Else
                ChangeLog = File.ReadAllText(Application.StartupPath & Path.DirectorySeparatorChar & "changelog_dev.html")
            End If
        Catch ex As Exception
            ' File not readable
        End Try
        Return ChangeLog
    End Function
#End Region

#Region "License Agreement"
    ''' <summary>
    ''' Local LicenseAgreement-File
    ''' </summary>
    Public Shared Function GetEULA() As String
        Dim License As String = "License"
        Try
            License = File.ReadAllText(Application.StartupPath & Path.DirectorySeparatorChar & "License.txt")
        Catch ex As Exception
            ' File not readable
        End Try
        Return License
    End Function
#End Region

#Region "Program ICON"

    ''' <summary>
    ''' Returns the program's icon.
    ''' </summary>
    Public Shared Function ProgramIcon() As Icon
        Return My.Resources.SpectraFoxLogo3
    End Function

#End Region

End Class
