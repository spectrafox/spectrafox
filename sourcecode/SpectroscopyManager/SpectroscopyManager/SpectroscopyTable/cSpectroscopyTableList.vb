Imports System.Threading
Imports Amib.Threading

Public Class cSpectroscopyTableList
    Implements iSingleSpectroscopyTableLoaded

#Region "Thead Pool and ListEntry Properties"
    ''' <summary>
    ''' List for the Current FileObjects representing the List
    ''' </summary>
    Private _CurrentFileList As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Returns the current fileobject list that is ready for loading.
    ''' </summary>
    Public ReadOnly Property CurrentFileList As Dictionary(Of String, cFileObject)
        Get
            Return Me._CurrentFileList
        End Get
    End Property

    ''' <summary>
    ''' Currently used Threadpool to load data.
    ''' </summary>
    Private _ThreadPool As SmartThreadPool

    ''' <summary>
    ''' Returns the currently used thread pool to load Spectroscopy Files.
    ''' </summary>
    Public ReadOnly Property ThreadPool As SmartThreadPool
        Get
            Return Me._ThreadPool
        End Get
    End Property

    ''' <summary>
    ''' Priority for fetching a list-entry.
    ''' </summary>
    Public Property ListEntryFetchPriority As WorkItemPriority = WorkItemPriority.BelowNormal

#End Region

#Region "Constructor"
    ''' <summary>
    ''' Sets the List of Spectroscopy-Table-Objects to be displayed.
    ''' </summary>
    Public Sub New(ByRef FileList As Dictionary(Of String, cFileObject),
                   ByRef ThreadPoolToUse As SmartThreadPool)
        ' Set the local list
        Me._CurrentFileList = FileList
        Me._ThreadPool = ThreadPoolToUse
    End Sub
#End Region

#Region "ListEntry Fetcher to Display Spectroscopy-File-Informations"
    ''' <summary>
    ''' Represents a List-Entry of a Spectroscopy-File.
    ''' </summary>
    Public Structure SpectroscopyListEntry
        Implements wDataBrowser.iListEntry
        Public Property ColumnNames As List(Of String) Implements wDataBrowser.iListEntry.ColumnNames
        Public Property Comment As String Implements wDataBrowser.iListEntry.Comment
        Public Property FileName As String Implements wDataBrowser.iListEntry.FileName
        Public Property FullFileName As String Implements wDataBrowser.iListEntry.FullFileName
        Public Property MeasurementPoints As String Implements wDataBrowser.iListEntry.MeasurementPoints
        Public Property PreviewImage As Image Implements wDataBrowser.iListEntry.PreviewImage
        Public Property RecordDate As Date Implements wDataBrowser.iListEntry.RecordDate
    End Structure

    ''' <summary>
    ''' List for saving the visible Spectroscopy-Table-Entries
    ''' </summary>
    Private _ListEntryList As New Dictionary(Of String, SpectroscopyListEntry)

    ''' <summary>
    ''' Returns the list of currently available ListEntries.
    ''' </summary>
    Public ReadOnly Property ListEntryList As Dictionary(Of String, SpectroscopyListEntry)
        Get
            Return Me._ListEntryList
        End Get
    End Property

    ''' <summary>
    ''' List that saves, if a file is already beeing fetched at the moment, to avoid double loading files.
    ''' </summary>
    Private ListOfListEntriesCurrentlyFetched As New List(Of String)

    ''' <summary>
    ''' Callback-Function to load the ListEntryList in separate Threads.
    ''' </summary>
    Private ListEntryFetcherCallback As New WorkItemCallback(AddressOf ListEntryFetcher)

    ''' <summary>
    ''' Mutex for blocking Threads that access the ListEntryObjects.
    ''' </summary>
    Private ListEntryFetchedMutex As New Mutex

    ''' <summary>
    ''' Event that gets fired, when a ListEntry was successfully created from a given ScanObject.
    ''' </summary>
    Public Event ListEntryFetchComplete(ByRef ListEntry As SpectroscopyListEntry)

    ''' <summary>
    ''' Preview-Image-Settings for the PreviewImage of a ListEntry
    ''' </summary>
    Public Property PreviewImageColumnName_X As String = My.Settings.LastPreviewImageList_ColumnNameX
    Public Property PreviewImageColumnName_Y As String = My.Settings.LastPreviewImageList_ColumnNameY
    Public Property PreviewImageWidth As Integer = My.Settings.LastPreviewImageList_Width
    Public Property PreviewImageHeigth As Integer = My.Settings.LastPreviewImageList_Height
    Public Property PreviewImageLogX As Boolean = My.Settings.ListPreviewImage_LogX
    Public Property PreviewImageLogY As Boolean = My.Settings.ListPreviewImage_LogY
    Public Property PreviewImageReducePointsForProcessing As Boolean = My.Settings.ListPreviewImage_ReducePoints

    ''' <summary>
    ''' Loads a ListEntry of the List from the SpectroscopyTableObject in a separate Thread.
    ''' </summary>
    Public Sub FetchListEntry(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded
        ' Send FileName with fetcher to ThreadPoolQueue
        If Not Me.ListOfListEntriesCurrentlyFetched.Contains(SpectroscopyTable.FullFileName) And
           Me._CurrentFileList.ContainsKey(SpectroscopyTable.FullFileName) Then
            ListOfListEntriesCurrentlyFetched.Add(SpectroscopyTable.FullFileName)

            ' Give the List-Fetching usually only a low priority
            Me._ThreadPool.QueueWorkItem(ListEntryFetcherCallback, SpectroscopyTable, Me.ListEntryFetchPriority)
        End If
    End Sub

    ''' <summary>
    ''' If the ListEntry was created successfully, then remove the List-Entry from the List
    ''' of currently fetched entries, and add the List-Entry-Object to the current ListOfListEntries.
    ''' </summary>
    Private Sub ListEntryFetcher_FetchComplete(ByRef ListEntry As SpectroscopyListEntry) Handles Me.ListEntryFetchComplete
        ' Block Threads to the used ressource
        Me.ListEntryFetchedMutex.WaitOne()

        SyncLock Me._ListEntryList
            ' Add the ListEntry to the ListEntryList
            If Not Me._ListEntryList.ContainsKey(ListEntry.FullFileName) Then
                Me._ListEntryList.Add(ListEntry.FullFileName, ListEntry)
            End If
        End SyncLock

        SyncLock Me.ListOfListEntriesCurrentlyFetched
            ' Remove File from the CurrentFetching List
            Me.ListOfListEntriesCurrentlyFetched.Remove(ListEntry.FullFileName)
        End SyncLock

        ' Release the Mutex again
        Me.ListEntryFetchedMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' File Fetcher to load the SpectroscopyTable-Files displayed in the list asynchronously.
    ''' </summary>
    Private Function ListEntryFetcher(ByVal SpectroscopyTableObject As Object) As Object
        Dim oSpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        ' Load the Preview-Image, if the Columns
        ' could be found in the Table.

        Dim iX As Integer = -1
        Dim iY As Integer = -1

        ' Check in the Spectroscopy-Table, if there are
        ' Columns with the Names to be displayed as Preview-Image.
        For Each oColumn As cSpectroscopyTable.DataColumn In oSpectroscopyTable.GetColumnList
            If oColumn.Name = Me.PreviewImageColumnName_X Then iX = oColumn.Index
            If oColumn.Name = Me.PreviewImageColumnName_Y Then iY = oColumn.Index
        Next

        ' Send Abort-Image, if the Columns were not found.
        Dim PreviewImage As Image
        If iX <> -1 And iY <> -1 Then
            PreviewImage = oSpectroscopyTable.GetPreviewImage(iX,
                                                           iY,
                                                           Me.PreviewImageWidth,
                                                           Me.PreviewImageHeigth,
                                                           Me.PreviewImageLogX,
                                                           Me.PreviewImageLogY,
                                                           Me.PreviewImageReducePointsForProcessing)
        Else
            PreviewImage = My.Resources.columns_do_not_exist
        End If

        ' Create new ListEntry
        Dim ListEntry As New SpectroscopyListEntry
        With oSpectroscopyTable
            ListEntry.FullFileName = .FullFileName
            ListEntry.FileName = .FileNameWithoutPath
            ListEntry.Comment = .Comment
            ListEntry.ColumnNames = .GetColumnNameList
            ListEntry.RecordDate = .RecordDate
            ListEntry.PreviewImage = PreviewImage
            ListEntry.MeasurementPoints = .MeasurementPoints.ToString("N0")
        End With

        RaiseEvent ListEntryFetchComplete(ListEntry)

        Return Nothing
    End Function
#End Region

#Region "Functions"
    ''' <summary>
    ''' Clears all fetched Objects from the list.
    ''' Usefull to initialize a reload for all files.
    ''' </summary>
    Public Sub ResetFetchedList()
        Me.ListOfListEntriesCurrentlyFetched.Clear()
        Me.ListEntryList.Clear()
    End Sub

    ''' <summary>
    ''' Removes a single entry from the ListEntryList.
    ''' Usefull to request a refetch of this entry.
    ''' </summary>
    Public Sub RemoveListEntry(ByVal Key As String)
        If Me._ListEntryList.ContainsKey(Key) Then
            Me._ListEntryList.Remove(Key)
        End If
    End Sub
#End Region

#Region "File Fetcher Event-Handling for different Purposes"

#Region "Multi-Threaded-Mutex"
    ''' <summary>
    ''' Mutex for synchronizing the Fetch-Complete Event-Handling
    ''' </summary>
    Private FileFetcherMutex As New Mutex
#End Region

#Region "Sub-Class of a Fetch-Object to keep track of the Callback-Functions to be loaded after the fetch."
    ''' <summary>
    ''' Fetch-Object to save all the callback-functions
    ''' and the currently running File-Fetcher-Objects.
    ''' </summary>
    Protected Class FetchObject
        ''' <summary>
        ''' Saves the File-Fetcher for that file-object.
        ''' </summary>
        Public ReadOnly SpectroscopyTableFetcher As cSpectroscopyTableFetcher

        ''' <summary>
        ''' Saves all callback-references for files that were fetched
        ''' by a single-request.
        ''' </summary>
        Public CallbackFunctions_SingleFetch As New List(Of iSingleSpectroscopyTableLoaded)

        ''' <summary>
        ''' Saves all callback-references for files that were fetched
        ''' by a multiple-fetch-request. The total amount for each callback is saved,
        ''' to just call the callback, if the total fetch was completed.
        ''' </summary>
        Public CallbackFunctions_MultipleFetch As New List(Of iMultipleSpectroscopyTablesLoaded)

        ''' <summary>
        ''' Constructor to set the Fetcher for this Object.
        ''' </summary>
        Public Sub New(ByRef Fetcher As cSpectroscopyTableFetcher)
            Me.SpectroscopyTableFetcher = Fetcher
        End Sub
    End Class
#End Region

#Region "File-Fetcher-Lists to keep track of running fetches and their callback functions!"
    ''' <summary>
    ''' Dictionary, that contains all running file-fetcher objects,
    ''' saved with the callback-functions to load after fetching is complete.
    ''' </summary>
    Private dFileFetcherList As New Dictionary(Of String, FetchObject)
#End Region

#Region "Loading-Functions to initialize a Fetch"
    ''' <summary>
    ''' Loads a SINGLE SpectroscopyTable-File by starting a background-thread.
    ''' Needs a callback-function that implements iSingleSpectroscopyTableLoaded to call
    ''' after the loading is complete.
    ''' </summary>
    Public Sub LoadFile(ByVal FullFileName As String,
                        ByVal CallbackFunction As iSingleSpectroscopyTableLoaded,
                        Optional ByVal ThreadPriority As WorkItemPriority = WorkItemPriority.Normal,
                        Optional ByVal FetchOnlyFileHeader As Boolean = False)
        If Not Me._CurrentFileList.ContainsKey(FullFileName) Then Return

        ' Block Thread, if other Thread is using current resources
        Me.FileFetcherMutex.WaitOne()

        ' Create new FileFetcher Object, or add to a running file-fetcher object.
        SyncLock dFileFetcherList
            If Not dFileFetcherList.ContainsKey(FullFileName) Then
                ' Create new object
                Dim FO As New FetchObject(New cSpectroscopyTableFetcher(Me._CurrentFileList(FullFileName),
                                                                        Me._ThreadPool,
                                                                        ThreadPriority,
                                                                        FetchOnlyFileHeader))

                ' Add Callback
                FO.CallbackFunctions_SingleFetch.Add(CallbackFunction)

                ' Register FetchComplete-Event
                AddHandler FO.SpectroscopyTableFetcher.FileFetchedComplete, AddressOf Me.FileFetcherAction_FetchComplete

                ' Add FetchObject to the Fetcher-List
                dFileFetcherList.Add(FullFileName, FO)

                ' Run the Fetcher in the Background-Thread
                dFileFetcherList(FullFileName).SpectroscopyTableFetcher.FetchAsync()
            Else
                ' Just add the Callback to the FetchObject, if the fetch is currently running.
                If Not dFileFetcherList(FullFileName).CallbackFunctions_SingleFetch.Contains(CallbackFunction) Then
                    dFileFetcherList(FullFileName).CallbackFunctions_SingleFetch.Add(CallbackFunction)
                End If
            End If
        End SyncLock

        ' Releases the Mutex again
        Me.FileFetcherMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Loads MULTIPLE SpectroscopyTable-Files by starting several background-threads.
    ''' Needs a callback-function that implements iMultipleSpectroscopyTablesLoaded to call
    ''' after the loading is complete.
    ''' </summary>
    Public Sub LoadFiles(ByVal FullFileNames As List(Of String),
                         ByVal CallbackFunction As iMultipleSpectroscopyTablesLoaded,
                         Optional ByVal ThreadPriority As WorkItemPriority = WorkItemPriority.Normal)

        ' Check, if all files are present in the current file-list.
        For Each FullFileName As String In FullFileNames
            If Not Me._CurrentFileList.ContainsKey(FullFileName) Then Return
        Next

        ' Set the total number of file to fetch in the Interface-Class
        CallbackFunction.TotalNumberOfFilesToFetch = FullFileNames.Count

        ' Block Thread, if other Thread is using current resources
        Me.FileFetcherMutex.WaitOne()

        For Each FullFileName As String In FullFileNames
            ' Create new FileFetcher Object, or add to a running file-fetcher object.
            If Not dFileFetcherList.ContainsKey(FullFileName) Then

                ' Create new object
                Dim FO As New FetchObject(New cSpectroscopyTableFetcher(Me._CurrentFileList(FullFileName),
                                                                        Me._ThreadPool,
                                                                        ThreadPriority))

                ' Add Callback
                FO.CallbackFunctions_MultipleFetch.Add(CallbackFunction)

                ' Register FetchComplete-Event
                AddHandler FO.SpectroscopyTableFetcher.FileFetchedComplete, AddressOf Me.FileFetcherAction_FetchComplete

                ' Add FetchObject to the Fetcher-List
                dFileFetcherList.Add(FullFileName, FO)

                ' Run the Fetcher in the Background-Thread
                dFileFetcherList(FullFileName).SpectroscopyTableFetcher.FetchAsync()
            Else
                ' Just add the Callback to the FetchObject, if the fetch is currently running.
                If Not dFileFetcherList(FullFileName).CallbackFunctions_MultipleFetch.Contains(CallbackFunction) Then
                    dFileFetcherList(FullFileName).CallbackFunctions_MultipleFetch.Add(CallbackFunction)
                End If
            End If
        Next

        ' Releases the Mutex again
        Me.FileFetcherMutex.ReleaseMutex()
    End Sub
#End Region

#Region "Fetch-Complete Handling"
    ''' <summary>
    ''' If a SpectroscopyTable-File was fetched successfully, perform the requested actions,
    ''' with the file, depending on why the fetcher was runned.
    ''' </summary>
    Private Sub FileFetcherAction_FetchComplete(ByRef SpectroscopyTable As cSpectroscopyTable)

        ' Create storage for the list of Callback-Functions
        Dim SingleCallbackList As New List(Of iSingleSpectroscopyTableLoaded)
        Dim MultipleCallbackList As New List(Of iMultipleSpectroscopyTablesLoaded)

        ' Block Thread, if other Thread is using current resources
        Me.FileFetcherMutex.WaitOne()

        ' Check, if FetchObject exists?
        If Me.dFileFetcherList.ContainsKey(SpectroscopyTable.FullFileName) Then

            ' Extract the Callback-references.
            SingleCallbackList = Me.dFileFetcherList(SpectroscopyTable.FullFileName).CallbackFunctions_SingleFetch
            MultipleCallbackList = Me.dFileFetcherList(SpectroscopyTable.FullFileName).CallbackFunctions_MultipleFetch

            ' Remove the Fetcher from the Fetching-List
            Me.dFileFetcherList.Remove(SpectroscopyTable.FullFileName)
        End If

        ' Releases the Mutex again
        Me.FileFetcherMutex.ReleaseMutex()

        '###########################
        ' SINGLE CALLBACK FUNCTIONS
        ' Go through all single-callbacks and load the corresponding Interface-functions!
        For Each Callback As iSingleSpectroscopyTableLoaded In SingleCallbackList
            Callback.SpectroscopyTableLoaded(SpectroscopyTable)
        Next
        '###########################

        '#############################
        ' MULTIPLE CALLBACK FUNCTIONS
        ' Go through all multiple-callbacks and decrease the counter for each load!
        For Each Callback As iMultipleSpectroscopyTablesLoaded In MultipleCallbackList
            Callback.OneOfAllSpectroscopyTablesFetched(SpectroscopyTable)

            ' Decrease the "Files-To-Load"-Counter by one,
            ' and call the final function, if all
            Callback.TotalNumberOfFilesToFetch -= 1
            If Callback.TotalNumberOfFilesToFetch <= 0 Then
                Callback.AllSpectroscopyTablesLoaded()
            End If
        Next
        '#############################
    End Sub
#End Region

#End Region

End Class