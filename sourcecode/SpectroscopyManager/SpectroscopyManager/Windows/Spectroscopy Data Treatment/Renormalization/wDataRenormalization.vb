Public Class wDataRenormalization
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for Data Renormalizing of the Selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataRenormalizer As cSpectroscopyTableDataRenormalizer
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

        ' Set Preview-Images:
        Me.pbSourceData.SetSinglePreviewImage(SpectroscopyTable)
        Me.pbTargetData.SetSinglePreviewImage(SpectroscopyTable)

        ' Create new DataRenormalizer Object
        Me.DataRenormalizer = New cSpectroscopyTableDataRenormalizer(Me._FileObject)
        ' Load the Spectroscopy-Table-File using Background-Class.
        Me.DataRenormalizer.CurrentSpectroscopyTable = SpectroscopyTable

        ' Load Properties From Settings if possible!
        With My.Settings
            Me.dsDataSmoothing.SelectedSmoothingMethod = DirectCast(.LastRenormalization_SmoothMethod, cNumericalMethods.SmoothingMethod)
            Me.dsDataSmoothing.SmoothingParameter = .LastRenormalization_SmoothNeighbors
            Me.ckbSaveDerivedData.Checked = .LastRenormalization_SaveDerived
            Me.ckbSaveSmoothedData.Checked = .LastRenormalization_SaveSmoothed
            Me.txtDerivedDataColumnName.Text = .LastRenormalization_DerivedColumnName
            Me.txtSmoothedDataColumnName.Text = .LastRenormalization_SmoothedColumnName
            Me.txtRenormalizedDataColumnName.Text = .LastRenormalization_RenormColumnName

            Me.pbSourceData.cbX.SelectedColumnName = .LastRenormalization_SourceColumnX
            Me.pbSourceData.cbY.SelectedColumnName = .LastRenormalization_SourceColumnY
            'Me.pbSourceData.RepaintImage()
            Me.pbTargetData.cbX.SelectedColumnName = .LastRenormalization_TargetColumnX
            Me.pbTargetData.cbY.SelectedColumnName = .LastRenormalization_TargetColumnY
            'Me.pbTargetData.RepaintImage()
        End With

        ' Settings:
        Me.pbTargetData.AllowAdjustingXColumn = False
    End Sub

    ''' <summary>
    ''' Runs the Renormalization and Saves the Data in a new SpectroscopyTable.
    ''' </summary>
    Public Sub btnApplyRenormalization_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyRenormalization.Click
        ' Security-Checks, if all Columns are selected.
        If Me.pbSourceData.cbX.SelectedColumnName = String.Empty Or
           Me.pbSourceData.cbY.SelectedColumnName = String.Empty Or
           Me.pbTargetData.cbX.SelectedColumnName = String.Empty Or
           Me.pbTargetData.cbY.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Send Renormalization Command to Background-Class
        Me.DataRenormalizer.RenormalizeColumnWITHDerivation_Async(Me.pbSourceData.cbX.SelectedColumnName,
                                                                  Me.pbSourceData.cbY.SelectedColumnName,
                                                                  Me.dsDataSmoothing.SelectedSmoothingMethod,
                                                                  Me.dsDataSmoothing.SmoothingParameter,
                                                                  Me.pbTargetData.cbX.SelectedColumnName,
                                                                  Me.pbTargetData.cbY.SelectedColumnName,
                                                                  Me.txtRenormalizedDataColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the Renormalization procedure is complete
    ''' </summary>
    Private Sub RenormalizationFinished(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RenormalizedColumn As cSpectroscopyTable.DataColumn) Handles DataRenormalizer.FileRenormalizedComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        OutputSpectroscopyTable.AddColumn(Me.DataRenormalizer.CurrentSpectroscopyTable.Column(Me.pbTargetData.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddColumn(RenormalizedColumn)
        OutputSpectroscopyTable.AddColumn(Me.DataRenormalizer.SmoothedColumn)
        OutputSpectroscopyTable.AddColumn(Me.DataRenormalizer.DerivatedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbOutput.SetSinglePreviewImage(OutputSpectroscopyTable)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbOutput.cbX.SelectedColumnName = Me.pbTargetData.cbX.SelectedColumnName
        Me.pbOutput.cbY.SelectedColumnName = RenormalizedColumn.Name

        ' Activate Save-Button
        Me.SetSaveButton(True)
    End Sub

#Region "Column Saving"
    ' This will be called from the worker thread to activate or deactivate the ColumnSave buttons
    Delegate Sub SetSaveButtonCallback(ByVal Enabled As Boolean)
    Friend Sub SetSaveButton(ByVal Enabled As Boolean)
        If Me.btnSaveColumns.InvokeRequired Then
            Dim _delegate As New SetSaveButtonCallback(AddressOf SetSaveButton)
            Me.Invoke(_delegate, Enabled)
        Else
            Me.btnSaveColumns.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the Columns Gained to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumns_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumns.Click
        If Me.DataRenormalizer.RenormalizedColumn Is Nothing Or
           Me.DataRenormalizer.DerivatedColumn Is Nothing Or
           Me.DataRenormalizer.SmoothedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataRenormalizer.SaveRenormalizedColumnToFileObject(Me.txtRenormalizedDataColumnName.Text)

        If Me.ckbSaveDerivedData.Checked Then
            Me.DataRenormalizer.SaveDerivatedColumnToFileObject(Me.txtDerivedDataColumnName.Text)
        End If

        If Me.ckbSaveSmoothedData.Checked Then
            Me.DataRenormalizer.SaveSmoothedColumnToFileObject(Me.txtSmoothedDataColumnName.Text)
        End If
    End Sub
#End Region

#Region "Column-Selection Synchronization in Source and Target Window"
    ''' <summary>
    ''' Set the X Column of the Source-Data to the Target-Preview-Window
    ''' </summary>
    Private Sub SourceSelectedColumnChanged() Handles pbSourceData.SelectedIndexChanged
        Me.pbTargetData.cbX.SelectedColumnName = Me.pbSourceData.cbX.SelectedColumnName
    End Sub
#End Region

#Region "ColumnName-Textbox Enabling/Disabling"
    ''' <summary>
    ''' Change the ReadOnly Attribute of the ColumnName Textboxes depending on
    ''' the Check-State of the CheckBoxes.
    ''' </summary>
    Private Sub ckbColumnNames_CheckedChanged(sender As System.Object, e As System.EventArgs) _
        Handles ckbSaveSmoothedData.CheckedChanged, ckbSaveDerivedData.CheckedChanged
        Me.txtDerivedDataColumnName.Enabled = Me.ckbSaveDerivedData.Checked
        Me.txtSmoothedDataColumnName.Enabled = Me.ckbSaveSmoothedData.Checked
    End Sub
#End Region

#Region "Window Closing Procedures"
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
            .LastRenormalization_DerivedColumnName = Me.txtDerivedDataColumnName.Text
            .LastRenormalization_RenormColumnName = Me.txtRenormalizedDataColumnName.Text
            .LastRenormalization_SaveDerived = Me.ckbSaveDerivedData.Checked
            .LastRenormalization_SaveSmoothed = Me.ckbSaveSmoothedData.Checked
            .LastRenormalization_SmoothedColumnName = Me.txtSmoothedDataColumnName.Text
            .LastRenormalization_SmoothMethod = Convert.ToInt32(Me.dsDataSmoothing.SelectedSmoothingMethod)
            .LastRenormalization_SmoothNeighbors = Me.dsDataSmoothing.SmoothingParameter
            .LastRenormalization_SourceColumnX = Me.pbSourceData.cbX.SelectedColumnName
            .LastRenormalization_SourceColumnY = Me.pbSourceData.cbY.SelectedColumnName
            .LastRenormalization_TargetColumnX = Me.pbTargetData.cbX.SelectedColumnName
            .LastRenormalization_TargetColumnY = Me.pbTargetData.cbY.SelectedColumnName
            .Save()
        End With
    End Sub
#End Region

End Class