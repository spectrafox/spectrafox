Public Class wSpectroscopyFFT
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for FFT of the selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataFFT As cSpectroscopyTableFFT

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
    ''' Function that gets called, when the Background-Class finished loading a spectroscopy file.
    ''' </summary>
    Private Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Create new DataFFT Object
        Me.DataFFT = New cSpectroscopyTableFFT(Me._FileObject)
        Me.DataFFT.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set Preview-Images:
        Me.pbBeforeFFT.SetSinglePreviewImage(SpectroscopyTable)
        Me.pbAfterFFT.SetSinglePreviewImage(SpectroscopyTable)
        Me.csColumnToFFT.InitializeColumns(SpectroscopyTable.GetColumnList)

        bReady = True

        ' Load properties from settings if possible!
        With My.Settings
            Me.pbBeforeFFT.cbX.SelectedColumnName = .LastFFT_ColumnName
            Me.csColumnToFFT.SelectedColumnName = .LastFFT_ColumnName
            Me.txtNewColumnName.Text = .LastFFT_ColumnTargetName
        End With

        ' Settings:
        Me.pbAfterFFT.AllowAdjustingXColumn = False
        Me.pbAfterFFT.AllowAdjustingYColumn = False
    End Sub

    ''' <summary>
    ''' Set the X column of the before-data to the after-PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbBeforeFFT.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterFFT.cbX.SelectedColumnName = Me.pbBeforeFFT.cbX.SelectedColumnName
        Me.csColumnToFFT.SelectedColumnName = Me.pbBeforeFFT.cbY.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected FFT Column
    ''' </summary>
    Private Sub SelectedFFTColumnChanged() Handles csColumnToFFT.SelectedIndexChanged
        If Not Me.bReady Then Return

        Me.bReady = False
        Me.pbBeforeFFT.cbY.SelectedColumnName = Me.csColumnToFFT.SelectedColumnName

        ' Set ColumnTitle to Selected ColumnTitle
        'Me.txtNewColumnName.Text = My.Resources.rDataNormalization.ColumnNameTemplate.Replace("%n", Me.csColumnToFFT.SelectedColumnName)
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Applies the selected FFT-Algorithm to the selected Column
    ''' </summary>
    Public Sub btnApplyFFT_Click(sender As System.Object, e As System.EventArgs) Handles btnApply.Click
        ' Security-Checks, if all Columns are selected.
        If Me.csColumnToFFT.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        Me.SetSaveButton(False)

        ' Send FFT Command to Background-Class
        Me.DataFFT.FFTColumnWITHAutomaticFetching_Async(Me.pbBeforeFFT.cbX.SelectedColumnName,
                                                        Me.txtNewColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the FFT procedure is complete
    ''' </summary>
    Private Sub FFTFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                      ByRef FFTColumn As cSpectroscopyTable.DataColumn) Handles DataFFT.FileFFTComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        OutputSpectroscopyTable.FullFileName = "FFT Preview"

        ' Add XColumn
        'OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataNormalizer.CurrentSpectroscopyTable.Column(Me.pbBeforeNormalization.cbX.SelectedColumnName))
        For Each Column As cSpectroscopyTable.DataColumn In Me.DataFFT.CurrentSpectroscopyTable.Columns.Values
            OutputSpectroscopyTable.AddNonPersistentColumn(Column)
        Next
        OutputSpectroscopyTable.AddNonPersistentColumn(FFTColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbAfterFFT.SetSinglePreviewImage(OutputSpectroscopyTable,
                                            Me.pbBeforeFFT.cbX.SelectedColumnName,
                                            FFTColumn.Name, False)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfterFFT.cbX.SelectedColumnName = Me.pbBeforeFFT.cbX.SelectedColumnName
        Me.pbAfterFFT.cbY.SelectedColumnName = FFTColumn.Name

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
            Me.btnApply.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the Column gained from the FFT to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.DataFFT.FFTColumn Is Nothing Then Return

        ' Save Columns
        Me.DataFFT.SaveColumnToFileObject(Me.txtNewColumnName.Text)
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
            .LastFFT_ColumnTargetName = Me.txtNewColumnName.Text
            .LastFFT_ColumnName = Me.pbBeforeFFT.cbX.SelectedColumnName
            cGlobal.SaveSettings()
        End With
    End Sub
#End Region

End Class