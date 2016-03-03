Imports System.IO
Imports System.Text.RegularExpressions

Public Class cFileImportNanonisSXM
    Implements iFileImport_ScanImage

    ''' <summary>
    ''' Imports the ScanImage of an SXM-File into an Image-Object
    ''' </summary>
    Public Function ImportSXM(ByRef FullFileNamePlusPath As String,
                              ByVal FetchOnlyFileHeader As Boolean,
                              Optional ByRef ReaderBuffer As String = "") As cScanImage Implements iFileImport_ScanImage.ImportScanImage

        ' Create New ScanImage object
        Dim oScanImage As New cScanImage
        oScanImage.FullFileName = FullFileNamePlusPath

        ' Load StreamReader with the Big Endian Encoding
        'Dim sr As New StreamReader(Me.sFullFileName)
        Dim fs As New FileStream(FullFileNamePlusPath, FileMode.Open, FileAccess.Read, FileShare.Read)
        ' Now Using BinaryReader to obtain Image-Data
        Dim br As New BinaryReader(fs, System.Text.Encoding.UTF8)

        ' Read File Line-By-Line and Export Settings and Data.
        Dim sHeader As String = ""
        Dim sLine As String = ""

        ' This variable tells, if the routine should read
        ' the next Header-Line, or if the Header-Line is already
        ' read in by e.g. the Comment-Column.
        Dim ReadNextTag As Boolean = True

        ' Variable for saving the Record-Time
        Dim TimeString As String = ""

        ' Variable for saving, if the Image is recorded from bottom or top
        Dim RecordingDirection As String = ""

        ' Buffer String
        ReaderBuffer = ""

        ' Read Settings up to the Position of the end of the Configuration-Settings
        ' A Setting is always introduced by the Setting-Name in the first line
        ' between two colons :SETTINGNAME: and the properties in the second line.
        ' Some properties - such as the comments - can consist of even more lines.
        ' Here the file is readed until the next settings-name is reached.
        Do Until sLine.Contains(":SCANIT_END:") Or br.BaseStream.Position = br.BaseStream.Length
            If ReadNextTag Then
                sHeader = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
            End If

            ' Get Setting-Name (Remove Colon at begin and end of name)
            sHeader = sHeader.Substring(1, sHeader.Length - 2)
            ReadNextTag = True

            ' Read the next Line = First line of Settings.
            sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim

            ' Read Settings
            Select Case sHeader
                Case "COMMENT"
                    ' Comment, read until next settings-name starts.
                    While Not sLine.StartsWith(":")
                        oScanImage.Comment &= sLine & vbCrLf
                        sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
                    End While
                    sHeader = sLine
                    ReadNextTag = False ' Already read the next tag
                Case "Z-CONTROLLER"

                    ' Read out the controller settings:
                    ' First line is the header line, second the parameter line.
                    Dim SetpointParameterString As String = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
                    oScanImage.ZControllerSettingsRawString &= sLine & vbCrLf & SetpointParameterString

                    If SetpointParameterString <> String.Empty Then
                        ' Interpret the contoller settings parameter line.
                        Dim SetpointParameters As String() = SetpointParameterString.Split(ControlChars.Tab)

                        ' Read "Double String" by RegEx: as interpreter of units
                        Dim RegExUnitValue As New Regex("(?<Value>[\-]?[0-9]*?\.[0-9]*?E[+\-][0-9]*)\s(?<Unit>.?)", RegexOptions.Compiled)
                        Dim M As Match

                        If SetpointParameters.Length >= 6 Then
                            oScanImage.ZControllerName = SetpointParameters(0)
                            oScanImage.ZControllerOn = (SetpointParameters(1) = "1")

                            M = RegExUnitValue.Match(SetpointParameters(2))
                            If M.Success Then
                                oScanImage.ZControllerSetpoint = Double.Parse(M.Groups("Value").Value, Globalization.CultureInfo.InvariantCulture)
                                oScanImage.ZControllerSetpointUnit = M.Groups("Unit").Value
                            End If
                            M = RegExUnitValue.Match(SetpointParameters(3))
                            If M.Success Then oScanImage.ZControllerProportionalGain = Double.Parse(M.Groups("Value").Value, Globalization.CultureInfo.InvariantCulture)
                            M = RegExUnitValue.Match(SetpointParameters(4))
                            If M.Success Then oScanImage.ZControllerIntegralGain = Double.Parse(M.Groups("Value").Value, Globalization.CultureInfo.InvariantCulture)
                            M = RegExUnitValue.Match(SetpointParameters(5))
                            If M.Success Then oScanImage.ZControllerTimeConstant = Double.Parse(M.Groups("Value").Value, Globalization.CultureInfo.InvariantCulture)
                        End If
                    End If

                Case "Multipass-Config"
                    ' Multipass-Config, read until next settings-name starts.
                    While Not sLine.StartsWith(":")
                        oScanImage.MultiPassConfig &= sLine & vbCrLf
                        sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
                    End While
                    sHeader = sLine
                    ReadNextTag = False ' Already read the next tag

                Case "REC_DATE"
                    TimeString &= " " & sLine
                Case "REC_TIME"
                    TimeString &= " " & sLine
                Case "SCAN_DIR"
                    RecordingDirection = sLine
                Case "SCAN_ANGLE"
                    oScanImage.ScanAngle = Double.Parse(sLine, Globalization.CultureInfo.InvariantCulture)
                Case "ACQ_TIME"
                    oScanImage.ACQ_Time = Double.Parse(sLine, Globalization.CultureInfo.InvariantCulture)
                Case "BIAS"
                    oScanImage.Bias = Double.Parse(sLine, Globalization.CultureInfo.InvariantCulture)
                Case "SCAN_RANGE"
                    Dim oRegEx As New Regex("(?<XValue>[\-]?[0-9]*?\.[0-9]*?E[+\-][0-9]*)\s*?(?<YValue>[\-]?[0-9]*?\.[0-9]*?E[+\-][0-9]*)", RegexOptions.Compiled)
                    Dim oMatch As Match = oRegEx.Match(sLine)

                    ' If the X and Z Parameters could be identified
                    If oMatch.Success Then
                        oScanImage.ScanRange_X = Double.Parse(oMatch.Groups("XValue").Value, Globalization.CultureInfo.InvariantCulture)
                        oScanImage.ScanRange_Y = Double.Parse(oMatch.Groups("YValue").Value, Globalization.CultureInfo.InvariantCulture)
                    End If
                Case "SCAN_OFFSET"
                    ' Read two parameters: use regex since separation is not unique
                    Dim oRegEx As New Regex("(?<XValue>[\-]?[0-9]*?\.[0-9]*?E[+\-][0-9]*)\s*?(?<YValue>[\-]?[0-9]*?\.[0-9]*?E[+\-][0-9]*)", RegexOptions.Compiled)
                    Dim oMatch As Match = oRegEx.Match(sLine)

                    ' If the X and Z Parameters could be identified
                    If oMatch.Success Then
                        oScanImage.ScanOffset_X = Double.Parse(oMatch.Groups("XValue").Value, Globalization.CultureInfo.InvariantCulture)
                        oScanImage.ScanOffset_Y = Double.Parse(oMatch.Groups("YValue").Value, Globalization.CultureInfo.InvariantCulture)
                    End If
                Case "SCAN_PIXELS"
                    ' Read two parameters: use regex since separation is not unique
                    Dim oRegEx As New Regex("\s*(?<XValue>\d*)\s*(?<YValue>\d*)\s*", RegexOptions.Compiled)
                    Dim oMatch As Match = oRegEx.Match(sLine)

                    ' If the X and Z Parameters could be identified
                    If oMatch.Success Then
                        oScanImage.ScanPixels_X = Integer.Parse(oMatch.Groups("XValue").Value, Globalization.CultureInfo.InvariantCulture)
                        oScanImage.ScanPixels_Y = Integer.Parse(oMatch.Groups("YValue").Value, Globalization.CultureInfo.InvariantCulture)
                    End If
                Case "DATA_INFO"
                    ' In the DATA_INFO section, all recorded Channels are saved.
                    '###########################################################
                    ' Jump to next Line, since the first line just contains the column headers
                    sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
                    ' Read DATA_INFO until next settings-name starts.
                    While Not sLine.StartsWith(":")
                        If sLine = "" Then
                            sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
                            Continue While
                        End If
                        ' Create and Save new Channels
                        Dim oChannel As New cScanImage.ScanChannel
                        With oChannel
                            Dim oRegEx As New Regex("(?<Number>\d*)\s*(?<Name>[\[\]\%_\w]*)\s*(?<Unit>\w*)\s*(?<Direction>\w*)\s*(?<Calibration>[+\-]?\d*?\.\d*?E[+\-]\d*)\s*(?<Offset>[+\-]?\d*?\.\d*?E[+\-]\d*)", RegexOptions.Compiled)
                            Dim oMatch As Match = oRegEx.Match(sLine)

                            ' Write all Parameters of the Channel in the properties
                            If oMatch.Success Then
                                oChannel.Name = oMatch.Groups("Name").Value
                                oChannel.UnitSymbol = oMatch.Groups("Unit").Value
                                oChannel.Unit = cUnits.GetUnitTypeFromSymbol(oChannel.UnitSymbol)
                                oChannel.Calibration = Double.Parse(oMatch.Groups("Calibration").Value, Globalization.CultureInfo.InvariantCulture)
                                oChannel.Offset = Double.Parse(oMatch.Groups("Offset").Value, Globalization.CultureInfo.InvariantCulture)
                                oChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Forward
                                oChannel.IsSpectraFoxGenerated = False
                                ' Add Channel
                                oScanImage.AddScanChannel(oChannel)
                                ' Create a copy of the Channel for the Backward-Channel, if the Direction is BOTH
                                If oMatch.Groups("Direction").Value = "both" Then
                                    Dim oChannelBW As cScanImage.ScanChannel = oChannel.GetCopy
                                    oChannelBW.Name &= " BW"
                                    oChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Backward
                                    oScanImage.AddScanChannel(oChannelBW)
                                End If
                            End If
                        End With

                        sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
                    End While
                    sHeader = sLine
                    ReadNextTag = False ' Already read the next tag
                Case Else
                    ' Not recognized tag: read as long until next tag comes
                    While Not sLine.StartsWith(":")
                        sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim
                    End While
                    sHeader = sLine
                    ReadNextTag = False ' Already read the next tag
            End Select
        Loop

        ' Parse the Record-Time
        If Not Date.TryParse(TimeString, Globalization.CultureInfo.CreateSpecificCulture("de-DE"), Globalization.DateTimeStyles.None, oScanImage.RecordDate) Then
            oScanImage.RecordDate = Now
        End If

        ' Modify the record coordinates so that they match the reference coordinate at the top left corner.
        ' The coordinates in Nanonis-files are given with respect to the CENTER of the image!!!!
        ' --> So perform a coordinate transformation to the top left corner of the image.
        With oScanImage
            Dim NewScanLocation As cNumericalMethods.Point2D = cNumericalMethods.BackCoordinateTransform(.ScanOffset_X - .ScanRange_X / 2,
                                                                                                         .ScanOffset_Y - .ScanRange_Y / 2,
                                                                                                         .ScanOffset_X, .ScanOffset_Y,
                                                                                                         -oScanImage.ScanAngle)
            .ScanOffset_X += NewScanLocation.x
            .ScanOffset_Y += NewScanLocation.y
        End With

        ' Not fetch the full data-set, if we should not ignore this section.
        If Not FetchOnlyFileHeader Then
            ' Continue Reading until the Binary Data Begins: \1A\04 (Hex)
            Dim s1 As Integer = 0
            Dim s2 As Integer = 0
            Do Until s1 = 26 And s2 = 4
                Dim s As Integer = br.Read
                s1 = s2
                s2 = s
            Loop
            ' Ascii Reading finished....

            ' Read all Channels        
            'Dim ExpectedSize As Integer = oScanImage.ScanChannels.Count * oScanImage.ScanPixels_X * oScanImage.ScanPixels_Y * 4 * 2
            'Dim RestSize As Long = br.BaseStream.Length - br.BaseStream.Position

            Dim ReadBuffer As Byte()

            ' Set Row-Fill-Direction depending on UP or DOWN Data-Accuisition-Direction
            Dim RowStartIndex As Integer
            Dim RowEndIndex As Integer
            Dim RowStep As Integer
            If RecordingDirection = "up" Then
                RowStartIndex = oScanImage.ScanPixels_Y - 1
                RowEndIndex = 0
                RowStep = -1
            Else
                RowStartIndex = 0
                RowEndIndex = oScanImage.ScanPixels_Y - 1
                RowStep = 1
            End If

            Dim ColumnStartIndex As Integer
            Dim ColumnEndIndex As Integer
            Dim ColumnStep As Integer

            ' Read the ScanImage-Data
            For Each ScanChannel As cScanImage.ScanChannel In oScanImage.ScanChannels.Values
                ' Set Column-Fill-Direction depending on FORWARD or BACKWARD Channel-Data
                If ScanChannel.ScanDirection <> cScanImage.ScanChannel.ScanDirections.Backward Then
                    ColumnStartIndex = oScanImage.ScanPixels_X - 1
                    ColumnEndIndex = 0
                    ColumnStep = -1
                Else
                    ColumnStartIndex = 0
                    ColumnEndIndex = oScanImage.ScanPixels_X - 1
                    ColumnStep = 1
                End If

                ' Create new matrix in the size of the picture
                ScanChannel.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Function(x As Integer, y As Integer) As Double
                                                                                                                                                      Return 0
                                                                                                                                                  End Function)
                For Y As Integer = RowStartIndex To RowEndIndex Step RowStep
                    ReadBuffer = br.ReadBytes(4 * oScanImage.ScanPixels_X)
                    If BitConverter.IsLittleEndian Then
                        ReadBuffer = cFileImport.ReverseBytes(ReadBuffer)
                    End If
                    For X As Integer = ColumnStartIndex To ColumnEndIndex Step ColumnStep
                        ScanChannel.ScanData(Y, X) = BitConverter.ToSingle(ReadBuffer, 4 * Math.Abs(ColumnEndIndex - X))
                    Next
                    ' The code below has been very slow!
                    'For X As Integer = ColumnStartIndex To ColumnEndIndex Step ColumnStep
                    '    ReadBuffer = br.ReadBytes(4)
                    '    If BitConverter.IsLittleEndian Then
                    '        ReadBuffer = cFileImport.ReverseBytes(ReadBuffer)
                    '    End If
                    '    ScanChannel.ScanData(Y, X) = BitConverter.ToSingle(ReadBuffer, 0)
                    'Next
                Next
            Next
        End If

        ' Finish Readers.
        br.Close()
        fs.Close()

        Return oScanImage
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_ScanImage.FileExtension
        Get
            Return ".sxm"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given File is a known Nanonis SXM File-Type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_ScanImage.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Dim sr As New StreamReader(FullFileNamePlusPath)
        ReaderBuffer = sr.ReadLine
        sr.Close()
        sr.Dispose()

        ' Nanonis-SXM File contains in first row:
        ' ":NANONIS_VERSION:"
        If ReaderBuffer.Contains(":NANONIS_VERSION:") Then
            Return True
        End If

        Return False
    End Function

End Class
