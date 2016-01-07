<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wAbout
    Inherits SpectroscopyManager.wFormBase

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    Friend WithEvents txtChangeLog As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.wbChangelog = New System.Windows.Forms.WebBrowser()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.txtEULA = New System.Windows.Forms.TextBox()
        Me.txtChangeLog = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.BackColor = System.Drawing.Color.Transparent
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OKButton.ForeColor = System.Drawing.Color.White
        Me.OKButton.Location = New System.Drawing.Point(1114, 389)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(100, 26)
        Me.OKButton.TabIndex = 0
        Me.OKButton.Text = "&OK"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(211, 385)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(433, 30)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "This open source software is free of charge. If it contributes to your" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "scientifi" &
    "c work, please acknowledge it in an appropriate way!"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'wbChangelog
        '
        Me.wbChangelog.AllowNavigation = False
        Me.wbChangelog.AllowWebBrowserDrop = False
        Me.wbChangelog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.wbChangelog.IsWebBrowserContextMenuEnabled = False
        Me.wbChangelog.Location = New System.Drawing.Point(650, -3)
        Me.wbChangelog.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbChangelog.Name = "wbChangelog"
        Me.wbChangelog.Size = New System.Drawing.Size(576, 179)
        Me.wbChangelog.TabIndex = 3
        Me.wbChangelog.WebBrowserShortcutsEnabled = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(676, 394)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(432, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Please visit http://www.spectrafox.com for further informations."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.Color.White
        Me.lblVersion.Location = New System.Drawing.Point(494, 7)
        Me.lblVersion.MaximumSize = New System.Drawing.Size(150, 13)
        Me.lblVersion.MinimumSize = New System.Drawing.Size(150, 15)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(150, 15)
        Me.lblVersion.TabIndex = 1
        Me.lblVersion.Text = "{0}.{1:00}.{2}.{3}"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtEULA
        '
        Me.txtEULA.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEULA.BackColor = System.Drawing.Color.Black
        Me.txtEULA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEULA.ForeColor = System.Drawing.Color.White
        Me.txtEULA.Location = New System.Drawing.Point(650, 176)
        Me.txtEULA.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.txtEULA.Multiline = True
        Me.txtEULA.Name = "txtEULA"
        Me.txtEULA.ReadOnly = True
        Me.txtEULA.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtEULA.Size = New System.Drawing.Size(576, 202)
        Me.txtEULA.TabIndex = 0
        Me.txtEULA.TabStop = False
        Me.txtEULA.Text = "EULA" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtChangeLog
        '
        Me.txtChangeLog.BackColor = System.Drawing.Color.Black
        Me.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChangeLog.ForeColor = System.Drawing.Color.White
        Me.txtChangeLog.Location = New System.Drawing.Point(653, 1)
        Me.txtChangeLog.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.txtChangeLog.Multiline = True
        Me.txtChangeLog.Name = "txtChangeLog"
        Me.txtChangeLog.ReadOnly = True
        Me.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtChangeLog.Size = New System.Drawing.Size(571, 156)
        Me.txtChangeLog.TabIndex = 0
        Me.txtChangeLog.TabStop = False
        Me.txtChangeLog.Text = "ChangeLog"
        Me.txtChangeLog.Visible = False
        '
        'wAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.SpectroscopyManager.My.Resources.Resources.Background4s
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(1226, 427)
        Me.Controls.Add(Me.txtEULA)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.wbChangelog)
        Me.Controls.Add(Me.txtChangeLog)
        Me.DoubleBuffered = True
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "wAbout"
        Me.Padding = New System.Windows.Forms.Padding(9)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtEULA As System.Windows.Forms.TextBox
    Friend WithEvents wbChangelog As System.Windows.Forms.WebBrowser
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class
