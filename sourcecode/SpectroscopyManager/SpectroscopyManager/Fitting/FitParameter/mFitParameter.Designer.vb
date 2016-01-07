<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mFitParameter
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
        Me.components = New System.ComponentModel.Container()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pbLockParameter = New System.Windows.Forms.PictureBox()
        Me.pbFixParameter = New System.Windows.Forms.PictureBox()
        Me.cmLockParameterTo = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnUnlock = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuLblLockFactor = New System.Windows.Forms.ToolStripLabel()
        Me.cmnuTxtLockFactor = New System.Windows.Forms.ToolStripTextBox()
        Me.pbBoundaries = New System.Windows.Forms.PictureBox()
        Me.txtValue = New SpectroscopyManager.NumericTextbox()
        Me.cmnuSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        CType(Me.pbLockParameter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbFixParameter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmLockParameterTo.SuspendLayout()
        CType(Me.pbBoundaries, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(6, 6)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(23, 13)
        Me.lblTitle.TabIndex = 16
        Me.lblTitle.Text = "title"
        '
        'pbLockParameter
        '
        Me.pbLockParameter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbLockParameter.BackgroundImage = Global.SpectroscopyManager.My.Resources.Resources.chain_23
        Me.pbLockParameter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.pbLockParameter.Location = New System.Drawing.Point(225, 0)
        Me.pbLockParameter.Name = "pbLockParameter"
        Me.pbLockParameter.Size = New System.Drawing.Size(27, 26)
        Me.pbLockParameter.TabIndex = 20
        Me.pbLockParameter.TabStop = False
        '
        'pbFixParameter
        '
        Me.pbFixParameter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbFixParameter.BackgroundImage = Global.SpectroscopyManager.My.Resources.Resources.lock_12
        Me.pbFixParameter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.pbFixParameter.Location = New System.Drawing.Point(193, 0)
        Me.pbFixParameter.Name = "pbFixParameter"
        Me.pbFixParameter.Size = New System.Drawing.Size(29, 26)
        Me.pbFixParameter.TabIndex = 20
        Me.pbFixParameter.TabStop = False
        '
        'cmLockParameterTo
        '
        Me.cmLockParameterTo.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnUnlock, Me.cmnuSeparator, Me.cmnuLblLockFactor, Me.cmnuTxtLockFactor, Me.cmnuSeparator2})
        Me.cmLockParameterTo.Name = "cmLockParameterTo"
        Me.cmLockParameterTo.Size = New System.Drawing.Size(178, 104)
        '
        'cmnUnlock
        '
        Me.cmnUnlock.Name = "cmnUnlock"
        Me.cmnUnlock.Size = New System.Drawing.Size(177, 22)
        Me.cmnUnlock.Text = "unlock parameter"
        '
        'cmnuSeparator
        '
        Me.cmnuSeparator.Name = "cmnuSeparator"
        Me.cmnuSeparator.Size = New System.Drawing.Size(174, 6)
        '
        'cmnuLblLockFactor
        '
        Me.cmnuLblLockFactor.Image = Global.SpectroscopyManager.My.Resources.Resources.multiply_16
        Me.cmnuLblLockFactor.Name = "cmnuLblLockFactor"
        Me.cmnuLblLockFactor.Size = New System.Drawing.Size(117, 16)
        Me.cmnuLblLockFactor.Text = " lock using factor:"
        '
        'cmnuTxtLockFactor
        '
        Me.cmnuTxtLockFactor.Name = "cmnuTxtLockFactor"
        Me.cmnuTxtLockFactor.Size = New System.Drawing.Size(100, 23)
        Me.cmnuTxtLockFactor.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'pbBoundaries
        '
        Me.pbBoundaries.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbBoundaries.BackgroundImage = Global.SpectroscopyManager.My.Resources.Resources.boundaries_16
        Me.pbBoundaries.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.pbBoundaries.Location = New System.Drawing.Point(255, 0)
        Me.pbBoundaries.Name = "pbBoundaries"
        Me.pbBoundaries.Size = New System.Drawing.Size(27, 26)
        Me.pbBoundaries.TabIndex = 20
        Me.pbBoundaries.TabStop = False
        '
        'txtValue
        '
        Me.txtValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtValue.BackColor = System.Drawing.Color.White
        Me.txtValue.ForeColor = System.Drawing.Color.Black
        Me.txtValue.FormatDecimalPlaces = 6
        Me.txtValue.Location = New System.Drawing.Point(89, 3)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtValue.Size = New System.Drawing.Size(100, 20)
        Me.txtValue.TabIndex = 18
        Me.txtValue.Text = "0.000000"
        Me.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'cmnuSeparator2
        '
        Me.cmnuSeparator2.Name = "cmnuSeparator2"
        Me.cmnuSeparator2.Size = New System.Drawing.Size(174, 6)
        '
        'mFitParameter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pbFixParameter)
        Me.Controls.Add(Me.pbBoundaries)
        Me.Controls.Add(Me.pbLockParameter)
        Me.Controls.Add(Me.txtValue)
        Me.Controls.Add(Me.lblTitle)
        Me.MaximumSize = New System.Drawing.Size(400, 26)
        Me.Name = "mFitParameter"
        Me.Size = New System.Drawing.Size(282, 26)
        CType(Me.pbLockParameter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbFixParameter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmLockParameterTo.ResumeLayout(False)
        Me.cmLockParameterTo.PerformLayout()
        CType(Me.pbBoundaries, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtValue As SpectroscopyManager.NumericTextbox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents pbLockParameter As System.Windows.Forms.PictureBox
    Friend WithEvents pbFixParameter As System.Windows.Forms.PictureBox
    Friend WithEvents cmLockParameterTo As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnUnlock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pbBoundaries As System.Windows.Forms.PictureBox
    Friend WithEvents cmnuSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuLblLockFactor As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cmnuTxtLockFactor As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents cmnuSeparator2 As System.Windows.Forms.ToolStripSeparator

End Class
