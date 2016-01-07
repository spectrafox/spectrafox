<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wFit_SingleDataSet
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wFit_SingleDataSet))
        Me.gbFitModels = New System.Windows.Forms.GroupBox()
        Me.gbImportExportFitModels = New System.Windows.Forms.GroupBox()
        Me.btnImportFitModels = New System.Windows.Forms.Button()
        Me.btnExportFitModels = New System.Windows.Forms.Button()
        Me.gbFitRange = New System.Windows.Forms.GroupBox()
        Me.btnSelectFitRange = New System.Windows.Forms.Button()
        Me.txtRightValue = New SpectroscopyManager.NumericTextbox()
        Me.txtLeftValue = New SpectroscopyManager.NumericTextbox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnAddFitModel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboAddFitModel = New System.Windows.Forms.ComboBox()
        Me.flpFitModels = New System.Windows.Forms.FlowLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange = New System.Windows.Forms.CheckBox()
        Me.lblSavingProgress = New System.Windows.Forms.Label()
        Me.btnSaveFitProcedureOutput = New System.Windows.Forms.Button()
        Me.btnSaveFitToSpectroscopyTable = New System.Windows.Forms.Button()
        Me.pgbSavingProgress = New System.Windows.Forms.ProgressBar()
        Me.gbSettings = New System.Windows.Forms.GroupBox()
        Me.ckbUpdatePreviewDuringFit = New System.Windows.Forms.CheckBox()
        Me.btnSetPreviewPointNumber = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.nudPreviewPoints = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.pbPreview = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.gbSourceDataSelector = New System.Windows.Forms.GroupBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cbY = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.cbX = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.gbOutput = New System.Windows.Forms.GroupBox()
        Me.txtFitEcho = New System.Windows.Forms.TextBox()
        Me.lblFitProgress = New System.Windows.Forms.Label()
        Me.pgbFitProgress = New System.Windows.Forms.ProgressBar()
        Me.cboFitProcedure = New System.Windows.Forms.ComboBox()
        Me.btnStartFitting = New System.Windows.Forms.Button()
        Me.gbFitProcedure = New System.Windows.Forms.GroupBox()
        Me.gbProgress = New System.Windows.Forms.GroupBox()
        Me.btnChangeParallelizationSettings = New System.Windows.Forms.Button()
        Me.btnChangeFitProcedureSettings = New System.Windows.Forms.Button()
        Me.btnAddFitToFitQueue = New System.Windows.Forms.Button()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.gbFitModels.SuspendLayout()
        Me.gbImportExportFitModels.SuspendLayout()
        Me.gbFitRange.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gbSettings.SuspendLayout()
        CType(Me.nudPreviewPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.gbSourceDataSelector.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        Me.gbFitProcedure.SuspendLayout()
        Me.gbProgress.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbFitModels
        '
        Me.gbFitModels.Controls.Add(Me.gbImportExportFitModels)
        Me.gbFitModels.Controls.Add(Me.gbFitRange)
        Me.gbFitModels.Controls.Add(Me.btnAddFitModel)
        Me.gbFitModels.Controls.Add(Me.Label1)
        Me.gbFitModels.Controls.Add(Me.cboAddFitModel)
        Me.gbFitModels.Controls.Add(Me.flpFitModels)
        Me.gbFitModels.Controls.Add(Me.GroupBox1)
        Me.gbFitModels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbFitModels.Location = New System.Drawing.Point(0, 0)
        Me.gbFitModels.Name = "gbFitModels"
        Me.gbFitModels.Size = New System.Drawing.Size(1354, 516)
        Me.gbFitModels.TabIndex = 15
        Me.gbFitModels.TabStop = False
        Me.gbFitModels.Text = "Fit-Models"
        '
        'gbImportExportFitModels
        '
        Me.gbImportExportFitModels.Controls.Add(Me.btnImportFitModels)
        Me.gbImportExportFitModels.Controls.Add(Me.btnExportFitModels)
        Me.gbImportExportFitModels.Location = New System.Drawing.Point(7, 123)
        Me.gbImportExportFitModels.Name = "gbImportExportFitModels"
        Me.gbImportExportFitModels.Size = New System.Drawing.Size(245, 108)
        Me.gbImportExportFitModels.TabIndex = 6
        Me.gbImportExportFitModels.TabStop = False
        Me.gbImportExportFitModels.Text = "Import / Export"
        '
        'btnImportFitModels
        '
        Me.btnImportFitModels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImportFitModels.Image = Global.SpectroscopyManager.My.Resources.Resources.import_25
        Me.btnImportFitModels.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImportFitModels.Location = New System.Drawing.Point(6, 60)
        Me.btnImportFitModels.Name = "btnImportFitModels"
        Me.btnImportFitModels.Size = New System.Drawing.Size(233, 37)
        Me.btnImportFitModels.TabIndex = 5
        Me.btnImportFitModels.TabStop = False
        Me.btnImportFitModels.Text = "import fit models"
        Me.btnImportFitModels.UseVisualStyleBackColor = True
        '
        'btnExportFitModels
        '
        Me.btnExportFitModels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportFitModels.Image = Global.SpectroscopyManager.My.Resources.Resources.export_25
        Me.btnExportFitModels.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportFitModels.Location = New System.Drawing.Point(6, 19)
        Me.btnExportFitModels.Name = "btnExportFitModels"
        Me.btnExportFitModels.Size = New System.Drawing.Size(233, 37)
        Me.btnExportFitModels.TabIndex = 5
        Me.btnExportFitModels.TabStop = False
        Me.btnExportFitModels.Text = "export fit models"
        Me.btnExportFitModels.UseVisualStyleBackColor = True
        '
        'gbFitRange
        '
        Me.gbFitRange.Controls.Add(Me.btnSelectFitRange)
        Me.gbFitRange.Controls.Add(Me.txtRightValue)
        Me.gbFitRange.Controls.Add(Me.txtLeftValue)
        Me.gbFitRange.Controls.Add(Me.Label4)
        Me.gbFitRange.Location = New System.Drawing.Point(7, 44)
        Me.gbFitRange.Name = "gbFitRange"
        Me.gbFitRange.Size = New System.Drawing.Size(245, 72)
        Me.gbFitRange.TabIndex = 4
        Me.gbFitRange.TabStop = False
        Me.gbFitRange.Text = "Fit-Range"
        '
        'btnSelectFitRange
        '
        Me.btnSelectFitRange.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_16
        Me.btnSelectFitRange.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectFitRange.Location = New System.Drawing.Point(14, 43)
        Me.btnSelectFitRange.Name = "btnSelectFitRange"
        Me.btnSelectFitRange.Size = New System.Drawing.Size(219, 23)
        Me.btnSelectFitRange.TabIndex = 4
        Me.btnSelectFitRange.TabStop = False
        Me.btnSelectFitRange.Text = "select fit range"
        Me.btnSelectFitRange.UseVisualStyleBackColor = True
        '
        'txtRightValue
        '
        Me.txtRightValue.BackColor = System.Drawing.Color.White
        Me.txtRightValue.ForeColor = System.Drawing.Color.Black
        Me.txtRightValue.FormatDecimalPlaces = 6
        Me.txtRightValue.Location = New System.Drawing.Point(133, 17)
        Me.txtRightValue.Name = "txtRightValue"
        Me.txtRightValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtRightValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtRightValue.Size = New System.Drawing.Size(100, 20)
        Me.txtRightValue.TabIndex = 1
        Me.txtRightValue.Text = "0.000000"
        Me.txtRightValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRightValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtLeftValue
        '
        Me.txtLeftValue.BackColor = System.Drawing.Color.White
        Me.txtLeftValue.ForeColor = System.Drawing.Color.Black
        Me.txtLeftValue.FormatDecimalPlaces = 6
        Me.txtLeftValue.Location = New System.Drawing.Point(14, 17)
        Me.txtLeftValue.Name = "txtLeftValue"
        Me.txtLeftValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtLeftValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtLeftValue.Size = New System.Drawing.Size(100, 20)
        Me.txtLeftValue.TabIndex = 0
        Me.txtLeftValue.Text = "0.000000"
        Me.txtLeftValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLeftValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(116, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(16, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "to"
        '
        'btnAddFitModel
        '
        Me.btnAddFitModel.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddFitModel.Location = New System.Drawing.Point(221, 15)
        Me.btnAddFitModel.Name = "btnAddFitModel"
        Me.btnAddFitModel.Size = New System.Drawing.Size(31, 23)
        Me.btnAddFitModel.TabIndex = 2
        Me.btnAddFitModel.TabStop = False
        Me.btnAddFitModel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Add:"
        '
        'cboAddFitModel
        '
        Me.cboAddFitModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAddFitModel.DropDownWidth = 250
        Me.cboAddFitModel.FormattingEnabled = True
        Me.cboAddFitModel.Location = New System.Drawing.Point(43, 17)
        Me.cboAddFitModel.Name = "cboAddFitModel"
        Me.cboAddFitModel.Size = New System.Drawing.Size(172, 21)
        Me.cboAddFitModel.TabIndex = 1
        Me.cboAddFitModel.TabStop = False
        '
        'flpFitModels
        '
        Me.flpFitModels.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpFitModels.AutoScroll = True
        Me.flpFitModels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpFitModels.Location = New System.Drawing.Point(258, 15)
        Me.flpFitModels.Name = "flpFitModels"
        Me.flpFitModels.Size = New System.Drawing.Size(1091, 495)
        Me.flpFitModels.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ckbSaveGenerateDataOnlyInSelectedFitRange)
        Me.GroupBox1.Controls.Add(Me.lblSavingProgress)
        Me.GroupBox1.Controls.Add(Me.btnSaveFitProcedureOutput)
        Me.GroupBox1.Controls.Add(Me.btnSaveFitToSpectroscopyTable)
        Me.GroupBox1.Controls.Add(Me.pgbSavingProgress)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 235)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(245, 153)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "saving fitted data"
        '
        'ckbSaveGenerateDataOnlyInSelectedFitRange
        '
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.AutoSize = True
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.Checked = True
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.Location = New System.Drawing.Point(10, 19)
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.Name = "ckbSaveGenerateDataOnlyInSelectedFitRange"
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.Size = New System.Drawing.Size(227, 17)
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.TabIndex = 6
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.Text = "generate data only in the selected fit-range"
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange.UseVisualStyleBackColor = True
        '
        'lblSavingProgress
        '
        Me.lblSavingProgress.AutoSize = True
        Me.lblSavingProgress.Location = New System.Drawing.Point(7, 95)
        Me.lblSavingProgress.Name = "lblSavingProgress"
        Me.lblSavingProgress.Size = New System.Drawing.Size(16, 13)
        Me.lblSavingProgress.TabIndex = 18
        Me.lblSavingProgress.Text = "..."
        '
        'btnSaveFitProcedureOutput
        '
        Me.btnSaveFitProcedureOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveFitProcedureOutput.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveFitProcedureOutput.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveFitProcedureOutput.Location = New System.Drawing.Point(6, 110)
        Me.btnSaveFitProcedureOutput.Name = "btnSaveFitProcedureOutput"
        Me.btnSaveFitProcedureOutput.Size = New System.Drawing.Size(233, 37)
        Me.btnSaveFitProcedureOutput.TabIndex = 5
        Me.btnSaveFitProcedureOutput.Text = "save fit-procedure output"
        Me.btnSaveFitProcedureOutput.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveFitProcedureOutput.UseVisualStyleBackColor = True
        '
        'btnSaveFitToSpectroscopyTable
        '
        Me.btnSaveFitToSpectroscopyTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveFitToSpectroscopyTable.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveFitToSpectroscopyTable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveFitToSpectroscopyTable.Location = New System.Drawing.Point(6, 36)
        Me.btnSaveFitToSpectroscopyTable.Name = "btnSaveFitToSpectroscopyTable"
        Me.btnSaveFitToSpectroscopyTable.Size = New System.Drawing.Size(233, 37)
        Me.btnSaveFitToSpectroscopyTable.TabIndex = 5
        Me.btnSaveFitToSpectroscopyTable.Text = "save fitted data"
        Me.btnSaveFitToSpectroscopyTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveFitToSpectroscopyTable.UseVisualStyleBackColor = True
        '
        'pgbSavingProgress
        '
        Me.pgbSavingProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbSavingProgress.Location = New System.Drawing.Point(8, 79)
        Me.pgbSavingProgress.Name = "pgbSavingProgress"
        Me.pgbSavingProgress.Size = New System.Drawing.Size(231, 12)
        Me.pgbSavingProgress.TabIndex = 17
        '
        'gbSettings
        '
        Me.gbSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbSettings.Controls.Add(Me.ckbUpdatePreviewDuringFit)
        Me.gbSettings.Controls.Add(Me.btnSetPreviewPointNumber)
        Me.gbSettings.Controls.Add(Me.Label18)
        Me.gbSettings.Controls.Add(Me.nudPreviewPoints)
        Me.gbSettings.Location = New System.Drawing.Point(3, 360)
        Me.gbSettings.Name = "gbSettings"
        Me.gbSettings.Size = New System.Drawing.Size(262, 58)
        Me.gbSettings.TabIndex = 14
        Me.gbSettings.TabStop = False
        Me.gbSettings.Text = "Settings:"
        '
        'ckbUpdatePreviewDuringFit
        '
        Me.ckbUpdatePreviewDuringFit.AutoSize = True
        Me.ckbUpdatePreviewDuringFit.Checked = Global.SpectroscopyManager.My.MySettings.Default.Fit_UpdatePreviewDuringFit
        Me.ckbUpdatePreviewDuringFit.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.SpectroscopyManager.My.MySettings.Default, "Fit_UpdatePreviewDuringFit", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.ckbUpdatePreviewDuringFit.Location = New System.Drawing.Point(107, 37)
        Me.ckbUpdatePreviewDuringFit.Name = "ckbUpdatePreviewDuringFit"
        Me.ckbUpdatePreviewDuringFit.Size = New System.Drawing.Size(142, 17)
        Me.ckbUpdatePreviewDuringFit.TabIndex = 3
        Me.ckbUpdatePreviewDuringFit.Text = "update preview during fit"
        Me.ckbUpdatePreviewDuringFit.UseVisualStyleBackColor = True
        '
        'btnSetPreviewPointNumber
        '
        Me.btnSetPreviewPointNumber.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_16
        Me.btnSetPreviewPointNumber.Location = New System.Drawing.Point(218, 12)
        Me.btnSetPreviewPointNumber.Name = "btnSetPreviewPointNumber"
        Me.btnSetPreviewPointNumber.Size = New System.Drawing.Size(31, 23)
        Me.btnSetPreviewPointNumber.TabIndex = 2
        Me.btnSetPreviewPointNumber.TabStop = False
        Me.btnSetPreviewPointNumber.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(9, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(130, 13)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "Number of preview points:"
        '
        'nudPreviewPoints
        '
        Me.nudPreviewPoints.Location = New System.Drawing.Point(145, 14)
        Me.nudPreviewPoints.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPreviewPoints.Name = "nudPreviewPoints"
        Me.nudPreviewPoints.Size = New System.Drawing.Size(67, 20)
        Me.nudPreviewPoints.TabIndex = 2
        Me.nudPreviewPoints.TabStop = False
        Me.nudPreviewPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudPreviewPoints.Value = New Decimal(New Integer() {500, 0, 0, 0})
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.pbPreview)
        Me.GroupBox4.Location = New System.Drawing.Point(655, 3)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(694, 414)
        Me.GroupBox4.TabIndex = 12
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Fit-Result / -Preview"
        '
        'pbPreview
        '
        Me.pbPreview.AllowAdjustingXColumn = True
        Me.pbPreview.AllowAdjustingYColumn = True
        Me.pbPreview.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbPreview.CallbackXRangeSelected = Nothing
        Me.pbPreview.CallbackXValueSelected = Nothing
        Me.pbPreview.CallbackXYRangeSelected = Nothing
        Me.pbPreview.CallbackYRangeSelected = Nothing
        Me.pbPreview.CallbackYValueSelected = Nothing
        Me.pbPreview.ClearPointSelectionModeAfterSelection = False
        Me.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbPreview.Location = New System.Drawing.Point(3, 16)
        Me.pbPreview.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbPreview.Name = "pbPreview"
        Me.pbPreview.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbPreview.ShowColumnSelectors = False
        Me.pbPreview.Size = New System.Drawing.Size(688, 395)
        Me.pbPreview.TabIndex = 0
        Me.pbPreview.TurnOnLastFilterSaving_Y = False
        Me.pbPreview.TurnOnLastSelectionSaving_Y = False
        '
        'gbSourceDataSelector
        '
        Me.gbSourceDataSelector.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbSourceDataSelector.Controls.Add(Me.Label15)
        Me.gbSourceDataSelector.Controls.Add(Me.Label14)
        Me.gbSourceDataSelector.Controls.Add(Me.cbY)
        Me.gbSourceDataSelector.Controls.Add(Me.cbX)
        Me.gbSourceDataSelector.Location = New System.Drawing.Point(271, 360)
        Me.gbSourceDataSelector.Name = "gbSourceDataSelector"
        Me.gbSourceDataSelector.Size = New System.Drawing.Size(378, 58)
        Me.gbSourceDataSelector.TabIndex = 13
        Me.gbSourceDataSelector.TabStop = False
        Me.gbSourceDataSelector.Text = "Source Data"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(192, 22)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(14, 13)
        Me.Label15.TabIndex = 10
        Me.Label15.Text = "Y"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(7, 22)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(14, 13)
        Me.Label14.TabIndex = 10
        Me.Label14.Text = "X"
        '
        'cbY
        '
        Me.cbY.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.cbY.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbY.Location = New System.Drawing.Point(212, 19)
        Me.cbY.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbY.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbY.Name = "cbY"
        Me.cbY.SelectedColumnName = ""
        Me.cbY.SelectedColumnNames = CType(resources.GetObject("cbY.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbY.SelectedEntries = CType(resources.GetObject("cbY.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbY.SelectedEntry = ""
        Me.cbY.Size = New System.Drawing.Size(154, 21)
        Me.cbY.TabIndex = 1
        Me.cbY.TabStop = False
        Me.cbY.TurnOnLastFilterSaving = False
        Me.cbY.TurnOnLastSelectionSaving = False
        '
        'cbX
        '
        Me.cbX.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.cbX.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbX.Location = New System.Drawing.Point(27, 19)
        Me.cbX.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbX.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbX.Name = "cbX"
        Me.cbX.SelectedColumnName = ""
        Me.cbX.SelectedColumnNames = CType(resources.GetObject("cbX.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbX.SelectedEntries = CType(resources.GetObject("cbX.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbX.SelectedEntry = ""
        Me.cbX.Size = New System.Drawing.Size(154, 21)
        Me.cbX.TabIndex = 0
        Me.cbX.TabStop = False
        Me.cbX.TurnOnLastFilterSaving = False
        Me.cbX.TurnOnLastSelectionSaving = False
        '
        'gbOutput
        '
        Me.gbOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbOutput.Controls.Add(Me.txtFitEcho)
        Me.gbOutput.Location = New System.Drawing.Point(3, 89)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Size = New System.Drawing.Size(646, 271)
        Me.gbOutput.TabIndex = 6
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "Fit Output:"
        '
        'txtFitEcho
        '
        Me.txtFitEcho.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFitEcho.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFitEcho.Location = New System.Drawing.Point(3, 16)
        Me.txtFitEcho.Multiline = True
        Me.txtFitEcho.Name = "txtFitEcho"
        Me.txtFitEcho.ReadOnly = True
        Me.txtFitEcho.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtFitEcho.Size = New System.Drawing.Size(637, 249)
        Me.txtFitEcho.TabIndex = 2
        Me.txtFitEcho.TabStop = False
        Me.txtFitEcho.WordWrap = False
        '
        'lblFitProgress
        '
        Me.lblFitProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFitProgress.AutoSize = True
        Me.lblFitProgress.Location = New System.Drawing.Point(10, 33)
        Me.lblFitProgress.Name = "lblFitProgress"
        Me.lblFitProgress.Size = New System.Drawing.Size(16, 13)
        Me.lblFitProgress.TabIndex = 18
        Me.lblFitProgress.Text = "..."
        '
        'pgbFitProgress
        '
        Me.pgbFitProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbFitProgress.Location = New System.Drawing.Point(11, 17)
        Me.pgbFitProgress.Name = "pgbFitProgress"
        Me.pgbFitProgress.Size = New System.Drawing.Size(358, 12)
        Me.pgbFitProgress.TabIndex = 17
        '
        'cboFitProcedure
        '
        Me.cboFitProcedure.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFitProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFitProcedure.FormattingEnabled = True
        Me.cboFitProcedure.Location = New System.Drawing.Point(10, 19)
        Me.cboFitProcedure.Name = "cboFitProcedure"
        Me.cboFitProcedure.Size = New System.Drawing.Size(356, 21)
        Me.cboFitProcedure.TabIndex = 0
        Me.cboFitProcedure.TabStop = False
        '
        'btnStartFitting
        '
        Me.btnStartFitting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartFitting.Enabled = False
        Me.btnStartFitting.Image = Global.SpectroscopyManager.My.Resources.Resources.run_25
        Me.btnStartFitting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStartFitting.Location = New System.Drawing.Point(393, 23)
        Me.btnStartFitting.Name = "btnStartFitting"
        Me.btnStartFitting.Size = New System.Drawing.Size(142, 43)
        Me.btnStartFitting.TabIndex = 19
        Me.btnStartFitting.TabStop = False
        Me.btnStartFitting.Text = "start fitting"
        Me.btnStartFitting.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnStartFitting.UseVisualStyleBackColor = True
        '
        'gbFitProcedure
        '
        Me.gbFitProcedure.Controls.Add(Me.gbProgress)
        Me.gbFitProcedure.Controls.Add(Me.btnStartFitting)
        Me.gbFitProcedure.Controls.Add(Me.btnChangeParallelizationSettings)
        Me.gbFitProcedure.Controls.Add(Me.btnChangeFitProcedureSettings)
        Me.gbFitProcedure.Controls.Add(Me.cboFitProcedure)
        Me.gbFitProcedure.Controls.Add(Me.btnAddFitToFitQueue)
        Me.gbFitProcedure.Location = New System.Drawing.Point(3, 3)
        Me.gbFitProcedure.Name = "gbFitProcedure"
        Me.gbFitProcedure.Size = New System.Drawing.Size(646, 80)
        Me.gbFitProcedure.TabIndex = 16
        Me.gbFitProcedure.TabStop = False
        Me.gbFitProcedure.Text = "Fit-Procedure"
        '
        'gbProgress
        '
        Me.gbProgress.Controls.Add(Me.lblFitProgress)
        Me.gbProgress.Controls.Add(Me.pgbFitProgress)
        Me.gbProgress.Location = New System.Drawing.Point(10, 19)
        Me.gbProgress.Name = "gbProgress"
        Me.gbProgress.Size = New System.Drawing.Size(375, 50)
        Me.gbProgress.TabIndex = 23
        Me.gbProgress.TabStop = False
        Me.gbProgress.Text = "Progress"
        Me.gbProgress.Visible = False
        '
        'btnChangeParallelizationSettings
        '
        Me.btnChangeParallelizationSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.btnChangeParallelizationSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeParallelizationSettings.Location = New System.Drawing.Point(203, 46)
        Me.btnChangeParallelizationSettings.Name = "btnChangeParallelizationSettings"
        Me.btnChangeParallelizationSettings.Size = New System.Drawing.Size(173, 23)
        Me.btnChangeParallelizationSettings.TabIndex = 24
        Me.btnChangeParallelizationSettings.Text = "performance settings"
        Me.btnChangeParallelizationSettings.UseVisualStyleBackColor = True
        '
        'btnChangeFitProcedureSettings
        '
        Me.btnChangeFitProcedureSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.btnChangeFitProcedureSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeFitProcedureSettings.Location = New System.Drawing.Point(10, 46)
        Me.btnChangeFitProcedureSettings.Name = "btnChangeFitProcedureSettings"
        Me.btnChangeFitProcedureSettings.Size = New System.Drawing.Size(192, 23)
        Me.btnChangeFitProcedureSettings.TabIndex = 24
        Me.btnChangeFitProcedureSettings.Text = "fit-procedure settings"
        Me.btnChangeFitProcedureSettings.UseVisualStyleBackColor = True
        '
        'btnAddFitToFitQueue
        '
        Me.btnAddFitToFitQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddFitToFitQueue.Enabled = False
        Me.btnAddFitToFitQueue.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddFitToFitQueue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddFitToFitQueue.Location = New System.Drawing.Point(541, 23)
        Me.btnAddFitToFitQueue.Name = "btnAddFitToFitQueue"
        Me.btnAddFitToFitQueue.Size = New System.Drawing.Size(99, 43)
        Me.btnAddFitToFitQueue.TabIndex = 19
        Me.btnAddFitToFitQueue.TabStop = False
        Me.btnAddFitToFitQueue.Text = "add to" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "fit queue"
        Me.btnAddFitToFitQueue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddFitToFitQueue.UseVisualStyleBackColor = True
        '
        'scMain
        '
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.Location = New System.Drawing.Point(0, 0)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.gbFitProcedure)
        Me.scMain.Panel1.Controls.Add(Me.gbSettings)
        Me.scMain.Panel1.Controls.Add(Me.gbOutput)
        Me.scMain.Panel1.Controls.Add(Me.gbSourceDataSelector)
        Me.scMain.Panel1.Controls.Add(Me.GroupBox4)
        Me.scMain.Panel1MinSize = 400
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.gbFitModels)
        Me.scMain.Panel2MinSize = 400
        Me.scMain.Size = New System.Drawing.Size(1354, 941)
        Me.scMain.SplitterDistance = 421
        Me.scMain.TabIndex = 7
        '
        'wFit_SingleDataSet
        '
        Me.ClientSize = New System.Drawing.Size(1354, 941)
        Me.Controls.Add(Me.scMain)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(1280, 980)
        Me.Name = "wFit_SingleDataSet"
        Me.Text = "Non-Linear Fit - "
        Me.Controls.SetChildIndex(Me.scMain, 0)
        Me.gbFitModels.ResumeLayout(False)
        Me.gbFitModels.PerformLayout()
        Me.gbImportExportFitModels.ResumeLayout(False)
        Me.gbFitRange.ResumeLayout(False)
        Me.gbFitRange.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbSettings.ResumeLayout(False)
        Me.gbSettings.PerformLayout()
        CType(Me.nudPreviewPoints, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.gbSourceDataSelector.ResumeLayout(False)
        Me.gbSourceDataSelector.PerformLayout()
        Me.gbOutput.ResumeLayout(False)
        Me.gbOutput.PerformLayout()
        Me.gbFitProcedure.ResumeLayout(False)
        Me.gbProgress.ResumeLayout(False)
        Me.gbProgress.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents gbOutput As System.Windows.Forms.GroupBox
    Friend WithEvents lblFitProgress As System.Windows.Forms.Label
    Friend WithEvents txtFitEcho As System.Windows.Forms.TextBox
    Friend WithEvents pgbFitProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents gbSettings As System.Windows.Forms.GroupBox
    Friend WithEvents btnSetPreviewPointNumber As System.Windows.Forms.Button
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents nudPreviewPoints As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents gbSourceDataSelector As System.Windows.Forms.GroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cbY As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents cbX As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents gbFitModels As System.Windows.Forms.GroupBox
    Friend WithEvents btnAddFitModel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboAddFitModel As System.Windows.Forms.ComboBox
    Friend WithEvents flpFitModels As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnStartFitting As System.Windows.Forms.Button
    Friend WithEvents cboFitProcedure As System.Windows.Forms.ComboBox
    Friend WithEvents pbPreview As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents gbFitProcedure As System.Windows.Forms.GroupBox
    Friend WithEvents gbProgress As System.Windows.Forms.GroupBox
    Friend WithEvents gbFitRange As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectFitRange As System.Windows.Forms.Button
    Friend WithEvents txtRightValue As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtLeftValue As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSaveFitToSpectroscopyTable As System.Windows.Forms.Button
    Friend WithEvents btnExportFitModels As System.Windows.Forms.Button
    Friend WithEvents btnImportFitModels As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents gbImportExportFitModels As System.Windows.Forms.GroupBox
    Friend WithEvents lblSavingProgress As System.Windows.Forms.Label
    Friend WithEvents pgbSavingProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents btnChangeFitProcedureSettings As System.Windows.Forms.Button
    Friend WithEvents btnSaveFitProcedureOutput As System.Windows.Forms.Button
    Friend WithEvents ckbSaveGenerateDataOnlyInSelectedFitRange As System.Windows.Forms.CheckBox
    Friend WithEvents btnAddFitToFitQueue As System.Windows.Forms.Button
    Friend WithEvents btnChangeParallelizationSettings As System.Windows.Forms.Button
    Friend WithEvents ckbUpdatePreviewDuringFit As System.Windows.Forms.CheckBox
End Class
