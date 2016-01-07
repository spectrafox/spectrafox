<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitProcedureSettingsPanel_LMA
    Inherits SpectroscopyManager.cFitProcedureSettingsPanel

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(cFitProcedureSettingsPanel_LMA))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblCenteredDiffDescription = New System.Windows.Forms.Label()
        Me.ckbUseCenteredDifferentiation = New System.Windows.Forms.CheckBox()
        Me.txtDerivativeDelta = New SpectroscopyManager.NumericTextbox()
        Me.lblDerivativeDelta = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudMaxIterations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblCenteredDiffDescription)
        Me.GroupBox1.Controls.Add(Me.ckbUseCenteredDifferentiation)
        Me.GroupBox1.Controls.Add(Me.txtDerivativeDelta)
        Me.GroupBox1.Controls.Add(Me.lblDerivativeDelta)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 86)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(213, 159)
        Me.GroupBox1.TabIndex = 27
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "numeric derivative calculation"
        '
        'lblCenteredDiffDescription
        '
        Me.lblCenteredDiffDescription.AutoSize = True
        Me.lblCenteredDiffDescription.Location = New System.Drawing.Point(15, 70)
        Me.lblCenteredDiffDescription.Name = "lblCenteredDiffDescription"
        Me.lblCenteredDiffDescription.Size = New System.Drawing.Size(177, 78)
        Me.lblCenteredDiffDescription.TabIndex = 25
        Me.lblCenteredDiffDescription.Text = resources.GetString("lblCenteredDiffDescription.Text")
        '
        'ckbUseCenteredDifferentiation
        '
        Me.ckbUseCenteredDifferentiation.AutoSize = True
        Me.ckbUseCenteredDifferentiation.Location = New System.Drawing.Point(16, 47)
        Me.ckbUseCenteredDifferentiation.Name = "ckbUseCenteredDifferentiation"
        Me.ckbUseCenteredDifferentiation.Size = New System.Drawing.Size(154, 17)
        Me.ckbUseCenteredDifferentiation.TabIndex = 24
        Me.ckbUseCenteredDifferentiation.Text = "use centered differentiation"
        Me.ckbUseCenteredDifferentiation.UseVisualStyleBackColor = True
        '
        'txtDerivativeDelta
        '
        Me.txtDerivativeDelta.BackColor = System.Drawing.Color.White
        Me.txtDerivativeDelta.ForeColor = System.Drawing.Color.Black
        Me.txtDerivativeDelta.FormatDecimalPlaces = 6
        Me.txtDerivativeDelta.Location = New System.Drawing.Point(103, 19)
        Me.txtDerivativeDelta.Name = "txtDerivativeDelta"
        Me.txtDerivativeDelta.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtDerivativeDelta.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtDerivativeDelta.Size = New System.Drawing.Size(99, 20)
        Me.txtDerivativeDelta.TabIndex = 23
        Me.txtDerivativeDelta.Text = "0.000000"
        Me.txtDerivativeDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDerivativeDelta.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblDerivativeDelta
        '
        Me.lblDerivativeDelta.AutoSize = True
        Me.lblDerivativeDelta.Location = New System.Drawing.Point(9, 22)
        Me.lblDerivativeDelta.Name = "lblDerivativeDelta"
        Me.lblDerivativeDelta.Size = New System.Drawing.Size(82, 13)
        Me.lblDerivativeDelta.TabIndex = 22
        Me.lblDerivativeDelta.Text = "derivative delta:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 13)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "change of Chi^2:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "max. iterations:"
        '
        'cFitProcedureSettingsPanel_LMA
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "cFitProcedureSettingsPanel_LMA"
        Me.Size = New System.Drawing.Size(225, 248)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudMaxIterations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDerivativeDelta As SpectroscopyManager.NumericTextbox
    Friend WithEvents lblDerivativeDelta As System.Windows.Forms.Label
    Friend WithEvents lblCenteredDiffDescription As System.Windows.Forms.Label
    Friend WithEvents ckbUseCenteredDifferentiation As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
