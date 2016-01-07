Imports System.ComponentModel

Public Class mLineScanViewer

#Region "Properties"
    ''' <summary>
    ''' List of SpectroscopyFiles used to display the LineScan
    ''' </summary>
    ''' <remarks></remarks>
    Private lSpectroscopyTables As List(Of cSpectroscopyTable)

    ''' <summary>
    ''' LineScan Object
    ''' </summary>
    ''' <remarks></remarks>
    Private oLineScanPlot As cLineScanPlot

    Private bReady As Boolean = False

    ''' <summary>
    ''' Line-Scan-List that is pending for loading.
    ''' This object is used as temporary List-Object,
    ''' if the Background-Worker is busy while getting the
    ''' task for drawing a new image.
    ''' </summary>
    Private oLineScanListPending As List(Of cSpectroscopyTable)

    ''' <summary>
    ''' Background-Worker Object to load the Scan-Image in a separate Thread.
    ''' </summary>
    Private ImageLoadingBackgroundWorker As New BackgroundWorker

    Private ImageFetcherResult As Bitmap
    Private ImageFetcherTargetWidth As Integer
    Private ImageFetcherTargetHeight As Integer
    Private ImageFetcherColumnX As String
    Private ImageFetcherColumnZ As String
    Private ImageFetcherColorScheme As cColorScheme
    Private ImageFetcherRedrawPending As Boolean = False
#End Region

#Region "Module Load Functions"
    ''' <summary>
    ''' Constructor: Sets the Adresses for the Scan-Image Background-Worker
    ''' </summary>
    Private Sub mLineScanViewer_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Set Worker-Settings
        With Me.ImageLoadingBackgroundWorker
            .WorkerReportsProgress = True
            .WorkerSupportsCancellation = True
            AddHandler .DoWork, AddressOf LineScanImageFetcher
            AddHandler .ProgressChanged, AddressOf ImageFetcher_ReportProgress
            AddHandler .RunWorkerCompleted, AddressOf ImageFetcher_FetchComplete
        End With
        Me.RecalculateImage()
    End Sub

    ''' <summary>
    ''' Sets the Object for this Module.
    ''' has to be called to perform some actions.
    ''' </summary>
    Public Sub SetLineScanList(ByRef ListOfSpectroscopyFiles As List(Of cSpectroscopyTable))
        ' Save the Image into the Pending Object, if the Worker is busy
        ' and send the Worker the Cancellation Signal
        ' If the Worker is idling, then just set the new Scan-Image:
        If Me.ImageLoadingBackgroundWorker.IsBusy Then
            ' BUSY
            '######
            Me.oLineScanListPending = ListOfSpectroscopyFiles
        Else
            ' IDLE
            '######
            
            ' Sort by Names in advance -> is the usual sorting, that the
            ' Scan-Programs name their Files (e.g. with dates or numbers in the File-Name)
            Dim SortedSpectroscopyFiles As New SortedList(Of String, cSpectroscopyTable)

            ' Extract all Names in the new sorting:
            For i As Integer = 0 To ListOfSpectroscopyFiles.Count - 1 Step 1
                SortedSpectroscopyFiles.Add(ListOfSpectroscopyFiles(i).FileNameWithoutPathAndExtension, ListOfSpectroscopyFiles(i))
            Next

            Me.lSpectroscopyTables = SortedSpectroscopyFiles.Values.ToList

            ' Create a new LineScanPlot From the Data:
            Me.oLineScanPlot = New cLineScanPlot(Me.lSpectroscopyTables)

            ' Common Columns
            Dim lCommonColumnNames As List(Of String) = cSpectroscopyTable.GetCommonColumns(Me.lSpectroscopyTables)

            ' Clear Old Columns:
            Me.lbCommonColumnsX.Items.Clear()
            Me.lbCommonColumnsZ.Items.Clear()

            ' Write the Common Columns to the Comboboxes:
            For Each ColName As String In lCommonColumnNames
                Me.lbCommonColumnsX.Items.Add(ColName)
                Me.lbCommonColumnsZ.Items.Add(ColName)
            Next

            ' Try to load the last used Columns:
            If Me.lbCommonColumnsX.Items.Contains(My.Settings.LastLineScan_ColumnX) Then Me.lbCommonColumnsX.SelectedIndex = Me.lbCommonColumnsX.Items.IndexOf(My.Settings.LastLineScan_ColumnX)
            If Me.lbCommonColumnsZ.Items.Contains(My.Settings.LastLineScan_ColumnZ) Then Me.lbCommonColumnsZ.SelectedIndex = Me.lbCommonColumnsZ.Items.IndexOf(My.Settings.LastLineScan_ColumnZ)

            ' Add Filenames to sorting list:
            With Me.dgvFileSortList.Rows
                .Clear()
                For Each oSpectroscopyTable As cSpectroscopyTable In Me.lSpectroscopyTables
                    Dim i As Integer = .Add()
                    With .Item(i)
                        .Cells(Me.colName.Index).Value = oSpectroscopyTable.FileNameWithoutPath
                    End With
                Next
            End With

            Me.bReady = True

            Me.SelectedColumnsChanged()
        End If

    End Sub
#End Region

#Region "Threaded Image-Load Functions"
    ''' <summary>
    ''' If the Image is fetched, check, if the progress was canceled
    ''' or, if another image is pending. If so, then do not paint the obtained image
    ''' and simply reload.
    ''' </summary>
    Private Sub ImageFetcher_FetchComplete(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ' Check, if a Scan-Image is pending, then just reload the Scan-Image
        If Not Me.oLineScanListPending Is Nothing Then
            ' Scan Image Pending
            '####################
            Me.SetLineScanList(Me.oLineScanListPending)
            Me.oLineScanListPending = Nothing
        Else
            ' Scan Image not Pending -> Display Calculated Image, if the worker was not cancelled.
            If Not e.Cancelled Then
                ' Paint rescaled image to draw-area:
                'Me.pgProgress.Visible = False
                Me.pbLineScan.Image = Me.ImageFetcherResult
                Me.pbLineScan.Visible = True
            End If

            If Me.ImageFetcherRedrawPending Then
                Me.ImageFetcherRedrawPending = False
                Me.RecalculateImage()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Function for reporting the Progress of the Image-Creating Worker.
    ''' </summary>
    Private Sub ImageFetcher_ReportProgress(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
        If e.ProgressPercentage <= 100 Then
            Me.pgProgress.Value = e.ProgressPercentage
        Else
            Me.pgProgress.Value = 100
        End If
        'Me.lblProgressBar.Text = Convert.ToString(e.UserState)
    End Sub

    ''' <summary>
    ''' Scan-Image-Fetch-Function
    ''' </summary>
    Private Sub LineScanImageFetcher(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ' Calculate Image:
        Me.oLineScanPlot.ColorScheme = Me.ImageFetcherColorScheme

        Try
            ' Calculate Image:
            Dim bm As Bitmap = oLineScanPlot.CreateImage(Me.pbLineScan.Width,
                                                         Me.pbLineScan.Height,
                                                         Convert.ToSingle(Me.vsValueRangeSelector.SelectedMaxValue),
                                                         Convert.ToSingle(Me.vsValueRangeSelector.SelectedMinValue),
                                                         Me.ImageFetcherColumnX,
                                                         Me.ImageFetcherColumnZ)
            Me.ImageFetcherResult = Me.oLineScanPlot.Image
        Catch ex As ArgumentOutOfRangeException
            MessageBox.Show(My.Resources.rLineScanViewer.ErrorDrawing & ex.Message,
                            My.Resources.rLineScanViewer.ErrorDrawing_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As ArgumentException
            MessageBox.Show(ex.Message,
                            My.Resources.rLineScanViewer.ErrorDrawing_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ''' <summary>
    ''' Recalculates the LineScan when the selected Value-Range changed.
    ''' </summary>
    Public Sub RecalculateImage() Handles vsValueRangeSelector.SelectedRangeChanged, Me.Resize, cpColorPicker.SelectedColorSchemaChanged
        If Not Me.bReady Then Return
        If Me.lSpectroscopyTables Is Nothing Then Return
        If Me.lbCommonColumnsZ.SelectedItem Is Nothing Then Return
        If Me.lbCommonColumnsX.SelectedItem Is Nothing Then Return

        ' If the Fetcher is Busy, Set the Redraw-Pending Variable to
        ' tell the Fetcher to redraw with the same scan-object, if it finishes.
        If Me.ImageLoadingBackgroundWorker.IsBusy Then
            Me.ImageFetcherRedrawPending = True
            Return
        End If

        ' Set ImageFetcherProperties
        Me.ImageFetcherTargetWidth = Me.pbLineScan.Width
        Me.ImageFetcherTargetHeight = Me.pbLineScan.Height
        Me.ImageFetcherColorScheme = Me.cpColorPicker.GetSelectedColorScheme
        Me.ImageFetcherColumnX = Convert.ToString(Me.lbCommonColumnsX.SelectedItem)
        Me.ImageFetcherColumnZ = Convert.ToString(Me.lbCommonColumnsZ.SelectedItem)

        'Me.pgProgress.Visible = True
        Me.pbLineScan.Visible = False

        Me.ImageLoadingBackgroundWorker.RunWorkerAsync()
    End Sub
#End Region

#Region "Interface Functions"
    ''' <summary>
    ''' Returns all Values in the Spectroscopy-Tables for displaying them in the ValueRangeSelector
    ''' </summary>
    Private Function GetTotalValueArray() As Double()
        Dim arr As New List(Of Double)
        For Each oSpectroscopyTable As cSpectroscopyTable In Me.lSpectroscopyTables
            Dim ValueColumn As cSpectroscopyTable.DataColumn = oSpectroscopyTable.Column(Convert.ToString(Me.lbCommonColumnsZ.SelectedItem))
            arr.AddRange(ValueColumn.Values)
        Next
        If arr.Count = 0 Then
            arr.Add(0)
        End If
        Return arr.ToArray
    End Function

    ''' <summary>
    ''' Shift selected Spectroscopy File Up or Down in the Sort-List.
    ''' </summary>
    Private Sub btnSortFiles_Click(sender As System.Object, e As System.EventArgs) _
        Handles btnSortFiles_OneUp.Click, btnSortFiles_OneDown.Click, btnSortFiles_Top.Click, btnSortFiles_Bottom.Click

        If Me.dgvFileSortList.SelectedRows.Count <> 1 Then Return

        Try
            Dim curr_index As Integer = Me.dgvFileSortList.CurrentCell.RowIndex
            Dim curr_col_index As Integer = Me.dgvFileSortList.CurrentCell.ColumnIndex
            Dim curr_row As DataGridViewRow = Me.dgvFileSortList.CurrentRow

            Dim NewIndex As Integer = 0

            With Me.dgvFileSortList.SelectedRows(0)
                If sender Is Me.btnSortFiles_OneUp Then
                    If .Index <= 0 Then Return

                    ' Shift Row Up:
                    NewIndex = curr_index - 1
                ElseIf sender Is Me.btnSortFiles_Top Then
                    If .Index <= 0 Then Return

                    ' Shift Row To Top of the List:
                    NewIndex = 0
                ElseIf sender Is Me.btnSortFiles_OneDown Then
                    If .Index >= Me.dgvFileSortList.Rows.Count - 1 Then Return

                    ' Shift Row Down:
                    NewIndex = curr_index + 1
                ElseIf sender Is Me.btnSortFiles_Bottom Then
                    If .Index >= Me.dgvFileSortList.Rows.Count - 1 Then Return

                    ' Shift Row to end of List:
                    NewIndex = Me.dgvFileSortList.Rows.Count - 1
                End If
            End With
            Me.dgvFileSortList.Rows.Remove(curr_row)
            Me.dgvFileSortList.Rows.Insert(NewIndex, curr_row)
            Me.dgvFileSortList.CurrentCell = Me.dgvFileSortList(curr_col_index, NewIndex)
        Catch ex As Exception
            ' do nothing if error encountered while trying to move the row up or down
        End Try
    End Sub

    ''' <summary>
    ''' Resorts the Spectroscopy-Table-List
    ''' </summary>
    Private Sub ApplySorting() Handles btnApplySorting.Click
        Dim SortedNames As New List(Of String)

        ' Extract all Names in the new sorting:
        For i As Integer = 0 To Me.dgvFileSortList.RowCount - 1 Step 1
            SortedNames.Add(Convert.ToString(Me.dgvFileSortList.Rows(i).Cells(Me.colName.Index).Value))
        Next

        Dim ResortedList As New List(Of cSpectroscopyTable)
        ' Resort the Spectroscopy-Table List by a temporary List:
        For i As Integer = 0 To SortedNames.Count - 1 Step 1
            For Each oSpectroscopyTalbe As cSpectroscopyTable In Me.lSpectroscopyTables
                If oSpectroscopyTalbe.FileNameWithoutPath = SortedNames(i) Then ResortedList.Add(oSpectroscopyTalbe)
            Next
        Next

        Me.lSpectroscopyTables = ResortedList
        Me.oLineScanPlot = New cLineScanPlot(Me.lSpectroscopyTables)

        Me.RecalculateImage()
    End Sub

    ''' <summary>
    ''' SelectedColumns Changed
    ''' </summary>
    Public Sub SelectedColumnsChanged() Handles lbCommonColumnsX.SelectedIndexChanged, lbCommonColumnsZ.SelectedIndexChanged
        If Not Me.bReady Then Return
        If Me.lSpectroscopyTables Is Nothing Then Return
        If Me.lbCommonColumnsZ.SelectedItem Is Nothing Then Return
        If Me.lbCommonColumnsX.SelectedItem Is Nothing Then Return

        Me.vsValueRangeSelector.SetValueArray(Me.GetTotalValueArray)

        ' Save ColumnNames for next time:
        My.Settings.LastLineScan_ColumnX = Convert.ToString(Me.lbCommonColumnsX.SelectedItem)
        My.Settings.LastLineScan_ColumnZ = Convert.ToString(Me.lbCommonColumnsZ.SelectedItem)
        My.Settings.Save()

        ' Recalculates Values
        Me.RecalculateImage()
    End Sub
#End Region

#Region "Addon Functions (Saving, Exporting)"
    ''' <summary>
    ''' Saves the Image to the Clipboard
    ''' </summary>
    Private Sub cmnuCopyImageToClipboard_Click(sender As System.Object, e As System.EventArgs) Handles cmnuCopyImageToClipboard.Click
        If New cClipboardInvoker(Me.pbLineScan.Image).Invoke Then Return
    End Sub

    ''' <summary>
    ''' Save Image to File:
    ''' </summary>
    Private Sub cmnuSaveAsImage_Click(sender As System.Object, e As System.EventArgs) Handles cmnuSaveAsImage.Click
        Dim fs As New SaveFileDialog
        With fs
            .InitialDirectory = My.Settings.LastSelectedPath
            .Title = My.Resources.Title_SaveImage
            .Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp|Gif Image|*.gif|Tiff Image|*.tiff|Exif Image|*.exif"
            If New cDialogInvoker(fs).Invoke = DialogResult.OK Then
                Select Case .FilterIndex
                    Case 1
                        Me.pbLineScan.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)
                    Case 2
                        Me.pbLineScan.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Png)
                    Case 3
                        Me.pbLineScan.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Bmp)
                    Case 4
                        Me.pbLineScan.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Gif)
                    Case 5
                        Me.pbLineScan.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Tiff)
                    Case 6
                        Me.pbLineScan.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Exif)
                End Select
            Else
                ' If no FileName is given:
                Return
            End If
        End With
    End Sub

    Private Delegate Sub _SaveAsWSXM()
    ''' <summary>
    ''' Save Image as WSXM file
    ''' </summary>
    Private Sub SaveAsWSXM()
        If Me.InvokeRequired Then
            Dim d As New _SaveAsWSXM(AddressOf Me.SaveAsWSXM)
            Me.Invoke(d)
        Else
            If Me.lSpectroscopyTables Is Nothing Then Return
            If Me.lbCommonColumnsZ.SelectedItem Is Nothing Then Return
            If Me.lbCommonColumnsX.SelectedItem Is Nothing Then Return

            ' Get FileName to save to:
            Dim FileName As String = ""
            Dim fs As New SaveFileDialog
            With fs
                .FileName = My.Settings.ExportTemplate_FileNameLineScan & Me.lSpectroscopyTables(0).FileNameWithoutPath & "-" & Me.lSpectroscopyTables(Me.lSpectroscopyTables.Count - 1).FileNameWithoutPath
                .InitialDirectory = My.Settings.LastExport_Path
                .Title = My.Resources.Title_ExportToWSxM
                .Filter = "WsXM Binary File|*.stp"
                If New cDialogInvoker(fs).Invoke = DialogResult.OK Then
                    FileName = .FileName
                Else
                    ' If no FileName is given:
                    Return
                End If
            End With

            ' Ask the User how often a row should be repeated.
            Dim RepetitionCounterUserInput As String = ""
            RepetitionCounterUserInput = InputBox(My.Resources.InputBox_WSxM_NumberOfRepetitions,
                                                  My.Resources.InputBox_Title_WSxM_NumberOfRepetitions,
                                                  Me.oLineScanPlot.CurrentNumberOfRowRepetitions.ToString("N0"))
            If IsNumeric(RepetitionCounterUserInput) Then
                Dim RepetitionCounter As Integer = Convert.ToInt32(Convert.ToDecimal(RepetitionCounterUserInput))

                ' Export the Image:
                Me.oLineScanPlot.ExportToWSXM(FileName,
                                              Convert.ToString(Me.lbCommonColumnsX.SelectedItem),
                                              Convert.ToString(Me.lbCommonColumnsZ.SelectedItem),
                                              RepetitionCounter)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Save Image as WSXM file
    ''' </summary>
    Private Sub cmnuSaveAsWSXM_Click(sender As System.Object, e As System.EventArgs) Handles cmnuSaveAsWSXM.Click
        Me.SaveAsWSXM()
    End Sub
#End Region

End Class
