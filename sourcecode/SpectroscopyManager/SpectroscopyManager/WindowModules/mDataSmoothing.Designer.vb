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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbMethods = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.udSmoothProperties = New System.Windows.Forms.NumericUpDown()
        Me.lblPropertyName = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.udSmoothProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Smoothing Method:"
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
        Me.cbMethods.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.txtDescription)
        Me.GroupBox1.Controls.Add(Me.udSmoothProperties)
        Me.GroupBox1.Controls.Add(Me.lblPropertyName)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 34)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(243, 125)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Smooth Properties:"
        '
        'txtDescription
        '
        Me.txtDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescription.Location = New System.Drawing.Point(6, 47)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(231, 72)
        Me.txtDescription.TabIndex = 2
        '
        'udSmoothProperties
        '
        Me.udSmoothProperties.Location = New System.Drawing.Point(150, 19)
        Me.udSmoothProperties.Name = "udSmoothProperties"
        Me.udSmoothProperties.Size = New System.Drawing.Size(46, 20)
        Me.udSmoothProperties.TabIndex = 1
        Me.udSmoothProperties.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblPropertyName
        '
        Me.lblPropertyName.AutoSize = True
        Me.lblPropertyName.Location = New System.Drawing.Point(15, 21)
        Me.lblPropertyName.Name = "lblPropertyName"
        Me.lblPropertyName.Size = New System.Drawing.Size(129, 13)
        Me.lblPropertyName.TabIndex = 0
        Me.lblPropertyName.Text = "Smooth over neighbors #:"
        '
        'mDataSmoothing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cbMethods)
        Me.Controls.Add(Me.Label1)
        Me.Name = "mDataSmoothing"
        Me.Size = New System.Drawing.Size(252, 162)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.udSmoothProperties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbMethods As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents udSmoothProperties As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPropertyName As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox

End Class
