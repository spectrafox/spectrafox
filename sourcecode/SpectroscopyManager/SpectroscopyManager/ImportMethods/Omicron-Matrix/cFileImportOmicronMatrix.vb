Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' Base class for file-imports regarding Omicron Matrix files.
''' </summary>
Public Class cFileImportOmicronMatrix

    ''' <summary>
    ''' Different identifiers of the Omicron-files.
    ''' </summary>
    Public Const FileIdentifier As String = "ONTMATRX0101"
    Public Const DataFileIdentifier As String = "ONTMATRX0101TLKB"
    Public Const ParameterFileIdentifier As String = "ONTMATRX0101ATEM"

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


    ''' <summary>
    ''' Maximum length of a string.
    ''' Used to check for a valid readout.
    ''' </summary>
    Public Const MaxStringLength As Integer = 10000

    ''' <summary>
    ''' Reads a Matrix-String. It consists out of a length as UInt32,
    ''' and is followed by the string in UTF16 format.
    ''' </summary>
    ''' <param name="br">BinaryReader pointing to the Stream-Object</param>
    ''' <returns>String with the extracted line.</returns>
    Public Shared Function ReadString(ByRef br As BinaryReader) As String
        Dim out As String = String.Empty

        ' Get the length of the string. It is written before the string.
        Dim LengthOfString As UInteger = br.ReadUInt32

        ' Check for a valid string length.
        If LengthOfString > MaxStringLength Then
            Debug.WriteLine("MatrixReader_ReadString: string not readable... too long")
            Return out
        End If

        ' Now read the 16bit per character string.
        Dim ch1 As String
        ' Read until end of stream, or the length of the stream is reached.
        Do Until br.BaseStream.Position = br.BaseStream.Length Or out.Length >= LengthOfString
            ch1 = br.ReadChar
            out &= ch1
        Loop
        Return out
    End Function

    ''' <summary>
    ''' Reads a UInt32 value. Announced by "GNOL".
    ''' </summary>
    Public Shared Function ReadUInt32(ByRef br As BinaryReader) As UInt32

        ' Now read 4 chars and check for the identifier.
        Dim Identifier As String = br.ReadChars(4)
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
        Dim Identifier As String = br.ReadChars(4)
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
        Dim Identifier As String = br.ReadChars(4)
        If Identifier = "BUOD" Then
            Return br.ReadDouble
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Reads a string announced by an identifier value. Announced by "GRTS".
    ''' </summary>
    Public Shared Function ReadStringByIdentifier(ByRef br As BinaryReader) As String

        ' Now read 4 chars and check for the identifier.
        Dim Identifier As String = br.ReadChars(4)
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
        Dim Identifier As String = Convert.ToChar(br.ReadByte) & Convert.ToChar(br.ReadByte) & Convert.ToChar(br.ReadByte) & Convert.ToChar(br.ReadByte)
        Select Case Identifier
            Case "GNOL"
                Return br.ReadUInt32.ToString
            Case "LOOB"
                Return Convert.ToBoolean(br.ReadUInt32).ToString
            Case "BUOD"
                Return br.ReadDouble().ToString
            Case "GRTS"
                Return ReadString(br)
            Case Else
                Return Nothing
        End Select


    End Function

End Class
