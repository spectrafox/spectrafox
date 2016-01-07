<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitProcedureSettingsPanel_NMA
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
        Me.txtAnnealingTemperature = New SpectroscopyManager.NumericTextbox()
        Me.lblDerivativeDelta = New System.Windows.Forms.Label()
        Me.gbcSimulatedAnnealing = New SpectroscopyManager.GroupBoxCheckable()
        Me.rbSimulatedAnnealingCoolingType_Exponential = New System.Windows.Forms.RadioButton()
        Me.rbSimulatedAnnealingCoolingType_Linear = New System.Windows.Forms.RadioButton()
        Me.txtAnnealingSteps = New SpectroscopyManager.NumericTextbox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.gbcSimulatedAnnealing.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtAnnealingTemperature
        '
        Me.txtAnnealingTemperature.BackColor = System.Drawing.Color.White
        Me.txtAnnealingTemperature.ForeColor = System.Drawing.Color.Black
        Me.txtAnnealingTemperature.FormatDecimalPlaces = 6
        Me.txtAnnealingTemperature.Location = New System.Drawing.Point(103, 93)
        Me.txtAnnealingTemperature.Name = "txtAnnealingTemperature"
        Me.txtAnnealingTemperature.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtAnnealingTemperature.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtAnnealingTemperature.Size = New System.Drawing.Size(99, 20)
        Me.txtAnnealingTemperature.TabIndex = 23
        Me.txtAnnealingTemperature.Text = "0.000000"
        Me.txtAnnealingTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAnnealingTemperature.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblDerivativeDelta
        '
        Me.lblDerivativeDelta.AutoSize = True
        Me.lblDerivativeDelta.Location = New System.Drawing.Point(9, 96)
        Me.lblDerivativeDelta.Name = "lblDerivativeDelta"
        Me.lblDerivativeDelta.Size = New System.Drawing.Size(89, 13)
        Me.lblDerivativeDelta.TabIndex = 22
        Me.lblDerivativeDelta.Text = "start temperature:"
        '
        'gbcSimulatedAnnealing
        '
        Me.gbcSimulatedAnnealing.Checked = False
        Me.gbcSimulatedAnnealing.Controls.Add(Me.rbSimulatedAnnealingCoolingType_Exponential)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.rbSimulatedAnnealingCoolingType_Linear)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.txtAnnealingSteps)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.Label6)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.Label4)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.Label1)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.txtAnnealingTemperature)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.Label5)
        Me.gbcSimulatedAnnealing.Controls.Add(Me.lblDerivativeDelta)
        Me.gbcSimulatedAnnealing.Location = New System.Drawing.Point(3, 86)
        Me.gbcSimulatedAnnealing.Name = "gbcSimulatedAnnealing"
        Me.gbcSimulatedAnnealing.Size = New System.Drawing.Size(213, 197)
        Me.gbcSimulatedAnnealing.TabIndex = 24
        Me.gbcSimulatedAnnealing.TabStop = False
        Me.gbcSimulatedAnnealing.Text = "use simulated annealing"
        '
        'rbSimulatedAnnealingCoolingType_Exponential
        '
        Me.rbSimulatedAnnealingCoolingType_Exponential.AutoSize = True
        Me.rbSimulatedAnnealingCoolingType_Exponential.Location = New System.Drawing.Point(109, 166)
        Me.rbSimulatedAnnealingCoolingType_Exponential.Name = "rbSimulatedAnnealingCoolingType_Exponential"
        Me.rbSimulatedAnnealingCoolingType_Exponential.Size = New System.Drawing.Size(79, 17)
        Me.rbSimulatedAnnealingCoolingType_Exponential.TabIndex = 24
        Me.rbSimulatedAnnealingCoolingType_Exponential.TabStop = True
        Me.rbSimulatedAnnealingCoolingType_Exponential.Text = "exponential"
        Me.rbSimulatedAnnealingCoolingType_Exponential.UseVisualStyleBackColor = True
        '
        'rbSimulatedAnnealingCoolingType_Linear
        '
        Me.rbSimulatedAnnealingCoolingType_Linear.AutoSize = True
        Me.rbSimulatedAnnealingCoolingType_Linear.Location = New System.Drawing.Point(26, 166)
        Me.rbSimulatedAnnealingCoolingType_Linear.Name = "rbSimulatedAnnealingCoolingType_Linear"
        Me.rbSimulatedAnnealingCoolingType_Linear.Size = New System.Drawing.Size(50, 17)
        Me.rbSimulatedAnnealingCoolingType_Linear.TabIndex = 24
        Me.rbSimulatedAnnealingCoolingType_Linear.TabStop = True
        Me.rbSimulatedAnnealingCoolingType_Linear.Text = "linear"
        Me.rbSimulatedAnnealingCoolingType_Linear.UseVisualStyleBackColor = True
        '
        'txtAnnealingSteps
        '
        Me.txtAnnealingSteps.BackColor = System.Drawing.Color.White
        Me.txtAnnealingSteps.ForeColor = System.Drawing.Color.Black
        Me.txtAnnealingSteps.FormatDecimalPlaces = 0
        Me.txtAnnealingSteps.Location = New System.Drawing.Point(103, 119)
        Me.txtAnnealingSteps.Name = "txtAnnealingSteps"
        Me.txtAnnealingSteps.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtAnnealingSteps.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtAnnealingSteps.Size = New System.Drawing.Size(99, 20)
        Me.txtAnnealingSteps.TabIndex = 23
        Me.txtAnnealingSteps.Text = "0"
        Me.txtAnnealingSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAnnealingSteps.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.IntegerValue
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(5, 56)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(203, 26)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "- performs always the full number of steps!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "- results are less accurate!"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 148)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "cooling type:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 122)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "annealing steps:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(32, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(141, 30)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "activate to avoid" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "local extermal points"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cFitProcedureSettingsPanel_NMA
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbcSimulatedAnnealing)
        Me.Name = "cFitProcedureSettingsPanel_NMA"
        Me.Size = New System.Drawing.Size(223, 286)
        Me.gbcSimulatedAnnealing.ResumeLayout(False)
        Me.gbcSimulatedAnnealing.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtAnnealingTemperature As SpectroscopyManager.NumericTextbox
    Friend WithEvents lblDerivativeDelta As System.Windows.Forms.Label
    Friend WithEvents gbcSimulatedAnnealing As SpectroscopyManager.GroupBoxCheckable
    Friend WithEvents txtAnnealingSteps As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbSimulatedAnnealingCoolingType_Exponential As System.Windows.Forms.RadioButton
    Friend WithEvents rbSimulatedAnnealingCoolingType_Linear As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label

End Class
