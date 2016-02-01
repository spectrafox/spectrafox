Public Class cFitSettingPanel_IETS_SpinExcitation
    Inherits cFitSettingPanel

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As New cFitFunction_IETS_SpinExcitation

    ''' <summary>
    ''' Initialization complete?
    ''' </summary>
    Public bReady As Boolean = False

#Region "Constructor"

    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        '#######################################
        ' Write the current parameter settings:

        Dim CurrentIdentifier As String

        ' apply the last settings
        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.Amplitude.ToString
        Me.fpAmplitude.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.LinearSlope.ToString
        Me.fpLinearSlope.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.D.ToString
        Me.fpIETS_D.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.E.ToString
        Me.fpIETS_E.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.Temperature.ToString
        Me.fpTemperature.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.Y0.ToString
        Me.fpYOffset.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.XCenter.ToString
        Me.fpXCenter.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BField.ToString
        Me.fpMagneticField.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAngleTheta.ToString
        Me.fpMagneticFieldAngleTheta.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAnglePhi.ToString
        Me.fpMagneticFieldAnglePhi.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        CurrentIdentifier = cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.g.ToString
        Me.fpIETS_gFactor.BindToFitParameter(Me._FitFunction.FitParameters.Parameter(CurrentIdentifier), Me._FitFunction.FitParametersGrouped)

        ' End of "Write the current parameter settings"
        '################################################

        '#############################################
        ' Register Parameter-Textboxes and Checkboxes
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.Amplitude.ToString, Me.fpAmplitude)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.LinearSlope.ToString, Me.fpLinearSlope)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.XCenter.ToString, Me.fpXCenter)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.Y0.ToString, Me.fpYOffset)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.D.ToString, Me.fpIETS_D)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.E.ToString, Me.fpIETS_E)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.Temperature.ToString, Me.fpTemperature)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BField.ToString, Me.fpMagneticField)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAngleTheta.ToString, Me.fpMagneticFieldAngleTheta)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.BAnglePhi.ToString, Me.fpMagneticFieldAnglePhi)
        MyBase.RegisterFitParameterSettingsBox(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.g.ToString, Me.fpIETS_gFactor)

        MyBase.RegisterLockedFitParameterOnMultipleFit(cFitFunction_IETS_SpinExcitation.FitParameterIdentifierSpinExc.Y0.ToString, 0D)
        ' END: Registerin finished
        '#############################################

        '###################################
        ' Write the Data-Type Combobox
        With Me.cbSignalType
            .Items.Add(New KeyValuePair(Of cFitFunction_IETS_SpinExcitation.SignalTypes, String)(cFitFunction_IETS_SpinExcitation.SignalTypes.I, My.Resources.rFitFunction_IETS_SpinExcitation.SignalTypes_I))
            .Items.Add(New KeyValuePair(Of cFitFunction_IETS_SpinExcitation.SignalTypes, String)(cFitFunction_IETS_SpinExcitation.SignalTypes.dIdV, My.Resources.rFitFunction_IETS_SpinExcitation.SignalTypes_dIdV))
            .ValueMember = "Key"
            .DisplayMember = "Value"

            ' Select the initial signal type (should be dI/dV)
            Me.SetSelectedSignalType(Me._FitFunction.SignalType)
        End With
        ' Initialize the spin combobox
        With Me.cbSpin
            For i As Integer = 1 To 20 Step 1
                .Items.Add(New KeyValuePair(Of Integer, String)(i, (i * 0.5).ToString("N1")))
            Next
            .ValueMember = "Key"
            .DisplayMember = "Value"

            ' Select the initial spin
            Me.SelectedSpinInOneHalfs = Me._FitFunction.SpinInOneHalfs
        End With
        ' END: Write the Data-Type Combobox
        '###################################

        Me.bReady = True
        Return True
    End Function
#End Region

#Region "Parameter-Selection by Range-Selection"

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

#Region "Additional Settings, not belonging to a certain FitParameter"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_IETS_FitFunctionInitialized() Handles MyBase.FitFunctionInitialized
        If Me.DesignMode Then Return
        If MyBase._FitFunction Is Nothing Then Return

        Me._FitFunction = DirectCast(MyBase._FitFunction, cFitFunction_IETS_SpinExcitation)

        ' Set the initial settings of the fit-function.
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.SetValue(Me._FitFunction.dEInterpolation_CurrentPrecalculation)
        Me.txtSettings_CalculateValuesBiasLowerRange.SetValue(Me._FitFunction.CalculateForBiasRangeLowerE)
        Me.txtSettings_CalculateValuesBiasUpperRange.SetValue(Me._FitFunction.CalculateForBiasRangeUpperE)
        Me.txtSettings_ConvolutionIntegralE_NEG.SetValue(Me._FitFunction.ConvolutionIntegralE_NEG)
        Me.txtSettings_ConvolutionIntegralE_POS.SetValue(Me._FitFunction.ConvolutionIntegralE_POS)
        Me.txtSettings_ConvolutionIntegrationStepSize.SetValue(Me._FitFunction.ConvolutionIntegrationStepSize)

    End Sub

    ''' <summary>
    ''' Sets and gets the selected spin in the interface combobox.
    ''' </summary>
    Public Property SelectedSpinInOneHalfs As Integer
        Get
            Return Me.cbSpin.SelectedIndex + 1
        End Get
        Set(value As Integer)
            If Me.cbSpin.Items.Count > value Then
                Me.cbSpin.SelectedIndex = value - 1
            End If
        End Set
    End Property

    ''' <summary>
    ''' Change the selected spin.
    ''' </summary>
    Private Sub cbSpin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSpin.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' the selected spin is always one larger in the index
        Me._FitFunction.SpinInOneHalfs = Me.SelectedSpinInOneHalfs
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Sets and gets the selected signal type in the interface combobox.
    ''' </summary>
    Public Function GetSelectedSignalType() As cFitFunction_IETS_SpinExcitation.SignalTypes
        Try
            If Me.cbSignalType.Items.Count <= 0 Then Return cFitFunction_IETS_SpinExcitation.SignalTypes.I
            Return CType(Me.cbSignalType.Items(Me.cbSignalType.SelectedIndex), KeyValuePair(Of cFitFunction_IETS_SpinExcitation.SignalTypes, String)).Key
        Catch ex As Exception
            Return cFitFunction_IETS_SpinExcitation.SignalTypes.I
        End Try
    End Function

    ''' <summary>
    ''' Sets and gets the selected signal type in the interface combobox.
    ''' </summary>
    Public Sub SetSelectedSignalType(value As cFitFunction_IETS_SpinExcitation.SignalTypes)
        If Me.cbSignalType.Items.Count <= 0 Then Return
        Select Case value
            Case cFitFunction_IETS_SpinExcitation.SignalTypes.I
                Me.cbSignalType.SelectedIndex = 0
            Case cFitFunction_IETS_SpinExcitation.SignalTypes.dIdV
                Me.cbSignalType.SelectedIndex = 1
        End Select
    End Sub

    ''' <summary>
    ''' Change the selected signal type.
    ''' </summary>
    Private Sub cbSignalType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSignalType.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' get the selected signal type 
        With Me.cbSignalType
            Me._FitFunction.SignalType = GetSelectedSignalType()
            Me.RaiseParameterChangedEvent()
        End With
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_ConvolutionIntegralE_POS_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_ConvolutionIntegralE_POS.ValidValueChanged
        Me._FitFunction.ConvolutionIntegralE_POS = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_ConvolutionIntegralE_NEG_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_ConvolutionIntegralE_NEG.ValidValueChanged
        Me._FitFunction.ConvolutionIntegralE_NEG = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_ConvolutionIntegrationStepSize_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_ConvolutionIntegrationStepSize.ValidValueChanged
        Me._FitFunction.ConvolutionIntegrationStepSize = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_CalculateValuesBiasUpperRange_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_CalculateValuesBiasUpperRange.ValidValueChanged
        Me._FitFunction.CalculateForBiasRangeUpperE = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_CalculateValuesBiasLowerRange_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_CalculateValuesBiasLowerRange.ValidValueChanged
        Me._FitFunction.CalculateForBiasRangeLowerE = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_CalculateValuesBiasInterpolationStepWidth_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_CalculateValuesBiasInterpolationStepWidth.ValidValueChanged
        Me._FitFunction.dEInterpolation_CurrentPrecalculation = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

#End Region

End Class
