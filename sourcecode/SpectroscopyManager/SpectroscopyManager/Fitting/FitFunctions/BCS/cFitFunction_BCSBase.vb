Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports System.Threading.Tasks
Public MustInherit Class cFitFunction_BCSBase
    Inherits cFitFunction_TipSampleConvolutionBase
    Implements iFitFunction_SubGapPeaks

    ''' <summary>
    ''' Abbruchwert der BCS-DOS-Funktion
    ''' </summary>
    Public Const CLOSE_TO_ONE As Double = 1.001

#Region "Initialize Fit-Parameters"
    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifierBCSBase
        ImaginaryDamping = 21
        BCSAmplitude = 22
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSBase.ImaginaryDamping.ToString, 0.000015, False, My.Resources.rFitFunction_BCSBase.Parameter_ImaginaryDamping))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifierBCSBase.BCSAmplitude.ToString, 1, False, My.Resources.rFitFunction_BCSBase.Parameter_BCSAmplitude))
        MyBase.InitializeFitParameters()
    End Sub
#End Region

#Region "Properties such as the BroadeningType"
    ''' <summary>
    ''' Changes the range in which the fit-function is defined.
    ''' Needed for convolution integrals and current integrals to estimate
    ''' the range of values to calculate the integral over.
    ''' </summary>
    Public Sub SetBCSEnergyRange(LowerValue As Double, HigherValue As Double)
        Me.ConvolutionIntegralE_NEG = LowerValue
        Me.ConvolutionIntegralE_POS = HigherValue
    End Sub
#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    Public Sub New()
        Me._FunctionImplementsCUDAVersion = True
    End Sub

#End Region

#Region "Actual BCS Function with included broadening convolution - CUDA compatible"
    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet die BCS-Zustandsdichte-Funktion                     *
    ''' // *                                                               *
    ''' // *    f(E) = |E|/sqrt( E^2 - D^2 )    :   |E| larger  |D|        *
    ''' // *           0                        :   |E| smallerequal |D|   *
    ''' // *                                                               *
    ''' // * entweder direkt (fBroad == BROAD_OFF) oder gefaltet mit einer *
    ''' // * normierten    Gauss-    oder    Lorentz-Funktion   (fBroad == *
    ''' // * BROAD_GAUSS oder BROAD_LORENTZ) g(E) mit Breite w:            *
    ''' // *                                                               *
    ''' // *      ( f * g ) (E) = int f(F) * g( E - F ) dF                 *
    ''' // *                                                               *
    ''' // * oder mit einer Lorentz-Kurve g(E) mit derselben Breite;       *
    ''' // *                                                               *
    ''' // *****************************************************************
    ''' 
    ''' CUDA compatible version
    ''' </summary>
    <Cudafy>
    Public Shared Function BCSFunc(ByVal E As Double,
                                   ByVal Delta As Double,
                                   ByVal BroadeningWidth As Double,
                                   ByVal BroadeningType As Integer,
                                   ByVal ImaginaryDamping As Double) As Double
        Return BCSDOSExpanded(E, Delta, ImaginaryDamping)
    End Function

    ''' <summary>
    ''' Actual BCS-DOS function, that may have a small imaginary damping
    ''' factor to smear out the peak structure by life-time effects.
    ''' Returns similar results, as a Lorentzian convolution! Due to 
    ''' calculation time, you should prefer the imaginary damping factor.
    ''' 
    ''' Used before, but is CUDA-incompatible and by approx. 16% slower.
    ''' </summary>
    Public Shared Function BCSDOSComplex(ByRef E As Double, ByRef Delta As Double, ByRef ImaginaryDamping As Double) As Double
        If ImaginaryDamping <> 0 Then
            ' Use complex imaginary damping factor in the energy.
            Dim complE As New Numerics.Complex(E, ImaginaryDamping)
            Dim complDelta As New Numerics.Complex(Delta, 0)

            ' Calculate the Abs.Real part of the BCS-DOS, that is seen in experiment.
            Return cNumericFunctions.MathAbs((complE / Numerics.Complex.Sqrt(complE * complE - complDelta * complDelta)).Real)
        Else
            ' Original unbroadened BCS DOS.
            If cNumericFunctions.MathAbs(E) > cNumericFunctions.MathAbs(Delta) Then
                Return cNumericFunctions.MathAbs(E) / Math.Sqrt(E * E - Delta * Delta)
            Else
                Return 0.0
            End If
        End If
    End Function

    ''' <summary>
    ''' Actual BCS-DOS function, that may have a small imaginary damping
    ''' factor to smear out the peak structure by life-time effects.
    ''' Returns similar results, as a Lorentzian convolution! Due to 
    ''' calculation time, you should prefer the imaginary damping factor.
    ''' 
    ''' Function got expanded by Mathematica to avoid the need of complex numbers.
    ''' </summary>
    <Cudafy>
    Public Shared Function BCSDOSExpanded(ByVal E As Double, ByVal Delta As Double, ByVal ImaginaryDamping As Double) As Double
        ' Calculate needed variables to save time
        Dim ESq As Double = E * E
        Dim DeltaSq As Double = Delta * Delta

        If ImaginaryDamping <> 0 Then
            Dim ImSq As Double = ImaginaryDamping * ImaginaryDamping

            Dim Arg As Double = 0.5 * Math.Atan2(2 * ImaginaryDamping * E, ESq - ImSq - DeltaSq)
            Dim S3 As Double = ImSq + DeltaSq - ESq
            Return cNumericFunctions.MathAbs((E * Math.Cos(Arg) + ImaginaryDamping * Math.Sin(Arg)) / Math.Sqrt(Math.Sqrt(4 * ImSq * ESq + S3 * S3)))
        Else
            ' Original unbroadened BCS DOS.
            If ESq > DeltaSq Then
                Return cNumericFunctions.MathAbs(E) / Math.Sqrt(ESq - DeltaSq)
            Else
                Return 0.0
            End If
        End If
    End Function

#End Region

#Region "Step Gap Function - CUDA compatible"
    ''' <summary>
    ''' Function that creates the step gap, instead of a BCS
    ''' Uses a Sigmoidal Boltzmann distribution.
    ''' </summary>
    <Cudafy>
    Public Shared Function StepGapFunction(x As Double,
                                           Width As Double,
                                           Delta As Double) As Double
        Return 1 + cNumericFunctions.SigmoidalBoltzmann(x, Delta, 0, 1, Width) - cNumericFunctions.SigmoidalBoltzmann(x, -Delta, 0, 1, Width)
    End Function
#End Region

#Region "Sub-Gap Peak Storage and Contribution Compatible"

    ''' <summary>
    ''' Set this variable to determine the type of sub-gap-peaks used in the fit.
    ''' </summary>
    Public Property SubGapPeakType As iFitFunction_SubGapPeaks.SubGapPeakTypes = iFitFunction_SubGapPeaks.SubGapPeakTypes.Lorentz Implements iFitFunction_SubGapPeaks.SubGapPeakType

    ''' <summary>
    ''' List to save all sub-gap-peaks.
    ''' 
    ''' -> changed to dictionary to keep existing SGPs at
    ''' the same position!!!
    ''' </summary>
    Private Property SubGapPeaks As New List(Of iFitFunction_SubGapPeaks.SubGapPeak) Implements iFitFunction_SubGapPeaks.SubGapPeaks

    ''' <summary>
    ''' Adds a new sub-gap peak to the fit-function.
    ''' Returns the index of the internal list, at which the peak got added.
    ''' </summary>
    Public Function AddSubGapPeak(Optional ByVal Index As Integer = -1) As Integer Implements iFitFunction_SubGapPeaks.AddSubGapPeak

        If Index < 0 Then
            Index = 0

            ' find free index for the SGP
            Dim SGPIndexOccupied As Boolean = False
            For i As Integer = 0 To Me.SubGapPeaks.Count Step 1
                SGPIndexOccupied = False
                For Each SGP As iFitFunction_SubGapPeaks.SubGapPeak In Me.SubGapPeaks
                    If SGP.SGPIndex = i Then
                        SGPIndexOccupied = True
                        Exit For
                    End If
                Next

                If SGPIndexOccupied = False Then
                    Index = i
                    Exit For
                End If
            Next
        End If

        Dim NewSGP As iFitFunction_SubGapPeaks.SubGapPeak = New iFitFunction_SubGapPeaks.SubGapPeak()
        NewSGP.SGPIndex = Index
        Me.SubGapPeaks.Add(NewSGP)
        Return Index
    End Function

    ''' <summary>
    ''' Removes the SGP from the list.
    ''' False, if the index was not found in the dictionary.
    ''' </summary>
    Public Function RemoveSubGapPeak(ByVal SubGapPeakIndex As Integer) As Boolean Implements iFitFunction_SubGapPeaks.RemoveSubGapPeak
        With Me.SubGapPeaks
            If .Count > SubGapPeakIndex Then
                .RemoveAt(SubGapPeakIndex)
                Return True
            Else
                Return False
            End If
        End With
    End Function

    ''' <summary>
    ''' Clears the list of all sub-gap-peaks.
    ''' </summary>
    Public Sub ClearSubGapPeaks() Implements iFitFunction_SubGapPeaks.ClearSubGapPeaks
        Me.SubGapPeaks.Clear()
    End Sub

    ''' <summary>
    ''' Currently registered SubGapPeaks.
    ''' </summary>
    Public ReadOnly Property RegisteredSubGapPeaks As List(Of iFitFunction_SubGapPeaks.SubGapPeak) Implements iFitFunction_SubGapPeaks.RegisteredSubGapPeaks
        Get
            Return Me.SubGapPeaks
        End Get
    End Property

    ''' <summary>
    ''' Represents a gaussian sub-gap-peak to be added to the sample-DOS.
    ''' </summary>
    <Cudafy>
    Public Shared Function SubGapPeakValue_Gauss(ByVal x As Double,
                                                 ByVal XCenter As Double,
                                                 ByVal Amplitude As Double,
                                                 ByVal Width As Double,
                                                 ByVal PosNegHeightRatio As Double) As Double
        Return Amplitude * (cNumericFunctions.GaussPeak_Amplitude_Normalized(x, XCenter, Width) _
                            + PosNegHeightRatio * cNumericFunctions.GaussPeak_Amplitude_Normalized(x, -XCenter, Width))
    End Function

    ''' <summary>
    ''' Represents a lorentzian sub-gap-peak to be added to the sample-DOS.
    ''' </summary>
    <Cudafy>
    Public Shared Function SubGapPeakValue_Lorentz(ByVal x As Double,
                                                   ByVal XCenter As Double,
                                                   ByVal Amplitude As Double,
                                                   ByVal Width As Double,
                                                   ByVal PosNegHeightRatio As Double) As Double
        Return Amplitude * (cNumericFunctions.LorentzPeak_Normalized(x, XCenter, Width) _
                            + PosNegHeightRatio * cNumericFunctions.LorentzPeak_Normalized(x, -XCenter, Width))
    End Function

    ''' <summary>
    ''' Gets the integrand contribution to the DOS created by all sub-gap peaks registered!
    ''' </summary>
    Protected Function GetSubGapPeakContribution(ByVal x As Double,
                                                 ByRef InputParameters As cFitParameterGroupGroup,
                                                 ByVal UseLorentz As Integer,
                                                 ByVal ParentDOS As Integer) As Double

        Dim SubGapPeakContribution As Double = 0

        ' Get the values by summing up all SGP contributions
        ' uses either Lorentz or Gauss
        If UseLorentz = 1 Then
            ' LORENTZ
            '#########
            For i As Integer = 0 To Me.SubGapPeaks.Count - 1 Step 1
                If Me.SubGapPeaks(i).SubGapPeakParentDOS = ParentDOS Then
                    SubGapPeakContribution += SubGapPeakValue_Lorentz(x,
                                                                      InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_XCenter(Me.SubGapPeaks(i).SGPIndex)).Value,
                                                                      InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_Amplitude(Me.SubGapPeaks(i).SGPIndex)).Value,
                                                                      InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_Width(Me.SubGapPeaks(i).SGPIndex)).Value,
                                                                      InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_PosNegRatio(Me.SubGapPeaks(i).SGPIndex)).Value)
                End If
            Next
        Else
            ' GAUSS
            '#######
            For i As Integer = 0 To Me.SubGapPeaks.Count - 1 Step 1
                If Me.SubGapPeaks(i).SubGapPeakParentDOS = ParentDOS Then
                    SubGapPeakContribution += SubGapPeakValue_Gauss(x,
                                                                    InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_XCenter(Me.SubGapPeaks(i).SGPIndex)).Value,
                                                                    InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_Amplitude(Me.SubGapPeaks(i).SGPIndex)).Value,
                                                                    InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_Width(Me.SubGapPeaks(i).SGPIndex)).Value,
                                                                    InputParameters.Group(Me.UseFitParameterGroupID).Parameter(iFitFunction_SubGapPeaks.SubGapPeak.SGPIdentifier_PosNegRatio(Me.SubGapPeaks(i).SGPIndex)).Value)
                End If
            Next
        End If
        Return SubGapPeakContribution
    End Function

#End Region

#Region "Register SubGapPeaks with the base fit-functions"
    ''' <summary>
    ''' Method that sets all FitParameters from the Sub-Gap-Peaks contained in this BCS-FitFunction.
    ''' </summary>
    Public Overrides Sub ReInitializeFitParameters() Implements iFitFunction_SubGapPeaks.ReInitializeFitParameters

        ' Remove old SGP fit-parameters from the Fit-Parameter-Array
        Dim FitParameterIdentifiersToRemove As New List(Of String)
        For Each FP As cFitParameter In Me.FitParameters

            If FP.Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPAmplitude.ToString) Or
                FP.Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPWidth.ToString) Or
                FP.Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPPosNegRatio.ToString) Or
                FP.Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPXCenter.ToString) Then
                FitParameterIdentifiersToRemove.Add(FP.Identifier)
            End If

        Next

        ' Remove all Fit-Parameters
        For i As Integer = 0 To FitParameterIdentifiersToRemove.Count - 1 Step 1
            Me.FitParameters.RemoveFitParameter(FitParameterIdentifiersToRemove(i))
        Next

        ' Modify the Fit-Parameter-Array and add all subgap resonances channels
        For i As Integer = 0 To Me.SubGapPeaks.Count - 1 Step 1
            ' add Fit-Parameters
            With Me.FitParameters
                .Add(Me.SubGapPeaks(i).XCenter)
                .Add(Me.SubGapPeaks(i).Amplitude)
                .Add(Me.SubGapPeaks(i).Width)
                .Add(Me.SubGapPeaks(i).PosNegRatio)
            End With
        Next

        ' Call the reinitialization for the IEC in the base-class
        MyBase.ReInitializeFitParameters()
    End Sub

#End Region

#Region "Import / Export"

    ''' <summary>
    ''' Check after a successfull import of a fit-parameter, if it belongs to a sub-gap-peak.
    ''' If so, add the peak, if not yet done, and adjust its settings.
    ''' </summary>
    Protected Overrides Sub Import_ParameterIdentified(ByVal Identifier As String, ByRef Parameter As cFitParameter)

        ' Do nothing, if we treat regular parameters
        If Not (Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPAmplitude.ToString) Or
            Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPWidth.ToString) Or
            Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPPosNegRatio.ToString) Or
            Identifier.Contains(iFitFunction_SubGapPeaks.SubGapPeak.ParameterIdentifier.SGPXCenter.ToString)) Then
            Return
        End If

        ' Get the index form the identifier
        Dim SubGapPeakIndex As Integer = iFitFunction_SubGapPeaks.SubGapPeak.SGPIndexFromIdentifier(Identifier)

        ' Check, if the necessary count of peaks has been added:
        Dim SubGapPeakToModify As iFitFunction_SubGapPeaks.SubGapPeak = Nothing
        For Each SGP As iFitFunction_SubGapPeaks.SubGapPeak In Me.SubGapPeaks
            If SGP.SGPIndex = SubGapPeakIndex Then
                SubGapPeakToModify = SGP
                Exit For
            End If
        Next

        If SubGapPeakToModify Is Nothing Then
            ' Create SGP
            Me.AddSubGapPeak(SubGapPeakIndex)
            Me.ReInitializeFitParameters()
            SubGapPeakToModify = Me.SubGapPeaks(Me.SubGapPeaks.Count - 1)
        End If

    End Sub

    ''' <summary>
    ''' Write additionally the parameter-settings about enabled or disabled SGPs.
    ''' </summary>
    Protected Overrides Sub Export_WriteAdditionalXMLElements(ByRef XMLWriter As Xml.XmlTextWriter)

        ' Give a warning, if disabled SGPs are present.
        Dim DisableSGPPresent As Boolean = False

        With XMLWriter
            ' Begin the element of the parameter description
            .WriteStartElement("SubGapPeakEnabledDisabledSettings") ' <SubGapPeakEnabledDisabledSettings>

            ' Write an element for each Fit-Parameter
            For Each SGP As iFitFunction_SubGapPeaks.SubGapPeak In Me.SubGapPeaks
                .WriteStartElement("SGPEnabled") ' <SGPEnabled
                .WriteAttributeString("Identifier", SGP.SGPIndex.ToString)
                .WriteAttributeString("Enabled", SGP.SubGapPeakEnabled.ToString())
                .WriteEndElement() ' SGPEnabled /> 

                If Not SGP.SubGapPeakEnabled Then DisableSGPPresent = True
            Next

            ' Close the Section
            .WriteEndElement() ' <\SubGapPeakEnabledDisabledSettings>
        End With

        ' warn the user, if SGPs were disabled!
        If DisableSGPPresent Then
            MessageBox.Show(My.Resources.rBCSFit_SubGapPeak.DisabledSGPsDiscoveredExport,
                            My.Resources.rBCSFit_SubGapPeak.DisabledSGPsDiscoveredExport_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        MyBase.Export_WriteAdditionalXMLElements(XMLWriter)
    End Sub

    ' ''' <summary>
    ' ''' Read unknown XML-Elements, if needed by the import.
    ' ''' </summary>
    'Protected Overrides Sub Import_UnknownXMLElementIdentified(ByVal XMLElementName As String, ByRef XMLReader As Xml.XmlReader)
    '    If XMLElementName = "SGPEnabled" Then
    '        ' Sub-Gap-Peak enabled/disabled data
    '        ' get and check the properties:
    '        '###############################
    '        ' go through all attributes
    '        Dim SubGapPeakIndex As Integer = -1
    '        With XMLReader
    '            If .AttributeCount > 0 Then
    '                While .MoveToNextAttribute
    '                    Select Case .Name
    '                        Case "Identifier"
    '                            SubGapPeakIndex = Convert.ToInt32(.Value)
    '                        Case "Enabled"
    '                            ' Only save, if the SGPIdentifier got read!
    '                            If SubGapPeakIndex >= 0 Then
    '                                ' Get the index of the SGP-Panel
    '                                Dim iSGPPanelIndex As Integer = Me.SubGapPeakIDsToPanelIndices.IndexOf(SubGapPeakIndex)
    '                                If iSGPPanelIndex = -1 Then Continue While

    '                                ' Go to the Settingspanel and set the parameters
    '                                Dim SGPPanel As cFitSettingSubParameter_SubGapPeakPanel = DirectCast(Me.flpSubGapPeaks.Controls(iSGPPanelIndex), cFitSettingSubParameter_SubGapPeakPanel)

    '                                ' Enable or disable the SGP.
    '                                Dim SGPEnabled As Boolean = Convert.ToBoolean(.Value)
    '                                If Not SGPEnabled Then SGPPanel.SetSGPEnabled(SGPEnabled)
    '                            End If
    '                    End Select
    '                End While
    '            End If
    '        End With
    '    Else
    '        ' else pass back to base-class
    '        MyBase.Import_UnknownXMLElementIdentified(XMLElementName, XMLReader)
    '    End If
    'End Sub

#End Region


End Class
