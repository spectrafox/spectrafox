Public Class mSpectroscopyTableDataTable

    ''' <summary>
    ''' Current spectroscopy-table object to use.
    ''' </summary>
    Private SpectroscopyTable As cSpectroscopyTable

    ''' <summary>
    ''' Reference to the data list.
    ''' </summary>
    Private ListOfColumns As List(Of ReadOnlyCollection(Of Double))

    ''' <summary>
    ''' Sets the displayed SpectroscopyTable
    ''' </summary>
    Public Sub SetSpectroscopyTable(ByRef InputSpectroscopyTable As cSpectroscopyTable)
        Me.SpectroscopyTable = InputSpectroscopyTable
        Me.ListOfColumns = Me.SpectroscopyTable.GetColumnValueList.Values.ToList
        Me.Redraw()
    End Sub

    ''' <summary>
    ''' Refills the DataGridView
    ''' </summary>
    Public Sub Redraw()

        ' Reset DataGridView
        Me.dgvSpectroscopyTable.RowCount = 0
        Me.dgvSpectroscopyTable.Columns.Clear()

        ' Set columns
        For Each DataColumn As cSpectroscopyTable.DataColumn In Me.SpectroscopyTable.GetColumnList.Values
            Dim DGVCol As New DataGridViewTextBoxColumn
            DGVCol.HeaderText = DataColumn.AxisTitle
            DGVCol.ValueType = Type.GetType("Double")
            DGVCol.DefaultCellStyle.Format = "0.####E-000"
            Me.dgvSpectroscopyTable.Columns.Add(DGVCol)
        Next

        ' Set the row-count to request the data-display.
        Me.dgvSpectroscopyTable.RowCount = Me.SpectroscopyTable.MeasurementPoints

    End Sub

    ''' <summary>
    ''' Processes the request for a cell value.
    ''' </summary>
    Private Sub dgvSpectroscopyTable_CellValueNeeded(sender As Object, e As DataGridViewCellValueEventArgs) Handles dgvSpectroscopyTable.CellValueNeeded
        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Return
        If Me.SpectroscopyTable Is Nothing Then Return

        e.Value = Me.ListOfColumns(e.ColumnIndex)(e.RowIndex)
    End Sub


#Region "Context Menu"

    ''' <summary>
    ''' Activate or deactivate the copy button.
    ''' </summary>
    Private Sub cmMenu_Show(sender As Object, e As EventArgs) Handles cmMenu.Opening
        If Me.dgvSpectroscopyTable.SelectedCells.Count = 0 Then
            Me.mnuCopyToClipboard.Enabled = False
        Else
            Me.mnuCopyToClipboard.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Copy the selected cells to the clipboard.
    ''' </summary>
    Private Sub mnuCopyToClipboard_Click(sender As Object, e As EventArgs) Handles mnuCopyToClipboard.Click
        Clipboard.SetDataObject(Me.dgvSpectroscopyTable.GetClipboardContent)
    End Sub

#End Region

End Class
