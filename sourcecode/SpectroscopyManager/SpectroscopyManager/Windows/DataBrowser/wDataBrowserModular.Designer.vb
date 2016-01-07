<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataBrowserModular
    Inherits SpectroscopyManager.wFormBase

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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.spDataPreview = New System.Windows.Forms.SplitContainer()
        Me.pbPreviewBox = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.svScanViewer = New SpectroscopyManager.mScanImageViewer()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.spDataPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.spDataPreview.Panel1.SuspendLayout()
        Me.spDataPreview.Panel2.SuspendLayout()
        Me.spDataPreview.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.spDataPreview)
        Me.SplitContainer1.Size = New System.Drawing.Size(1326, 883)
        Me.SplitContainer1.SplitterDistance = 744
        Me.SplitContainer1.TabIndex = 0
        '
        'spDataPreview
        '
        Me.spDataPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.spDataPreview.Location = New System.Drawing.Point(0, 0)
        Me.spDataPreview.Name = "spDataPreview"
        Me.spDataPreview.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'spDataPreview.Panel1
        '
        Me.spDataPreview.Panel1.Controls.Add(Me.pbPreviewBox)
        '
        'spDataPreview.Panel2
        '
        Me.spDataPreview.Panel2.Controls.Add(Me.svScanViewer)
        Me.spDataPreview.Size = New System.Drawing.Size(578, 883)
        Me.spDataPreview.SplitterDistance = 432
        Me.spDataPreview.TabIndex = 0
        '
        'pbPreviewBox
        '
        Me.pbPreviewBox.AllowAdjustingXColumn = True
        Me.pbPreviewBox.AllowAdjustingYColumn = True
        Me.pbPreviewBox.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbPreviewBox.CallbackXRangeSelected = Nothing
        Me.pbPreviewBox.CallbackXValueSelected = Nothing
        Me.pbPreviewBox.CallbackXYRangeSelected = Nothing
        Me.pbPreviewBox.CallbackYRangeSelected = Nothing
        Me.pbPreviewBox.CallbackYValueSelected = Nothing
        Me.pbPreviewBox.ClearPointSelectionModeAfterSelection = False
        Me.pbPreviewBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbPreviewBox.Location = New System.Drawing.Point(0, 0)
        Me.pbPreviewBox.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.pbPreviewBox.Name = "pbPreviewBox"
        Me.pbPreviewBox.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbPreviewBox.ShowColumnSelectors = True
        Me.pbPreviewBox.Size = New System.Drawing.Size(578, 432)
        Me.pbPreviewBox.TabIndex = 14
        Me.pbPreviewBox.TurnOnLastFilterSaving_Y = True
        Me.pbPreviewBox.TurnOnLastSelectionSaving_Y = True
        '
        'svScanViewer
        '
        Me.svScanViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svScanViewer.Location = New System.Drawing.Point(0, 0)
        Me.svScanViewer.Name = "svScanViewer"
        Me.svScanViewer.Size = New System.Drawing.Size(578, 447)
        Me.svScanViewer.TabIndex = 1
        '
        'wDataBrowserModular
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1326, 883)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "wDataBrowserModular"
        Me.Text = "Data Browser"
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.spDataPreview.Panel1.ResumeLayout(False)
        Me.spDataPreview.Panel2.ResumeLayout(False)
        CType(Me.spDataPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.spDataPreview.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents spDataPreview As System.Windows.Forms.SplitContainer
    Friend WithEvents pbPreviewBox As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents svScanViewer As SpectroscopyManager.mScanImageViewer
End Class
