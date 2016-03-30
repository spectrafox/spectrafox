Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' Base class for file-imports regarding Omicron Matrix files.
''' </summary>
Public Class cFileImportOmicronMatrix

#Region "Filename analysis and interpretation"

    ''' <summary>
    ''' Different identifiers of the Omicron-files.
    ''' </summary>
    Public Const FileIdentifier As String = "ONTMATRX0101"
    Public Const DataFileIdentifier As String = "ONTMATRX0101TLKB"

    ''' <summary>
    ''' Regular expression to extract the curve name of the spectroscopy file from the file extension.
    ''' </summary>
    Protected SpectroscopyFileExtension As New Regex("^.(?<curve>.*?\(?\))_mtrx$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Regex to recognize a SpectroscopyFile of the Omicron Matrix system.
    ''' It is setup by four parts. "20140823-115152--3_1.Aux1(V)_mtrx"
    ''' basename: 20140823-115152
    ''' nocurve: 3
    ''' nopass: 1
    ''' curve: Aux1
    ''' unit: V
    ''' </summary>
    Protected SpectroscopyFileRegex As New Regex("^(?<basename>.*?)--(?<nocurve>\d+)_(?<nopass>\d+)\.(?<curve>\w*?)\((?<unit>.*?)\)_mtrx$", RegexOptions.Compiled Or RegexOptions.Singleline Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Structure that stores the contents of a filename.
    ''' </summary>
    Public Structure SpectroscopyFileName
        Public BaseName As String
        Public CurveNumber As String
        Public PassNumber As String
        Public CurveName As String
        Public DataUnit As String
    End Structure

    ''' <summary>
    ''' Uses a regular expression to split up the filename of a Omicron Matrix spectroscopy file.
    ''' </summary>
    Public Function GetSpectroscopyFileNameComponents(ByVal FileNameWithoutPath As String) As SpectroscopyFileName
        Dim SpectroscopyFileMatch As Match = SpectroscopyFileRegex.Match(FileNameWithoutPath)
        Dim FileNameComponents As New SpectroscopyFileName
        If SpectroscopyFileMatch.Groups.Count > 1 Then
            FileNameComponents.BaseName = SpectroscopyFileMatch.Groups.Item("basename").Value
            FileNameComponents.CurveNumber = SpectroscopyFileMatch.Groups.Item("nocurve").Value
            FileNameComponents.PassNumber = SpectroscopyFileMatch.Groups.Item("nopass").Value
            FileNameComponents.CurveName = SpectroscopyFileMatch.Groups.Item("curve").Value
            FileNameComponents.DataUnit = SpectroscopyFileMatch.Groups.Item("unit").Value
        End If
        Return FileNameComponents
    End Function

    ''' <summary>
    ''' Regular expression to extract the scan image name of the scan image file from the extension.
    ''' </summary>
    Protected ScanImageFileExtension As New Regex("^\.(?<channel>\w*?)_mtrx$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Regex to recognize a ScanImage of the Omicron Matrix system.
    ''' It is setup by four parts. "20140823-115152--1_1.Z_mtrx"
    ''' basename: 20140823-115152
    ''' nocurve: 1
    ''' nopass: 1
    ''' channel: Z
    ''' </summary>
    Protected ScanImageFileRegex As New Regex("^(?<basename>.*?)--(?<nocurve>\d+)_(?<nopass>\d+)\.(?<channel>\w*?)_mtrx$", RegexOptions.Compiled Or RegexOptions.Singleline Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Structure that stores the contents of a filename.
    ''' </summary>
    Public Structure ScanImageFileName
        Public BaseName As String
        Public CurveNumber As String
        Public PassNumber As String
        Public ChannelName As String
    End Structure

    ''' <summary>
    ''' Uses a regular expression to split up the filename of a Omicron Matrix scan image file.
    ''' </summary>
    Public Function GetScanImageFileNameComponents(ByVal FileNameWithoutPath As String) As ScanImageFileName
        Dim ScanImageFileMatch As Match = ScanImageFileRegex.Match(FileNameWithoutPath)
        Dim FileNameComponents As New ScanImageFileName
        If ScanImageFileMatch.Groups.Count > 1 Then
            FileNameComponents.BaseName = ScanImageFileMatch.Groups.Item("basename").Value
            FileNameComponents.CurveNumber = ScanImageFileMatch.Groups.Item("nocurve").Value
            FileNameComponents.PassNumber = ScanImageFileMatch.Groups.Item("nopass").Value
            FileNameComponents.ChannelName = ScanImageFileMatch.Groups.Item("channel").Value
        End If
        Return FileNameComponents
    End Function

#End Region

#Region "File Control, e.g. Datablocks, etc."

    ''' <summary>
    ''' Structure that represents a data-block containing no timestamp.
    ''' </summary>
    Public Structure DataBlockWithoutTime
        Public Identifier As String
        Public Position As Int64
        Public Length As UInt32
        Public Content As Byte()
    End Structure

    ''' <summary>
    ''' Structure that represents a data-block containing a timestamp.
    ''' </summary>
    Public Structure DataBlockWithTime
        Public Identifier As String
        Public Position As Int64
        Public Length As UInt32
        Public TimeStamp As UInt64
        Public Time As Date
        Public Content As Byte()
    End Structure

    ''' <summary>
    ''' Goes to the beginning of the data block.
    ''' </summary>
    Public Shared Sub GotoBlock(ByRef br As BinaryReader, Block As DataBlockWithoutTime)
        br.BaseStream.Position = Block.Position
    End Sub

    ''' <summary>
    ''' Goes to the beginning of the data block.
    ''' </summary>
    Public Shared Sub GotoBlock(ByRef br As BinaryReader, Block As DataBlockWithTime)
        br.BaseStream.Position = Block.Position
    End Sub

    ''' <summary>
    ''' Goes to the data block end of a timeless block.
    ''' </summary>
    Public Shared Sub GotoBlockEnd(ByRef br As BinaryReader, Block As DataBlockWithoutTime)
        br.BaseStream.Position = Block.Position + Block.Length + 4
    End Sub

    ''' <summary>
    ''' Goes to the data block end of a timestamp block.
    ''' </summary>
    Public Shared Sub GotoBlockEnd(ByRef br As BinaryReader, Block As DataBlockWithTime)
        br.BaseStream.Position = Block.Position + Block.Length + 12
    End Sub

    ''' <summary>
    ''' Reads the next block of data, given by the length of the parameter.
    ''' </summary>
    <DebuggerStepThrough>
    Public Shared Function ReadBlock(ByRef br As BinaryReader, BlockLength As UInt32) As Byte()
        Return br.ReadBytes(CInt(BlockLength))
    End Function

    ''' <summary>
    ''' Creates a new data block, with the content of the data-block itself.
    ''' The 4 byte identifier should already be read!
    ''' </summary>
    Public Shared Function ReadBlockWithoutTime(ByRef br As BinaryReader, Identifier As String) As DataBlockWithoutTime
        Dim DB As New DataBlockWithoutTime
        With DB
            .Identifier = Identifier
            .Position = br.BaseStream.Position
            ' After each parameter the length of the following block is stored!
            ' Style: "XXXX"+4byte length of the block
            .Length = br.ReadUInt32
            .Content = ReadBlock(br, .Length)
        End With
        Return DB
    End Function

    ''' <summary>
    ''' Creates a new data block, with the content of the data-block itself.
    ''' The 4 byte identifier should already be read!
    ''' </summary>
    Public Shared Function ReadBlockWithTime(ByRef br As BinaryReader, Identifier As String) As DataBlockWithTime
        Dim DB As New DataBlockWithTime
        With DB
            .Identifier = Identifier
            .Position = br.BaseStream.Position
            ' After each parameter the length of the following block is stored!
            ' Style: "XXXX"+4byte length of the block
            .Length = br.ReadUInt32
            ' For some entries the timestamp is stored after the 4 bytes.
            ' In the HEX-editor they always look like "8døS...."
            .TimeStamp = br.ReadUInt64
            .Time = New DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(.TimeStamp).ToLocalTime()
            .Content = ReadBlock(br, .Length)
        End With
        Return DB
    End Function

    ''' <summary>
    ''' Returns a BinaryReader for the specific data block.
    ''' </summary>
    Public Shared Function GetBinaryReaderForBlockContent(DataBlock As DataBlockWithoutTime) As BinaryReader
        Return New BinaryReader(New MemoryStream(DataBlock.Content), System.Text.Encoding.Unicode)
    End Function

    ''' <summary>
    ''' Returns a BinaryReader for the specific data block.
    ''' </summary>
    Public Shared Function GetBinaryReaderForBlockContent(DataBlock As DataBlockWithTime) As BinaryReader
        Return New BinaryReader(New MemoryStream(DataBlock.Content), System.Text.Encoding.Unicode)
    End Function

    ''' <summary>
    ''' Maximum length of a string.
    ''' Used to check for a valid readout.
    ''' </summary>
    Protected Const MaxStringLength As Integer = 10000

    ''' <summary>
    ''' Reads a Matrix-String. It consists out of a length as UInt32,
    ''' and is followed by the string in UTF16 format.
    ''' </summary>
    ''' <param name="br">BinaryReader pointing to the Stream-Object</param>
    ''' <returns>String with the extracted line.</returns>
    Public Shared Function ReadString(ByRef br As BinaryReader) As String

        ' Get the length of the string. It is written before the string.
        Dim LengthOfString As Integer = CInt(br.ReadUInt32)

        ' Check for a valid string length.
        If LengthOfString > MaxStringLength Then
            Debug.WriteLine("MatrixReader_ReadString: string not readable... too long")
            Return String.Empty
        End If

        ' Read the whole string.
        Dim out As String = br.ReadChars(LengthOfString)

        Return out

    End Function

    ''' <summary>
    ''' Reads a UInt32 value. Announced by "GNOL".
    ''' </summary>
    Public Shared Function ReadUInt32(ByRef br As BinaryReader) As UInt32

        ' Now read 4 chars and check for the identifier.
        Dim Identifier As String = Read4Bytes(br)
        If Identifier = "GNOL" Then
            Return br.ReadUInt32
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Reads a Boolean value (32bit). Announced by "LOOB".
    ''' </summary>
    Public Shared Function ReadBool(ByRef br As BinaryReader) As Boolean

        ' Now read 4 chars and check for the identifier.
        Dim Identifier As String = Read4Bytes(br)
        If Identifier = "LOOB" Then
            Return Convert.ToBoolean(br.ReadUInt32)
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Reads a double value. Announced by "BUOD".
    ''' </summary>
    Public Shared Function ReadDouble(ByRef br As BinaryReader) As Double

        ' Now read 4 chars and check for the identifier.
        Dim Identifier As String = Read4Bytes(br)
        If Identifier = "BUOD" Then
            Return br.ReadDouble()
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Reads a string announced by an identifier value. Announced by "GRTS".
    ''' </summary>
    Public Shared Function ReadStringByIdentifier(ByRef br As BinaryReader) As String

        ' Now read 4 chars and check for the identifier.
        Dim Identifier As String = Read4Bytes(br)
        If Identifier = "GRTS" Then
            Return ReadString(br)
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Reads the next value, and outputs it as string.
    ''' The data is interpreted by the identifier.
    ''' </summary>
    Public Shared Function ReadObject(ByRef br As BinaryReader) As String

        ' Now read 4 chars and check for the identifier.
        Dim Identifier As String = Read4Bytes(br)
        Select Case Identifier
            Case "GNOL"
                Dim Int As UInt32 = br.ReadUInt32
                Return Int.ToString(System.Globalization.CultureInfo.InvariantCulture)
            Case "LOOB"
                Dim Int As UInt32 = br.ReadUInt32
                Dim Bool As Boolean = Convert.ToBoolean(Int)
                Return Bool.ToString(System.Globalization.CultureInfo.InvariantCulture)
            Case "BUOD"
                Dim Val As Double = br.ReadDouble
                Return Val.ToString(System.Globalization.CultureInfo.InvariantCulture)
            Case "GRTS"
                Dim S As String = ReadString(br)
                Return S
            Case Else
                Return Nothing
        End Select

    End Function

    ''' <summary>
    ''' Function that returns the last four chars by proceeding by one byte.
    ''' Requires a String with less than four chars long.
    ''' </summary>
    Public Shared Function GetLastFourCharsByProceedingOneByte(ByRef br As BinaryReader, ByVal StringSoFar As String) As String

        Dim LastFourChars(3) As Char
        StringSoFar.CopyTo(0, LastFourChars, 0, StringSoFar.Length)

        ' Move the identifier buffer by one byte.
        LastFourChars(0) = LastFourChars(1)
        LastFourChars(1) = LastFourChars(2)
        LastFourChars(2) = LastFourChars(3)
        LastFourChars(3) = Convert.ToChar(br.ReadByte)

        Return LastFourChars

    End Function

    ''' <summary>
    ''' Reads exactly four bytes and returns them as char.
    ''' This is used for the identifier strings, e.g. "LOOB", etc.
    ''' </summary>
    Public Shared Function Read4Bytes(ByRef br As BinaryReader) As String
        Return Convert.ToChar(br.ReadByte) & Convert.ToChar(br.ReadByte) & Convert.ToChar(br.ReadByte) & Convert.ToChar(br.ReadByte)
    End Function

#End Region

End Class
