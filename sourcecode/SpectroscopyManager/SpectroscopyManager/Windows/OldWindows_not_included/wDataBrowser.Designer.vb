<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataBrowser
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
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
        Me.cmnuSpectroscopyTableShowDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuVisualization = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCreateContourPlotFromLinescans = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuDataManipulations = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCropData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCropDataUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuMultiplyData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuMultiplyDataLastFactor = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuMultiplyDataLastOtherColumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuShiftColumnValues = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.cmnuRenormalizeDataByParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuRenormalizeDataByParameterUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuRenormalizeData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuRenormalizeDataUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuCalculateDataDerivative = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCalculateDataDerivativeUsingLastSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuInterpolateData = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuFitNonLinear = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuScanImageShowDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuCacheManagement = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCacheClearCreatedDataColumns = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCacheClearCreatedScanChannels = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCacheClearPreviewImages = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCacheClearCropInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuCacheClearAdditionalComments = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.pgProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.lblProgressBar = New System.Windows.Forms.ToolStripStatusLabel()
        Me.dpSettings = New SpectroscopyManager.DockablePanel()
        Me.gbScanImageSettings = New System.Windows.Forms.GroupBox()
        Me.ckbPlaneSubstractionGlobal = New System.Windows.Forms.CheckBox()
        Me.cbScanImages_PreviewChannel = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.gbTableFileSettings = New System.Windows.Forms.GroupBox()
        Me.ckbTableFiles_LogY = New System.Windows.Forms.CheckBox()
        Me.cbTableFiles_PreviewYColumn = New System.Windows.Forms.ComboBox()
        Me.cbTableFiles_PreviewXColumn = New System.Windows.Forms.ComboBox()
        Me.ckbTableFiles_LogX = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gbPreviewSettings = New System.Windows.Forms.GroupBox()
        Me.ckbEnablePointReduction = New System.Windows.Forms.CheckBox()
        Me.cbPreviewImageSize = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvFileList = New System.Windows.Forms.DataGridView()
        Me.colPreviewImage = New System.Windows.Forms.DataGridViewImageColumn()
        Me.colFileType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFullFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFileName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colColumnList = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colComment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAdditionalComment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDataPoints = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFileDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FileListMenu = New System.Windows.Forms.MenuStrip()
        Me.mnuRefreshList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.spDataPreview = New System.Windows.Forms.SplitContainer()
        Me.pbPreviewBox = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.svScanViewer = New SpectroscopyManager.mScanImageViewer()
        Me.cmFileList.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.MainStatusStrip.SuspendLayout()
        Me.dpSettings.SuspendLayout()
        Me.gbScanImageSettings.SuspendLayout()
        Me.gbTableFileSettings.SuspendLayout()
        Me.gbPreviewSettings.SuspendLayout()
        CType(Me.dgvFileList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FileListMenu.SuspendLayout()
        CType(Me.spDataPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.spDataPreview.Panel1.SuspendLayout()
        Me.spDataPreview.Panel2.SuspendLayout()
        Me.spDataPreview.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmFileList
        '
        Me.cmFileList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuTestSingle, Me.cmnuTestMultiple, Me.cmnuShowNearestScanImage, Me.cmnuDisplayTogether, Me.ToolStripSeparator11, Me.cmnuOpenExportWizard, Me.cmnuCopyDataToClipboardOriginCompatible, Me.cmnuCopyDataToClipboard, Me.ToolStripSeparator4, Me.cmnuSpectroscopyTableShowDetails, Me.cmnuVisualization, Me.cmnuDataManipulations, Me.cmnuNumericalManipulations, Me.cmnuFitNonLinear, Me.ToolStripSeparator1, Me.cmnuScanImageShowDetails, Me.ToolStripSeparator9, Me.cmnuCacheManagement})
        Me.cmFileList.Name = "dgvContextMenu"
        Me.cmFileList.ShowImageMargin = False
        Me.cmFileList.Size = New System.Drawing.Size(287, 358)
        '
        'cmnuTestSingle
        '
        Me.cmnuTestSingle.Name = "cmnuTestSingle"
        Me.cmnuTestSingle.Size = New System.Drawing.Size(286, 22)
        Me.cmnuTestSingle.Text = "Test-Function - Single"
        '
        'cmnuTestMultiple
        '
        Me.cmnuTestMultiple.Name = "cmnuTestMultiple"
        Me.cmnuTestMultiple.Size = New System.Drawing.Size(286, 22)
        Me.cmnuTestMultiple.Text = "Test-Function - Multiple"
        '
        'cmnuShowNearestScanImage
        '
        Me.cmnuShowNearestScanImage.Enabled = False
        Me.cmnuShowNearestScanImage.Name = "cmnuShowNearestScanImage"
        Me.cmnuShowNearestScanImage.Size = New System.Drawing.Size(286, 22)
        Me.cmnuShowNearestScanImage.Text = "Show nearest Scan-Image"
        '
        'cmnuDisplayTogether
        '
        Me.cmnuDisplayTogether.Enabled = False
        Me.cmnuDisplayTogether.Name = "cmnuDisplayTogether"
        Me.cmnuDisplayTogether.Size = New System.Drawing.Size(286, 22)
        Me.cmnuDisplayTogether.Text = "Show together in preview window"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(283, 6)
        '
        'cmnuOpenExportWizard
        '
        Me.cmnuOpenExportWizard.Name = "cmnuOpenExportWizard"
        Me.cmnuOpenExportWizard.Size = New System.Drawing.Size(286, 22)
        Me.cmnuOpenExportWizard.Text = "Open Export Wizard"
        '
        'cmnuCopyDataToClipboardOriginCompatible
        '
        Me.cmnuCopyDataToClipboardOriginCompatible.Enabled = False
        Me.cmnuCopyDataToClipboardOriginCompatible.Name = "cmnuCopyDataToClipboardOriginCompatible"
        Me.cmnuCopyDataToClipboardOriginCompatible.Size = New System.Drawing.Size(286, 22)
        Me.cmnuCopyDataToClipboardOriginCompatible.Text = "Copy Data to Clipboard (Origin Compatible)"
        '
        'cmnuCopyDataToClipboard
        '
        Me.cmnuCopyDataToClipboard.Enabled = False
        Me.cmnuCopyDataToClipboard.Name = "cmnuCopyDataToClipboard"
        Me.cmnuCopyDataToClipboard.Size = New System.Drawing.Size(286, 22)
        Me.cmnuCopyDataToClipboard.Text = "Copy Data to Clipboard (English Format)"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(283, 6)
        '
        'cmnuSpectroscopyTableShowDetails
        '
        Me.cmnuSpectroscopyTableShowDetails.Name = "cmnuSpectroscopyTableShowDetails"
        Me.cmnuSpectroscopyTableShowDetails.Size = New System.Drawing.Size(286, 22)
        Me.cmnuSpectroscopyTableShowDetails.Text = "Open Spectroscopy Files in Separate Window"
        '
        'cmnuVisualization
        '
        Me.cmnuVisualization.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuCreateContourPlotFromLinescans})
        Me.cmnuVisualization.Name = "cmnuVisualization"
        Me.cmnuVisualization.Size = New System.Drawing.Size(286, 22)
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
        Me.cmnuDataManipulations.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuCropData, Me.cmnuCropDataUsingLastSettings, Me.ToolStripSeparator7, Me.cmnuMultiplyData, Me.cmnuMultiplyDataLastFactor, Me.cmnuMultiplyDataLastOtherColumn, Me.ToolStripSeparator8, Me.cmnuShiftColumnValues})
        Me.cmnuDataManipulations.Name = "cmnuDataManipulations"
        Me.cmnuDataManipulations.Size = New System.Drawing.Size(286, 22)
        Me.cmnuDataManipulations.Text = "Data Manipulations"
        '
        'cmnuCropData
        '
        Me.cmnuCropData.Name = "cmnuCropData"
        Me.cmnuCropData.Size = New System.Drawing.Size(338, 22)
        Me.cmnuCropData.Text = "Crop Data from Table"
        '
        'cmnuCropDataUsingLastSettings
        '
        Me.cmnuCropDataUsingLastSettings.Name = "cmnuCropDataUsingLastSettings"
        Me.cmnuCropDataUsingLastSettings.Size = New System.Drawing.Size(338, 22)
        Me.cmnuCropDataUsingLastSettings.Text = "Crop Data using last settings"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(335, 6)
        '
        'cmnuMultiplyData
        '
        Me.cmnuMultiplyData.Name = "cmnuMultiplyData"
        Me.cmnuMultiplyData.Size = New System.Drawing.Size(338, 22)
        Me.cmnuMultiplyData.Text = "Multiply / Divide Data"
        '
        'cmnuMultiplyDataLastFactor
        '
        Me.cmnuMultiplyDataLastFactor.Name = "cmnuMultiplyDataLastFactor"
        Me.cmnuMultiplyDataLastFactor.Size = New System.Drawing.Size(338, 22)
        Me.cmnuMultiplyDataLastFactor.Text = "Multiply / Divide Data using last factor"
        '
        'cmnuMultiplyDataLastOtherColumn
        '
        Me.cmnuMultiplyDataLastOtherColumn.Name = "cmnuMultiplyDataLastOtherColumn"
        Me.cmnuMultiplyDataLastOtherColumn.Size = New System.Drawing.Size(338, 22)
        Me.cmnuMultiplyDataLastOtherColumn.Text = "Multiply / Divide Data using last reference column"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(335, 6)
        '
        'cmnuShiftColumnValues
        '
        Me.cmnuShiftColumnValues.Enabled = False
        Me.cmnuShiftColumnValues.Name = "cmnuShiftColumnValues"
        Me.cmnuShiftColumnValues.Size = New System.Drawing.Size(338, 22)
        Me.cmnuShiftColumnValues.Text = "Shift Column Values"
        '
        'cmnuNumericalManipulations
        '
        Me.cmnuNumericalManipulations.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuAverageDataSingleFile, Me.cmnuAverageDataMultipleFiles, Me.ToolStripSeparator2, Me.cmnuSmoothData, Me.cmnuSmoothDataUsingLastSettings, Me.ToolStripSeparator5, Me.cmnuNormalizeData, Me.cmnuNormalizeDataUsingLastSettings, Me.ToolStripSeparator3, Me.cmnuRenormalizeDataByParameter, Me.cmnuRenormalizeDataByParameterUsingLastSettings, Me.ToolStripSeparator10, Me.cmnuRenormalizeData, Me.cmnuRenormalizeDataUsingLastSettings, Me.ToolStripSeparator12, Me.cmnuCalculateDataDerivative, Me.cmnuCalculateDataDerivativeUsingLastSettings, Me.ToolStripSeparator6, Me.cmnuInterpolateData})
        Me.cmnuNumericalManipulations.Name = "cmnuNumericalManipulations"
        Me.cmnuNumericalManipulations.Size = New System.Drawing.Size(286, 22)
        Me.cmnuNumericalManipulations.Text = "Numerical Manipulations"
        '
        'cmnuAverageDataSingleFile
        '
        Me.cmnuAverageDataSingleFile.Enabled = False
        Me.cmnuAverageDataSingleFile.Name = "cmnuAverageDataSingleFile"
        Me.cmnuAverageDataSingleFile.Size = New System.Drawing.Size(410, 22)
        Me.cmnuAverageDataSingleFile.Text = "Average Data in a Single File"
        Me.cmnuAverageDataSingleFile.Visible = False
        '
        'cmnuAverageDataMultipleFiles
        '
        Me.cmnuAverageDataMultipleFiles.Enabled = False
        Me.cmnuAverageDataMultipleFiles.Name = "cmnuAverageDataMultipleFiles"
        Me.cmnuAverageDataMultipleFiles.Size = New System.Drawing.Size(410, 22)
        Me.cmnuAverageDataMultipleFiles.Text = "Average Data from Multiple Files"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(407, 6)
        '
        'cmnuSmoothData
        '
        Me.cmnuSmoothData.Enabled = False
        Me.cmnuSmoothData.Name = "cmnuSmoothData"
        Me.cmnuSmoothData.Size = New System.Drawing.Size(410, 22)
        Me.cmnuSmoothData.Text = "Smooth Data"
        '
        'cmnuSmoothDataUsingLastSettings
        '
        Me.cmnuSmoothDataUsingLastSettings.Enabled = False
        Me.cmnuSmoothDataUsingLastSettings.Name = "cmnuSmoothDataUsingLastSettings"
        Me.cmnuSmoothDataUsingLastSettings.Size = New System.Drawing.Size(410, 22)
        Me.cmnuSmoothDataUsingLastSettings.Text = "Smooth Data using last settings"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(407, 6)
        '
        'cmnuNormalizeData
        '
        Me.cmnuNormalizeData.Enabled = False
        Me.cmnuNormalizeData.Name = "cmnuNormalizeData"
        Me.cmnuNormalizeData.Size = New System.Drawing.Size(410, 22)
        Me.cmnuNormalizeData.Text = "Normalize Data to certain Range"
        '
        'cmnuNormalizeDataUsingLastSettings
        '
        Me.cmnuNormalizeDataUsingLastSettings.Enabled = False
        Me.cmnuNormalizeDataUsingLastSettings.Name = "cmnuNormalizeDataUsingLastSettings"
        Me.cmnuNormalizeDataUsingLastSettings.Size = New System.Drawing.Size(410, 22)
        Me.cmnuNormalizeDataUsingLastSettings.Text = "Normalize Data using last settings"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(407, 6)
        '
        'cmnuRenormalizeDataByParameter
        '
        Me.cmnuRenormalizeDataByParameter.Enabled = False
        Me.cmnuRenormalizeDataByParameter.Name = "cmnuRenormalizeDataByParameter"
        Me.cmnuRenormalizeDataByParameter.Size = New System.Drawing.Size(410, 22)
        Me.cmnuRenormalizeDataByParameter.Text = "Re-gauge Data by Data Acuisition Parameters"
        '
        'cmnuRenormalizeDataByParameterUsingLastSettings
        '
        Me.cmnuRenormalizeDataByParameterUsingLastSettings.Enabled = False
        Me.cmnuRenormalizeDataByParameterUsingLastSettings.Name = "cmnuRenormalizeDataByParameterUsingLastSettings"
        Me.cmnuRenormalizeDataByParameterUsingLastSettings.Size = New System.Drawing.Size(410, 22)
        Me.cmnuRenormalizeDataByParameterUsingLastSettings.Text = "Re-gauge Data by Data Acuisition Parameters using last settings"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(407, 6)
        '
        'cmnuRenormalizeData
        '
        Me.cmnuRenormalizeData.Enabled = False
        Me.cmnuRenormalizeData.Name = "cmnuRenormalizeData"
        Me.cmnuRenormalizeData.Size = New System.Drawing.Size(410, 22)
        Me.cmnuRenormalizeData.Text = "Re-gauge Data to Numeric Derivative"
        '
        'cmnuRenormalizeDataUsingLastSettings
        '
        Me.cmnuRenormalizeDataUsingLastSettings.Enabled = False
        Me.cmnuRenormalizeDataUsingLastSettings.Name = "cmnuRenormalizeDataUsingLastSettings"
        Me.cmnuRenormalizeDataUsingLastSettings.Size = New System.Drawing.Size(410, 22)
        Me.cmnuRenormalizeDataUsingLastSettings.Text = "Re-gauge Data to Numeric Derivative using last settings"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(407, 6)
        '
        'cmnuCalculateDataDerivative
        '
        Me.cmnuCalculateDataDerivative.Enabled = False
        Me.cmnuCalculateDataDerivative.Name = "cmnuCalculateDataDerivative"
        Me.cmnuCalculateDataDerivative.Size = New System.Drawing.Size(410, 22)
        Me.cmnuCalculateDataDerivative.Text = "Calculate Numeric Derivative of Data"
        '
        'cmnuCalculateDataDerivativeUsingLastSettings
        '
        Me.cmnuCalculateDataDerivativeUsingLastSettings.Enabled = False
        Me.cmnuCalculateDataDerivativeUsingLastSettings.Name = "cmnuCalculateDataDerivativeUsingLastSettings"
        Me.cmnuCalculateDataDerivativeUsingLastSettings.Size = New System.Drawing.Size(410, 22)
        Me.cmnuCalculateDataDerivativeUsingLastSettings.Text = "Calculate Numeric Derivative of Data using last settings"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(407, 6)
        Me.ToolStripSeparator6.Visible = False
        '
        'cmnuInterpolateData
        '
        Me.cmnuInterpolateData.Enabled = False
        Me.cmnuInterpolateData.Name = "cmnuInterpolateData"
        Me.cmnuInterpolateData.Size = New System.Drawing.Size(410, 22)
        Me.cmnuInterpolateData.Text = "Interpolate Data"
        Me.cmnuInterpolateData.Visible = False
        '
        'cmnuFitNonLinear
        '
        Me.cmnuFitNonLinear.Name = "cmnuFitNonLinear"
        Me.cmnuFitNonLinear.Size = New System.Drawing.Size(286, 22)
        Me.cmnuFitNonLinear.Text = "Non linear Data Fitting"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(283, 6)
        '
        'cmnuScanImageShowDetails
        '
        Me.cmnuScanImageShowDetails.Name = "cmnuScanImageShowDetails"
        Me.cmnuScanImageShowDetails.Size = New System.Drawing.Size(286, 22)
        Me.cmnuScanImageShowDetails.Text = "Open Scan Image Files in Separate Window"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(283, 6)
        '
        'cmnuCacheManagement
        '
        Me.cmnuCacheManagement.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuCacheClearCreatedDataColumns, Me.cmnuCacheClearCreatedScanChannels, Me.cmnuCacheClearPreviewImages, Me.cmnuCacheClearCropInformation, Me.cmnuCacheClearAdditionalComments})
        Me.cmnuCacheManagement.Name = "cmnuCacheManagement"
        Me.cmnuCacheManagement.Size = New System.Drawing.Size(286, 22)
        Me.cmnuCacheManagement.Text = "Persistent Data Cache"
        '
        'cmnuCacheClearCreatedDataColumns
        '
        Me.cmnuCacheClearCreatedDataColumns.Enabled = False
        Me.cmnuCacheClearCreatedDataColumns.Name = "cmnuCacheClearCreatedDataColumns"
        Me.cmnuCacheClearCreatedDataColumns.Size = New System.Drawing.Size(285, 22)
        Me.cmnuCacheClearCreatedDataColumns.Text = "clear added spectroscopy data-columns"
        '
        'cmnuCacheClearCreatedScanChannels
        '
        Me.cmnuCacheClearCreatedScanChannels.Enabled = False
        Me.cmnuCacheClearCreatedScanChannels.Name = "cmnuCacheClearCreatedScanChannels"
        Me.cmnuCacheClearCreatedScanChannels.Size = New System.Drawing.Size(285, 22)
        Me.cmnuCacheClearCreatedScanChannels.Text = "clear created scan-channels"
        '
        'cmnuCacheClearPreviewImages
        '
        Me.cmnuCacheClearPreviewImages.Enabled = False
        Me.cmnuCacheClearPreviewImages.Name = "cmnuCacheClearPreviewImages"
        Me.cmnuCacheClearPreviewImages.Size = New System.Drawing.Size(285, 22)
        Me.cmnuCacheClearPreviewImages.Text = "clear preview images"
        '
        'cmnuCacheClearCropInformation
        '
        Me.cmnuCacheClearCropInformation.Enabled = False
        Me.cmnuCacheClearCropInformation.Name = "cmnuCacheClearCropInformation"
        Me.cmnuCacheClearCropInformation.Size = New System.Drawing.Size(285, 22)
        Me.cmnuCacheClearCropInformation.Text = "clear crop-informations"
        '
        'cmnuCacheClearAdditionalComments
        '
        Me.cmnuCacheClearAdditionalComments.Enabled = False
        Me.cmnuCacheClearAdditionalComments.Name = "cmnuCacheClearAdditionalComments"
        Me.cmnuCacheClearAdditionalComments.Size = New System.Drawing.Size(285, 22)
        Me.cmnuCacheClearAdditionalComments.Text = "clear extended comments"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.MainStatusStrip)
        Me.SplitContainer1.Panel1.Controls.Add(Me.dpSettings)
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgvFileList)
        Me.SplitContainer1.Panel1.Controls.Add(Me.FileListMenu)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.spDataPreview)
        Me.SplitContainer1.Size = New System.Drawing.Size(1326, 883)
        Me.SplitContainer1.SplitterDistance = 744
        Me.SplitContainer1.TabIndex = 0
        '
        'MainStatusStrip
        '
        Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.pgProgress, Me.lblProgressBar})
        Me.MainStatusStrip.Location = New System.Drawing.Point(0, 861)
        Me.MainStatusStrip.Name = "MainStatusStrip"
        Me.MainStatusStrip.Size = New System.Drawing.Size(744, 22)
        Me.MainStatusStrip.SizingGrip = False
        Me.MainStatusStrip.TabIndex = 23
        Me.MainStatusStrip.Text = "StatusStrip1"
        '
        'pgProgress
        '
        Me.pgProgress.Name = "pgProgress"
        Me.pgProgress.Size = New System.Drawing.Size(200, 16)
        '
        'lblProgressBar
        '
        Me.lblProgressBar.Name = "lblProgressBar"
        Me.lblProgressBar.Size = New System.Drawing.Size(527, 17)
        Me.lblProgressBar.Spring = True
        Me.lblProgressBar.Text = "Ready"
        Me.lblProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dpSettings
        '
        Me.dpSettings.Controls.Add(Me.gbScanImageSettings)
        Me.dpSettings.Controls.Add(Me.gbTableFileSettings)
        Me.dpSettings.Controls.Add(Me.gbPreviewSettings)
        Me.dpSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.dpSettings.Location = New System.Drawing.Point(0, 24)
        Me.dpSettings.Name = "dpSettings"
        'TODO: Beim Generieren des Codes für "" ist die Ausnahme "Ungültiger primitiver Typ: System.IntPtr. Verwenden Sie CodeObjectCreateExpression." aufgetreten.
        Me.dpSettings.Size = New System.Drawing.Size(744, 100)
        Me.dpSettings.SlidePixelsPerTimerTick = 25
        Me.dpSettings.TabIndex = 12
        '
        'gbScanImageSettings
        '
        Me.gbScanImageSettings.Controls.Add(Me.ckbPlaneSubstractionGlobal)
        Me.gbScanImageSettings.Controls.Add(Me.cbScanImages_PreviewChannel)
        Me.gbScanImageSettings.Controls.Add(Me.Label4)
        Me.gbScanImageSettings.Location = New System.Drawing.Point(425, 4)
        Me.gbScanImageSettings.Name = "gbScanImageSettings"
        Me.gbScanImageSettings.Size = New System.Drawing.Size(200, 93)
        Me.gbScanImageSettings.TabIndex = 1
        Me.gbScanImageSettings.TabStop = False
        Me.gbScanImageSettings.Text = "Scan Image Settings"
        '
        'ckbPlaneSubstractionGlobal
        '
        Me.ckbPlaneSubstractionGlobal.AutoSize = True
        Me.ckbPlaneSubstractionGlobal.Enabled = False
        Me.ckbPlaneSubstractionGlobal.Location = New System.Drawing.Point(18, 22)
        Me.ckbPlaneSubstractionGlobal.Name = "ckbPlaneSubstractionGlobal"
        Me.ckbPlaneSubstractionGlobal.Size = New System.Drawing.Size(148, 17)
        Me.ckbPlaneSubstractionGlobal.TabIndex = 0
        Me.ckbPlaneSubstractionGlobal.Text = "Global Plane Substraction"
        Me.ckbPlaneSubstractionGlobal.UseVisualStyleBackColor = True
        '
        'cbScanImages_PreviewChannel
        '
        Me.cbScanImages_PreviewChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbScanImages_PreviewChannel.FormattingEnabled = True
        Me.cbScanImages_PreviewChannel.Location = New System.Drawing.Point(14, 58)
        Me.cbScanImages_PreviewChannel.Name = "cbScanImages_PreviewChannel"
        Me.cbScanImages_PreviewChannel.Size = New System.Drawing.Size(176, 21)
        Me.cbScanImages_PreviewChannel.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 44)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(87, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Preview Channel"
        '
        'gbTableFileSettings
        '
        Me.gbTableFileSettings.Controls.Add(Me.ckbTableFiles_LogY)
        Me.gbTableFileSettings.Controls.Add(Me.cbTableFiles_PreviewYColumn)
        Me.gbTableFileSettings.Controls.Add(Me.cbTableFiles_PreviewXColumn)
        Me.gbTableFileSettings.Controls.Add(Me.ckbTableFiles_LogX)
        Me.gbTableFileSettings.Controls.Add(Me.Label3)
        Me.gbTableFileSettings.Controls.Add(Me.Label2)
        Me.gbTableFileSettings.Location = New System.Drawing.Point(219, 4)
        Me.gbTableFileSettings.Name = "gbTableFileSettings"
        Me.gbTableFileSettings.Size = New System.Drawing.Size(200, 93)
        Me.gbTableFileSettings.TabIndex = 1
        Me.gbTableFileSettings.TabStop = False
        Me.gbTableFileSettings.Text = "Table File Settings"
        '
        'ckbTableFiles_LogY
        '
        Me.ckbTableFiles_LogY.AutoSize = True
        Me.ckbTableFiles_LogY.Location = New System.Drawing.Point(154, 72)
        Me.ckbTableFiles_LogY.Name = "ckbTableFiles_LogY"
        Me.ckbTableFiles_LogY.Size = New System.Drawing.Size(40, 17)
        Me.ckbTableFiles_LogY.TabIndex = 0
        Me.ckbTableFiles_LogY.Text = "log"
        Me.ckbTableFiles_LogY.UseVisualStyleBackColor = True
        '
        'cbTableFiles_PreviewYColumn
        '
        Me.cbTableFiles_PreviewYColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTableFiles_PreviewYColumn.FormattingEnabled = True
        Me.cbTableFiles_PreviewYColumn.Location = New System.Drawing.Point(14, 68)
        Me.cbTableFiles_PreviewYColumn.Name = "cbTableFiles_PreviewYColumn"
        Me.cbTableFiles_PreviewYColumn.Size = New System.Drawing.Size(134, 21)
        Me.cbTableFiles_PreviewYColumn.TabIndex = 1
        '
        'cbTableFiles_PreviewXColumn
        '
        Me.cbTableFiles_PreviewXColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTableFiles_PreviewXColumn.FormattingEnabled = True
        Me.cbTableFiles_PreviewXColumn.Location = New System.Drawing.Point(14, 32)
        Me.cbTableFiles_PreviewXColumn.Name = "cbTableFiles_PreviewXColumn"
        Me.cbTableFiles_PreviewXColumn.Size = New System.Drawing.Size(134, 21)
        Me.cbTableFiles_PreviewXColumn.TabIndex = 1
        '
        'ckbTableFiles_LogX
        '
        Me.ckbTableFiles_LogX.AutoSize = True
        Me.ckbTableFiles_LogX.Location = New System.Drawing.Point(154, 36)
        Me.ckbTableFiles_LogX.Name = "ckbTableFiles_LogX"
        Me.ckbTableFiles_LogX.Size = New System.Drawing.Size(40, 17)
        Me.ckbTableFiles_LogX.TabIndex = 0
        Me.ckbTableFiles_LogX.Text = "log"
        Me.ckbTableFiles_LogX.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Preview Y column:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Preview X column:"
        '
        'gbPreviewSettings
        '
        Me.gbPreviewSettings.Controls.Add(Me.ckbEnablePointReduction)
        Me.gbPreviewSettings.Controls.Add(Me.cbPreviewImageSize)
        Me.gbPreviewSettings.Controls.Add(Me.Label1)
        Me.gbPreviewSettings.Location = New System.Drawing.Point(4, 4)
        Me.gbPreviewSettings.Name = "gbPreviewSettings"
        Me.gbPreviewSettings.Size = New System.Drawing.Size(208, 93)
        Me.gbPreviewSettings.TabIndex = 0
        Me.gbPreviewSettings.TabStop = False
        Me.gbPreviewSettings.Text = "General Preview Settings"
        '
        'ckbEnablePointReduction
        '
        Me.ckbEnablePointReduction.AutoSize = True
        Me.ckbEnablePointReduction.Location = New System.Drawing.Point(11, 62)
        Me.ckbEnablePointReduction.Name = "ckbEnablePointReduction"
        Me.ckbEnablePointReduction.Size = New System.Drawing.Size(131, 17)
        Me.ckbEnablePointReduction.TabIndex = 2
        Me.ckbEnablePointReduction.Text = "enable point reduction"
        Me.ckbEnablePointReduction.UseVisualStyleBackColor = True
        '
        'cbPreviewImageSize
        '
        Me.cbPreviewImageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreviewImageSize.FormattingEnabled = True
        Me.cbPreviewImageSize.Items.AddRange(New Object() {"150x100 Small (Landscape)", "300x200 Medium (Landscape)", "450x300 Large (Landscape)", "128x128 Small (Square)", "256x256 Medium (Square)", "320x320 Small (Square)"})
        Me.cbPreviewImageSize.Location = New System.Drawing.Point(11, 32)
        Me.cbPreviewImageSize.Name = "cbPreviewImageSize"
        Me.cbPreviewImageSize.Size = New System.Drawing.Size(184, 21)
        Me.cbPreviewImageSize.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Image Size:"
        '
        'dgvFileList
        '
        Me.dgvFileList.AllowUserToAddRows = False
        Me.dgvFileList.AllowUserToDeleteRows = False
        Me.dgvFileList.AllowUserToOrderColumns = True
        Me.dgvFileList.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow
        Me.dgvFileList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFileList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvFileList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPreviewImage, Me.colFileType, Me.colFullFileName, Me.colFileName, Me.colColumnList, Me.colComment, Me.colAdditionalComment, Me.colDataPoints, Me.colTime, Me.colFileDate})
        Me.dgvFileList.ContextMenuStrip = Me.cmFileList
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFileList.DefaultCellStyle = DataGridViewCellStyle9
        Me.dgvFileList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFileList.Location = New System.Drawing.Point(0, 24)
        Me.dgvFileList.Name = "dgvFileList"
        Me.dgvFileList.ReadOnly = True
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFileList.RowHeadersDefaultCellStyle = DataGridViewCellStyle10
        Me.dgvFileList.RowHeadersVisible = False
        Me.dgvFileList.RowTemplate.Height = 125
        Me.dgvFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFileList.Size = New System.Drawing.Size(744, 859)
        Me.dgvFileList.TabIndex = 11
        Me.dgvFileList.VirtualMode = True
        '
        'colPreviewImage
        '
        Me.colPreviewImage.Frozen = True
        Me.colPreviewImage.HeaderText = "Preview Image"
        Me.colPreviewImage.Name = "colPreviewImage"
        Me.colPreviewImage.ReadOnly = True
        Me.colPreviewImage.Width = 83
        '
        'colFileType
        '
        Me.colFileType.HeaderText = "FileType"
        Me.colFileType.Name = "colFileType"
        Me.colFileType.ReadOnly = True
        Me.colFileType.Visible = False
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
        Me.colFileName.Frozen = True
        Me.colFileName.HeaderText = "filename"
        Me.colFileName.Name = "colFileName"
        Me.colFileName.ReadOnly = True
        Me.colFileName.Width = 150
        '
        'colColumnList
        '
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.colColumnList.DefaultCellStyle = DataGridViewCellStyle3
        Me.colColumnList.HeaderText = "columns"
        Me.colColumnList.Name = "colColumnList"
        Me.colColumnList.ReadOnly = True
        Me.colColumnList.Width = 150
        '
        'colComment
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        Me.colComment.DefaultCellStyle = DataGridViewCellStyle4
        Me.colComment.HeaderText = "file comment"
        Me.colComment.Name = "colComment"
        Me.colComment.ReadOnly = True
        Me.colComment.Width = 200
        '
        'colAdditionalComment
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        Me.colAdditionalComment.DefaultCellStyle = DataGridViewCellStyle5
        Me.colAdditionalComment.HeaderText = "extended comment"
        Me.colAdditionalComment.Name = "colAdditionalComment"
        Me.colAdditionalComment.ReadOnly = True
        Me.colAdditionalComment.Width = 200
        '
        'colDataPoints
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.colDataPoints.DefaultCellStyle = DataGridViewCellStyle6
        Me.colDataPoints.HeaderText = "points"
        Me.colDataPoints.Name = "colDataPoints"
        Me.colDataPoints.ReadOnly = True
        Me.colDataPoints.Width = 130
        '
        'colTime
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.Format = "G"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.colTime.DefaultCellStyle = DataGridViewCellStyle7
        Me.colTime.HeaderText = "record-date"
        Me.colTime.Name = "colTime"
        Me.colTime.ReadOnly = True
        Me.colTime.Width = 70
        '
        'colFileDate
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.Format = "G"
        Me.colFileDate.DefaultCellStyle = DataGridViewCellStyle8
        Me.colFileDate.HeaderText = "file-date"
        Me.colFileDate.Name = "colFileDate"
        Me.colFileDate.ReadOnly = True
        Me.colFileDate.Width = 70
        '
        'FileListMenu
        '
        Me.FileListMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRefreshList, Me.mnuSettings})
        Me.FileListMenu.Location = New System.Drawing.Point(0, 0)
        Me.FileListMenu.Name = "FileListMenu"
        Me.FileListMenu.Size = New System.Drawing.Size(744, 24)
        Me.FileListMenu.TabIndex = 1
        Me.FileListMenu.Text = "MenuStrip1"
        '
        'mnuRefreshList
        '
        Me.mnuRefreshList.Image = Global.SpectroscopyManager.My.Resources.Resources.reload_16
        Me.mnuRefreshList.Name = "mnuRefreshList"
        Me.mnuRefreshList.Size = New System.Drawing.Size(102, 20)
        Me.mnuRefreshList.Text = "Refresh View"
        '
        'mnuSettings
        '
        Me.mnuSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.mnuSettings.Name = "mnuSettings"
        Me.mnuSettings.Size = New System.Drawing.Size(77, 20)
        Me.mnuSettings.Text = "Settings"
        '
        'spDataPreview
        '
        Me.spDataPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.spDataPreview.Location = New System.Drawing.Point(0, 0)
        Me.spDataPreview.Name = "spDataPreview"
        Me.spDataPreview.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'spDataPreview.Panel1
        '
        Me.spDataPreview.Panel1.Controls.Add(Me.pbPreviewBox)
        '
        'spDataPreview.Panel2
        '
        Me.spDataPreview.Panel2.Controls.Add(Me.svScanViewer)
        Me.spDataPreview.Size = New System.Drawing.Size(578, 883)
        Me.spDataPreview.SplitterDistance = 432
        Me.spDataPreview.TabIndex = 0
        '
        'pbPreviewBox
        '
        Me.pbPreviewBox.AllowAdjustingXColumn = True
        Me.pbPreviewBox.AllowAdjustingYColumn = True
        Me.pbPreviewBox.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbPreviewBox.CallbackXRangeSelected = Nothing
        Me.pbPreviewBox.CallbackXValueSelected = Nothing
        Me.pbPreviewBox.CallbackXYRangeSelected = Nothing
        Me.pbPreviewBox.CallbackYRangeSelected = Nothing
        Me.pbPreviewBox.CallbackYValueSelected = Nothing
        Me.pbPreviewBox.ClearPointSelectionModeAfterSelection = False
        Me.pbPreviewBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbPreviewBox.Location = New System.Drawing.Point(0, 0)
        Me.pbPreviewBox.Name = "pbPreviewBox"
        Me.pbPreviewBox.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbPreviewBox.ShowColumnSelectors = True
        Me.pbPreviewBox.Size = New System.Drawing.Size(578, 432)
        Me.pbPreviewBox.TabIndex = 14
        '
        'svScanViewer
        '
        Me.svScanViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svScanViewer.Location = New System.Drawing.Point(0, 0)
        Me.svScanViewer.Name = "svScanViewer"
        Me.svScanViewer.Size = New System.Drawing.Size(578, 447)
        Me.svScanViewer.TabIndex = 1
        '
        'wDataBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1326, 883)
        Me.Controls.Add(Me.SplitContainer1)
        Me.MainMenuStrip = Me.FileListMenu
        Me.Name = "wDataBrowser"
        Me.Text = "Data Browser"
        Me.cmFileList.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.MainStatusStrip.ResumeLayout(False)
        Me.MainStatusStrip.PerformLayout()
        Me.dpSettings.ResumeLayout(False)
        Me.gbScanImageSettings.ResumeLayout(False)
        Me.gbScanImageSettings.PerformLayout()
        Me.gbTableFileSettings.ResumeLayout(False)
        Me.gbTableFileSettings.PerformLayout()
        Me.gbPreviewSettings.ResumeLayout(False)
        Me.gbPreviewSettings.PerformLayout()
        CType(Me.dgvFileList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FileListMenu.ResumeLayout(False)
        Me.FileListMenu.PerformLayout()
        Me.spDataPreview.Panel1.ResumeLayout(False)
        Me.spDataPreview.Panel2.ResumeLayout(False)
        CType(Me.spDataPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.spDataPreview.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents FileListMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuRefreshList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents spDataPreview As System.Windows.Forms.SplitContainer
    Friend WithEvents pbPreviewBox As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents svScanViewer As SpectroscopyManager.mScanImageViewer
    Friend WithEvents mnuSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dpSettings As SpectroscopyManager.DockablePanel
    Friend WithEvents dgvFileList As System.Windows.Forms.DataGridView
    Friend WithEvents gbPreviewSettings As System.Windows.Forms.GroupBox
    Friend WithEvents gbScanImageSettings As System.Windows.Forms.GroupBox
    Friend WithEvents ckbPlaneSubstractionGlobal As System.Windows.Forms.CheckBox
    Friend WithEvents gbTableFileSettings As System.Windows.Forms.GroupBox
    Friend WithEvents ckbEnablePointReduction As System.Windows.Forms.CheckBox
    Friend WithEvents cbPreviewImageSize As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ckbTableFiles_LogY As System.Windows.Forms.CheckBox
    Friend WithEvents ckbTableFiles_LogX As System.Windows.Forms.CheckBox
    Friend WithEvents cbTableFiles_PreviewYColumn As System.Windows.Forms.ComboBox
    Friend WithEvents cbTableFiles_PreviewXColumn As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbScanImages_PreviewChannel As System.Windows.Forms.ComboBox
    Friend WithEvents MainStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents pgProgress As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents lblProgressBar As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents cmFileList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuTestSingle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuTestMultiple As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuShowNearestScanImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuDisplayTogether As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuOpenExportWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCopyDataToClipboardOriginCompatible As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCopyDataToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuVisualization As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCreateContourPlotFromLinescans As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuDataManipulations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuShiftColumnValues As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuCropData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCropDataUsingLastSettings As System.Windows.Forms.ToolStripMenuItem
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
    Friend WithEvents cmnuFitNonLinear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSpectroscopyTableShowDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuScanImageShowDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuMultiplyData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuMultiplyDataLastFactor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuMultiplyDataLastOtherColumn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuCacheManagement As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCacheClearCreatedDataColumns As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCacheClearCreatedScanChannels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCacheClearPreviewImages As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCacheClearCropInformation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCacheClearAdditionalComments As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents colPreviewImage As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents colFileType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFullFileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colColumnList As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colComment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAdditionalComment As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDataPoints As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFileDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cmnuRenormalizeDataByParameter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuRenormalizeDataByParameterUsingLastSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuCalculateDataDerivative As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuCalculateDataDerivativeUsingLastSettings As System.Windows.Forms.ToolStripMenuItem
End Class
