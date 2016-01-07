''' <summary>
''' Not abstract, since we want to use the Designer to style the settings.
''' </summary>
Public Class cFitProcedureSettingsPanel
    Inherits UserControl
    Friend WithEvents nudMaxIterations As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblMaxIterations As System.Windows.Forms.Label
    Friend WithEvents lblChangeOfChi2 As System.Windows.Forms.Label
    Friend WithEvents txtMinChiSq As SpectroscopyManager.NumericTextbox
    Friend WithEvents gbStopConditions_General As System.Windows.Forms.GroupBox

    ''' <summary>
    ''' Returns the current settings chosen for the fit procedure.
    ''' OVERRIDE THIS!!!
    ''' </summary>
    Public Overridable ReadOnly Property SelectedFitSettings As iFitProcedureSettings
        Get
            Return Nothing
        End Get
    End Property

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Me.gbStopConditions_General = New System.Windows.Forms.GroupBox()
        Me.txtMinChiSq = New SpectroscopyManager.NumericTextbox()
        Me.lblChangeOfChi2 = New System.Windows.Forms.Label()
        Me.lblMaxIterations = New System.Windows.Forms.Label()
        Me.nudMaxIterations = New System.Windows.Forms.NumericUpDown()
        Me.gbStopConditions_General.SuspendLayout()
        CType(Me.nudMaxIterations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbStopConditions_General
        '
        Me.gbStopConditions_General.Controls.Add(Me.txtMinChiSq)
        Me.gbStopConditions_General.Controls.Add(Me.lblChangeOfChi2)
        Me.gbStopConditions_General.Controls.Add(Me.lblMaxIterations)
        Me.gbStopConditions_General.Controls.Add(Me.nudMaxIterations)
        Me.gbStopConditions_General.Location = New System.Drawing.Point(3, 3)
        Me.gbStopConditions_General.Name = "gbStopConditions_General"
        Me.gbStopConditions_General.Size = New System.Drawing.Size(213, 77)
        Me.gbStopConditions_General.TabIndex = 27
        Me.gbStopConditions_General.TabStop = False
        Me.gbStopConditions_General.Text = "stop conditions of the algorithm"
        '
        'txtMinChiSq
        '
        Me.txtMinChiSq.BackColor = System.Drawing.Color.White
        Me.txtMinChiSq.ForeColor = System.Drawing.Color.Black
        Me.txtMinChiSq.FormatDecimalPlaces = 6
        Me.txtMinChiSq.Location = New System.Drawing.Point(103, 46)
        Me.txtMinChiSq.Name = "txtMinChiSq"
        Me.txtMinChiSq.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtMinChiSq.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtMinChiSq.Size = New System.Drawing.Size(99, 20)
        Me.txtMinChiSq.TabIndex = 23
        Me.txtMinChiSq.Text = "0.000000"
        Me.txtMinChiSq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMinChiSq.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblChangeOfChi2
        '
        Me.lblChangeOfChi2.AutoSize = True
        Me.lblChangeOfChi2.Location = New System.Drawing.Point(9, 49)
        Me.lblChangeOfChi2.Name = "lblChangeOfChi2"
        Me.lblChangeOfChi2.Size = New System.Drawing.Size(88, 13)
        Me.lblChangeOfChi2.TabIndex = 22
        Me.lblChangeOfChi2.Text = "change of Chi^2:"
        '
        'lblMaxIterations
        '
        Me.lblMaxIterations.AutoSize = True
        Me.lblMaxIterations.Location = New System.Drawing.Point(9, 22)
        Me.lblMaxIterations.Name = "lblMaxIterations"
        Me.lblMaxIterations.Size = New System.Drawing.Size(77, 13)
        Me.lblMaxIterations.TabIndex = 22
        Me.lblMaxIterations.Text = "max. iterations:"
        '
        'nudMaxIterations
        '
        Me.nudMaxIterations.Location = New System.Drawing.Point(103, 20)
        Me.nudMaxIterations.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMaxIterations.Name = "nudMaxIterations"
        Me.nudMaxIterations.Size = New System.Drawing.Size(56, 20)
        Me.nudMaxIterations.TabIndex = 21
        Me.nudMaxIterations.TabStop = False
        Me.nudMaxIterations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudMaxIterations.Value = New Decimal(New Integer() {500, 0, 0, 0})
        '
        'cFitProcedureSettingsPanel
        '
        Me.Controls.Add(Me.gbStopConditions_General)
        Me.Name = "cFitProcedureSettingsPanel"
        Me.Size = New System.Drawing.Size(220, 84)
        Me.gbStopConditions_General.ResumeLayout(False)
        Me.gbStopConditions_General.PerformLayout()
        CType(Me.nudMaxIterations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
End Class
