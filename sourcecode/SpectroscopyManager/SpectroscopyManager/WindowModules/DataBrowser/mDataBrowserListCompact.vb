Public Class mDataBrowserListCompact

#Region "Properties"

    ''' <summary>
    ''' Local copy of the file-list.
    ''' </summary>
    Private _FileList As cFileImport

    Private bReady As Boolean = False

    ''' <summary>
    ''' Show the spectroscopy-tables in the file-list.
    ''' </summary>
    Public Property ShowSpectroscopyTables As Boolean = True

    ''' <summary>
    ''' Show the scan images in the file-list.
    ''' </summary>
    Public Property ShowScanImages As Boolean = True

    ''' <summary>
    ''' Selection mode of this list.
    ''' </summary>
    Public Property SelectionMode As SelectionMode
        Get
            Return Me.lbFiles.SelectionMode
        End Get
        Set(value As SelectionMode)
            Me.lbFiles.SelectionMode = value
        End Set
    End Property

#End Region

#Region "Events"

    ''' <summary>
    ''' Selected Fileobjects change.
    ''' </summary>
    Public Event SelectionChanged(ByRef SelectedFileObjects As Dictionary(Of String, cFileObject))

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    ''' <summary>
    ''' Constructor, that takes a File-List and stores a local reference.
    ''' </summary>
    Public Sub SetFileList(ByRef FileList As cFileImport)
        Me._FileList = FileList
        Me.DrawList()
    End Sub

#End Region

#Region "List-Drawing"

    ''' <summary>
    ''' Fills the acutal list-box.
    ''' </summary>
    Private Sub DrawList()

        Me.bReady = False

        ' Store the last selected entries.
        Dim LastSelectedEntries As Dictionary(Of String, cFileObject) = Me.SelectedEntries

        ' Clear the item-list.
        Me.lbFiles.Items.Clear()

        ' Set the display and the value properties.
        Me.lbFiles.DisplayMember = "Value"
        Me.lbFiles.ValueMember = "Key"

        ' Add all the names.
        For Each FileKV As KeyValuePair(Of String, cFileObject) In Me._FileList.FileBuffer_Filtered

            ' Filter unwanted entries.
            Select Case FileKV.Value.FileType
                Case cFileObject.FileTypes.SpectroscopyTable
                    If Not Me.ShowSpectroscopyTables Then Continue For
                Case cFileObject.FileTypes.ScanImage
                    If Not Me.ShowScanImages Then Continue For
                Case Else
                    Continue For
            End Select

            ' Add the name to the list
            Me.lbFiles.Items.Add(New KeyValuePair(Of String, String)(FileKV.Key, FileKV.Value.FileNameWithoutPath))

        Next

        ' Restore the selection.
        Me.SetSelectedEntries(LastSelectedEntries.Keys.ToList)

        Me.bReady = True

    End Sub

#End Region

#Region "Data Selection"

    ''' <summary>
    ''' Returns the selected entries of this list-box.
    ''' </summary>
    Public Function SelectedEntries() As Dictionary(Of String, cFileObject)
        Dim ReturnList As New Dictionary(Of String, cFileObject)(Me.lbFiles.SelectedItems.Count)

        For Each EntryKV As KeyValuePair(Of String, String) In Me.lbFiles.SelectedItems
            ReturnList.Add(EntryKV.Key, Me._FileList.FileBuffer_Full(EntryKV.Key))
        Next

        Return ReturnList
    End Function

    ''' <summary>
    ''' Entry-List to select.
    ''' </summary>
    Public Sub SetSelectedEntries(ByVal ListOfEntries As List(Of String))
        Dim EntryKey As String
        Me.bReady = False
        Me.lbFiles.BeginUpdate()
        For i As Integer = 0 To Me.lbFiles.Items.Count - 1 Step 1
            EntryKey = DirectCast(Me.lbFiles.Items(i), KeyValuePair(Of String, String)).Key
            Me.lbFiles.SetSelected(i, ListOfEntries.Contains(EntryKey))
        Next
        Me.lbFiles.EndUpdate()
        Me.bReady = True
        RaiseEvent SelectionChanged(Me.SelectedEntries)
    End Sub

    ''' <summary>
    ''' Selection changed.
    ''' </summary>
    Private Sub lbFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbFiles.SelectedIndexChanged
        If Not Me.bReady Then Return
        RaiseEvent SelectionChanged(Me.SelectedEntries)
    End Sub

    ''' <summary>
    ''' Key pressed on the control... use it to ease selections. E.g. Ctrl+A
    ''' </summary>
    Private Sub lbFiles_KeyDown(sender As Object, e As KeyEventArgs) Handles lbFiles.KeyDown
        If Not Me.bReady Then Return

        If (e.KeyCode And Not Keys.Modifiers) = Keys.A AndAlso e.Modifiers = Keys.Control Then
            Me.SetSelectedEntries(Me._FileList.FileBuffer_Filtered.Keys.ToList)
        End If

    End Sub

#End Region

End Class
