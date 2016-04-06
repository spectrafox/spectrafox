Public Class wDataSummationTwoSets
    Inherits wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad

#Region "Properties"

    ''' <summary>
    ''' Object for Data Summation of the Selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataSummer As cSpectroscopyTableDataSummerTwoSets

    Private bReady As Boolean = True
#End Region

#Region "Form Contructor"
    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set Window Title
        Me.Text &= MyBase._FileObjectList.First.FileName & " + " & MyBase._FileObjectList.Last.FileName

    End Sub
#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the Spectroscopy-File to the display.
    ''' </summary>
    Public Sub SpectroscopyTablesFetched() Handles MyBase.AllFilesFetched

        ' Create new DataSummer Object
        Me.DataSummer = New cSpectroscopyTableDataSummerTwoSets(Me._FileObjectList.First, Me._FileObjectList.Last)
        Dim LoadedSpectroscopyTables As New Dictionary(Of cFileObject, cSpectroscopyTable)
        LoadedSpectroscopyTables.Add(Me._FileObjectList.First, Me._SpectroscopyTableList.First)
        LoadedSpectroscopyTables.Add(Me._FileObjectList.Last, Me._SpectroscopyTableList.Last)
        Me.DataSummer.SpectroscopyTables = LoadedSpectroscopyTables

        ' Set Preview-Images:
        Me.pbBeforeSummation.SetSinglePreviewImage(Me._SpectroscopyTableList.First)
        Me.pbAfterSummation.SetSinglePreviewImage(Me._SpectroscopyTableList.First)
        Me.pbReferenceData.SetSinglePreviewImage(Me._SpectroscopyTableList.Last)

        ' Set Columns
        Me.csColumnToSum.InitializeColumns(Me._SpectroscopyTableList.First.GetColumnList)
        Me.csColumnToSumWith.InitializeColumns(Me._SpectroscopyTableList.Last.GetColumnList)

        ' Load Properties from Settings if possible!
        With My.Settings
            Me.pbBeforeSummation.cbX.SelectedColumnName = .LastDataSummationTwoSets_ColumnX
            Me.pbBeforeSummation.cbY.SelectedColumnName = .LastDataSummationTwoSets_ColumnToSum
            Me.csColumnToSumWith.SelectedColumnName = .LastDataSummationTwoSets_ColumnToSumWith
            Me.txtNewColumnName.Text = .LastDataSummationTwoSets_NewColumnName
            Me.txtSummationFactor.SetValue(.LastDataSummationTwoSets_ScalingFactor)

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
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected column
    ''' </summary>
    Private Sub SelectedYColumnChanged() Handles csColumnToSum.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterSummation.cbY.SelectedColumnName = Me.csColumnToSum.SelectedColumnName
        Me.pbBeforeSummation.cbY.SelectedColumnName = Me.csColumnToSum.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected reference Column
    ''' </summary>
    Private Sub SelectedReferenceColumnChanged() Handles csColumnToSumWith.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbReferenceData.cbY.SelectedColumnName = Me.csColumnToSumWith.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the X Column of the Before-Data to the After-PreviewBox
    ''' </summary>
    Private Sub PreviewReferenceColumnChanged() Handles pbReferenceData.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.csColumnToSumWith.SelectedColumnName = Me.pbReferenceData.cbY.SelectedColumnName
        Me.bReady = True
    End Sub
#End Region

    ''' <summary>
    ''' Applies the procedure to the selected columns
    ''' </summary>
    Public Sub btnApply_Click(sender As System.Object, e As System.EventArgs) Handles btnApplySummation.Click

        ' Security-Checks, if all Columns are selected.
        If Me.csColumnToSum.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        If Me.csColumnToSumWith.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Disable the interface.
        Me.SetSaveButton(False)

        ' Send multiplication command to Background-Class
        Me.DataSummer.SumColumnWithoutFetching_ASync(Me.csColumnToSum.SelectedColumnName,
                                                     Me.csColumnToSum.SelectedColumnName,
                                                     Me.txtSummationFactor.DecimalValue,
                                                     Me.txtNewColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the summation procedure is complete
    ''' </summary>
    Private Sub SummationFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                  ByRef SummedColumn As cSpectroscopyTable.DataColumn) Handles DataSummer.FileSummationComplete

        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataSummer.SpectroscopyTables.First.Value.Column(Me.pbBeforeSummation.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(SummedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbAfterSummation.SetSinglePreviewImage(OutputSpectroscopyTable,
                                                  Me.pbBeforeSummation.cbX.SelectedColumnName,
                                                  SummedColumn.Name, False)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfterSummation.cbX.SelectedColumnName = Me.pbBeforeSummation.cbX.SelectedColumnName
        Me.pbAfterSummation.cbY.SelectedColumnName = SummedColumn.Name

        ' Activate Save-Button
        Me.SetSaveButton(True)
    End Sub


#Region "Exchange source and reference file."

    ''' <summary>
    ''' Exchange source and reference file.
    ''' </summary>
    Private Sub btnExchangeSourceAndReferenceData_Click(sender As Object, e As EventArgs) Handles btnExchangeSourceAndReferenceData.Click

        ' Reverse the order of the file list to exchange source and reference.
        Me._FileObjectList.Reverse()
        Me._SpectroscopyTableList.Reverse()

        ' Reload the interface.
        Me.SaveSettings()
        Me.SpectroscopyTablesFetched()

    End Sub

#End Region

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
        Me.SaveSettings()
    End Sub

    ''' <summary>
    ''' Saves the current settings.
    ''' </summary>
    Public Sub SaveSettings()
        ' Saves the Chosen Parameters to the Settings.
        With My.Settings
            .LastDataSummationTwoSets_ScalingFactor = Me.txtSummationFactor.DecimalValue
            .LastDataSummationTwoSets_ReferenceFileFullPath = MyBase._FileObjectList.Last.FullFileNameInclPath
            .LastDataSummationTwoSets_NewColumnName = Me.txtNewColumnName.Text
            .LastDataSummationTwoSets_ColumnX = Me.pbBeforeSummation.cbX.SelectedColumnName
            .LastDataSummationTwoSets_ColumnToSum = Me.pbBeforeSummation.cbY.SelectedColumnName
            .LastDataSummationTwoSets_ColumnToSumWith = Me.pbReferenceData.cbY.SelectedColumnName
            cGlobal.SaveSettings()
        End With
    End Sub
#End Region

End Class