Public Class wDataSummation
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for Data Summation of the Selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataSummer As cSpectroscopyTableDataSummer

    Private bReady As Boolean = True
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

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the Spectroscopy-File to the display.
    ''' </summary>
    Public Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Create new DataSummer Object
        Me.DataSummer = New cSpectroscopyTableDataSummer(Me._FileObject)
        Me.DataSummer.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set Preview-Images:
        Me.pbBeforeSummation.SetSinglePreviewImage(SpectroscopyTable)
        Me.pbAfterSummation.SetSinglePreviewImage(SpectroscopyTable)

        ' Set Columns
        Me.csColumnToSum.InitializeColumns(SpectroscopyTable.GetColumnList)
        Me.csColumnToSumWith.InitializeColumns(SpectroscopyTable.GetColumnList)

        ' Load Properties from Settings if possible!
        With My.Settings
            Me.pbBeforeSummation.cbX.SelectedColumnName = .LastDataSummation_ColumnX
            Me.pbBeforeSummation.cbY.SelectedColumnName = .LastDataSummation_ColumnToSum
            Me.csColumnToSumWith.SelectedColumnName = .LastDataSummation_ColumnToBeSummedWith
            Me.txtSummationFactor.SetValue(.LastDataSummation_Factor)

            Select Case .LastDataSummation_Method
                Case cSpectroscopyTableDataSummer.SummationMode.ByValue
                    Me.tcSummationMethod.SelectedTab = Me.tpFactorSummation
                Case cSpectroscopyTableDataSummer.SummationMode.OtherColumnSum
                    Me.rbAdd.Checked = True
                    Me.tcSummationMethod.SelectedTab = Me.tpOtherColumnSummation
                Case cSpectroscopyTableDataSummer.SummationMode.OtherColumnSubtract
                    Me.rbSubtract.Checked = True
                    Me.tcSummationMethod.SelectedTab = Me.tpOtherColumnSummation
            End Select
        End With

    End Sub

#End Region

#Region "Synchronize the Columns"
    ''' <summary>
    ''' Set the X Column of the Before-Data to the After-PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbBeforeSummation.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterSummation.cbX.SelectedColumnName = Me.pbBeforeSummation.cbX.SelectedColumnName
        Me.csColumnToSum.SelectedColumnName = Me.pbBeforeSummation.cbY.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected multiplication Column
    ''' </summary>
    Private Sub SelectedMultiplicationColumnChanged() Handles csColumnToSum.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterSummation.cbY.SelectedColumnName = Me.csColumnToSum.SelectedColumnName
        Me.pbBeforeSummation.cbY.SelectedColumnName = Me.csColumnToSum.SelectedColumnName
        Me.bReady = True
    End Sub
#End Region

    ''' <summary>
    ''' Applies the selected summation to the column.
    ''' </summary>
    Public Sub btnApplySummation_Click(sender As System.Object, e As System.EventArgs) Handles btnApplySummation.Click
        ' Security-Checks, if all Columns are selected.
        If Me.csColumnToSum.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        If Me.csColumnToSumWith.SelectedColumnName = String.Empty And Me.tcSummationMethod.SelectedTab.Name = Me.tpOtherColumnSummation.Name Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Deactivate the interface.
        Me.SetSaveButton(False)

        ' Send multiplication command to Background-Class
        Select Case Me.tcSummationMethod.SelectedTab.Name
            Case Me.tpFactorSummation.Name
                ' Multiply by fixed factor
                Me.DataSummer.SumColumnWithoutFetching_ASync(Me.csColumnToSum.SelectedColumnName,
                                                             Me.txtSummationFactor.DecimalValue,
                                                             Me.txtNewColumnName.Text)
            Case Me.tpOtherColumnSummation.Name
                Dim SumMode As cSpectroscopyTableDataSummer.SummationMode
                If Me.rbAdd.Checked Then
                    SumMode = cSpectroscopyTableDataSummer.SummationMode.OtherColumnSum
                Else
                    SumMode = cSpectroscopyTableDataSummer.SummationMode.OtherColumnSubtract
                End If

                ' Sum other column
                Me.DataSummer.SumColumnWithoutFetching_ASync(Me.csColumnToSum.SelectedColumnName,
                                                             SumMode,
                                                             Me.csColumnToSumWith.SelectedColumnName,
                                                             Me.txtNewColumnName.Text)
        End Select
    End Sub

    ''' <summary>
    ''' Function that gets called, when the summation procedure is complete
    ''' </summary>
    Private Sub SummationFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                  ByRef SummedColumn As cSpectroscopyTable.DataColumn) Handles DataSummer.FileSummationComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataSummer.CurrentSpectroscopyTable.Column(Me.pbBeforeSummation.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(SummedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbAfterSummation.SetSinglePreviewImage(OutputSpectroscopyTable,
                                                  Me.pbBeforeSummation.cbX.SelectedColumnName,
                                                  SummedColumn.Name,
                                                  False)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfterSummation.cbX.SelectedColumnName = Me.pbBeforeSummation.cbX.SelectedColumnName
        Me.pbAfterSummation.cbY.SelectedColumnName = SummedColumn.Name

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
            Me.btnApplySummation.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the Column Gained from the multiplication to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataSummer.SummedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataSummer.SaveSummedColumnToFileObject(Me.txtNewColumnName.Text)
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
            Dim SumMode As cSpectroscopyTableDataSummer.SummationMode
            Select Case Me.tcSummationMethod.SelectedTab.Name
                Case Me.tpFactorSummation.Name
                    SumMode = cSpectroscopyTableDataSummer.SummationMode.ByValue
                Case Me.tpOtherColumnSummation.Name
                    If Me.rbAdd.Checked Then
                        SumMode = cSpectroscopyTableDataSummer.SummationMode.OtherColumnSum
                    Else
                        SumMode = cSpectroscopyTableDataSummer.SummationMode.OtherColumnSubtract
                    End If
            End Select

            .LastDataSummation_Factor = Me.txtSummationFactor.DecimalValue
            .LastDataSummation_ColumnToBeSummedWith = Me.csColumnToSumWith.SelectedColumnName
            .LastDataSummation_Method = SumMode
            .LastDataSummation_ColumnX = Me.pbBeforeSummation.cbX.SelectedColumnName
            .LastDataSummation_ColumnToSum = Me.pbBeforeSummation.cbY.SelectedColumnName
            .LastDataSummation_NewColumnName = Me.txtNewColumnName.Text
            .Save()
        End With
    End Sub
#End Region

End Class