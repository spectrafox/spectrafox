<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wBoundarySelector
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
        Me.txtLowerBoundary = New SpectroscopyManager.NumericTextbox()
        Me.txtUpperBoundary = New SpectroscopyManager.NumericTextbox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ckbActivateUpper = New System.Windows.Forms.CheckBox()
        Me.ckbActivateLower = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'txtLowerBoundary
        '
        Me.txtLowerBoundary.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLowerBoundary.BackColor = System.Drawing.Color.White
        Me.txtLowerBoundary.ForeColor = System.Drawing.Color.Black
        Me.txtLowerBoundary.FormatDecimalPlaces = 6
        Me.txtLowerBoundary.Location = New System.Drawing.Point(97, 30)
        Me.txtLowerBoundary.Name = "txtLowerBoundary"
        Me.txtLowerBoundary.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtLowerBoundary.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtLowerBoundary.Size = New System.Drawing.Size(100, 20)
        Me.txtLowerBoundary.TabIndex = 2
        Me.txtLowerBoundary.Text = "0.000000"
        Me.txtLowerBoundary.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLowerBoundary.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtUpperBoundary
        '
        Me.txtUpperBoundary.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUpperBoundary.BackColor = System.Drawing.Color.White
        Me.txtUpperBoundary.ForeColor = System.Drawing.Color.Black
        Me.txtUpperBoundary.FormatDecimalPlaces = 6
        Me.txtUpperBoundary.Location = New System.Drawing.Point(97, 6)
        Me.txtUpperBoundary.Name = "txtUpperBoundary"
        Me.txtUpperBoundary.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtUpperBoundary.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtUpperBoundary.Size = New System.Drawing.Size(100, 20)
        Me.txtUpperBoundary.TabIndex = 1
        Me.txtUpperBoundary.Text = "0.000000"
        Me.txtUpperBoundary.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtUpperBoundary.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "lower boundary:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "upper boundary:"
        '
        'ckbActivateUpper
        '
        Me.ckbActivateUpper.AutoSize = True
        Me.ckbActivateUpper.Location = New System.Drawing.Point(207, 9)
        Me.ckbActivateUpper.Name = "ckbActivateUpper"
        Me.ckbActivateUpper.Size = New System.Drawing.Size(15, 14)
        Me.ckbActivateUpper.TabIndex = 3
        Me.ckbActivateUpper.UseVisualStyleBackColor = True
        '
        'ckbActivateLower
        '
        Me.ckbActivateLower.AutoSize = True
        Me.ckbActivateLower.Location = New System.Drawing.Point(207, 34)
        Me.ckbActivateLower.Name = "ckbActivateLower"
        Me.ckbActivateLower.Size = New System.Drawing.Size(15, 14)
        Me.ckbActivateLower.TabIndex = 4
        Me.ckbActivateLower.UseVisualStyleBackColor = True
        '
        'wBoundarySelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(236, 57)
        Me.Controls.Add(Me.ckbActivateLower)
        Me.Controls.Add(Me.ckbActivateUpper)
        Me.Controls.Add(Me.txtLowerBoundary)
        Me.Controls.Add(Me.txtUpperBoundary)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "wBoundarySelector"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "boundary selector"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtLowerBoundary As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtUpperBoundary As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ckbActivateUpper As System.Windows.Forms.CheckBox
    Friend WithEvents ckbActivateLower As System.Windows.Forms.CheckBox
End Class
