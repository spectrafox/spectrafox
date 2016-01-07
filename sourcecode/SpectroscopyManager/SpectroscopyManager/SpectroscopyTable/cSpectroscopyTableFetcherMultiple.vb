Imports Amib.Threading

''' <summary>
''' Base Class for fetching multiple desired tables in a background-thread
''' </summary>
Public Class cSpectroscopyTableFetcherMultiple

#Region "Properties"
    ''' <summary>
    ''' SpectroscopyTable Object List
    ''' </summary>
    Protected lSpectroscopyTableList As New Dictionary(Of cFileObject, cSpectroscopyTable)

    ''' <summary>
    ''' Returns the currently loaded Spectroscopy-Table,
    ''' </summary>
    Public Property SpectroscopyTables As Dictionary(Of cFileObject, cSpectroscopyTable)
        Get
            Return Me.lSpectroscopyTableList
        End Get
        Set(value As Dictionary(Of cFileObject, cSpectroscopyTable))
            Me.lSpectroscopyTableList = value
        End Set
    End Property

    ''' <summary>
    ''' Priority of the background thread used.
    ''' </summary>
    Private FetchThreadPriority As WorkItemPriority

    ''' <summary>
    ''' Currently used Threadpool to load data.
    ''' </summary>
    Private _ThreadPool As SmartThreadPool

    ''' <summary>
    ''' Just fetch the header of the file?
    ''' Speeds things up, if data not needed!
    ''' </summary>
    Private _FetchOnlyFileHeader As Boolean
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' Allows to hand over a Thread-Pool that is used to manage the 
    ''' fetch-thread. If not set, or set to nothing, a separate,
    ''' non-managed thread will be used.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFiles As List(Of cFileObject),
                   Optional ByRef ThreadPoolToUse As SmartThreadPool = Nothing,
                   Optional ByVal FetchPriority As WorkItemPriority = WorkItemPriority.Normal,
                   Optional ByVal FetchOnlyFileHeader As Boolean = False)
        Me.New(SpectroscopyFiles.ToArray,
               ThreadPoolToUse,
               FetchPriority,
               FetchOnlyFileHeader)
    End Sub

    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' Allows to hand over a Thread-Pool that is used to manage the 
    ''' fetch-thread. If not set, or set to nothing, a separate,
    ''' non-managed thread will be used.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFiles As cFileObject(),
                   Optional ByRef ThreadPoolToUse As SmartThreadPool = Nothing,
                   Optional ByVal FetchPriority As WorkItemPriority = WorkItemPriority.Normal,
                   Optional ByVal FetchOnlyFileHeader As Boolean = False)
        ' Add file to local Object
        lSpectroscopyTableList.Clear()
        For Each FileObject In SpectroscopyFiles
            lSpectroscopyTableList.Add(FileObject, Nothing)
        Next

        ' Set up the settings
        Me._ThreadPool = ThreadPoolToUse
        Me.FetchThreadPriority = FetchPriority
        Me._FetchOnlyFileHeader = FetchOnlyFileHeader
    End Sub
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when ALL the SpectroscopyTables were successfully loaded.
    ''' </summary>
    Public Event FileFetchedComplete()

    ''' <summary>
    ''' Event that gets fired, when one of the SpectroscopyTables was successfully loaded.
    ''' </summary>
    Public Event SingleFileFetched(ByVal CurrentCount As Integer, ByVal TotalCount As Integer, ByRef SpectroscopyTable As cSpectroscopyTable)

    ''' <summary>
    ''' Event that gets fired, when one of the SpectroscopyTables could not be loaded.
    ''' </summary>
    Public Event SingleFileFetchFailed(ByVal CurrentCount As Integer, ByVal TotalCount As Integer, ByRef SpectroscopyTable As cSpectroscopyTable)
#End Region

#Region "File Fetch Function"
    ''' <summary>
    ''' Starts the Fetching Procedure for the selected File. (ASYNC)
    ''' </summary>
    Public Sub FetchAsync()

        ' If no thread-pool is definied, use a non-managed thread to load the data.
        If Me._ThreadPool Is Nothing Then
            Dim NonMangedThread As New Threading.Thread(AddressOf Me.FileFetcherSub)
            NonMangedThread.Start(Me.lSpectroscopyTableList)
        Else
            ' Send FileObject to ThreadPoolQueue
            Me._ThreadPool.QueueWorkItem(AddressOf Me.FileFetcher,
                                         Me.lSpectroscopyTableList,
                                         Me.FetchThreadPriority)
        End If

    End Sub

    ''' <summary>
    ''' Starts the Fetching Procedure for the selected File (DIRECT)
    ''' </summary>
    Public Sub FetchDirect()
        Me.FileFetcherSub(Me.lSpectroscopyTableList)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Fetcher to load the spectroscopy files.
    ''' </summary>
    Private Function FileFetcher(FileObjectsToLoad As Object) As Object
        Dim lFileObjectsToLoad As Dictionary(Of cFileObject, cSpectroscopyTable) = CType(FileObjectsToLoad, Dictionary(Of cFileObject, cSpectroscopyTable))

        ' Load the Files from Disk
        Dim oSpectroscopyTable As cSpectroscopyTable = Nothing
        Dim FileObjectCollection As List(Of cFileObject) = lFileObjectsToLoad.Keys.ToList

        Dim i As Integer = 0
        For Each FileObject As cFileObject In FileObjectCollection
            i += 1
            oSpectroscopyTable = Nothing
            If cFileImport.GetSpectroscopyFile(FileObject, oSpectroscopyTable, Me._FetchOnlyFileHeader) Then
                Me.lSpectroscopyTableList(FileObject) = oSpectroscopyTable

                ' Raise the status event
                RaiseEvent SingleFileFetched(i, lFileObjectsToLoad.Count, oSpectroscopyTable)
            Else
                ' Raise the status event
                RaiseEvent SingleFileFetchFailed(i, lFileObjectsToLoad.Count, oSpectroscopyTable)
            End If
        Next

        oSpectroscopyTable = Nothing
        RaiseEvent FileFetchedComplete()
        Return Nothing
    End Function

    ''' <summary>
    ''' Sub-Delegate version
    ''' </summary>
    Private Sub FileFetcherSub(FileObjectsToLoad As Object)
        Me.FileFetcher(FileObjectsToLoad)
    End Sub
#End Region

End Class
