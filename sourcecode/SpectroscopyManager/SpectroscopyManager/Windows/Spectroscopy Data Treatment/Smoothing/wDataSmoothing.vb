Public Class wDataSmoothing
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    Private bReady As Boolean = False

    ''' <summary>
    ''' Object for Data Smoothing of the Selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataSmoother As cSpectroscopyTableDataSmoother

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

        ' Set the preview-boxes
        Me.pbBeforeSmoothing.AllowAdjustingYColumn = False

    End Sub
#End Region

    ''' <summary>
    ''' Function that gets called, when the Background-Class finished loading a spectroscopy file.
    ''' </summary>
    Private Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Create new DataSmoother Object
        Me.DataSmoother = New cSpectroscopyTableDataSmoother(Me._FileObject)
        ' Load the Spectroscopy-Table-File using Background-Class.
        Me.DataSmoother.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set the preview
        Me.pbBeforeSmoothing.SetSinglePreviewImage(SpectroscopyTable, My.Settings.LastSmoothing_ColumnX, My.Settings.LastSmoothing_ColumnToSmooth)

        ' Set Preview-Images:
        Me.csColumnToSmooth.InitializeColumns(SpectroscopyTable.GetColumnList, My.Settings.LastSmoothing_ColumnToSmooth)

        Me.bReady = True

        ' Load Properties from settings if possible!
        With My.Settings
            'Me.csColumnToSmooth.SelectedColumnName = .LastSmoothing_ColumnToSmooth
            Me.pbBeforeSmoothing.cbY.SelectedColumnName = .LastSmoothing_ColumnToSmooth
            Me.dsDataSmoother.SelectedSmoothingMethodType = cNumericalMethods.GetSmoothingMethodFromString(.LastSmoothing_SmoothMethod)
            Me.dsDataSmoother.SetSmoothingSettings(.LastSmoothing_SmoothOptions)
            Me.txtNewColumnName.Text = .LastSmoothing_SmoothedColumnName
        End With

    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected Smoothing Column
    ''' </summary>
    Private Sub SelectedSmoothingColumnChanged() Handles csColumnToSmooth.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbBeforeSmoothing.cbY.SelectedColumnName = Me.csColumnToSmooth.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Applies the selected Smoothing-Algorithm to the selected Column
    ''' </summary>
    Public Sub btnApplySmoothing_Click(sender As System.Object, e As System.EventArgs) Handles btnApplySmoothing.Click
        ' Security-Checks, if all Columns are selected.
        If Me.csColumnToSmooth.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Activate Save-Button
        Me.SetSaveButton(False)

        ' Set the selected column name, to reselect it later.
        Me._SelectedXColumn = Me.pbBeforeSmoothing.cbX.SelectedColumnName

        ' Send Smoothing Command to Background-Class
        Me.DataSmoother.SmoothColumnWithoutFetching_Async(Me.csColumnToSmooth.SelectedColumnName,
                                                          Me.dsDataSmoother.GetSmoothingMethod,
                                                          Me.txtNewColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the Smoothing procedure is complete
    ''' </summary>
    Private Sub SmoothingFinished(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef SmoothedColumn As cSpectroscopyTable.DataColumn) Handles DataSmoother.FileSmoothingComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        'OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataSmoother.CurrentSpectroscopyTable.Column(Me._SelectedXColumn))
        'OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataSmoother.CurrentSpectroscopyTable.Column(Me.csColumnToSmooth.SelectedColumnName))
        For Each Column As cSpectroscopyTable.DataColumn In Me.DataSmoother.CurrentSpectroscopyTable.Columns.Values
            OutputSpectroscopyTable.AddNonPersistentColumn(Column)
        Next
        OutputSpectroscopyTable.AddNonPersistentColumn(SmoothedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        With Me.pbBeforeSmoothing
            .ClearSpectroscopyTables()
            .AddSpectroscopyTable(OutputSpectroscopyTable)
            .SelectColumns(Me._SelectedXColumn, New List(Of String)({SmoothedColumn.Name, Me.csColumnToSmooth.SelectedColumnName}))
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
            Me.btnApplySmoothing.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the Column Gained from the Smoothing to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataSmoother.SmoothedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataSmoother.SaveSmoothedColumnToFileObject(Me.txtNewColumnName.Text)
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
            .LastSmoothing_SmoothMethod = Me.dsDataSmoother.SelectedSmoothingMethodType.ToString
            .LastSmoothing_SmoothOptions = Me.dsDataSmoother.GetSmoothingSettings
            .LastSmoothing_ColumnX = Me.pbBeforeSmoothing.cbX.SelectedColumnName
            .LastSmoothing_ColumnToSmooth = Me.csColumnToSmooth.SelectedColumnName
            .LastSmoothing_SmoothedColumnName = Me.txtNewColumnName.Text
            .Save()
        End With
    End Sub
#End Region

End Class