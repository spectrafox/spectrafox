Imports System.Collections.Specialized

Public Class ucScanChannelSelector
    Inherits ucFilteredListComboBoxSelector

#Region "Overrides of Settings-Accessors in the base class"

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastFilterText() As String
        Return My.Settings.ScanChannelSelector_LastFilterText
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastFilterText(ByVal Value As String)
        My.Settings.ScanChannelSelector_LastFilterText = Value
        'cGlobal.SaveSettings() ##### NEEDS TO MUCH TIME!!
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastUsedFilters() As StringCollection
        Return My.Settings.ScanChannelFilter_LastUsedFilters
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastUsedFilters(ByVal Value As StringCollection)
        My.Settings.ScanChannelFilter_LastUsedFilters = Value
        'cGlobal.SaveSettings() ##### NEEDS TO MUCH TIME!!
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastSelection() As StringCollection
        Return My.Settings.ScanChannelSelector_LastSelectedChannelNames
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastSelection(ByVal Value As StringCollection)
        For Each V As String In Value
            If V = String.Empty Then
                Debug.WriteLine("ucChannelSelector: SetLastSelection->EmptyString")
            End If
            Debug.WriteLine("ucChannelSelector: storing last selection: " & V)
        Next
        My.Settings.ScanChannelSelector_LastSelectedChannelNames = Value
        'cGlobal.SaveSettings() '##### NEEDS To MUCH TIME!!
    End Sub

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Load constructor
    ''' </summary>
    Private Sub ucScanChannelSelector_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.LastFilterHistory_MaxCount = My.Settings.ScanChannelFilter_Max
    End Sub

#End Region

#Region "Initialization"

    ''' <summary>
    ''' Writes all given ScanChannels in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfChannels As List(Of cScanImage.ScanChannel),
                                           Optional ByVal PreSelectedChannelName As String = "",
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        ' Extract ColumnNames
        Dim l As New List(Of String)
        For Each Col As cScanImage.ScanChannel In ListOfChannels
            l.Add(Col.Name)
        Next

        Me.InitializeColumns(l, PreSelectedChannelName, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfChannels As List(Of cScanImage.ScanChannel),
                                           ByVal PreSelectedChannelNames As List(Of String),
                                           Optional ByVal UseLastUsedFilter As Boolean = False)

        ' Extract ColumnNames
        Dim l As New List(Of String)
        For Each Col As cScanImage.ScanChannel In ListOfChannels
            l.Add(Col.Name)
        Next

        Me.InitializeColumns(l, PreSelectedChannelNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfChannels As List(Of cScanImage.ScanChannel),
                                           ByVal PreSelectedChannelNames As System.Collections.Specialized.StringCollection,
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedColumnNames As New List(Of String)
        If Not PreSelectedChannelNames Is Nothing Then
            For Each S As String In PreSelectedChannelNames
                ListOfSelectedColumnNames.Add(S)
            Next
        End If
        Me.InitializeColumns(ListOfChannels, ListOfSelectedColumnNames, UseLastUsedFilter)
    End Sub

#End Region

End Class
