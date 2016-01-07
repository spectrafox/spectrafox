<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mValueRangeSelector
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
        Me.gbPlotArea = New System.Windows.Forms.GroupBox()
        Me.pPaintArea = New SpectroscopyManager.UserPaintArea()
        Me.btnFullScale = New System.Windows.Forms.Button()
        Me.lblMinValue = New System.Windows.Forms.Label()
        Me.lblMaxValue = New System.Windows.Forms.Label()
        Me.ttInfo = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnUseSigma = New System.Windows.Forms.Button()
        Me.txtSigma = New SpectroscopyManager.NumericTextbox()
        Me.txtMaxValue = New SpectroscopyManager.NumericTextbox()
        Me.txtMinValue = New SpectroscopyManager.NumericTextbox()
        Me.gbPlotArea.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbPlotArea
        '
        Me.gbPlotArea.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPlotArea.Controls.Add(Me.pPaintArea)
        Me.gbPlotArea.Location = New System.Drawing.Point(3, 3)
        Me.gbPlotArea.Name = "gbPlotArea"
        Me.gbPlotArea.Size = New System.Drawing.Size(107, 258)
        Me.gbPlotArea.TabIndex = 11
        Me.gbPlotArea.TabStop = False
        Me.gbPlotArea.Text = "histogram"
        '
        'pPaintArea
        '
        Me.pPaintArea.BackColor = System.Drawing.Color.MidnightBlue
        Me.pPaintArea.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pPaintArea.Location = New System.Drawing.Point(3, 16)
        Me.pPaintArea.Name = "pPaintArea"
        Me.pPaintArea.Size = New System.Drawing.Size(101, 239)
        Me.pPaintArea.TabIndex = 0
        Me.ttInfo.SetToolTip(Me.pPaintArea, "left-click to select maximum value" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "right-click to select minimum value")
        '
        'btnFullScale
        '
        Me.btnFullScale.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFullScale.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnFullScale.Location = New System.Drawing.Point(113, 3)
        Me.btnFullScale.Margin = New System.Windows.Forms.Padding(0)
        Me.btnFullScale.Name = "btnFullScale"
        Me.btnFullScale.Size = New System.Drawing.Size(57, 20)
        Me.btnFullScale.TabIndex = 1
        Me.btnFullScale.Text = "full scale"
        Me.btnFullScale.UseVisualStyleBackColor = True
        '
        'lblMinValue
        '
        Me.lblMinValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMinValue.AutoSize = True
        Me.lblMinValue.Location = New System.Drawing.Point(110, 90)
        Me.lblMinValue.Name = "lblMinValue"
        Me.lblMinValue.Size = New System.Drawing.Size(58, 13)
        Me.lblMinValue.TabIndex = 15
        Me.lblMinValue.Text = "min. value:"
        '
        'lblMaxValue
        '
        Me.lblMaxValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMaxValue.AutoSize = True
        Me.lblMaxValue.Location = New System.Drawing.Point(109, 129)
        Me.lblMaxValue.Name = "lblMaxValue"
        Me.lblMaxValue.Size = New System.Drawing.Size(61, 13)
        Me.lblMaxValue.TabIndex = 15
        Me.lblMaxValue.Text = "max. value:"
        '
        'btnUseSigma
        '
        Me.btnUseSigma.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUseSigma.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnUseSigma.Location = New System.Drawing.Point(113, 27)
        Me.btnUseSigma.Margin = New System.Windows.Forms.Padding(0)
        Me.btnUseSigma.Name = "btnUseSigma"
        Me.btnUseSigma.Size = New System.Drawing.Size(57, 21)
        Me.btnUseSigma.TabIndex = 2
        Me.btnUseSigma.Text = "sigma"
        Me.btnUseSigma.UseVisualStyleBackColor = True
        '
        'txtSigma
        '
        Me.txtSigma.AllowZero = True
        Me.txtSigma.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSigma.BackColor = System.Drawing.Color.White
        Me.txtSigma.ForeColor = System.Drawing.Color.Black
        Me.txtSigma.FormatDecimalPlaces = 2
        Me.txtSigma.Location = New System.Drawing.Point(114, 51)
        Me.txtSigma.Name = "txtSigma"
        Me.txtSigma.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtSigma.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtSigma.Size = New System.Drawing.Size(56, 20)
        Me.txtSigma.TabIndex = 3
        Me.txtSigma.Text = "0.000000"
        Me.txtSigma.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSigma.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtMaxValue
        '
        Me.txtMaxValue.AllowZero = True
        Me.txtMaxValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMaxValue.BackColor = System.Drawing.Color.White
        Me.txtMaxValue.ForeColor = System.Drawing.Color.Black
        Me.txtMaxValue.FormatDecimalPlaces = 3
        Me.txtMaxValue.Location = New System.Drawing.Point(111, 145)
        Me.txtMaxValue.Name = "txtMaxValue"
        Me.txtMaxValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtMaxValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtMaxValue.Size = New System.Drawing.Size(58, 20)
        Me.txtMaxValue.TabIndex = 5
        Me.txtMaxValue.Text = "0.000000"
        Me.txtMaxValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMaxValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'txtMinValue
        '
        Me.txtMinValue.AllowZero = True
        Me.txtMinValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMinValue.BackColor = System.Drawing.Color.White
        Me.txtMinValue.ForeColor = System.Drawing.Color.Black
        Me.txtMinValue.FormatDecimalPlaces = 3
        Me.txtMinValue.Location = New System.Drawing.Point(112, 106)
        Me.txtMinValue.Name = "txtMinValue"
        Me.txtMinValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtMinValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtMinValue.Size = New System.Drawing.Size(58, 20)
        Me.txtMinValue.TabIndex = 4
        Me.txtMinValue.Text = "0.000000"
        Me.txtMinValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMinValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'mValueRangeSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtSigma)
        Me.Controls.Add(Me.btnUseSigma)
        Me.Controls.Add(Me.lblMaxValue)
        Me.Controls.Add(Me.txtMaxValue)
        Me.Controls.Add(Me.lblMinValue)
        Me.Controls.Add(Me.txtMinValue)
        Me.Controls.Add(Me.btnFullScale)
        Me.Controls.Add(Me.gbPlotArea)
        Me.Name = "mValueRangeSelector"
        Me.Size = New System.Drawing.Size(171, 264)
        Me.gbPlotArea.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbPlotArea As System.Windows.Forms.GroupBox
    Friend WithEvents pPaintArea As UserPaintArea
    Friend WithEvents btnFullScale As System.Windows.Forms.Button
    Friend WithEvents txtMinValue As NumericTextbox
    Friend WithEvents lblMinValue As Label
    Friend WithEvents txtMaxValue As NumericTextbox
    Friend WithEvents lblMaxValue As Label
    Friend WithEvents ttInfo As ToolTip
    Friend WithEvents btnUseSigma As Button
    Friend WithEvents txtSigma As NumericTextbox
End Class
