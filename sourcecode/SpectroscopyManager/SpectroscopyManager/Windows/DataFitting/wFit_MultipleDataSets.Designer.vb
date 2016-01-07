<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wFit_MultipleDataSets
    Inherits SpectroscopyManager.wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wFit_MultipleDataSets))
        Me.gbFitModels_Set1 = New System.Windows.Forms.GroupBox()
        Me.flpFitModels_Set1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbImportExportFitModels = New System.Windows.Forms.GroupBox()
        Me.btnImportFitModels = New System.Windows.Forms.Button()
        Me.btnExportFitModels = New System.Windows.Forms.Button()
        Me.gbFitRange_Set1 = New System.Windows.Forms.GroupBox()
        Me.btnSelectFitRange_Set1 = New System.Windows.Forms.Button()
        Me.txtFitRange_RightValue_Set1 = New SpectroscopyManager.NumericTextbox()
        Me.txtFitRange_LeftValue_Set1 = New SpectroscopyManager.NumericTextbox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnAddFitModel_Set1 = New System.Windows.Forms.Button()
        Me.cboAddFitModel = New System.Windows.Forms.ComboBox()
        Me.gbSaveData = New System.Windows.Forms.GroupBox()
        Me.ckbSaveGenerateDataOnlyInSelectedFitRange = New System.Windows.Forms.CheckBox()
        Me.lblSavingProgress = New System.Windows.Forms.Label()
        Me.btnSaveFitProcedureOutput_Set1 = New System.Windows.Forms.Button()
        Me.btnSaveFitProcedureOutput_Set2 = New System.Windows.Forms.Button()
        Me.btnSaveFitToSpectroscopyTable_Set1 = New System.Windows.Forms.Button()
        Me.btnSaveFitToSpectroscopyTable_Set2 = New System.Windows.Forms.Button()
        Me.pgbSavingProgress = New System.Windows.Forms.ProgressBar()
        Me.gbSettings = New System.Windows.Forms.GroupBox()
        Me.ckbUpdatePreviewDuringFit = New System.Windows.Forms.CheckBox()
        Me.btnSetPreviewPointNumber = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.nudPreviewPoints = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.gbSourceDataSelector_Set1 = New System.Windows.Forms.GroupBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cbY_Set1 = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.cbX_Set1 = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.pbPreview_Set1 = New SpectroscopyManager.mSpectroscopyTableViewer()
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
        Me.gbAddFitModels = New System.Windows.Forms.GroupBox()
        Me.btnAddFitModel_Set2 = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.scFitPreviewForBothModels = New System.Windows.Forms.SplitContainer()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.gbFitRange_Set2 = New System.Windows.Forms.GroupBox()
        Me.btnSelectFitRange_Set2 = New System.Windows.Forms.Button()
        Me.txtFitRange_RightValue_Set2 = New SpectroscopyManager.NumericTextbox()
        Me.txtFitRange_LeftValue_Set2 = New SpectroscopyManager.NumericTextbox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gbSourceDataSelector_Set2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbY_Set2 = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.cbX_Set2 = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.pbPreview_Set2 = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.scFitModelsForBothSets = New System.Windows.Forms.SplitContainer()
        Me.gbFitModels_Set2 = New System.Windows.Forms.GroupBox()
        Me.flpFitModels_Set2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbFitModels_Set1.SuspendLayout()
        Me.gbImportExportFitModels.SuspendLayout()
        Me.gbFitRange_Set1.SuspendLayout()
        Me.gbSaveData.SuspendLayout()
        Me.gbSettings.SuspendLayout()
        CType(Me.nudPreviewPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.gbSourceDataSelector_Set1.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        Me.gbFitProcedure.SuspendLayout()
        Me.gbProgress.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.gbAddFitModels.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.scFitPreviewForBothModels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scFitPreviewForBothModels.Panel1.SuspendLayout()
        Me.scFitPreviewForBothModels.Panel2.SuspendLayout()
        Me.scFitPreviewForBothModels.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.gbFitRange_Set2.SuspendLayout()
        Me.gbSourceDataSelector_Set2.SuspendLayout()
        CType(Me.scFitModelsForBothSets, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scFitModelsForBothSets.Panel1.SuspendLayout()
        Me.scFitModelsForBothSets.Panel2.SuspendLayout()
        Me.scFitModelsForBothSets.SuspendLayout()
        Me.gbFitModels_Set2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbFitModels_Set1
        '
        Me.gbFitModels_Set1.Controls.Add(Me.flpFitModels_Set1)
        Me.gbFitModels_Set1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbFitModels_Set1.Location = New System.Drawing.Point(0, 0)
        Me.gbFitModels_Set1.Name = "gbFitModels_Set1"
        Me.gbFitModels_Set1.Size = New System.Drawing.Size(987, 371)
        Me.gbFitModels_Set1.TabIndex = 15
        Me.gbFitModels_Set1.TabStop = False
        Me.gbFitModels_Set1.Text = "Fit-Models"
        '
        'flpFitModels_Set1
        '
        Me.flpFitModels_Set1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpFitModels_Set1.AutoScroll = True
        Me.flpFitModels_Set1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpFitModels_Set1.Location = New System.Drawing.Point(6, 15)
        Me.flpFitModels_Set1.Name = "flpFitModels_Set1"
        Me.flpFitModels_Set1.Size = New System.Drawing.Size(976, 350)
        Me.flpFitModels_Set1.TabIndex = 0
        '
        'gbImportExportFitModels
        '
        Me.gbImportExportFitModels.Controls.Add(Me.btnImportFitModels)
        Me.gbImportExportFitModels.Controls.Add(Me.btnExportFitModels)
        Me.gbImportExportFitModels.Location = New System.Drawing.Point(527, 3)
        Me.gbImportExportFitModels.Name = "gbImportExportFitModels"
        Me.gbImportExportFitModels.Size = New System.Drawing.Size(202, 109)
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
        Me.btnImportFitModels.Size = New System.Drawing.Size(190, 37)
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
        Me.btnExportFitModels.Size = New System.Drawing.Size(190, 37)
        Me.btnExportFitModels.TabIndex = 5
        Me.btnExportFitModels.TabStop = False
        Me.btnExportFitModels.Text = "export fit models"
        Me.btnExportFitModels.UseVisualStyleBackColor = True
        '
        'gbFitRange_Set1
        '
        Me.gbFitRange_Set1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbFitRange_Set1.Controls.Add(Me.btnSelectFitRange_Set1)
        Me.gbFitRange_Set1.Controls.Add(Me.txtFitRange_RightValue_Set1)
        Me.gbFitRange_Set1.Controls.Add(Me.txtFitRange_LeftValue_Set1)
        Me.gbFitRange_Set1.Controls.Add(Me.Label4)
        Me.gbFitRange_Set1.Location = New System.Drawing.Point(6, 293)
        Me.gbFitRange_Set1.Name = "gbFitRange_Set1"
        Me.gbFitRange_Set1.Size = New System.Drawing.Size(245, 72)
        Me.gbFitRange_Set1.TabIndex = 4
        Me.gbFitRange_Set1.TabStop = False
        Me.gbFitRange_Set1.Text = "Fit-Range"
        '
        'btnSelectFitRange_Set1
        '
        Me.btnSelectFitRange_Set1.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_16
        Me.btnSelectFitRange_Set1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectFitRange_Set1.Location = New System.Drawing.Point(14, 43)
        Me.btnSelectFitRange_Set1.Name = "btnSelectFitRange_Set1"
        Me.btnSelectFitRange_Set1.Size = New System.Drawing.Size(219, 23)
        Me.btnSelectFitRange_Set1.TabIndex = 4
        Me.btnSelectFitRange_Set1.TabStop = False
        Me.btnSelectFitRange_Set1.Text = "select fit range"
        Me.btnSelectFitRange_Set1.UseVisualStyleBackColor = True
        '
        'txtFitRange_RightValue_Set1
        '
        Me.txtFitRange_RightValue_Set1.BackColor = System.Drawing.Color.White
        Me.txtFitRange_RightValue_Set1.ForeColor = System.Drawing.Color.Black
        Me.txtFitRange_RightValue_Set1.FormatDecimalPlaces = 6
        Me.txtFitRange_RightValue_Set1.Location = New System.Drawing.Point(133, 17)
        Me.txtFitRange_RightValue_Set1.Name = "txtFitRange_RightValue_Set1"
        Me.txtFitRange_RightValue_Set1.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtFitRange_RightValue_Set1.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtFitRange_RightValue_Set1.Size = New System.Drawing.Size(100, 20)
        Me.txtFitRange_RightValue_Set1.TabIndex = 1
        Me.txtFitRange_RightValue_Set1.Text = "0.000000"
        Me.txtFitRange_RightValue_Set1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFitRange_RightValue_Set1.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtFitRange_LeftValue_Set1
        '
        Me.txtFitRange_LeftValue_Set1.BackColor = System.Drawing.Color.White
        Me.txtFitRange_LeftValue_Set1.ForeColor = System.Drawing.Color.Black
        Me.txtFitRange_LeftValue_Set1.FormatDecimalPlaces = 6
        Me.txtFitRange_LeftValue_Set1.Location = New System.Drawing.Point(14, 17)
        Me.txtFitRange_LeftValue_Set1.Name = "txtFitRange_LeftValue_Set1"
        Me.txtFitRange_LeftValue_Set1.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtFitRange_LeftValue_Set1.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtFitRange_LeftValue_Set1.Size = New System.Drawing.Size(100, 20)
        Me.txtFitRange_LeftValue_Set1.TabIndex = 0
        Me.txtFitRange_LeftValue_Set1.Text = "0.000000"
        Me.txtFitRange_LeftValue_Set1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFitRange_LeftValue_Set1.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
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
        'btnAddFitModel_Set1
        '
        Me.btnAddFitModel_Set1.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddFitModel_Set1.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnAddFitModel_Set1.Location = New System.Drawing.Point(32, 39)
        Me.btnAddFitModel_Set1.Name = "btnAddFitModel_Set1"
        Me.btnAddFitModel_Set1.Size = New System.Drawing.Size(93, 23)
        Me.btnAddFitModel_Set1.TabIndex = 2
        Me.btnAddFitModel_Set1.TabStop = False
        Me.btnAddFitModel_Set1.Text = "add to set 1"
        Me.btnAddFitModel_Set1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddFitModel_Set1.UseVisualStyleBackColor = True
        '
        'cboAddFitModel
        '
        Me.cboAddFitModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAddFitModel.DropDownWidth = 250
        Me.cboAddFitModel.FormattingEnabled = True
        Me.cboAddFitModel.Location = New System.Drawing.Point(10, 16)
        Me.cboAddFitModel.Name = "cboAddFitModel"
        Me.cboAddFitModel.Size = New System.Drawing.Size(236, 21)
        Me.cboAddFitModel.TabIndex = 1
        Me.cboAddFitModel.TabStop = False
        '
        'gbSaveData
        '
        Me.gbSaveData.Controls.Add(Me.ckbSaveGenerateDataOnlyInSelectedFitRange)
        Me.gbSaveData.Controls.Add(Me.lblSavingProgress)
        Me.gbSaveData.Controls.Add(Me.btnSaveFitProcedureOutput_Set1)
        Me.gbSaveData.Controls.Add(Me.btnSaveFitProcedureOutput_Set2)
        Me.gbSaveData.Controls.Add(Me.btnSaveFitToSpectroscopyTable_Set1)
        Me.gbSaveData.Controls.Add(Me.btnSaveFitToSpectroscopyTable_Set2)
        Me.gbSaveData.Controls.Add(Me.pgbSavingProgress)
        Me.gbSaveData.Location = New System.Drawing.Point(277, 3)
        Me.gbSaveData.Name = "gbSaveData"
        Me.gbSaveData.Size = New System.Drawing.Size(244, 161)
        Me.gbSaveData.TabIndex = 6
        Me.gbSaveData.TabStop = False
        Me.gbSaveData.Text = "saving fitted data"
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
        Me.lblSavingProgress.Location = New System.Drawing.Point(7, 96)
        Me.lblSavingProgress.Name = "lblSavingProgress"
        Me.lblSavingProgress.Size = New System.Drawing.Size(16, 13)
        Me.lblSavingProgress.TabIndex = 18
        Me.lblSavingProgress.Text = "..."
        '
        'btnSaveFitProcedureOutput_Set1
        '
        Me.btnSaveFitProcedureOutput_Set1.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveFitProcedureOutput_Set1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveFitProcedureOutput_Set1.Location = New System.Drawing.Point(10, 116)
        Me.btnSaveFitProcedureOutput_Set1.Name = "btnSaveFitProcedureOutput_Set1"
        Me.btnSaveFitProcedureOutput_Set1.Size = New System.Drawing.Size(113, 37)
        Me.btnSaveFitProcedureOutput_Set1.TabIndex = 5
        Me.btnSaveFitProcedureOutput_Set1.Text = "save output for" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "data set 1"
        Me.btnSaveFitProcedureOutput_Set1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveFitProcedureOutput_Set1.UseVisualStyleBackColor = True
        '
        'btnSaveFitProcedureOutput_Set2
        '
        Me.btnSaveFitProcedureOutput_Set2.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveFitProcedureOutput_Set2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveFitProcedureOutput_Set2.Location = New System.Drawing.Point(125, 116)
        Me.btnSaveFitProcedureOutput_Set2.Name = "btnSaveFitProcedureOutput_Set2"
        Me.btnSaveFitProcedureOutput_Set2.Size = New System.Drawing.Size(112, 37)
        Me.btnSaveFitProcedureOutput_Set2.TabIndex = 5
        Me.btnSaveFitProcedureOutput_Set2.Text = "save output for" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "data set 2"
        Me.btnSaveFitProcedureOutput_Set2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveFitProcedureOutput_Set2.UseVisualStyleBackColor = True
        '
        'btnSaveFitToSpectroscopyTable_Set1
        '
        Me.btnSaveFitToSpectroscopyTable_Set1.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveFitToSpectroscopyTable_Set1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveFitToSpectroscopyTable_Set1.Location = New System.Drawing.Point(8, 36)
        Me.btnSaveFitToSpectroscopyTable_Set1.Name = "btnSaveFitToSpectroscopyTable_Set1"
        Me.btnSaveFitToSpectroscopyTable_Set1.Size = New System.Drawing.Size(114, 37)
        Me.btnSaveFitToSpectroscopyTable_Set1.TabIndex = 5
        Me.btnSaveFitToSpectroscopyTable_Set1.Text = "save fitted" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "data set 1"
        Me.btnSaveFitToSpectroscopyTable_Set1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveFitToSpectroscopyTable_Set1.UseVisualStyleBackColor = True
        '
        'btnSaveFitToSpectroscopyTable_Set2
        '
        Me.btnSaveFitToSpectroscopyTable_Set2.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveFitToSpectroscopyTable_Set2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveFitToSpectroscopyTable_Set2.Location = New System.Drawing.Point(125, 36)
        Me.btnSaveFitToSpectroscopyTable_Set2.Name = "btnSaveFitToSpectroscopyTable_Set2"
        Me.btnSaveFitToSpectroscopyTable_Set2.Size = New System.Drawing.Size(113, 37)
        Me.btnSaveFitToSpectroscopyTable_Set2.TabIndex = 5
        Me.btnSaveFitToSpectroscopyTable_Set2.Text = "save fitted" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "data set 2"
        Me.btnSaveFitToSpectroscopyTable_Set2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveFitToSpectroscopyTable_Set2.UseVisualStyleBackColor = True
        '
        'pgbSavingProgress
        '
        Me.pgbSavingProgress.Location = New System.Drawing.Point(8, 76)
        Me.pgbSavingProgress.Name = "pgbSavingProgress"
        Me.pgbSavingProgress.Size = New System.Drawing.Size(229, 17)
        Me.pgbSavingProgress.TabIndex = 17
        '
        'gbSettings
        '
        Me.gbSettings.Controls.Add(Me.ckbUpdatePreviewDuringFit)
        Me.gbSettings.Controls.Add(Me.btnSetPreviewPointNumber)
        Me.gbSettings.Controls.Add(Me.Label18)
        Me.gbSettings.Controls.Add(Me.nudPreviewPoints)
        Me.gbSettings.Location = New System.Drawing.Point(527, 110)
        Me.gbSettings.Name = "gbSettings"
        Me.gbSettings.Size = New System.Drawing.Size(202, 54)
        Me.gbSettings.TabIndex = 14
        Me.gbSettings.TabStop = False
        Me.gbSettings.Text = "Preview-Settings:"
        '
        'ckbUpdatePreviewDuringFit
        '
        Me.ckbUpdatePreviewDuringFit.AutoSize = True
        Me.ckbUpdatePreviewDuringFit.Checked = Global.SpectroscopyManager.My.MySettings.Default.Fit_UpdatePreviewDuringFit
        Me.ckbUpdatePreviewDuringFit.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.SpectroscopyManager.My.MySettings.Default, "Fit_UpdatePreviewDuringFit", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.ckbUpdatePreviewDuringFit.Location = New System.Drawing.Point(50, 35)
        Me.ckbUpdatePreviewDuringFit.Name = "ckbUpdatePreviewDuringFit"
        Me.ckbUpdatePreviewDuringFit.Size = New System.Drawing.Size(142, 17)
        Me.ckbUpdatePreviewDuringFit.TabIndex = 4
        Me.ckbUpdatePreviewDuringFit.Text = "update preview during fit"
        Me.ckbUpdatePreviewDuringFit.UseVisualStyleBackColor = True
        '
        'btnSetPreviewPointNumber
        '
        Me.btnSetPreviewPointNumber.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_16
        Me.btnSetPreviewPointNumber.Location = New System.Drawing.Point(160, 11)
        Me.btnSetPreviewPointNumber.Name = "btnSetPreviewPointNumber"
        Me.btnSetPreviewPointNumber.Size = New System.Drawing.Size(31, 23)
        Me.btnSetPreviewPointNumber.TabIndex = 2
        Me.btnSetPreviewPointNumber.TabStop = False
        Me.btnSetPreviewPointNumber.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(7, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(78, 13)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "preview points:"
        '
        'nudPreviewPoints
        '
        Me.nudPreviewPoints.Location = New System.Drawing.Point(87, 13)
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
        Me.GroupBox4.Controls.Add(Me.gbFitRange_Set1)
        Me.GroupBox4.Controls.Add(Me.gbSourceDataSelector_Set1)
        Me.GroupBox4.Controls.Add(Me.pbPreview_Set1)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(495, 371)
        Me.GroupBox4.TabIndex = 12
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Data-Set 1"
        '
        'gbSourceDataSelector_Set1
        '
        Me.gbSourceDataSelector_Set1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbSourceDataSelector_Set1.Controls.Add(Me.Label15)
        Me.gbSourceDataSelector_Set1.Controls.Add(Me.Label14)
        Me.gbSourceDataSelector_Set1.Controls.Add(Me.cbY_Set1)
        Me.gbSourceDataSelector_Set1.Controls.Add(Me.cbX_Set1)
        Me.gbSourceDataSelector_Set1.Location = New System.Drawing.Point(257, 293)
        Me.gbSourceDataSelector_Set1.Name = "gbSourceDataSelector_Set1"
        Me.gbSourceDataSelector_Set1.Size = New System.Drawing.Size(226, 72)
        Me.gbSourceDataSelector_Set1.TabIndex = 13
        Me.gbSourceDataSelector_Set1.TabStop = False
        Me.gbSourceDataSelector_Set1.Text = "Source Data"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 49)
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
        'cbY_Set1
        '
        Me.cbY_Set1.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.cbY_Set1.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbY_Set1.Location = New System.Drawing.Point(26, 46)
        Me.cbY_Set1.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbY_Set1.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbY_Set1.Name = "cbY_Set1"
        Me.cbY_Set1.SelectedColumnName = ""
        Me.cbY_Set1.SelectedColumnNames = CType(resources.GetObject("cbY_Set1.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbY_Set1.SelectedEntries = CType(resources.GetObject("cbY_Set1.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbY_Set1.SelectedEntry = ""
        Me.cbY_Set1.Size = New System.Drawing.Size(194, 21)
        Me.cbY_Set1.TabIndex = 1
        Me.cbY_Set1.TabStop = False
        Me.cbY_Set1.TurnOnLastFilterSaving = False
        Me.cbY_Set1.TurnOnLastSelectionSaving = False
        '
        'cbX_Set1
        '
        Me.cbX_Set1.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.cbX_Set1.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbX_Set1.Location = New System.Drawing.Point(27, 19)
        Me.cbX_Set1.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbX_Set1.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbX_Set1.Name = "cbX_Set1"
        Me.cbX_Set1.SelectedColumnName = ""
        Me.cbX_Set1.SelectedColumnNames = CType(resources.GetObject("cbX_Set1.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbX_Set1.SelectedEntries = CType(resources.GetObject("cbX_Set1.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbX_Set1.SelectedEntry = ""
        Me.cbX_Set1.Size = New System.Drawing.Size(193, 21)
        Me.cbX_Set1.TabIndex = 0
        Me.cbX_Set1.TabStop = False
        Me.cbX_Set1.TurnOnLastFilterSaving = False
        Me.cbX_Set1.TurnOnLastSelectionSaving = False
        '
        'pbPreview_Set1
        '
        Me.pbPreview_Set1.AllowAdjustingXColumn = True
        Me.pbPreview_Set1.AllowAdjustingYColumn = True
        Me.pbPreview_Set1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPreview_Set1.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbPreview_Set1.CallbackXRangeSelected = Nothing
        Me.pbPreview_Set1.CallbackXValueSelected = Nothing
        Me.pbPreview_Set1.CallbackXYRangeSelected = Nothing
        Me.pbPreview_Set1.CallbackYRangeSelected = Nothing
        Me.pbPreview_Set1.CallbackYValueSelected = Nothing
        Me.pbPreview_Set1.ClearPointSelectionModeAfterSelection = False
        Me.pbPreview_Set1.Location = New System.Drawing.Point(3, 16)
        Me.pbPreview_Set1.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbPreview_Set1.Name = "pbPreview_Set1"
        Me.pbPreview_Set1.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbPreview_Set1.ShowColumnSelectors = False
        Me.pbPreview_Set1.Size = New System.Drawing.Size(485, 270)
        Me.pbPreview_Set1.TabIndex = 0
        Me.pbPreview_Set1.TurnOnLastFilterSaving_Y = False
        Me.pbPreview_Set1.TurnOnLastSelectionSaving_Y = False
        '
        'gbOutput
        '
        Me.gbOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOutput.Controls.Add(Me.txtFitEcho)
        Me.gbOutput.Location = New System.Drawing.Point(735, 3)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Size = New System.Drawing.Size(745, 185)
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
        Me.txtFitEcho.Size = New System.Drawing.Size(736, 163)
        Me.txtFitEcho.TabIndex = 2
        Me.txtFitEcho.TabStop = False
        Me.txtFitEcho.WordWrap = False
        '
        'lblFitProgress
        '
        Me.lblFitProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFitProgress.AutoSize = True
        Me.lblFitProgress.Location = New System.Drawing.Point(10, 36)
        Me.lblFitProgress.Name = "lblFitProgress"
        Me.lblFitProgress.Size = New System.Drawing.Size(16, 13)
        Me.lblFitProgress.TabIndex = 18
        Me.lblFitProgress.Text = "..."
        '
        'pgbFitProgress
        '
        Me.pgbFitProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbFitProgress.Location = New System.Drawing.Point(11, 20)
        Me.pgbFitProgress.Name = "pgbFitProgress"
        Me.pgbFitProgress.Size = New System.Drawing.Size(236, 12)
        Me.pgbFitProgress.TabIndex = 17
        '
        'cboFitProcedure
        '
        Me.cboFitProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFitProcedure.FormattingEnabled = True
        Me.cboFitProcedure.Location = New System.Drawing.Point(10, 19)
        Me.cboFitProcedure.Name = "cboFitProcedure"
        Me.cboFitProcedure.Size = New System.Drawing.Size(249, 21)
        Me.cboFitProcedure.TabIndex = 0
        Me.cboFitProcedure.TabStop = False
        '
        'btnStartFitting
        '
        Me.btnStartFitting.Enabled = False
        Me.btnStartFitting.Image = Global.SpectroscopyManager.My.Resources.Resources.run_25
        Me.btnStartFitting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStartFitting.Location = New System.Drawing.Point(12, 72)
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
        Me.gbFitProcedure.Size = New System.Drawing.Size(268, 120)
        Me.gbFitProcedure.TabIndex = 16
        Me.gbFitProcedure.TabStop = False
        Me.gbFitProcedure.Text = "Fit-Procedure"
        '
        'gbProgress
        '
        Me.gbProgress.Controls.Add(Me.lblFitProgress)
        Me.gbProgress.Controls.Add(Me.pgbFitProgress)
        Me.gbProgress.Location = New System.Drawing.Point(6, 16)
        Me.gbProgress.Name = "gbProgress"
        Me.gbProgress.Size = New System.Drawing.Size(253, 53)
        Me.gbProgress.TabIndex = 23
        Me.gbProgress.TabStop = False
        Me.gbProgress.Text = "Progress"
        Me.gbProgress.Visible = False
        '
        'btnChangeParallelizationSettings
        '
        Me.btnChangeParallelizationSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.btnChangeParallelizationSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeParallelizationSettings.Location = New System.Drawing.Point(160, 46)
        Me.btnChangeParallelizationSettings.Name = "btnChangeParallelizationSettings"
        Me.btnChangeParallelizationSettings.Size = New System.Drawing.Size(99, 23)
        Me.btnChangeParallelizationSettings.TabIndex = 24
        Me.btnChangeParallelizationSettings.Text = "performance"
        Me.btnChangeParallelizationSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnChangeParallelizationSettings.UseVisualStyleBackColor = True
        '
        'btnChangeFitProcedureSettings
        '
        Me.btnChangeFitProcedureSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.btnChangeFitProcedureSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeFitProcedureSettings.Location = New System.Drawing.Point(10, 46)
        Me.btnChangeFitProcedureSettings.Name = "btnChangeFitProcedureSettings"
        Me.btnChangeFitProcedureSettings.Size = New System.Drawing.Size(144, 23)
        Me.btnChangeFitProcedureSettings.TabIndex = 24
        Me.btnChangeFitProcedureSettings.Text = "fit-procedure settings"
        Me.btnChangeFitProcedureSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnChangeFitProcedureSettings.UseVisualStyleBackColor = True
        '
        'btnAddFitToFitQueue
        '
        Me.btnAddFitToFitQueue.Enabled = False
        Me.btnAddFitToFitQueue.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddFitToFitQueue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddFitToFitQueue.Location = New System.Drawing.Point(160, 72)
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
        Me.scMain.Panel1.Controls.Add(Me.gbImportExportFitModels)
        Me.scMain.Panel1.Controls.Add(Me.gbFitProcedure)
        Me.scMain.Panel1.Controls.Add(Me.gbSettings)
        Me.scMain.Panel1.Controls.Add(Me.gbOutput)
        Me.scMain.Panel1.Controls.Add(Me.gbSaveData)
        Me.scMain.Panel1.Controls.Add(Me.gbAddFitModels)
        Me.scMain.Panel1MinSize = 192
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.SplitContainer1)
        Me.scMain.Panel2MinSize = 400
        Me.scMain.Size = New System.Drawing.Size(1486, 941)
        Me.scMain.SplitterDistance = 192
        Me.scMain.TabIndex = 7
        '
        'gbAddFitModels
        '
        Me.gbAddFitModels.Controls.Add(Me.btnAddFitModel_Set2)
        Me.gbAddFitModels.Controls.Add(Me.cboAddFitModel)
        Me.gbAddFitModels.Controls.Add(Me.btnAddFitModel_Set1)
        Me.gbAddFitModels.Location = New System.Drawing.Point(5, 124)
        Me.gbAddFitModels.Name = "gbAddFitModels"
        Me.gbAddFitModels.Size = New System.Drawing.Size(266, 65)
        Me.gbAddFitModels.TabIndex = 17
        Me.gbAddFitModels.TabStop = False
        Me.gbAddFitModels.Text = "add fit models"
        '
        'btnAddFitModel_Set2
        '
        Me.btnAddFitModel_Set2.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddFitModel_Set2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnAddFitModel_Set2.Location = New System.Drawing.Point(131, 39)
        Me.btnAddFitModel_Set2.Name = "btnAddFitModel_Set2"
        Me.btnAddFitModel_Set2.Size = New System.Drawing.Size(93, 23)
        Me.btnAddFitModel_Set2.TabIndex = 2
        Me.btnAddFitModel_Set2.TabStop = False
        Me.btnAddFitModel_Set2.Text = "add to set 2"
        Me.btnAddFitModel_Set2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddFitModel_Set2.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.scFitPreviewForBothModels)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.scFitModelsForBothSets)
        Me.SplitContainer1.Size = New System.Drawing.Size(1486, 745)
        Me.SplitContainer1.SplitterDistance = 495
        Me.SplitContainer1.TabIndex = 16
        '
        'scFitPreviewForBothModels
        '
        Me.scFitPreviewForBothModels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scFitPreviewForBothModels.IsSplitterFixed = True
        Me.scFitPreviewForBothModels.Location = New System.Drawing.Point(0, 0)
        Me.scFitPreviewForBothModels.Name = "scFitPreviewForBothModels"
        Me.scFitPreviewForBothModels.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scFitPreviewForBothModels.Panel1
        '
        Me.scFitPreviewForBothModels.Panel1.Controls.Add(Me.GroupBox4)
        '
        'scFitPreviewForBothModels.Panel2
        '
        Me.scFitPreviewForBothModels.Panel2.Controls.Add(Me.GroupBox3)
        Me.scFitPreviewForBothModels.Size = New System.Drawing.Size(495, 745)
        Me.scFitPreviewForBothModels.SplitterDistance = 371
        Me.scFitPreviewForBothModels.TabIndex = 13
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.gbFitRange_Set2)
        Me.GroupBox3.Controls.Add(Me.gbSourceDataSelector_Set2)
        Me.GroupBox3.Controls.Add(Me.pbPreview_Set2)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(495, 370)
        Me.GroupBox3.TabIndex = 13
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Data-Set 2:"
        '
        'gbFitRange_Set2
        '
        Me.gbFitRange_Set2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbFitRange_Set2.Controls.Add(Me.btnSelectFitRange_Set2)
        Me.gbFitRange_Set2.Controls.Add(Me.txtFitRange_RightValue_Set2)
        Me.gbFitRange_Set2.Controls.Add(Me.txtFitRange_LeftValue_Set2)
        Me.gbFitRange_Set2.Controls.Add(Me.Label2)
        Me.gbFitRange_Set2.Location = New System.Drawing.Point(6, 292)
        Me.gbFitRange_Set2.Name = "gbFitRange_Set2"
        Me.gbFitRange_Set2.Size = New System.Drawing.Size(245, 72)
        Me.gbFitRange_Set2.TabIndex = 4
        Me.gbFitRange_Set2.TabStop = False
        Me.gbFitRange_Set2.Text = "Fit-Range"
        '
        'btnSelectFitRange_Set2
        '
        Me.btnSelectFitRange_Set2.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_16
        Me.btnSelectFitRange_Set2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelectFitRange_Set2.Location = New System.Drawing.Point(14, 43)
        Me.btnSelectFitRange_Set2.Name = "btnSelectFitRange_Set2"
        Me.btnSelectFitRange_Set2.Size = New System.Drawing.Size(219, 23)
        Me.btnSelectFitRange_Set2.TabIndex = 4
        Me.btnSelectFitRange_Set2.TabStop = False
        Me.btnSelectFitRange_Set2.Text = "select fit range"
        Me.btnSelectFitRange_Set2.UseVisualStyleBackColor = True
        '
        'txtFitRange_RightValue_Set2
        '
        Me.txtFitRange_RightValue_Set2.BackColor = System.Drawing.Color.White
        Me.txtFitRange_RightValue_Set2.ForeColor = System.Drawing.Color.Black
        Me.txtFitRange_RightValue_Set2.FormatDecimalPlaces = 6
        Me.txtFitRange_RightValue_Set2.Location = New System.Drawing.Point(133, 17)
        Me.txtFitRange_RightValue_Set2.Name = "txtFitRange_RightValue_Set2"
        Me.txtFitRange_RightValue_Set2.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtFitRange_RightValue_Set2.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtFitRange_RightValue_Set2.Size = New System.Drawing.Size(100, 20)
        Me.txtFitRange_RightValue_Set2.TabIndex = 1
        Me.txtFitRange_RightValue_Set2.Text = "0.000000"
        Me.txtFitRange_RightValue_Set2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFitRange_RightValue_Set2.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtFitRange_LeftValue_Set2
        '
        Me.txtFitRange_LeftValue_Set2.BackColor = System.Drawing.Color.White
        Me.txtFitRange_LeftValue_Set2.ForeColor = System.Drawing.Color.Black
        Me.txtFitRange_LeftValue_Set2.FormatDecimalPlaces = 6
        Me.txtFitRange_LeftValue_Set2.Location = New System.Drawing.Point(14, 17)
        Me.txtFitRange_LeftValue_Set2.Name = "txtFitRange_LeftValue_Set2"
        Me.txtFitRange_LeftValue_Set2.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtFitRange_LeftValue_Set2.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtFitRange_LeftValue_Set2.Size = New System.Drawing.Size(100, 20)
        Me.txtFitRange_LeftValue_Set2.TabIndex = 0
        Me.txtFitRange_LeftValue_Set2.Text = "0.000000"
        Me.txtFitRange_LeftValue_Set2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFitRange_LeftValue_Set2.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(116, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "to"
        '
        'gbSourceDataSelector_Set2
        '
        Me.gbSourceDataSelector_Set2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbSourceDataSelector_Set2.Controls.Add(Me.Label3)
        Me.gbSourceDataSelector_Set2.Controls.Add(Me.Label5)
        Me.gbSourceDataSelector_Set2.Controls.Add(Me.cbY_Set2)
        Me.gbSourceDataSelector_Set2.Controls.Add(Me.cbX_Set2)
        Me.gbSourceDataSelector_Set2.Location = New System.Drawing.Point(257, 292)
        Me.gbSourceDataSelector_Set2.Name = "gbSourceDataSelector_Set2"
        Me.gbSourceDataSelector_Set2.Size = New System.Drawing.Size(226, 72)
        Me.gbSourceDataSelector_Set2.TabIndex = 13
        Me.gbSourceDataSelector_Set2.TabStop = False
        Me.gbSourceDataSelector_Set2.Text = "Source Data"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(14, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Y"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(14, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "X"
        '
        'cbY_Set2
        '
        Me.cbY_Set2.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.cbY_Set2.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbY_Set2.Location = New System.Drawing.Point(26, 46)
        Me.cbY_Set2.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbY_Set2.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbY_Set2.Name = "cbY_Set2"
        Me.cbY_Set2.SelectedColumnName = ""
        Me.cbY_Set2.SelectedColumnNames = CType(resources.GetObject("cbY_Set2.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbY_Set2.SelectedEntries = CType(resources.GetObject("cbY_Set2.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbY_Set2.SelectedEntry = ""
        Me.cbY_Set2.Size = New System.Drawing.Size(194, 21)
        Me.cbY_Set2.TabIndex = 1
        Me.cbY_Set2.TabStop = False
        Me.cbY_Set2.TurnOnLastFilterSaving = False
        Me.cbY_Set2.TurnOnLastSelectionSaving = False
        '
        'cbX_Set2
        '
        Me.cbX_Set2.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.cbX_Set2.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbX_Set2.Location = New System.Drawing.Point(27, 19)
        Me.cbX_Set2.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbX_Set2.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbX_Set2.Name = "cbX_Set2"
        Me.cbX_Set2.SelectedColumnName = ""
        Me.cbX_Set2.SelectedColumnNames = CType(resources.GetObject("cbX_Set2.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbX_Set2.SelectedEntries = CType(resources.GetObject("cbX_Set2.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbX_Set2.SelectedEntry = ""
        Me.cbX_Set2.Size = New System.Drawing.Size(193, 21)
        Me.cbX_Set2.TabIndex = 0
        Me.cbX_Set2.TabStop = False
        Me.cbX_Set2.TurnOnLastFilterSaving = False
        Me.cbX_Set2.TurnOnLastSelectionSaving = False
        '
        'pbPreview_Set2
        '
        Me.pbPreview_Set2.AllowAdjustingXColumn = True
        Me.pbPreview_Set2.AllowAdjustingYColumn = True
        Me.pbPreview_Set2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPreview_Set2.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbPreview_Set2.CallbackXRangeSelected = Nothing
        Me.pbPreview_Set2.CallbackXValueSelected = Nothing
        Me.pbPreview_Set2.CallbackXYRangeSelected = Nothing
        Me.pbPreview_Set2.CallbackYRangeSelected = Nothing
        Me.pbPreview_Set2.CallbackYValueSelected = Nothing
        Me.pbPreview_Set2.ClearPointSelectionModeAfterSelection = False
        Me.pbPreview_Set2.Location = New System.Drawing.Point(3, 16)
        Me.pbPreview_Set2.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbPreview_Set2.Name = "pbPreview_Set2"
        Me.pbPreview_Set2.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbPreview_Set2.ShowColumnSelectors = False
        Me.pbPreview_Set2.Size = New System.Drawing.Size(485, 269)
        Me.pbPreview_Set2.TabIndex = 0
        Me.pbPreview_Set2.TurnOnLastFilterSaving_Y = False
        Me.pbPreview_Set2.TurnOnLastSelectionSaving_Y = False
        '
        'scFitModelsForBothSets
        '
        Me.scFitModelsForBothSets.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scFitModelsForBothSets.IsSplitterFixed = True
        Me.scFitModelsForBothSets.Location = New System.Drawing.Point(0, 0)
        Me.scFitModelsForBothSets.Name = "scFitModelsForBothSets"
        Me.scFitModelsForBothSets.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scFitModelsForBothSets.Panel1
        '
        Me.scFitModelsForBothSets.Panel1.Controls.Add(Me.gbFitModels_Set1)
        '
        'scFitModelsForBothSets.Panel2
        '
        Me.scFitModelsForBothSets.Panel2.Controls.Add(Me.gbFitModels_Set2)
        Me.scFitModelsForBothSets.Size = New System.Drawing.Size(987, 745)
        Me.scFitModelsForBothSets.SplitterDistance = 371
        Me.scFitModelsForBothSets.TabIndex = 16
        '
        'gbFitModels_Set2
        '
        Me.gbFitModels_Set2.Controls.Add(Me.flpFitModels_Set2)
        Me.gbFitModels_Set2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbFitModels_Set2.Location = New System.Drawing.Point(0, 0)
        Me.gbFitModels_Set2.Name = "gbFitModels_Set2"
        Me.gbFitModels_Set2.Size = New System.Drawing.Size(987, 370)
        Me.gbFitModels_Set2.TabIndex = 16
        Me.gbFitModels_Set2.TabStop = False
        Me.gbFitModels_Set2.Text = "Fit-Models"
        '
        'flpFitModels_Set2
        '
        Me.flpFitModels_Set2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpFitModels_Set2.AutoScroll = True
        Me.flpFitModels_Set2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpFitModels_Set2.Location = New System.Drawing.Point(6, 15)
        Me.flpFitModels_Set2.Name = "flpFitModels_Set2"
        Me.flpFitModels_Set2.Size = New System.Drawing.Size(976, 349)
        Me.flpFitModels_Set2.TabIndex = 0
        '
        'wFit_MultipleDataSets
        '
        Me.ClientSize = New System.Drawing.Size(1486, 941)
        Me.Controls.Add(Me.scMain)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(1280, 980)
        Me.Name = "wFit_MultipleDataSets"
        Me.Text = "Non-Linear Fit - "
        Me.Controls.SetChildIndex(Me.scMain, 0)
        Me.gbFitModels_Set1.ResumeLayout(False)
        Me.gbImportExportFitModels.ResumeLayout(False)
        Me.gbFitRange_Set1.ResumeLayout(False)
        Me.gbFitRange_Set1.PerformLayout()
        Me.gbSaveData.ResumeLayout(False)
        Me.gbSaveData.PerformLayout()
        Me.gbSettings.ResumeLayout(False)
        Me.gbSettings.PerformLayout()
        CType(Me.nudPreviewPoints, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.gbSourceDataSelector_Set1.ResumeLayout(False)
        Me.gbSourceDataSelector_Set1.PerformLayout()
        Me.gbOutput.ResumeLayout(False)
        Me.gbOutput.PerformLayout()
        Me.gbFitProcedure.ResumeLayout(False)
        Me.gbProgress.ResumeLayout(False)
        Me.gbProgress.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.gbAddFitModels.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.scFitPreviewForBothModels.Panel1.ResumeLayout(False)
        Me.scFitPreviewForBothModels.Panel2.ResumeLayout(False)
        CType(Me.scFitPreviewForBothModels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scFitPreviewForBothModels.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.gbFitRange_Set2.ResumeLayout(False)
        Me.gbFitRange_Set2.PerformLayout()
        Me.gbSourceDataSelector_Set2.ResumeLayout(False)
        Me.gbSourceDataSelector_Set2.PerformLayout()
        Me.scFitModelsForBothSets.Panel1.ResumeLayout(False)
        Me.scFitModelsForBothSets.Panel2.ResumeLayout(False)
        CType(Me.scFitModelsForBothSets, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scFitModelsForBothSets.ResumeLayout(False)
        Me.gbFitModels_Set2.ResumeLayout(False)
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
    Friend WithEvents gbSourceDataSelector_Set1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cbY_Set1 As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents cbX_Set1 As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents gbFitModels_Set1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnAddFitModel_Set1 As System.Windows.Forms.Button
    Friend WithEvents cboAddFitModel As System.Windows.Forms.ComboBox
    Friend WithEvents flpFitModels_Set1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnStartFitting As System.Windows.Forms.Button
    Friend WithEvents cboFitProcedure As System.Windows.Forms.ComboBox
    Friend WithEvents pbPreview_Set1 As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents gbFitProcedure As System.Windows.Forms.GroupBox
    Friend WithEvents gbProgress As System.Windows.Forms.GroupBox
    Friend WithEvents gbFitRange_Set1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectFitRange_Set1 As System.Windows.Forms.Button
    Friend WithEvents txtFitRange_RightValue_Set1 As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtFitRange_LeftValue_Set1 As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSaveFitToSpectroscopyTable_Set2 As System.Windows.Forms.Button
    Friend WithEvents btnExportFitModels As System.Windows.Forms.Button
    Friend WithEvents btnImportFitModels As System.Windows.Forms.Button
    Friend WithEvents gbSaveData As System.Windows.Forms.GroupBox
    Friend WithEvents gbImportExportFitModels As System.Windows.Forms.GroupBox
    Friend WithEvents lblSavingProgress As System.Windows.Forms.Label
    Friend WithEvents pgbSavingProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents btnChangeFitProcedureSettings As System.Windows.Forms.Button
    Friend WithEvents btnSaveFitProcedureOutput_Set2 As System.Windows.Forms.Button
    Friend WithEvents ckbSaveGenerateDataOnlyInSelectedFitRange As System.Windows.Forms.CheckBox
    Friend WithEvents btnAddFitToFitQueue As System.Windows.Forms.Button
    Friend WithEvents btnChangeParallelizationSettings As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnAddFitModel_Set2 As System.Windows.Forms.Button
    Friend WithEvents scFitPreviewForBothModels As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents gbFitRange_Set2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectFitRange_Set2 As System.Windows.Forms.Button
    Friend WithEvents txtFitRange_RightValue_Set2 As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtFitRange_LeftValue_Set2 As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents gbSourceDataSelector_Set2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbY_Set2 As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents cbX_Set2 As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbPreview_Set2 As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents scFitModelsForBothSets As System.Windows.Forms.SplitContainer
    Friend WithEvents gbFitModels_Set2 As System.Windows.Forms.GroupBox
    Friend WithEvents flpFitModels_Set2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents ckbUpdatePreviewDuringFit As System.Windows.Forms.CheckBox
    Friend WithEvents btnSaveFitToSpectroscopyTable_Set1 As System.Windows.Forms.Button
    Friend WithEvents btnSaveFitProcedureOutput_Set1 As System.Windows.Forms.Button
    Friend WithEvents gbAddFitModels As System.Windows.Forms.GroupBox
End Class
