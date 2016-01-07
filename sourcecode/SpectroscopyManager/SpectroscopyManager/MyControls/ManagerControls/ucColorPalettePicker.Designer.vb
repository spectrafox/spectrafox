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
        Me.cbColorPicker = New System.Windows.Forms.ComboBox()
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
        'ucColorPalettePicker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cbColorPicker)
        Me.Name = "ucColorPalettePicker"
        Me.Size = New System.Drawing.Size(212, 23)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbColorPicker As System.Windows.Forms.ComboBox

End Class
