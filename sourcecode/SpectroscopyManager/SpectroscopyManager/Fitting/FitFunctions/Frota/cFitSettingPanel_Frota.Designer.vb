<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class cFitSettingPanel_Frota
    Inherits SpectroscopyManager.cFitSettingPanel

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnSelectYOffset = New System.Windows.Forms.Button()
        Me.btnSelectXCenter = New System.Windows.Forms.Button()
        Me.btnSelectAmplitude = New System.Windows.Forms.Button()
        Me.btnSelectWidth = New System.Windows.Forms.Button()
        Me.fpAmplitude = New SpectroscopyManager.mFitParameter()
        Me.fpB = New SpectroscopyManager.mFitParameter()
        Me.fpC = New SpectroscopyManager.mFitParameter()
        Me.fpGamma = New SpectroscopyManager.mFitParameter()
        Me.fpPhi = New SpectroscopyManager.mFitParameter()
        Me.fpXCenter = New SpectroscopyManager.mFitParameter()
        Me.SuspendLayout()
        '
        'btnSelectYOffset
        '
        Me.btnSelectYOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectYOffset.Location = New System.Drawing.Point(589, 6)
        Me.btnSelectYOffset.Name = "btnSelectYOffset"
        Me.btnSelectYOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectYOffset.TabIndex = 5
        Me.btnSelectYOffset.TabStop = False
        Me.btnSelectYOffset.UseVisualStyleBackColor = True
        '
        'btnSelectXCenter
        '
        Me.btnSelectXCenter.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectXCenter.Location = New System.Drawing.Point(589, 31)
        Me.btnSelectXCenter.Name = "btnSelectXCenter"
        Me.btnSelectXCenter.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectXCenter.TabIndex = 5
        Me.btnSelectXCenter.TabStop = False
        Me.btnSelectXCenter.UseVisualStyleBackColor = True
        '
        'btnSelectAmplitude
        '
        Me.btnSelectAmplitude.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectAmplitude.Location = New System.Drawing.Point(589, 79)
        Me.btnSelectAmplitude.Name = "btnSelectAmplitude"
        Me.btnSelectAmplitude.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectAmplitude.TabIndex = 5
        Me.btnSelectAmplitude.TabStop = False
        Me.btnSelectAmplitude.UseVisualStyleBackColor = True
        '
        'btnSelectWidth
        '
        Me.btnSelectWidth.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnSelectWidth.Location = New System.Drawing.Point(589, 103)
        Me.btnSelectWidth.Name = "btnSelectWidth"
        Me.btnSelectWidth.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectWidth.TabIndex = 5
        Me.btnSelectWidth.TabStop = False
        Me.btnSelectWidth.UseVisualStyleBackColor = True
        '
        'fpAmplitude
        '
        Me.fpAmplitude.DecimalValue = 0R
        Me.fpAmplitude.Location = New System.Drawing.Point(287, 75)
        Me.fpAmplitude.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpAmplitude.Name = "fpAmplitude"
        Me.fpAmplitude.Size = New System.Drawing.Size(296, 26)
        Me.fpAmplitude.TabIndex = 21
        Me.fpAmplitude.Value = 0R
        '
        'fpB
        '
        Me.fpB.DecimalValue = 0R
        Me.fpB.Location = New System.Drawing.Point(287, 51)
        Me.fpB.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpB.Name = "fpB"
        Me.fpB.Size = New System.Drawing.Size(296, 26)
        Me.fpB.TabIndex = 20
        Me.fpB.Value = 0R
        '
        'fpC
        '
        Me.fpC.DecimalValue = 0R
        Me.fpC.Location = New System.Drawing.Point(287, 4)
        Me.fpC.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpC.Name = "fpC"
        Me.fpC.Size = New System.Drawing.Size(296, 26)
        Me.fpC.TabIndex = 19
        Me.fpC.Value = 0R
        '
        'fpGamma
        '
        Me.fpGamma.DecimalValue = 0R
        Me.fpGamma.Location = New System.Drawing.Point(287, 99)
        Me.fpGamma.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpGamma.Name = "fpGamma"
        Me.fpGamma.Size = New System.Drawing.Size(296, 26)
        Me.fpGamma.TabIndex = 21
        Me.fpGamma.Value = 0R
        '
        'fpPhi
        '
        Me.fpPhi.DecimalValue = 0R
        Me.fpPhi.Location = New System.Drawing.Point(287, 123)
        Me.fpPhi.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpPhi.Name = "fpPhi"
        Me.fpPhi.Size = New System.Drawing.Size(296, 26)
        Me.fpPhi.TabIndex = 21
        Me.fpPhi.Value = 0R
        '
        'fpXCenter
        '
        Me.fpXCenter.DecimalValue = 0R
        Me.fpXCenter.Location = New System.Drawing.Point(287, 28)
        Me.fpXCenter.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpXCenter.Name = "fpXCenter"
        Me.fpXCenter.Size = New System.Drawing.Size(296, 26)
        Me.fpXCenter.TabIndex = 20
        Me.fpXCenter.Value = 0R
        '
        'cFitSettingPanel_Frota
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.fpPhi)
        Me.Controls.Add(Me.fpGamma)
        Me.Controls.Add(Me.fpAmplitude)
        Me.Controls.Add(Me.fpXCenter)
        Me.Controls.Add(Me.fpB)
        Me.Controls.Add(Me.fpC)
        Me.Controls.Add(Me.btnSelectWidth)
        Me.Controls.Add(Me.btnSelectAmplitude)
        Me.Controls.Add(Me.btnSelectXCenter)
        Me.Controls.Add(Me.btnSelectYOffset)
        Me.Name = "cFitSettingPanel_Frota"
        Me.Size = New System.Drawing.Size(713, 159)
        Me.Controls.SetChildIndex(Me.btnSelectYOffset, 0)
        Me.Controls.SetChildIndex(Me.btnSelectXCenter, 0)
        Me.Controls.SetChildIndex(Me.btnSelectAmplitude, 0)
        Me.Controls.SetChildIndex(Me.btnSelectWidth, 0)
        Me.Controls.SetChildIndex(Me.fpC, 0)
        Me.Controls.SetChildIndex(Me.fpB, 0)
        Me.Controls.SetChildIndex(Me.fpXCenter, 0)
        Me.Controls.SetChildIndex(Me.fpAmplitude, 0)
        Me.Controls.SetChildIndex(Me.fpGamma, 0)
        Me.Controls.SetChildIndex(Me.fpPhi, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSelectYOffset As System.Windows.Forms.Button
    Friend WithEvents btnSelectXCenter As System.Windows.Forms.Button
    Friend WithEvents btnSelectAmplitude As System.Windows.Forms.Button
    Friend WithEvents btnSelectWidth As System.Windows.Forms.Button
    Friend WithEvents fpAmplitude As SpectroscopyManager.mFitParameter
    Friend WithEvents fpB As SpectroscopyManager.mFitParameter
    Friend WithEvents fpC As SpectroscopyManager.mFitParameter
    Friend WithEvents fpGamma As SpectroscopyManager.mFitParameter
    Friend WithEvents fpPhi As SpectroscopyManager.mFitParameter
    Friend WithEvents fpXCenter As mFitParameter
End Class
