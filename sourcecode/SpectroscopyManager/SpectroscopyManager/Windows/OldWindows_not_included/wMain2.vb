Public Class wMain2
    Inherits wFormBase

#Region "Properties"

    ''' <summary>
    ''' Saves the current working Directory.
    ''' </summary>
    ''' <remarks></remarks>
    Private sWorkingDirectoryPath As String

    ''' <summary>
    ''' file-importer which loads all spectroscopy-files
    ''' </summary>
    ''' <remarks></remarks>
    Private oFileImporter As cFileImport

    ''' <summary>
    ''' Background-Thread for fetching the Files from the Directory.
    ''' </summary>
    ''' <remarks></remarks>
    Private oFileBufferFetcher As New System.ComponentModel.BackgroundWorker

    Private bReady As Boolean = False

#End Region

#Region "Window Loading Routine"
    ''' <summary>
    ''' Constructor: Window gets loaded
    ''' Initializations are running.
    ''' </summary>
    Private Sub wMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Add adresses to the File-Buffer Fetcher
        Me.oFileBufferFetcher.WorkerSupportsCancellation = True
        Me.oFileBufferFetcher.WorkerReportsProgress = True
        AddHandler Me.oFileBufferFetcher.DoWork, AddressOf Me.FileBufferFetcher
        AddHandler Me.oFileBufferFetcher.RunWorkerCompleted, AddressOf Me.FileBufferFetcher_AllComplete
        AddHandler Me.oFileBufferFetcher.ProgressChanged, AddressOf Me.FileBufferFetcher_ReportProgress

        ' Initialize FileTreeList
        Me.dbDictionaryBrowser.RootItem = ExpTreeLib.CShItem.GetDeskTop

        '### Load Last Settings, if they are still valid.

        ' Load StartupPath
        Dim StartupPath As String = My.Settings.LastSelectedPath

        ' Try Several alternative Paths, if the Path from the setting does not exist!
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
        End If

        Dim StartupPathItem As ExpTreeLib.CShItem = ExpTreeLib.CShItem.GetCShItem(StartupPath)
        If Not StartupPathItem Is Nothing Then
            Me.dbDictionaryBrowser.ExpandANode(StartupPathItem)
            Me.sWorkingDirectoryPath = StartupPath
        End If

        ' Save Settings.
        My.Settings.LastSelectedPath = StartupPath
        My.Settings.Save()

        ' Set the Default-State of the Progress-Bar:
        ' Makes the Progress-Bar and Text invisible.
        Me.MainStatusStrip.Visible = False
        'Me.pgProgress.Visible = False
        'Me.lblProgressBar.Visible = False

        ' Depending on the previous panel state, set the docking panels to be
        ' collapsed or expanded before the window opens.
        If Not My.Settings.MainWindow_Layout_FileBrowserVisible Then
            Me.pFileBrowser.SlideIn(True)
        End If
        If Not My.Settings.MainWindow_Layout_SpectroscopyPropertiesVisible Then
            Me.pSpectroscopyTableProperties.SlideIn(True)
        End If
        If Not My.Settings.MainWindow_Layout_ScanPropertiesVisible Then
            Me.pScanImageProperties.SlideIn(True)
        End If

        ' Set "Refresh-List"-Button to "Refresh"
        Me.SetButtonsToLoadingListModus(False)

        ' Hide Test-Window Button
#If Not Debug Then
        Me.tmnuTest.Visible = False
#End If

        Me.bReady = True
    End Sub
#End Region

#Region "File-Buffer Fetcher"
    ''' <summary>
    ''' Loads all Relevant Contents From a Directory.
    ''' </summary>
    Public Sub FillDGV()
        ' Set "Refresh-List"-Button to "Cancel"
        Me.SetButtonsToLoadingListModus(True)

        ' Makes the Progress-Bar and Text visible again.
        Me.pgProgress.Value = 0
        Me.lblProgressBar.Text = ""
        'Me.pgProgress.Visible = True
        'Me.lblProgressBar.Visible = True
        Me.MainStatusStrip.Visible = True

        ' Start the File-Buffer-Fetcher.
        If Not oFileBufferFetcher.IsBusy Then Me.oFileBufferFetcher.RunWorkerAsync()
    End Sub

    ''' <summary>
    ''' Fetches all the Files with their Contents from the Working-Directory.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FileBufferFetcher(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ' Create new File-Importer-Object.
        Me.oFileImporter = New cFileImport

        ' Scan the Directory and Create a FileBuffer.
        Me.oFileImporter.CreateFileBuffer(Me.sWorkingDirectoryPath, Me.oFileBufferFetcher)
    End Sub

    ''' <summary>
    ''' If the whole File-Buffer got fetched, this Function initializes the Filtering
    ''' and fills the DataGridView
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FileBufferFetcher_AllComplete(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Me.bReady = False

        ' Set the Scan-Image-File-List to the File-List Module
        Me.slScanImageList.SetCurrentlyDisplayedFileList(Me.oFileImporter.CurrentFileBufferFilteredByType(cFileImport.FileType.ScanImage))

        ' Set the Spectroscopy-Table-File-List to the File-List Module
        Me.slSpectroscopyTableList.SetCurrentlyDisplayedFileList(Me.oFileImporter.CurrentFileBufferFilteredByType(cFileImport.FileType.SpectroscopyTable))

        ' Makes the Progress-Bar and Text invisible again.
        Me.MainStatusStrip.Visible = False
        'Me.pgProgress.Visible = False
        'Me.lblProgressBar.Visible = False

        ' Set "Refresh-List"-Button to "Refresh"
        Me.SetButtonsToLoadingListModus(False)

        ' Activate Handlers
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Function for reporting the Progress of the File-Fetching to the User.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FileBufferFetcher_ReportProgress(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
        If e.ProgressPercentage <= 100 Then
            Me.pgProgress.Value = e.ProgressPercentage
        Else
            Me.pgProgress.Value = 100
        End If
        Me.lblProgressBar.Text = Convert.ToString(e.UserState)
    End Sub
#End Region

#Region "List-Refresh-Functions"
    ''' <summary>
    ''' Initialize a List-Refresh by the Button-Click, if the selected working directory exists.
    ''' </summary>
    Private Sub btnRefreshList_Click(sender As System.Object, e As System.EventArgs) Handles btnRefreshList.Click, tmnuReloadFileList.Click
        ' If the Worker is Not Working: Start it.
        ' Otherwise act as Cancel-Button to Quit the Worker.
        If Me.oFileBufferFetcher.IsBusy Then
            Me.oFileBufferFetcher.CancelAsync()
        Else
            If IO.Directory.Exists(Me.sWorkingDirectoryPath) Then
                Me.FillDGV()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Enables/disables the List-Load buttons and set's the icons.
    ''' </summary>
    Private Sub SetButtonsToLoadingListModus(ByVal IsListLoading As Boolean)
        If IsListLoading Then
            Me.btnRefreshList.Text = My.Resources.FileListRefresh_Cancel
            Me.btnRefreshList.Image = My.Resources.cancel_16
            Me.tmnuReloadFileList.Text = My.Resources.FileListRefresh_Cancel
            Me.tmnuReloadFileList.Image = My.Resources.cancel_16
        Else
            Me.btnRefreshList.Text = My.Resources.FileListRefresh_RefreshList
            Me.btnRefreshList.Image = My.Resources.reload_16
            Me.tmnuReloadFileList.Text = My.Resources.FileListRefresh_RefreshList
            Me.tmnuReloadFileList.Image = My.Resources.reload_16
        End If
    End Sub

    ''' <summary>
    ''' Called function if the selected Dictionary in the Directory-Browser changes
    ''' Sets then this Folder as new WorkingFolder.
    ''' </summary>
    Private Sub dbDictionaryBrowser_ExpTreeNodeSelected(SelPath As System.String, Item As ExpTreeLib.CShItem) Handles dbDictionaryBrowser.ExpTreeNodeSelected
        If Not Me.bReady Then Return
        ' Save the Path
        Me.sWorkingDirectoryPath = SelPath
        My.Settings.LastSelectedPath = SelPath
        My.Settings.Save()
    End Sub
#End Region

#Region "SpectroscopyTable or Scan-Image selection -> mediator between the List modules"
    Delegate Sub _SpectroscopyTableList_SingleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTable As cSpectroscopyTable)
    ''' <summary>
    ''' Selection of a Single SpectroscopyTable in Spectroscopy-Table-Filelist Changed.
    ''' Reload the Spectroscopy-Table Preview, and check, if the Scan-Image has to be refreshed for showing
    ''' additional point marks.
    ''' </summary>
    Private Sub SpectroscopyTableList_SingleSpectroscopyTableSelected(ByRef SpectroscopyTable As cSpectroscopyTable) Handles slSpectroscopyTableList.SingleSpectroscopyTableSelected
        If Not Me.bReady Then Return

        If Me.pgImageProperies.InvokeRequired Then
            Dim _delegate As New _SpectroscopyTableList_SingleSpectroscopyTableSelected(AddressOf SpectroscopyTableList_SingleSpectroscopyTableSelected)
            Me.Invoke(_delegate, SpectroscopyTable)
        Else
            ' Paste the Properties to the PropertyGrid
            Me.pgSpectrumProperies.SelectedObject = SpectroscopyTable

            ' Set Comment-Text-Field
            Me.txtSpectraComment.Text = SpectroscopyTable.Comment

            ' Fill Spectrum in Preview Box
            Me.pbPreviewBox.SetSinglePreviewImage(SpectroscopyTable,
                                            Me.slSpectroscopyTableList.PreviewImageColumnNameX,
                                            Me.slSpectroscopyTableList.PreviewImageColumnNameY)

            ' Check for possible Scan-Image-Files, that are
            ' in the Range of the Position the Spectrum was taken at.
            Dim ScanImageFileObjectsInLocation As List(Of cFileImport.FileObject) = Me.GetScanImagesInSpectrumLocation(SpectroscopyTable.BaseFileObject)

            ' Add the ScanImageFileObjects to the List
            Me.lbPossibleScanImages.Items.Clear()
            Me.lbPossibleScanImages.DisplayMember = "FileNameWithoutPath"
            Me.lbPossibleScanImages.ValueMember = "FullFileName"
            Me.lbPossibleScanImages.Items.AddRange(ScanImageFileObjectsInLocation.ToArray)

            '' Set Point-Marks in the Preview-Window of the ScanImage,
            '' if the selected Image contains the selected Spectroscopy-Files:
            Me.svScanViewer.ClearPointMarkList()
            For Each SpectroscopyTableFileObject As cFileImport.FileObject In Me.slSpectroscopyTableList.GetSelectedSpectroscopyTableFiles.Values ' Would show the Points of all Spectra: Me.oFileImporter.CurrentFileBufferFilteredByType(cFileImport.FileType.SpectroscopyTable).Values
                Me.svScanViewer.AddPointMark(SpectroscopyTableFileObject.RecordLocation_X, SpectroscopyTableFileObject.RecordLocation_Y)
            Next
            Me.svScanViewer.RecalculateImage()
        End If
    End Sub

    Delegate Sub _SpectroscopyTableList_MultipleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTables As List(Of cSpectroscopyTable))
    ''' <summary>
    ''' Selection of Multiple SpectroscopyTable in Spectroscopy-Table-Filelist Changed.
    ''' If the FileObject is in the correct location, refresh the Scan-Image has to show the additional point marks.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpectroscopyTableList_MultipleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTables As List(Of cSpectroscopyTable)) Handles slSpectroscopyTableList.MultipleSpectroscopyTablesSelected
        If Not Me.bReady Then Return

        If Me.pgImageProperies.InvokeRequired Then
            Dim _delegate As New _SpectroscopyTableList_MultipleSpectroscopyTableSelected(AddressOf SpectroscopyTableList_MultipleSpectroscopyTableSelected)
            Me.Invoke(_delegate, SelectedSpectroscopyTables)
        Else
            ' Reset the Comment-Field and the PropertyBox
            Me.pgSpectrumProperies.SelectedObject = Nothing
            Me.txtSpectraComment.Text = ""

            ' Fill Spectra in Preview Box
            Me.pbPreviewBox.AddSpectroscopyTables(SelectedSpectroscopyTables)
            
            '' Set Point-Marks in the Preview-Window of the ScanImage,
            '' if the selected Image contains the selected Spectroscopy-Files:
            Me.svScanViewer.ClearPointMarkList()
            For Each SpectroscopyTable As cSpectroscopyTable In SelectedSpectroscopyTables ' Would show the Points of all Spectra: Me.oFileImporter.CurrentFileBufferFilteredByType(cFileImport.FileType.SpectroscopyTable).Values
                Me.svScanViewer.AddPointMark(SpectroscopyTable.Location_X, SpectroscopyTable.Location_Y)
            Next
            Me.svScanViewer.RecalculateImage()
        End If
    End Sub

    Delegate Sub _ScanImageList_SelectedScanImageChanged(ByRef SelectedScanImage As cScanImage)
    ''' <summary>
    ''' Selection Change in the Scan-Image-List changes the Scan-Image displayed as preview.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ScanImageList_SelectedScanImageChanged(ByRef SelectedScanImage As cScanImage) Handles slScanImageList.SelectedScanImageChanged
        If Not Me.bReady Then Return

        If Me.pgImageProperies.InvokeRequired Then
            Dim _delegate As New _ScanImageList_SelectedScanImageChanged(AddressOf ScanImageList_SelectedScanImageChanged)
            Me.Invoke(_delegate, SelectedScanImage)
        Else
            ' Paste the Properties to the PropertyGrid
            Me.pgImageProperies.SelectedObject = SelectedScanImage

            ' Set Comment-Text-Field
            Me.txtImageComment.Text = SelectedScanImage.Comment

            ' Check for possible Spectroscopy-Table-Files, that
            ' are in the Range of the Position the ScanImage shows.
            Dim SpectraInLocation As List(Of cFileImport.FileObject) = Me.GetSpectraInScanImageLocation(SelectedScanImage.BaseFileObject)

            ' Add the possible Spectra-File-Objects to the List.
            Me.lbPossibleSpectra.Items.Clear()
            Me.lbPossibleSpectra.DisplayMember = "FileNameWithoutPath"
            Me.lbPossibleSpectra.ValueMember = "FullFileName"
            Me.lbPossibleSpectra.Items.AddRange(SpectraInLocation.ToArray)

            ' Set the Preview-Image
            Me.svScanViewer.SetScanImageObject(SelectedScanImage)
        End If
    End Sub
#End Region

#Region "Select Scan-Image for a Spectrum-Location"
    ''' <summary>
    ''' Shows the nearest Scan-Image to a selected Spectroscopy-File.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub NearestScanImageRequested(ByRef FileObject As cFileImport.FileObject) Handles slSpectroscopyTableList.NearestScanImageInTimeForSpectroscopyTableSearched
        Dim ClosestScanImageFileObject As cFileImport.FileObject = Me.GetScanImageNearestToSpectrumInTime(FileObject)
        If Not ClosestScanImageFileObject Is Nothing Then
            Me.slScanImageList.SelectScanImageInList(ClosestScanImageFileObject.FullName)
        End If
    End Sub

    ''' <summary>
    ''' Returns all Scan-Images-FileNames in the Location
    ''' of the Spectroscopy-Table-FileObject
    ''' sorted by their absolute distance in time.
    ''' </summary>
    ''' <param name="SpectroscopyTableFileObject">FileObject of the SpectroscopyTable for which we should search the ScanImage.</param>
    ''' <remarks></remarks>
    Private Function GetScanImagesInSpectrumLocation(ByRef SpectroscopyTableFileObject As cFileImport.FileObject) As List(Of cFileImport.FileObject)
        Dim ResultList As New List(Of cFileImport.FileObject)
        For Each ScanImageFileObject As cFileImport.FileObject In Me.oFileImporter.CurrentFileBufferFilteredByType(cFileImport.FileType.ScanImage).Values
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
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetScanImageNearestToSpectrumInTime(ByRef SpectroscopyTableFileObject As cFileImport.FileObject) As cFileImport.FileObject
        Dim ScanImageFileObjectList As List(Of cFileImport.FileObject) = Me.GetScanImagesInSpectrumLocation(SpectroscopyTableFileObject)
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
    ''' <remarks></remarks>
    Private Function GetSpectraInScanImageLocation(ByRef ScanImageFileObject As cFileImport.FileObject) As List(Of cFileImport.FileObject)
        Dim ResultList As New List(Of cFileImport.FileObject)
        For Each SpectroscopyTableFileObject As cFileImport.FileObject In Me.oFileImporter.CurrentFileBufferFilteredByType(cFileImport.FileType.SpectroscopyTable).Values
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

#Region "Progress Bar Change"
    ' This will be called from the worker thread to set
    ' the work's progress
    Delegate Sub SetProgressCallback(ByVal MaxVal As Integer, CurrentVal As Integer, Text As String)
    Friend Sub SetProgress(MaxVal As Integer, CurrentVal As Integer, Text As String)
        ' Check on a arbitrary control, if an invoke is required
        If Me.lbPossibleSpectra.InvokeRequired Then
            Dim _delegate As New SetProgressCallback(AddressOf SetProgress)
            Me.Invoke(_delegate, MaxVal, CurrentVal, Text)
        Else
            If CurrentVal >= MaxVal Then
                Me.pgProgress.Visible = False
                Me.lblProgressBar.Visible = False
            Else
                ' Display the % work done
                If MaxVal > 0 Then
                    Dim ProgressValue As Integer = CInt(Math.Truncate(CurrentVal / MaxVal * 100))
                    Me.lblProgressBar.Text = Text
                    If ProgressValue <= 100 Then
                        Me.pgProgress.Value = ProgressValue
                    End If
                End If
            End If
        End If
    End Sub
#End Region

#Region "MenuBar Buttons Clicks"
    ''' <summary>
    ''' Show About Window
    ''' </summary>
    Private Sub tmnuInfo_Click(sender As System.Object, e As System.EventArgs) Handles tmnuInfo.Click
        Dim AboutWindow As New wAbout
        AboutWindow.ShowDialog()
        AboutWindow.Dispose()
    End Sub

    ''' <summary>
    ''' Open Test-Window.
    ''' </summary>
    Private Sub TestToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles tmnuTest.Click
        'Dim oTest As New wSXMTest
        'oTest.Show()

        'Dim oTest2 As New wFitTestGauss2
        'oTest2.Show()
    End Sub

    ''' <summary>
    ''' Exits the Program
    ''' </summary>
    Private Sub tmnuExit_Click(sender As System.Object, e As System.EventArgs) Handles tmnuExit.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Hide or shows the Scan-Image-Details pane, and saves the state for the next program launch.
    ''' </summary>
    Private Sub tmnuShowScanPane_Click(sender As System.Object, e As System.EventArgs) Handles tmnuShowScanPane.Click
        With Me.pScanImageProperties
            If .SlideState = DockablePanel.SlideStates.SlidOut Then
                .SlideIn()
            Else
                .SlideOut()
            End If
            My.Settings.MainWindow_Layout_ScanPropertiesVisible = .IsPanelSlidOut
            My.Settings.Save()
        End With
    End Sub

    ''' <summary>
    ''' Hide or shows the SpectroscopyTable-Details pane, and saves the state for the next program launch.
    ''' </summary>
    Private Sub tmnuShowSpectroscopyPane_Click(sender As System.Object, e As System.EventArgs) Handles tmnuShowSpectroscopyPane.Click
        With Me.pSpectroscopyTableProperties
            If .SlideState = DockablePanel.SlideStates.SlidOut Then
                .SlideIn()
            Else
                .SlideOut()
            End If
            My.Settings.MainWindow_Layout_SpectroscopyPropertiesVisible = .IsPanelSlidOut
            My.Settings.Save()
        End With
    End Sub

    ''' <summary>
    ''' Hide or shows the folder selection pane, and saves the state for the next program launch.
    ''' </summary>
    Private Sub tmnuShowFolderPane_Click(sender As System.Object, e As System.EventArgs) Handles tmnuShowFolderPane.Click
        With Me.pFileBrowser
            If .SlideState = DockablePanel.SlideStates.SlidOut Then
                .SlideIn()
            Else
                .SlideOut()
            End If
            My.Settings.MainWindow_Layout_FileBrowserVisible = .IsPanelSlidOut
            My.Settings.Save()
        End With
    End Sub
#End Region

End Class
