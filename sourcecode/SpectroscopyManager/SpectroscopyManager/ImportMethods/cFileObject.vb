Imports System.IO
''' <summary>
''' Represents a File-Object with its properties.
''' </summary>
Public Class cFileObject
    Implements IDisposable

#Region "File Types"

    ''' <summary>
    ''' File-Types to Recognize
    ''' </summary>
    Public Enum FileTypes
        UNIDENTIFIED
        SpectroscopyTable
        ScanImage
        SpectraFoxFile
        GridFile
    End Enum

    ''' <summary>
    ''' Returns the filetype from a type-string.
    ''' </summary>
    Public Shared Function GetFileTypeFromString(ByVal S As String) As FileTypes
        Select Case S
            Case FileTypes.SpectroscopyTable.ToString
                Return FileTypes.SpectroscopyTable
            Case FileTypes.ScanImage.ToString
                Return FileTypes.ScanImage
            Case FileTypes.SpectraFoxFile.ToString
                Return FileTypes.SpectraFoxFile
            Case FileTypes.GridFile.ToString
                Return FileTypes.GridFile
            Case Else
                Return FileTypes.UNIDENTIFIED
        End Select
    End Function

#End Region

#Region "Events"

    ''' <summary>
    ''' Should get raised, if the file-object changed somehow!
    ''' </summary>
    Public eFileObjectChanged As New EventHandler(AddressOf RaiseEventFileObjectChanged)

    ''' <summary>
    ''' Call to raise the file-object changed event.
    ''' </summary>
    Public Sub OnFileObjectChanged()
        'RaiseEventAndExecuteItInAnExplicitOrUIThread(eFileObjectChanged, Nothing, Nothing)
        eFileObjectChanged.Raise(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Event, that the underlying file-object has changed.
    ''' </summary>
    Public Event FileObjectChanged()

    ''' <summary>
    ''' Raises the actual event to the outside!
    ''' </summary>
    Public Sub RaiseEventFileObjectChanged(sender As Object, e As EventArgs)
        RaiseEvent FileObjectChanged()
    End Sub

#End Region

#Region "General Properties extractable from the raw file."

    ''' <summary>
    ''' Stores the type of the file.
    ''' </summary>
    Public FileType As FileTypes

    ''' <summary>
    ''' Stores the date of the last change of the file.
    ''' </summary>
    Public LastFileChange As Date

    ''' <summary>
    ''' Stores a link to the required import-routine.
    ''' </summary>
    Public ImportRoutine As Type

    ''' <summary>
    ''' Returns the full filename that includes the full path.
    ''' e.g. C:\folder\file.dat
    ''' </summary>
    Public Property FullFileNameInclPath As String

    ''' <summary>
    ''' Returns just the filename without path, but with extension.
    ''' </summary>
    Public ReadOnly Property FileNameWithoutPath As String
        Get
            Return System.IO.Path.GetFileName(Me.FullFileNameInclPath)
        End Get
    End Property

    ''' <summary>
    ''' Returns just the filename without path, but with extension.
    ''' </summary>
    Public ReadOnly Property FileName As String
        Get
            Return System.IO.Path.GetFileName(Me.FullFileNameInclPath)
        End Get
    End Property

    ''' <summary>
    ''' Returns just the path without the filename.
    ''' </summary>
    Public ReadOnly Property Path As String
        Get
            Return System.IO.Path.GetDirectoryName(Me.FullFileNameInclPath)
        End Get
    End Property

    ''' <summary>
    ''' Name that is displayed in the file entry for this fileobject.
    ''' </summary>
    Private _DisplayName As String = String.Empty

    ''' <summary>
    ''' Name that is displayed in the file entry for this fileobject.
    ''' If the custom name is empty, it returns the filenamewithoutpath.
    ''' </summary>
    Public Property DisplayName As String
        Get
            If Me._DisplayName = String.Empty Then
                Return Me.FileNameWithoutPath
            Else
                Return Me._DisplayName
            End If
        End Get
        Set(value As String)
            Me._DisplayName = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' This list stores the filenames that a single file object is describing.
    ''' This is important for systems with multiple files per spectrum or image,
    ''' where we want to combine all files in a single entry.
    ''' All files in this list are excluded from the import.
    ''' 
    ''' IMPORTANT: This list should only contain filenames! NO PATHS, as these are
    '''            relative on each computer!
    ''' </summary>
    Protected Friend _FileObjectDescribedByMultipleFiles As List(Of String)

    ''' <summary>
    ''' This list stores the filenames that a single file object is describing.
    ''' This is important for systems with multiple files per spectrum or image,
    ''' where we want to combine all files in a single entry.
    ''' All files in this list are excluded from the import.
    ''' 
    ''' IMPORTANT: This list should only contain filenames! NO PATHS, as these are
    '''            relative on each computer!
    ''' </summary>
    Public ReadOnly Property FileObjectDescribedByMultipleFiles As List(Of String)
        Get
            Return Me._FileObjectDescribedByMultipleFiles
        End Get
    End Property

#End Region

#Region "Programmatically added data"

#Region "Record-Date, Location and Dimensions"

    ''' <summary>
    ''' Record date of the file
    ''' </summary>
    Public RecordDate As Date

    ''' <summary>
    ''' Location of the File
    ''' </summary>
    Public RecordLocation_X As Double
    Public RecordLocation_Y As Double
    Public RecordLocation_Z As Double

    ''' <summary>
    ''' Special locations stored in the fileobject.
    ''' E.g. used in the grid file to store the locations of the spectra.
    ''' </summary>
    Public SpecialLocationList As New List(Of cNumericalMethods.Point3D)

    ''' <summary>
    ''' Variable that saves the Record-Range of a ScanImage
    ''' </summary>
    Public ScanImageRange_X As Double
    Public ScanImageRange_Y As Double

    ''' <summary>
    ''' Saves a string for the measurement dimensions
    ''' </summary>
    Public MeasurementDimensions As String = ""

#End Region

#Region "source file comment"

    Protected Friend _SourceFileComment As String = String.Empty
    ''' <summary>
    ''' Comment present in the source file.
    ''' </summary>
    Public ReadOnly Property SourceFileComment As String
        Get
            Return Me._SourceFileComment
        End Get
    End Property

#End Region

#Region "extended comment"
    Private _ExtendedComment As String = String.Empty
    ''' <summary>
    ''' Extended comment, e.g. used to save the output of several routines.
    ''' </summary>
    Public ReadOnly Property ExtendedComment As String
        Get
            Return Me._ExtendedComment
        End Get
    End Property

    ''' <summary>
    ''' Sets the extended comment
    ''' </summary>
    Public Sub SetExtendedComment(ByVal Comment As String,
                                  Optional ByVal SaveSFXFile As Boolean = True)
        Me._ExtendedComment = Comment.Trim
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()
    End Sub

    ''' <summary>
    ''' Adds an extended comment,
    ''' and adds the NewLine and seperator lines.
    ''' </summary>
    Public Sub AddExtendedComment(ByVal Comment As String,
                                    Optional ByVal SaveSFXFile As Boolean = True)
        If Me._ExtendedComment <> String.Empty Then
            Me._ExtendedComment = Me._ExtendedComment & vbNewLine & "---------- " & Now.ToString("r") & " ----------" & vbNewLine
        End If
        Me._ExtendedComment = Me._ExtendedComment & Comment.Trim
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()
    End Sub
#End Region

#Region "Preview-Image storage"
    Protected _PreviewImageStorage As New Dictionary(Of String, Image)
    ''' <summary>
    ''' Saves preview-images for the selected column.
    ''' </summary>
    Public ReadOnly Property PreviewImageStorage As Dictionary(Of String, Image)
        Get
            Return Me._PreviewImageStorage
        End Get
    End Property

    ''' <summary>
    ''' Sets the preview-image for a given columnindex-pair (x-index;y-index).
    ''' !!!!!!!!!!! Don't raises the on fileobject-changed event     !!!!!!!!!!!!!!!!!!
    ''' !!!!!!!!!!! because it is not needed for this cache storage. !!!!!!!!!!!!!!!!!!
    ''' </summary>
    Public Sub SetPreviewImageStorage(ByVal Index As String,
                                      ByVal PreviewImage As Image)
        If Me._PreviewImageStorage.ContainsKey(Index) Then
            Me._PreviewImageStorage(Index) = PreviewImage
        Else
            Me._PreviewImageStorage.Add(Index, PreviewImage)
        End If
        '# 16.10.2015: removed, since preview images are not stored anymore in the SFX files!
        'If SaveSFXFile Then Me.SaveChangesAsFile()
    End Sub

    ''' <summary>
    ''' Removes an entry from the preview-image list in the file-object.
    ''' </summary>
    Public Sub RemovePreviewImage(ByRef ColumnIndex As String)
        If Me._PreviewImageStorage.ContainsKey(ColumnIndex) Then
            Me._PreviewImageStorage.Remove(ColumnIndex)
            Me.OnFileObjectChanged()
        End If
    End Sub

    ''' <summary>
    ''' Clears all preview-images added.
    ''' </summary>
    Public Sub ClearAllPreviewImages()
        Me._PreviewImageStorage.Clear()
        Me.OnFileObjectChanged()
    End Sub

#End Region

#Region "Crop Information for the SpectroscopyFileObject"

    ''' <summary>
    ''' Stores the crop information for the spectroscopy-table object.
    ''' </summary>
    Protected _SpectroscopyTable_CropInformation As New cSpectroscopyTable.DataColumn.CropInformation

    ''' <summary>
    ''' Information about the cropped columns in a spectroscopy-table.
    ''' </summary>
    Public ReadOnly Property SpectroscopyTable_CropInformation As cSpectroscopyTable.DataColumn.CropInformation
        Get
            Return Me._SpectroscopyTable_CropInformation
        End Get
    End Property

    ''' <summary>
    ''' Sets the crop information for the fileobject.
    ''' </summary>
    Public Sub Set_SpectroscopyTable_CropInformation(ByVal CropInformation As cSpectroscopyTable.DataColumn.CropInformation,
                                                     Optional ByVal SaveSFXFile As Boolean = True)

        ' Save the information for this file.
        Me._SpectroscopyTable_CropInformation = CropInformation

        ' now add the crop information to all the columns
        If Not Me.SpectroscopyTable Is Nothing Then

            For Each Col As cSpectroscopyTable.DataColumn In Me.SpectroscopyTable.Columns.Values
                Col.CurrentCropInformation = Me._SpectroscopyTable_CropInformation
            Next

        End If

        ' Store an SFX file?
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()

    End Sub

    ''' <summary>
    ''' Removes an entry from the crop-info list in the file-object.
    ''' </summary>
    Public Sub Remove_SpectroscopyTable_CropInformation(Optional ByVal SaveSFXFile As Boolean = True)

        ' Remove the crop information object from the FileObject
        Me._SpectroscopyTable_CropInformation = New cSpectroscopyTable.DataColumn.CropInformation

        ' Remove it from the underlying spectroscopy table.
        If Not Me.SpectroscopyTable Is Nothing Then
            Me.SpectroscopyTable.SetCropInformation(New cSpectroscopyTable.DataColumn.CropInformation)
        End If

        ' Clear the preview images, since these show cropped preview.
        Me.ClearAllPreviewImages()

        ' Save the stuff?
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()

    End Sub

    ''' <summary>
    ''' Clears all crop-infos added.
    ''' </summary>
    Public Sub ClearAllCropInformation(Optional ByVal SaveSFXFile As Boolean = True)
        Me.Remove_SpectroscopyTable_CropInformation(SaveSFXFile)
    End Sub

#End Region

#Region "Spectroscopy Columns Added/Changed"

    ''' <summary>
    ''' Reference to the SpectroscopyTable object, connected to this FileObject,
    ''' if the FileType is SpectroscopyTable.
    ''' </summary>
    Public SpectroscopyTable As cSpectroscopyTable = Nothing

    ''' <summary>
    ''' Adds a SpectroscopyColumn.
    ''' Returns the new name of the column, after adding it the the file.
    ''' The name may change, if a column with the same name exists already.
    ''' </summary>
    Public Function AddSpectroscopyColumn(ByRef Column As cSpectroscopyTable.DataColumn,
                                          Optional ByVal SaveSFXFile As Boolean = True,
                                          Optional ByVal RaiseFileObjectChange As Boolean = True) As String

        ' Check, if we already own a SpectroscopyTable object.
        ' This is initially not the case, to store memory in ScanImage-Objects.
        If Me.SpectroscopyTable Is Nothing Then Me.SpectroscopyTable = New cSpectroscopyTable

        ' Get a hard-copy of the column, to rename it, etc.
        Dim ColumnCopy As cSpectroscopyTable.DataColumn = Column.GetCopy

        ' Check, if the Name already exists, if yes, then add a number in the end.
        Dim iCounter As Integer = 2
        Dim sNameWithoutCounter As String = ColumnCopy.Name
        Do While cSpectroscopyTable.CheckIfColumnNameAlreadyExists(Me.SpectroscopyTable.Columns, ColumnCopy.Name)
            ColumnCopy.Name = sNameWithoutCounter & " (" & iCounter & ")"
            iCounter += 1
        Loop

        ' Add the current Crop-Informations
        ColumnCopy.CurrentCropInformation = Me._SpectroscopyTable_CropInformation

        ' Adds the spectroscopy-column to the storage.
        Me.SpectroscopyTable.Columns.Add(ColumnCopy.Name, ColumnCopy)

        ' Save the file as SFX?
        If SaveSFXFile Then Me.SaveChangesAsFile()

        ' Raise an event?
        If RaiseFileObjectChange Then Me.OnFileObjectChanged()

        Return ColumnCopy.Name
    End Function

    ''' <summary>
    ''' Adds multiple SpectroscopyTables.
    ''' </summary>
    Public Sub AddSpectroscopyColumns(ByRef ColumnList As List(Of cSpectroscopyTable.DataColumn),
                                      Optional ByVal SaveSFXFile As Boolean = True)

        For Each Col As cSpectroscopyTable.DataColumn In ColumnList
            Me.AddSpectroscopyColumn(Col, False, False)
        Next
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()
    End Sub

    ''' <summary>
    ''' Removes an entry from spectroscopy-table list in the file-object,
    ''' if it has been generated by SpectraFox
    ''' </summary>
    Public Function RemoveSpectroscopyColumn(ByRef Column As cSpectroscopyTable.DataColumn,
                                             Optional ByVal SaveSFXFile As Boolean = True) As Boolean

        If Not Column.IsSpectraFoxGenerated Then Return False

        Me.SpectroscopyTable.Columns.Remove(Column.Name)

        ' Save the file?
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()

        Return True
    End Function

    ''' <summary>
    ''' Clears all spectroscopycolumns added.
    ''' </summary>
    Public Sub ClearAllSpectroscopyColumns(Optional ByVal SaveSFXFile As Boolean = True)

        Dim KeysToRemove As New List(Of String)
        For Each Col As cSpectroscopyTable.DataColumn In Me.SpectroscopyTable.Columns.Values
            If Col.IsSpectraFoxGenerated Then KeysToRemove.Add(Col.Name)
        Next

        ' Remove the columns!
        For Each Key As String In KeysToRemove
            Me.SpectroscopyTable.Columns.Remove(Key)
        Next

        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()
    End Sub


    ''' <summary>
    ''' Returns the saved DataColumn-List of Names.
    ''' </summary>
    Public Function GetColumnNameList() As List(Of String)
        If Me.SpectroscopyTable Is Nothing Then Return New List(Of String)
        Return Me.SpectroscopyTable.Columns.Keys.ToList
    End Function

#End Region

#Region "Scan Images Added"

    ''' <summary>
    ''' Reference to the ScanImage object, connected to this FileObject,
    ''' if the FileType is ScanImage.
    ''' </summary>
    Public ScanImage As cScanImage = Nothing

    ''' <summary>
    ''' Adds a ScanChannel.
    ''' Returns the new name of the channel, after adding it the the file.
    ''' The name may change, if a channel with the same name exists already.
    ''' </summary>
    Public Function AddScanChannel(ByRef Channel As cScanImage.ScanChannel,
                                   Optional ByVal SaveSFXFile As Boolean = True,
                                   Optional ByVal RaiseFileObjectChange As Boolean = True) As String

        ' Check, if we already own a ScanImage object.
        ' This is initially not the case, to store memory in SpectroscopyTable-Objects.
        If Me.ScanImage Is Nothing Then Me.ScanImage = New cScanImage

        Dim ChannelCopy As cScanImage.ScanChannel = Channel.GetCopy

        ' Check, if the Name already exists, if yes, then add a number in the end.
        Dim iCounter As Integer = 2
        Dim sNameWithoutCounter As String = ChannelCopy.Name
        Do While cScanImage.CheckIfChannelNameAlreadyExists(Me.ScanImage.ScanChannels, ChannelCopy.Name)
            ChannelCopy.Name = sNameWithoutCounter & " (" & iCounter & ")"
            iCounter += 1
        Loop

        ' Adds the spectroscopy-column to the storage.
        Me.ScanImage.ScanChannels.Add(ChannelCopy.Name, ChannelCopy)

        ' Save the file as SFX?
        If SaveSFXFile Then Me.SaveChangesAsFile()

        ' Raise an event?
        If RaiseFileObjectChange Then Me.OnFileObjectChanged()

        Return ChannelCopy.Name
    End Function

    ''' <summary>
    ''' Adds multiple ScanChannels.
    ''' </summary>
    Public Sub AddScanChannels(ByRef ChannelList As List(Of cScanImage.ScanChannel),
                               Optional ByVal SaveSFXFile As Boolean = True)
        For Each Chan As cScanImage.ScanChannel In ChannelList
            Me.AddScanChannel(Chan, False, False)
        Next
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()
    End Sub

    ''' <summary>
    ''' Removes an entry from the added scan-channel list in the file-object.
    ''' </summary>
    Public Function RemoveScanChannel(ByRef Channel As cScanImage.ScanChannel,
                                      Optional ByVal SaveSFXFile As Boolean = True) As Boolean

        If Not Channel.IsSpectraFoxGenerated Then Return False

        Me.ScanImage.ScanChannels.Remove(Channel.Name)

        ' Save the file?
        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()

        Return True
    End Function

    ''' <summary>
    ''' Clears all scan-channels added.
    ''' </summary>
    Public Sub ClearAllScanChannels(Optional ByVal SaveSFXFile As Boolean = True)
        Dim KeysToRemove As New List(Of String)
        For Each Chan As cScanImage.ScanChannel In Me.ScanImage.ScanChannels.Values
            If Chan.IsSpectraFoxGenerated Then KeysToRemove.Add(Chan.Name)
        Next

        ' Remove the columns!
        For Each Key As String In KeysToRemove
            Me.ScanImage.ScanChannels.Remove(Key)
        Next

        If SaveSFXFile Then Me.SaveChangesAsFile()
        Me.OnFileObjectChanged()
    End Sub

    ''' <summary>
    ''' Returns the saved ScanChannel-List of names.
    ''' </summary>
    Public Function GetScanChannelNameList() As List(Of String)
        If Me.ScanImage Is Nothing Then Return New List(Of String)
        Return Me.ScanImage.ScanChannels.Keys.ToList
    End Function

    ''' <summary>
    ''' Returns the saved ScanChannel-List.
    ''' </summary>
    Public Function GetScanChannelList() As Dictionary(Of String, cScanImage.ScanChannel)
        If Me.ScanImage Is Nothing Then Return New Dictionary(Of String, cScanImage.ScanChannel)
        Return Me.ScanImage.ScanChannels
    End Function


#End Region

#Region "Grid Files Added"

    ''' <summary>
    ''' Reference to the GridFile object, connected to this FileObject,
    ''' if the FileType is GridFile.
    ''' </summary>
    Public GridFile As cGridFile = Nothing


    ''' <summary>
    ''' Returns the saved GridFile Spectroscopy-List of names.
    ''' </summary>
    Public Function GetGridSpectroscopyTableNameList() As List(Of String)
        If Me.GridFile Is Nothing Then Return Nothing
        Return Me.GridFile.ChannelsRecorded
    End Function

    ''' <summary>
    ''' Returns the saved GridFile Spectroscopy-List.
    ''' </summary>
    Public Function GetGridSpectroscopyTableList() As List(Of cSpectroscopyTable)
        If Me.GridFile Is Nothing Then Return Nothing
        Return Me.GridFile.SpectroscopyTables
    End Function

#End Region


#End Region

#Region "Copy File-Object"

    ''' <summary>
    ''' Returns a hard copy, NOT just a reference to a fileobject.
    ''' </summary>
    Public Function GetCopy() As cFileObject
        Dim NewFileObject As New cFileObject
        With NewFileObject
            .FileType = Me.FileType
            .ImportRoutine = Me.ImportRoutine
            .FullFileNameInclPath = Me.FullFileNameInclPath
            ._DisplayName = Me._DisplayName

            .LastFileChange = Me.LastFileChange
            .RecordDate = Me.RecordDate
            .RecordLocation_X = Me.RecordLocation_X
            .RecordLocation_Y = Me.RecordLocation_Y
            .RecordLocation_Z = Me.RecordLocation_Z
            .ScanImageRange_X = Me.ScanImageRange_X
            .ScanImageRange_Y = Me.ScanImageRange_Y
            .MeasurementDimensions = Me.MeasurementDimensions
            ._ExtendedComment = Me._ExtendedComment
            ._SourceFileComment = Me._SourceFileComment
            .SpecialLocationList.AddRange(Me.SpecialLocationList)
            If Me._FileObjectDescribedByMultipleFiles IsNot Nothing Then
                ._FileObjectDescribedByMultipleFiles.AddRange(Me._FileObjectDescribedByMultipleFiles)
            End If

            ' Copy SpectroscopyTable information
            ._SpectroscopyTable_CropInformation = Me._SpectroscopyTable_CropInformation.GetCopy()
            .SpectroscopyTable = Me.SpectroscopyTable

            ' Copy ScanImage information
            .ScanImage = Me.ScanImage

            ' Copy GridFile information
            .GridFile = Me.GridFile
        End With
        Return NewFileObject
    End Function

#End Region

#Region "File-Object loading"

    ''' <summary>
    ''' Marks, if the file-object contains just cache information,
    ''' or if it is a fully loaded file-object.
    ''' 
    ''' This information is necessary,
    ''' </summary>
    Public Property ContainsOnlyCacheInformation As Boolean = True

#End Region

#Region "SFC - spectrafox file buffer cache XML-line"

    ''' <summary>
    ''' Get a fileobject from a single-line XML-cache.
    ''' </summary>
    Public Shared Function GetFileObjectFromSingleXMLCacheLine(ByRef XMLReader As Xml.XmlReader) As cFileObject

        Dim FO As New cFileObject
        FO.ContainsOnlyCacheInformation = True

        Try

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

                                Case "Properties"

                                    Dim FileName As String = ""
                                    Dim Path As String = ""

                                    While .MoveToNextAttribute
                                        Select Case .Name
                                            Case "FileName"
                                                FileName = .Value
                                            Case "Path"
                                                Path = .Value
                                            Case "DisplayName"
                                                FO._DisplayName = .Value
                                            Case "FileType"
                                                FO.FileType = GetFileTypeFromString(.Value)
                                            Case "ImportRoutine"

                                                ' Try getting the import routine
                                                Try
                                                    FO.ImportRoutine = Type.GetType(.Value)
                                                Catch ex As Exception
                                                    Trace.WriteLine("FileObject XML loading: error determining the import routine ...")
                                                    Return Nothing
                                                End Try

                                            Case "LastFileChange"
                                                FO.LastFileChange = Convert.ToDateTime(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            Case "RecordDate"
                                                FO.RecordDate = Convert.ToDateTime(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            Case "RecordLocation_X"
                                                FO.RecordLocation_X = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            Case "RecordLocation_Y"
                                                FO.RecordLocation_Y = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            Case "RecordLocation_Z"
                                                FO.RecordLocation_Z = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            Case "ScanImageRange_X"
                                                FO.ScanImageRange_X = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            Case "ScanImageRange_Y"
                                                FO.ScanImageRange_Y = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            Case "MeasurementDimensions"
                                                FO.MeasurementDimensions = .Value

                                        End Select
                                    End While

                                    FO.FullFileNameInclPath = Path & System.IO.Path.DirectorySeparatorChar & FileName

                                Case "PreviewImage"

                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then

                                        Dim PreviewImageID As String = String.Empty

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "ImageID"
                                                    PreviewImageID = .Value
                                            End Select
                                        End While

                                        ' Get the data from the innerxml
                                        Try
                                            Dim ImageBytes As Byte() = Convert.FromBase64String(.ReadString())
                                            Dim PreviewImage As Image = cImageConverter.ByteArrayToImage(ImageBytes)

                                            ' Save newly imported preview image
                                            If PreviewImageID <> String.Empty Then FO.SetPreviewImageStorage(PreviewImageID, PreviewImage)
                                        Catch ex As Exception
                                            Debug.WriteLine("Image import from cache file failed: " & ex.Message)
                                        End Try

                                    End If

                                Case "SpectroscopyDataColumn"

                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then
                                        ' Container for the added column.
                                        Dim DC As New cSpectroscopyTable.DataColumn

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Name"
                                                    DC.Name = .Value
                                                Case "InSourceFile"
                                                    DC.IsSpectraFoxGenerated = Convert.ToBoolean(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            End Select
                                        End While

                                        ' Add the data-column to the file object.
                                        FO.AddSpectroscopyColumn(DC, False, False)
                                    End If

                                Case "ScanImageChannel"

                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then
                                        ' Container for the added channel.
                                        Dim SC As New cScanImage.ScanChannel

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Name"
                                                    SC.Name = .Value
                                                Case "InSourceFile"
                                                    SC.IsSpectraFoxGenerated = Convert.ToBoolean(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            End Select
                                        End While

                                        ' Add the data-column to the file object.
                                        FO.AddScanChannel(SC, False, False)
                                    End If

                                Case "SourceFileComment"

                                    ' Save the source file comment.
                                    FO._SourceFileComment = .ReadElementContentAsString

                                Case "FileObjectDescribedByDataFile"

                                    ' Read the list that is storing the information
                                    ' which files are described by this file object.
                                    ' This is necessary for e.g. omicron files,
                                    ' where each channel is stored in a separate file.

                                    ' go through all attributes
                                    If .AttributeCount > 0 Then

                                        Dim FileName As String = String.Empty

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "FileName"
                                                    FileName = .Value
                                            End Select
                                        End While

                                        ' Add the file to the list
                                        If FO._FileObjectDescribedByMultipleFiles Is Nothing Then FO._FileObjectDescribedByMultipleFiles = New List(Of String)
                                        FO._FileObjectDescribedByMultipleFiles.Add(FileName)

                                    End If

                                Case "SpecialLocation"

                                    ' Special locations are used to store positions in images
                                    ' or spectra. Such as the record location of grid-spectra
                                    ' in a single grid file.

                                    ' go through all attributes
                                    If .AttributeCount > 0 Then

                                        Dim X As Double = Double.NaN
                                        Dim Y As Double = Double.NaN
                                        Dim Z As Double = Double.NaN

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "X"
                                                    X = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "Y"
                                                    Y = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "Z"
                                                    Z = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            End Select
                                        End While

                                        If Not Double.IsNaN(X) AndAlso Not Double.IsNaN(Y) AndAlso Not Double.IsNaN(Z) Then
                                            FO.SpecialLocationList.Add(New cNumericalMethods.Point3D(X, Y, Z))
                                        End If

                                    End If

                            End Select

                    End Select

                Loop
            End With
        Catch ex As Exception
            FO = Nothing
            Debug.WriteLine("FileObject could not be loaded from XMLLine: " & ex.Message)
        End Try

        Return FO
    End Function

    ''' <summary>
    ''' Get a fileobject from a single-line XML-cache.
    ''' </summary>
    Public Sub WriteFileObjectToSingleXMLCacheLine(ByRef XMLWriter As Xml.XmlTextWriter)

        With XMLWriter
            ' Write an element for each data-column
            .WriteStartElement("FileObject")

            .WriteStartElement("Properties")
            .WriteAttributeString("FileName", Me.FileNameWithoutPath)
            .WriteAttributeString("DisplayName", Me._DisplayName)
            .WriteAttributeString("Path", Me.Path)
            .WriteAttributeString("FileType", Me.FileType.ToString)
            .WriteAttributeString("ImportRoutine", Me.ImportRoutine.ToString)
            .WriteAttributeString("LastFileChange", Me.LastFileChange.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("RecordDate", Me.RecordDate.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("RecordLocation_X", Me.RecordLocation_X.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("RecordLocation_Y", Me.RecordLocation_Y.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("RecordLocation_Z", Me.RecordLocation_Z.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ScanImageRange_X", Me.ScanImageRange_X.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ScanImageRange_Y", Me.ScanImageRange_Y.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("MeasurementDimensions", Me.MeasurementDimensions.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteEndElement()

            ' Save the source file comment
            .WriteStartElement("SourceFileComment")
            .WriteString(Me.SourceFileComment)
            .WriteEndElement()

            ' Store the ignore file list for FileObjects consisting out of data of multiple files.
            If Not Me._FileObjectDescribedByMultipleFiles Is Nothing Then
                .WriteStartElement("FileObjectDescribedByMultipleFiles")
                For Each FileName As String In Me._FileObjectDescribedByMultipleFiles
                    .WriteStartElement("FileObjectDescribedByDataFile")
                    .WriteAttributeString("FileName", FileName)
                    .WriteEndElement()
                Next
                .WriteEndElement()
            End If

            '######################

            ' Begin the section of the additional data columns
            If Not Me.SpectroscopyTable Is Nothing Then
                .WriteStartElement("SpectroscopyDataColumns")
                For Each DC As cSpectroscopyTable.DataColumn In Me.SpectroscopyTable.Columns.Values
                    .WriteStartElement("SpectroscopyDataColumn")
                    .WriteAttributeString("Name", DC.Name)
                    .WriteAttributeString("InSourceFile", DC.IsSpectraFoxGenerated.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteEndElement()
                Next
                .WriteEndElement()
            End If

            ' Begin the section of the additional scan channels
            If Not Me.ScanImage Is Nothing Then
                .WriteStartElement("ScanImageChannels")
                For Each SCKV As KeyValuePair(Of String, cScanImage.ScanChannel) In Me.ScanImage.ScanChannels
                    .WriteStartElement("ScanImageChannel")
                    .WriteAttributeString("Name", SCKV.Key)
                    .WriteAttributeString("InSourceFile", SCKV.Value.IsSpectraFoxGenerated.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteEndElement()
                Next
                .WriteEndElement()
            End If

            ' Begin the section of the grid files
            If Not Me.GridFile Is Nothing Then
                .WriteStartElement("GridFileChannels")
                ' Write all initially recorded grid file channels
                For Each ChannelName As String In Me.GridFile.ChannelsRecorded
                    .WriteStartElement("GridFileChannel")
                    .WriteAttributeString("Name", ChannelName)
                    .WriteAttributeString("InSourceFile", False.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteEndElement()
                Next
                .WriteEndElement()
            End If

            If Me.SpecialLocationList IsNot Nothing AndAlso Me.SpecialLocationList.Count > 0 Then
                .WriteStartElement("SpecialLocationList")
                ' Write all locations.
                For Each Location As cNumericalMethods.Point3D In Me.SpecialLocationList
                    .WriteStartElement("SpecialLocation")
                    .WriteAttributeString("X", Location.x.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteAttributeString("Y", Location.y.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteAttributeString("Z", Location.z.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteEndElement()
                Next
                .WriteEndElement()
            End If

            ' Begin the section of the preview images stored
            .WriteStartElement("PreviewImageStorage")
            Dim ImageAsByte As Byte()
            For Each PreviewImageKV As KeyValuePair(Of String, Image) In Me._PreviewImageStorage
                If PreviewImageKV.Value Is Nothing Then Continue For
                ' Write an element for each preview-image
                .WriteStartElement("PreviewImage")
                .WriteAttributeString("ImageID", PreviewImageKV.Key)
                ImageAsByte = cImageConverter.ImageToByteArray(PreviewImageKV.Value)
                .WriteBase64(ImageAsByte, 0, ImageAsByte.Length)
                .WriteEndElement()
            Next
            .WriteEndElement()

            .WriteEndElement()
        End With

    End Sub

#End Region

#Region "Creates a preview image for the given columns."
    ''' <summary>
    ''' Uses ZedGraph to Plot a quick 2D Image of the Data.
    ''' </summary>
    Public Function GetSpectroscopyTablePreviewImage(ByVal XColumnName As String,
                                                     ByVal YColumnName As String,
                                                     ByVal Width As Integer,
                                                     ByVal Height As Integer,
                                                     Optional ByVal ScaleXLog As Boolean = False,
                                                     Optional ByVal ScaleYLog As Boolean = False,
                                                     Optional ByVal ReducePointsForFastPaint As Boolean = False) As Image
        Dim OutputImage As Image = Nothing
        Dim StepSize As Double

        ' Create the identification key of the cache image
        ' by including the settings of the preview image.
        Dim CacheImageKey As String = XColumnName & "," & YColumnName & ";"
        If ScaleXLog Then
            CacheImageKey &= "log,"
        Else
            CacheImageKey &= "lin,"
        End If
        If ScaleYLog Then
            CacheImageKey &= "log"
        Else
            CacheImageKey &= "lin"
        End If

        ' temporary variable that decides whether to use the cached version of the image
        Dim UseCacheImage As Boolean = False

        ' Look up the BaseFileObject for a preview image already created for these channels
        If Me.PreviewImageStorage.ContainsKey(CacheImageKey) Then
            ' Check if the dimensions of the cached image are compatible (of the same size)
            ' If we can not access the image at all, then something went wrong when storing it.
            Try
                If Me.PreviewImageStorage(CacheImageKey).Size = New Size(Width, Height) Then
                    OutputImage = Me.PreviewImageStorage(CacheImageKey)
                    UseCacheImage = True
                End If
            Catch ex As Exception
                Trace.WriteLine("#TRACE: cFileObject.GetPreviewImage: stumbled upon a broken cached image ... " & Me.FileNameWithoutPath)
                UseCacheImage = False
                OutputImage = Nothing
            End Try

        End If

        If Not UseCacheImage Then

            ' Create zedgraph-Object
            Dim list As New ZedGraph.PointPairList
            Dim oGraph As New ZedGraph.GraphPane

            ' Load the corresponding spectroscopy table object
            Dim oSpectroscopyTable As cSpectroscopyTable = Nothing
            cFileImport.GetSpectroscopyFile(Me, oSpectroscopyTable)

            ' Check, if file has been loaded successfully.
            If Not oSpectroscopyTable Is Nothing Then
                Try

                    ' Check, if Column exists:
                    If Not oSpectroscopyTable.ColumnExists(XColumnName) Or Not oSpectroscopyTable.ColumnExists(YColumnName) Then Return My.Resources.cancel

                    ' Extract Count of Values
                    Dim XValues As ReadOnlyCollection(Of Double) = oSpectroscopyTable.Column(XColumnName).Values
                    Dim YValues As ReadOnlyCollection(Of Double) = oSpectroscopyTable.Column(YColumnName).Values
                    Dim ValuesCount As Integer = XValues.Count

                    ' Reduces the shown number of Points to 3000, if wanted.
                    If ReducePointsForFastPaint And ValuesCount > 3000 Then
                        StepSize = ValuesCount / 3000
                    Else
                        StepSize = 1
                    End If

                    For j As Double = 0 To ValuesCount - 1 Step StepSize
                        ' Consider non-integer step-size, but check, if values are still in range,
                        ' since rounding will allow values of i higher than ValuesCount.
                        Dim i As Integer = CInt(j)
                        If i <= ValuesCount And Not Double.IsNaN(XValues(i)) And Not Double.IsNaN(YValues(i)) Then
                            list.Add(XValues(i), YValues(i))
                        End If
                    Next
                    With oGraph
                        Dim oLine As ZedGraph.LineItem = .AddCurve("", list, Color.Black, ZedGraph.SymbolType.Circle)
                        oLine.Line.IsVisible = False
                        oLine.Symbol.Fill = New ZedGraph.Fill(Color.Black)
                        .Title.Text = ""

                        '#################
                        ' X axis settings
                        With .XAxis
                            .Scale.IsVisible = False
                            With .Title

                                ' Generate XAxis title
                                Dim XAxisTitle As New Text.StringBuilder
                                XAxisTitle.AppendLine(oSpectroscopyTable.Column(XColumnName).AxisTitle)
                                XAxisTitle.Append(cUnits.GetFormatedValueString(oSpectroscopyTable.Column(XColumnName).GetMinimumValueOfColumn, 1))
                                XAxisTitle.Append(" → ")
                                XAxisTitle.Append(cUnits.GetFormatedValueString(oSpectroscopyTable.Column(XColumnName).GetMaximumValueOfColumn, 1))

                                ' Set the text
                                .Text = XAxisTitle.ToString
                                .FontSpec.Size = 32
                                .Gap = 0
                            End With
                            If ScaleXLog Then
                                .Type = ZedGraph.AxisType.Log
                            Else
                                .Type = ZedGraph.AxisType.Linear
                            End If

                        End With

                        '#################
                        ' Y axis settings
                        With .YAxis
                            .Scale.IsVisible = False
                            With .Title

                                ' Generate YAxis title
                                Dim YAxisTitle As New Text.StringBuilder
                                YAxisTitle.AppendLine(oSpectroscopyTable.Column(YColumnName).AxisTitle)
                                YAxisTitle.Append(cUnits.GetFormatedValueString(oSpectroscopyTable.Column(YColumnName).GetMinimumValueOfColumn, 1))
                                YAxisTitle.Append(" → ")
                                YAxisTitle.Append(cUnits.GetFormatedValueString(oSpectroscopyTable.Column(YColumnName).GetMaximumValueOfColumn, 1))

                                ' Set the text
                                .Text = YAxisTitle.ToString
                                .FontSpec.Size = 32
                                .Gap = 0
                            End With
                            If ScaleYLog Then
                                .Type = ZedGraph.AxisType.Log
                            Else
                                .Type = ZedGraph.AxisType.Linear
                            End If

                        End With
                        .AxisChange()
                    End With

                    ' Security checks of the image.
                    If Width <= 0 Then Width = 1
                    If Height <= 0 Then Height = 1

                    ' Set the new output image
                    OutputImage = oGraph.GetImage(Width, Height, 150, False)

                    ' Save the new output image to the cache.
                    Me.SetPreviewImageStorage(CacheImageKey, OutputImage)

                    ' Raise the file-object changed event.
                    Me.OnFileObjectChanged()

                Catch ex As Exception
                    MessageBox.Show("Plotting of the data failed!" & vbCrLf & ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If

        End If

        If OutputImage Is Nothing Then OutputImage = New Bitmap(1, 1)
        Return OutputImage
    End Function
#End Region

#Region "Creates a preview image for the selected ScanImage-channel."
    ''' <summary>
    ''' Creates a preview-image of the scan-channel
    ''' </summary>
    Public Function GetScanChannelPreviewImage(ByVal ChannelName As String,
                                               ByVal Width As Integer,
                                               ByVal Height As Integer) As Image
        Dim OutputImage As Image = Nothing

        ' Preview-Image-Cache
        Dim CacheImageKey As String = ChannelName
        Dim UseCacheImage As Boolean = False

        ' Look up the BaseFileObject for a preview image already created for these channels
        If Me.PreviewImageStorage.ContainsKey(CacheImageKey) Then

            ' Check if the dimensions of the cached image are compatible (of the same size).
            ' For scan channels we usually just plot symmetric pictures, so here either the width, or the height may be correct.
            ' If we can not access the image at all, then something went wrong when storing it.
            Try
                If Me.PreviewImageStorage(CacheImageKey).Size.Width = Width Or Me.PreviewImageStorage(CacheImageKey).Size.Height = Height Then
                    OutputImage = Me.PreviewImageStorage(CacheImageKey)
                    UseCacheImage = True
                End If
            Catch ex As Exception
                Trace.WriteLine("#TRACE: cFileObject.GetPreviewImage: stumbled upon a broken cached image ... " & Me.FileNameWithoutPath)
                UseCacheImage = False
                OutputImage = Nothing
            End Try

        End If

        If Not UseCacheImage Then
            Try

                ' Fetching the ScanImage.
                Dim oScanImage As cScanImage = Nothing
                cFileImport.GetScanImageFile(Me, oScanImage)

                ' Check, if file has been loaded successfully.
                If oScanImage Is Nothing Then Return Nothing

                Dim ScanImagePlot As New cScanImagePlot(oScanImage)
                ScanImagePlot.ColorScheme = cColorScheme.Gray
                If oScanImage.ScanChannels.ContainsKey(ChannelName) Then
                    Dim MaxValue As Double = oScanImage.ScanChannels(ChannelName).GetMaximumValue
                    Dim MinValue As Double = oScanImage.ScanChannels(ChannelName).GetMinimumValue
                    OutputImage = ScanImagePlot.CreateImage(MaxValue,
                                                            MinValue,
                                                            ChannelName,
                                                            Width, Height, False)

                    ' Save the new output image to the cache.
                    Me.SetPreviewImageStorage(CacheImageKey, OutputImage)
                End If
            Catch ex As Exception
                MessageBox.Show("Plotting of the preview-image for a scan-channel failed!" & vbCrLf & ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        If OutputImage Is Nothing Then OutputImage = New Bitmap(1, 1)
        Return OutputImage
    End Function
#End Region

#Region "SFX File Saving"

    ''' <summary>
    ''' This functions saves all changes to an extension file
    ''' at the path of the Mother-FileObject-File itself. (Extension .sfx).
    ''' It has the same name, just added with the additional file-extension.
    ''' All changes made by the software, and the data contained in this file-object are saved here.
    ''' </summary>
    Public Sub SaveChangesAsFile(Optional ByVal RaiseFileObjectChanged As Boolean = False)

        ' If the file-object is not fully loaded, then saving the cache-file would create a broken SFX file!
        ' In that case, load the file!
        If Me.ContainsOnlyCacheInformation Then
            GetFileObjectFromPath(Me)
        End If

        Me.WriteChangesAsFile()
        If RaiseFileObjectChanged Then Me.OnFileObjectChanged()

    End Sub

    ''' <summary>
    ''' Function that is called in a separate thread to save SFX-Files.
    ''' First writes the file to a temp-file, and if everything was successfull,
    ''' moves the file to the real target file-name.
    ''' </summary>
    Protected Sub WriteChangesAsFile()

        Dim TargetFileName As String = Me.FullFileNameInclPath & ".sfx"
        Dim TMPFileName As String = Me.FullFileNameInclPath & ".sfx.~TMP~"
        Dim BackupFileName As String = Me.FullFileNameInclPath & ".sfx.~bak~"

        Try
            ' Select the File-Encoding
            Dim enc As New System.Text.UnicodeEncoding

            ' Create the XmlTextWriter object
            Using XMLobj As New Xml.XmlTextWriter(TMPFileName, enc)

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

                    ' Begin the base file description
                    ' Write properties of the base-file-object
                    .WriteStartElement("BaseFileInformation")
                    .WriteElementString("Name", Me.FileNameWithoutPath)
                    .WriteElementString("Type", Me.FileType.ToString)
                    '.WriteElementString("DetailedType", Me.DetailedFileType.ToString)
                    .WriteEndElement()

                    ' Begin the section of the additionally created data-columns
                    If Not Me.SpectroscopyTable Is Nothing Then
                        .WriteStartElement("SpectroscopyColumnsAdded")
                        For Each Col As cSpectroscopyTable.DataColumn In Me.SpectroscopyTable.Columns.Values
                            If Not Col.IsSpectraFoxGenerated Then Continue For

                            ' Write an element for each data-column
                            .WriteStartElement("SpectroscopyColumn")
                            .WriteAttributeString("Name", Col.Name)
                            .WriteAttributeString("UnitSymbol", Col.UnitSymbol)
                            .WriteAttributeString("UnitType", Col.UnitType.ToString)
                            .WriteAttributeString("ValueCount", Col.Values(True).Count.ToString(System.Globalization.CultureInfo.InvariantCulture))
                            For j As Integer = 0 To Col.Values(True).Count - 1 Step 1
                                .WriteString(Col.Values(True)(j).ToString("E5", System.Globalization.CultureInfo.InvariantCulture))
                                .WriteString(";")
                            Next
                            .WriteEndElement()
                        Next
                        .WriteEndElement()
                    End If

                    ' Begin the section of the crop informations stored for spectroscopy tables.
                    .WriteStartElement("SpectroscopyTable_CropInformation")
                    .WriteAttributeString("MaxIndexIncl", Me._SpectroscopyTable_CropInformation.MaxIndexIncl.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteAttributeString("MinIndexIncl", Me._SpectroscopyTable_CropInformation.MinIndexIncl.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteEndElement()

                    ' Begin the section of the additionally created scan-channels
                    If Not Me.ScanImage Is Nothing Then
                        .WriteStartElement("ScanChannelsAdded")
                        For Each Chan As cScanImage.ScanChannel In Me.ScanImage.ScanChannels.Values
                            If Not Chan.IsSpectraFoxGenerated Then Continue For
                            ' Write an element for each data-column
                            .WriteStartElement("ScanChannel")
                            .WriteAttributeString("Name", Chan.Name)
                            .WriteAttributeString("InSourceFile", Chan.IsSpectraFoxGenerated.ToString(System.Globalization.CultureInfo.InvariantCulture))
                            .WriteAttributeString("UnitSymbol", Chan.UnitSymbol)
                            .WriteAttributeString("UnitType", Chan.Unit.ToString)
                            .WriteAttributeString("ScanDirection", Chan.ScanDirection.ToString)
                            .WriteAttributeString("Bias", Chan.Bias.ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                            .WriteAttributeString("Calibration", Chan.Calibration.ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                            .WriteAttributeString("Offset", Chan.Offset.ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                            .WriteAttributeString("Columns", Chan.ScanData.ColumnCount.ToString(System.Globalization.CultureInfo.InvariantCulture))
                            .WriteAttributeString("Rows", Chan.ScanData.RowCount.ToString(System.Globalization.CultureInfo.InvariantCulture))
                            For y As Integer = 0 To Chan.ScanData.RowCount - 1 Step 1
                                For x As Integer = 0 To Chan.ScanData.ColumnCount - 1 Step 1
                                    .WriteString(Chan.ScanData(y, x).ToString("E5", System.Globalization.CultureInfo.InvariantCulture))
                                    .WriteString(";")
                                Next
                            Next
                            .WriteEndElement()
                        Next
                        .WriteEndElement()

                        ' Begin the section of the additional scan channels
                        .WriteStartElement("ScanImageFilters")
                        For Each Chan As cScanImage.ScanChannel In Me.ScanImage.ScanChannels.Values
                            For i As Integer = 0 To Chan.FilterCount - 1 Step 1
                                .WriteStartElement("ScanImageFilter")
                                .WriteAttributeString("ChannelName", Chan.Name.ToString(System.Globalization.CultureInfo.InvariantCulture))
                                .WriteAttributeString("FilterType", Chan.Filter(i).GetType.ToString)
                                .WriteAttributeString("Settings", Chan.Filter(i).FilterSettingsString)
                                .WriteEndElement()
                            Next
                        Next
                        .WriteEndElement()
                    End If

                    ' Write AdditionalComment
                    .WriteElementString("AdditionalComment", Me.ExtendedComment)

                    ' End <root>
                    .WriteEndElement()

                    ' Close the document
                    .WriteEndDocument()
                End With
            End Using

            ' If everything was ok so far, so if we are at this point,
            ' move the temporary file to the real target-file-name,
            ' and overwrite the copy.
            If System.IO.File.Exists(TargetFileName) Then System.IO.File.Move(TargetFileName, BackupFileName)
            System.IO.File.Move(TMPFileName, TargetFileName)
            If System.IO.File.Exists(BackupFileName) Then System.IO.File.Delete(BackupFileName)

        Catch ex As Exception
            'MessageBox.Show("Error writing SFX-file:" & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine("Error writing SFX-file:" & ex.Message)

            ' Move back the backup, if it existed!
            Try
                If System.IO.File.Exists(BackupFileName) Then System.IO.File.Move(BackupFileName, TargetFileName)
            Catch ex2 As Exception
                Debug.WriteLine("Error moving backup SFX-file back in place:" & ex2.Message)
            End Try
        End Try
    End Sub

#End Region

#Region "Fetch FileObject from path"

    ''' <summary>
    ''' Loads all additional information from a cache SFX file to a given file-object.
    ''' </summary>
    Public Shared Function GetFileObjectFromPath(ByRef InputFileObject As cFileObject) As cFileObject

        ' Check if a SpectraFox-extension file exists, that modifys the file-object
        Dim SpectraFoxExtensionFileName As String = InputFileObject.FullFileNameInclPath & ".sfx"
        If File.Exists(SpectraFoxExtensionFileName) Then
            Try
                If cFileImportSpectraFoxSFX.IdentifyFile(SpectraFoxExtensionFileName) Then
                    cFileImportSpectraFoxSFX.ImportFileObject(InputFileObject, SpectraFoxExtensionFileName)
                End If
            Catch ex As Exception
                Trace.WriteLine("#TRACE: cFileImport.ImportFile: SFX-File vanished during the scan ... ignoring it...")
            End Try
        End If

        ' Mark, that all the information of the file-object have been loaded.
        InputFileObject.ContainsOnlyCacheInformation = False

        Return InputFileObject
    End Function

    ''' <summary>
    ''' Identifies the file, and returns a cFileObject for a given file.
    ''' For speed reasons deliver the optional list of import routines, so
    ''' that the program does not have to load it for every file.
    ''' Also provide an external reader buffer.
    ''' </summary>
    Public Shared Function GetFileObjectFromPath(ByVal oFile As FileInfo,
                                                 Optional ByRef ReaderBuffer As String = "",
                                                 Optional ByRef ImportRoutines_SpectroscopyTables As List(Of iFileImport_SpectroscopyTable) = Nothing,
                                                 Optional ByRef ImportRoutines_ScanImages As List(Of iFileImport_ScanImage) = Nothing,
                                                 Optional ByRef ImportRoutines_GridFiles As List(Of iFileImport_GridFile) = Nothing) As cFileObject

        ' reset the file-identified status
        Dim bFileIdentified As Boolean = False
        Dim tImportRoutineType As Type = Nothing
        Dim tFileType As cFileObject.FileTypes = cFileObject.FileTypes.UNIDENTIFIED

        ' Create the file-object
        Dim oFileObject As cFileObject = Nothing

        ' Get all available import filters, if we don't get them delivered from outside the function
        If ImportRoutines_SpectroscopyTables Is Nothing Then ImportRoutines_SpectroscopyTables = cFileImport.GetAllImportRoutines_SpectroscopyTable
        If ImportRoutines_ScanImages Is Nothing Then ImportRoutines_ScanImages = cFileImport.GetAllImportRoutines_ScanImage
        If ImportRoutines_GridFiles Is Nothing Then ImportRoutines_GridFiles = cFileImport.GetAllImportRoutines_GridFile

        '####################
        ' Scan file and get all informations about the file-type.
        ' Always exit the loop, if the file has been identified.
        If Not bFileIdentified Then
            For Each ImportRoutine As iFileImport_SpectroscopyTable In ImportRoutines_SpectroscopyTables
                If bFileIdentified Then Exit For

                ' Check the file-extension
                If oFile.Extension.ToLower.EndsWith(ImportRoutine.FileExtension.ToLower) Then
                    ' Check, if the file can be interpreted by the import routine
                    If ImportRoutine.IdentifyFile(oFile.FullName, ReaderBuffer) Then
                        tImportRoutineType = ImportRoutine.GetType
                        tFileType = cFileObject.FileTypes.SpectroscopyTable
                        bFileIdentified = True
                    End If
                End If
            Next
        End If
        If Not bFileIdentified Then
            For Each ImportRoutine As iFileImport_ScanImage In ImportRoutines_ScanImages
                If bFileIdentified Then Exit For
                ' Check the file-extension
                If oFile.Extension.ToLower.EndsWith(ImportRoutine.FileExtension.ToLower) Then
                    ' Check, if the file can be interpreted by the import routine
                    If ImportRoutine.IdentifyFile(oFile.FullName, ReaderBuffer) Then
                        tImportRoutineType = ImportRoutine.GetType
                        tFileType = cFileObject.FileTypes.ScanImage
                        bFileIdentified = True
                    End If
                End If
            Next
        End If
        If Not bFileIdentified Then
            For Each ImportRoutine As iFileImport_GridFile In ImportRoutines_GridFiles
                If bFileIdentified Then Exit For
                ' Check the file-extension
                If oFile.Extension.ToLower.EndsWith(ImportRoutine.FileExtension.ToLower) Then
                    ' Check, if the file can be interpreted by the import routine
                    If ImportRoutine.IdentifyFile(oFile.FullName, ReaderBuffer) Then
                        tImportRoutineType = ImportRoutine.GetType
                        tFileType = cFileObject.FileTypes.GridFile
                        bFileIdentified = True
                    End If
                End If
            Next
        End If
        '#################

        ' If the file could get identified, then create a FileObject
        ' and store the ImportRoutine, path, and type.
        If bFileIdentified Then

            oFileObject = New cFileObject

            ' Set properties
            With oFileObject
                .FullFileNameInclPath = oFile.FullName
                .LastFileChange = oFile.LastWriteTime
                .ImportRoutine = tImportRoutineType
                .FileType = tFileType
            End With

            ' Load the rest of the file-object from the SFX-file.
            GetFileObjectFromPath(oFileObject)

        End If

        Return oFileObject
    End Function

#End Region

#Region "Dispose"

    ''' <summary>
    ''' Dispose function.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Me.eFileObjectChanged = Nothing
    End Sub

#End Region

End Class