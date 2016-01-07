<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataMultiplication
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataMultiplication))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplyMultiplication = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBeforeMultiplication = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbAfterMultiplication = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csColumnToMultiply = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.txtMuliplicationFactor = New SpectroscopyManager.NumericTextbox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.tcMultiplicationMethod = New System.Windows.Forms.TabControl()
        Me.tpFactorMultiplication = New System.Windows.Forms.TabPage()
        Me.tpOtherColumnMultiplication = New System.Windows.Forms.TabPage()
        Me.rbDivide = New System.Windows.Forms.RadioButton()
        Me.rbMultiply = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.csColumnToMultiplyWith = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tcMultiplicationMethod.SuspendLayout()
        Me.tpFactorMultiplication.SuspendLayout()
        Me.tpOtherColumnMultiplication.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(656, 179)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Save as ColumnName:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(777, 176)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "Multiplied / Divided Data"
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
        Me.btnSaveColumn.Location = New System.Drawing.Point(843, 213)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 10
        Me.btnSaveColumn.Text = "Save Column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplyMultiplication
        '
        Me.btnApplyMultiplication.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplyMultiplication.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplyMultiplication.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyMultiplication.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyMultiplication.Location = New System.Drawing.Point(658, 214)
        Me.btnApplyMultiplication.Name = "btnApplyMultiplication"
        Me.btnApplyMultiplication.Size = New System.Drawing.Size(179, 64)
        Me.btnApplyMultiplication.TabIndex = 9
        Me.btnApplyMultiplication.Text = "Apply multiplication"
        Me.btnApplyMultiplication.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyMultiplication.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(652, 712)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(291, 29)
        Me.btnCloseWindow.TabIndex = 11
        Me.btnCloseWindow.Text = "Close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pbBeforeMultiplication)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(649, 368)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "before multiplication"
        '
        'pbBeforeMultiplication
        '
        Me.pbBeforeMultiplication.AllowAdjustingXColumn = True
        Me.pbBeforeMultiplication.AllowAdjustingYColumn = True
        Me.pbBeforeMultiplication.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBeforeMultiplication.CallbackDataPointSelected = Nothing
        Me.pbBeforeMultiplication.CallbackXRangeSelected = Nothing
        Me.pbBeforeMultiplication.CallbackXValueSelected = Nothing
        Me.pbBeforeMultiplication.CallbackXYRangeSelected = Nothing
        Me.pbBeforeMultiplication.CallbackXYValueSelected = Nothing
        Me.pbBeforeMultiplication.CallbackYRangeSelected = Nothing
        Me.pbBeforeMultiplication.CallbackYValueSelected = Nothing
        Me.pbBeforeMultiplication.ClearPointSelectionModeAfterSelection = False
        Me.pbBeforeMultiplication.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBeforeMultiplication.Location = New System.Drawing.Point(3, 16)
        Me.pbBeforeMultiplication.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBeforeMultiplication.Name = "pbBeforeMultiplication"
        Me.pbBeforeMultiplication.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBeforeMultiplication.ShowColumnSelectors = True
        Me.pbBeforeMultiplication.Size = New System.Drawing.Size(643, 349)
        Me.pbBeforeMultiplication.TabIndex = 6
        Me.pbBeforeMultiplication.TurnOnLastFilterSaving_Y = True
        Me.pbBeforeMultiplication.TurnOnLastSelectionSaving_Y = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(1, 3)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(649, 738)
        Me.SplitContainer1.SplitterDistance = 368
        Me.SplitContainer1.TabIndex = 13
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbAfterMultiplication)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(649, 366)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "after multiplication:"
        '
        'pbAfterMultiplication
        '
        Me.pbAfterMultiplication.AllowAdjustingXColumn = True
        Me.pbAfterMultiplication.AllowAdjustingYColumn = True
        Me.pbAfterMultiplication.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfterMultiplication.CallbackDataPointSelected = Nothing
        Me.pbAfterMultiplication.CallbackXRangeSelected = Nothing
        Me.pbAfterMultiplication.CallbackXValueSelected = Nothing
        Me.pbAfterMultiplication.CallbackXYRangeSelected = Nothing
        Me.pbAfterMultiplication.CallbackXYValueSelected = Nothing
        Me.pbAfterMultiplication.CallbackYRangeSelected = Nothing
        Me.pbAfterMultiplication.CallbackYValueSelected = Nothing
        Me.pbAfterMultiplication.ClearPointSelectionModeAfterSelection = False
        Me.pbAfterMultiplication.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfterMultiplication.Location = New System.Drawing.Point(3, 16)
        Me.pbAfterMultiplication.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfterMultiplication.Name = "pbAfterMultiplication"
        Me.pbAfterMultiplication.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfterMultiplication.ShowColumnSelectors = True
        Me.pbAfterMultiplication.Size = New System.Drawing.Size(643, 347)
        Me.pbAfterMultiplication.TabIndex = 6
        Me.pbAfterMultiplication.TurnOnLastFilterSaving_Y = False
        Me.pbAfterMultiplication.TurnOnLastSelectionSaving_Y = False
        '
        'csColumnToMultiply
        '
        Me.csColumnToMultiply.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToMultiply.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToMultiply.Location = New System.Drawing.Point(65, 22)
        Me.csColumnToMultiply.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToMultiply.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToMultiply.Name = "csColumnToMultiply"
        Me.csColumnToMultiply.SelectedColumnName = ""
        Me.csColumnToMultiply.SelectedColumnNames = CType(resources.GetObject("csColumnToMultiply.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToMultiply.SelectedEntries = CType(resources.GetObject("csColumnToMultiply.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToMultiply.SelectedEntry = ""
        Me.csColumnToMultiply.Size = New System.Drawing.Size(211, 21)
        Me.csColumnToMultiply.TabIndex = 5
        Me.csColumnToMultiply.TurnOnLastFilterSaving = False
        Me.csColumnToMultiply.TurnOnLastSelectionSaving = False
        '
        'txtMuliplicationFactor
        '
        Me.txtMuliplicationFactor.BackColor = System.Drawing.Color.White
        Me.txtMuliplicationFactor.ForeColor = System.Drawing.Color.Black
        Me.txtMuliplicationFactor.FormatDecimalPlaces = 6
        Me.txtMuliplicationFactor.Location = New System.Drawing.Point(139, 29)
        Me.txtMuliplicationFactor.Name = "txtMuliplicationFactor"
        Me.txtMuliplicationFactor.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtMuliplicationFactor.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtMuliplicationFactor.Size = New System.Drawing.Size(130, 20)
        Me.txtMuliplicationFactor.TabIndex = 15
        Me.txtMuliplicationFactor.Text = "0.000000"
        Me.txtMuliplicationFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMuliplicationFactor.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(133, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Multiply column with value:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.tcMultiplicationMethod)
        Me.GroupBox3.Controls.Add(Me.csColumnToMultiply)
        Me.GroupBox3.Location = New System.Drawing.Point(658, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(289, 167)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "settings"
        '
        'tcMultiplicationMethod
        '
        Me.tcMultiplicationMethod.Controls.Add(Me.tpFactorMultiplication)
        Me.tcMultiplicationMethod.Controls.Add(Me.tpOtherColumnMultiplication)
        Me.tcMultiplicationMethod.Location = New System.Drawing.Point(3, 54)
        Me.tcMultiplicationMethod.Name = "tcMultiplicationMethod"
        Me.tcMultiplicationMethod.SelectedIndex = 0
        Me.tcMultiplicationMethod.Size = New System.Drawing.Size(283, 104)
        Me.tcMultiplicationMethod.TabIndex = 16
        '
        'tpFactorMultiplication
        '
        Me.tpFactorMultiplication.Controls.Add(Me.txtMuliplicationFactor)
        Me.tpFactorMultiplication.Controls.Add(Me.Label3)
        Me.tpFactorMultiplication.Location = New System.Drawing.Point(4, 22)
        Me.tpFactorMultiplication.Name = "tpFactorMultiplication"
        Me.tpFactorMultiplication.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFactorMultiplication.Size = New System.Drawing.Size(275, 78)
        Me.tpFactorMultiplication.TabIndex = 0
        Me.tpFactorMultiplication.Text = "factor multiplication"
        Me.tpFactorMultiplication.UseVisualStyleBackColor = True
        '
        'tpOtherColumnMultiplication
        '
        Me.tpOtherColumnMultiplication.Controls.Add(Me.rbDivide)
        Me.tpOtherColumnMultiplication.Controls.Add(Me.rbMultiply)
        Me.tpOtherColumnMultiplication.Controls.Add(Me.Label5)
        Me.tpOtherColumnMultiplication.Controls.Add(Me.csColumnToMultiplyWith)
        Me.tpOtherColumnMultiplication.Location = New System.Drawing.Point(4, 22)
        Me.tpOtherColumnMultiplication.Name = "tpOtherColumnMultiplication"
        Me.tpOtherColumnMultiplication.Padding = New System.Windows.Forms.Padding(3)
        Me.tpOtherColumnMultiplication.Size = New System.Drawing.Size(275, 78)
        Me.tpOtherColumnMultiplication.TabIndex = 1
        Me.tpOtherColumnMultiplication.Text = "Multiply / Divide with other column"
        Me.tpOtherColumnMultiplication.UseVisualStyleBackColor = True
        '
        'rbDivide
        '
        Me.rbDivide.AutoSize = True
        Me.rbDivide.Location = New System.Drawing.Point(138, 52)
        Me.rbDivide.Name = "rbDivide"
        Me.rbDivide.Size = New System.Drawing.Size(53, 17)
        Me.rbDivide.TabIndex = 18
        Me.rbDivide.Text = "divide"
        Me.rbDivide.UseVisualStyleBackColor = True
        '
        'rbMultiply
        '
        Me.rbMultiply.AutoSize = True
        Me.rbMultiply.Checked = True
        Me.rbMultiply.Location = New System.Drawing.Point(69, 52)
        Me.rbMultiply.Name = "rbMultiply"
        Me.rbMultiply.Size = New System.Drawing.Size(59, 17)
        Me.rbMultiply.TabIndex = 18
        Me.rbMultiply.TabStop = True
        Me.rbMultiply.Text = "multiply"
        Me.rbMultiply.UseVisualStyleBackColor = True
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
        'csColumnToMultiplyWith
        '
        Me.csColumnToMultiplyWith.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToMultiplyWith.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToMultiplyWith.Location = New System.Drawing.Point(30, 25)
        Me.csColumnToMultiplyWith.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToMultiplyWith.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToMultiplyWith.Name = "csColumnToMultiplyWith"
        Me.csColumnToMultiplyWith.SelectedColumnName = ""
        Me.csColumnToMultiplyWith.SelectedColumnNames = CType(resources.GetObject("csColumnToMultiplyWith.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToMultiplyWith.SelectedEntries = CType(resources.GetObject("csColumnToMultiplyWith.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToMultiplyWith.SelectedEntry = ""
        Me.csColumnToMultiplyWith.Size = New System.Drawing.Size(225, 21)
        Me.csColumnToMultiplyWith.TabIndex = 17
        Me.csColumnToMultiplyWith.TurnOnLastFilterSaving = False
        Me.csColumnToMultiplyWith.TurnOnLastSelectionSaving = False
        '
        'wDataMultiplication
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(949, 744)
        Me.Controls.Add(Me.btnApplyMultiplication)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataMultiplication"
        Me.Text = "Data Multiplication - "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.Controls.SetChildIndex(Me.btnApplyMultiplication, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.tcMultiplicationMethod.ResumeLayout(False)
        Me.tpFactorMultiplication.ResumeLayout(False)
        Me.tpFactorMultiplication.PerformLayout()
        Me.tpOtherColumnMultiplication.ResumeLayout(False)
        Me.tpOtherColumnMultiplication.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csColumnToMultiply As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbBeforeMultiplication As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplyMultiplication As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbAfterMultiplication As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents txtMuliplicationFactor As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents tcMultiplicationMethod As System.Windows.Forms.TabControl
    Friend WithEvents tpFactorMultiplication As System.Windows.Forms.TabPage
    Friend WithEvents tpOtherColumnMultiplication As System.Windows.Forms.TabPage
    Friend WithEvents rbDivide As System.Windows.Forms.RadioButton
    Friend WithEvents rbMultiply As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents csColumnToMultiplyWith As SpectroscopyManager.ucSpectroscopyColumnSelector
End Class
