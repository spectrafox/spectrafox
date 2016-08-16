Public Class cFitSettingSubParameterPanel_2

    Private _Identifier As Integer
    Private _FitParameter1 As cFitParameter
    Private _FitParameter2 As cFitParameter
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
    ''' Second fit-parameter shown.
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
    ''' All fit-parameter shown.
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
                   ByRef AllFitParameters As cFitParameterGroupGroup)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me._AllFitParameters = AllFitParameters
        Me.FitParameter1 = FitParameter1
        Me.FitParameter2 = FitParameter2
    End Sub
#End Region

#Region "Remove the sub-parameter-panel"
    ''' <summary>
    ''' Event to request a removal of the sub-panel.
    ''' </summary>
    Public Event RequestRemovalOfSubPanel(ByRef PanelToRemove As cFitSettingSubParameterPanel_2)

    ''' <summary>
    ''' Raise the event to request a remove the inelastic-channel-panel
    ''' </summary>
    Private Sub btnRemoveSubPanel_Click(sender As Object, e As EventArgs) Handles btnRemoveSubParameterPanel.Click
        RaiseEvent RequestRemovalOfSubPanel(Me)
    End Sub
#End Region

End Class
