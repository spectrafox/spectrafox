<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wProgramUpdate
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


    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.wbChangeLog = New System.Windows.Forms.WebBrowser()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pgbUpdateProgress = New System.Windows.Forms.ProgressBar()
        Me.lblUpdateProgress = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'wbChangeLog
        '
        Me.wbChangeLog.AllowNavigation = False
        Me.wbChangeLog.AllowWebBrowserDrop = False
        Me.wbChangeLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.wbChangeLog.IsWebBrowserContextMenuEnabled = False
        Me.wbChangeLog.Location = New System.Drawing.Point(651, 129)
        Me.wbChangeLog.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbChangeLog.Name = "wbChangeLog"
        Me.wbChangeLog.Size = New System.Drawing.Size(514, 248)
        Me.wbChangeLog.TabIndex = 5
        Me.wbChangeLog.WebBrowserShortcutsEnabled = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(3, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(296, 31)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Update in progress ..."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(1, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(179, 31)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "What's new?"
        '
        'pgbUpdateProgress
        '
        Me.pgbUpdateProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbUpdateProgress.Location = New System.Drawing.Point(658, 50)
        Me.pgbUpdateProgress.Name = "pgbUpdateProgress"
        Me.pgbUpdateProgress.Size = New System.Drawing.Size(483, 23)
        Me.pgbUpdateProgress.TabIndex = 3
        '
        'lblUpdateProgress
        '
        Me.lblUpdateProgress.AutoSize = True
        Me.lblUpdateProgress.BackColor = System.Drawing.Color.Transparent
        Me.lblUpdateProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUpdateProgress.ForeColor = System.Drawing.Color.White
        Me.lblUpdateProgress.Location = New System.Drawing.Point(17, 77)
        Me.lblUpdateProgress.MaximumSize = New System.Drawing.Size(150, 13)
        Me.lblUpdateProgress.MinimumSize = New System.Drawing.Size(150, 15)
        Me.lblUpdateProgress.Name = "lblUpdateProgress"
        Me.lblUpdateProgress.Size = New System.Drawing.Size(150, 15)
        Me.lblUpdateProgress.TabIndex = 1
        Me.lblUpdateProgress.Text = "UpdateText"
        Me.lblUpdateProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(174, 384)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(471, 30)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "This software is free of charge. If it contributes to publish" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "a scientific work," & _
    " please acknowledge SpectraFox in an appropriate way!"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(720, 393)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(432, 16)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Please visit http://www.spectrafox.com for further informations."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Black
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.lblUpdateProgress)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(651, -1)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(524, 379)
        Me.Panel1.TabIndex = 8
        '
        'wProgramUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.SpectroscopyManager.My.Resources.Resources.Background4s
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(1164, 423)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.wbChangeLog)
        Me.Controls.Add(Me.pgbUpdateProgress)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "wProgramUpdate"
        Me.Padding = New System.Windows.Forms.Padding(9)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Program Update"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pgbUpdateProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblUpdateProgress As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents wbChangeLog As System.Windows.Forms.WebBrowser
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel

End Class
