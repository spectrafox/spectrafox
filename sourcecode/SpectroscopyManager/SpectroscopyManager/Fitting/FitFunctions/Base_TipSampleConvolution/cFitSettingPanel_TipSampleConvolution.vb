Public Class cFitSettingPanel_TipSampleConvolution
    Inherits cFitSettingPanel

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As cFitFunction_TipSampleConvolutionBase

    ''' <summary>
    ''' Dummy constructor
    ''' </summary>
    Public Sub New()
        ' This call is required by the designer.
        Me.InitializeComponent()
    End Sub

    ''' <summary>
    ''' Initialization variable
    ''' </summary>
    Public bReady As Boolean = False

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_Convolution_FitFunctionInitialized() Handles MyBase.FitFunctionInitialized
        If Me.DesignMode Then Return
        If MyBase._FitFunction Is Nothing Then Return

        Me._FitFunction = DirectCast(MyBase._FitFunction, cFitFunction_TipSampleConvolutionBase)

        ' Set the initial settings of the fit-function.
        Me.txtSettings_BroadeningStepWidth.SetValue(Me._FitFunction.dE_BroadeningStepWidth)
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.SetValue(Me._FitFunction.dE_BiasStepWidth)
        Me.txtSettings_CalculateValuesBiasLowerRange.SetValue(Me._FitFunction.CalculateForBiasRangeLowerE)
        Me.txtSettings_CalculateValuesBiasUpperRange.SetValue(Me._FitFunction.CalculateForBiasRangeUpperE)
        Me.txtSettings_ConvolutionIntegralE_NEG.SetValue(Me._FitFunction.ConvolutionIntegralE_NEG)
        Me.txtSettings_ConvolutionIntegralE_POS.SetValue(Me._FitFunction.ConvolutionIntegralE_POS)
        Me.txtSettings_ConvolutionIntegrationStepSize.SetValue(Me._FitFunction.ConvolutionIntegrationStepSize)

        '###################################
        ' Write the Data-Type Combobox
        With Me.cboFitDataType
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of cFitFunction_TipSampleConvolutionBase.FitFunctionType, String)(cFitFunction_TipSampleConvolutionBase.FitFunctionType.I, My.Resources.rFitFunction_TipSampleConvolutionBase.SignalTypes_I))
            .Items.Add(New KeyValuePair(Of cFitFunction_TipSampleConvolutionBase.FitFunctionType, String)(cFitFunction_TipSampleConvolutionBase.FitFunctionType.dIdV, My.Resources.rFitFunction_TipSampleConvolutionBase.SignalTypes_dIdV))
            .ValueMember = "Key"
            .DisplayMember = "Value"

            ' Select the initial signal type (should be dI/dV)
            Me.SetSelectedSignalType(Me._FitFunction.FitDataType)
        End With

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Binds all the fit-parameters of the fit-function to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        Return True
    End Function

    ''' <summary>
    ''' Sets and gets the selected signal type in the interface combobox.
    ''' </summary>
    Public Function GetSelectedSignalType() As cFitFunction_TipSampleConvolutionBase.FitFunctionType
        Try
            If Me.cboFitDataType.Items.Count <= 0 Then Return cFitFunction_TipSampleConvolutionBase.FitFunctionType.I
            Return CType(Me.cboFitDataType.Items(Me.cboFitDataType.SelectedIndex), KeyValuePair(Of cFitFunction_TipSampleConvolutionBase.FitFunctionType, String)).Key
        Catch ex As Exception
            Return cFitFunction_TipSampleConvolutionBase.FitFunctionType.I
        End Try
    End Function

    ''' <summary>
    ''' Sets and gets the selected signal type in the interface combobox.
    ''' </summary>
    Public Sub SetSelectedSignalType(value As cFitFunction_TipSampleConvolutionBase.FitFunctionType)
        If Me.cboFitDataType.Items.Count <= 0 Then Return
        Select Case value
            Case cFitFunction_TipSampleConvolutionBase.FitFunctionType.I
                Me.cboFitDataType.SelectedIndex = 0
            Case cFitFunction_TipSampleConvolutionBase.FitFunctionType.dIdV
                Me.cboFitDataType.SelectedIndex = 1
        End Select
    End Sub

    ''' <summary>
    ''' Change the selected signal type.
    ''' </summary>
    Private Sub cbSignalType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFitDataType.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' get the selected signal type 
        With Me.cboFitDataType
            Me._FitFunction.FitDataType = GetSelectedSignalType()
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
        Me._FitFunction.dE_BiasStepWidth = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_BroadeningStepWidth_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_BroadeningStepWidth.ValidValueChanged
        Me._FitFunction.dE_BroadeningStepWidth = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

End Class
