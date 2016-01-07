<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wMain
    Inherits SpectroscopyManager.wFormBase

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.msMainMenu = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBrowseFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuQuitProgram = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPrograms = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuProgramSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuComputingCloud = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActivateDeactivateComputingCloud = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuComputingCloudDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGPUComputing = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGPUComputingSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnableAutomaticUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInstallDevelopmentReleases = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp_ShowHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTest = New System.Windows.Forms.ToolStripMenuItem()
        Me.msMainMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'msMainMenu
        '
        Me.msMainMenu.Dock = System.Windows.Forms.DockStyle.Fill
        Me.msMainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuPrograms, Me.mnuWindows, Me.mnuComputingCloud, Me.mnuGPUComputing, Me.mnuHelp, Me.mnuTest})
        Me.msMainMenu.Location = New System.Drawing.Point(0, 0)
        Me.msMainMenu.Name = "msMainMenu"
        Me.msMainMenu.Size = New System.Drawing.Size(1016, 24)
        Me.msMainMenu.TabIndex = 0
        Me.msMainMenu.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuBrowseFolder, Me.ToolStripSeparator1, Me.mnuQuitProgram})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuBrowseFolder
        '
        Me.mnuBrowseFolder.Image = Global.SpectroscopyManager.My.Resources.Resources.folder_16
        Me.mnuBrowseFolder.Name = "mnuBrowseFolder"
        Me.mnuBrowseFolder.Size = New System.Drawing.Size(148, 22)
        Me.mnuBrowseFolder.Text = "Browse Folder"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(145, 6)
        '
        'mnuQuitProgram
        '
        Me.mnuQuitProgram.Image = Global.SpectroscopyManager.My.Resources.Resources.power_16
        Me.mnuQuitProgram.Name = "mnuQuitProgram"
        Me.mnuQuitProgram.Size = New System.Drawing.Size(148, 22)
        Me.mnuQuitProgram.Text = "Exit"
        '
        'mnuPrograms
        '
        Me.mnuPrograms.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuProgramSettings})
        Me.mnuPrograms.Name = "mnuPrograms"
        Me.mnuPrograms.Size = New System.Drawing.Size(65, 20)
        Me.mnuPrograms.Text = "Program"
        Me.mnuPrograms.Visible = False
        '
        'mnuProgramSettings
        '
        Me.mnuProgramSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.mnuProgramSettings.Name = "mnuProgramSettings"
        Me.mnuProgramSettings.Size = New System.Drawing.Size(116, 22)
        Me.mnuProgramSettings.Text = "Settings"
        '
        'mnuWindows
        '
        Me.mnuWindows.Name = "mnuWindows"
        Me.mnuWindows.Size = New System.Drawing.Size(68, 20)
        Me.mnuWindows.Text = "Windows"
        '
        'mnuComputingCloud
        '
        Me.mnuComputingCloud.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuActivateDeactivateComputingCloud, Me.mnuComputingCloudDetails})
        Me.mnuComputingCloud.Name = "mnuComputingCloud"
        Me.mnuComputingCloud.Size = New System.Drawing.Size(115, 20)
        Me.mnuComputingCloud.Text = "Computing Cloud"
        '
        'mnuActivateDeactivateComputingCloud
        '
        Me.mnuActivateDeactivateComputingCloud.Name = "mnuActivateDeactivateComputingCloud"
        Me.mnuActivateDeactivateComputingCloud.Size = New System.Drawing.Size(208, 22)
        Me.mnuActivateDeactivateComputingCloud.Text = "enable Computing Cloud"
        '
        'mnuComputingCloudDetails
        '
        Me.mnuComputingCloudDetails.Name = "mnuComputingCloudDetails"
        Me.mnuComputingCloudDetails.Size = New System.Drawing.Size(208, 22)
        Me.mnuComputingCloudDetails.Text = "Show Cloud Details"
        '
        'mnuGPUComputing
        '
        Me.mnuGPUComputing.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGPUComputingSettings})
        Me.mnuGPUComputing.Name = "mnuGPUComputing"
        Me.mnuGPUComputing.Size = New System.Drawing.Size(106, 20)
        Me.mnuGPUComputing.Text = "GPU Computing"
        '
        'mnuGPUComputingSettings
        '
        Me.mnuGPUComputingSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.mnuGPUComputingSettings.Name = "mnuGPUComputingSettings"
        Me.mnuGPUComputingSettings.Size = New System.Drawing.Size(116, 22)
        Me.mnuGPUComputingSettings.Text = "Settings"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEnableAutomaticUpdate, Me.mnuInstallDevelopmentReleases, Me.ToolStripSeparator2, Me.mnuHelp_ShowHelp, Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'mnuEnableAutomaticUpdate
        '
        Me.mnuEnableAutomaticUpdate.Checked = True
        Me.mnuEnableAutomaticUpdate.CheckOnClick = True
        Me.mnuEnableAutomaticUpdate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuEnableAutomaticUpdate.Name = "mnuEnableAutomaticUpdate"
        Me.mnuEnableAutomaticUpdate.Size = New System.Drawing.Size(248, 22)
        Me.mnuEnableAutomaticUpdate.Text = "check for updates automatically"
        '
        'mnuInstallDevelopmentReleases
        '
        Me.mnuInstallDevelopmentReleases.Checked = True
        Me.mnuInstallDevelopmentReleases.CheckOnClick = True
        Me.mnuInstallDevelopmentReleases.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuInstallDevelopmentReleases.Name = "mnuInstallDevelopmentReleases"
        Me.mnuInstallDevelopmentReleases.Size = New System.Drawing.Size(248, 22)
        Me.mnuInstallDevelopmentReleases.Text = "install only stable releases"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(245, 6)
        '
        'mnuAbout
        '
        Me.mnuAbout.Image = Global.SpectroscopyManager.My.Resources.Resources.info_16
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(248, 22)
        Me.mnuAbout.Text = "about SpectraFox"
        '
        'mnuHelp_ShowHelp
        '
        Me.mnuHelp_ShowHelp.Image = Global.SpectroscopyManager.My.Resources.Resources.question_16
        Me.mnuHelp_ShowHelp.Name = "mnuHelp_ShowHelp"
        Me.mnuHelp_ShowHelp.Size = New System.Drawing.Size(248, 22)
        Me.mnuHelp_ShowHelp.Text = "open online help in web-browser"
        '
        'mnuTest
        '
        Me.mnuTest.Name = "mnuTest"
        Me.mnuTest.Size = New System.Drawing.Size(41, 20)
        Me.mnuTest.Text = "Test"
        '
        'wMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 24)
        Me.Controls.Add(Me.msMainMenu)
        Me.MainMenuStrip = Me.msMainMenu
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(4000, 63)
        Me.MinimumSize = New System.Drawing.Size(400, 63)
        Me.Name = "wMain"
        Me.Text = "Welcome"
        Me.msMainMenu.ResumeLayout(False)
        Me.msMainMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents msMainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuBrowseFolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPrograms As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindows As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuQuitProgram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuProgramSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuComputingCloud As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuActivateDeactivateComputingCloud As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuComputingCloudDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGPUComputing As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGPUComputingSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTest As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuEnableAutomaticUpdate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuInstallDevelopmentReleases As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuHelp_ShowHelp As ToolStripMenuItem
End Class
