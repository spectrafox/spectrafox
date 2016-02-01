<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_IETS_SpinExcitation
    Inherits SpectroscopyManager.cFitSettingPanel_TipSampleConvolution

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
        Me.cbSpin = New System.Windows.Forms.ComboBox()
        Me.gbIETSParameters = New System.Windows.Forms.GroupBox()
        Me.fpIETS_gFactor = New SpectroscopyManager.mFitParameter()
        Me.fpTemperatureSample = New SpectroscopyManager.mFitParameter()
        Me.fpTemperatureTip = New SpectroscopyManager.mFitParameter()
        Me.fpIETS_E = New SpectroscopyManager.mFitParameter()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.fpMagneticFieldAnglePhi = New SpectroscopyManager.mFitParameter()
        Me.fpMagneticFieldAngleTheta = New SpectroscopyManager.mFitParameter()
        Me.fpMagneticField = New SpectroscopyManager.mFitParameter()
        Me.fpSystemBroadening = New SpectroscopyManager.mFitParameter()
        Me.gbGeneralFitParameters.SuspendLayout()
        Me.gbIETSSetup.SuspendLayout()
        Me.gbIETSParameters.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
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
        Me.gbGeneralFitParameters.Controls.Add(Me.fpSystemBroadening)
        Me.gbGeneralFitParameters.Controls.Add(Me.fpLinearSlope)
        Me.gbGeneralFitParameters.Location = New System.Drawing.Point(559, 3)
        Me.gbGeneralFitParameters.Name = "gbGeneralFitParameters"
        Me.gbGeneralFitParameters.Size = New System.Drawing.Size(380, 159)
        Me.gbGeneralFitParameters.TabIndex = 28
        Me.gbGeneralFitParameters.TabStop = False
        Me.gbGeneralFitParameters.Text = "general fit parameters"
        '
        'btnSelectAmplitude
        '
        Me.btnSelectAmplitude.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectAmplitude.Location = New System.Drawing.Point(314, 72)
        Me.btnSelectAmplitude.Name = "btnSelectAmplitude"
        Me.btnSelectAmplitude.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectAmplitude.TabIndex = 25
        Me.btnSelectAmplitude.TabStop = False
        Me.btnSelectAmplitude.UseVisualStyleBackColor = True
        '
        'btnSelectXCenter
        '
        Me.btnSelectXCenter.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectXCenter.Location = New System.Drawing.Point(314, 48)
        Me.btnSelectXCenter.Name = "btnSelectXCenter"
        Me.btnSelectXCenter.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectXCenter.TabIndex = 23
        Me.btnSelectXCenter.TabStop = False
        Me.btnSelectXCenter.UseVisualStyleBackColor = True
        '
        'btnSelectYOffset
        '
        Me.btnSelectYOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectYOffset.Location = New System.Drawing.Point(314, 22)
        Me.btnSelectYOffset.Name = "btnSelectYOffset"
        Me.btnSelectYOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectYOffset.TabIndex = 21
        Me.btnSelectYOffset.TabStop = False
        Me.btnSelectYOffset.UseVisualStyleBackColor = True
        '
        'gbIETSSetup
        '
        Me.gbIETSSetup.Controls.Add(Me.cbSpin)
        Me.gbIETSSetup.Location = New System.Drawing.Point(876, 168)
        Me.gbIETSSetup.Name = "gbIETSSetup"
        Me.gbIETSSetup.Size = New System.Drawing.Size(63, 51)
        Me.gbIETSSetup.TabIndex = 29
        Me.gbIETSSetup.TabStop = False
        Me.gbIETSSetup.Text = "spin:"
        '
        'cbSpin
        '
        Me.cbSpin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSpin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbSpin.FormattingEnabled = True
        Me.cbSpin.Location = New System.Drawing.Point(6, 19)
        Me.cbSpin.Name = "cbSpin"
        Me.cbSpin.Size = New System.Drawing.Size(49, 23)
        Me.cbSpin.TabIndex = 10
        '
        'gbIETSParameters
        '
        Me.gbIETSParameters.Controls.Add(Me.fpIETS_gFactor)
        Me.gbIETSParameters.Controls.Add(Me.fpTemperatureSample)
        Me.gbIETSParameters.Controls.Add(Me.fpTemperatureTip)
        Me.gbIETSParameters.Controls.Add(Me.fpIETS_E)
        Me.gbIETSParameters.Controls.Add(Me.fpIETS_D)
        Me.gbIETSParameters.Location = New System.Drawing.Point(559, 168)
        Me.gbIETSParameters.Name = "gbIETSParameters"
        Me.gbIETSParameters.Size = New System.Drawing.Size(311, 162)
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
        'fpTemperatureSample
        '
        Me.fpTemperatureSample.DecimalValue = 0R
        Me.fpTemperatureSample.Location = New System.Drawing.Point(6, 124)
        Me.fpTemperatureSample.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpTemperatureSample.Name = "fpTemperatureSample"
        Me.fpTemperatureSample.Size = New System.Drawing.Size(296, 26)
        Me.fpTemperatureSample.TabIndex = 34
        Me.fpTemperatureSample.Value = 0R
        '
        'fpTemperatureTip
        '
        Me.fpTemperatureTip.DecimalValue = 0R
        Me.fpTemperatureTip.Location = New System.Drawing.Point(6, 98)
        Me.fpTemperatureTip.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpTemperatureTip.Name = "fpTemperatureTip"
        Me.fpTemperatureTip.Size = New System.Drawing.Size(296, 26)
        Me.fpTemperatureTip.TabIndex = 33
        Me.fpTemperatureTip.Value = 0R
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
        Me.GroupBox1.Location = New System.Drawing.Point(559, 336)
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
        'fpSystemBroadening
        '
        Me.fpSystemBroadening.DecimalValue = 0R
        Me.fpSystemBroadening.Location = New System.Drawing.Point(6, 119)
        Me.fpSystemBroadening.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpSystemBroadening.Name = "fpSystemBroadening"
        Me.fpSystemBroadening.Size = New System.Drawing.Size(296, 26)
        Me.fpSystemBroadening.TabIndex = 27
        Me.fpSystemBroadening.Value = 0R
        '
        'cFitSettingPanel_IETS_SpinExcitation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbIETSSetup)
        Me.Controls.Add(Me.gbGeneralFitParameters)
        Me.Controls.Add(Me.gbIETSParameters)
        Me.Name = "cFitSettingPanel_IETS_SpinExcitation"
        Me.Size = New System.Drawing.Size(947, 454)
        Me.Controls.SetChildIndex(Me.gbIETSParameters, 0)
        Me.Controls.SetChildIndex(Me.gbGeneralFitParameters, 0)
        Me.Controls.SetChildIndex(Me.gbIETSSetup, 0)
        Me.Controls.SetChildIndex(Me.GroupBox1, 0)
        Me.gbGeneralFitParameters.ResumeLayout(False)
        Me.gbIETSSetup.ResumeLayout(False)
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
    Friend WithEvents gbIETSParameters As System.Windows.Forms.GroupBox
    Friend WithEvents fpTemperatureTip As SpectroscopyManager.mFitParameter
    Friend WithEvents fpIETS_E As SpectroscopyManager.mFitParameter
    Friend WithEvents btnSelectAmplitude As System.Windows.Forms.Button
    Friend WithEvents btnSelectXCenter As System.Windows.Forms.Button
    Friend WithEvents btnSelectYOffset As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents fpMagneticFieldAngleTheta As SpectroscopyManager.mFitParameter
    Friend WithEvents fpMagneticField As SpectroscopyManager.mFitParameter
    Friend WithEvents fpIETS_gFactor As SpectroscopyManager.mFitParameter
    Friend WithEvents fpMagneticFieldAnglePhi As SpectroscopyManager.mFitParameter
    Friend WithEvents fpTemperatureSample As mFitParameter
    Friend WithEvents fpSystemBroadening As mFitParameter
End Class
