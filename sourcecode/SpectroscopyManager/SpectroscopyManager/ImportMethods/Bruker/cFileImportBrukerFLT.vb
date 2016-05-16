Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions

Public Class cFileImportBrukerFLT
    Inherits cFileImportBruker
    Implements iFileImport_ScanImage

    ''' <summary>
    ''' Regular expression to extract the file name from the file extension.
    ''' </summary>
    Private ImageFileNameRegex As New Regex("^(?<BaseName>.*)\.(?<SignalName>.*)\.FLT$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Imports the ScanImage from the FLT file into an cScanImage-object
    ''' </summary>
    Public Function ImportImage(ByRef FullFileNamePlusPath As String,
                                ByVal FetchOnlyFileHeader As Boolean,
                                Optional ByRef ReaderBuffer As String = "",
                                Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                                Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cScanImage Implements iFileImport_ScanImage.ImportScanImage

        ' Check, if the ignore list exists
        If FilesToIgnoreAfterThisImport Is Nothing Then FilesToIgnoreAfterThisImport = New List(Of String)

        ' Get basic properties of the file:
        Dim BaseName As String = String.Empty

        Dim FileName As String = IO.Path.GetFileName(FullFileNamePlusPath)
        Dim FileNameMatch As Match = ImageFileNameRegex.Match(FileName)
        If FileNameMatch.Success Then
            If FileNameMatch.Groups.Count >= 1 Then
                BaseName = FileNameMatch.Groups.Item("BaseName").Value
            End If
        Else
            Return Nothing
        End If

        ' Nanotec image files consist out of a separate file for forward and backward,
        ' and a separate file for each channel. This is given by the file extension.
        Dim ListOfAllFilesForThisImage As New List(Of String)

        Dim FileDirectory As New DirectoryInfo(IO.Path.GetDirectoryName(FullFileNamePlusPath))
        Dim FileListInDirectory As FileInfo() = FileDirectory.GetFiles()
        For Each oFile As FileInfo In FileListInDirectory

            Dim OtherChannelFiles As Match = ImageFileNameRegex.Match(oFile.Name)
            If OtherChannelFiles.Success Then
                If OtherChannelFiles.Groups.Count >= 1 Then
                    If OtherChannelFiles.Groups.Item("BaseName").Value = BaseName Then
                        ListOfAllFilesForThisImage.Add(oFile.FullName)
                        FilesToIgnoreAfterThisImport.Add(oFile.FullName)
                    End If
                End If
            End If

        Next

        ' Create New ScanImage object
        Dim oScanImage As New cScanImage
        oScanImage.FullFileName = FullFileNamePlusPath
        oScanImage.DisplayName = BaseName

        ' Start to read ALL files of this scan image.
        ' Each file contains a single scan channel.
        For Each DataFile As String In ListOfAllFilesForThisImage

            ' Create the new scan-channel object.
            Dim ScanChannel As New cScanImage.ScanChannel
            ScanChannel.IsSpectraFoxGenerated = False

            ' Load StreamReader with the Big Endian Encoding
            Using fs As New FileStream(DataFile, FileMode.Open, FileAccess.Read, FileShare.Read)
                ' Now Using BinaryReader to obtain Image-Data
                Using br As New BinaryReader(fs, System.Text.Encoding.Default)

                    ' Buffer String
                    ReaderBuffer = ""

                    ' Temporary variables
                    Dim SettingsName As String
                    Dim SettingsValue As String
                    Dim SettingsMatch As Match

                    ' Data variables
                    Dim ZTransferCoefficient As Double = 1

                    ' Read settings up to the position of the end of the configuration-settings.
                    Do Until ReaderBuffer.Contains("[Data]") Or br.BaseStream.Position = br.BaseStream.Length

                        ' Read the settings line.
                        ReaderBuffer = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim

                        ' Split the settings.
                        SettingsMatch = SettingsRegex.Match(ReaderBuffer)

                        ' Only consider settings with two splitted values:
                        If SettingsMatch.Success Then

                            ' Get the setting-name
                            SettingsName = SettingsMatch.Groups("name").Value
                            SettingsValue = SettingsMatch.Groups("value").Value

                            ' Read Settings
                            Select Case SettingsName
                                Case "DataName"
                                    ' Get the channel name
                                    ScanChannel.Name = SettingsValue

                                Case "ScanDirection"
                                    ' Get the scan direction
                                    If SettingsValue = "Backward" Then
                                        ScanChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Backward
                                        ScanChannel.Name &= " BW"
                                    Else
                                        ScanChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Forward
                                    End If

                                Case "CreationTime"
                                    If Not Date.TryParse(SettingsValue, CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, oScanImage.RecordDate) Then
                                        oScanImage.RecordDate = Now
                                    End If


                                Case "ResolutionX"
                                    oScanImage.ScanPixels_X = Integer.Parse(SettingsValue)
                                Case "ResolutionY"
                                    oScanImage.ScanPixels_Y = Integer.Parse(SettingsValue)

                                Case "ScanRangeX"
                                    oScanImage.ScanRange_X = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value
                                Case "ScanRangeY"
                                    oScanImage.ScanRange_Y = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value

                                Case "OffsetX"
                                    oScanImage.ScanOffset_X = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value
                                Case "OffsetY"
                                    oScanImage.ScanOffset_Y = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value

                                Case "Rotation"
                                    oScanImage.ScanAngle = Me.GetKeyValuePair(SettingsValue).Key

                                Case "SetPoint"
                                    oScanImage.ZControllerSetpoint = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value
                                    oScanImage.ZControllerSetpointUnit = cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Key

                                Case "Mode"
                                    oScanImage.ZControllerName = SettingsValue

                                Case "ZTransferCoefficient"
                                    ZTransferCoefficient = Me.GetKeyValuePair(SettingsValue).Key

                                    ' Get the channel unit:
                                    Dim Unit1 As String = cUnits.GetUnitVsUnitFromString(Me.GetKeyValuePair(SettingsValue).Value).Item1
                                    Dim UnitFactorKeyValuePair As KeyValuePair(Of String, Double) = cUnits.GetFactorFromPrefix(Unit1)
                                    Unit1 = UnitFactorKeyValuePair.Key
                                    ZTransferCoefficient *= UnitFactorKeyValuePair.Value

                                    ScanChannel.Unit = cUnits.GetUnitTypeFromSymbol(Unit1)
                                    ScanChannel.UnitSymbol = Unit1

                                Case Else
                                    ' Add to the general property array.
                                    oScanImage.AddGeneralProperty(SettingsName, SettingsValue)

                            End Select

                        End If

                    Loop

                    ' Not fetch the full data-set, if we should not ignore this section.
                    If Not FetchOnlyFileHeader Then

                        Dim ReadBuffer As Byte()

                        ' Create new matrix in the size of the picture
                        ScanChannel.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, 0)

                        ' Determine, if the picture is built up from bottom or top.
                        Dim StartY As Integer = oScanImage.ScanPixels_Y - 1
                        Dim EndY As Integer = 0
                        Dim StepY As Integer = -1
                        Dim StartX As Integer = oScanImage.ScanPixels_X - 1
                        Dim EndX As Integer = 0
                        Dim StepX As Integer = -1


                        Dim BytePerDataPoint As Integer = 4
                        For Y As Integer = StartY To EndY Step StepY
                            ReadBuffer = br.ReadBytes(BytePerDataPoint * oScanImage.ScanPixels_X)
                            If ReadBuffer.Length = oScanImage.ScanPixels_X * BytePerDataPoint Then
                                For X As Integer = StartX To EndX Step StepX
                                    ScanChannel.ScanData(Y, X) = BitConverter.ToSingle(ReadBuffer, BytePerDataPoint * (StartX - X)) * ZTransferCoefficient
                                Next
                            End If
                        Next

                        '' Convert the values to the correct units.
                        '' Use the ZRange to set maximum and minimum values.
                        'Dim MaxValue As Double = ScanChannel.GetMaximumValue
                        'Dim MinValue As Double = ScanChannel.GetMinimumValue
                        'If MaxValue <> MinValue Then
                        '    Dim ValueDifference As Double = (MaxValue - MinValue)
                        '    Dim ConversionFactor As Double = ZRange / ValueDifference

                        '    ' Convert all values.
                        '    For Y As Integer = StartY To EndY Step StepY
                        '        For X As Integer = StartX To EndX Step StepX
                        '            ScanChannel.ScanData(Y, X) = (ScanChannel.ScanData(Y, X) - MinValue) * ConversionFactor
                        '        Next
                        '    Next
                        'End If

                    End If

                End Using
            End Using

            ' Add the scan-channel to the scan-image
            oScanImage.AddScanChannel(ScanChannel)

        Next

        Return oScanImage
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_ScanImage.FileExtension
        Get
            Return ".flt"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a BRUKER FLT file.
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_ScanImage.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Using sr As New StreamReader(FullFileNamePlusPath)
            ReaderBuffer = sr.ReadLine
        End Using

        ' Bruker-FLT files contains in first row:
        ' "[Data Version]"
        If ReaderBuffer.Contains("[Data Version]") Then
            Return True
        End If

        Return False
    End Function

End Class
