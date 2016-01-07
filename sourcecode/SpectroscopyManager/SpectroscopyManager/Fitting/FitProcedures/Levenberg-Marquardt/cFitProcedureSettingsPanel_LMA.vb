Public Class cFitProcedureSettingsPanel_LMA
    Inherits cFitProcedureSettingsPanel

    Public Property FitProcedureSettings As New cLMAFit.cFitProcedureSettings
    Public bReady As Boolean = False

    ''' <summary>
    ''' Initialize the Settings.
    ''' </summary>
    Private Sub wLMAFit_Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Me.FitProcedureSettings Is Nothing Then Me.FitProcedureSettings = New cLMAFit.cFitProcedureSettings
        ' Set the default values
        Me.txtDerivativeDelta.SetValue(Me.FitProcedureSettings.DifferentiationStep)
        Me.txtMinChiSq.SetValue(Me.FitProcedureSettings.MinDeltaChi2)
        Me.nudMaxIterations.Value = Me.FitProcedureSettings.MaxIteration
        Me.ckbUseCenteredDifferentiation.Checked = Me.FitProcedureSettings.UseCenterDifferentiationJacobianCalculation
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
    Private Sub txtDerivativeDelta_TextChanged() Handles txtDerivativeDelta.ValidValueChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.DifferentiationStep = Me.txtDerivativeDelta.DecimalValue
    End Sub
    Private Sub nudMaxIterations_ValueChanged(sender As Object, e As EventArgs) Handles nudMaxIterations.ValueChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.MaxIteration = Convert.ToInt32(Me.nudMaxIterations.Value)
    End Sub
    Private Sub txtMinChiSq_TextChanged() Handles txtMinChiSq.ValidValueChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.MinDeltaChi2 = Me.txtMinChiSq.DecimalValue
    End Sub
    Private Sub ckbUseCenteredDifferentiation_CheckedChanged(sender As Object, e As EventArgs) Handles ckbUseCenteredDifferentiation.CheckedChanged
        If Not bReady Then Return
        Me.FitProcedureSettings.UseCenterDifferentiationJacobianCalculation = Me.ckbUseCenteredDifferentiation.Checked
    End Sub
#End Region
End Class
