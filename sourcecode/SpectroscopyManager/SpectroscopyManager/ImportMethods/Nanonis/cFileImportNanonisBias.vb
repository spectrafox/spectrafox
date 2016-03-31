Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportNanonisBias
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Regex for parsing the column header.
    ''' </summary>
    Private ColumnHeaderRegex As New Regex("(?<ColumnName>.*?)\s\((?<Unit>[a-zA-Z]*?)\)", RegexOptions.Compiled)

    ''' <summary>
    ''' Imports the Bias-Spectroscopy-File into a Spectroscopy Table
    ''' </summary>
    Public Function ImportBias(ByRef FullFileNamePlusPath As String,
                               ByVal FetchOnlyFileHeader As Boolean,
                               Optional ByRef ReaderBuffer As String = "",
                               Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                               Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cSpectroscopyTable Implements iFileImport_SpectroscopyTable.ImportSpectroscopyTable
        ' Create new SpectroscopyTable
        Dim oSpectroscopyTable As New cSpectroscopyTable
        oSpectroscopyTable.FullFileName = FullFileNamePlusPath

        ' Load StreamReader
        Dim sr As New StreamReader(FullFileNamePlusPath)

        ' Read File Line-By-Line and Export Settings
        ' and Data. Write the Splitted Values in a String-Array
        ReaderBuffer = ""
        'Dim ReaderBuffer As String = ""
        Dim SplittedLine As String()

        ' Read Settings up to the Position of Spectroscopy
        ' Data in the file. Starts with [DATA].
        Do Until sr.EndOfStream Or ReaderBuffer.Contains("[DATA]")
            ReaderBuffer = sr.ReadLine

            SplittedLine = Split(ReaderBuffer, vbTab, 2)
            If SplittedLine.Length <> 2 Then Continue Do
            Dim sPropertyName As String = SplittedLine(0).Trim
            Dim sPropertyValue As String = SplittedLine(1).Trim

            With oSpectroscopyTable
                ' Treat Comment separately:
                If sPropertyName.Contains("Comment") Then
                    .Comment &= sPropertyValue & vbCrLf
                End If

                ' Other Properties
                Select Case sPropertyName

                    Case "Date"
                        If Not Date.TryParse(sPropertyValue, Globalization.CultureInfo.CreateSpecificCulture("de-DE"), Globalization.DateTimeStyles.None, .RecordDate) Then
                            .RecordDate = Now
                        End If


                    Case "X (m)"
                        .Location_X = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Y (m)"
                        .Location_Y = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Z (m)"
                        .Location_Z = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)

                    Case "Z offset (m)"
                        .Z_Offset = Double.NaN
                        Double.TryParse(sPropertyValue, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, .Z_Offset)
                    Case "Settling time (s)"
                        .SettlingTime_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Integration time (s)"
                        .IntegrationTime_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Z-Ctrl hold"
                        .FeedbackOff = Boolean.Parse(sPropertyValue)


                    Case "Bias>Bias (V)"
                        .FeedbackOpenBias_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Bias>Calibration (V/V)"
                        .Bias_Calibration_V_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Bias>Offset (V)"
                        .Bias_OffSet_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)


                    Case "Bias Spectroscopy>Sweep Start (V)"
                        .BiasSpec_SweepStart_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Bias Spectroscopy>Sweep End (V)"
                        .BiasSpec_SweepEnd_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Bias Spectroscopy>Num Pixel"
                        .MeasurementPoints = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Bias Spectroscopy>Z Avg time"
                        .Z_Avg_Time_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Bias Spectroscopy>backward sweep"
                        .Backward_Sweep = Boolean.Parse(sPropertyValue)
                    Case "Bias Spectroscopy>Number of sweeps"
                        .NumberOfSweeps = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)

                    Case "Current>Current (A)"
                        .Curr_Current = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Current>Calibration (A/V)"
                        .Curr_Calibration = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Current>Offset (A)"
                        .Curr_Offset = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Current>Gain"
                        .Curr_Gain = Integer.Parse(sPropertyValue.Replace("LN 10^", ""), Globalization.CultureInfo.InvariantCulture)


                    Case "Z-Controller>Setpoint"
                        .FeedbackOpenCurrent_A = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                        .ZController_Setpoint = .FeedbackOpenCurrent_A
                    Case "Z-Controller>Setpoint unit"
                        .ZController_SetpointUnit = sPropertyValue
                    Case "Z-Controller>Controller name"
                        .ZController_ControllerName = sPropertyValue
                    Case "Z-Controller>Controller status"
                        .ZController_ControllerStatus = (sPropertyValue = "OFF")
                    Case "Z-Controller>P gain"
                        .ZController_PGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Z-Controller>I gain"
                        .ZController_IGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Z-Controller>Time const (s)"
                        .ZController_TimeConst = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Z-Controller>TipLift (m)"
                        .ZController_TipLift = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Z-Controller>Switch off delay (s)"
                        .ZController_SwitchOffDelay = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Z-Controller>Z (m)"
                        .ZController_Z = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)


                    Case "Oscillation Control>differential input"
                        .OscCntrl_DifferentialInput = Boolean.Parse(sPropertyValue)
                    Case "Oscillation Control>input 1/10"
                        .OscCntrl_Input1To10 = Boolean.Parse(sPropertyValue)
                    Case "Oscillation Control>Input Calibration (m/V)"
                        .OscCntrl_InputCalibration = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Input Range (m)"
                        .OscCntrl_InputRange = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Center Frequency (Hz)"
                        .OscCntrl_CenterFrequency = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Range (Hz)"
                        .OscCntrl_Range = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Reference Phase (deg)"
                        .OscCntrl_ReferencePhase = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Harmonic"
                        .OscCntrl_Harmonic = Int32.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Phase P gain (Hz/rad)"
                        .OscCntrl_PhasePGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Phase I gain (Hz/rad/s)"
                        .OscCntrl_PhaseIGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>FrequencyShift (Hz)"
                        .OscCntrl_FrequencyShift = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Amplitude Setpoint (m)"
                        .OscCntrl_AmplitudeSetpoint = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Amplitude P gain (V/nm)"
                        .OscCntrl_AmplitudePGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Amplitude I gain (V/nm/s)"
                        .OscCntrl_AmplitudeIGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Amplitude controller on"
                        .OscCntrl_AmplitudeControllerStatus = Boolean.Parse(sPropertyValue)
                    Case "Oscillation Control>output off"
                        .OscCntrl_OutputOff = Boolean.Parse(sPropertyValue)
                    Case "Oscillation Control>output add"
                        .OscCntrl_OutputAdd = Boolean.Parse(sPropertyValue)
                    Case "Oscillation Control>output divider"
                        .OscCntrl_OutputDivider = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>PLL-Setup Q-Factor"
                        .OscCntrl_PLLSetup_QFactor = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>PLL-Setup Demod. Bandwidth Amp (Hz)"
                        .OscCntrl_PLLSetup_DemodBandwidthAmplitude = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>PLL-Setup Demod. Bandwidth Pha (Hz)"
                        .OscCntrl_PLLSetup_DemodBandwidthPhase = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>PLL-Setup amplitude/excitation (m/V)"
                        .OscCntrl_PLLSetup_AmplitudeToExcitation = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Oscillation Control>Excitation (V)"
                        .OscCntrl_Excitation = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)


                    Case "Temperature 1>Temperature 1 (K)"
                        .Temperature = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)

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
            ' does not select "Bias-Spectroscopy-Parameters" to be saved!
            ' Also multi-segment-bias-spectroscopy gives the wrong number in the
            ' "Bias-Spectroscopy-Parameters"-Section.
            If lColumns.Count <> 0 Then
                If lColumns(0).Values(True).Count <> oSpectroscopyTable.MeasurementPoints Then
                    oSpectroscopyTable.MeasurementPoints = lColumns(0).Values(True).Count
                End If
            End If
        End If

        sr.Close()
        sr.Dispose()

        ' Finally add all Columns from the Temporary List to the Spectroscopy-Table
        For Each oColumn As cSpectroscopyTable.DataColumn In lColumns
            oColumn.IsSpectraFoxGenerated = False
            oSpectroscopyTable.AddNonPersistentColumn(oColumn)
        Next

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
    ''' Checks, if the given File is a known Nanonis File-Type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Dim sr As New StreamReader(FullFileNamePlusPath)
        ReaderBuffer = sr.ReadLine
        sr.Close()
        sr.Dispose()

        ' Nanonis-Bias File contains in first row:
        ' "Experiment	bias spectroscopy"
        If ReaderBuffer.Contains("bias spectroscopy") Then
            Return True
        End If

        Return False
    End Function

End Class
