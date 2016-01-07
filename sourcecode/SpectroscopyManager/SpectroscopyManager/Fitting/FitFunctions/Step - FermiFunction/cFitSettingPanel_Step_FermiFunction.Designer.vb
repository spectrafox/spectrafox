<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_Step_FermiFunction
    Inherits SpectroscopyManager.cFitSettingPanel

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
        Me.btnSelectYOffset = New System.Windows.Forms.Button()
        Me.btnSelectXCenter = New System.Windows.Forms.Button()
        Me.btnSelectAmplitude = New System.Windows.Forms.Button()
        Me.btnSelectWidth = New System.Windows.Forms.Button()
        Me.fpYOffset = New SpectroscopyManager.mFitParameter()
        Me.fpXCenter = New SpectroscopyManager.mFitParameter()
        Me.fpAmplitude = New SpectroscopyManager.mFitParameter()
        Me.fpTemperature = New SpectroscopyManager.mFitParameter()
        Me.SuspendLayout()
        '
        'btnSelectYOffset
        '
        Me.btnSelectYOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectYOffset.Location = New System.Drawing.Point(592, 18)
        Me.btnSelectYOffset.Name = "btnSelectYOffset"
        Me.btnSelectYOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectYOffset.TabIndex = 5
        Me.btnSelectYOffset.TabStop = False
        Me.btnSelectYOffset.UseVisualStyleBackColor = True
        '
        'btnSelectXCenter
        '
        Me.btnSelectXCenter.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectXCenter.Location = New System.Drawing.Point(592, 42)
        Me.btnSelectXCenter.Name = "btnSelectXCenter"
        Me.btnSelectXCenter.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectXCenter.TabIndex = 5
        Me.btnSelectXCenter.TabStop = False
        Me.btnSelectXCenter.UseVisualStyleBackColor = True
        '
        'btnSelectAmplitude
        '
        Me.btnSelectAmplitude.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectAmplitude.Location = New System.Drawing.Point(592, 66)
        Me.btnSelectAmplitude.Name = "btnSelectAmplitude"
        Me.btnSelectAmplitude.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectAmplitude.TabIndex = 5
        Me.btnSelectAmplitude.TabStop = False
        Me.btnSelectAmplitude.UseVisualStyleBackColor = True
        '
        'btnSelectWidth
        '
        Me.btnSelectWidth.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnSelectWidth.Location = New System.Drawing.Point(592, 89)
        Me.btnSelectWidth.Name = "btnSelectWidth"
        Me.btnSelectWidth.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectWidth.TabIndex = 5
        Me.btnSelectWidth.TabStop = False
        Me.btnSelectWidth.UseVisualStyleBackColor = True
        '
        'fpYOffset
        '
        Me.fpYOffset.DecimalValue = 0.0R
        Me.fpYOffset.Location = New System.Drawing.Point(274, 13)
        Me.fpYOffset.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpYOffset.Name = "fpYOffset"
        Me.fpYOffset.Size = New System.Drawing.Size(312, 26)
        Me.fpYOffset.TabIndex = 10
        Me.fpYOffset.Value = 0.0R
        '
        'fpXCenter
        '
        Me.fpXCenter.DecimalValue = 0.0R
        Me.fpXCenter.Location = New System.Drawing.Point(274, 37)
        Me.fpXCenter.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpXCenter.Name = "fpXCenter"
        Me.fpXCenter.Size = New System.Drawing.Size(312, 26)
        Me.fpXCenter.TabIndex = 11
        Me.fpXCenter.Value = 0.0R
        '
        'fpAmplitude
        '
        Me.fpAmplitude.DecimalValue = 0.0R
        Me.fpAmplitude.Location = New System.Drawing.Point(274, 61)
        Me.fpAmplitude.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpAmplitude.Name = "fpAmplitude"
        Me.fpAmplitude.Size = New System.Drawing.Size(312, 26)
        Me.fpAmplitude.TabIndex = 12
        Me.fpAmplitude.Value = 0.0R
        '
        'fpTemperature
        '
        Me.fpTemperature.DecimalValue = 0.0R
        Me.fpTemperature.Location = New System.Drawing.Point(274, 85)
        Me.fpTemperature.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpTemperature.Name = "fpTemperature"
        Me.fpTemperature.Size = New System.Drawing.Size(312, 26)
        Me.fpTemperature.TabIndex = 13
        Me.fpTemperature.Value = 0.0R
        '
        'cFitSettingPanel_Step_FermiFunction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.fpTemperature)
        Me.Controls.Add(Me.fpAmplitude)
        Me.Controls.Add(Me.fpXCenter)
        Me.Controls.Add(Me.fpYOffset)
        Me.Controls.Add(Me.btnSelectWidth)
        Me.Controls.Add(Me.btnSelectAmplitude)
        Me.Controls.Add(Me.btnSelectXCenter)
        Me.Controls.Add(Me.btnSelectYOffset)
        Me.Name = "cFitSettingPanel_Step_FermiFunction"
        Me.Size = New System.Drawing.Size(659, 125)
        Me.Controls.SetChildIndex(Me.btnSelectYOffset, 0)
        Me.Controls.SetChildIndex(Me.btnSelectXCenter, 0)
        Me.Controls.SetChildIndex(Me.btnSelectAmplitude, 0)
        Me.Controls.SetChildIndex(Me.btnSelectWidth, 0)
        Me.Controls.SetChildIndex(Me.fpYOffset, 0)
        Me.Controls.SetChildIndex(Me.fpXCenter, 0)
        Me.Controls.SetChildIndex(Me.fpAmplitude, 0)
        Me.Controls.SetChildIndex(Me.fpTemperature, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSelectYOffset As System.Windows.Forms.Button
    Friend WithEvents btnSelectXCenter As System.Windows.Forms.Button
    Friend WithEvents btnSelectAmplitude As System.Windows.Forms.Button
    Friend WithEvents btnSelectWidth As System.Windows.Forms.Button
    Friend WithEvents fpYOffset As SpectroscopyManager.mFitParameter
    Friend WithEvents fpXCenter As SpectroscopyManager.mFitParameter
    Friend WithEvents fpAmplitude As SpectroscopyManager.mFitParameter
    Friend WithEvents fpTemperature As SpectroscopyManager.mFitParameter

End Class
