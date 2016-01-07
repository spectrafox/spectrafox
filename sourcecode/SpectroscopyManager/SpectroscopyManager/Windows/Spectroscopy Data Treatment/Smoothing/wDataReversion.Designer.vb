<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataReversion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataReversion))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplyReversion = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbBefore = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.csColumnToRevert = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(731, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "new column name:"
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(832, 58)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(185, 20)
        Me.txtNewColumnName.TabIndex = 4
        Me.txtNewColumnName.Text = "reversed data"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(732, 34)
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
        Me.btnSaveColumn.Location = New System.Drawing.Point(917, 84)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 10
        Me.btnSaveColumn.Text = "save data"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplyReversion
        '
        Me.btnApplyReversion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplyReversion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplyReversion.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyReversion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyReversion.Location = New System.Drawing.Point(732, 85)
        Me.btnApplyReversion.Name = "btnApplyReversion"
        Me.btnApplyReversion.Size = New System.Drawing.Size(179, 64)
        Me.btnApplyReversion.TabIndex = 9
        Me.btnApplyReversion.Text = "revert data order"
        Me.btnApplyReversion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyReversion.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(735, 433)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(291, 29)
        Me.btnCloseWindow.TabIndex = 11
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.pbBefore)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(720, 462)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "before:"
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
        Me.pbBefore.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.pbBefore.Name = "pbBefore"
        Me.pbBefore.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBefore.ShowColumnSelectors = True
        Me.pbBefore.Size = New System.Drawing.Size(714, 443)
        Me.pbBefore.TabIndex = 6
        Me.pbBefore.TurnOnLastFilterSaving_Y = True
        Me.pbBefore.TurnOnLastSelectionSaving_Y = False
        '
        'csColumnToRevert
        '
        Me.csColumnToRevert.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.csColumnToRevert.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumnToRevert.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumnToRevert.Location = New System.Drawing.Point(783, 31)
        Me.csColumnToRevert.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumnToRevert.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumnToRevert.Name = "csColumnToRevert"
        Me.csColumnToRevert.SelectedColumnName = ""
        Me.csColumnToRevert.SelectedColumnNames = CType(resources.GetObject("csColumnToRevert.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumnToRevert.SelectedEntries = CType(resources.GetObject("csColumnToRevert.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumnToRevert.SelectedEntry = ""
        Me.csColumnToRevert.Size = New System.Drawing.Size(234, 21)
        Me.csColumnToRevert.TabIndex = 5
        Me.csColumnToRevert.TurnOnLastFilterSaving = False
        Me.csColumnToRevert.TurnOnLastSelectionSaving = False
        '
        'lblDescription
        '
        Me.lblDescription.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(731, 9)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(249, 13)
        Me.lblDescription.TabIndex = 13
        Me.lblDescription.Text = "This tool reverses the order of the data in a column."
        '
        'wDataReversion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1029, 466)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.btnApplyReversion)
        Me.Controls.Add(Me.csColumnToRevert)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataReversion"
        Me.Text = "Data Reversion - "
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.csColumnToRevert, 0)
        Me.Controls.SetChildIndex(Me.btnApplyReversion, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.Controls.SetChildIndex(Me.lblDescription, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents csColumnToRevert As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents pbBefore As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplyReversion As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDescription As Label
End Class
