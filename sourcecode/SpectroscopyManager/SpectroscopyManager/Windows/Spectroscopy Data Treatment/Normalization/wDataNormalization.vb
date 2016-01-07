Public Class wDataNormalization
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for Data Normalization of the Selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataNormalizer As cSpectroscopyTableDataNormalizer

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

    ''' <summary>
    ''' Limits for the reference of the normalization data.
    ''' </summary>
    Private LeftLimit As Double = My.Settings.LastNormalization_LeftPoint
    Private RightLimit As Double = My.Settings.LastNormalization_RightPoint

    ''' <summary>
    ''' Function that gets called, when the Background-Class finished loading a spectroscopy file.
    ''' </summary>
    Private Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Create new DataNormalizer Object
        Me.DataNormalizer = New cSpectroscopyTableDataNormalizer(Me._FileObject)
        Me.DataNormalizer.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set Preview-Images:
        Me.pbBeforeNormalization.SetSinglePreviewImage(SpectroscopyTable)
        Me.pbAfterNormalization.SetSinglePreviewImage(SpectroscopyTable)
        Me.csColumnToNormalize.InitializeColumns(SpectroscopyTable.GetColumnList)

        bReady = True

        ' Load properties from settings if possible!
        With My.Settings
            Me.pbBeforeNormalization.cbX.SelectedColumnName = .LastNormalization_ColumnX
            Me.csColumnToNormalize.SelectedColumnName = .LastNormalization_ColumnToNormalize
            Me.dsDataSmoother.SelectedSmoothingMethod = DirectCast(.LastNormalization_SmoothMethod, cNumericalMethods.SmoothingMethod)
            Me.dsDataSmoother.SmoothingParameter = .LastNormalization_SmoothNeighbors
            'Me.txtNewColumnName.Text = .LastNormalization_NewColumnName
            Me.txtLeftValue.SetValue(.LastNormalization_LeftPoint)
            Me.txtRightValue.SetValue(.LastNormalization_RightPoint)
            Me.LeftLimit = .LastNormalization_LeftPoint
            Me.RightLimit = .LastNormalization_RightPoint
            Me.ReplotCropRanges()
        End With

        ' Settings:
        Me.pbAfterNormalization.AllowAdjustingXColumn = False
        Me.pbAfterNormalization.AllowAdjustingYColumn = False

        ' Enable Point-Selection:
        Me.pbBeforeNormalization.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XRange
        Me.pbBeforeNormalization.Refresh()
    End Sub

    ''' <summary>
    ''' Set the X column of the before-data to the after-PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbBeforeNormalization.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterNormalization.cbX.SelectedColumnName = Me.pbBeforeNormalization.cbX.SelectedColumnName
        Me.csColumnToNormalize.SelectedColumnName = Me.pbBeforeNormalization.cbY.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected Smoothing Column
    ''' </summary>
    Private Sub SelectedNormalizationColumnChanged() Handles csColumnToNormalize.SelectedIndexChanged
        If Not Me.bReady Then Return

        Me.bReady = False
        Me.pbBeforeNormalization.cbY.SelectedColumnName = Me.csColumnToNormalize.SelectedColumnName

        ' Set ColumnTitle to Selected ColumnTitle
        Me.txtNewColumnName.Text = My.Resources.rDataNormalization.ColumnNameTemplate.Replace("%n", Me.csColumnToNormalize.SelectedColumnName)
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Applies the selected Smoothing-Algorithm to the selected Column
    ''' </summary>
    Public Sub btnApplyNormalization_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyNormalization.Click
        ' Security-Checks, if all Columns are selected.
        If Me.csColumnToNormalize.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Check, if the values are valid
        If Me.RightLimit < Me.LeftLimit Then
            MessageBox.Show(My.Resources.Message_UpperBorderLargerThanLower,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        Me.SetSaveButton(False)

        ' Send Normalization Command to Background-Class
        Me.DataNormalizer.NormalizeColumnWITHSmoothing_Async(Me.pbBeforeNormalization.cbX.SelectedColumnName,
                                                       Me.pbBeforeNormalization.cbY.SelectedColumnName,
                                                       Me.LeftLimit,
                                                       Me.RightLimit,
                                                       Me.dsDataSmoother.SelectedSmoothingMethod,
                                                       Me.dsDataSmoother.SmoothingParameter,
                                                       Me.txtNewColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the Normalization procedure is complete
    ''' </summary>
    Private Sub NormalizationFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                      ByRef NormalizedColumn As cSpectroscopyTable.DataColumn) Handles DataNormalizer.FileNormalizedComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        OutputSpectroscopyTable.FullFileName = "Normalization Preview"

        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataNormalizer.CurrentSpectroscopyTable.Column(Me.pbBeforeNormalization.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(NormalizedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbAfterNormalization.SetSinglePreviewImage(OutputSpectroscopyTable,
                                                      Me.pbBeforeNormalization.cbX.SelectedColumnName,
                                                      NormalizedColumn.Name, False)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfterNormalization.cbX.SelectedColumnName = Me.pbBeforeNormalization.cbX.SelectedColumnName
        Me.pbAfterNormalization.cbY.SelectedColumnName = NormalizedColumn.Name

        ' Activate Save-Button
        Me.SetSaveButton(True)
    End Sub

    ''' <summary>
    ''' Updates the crop ranges.
    ''' </summary>
    Protected Sub ReplotCropRanges()

        With Me.pbBeforeNormalization
            ' Set the crop markers to display
            .ClearHighlightRanges()
            .AddHighlightRange(mSpectroscopyTableViewer.SelectionModes.XRange,
                               New ZedGraph.PointPair(Me.LeftLimit, 0),
                               New ZedGraph.PointPair(Me.RightLimit, 0))

        End With

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
            Me.btnApplyNormalization.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the Column gained from the Normalization to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataNormalizer.NormalizedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataNormalizer.SaveNormalizedColumnToFileObject(Me.txtNewColumnName.Text)
    End Sub
#End Region

#Region "Point-Selection of the Normalization-Range"
    ''' <summary>
    ''' Save the left and right border.
    ''' </summary>
    Private Sub txtAbsoluteValue_TextChanged(ByRef SourceTextBox As NumericTextbox) Handles txtLeftValue.ValidValueChanged, txtRightValue.ValidValueChanged
        If SourceTextBox Is Me.txtRightValue Then
            Me.RightLimit = SourceTextBox.DecimalValue
        ElseIf SourceTextBox Is Me.txtLeftValue Then
            Me.LeftLimit = SourceTextBox.DecimalValue
        End If

        If Me.RightLimit < Me.LeftLimit Then
            Dim TMP As Double = Me.LeftLimit
            Me.LeftLimit = Me.RightLimit
            Me.RightLimit = TMP

            Me.txtLeftValue.SetValue(Me.LeftLimit,, False)
            Me.txtRightValue.SetValue(Me.RightLimit,, False)
        End If
        Me.ReplotCropRanges()
    End Sub

    ''' <summary>
    ''' Calculate from the entered Absolute Value the nearest point number.
    ''' </summary>
    Private Sub PointSelectionChanged(LeftValue As Double,
                                      RightValue As Double) Handles pbBeforeNormalization.PointSelectionChanged_XRange
        Me.LeftLimit = LeftValue
        Me.RightLimit = RightValue

        Me.txtLeftValue.SetValue(Me.LeftLimit)
        Me.txtRightValue.SetValue(Me.RightLimit)

        Me.ReplotCropRanges()
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
            .LastNormalization_LeftPoint = Me.LeftLimit
            .LastNormalization_RightPoint = Me.RightLimit
            .LastNormalization_SmoothMethod = Convert.ToInt32(Me.dsDataSmoother.SelectedSmoothingMethod)
            .LastNormalization_SmoothNeighbors = Me.dsDataSmoother.SmoothingParameter
            .LastNormalization_ColumnX = Me.pbBeforeNormalization.cbX.SelectedColumnName
            .LastNormalization_ColumnToNormalize = Me.pbBeforeNormalization.cbY.SelectedColumnName
            .Save()
        End With
    End Sub
#End Region

End Class