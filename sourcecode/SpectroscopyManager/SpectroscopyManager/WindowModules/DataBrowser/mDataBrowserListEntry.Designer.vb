<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mDataBrowserListEntry
    Inherits SpectroscopyManager.MouseBoundControl

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pbPreview = New System.Windows.Forms.PictureBox()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.lblPoints = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblRecordTime = New System.Windows.Forms.Label()
        Me.lblFileTime = New System.Windows.Forms.Label()
        Me.gbFileProperties = New System.Windows.Forms.GroupBox()
        Me.gbColumns = New System.Windows.Forms.GroupBox()
        Me.lbDataColumns = New System.Windows.Forms.ListBox()
        Me.gbComment = New System.Windows.Forms.GroupBox()
        Me.txtComment = New System.Windows.Forms.TextBox()
        Me.panLoading = New System.Windows.Forms.Panel()
        Me.lblLoadingProgress = New System.Windows.Forms.Label()
        Me.gbTools = New System.Windows.Forms.GroupBox()
        Me.flpTools = New System.Windows.Forms.FlowLayoutPanel()
        Me.panSelection = New System.Windows.Forms.Panel()
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbFileProperties.SuspendLayout()
        Me.gbColumns.SuspendLayout()
        Me.gbComment.SuspendLayout()
        Me.panLoading.SuspendLayout()
        Me.gbTools.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbPreview
        '
        Me.pbPreview.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbPreview.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbPreview.Location = New System.Drawing.Point(21, 4)
        Me.pbPreview.Name = "pbPreview"
        Me.pbPreview.Size = New System.Drawing.Size(229, 138)
        Me.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbPreview.TabIndex = 0
        Me.pbPreview.TabStop = False
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblFileName.Location = New System.Drawing.Point(256, 4)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(122, 24)
        Me.lblFileName.TabIndex = 1
        Me.lblFileName.Text = "lblFileName"
        '
        'lblPoints
        '
        Me.lblPoints.AutoSize = True
        Me.lblPoints.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPoints.Location = New System.Drawing.Point(44, 78)
        Me.lblPoints.Name = "lblPoints"
        Me.lblPoints.Size = New System.Drawing.Size(55, 13)
        Me.lblPoints.TabIndex = 2
        Me.lblPoints.Text = "lblPoints"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 78)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "data:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "record time:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "file time:"
        '
        'lblRecordTime
        '
        Me.lblRecordTime.AutoSize = True
        Me.lblRecordTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecordTime.Location = New System.Drawing.Point(6, 31)
        Me.lblRecordTime.MaximumSize = New System.Drawing.Size(150, 13)
        Me.lblRecordTime.MinimumSize = New System.Drawing.Size(150, 13)
        Me.lblRecordTime.Name = "lblRecordTime"
        Me.lblRecordTime.Size = New System.Drawing.Size(150, 13)
        Me.lblRecordTime.TabIndex = 2
        Me.lblRecordTime.Text = "lblRecordTime"
        Me.lblRecordTime.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblFileTime
        '
        Me.lblFileTime.AutoSize = True
        Me.lblFileTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileTime.Location = New System.Drawing.Point(6, 60)
        Me.lblFileTime.MaximumSize = New System.Drawing.Size(150, 13)
        Me.lblFileTime.MinimumSize = New System.Drawing.Size(150, 13)
        Me.lblFileTime.Name = "lblFileTime"
        Me.lblFileTime.Size = New System.Drawing.Size(150, 13)
        Me.lblFileTime.TabIndex = 2
        Me.lblFileTime.Text = "lblFileTime"
        Me.lblFileTime.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'gbFileProperties
        '
        Me.gbFileProperties.Controls.Add(Me.lblRecordTime)
        Me.gbFileProperties.Controls.Add(Me.Label1)
        Me.gbFileProperties.Controls.Add(Me.Label3)
        Me.gbFileProperties.Controls.Add(Me.lblPoints)
        Me.gbFileProperties.Controls.Add(Me.lblFileTime)
        Me.gbFileProperties.Controls.Add(Me.Label2)
        Me.gbFileProperties.Location = New System.Drawing.Point(257, 29)
        Me.gbFileProperties.Name = "gbFileProperties"
        Me.gbFileProperties.Size = New System.Drawing.Size(163, 112)
        Me.gbFileProperties.TabIndex = 4
        Me.gbFileProperties.TabStop = False
        Me.gbFileProperties.Text = "file properties:"
        '
        'gbColumns
        '
        Me.gbColumns.Controls.Add(Me.lbDataColumns)
        Me.gbColumns.Location = New System.Drawing.Point(738, 29)
        Me.gbColumns.Name = "gbColumns"
        Me.gbColumns.Size = New System.Drawing.Size(146, 111)
        Me.gbColumns.TabIndex = 5
        Me.gbColumns.TabStop = False
        Me.gbColumns.Text = "data:"
        '
        'lbDataColumns
        '
        Me.lbDataColumns.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbDataColumns.FormattingEnabled = True
        Me.lbDataColumns.Location = New System.Drawing.Point(3, 16)
        Me.lbDataColumns.Name = "lbDataColumns"
        Me.lbDataColumns.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.lbDataColumns.Size = New System.Drawing.Size(140, 92)
        Me.lbDataColumns.TabIndex = 0
        '
        'gbComment
        '
        Me.gbComment.Controls.Add(Me.txtComment)
        Me.gbComment.Location = New System.Drawing.Point(426, 29)
        Me.gbComment.Name = "gbComment"
        Me.gbComment.Size = New System.Drawing.Size(308, 111)
        Me.gbComment.TabIndex = 6
        Me.gbComment.TabStop = False
        Me.gbComment.Text = "comment:"
        '
        'txtComment
        '
        Me.txtComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtComment.Location = New System.Drawing.Point(3, 16)
        Me.txtComment.Multiline = True
        Me.txtComment.Name = "txtComment"
        Me.txtComment.ReadOnly = True
        Me.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtComment.Size = New System.Drawing.Size(302, 92)
        Me.txtComment.TabIndex = 0
        '
        'panLoading
        '
        Me.panLoading.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panLoading.Controls.Add(Me.lblLoadingProgress)
        Me.panLoading.Location = New System.Drawing.Point(256, 31)
        Me.panLoading.Name = "panLoading"
        Me.panLoading.Size = New System.Drawing.Size(733, 111)
        Me.panLoading.TabIndex = 7
        '
        'lblLoadingProgress
        '
        Me.lblLoadingProgress.AutoSize = True
        Me.lblLoadingProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoadingProgress.Location = New System.Drawing.Point(6, 5)
        Me.lblLoadingProgress.Name = "lblLoadingProgress"
        Me.lblLoadingProgress.Size = New System.Drawing.Size(136, 24)
        Me.lblLoadingProgress.TabIndex = 1
        Me.lblLoadingProgress.Text = "loading file ..."
        '
        'gbTools
        '
        Me.gbTools.Controls.Add(Me.flpTools)
        Me.gbTools.Location = New System.Drawing.Point(886, 29)
        Me.gbTools.Name = "gbTools"
        Me.gbTools.Size = New System.Drawing.Size(100, 111)
        Me.gbTools.TabIndex = 8
        Me.gbTools.TabStop = False
        Me.gbTools.Text = "quick tools:"
        '
        'flpTools
        '
        Me.flpTools.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpTools.Location = New System.Drawing.Point(3, 16)
        Me.flpTools.Name = "flpTools"
        Me.flpTools.Size = New System.Drawing.Size(94, 92)
        Me.flpTools.TabIndex = 0
        '
        'panSelection
        '
        Me.panSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.panSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panSelection.Cursor = System.Windows.Forms.Cursors.Hand
        Me.panSelection.Dock = System.Windows.Forms.DockStyle.Left
        Me.panSelection.Location = New System.Drawing.Point(0, 0)
        Me.panSelection.Name = "panSelection"
        Me.panSelection.Size = New System.Drawing.Size(20, 145)
        Me.panSelection.TabIndex = 9
        '
        'mDataBrowserListEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.panSelection)
        Me.Controls.Add(Me.gbTools)
        Me.Controls.Add(Me.gbColumns)
        Me.Controls.Add(Me.gbComment)
        Me.Controls.Add(Me.gbFileProperties)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me.pbPreview)
        Me.Controls.Add(Me.panLoading)
        Me.Name = "mDataBrowserListEntry"
        Me.Size = New System.Drawing.Size(992, 145)
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbFileProperties.ResumeLayout(False)
        Me.gbFileProperties.PerformLayout()
        Me.gbColumns.ResumeLayout(False)
        Me.gbComment.ResumeLayout(False)
        Me.gbComment.PerformLayout()
        Me.panLoading.ResumeLayout(False)
        Me.panLoading.PerformLayout()
        Me.gbTools.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbPreview As System.Windows.Forms.PictureBox
    Friend WithEvents lblFileName As System.Windows.Forms.Label
    Friend WithEvents lblPoints As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblRecordTime As System.Windows.Forms.Label
    Friend WithEvents lblFileTime As System.Windows.Forms.Label
    Friend WithEvents gbFileProperties As System.Windows.Forms.GroupBox
    Friend WithEvents gbColumns As System.Windows.Forms.GroupBox
    Friend WithEvents lbDataColumns As System.Windows.Forms.ListBox
    Friend WithEvents gbComment As System.Windows.Forms.GroupBox
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents panLoading As System.Windows.Forms.Panel
    Friend WithEvents lblLoadingProgress As System.Windows.Forms.Label
    Friend WithEvents gbTools As System.Windows.Forms.GroupBox
    Friend WithEvents flpTools As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents panSelection As System.Windows.Forms.Panel

End Class
