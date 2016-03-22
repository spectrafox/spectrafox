Public Class wDataRenormalization2
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for data regauging of the Selected Spectroscopy Files
    ''' using the numeric derivative.
    ''' </summary>
    Private WithEvents DataRegaugerByFit As cSpectroscopyTableDataRegaugeByNumericDerivative

    ''' <summary>
    ''' Object for data regauging of the selected Spectroscopy Files
    ''' using entered parameters
    ''' </summary>
    Private WithEvents DataRegaugerByParameters As cSpectroscopyTableDataRegaugeByFitParameters

    ''' <summary>
    ''' Determines for the save-button which output to use.
    ''' </summary>
    Private LastRegaugeByFit As Boolean = False
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
        Me.DataRegaugerByFit = New cSpectroscopyTableDataRegaugeByNumericDerivative(Me._FileObject)
        ' Load the Spectroscopy-Table-File using Background-Class.
        Me.DataRegaugerByFit.CurrentSpectroscopyTable = SpectroscopyTable

        ' Create new DataRegaugeByParameter Object
        Me.DataRegaugerByParameters = New cSpectroscopyTableDataRegaugeByFitParameters(Me._FileObject)
        ' Load the Spectroscopy-Table-File using Background-Class.
        Me.DataRegaugerByParameters.CurrentSpectroscopyTable = SpectroscopyTable

        ' Load Properties From Settings if possible!
        With My.Settings
            Me.dsDataSmoothing.SelectedSmoothingMethodType = cNumericalMethods.GetSmoothingMethodFromString(.LastRenormalization_SmoothMethod)
            Me.dsDataSmoothing.SetSmoothingSettings(.LastRenormalization_SmoothOptions)
            Me.ckbSaveDerivedData.Checked = .LastRenormalization_SaveDerived
            Me.ckbSaveSmoothedData.Checked = .LastRenormalization_SaveSmoothed
            Me.txtDerivedDataColumnName.Text = .LastRenormalization_DerivedColumnName
            Me.txtSmoothedDataColumnName.Text = .LastRenormalization_SmoothedColumnName
            Me.txtRenormalizedDataColumnName.Text = .LastRenormalization_RenormColumnName
            Me.txtRegaugeRange_xMin.SetValue(.LastRenormalization_Boundaries_xMin)
            Me.txtRegaugeRange_xMax.SetValue(.LastRenormalization_Boundaries_xMax)
            Me.txtFittedParameter_y0.SetValue(.LastRenormalization_Parameter_y0, , False)
            Me.txtFittedParameter_m.SetValue(.LastRenormalization_Parameter_m, , False)

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
    ''' Runs the renormalization using the fit-procedure and saves the data in a new spectroscopytable.
    ''' </summary>
    Public Sub ApplyFitRenormalizaton() Handles btnRegauge.Click
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

        ' Deactivate the interface
        Me.SetSaveButton(False)

        ' Send renormalization command to background-class
        Me.DataRegaugerByFit.RenormalizeColumnWITHDerivation_Async(Me.pbSourceData.cbX.SelectedColumnName,
                                                                  Me.pbSourceData.cbY.SelectedColumnName,
                                                                  Me.dsDataSmoothing.GetSmoothingMethod,
                                                                  Me.pbTargetData.cbX.SelectedColumnName,
                                                                  Me.pbTargetData.cbY.SelectedColumnName,
                                                                  Me.gbRegaugeRange.Checked,
                                                                  Me.txtRegaugeRange_xMin.DecimalValue,
                                                                  Me.txtRegaugeRange_xMax.DecimalValue,
                                                                  Me.txtRenormalizedDataColumnName.Text)
    End Sub

    ''' <summary>
    ''' Runs the regauge using the entered parameters and saves the data in a new SpectroscopyTable.
    ''' </summary>
    Public Sub ApplyParameterRenormalizaton() Handles txtFittedParameter_y0.ValidValueChanged, txtFittedParameter_m.ValidValueChanged
        ' Security-Checks, if all Columns are selected.
        If Me.pbTargetData.cbX.SelectedColumnName = String.Empty Or
           Me.pbTargetData.cbY.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Deactivate the interface
        Me.SetSaveButton(False)

        ' Send regauge command to background-class
        Me.DataRegaugerByParameters.RenormalizeColumnByParameters_Async(Me.pbTargetData.cbY.SelectedColumnName,
                                                                        Me.txtFittedParameter_y0.DecimalValue,
                                                                        Me.txtFittedParameter_m.DecimalValue,
                                                                        Me.txtRenormalizedDataColumnName.Text)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the regauge procedure that uses derivative-fitting is complete
    ''' </summary>
    Private Sub RegaugeByFittingFinished(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RenormalizedColumn As cSpectroscopyTable.DataColumn) Handles DataRegaugerByFit.FileRenormalizedComplete

        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataRegaugerByFit.CurrentSpectroscopyTable.Column(Me.pbTargetData.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(RenormalizedColumn)
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataRegaugerByFit.SmoothedColumn)
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataRegaugerByFit.DerivatedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbOutput.SetSinglePreviewImage(OutputSpectroscopyTable, Me.pbTargetData.cbX.SelectedColumnName, RenormalizedColumn.Name)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbOutput.cbX.SelectedColumnName = Me.pbTargetData.cbX.SelectedColumnName
        Me.pbOutput.cbY.SelectedColumnName = RenormalizedColumn.Name

        ' Write fit-result to output window.
        Me.txtFittedParameter_y0.SetValue(Me.DataRegaugerByFit.Parameter_y0, , False)
        Me.txtFittedParameter_m.SetValue(Me.DataRegaugerByFit.Parameter_m, , False)
        Me.SetFittedParameterFitIndicator(True)

        ' save the fitted parameters in the settings to be able to use them
        ' in the back-ground renormalization task.
        With My.Settings
            .LastRenormalization_Parameter_m = Me.DataRegaugerByFit.Parameter_m
            .LastRenormalization_Parameter_y0 = Me.DataRegaugerByFit.Parameter_y0
            .Save()
        End With

        Me.LastRegaugeByFit = True

        ' Activate Save-Button
        Me.SetSaveButton(True)
    End Sub

    ''' <summary>
    ''' Function that gets called, when the regauge-procedure that uses the given parameters has finished.
    ''' </summary>
    Private Sub RegaugeByParametersFinished(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RenormalizedColumn As cSpectroscopyTable.DataColumn) Handles DataRegaugerByParameters.FileRegaugeComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataRegaugerByParameters.CurrentSpectroscopyTable.Column(Me.pbTargetData.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(RenormalizedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbOutput.SetSinglePreviewImage(OutputSpectroscopyTable)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbOutput.cbX.SelectedColumnName = Me.pbTargetData.cbX.SelectedColumnName
        Me.pbOutput.cbY.SelectedColumnName = RenormalizedColumn.Name

        Me.LastRegaugeByFit = False

        ' Activate Save-Button
        Me.SetSaveButton(True)
    End Sub

    ''' <summary>
    ''' Called, if the valid value of the fitted parameters have been changed.
    ''' To manually adapt the re-gauged output.
    ''' </summary>
    Private Sub txtFittedParameter_ValidValueChanged(ByRef T As NumericTextbox) Handles txtFittedParameter_m.ValidValueChanged, txtFittedParameter_y0.ValidValueChanged
        Me.SetFittedParameterFitIndicator(False)
        Me.ApplyParameterRenormalizaton()
        ' save the fitted parameters in the settings to be able to use them
        ' in the back-ground renormalization task.
        With My.Settings
            .LastRenormalization_Parameter_m = Me.txtFittedParameter_m.DecimalValue
            .LastRenormalization_Parameter_y0 = Me.txtFittedParameter_y0.DecimalValue
            .Save()
        End With
    End Sub

    ''' <summary>
    ''' Delegate for a thread-safe-call.
    ''' </summary>
    Protected Delegate Sub _SetFittedParameterFitIndicator(ByVal IsFitted As Boolean)

    ''' <summary>
    ''' Set the warning of the fitted parameters.
    ''' </summary>
    Protected Sub SetFittedParameterFitIndicator(ByVal IsFitted As Boolean)
        If Me.lblNotFittedWarning.InvokeRequired Then
            Dim d As New _SetFittedParameterFitIndicator(AddressOf SetFittedParameterFitIndicator)
            Me.lblNotFittedWarning.Invoke(d, IsFitted)
        Else
            If IsFitted Then
                Me.lblNotFittedWarning.Text = My.Resources.rDataRenormalization.NotFittedWarning_Fitted
                Me.lblNotFittedWarning.ForeColor = Color.Green
            Else
                Me.lblNotFittedWarning.Text = My.Resources.rDataRenormalization.NotFittedWarning_NotFitted
                Me.lblNotFittedWarning.ForeColor = Color.Red
            End If
        End If
    End Sub

#Region "Column Saving"
    ' This will be called from the worker thread to activate or deactivate the ColumnSave buttons
    Delegate Sub SetSaveButtonCallback(ByVal Enabled As Boolean)
    Friend Sub SetSaveButton(ByVal Enabled As Boolean)
        If Me.btnSaveColumns.InvokeRequired Then
            Dim _delegate As New SetSaveButtonCallback(AddressOf SetSaveButton)
            Me.Invoke(_delegate, Enabled)
        Else
            ' Disable the button, until the renormalization is finished.
            Me.btnRegauge.Enabled = Enabled
            Me.txtFittedParameter_m.Enabled = Enabled
            Me.txtFittedParameter_y0.Enabled = Enabled
            Me.btnSaveColumns.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the Columns Gained to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumns_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumns.Click

        If LastRegaugeByFit Then
            ' SAVE FITTED COLUMNS
            If Me.DataRegaugerByFit.RenormalizedColumn Is Nothing Or
               Me.DataRegaugerByFit.DerivatedColumn Is Nothing Or
               Me.DataRegaugerByFit.SmoothedColumn Is Nothing Then Return

            ' Save Columns
            Me.DataRegaugerByFit.SaveRenormalizedColumnToFileObject(Me.txtRenormalizedDataColumnName.Text)

            If Me.ckbSaveDerivedData.Checked Then
                Me.DataRegaugerByFit.SaveDerivatedColumnToFileObject(Me.txtDerivedDataColumnName.Text)
            End If

            If Me.ckbSaveSmoothedData.Checked Then
                Me.DataRegaugerByFit.SaveSmoothedColumnToFileObject(Me.txtSmoothedDataColumnName.Text)
            End If
        Else
            ' SAVE PARAMETER COLUMNS
            If Me.DataRegaugerByParameters.RegaugedColumn Is Nothing Then Return

            ' Save Columns
            Me.DataRegaugerByParameters.SaveRegaugedColumnToFileObject(Me.txtRenormalizedDataColumnName.Text)
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
            .LastRenormalization_SmoothMethod = Me.dsDataSmoothing.SelectedSmoothingMethodType.ToString
            .LastRenormalization_SmoothOptions = Me.dsDataSmoothing.GetSmoothingSettings
            .LastRenormalization_SourceColumnX = Me.pbSourceData.cbX.SelectedColumnName
            .LastRenormalization_SourceColumnY = Me.pbSourceData.cbY.SelectedColumnName
            .LastRenormalization_TargetColumnX = Me.pbTargetData.cbX.SelectedColumnName
            .LastRenormalization_TargetColumnY = Me.pbTargetData.cbY.SelectedColumnName
            .Save()
        End With
    End Sub
#End Region

#Region "Limit the regauging to a certain range."
    ''' <summary>
    ''' Activate the range-selection, if we just want to regauge to a certain range.
    ''' </summary>
    Private Sub gbRegaugeRange_CheckChanged() Handles gbRegaugeRange.CheckChanged

        If Me.gbRegaugeRange.Checked Then
            ' Activate the range-selection
            Me.pbSourceData.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XRange
        Else
            ' deactivate the range-selection
            Me.pbSourceData.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.None
        End If

    End Sub

    ''' <summary>
    ''' Save the selected X-Range.
    ''' </summary>
    Private Sub SetRegaugeRangeFromSelectionMode(LeftValue As Double, RightValue As Double) Handles pbSourceData.PointSelectionChanged_XRange
        Me.txtRegaugeRange_xMin.SetValue(LeftValue)
        Me.txtRegaugeRange_xMax.SetValue(RightValue)
        With My.Settings
            .LastRenormalization_Boundaries_xMin = LeftValue
            .LastRenormalization_Boundaries_xMax = RightValue
            .Save()
        End With
    End Sub
#End Region

End Class