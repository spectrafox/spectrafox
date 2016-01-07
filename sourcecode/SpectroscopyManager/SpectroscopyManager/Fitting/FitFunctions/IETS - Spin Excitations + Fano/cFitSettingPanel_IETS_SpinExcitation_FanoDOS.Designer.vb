<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_IETS_SpinExcitation_FanoDOS
    Inherits SpectroscopyManager.cFitSettingPanel_IETS_SpinExcitation

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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.fpFanoGRes = New SpectroscopyManager.mFitParameter()
        Me.fpFanoXc = New SpectroscopyManager.mFitParameter()
        Me.fpFanoY0 = New SpectroscopyManager.mFitParameter()
        Me.fpFanoQ = New SpectroscopyManager.mFitParameter()
        Me.fpFanoAmplitude = New SpectroscopyManager.mFitParameter()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.fpFanoGRes)
        Me.GroupBox2.Controls.Add(Me.fpFanoXc)
        Me.GroupBox2.Controls.Add(Me.fpFanoY0)
        Me.GroupBox2.Controls.Add(Me.fpFanoQ)
        Me.GroupBox2.Controls.Add(Me.fpFanoAmplitude)
        Me.GroupBox2.Location = New System.Drawing.Point(262, 312)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(672, 104)
        Me.GroupBox2.TabIndex = 33
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Fano parameters"
        '
        'fpFanoGRes
        '
        Me.fpFanoGRes.DecimalValue = 0.0R
        Me.fpFanoGRes.Location = New System.Drawing.Point(328, 19)
        Me.fpFanoGRes.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFanoGRes.Name = "fpFanoGRes"
        Me.fpFanoGRes.Size = New System.Drawing.Size(301, 26)
        Me.fpFanoGRes.TabIndex = 0
        Me.fpFanoGRes.Value = 0.0R
        '
        'fpFanoXc
        '
        Me.fpFanoXc.DecimalValue = 0.0R
        Me.fpFanoXc.Location = New System.Drawing.Point(6, 44)
        Me.fpFanoXc.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFanoXc.Name = "fpFanoXc"
        Me.fpFanoXc.Size = New System.Drawing.Size(296, 26)
        Me.fpFanoXc.TabIndex = 0
        Me.fpFanoXc.Value = 0.0R
        '
        'fpFanoY0
        '
        Me.fpFanoY0.DecimalValue = 0.0R
        Me.fpFanoY0.Location = New System.Drawing.Point(6, 19)
        Me.fpFanoY0.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFanoY0.Name = "fpFanoY0"
        Me.fpFanoY0.Size = New System.Drawing.Size(296, 26)
        Me.fpFanoY0.TabIndex = 0
        Me.fpFanoY0.Value = 0.0R
        '
        'fpFanoQ
        '
        Me.fpFanoQ.DecimalValue = 0.0R
        Me.fpFanoQ.Location = New System.Drawing.Point(328, 44)
        Me.fpFanoQ.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFanoQ.Name = "fpFanoQ"
        Me.fpFanoQ.Size = New System.Drawing.Size(301, 26)
        Me.fpFanoQ.TabIndex = 0
        Me.fpFanoQ.Value = 0.0R
        '
        'fpFanoAmplitude
        '
        Me.fpFanoAmplitude.DecimalValue = 0.0R
        Me.fpFanoAmplitude.Location = New System.Drawing.Point(6, 70)
        Me.fpFanoAmplitude.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFanoAmplitude.Name = "fpFanoAmplitude"
        Me.fpFanoAmplitude.Size = New System.Drawing.Size(296, 26)
        Me.fpFanoAmplitude.TabIndex = 0
        Me.fpFanoAmplitude.Value = 0.0R
        '
        'cFitSettingPanel_IETS_SpinExcitation_FanoDOS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "cFitSettingPanel_IETS_SpinExcitation_FanoDOS"
        Me.SelectedSpinInOneHalfs = 4
        Me.Size = New System.Drawing.Size(937, 418)
        Me.Controls.SetChildIndex(Me.GroupBox2, 0)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents fpFanoAmplitude As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFanoGRes As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFanoXc As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFanoY0 As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFanoQ As SpectroscopyManager.mFitParameter

End Class
