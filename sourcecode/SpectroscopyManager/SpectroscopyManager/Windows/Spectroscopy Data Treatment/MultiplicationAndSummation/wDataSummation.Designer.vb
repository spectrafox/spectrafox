<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataSummation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataSummation))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplySummation = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBeforeSummation = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbAfterSummation = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csColumnToSum = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.txtSummationFactor = New SpectroscopyManager.NumericTextbox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.tcSummationMethod = New System.Windows.Forms.TabControl()
        Me.tpFactorSummation = New System.Windows.Forms.TabPage()
        Me.tpOtherColumnSummation = New System.Windows.Forms.TabPage()
        Me.rbSubstract = New System.Windows.Forms.RadioButton()
        Me.rbAdd = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.csColumnToSumWith = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tcSummationMethod.SuspendLayout()
        Me.tpFactorSummation.SuspendLayout()
        Me.tpOtherColumnSummation.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(656, 177)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Save as:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(711, 174)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(232, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "Data Sum"
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
        Me.btnSaveColumn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveColumn.Enabled = False
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(843, 211)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 10
        Me.btnSaveColumn.Text = "save column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplySummation
        '
        Me.btnApplySummation.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplySummation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplySummation.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplySummation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplySummation.Location = New System.Drawing.Point(658, 212)
        Me.btnApplySummation.Name = "btnApplySummation"
        Me.btnApplySummation.Size = New System.Drawing.Size(179, 64)
        Me.btnApplySummation.TabIndex = 9
        Me.btnApplySummation.Text = "apply summation"
        Me.btnApplySummation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplySummation.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(658, 709)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(291, 29)
        Me.btnCloseWindow.TabIndex = 11
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pbBeforeSummation)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(649, 369)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "before summation"
        '
        'pbBeforeSummation
        '
        Me.pbBeforeSummation.AllowAdjustingXColumn = True
        Me.pbBeforeSummation.AllowAdjustingYColumn = True
        Me.pbBeforeSummation.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBeforeSummation.CallbackDataPointSelected = Nothing
        Me.pbBeforeSummation.CallbackXRangeSelected = Nothing
        Me.pbBeforeSummation.CallbackXValueSelected = Nothing
        Me.pbBeforeSummation.CallbackXYRangeSelected = Nothing
        Me.pbBeforeSummation.CallbackXYValueSelected = Nothing
        Me.pbBeforeSummation.CallbackYRangeSelected = Nothing
        Me.pbBeforeSummation.CallbackYValueSelected = Nothing
        Me.pbBeforeSummation.ClearPointSelectionModeAfterSelection = False
        Me.pbBeforeSummation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBeforeSummation.Location = New System.Drawing.Point(3, 16)
        Me.pbBeforeSummation.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBeforeSummation.Name = "pbBeforeSummation"
        Me.pbBeforeSummation.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBeforeSummation.ShowColumnSelectors = True
        Me.pbBeforeSummation.Size = New System.Drawing.Size(643, 350)
        Me.pbBeforeSummation.TabIndex = 6
        Me.pbBeforeSummation.TurnOnLastFilterSaving_Y = True
        Me.pbBeforeSummation.TurnOnLastSelectionSaving_Y = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(1, 1)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(649, 740)
        Me.SplitContainer1.SplitterDistance = 369
        Me.SplitContainer1.TabIndex = 13
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbAfterSummation)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(649, 367)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "after summation"
        '
        'pbAfterSummation
        '
        Me.pbAfterSummation.AllowAdjustingXColumn = True
        Me.pbAfterSummation.AllowAdjustingYColumn = True
        Me.pbAfterSummation.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfterSummation.CallbackDataPointSelected = Nothing
        Me.pbAfterSummation.CallbackXRangeSelected = Nothing
        Me.pbAfterSummation.CallbackXValueSelected = Nothing
        Me.pbAfterSummation.CallbackXYRangeSelected = Nothing
        Me.pbAfterSummation.CallbackXYValueSelected = Nothing
        Me.pbAfterSummation.CallbackYRangeSelected = Nothing
        Me.pbAfterSummation.CallbackYValueSelected = Nothing
        Me.pbAfterSummation.ClearPointSelectionModeAfterSelection = False
        Me.pbAfterSummation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfterSummation.Location = New System.Drawing.Point(3, 16)
        Me.pbAfterSummation.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfterSummation.Name = "pbAfterSummation"
        Me.pbAfterSummation.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfterSummation.ShowColumnSelectors = True
        Me.pbAfterSummation.Size = New System.Drawing.Size(643, 348)
        Me.pbAfterSummation.TabIndex = 6
        Me.pbAfterSummation.TurnOnLastFilterSaving_Y = False
        Me.pbAfterSummation.TurnOnLastSelectionSaving_Y = False
        '
        'csColumnToSum
        '
        Me.csColumnToSum.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToSum.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToSum.Location = New System.Drawing.Point(65, 22)
        Me.csColumnToSum.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToSum.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToSum.Name = "csColumnToSum"
        Me.csColumnToSum.SelectedColumnName = ""
        Me.csColumnToSum.SelectedColumnNames = CType(resources.GetObject("csColumnToSum.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToSum.SelectedEntries = CType(resources.GetObject("csColumnToSum.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToSum.SelectedEntry = ""
        Me.csColumnToSum.Size = New System.Drawing.Size(211, 21)
        Me.csColumnToSum.TabIndex = 5
        Me.csColumnToSum.TurnOnLastFilterSaving = False
        Me.csColumnToSum.TurnOnLastSelectionSaving = False
        '
        'txtSummationFactor
        '
        Me.txtSummationFactor.BackColor = System.Drawing.Color.White
        Me.txtSummationFactor.ForeColor = System.Drawing.Color.Black
        Me.txtSummationFactor.FormatDecimalPlaces = 6
        Me.txtSummationFactor.Location = New System.Drawing.Point(139, 29)
        Me.txtSummationFactor.Name = "txtSummationFactor"
        Me.txtSummationFactor.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtSummationFactor.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSummationFactor.Size = New System.Drawing.Size(130, 20)
        Me.txtSummationFactor.TabIndex = 15
        Me.txtSummationFactor.Text = "0,000000E+000"
        Me.txtSummationFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSummationFactor.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "sum column with const.:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.tcSummationMethod)
        Me.GroupBox3.Controls.Add(Me.csColumnToSum)
        Me.GroupBox3.Location = New System.Drawing.Point(658, 1)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(289, 167)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "settings"
        '
        'tcSummationMethod
        '
        Me.tcSummationMethod.Controls.Add(Me.tpFactorSummation)
        Me.tcSummationMethod.Controls.Add(Me.tpOtherColumnSummation)
        Me.tcSummationMethod.Location = New System.Drawing.Point(3, 54)
        Me.tcSummationMethod.Name = "tcSummationMethod"
        Me.tcSummationMethod.SelectedIndex = 0
        Me.tcSummationMethod.Size = New System.Drawing.Size(283, 104)
        Me.tcSummationMethod.TabIndex = 16
        '
        'tpFactorSummation
        '
        Me.tpFactorSummation.Controls.Add(Me.txtSummationFactor)
        Me.tpFactorSummation.Controls.Add(Me.Label3)
        Me.tpFactorSummation.Location = New System.Drawing.Point(4, 22)
        Me.tpFactorSummation.Name = "tpFactorSummation"
        Me.tpFactorSummation.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFactorSummation.Size = New System.Drawing.Size(275, 78)
        Me.tpFactorSummation.TabIndex = 0
        Me.tpFactorSummation.Text = "const. summation"
        Me.tpFactorSummation.UseVisualStyleBackColor = True
        '
        'tpOtherColumnSummation
        '
        Me.tpOtherColumnSummation.Controls.Add(Me.rbSubstract)
        Me.tpOtherColumnSummation.Controls.Add(Me.rbAdd)
        Me.tpOtherColumnSummation.Controls.Add(Me.Label5)
        Me.tpOtherColumnSummation.Controls.Add(Me.csColumnToSumWith)
        Me.tpOtherColumnSummation.Location = New System.Drawing.Point(4, 22)
        Me.tpOtherColumnSummation.Name = "tpOtherColumnSummation"
        Me.tpOtherColumnSummation.Padding = New System.Windows.Forms.Padding(3)
        Me.tpOtherColumnSummation.Size = New System.Drawing.Size(275, 78)
        Me.tpOtherColumnSummation.TabIndex = 1
        Me.tpOtherColumnSummation.Text = "add / substract other column"
        Me.tpOtherColumnSummation.UseVisualStyleBackColor = True
        '
        'rbSubstract
        '
        Me.rbSubstract.AutoSize = True
        Me.rbSubstract.Location = New System.Drawing.Point(138, 52)
        Me.rbSubstract.Name = "rbSubstract"
        Me.rbSubstract.Size = New System.Drawing.Size(68, 17)
        Me.rbSubstract.TabIndex = 18
        Me.rbSubstract.Text = "substract"
        Me.rbSubstract.UseVisualStyleBackColor = True
        '
        'rbAdd
        '
        Me.rbAdd.AutoSize = True
        Me.rbAdd.Checked = True
        Me.rbAdd.Location = New System.Drawing.Point(69, 52)
        Me.rbAdd.Name = "rbAdd"
        Me.rbAdd.Size = New System.Drawing.Size(43, 17)
        Me.rbAdd.TabIndex = 18
        Me.rbAdd.TabStop = True
        Me.rbAdd.Text = "add"
        Me.rbAdd.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "reference column:"
        '
        'csColumnToSumWith
        '
        Me.csColumnToSumWith.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToSumWith.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToSumWith.Location = New System.Drawing.Point(30, 25)
        Me.csColumnToSumWith.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToSumWith.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToSumWith.Name = "csColumnToSumWith"
        Me.csColumnToSumWith.SelectedColumnName = ""
        Me.csColumnToSumWith.SelectedColumnNames = CType(resources.GetObject("csColumnToSumWith.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToSumWith.SelectedEntries = CType(resources.GetObject("csColumnToSumWith.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToSumWith.SelectedEntry = ""
        Me.csColumnToSumWith.Size = New System.Drawing.Size(225, 21)
        Me.csColumnToSumWith.TabIndex = 17
        Me.csColumnToSumWith.TurnOnLastFilterSaving = False
        Me.csColumnToSumWith.TurnOnLastSelectionSaving = False
        '
        'wDataSummation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(949, 744)
        Me.Controls.Add(Me.btnApplySummation)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataSummation"
        Me.Text = "Data Summation - "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.Controls.SetChildIndex(Me.btnApplySummation, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.tcSummationMethod.ResumeLayout(False)
        Me.tpFactorSummation.ResumeLayout(False)
        Me.tpFactorSummation.PerformLayout()
        Me.tpOtherColumnSummation.ResumeLayout(False)
        Me.tpOtherColumnSummation.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csColumnToSum As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbBeforeSummation As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplySummation As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbAfterSummation As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents txtSummationFactor As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents tcSummationMethod As System.Windows.Forms.TabControl
    Friend WithEvents tpFactorSummation As System.Windows.Forms.TabPage
    Friend WithEvents tpOtherColumnSummation As System.Windows.Forms.TabPage
    Friend WithEvents rbSubstract As System.Windows.Forms.RadioButton
    Friend WithEvents rbAdd As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents csColumnToSumWith As SpectroscopyManager.ucSpectroscopyColumnSelector
End Class
