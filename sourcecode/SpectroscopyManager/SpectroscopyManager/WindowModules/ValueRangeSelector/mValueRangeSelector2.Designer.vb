<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mValueRangeSelector2
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
        Me.pValueHistogram = New System.Windows.Forms.Panel()
        Me.btnScale_FullScale = New System.Windows.Forms.Button()
        Me.lblValues = New System.Windows.Forms.Label()
        Me.btnScale_Sigma = New System.Windows.Forms.Button()
        Me.btnScale_SigmaValue = New SpectroscopyManager.NumericTextbox()
        Me.btnScale_Weighted = New System.Windows.Forms.Button()
        Me.pColorScale = New System.Windows.Forms.Panel()
        Me.lblWeights = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'pValueHistogram
        '
        Me.pValueHistogram.BackColor = System.Drawing.Color.MidnightBlue
        Me.pValueHistogram.Location = New System.Drawing.Point(3, 19)
        Me.pValueHistogram.Name = "pValueHistogram"
        Me.pValueHistogram.Size = New System.Drawing.Size(46, 302)
        Me.pValueHistogram.TabIndex = 0
        '
        'btnScale_FullScale
        '
        Me.btnScale_FullScale.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnScale_FullScale.Location = New System.Drawing.Point(55, 19)
        Me.btnScale_FullScale.Name = "btnScale_FullScale"
        Me.btnScale_FullScale.Size = New System.Drawing.Size(39, 23)
        Me.btnScale_FullScale.TabIndex = 13
        Me.btnScale_FullScale.Text = "full"
        Me.btnScale_FullScale.UseVisualStyleBackColor = True
        '
        'lblValues
        '
        Me.lblValues.AutoSize = True
        Me.lblValues.Location = New System.Drawing.Point(6, 4)
        Me.lblValues.Name = "lblValues"
        Me.lblValues.Size = New System.Drawing.Size(41, 13)
        Me.lblValues.TabIndex = 15
        Me.lblValues.Text = "values:"
        '
        'btnScale_Sigma
        '
        Me.btnScale_Sigma.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnScale_Sigma.Location = New System.Drawing.Point(55, 46)
        Me.btnScale_Sigma.Name = "btnScale_Sigma"
        Me.btnScale_Sigma.Size = New System.Drawing.Size(39, 23)
        Me.btnScale_Sigma.TabIndex = 13
        Me.btnScale_Sigma.Text = "dev."
        Me.btnScale_Sigma.UseVisualStyleBackColor = True
        '
        'btnScale_SigmaValue
        '
        Me.btnScale_SigmaValue.BackColor = System.Drawing.Color.White
        Me.btnScale_SigmaValue.ForeColor = System.Drawing.Color.Black
        Me.btnScale_SigmaValue.FormatDecimalPlaces = 6
        Me.btnScale_SigmaValue.Location = New System.Drawing.Point(55, 72)
        Me.btnScale_SigmaValue.Name = "btnScale_SigmaValue"
        Me.btnScale_SigmaValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.btnScale_SigmaValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.btnScale_SigmaValue.Size = New System.Drawing.Size(39, 20)
        Me.btnScale_SigmaValue.TabIndex = 16
        Me.btnScale_SigmaValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.btnScale_SigmaValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'btnScale_Weighted
        '
        Me.btnScale_Weighted.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnScale_Weighted.Location = New System.Drawing.Point(55, 98)
        Me.btnScale_Weighted.Name = "btnScale_Weighted"
        Me.btnScale_Weighted.Size = New System.Drawing.Size(39, 23)
        Me.btnScale_Weighted.TabIndex = 13
        Me.btnScale_Weighted.Text = "weight"
        Me.btnScale_Weighted.UseVisualStyleBackColor = True
        '
        'pColorScale
        '
        Me.pColorScale.BackColor = System.Drawing.Color.MidnightBlue
        Me.pColorScale.Location = New System.Drawing.Point(97, 19)
        Me.pColorScale.Name = "pColorScale"
        Me.pColorScale.Size = New System.Drawing.Size(46, 302)
        Me.pColorScale.TabIndex = 0
        '
        'lblWeights
        '
        Me.lblWeights.AutoSize = True
        Me.lblWeights.Location = New System.Drawing.Point(100, 4)
        Me.lblWeights.Name = "lblWeights"
        Me.lblWeights.Size = New System.Drawing.Size(35, 13)
        Me.lblWeights.TabIndex = 15
        Me.lblWeights.Text = "scale:"
        '
        'mValueRangeSelector2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnScale_SigmaValue)
        Me.Controls.Add(Me.lblWeights)
        Me.Controls.Add(Me.pColorScale)
        Me.Controls.Add(Me.lblValues)
        Me.Controls.Add(Me.pValueHistogram)
        Me.Controls.Add(Me.btnScale_Sigma)
        Me.Controls.Add(Me.btnScale_Weighted)
        Me.Controls.Add(Me.btnScale_FullScale)
        Me.DoubleBuffered = True
        Me.Name = "mValueRangeSelector2"
        Me.Size = New System.Drawing.Size(148, 324)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pValueHistogram As System.Windows.Forms.Panel
    Friend WithEvents btnScale_FullScale As System.Windows.Forms.Button
    Friend WithEvents lblValues As System.Windows.Forms.Label
    Friend WithEvents btnScale_Sigma As System.Windows.Forms.Button
    Friend WithEvents btnScale_SigmaValue As SpectroscopyManager.NumericTextbox
    Friend WithEvents btnScale_Weighted As System.Windows.Forms.Button
    Friend WithEvents pColorScale As System.Windows.Forms.Panel
    Friend WithEvents lblWeights As System.Windows.Forms.Label

End Class
