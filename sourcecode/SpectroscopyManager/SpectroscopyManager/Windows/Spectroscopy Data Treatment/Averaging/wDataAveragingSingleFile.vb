Public Class wDataAveragingSingleFile
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

    Private bReady As Boolean = False

#Region "Properties"
    ''' <summary>
    ''' Object for Data Averaging of the selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataAverager As cSpectroscopyTableDataAverager
#End Region

#Region "Form Contructor"
    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set Window Title
        Me.Text &= MyBase._FileObject.FileName

    End Sub
#End Region

    ''' <summary>
    ''' Function that gets called, when the Background-Class finished loading a spectroscopy file.
    ''' </summary>
    Private Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Create new DataAverager Object
        Me.DataAverager = New cSpectroscopyTableDataAverager(Me._FileObject)
        ' Set the loaded SpectroscopyTable to the averager
        Me.DataAverager.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set Preview-Images:
        Me.pbAfterAveraging.SetSinglePreviewImage(SpectroscopyTable)
        Me.lbSourceColumnSelector.InitializeColumns(SpectroscopyTable.GetColumnList)

        ' Load properties from settings if possible!
        With My.Settings
            Dim LastSelectedColumns As New List(Of String)
            For Each S As String In .LastDataAveragingSingleFile_ColumnNames
                LastSelectedColumns.Add(S)
            Next
            Me.lbSourceColumnSelector.SelectedColumnNames = LastSelectedColumns
            Me.pbAfterAveraging.cbX.SelectedColumnName = .LastDataAveragingSingleFile_ColumnX
            Me.txtNewColumnName.Text = .LastDataAveragingSingleFile_NewColumnName
        End With

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Applies the averaging of the selected columns.
    ''' </summary>
    Public Sub btnApplyAveraging_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyAveraging.Click
        If Not Me.bReady Then Return

        Me.ApplyProcedure()
    End Sub

    ''' <summary>
    ''' Applies the procedure.
    ''' </summary>
    Public Sub ApplyProcedure()
        Dim SelectedColumnNames As List(Of String) = Me.lbSourceColumnSelector.SelectedColumnNames

        ' Security-Checks, if columns are selected.
        If SelectedColumnNames.Count <= 0 Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        If Me.txtNewColumnName.Text = String.Empty Then Me.txtNewColumnName.Text = "averaged data"

        ' Average the columns in the background
        Me.DataAverager.AverageColumnsWithoutFetching_ASync(SelectedColumnNames, Me.txtNewColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the averaging procedure is complete
    ''' </summary>
    Private Sub AveragingFinished(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef AveragedColumn As cSpectroscopyTable.DataColumn) Handles DataAverager.FileAveragingComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataAverager.CurrentSpectroscopyTable.Column(Me.pbAfterAveraging.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(AveragedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbAfterAveraging.SetSinglePreviewImage(OutputSpectroscopyTable,
                                                  Me.pbAfterAveraging.cbX.SelectedColumnName,
                                                  AveragedColumn.Name, False)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfterAveraging.cbX.SelectedColumnName = Me.pbAfterAveraging.cbX.SelectedColumnName
        Me.pbAfterAveraging.cbY.SelectedColumnName = AveragedColumn.Name

        ' Activate Save-Button
        Me.SetSaveButton(True)
    End Sub

#Region "Column Saving"
    ' This will be called from the worker thread to activate or deactivate the ColumnSave buttons
    Delegate Sub SetSaveButtonCallback(ByVal Enabled As Boolean)
    Friend Sub SetSaveButton(ByVal Enabled As Boolean)
        If Me.btnSaveColumn.InvokeRequired Then
            Dim _delegate As New SetSaveButtonCallback(AddressOf SetSaveButton)
            Me.Invoke(_delegate, Enabled)
        Else
            Me.btnSaveColumn.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the column gained from the averaging back to the source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataAverager.AveragedColumn Is Nothing Then Return

        ' save columns
        Me.DataAverager.SaveAveragedColumnToFileObject(Me.txtNewColumnName.Text)
    End Sub
#End Region

#Region "Window Closing"
    ''' <summary>
    ''' Close Window
    ''' </summary>
    Private Sub btnCloseWindow_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseWindow.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Function that runs when the form is closed... saves Settings.
    ''' </summary>
    Private Sub FormIsClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Saves the list of selected column names.
        With My.Settings
            .LastDataAveragingSingleFile_ColumnNames.Clear()
            .LastDataAveragingSingleFile_ColumnNames.AddRange(Me.lbSourceColumnSelector.SelectedColumnNames.ToArray)
            .Save()
        End With
    End Sub
#End Region

#Region "Automatic GUI update"
    ''' <summary>
    ''' update the GUI automatically, when changing values
    ''' </summary>
    Private Sub lbSourceColumnSelector_SelectedIndexChanged() Handles lbSourceColumnSelector.SelectedIndexChanged
        If Not Me.bReady Then Return

        If Me.lbSourceColumnSelector.SelectedColumnNames.Count > 0 Then
            Me.ApplyProcedure()
        End If
    End Sub
#End Region

End Class