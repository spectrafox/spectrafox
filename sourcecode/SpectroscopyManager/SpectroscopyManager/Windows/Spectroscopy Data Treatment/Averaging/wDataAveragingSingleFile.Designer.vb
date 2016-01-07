<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataAveragingSingleFile
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataAveragingSingleFile))
        Me.lbSourceColumnSelector = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbAfterAveraging = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.btnApplyAveraging = New System.Windows.Forms.Button()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.lblSaveAs = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbSourceColumnSelector
        '
        Me.lbSourceColumnSelector.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbSourceColumnSelector.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Listbox
        Me.lbSourceColumnSelector.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lbSourceColumnSelector.Location = New System.Drawing.Point(675, 39)
        Me.lbSourceColumnSelector.MinimumSize = New System.Drawing.Size(0, 21)
        Me.lbSourceColumnSelector.Name = "lbSourceColumnSelector"
        Me.lbSourceColumnSelector.SelectedColumnName = ""
        Me.lbSourceColumnSelector.SelectedColumnNames = CType(resources.GetObject("lbSourceColumnSelector.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.lbSourceColumnSelector.SelectedEntries = CType(resources.GetObject("lbSourceColumnSelector.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.lbSourceColumnSelector.SelectedEntry = ""
        Me.lbSourceColumnSelector.Size = New System.Drawing.Size(205, 144)
        Me.lbSourceColumnSelector.TabIndex = 1
        Me.lbSourceColumnSelector.TurnOnLastFilterSaving = False
        Me.lbSourceColumnSelector.TurnOnLastSelectionSaving = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.pbAfterAveraging)
        Me.GroupBox2.Location = New System.Drawing.Point(0, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(589, 324)
        Me.GroupBox2.TabIndex = 19
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "after averaging:"
        '
        'pbAfterAveraging
        '
        Me.pbAfterAveraging.AllowAdjustingXColumn = True
        Me.pbAfterAveraging.AllowAdjustingYColumn = True
        Me.pbAfterAveraging.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfterAveraging.CallbackDataPointSelected = Nothing
        Me.pbAfterAveraging.CallbackXRangeSelected = Nothing
        Me.pbAfterAveraging.CallbackXValueSelected = Nothing
        Me.pbAfterAveraging.CallbackXYRangeSelected = Nothing
        Me.pbAfterAveraging.CallbackXYValueSelected = Nothing
        Me.pbAfterAveraging.CallbackYRangeSelected = Nothing
        Me.pbAfterAveraging.CallbackYValueSelected = Nothing
        Me.pbAfterAveraging.ClearPointSelectionModeAfterSelection = False
        Me.pbAfterAveraging.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfterAveraging.Location = New System.Drawing.Point(3, 16)
        Me.pbAfterAveraging.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbAfterAveraging.Name = "pbAfterAveraging"
        Me.pbAfterAveraging.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfterAveraging.ShowColumnSelectors = True
        Me.pbAfterAveraging.Size = New System.Drawing.Size(583, 305)
        Me.pbAfterAveraging.TabIndex = 6
        Me.pbAfterAveraging.TurnOnLastFilterSaving_Y = False
        Me.pbAfterAveraging.TurnOnLastSelectionSaving_Y = False
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(597, 297)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(283, 29)
        Me.btnCloseWindow.TabIndex = 18
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'btnSaveColumn
        '
        Me.btnSaveColumn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveColumn.Enabled = False
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(774, 215)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(106, 65)
        Me.btnSaveColumn.TabIndex = 17
        Me.btnSaveColumn.Text = "save column"
        Me.btnSaveColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'btnApplyAveraging
        '
        Me.btnApplyAveraging.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApplyAveraging.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplyAveraging.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyAveraging.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyAveraging.Location = New System.Drawing.Point(597, 216)
        Me.btnApplyAveraging.Name = "btnApplyAveraging"
        Me.btnApplyAveraging.Size = New System.Drawing.Size(171, 64)
        Me.btnApplyAveraging.TabIndex = 16
        Me.btnApplyAveraging.Text = "average columns"
        Me.btnApplyAveraging.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyAveraging.UseVisualStyleBackColor = True
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(675, 189)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 15
        Me.txtNewColumnName.Text = "averaged data"
        '
        'lblSaveAs
        '
        Me.lblSaveAs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSaveAs.AutoSize = True
        Me.lblSaveAs.Location = New System.Drawing.Point(595, 192)
        Me.lblSaveAs.Name = "lblSaveAs"
        Me.lblSaveAs.Size = New System.Drawing.Size(80, 13)
        Me.lblSaveAs.TabIndex = 14
        Me.lblSaveAs.Text = "save output as:"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(594, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(244, 39)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "This tool allows you to average multiple columns" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "in the same file. Just select t" &
    "he columns you would" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "like to average:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'wDataAveragingSingleFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(886, 334)
        Me.Controls.Add(Me.lbSourceColumnSelector)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnSaveColumn)
        Me.Controls.Add(Me.btnApplyAveraging)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblSaveAs)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataAveragingSingleFile"
        Me.Text = "Data Averaging - "
        Me.Controls.SetChildIndex(Me.lblSaveAs, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.txtNewColumnName, 0)
        Me.Controls.SetChildIndex(Me.btnApplyAveraging, 0)
        Me.Controls.SetChildIndex(Me.btnSaveColumn, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.GroupBox2, 0)
        Me.Controls.SetChildIndex(Me.lbSourceColumnSelector, 0)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbSourceColumnSelector As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbAfterAveraging As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents btnApplyAveraging As System.Windows.Forms.Button
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
