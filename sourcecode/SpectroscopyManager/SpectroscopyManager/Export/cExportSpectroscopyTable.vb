Imports System.Threading

''' <summary>
''' Class for exporting Spectroscopy Tables
''' </summary>
Public Class cExportSpectroscopyTable
    Inherits cExport

#Region "Properties"
    ''' <summary>
    ''' List with the Spectroscopy-Table-Names to Export.
    ''' </summary>
    Private lSpectroscopyTableFileNames As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Callback-Function to load the SpectroscopyFiles and export them in separate Threads.
    ''' </summary>
    Private FileFetchAndExportCallback As New WaitCallback(AddressOf SpectroscopyFileFetcherAndExporter)

    Private ExportPath As String = ""

    Private ExportMethod As iExportMethod_Ascii

    Private bReady As Boolean = False
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes a list of all SpectroscopyTable-FileObjects.
    ''' </summary>
    Public Sub New(ListOfSpectroscopyFiles As Dictionary(Of String, cFileObject))
        ' Add files to local list.
        For Each SpectroscopyTableFile As KeyValuePair(Of String, cFileObject) In ListOfSpectroscopyFiles
            ' Filter out all Spectroscopy-Table-Files
            If SpectroscopyTableFile.Value.FileType = cFileObject.FileTypes.SpectroscopyTable Then
                Me.lSpectroscopyTableFileNames.Add(SpectroscopyTableFile.Key, SpectroscopyTableFile.Value)
            End If
        Next
        ' Set Statistic Counter
        Me.MaxFileExports = ListOfSpectroscopyFiles.Count
    End Sub
#End Region

#Region "Export Function"
    ''' <summary>
    ''' Starts the Export for each file in the list.
    ''' </summary>
    Public Sub StartExport(ByVal TargetPath As String, ByVal TargetExportMethod As iExportMethod_Ascii)
        ' Reset Statistic counter
        Me.CurrentFileExports = 0
        Me.MaxFileExports = Me.lSpectroscopyTableFileNames.Count

        ' Set paths and settings!
        Me.ExportPath = TargetPath
        Me.ExportMethod = TargetExportMethod

        ' Set current export path
        If Not Me.ExportPath.EndsWith(IO.Path.DirectorySeparatorChar) Then
            Me.ExportPath &= IO.Path.DirectorySeparatorChar
        End If

        ' Check for existing directory
        If Not System.IO.Directory.Exists(Me.ExportPath) Then
            Throw New Exception("Export Directory " & Me.ExportPath & " not found.")
        End If

        For Each FileObject As cFileObject In Me.lSpectroscopyTableFileNames.Values
            ' Send FileObjects to ThreadPoolQueue
            ThreadPool.QueueUserWorkItem(FileFetchAndExportCallback, FileObject)
        Next
    End Sub

    ''' <summary>
    ''' Spectroscopy File Fetcher to load the spectroscopy file and export it.
    ''' </summary>
    Private Sub SpectroscopyFileFetcherAndExporter(SpectroscopyFileObjectToLoad As Object)
        Dim FileObjectToLoad As cFileObject = CType(SpectroscopyFileObjectToLoad, cFileObject)

        ' Load the File from Disk
        Dim oSpectroscopyTable As cSpectroscopyTable = Nothing
        If Not cFileImport.GetSpectroscopyFile(FileObjectToLoad, oSpectroscopyTable) Then Return

        ' Creates the Export-String depending on the selected Format
        Dim TargetFileName As String = Me.ExportPath & oSpectroscopyTable.FileNameWithoutPathAndExtension & Me.ExportMethod.FileExtension
        Dim sExportString As String = Me.ExportMethod.GetExportOutput(oSpectroscopyTable)

        ' Opens a file-writer-stream and writes the selected File
        If Not TargetFileName = "" Then
            Dim ws As New IO.StreamWriter(TargetFileName)
            ws.Write(sExportString)
            ws.Close()
        End If

        MyBase.RaiseEventFileExportedSuccessfully(FileObjectToLoad, TargetFileName)
    End Sub

#End Region
End Class
