<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wFitTestGauss2
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
        Me.components = New System.ComponentModel.Container()
        Me.ckbFixHeight = New System.Windows.Forms.CheckBox()
        Me.ckbFixWidth = New System.Windows.Forms.CheckBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ZedGraphControl2 = New ZedGraph.ZedGraphControl()
        Me.ZedGraphControl1 = New ZedGraph.ZedGraphControl()
        Me.NumericTextbox1 = New SpectroscopyManager.NumericTextbox()
        Me.SuspendLayout()
        '
        'ckbFixHeight
        '
        Me.ckbFixHeight.AutoSize = True
        Me.ckbFixHeight.Location = New System.Drawing.Point(82, 614)
        Me.ckbFixHeight.Name = "ckbFixHeight"
        Me.ckbFixHeight.Size = New System.Drawing.Size(68, 17)
        Me.ckbFixHeight.TabIndex = 3
        Me.ckbFixHeight.Text = "fix height"
        Me.ckbFixHeight.UseVisualStyleBackColor = True
        '
        'ckbFixWidth
        '
        Me.ckbFixWidth.AutoSize = True
        Me.ckbFixWidth.Location = New System.Drawing.Point(12, 614)
        Me.ckbFixWidth.Name = "ckbFixWidth"
        Me.ckbFixWidth.Size = New System.Drawing.Size(64, 17)
        Me.ckbFixWidth.TabIndex = 3
        Me.ckbFixWidth.Text = "fix width"
        Me.ckbFixWidth.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(536, 12)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(531, 592)
        Me.TextBox1.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(443, 610)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ZedGraphControl2
        '
        Me.ZedGraphControl2.Location = New System.Drawing.Point(12, 311)
        Me.ZedGraphControl2.Name = "ZedGraphControl2"
        Me.ZedGraphControl2.ScrollGrace = 0.0R
        Me.ZedGraphControl2.ScrollMaxX = 0.0R
        Me.ZedGraphControl2.ScrollMaxY = 0.0R
        Me.ZedGraphControl2.ScrollMaxY2 = 0.0R
        Me.ZedGraphControl2.ScrollMinX = 0.0R
        Me.ZedGraphControl2.ScrollMinY = 0.0R
        Me.ZedGraphControl2.ScrollMinY2 = 0.0R
        Me.ZedGraphControl2.Size = New System.Drawing.Size(506, 293)
        Me.ZedGraphControl2.TabIndex = 0
        '
        'ZedGraphControl1
        '
        Me.ZedGraphControl1.Location = New System.Drawing.Point(12, 12)
        Me.ZedGraphControl1.Name = "ZedGraphControl1"
        Me.ZedGraphControl1.ScrollGrace = 0.0R
        Me.ZedGraphControl1.ScrollMaxX = 0.0R
        Me.ZedGraphControl1.ScrollMaxY = 0.0R
        Me.ZedGraphControl1.ScrollMaxY2 = 0.0R
        Me.ZedGraphControl1.ScrollMinX = 0.0R
        Me.ZedGraphControl1.ScrollMinY = 0.0R
        Me.ZedGraphControl1.ScrollMinY2 = 0.0R
        Me.ZedGraphControl1.Size = New System.Drawing.Size(506, 293)
        Me.ZedGraphControl1.TabIndex = 0
        '
        'NumericTextbox1
        '
        Me.NumericTextbox1.BackColor = System.Drawing.Color.White
        Me.NumericTextbox1.ForeColor = System.Drawing.Color.Black
        Me.NumericTextbox1.FormatDecimalPlaces = 6
        Me.NumericTextbox1.Location = New System.Drawing.Point(536, 611)
        Me.NumericTextbox1.Name = "NumericTextbox1"
        Me.NumericTextbox1.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.NumericTextbox1.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.NumericTextbox1.Size = New System.Drawing.Size(100, 20)
        Me.NumericTextbox1.TabIndex = 4
        Me.NumericTextbox1.Text = "0"
        Me.NumericTextbox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NumericTextbox1.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'wFitTestGauss2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1079, 648)
        Me.Controls.Add(Me.NumericTextbox1)
        Me.Controls.Add(Me.ckbFixHeight)
        Me.Controls.Add(Me.ckbFixWidth)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ZedGraphControl2)
        Me.Controls.Add(Me.ZedGraphControl1)
        Me.FadeOpacity = 75.0R
        Me.Name = "wFitTestGauss2"
        Me.Opacity = 1.0R
        Me.Text = "wFitTest Gauss LMA"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ZedGraphControl1 As ZedGraph.ZedGraphControl
    Friend WithEvents ZedGraphControl2 As ZedGraph.ZedGraphControl
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ckbFixWidth As System.Windows.Forms.CheckBox
    Friend WithEvents ckbFixHeight As System.Windows.Forms.CheckBox
    Friend WithEvents NumericTextbox1 As SpectroscopyManager.NumericTextbox
End Class
