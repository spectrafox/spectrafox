Imports Amib.Threading

''' <summary>
''' Base Class for fetching a desired Scan-Image in a background-Thread
''' </summary>
Public Class cScanImageFetcher

#Region "Properties"
    ''' <summary>
    ''' Scan-Image Object to Load
    ''' </summary>
    Private oScanImage As cScanImage

    ''' <summary>
    ''' Returns the currently loaded Scan-Image Object
    ''' </summary>
    Public ReadOnly Property CurrentScanImage As cScanImage
        Get
            If Me.oScanImage Is Nothing Then Return New cScanImage
            Return Me.oScanImage
        End Get
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
    ''' Constructor takes the selected ScanIamge-FileObject.
    ''' Allows to hand over a Thread-Pool that is used to manage the 
    ''' fetch-thread. If not set, or set to nothing, a separate,
    ''' non-managed thread will be used.
    ''' </summary>
    Public Sub New(ByRef ScanImageFile As cFileObject,
                   Optional ByRef ThreadPoolToUse As SmartThreadPool = Nothing,
                   Optional ByVal FetchPriority As WorkItemPriority = WorkItemPriority.Normal,
                   Optional ByVal FetchOnlyFileHeader As Boolean = False)
        ' Add file to local Object
        Me.CurrentFileObject = ScanImageFile
        Me._ThreadPool = ThreadPoolToUse
        Me.FetchThreadPriority = FetchPriority
        Me._FetchOnlyFileHeader = FetchOnlyFileHeader
    End Sub
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the ScanImage was successfully loaded.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event FileFetchedComplete(ByRef ScanImage As cScanImage)
#End Region

#Region "File Fetch Function"
    ''' <summary>
    ''' Starts the Fetching Procedure for the selected File.
    ''' 
    ''' Works Async with a Background class.
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
    ''' Starts the fetching procedure for the selected File (DIRECT)
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
        Me.oScanImage = Nothing
        If cFileImport.GetScanImageFile(oFileObjectToLoad, oScanImage, Me._FetchOnlyFileHeader) Then
            RaiseEvent FileFetchedComplete(Me.oScanImage)
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
