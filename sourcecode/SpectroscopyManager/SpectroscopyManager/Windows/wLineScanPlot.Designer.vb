<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wLineScanPlot
    Inherits SpectroscopyManager.wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wLineScanPlot))
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lvLineScanViewer = New SpectroscopyManager.mLineScanViewer()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(1114, 588)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(60, 23)
        Me.btnClose.TabIndex = 13
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lvLineScanViewer
        '
        Me.lvLineScanViewer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvLineScanViewer.Location = New System.Drawing.Point(1, 1)
        Me.lvLineScanViewer.Name = "lvLineScanViewer"
        Me.lvLineScanViewer.Size = New System.Drawing.Size(1174, 581)
        Me.lvLineScanViewer.TabIndex = 0
        '
        'wLineScanPlot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1177, 614)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lvLineScanViewer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(454, 292)
        Me.Name = "wLineScanPlot"
        Me.Text = "Create Linescan Plot"
        Me.Controls.SetChildIndex(Me.lvLineScanViewer, 0)
        Me.Controls.SetChildIndex(Me.btnClose, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvLineScanViewer As SpectroscopyManager.mLineScanViewer
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
