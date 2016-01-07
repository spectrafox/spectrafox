Imports System.Collections.Specialized

Public Class ucSpectroscopyTableSelector
    Inherits ucFilteredListComboBoxSelector

#Region "Overrides of Settings-Accessors in the base class"

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastFilterText() As String
        Return My.Settings.LastSpectroscopyTableSelector_FilterText
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastFilterText(ByVal Value As String)
        My.Settings.LastSpectroscopyTableSelector_FilterText = Value
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastUsedFilters() As StringCollection
        Return My.Settings.LastSpectroscopyTableSelector_LastUsedFilters
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastUsedFilters(ByVal Value As StringCollection)
        My.Settings.LastSpectroscopyTableSelector_LastUsedFilters = Value
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastSelection() As StringCollection
        Return My.Settings.LastSpectroscopyTableSelector_SelectedTables
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastSelection(ByVal Value As StringCollection)
        My.Settings.LastSpectroscopyTableSelector_SelectedTables = Value
    End Sub

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Load constructor
    ''' </summary>
    Private Sub ucScanChannelSelector_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.LastFilterHistory_MaxCount = My.Settings.DataColumnFilter_Max
    End Sub

#End Region

#Region "Initialization"

    ''' <summary>
    ''' Writes all SpectroscopyTables in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfSpectroscopyTables As Dictionary(Of String, cSpectroscopyTable),
                                          Optional ByVal SelectedTable As String = "",
                                          Optional ByVal UseLastUsedFilter As Boolean = False)
        Me.InitializeColumns(ListOfSpectroscopyTables.Keys.ToList, SelectedTable, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfSpectroscopyTables As List(Of cSpectroscopyTable),
                                          Optional ByVal SelectedTable As String = "",
                                          Optional ByVal UseLastUsedFilter As Boolean = False)
        ' Extract ColumnNames
        Dim l As New List(Of String)(ListOfSpectroscopyTables.Count)
        For Each Col As cSpectroscopyTable In ListOfSpectroscopyTables
            l.Add(Col.FileNameWithoutPath)
        Next
        Me.InitializeColumns(l, SelectedTable, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfSpectroscopyTables As List(Of String),
                                          ByVal SelectedTables As System.Collections.Specialized.StringCollection,
                                          Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedTables As New List(Of String)
        If Not SelectedTables Is Nothing Then
            For Each S As String In SelectedTables
                ListOfSelectedTables.Add(S)
            Next
        End If
        Me.InitializeColumns(ListOfSpectroscopyTables, ListOfSelectedTables, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfSpectroscopyTables As Dictionary(Of String, cSpectroscopyTable),
                                           ByVal SelectedTables As List(Of String),
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Me.InitializeColumns(ListOfSpectroscopyTables.Keys.ToList, SelectedTables, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfSpectroscopyTables As List(Of cSpectroscopyTable),
                                          ByVal SelectedTables As List(Of String),
                                          Optional ByVal UseLastUsedFilter As Boolean = False)
        ' Extract ColumnNames
        Dim l As New List(Of String)(ListOfSpectroscopyTables.Count)
        For Each Col As cSpectroscopyTable In ListOfSpectroscopyTables
            l.Add(Col.FileNameWithoutPath)
        Next
        Me.InitializeColumns(l, SelectedTables, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfSpectroscopyTables As List(Of cSpectroscopyTable),
                                          ByVal SelectedTables As System.Collections.Specialized.StringCollection,
                                          Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedTables As New List(Of String)
        If Not SelectedTables Is Nothing Then
            For Each S As String In SelectedTables
                ListOfSelectedTables.Add(S)
            Next
        End If
        Me.InitializeColumns(ListOfSpectroscopyTables, ListOfSelectedTables, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfSpectroscopyTables As Dictionary(Of String, cSpectroscopyTable),
                                          ByVal SelectedTables As System.Collections.Specialized.StringCollection,
                                          Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedTables As New List(Of String)
        If Not SelectedTables Is Nothing Then
            For Each S As String In SelectedTables
                ListOfSelectedTables.Add(S)
            Next
        End If
        Me.InitializeColumns(ListOfSpectroscopyTables, ListOfSelectedTables, UseLastUsedFilter)
    End Sub

#End Region

#Region "Setter and Getters for Compatibility Reasons"

    ''' <summary>
    ''' Obsolete compatibility function.
    ''' Use Me.SelectedEntry!
    ''' </summary>
    Public Property SelectedTable As String
        Get
            Return Me.SelectedEntry
        End Get
        Set(value As String)
            Me.SelectedEntry = value
        End Set
    End Property

    ''' <summary>
    ''' Obsolete compatibility function.
    ''' Use Me.SelectedEntries!
    ''' </summary>
    Public Property SelectedTables As List(Of String)
        Get
            Return Me.SelectedEntries
        End Get
        Set(value As List(Of String))
            Me.SelectedEntries = value
        End Set
    End Property

#End Region

End Class
