<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wFolderBrowser
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
        Me.dbDictionaryBrowser = New ExpTreeLib.ExpTree()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuRefreshView = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFolderHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lv1 = New SpectroscopyManager.ListView_DoubleBuffered()
        Me.chName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chLastModified = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chTypeStr = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chAttributes = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chCreated = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnBrowseFolder = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsslLeft = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslMiddle = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsslRight = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dbDictionaryBrowser
        '
        Me.dbDictionaryBrowser.AllowDrop = True
        Me.dbDictionaryBrowser.AllowFolderRename = False
        Me.dbDictionaryBrowser.Cursor = System.Windows.Forms.Cursors.Default
        Me.dbDictionaryBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dbDictionaryBrowser.Location = New System.Drawing.Point(0, 0)
        Me.dbDictionaryBrowser.Name = "dbDictionaryBrowser"
        Me.dbDictionaryBrowser.ShowHiddenFolders = False
        Me.dbDictionaryBrowser.ShowRootLines = False
        Me.dbDictionaryBrowser.Size = New System.Drawing.Size(314, 688)
        Me.dbDictionaryBrowser.StartUpDirectory = ExpTreeLib.ExpTree.StartDir.MyComputer
        Me.dbDictionaryBrowser.TabIndex = 20
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRefreshView, Me.mnuFolderHistory})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(721, 24)
        Me.MenuStrip1.TabIndex = 21
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuRefreshView
        '
        Me.mnuRefreshView.Image = Global.SpectroscopyManager.My.Resources.Resources.reload_16
        Me.mnuRefreshView.Name = "mnuRefreshView"
        Me.mnuRefreshView.Size = New System.Drawing.Size(89, 20)
        Me.mnuRefreshView.Text = "refresh list"
        Me.mnuRefreshView.ToolTipText = "refresh the folder if the content has changed"
        '
        'mnuFolderHistory
        '
        Me.mnuFolderHistory.Image = Global.SpectroscopyManager.My.Resources.Resources.favourite_16
        Me.mnuFolderHistory.Name = "mnuFolderHistory"
        Me.mnuFolderHistory.Size = New System.Drawing.Size(105, 20)
        Me.mnuFolderHistory.Text = "folder history"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.dbDictionaryBrowser)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lv1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(721, 688)
        Me.SplitContainer1.SplitterDistance = 314
        Me.SplitContainer1.TabIndex = 22
        '
        'lv1
        '
        Me.lv1.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lv1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chName, Me.chSize, Me.chLastModified, Me.chTypeStr, Me.chAttributes, Me.chCreated})
        Me.lv1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lv1.LabelEdit = True
        Me.lv1.Location = New System.Drawing.Point(0, 59)
        Me.lv1.Name = "lv1"
        Me.lv1.Size = New System.Drawing.Size(403, 629)
        Me.lv1.TabIndex = 2
        Me.lv1.UseCompatibleStateImageBehavior = False
        Me.lv1.View = System.Windows.Forms.View.Details
        '
        'chName
        '
        Me.chName.Text = "Name"
        Me.chName.Width = 150
        '
        'chSize
        '
        Me.chSize.Text = "Size"
        Me.chSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chSize.Width = 88
        '
        'chLastModified
        '
        Me.chLastModified.Text = "LastMod Date"
        Me.chLastModified.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chLastModified.Width = 122
        '
        'chTypeStr
        '
        Me.chTypeStr.Text = "Type"
        Me.chTypeStr.Width = 100
        '
        'chAttributes
        '
        Me.chAttributes.Text = "Attributes"
        Me.chAttributes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.chAttributes.Width = 80
        '
        'chCreated
        '
        Me.chCreated.Text = "Created"
        Me.chCreated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chCreated.Width = 122
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnBrowseFolder)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(403, 59)
        Me.Panel1.TabIndex = 3
        '
        'btnBrowseFolder
        '
        Me.btnBrowseFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnBrowseFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseFolder.Image = Global.SpectroscopyManager.My.Resources.Resources.openfolder_25
        Me.btnBrowseFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBrowseFolder.Location = New System.Drawing.Point(0, 0)
        Me.btnBrowseFolder.Name = "btnBrowseFolder"
        Me.btnBrowseFolder.Size = New System.Drawing.Size(403, 59)
        Me.btnBrowseFolder.TabIndex = 0
        Me.btnBrowseFolder.Text = "open data-browser for the selected folder"
        Me.btnBrowseFolder.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslLeft, Me.tsslMiddle, Me.tsslRight})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 712)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(721, 22)
        Me.StatusStrip1.TabIndex = 23
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'tsslLeft
        '
        Me.tsslLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsslLeft.Name = "tsslLeft"
        Me.tsslLeft.Size = New System.Drawing.Size(39, 17)
        Me.tsslLeft.Text = "Ready"
        '
        'tsslMiddle
        '
        Me.tsslMiddle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsslMiddle.Name = "tsslMiddle"
        Me.tsslMiddle.Size = New System.Drawing.Size(667, 17)
        Me.tsslMiddle.Spring = True
        '
        'tsslRight
        '
        Me.tsslRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsslRight.Name = "tsslRight"
        Me.tsslRight.Size = New System.Drawing.Size(0, 17)
        '
        'wFolderBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(721, 734)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "wFolderBrowser"
        Me.Text = "Select Folder"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dbDictionaryBrowser As ExpTreeLib.ExpTree
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuRefreshView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lv1 As ListView_DoubleBuffered
    Friend WithEvents chName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents chLastModified As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTypeStr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chAttributes As System.Windows.Forms.ColumnHeader
    Friend WithEvents chCreated As System.Windows.Forms.ColumnHeader
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tsslLeft As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsslMiddle As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsslRight As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnBrowseFolder As System.Windows.Forms.Button
    Friend WithEvents mnuFolderHistory As System.Windows.Forms.ToolStripMenuItem
End Class
