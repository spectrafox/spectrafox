<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_Polynomial
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
        Me.btnSelectParameterA = New System.Windows.Forms.Button()
        Me.btnSelectParameterB = New System.Windows.Forms.Button()
        Me.btnSelectParameterC = New System.Windows.Forms.Button()
        Me.btnSelectParameterD = New System.Windows.Forms.Button()
        Me.btnSelectAmplitude = New System.Windows.Forms.Button()
        Me.fpAmplitude = New SpectroscopyManager.mFitParameter()
        Me.fpXCenter = New SpectroscopyManager.mFitParameter()
        Me.fpYOffset = New SpectroscopyManager.mFitParameter()
        Me.fpFactorA = New SpectroscopyManager.mFitParameter()
        Me.fpFactorB = New SpectroscopyManager.mFitParameter()
        Me.fpFactorC = New SpectroscopyManager.mFitParameter()
        Me.fpFactorD = New SpectroscopyManager.mFitParameter()
        Me.SuspendLayout()
        '
        'btnSelectYOffset
        '
        Me.btnSelectYOffset.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectYOffset.Location = New System.Drawing.Point(577, 15)
        Me.btnSelectYOffset.Name = "btnSelectYOffset"
        Me.btnSelectYOffset.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectYOffset.TabIndex = 5
        Me.btnSelectYOffset.TabStop = False
        Me.btnSelectYOffset.UseVisualStyleBackColor = True
        '
        'btnSelectXCenter
        '
        Me.btnSelectXCenter.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelectXCenter.Location = New System.Drawing.Point(577, 38)
        Me.btnSelectXCenter.Name = "btnSelectXCenter"
        Me.btnSelectXCenter.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectXCenter.TabIndex = 5
        Me.btnSelectXCenter.TabStop = False
        Me.btnSelectXCenter.UseVisualStyleBackColor = True
        '
        'btnSelectParameterA
        '
        Me.btnSelectParameterA.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectParameterA.Location = New System.Drawing.Point(577, 111)
        Me.btnSelectParameterA.Name = "btnSelectParameterA"
        Me.btnSelectParameterA.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectParameterA.TabIndex = 5
        Me.btnSelectParameterA.TabStop = False
        Me.btnSelectParameterA.UseVisualStyleBackColor = True
        '
        'btnSelectParameterB
        '
        Me.btnSelectParameterB.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectParameterB.Location = New System.Drawing.Point(577, 135)
        Me.btnSelectParameterB.Name = "btnSelectParameterB"
        Me.btnSelectParameterB.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectParameterB.TabIndex = 5
        Me.btnSelectParameterB.TabStop = False
        Me.btnSelectParameterB.UseVisualStyleBackColor = True
        '
        'btnSelectParameterC
        '
        Me.btnSelectParameterC.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectParameterC.Location = New System.Drawing.Point(577, 159)
        Me.btnSelectParameterC.Name = "btnSelectParameterC"
        Me.btnSelectParameterC.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectParameterC.TabIndex = 5
        Me.btnSelectParameterC.TabStop = False
        Me.btnSelectParameterC.UseVisualStyleBackColor = True
        '
        'btnSelectParameterD
        '
        Me.btnSelectParameterD.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_12
        Me.btnSelectParameterD.Location = New System.Drawing.Point(577, 183)
        Me.btnSelectParameterD.Name = "btnSelectParameterD"
        Me.btnSelectParameterD.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectParameterD.TabIndex = 5
        Me.btnSelectParameterD.TabStop = False
        Me.btnSelectParameterD.UseVisualStyleBackColor = True
        '
        'btnSelectAmplitude
        '
        Me.btnSelectAmplitude.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_16
        Me.btnSelectAmplitude.Location = New System.Drawing.Point(577, 61)
        Me.btnSelectAmplitude.Name = "btnSelectAmplitude"
        Me.btnSelectAmplitude.Size = New System.Drawing.Size(30, 19)
        Me.btnSelectAmplitude.TabIndex = 5
        Me.btnSelectAmplitude.TabStop = False
        Me.btnSelectAmplitude.UseVisualStyleBackColor = True
        '
        'fpAmplitude
        '
        Me.fpAmplitude.Location = New System.Drawing.Point(275, 58)
        Me.fpAmplitude.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpAmplitude.Name = "fpAmplitude"
        Me.fpAmplitude.Size = New System.Drawing.Size(296, 26)
        Me.fpAmplitude.TabIndex = 27
        '
        'fpXCenter
        '
        Me.fpXCenter.Location = New System.Drawing.Point(275, 34)
        Me.fpXCenter.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpXCenter.Name = "fpXCenter"
        Me.fpXCenter.Size = New System.Drawing.Size(296, 26)
        Me.fpXCenter.TabIndex = 26
        '
        'fpYOffset
        '
        Me.fpYOffset.Location = New System.Drawing.Point(275, 10)
        Me.fpYOffset.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpYOffset.Name = "fpYOffset"
        Me.fpYOffset.Size = New System.Drawing.Size(296, 26)
        Me.fpYOffset.TabIndex = 25
        '
        'fpFactorA
        '
        Me.fpFactorA.Location = New System.Drawing.Point(275, 108)
        Me.fpFactorA.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFactorA.Name = "fpFactorA"
        Me.fpFactorA.Size = New System.Drawing.Size(296, 26)
        Me.fpFactorA.TabIndex = 25
        '
        'fpFactorB
        '
        Me.fpFactorB.Location = New System.Drawing.Point(275, 132)
        Me.fpFactorB.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFactorB.Name = "fpFactorB"
        Me.fpFactorB.Size = New System.Drawing.Size(296, 26)
        Me.fpFactorB.TabIndex = 25
        '
        'fpFactorC
        '
        Me.fpFactorC.Location = New System.Drawing.Point(275, 156)
        Me.fpFactorC.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFactorC.Name = "fpFactorC"
        Me.fpFactorC.Size = New System.Drawing.Size(296, 26)
        Me.fpFactorC.TabIndex = 25
        '
        'fpFactorD
        '
        Me.fpFactorD.Location = New System.Drawing.Point(275, 180)
        Me.fpFactorD.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFactorD.Name = "fpFactorD"
        Me.fpFactorD.Size = New System.Drawing.Size(296, 26)
        Me.fpFactorD.TabIndex = 25
        '
        'cFitSettingPanel_Polynomial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.fpAmplitude)
        Me.Controls.Add(Me.fpXCenter)
        Me.Controls.Add(Me.fpFactorD)
        Me.Controls.Add(Me.fpFactorC)
        Me.Controls.Add(Me.fpFactorB)
        Me.Controls.Add(Me.fpFactorA)
        Me.Controls.Add(Me.fpYOffset)
        Me.Controls.Add(Me.btnSelectParameterD)
        Me.Controls.Add(Me.btnSelectParameterC)
        Me.Controls.Add(Me.btnSelectParameterB)
        Me.Controls.Add(Me.btnSelectAmplitude)
        Me.Controls.Add(Me.btnSelectParameterA)
        Me.Controls.Add(Me.btnSelectXCenter)
        Me.Controls.Add(Me.btnSelectYOffset)
        Me.Name = "cFitSettingPanel_Polynomial"
        Me.Size = New System.Drawing.Size(660, 221)
        Me.Controls.SetChildIndex(Me.btnSelectYOffset, 0)
        Me.Controls.SetChildIndex(Me.btnSelectXCenter, 0)
        Me.Controls.SetChildIndex(Me.btnSelectParameterA, 0)
        Me.Controls.SetChildIndex(Me.btnSelectAmplitude, 0)
        Me.Controls.SetChildIndex(Me.btnSelectParameterB, 0)
        Me.Controls.SetChildIndex(Me.btnSelectParameterC, 0)
        Me.Controls.SetChildIndex(Me.btnSelectParameterD, 0)
        Me.Controls.SetChildIndex(Me.fpYOffset, 0)
        Me.Controls.SetChildIndex(Me.fpFactorA, 0)
        Me.Controls.SetChildIndex(Me.fpFactorB, 0)
        Me.Controls.SetChildIndex(Me.fpFactorC, 0)
        Me.Controls.SetChildIndex(Me.fpFactorD, 0)
        Me.Controls.SetChildIndex(Me.fpXCenter, 0)
        Me.Controls.SetChildIndex(Me.fpAmplitude, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSelectYOffset As System.Windows.Forms.Button
    Friend WithEvents btnSelectXCenter As System.Windows.Forms.Button
    Friend WithEvents btnSelectParameterA As System.Windows.Forms.Button
    Friend WithEvents btnSelectParameterB As System.Windows.Forms.Button
    Friend WithEvents btnSelectParameterC As System.Windows.Forms.Button
    Friend WithEvents btnSelectParameterD As System.Windows.Forms.Button
    Friend WithEvents btnSelectAmplitude As System.Windows.Forms.Button
    Friend WithEvents fpAmplitude As SpectroscopyManager.mFitParameter
    Friend WithEvents fpXCenter As SpectroscopyManager.mFitParameter
    Friend WithEvents fpYOffset As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFactorA As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFactorB As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFactorC As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFactorD As SpectroscopyManager.mFitParameter

End Class
