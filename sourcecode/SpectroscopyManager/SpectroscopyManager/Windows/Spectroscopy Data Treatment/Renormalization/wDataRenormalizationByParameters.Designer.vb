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
        Me.gbBefore = New System.Windows.Forms.GroupBox()
        Me.pbBefore = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.gbAfter = New System.Windows.Forms.GroupBox()
        Me.pbAfter = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csSourceColumn = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.grpSettings = New System.Windows.Forms.GroupBox()
        Me.txtBiasModulation = New SpectroscopyManager.NumericTextbox()
        Me.txtLockInSensitivity = New SpectroscopyManager.NumericTextbox()
        Me.nudAmplifierGain = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.gbBefore.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.gbAfter.SuspendLayout()
        Me.grpSettings.SuspendLayout()
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
        Me.txtNewColumnName.TabIndex = 5
        Me.txtNewColumnName.Text = "re-gauged dI/dV"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "lock-in column:"
        '
        'btnSaveColumn
        '
        Me.btnSaveColumn.Enabled = False
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(193, 258)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 11
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
        Me.btnApplyRegauging.TabIndex = 10
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
        Me.btnCloseWindow.TabIndex = 12
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'gbBefore
        '
        Me.gbBefore.Controls.Add(Me.pbBefore)
        Me.gbBefore.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbBefore.Location = New System.Drawing.Point(0, 0)
        Me.gbBefore.Name = "gbBefore"
        Me.gbBefore.Size = New System.Drawing.Size(624, 351)
        Me.gbBefore.TabIndex = 12
        Me.gbBefore.TabStop = False
        Me.gbBefore.Text = "before re-gauging"
        '
        'pbBefore
        '
        Me.pbBefore.AllowAdjustingXColumn = True
        Me.pbBefore.AllowAdjustingYColumn = True
        Me.pbBefore.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBefore.CallbackDataPointSelected = Nothing
        Me.pbBefore.CallbackXRangeSelected = Nothing
        Me.pbBefore.CallbackXValueSelected = Nothing
        Me.pbBefore.CallbackXYRangeSelected = Nothing
        Me.pbBefore.CallbackXYValueSelected = Nothing
        Me.pbBefore.CallbackYRangeSelected = Nothing
        Me.pbBefore.CallbackYValueSelected = Nothing
        Me.pbBefore.ClearPointSelectionModeAfterSelection = False
        Me.pbBefore.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBefore.Location = New System.Drawing.Point(3, 16)
        Me.pbBefore.MultipleSpectraStackOffset = 0R
        Me.pbBefore.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBefore.Name = "pbBefore"
        Me.pbBefore.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBefore.ShowColumnSelectors = True
        Me.pbBefore.Size = New System.Drawing.Size(618, 332)
        Me.pbBefore.TabIndex = 20
        Me.pbBefore.TurnOnLastFilterSaving_Y = True
        Me.pbBefore.TurnOnLastSelectionSaving_Y = False
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.gbBefore)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.gbAfter)
        Me.SplitContainer1.Size = New System.Drawing.Size(624, 704)
        Me.SplitContainer1.SplitterDistance = 351
        Me.SplitContainer1.TabIndex = 13
        '
        'gbAfter
        '
        Me.gbAfter.Controls.Add(Me.pbAfter)
        Me.gbAfter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAfter.Location = New System.Drawing.Point(0, 0)
        Me.gbAfter.Name = "gbAfter"
        Me.gbAfter.Size = New System.Drawing.Size(624, 349)
        Me.gbAfter.TabIndex = 13
        Me.gbAfter.TabStop = False
        Me.gbAfter.Text = "after re-gauging:"
        '
        'pbAfter
        '
        Me.pbAfter.AllowAdjustingXColumn = True
        Me.pbAfter.AllowAdjustingYColumn = True
        Me.pbAfter.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfter.CallbackDataPointSelected = Nothing
        Me.pbAfter.CallbackXRangeSelected = Nothing
        Me.pbAfter.CallbackXValueSelected = Nothing
        Me.pbAfter.CallbackXYRangeSelected = Nothing
        Me.pbAfter.CallbackXYValueSelected = Nothing
        Me.pbAfter.CallbackYRangeSelected = Nothing
        Me.pbAfter.CallbackYValueSelected = Nothing
        Me.pbAfter.ClearPointSelectionModeAfterSelection = False
        Me.pbAfter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfter.Location = New System.Drawing.Point(3, 16)
        Me.pbAfter.MultipleSpectraStackOffset = 0R
        Me.pbAfter.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfter.Name = "pbAfter"
        Me.pbAfter.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfter.ShowColumnSelectors = True
        Me.pbAfter.Size = New System.Drawing.Size(618, 330)
        Me.pbAfter.TabIndex = 30
        Me.pbAfter.TurnOnLastFilterSaving_Y = False
        Me.pbAfter.TurnOnLastSelectionSaving_Y = False
        '
        'csSourceColumn
        '
        Me.csSourceColumn.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csSourceColumn.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csSourceColumn.Location = New System.Drawing.Point(90, 22)
        Me.csSourceColumn.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csSourceColumn.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csSourceColumn.Name = "csSourceColumn"
        Me.csSourceColumn.SelectedColumnName = ""
        Me.csSourceColumn.SelectedColumnNames = CType(resources.GetObject("csSourceColumn.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csSourceColumn.SelectedEntries = CType(resources.GetObject("csSourceColumn.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csSourceColumn.SelectedEntry = ""
        Me.csSourceColumn.Size = New System.Drawing.Size(190, 21)
        Me.csSourceColumn.TabIndex = 1
        Me.csSourceColumn.TurnOnLastFilterSaving = False
        Me.csSourceColumn.TurnOnLastSelectionSaving = False
        '
        'grpSettings
        '
        Me.grpSettings.Controls.Add(Me.txtBiasModulation)
        Me.grpSettings.Controls.Add(Me.txtLockInSensitivity)
        Me.grpSettings.Controls.Add(Me.nudAmplifierGain)
        Me.grpSettings.Controls.Add(Me.Label5)
        Me.grpSettings.Controls.Add(Me.Label4)
        Me.grpSettings.Controls.Add(Me.Label3)
        Me.grpSettings.Controls.Add(Me.Label2)
        Me.grpSettings.Controls.Add(Me.csSourceColumn)
        Me.grpSettings.Location = New System.Drawing.Point(8, 65)
        Me.grpSettings.Name = "grpSettings"
        Me.grpSettings.Size = New System.Drawing.Size(289, 140)
        Me.grpSettings.TabIndex = 16
        Me.grpSettings.TabStop = False
        Me.grpSettings.Text = "settings"
        '
        'txtBiasModulation
        '
        Me.txtBiasModulation.AllowZero = True
        Me.txtBiasModulation.BackColor = System.Drawing.Color.White
        Me.txtBiasModulation.ForeColor = System.Drawing.Color.Black
        Me.txtBiasModulation.FormatDecimalPlaces = 6
        Me.txtBiasModulation.Location = New System.Drawing.Point(176, 53)
        Me.txtBiasModulation.Name = "txtBiasModulation"
        Me.txtBiasModulation.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtBiasModulation.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtBiasModulation.Size = New System.Drawing.Size(87, 20)
        Me.txtBiasModulation.TabIndex = 2
        Me.txtBiasModulation.Text = "0.000000"
        Me.txtBiasModulation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtBiasModulation.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtLockInSensitivity
        '
        Me.txtLockInSensitivity.AllowZero = True
        Me.txtLockInSensitivity.BackColor = System.Drawing.Color.White
        Me.txtLockInSensitivity.ForeColor = System.Drawing.Color.Black
        Me.txtLockInSensitivity.FormatDecimalPlaces = 6
        Me.txtLockInSensitivity.Location = New System.Drawing.Point(176, 79)
        Me.txtLockInSensitivity.Name = "txtLockInSensitivity"
        Me.txtLockInSensitivity.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtLockInSensitivity.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtLockInSensitivity.Size = New System.Drawing.Size(87, 20)
        Me.txtLockInSensitivity.TabIndex = 3
        Me.txtLockInSensitivity.Text = "0.000000"
        Me.txtLockInSensitivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLockInSensitivity.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'nudAmplifierGain
        '
        Me.nudAmplifierGain.Location = New System.Drawing.Point(176, 106)
        Me.nudAmplifierGain.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.nudAmplifierGain.Name = "nudAmplifierGain"
        Me.nudAmplifierGain.Size = New System.Drawing.Size(48, 20)
        Me.nudAmplifierGain.TabIndex = 4
        Me.nudAmplifierGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudAmplifierGain.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        Me.nudAmplifierGain.Value = New Decimal(New Integer() {9, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(27, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(142, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "preamplifier gain (pow of 10):"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(80, 82)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "lock-in sensitivity:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(41, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "lock-in modulation (RMS):"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(10, 11)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(543, 39)
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
        Me.Controls.Add(Me.grpSettings)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataRenormalizationByParameters"
        Me.Text = "re-gauge lockin data by known parameters - "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.Controls.SetChildIndex(Me.grpSettings, 0)
        Me.Controls.SetChildIndex(Me.btnApplyRegauging, 0)
        Me.Controls.SetChildIndex(Me.lblDescription, 0)
        Me.gbBefore.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.gbAfter.ResumeLayout(False)
        Me.grpSettings.ResumeLayout(False)
        Me.grpSettings.PerformLayout()
        CType(Me.nudAmplifierGain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csSourceColumn As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbBefore As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplyRegauging As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents gbBefore As System.Windows.Forms.GroupBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents gbAfter As System.Windows.Forms.GroupBox
    Friend WithEvents pbAfter As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents grpSettings As System.Windows.Forms.GroupBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtBiasModulation As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtLockInSensitivity As SpectroscopyManager.NumericTextbox
    Friend WithEvents nudAmplifierGain As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
