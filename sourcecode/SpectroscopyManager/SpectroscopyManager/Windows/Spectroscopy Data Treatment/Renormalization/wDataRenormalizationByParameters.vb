Public Class wDataRenormalizationByParameters
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"

    Private bReady As Boolean = False

    ''' <summary>
    ''' Object for regauging the selected file by known lockin parameters.
    ''' </summary>
    Private WithEvents DataRenormalizer As cSpectroscopyTableDataRegaugeByLockinParameter

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
        Me.pbBeforeRegauging.SetSinglePreviewImage(SpectroscopyTable)
        Me.pbAfterRegauging.SetSinglePreviewImage(SpectroscopyTable)

        ' Create new DataRenormalizer Object
        Me.DataRenormalizer = New cSpectroscopyTableDataRegaugeByLockinParameter(Me._FileObject)
        ' Set the currently loaded SpectroscopyFile to the Renormalizer-Object.
        Me.DataRenormalizer.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set Columns
        Me.csSourceColumn.InitializeColumns(SpectroscopyTable.GetColumnList)

        ' Load Properties from Settings if possible!
        With My.Settings
            Me.pbBeforeRegauging.cbX.SelectedColumnName = .LastRenormalizationByParameter_SourceColumnX
            Me.pbBeforeRegauging.cbY.SelectedColumnName = .LastRenormalizationByParameter_SourceColumn
            Me.csSourceColumn.SelectedColumnName = .LastRenormalizationByParameter_SourceColumn
            Me.txtBiasModulation.SetValue(.LastRenormalizationByParameter_LockInBiasModulation)
            Me.txtLockInSensitivity.SetValue(.LastRenormalizationByParameter_LockInSensitivity)
            Me.nudAmplifierGain.Value = .LastRenormalizationByParameter_AmplifierGain
            Me.txtNewColumnName.Text = .LastRenormalizationByParameter_NewColumnName
        End With

        Me.bReady = True

        ' disable selection of columns
        Me.pbAfterRegauging.AllowAdjustingXColumn = False
    End Sub

#End Region

#Region "Synchronize the Columns"
    ''' <summary>
    ''' Set the X Column of the Before-Data to the After-PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbBeforeRegauging.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterRegauging.cbX.SelectedColumnName = Me.pbBeforeRegauging.cbX.SelectedColumnName
        Me.csSourceColumn.SelectedColumnName = Me.pbBeforeRegauging.cbY.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected multiplication Column
    ''' </summary>
    Private Sub SelectedMultiplicationColumnChanged() Handles csSourceColumn.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbBeforeRegauging.cbY.SelectedColumnName = Me.csSourceColumn.SelectedColumnName
        Me.pbAfterRegauging.cbY.SelectedColumnName = Me.csSourceColumn.SelectedColumnName
        Me.bReady = True
    End Sub
#End Region

    ''' <summary>
    ''' Applies the re-gauging to the selected Column
    ''' </summary>
    Public Sub btnApplyRegauging_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyRegauging.Click

        ' Security-Checks, if all Columns are selected.
        If Me.csSourceColumn.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Send Re-Gauging command to Background-Class
        Me.DataRenormalizer.RenormalizeColumnWithoutFetch_Async(Me.csSourceColumn.SelectedColumnName,
                                                                Me.txtBiasModulation.DecimalValue,
                                                                Me.txtLockInSensitivity.DecimalValue,
                                                                CInt(Me.nudAmplifierGain.Value),
                                                                Me.txtNewColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the re-gauging procedure is complete
    ''' </summary>
    Private Sub ReGaugingFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                  ByRef Column As cSpectroscopyTable.DataColumn) Handles DataRenormalizer.FileRenormalizedComplete

        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable

        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataRenormalizer.CurrentSpectroscopyTable.Column(Me.pbBeforeRegauging.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(Column)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbAfterRegauging.SetSinglePreviewImage(OutputSpectroscopyTable)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfterRegauging.cbX.SelectedColumnName = Me.pbBeforeRegauging.cbX.SelectedColumnName
        Me.pbAfterRegauging.cbY.SelectedColumnName = Column.Name

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
    ''' Saves the Column Gained from the multiplication to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataRenormalizer.RenormalizedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataRenormalizer.SaveRenormalizedColumnToFileObject(Me.txtNewColumnName.Text)
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
            .LastRenormalizationByParameter_LockInBiasModulation = Me.txtBiasModulation.DecimalValue
            .LastRenormalizationByParameter_LockInSensitivity = Me.txtLockInSensitivity.DecimalValue
            .LastRenormalizationByParameter_AmplifierGain = CInt(Me.nudAmplifierGain.Value)
            .LastRenormalizationByParameter_SourceColumnX = Me.pbBeforeRegauging.cbX.SelectedColumnName
            .LastRenormalizationByParameter_SourceColumn = Me.pbBeforeRegauging.cbY.SelectedColumnName
            .LastRenormalizationByParameter_NewColumnName = Me.txtNewColumnName.Text
            .Save()
        End With
    End Sub
#End Region

End Class