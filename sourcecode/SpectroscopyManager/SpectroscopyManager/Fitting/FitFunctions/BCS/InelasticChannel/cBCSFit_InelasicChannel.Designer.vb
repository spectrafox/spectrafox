<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cBCSFit_InelasticChannel
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
        Me.btnRemoveInelasticChannel = New System.Windows.Forms.Button()
        Me.btnSelect_IECEnergy = New System.Windows.Forms.Button()
        Me.ckbFix_IECEnergy = New System.Windows.Forms.CheckBox()
        Me.ckbFix_IECProbability = New System.Windows.Forms.CheckBox()
        Me.lblIECEnergy = New System.Windows.Forms.Label()
        Me.lblIECProbability = New System.Windows.Forms.Label()
        Me.txtIECEnergy = New SpectroscopyManager.NumericTextbox()
        Me.txtIECProbability = New SpectroscopyManager.NumericTextbox()
        Me.SuspendLayout()
        '
        'btnRemoveInelasticChannel
        '
        Me.btnRemoveInelasticChannel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveInelasticChannel.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnRemoveInelasticChannel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveInelasticChannel.Location = New System.Drawing.Point(567, 3)
        Me.btnRemoveInelasticChannel.Name = "btnRemoveInelasticChannel"
        Me.btnRemoveInelasticChannel.Size = New System.Drawing.Size(68, 22)
        Me.btnRemoveInelasticChannel.TabIndex = 1
        Me.btnRemoveInelasticChannel.TabStop = False
        Me.btnRemoveInelasticChannel.Text = "remove"
        Me.btnRemoveInelasticChannel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveInelasticChannel.UseVisualStyleBackColor = True
        '
        'btnSelect_IECEnergy
        '
        Me.btnSelect_IECEnergy.Enabled = False
        Me.btnSelect_IECEnergy.Image = Global.SpectroscopyManager.My.Resources.Resources.select_12
        Me.btnSelect_IECEnergy.Location = New System.Drawing.Point(236, 10)
        Me.btnSelect_IECEnergy.Name = "btnSelect_IECEnergy"
        Me.btnSelect_IECEnergy.Size = New System.Drawing.Size(30, 19)
        Me.btnSelect_IECEnergy.TabIndex = 24
        Me.btnSelect_IECEnergy.TabStop = False
        Me.btnSelect_IECEnergy.UseVisualStyleBackColor = True
        '
        'ckbFix_IECEnergy
        '
        Me.ckbFix_IECEnergy.AutoSize = True
        Me.ckbFix_IECEnergy.Location = New System.Drawing.Point(194, 12)
        Me.ckbFix_IECEnergy.Name = "ckbFix_IECEnergy"
        Me.ckbFix_IECEnergy.Size = New System.Drawing.Size(36, 17)
        Me.ckbFix_IECEnergy.TabIndex = 28
        Me.ckbFix_IECEnergy.Text = "fix"
        Me.ckbFix_IECEnergy.UseVisualStyleBackColor = True
        '
        'ckbFix_IECProbability
        '
        Me.ckbFix_IECProbability.AutoSize = True
        Me.ckbFix_IECProbability.Location = New System.Drawing.Point(483, 12)
        Me.ckbFix_IECProbability.Name = "ckbFix_IECProbability"
        Me.ckbFix_IECProbability.Size = New System.Drawing.Size(36, 17)
        Me.ckbFix_IECProbability.TabIndex = 30
        Me.ckbFix_IECProbability.Text = "fix"
        Me.ckbFix_IECProbability.UseVisualStyleBackColor = True
        '
        'lblIECEnergy
        '
        Me.lblIECEnergy.AutoSize = True
        Me.lblIECEnergy.Location = New System.Drawing.Point(11, 12)
        Me.lblIECEnergy.Name = "lblIECEnergy"
        Me.lblIECEnergy.Size = New System.Drawing.Size(62, 13)
        Me.lblIECEnergy.TabIndex = 19
        Me.lblIECEnergy.Text = "IEC energy:"
        '
        'lblIECProbability
        '
        Me.lblIECProbability.AutoSize = True
        Me.lblIECProbability.Location = New System.Drawing.Point(300, 12)
        Me.lblIECProbability.Name = "lblIECProbability"
        Me.lblIECProbability.Size = New System.Drawing.Size(77, 13)
        Me.lblIECProbability.TabIndex = 20
        Me.lblIECProbability.Text = "IEC probability:"
        '
        'txtIECEnergy
        '
        Me.txtIECEnergy.BackColor = System.Drawing.Color.White
        Me.txtIECEnergy.ForeColor = System.Drawing.Color.Black
        Me.txtIECEnergy.FormatDecimalPlaces = 6
        Me.txtIECEnergy.Location = New System.Drawing.Point(88, 9)
        Me.txtIECEnergy.Name = "txtIECEnergy"
        Me.txtIECEnergy.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtIECEnergy.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtIECEnergy.Size = New System.Drawing.Size(100, 20)
        Me.txtIECEnergy.TabIndex = 25
        Me.txtIECEnergy.Text = "0.000000"
        Me.txtIECEnergy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtIECEnergy.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtIECProbability
        '
        Me.txtIECProbability.BackColor = System.Drawing.Color.White
        Me.txtIECProbability.ForeColor = System.Drawing.Color.Black
        Me.txtIECProbability.FormatDecimalPlaces = 6
        Me.txtIECProbability.Location = New System.Drawing.Point(377, 9)
        Me.txtIECProbability.Name = "txtIECProbability"
        Me.txtIECProbability.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtIECProbability.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtIECProbability.Size = New System.Drawing.Size(100, 20)
        Me.txtIECProbability.TabIndex = 27
        Me.txtIECProbability.Text = "0.000000"
        Me.txtIECProbability.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtIECProbability.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'cBCSFit_InelasticChannel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.btnSelect_IECEnergy)
        Me.Controls.Add(Me.ckbFix_IECEnergy)
        Me.Controls.Add(Me.txtIECEnergy)
        Me.Controls.Add(Me.ckbFix_IECProbability)
        Me.Controls.Add(Me.txtIECProbability)
        Me.Controls.Add(Me.lblIECEnergy)
        Me.Controls.Add(Me.lblIECProbability)
        Me.Controls.Add(Me.btnRemoveInelasticChannel)
        Me.Name = "cBCSFit_InelasticChannel"
        Me.Size = New System.Drawing.Size(638, 40)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRemoveInelasticChannel As System.Windows.Forms.Button
    Friend WithEvents btnSelect_IECEnergy As System.Windows.Forms.Button
    Friend WithEvents ckbFix_IECEnergy As System.Windows.Forms.CheckBox
    Friend WithEvents txtIECEnergy As SpectroscopyManager.NumericTextbox
    Friend WithEvents ckbFix_IECProbability As System.Windows.Forms.CheckBox
    Friend WithEvents txtIECProbability As SpectroscopyManager.NumericTextbox
    Friend WithEvents lblIECEnergy As System.Windows.Forms.Label
    Friend WithEvents lblIECProbability As System.Windows.Forms.Label

End Class
