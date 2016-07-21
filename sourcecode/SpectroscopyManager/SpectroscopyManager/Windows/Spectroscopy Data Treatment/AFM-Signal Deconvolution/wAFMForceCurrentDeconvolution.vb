''' <summary>
''' In this window we perform and setup the force and current deconvolution routine
''' for frequency-modulated AFM signals.
''' </summary>
Public Class wAFMForceCurrentDeconvolution
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"

    Private bReady As Boolean = False

    ''' <summary>
    ''' Object of the force current deconvolution tool.
    ''' </summary>
    Private WithEvents AFMDeconvolver As cSpectroscopyTableAFMForceCurrentDeconvolution

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

#Region "Sub called after fetching the SpectroscopyTable"

    ''' <summary>
    ''' Set the Spectroscopy-File to the display.
    ''' </summary>
    Public Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Create new deconvolution object.
        Me.AFMDeconvolver = New cSpectroscopyTableAFMForceCurrentDeconvolution(Me._FileObject)
        Me.AFMDeconvolver.CurrentSpectroscopyTable = SpectroscopyTable

        ' Set Preview-Images:
        Me.pbBefore.SetSinglePreviewImage(SpectroscopyTable)
        
        ' Set column selectors to the current data list.
        Me.csColumn_ZRel.InitializeColumns(SpectroscopyTable.GetColumnList)
        Me.csColumn_ForceGradient.InitializeColumns(SpectroscopyTable.GetColumnList)
        Me.csColumn_CurrentSignal.InitializeColumns(SpectroscopyTable.GetColumnList)

        ' Load properties from software's settings if possible!
        With My.Settings
            Me.txtAFMSetupOscAmp.SetValue(.AFMDeconvolution_OscillationAmplitude)
            Me.txtAFMSetupResFreq.SetValue(.AFMDeconvolution_ResonanceFrequency)
            Me.txtAFMSetupSpringConst.SetValue(.AFMDeconvolution_SpringConstant)

            Me.ckbSettings_RemoveFrequencyShiftOffset.Checked = .AFMDeconvolution_RemoveFrequencyShiftOffset

            Me.txtOutputName_Current.Text = .AFMDeconvolution_OutputName_Current
            Me.txtOutputName_Force.Text = .AFMDeconvolution_OutputName_Force

            Me.csColumn_ZRel.SelectedColumnName = .AFMDeconvolution_LastColumn_ZRel
            Me.csColumn_ForceGradient.SelectedColumnName = .AFMDeconvolution_LastColumn_ForceGradient
            Me.csColumn_CurrentSignal.SelectedColumnName = .AFMDeconvolution_LastColumn_CurrentSignal

            Me.pbBefore.cbX.SelectedColumnName = .AFMDeconvolution_LastColumn_ZRel
            Me.pbBefore.cbY.SelectedColumnName = .AFMDeconvolution_LastColumn_ForceGradient

            Me.dsDataSmoother.SelectedSmoothingMethodType = cNumericalMethods.GetSmoothingMethodFromString(.AFMDeconvolution_LastSmoothMethod)
            Me.dsDataSmoother.SetSmoothingSettings(.AFMDeconvolution_LastSmoothSettings)

        End With

        Me.bReady = True
    End Sub

#End Region

#Region "Synchronize the Columns"

    ''' <summary>
    ''' Set the X column of the before-data to the after PreviewBox
    ''' </summary>
    Private Sub BeforeSelectedColumnChanged() Handles pbBefore.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.bReady = False
        Me.csColumn_ZRel.SelectedColumnName = Me.pbBefore.cbX.SelectedColumnName
        Me.bReady = True
    End Sub

#End Region

#Region "Start the deconvolution"

    ''' <summary>
    ''' Click on the start button.
    ''' </summary>
    Private Sub btnDoForceDeconvolution_Click(sender As Object, e As EventArgs) Handles btnDoForceDeconvolution.Click

        ' Security-Checks, if all Columns are selected.
        If Me.csColumn_ZRel.SelectedColumnName = String.Empty OrElse Me.csColumn_ForceGradient.SelectedColumnName = String.Empty OrElse Me.csColumn_CurrentSignal.SelectedColumnName = String.Empty Then
            MessageBox.Show(My.Resources.Message_PleaseSelectDataColumn,
                            My.Resources.Title_SelectionNecessary,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return
        End If

        ' Deactivate the interface.
        Me.SetSaveButton(False)

        ' Set the settings of the deconvolution procedure:
        Me.AFMDeconvolver.OscillationAmplitude = Me.txtAFMSetupOscAmp.DecimalValue
        Me.AFMDeconvolver.ResonanceFrequency = Me.txtAFMSetupResFreq.DecimalValue
        Me.AFMDeconvolver.SpringConstant = Me.txtAFMSetupSpringConst.DecimalValue
        Me.AFMDeconvolver.RemoveFrequencyShiftOffsetToZero = Me.ckbSettings_RemoveFrequencyShiftOffset.Checked
        Me.AFMDeconvolver.DataSmoother = Me.dsDataSmoother.GetSmoothingMethod
        Me.AFMDeconvolver.OutputColumnName_Current = Me.txtOutputName_Current.Text
        Me.AFMDeconvolver.OutputColumnName_Force = Me.txtOutputName_Force.Text

        ' Start the ASync deconvolution procedurce in the background object.
        Me.AFMDeconvolver.DeconvoluteWITHFetch_Async(Me.csColumn_ZRel.SelectedColumnName,
                                                     Me.csColumn_ForceGradient.SelectedColumnName,
                                                     Me.csColumn_CurrentSignal.SelectedColumnName,
                                                     "Force", "Current")

    End Sub

#End Region

#Region "Deconvolution procedure"

    ''' <summary>
    ''' Function that gets called, when the deconvolution procedure is complete
    ''' </summary>
    Private Sub DeconvolutionFinished(ByRef SpectroscopyTable As cSpectroscopyTable,
                                      ByRef ForceColumn As cSpectroscopyTable.DataColumn,
                                      ByRef CurrentColumn As cSpectroscopyTable.DataColumn) Handles AFMDeconvolver.CurrentForceDeconvolutionComplete

        ' Activate Save-Button
        Me.SetSaveButton(True)

        If SpectroscopyTable Is Nothing Then Return
        If ForceColumn Is Nothing Then Return
        If CurrentColumn Is Nothing Then Return

        ' Set the output SpectroscopyTable to the preview box.
        Me.pbAfter.SetSinglePreviewImage(SpectroscopyTable,
                                         Me.pbBefore.cbX.SelectedColumnName,
                                         ForceColumn.Name,
                                         False)

        ' Set Out-Preview-Box Columns to TargetBox-Columns
        Me.pbAfter.cbX.SelectedColumnName = Me.pbBefore.cbX.SelectedColumnName
        Me.pbAfter.cbY.SelectedColumnName = ForceColumn.Name

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
            Me.btnDoForceDeconvolution.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Saves the Column Gained from the multiplication to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSaveColumn_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumn.Click
        If Me.AFMDeconvolver.DeconvolutedCurrent Is Nothing Then Return
        If Me.AFMDeconvolver.DeconvolutedForce Is Nothing Then Return

        ' Save the data columns in the file-object.
        Me.AFMDeconvolver.SaveColumnsToFileObject(Me.txtOutputName_Force.Text, Me.txtOutputName_Current.Text)
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

        ' Check, if the deconvolution is in progress. Then abort.
        If Me.AFMDeconvolver.DeconvolutionInProgress Then
            MessageBox.Show(My.Resources.rAFMForceCurrentDeconvolution.WindowClosing_StillBusy,
                            My.Resources.rAFMForceCurrentDeconvolution.WindowClosing_StillBusy_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Cancel = True
            Return
        End If


        ' Saves the Chosen Parameters to the Settings.
        With My.Settings

            .AFMDeconvolution_OscillationAmplitude = Me.txtAFMSetupOscAmp.DecimalValue
            .AFMDeconvolution_ResonanceFrequency = Me.txtAFMSetupResFreq.DecimalValue
            .AFMDeconvolution_SpringConstant = Me.txtAFMSetupSpringConst.DecimalValue
            .AFMDeconvolution_RemoveFrequencyShiftOffset = Me.ckbSettings_RemoveFrequencyShiftOffset.Checked

            .AFMDeconvolution_LastColumn_ZRel = Me.csColumn_ZRel.SelectedColumnName
            .AFMDeconvolution_LastColumn_ForceGradient = Me.csColumn_ForceGradient.SelectedColumnName
            .AFMDeconvolution_LastColumn_CurrentSignal = Me.csColumn_CurrentSignal.SelectedColumnName

            .AFMDeconvolution_OutputName_Current = Me.txtOutputName_Current.Text
            .AFMDeconvolution_OutputName_Force = Me.txtOutputName_Force.Text

            .AFMDeconvolution_LastSmoothMethod = Me.dsDataSmoother.SelectedSmoothingMethodType.ToString
            .AFMDeconvolution_LastSmoothSettings = Me.dsDataSmoother.GetSmoothingSettings

            .Save()
        End With
    End Sub

#End Region

#Region "Change Settings of the Deconvolution Procedure"

    ''' <summary>
    ''' Settings of regarding the AFM changed.
    ''' </summary>
    Private Sub txtAFMSettings_TextChanged(ByRef NT As NumericTextbox) Handles txtAFMSetupResFreq.ValidValueChanged, txtAFMSetupOscAmp.ValidValueChanged, txtAFMSetupSpringConst.ValidValueChanged

        With My.Settings
            .AFMDeconvolution_OscillationAmplitude = Me.txtAFMSetupOscAmp.DecimalValue
            .AFMDeconvolution_ResonanceFrequency = Me.txtAFMSetupResFreq.DecimalValue
            .AFMDeconvolution_SpringConstant = Me.txtAFMSetupSpringConst.DecimalValue
        End With

    End Sub

#End Region


End Class