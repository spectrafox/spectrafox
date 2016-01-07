<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataExplorer_ScanImage
    Inherits SpectroscopyManager.wFormBaseExpectsScanImageFileObjectOnLoad

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
        Me.gbSpectroscopyTablePropertyGrid = New System.Windows.Forms.GroupBox()
        Me.pgSpectrumProperies = New System.Windows.Forms.PropertyGrid()
        Me.gbSpectraComment = New System.Windows.Forms.GroupBox()
        Me.txtSpectraComment = New System.Windows.Forms.TextBox()
        Me.tcSpectroscopyTable = New System.Windows.Forms.TabControl()
        Me.tpPlot = New System.Windows.Forms.TabPage()
        Me.svScanImage = New SpectroscopyManager.mScanImageViewer()
        Me.tpAdditionalComment = New System.Windows.Forms.TabPage()
        Me.btnSaveAdditionalComment = New System.Windows.Forms.Button()
        Me.txtAdditionalComment = New System.Windows.Forms.TextBox()
        Me.gbSpectroscopyTablePropertyGrid.SuspendLayout()
        Me.gbSpectraComment.SuspendLayout()
        Me.tcSpectroscopyTable.SuspendLayout()
        Me.tpPlot.SuspendLayout()
        Me.tpAdditionalComment.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbSpectroscopyTablePropertyGrid
        '
        Me.gbSpectroscopyTablePropertyGrid.Controls.Add(Me.pgSpectrumProperies)
        Me.gbSpectroscopyTablePropertyGrid.Location = New System.Drawing.Point(3, 3)
        Me.gbSpectroscopyTablePropertyGrid.Name = "gbSpectroscopyTablePropertyGrid"
        Me.gbSpectroscopyTablePropertyGrid.Size = New System.Drawing.Size(248, 517)
        Me.gbSpectroscopyTablePropertyGrid.TabIndex = 23
        Me.gbSpectroscopyTablePropertyGrid.TabStop = False
        Me.gbSpectroscopyTablePropertyGrid.Text = "File Properties"
        '
        'pgSpectrumProperies
        '
        Me.pgSpectrumProperies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgSpectrumProperies.HelpVisible = False
        Me.pgSpectrumProperies.Location = New System.Drawing.Point(3, 16)
        Me.pgSpectrumProperies.Name = "pgSpectrumProperies"
        Me.pgSpectrumProperies.Size = New System.Drawing.Size(242, 498)
        Me.pgSpectrumProperies.TabIndex = 12
        Me.pgSpectrumProperies.ToolbarVisible = False
        '
        'gbSpectraComment
        '
        Me.gbSpectraComment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbSpectraComment.Controls.Add(Me.txtSpectraComment)
        Me.gbSpectraComment.Location = New System.Drawing.Point(3, 526)
        Me.gbSpectraComment.Name = "gbSpectraComment"
        Me.gbSpectraComment.Size = New System.Drawing.Size(248, 233)
        Me.gbSpectraComment.TabIndex = 22
        Me.gbSpectraComment.TabStop = False
        Me.gbSpectraComment.Text = "File-Comment"
        '
        'txtSpectraComment
        '
        Me.txtSpectraComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSpectraComment.Location = New System.Drawing.Point(3, 16)
        Me.txtSpectraComment.Multiline = True
        Me.txtSpectraComment.Name = "txtSpectraComment"
        Me.txtSpectraComment.ReadOnly = True
        Me.txtSpectraComment.Size = New System.Drawing.Size(242, 214)
        Me.txtSpectraComment.TabIndex = 0
        '
        'tcSpectroscopyTable
        '
        Me.tcSpectroscopyTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcSpectroscopyTable.Controls.Add(Me.tpPlot)
        Me.tcSpectroscopyTable.Controls.Add(Me.tpAdditionalComment)
        Me.tcSpectroscopyTable.Location = New System.Drawing.Point(253, 3)
        Me.tcSpectroscopyTable.Name = "tcSpectroscopyTable"
        Me.tcSpectroscopyTable.SelectedIndex = 0
        Me.tcSpectroscopyTable.Size = New System.Drawing.Size(933, 756)
        Me.tcSpectroscopyTable.TabIndex = 21
        '
        'tpPlot
        '
        Me.tpPlot.Controls.Add(Me.svScanImage)
        Me.tpPlot.Location = New System.Drawing.Point(4, 22)
        Me.tpPlot.Name = "tpPlot"
        Me.tpPlot.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPlot.Size = New System.Drawing.Size(925, 730)
        Me.tpPlot.TabIndex = 0
        Me.tpPlot.Text = "plot of data"
        Me.tpPlot.UseVisualStyleBackColor = True
        '
        'svScanImage
        '
        Me.svScanImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svScanImage.Location = New System.Drawing.Point(3, 3)
        Me.svScanImage.Name = "svScanImage"
        Me.svScanImage.Size = New System.Drawing.Size(919, 724)
        Me.svScanImage.TabIndex = 0
        '
        'tpAdditionalComment
        '
        Me.tpAdditionalComment.Controls.Add(Me.btnSaveAdditionalComment)
        Me.tpAdditionalComment.Controls.Add(Me.txtAdditionalComment)
        Me.tpAdditionalComment.Location = New System.Drawing.Point(4, 22)
        Me.tpAdditionalComment.Name = "tpAdditionalComment"
        Me.tpAdditionalComment.Size = New System.Drawing.Size(925, 730)
        Me.tpAdditionalComment.TabIndex = 2
        Me.tpAdditionalComment.Text = "extended comment"
        Me.tpAdditionalComment.UseVisualStyleBackColor = True
        '
        'btnSaveAdditionalComment
        '
        Me.btnSaveAdditionalComment.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveAdditionalComment.Image = Global.SpectroscopyManager.My.Resources.Resources.save_16
        Me.btnSaveAdditionalComment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveAdditionalComment.Location = New System.Drawing.Point(762, 704)
        Me.btnSaveAdditionalComment.Name = "btnSaveAdditionalComment"
        Me.btnSaveAdditionalComment.Size = New System.Drawing.Size(160, 23)
        Me.btnSaveAdditionalComment.TabIndex = 3
        Me.btnSaveAdditionalComment.Text = "save extended comment"
        Me.btnSaveAdditionalComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveAdditionalComment.UseVisualStyleBackColor = True
        '
        'txtAdditionalComment
        '
        Me.txtAdditionalComment.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdditionalComment.Font = New System.Drawing.Font("Consolas", 8.25!)
        Me.txtAdditionalComment.Location = New System.Drawing.Point(3, 3)
        Me.txtAdditionalComment.Multiline = True
        Me.txtAdditionalComment.Name = "txtAdditionalComment"
        Me.txtAdditionalComment.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtAdditionalComment.Size = New System.Drawing.Size(919, 695)
        Me.txtAdditionalComment.TabIndex = 2
        Me.txtAdditionalComment.WordWrap = False
        '
        'wDataExplorer_ScanImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1183, 758)
        Me.Controls.Add(Me.gbSpectroscopyTablePropertyGrid)
        Me.Controls.Add(Me.gbSpectraComment)
        Me.Controls.Add(Me.tcSpectroscopyTable)
        Me.Name = "wDataExplorer_ScanImage"
        Me.Text = "Image Explorer - "
        Me.Controls.SetChildIndex(Me.tcSpectroscopyTable, 0)
        Me.Controls.SetChildIndex(Me.gbSpectraComment, 0)
        Me.Controls.SetChildIndex(Me.gbSpectroscopyTablePropertyGrid, 0)
        Me.gbSpectroscopyTablePropertyGrid.ResumeLayout(False)
        Me.gbSpectraComment.ResumeLayout(False)
        Me.gbSpectraComment.PerformLayout()
        Me.tcSpectroscopyTable.ResumeLayout(False)
        Me.tpPlot.ResumeLayout(False)
        Me.tpAdditionalComment.ResumeLayout(False)
        Me.tpAdditionalComment.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbSpectroscopyTablePropertyGrid As System.Windows.Forms.GroupBox
    Friend WithEvents pgSpectrumProperies As System.Windows.Forms.PropertyGrid
    Friend WithEvents gbSpectraComment As System.Windows.Forms.GroupBox
    Friend WithEvents txtSpectraComment As System.Windows.Forms.TextBox
    Friend WithEvents tcSpectroscopyTable As System.Windows.Forms.TabControl
    Friend WithEvents tpPlot As System.Windows.Forms.TabPage
    Friend WithEvents svScanImage As SpectroscopyManager.mScanImageViewer
    Friend WithEvents tpAdditionalComment As System.Windows.Forms.TabPage
    Friend WithEvents btnSaveAdditionalComment As System.Windows.Forms.Button
    Friend WithEvents txtAdditionalComment As System.Windows.Forms.TextBox
End Class
