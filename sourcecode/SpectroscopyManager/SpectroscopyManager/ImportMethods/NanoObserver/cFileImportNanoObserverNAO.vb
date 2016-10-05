Imports System.IO
Imports System.Text.RegularExpressions
Imports Ionic.Zip

''' <summary>
''' NanoObserver NAO scan image files.
''' </summary>
Public Class cFileImportNanoObserverNAO
    Implements iFileImport_ScanImage

    ''' <summary>
    ''' Regex for parsing the scan channel names.
    ''' </summary>
    Private ScanChannelHeaderRegex As New Regex("(?<Number>\d*)\s*(?<Name>[\[\]\%_\w]*)\s*(?<Unit>\w*)\s*(?<Direction>\w*)\s*(?<Calibration>[+\-]?\d*?\.\d*?E[+\-]\d*)\s*(?<Offset>[+\-]?\d*?\.\d*?E[+\-]\d*)", RegexOptions.Compiled)

    ''' <summary>
    ''' Imports the ScanImage of a NAO-File into an Image-Object
    ''' </summary>
    Public Function ImportSXM(ByRef FullFileNamePlusPath As String,
                              ByVal FetchOnlyFileHeader As Boolean,
                              Optional ByRef ReaderBuffer As String = "",
                              Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                              Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cScanImage Implements iFileImport_ScanImage.ImportScanImage

        ' Create New ScanImage object
        Dim oScanImage As New cScanImage
        oScanImage.FullFileName = FullFileNamePlusPath

        ' Stores the name and unit.
        Dim lExpectedChannels As New Dictionary(Of String, cUnits.UnitType)

        ' Unzip the files into memory
        Using DataContainer As ZipFile = ZipFile.Read(FullFileNamePlusPath)

            ' Go through all files, and first extract the header file.
            ' It is called "Measure.xml".
            For Each ZippedObject As ZipEntry In DataContainer

                ' Get the measurement informations
                If ZippedObject.FileName = "Scan/Measure.xml" Then

                    ' Set the recorddate to the modified time
                    oScanImage.RecordDate = ZippedObject.ModifiedTime

                    ' Extract into a MemoryStream to read.
                    Using MS As New MemoryStream
                        ZippedObject.Extract(MS)

                        Dim XMLSettings As New Xml.XmlReaderSettings()
                        XMLSettings.ConformanceLevel = Xml.ConformanceLevel.Fragment

                        ' Parse the header file as XML
                        MS.Seek(0, SeekOrigin.Begin)
                        Dim S As String = System.Text.Encoding.UTF8.GetString(cCompression.CopyStreamToByte(MS))

                        Dim _byteOrderMarkUtf8 As String = Text.Encoding.UTF8.GetString(Text.Encoding.UTF8.GetPreamble())
                        If S.StartsWith(_byteOrderMarkUtf8) Then
                            S = S.Remove(0, _byteOrderMarkUtf8.Length)
                        End If

                        Dim XMLDefinitionEnd As Integer = S.IndexOf(CChar(">")) + 1
                        S = S.Insert(XMLDefinitionEnd, vbCrLf & "<root>")
                        S = S & vbCrLf & "</root>"

                        Using SC As New MemoryStream(System.Text.Encoding.UTF8.GetBytes(S))
                            Using XR As Xml.XmlReader = New Xml.XmlTextReader(SC)
                                With XR

                                    ' Read as long as we can.
                                    Do While .Read Or Not .EOF

                                        ' Check for the type of data
                                        Select Case .NodeType

                                            Case Xml.XmlNodeType.Element
                                                ' An element comes: this is what we are looking for!
                                                '####################################################
                                                Select Case .Name
                                                    Case "X"
                                                        oScanImage.ScanOffset_X = Convert.ToDouble(.ReadElementContentAsString, Globalization.CultureInfo.InvariantCulture)
                                                    Case "Y"
                                                        oScanImage.ScanOffset_Y = Convert.ToDouble(.ReadElementContentAsString, Globalization.CultureInfo.InvariantCulture)
                                                    Case "Angle"
                                                        oScanImage.ScanAngle = Convert.ToDouble(.ReadElementContentAsString, Globalization.CultureInfo.InvariantCulture)

                                                    Case "Resolution"
                                                        ' number of pixels
                                                        Dim XYString As String() = .ReadElementContentAsString.Split(CChar(","))
                                                        If XYString.Length = 2 Then
                                                            If IsNumeric(XYString(0)) AndAlso IsNumeric(XYString(1)) Then
                                                                oScanImage.ScanPixels_X = Convert.ToInt32(XYString(0).Trim, Globalization.CultureInfo.InvariantCulture)
                                                                oScanImage.ScanPixels_Y = Convert.ToInt32(XYString(1).Trim, Globalization.CultureInfo.InvariantCulture)
                                                            End If
                                                        End If

                                                    Case "Size"
                                                        ' Measurement dimension
                                                        Dim XYString As String() = .ReadElementContentAsString.Split(CChar(","))
                                                        If XYString.Length = 2 Then
                                                            If IsNumeric(XYString(0)) AndAlso IsNumeric(XYString(1)) Then
                                                                oScanImage.ScanRange_X = Convert.ToDouble(XYString(0).Trim, Globalization.CultureInfo.InvariantCulture)
                                                                oScanImage.ScanRange_Y = Convert.ToDouble(XYString(1).Trim, Globalization.CultureInfo.InvariantCulture)
                                                            End If
                                                        End If

                                                    Case "Stream"

                                                        ' here comes an image
                                                        If .AttributeCount > 0 Then

                                                            Dim ChannelID As String = ""
                                                            Dim ChannelUnit As cUnits.UnitType = cUnits.UnitType.Unknown
                                                            While .MoveToNextAttribute
                                                                Select Case .Name
                                                                    Case "Id"
                                                                        ChannelID = .Value
                                                                    Case "Unit"
                                                                        ChannelUnit = cUnits.GetUnitTypeFromSymbol(.Value)
                                                                End Select
                                                            End While

                                                            lExpectedChannels.Add(ChannelID, ChannelUnit)

                                                        End If

                                                    Case "PidI"
                                                        oScanImage.ZControllerIntegralGain = Convert.ToDouble(.ReadElementContentAsString, Globalization.CultureInfo.InvariantCulture)

                                                    Case "PidP"
                                                        oScanImage.ZControllerProportionalGain = Convert.ToDouble(.ReadElementContentAsString, Globalization.CultureInfo.InvariantCulture)

                                                    Case "SampBiasDC"
                                                        oScanImage.Bias = Convert.ToDouble(.ReadElementContentAsString, Globalization.CultureInfo.InvariantCulture)

                                                    Case "SetPoint"
                                                        oScanImage.ZControllerSetpoint = Convert.ToDouble(.ReadElementContentAsString, Globalization.CultureInfo.InvariantCulture)

                                                End Select

                                        End Select


                                    Loop

                                End With

                            End Using
                        End Using
                    End Using

                    Exit For
                End If
            Next

            '' Modify the record coordinates so that they match the reference coordinate at the top left corner.
            '' The coordinates in Nanonis-files are given with respect to the CENTER of the image!!!!
            '' --> So perform a coordinate transformation to the top left corner of the image.
            'With oScanImage
            '    Dim NewScanLocation As cNumericalMethods.Point2D = cNumericalMethods.BackCoordinateTransform(.ScanOffset_X - .ScanRange_X / 2,
            '                                                                                         .ScanOffset_Y - .ScanRange_Y / 2,
            '                                                                                         .ScanOffset_X, .ScanOffset_Y,
            '                                                                                         -oScanImage.ScanAngle)
            '    .ScanOffset_X += NewScanLocation.x
            '    .ScanOffset_Y += NewScanLocation.y
            'End With

            ' Not fetch the full data-set, if we should not ignore this section.
            If Not FetchOnlyFileHeader Then
                ' Now extract all data files.
                For Each ZippedObject As ZipEntry In DataContainer

                    Dim FileName As String = ZippedObject.FileName

                    ' The data files end with ".dat".
                    If FileName.EndsWith(".dat") Then

                        ' Extract into a MemoryStream to read.
                        Using MS As New MemoryStream
                            ZippedObject.Extract(MS)

                            MS.Seek(0, SeekOrigin.Begin)
                            ' Now use a BinaryReader to obtain the image data
                            Using br As New BinaryReader(MS, System.Text.Encoding.UTF8)

                                ' Get a buffer.
                                Dim ReadBuffer As Byte()

                                ' Create a new scan-channel
                                Dim ScanChannel As New cScanImage.ScanChannel

                                ' Decide between forward and backward.
                                If FileName.EndsWith("_Right.dat") Then
                                    ScanChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Backward
                                    ScanChannel.Name = FileName.Replace("_Right.dat", " BW")
                                Else
                                    ScanChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Forward
                                    ScanChannel.Name = FileName.Replace("_Left.dat", " FW")
                                End If

                                ' Set the unit type extracted before.
                                If lExpectedChannels.ContainsKey(ScanChannel.Name) Then
                                    ScanChannel.Unit = lExpectedChannels(ScanChannel.Name)
                                    ScanChannel.UnitSymbol = cUnits.GetUnitSymbolFromType(ScanChannel.Unit)
                                End If

                                ' Set Row-Fill-Direction depending on UP or DOWN Data-Accuisition-Direction
                                Dim RowStartIndex As Integer
                                Dim RowEndIndex As Integer
                                Dim RowStep As Integer
                                If 1 = 1 Then
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

                                ' Set Column-Fill-Direction depending on FORWARD or BACKWARD Channel-Data
                                If 1 <> 1 Then  ' ScanChannel.ScanDirection <> cScanImage.ScanChannel.ScanDirections.Backward Then
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
                                    For X As Integer = ColumnStartIndex To ColumnEndIndex Step ColumnStep
                                        ScanChannel.ScanData(Y, X) = BitConverter.ToSingle(ReadBuffer, 4 * Math.Abs(ColumnEndIndex - X))
                                    Next
                                Next

                                ' Add the scan channel to the image
                                oScanImage.AddScanChannel(ScanChannel)
                            End Using
                        End Using
                    End If
                Next
            End If


        End Using

        Return oScanImage
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_ScanImage.FileExtension
        Get
            Return ".nao"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known NanoObserver filetype.
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_ScanImage.IdentifyFile
        ' it is a zip file, so NAO may be the only possibility
        Return True
    End Function

End Class
