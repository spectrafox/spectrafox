<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wGridExporter
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wGridExporter))
        Me.spExportSelector = New SpectroscopyManager.ucSpectroscopyTableSelector()
        Me.lblValueRange_SelectPlotData = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.pgProgressBar = New System.Windows.Forms.ProgressBar()
        Me.lblProgressBar = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'spExportSelector
        '
        Me.spExportSelector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.spExportSelector.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Listbox
        Me.spExportSelector.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.spExportSelector.Location = New System.Drawing.Point(15, 25)
        Me.spExportSelector.MinimumSize = New System.Drawing.Size(0, 21)
        Me.spExportSelector.Name = "spExportSelector"
        Me.spExportSelector.SelectedEntries = CType(resources.GetObject("spExportSelector.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.spExportSelector.SelectedEntry = ""
        Me.spExportSelector.SelectedTable = ""
        Me.spExportSelector.SelectedTables = CType(resources.GetObject("spExportSelector.SelectedTables"), System.Collections.Generic.List(Of String))
        Me.spExportSelector.Size = New System.Drawing.Size(539, 296)
        Me.spExportSelector.TabIndex = 52
        Me.spExportSelector.TurnOnLastFilterSaving = False
        Me.spExportSelector.TurnOnLastSelectionSaving = False
        '
        'lblValueRange_SelectPlotData
        '
        Me.lblValueRange_SelectPlotData.AutoSize = True
        Me.lblValueRange_SelectPlotData.Location = New System.Drawing.Point(12, 9)
        Me.lblValueRange_SelectPlotData.Name = "lblValueRange_SelectPlotData"
        Me.lblValueRange_SelectPlotData.Size = New System.Drawing.Size(242, 13)
        Me.lblValueRange_SelectPlotData.TabIndex = 51
        Me.lblValueRange_SelectPlotData.Text = "select the experiments to export as a separate file:"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(15, 356)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 53
        Me.btnClose.Text = "close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Image = Global.SpectroscopyManager.My.Resources.Resources.export_25
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(319, 345)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(217, 35)
        Me.btnExport.TabIndex = 81
        Me.btnExport.Text = "export selected experiments"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'pgProgressBar
        '
        Me.pgProgressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgProgressBar.Location = New System.Drawing.Point(15, 327)
        Me.pgProgressBar.Name = "pgProgressBar"
        Me.pgProgressBar.Size = New System.Drawing.Size(521, 10)
        Me.pgProgressBar.TabIndex = 82
        '
        'lblProgressBar
        '
        Me.lblProgressBar.AutoSize = True
        Me.lblProgressBar.Location = New System.Drawing.Point(15, 338)
        Me.lblProgressBar.Name = "lblProgressBar"
        Me.lblProgressBar.Size = New System.Drawing.Size(10, 13)
        Me.lblProgressBar.TabIndex = 83
        Me.lblProgressBar.Text = "-"
        '
        'wGridExporter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(566, 385)
        Me.Controls.Add(Me.lblProgressBar)
        Me.Controls.Add(Me.pgProgressBar)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.spExportSelector)
        Me.Controls.Add(Me.lblValueRange_SelectPlotData)
        Me.Name = "wGridExporter"
        Me.Text = "spectra export from grid file"
        Me.Controls.SetChildIndex(Me.lblValueRange_SelectPlotData, 0)
        Me.Controls.SetChildIndex(Me.spExportSelector, 0)
        Me.Controls.SetChildIndex(Me.btnClose, 0)
        Me.Controls.SetChildIndex(Me.btnExport, 0)
        Me.Controls.SetChildIndex(Me.pgProgressBar, 0)
        Me.Controls.SetChildIndex(Me.lblProgressBar, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents spExportSelector As ucSpectroscopyTableSelector
    Friend WithEvents lblValueRange_SelectPlotData As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents pgProgressBar As ProgressBar
    Friend WithEvents lblProgressBar As Label
End Class
