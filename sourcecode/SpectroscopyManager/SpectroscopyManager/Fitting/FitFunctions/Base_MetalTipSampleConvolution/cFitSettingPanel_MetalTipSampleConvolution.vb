Public Class cFitSettingPanel_MetalTipSampleConvolution
    Inherits cFitSettingPanel

    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Private Shadows _FitFunction As cFitFunction_MetalTipSampleConvolutionBase

    ''' <summary>
    ''' Dummy constructor
    ''' </summary>
    Public Sub New()
        ' This call is required by the designer.
        Me.InitializeComponent()
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub cFitSettingPanel_Convolution_FitFunctionInitialized() Handles MyBase.FitFunctionInitialized
        If Me.DesignMode Then Return
        If MyBase._FitFunction Is Nothing Then Return

        Me._FitFunction = DirectCast(MyBase._FitFunction, cFitFunction_MetalTipSampleConvolutionBase)

        ' Set the initial settings of the fit-function.
        Me.txtSettings_BroadeningStepWidth.SetValue(cFitFunction_MetalTipSampleConvolutionBase.dE_BroadeningStepWidth)
        Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.SetValue(Me._FitFunction.dEInterpolation_dIdVPrecalculation)
        Me.txtSettings_CalculateValuesBiasLowerRange.SetValue(Me._FitFunction.CalculateForBiasRangeLowerE)
        Me.txtSettings_CalculateValuesBiasUpperRange.SetValue(Me._FitFunction.CalculateForBiasRangeUpperE)
        Me.txtSettings_ConvolutionIntegralE_NEG.SetValue(Me._FitFunction.ConvolutionIntegralE_NEG)
        Me.txtSettings_ConvolutionIntegralE_POS.SetValue(Me._FitFunction.ConvolutionIntegralE_POS)
        Me.txtSettings_ConvolutionIntegrationStepSize.SetValue(Me._FitFunction.ConvolutionIntegrationStepSize)

    End Sub

    ''' <summary>
    ''' Binds the fit-parameters to the GUI.
    ''' </summary>
    Protected Overrides Function BindFitParameters() As Boolean
        Return MyBase.BindFitParameters()
    End Function

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
        Me._FitFunction.dEInterpolation_dIdVPrecalculation = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

    ''' <summary>
    ''' Changes the input parameters for the fit-function.
    ''' </summary>
    Private Sub txtSettings_BroadeningStepWidth_TextChanged(ByRef source As NumericTextbox) Handles txtSettings_BroadeningStepWidth.ValidValueChanged
        cFitFunction_MetalTipSampleConvolutionBase.dE_BroadeningStepWidth = source.DecimalValue
        Me._FitFunction.ForceNewPrecalculation()
        Me.RaiseParameterChangedEvent()
    End Sub

#Region "Export/Import routine for the additional function parameters (convolution step size, etc.)"

    ''' <summary>
    ''' Write additionally the parameter-settings about enabled or disabled SGPs.
    ''' </summary>
    Protected Overrides Sub Export_WriteAdditionalXMLElements(ByRef XMLWriter As Xml.XmlTextWriter)

        With XMLWriter
            ' Begin the element of the parameter description
            .WriteStartElement("dIdVIntegralConvolutionSettings") ' <CurrentIntegralConvolutionSettings>
            .WriteAttributeString("dIdVPrecalculationInterpolationStepWidth", Me._FitFunction.dEInterpolation_dIdVPrecalculation.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BiasLimitUpperE", Me._FitFunction.CalculateForBiasRangeUpperE.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BiasLimitLowerE", Me._FitFunction.CalculateForBiasRangeLowerE.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ConvolutionIntegralEPos", Me._FitFunction.ConvolutionIntegralE_POS.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ConvolutionIntegralENeg", Me._FitFunction.ConvolutionIntegralE_NEG.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("IntegrationEnergyStep", Me._FitFunction.ConvolutionIntegrationStepSize.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BroadeningStepWidth", cFitFunction_MetalTipSampleConvolutionBase.dE_BroadeningStepWidth.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteEndElement() ' <\CurrentIntegralConvolutionSettings>
        End With

        MyBase.Export_WriteAdditionalXMLElements(XMLWriter)
    End Sub

    ''' <summary>
    ''' Read unknown XML-Elements, if needed by the import.
    ''' </summary>
    Protected Overrides Sub Import_UnknownXMLElementIdentified(ByVal XMLElementName As String, ByRef XMLReader As Xml.XmlReader)
        If XMLElementName = "dIdVIntegralConvolutionSettings" Then
            With XMLReader
                If .AttributeCount > 0 Then
                    While .MoveToNextAttribute
                        Select Case .Name
                            ' extract all the current integral settings
                            Case "dIdVPrecalculationInterpolationStepWidth"
                                Me._FitFunction.dEInterpolation_dIdVPrecalculation = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                Me.txtSettings_CalculateValuesBiasInterpolationStepWidth.SetValue(Me._FitFunction.dEInterpolation_dIdVPrecalculation)
                            Case "BiasLimitUpperE"
                                Me._FitFunction.CalculateForBiasRangeUpperE = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                Me.txtSettings_CalculateValuesBiasUpperRange.SetValue(Me._FitFunction.CalculateForBiasRangeUpperE)
                            Case "BiasLimitLowerE"
                                Me._FitFunction.CalculateForBiasRangeLowerE = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                Me.txtSettings_CalculateValuesBiasLowerRange.SetValue(Me._FitFunction.CalculateForBiasRangeLowerE)
                            Case "IntegrationEnergyStep"
                                Me._FitFunction.ConvolutionIntegrationStepSize = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                Me.txtSettings_ConvolutionIntegrationStepSize.SetValue(Me._FitFunction.ConvolutionIntegrationStepSize)
                            Case "ConvolutionIntegralEPos"
                                Me._FitFunction.ConvolutionIntegralE_POS = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                Me.txtSettings_ConvolutionIntegralE_POS.SetValue(Me._FitFunction.ConvolutionIntegralE_POS)
                            Case "ConvolutionIntegralENeg"
                                Me._FitFunction.ConvolutionIntegralE_NEG = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                Me.txtSettings_ConvolutionIntegralE_NEG.SetValue(Me._FitFunction.ConvolutionIntegralE_NEG)
                            Case "BroadeningStepWidth"
                                cFitFunction_MetalTipSampleConvolutionBase.dE_BroadeningStepWidth = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                Me.txtSettings_BroadeningStepWidth.SetValue(cFitFunction_MetalTipSampleConvolutionBase.dE_BroadeningStepWidth)
                        End Select
                    End While
                End If
            End With
        Else
            ' else pass back to base-class
            MyBase.Import_UnknownXMLElementIdentified(XMLElementName, XMLReader)
        End If
    End Sub

#End Region

End Class
