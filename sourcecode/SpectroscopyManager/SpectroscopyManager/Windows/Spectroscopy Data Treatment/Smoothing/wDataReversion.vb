Public Class wDataReversion
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for reversing the data in a spectroscopy file.
    ''' </summary>
    Private WithEvents DataReverser As cSpectroscopyTableDataReverter

    Private bReady As Boolean = False

    ''' <summary>
    ''' Stores the selected X-Column name that was displayed before the smoothing.
    ''' </summary>
    Private _SelectedXColumn As String

#End Region

#Region "Form Contructor"
    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set Window Title
        Me.Text &= MyBase._FileObject.FileName

        ' Forbid preview Y column change
        Me.pbBefore.AllowAdjustingYColumn = False

    End Sub
#End Region

    ''' <summary>
    ''' Function that gets called, when the Background-Class finished loading a spectroscopy file.
    ''' </summary>
    Private Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        Me.DataReverser = New cSpectroscopyTableDataReverter(Me._FileObject)
        ' Load the Spectroscopy-Table-File using Background-Class.
        Me.DataReverser.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set the preview
        Me.pbBefore.SetSinglePreviewImage(SpectroscopyTable, My.Settings.LastReversion_ColumnX, My.Settings.LastReversion_ColumnToReverse)

        ' Set Preview-Images:
        Me.csColumnToRevert.InitializeColumns(SpectroscopyTable.GetColumnList)

        Me.bReady = True

        ' Load Properties From Settings if possible!
        With My.Settings
            'Me.pbBefore.cbX.SelectedColumnName = .LastReversion_ColumnX
            Me.pbBefore.cbY.SelectedColumnName = .LastReversion_ColumnToReverse
            Me.txtNewColumnName.Text = .LastReversion_ReversedColumnName
        End With

    End Sub

    ''' <summary>
    ''' Set the X Column of the Before-Data to the After-PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbBefore.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.csColumnToRevert.SelectedColumnName = Me.pbBefore.cbY.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected Smoothing Column
    ''' </summary>
    Private Sub SelectedSmoothingColumnChanged() Handles csColumnToRevert.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbBefore.cbY.SelectedColumnName = Me.csColumnToRevert.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Reverts the selected column
    ''' </summary>
    Public Sub btnApplyReversion_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyReversion.Click
        ' Security-Checks, if all Columns are selected.
        If Me.csColumnToRevert.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Disable interface
        Me.SetSaveButton(False)

        ' Set the selected column name, to reselect it later.
        Me._SelectedXColumn = Me.pbBefore.cbX.SelectedColumnName

        ' Send reversion command to Background-Class
        Me.DataReverser.ReverseColumnWithoutFetching_Async(Me.csColumnToRevert.SelectedColumnName,
                                                           Me.txtNewColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the reversion procedure is complete
    ''' </summary>
    Private Sub ReversionFinished(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef ReversedColumn As cSpectroscopyTable.DataColumn) Handles DataReverser.FileReversionComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        'OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataReverser.CurrentSpectroscopyTable.Column(Me._SelectedXColumn))
        'OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataReverser.CurrentSpectroscopyTable.Column(Me.csColumnToRevert.SelectedColumnName))
        For Each Column As cSpectroscopyTable.DataColumn In Me.DataReverser.CurrentSpectroscopyTable.Columns.Values
            OutputSpectroscopyTable.AddNonPersistentColumn(Column)
        Next
        OutputSpectroscopyTable.AddNonPersistentColumn(ReversedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        With Me.pbBefore
            .ClearSpectroscopyTables()
            .AddSpectroscopyTable(OutputSpectroscopyTable)
            .SelectColumns(Me._SelectedXColumn, New List(Of String)({ReversedColumn.Name, Me.csColumnToRevert.SelectedColumnName}))
        End With

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
            Me.btnApplyReversion.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the column generated by the reversion to the source spectroscopytable.
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataReverser.ReversedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataReverser.SaveReversedColumnToFileObject(Me.txtNewColumnName.Text)
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
        ' Saves the Chosen Parameters to the Settings.
        With My.Settings
            .LastReversion_ColumnX = Me.pbBefore.cbX.SelectedColumnName
            .LastReversion_ColumnToReverse = Me.csColumnToRevert.SelectedColumnName
            .LastReversion_ReversedColumnName = Me.txtNewColumnName.Text
            .Save()
        End With
    End Sub
#End Region

End Class