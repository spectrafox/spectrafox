<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wSpectroscopyFFT
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wSpectroscopyFFT))
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.csColumnToFFT = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBeforeFFT = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbAfterFFT = New SpectroscopyManager.mSpectroscopyTableViewer()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(689, 811)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(290, 29)
        Me.btnCloseWindow.TabIndex = 21
        Me.btnCloseWindow.Text = "Close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'btnSaveColumn
        '
        Me.btnSaveColumn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveColumn.Enabled = False
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(873, 68)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 20
        Me.btnSaveColumn.Text = "Save Column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApply.Location = New System.Drawing.Point(688, 69)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(179, 64)
        Me.btnApply.TabIndex = 19
        Me.btnApply.Text = "Perform FFT"
        Me.btnApply.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'csColumnToFFT
        '
        Me.csColumnToFFT.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.csColumnToFFT.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToFFT.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToFFT.Location = New System.Drawing.Point(736, 12)
        Me.csColumnToFFT.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToFFT.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToFFT.Name = "csColumnToFFT"
        Me.csColumnToFFT.SelectedColumnName = ""
        Me.csColumnToFFT.SelectedColumnNames = CType(resources.GetObject("csColumnToFFT.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToFFT.SelectedEntries = CType(resources.GetObject("csColumnToFFT.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToFFT.SelectedEntry = ""
        Me.csColumnToFFT.Size = New System.Drawing.Size(240, 21)
        Me.csColumnToFFT.TabIndex = 15
        Me.csColumnToFFT.TurnOnLastFilterSaving = False
        Me.csColumnToFFT.TurnOnLastSelectionSaving = False
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(807, 42)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 18
        Me.txtNewColumnName.Text = "FFT Result"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(685, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Column:"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(686, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Save as ColumnName:"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(12, 12)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(671, 828)
        Me.SplitContainer1.SplitterDistance = 410
        Me.SplitContainer1.TabIndex = 22
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pbBeforeFFT)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(671, 410)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "source data:"
        '
        'pbBeforeFFT
        '
        Me.pbBeforeFFT.AllowAdjustingXColumn = True
        Me.pbBeforeFFT.AllowAdjustingYColumn = True
        Me.pbBeforeFFT.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBeforeFFT.CallbackDataPointSelected = Nothing
        Me.pbBeforeFFT.CallbackXRangeSelected = Nothing
        Me.pbBeforeFFT.CallbackXValueSelected = Nothing
        Me.pbBeforeFFT.CallbackXYRangeSelected = Nothing
        Me.pbBeforeFFT.CallbackXYValueSelected = Nothing
        Me.pbBeforeFFT.CallbackYRangeSelected = Nothing
        Me.pbBeforeFFT.CallbackYValueSelected = Nothing
        Me.pbBeforeFFT.ClearPointSelectionModeAfterSelection = False
        Me.pbBeforeFFT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBeforeFFT.Location = New System.Drawing.Point(3, 16)
        Me.pbBeforeFFT.MultipleSpectraStackOffset = 0R
        Me.pbBeforeFFT.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBeforeFFT.Name = "pbBeforeFFT"
        Me.pbBeforeFFT.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBeforeFFT.ShowColumnSelectors = True
        Me.pbBeforeFFT.Size = New System.Drawing.Size(665, 391)
        Me.pbBeforeFFT.TabIndex = 6
        Me.pbBeforeFFT.TurnOnLastFilterSaving_Y = True
        Me.pbBeforeFFT.TurnOnLastSelectionSaving_Y = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbAfterFFT)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(671, 414)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "FFT result:"
        '
        'pbAfterFFT
        '
        Me.pbAfterFFT.AllowAdjustingXColumn = True
        Me.pbAfterFFT.AllowAdjustingYColumn = True
        Me.pbAfterFFT.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfterFFT.CallbackDataPointSelected = Nothing
        Me.pbAfterFFT.CallbackXRangeSelected = Nothing
        Me.pbAfterFFT.CallbackXValueSelected = Nothing
        Me.pbAfterFFT.CallbackXYRangeSelected = Nothing
        Me.pbAfterFFT.CallbackXYValueSelected = Nothing
        Me.pbAfterFFT.CallbackYRangeSelected = Nothing
        Me.pbAfterFFT.CallbackYValueSelected = Nothing
        Me.pbAfterFFT.ClearPointSelectionModeAfterSelection = False
        Me.pbAfterFFT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfterFFT.Location = New System.Drawing.Point(3, 16)
        Me.pbAfterFFT.MultipleSpectraStackOffset = 0R
        Me.pbAfterFFT.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfterFFT.Name = "pbAfterFFT"
        Me.pbAfterFFT.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfterFFT.ShowColumnSelectors = True
        Me.pbAfterFFT.Size = New System.Drawing.Size(665, 395)
        Me.pbAfterFFT.TabIndex = 6
        Me.pbAfterFFT.TurnOnLastFilterSaving_Y = False
        Me.pbAfterFFT.TurnOnLastSelectionSaving_Y = False
        '
        'wSpectroscopyFFT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(988, 845)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.csColumnToFFT)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "wSpectroscopyFFT"
        Me.Text = "Fourier Transformation "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.csColumnToFFT, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnCloseWindow As Button
    Friend WithEvents btnSaveColumn As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents csColumnToFFT As ucSpectroscopyColumnSelector
    Friend WithEvents txtNewColumnName As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents pbBeforeFFT As mSpectroscopyTableViewer
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents pbAfterFFT As mSpectroscopyTableViewer
End Class
