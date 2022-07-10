Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' General class which contains functions to import Nanonis files.
''' </summary>
Public Class cFileImportGeneric
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Separator Characters [Tab;,]
    ''' </summary>
    Public Shared SeparatorCharacters As Char() = New Char() {ControlChars.Tab, Chr(59), Chr(44)}

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

        ' Determine the column separator character
        Dim cColumnSeparatorChar As Char = cFileImportGeneric.DetermineSeparatorCharacter(FullFileNamePlusPath, ReaderBuffer)
        If cColumnSeparatorChar = Chr(0) Then
            Throw New Exception("Column separator character in file " & oSpectroscopyTable.FullFileName & " could not be identified!")
        End If

        ' Initialize the list of columns to be added SpectroscopyTable
        Dim lColumns As New List(Of cSpectroscopyTable.DataColumn)
        Dim iPointColumnIndex As Integer

        ' Load StreamReader
        Using sr As New StreamReader(FullFileNamePlusPath)
            Dim SplittedLine As String()

            ' read the first line as header
            If Not sr.EndOfStream Then
                ' split the line with the separator character
                ReaderBuffer = sr.ReadLine
                SplittedLine = ReaderBuffer.Split(cColumnSeparatorChar)

                ' Check if this is a header line (i.e. the data is non-numeric)
                For i As Integer = 0 To SplittedLine.Length - 1 Step 1
                    SplittedLine(i) = SplittedLine(i).Trim
                    If SplittedLine(i) <> "" Then
                        ' Add Column
                        Dim oColumn As New cSpectroscopyTable.DataColumn With {
                            .Name = SplittedLine(i),
                            .UnitType = cUnits.UnitType.Unknown
                        }
                        lColumns.Add(oColumn)
                    End If
                Next

                ' Add a separate column just for the measurement point number
                Dim PointColumn As New cSpectroscopyTable.DataColumn With {
                    .Name = My.Resources.ColumnName_MeasurementPoints,
                    .UnitSymbol = "1",
                    .UnitType = cUnits.UnitType.Unitary
                }
                lColumns.Add(PointColumn)
                iPointColumnIndex = lColumns.Count - 1
            End If

            If Not FetchOnlyFileHeader Then
                sr.Peek()

                ' Read Spectroscopy-Data
                Dim iRowCounter As Integer = 1
                Dim ParsedDouble As Double
                Do Until sr.EndOfStream
                    ' split the line with the separator character
                    ReaderBuffer = sr.ReadLine
                    SplittedLine = ReaderBuffer.Split(cColumnSeparatorChar)
                    If lColumns.Count < SplittedLine.Length Then Exit Do

                    For i As Integer = 0 To SplittedLine.Length - 1 Step 1
                        SplittedLine(i) = SplittedLine(i).Trim
                        If SplittedLine(i) <> "" Then
                            ' Save Spectroscopy-Values.
                            If Double.TryParse(SplittedLine(i), NumberStyles.Float, CultureInfo.InvariantCulture, ParsedDouble) Then
                                lColumns(i).AddValueToColumn(ParsedDouble)
                            Else
                                lColumns(i).AddValueToColumn(0D)
                            End If
                        End If
                    Next

                    lColumns(iPointColumnIndex).AddValueToColumn(iRowCounter)

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
    ''' Determines the separator character of this specific file.
    ''' </summary>
    ''' <param name="iMaxLineCountToCheck">To speed up the process, the function may be limited to a max amount of lines to check for the usage of the same split character.</param>
    ''' <returns>Chr(0) if no character could be found</returns>
    Public Shared Function DetermineSeparatorCharacter(ByRef FullFileNamePlusPath As String,
                                                       Optional ByRef ReaderBuffer As String = "",
                                                       Optional ByVal iMaxLineCountToCheck As Integer = 5) As Char
        Dim sSeparatorCharacter As Char = Chr(0)
        Dim iFirstLineSplitLength As Integer = Nothing
        Dim iLineCount As Integer = 0

        ' Each line is split by one of the separator characters
        Using sr As New StreamReader(FullFileNamePlusPath)
            Do Until sr.EndOfStream Or iLineCount >= iMaxLineCountToCheck
                ReaderBuffer = sr.ReadLine
                iLineCount += 1

                If sSeparatorCharacter = Chr(0) Then
                    ' determine first line's separator character

                    For Each sCharacter As Char In SeparatorCharacters
                        iFirstLineSplitLength = ReaderBuffer.Split(sCharacter).Length
                        If iFirstLineSplitLength > 1 Then
                            sSeparatorCharacter = sCharacter
                            Exit For
                        End If
                    Next
                Else
                    ' try to split all consecutive lines with the determined character
                    If ReaderBuffer.Split(sSeparatorCharacter).Length <> iFirstLineSplitLength Then
                        Return Chr(0)
                    End If
                End If
            Loop
        End Using
        Return sSeparatorCharacter
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
    ''' Checks, if the given file is a known GSXM file-type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        Return cFileImportGeneric.DetermineSeparatorCharacter(FullFileNamePlusPath, ReaderBuffer) <> Chr(0)
    End Function

End Class

