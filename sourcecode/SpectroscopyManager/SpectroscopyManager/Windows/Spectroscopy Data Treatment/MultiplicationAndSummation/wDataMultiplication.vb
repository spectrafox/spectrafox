Public Class wDataMultiplication
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"
    ''' <summary>
    ''' Object for Data Multiplication of the Selected Spectroscopy Files
    ''' </summary>
    Private WithEvents DataMultiplier As cSpectroscopyTableDataMultiplier

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

        ' Create new DataMultiplier Object
        Me.DataMultiplier = New cSpectroscopyTableDataMultiplier(Me._FileObject)
        Me.DataMultiplier.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set Preview-Images:
        Me.pbBeforeMultiplication.SetSinglePreviewImage(SpectroscopyTable)
        Me.pbAfterMultiplication.SetSinglePreviewImage(SpectroscopyTable)

        ' Set Columns
        Me.csColumnToMultiply.InitializeColumns(SpectroscopyTable.GetColumnList)
        Me.csColumnToMultiplyWith.InitializeColumns(SpectroscopyTable.GetColumnList)

        ' Load Properties from Settings if possible!
        With My.Settings
            Me.pbBeforeMultiplication.cbX.SelectedColumnName = .LastDataMultiplication_ColumnX
            Me.pbBeforeMultiplication.cbY.SelectedColumnName = .LastDataMultiplication_ColumnToMultiply
            Me.csColumnToMultiplyWith.SelectedColumnName = .LastDataMultiplication_ColumnToBeMultipliedWith
            Me.txtMuliplicationFactor.SetValue(.LastDataMultiplication_Factor)

            Select Case .LastDataMultiplication_MultiplicationMethod
                Case cSpectroscopyTableDataMultiplier.MultiplicationMode.Factor
                    Me.tcMultiplicationMethod.SelectedTab = Me.tpFactorMultiplication
                Case cSpectroscopyTableDataMultiplier.MultiplicationMode.OtherColumnMultiply
                    Me.rbMultiply.Checked = True
                    Me.tcMultiplicationMethod.SelectedTab = Me.tpOtherColumnMultiplication
                Case cSpectroscopyTableDataMultiplier.MultiplicationMode.OtherColumnDivide
                    Me.rbDivide.Checked = True
                    Me.tcMultiplicationMethod.SelectedTab = Me.tpOtherColumnMultiplication
            End Select
        End With
    End Sub

#End Region

#Region "Synchronize the Columns"
    ''' <summary>
    ''' Set the X Column of the Before-Data to the After-PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbBeforeMultiplication.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterMultiplication.cbX.SelectedColumnName = Me.pbBeforeMultiplication.cbX.SelectedColumnName
        Me.csColumnToMultiply.SelectedColumnName = Me.pbBeforeMultiplication.cbY.SelectedColumnName
        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Set the Y Column of the Before- and After-PreviewBox to the selected multiplication Column
    ''' </summary>
    Private Sub SelectedMultiplicationColumnChanged() Handles csColumnToMultiply.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.pbAfterMultiplication.cbY.SelectedColumnName = Me.csColumnToMultiply.SelectedColumnName
        Me.pbBeforeMultiplication.cbY.SelectedColumnName = Me.csColumnToMultiply.SelectedColumnName
        Me.bReady = True

        ' Set ColumnTitle to Smoothed Selected ColumnTitle
        'Me.txtNewColumnName.Text = My.Resources.ColumnTemplate_Smoothed.Replace("%%", Me.oSpectroscopyTable.Column(Me.csColumnToSmooth.SelectedColumnIndex).Name)
    End Sub
#End Region

    ''' <summary>
    ''' Applies the selected multiplication to the selected Column
    ''' </summary>
    Public Sub btnApplyMultiplication_Click(sender As System.Object, e As System.EventArgs) Handles btnApplyMultiplication.Click
        ' Security-Checks, if all Columns are selected.
        If Me.csColumnToMultiply.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        If Me.csColumnToMultiplyWith.SelectedColumnName = String.Empty And Me.tcMultiplicationMethod.SelectedTab.Name = Me.tpOtherColumnMultiplication.Name Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Send multiplication command to Background-Class
        Select Case Me.tcMultiplicationMethod.SelectedTab.Name
            Case Me.tpFactorMultiplication.Name
                ' Multiply by fixed factor
                Me.DataMultiplier.MultiplyColumnWithoutFetching_ASync(Me.csColumnToMultiply.SelectedColumnName,
                                                                      Me.txtMuliplicationFactor.DecimalValue,
                                                                      Me.txtNewColumnName.Text)
            Case Me.tpOtherColumnMultiplication.Name
                Dim MultiplicationMode As cSpectroscopyTableDataMultiplier.MultiplicationMode
                If Me.rbMultiply.Checked Then
                    MultiplicationMode = cSpectroscopyTableDataMultiplier.MultiplicationMode.OtherColumnMultiply
                Else
                    MultiplicationMode = cSpectroscopyTableDataMultiplier.MultiplicationMode.OtherColumnDivide
                End If

                ' Multiply other column
                Me.DataMultiplier.MultiplyColumnWithoutFetching_ASync(Me.csColumnToMultiply.SelectedColumnName,
                                                                      MultiplicationMode,
                                                                      Me.csColumnToMultiplyWith.SelectedColumnName,
                                                                      Me.txtNewColumnName.Text)
        End Select
    End Sub

    ''' <summary>
    ''' Function that gets called, when the multiplication procedure is complete
    ''' </summary>
    Private Sub MultiplicationFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                       ByRef MultipliedColumn As cSpectroscopyTable.DataColumn) Handles DataMultiplier.FileMultiplicationComplete
        ' Copy SpectroscopyTable to New SpectroscopyTable
        Dim OutputSpectroscopyTable As cSpectroscopyTable = New cSpectroscopyTable
        ' Add XColumn
        OutputSpectroscopyTable.AddNonPersistentColumn(Me.DataMultiplier.CurrentSpectroscopyTable.Column(Me.pbBeforeMultiplication.cbX.SelectedColumnName))
        OutputSpectroscopyTable.AddNonPersistentColumn(MultipliedColumn)

        ' Set Output Spectroscopy Table to Preview-Box.
        Me.pbAfterMultiplication.SetSinglePreviewImage(OutputSpectroscopyTable,
                                                       Me.pbBeforeMultiplication.cbX.SelectedColumnName,
                                                       MultipliedColumn.Name)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfterMultiplication.cbX.SelectedColumnName = Me.pbBeforeMultiplication.cbX.SelectedColumnName
        Me.pbAfterMultiplication.cbY.SelectedColumnName = MultipliedColumn.Name

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
        If Me.DataMultiplier.MultipliedColumn Is Nothing Then Return

        ' Save Columns
        Me.DataMultiplier.SaveMultipliedColumnToFileObject(Me.txtNewColumnName.Text)
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
            Dim MultiplicationMode As cSpectroscopyTableDataMultiplier.MultiplicationMode
            Select Case Me.tcMultiplicationMethod.SelectedTab.Name
                Case Me.tpFactorMultiplication.Name
                    MultiplicationMode = cSpectroscopyTableDataMultiplier.MultiplicationMode.Factor
                Case Me.tpOtherColumnMultiplication.Name
                    If Me.rbMultiply.Checked Then
                        MultiplicationMode = cSpectroscopyTableDataMultiplier.MultiplicationMode.OtherColumnMultiply
                    Else
                        MultiplicationMode = cSpectroscopyTableDataMultiplier.MultiplicationMode.OtherColumnDivide
                    End If
            End Select

            .LastDataMultiplication_Factor = Me.txtMuliplicationFactor.DecimalValue
            .LastDataMultiplication_ColumnToBeMultipliedWith = Me.csColumnToMultiplyWith.SelectedColumnName
            .LastDataMultiplication_MultiplicationMethod = MultiplicationMode
            .LastDataMultiplication_ColumnX = Me.pbBeforeMultiplication.cbX.SelectedColumnName
            .LastDataMultiplication_ColumnToMultiply = Me.pbBeforeMultiplication.cbY.SelectedColumnName
            .LastDataMultiplication_NewColumnName = Me.txtNewColumnName.Text
            .Save()
        End With
    End Sub
#End Region

End Class