Public Class cFitSettingPanel_FrotaDerivative
    Inherits cFitSettingPanel

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As cFitFunction_FrotaDerivative

    Private bReady As Boolean = False

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_Convolution_FitFunctionInitialized() Handles MyBase.FitFunctionInitialized
        If Me.DesignMode Then Return
        If MyBase._FitFunction Is Nothing Then Return

        Me._FitFunction = DirectCast(MyBase._FitFunction, cFitFunction_FrotaDerivative)

        ' Set the initial settings of the fit-function.
        Me.txtSettings_DerivativeStepSize.SetValue(Me._FitFunction.DerivativeDistance)

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As String

        ' apply the last settings
        CurrentIdentifier = cFitFunction_Frota.FitParameterIdentifier.a.ToString
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Frota.FitParameterIdentifier.Gamma.ToString
        Me.fpGamma.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Frota.FitParameterIdentifier.Phi.ToString
        Me.fpPhi.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Frota.FitParameterIdentifier.c.ToString
        Me.fpC.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Frota.FitParameterIdentifier.b.ToString
        Me.fpB.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Frota.FitParameterIdentifier.XCenter.ToString
        Me.fpXCenter.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Frota.FitParameterIdentifier.a.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Frota.FitParameterIdentifier.Gamma.ToString, Me.fpGamma)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Frota.FitParameterIdentifier.XCenter.ToString, Me.fpXCenter)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Frota.FitParameterIdentifier.c.ToString, Me.fpC)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Frota.FitParameterIdentifier.Phi.ToString, Me.fpPhi)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Frota.FitParameterIdentifier.b.ToString, Me.fpB)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_Frota.FitParameterIdentifier.c.ToString, 0D)
        ' END: Registerin finished
        '#############################################
        Return True
    End Function
#End Region

#Region "Parameter-Selection by Range-Selection"

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectWidth_Click(sender As Object, e As EventArgs) Handles btnSelectWidth.Click
        MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeWidthFromXRange)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeWidthFromXRange(LeftValue As Double, RightValue As Double)
        Me.fpGamma.SetValue(RightValue - LeftValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectAmplitude_Click(sender As Object, e As EventArgs) Handles btnSelectAmplitude.Click
        MyBase.RaiseYRangeSelectionRequest(AddressOf Me.ChangeAmplitudeFromYRange)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeAmplitudeFromYRange(UpperValue As Double, LowerValue As Double)
        Me.fpAmplitude.SetValue(UpperValue - LowerValue)
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
        Me.fpC.SetValue(YValue)
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

    ''' <summary>
    ''' Change the step size of the numeric derivative.
    ''' </summary>
    Private Sub txtSettings_DerivativeStepSize_TextChanged(ByRef sender As NumericTextbox) Handles txtSettings_DerivativeStepSize.ValidValueChanged
        If Not bReady Then Return

        Me._FitFunction.DerivativeDistance = sender.DecimalValue
        Me.RaiseParameterChangedEvent()
    End Sub

#End Region

End Class
