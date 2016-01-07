<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mScanImageList
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvScanImageFileList = New System.Windows.Forms.DataGridView()
        Me.colPreview = New System.Windows.Forms.DataGridViewImageColumn()
        Me.colFullFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colChannels = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colComment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cmFileList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuOpenExportWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuPreviewImageHeading = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCBPreviewImageChannel = New System.Windows.Forms.ToolStripComboBox()
        Me.cmnuPreviewImageDisplay = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.dgvScanImageFileList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmFileList.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvScanImageFileList
        '
        Me.dgvScanImageFileList.AllowUserToAddRows = False
        Me.dgvScanImageFileList.AllowUserToDeleteRows = False
        Me.dgvScanImageFileList.AllowUserToOrderColumns = True
        Me.dgvScanImageFileList.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow
        Me.dgvScanImageFileList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvScanImageFileList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvScanImageFileList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPreview, Me.colFullFileName, Me.colFileName, Me.colChannels, Me.colComment, Me.colSize, Me.colDate})
        Me.dgvScanImageFileList.ContextMenuStrip = Me.cmFileList
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvScanImageFileList.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvScanImageFileList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvScanImageFileList.Location = New System.Drawing.Point(0, 0)
        Me.dgvScanImageFileList.MultiSelect = False
        Me.dgvScanImageFileList.Name = "dgvScanImageFileList"
        Me.dgvScanImageFileList.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvScanImageFileList.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvScanImageFileList.RowHeadersVisible = False
        Me.dgvScanImageFileList.RowTemplate.Height = 150
        Me.dgvScanImageFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScanImageFileList.Size = New System.Drawing.Size(530, 470)
        Me.dgvScanImageFileList.TabIndex = 11
        Me.dgvScanImageFileList.VirtualMode = True
        '
        'colPreview
        '
        Me.colPreview.Frozen = True
        Me.colPreview.HeaderText = "Preview Image"
        Me.colPreview.Name = "colPreview"
        Me.colPreview.ReadOnly = True
        Me.colPreview.Width = 150
        '
        'colFullFileName
        '
        Me.colFullFileName.HeaderText = "FullFileName"
        Me.colFullFileName.Name = "colFullFileName"
        Me.colFullFileName.ReadOnly = True
        Me.colFullFileName.Visible = False
        '
        'colFileName
        '
        Me.colFileName.HeaderText = "Filename"
        Me.colFileName.Name = "colFileName"
        Me.colFileName.ReadOnly = True
        Me.colFileName.Width = 150
        '
        'colChannels
        '
        Me.colChannels.HeaderText = "Channels"
        Me.colChannels.Name = "colChannels"
        Me.colChannels.ReadOnly = True
        Me.colChannels.Width = 150
        '
        'colComment
        '
        Me.colComment.HeaderText = "Comment"
        Me.colComment.Name = "colComment"
        Me.colComment.ReadOnly = True
        Me.colComment.Width = 200
        '
        'colSize
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.colSize.DefaultCellStyle = DataGridViewCellStyle3
        Me.colSize.HeaderText = "Size"
        Me.colSize.Name = "colSize"
        Me.colSize.ReadOnly = True
        Me.colSize.Width = 140
        '
        'colDate
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.Format = "G"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.colDate.DefaultCellStyle = DataGridViewCellStyle4
        Me.colDate.HeaderText = "Date"
        Me.colDate.Name = "colDate"
        Me.colDate.ReadOnly = True
        Me.colDate.Width = 70
        '
        'cmFileList
        '
        Me.cmFileList.Enabled = False
        Me.cmFileList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuOpenExportWizard, Me.ToolStripSeparator1, Me.cmnuPreviewImageHeading, Me.cmnuCBPreviewImageChannel, Me.cmnuPreviewImageDisplay})
        Me.cmFileList.Name = "dgvContextMenu"
        Me.cmFileList.ShowImageMargin = False
        Me.cmFileList.Size = New System.Drawing.Size(236, 125)
        '
        'cmnuOpenExportWizard
        '
        Me.cmnuOpenExportWizard.Enabled = False
        Me.cmnuOpenExportWizard.Name = "cmnuOpenExportWizard"
        Me.cmnuOpenExportWizard.Size = New System.Drawing.Size(235, 22)
        Me.cmnuOpenExportWizard.Text = "Open Export Wizard"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(232, 6)
        '
        'cmnuPreviewImageHeading
        '
        Me.cmnuPreviewImageHeading.Enabled = False
        Me.cmnuPreviewImageHeading.Name = "cmnuPreviewImageHeading"
        Me.cmnuPreviewImageHeading.Size = New System.Drawing.Size(235, 22)
        Me.cmnuPreviewImageHeading.Text = "Preview-Image Settings"
        '
        'cmnuCBPreviewImageChannel
        '
        Me.cmnuCBPreviewImageChannel.BackColor = System.Drawing.Color.Gainsboro
        Me.cmnuCBPreviewImageChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmnuCBPreviewImageChannel.Enabled = False
        Me.cmnuCBPreviewImageChannel.Name = "cmnuCBPreviewImageChannel"
        Me.cmnuCBPreviewImageChannel.Size = New System.Drawing.Size(200, 23)
        Me.cmnuCBPreviewImageChannel.ToolTipText = "X Column of the Preview Images"
        '
        'cmnuPreviewImageDisplay
        '
        Me.cmnuPreviewImageDisplay.Enabled = False
        Me.cmnuPreviewImageDisplay.Name = "cmnuPreviewImageDisplay"
        Me.cmnuPreviewImageDisplay.Size = New System.Drawing.Size(235, 22)
        Me.cmnuPreviewImageDisplay.Text = "Display Options"
        '
        'mScanImageList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgvScanImageFileList)
        Me.Name = "mScanImageList"
        Me.Size = New System.Drawing.Size(530, 470)
        CType(Me.dgvScanImageFileList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmFileList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvScanImageFileList As System.Windows.Forms.DataGridView
    Friend WithEvents cmFileList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuOpenExportWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuPreviewImageHeading As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCBPreviewImageChannel As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents cmnuPreviewImageDisplay As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents colPreview As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents colFullFileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colChannels As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colComment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSize As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDate As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
