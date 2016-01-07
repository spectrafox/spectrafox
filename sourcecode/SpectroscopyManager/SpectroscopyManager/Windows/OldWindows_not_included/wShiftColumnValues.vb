Imports ZedGraph

Public Class wShiftColumnValues
    Inherits wFormBase
    Implements iMultipleSpectroscopyTablesLoaded

    Private bReady As Boolean = False

    Private SpectroscopyTableList As New Dictionary(Of String, cSpectroscopyTable)
    Private ShiftedColumnsX As New Dictionary(Of String, cSpectroscopyTable.DataColumn)
    Private ShiftedColumnsY As New Dictionary(Of String, cSpectroscopyTable.DataColumn)

    Private iSelectedRowIndex As Integer = 0

    ''' <summary>
    ''' Current list of SpectroscopyTables
    ''' </summary>
    Public ReadOnly Property CurrentListOfSpectroscopyTables As List(Of cSpectroscopyTable)
        Get
            Return Me.SpectroscopyTableList.Values.ToList
        End Get
    End Property

#Region "Interface Functions"

    Private Delegate Sub _ShowDialog()
    Public Sub AllSpectroscopyTablesLoaded() Implements iMultipleSpectroscopyTablesLoaded.AllSpectroscopyTablesLoaded
        Dim SpectroscopyTableList As List(Of cSpectroscopyTable) = Me.SpectroscopyTableList.Values.ToList
        Me.SetSpectroscopyTables(SpectroscopyTableList)

        If zPreview.InvokeRequired Then
            zPreview.Invoke(New _ShowDialog(AddressOf Me.ShowDialog))
            'Me.ShowDialog()
        End If
    End Sub

    Public Sub OneOfAllSpectroscopyTablesFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iMultipleSpectroscopyTablesLoaded.OneOfAllSpectroscopyTablesFetched
        ' Add to Dictionary
        Me.SpectroscopyTableList.Add(SpectroscopyTable.FullFileName, SpectroscopyTable)
    End Sub

    Public Property TotalNumberOfFilesToFetch As Integer Implements iMultipleSpectroscopyTablesLoaded.TotalNumberOfFilesToFetch
#End Region

    ''' <summary>
    ''' Takes the inital list of Spectroscopy-Files:
    ''' </summary>
    Public Sub SetSpectroscopyTables(ByVal SpectroscopyTableList As List(Of cSpectroscopyTable),
                                     Optional ByVal SelectedColumnNameX As String = "",
                                     Optional ByVal SelectedColumnNameY As String = "")

        Me.SpectroscopyTableList.Clear()

        ' Check for empty list
        If SpectroscopyTableList.Count <= 0 Then Return

        ' Write File-List
        With Me.dgvSpectroscopyFiles
            .Rows.Clear()

            For Each oSpectroscopyTable As cSpectroscopyTable In SpectroscopyTableList

                ' Add New Spectroscopy-Table-Row:
                Dim i As Integer = .Rows.Add()
                With .Rows(i)
                    .Cells(Me.colShow.Index).Value = True
                    .Cells(Me.colName.Index).Value = oSpectroscopyTable.FullFileName
                    .Cells(Me.colShiftX.Index).Value = "0"
                    .Cells(Me.colShiftY.Index).Value = "0"
                    .Cells(Me.colSaveX.Index).Value = My.Resources.Word_SaveColumn
                    .Cells(Me.colSaveY.Index).Value = My.Resources.Word_SaveColumn

                    ' Fill ColumnNames
                    With DirectCast(.Cells(Me.colX.Index), DataGridViewComboBoxCell)
                        .Items.Clear()
                        For Each DataCol As cSpectroscopyTable.DataColumn In oSpectroscopyTable.GetColumnList
                            .Items.Add(DataCol.Name)
                            If SelectedColumnNameX = DataCol.Name Then
                                .Value = DataCol.Name
                            End If
                        Next
                    End With
                    With DirectCast(.Cells(Me.colY.Index), DataGridViewComboBoxCell)
                        .Items.Clear()
                        For Each DataCol As cSpectroscopyTable.DataColumn In oSpectroscopyTable.GetColumnList
                            .Items.Add(DataCol.Name)
                            If SelectedColumnNameY = DataCol.Name Then
                                .Value = DataCol.Name
                            End If
                        Next
                    End With

                    .Cells(Me.colColumnNameX.Index).Value = My.Resources.Word_ShiftOf & Convert.ToString(.Cells(Me.colX.Index).Value)
                    .Cells(Me.colColumnNameY.Index).Value = My.Resources.Word_ShiftOf & Convert.ToString(.Cells(Me.colY.Index).Value)
                End With

                ' Add to Dictionary
                Me.SpectroscopyTableList.Add(oSpectroscopyTable.FullFileName, oSpectroscopyTable)
            Next

        End With

        ' Fill the Column-Selectors.
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Paints the Preview-Image with the selected Columns:
    ''' </summary>
    Private Sub PaintPreviewImage()
        Dim lPointPairLists As New Dictionary(Of ZedGraph.PointPairList, Boolean)

            ' Go through all rows an add a curve to the Paint-Area
            For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
                Dim SpecName As String = Convert.ToString(Row.Cells(Me.colName.Index).Value)

                ' Check, if Row should be displayed:
                If Not Convert.ToBoolean(DirectCast(Row.Cells(Me.colShow.Index), DataGridViewCheckBoxCell).Value) Then Continue For

                ' Check, if Spectroscopy-Table exists
                If Not Me.SpectroscopyTableList.ContainsKey(SpecName) Then Continue For
                Dim oSpectroscopyTable As cSpectroscopyTable = Me.SpectroscopyTableList(SpecName)

                ' Check, if ColumnNames exist
                Dim ColumnIDX As Integer = oSpectroscopyTable.GetColumnIndexByName(Convert.ToString(Row.Cells(Me.colX.Index).Value))
                Dim ColumnIDY As Integer = oSpectroscopyTable.GetColumnIndexByName(Convert.ToString(Row.Cells(Me.colY.Index).Value))
                If ColumnIDX = -1 Or ColumnIDY = -1 Then Continue For

                ' Check, if to use the Shifted Columns
                Dim PaintColumnX As cSpectroscopyTable.DataColumn
                Dim PaintColumnY As cSpectroscopyTable.DataColumn

                If ShiftedColumnsX.ContainsKey(SpecName) Then
                    PaintColumnX = ShiftedColumnsX(SpecName)
                Else
                    PaintColumnX = oSpectroscopyTable.Column(ColumnIDX)
                End If

                If ShiftedColumnsY.ContainsKey(SpecName) Then
                    PaintColumnY = ShiftedColumnsY(SpecName)
                Else
                    PaintColumnY = oSpectroscopyTable.Column(ColumnIDY)
                End If

                ' Add Points to list
                Dim list As New PointPairList
                For i As Integer = 0 To PaintColumnX.Values.Count - 1 Step 1
                    list.Add(PaintColumnX.Values(i), PaintColumnY.Values(i))
                Next

                ' highlight selected Row's graph
                If Row.Index = iSelectedRowIndex Then
                    lPointPairLists.Add(list, True)
                Else
                    lPointPairLists.Add(list, False)
                End If
            Next

            ' Paint Graph
            With Me.zPreview.GraphPane
                .Title.Text = ""
                .XAxis.Title.Text = ""
                If Me.ckbLogX.Checked Then
                    .XAxis.Type = AxisType.Log
                Else
                    .XAxis.Type = AxisType.Linear
                End If
                .YAxis.Title.Text = ""
                If Me.ckbLogY.Checked Then
                    .YAxis.Type = AxisType.Log
                Else
                    .YAxis.Type = AxisType.Linear
                End If
                .CurveList.Clear()
                For Each pl As KeyValuePair(Of PointPairList, Boolean) In lPointPairLists
                    Dim oLine As ZedGraph.LineItem = .AddCurve("", pl.Key, Color.Black, ZedGraph.SymbolType.Circle)
                    With oLine
                        ' Highlight:
                        If pl.Value Then
                            .Line.IsVisible = False
                            .Symbol.Fill = New ZedGraph.Fill(Color.Red)
                            .Symbol.Type = ZedGraph.SymbolType.Circle
                            .Symbol.Size = 4
                            '.Symbol.IsVisible = False
                        Else
                            .Line.IsVisible = True
                            .Symbol.Fill = New ZedGraph.Fill(Color.Black)
                            .Symbol.Type = ZedGraph.SymbolType.Circle
                            .Symbol.Size = 4
                            .Symbol.IsVisible = False
                        End If
                    End With
                Next
            End With
            Me.zPreview.Refresh()
            Me.zPreview.AxisChange()
    End Sub

    ''' <summary>
    ''' Ending the Edit-Mode
    ''' </summary>
    Private Sub dgvSpectroscopyFiles_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSpectroscopyFiles.CellEndEdit
        If Me.dgvSpectroscopyFiles.Rows.Count <= 0 Then Return

        With Me.dgvSpectroscopyFiles.Rows(e.RowIndex)
            Select Case e.ColumnIndex
                Case Me.colShiftX.Index
                    '.Cells(e.ColumnIndex).Value = .Cells(e.ColumnIndex).Value
            End Select
        End With
    End Sub

    ''' <summary>
    ''' On Clicking on a Cell, begin editing:
    ''' </summary>
    Private Sub dgvSpectroscopyFiles_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSpectroscopyFiles.CellClick
        If Me.dgvSpectroscopyFiles.Rows.Count <= 0 Then Return

        With Me.dgvSpectroscopyFiles.Rows(e.RowIndex)
            Dim SpecName As String = Convert.ToString(.Cells(Me.colName.Index).Value)

            Select Case e.ColumnIndex
                Case Me.colShiftX.Index
                    .Cells(e.ColumnIndex).Selected = True
                    Me.dgvSpectroscopyFiles.BeginEdit(False)
                Case Me.colShiftY.Index
                    .Cells(e.ColumnIndex).Selected = True
                    Me.dgvSpectroscopyFiles.BeginEdit(False)

                Case Me.colSaveX.Index
                    Dim oCol As cSpectroscopyTable.DataColumn
                    ' ausgewählte Spalte speichern, wenn sie verändert wurde:
                    If Me.ShiftedColumnsX.ContainsKey(SpecName) Then
                        oCol = Me.ShiftedColumnsX(SpecName)
                    Else
                        ' Check for ColumnID
                        Dim ColumnIDX As Integer = Me.SpectroscopyTableList(SpecName).GetColumnIndexByName(Convert.ToString(.Cells(Me.colX.Index).Value))
                        If ColumnIDX = -1 Then Return
                        oCol = Me.SpectroscopyTableList(SpecName).CopyColumn(ColumnIDX)
                    End If

                    ' Extract new Name
                    Dim ColName As String = Convert.ToString(.Cells(Me.colColumnNameX.Index).Value)
                    oCol.Name = ColName

                    Me.SpectroscopyTableList(SpecName).BaseFileObject.AddSpectroscopyColumn(oCol)
                    'MessageBox.Show(My.Resources.Message_SavedSingleColumn, My.Resources.Title_Success, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Case Me.colSaveY.Index
                    Dim oCol As cSpectroscopyTable.DataColumn
                    ' ausgewählte Spalte speichern, wenn sie verändert wurde:
                    If Me.ShiftedColumnsY.ContainsKey(SpecName) Then
                        oCol = Me.ShiftedColumnsY(SpecName)
                    Else
                        ' Check for ColumnID
                        Dim ColumnIDY As Integer = Me.SpectroscopyTableList(SpecName).GetColumnIndexByName(Convert.ToString(.Cells(Me.colY.Index).Value))
                        If ColumnIDY = -1 Then Return
                        oCol = Me.SpectroscopyTableList(SpecName).CopyColumn(ColumnIDY)
                    End If

                    ' Extract new Name
                    Dim ColName As String = Convert.ToString(.Cells(Me.colColumnNameY.Index).Value)
                    oCol.Name = ColName

                    Me.SpectroscopyTableList(SpecName).BaseFileObject.AddSpectroscopyColumn(oCol)
                    'MessageBox.Show(My.Resources.Message_SavedSingleColumn, My.Resources.Title_Success, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Select
        End With
    End Sub

    ''' <summary>
    ''' Selected Columns Change
    ''' </summary>
    Private Sub dgvSpectroscopyFiles_CellValueChanged(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dgvSpectroscopyFiles.CellValueChanged

        If Me.dgvSpectroscopyFiles.Rows.Count <= 0 Then Return

        With Me.dgvSpectroscopyFiles.Rows(e.RowIndex)
            Dim SpecName As String = Convert.ToString(.Cells(Me.colName.Index).Value)

            ' Check, if Spectroscopy-Table exists
            If Not Me.SpectroscopyTableList.ContainsKey(SpecName) Then Return
            Dim oSpectroscopyTable As cSpectroscopyTable = Me.SpectroscopyTableList(SpecName)

            Select Case e.ColumnIndex
                Case Me.colX.Index
                    .Cells(Me.colColumnNameX.Index).Value = My.Resources.Word_ShiftOf & Convert.ToString(.Cells(Me.colX.Index).Value)
                    Me.PaintPreviewImage()
                Case Me.colY.Index
                    .Cells(Me.colColumnNameY.Index).Value = My.Resources.Word_ShiftOf & Convert.ToString(.Cells(Me.colY.Index).Value)
                    Me.PaintPreviewImage()
                Case Me.colShow.Index
                    Me.PaintPreviewImage()
                Case Me.colShiftX.Index
                    ' Check, if Value is Numeric
                    If Not IsNumeric(.Cells(Me.colShiftX.Index).Value) Then .Cells(Me.colShiftX.Index).Value = "0"
                    Dim ShiftValue As Double = Convert.ToDouble(.Cells(Me.colShiftX.Index).Value)

                    ' Check for ColumnID
                    Dim ColumnIDX As Integer = oSpectroscopyTable.GetColumnIndexByName(Convert.ToString(.Cells(Me.colX.Index).Value))
                    If ColumnIDX = -1 Then Return

                    ' Shift Data
                    Dim oShiftedCol As cSpectroscopyTable.DataColumn = oSpectroscopyTable.CopyColumn(ColumnIDX)
                    oShiftedCol.ShiftValuesByFixedValue(ShiftValue)

                    If Me.ShiftedColumnsX.ContainsKey(SpecName) Then
                        Me.ShiftedColumnsX(SpecName) = oShiftedCol
                    Else
                        Me.ShiftedColumnsX.Add(SpecName, oShiftedCol)
                    End If

                    ' Repaint
                    Me.PaintPreviewImage()
                Case Me.colShiftY.Index
                    ' Check, if Value is Numeric
                    If Not IsNumeric(.Cells(Me.colShiftY.Index).Value) Then .Cells(Me.colShiftY.Index).Value = "0"
                    Dim ShiftValue As Double = Convert.ToDouble(.Cells(Me.colShiftY.Index).Value)

                    ' Check for ColumnID
                    Dim ColumnIDY As Integer = oSpectroscopyTable.GetColumnIndexByName(Convert.ToString(.Cells(Me.colY.Index).Value))
                    If ColumnIDY = -1 Then Return

                    ' Shift Data
                    Dim oShiftedCol As cSpectroscopyTable.DataColumn = oSpectroscopyTable.CopyColumn(ColumnIDY)
                    oShiftedCol.ShiftValuesByFixedValue(ShiftValue)

                    If Me.ShiftedColumnsY.ContainsKey(SpecName) Then
                        Me.ShiftedColumnsY(SpecName) = oShiftedCol
                    Else
                        Me.ShiftedColumnsY.Add(SpecName, oShiftedCol)
                    End If

                    ' Repaint
                    Me.PaintPreviewImage()
            End Select
        End With
    End Sub

    ''' <summary>
    ''' Repaint, if selected Row changes
    ''' </summary>
    Private Sub dgvSpectroscopyFiles_RepaintIfRowChanges(sender As System.Object, e As System.EventArgs) Handles dgvSpectroscopyFiles.SelectionChanged
        If Me.dgvSpectroscopyFiles.SelectedCells.Count <> 1 Then Return

        ' Check, if Row Changed:
        If Me.dgvSpectroscopyFiles.SelectedCells(0).RowIndex <> iSelectedRowIndex Then
            iSelectedRowIndex = Me.dgvSpectroscopyFiles.SelectedCells(0).RowIndex
            Me.PaintPreviewImage()
        End If
    End Sub

    ''' <summary>
    ''' Change to Logarithmic Scale
    ''' </summary>
    Private Sub ckbLogY_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ckbLogY.CheckedChanged, ckbLogX.CheckedChanged
        Me.PaintPreviewImage()
    End Sub

    ''' <summary>
    ''' Shows all Spectroscopy-Tables
    ''' </summary>
    Private Sub btnShowAll_Click(sender As System.Object, e As System.EventArgs) Handles btnShowAll.Click
        For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
            Row.Cells(Me.colShow.Index).Value = True
        Next
    End Sub

    ''' <summary>
    ''' Shifts all Columns linear by the given Amount
    ''' </summary>
    Private Sub btnShiftAllLinear_Click(sender As System.Object, e As System.EventArgs) Handles btnShiftAllYLinear.Click, btnShiftAllXLinear.Click
        If sender Is Me.btnShiftAllXLinear Then
            ' Check for correct Double-Value
            If Not IsNumeric(Me.txtShiftAllXLinear.Text) Then Return
            Dim ShiftValueX As Double = Convert.ToDouble(Me.txtShiftAllXLinear.Text)

            For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
                With Row.Cells(Me.colShiftX.Index)
                    .Value = Convert.ToDouble(.Value) + ShiftValueX
                End With
            Next
        End If
        If sender Is Me.btnShiftAllYLinear Then
            ' Check for correct Double-Value
            If Not IsNumeric(Me.txtShiftAllYLinear.Text) Then Return
            Dim ShiftValueY As Double = Convert.ToDouble(Me.txtShiftAllYLinear.Text)

            For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
                With Row.Cells(Me.colShiftY.Index)
                    .Value = Convert.ToDouble(.Value) + ShiftValueY
                End With
            Next
        End If
    End Sub

    ''' <summary>
    ''' Sets all Shifts to 0
    ''' by deleting the shifted Columns
    ''' </summary>
    Private Sub btnResetShifts_Click(sender As System.Object, e As System.EventArgs) Handles btnResetShiftsY.Click, btnResetShiftsX.Click
        If sender Is Me.btnResetShiftsX Then
            For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
                Row.Cells(Me.colShiftX.Index).Value = "0"
            Next
            'Me.ShiftedColumnsX.Clear()
        End If
        If sender Is Me.btnResetShiftsY Then
            For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
                Row.Cells(Me.colShiftY.Index).Value = "0"
            Next
            'Me.ShiftedColumnsY.Clear()
        End If
    End Sub

    ''' <summary>
    ''' Hides all Spectroscopy-Tables
    ''' </summary>
    Private Sub btnHideAll_Click(sender As System.Object, e As System.EventArgs) Handles btnHideAll.Click
        For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
            Row.Cells(Me.colShow.Index).Value = False
        Next
    End Sub

#Region "Column Saving"
    ''' <summary>
    ''' Saves the Shifted Columns back to their Base-File-Objects
    ''' </summary>
    Private Sub btnSaveColumns_Click(sender As System.Object, e As System.EventArgs) _
        Handles btnSaveXColumns.Click, btnSaveYColumns.Click, btnSaveAllColumns.Click
        Try
            ' save all shifted X-Columns.
            If sender Is Me.btnSaveXColumns Or sender Is Me.btnSaveAllColumns Then
                For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
                    Dim SpecName As String = Convert.ToString(Row.Cells(Me.colName.Index).Value)
                    Dim oCol As cSpectroscopyTable.DataColumn

                    ' ausgewählte Spalte speichern, wenn sie verändert wurde:
                    If Me.ShiftedColumnsX.ContainsKey(SpecName) Then
                        oCol = Me.ShiftedColumnsX(SpecName)
                    Else
                        ' Check for ColumnID
                        Dim ColumnIDX As Integer = Me.SpectroscopyTableList(SpecName).GetColumnIndexByName(Convert.ToString(Row.Cells(Me.colX.Index).Value))
                        If ColumnIDX = -1 Then Return
                        oCol = Me.SpectroscopyTableList(SpecName).CopyColumn(ColumnIDX)
                    End If

                    ' Extract new Name
                    Dim ColName As String = Convert.ToString(Row.Cells(Me.colColumnNameX.Index).Value)
                    oCol.Name = ColName

                    Me.SpectroscopyTableList(SpecName).BaseFileObject.AddSpectroscopyColumn(oCol)
                Next
            End If

            ' save all shifted Y-Columns.
            If sender Is Me.btnSaveYColumns Or sender Is Me.btnSaveAllColumns Then
                For Each Row As DataGridViewRow In Me.dgvSpectroscopyFiles.Rows
                    Dim SpecName As String = Convert.ToString(Row.Cells(Me.colName.Index).Value)
                    Dim oCol As cSpectroscopyTable.DataColumn

                    ' ausgewählte Spalte speichern, wenn sie verändert wurde:
                    If Me.ShiftedColumnsY.ContainsKey(SpecName) Then
                        oCol = Me.ShiftedColumnsY(SpecName)
                    Else
                        ' Check for ColumnID
                        Dim ColumnIDY As Integer = Me.SpectroscopyTableList(SpecName).GetColumnIndexByName(Convert.ToString(Row.Cells(Me.colY.Index).Value))
                        If ColumnIDY = -1 Then Return
                        oCol = Me.SpectroscopyTableList(SpecName).CopyColumn(ColumnIDY)
                    End If

                    ' Extract new Name
                    Dim ColName As String = Convert.ToString(Row.Cells(Me.colColumnNameY.Index).Value)
                    oCol.Name = ColName

                    Me.SpectroscopyTableList(SpecName).BaseFileObject.AddSpectroscopyColumn(oCol)
                Next
            End If
            MessageBox.Show(My.Resources.Message_SavedAllColumn,
                            My.Resources.Title_Success,
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error saving Columns: " & vbCrLf & ex.Message)
        End Try
    End Sub
#End Region

#Region "Window Closing"
    ''' <summary>
    ''' Closes the Window
    ''' </summary>
    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
#End Region

End Class