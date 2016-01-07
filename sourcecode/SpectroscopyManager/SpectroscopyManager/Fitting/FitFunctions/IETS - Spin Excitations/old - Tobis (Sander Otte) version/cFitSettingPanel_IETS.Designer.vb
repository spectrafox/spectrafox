<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_IETS
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
        Me.txtIntegrationStepSize = New SpectroscopyManager.NumericTextbox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbSpin = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbIETSParameters = New System.Windows.Forms.GroupBox()
        Me.fpIETS_gFactor = New SpectroscopyManager.mFitParameter()
        Me.fpTemperature = New SpectroscopyManager.mFitParameter()
        Me.fpIETS_E = New SpectroscopyManager.mFitParameter()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.fpMagneticFieldAngle = New SpectroscopyManager.mFitParameter()
        Me.fpMagneticField = New SpectroscopyManager.mFitParameter()
        Me.gbGeneralFitParameters.SuspendLayout()
        Me.gbIETSSetup.SuspendLayout()
        Me.gbIETSParameters.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'fpIETS_D
        '
        Me.fpIETS_D.DecimalValue = 0.0R
        Me.fpIETS_D.Location = New System.Drawing.Point(6, 19)
        Me.fpIETS_D.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpIETS_D.Name = "fpIETS_D"
        Me.fpIETS_D.Size = New System.Drawing.Size(296, 26)
        Me.fpIETS_D.TabIndex = 24
        Me.fpIETS_D.Value = 0.0R
        '
        'fpLinearSlope
        '
        Me.fpLinearSlope.DecimalValue = 0.0R
        Me.fpLinearSlope.Location = New System.Drawing.Point(6, 92)
        Me.fpLinearSlope.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpLinearSlope.Name = "fpLinearSlope"
        Me.fpLinearSlope.Size = New System.Drawing.Size(296, 26)
        Me.fpLinearSlope.TabIndex = 25
        Me.fpLinearSlope.Value = 0.0R
        '
        'fpAmplitude
        '
        Me.fpAmplitude.DecimalValue = 0.0R
        Me.fpAmplitude.Location = New System.Drawing.Point(6, 68)
        Me.fpAmplitude.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpAmplitude.Name = "fpAmplitude"
        Me.fpAmplitude.Size = New System.Drawing.Size(296, 26)
        Me.fpAmplitude.TabIndex = 26
        Me.fpAmplitude.Value = 0.0R
        '
        'fpXCenter
        '
        Me.fpXCenter.DecimalValue = 0.0R
        Me.fpXCenter.Location = New System.Drawing.Point(6, 43)
        Me.fpXCenter.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpXCenter.Name = "fpXCenter"
        Me.fpXCenter.Size = New System.Drawing.Size(296, 26)
        Me.fpXCenter.TabIndex = 23
        Me.fpXCenter.Value = 0.0R
        '
        'fpYOffset
        '
        Me.fpYOffset.DecimalValue = 0.0R
        Me.fpYOffset.Location = New System.Drawing.Point(6, 19)
        Me.fpYOffset.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpYOffset.Name = "fpYOffset"
        Me.fpYOffset.Size = New System.Drawing.Size(296, 26)
        Me.fpYOffset.TabIndex = 22
        Me.fpYOffset.Value = 0.0R
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
        Me.gbGeneralFitParameters.Location = New System.Drawing.Point(262, 3)
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
        Me.btnSelectAmplitude.TabIndex = 27
        Me.btnSelectAmplitude.TabStop = False
        Me.btnSelectAmplitude.UseVisualStyleBackColor = True
        '
        'btnSelectXCenter
        '
        Me.btnSelectXCenter.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectXCenter.Location = New System.Drawing.Point(308, 48)
        Me.btnSelectXCenter.Name = "btnSelectXCenter"
        Me.btnSelectXCenter.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectXCenter.TabIndex = 28
        Me.btnSelectXCenter.TabStop = False
        Me.btnSelectXCenter.UseVisualStyleBackColor = True
        '
        'btnSelectYOffset
        '
        Me.btnSelectYOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectYOffset.Location = New System.Drawing.Point(308, 22)
        Me.btnSelectYOffset.Name = "btnSelectYOffset"
        Me.btnSelectYOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectYOffset.TabIndex = 29
        Me.btnSelectYOffset.TabStop = False
        Me.btnSelectYOffset.UseVisualStyleBackColor = True
        '
        'gbIETSSetup
        '
        Me.gbIETSSetup.Controls.Add(Me.txtIntegrationStepSize)
        Me.gbIETSSetup.Controls.Add(Me.Label2)
        Me.gbIETSSetup.Controls.Add(Me.cbSpin)
        Me.gbIETSSetup.Controls.Add(Me.Label1)
        Me.gbIETSSetup.Location = New System.Drawing.Point(579, 133)
        Me.gbIETSSetup.Name = "gbIETSSetup"
        Me.gbIETSSetup.Size = New System.Drawing.Size(105, 97)
        Me.gbIETSSetup.TabIndex = 29
        Me.gbIETSSetup.TabStop = False
        Me.gbIETSSetup.Text = "IETS settings"
        '
        'txtIntegrationStepSize
        '
        Me.txtIntegrationStepSize.BackColor = System.Drawing.Color.White
        Me.txtIntegrationStepSize.ForeColor = System.Drawing.Color.Black
        Me.txtIntegrationStepSize.FormatDecimalPlaces = 6
        Me.txtIntegrationStepSize.Location = New System.Drawing.Point(22, 66)
        Me.txtIntegrationStepSize.Name = "txtIntegrationStepSize"
        Me.txtIntegrationStepSize.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtIntegrationStepSize.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtIntegrationStepSize.Size = New System.Drawing.Size(76, 20)
        Me.txtIntegrationStepSize.TabIndex = 31
        Me.txtIntegrationStepSize.Text = "0.000000"
        Me.txtIntegrationStepSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtIntegrationStepSize.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 30
        Me.Label2.Text = "integration steps:"
        '
        'cbSpin
        '
        Me.cbSpin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSpin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbSpin.FormattingEnabled = True
        Me.cbSpin.Location = New System.Drawing.Point(46, 17)
        Me.cbSpin.Name = "cbSpin"
        Me.cbSpin.Size = New System.Drawing.Size(49, 23)
        Me.cbSpin.TabIndex = 29
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
        Me.gbIETSParameters.Location = New System.Drawing.Point(262, 133)
        Me.gbIETSParameters.Name = "gbIETSParameters"
        Me.gbIETSParameters.Size = New System.Drawing.Size(311, 130)
        Me.gbIETSParameters.TabIndex = 30
        Me.gbIETSParameters.TabStop = False
        Me.gbIETSParameters.Text = "IETS parameters"
        '
        'fpIETS_gFactor
        '
        Me.fpIETS_gFactor.DecimalValue = 0.0R
        Me.fpIETS_gFactor.Location = New System.Drawing.Point(6, 71)
        Me.fpIETS_gFactor.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpIETS_gFactor.Name = "fpIETS_gFactor"
        Me.fpIETS_gFactor.Size = New System.Drawing.Size(296, 26)
        Me.fpIETS_gFactor.TabIndex = 24
        Me.fpIETS_gFactor.Value = 0.0R
        '
        'fpTemperature
        '
        Me.fpTemperature.DecimalValue = 0.0R
        Me.fpTemperature.Location = New System.Drawing.Point(6, 98)
        Me.fpTemperature.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpTemperature.Name = "fpTemperature"
        Me.fpTemperature.Size = New System.Drawing.Size(296, 26)
        Me.fpTemperature.TabIndex = 24
        Me.fpTemperature.Value = 0.0R
        '
        'fpIETS_E
        '
        Me.fpIETS_E.DecimalValue = 0.0R
        Me.fpIETS_E.Location = New System.Drawing.Point(6, 45)
        Me.fpIETS_E.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpIETS_E.Name = "fpIETS_E"
        Me.fpIETS_E.Size = New System.Drawing.Size(296, 26)
        Me.fpIETS_E.TabIndex = 24
        Me.fpIETS_E.Value = 0.0R
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.fpMagneticFieldAngle)
        Me.GroupBox1.Controls.Add(Me.fpMagneticField)
        Me.GroupBox1.Location = New System.Drawing.Point(262, 269)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(311, 79)
        Me.GroupBox1.TabIndex = 31
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "magnetic field"
        '
        'fpMagneticFieldAngle
        '
        Me.fpMagneticFieldAngle.DecimalValue = 0.0R
        Me.fpMagneticFieldAngle.Location = New System.Drawing.Point(6, 45)
        Me.fpMagneticFieldAngle.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpMagneticFieldAngle.Name = "fpMagneticFieldAngle"
        Me.fpMagneticFieldAngle.Size = New System.Drawing.Size(296, 26)
        Me.fpMagneticFieldAngle.TabIndex = 24
        Me.fpMagneticFieldAngle.Value = 0.0R
        '
        'fpMagneticField
        '
        Me.fpMagneticField.DecimalValue = 0.0R
        Me.fpMagneticField.Location = New System.Drawing.Point(6, 19)
        Me.fpMagneticField.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpMagneticField.Name = "fpMagneticField"
        Me.fpMagneticField.Size = New System.Drawing.Size(296, 26)
        Me.fpMagneticField.TabIndex = 24
        Me.fpMagneticField.Value = 0.0R
        '
        'cFitSettingPanel_IETS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbIETSSetup)
        Me.Controls.Add(Me.gbGeneralFitParameters)
        Me.Controls.Add(Me.gbIETSParameters)
        Me.Name = "cFitSettingPanel_IETS"
        Me.Size = New System.Drawing.Size(691, 359)
        Me.Controls.SetChildIndex(Me.gbIETSParameters, 0)
        Me.Controls.SetChildIndex(Me.gbGeneralFitParameters, 0)
        Me.Controls.SetChildIndex(Me.gbIETSSetup, 0)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.gbGeneralFitParameters.ResumeLayout(False)
        Me.gbIETSSetup.ResumeLayout(False)
        Me.gbIETSSetup.PerformLayout()
        Me.gbIETSParameters.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
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
    Friend WithEvents fpMagneticFieldAngle As SpectroscopyManager.mFitParameter
    Friend WithEvents fpMagneticField As SpectroscopyManager.mFitParameter
    Friend WithEvents fpIETS_gFactor As SpectroscopyManager.mFitParameter
    Friend WithEvents txtIntegrationStepSize As SpectroscopyManager.NumericTextbox
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
