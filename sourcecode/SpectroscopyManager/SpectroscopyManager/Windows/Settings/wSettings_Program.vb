Public Class wSettings_Program

#Region "Properties"

    ''' <summary>
    ''' List of external viewers.
    ''' </summary>
    Private lExternalViewers As New cExternalViewers

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    Private Sub wSettings_Program_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Show the external viewers in the list.
        For Each V As cExternalViewers.ExternalViewer In Me.lExternalViewers.ExternalViewers
            With Me.dgvExternalViewers.Rows
                Dim i As Integer = .Add()
                With .Item(i)
                    .Cells(Me.dgvExternalViewers_colDisplayName.Index).Value = V.DisplayName
                    .Cells(Me.dgvExternalViewers_colPath.Index).Value = V.Path
                    .Cells(Me.dgvExternalViewers_colArguments.Index).Value = V.Arguments
                End With
            End With
        Next

    End Sub

#End Region

#Region "External viewers"

    ''' <summary>
    ''' Store the settings for the external viewers.
    ''' </summary>
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        ' Store the viewers.
        Dim Viewers As New List(Of cExternalViewers.ExternalViewer)

        ' Ignore the last column.
        For i As Integer = 0 To Me.dgvExternalViewers.RowCount - 2 Step 1
            Dim V As New cExternalViewers.ExternalViewer
            With Me.dgvExternalViewers.Rows(i)
                V.DisplayName = .Cells(Me.dgvExternalViewers_colDisplayName.Index).Value.ToString
                V.Path = .Cells(Me.dgvExternalViewers_colPath.Index).Value.ToString
                V.Arguments = .Cells(Me.dgvExternalViewers_colArguments.Index).Value.ToString
            End With
            Viewers.Add(V)
        Next

        ' Store the list.
        Me.lExternalViewers.SaveList(Viewers)

    End Sub

    ''' <summary>
    ''' Open window to select a software.
    ''' </summary>
    Private Sub dgvViewers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExternalViewers.CellClick

        If e.ColumnIndex = Me.dgvExternalViewers_colBrowse.Index Then

            Dim B As New OpenFileDialog
            With B
                B.Title = My.Resources.rExport.ExternalViewer_BrowseTitle
                B.Filter = "executables (*.exe)|*.exe"
                B.CheckFileExists = True
                If .ShowDialog = DialogResult.OK Then
                    Me.dgvExternalViewers.Rows(e.RowIndex).Cells(Me.dgvExternalViewers_colPath.Index).Value = B.FileName
                End If
            End With

        End If

    End Sub


#End Region
End Class