Imports System.Text.RegularExpressions
Imports System.Collections.Specialized

Public Class ucFilteredListComboBoxSelector

#Region "Access to Settings"

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overridable Function GetLastFilterText() As String
        Return String.Empty
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overridable Sub SetLastFilterText(ByVal Value As String)
        ' Nothing
        Return
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overridable Function GetLastUsedFilters() As StringCollection
        Return New StringCollection()
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overridable Sub SetLastUsedFilters(ByVal Value As StringCollection)
        ' Nothing
        Return
    End Sub

    ''' <summary>
    ''' Returns a certain settings-variable.
    ''' </summary>
    Public Overridable Function GetLastSelection() As StringCollection
        Return New StringCollection()
    End Function

    ''' <summary>
    ''' Override to save value to a certain settings variable.
    ''' </summary>
    Public Overridable Sub SetLastSelection(ByVal Value As StringCollection)
        ' Nothing
        Return
    End Sub

    ''' <summary>
    ''' Gives the number of history entries.
    ''' </summary>
    Public LastFilterHistory_MaxCount As Integer = 10

    ''' <summary>
    ''' If false, it does not react on events!
    ''' </summary>
    Private bReady As Boolean = True

#End Region

#Region "Events"
    ''' <summary>
    ''' Fired, if the Selection Changed
    ''' </summary>
    Public Event SelectedIndexChanged()
#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub ucFilteredListComboBoxSelector_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.cbEntry.Width = Me.Width - Me.pbContextMenu.Width - 3
        Me.lbEntry.Width = Me.Width - Me.pbContextMenu.Width - 3
        Me.lbEntry.Height = Me.Height
    End Sub

#End Region

#Region "Properties"
    Private ListOfEntries As New List(Of String)
    Private ListOfEntriesFiltered As New List(Of String)

    Private _SelectedEntry As String = String.Empty
    Private _SelectedEntries As New List(Of String)

    ''' <summary>
    ''' AppereanceTypes of the Selector
    ''' </summary>
    Public Enum SelectorType
        Combobox
        Listbox
    End Enum

    Private _AppereanceType As SelectorType = SelectorType.Combobox
    ''' <summary>
    ''' AppereanceType of the Selector
    ''' </summary>
    Public Property AppereanceType As SelectorType
        Get
            Return Me._AppereanceType
        End Get
        Set(value As SelectorType)
            Me._AppereanceType = value

            ' Set the visibility of the FrontEnd
            Select Case Me._AppereanceType
                Case SelectorType.Combobox
                    Me.cbEntry.Visible = True
                    Me.lbEntry.Visible = False
                Case SelectorType.Listbox
                    Me.cbEntry.Visible = False
                    Me.lbEntry.Visible = True
            End Select
        End Set
    End Property

    ''' <summary>
    ''' Set the selection mode of the listbox.
    ''' </summary>
    Public Property IfAppearanceListBox_MultiSelectionMode As SelectionMode
        Get
            Return Me.lbEntry.SelectionMode
        End Get
        Set(value As SelectionMode)
            Me.lbEntry.SelectionMode = value
        End Set
    End Property

    ''' <summary>
    ''' Determines, if this module tracks the last selected values.
    ''' </summary>
    Public Property TurnOnLastSelectionSaving As Boolean = False

    ''' <summary>
    ''' Determines, if this module tracks the last filtered values.
    ''' </summary>
    Public Property TurnOnLastFilterSaving As Boolean = False

#End Region

#Region "Initialization"

    ''' <summary>
    ''' Writes all given Entries in the Combobox.
    ''' </summary>
    Public Sub InitializeColumns(ByRef Entries As List(Of String),
                                 Optional ByVal PreSelectEntry As String = "",
                                 Optional ByVal UseLastUsedFilter As Boolean = False)
        ' Extract ColumnNames
        Me.ListOfEntries = Entries

        ' apply filter, and set the list of displayed entries
        If UseLastUsedFilter Then Me.FilterColumnNames(Me.GetLastFilterText) Else Me.FilterColumnNames(String.Empty)

        ' Try to set the selected ColumnName
        Me.SelectedEntry = PreSelectEntry
    End Sub

    ''' <summary>
    ''' Writes all given Columns in the Combobox.
    ''' </summary>
    Public Sub InitializeColumns(ByRef Entries As List(Of String),
                                 ByVal PreSelectedEntries As List(Of String),
                                 Optional ByVal UseLastUsedFilter As Boolean = False)
        ' Extract ColumnNames
        Me.ListOfEntries = Entries

        ' apply filter, and set the list of displayed entries
        If UseLastUsedFilter Then Me.FilterColumnNames(Me.GetLastFilterText) Else Me.FilterColumnNames(String.Empty)

        ' Try to set the selected ColumnName
        If Not PreSelectedEntries Is Nothing Then Me.SelectedEntries = PreSelectedEntries
    End Sub

    ''' <summary>
    ''' This overloaded to take care of StringCollections
    ''' </summary>
    Public Sub InitializeColumns(ByRef Entries As List(Of String),
                                 ByVal PreSelectedEntries As System.Collections.Specialized.StringCollection,
                                 Optional ByVal UseLastUsedFilter As Boolean = False)
        Dim ListOfSelectedColumnNames As New List(Of String)
        If Not PreSelectedEntries Is Nothing Then
            For Each S As String In PreSelectedEntries
                ListOfSelectedColumnNames.Add(S)
            Next
        End If
        Me.InitializeColumns(Entries, ListOfSelectedColumnNames, UseLastUsedFilter)
    End Sub

#End Region

#Region "Filling of the Column-Selection-Boxes"
    ' This will be called from the worker thread to set
    ' the work's progress
    Delegate Sub _SetListValue(ByVal ListOfEntires As List(Of String))

    ''' <summary>
    ''' Sets the values to the boxes.
    ''' </summary>
    Friend Sub SetListValue(ByVal ListOfEntires As List(Of String))
        Dim _delegate As New _SetListValue(AddressOf Me.SetListValue)
        If Me.AppereanceType = SelectorType.Combobox Then
            If Me.cbEntry.InvokeRequired Then
                Me.cbEntry.Invoke(_delegate, ListOfEntires)
            Else

                Me.bReady = False
                '##########
                ' Combobox
                With Me.cbEntry
                    Me.ComboBoxClearThreadSafe()
                    .DisplayMember = "Value"
                    .ValueMember = "Key"

                    For Each EntryName As String In ListOfEntires
                        Me.ComboBoxAddItemThreadSafe(EntryName)
                        If Me.SelectedEntry = EntryName Then
                            .SelectedIndex = (.Items.Count - 1)
                        End If
                    Next

                    If Me.SelectedEntry = String.Empty And ListOfEntires.Count > 0 Then
                        .SelectedIndex = 0
                    End If
                End With
                Me.bReady = True
            End If
        Else
            If Me.lbEntry.InvokeRequired Then
                Me.lbEntry.Invoke(_delegate, ListOfEntires)
            Else
                Me.bReady = False
                '#########
                ' Listbox
                With Me.lbEntry
                    Me.ListBoxClearThreadSafe()
                    .DisplayMember = "Value"
                    .ValueMember = "Key"

                    For Each EntryName As String In ListOfEntires
                        Me.ListBoxAddItemThreadSafe(EntryName)
                        If Me.SelectedEntry = EntryName Then
                            .SelectedIndex = (.Items.Count - 1)
                        End If
                    Next

                    If Me.SelectedEntry = String.Empty And ListOfEntires.Count > 0 Then
                        .SelectedIndex = 0
                    End If
                End With
                Me.bReady = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Refreshes the displayed Columns.
    ''' </summary>
    Public Sub ReInitializeColumns()
        If Me.ListOfEntries Is Nothing Then Return
        Me.SetListValue(Me.ListOfEntries)
        Me.SelectedEntry = Me.SelectedEntry
    End Sub
#End Region

#Region "Thread-Safe add and clear functions for the Listbox and Combobox"
    Private Delegate Sub VoidFunction()
    Private Delegate Sub StringFunction(Text As String)

    Public Sub ListBoxAddItemThreadSafe(Text As String)
        With Me.lbEntry
            If .InvokeRequired Then
                .Invoke(New StringFunction(AddressOf ListBoxAddItemThreadSafe), Text)
                Return
            End If

            .Items.Add(Text)
        End With
    End Sub

    Public Sub ListBoxClearThreadSafe()
        With Me.lbEntry
            If .InvokeRequired Then
                .Invoke(New VoidFunction(AddressOf ListBoxClearThreadSafe))
                Return
            End If

            .Items.Clear()
        End With
    End Sub

    Public Sub ComboBoxAddItemThreadSafe(Text As String)
        With Me.cbEntry
            If .InvokeRequired Then
                .Invoke(New StringFunction(AddressOf ListBoxAddItemThreadSafe), Text)
                Return
            End If

            .Items.Add(Text)
        End With
    End Sub

    Public Sub ComboBoxClearThreadSafe()
        With Me.cbEntry
            If .InvokeRequired Then
                .Invoke(New VoidFunction(AddressOf ListBoxClearThreadSafe))
                Return
            End If

            .Items.Clear()
        End With
    End Sub

#End Region

#Region "Get/Set Data Functions"
    ''' <summary>
    ''' Set/Get selected Entry
    ''' </summary>
    Public Property SelectedEntry() As String
        Get
            Return Me._SelectedEntry
        End Get
        Set(value As String)
            If value = "" Then
                ' If we are a combobox, never allow an empty selection.
                If Me._AppereanceType = SelectorType.Combobox AndAlso
                   Me.ListOfEntriesFiltered.Count > 0 Then
                    value = Me.ListOfEntriesFiltered(0)
                Else
                    Return
                End If
            End If

            Me._SelectedEntries = {value}.ToList
            Me._SelectedEntry = value

            ' Set them in the list
            Me.SetSelectedEntry(value)

        End Set
    End Property

    ''' <summary>
    ''' Set/Get selected Entries
    ''' List for multiple selection.
    ''' </summary>
    Public Property SelectedEntries() As List(Of String)
        Get
            Return Me._SelectedEntries
        End Get
        Set(value As List(Of String))
            If Not value Is Nothing Then
                Me._SelectedEntries = value
                If Me._SelectedEntries.Count > 0 Then Me._SelectedEntry = Me._SelectedEntries(0)

                ' Set them in the list
                Me.SetSelectedColumnNames(value)
            End If
        End Set
    End Property

    ' This will be called from the worker thread to set
    ' the work's progress
    Delegate Sub _SetSelectedEntry(ByVal Entry As String)
    Friend Sub SetSelectedEntry(ByVal Entry As String)
        If Me.cbEntry.InvokeRequired Then
            Dim _delegate As New _SetSelectedEntry(AddressOf SetSelectedEntry)
            Me.Invoke(_delegate, Entry)
        Else

            Me.bReady = False
            '##########
            ' Combobox
            If Me._AppereanceType = SelectorType.Combobox Then

                With Me.cbEntry
                    For i As Integer = 0 To .Items.Count - 1 Step 1
                        Dim SelectedColumnName As String = DirectCast(.Items(i), String)
                        If SelectedColumnName = Entry Then
                            .SelectedIndex = i
                        End If
                    Next
                End With

                ' Just call the selection changed event once!
                Me.bReady = True
                Me.CB_SelectedColumnIndexChanged()

            End If

            '##########
            ' ListBox
            If Me._AppereanceType = SelectorType.Listbox Then

                With Me.lbEntry
                    .ClearSelected()
                    For i As Integer = 0 To .Items.Count - 1 Step 1
                        Dim SelectedColumnName As String = DirectCast(.Items(i), String)
                        If SelectedColumnName = Entry Then
                            .SelectedIndex = i
                        End If
                    Next

                End With

                ' Just call the selection changed event once!
                Me.bReady = True
                Me.LB_SelectedColumnIndexChanged()

            End If
        End If

        ' Save the last selected column names in the settings
        If TurnOnLastSelectionSaving Then
            Me.SetLastSelection(New StringCollection() From {Entry})
        End If
    End Sub

    Delegate Sub _SetSelectedColumnNames(ByVal Entries As List(Of String))
    Friend Sub SetSelectedColumnNames(ByVal Entries As List(Of String))
        If Me.lbEntry.InvokeRequired Then
            Dim _delegate As New _SetSelectedColumnNames(AddressOf SetSelectedColumnNames)
            Me.Invoke(_delegate, Entries)
        Else
            If Entries Is Nothing Then Return

            Me.bReady = False
            '##########
            ' ListBox
            Dim SelectedColumnName As String
            For i As Integer = 0 To Me.lbEntry.Items.Count - 1 Step 1
                SelectedColumnName = DirectCast(Me.lbEntry.Items(i), String)
                Me.lbEntry.SetSelected(i, Entries.Contains(SelectedColumnName))
            Next
            Me.bReady = True
            If Me._AppereanceType = SelectorType.Listbox Then Me.LB_SelectedColumnIndexChanged()

            ' Save the last selected column names in the settings
            If TurnOnLastSelectionSaving Then
                Dim l As New StringCollection()
                l.AddRange(Entries.ToArray)
                Me.SetLastSelection(l)
            End If

        End If
    End Sub

    ''' <summary>
    ''' Returns the Number of Entries in this Selector
    ''' </summary>
    Public Function CountEntries() As Integer
        Return Me.cbEntry.Items.Count
    End Function
#End Region

#Region "Capture Selection Changed-Events"
    ''' <summary>
    ''' Throw Selected Index Changed
    ''' </summary>
    Private Sub CB_SelectedColumnIndexChanged() Handles cbEntry.SelectedIndexChanged
        If Not Me.bReady Then Return

        With Me.cbEntry
            If Not .SelectedItem Is Nothing Then
                Me._SelectedEntries.Clear()
                Me._SelectedEntry = DirectCast(.SelectedItem, String)
                Me._SelectedEntries.Add(Me._SelectedEntry)

                ' Save the last selected column names in the settings
                If TurnOnLastSelectionSaving Then
                    Me.SetLastSelection(New StringCollection() From {Me._SelectedEntry})
                End If
            End If
        End With
        RaiseEvent SelectedIndexChanged()
    End Sub

    ''' <summary>
    ''' Throw Selected Index Changed
    ''' </summary>
    Private Sub LB_SelectedColumnIndexChanged() Handles lbEntry.SelectedIndexChanged
        If Not Me.bReady Then Return

        With Me.lbEntry
            If .SelectedItems IsNot Nothing Then
                Me._SelectedEntries.Clear()
                If .SelectedItems.Count > 1 Then
                    For Each Item As String In .SelectedItems
                        Me._SelectedEntries.Add(Item)
                    Next
                    Me._SelectedEntry = "multiple columns selected"

                    ' Save the last selected column names in the settings
                    If TurnOnLastSelectionSaving Then
                        Dim l As New StringCollection()
                        l.AddRange(Me._SelectedEntries.ToArray)
                        Me.SetLastSelection(l)
                    End If

                ElseIf .SelectedItems.Count = 1 Then
                    Me._SelectedEntry = DirectCast(.SelectedItem, String)
                    Me._SelectedEntries.Add(Me._SelectedEntry)

                    ' Save the last selected column names in the settings
                    If TurnOnLastSelectionSaving Then
                        Me.SetLastSelection(New StringCollection() From {Me._SelectedEntry})
                    End If
                Else
                    ' Nothing
                End If
            Else
                Me._SelectedEntries.Clear()
                Me._SelectedEntry = String.Empty
            End If
        End With
        RaiseEvent SelectedIndexChanged()
    End Sub
#End Region

#Region "Context-Menu filtering"

    ''' <summary>
    ''' Open context menu.
    ''' </summary>
    Private Sub pbContextMenu_Click(sender As Object, e As EventArgs) Handles pbContextMenu.Click
        Me.cmnuFilter.Show(Me, Me.pbContextMenu.Location)
    End Sub

    ''' <summary>
    ''' Close the filter on pressing enter.
    ''' </summary>
    Private Sub cmnuFilterText_KeyDown(sender As Object, e As KeyEventArgs) Handles cmnuFilterText.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.cmnuFilter.Close()
        End If
    End Sub

    ''' <summary>
    ''' on changing the filter text, rerun RegEx filtering of the column names.
    ''' </summary>
    Private Sub cmnuFilterCode_TextChanged(sender As Object, e As EventArgs) Handles cmnuFilterText.TextChanged
        Me.FilterColumnNames(Me.cmnuFilterText.Text)
    End Sub

    ''' <summary>
    ''' clear the filter
    ''' </summary>
    Private Sub cmnuFilterClear_Click(sender As Object, e As EventArgs) Handles cmnuFilterClear.Click
        Me.cmnuFilterText.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Use a regex to filter the column-names.
    ''' Replace * by .*? in the regex.
    ''' </summary>
    Private Sub FilterColumnNames(ByVal FilterText As String)

        ' Trim empty space
        FilterText = FilterText.Trim

        ' Empty filtered list
        Me.ListOfEntriesFiltered.Clear()

        ' Return without regex, if the filter is empty.
        If FilterText = String.Empty Then
            For i As Integer = 0 To Me.ListOfEntries.Count - 1 Step 1
                Me.ListOfEntriesFiltered.Add(Me.ListOfEntries(i))
            Next
        Else
            Try
                ' replace * by regex *, and escape other signs
                FilterText = Regex.Escape(FilterText)
                FilterText = FilterText.Replace("\*", ".*?")

                Dim RegexMatch As New Regex("^" & FilterText & "$", RegexOptions.IgnoreCase Or RegexOptions.Singleline)

                ' add all matching names
                For i As Integer = 0 To Me.ListOfEntries.Count - 1 Step 1
                    If RegexMatch.IsMatch(Me.ListOfEntries(i)) Then Me.ListOfEntriesFiltered.Add(Me.ListOfEntries(i))
                Next
            Catch ex As Exception
                For i As Integer = 0 To Me.ListOfEntries.Count - 1 Step 1
                    Me.ListOfEntriesFiltered.Add(Me.ListOfEntries(i))
                Next
            End Try
        End If

        ' refresh the displayed list
        Me.SetListValue(Me.ListOfEntriesFiltered)

        ' Save the filtered text for the next time
        If TurnOnLastFilterSaving Then
            Me.SetLastFilterText(FilterText)
        End If

    End Sub

#End Region

#Region "Load and save the last used filters"

    ''' <summary>
    ''' Appends the last used filter to the list.
    ''' </summary>
    Private Sub AppendToFilterHistory(ByVal Filter As String)

        ' Append filters to history
        Me.SetLastUsedFilters(cStringCollectionHistory.AppendToHistory(Me.GetLastUsedFilters,
                                                                       Filter,
                                                                       Me.LastFilterHistory_MaxCount))

        ' Save the settings
        cGlobal.SaveSettings()

        ' Update the drop down history
        Me.UpdateFilterHistory()
    End Sub

    ''' <summary>
    ''' Add the current filter text to the history.
    ''' </summary>
    Private Sub cmnuFilterAddToHistory_Click(sender As Object, e As EventArgs) Handles cmnuFilterAddToHistory.Click
        Me.AppendToFilterHistory(Me.cmnuFilterText.Text)
    End Sub

    ''' <summary>
    ''' Show the history entries.
    ''' </summary>
    Private Sub cmnuFilter_Opening(sender As Object, e As EventArgs) Handles cmnuFilter.Opening
        Me.UpdateFilterHistory()
    End Sub

    ''' <summary>
    ''' Updates the drop down history.
    ''' </summary>
    Private Sub UpdateFilterHistory()
        With Me.cmnuLastUsedFilters

            ' remove the handlers
            For i As Integer = 0 To .DropDownItems.Count - 1 Step 1
                RemoveHandler .DropDownItems(i).Click, AddressOf SelectFilterFromHistory
            Next

            ' remove the old buttons
            .DropDownItems.Clear()

            ' add button and handlers
            If My.Settings.DataColumnFilter_LastUsedFilters.Count > 0 Then
                .Enabled = True
                ' Add the stored history
                For i As Integer = Me.GetLastUsedFilters.Count - 1 To 0 Step -1
                    .DropDownItems.Add(Me.GetLastUsedFilters(i))
                    AddHandler .DropDownItems(Me.GetLastUsedFilters.Count - 1 - i).Click, AddressOf SelectFilterFromHistory
                Next
            Else
                .Enabled = False
            End If

        End With
    End Sub

    ''' <summary>
    ''' remove the history entries
    ''' </summary>
    Private Sub UserControl_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed

        With Me.cmnuLastUsedFilters
            ' remove the handlers
            For i As Integer = 0 To .DropDownItems.Count - 1 Step 1
                RemoveHandler .DropDownItems(i).Click, AddressOf Me.SelectFilterFromHistory
            Next

            ' remove the old buttons
            .DropDownItems.Clear()
        End With

    End Sub

    ''' <summary>
    ''' Select a filter from the history.
    ''' </summary>
    Private Sub SelectFilterFromHistory(sender As Object, e As EventArgs)
        Dim B As ToolStripDropDownItem = DirectCast(sender, ToolStripDropDownItem)

        Me.cmnuFilterText.Text = B.Text
    End Sub

#End Region

End Class
