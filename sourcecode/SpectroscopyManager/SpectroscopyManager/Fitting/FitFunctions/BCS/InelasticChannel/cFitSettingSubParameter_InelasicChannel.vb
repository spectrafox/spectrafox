Public Class cFitSettingSubParameter_InelasticChannel
    Inherits cFitSettingSubParameterPanel_2

    ''' <summary>
    ''' Local reference to the Inelastic Channel.
    ''' </summary>
    Public IEC As cFitFunction_TipSampleConvolutionBase.InelasticChannel

    ''' <summary>
    ''' Ready
    ''' </summary>
    Public bReady As Boolean = False

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New(ByRef IEC As cFitFunction_TipSampleConvolutionBase.InelasticChannel,
                   ByRef AllFitParameters As cFitParameterGroupGroup)
        MyBase.New(IEC.Energy, IEC.Probability, AllFitParameters)

        ' Save reference to IEC.
        Me.IEC = IEC

        ' Komponenten initialisieren
        Me.InitializeComponent()

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Sub that resets the FitParameterGroup to lock parameters together.
    ''' </summary>
    Public Function SetFitParameterGroups(ByRef FitParameterGroups As cFitParameterGroupGroup) As Boolean
        For Each C As Control In Me.Controls
            If C.GetType Is GetType(mFitParameter) Then
                DirectCast(C, mFitParameter).SetFitParameterGroups(FitParameterGroups)
            End If
        Next
        Return True
    End Function

End Class
