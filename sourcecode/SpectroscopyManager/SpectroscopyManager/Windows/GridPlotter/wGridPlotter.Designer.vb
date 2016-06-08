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
        Me.tcGridFilesSelector = New System.Windows.Forms.TabControl()
        Me.tpIndividualSpectroscopyFiles = New System.Windows.Forms.TabPage()
        Me.dsSpectroscopyFiles = New SpectroscopyManager.mDataBrowserListCompact()
        Me.tpGridFiles = New System.Windows.Forms.TabPage()
        Me.dsGridFiles = New SpectroscopyManager.mDataBrowserListCompact()
        Me.btnLoadData = New System.Windows.Forms.Button()
        Me.gbScanImages = New System.Windows.Forms.GroupBox()
        Me.dsScanImages = New SpectroscopyManager.mDataBrowserListCompact()
        Me.scGridPlotter = New System.Windows.Forms.SplitContainer()
        Me.gbDataRange = New System.Windows.Forms.GroupBox()
        Me.scDataPreview = New System.Windows.Forms.SplitContainer()
        Me.btnSelectValueRangeWindow = New System.Windows.Forms.Button()
        Me.pbDataRangeSelector = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.txtValueRangeWindow = New SpectroscopyManager.NumericTextbox()
        Me.btnSelectValueRangeStart = New System.Windows.Forms.Button()
        Me.lblValueRangeWindow = New System.Windows.Forms.Label()
        Me.lblValueRangeStart = New System.Windows.Forms.Label()
        Me.txtValueRangeStart = New SpectroscopyManager.NumericTextbox()
        Me.spDataRangeSelector_DataToDisplay = New SpectroscopyManager.ucSpectroscopyTableSelector()
        Me.lblValueRange_SelectPlotData = New System.Windows.Forms.Label()
        Me.gbGenerateGIF = New System.Windows.Forms.GroupBox()
        Me.lblGIFEndValue = New System.Windows.Forms.Label()
        Me.lblGIFStartValue = New System.Windows.Forms.Label()
        Me.lblAnimationTime = New System.Windows.Forms.Label()
        Me.txtGIFEndValue = New SpectroscopyManager.NumericTextbox()
        Me.txtGIFStartValue = New SpectroscopyManager.NumericTextbox()
        Me.txtAnimationTime = New SpectroscopyManager.NumericTextbox()
        Me.btnGenerateGIF = New System.Windows.Forms.Button()
        Me.gbExport = New System.Windows.Forms.GroupBox()
        Me.btnExport_SaveAsXYZ = New System.Windows.Forms.Button()
        Me.gbOutput = New System.Windows.Forms.GroupBox()
        Me.svOutputImage = New SpectroscopyManager.mScanImageViewer()
        Me.gbPlotSettings = New System.Windows.Forms.GroupBox()
        Me.ckbGIFKeepValueRangeConstant = New System.Windows.Forms.CheckBox()
        Me.ckbPlotSettings_BiasIndicatorSize = New System.Windows.Forms.CheckBox()
        Me.vrsPlotSettings_ColorScaling = New SpectroscopyManager.mValueRangeSelector()
        Me.cpPlotSettings_ColorCode = New SpectroscopyManager.ucColorPalettePicker()
        Me.txtPlotSettings_BiasIndicatorSize = New SpectroscopyManager.NumericTextbox()
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
        Me.tcGridFilesSelector.SuspendLayout()
        Me.tpIndividualSpectroscopyFiles.SuspendLayout()
        Me.tpGridFiles.SuspendLayout()
        Me.gbScanImages.SuspendLayout()
        CType(Me.scGridPlotter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scGridPlotter.Panel1.SuspendLayout()
        Me.scGridPlotter.Panel2.SuspendLayout()
        Me.scGridPlotter.SuspendLayout()
        Me.gbDataRange.SuspendLayout()
        CType(Me.scDataPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scDataPreview.Panel1.SuspendLayout()
        Me.scDataPreview.Panel2.SuspendLayout()
        Me.scDataPreview.SuspendLayout()
        Me.gbGenerateGIF.SuspendLayout()
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
        Me.scDataSelector.Panel1.Controls.Add(Me.tcGridFilesSelector)
        Me.scDataSelector.Panel1.Controls.Add(Me.btnLoadData)
        Me.scDataSelector.Panel1.Controls.Add(Me.gbScanImages)
        '
        'scDataSelector.Panel2
        '
        Me.scDataSelector.Panel2.Controls.Add(Me.scGridPlotter)
        Me.scDataSelector.Size = New System.Drawing.Size(1219, 916)
        Me.scDataSelector.SplitterDistance = 235
        Me.scDataSelector.TabIndex = 0
        '
        'tcGridFilesSelector
        '
        Me.tcGridFilesSelector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcGridFilesSelector.Controls.Add(Me.tpIndividualSpectroscopyFiles)
        Me.tcGridFilesSelector.Controls.Add(Me.tpGridFiles)
        Me.tcGridFilesSelector.Location = New System.Drawing.Point(3, 46)
        Me.tcGridFilesSelector.Name = "tcGridFilesSelector"
        Me.tcGridFilesSelector.SelectedIndex = 0
        Me.tcGridFilesSelector.Size = New System.Drawing.Size(225, 608)
        Me.tcGridFilesSelector.TabIndex = 20
        '
        'tpIndividualSpectroscopyFiles
        '
        Me.tpIndividualSpectroscopyFiles.Controls.Add(Me.dsSpectroscopyFiles)
        Me.tpIndividualSpectroscopyFiles.Location = New System.Drawing.Point(4, 22)
        Me.tpIndividualSpectroscopyFiles.Name = "tpIndividualSpectroscopyFiles"
        Me.tpIndividualSpectroscopyFiles.Padding = New System.Windows.Forms.Padding(3)
        Me.tpIndividualSpectroscopyFiles.Size = New System.Drawing.Size(217, 582)
        Me.tpIndividualSpectroscopyFiles.TabIndex = 0
        Me.tpIndividualSpectroscopyFiles.Text = "individual spectroscopy files"
        Me.tpIndividualSpectroscopyFiles.UseVisualStyleBackColor = True
        '
        'dsSpectroscopyFiles
        '
        Me.dsSpectroscopyFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dsSpectroscopyFiles.Location = New System.Drawing.Point(3, 3)
        Me.dsSpectroscopyFiles.Name = "dsSpectroscopyFiles"
        Me.dsSpectroscopyFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.dsSpectroscopyFiles.ShowGridFiles = False
        Me.dsSpectroscopyFiles.ShowScanImages = False
        Me.dsSpectroscopyFiles.ShowSpectroscopyTables = True
        Me.dsSpectroscopyFiles.Size = New System.Drawing.Size(211, 576)
        Me.dsSpectroscopyFiles.TabIndex = 21
        '
        'tpGridFiles
        '
        Me.tpGridFiles.Controls.Add(Me.dsGridFiles)
        Me.tpGridFiles.Location = New System.Drawing.Point(4, 22)
        Me.tpGridFiles.Name = "tpGridFiles"
        Me.tpGridFiles.Padding = New System.Windows.Forms.Padding(3)
        Me.tpGridFiles.Size = New System.Drawing.Size(217, 582)
        Me.tpGridFiles.TabIndex = 1
        Me.tpGridFiles.Text = "grid files"
        Me.tpGridFiles.UseVisualStyleBackColor = True
        '
        'dsGridFiles
        '
        Me.dsGridFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dsGridFiles.Location = New System.Drawing.Point(3, 3)
        Me.dsGridFiles.Name = "dsGridFiles"
        Me.dsGridFiles.SelectionMode = System.Windows.Forms.SelectionMode.One
        Me.dsGridFiles.ShowGridFiles = True
        Me.dsGridFiles.ShowScanImages = False
        Me.dsGridFiles.ShowSpectroscopyTables = False
        Me.dsGridFiles.Size = New System.Drawing.Size(211, 576)
        Me.dsGridFiles.TabIndex = 22
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
        Me.btnLoadData.Size = New System.Drawing.Size(226, 37)
        Me.btnLoadData.TabIndex = 10
        Me.btnLoadData.Text = "load selected data set"
        Me.btnLoadData.UseVisualStyleBackColor = True
        '
        'gbScanImages
        '
        Me.gbScanImages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbScanImages.Controls.Add(Me.dsScanImages)
        Me.gbScanImages.Location = New System.Drawing.Point(1, 657)
        Me.gbScanImages.Name = "gbScanImages"
        Me.gbScanImages.Size = New System.Drawing.Size(232, 256)
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
        Me.dsScanImages.ShowGridFiles = False
        Me.dsScanImages.ShowScanImages = True
        Me.dsScanImages.ShowSpectroscopyTables = False
        Me.dsScanImages.Size = New System.Drawing.Size(226, 237)
        Me.dsScanImages.TabIndex = 30
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
        Me.scGridPlotter.Panel2.Controls.Add(Me.gbGenerateGIF)
        Me.scGridPlotter.Panel2.Controls.Add(Me.gbExport)
        Me.scGridPlotter.Panel2.Controls.Add(Me.gbOutput)
        Me.scGridPlotter.Panel2.Controls.Add(Me.gbPlotSettings)
        Me.scGridPlotter.Size = New System.Drawing.Size(980, 916)
        Me.scGridPlotter.SplitterDistance = 391
        Me.scGridPlotter.TabIndex = 7
        '
        'gbDataRange
        '
        Me.gbDataRange.Controls.Add(Me.scDataPreview)
        Me.gbDataRange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbDataRange.Enabled = False
        Me.gbDataRange.Location = New System.Drawing.Point(0, 0)
        Me.gbDataRange.Name = "gbDataRange"
        Me.gbDataRange.Size = New System.Drawing.Size(980, 391)
        Me.gbDataRange.TabIndex = 4
        Me.gbDataRange.TabStop = False
        Me.gbDataRange.Text = "select data range to display:"
        '
        'scDataPreview
        '
        Me.scDataPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scDataPreview.Location = New System.Drawing.Point(3, 16)
        Me.scDataPreview.Name = "scDataPreview"
        '
        'scDataPreview.Panel1
        '
        Me.scDataPreview.Panel1.Controls.Add(Me.btnSelectValueRangeWindow)
        Me.scDataPreview.Panel1.Controls.Add(Me.pbDataRangeSelector)
        Me.scDataPreview.Panel1.Controls.Add(Me.txtValueRangeWindow)
        Me.scDataPreview.Panel1.Controls.Add(Me.btnSelectValueRangeStart)
        Me.scDataPreview.Panel1.Controls.Add(Me.lblValueRangeWindow)
        Me.scDataPreview.Panel1.Controls.Add(Me.lblValueRangeStart)
        Me.scDataPreview.Panel1.Controls.Add(Me.txtValueRangeStart)
        '
        'scDataPreview.Panel2
        '
        Me.scDataPreview.Panel2.Controls.Add(Me.spDataRangeSelector_DataToDisplay)
        Me.scDataPreview.Panel2.Controls.Add(Me.lblValueRange_SelectPlotData)
        Me.scDataPreview.Size = New System.Drawing.Size(974, 372)
        Me.scDataPreview.SplitterDistance = 754
        Me.scDataPreview.TabIndex = 9
        '
        'btnSelectValueRangeWindow
        '
        Me.btnSelectValueRangeWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectValueRangeWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnSelectValueRangeWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectValueRangeWindow.Location = New System.Drawing.Point(322, 343)
        Me.btnSelectValueRangeWindow.Name = "btnSelectValueRangeWindow"
        Me.btnSelectValueRangeWindow.Size = New System.Drawing.Size(100, 23)
        Me.btnSelectValueRangeWindow.TabIndex = 44
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
        Me.pbDataRangeSelector.Location = New System.Drawing.Point(3, 3)
        Me.pbDataRangeSelector.MultipleSpectraStackOffset = 0R
        Me.pbDataRangeSelector.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbDataRangeSelector.Name = "pbDataRangeSelector"
        Me.pbDataRangeSelector.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbDataRangeSelector.ShowColumnSelectors = True
        Me.pbDataRangeSelector.Size = New System.Drawing.Size(748, 312)
        Me.pbDataRangeSelector.TabIndex = 40
        Me.pbDataRangeSelector.TurnOnLastFilterSaving_Y = False
        Me.pbDataRangeSelector.TurnOnLastSelectionSaving_Y = False
        '
        'txtValueRangeWindow
        '
        Me.txtValueRangeWindow.AllowZero = False
        Me.txtValueRangeWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtValueRangeWindow.BackColor = System.Drawing.Color.White
        Me.txtValueRangeWindow.ForeColor = System.Drawing.Color.Black
        Me.txtValueRangeWindow.FormatDecimalPlaces = 6
        Me.txtValueRangeWindow.Location = New System.Drawing.Point(322, 321)
        Me.txtValueRangeWindow.Name = "txtValueRangeWindow"
        Me.txtValueRangeWindow.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtValueRangeWindow.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtValueRangeWindow.Size = New System.Drawing.Size(100, 20)
        Me.txtValueRangeWindow.TabIndex = 43
        Me.txtValueRangeWindow.Text = "0.000000"
        Me.txtValueRangeWindow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtValueRangeWindow.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'btnSelectValueRangeStart
        '
        Me.btnSelectValueRangeStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectValueRangeStart.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectValueRangeStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectValueRangeStart.Location = New System.Drawing.Point(105, 343)
        Me.btnSelectValueRangeStart.Name = "btnSelectValueRangeStart"
        Me.btnSelectValueRangeStart.Size = New System.Drawing.Size(100, 23)
        Me.btnSelectValueRangeStart.TabIndex = 42
        Me.btnSelectValueRangeStart.Text = "select in graph"
        Me.btnSelectValueRangeStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSelectValueRangeStart.UseVisualStyleBackColor = True
        '
        'lblValueRangeWindow
        '
        Me.lblValueRangeWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblValueRangeWindow.AutoSize = True
        Me.lblValueRangeWindow.Location = New System.Drawing.Point(220, 324)
        Me.lblValueRangeWindow.Name = "lblValueRangeWindow"
        Me.lblValueRangeWindow.Size = New System.Drawing.Size(96, 13)
        Me.lblValueRangeWindow.TabIndex = 6
        Me.lblValueRangeWindow.Text = "with window width:"
        '
        'lblValueRangeStart
        '
        Me.lblValueRangeStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblValueRangeStart.AutoSize = True
        Me.lblValueRangeStart.Location = New System.Drawing.Point(10, 324)
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
        Me.txtValueRangeStart.Location = New System.Drawing.Point(105, 321)
        Me.txtValueRangeStart.Name = "txtValueRangeStart"
        Me.txtValueRangeStart.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtValueRangeStart.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtValueRangeStart.Size = New System.Drawing.Size(100, 20)
        Me.txtValueRangeStart.TabIndex = 41
        Me.txtValueRangeStart.Text = "0.000000"
        Me.txtValueRangeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtValueRangeStart.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'spDataRangeSelector_DataToDisplay
        '
        Me.spDataRangeSelector_DataToDisplay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.spDataRangeSelector_DataToDisplay.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Listbox
        Me.spDataRangeSelector_DataToDisplay.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.spDataRangeSelector_DataToDisplay.Location = New System.Drawing.Point(5, 19)
        Me.spDataRangeSelector_DataToDisplay.MinimumSize = New System.Drawing.Size(0, 21)
        Me.spDataRangeSelector_DataToDisplay.Name = "spDataRangeSelector_DataToDisplay"
        Me.spDataRangeSelector_DataToDisplay.SelectedEntries = CType(resources.GetObject("spDataRangeSelector_DataToDisplay.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.spDataRangeSelector_DataToDisplay.SelectedEntry = ""
        Me.spDataRangeSelector_DataToDisplay.SelectedTable = ""
        Me.spDataRangeSelector_DataToDisplay.SelectedTables = CType(resources.GetObject("spDataRangeSelector_DataToDisplay.SelectedTables"), System.Collections.Generic.List(Of String))
        Me.spDataRangeSelector_DataToDisplay.Size = New System.Drawing.Size(208, 312)
        Me.spDataRangeSelector_DataToDisplay.TabIndex = 50
        Me.spDataRangeSelector_DataToDisplay.TurnOnLastFilterSaving = False
        Me.spDataRangeSelector_DataToDisplay.TurnOnLastSelectionSaving = False
        '
        'lblValueRange_SelectPlotData
        '
        Me.lblValueRange_SelectPlotData.AutoSize = True
        Me.lblValueRange_SelectPlotData.Location = New System.Drawing.Point(2, 3)
        Me.lblValueRange_SelectPlotData.Name = "lblValueRange_SelectPlotData"
        Me.lblValueRange_SelectPlotData.Size = New System.Drawing.Size(171, 13)
        Me.lblValueRange_SelectPlotData.TabIndex = 6
        Me.lblValueRange_SelectPlotData.Text = "select data to show in the preview:"
        '
        'gbGenerateGIF
        '
        Me.gbGenerateGIF.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbGenerateGIF.Controls.Add(Me.lblGIFEndValue)
        Me.gbGenerateGIF.Controls.Add(Me.lblGIFStartValue)
        Me.gbGenerateGIF.Controls.Add(Me.lblAnimationTime)
        Me.gbGenerateGIF.Controls.Add(Me.txtGIFEndValue)
        Me.gbGenerateGIF.Controls.Add(Me.txtGIFStartValue)
        Me.gbGenerateGIF.Controls.Add(Me.txtAnimationTime)
        Me.gbGenerateGIF.Controls.Add(Me.btnGenerateGIF)
        Me.gbGenerateGIF.Location = New System.Drawing.Point(785, 77)
        Me.gbGenerateGIF.Name = "gbGenerateGIF"
        Me.gbGenerateGIF.Size = New System.Drawing.Size(189, 118)
        Me.gbGenerateGIF.TabIndex = 10
        Me.gbGenerateGIF.TabStop = False
        Me.gbGenerateGIF.Text = "generate animated GIF:"
        '
        'lblGIFEndValue
        '
        Me.lblGIFEndValue.AutoSize = True
        Me.lblGIFEndValue.Location = New System.Drawing.Point(9, 63)
        Me.lblGIFEndValue.Name = "lblGIFEndValue"
        Me.lblGIFEndValue.Size = New System.Drawing.Size(76, 13)
        Me.lblGIFEndValue.TabIndex = 11
        Me.lblGIFEndValue.Text = "end value in x:"
        '
        'lblGIFStartValue
        '
        Me.lblGIFStartValue.AutoSize = True
        Me.lblGIFStartValue.Location = New System.Drawing.Point(7, 41)
        Me.lblGIFStartValue.Name = "lblGIFStartValue"
        Me.lblGIFStartValue.Size = New System.Drawing.Size(78, 13)
        Me.lblGIFStartValue.TabIndex = 11
        Me.lblGIFStartValue.Text = "start value in x:"
        '
        'lblAnimationTime
        '
        Me.lblAnimationTime.AutoSize = True
        Me.lblAnimationTime.Location = New System.Drawing.Point(11, 20)
        Me.lblAnimationTime.Name = "lblAnimationTime"
        Me.lblAnimationTime.Size = New System.Drawing.Size(74, 13)
        Me.lblAnimationTime.TabIndex = 10
        Me.lblAnimationTime.Text = "total time (ms):"
        '
        'txtGIFEndValue
        '
        Me.txtGIFEndValue.AllowZero = True
        Me.txtGIFEndValue.BackColor = System.Drawing.Color.White
        Me.txtGIFEndValue.ForeColor = System.Drawing.Color.Black
        Me.txtGIFEndValue.FormatDecimalPlaces = 6
        Me.txtGIFEndValue.Location = New System.Drawing.Point(87, 61)
        Me.txtGIFEndValue.Name = "txtGIFEndValue"
        Me.txtGIFEndValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtGIFEndValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtGIFEndValue.Size = New System.Drawing.Size(96, 20)
        Me.txtGIFEndValue.TabIndex = 92
        Me.txtGIFEndValue.Text = "0.000000"
        Me.txtGIFEndValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGIFEndValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtGIFStartValue
        '
        Me.txtGIFStartValue.AllowZero = True
        Me.txtGIFStartValue.BackColor = System.Drawing.Color.White
        Me.txtGIFStartValue.ForeColor = System.Drawing.Color.Black
        Me.txtGIFStartValue.FormatDecimalPlaces = 6
        Me.txtGIFStartValue.Location = New System.Drawing.Point(87, 39)
        Me.txtGIFStartValue.Name = "txtGIFStartValue"
        Me.txtGIFStartValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtGIFStartValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtGIFStartValue.Size = New System.Drawing.Size(96, 20)
        Me.txtGIFStartValue.TabIndex = 91
        Me.txtGIFStartValue.Text = "0.000000"
        Me.txtGIFStartValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGIFStartValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtAnimationTime
        '
        Me.txtAnimationTime.AllowZero = True
        Me.txtAnimationTime.BackColor = System.Drawing.Color.White
        Me.txtAnimationTime.ForeColor = System.Drawing.Color.Black
        Me.txtAnimationTime.FormatDecimalPlaces = 6
        Me.txtAnimationTime.Location = New System.Drawing.Point(87, 17)
        Me.txtAnimationTime.Name = "txtAnimationTime"
        Me.txtAnimationTime.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtAnimationTime.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtAnimationTime.Size = New System.Drawing.Size(96, 20)
        Me.txtAnimationTime.TabIndex = 90
        Me.txtAnimationTime.Text = "0.000000"
        Me.txtAnimationTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAnimationTime.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'btnGenerateGIF
        '
        Me.btnGenerateGIF.Image = Global.SpectroscopyManager.My.Resources.Resources.export_16
        Me.btnGenerateGIF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerateGIF.Location = New System.Drawing.Point(10, 86)
        Me.btnGenerateGIF.Name = "btnGenerateGIF"
        Me.btnGenerateGIF.Size = New System.Drawing.Size(173, 23)
        Me.btnGenerateGIF.TabIndex = 93
        Me.btnGenerateGIF.Text = "generate GIF"
        Me.btnGenerateGIF.UseVisualStyleBackColor = True
        '
        'gbExport
        '
        Me.gbExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbExport.Controls.Add(Me.btnExport_SaveAsXYZ)
        Me.gbExport.Location = New System.Drawing.Point(785, 2)
        Me.gbExport.Name = "gbExport"
        Me.gbExport.Size = New System.Drawing.Size(189, 68)
        Me.gbExport.TabIndex = 6
        Me.gbExport.TabStop = False
        Me.gbExport.Text = "export current data:"
        '
        'btnExport_SaveAsXYZ
        '
        Me.btnExport_SaveAsXYZ.Image = Global.SpectroscopyManager.My.Resources.Resources.export_25
        Me.btnExport_SaveAsXYZ.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport_SaveAsXYZ.Location = New System.Drawing.Point(6, 20)
        Me.btnExport_SaveAsXYZ.Name = "btnExport_SaveAsXYZ"
        Me.btnExport_SaveAsXYZ.Size = New System.Drawing.Size(177, 35)
        Me.btnExport_SaveAsXYZ.TabIndex = 80
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
        Me.gbOutput.Size = New System.Drawing.Size(528, 516)
        Me.gbOutput.TabIndex = 2
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "local spectral intensity:"
        '
        'svOutputImage
        '
        Me.svOutputImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svOutputImage.Location = New System.Drawing.Point(3, 16)
        Me.svOutputImage.Name = "svOutputImage"
        Me.svOutputImage.SelectedPoints_PixelCoordinate = CType(resources.GetObject("svOutputImage.SelectedPoints_PixelCoordinate"), System.Collections.Generic.List(Of System.Drawing.Point))
        Me.svOutputImage.SelectionMode = SpectroscopyManager.mScanImageViewer.SelectionModes.None
        Me.svOutputImage.Size = New System.Drawing.Size(522, 497)
        Me.svOutputImage.TabIndex = 60
        '
        'gbPlotSettings
        '
        Me.gbPlotSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPlotSettings.Controls.Add(Me.ckbGIFKeepValueRangeConstant)
        Me.gbPlotSettings.Controls.Add(Me.ckbPlotSettings_BiasIndicatorSize)
        Me.gbPlotSettings.Controls.Add(Me.vrsPlotSettings_ColorScaling)
        Me.gbPlotSettings.Controls.Add(Me.cpPlotSettings_ColorCode)
        Me.gbPlotSettings.Controls.Add(Me.txtPlotSettings_BiasIndicatorSize)
        Me.gbPlotSettings.Controls.Add(Me.txtPlotSettings_PointDiameter)
        Me.gbPlotSettings.Controls.Add(Me.lblPlotSettings_ColorCode)
        Me.gbPlotSettings.Controls.Add(Me.lblPlotSettings_DataPointDiameter)
        Me.gbPlotSettings.Location = New System.Drawing.Point(537, 2)
        Me.gbPlotSettings.Name = "gbPlotSettings"
        Me.gbPlotSettings.Size = New System.Drawing.Size(242, 516)
        Me.gbPlotSettings.TabIndex = 5
        Me.gbPlotSettings.TabStop = False
        Me.gbPlotSettings.Text = "grid plot settings"
        '
        'ckbGIFKeepValueRangeConstant
        '
        Me.ckbGIFKeepValueRangeConstant.AutoSize = True
        Me.ckbGIFKeepValueRangeConstant.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ckbGIFKeepValueRangeConstant.Location = New System.Drawing.Point(15, 139)
        Me.ckbGIFKeepValueRangeConstant.Name = "ckbGIFKeepValueRangeConstant"
        Me.ckbGIFKeepValueRangeConstant.Size = New System.Drawing.Size(198, 17)
        Me.ckbGIFKeepValueRangeConstant.TabIndex = 74
        Me.ckbGIFKeepValueRangeConstant.Text = "stop auto-adjustment of value range:"
        Me.ckbGIFKeepValueRangeConstant.UseVisualStyleBackColor = True
        '
        'ckbPlotSettings_BiasIndicatorSize
        '
        Me.ckbPlotSettings_BiasIndicatorSize.AutoSize = True
        Me.ckbPlotSettings_BiasIndicatorSize.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ckbPlotSettings_BiasIndicatorSize.Location = New System.Drawing.Point(15, 117)
        Me.ckbPlotSettings_BiasIndicatorSize.Name = "ckbPlotSettings_BiasIndicatorSize"
        Me.ckbPlotSettings_BiasIndicatorSize.Size = New System.Drawing.Size(136, 17)
        Me.ckbPlotSettings_BiasIndicatorSize.TabIndex = 72
        Me.ckbPlotSettings_BiasIndicatorSize.Text = "value indicator size (%):"
        Me.ckbPlotSettings_BiasIndicatorSize.UseVisualStyleBackColor = True
        '
        'vrsPlotSettings_ColorScaling
        '
        Me.vrsPlotSettings_ColorScaling.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vrsPlotSettings_ColorScaling.Location = New System.Drawing.Point(15, 161)
        Me.vrsPlotSettings_ColorScaling.Name = "vrsPlotSettings_ColorScaling"
        Me.vrsPlotSettings_ColorScaling.SelectedMaxValue = 0R
        Me.vrsPlotSettings_ColorScaling.SelectedMinValue = 0R
        Me.vrsPlotSettings_ColorScaling.Size = New System.Drawing.Size(213, 349)
        Me.vrsPlotSettings_ColorScaling.TabIndex = 75
        '
        'cpPlotSettings_ColorCode
        '
        Me.cpPlotSettings_ColorCode.Location = New System.Drawing.Point(12, 40)
        Me.cpPlotSettings_ColorCode.Name = "cpPlotSettings_ColorCode"
        Me.cpPlotSettings_ColorCode.Size = New System.Drawing.Size(212, 45)
        Me.cpPlotSettings_ColorCode.TabIndex = 70
        '
        'txtPlotSettings_BiasIndicatorSize
        '
        Me.txtPlotSettings_BiasIndicatorSize.AllowZero = True
        Me.txtPlotSettings_BiasIndicatorSize.BackColor = System.Drawing.Color.Green
        Me.txtPlotSettings_BiasIndicatorSize.ForeColor = System.Drawing.Color.White
        Me.txtPlotSettings_BiasIndicatorSize.FormatDecimalPlaces = 0
        Me.txtPlotSettings_BiasIndicatorSize.Location = New System.Drawing.Point(157, 115)
        Me.txtPlotSettings_BiasIndicatorSize.Name = "txtPlotSettings_BiasIndicatorSize"
        Me.txtPlotSettings_BiasIndicatorSize.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.PlainNumber
        Me.txtPlotSettings_BiasIndicatorSize.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtPlotSettings_BiasIndicatorSize.Size = New System.Drawing.Size(77, 20)
        Me.txtPlotSettings_BiasIndicatorSize.TabIndex = 73
        Me.txtPlotSettings_BiasIndicatorSize.Text = "0.000000"
        Me.txtPlotSettings_BiasIndicatorSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPlotSettings_BiasIndicatorSize.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.IntegerValue
        '
        'txtPlotSettings_PointDiameter
        '
        Me.txtPlotSettings_PointDiameter.AllowZero = True
        Me.txtPlotSettings_PointDiameter.BackColor = System.Drawing.Color.Green
        Me.txtPlotSettings_PointDiameter.ForeColor = System.Drawing.Color.White
        Me.txtPlotSettings_PointDiameter.FormatDecimalPlaces = 6
        Me.txtPlotSettings_PointDiameter.Location = New System.Drawing.Point(121, 94)
        Me.txtPlotSettings_PointDiameter.Name = "txtPlotSettings_PointDiameter"
        Me.txtPlotSettings_PointDiameter.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtPlotSettings_PointDiameter.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtPlotSettings_PointDiameter.Size = New System.Drawing.Size(113, 20)
        Me.txtPlotSettings_PointDiameter.TabIndex = 71
        Me.txtPlotSettings_PointDiameter.Text = "0.000000"
        Me.txtPlotSettings_PointDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPlotSettings_PointDiameter.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblPlotSettings_ColorCode
        '
        Me.lblPlotSettings_ColorCode.AutoSize = True
        Me.lblPlotSettings_ColorCode.Location = New System.Drawing.Point(12, 20)
        Me.lblPlotSettings_ColorCode.Name = "lblPlotSettings_ColorCode"
        Me.lblPlotSettings_ColorCode.Size = New System.Drawing.Size(60, 13)
        Me.lblPlotSettings_ColorCode.TabIndex = 0
        Me.lblPlotSettings_ColorCode.Text = "color code:"
        '
        'lblPlotSettings_DataPointDiameter
        '
        Me.lblPlotSettings_DataPointDiameter.AutoSize = True
        Me.lblPlotSettings_DataPointDiameter.Location = New System.Drawing.Point(15, 97)
        Me.lblPlotSettings_DataPointDiameter.Name = "lblPlotSettings_DataPointDiameter"
        Me.lblPlotSettings_DataPointDiameter.Size = New System.Drawing.Size(100, 13)
        Me.lblPlotSettings_DataPointDiameter.TabIndex = 0
        Me.lblPlotSettings_DataPointDiameter.Text = "data point diameter:"
        '
        'ssProgress
        '
        Me.ssProgress.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.pgProgress, Me.lblProgress})
        Me.ssProgress.Location = New System.Drawing.Point(0, 916)
        Me.ssProgress.Name = "ssProgress"
        Me.ssProgress.Size = New System.Drawing.Size(1219, 22)
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
        Me.ClientSize = New System.Drawing.Size(1219, 938)
        Me.Controls.Add(Me.scDataSelector)
        Me.Controls.Add(Me.ssProgress)
        Me.Name = "wGridPlotter"
        Me.Text = "Spectral Intensity Map Plotter"
        Me.scDataSelector.Panel1.ResumeLayout(False)
        Me.scDataSelector.Panel2.ResumeLayout(False)
        CType(Me.scDataSelector, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scDataSelector.ResumeLayout(False)
        Me.tcGridFilesSelector.ResumeLayout(False)
        Me.tpIndividualSpectroscopyFiles.ResumeLayout(False)
        Me.tpGridFiles.ResumeLayout(False)
        Me.gbScanImages.ResumeLayout(False)
        Me.scGridPlotter.Panel1.ResumeLayout(False)
        Me.scGridPlotter.Panel2.ResumeLayout(False)
        CType(Me.scGridPlotter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scGridPlotter.ResumeLayout(False)
        Me.gbDataRange.ResumeLayout(False)
        Me.scDataPreview.Panel1.ResumeLayout(False)
        Me.scDataPreview.Panel1.PerformLayout()
        Me.scDataPreview.Panel2.ResumeLayout(False)
        Me.scDataPreview.Panel2.PerformLayout()
        CType(Me.scDataPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scDataPreview.ResumeLayout(False)
        Me.gbGenerateGIF.ResumeLayout(False)
        Me.gbGenerateGIF.PerformLayout()
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
    Friend WithEvents gbScanImages As GroupBox
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
    Friend WithEvents tcGridFilesSelector As TabControl
    Friend WithEvents tpIndividualSpectroscopyFiles As TabPage
    Friend WithEvents dsSpectroscopyFiles As mDataBrowserListCompact
    Friend WithEvents tpGridFiles As TabPage
    Friend WithEvents dsGridFiles As mDataBrowserListCompact
    Friend WithEvents scDataPreview As SplitContainer
    Friend WithEvents btnGenerateGIF As Button
    Friend WithEvents gbGenerateGIF As GroupBox
    Friend WithEvents lblGIFEndValue As Label
    Friend WithEvents lblGIFStartValue As Label
    Friend WithEvents lblAnimationTime As Label
    Friend WithEvents txtGIFEndValue As NumericTextbox
    Friend WithEvents txtGIFStartValue As NumericTextbox
    Friend WithEvents txtAnimationTime As NumericTextbox
    Friend WithEvents txtPlotSettings_BiasIndicatorSize As NumericTextbox
    Friend WithEvents ckbPlotSettings_BiasIndicatorSize As CheckBox
    Friend WithEvents ckbGIFKeepValueRangeConstant As CheckBox
End Class
