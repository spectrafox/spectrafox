<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mLineScanViewer
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pbLineScan = New System.Windows.Forms.PictureBox()
        Me.cmnuImageContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuCopyImageToClipboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSaveAsImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuSaveAsWSXM = New System.Windows.Forms.ToolStripMenuItem()
        Me.lbCommonColumnsX = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lbCommonColumnsZ = New System.Windows.Forms.ListBox()
        Me.dgvFileSortList = New System.Windows.Forms.DataGridView()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnSortFiles_OneUp = New System.Windows.Forms.Button()
        Me.btnSortFiles_OneDown = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cpColorPicker = New SpectroscopyManager.ucColorPalettePicker()
        Me.btnSortFiles_Top = New System.Windows.Forms.Button()
        Me.btnSortFiles_Bottom = New System.Windows.Forms.Button()
        Me.vsValueRangeSelector = New SpectroscopyManager.mValueRangeSelector()
        Me.btnApplySorting = New System.Windows.Forms.Button()
        Me.pgProgress = New System.Windows.Forms.ProgressBar()
        Me.pbLoading = New System.Windows.Forms.PictureBox()
        CType(Me.pbLineScan, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmnuImageContextMenuStrip.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgvFileSortList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.pbLoading, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbLineScan
        '
        Me.pbLineScan.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbLineScan.ContextMenuStrip = Me.cmnuImageContextMenuStrip
        Me.pbLineScan.Location = New System.Drawing.Point(3, 3)
        Me.pbLineScan.Name = "pbLineScan"
        Me.pbLineScan.Size = New System.Drawing.Size(345, 709)
        Me.pbLineScan.TabIndex = 0
        Me.pbLineScan.TabStop = False
        '
        'cmnuImageContextMenuStrip
        '
        Me.cmnuImageContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuCopyImageToClipboard, Me.cmnuSaveAsImage, Me.ToolStripSeparator1, Me.cmnuSaveAsWSXM})
        Me.cmnuImageContextMenuStrip.Name = "ImageContextMenuStrip"
        Me.cmnuImageContextMenuStrip.Size = New System.Drawing.Size(208, 76)
        '
        'cmnuCopyImageToClipboard
        '
        Me.cmnuCopyImageToClipboard.Name = "cmnuCopyImageToClipboard"
        Me.cmnuCopyImageToClipboard.Size = New System.Drawing.Size(207, 22)
        Me.cmnuCopyImageToClipboard.Text = "Copy Image to Clipboard"
        '
        'cmnuSaveAsImage
        '
        Me.cmnuSaveAsImage.Name = "cmnuSaveAsImage"
        Me.cmnuSaveAsImage.Size = New System.Drawing.Size(207, 22)
        Me.cmnuSaveAsImage.Text = "Save Image to File"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(204, 6)
        '
        'cmnuSaveAsWSXM
        '
        Me.cmnuSaveAsWSXM.Name = "cmnuSaveAsWSXM"
        Me.cmnuSaveAsWSXM.Size = New System.Drawing.Size(207, 22)
        Me.cmnuSaveAsWSXM.Text = "Save as WSxM-File"
        '
        'lbCommonColumnsX
        '
        Me.lbCommonColumnsX.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbCommonColumnsX.FormattingEnabled = True
        Me.lbCommonColumnsX.Location = New System.Drawing.Point(3, 16)
        Me.lbCommonColumnsX.Name = "lbCommonColumnsX"
        Me.lbCommonColumnsX.Size = New System.Drawing.Size(157, 79)
        Me.lbCommonColumnsX.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lbCommonColumnsX)
        Me.GroupBox1.Location = New System.Drawing.Point(352, 273)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(163, 98)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "X Column:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.lbCommonColumnsZ)
        Me.GroupBox2.Location = New System.Drawing.Point(352, 374)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(163, 99)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Value Column:"
        '
        'lbCommonColumnsZ
        '
        Me.lbCommonColumnsZ.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbCommonColumnsZ.FormattingEnabled = True
        Me.lbCommonColumnsZ.Location = New System.Drawing.Point(3, 16)
        Me.lbCommonColumnsZ.Name = "lbCommonColumnsZ"
        Me.lbCommonColumnsZ.Size = New System.Drawing.Size(157, 80)
        Me.lbCommonColumnsZ.TabIndex = 3
        '
        'dgvFileSortList
        '
        Me.dgvFileSortList.AllowUserToAddRows = False
        Me.dgvFileSortList.AllowUserToDeleteRows = False
        Me.dgvFileSortList.AllowUserToResizeRows = False
        Me.dgvFileSortList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFileSortList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvFileSortList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFileSortList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvFileSortList.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvFileSortList.Location = New System.Drawing.Point(352, 538)
        Me.dgvFileSortList.MultiSelect = False
        Me.dgvFileSortList.Name = "dgvFileSortList"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFileSortList.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvFileSortList.RowHeadersVisible = False
        Me.dgvFileSortList.RowTemplate.Height = 18
        Me.dgvFileSortList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFileSortList.Size = New System.Drawing.Size(143, 174)
        Me.dgvFileSortList.TabIndex = 6
        '
        'colName
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.colName.DefaultCellStyle = DataGridViewCellStyle2
        Me.colName.HeaderText = "Filename"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.Width = 130
        '
        'btnSortFiles_OneUp
        '
        Me.btnSortFiles_OneUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortFiles_OneUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSortFiles_OneUp.Location = New System.Drawing.Point(498, 538)
        Me.btnSortFiles_OneUp.Name = "btnSortFiles_OneUp"
        Me.btnSortFiles_OneUp.Size = New System.Drawing.Size(20, 23)
        Me.btnSortFiles_OneUp.TabIndex = 7
        Me.btnSortFiles_OneUp.Text = "u"
        Me.btnSortFiles_OneUp.UseVisualStyleBackColor = True
        '
        'btnSortFiles_OneDown
        '
        Me.btnSortFiles_OneDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortFiles_OneDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSortFiles_OneDown.Location = New System.Drawing.Point(498, 563)
        Me.btnSortFiles_OneDown.Name = "btnSortFiles_OneDown"
        Me.btnSortFiles_OneDown.Size = New System.Drawing.Size(20, 23)
        Me.btnSortFiles_OneDown.TabIndex = 7
        Me.btnSortFiles_OneDown.Text = "d"
        Me.btnSortFiles_OneDown.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.cpColorPicker)
        Me.GroupBox3.Location = New System.Drawing.Point(352, 476)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(160, 56)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Settings:"
        '
        'cpColorPicker
        '
        Me.cpColorPicker.Location = New System.Drawing.Point(7, 20)
        Me.cpColorPicker.Name = "cpColorPicker"
        Me.cpColorPicker.Size = New System.Drawing.Size(147, 23)
        Me.cpColorPicker.TabIndex = 0
        '
        'btnSortFiles_Top
        '
        Me.btnSortFiles_Top.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortFiles_Top.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSortFiles_Top.Location = New System.Drawing.Point(498, 589)
        Me.btnSortFiles_Top.Name = "btnSortFiles_Top"
        Me.btnSortFiles_Top.Size = New System.Drawing.Size(20, 23)
        Me.btnSortFiles_Top.TabIndex = 7
        Me.btnSortFiles_Top.Text = "t"
        Me.btnSortFiles_Top.UseVisualStyleBackColor = True
        '
        'btnSortFiles_Bottom
        '
        Me.btnSortFiles_Bottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortFiles_Bottom.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSortFiles_Bottom.Location = New System.Drawing.Point(498, 614)
        Me.btnSortFiles_Bottom.Name = "btnSortFiles_Bottom"
        Me.btnSortFiles_Bottom.Size = New System.Drawing.Size(20, 23)
        Me.btnSortFiles_Bottom.TabIndex = 7
        Me.btnSortFiles_Bottom.Text = "b"
        Me.btnSortFiles_Bottom.UseVisualStyleBackColor = True
        '
        'vsValueRangeSelector
        '
        Me.vsValueRangeSelector.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsValueRangeSelector.Location = New System.Drawing.Point(355, 3)
        Me.vsValueRangeSelector.Name = "vsValueRangeSelector"
        Me.vsValueRangeSelector.Size = New System.Drawing.Size(163, 264)
        Me.vsValueRangeSelector.TabIndex = 5
        '
        'btnApplySorting
        '
        Me.btnApplySorting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplySorting.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnApplySorting.Location = New System.Drawing.Point(498, 643)
        Me.btnApplySorting.Name = "btnApplySorting"
        Me.btnApplySorting.Size = New System.Drawing.Size(20, 72)
        Me.btnApplySorting.TabIndex = 7
        Me.btnApplySorting.Text = "ok"
        Me.btnApplySorting.UseVisualStyleBackColor = True
        '
        'pgProgress
        '
        Me.pgProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgProgress.Location = New System.Drawing.Point(73, 35)
        Me.pgProgress.Name = "pgProgress"
        Me.pgProgress.Size = New System.Drawing.Size(195, 14)
        Me.pgProgress.TabIndex = 9
        Me.pgProgress.Value = 90
        Me.pgProgress.Visible = False
        '
        'pbLoading
        '
        Me.pbLoading.BackColor = System.Drawing.Color.Transparent
        Me.pbLoading.Image = Global.SpectroscopyManager.My.Resources.Resources.loading_25
        Me.pbLoading.Location = New System.Drawing.Point(3, 3)
        Me.pbLoading.Name = "pbLoading"
        Me.pbLoading.Size = New System.Drawing.Size(29, 29)
        Me.pbLoading.TabIndex = 14
        Me.pbLoading.TabStop = False
        '
        'mLineScanViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pgProgress)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnSortFiles_Bottom)
        Me.Controls.Add(Me.btnApplySorting)
        Me.Controls.Add(Me.btnSortFiles_Top)
        Me.Controls.Add(Me.btnSortFiles_OneDown)
        Me.Controls.Add(Me.btnSortFiles_OneUp)
        Me.Controls.Add(Me.dgvFileSortList)
        Me.Controls.Add(Me.vsValueRangeSelector)
        Me.Controls.Add(Me.pbLineScan)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.pbLoading)
        Me.Name = "mLineScanViewer"
        Me.Size = New System.Drawing.Size(521, 715)
        CType(Me.pbLineScan, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmnuImageContextMenuStrip.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.dgvFileSortList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.pbLoading, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbLineScan As System.Windows.Forms.PictureBox
    Friend WithEvents lbCommonColumnsX As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lbCommonColumnsZ As System.Windows.Forms.ListBox
    Friend WithEvents vsValueRangeSelector As SpectroscopyManager.mValueRangeSelector
    Friend WithEvents dgvFileSortList As System.Windows.Forms.DataGridView
    Friend WithEvents btnSortFiles_OneUp As System.Windows.Forms.Button
    Friend WithEvents btnSortFiles_OneDown As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cpColorPicker As SpectroscopyManager.ucColorPalettePicker
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnSortFiles_Top As System.Windows.Forms.Button
    Friend WithEvents btnSortFiles_Bottom As System.Windows.Forms.Button
    Friend WithEvents cmnuImageContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuCopyImageToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSaveAsImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuSaveAsWSXM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnApplySorting As System.Windows.Forms.Button
    Friend WithEvents pgProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents pbLoading As System.Windows.Forms.PictureBox

End Class
