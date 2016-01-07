Imports System.Threading
Imports Amib.Threading

''' <summary>
''' Folder Browser Class
''' </summary>
Public Class wDataBrowser
    Inherits wFormBase
    Implements iMultipleSpectroscopyTablesLoaded
    Implements iSingleSpectroscopyTableLoaded
    Implements iMultipleScanImagesLoaded
    Implements iSingleScanImageLoaded

#Region "Thread Pool Definition"

    ''' <summary>
    ''' Create the new Smart-Thread-Pool instance for this data-browser window.
    ''' </summary>
    Private SmartThreadPool As New SmartThreadPool
#End Region

#Region "Properties"
    ''' <summary>
    ''' Variable that tells, if events should be handled or ignored by the Control.
    ''' </summary>
    Private bReady As Boolean = False

    ''' <summary>
    ''' Variable, that saves the current working directory!
    ''' </summary>
    Private sWorkingDirectory As String

    ''' <summary>
    ''' If list exists and is filled, restricts the file-load from the working directory to just
    ''' the given file names in the working directory.
    ''' </summary>
    Private OnlyLoadListOfFilesFromWorkingDirectory As List(Of String)

    ''' <summary>
    ''' Current Sort-Direction of the List
    ''' </summary>
    Private SortDirection As SortOrder = SortOrder.Ascending

    ''' <summary>
    ''' Current ColumnIndex that is used to sort the list.
    ''' </summary>
    Private SortColumnIndex As Integer

    ''' <summary>
    ''' Currently loaded file-list, as Dictionary of FullPath, FileObject.
    ''' </summary>
    Private FileListDisplayed As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Currently loaded file-list. Sorted according the actual sorting-settings.
    ''' </summary>
    Private FileListDisplayed_Sorted As New List(Of cFileObject)

    ''' <summary>
    ''' Background-Thread for fetching the files from the Directory.
    ''' </summary>
    Private oFileBufferFetcher As New System.ComponentModel.BackgroundWorker

    ''' <summary>
    ''' File-importer which loads and identifies all files in the given directory.
    ''' </summary>
    Private oFileImporter As cFileImport

    ''' <summary>
    ''' Currently selected ColumnName for the X Column
    ''' </summary>
    Public ReadOnly Property PreviewImageColumnNameX As String
        Get
            If Me.SpectroscopyTableList Is Nothing Then Return ""
            Return Me.SpectroscopyTableList.PreviewImageColumnName_X
        End Get
    End Property

    ''' <summary>
    ''' Currently selected ColumnName for the Y Column
    ''' </summary>
    Public ReadOnly Property PreviewImageColumnNameY As String
        Get
            If Me.SpectroscopyTableList Is Nothing Then Return ""
            Return Me.SpectroscopyTableList.PreviewImageColumnName_Y
        End Get
    End Property

    ''' <summary>
    ''' Object for fetching the list of Spectroscopy Tables displayed in the Datagridview of this Control.
    ''' </summary>
    Private WithEvents SpectroscopyTableList As cSpectroscopyTableList

    ''' <summary>
    ''' List with preview-image-column-names to be displayed in the context-menu
    ''' </summary>
    Private ListOfPreviewImageColumns As New List(Of String)

    ''' <summary>
    ''' Object for fetching the list of ScanImage Entries displayed.
    ''' </summary>
    Private WithEvents ScanImageList As cScanImageList

    ''' <summary>
    ''' List with channel-names to be displayed in the context-menu
    ''' </summary>
    Private ListOfPreviewImageChannels As New List(Of String)

    ''' <summary>
    ''' Currently selected ChannelName for the Scan-Image Preview.
    ''' </summary>
    Public ReadOnly Property ScanImagePreviewChannel As String
        Get
            If Me.ScanImageList Is Nothing Then Return ""
            Return Me.ScanImageList.PreviewImageChannel
        End Get
    End Property

#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets raised, if a single file gets selected in the list
    ''' </summary>
    Public Event SingleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTable As cSpectroscopyTable)

    ''' <summary>
    ''' Event that gets raised, if a multiple files get selected in the list
    ''' </summary>
    Public Event MultipleSpectroscopyTablesSelected(ByRef SelectedSpectroscopyTableFileObjects As List(Of cSpectroscopyTable))

    ''' <summary>
    ''' Event that gets raised, if the nearest ScanImage for the SpectroscopyTable is searched
    ''' </summary>
    Public Event NearestScanImageInTimeForSpectroscopyTableSearched(ByRef FileObject As cFileObject)

    ''' <summary>
    ''' Event that gets raised, if a single file gets selected in the list
    ''' </summary>
    Public Event SingleScanImageSelected(ByRef SelectedScanImage As cScanImage)

    ''' <summary>
    ''' Event that gets raised, if a multiple files get selected in the list
    ''' </summary>
    Public Event MultipleScanImagesSelected(ByRef SelectedScanImageFileObjects As List(Of cScanImage))

    ''' <summary>
    ''' Event that gets raised, if a single file gets selected in the list
    ''' </summary>
    Public Event SingleFileObjectSelected(ByRef SelectedFileObject As cFileObject)

    ''' <summary>
    ''' Event that gets raised, if a multiple files get selected in the list
    ''' </summary>
    Public Event MultipleFileObjectsSelected(ByRef SelectedFileObjects As List(Of cFileObject))
#End Region

#Region "Show and ShowDialog-Shadows"
    ''' <summary>
    ''' Sub shadowing the BaseClass to recieve a working directory
    ''' before opening!
    ''' </summary>
    Public Shadows Sub Show(WorkingDirectory As String,
                            Optional ByVal OnlyLoadListOfFiles As List(Of String) = Nothing)
        Me.sWorkingDirectory = WorkingDirectory
        Me.OnlyLoadListOfFilesFromWorkingDirectory = OnlyLoadListOfFiles
        MyBase.Show()
    End Sub

    ''' <summary>
    ''' Sub shadowing the BaseClass to recieve a working directory
    ''' before opening!
    ''' </summary>
    Public Shadows Sub ShowDialog(WorkingDirectory As String,
                                  Optional ByVal OnlyLoadListOfFiles As List(Of String) = Nothing)
        Me.sWorkingDirectory = WorkingDirectory
        Me.OnlyLoadListOfFilesFromWorkingDirectory = OnlyLoadListOfFiles
        MyBase.ShowDialog()
    End Sub
#End Region

#Region "Form Load and Close"
    ''' <summary>
    ''' Function of the Form-Load Event.
    ''' </summary>
    Private Sub wDataBrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '##########################################
        ' STYLE SETTINGS

        ' Hide Test-Window Button
#If Not Debug Then
        Me.cmnuTestSingle.Visible = False
        Me.cmnuTestMultiple.Visible = False
        Me.cmnuRenormalizeDataByParameter.Visible = False
        Me.cmnuRenormalizeDataByParameterUsingLastSettings.Visible = False
        Me.ToolStripSeparator10.Visible = False
#End If

        ' Set the window title to the Working Directory
        Me.Text = Me.sWorkingDirectory

        ' Collapse the Settings-Panel
        Me.dpSettings.SlideIn(True)

        ' Set Inital Sort-Column
        Me.SortColumnIndex = Me.colFileDate.Index

        ' Set Sorting Icon
        Me.dgvFileList.Columns(SortColumnIndex).HeaderCell.SortGlyphDirection = SortDirection

        ' Set "Refresh-List"-Button to "Refresh"
        Me.SetInterfaceToLoadingListModus(False)

        ' END STYLE SETTINGS
        '##########################################

        '########################
        ' FILE BUFFER
        ' Add adresses to the File-Buffer Fetcher
        Me.oFileBufferFetcher.WorkerSupportsCancellation = True
        Me.oFileBufferFetcher.WorkerReportsProgress = True
        AddHandler Me.oFileBufferFetcher.DoWork, AddressOf Me.FileBufferFetcher_DoWork
        AddHandler Me.oFileBufferFetcher.RunWorkerCompleted, AddressOf Me.FileBufferFetcher_AllComplete
        AddHandler Me.oFileBufferFetcher.ProgressChanged, AddressOf Me.FileBufferFetcher_ReportProgress

        ' END FILE BUFFER
        '########################

        ' Get File-List for current directory
        Me.GetCurrentFileList()

        ' Set the window to ready!
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Function of the Form-Closing Event.
    ''' Abort the Closing, if the File-Buffer is fetching!
    ''' </summary>
    Private Sub wDataBrowser_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If oFileBufferFetcher.IsBusy Then
            MessageBox.Show(My.Resources.rDataBrowser.FormClose_FileFetcherRunning,
                            My.Resources.rDataBrowser.FormClose_FileFetcherRunning_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            e.Cancel = True
        End If
    End Sub
#End Region

#Region "GUI Switch during Loading!"

    ''' <summary>
    ''' Enables/disables the List-Load buttons and set's the icons.
    ''' </summary>
    Private Sub SetInterfaceToLoadingListModus(ByVal IsListLoading As Boolean)
        If IsListLoading Then
            Me.mnuRefreshList.Text = My.Resources.FileListRefresh_Cancel
            Me.mnuRefreshList.Image = My.Resources.cancel_16
            Me.mnuSettings.Enabled = False
        Else
            Me.mnuRefreshList.Text = My.Resources.FileListRefresh_RefreshList
            Me.mnuRefreshList.Image = My.Resources.reload_16
            Me.mnuSettings.Enabled = True
        End If
    End Sub

#End Region

#Region "Get File List and File Buffer Fetcher"

    ''' <summary>
    ''' Initialize a List-Refresh by the Button-Click, if the selected working directory exists.
    ''' </summary>
    Private Sub btnRefreshList_Click(sender As System.Object, e As System.EventArgs) Handles mnuRefreshList.Click
        ' If the Worker is Not Working: Start it.
        ' Otherwise act as Cancel-Button to Quit the Worker.
        If Me.oFileBufferFetcher.IsBusy Then
            'Me.oFileImporter.CancelRunningSTPFileBufferCreation()
            Me.oFileBufferFetcher.CancelAsync()
        Else
            If IO.Directory.Exists(Me.sWorkingDirectory) Then
                Me.GetCurrentFileList()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Loads the File-List for the current folder!
    ''' </summary>
    Public Sub GetCurrentFileList()
        ' Set "Refresh-List"-Button to "Cancel"
        Me.SetInterfaceToLoadingListModus(True)

        ' Makes the Progress-Bar and Text visible again.
        Me.pgProgress.Value = 0
        Me.lblProgressBar.Text = ""
        Me.MainStatusStrip.Visible = True

        ' Start the File-Buffer-Fetcher.
        If Not oFileBufferFetcher.IsBusy Then Me.oFileBufferFetcher.RunWorkerAsync()
    End Sub

    ''' <summary>
    ''' Fetches all the Files with their Contents from the Working-Directory.
    ''' </summary>
    Private Sub FileBufferFetcher_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ' Create new File-Importer-Object.
        Me.oFileImporter = New cFileImport

        ' Scan the Directory and Create a FileBuffer.
        Me.oFileImporter.CreateFileBuffer(Me.sWorkingDirectory, Me.oFileBufferFetcher, Me.OnlyLoadListOfFilesFromWorkingDirectory)
        'Me.oFileImporter.CreateFileBuffer(Me.sWorkingDirectory, Me.oFileBufferFetcher)
    End Sub

    ''' <summary>
    ''' If the whole File-Buffer got fetched, this Function initializes the Filtering
    ''' and fills the DataGridView
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FileBufferFetcher_AllComplete(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Me.bReady = False

        ' Clear the sorted File-List, to request a resort!
        Me.FileListDisplayed_Sorted.Clear()

        ' Take the current File-List and compare it to the new one.
        ' If new elements are discovered, add them at the end!
        For Each PathFileObjectPair As KeyValuePair(Of String, cFileObject) In Me.oFileImporter.CurrentFileBuffer

            ' Old element discovered! Just if file has been changed.
            If Me.FileListDisplayed.ContainsKey(PathFileObjectPair.Key) Then
                If Me.FileListDisplayed(PathFileObjectPair.Key).LastFileChange <> PathFileObjectPair.Value.LastFileChange Then
                    Me.FileListDisplayed(PathFileObjectPair.Key) = PathFileObjectPair.Value
                End If
            Else
                Me.FileListDisplayed.Add(PathFileObjectPair.Key, PathFileObjectPair.Value)
            End If

        Next

        ' Create new SpectroscopyTable-List object and ScanImage-List object
        ' from the given File-List, so that the entries can be fetched!
        Me.SpectroscopyTableList = New cSpectroscopyTableList(FileListDisplayed, Me.SmartThreadPool)
        Me.ScanImageList = New cScanImageList(FileListDisplayed, Me.SmartThreadPool)

        ' Delete Preview Image Column- and Channel-List
        Me.ListOfPreviewImageColumns.Clear()
        Me.ListOfPreviewImageChannels.Clear()

        ' Row Template Height gets set to the choosen image height
        Me.dgvFileList.RowTemplate.Height = Me.SpectroscopyTableList.PreviewImageHeigth
        Me.dgvFileList.Columns(Me.colPreviewImage.Index).Width = Me.SpectroscopyTableList.PreviewImageWidth

        ' Set the Row-Count of the List to start the loading of the DataGridView
        Me.dgvFileList.Rows.Clear()
        Me.dgvFileList.RowCount = FileListDisplayed.Count
        Me.dgvFileList.ClearSelection()

        '############################################

        ' Makes the Progress-Bar and Text invisible again.
        Me.MainStatusStrip.Visible = False

        ' Set "Refresh-List"-Button to "Refresh"
        Me.SetInterfaceToLoadingListModus(False)

        ' Activate Handlers
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Function for reporting the Progress of the File-Fetching to the User.
    ''' </summary>
    Private Sub FileBufferFetcher_ReportProgress(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
        If e.ProgressPercentage <= 100 Then
            Me.pgProgress.Value = e.ProgressPercentage
        Else
            Me.pgProgress.Value = 100
        End If
        Me.lblProgressBar.Text = Convert.ToString(e.UserState)
    End Sub

#End Region

#Region "Extract Channel- and Column-Names for the Preview-Selection"

    ''' <summary>
    ''' Adds unique entrys to the ColumnNameList
    ''' </summary>
    Private Sub AdaptPreviewImageColumnFilters(ColumnNames As List(Of String))
        Dim bValuesAdded As Boolean = False

        SyncLock Me.ListOfPreviewImageColumns
            For Each ColName As String In ColumnNames
                If Not Me.ListOfPreviewImageColumns.Contains(ColName) Then
                    Me.ListOfPreviewImageColumns.Add(ColName)
                    bValuesAdded = True
                End If
            Next
        End SyncLock
    End Sub

    ''' <summary>
    ''' Adds unique entrys to the ChannelNameList
    ''' </summary>
    Private Sub AdaptPreviewImageChannelNameFilters(ChannelNames As List(Of String))
        Dim bValuesAdded As Boolean = False
        SyncLock Me.ListOfPreviewImageChannels
            For Each CName As String In ChannelNames
                If Not Me.ListOfPreviewImageChannels.Contains(CName) Then
                    Me.ListOfPreviewImageChannels.Add(CName)
                    bValuesAdded = True
                End If
            Next
        End SyncLock
    End Sub

#End Region

#Region "Row Identification Functions and Sorting"

    ''' <summary>
    ''' Uses the current settings to sort the <code>DisplayedFileList</code>
    ''' by a certain column. ASC or DESC.
    ''' </summary>
    Private Sub SortFileListUsingCurrentSettings()
        Dim SortedFileObjects As List(Of cFileObject)

        ' Sorting ASC of the List
        Select Case Me.SortColumnIndex
            Case Me.colTime.Index
                SortedFileObjects = Me.FileListDisplayed.Values.OrderBy(Function(obj) obj.RecordDate).ToList
            Case Me.colFileDate.Index
                SortedFileObjects = Me.FileListDisplayed.Values.OrderBy(Function(obj) obj.LastFileChange).ToList
            Case Me.colFileName.Index
                SortedFileObjects = Me.FileListDisplayed.Values.OrderBy(Function(obj) obj.FileNameWithoutPath).ToList
            Case Else
                SortedFileObjects = Me.FileListDisplayed.Values.ToList
        End Select

        ' Sorting DESC -> Reverse the list
        If Me.SortDirection = SortOrder.Descending Then
            SortedFileObjects.Reverse()
        End If

        ' Set the sorted File-List
        Me.FileListDisplayed_Sorted = SortedFileObjects
    End Sub

    ''' <summary>
    ''' Returns the File-Name of the Column determined by the Row-Index.
    ''' For performance reasons, this function needs a sorted list, which must be sorted in advance.
    ''' </summary>
    Private Function GetFileNameOfColumnByRowIndex(ByVal RowIndex As Integer) As String
        Dim FileNameOfColumn As String = ""

        If Me.FileListDisplayed_Sorted.Count <> Me.FileListDisplayed.Count Then
            Debug.WriteLine("Sorted FileList empty. Resorting ...")
            Me.SortFileListUsingCurrentSettings()
        End If

        If Me.FileListDisplayed_Sorted.Count > RowIndex Then
            FileNameOfColumn = Me.FileListDisplayed_Sorted(RowIndex).FullFileNameInclPath
        End If
        Return FileNameOfColumn
    End Function


    ''' <summary>
    ''' Returns the Row-Id of the Column determined by the FileName.
    ''' </summary>
    Private Function GetRowIndexOfColumnByFileName(ByVal FullFileName As String) As Integer
        For Each Row As DataGridViewRow In Me.dgvFileList.Rows
            If Convert.ToString(Row.Cells(Me.colFullFileName.Index).Value) = FullFileName Then
                Return Row.Index
            End If
        Next
        Return 0
    End Function

    ''' <summary>
    ''' Gets a Dictionary of all selected Spectroscopy-Table-FileObjects!
    ''' </summary>
    Public Function GetSelectedSpectroscopyTableFiles() As Dictionary(Of String, cFileObject)

        Dim lSpectraList As New Dictionary(Of String, cFileObject)

        ' Go through all rows and get all SpectroscopyTable-FileObjects.
        SyncLock Me.dgvFileList
            For Each Row As DataGridViewRow In Me.dgvFileList.SelectedRows

                ' Ignore other rows.
                If Convert.ToString(Row.Cells(Me.colFileType.Index).Value) <> Convert.ToString(Convert.ToInt32(cFileObject.FileTypes.SpectroscopyTable)) Then Continue For

                Dim sFullFileName As String = Convert.ToString(Row.Cells(Me.colFullFileName.Index).Value)

                If Not Me.SpectroscopyTableList.CurrentFileList.ContainsKey(sFullFileName) Then Continue For
                lSpectraList.Add(sFullFileName, SpectroscopyTableList.CurrentFileList(sFullFileName))
            Next
        End SyncLock
        Return lSpectraList
    End Function

    ''' <summary>
    ''' Gets a Dictionary of all selected Scan-Image-FileObjects!
    ''' </summary>
    Public Function GetSelectedScanImageFiles() As Dictionary(Of String, cFileObject)

        Dim lScanImageList As New Dictionary(Of String, cFileObject)

        ' Go through all rows and get all SpectroscopyTable-FileObjects.
        SyncLock Me.dgvFileList
            For Each Row As DataGridViewRow In Me.dgvFileList.SelectedRows

                ' Ignore other rows.
                If Convert.ToString(Row.Cells(Me.colFileType.Index).Value) <> Convert.ToString(Convert.ToInt32(cFileObject.FileTypes.ScanImage)) Then Continue For

                Dim sFullFileName As String = Convert.ToString(Row.Cells(Me.colFullFileName.Index).Value)

                If Not Me.ScanImageList.CurrentFileList.ContainsKey(sFullFileName) Then Continue For
                lScanImageList.Add(sFullFileName, ScanImageList.CurrentFileList(sFullFileName))
            Next
        End SyncLock

        Return lScanImageList
    End Function

#End Region

#Region "List-Entry Interface Definition"

    ''' <summary>
    ''' Represents a List-Entry to be displayed in the File-List.
    ''' </summary>
    Public Interface iListEntry
        Property FullFileName As String
        Property FileName As String
        Property PreviewImage As Image
        Property ColumnNames As List(Of String)
        Property Comment As String
        Property RecordDate As Date
        Property MeasurementPoints As String
    End Interface

#End Region

#Region "###### Cell-Value-Fetching"

    Private CellValueFetchMutex As New Mutex

    Private RowReloadCount As New Collections.Concurrent.ConcurrentDictionary(Of Integer, Integer)

    ''' <summary>
    ''' Function that Handles the Request for Data by the File-List,
    ''' and fills the visible part of the DataGridView.
    ''' </summary>
    Private Sub dgvFileList_CellValueNeeded(sender As System.Object, e As System.Windows.Forms.DataGridViewCellValueEventArgs) _
        Handles dgvFileList.CellValueNeeded
        If Not Me.bReady Then Return

        Dim FileNameOfColumn As String = Me.GetFileNameOfColumnByRowIndex(e.RowIndex)

        If FileNameOfColumn = "" Then
            Debug.WriteLine("Recieved empty Column-Filename -> sorted list must have a failure!")
            Return
        End If

        ' Write already fetched Properties:
        Select Case e.ColumnIndex
            Case Me.colFullFileName.Index
                e.Value = FileNameOfColumn
            Case Me.colFileName.Index
                e.Value = My.Resources.rDataBrowser.File_Loading_Tag & Me.FileListDisplayed(FileNameOfColumn).FileNameWithoutPath
            Case Me.colPreviewImage.Index
                e.Value = My.Resources.loading_25
            Case Me.colFileDate.Index
                e.Value = Me.FileListDisplayed(FileNameOfColumn).LastFileChange
            Case Me.colAdditionalComment.Index
                e.Value = Me.FileListDisplayed(FileNameOfColumn).ExtendedComment
#If DEBUG Then
            Case Me.colDataPoints.Index
                SyncLock RowReloadCount
                    RowReloadCount.AddOrUpdate(e.RowIndex, 1, Function(key, oldValue) oldValue + 1)
                    e.Value = "During Debug" & vbCrLf & "Cell-Reload-Count:" & vbCrLf & RowReloadCount.GetOrAdd(e.RowIndex, 1)
                End SyncLock
#End If
            Case Me.colFileType.Index
                e.Value = Convert.ToInt32(Me.FileListDisplayed(FileNameOfColumn).FileType)
        End Select

        Dim ListEntryToShow As iListEntry = Nothing

        CellValueFetchMutex.WaitOne()
        ' Load and display more data, depending on the File-Type
        Select Case Me.FileListDisplayed(FileNameOfColumn).FileType
            Case cFileObject.FileTypes.SpectroscopyTable
                ' Load the file from disk, if it was not loaded yet.
                SyncLock SpectroscopyTableList.ListEntryList
                    If Not Me.SpectroscopyTableList.ListEntryList.ContainsKey(FileNameOfColumn) Then
                        ' Send the Load-Request to the Background-Class,
                        ' which again processes the fetch of the individual list entry.
                        Me.SpectroscopyTableList.LoadFile(FileNameOfColumn, SpectroscopyTableList, WorkItemPriority.BelowNormal)
                    Else
                        ' Write File-Properties from the Spectroscopy-File to the List
                        ListEntryToShow = SpectroscopyTableList.ListEntryList(FileNameOfColumn)
                    End If
                End SyncLock
            Case cFileObject.FileTypes.ScanImage
                ' Load the File from Disk, if it was not loaded yet.
                SyncLock ScanImageList.ListEntryList
                    If Not ScanImageList.ListEntryList.ContainsKey(FileNameOfColumn) Then
                        ' Send the Load-Request to the Background-Class
                        ScanImageList.LoadFile(FileNameOfColumn, ScanImageList, WorkItemPriority.BelowNormal)
                    Else
                        ' Write File-Properties from the ScanImage-ListEntry to the List
                        ListEntryToShow = ScanImageList.ListEntryList(FileNameOfColumn)
                    End If
                End SyncLock
        End Select
        CellValueFetchMutex.ReleaseMutex()

        ' Write File-Properties from the ScanImage-ListEntry to the List
        If Not ListEntryToShow Is Nothing Then
            With ListEntryToShow
                Select Case e.ColumnIndex
                    Case Me.colFileName.Index
                        e.Value = .FileName
                    Case Me.colTime.Index
                        e.Value = .RecordDate
                    Case Me.colComment.Index
                        e.Value = .Comment
#If Not Debug Then
                    Case Me.colDataPoints.Index
                        e.Value = .MeasurementPoints
#End If
                    Case Me.colColumnList.Index
                        Dim sChannelNames As String = ""
                        Dim iChannelCounter As Integer = 0
                        Dim iChannelMaxShow As Integer = 6
                        ' Write a list of all Columns in the Data-Table.
                        For Each ChannelName As String In .ColumnNames
                            If iChannelCounter <= iChannelMaxShow Then
                                sChannelNames &= ChannelName & vbCrLf
                            End If
                            iChannelCounter += 1
                        Next
                        If iChannelCounter > iChannelMaxShow Then
                            sChannelNames &= My.Resources.Template_AdditionalMore.Replace("%%", (iChannelCounter - iChannelMaxShow).ToString("N0"))
                        End If
                        e.Value = sChannelNames
                    Case Me.colPreviewImage.Index
                        If Not .PreviewImage Is Nothing And TypeOf .PreviewImage Is Image Then
                            e.Value = .PreviewImage
                        End If
                End Select
            End With
        End If
    End Sub
#End Region

#Region "Selection of File-List entries."

    ''' <summary>
    ''' Selection in Spectroscopy-Table-Filelist Changed.
    ''' Send Event with the selected List.
    ''' </summary>
    Private Sub dgvSpectroscopyFileList_SelectionChanged(sender As System.Object, e As EventArgs) Handles dgvFileList.SelectionChanged
        If Not Me.bReady Then Return

        '#######################################################################
        ' 
        '               REGION: Selected SpectroscopyTables

        Dim SelectedSpectroscopyTableFiles As Dictionary(Of String, cFileObject) = Me.GetSelectedSpectroscopyTableFiles

        ' Enable or Disable Buttons depending on a single or multiple Selection.
        Select Case SelectedSpectroscopyTableFiles.Count
            Case 0
                Me.cmnuOpenExportWizard.Enabled = False
                Me.cmnuCopyDataToClipboard.Enabled = False
                Me.cmnuCopyDataToClipboardOriginCompatible.Enabled = False
                Me.cmnuSmoothData.Enabled = False
                Me.cmnuSmoothDataUsingLastSettings.Enabled = False
                Me.cmnuAverageDataMultipleFiles.Enabled = False
                Me.cmnuAverageDataSingleFile.Enabled = False
                Me.cmnuRenormalizeData.Enabled = False
                Me.cmnuRenormalizeDataUsingLastSettings.Enabled = False
                'Me.cmnuInterpolateData.Enabled = False
                Me.cmnuShiftColumnValues.Enabled = False
                Me.cmnuNormalizeData.Enabled = False
                Me.cmnuNormalizeDataUsingLastSettings.Enabled = False
                Me.cmnuCreateContourPlotFromLinescans.Enabled = False
                Me.cmnuShowNearestScanImage.Enabled = False
                Me.cmnuCropData.Enabled = False
                Me.cmnuCropDataUsingLastSettings.Enabled = False
                Me.cmnuFitNonLinear.Enabled = False
                Me.cmnuDisplayTogether.Enabled = False
                Me.cmnuTestSingle.Enabled = False
                Me.cmnuTestMultiple.Enabled = False
                Me.cmnuSpectroscopyTableShowDetails.Enabled = False
                Me.cmnuMultiplyData.Enabled = False
                Me.cmnuMultiplyDataLastFactor.Enabled = False
                Me.cmnuMultiplyDataLastOtherColumn.Enabled = False
                Me.cmnuCacheManagement.Enabled = False
                Me.cmnuCacheClearCreatedDataColumns.Enabled = False
                Me.cmnuCacheClearPreviewImages.Enabled = False
                Me.cmnuCacheClearCreatedScanChannels.Enabled = False
                Me.cmnuCacheClearCropInformation.Enabled = False
                Me.cmnuCacheClearAdditionalComments.Enabled = False
                Me.cmnuRenormalizeDataByParameter.Enabled = False
                Me.cmnuRenormalizeDataByParameterUsingLastSettings.Enabled = False
                Me.cmnuCalculateDataDerivative.Enabled = False
                Me.cmnuCalculateDataDerivativeUsingLastSettings.Enabled = False
            Case 1
                Me.cmnuOpenExportWizard.Enabled = True
                Me.cmnuCopyDataToClipboard.Enabled = True
                Me.cmnuCopyDataToClipboardOriginCompatible.Enabled = True
                Me.cmnuSmoothData.Enabled = True
                Me.cmnuSmoothDataUsingLastSettings.Enabled = True
                Me.cmnuAverageDataMultipleFiles.Enabled = False
                Me.cmnuAverageDataSingleFile.Enabled = True
                Me.cmnuRenormalizeData.Enabled = True
                Me.cmnuRenormalizeDataUsingLastSettings.Enabled = True
                'Me.cmnuInterpolateData.Enabled = True
                Me.cmnuShiftColumnValues.Enabled = True
                Me.cmnuNormalizeData.Enabled = True
                Me.cmnuNormalizeDataUsingLastSettings.Enabled = True
                Me.cmnuCreateContourPlotFromLinescans.Enabled = True
                Me.cmnuShowNearestScanImage.Enabled = True
                Me.cmnuCropData.Enabled = True
                Me.cmnuCropDataUsingLastSettings.Enabled = True
                Me.cmnuFitNonLinear.Enabled = True
                Me.cmnuDisplayTogether.Enabled = False
                Me.cmnuTestSingle.Enabled = True
                Me.cmnuTestMultiple.Enabled = False
                Me.cmnuSpectroscopyTableShowDetails.Enabled = True
                Me.cmnuMultiplyData.Enabled = True
                Me.cmnuMultiplyDataLastFactor.Enabled = True
                Me.cmnuMultiplyDataLastOtherColumn.Enabled = True
                Me.cmnuCacheManagement.Enabled = True
                Me.cmnuCacheClearCreatedDataColumns.Enabled = True
                Me.cmnuCacheClearPreviewImages.Enabled = True
                Me.cmnuCacheClearCreatedScanChannels.Enabled = True
                Me.cmnuCacheClearCropInformation.Enabled = True
                Me.cmnuCacheClearAdditionalComments.Enabled = True
                Me.cmnuRenormalizeDataByParameter.Enabled = True
                Me.cmnuRenormalizeDataByParameterUsingLastSettings.Enabled = True
                Me.cmnuCalculateDataDerivative.Enabled = True
                Me.cmnuCalculateDataDerivativeUsingLastSettings.Enabled = True
            Case Else
                Me.cmnuOpenExportWizard.Enabled = True
                Me.cmnuCopyDataToClipboard.Enabled = False
                Me.cmnuCopyDataToClipboardOriginCompatible.Enabled = False
                Me.cmnuSmoothData.Enabled = False
                Me.cmnuSmoothDataUsingLastSettings.Enabled = True
                Me.cmnuAverageDataMultipleFiles.Enabled = True
                Me.cmnuAverageDataSingleFile.Enabled = False
                Me.cmnuRenormalizeData.Enabled = False
                Me.cmnuRenormalizeDataUsingLastSettings.Enabled = True
                'Me.cmnuInterpolateData.Enabled = False
                Me.cmnuShiftColumnValues.Enabled = True
                Me.cmnuNormalizeData.Enabled = False
                Me.cmnuNormalizeDataUsingLastSettings.Enabled = True
                Me.cmnuCreateContourPlotFromLinescans.Enabled = True
                Me.cmnuShowNearestScanImage.Enabled = False
                Me.cmnuCropData.Enabled = False
                Me.cmnuCropDataUsingLastSettings.Enabled = True
                Me.cmnuFitNonLinear.Enabled = False
                Me.cmnuDisplayTogether.Enabled = True
                Me.cmnuTestSingle.Enabled = False
                Me.cmnuTestMultiple.Enabled = True
                Me.cmnuSpectroscopyTableShowDetails.Enabled = True
                Me.cmnuMultiplyData.Enabled = False
                Me.cmnuMultiplyDataLastFactor.Enabled = True
                Me.cmnuMultiplyDataLastOtherColumn.Enabled = True
                Me.cmnuCacheManagement.Enabled = True
                Me.cmnuCacheClearCreatedDataColumns.Enabled = True
                Me.cmnuCacheClearPreviewImages.Enabled = True
                Me.cmnuCacheClearCreatedScanChannels.Enabled = True
                Me.cmnuCacheClearCropInformation.Enabled = True
                Me.cmnuCacheClearAdditionalComments.Enabled = True
                Me.cmnuRenormalizeDataByParameter.Enabled = False
                Me.cmnuRenormalizeDataByParameterUsingLastSettings.Enabled = True
                Me.cmnuCalculateDataDerivative.Enabled = False
                Me.cmnuCalculateDataDerivativeUsingLastSettings.Enabled = True
        End Select

        ' --> Load a single selected SpectroscopyTableObject to display it as a preview!
        ' Multiple selected files only get loaded, if the user uses the context-menu
        ' or - as planned later - drag&drops the selected items.
        If SelectedSpectroscopyTableFiles.Count = 1 Then
            Me.ListOfSelectedSpectroscopyTables.Clear()
            SpectroscopyTableList.LoadFile(SelectedSpectroscopyTableFiles.First.Key, Me, WorkItemPriority.Normal)
        End If

        ' ENDE: Selected SpectroscopyTables
        '#######################################################################

        '#######################################################################
        ' 
        '               REGION: Selected ScanImages

        Dim SelectedScanImageFiles As Dictionary(Of String, cFileObject) = Me.GetSelectedScanImageFiles

        ' Enable or Disable Buttons depending on a single or multiple Selection.
        Select Case SelectedScanImageFiles.Count
            Case 0
                Me.cmnuScanImageShowDetails.Enabled = False
            Case 1
                Me.cmnuScanImageShowDetails.Enabled = True
            Case Else
                Me.cmnuScanImageShowDetails.Enabled = True
        End Select

        ' --> Load a single selected ScanImageObject to display it as a preview!
        ' Multiple selected files only get loaded, if the user uses the context-menu
        ' or - as planned later - drag&drops the selected items.
        If SelectedScanImageFiles.Count = 1 Then
            Me.ListOfSelectedScanImages.Clear()
            ScanImageList.LoadFile(SelectedScanImageFiles.First.Key, Me, WorkItemPriority.Normal)
        End If

        ' ENDE: Selected ScanImages
        '#######################################################################


        Dim TotalFileList As List(Of cFileObject) = SelectedScanImageFiles.Values.ToList
        TotalFileList.AddRange(SelectedSpectroscopyTableFiles.Values.ToList)

        If TotalFileList.Count = 1 Then
            RaiseEvent SingleFileObjectSelected(TotalFileList(0))
        ElseIf TotalFileList.Count > 1 Then
            RaiseEvent MultipleFileObjectsSelected(TotalFileList)
        End If

    End Sub

#End Region

#Region "Change the Sorting"
    ''' <summary>
    ''' Change the Sorting of the File-List when clicking on the header of the columns.
    ''' </summary>
    Private Sub dgvFileList_ColumnHeaderClick(sender As System.Object, e As DataGridViewCellMouseEventArgs) _
        Handles dgvFileList.ColumnHeaderMouseClick
        If Not Me.bReady Then Return

        ' If we sort already by the clicked column,
        ' then just change the sort direction.
        ' Else, we change the sort-index, but not the direction.
        If SortColumnIndex = e.ColumnIndex Then
            If SortDirection = SortOrder.Ascending Then
                SortDirection = SortOrder.Descending
            Else
                SortDirection = SortOrder.Ascending
            End If
        End If

        ' Check, which ColumnIndex is the one to use for sorting.
        Dim NewSortColumnIndex As Integer = -1
        Select Case e.ColumnIndex
            Case Me.colFileName.Index
            Case Me.colTime.Index
            Case Me.colFileDate.Index
            Case Else
                ' Abort
                Return
        End Select
        NewSortColumnIndex = e.ColumnIndex

        ' Reset the symbol of the former sort-column
        If NewSortColumnIndex <> Me.SortColumnIndex Then
            Me.dgvFileList.Columns(SortColumnIndex).HeaderCell.SortGlyphDirection = SortOrder.None
        End If
        Me.SortColumnIndex = NewSortColumnIndex

        ' Set Sorting Icon
        Me.dgvFileList.Columns(SortColumnIndex).HeaderCell.SortGlyphDirection = SortDirection

        ' Remove all entries from the sorted list.
        Me.FileListDisplayed_Sorted.Clear()

        ' Refresh the List
        For Each Col As DataGridViewColumn In Me.dgvFileList.Columns
            Me.dgvFileList.InvalidateColumn(Col.Index)
        Next

    End Sub
#End Region

#Region "File Fetcher Event Handling, e.g. if fetch is completed"
    ' #########################################################################################################
    ' 
    '          -> Perform now specific actions, like showing a List-Entry in the DataGridView
    ' 
    ' #########################################################################################################

    ''' <summary>
    ''' If the ListEntry got fetched successfully, reload the row corresponding to the ListEntry.
    ''' </summary>
    Private Sub ListEntryFetchComplete_SpectroscopyTable(ByRef ListEntry As cSpectroscopyTableList.SpectroscopyListEntry) Handles SpectroscopyTableList.ListEntryFetchComplete
        ' Fills the ColumnNames in the Filter-Comboboxes. Only Unique Names
        ' to be displayed in the Context-Menu when activated.
        Me.AdaptPreviewImageColumnFilters(ListEntry.ColumnNames)

        ' Invalidates the row in the DGV to fetch all the loaded details
        ' out of the List-Entry now available
        SyncLock Me.dgvFileList
            Me.dgvFileList.InvalidateRow(Me.GetRowIndexOfColumnByFileName(ListEntry.FullFileName))
        End SyncLock
    End Sub

    ''' <summary>
    ''' If the ListEntry got fetched successfully, reload the row corresponding to the ListEntry.
    ''' </summary>
    Private Sub ListEntryFetchComplete_ScanImage(ByRef ListEntry As cScanImageList.ListEntry) Handles ScanImageList.ListEntryFetchComplete
        ' Fills the ColumnNames in the Filter-Comboboxes. Only Unique Names
        ' to be displayed in the Context-Menu when activated.
        Me.AdaptPreviewImageChannelNameFilters(ListEntry.ColumnNames)

        ' Invalidates the row in the DGV to fetch all the loaded details
        ' out of the List-Entry now available
        SyncLock Me.dgvFileList
            Me.dgvFileList.InvalidateRow(Me.GetRowIndexOfColumnByFileName(ListEntry.FullFileName))
        End SyncLock
    End Sub
#End Region

#Region "Invalidate Cell and Rows thread-safe"
    Private Delegate Sub _InvalidateCell(ByVal ColumnIndex As Integer, ByVal RowIndex As Integer)
    ''' <summary>
    ''' Invalidates a Cell of the DataGridView.
    ''' </summary>
    Private Sub InvalidateCell(ByVal ColumnIndex As Integer, ByVal RowIndex As Integer)
        If Me.InvokeRequired Then
            Dim _delegate As New _InvalidateCell(AddressOf InvalidateCell)
            Me.Invoke(_delegate, ColumnIndex, RowIndex)
        Else
            Me.dgvFileList.InvalidateCell(ColumnIndex, RowIndex)
        End If
    End Sub
#End Region

#Region "Capture DataError-Event"
    Private Sub DataErrorCatcher(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvFileList.DataError
        e.Cancel = True
    End Sub
#End Region

#Region "Settings Panel"
    ''' <summary>
    ''' Show the settings panel!
    ''' </summary>
    Private Sub mnuSettings_Click(sender As Object, e As EventArgs) Handles mnuSettings.Click
        If Me.dpSettings.SlideState = DockablePanel.SlideStates.SlidIn Then
            ' Write all Column and Channel names to the combobox to make them selectable in the settings panel.
            Me.SetPreviewImageColumnsAndChannels()

            ' Slide the panel out
            Me.dpSettings.SlideOut()
        End If
    End Sub

    ''' <summary>
    ''' Hide on Mouse-Leave.
    ''' </summary>
    Private Sub dpRight_MouseLeave(sender As Object, e As EventArgs) Handles dpSettings.MouseLeave_PanelArea
        Me.dpSettings.SlideIn()

        ' Set the focus back to the file-list, to avoid scrolling through 
        ' controls in the settings panel
        Me.dgvFileList.Focus()
    End Sub

    ''' <summary>
    ''' Show on Mouse-Enter.
    ''' </summary>
    Private Sub dpRight_MouseEnter(sender As Object, e As EventArgs) Handles dpSettings.MouseLeave_PanelArea
        Me.dpSettings.SlideOut()
    End Sub

    ''' <summary>
    ''' Applies the Settings to the Preview-Images of the SpectroscopyTables:
    ''' </summary>
    Private Sub PreviewImageSettingsChanged_SpectroscopyTable(sender As System.Object, e As System.EventArgs) _
        Handles cbTableFiles_PreviewXColumn.SelectedIndexChanged, cbTableFiles_PreviewYColumn.SelectedIndexChanged, _
                ckbTableFiles_LogX.CheckedChanged, ckbTableFiles_LogY.CheckedChanged, _
                ckbEnablePointReduction.CheckedChanged, cbPreviewImageSize.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' Write Spectroscopy-Settings
        Me.SpectroscopyTableList.PreviewImageColumnName_X = Convert.ToString(Me.cbTableFiles_PreviewXColumn.SelectedItem)
        Me.SpectroscopyTableList.PreviewImageColumnName_Y = Convert.ToString(Me.cbTableFiles_PreviewYColumn.SelectedItem)
        Me.SpectroscopyTableList.PreviewImageLogX = Me.ckbTableFiles_LogX.Checked
        Me.SpectroscopyTableList.PreviewImageLogY = Me.ckbTableFiles_LogY.Checked

        ' Extract PreviewImage Height and Width
        Me.SetPreviewImageSizeFromSizeString(Convert.ToString(Me.cbPreviewImageSize.SelectedItem))

        ' Saves the selected Columns and Image-Parameters to the Settings for the Next Loading
        With My.Settings
            .ListPreviewImage_LogX = Me.SpectroscopyTableList.PreviewImageLogX
            .ListPreviewImage_LogY = Me.SpectroscopyTableList.PreviewImageLogY
            .ListPreviewImage_ReducePoints = Me.SpectroscopyTableList.PreviewImageReducePointsForProcessing
            .LastPreviewImageList_ColumnNameX = Me.SpectroscopyTableList.PreviewImageColumnName_X
            .LastPreviewImageList_ColumnNameY = Me.SpectroscopyTableList.PreviewImageColumnName_Y
            .Save()
        End With

        ' Delete List-Entrys loaded so far:
        Me.SpectroscopyTableList.ResetFetchedList()

        ' Reload Preview-Image-Column
        Me.dgvFileList.InvalidateColumn(Me.colPreviewImage.Index)
    End Sub

    ''' <summary>
    ''' Applies the Settings to the Preview-Images:
    ''' </summary>
    Private Sub PreviewImageSettingsChanged_ScanImage(sender As System.Object, e As System.EventArgs) _
        Handles cbPreviewImageSize.SelectedIndexChanged, cbScanImages_PreviewChannel.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' Write Scan-Settings
        Me.ScanImageList.PreviewImageChannel = Convert.ToString(Me.cbScanImages_PreviewChannel.SelectedItem)

        ' Extract PreviewImage Height and Width
        Me.SetPreviewImageSizeFromSizeString(Convert.ToString(Me.cbPreviewImageSize.SelectedItem))

        ' Saves the selected Columns and Image-Parameters to the Settings for the Next Loading
        With My.Settings
            .LastPreviewImageList_ChannelName = Me.ScanImageList.PreviewImageChannel
            .Save()
        End With

        ' Delete List-Entrys loaded so far:
        Me.ScanImageList.ResetFetchedList()

        ' Reload Preview-Image-Column
        Me.dgvFileList.InvalidateColumn(Me.colPreviewImage.Index)
    End Sub

    ''' <summary>
    ''' Sets the Preview-Image-Size in the list from a size string of the Format:
    ''' WIDTHxHEIGHT Description
    ''' </summary>
    Private Sub SetPreviewImageSizeFromSizeString(ByVal SizeString As String)
        If Not SizeString.Contains(" ") And Not SizeString.Contains("x") Then SizeString = Convert.ToString(Me.cbPreviewImageSize.Items(0))
        ' Extract PreviewImage Height and Width
        Dim sPreviewImageSize As String() = SizeString.Split(CChar(" "))(0).Split(CChar("x"))

        ' Save the new Dimensions
        Me.SpectroscopyTableList.PreviewImageHeigth = Convert.ToInt32(sPreviewImageSize(1))
        Me.SpectroscopyTableList.PreviewImageWidth = Convert.ToInt32(sPreviewImageSize(0))
        Me.ScanImageList.PreviewImageHeigth = Me.SpectroscopyTableList.PreviewImageHeigth
        Me.ScanImageList.PreviewImageWidth = Me.SpectroscopyTableList.PreviewImageWidth

        ' Apply the new dimensions to all Rows and the Column
        Me.dgvFileList.RowTemplate.Height = Me.SpectroscopyTableList.PreviewImageHeigth
        For Each Row As DataGridViewRow In Me.dgvFileList.Rows
            Row.Height = Me.SpectroscopyTableList.PreviewImageHeigth
        Next
        Me.dgvFileList.Columns(Me.colPreviewImage.Index).Width = Me.SpectroscopyTableList.PreviewImageWidth

        ' Save the Settings
        My.Settings.LastPreviewImageList_SizeString = SizeString
        My.Settings.LastPreviewImageList_Height = Me.SpectroscopyTableList.PreviewImageHeigth
        My.Settings.LastPreviewImageList_Width = Me.SpectroscopyTableList.PreviewImageWidth
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Sets the PreviewImage ColumnNames and Channels to the SettingsPanel.
    ''' </summary>
    Private Sub SetPreviewImageColumnsAndChannels()
        Me.bReady = False

        ' Add the Filter-Columns to the Comboboxes
        Me.cbTableFiles_PreviewXColumn.Items.Clear()
        Me.cbTableFiles_PreviewYColumn.Items.Clear()
        For Each ColumnName As String In Me.ListOfPreviewImageColumns
            Me.cbTableFiles_PreviewXColumn.Items.Add(ColumnName)
            Me.cbTableFiles_PreviewYColumn.Items.Add(ColumnName)
            If Me.SpectroscopyTableList.PreviewImageColumnName_X = ColumnName Then Me.cbTableFiles_PreviewXColumn.SelectedItem = ColumnName
            If Me.SpectroscopyTableList.PreviewImageColumnName_Y = ColumnName Then Me.cbTableFiles_PreviewYColumn.SelectedItem = ColumnName
        Next

        ' Add Filter-Channels to the Combobox
        Me.cbScanImages_PreviewChannel.Items.Clear()
        For Each ChannelName As String In Me.ListOfPreviewImageChannels
            Me.cbScanImages_PreviewChannel.Items.Add(ChannelName)
            If Me.ScanImageList.PreviewImageChannel = ChannelName Then Me.cbScanImages_PreviewChannel.SelectedItem = ChannelName
        Next

        ' Load Preview Image size string
        If Me.cbPreviewImageSize.Items.Contains(My.Settings.LastPreviewImageList_SizeString) Then
            Me.cbPreviewImageSize.SelectedItem = My.Settings.LastPreviewImageList_SizeString
        End If

        Me.bReady = True
    End Sub

#End Region

#Region "Implementation of the Fetcher Interface Functions"

    ''' <summary>
    ''' SpectroscopyTables selected
    ''' </summary>
    Private ListOfSelectedSpectroscopyTables As New List(Of cSpectroscopyTable)

    ''' <summary>
    ''' ScanImages selected
    ''' </summary>
    Private ListOfSelectedScanImages As New List(Of cScanImage)

#Region "SpectroscopyTable"
    Public Sub AllSpectroscopyTablesLoaded() Implements iMultipleSpectroscopyTablesLoaded.AllSpectroscopyTablesLoaded
        If ListOfSelectedSpectroscopyTables.Count = 0 Then
            ' Nothing
        ElseIf ListOfSelectedSpectroscopyTables.Count = 1 Then
            RaiseEvent SingleSpectroscopyTableSelected(ListOfSelectedSpectroscopyTables(0))
        Else
            RaiseEvent MultipleSpectroscopyTablesSelected(ListOfSelectedSpectroscopyTables)
        End If
    End Sub

    Public Sub OneOfAllSpectroscopyTablesFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iMultipleSpectroscopyTablesLoaded.OneOfAllSpectroscopyTablesFetched
        Me.ListOfSelectedSpectroscopyTables.Add(SpectroscopyTable)
    End Sub

    Public Property TotalNumberOfFilesToFetch_SpectroscopyTable As Integer Implements iMultipleSpectroscopyTablesLoaded.TotalNumberOfFilesToFetch

    Public Sub SpectroscopyTableLoaded(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded
        RaiseEvent SingleSpectroscopyTableSelected(SpectroscopyTable)
    End Sub
#End Region

#Region "ScanImage"
    Public Sub AllScanImagesLoaded() Implements iMultipleScanImagesLoaded.AllScanImagesLoaded
        If ListOfSelectedScanImages.Count = 0 Then
            ' Nothing
        ElseIf ListOfSelectedScanImages.Count = 1 Then
            RaiseEvent SingleScanImageSelected(ListOfSelectedScanImages(0))
        Else
            RaiseEvent MultipleScanImagesSelected(ListOfSelectedScanImages)
        End If
    End Sub

    Public Sub OneOfAllScanImagesFetched(ByRef ScanImage As cScanImage) Implements iMultipleScanImagesLoaded.OneOfAllScanImagesFetched
        Me.ListOfSelectedScanImages.Add(ScanImage)
    End Sub

    Public Property TotalNumberOfFilesToFetch_ScanImage As Integer Implements iMultipleScanImagesLoaded.TotalNumberOfFilesToFetch

    Public Sub SpectroscopyTableLoaded(ByRef ScanImage As cScanImage) Implements iSingleScanImageLoaded.ScanImageLoaded
        RaiseEvent SingleScanImageSelected(ScanImage)
    End Sub
#End Region

#End Region

#Region "Cell Double-Click -> Show Details"

    ''' <summary>
    ''' Calls a Detail-Window of the selected File, if the user clicks double on a specific cell.
    ''' </summary>
    Private Sub dgvFileList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFileList.CellDoubleClick
        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Return

        Dim FileNameOfColumn As String = Me.GetFileNameOfColumnByRowIndex(e.RowIndex)

        If FileNameOfColumn = "" Then
            Debug.WriteLine("Recieved empty Column-Filename -> sorted list must have a failure!")
            Return
        End If

        Select Case Me.FileListDisplayed(FileNameOfColumn).FileType
            Case cFileObject.FileTypes.SpectroscopyTable
                ' Show Spectroscopy-Table Details
                Dim DataExplorer As New wDataExplorer_SpectroscopyTable
                DataExplorer.Show(Me.GetSelectedSpectroscopyTableFiles.First.Value)
                DataExplorer.SetInitialColumnSelection(Me.PreviewImageColumnNameX, Me.PreviewImageColumnNameY)

            Case cFileObject.FileTypes.ScanImage
                ' Show ScanImage Details
                Dim DataExplorer As New wDataExplorer_ScanImage
                DataExplorer.Show(Me.GetSelectedScanImageFiles.First.Value)
                DataExplorer.SetInitialChannelSelection(Me.ScanImagePreviewChannel)
        End Select

    End Sub

#End Region

#Region "SpectroscopyTable or Scan-Image selection -> mediator between the List modules"
    Delegate Sub _SpectroscopyTableList_SingleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTable As cSpectroscopyTable)
    ''' <summary>
    ''' Selection of a Single SpectroscopyTable in Spectroscopy-Table-Filelist Changed.
    ''' Reload the Spectroscopy-Table Preview, and check, if the Scan-Image has to be refreshed for showing
    ''' additional point marks.
    ''' </summary>
    Private Sub SpectroscopyTableList_SingleSpectroscopyTableSelected(ByRef SpectroscopyTable As cSpectroscopyTable) Handles Me.SingleSpectroscopyTableSelected
        If Not Me.bReady Then Return

        If Me.InvokeRequired Then
            Dim _delegate As New _SpectroscopyTableList_SingleSpectroscopyTableSelected(AddressOf SpectroscopyTableList_SingleSpectroscopyTableSelected)
            Me.Invoke(_delegate, SpectroscopyTable)
        Else
            ' Fill Spectrum in Preview Box
            Me.pbPreviewBox.SetSinglePreviewImage(SpectroscopyTable,
                                                  Me.PreviewImageColumnNameX,
                                                  Me.PreviewImageColumnNameY)

            '' Set Point-Marks in the Preview-Window of the ScanImage,
            '' if the selected Image contains the selected Spectroscopy-Files:
            Me.svScanViewer.ClearPointMarkList()
            For Each SpectroscopyTableFileObject As cFileObject In Me.GetSelectedSpectroscopyTableFiles.Values ' Would show the Points of all Spectra: Me.oFileImporter.CurrentFileBufferFilteredByType(cFileObject.FileTypes.SpectroscopyTable).Values
                Me.svScanViewer.AddPointMark(SpectroscopyTableFileObject.RecordLocation_X,
                                             SpectroscopyTableFileObject.RecordLocation_Y,
                                             SpectroscopyTableFileObject.RecordLocation_Z)
            Next
            Me.svScanViewer.RecalculateImage()
        End If
    End Sub

    Delegate Sub _SpectroscopyTableList_MultipleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTables As List(Of cSpectroscopyTable))
    ''' <summary>
    ''' Selection of Multiple SpectroscopyTable in Spectroscopy-Table-Filelist Changed.
    ''' If the FileObject is in the correct location, refresh the Scan-Image has to show the additional point marks.
    ''' </summary>
    Private Sub SpectroscopyTableList_MultipleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTables As List(Of cSpectroscopyTable)) Handles Me.MultipleSpectroscopyTablesSelected
        If Not Me.bReady Then Return

        If Me.InvokeRequired Then
            Dim _delegate As New _SpectroscopyTableList_MultipleSpectroscopyTableSelected(AddressOf SpectroscopyTableList_MultipleSpectroscopyTableSelected)
            Me.Invoke(_delegate, SelectedSpectroscopyTables)
        Else
            ' Fill Spectra in Preview Box
            Me.pbPreviewBox.AddSpectroscopyTables(SelectedSpectroscopyTables)

            '' Set Point-Marks in the Preview-Window of the ScanImage,
            '' if the selected Image contains the selected Spectroscopy-Files:
            Me.svScanViewer.ClearPointMarkList()
            For Each SpectroscopyTable As cSpectroscopyTable In SelectedSpectroscopyTables ' Would show the Points of all Spectra: Me.oFileImporter.CurrentFileBufferFilteredByType(cFileObject.FileTypes.SpectroscopyTable).Values
                Me.svScanViewer.AddPointMark(SpectroscopyTable.Location_X,
                                             SpectroscopyTable.Location_Y,
                                             SpectroscopyTable.Location_Z)
            Next
            Me.svScanViewer.RecalculateImage()
        End If
    End Sub

    Delegate Sub _ScanImageList_SelectedScanImageChanged(ByRef SelectedScanImage As cScanImage)
    ''' <summary>
    ''' Selection Change in the Scan-Image-List changes the Scan-Image displayed as preview.
    ''' </summary>
    Private Sub ScanImageList_SelectedScanImageChanged(ByRef SelectedScanImage As cScanImage) Handles Me.SingleScanImageSelected
        If Not Me.bReady Then Return

        If Me.InvokeRequired Then
            Dim _delegate As New _ScanImageList_SelectedScanImageChanged(AddressOf ScanImageList_SelectedScanImageChanged)
            Me.Invoke(_delegate, SelectedScanImage)
        Else
            ' Set the Preview-Image
            Me.svScanViewer.SetScanImageObject(SelectedScanImage, Me.ScanImagePreviewChannel)
        End If
    End Sub

    ''' <summary>
    ''' Selection of a Single File-Object,
    ''' Draw PointMarks in the ScanImage
    ''' </summary>
    Private Sub List_SingleFileObjectSelected(ByRef SelectedFileObject As cFileObject) Handles Me.SingleFileObjectSelected
        If Not Me.bReady Then Return

        '' Set Point-Marks in the Preview-Window of the ScanImage,
        '' if the selected Image contains the selected Spectroscopy-Files:
        Me.svScanViewer.ClearPointMarkList()
        For Each FO As cFileObject In Me.GetSelectedSpectroscopyTableFiles.Values ' Would show the Points of all Spectra: Me.oFileImporter.CurrentFileBufferFilteredByType(cFileObject.FileTypes.SpectroscopyTable).Values
            Me.svScanViewer.AddPointMark(FO.RecordLocation_X, FO.RecordLocation_Y, FO.RecordLocation_Z)
        Next
        Me.svScanViewer.RecalculateImage()
    End Sub

    ''' <summary>
    ''' Selection of Multiple File-Objects,
    ''' Draw PointMarks in the ScanImage
    ''' </summary>
    Private Sub List_MultipleFileObjectsSelected(ByRef SelectedFileObjects As List(Of cFileObject)) Handles Me.MultipleFileObjectsSelected
        If Not Me.bReady Then Return

        '' Set Point-Marks in the Preview-Window of the ScanImage,
        '' if the selected Image contains the selected Spectroscopy-Files:
        Me.svScanViewer.ClearPointMarkList()
        For Each FO As cFileObject In Me.GetSelectedSpectroscopyTableFiles.Values ' Would show the Points of all Spectra: Me.oFileImporter.CurrentFileBufferFilteredByType(cFileObject.FileTypes.SpectroscopyTable).Values
            Me.svScanViewer.AddPointMark(FO.RecordLocation_X, FO.RecordLocation_Y, FO.RecordLocation_Z)
        Next
        Me.svScanViewer.RecalculateImage()
    End Sub
#End Region

#Region "Select Scan-Image for a Spectrum-Location"
    ''' <summary>
    ''' Shows the nearest Scan-Image to a selected Spectroscopy-File.
    ''' </summary>
    Private Sub NearestScanImageRequested(ByRef FileObject As cFileObject) Handles Me.NearestScanImageInTimeForSpectroscopyTableSearched
        Dim ClosestScanImageFileObject As cFileObject = Me.GetScanImageNearestToSpectrumInTime(FileObject)
        If Not ClosestScanImageFileObject Is Nothing Then
            ScanImageList.LoadFile(ClosestScanImageFileObject.FullFileName, Me)
        End If
    End Sub

    ''' <summary>
    ''' Returns all Scan-Images-FileNames in the Location
    ''' of the Spectroscopy-Table-FileObject
    ''' sorted by their absolute distance in time.
    ''' </summary>
    Private Function GetScanImagesInSpectrumLocation(ByRef SpectroscopyTableFileObject As cFileObject) As List(Of cFileObject)
        Dim ResultList As New List(Of cFileObject)
        For Each ScanImageFileObject As cFileObject In Me.oFileImporter.CurrentFileBufferFilteredByType(cFileObject.FileTypes.ScanImage).Values
            If cScanImage.CheckIfCoordinateLiesInImage(SpectroscopyTableFileObject.RecordLocation_X,
                                                       SpectroscopyTableFileObject.RecordLocation_Y,
                                                       ScanImageFileObject.RecordLocation_X,
                                                       ScanImageFileObject.RecordLocation_Y,
                                                       ScanImageFileObject.ScanImageRange_X,
                                                       ScanImageFileObject.ScanImageRange_Y) Then
                ResultList.Add(ScanImageFileObject)
            End If
        Next
        Return ResultList
    End Function

    ''' <summary>
    ''' Returns the Scan-Image-FileObject that lies closest in time to the SpectroscopyFileObject.
    ''' </summary>
    Private Function GetScanImageNearestToSpectrumInTime(ByRef SpectroscopyTableFileObject As cFileObject) As cFileObject
        Dim ScanImageFileObjectList As List(Of cFileObject) = Me.GetScanImagesInSpectrumLocation(SpectroscopyTableFileObject)
        ' Calculate the Time-Span between Scan-Image record-date and Spectroscopy-Table record date
        Dim CurrentTimeSpanBetween As Long
        Dim NearestTimeSpanBetween As Long = Long.MaxValue
        Dim NearestIndex As Integer = -1
        For i As Integer = 0 To ScanImageFileObjectList.Count - 1 Step 1
            CurrentTimeSpanBetween = ScanImageFileObjectList(i).RecordDate.Ticks - SpectroscopyTableFileObject.RecordDate.Ticks
            If Math.Abs(CurrentTimeSpanBetween) < NearestTimeSpanBetween Then
                NearestTimeSpanBetween = Math.Abs(CurrentTimeSpanBetween)
                NearestIndex = i
            End If
        Next
        If NearestIndex = -1 Then
            Return Nothing
        Else
            Return ScanImageFileObjectList(NearestIndex)
        End If
    End Function

    ''' <summary>
    ''' Returns all Spectroscopy File-Objects in the Location of the Scan-Image-FileObject
    ''' sorted by their absolute distance in time.
    ''' </summary>
    Private Function GetSpectraInScanImageLocation(ByRef ScanImageFileObject As cFileObject) As List(Of cFileObject)
        Dim ResultList As New List(Of cFileObject)
        For Each SpectroscopyTableFileObject As cFileObject In Me.oFileImporter.CurrentFileBufferFilteredByType(cFileObject.FileTypes.SpectroscopyTable).Values
            If cScanImage.CheckIfCoordinateLiesInImage(SpectroscopyTableFileObject.RecordLocation_X,
                                                       SpectroscopyTableFileObject.RecordLocation_Y,
                                                       ScanImageFileObject.RecordLocation_X,
                                                       ScanImageFileObject.RecordLocation_Y,
                                                       ScanImageFileObject.ScanImageRange_X,
                                                       ScanImageFileObject.ScanImageRange_Y) Then
                ResultList.Add(SpectroscopyTableFileObject)
            End If
        Next
        Return ResultList
    End Function
#End Region


    '##############################################################################################
    '##############################################################################################
    '##############################################################################################
    '                                                                                             '
    '                                   Menu-Button-Actions                                       '
    '                                                                                             '
    '##############################################################################################
    '##############################################################################################
    '##############################################################################################

    ''' <summary>
    ''' Delegate for old Windows, that expect SpectroscopyTables as input
    ''' </summary>
    Private Delegate Sub _SpectroscopyTableListExpectingWindow(ByRef SpectroscopyTableList As List(Of cSpectroscopyTable))

#Region "Value-Shifting - OK"
    ''' <summary>
    ''' Show the Shift-Column-Values window
    ''' </summary>
    Private Sub cmnuShiftColumnValues_Click(sender As System.Object, e As System.EventArgs) _
        Handles cmnuShiftColumnValues.Click

        Dim SelectedSpectroscopyFileNames As List(Of String) = Me.GetSelectedSpectroscopyTableFiles.Keys.ToList

        Dim Window As New wShiftColumnValues

        AddHandler Window.FormClosed, AddressOf ShiftColumnValuesWindowClosed

        Me.SpectroscopyTableList.LoadFiles(SelectedSpectroscopyFileNames,
                                           Window)
    End Sub

    ''' <summary>
    ''' Opens a window for shifting values of Columns from the selected SpectroscopyTables
    ''' </summary>
    Private Sub ShiftColumnValuesWindowClosed(sender As Object, e As FormClosedEventArgs)
        Dim SpectroscopyTableList As List(Of cSpectroscopyTable) = DirectCast(sender, wShiftColumnValues).CurrentListOfSpectroscopyTables
        Try
            ' Request a refesh of all ColumnList-Columns for the treated SpectroscopyTables:
            For i As Integer = 0 To SpectroscopyTableList.Count - 1 Step 1
                ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
                Me.SpectroscopyTableList.RemoveListEntry(SpectroscopyTableList(i).FullFileName)

                ' Invalidate Cell of Renormalized Row, where the ColumnList can be modified:
                InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTableList(i).FullFileName))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "Data ReNormalization (by Parameters) - OK"
    ''' <summary>
    ''' List that contains the DatarenormalizerObjects.
    ''' </summary>
    Private DataRenormalizerByParameterList As New Dictionary(Of String, cSpectroscopyTableDataRenormalizerByParameter)

    Private RenormalizerMutexByParameter As New Mutex

    ''' <summary>
    ''' Opens the Window for Data-Renormalization.
    ''' </summary>
    Private Sub tmnuRenormalizeDataByParameter_Click(sender As System.Object, e As System.EventArgs) Handles cmnuRenormalizeDataByParameter.Click
        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        Dim Window As New wDataRenormalizationByParameters

        Window.Show(Me.GetSelectedSpectroscopyTableFiles.First.Value)

        AddHandler Window.FormClosed, AddressOf Me.wDataRenormalizationByParameterWindowClosed
    End Sub

    ''' <summary>
    ''' DataRenormalizationByParameter-Window closed.
    ''' </summary>
    Private Sub wDataRenormalizationByParameterWindowClosed(sender As Object, e As FormClosedEventArgs)
        Dim SpectroscopyTable As cSpectroscopyTable = DirectCast(sender, wDataRenormalizationByParameters).CurrentSpectroscopyTable
        Try
            ' Request a refesh of the ColumnList-Column for the treated SpectroscopyTable:
            ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
            Me.SpectroscopyTableList.RemoveListEntry(SpectroscopyTable.FullFileName)

            ' Invalidate Cell of Row, where the ColumnList can be modified:
            InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTable.FullFileName))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        RemoveHandler DirectCast(sender, wDataRenormalizationByParameters).FormClosed, AddressOf wDataRenormalizationByParameterWindowClosed
    End Sub

    ''' <summary>
    ''' Renormalizes the Data using the last Settings from the Renormalization Window.
    ''' </summary>
    Private Sub tmnuRenormalizeDataByParameterUsingLastSettings_Click(sender As System.Object, e As System.EventArgs) Handles cmnuRenormalizeDataByParameterUsingLastSettings.Click, cmnuRenormalizeDataUsingLastSettings.Click
        ' Check, if Settings had been saved.
        With My.Settings
            If .LastRenormalizationByParameter_AmplifierGain < 0 Or
               .LastRenormalizationByParameter_LockInBiasModulation <= 0 Or
               .LastRenormalizationByParameter_LockInSensitivity <= 0 Or
               .LastRenormalizationByParameter_SourceColumnX = "" Or
               .LastRenormalizationByParameter_SourceColumn = "" Or
               .LastRenormalizationByParameter_NewColumnName = "" Then
                MessageBox.Show(My.Resources.Message_SettingsMissing,
                                My.Resources.Title_SettingsMissing,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Return
            End If
        End With

        RenormalizerMutexByParameter.WaitOne()
        SyncLock DataRenormalizerByParameterList
            DataRenormalizerByParameterList.Clear()
            For Each FileObjectKV As KeyValuePair(Of String, cFileObject) In Me.GetSelectedSpectroscopyTableFiles
                ' Add the new DataRenormalizerByParameterObject
                DataRenormalizerByParameterList.Add(FileObjectKV.Key, New cSpectroscopyTableDataRenormalizerByParameter(FileObjectKV.Value))

                ' Add the Events of the Renormalizer-Objects to the DataRenormalier
                AddHandler DataRenormalizerByParameterList(FileObjectKV.Key).FileRenormalizedComplete, AddressOf Me.RenormalizingOfDataByParameterComplete

                ' Start the File-Renormalization-Process
                DataRenormalizerByParameterList(FileObjectKV.Key).RenormalizeColumnWITHFetch(My.Settings.LastRenormalizationByParameter_SourceColumn,
                                                                                             My.Settings.LastRenormalizationByParameter_LockInBiasModulation,
                                                                                             My.Settings.LastRenormalizationByParameter_LockInSensitivity,
                                                                                             My.Settings.LastRenormalizationByParameter_AmplifierGain,
                                                                                             My.Settings.LastRenormalizationByParameter_NewColumnName)
            Next
        End SyncLock
        RenormalizerMutexByParameter.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Function called after the Renormalization-Process -> removes the DataRenormalizer again,
    ''' and invalidates the row to update the ColumnNames
    ''' </summary>
    Private Sub RenormalizingOfDataByParameterComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RenormalizedDataColumn As cSpectroscopyTable.DataColumn)
        RenormalizerMutexByParameter.WaitOne()
        SyncLock DataRenormalizerByParameterList
            ' Save the Renormalized Column and then removes the DataRenormalizer from the list
            DataRenormalizerByParameterList(SpectroscopyTable.FullFileName).SaveRenormalizedColumnToFileObject(My.Settings.LastRenormalizationByParameter_NewColumnName)
            DataRenormalizerByParameterList.Remove(SpectroscopyTable.FullFileName)
        End SyncLock
        RenormalizerMutexByParameter.ReleaseMutex()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(SpectroscopyTable.FullFileName)

        ' Invalidate the Cell of the ColumnList.
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTable.FullFileName))
    End Sub
#End Region

#Region "Display Spectroscopy-Tables together in a single preview window"
    ''' <summary>
    ''' Loads all selected SpectroscopyTables.
    ''' </summary>
    Private Sub cmnuDisplayTogetherAsPreview() Handles cmnuDisplayTogether.Click
        Dim SelectedSpectroscopyTableFiles As Dictionary(Of String, cFileObject) = Me.GetSelectedSpectroscopyTableFiles
        If SelectedSpectroscopyTableFiles.Count > 0 Then
            ' --> Load the corresponding SpectroscopyTableObject to display them as a preview!
            Me.ListOfSelectedSpectroscopyTables.Clear()
            SpectroscopyTableList.LoadFiles(SelectedSpectroscopyTableFiles.Keys.ToList, Me)
        End If
    End Sub
#End Region

End Class