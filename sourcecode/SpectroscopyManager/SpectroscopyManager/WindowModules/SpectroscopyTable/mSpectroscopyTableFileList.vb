Imports System.ComponentModel
Imports System.Threading

Public Class mSpectroscopyTableFileList
    Implements iMultipleSpectroscopyTablesLoaded
    Implements iSingleSpectroscopyTableLoaded

#Region "Properties"
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
    ''' Variable that tells, if events should be handled or ignored by the Control.
    ''' </summary>
    Private bReady As Boolean = False

    ''' <summary>
    ''' List with preview-image-column-names to be displayed in the context-menu
    ''' </summary>
    Private ListOfPreviewImageColumns As New List(Of String)

    ''' <summary>
    ''' Current Sort-Direction of the List
    ''' </summary>
    Private SortDirection As SortOrder = SortOrder.Ascending

    ''' <summary>
    ''' Current ColumnIndex that is used to sort the list.
    ''' </summary>
    Private SortColumnIndex As Integer

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
    Public Event NearestScanImageInTimeForSpectroscopyTableSearched(ByRef FileObject As cFileImport.FileObject)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor: Just activate the Interface.
    ''' </summary>
    Private Sub mList_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.bReady = True

        ' Set Inital Sort-Column
        Me.SortColumnIndex = Me.colTime.Index

        ' Set Sorting Icon
        Me.dgvSpectroscopyFileList.Columns(SortColumnIndex).HeaderCell.SortGlyphDirection = SortDirection

        ' Hide Test-Window Button
#If Not Debug Then
        Me.cmnuTestSingle.Visible = False
        Me.cmnuTestMultiple.Visible = False
#End If
    End Sub

    ''' <summary>
    ''' Sets the List of Spectroscopy-Table-Objects to be displayed.
    ''' </summary>
    Public Sub SetCurrentlyDisplayedFileList(ByRef FileList As Dictionary(Of String, cFileImport.FileObject))
        Me.bReady = False

        ' Delete Preview Image-Column-List
        Me.ListOfPreviewImageColumns.Clear()

        ' Create new SpectroscopyTable-List object from the given File-List
        Me.SpectroscopyTableList = New cSpectroscopyTableList(FileList)

        ' Row Template Height gets set to the choosen image height
        Me.dgvSpectroscopyFileList.RowTemplate.Height = Me.SpectroscopyTableList.PreviewImageHeigth
        Me.dgvSpectroscopyFileList.Columns(Me.colPreviewImage.Index).Width = Me.SpectroscopyTableList.PreviewImageWidth

        ' Set the Row-Count of the List to start the loading of the DataGridView
        Me.dgvSpectroscopyFileList.Rows.Clear()
        Me.dgvSpectroscopyFileList.RowCount = FileList.Count
        Me.dgvSpectroscopyFileList.ClearSelection()

        Me.bReady = True
    End Sub
#End Region

#Region "File Fetcher Event Handling"
    ' #########################################################################################################
    ' 
    '          -> Perform now specific actions, like showing a List-Entry in the DataGridView
    ' 
    ' #########################################################################################################

    ''' <summary>
    ''' If the ListEntry got fetched successfully, reload the row corresponding to the ListEntry.
    ''' </summary>
    Private Sub ListEntryFetchComplete(ByRef ListEntry As cSpectroscopyTableList.SpectroscopyListEntry) Handles SpectroscopyTableList.ListEntryFetchComplete
        ' Fills the ColumnNames in the Filter-Comboboxes. Only Unique Names
        ' to be displayed in the Context-Menu when activated.
        Me.AdaptPreviewImageColumnFilters(ListEntry.ColumnNames)

        ' Invalidates the row in the DGV to fetch all the loaded details
        ' out of the List-Entry now available
        Me.dgvSpectroscopyFileList.InvalidateRow(Me.GetRowIndexOfColumnByFileName(ListEntry.FullFileName))
    End Sub
#End Region

#Region "Row Identification Functions"
    ''' <summary>
    ''' Adds unique entrys to the ColumnNameList
    ''' </summary>
    Private Sub AdaptPreviewImageColumnFilters(ColumnNames As List(Of String))
        Dim bValuesAdded As Boolean = False
        For Each ColName As String In ColumnNames
            If Not Me.ListOfPreviewImageColumns.Contains(ColName) Then
                Me.ListOfPreviewImageColumns.Add(ColName)
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
        Dim FileNameOfColumn As String = ""

        Dim SpectroscopyTableFileObjects As List(Of cFileImport.FileObject) = SpectroscopyTableList.CurrentFileList.Values.ToList()

        ' Sorting ASC of the List
        Select Case SortColumnIndex
            Case Me.colTime.Index
                SpectroscopyTableFileObjects = SpectroscopyTableFileObjects.OrderBy(Function(obj) obj.RecordDate).ToList
            Case Me.colFileName.Index
                SpectroscopyTableFileObjects = SpectroscopyTableFileObjects.OrderBy(Function(obj) obj.FileName).ToList
        End Select

        ' Sorting DESC -> Reverse the list
        If Me.SortDirection = SortOrder.Descending Then
            SpectroscopyTableFileObjects.Reverse()
        End If

        ' Check which element to load
        For i As Integer = 0 To SpectroscopyTableFileObjects.Count - 1 Step 1
            If i = RowIndex Then
                FileNameOfColumn = SpectroscopyTableFileObjects(i).FullName
                Exit For
            End If
        Next
        Return FileNameOfColumn
    End Function

    ''' <summary>
    ''' Returns the Row-Id of the Column determined by the FileName.
    ''' </summary>
    Private Function GetRowIndexOfColumnByFileName(ByVal FullFileName As String) As Integer
        For Each Row As DataGridViewRow In Me.dgvSpectroscopyFileList.Rows
            If Convert.ToString(Row.Cells(Me.colFullFileName.Index).Value) = FullFileName Then
                Return Row.Index
            End If
        Next
        Return 0
    End Function

    ''' <summary>
    ''' Returns the selected Spectroscopy-Table-FileObjects
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSelectedSpectroscopyTableFiles() As Dictionary(Of String, cFileImport.FileObject)
        Dim lSpectraList As New Dictionary(Of String, cFileImport.FileObject)
        For Each Row As DataGridViewRow In Me.dgvSpectroscopyFileList.SelectedRows
            Dim sFullFileName As String = Me.GetFileNameOfColumnByRowIndex(Row.Index)
            If Not SpectroscopyTableList.CurrentFileList.ContainsKey(sFullFileName) Then Continue For
            lSpectraList.Add(sFullFileName, SpectroscopyTableList.CurrentFileList(sFullFileName))
        Next
        Return lSpectraList
    End Function
#End Region

#Region "Data Grid View Functions, such as Selections and Cell-Value-Fetching"
    ''' <summary>
    ''' Function that Handles the Request for Data by the Spectroscopy-File-List,
    ''' and fills the visible Part of the DataGridView for the SpectroscopyTables.
    ''' </summary>
    Private Sub dgvSpectroscopyFileList_CellValueNeeded(sender As System.Object, e As System.Windows.Forms.DataGridViewCellValueEventArgs) _
        Handles dgvSpectroscopyFileList.CellValueNeeded
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
        If Not SpectroscopyTableList.ListEntryList.ContainsKey(FileNameOfColumn) Then
            ' Send the Load-Request to the Background-Class,
            ' which again processes the fetch of the individual list entry.
            SpectroscopyTableList.LoadFile(FileNameOfColumn, SpectroscopyTableList)
            Return
        End If

        ' Write File-Properties from the Spectroscopy-File to the List
        With SpectroscopyTableList.ListEntryList(FileNameOfColumn)
            Select Case e.ColumnIndex
                Case Me.colFileName.Index
                    e.Value = .FileName
                Case Me.colDataPoints.Index
                    e.Value = .MeasurementPoints
                Case Me.colTime.Index
                    e.Value = .RecordDate
                Case Me.colComment.Index
                    e.Value = .Comment
                Case Me.colColumnList.Index
                    Dim sColumnNames As String = ""
                    Dim iColumnCounter As Integer = 0
                    Dim iColumnMaxShow As Integer = 6
                    ' Write a list of all Columns in the Data-Table.
                    For Each oColumn As String In .ColumnNames
                        If iColumnCounter <= iColumnMaxShow Then
                            sColumnNames &= oColumn & vbCrLf
                        End If
                        iColumnCounter += 1
                    Next
                    If iColumnCounter > iColumnMaxShow Then
                        sColumnNames &= My.Resources.Template_AdditionalMore.Replace("%%", (iColumnCounter - iColumnMaxShow).ToString("N0"))
                    End If
                    e.Value = sColumnNames
                Case Me.colPreviewImage.Index
                    If Not .PreviewImage Is Nothing And TypeOf .PreviewImage Is Image Then
                        e.Value = .PreviewImage
                    End If
            End Select
        End With
    End Sub

    ''' <summary>
    ''' Selection in Spectroscopy-Table-Filelist Changed.
    ''' Send Event with the selected List.
    ''' </summary>
    Private Sub dgvSpectroscopyFileList_SelectionChanged(sender As System.Object, e As EventArgs) Handles dgvSpectroscopyFileList.SelectionChanged
        If Not Me.bReady Then Return

        Dim SelectedSpectroscopyTableFiles As Dictionary(Of String, cFileImport.FileObject) = Me.GetSelectedSpectroscopyTableFiles

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
        End Select

        ' --> Load a single selected SpectroscopyTableObject to display it as a preview!
        ' Multiple selected files only get loaded, if the user uses the context-menu
        ' or - as planned later - drag&drops the selected items.
        If SelectedSpectroscopyTableFiles.Count = 1 Then
            Me.ListOfSelectedSpectroscopyTables.Clear()
            SpectroscopyTableList.LoadFile(SelectedSpectroscopyTableFiles.First.Key, Me)
        End If
    End Sub

    ''' <summary>
    ''' Loads all selected SpectroscopyTables.
    ''' </summary>
    Private Sub cmnuDisplayTogetherAsPreview() Handles cmnuDisplayTogether.Click
        Dim SelectedSpectroscopyTableFiles As Dictionary(Of String, cFileImport.FileObject) = Me.GetSelectedSpectroscopyTableFiles
        If SelectedSpectroscopyTableFiles.Count > 0 Then
            ' --> Load the corresponding SpectroscopyTableObject to display them as a preview!
            Me.ListOfSelectedSpectroscopyTables.Clear()
            SpectroscopyTableList.LoadFiles(SelectedSpectroscopyTableFiles.Keys.ToList, Me)
        End If
    End Sub

    ''' <summary>
    ''' Change the Sorting of the File-List when clicking on the header of the columns.
    ''' </summary>
    Private Sub dgvSpectroscopyFileList_ColumnHeaderClick(sender As System.Object, e As DataGridViewCellMouseEventArgs) _
        Handles dgvSpectroscopyFileList.ColumnHeaderMouseClick
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
            Case Else
                ' Abort
                Return
        End Select
        NewSortColumnIndex = e.ColumnIndex

        ' Reset the symbol of the former sort-column
        If NewSortColumnIndex <> Me.SortColumnIndex Then
            Me.dgvSpectroscopyFileList.Columns(SortColumnIndex).HeaderCell.SortGlyphDirection = SortOrder.None
        End If
        Me.SortColumnIndex = NewSortColumnIndex

        ' Set Sorting Icon
        Me.dgvSpectroscopyFileList.Columns(SortColumnIndex).HeaderCell.SortGlyphDirection = SortDirection

        ' Refresh the List
        For Each Col As DataGridViewColumn In Me.dgvSpectroscopyFileList.Columns
            Me.dgvSpectroscopyFileList.InvalidateColumn(Col.Index)
        Next

    End Sub
#End Region

#Region "Capture DataError-Event"
    Private Sub DataErrorCatcher(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvSpectroscopyFileList.DataError
        e.Cancel = True
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
            Me.dgvSpectroscopyFileList.InvalidateCell(ColumnIndex, RowIndex)
        End If
    End Sub
#End Region

#Region "Context Menu Functions"
    ''' <summary>
    ''' Applies the Settings to the Preview-Images:
    ''' </summary>
    Private Sub btnChangePreviewImages_Click(sender As System.Object, e As System.EventArgs) _
        Handles cmnuCBPreviewImageX.SelectedIndexChanged, cmnuCBPreviewImageY.SelectedIndexChanged, _
                cmnuPreviewImageLogarithmicX.CheckedChanged, cmnuPreviewImageLogarithmicY.CheckedChanged, _
                cmnuPreviewImagePointReduction.CheckedChanged, cmnuCBPreviewImageSize.SelectedIndexChanged
        If Not Me.bReady Then Return

        Me.SpectroscopyTableList.PreviewImageColumnName_X = Convert.ToString(Me.cmnuCBPreviewImageX.SelectedItem)
        Me.SpectroscopyTableList.PreviewImageColumnName_Y = Convert.ToString(Me.cmnuCBPreviewImageY.SelectedItem)
        Me.SpectroscopyTableList.PreviewImageLogX = Me.cmnuPreviewImageLogarithmicX.Checked
        Me.SpectroscopyTableList.PreviewImageLogY = Me.cmnuPreviewImageLogarithmicY.Checked
        Me.SpectroscopyTableList.PreviewImageReducePointsForProcessing = Me.cmnuPreviewImagePointReduction.Checked

        ' Extract PreviewImage Height and Width
        Me.SetPreviewImageSizeFromSizeString(Convert.ToString(Me.cmnuCBPreviewImageSize.SelectedItem))

        ' Save new height in row-template:
        Me.dgvSpectroscopyFileList.RowTemplate.Height = Me.SpectroscopyTableList.PreviewImageHeigth

        ' Invalidates the View, that the List is getting repainted.
        Me.dgvSpectroscopyFileList.InvalidateColumn(Me.colPreviewImage.Index)

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
        Me.dgvSpectroscopyFileList.InvalidateColumn(Me.colPreviewImage.Index)
    End Sub

    ''' <summary>
    ''' Sets the Preview-Image-Size in the list from a size string of the Format:
    ''' WIDTHxHEIGHT Description
    ''' </summary>
    Private Sub SetPreviewImageSizeFromSizeString(ByVal SizeString As String)
        If Not SizeString.Contains(" ") And Not SizeString.Contains("x") Then SizeString = Convert.ToString(Me.cmnuCBPreviewImageSize.Items(0))
        ' Extract PreviewImage Height and Width
        Dim sPreviewImageSize As String() = SizeString.Split(CChar(" "))(0).Split(CChar("x"))

        ' Save the new Dimensions
        Me.SpectroscopyTableList.PreviewImageHeigth = Convert.ToInt32(sPreviewImageSize(1))
        Me.SpectroscopyTableList.PreviewImageWidth = Convert.ToInt32(sPreviewImageSize(0))

        ' Apply the new dimensions to all Rows and the Column
        Me.dgvSpectroscopyFileList.RowTemplate.Height = Me.SpectroscopyTableList.PreviewImageHeigth
        For Each Row As DataGridViewRow In Me.dgvSpectroscopyFileList.Rows
            Row.Height = Me.SpectroscopyTableList.PreviewImageHeigth
        Next
        Me.dgvSpectroscopyFileList.Columns(Me.colPreviewImage.Index).Width = Me.SpectroscopyTableList.PreviewImageWidth

        ' Save the Settings
        My.Settings.LastPreviewImageList_SizeString = SizeString
        My.Settings.LastPreviewImageList_Height = Me.SpectroscopyTableList.PreviewImageHeigth
        My.Settings.LastPreviewImageList_Width = Me.SpectroscopyTableList.PreviewImageWidth
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' On Opening the ContextMenu, add the ColumnNames to the Comboboxes
    ''' </summary>
    Private Sub cmFileList_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles cmFileList.Opening
        ' Add the Filter-Columns to the Comboboxes
        Me.bReady = False
        Me.cmnuCBPreviewImageX.Items.Clear()
        Me.cmnuCBPreviewImageY.Items.Clear()
        For Each ColumnName As String In Me.ListOfPreviewImageColumns
            Me.cmnuCBPreviewImageX.Items.Add(ColumnName)
            Me.cmnuCBPreviewImageY.Items.Add(ColumnName)
            If Me.SpectroscopyTableList.PreviewImageColumnName_X = ColumnName Then Me.cmnuCBPreviewImageX.SelectedItem = ColumnName
            If Me.SpectroscopyTableList.PreviewImageColumnName_Y = ColumnName Then Me.cmnuCBPreviewImageY.SelectedItem = ColumnName
        Next

        ' Load Preview Image size string
        If Me.cmnuCBPreviewImageSize.Items.Contains(My.Settings.LastPreviewImageList_SizeString) Then
            Me.cmnuCBPreviewImageSize.SelectedItem = My.Settings.LastPreviewImageList_SizeString
        End If
        Me.bReady = True
    End Sub
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

#Region "Exporting - OK"
    ''' <summary>
    ''' Opens the Export-Wizard with the selected Files.
    ''' </summary>
    Private Sub OpenExportWizardWithSelectedFiles(sender As System.Object, e As System.EventArgs) Handles cmnuOpenExportWizard.Click
        ' Open the Window only, if the List contains at least one File.
        If Me.GetSelectedSpectroscopyTableFiles.Count < 1 Then Return

        Dim wdExportWizard As New wExportWizard_SpectroscopyTable
        ' Add the Files for the Export.
        wdExportWizard.SetExportSpectroscopyTableNames(Me.GetSelectedSpectroscopyTableFiles)

        ' Show Dialog
        wdExportWizard.ShowDialog()
        ' Destroy the Window.
        wdExportWizard.Dispose()
    End Sub
#End Region

#Region "Multiple File Averaging - OK"

    ''' <summary>
    ''' Averages Data in the Same Column of Multiple Files
    ''' </summary>
    Private Sub cmnuAverageDataMultipleFiles_Click(sender As System.Object, e As System.EventArgs) _
        Handles cmnuAverageDataMultipleFiles.Click

        Dim SelectedSpectroscopyFileNames As List(Of String) = Me.GetSelectedSpectroscopyTableFiles.Keys.ToList

        Dim Window As New wDataAveragingMultipleFiles

        AddHandler Window.FormClosed, AddressOf AverageColumnValuesWindowClosed

        Me.SpectroscopyTableList.LoadFiles(SelectedSpectroscopyFileNames,
                                           Window)
    End Sub

    ''' <summary>
    ''' Opens a window for averaging values of Columns from the selected SpectroscopyTables
    ''' </summary>
    Private Sub AverageColumnValuesWindowClosed(sender As Object, e As FormClosedEventArgs)
        Dim SpectroscopyTableList As List(Of cSpectroscopyTable) = DirectCast(sender, wDataAveragingMultipleFiles).CurrentListOfSpectroscopyTables
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

#Region "Value-Cropping - OK"
    ''' <summary>
    ''' List that contains the DataCropperObjects.
    ''' </summary>
    ''' <remarks></remarks>
    Private DataCropperList As New Dictionary(Of String, cSpectroscopyTableDataCropper)

    Private CropperMutex As New Mutex

    ''' <summary>
    ''' Opens the Window for Data-Cropping.
    ''' </summary>
    Private Sub tmnuCropData_Click(sender As System.Object, e As System.EventArgs) Handles cmnuCropData.Click
        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        Dim CroppingWindow As New wDataCropping

        'CroppingWindow.SetSpectroscopyTableFile(Me.GetSelectedSpectroscopyTableFiles.First.Value)
        CroppingWindow.ShowDialog(Me.GetSelectedSpectroscopyTableFiles.First.Value)
        CroppingWindow.Dispose()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(Me.GetSelectedSpectroscopyTableFiles.First.Key)

        ' Invalidate Cell of the ColumnList and the Datapoint-Number
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(Me.GetSelectedSpectroscopyTableFiles.First.Key))
        InvalidateCell(Me.colDataPoints.Index, Me.GetRowIndexOfColumnByFileName(Me.GetSelectedSpectroscopyTableFiles.First.Key))
    End Sub

    ''' <summary>
    ''' Crops the data using the last settings used Window.
    ''' </summary>
    Private Sub tmnuCropDataUsingLastSettings_Click(sender As System.Object, e As System.EventArgs) Handles cmnuCropDataUsingLastSettings.Click
        ' Check, if Settings had been saved.
        With My.Settings
            If .LastCropping_LeftPoint = -1 Or
               .LastCropping_RightPoint = -1 Or
               .LastCropping_ColumnX = "" Then
                MessageBox.Show(My.Resources.Message_SettingsMissing,
                                My.Resources.Title_SettingsMissing,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Return
            End If
        End With


        CropperMutex.WaitOne()
        DataCropperList.Clear()
        For Each FileObjectKV As KeyValuePair(Of String, cFileImport.FileObject) In Me.GetSelectedSpectroscopyTableFiles
            ' Add the new DataCropperObject
            DataCropperList.Add(FileObjectKV.Key, New cSpectroscopyTableDataCropper(FileObjectKV.Value))

            ' Add the Events of the Cropper-Objects to the DataNormalier
            AddHandler DataCropperList(FileObjectKV.Key).FileCroppingComplete, AddressOf Me.CroppingOfDataComplete

            ' Start the File-Normalization-Process
            DataCropperList(FileObjectKV.Key).CropColumnWITHAutomaticFetching(My.Settings.LastCropping_ColumnX,
                                                                              My.Settings.LastCropping_LeftPoint,
                                                                              My.Settings.LastCropping_RightPoint)
        Next
        CropperMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Function called after the Cropper-Process -> removes the DataCropper again,
    ''' and invalidates the row to update the ColumnNames
    ''' </summary>
    Private Sub CroppingOfDataComplete(ByRef SpectroscopyTable As cSpectroscopyTable)
        CropperMutex.WaitOne()
        DataCropperList(SpectroscopyTable.FullFileName).SaveBackToFileObject()
        DataCropperList.Remove(SpectroscopyTable.FullFileName)
        CropperMutex.ReleaseMutex()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(SpectroscopyTable.FullFileName)

        ' Invalidate the Cell of the ColumnList and the datapointnumber
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTable.FullFileName))
        InvalidateCell(Me.colDataPoints.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTable.FullFileName))
    End Sub
#End Region

#Region "LineScan Plotting - OK"
    ''' <summary>
    ''' Open the Window to create Contour-Plots from the Line-Scan.
    ''' </summary>
    Private Sub cmnuCreateContourPlotFromLinescans_Click(sender As System.Object, e As System.EventArgs) _
        Handles cmnuCreateContourPlotFromLinescans.Click

        Dim SelectedSpectroscopyFileNames As List(Of String) = Me.GetSelectedSpectroscopyTableFiles.Keys.ToList

        ' Check before, if one or more files are selected.
        If SelectedSpectroscopyFileNames.Count < 1 Then Return

        Dim Window As New wLineScanPlot

        Me.SpectroscopyTableList.LoadFiles(SelectedSpectroscopyFileNames,
                                           Window)
    End Sub
#End Region

#Region "Data Normalization - OK"
    ''' <summary>
    ''' List that contains the DataNormalizerObjects.
    ''' </summary>
    Private DataNormalizerList As New Dictionary(Of String, cSpectroscopyTableDataNormalizer)

    Private NormalizerMutex As New Mutex

    ''' <summary>
    ''' Opens the Window for Data-Normalization.
    ''' </summary>
    Private Sub tmnuNormalizeData_Click(sender As System.Object, e As System.EventArgs) Handles cmnuNormalizeData.Click
        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        Dim NormalizationWindow As New wDataNormalization

        NormalizationWindow.SetSpectroscopyTableFile(Me.GetSelectedSpectroscopyTableFiles.First.Value)
        NormalizationWindow.ShowDialog()
        NormalizationWindow.Dispose()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(Me.GetSelectedSpectroscopyTableFiles.First.Key)

        ' Invalidate Cell of the ColumnList
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(Me.GetSelectedSpectroscopyTableFiles.First.Key))
    End Sub

    ''' <summary>
    ''' Renormalizes the Data using the last Settings from the Renormalization Window.
    ''' </summary>
    Private Sub tmnuNormalizeDataUsingLastSettings_Click(sender As System.Object, e As System.EventArgs) Handles cmnuNormalizeDataUsingLastSettings.Click
        ' Check, if Settings had been saved.
        With My.Settings
            If .LastNormalization_NewColumnName = "" Or
               .LastNormalization_ColumnX = "" Or
               .LastNormalization_ColumnToNormalize = "" Or
               .LastNormalization_SmoothNeighbors = 0 Then
                MessageBox.Show(My.Resources.Message_SettingsMissing,
                                My.Resources.Title_SettingsMissing,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Return
            End If
        End With


        NormalizerMutex.WaitOne()
        DataNormalizerList.Clear()
        For Each FileObjectKV As KeyValuePair(Of String, cFileImport.FileObject) In Me.GetSelectedSpectroscopyTableFiles
            ' Add the new DataNormalizerObject
            DataNormalizerList.Add(FileObjectKV.Key, New cSpectroscopyTableDataNormalizer(FileObjectKV.Value))

            ' Add the Events of the Normalizer-Objects to the DataNormalier
            AddHandler DataNormalizerList(FileObjectKV.Key).FileNormalizedComplete, AddressOf Me.NormalizingOfDataComplete

            ' Start the File-Normalization-Process
            DataNormalizerList(FileObjectKV.Key).NormalizeColumnWITHSmoothing(
                My.Settings.LastNormalization_ColumnX,
                My.Settings.LastNormalization_ColumnToNormalize,
                My.Settings.LastNormalization_LeftPoint,
                My.Settings.LastNormalization_RightPoint,
                DirectCast(My.Settings.LastNormalization_SmoothMethod, cNumericalMethods.SmoothingMethod),
                My.Settings.LastNormalization_SmoothNeighbors,
                My.Settings.LastNormalization_NewColumnName)
        Next
        NormalizerMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Function called after the Normalization-Process -> removes the DataNormalizer again,
    ''' and invalidates the row to update the ColumnNames
    ''' </summary>
    Private Sub NormalizingOfDataComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef NormalizedDataColumn As cSpectroscopyTable.DataColumn)
        NormalizerMutex.WaitOne()
        ' Save the Normalized Column and then removes the DataNormalizer from the list
        DataNormalizerList(SpectroscopyTable.FullFileName).SaveNormalizedColumnToFileObject(My.Settings.LastNormalization_NewColumnName)

        DataNormalizerList.Remove(SpectroscopyTable.FullFileName)
        NormalizerMutex.ReleaseMutex()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(SpectroscopyTable.FullFileName)

        ' Invalidate the Cell of the ColumnList.
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTable.FullFileName))
    End Sub
#End Region

#Region "Data ReNormalization - OK"
    ''' <summary>
    ''' List that contains the DatarenormalizerObjects.
    ''' </summary>
    Private DataRenormalizerList As New Dictionary(Of String, cSpectroscopyTableDataRenormalizer)

    Private RenormalizerMutex As New Mutex

    ''' <summary>
    ''' Opens the Window for Data-Renormalization.
    ''' </summary>
    Private Sub tmnuRenormalizeData_Click(sender As System.Object, e As System.EventArgs) Handles cmnuRenormalizeData.Click
        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        ' Create the RenormalizationWindow and Set the first SpectroscopyTable in the List -> it should be the only one.
        Dim RenormalizationWindow As New wDataRenormalization
        RenormalizationWindow.SetSpectroscopyTableFileObject(Me.GetSelectedSpectroscopyTableFiles.First.Value)
        RenormalizationWindow.ShowDialog()
        RenormalizationWindow.Dispose()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(Me.GetSelectedSpectroscopyTableFiles.First.Key)

        ' Invalidate Cell of Renormalized Row, where the ColumnList can be modified:
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(Me.GetSelectedSpectroscopyTableFiles.First.Key))
    End Sub

    ''' <summary>
    ''' Renormalizes the Data using the last Settings from the Renormalization Window.
    ''' </summary>
    Private Sub tmnuRenormalizeDataUsingLastSettings_Click(sender As System.Object, e As System.EventArgs) Handles cmnuRenormalizeDataUsingLastSettings.Click
        ' Check, if Settings had been saved.
        With My.Settings
            If .LastRenormalization_DerivedColumnName = "" Or
               .LastRenormalization_RenormColumnName = "" Or
               .LastRenormalization_SmoothedColumnName = "" Or
               .LastRenormalization_SmoothNeighbors = 0 Or
               .LastRenormalization_SourceColumnX = "" Or
               .LastRenormalization_SourceColumnY = "" Or
               .LastRenormalization_TargetColumnX = "" Or
               .LastRenormalization_TargetColumnY = "" Then
                MessageBox.Show(My.Resources.Message_SettingsMissing,
                                My.Resources.Title_SettingsMissing,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Return
            End If
        End With

        RenormalizerMutex.WaitOne()
        DataRenormalizerList.Clear()
        For Each FileObjectKV As KeyValuePair(Of String, cFileImport.FileObject) In Me.GetSelectedSpectroscopyTableFiles
            ' Add the new DataRenormalizerObject
            DataRenormalizerList.Add(FileObjectKV.Key, New cSpectroscopyTableDataRenormalizer(FileObjectKV.Value))

            ' Add the Events of the Renormalizer-Objects to the DataRenormalier
            AddHandler DataRenormalizerList(FileObjectKV.Key).FileRenormalizedComplete, AddressOf Me.RenormalizingOfDataComplete

            ' Start the File-Renormalization-Process
            DataRenormalizerList(FileObjectKV.Key).RenormalizeColumnWITHDerivation(
                My.Settings.LastRenormalization_SourceColumnX,
                My.Settings.LastRenormalization_SourceColumnY,
                DirectCast(My.Settings.LastRenormalization_SmoothMethod, cNumericalMethods.SmoothingMethod),
                My.Settings.LastRenormalization_SmoothNeighbors,
                My.Settings.LastRenormalization_TargetColumnX,
                My.Settings.LastRenormalization_TargetColumnY)
        Next
        RenormalizerMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Function called after the Renormalization-Process -> removes the DataRenormalizer again,
    ''' and invalidates the row to update the ColumnNames
    ''' </summary>
    Private Sub RenormalizingOfDataComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RenormalizedDataColumn As cSpectroscopyTable.DataColumn)
        RenormalizerMutex.WaitOne()
        ' Save the Renormalized Column and then removes the DataRenormalizer from the list
        DataRenormalizerList(SpectroscopyTable.FullFileName).SaveRenormalizedColumnToFileObject(My.Settings.LastRenormalization_RenormColumnName)
        If My.Settings.LastRenormalization_SaveDerived Then
            DataRenormalizerList(SpectroscopyTable.FullFileName).SaveDerivatedColumnToFileObject(My.Settings.LastRenormalization_DerivedColumnName)
        End If
        If My.Settings.LastRenormalization_SaveSmoothed Then
            DataRenormalizerList(SpectroscopyTable.FullFileName).SaveSmoothedColumnToFileObject(My.Settings.LastRenormalization_SmoothedColumnName)
        End If

        DataRenormalizerList.Remove(SpectroscopyTable.FullFileName)
        RenormalizerMutex.ReleaseMutex()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(SpectroscopyTable.FullFileName)

        ' Invalidate the Cell of the ColumnList.
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTable.FullFileName))
    End Sub
#End Region

#Region "Copying to Clipboard - OK"
    ''' <summary>
    ''' If only a single File is selected, copy the Data-Table to the Clipboard,
    ''' by requesting a FileFetch Process.
    ''' </summary>
    Private Sub CopyToClipboard_AsciiEnglish_Launcher(sender As System.Object, e As System.EventArgs) _
        Handles cmnuCopyDataToClipboard.Click

        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        Dim Clipboard As New cClipboard_SpectroscopyTable
        Clipboard.ExportMethod = New cExportMethod_Ascii_TAB(True, False)
        AddHandler Clipboard.ClipboardContentReady, AddressOf CopyToClipboard

        ' Launch fetching procedure
        Me.SpectroscopyTableList.LoadFile(Me.GetSelectedSpectroscopyTableFiles.First.Key, Clipboard)
    End Sub

    ''' <summary>
    ''' If only a single File is selected, copy the Data-Table to the Clipboard in an
    ''' Origin compatible format inserting two extra lines.
    ''' </summary>
    Private Sub CopyToClipboard_OriginCompatible_Launcher(sender As System.Object, e As System.EventArgs) _
        Handles cmnuCopyDataToClipboardOriginCompatible.Click

        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        Dim Clipboard As New cClipboard_SpectroscopyTable
        Clipboard.ExportMethod = New cExportMethod_Ascii_ORIGIN(1) ' Add an empty line between header and data
        AddHandler Clipboard.ClipboardContentReady, AddressOf CopyToClipboard

        ' Launch fetching procedure
        Me.SpectroscopyTableList.LoadFile(Me.GetSelectedSpectroscopyTableFiles.First.Key, Clipboard)
    End Sub

    Private Delegate Sub _CopyToClipboard(ByVal Text As String)
    ''' <summary>
    ''' Function for Copying the desired Text to the Clipboard.
    ''' </summary>
    Private Sub CopyToClipboard(ByVal Text As String)
        If Me.InvokeRequired Then
            Dim _delegate As New _CopyToClipboard(AddressOf CopyToClipboard)
            Me.Invoke(_delegate, Text)
        Else
            Clipboard.SetDataObject(Text)
        End If
    End Sub
#End Region

#Region "Nearest ScanImage Searched - OK"
    ''' <summary>
    ''' Requests by an event to select the nearest Scan-Image for the current SpectroscopyFile.
    ''' </summary>
    Private Sub cmnuShowNearestScanImage_Click(sender As System.Object, e As System.EventArgs) Handles cmnuShowNearestScanImage.Click
        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        RaiseEvent NearestScanImageInTimeForSpectroscopyTableSearched(Me.GetSelectedSpectroscopyTableFiles.First.Value)
    End Sub
#End Region

#Region "Data Smoothing - OK"
    ''' <summary>
    ''' List that contains the DataSmootherObjects.
    ''' </summary>
    Private DataSmootherList As New Dictionary(Of String, cSpectroscopyTableDataSmoother)

    Private SmoothingMutex As New Mutex

    ''' <summary>
    ''' Opens a Window for Smoothing a certain Column.
    ''' </summary>
    Private Sub tmnuSmoothData_Click(sender As System.Object, e As System.EventArgs) Handles cmnuSmoothData.Click
        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        ' Create the Smoothing Window and Set the first SpectroscopyTable in the List -> it should be the only one.
        Dim SmoothingWindow As New wDataSmoothing
        SmoothingWindow.SetSpectroscopyTableFile(Me.GetSelectedSpectroscopyTableFiles.First.Value)
        SmoothingWindow.ShowDialog()
        SmoothingWindow.Dispose()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(Me.GetSelectedSpectroscopyTableFiles.First.Key)

        ' Invalidate Cell of Smoothed Row, where the ColumnList can be modified:
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(Me.GetSelectedSpectroscopyTableFiles.First.Key))
    End Sub

    ''' <summary>
    ''' Smoothes the Spectroscopy-Files using the last settings.
    ''' </summary>
    Private Sub cmnuSmoothDataUsingLastSettings_Click(sender As System.Object, e As System.EventArgs) Handles cmnuSmoothDataUsingLastSettings.Click
        ' Check, if Settings had been saved.
        With My.Settings
            If .LastSmoothing_SmoothedColumnName = "" Or
               .LastSmoothing_ColumnToSmooth = "" Or
               .LastSmoothing_ColumnX = "" Or
               .LastSmoothing_SmoothNeighbors = 0 Then
                MessageBox.Show(My.Resources.Message_SettingsMissing,
                                My.Resources.Title_SettingsMissing,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Return
            End If
        End With

        SmoothingMutex.WaitOne()
        DataSmootherList.Clear()
        For Each FileObjectKV As KeyValuePair(Of String, cFileImport.FileObject) In Me.GetSelectedSpectroscopyTableFiles
            ' Add the new DataSmootherObject
            DataSmootherList.Add(FileObjectKV.Key, New cSpectroscopyTableDataSmoother(FileObjectKV.Value))

            ' Add the Events of the Smoothing-Objects to the DataSmoother
            AddHandler DataSmootherList(FileObjectKV.Key).FileSmoothingComplete, AddressOf Me.SmoothingOfDataComplete

            ' Start the File-Smoothing-Process
            DataSmootherList(FileObjectKV.Key).SmoothColumnWITHAutomaticFetching(
                My.Settings.LastSmoothing_ColumnToSmooth,
                DirectCast(My.Settings.LastSmoothing_SmoothMethod, cNumericalMethods.SmoothingMethod),
                My.Settings.LastSmoothing_SmoothNeighbors)
        Next
        SmoothingMutex.ReleaseMutex()
    End Sub

    ''' <summary>
    ''' Function called after the Smoothing-Process -> removes the DataSmoother again,
    ''' and invalidates the row to update the ColumnNames
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SmoothingOfDataComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef SmoothedDataColumn As cSpectroscopyTable.DataColumn)
        SmoothingMutex.WaitOne()
        ' Save the Smoothed Column and then remove the DataSmoother from the list
        DataSmootherList(SpectroscopyTable.FullFileName).SaveSmoothedColumnToFileObject(My.Settings.LastSmoothing_SmoothedColumnName)

        DataSmootherList.Remove(SpectroscopyTable.FullFileName)
        SmoothingMutex.ReleaseMutex()

        ' Delete the File-List-Entry to request a refetch of the file, if the Columns have changed
        Me.SpectroscopyTableList.RemoveListEntry(SpectroscopyTable.FullFileName)

        ' Invalidate the Cell of the ColumnList.
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(SpectroscopyTable.FullFileName))
    End Sub
#End Region

#Region "Non Linear Fitting - OK"
    ''' <summary>
    ''' If a single file is selected, load the file and start the Fit-Window
    ''' </summary>
    Private Sub FitNonLinear_Click(sender As System.Object, e As System.EventArgs) Handles cmnuFitNonLinear.Click


        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        Dim FitWindow As New wFitBase

        ' Launch fetching procedure
        Me.SpectroscopyTableList.LoadFile(Me.GetSelectedSpectroscopyTableFiles.First.Key,
                                          FitWindow)

        ' Invalidate Cell of Fitted Row, where the ColumnList can be modified:
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(Me.GetSelectedSpectroscopyTableFiles.First.Key))
    End Sub
#End Region

#Region "Implementation of the Multiple SpectroscopyTable-Fetcher Interface Functions"

    ''' <summary>
    ''' SpectroscopyTables selected
    ''' </summary>
    Private ListOfSelectedSpectroscopyTables As New List(Of cSpectroscopyTable)

#Region "Interface Functions"
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

    Public Property TotalNumberOfFilesToFetch As Integer Implements iMultipleSpectroscopyTablesLoaded.TotalNumberOfFilesToFetch

    Public Sub SpectroscopyTableLoaded(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded
        RaiseEvent SingleSpectroscopyTableSelected(SpectroscopyTable)
    End Sub
#End Region
#End Region

#Region "TEST-Functions"
    ''' <summary>
    ''' Function for testing purposes
    ''' </summary>
    Private Sub cmnuTestSingle_Click(sender As Object, e As EventArgs) Handles cmnuTestSingle.Click
        ' Check before, if just a single file is selected.
        If Me.GetSelectedSpectroscopyTableFiles.Count <> 1 Then Return

        Dim oTest As New wFitBase

        ' Launch fetching procedure
        Me.SpectroscopyTableList.LoadFile(Me.GetSelectedSpectroscopyTableFiles.First.Key,
                                          oTest)

        ' Invalidate Cell of Fitted Row, where the ColumnList can be modified:
        InvalidateCell(Me.colColumnList.Index, Me.GetRowIndexOfColumnByFileName(Me.GetSelectedSpectroscopyTableFiles.First.Key))
    End Sub
#End Region
End Class