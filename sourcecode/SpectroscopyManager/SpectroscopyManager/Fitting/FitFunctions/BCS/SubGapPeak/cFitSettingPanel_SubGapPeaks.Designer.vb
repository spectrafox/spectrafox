<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_SubGapPeaks
    Inherits SpectroscopyManager.cFitSettingPanel_TipSampleConvolution

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
        Me.gbSubGapPeaks = New System.Windows.Forms.GroupBox()
        Me.btnAddSubGapPeak = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboSubGapPeakType = New System.Windows.Forms.ComboBox()
        Me.flpSubGapPeaks = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbSubGapPeaks.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbSubGapPeaks
        '
        Me.gbSubGapPeaks.Controls.Add(Me.btnAddSubGapPeak)
        Me.gbSubGapPeaks.Controls.Add(Me.Label1)
        Me.gbSubGapPeaks.Controls.Add(Me.cboSubGapPeakType)
        Me.gbSubGapPeaks.Controls.Add(Me.flpSubGapPeaks)
        Me.gbSubGapPeaks.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gbSubGapPeaks.Location = New System.Drawing.Point(256, 244)
        Me.gbSubGapPeaks.Name = "gbSubGapPeaks"
        Me.gbSubGapPeaks.Size = New System.Drawing.Size(713, 319)
        Me.gbSubGapPeaks.TabIndex = 24
        Me.gbSubGapPeaks.TabStop = False
        Me.gbSubGapPeaks.Text = "Sub-Gap-Peaks"
        '
        'btnAddSubGapPeak
        '
        Me.btnAddSubGapPeak.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddSubGapPeak.Location = New System.Drawing.Point(111, 13)
        Me.btnAddSubGapPeak.Name = "btnAddSubGapPeak"
        Me.btnAddSubGapPeak.Size = New System.Drawing.Size(31, 23)
        Me.btnAddSubGapPeak.TabIndex = 5
        Me.btnAddSubGapPeak.TabStop = False
        Me.btnAddSubGapPeak.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "add sub-gap-peak:"
        '
        'cboSubGapPeakType
        '
        Me.cboSubGapPeakType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubGapPeakType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubGapPeakType.FormattingEnabled = True
        Me.cboSubGapPeakType.Location = New System.Drawing.Point(535, 13)
        Me.cboSubGapPeakType.Name = "cboSubGapPeakType"
        Me.cboSubGapPeakType.Size = New System.Drawing.Size(172, 21)
        Me.cboSubGapPeakType.TabIndex = 3
        Me.cboSubGapPeakType.TabStop = False
        Me.cboSubGapPeakType.Visible = False
        '
        'flpSubGapPeaks
        '
        Me.flpSubGapPeaks.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpSubGapPeaks.AutoScroll = True
        Me.flpSubGapPeaks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpSubGapPeaks.Location = New System.Drawing.Point(6, 40)
        Me.flpSubGapPeaks.Name = "flpSubGapPeaks"
        Me.flpSubGapPeaks.Size = New System.Drawing.Size(701, 273)
        Me.flpSubGapPeaks.TabIndex = 1
        '
        'cFitSettingPanel_SubGapPeaks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbSubGapPeaks)
        Me.Name = "cFitSettingPanel_SubGapPeaks"
        Me.Size = New System.Drawing.Size(969, 563)
        Me.Controls.SetChildIndex(Me.gbSubGapPeaks, 0)
        Me.gbSubGapPeaks.ResumeLayout(False)
        Me.gbSubGapPeaks.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbSubGapPeaks As System.Windows.Forms.GroupBox
    Friend WithEvents btnAddSubGapPeak As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboSubGapPeakType As System.Windows.Forms.ComboBox
    Friend WithEvents flpSubGapPeaks As System.Windows.Forms.FlowLayoutPanel

End Class
