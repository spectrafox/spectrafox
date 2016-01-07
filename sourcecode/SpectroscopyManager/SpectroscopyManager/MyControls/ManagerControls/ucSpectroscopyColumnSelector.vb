Imports System.Collections.Specialized

Public Class ucSpectroscopyColumnSelector
    Inherits ucFilteredListComboBoxSelector

#Region "Overrides of Settings-Accessors in the base class"

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastFilterText() As String
        Return My.Settings.LastSpectroscopyPlot_FilterText
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastFilterText(ByVal Value As String)
        My.Settings.LastSpectroscopyPlot_FilterText = Value
        'My.Settings.Save() ##### NEEDS TO MUCH TIME!!
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastUsedFilters() As StringCollection
        Return My.Settings.DataColumnFilter_LastUsedFilters
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastUsedFilters(ByVal Value As StringCollection)
        My.Settings.DataColumnFilter_LastUsedFilters = Value
        'My.Settings.Save() ##### NEEDS TO MUCH TIME!!
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overrides Function GetLastSelection() As StringCollection
        Return My.Settings.LastSpectroscopyPlot_SelectedColumnNames
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overrides Sub SetLastSelection(ByVal Value As StringCollection)
        For Each V As String In Value
            If V = String.Empty Then
                Debug.WriteLine("ucColumnSelector: SetLastSelection->EmptyString")
            End If
        Next
        My.Settings.LastSpectroscopyPlot_SelectedColumnNames = Value
        'My.Settings.Save() '##### NEEDS To MUCH TIME!!
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
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfColumns As Dictionary(Of String, cSpectroscopyTable.DataColumn),
                                           Optional ByVal SelectedColumnName As String = "",
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Me.InitializeColumns(ListOfColumns.Keys.ToList, SelectedColumnName, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfColumns As List(Of cSpectroscopyTable.DataColumn),
                                           Optional ByVal SelectedColumnName As String = "",
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        ' Extract ColumnNames
        Dim l As New List(Of String)
        For Each Col As cSpectroscopyTable.DataColumn In ListOfColumns
            l.Add(Col.Name)
        Next
        Me.InitializeColumns(l, SelectedColumnName, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded function takes several SpectroscopyTable-Objects,
    ''' and adds (and therefore displays) only Columns with common Names!!!
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef SpectroscopyTableList As List(Of cSpectroscopyTable),
                                           Optional ByVal SelectedColumnName As String = "",
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim l As List(Of String) = cSpectroscopyTable.GetCommonColumns(SpectroscopyTableList)

        Me.InitializeColumns(l, SelectedColumnName, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded function takes several SpectroscopyTable-Objects,
    ''' and adds (and therefore displays) only Columns with common Names!!!
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef SpectroscopyTableList As Dictionary(Of String, cSpectroscopyTable),
                                           Optional ByVal SelectedColumnName As String = "",
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim l As List(Of String) = cSpectroscopyTable.GetCommonColumns(SpectroscopyTableList.Values.ToList)
        Me.InitializeColumns(l, SelectedColumnName, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfColumns As List(Of String),
                                           ByVal SelectedColumnNames As System.Collections.Specialized.StringCollection,
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedColumnNames As New List(Of String)
        If Not SelectedColumnNames Is Nothing Then
            For Each S As String In SelectedColumnNames
                ListOfSelectedColumnNames.Add(S)
            Next
        End If
        Me.InitializeColumns(ListOfColumns, ListOfSelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfColumns As Dictionary(Of String, cSpectroscopyTable.DataColumn),
                                           ByVal SelectedColumnNames As List(Of String),
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Me.InitializeColumns(ListOfColumns.Keys.ToList, SelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfColumns As List(Of cSpectroscopyTable.DataColumn),
                                           ByVal SelectedColumnNames As List(Of String),
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        ' Extract ColumnNames
        Dim l As New List(Of String)
        For Each Col As cSpectroscopyTable.DataColumn In ListOfColumns
            l.Add(Col.Name)
        Next
        Me.InitializeColumns(l, SelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfColumns As List(Of cSpectroscopyTable.DataColumn),
                                           ByVal SelectedColumnNames As System.Collections.Specialized.StringCollection,
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedColumnNames As New List(Of String)
        If Not SelectedColumnNames Is Nothing Then
            For Each S As String In SelectedColumnNames
                ListOfSelectedColumnNames.Add(S)
            Next
        End If
        Me.InitializeColumns(ListOfColumns, ListOfSelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef ListOfColumns As Dictionary(Of String, cSpectroscopyTable.DataColumn),
                                           ByVal SelectedColumnNames As System.Collections.Specialized.StringCollection,
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedColumnNames As New List(Of String)
        If Not SelectedColumnNames Is Nothing Then
            For Each S As String In SelectedColumnNames
                ListOfSelectedColumnNames.Add(S)
            Next
        End If
        Me.InitializeColumns(ListOfColumns, ListOfSelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded function takes several SpectroscopyTable-Objects,
    ''' and adds (and therefore displays) only Columns with common Names!!!
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef SpectroscopyTableList As List(Of cSpectroscopyTable),
                                           ByVal SelectedColumnNames As List(Of String),
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim l As List(Of String) = cSpectroscopyTable.GetCommonColumns(SpectroscopyTableList)

        Me.InitializeColumns(l, SelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StrinCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef SpectroscopyTableList As List(Of cSpectroscopyTable),
                                           ByVal SelectedColumnNames As System.Collections.Specialized.StringCollection,
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedColumnNames As New List(Of String)
        If Not SelectedColumnNames Is Nothing Then
            For Each S As String In SelectedColumnNames
                ListOfSelectedColumnNames.Add(S)
            Next
        End If
        Me.InitializeColumns(SpectroscopyTableList, ListOfSelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded function takes several SpectroscopyTable-Objects,
    ''' and adds (and therefore displays) only Columns with common Names!!!
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef SpectroscopyTableList As Dictionary(Of String, cSpectroscopyTable),
                                           ByVal SelectedColumnNames As List(Of String),
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim l As List(Of String) = cSpectroscopyTable.GetCommonColumns(SpectroscopyTableList.Values.ToList)
        Me.InitializeColumns(l, SelectedColumnNames, UseLastUsedFilter)
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StrinCollections
    ''' </summary>
    Public Overloads Sub InitializeColumns(ByRef SpectroscopyTableList As Dictionary(Of String, cSpectroscopyTable),
                                           ByVal SelectedColumnNames As System.Collections.Specialized.StringCollection,
                                           Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedColumnNames As New List(Of String)
        If Not SelectedColumnNames Is Nothing Then
            For Each S As String In SelectedColumnNames
                ListOfSelectedColumnNames.Add(S)
            Next
        End If
        Me.InitializeColumns(SpectroscopyTableList, ListOfSelectedColumnNames, UseLastUsedFilter)
    End Sub
#End Region

#Region "Setter and Getters for Compatibility Reasons"

    ''' <summary>
    ''' Obsolete compatibility function.
    ''' Use Me.SelectedEntry!
    ''' </summary>
    Public Property SelectedColumnName As String
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
    Public Property SelectedColumnNames As List(Of String)
        Get
            Return Me.SelectedEntries
        End Get
        Set(value As List(Of String))
            Me.SelectedEntries = value
        End Set
    End Property

#End Region

End Class
