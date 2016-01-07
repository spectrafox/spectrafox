Imports Amib.Threading

''' <summary>
''' Base Class for fetching a desired Spectroscopy Table in a background-Thread
''' </summary>
Public Class cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' SpectroscopyTable Object
    ''' </summary>
    Protected oSpectroscopyTable As cSpectroscopyTable

    ''' <summary>
    ''' Returns the currently loaded Spectroscopy-Table,
    ''' </summary>
    Public Property CurrentSpectroscopyTable As cSpectroscopyTable
        Get
            Return Me.oSpectroscopyTable
        End Get
        Set(value As cSpectroscopyTable)
            Me.oSpectroscopyTable = value
        End Set
    End Property

    ''' <summary>
    ''' File-Object of the Spectroscopy-File
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
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' Allows to hand over a Thread-Pool that is used to manage the 
    ''' fetch-thread. If not set, or set to nothing, a separate,
    ''' non-managed thread will be used.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject,
                   Optional ByRef ThreadPoolToUse As SmartThreadPool = Nothing,
                   Optional ByVal FetchPriority As WorkItemPriority = WorkItemPriority.Normal,
                   Optional ByVal FetchOnlyFileHeader As Boolean = False)
        ' Add file to local Object
        Me.CurrentFileObject = SpectroscopyFile
        Me._ThreadPool = ThreadPoolToUse
        Me.FetchThreadPriority = FetchPriority
        Me._FetchOnlyFileHeader = FetchOnlyFileHeader
    End Sub
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the spectroscopytable was successfully loaded.
    ''' </summary>
    Public Event FileFetchedComplete(ByRef SpectroscopyTable As cSpectroscopyTable)
#End Region

#Region "File Fetch Function"
    ''' <summary>
    ''' Starts the Fetching Procedure for the selected File. (ASYNC)
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
    ''' Starts the Fetching Procedure for the selected File (DIRECT)
    ''' </summary>
    Public Sub FetchDirect()
        Me.FileFetcherSub(Me.CurrentFileObject)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Fetcher to load the spectroscopy file.
    ''' </summary>
    Private Function FileFetcher(FileObjectToLoad As Object) As Object
        Dim oFileObjectToLoad As cFileObject = CType(FileObjectToLoad, cFileObject)

        ' Load the File from Disk
        Me.oSpectroscopyTable = Nothing
        If cFileImport.GetSpectroscopyFile(oFileObjectToLoad, oSpectroscopyTable, Me._FetchOnlyFileHeader) Then
            RaiseEvent FileFetchedComplete(Me.oSpectroscopyTable)
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
