Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
''' Class that imports the Createc Scan-Image File (.dat)
''' 
''' Currently supported STMAFM-Versions: 1, 2, 3
''' 
''' Author: Mike Ruby
''' Last Change: 13.03.2012
''' </summary>
Public Class cFileImportCreatecDAT
    Implements iFileImport_ScanImage

    Private Enum STMAFM_Version
        Version2Compressed
        Version2
        Version1
    End Enum

    ''' <summary>
    ''' Imports the ScanImage of an SXM-File into an Image-Object
    ''' </summary>
    Public Function ImportDAT(ByRef FullFileNamePlusPath As String,
                              ByVal FetchOnlyFileHeader As Boolean,
                              Optional ByRef ReaderBuffer As String = "",
                              Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing) As cScanImage Implements iFileImport_ScanImage.ImportScanImage
        ' STMAFM-specific Parameters:
        Dim STMAFMVersion As STMAFM_Version

        Dim GainX As Integer = 0
        Dim GainY As Integer = 0
        Dim GainZ As Integer = 0
        Dim GainPreamplifier As Integer = 0

        Dim XPiezoConst As Double = 0
        Dim YPiezoConst As Double = 0
        Dim ZPiezoConst As Double = 0

        Dim DACToXYConversionFactor As Double = 0
        Dim DACToZConversionFactor As Double = 0

        Dim ChannelCount As Integer = 0

        ' Create New ScanImage object
        Dim oScanImage As New cScanImage
        oScanImage.FullFileName = FullFileNamePlusPath

        ' Load StreamReader with the Big Endian Encoding
        Dim fs As New FileStream(FullFileNamePlusPath, FileMode.Open, FileAccess.Read, FileShare.Read)

        ' Create a Stream-Reader for ASCII-Encoding on the FileStream
        Dim sr As New StreamReader(fs, System.Text.Encoding.ASCII)

        ' Read File Line-By-Line and Export Settings and Data.
        ReaderBuffer = ""
        'Dim ReaderBuffer As String = ""
        Dim SplittedLine As String()

        ' Read First Line
        ReaderBuffer = sr.ReadLine()

        ' DAT files contain in first row "[Paramet32]" in Version 2 and
        ' "[Parameter]" in older Versions. The newest Version 3 can
        ' compress the Files, which is marked by "[Paramco32]".
        If ReaderBuffer.Contains("[Parameter]") Then
            STMAFMVersion = STMAFM_Version.Version1
        ElseIf ReaderBuffer.Contains("[Paramet32]") Then
            STMAFMVersion = STMAFM_Version.Version2
        ElseIf ReaderBuffer.Contains("[Paramco32]") Then
            STMAFMVersion = STMAFM_Version.Version2Compressed
        Else
            ' If none of these is found return the empty Image
            Return oScanImage
        End If

        '#######################################################
        '
        ' Read Settings out of the Header, until the Header ends
        '
        '#######################################################
        Do Until fs.Position >= fs.Length Or fs.Position >= 16384
            ' Read the next Line
            ReaderBuffer = sr.ReadLine

            ' Before Splitting the parameters at "="
            ' check, if the line contains comments:
            If ReaderBuffer.StartsWith("memo:") Then
                oScanImage.Comment &= ReaderBuffer.Substring(5, ReaderBuffer.Length - 5) & vbCrLf
                Continue Do
            End If

            SplittedLine = Split(ReaderBuffer, "=", 2)
            If SplittedLine.Length <> 2 Then Continue Do
            Dim sPropertyName As String = SplittedLine(0).Trim
            Dim sPropertyValue As String = SplittedLine(1).Trim

            ' Read Settings
            ' Other Properties
            With oScanImage
                Select Case sPropertyName
                    Case "Num.X / Num.X"
                        .ScanPixels_X = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Num.Y / Num.Y"
                        .ScanPixels_Y = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "GainX / GainX"
                        GainX = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "GainY / GainY"
                        GainY = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "GainZ / GainZ"
                        GainZ = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Gainpreamp / GainPre 10^"
                        GainPreamplifier = CInt(Math.Pow(10, Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)))
                    Case "Channels / Channels"
                        ChannelCount = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Dacto[A]z"
                        DACToZConversionFactor = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Dacto[A]xy"
                        DACToXYConversionFactor = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Length x[A]"
                        .ScanRange_X = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Length y[A]"
                        .ScanRange_Y = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Scanrotoffx / OffsetX"
                        .ScanOffset_X = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Scanrotoffy / OffsetY"
                        .ScanOffset_Y = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Biasvolt[mV]"
                        .Bias = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture) * 0.01
                    Case "Current[A]"
                        .Current = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Sec/Image:"
                        .ACQ_Time = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Rotation / Rotation"
                        .ScanAngle = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "memo"
                        .Comment &= sPropertyValue & vbCrLf
                    Case "FBLogIset"
                        .ZControllerSetpoint = Double.Parse(sPropertyValue.Trim, Globalization.CultureInfo.InvariantCulture)
                        If GainPreamplifier <> 0 Then .ZControllerSetpoint = .ZControllerSetpoint / GainPreamplifier * 0.001
                        .ZControllerSetpointUnit = "A"
                    Case "FBIntegral"
                        .ZControllerIntegralGain = Double.Parse(sPropertyValue.Trim, Globalization.CultureInfo.InvariantCulture)
                    Case "FBProp"
                        .ZControllerProportionalGain = Double.Parse(sPropertyValue.Trim, Globalization.CultureInfo.InvariantCulture)
                End Select
            End With
        Loop

        ' Convert offset and range to real units and SI units as base:
        oScanImage.ScanOffset_X *= DACToXYConversionFactor * GainX * 0.0000000001
        oScanImage.ScanOffset_Y *= DACToXYConversionFactor * GainY * 0.0000000001
        oScanImage.ScanRange_X *= 0.0000000001
        oScanImage.ScanRange_Y *= 0.0000000001

        ' Modify the record coordinates so that they match the reference coordinate at the top left corner.
        ' The coordinates in CREATEC-files are given with respect to the TOP-CENTER of the image!!!!
        ' --> So perform a coordinate transformation to the top left corner of the image.
        With oScanImage
            Dim NewScanLocation As cNumericalMethods.Point2D = cNumericalMethods.BackCoordinateTransform(.ScanOffset_X - .ScanRange_X / 2,
                                                                                                         .ScanOffset_Y,
                                                                                                         .ScanOffset_X, .ScanOffset_Y,
                                                                                                         -oScanImage.ScanAngle)
            .ScanOffset_X += NewScanLocation.x
            .ScanOffset_Y += NewScanLocation.y
        End With

        '############################################################
        '
        ' Create Channels by Looping through the Number of Channels:
        ' Backward Scans are included in a separate Channel
        '
        '############################################################
        For i As Integer = 1 To ChannelCount Step 1
            ' Create and Save new Channels
            Dim oChannel As New cScanImage.ScanChannel
            With oChannel
                oChannel.Name = "Channel " & i.ToString("N0")
                oChannel.UnitSymbol = cUnits.GetUnitSymbolFromType(cUnits.UnitType.Unknown)
                oChannel.Unit = cUnits.UnitType.Unknown
                oChannel.Calibration = 0
                oChannel.Offset = 0
                oChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Forward
                oChannel.IsSpectraFoxGenerated = False
                ' Add Channel
                oScanImage.AddScanChannel(oChannel)
            End With
        Next

        ' Get the Record-Time from the Last File Write Time
        oScanImage.RecordDate = IO.File.GetLastWriteTime(FullFileNamePlusPath)

        '########################################
        '
        ' Preparations for Binary-Data-Read-In:
        '
        '########################################
        ' The Createc Header Sections ends with "DATA" after (128 * 128) = 16384 bytes.
        ' Depending on the Data-Type, 2 or 4 unused bytes follow directly.
        ' The Data-Type depends on the STMAFM-Version. The Offset to Skip the Header
        ' and the Data-Type is now determined:
        Dim BytePerPixel As Short = 0

        If STMAFMVersion = STMAFM_Version.Version1 Then
            BytePerPixel = 2
            ' Header + 2 unused "NULL"-Bytes
            fs.Seek(2, SeekOrigin.Current)
        ElseIf STMAFMVersion = STMAFM_Version.Version2 Then
            BytePerPixel = 4
            ' Header + 4 unused "NULL"-Bytes
            fs.Seek(4, SeekOrigin.Current)
        ElseIf STMAFMVersion = STMAFM_Version.Version2Compressed Then
            BytePerPixel = 4
            ' No Seek of additional bytes, since they are compressed:
            'fs.Seek(0, SeekOrigin.Current)
        Else
            ' Nothing -> Case is already catched by returning an empty image.
        End If

        If Not FetchOnlyFileHeader Then
            ' Create a new BinaryReader for the File, to read the data in UTF8 encoding
            Dim br As BinaryReader = New BinaryReader(fs, System.Text.Encoding.UTF8)

            Dim ScanDataStream As Stream
            Dim ReadBuffer As Byte() = New Byte(BytePerPixel * oScanImage.ScanPixels_X) {}
            ' PERFORMANCE OPTIMIZATION: increased buffer!
            'Dim ReadBuffer As Byte() = New Byte(3) {}
            ' If the Data is Compressed, create an Unzip-Stream mirroring the FileStream.
            If STMAFMVersion = STMAFM_Version.Version2Compressed Then
                Try
                    ScanDataStream = New Ionic.Zlib.ZlibStream(fs, Ionic.Zlib.CompressionMode.Decompress, False)
                Catch ex As Exception
                    Throw ex
                End Try
                ' Skip the first 4 bytes of Null-Data in the uncompressed Stream
                ScanDataStream.Read(ReadBuffer, 0, 4)
            Else
                ' Every other version has non-compressed files,
                ' so the Byte offset is already skipped.
                ' Just get the FileStream as Stream-Object.
                ScanDataStream = fs
            End If

            For Each ScanChannel As cScanImage.ScanChannel In oScanImage.ScanChannels.Values
                ' Create New Matrix in the Size of the Picture
                ScanChannel.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Function(x As Integer, y As Integer) As Double
                                                                                                                                                      Return 0
                                                                                                                                                  End Function)
                For Y As Integer = 0 To oScanImage.ScanPixels_Y - 1 Step 1
                    ScanDataStream.Read(ReadBuffer, 0, BytePerPixel * oScanImage.ScanPixels_X)
                    For X As Integer = 0 To oScanImage.ScanPixels_X - 1 Step 1
                        If BytePerPixel = 4 Then
                            ' Interpret 4 Byte Float from the Data
                            ScanChannel.ScanData(Y, X) = BitConverter.ToSingle(ReadBuffer, BytePerPixel * X)
                        ElseIf BytePerPixel = 2 Then
                            ' Interpret 2 Byte Integer from the Data
                            ScanChannel.ScanData(Y, X) = BitConverter.ToInt32(ReadBuffer, BytePerPixel * X)
                        End If
                        If ScanChannel.ScanData(Y, X) = 0 Or Double.IsInfinity(ScanChannel.ScanData(Y, X)) Then ScanChannel.ScanData(Y, X) = Double.NaN
                    Next
                    ' PERFORMANCE OPTIMIZATION: Read whole line at once with bigger buffer!
                    'For X As Integer = 0 To oScanImage.ScanPixels_X - 1 Step 1
                    '    If BytePerPixel = 4 Then
                    '        ' Read 4 Byte Float from the Data
                    '        ScanDataStream.Read(ReadBuffer, 0, 4)
                    '        ScanChannel.ScanData(Y, X) = BitConverter.ToSingle(ReadBuffer, 0)
                    '        If ScanChannel.ScanData(Y, X) = 0 Or Double.IsInfinity(ScanChannel.ScanData(Y, X)) Then ScanChannel.ScanData(Y, X) = Double.NaN
                    '    ElseIf BytePerPixel = 2 Then
                    '        ' Read 2 Byte Integer from the Data
                    '        ScanDataStream.Read(ReadBuffer, 0, 2)
                    '        ScanChannel.ScanData(Y, X) = Convert.ToDouble(BitConverter.ToInt32(ReadBuffer, 0))
                    '        If ScanChannel.ScanData(Y, X) = 0 Or Double.IsInfinity(ScanChannel.ScanData(Y, X)) Then ScanChannel.ScanData(Y, X) = Double.NaN
                    '    End If
                    'Next
                Next
            Next
            ScanDataStream.Close()
            br.Close()
            ScanDataStream.Dispose()
            br.Dispose()
        End If

        ' Close the FileStream and Binary Reader:
        sr.Close()
        sr.Dispose()
        fs.Dispose()

        ' Adapt Scan-Offset Coordinate to the top-left corner of the picture
        ' Createc always has the reference coordinate in the top-center of the scan-frame
        'oScanImage.ScanOffset_X -= oScanImage.ScanRange_X / 2
        'oScanImage.ScanOffset_Y -= oScanImage.ScanRange_Y / 2

        Return oScanImage
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_ScanImage.FileExtension
        Get
            Return ".dat"
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

        ' VERT files contain in first row "[ParVERT30]" or "[Paramet32]" in Version 3 and
        ' "[Parameter]" in older Versions
        If ReaderBuffer.Contains("[ParVERT30]") Or
           ReaderBuffer.Contains("[Parameter]") Or
           ReaderBuffer.Contains("[Paramet32]") Or
           ReaderBuffer.Contains("[Paramco32]") Then
            Return True
        End If

        Return False
    End Function
End Class