Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportBrukerDAT
    Inherits cFileImportBruker
    Implements iFileImport_SpectroscopyTable

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

            ' Read Settings up to the Position of Spectroscopy
            ' Data in the file. Starts with [DATA].
            Do Until sr.EndOfStream Or ReaderBuffer.Contains("[Signal Tracing Data]")
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

                            Case "Feedback"
                                .ZController_ControllerStatus = (SettingsValue = "ON")

                            Case "Points Number"
                                .MeasurementPoints = Integer.Parse(SettingsValue, Globalization.CultureInfo.InvariantCulture)

                            Case Else
                                ' Add to the general property array.
                                .AddGeneralProperty(SettingsName, SettingsValue)

                        End Select
                    End With
                End If
            Loop


            ' Create a temporary list of DataColumns
            Dim lColumns As New List(Of cSpectroscopyTable.DataColumn)

            ' now create the columns from the settings:
            For Each PropertyKV As KeyValuePair(Of String, String) In oSpectroscopyTable.GeneralPropertyArray
                If PropertyKV.Key.StartsWith("Column") Then

                    ' Found a column:

                    ' Add Column
                    Dim oColumn As New cSpectroscopyTable.DataColumn

                    ' Save Column-Name
                    oColumn.Name = PropertyKV.Value

                    ' Save Column-Unit
                    'oColumn.UnitSymbol = oMatch.Groups("Unit").Value
                    'oColumn.UnitType = cUnits.GetUnitTypeFromSymbol(oMatch.Groups("Unit").Value)
                    lColumns.Add(oColumn)

                End If
            Next

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
                Dim SplittedLine As String()

                Do Until sr.EndOfStream
                    ReaderBuffer = sr.ReadLine

                    SplittedLine = Split(ReaderBuffer, vbTab)

                    ' Check, if the number of columns matches the number of values.
                    If lColumns.Count < SplittedLine.Length Then Exit Do

                    For i As Integer = 0 To SplittedLine.Length - 1 Step 1
                        If SplittedLine(i) = String.Empty Then Continue For
                        If Double.TryParse(SplittedLine(i), NumberStyles.Float, CultureInfo.InvariantCulture, ParsedDouble) Then
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

        ' BRUKER spectroscopy files contain in first row:
        ' "[Signal Tracing Parameters]"
        If ReaderBuffer.Contains("[Signal Tracing Parameters]") Then
            Return True
        End If

        Return False
    End Function

End Class
