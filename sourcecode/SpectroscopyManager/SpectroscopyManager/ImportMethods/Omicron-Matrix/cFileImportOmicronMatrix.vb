Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportOmicronMatrix
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Different identifiers of the Omicron-files.
    ''' </summary>
    Public Const FileIdentifier As String = "ONTMATRX0101"
    Public Const DataFileIdentifier As String = "ONTMATRX0101TLKB"
    Public Const ParameterFileIdentifier As String = "ONTMATRX0101ATEM"

    ''' <summary>
    ''' Regular expression to extract the curve name of the spectroscopy file from the file extension.
    ''' </summary>
    Private SpectroscopyFileExtension As New Regex("^.(?<curve>.*?\(?\))_mtrx$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Regular expression to extract the scan image name of the scan image file from the extension.
    ''' </summary>
    Private ScanImageFileExtension As New Regex("^.(?<curve>.[^\(\)]*?)_mtrx$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Imports the spectroscopy-file into a SpectroscopyTable object.
    ''' </summary>
    Public Function ImportBias(ByRef FullFileNamePlusPath As String,
                               ByVal FetchOnlyFileHeader As Boolean,
                               Optional ByRef ReaderBuffer As String = "",
                               Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing) As cSpectroscopyTable Implements iFileImport_SpectroscopyTable.ImportSpectroscopyTable

        ' Check, if the ignore list is nothing. If yes, then create a new list.
        If FilesToIgnoreAfterThisImport Is Nothing Then FilesToIgnoreAfterThisImport = New List(Of String)

        ' First get the file base name from the individual file name.
        Dim FileName As String = IO.Path.GetFileName(FullFileNamePlusPath)
        Dim FileExtension As String = IO.Path.GetExtension(FileName)

        ' Get the curve name
        Dim CurveNameMatch As Match = SpectroscopyFileExtension.Match(FileExtension)
        Dim CurveName As String = String.Empty
        If CurveNameMatch.Groups.Count > 1 Then
            CurveName = CurveNameMatch.Groups.Item("curve").Value
        End If

        ' Create new SpectroscopyTable
        Dim oSpectroscopyTable As New cSpectroscopyTable
        oSpectroscopyTable.FullFileName = FullFileNamePlusPath

        ' Load StreamReader
        Dim sr As New FileStream(FullFileNamePlusPath, FileMode.Open)

        ReaderBuffer = ""

        ' Buffers
        Dim LastFourChars(3) As Char
        Dim Int32Buffer(3) As Byte
        Dim Int64Buffer(7) As Byte

        Dim ExpectedNumber As UInt32
        Dim RecordedNumber As UInt32
        Dim RecordTimeTicks As ULong
        Dim RecordTime As Date
        Dim Data As List(Of Integer)

        ' Read the header up to the position of the data, which is announced by "ATAD".
        Do Until sr.Position = sr.Length Or LastFourChars = "ATAD"

            ' Move the identifier buffer by one byte.
            LastFourChars(0) = LastFourChars(1)
            LastFourChars(1) = LastFourChars(2)
            LastFourChars(2) = LastFourChars(3)
            LastFourChars(3) = Convert.ToChar(sr.ReadByte)

            Select Case LastFourChars

                Case "ATAD"

                    ' Abort reading of the header. From here on the data starts.
                    'If FetchOnlyFileHeader Then
                    '    Exit Do
                    'Else

                    ' Jump over the first 4 bytes!
                    sr.Seek(4, SeekOrigin.Current)

                    ' read the data
                    Data = New List(Of Integer)
                    Do Until sr.Position = sr.Length
                        sr.Read(Int32Buffer, 0, 4)
                        Data.Add(BitConverter.ToInt32(Int32Buffer, 0))
                    Loop
                    'End If

                Case "TLKB"
                    ' Timestamp of the file. The next 8 bytes.
                    sr.Read(Int64Buffer, 0, 8)
                    Array.Reverse(Int64Buffer)
                    RecordTimeTicks = BitConverter.ToUInt64(Int64Buffer, 0)
                    'RecordTime = Convert.ToDateTime(RecordTimeTicks)

                Case "CSED"
                    ' The next 24 bytes are unknown.
                    sr.Seek(24, SeekOrigin.Current)
                    ' The next 4 bytes are UINT32-LE as point number.
                    sr.Read(Int32Buffer, 0, 4)
                    ExpectedNumber = BitConverter.ToUInt32(Int32Buffer, 0)
                    sr.Read(Int32Buffer, 0, 4)
                    RecordedNumber = BitConverter.ToUInt32(Int32Buffer, 0)

            End Select

        Loop

        sr.Close()
        sr.Dispose()

        ' File Exists, so set the property.
        oSpectroscopyTable._bFileExists = True

        Return oSpectroscopyTable
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_SpectroscopyTable.FileExtension
        Get
            Return "_mtrx"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known Omicron-Matrix File-Type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile

        ' Load StreamReader and read first identifier.
        ' Is the only one needed for Identification.
        Dim sr As New StreamReader(FullFileNamePlusPath)
        Dim Buffer(DataFileIdentifier.Length - 1) As Char
        sr.ReadBlock(Buffer, 0, DataFileIdentifier.Length)
        sr.Close()
        sr.Dispose()

        ' All Omicron data files start with the DataFileIdentifier.
        ' If we stumble upon this identifier, we can start loading the file.
        ' For the individual we can then load the parameter file separately.
        If Buffer = DataFileIdentifier Then

            ' Now check, if the file is a SpectroscopyFile.
            ' This is done from the file-extension, which should contain
            ' the curve name, e.g. I(V)_mtrx.
            Dim FileExtension As String = IO.Path.GetExtension(FullFileNamePlusPath)
            If SpectroscopyFileExtension.IsMatch(FileExtension) Then
                Return True
            End If

        End If

        Return False
    End Function

End Class
