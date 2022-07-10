'Imports System.IO
'Imports System.Text.RegularExpressions
'Imports System.Globalization

'''' <summary>
'''' General class which contains functions to import Nanonis files.
'''' </summary>
'Public Class cFileImportGSXM
'    Implements iFileImport_SpectroscopyTable

'    ''' <summary>
'    ''' Regex for parsing the column header.
'    ''' </summary>
'    Public Shared ColumnHeaderRegex As New Regex("(?<ColumnName>.*?)\s\((?<Unit>[\(\)/a-zA-Z]*?)\)", RegexOptions.Compiled)

'    Public Shared HeaderValueParserRegex As New Regex("(?<Variable>.*?)=(?<Value>[\-\+\d\.]*?) (?<Unit>\w*?)", RegexOptions.Compiled)


'    ''' <summary>
'    ''' Imports the Spectroscopy-File into a cSpectroscopyTable-object
'    ''' </summary>
'    Public Function ImportSpectroscopyFile(ByRef FullFileNamePlusPath As String,
'                                           ByVal FetchOnlyFileHeader As Boolean,
'                                           Optional ByRef ReaderBuffer As String = "",
'                                           Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
'                                           Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cSpectroscopyTable Implements iFileImport_SpectroscopyTable.ImportSpectroscopyTable

'        ' Create new SpectroscopyTable
'        Dim oSpectroscopyTable As New cSpectroscopyTable
'        oSpectroscopyTable.FullFileName = FullFileNamePlusPath

'        ' Load StreamReader
'        Using sr As New StreamReader(FullFileNamePlusPath)

'            ' Read file Line-By-Line and export settings and data.
'            ' Write the splitted values in a string-array.
'            ReaderBuffer = "#"
'            Dim SplittedLine As String()

'            Do Until sr.EndOfStream
'                ReaderBuffer = sr.ReadLine

'                ' COLUMN information
'                If ReaderBuffer.StartsWith("#C Full Position Vector List") Then
'                    ' Create a Temporary List of DataColumns
'                    Dim lColumns As New List(Of cSpectroscopyTable.DataColumn)

'                    ' Read-Column Headers in the First Line after [DATA]-Tag
'                    ReaderBuffer = sr.ReadLine
'                    SplittedLine = Split(ReaderBuffer, vbTab)
'                    For i As Integer = 0 To SplittedLine.Length - 1 Step 1
'                        ' Add Column
'                        Dim oColumn As New cSpectroscopyTable.DataColumn

'                        ' Using Regular-Expression the unit and the Column-Name get extracted.
'                        Dim oMatch As Match = ColumnHeaderRegex.Match(SplittedLine(i))

'                        ' If the Column could be identified:
'                        If oMatch.Success Then
'                            ' Save Column-Name
'                            oColumn.Name = oMatch.Groups("ColumnName").Value
'                            ' Save Column-Unit
'                            oColumn.UnitSymbol = oMatch.Groups("Unit").Value
'                            oColumn.UnitType = cUnits.GetUnitTypeFromSymbol(oMatch.Groups("Unit").Value)
'                        Else
'                            Throw New Exception("Columns in File " & oSpectroscopyTable.FullFileName & " could not be identified!")
'                        End If
'                        lColumns.Add(oColumn)
'                    Next

'                    ' Add a separate Column just for the Measurement Point Number
'                    Dim PointColumn As New cSpectroscopyTable.DataColumn
'                    PointColumn.Name = My.Resources.ColumnName_MeasurementPoints
'                    PointColumn.UnitSymbol = "1"
'                    PointColumn.UnitType = cUnits.UnitType.Unitary
'                    lColumns.Add(PointColumn)
'                    Dim PointColumnIndex As Integer = lColumns.Count - 1
'                End If

'                ' DATA SECTION
'                If ReaderBuffer.StartsWith("#C Index") Then

'                    ' The current line starting with #C Index contains the column header names
'                    Do Until sr.EndOfStream Or ReaderBuffer.StartsWith("#C")
'                        ReaderBuffer = sr.ReadLine


'                    Loop
'                End If

'                ' strip the "#" character at the beginning of each line
'                ReaderBuffer = ReaderBuffer.Substring(1)

'                ' split property name from the content section
'                SplittedLine = Split(ReaderBuffer, "::", 2)
'                If SplittedLine.Length <> 2 Then Continue Do
'                Dim sPropertyName As String = SplittedLine(0).Trim
'                Dim sPropertyValue As String = SplittedLine(1).Trim

'                ' Global variables
'                Dim GlobalOffsetX As Double = 0
'                Dim GlobalOffsetY As Double = 0

'                With oSpectroscopyTable
'                    ' Treat Comment separately:
'                    If sPropertyName.Contains("GXSM-Main-Comment") Then
'                        ' strip the first line's variable name
'                        .Comment &= sPropertyValue.Substring("comment=".Length) & vbCrLf

'                        ' read the comment up to the next line starting with "#", which would be the next header line
'                        Do Until sr.EndOfStream Or ReaderBuffer.StartsWith("#")
'                            ReaderBuffer = sr.ReadLine
'                            .Comment &= sPropertyValue & vbCrLf
'                        Loop

'                        ' remove the last character, which terminates the comment
'                        .Comment = .Comment.Substring(0, .Comment.Length - 2)
'                    End If

'                    ' Other Properties
'                    Select Case sPropertyName

'                        Case "Probe Data Number"
'                            .MeasurementPoints = Integer.Parse(sPropertyValue.Substring("N=".Length), Globalization.CultureInfo.InvariantCulture)

'                        Case "Date"
'                            '# Date                   :: date=Thu Apr 28 16:19:02 2022
'                            If Not Date.TryParse(sPropertyValue.Substring("date=".Length), Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, .RecordDate) Then
'                                .RecordDate = Now
'                            End If

'                        Case "GXSM-Main-Offset"
'                            ' the main offset has to be added to each spectrum location to obtain the actual spectrum position
'                            ' # GXSM-Main-Offset       :: X0=-2290.65 Ang  Y0=-1821.24 Ang, iX0=373 Pix iX0=454 Pix
'                            Dim oMatch As Match = HeaderValueParserRegex.Match(sPropertyValue)

'                            ' If the Column could be identified:
'                            If oMatch.Success Then
'                                Console.WriteLine(" match")
'                            End If

'                        Case "X (m)"
'                            .Location_X = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Y (m)"
'                            .Location_Y = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z (m)"
'                            .Location_Z = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)

'                        Case "Z offset (m)"
'                            .Z_Offset = Double.NaN
'                            Double.TryParse(sPropertyValue, Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, .Z_Offset)
'                        Case "Z sweep distance (m)"
'                            .Z_Sweep_Distance = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Settling time (s)"
'                            .SettlingTime_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Integration time (s)"
'                            .IntegrationTime_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z-Ctrl hold"
'                            .FeedbackOff = Boolean.Parse(sPropertyValue)


'                        Case "Bias>Bias (V)"
'                            .FeedbackOpenBias_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Bias>Calibration (V/V)"
'                            .Bias_Calibration_V_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Bias>Offset (V)"
'                            .Bias_OffSet_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)


'                        Case "Bias Spectroscopy>Sweep Start (V)"
'                            .BiasSpec_SweepStart_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Bias Spectroscopy>Sweep End (V)"
'                            .BiasSpec_SweepEnd_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Bias Spectroscopy>Num Pixel"
'                            .MeasurementPoints = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Bias Spectroscopy>Z Avg time"
'                            .Z_Avg_Time_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Bias Spectroscopy>backward sweep"
'                            .Backward_Sweep = Boolean.Parse(sPropertyValue)
'                        Case "Bias Spectroscopy>Number of sweeps"
'                            .NumberOfSweeps = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)

'                        Case "Current>Current (A)"
'                            .Curr_Current = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Current>Calibration (A/V)"
'                            .Curr_Calibration = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Current>Offset (A)"
'                            .Curr_Offset = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Current>Gain"
'                            .Curr_Gain = sPropertyValue

'                        Case "Z Spectroscopy>Num Pixel"
'                            .MeasurementPoints = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z Spectroscopy>Z Avg time (s)"
'                            .Z_Avg_Time_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z Spectroscopy>backward sweep"
'                            .Backward_Sweep = Boolean.Parse(sPropertyValue)
'                        Case "Z Spectroscopy>Number of sweeps"
'                            .NumberOfSweeps = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)

'                        Case "Z-Controller>Setpoint"
'                            .FeedbackOpenCurrent_A = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                            .ZController_Setpoint = .FeedbackOpenCurrent_A
'                        Case "Z-Controller>Setpoint unit"
'                            .ZController_SetpointUnit = sPropertyValue
'                        Case "Z-Controller>Controller name"
'                            .ZController_ControllerName = sPropertyValue
'                        Case "Z-Controller>Controller status"
'                            .ZController_ControllerStatus = (sPropertyValue = "OFF")
'                        Case "Z-Controller>P gain"
'                            .ZController_PGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z-Controller>I gain"
'                            .ZController_IGain = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z-Controller>Time const (s)"
'                            .ZController_TimeConst = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z-Controller>TipLift (m)"
'                            .ZController_TipLift = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z-Controller>Switch off delay (s)"
'                            .ZController_SwitchOffDelay = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
'                        Case "Z-Controller>Z (m)"
'                            .ZController_Z = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)

'                        Case Else
'                            ' Add to the general property array.
'                            .AddGeneralProperty(sPropertyName, sPropertyValue)

'                    End Select
'                End With
'            Loop


'            If Not FetchOnlyFileHeader Then

'                sr.Peek()

'                ' Read Spectroscopy-Data
'                Dim iRowCounter As Integer = 1
'                Dim ParsedDouble As Double
'                Do Until sr.EndOfStream
'                    ReaderBuffer = sr.ReadLine

'                    SplittedLine = Split(ReaderBuffer, vbTab)

'                    '## Changed: 11/13/2014
'                    ' Check, if the number of columns matches the number of values...
'                    ' sometimes this is not the case ... who knows why ...???
'                    If lColumns.Count < SplittedLine.Length Then Exit Do

'                    For i As Integer = 0 To SplittedLine.Length - 1 Step 1
'                        ' Save Spectroscopy-Values.
'                        If Double.TryParse(SplittedLine(i), NumberStyles.Float, CultureInfo.InvariantCulture, ParsedDouble) Then
'                            lColumns(i).AddValueToColumn(ParsedDouble)
'                        Else
'                            lColumns(i).AddValueToColumn(0D)
'                        End If
'                        ' The code below has been very slow, compared to the code above!
'                        'If IsNumeric(SplittedLine(i)) Then
'                        '    lColumns(i).Values.Add(Double.Parse(SplittedLine(i), Globalization.CultureInfo.InvariantCulture))
'                        'Else
'                        '    lColumns(i).Values.Add(0D)
'                        'End If
'                    Next

'                    lColumns(PointColumnIndex).AddValueToColumn(iRowCounter)

'                    iRowCounter += 1
'                Loop

'                ' Check, if the number of Measurement-Points was read correctly 
'                ' from the Spectroscopy-File --> this is not the case, if the user
'                ' does not select some spectroscopy parameter sets to be saved!
'                ' Also multi-segment-bias-spectroscopy gives the wrong number in the
'                If lColumns.Count <> 0 Then
'                    If lColumns(0).Values(True).Count <> oSpectroscopyTable.MeasurementPoints Then
'                        oSpectroscopyTable.MeasurementPoints = lColumns(0).Values(True).Count
'                    End If
'                End If
'            End If

'            ' Finally add all Columns from the Temporary List to the Spectroscopy-Table
'            For Each oColumn As cSpectroscopyTable.DataColumn In lColumns
'                oColumn.IsSpectraFoxGenerated = False
'                oSpectroscopyTable.AddNonPersistentColumn(oColumn)
'            Next

'        End Using

'        ' File Exists, so Set the Property.
'        oSpectroscopyTable._bFileExists = True

'        Return oSpectroscopyTable
'    End Function

'    ''' <summary>
'    ''' File-Extension
'    ''' </summary>
'    Public ReadOnly Property FileExtension As String Implements iFileImport_SpectroscopyTable.FileExtension
'        Get
'            Return ".vpdata"
'        End Get
'    End Property

'    ''' <summary>
'    ''' Checks, if the given file is a known GSXM file-type
'    ''' </summary>
'    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
'                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
'        ' Load StreamReader and read the SECOND line.
'        ' Is the only one needed for Identification.
'        Using sr As New StreamReader(FullFileNamePlusPath)
'            ReaderBuffer = sr.ReadLine
'            ReaderBuffer = sr.ReadLine
'        End Using

'        If ReaderBuffer.StartsWith("# GXSM Vector Probe Data") Then
'            Return True
'        End If
'        Return False
'    End Function

'End Class

