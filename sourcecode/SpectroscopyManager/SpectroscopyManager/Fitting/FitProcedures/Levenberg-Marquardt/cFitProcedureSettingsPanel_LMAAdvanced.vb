Public Class cFitProcedureSettingsPanel_LMAAdvanced
    Inherits cFitProcedureSettingsPanel

    Public Property FitProcedureSettings As New cLMAFit_Advanced.cFitProcedureSettings

    ''' <summary>
    ''' Initialize the Settings.
    ''' </summary>
    Private Sub wLMAFit_Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Me.FitProcedureSettings Is Nothing Then Me.FitProcedureSettings = New cLMAFit_Advanced.cFitProcedureSettings
        ' Set the default values
        Me.txtDerivativeDelta.SetValue(Me.FitProcedureSettings.DifferentiationStep)
        Me.txtMinChiSq.SetValue(Me.FitProcedureSettings.MinDeltaChi2)
        Me.nudMaxIterations.Value = Me.FitProcedureSettings.MaxIteration
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
    Private Sub txtDerivativeDelta_TextChanged() Handles txtDerivativeDelta.ValidValueChanged
        Me.FitProcedureSettings.DifferentiationStep = Me.txtDerivativeDelta.DecimalValue
    End Sub
    Private Sub nudMaxIterations_ValueChanged(sender As Object, e As EventArgs) Handles nudMaxIterations.ValueChanged
        Me.FitProcedureSettings.MaxIteration = Convert.ToInt32(Me.nudMaxIterations.Value)
    End Sub
    Private Sub txtMinChiSq_TextChanged() Handles txtMinChiSq.ValidValueChanged
        Me.FitProcedureSettings.MinDeltaChi2 = Me.txtMinChiSq.DecimalValue
    End Sub
#End Region

End Class
