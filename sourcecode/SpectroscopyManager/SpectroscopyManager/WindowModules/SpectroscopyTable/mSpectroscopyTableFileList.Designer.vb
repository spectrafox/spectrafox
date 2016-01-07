<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mSpectroscopyTableFileList
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
        Me.dgvSpectroscopyFileList = New System.Windows.Forms.DataGridView()
        Me.colPreviewImage = New System.Windows.Forms.DataGridViewImageColumn()
        Me.colFullFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colColumnList = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colComment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDataPoints = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cmFileList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuTestSingle = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuTestMultiple = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuShowNearestScanImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuDisplayTogether = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuOpenExportWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCopyDataToClipboardOriginCompatible = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCopyDataToClipboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuVisualization = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCreateContourPlotFromLinescans = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuDataManipulations = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuShiftColumnValues = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuCropData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCropDataUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuNumericalManipulations = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuAverageDataSingleFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuAverageDataMultipleFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuSmoothData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSmoothDataUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuNormalizeData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuNormalizeDataUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuRenormalizeData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuRenormalizeDataUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuInterpolateData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuFitNonLinear = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuPreviewImageHeading = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCBPreviewImageX = New System.Windows.Forms.ToolStripComboBox()
        Me.cmnuCBPreviewImageY = New System.Windows.Forms.ToolStripComboBox()
        Me.cmnuPreviewImageDisplay = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuPreviewImageLogarithmicX = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuPreviewImageLogarithmicY = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuPreviewImagePointReduction = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuCBPreviewImageSize = New System.Windows.Forms.ToolStripComboBox()
        CType(Me.dgvSpectroscopyFileList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmFileList.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvSpectroscopyFileList
        '
        Me.dgvSpectroscopyFileList.AllowUserToAddRows = False
        Me.dgvSpectroscopyFileList.AllowUserToDeleteRows = False
        Me.dgvSpectroscopyFileList.AllowUserToOrderColumns = True
        Me.dgvSpectroscopyFileList.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow
        Me.dgvSpectroscopyFileList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSpectroscopyFileList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvSpectroscopyFileList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPreviewImage, Me.colFullFileName, Me.colFileName, Me.colColumnList, Me.colComment, Me.colDataPoints, Me.colTime})
        Me.dgvSpectroscopyFileList.ContextMenuStrip = Me.cmFileList
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSpectroscopyFileList.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvSpectroscopyFileList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSpectroscopyFileList.Location = New System.Drawing.Point(0, 0)
        Me.dgvSpectroscopyFileList.Name = "dgvSpectroscopyFileList"
        Me.dgvSpectroscopyFileList.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvSpectroscopyFileList.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvSpectroscopyFileList.RowHeadersVisible = False
        Me.dgvSpectroscopyFileList.RowTemplate.Height = 125
        Me.dgvSpectroscopyFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSpectroscopyFileList.Size = New System.Drawing.Size(633, 487)
        Me.dgvSpectroscopyFileList.TabIndex = 10
        Me.dgvSpectroscopyFileList.VirtualMode = True
        '
        'colPreviewImage
        '
        Me.colPreviewImage.Frozen = True
        Me.colPreviewImage.HeaderText = "Preview Image"
        Me.colPreviewImage.Name = "colPreviewImage"
        Me.colPreviewImage.ReadOnly = True
        Me.colPreviewImage.Width = 83
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
        'colColumnList
        '
        Me.colColumnList.HeaderText = "Columns"
        Me.colColumnList.Name = "colColumnList"
        Me.colColumnList.ReadOnly = True
        Me.colColumnList.Width = 150
        '
        'colComment
        '
        Me.colComment.HeaderText = "Comment"
        Me.colComment.Name = "colComment"
        Me.colComment.ReadOnly = True
        Me.colComment.Width = 200
        '
        'colDataPoints
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.colDataPoints.DefaultCellStyle = DataGridViewCellStyle3
        Me.colDataPoints.HeaderText = "Points"
        Me.colDataPoints.Name = "colDataPoints"
        Me.colDataPoints.ReadOnly = True
        Me.colDataPoints.Width = 61
        '
        'colTime
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.Format = "G"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.colTime.DefaultCellStyle = DataGridViewCellStyle4
        Me.colTime.HeaderText = "Date"
        Me.colTime.Name = "colTime"
        Me.colTime.ReadOnly = True
        Me.colTime.Width = 70
        '
        'cmFileList
        '
        Me.cmFileList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuTestSingle, Me.cmnuTestMultiple, Me.cmnuShowNearestScanImage, Me.cmnuDisplayTogether, Me.ToolStripSeparator11, Me.cmnuOpenExportWizard, Me.cmnuCopyDataToClipboardOriginCompatible, Me.cmnuCopyDataToClipboard, Me.ToolStripSeparator4, Me.cmnuVisualization, Me.cmnuDataManipulations, Me.cmnuNumericalManipulations, Me.cmnuFitNonLinear, Me.ToolStripSeparator1, Me.cmnuPreviewImageHeading, Me.cmnuCBPreviewImageX, Me.cmnuCBPreviewImageY, Me.cmnuPreviewImageDisplay})
        Me.cmFileList.Name = "dgvContextMenu"
        Me.cmFileList.ShowImageMargin = False
        Me.cmFileList.Size = New System.Drawing.Size(283, 384)
        '
        'cmnuTestSingle
        '
        Me.cmnuTestSingle.Name = "cmnuTestSingle"
        Me.cmnuTestSingle.Size = New System.Drawing.Size(282, 22)
        Me.cmnuTestSingle.Text = "Test-Function - Single"
        '
        'cmnuTestMultiple
        '
        Me.cmnuTestMultiple.Name = "cmnuTestMultiple"
        Me.cmnuTestMultiple.Size = New System.Drawing.Size(282, 22)
        Me.cmnuTestMultiple.Text = "Test-Function - Multiple"
        '
        'cmnuShowNearestScanImage
        '
        Me.cmnuShowNearestScanImage.Enabled = False
        Me.cmnuShowNearestScanImage.Name = "cmnuShowNearestScanImage"
        Me.cmnuShowNearestScanImage.Size = New System.Drawing.Size(282, 22)
        Me.cmnuShowNearestScanImage.Text = "Show nearest Scan-Image"
        '
        'cmnuDisplayTogether
        '
        Me.cmnuDisplayTogether.Enabled = False
        Me.cmnuDisplayTogether.Name = "cmnuDisplayTogether"
        Me.cmnuDisplayTogether.Size = New System.Drawing.Size(282, 22)
        Me.cmnuDisplayTogether.Text = "Show together in preview window"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(279, 6)
        '
        'cmnuOpenExportWizard
        '
        Me.cmnuOpenExportWizard.Name = "cmnuOpenExportWizard"
        Me.cmnuOpenExportWizard.Size = New System.Drawing.Size(282, 22)
        Me.cmnuOpenExportWizard.Text = "Open Export Wizard"
        '
        'cmnuCopyDataToClipboardOriginCompatible
        '
        Me.cmnuCopyDataToClipboardOriginCompatible.Enabled = False
        Me.cmnuCopyDataToClipboardOriginCompatible.Name = "cmnuCopyDataToClipboardOriginCompatible"
        Me.cmnuCopyDataToClipboardOriginCompatible.Size = New System.Drawing.Size(282, 22)
        Me.cmnuCopyDataToClipboardOriginCompatible.Text = "Copy Data to Clipboard (Origin Compatible)"
        '
        'cmnuCopyDataToClipboard
        '
        Me.cmnuCopyDataToClipboard.Enabled = False
        Me.cmnuCopyDataToClipboard.Name = "cmnuCopyDataToClipboard"
        Me.cmnuCopyDataToClipboard.Size = New System.Drawing.Size(282, 22)
        Me.cmnuCopyDataToClipboard.Text = "Copy Data to Clipboard (English Format)"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(279, 6)
        '
        'cmnuVisualization
        '
        Me.cmnuVisualization.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuCreateContourPlotFromLinescans})
        Me.cmnuVisualization.Name = "cmnuVisualization"
        Me.cmnuVisualization.Size = New System.Drawing.Size(282, 22)
        Me.cmnuVisualization.Text = "Visualization"
        '
        'cmnuCreateContourPlotFromLinescans
        '
        Me.cmnuCreateContourPlotFromLinescans.Enabled = False
        Me.cmnuCreateContourPlotFromLinescans.Name = "cmnuCreateContourPlotFromLinescans"
        Me.cmnuCreateContourPlotFromLinescans.Size = New System.Drawing.Size(257, 22)
        Me.cmnuCreateContourPlotFromLinescans.Text = "Create Contour Plot from Linescan"
        '
        'cmnuDataManipulations
        '
        Me.cmnuDataManipulations.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuShiftColumnValues, Me.ToolStripSeparator8, Me.cmnuCropData, Me.cmnuCropDataUsingLastSettings})
        Me.cmnuDataManipulations.Name = "cmnuDataManipulations"
        Me.cmnuDataManipulations.Size = New System.Drawing.Size(282, 22)
        Me.cmnuDataManipulations.Text = "Data Manipulations"
        '
        'cmnuShiftColumnValues
        '
        Me.cmnuShiftColumnValues.Enabled = False
        Me.cmnuShiftColumnValues.Name = "cmnuShiftColumnValues"
        Me.cmnuShiftColumnValues.Size = New System.Drawing.Size(224, 22)
        Me.cmnuShiftColumnValues.Text = "Shift Column Values"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(221, 6)
        '
        'cmnuCropData
        '
        Me.cmnuCropData.Name = "cmnuCropData"
        Me.cmnuCropData.Size = New System.Drawing.Size(224, 22)
        Me.cmnuCropData.Text = "Crop Data from Table"
        '
        'cmnuCropDataUsingLastSettings
        '
        Me.cmnuCropDataUsingLastSettings.Name = "cmnuCropDataUsingLastSettings"
        Me.cmnuCropDataUsingLastSettings.Size = New System.Drawing.Size(224, 22)
        Me.cmnuCropDataUsingLastSettings.Text = "Crop Data using last settings"
        '
        'cmnuNumericalManipulations
        '
        Me.cmnuNumericalManipulations.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuAverageDataSingleFile, Me.cmnuAverageDataMultipleFiles, Me.ToolStripSeparator2, Me.cmnuSmoothData, Me.cmnuSmoothDataUsingLastSettings, Me.ToolStripSeparator5, Me.cmnuNormalizeData, Me.cmnuNormalizeDataUsingLastSettings, Me.ToolStripSeparator3, Me.cmnuRenormalizeData, Me.cmnuRenormalizeDataUsingLastSettings, Me.ToolStripSeparator6, Me.cmnuInterpolateData})
        Me.cmnuNumericalManipulations.Name = "cmnuNumericalManipulations"
        Me.cmnuNumericalManipulations.Size = New System.Drawing.Size(282, 22)
        Me.cmnuNumericalManipulations.Text = "Numerical Manipulations"
        '
        'cmnuAverageDataSingleFile
        '
        Me.cmnuAverageDataSingleFile.Enabled = False
        Me.cmnuAverageDataSingleFile.Name = "cmnuAverageDataSingleFile"
        Me.cmnuAverageDataSingleFile.Size = New System.Drawing.Size(252, 22)
        Me.cmnuAverageDataSingleFile.Text = "Average Data in a Single File"
        Me.cmnuAverageDataSingleFile.Visible = False
        '
        'cmnuAverageDataMultipleFiles
        '
        Me.cmnuAverageDataMultipleFiles.Enabled = False
        Me.cmnuAverageDataMultipleFiles.Name = "cmnuAverageDataMultipleFiles"
        Me.cmnuAverageDataMultipleFiles.Size = New System.Drawing.Size(252, 22)
        Me.cmnuAverageDataMultipleFiles.Text = "Average Data from Multiple Files"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(249, 6)
        '
        'cmnuSmoothData
        '
        Me.cmnuSmoothData.Enabled = False
        Me.cmnuSmoothData.Name = "cmnuSmoothData"
        Me.cmnuSmoothData.Size = New System.Drawing.Size(252, 22)
        Me.cmnuSmoothData.Text = "Smooth Data"
        '
        'cmnuSmoothDataUsingLastSettings
        '
        Me.cmnuSmoothDataUsingLastSettings.Enabled = False
        Me.cmnuSmoothDataUsingLastSettings.Name = "cmnuSmoothDataUsingLastSettings"
        Me.cmnuSmoothDataUsingLastSettings.Size = New System.Drawing.Size(252, 22)
        Me.cmnuSmoothDataUsingLastSettings.Text = "Smooth Data using last settings"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(249, 6)
        '
        'cmnuNormalizeData
        '
        Me.cmnuNormalizeData.Enabled = False
        Me.cmnuNormalizeData.Name = "cmnuNormalizeData"
        Me.cmnuNormalizeData.Size = New System.Drawing.Size(252, 22)
        Me.cmnuNormalizeData.Text = "Normalize Data to certain Range"
        '
        'cmnuNormalizeDataUsingLastSettings
        '
        Me.cmnuNormalizeDataUsingLastSettings.Enabled = False
        Me.cmnuNormalizeDataUsingLastSettings.Name = "cmnuNormalizeDataUsingLastSettings"
        Me.cmnuNormalizeDataUsingLastSettings.Size = New System.Drawing.Size(252, 22)
        Me.cmnuNormalizeDataUsingLastSettings.Text = "Normalize Data using last settings"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(249, 6)
        '
        'cmnuRenormalizeData
        '
        Me.cmnuRenormalizeData.Enabled = False
        Me.cmnuRenormalizeData.Name = "cmnuRenormalizeData"
        Me.cmnuRenormalizeData.Size = New System.Drawing.Size(252, 22)
        Me.cmnuRenormalizeData.Text = "Regauge Data to Derivative"
        '
        'cmnuRenormalizeDataUsingLastSettings
        '
        Me.cmnuRenormalizeDataUsingLastSettings.Enabled = False
        Me.cmnuRenormalizeDataUsingLastSettings.Name = "cmnuRenormalizeDataUsingLastSettings"
        Me.cmnuRenormalizeDataUsingLastSettings.Size = New System.Drawing.Size(252, 22)
        Me.cmnuRenormalizeDataUsingLastSettings.Text = "Regauge Data using last settings"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(249, 6)
        '
        'cmnuInterpolateData
        '
        Me.cmnuInterpolateData.Enabled = False
        Me.cmnuInterpolateData.Name = "cmnuInterpolateData"
        Me.cmnuInterpolateData.Size = New System.Drawing.Size(252, 22)
        Me.cmnuInterpolateData.Text = "Interpolate Data"
        '
        'cmnuFitNonLinear
        '
        Me.cmnuFitNonLinear.Name = "cmnuFitNonLinear"
        Me.cmnuFitNonLinear.Size = New System.Drawing.Size(282, 22)
        Me.cmnuFitNonLinear.Text = "Non linear Data Fitting"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(279, 6)
        '
        'cmnuPreviewImageHeading
        '
        Me.cmnuPreviewImageHeading.Enabled = False
        Me.cmnuPreviewImageHeading.Name = "cmnuPreviewImageHeading"
        Me.cmnuPreviewImageHeading.Size = New System.Drawing.Size(282, 22)
        Me.cmnuPreviewImageHeading.Text = "Preview-Image Settings"
        '
        'cmnuCBPreviewImageX
        '
        Me.cmnuCBPreviewImageX.BackColor = System.Drawing.Color.Gainsboro
        Me.cmnuCBPreviewImageX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmnuCBPreviewImageX.Name = "cmnuCBPreviewImageX"
        Me.cmnuCBPreviewImageX.Size = New System.Drawing.Size(200, 23)
        Me.cmnuCBPreviewImageX.ToolTipText = "X Column of the Preview Images"
        '
        'cmnuCBPreviewImageY
        '
        Me.cmnuCBPreviewImageY.BackColor = System.Drawing.Color.Gainsboro
        Me.cmnuCBPreviewImageY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmnuCBPreviewImageY.Name = "cmnuCBPreviewImageY"
        Me.cmnuCBPreviewImageY.Size = New System.Drawing.Size(200, 23)
        Me.cmnuCBPreviewImageY.ToolTipText = "Y Column of the Preview Images"
        '
        'cmnuPreviewImageDisplay
        '
        Me.cmnuPreviewImageDisplay.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuPreviewImageLogarithmicX, Me.cmnuPreviewImageLogarithmicY, Me.cmnuPreviewImagePointReduction, Me.ToolStripSeparator7, Me.cmnuCBPreviewImageSize})
        Me.cmnuPreviewImageDisplay.Name = "cmnuPreviewImageDisplay"
        Me.cmnuPreviewImageDisplay.Size = New System.Drawing.Size(282, 22)
        Me.cmnuPreviewImageDisplay.Text = "Display Options"
        '
        'cmnuPreviewImageLogarithmicX
        '
        Me.cmnuPreviewImageLogarithmicX.CheckOnClick = True
        Me.cmnuPreviewImageLogarithmicX.Name = "cmnuPreviewImageLogarithmicX"
        Me.cmnuPreviewImageLogarithmicX.Size = New System.Drawing.Size(269, 22)
        Me.cmnuPreviewImageLogarithmicX.Text = "Logarithmic: Column X"
        '
        'cmnuPreviewImageLogarithmicY
        '
        Me.cmnuPreviewImageLogarithmicY.CheckOnClick = True
        Me.cmnuPreviewImageLogarithmicY.Name = "cmnuPreviewImageLogarithmicY"
        Me.cmnuPreviewImageLogarithmicY.Size = New System.Drawing.Size(269, 22)
        Me.cmnuPreviewImageLogarithmicY.Text = "Logarithmic: Column Y"
        '
        'cmnuPreviewImagePointReduction
        '
        Me.cmnuPreviewImagePointReduction.CheckOnClick = True
        Me.cmnuPreviewImagePointReduction.Name = "cmnuPreviewImagePointReduction"
        Me.cmnuPreviewImagePointReduction.Size = New System.Drawing.Size(269, 22)
        Me.cmnuPreviewImagePointReduction.Text = "Point Reduction for faster Processing"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(266, 6)
        '
        'cmnuCBPreviewImageSize
        '
        Me.cmnuCBPreviewImageSize.BackColor = System.Drawing.Color.Gainsboro
        Me.cmnuCBPreviewImageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmnuCBPreviewImageSize.Items.AddRange(New Object() {"150x100 Small", "300x200 Medium", "450x300 Large"})
        Me.cmnuCBPreviewImageSize.Name = "cmnuCBPreviewImageSize"
        Me.cmnuCBPreviewImageSize.Size = New System.Drawing.Size(200, 23)
        '
        'mSpectroscopyTableFileList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgvSpectroscopyFileList)
        Me.Name = "mSpectroscopyTableFileList"
        Me.Size = New System.Drawing.Size(633, 487)
        CType(Me.dgvSpectroscopyFileList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmFileList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvSpectroscopyFileList As System.Windows.Forms.DataGridView
    Friend WithEvents cmFileList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuShowNearestScanImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuOpenExportWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCopyDataToClipboardOriginCompatible As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCopyDataToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuVisualization As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCreateContourPlotFromLinescans As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuDataManipulations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuShiftColumnValues As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuNumericalManipulations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuAverageDataSingleFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuAverageDataMultipleFiles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuSmoothData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSmoothDataUsingLastSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuNormalizeData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuNormalizeDataUsingLastSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuRenormalizeData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuRenormalizeDataUsingLastSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuInterpolateData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuPreviewImageDisplay As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuPreviewImageLogarithmicX As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuPreviewImageLogarithmicY As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuPreviewImageHeading As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCBPreviewImageX As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents cmnuCBPreviewImageY As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents cmnuPreviewImagePointReduction As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCBPreviewImageSize As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents colPreviewImage As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents colFullFileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colColumnList As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colComment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDataPoints As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuCropData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCropDataUsingLastSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuDisplayTogether As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuTestSingle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuTestMultiple As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuFitNonLinear As System.Windows.Forms.ToolStripMenuItem

End Class
