<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataRenormalization2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataRenormalization2))
        Me.gbCurrent = New System.Windows.Forms.GroupBox()
        Me.pbSourceData = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.gbdIdV = New System.Windows.Forms.GroupBox()
        Me.pbTargetData = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.dbRegaugedResult = New System.Windows.Forms.GroupBox()
        Me.pbOutput = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.btnSaveColumns = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtRenormalizedDataColumnName = New System.Windows.Forms.TextBox()
        Me.txtSmoothedDataColumnName = New System.Windows.Forms.TextBox()
        Me.ckbSaveDerivedData = New System.Windows.Forms.CheckBox()
        Me.ckbSaveSmoothedData = New System.Windows.Forms.CheckBox()
        Me.txtDerivedDataColumnName = New System.Windows.Forms.TextBox()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.dsDataSmoothing = New SpectroscopyManager.mDataSmoothing()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.panControls = New System.Windows.Forms.Panel()
        Me.gbRegaugeRange = New SpectroscopyManager.GroupBoxCheckable()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtRegaugeRange_xMax = New SpectroscopyManager.NumericTextbox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtRegaugeRange_xMin = New SpectroscopyManager.NumericTextbox()
        Me.btnRegauge = New System.Windows.Forms.Button()
        Me.gbFitResults = New System.Windows.Forms.GroupBox()
        Me.lblNotFittedWarning = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFittedParameter_m = New SpectroscopyManager.NumericTextbox()
        Me.txtFittedParameter_y0 = New SpectroscopyManager.NumericTextbox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.gbCurrent.SuspendLayout()
        Me.gbdIdV.SuspendLayout()
        Me.dbRegaugedResult.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.panControls.SuspendLayout()
        Me.gbRegaugeRange.SuspendLayout()
        Me.gbFitResults.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbCurrent
        '
        Me.gbCurrent.Controls.Add(Me.pbSourceData)
        Me.gbCurrent.Location = New System.Drawing.Point(11, 60)
        Me.gbCurrent.Name = "gbCurrent"
        Me.gbCurrent.Size = New System.Drawing.Size(383, 291)
        Me.gbCurrent.TabIndex = 5
        Me.gbCurrent.TabStop = False
        Me.gbCurrent.Text = "Reference data of which the numeric derivative will be calculated"
        '
        'pbSourceData
        '
        Me.pbSourceData.AllowAdjustingXColumn = True
        Me.pbSourceData.AllowAdjustingYColumn = True
        Me.pbSourceData.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbSourceData.CallbackXRangeSelected = Nothing
        Me.pbSourceData.CallbackXValueSelected = Nothing
        Me.pbSourceData.CallbackXYRangeSelected = Nothing
        Me.pbSourceData.CallbackYRangeSelected = Nothing
        Me.pbSourceData.CallbackYValueSelected = Nothing
        Me.pbSourceData.ClearPointSelectionModeAfterSelection = False
        Me.pbSourceData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbSourceData.Location = New System.Drawing.Point(3, 16)
        Me.pbSourceData.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbSourceData.Name = "pbSourceData"
        Me.pbSourceData.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbSourceData.ShowColumnSelectors = True
        Me.pbSourceData.Size = New System.Drawing.Size(377, 272)
        Me.pbSourceData.TabIndex = 0
        Me.pbSourceData.TurnOnLastFilterSaving_Y = False
        Me.pbSourceData.TurnOnLastSelectionSaving_Y = False
        '
        'gbdIdV
        '
        Me.gbdIdV.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbdIdV.Controls.Add(Me.pbTargetData)
        Me.gbdIdV.Location = New System.Drawing.Point(877, 60)
        Me.gbdIdV.Name = "gbdIdV"
        Me.gbdIdV.Size = New System.Drawing.Size(322, 291)
        Me.gbdIdV.TabIndex = 6
        Me.gbdIdV.TabStop = False
        Me.gbdIdV.Text = "Data to re-gauge by fitting it to the reference data on the left"
        '
        'pbTargetData
        '
        Me.pbTargetData.AllowAdjustingXColumn = True
        Me.pbTargetData.AllowAdjustingYColumn = True
        Me.pbTargetData.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbTargetData.CallbackXRangeSelected = Nothing
        Me.pbTargetData.CallbackXValueSelected = Nothing
        Me.pbTargetData.CallbackXYRangeSelected = Nothing
        Me.pbTargetData.CallbackYRangeSelected = Nothing
        Me.pbTargetData.CallbackYValueSelected = Nothing
        Me.pbTargetData.ClearPointSelectionModeAfterSelection = False
        Me.pbTargetData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbTargetData.Location = New System.Drawing.Point(3, 16)
        Me.pbTargetData.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbTargetData.Name = "pbTargetData"
        Me.pbTargetData.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbTargetData.ShowColumnSelectors = True
        Me.pbTargetData.Size = New System.Drawing.Size(316, 272)
        Me.pbTargetData.TabIndex = 0
        Me.pbTargetData.TurnOnLastFilterSaving_Y = False
        Me.pbTargetData.TurnOnLastSelectionSaving_Y = False
        '
        'dbRegaugedResult
        '
        Me.dbRegaugedResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dbRegaugedResult.Controls.Add(Me.pbOutput)
        Me.dbRegaugedResult.Location = New System.Drawing.Point(626, 424)
        Me.dbRegaugedResult.Name = "dbRegaugedResult"
        Me.dbRegaugedResult.Size = New System.Drawing.Size(571, 355)
        Me.dbRegaugedResult.TabIndex = 7
        Me.dbRegaugedResult.TabStop = False
        Me.dbRegaugedResult.Text = "regauged result"
        '
        'pbOutput
        '
        Me.pbOutput.AllowAdjustingXColumn = True
        Me.pbOutput.AllowAdjustingYColumn = True
        Me.pbOutput.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbOutput.CallbackXRangeSelected = Nothing
        Me.pbOutput.CallbackXValueSelected = Nothing
        Me.pbOutput.CallbackXYRangeSelected = Nothing
        Me.pbOutput.CallbackYRangeSelected = Nothing
        Me.pbOutput.CallbackYValueSelected = Nothing
        Me.pbOutput.ClearPointSelectionModeAfterSelection = False
        Me.pbOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbOutput.Location = New System.Drawing.Point(3, 16)
        Me.pbOutput.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbOutput.Name = "pbOutput"
        Me.pbOutput.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbOutput.ShowColumnSelectors = True
        Me.pbOutput.Size = New System.Drawing.Size(565, 336)
        Me.pbOutput.TabIndex = 0
        Me.pbOutput.TurnOnLastFilterSaving_Y = False
        Me.pbOutput.TurnOnLastSelectionSaving_Y = False
        '
        'btnSaveColumns
        '
        Me.btnSaveColumns.Enabled = False
        Me.btnSaveColumns.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumns.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumns.Location = New System.Drawing.Point(6, 172)
        Me.btnSaveColumns.Name = "btnSaveColumns"
        Me.btnSaveColumns.Size = New System.Drawing.Size(167, 43)
        Me.btnSaveColumns.TabIndex = 8
        Me.btnSaveColumns.Text = "save columns"
        Me.btnSaveColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumns.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.txtRenormalizedDataColumnName)
        Me.GroupBox4.Controls.Add(Me.txtSmoothedDataColumnName)
        Me.GroupBox4.Controls.Add(Me.ckbSaveDerivedData)
        Me.GroupBox4.Controls.Add(Me.ckbSaveSmoothedData)
        Me.GroupBox4.Controls.Add(Me.btnSaveColumns)
        Me.GroupBox4.Controls.Add(Me.txtDerivedDataColumnName)
        Me.GroupBox4.Location = New System.Drawing.Point(444, 424)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(176, 223)
        Me.GroupBox4.TabIndex = 9
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "save regauged results"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Re-gauged Column Name:"
        '
        'txtRenormalizedDataColumnName
        '
        Me.txtRenormalizedDataColumnName.Location = New System.Drawing.Point(6, 35)
        Me.txtRenormalizedDataColumnName.Name = "txtRenormalizedDataColumnName"
        Me.txtRenormalizedDataColumnName.Size = New System.Drawing.Size(167, 20)
        Me.txtRenormalizedDataColumnName.TabIndex = 10
        Me.txtRenormalizedDataColumnName.Text = "Regauged Data"
        '
        'txtSmoothedDataColumnName
        '
        Me.txtSmoothedDataColumnName.Enabled = False
        Me.txtSmoothedDataColumnName.Location = New System.Drawing.Point(6, 94)
        Me.txtSmoothedDataColumnName.Name = "txtSmoothedDataColumnName"
        Me.txtSmoothedDataColumnName.Size = New System.Drawing.Size(167, 20)
        Me.txtSmoothedDataColumnName.TabIndex = 10
        Me.txtSmoothedDataColumnName.Text = "Smoothed Data"
        '
        'ckbSaveDerivedData
        '
        Me.ckbSaveDerivedData.AutoSize = True
        Me.ckbSaveDerivedData.Location = New System.Drawing.Point(6, 127)
        Me.ckbSaveDerivedData.Name = "ckbSaveDerivedData"
        Me.ckbSaveDerivedData.Size = New System.Drawing.Size(140, 17)
        Me.ckbSaveDerivedData.TabIndex = 9
        Me.ckbSaveDerivedData.Text = "saved derivative of data"
        Me.ckbSaveDerivedData.UseVisualStyleBackColor = True
        '
        'ckbSaveSmoothedData
        '
        Me.ckbSaveSmoothedData.AutoSize = True
        Me.ckbSaveSmoothedData.Location = New System.Drawing.Point(6, 73)
        Me.ckbSaveSmoothedData.Name = "ckbSaveSmoothedData"
        Me.ckbSaveSmoothedData.Size = New System.Drawing.Size(122, 17)
        Me.ckbSaveSmoothedData.TabIndex = 9
        Me.ckbSaveSmoothedData.Text = "save smoothed data"
        Me.ckbSaveSmoothedData.UseVisualStyleBackColor = True
        '
        'txtDerivedDataColumnName
        '
        Me.txtDerivedDataColumnName.Enabled = False
        Me.txtDerivedDataColumnName.Location = New System.Drawing.Point(6, 146)
        Me.txtDerivedDataColumnName.Name = "txtDerivedDataColumnName"
        Me.txtDerivedDataColumnName.Size = New System.Drawing.Size(167, 20)
        Me.txtDerivedDataColumnName.TabIndex = 10
        Me.txtDerivedDataColumnName.Text = "Derivative of Data"
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(444, 653)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(176, 33)
        Me.btnCloseWindow.TabIndex = 8
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'dsDataSmoothing
        '
        Me.dsDataSmoothing.Location = New System.Drawing.Point(427, 65)
        Me.dsDataSmoothing.Name = "dsDataSmoothing"
        Me.dsDataSmoothing.SelectedSmoothingMethod = SpectroscopyManager.cNumericalMethods.SmoothingMethod.SavitzkyGolay
        Me.dsDataSmoothing.Size = New System.Drawing.Size(239, 284)
        Me.dsDataSmoothing.SmoothingParameter = 5
        Me.dsDataSmoothing.TabIndex = 7
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(11, 13)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(553, 39)
        Me.lblDescription.TabIndex = 11
        Me.lblDescription.Text = resources.GetString("lblDescription.Text")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(570, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(545, 39)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = resources.GetString("Label2.Text")
        '
        'panControls
        '
        Me.panControls.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panControls.Controls.Add(Me.gbRegaugeRange)
        Me.panControls.Controls.Add(Me.btnRegauge)
        Me.panControls.Controls.Add(Me.gbFitResults)
        Me.panControls.Controls.Add(Me.Label3)
        Me.panControls.Controls.Add(Me.PictureBox1)
        Me.panControls.Controls.Add(Me.gbCurrent)
        Me.panControls.Controls.Add(Me.Label2)
        Me.panControls.Controls.Add(Me.lblDescription)
        Me.panControls.Controls.Add(Me.dsDataSmoothing)
        Me.panControls.Controls.Add(Me.GroupBox4)
        Me.panControls.Controls.Add(Me.btnCloseWindow)
        Me.panControls.Controls.Add(Me.dbRegaugedResult)
        Me.panControls.Controls.Add(Me.gbdIdV)
        Me.panControls.Controls.Add(Me.PictureBox4)
        Me.panControls.Controls.Add(Me.PictureBox5)
        Me.panControls.Controls.Add(Me.PictureBox6)
        Me.panControls.Controls.Add(Me.PictureBox3)
        Me.panControls.Controls.Add(Me.PictureBox2)
        Me.panControls.Location = New System.Drawing.Point(3, 3)
        Me.panControls.Name = "panControls"
        Me.panControls.Size = New System.Drawing.Size(1202, 788)
        Me.panControls.TabIndex = 10
        '
        'gbRegaugeRange
        '
        Me.gbRegaugeRange.Checked = Global.SpectroscopyManager.My.MySettings.Default.LastRenormalization_UseBoundaries
        Me.gbRegaugeRange.Controls.Add(Me.Label8)
        Me.gbRegaugeRange.Controls.Add(Me.Label7)
        Me.gbRegaugeRange.Controls.Add(Me.txtRegaugeRange_xMax)
        Me.gbRegaugeRange.Controls.Add(Me.Label9)
        Me.gbRegaugeRange.Controls.Add(Me.Label6)
        Me.gbRegaugeRange.Controls.Add(Me.txtRegaugeRange_xMin)
        Me.gbRegaugeRange.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.SpectroscopyManager.My.MySettings.Default, "LastRenormalization_UseBoundaries", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.gbRegaugeRange.Location = New System.Drawing.Point(11, 359)
        Me.gbRegaugeRange.Name = "gbRegaugeRange"
        Me.gbRegaugeRange.Size = New System.Drawing.Size(383, 97)
        Me.gbRegaugeRange.TabIndex = 17
        Me.gbRegaugeRange.TabStop = False
        Me.gbRegaugeRange.Text = "select x-range to consider for regauging (monotonicity required!)"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(198, 52)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(40, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "max. x:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(23, 51)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "min. x:"
        '
        'txtRegaugeRange_xMax
        '
        Me.txtRegaugeRange_xMax.BackColor = System.Drawing.Color.White
        Me.txtRegaugeRange_xMax.ForeColor = System.Drawing.Color.Black
        Me.txtRegaugeRange_xMax.FormatDecimalPlaces = 6
        Me.txtRegaugeRange_xMax.Location = New System.Drawing.Point(241, 48)
        Me.txtRegaugeRange_xMax.Name = "txtRegaugeRange_xMax"
        Me.txtRegaugeRange_xMax.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtRegaugeRange_xMax.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtRegaugeRange_xMax.Size = New System.Drawing.Size(100, 20)
        Me.txtRegaugeRange_xMax.TabIndex = 0
        Me.txtRegaugeRange_xMax.Text = "0.000000"
        Me.txtRegaugeRange_xMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRegaugeRange_xMax.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(16, 74)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(337, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "!!! watch out: if set to an inappropriate range the result will be wrong !!!"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(13, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(336, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "the regauging procedure will just consider the selected range for fitting"
        '
        'txtRegaugeRange_xMin
        '
        Me.txtRegaugeRange_xMin.BackColor = System.Drawing.Color.White
        Me.txtRegaugeRange_xMin.ForeColor = System.Drawing.Color.Black
        Me.txtRegaugeRange_xMin.FormatDecimalPlaces = 6
        Me.txtRegaugeRange_xMin.Location = New System.Drawing.Point(66, 48)
        Me.txtRegaugeRange_xMin.Name = "txtRegaugeRange_xMin"
        Me.txtRegaugeRange_xMin.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtRegaugeRange_xMin.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtRegaugeRange_xMin.Size = New System.Drawing.Size(100, 20)
        Me.txtRegaugeRange_xMin.TabIndex = 0
        Me.txtRegaugeRange_xMin.Text = "0.000000"
        Me.txtRegaugeRange_xMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRegaugeRange_xMin.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'btnRegauge
        '
        Me.btnRegauge.Image = Global.SpectroscopyManager.My.Resources.Resources.download
        Me.btnRegauge.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnRegauge.Location = New System.Drawing.Point(759, 240)
        Me.btnRegauge.Name = "btnRegauge"
        Me.btnRegauge.Size = New System.Drawing.Size(105, 120)
        Me.btnRegauge.TabIndex = 16
        Me.btnRegauge.Text = "go"
        Me.btnRegauge.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnRegauge.UseVisualStyleBackColor = True
        '
        'gbFitResults
        '
        Me.gbFitResults.Controls.Add(Me.lblNotFittedWarning)
        Me.gbFitResults.Controls.Add(Me.Label5)
        Me.gbFitResults.Controls.Add(Me.Label4)
        Me.gbFitResults.Controls.Add(Me.txtFittedParameter_m)
        Me.gbFitResults.Controls.Add(Me.txtFittedParameter_y0)
        Me.gbFitResults.Location = New System.Drawing.Point(759, 366)
        Me.gbFitResults.Name = "gbFitResults"
        Me.gbFitResults.Size = New System.Drawing.Size(335, 58)
        Me.gbFitResults.TabIndex = 15
        Me.gbFitResults.TabStop = False
        Me.gbFitResults.Text = "fitted parameters"
        '
        'lblNotFittedWarning
        '
        Me.lblNotFittedWarning.AutoSize = True
        Me.lblNotFittedWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotFittedWarning.ForeColor = System.Drawing.Color.Red
        Me.lblNotFittedWarning.Location = New System.Drawing.Point(129, 41)
        Me.lblNotFittedWarning.Name = "lblNotFittedWarning"
        Me.lblNotFittedWarning.Size = New System.Drawing.Size(90, 13)
        Me.lblNotFittedWarning.TabIndex = 2
        Me.lblNotFittedWarning.Text = "!!! not fitted !!!"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(167, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(27, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "m = "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "y0 = "
        '
        'txtFittedParameter_m
        '
        Me.txtFittedParameter_m.BackColor = System.Drawing.Color.White
        Me.txtFittedParameter_m.ForeColor = System.Drawing.Color.Black
        Me.txtFittedParameter_m.FormatDecimalPlaces = 6
        Me.txtFittedParameter_m.Location = New System.Drawing.Point(196, 20)
        Me.txtFittedParameter_m.Name = "txtFittedParameter_m"
        Me.txtFittedParameter_m.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtFittedParameter_m.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtFittedParameter_m.Size = New System.Drawing.Size(100, 20)
        Me.txtFittedParameter_m.TabIndex = 0
        Me.txtFittedParameter_m.Text = "0.000000"
        Me.txtFittedParameter_m.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFittedParameter_m.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtFittedParameter_y0
        '
        Me.txtFittedParameter_y0.BackColor = System.Drawing.Color.White
        Me.txtFittedParameter_y0.ForeColor = System.Drawing.Color.Black
        Me.txtFittedParameter_y0.FormatDecimalPlaces = 6
        Me.txtFittedParameter_y0.Location = New System.Drawing.Point(51, 20)
        Me.txtFittedParameter_y0.Name = "txtFittedParameter_y0"
        Me.txtFittedParameter_y0.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtFittedParameter_y0.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtFittedParameter_y0.Size = New System.Drawing.Size(100, 20)
        Me.txtFittedParameter_y0.TabIndex = 0
        Me.txtFittedParameter_y0.Text = "0.000000"
        Me.txtFittedParameter_y0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFittedParameter_y0.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(758, 144)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 26)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "fit right to left" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "g(x) = y0 + m * f(x)"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.SpectroscopyManager.My.Resources.Resources.derivative
        Me.PictureBox1.Location = New System.Drawing.Point(701, 183)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(44, 47)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.SpectroscopyManager.My.Resources.Resources.right_25
        Me.PictureBox4.Location = New System.Drawing.Point(401, 194)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(25, 22)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 13
        Me.PictureBox4.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = Global.SpectroscopyManager.My.Resources.Resources.regauge
        Me.PictureBox5.Location = New System.Drawing.Point(785, 175)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(59, 60)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox5.TabIndex = 13
        Me.PictureBox5.TabStop = False
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = Global.SpectroscopyManager.My.Resources.Resources.left_25
        Me.PictureBox6.Location = New System.Drawing.Point(848, 194)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(25, 22)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox6.TabIndex = 13
        Me.PictureBox6.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.SpectroscopyManager.My.Resources.Resources.right_25
        Me.PictureBox3.Location = New System.Drawing.Point(754, 194)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(25, 22)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 13
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.SpectroscopyManager.My.Resources.Resources.right_25
        Me.PictureBox2.Location = New System.Drawing.Point(670, 194)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(25, 22)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 13
        Me.PictureBox2.TabStop = False
        '
        'wDataRenormalization2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1208, 794)
        Me.Controls.Add(Me.panControls)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataRenormalization2"
        Me.Text = "Gauge Data to Numeric Derivative - "
        Me.Controls.SetChildIndex(Me.panControls, 0)
        Me.gbCurrent.ResumeLayout(False)
        Me.gbdIdV.ResumeLayout(False)
        Me.dbRegaugedResult.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.panControls.ResumeLayout(False)
        Me.panControls.PerformLayout()
        Me.gbRegaugeRange.ResumeLayout(False)
        Me.gbRegaugeRange.PerformLayout()
        Me.gbFitResults.ResumeLayout(False)
        Me.gbFitResults.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbCurrent As System.Windows.Forms.GroupBox
    Friend WithEvents pbSourceData As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents gbdIdV As System.Windows.Forms.GroupBox
    Friend WithEvents pbTargetData As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents dsDataSmoothing As SpectroscopyManager.mDataSmoothing
    Friend WithEvents dbRegaugedResult As System.Windows.Forms.GroupBox
    Friend WithEvents pbOutput As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumns As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents txtRenormalizedDataColumnName As System.Windows.Forms.TextBox
    Friend WithEvents txtSmoothedDataColumnName As System.Windows.Forms.TextBox
    Friend WithEvents ckbSaveDerivedData As System.Windows.Forms.CheckBox
    Friend WithEvents ckbSaveSmoothedData As System.Windows.Forms.CheckBox
    Friend WithEvents txtDerivedDataColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents panControls As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents gbFitResults As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtFittedParameter_m As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtFittedParameter_y0 As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents btnRegauge As System.Windows.Forms.Button
    Friend WithEvents gbRegaugeRange As SpectroscopyManager.GroupBoxCheckable
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtRegaugeRange_xMax As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtRegaugeRange_xMin As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblNotFittedWarning As System.Windows.Forms.Label
End Class
