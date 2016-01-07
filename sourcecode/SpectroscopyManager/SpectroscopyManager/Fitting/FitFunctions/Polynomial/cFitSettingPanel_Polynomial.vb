Public Class cFitSettingPanel_Polynomial
    Inherits cFitSettingPanel

#Region "Constructor"
    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As cFitFunction_Polynomial.FitParameterIdentifier

        ' apply the last settings
        CurrentIdentifier = cFitFunction_Polynomial.FitParameterIdentifier.Y0
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Polynomial.FitParameterIdentifier.XCenter
        Me.fpXCenter.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Polynomial.FitParameterIdentifier.XAmplitude
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Polynomial.FitParameterIdentifier.a
        Me.fpFactorA.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Polynomial.FitParameterIdentifier.b
        Me.fpFactorB.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Polynomial.FitParameterIdentifier.c
        Me.fpFactorC.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Polynomial.FitParameterIdentifier.d
        Me.fpFactorD.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Polynomial.FitParameterIdentifier.XCenter.ToString, Me.fpXCenter)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Polynomial.FitParameterIdentifier.Y0.ToString, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Polynomial.FitParameterIdentifier.XAmplitude.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Polynomial.FitParameterIdentifier.a.ToString, Me.fpFactorA)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Polynomial.FitParameterIdentifier.b.ToString, Me.fpFactorB)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Polynomial.FitParameterIdentifier.c.ToString, Me.fpFactorC)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Polynomial.FitParameterIdentifier.d.ToString, Me.fpFactorD)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_Polynomial.FitParameterIdentifier.Y0.ToString, 0D)
        ' END: Registerin finished
        '#############################################
        Return True
    End Function
#End Region

#Region "Parameter-Selection by Range-Selection"

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectXAmplitude_Click(sender As Object, e As EventArgs) Handles btnSelectAmplitude.Click
        MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeXAmplitudeFromXRange)
    End Sub

    ''' <summary>
    ''' Changes the Parameters by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeXAmplitudeFromXRange(LeftValue As Double, RightValue As Double)
        Me.fpAmplitude.SetValue(RightValue - LeftValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectParameterA_Click(sender As Object, e As EventArgs) Handles btnSelectParameterA.Click
        MyBase.RaiseYRangeSelectionRequest(AddressOf Me.ChangeParameterAFromYRange)
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectParameterB_Click(sender As Object, e As EventArgs) Handles btnSelectParameterB.Click
        MyBase.RaiseYRangeSelectionRequest(AddressOf Me.ChangeParameterBFromYRange)
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectParameterC_Click(sender As Object, e As EventArgs) Handles btnSelectParameterC.Click
        MyBase.RaiseYRangeSelectionRequest(AddressOf Me.ChangeParameterCFromYRange)
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectParameterD_Click(sender As Object, e As EventArgs) Handles btnSelectParameterD.Click
        MyBase.RaiseYRangeSelectionRequest(AddressOf Me.ChangeParameterDFromYRange)
    End Sub

    ''' <summary>
    ''' Changes the Parameters by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeParameterAFromYRange(UpperValue As Double, LowerValue As Double)
        Me.fpFactorA.SetValue(UpperValue - LowerValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the Parameters by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeParameterBFromYRange(UpperValue As Double, LowerValue As Double)
        Me.fpFactorB.SetValue(UpperValue - LowerValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the Parameters by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeParameterCFromYRange(UpperValue As Double, LowerValue As Double)
        Me.fpFactorC.SetValue(UpperValue - LowerValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the Parameters by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeParameterDFromYRange(UpperValue As Double, LowerValue As Double)
        Me.fpFactorD.SetValue(UpperValue - LowerValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectYOffset_Click(sender As Object, e As EventArgs) Handles btnSelectYOffset.Click
        MyBase.RaiseYValueSelectionRequest(AddressOf Me.ChangeYOffsetFromYValue)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeYOffsetFromYValue(YValue As Double)
        Me.fpYOffset.SetValue(YValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectXCenter_Click(sender As Object, e As EventArgs) Handles btnSelectXCenter.Click
        MyBase.RaiseXValueSelectionRequest(AddressOf Me.ChangeXCenterFromXValue)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeXCenterFromXValue(XValue As Double)
        Me.fpXCenter.SetValue(XValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

#End Region

End Class
