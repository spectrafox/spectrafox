Public Class wDataNumericDerivative
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for Data Derivative calculation of the Selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataDeriver As cSpectroscopyTableDataDeriver

    Private bReady As Boolean = False
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
        ' Set Preview-Images:
        Me.pbSourcePreview.SetSinglePreviewImage(SpectroscopyTable, My.Settings.LastDerivative_SourceColumnX, My.Settings.LastDerivative_SourceColumn)
        Me.pbTargetPreview.SetSinglePreviewImage(SpectroscopyTable, My.Settings.LastDerivative_SourceColumnX, My.Settings.LastDerivative_SourceColumn)

        ' Set Columns
        Me.csColumn.InitializeColumns(SpectroscopyTable.GetColumnList, My.Settings.LastDerivative_SourceColumn)

        ' Enable the interface
        Me.bReady = True

        ' Load Properties from Settings if possible!
        With My.Settings
            'Me.pbSourcePreview.cbX.SelectedColumnName = .LastDerivative_SourceColumnX
            Me.pbSourcePreview.cbY.SelectedColumnName = .LastDerivative_SourceColumn
            Me.nudDerivativeOrder.Value = .LastDerivative_DerivativeOrder
            Me.dsDataSmoother.SelectedSmoothingMethodType = cNumericalMethods.GetSmoothingMethodFromString(.LastDerivative_SmoothingMethod)
            Me.dsDataSmoother.SetSmoothingSettings(.LastDerivative_SmoothingOptions)
            Me.txtNewColumnName.Text = .LastDerivative_NewColumnName
        End With

        ' Create new DataMultiplier Object
        Me.DataDeriver = New cSpectroscopyTableDataDeriver(Me._FileObject)
        Me.DataDeriver.CurrentSpectroscopyTable = SpectroscopyTable

    End Sub

#End Region

#Region "Synchronize the Columns"
    ''' <summary>
    ''' Set the X Column of the Before-Data to the After-PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbSourcePreview.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbTargetPreview.cbX.SelectedColumnName = Me.pbSourcePreview.cbX.SelectedColumnName
        Me.csColumn.SelectedColumnName = Me.pbSourcePreview.cbY.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected multiplication Column
    ''' </summary>
    Private Sub SelectedMultiplicationColumnChanged() Handles csColumn.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbTargetPreview.cbY.SelectedColumnName = Me.csColumn.SelectedColumnName
        Me.pbSourcePreview.cbY.SelectedColumnName = Me.csColumn.SelectedColumnName
        Me.bReady = True
    End Sub
#End Region

    ''' <summary>
    ''' Applies the selected derivative to the selected Column
    ''' </summary>
    Public Sub btnApplyDerivation_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyDerivation.Click
        ' Security-Checks, if all Columns are selected.
        If Me.csColumn.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Send derivation command to Background-Class
        Me.DataDeriver.DerivateColumnWITHSmoothing_Async(Me.pbSourcePreview.cbX.SelectedColumnName,
                                                         Me.csColumn.SelectedColumnName,
                                                         Me.dsDataSmoother.GetSmoothingMethod,
                                                         Me.txtNewColumnName.Text,
                                                         CInt(Me.nudDerivativeOrder.Value))

    End Sub

    ''' <summary>
    ''' Function that gets called, when the derivation procedure is complete
    ''' </summary>
    Private Sub DerivationFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                   ByRef DerivedColumn As cSpectroscopyTable.DataColumn) Handles DataDeriver.FileDerivatedComplete

        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        'OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataDeriver.CurrentSpectroscopyTable.Column(Me.pbSourcePreview.cbX.SelectedColumnName))
        For Each Column As cSpectroscopyTable.DataColumn In Me.DataDeriver.CurrentSpectroscopyTable.Columns.Values
            OutputSpectroscopyTable.AddNonPersistentColumn(Column)
        Next
        OutputSpectroscopyTable.AddNonPersistentColumn(DerivedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbTargetPreview.SetSinglePreviewImage(OutputSpectroscopyTable,
                                                 Me.pbSourcePreview.cbX.SelectedColumnName,
                                                 DerivedColumn.Name, False)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbTargetPreview.cbX.SelectedColumnName = Me.pbSourcePreview.cbX.SelectedColumnName
        Me.pbTargetPreview.cbY.SelectedColumnName = DerivedColumn.Name

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
    ''' Saves the Column Gained from the derivation to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataDeriver.DerivatedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataDeriver.SaveDerivatedColumnToFileObject(Me.txtNewColumnName.Text)
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
            .LastDerivative_DerivativeOrder = CInt(Me.nudDerivativeOrder.Value)
            .LastDerivative_SmoothingMethod = Me.dsDataSmoother.SelectedSmoothingMethodType.ToString
            .LastDerivative_SmoothingOptions = Me.dsDataSmoother.GetSmoothingSettings
            .LastDerivative_SourceColumnX = Me.pbSourcePreview.cbX.SelectedColumnName
            .LastDerivative_SourceColumn = Me.pbSourcePreview.cbY.SelectedColumnName
            .LastDerivative_NewColumnName = Me.txtNewColumnName.Text
            .Save()
        End With
    End Sub
#End Region

End Class