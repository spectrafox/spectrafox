Imports System.IO
Imports System.Text.RegularExpressions

Public Class cFileImportNanonisGrid
    Implements iFileImport_GridFile

    ''' <summary>
    ''' Regex for parsing the settings key value pairs.
    ''' </summary>
    Private SettingsParser As New Regex("^(?<key>.*?)=" & Chr(34) & "?(?<value>.*?)" & Chr(34) & "?$", RegexOptions.Compiled)

    ''' <summary>
    ''' Imports the GridFile of an 3DS-File into a cGridFile-Object
    ''' </summary>
    Public Function ImportGridFile(ByRef FullFileNamePlusPath As String,
                                   ByVal FetchOnlyFileHeader As Boolean,
                                   Optional ByRef ReaderBuffer As String = "",
                                   Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                                   Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cGridFile Implements iFileImport_GridFile.ImportGridFile

        ' Create the new GridFile object.
        Dim oGridFile As New cGridFile
        oGridFile.FullFileName = FullFileNamePlusPath

        ' Load StreamReader with the Big Endian Encoding
        Using fs As New FileStream(FullFileNamePlusPath, FileMode.Open, FileAccess.Read, FileShare.Read)

            ' Now Using BinaryReader to obtain Image-Data
            Using br As New BinaryReader(fs, System.Text.Encoding.UTF8)

                ' Read File Line-By-Line and Export Settings and Data.
                Dim sLine As String = ""

                ' Buffer String
                ReaderBuffer = ""

                ' Settings-parse variables.
                Dim SettingsParserMatch As Match
                Dim SettingsKey As String = ""
                Dim SettingsValue As String = ""

                '##
                ' Import variables
                Dim ExperimentSizeInBytes As Integer = 0

                '##

                ' Read settings up to the position of the end of the configuration-settings.
                ' This end is marked by ":HEADER_END:".
                ' A setting is always given by key="value".
                Do Until sLine.Contains(":HEADER_END:") Or br.BaseStream.Position = br.BaseStream.Length

                    ' Read the next Line = First line of Settings.
                    sLine = cFileImport.ReadASCIILineFromBinaryStream(br, ReaderBuffer).Trim

                    ' Parse the settings line
                    SettingsParserMatch = SettingsParser.Match(sLine)

                    ' If the settings could not be read successfully, continue.
                    If Not SettingsParserMatch.Success Then Continue Do

                    ' Get the settings:
                    SettingsKey = SettingsParserMatch.Groups("key").Value
                    SettingsValue = SettingsParserMatch.Groups("value").Value

                    ' Read Settings
                    Select Case SettingsKey
                        Case "Comment"

                            ' new lines are given by "\0A".
                            oGridFile.Comment = SettingsValue.Replace("\0A", vbNewLine)

                        Case "Grid dim"

                            ' Dimension of the grid in number of points.
                            ' Format is 'Nx x Ny' with Nx = Number of points in x, Ny = Number of points in y.
                            Dim DimensionSplit As String() = SettingsValue.Split({"x"}, StringSplitOptions.None)
                            If DimensionSplit.Length = 2 Then
                                Dim SizeX As Integer
                                Dim SizeY As Integer
                                Integer.TryParse(DimensionSplit(0), Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, SizeX)
                                Integer.TryParse(DimensionSplit(1), Globalization.NumberStyles.Integer, Globalization.CultureInfo.InvariantCulture, SizeY)
                                oGridFile.GridDimensions = New Size(SizeX, SizeY)
                            End If

                        Case "Grid settings"

                            ' Grid position and dimension in physical units (m).
                            ' Format is 'Cx;Cy;W;H;A' with Cx = grid center x, Cy = grid center y,
                            ' W = grid width, H = grid height, A = angle.
                            ' Cx, Cy, W, H are in meters (m), A in degrees (deg).
                            Dim PositionSplit As String() = SettingsValue.Split({";"}, StringSplitOptions.None)
                            If PositionSplit.Length = 5 Then
                                Dim CenterX As Double
                                Dim CenterY As Double
                                Dim Width As Double
                                Dim Height As Double
                                Dim Angle As Double
                                Double.TryParse(PositionSplit(0), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, CenterX)
                                Double.TryParse(PositionSplit(1), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, CenterY)
                                Double.TryParse(PositionSplit(1), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, Width)
                                Double.TryParse(PositionSplit(1), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, Height)
                                Double.TryParse(PositionSplit(1), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, Angle)
                                oGridFile.GridCenterPosition = New cNumericalMethods.Point2D(CenterX, CenterY)
                                oGridFile.GridDimensionsSI = New cNumericalMethods.Point2D(Width, Height)
                                oGridFile.GridAngle = Angle
                            End If

                        Case "Sweep Signal"

                            ' Name of the sweeped parameter.
                            ' This is a string, usually a signal name followed
                            ' by its units in brackets. Example: 'Bias (V)'.
                            oGridFile.SweepSignalColumn = SettingsValue

                        Case "Fixed parameters"

                            ' List of required parameters.
                            ' These are stored at the beginning of each experiment.
                            ' Usually the fixed parameters are 'Sweep Start' and 'Sweep End', i.e. the limits of the sweep signal.
                            oGridFile.FixedParameters = SettingsValue.Split({";"}, StringSplitOptions.RemoveEmptyEntries).ToList

                        Case "Experiment parameters"

                            ' Additional parameters stored for each experiment.
                            ' These can contain the position where the experiment is taken
                            ' ('X (m)', 'Y (m)', 'Z (m)') and other paramters.
                            ' It's recommended to store at least the Z position as a parameter
                            ' as this can be used to reconstruct the topography afterwards.
                            ' e.g.: Experiment parameters="X (m);Y (m);Z (m);Z offset (m);Settling time (s);Integration time (s);Z-Ctrl hold;Final Z (m);Scan:Current (A);Scan:LockIn-X (V);Scan:Z (m);Scan:Phase (deg);Scan:Amplitude (m);Scan:Frequency Shift (Hz);Scan:Excitation (V)"
                            oGridFile.AdditionalParameters = SettingsValue.Split({";"}, StringSplitOptions.RemoveEmptyEntries).ToList


                        Case "# Parameters (4 byte)"

                            ' Total number of parameters stored with each experiment
                            ' (= number of fixed parameters + number of experiment parameters).
                            ' Each parameter is stored as a single precision floating point number using 4 bytes (big-endian).
                            Integer.TryParse(SettingsValue,
                                     Globalization.NumberStyles.Integer,
                                     Globalization.CultureInfo.InvariantCulture,
                                     oGridFile.ParametersStoredWithExperiment)

                        Case "Experiment size (bytes)"

                            ' Size of experiment data in bytes.
                            ' Each floating point number uses 4 bytes.
                            ' When acquiring 1 channel forward and backward,
                            ' 256 points, this will be 2 x 256 x 4 bytes = 2048 bytes.
                            Integer.TryParse(SettingsValue,
                                     Globalization.NumberStyles.Integer,
                                     Globalization.CultureInfo.InvariantCulture,
                                     ExperimentSizeInBytes)

                        Case "Points"

                            ' Number of acquired points in the experiment (e.g. bias spectroscopy).
                            Integer.TryParse(SettingsValue,
                                     Globalization.NumberStyles.Integer,
                                     Globalization.CultureInfo.InvariantCulture,
                                     oGridFile.PointCount)

                        Case "Channels"

                            ' Channels acquired in the experiment,
                            ' separated by semicolons (';').
                            ' When acquiring data forward and backward 2 channels will be listed.
                            ' Example: 'Current (A);Current [bwd] (A)'.
                            ' e.g.: Channels="Current [AVG] (A);LockIn-X [AVG] (V);LockIn-2nd harm. x [AVG] (V);Input 8 [AVG] (V);Bias [AVG] (V);Z [AVG] (m);Phase [AVG] (deg);Amplitude [AVG] (m);Frequency Shift [AVG] (Hz);Excitation [AVG] (V);Current [00001] (A);LockIn-X [00001] (V);LockIn-2nd harm. x [00001] (V);Input 8 [00001] (V);Bias [00001] (V);Z [00001] (m);Phase [00001] (deg);Amplitude [00001] (m);Frequency Shift [00001] (Hz);Excitation [00001] (V);Current [00002] (A);LockIn-X [00002] (V);LockIn-2nd harm. x [00002] (V);Input 8 [00002] (V);Bias [00002] (V);Z [00002] (m);Phase [00002] (deg);Amplitude [00002] (m);Frequency Shift [00002] (Hz);Excitation [00002] (V);Current [00003] (A);LockIn-X [00003] (V);LockIn-2nd harm. x [00003] (V);Input 8 [00003] (V);Bias [00003] (V);Z [00003] (m);Phase [00003] (deg);Amplitude [00003] (m);Frequency Shift [00003] (Hz);Excitation [00003] (V);Current [AVG] [bwd] (A);LockIn-X [AVG] [bwd] (V);LockIn-2nd harm. x [AVG] [bwd] (V);Input 8 [AVG] [bwd] (V);Bias [AVG] [bwd] (V);Z [AVG] [bwd] (m);Phase [AVG] [bwd] (deg);Amplitude [AVG] [bwd] (m);Frequency Shift [AVG] [bwd] (Hz);Excitation [AVG] [bwd] (V);Current [00001] [bwd] (A);LockIn-X [00001] [bwd] (V);LockIn-2nd harm. x [00001] [bwd] (V);Input 8 [00001] [bwd] (V);Bias [00001] [bwd] (V);Z [00001] [bwd] (m);Phase [00001] [bwd] (deg);Amplitude [00001] [bwd] (m);Frequency Shift [00001] [bwd] (Hz);Excitation [00001] [bwd] (V);Current [00002] [bwd] (A);LockIn-X [00002] [bwd] (V);LockIn-2nd harm. x [00002] [bwd] (V);Input 8 [00002] [bwd] (V);Bias [00002] [bwd] (V);Z [00002] [bwd] (m);Phase [00002] [bwd] (deg);Amplitude [00002] [bwd] (m);Frequency Shift [00002] [bwd] (Hz);Excitation [00002] [bwd] (V);Current [00003] [bwd] (A);LockIn-X [00003] [bwd] (V);LockIn-2nd harm. x [00003] [bwd] (V);Input 8 [00003] [bwd] (V);Bias [00003] [bwd] (V);Z [00003] [bwd] (m);Phase [00003] [bwd] (deg);Amplitude [00003] [bwd] (m);Frequency Shift [00003] [bwd] (Hz);Excitation [00003] [bwd] (V)"
                            oGridFile.ChannelsRecorded = SettingsValue.Split({";"}, StringSplitOptions.RemoveEmptyEntries).ToList

                        Case "Start time"
                            Date.TryParse(SettingsValue,
                                  Globalization.CultureInfo.CreateSpecificCulture("de-DE"),
                                  Globalization.DateTimeStyles.None,
                                  oGridFile.StartDate)

                        Case "End time"
                            Date.TryParse(SettingsValue,
                                  Globalization.CultureInfo.CreateSpecificCulture("de-DE"),
                                  Globalization.DateTimeStyles.None,
                                  oGridFile.EndDate)

                    End Select
                Loop

                ' Modify the record coordinates so that they match the reference coordinate at the top left corner.
                ' The coordinates in Nanonis-files are given with respect to the CENTER of the image!!!!
                ' --> So perform a coordinate transformation to the top left corner of the image.
                'With oScanImage
                '    Dim NewScanLocation As cNumericalMethods.Point2D = cNumericalMethods.BackCoordinateTransform(.ScanOffset_X - .ScanRange_X / 2,
                '                                                                                                 .ScanOffset_Y - .ScanRange_Y / 2,
                '                                                                                                 .ScanOffset_X, .ScanOffset_Y,
                '                                                                                                 -oScanImage.ScanAngle)
                '    .ScanOffset_X += NewScanLocation.x
                '    .ScanOffset_Y += NewScanLocation.y
                'End With

                ' Not fetch the full data-set, if we should not ignore this section.
                If Not FetchOnlyFileHeader Then

                    ' The binary data begins after the header.
                    ' All data is stored in 4 byte big endian floats with the most significant bit (MSB) first.
                    ' The experiments aren't separated, all data is written into the file continuously.
                    ' Each experiment starts with the fixed parameters,
                    ' followed by the experiment parameters and the experiment data (Channels one after the other).
                    ' The size of the experiment data is defined in the header so it's easy to read a specific experiment.
                    ' From the start of the binary data an experiment including
                    ' the fixed and experiment parameters always takes (# Parameters) * 4 + (Experiment size) bytes.

                    Dim NumberOfExperiments As Integer = oGridFile.GridDimensions.Height * oGridFile.GridDimensions.Width
                    Dim ReadBuffer(3) As Byte
                    Dim ExperimentData(4 * ExperimentSizeInBytes - 1) As Byte
                    Dim CurrentValue As Single

                    ' Read data for each experiment.
                    For ExperimentCount As Integer = 1 To NumberOfExperiments

                        ' Check, if the stream is still long enough!
                        If br.BaseStream.Length - br.BaseStream.Position < ExperimentData.Length Then Exit For

                        ' Load the full experiment into the RAM
                        ExperimentData = br.ReadBytes(ExperimentData.Length)

                        ' Go through the data, and store the values
                        For ValuePosition As Integer = 0 To ExperimentData.Length - 4 Step 4

                            ' Read four bytes.
                            ReadBuffer(0) = ExperimentData(ValuePosition)
                            ReadBuffer(1) = ExperimentData(ValuePosition + 1)
                            ReadBuffer(2) = ExperimentData(ValuePosition + 2)
                            ReadBuffer(3) = ExperimentData(ValuePosition + 3)

                            If BitConverter.IsLittleEndian Then
                                ReadBuffer = cFileImport.ReverseBytes(ReadBuffer)
                            End If

                            ' Get the corresponding value.
                            CurrentValue = BitConverter.ToSingle(ReadBuffer, 0)
                        Next

                    Next

                End If

            End Using
        End Using

        Return oGridFile
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_GridFile.FileExtension
        Get
            Return ".3ds"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known Nanonis grid file type.
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_GridFile.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Using sr As New StreamReader(FullFileNamePlusPath)
            ReaderBuffer = sr.ReadLine
        End Using

        ' Nanonis-SXM File contains in first row:
        ' ":NANONIS_VERSION:"
        If ReaderBuffer.StartsWith("Grid dim") Then
            Return True
        End If

        Return False
    End Function

End Class
