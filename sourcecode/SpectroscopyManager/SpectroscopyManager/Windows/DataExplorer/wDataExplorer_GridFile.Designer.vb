<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataExplorer_GridFile
    Inherits wFormBaseExpectsGridFileOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataExplorer_GridFile))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.pbSpectrumViewer = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.spSpectraSelector = New SpectroscopyManager.ucSpectroscopyTableSelector()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.pbSpectrumViewer)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.spSpectraSelector)
        Me.SplitContainer1.Size = New System.Drawing.Size(1113, 559)
        Me.SplitContainer1.SplitterDistance = 796
        Me.SplitContainer1.TabIndex = 1
        '
        'pbSpectrumViewer
        '
        Me.pbSpectrumViewer.AllowAdjustingXColumn = True
        Me.pbSpectrumViewer.AllowAdjustingYColumn = True
        Me.pbSpectrumViewer.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbSpectrumViewer.CallbackDataPointSelected = Nothing
        Me.pbSpectrumViewer.CallbackXRangeSelected = Nothing
        Me.pbSpectrumViewer.CallbackXValueSelected = Nothing
        Me.pbSpectrumViewer.CallbackXYRangeSelected = Nothing
        Me.pbSpectrumViewer.CallbackXYValueSelected = Nothing
        Me.pbSpectrumViewer.CallbackYRangeSelected = Nothing
        Me.pbSpectrumViewer.CallbackYValueSelected = Nothing
        Me.pbSpectrumViewer.ClearPointSelectionModeAfterSelection = False
        Me.pbSpectrumViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbSpectrumViewer.Location = New System.Drawing.Point(0, 0)
        Me.pbSpectrumViewer.MultipleSpectraStackOffset = 0R
        Me.pbSpectrumViewer.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbSpectrumViewer.Name = "pbSpectrumViewer"
        Me.pbSpectrumViewer.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbSpectrumViewer.ShowColumnSelectors = True
        Me.pbSpectrumViewer.Size = New System.Drawing.Size(796, 559)
        Me.pbSpectrumViewer.TabIndex = 41
        Me.pbSpectrumViewer.TurnOnLastFilterSaving_Y = False
        Me.pbSpectrumViewer.TurnOnLastSelectionSaving_Y = False
        '
        'spSpectraSelector
        '
        Me.spSpectraSelector.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Listbox
        Me.spSpectraSelector.Dock = System.Windows.Forms.DockStyle.Fill
        Me.spSpectraSelector.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.spSpectraSelector.Location = New System.Drawing.Point(0, 0)
        Me.spSpectraSelector.MinimumSize = New System.Drawing.Size(0, 21)
        Me.spSpectraSelector.Name = "spSpectraSelector"
        Me.spSpectraSelector.SelectedEntries = CType(resources.GetObject("spSpectraSelector.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.spSpectraSelector.SelectedEntry = ""
        Me.spSpectraSelector.SelectedTable = ""
        Me.spSpectraSelector.SelectedTables = CType(resources.GetObject("spSpectraSelector.SelectedTables"), System.Collections.Generic.List(Of String))
        Me.spSpectraSelector.Size = New System.Drawing.Size(313, 559)
        Me.spSpectraSelector.TabIndex = 53
        Me.spSpectraSelector.TurnOnLastFilterSaving = False
        Me.spSpectraSelector.TurnOnLastSelectionSaving = False
        '
        'wDataExplorer_GridFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1113, 559)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "wDataExplorer_GridFile"
        Me.Text = "grid explorer"
        Me.Controls.SetChildIndex(Me.SplitContainer1, 0)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents spSpectraSelector As ucSpectroscopyTableSelector
    Friend WithEvents pbSpectrumViewer As mSpectroscopyTableViewer
End Class
