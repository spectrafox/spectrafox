Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' General class which contains functions to import Nanonis files.
''' </summary>
Public Class cFileImportNanonisSpectroscopyFile
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Regex for parsing the column header.
    ''' </summary>
    Public Shared ColumnHeaderRegex As New Regex("(?<ColumnName>.*?)\s\((?<Unit>[\(\)/a-zA-Z]*?)\)", RegexOptions.Compiled)

    ''' <summary>
    ''' Imports the Spectroscopy-File into a cSpectroscopyTable-object
    ''' </summary>
    Public Function ImportSpectroscopyFile(ByRef FullFileNamePlusPath As String,
                                           ByVal FetchOnlyFileHeader As Boolean,
                                           Optional ByRef ReaderBuffer As String = "",
                                           Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                                           Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cSpectroscopyTable Implements iFileImport_SpectroscopyTable.ImportSpectroscopyTable

        ' Create new SpectroscopyTable
        Dim oSpectroscopyTable As New cSpectroscopyTable
        oSpectroscopyTable.FullFileName = FullFileNamePlusPath

        ' Load StreamReader
        Using sr As New StreamReader(FullFileNamePlusPath)

            ' Read file Line-By-Line and export settings
            ' and data. Write the splitted values in a string-array
            ReaderBuffer = ""
            Dim SplittedLine As String()

            ' Read settings up to the position of spectroscopy data in the file. Starts with [DATA].
            Do Until sr.EndOfStream Or ReaderBuffer.Contains("[DATA]")
                ReaderBuffer = sr.ReadLine

                SplittedLine = Split(ReaderBuffer, vbTab, 2)
                If SplittedLine.Length <> 2 Then Continue Do
                Dim propertyName As String = SplittedLine(0).Trim
                Dim stringValue As String = SplittedLine(1).Trim

                ' Try to parse different data formats
                Dim isFloat As Boolean = False
                Dim floatValue As Double = Nothing

                Dim isInteger As Boolean = False
                Dim integerValue As Integer = Nothing

                Dim isBool As Boolean = False
                Dim boolValue As Boolean = Nothing

                Dim isDate As Boolean = False
                Dim dateValue As Date = Nothing

                Select Case (True)
                    Case Integer.TryParse(stringValue, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, integerValue)
                        isInteger = True
                    Case Double.TryParse(stringValue, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, floatValue)
                        isFloat = True
                    Case Boolean.TryParse(stringValue, boolValue)
                        isBool = True
                    Case Date.TryParse(stringValue, Globalization.CultureInfo.CreateSpecificCulture("de-DE"), Globalization.DateTimeStyles.None, dateValue)
                        isDate = True
                End Select

                If (Not isBool) Then
                    If (isFloat) Then
                        If (floatValue = 1) Then
                            isBool = True
                            boolValue = True
                        ElseIf (floatValue = 0) Then
                            isBool = False
                            boolValue = False
                        End If
                    ElseIf (isInteger) Then
                        If (integerValue = 1) Then
                            isBool = True
                            boolValue = True
                        ElseIf (integerValue = 0) Then
                            isBool = False
                            boolValue = False
                        End If
                    End If
                End If

                With oSpectroscopyTable
                    ' Treat Comment separately:
                    If propertyName.Contains("Comment") Then
                        .Comment &= stringValue & vbCrLf
                    End If

                    ' Other Properties
                    Select Case propertyName

                        Case "Date"
                            If isDate Then
                                .RecordDate = dateValue
                            Else
                                .RecordDate = Now
                            End If

                        Case "X (m)"
                            .Location_X = floatValue
                        Case "Y (m)"
                            .Location_Y = floatValue
                        Case "Z (m)"
                            .Location_Z = floatValue

                        Case "Z offset (m)"
                            .Z_Offset = floatValue
                        Case "Z sweep distance (m)"
                            .Z_Sweep_Distance = floatValue
                        Case "Settling time (s)"
                            .SettlingTime_s = floatValue
                        Case "Integration time (s)"
                            .IntegrationTime_s = floatValue
                        Case "Z-Ctrl hold"
                            .FeedbackOff = boolValue


                        Case "Bias>Bias (V)"
                            .FeedbackOpenBias_V = floatValue
                        Case "Bias>Calibration (V/V)"
                            .Bias_Calibration_V_V = floatValue
                        Case "Bias>Offset (V)"
                            .Bias_OffSet_V = floatValue


                        Case "Bias Spectroscopy>Sweep Start (V)"
                            .BiasSpec_SweepStart_V = floatValue
                        Case "Bias Spectroscopy>Sweep End (V)"
                            .BiasSpec_SweepEnd_V = floatValue
                        Case "Bias Spectroscopy>Num Pixel"
                            .MeasurementPoints = integerValue
                        Case "Bias Spectroscopy>Z Avg time"
                            .Z_Avg_Time_s = floatValue
                        Case "Bias Spectroscopy>backward sweep"
                            .Backward_Sweep = boolValue
                        Case "Bias Spectroscopy>Number of sweeps"
                            .NumberOfSweeps = integerValue

                        Case "Current>Current (A)"
                            .Curr_Current = floatValue
                        Case "Current>Calibration (A/V)"
                            .Curr_Calibration = floatValue
                        Case "Current>Offset (A)"
                            .Curr_Offset = floatValue
                        Case "Current>Gain"
                            .Curr_Gain = stringValue

                        Case "Z Spectroscopy>Num Pixel"
                            .MeasurementPoints = integerValue
                        Case "Z Spectroscopy>Z Avg time (s)"
                            .Z_Avg_Time_s = floatValue
                        Case "Z Spectroscopy>backward sweep"
                            .Backward_Sweep = boolValue
                        Case "Z Spectroscopy>Number of sweeps"
                            .NumberOfSweeps = integerValue

                        Case "Z-Controller>Setpoint"
                            .FeedbackOpenCurrent_A = floatValue
                            .ZController_Setpoint = .FeedbackOpenCurrent_A
                        Case "Z-Controller>Setpoint unit"
                            .ZController_SetpointUnit = stringValue
                        Case "Z-Controller>Controller name"
                            .ZController_ControllerName = stringValue
                        Case "Z-Controller>Controller status"
                            .ZController_ControllerStatus = (stringValue = "OFF")
                        Case "Z-Controller>P gain"
                            .ZController_PGain = floatValue
                        Case "Z-Controller>I gain"
                            .ZController_IGain = floatValue
                        Case "Z-Controller>Time const (s)"
                            .ZController_TimeConst = floatValue
                        Case "Z-Controller>TipLift (m)"
                            .ZController_TipLift = floatValue
                        Case "Z-Controller>Switch off delay (s)"
                            .ZController_SwitchOffDelay = floatValue
                        Case "Z-Controller>Z (m)"
                            .ZController_Z = floatValue


                        Case "Oscillation Control>differential input"
                            .OscCntrl_DifferentialInput = boolValue
                        Case "Oscillation Control>input 1/10"
                            .OscCntrl_Input1To10 = boolValue
                        Case "Oscillation Control>Input Calibration (m/V)"
                            .OscCntrl_InputCalibration = floatValue
                        Case "Oscillation Control>Input Range (m)"
                            .OscCntrl_InputRange = floatValue
                        Case "Oscillation Control>Center Frequency (Hz)"
                            .OscCntrl_CenterFrequency = floatValue
                        Case "Oscillation Control>Range (Hz)"
                            .OscCntrl_Range = floatValue
                        Case "Oscillation Control>Reference Phase (deg)"
                            .OscCntrl_ReferencePhase = floatValue
                        Case "Oscillation Control>Harmonic"
                            .OscCntrl_Harmonic = integerValue
                        Case "Oscillation Control>Phase P gain (Hz/rad)"
                            .OscCntrl_PhasePGain = floatValue
                        Case "Oscillation Control>Phase I gain (Hz/rad/s)"
                            .OscCntrl_PhaseIGain = floatValue
                        Case "Oscillation Control>FrequencyShift (Hz)"
                            .OscCntrl_FrequencyShift = floatValue
                        Case "Oscillation Control>Amplitude Setpoint (m)"
                            .OscCntrl_AmplitudeSetpoint = floatValue
                        Case "Oscillation Control>Amplitude P gain (V/nm)"
                            .OscCntrl_AmplitudePGain = floatValue
                        Case "Oscillation Control>Amplitude I gain (V/nm/s)"
                            .OscCntrl_AmplitudeIGain = floatValue
                        Case "Oscillation Control>Amplitude controller on"
                            .OscCntrl_AmplitudeControllerStatus = boolValue
                        Case "Oscillation Control>output off"
                            .OscCntrl_OutputOff = boolValue
                        Case "Oscillation Control>output add"
                            .OscCntrl_OutputAdd = boolValue
                        Case "Oscillation Control>output divider"
                            .OscCntrl_OutputDivider = floatValue
                        Case "Oscillation Control>PLL-Setup Q-Factor"
                            .OscCntrl_PLLSetup_QFactor = floatValue
                        Case "Oscillation Control>PLL-Setup Demod. Bandwidth Amp (Hz)"
                            .OscCntrl_PLLSetup_DemodBandwidthAmplitude = floatValue
                        Case "Oscillation Control>PLL-Setup Demod. Bandwidth Pha (Hz)"
                            .OscCntrl_PLLSetup_DemodBandwidthPhase = floatValue
                        Case "Oscillation Control>PLL-Setup amplitude/excitation (m/V)"
                            .OscCntrl_PLLSetup_AmplitudeToExcitation = floatValue
                        Case "Oscillation Control>Excitation (V)"
                            .OscCntrl_Excitation = floatValue


                        Case "Temperature 1>Temperature 1 (K)"
                            .Temperature = floatValue

                        Case Else
                            ' Add to the general property array.
                            .AddGeneralProperty(propertyName, stringValue)
                    End Select
                End With
            Loop

            ' Create a Temporary List of DataColumns
            Dim lColumns As New List(Of cSpectroscopyTable.DataColumn)

            ' Read-Column Headers in the First Line after [DATA]-Tag
            ReaderBuffer = sr.ReadLine
            SplittedLine = Split(ReaderBuffer, vbTab)
            For i As Integer = 0 To SplittedLine.Length - 1 Step 1
                ' Add Column
                Dim oColumn As New cSpectroscopyTable.DataColumn

                ' Using Regular-Expression the unit and the Column-Name get extracted.
                Dim oMatch As Match = ColumnHeaderRegex.Match(SplittedLine(i))

                ' If the Column could be identified:
                If oMatch.Success Then
                    ' Save Column-Name
                    oColumn.Name = oMatch.Groups("ColumnName").Value
                    ' Save Column-Unit
                    oColumn.UnitSymbol = oMatch.Groups("Unit").Value
                    oColumn.UnitType = cUnits.GetUnitTypeFromSymbol(oMatch.Groups("Unit").Value)
                Else
                    Throw New Exception("Columns in File " & oSpectroscopyTable.FullFileName & " could not be identified!")
                End If
                lColumns.Add(oColumn)
            Next

            ' Add a separate Column just for the Measurement Point Number
            Dim PointColumn As New cSpectroscopyTable.DataColumn
            PointColumn.Name = My.Resources.ColumnName_MeasurementPoints
            PointColumn.UnitSymbol = "1"
            PointColumn.UnitType = cUnits.UnitType.Unitary
            lColumns.Add(PointColumn)
            Dim PointColumnIndex As Integer = lColumns.Count - 1

            If Not FetchOnlyFileHeader Then
                ' Read Spectroscopy-Data
                Dim iRowCounter As Integer = 1
                Dim ParsedDouble As Double
                Do Until sr.EndOfStream
                    ReaderBuffer = sr.ReadLine

                    SplittedLine = Split(ReaderBuffer, vbTab)

                    '## Changed: 11/13/2014
                    ' Check, if the number of columns matches the number of values...
                    ' sometimes this is not the case ... who knows why ...???
                    If lColumns.Count < SplittedLine.Length Then Exit Do

                    For i As Integer = 0 To SplittedLine.Length - 1 Step 1
                        ' Save Spectroscopy-Values.
                        If Double.TryParse(SplittedLine(i), NumberStyles.Float, CultureInfo.InvariantCulture, ParsedDouble) Then
                            lColumns(i).AddValueToColumn(ParsedDouble)
                        Else
                            lColumns(i).AddValueToColumn(0D)
                        End If
                        ' The code below has been very slow, compared to the code above!
                        'If IsNumeric(SplittedLine(i)) Then
                        '    lColumns(i).Values.Add(Double.Parse(SplittedLine(i), Globalization.CultureInfo.InvariantCulture))
                        'Else
                        '    lColumns(i).Values.Add(0D)
                        'End If
                    Next

                    lColumns(PointColumnIndex).AddValueToColumn(iRowCounter)

                    iRowCounter += 1
                Loop

                ' Check, if the number of Measurement-Points was read correctly 
                ' from the Spectroscopy-File --> this is not the case, if the user
                ' does not select some spectroscopy parameter sets to be saved!
                ' Also multi-segment-bias-spectroscopy gives the wrong number in the
                If lColumns.Count <> 0 Then
                    If lColumns(0).Values(True).Count <> oSpectroscopyTable.MeasurementPoints Then
                        oSpectroscopyTable.MeasurementPoints = lColumns(0).Values(True).Count
                    End If
                End If
            End If

            ' Finally add all Columns from the Temporary List to the Spectroscopy-Table
            For Each oColumn As cSpectroscopyTable.DataColumn In lColumns
                oColumn.IsSpectraFoxGenerated = False
                oSpectroscopyTable.AddNonPersistentColumn(oColumn)
            Next

        End Using

        ' File Exists, so Set the Property.
        oSpectroscopyTable._bFileExists = True

        Return oSpectroscopyTable
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_SpectroscopyTable.FileExtension
        Get
            Return ".dat"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known Nanonis file-type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Using sr As New StreamReader(FullFileNamePlusPath)
            ReaderBuffer = sr.ReadLine
        End Using

        ' Nanonis Bias-spectroscopy contains in first row:
        ' "Experiment	bias spectroscopy"
        ' Nanonis Z-spectroscopy contains in first row:
        ' "Experiment	Z spectroscopy"
        ' Nanonis OC4 Sweeper contains in first row:
        ' "Experiment	Frequency Sweep"
        ' Nanonis General Sweeper contains in first row:
        ' "Experiment	Sweep"
        If ReaderBuffer.StartsWith("Experiment") Then
            Return True
        End If

        Return False
    End Function

End Class

