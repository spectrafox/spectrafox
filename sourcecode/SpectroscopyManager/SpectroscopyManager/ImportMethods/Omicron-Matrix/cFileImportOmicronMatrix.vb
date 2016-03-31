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
        Public DataIsUnitOf As String
        Public Function ChannelNameInclDependingUnit() As String
            Return CurveName & "(" & DataIsUnitOf & ")"
        End Function
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
            FileNameComponents.DataIsUnitOf = SpectroscopyFileMatch.Groups.Item("unit").Value
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

#Region "Transferfunction definition"

    ''' <summary>
    ''' Types of possible data transfer functions.
    ''' </summary>
    Public Enum TransferFunctionTypes
        Unknown
        Linear1D
        MultiLinear1D
    End Enum

    ''' <summary>
    ''' Structure that stores the scaling information of the values.
    ''' </summary>
    Public Structure TransferFunction
        Public TransferFunctionType As TransferFunctionTypes
        Public Factor_1 As Double
        Public Offset As Double
        Public NeutralFactor_2 As Double
        Public Prefactor_2 As Double
        Public Preoffset_2 As Double
        Public Raw1_2 As Double
        Public Whole_2 As Double
        Public ChannelNumber As Integer
        Public Unit As String
    End Structure

    ''' <summary>
    ''' Returns the TransferFunction by the name in the file.
    ''' </summary>
    Public Shared Function GetTransferFunctionByName(ByVal Name As String) As TransferFunctionTypes
        Select Case Name
            Case "TFF_Linear1D"
                Return TransferFunctionTypes.Linear1D
            Case "TFF_MultiLinear1D"
                Return TransferFunctionTypes.MultiLinear1D
            Case Else
                Return TransferFunctionTypes.Unknown
        End Select
    End Function

    ''' <summary>
    ''' Calculates from the given Integer value the correct double value,
    ''' using the current transfer function stored in <code>TransferFunction</code>.
    ''' </summary>
    Public Shared Function GetValueByTransferFunction(ByVal Value As Integer, ByVal ZScaling As TransferFunction) As Double
        Dim DoubleValue As Double = Convert.ToDouble(Value)
        Select Case ZScaling.TransferFunctionType
            Case TransferFunctionTypes.Linear1D
                ' // use linear1d: p = (r - n)/f
                Return (DoubleValue - ZScaling.Offset) / ZScaling.Factor_1
            Case TransferFunctionTypes.MultiLinear1D
                ' // use multilinear1d:
                ' // p = (r - n)*(r0 - n0)/(fn * f0)
                ' //= (r - n)*s.whole_2
                Return (DoubleValue - ZScaling.Preoffset_2) * ZScaling.Whole_2
            Case TransferFunctionTypes.Unknown
                Return DoubleValue
            Case Else
                Return DoubleValue
        End Select
    End Function

#End Region

#Region "Measurement Details"

    ''' <summary>
    ''' Class that contains all details of a measurement.
    ''' E.g. Data types, channels, etc.
    ''' </summary>
    Public Class MeasurementDetails

        ''' <summary>
        ''' Date of the measurement.
        ''' </summary>
        Public Time As Date

        ''' <summary>
        ''' Data types of all channels
        ''' </summary>
        Public DataTypeList As New Dictionary(Of String, String)

        ''' <summary>
        ''' Channellist in the format (ChannelID, (Name, Unit)
        ''' </summary>
        Public Channels As New Dictionary(Of Integer, KeyValuePair(Of String, String))

        ''' <summary>
        ''' Block Storage List. Format: (ChannelIndex, StorageString)
        ''' </summary>
        Public BlockStorage As New Dictionary(Of Integer, String)

        ''' <summary>
        ''' Transferfunction dictionary (ChannelID, Transferfunction)
        ''' </summary>
        Public TransferFunctions As New Dictionary(Of Integer, TransferFunction)

        ''' <summary>
        ''' Returns the ChannelID by the ChannelName from the list of channels.
        ''' </summary>
        ''' <return>-1 if not found</return>
        Public Function GetChannelIDFromMeasurement(ChannelName As String) As Integer
            For Each ChannelKV As KeyValuePair(Of Integer, KeyValuePair(Of String, String)) In Me.Channels
                If ChannelKV.Value.Key = ChannelName Then
                    Return ChannelKV.Key
                End If
            Next
            Return -1
        End Function

    End Class

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

        ' Copy the string, and leave out the first char,
        ' that we anyhow would have thrown away.
        If StringSoFar.Length >= 1 Then
            StringSoFar.CopyTo(1, LastFourChars, 0, StringSoFar.Length - 1)
        End If

        ' Read the new char at the end of the string.
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

#Region "Parameterfile: property array conversion"

    ''' <summary>
    ''' Structure storing the experiment configuration of the Matrix parameter file.
    ''' </summary>
    Public Structure ExperimentConfiguration
        Public Category As String
        Public Instruction As String
        Public Prop As String
        Public Unit As String

        ''' <summary>
        ''' Returns the PropertyArray identifier of the property.
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Category & ":" & Instruction & "." & Prop & "[" & Unit & "]"
        End Function

        ''' <summary>
        ''' Regular expression to extract the information in an EEPA string from the property value again.
        ''' Format of the string: Category & ":" & Instruction & "." & Prop & "[" & Unit & "]"
        ''' </summary>
        Public Shared PropertyArrayRegex As New Regex("^(?<cat>.{4})\:(?<instr>.*?)\.(?<prop>.*?)\[(?<unit>.*?)\]$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

        ''' <summary>
        ''' Returns a property array instruction from the given input string.
        ''' </summary>
        ''' <returns>Empty dummy instruction, if not interpretable. = Nothing</returns>
        Public Shared Function FromString(ByVal ParseString As String) As ExperimentConfiguration
            Dim Instruction As New ExperimentConfiguration

            ' Try to parse the string.
            Dim M As Match = PropertyArrayRegex.Match(ParseString)
            If M.Success AndAlso M.Groups.Count >= 4 Then

                ' Return the parsed array.
                With Instruction
                    .Category = M.Groups.Item("cat").Value
                    .Instruction = M.Groups.Item("instr").Value
                    .Prop = M.Groups.Item("prop").Value
                    .Unit = M.Groups.Item("unit").Value
                End With

            Else

                ' Return dummy.
                With Instruction
                    .Category = String.Empty
                    .Instruction = String.Empty
                    .Prop = String.Empty
                    .Unit = String.Empty
                End With

            End If

            Return Instruction

        End Function

    End Structure

#End Region

#Region "Matrix STS Location interpreter"

    ''' <summary>
    ''' Regular expression to extract the information in an EEPA string from the property value again.
    ''' Format of the string: Category & ":" & Instruction & "." & Prop & "[" & Unit & "]"
    ''' </summary>
    Public Shared MatrixSTSLocationRegex As New Regex("^MTRX\$STS_LOCATION(?<xpoint>.*?),(?<ypoint>.*?);(?<xcoord>.*?),(?<ycoord>.*?)\%\%(?<extra>.*?)\%\%$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Stores the location of a spectroscopy experiment.
    ''' </summary>
    Public Structure STSLocation
        Public XPoint As Integer
        Public YPoint As Integer
        Public XCoord As Double
        Public YCoord As Double
    End Structure

    ''' <summary>
    ''' Tries to convert the property string into a match to <code>MatrixSTSLocationRegex</code>.
    ''' True, if conversion was successfull.
    ''' </summary>
    Public Shared Function GetSTSLocationExperimentConfigurationFromMatrix(ByVal PropertyString As String,
                                                                           ByRef SpectroscopyLocation As STSLocation) As Boolean

        Dim M As Match = MatrixSTSLocationRegex.Match(PropertyString)
        If M.Success AndAlso M.Groups.Count >= 4 Then
            SpectroscopyLocation = New STSLocation
            With SpectroscopyLocation
                .XCoord = Convert.ToDouble(M.Groups.Item("xcoord").Value, Globalization.CultureInfo.InvariantCulture)
                .YCoord = Convert.ToDouble(M.Groups.Item("ycoord").Value, Globalization.CultureInfo.InvariantCulture)
                .XPoint = Convert.ToInt32(M.Groups.Item("xpoint").Value, Globalization.CultureInfo.InvariantCulture)
                .YPoint = Convert.ToInt32(M.Groups.Item("ypoint").Value, Globalization.CultureInfo.InvariantCulture)
            End With
            Return True
        End If

        Return False
    End Function


#End Region

#Region "XYScanner specific functions"

    ''' <summary>
    ''' Structure that stores the properties of the XYScanner,
    ''' which contains the information on how to interpret the data.
    ''' </summary>
    Public Class XYScannerProperties

        Public Height As Double = -1
        Public Width As Double = -1
        Public XPoints As Integer = -1
        Public YPoints As Integer = -1

        Public XOffset As Double = 0
        Public YOffset As Double = 0
        Public Angle As Double = 0

        Public Setpoint As Double = 0
        Public SetpointUnit As String = ""
        Public ProportionalGain As Double = 0
        Public BiasVoltage As Double = 0

        ''' <summary>
        ''' Raster time in seconds.
        ''' </summary>
        Public RasterPeriodTime As Double = 1

        ''' <summary>
        ''' Data mode.
        ''' </summary>
        Public GridMode As Integer = 0

        ''' <summary>
        ''' Zoom of the image.
        ''' </summary>
        Public Zoom As Integer = 1

    End Class

    ''' <summary>
    ''' Returns the XYScanner properties from the parameter file for the specific image file.
    ''' </summary>
    Public Shared Function GetXYScannerPropertiesFromPropertyArray(ByVal ImageFileNameWithoutPath As String,
                                                                   ByRef ParameterFile As cFileImportOmicronMatrixParameterFile) As XYScannerProperties

        Dim XY As New XYScannerProperties

        ' Check, if the parameter file is valid.
        If ParameterFile Is Nothing Then Return XY

        ' First get the initial parameter set,
        ' that is modified afterwards by the PMOD commands.
        For Each Prop As KeyValuePair(Of ExperimentConfiguration, String) In ParameterFile.InitialExperimentConfigurationArray
            ModifyXYScannerByParameter(XY, Prop.Key, Prop.Value)
        Next

        ' Now we get the position of the file in the chronology.
        Dim FilePosition As Integer = ParameterFile.GetPositionOfPropertyValueInActionList(ImageFileNameWithoutPath)

        If FilePosition >= 0 Then
            ' Now we get the modified parameters specific for the image
            ' by walking through the chronology up to the image,
            ' and modifying the parameters accordingly.
            For i As Integer = 0 To FilePosition - 1 Step 1

                ' Try to get a XYscanner parameter.
                ModifyXYScannerByParameter(XY,
                                           ExperimentConfiguration.FromString(ParameterFile.ActionsByTime(i).Value.Key),
                                           ParameterFile.ActionsByTime(i).Value.Value)

            Next
        End If

        Return XY
    End Function

    ''' <summary>
    ''' Uses a value from the property array of a parameter file to modify the XYScanner object.
    ''' </summary>
    Protected Shared Sub ModifyXYScannerByParameter(ByRef XY As XYScannerProperties,
                                                    ByVal ExpConfig As ExperimentConfiguration,
                                                    ByVal Value As String)

        ' Check, if the property deals with the XYScanner.
        If ExpConfig.Category = "EEPA" AndAlso ExpConfig.Instruction = "XYScanner" Then

            Select Case ExpConfig.Prop

                Case "Height"
                    XY.Height = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Width"
                    XY.Width = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Points", "X_Points"
                    XY.XPoints = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Lines", "Y_Points"
                    XY.YPoints = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Raster_Time", "Raster_Period_Time"
                    XY.RasterPeriodTime = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Scan_Constraint"
                    XY.GridMode = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Zoom"
                    XY.Zoom = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Angle"
                    XY.Angle = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                Case "X_Offset"
                    XY.XOffset = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                Case "Y_Offset"
                    XY.YOffset = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)

            End Select

        End If

        ' Check, if the property deals with the Regulator.
        If ExpConfig.Category = "EEPA" AndAlso ExpConfig.Instruction = "Regulator" Then

            Select Case ExpConfig.Prop

                Case "Setpoint_1"
                    XY.Setpoint = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                    XY.SetpointUnit = ExpConfig.Unit
                Case "Retraction_Speed"
                    XY.ProportionalGain = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)

            End Select

        End If

        ' Check, if the property deals with the Regulator.
        If ExpConfig.Category = "EEPA" AndAlso ExpConfig.Instruction = "GapVoltageControl" Then

            Select Case ExpConfig.Prop

                Case "Voltage"
                    XY.BiasVoltage = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)

            End Select

        End If

    End Sub

#End Region

#Region "Spectroscopy specific functions"

    ''' <summary>
    ''' Structure that stores the properties of the SpectroscopyUnit,
    ''' which contains the information on how to interpret the data.
    ''' </summary>
    Public Class SpectroscopyProperties

        Public Device1_SweepStart As Double = 0
        Public Device1_SweepEnd As Double = 0
        Public Device1_UnitString As String = String.Empty
        Public Device1_Points As Integer = 0
        Public Device1_ForwardAndBackward As Boolean = False
        Public Device1_SweepNumber As Integer = 0

        Public Device2_SweepStart As Double = 0
        Public Device2_SweepEnd As Double = 0
        Public Device2_UnitString As String = String.Empty
        Public Device2_Points As Integer = 0
        Public Device2_ForwardAndBackward As Boolean = False
        Public Device2_SweepNumber As Integer = 0
        Public Device2_Offset As Double = 0

        ''' <summary>
        ''' Feedback on?
        ''' </summary>
        Public DisableFeedback As Boolean

        ''' <summary>
        ''' Location of the spectroscopy.
        ''' </summary>
        Public Location As New STSLocation

    End Class

    ''' <summary>
    ''' Returns the Spectroscopy properties from the parameter file for the specific image file.
    ''' </summary>
    Public Shared Function GetSpectroscopyPropertiesFromPropertyArray(ByVal ImageFileNameWithoutPath As String,
                                                                      ByRef ParameterFile As cFileImportOmicronMatrixParameterFile) As SpectroscopyProperties

        Dim SpecProp As New SpectroscopyProperties

        ' Check, if the parameter file is valid.
        If ParameterFile Is Nothing Then Return SpecProp

        ' First get the initial parameter set,
        ' that is modified afterwards by the PMOD commands.
        For Each Prop As KeyValuePair(Of ExperimentConfiguration, String) In ParameterFile.InitialExperimentConfigurationArray
            ModifySpectroscopyPropertiesByParameter(SpecProp, Prop.Key, Prop.Value)
        Next

        ' Now we get the position of the file in the chronology.
        Dim FilePosition As Integer = ParameterFile.GetPositionOfPropertyValueInActionList(ImageFileNameWithoutPath)

        If FilePosition >= 0 Then
            ' Now we get the modified parameters specific for the image
            ' by walking through the chronology up to the image,
            ' and modifying the parameters accordingly.
            For i As Integer = 0 To FilePosition - 1 Step 1

                ' Try to get a XYscanner parameter.
                ModifySpectroscopyPropertiesByParameter(SpecProp,
                                                        ExperimentConfiguration.FromString(ParameterFile.ActionsByTime(i).Value.Key),
                                                        ParameterFile.ActionsByTime(i).Value.Value)

                ' Try to get the spectroscopy location.
                GetSTSLocationExperimentConfigurationFromMatrix(ParameterFile.ActionsByTime(i).Value.Key, SpecProp.Location)

            Next
        End If

        Return SpecProp
    End Function

    ''' <summary>
    ''' Uses a value from the property array of a parameter file to modify the XYScanner object.
    ''' </summary>
    Protected Shared Sub ModifySpectroscopyPropertiesByParameter(ByRef SpecProp As SpectroscopyProperties,
                                                                 ByVal ExpConfig As ExperimentConfiguration,
                                                                 ByVal Value As String)

        ' Check, if the property deals with the Spectroscopy properties.
        ' If not then skip the function.
        If ExpConfig.Category <> "EEPA" AndAlso ExpConfig.Instruction <> "Spectroscopy" Then Return

        Select Case ExpConfig.Prop

            Case "Device_1_Start"
                SpecProp.Device1_SweepStart = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                SpecProp.Device1_UnitString = ExpConfig.Unit
            Case "Device_1_End"
                SpecProp.Device1_SweepEnd = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Device_1_Points"
                SpecProp.Device1_Points = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Enable_Device_1_Ramp_Reversal"
                SpecProp.Device1_ForwardAndBackward = Convert.ToBoolean(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Device_1_Repetitions"
                SpecProp.Device1_SweepNumber = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)

            Case "Device_2_Start"
                SpecProp.Device2_SweepStart = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
                SpecProp.Device2_UnitString = ExpConfig.Unit
            Case "Device_2_End"
                SpecProp.Device2_SweepEnd = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Device_2_Points"
                SpecProp.Device2_Points = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Enable_Device_2_Ramp_Reversal"
                SpecProp.Device2_ForwardAndBackward = Convert.ToBoolean(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Device_2_Repetitions"
                SpecProp.Device2_SweepNumber = Convert.ToInt32(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Device_2_Offset"
                SpecProp.Device2_Offset = Convert.ToDouble(Value, Globalization.CultureInfo.InvariantCulture)

            Case "Disable_Feedback_Loop"
                SpecProp.DisableFeedback = Convert.ToBoolean(Value, Globalization.CultureInfo.InvariantCulture)
            Case "Enable_Feedback_Loop"
                SpecProp.DisableFeedback = Not Convert.ToBoolean(Value, Globalization.CultureInfo.InvariantCulture)


        End Select
    End Sub

#End Region

End Class
