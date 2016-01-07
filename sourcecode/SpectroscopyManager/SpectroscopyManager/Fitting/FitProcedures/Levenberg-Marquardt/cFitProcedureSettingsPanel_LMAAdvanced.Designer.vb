<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitProcedureSettingsPanel_LMAAdvanced
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtDerivativeDelta = New SpectroscopyManager.NumericTextbox()
        Me.lblDerivativeDelta = New System.Windows.Forms.Label()
        Me.gbStopConditions = New System.Windows.Forms.GroupBox()
        Me.txtMinChiSq = New SpectroscopyManager.NumericTextbox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudMaxIterations = New System.Windows.Forms.NumericUpDown()
        Me.lblDerivativeDeltaReference = New System.Windows.Forms.Label()
        Me.cboDerivativeCalculationMethod = New System.Windows.Forms.ComboBox()
        Me.GroupBox1.SuspendLayout()
        Me.gbStopConditions.SuspendLayout()
        CType(Me.nudMaxIterations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cboDerivativeCalculationMethod)
        Me.GroupBox1.Controls.Add(Me.txtDerivativeDelta)
        Me.GroupBox1.Controls.Add(Me.lblDerivativeDeltaReference)
        Me.GroupBox1.Controls.Add(Me.lblDerivativeDelta)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 86)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(213, 74)
        Me.GroupBox1.TabIndex = 27
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "numeric derivative calculation"
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
        'gbStopConditions
        '
        Me.gbStopConditions.Controls.Add(Me.txtMinChiSq)
        Me.gbStopConditions.Controls.Add(Me.Label3)
        Me.gbStopConditions.Controls.Add(Me.Label2)
        Me.gbStopConditions.Controls.Add(Me.nudMaxIterations)
        Me.gbStopConditions.Location = New System.Drawing.Point(3, 3)
        Me.gbStopConditions.Name = "gbStopConditions"
        Me.gbStopConditions.Size = New System.Drawing.Size(213, 77)
        Me.gbStopConditions.TabIndex = 26
        Me.gbStopConditions.TabStop = False
        Me.gbStopConditions.Text = "stop conditions of the algorithm"
        '
        'txtMinChiSq
        '
        Me.txtMinChiSq.BackColor = System.Drawing.Color.White
        Me.txtMinChiSq.ForeColor = System.Drawing.Color.Black
        Me.txtMinChiSq.FormatDecimalPlaces = 6
        Me.txtMinChiSq.Location = New System.Drawing.Point(103, 46)
        Me.txtMinChiSq.Name = "txtMinChiSq"
        Me.txtMinChiSq.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtMinChiSq.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtMinChiSq.Size = New System.Drawing.Size(99, 20)
        Me.txtMinChiSq.TabIndex = 23
        Me.txtMinChiSq.Text = "0.000000"
        Me.txtMinChiSq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMinChiSq.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
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
        'nudMaxIterations
        '
        Me.nudMaxIterations.Location = New System.Drawing.Point(103, 20)
        Me.nudMaxIterations.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMaxIterations.Name = "nudMaxIterations"
        Me.nudMaxIterations.Size = New System.Drawing.Size(56, 20)
        Me.nudMaxIterations.TabIndex = 21
        Me.nudMaxIterations.TabStop = False
        Me.nudMaxIterations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudMaxIterations.Value = New Decimal(New Integer() {500, 0, 0, 0})
        '
        'lblDerivativeDeltaReference
        '
        Me.lblDerivativeDeltaReference.AutoSize = True
        Me.lblDerivativeDeltaReference.Location = New System.Drawing.Point(9, 49)
        Me.lblDerivativeDeltaReference.Name = "lblDerivativeDeltaReference"
        Me.lblDerivativeDeltaReference.Size = New System.Drawing.Size(99, 13)
        Me.lblDerivativeDeltaReference.TabIndex = 22
        Me.lblDerivativeDeltaReference.Text = "calculation method:"
        '
        'cboDerivativeCalculationMethod
        '
        Me.cboDerivativeCalculationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDerivativeCalculationMethod.FormattingEnabled = True
        Me.cboDerivativeCalculationMethod.Location = New System.Drawing.Point(114, 45)
        Me.cboDerivativeCalculationMethod.Name = "cboDerivativeCalculationMethod"
        Me.cboDerivativeCalculationMethod.Size = New System.Drawing.Size(88, 21)
        Me.cboDerivativeCalculationMethod.TabIndex = 24
        '
        'cFitProcedureSettingsPanel_LMAAdvanced
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbStopConditions)
        Me.Name = "cFitProcedureSettingsPanel_LMAAdvanced"
        Me.Size = New System.Drawing.Size(224, 170)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbStopConditions.ResumeLayout(False)
        Me.gbStopConditions.PerformLayout()
        CType(Me.nudMaxIterations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDerivativeDelta As SpectroscopyManager.NumericTextbox
    Friend WithEvents lblDerivativeDelta As System.Windows.Forms.Label
    Friend WithEvents gbStopConditions As System.Windows.Forms.GroupBox
    Friend WithEvents txtMinChiSq As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudMaxIterations As System.Windows.Forms.NumericUpDown
    Friend WithEvents cboDerivativeCalculationMethod As System.Windows.Forms.ComboBox
    Friend WithEvents lblDerivativeDeltaReference As System.Windows.Forms.Label

End Class
