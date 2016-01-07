Imports System.ComponentModel

Public Class mScanImageList
    Implements iMultipleScanImagesLoaded
    Implements iSingleScanImageLoaded

#Region "Properties"
    Private bReady As Boolean = False

    ''' <summary>
    ''' Object for fetching the list of ScanImage Entries displayed.
    ''' </summary>
    Private WithEvents ScanImageList As cScanImageList

    ''' <summary>
    ''' List with channel-names to be displayed in the context-menu
    ''' </summary>
    Private ListOfPreviewImageChannels As New List(Of String)
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets raised, if the Selected File Changed
    ''' </summary>
    Public Event SelectedScanImageChanged(ByRef SelectedScanImage As cScanImage)
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets raised, if a single file gets selected in the list
    ''' </summary>
    Public Event SingleScanImageSelected(ByRef SelectedScanImage As cScanImage)

    ''' <summary>
    ''' Event that gets raised, if a multiple files get selected in the list
    ''' </summary>
    Public Event MultipleScanImagesSelected(ByRef SelectedScanImage As List(Of cScanImage))
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor: Just activate the Interface.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mScanImageViewer_Load(sender As System.Object, e As System.EventArgs)
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Sets the List of ScanImage-File-Objects to be displayed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetCurrentlyDisplayedFileList(ByRef FileList As Dictionary(Of String, cFileImport.FileObject))
        Me.bReady = False

        ' Delete Preview Image-Channel-List
        Me.ListOfPreviewImageChannels.Clear()

        ' Create new ScanImageList object from the given File-List
        Me.ScanImageList = New cScanImageList(FileList)

        ' Row Template Height gets set to the choosen image height
        Me.dgvScanImageFileList.RowTemplate.Height = Me.ScanImageList.PreviewImageHeigth
        Me.dgvScanImageFileList.Columns(Me.colPreview.Index).Width = Me.ScanImageList.PreviewImageWidth

        ' Set the Row-Count of the List to start the loading of the DataGridView
        Me.dgvScanImageFileList.Rows.Clear()
        Me.dgvScanImageFileList.RowCount = FileList.Count
        Me.dgvScanImageFileList.ClearSelection()

        Me.bReady = True
    End Sub
#End Region

#Region "File Fetcher Event Handling"
    ' ####################################################################################################
    ' 
   '          -> Perform now specific actions, like showing a List-Entry in the DataGridView
    ' 
    ' ####################################################################################################

    ''' <summary>
    ''' If the ListEntry got fetched successfully, reload the row corresponding to the ListEntry.
    ''' </summary>
    Private Sub ListEntryFetchComplete(ByRef ListEntry As cScanImageList.ListEntry) Handles ScanImageList.ListEntryFetchComplete
        ' Fills the Channel-Names in the Filter-Comboboxes. Only Unique Names
        ' to be displayed in the Context-Menu when activated.
        Me.AdaptPreviewImageChannelNameFilters(ListEntry.ColumnNames)

        ' Invalidates the row in the DGV to fetch all the loaded details
        ' out of the List-Entry now available
        Me.dgvScanImageFileList.InvalidateRow(Me.GetRowIndexOfColumnByFileName(ListEntry.FullFileName))
    End Sub
#End Region

#Region "Row Identification Functions"
    ''' <summary>
    ''' Adds unique entrys to the ChannelNameList
    ''' </summary>
    Private Sub AdaptPreviewImageChannelNameFilters(ChannelNames As List(Of String))
        Dim bValuesAdded As Boolean = False
        For Each CName As String In ChannelNames
            If Not Me.ListOfPreviewImageChannels.Contains(CName) Then
                Me.ListOfPreviewImageChannels.Add(CName)
                bValuesAdded = True
            End If
        Next
    End Sub

    ''' <summary>
    ''' Returns the File-Name of the Column determined by the Row-Index.
    ''' This function allows sorting of the list, although it is a virtual-mode
    ''' datagridview.
    ''' </summary>
    Private Function GetFileNameOfColumnByRowIndex(ByVal RowIndex As Integer) As String
        Dim iRowCounter As Integer = 0
        Dim FileNameOfColumn As String = ""
        Dim FileNameList As List(Of String) = ScanImageList.CurrentFileList.Keys.ToList
        For i As Integer = 0 To FileNameList.Count - 1 Step 1
            If iRowCounter = RowIndex Then
                FileNameOfColumn = FileNameList(i)
                Exit For
            End If
            iRowCounter += 1
        Next
        Return FileNameOfColumn
    End Function

    ''' <summary>
    ''' Returns the Row-Id of the Column determined by the FileName.
    ''' Returns -1, if Row was not found
    ''' </summary>
    Private Function GetRowIndexOfColumnByFileName(ByVal FullFileName As String) As Integer
        For i As Integer = 0 To Me.dgvScanImageFileList.RowCount - 1 Step 1
            If Convert.ToString(Me.dgvScanImageFileList.Rows(i).Cells(Me.colFullFileName.Index).Value) = FullFileName Then
                Return i
            End If
        Next
        Return 0
    End Function

    ''' <summary>
    ''' Returns the selected ScanImage-FileObjects
    ''' </summary>
    Public Function GetSelectedScanImageFiles() As Dictionary(Of String, cFileImport.FileObject)
        Dim lImageList As New Dictionary(Of String, cFileImport.FileObject)
        ' Add Files to average
        For Each Row As DataGridViewRow In Me.dgvScanImageFileList.SelectedRows
            Dim sFullFileName As String = Me.GetFileNameOfColumnByRowIndex(Row.Index)
            If Not ScanImageList.CurrentFileList.ContainsKey(sFullFileName) Then Continue For
            lImageList.Add(sFullFileName, ScanImageList.CurrentFileList(sFullFileName))
        Next
        Return lImageList
    End Function
#End Region

#Region "Data Grid View Functions"
    ''' <summary>
    ''' Function that Handles the Request for Data by the ScanImage-List,
    ''' and fills the visible Part of the DataGridView for the ScanImage.
    ''' </summary>
    Private Sub dgvScanImageList_CellValueNeeded(sender As System.Object, e As System.Windows.Forms.DataGridViewCellValueEventArgs) Handles dgvScanImageFileList.CellValueNeeded
        If Not Me.bReady Then Return

        Dim FileNameOfColumn As String = Me.GetFileNameOfColumnByRowIndex(e.RowIndex)

        If FileNameOfColumn = "" Then Return

        ' Write already Fetched Properties:
        Select Case e.ColumnIndex
            Case Me.colFullFileName.Index
                e.Value = FileNameOfColumn
            Case Me.colFileName.Index
                e.Value = FileNameOfColumn
        End Select

        ' Load the File from Disk, if it was not loaded yet.
        If Not ScanImageList.ListEntryList.ContainsKey(FileNameOfColumn) Then
            ' Send the Load-Request to the Background-Class
            ScanImageList.LoadFile(FileNameOfColumn, ScanImageList)
            Return
        End If

        ' Write File-Properties from the ScanImage-ListEntry to the List
        With ScanImageList.ListEntryList(FileNameOfColumn)
            Select Case e.ColumnIndex
                Case Me.colFileName.Index
                    e.Value = .FileName
                Case Me.colDate.Index
                    e.Value = .RecordDate
                Case Me.colComment.Index
                    e.Value = .Comment
                Case Me.colSize.Index
                    e.Value = .MeasurementPoints
                Case Me.colChannels.Index
                    Dim sChannelNames As String = ""
                    Dim iChannelCounter As Integer = 0
                    Dim iChannelMaxShow As Integer = 6
                    ' Write a list of all Columns in the Data-Table.
                    For Each oChannel As String In .ColumnNames
                        If iChannelCounter <= iChannelMaxShow Then
                            sChannelNames &= oChannel & vbCrLf
                        End If
                        iChannelCounter += 1
                    Next
                    If iChannelCounter > iChannelMaxShow Then
                        sChannelNames &= My.Resources.Template_AdditionalMore.Replace("%%", (iChannelCounter - iChannelMaxShow).ToString("N0"))
                    End If
                    e.Value = sChannelNames
                Case Me.colPreview.Index
                    If Not .PreviewImage Is Nothing And TypeOf .PreviewImage Is Image Then
                        e.Value = .PreviewImage
                    End If
            End Select
        End With
    End Sub

    ''' <summary>
    ''' Selection in ScanImage-Filelist Changed.
    ''' Send Event with the selected List.
    ''' </summary>
    Private Sub dgvScanImageFileList_SelectionChanged(sender As System.Object, e As EventArgs) Handles dgvScanImageFileList.SelectionChanged
        If Not Me.bReady Then Return

        Dim SelectedScanImageFiles As Dictionary(Of String, cFileImport.FileObject) = Me.GetSelectedScanImageFiles

        ' Enable or Disable Buttons depending on a single or multiple Selection.
        Select Case SelectedScanImageFiles.Count
            Case 0

            Case 1
                Me.ListOfScanImages.Clear()
                ScanImageList.LoadFile(SelectedScanImageFiles.First.Key, Me)
            Case Else

        End Select
    End Sub

    ''' <summary>
    ''' Function, that searches for the selected FileName and selects the
    ''' corresponding entry, if it was found.
    ''' </summary>
    Public Sub SelectScanImageInList(ByVal FullFileName As String)
        Dim RowID As Integer = Me.GetRowIndexOfColumnByFileName(FullFileName)
        If RowID > -1 Then
            Me.dgvScanImageFileList.Rows(RowID).Selected = True
        End If
    End Sub
#End Region

#Region "Capture DataError-Event"
    Private Sub DataErrorCatcher(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvScanImageFileList.DataError
        e.Cancel = True
    End Sub
#End Region

#Region "Context Menu Functions"
    ''' <summary>
    ''' On Opening the ContextMenu, add the ColumnNames to the Comboboxes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmFileList_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles cmFileList.Opening
        ' Add the Filter-Columns to the Comboboxes
        Me.bReady = False
        Me.cmnuCBPreviewImageChannel.Items.Clear()
        For Each ChannelName As String In Me.ListOfPreviewImageChannels
            Me.cmnuCBPreviewImageChannel.Items.Add(ChannelName)
        Next

        Me.bReady = True
    End Sub
#End Region

#Region "Implementation of the Multiple ScanImage-Fetcher Interface Functions"

    ''' <summary>
    ''' ScanImages selected
    ''' </summary>
    Private ListOfScanImages As New List(Of cScanImage)

#Region "Interface Functions"
    Public Sub AllScanImagesLoaded() Implements iMultipleScanImagesLoaded.AllScanImagesLoaded
        If ListOfScanImages.Count = 0 Then
            ' Nothing
        ElseIf ListOfScanImages.Count = 1 Then
            RaiseEvent SingleScanImageSelected(ListOfScanImages(0))
        Else
            RaiseEvent MultipleScanImagesSelected(ListOfScanImages)
        End If
    End Sub

    Public Sub OneOfAllScanImagesFetched(ByRef ScanImage As cScanImage) Implements iMultipleScanImagesLoaded.OneOfAllScanImagesFetched
        Me.ListOfScanImages.Add(ScanImage)
    End Sub

    Public Property TotalNumberOfFilesToFetch As Integer Implements iMultipleScanImagesLoaded.TotalNumberOfFilesToFetch

    Public Sub SpectroscopyTableLoaded(ByRef ScanImage As cScanImage) Implements iSingleScanImageLoaded.ScanImageLoaded
        RaiseEvent SingleScanImageSelected(ScanImage)
    End Sub
#End Region
#End Region

End Class
