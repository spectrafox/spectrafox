Public Class cFitProcedureSettingsPanel_NMA
    Inherits cFitProcedureSettingsPanel

    Public Property FitProcedureSettings As New cNMAFit.cFitProcedureSettings
    Public bReady As Boolean = False

    ''' <summary>
    ''' Initialize the Settings.
    ''' </summary>
    Private Sub wNMAFit_Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Me.FitProcedureSettings Is Nothing Then Me.FitProcedureSettings = New cNMAFit.cFitProcedureSettings

        ' Set the default values
        Me.gbcSimulatedAnnealing.Checked = Me.FitProcedureSettings.UseSimulatedAnnealing
        Me.txtAnnealingSteps.SetValue(Me.FitProcedureSettings.SimulatedAnnealingSteps)
        Me.txtAnnealingTemperature.SetValue(Me.FitProcedureSettings.SimulatedAnnealingStartTemperature)
        Me.txtMinChiSq.SetValue(Me.FitProcedureSettings.StopCondition_MinChi2Change)
        Me.nudMaxIterations.Value = Me.FitProcedureSettings.StopCondition_MaxIterations

        Me.rbSimulatedAnnealingCoolingType_Linear.Checked = (Me.FitProcedureSettings.SimulatedAnnealingCoolingType = cNMAFit.cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Linear)
        Me.rbSimulatedAnnealingCoolingType_Exponential.Checked = (Me.FitProcedureSettings.SimulatedAnnealingCoolingType = cNMAFit.cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Exponential)

        bReady = True
    End Sub

    ''' <summary>
    ''' Return the Settings
    ''' </summary>
    Public Overrides ReadOnly Property SelectedFitSettings As iFitProcedureSettings
        Get
            Return FitProcedureSettings
        End Get
    End Property

#Region "Save Values"
    Private Sub txtAnnealingTemperature_TextChanged() Handles txtAnnealingTemperature.ValidValueChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.SimulatedAnnealingStartTemperature = Me.txtAnnealingTemperature.DecimalValue
    End Sub
    Private Sub txtAnnealingSteps_TextChanged() Handles txtAnnealingSteps.ValidValueChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.SimulatedAnnealingSteps = Me.txtAnnealingSteps.IntValue
    End Sub
    Private Sub nudMaxIterations_ValueChanged(sender As Object, e As EventArgs) Handles nudMaxIterations.ValueChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.StopCondition_MaxIterations = Convert.ToInt32(Me.nudMaxIterations.Value)
    End Sub
    Private Sub txtMinChiSq_TextChanged() Handles txtMinChiSq.ValidValueChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.StopCondition_MinChi2Change = Me.txtMinChiSq.DecimalValue
    End Sub
    Private Sub gbcSimulatedAnnealing_CheckChanged() Handles gbcSimulatedAnnealing.CheckChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.UseSimulatedAnnealing = Me.gbcSimulatedAnnealing.Checked
    End Sub
    Private Sub rbSimulatedAnnealingCoolingType_CheckedChanged(sender As Object, e As EventArgs) Handles rbSimulatedAnnealingCoolingType_Linear.CheckedChanged, rbSimulatedAnnealingCoolingType_Exponential.CheckedChanged
        If Not bReady Then Return
        If Me.rbSimulatedAnnealingCoolingType_Linear.Checked Then
            Me.FitProcedureSettings.SimulatedAnnealingCoolingType = cNMAFit.cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Linear
        ElseIf Me.rbSimulatedAnnealingCoolingType_Exponential.Checked Then
            Me.FitProcedureSettings.SimulatedAnnealingCoolingType = cNMAFit.cFitProcedureSettings.SimulatedAnnealingCoolingTypes.Exponential
        End If
    End Sub
#End Region
End Class
