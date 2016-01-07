Imports System.Threading
Imports Amib.Threading

''' <summary>
''' Folder Browser Class
''' </summary>
Public Class wDataBrowserModular
    Inherits wFormBase
    Implements iSingleSpectroscopyTableLoaded
    Implements iSingleScanImageLoaded
    Implements iMultipleSpectroscopyTablesLoaded
    Implements iMultipleScanImagesLoaded

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
    ''' Container for the displayed data browser list.
    ''' </summary>
    Private WithEvents DataBrowserList As mDataBrowserList

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
        ' Set the window title to the Working Directory
        Me.Text = Me.sWorkingDirectory
        ' END STYLE SETTINGS
        '##########################################

        ' Create browser-list in the left panel
        Me.DataBrowserList = New mDataBrowserList(Me.sWorkingDirectory,
                                                  Me,
                                                  Me.OnlyLoadListOfFilesFromWorkingDirectory)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DataBrowserList)
        Me.DataBrowserList.Dock = DockStyle.Fill

        ' Set the window to ready!
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Function of the Form-Closing Event.
    ''' Abort the Closing, if the File-Buffer is fetching!
    ''' </summary>
    Private Sub wDataBrowser_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me.DataBrowserList Is Nothing Then Return
        If Me.DataBrowserList.IsBusy Then
            MessageBox.Show(My.Resources.rDataBrowser.FormClose_FileFetcherRunning,
                            My.Resources.rDataBrowser.FormClose_FileFetcherRunning_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            e.Cancel = True
        End If
    End Sub
#End Region

#Region "Event Handling, e.g. if list-entry is selected in the data Browser list"
    ''' <summary>
    ''' Multiple Spectroscopy-Files selected.
    ''' Fetch the files in the background!
    ''' </summary>
    Private Sub OnMultipleSpectroscopyTablesSelected(ByRef FileObjects As List(Of cFileObject)) Handles DataBrowserList.MultipleSpectroscopyTableSelected

        ' Set Point-Marks in the Preview-Window of the ScanImage,
        '' if the selected Image contains the selected Spectroscopy-Files:
        Me.svScanViewer.ClearPointMarkList()
        For Each FO As cFileObject In FileObjects
            Me.svScanViewer.AddPointMark(New cScanImagePlot.PointMark(
                                                        FO.RecordLocation_X,
                                                        FO.RecordLocation_Y,
                                                        FO.RecordLocation_Z,,,, FO.FileName
                                                        ))
        Next
        Me.svScanViewer.RecalculateImage()

    End Sub

    ''' <summary>
    ''' Multiple Spectroscopy-Files selected to show them in the preview-window
    ''' Fetch the files in the background!
    ''' </summary>
    Private Sub OnMultipleSpectroscopyTablesSelectedToShowInPreview(ByRef FileObjects As List(Of cFileObject)) Handles DataBrowserList.MultipleSpectroscopyTableSelectedShowInPreview

        cFileImport.GetSpectroscopyFiles_Async(FileObjects,
                                               Me, False, Me.DataBrowserList.ThreadPool)

    End Sub

    ''' <summary>
    ''' Single SpectroscopyTable selected.
    ''' Fetch the file in the background!
    ''' </summary>
    Private Sub OnSingleSpectroscopyTableSelected(ByRef FileObject As cFileObject) Handles DataBrowserList.SingleSpectroscopyTableSelected

        cFileImport.GetSpectroscopyFile_Async(FileObject,
                                              Me, False, Me.DataBrowserList.ThreadPool)

    End Sub

    ''' <summary>
    ''' Single ScanImage selected.
    ''' Fetch the file in the background!
    ''' </summary>
    Private Sub OnSingleScanImageSelected(ByRef FileObject As cFileObject) Handles DataBrowserList.SingleScanImageSelected

        cFileImport.GetScanImageFile_Async(FileObject,
                                           Me, False, Me.DataBrowserList.ThreadPool)

    End Sub

    ''' <summary>
    ''' Multiple ScanImages selected.
    ''' Fetch the file in the background!
    ''' </summary>
    Private Sub OnMultipleScanImageSelected(ByRef FileObjects As List(Of cFileObject)) Handles DataBrowserList.MultipleScanImagesSelected

        cFileImport.GetScanImageFiles_Async(FileObjects,
                                            Me, False, Me.DataBrowserList.ThreadPool)

    End Sub
#End Region

#Region "SpectroscopyTable or Scan-Image selection -> mediator between the List module and Preview-Boxes"
    Delegate Sub _SpectroscopyTableList_SingleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTable As cSpectroscopyTable)
    ''' <summary>
    ''' Selection of a Single SpectroscopyTable in Spectroscopy-Table-Filelist Changed.
    ''' Reload the Spectroscopy-Table Preview, and check, if the Scan-Image has to be refreshed for showing
    ''' additional point marks.
    ''' </summary>
    Private Sub SpectroscopyTableList_SingleSpectroscopyTableSelected(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded
        If Not Me.bReady Then Return

        If Me.InvokeRequired Then
            Dim _delegate As New _SpectroscopyTableList_SingleSpectroscopyTableSelected(AddressOf SpectroscopyTableList_SingleSpectroscopyTableSelected)
            Me.Invoke(_delegate, SpectroscopyTable)
        Else
            ' Fill Spectrum in Preview Box
            Me.pbPreviewBox.SetSinglePreviewImage(SpectroscopyTable,
                                                  Me.DataBrowserList.CurrentPreviewImageSettings.SpectroscopyTable_ColumnNameX,
                                                  Me.DataBrowserList.CurrentPreviewImageSettings.SpectroscopyTable_ColumnNameY,
                                                  True)

            '' Set Point-Marks in the Preview-Window of the ScanImage,
            '' if the selected Image contains the selected Spectroscopy-Files:
            Me.svScanViewer.ClearPointMarkList()
            Me.svScanViewer.AddPointMark(New cScanImagePlot.PointMark(
                                                        SpectroscopyTable.Location_X,
                                                        SpectroscopyTable.Location_Y,
                                                        SpectroscopyTable.Location_Z,,,, SpectroscopyTable.FileNameWithoutPathAndExtension
                                                        ))
            Me.svScanViewer.RecalculateImage()
        End If
    End Sub

    Delegate Sub _SpectroscopyTableList_MultipleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTables As List(Of cSpectroscopyTable))
    ''' <summary>
    ''' Selection of multiple SpectroscopyTable in Spectroscopy-Table-Filelist changed.
    ''' If the FileObject is in the correct location, refresh the Scan-Image has to show the additional point marks.
    ''' </summary>
    Private Sub SpectroscopyTableList_MultipleSpectroscopyTableSelected(ByRef SelectedSpectroscopyTables As List(Of cSpectroscopyTable)) Implements iMultipleSpectroscopyTablesLoaded.AllSpectroscopyTablesLoaded
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
                Me.svScanViewer.AddPointMark(New cScanImagePlot.PointMark(
                                                        SpectroscopyTable.Location_X,
                                                        SpectroscopyTable.Location_Y,
                                                        SpectroscopyTable.Location_Z,,,, SpectroscopyTable.FileNameWithoutPathAndExtension
                                                        ))
            Next
            Me.svScanViewer.RecalculateImage()
        End If
    End Sub

    Delegate Sub _ScanImageList_SelectedScanImageChanged(ByRef SelectedScanImage As cScanImage)
    ''' <summary>
    ''' Selection change in the Scan-Image-List changes the Scan-Image displayed as preview.
    ''' </summary>
    Private Sub ScanImageList_SelectedScanImageChanged(ByRef SelectedScanImage As cScanImage) Implements iSingleScanImageLoaded.ScanImageLoaded
        If Not Me.bReady Then Return

        If Me.InvokeRequired Then
            Dim _delegate As New _ScanImageList_SelectedScanImageChanged(AddressOf ScanImageList_SelectedScanImageChanged)
            Me.Invoke(_delegate, SelectedScanImage)
        Else
            ' Set the Preview-Image
            Me.svScanViewer.SetScanImageObjects(SelectedScanImage, Me.DataBrowserList.CurrentPreviewImageSettings.ScanImage_Channel)
        End If
    End Sub

    Delegate Sub _ScanImageList_MultipleScanImagesSelected(ByRef SelectedScanImages As List(Of cScanImage))
    ''' <summary>
    ''' Selection change in the Scan-Image-List changes the Scan-Image displayed as preview.
    ''' </summary>
    Public Sub ScanImageList_MultipleScanImagesSelected(ByRef SelectedScanImages As List(Of cScanImage)) Implements iMultipleScanImagesLoaded.AllScanImagesLoaded
        If Not Me.bReady Then Return

        If Me.InvokeRequired Then
            Dim _delegate As New _ScanImageList_MultipleScanImagesSelected(AddressOf ScanImageList_MultipleScanImagesSelected)
            Me.Invoke(_delegate, SelectedScanImages)
        Else

            ' Set the Preview-Image
            Me.svScanViewer.SetScanImageObjects(SelectedScanImages, Me.DataBrowserList.CurrentPreviewImageSettings.ScanImage_Channel)

        End If
    End Sub

#End Region

End Class