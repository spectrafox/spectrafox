Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportNanotecCUR
    Inherits cFileImportNanotec
    Implements iFileImport_SpectroscopyTable

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

        ' Temporary variables
        Dim SettingsName As String
        Dim SettingsValue As String
        Dim SettingsMatch As Match

        ' Properties of this curve
        Dim XAxisLabel As String = String.Empty
        Dim YAxisLabel As String = String.Empty
        Dim XAxisUnitSymbol As String = String.Empty
        Dim YAxisUnitSymbol As String = String.Empty
        Dim NumberOfLines As Integer = 0

        ' Read settings up to the position of the spectroscopy data.
        Do Until sr.EndOfStream Or ReaderBuffer.Contains("[Header end]")
            ReaderBuffer = sr.ReadLine

            ' Split the settings.
            SettingsMatch = SettingsRegex.Match(ReaderBuffer)

            ' Only consider settings with two splitted values:
            If SettingsMatch.Success Then

                ' Get the setting-name
                SettingsName = SettingsMatch.Groups("name").Value
                SettingsValue = SettingsMatch.Groups("value").Value

                With oSpectroscopyTable

                    Select Case SettingsName

                        Case "Set Point"
                            .FeedbackOpenBias_V = Me.GetKeyValuePair(SettingsValue).Key * cUnits.GetFactorFromPrefix(Me.GetKeyValuePair(SettingsValue).Value).Value
                        Case "Number of points"
                            .MeasurementPoints = Integer.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)

                        Case "X axis text"
                            XAxisLabel = SettingsValue
                        Case "Y axis text"
                            YAxisLabel = SettingsValue
                        Case "X axis unit"
                            XAxisUnitSymbol = SettingsValue
                        Case "Y axis unit"
                            YAxisUnitSymbol = SettingsValue

                        Case "Number of lines"
                            NumberOfLines = Integer.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)

                    End Select
                End With

            End If
        Loop

        ' Create a temporary list of DataColumns
        Dim lColumns As New List(Of cSpectroscopyTable.DataColumn)

        ' Create all columns
        For i As Integer = 1 To NumberOfLines * 2 Step 1

            ' Add Column
            Dim oColumn As New cSpectroscopyTable.DataColumn

            If i Mod 2 = 0 Then
                ' Y Axis
                oColumn.Name = YAxisLabel.Replace("#y", YAxisUnitSymbol)
                oColumn.UnitSymbol = YAxisUnitSymbol
                oColumn.UnitType = cUnits.GetUnitTypeFromSymbol(YAxisUnitSymbol)
            Else
                ' X Axis
                oColumn.Name = XAxisLabel.Replace("#x", XAxisUnitSymbol)
                oColumn.UnitSymbol = XAxisUnitSymbol
                oColumn.UnitType = cUnits.GetUnitTypeFromSymbol(XAxisUnitSymbol)
            End If
            lColumns.Add(oColumn)

        Next

        ' Add a separate Column just for the measurement point number
        Dim PointColumn As New cSpectroscopyTable.DataColumn
        PointColumn.Name = My.Resources.ColumnName_MeasurementPoints
        PointColumn.UnitSymbol = "1"
        PointColumn.UnitType = cUnits.UnitType.Unitary
        lColumns.Add(PointColumn)
        Dim PointColumnIndex As Integer = lColumns.Count - 1

        If Not FetchOnlyFileHeader Then

            ' Get temporary variables.
            Dim SplittedLine As String()
            Dim iRowCounter As Integer = 1
            Dim ParsedDouble As Double

            ' Read Spectroscopy-Data
            Do Until sr.EndOfStream
                ReaderBuffer = sr.ReadLine

                SplittedLine = Split(ReaderBuffer, " ", 2 * NumberOfLines)

                ' Check for valid number of columns
                If lColumns.Count < SplittedLine.Length Then Exit Do

                For i As Integer = 0 To SplittedLine.Length - 1 Step 1
                    ' Save Spectroscopy-Values.
                    If Double.TryParse(SplittedLine(i), NumberStyles.Float, CultureInfo.InvariantCulture, ParsedDouble) Then
                        lColumns(i).AddValueToColumn(ParsedDouble)
                    Else
                        lColumns(i).AddValueToColumn(0D)
                    End If
                Next

                lColumns(PointColumnIndex).AddValueToColumn(iRowCounter)

                iRowCounter += 1
            Loop

            ' Check, if the number of points was read correctly from the Spectroscopy-File
            If lColumns.Count <> 0 Then
                If lColumns(0).Values(True).Count <> oSpectroscopyTable.MeasurementPoints Then
                    oSpectroscopyTable.MeasurementPoints = lColumns(0).Values(True).Count
                End If
            End If
        End If

        sr.Close()
        sr.Dispose()

        ' Finally add all columns from the Temporary List to the Spectroscopy-Table
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
            Return ".cur"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given File is a known Nanotec curve file type.
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        ' Load StreamReader and read first two lines.
        ' Is the only one needed for identification.
        Dim sr As New StreamReader(FullFileNamePlusPath)
        ReaderBuffer = sr.ReadLine
        ReaderBuffer = sr.ReadLine
        sr.Close()
        sr.Dispose()

        ' Nanotec IV curves contain in the second line "IV curve file"
        If ReaderBuffer.Contains("IV curve file") Then
            Return True
        End If

        Return False
    End Function

End Class
