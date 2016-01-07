Public Interface iFitFunction_SubGapPeaks
    Inherits iFitFunction

    ''' <summary>
    ''' Type of the Sub-Gap-Peaks.
    ''' </summary>
    Enum SubGapPeakTypes
        Lorentz
        Gauss
    End Enum

    ''' <summary>
    ''' Set this variable to determine the type of sub-gap-peaks used in the fit.
    ''' </summary>
    Property SubGapPeakType As SubGapPeakTypes

    ''' <summary>
    ''' Sub-Class to define a sub-gap peak.
    ''' </summary>
    Class SubGapPeak

        ''' <summary>
        ''' Count of the parameters each SGP is depending on.
        ''' </summary>
        Public Const SGPParameterCount As Integer = 4

        ''' <summary>
        ''' Parameters of the sub-gap-peak
        ''' </summary>
        Public Enum ParameterIdentifier
            SGPAmplitude = 0
            SGPWidth = 1
            SGPPosNegRatio = 2
            SGPXCenter = 3
        End Enum

        Protected _SGPIndex As Integer
        ''' <summary>
        ''' Index 
        ''' </summary>
        Public Property SGPIndex As Integer
            Get
                Return Me._SGPIndex
            End Get
            Set(value As Integer)
                Me.XCenter.ChangeIdentifier(SGPIdentifier_XCenter(value))
                Me.Amplitude.ChangeIdentifier(SGPIdentifier_Amplitude(value))
                Me.Width.ChangeIdentifier(SGPIdentifier_Width(value))
                Me.PosNegRatio.ChangeIdentifier(SGPIdentifier_PosNegRatio(value))

                Me.XCenter.Description = SGPDescription_XCenter(value)
                Me.Amplitude.Description = SGPDescription_Amplitude(value)
                Me.Width.Description = SGPDescription_Width(value)
                Me.PosNegRatio.Description = SGPDescription_PosNegRatio(value)

                Me._SGPIndex = value
            End Set
        End Property

        ''' <summary>
        ''' Can be used to activate or deactivate the SubGapPeak.
        ''' </summary>
        Public SubGapPeakEnabled As Boolean = True

        ''' <summary>
        ''' Can be used to mark the DOS the SubGapPeak is belonging to!
        ''' </summary>
        Public SubGapPeakParentDOS As cFitFunction_BCSBase.DOSTypes = cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample

        ''' <summary>
        ''' Center of the SGP
        ''' </summary>
        Public XCenter As New cFitParameter(ParameterIdentifier.SGPXCenter.ToString, 0.0005, False, My.Resources.rBCSFit_SubGapPeak.XCenter)

        ''' <summary>
        ''' Amplitude of the SGP
        ''' </summary>
        Public Amplitude As New cFitParameter(ParameterIdentifier.SGPAmplitude.ToString, 1, False, My.Resources.rBCSFit_SubGapPeak.Amplitude)

        ''' <summary>
        ''' Width of the SGP
        ''' </summary>
        Public Width As New cFitParameter(ParameterIdentifier.SGPWidth.ToString, 0.00002, False, My.Resources.rBCSFit_SubGapPeak.Width)

        ''' <summary>
        ''' Positive to negative ratio of the SGP
        ''' </summary>
        Public PosNegRatio As New cFitParameter(ParameterIdentifier.SGPPosNegRatio.ToString, 1, False, My.Resources.rBCSFit_SubGapPeak.PosNegRatio)

        ''' <summary>
        ''' Returns a panel with all parameters locked to the parameter-selection boxes
        ''' </summary>
        Public Function GetSubGapPeakPanel(ByRef AllFitParameters As cFitParameterGroupGroup) As cFitSettingSubParameter_SubGapPeakPanel
            Dim P As New cFitSettingSubParameter_SubGapPeakPanel(Me,
                                                                 AllFitParameters)
            P.Identifier = Me.SGPIndex
            Return P
        End Function

        ''' <summary>
        ''' Constructor
        ''' </summary>
        Public Sub New()
            AddHandler XCenter.ValueChanged, AddressOf RaiseValueChangedEvent
            AddHandler Amplitude.ValueChanged, AddressOf RaiseValueChangedEvent
            AddHandler Width.ValueChanged, AddressOf RaiseValueChangedEvent
            AddHandler PosNegRatio.ValueChanged, AddressOf RaiseValueChangedEvent
        End Sub

        ''' <summary>
        ''' Value changed of SGP
        ''' </summary>
        Public Event SubGapPeakValueChanged()

        ''' <summary>
        ''' Raise the Event
        ''' </summary>
        Private Sub RaiseValueChangedEvent(NewValue As Double)
            RaiseEvent SubGapPeakValueChanged()
        End Sub

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPDescription_XCenter(ByVal i As Integer) As String
            Return "#" & i.ToString & " " & My.Resources.rBCSFit_SubGapPeak.XCenter
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPDescription_Amplitude(ByVal i As Integer) As String
            Return "#" & i.ToString & " " & My.Resources.rBCSFit_SubGapPeak.Amplitude
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPDescription_Width(ByVal i As Integer) As String
            Return "#" & i.ToString & " " & My.Resources.rBCSFit_SubGapPeak.Width
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPDescription_PosNegRatio(ByVal i As Integer) As String
            Return "#" & i.ToString & " " & My.Resources.rBCSFit_SubGapPeak.PosNegRatio
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPIdentifier_XCenter(ByVal i As Integer) As String
            Return ParameterIdentifier.SGPXCenter.ToString & i.ToString
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPIdentifier_Amplitude(ByVal i As Integer) As String
            Return ParameterIdentifier.SGPAmplitude.ToString & i.ToString
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPIdentifier_Width(ByVal i As Integer) As String
            Return ParameterIdentifier.SGPWidth.ToString & i.ToString
        End Function

        ''' <summary>
        ''' Returns the Nth identifier of an SGP
        ''' </summary>
        Public Shared Function SGPIdentifier_PosNegRatio(ByVal i As Integer) As String
            Return ParameterIdentifier.SGPPosNegRatio.ToString & i.ToString
        End Function

        ''' <summary>
        ''' The the Peak-Identifier from a String (last digit is number)
        ''' </summary>
        Public Shared Function SGPIndexFromIdentifier(ByVal Identifier As String) As Integer
            With ParameterIdentifier.SGPXCenter
                If Identifier.StartsWith(.ToString) Then Return Convert.ToInt32(Identifier.Remove(0, .ToString.Length))
            End With
            With ParameterIdentifier.SGPAmplitude
                If Identifier.StartsWith(.ToString) Then Return Convert.ToInt32(Identifier.Remove(0, .ToString.Length))
            End With
            With ParameterIdentifier.SGPPosNegRatio
                If Identifier.StartsWith(.ToString) Then Return Convert.ToInt32(Identifier.Remove(0, .ToString.Length))
            End With
            With ParameterIdentifier.SGPWidth
                If Identifier.StartsWith(.ToString) Then Return Convert.ToInt32(Identifier.Remove(0, .ToString.Length))
            End With
            Return -1
        End Function

    End Class

    ''' <summary>
    ''' List to save all sub-gap-peaks.
    ''' </summary>
    Property SubGapPeaks As List(Of SubGapPeak)

    ''' <summary>
    ''' Adds a new sub-gap peak to the fit-function.
    ''' Returns the index of the internal list, at which the peak got added.
    ''' </summary>
    Function AddSubGapPeak(Optional ByVal Index As Integer = -1) As Integer

    ''' <summary>
    ''' Removes the SGP from the list.
    ''' False, if the index was not found in the dictionary.
    ''' </summary>
    Function RemoveSubGapPeak(ByVal SubGapPeakIndex As Integer) As Boolean

    ''' <summary>
    ''' Clears the list of all sub-gap-peaks.
    ''' </summary>
    Sub ClearSubGapPeaks()

    ''' <summary>
    ''' Currently registered SubGapPeaks.
    ''' </summary>
    ReadOnly Property RegisteredSubGapPeaks As List(Of SubGapPeak)

    ''' <summary>
    ''' Method that sets all Initial FitParameters from the individual FitFunctions
    ''' in this multiple-function-container
    ''' </summary>
    Sub ReInitializeFitParameters()

End Interface
