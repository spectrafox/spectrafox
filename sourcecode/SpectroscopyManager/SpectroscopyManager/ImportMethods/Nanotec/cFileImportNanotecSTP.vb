Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportNanotecSTP
    Inherits cFileImportNanotec
    Implements iFileImport_ScanImage

    ''' <summary>
    ''' Imports the ScanImage of an Nanotec image file into an ScanImage.
    ''' </summary>
    Public Function ImportSXM(ByRef FullFileNamePlusPath As String,
                              ByVal FetchOnlyFileHeader As Boolean,
                              Optional ByRef ReaderBuffer As String = "",
                              Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing) As cScanImage Implements iFileImport_ScanImage.ImportScanImage

        ' Create New ScanImage object
        Dim oScanImage As New cScanImage
        oScanImage.FullFileName = FullFileNamePlusPath

        '###################################
        ' Start to read the scan image file.

        ' Create the new scan-channel object.
        Dim ScanChannel As New cScanImage.ScanChannel

        ' Load StreamReader with the Big Endian Encoding
        Dim fs As New FileStream(FullFileNamePlusPath, FileMode.Open, FileAccess.Read, FileShare.Read)
        ' Now Using BinaryReader to obtain Image-Data
        Dim br As New BinaryReader(fs, System.Text.Encoding.Default)

        Try

            ' Buffer String
            ReaderBuffer = ""

            ' Temporary variables
            Dim SettingsName As String
            Dim SettingsValue As String
            Dim SettingsMatch As Match

            ' Channel parameters
            Dim DownScanning As Boolean
            Dim ValueTypeIsDouble As Boolean = False
            Dim ZRange As Double = 1

            ' Read settings up to the position of the end of the configuration-settings.
            Do Until ReaderBuffer.Contains("[Header end]") Or br.BaseStream.Position = br.BaseStream.Length

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
                        Case "Acquisition channel"
                            ' Get the channel name
                            ScanChannel.Name = SettingsValue

                        Case "X scanning direction"
                            ' Get the scan direction
                            If SettingsValue = "Backward" Then
                                ScanChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Backward
                                ScanChannel.Name &= " BW"
                            Else
                                ScanChannel.ScanDirection = cScanImage.ScanChannel.ScanDirections.Forward
                            End If

                        Case "Topography Bias"
                            ScanChannel.Bias = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value

                        Case "Set Point"
                            oScanImage.ZControllerSetpoint = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value
                            oScanImage.ZControllerSetpointUnit = cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Key
                        Case "Input channel"
                            oScanImage.ZControllerName = SettingsValue
                        Case "Proportional"
                            oScanImage.ZControllerProportionalGain = Double.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)
                        Case "Integral"
                            oScanImage.ZControllerIntegralGain = Double.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)

                        Case "Number of columns"
                            oScanImage.ScanPixels_X = Integer.Parse(SettingsValue)
                        Case "Number of rows"
                            oScanImage.ScanPixels_Y = Integer.Parse(SettingsValue)

                        Case "X Amplitude"
                            oScanImage.ScanRange_X = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value
                        Case "Y Amplitude"
                            oScanImage.ScanRange_Y = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value

                        Case "X Offset"
                            oScanImage.ScanOffset_X = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value
                        Case "Y Offset"
                            oScanImage.ScanOffset_Y = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value

                        Case "Angle"
                            oScanImage.ScanAngle = Double.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)

                        Case "Acquisition time"
                            If Not Date.TryParse(SettingsValue, CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, oScanImage.RecordDate) Then
                                oScanImage.RecordDate = Now
                            End If

                        Case "Y scanning direction"
                            If SettingsValue = "Down" Then
                                DownScanning = True
                            Else
                                DownScanning = False
                            End If

                        Case "Image Data Type"
                            If SettingsValue = "short" Then
                                ValueTypeIsDouble = False
                            ElseIf SettingsValue = "double" Then
                                ValueTypeIsDouble = True
                            End If

                        Case "Z Amplitude"
                            Dim Value As KeyValuePair(Of Double, String) = Me.GetKeyValuePair(SettingsValue)
                            Dim UnitFactor As KeyValuePair(Of String, Double) = cUnits.GetFactorFromPrefix(Value.Value)
                            ZRange = Value.Key * UnitFactor.Value
                            ScanChannel.UnitSymbol = UnitFactor.Key
                            ScanChannel.Unit = cUnits.GetUnitTypeFromSymbol(ScanChannel.UnitSymbol)

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


                Dim BytePerDataPoint As Integer
                If ValueTypeIsDouble Then
                    BytePerDataPoint = 8
                Else
                    BytePerDataPoint = 2
                End If
                For Y As Integer = StartY To EndY Step StepY
                    ReadBuffer = br.ReadBytes(BytePerDataPoint * oScanImage.ScanPixels_X)
                    If ReadBuffer.Length = oScanImage.ScanPixels_X * BytePerDataPoint Then
                        For X As Integer = StartX To EndX Step StepX
                            If ValueTypeIsDouble Then
                                ScanChannel.ScanData(Y, X) = BitConverter.ToDouble(ReadBuffer, BytePerDataPoint * (StartX - X))
                            Else
                                ScanChannel.ScanData(Y, X) = BitConverter.ToInt16(ReadBuffer, BytePerDataPoint * (StartX - X))
                            End If
                        Next
                    End If
                Next

                ' Convert the values to the correct units.
                ' Use the ZRange to set maximum and minimum values.
                Dim MaxValue As Double = ScanChannel.GetMaximumValue
                Dim MinValue As Double = ScanChannel.GetMinimumValue
                If MaxValue <> MinValue Then
                    Dim ValueDifference As Double = (MaxValue - MinValue)
                    Dim ConversionFactor As Double = ZRange / ValueDifference

                    ' Convert all values.
                    For Y As Integer = StartY To EndY Step StepY
                        For X As Integer = StartX To EndX Step StepX
                            ScanChannel.ScanData(Y, X) = (ScanChannel.ScanData(Y, X) - MinValue) * ConversionFactor
                        Next
                    Next
                End If


            End If


        Catch ex As Exception
            Debug.WriteLine("Error importing Nanotec image: " & ex.Message)
        Finally

            ' Finish readers for the single channel file.
            br.Close()
            fs.Close()
            br.Dispose()
            fs.Dispose()

        End Try

        ' Add the scan-channel to the scan-image
        oScanImage.AddScanChannel(ScanChannel)

        Return oScanImage
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_ScanImage.FileExtension
        Get
            Return ".stp"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known Nanotec topography file-type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_ScanImage.IdentifyFile
        ' Load StreamReader and read second line.
        ' Is the only one needed for Identification.
        Dim sr As New StreamReader(FullFileNamePlusPath)
        ReaderBuffer = sr.ReadLine
        ReaderBuffer = sr.ReadLine
        sr.Close()
        sr.Dispose()

        ' Nanotec image file contains in second row:
        ' "SxM Image file"
        If ReaderBuffer.Contains("SxM Image file") Then
            Return True
        End If

        Return False
    End Function

End Class
