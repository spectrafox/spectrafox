<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wFitQueue
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
        Me.lbQueue = New System.Windows.Forms.ListBox()
        Me.gbQueue = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnClearFitQueue = New System.Windows.Forms.Button()
        Me.btnPlayPause = New System.Windows.Forms.Button()
        Me.btnRemoveFromQueue = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnGoToRunningFitWindow = New System.Windows.Forms.Button()
        Me.lblCurrentFit = New System.Windows.Forms.Label()
        Me.gbQueue.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbQueue
        '
        Me.lbQueue.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbQueue.FormattingEnabled = True
        Me.lbQueue.Location = New System.Drawing.Point(6, 19)
        Me.lbQueue.Name = "lbQueue"
        Me.lbQueue.Size = New System.Drawing.Size(391, 316)
        Me.lbQueue.TabIndex = 0
        '
        'gbQueue
        '
        Me.gbQueue.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbQueue.Controls.Add(Me.Label2)
        Me.gbQueue.Controls.Add(Me.btnClearFitQueue)
        Me.gbQueue.Controls.Add(Me.btnPlayPause)
        Me.gbQueue.Controls.Add(Me.btnRemoveFromQueue)
        Me.gbQueue.Controls.Add(Me.lbQueue)
        Me.gbQueue.Location = New System.Drawing.Point(12, 33)
        Me.gbQueue.Name = "gbQueue"
        Me.gbQueue.Size = New System.Drawing.Size(484, 356)
        Me.gbQueue.TabIndex = 1
        Me.gbQueue.TabStop = False
        Me.gbQueue.Text = "queued fits"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 338)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(217, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "(double click to show the queued fit window)"
        '
        'btnClearFitQueue
        '
        Me.btnClearFitQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearFitQueue.Image = Global.SpectroscopyManager.My.Resources.Resources.abort_25
        Me.btnClearFitQueue.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnClearFitQueue.Location = New System.Drawing.Point(403, 288)
        Me.btnClearFitQueue.Name = "btnClearFitQueue"
        Me.btnClearFitQueue.Size = New System.Drawing.Size(75, 47)
        Me.btnClearFitQueue.TabIndex = 1
        Me.btnClearFitQueue.Text = "clear queue"
        Me.btnClearFitQueue.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnClearFitQueue.UseVisualStyleBackColor = True
        '
        'btnPlayPause
        '
        Me.btnPlayPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPlayPause.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.btnPlayPause.Image = Global.SpectroscopyManager.My.Resources.Resources.play_16
        Me.btnPlayPause.Location = New System.Drawing.Point(406, 19)
        Me.btnPlayPause.Name = "btnPlayPause"
        Me.btnPlayPause.Size = New System.Drawing.Size(65, 52)
        Me.btnPlayPause.TabIndex = 1
        Me.btnPlayPause.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnPlayPause.UseVisualStyleBackColor = False
        '
        'btnRemoveFromQueue
        '
        Me.btnRemoveFromQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveFromQueue.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_25
        Me.btnRemoveFromQueue.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnRemoveFromQueue.Location = New System.Drawing.Point(403, 220)
        Me.btnRemoveFromQueue.Name = "btnRemoveFromQueue"
        Me.btnRemoveFromQueue.Size = New System.Drawing.Size(75, 62)
        Me.btnRemoveFromQueue.TabIndex = 1
        Me.btnRemoveFromQueue.Text = "remove from queue"
        Me.btnRemoveFromQueue.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnRemoveFromQueue.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "current fit:"
        '
        'btnGoToRunningFitWindow
        '
        Me.btnGoToRunningFitWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGoToRunningFitWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.right_25
        Me.btnGoToRunningFitWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGoToRunningFitWindow.Location = New System.Drawing.Point(372, 7)
        Me.btnGoToRunningFitWindow.Name = "btnGoToRunningFitWindow"
        Me.btnGoToRunningFitWindow.Size = New System.Drawing.Size(124, 27)
        Me.btnGoToRunningFitWindow.TabIndex = 3
        Me.btnGoToRunningFitWindow.Text = "go to fit window"
        Me.btnGoToRunningFitWindow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGoToRunningFitWindow.UseVisualStyleBackColor = True
        '
        'lblCurrentFit
        '
        Me.lblCurrentFit.AutoSize = True
        Me.lblCurrentFit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentFit.Location = New System.Drawing.Point(87, 14)
        Me.lblCurrentFit.Name = "lblCurrentFit"
        Me.lblCurrentFit.Size = New System.Drawing.Size(107, 13)
        Me.lblCurrentFit.TabIndex = 2
        Me.lblCurrentFit.Text = "lblCurrentFitName"
        '
        'wFitQueue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(508, 401)
        Me.Controls.Add(Me.btnGoToRunningFitWindow)
        Me.Controls.Add(Me.lblCurrentFit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.gbQueue)
        Me.MinimumSize = New System.Drawing.Size(524, 440)
        Me.Name = "wFitQueue"
        Me.Text = "Fit Queue"
        Me.gbQueue.ResumeLayout(False)
        Me.gbQueue.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbQueue As System.Windows.Forms.ListBox
    Friend WithEvents gbQueue As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemoveFromQueue As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnGoToRunningFitWindow As System.Windows.Forms.Button
    Friend WithEvents lblCurrentFit As System.Windows.Forms.Label
    Friend WithEvents btnClearFitQueue As System.Windows.Forms.Button
    Friend WithEvents btnPlayPause As System.Windows.Forms.Button
End Class
