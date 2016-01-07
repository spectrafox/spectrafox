<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataNormalization
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataNormalization))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplyNormalization = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBeforeNormalization = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbAfterNormalization = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csColumnToNormalize = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.dsDataSmoother = New SpectroscopyManager.mDataSmoothing()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtRightValue = New SpectroscopyManager.NumericTextbox()
        Me.txtLeftValue = New SpectroscopyManager.NumericTextbox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(652, 366)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Save as ColumnName:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(773, 363)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "Normalized Data"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(657, 4)
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
        Me.btnSaveColumn.Location = New System.Drawing.Point(839, 389)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 6
        Me.btnSaveColumn.Text = "Save Column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplyNormalization
        '
        Me.btnApplyNormalization.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplyNormalization.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplyNormalization.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyNormalization.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyNormalization.Location = New System.Drawing.Point(654, 390)
        Me.btnApplyNormalization.Name = "btnApplyNormalization"
        Me.btnApplyNormalization.Size = New System.Drawing.Size(179, 64)
        Me.btnApplyNormalization.TabIndex = 5
        Me.btnApplyNormalization.Text = "Apply Normalization"
        Me.btnApplyNormalization.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyNormalization.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(654, 711)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(290, 29)
        Me.btnCloseWindow.TabIndex = 7
        Me.btnCloseWindow.Text = "Close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pbBeforeNormalization)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(640, 368)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Before Normalization:"
        '
        'pbBeforeNormalization
        '
        Me.pbBeforeNormalization.AllowAdjustingXColumn = True
        Me.pbBeforeNormalization.AllowAdjustingYColumn = True
        Me.pbBeforeNormalization.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBeforeNormalization.CallbackDataPointSelected = Nothing
        Me.pbBeforeNormalization.CallbackXRangeSelected = Nothing
        Me.pbBeforeNormalization.CallbackXValueSelected = Nothing
        Me.pbBeforeNormalization.CallbackXYRangeSelected = Nothing
        Me.pbBeforeNormalization.CallbackXYValueSelected = Nothing
        Me.pbBeforeNormalization.CallbackYRangeSelected = Nothing
        Me.pbBeforeNormalization.CallbackYValueSelected = Nothing
        Me.pbBeforeNormalization.ClearPointSelectionModeAfterSelection = False
        Me.pbBeforeNormalization.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBeforeNormalization.Location = New System.Drawing.Point(3, 16)
        Me.pbBeforeNormalization.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBeforeNormalization.Name = "pbBeforeNormalization"
        Me.pbBeforeNormalization.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBeforeNormalization.ShowColumnSelectors = True
        Me.pbBeforeNormalization.Size = New System.Drawing.Size(634, 349)
        Me.pbBeforeNormalization.TabIndex = 6
        Me.pbBeforeNormalization.TurnOnLastFilterSaving_Y = True
        Me.pbBeforeNormalization.TurnOnLastSelectionSaving_Y = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(8, 1)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(640, 739)
        Me.SplitContainer1.SplitterDistance = 368
        Me.SplitContainer1.TabIndex = 13
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbAfterNormalization)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(640, 367)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "After Normalization:"
        '
        'pbAfterNormalization
        '
        Me.pbAfterNormalization.AllowAdjustingXColumn = True
        Me.pbAfterNormalization.AllowAdjustingYColumn = True
        Me.pbAfterNormalization.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfterNormalization.CallbackDataPointSelected = Nothing
        Me.pbAfterNormalization.CallbackXRangeSelected = Nothing
        Me.pbAfterNormalization.CallbackXValueSelected = Nothing
        Me.pbAfterNormalization.CallbackXYRangeSelected = Nothing
        Me.pbAfterNormalization.CallbackXYValueSelected = Nothing
        Me.pbAfterNormalization.CallbackYRangeSelected = Nothing
        Me.pbAfterNormalization.CallbackYValueSelected = Nothing
        Me.pbAfterNormalization.ClearPointSelectionModeAfterSelection = False
        Me.pbAfterNormalization.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfterNormalization.Location = New System.Drawing.Point(3, 16)
        Me.pbAfterNormalization.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfterNormalization.Name = "pbAfterNormalization"
        Me.pbAfterNormalization.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfterNormalization.ShowColumnSelectors = True
        Me.pbAfterNormalization.Size = New System.Drawing.Size(634, 348)
        Me.pbAfterNormalization.TabIndex = 6
        Me.pbAfterNormalization.TurnOnLastFilterSaving_Y = False
        Me.pbAfterNormalization.TurnOnLastSelectionSaving_Y = False
        '
        'csColumnToNormalize
        '
        Me.csColumnToNormalize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.csColumnToNormalize.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToNormalize.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToNormalize.Location = New System.Drawing.Point(708, 1)
        Me.csColumnToNormalize.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToNormalize.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToNormalize.Name = "csColumnToNormalize"
        Me.csColumnToNormalize.SelectedColumnName = ""
        Me.csColumnToNormalize.SelectedColumnNames = CType(resources.GetObject("csColumnToNormalize.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToNormalize.SelectedEntries = CType(resources.GetObject("csColumnToNormalize.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToNormalize.SelectedEntry = ""
        Me.csColumnToNormalize.Size = New System.Drawing.Size(240, 21)
        Me.csColumnToNormalize.TabIndex = 0
        Me.csColumnToNormalize.TurnOnLastFilterSaving = False
        Me.csColumnToNormalize.TurnOnLastSelectionSaving = False
        '
        'dsDataSmoother
        '
        Me.dsDataSmoother.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dsDataSmoother.Location = New System.Drawing.Point(3, 16)
        Me.dsDataSmoother.Name = "dsDataSmoother"
        Me.dsDataSmoother.SelectedSmoothingMethod = SpectroscopyManager.cNumericalMethods.SmoothingMethod.SavitzkyGolay
        Me.dsDataSmoother.Size = New System.Drawing.Size(288, 172)
        Me.dsDataSmoother.SmoothingParameter = 5
        Me.dsDataSmoother.TabIndex = 1
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.txtRightValue)
        Me.GroupBox3.Controls.Add(Me.txtLeftValue)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Location = New System.Drawing.Point(654, 225)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(294, 121)
        Me.GroupBox3.TabIndex = 14
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Select Area to take as Reference:"
        '
        'txtRightValue
        '
        Me.txtRightValue.BackColor = System.Drawing.Color.White
        Me.txtRightValue.ForeColor = System.Drawing.Color.Black
        Me.txtRightValue.FormatDecimalPlaces = 6
        Me.txtRightValue.Location = New System.Drawing.Point(119, 49)
        Me.txtRightValue.Name = "txtRightValue"
        Me.txtRightValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtRightValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtRightValue.Size = New System.Drawing.Size(143, 20)
        Me.txtRightValue.TabIndex = 5
        Me.txtRightValue.Text = "0.000000"
        Me.txtRightValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRightValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtLeftValue
        '
        Me.txtLeftValue.BackColor = System.Drawing.Color.White
        Me.txtLeftValue.ForeColor = System.Drawing.Color.Black
        Me.txtLeftValue.FormatDecimalPlaces = 6
        Me.txtLeftValue.Location = New System.Drawing.Point(119, 19)
        Me.txtLeftValue.Name = "txtLeftValue"
        Me.txtLeftValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtLeftValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtLeftValue.Size = New System.Drawing.Size(143, 20)
        Me.txtLeftValue.TabIndex = 5
        Me.txtLeftValue.Text = "0.000000"
        Me.txtLeftValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLeftValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(41, 77)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(203, 39)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Note: Limit selection is possible by" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "selecting a range of data-points in" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the up" &
    "per preview graph."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(29, 52)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Upper Limit in X:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(29, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Lower Limit in X:"
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.dsDataSmoother)
        Me.GroupBox4.Location = New System.Drawing.Point(654, 28)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(294, 191)
        Me.GroupBox4.TabIndex = 15
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Smoothing of the background reference signal:"
        '
        'wDataNormalization
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(949, 744)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.btnApplyNormalization)
        Me.Controls.Add(Me.csColumnToNormalize)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataNormalization"
        Me.Text = "Data Normalization - "
        Me.Controls.SetChildIndex(Me.GroupBox4, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.csColumnToNormalize, 0)
        Me.Controls.SetChildIndex(Me.btnApplyNormalization, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dsDataSmoother As SpectroscopyManager.mDataSmoothing
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csColumnToNormalize As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbBeforeNormalization As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplyNormalization As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbAfterNormalization As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtRightValue As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtLeftValue As SpectroscopyManager.NumericTextbox
End Class
