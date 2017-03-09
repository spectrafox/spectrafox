<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataExplorer_SpectroscopyTable
    Inherits SpectroscopyManager.wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

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
        Me.tcSpectroscopyTable = New System.Windows.Forms.TabControl()
        Me.tpPlot = New System.Windows.Forms.TabPage()
        Me.scPlotBrowser = New System.Windows.Forms.SplitContainer()
        Me.svDataViewer = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.gbDataCollector = New System.Windows.Forms.GroupBox()
        Me.ckbDataCollector_UseTab = New System.Windows.Forms.CheckBox()
        Me.txtDataSelector_ValueSeparator = New System.Windows.Forms.TextBox()
        Me.btnDataSelector_ClearPoints = New System.Windows.Forms.Button()
        Me.rdbDataCollector_ShowVertically = New System.Windows.Forms.RadioButton()
        Me.rdbDataCollector_ShowHorizonally = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblDataCollector_Settings = New System.Windows.Forms.Label()
        Me.ckbDataCollector_ShowLabels = New System.Windows.Forms.CheckBox()
        Me.ckbDataCollector_ShowYPoints = New System.Windows.Forms.CheckBox()
        Me.ckbDataCollector_ShowXPoints = New System.Windows.Forms.CheckBox()
        Me.txtDataCollector_Output = New System.Windows.Forms.TextBox()
        Me.tpShow = New System.Windows.Forms.TabPage()
        Me.dtSpectroscopyTable = New SpectroscopyManager.mSpectroscopyTableDataTable()
        Me.tpAdditionalComment = New System.Windows.Forms.TabPage()
        Me.btnSaveAdditionalComment = New System.Windows.Forms.Button()
        Me.txtAdditionalComment = New System.Windows.Forms.TextBox()
        Me.tpGeneralProperties = New System.Windows.Forms.TabPage()
        Me.dtGeneralPropertyArray = New System.Windows.Forms.DataGridView()
        Me.colGeneralPropertyArray_Key = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colGeneralPropertyArray_Value = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.gbSpectraComment = New System.Windows.Forms.GroupBox()
        Me.txtSpectraComment = New System.Windows.Forms.TextBox()
        Me.gbSpectroscopyTablePropertyGrid = New System.Windows.Forms.GroupBox()
        Me.pgSpectrumProperies = New System.Windows.Forms.PropertyGrid()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.tcSpectroscopyTable.SuspendLayout()
        Me.tpPlot.SuspendLayout()
        CType(Me.scPlotBrowser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scPlotBrowser.Panel1.SuspendLayout()
        Me.scPlotBrowser.Panel2.SuspendLayout()
        Me.scPlotBrowser.SuspendLayout()
        Me.gbDataCollector.SuspendLayout()
        Me.tpShow.SuspendLayout()
        Me.tpAdditionalComment.SuspendLayout()
        Me.tpGeneralProperties.SuspendLayout()
        CType(Me.dtGeneralPropertyArray, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSpectraComment.SuspendLayout()
        Me.gbSpectroscopyTablePropertyGrid.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcSpectroscopyTable
        '
        Me.tcSpectroscopyTable.Controls.Add(Me.tpPlot)
        Me.tcSpectroscopyTable.Controls.Add(Me.tpShow)
        Me.tcSpectroscopyTable.Controls.Add(Me.tpAdditionalComment)
        Me.tcSpectroscopyTable.Controls.Add(Me.tpGeneralProperties)
        Me.tcSpectroscopyTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcSpectroscopyTable.Location = New System.Drawing.Point(0, 0)
        Me.tcSpectroscopyTable.Name = "tcSpectroscopyTable"
        Me.tcSpectroscopyTable.SelectedIndex = 0
        Me.tcSpectroscopyTable.Size = New System.Drawing.Size(1064, 842)
        Me.tcSpectroscopyTable.TabIndex = 0
        '
        'tpPlot
        '
        Me.tpPlot.Controls.Add(Me.scPlotBrowser)
        Me.tpPlot.Location = New System.Drawing.Point(4, 22)
        Me.tpPlot.Name = "tpPlot"
        Me.tpPlot.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPlot.Size = New System.Drawing.Size(1056, 816)
        Me.tpPlot.TabIndex = 0
        Me.tpPlot.Text = "plot of data"
        Me.tpPlot.UseVisualStyleBackColor = True
        '
        'scPlotBrowser
        '
        Me.scPlotBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scPlotBrowser.Location = New System.Drawing.Point(3, 3)
        Me.scPlotBrowser.Name = "scPlotBrowser"
        Me.scPlotBrowser.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scPlotBrowser.Panel1
        '
        Me.scPlotBrowser.Panel1.Controls.Add(Me.svDataViewer)
        '
        'scPlotBrowser.Panel2
        '
        Me.scPlotBrowser.Panel2.Controls.Add(Me.gbDataCollector)
        Me.scPlotBrowser.Size = New System.Drawing.Size(1050, 810)
        Me.scPlotBrowser.SplitterDistance = 651
        Me.scPlotBrowser.TabIndex = 1
        '
        'svDataViewer
        '
        Me.svDataViewer.AllowAdjustingXColumn = True
        Me.svDataViewer.AllowAdjustingYColumn = True
        Me.svDataViewer.AutomaticallyRestoreScaleAfterRedraw = True
        Me.svDataViewer.CallbackDataPointSelected = Nothing
        Me.svDataViewer.CallbackXRangeSelected = Nothing
        Me.svDataViewer.CallbackXValueSelected = Nothing
        Me.svDataViewer.CallbackXYRangeSelected = Nothing
        Me.svDataViewer.CallbackXYValueSelected = Nothing
        Me.svDataViewer.CallbackYRangeSelected = Nothing
        Me.svDataViewer.CallbackYValueSelected = Nothing
        Me.svDataViewer.ClearPointSelectionModeAfterSelection = False
        Me.svDataViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svDataViewer.Location = New System.Drawing.Point(0, 0)
        Me.svDataViewer.MultipleSpectraStackOffset = 0R
        Me.svDataViewer.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.svDataViewer.Name = "svDataViewer"
        Me.svDataViewer.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.svDataViewer.ShowColumnSelectors = True
        Me.svDataViewer.Size = New System.Drawing.Size(1050, 651)
        Me.svDataViewer.TabIndex = 0
        Me.svDataViewer.TurnOnLastFilterSaving_Y = False
        Me.svDataViewer.TurnOnLastSelectionSaving_Y = False
        '
        'gbDataCollector
        '
        Me.gbDataCollector.Controls.Add(Me.ckbDataCollector_UseTab)
        Me.gbDataCollector.Controls.Add(Me.txtDataSelector_ValueSeparator)
        Me.gbDataCollector.Controls.Add(Me.btnDataSelector_ClearPoints)
        Me.gbDataCollector.Controls.Add(Me.rdbDataCollector_ShowVertically)
        Me.gbDataCollector.Controls.Add(Me.rdbDataCollector_ShowHorizonally)
        Me.gbDataCollector.Controls.Add(Me.Label1)
        Me.gbDataCollector.Controls.Add(Me.lblDataCollector_Settings)
        Me.gbDataCollector.Controls.Add(Me.ckbDataCollector_ShowLabels)
        Me.gbDataCollector.Controls.Add(Me.ckbDataCollector_ShowYPoints)
        Me.gbDataCollector.Controls.Add(Me.ckbDataCollector_ShowXPoints)
        Me.gbDataCollector.Controls.Add(Me.txtDataCollector_Output)
        Me.gbDataCollector.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbDataCollector.Location = New System.Drawing.Point(0, 0)
        Me.gbDataCollector.Name = "gbDataCollector"
        Me.gbDataCollector.Size = New System.Drawing.Size(1050, 155)
        Me.gbDataCollector.TabIndex = 0
        Me.gbDataCollector.TabStop = False
        Me.gbDataCollector.Text = "collect data points (double click, to select a data-point)"
        '
        'ckbDataCollector_UseTab
        '
        Me.ckbDataCollector_UseTab.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ckbDataCollector_UseTab.AutoSize = True
        Me.ckbDataCollector_UseTab.Location = New System.Drawing.Point(751, 135)
        Me.ckbDataCollector_UseTab.Name = "ckbDataCollector_UseTab"
        Me.ckbDataCollector_UseTab.Size = New System.Drawing.Size(41, 17)
        Me.ckbDataCollector_UseTab.TabIndex = 6
        Me.ckbDataCollector_UseTab.Text = "tab"
        Me.ckbDataCollector_UseTab.UseVisualStyleBackColor = True
        '
        'txtDataSelector_ValueSeparator
        '
        Me.txtDataSelector_ValueSeparator.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDataSelector_ValueSeparator.Location = New System.Drawing.Point(644, 132)
        Me.txtDataSelector_ValueSeparator.Name = "txtDataSelector_ValueSeparator"
        Me.txtDataSelector_ValueSeparator.Size = New System.Drawing.Size(100, 20)
        Me.txtDataSelector_ValueSeparator.TabIndex = 5
        '
        'btnDataSelector_ClearPoints
        '
        Me.btnDataSelector_ClearPoints.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDataSelector_ClearPoints.Location = New System.Drawing.Point(938, 131)
        Me.btnDataSelector_ClearPoints.Name = "btnDataSelector_ClearPoints"
        Me.btnDataSelector_ClearPoints.Size = New System.Drawing.Size(107, 20)
        Me.btnDataSelector_ClearPoints.TabIndex = 4
        Me.btnDataSelector_ClearPoints.Text = "clear points"
        Me.btnDataSelector_ClearPoints.UseVisualStyleBackColor = True
        '
        'rdbDataCollector_ShowVertically
        '
        Me.rdbDataCollector_ShowVertically.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rdbDataCollector_ShowVertically.AutoSize = True
        Me.rdbDataCollector_ShowVertically.Location = New System.Drawing.Point(426, 134)
        Me.rdbDataCollector_ShowVertically.Name = "rdbDataCollector_ShowVertically"
        Me.rdbDataCollector_ShowVertically.Size = New System.Drawing.Size(99, 17)
        Me.rdbDataCollector_ShowVertically.TabIndex = 3
        Me.rdbDataCollector_ShowVertically.Text = "output vertically"
        Me.rdbDataCollector_ShowVertically.UseVisualStyleBackColor = True
        '
        'rdbDataCollector_ShowHorizonally
        '
        Me.rdbDataCollector_ShowHorizonally.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rdbDataCollector_ShowHorizonally.AutoSize = True
        Me.rdbDataCollector_ShowHorizonally.Checked = True
        Me.rdbDataCollector_ShowHorizonally.Location = New System.Drawing.Point(310, 134)
        Me.rdbDataCollector_ShowHorizonally.Name = "rdbDataCollector_ShowHorizonally"
        Me.rdbDataCollector_ShowHorizonally.Size = New System.Drawing.Size(110, 17)
        Me.rdbDataCollector_ShowHorizonally.TabIndex = 3
        Me.rdbDataCollector_ShowHorizonally.TabStop = True
        Me.rdbDataCollector_ShowHorizonally.Text = "output horizontally"
        Me.rdbDataCollector_ShowHorizonally.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(557, 136)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "value separator:"
        '
        'lblDataCollector_Settings
        '
        Me.lblDataCollector_Settings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDataCollector_Settings.AutoSize = True
        Me.lblDataCollector_Settings.Location = New System.Drawing.Point(7, 136)
        Me.lblDataCollector_Settings.Name = "lblDataCollector_Settings"
        Me.lblDataCollector_Settings.Size = New System.Drawing.Size(79, 13)
        Me.lblDataCollector_Settings.TabIndex = 2
        Me.lblDataCollector_Settings.Text = "output settings:"
        '
        'ckbDataCollector_ShowLabels
        '
        Me.ckbDataCollector_ShowLabels.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ckbDataCollector_ShowLabels.AutoSize = True
        Me.ckbDataCollector_ShowLabels.Location = New System.Drawing.Point(221, 135)
        Me.ckbDataCollector_ShowLabels.Name = "ckbDataCollector_ShowLabels"
        Me.ckbDataCollector_ShowLabels.Size = New System.Drawing.Size(83, 17)
        Me.ckbDataCollector_ShowLabels.TabIndex = 1
        Me.ckbDataCollector_ShowLabels.Text = "curve labels"
        Me.ckbDataCollector_ShowLabels.UseVisualStyleBackColor = True
        '
        'ckbDataCollector_ShowYPoints
        '
        Me.ckbDataCollector_ShowYPoints.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ckbDataCollector_ShowYPoints.AutoSize = True
        Me.ckbDataCollector_ShowYPoints.Location = New System.Drawing.Point(155, 135)
        Me.ckbDataCollector_ShowYPoints.Name = "ckbDataCollector_ShowYPoints"
        Me.ckbDataCollector_ShowYPoints.Size = New System.Drawing.Size(67, 17)
        Me.ckbDataCollector_ShowYPoints.TabIndex = 1
        Me.ckbDataCollector_ShowYPoints.Text = "Y values"
        Me.ckbDataCollector_ShowYPoints.UseVisualStyleBackColor = True
        '
        'ckbDataCollector_ShowXPoints
        '
        Me.ckbDataCollector_ShowXPoints.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ckbDataCollector_ShowXPoints.AutoSize = True
        Me.ckbDataCollector_ShowXPoints.Location = New System.Drawing.Point(88, 135)
        Me.ckbDataCollector_ShowXPoints.Name = "ckbDataCollector_ShowXPoints"
        Me.ckbDataCollector_ShowXPoints.Size = New System.Drawing.Size(67, 17)
        Me.ckbDataCollector_ShowXPoints.TabIndex = 1
        Me.ckbDataCollector_ShowXPoints.Text = "X values"
        Me.ckbDataCollector_ShowXPoints.UseVisualStyleBackColor = True
        '
        'txtDataCollector_Output
        '
        Me.txtDataCollector_Output.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDataCollector_Output.Location = New System.Drawing.Point(6, 19)
        Me.txtDataCollector_Output.Multiline = True
        Me.txtDataCollector_Output.Name = "txtDataCollector_Output"
        Me.txtDataCollector_Output.ReadOnly = True
        Me.txtDataCollector_Output.Size = New System.Drawing.Size(1038, 109)
        Me.txtDataCollector_Output.TabIndex = 0
        Me.txtDataCollector_Output.WordWrap = False
        '
        'tpShow
        '
        Me.tpShow.Controls.Add(Me.dtSpectroscopyTable)
        Me.tpShow.Location = New System.Drawing.Point(4, 22)
        Me.tpShow.Name = "tpShow"
        Me.tpShow.Padding = New System.Windows.Forms.Padding(3)
        Me.tpShow.Size = New System.Drawing.Size(1056, 816)
        Me.tpShow.TabIndex = 1
        Me.tpShow.Text = "data table"
        Me.tpShow.UseVisualStyleBackColor = True
        '
        'dtSpectroscopyTable
        '
        Me.dtSpectroscopyTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtSpectroscopyTable.Location = New System.Drawing.Point(3, 3)
        Me.dtSpectroscopyTable.Name = "dtSpectroscopyTable"
        Me.dtSpectroscopyTable.Size = New System.Drawing.Size(1050, 810)
        Me.dtSpectroscopyTable.TabIndex = 0
        '
        'tpAdditionalComment
        '
        Me.tpAdditionalComment.Controls.Add(Me.btnSaveAdditionalComment)
        Me.tpAdditionalComment.Controls.Add(Me.txtAdditionalComment)
        Me.tpAdditionalComment.Location = New System.Drawing.Point(4, 22)
        Me.tpAdditionalComment.Name = "tpAdditionalComment"
        Me.tpAdditionalComment.Padding = New System.Windows.Forms.Padding(3)
        Me.tpAdditionalComment.Size = New System.Drawing.Size(1056, 816)
        Me.tpAdditionalComment.TabIndex = 2
        Me.tpAdditionalComment.Text = "extended comment"
        Me.tpAdditionalComment.UseVisualStyleBackColor = True
        '
        'btnSaveAdditionalComment
        '
        Me.btnSaveAdditionalComment.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAdditionalComment.Image = Global.SpectroscopyManager.My.Resources.Resources.save_16
        Me.btnSaveAdditionalComment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveAdditionalComment.Location = New System.Drawing.Point(937, 790)
        Me.btnSaveAdditionalComment.Name = "btnSaveAdditionalComment"
        Me.btnSaveAdditionalComment.Size = New System.Drawing.Size(160, 23)
        Me.btnSaveAdditionalComment.TabIndex = 1
        Me.btnSaveAdditionalComment.Text = "save extended comment"
        Me.btnSaveAdditionalComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveAdditionalComment.UseVisualStyleBackColor = True
        '
        'txtAdditionalComment
        '
        Me.txtAdditionalComment.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdditionalComment.Font = New System.Drawing.Font("Consolas", 8.25!)
        Me.txtAdditionalComment.Location = New System.Drawing.Point(3, 3)
        Me.txtAdditionalComment.Multiline = True
        Me.txtAdditionalComment.Name = "txtAdditionalComment"
        Me.txtAdditionalComment.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtAdditionalComment.Size = New System.Drawing.Size(1092, 781)
        Me.txtAdditionalComment.TabIndex = 0
        Me.txtAdditionalComment.WordWrap = False
        '
        'tpGeneralProperties
        '
        Me.tpGeneralProperties.Controls.Add(Me.dtGeneralPropertyArray)
        Me.tpGeneralProperties.Location = New System.Drawing.Point(4, 22)
        Me.tpGeneralProperties.Name = "tpGeneralProperties"
        Me.tpGeneralProperties.Padding = New System.Windows.Forms.Padding(3)
        Me.tpGeneralProperties.Size = New System.Drawing.Size(1056, 816)
        Me.tpGeneralProperties.TabIndex = 3
        Me.tpGeneralProperties.Text = "headers extracted from data file"
        Me.tpGeneralProperties.UseVisualStyleBackColor = True
        '
        'dtGeneralPropertyArray
        '
        Me.dtGeneralPropertyArray.AllowUserToAddRows = False
        Me.dtGeneralPropertyArray.AllowUserToDeleteRows = False
        Me.dtGeneralPropertyArray.AllowUserToResizeRows = False
        Me.dtGeneralPropertyArray.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dtGeneralPropertyArray.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dtGeneralPropertyArray.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtGeneralPropertyArray.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colGeneralPropertyArray_Key, Me.colGeneralPropertyArray_Value})
        Me.dtGeneralPropertyArray.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtGeneralPropertyArray.Location = New System.Drawing.Point(3, 3)
        Me.dtGeneralPropertyArray.Name = "dtGeneralPropertyArray"
        Me.dtGeneralPropertyArray.ReadOnly = True
        Me.dtGeneralPropertyArray.RowHeadersVisible = False
        Me.dtGeneralPropertyArray.Size = New System.Drawing.Size(1050, 810)
        Me.dtGeneralPropertyArray.TabIndex = 0
        '
        'colGeneralPropertyArray_Key
        '
        Me.colGeneralPropertyArray_Key.HeaderText = "property name"
        Me.colGeneralPropertyArray_Key.Name = "colGeneralPropertyArray_Key"
        Me.colGeneralPropertyArray_Key.ReadOnly = True
        Me.colGeneralPropertyArray_Key.Width = 99
        '
        'colGeneralPropertyArray_Value
        '
        Me.colGeneralPropertyArray_Value.HeaderText = "property value"
        Me.colGeneralPropertyArray_Value.Name = "colGeneralPropertyArray_Value"
        Me.colGeneralPropertyArray_Value.ReadOnly = True
        Me.colGeneralPropertyArray_Value.Width = 99
        '
        'gbSpectraComment
        '
        Me.gbSpectraComment.Controls.Add(Me.txtSpectraComment)
        Me.gbSpectraComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSpectraComment.Location = New System.Drawing.Point(0, 0)
        Me.gbSpectraComment.Name = "gbSpectraComment"
        Me.gbSpectraComment.Size = New System.Drawing.Size(296, 279)
        Me.gbSpectraComment.TabIndex = 19
        Me.gbSpectraComment.TabStop = False
        Me.gbSpectraComment.Text = "File-Comment"
        '
        'txtSpectraComment
        '
        Me.txtSpectraComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSpectraComment.Location = New System.Drawing.Point(3, 16)
        Me.txtSpectraComment.Multiline = True
        Me.txtSpectraComment.Name = "txtSpectraComment"
        Me.txtSpectraComment.ReadOnly = True
        Me.txtSpectraComment.Size = New System.Drawing.Size(290, 260)
        Me.txtSpectraComment.TabIndex = 0
        '
        'gbSpectroscopyTablePropertyGrid
        '
        Me.gbSpectroscopyTablePropertyGrid.Controls.Add(Me.pgSpectrumProperies)
        Me.gbSpectroscopyTablePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSpectroscopyTablePropertyGrid.Location = New System.Drawing.Point(0, 0)
        Me.gbSpectroscopyTablePropertyGrid.Name = "gbSpectroscopyTablePropertyGrid"
        Me.gbSpectroscopyTablePropertyGrid.Size = New System.Drawing.Size(296, 559)
        Me.gbSpectroscopyTablePropertyGrid.TabIndex = 20
        Me.gbSpectroscopyTablePropertyGrid.TabStop = False
        Me.gbSpectroscopyTablePropertyGrid.Text = "File Properties"
        '
        'pgSpectrumProperies
        '
        Me.pgSpectrumProperies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgSpectrumProperies.HelpVisible = False
        Me.pgSpectrumProperies.Location = New System.Drawing.Point(3, 16)
        Me.pgSpectrumProperies.Name = "pgSpectrumProperies"
        Me.pgSpectrumProperies.Size = New System.Drawing.Size(290, 540)
        Me.pgSpectrumProperies.TabIndex = 12
        Me.pgSpectrumProperies.ToolbarVisible = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.tcSpectroscopyTable)
        Me.SplitContainer1.Size = New System.Drawing.Size(1364, 842)
        Me.SplitContainer1.SplitterDistance = 296
        Me.SplitContainer1.TabIndex = 21
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.gbSpectroscopyTablePropertyGrid)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.gbSpectraComment)
        Me.SplitContainer2.Size = New System.Drawing.Size(296, 842)
        Me.SplitContainer2.SplitterDistance = 559
        Me.SplitContainer2.TabIndex = 21
        '
        'wDataExplorer_SpectroscopyTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1364, 842)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "wDataExplorer_SpectroscopyTable"
        Me.Text = "Spectroscopy Explorer - "
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.tcSpectroscopyTable.ResumeLayout(False)
        Me.tpPlot.ResumeLayout(False)
        Me.scPlotBrowser.Panel1.ResumeLayout(False)
        Me.scPlotBrowser.Panel2.ResumeLayout(False)
        CType(Me.scPlotBrowser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scPlotBrowser.ResumeLayout(False)
        Me.gbDataCollector.ResumeLayout(False)
        Me.gbDataCollector.PerformLayout()
        Me.tpShow.ResumeLayout(False)
        Me.tpAdditionalComment.ResumeLayout(False)
        Me.tpAdditionalComment.PerformLayout()
        Me.tpGeneralProperties.ResumeLayout(False)
        CType(Me.dtGeneralPropertyArray, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSpectraComment.ResumeLayout(False)
        Me.gbSpectraComment.PerformLayout()
        Me.gbSpectroscopyTablePropertyGrid.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcSpectroscopyTable As System.Windows.Forms.TabControl
    Friend WithEvents tpPlot As System.Windows.Forms.TabPage
    Friend WithEvents tpShow As System.Windows.Forms.TabPage
    Friend WithEvents svDataViewer As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents gbSpectraComment As System.Windows.Forms.GroupBox
    Friend WithEvents txtSpectraComment As System.Windows.Forms.TextBox
    Friend WithEvents gbSpectroscopyTablePropertyGrid As System.Windows.Forms.GroupBox
    Friend WithEvents pgSpectrumProperies As System.Windows.Forms.PropertyGrid
    Friend WithEvents dtSpectroscopyTable As SpectroscopyManager.mSpectroscopyTableDataTable
    Friend WithEvents tpAdditionalComment As System.Windows.Forms.TabPage
    Friend WithEvents txtAdditionalComment As System.Windows.Forms.TextBox
    Friend WithEvents btnSaveAdditionalComment As System.Windows.Forms.Button
    Friend WithEvents scPlotBrowser As System.Windows.Forms.SplitContainer
    Friend WithEvents gbDataCollector As System.Windows.Forms.GroupBox
    Friend WithEvents txtDataCollector_Output As System.Windows.Forms.TextBox
    Friend WithEvents rdbDataCollector_ShowVertically As System.Windows.Forms.RadioButton
    Friend WithEvents rdbDataCollector_ShowHorizonally As System.Windows.Forms.RadioButton
    Friend WithEvents lblDataCollector_Settings As System.Windows.Forms.Label
    Friend WithEvents ckbDataCollector_ShowYPoints As System.Windows.Forms.CheckBox
    Friend WithEvents ckbDataCollector_ShowXPoints As System.Windows.Forms.CheckBox
    Friend WithEvents btnDataSelector_ClearPoints As System.Windows.Forms.Button
    Friend WithEvents ckbDataCollector_ShowLabels As System.Windows.Forms.CheckBox
    Friend WithEvents txtDataSelector_ValueSeparator As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ckbDataCollector_UseTab As System.Windows.Forms.CheckBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents tpGeneralProperties As TabPage
    Friend WithEvents dtGeneralPropertyArray As DataGridView
    Friend WithEvents colGeneralPropertyArray_Key As DataGridViewTextBoxColumn
    Friend WithEvents colGeneralPropertyArray_Value As DataGridViewTextBoxColumn
End Class
