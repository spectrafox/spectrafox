<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mDataSmoothing
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
        Me.lblSmoothingMethod = New System.Windows.Forms.Label()
        Me.cbMethods = New System.Windows.Forms.ComboBox()
        Me.tcSettings = New System.Windows.Forms.TabControl()
        Me.tpSettings = New System.Windows.Forms.TabPage()
        Me.tpDescription = New System.Windows.Forms.TabPage()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.tcSettings.SuspendLayout()
        Me.tpDescription.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSmoothingMethod
        '
        Me.lblSmoothingMethod.AutoSize = True
        Me.lblSmoothingMethod.Location = New System.Drawing.Point(7, 10)
        Me.lblSmoothingMethod.Name = "lblSmoothingMethod"
        Me.lblSmoothingMethod.Size = New System.Drawing.Size(96, 13)
        Me.lblSmoothingMethod.TabIndex = 0
        Me.lblSmoothingMethod.Text = "smoothing method:"
        '
        'cbMethods
        '
        Me.cbMethods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMethods.FormattingEnabled = True
        Me.cbMethods.Location = New System.Drawing.Point(108, 7)
        Me.cbMethods.Name = "cbMethods"
        Me.cbMethods.Size = New System.Drawing.Size(141, 21)
        Me.cbMethods.Sorted = True
        Me.cbMethods.TabIndex = 1
        '
        'tcSettings
        '
        Me.tcSettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcSettings.Controls.Add(Me.tpSettings)
        Me.tcSettings.Controls.Add(Me.tpDescription)
        Me.tcSettings.Location = New System.Drawing.Point(6, 34)
        Me.tcSettings.Name = "tcSettings"
        Me.tcSettings.SelectedIndex = 0
        Me.tcSettings.Size = New System.Drawing.Size(243, 125)
        Me.tcSettings.TabIndex = 3
        '
        'tpSettings
        '
        Me.tpSettings.Location = New System.Drawing.Point(4, 22)
        Me.tpSettings.Name = "tpSettings"
        Me.tpSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSettings.Size = New System.Drawing.Size(235, 99)
        Me.tpSettings.TabIndex = 0
        Me.tpSettings.Text = "settings"
        Me.tpSettings.UseVisualStyleBackColor = True
        '
        'tpDescription
        '
        Me.tpDescription.Controls.Add(Me.txtDescription)
        Me.tpDescription.Location = New System.Drawing.Point(4, 22)
        Me.tpDescription.Name = "tpDescription"
        Me.tpDescription.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDescription.Size = New System.Drawing.Size(235, 99)
        Me.tpDescription.TabIndex = 1
        Me.tpDescription.Text = "description"
        Me.tpDescription.UseVisualStyleBackColor = True
        '
        'txtDescription
        '
        Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDescription.Location = New System.Drawing.Point(3, 3)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(229, 93)
        Me.txtDescription.TabIndex = 3
        '
        'mDataSmoothing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tcSettings)
        Me.Controls.Add(Me.cbMethods)
        Me.Controls.Add(Me.lblSmoothingMethod)
        Me.Name = "mDataSmoothing"
        Me.Size = New System.Drawing.Size(252, 162)
        Me.tcSettings.ResumeLayout(False)
        Me.tpDescription.ResumeLayout(False)
        Me.tpDescription.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSmoothingMethod As System.Windows.Forms.Label
    Friend WithEvents cbMethods As System.Windows.Forms.ComboBox
    Friend WithEvents tcSettings As TabControl
    Friend WithEvents tpSettings As TabPage
    Friend WithEvents tpDescription As TabPage
    Friend WithEvents txtDescription As TextBox
End Class
