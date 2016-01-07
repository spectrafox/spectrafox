Public Class cFitSettingPanel_HighTC
    Inherits cFitSettingPanel_MetalTipSampleConvolution

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As New cFitFunction_HighTC

    Public Sub New()

        ' This call is required by the designer.
        Me.InitializeComponent()

    End Sub

#Region "Constructor"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_HighTC_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Set the Fit-Function of the BaseClass
        MyBase._FitFunction = Me._FitFunction
        MyBase.OnFitFunctionInitialized()

        ' Initialize the Fit-Function user independent settings
        Me._FitFunction.ConvolutionFunction = cFitFunction_MetalTipSampleConvolutionBase.ConvolutionFunctions.Gauss

        ' bind the fit-parameters to the GUI
        Me.BindFitParameters()
    End Sub

    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As Integer

        ' apply the last settings
        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifier.SystemBroadening
        Me.fpBroadeningWidth.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifierHighTC.Delta_Sample
        Me.fpSampleGap.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifier.GlobalXOffset
        Me.fpXOffset.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifier.GlobalYOffset
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifierHighTC.BCSAmplitude
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifierHighTC.ImaginaryDamping
        Me.fpImaginaryDamping.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifier.T_sample
        Me.fpSampleTemperature.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifier.T_tip
        Me.fpTipTemperature.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_HighTC.FitParameterIdentifierHighTC.LinearFactor
        Me.fpLinearFactor.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifier.SystemBroadening, Me.fpBroadeningWidth)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifierHighTC.Delta_Sample, Me.fpSampleGap)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifier.GlobalXOffset, Me.fpXOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifier.GlobalYOffset, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifierHighTC.BCSAmplitude, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifierHighTC.ImaginaryDamping, Me.fpImaginaryDamping)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifier.T_sample, Me.fpSampleTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifier.T_tip, Me.fpTipTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_HighTC.FitParameterIdentifierHighTC.LinearFactor, Me.fpLinearFactor)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_HighTC.FitParameterIdentifier.GlobalYOffset, 0D)
        ' END: Registerin finished
        '#############################################

        ' write additional fitfunction parameter
        'Me.txtThetaIntegrationCount.SetValue(Me._FitFunction.nDOS)

        Return True
    End Function

#End Region

#Region "Parameter-Selection by Range-Selection"

    ' ''' <summary>
    ' ''' Initialize the range-selection.
    ' ''' </summary>
    'Private Sub btnXRange_Click(sender As Object, e As EventArgs) _
    '    Handles btnSelect_BroadeningWidth.Click, btnSelect_Sample_Gap.Click
    '    If sender Is btnSelect_BroadeningWidth Then
    '        MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeBroadeningWidthFromXRange)
    '    ElseIf sender Is btnSelect_Sample_Gap Then
    '        MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeSampleGap1FromXRange)
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' Changes the width by a selected range in the preview-image of the fit-window.
    ' ''' </summary>
    'Private Sub ChangeBroadeningWidthFromXRange(LeftValue As Double, RightValue As Double)
    '    Me.fpBroadeningWidth.SetValue(RightValue - LeftValue)
    '    MyBase.RaiseParameterChangedEvent()
    'End Sub

    ' ''' <summary>
    ' ''' Changes the sample gap by a selected range in the preview-image of the fit-window.
    ' ''' The value taken is referenced to the current tip gap!
    ' ''' </summary>
    'Private Sub ChangeSampleGap1FromXRange(LeftValue As Double, RightValue As Double)
    '    Me.fpSampleGap.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap.DecimalValue)
    '    MyBase.RaiseParameterChangedEvent()
    'End Sub

    ' ''' <summary>
    ' ''' Initialize the range-selection.
    ' ''' </summary>
    'Private Sub btnSelectAmplitude_Click(sender As Object, e As EventArgs)
    '    MyBase.RaiseYRangeSelectionRequest(AddressOf Me.ChangeAmplitudeFromYRange)
    'End Sub

    ' ''' <summary>
    ' ''' Changes the width by a selected range in the preview-image of the fit-window.
    ' ''' </summary>
    'Private Sub ChangeAmplitudeFromYRange(UpperValue As Double, LowerValue As Double)
    '    Me.fpAmplitude.SetValue(UpperValue - LowerValue)
    '    MyBase.RaiseParameterChangedEvent()
    'End Sub

    ' ''' <summary>
    ' ''' Initialize the range-selection.
    ' ''' </summary>
    'Private Sub btnSelectYOffset_Click(sender As Object, e As EventArgs) Handles btnSelect_YOffset.Click
    '    MyBase.RaiseYValueSelectionRequest(AddressOf Me.ChangeYOffsetFromYValue)
    'End Sub

    ' ''' <summary>
    ' ''' Changes the width by a selected range in the preview-image of the fit-window.
    ' ''' </summary>
    'Private Sub ChangeYOffsetFromYValue(YValue As Double)
    '    Me.fpYOffset.SetValue(YValue)
    '    MyBase.RaiseParameterChangedEvent()
    'End Sub

    ' ''' <summary>
    ' ''' Initialize the range-selection.
    ' ''' </summary>
    'Private Sub btnSelectXCenter_Click(sender As Object, e As EventArgs) Handles btnSelect_XOffset.Click
    '    MyBase.RaiseXValueSelectionRequest(AddressOf Me.ChangeXCenterFromXValue)
    'End Sub

    ' ''' <summary>
    ' ''' Changes the width by a selected range in the preview-image of the fit-window.
    ' ''' </summary>
    'Private Sub ChangeXCenterFromXValue(XValue As Double)
    '    Me.fpXOffset.SetValue(-XValue)
    '    MyBase.RaiseParameterChangedEvent()
    'End Sub

#End Region

End Class
