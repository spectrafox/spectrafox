<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wExportWizard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wExportWizard))
        Me.cbExportType = New System.Windows.Forms.ComboBox()
        Me.btnStartExport = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.etExportFolder = New ExpTreeLib.ExpTree()
        Me.gbExportFolder = New System.Windows.Forms.GroupBox()
        Me.btnGoToCurrentWorkingDirectory = New System.Windows.Forms.Button()
        Me.gbExportFiles = New System.Windows.Forms.GroupBox()
        Me.lbExportFiles = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtExportFormatDesciption = New System.Windows.Forms.TextBox()
        Me.pbProgress = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.gbExportFolder.SuspendLayout()
        Me.gbExportFiles.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbExportType
        '
        Me.cbExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbExportType.FormattingEnabled = True
        Me.cbExportType.Location = New System.Drawing.Point(6, 19)
        Me.cbExportType.Name = "cbExportType"
        Me.cbExportType.Size = New System.Drawing.Size(429, 21)
        Me.cbExportType.TabIndex = 6
        '
        'btnStartExport
        '
        Me.btnStartExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartExport.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnStartExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStartExport.Location = New System.Drawing.Point(270, 440)
        Me.btnStartExport.Name = "btnStartExport"
        Me.btnStartExport.Size = New System.Drawing.Size(445, 38)
        Me.btnStartExport.TabIndex = 10
        Me.btnStartExport.Text = "Export Files"
        Me.btnStartExport.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(653, 519)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(62, 27)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'etExportFolder
        '
        Me.etExportFolder.AllowDrop = True
        Me.etExportFolder.AllowFolderRename = False
        Me.etExportFolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.etExportFolder.Location = New System.Drawing.Point(3, 51)
        Me.etExportFolder.Name = "etExportFolder"
        Me.etExportFolder.ShowRootLines = False
        Me.etExportFolder.Size = New System.Drawing.Size(253, 483)
        Me.etExportFolder.StartUpDirectory = ExpTreeLib.ExpTree.StartDir.MyComputer
        Me.etExportFolder.TabIndex = 15
        '
        'gbExportFolder
        '
        Me.gbExportFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbExportFolder.Controls.Add(Me.btnGoToCurrentWorkingDirectory)
        Me.gbExportFolder.Controls.Add(Me.etExportFolder)
        Me.gbExportFolder.Location = New System.Drawing.Point(8, 9)
        Me.gbExportFolder.Name = "gbExportFolder"
        Me.gbExportFolder.Size = New System.Drawing.Size(259, 537)
        Me.gbExportFolder.TabIndex = 16
        Me.gbExportFolder.TabStop = False
        Me.gbExportFolder.Text = "Export Folder"
        '
        'btnGoToCurrentWorkingDirectory
        '
        Me.btnGoToCurrentWorkingDirectory.Location = New System.Drawing.Point(6, 19)
        Me.btnGoToCurrentWorkingDirectory.Name = "btnGoToCurrentWorkingDirectory"
        Me.btnGoToCurrentWorkingDirectory.Size = New System.Drawing.Size(247, 26)
        Me.btnGoToCurrentWorkingDirectory.TabIndex = 16
        Me.btnGoToCurrentWorkingDirectory.Text = "switch to current working directory"
        Me.btnGoToCurrentWorkingDirectory.UseVisualStyleBackColor = True
        '
        'gbExportFiles
        '
        Me.gbExportFiles.Controls.Add(Me.lbExportFiles)
        Me.gbExportFiles.Location = New System.Drawing.Point(274, 12)
        Me.gbExportFiles.Name = "gbExportFiles"
        Me.gbExportFiles.Size = New System.Drawing.Size(441, 208)
        Me.gbExportFiles.TabIndex = 17
        Me.gbExportFiles.TabStop = False
        Me.gbExportFiles.Text = "Files selected for export"
        '
        'lbExportFiles
        '
        Me.lbExportFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbExportFiles.FormattingEnabled = True
        Me.lbExportFiles.Location = New System.Drawing.Point(3, 16)
        Me.lbExportFiles.Name = "lbExportFiles"
        Me.lbExportFiles.Size = New System.Drawing.Size(435, 189)
        Me.lbExportFiles.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtExportFormatDesciption)
        Me.GroupBox1.Controls.Add(Me.cbExportType)
        Me.GroupBox1.Location = New System.Drawing.Point(274, 226)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(441, 208)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Export Format"
        '
        'txtExportFormatDesciption
        '
        Me.txtExportFormatDesciption.Location = New System.Drawing.Point(6, 47)
        Me.txtExportFormatDesciption.Multiline = True
        Me.txtExportFormatDesciption.Name = "txtExportFormatDesciption"
        Me.txtExportFormatDesciption.ReadOnly = True
        Me.txtExportFormatDesciption.Size = New System.Drawing.Size(429, 155)
        Me.txtExportFormatDesciption.TabIndex = 7
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(270, 485)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(445, 23)
        Me.pbProgress.TabIndex = 18
        Me.pbProgress.Value = 60
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(270, 511)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(145, 13)
        Me.lblProgress.TabIndex = 19
        Me.lblProgress.Text = "export running ... please wait."
        '
        'wExportWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(727, 558)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.pbProgress)
        Me.Controls.Add(Me.gbExportFolder)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnStartExport)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbExportFiles)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wExportWizard"
        Me.Text = "Export Wizard"
        Me.gbExportFolder.ResumeLayout(False)
        Me.gbExportFiles.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbExportType As System.Windows.Forms.ComboBox
    Friend WithEvents btnStartExport As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents etExportFolder As ExpTreeLib.ExpTree
    Friend WithEvents gbExportFolder As System.Windows.Forms.GroupBox
    Friend WithEvents gbExportFiles As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtExportFormatDesciption As System.Windows.Forms.TextBox
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents lbExportFiles As System.Windows.Forms.ListBox
    Friend WithEvents btnGoToCurrentWorkingDirectory As System.Windows.Forms.Button
End Class
