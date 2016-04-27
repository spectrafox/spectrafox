Imports NetSparkle

Public Class wMain
    Inherits wFormBase

    ''' <summary>
    ''' automatic program updater
    ''' </summary>
    Private _SparkleUpdater As Sparkle

#Region "Main-Window Loading and Closing"
    ''' <summary>
    ''' Loading of the Main-Window
    ''' </summary>
    Private Sub wMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Debug.WriteLine(Now.ToString & " SpectraFox started and running in DEBUG-Mode")

        ' Report Status
        wSplash.SetStatus(My.Resources.rMain.LoadingMessages_CheckingSettingsFile)

        ' Check for updated settings:
        If My.Settings.SettingsUpgradeRequired Then
            My.Settings.Upgrade()
            My.Settings.SettingsUpgradeRequired = False
            cGlobal.SaveSettings()
        End If

        ' Change interface
        Me.mnuEnableAutomaticUpdate.Checked = My.Settings.AutoUpdate
        Me.mnuInstallDevelopmentReleases.Checked = My.Settings.AutoUpdateOnlyStable

        ' Setup the autoupdater.
        ' Check, if the setup was performed with low privileges,
        ' and modify the version information files, respectively.
        Dim LowPrivilegeSetupAppendix As String = String.Empty
        If IO.File.Exists(Application.StartupPath & "\LowPrivilegeSetup.txt") Then
            LowPrivilegeSetupAppendix = "_lowpriv"
        End If

        ' Now distinguish between the different update channels!
        If My.Settings.AutoUpdateOnlyStable Then
            _SparkleUpdater = New Sparkle("http://download.spectrafox.com/versioninfo_stable" & LowPrivilegeSetupAppendix & ".xml", cProgrammDeployment.ProgramIcon)
        Else
            _SparkleUpdater = New Sparkle("http://download.spectrafox.com/versioninfo_dev" & LowPrivilegeSetupAppendix & ".xml", cProgrammDeployment.ProgramIcon)
        End If

        ' start the Auto-Update, if settings don't forbid:
        If My.Settings.AutoUpdate Then
            ' Report Status
            wSplash.SetStatus(My.Resources.rMain.LoadingMessages_StartingAutoUpdater)

            ' Start the update-check
            _SparkleUpdater.StartLoop(True, True)
        End If

        ' Report Status
        wSplash.SetStatus(My.Resources.rMain.LoadingMessages_GettingScreenPosition)

        ' Set width and location depending on the TaskBar-Location and width!
        Dim TBLocation As TaskbarLocation = Me.GetTaskbarLocation
        Select Case TBLocation
            Case TaskbarLocation.Top
                Me.Width = SystemInformation.PrimaryMonitorSize.Width
                Me.Location = New Point(0, Me.GetTaskbarDimension.Height)
            Case TaskbarLocation.Left
                Me.Width = SystemInformation.PrimaryMonitorSize.Width - Me.GetTaskbarDimension.Width
                Me.Location = New Point(Me.GetTaskbarDimension.Width, 0)
            Case TaskbarLocation.Right
                Me.Width = SystemInformation.PrimaryMonitorSize.Width - Me.GetTaskbarDimension.Width
                Me.Location = New Point(0, 0)
            Case Else
                Me.Width = SystemInformation.PrimaryMonitorSize.Width
                Me.Location = New Point(0, 0)
        End Select

        ' Disable and hide some buttons, when not in Debug-Mode
#If Not Debug Then
        Me.mnuComputingCloud.Visible = False
        Me.mnuTest.Visible = False
#End If

        ' Start the Computing-Cloud,
        ' if saved like this in the settings.
#If DEBUG Then
        If True Then
#Else
        If My.Settings.ComputingCloud_StartAutomatic Then
#End If
            ' Report Status
            wSplash.SetStatus(My.Resources.rMain.LoadingMessages_StartingComputingCloud)
            Me.StartStopComputingCloud()
        End If

        ' Loading all plugins in the plugins-folder
        ' if no error occured before!
        If Not My.Settings.LoadWithoutPlugins Then
            Try
                wSplash.SetStatus(My.Resources.rMain.LoadingMessages_LoadingPlugins)

                Dim PluginPath As String = Application.StartupPath & "\plugins"

                ' If the plugin-folder exists:
                If System.IO.Directory.Exists(PluginPath) Then
                    ' Call the find plugins routine, to search in our Plugins Folder
                    cGlobal.Plugins.FindPlugins(PluginPath)
                End If
            Catch ex As Exception
                My.Settings.LoadWithoutPlugins = True
                cGlobal.SaveSettings()
            End Try
        Else
            My.Settings.LoadWithoutPlugins = False
            cGlobal.SaveSettings()
        End If

        ' Report Status
        wSplash.SetStatus(My.Resources.rMain.LoadingMessages_LoadingLastUsedFolder)

        ' Open the Folder-Selector window!
        Me.mnuOpenFolder_Click(Nothing, Nothing)

        ' Report Status
        wSplash.SetStatus(My.Resources.rMain.LoadingMessages_StartingInterface)
    End Sub

    ''' <summary>
    ''' Ask if closing is wanted
    ''' </summary>
    Private Sub wMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        ' Ask the obligatory question if not in Debug-Mode!
        If e.CloseReason = CloseReason.UserClosing Then
#If Not Debug Then
            If MessageBox.Show(My.Resources.Program_ClosingQuestion,
                           My.Resources.Program_ClosingQuestion_Title,
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Exclamation) = DialogResult.No Then
                e.Cancel = True
            End If
#End If
        End If

    End Sub

    ''' <summary>
    ''' Close on clicking the button.
    ''' </summary>
    Private Sub mnuQuitProgram_Click(sender As Object, e As EventArgs) Handles mnuQuitProgram.Click
        Me.Close()
    End Sub

#End Region

#Region "Window Sub-Menu"
    ''' <summary>
    ''' Dictionary to save all open windows.
    ''' </summary>
    Private WindowDropDownList As New Dictionary(Of ToolStripMenuItem, Form)

    ''' <summary>
    ''' On showing the Sub-Menu of the window button, get a list of all windows that are open in the program,
    ''' to display the list in the sub-menu.
    ''' </summary>
    Private Sub mnuWindows_DropDownOpening(sender As Object, e As EventArgs) Handles mnuWindows.DropDownOpening

        Dim DropDownItemList As New List(Of ToolStripMenuItem)
        WindowDropDownList.Clear()

        ' Go through all forms and show them in the list.
        For i As Integer = 0 To Application.OpenForms.Count - 1 Step 1

            ' Jump over the Main-Window itself!
            If TypeOf Application.OpenForms.Item(i) Is wMain Then Continue For

            ' Create menu button
            DropDownItemList.Add(New ToolStripMenuItem(Application.OpenForms.Item(i).Text))
            AddHandler DropDownItemList.Last().Click, AddressOf Me.SelectWindowInWindowMenu

            ' Save connection between MenuItem and Form
            WindowDropDownList.Add(DropDownItemList.Last(), Application.OpenForms.Item(i))

        Next

        ' Add the list to the DropDownItems
        Me.mnuWindows.DropDownItems.Clear()
        Me.mnuWindows.DropDownItems.AddRange(DropDownItemList.ToArray)

        ' Add a dummy item, to tell that there are now windows, if there are no windows open.
        If DropDownItemList.Count = 0 Then
            Dim DummyToolStripItem As New ToolStripMenuItem(My.Resources.rMain.WindowMenu_NoWindowsOpen)
            DummyToolStripItem.Enabled = False
            Me.mnuWindows.DropDownItems.Add(DummyToolStripItem)
        End If
    End Sub

    ''' <summary>
    ''' On clicking on a window in the window submenu, bring this window to front.
    ''' </summary>
    Private Sub SelectWindowInWindowMenu(sender As Object, e As EventArgs)
        Dim ToolStripItem As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        If WindowDropDownList.ContainsKey(ToolStripItem) Then
            Me.BringWindowToFront(WindowDropDownList(ToolStripItem))
        End If
    End Sub

    Private Delegate Sub _BringWindowToFront(ByRef Window As Form)
    ''' <summary>
    ''' Brings a Window from the background to the front (thread safe)
    ''' </summary>
    Private Sub BringWindowToFront(ByRef Window As Form)
        If Window.InvokeRequired Then
            Window.Invoke(New _BringWindowToFront(AddressOf Me.BringWindowToFront), Window)
        Else
            Window.BringToFront()
            Window.Focus()
        End If
    End Sub
#End Region

#Region "About-Window"
    ''' <summary>
    ''' Show About Window
    ''' </summary>
    Private Sub mnuAbout_Click(sender As System.Object, e As System.EventArgs) Handles mnuAbout.Click
        Dim AboutWindow As New wAbout
        AboutWindow.Show()
    End Sub
#End Region

#Region "Open Folder Dialog"
    ''' <summary>
    ''' Opens the window to select a folder to browse!
    ''' </summary>
    Private Sub mnuOpenFolder_Click(sender As Object, e As EventArgs) Handles mnuBrowseFolder.Click
        ' Only allow ONE folder selector!
        ' Therefor search in the open window-list for an open FolderSelector.
        Dim bFolderSelectorWindowFound As Boolean = False

        ' Go through all forms and show them in the list.
        For i As Integer = 0 To Application.OpenForms.Count - 1 Step 1

            If TypeOf Application.OpenForms.Item(i) Is wFolderBrowser Then
                bFolderSelectorWindowFound = True
                Application.OpenForms.Item(i).BringToFront()
            End If

        Next

        ' If no open window was found, open a new one!
        If Not bFolderSelectorWindowFound Then
            Dim oFolderSelector As New wFolderBrowser

            ' Open right under the main window.
            oFolderSelector.Show()
            oFolderSelector.Location = New Point(Me.Location.X, Me.Location.Y + Me.Height)
        End If
    End Sub
#End Region

#Region "Get Taskbar location and dimension, for positioning the main window"

    ''' <summary>
    ''' Possible locations of the taskbar!
    ''' </summary>
    Public Enum TaskbarLocation
        Top
        Bottom
        Left
        Right
        Hidden
    End Enum

    ''' <summary>
    ''' Returns the Taskbar Location
    ''' </summary>
    Public Function GetTaskbarLocation() As TaskbarLocation
        Dim bounds As Rectangle = Screen.PrimaryScreen.Bounds
        Dim working As Rectangle = Screen.PrimaryScreen.WorkingArea
        If working.Height < bounds.Height And working.Y > 0 Then
            Return TaskbarLocation.Top
        ElseIf working.Height < bounds.Height And working.Y = 0 Then
            Return TaskbarLocation.Bottom
        ElseIf working.Width < bounds.Width And working.X > 0 Then
            Return TaskbarLocation.Left
        ElseIf working.Width < bounds.Width And working.X = 0 Then
            Return TaskbarLocation.Right
        Else
            Return TaskbarLocation.Hidden
        End If
    End Function

    ''' <summary>
    ''' Returns the Taskbar Dimension
    ''' </summary>
    Public Function GetTaskbarDimension() As Size
        Dim bounds As Rectangle = Screen.PrimaryScreen.Bounds
        Dim working As Rectangle = Screen.PrimaryScreen.WorkingArea
        Dim TBLocation As TaskbarLocation = Me.GetTaskbarLocation
        Dim TBSize As New Size(0, 0)

        If TBLocation = TaskbarLocation.Top Or TBLocation = TaskbarLocation.Bottom Then
            TBSize = New Size(bounds.Width, bounds.Height - working.Height)
        ElseIf TBLocation = TaskbarLocation.Left Or TBLocation = TaskbarLocation.Right Then
            TBSize = New Size(bounds.Width - working.Width, bounds.Height)
        End If

        Return TBSize
    End Function


#End Region

#Region "Computing Cloud"

    ''' <summary>
    ''' Computing-Cloud Object, that is used to start the server.
    ''' </summary>
    Private ComputingCloud As cComputingCloud

    ''' <summary>
    ''' Click button to start the computing cloud.
    ''' </summary>
    Private Sub mnuActivateDeactivateComputingCloud_Click(sender As Object, e As EventArgs) Handles mnuActivateDeactivateComputingCloud.Click
        ' Try to start the computing cloud.
        Me.StartStopComputingCloud()
    End Sub

    ''' <summary>
    ''' Starts or stops the computing cloud of
    ''' the program.
    ''' </summary>
    Public Sub StartStopComputingCloud()
        ' Start, if the Computing-Cloud is not defined.
        If Me.ComputingCloud Is Nothing Then
            Me.ComputingCloud = New cComputingCloud
            If Not Me.ComputingCloud.Start() Then
                Me.ComputingCloud = Nothing
            End If
        Else
            Me.ComputingCloud.Stop()
            Me.ComputingCloud = Nothing
        End If

        ' Change the Check-State of the Activation button.
        Me.mnuActivateDeactivateComputingCloud.Checked = (Me.ComputingCloud IsNot Nothing)
        Me.mnuComputingCloudDetails.Enabled = (Me.ComputingCloud IsNot Nothing)
    End Sub

    ''' <summary>
    ''' Open settings-window of the computing cloud.
    ''' </summary>
    Private Sub mnuComputingCloudSettings_Click(sender As Object, e As EventArgs) Handles mnuComputingCloudDetails.Click
        If Not Me.ComputingCloud Is Nothing Then
            Dim w As New wComputingCloudDetails
            w.Show(Me.ComputingCloud)
        End If
    End Sub

    ''' <summary>
    ''' Open GPU-Computing Settings
    ''' </summary>
    Private Sub mnuGPUComputingSettings_Click(sender As Object, e As EventArgs) Handles mnuGPUComputingSettings.Click
        Dim w As New wGPUComputingSettings
        w.Show()
    End Sub

#End Region

    Private Sub mnuTest_Click(sender As Object, e As EventArgs) Handles mnuTest.Click
        Dim w As New wTest
        w.Show()
    End Sub

#Region "Auto-Update-Buttons: deactivate / activate the auto-update"
    ''' <summary>
    ''' Enable update
    ''' </summary>
    Private Sub mnuEnableAutomaticUpdate_Click(sender As Object, e As EventArgs) Handles mnuEnableAutomaticUpdate.Click
        If Me.mnuEnableAutomaticUpdate.Checked Then
            ' enable
            '########
            ' Start the update-check
            If Not _SparkleUpdater.IsUpdateLoopRunning Then
                _SparkleUpdater.StartLoop(True, True)
            End If
        Else
            ' disable
            '#########
            If _SparkleUpdater.IsUpdateLoopRunning Then
                _SparkleUpdater.StopLoop()
            End If
        End If

        My.Settings.AutoUpdate = Me.mnuEnableAutomaticUpdate.Checked
        cGlobal.SaveSettings()
    End Sub

    ''' <summary>
    ''' Enable beta updates
    ''' </summary>
    Private Sub mnuInstallDevelopmentReleases_Click(sender As Object, e As EventArgs) Handles mnuInstallDevelopmentReleases.Click
        My.Settings.AutoUpdateOnlyStable = Me.mnuInstallDevelopmentReleases.Checked
        cGlobal.SaveSettings()
    End Sub

    ''' <summary>
    ''' Opens the default web-browser to show the help page.
    ''' </summary>
    Private Sub mnuHelp_ShowHelp_Click(sender As Object, e As EventArgs) Handles mnuHelp_ShowHelp.Click
        Try
            Dim sInfo As ProcessStartInfo = New ProcessStartInfo(My.Settings.HelpFileURL)
            Process.Start(sInfo)
        Catch ex As Exception
            MessageBox.Show(My.Resources.rMain.Help_ErrorOpeningWebbrowser.Replace("%url", My.Settings.HelpFileURL).Replace("%e", ex.Message),
                            My.Resources.rMain.Help_ErrorOpeningWebbrowser_Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Open the settings window.
    ''' </summary>
    Private Sub mnuProgramSettings_Click(sender As Object, e As EventArgs) Handles mnuProgramSettings.Click
        Dim W As New wSettings_Program
        W.Show()
    End Sub

#End Region
End Class