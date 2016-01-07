<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_MetalTipSampleConvolution
    Inherits SpectroscopyManager.cFitSettingPanel

    'UserControl overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.gbFitFunctionSettings = New System.Windows.Forms.GroupBox()
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth = New SpectroscopyManager.NumericTextbox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSettings_CalculateValuesBiasLowerRange = New SpectroscopyManager.NumericTextbox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSettings_ConvolutionIntegralE_NEG = New SpectroscopyManager.NumericTextbox()
        Me.txtSettings_CalculateValuesBiasUpperRange = New SpectroscopyManager.NumericTextbox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSettings_ConvolutionIntegralE_POS = New SpectroscopyManager.NumericTextbox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSettings_BroadeningStepWidth = New SpectroscopyManager.NumericTextbox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSettings_ConvolutionIntegrationStepSize = New SpectroscopyManager.NumericTextbox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbFitFunctionSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbFitFunctionSettings
        '
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_CalculateValuesBiasInterpolationStepWidth)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label7)
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_CalculateValuesBiasLowerRange)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label6)
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_ConvolutionIntegralE_NEG)
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_CalculateValuesBiasUpperRange)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label4)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label5)
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_ConvolutionIntegralE_POS)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label3)
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_BroadeningStepWidth)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label2)
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_ConvolutionIntegrationStepSize)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label1)
        Me.gbFitFunctionSettings.Location = New System.Drawing.Point(259, 3)
        Me.gbFitFunctionSettings.Name = "gbFitFunctionSettings"
        Me.gbFitFunctionSettings.Size = New System.Drawing.Size(294, 190)
        Me.gbFitFunctionSettings.TabIndex = 22
        Me.gbFitFunctionSettings.TabStop = False
        Me.gbFitFunctionSettings.Text = "fit function settings"
        '
        'txtSettings_CalculateValuesBiasInterpolationStepWidth
        '
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.BackColor = System.Drawing.Color.White
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.FormatDecimalPlaces = 6
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Location = New System.Drawing.Point(198, 165)
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Name = "txtSettings_CalculateValuesBiasInterpolationStepWidth"
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Size = New System.Drawing.Size(89, 20)
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.TabIndex = 1
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Text = "0.000000"
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 168)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(170, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "interpolation steps between points:"
        '
        'txtSettings_CalculateValuesBiasLowerRange
        '
        Me.txtSettings_CalculateValuesBiasLowerRange.BackColor = System.Drawing.Color.White
        Me.txtSettings_CalculateValuesBiasLowerRange.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_CalculateValuesBiasLowerRange.FormatDecimalPlaces = 6
        Me.txtSettings_CalculateValuesBiasLowerRange.Location = New System.Drawing.Point(198, 143)
        Me.txtSettings_CalculateValuesBiasLowerRange.Name = "txtSettings_CalculateValuesBiasLowerRange"
        Me.txtSettings_CalculateValuesBiasLowerRange.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_CalculateValuesBiasLowerRange.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_CalculateValuesBiasLowerRange.Size = New System.Drawing.Size(89, 20)
        Me.txtSettings_CalculateValuesBiasLowerRange.TabIndex = 1
        Me.txtSettings_CalculateValuesBiasLowerRange.Text = "0.000000"
        Me.txtSettings_CalculateValuesBiasLowerRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_CalculateValuesBiasLowerRange.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 146)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(179, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "calculate values in bias range (from):"
        '
        'txtSettings_ConvolutionIntegralE_NEG
        '
        Me.txtSettings_ConvolutionIntegralE_NEG.BackColor = System.Drawing.Color.White
        Me.txtSettings_ConvolutionIntegralE_NEG.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_ConvolutionIntegralE_NEG.FormatDecimalPlaces = 6
        Me.txtSettings_ConvolutionIntegralE_NEG.Location = New System.Drawing.Point(198, 91)
        Me.txtSettings_ConvolutionIntegralE_NEG.Name = "txtSettings_ConvolutionIntegralE_NEG"
        Me.txtSettings_ConvolutionIntegralE_NEG.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_ConvolutionIntegralE_NEG.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_ConvolutionIntegralE_NEG.Size = New System.Drawing.Size(89, 20)
        Me.txtSettings_ConvolutionIntegralE_NEG.TabIndex = 1
        Me.txtSettings_ConvolutionIntegralE_NEG.Text = "0.000000"
        Me.txtSettings_ConvolutionIntegralE_NEG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_ConvolutionIntegralE_NEG.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtSettings_CalculateValuesBiasUpperRange
        '
        Me.txtSettings_CalculateValuesBiasUpperRange.BackColor = System.Drawing.Color.White
        Me.txtSettings_CalculateValuesBiasUpperRange.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_CalculateValuesBiasUpperRange.FormatDecimalPlaces = 6
        Me.txtSettings_CalculateValuesBiasUpperRange.Location = New System.Drawing.Point(198, 121)
        Me.txtSettings_CalculateValuesBiasUpperRange.Name = "txtSettings_CalculateValuesBiasUpperRange"
        Me.txtSettings_CalculateValuesBiasUpperRange.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_CalculateValuesBiasUpperRange.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_CalculateValuesBiasUpperRange.Size = New System.Drawing.Size(89, 20)
        Me.txtSettings_CalculateValuesBiasUpperRange.TabIndex = 1
        Me.txtSettings_CalculateValuesBiasUpperRange.Text = "0.000000"
        Me.txtSettings_CalculateValuesBiasUpperRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_CalculateValuesBiasUpperRange.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(156, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "convolution integral energy min:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 124)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(168, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "calculate values in bias range (to):"
        '
        'txtSettings_ConvolutionIntegralE_POS
        '
        Me.txtSettings_ConvolutionIntegralE_POS.BackColor = System.Drawing.Color.White
        Me.txtSettings_ConvolutionIntegralE_POS.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_ConvolutionIntegralE_POS.FormatDecimalPlaces = 6
        Me.txtSettings_ConvolutionIntegralE_POS.Location = New System.Drawing.Point(198, 69)
        Me.txtSettings_ConvolutionIntegralE_POS.Name = "txtSettings_ConvolutionIntegralE_POS"
        Me.txtSettings_ConvolutionIntegralE_POS.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_ConvolutionIntegralE_POS.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_ConvolutionIntegralE_POS.Size = New System.Drawing.Size(89, 20)
        Me.txtSettings_ConvolutionIntegralE_POS.TabIndex = 1
        Me.txtSettings_ConvolutionIntegralE_POS.Text = "0.000000"
        Me.txtSettings_ConvolutionIntegralE_POS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_ConvolutionIntegralE_POS.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(159, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "convolution integral energy max:"
        '
        'txtSettings_BroadeningStepWidth
        '
        Me.txtSettings_BroadeningStepWidth.BackColor = System.Drawing.Color.White
        Me.txtSettings_BroadeningStepWidth.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_BroadeningStepWidth.FormatDecimalPlaces = 6
        Me.txtSettings_BroadeningStepWidth.Location = New System.Drawing.Point(198, 40)
        Me.txtSettings_BroadeningStepWidth.Name = "txtSettings_BroadeningStepWidth"
        Me.txtSettings_BroadeningStepWidth.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_BroadeningStepWidth.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtSettings_BroadeningStepWidth.Size = New System.Drawing.Size(89, 20)
        Me.txtSettings_BroadeningStepWidth.TabIndex = 1
        Me.txtSettings_BroadeningStepWidth.Text = "0.000000"
        Me.txtSettings_BroadeningStepWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_BroadeningStepWidth.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(172, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "broadening convolution step-width:"
        '
        'txtSettings_ConvolutionIntegrationStepSize
        '
        Me.txtSettings_ConvolutionIntegrationStepSize.BackColor = System.Drawing.Color.White
        Me.txtSettings_ConvolutionIntegrationStepSize.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_ConvolutionIntegrationStepSize.FormatDecimalPlaces = 6
        Me.txtSettings_ConvolutionIntegrationStepSize.Location = New System.Drawing.Point(198, 17)
        Me.txtSettings_ConvolutionIntegrationStepSize.Name = "txtSettings_ConvolutionIntegrationStepSize"
        Me.txtSettings_ConvolutionIntegrationStepSize.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_ConvolutionIntegrationStepSize.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtSettings_ConvolutionIntegrationStepSize.Size = New System.Drawing.Size(89, 20)
        Me.txtSettings_ConvolutionIntegrationStepSize.TabIndex = 1
        Me.txtSettings_ConvolutionIntegrationStepSize.Text = "0.000000"
        Me.txtSettings_ConvolutionIntegrationStepSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_ConvolutionIntegrationStepSize.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "energy integration step-width:"
        '
        'cFitSettingPanel_MetalTipSampleConvolution
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbFitFunctionSettings)
        Me.Name = "cFitSettingPanel_MetalTipSampleConvolution"
        Me.Size = New System.Drawing.Size(761, 196)
        Me.Controls.SetChildIndex(Me.gbFitFunctionSettings, 0)
        Me.gbFitFunctionSettings.ResumeLayout(False)
        Me.gbFitFunctionSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbFitFunctionSettings As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_ConvolutionIntegrationStepSize As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtSettings_ConvolutionIntegralE_NEG As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_ConvolutionIntegralE_POS As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_BroadeningStepWidth As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_CalculateValuesBiasLowerRange As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_CalculateValuesBiasUpperRange As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_CalculateValuesBiasInterpolationStepWidth As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label7 As System.Windows.Forms.Label

End Class
