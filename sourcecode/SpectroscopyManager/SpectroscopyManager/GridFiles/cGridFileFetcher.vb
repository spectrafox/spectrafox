Imports Amib.Threading

''' <summary>
''' Base class for fetching a desired grid file in a background-thread
''' </summary>
Public Class cGridFileFetcher

#Region "Properties"
    ''' <summary>
    ''' GridFile Object
    ''' </summary>
    Protected oGridFile As cGridFile

    ''' <summary>
    ''' Returns the currently loaded GridFile,
    ''' </summary>
    Public Property CurrentGridFile As cGridFile
        Get
            Return Me.oGridFile
        End Get
        Set(value As cGridFile)
            Me.oGridFile = value
        End Set
    End Property

    ''' <summary>
    ''' File-Object of the Grid-File
    ''' </summary>
    Protected CurrentFileObject As cFileObject

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
    ''' Constructor takes the selected GridFile-FileObject.
    ''' Allows to hand over a Thread-Pool that is used to manage the 
    ''' fetch-thread. If not set, or set to nothing, a separate,
    ''' non-managed thread will be used.
    ''' </summary>
    Public Sub New(ByRef GridFile As cFileObject,
                   Optional ByRef ThreadPoolToUse As SmartThreadPool = Nothing,
                   Optional ByVal FetchPriority As WorkItemPriority = WorkItemPriority.Normal,
                   Optional ByVal FetchOnlyFileHeader As Boolean = False)
        ' Add file to local Object
        Me.CurrentFileObject = GridFile
        Me._ThreadPool = ThreadPoolToUse
        Me.FetchThreadPriority = FetchPriority
        Me._FetchOnlyFileHeader = FetchOnlyFileHeader
    End Sub
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the GridFile was successfully loaded.
    ''' </summary>
    Public Event FileFetchedComplete(ByRef GridFile As cGridFile)
#End Region

#Region "File Fetch Function"

    ''' <summary>
    ''' Starts the fetching procedure for the selected File. (ASYNC)
    ''' </summary>
    Public Sub FetchAsync()

        ' If no thread-pool is definied, use a non-managed thread to load the data.
        If Me._ThreadPool Is Nothing Then
            Dim NonMangedThread As New Threading.Thread(AddressOf Me.FileFetcherSub)
            NonMangedThread.Start(Me.CurrentFileObject)
        Else
            ' Send FileObject to ThreadPoolQueue
            Me._ThreadPool.QueueWorkItem(AddressOf Me.FileFetcher,
                                         Me.CurrentFileObject,
                                         Me.FetchThreadPriority)
        End If

    End Sub

    ''' <summary>
    ''' Starts the fetching procedure for the selected file (DIRECT)
    ''' </summary>
    Public Sub FetchDirect()
        Me.FileFetcherSub(Me.CurrentFileObject)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Fetcher to load the Gridfile.
    ''' </summary>
    Private Function FileFetcher(FileObjectToLoad As Object) As Object
        Dim oFileObjectToLoad As cFileObject = CType(FileObjectToLoad, cFileObject)

        ' Load the file from disk
        Me.oGridFile = Nothing
        If cFileImport.GetGridFile(oFileObjectToLoad, oGridFile, Me._FetchOnlyFileHeader) Then
            RaiseEvent FileFetchedComplete(Me.oGridFile)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Sub-Delegate version
    ''' </summary>
    Private Sub FileFetcherSub(FileObjectToLoad As Object)
        Me.FileFetcher(FileObjectToLoad)
    End Sub
#End Region

End Class
