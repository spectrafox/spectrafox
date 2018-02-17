Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportUnknownDAT
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Settings-Entry Regex (e.g. "CreationTime=Jun/09/2014 4:59:48 PM").
    ''' </summary>
    Protected SettingsRegex As New Regex("^(?<name>.*?)\=(?<value>.*?)$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    Protected ColumnSplitter As New Regex("\s*(?<ColumnName>[\w\s]*?),\s*(?<ColumnUnit>[\w]{1,})", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
    Protected ValueSplitter As New Regex("(?<Value>[\-\+\d]{1,}.[\-\+\d]{1,})", RegexOptions.Compiled Or RegexOptions.IgnoreCase)


    ''' <summary>
    ''' Value-pair Regex (matches values with a unit, e.g. 0.986 Hz).
    ''' </summary>
    Protected ValuePairRegex As New Regex("^(?<value>[+-]?\s?[\d\.]*?)\s(?<unit>\D*?)$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Returns a key value pair from a string (e.g. 0.986 Hz).
    ''' </summary>
    Protected Function GetKeyValuePair(ByVal ValueString As String) As KeyValuePair(Of Double, String)

        Dim KV As KeyValuePair(Of Double, String)
        Dim M As Match = ValuePairRegex.Match(ValueString)
        If M.Success Then
            KV = New KeyValuePair(Of Double, String)(Convert.ToDouble(M.Groups.Item("value").Value, CultureInfo.InvariantCulture), M.Groups.Item("unit").Value)
        Else
            KV = New KeyValuePair(Of Double, String)()
        End If

        Return KV
    End Function


    ''' <summary>
    ''' Imports the spectroscopy-file into a cSpectroscopyTable
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
        Using sr As New StreamReader(FullFileNamePlusPath)

            ' Read file Line-By-Line and export settings.
            ReaderBuffer = ""

            ' Temporary variables
            Dim SettingsName As String
            Dim SettingsValue As String
            Dim SettingsMatch As Match

            ' Read Settings up to the Position of column description
            Do Until sr.EndOfStream Or ReaderBuffer.Contains("[Series layout]")
                ReaderBuffer = sr.ReadLine

                ' Split the settings.
                SettingsMatch = SettingsRegex.Match(ReaderBuffer)

                ' Only consider settings with two splitted values:
                If SettingsMatch.Success Then

                    ' Get the setting-name
                    SettingsName = SettingsMatch.Groups("name").Value
                    SettingsValue = SettingsMatch.Groups("value").Value

                    With oSpectroscopyTable

                        ' Other Properties
                        Select Case SettingsName

                            Case "Drive Start"
                                .BiasSpec_SweepStart_V = GetKeyValuePair(SettingsValue).Key

                            Case "Drive End"
                                .BiasSpec_SweepEnd_V = GetKeyValuePair(SettingsValue).Key

                            Case "Points Number"
                                .MeasurementPoints = Integer.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)

                            Case Else
                                ' Add to the general property array.
                                .AddGeneralProperty(SettingsName, SettingsValue)

                        End Select
                    End With
                End If
            Loop

            Dim lColumns As New List(Of cSpectroscopyTable.DataColumn)
            ' Read Settings up to the position of data
            Do Until sr.EndOfStream Or ReaderBuffer.Contains("[Data]")
                ReaderBuffer = sr.ReadLine

                ' Split the settings.
                SettingsMatch = SettingsRegex.Match(ReaderBuffer)

                ' Only consider settings with two splitted values:
                If SettingsMatch.Success Then

                    ' Get the setting-name
                    SettingsName = SettingsMatch.Groups("name").Value
                    SettingsValue = SettingsMatch.Groups("value").Value

                    With oSpectroscopyTable

                        ' Other Properties
                        Select Case SettingsName

                            Case "Points Number"
                                .MeasurementPoints = Integer.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)

                            Case Else
                                ' Add to the general property array.
                                .AddGeneralProperty(SettingsName, SettingsValue)

                        End Select
                    End With
                Else
                    ' Spalten auslesen
                    If ReaderBuffer.StartsWith("Trace") Then
                        ' Split the columns.
                        Dim ColumnMatch As Match = ColumnSplitter.Match(ReaderBuffer)
                        While ColumnMatch.Success
                                Dim ColumnName As String = ColumnMatch.Groups("ColumnName").Value.Trim
                                Dim ColumnUnit As String = ColumnMatch.Groups("ColumnUnit").Value.Trim
                                ' Add Column
                                Dim oColumn As New cSpectroscopyTable.DataColumn
                                oColumn.Name = ColumnName
                                oColumn.UnitSymbol = ColumnUnit
                                lColumns.Add(oColumn)

                                ColumnMatch = ColumnMatch.NextMatch()
                        End While
                    End If
                End If
            Loop

            ' Add a separate column just for the measurement point number
            Dim PointColumn As New cSpectroscopyTable.DataColumn
            PointColumn.Name = My.Resources.ColumnName_MeasurementPoints
            PointColumn.UnitSymbol = "1"
            PointColumn.UnitType = cUnits.UnitType.Unitary
            lColumns.Add(PointColumn)
            Dim PointColumnIndex As Integer = lColumns.Count - 1

            If Not FetchOnlyFileHeader Then

                ' Read spectroscopy-data
                Dim iRowCounter As Integer = 1
                Dim ParsedDouble As Double

                Do Until sr.EndOfStream
                    ReaderBuffer = sr.ReadLine

                    Dim ValueList As New List(Of String)
                    Dim ValueMatch As Match = ValueSplitter.Match(ReaderBuffer)
                    While ValueMatch.Success
                        ValueList.Add(ValueMatch.Groups("Value").Value.Trim)
                        ValueMatch = ValueMatch.NextMatch()
                    End While

                    For i As Integer = 0 To ValueList.Count - 1 Step 1
                        If ValueList(i) = String.Empty Then Continue For
                        If Double.TryParse(ValueList(i), NumberStyles.Float, CultureInfo.InvariantCulture, ParsedDouble) Then
                            lColumns(i).AddValueToColumn(ParsedDouble)
                        Else
                            lColumns(i).AddValueToColumn(0D)
                        End If
                    Next

                    lColumns(PointColumnIndex).AddValueToColumn(iRowCounter)

                    iRowCounter += 1
                Loop

                ' Check, if the number of MeasurementPoints was read correctly from the SpectroscopyFile
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
    ''' Checks, if the given file is a BRUKER spectroscopy file.
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Using sr As New StreamReader(FullFileNamePlusPath)
            ReaderBuffer = sr.ReadLine
        End Using

        If ReaderBuffer.Contains("[Point Spectroscopy Data]") Then
            Return True
        End If

        Return False
    End Function



End Class
