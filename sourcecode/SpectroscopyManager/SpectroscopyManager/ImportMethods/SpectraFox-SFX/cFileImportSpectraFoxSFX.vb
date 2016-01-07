Imports System.IO
Imports System.Globalization

Public Class cFileImportSpectraFoxSFX
    ''' <summary>
    ''' Imports the SpectraFox-Extension-File to modify an existing file-object.
    ''' </summary>
    Public Shared Function ImportFileObject(ByRef InputFileObject As cFileObject,
                                            ByRef FullFileNamePlusPath As String) As cFileObject

        Try
            ' Open the XML-reader object for the specified file
            Dim XMLReader As Xml.XmlReader = New Xml.XmlTextReader(FullFileNamePlusPath)

            ' Save the Spectrafox-version that created the file to 
            ' check against old files and new features.
            Dim SpectraFoxVersionOfFile As String = ""

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
                                Case "SpectraFox"
                                    ' get and check the properties:
                                    '###############################
                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Version"
                                                    SpectraFoxVersionOfFile = .Value
                                            End Select
                                        End While
                                    End If

                                Case "BaseFileInformation"
                                    ' get and check the properties:
                                    '###############################
                                    Dim SubTreeReader As Xml.XmlReader = .ReadSubtree
                                    While SubTreeReader.Read
                                        If SubTreeReader.NodeType <> Xml.XmlNodeType.Element Then Continue While

                                        Select Case SubTreeReader.Name
                                            Case "Name"
                                                SubTreeReader.Read()

                                                If InputFileObject.FileNameWithoutPath.ToString <> SubTreeReader.Value Then
                                                    Exit Do
                                                End If
                                            Case "Type"
                                                SubTreeReader.Read()

                                                If InputFileObject.FileType.ToString <> SubTreeReader.Value Then
                                                    Exit Do
                                                End If

                                                ' Create file-type specific objects
                                                Select Case InputFileObject.FileType
                                                    Case cFileObject.FileTypes.SpectroscopyTable
                                                        ' Create Spectroscopy-Table Object, if not yet created in the file-object.
                                                        If InputFileObject.SpectroscopyTable Is Nothing Then
                                                            InputFileObject.SpectroscopyTable = New cSpectroscopyTable
                                                        End If

                                                End Select
                                            Case "DetailedType"
                                                'If InputFileObject.DetailedFileType.ToString <> .Value Then
                                                '    Exit Do
                                                'End If
                                        End Select
                                    End While

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
                                        Dim Values As String = .ReadString()
                                        Dim Buffer As String = String.Empty
                                        For i As Integer = 0 To Values.Length - 1 Step 1
                                            ' Read value
                                            If Values(i) <> ";" Then
                                                Buffer = Buffer & Values(i)
                                            Else
                                                ColumnValues.Add(Convert.ToDouble(Buffer, System.Globalization.CultureInfo.InvariantCulture))
                                                Buffer = ""
                                            End If
                                        Next
                                        SpectroscopyColumn.SetValueList(ColumnValues)

                                        ' Add the imported column only to the FileObject,
                                        ' if the value-count is not zero! Otherwise something went wrong,
                                        ' either during saving, or during loading!"
                                        If ColumnValues.Count > 0 Then
                                            If Not InputFileObject.SpectroscopyTable.Columns.ContainsKey(SpectroscopyColumn.Name) Then
                                                ' add new column
                                                InputFileObject.AddSpectroscopyColumn(SpectroscopyColumn, False)
                                            Else
                                                ' column exists, just update data
                                                InputFileObject.SpectroscopyTable.Columns(SpectroscopyColumn.Name) = SpectroscopyColumn
                                            End If
                                        End If
                                    End If

                                Case "SpectroscopyTable_CropInformation"

                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then

                                        Dim MaxIndexIncl As Integer = -1
                                        Dim MinIndexIncl As Integer = -1

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "MaxIndexIncl"
                                                    MaxIndexIncl = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "MinIndexIncl"
                                                    MinIndexIncl = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            End Select
                                        End While

                                        Dim CI As New cSpectroscopyTable.DataColumn.CropInformation(MinIndexIncl, MaxIndexIncl)

                                        ' Save newly imported crop information
                                        InputFileObject.Set_SpectroscopyTable_CropInformation(CI, False)
                                    End If

                                Case "ScanChannel"

                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then
                                        ' Container for the added column.
                                        Dim ScanChannel As New cScanImage.ScanChannel
                                        Dim RowCount As Integer
                                        Dim ColumnCount As Integer

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Name"
                                                    ScanChannel.Name = .Value
                                                Case "UnitSymbol"
                                                    ScanChannel.UnitSymbol = .Value
                                                Case "UnitType"
                                                    ScanChannel.Unit = cUnits.GetUnitTypeFromTypeString(.Value)
                                                Case "ScanDirection"
                                                    ScanChannel.ScanDirection = cScanImage.ScanChannel.GetScanDirectionFromString(.Value)
                                                Case "Bias"
                                                    ScanChannel.Bias = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "Calibration"
                                                    ScanChannel.Calibration = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "Offset"
                                                    ScanChannel.Offset = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "Rows"
                                                    RowCount = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "Columns"
                                                    ColumnCount = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "InSourceFile"
                                                    ScanChannel.IsSpectraFoxGenerated = Convert.ToBoolean(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            End Select
                                        End While

                                        ' Create ScanChannel-Matrix
                                        ScanChannel.ScanData = New MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(RowCount, ColumnCount)

                                        ' Get the data from the innerxml
                                        Dim Values As String = .ReadString()
                                        Dim Buffer As String = String.Empty
                                        Dim x As Integer = 0
                                        Dim y As Integer = 0
                                        For i As Integer = 0 To Values.Length - 1 Step 1
                                            ' Read value
                                            If Values(i) <> ";" Then
                                                Buffer = Buffer & Values(i)
                                            Else
                                                ScanChannel.ScanData(y, x) = Convert.ToDouble(Buffer, System.Globalization.CultureInfo.InvariantCulture)
                                                x += 1
                                                If x >= ColumnCount Then
                                                    x = 0
                                                    y += 1
                                                End If
                                                Buffer = ""
                                            End If
                                        Next

                                        ' Save newly imported scan channel
                                        InputFileObject.AddScanChannel(ScanChannel, False)
                                    End If

                                Case "ScanImageFilter"

                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then

                                        ' Container for the filter.
                                        Dim Filter As iScanImageFilter = Nothing
                                        Dim FilterChannelName As String = String.Empty

                                        Try
                                            While .MoveToNextAttribute
                                                Select Case .Name
                                                    Case "ChannelName"
                                                        FilterChannelName = .Value
                                                    Case "FilterType"
                                                        ' Try get the filter from the type
                                                        Dim FilterType As Type = Type.GetType(.Value)
                                                        Filter = CType(Activator.CreateInstance(FilterType), iScanImageFilter)
                                                    Case "Settings"
                                                        If Not Filter Is Nothing Then Filter.FilterSettingsString = .Value
                                                End Select
                                            End While

                                            ' Apply the filter to the image
                                            If Not Filter Is Nothing AndAlso FilterChannelName <> String.Empty AndAlso InputFileObject.ScanImage IsNot Nothing Then
                                                For Each SC As cScanImage.ScanChannel In InputFileObject.GetScanChannelList.Values
                                                    If SC.Name = FilterChannelName Then
                                                        SC.AddFilter(Filter)
                                                    End If
                                                Next
                                            End If

                                        Catch ex As Exception
                                            Debug.WriteLine("ScanImage filter given in the cache file not could not be loaded. " & ex.Message)
                                        End Try

                                    End If

                                'Case "PreviewImage"

                                '    ' get all parameters:
                                '    '#####################
                                '    ' go through all attributes
                                '    If .AttributeCount > 0 Then
                                '        ' Container for the added column.
                                '        Dim PreviewImageID As String = String.Empty

                                '        While .MoveToNextAttribute
                                '            Select Case .Name
                                '                Case "ImageID"
                                '                    PreviewImageID = .Value
                                '            End Select
                                '        End While

                                '        ' Get the data from the innerxml
                                '        Dim ImageBytes As Byte() = Convert.FromBase64String(.ReadString())
                                '        Dim PreviewImage As Image = cImageConverter.ByteArrayToImage(ImageBytes)

                                '        ' Save newly imported preview image
                                '        If PreviewImageID <> String.Empty Then InputFileObject.SetPreviewImageStorage(PreviewImageID, PreviewImage)
                                '    End If

                                Case "AdditionalComment"

                                    ' Save the additional comment:
                                    InputFileObject.SetExtendedComment(.ReadString(), False)


                            End Select

                        Case Xml.XmlNodeType.Text
                            ' Text is unimportant so far!
                            '#############################

                        Case Xml.XmlNodeType.Comment
                            ' A comment is also unimportant so far!
                            '#######################################

                    End Select

                Loop

                ' Close the XML-Reader
                .Close()
                .Dispose()
            End With
        Catch ex As Exception
            Debug.WriteLine("SFX-Import failed in file " & InputFileObject.FileNameWithoutPath & ", due to exception: " & vbNewLine & ex.Message)
        End Try

        Return InputFileObject
    End Function

    '#########################################################################################
    '
    '                      SHARED PART - Accessible from other classes.
    '
    '#########################################################################################

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public Shared FileExtension As String = ".sfx"

    ''' <summary>
    ''' Checks, if the given File is a known SpectraFox-File
    ''' </summary>
    Public Shared Function IdentifyFile(FullFileName As String) As Boolean
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Dim sr As New StreamReader(FullFileName)
        Dim sFirstLine As String = sr.ReadLine
        sr.Close()
        sr.Dispose()

        If Not sFirstLine Is Nothing Then
            ' SpectraFox File is an XML-File:
            If sFirstLine.Contains("<?xml") Then
                Return True
            End If
        End If

        Return False
    End Function
End Class
