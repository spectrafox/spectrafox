<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wTest
    Inherits System.Windows.Forms.Form

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
        Me.vpList = New SpectroscopyManager.VirtualVerticalPanel()
        Me.vListScroll = New System.Windows.Forms.VScrollBar()
        Me.lblVisibleControlsHeader = New System.Windows.Forms.Label()
        Me.lblVisibleControls = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'vpList
        '
        Me.vpList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vpList.Location = New System.Drawing.Point(0, 20)
        Me.vpList.Name = "vpList"
        Me.vpList.ScrollOffset = 0
        Me.vpList.Size = New System.Drawing.Size(784, 583)
        Me.vpList.TabIndex = 0
        Me.vpList.VirtualHeightOfThePanel = 100
        '
        'vListScroll
        '
        Me.vListScroll.Dock = System.Windows.Forms.DockStyle.Right
        Me.vListScroll.Location = New System.Drawing.Point(766, 0)
        Me.vListScroll.Name = "vListScroll"
        Me.vListScroll.Size = New System.Drawing.Size(18, 603)
        Me.vListScroll.TabIndex = 1
        '
        'lblVisibleControlsHeader
        '
        Me.lblVisibleControlsHeader.AutoSize = True
        Me.lblVisibleControlsHeader.Location = New System.Drawing.Point(2, 4)
        Me.lblVisibleControlsHeader.Name = "lblVisibleControlsHeader"
        Me.lblVisibleControlsHeader.Size = New System.Drawing.Size(79, 13)
        Me.lblVisibleControlsHeader.TabIndex = 2
        Me.lblVisibleControlsHeader.Text = "visible controls:"
        '
        'lblVisibleControls
        '
        Me.lblVisibleControls.AutoSize = True
        Me.lblVisibleControls.Location = New System.Drawing.Point(83, 4)
        Me.lblVisibleControls.Name = "lblVisibleControls"
        Me.lblVisibleControls.Size = New System.Drawing.Size(14, 13)
        Me.lblVisibleControls.TabIndex = 2
        Me.lblVisibleControls.Text = "#"
        '
        'wTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 603)
        Me.Controls.Add(Me.lblVisibleControls)
        Me.Controls.Add(Me.lblVisibleControlsHeader)
        Me.Controls.Add(Me.vListScroll)
        Me.Controls.Add(Me.vpList)
        Me.Name = "wTest"
        Me.Text = "wTest"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vpList As SpectroscopyManager.VirtualVerticalPanel
    Friend WithEvents vListScroll As System.Windows.Forms.VScrollBar
    Friend WithEvents lblVisibleControlsHeader As System.Windows.Forms.Label
    Friend WithEvents lblVisibleControls As System.Windows.Forms.Label
End Class
