Imports System.IO
Imports Amib.Threading

Public Class cFileImport
    Implements IDisposable

#Region "Properties"

    ''' <summary>
    ''' Marks, if the file buffer is currently fetched!
    ''' </summary>
    Private FileBufferIsFetching As Boolean = False

    ''' <summary>
    ''' Stores a local version of the full file-buffer.
    ''' </summary>
    Private _FileBuffer_Full As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' The File-Buffer created by the last run of the FileBufferFetcher.
    ''' </summary>
    Public ReadOnly Property FileBuffer_Full As Dictionary(Of String, cFileObject)
        Get
            Return Me._FileBuffer_Full
        End Get
    End Property

    ''' <summary>
    ''' Stores a local version of the filtered file-buffer.
    ''' </summary>
    Private _FileBuffer_Filtered As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Returns the file-buffer, filtered by the given names.
    ''' </summary>
    Public ReadOnly Property FileBuffer_Filtered() As Dictionary(Of String, cFileObject)
        Get
            Return Me._FileBuffer_Filtered
        End Get
    End Property

    ''' <summary>
    ''' Fill this list with names of files that
    ''' should be included in the filtered filebuffer list.
    ''' </summary>
    Public Property FileNameFilterToInclude As New List(Of String)

#End Region

#Region "File import exceptions"

    ''' <summary>
    ''' Checks, if the file matches an exception for the import.
    ''' This is the case for all SpectraFox-internal files.
    ''' If True, do not handle this file.
    ''' </summary>
    Public Shared Function IsFileImportException(ByRef FI As FileInfo) As Boolean

        Dim FileEndings As String() = {cFileImportSpectraFoxSFX.FileExtension,
                                       My.Resources.rFitting.FitModelExport_FileExtension_MultipleData,
                                       My.Resources.rFitting.FitModelExport_FileExtension_SingleData,
                                       ".sfc"}

        If FileEndings.Contains(FI.Extension) Then
            Return True
        End If

        Return False
    End Function

#End Region

#Region "File Buffer Creation - Smart Thread Pool"

    '' Smart Thread Pool
    'Private STP As SmartThreadPool

    '' Save for reporting from the STP the current backgroundworker
    'Private CurrentBackgroundWorker As System.ComponentModel.BackgroundWorker

    '' Save FileMax for Thread Pool
    'Private STPFetchMax As Integer

    ' ''' <summary>
    ' ''' Reads all Contents from the selected Path and returns a list with recognized Files.
    ' ''' </summary>
    'Public Function CreateFileBufferSTP(ByVal Path As String,
    '                                    Optional ByRef BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing,
    '                                    Optional ByVal OnlyIncludeFileNames As List(Of String) = Nothing) As Dictionary(Of String, cFileObject)

    '    Me.STP = New SmartThreadPool

    '    ' Return-Dictionary
    '    Me._FileBuffer = New Dictionary(Of String, cFileObject)

    '    ' make a reference to a directory
    '    Dim oDirectoryInfo As New DirectoryInfo(Path)
    '    Dim fiFileList As FileInfo() = oDirectoryInfo.GetFiles()

    '    ' If the job is done by a background-worker, then report the progress
    '    ' and check, if the User has requested a cancellation of the Thread.
    '    If Not BackgroundWorker Is Nothing Then
    '        CurrentBackgroundWorker = BackgroundWorker
    '        CurrentBackgroundWorker.ReportProgress(0, String.Empty)
    '    End If

    '    ' list the names of all files in the specified directory
    '    Me.STPFetchMax = fiFileList.Length
    '    For Each oFile As FileInfo In fiFileList

    '        ' If filtering requested, only show the filtered files.
    '        If Not OnlyIncludeFileNames Is Nothing Then
    '            If Not OnlyIncludeFileNames.Contains(oFile.Name) Then
    '                Continue For
    '            End If
    '        End If

    '        STP.QueueWorkItem(New WorkItemCallback(AddressOf Me.STPGetFileObject), oFile)
    '    Next

    '    ' Wait until all fetch processes were successful.
    '    STP.WaitForIdle()

    '    ' Shutdown the Thread-Pool
    '    STP.Shutdown()

    '    ' Remote the Smart-Thread-Pool
    '    Me.STP.Dispose()
    '    Me.STP = Nothing

    '    Return Me._FileBuffer
    'End Function

    ' ''' <summary>
    ' ''' Smart Thread Pool assisted File-Object Fetching
    ' ''' </summary>
    'Private Function STPGetFileObject(State As Object) As Object
    '    ' If the Job is done by a Background-Worker, then report the Progress
    '    ' and check, if the User has requested a cancellation of the Thread.
    '    If Not CurrentBackgroundWorker Is Nothing Then
    '        Dim CountLeft As Integer = 100 - CInt(Me.STP.WaitingCallbacks / Me.STPFetchMax * 100)
    '        CurrentBackgroundWorker.ReportProgress(CountLeft, "(" & Me.STPFetchMax - Me.STP.WaitingCallbacks & "|" & Me.STPFetchMax & ") " & My.Resources.FileImport_ScanningFiles.Replace("%", CType(State, FileInfo).Name))
    '    End If

    '    ' Scan File and get all Informations about File-Type and File-Path
    '    Dim oFileObject As cFileObject = cFileImport.GetFileObject(CType(State, FileInfo))
    '    oFileObject.LastFileChange = CType(State, FileInfo).LastWriteTime

    '    SyncLock Me._FileBuffer
    '        If oFileObject.FileType <> cFileObject.FileTypes.UNIDENTIFIED Then Me._FileBuffer.Add(oFileObject.FullName, oFileObject)
    '    End SyncLock

    '    Return Nothing
    'End Function

    ' ''' <summary>
    ' ''' Cancels a running multiple thread file buffer creation.
    ' ''' </summary>
    'Public Sub CancelRunningSTPFileBufferCreation()
    '    If Not Me.STP Is Nothing Then
    '        Me.STP.Cancel()
    '    End If
    'End Sub
#End Region

#Region "File Buffer creation - BackgroundWorker"

    ''' <summary>
    ''' Reads all contents from the selected path and returns a list with recognized Files.
    ''' </summary>
    Public Function CreateFileBuffer(ByVal Path As String,
                                     Optional ByRef BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing) As Dictionary(Of String, cFileObject)
        Me.FileBufferIsFetching = True

        ' Return-Dictionary
        Dim diFileList As Dictionary(Of String, cFileObject)

        ' Use - if existing - the current file-buffer,
        ' or - if not - create a new empty buffer
        If Me._FileBuffer_Full Is Nothing Then Me._FileBuffer_Full = New Dictionary(Of String, cFileObject)
        diFileList = Me._FileBuffer_Full

        ' make a reference to a directory, and fetch all files and folders
        Dim oDirectoryInfo As New DirectoryInfo(Path)
        Dim fiFileList As FileInfo() = oDirectoryInfo.GetFiles()
        Dim diDirectoryList As DirectoryInfo() = oDirectoryInfo.GetDirectories()

        ' Create a list that allows to ignore other files after a successfull file fetch.
        ' This is necessary for file-formats that consist out of multiple files.
        Dim lFilesToIgnoreDuringImport As New List(Of String)

        ' Create a list that caches all parameter files,
        ' that are valid for many subfiles, and thus shall not be disposed!
        Dim lParameterFileCache As New List(Of iFileImport_ParameterFileToBeImportedOnce)

        ' Get all available import filters:
        Dim ImportRoutines_SpectroscopyTables As List(Of iFileImport_SpectroscopyTable) = cFileImport.GetAllImportRoutines_SpectroscopyTable
        Dim ImportRoutines_ScanImages As List(Of iFileImport_ScanImage) = cFileImport.GetAllImportRoutines_ScanImage

        ' Temporary variables
        Dim i As Integer = 1
        Dim iMax As Integer = fiFileList.Length
        Dim ReaderBuffer As String = ""

        ' Check the filter-list, and throw out all folders!
        If Me.FileNameFilterToInclude Is Nothing Then Me.FileNameFilterToInclude = New List(Of String)
        For Each Folder As DirectoryInfo In diDirectoryList
            FileNameFilterToInclude.Remove(Folder.Name)
        Next

        '#################################################
        '
        ' First of all, check, if files have vanished,
        ' that are still present in the current file-buffer.
        ' If so, remove these files from the file-buffer!
        '
        Dim lVanishedFileNames As New List(Of String)
        For Each FileObjectKV As KeyValuePair(Of String, cFileObject) In diFileList
            Dim bFileFound As Boolean = False
            For Each oFile As FileInfo In fiFileList
                If oFile.Name = FileObjectKV.Value.FileNameWithoutPath Then
                    bFileFound = True
                    Exit For
                End If
            Next

            ' File not found? Then remove it from the cache!
            If Not bFileFound Then
                lVanishedFileNames.Add(FileObjectKV.Key)
            End If
        Next

        ' Remove the vanished file names
        For Each VanishedName As String In lVanishedFileNames
            diFileList.Remove(VanishedName)
        Next

        ' #####################################
        '
        ' Loop through all files in the folder
        ' to import all the necessary files!
        '
        For Each oFile As FileInfo In fiFileList

            '######### Progress Reporting ##########
            ' If the Job is done by a Background-Worker, then report the Progress
            ' and check, if the User has requested a cancellation of the Thread.
            If Not BackgroundWorker Is Nothing Then
                BackgroundWorker.ReportProgress(Convert.ToInt32(i / (iMax + 1) * 100),
                                                My.Resources.rFileImport.FileImport_ScanningFiles.Replace("%f", oFile.Name) _
                                                                                                 .Replace("%i", i.ToString) _
                                                                                                 .Replace("%m", iMax.ToString))
                If BackgroundWorker.CancellationPending Then Exit For
            End If
            '#######################################


            '############## FILE FILTER SECTION ###############
            If IsFileImportException(oFile) Then Continue For
            If lFilesToIgnoreDuringImport.Contains(oFile.FullName) Then Continue For

            ' Filter here for the selected names, because people use filters
            ' to save time, and to not load all files in the folder.
            ' Other files can be loaded in a different run, if necessary.
            If FileNameFilterToInclude.Count > 0 Then
                If Not FileNameFilterToInclude.Contains(oFile.Name) Then Continue For
            End If

            ' Now check, if we have already fetched these files in the buffer,
            ' and if the files have changed. If not, we can also jump over these files.
            If diFileList.ContainsKey(oFile.FullName) Then
                ' Ignore differences smaller than seconds!
                If diFileList(oFile.FullName).LastFileChange.Truncate(TimeSpan.TicksPerSecond) = oFile.LastWriteTime.Truncate(TimeSpan.TicksPerSecond) Then
                    Continue For
                End If
            End If


            '##################################################


            '############## FILE IMPORT SECTION ###############

            ' Create the file-object
            Dim oFileObject As cFileObject = cFileObject.GetFileObjectFromPath(oFile, ReaderBuffer, ImportRoutines_SpectroscopyTables, ImportRoutines_ScanImages)
            If Not oFileObject Is Nothing Then

                ' Fetch the headers of the files.
                Select Case oFileObject.FileType
                    Case cFileObject.FileTypes.SpectroscopyTable
                        Dim oSpectroscopyTable As cSpectroscopyTable = Nothing
                        cFileImport.GetSpectroscopyFile(oFileObject, oSpectroscopyTable, True, lFilesToIgnoreDuringImport, lParameterFileCache)
                    Case cFileObject.FileTypes.ScanImage
                        Dim oScanImage As cScanImage = Nothing
                        cFileImport.GetScanImageFile(oFileObject, oScanImage, True, lFilesToIgnoreDuringImport, lParameterFileCache)
                End Select

                ' Add the file-object to the output list, if we do not have to overwrite it.
                If diFileList.ContainsKey(oFileObject.FullFileNameInclPath) Then
                    diFileList(oFileObject.FullFileNameInclPath) = oFileObject
                Else
                    diFileList.Add(oFileObject.FullFileNameInclPath, oFileObject)
                End If
            End If

            '#################################################

            i += 1
        Next

        Me._FileBuffer_Full = diFileList
        Me.CreateFilteredListFromFullList()

        Me.FileBufferIsFetching = False

        Return diFileList
    End Function

    ''' <summary>
    ''' Takes the full list, and creates a filtered list by the current set of applied name filters.
    ''' </summary>
    Protected Sub CreateFilteredListFromFullList()

        ' Just filter, if the filter contains names
        If Me.FileNameFilterToInclude Is Nothing Then
            Me._FileBuffer_Filtered = Me._FileBuffer_Full
            Return
        End If
        If Me.FileNameFilterToInclude.Count <= 0 Then
            Me._FileBuffer_Filtered = Me._FileBuffer_Full
            Return
        End If

        ' Clear the old list
        Me._FileBuffer_Filtered.Clear()

        ' Add all objects to the new list.
        For Each FBKV As KeyValuePair(Of String, cFileObject) In Me._FileBuffer_Full

            ' If the filename is on the filter-list, add it to the filtered list as well.
            If Me.FileNameFilterToInclude.Contains(FBKV.Value.FileNameWithoutPath) Then
                Me._FileBuffer_Filtered.Add(FBKV.Key, FBKV.Value)
            End If

        Next

    End Sub

    ''' <summary>
    ''' Returns the current file buffer filteres by the selected File-Type
    ''' </summary>
    Public Function CurrentFileBufferFilteredByType(ByVal FileType As cFileObject.FileTypes) As Dictionary(Of String, cFileObject)
        Dim ReturnDic As New Dictionary(Of String, cFileObject)
        For Each FKV As KeyValuePair(Of String, cFileObject) In Me._FileBuffer_Full
            If FKV.Value.FileType = FileType Then
                ReturnDic.Add(FKV.Key, FKV.Value)
            End If
        Next
        Return ReturnDic
    End Function

#End Region

#Region "File Buffer Fetcher - Store/get the fetched buffer in/from a cache file."

    ''' <summary>
    ''' Write the file-buffer to a stream.
    ''' </summary>
    Public Function WriteFileBufferToStream(ByRef S As MemoryStream,
                                            Optional ByRef BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing) As Boolean

        If Me.FileBufferIsFetching Then Return False

        Try

            ' Go to the beginning of the stream.
            S.Close()
            S.Dispose()
            S = New MemoryStream
            S.Seek(0, IO.SeekOrigin.Begin)

            ' Select the File-Encoding
            Dim enc As New System.Text.UnicodeEncoding

            ' Create the XmlTextWriter object
            Dim XMLobj As New Xml.XmlTextWriter(S, enc)

            With XMLobj
                ' Set the proper formatting
                .Formatting = Xml.Formatting.Indented
                .Indentation = 4

                ' create the document header
                .WriteStartDocument()
                .WriteStartElement("root")

                ' Begin with SpectraFox program properties
                .WriteStartElement("SpectraFox")
                .WriteAttributeString("Version", cProgrammDeployment.GetProgramVersionString)
                .WriteEndElement()

                ' Write properties of the cache file
                .WriteStartElement("CacheInformation")
                .WriteAttributeString("Count", Me._FileBuffer_Full.Count.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .WriteEndElement()

                ' Begin the section of the file-object entries
                .WriteStartElement("FileObjects")
                Dim i As Integer = 0
                For Each FileObject As cFileObject In Me._FileBuffer_Full.Values

                    ' Report progress
                    If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(CInt(i / Me._FileBuffer_Full.Count), "Writing objects to cache file ...")
                    i += 1

                    ' Write the information for each file.
                    FileObject.WriteFileObjectToSingleXMLCacheLine(XMLobj)

                Next
                .WriteEndElement()

                ' End <root>
                .WriteEndElement()

                ' Close the document
                .WriteEndDocument()

                ' Close the XML-Document
                .Flush()
                '.Close() ' Document
                '.Dispose()
            End With
            XMLobj = Nothing

        Catch ex As Exception
            Debug.WriteLine("Error writing cache-file to stream: " & ex.Message)
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Saves the file-buffer with all file-informations to a cache-file,
    ''' that can get loaded instead of rebuilding the whole file-buffer from scratch.
    ''' 
    ''' Returns, if writing has been successfull.
    ''' </summary>
    Public Function WriteFileBufferAsFile(ByVal PathToFileInclFullFile As String,
                                          Optional ByRef BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing) As Boolean

        If Me.FileBufferIsFetching Then Return False

        Try

            ' Report progress
            If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(5, "Opening file for writing ...")

            Dim MS As New MemoryStream

            Dim ExportSuccessFull As Boolean = False
            Try
                ' open filestream
                ExportSuccessFull = Me.WriteFileBufferToStream(MS, BackgroundWorker)

                ' Write the stream to the file.
                Me.WriteFileBufferStreamAsFile(MS, PathToFileInclFullFile, BackgroundWorker)

                ' Report progress
                If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(98, "Closing file, and releasing ressources ...")
            Catch ex As Exception
            Finally
                ' Close the stream
                MS.Close()
                MS.Dispose()
                MS = Nothing
            End Try

        Catch ex As Exception
            Debug.WriteLine("Error writing cache-file: " & ex.Message)
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Saves the file-buffer with all file-informations to a cache-file,
    ''' that can get loaded instead of rebuilding the whole file-buffer from scratch.
    ''' 
    ''' Returns, if writing has been successfull.
    ''' </summary>
    Public Function WriteFileBufferStreamAsFile(ByRef FileBufferStream As MemoryStream,
                                                ByVal PathToFileInclFullFile As String,
                                                Optional ByRef BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing) As Boolean

        Dim TargetFileName As String = PathToFileInclFullFile
        Dim TMPFileName As String = PathToFileInclFullFile & ".~TMP~"
        Dim BackupFileName As String = PathToFileInclFullFile & ".~bak~"

        Try

            ' Report progress
            If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(5, "Opening file for writing ...")

            ' Create a filestream.
            Dim FS As New FileStream(TMPFileName, FileMode.Create)

            Dim ExportSuccessFull As Boolean = False
            Try
                ' Write the stream to the file.
                FileBufferStream.WriteTo(FS)
                ExportSuccessFull = True

                ' Report progress
                If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(98, "Closing file, and releasing ressources ...")
            Catch ex As Exception
            Finally
                ' Close the stream
                FS.Close()
                FS.Dispose()
                FS = Nothing
            End Try

            If ExportSuccessFull Then
                ' If everything was ok so far, so if we are at this point,
                ' move the temporary file to the real target-file-name,
                ' and overwrite the copy.
                If System.IO.File.Exists(BackupFileName) Then System.IO.File.Delete(BackupFileName)
                If System.IO.File.Exists(TargetFileName) Then System.IO.File.Move(TargetFileName, BackupFileName)
                System.IO.File.Move(TMPFileName, TargetFileName)
                If System.IO.File.Exists(BackupFileName) Then System.IO.File.Delete(BackupFileName)
            End If

        Catch ex As Exception
            Debug.WriteLine("Error writing cache-file: " & ex.Message)

            ' Move back the backup, if it existed!
            Try
                If System.IO.File.Exists(BackupFileName) Then System.IO.File.Move(BackupFileName, TargetFileName)
            Catch ex2 As Exception
                Debug.WriteLine("Error moving backup cache-file back in place: " & ex2.Message)
            End Try

            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Gets the file-buffer from a stream.
    ''' </summary>
    Public Function GetFileBufferFromStream(ByRef S As MemoryStream,
                                            ByVal BasePath As String,
                                            Optional ByRef BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing) As Dictionary(Of String, cFileObject)

        ' Return-Dictionary
        Dim diFileList As New Dictionary(Of String, cFileObject)

        Try

            ' Go to the beginning of the stream.
            S.Seek(0, IO.SeekOrigin.Begin)

            ' Report progress
            If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(5, "Opening cache file ...")

            ' Open the XML-reader object for the specified file
            Dim XMLReader As New Xml.XmlTextReader(S)

            ' Save the Spectrafox-version that created the file to 
            ' check against old files and new features.
            Dim SpectraFoxVersionOfFile As String = ""

            ' Number of files expected in the file-cache.
            Dim CacheCount As Integer = -1

            ' Now read the XML-file, and import the settings.
            With XMLReader
                ' read up to the end of the file
                Do While .Read
                    ' Check for the type of data
                    Select Case .NodeType
                        Case Xml.XmlNodeType.Element
                            ' An element comes: this is what we are looking for!
                            '####################################################
                            Select Case .Name
                                Case "SpectraFox"
                                    ' get and check the properties:
                                    '###############################
                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Version"
                                                    SpectraFoxVersionOfFile = .Value
                                            End Select
                                        End While
                                    End If

                                Case "CacheInformation"
                                    ' get and check the properties:
                                    '###############################
                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Count"
                                                    CacheCount = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            End Select
                                        End While
                                    End If

                                Case "FileObject"

                                    Dim NewFileObject As cFileObject = cFileObject.GetFileObjectFromSingleXMLCacheLine(.ReadSubtree)

                                    If Not NewFileObject Is Nothing Then
                                        ' Set additional settings
                                        With NewFileObject
                                            ' Use the current path to the file, since this may change on different computers.
                                            .FullFileNameInclPath = BasePath & IO.Path.DirectorySeparatorChar & .FileName
                                        End With

                                        ' Add the current path + the new file-object.
                                        diFileList.Add(NewFileObject.FullFileNameInclPath, NewFileObject)

                                        ' Report progress
                                        If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(CInt(diFileList.Count / CacheCount), "Reading cached file objects ...")

                                    End If

                            End Select

                    End Select
                Loop

                ' Close the XML-Reader
                .Close()
                .Dispose()
            End With
            XMLReader = Nothing
        Catch ex As Exception
            Debug.WriteLine("Cache file import failed from stream: " & ex.Message)
        End Try

        ' Set the current filebuffer.
        Me._FileBuffer_Full = diFileList
        Me.CreateFilteredListFromFullList()

        Return Me._FileBuffer_Full
    End Function

    ''' <summary>
    ''' Gets the file-buffer from a cache file.
    ''' </summary>
    Public Function GetFileBufferFromFile(ByVal FullFilenameIncludingPath As String,
                                          Optional ByRef BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing) As Dictionary(Of String, cFileObject)

        ' Construct the full file path
        Dim PathToFile As String = System.IO.Path.GetDirectoryName(FullFilenameIncludingPath)
        Dim FileName As String = System.IO.Path.GetFileName(FullFilenameIncludingPath)

        Try
            ' Report progress
            If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(5, "Opening cache file ...")

            ' Create a filestream.
            Dim FS As New FileStream(FullFilenameIncludingPath, FileMode.Open)
            Dim MS As New MemoryStream

            Try
                ' Read the file
                FS.CopyTo(MS)
            Catch ex As Exception
                Debug.WriteLine("Cache file import failed in file " & FileName & ", due to exception: " & vbNewLine & ex.Message)
            Finally
                ' Close the stream
                FS.Close()
                FS.Dispose()
                FS = Nothing
            End Try

            Try
                ' open filestream
                Me.GetFileBufferFromStream(MS, PathToFile, BackgroundWorker)

                ' Report progress
                If Not BackgroundWorker Is Nothing Then BackgroundWorker.ReportProgress(98, "Closing file, and releasing resources ...")
            Catch ex As Exception
                Debug.WriteLine("Cache file import failed in file " & FileName & ", due to exception: " & vbNewLine & ex.Message)
            Finally
                ' Close the stream
                MS.Close()
                MS.Dispose()
                MS = Nothing
            End Try

        Catch ex As Exception
            Debug.WriteLine("Cache file import failed in file " & FileName & ", due to exception: " & vbNewLine & ex.Message)
        End Try

        Return Me._FileBuffer_Full
    End Function

#End Region

#Region "Fetcher functions to load the full-data for a file-object."

#Region "Multiple-File fetcher Class"

    ''' <summary>
    ''' Class that handles all threaded loading of multiple scan or spectroscopy files.
    ''' It finally just calls the multiple-file fetched handler.
    ''' </summary>
    Public Class AsyncMultipleFileLoader
        Implements iSingleSpectroscopyTableLoaded
        Implements iSingleScanImageLoaded

        ''' <summary>
        ''' Counter for all files that are fetched
        ''' </summary>
        Private iFetchedFilesCounter As Integer = 0

        ''' <summary>
        ''' Threadpool
        ''' </summary>
        Private _ThreadPool As cSmartThreadPoolExtended

        ''' <summary>
        ''' Fetch only the file-header?
        ''' </summary>
        Private _FetchOnlyHeader As Boolean

        ''' <summary>
        ''' List to handle
        ''' </summary>
        Private _FileObjectList As List(Of cFileObject)

#Region "Spectroscopy-Table section"

        ''' <summary>
        ''' Sets the initial variables, and starts the fetch procedure.
        ''' </summary>
        Public Sub New(ByRef FileObjectList As List(Of cFileObject),
                       ByVal Callback As iMultipleSpectroscopyTablesLoaded,
                       Optional ByVal FetchOnlyFileHeader As Boolean = False,
                       Optional ByRef ThreadPool As cSmartThreadPoolExtended = Nothing)
            Me.CallBackFunctionSpec = Callback
            Me._ThreadPool = ThreadPool
            Me._FileObjectList = FileObjectList
            Me._FetchOnlyHeader = FetchOnlyFileHeader

            ' Start the fetch
            cFileImport.GetSpectroscopyFile_Async(_FileObjectList(iFetchedFilesCounter),
                                                  Me, Me._FetchOnlyHeader, Me._ThreadPool)
            iFetchedFilesCounter += 1
        End Sub

        ''' <summary>
        ''' Callback called on all files loaded
        ''' </summary>
        Public CallBackFunctionSpec As iMultipleSpectroscopyTablesLoaded

        ''' <summary>
        ''' Store the fetched files.
        ''' </summary>
        Private OutputListSpec As New List(Of cSpectroscopyTable)

        ''' <summary>
        ''' Fetch the next file
        ''' </summary>
        Public Sub SpectroscopyTableLoaded(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded

            ' Store the loaded spectroscopy-table
            Me.OutputListSpec.Add(SpectroscopyTable)

            If iFetchedFilesCounter < _FileObjectList.Count Then
                ' Fetch next file
                '#################
                cFileImport.GetSpectroscopyFile_Async(_FileObjectList(iFetchedFilesCounter),
                                                      Me, Me._FetchOnlyHeader, Me._ThreadPool)
                iFetchedFilesCounter += 1
            Else
                ' last file fetched, so call the callback
                '#########################################
                CallBackFunctionSpec.AllSpectroscopyTablesLoaded(Me.OutputListSpec)
            End If
        End Sub

#End Region

#Region "Scan-Image section"
        ''' <summary>
        ''' Sets the initial variables, and starts the fetch procedure.
        ''' </summary>
        Public Sub New(ByRef FileObjectList As List(Of cFileObject),
                       ByVal Callback As iMultipleScanImagesLoaded,
                       Optional ByVal FetchOnlyFileHeader As Boolean = False,
                       Optional ByRef ThreadPool As cSmartThreadPoolExtended = Nothing)
            Me.CallBackFunctionScan = Callback
            Me._ThreadPool = ThreadPool
            Me._FileObjectList = FileObjectList
            Me._FetchOnlyHeader = FetchOnlyFileHeader

            ' Start the fetch
            cFileImport.GetScanImageFile_Async(_FileObjectList(iFetchedFilesCounter),
                                               Me, Me._FetchOnlyHeader, Me._ThreadPool)
            iFetchedFilesCounter += 1
        End Sub

        ''' <summary>
        ''' Callback called on all files loaded
        ''' </summary>
        Public CallBackFunctionScan As iMultipleScanImagesLoaded

        ''' <summary>
        ''' Store the fetched files.
        ''' </summary>
        Private OutputListScan As New List(Of cScanImage)

        ''' <summary>
        ''' Fetch the next file
        ''' </summary>
        Public Sub ScanImageLoaded(ByRef ScanImage As cScanImage) Implements iSingleScanImageLoaded.ScanImageLoaded

            ' Store the loaded spectroscopy-table
            Me.OutputListScan.Add(ScanImage)

            If iFetchedFilesCounter < _FileObjectList.Count Then
                ' Fetch next file
                '#################
                cFileImport.GetScanImageFile_Async(_FileObjectList(iFetchedFilesCounter),
                                                   Me, Me._FetchOnlyHeader, Me._ThreadPool)
                iFetchedFilesCounter += 1
            Else
                ' last file fetched, so call the callback
                '#########################################
                CallBackFunctionScan.AllScanImagesLoaded(Me.OutputListScan)
            End If
        End Sub

#End Region

    End Class

#End Region

#Region "Spectroscopy-File"
    ''' <summary>
    ''' Reads the selected Spectroscopy File and returns a SpectroscopyTable object
    ''' TargetSpectroscopyTable will be overridden!
    ''' </summary>
    Public Shared Function GetSpectroscopyFile(ByRef FileObject As cFileObject,
                                               ByRef TargetSpectroscopyTable As cSpectroscopyTable,
                                               Optional ByVal FetchOnlyFileHeader As Boolean = False,
                                               Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                                               Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As Boolean

        ' Check, if the file still exists
        If Not System.IO.File.Exists(FileObject.FullFileNameInclPath) Then Return False

        ' Check, if the file is a Spectroscopy Table
        If FileObject.FileType <> cFileObject.FileTypes.SpectroscopyTable Then Return False

        Try
            ' Start Import depending on FileType.
            ' Get the import routine for this.
            Dim ImportRoutine As iFileImport_SpectroscopyTable = cFileImport.GetImportRoutineFromType_SpectroscopyTable(FileObject.ImportRoutine)
            If ImportRoutine Is Nothing Then Return False

            '' Get again the FULL FileObject
            FileObject = cFileObject.GetFileObjectFromPath(New FileInfo(FileObject.FullFileNameInclPath), , {ImportRoutine}.ToList,)

            ' Start the import:
            TargetSpectroscopyTable = ImportRoutine.ImportSpectroscopyTable(FileObject.FullFileNameInclPath, FetchOnlyFileHeader, , FilesToIgnoreAfterThisImport, ParameterFilesImportedOnce)
            FileObject.SpectroscopyTable = TargetSpectroscopyTable

            '' Get again the FULL FileObject
            FileObject = cFileObject.GetFileObjectFromPath(FileObject)

            ' Save the Spectrum-Location in the FileObject,
            ' since we can use it for faster identification of SpectroscopyFiles
            ' in a specific scan-region
            FileObject.RecordLocation_X = TargetSpectroscopyTable.Location_X
            FileObject.RecordLocation_Y = TargetSpectroscopyTable.Location_Y

            ' Save the Record-Date
            FileObject.RecordDate = TargetSpectroscopyTable.RecordDate

            ' Set properties of the BaseFileObject
            FileObject.MeasurementDimensions = TargetSpectroscopyTable.MeasurementPoints.ToString("N0") & My.Resources.rDataBrowser.Word_DataPoints
            FileObject._SourceFileComment = TargetSpectroscopyTable.Comment

            ' Get the crop information and apply it to all data columns.
            TargetSpectroscopyTable.SetCropInformation(FileObject.SpectroscopyTable_CropInformation)

            ' Set Base-FileObject-Reference
            TargetSpectroscopyTable.BaseFileObject = FileObject

            Return True
        Catch ex As OutOfMemoryException
            ' If we run out of memory during the fetch,
            ' try to free up some space by running the garbage collector manually.
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Return GetSpectroscopyFile(FileObject, TargetSpectroscopyTable)
        Catch ex As Exception
            ' File-Import failed.
            MessageBox.Show(My.Resources.rFileImport.FileImportError.Replace("%fn", FileObject.FullFileNameInclPath) _
                                                                    .Replace("%e", ex.Message),
                            My.Resources.rFileImport.FileImportError_title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Reads the selected Spectroscopy File and returns a SpectroscopyTable object
    ''' </summary>
    Public Shared Sub GetSpectroscopyFile_Async(ByRef FileObject As cFileObject,
                                                ByVal Callback As iSingleSpectroscopyTableLoaded,
                                                Optional ByVal FetchOnlyFileHeader As Boolean = False,
                                                Optional ByRef ThreadPool As cSmartThreadPoolExtended = Nothing)
        ' if no thread-pool is handed over, 
        ' create a separate thread and start the fetch
        ' else, queue the work.
        Dim WorkerState As Object() = New Object() {FileObject, Callback, FetchOnlyFileHeader}
        If ThreadPool Is Nothing Then
            Dim T As New Threading.Thread(AddressOf AsyncSpectroscopyFileGetter)
            T.Start(WorkerState)
        Else
            ThreadPool.QueueWorkItem(New WorkItemCallback(AddressOf AsyncSpectroscopyFileGetter), WorkerState)
        End If
    End Sub

    ''' <summary>
    ''' Async spectroscopy file getter.
    ''' </summary>
    Private Shared Function AsyncSpectroscopyFileGetter(ParameterArray As Object) As Object
        Dim Params As Object() = DirectCast(ParameterArray, Object())
        Dim FileObject As cFileObject = DirectCast(Params(0), cFileObject)
        Dim Callback As iSingleSpectroscopyTableLoaded = DirectCast(Params(1), iSingleSpectroscopyTableLoaded)
        Dim FetchOnlyFileHeader As Boolean = DirectCast(Params(2), Boolean)

        Dim oSpectroscopyTable As cSpectroscopyTable = Nothing
        GetSpectroscopyFile(FileObject, oSpectroscopyTable, FetchOnlyFileHeader)

        If Not oSpectroscopyTable Is Nothing Then
            Callback.SpectroscopyTableLoaded(oSpectroscopyTable)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Reads the selected Spectroscopy Files and returns a list of SpectroscopyTable objects
    ''' </summary>
    Public Shared Function GetSpectroscopyFiles(ByRef FileObjectList As List(Of cFileObject),
                                                ByRef TargetSpectroscopyTables As List(Of cSpectroscopyTable),
                                                Optional ByVal FetchOnlyFileHeader As Boolean = False) As Boolean
        ' Create new list, if target-list is empty
        If TargetSpectroscopyTables Is Nothing Then
            TargetSpectroscopyTables = New List(Of cSpectroscopyTable)
        End If

        Dim CurrentSpectroscopyTable As cSpectroscopyTable = Nothing
        For i As Integer = 0 To FileObjectList.Count - 1 Step 1
            ' create new 
            CurrentSpectroscopyTable = Nothing
            GetSpectroscopyFile(FileObjectList(i), CurrentSpectroscopyTable, FetchOnlyFileHeader)
            If TargetSpectroscopyTables.Count = i Then
                TargetSpectroscopyTables.Add(CurrentSpectroscopyTable)
            Else
                TargetSpectroscopyTables(i) = CurrentSpectroscopyTable
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Reads the selected Spectroscopy File async and calls the callback afterwards.
    ''' </summary>
    Public Shared Function GetSpectroscopyFiles_Async(ByRef FileObjectList As List(Of cFileObject),
                                                      ByVal Callback As iMultipleSpectroscopyTablesLoaded,
                                                      Optional ByVal FetchOnlyFileHeader As Boolean = False,
                                                      Optional ByRef ThreadPool As cSmartThreadPoolExtended = Nothing) As Boolean

        ' Create and launch the sub-class for multiple-file fetching
        Dim AsyncFileLoader As New AsyncMultipleFileLoader(FileObjectList,
                                                           Callback,
                                                           FetchOnlyFileHeader,
                                                           ThreadPool)
        Return True
    End Function

#End Region

#Region "Scan-Image File"

    ''' <summary>
    ''' Reads the selected scan image and returns a ScanImage object
    ''' </summary>
    Public Shared Function GetScanImageFile(ByRef FileObject As cFileObject,
                                            ByRef TargetScanImage As cScanImage,
                                            Optional ByVal FetchOnlyFileHeader As Boolean = False,
                                            Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                                            Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As Boolean

        ' Check, if the File still exists
        If Not System.IO.File.Exists(FileObject.FullFileNameInclPath) Then Return False

        ' Check, if the file is a Scan Image
        If FileObject.FileType <> cFileObject.FileTypes.ScanImage Then Return False

        Try
            ' Start Import depending on FileType.
            ' Get the import routine for this.
            Dim ImportRoutine As iFileImport_ScanImage = cFileImport.GetImportRoutineFromType_ScanImage(FileObject.ImportRoutine)
            If ImportRoutine Is Nothing Then Return False

            '' Get again the FULL FileObject
            FileObject = cFileObject.GetFileObjectFromPath(New FileInfo(FileObject.FullFileNameInclPath), , , {ImportRoutine}.ToList)

            ' Start the import:
            TargetScanImage = ImportRoutine.ImportScanImage(FileObject.FullFileNameInclPath, FetchOnlyFileHeader, , FilesToIgnoreAfterThisImport, ParameterFilesImportedOnce)
            FileObject.ScanImage = TargetScanImage

            '' Get again the FULL FileObject
            FileObject = cFileObject.GetFileObjectFromPath(FileObject)

            ' Save the ScanImage-Location in the FileObject,
            ' since we can use it for faster identification of SpectroscopyFiles
            ' in a specific scan-region
            FileObject.RecordLocation_X = TargetScanImage.ScanOffset_X
            FileObject.RecordLocation_Y = TargetScanImage.ScanOffset_Y
            FileObject.ScanImageRange_X = TargetScanImage.ScanRange_X
            FileObject.ScanImageRange_Y = TargetScanImage.ScanRange_Y

            ' Save the Record-Date
            FileObject.RecordDate = TargetScanImage.RecordDate

            ' Set properties of the BaseFileObject
            FileObject.MeasurementDimensions = My.Resources.rFileImport.ScanImage_MeasurementDimensions _
                                                        .Replace("%xp", cUnits.GetFormatedValueString(TargetScanImage.ScanRange_X, 2)) _
                                                        .Replace("%yp", cUnits.GetFormatedValueString(TargetScanImage.ScanRange_Y, 2)) _
                                                        .Replace("%spc", cUnits.GetFormatedValueString(TargetScanImage.ZControllerSetpoint, 1) & TargetScanImage.ZControllerSetpointUnit) _
                                                        .Replace("%spb", cUnits.GetFormatedValueString(TargetScanImage.Bias, 1))
            FileObject._SourceFileComment = TargetScanImage.Comment

            ' Set Base-FileObject-Reference
            TargetScanImage.BaseFileObject = FileObject

            Return True
        Catch ex As OutOfMemoryException
            ' If we run out of memory during the fetch,
            ' try to free up some space by running the garbage collector manually.
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Return GetScanImageFile(FileObject, TargetScanImage)
        Catch ex As Exception
            ' File-Import failed.
            MessageBox.Show(My.Resources.rFileImport.FileImportError.Replace("%fn", FileObject.FullFileNameInclPath) _
                                                                    .Replace("%e", ex.Message),
                            My.Resources.rFileImport.FileImportError_title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Reads the selected ScanImage File and returns a ScanImage object
    ''' </summary>
    Public Shared Sub GetScanImageFile_Async(ByRef FileObject As cFileObject,
                                             ByVal Callback As iSingleScanImageLoaded,
                                             Optional ByVal FetchOnlyFileHeader As Boolean = False,
                                             Optional ByRef ThreadPool As cSmartThreadPoolExtended = Nothing)
        ' if no thread-pool is handed over, 
        ' create a separate thread and start the fetch
        ' else, queue the work.
        Dim WorkerState As Object() = New Object() {FileObject, Callback, FetchOnlyFileHeader}
        If ThreadPool Is Nothing Then
            Dim T As New Threading.Thread(AddressOf AsyncScanImageFileGetter)
            T.Start(WorkerState)
        Else
            ThreadPool.QueueWorkItem(New WorkItemCallback(AddressOf AsyncScanImageFileGetter), WorkerState)
        End If
    End Sub

    ''' <summary>
    ''' Async scan image file getter.
    ''' </summary>
    Private Shared Function AsyncScanImageFileGetter(ParameterArray As Object) As Object
        Dim Params As Object() = DirectCast(ParameterArray, Object())
        Dim FileObject As cFileObject = DirectCast(Params(0), cFileObject)
        Dim Callback As iSingleScanImageLoaded = DirectCast(Params(1), iSingleScanImageLoaded)
        Dim FetchOnlyFileHeader As Boolean = DirectCast(Params(2), Boolean)

        Dim oScanImage As cScanImage = Nothing
        GetScanImageFile(FileObject, oScanImage, FetchOnlyFileHeader)

        If Not oScanImage Is Nothing Then
            Callback.ScanImageLoaded(oScanImage)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Reads the selected ScanImage Files and returns a list of ScanImage objects
    ''' </summary>
    Public Shared Function GetScanImageFiles(ByRef FileObjectList As List(Of cFileObject),
                                             ByRef TargetScanImages As List(Of cScanImage),
                                             Optional ByVal FetchOnlyFileHeader As Boolean = False) As Boolean
        ' Create new list, if target-list is empty
        If TargetScanImages Is Nothing Then
            TargetScanImages = New List(Of cScanImage)
        End If

        Dim CurrentScanImage As cScanImage = Nothing
        For i As Integer = 0 To FileObjectList.Count - 1 Step 1
            ' create new 
            GetScanImageFile(FileObjectList(i), CurrentScanImage, FetchOnlyFileHeader)
            If TargetScanImages.Count = i Then
                TargetScanImages.Add(CurrentScanImage)
            Else
                TargetScanImages(i) = CurrentScanImage
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Reads the selected ScanImage files async and calls the callback afterwards.
    ''' </summary>
    Public Shared Function GetScanImageFiles_Async(ByRef FileObjectList As List(Of cFileObject),
                                                   ByVal Callback As iMultipleScanImagesLoaded,
                                                   Optional ByVal FetchOnlyFileHeader As Boolean = False,
                                                   Optional ByRef ThreadPool As cSmartThreadPoolExtended = Nothing) As Boolean

        ' Create and launch the sub-class for multiple-file fetching
        Dim AsyncFileLoader As New AsyncMultipleFileLoader(FileObjectList,
                                                           Callback,
                                                           FetchOnlyFileHeader,
                                                           ThreadPool)
        Return True
    End Function

#End Region

#Region "Helper Functions"

    ''' <summary>
    ''' Reads as long as not Linefeed is found from the Binary-Stream.
    ''' </summary>
    ''' <param name="br">BinaryReader pointing to the Stream-Object</param>
    ''' <returns>String with the extracted line.</returns>
    Public Shared Function ReadASCIILineFromBinaryStream(ByRef br As BinaryReader,
                                                         Optional ByRef out As String = "") As String
        out = String.Empty
        Dim ch1 As Char
        ' Read Until end of Stream, or Line-Feed.
        Do Until br.BaseStream.Position = br.BaseStream.Length Or ch1 = vbLf
            ch1 = br.ReadChar()
            out &= ch1
        Loop
        Return out
    End Function

    ''' <summary>
    ''' Reads as long as not Linefeed is found from the Binary-Stream.
    ''' </summary>
    ''' <param name="br">BinaryReader pointing to the Stream-Object</param>
    ''' <returns>String with the extracted line.</returns>
    Public Shared Function ReadASCIIFromBinaryStreamUntilSign(ByRef br As BinaryReader,
                                                              ByVal EndMarker As Char,
                                                              Optional ByRef out As String = "") As String
        out = String.Empty
        Dim ch1 As Char
        If EndMarker = ch1 Then ch1 = ControlChars.Cr
        ' Read Until end of Stream, or Line-Feed.
        Do Until br.BaseStream.Position = br.BaseStream.Length Or ch1 = EndMarker
            ch1 = br.ReadChar()
            out &= ch1
        Loop
        Return out
    End Function

    ''' <summary>
    ''' Reverses the order of a given Byte-Array.
    ''' </summary>
    ''' <returns>InputBytes-Array in Reversed Order.</returns>
    Public Shared Function ReverseBytes(ByRef InputBytes As Byte()) As Byte()
        Return InputBytes.Reverse.ToArray
    End Function
#End Region

#End Region

#Region "API Discovery"
    ''' <summary>
    ''' Returns a list with all import routines implemented in the program.
    ''' </summary>
    Public Shared Function GetAllImportRoutines_SpectroscopyTable() As List(Of iFileImport_SpectroscopyTable)
        Dim APIList As New List(Of iFileImport_SpectroscopyTable)

        Try
            ' fill the list of with the interfaces found.
            With APIList
                Dim APIType = GetType(iFileImport_SpectroscopyTable)
                Dim AllAPIImplementingInterfaces As IEnumerable(Of Type) = AppDomain.CurrentDomain.GetAssemblies() _
                                                                           .SelectMany(Function(s) s.GetTypes()) _
                                                                           .Where(Function(p) APIType.IsAssignableFrom(p) And p.IsClass And Not p.IsAbstract)
                For Each ImplementingType As Type In AllAPIImplementingInterfaces
                    .Add(DirectCast(System.Activator.CreateInstance(ImplementingType), iFileImport_SpectroscopyTable))
                Next
            End With
        Catch ex As Exception
            Trace.WriteLine("#ERROR: cFileImport.GetAllImportRoutines_SpectroscopyTable: Error on loading: " & ex.Message)
        End Try

        Return APIList
    End Function

    ''' <summary>
    ''' Returns the Import-Routine, if the type compatible. Else Nothing
    ''' </summary>
    Public Shared Function GetImportRoutineFromType_SpectroscopyTable(ByRef T As Type) As iFileImport_SpectroscopyTable
        If GetType(iFileImport_SpectroscopyTable).IsAssignableFrom(T) Then
            Return DirectCast(System.Activator.CreateInstance(T), iFileImport_SpectroscopyTable)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns a list with all import routines implemented in the program.
    ''' </summary>
    Public Shared Function GetAllImportRoutines_ScanImage() As List(Of iFileImport_ScanImage)
        Dim APIList As New List(Of iFileImport_ScanImage)

        Try
            ' fill the list of with the interfaces found.
            With APIList
                Dim APIType = GetType(iFileImport_ScanImage)
                Dim AllAPIImplementingInterfaces As IEnumerable(Of Type) = AppDomain.CurrentDomain.GetAssemblies() _
                                                                       .SelectMany(Function(s) s.GetTypes()) _
                                                                       .Where(Function(p) APIType.IsAssignableFrom(p) And p.IsClass And Not p.IsAbstract)
                For Each ImplementingType As Type In AllAPIImplementingInterfaces
                    .Add(DirectCast(System.Activator.CreateInstance(ImplementingType), iFileImport_ScanImage))
                Next
            End With
        Catch ex As Exception
            Trace.WriteLine("#ERROR: cFileImport.GetAllImportRoutines_ScanImage: Error on loading: " & ex.Message)
        End Try

        Return APIList
    End Function

    ''' <summary>
    ''' Returns the Import-Routine, if the type compatible. Else Nothing
    ''' </summary>
    Public Shared Function GetImportRoutineFromType_ScanImage(ByRef T As Type) As iFileImport_ScanImage
        If GetType(iFileImport_ScanImage).IsAssignableFrom(T) Then
            Return DirectCast(System.Activator.CreateInstance(T), iFileImport_ScanImage)
        End If
        Return Nothing
    End Function

#End Region

#Region "Dispose"

    ''' <summary>
    ''' Disposes the Object.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose

        ' Clear the name filter
        If Me.FileNameFilterToInclude IsNot Nothing Then Me.FileNameFilterToInclude.Clear()
        Me.FileNameFilterToInclude = Nothing

        ' Clear the file-buffer.
        For Each FO As cFileObject In Me._FileBuffer_Full.Values
            FO.Dispose()
            FO = Nothing
        Next
        Me._FileBuffer_Filtered.Clear()
        Me._FileBuffer_Filtered = Nothing
        Me._FileBuffer_Full.Clear()
        Me._FileBuffer_Full = Nothing

    End Sub

#End Region

End Class
