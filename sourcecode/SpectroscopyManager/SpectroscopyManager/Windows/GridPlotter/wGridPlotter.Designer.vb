<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wGridPlotter
    Inherits wFormBase

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wGridPlotter))
        Me.scDataSelector = New System.Windows.Forms.SplitContainer()
        Me.btnLoadData = New System.Windows.Forms.Button()
        Me.gbScanImages = New System.Windows.Forms.GroupBox()
        Me.dsScanImages = New SpectroscopyManager.mDataBrowserListCompact()
        Me.gbDataSelector = New System.Windows.Forms.GroupBox()
        Me.dsSpectroscopyFiles = New SpectroscopyManager.mDataBrowserListCompact()
        Me.scGridPlotter = New System.Windows.Forms.SplitContainer()
        Me.gbDataRange = New System.Windows.Forms.GroupBox()
        Me.spDataRangeSelector_DataToDisplay = New SpectroscopyManager.ucSpectroscopyTableSelector()
        Me.lblValueRange_SelectPlotData = New System.Windows.Forms.Label()
        Me.btnSelectValueRangeWindow = New System.Windows.Forms.Button()
        Me.pbDataRangeSelector = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.btnSelectValueRangeStart = New System.Windows.Forms.Button()
        Me.lblValueRangeStart = New System.Windows.Forms.Label()
        Me.txtValueRangeStart = New SpectroscopyManager.NumericTextbox()
        Me.lblValueRangeWindow = New System.Windows.Forms.Label()
        Me.txtValueRangeWindow = New SpectroscopyManager.NumericTextbox()
        Me.gbExport = New System.Windows.Forms.GroupBox()
        Me.btnExport_SaveAsXYZ = New System.Windows.Forms.Button()
        Me.gbOutput = New System.Windows.Forms.GroupBox()
        Me.svOutputImage = New SpectroscopyManager.mScanImageViewer()
        Me.gbPlotSettings = New System.Windows.Forms.GroupBox()
        Me.vrsPlotSettings_ColorScaling = New SpectroscopyManager.mValueRangeSelector()
        Me.cpPlotSettings_ColorCode = New SpectroscopyManager.ucColorPalettePicker()
        Me.txtPlotSettings_PointDiameter = New SpectroscopyManager.NumericTextbox()
        Me.lblPlotSettings_ColorCode = New System.Windows.Forms.Label()
        Me.lblPlotSettings_DataPointDiameter = New System.Windows.Forms.Label()
        Me.ssProgress = New System.Windows.Forms.StatusStrip()
        Me.pgProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.lblProgress = New System.Windows.Forms.ToolStripStatusLabel()
        CType(Me.scDataSelector, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scDataSelector.Panel1.SuspendLayout()
        Me.scDataSelector.Panel2.SuspendLayout()
        Me.scDataSelector.SuspendLayout()
        Me.gbScanImages.SuspendLayout()
        Me.gbDataSelector.SuspendLayout()
        CType(Me.scGridPlotter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scGridPlotter.Panel1.SuspendLayout()
        Me.scGridPlotter.Panel2.SuspendLayout()
        Me.scGridPlotter.SuspendLayout()
        Me.gbDataRange.SuspendLayout()
        Me.gbExport.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        Me.gbPlotSettings.SuspendLayout()
        Me.ssProgress.SuspendLayout()
        Me.SuspendLayout()
        '
        'scDataSelector
        '
        Me.scDataSelector.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scDataSelector.Location = New System.Drawing.Point(0, 0)
        Me.scDataSelector.Name = "scDataSelector"
        '
        'scDataSelector.Panel1
        '
        Me.scDataSelector.Panel1.Controls.Add(Me.btnLoadData)
        Me.scDataSelector.Panel1.Controls.Add(Me.gbScanImages)
        Me.scDataSelector.Panel1.Controls.Add(Me.gbDataSelector)
        '
        'scDataSelector.Panel2
        '
        Me.scDataSelector.Panel2.Controls.Add(Me.scGridPlotter)
        Me.scDataSelector.Size = New System.Drawing.Size(1206, 919)
        Me.scDataSelector.SplitterDistance = 233
        Me.scDataSelector.TabIndex = 0
        '
        'btnLoadData
        '
        Me.btnLoadData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadData.Enabled = False
        Me.btnLoadData.Image = Global.SpectroscopyManager.My.Resources.Resources.run_25
        Me.btnLoadData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLoadData.Location = New System.Drawing.Point(3, 3)
        Me.btnLoadData.Name = "btnLoadData"
        Me.btnLoadData.Size = New System.Drawing.Size(224, 37)
        Me.btnLoadData.TabIndex = 1
        Me.btnLoadData.Text = "load selected data set"
        Me.btnLoadData.UseVisualStyleBackColor = True
        '
        'gbScanImages
        '
        Me.gbScanImages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbScanImages.Controls.Add(Me.dsScanImages)
        Me.gbScanImages.Location = New System.Drawing.Point(1, 660)
        Me.gbScanImages.Name = "gbScanImages"
        Me.gbScanImages.Size = New System.Drawing.Size(230, 256)
        Me.gbScanImages.TabIndex = 0
        Me.gbScanImages.TabStop = False
        Me.gbScanImages.Text = "scan image of the grid:"
        '
        'dsScanImages
        '
        Me.dsScanImages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dsScanImages.Location = New System.Drawing.Point(3, 16)
        Me.dsScanImages.Name = "dsScanImages"
        Me.dsScanImages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.dsScanImages.ShowScanImages = True
        Me.dsScanImages.ShowSpectroscopyTables = False
        Me.dsScanImages.Size = New System.Drawing.Size(224, 237)
        Me.dsScanImages.TabIndex = 0
        '
        'gbDataSelector
        '
        Me.gbDataSelector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDataSelector.Controls.Add(Me.dsSpectroscopyFiles)
        Me.gbDataSelector.Location = New System.Drawing.Point(1, 46)
        Me.gbDataSelector.Name = "gbDataSelector"
        Me.gbDataSelector.Size = New System.Drawing.Size(230, 608)
        Me.gbDataSelector.TabIndex = 0
        Me.gbDataSelector.TabStop = False
        Me.gbDataSelector.Text = "spectroscopy files of the grid:"
        '
        'dsSpectroscopyFiles
        '
        Me.dsSpectroscopyFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dsSpectroscopyFiles.Location = New System.Drawing.Point(3, 16)
        Me.dsSpectroscopyFiles.Name = "dsSpectroscopyFiles"
        Me.dsSpectroscopyFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.dsSpectroscopyFiles.ShowScanImages = False
        Me.dsSpectroscopyFiles.ShowSpectroscopyTables = True
        Me.dsSpectroscopyFiles.Size = New System.Drawing.Size(224, 589)
        Me.dsSpectroscopyFiles.TabIndex = 0
        '
        'scGridPlotter
        '
        Me.scGridPlotter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scGridPlotter.Location = New System.Drawing.Point(0, 0)
        Me.scGridPlotter.Name = "scGridPlotter"
        Me.scGridPlotter.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scGridPlotter.Panel1
        '
        Me.scGridPlotter.Panel1.Controls.Add(Me.gbDataRange)
        '
        'scGridPlotter.Panel2
        '
        Me.scGridPlotter.Panel2.Controls.Add(Me.gbExport)
        Me.scGridPlotter.Panel2.Controls.Add(Me.gbOutput)
        Me.scGridPlotter.Panel2.Controls.Add(Me.gbPlotSettings)
        Me.scGridPlotter.Size = New System.Drawing.Size(969, 919)
        Me.scGridPlotter.SplitterDistance = 393
        Me.scGridPlotter.TabIndex = 7
        '
        'gbDataRange
        '
        Me.gbDataRange.Controls.Add(Me.spDataRangeSelector_DataToDisplay)
        Me.gbDataRange.Controls.Add(Me.lblValueRange_SelectPlotData)
        Me.gbDataRange.Controls.Add(Me.btnSelectValueRangeWindow)
        Me.gbDataRange.Controls.Add(Me.pbDataRangeSelector)
        Me.gbDataRange.Controls.Add(Me.btnSelectValueRangeStart)
        Me.gbDataRange.Controls.Add(Me.lblValueRangeStart)
        Me.gbDataRange.Controls.Add(Me.txtValueRangeStart)
        Me.gbDataRange.Controls.Add(Me.lblValueRangeWindow)
        Me.gbDataRange.Controls.Add(Me.txtValueRangeWindow)
        Me.gbDataRange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbDataRange.Enabled = False
        Me.gbDataRange.Location = New System.Drawing.Point(0, 0)
        Me.gbDataRange.Name = "gbDataRange"
        Me.gbDataRange.Size = New System.Drawing.Size(969, 393)
        Me.gbDataRange.TabIndex = 4
        Me.gbDataRange.TabStop = False
        Me.gbDataRange.Text = "select data range to display:"
        '
        'spDataRangeSelector_DataToDisplay
        '
        Me.spDataRangeSelector_DataToDisplay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.spDataRangeSelector_DataToDisplay.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Listbox
        Me.spDataRangeSelector_DataToDisplay.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.spDataRangeSelector_DataToDisplay.Location = New System.Drawing.Point(763, 35)
        Me.spDataRangeSelector_DataToDisplay.MinimumSize = New System.Drawing.Size(0, 21)
        Me.spDataRangeSelector_DataToDisplay.Name = "spDataRangeSelector_DataToDisplay"
        Me.spDataRangeSelector_DataToDisplay.SelectedEntries = CType(resources.GetObject("spDataRangeSelector_DataToDisplay.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.spDataRangeSelector_DataToDisplay.SelectedEntry = ""
        Me.spDataRangeSelector_DataToDisplay.SelectedTable = ""
        Me.spDataRangeSelector_DataToDisplay.SelectedTables = CType(resources.GetObject("spDataRangeSelector_DataToDisplay.SelectedTables"), System.Collections.Generic.List(Of String))
        Me.spDataRangeSelector_DataToDisplay.Size = New System.Drawing.Size(199, 347)
        Me.spDataRangeSelector_DataToDisplay.TabIndex = 8
        Me.spDataRangeSelector_DataToDisplay.TurnOnLastFilterSaving = False
        Me.spDataRangeSelector_DataToDisplay.TurnOnLastSelectionSaving = False
        '
        'lblValueRange_SelectPlotData
        '
        Me.lblValueRange_SelectPlotData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblValueRange_SelectPlotData.AutoSize = True
        Me.lblValueRange_SelectPlotData.Location = New System.Drawing.Point(760, 16)
        Me.lblValueRange_SelectPlotData.Name = "lblValueRange_SelectPlotData"
        Me.lblValueRange_SelectPlotData.Size = New System.Drawing.Size(171, 13)
        Me.lblValueRange_SelectPlotData.TabIndex = 6
        Me.lblValueRange_SelectPlotData.Text = "select data to show in the preview:"
        '
        'btnSelectValueRangeWindow
        '
        Me.btnSelectValueRangeWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectValueRangeWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnSelectValueRangeWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectValueRangeWindow.Location = New System.Drawing.Point(336, 359)
        Me.btnSelectValueRangeWindow.Name = "btnSelectValueRangeWindow"
        Me.btnSelectValueRangeWindow.Size = New System.Drawing.Size(100, 23)
        Me.btnSelectValueRangeWindow.TabIndex = 7
        Me.btnSelectValueRangeWindow.Text = "select in graph"
        Me.btnSelectValueRangeWindow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSelectValueRangeWindow.UseVisualStyleBackColor = True
        '
        'pbDataRangeSelector
        '
        Me.pbDataRangeSelector.AllowAdjustingXColumn = True
        Me.pbDataRangeSelector.AllowAdjustingYColumn = True
        Me.pbDataRangeSelector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbDataRangeSelector.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbDataRangeSelector.CallbackDataPointSelected = Nothing
        Me.pbDataRangeSelector.CallbackXRangeSelected = Nothing
        Me.pbDataRangeSelector.CallbackXValueSelected = Nothing
        Me.pbDataRangeSelector.CallbackXYRangeSelected = Nothing
        Me.pbDataRangeSelector.CallbackXYValueSelected = Nothing
        Me.pbDataRangeSelector.CallbackYRangeSelected = Nothing
        Me.pbDataRangeSelector.CallbackYValueSelected = Nothing
        Me.pbDataRangeSelector.ClearPointSelectionModeAfterSelection = False
        Me.pbDataRangeSelector.Location = New System.Drawing.Point(6, 19)
        Me.pbDataRangeSelector.MultipleSpectraStackOffset = 0R
        Me.pbDataRangeSelector.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbDataRangeSelector.Name = "pbDataRangeSelector"
        Me.pbDataRangeSelector.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbDataRangeSelector.ShowColumnSelectors = True
        Me.pbDataRangeSelector.Size = New System.Drawing.Size(755, 312)
        Me.pbDataRangeSelector.TabIndex = 3
        Me.pbDataRangeSelector.TurnOnLastFilterSaving_Y = False
        Me.pbDataRangeSelector.TurnOnLastSelectionSaving_Y = False
        '
        'btnSelectValueRangeStart
        '
        Me.btnSelectValueRangeStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectValueRangeStart.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectValueRangeStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectValueRangeStart.Location = New System.Drawing.Point(109, 359)
        Me.btnSelectValueRangeStart.Name = "btnSelectValueRangeStart"
        Me.btnSelectValueRangeStart.Size = New System.Drawing.Size(100, 23)
        Me.btnSelectValueRangeStart.TabIndex = 7
        Me.btnSelectValueRangeStart.Text = "select in graph"
        Me.btnSelectValueRangeStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSelectValueRangeStart.UseVisualStyleBackColor = True
        '
        'lblValueRangeStart
        '
        Me.lblValueRangeStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblValueRangeStart.AutoSize = True
        Me.lblValueRangeStart.Location = New System.Drawing.Point(14, 340)
        Me.lblValueRangeStart.Name = "lblValueRangeStart"
        Me.lblValueRangeStart.Size = New System.Drawing.Size(89, 13)
        Me.lblValueRangeStart.TabIndex = 6
        Me.lblValueRangeStart.Text = "value range start:"
        '
        'txtValueRangeStart
        '
        Me.txtValueRangeStart.AllowZero = True
        Me.txtValueRangeStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtValueRangeStart.BackColor = System.Drawing.Color.White
        Me.txtValueRangeStart.ForeColor = System.Drawing.Color.Black
        Me.txtValueRangeStart.FormatDecimalPlaces = 6
        Me.txtValueRangeStart.Location = New System.Drawing.Point(109, 337)
        Me.txtValueRangeStart.Name = "txtValueRangeStart"
        Me.txtValueRangeStart.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtValueRangeStart.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtValueRangeStart.Size = New System.Drawing.Size(100, 20)
        Me.txtValueRangeStart.TabIndex = 5
        Me.txtValueRangeStart.Text = "0.000000"
        Me.txtValueRangeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtValueRangeStart.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblValueRangeWindow
        '
        Me.lblValueRangeWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblValueRangeWindow.AutoSize = True
        Me.lblValueRangeWindow.Location = New System.Drawing.Point(234, 340)
        Me.lblValueRangeWindow.Name = "lblValueRangeWindow"
        Me.lblValueRangeWindow.Size = New System.Drawing.Size(96, 13)
        Me.lblValueRangeWindow.TabIndex = 6
        Me.lblValueRangeWindow.Text = "with window width:"
        '
        'txtValueRangeWindow
        '
        Me.txtValueRangeWindow.AllowZero = False
        Me.txtValueRangeWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtValueRangeWindow.BackColor = System.Drawing.Color.White
        Me.txtValueRangeWindow.ForeColor = System.Drawing.Color.Black
        Me.txtValueRangeWindow.FormatDecimalPlaces = 6
        Me.txtValueRangeWindow.Location = New System.Drawing.Point(336, 337)
        Me.txtValueRangeWindow.Name = "txtValueRangeWindow"
        Me.txtValueRangeWindow.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtValueRangeWindow.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtValueRangeWindow.Size = New System.Drawing.Size(100, 20)
        Me.txtValueRangeWindow.TabIndex = 5
        Me.txtValueRangeWindow.Text = "0.000000"
        Me.txtValueRangeWindow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtValueRangeWindow.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'gbExport
        '
        Me.gbExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbExport.Controls.Add(Me.btnExport_SaveAsXYZ)
        Me.gbExport.Location = New System.Drawing.Point(774, 2)
        Me.gbExport.Name = "gbExport"
        Me.gbExport.Size = New System.Drawing.Size(171, 66)
        Me.gbExport.TabIndex = 6
        Me.gbExport.TabStop = False
        Me.gbExport.Text = "export"
        '
        'btnExport_SaveAsXYZ
        '
        Me.btnExport_SaveAsXYZ.Image = Global.SpectroscopyManager.My.Resources.Resources.export_25
        Me.btnExport_SaveAsXYZ.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport_SaveAsXYZ.Location = New System.Drawing.Point(6, 20)
        Me.btnExport_SaveAsXYZ.Name = "btnExport_SaveAsXYZ"
        Me.btnExport_SaveAsXYZ.Size = New System.Drawing.Size(159, 35)
        Me.btnExport_SaveAsXYZ.TabIndex = 0
        Me.btnExport_SaveAsXYZ.Text = "save as xyz-file"
        Me.btnExport_SaveAsXYZ.UseVisualStyleBackColor = True
        '
        'gbOutput
        '
        Me.gbOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOutput.Controls.Add(Me.svOutputImage)
        Me.gbOutput.Location = New System.Drawing.Point(3, 2)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Size = New System.Drawing.Size(517, 517)
        Me.gbOutput.TabIndex = 2
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "local spectral intensity:"
        '
        'svOutputImage
        '
        Me.svOutputImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svOutputImage.Location = New System.Drawing.Point(3, 16)
        Me.svOutputImage.Name = "svOutputImage"
        Me.svOutputImage.Size = New System.Drawing.Size(511, 498)
        Me.svOutputImage.TabIndex = 0
        '
        'gbPlotSettings
        '
        Me.gbPlotSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPlotSettings.Controls.Add(Me.vrsPlotSettings_ColorScaling)
        Me.gbPlotSettings.Controls.Add(Me.cpPlotSettings_ColorCode)
        Me.gbPlotSettings.Controls.Add(Me.txtPlotSettings_PointDiameter)
        Me.gbPlotSettings.Controls.Add(Me.lblPlotSettings_ColorCode)
        Me.gbPlotSettings.Controls.Add(Me.lblPlotSettings_DataPointDiameter)
        Me.gbPlotSettings.Location = New System.Drawing.Point(526, 2)
        Me.gbPlotSettings.Name = "gbPlotSettings"
        Me.gbPlotSettings.Size = New System.Drawing.Size(242, 517)
        Me.gbPlotSettings.TabIndex = 5
        Me.gbPlotSettings.TabStop = False
        Me.gbPlotSettings.Text = "grid plot settings"
        '
        'vrsPlotSettings_ColorScaling
        '
        Me.vrsPlotSettings_ColorScaling.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vrsPlotSettings_ColorScaling.Location = New System.Drawing.Point(15, 91)
        Me.vrsPlotSettings_ColorScaling.Name = "vrsPlotSettings_ColorScaling"
        Me.vrsPlotSettings_ColorScaling.Size = New System.Drawing.Size(213, 420)
        Me.vrsPlotSettings_ColorScaling.TabIndex = 4
        '
        'cpPlotSettings_ColorCode
        '
        Me.cpPlotSettings_ColorCode.Location = New System.Drawing.Point(17, 62)
        Me.cpPlotSettings_ColorCode.Name = "cpPlotSettings_ColorCode"
        Me.cpPlotSettings_ColorCode.Size = New System.Drawing.Size(212, 23)
        Me.cpPlotSettings_ColorCode.TabIndex = 5
        '
        'txtPlotSettings_PointDiameter
        '
        Me.txtPlotSettings_PointDiameter.AllowZero = True
        Me.txtPlotSettings_PointDiameter.BackColor = System.Drawing.Color.Green
        Me.txtPlotSettings_PointDiameter.ForeColor = System.Drawing.Color.White
        Me.txtPlotSettings_PointDiameter.FormatDecimalPlaces = 6
        Me.txtPlotSettings_PointDiameter.Location = New System.Drawing.Point(123, 19)
        Me.txtPlotSettings_PointDiameter.Name = "txtPlotSettings_PointDiameter"
        Me.txtPlotSettings_PointDiameter.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtPlotSettings_PointDiameter.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtPlotSettings_PointDiameter.Size = New System.Drawing.Size(92, 20)
        Me.txtPlotSettings_PointDiameter.TabIndex = 1
        Me.txtPlotSettings_PointDiameter.Text = "0.000000"
        Me.txtPlotSettings_PointDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPlotSettings_PointDiameter.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblPlotSettings_ColorCode
        '
        Me.lblPlotSettings_ColorCode.AutoSize = True
        Me.lblPlotSettings_ColorCode.Location = New System.Drawing.Point(17, 42)
        Me.lblPlotSettings_ColorCode.Name = "lblPlotSettings_ColorCode"
        Me.lblPlotSettings_ColorCode.Size = New System.Drawing.Size(60, 13)
        Me.lblPlotSettings_ColorCode.TabIndex = 0
        Me.lblPlotSettings_ColorCode.Text = "color code:"
        '
        'lblPlotSettings_DataPointDiameter
        '
        Me.lblPlotSettings_DataPointDiameter.AutoSize = True
        Me.lblPlotSettings_DataPointDiameter.Location = New System.Drawing.Point(17, 22)
        Me.lblPlotSettings_DataPointDiameter.Name = "lblPlotSettings_DataPointDiameter"
        Me.lblPlotSettings_DataPointDiameter.Size = New System.Drawing.Size(100, 13)
        Me.lblPlotSettings_DataPointDiameter.TabIndex = 0
        Me.lblPlotSettings_DataPointDiameter.Text = "data point diameter:"
        '
        'ssProgress
        '
        Me.ssProgress.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.pgProgress, Me.lblProgress})
        Me.ssProgress.Location = New System.Drawing.Point(0, 919)
        Me.ssProgress.Name = "ssProgress"
        Me.ssProgress.Size = New System.Drawing.Size(1206, 22)
        Me.ssProgress.TabIndex = 3
        Me.ssProgress.Text = "StatusStrip1"
        '
        'pgProgress
        '
        Me.pgProgress.Name = "pgProgress"
        Me.pgProgress.Size = New System.Drawing.Size(150, 16)
        Me.pgProgress.Visible = False
        '
        'lblProgress
        '
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(12, 17)
        Me.lblProgress.Text = "-"
        Me.lblProgress.Visible = False
        '
        'wGridPlotter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1206, 941)
        Me.Controls.Add(Me.scDataSelector)
        Me.Controls.Add(Me.ssProgress)
        Me.Name = "wGridPlotter"
        Me.Text = "Spectral Intensity Map Plotter"
        Me.scDataSelector.Panel1.ResumeLayout(False)
        Me.scDataSelector.Panel2.ResumeLayout(False)
        CType(Me.scDataSelector, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scDataSelector.ResumeLayout(False)
        Me.gbScanImages.ResumeLayout(False)
        Me.gbDataSelector.ResumeLayout(False)
        Me.scGridPlotter.Panel1.ResumeLayout(False)
        Me.scGridPlotter.Panel2.ResumeLayout(False)
        CType(Me.scGridPlotter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scGridPlotter.ResumeLayout(False)
        Me.gbDataRange.ResumeLayout(False)
        Me.gbDataRange.PerformLayout()
        Me.gbExport.ResumeLayout(False)
        Me.gbOutput.ResumeLayout(False)
        Me.gbPlotSettings.ResumeLayout(False)
        Me.gbPlotSettings.PerformLayout()
        Me.ssProgress.ResumeLayout(False)
        Me.ssProgress.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents scDataSelector As SplitContainer
    Friend WithEvents gbDataSelector As GroupBox
    Friend WithEvents gbScanImages As GroupBox
    Friend WithEvents dsSpectroscopyFiles As mDataBrowserListCompact
    Friend WithEvents dsScanImages As mDataBrowserListCompact
    Friend WithEvents gbOutput As GroupBox
    Friend WithEvents pbDataRangeSelector As mSpectroscopyTableViewer
    Friend WithEvents gbDataRange As GroupBox
    Friend WithEvents lblValueRangeWindow As Label
    Friend WithEvents txtValueRangeWindow As NumericTextbox
    Friend WithEvents btnLoadData As Button
    Friend WithEvents ssProgress As StatusStrip
    Friend WithEvents pgProgress As ToolStripProgressBar
    Friend WithEvents lblProgress As ToolStripStatusLabel
    Friend WithEvents lblValueRangeStart As Label
    Friend WithEvents txtValueRangeStart As NumericTextbox
    Friend WithEvents btnSelectValueRangeWindow As Button
    Friend WithEvents btnSelectValueRangeStart As Button
    Friend WithEvents gbPlotSettings As GroupBox
    Friend WithEvents txtPlotSettings_PointDiameter As NumericTextbox
    Friend WithEvents lblPlotSettings_DataPointDiameter As Label
    Friend WithEvents vrsPlotSettings_ColorScaling As mValueRangeSelector
    Friend WithEvents cpPlotSettings_ColorCode As ucColorPalettePicker
    Friend WithEvents lblPlotSettings_ColorCode As Label
    Friend WithEvents spDataRangeSelector_DataToDisplay As ucSpectroscopyTableSelector
    Friend WithEvents lblValueRange_SelectPlotData As Label
    Friend WithEvents svOutputImage As mScanImageViewer
    Friend WithEvents gbExport As GroupBox
    Friend WithEvents scGridPlotter As SplitContainer
    Friend WithEvents btnExport_SaveAsXYZ As Button
End Class
