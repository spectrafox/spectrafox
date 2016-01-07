<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_BCSDoubleGap_SubGapPeaks
    Inherits SpectroscopyManager.cFitSettingPanel_SubGapPeaks

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
        Me.gbSampleParameters = New System.Windows.Forms.GroupBox()
        Me.fpSampleTemperature = New SpectroscopyManager.mFitParameter()
        Me.fpSampleGapRatio = New SpectroscopyManager.mFitParameter()
        Me.fpSampleGap2 = New SpectroscopyManager.mFitParameter()
        Me.fpSampleGap1 = New SpectroscopyManager.mFitParameter()
        Me.btnSelect_Sample_Gap2 = New System.Windows.Forms.Button()
        Me.btnSelect_Sample_Gap1 = New System.Windows.Forms.Button()
        Me.gbGeneralParameters = New System.Windows.Forms.GroupBox()
        Me.fpImaginaryDamping = New SpectroscopyManager.mFitParameter()
        Me.fpBroadeningWidth = New SpectroscopyManager.mFitParameter()
        Me.fpAmplitude = New SpectroscopyManager.mFitParameter()
        Me.fpYOffset = New SpectroscopyManager.mFitParameter()
        Me.fpXOffset = New SpectroscopyManager.mFitParameter()
        Me.btnSelect_BroadeningWidth = New System.Windows.Forms.Button()
        Me.btnSelect_XOffset = New System.Windows.Forms.Button()
        Me.btnSelect_YOffset = New System.Windows.Forms.Button()
        Me.gbTipParameters = New System.Windows.Forms.GroupBox()
        Me.fpTipTemperature = New SpectroscopyManager.mFitParameter()
        Me.fpTipGap = New SpectroscopyManager.mFitParameter()
        Me.gbSampleParameters.SuspendLayout()
        Me.gbGeneralParameters.SuspendLayout()
        Me.gbTipParameters.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbSampleParameters
        '
        Me.gbSampleParameters.Controls.Add(Me.fpSampleTemperature)
        Me.gbSampleParameters.Controls.Add(Me.fpSampleGapRatio)
        Me.gbSampleParameters.Controls.Add(Me.fpSampleGap2)
        Me.gbSampleParameters.Controls.Add(Me.fpSampleGap1)
        Me.gbSampleParameters.Controls.Add(Me.btnSelect_Sample_Gap2)
        Me.gbSampleParameters.Controls.Add(Me.btnSelect_Sample_Gap1)
        Me.gbSampleParameters.Location = New System.Drawing.Point(559, 152)
        Me.gbSampleParameters.Name = "gbSampleParameters"
        Me.gbSampleParameters.Size = New System.Drawing.Size(341, 116)
        Me.gbSampleParameters.TabIndex = 20
        Me.gbSampleParameters.TabStop = False
        Me.gbSampleParameters.Text = "Sample Parameters"
        '
        'fpSampleTemperature
        '
        Me.fpSampleTemperature.DecimalValue = 0.0R
        Me.fpSampleTemperature.Location = New System.Drawing.Point(6, 88)
        Me.fpSampleTemperature.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpSampleTemperature.Name = "fpSampleTemperature"
        Me.fpSampleTemperature.Size = New System.Drawing.Size(285, 26)
        Me.fpSampleTemperature.TabIndex = 21
        Me.fpSampleTemperature.Value = 0.0R
        '
        'fpSampleGapRatio
        '
        Me.fpSampleGapRatio.DecimalValue = 0.0R
        Me.fpSampleGapRatio.Location = New System.Drawing.Point(6, 63)
        Me.fpSampleGapRatio.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpSampleGapRatio.Name = "fpSampleGapRatio"
        Me.fpSampleGapRatio.Size = New System.Drawing.Size(285, 26)
        Me.fpSampleGapRatio.TabIndex = 22
        Me.fpSampleGapRatio.Value = 0.0R
        '
        'fpSampleGap2
        '
        Me.fpSampleGap2.DecimalValue = 0.0R
        Me.fpSampleGap2.Location = New System.Drawing.Point(6, 39)
        Me.fpSampleGap2.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpSampleGap2.Name = "fpSampleGap2"
        Me.fpSampleGap2.Size = New System.Drawing.Size(285, 26)
        Me.fpSampleGap2.TabIndex = 22
        Me.fpSampleGap2.Value = 0.0R
        '
        'fpSampleGap1
        '
        Me.fpSampleGap1.DecimalValue = 0.0R
        Me.fpSampleGap1.Location = New System.Drawing.Point(6, 15)
        Me.fpSampleGap1.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpSampleGap1.Name = "fpSampleGap1"
        Me.fpSampleGap1.Size = New System.Drawing.Size(285, 26)
        Me.fpSampleGap1.TabIndex = 23
        Me.fpSampleGap1.Value = 0.0R
        '
        'btnSelect_Sample_Gap2
        '
        Me.btnSelect_Sample_Gap2.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnSelect_Sample_Gap2.Location = New System.Drawing.Point(297, 42)
        Me.btnSelect_Sample_Gap2.Name = "btnSelect_Sample_Gap2"
        Me.btnSelect_Sample_Gap2.Size = New System.Drawing.Size(30, 19)
        Me.btnSelect_Sample_Gap2.TabIndex = 5
        Me.btnSelect_Sample_Gap2.TabStop = False
        Me.btnSelect_Sample_Gap2.UseVisualStyleBackColor = True
        '
        'btnSelect_Sample_Gap1
        '
        Me.btnSelect_Sample_Gap1.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnSelect_Sample_Gap1.Location = New System.Drawing.Point(297, 19)
        Me.btnSelect_Sample_Gap1.Name = "btnSelect_Sample_Gap1"
        Me.btnSelect_Sample_Gap1.Size = New System.Drawing.Size(30, 19)
        Me.btnSelect_Sample_Gap1.TabIndex = 5
        Me.btnSelect_Sample_Gap1.TabStop = False
        Me.btnSelect_Sample_Gap1.UseVisualStyleBackColor = True
        '
        'gbGeneralParameters
        '
        Me.gbGeneralParameters.Controls.Add(Me.fpImaginaryDamping)
        Me.gbGeneralParameters.Controls.Add(Me.fpBroadeningWidth)
        Me.gbGeneralParameters.Controls.Add(Me.fpAmplitude)
        Me.gbGeneralParameters.Controls.Add(Me.fpYOffset)
        Me.gbGeneralParameters.Controls.Add(Me.fpXOffset)
        Me.gbGeneralParameters.Controls.Add(Me.btnSelect_BroadeningWidth)
        Me.gbGeneralParameters.Controls.Add(Me.btnSelect_XOffset)
        Me.gbGeneralParameters.Controls.Add(Me.btnSelect_YOffset)
        Me.gbGeneralParameters.Location = New System.Drawing.Point(559, 3)
        Me.gbGeneralParameters.Name = "gbGeneralParameters"
        Me.gbGeneralParameters.Size = New System.Drawing.Size(341, 143)
        Me.gbGeneralParameters.TabIndex = 23
        Me.gbGeneralParameters.TabStop = False
        Me.gbGeneralParameters.Text = "general parameters"
        '
        'fpImaginaryDamping
        '
        Me.fpImaginaryDamping.DecimalValue = 0.0R
        Me.fpImaginaryDamping.Location = New System.Drawing.Point(6, 111)
        Me.fpImaginaryDamping.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpImaginaryDamping.Name = "fpImaginaryDamping"
        Me.fpImaginaryDamping.Size = New System.Drawing.Size(285, 26)
        Me.fpImaginaryDamping.TabIndex = 20
        Me.fpImaginaryDamping.Value = 0.0R
        '
        'fpBroadeningWidth
        '
        Me.fpBroadeningWidth.DecimalValue = 0.0R
        Me.fpBroadeningWidth.Location = New System.Drawing.Point(6, 86)
        Me.fpBroadeningWidth.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpBroadeningWidth.Name = "fpBroadeningWidth"
        Me.fpBroadeningWidth.Size = New System.Drawing.Size(285, 26)
        Me.fpBroadeningWidth.TabIndex = 20
        Me.fpBroadeningWidth.Value = 0.0R
        '
        'fpAmplitude
        '
        Me.fpAmplitude.DecimalValue = 0.0R
        Me.fpAmplitude.Location = New System.Drawing.Point(6, 61)
        Me.fpAmplitude.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpAmplitude.Name = "fpAmplitude"
        Me.fpAmplitude.Size = New System.Drawing.Size(285, 26)
        Me.fpAmplitude.TabIndex = 20
        Me.fpAmplitude.Value = 0.0R
        '
        'fpYOffset
        '
        Me.fpYOffset.DecimalValue = 0.0R
        Me.fpYOffset.Location = New System.Drawing.Point(6, 37)
        Me.fpYOffset.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpYOffset.Name = "fpYOffset"
        Me.fpYOffset.Size = New System.Drawing.Size(285, 26)
        Me.fpYOffset.TabIndex = 20
        Me.fpYOffset.Value = 0.0R
        '
        'fpXOffset
        '
        Me.fpXOffset.DecimalValue = 0.0R
        Me.fpXOffset.Location = New System.Drawing.Point(6, 14)
        Me.fpXOffset.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpXOffset.Name = "fpXOffset"
        Me.fpXOffset.Size = New System.Drawing.Size(285, 26)
        Me.fpXOffset.TabIndex = 20
        Me.fpXOffset.Value = 0.0R
        '
        'btnSelect_BroadeningWidth
        '
        Me.btnSelect_BroadeningWidth.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnSelect_BroadeningWidth.Location = New System.Drawing.Point(297, 89)
        Me.btnSelect_BroadeningWidth.Name = "btnSelect_BroadeningWidth"
        Me.btnSelect_BroadeningWidth.Size = New System.Drawing.Size(30, 19)
        Me.btnSelect_BroadeningWidth.TabIndex = 5
        Me.btnSelect_BroadeningWidth.TabStop = False
        Me.btnSelect_BroadeningWidth.UseVisualStyleBackColor = True
        '
        'btnSelect_XOffset
        '
        Me.btnSelect_XOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelect_XOffset.Location = New System.Drawing.Point(297, 18)
        Me.btnSelect_XOffset.Name = "btnSelect_XOffset"
        Me.btnSelect_XOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelect_XOffset.TabIndex = 5
        Me.btnSelect_XOffset.TabStop = False
        Me.btnSelect_XOffset.UseVisualStyleBackColor = True
        '
        'btnSelect_YOffset
        '
        Me.btnSelect_YOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelect_YOffset.Location = New System.Drawing.Point(297, 42)
        Me.btnSelect_YOffset.Name = "btnSelect_YOffset"
        Me.btnSelect_YOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelect_YOffset.TabIndex = 5
        Me.btnSelect_YOffset.TabStop = False
        Me.btnSelect_YOffset.UseVisualStyleBackColor = True
        '
        'gbTipParameters
        '
        Me.gbTipParameters.Controls.Add(Me.fpTipTemperature)
        Me.gbTipParameters.Controls.Add(Me.fpTipGap)
        Me.gbTipParameters.Location = New System.Drawing.Point(559, 274)
        Me.gbTipParameters.Name = "gbTipParameters"
        Me.gbTipParameters.Size = New System.Drawing.Size(341, 66)
        Me.gbTipParameters.TabIndex = 21
        Me.gbTipParameters.TabStop = False
        Me.gbTipParameters.Text = "tip parameters"
        '
        'fpTipTemperature
        '
        Me.fpTipTemperature.DecimalValue = 0.0R
        Me.fpTipTemperature.Location = New System.Drawing.Point(6, 36)
        Me.fpTipTemperature.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpTipTemperature.Name = "fpTipTemperature"
        Me.fpTipTemperature.Size = New System.Drawing.Size(285, 26)
        Me.fpTipTemperature.TabIndex = 20
        Me.fpTipTemperature.Value = 0.0R
        '
        'fpTipGap
        '
        Me.fpTipGap.DecimalValue = 0.0R
        Me.fpTipGap.Location = New System.Drawing.Point(6, 13)
        Me.fpTipGap.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpTipGap.Name = "fpTipGap"
        Me.fpTipGap.Size = New System.Drawing.Size(285, 26)
        Me.fpTipGap.TabIndex = 20
        Me.fpTipGap.Value = 0.0R
        '
        'cFitSettingPanel_BCSDoubleGap_SubGapPeaks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbTipParameters)
        Me.Controls.Add(Me.gbSampleParameters)
        Me.Controls.Add(Me.gbGeneralParameters)
        Me.Name = "cFitSettingPanel_BCSDoubleGap_SubGapPeaks"
        Me.Size = New System.Drawing.Size(908, 675)
        Me.Controls.SetChildIndex(Me.gbGeneralParameters, 0)
        Me.Controls.SetChildIndex(Me.gbSampleParameters, 0)
        Me.Controls.SetChildIndex(Me.gbTipParameters, 0)
        Me.gbSampleParameters.ResumeLayout(False)
        Me.gbGeneralParameters.ResumeLayout(False)
        Me.gbTipParameters.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbSampleParameters As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelect_Sample_Gap2 As System.Windows.Forms.Button
    Friend WithEvents btnSelect_Sample_Gap1 As System.Windows.Forms.Button
    Friend WithEvents gbGeneralParameters As System.Windows.Forms.GroupBox
    Friend WithEvents fpImaginaryDamping As SpectroscopyManager.mFitParameter
    Friend WithEvents fpBroadeningWidth As SpectroscopyManager.mFitParameter
    Friend WithEvents fpAmplitude As SpectroscopyManager.mFitParameter
    Friend WithEvents fpYOffset As SpectroscopyManager.mFitParameter
    Friend WithEvents fpXOffset As SpectroscopyManager.mFitParameter
    Friend WithEvents btnSelect_BroadeningWidth As System.Windows.Forms.Button
    Friend WithEvents btnSelect_XOffset As System.Windows.Forms.Button
    Friend WithEvents btnSelect_YOffset As System.Windows.Forms.Button
    Friend WithEvents gbTipParameters As System.Windows.Forms.GroupBox
    Friend WithEvents fpTipTemperature As SpectroscopyManager.mFitParameter
    Friend WithEvents fpTipGap As SpectroscopyManager.mFitParameter
    Friend WithEvents fpSampleTemperature As SpectroscopyManager.mFitParameter
    Friend WithEvents fpSampleGapRatio As SpectroscopyManager.mFitParameter
    Friend WithEvents fpSampleGap2 As SpectroscopyManager.mFitParameter
    Friend WithEvents fpSampleGap1 As SpectroscopyManager.mFitParameter

End Class
