Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' General class which contains functions to import SpectraFox data-files.
''' </summary>
Public Class cFileImportSpectraFoxSFD
    Implements iFileImport_SpectroscopyTable

#Region "Import SpectraFoxSFD File"

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

        ' Create a filestream for the file.
        Using FS As New FileStream(FullFileNamePlusPath, FileMode.Open)

            ' Uncompress the file using a GZipStream
            Using GZ As New Compression.GZipStream(FS, Compression.CompressionMode.Decompress, True)

                ' Open the XML-reader object on the decompressed stream
                Using XMLReader As New Xml.XmlTextReader(GZ)

                    ' Now read the XML-file, and import the settings.
                    With XMLReader
                        ' read up to the end of the file
                        Do While .Read
                            ' Check for the type of data
                            Select Case .NodeType
                                Case Xml.XmlNodeType.Element
                                    ' An element comes: this is what we are looking for!
                                    '####################################################
                                    Select Case .Name

                                        Case "GeneralProperty"

                                            ' Add a file property as Header
                                            Dim GeneralFilePropertyKey As String = String.Empty
                                            Dim GeneralFilePropertyValue As String = String.Empty
                                            While .MoveToNextAttribute
                                                Select Case .Name
                                                    Case "Key"
                                                        GeneralFilePropertyKey = .Value
                                                    Case "Value"
                                                        GeneralFilePropertyValue = .Value
                                                End Select
                                            End While
                                            oSpectroscopyTable.AddGeneralProperty(GeneralFilePropertyKey, GeneralFilePropertyValue)

                                        Case "SpectroscopyColumn"
                                            ' get all parameters:
                                            '#####################
                                            ' go through all attributes
                                            If .AttributeCount > 0 Then
                                                ' Container for the added column.
                                                Dim SpectroscopyColumn As New cSpectroscopyTable.DataColumn

                                                While .MoveToNextAttribute
                                                    Select Case .Name
                                                        Case "Name"
                                                            SpectroscopyColumn.Name = .Value
                                                        Case "UnitSymbol"
                                                            SpectroscopyColumn.UnitSymbol = .Value
                                                        Case "UnitType"
                                                            SpectroscopyColumn.UnitType = cUnits.GetUnitTypeFromTypeString(.Value)
                                                        Case "ValueCount"
                                                            SpectroscopyColumn.ValueCount = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                    End Select
                                                End While

                                                ' Get the data from the innerxml
                                                Dim ColumnValues As New List(Of Double)(SpectroscopyColumn.ValueCount)
                                                Dim SingleValues As String() = .ReadString().Split({";"}, StringSplitOptions.RemoveEmptyEntries)
                                                For i As Integer = 0 To SingleValues.Length - 1 Step 1
                                                    ColumnValues.Add(Convert.ToDouble(SingleValues(i), System.Globalization.CultureInfo.InvariantCulture))
                                                Next
                                                SpectroscopyColumn.SetValueList(ColumnValues)
                                                SpectroscopyColumn.IsSpectraFoxGenerated = False

                                                ' Add the column to the file.
                                                oSpectroscopyTable.AddNonPersistentColumn(SpectroscopyColumn)
                                            End If

                                        Case "Location"
                                            ' get all parameters:
                                            '#####################
                                            ' go through all attributes
                                            If .AttributeCount > 0 Then
                                                While .MoveToNextAttribute
                                                    Select Case .Name
                                                        Case "X"
                                                            oSpectroscopyTable.Location_X = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                        Case "Y"
                                                            oSpectroscopyTable.Location_Y = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                        Case "Z"
                                                            oSpectroscopyTable.Location_Z = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                    End Select
                                                End While
                                            End If

                                        Case "Record"
                                            ' get all parameters:
                                            '#####################
                                            ' go through all attributes
                                            If .AttributeCount > 0 Then
                                                While .MoveToNextAttribute
                                                    Select Case .Name
                                                        Case "Date"
                                                            oSpectroscopyTable.RecordDate = Convert.ToDateTime(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                    End Select
                                                End While
                                            End If

                                        Case "Comment"
                                            oSpectroscopyTable.Comment = .ReadString()

                                    End Select

                            End Select
                        Loop

                    End With

                End Using

            End Using

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
            Return ".sfd"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known Nanonis file-type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        Dim bIsSFDFile As Boolean = False

        ' Create a filestream.
        Using FS As New FileStream(FullFileNamePlusPath, FileMode.Open)

            ' SpectraFox data-files are compressed XML.
            If cCompression.IsPossiblyGZippedBytes(FS) Then
                ' Uncompress
                Using GZ As New Compression.GZipStream(FS, Compression.CompressionMode.Decompress, True)
                    Dim sFirstLine As String
                    Using sr As New StreamReader(GZ)
                        sFirstLine = sr.ReadLine
                    End Using

                    If Not sFirstLine Is Nothing Then
                        ' SpectraFox data-files are XML-Files:
                        If sFirstLine.Contains("<?xml") Then
                            bIsSFDFile = True
                        End If
                    End If
                End Using
            End If

        End Using

        Return bIsSFDFile
    End Function

#End Region

#Region "Export SpectroscopyTable as SFD File"

    ''' <summary>
    ''' Writes a SpectroscopyTable as SFD file.
    ''' The given Filename will be overridden, if it exists already.
    ''' Give the filename WITHOUT the .sfd ending!
    ''' </summary>
    Public Shared Sub WriteSpectraFoxSFDFile(ByVal OutputFilename As String,
                                             ByVal SpectroscopyTable As cSpectroscopyTable)

        Dim TargetFileName As String = OutputFilename & ".sfd"
        Dim TMPFileName As String = OutputFilename & ".sfd.~TMP~"
        Dim BackupFileName As String = OutputFilename & ".sfd.~bak~"

        Try

            ' Create a filestream.
            Using FS As New FileStream(TMPFileName, FileMode.Create)

                ' Compress using GZip
                Using GZ As New Compression.GZipStream(FS, Compression.CompressionMode.Compress, True)

                    ' Select the File-Encoding
                    Dim enc As New System.Text.UnicodeEncoding

                    ' Create the XmlTextWriter object
                    Using XMLobj As New Xml.XmlTextWriter(GZ, enc)

                        With XMLobj
                            ' Set the proper formatting
                            .Formatting = Xml.Formatting.Indented
                            .Indentation = 4

                            ' create the document header
                            .WriteStartDocument()
                            .WriteStartElement("root")

                            ' Begin with SpectraFox program properties
                            .WriteStartElement("SpectraFox")
                            .WriteAttributeString("Version", cProgrammDeployment.GetProgramVersionString)
                            .WriteEndElement()

                            .WriteStartElement("Properties")

                            .WriteStartElement("Record")
                            .WriteAttributeString("Date", SpectroscopyTable.RecordDate.ToString(System.Globalization.CultureInfo.InvariantCulture))
                            .WriteEndElement()
                            .WriteStartElement("Location")
                            .WriteAttributeString("X", SpectroscopyTable.Location_X.ToString("E10", System.Globalization.CultureInfo.InvariantCulture))
                            .WriteAttributeString("Y", SpectroscopyTable.Location_Y.ToString("E10", System.Globalization.CultureInfo.InvariantCulture))
                            .WriteAttributeString("Z", SpectroscopyTable.Location_Z.ToString("E10", System.Globalization.CultureInfo.InvariantCulture))
                            .WriteEndElement()

                            .WriteEndElement()

                            ' Begin the section of the data-columns
                            If Not SpectroscopyTable Is Nothing Then
                                .WriteStartElement("SpectroscopyColumns")
                                For Each Col As cSpectroscopyTable.DataColumn In SpectroscopyTable.Columns.Values
                                    If Col.IsSpectraFoxGenerated Then Continue For

                                    ' Write an element for each data-column
                                    .WriteStartElement("SpectroscopyColumn")
                                    .WriteAttributeString("Name", Col.Name)
                                    .WriteAttributeString("UnitSymbol", Col.UnitSymbol)
                                    .WriteAttributeString("UnitType", Col.UnitType.ToString)
                                    .WriteAttributeString("ValueCount", Col.Values(True).Count.ToString(System.Globalization.CultureInfo.InvariantCulture))
                                    For j As Integer = 0 To Col.Values(True).Count - 1 Step 1
                                        .WriteString(Col.Values(True)(j).ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                                        .WriteString(";")
                                    Next
                                    .WriteEndElement()
                                Next
                                .WriteEndElement()
                            End If

                            ' Write AdditionalComment
                            .WriteElementString("Comment", SpectroscopyTable.Comment)

                            ' End <root>
                            .WriteEndElement()

                            ' Close the document
                            .WriteEndDocument()
                        End With
                    End Using

                End Using

            End Using

            ' If everything was ok so far, so if we are at this point,
            ' move the temporary file to the real target-file-name,
            ' and overwrite the copy.
            If System.IO.File.Exists(TargetFileName) Then System.IO.File.Move(TargetFileName, BackupFileName)
            System.IO.File.Move(TMPFileName, TargetFileName)
            If System.IO.File.Exists(BackupFileName) Then System.IO.File.Delete(BackupFileName)

        Catch ex As Exception
            Debug.WriteLine("Error writing SFD-file:" & ex.Message)

            ' Move back the backup, if it existed!
            Try
                If System.IO.File.Exists(BackupFileName) Then System.IO.File.Move(BackupFileName, TargetFileName)
            Catch ex2 As Exception
                Debug.WriteLine("Error moving backup SFD-file back in place:" & ex2.Message)
            End Try
        End Try

    End Sub

#End Region

End Class

