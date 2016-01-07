<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingSubParameterPanel_4
    Inherits System.Windows.Forms.UserControl

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
        Me.btnRemoveSubParameterPanel = New System.Windows.Forms.Button()
        Me.fpFitParameter1 = New SpectroscopyManager.mFitParameter()
        Me.fpFitParameter2 = New SpectroscopyManager.mFitParameter()
        Me.fpFitParameter3 = New SpectroscopyManager.mFitParameter()
        Me.fpFitParameter4 = New SpectroscopyManager.mFitParameter()
        Me.SuspendLayout()
        '
        'btnRemoveSubParameterPanel
        '
        Me.btnRemoveSubParameterPanel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveSubParameterPanel.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnRemoveSubParameterPanel.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnRemoveSubParameterPanel.Location = New System.Drawing.Point(652, 13)
        Me.btnRemoveSubParameterPanel.Name = "btnRemoveSubParameterPanel"
        Me.btnRemoveSubParameterPanel.Size = New System.Drawing.Size(68, 37)
        Me.btnRemoveSubParameterPanel.TabIndex = 1
        Me.btnRemoveSubParameterPanel.TabStop = False
        Me.btnRemoveSubParameterPanel.Text = "remove"
        Me.btnRemoveSubParameterPanel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnRemoveSubParameterPanel.UseVisualStyleBackColor = True
        '
        'fpFitParameter1
        '
        Me.fpFitParameter1.DecimalValue = 0.0R
        Me.fpFitParameter1.Location = New System.Drawing.Point(3, 5)
        Me.fpFitParameter1.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFitParameter1.Name = "fpFitParameter1"
        Me.fpFitParameter1.Size = New System.Drawing.Size(304, 26)
        Me.fpFitParameter1.TabIndex = 2
        Me.fpFitParameter1.Value = 0.0R
        '
        'fpFitParameter2
        '
        Me.fpFitParameter2.DecimalValue = 0.0R
        Me.fpFitParameter2.Location = New System.Drawing.Point(3, 32)
        Me.fpFitParameter2.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFitParameter2.Name = "fpFitParameter2"
        Me.fpFitParameter2.Size = New System.Drawing.Size(304, 26)
        Me.fpFitParameter2.TabIndex = 2
        Me.fpFitParameter2.Value = 0.0R
        '
        'fpFitParameter3
        '
        Me.fpFitParameter3.DecimalValue = 0.0R
        Me.fpFitParameter3.Location = New System.Drawing.Point(321, 5)
        Me.fpFitParameter3.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFitParameter3.Name = "fpFitParameter3"
        Me.fpFitParameter3.Size = New System.Drawing.Size(304, 26)
        Me.fpFitParameter3.TabIndex = 2
        Me.fpFitParameter3.Value = 0.0R
        '
        'fpFitParameter4
        '
        Me.fpFitParameter4.DecimalValue = 0.0R
        Me.fpFitParameter4.Location = New System.Drawing.Point(321, 32)
        Me.fpFitParameter4.MaximumSize = New System.Drawing.Size(400, 26)
        Me.fpFitParameter4.Name = "fpFitParameter4"
        Me.fpFitParameter4.Size = New System.Drawing.Size(304, 26)
        Me.fpFitParameter4.TabIndex = 2
        Me.fpFitParameter4.Value = 0.0R
        '
        'cFitSettingSubParameterPanel_4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.fpFitParameter4)
        Me.Controls.Add(Me.fpFitParameter3)
        Me.Controls.Add(Me.fpFitParameter2)
        Me.Controls.Add(Me.fpFitParameter1)
        Me.Controls.Add(Me.btnRemoveSubParameterPanel)
        Me.Name = "cFitSettingSubParameterPanel_4"
        Me.Size = New System.Drawing.Size(723, 63)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnRemoveSubParameterPanel As System.Windows.Forms.Button
    Friend WithEvents fpFitParameter1 As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFitParameter2 As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFitParameter3 As SpectroscopyManager.mFitParameter
    Friend WithEvents fpFitParameter4 As SpectroscopyManager.mFitParameter

End Class
