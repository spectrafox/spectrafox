<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cNumericSmoothingPanel_AdjacentAverageSmooth
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudNeighbors = New System.Windows.Forms.NumericUpDown()
        CType(Me.nudNeighbors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "No. of nearest neighbors:"
        '
        'nudNeighbors
        '
        Me.nudNeighbors.Location = New System.Drawing.Point(140, 3)
        Me.nudNeighbors.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudNeighbors.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudNeighbors.Name = "nudNeighbors"
        Me.nudNeighbors.Size = New System.Drawing.Size(62, 20)
        Me.nudNeighbors.TabIndex = 1
        Me.nudNeighbors.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cNumericSmoothingPanel_AdjacentAverageSmooth
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.nudNeighbors)
        Me.Controls.Add(Me.Label1)
        Me.Name = "cNumericSmoothingPanel_AdjacentAverageSmooth"
        Me.Size = New System.Drawing.Size(212, 26)
        CType(Me.nudNeighbors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents nudNeighbors As NumericUpDown
End Class
