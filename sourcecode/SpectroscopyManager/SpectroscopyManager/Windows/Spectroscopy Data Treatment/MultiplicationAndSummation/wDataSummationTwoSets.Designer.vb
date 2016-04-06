<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class wDataSummationTwoSets
    Inherits SpectroscopyManager.wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataSummationTwoSets))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplySummation = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBeforeSummation = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.pbReferenceData = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbAfterSummation = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csColumnToSum = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.txtSummationFactor = New SpectroscopyManager.NumericTextbox()
        Me.lblScalingFactor = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.csColumnToSumWith = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnExchangeSourceAndReferenceData = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(773, 177)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "save as column name:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(894, 174)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "Data Sum"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "column:"
        '
        'btnSaveColumn
        '
        Me.btnSaveColumn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveColumn.Enabled = False
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(961, 246)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 64)
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
        Me.btnApplySummation.Location = New System.Drawing.Point(776, 246)
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
        Me.btnCloseWindow.Location = New System.Drawing.Point(774, 533)
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
        Me.GroupBox1.Size = New System.Drawing.Size(380, 280)
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
        Me.pbBeforeSummation.MultipleSpectraStackOffset = 0R
        Me.pbBeforeSummation.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBeforeSummation.Name = "pbBeforeSummation"
        Me.pbBeforeSummation.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBeforeSummation.ShowColumnSelectors = True
        Me.pbBeforeSummation.Size = New System.Drawing.Size(374, 261)
        Me.pbBeforeSummation.TabIndex = 6
        Me.pbBeforeSummation.TurnOnLastFilterSaving_Y = True
        Me.pbBeforeSummation.TurnOnLastSelectionSaving_Y = False
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox2)
        Me.SplitContainer1.Size = New System.Drawing.Size(769, 562)
        Me.SplitContainer1.SplitterDistance = 280
        Me.SplitContainer1.TabIndex = 13
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.GroupBox1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.GroupBox4)
        Me.SplitContainer2.Size = New System.Drawing.Size(769, 280)
        Me.SplitContainer2.SplitterDistance = 380
        Me.SplitContainer2.TabIndex = 17
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.pbReferenceData)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(385, 280)
        Me.GroupBox4.TabIndex = 13
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "reference data"
        '
        'pbReferenceData
        '
        Me.pbReferenceData.AllowAdjustingXColumn = True
        Me.pbReferenceData.AllowAdjustingYColumn = True
        Me.pbReferenceData.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbReferenceData.CallbackDataPointSelected = Nothing
        Me.pbReferenceData.CallbackXRangeSelected = Nothing
        Me.pbReferenceData.CallbackXValueSelected = Nothing
        Me.pbReferenceData.CallbackXYRangeSelected = Nothing
        Me.pbReferenceData.CallbackXYValueSelected = Nothing
        Me.pbReferenceData.CallbackYRangeSelected = Nothing
        Me.pbReferenceData.CallbackYValueSelected = Nothing
        Me.pbReferenceData.ClearPointSelectionModeAfterSelection = False
        Me.pbReferenceData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbReferenceData.Location = New System.Drawing.Point(3, 16)
        Me.pbReferenceData.MultipleSpectraStackOffset = 0R
        Me.pbReferenceData.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbReferenceData.Name = "pbReferenceData"
        Me.pbReferenceData.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbReferenceData.ShowColumnSelectors = True
        Me.pbReferenceData.Size = New System.Drawing.Size(379, 261)
        Me.pbReferenceData.TabIndex = 6
        Me.pbReferenceData.TurnOnLastFilterSaving_Y = True
        Me.pbReferenceData.TurnOnLastSelectionSaving_Y = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbAfterSummation)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(769, 278)
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
        Me.pbAfterSummation.MultipleSpectraStackOffset = 0R
        Me.pbAfterSummation.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfterSummation.Name = "pbAfterSummation"
        Me.pbAfterSummation.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfterSummation.ShowColumnSelectors = True
        Me.pbAfterSummation.Size = New System.Drawing.Size(763, 259)
        Me.pbAfterSummation.TabIndex = 6
        Me.pbAfterSummation.TurnOnLastFilterSaving_Y = False
        Me.pbAfterSummation.TurnOnLastSelectionSaving_Y = False
        '
        'csColumnToSum
        '
        Me.csColumnToSum.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToSum.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToSum.Location = New System.Drawing.Point(50, 36)
        Me.csColumnToSum.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToSum.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToSum.Name = "csColumnToSum"
        Me.csColumnToSum.SelectedColumnName = ""
        Me.csColumnToSum.SelectedColumnNames = CType(resources.GetObject("csColumnToSum.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToSum.SelectedEntries = CType(resources.GetObject("csColumnToSum.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToSum.SelectedEntry = ""
        Me.csColumnToSum.Size = New System.Drawing.Size(226, 21)
        Me.csColumnToSum.TabIndex = 5
        Me.csColumnToSum.TurnOnLastFilterSaving = False
        Me.csColumnToSum.TurnOnLastSelectionSaving = False
        '
        'txtSummationFactor
        '
        Me.txtSummationFactor.AllowZero = True
        Me.txtSummationFactor.BackColor = System.Drawing.Color.White
        Me.txtSummationFactor.ForeColor = System.Drawing.Color.Black
        Me.txtSummationFactor.FormatDecimalPlaces = 6
        Me.txtSummationFactor.Location = New System.Drawing.Point(185, 104)
        Me.txtSummationFactor.Name = "txtSummationFactor"
        Me.txtSummationFactor.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtSummationFactor.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSummationFactor.Size = New System.Drawing.Size(98, 20)
        Me.txtSummationFactor.TabIndex = 15
        Me.txtSummationFactor.Text = "0,000000E+000"
        Me.txtSummationFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSummationFactor.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblScalingFactor
        '
        Me.lblScalingFactor.AutoSize = True
        Me.lblScalingFactor.Location = New System.Drawing.Point(9, 107)
        Me.lblScalingFactor.Name = "lblScalingFactor"
        Me.lblScalingFactor.Size = New System.Drawing.Size(170, 13)
        Me.lblScalingFactor.TabIndex = 14
        Me.lblScalingFactor.Text = "scaling factor of reference column:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.btnExchangeSourceAndReferenceData)
        Me.GroupBox3.Controls.Add(Me.txtSummationFactor)
        Me.GroupBox3.Controls.Add(Me.lblScalingFactor)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.csColumnToSum)
        Me.GroupBox3.Controls.Add(Me.csColumnToSumWith)
        Me.GroupBox3.Location = New System.Drawing.Point(776, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(289, 165)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "settings"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 60)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "reference column:"
        '
        'csColumnToSumWith
        '
        Me.csColumnToSumWith.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToSumWith.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToSumWith.Location = New System.Drawing.Point(50, 76)
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
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(779, 206)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(284, 26)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "NOTICE: the data will only be saved in the source file (left)." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "The reference dat" &
    "a file will stay untouched!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnExchangeSourceAndReferenceData
        '
        Me.btnExchangeSourceAndReferenceData.Image = Global.SpectroscopyManager.My.Resources.Resources.reload_16
        Me.btnExchangeSourceAndReferenceData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExchangeSourceAndReferenceData.Location = New System.Drawing.Point(12, 132)
        Me.btnExchangeSourceAndReferenceData.Name = "btnExchangeSourceAndReferenceData"
        Me.btnExchangeSourceAndReferenceData.Size = New System.Drawing.Size(263, 23)
        Me.btnExchangeSourceAndReferenceData.TabIndex = 18
        Me.btnExchangeSourceAndReferenceData.Text = "exchange source and reference data"
        Me.btnExchangeSourceAndReferenceData.UseVisualStyleBackColor = True
        '
        'wDataSummationTwoSets
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 566)
        Me.Controls.Add(Me.btnApplySummation)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataSummationTwoSets"
        Me.Text = "Data Summation - "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label3, 0)
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
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
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
    Friend WithEvents lblScalingFactor As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents csColumnToSumWith As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents pbReferenceData As mSpectroscopyTableViewer
    Friend WithEvents btnExchangeSourceAndReferenceData As Button
    Friend WithEvents Label3 As Label
End Class
