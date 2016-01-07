Public Class wDataAveragingMultipleFiles
    Inherits wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad


    ''' <summary>
    ''' Sets the used SpectroscopyTables and enables the Interface.
    ''' </summary>
    Public Sub SetSpectroscopyTables() Handles Me.AllFilesFetched
        Me.Enabled = True

        Dim ListOfCommonColumnNames As List(Of String) = cSpectroscopyTable.GetCommonColumns(Me.CurrentSpectroscopyTableList)

        ' Write all Column-Names into the Combobox, that exist in each Spectroscopy-Table.
        For Each ColName As String In ListOfCommonColumnNames
            Me.lbCommonColumns.Items.Add(ColName)
        Next

        ' Change Label to the Number of Files:
        Me.lblHeading.Text = Me.lblHeading.Text.Replace("%%", Me.CurrentSpectroscopyTableList.Count.ToString("N0"))
    End Sub

    ''' <summary>
    ''' Set the Name of the selected Averaging Column to the New Column-Name
    ''' </summary>
    Private Sub SelectedAveragingColumnChanged() Handles lbCommonColumns.SelectedIndexChanged
        If Me.lbCommonColumns.SelectedItems.Count <= 0 Then Return

        ' Set ColumnTitle to Smoothed Selected ColumnTitle
        Me.txtNewColumnName.Text = My.Resources.ColumnTemplate_AveragedMultiple.Replace("%c%", Me.lbCommonColumns.SelectedItem.ToString).Replace("%n%", Me.CurrentSpectroscopyTableList.Count.ToString("N0"))

        ' Activate Button to Average
        Me.btnApplyAveraging.Enabled = True
    End Sub

    ''' <summary>
    ''' Performs the Averaging and saves the Averaged Columns back to the Original Spectroscopy-Tables.
    ''' </summary>
    Private Sub btnApplyAveraging_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyAveraging.Click
        If Me.lbCommonColumns.SelectedItems.Count <= 0 Then Return
        If Me.lbCommonColumns.SelectedItems.Count > 1 Then Return

        ' Load Columns to Average:
        Dim ColumnNameToAverage As String = Convert.ToString(Me.lbCommonColumns.SelectedItem)
        Dim lDataList As New List(Of ReadOnlyCollection(Of Double))
        For i As Integer = 0 To Me.CurrentSpectroscopyTableList.Count - 1 Step 1
            lDataList.Add(Me.CurrentSpectroscopyTableList(i).Column(ColumnNameToAverage).Values)
        Next

        Try
            ' Average Data
            Dim lAveragedData As List(Of Double) = cNumericalMethods.AverageDataColumnsFromMultipleFiles(lDataList)

            ' Save Data in a new DataColumn to the SpectroscopyTables:
            For i As Integer = 0 To Me.CurrentSpectroscopyTableList.Count - 1 Step 1
                Dim dc As New cSpectroscopyTable.DataColumn
                dc.Name = Me.txtNewColumnName.Text
                dc.UnitSymbol = Me.CurrentSpectroscopyTableList(i).Column(ColumnNameToAverage).UnitSymbol
                dc.UnitType = Me.CurrentSpectroscopyTableList(i).Column(ColumnNameToAverage).UnitType
                dc.SetValueList(lAveragedData)
                Me.CurrentSpectroscopyTableList(i).BaseFileObject.AddSpectroscopyColumn(dc)
            Next

            ' Show Message:
            MessageBox.Show(My.Resources.Message_AveragingFinished,
                            My.Resources.Title_Success, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#Region "Window Closing"
    ''' <summary>
    ''' Close the Window:
    ''' </summary>
    Private Sub btnCloseWindow_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseWindow.Click
        Me.Close()
    End Sub
#End Region

End Class