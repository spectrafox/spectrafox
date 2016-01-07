<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataNumericDerivative
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataNumericDerivative))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplyDerivation = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbSourcePreview = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbTargetPreview = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csColumn = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.dsDataSmoother = New SpectroscopyManager.mDataSmoothing()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nudDerivativeOrder = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.nudDerivativeOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(706, 294)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Save as ColumnName:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(827, 291)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "Derivative of Data"
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
        Me.btnSaveColumn.Location = New System.Drawing.Point(893, 328)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 10
        Me.btnSaveColumn.Text = "save column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplyDerivation
        '
        Me.btnApplyDerivation.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplyDerivation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplyDerivation.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyDerivation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyDerivation.Location = New System.Drawing.Point(708, 329)
        Me.btnApplyDerivation.Name = "btnApplyDerivation"
        Me.btnApplyDerivation.Size = New System.Drawing.Size(179, 64)
        Me.btnApplyDerivation.TabIndex = 9
        Me.btnApplyDerivation.Text = "calculate derivative"
        Me.btnApplyDerivation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyDerivation.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(706, 644)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(291, 29)
        Me.btnCloseWindow.TabIndex = 11
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pbSourcePreview)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(697, 335)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "source data to calculate the derivative from:"
        '
        'pbSourcePreview
        '
        Me.pbSourcePreview.AllowAdjustingXColumn = True
        Me.pbSourcePreview.AllowAdjustingYColumn = True
        Me.pbSourcePreview.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbSourcePreview.CallbackDataPointSelected = Nothing
        Me.pbSourcePreview.CallbackXRangeSelected = Nothing
        Me.pbSourcePreview.CallbackXValueSelected = Nothing
        Me.pbSourcePreview.CallbackXYRangeSelected = Nothing
        Me.pbSourcePreview.CallbackXYValueSelected = Nothing
        Me.pbSourcePreview.CallbackYRangeSelected = Nothing
        Me.pbSourcePreview.CallbackYValueSelected = Nothing
        Me.pbSourcePreview.ClearPointSelectionModeAfterSelection = False
        Me.pbSourcePreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbSourcePreview.Location = New System.Drawing.Point(3, 16)
        Me.pbSourcePreview.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbSourcePreview.Name = "pbSourcePreview"
        Me.pbSourcePreview.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbSourcePreview.ShowColumnSelectors = True
        Me.pbSourcePreview.Size = New System.Drawing.Size(691, 316)
        Me.pbSourcePreview.TabIndex = 6
        Me.pbSourcePreview.TurnOnLastFilterSaving_Y = True
        Me.pbSourcePreview.TurnOnLastSelectionSaving_Y = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 2)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(697, 671)
        Me.SplitContainer1.SplitterDistance = 335
        Me.SplitContainer1.TabIndex = 13
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbTargetPreview)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(697, 332)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "derivative of the selected data"
        '
        'pbTargetPreview
        '
        Me.pbTargetPreview.AllowAdjustingXColumn = True
        Me.pbTargetPreview.AllowAdjustingYColumn = True
        Me.pbTargetPreview.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbTargetPreview.CallbackDataPointSelected = Nothing
        Me.pbTargetPreview.CallbackXRangeSelected = Nothing
        Me.pbTargetPreview.CallbackXValueSelected = Nothing
        Me.pbTargetPreview.CallbackXYRangeSelected = Nothing
        Me.pbTargetPreview.CallbackXYValueSelected = Nothing
        Me.pbTargetPreview.CallbackYRangeSelected = Nothing
        Me.pbTargetPreview.CallbackYValueSelected = Nothing
        Me.pbTargetPreview.ClearPointSelectionModeAfterSelection = False
        Me.pbTargetPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbTargetPreview.Location = New System.Drawing.Point(3, 16)
        Me.pbTargetPreview.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbTargetPreview.Name = "pbTargetPreview"
        Me.pbTargetPreview.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbTargetPreview.ShowColumnSelectors = True
        Me.pbTargetPreview.Size = New System.Drawing.Size(691, 313)
        Me.pbTargetPreview.TabIndex = 6
        Me.pbTargetPreview.TurnOnLastFilterSaving_Y = False
        Me.pbTargetPreview.TurnOnLastSelectionSaving_Y = False
        '
        'csColumn
        '
        Me.csColumn.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumn.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumn.Location = New System.Drawing.Point(65, 22)
        Me.csColumn.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumn.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumn.Name = "csColumn"
        Me.csColumn.SelectedColumnName = ""
        Me.csColumn.SelectedColumnNames = CType(resources.GetObject("csColumn.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumn.SelectedEntries = CType(resources.GetObject("csColumn.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumn.SelectedEntry = ""
        Me.csColumn.Size = New System.Drawing.Size(211, 21)
        Me.csColumn.TabIndex = 5
        Me.csColumn.TurnOnLastFilterSaving = False
        Me.csColumn.TurnOnLastSelectionSaving = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.dsDataSmoother)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.nudDerivativeOrder)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.csColumn)
        Me.GroupBox3.Location = New System.Drawing.Point(708, 1)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(289, 278)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "settings"
        '
        'dsDataSmoother
        '
        Me.dsDataSmoother.Location = New System.Drawing.Point(17, 71)
        Me.dsDataSmoother.Name = "dsDataSmoother"
        Me.dsDataSmoother.SelectedSmoothingMethod = SpectroscopyManager.cNumericalMethods.SmoothingMethod.SavitzkyGolay
        Me.dsDataSmoother.Size = New System.Drawing.Size(252, 194)
        Me.dsDataSmoother.SmoothingParameter = 5
        Me.dsDataSmoother.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 51)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Order of derivative:"
        '
        'nudDerivativeOrder
        '
        Me.nudDerivativeOrder.Location = New System.Drawing.Point(117, 49)
        Me.nudDerivativeOrder.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.nudDerivativeOrder.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDerivativeOrder.Name = "nudDerivativeOrder"
        Me.nudDerivativeOrder.Size = New System.Drawing.Size(45, 20)
        Me.nudDerivativeOrder.TabIndex = 6
        Me.nudDerivativeOrder.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'wDataNumericDerivative
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(997, 674)
        Me.Controls.Add(Me.btnApplyDerivation)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataNumericDerivative"
        Me.Text = "Calculate Numeric Derivative - "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.Controls.SetChildIndex(Me.btnApplyDerivation, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.nudDerivativeOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csColumn As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbSourcePreview As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplyDerivation As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbTargetPreview As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents nudDerivativeOrder As System.Windows.Forms.NumericUpDown
    Friend WithEvents dsDataSmoother As SpectroscopyManager.mDataSmoothing
End Class
