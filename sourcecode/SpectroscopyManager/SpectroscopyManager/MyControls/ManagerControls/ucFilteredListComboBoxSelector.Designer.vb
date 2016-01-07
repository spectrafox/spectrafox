<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucFilteredListComboBoxSelector
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
        Me.components = New System.ComponentModel.Container()
        Me.cbEntry = New System.Windows.Forms.ComboBox()
        Me.lbEntry = New System.Windows.Forms.ListBox()
        Me.pbContextMenu = New System.Windows.Forms.PictureBox()
        Me.cmnuFilter = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuFilterTitle = New System.Windows.Forms.ToolStripLabel()
        Me.cmnuFilterText = New System.Windows.Forms.ToolStripTextBox()
        Me.cmnuFilterAddToHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuLastUsedFilters = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuFilterClear = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pbContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmnuFilter.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbEntry
        '
        Me.cbEntry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbEntry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEntry.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cbEntry.FormattingEnabled = True
        Me.cbEntry.Location = New System.Drawing.Point(0, 0)
        Me.cbEntry.Name = "cbEntry"
        Me.cbEntry.Size = New System.Drawing.Size(175, 21)
        Me.cbEntry.TabIndex = 0
        '
        'lbEntry
        '
        Me.lbEntry.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbEntry.FormattingEnabled = True
        Me.lbEntry.Location = New System.Drawing.Point(0, 0)
        Me.lbEntry.Name = "lbEntry"
        Me.lbEntry.Size = New System.Drawing.Size(175, 17)
        Me.lbEntry.TabIndex = 1
        '
        'pbContextMenu
        '
        Me.pbContextMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbContextMenu.Image = Global.SpectroscopyManager.My.Resources.Resources.filter_16
        Me.pbContextMenu.Location = New System.Drawing.Point(178, 3)
        Me.pbContextMenu.Name = "pbContextMenu"
        Me.pbContextMenu.Size = New System.Drawing.Size(16, 16)
        Me.pbContextMenu.TabIndex = 2
        Me.pbContextMenu.TabStop = False
        '
        'cmnuFilter
        '
        Me.cmnuFilter.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuFilterTitle, Me.cmnuFilterText, Me.cmnuFilterAddToHistory, Me.cmnuLastUsedFilters, Me.cmnuFilterClear})
        Me.cmnuFilter.Name = "cmnuFilter"
        Me.cmnuFilter.Size = New System.Drawing.Size(295, 113)
        '
        'cmnuFilterTitle
        '
        Me.cmnuFilterTitle.Name = "cmnuFilterTitle"
        Me.cmnuFilterTitle.Size = New System.Drawing.Size(234, 15)
        Me.cmnuFilterTitle.Text = "Filter column names: (use * as placeholder)"
        '
        'cmnuFilterText
        '
        Me.cmnuFilterText.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cmnuFilterText.Name = "cmnuFilterText"
        Me.cmnuFilterText.Size = New System.Drawing.Size(200, 23)
        '
        'cmnuFilterAddToHistory
        '
        Me.cmnuFilterAddToHistory.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.cmnuFilterAddToHistory.Name = "cmnuFilterAddToHistory"
        Me.cmnuFilterAddToHistory.Size = New System.Drawing.Size(294, 22)
        Me.cmnuFilterAddToHistory.Text = "add current filter to history"
        '
        'cmnuLastUsedFilters
        '
        Me.cmnuLastUsedFilters.Image = Global.SpectroscopyManager.My.Resources.Resources.filter_16
        Me.cmnuLastUsedFilters.Name = "cmnuLastUsedFilters"
        Me.cmnuLastUsedFilters.Size = New System.Drawing.Size(294, 22)
        Me.cmnuLastUsedFilters.Text = "last used filters ..."
        '
        'cmnuFilterClear
        '
        Me.cmnuFilterClear.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.cmnuFilterClear.Name = "cmnuFilterClear"
        Me.cmnuFilterClear.Size = New System.Drawing.Size(294, 22)
        Me.cmnuFilterClear.Text = "clear the filter"
        '
        'ucFilteredListComboBoxSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pbContextMenu)
        Me.Controls.Add(Me.lbEntry)
        Me.Controls.Add(Me.cbEntry)
        Me.MinimumSize = New System.Drawing.Size(0, 21)
        Me.Name = "ucFilteredListComboBoxSelector"
        Me.Size = New System.Drawing.Size(196, 21)
        CType(Me.pbContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmnuFilter.ResumeLayout(False)
        Me.cmnuFilter.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbEntry As System.Windows.Forms.ComboBox
    Friend WithEvents lbEntry As System.Windows.Forms.ListBox
    Friend WithEvents pbContextMenu As System.Windows.Forms.PictureBox
    Friend WithEvents cmnuFilter As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuFilterText As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents cmnuFilterTitle As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cmnuFilterClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuLastUsedFilters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuFilterAddToHistory As System.Windows.Forms.ToolStripMenuItem

End Class
