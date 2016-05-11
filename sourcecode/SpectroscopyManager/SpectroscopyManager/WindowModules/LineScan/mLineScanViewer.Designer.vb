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
        Me.gbXValues = New System.Windows.Forms.GroupBox()
        Me.lblMaxXRange = New System.Windows.Forms.Label()
        Me.lblMinXRange = New System.Windows.Forms.Label()
        Me.txtMaxXRange = New SpectroscopyManager.NumericTextbox()
        Me.txtMinXRange = New SpectroscopyManager.NumericTextbox()
        Me.gbZValues = New System.Windows.Forms.GroupBox()
        Me.ckbLogZ = New System.Windows.Forms.CheckBox()
        Me.lbCommonColumnsZ = New System.Windows.Forms.ListBox()
        Me.dgvFileSortList = New System.Windows.Forms.DataGridView()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnSortFiles_OneUp = New System.Windows.Forms.Button()
        Me.btnSortFiles_OneDown = New System.Windows.Forms.Button()
        Me.gbSettings = New System.Windows.Forms.GroupBox()
        Me.cpColorPicker = New SpectroscopyManager.ucColorPalettePicker()
        Me.btnSortFiles_Top = New System.Windows.Forms.Button()
        Me.btnSortFiles_Bottom = New System.Windows.Forms.Button()
        Me.btnApplySorting = New System.Windows.Forms.Button()
        Me.pbLoading = New System.Windows.Forms.PictureBox()
        Me.vsValueRangeSelector = New SpectroscopyManager.mValueRangeSelector()
        Me.gbSpectraSorting = New System.Windows.Forms.GroupBox()
        Me.scColumnSelector = New System.Windows.Forms.SplitContainer()
        CType(Me.pbLineScan, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmnuImageContextMenuStrip.SuspendLayout()
        Me.gbXValues.SuspendLayout()
        Me.gbZValues.SuspendLayout()
        CType(Me.dgvFileSortList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSettings.SuspendLayout()
        CType(Me.pbLoading, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSpectraSorting.SuspendLayout()
        CType(Me.scColumnSelector, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scColumnSelector.Panel1.SuspendLayout()
        Me.scColumnSelector.Panel2.SuspendLayout()
        Me.scColumnSelector.SuspendLayout()
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
        Me.pbLineScan.Size = New System.Drawing.Size(761, 700)
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
        Me.lbCommonColumnsX.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbCommonColumnsX.FormattingEnabled = True
        Me.lbCommonColumnsX.Location = New System.Drawing.Point(3, 16)
        Me.lbCommonColumnsX.Name = "lbCommonColumnsX"
        Me.lbCommonColumnsX.Size = New System.Drawing.Size(157, 121)
        Me.lbCommonColumnsX.TabIndex = 3
        '
        'gbXValues
        '
        Me.gbXValues.Controls.Add(Me.lblMaxXRange)
        Me.gbXValues.Controls.Add(Me.lblMinXRange)
        Me.gbXValues.Controls.Add(Me.txtMaxXRange)
        Me.gbXValues.Controls.Add(Me.txtMinXRange)
        Me.gbXValues.Controls.Add(Me.lbCommonColumnsX)
        Me.gbXValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbXValues.Location = New System.Drawing.Point(0, 0)
        Me.gbXValues.Name = "gbXValues"
        Me.gbXValues.Size = New System.Drawing.Size(163, 194)
        Me.gbXValues.TabIndex = 4
        Me.gbXValues.TabStop = False
        Me.gbXValues.Text = "x-values:"
        '
        'lblMaxXRange
        '
        Me.lblMaxXRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMaxXRange.AutoSize = True
        Me.lblMaxXRange.Location = New System.Drawing.Point(6, 172)
        Me.lblMaxXRange.Name = "lblMaxXRange"
        Me.lblMaxXRange.Size = New System.Drawing.Size(42, 13)
        Me.lblMaxXRange.TabIndex = 5
        Me.lblMaxXRange.Text = "max. X:"
        '
        'lblMinXRange
        '
        Me.lblMinXRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMinXRange.AutoSize = True
        Me.lblMinXRange.Location = New System.Drawing.Point(6, 146)
        Me.lblMinXRange.Name = "lblMinXRange"
        Me.lblMinXRange.Size = New System.Drawing.Size(39, 13)
        Me.lblMinXRange.TabIndex = 5
        Me.lblMinXRange.Text = "min. X:"
        '
        'txtMaxXRange
        '
        Me.txtMaxXRange.AllowZero = True
        Me.txtMaxXRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMaxXRange.BackColor = System.Drawing.Color.White
        Me.txtMaxXRange.ForeColor = System.Drawing.Color.Black
        Me.txtMaxXRange.FormatDecimalPlaces = 6
        Me.txtMaxXRange.Location = New System.Drawing.Point(57, 169)
        Me.txtMaxXRange.Name = "txtMaxXRange"
        Me.txtMaxXRange.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtMaxXRange.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtMaxXRange.Size = New System.Drawing.Size(100, 20)
        Me.txtMaxXRange.TabIndex = 4
        Me.txtMaxXRange.Text = "0.000000"
        Me.txtMaxXRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMaxXRange.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtMinXRange
        '
        Me.txtMinXRange.AllowZero = True
        Me.txtMinXRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMinXRange.BackColor = System.Drawing.Color.White
        Me.txtMinXRange.ForeColor = System.Drawing.Color.Black
        Me.txtMinXRange.FormatDecimalPlaces = 6
        Me.txtMinXRange.Location = New System.Drawing.Point(57, 143)
        Me.txtMinXRange.Name = "txtMinXRange"
        Me.txtMinXRange.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtMinXRange.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtMinXRange.Size = New System.Drawing.Size(100, 20)
        Me.txtMinXRange.TabIndex = 4
        Me.txtMinXRange.Text = "0.000000"
        Me.txtMinXRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMinXRange.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'gbZValues
        '
        Me.gbZValues.Controls.Add(Me.ckbLogZ)
        Me.gbZValues.Controls.Add(Me.lbCommonColumnsZ)
        Me.gbZValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbZValues.Location = New System.Drawing.Point(0, 0)
        Me.gbZValues.Name = "gbZValues"
        Me.gbZValues.Size = New System.Drawing.Size(163, 190)
        Me.gbZValues.TabIndex = 4
        Me.gbZValues.TabStop = False
        Me.gbZValues.Text = "z-values:"
        '
        'ckbLogZ
        '
        Me.ckbLogZ.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ckbLogZ.AutoSize = True
        Me.ckbLogZ.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ckbLogZ.Location = New System.Drawing.Point(6, 167)
        Me.ckbLogZ.Name = "ckbLogZ"
        Me.ckbLogZ.Size = New System.Drawing.Size(86, 17)
        Me.ckbLogZ.TabIndex = 4
        Me.ckbLogZ.Text = "logarithmic Z"
        Me.ckbLogZ.UseVisualStyleBackColor = True
        '
        'lbCommonColumnsZ
        '
        Me.lbCommonColumnsZ.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbCommonColumnsZ.FormattingEnabled = True
        Me.lbCommonColumnsZ.Location = New System.Drawing.Point(3, 16)
        Me.lbCommonColumnsZ.Name = "lbCommonColumnsZ"
        Me.lbCommonColumnsZ.Size = New System.Drawing.Size(157, 147)
        Me.lbCommonColumnsZ.TabIndex = 3
        '
        'dgvFileSortList
        '
        Me.dgvFileSortList.AllowUserToAddRows = False
        Me.dgvFileSortList.AllowUserToDeleteRows = False
        Me.dgvFileSortList.AllowUserToResizeColumns = False
        Me.dgvFileSortList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFileSortList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
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
        Me.dgvFileSortList.Location = New System.Drawing.Point(7, 18)
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
        Me.dgvFileSortList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.dgvFileSortList.RowTemplate.Height = 18
        Me.dgvFileSortList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFileSortList.Size = New System.Drawing.Size(154, 597)
        Me.dgvFileSortList.TabIndex = 6
        '
        'colName
        '
        Me.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.colName.DefaultCellStyle = DataGridViewCellStyle2
        Me.colName.HeaderText = "filename"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.Width = 65
        '
        'btnSortFiles_OneUp
        '
        Me.btnSortFiles_OneUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortFiles_OneUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSortFiles_OneUp.Location = New System.Drawing.Point(167, 18)
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
        Me.btnSortFiles_OneDown.Location = New System.Drawing.Point(167, 43)
        Me.btnSortFiles_OneDown.Name = "btnSortFiles_OneDown"
        Me.btnSortFiles_OneDown.Size = New System.Drawing.Size(20, 23)
        Me.btnSortFiles_OneDown.TabIndex = 7
        Me.btnSortFiles_OneDown.Text = "d"
        Me.btnSortFiles_OneDown.UseVisualStyleBackColor = True
        '
        'gbSettings
        '
        Me.gbSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSettings.Controls.Add(Me.cpColorPicker)
        Me.gbSettings.Location = New System.Drawing.Point(939, 3)
        Me.gbSettings.Name = "gbSettings"
        Me.gbSettings.Size = New System.Drawing.Size(193, 72)
        Me.gbSettings.TabIndex = 8
        Me.gbSettings.TabStop = False
        Me.gbSettings.Text = "color-scheme:"
        '
        'cpColorPicker
        '
        Me.cpColorPicker.Location = New System.Drawing.Point(7, 20)
        Me.cpColorPicker.Name = "cpColorPicker"
        Me.cpColorPicker.Size = New System.Drawing.Size(180, 46)
        Me.cpColorPicker.TabIndex = 0
        '
        'btnSortFiles_Top
        '
        Me.btnSortFiles_Top.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortFiles_Top.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSortFiles_Top.Location = New System.Drawing.Point(167, 69)
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
        Me.btnSortFiles_Bottom.Location = New System.Drawing.Point(167, 94)
        Me.btnSortFiles_Bottom.Name = "btnSortFiles_Bottom"
        Me.btnSortFiles_Bottom.Size = New System.Drawing.Size(20, 23)
        Me.btnSortFiles_Bottom.TabIndex = 7
        Me.btnSortFiles_Bottom.Text = "b"
        Me.btnSortFiles_Bottom.UseVisualStyleBackColor = True
        '
        'btnApplySorting
        '
        Me.btnApplySorting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplySorting.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnApplySorting.Location = New System.Drawing.Point(167, 123)
        Me.btnApplySorting.Name = "btnApplySorting"
        Me.btnApplySorting.Size = New System.Drawing.Size(20, 72)
        Me.btnApplySorting.TabIndex = 7
        Me.btnApplySorting.Text = "ok"
        Me.btnApplySorting.UseVisualStyleBackColor = True
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
        'vsValueRangeSelector
        '
        Me.vsValueRangeSelector.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsValueRangeSelector.Location = New System.Drawing.Point(770, 3)
        Me.vsValueRangeSelector.Name = "vsValueRangeSelector"
        Me.vsValueRangeSelector.SelectedMaxValue = 0R
        Me.vsValueRangeSelector.SelectedMinValue = 0R
        Me.vsValueRangeSelector.Size = New System.Drawing.Size(163, 309)
        Me.vsValueRangeSelector.TabIndex = 5
        '
        'gbSpectraSorting
        '
        Me.gbSpectraSorting.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSpectraSorting.Controls.Add(Me.dgvFileSortList)
        Me.gbSpectraSorting.Controls.Add(Me.btnSortFiles_OneUp)
        Me.gbSpectraSorting.Controls.Add(Me.btnSortFiles_Bottom)
        Me.gbSpectraSorting.Controls.Add(Me.btnSortFiles_OneDown)
        Me.gbSpectraSorting.Controls.Add(Me.btnApplySorting)
        Me.gbSpectraSorting.Controls.Add(Me.btnSortFiles_Top)
        Me.gbSpectraSorting.Location = New System.Drawing.Point(939, 82)
        Me.gbSpectraSorting.Name = "gbSpectraSorting"
        Me.gbSpectraSorting.Size = New System.Drawing.Size(193, 621)
        Me.gbSpectraSorting.TabIndex = 15
        Me.gbSpectraSorting.TabStop = False
        Me.gbSpectraSorting.Text = "spectra sorting"
        '
        'scColumnSelector
        '
        Me.scColumnSelector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scColumnSelector.Location = New System.Drawing.Point(770, 315)
        Me.scColumnSelector.Name = "scColumnSelector"
        Me.scColumnSelector.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scColumnSelector.Panel1
        '
        Me.scColumnSelector.Panel1.Controls.Add(Me.gbXValues)
        '
        'scColumnSelector.Panel2
        '
        Me.scColumnSelector.Panel2.Controls.Add(Me.gbZValues)
        Me.scColumnSelector.Size = New System.Drawing.Size(163, 388)
        Me.scColumnSelector.SplitterDistance = 194
        Me.scColumnSelector.TabIndex = 16
        '
        'mLineScanViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbSettings)
        Me.Controls.Add(Me.vsValueRangeSelector)
        Me.Controls.Add(Me.pbLineScan)
        Me.Controls.Add(Me.pbLoading)
        Me.Controls.Add(Me.gbSpectraSorting)
        Me.Controls.Add(Me.scColumnSelector)
        Me.Name = "mLineScanViewer"
        Me.Size = New System.Drawing.Size(1135, 706)
        CType(Me.pbLineScan, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmnuImageContextMenuStrip.ResumeLayout(False)
        Me.gbXValues.ResumeLayout(False)
        Me.gbXValues.PerformLayout()
        Me.gbZValues.ResumeLayout(False)
        Me.gbZValues.PerformLayout()
        CType(Me.dgvFileSortList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSettings.ResumeLayout(False)
        CType(Me.pbLoading, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSpectraSorting.ResumeLayout(False)
        Me.scColumnSelector.Panel1.ResumeLayout(False)
        Me.scColumnSelector.Panel2.ResumeLayout(False)
        CType(Me.scColumnSelector, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scColumnSelector.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbLineScan As System.Windows.Forms.PictureBox
    Friend WithEvents lbCommonColumnsX As System.Windows.Forms.ListBox
    Friend WithEvents gbXValues As System.Windows.Forms.GroupBox
    Friend WithEvents gbZValues As System.Windows.Forms.GroupBox
    Friend WithEvents lbCommonColumnsZ As System.Windows.Forms.ListBox
    Friend WithEvents vsValueRangeSelector As SpectroscopyManager.mValueRangeSelector
    Friend WithEvents dgvFileSortList As System.Windows.Forms.DataGridView
    Friend WithEvents btnSortFiles_OneUp As System.Windows.Forms.Button
    Friend WithEvents btnSortFiles_OneDown As System.Windows.Forms.Button
    Friend WithEvents gbSettings As System.Windows.Forms.GroupBox
    Friend WithEvents cpColorPicker As SpectroscopyManager.ucColorPalettePicker
    Friend WithEvents btnSortFiles_Top As System.Windows.Forms.Button
    Friend WithEvents btnSortFiles_Bottom As System.Windows.Forms.Button
    Friend WithEvents cmnuImageContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuCopyImageToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSaveAsImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuSaveAsWSXM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnApplySorting As System.Windows.Forms.Button
    Friend WithEvents pbLoading As System.Windows.Forms.PictureBox
    Friend WithEvents txtMaxXRange As NumericTextbox
    Friend WithEvents txtMinXRange As NumericTextbox
    Friend WithEvents gbSpectraSorting As GroupBox
    Friend WithEvents lblMaxXRange As Label
    Friend WithEvents lblMinXRange As Label
    Friend WithEvents ckbLogZ As CheckBox
    Friend WithEvents scColumnSelector As SplitContainer
    Friend WithEvents colName As DataGridViewTextBoxColumn
End Class
