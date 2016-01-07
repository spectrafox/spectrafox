Public Class cFitSettingPanel_BCSDoubleGap_TipSubGapPeaks
    Inherits cFitSettingPanel_SubGapPeaks

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As New cFitFunction_BCSDoubleGap_TipSubGapPeaks

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me._FitFunctionSGP = Me._FitFunction
    End Sub

#Region "Constructor"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_BCSDoubleGap_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' Set the Fit-Function of the BaseClass
        MyBase._FitFunction = Me._FitFunction
        ' Tell others, that the fit function finally has been initialized!
        MyBase.OnFitFunctionInitialized()

        ' Initialize the Layout
        MyBase.Initialize()

        ' Bind all fit-parameters to the GUI
        Me.BindFitParameters()
    End Sub

    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean

        ' Initialize the Fit-Function user independent settings
        Me._FitFunction.ConvolutionFunction = cFitFunction_TipSampleConvolutionBase.ConvolutionFunctions.Gauss

        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As Integer

        ' apply the last settings
        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.SystemBroadening
        Me.fpBroadeningWidth.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.Delta_Sample1
        Me.fpSampleGap1.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.Delta_Sample2
        Me.fpSampleGap2.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.Delta_Tip
        Me.fpTipGap.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.GlobalXOffset
        Me.fpXOffset.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.GlobalYOffset
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSBase.BCSAmplitude
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSBase.ImaginaryDamping
        Me.fpImaginaryDamping.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2
        Me.fpSampleGapRatio.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.T_sample
        Me.fpSampleTemperature.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        CurrentIdentifier = cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.T_tip
        Me.fpTipTemperature.BindToFitParameter(Me._FitFunction.FitParameters(CurrentIdentifier), Me._FitFunction.FitParameters)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.SystemBroadening, Me.fpBroadeningWidth)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.Delta_Sample1, Me.fpSampleGap1)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.Delta_Sample2, Me.fpSampleGap2)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.Delta_Tip, Me.fpTipGap)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.GlobalXOffset, Me.fpXOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.GlobalYOffset, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSBase.BCSAmplitude, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSBase.ImaginaryDamping, Me.fpImaginaryDamping)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifierBCSDoubleGap.RatioSample1ToSample2, Me.fpSampleGapRatio)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.T_sample, Me.fpSampleTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.T_tip, Me.fpTipTemperature)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_BCSDoubleGap_TipSubGapPeaks.FitParameterIdentifier.GlobalYOffset, 0D)
        ' END: Registerin finished
        '#############################################

        Return MyBase.BindFitParameters
    End Function
#End Region

#Region "Parameter-Selection by Range-Selection"

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnXRange_Click(sender As Object, e As EventArgs) _
        Handles btnSelect_BroadeningWidth.Click,
                btnSelect_Sample_Gap2.Click,
                btnSelect_Sample_Gap1.Click
        If sender Is btnSelect_BroadeningWidth Then
            MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeBroadeningWidthFromXRange)
        ElseIf sender Is btnSelect_Sample_Gap1 Then
            MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeSampleGap1FromXRange)
        ElseIf sender Is btnSelect_Sample_Gap2 Then
            MyBase.RaiseXRangeSelectionRequest(AddressOf Me.ChangeSampleGap2FromXRange)
        End If
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeBroadeningWidthFromXRange(LeftValue As Double, RightValue As Double)
        Me.fpBroadeningWidth.SetValue(RightValue - LeftValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the sample gap by a selected range in the preview-image of the fit-window.
    ''' The value taken is referenced to the current tip gap!
    ''' </summary>
    Private Sub ChangeSampleGap1FromXRange(LeftValue As Double, RightValue As Double)
        Me.fpSampleGap1.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap.DecimalValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the sample gap by a selected range in the preview-image of the fit-window.
    ''' The value taken is referenced to the current tip gap!
    ''' </summary>
    Private Sub ChangeSampleGap2FromXRange(LeftValue As Double, RightValue As Double)
        Me.fpSampleGap2.SetValue((RightValue - LeftValue) / 2 - Me.fpTipGap.DecimalValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Initialize the range-selection.
    ''' </summary>
    Private Sub btnSelectAmplitude_Click(sender As Object, e As EventArgs)
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
    Private Sub btnSelectYOffset_Click(sender As Object, e As EventArgs)
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
    Private Sub btnSelectXCenter_Click(sender As Object, e As EventArgs)
        MyBase.RaiseXValueSelectionRequest(AddressOf Me.ChangeXCenterFromXValue)
    End Sub

    ''' <summary>
    ''' Changes the width by a selected range in the preview-image of the fit-window.
    ''' </summary>
    Private Sub ChangeXCenterFromXValue(XValue As Double)
        Me.fpXOffset.SetValue(-XValue)
        MyBase.RaiseParameterChangedEvent()
    End Sub

#End Region

End Class
