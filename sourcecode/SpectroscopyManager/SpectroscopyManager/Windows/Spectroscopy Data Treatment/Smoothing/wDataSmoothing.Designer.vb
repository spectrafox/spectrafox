<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class wDataSmoothing
    Inherits SpectroscopyManager.wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataSmoothing))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplySmoothing = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBeforeSmoothing = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csColumnToSmooth = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.dsDataSmoother = New SpectroscopyManager.mDataSmoothing()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(776, 378)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Save as ColumnName:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(897, 375)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "Smoothed Data"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(778, 351)
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
        Me.btnSaveColumn.Location = New System.Drawing.Point(963, 401)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 47)
        Me.btnSaveColumn.TabIndex = 10
        Me.btnSaveColumn.Text = "Save Column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplySmoothing
        '
        Me.btnApplySmoothing.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplySmoothing.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplySmoothing.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplySmoothing.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplySmoothing.Location = New System.Drawing.Point(778, 402)
        Me.btnApplySmoothing.Name = "btnApplySmoothing"
        Me.btnApplySmoothing.Size = New System.Drawing.Size(179, 46)
        Me.btnApplySmoothing.TabIndex = 9
        Me.btnApplySmoothing.Text = "Apply Smoothing"
        Me.btnApplySmoothing.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplySmoothing.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(778, 480)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(291, 29)
        Me.btnCloseWindow.TabIndex = 11
        Me.btnCloseWindow.Text = "Close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.pbBeforeSmoothing)
        Me.GroupBox1.Location = New System.Drawing.Point(2, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(770, 512)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "data preview"
        '
        'pbBeforeSmoothing
        '
        Me.pbBeforeSmoothing.AllowAdjustingXColumn = True
        Me.pbBeforeSmoothing.AllowAdjustingYColumn = True
        Me.pbBeforeSmoothing.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBeforeSmoothing.CallbackDataPointSelected = Nothing
        Me.pbBeforeSmoothing.CallbackXRangeSelected = Nothing
        Me.pbBeforeSmoothing.CallbackXValueSelected = Nothing
        Me.pbBeforeSmoothing.CallbackXYRangeSelected = Nothing
        Me.pbBeforeSmoothing.CallbackXYValueSelected = Nothing
        Me.pbBeforeSmoothing.CallbackYRangeSelected = Nothing
        Me.pbBeforeSmoothing.CallbackYValueSelected = Nothing
        Me.pbBeforeSmoothing.ClearPointSelectionModeAfterSelection = False
        Me.pbBeforeSmoothing.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBeforeSmoothing.Location = New System.Drawing.Point(3, 16)
        Me.pbBeforeSmoothing.MultipleSpectraStackOffset = 0R
        Me.pbBeforeSmoothing.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.pbBeforeSmoothing.Name = "pbBeforeSmoothing"
        Me.pbBeforeSmoothing.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBeforeSmoothing.ShowColumnSelectors = True
        Me.pbBeforeSmoothing.Size = New System.Drawing.Size(764, 493)
        Me.pbBeforeSmoothing.TabIndex = 6
        Me.pbBeforeSmoothing.TurnOnLastFilterSaving_Y = True
        Me.pbBeforeSmoothing.TurnOnLastSelectionSaving_Y = False
        '
        'csColumnToSmooth
        '
        Me.csColumnToSmooth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.csColumnToSmooth.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToSmooth.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToSmooth.Location = New System.Drawing.Point(829, 348)
        Me.csColumnToSmooth.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToSmooth.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToSmooth.Name = "csColumnToSmooth"
        Me.csColumnToSmooth.SelectedColumnName = ""
        Me.csColumnToSmooth.SelectedColumnNames = CType(resources.GetObject("csColumnToSmooth.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToSmooth.SelectedEntries = CType(resources.GetObject("csColumnToSmooth.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToSmooth.SelectedEntry = ""
        Me.csColumnToSmooth.Size = New System.Drawing.Size(234, 21)
        Me.csColumnToSmooth.TabIndex = 5
        Me.csColumnToSmooth.TurnOnLastFilterSaving = False
        Me.csColumnToSmooth.TurnOnLastSelectionSaving = False
        '
        'dsDataSmoother
        '
        Me.dsDataSmoother.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dsDataSmoother.Location = New System.Drawing.Point(778, 1)
        Me.dsDataSmoother.Name = "dsDataSmoother"
        Me.dsDataSmoother.Size = New System.Drawing.Size(288, 334)
        Me.dsDataSmoother.TabIndex = 0
        '
        'wDataSmoothing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1069, 512)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.btnApplySmoothing)
        Me.Controls.Add(Me.csColumnToSmooth)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dsDataSmoother)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataSmoothing"
        Me.Text = "Data Smoothing - "
        Me.Controls.SetChildIndex(Me.dsDataSmoother, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.csColumnToSmooth, 0)
        Me.Controls.SetChildIndex(Me.btnApplySmoothing, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dsDataSmoother As SpectroscopyManager.mDataSmoothing
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csColumnToSmooth As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbBeforeSmoothing As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplySmoothing As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
