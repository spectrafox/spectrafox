<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucColorPalettePicker
    Inherits System.Windows.Forms.UserControl

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
        Me.components = New System.ComponentModel.Container()
        Me.cbColorPicker = New System.Windows.Forms.ComboBox()
        Me.pbSchemePreview = New System.Windows.Forms.PictureBox()
        Me.cmPreviewImage = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pbSchemePreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmPreviewImage.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbColorPicker
        '
        Me.cbColorPicker.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbColorPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColorPicker.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cbColorPicker.FormattingEnabled = True
        Me.cbColorPicker.Location = New System.Drawing.Point(3, 0)
        Me.cbColorPicker.Name = "cbColorPicker"
        Me.cbColorPicker.Size = New System.Drawing.Size(206, 21)
        Me.cbColorPicker.TabIndex = 0
        '
        'pbSchemePreview
        '
        Me.pbSchemePreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbSchemePreview.ContextMenuStrip = Me.cmPreviewImage
        Me.pbSchemePreview.Location = New System.Drawing.Point(0, 25)
        Me.pbSchemePreview.Name = "pbSchemePreview"
        Me.pbSchemePreview.Size = New System.Drawing.Size(212, 18)
        Me.pbSchemePreview.TabIndex = 1
        Me.pbSchemePreview.TabStop = False
        '
        'cmPreviewImage
        '
        Me.cmPreviewImage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem})
        Me.cmPreviewImage.Name = "cmPreviewImage"
        Me.cmPreviewImage.Size = New System.Drawing.Size(153, 48)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CopyToolStripMenuItem.Text = "copy"
        '
        'ucColorPalettePicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pbSchemePreview)
        Me.Controls.Add(Me.cbColorPicker)
        Me.Name = "ucColorPalettePicker"
        Me.Size = New System.Drawing.Size(212, 45)
        CType(Me.pbSchemePreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmPreviewImage.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbColorPicker As System.Windows.Forms.ComboBox
    Friend WithEvents pbSchemePreview As PictureBox
    Friend WithEvents cmPreviewImage As ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
End Class
