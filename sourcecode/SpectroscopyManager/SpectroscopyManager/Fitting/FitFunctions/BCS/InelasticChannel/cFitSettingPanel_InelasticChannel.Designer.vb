<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel_InelasticChannel
    Inherits SpectroscopyManager.cFitSettingPanel_TipSampleConvolution

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
        Me.gbInelasticChannels = New System.Windows.Forms.GroupBox()
        Me.btnAddInelasticChannel = New System.Windows.Forms.Button()
        Me.lblAddInelasticChannel = New System.Windows.Forms.Label()
        Me.flpInelasticChannels = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbInelasticChannels.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbInelasticChannels
        '
        Me.gbInelasticChannels.Controls.Add(Me.btnAddInelasticChannel)
        Me.gbInelasticChannels.Controls.Add(Me.lblAddInelasticChannel)
        Me.gbInelasticChannels.Controls.Add(Me.flpInelasticChannels)
        Me.gbInelasticChannels.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gbInelasticChannels.Location = New System.Drawing.Point(256, 238)
        Me.gbInelasticChannels.Name = "gbInelasticChannels"
        Me.gbInelasticChannels.Size = New System.Drawing.Size(768, 336)
        Me.gbInelasticChannels.TabIndex = 25
        Me.gbInelasticChannels.TabStop = False
        Me.gbInelasticChannels.Text = "inelastic channels:"
        '
        'btnAddInelasticChannel
        '
        Me.btnAddInelasticChannel.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddInelasticChannel.Location = New System.Drawing.Point(126, 13)
        Me.btnAddInelasticChannel.Name = "btnAddInelasticChannel"
        Me.btnAddInelasticChannel.Size = New System.Drawing.Size(31, 23)
        Me.btnAddInelasticChannel.TabIndex = 5
        Me.btnAddInelasticChannel.TabStop = False
        Me.btnAddInelasticChannel.UseVisualStyleBackColor = True
        '
        'lblAddInelasticChannel
        '
        Me.lblAddInelasticChannel.AutoSize = True
        Me.lblAddInelasticChannel.Location = New System.Drawing.Point(10, 18)
        Me.lblAddInelasticChannel.Name = "lblAddInelasticChannel"
        Me.lblAddInelasticChannel.Size = New System.Drawing.Size(110, 13)
        Me.lblAddInelasticChannel.TabIndex = 4
        Me.lblAddInelasticChannel.Text = "add inelastic channel:"
        '
        'flpInelasticChannels
        '
        Me.flpInelasticChannels.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpInelasticChannels.AutoScroll = True
        Me.flpInelasticChannels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpInelasticChannels.Location = New System.Drawing.Point(6, 40)
        Me.flpInelasticChannels.Name = "flpInelasticChannels"
        Me.flpInelasticChannels.Size = New System.Drawing.Size(756, 290)
        Me.flpInelasticChannels.TabIndex = 1
        '
        'cFitSettingPanel_InelasticChannel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbInelasticChannels)
        Me.Name = "cFitSettingPanel_InelasticChannel"
        Me.Size = New System.Drawing.Size(1024, 574)
        Me.Controls.SetChildIndex(Me.gbInelasticChannels, 0)
        Me.gbInelasticChannels.ResumeLayout(False)
        Me.gbInelasticChannels.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents gbInelasticChannels As GroupBox
    Friend WithEvents btnAddInelasticChannel As Button
    Friend WithEvents lblAddInelasticChannel As Label
    Friend WithEvents flpInelasticChannels As FlowLayoutPanel
End Class
