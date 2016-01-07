Public Class cFitSettingSubParameterPanel_4

    Private _Identifier As Integer
    Private _FitParameter1 As cFitParameter
    Private _FitParameter2 As cFitParameter
    Private _FitParameter3 As cFitParameter
    Private _FitParameter4 As cFitParameter
    Private _AllFitParameters As cFitParameterGroupGroup

#Region "Accessors to the currently saved fit-parameters"

    ''' <summary>
    ''' Identifier
    ''' </summary>
    Public Property Identifier As Integer
        Get
            Return Me._Identifier
        End Get
        Set(value As Integer)
            Me._Identifier = value
        End Set
    End Property

    ''' <summary>
    ''' First fit-parameter shown.
    ''' </summary>
    Public Property FitParameter1 As cFitParameter
        Get
            Return Me._FitParameter1
        End Get
        Set(value As cFitParameter)
            Me._FitParameter1 = value
            Me.fpFitParameter1.BindToFitParameter(Me._FitParameter1, Me._AllFitParameters)
        End Set
    End Property

    ''' <summary>
    ''' First fit-parameter shown.
    ''' </summary>
    Public Property FitParameter2 As cFitParameter
        Get
            Return Me._FitParameter2
        End Get
        Set(value As cFitParameter)
            Me._FitParameter2 = value
            Me.fpFitParameter2.BindToFitParameter(Me._FitParameter2, Me._AllFitParameters)
        End Set
    End Property

    ''' <summary>
    ''' First fit-parameter shown.
    ''' </summary>
    Public Property FitParameter3 As cFitParameter
        Get
            Return Me._FitParameter3
        End Get
        Set(value As cFitParameter)
            Me._FitParameter3 = value
            Me.fpFitParameter3.BindToFitParameter(Me._FitParameter3, Me._AllFitParameters)
        End Set
    End Property

    ''' <summary>
    ''' First fit-parameter shown.
    ''' </summary>
    Public Property FitParameter4 As cFitParameter
        Get
            Return Me._FitParameter4
        End Get
        Set(value As cFitParameter)
            Me._FitParameter4 = value
            Me.fpFitParameter4.BindToFitParameter(Me._FitParameter4, Me._AllFitParameters)
        End Set
    End Property

    ''' <summary>
    ''' First fit-parameter shown.
    ''' </summary>
    Public Property AllFitParameters As cFitParameterGroupGroup
        Get
            Return Me._AllFitParameters
        End Get
        Set(value As cFitParameterGroupGroup)
            Me._AllFitParameters = value

            ' Set all fit-parameter boxes new.
            Me.fpFitParameter1.BindToFitParameter(Me._FitParameter1, Me._AllFitParameters)
            Me.fpFitParameter2.BindToFitParameter(Me._FitParameter2, Me._AllFitParameters)
            Me.fpFitParameter3.BindToFitParameter(Me._FitParameter3, Me._AllFitParameters)
            Me.fpFitParameter4.BindToFitParameter(Me._FitParameter4, Me._AllFitParameters)
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    End Sub

    ''' <summary>
    ''' Constructor expecting the shown fit-parameters,
    ''' that have to be created before.
    ''' </summary>
    Public Sub New(ByRef FitParameter1 As cFitParameter,
                   ByRef FitParameter2 As cFitParameter,
                   ByRef FitParameter3 As cFitParameter,
                   ByRef FitParameter4 As cFitParameter,
                   ByRef AllFitParameters As cFitParameterGroupGroup)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me._AllFitParameters = AllFitParameters
        Me.FitParameter1 = FitParameter1
        Me.FitParameter2 = FitParameter2
        Me.FitParameter3 = FitParameter3
        Me.FitParameter4 = FitParameter4
    End Sub
#End Region

#Region "Remove the sub-parameter-panel"
    ''' <summary>
    ''' Event to remove the sub-gap-peak out of the fit-functions-DOS.
    ''' </summary>
    Public Event RequestRemovalOfSubGapPeak(ByRef PanelToRemove As cFitSettingSubParameterPanel_4)

    ''' <summary>
    ''' Raise the event to remove the sub-parameter-panel
    ''' </summary>
    Private Sub btnRemoveSupParameterPanel_Click(sender As Object, e As EventArgs) Handles btnRemoveSubParameterPanel.Click
        RaiseEvent RequestRemovalOfSubGapPeak(Me)
    End Sub
#End Region

End Class
