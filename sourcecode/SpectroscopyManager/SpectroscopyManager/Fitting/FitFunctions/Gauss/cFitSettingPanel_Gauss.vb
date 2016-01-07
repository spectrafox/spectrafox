Public Class cFitSettingPanel_Gauss
    Inherits cFitSettingPanel

#Region "Constructor"
    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean

        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As cFitFunction_Gauss.FitParameterIdentifier

        ' apply the last settings
        CurrentIdentifier = cFitFunction_Gauss.FitParameterIdentifier.Amplitude
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Gauss.FitParameterIdentifier.Width
        Me.fpWidth.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Gauss.FitParameterIdentifier.Y0
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_Gauss.FitParameterIdentifier.XCenter
        Me.fpXCenter.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier.ToString), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Gauss.FitParameterIdentifier.Amplitude.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Gauss.FitParameterIdentifier.Width.ToString, Me.fpWidth)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Gauss.FitParameterIdentifier.XCenter.ToString, Me.fpXCenter)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_Gauss.FitParameterIdentifier.Y0.ToString, Me.fpYOffset)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_Gauss.FitParameterIdentifier.Y0.ToString, 0D)
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
        Me.fpWidth.SetValue(RightValue - LeftValue)
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
