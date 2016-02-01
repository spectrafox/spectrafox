<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_IETS_SpinExcitation
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
        Me.fpIETS_D = New SpectroscopyManager.mFitParameter()
        Me.fpLinearSlope = New SpectroscopyManager.mFitParameter()
        Me.fpAmplitude = New SpectroscopyManager.mFitParameter()
        Me.fpXCenter = New SpectroscopyManager.mFitParameter()
        Me.fpYOffset = New SpectroscopyManager.mFitParameter()
        Me.gbGeneralFitParameters = New System.Windows.Forms.GroupBox()
        Me.btnSelectAmplitude = New System.Windows.Forms.Button()
        Me.btnSelectXCenter = New System.Windows.Forms.Button()
        Me.btnSelectYOffset = New System.Windows.Forms.Button()
        Me.gbIETSSetup = New System.Windows.Forms.GroupBox()
        Me.cbSignalType = New System.Windows.Forms.ComboBox()
        Me.cbSpin = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbIETSParameters = New System.Windows.Forms.GroupBox()
        Me.fpIETS_gFactor = New SpectroscopyManager.mFitParameter()
        Me.fpTemperature = New SpectroscopyManager.mFitParameter()
        Me.fpIETS_E = New SpectroscopyManager.mFitParameter()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.fpMagneticFieldAnglePhi = New SpectroscopyManager.mFitParameter()
        Me.fpMagneticFieldAngleTheta = New SpectroscopyManager.mFitParameter()
        Me.fpMagneticField = New SpectroscopyManager.mFitParameter()
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
        Me.txtSettings_ConvolutionIntegrationStepSize = New SpectroscopyManager.NumericTextbox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.gbGeneralFitParameters.SuspendLayout()
        Me.gbIETSSetup.SuspendLayout()
        Me.gbIETSParameters.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gbFitFunctionSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'fpIETS_D
        '
        Me.fpIETS_D.DecimalValue = 0R
        Me.fpIETS_D.Location = New System.Drawing.Point(6, 19)
        Me.fpIETS_D.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpIETS_D.Name = "fpIETS_D"
        Me.fpIETS_D.Size = New System.Drawing.Size(296, 26)
        Me.fpIETS_D.TabIndex = 30
        Me.fpIETS_D.Value = 0R
        '
        'fpLinearSlope
        '
        Me.fpLinearSlope.DecimalValue = 0R
        Me.fpLinearSlope.Location = New System.Drawing.Point(6, 92)
        Me.fpLinearSlope.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpLinearSlope.Name = "fpLinearSlope"
        Me.fpLinearSlope.Size = New System.Drawing.Size(296, 26)
        Me.fpLinearSlope.TabIndex = 26
        Me.fpLinearSlope.Value = 0R
        '
        'fpAmplitude
        '
        Me.fpAmplitude.DecimalValue = 0R
        Me.fpAmplitude.Location = New System.Drawing.Point(6, 68)
        Me.fpAmplitude.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpAmplitude.Name = "fpAmplitude"
        Me.fpAmplitude.Size = New System.Drawing.Size(296, 26)
        Me.fpAmplitude.TabIndex = 24
        Me.fpAmplitude.Value = 0R
        '
        'fpXCenter
        '
        Me.fpXCenter.DecimalValue = 0R
        Me.fpXCenter.Location = New System.Drawing.Point(6, 43)
        Me.fpXCenter.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpXCenter.Name = "fpXCenter"
        Me.fpXCenter.Size = New System.Drawing.Size(296, 26)
        Me.fpXCenter.TabIndex = 22
        Me.fpXCenter.Value = 0R
        '
        'fpYOffset
        '
        Me.fpYOffset.DecimalValue = 0R
        Me.fpYOffset.Location = New System.Drawing.Point(6, 19)
        Me.fpYOffset.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpYOffset.Name = "fpYOffset"
        Me.fpYOffset.Size = New System.Drawing.Size(296, 26)
        Me.fpYOffset.TabIndex = 20
        Me.fpYOffset.Value = 0R
        '
        'gbGeneralFitParameters
        '
        Me.gbGeneralFitParameters.Controls.Add(Me.btnSelectAmplitude)
        Me.gbGeneralFitParameters.Controls.Add(Me.btnSelectXCenter)
        Me.gbGeneralFitParameters.Controls.Add(Me.btnSelectYOffset)
        Me.gbGeneralFitParameters.Controls.Add(Me.fpYOffset)
        Me.gbGeneralFitParameters.Controls.Add(Me.fpXCenter)
        Me.gbGeneralFitParameters.Controls.Add(Me.fpAmplitude)
        Me.gbGeneralFitParameters.Controls.Add(Me.fpLinearSlope)
        Me.gbGeneralFitParameters.Location = New System.Drawing.Point(262, 179)
        Me.gbGeneralFitParameters.Name = "gbGeneralFitParameters"
        Me.gbGeneralFitParameters.Size = New System.Drawing.Size(356, 124)
        Me.gbGeneralFitParameters.TabIndex = 28
        Me.gbGeneralFitParameters.TabStop = False
        Me.gbGeneralFitParameters.Text = "general fit parameters"
        '
        'btnSelectAmplitude
        '
        Me.btnSelectAmplitude.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectAmplitude.Location = New System.Drawing.Point(308, 72)
        Me.btnSelectAmplitude.Name = "btnSelectAmplitude"
        Me.btnSelectAmplitude.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectAmplitude.TabIndex = 25
        Me.btnSelectAmplitude.TabStop = False
        Me.btnSelectAmplitude.UseVisualStyleBackColor = True
        '
        'btnSelectXCenter
        '
        Me.btnSelectXCenter.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectXCenter.Location = New System.Drawing.Point(308, 48)
        Me.btnSelectXCenter.Name = "btnSelectXCenter"
        Me.btnSelectXCenter.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectXCenter.TabIndex = 23
        Me.btnSelectXCenter.TabStop = False
        Me.btnSelectXCenter.UseVisualStyleBackColor = True
        '
        'btnSelectYOffset
        '
        Me.btnSelectYOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectYOffset.Location = New System.Drawing.Point(308, 22)
        Me.btnSelectYOffset.Name = "btnSelectYOffset"
        Me.btnSelectYOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectYOffset.TabIndex = 21
        Me.btnSelectYOffset.TabStop = False
        Me.btnSelectYOffset.UseVisualStyleBackColor = True
        '
        'gbIETSSetup
        '
        Me.gbIETSSetup.Controls.Add(Me.cbSignalType)
        Me.gbIETSSetup.Controls.Add(Me.cbSpin)
        Me.gbIETSSetup.Controls.Add(Me.Label2)
        Me.gbIETSSetup.Controls.Add(Me.Label1)
        Me.gbIETSSetup.Location = New System.Drawing.Point(624, 4)
        Me.gbIETSSetup.Name = "gbIETSSetup"
        Me.gbIETSSetup.Size = New System.Drawing.Size(311, 51)
        Me.gbIETSSetup.TabIndex = 29
        Me.gbIETSSetup.TabStop = False
        Me.gbIETSSetup.Text = "IETS settings"
        '
        'cbSignalType
        '
        Me.cbSignalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSignalType.FormattingEnabled = True
        Me.cbSignalType.Location = New System.Drawing.Point(203, 18)
        Me.cbSignalType.Name = "cbSignalType"
        Me.cbSignalType.Size = New System.Drawing.Size(64, 21)
        Me.cbSignalType.TabIndex = 11
        '
        'cbSpin
        '
        Me.cbSpin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSpin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbSpin.FormattingEnabled = True
        Me.cbSpin.Location = New System.Drawing.Point(46, 17)
        Me.cbSpin.Name = "cbSpin"
        Me.cbSpin.Size = New System.Drawing.Size(49, 23)
        Me.cbSpin.TabIndex = 10
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(114, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "calculated signal:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "spin:"
        '
        'gbIETSParameters
        '
        Me.gbIETSParameters.Controls.Add(Me.fpIETS_gFactor)
        Me.gbIETSParameters.Controls.Add(Me.fpTemperature)
        Me.gbIETSParameters.Controls.Add(Me.fpIETS_E)
        Me.gbIETSParameters.Controls.Add(Me.fpIETS_D)
        Me.gbIETSParameters.Location = New System.Drawing.Point(624, 60)
        Me.gbIETSParameters.Name = "gbIETSParameters"
        Me.gbIETSParameters.Size = New System.Drawing.Size(311, 130)
        Me.gbIETSParameters.TabIndex = 30
        Me.gbIETSParameters.TabStop = False
        Me.gbIETSParameters.Text = "IETS parameters"
        '
        'fpIETS_gFactor
        '
        Me.fpIETS_gFactor.DecimalValue = 0R
        Me.fpIETS_gFactor.Location = New System.Drawing.Point(6, 71)
        Me.fpIETS_gFactor.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpIETS_gFactor.Name = "fpIETS_gFactor"
        Me.fpIETS_gFactor.Size = New System.Drawing.Size(296, 26)
        Me.fpIETS_gFactor.TabIndex = 32
        Me.fpIETS_gFactor.Value = 0R
        '
        'fpTemperature
        '
        Me.fpTemperature.DecimalValue = 0R
        Me.fpTemperature.Location = New System.Drawing.Point(6, 98)
        Me.fpTemperature.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpTemperature.Name = "fpTemperature"
        Me.fpTemperature.Size = New System.Drawing.Size(296, 26)
        Me.fpTemperature.TabIndex = 33
        Me.fpTemperature.Value = 0R
        '
        'fpIETS_E
        '
        Me.fpIETS_E.DecimalValue = 0R
        Me.fpIETS_E.Location = New System.Drawing.Point(6, 45)
        Me.fpIETS_E.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpIETS_E.Name = "fpIETS_E"
        Me.fpIETS_E.Size = New System.Drawing.Size(296, 26)
        Me.fpIETS_E.TabIndex = 31
        Me.fpIETS_E.Value = 0R
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.fpMagneticFieldAnglePhi)
        Me.GroupBox1.Controls.Add(Me.fpMagneticFieldAngleTheta)
        Me.GroupBox1.Controls.Add(Me.fpMagneticField)
        Me.GroupBox1.Location = New System.Drawing.Point(624, 196)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(311, 107)
        Me.GroupBox1.TabIndex = 31
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "magnetic field"
        '
        'fpMagneticFieldAnglePhi
        '
        Me.fpMagneticFieldAnglePhi.DecimalValue = 0R
        Me.fpMagneticFieldAnglePhi.Location = New System.Drawing.Point(6, 72)
        Me.fpMagneticFieldAnglePhi.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpMagneticFieldAnglePhi.Name = "fpMagneticFieldAnglePhi"
        Me.fpMagneticFieldAnglePhi.Size = New System.Drawing.Size(296, 26)
        Me.fpMagneticFieldAnglePhi.TabIndex = 42
        Me.fpMagneticFieldAnglePhi.Value = 0R
        '
        'fpMagneticFieldAngleTheta
        '
        Me.fpMagneticFieldAngleTheta.DecimalValue = 0R
        Me.fpMagneticFieldAngleTheta.Location = New System.Drawing.Point(6, 45)
        Me.fpMagneticFieldAngleTheta.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpMagneticFieldAngleTheta.Name = "fpMagneticFieldAngleTheta"
        Me.fpMagneticFieldAngleTheta.Size = New System.Drawing.Size(296, 26)
        Me.fpMagneticFieldAngleTheta.TabIndex = 41
        Me.fpMagneticFieldAngleTheta.Value = 0R
        '
        'fpMagneticField
        '
        Me.fpMagneticField.DecimalValue = 0R
        Me.fpMagneticField.Location = New System.Drawing.Point(6, 19)
        Me.fpMagneticField.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpMagneticField.Name = "fpMagneticField"
        Me.fpMagneticField.Size = New System.Drawing.Size(296, 26)
        Me.fpMagneticField.TabIndex = 24
        Me.fpMagneticField.Value = 0R
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
        Me.gbFitFunctionSettings.Controls.Add(Me.txtSettings_ConvolutionIntegrationStepSize)
        Me.gbFitFunctionSettings.Controls.Add(Me.Label9)
        Me.gbFitFunctionSettings.Location = New System.Drawing.Point(262, 3)
        Me.gbFitFunctionSettings.Name = "gbFitFunctionSettings"
        Me.gbFitFunctionSettings.Size = New System.Drawing.Size(294, 170)
        Me.gbFitFunctionSettings.TabIndex = 32
        Me.gbFitFunctionSettings.TabStop = False
        Me.gbFitFunctionSettings.Text = "fit function settings"
        '
        'txtSettings_CalculateValuesBiasInterpolationStepWidth
        '
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.AllowZero = True
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.BackColor = System.Drawing.Color.White
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.FormatDecimalPlaces = 6
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Location = New System.Drawing.Point(197, 141)
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Name = "txtSettings_CalculateValuesBiasInterpolationStepWidth"
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Size = New System.Drawing.Size(90, 20)
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.TabIndex = 6
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.Text = "0.000000"
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 144)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(170, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "interpolation steps between points:"
        '
        'txtSettings_CalculateValuesBiasLowerRange
        '
        Me.txtSettings_CalculateValuesBiasLowerRange.AllowZero = True
        Me.txtSettings_CalculateValuesBiasLowerRange.BackColor = System.Drawing.Color.White
        Me.txtSettings_CalculateValuesBiasLowerRange.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_CalculateValuesBiasLowerRange.FormatDecimalPlaces = 6
        Me.txtSettings_CalculateValuesBiasLowerRange.Location = New System.Drawing.Point(197, 119)
        Me.txtSettings_CalculateValuesBiasLowerRange.Name = "txtSettings_CalculateValuesBiasLowerRange"
        Me.txtSettings_CalculateValuesBiasLowerRange.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_CalculateValuesBiasLowerRange.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_CalculateValuesBiasLowerRange.Size = New System.Drawing.Size(90, 20)
        Me.txtSettings_CalculateValuesBiasLowerRange.TabIndex = 5
        Me.txtSettings_CalculateValuesBiasLowerRange.Text = "0.000000"
        Me.txtSettings_CalculateValuesBiasLowerRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_CalculateValuesBiasLowerRange.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 122)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(179, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "calculate values in bias range (from):"
        '
        'txtSettings_ConvolutionIntegralE_NEG
        '
        Me.txtSettings_ConvolutionIntegralE_NEG.AllowZero = True
        Me.txtSettings_ConvolutionIntegralE_NEG.BackColor = System.Drawing.Color.White
        Me.txtSettings_ConvolutionIntegralE_NEG.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_ConvolutionIntegralE_NEG.FormatDecimalPlaces = 6
        Me.txtSettings_ConvolutionIntegralE_NEG.Location = New System.Drawing.Point(197, 67)
        Me.txtSettings_ConvolutionIntegralE_NEG.Name = "txtSettings_ConvolutionIntegralE_NEG"
        Me.txtSettings_ConvolutionIntegralE_NEG.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_ConvolutionIntegralE_NEG.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_ConvolutionIntegralE_NEG.Size = New System.Drawing.Size(90, 20)
        Me.txtSettings_ConvolutionIntegralE_NEG.TabIndex = 3
        Me.txtSettings_ConvolutionIntegralE_NEG.Text = "0.000000"
        Me.txtSettings_ConvolutionIntegralE_NEG.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_ConvolutionIntegralE_NEG.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtSettings_CalculateValuesBiasUpperRange
        '
        Me.txtSettings_CalculateValuesBiasUpperRange.AllowZero = True
        Me.txtSettings_CalculateValuesBiasUpperRange.BackColor = System.Drawing.Color.White
        Me.txtSettings_CalculateValuesBiasUpperRange.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_CalculateValuesBiasUpperRange.FormatDecimalPlaces = 6
        Me.txtSettings_CalculateValuesBiasUpperRange.Location = New System.Drawing.Point(197, 97)
        Me.txtSettings_CalculateValuesBiasUpperRange.Name = "txtSettings_CalculateValuesBiasUpperRange"
        Me.txtSettings_CalculateValuesBiasUpperRange.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_CalculateValuesBiasUpperRange.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_CalculateValuesBiasUpperRange.Size = New System.Drawing.Size(90, 20)
        Me.txtSettings_CalculateValuesBiasUpperRange.TabIndex = 4
        Me.txtSettings_CalculateValuesBiasUpperRange.Text = "0.000000"
        Me.txtSettings_CalculateValuesBiasUpperRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_CalculateValuesBiasUpperRange.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(156, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "convolution integral energy min:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(168, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "calculate values in bias range (to):"
        '
        'txtSettings_ConvolutionIntegralE_POS
        '
        Me.txtSettings_ConvolutionIntegralE_POS.AllowZero = True
        Me.txtSettings_ConvolutionIntegralE_POS.BackColor = System.Drawing.Color.White
        Me.txtSettings_ConvolutionIntegralE_POS.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_ConvolutionIntegralE_POS.FormatDecimalPlaces = 6
        Me.txtSettings_ConvolutionIntegralE_POS.Location = New System.Drawing.Point(197, 45)
        Me.txtSettings_ConvolutionIntegralE_POS.Name = "txtSettings_ConvolutionIntegralE_POS"
        Me.txtSettings_ConvolutionIntegralE_POS.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_ConvolutionIntegralE_POS.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSettings_ConvolutionIntegralE_POS.Size = New System.Drawing.Size(90, 20)
        Me.txtSettings_ConvolutionIntegralE_POS.TabIndex = 2
        Me.txtSettings_ConvolutionIntegralE_POS.Text = "0.000000"
        Me.txtSettings_ConvolutionIntegralE_POS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_ConvolutionIntegralE_POS.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(159, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "convolution integral energy max:"
        '
        'txtSettings_ConvolutionIntegrationStepSize
        '
        Me.txtSettings_ConvolutionIntegrationStepSize.AllowZero = True
        Me.txtSettings_ConvolutionIntegrationStepSize.BackColor = System.Drawing.Color.White
        Me.txtSettings_ConvolutionIntegrationStepSize.ForeColor = System.Drawing.Color.Black
        Me.txtSettings_ConvolutionIntegrationStepSize.FormatDecimalPlaces = 6
        Me.txtSettings_ConvolutionIntegrationStepSize.Location = New System.Drawing.Point(197, 17)
        Me.txtSettings_ConvolutionIntegrationStepSize.Name = "txtSettings_ConvolutionIntegrationStepSize"
        Me.txtSettings_ConvolutionIntegrationStepSize.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSettings_ConvolutionIntegrationStepSize.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtSettings_ConvolutionIntegrationStepSize.Size = New System.Drawing.Size(90, 20)
        Me.txtSettings_ConvolutionIntegrationStepSize.TabIndex = 1
        Me.txtSettings_ConvolutionIntegrationStepSize.Text = "0.000000"
        Me.txtSettings_ConvolutionIntegrationStepSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettings_ConvolutionIntegrationStepSize.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 20)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(145, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "energy integration step-width:"
        '
        'cFitSettingPanel_IETS_SpinExcitation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbFitFunctionSettings)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbIETSSetup)
        Me.Controls.Add(Me.gbGeneralFitParameters)
        Me.Controls.Add(Me.gbIETSParameters)
        Me.Name = "cFitSettingPanel_IETS_SpinExcitation"
        Me.Size = New System.Drawing.Size(939, 312)
        Me.Controls.SetChildIndex(Me.gbIETSParameters, 0)
        Me.Controls.SetChildIndex(Me.gbGeneralFitParameters, 0)
        Me.Controls.SetChildIndex(Me.gbIETSSetup, 0)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.Controls.SetChildIndex(Me.gbFitFunctionSettings, 0)
        Me.gbGeneralFitParameters.ResumeLayout(False)
        Me.gbIETSSetup.ResumeLayout(False)
        Me.gbIETSSetup.PerformLayout()
        Me.gbIETSParameters.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.gbFitFunctionSettings.ResumeLayout(False)
        Me.gbFitFunctionSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fpIETS_D As SpectroscopyManager.mFitParameter
    Friend WithEvents fpLinearSlope As SpectroscopyManager.mFitParameter
    Friend WithEvents fpAmplitude As SpectroscopyManager.mFitParameter
    Friend WithEvents fpXCenter As SpectroscopyManager.mFitParameter
    Friend WithEvents fpYOffset As SpectroscopyManager.mFitParameter
    Friend WithEvents gbGeneralFitParameters As System.Windows.Forms.GroupBox
    Friend WithEvents gbIETSSetup As System.Windows.Forms.GroupBox
    Friend WithEvents cbSpin As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gbIETSParameters As System.Windows.Forms.GroupBox
    Friend WithEvents fpTemperature As SpectroscopyManager.mFitParameter
    Friend WithEvents fpIETS_E As SpectroscopyManager.mFitParameter
    Friend WithEvents btnSelectAmplitude As System.Windows.Forms.Button
    Friend WithEvents btnSelectXCenter As System.Windows.Forms.Button
    Friend WithEvents btnSelectYOffset As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents fpMagneticFieldAngleTheta As SpectroscopyManager.mFitParameter
    Friend WithEvents fpMagneticField As SpectroscopyManager.mFitParameter
    Friend WithEvents fpIETS_gFactor As SpectroscopyManager.mFitParameter
    Friend WithEvents gbFitFunctionSettings As System.Windows.Forms.GroupBox
    Friend WithEvents txtSettings_CalculateValuesBiasInterpolationStepWidth As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_CalculateValuesBiasLowerRange As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_ConvolutionIntegralE_NEG As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtSettings_CalculateValuesBiasUpperRange As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_ConvolutionIntegralE_POS As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtSettings_ConvolutionIntegrationStepSize As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbSignalType As System.Windows.Forms.ComboBox
    Friend WithEvents fpMagneticFieldAnglePhi As SpectroscopyManager.mFitParameter

End Class
