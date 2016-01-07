<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wFitBCSDoubleGap3
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wFitBCSDoubleGap3))
        Me.zgFitResult = New ZedGraph.ZedGraphControl()
        Me.zgTipDOS = New ZedGraph.ZedGraphControl()
        Me.btnStartFitting = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.FitEcho = New System.Windows.Forms.TextBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.gbSaveFit = New System.Windows.Forms.GroupBox()
        Me.btnSaveColumns = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtColNameFermiFunction = New System.Windows.Forms.TextBox()
        Me.txtColNameSampleDOS = New System.Windows.Forms.TextBox()
        Me.txtColNameTipDOS = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtColNameFitResult = New System.Windows.Forms.TextBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.zgSampleDOS = New ZedGraph.ZedGraphControl()
        Me.gbParameters = New System.Windows.Forms.GroupBox()
        Me.cboFitDataType = New System.Windows.Forms.ComboBox()
        Me.cboBroadeningType = New System.Windows.Forms.ComboBox()
        Me.txtFitRangeMax = New SpectroscopyManager.NumericTextbox()
        Me.txtFitRangeMin = New SpectroscopyManager.NumericTextbox()
        Me.txtImaginaryDamping = New SpectroscopyManager.NumericTextbox()
        Me.txtBroadeningWidth = New SpectroscopyManager.NumericTextbox()
        Me.txtYStretch = New SpectroscopyManager.NumericTextbox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtYOffset = New SpectroscopyManager.NumericTextbox()
        Me.txtXOffset = New SpectroscopyManager.NumericTextbox()
        Me.txtGapRatio2vs1 = New SpectroscopyManager.NumericTextbox()
        Me.txtSampleGap2 = New SpectroscopyManager.NumericTextbox()
        Me.txtSampleGap1 = New SpectroscopyManager.NumericTextbox()
        Me.btnSelectFitRange = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnSelectStretchFactor = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnSelectYOffset = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnSelectXOffset = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnSelectGapRatio = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnSelectSampleGap2 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnSelectSampleGap1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gbTipParameters = New System.Windows.Forms.GroupBox()
        Me.txtTemperature = New SpectroscopyManager.NumericTextbox()
        Me.txtTipGap = New SpectroscopyManager.NumericTextbox()
        Me.scParameters = New System.Windows.Forms.SplitContainer()
        Me.scFit = New System.Windows.Forms.SplitContainer()
        Me.gbSettings = New System.Windows.Forms.GroupBox()
        Me.btnSetPreviewPointNumber = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.nudPreviewPoints = New System.Windows.Forms.NumericUpDown()
        Me.gbSourceDataSelector = New System.Windows.Forms.GroupBox()
        Me.ckbYCorrection = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cbY = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.cbX = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.scDOSPanel = New System.Windows.Forms.SplitContainer()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.gbSaveFit.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.gbParameters.SuspendLayout()
        Me.gbTipParameters.SuspendLayout()
        CType(Me.scParameters, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scParameters.Panel1.SuspendLayout()
        Me.scParameters.Panel2.SuspendLayout()
        Me.scParameters.SuspendLayout()
        CType(Me.scFit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scFit.Panel1.SuspendLayout()
        Me.scFit.Panel2.SuspendLayout()
        Me.scFit.SuspendLayout()
        Me.gbSettings.SuspendLayout()
        CType(Me.nudPreviewPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSourceDataSelector.SuspendLayout()
        CType(Me.scDOSPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scDOSPanel.Panel1.SuspendLayout()
        Me.scDOSPanel.Panel2.SuspendLayout()
        Me.scDOSPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'zgFitResult
        '
        Me.zgFitResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgFitResult.Location = New System.Drawing.Point(3, 16)
        Me.zgFitResult.Name = "zgFitResult"
        Me.zgFitResult.ScrollGrace = 0.0R
        Me.zgFitResult.ScrollMaxX = 0.0R
        Me.zgFitResult.ScrollMaxY = 0.0R
        Me.zgFitResult.ScrollMaxY2 = 0.0R
        Me.zgFitResult.ScrollMinX = 0.0R
        Me.zgFitResult.ScrollMinY = 0.0R
        Me.zgFitResult.ScrollMinY2 = 0.0R
        Me.zgFitResult.Size = New System.Drawing.Size(580, 450)
        Me.zgFitResult.TabIndex = 0
        '
        'zgTipDOS
        '
        Me.zgTipDOS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgTipDOS.Location = New System.Drawing.Point(3, 16)
        Me.zgTipDOS.Name = "zgTipDOS"
        Me.zgTipDOS.ScrollGrace = 0.0R
        Me.zgTipDOS.ScrollMaxX = 0.0R
        Me.zgTipDOS.ScrollMaxY = 0.0R
        Me.zgTipDOS.ScrollMaxY2 = 0.0R
        Me.zgTipDOS.ScrollMinX = 0.0R
        Me.zgTipDOS.ScrollMinY = 0.0R
        Me.zgTipDOS.ScrollMinY2 = 0.0R
        Me.zgTipDOS.Size = New System.Drawing.Size(424, 298)
        Me.zgTipDOS.TabIndex = 0
        '
        'btnStartFitting
        '
        Me.btnStartFitting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartFitting.Enabled = False
        Me.btnStartFitting.Image = Global.SpectroscopyManager.My.Resources.Resources.run_25
        Me.btnStartFitting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStartFitting.Location = New System.Drawing.Point(594, 400)
        Me.btnStartFitting.Name = "btnStartFitting"
        Me.btnStartFitting.Size = New System.Drawing.Size(232, 39)
        Me.btnStartFitting.TabIndex = 16
        Me.btnStartFitting.Text = "Start Fitting"
        Me.btnStartFitting.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.zgTipDOS)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(430, 317)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Tip Density of States"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.FitEcho)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(838, 338)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Fit Output:"
        '
        'FitEcho
        '
        Me.FitEcho.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FitEcho.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FitEcho.Location = New System.Drawing.Point(3, 16)
        Me.FitEcho.Multiline = True
        Me.FitEcho.Name = "FitEcho"
        Me.FitEcho.ReadOnly = True
        Me.FitEcho.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.FitEcho.Size = New System.Drawing.Size(832, 319)
        Me.FitEcho.TabIndex = 2
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.zgFitResult)
        Me.GroupBox4.Location = New System.Drawing.Point(3, 93)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(586, 469)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Fit-Result / -Preview"
        '
        'gbSaveFit
        '
        Me.gbSaveFit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSaveFit.Controls.Add(Me.btnSaveColumns)
        Me.gbSaveFit.Controls.Add(Me.Label19)
        Me.gbSaveFit.Controls.Add(Me.Label17)
        Me.gbSaveFit.Controls.Add(Me.Label16)
        Me.gbSaveFit.Controls.Add(Me.txtColNameFermiFunction)
        Me.gbSaveFit.Controls.Add(Me.txtColNameSampleDOS)
        Me.gbSaveFit.Controls.Add(Me.txtColNameTipDOS)
        Me.gbSaveFit.Controls.Add(Me.Label12)
        Me.gbSaveFit.Controls.Add(Me.txtColNameFitResult)
        Me.gbSaveFit.Location = New System.Drawing.Point(9, 345)
        Me.gbSaveFit.Name = "gbSaveFit"
        Me.gbSaveFit.Size = New System.Drawing.Size(417, 98)
        Me.gbSaveFit.TabIndex = 1
        Me.gbSaveFit.TabStop = False
        Me.gbSaveFit.Text = "Save Fit:"
        '
        'btnSaveColumns
        '
        Me.btnSaveColumns.Enabled = False
        Me.btnSaveColumns.Image = Global.SpectroscopyManager.My.Resources.Resources.save_16
        Me.btnSaveColumns.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumns.Location = New System.Drawing.Point(16, 71)
        Me.btnSaveColumns.Name = "btnSaveColumns"
        Me.btnSaveColumns.Size = New System.Drawing.Size(389, 20)
        Me.btnSaveColumns.TabIndex = 20
        Me.btnSaveColumns.Text = "save DOS and fit using current set of parameters"
        Me.btnSaveColumns.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(210, 22)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(86, 13)
        Me.Label19.TabIndex = 11
        Me.Label19.Text = "Save Fermi-f. as:"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(210, 48)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(93, 13)
        Me.Label17.TabIndex = 11
        Me.Label17.Text = "Save Tip-DOS as:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(7, 48)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(93, 13)
        Me.Label16.TabIndex = 11
        Me.Label16.Text = "Save Tip-DOS as:"
        '
        'txtColNameFermiFunction
        '
        Me.txtColNameFermiFunction.Location = New System.Drawing.Point(305, 19)
        Me.txtColNameFermiFunction.Name = "txtColNameFermiFunction"
        Me.txtColNameFermiFunction.Size = New System.Drawing.Size(100, 20)
        Me.txtColNameFermiFunction.TabIndex = 19
        Me.txtColNameFermiFunction.Text = "Fermi Function"
        '
        'txtColNameSampleDOS
        '
        Me.txtColNameSampleDOS.Location = New System.Drawing.Point(305, 45)
        Me.txtColNameSampleDOS.Name = "txtColNameSampleDOS"
        Me.txtColNameSampleDOS.Size = New System.Drawing.Size(100, 20)
        Me.txtColNameSampleDOS.TabIndex = 19
        Me.txtColNameSampleDOS.Text = "Sample DOS"
        '
        'txtColNameTipDOS
        '
        Me.txtColNameTipDOS.Location = New System.Drawing.Point(102, 45)
        Me.txtColNameTipDOS.Name = "txtColNameTipDOS"
        Me.txtColNameTipDOS.Size = New System.Drawing.Size(100, 20)
        Me.txtColNameTipDOS.TabIndex = 18
        Me.txtColNameTipDOS.Text = "Tip DOS"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(7, 22)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(63, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Save Fit as:"
        '
        'txtColNameFitResult
        '
        Me.txtColNameFitResult.Location = New System.Drawing.Point(102, 19)
        Me.txtColNameFitResult.Name = "txtColNameFitResult"
        Me.txtColNameFitResult.Size = New System.Drawing.Size(100, 20)
        Me.txtColNameFitResult.TabIndex = 17
        Me.txtColNameFitResult.Text = "Fit result"
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.Controls.Add(Me.zgSampleDOS)
        Me.GroupBox6.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(423, 336)
        Me.GroupBox6.TabIndex = 6
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Sample DOS:"
        '
        'zgSampleDOS
        '
        Me.zgSampleDOS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgSampleDOS.Location = New System.Drawing.Point(3, 16)
        Me.zgSampleDOS.Name = "zgSampleDOS"
        Me.zgSampleDOS.ScrollGrace = 0.0R
        Me.zgSampleDOS.ScrollMaxX = 0.0R
        Me.zgSampleDOS.ScrollMaxY = 0.0R
        Me.zgSampleDOS.ScrollMaxY2 = 0.0R
        Me.zgSampleDOS.ScrollMinX = 0.0R
        Me.zgSampleDOS.ScrollMinY = 0.0R
        Me.zgSampleDOS.ScrollMinY2 = 0.0R
        Me.zgSampleDOS.Size = New System.Drawing.Size(417, 317)
        Me.zgSampleDOS.TabIndex = 0
        '
        'gbParameters
        '
        Me.gbParameters.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbParameters.Controls.Add(Me.cboFitDataType)
        Me.gbParameters.Controls.Add(Me.cboBroadeningType)
        Me.gbParameters.Controls.Add(Me.txtFitRangeMax)
        Me.gbParameters.Controls.Add(Me.txtFitRangeMin)
        Me.gbParameters.Controls.Add(Me.txtImaginaryDamping)
        Me.gbParameters.Controls.Add(Me.txtBroadeningWidth)
        Me.gbParameters.Controls.Add(Me.txtYStretch)
        Me.gbParameters.Controls.Add(Me.Label20)
        Me.gbParameters.Controls.Add(Me.Label7)
        Me.gbParameters.Controls.Add(Me.Label6)
        Me.gbParameters.Controls.Add(Me.txtYOffset)
        Me.gbParameters.Controls.Add(Me.txtXOffset)
        Me.gbParameters.Controls.Add(Me.txtGapRatio2vs1)
        Me.gbParameters.Controls.Add(Me.txtSampleGap2)
        Me.gbParameters.Controls.Add(Me.txtSampleGap1)
        Me.gbParameters.Controls.Add(Me.btnSelectFitRange)
        Me.gbParameters.Controls.Add(Me.Label13)
        Me.gbParameters.Controls.Add(Me.Label11)
        Me.gbParameters.Controls.Add(Me.btnSelectStretchFactor)
        Me.gbParameters.Controls.Add(Me.Label10)
        Me.gbParameters.Controls.Add(Me.btnSelectYOffset)
        Me.gbParameters.Controls.Add(Me.Label9)
        Me.gbParameters.Controls.Add(Me.btnSelectXOffset)
        Me.gbParameters.Controls.Add(Me.Label8)
        Me.gbParameters.Controls.Add(Me.btnSelectGapRatio)
        Me.gbParameters.Controls.Add(Me.Label5)
        Me.gbParameters.Controls.Add(Me.btnSelectSampleGap2)
        Me.gbParameters.Controls.Add(Me.Label4)
        Me.gbParameters.Controls.Add(Me.btnSelectSampleGap1)
        Me.gbParameters.Controls.Add(Me.Label3)
        Me.gbParameters.Enabled = False
        Me.gbParameters.Location = New System.Drawing.Point(592, 8)
        Me.gbParameters.Name = "gbParameters"
        Me.gbParameters.Size = New System.Drawing.Size(239, 386)
        Me.gbParameters.TabIndex = 8
        Me.gbParameters.TabStop = False
        Me.gbParameters.Text = "define initial set of parameters"
        '
        'cboFitDataType
        '
        Me.cboFitDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFitDataType.FormattingEnabled = True
        Me.cboFitDataType.Location = New System.Drawing.Point(120, 244)
        Me.cboFitDataType.Name = "cboFitDataType"
        Me.cboFitDataType.Size = New System.Drawing.Size(114, 21)
        Me.cboFitDataType.TabIndex = 11
        '
        'cboBroadeningType
        '
        Me.cboBroadeningType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBroadeningType.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBroadeningType.FormattingEnabled = True
        Me.cboBroadeningType.Location = New System.Drawing.Point(9, 296)
        Me.cboBroadeningType.Name = "cboBroadeningType"
        Me.cboBroadeningType.Size = New System.Drawing.Size(224, 20)
        Me.cboBroadeningType.TabIndex = 12
        '
        'txtFitRangeMax
        '
        Me.txtFitRangeMax.AllowSpace = False
        Me.txtFitRangeMax.FormatString = "N6"
        Me.txtFitRangeMax.Location = New System.Drawing.Point(114, 217)
        Me.txtFitRangeMax.MaxLength = 10
        Me.txtFitRangeMax.Name = "txtFitRangeMax"
        Me.txtFitRangeMax.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtFitRangeMax.Size = New System.Drawing.Size(73, 20)
        Me.txtFitRangeMax.TabIndex = 10
        Me.txtFitRangeMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFitRangeMax.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtFitRangeMin
        '
        Me.txtFitRangeMin.AllowSpace = False
        Me.txtFitRangeMin.FormatString = "N6"
        Me.txtFitRangeMin.Location = New System.Drawing.Point(114, 192)
        Me.txtFitRangeMin.MaxLength = 10
        Me.txtFitRangeMin.Name = "txtFitRangeMin"
        Me.txtFitRangeMin.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Negative
        Me.txtFitRangeMin.Size = New System.Drawing.Size(73, 20)
        Me.txtFitRangeMin.TabIndex = 9
        Me.txtFitRangeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFitRangeMin.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtImaginaryDamping
        '
        Me.txtImaginaryDamping.AllowSpace = False
        Me.txtImaginaryDamping.BackColor = System.Drawing.Color.White
        Me.txtImaginaryDamping.FormatString = "N6"
        Me.txtImaginaryDamping.Location = New System.Drawing.Point(130, 349)
        Me.txtImaginaryDamping.MaxLength = 10
        Me.txtImaginaryDamping.Name = "txtImaginaryDamping"
        Me.txtImaginaryDamping.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtImaginaryDamping.Size = New System.Drawing.Size(73, 20)
        Me.txtImaginaryDamping.TabIndex = 13
        Me.txtImaginaryDamping.Text = "0.5"
        Me.txtImaginaryDamping.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtImaginaryDamping.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtBroadeningWidth
        '
        Me.txtBroadeningWidth.AllowSpace = False
        Me.txtBroadeningWidth.BackColor = System.Drawing.Color.White
        Me.txtBroadeningWidth.FormatString = "N6"
        Me.txtBroadeningWidth.Location = New System.Drawing.Point(130, 323)
        Me.txtBroadeningWidth.MaxLength = 10
        Me.txtBroadeningWidth.Name = "txtBroadeningWidth"
        Me.txtBroadeningWidth.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtBroadeningWidth.Size = New System.Drawing.Size(73, 20)
        Me.txtBroadeningWidth.TabIndex = 13
        Me.txtBroadeningWidth.Text = "0.5"
        Me.txtBroadeningWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtBroadeningWidth.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtYStretch
        '
        Me.txtYStretch.AllowSpace = False
        Me.txtYStretch.BackColor = System.Drawing.Color.White
        Me.txtYStretch.FormatString = "N6"
        Me.txtYStretch.Location = New System.Drawing.Point(114, 163)
        Me.txtYStretch.MaxLength = 10
        Me.txtYStretch.Name = "txtYStretch"
        Me.txtYStretch.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtYStretch.Size = New System.Drawing.Size(73, 20)
        Me.txtYStretch.TabIndex = 8
        Me.txtYStretch.Text = "1"
        Me.txtYStretch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtYStretch.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(30, 352)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(100, 13)
        Me.Label20.TabIndex = 0
        Me.Label20.Text = "Imaginary Damping:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 274)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Broadening:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 326)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(102, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Broad. Width [meV]:"
        '
        'txtYOffset
        '
        Me.txtYOffset.AllowSpace = False
        Me.txtYOffset.BackColor = System.Drawing.Color.White
        Me.txtYOffset.FormatString = "N6"
        Me.txtYOffset.Location = New System.Drawing.Point(114, 134)
        Me.txtYOffset.MaxLength = 10
        Me.txtYOffset.Name = "txtYOffset"
        Me.txtYOffset.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtYOffset.Size = New System.Drawing.Size(73, 20)
        Me.txtYOffset.TabIndex = 7
        Me.txtYOffset.Text = "0"
        Me.txtYOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtYOffset.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtXOffset
        '
        Me.txtXOffset.AllowSpace = False
        Me.txtXOffset.BackColor = System.Drawing.Color.White
        Me.txtXOffset.FormatString = "N6"
        Me.txtXOffset.Location = New System.Drawing.Point(114, 105)
        Me.txtXOffset.MaxLength = 10
        Me.txtXOffset.Name = "txtXOffset"
        Me.txtXOffset.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtXOffset.Size = New System.Drawing.Size(73, 20)
        Me.txtXOffset.TabIndex = 6
        Me.txtXOffset.Text = "0"
        Me.txtXOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtXOffset.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtGapRatio2vs1
        '
        Me.txtGapRatio2vs1.AllowSpace = False
        Me.txtGapRatio2vs1.BackColor = System.Drawing.Color.White
        Me.txtGapRatio2vs1.FormatString = "N6"
        Me.txtGapRatio2vs1.Location = New System.Drawing.Point(114, 76)
        Me.txtGapRatio2vs1.MaxLength = 10
        Me.txtGapRatio2vs1.Name = "txtGapRatio2vs1"
        Me.txtGapRatio2vs1.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtGapRatio2vs1.Size = New System.Drawing.Size(73, 20)
        Me.txtGapRatio2vs1.TabIndex = 5
        Me.txtGapRatio2vs1.Text = "0.1"
        Me.txtGapRatio2vs1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGapRatio2vs1.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtSampleGap2
        '
        Me.txtSampleGap2.AllowSpace = False
        Me.txtSampleGap2.BackColor = System.Drawing.Color.White
        Me.txtSampleGap2.FormatString = "N6"
        Me.txtSampleGap2.Location = New System.Drawing.Point(114, 47)
        Me.txtSampleGap2.MaxLength = 10
        Me.txtSampleGap2.Name = "txtSampleGap2"
        Me.txtSampleGap2.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtSampleGap2.Size = New System.Drawing.Size(73, 20)
        Me.txtSampleGap2.TabIndex = 4
        Me.txtSampleGap2.Text = "1.42"
        Me.txtSampleGap2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSampleGap2.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtSampleGap1
        '
        Me.txtSampleGap1.AllowSpace = False
        Me.txtSampleGap1.BackColor = System.Drawing.Color.White
        Me.txtSampleGap1.FormatString = "N6"
        Me.txtSampleGap1.Location = New System.Drawing.Point(114, 18)
        Me.txtSampleGap1.MaxLength = 10
        Me.txtSampleGap1.Name = "txtSampleGap1"
        Me.txtSampleGap1.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtSampleGap1.Size = New System.Drawing.Size(73, 20)
        Me.txtSampleGap1.TabIndex = 3
        Me.txtSampleGap1.Text = "1.32"
        Me.txtSampleGap1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSampleGap1.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'btnSelectFitRange
        '
        Me.btnSelectFitRange.Enabled = False
        Me.btnSelectFitRange.Location = New System.Drawing.Point(189, 203)
        Me.btnSelectFitRange.Name = "btnSelectFitRange"
        Me.btnSelectFitRange.Size = New System.Drawing.Size(45, 23)
        Me.btnSelectFitRange.TabIndex = 1
        Me.btnSelectFitRange.Text = "select"
        Me.btnSelectFitRange.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 247)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(74, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Fit Data Type:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 195)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(86, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Fit-Range [meV]:"
        '
        'btnSelectStretchFactor
        '
        Me.btnSelectStretchFactor.Enabled = False
        Me.btnSelectStretchFactor.Location = New System.Drawing.Point(189, 161)
        Me.btnSelectStretchFactor.Name = "btnSelectStretchFactor"
        Me.btnSelectStretchFactor.Size = New System.Drawing.Size(45, 23)
        Me.btnSelectStretchFactor.TabIndex = 1
        Me.btnSelectStretchFactor.Text = "select"
        Me.btnSelectStretchFactor.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 166)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Y Stretch Factor:"
        '
        'btnSelectYOffset
        '
        Me.btnSelectYOffset.Enabled = False
        Me.btnSelectYOffset.Location = New System.Drawing.Point(189, 132)
        Me.btnSelectYOffset.Name = "btnSelectYOffset"
        Me.btnSelectYOffset.Size = New System.Drawing.Size(45, 23)
        Me.btnSelectYOffset.TabIndex = 1
        Me.btnSelectYOffset.Text = "select"
        Me.btnSelectYOffset.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 137)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(81, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Global Y Offset:"
        Me.Label9.UseMnemonic = False
        '
        'btnSelectXOffset
        '
        Me.btnSelectXOffset.Enabled = False
        Me.btnSelectXOffset.Location = New System.Drawing.Point(189, 103)
        Me.btnSelectXOffset.Name = "btnSelectXOffset"
        Me.btnSelectXOffset.Size = New System.Drawing.Size(45, 23)
        Me.btnSelectXOffset.TabIndex = 1
        Me.btnSelectXOffset.Text = "select"
        Me.btnSelectXOffset.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 108)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(111, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Global X Offset [meV]:"
        '
        'btnSelectGapRatio
        '
        Me.btnSelectGapRatio.Enabled = False
        Me.btnSelectGapRatio.Location = New System.Drawing.Point(189, 74)
        Me.btnSelectGapRatio.Name = "btnSelectGapRatio"
        Me.btnSelectGapRatio.Size = New System.Drawing.Size(45, 23)
        Me.btnSelectGapRatio.TabIndex = 1
        Me.btnSelectGapRatio.Text = "select"
        Me.btnSelectGapRatio.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 79)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(101, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Gap Ratio Gap 2/1:"
        '
        'btnSelectSampleGap2
        '
        Me.btnSelectSampleGap2.Enabled = False
        Me.btnSelectSampleGap2.Location = New System.Drawing.Point(189, 45)
        Me.btnSelectSampleGap2.Name = "btnSelectSampleGap2"
        Me.btnSelectSampleGap2.Size = New System.Drawing.Size(45, 23)
        Me.btnSelectSampleGap2.TabIndex = 1
        Me.btnSelectSampleGap2.Text = "select"
        Me.btnSelectSampleGap2.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(107, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Sample Gap 2 [meV]:"
        '
        'btnSelectSampleGap1
        '
        Me.btnSelectSampleGap1.Enabled = False
        Me.btnSelectSampleGap1.Location = New System.Drawing.Point(189, 16)
        Me.btnSelectSampleGap1.Name = "btnSelectSampleGap1"
        Me.btnSelectSampleGap1.Size = New System.Drawing.Size(45, 23)
        Me.btnSelectSampleGap1.TabIndex = 1
        Me.btnSelectSampleGap1.Text = "select"
        Me.btnSelectSampleGap1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Sample Gap 1 [meV]:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Temperature [K]:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(206, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tip Gap [meV]:"
        '
        'gbTipParameters
        '
        Me.gbTipParameters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTipParameters.Controls.Add(Me.Label1)
        Me.gbTipParameters.Controls.Add(Me.Label2)
        Me.gbTipParameters.Controls.Add(Me.txtTemperature)
        Me.gbTipParameters.Controls.Add(Me.txtTipGap)
        Me.gbTipParameters.Location = New System.Drawing.Point(3, 326)
        Me.gbTipParameters.Name = "gbTipParameters"
        Me.gbTipParameters.Size = New System.Drawing.Size(421, 58)
        Me.gbTipParameters.TabIndex = 9
        Me.gbTipParameters.TabStop = False
        Me.gbTipParameters.Text = "Define Tip Parameters:"
        '
        'txtTemperature
        '
        Me.txtTemperature.AllowSpace = False
        Me.txtTemperature.BackColor = System.Drawing.Color.White
        Me.txtTemperature.FormatString = "N6"
        Me.txtTemperature.Location = New System.Drawing.Point(110, 24)
        Me.txtTemperature.MaxLength = 10
        Me.txtTemperature.Name = "txtTemperature"
        Me.txtTemperature.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtTemperature.Size = New System.Drawing.Size(73, 20)
        Me.txtTemperature.TabIndex = 14
        Me.txtTemperature.Text = "1.19"
        Me.txtTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTemperature.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'txtTipGap
        '
        Me.txtTipGap.AllowSpace = False
        Me.txtTipGap.BackColor = System.Drawing.Color.White
        Me.txtTipGap.FormatString = "N6"
        Me.txtTipGap.Location = New System.Drawing.Point(302, 24)
        Me.txtTipGap.MaxLength = 10
        Me.txtTipGap.Name = "txtTipGap"
        Me.txtTipGap.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtTipGap.Size = New System.Drawing.Size(73, 20)
        Me.txtTipGap.TabIndex = 15
        Me.txtTipGap.Text = "1.34"
        Me.txtTipGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTipGap.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.DecimalValue
        '
        'scParameters
        '
        Me.scParameters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scParameters.Location = New System.Drawing.Point(0, 0)
        Me.scParameters.Name = "scParameters"
        '
        'scParameters.Panel1
        '
        Me.scParameters.Panel1.Controls.Add(Me.scFit)
        '
        'scParameters.Panel2
        '
        Me.scParameters.Panel2.Controls.Add(Me.scDOSPanel)
        Me.scParameters.Size = New System.Drawing.Size(1278, 907)
        Me.scParameters.SplitterDistance = 838
        Me.scParameters.TabIndex = 10
        '
        'scFit
        '
        Me.scFit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scFit.Location = New System.Drawing.Point(0, 0)
        Me.scFit.Name = "scFit"
        Me.scFit.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scFit.Panel1
        '
        Me.scFit.Panel1.Controls.Add(Me.gbParameters)
        Me.scFit.Panel1.Controls.Add(Me.btnStartFitting)
        Me.scFit.Panel1.Controls.Add(Me.gbSettings)
        Me.scFit.Panel1.Controls.Add(Me.GroupBox4)
        Me.scFit.Panel1.Controls.Add(Me.gbSourceDataSelector)
        '
        'scFit.Panel2
        '
        Me.scFit.Panel2.Controls.Add(Me.GroupBox3)
        Me.scFit.Size = New System.Drawing.Size(838, 907)
        Me.scFit.SplitterDistance = 565
        Me.scFit.TabIndex = 12
        '
        'gbSettings
        '
        Me.gbSettings.Controls.Add(Me.btnSetPreviewPointNumber)
        Me.gbSettings.Controls.Add(Me.Label18)
        Me.gbSettings.Controls.Add(Me.nudPreviewPoints)
        Me.gbSettings.Location = New System.Drawing.Point(321, 8)
        Me.gbSettings.Name = "gbSettings"
        Me.gbSettings.Size = New System.Drawing.Size(257, 79)
        Me.gbSettings.TabIndex = 11
        Me.gbSettings.TabStop = False
        Me.gbSettings.Text = "Settings:"
        '
        'btnSetPreviewPointNumber
        '
        Me.btnSetPreviewPointNumber.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_16
        Me.btnSetPreviewPointNumber.Location = New System.Drawing.Point(216, 16)
        Me.btnSetPreviewPointNumber.Name = "btnSetPreviewPointNumber"
        Me.btnSetPreviewPointNumber.Size = New System.Drawing.Size(31, 23)
        Me.btnSetPreviewPointNumber.TabIndex = 2
        Me.btnSetPreviewPointNumber.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(7, 20)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(130, 13)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "Number of preview points:"
        '
        'nudPreviewPoints
        '
        Me.nudPreviewPoints.Location = New System.Drawing.Point(143, 18)
        Me.nudPreviewPoints.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPreviewPoints.Name = "nudPreviewPoints"
        Me.nudPreviewPoints.Size = New System.Drawing.Size(67, 20)
        Me.nudPreviewPoints.TabIndex = 2
        Me.nudPreviewPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudPreviewPoints.Value = New Decimal(New Integer() {500, 0, 0, 0})
        '
        'gbSourceDataSelector
        '
        Me.gbSourceDataSelector.Controls.Add(Me.ckbYCorrection)
        Me.gbSourceDataSelector.Controls.Add(Me.Label15)
        Me.gbSourceDataSelector.Controls.Add(Me.Label14)
        Me.gbSourceDataSelector.Controls.Add(Me.cbY)
        Me.gbSourceDataSelector.Controls.Add(Me.cbX)
        Me.gbSourceDataSelector.Location = New System.Drawing.Point(6, 8)
        Me.gbSourceDataSelector.Name = "gbSourceDataSelector"
        Me.gbSourceDataSelector.Size = New System.Drawing.Size(309, 79)
        Me.gbSourceDataSelector.TabIndex = 10
        Me.gbSourceDataSelector.TabStop = False
        Me.gbSourceDataSelector.Text = "Experimental Source Data"
        '
        'ckbYCorrection
        '
        Me.ckbYCorrection.AutoSize = True
        Me.ckbYCorrection.CheckAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbYCorrection.Location = New System.Drawing.Point(250, 45)
        Me.ckbYCorrection.Name = "ckbYCorrection"
        Me.ckbYCorrection.Size = New System.Drawing.Size(59, 31)
        Me.ckbYCorrection.TabIndex = 11
        Me.ckbYCorrection.Text = "Correction"
        Me.ckbYCorrection.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(7, 49)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(55, 13)
        Me.Label15.TabIndex = 10
        Me.Label15.Text = "I or dI/dV:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(7, 22)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(30, 13)
        Me.Label14.TabIndex = 10
        Me.Label14.Text = "Bias:"
        '
        'cbY
        '
        Me.cbY.Location = New System.Drawing.Point(65, 46)
        Me.cbY.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbY.Name = "cbY"
        Me.cbY.SelectedColumnIndex = 0
        Me.cbY.SelectedColumnName = ""
        Me.cbY.Size = New System.Drawing.Size(178, 21)
        Me.cbY.TabIndex = 1
        '
        'cbX
        '
        Me.cbX.Location = New System.Drawing.Point(65, 19)
        Me.cbX.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.cbX.Name = "cbX"
        Me.cbX.SelectedColumnIndex = 0
        Me.cbX.SelectedColumnName = ""
        Me.cbX.Size = New System.Drawing.Size(178, 21)
        Me.cbX.TabIndex = 0
        '
        'scDOSPanel
        '
        Me.scDOSPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scDOSPanel.Location = New System.Drawing.Point(0, 0)
        Me.scDOSPanel.Name = "scDOSPanel"
        Me.scDOSPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scDOSPanel.Panel1
        '
        Me.scDOSPanel.Panel1.Controls.Add(Me.gbTipParameters)
        Me.scDOSPanel.Panel1.Controls.Add(Me.GroupBox2)
        '
        'scDOSPanel.Panel2
        '
        Me.scDOSPanel.Panel2.Controls.Add(Me.gbSaveFit)
        Me.scDOSPanel.Panel2.Controls.Add(Me.GroupBox6)
        Me.scDOSPanel.Size = New System.Drawing.Size(436, 907)
        Me.scDOSPanel.SplitterDistance = 454
        Me.scDOSPanel.TabIndex = 10
        '
        'wFitBCSDoubleGap2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1278, 907)
        Me.Controls.Add(Me.scParameters)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wFitBCSDoubleGap2"
        Me.Text = "Double Gap Fitting 3 (Simplex Method)"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.gbSaveFit.ResumeLayout(False)
        Me.gbSaveFit.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.gbParameters.ResumeLayout(False)
        Me.gbParameters.PerformLayout()
        Me.gbTipParameters.ResumeLayout(False)
        Me.gbTipParameters.PerformLayout()
        Me.scParameters.Panel1.ResumeLayout(False)
        Me.scParameters.Panel2.ResumeLayout(False)
        CType(Me.scParameters, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scParameters.ResumeLayout(False)
        Me.scFit.Panel1.ResumeLayout(False)
        Me.scFit.Panel2.ResumeLayout(False)
        CType(Me.scFit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scFit.ResumeLayout(False)
        Me.gbSettings.ResumeLayout(False)
        Me.gbSettings.PerformLayout()
        CType(Me.nudPreviewPoints, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSourceDataSelector.ResumeLayout(False)
        Me.gbSourceDataSelector.PerformLayout()
        Me.scDOSPanel.Panel1.ResumeLayout(False)
        Me.scDOSPanel.Panel2.ResumeLayout(False)
        CType(Me.scDOSPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scDOSPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents zgFitResult As ZedGraph.ZedGraphControl
    Friend WithEvents zgTipDOS As ZedGraph.ZedGraphControl
    Friend WithEvents btnStartFitting As System.Windows.Forms.Button
    Friend WithEvents FitEcho As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents gbParameters As System.Windows.Forms.GroupBox
    Friend WithEvents txtTemperature As NumericTextbox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFitRangeMax As NumericTextbox
    Friend WithEvents txtFitRangeMin As NumericTextbox
    Friend WithEvents txtYStretch As NumericTextbox
    Friend WithEvents txtYOffset As NumericTextbox
    Friend WithEvents txtXOffset As NumericTextbox
    Friend WithEvents txtBroadeningWidth As NumericTextbox
    Friend WithEvents txtGapRatio2vs1 As NumericTextbox
    Friend WithEvents txtSampleGap2 As NumericTextbox
    Friend WithEvents txtSampleGap1 As NumericTextbox
    Friend WithEvents txtTipGap As NumericTextbox
    Friend WithEvents btnSelectFitRange As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnSelectStretchFactor As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents btnSelectYOffset As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnSelectXOffset As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSelectGapRatio As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSelectSampleGap2 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSelectSampleGap1 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents gbTipParameters As System.Windows.Forms.GroupBox
    Friend WithEvents scParameters As System.Windows.Forms.SplitContainer
    Friend WithEvents btnSaveColumns As System.Windows.Forms.Button
    Friend WithEvents txtColNameFitResult As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cboFitDataType As System.Windows.Forms.ComboBox
    Friend WithEvents cboBroadeningType As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents gbSaveFit As System.Windows.Forms.GroupBox
    Friend WithEvents gbSourceDataSelector As System.Windows.Forms.GroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cbY As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents cbX As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents zgSampleDOS As ZedGraph.ZedGraphControl
    Friend WithEvents gbSettings As System.Windows.Forms.GroupBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtColNameSampleDOS As System.Windows.Forms.TextBox
    Friend WithEvents txtColNameTipDOS As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents nudPreviewPoints As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnSetPreviewPointNumber As System.Windows.Forms.Button
    Friend WithEvents scFit As System.Windows.Forms.SplitContainer
    Friend WithEvents scDOSPanel As System.Windows.Forms.SplitContainer
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtColNameFermiFunction As System.Windows.Forms.TextBox
    Friend WithEvents txtImaginaryDamping As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents ckbYCorrection As System.Windows.Forms.CheckBox
End Class
