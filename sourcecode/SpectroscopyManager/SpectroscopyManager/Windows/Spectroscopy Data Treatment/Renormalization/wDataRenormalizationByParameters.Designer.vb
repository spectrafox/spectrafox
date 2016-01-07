<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataRenormalizationByParameters
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataRenormalizationByParameters))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplyRegauging = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBeforeRegauging = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbAfterRegauging = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csSourceColumn = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtBiasModulation = New SpectroscopyManager.NumericTextbox()
        Me.txtLockInSensitivity = New SpectroscopyManager.NumericTextbox()
        Me.nudAmplifierGain = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.nudAmplifierGain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 224)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Save as column-name:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Location = New System.Drawing.Point(127, 221)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "Re-gauged Data"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Column:"
        '
        'btnSaveColumn
        '
        Me.btnSaveColumn.Enabled = False
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(193, 258)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 10
        Me.btnSaveColumn.Text = "save column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplyRegauging
        '
        Me.btnApplyRegauging.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplyRegauging.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyRegauging.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyRegauging.Location = New System.Drawing.Point(8, 259)
        Me.btnApplyRegauging.Name = "btnApplyRegauging"
        Me.btnApplyRegauging.Size = New System.Drawing.Size(179, 64)
        Me.btnApplyRegauging.TabIndex = 9
        Me.btnApplyRegauging.Text = "re-gauge data"
        Me.btnApplyRegauging.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyRegauging.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(8, 340)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(291, 29)
        Me.btnCloseWindow.TabIndex = 11
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pbBeforeRegauging)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(624, 351)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "before re-gauging"
        '
        'pbBeforeRegauging
        '
        Me.pbBeforeRegauging.AllowAdjustingXColumn = True
        Me.pbBeforeRegauging.AllowAdjustingYColumn = True
        Me.pbBeforeRegauging.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBeforeRegauging.CallbackXRangeSelected = Nothing
        Me.pbBeforeRegauging.CallbackXValueSelected = Nothing
        Me.pbBeforeRegauging.CallbackXYRangeSelected = Nothing
        Me.pbBeforeRegauging.CallbackYRangeSelected = Nothing
        Me.pbBeforeRegauging.CallbackYValueSelected = Nothing
        Me.pbBeforeRegauging.ClearPointSelectionModeAfterSelection = False
        Me.pbBeforeRegauging.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBeforeRegauging.Location = New System.Drawing.Point(3, 16)
        Me.pbBeforeRegauging.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBeforeRegauging.Name = "pbBeforeRegauging"
        Me.pbBeforeRegauging.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBeforeRegauging.ShowColumnSelectors = True
        Me.pbBeforeRegauging.Size = New System.Drawing.Size(618, 332)
        Me.pbBeforeRegauging.TabIndex = 6
        Me.pbBeforeRegauging.TurnOnLastFilterSaving_Y = True
        Me.pbBeforeRegauging.TurnOnLastSelectionSaving_Y = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(305, 65)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox2)
        Me.SplitContainer1.Size = New System.Drawing.Size(624, 704)
        Me.SplitContainer1.SplitterDistance = 351
        Me.SplitContainer1.TabIndex = 13
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbAfterRegauging)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(624, 349)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "after re-gauging:"
        '
        'pbAfterRegauging
        '
        Me.pbAfterRegauging.AllowAdjustingXColumn = True
        Me.pbAfterRegauging.AllowAdjustingYColumn = True
        Me.pbAfterRegauging.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfterRegauging.CallbackXRangeSelected = Nothing
        Me.pbAfterRegauging.CallbackXValueSelected = Nothing
        Me.pbAfterRegauging.CallbackXYRangeSelected = Nothing
        Me.pbAfterRegauging.CallbackYRangeSelected = Nothing
        Me.pbAfterRegauging.CallbackYValueSelected = Nothing
        Me.pbAfterRegauging.ClearPointSelectionModeAfterSelection = False
        Me.pbAfterRegauging.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfterRegauging.Location = New System.Drawing.Point(3, 16)
        Me.pbAfterRegauging.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfterRegauging.Name = "pbAfterRegauging"
        Me.pbAfterRegauging.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfterRegauging.ShowColumnSelectors = True
        Me.pbAfterRegauging.Size = New System.Drawing.Size(618, 330)
        Me.pbAfterRegauging.TabIndex = 6
        Me.pbAfterRegauging.TurnOnLastFilterSaving_Y = False
        Me.pbAfterRegauging.TurnOnLastSelectionSaving_Y = False
        '
        'csSourceColumn
        '
        Me.csSourceColumn.AppereanceType = SpectroscopyManager.ucSpectroscopyColumnSelector.SelectorType.Combobox
        Me.csSourceColumn.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csSourceColumn.Location = New System.Drawing.Point(65, 22)
        Me.csSourceColumn.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csSourceColumn.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csSourceColumn.Name = "csSourceColumn"
        Me.csSourceColumn.SelectedColumnName = ""
        Me.csSourceColumn.SelectedColumnNames = CType(resources.GetObject("csSourceColumn.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csSourceColumn.Size = New System.Drawing.Size(211, 21)
        Me.csSourceColumn.TabIndex = 5
        Me.csSourceColumn.TurnOnLastFilterSaving = False
        Me.csSourceColumn.TurnOnLastSelectionSaving = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtBiasModulation)
        Me.GroupBox3.Controls.Add(Me.txtLockInSensitivity)
        Me.GroupBox3.Controls.Add(Me.nudAmplifierGain)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.csSourceColumn)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 65)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(289, 140)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "settings"
        '
        'txtBiasModulation
        '
        Me.txtBiasModulation.BackColor = System.Drawing.Color.White
        Me.txtBiasModulation.ForeColor = System.Drawing.Color.Black
        Me.txtBiasModulation.FormatDecimalPlaces = 6
        Me.txtBiasModulation.Location = New System.Drawing.Point(189, 53)
        Me.txtBiasModulation.Name = "txtBiasModulation"
        Me.txtBiasModulation.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtBiasModulation.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtBiasModulation.Size = New System.Drawing.Size(87, 20)
        Me.txtBiasModulation.TabIndex = 7
        Me.txtBiasModulation.Text = "0.000000"
        Me.txtBiasModulation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtBiasModulation.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtLockInSensitivity
        '
        Me.txtLockInSensitivity.BackColor = System.Drawing.Color.White
        Me.txtLockInSensitivity.ForeColor = System.Drawing.Color.Black
        Me.txtLockInSensitivity.FormatDecimalPlaces = 6
        Me.txtLockInSensitivity.Location = New System.Drawing.Point(189, 79)
        Me.txtLockInSensitivity.Name = "txtLockInSensitivity"
        Me.txtLockInSensitivity.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtLockInSensitivity.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtLockInSensitivity.Size = New System.Drawing.Size(87, 20)
        Me.txtLockInSensitivity.TabIndex = 7
        Me.txtLockInSensitivity.Text = "0.000000"
        Me.txtLockInSensitivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLockInSensitivity.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'nudAmplifierGain
        '
        Me.nudAmplifierGain.Location = New System.Drawing.Point(189, 106)
        Me.nudAmplifierGain.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.nudAmplifierGain.Name = "nudAmplifierGain"
        Me.nudAmplifierGain.Size = New System.Drawing.Size(48, 20)
        Me.nudAmplifierGain.TabIndex = 6
        Me.nudAmplifierGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudAmplifierGain.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        Me.nudAmplifierGain.Value = New Decimal(New Integer() {9, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(75, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(111, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Current Amplifier Gain:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(90, 82)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Lock-in Sensitivity:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(172, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Lock-in Bias-Modulation Amplitude:"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(10, 11)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(577, 39)
        Me.lblDescription.TabIndex = 18
        Me.lblDescription.Text = resources.GetString("lblDescription.Text")
        '
        'wDataRenormalizationByParameters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(933, 775)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.btnApplyRegauging)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataRenormalizationByParameters"
        Me.Text = "Re-Gauge Data by Data Acuisition Parameters - "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.Controls.SetChildIndex(Me.btnApplyRegauging, 0)
        Me.Controls.SetChildIndex(Me.lblDescription, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.nudAmplifierGain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csSourceColumn As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbBeforeRegauging As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplyRegauging As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbAfterRegauging As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtBiasModulation As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtLockInSensitivity As SpectroscopyManager.NumericTextbox
    Friend WithEvents nudAmplifierGain As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
