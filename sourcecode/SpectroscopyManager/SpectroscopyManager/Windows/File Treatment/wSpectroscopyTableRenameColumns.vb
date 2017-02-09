Public Class wSpectroscopyTableRenameColumns
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

    ''' <summary>
    ''' Renaming Tool
    ''' </summary>
    Protected ColumnRenamer As cSpectroscopyTableColumnRenamer

#Region "Form Constructor"
    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set Window Title
        Me.Text &= MyBase._FileObject.FileName

    End Sub
#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the Spectroscopy-File to the display.
    ''' </summary>
    Public Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Set the column-names of the file into the selection combobox
        With Me.colSourceColumnSelector.Items
            .Clear()

            For Each Col As cSpectroscopyTable.DataColumn In SpectroscopyTable.GetColumnList.Values
                If Col.IsSpectraFoxGenerated Then .Add(Col.Name)
            Next
        End With

        ' Load Properties from Settings if possible!
        Dim RenameRules As List(Of cSpectroscopyTableColumnRenamer.ReplaceRule) = cSpectroscopyTableColumnRenamer.GetRenameRulesFromSettings

        ' Write the rules
        Me.dgvColumnNames.Rows.Clear()
        For Each RenameRule As cSpectroscopyTableColumnRenamer.ReplaceRule In RenameRules
            Dim i As Integer = Me.dgvColumnNames.Rows.Add
            With Me.dgvColumnNames.Rows(i)
                .Cells(Me.colSourceName.Index).Value = RenameRule.SearchFor
                .Cells(Me.colDeleteColumn.Index).Value = RenameRule.DeleteInsteadOfReplace
                .Cells(Me.colTargetColumnName.Index).Value = RenameRule.ReplaceBy

                If RenameRule.DeleteInsteadOfReplace Then
                    .Cells(Me.colTargetColumnName.Index).ReadOnly = True
                    .Cells(Me.colTargetColumnName.Index).Style.BackColor = Color.DarkGray
                Else
                    .Cells(Me.colTargetColumnName.Index).ReadOnly = False
                    .Cells(Me.colTargetColumnName.Index).Style.BackColor = Color.White
                End If

            End With
        Next

        ColumnRenamer = New cSpectroscopyTableColumnRenamer(Me._FileObject)
        ColumnRenamer.CurrentSpectroscopyTable = Me.SpectroscopyTable

    End Sub

#End Region

#Region "Column selection"

    Private ColumnNameSelectorSelectedIndexHandler As New EventHandler(AddressOf ColumnNamesSelector_EditingIndexChanged)
    Private ColumnNameSelectorRemoveHandler As New EventHandler(AddressOf ColumnNamesSelectorChangedComboboxLeave)

    ''' <summary>
    ''' Write the name of the selected source-column to the textbox-column.
    ''' </summary>
    Private Sub ColumnNamesSelectorChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvColumnNames.CellEndEdit
        If e.ColumnIndex = colSourceColumnSelector.Index Then
            Me.dgvColumnNames.Rows(e.RowIndex).Cells(colSourceName.Index).Value = Me.dgvColumnNames.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        End If
    End Sub

    ''' <summary>
    ''' Set selected index changed handler.
    ''' </summary>
    Public Sub ColumnNamesSelectorChangedComboboxShowing(Sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvColumnNames.EditingControlShowing
        Dim cb As ComboBox = TryCast(e.Control, ComboBox)
        If cb IsNot Nothing Then
            Dim editingComboBox As ComboBox = DirectCast(e.Control, ComboBox)
            ' Remove an existing event-handler, if present, to avoid 
            ' adding multiple handlers when the editing control is reused.
            RemoveHandler editingComboBox.SelectedIndexChanged, ColumnNameSelectorSelectedIndexHandler
            RemoveHandler editingComboBox.Leave, ColumnNameSelectorRemoveHandler
            ' Add the event handler. 
            AddHandler editingComboBox.SelectedIndexChanged, ColumnNameSelectorSelectedIndexHandler
            AddHandler editingComboBox.Leave, ColumnNameSelectorRemoveHandler

        End If
    End Sub

    ''' <summary>
    ''' Selected Index of ComboboxColumn changed.
    ''' </summary>
    Public Sub ColumnNamesSelector_EditingIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dgvc As DataGridViewCell = TryCast(Me.dgvColumnNames.CurrentCell, DataGridViewCell)

        Dim comboBox1 As ComboBox = CType(sender, ComboBox)
        Dim cb As ComboBox = TryCast(comboBox1, ComboBox)
        If cb IsNot Nothing Then
            If dgvc IsNot Nothing Then
                If comboBox1.SelectedItem IsNot Nothing Then
                    Me.dgvColumnNames.Rows(dgvc.RowIndex).Cells(Me.colSourceName.Index).Value = comboBox1.SelectedItem.ToString
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Set selected index changed handler.
    ''' </summary>
    Public Sub ColumnNamesSelectorChangedComboboxLeave(Sender As Object, e As EventArgs)
        If TypeOf (Sender) Is ComboBox Then
            Dim cboThisCombobox = DirectCast(Sender, ComboBox)

            RemoveHandler cboThisCombobox.SelectedValueChanged, ColumnNameSelectorSelectedIndexHandler
            RemoveHandler cboThisCombobox.Leave, ColumnNameSelectorRemoveHandler
        End If
    End Sub

    ''' <summary>
    ''' Cell-Content Click
    ''' </summary>
    Private Sub dgvColumnNames_OnCellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvColumnNames.CellMouseUp

        ' just react on existing rows
        If e.RowIndex < 0 Then Return
        If Me.dgvColumnNames.Rows(e.RowIndex).IsNewRow Then Return

        ' delete, when clicking on the delete column
        If e.ColumnIndex = Me.colDelete.Index Then
            Me.dgvColumnNames.Rows.RemoveAt(e.RowIndex)
        ElseIf e.ColumnIndex = Me.colDeleteColumn.Index Then

            ' Commit the current status
            Me.dgvColumnNames.CommitEdit(DataGridViewDataErrorContexts.Commit)
            Me.dgvColumnNames.EndEdit()

        End If

    End Sub

    ''' <summary>
    ''' CellValueChanged event
    ''' </summary>
    Private Sub dgvColumnNames_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvColumnNames.CellValueChanged

        ' just react on existing rows
        If e.RowIndex < 0 Then Return
        If Me.dgvColumnNames.Rows(e.RowIndex).IsNewRow Then Return

        If e.ColumnIndex = Me.colDeleteColumn.Index Then

            ' Checkbox status changed
            Dim CurrentStatus As Boolean = Convert.ToBoolean(Me.dgvColumnNames.Rows(e.RowIndex).Cells(Me.colDeleteColumn.Index).Value)
            If CurrentStatus Then
                With Me.dgvColumnNames.Rows(e.RowIndex).Cells(Me.colTargetColumnName.Index)
                    .ReadOnly = True
                    .Style.BackColor = Color.DarkGray
                End With
            Else
                With Me.dgvColumnNames.Rows(e.RowIndex).Cells(Me.colTargetColumnName.Index)
                    .ReadOnly = False
                    .Style.BackColor = Color.White
                End With
            End If

        End If

    End Sub



#End Region

#Region "Renaming"

    ''' <summary>
    ''' Renaming of columns.
    ''' </summary>
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        ' Get RenameRules from the DGV
        Dim RenameRules As New List(Of cSpectroscopyTableColumnRenamer.ReplaceRule)

        Dim OldName As String = ""
        Dim NewName As String = ""
        Dim DeleteInsteadOfRenaming As Boolean = False
        For Each Row As DataGridViewRow In Me.dgvColumnNames.Rows
            If Row.IsNewRow Then Continue For
            OldName = Convert.ToString(Row.Cells(Me.colSourceName.Index).Value)
            NewName = Convert.ToString(Row.Cells(Me.colTargetColumnName.Index).Value)
            DeleteInsteadOfRenaming = Convert.ToBoolean(Row.Cells(Me.colDeleteColumn.Index).Value)

            RenameRules.Add(New cSpectroscopyTableColumnRenamer.ReplaceRule(OldName, NewName, DeleteInsteadOfRenaming))
        Next

        ' Apply the rules
        ColumnRenamer.RenameColumns(RenameRules)

        ' Store rules to settings!
        cSpectroscopyTableColumnRenamer.SaveRenameRulesToSettings(RenameRules)

        ' Close window!
        Me.Close()
    End Sub

#End Region

#Region "Window Closing"
    ''' <summary>
    ''' Close the window.
    ''' </summary>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

#End Region

End Class