<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cNumericSmoothingPanel_SavitzkyGolay
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
        Me.nudNeighbors = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.nudNeighbors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'nudNeighbors
        '
        Me.nudNeighbors.Location = New System.Drawing.Point(144, 8)
        Me.nudNeighbors.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.nudNeighbors.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudNeighbors.Name = "nudNeighbors"
        Me.nudNeighbors.Size = New System.Drawing.Size(62, 20)
        Me.nudNeighbors.TabIndex = 3
        Me.nudNeighbors.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "No. of nearest neighbors:"
        '
        'cNumericSmoothingPanel_SavitzkyGolay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.nudNeighbors)
        Me.Controls.Add(Me.Label1)
        Me.Name = "cNumericSmoothingPanel_SavitzkyGolay"
        Me.Size = New System.Drawing.Size(222, 32)
        CType(Me.nudNeighbors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents nudNeighbors As NumericUpDown
    Friend WithEvents Label1 As Label
End Class
