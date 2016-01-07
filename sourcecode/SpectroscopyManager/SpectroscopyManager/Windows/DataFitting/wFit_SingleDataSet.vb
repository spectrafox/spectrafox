Imports System.ComponentModel
Imports System.Text.RegularExpressions

''' <summary>
''' Base class for a Data-Fitting window.
''' </summary>
Public Class wFit_SingleDataSet
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad
    Implements iFitWindow

    Private bReady As Boolean = False

    ''' <summary>
    ''' Event that gets raised, if the fit finished.
    ''' </summary>
    Public Event FitFinishedEvent() Implements iFitWindow.FitFinishedEvent

#Region "Spectroscopy-Table Object, Interface Function and Column-Selection"
    ''' <summary>
    ''' Sets the used Spectroscopy-Table and enables the Interface.
    ''' </summary>
    Public Sub SetSpectroscopyTable(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall
        Me.bReady = True

        ' Add the Spectroscopy-Table to the preview-display.
        Me.pbPreview.AddSpectroscopyTable(Me.CurrentSpectroscopyTable)
        'Me.SpectroscopyColumnSelectionChangedX()
        'Me.SpectroscopyColumnSelectionChangedY()

        ' Initialize the column-selection
        Me.cbX.InitializeColumns(Me.CurrentSpectroscopyTable.GetColumnList, My.Settings.Fit_SingleFile_LastCol_X)
        Me.cbY.InitializeColumns(Me.CurrentSpectroscopyTable.GetColumnList, My.Settings.Fit_SingleFile_LastCol_Y)

        ' Change the title of the window.
        Me.Text &= Me.CurrentSpectroscopyTable.FileNameWithoutPath

    End Sub

    ''' <summary>
    ''' Selected columns change
    ''' </summary>
    Public Sub SpectroscopyColumnSelectionChangedX() Handles cbX.SelectedIndexChanged
        If Not Me.bReady Then Return

        Me.pbPreview.cbX.SelectedEntry = Me.cbX.SelectedColumnName
        Me.CalculatePreviewImage()

        ' Save to settings for the next time the window opens
        My.Settings.Fit_SingleFile_LastCol_X = Me.cbX.SelectedColumnName
    End Sub

    ''' <summary>
    ''' Selected columns change
    ''' </summary>
    Public Sub SpectroscopyColumnSelectionChangedY() Handles cbY.SelectedIndexChanged
        If Not Me.bReady Then Return

        Me.CalculatePreviewImage()
        Me.pbPreview.cbY.SelectedEntry = Me.cbY.SelectedColumnName

        ' Save to settings for the next time the window opens
        My.Settings.Fit_SingleFile_LastCol_Y = Me.cbY.SelectedColumnName
    End Sub
#End Region

#Region "Fit-Function"

    ''' <summary>
    ''' Fit-Function Container
    ''' </summary>
    Private FitFunction As iFitFunction

    ''' <summary>
    ''' Different Fit-Functions.
    ''' </summary>
    Private FitFunctionSettings As New List(Of cFitSettingPanel)

    ''' <summary>
    ''' List containing all fit-functions usable in the fit-window.
    ''' Registrations are done in the window constructor!
    ''' The Boolean determines, if the function should be shown in the selection,
    ''' or if it is just available by importing. (In #debug all functions will be shown!)
    ''' </summary>
    Private RegisteredFitFunctions As New Dictionary(Of Type, Boolean)

    ''' <summary>
    ''' Registration of all Fit-Functions
    ''' </summary>
    Private Sub RegisterFitFunctions()

        ' Get all fit-functions implemented in the program or a plugin-dll
        Dim APIList As List(Of Type) = cFitFunction.GetAllLoadableFitFunctions()

        With Me.RegisteredFitFunctions
            For Each FFType As Type In APIList
                .Add(FFType, True)
            Next

            ' Exclude hidden functions implemented by the program.
            .Item(GetType(cFitFunction_MultiplePeaks)) = False

        End With
    End Sub

    ''' <summary>
    ''' Returns a cFitSettingPanel, depending on the identifier, or Nothing, if the
    ''' Function was not recognized.
    ''' </summary>
    Private Function GetFitSettingsPanelByType(FitFunctionType As Type) As cFitSettingPanel

        Dim FF As iFitFunction = cFitFunction.GetFitFunctionByType(FitFunctionType)
        If Not FF Is Nothing Then
            Return FF.GetFunctionSettingPanel
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Returns the currently selected Fit-Function to add
    ''' </summary>
    Private Function GetSelectedFitFunctionToAdd() As iFitFunction
        Dim SelectedFitFunctionType As Type = DirectCast(Me.cboAddFitModel.SelectedItem, KeyValuePair(Of Type, String)).Key
        Dim FF As iFitFunction = cFitFunction.GetFitFunctionByType(SelectedFitFunctionType)
        If Not FF Is Nothing Then
            ' Set the initial multi-threading options
            FF.MultiThreadingOptions = Me.MultiThreadingSettings
            Return FF
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Fit-Procedure"

    ''' <summary>
    ''' FitProcedure
    ''' </summary>
    Private WithEvents FitProcedure As New cFitProcedureSingleDataFit

    ''' <summary>
    ''' Current set of Fit-Procedure Settings
    ''' </summary>
    Private FitProcedureSettings As iFitProcedureSettings

    ''' <summary>
    ''' List containing all fit-procedures usable in the fit-window.
    ''' Registrations are done in the window constructor!
    ''' The Boolean determines, if the procedure should be shown in the selection,
    ''' or if it is just available in debug mode.
    ''' </summary>
    Private RegisteredFitProcedures As New Dictionary(Of Type, Boolean)

    ''' <summary>
    ''' Registration of all Fit-Procedures
    ''' </summary>
    Private Sub RegisterFitProcedures()
        With Me.RegisteredFitProcedures
            .Add(GetType(cLMAFit), True)
            '.Add(GetType(cLMAFit_Advanced), False)
            .Add(GetType(cNMAFit), True)
        End With
    End Sub

    ''' <summary>
    ''' Returns a cFitProcedureSettingsPanel, depending on the identifier, or Nothing, if the
    ''' Fit-Procedure was not recognized.
    ''' </summary>
    Private Function GetFitProcedureSettingsPanelByType(FitProcedureType As Type) As cFitProcedureSettingsPanel

        Dim FP As iFitProcedure = Me.GetFitProcedureByType(FitProcedureType)
        If Not FP Is Nothing Then
            Return FP.ProcedureSettingPanel
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Returns a cFitProcedure, depending on the identifier, or Nothing, if the
    ''' Fit-Procedure was not recognized.
    ''' </summary>
    Private Function GetFitProcedureByType(FitProcedureType As Type) As iFitProcedure
        ' go through all registered fit-procedures, and look for the
        ' right one to load 
        For Each RegisteredType As Type In Me.RegisteredFitProcedures.Keys
            If RegisteredType Is FitProcedureType Then
                Return CType(Activator.CreateInstance(FitProcedureType), iFitProcedure)
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns the currently selected Fit-Procedure to add
    ''' </summary>
    Private Function GetSelectedFitProcedure() As iFitProcedure
        Dim SelectedFitProcedureType As Type = DirectCast(Me.cboFitProcedure.SelectedItem, KeyValuePair(Of Type, String)).Key
        Dim FP As iFitProcedure = Me.GetFitProcedureByType(SelectedFitProcedureType)
        If Not FP Is Nothing Then
            Return FP
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Window-Initialization"

    ''' <summary>
    ''' Window Initialization
    ''' </summary>
    Protected Sub wFitBase_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ' Initialize Buttons
        Me.SetButtonsToFitModus(False)

        ' Register all fit-functions
        Me.RegisterFitFunctions()

        ' Register Fit-Procedures
        Me.RegisterFitProcedures()

        ' Fill the Combobox with the fit-procedures
        With Me.cboFitProcedure
            .Items.Clear()
            ' Add all Fit-Procedure to the selection
            For Each RegisteredFitProcedureTypeKV As KeyValuePair(Of Type, Boolean) In Me.RegisteredFitProcedures
#If Not Debug Then
                If RegisteredFitProcedureTypeKV.Value Then
#End If
                .Items.Add(New KeyValuePair(Of Type, String)(RegisteredFitProcedureTypeKV.Key,
                                                             Me.GetFitProcedureByType(RegisteredFitProcedureTypeKV.Key).Name))
#If Not Debug Then
                End If
#End If
            Next
            .ValueMember = "Key"
            .DisplayMember = "Value"
            .Sorted = True
            For i As Integer = 0 To .Items.Count - 1 Step 1
                Dim KV As KeyValuePair(Of Type, String) = DirectCast(.Items(i), KeyValuePair(Of Type, String))
                If KV.Key.Name = My.Settings.Fit_SingleFile_Procedure Then
                    .SelectedIndex = i ' Select last used fit-procedure
                End If
            Next
            If .SelectedIndex < 0 Then .SelectedIndex = 0
        End With

        ' Fit the fit-function combobox
        With Me.cboAddFitModel
            .Items.Clear()

            ' Add all Fit-Functions to the selection
            For Each RegisteredFitFunctionTypeKV As KeyValuePair(Of Type, Boolean) In Me.RegisteredFitFunctions
#If Not DEBUG Then
                If RegisteredFitFunctionTypeKV.Value Then
#End If
                .Items.Add(New KeyValuePair(Of Type, String)(RegisteredFitFunctionTypeKV.Key,
                                                             cFitFunction.GetFitFunctionByType(RegisteredFitFunctionTypeKV.Key).FitFunctionName))
#If Not DEBUG Then
                End If
#End If
            Next
            .ValueMember = "Key"
            .DisplayMember = "Value"
            .Sorted = True
            For i As Integer = 0 To .Items.Count - 1 Step 1
                Dim KV As KeyValuePair(Of Type, String) = DirectCast(.Items(i), KeyValuePair(Of Type, String))
                If KV.Key.Name = My.Settings.Fit_SingleFile_Model Then
                    .SelectedIndex = i ' Select last used fit-procedure
                End If
            Next
        End With

        ' Apply last settings.
        Me.SetFitRange(My.Settings.Fit_SingleFile_FitRange_LeftValue, My.Settings.Fit_SingleFile_FitRange_RightValue)

        ' Apply the last number of preview-points used.
        Me.PreviewPoints = My.Settings.Fit_SingleFile_PreviewPoints

        ' Set current worker properties
        Me.FitDataSaver.WorkerReportsProgress = True

        ' Set the multi-threading properties
        Me.MultiThreadingSettings.MaxDegreeOfParallelism = My.Settings.Fitting_MaxParallelization

    End Sub
#End Region

#Region "Add and remove a Fit-Function to or from the list"
    ''' <summary>
    ''' Add a fit-function to the list.
    ''' </summary>
    Private Sub btnAddFitModel_Click(sender As Object, e As EventArgs) Handles btnAddFitModel.Click
        Me.AddFitFunction(Me.GetSelectedFitFunctionToAdd)

        ' Save last used to settings
        Dim SelectedFitFunctionType As Type = DirectCast(Me.cboAddFitModel.SelectedItem, KeyValuePair(Of Type, String)).Key
        My.Settings.Fit_SingleFile_Model = SelectedFitFunctionType.Name
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Adds a Fit-Function-Settings-Panel to the list.
    ''' </summary>
    Private Function AddFitFunction(FitFunction As iFitFunction) As Boolean

        ' Add a specific SettingPanel
        Dim SettingPanel As cFitSettingPanel = FitFunction.GetFunctionSettingPanel

        ' Check, if Settingspanel got loaded
        If SettingPanel Is Nothing Then Return False

        ' Set the multi-threading options
        FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings

        ' Set the event-handlers
        AddHandler SettingPanel.RequestRemovalOfFitFunction, AddressOf Me.RemoveFitFunction
        AddHandler SettingPanel.ParameterChanged, AddressOf Me.CalculatePreviewImage
        AddHandler SettingPanel.XRangeSelectionRequest, AddressOf Me.SelectXRange
        AddHandler SettingPanel.YRangeSelectionRequest, AddressOf Me.SelectYRange
        AddHandler SettingPanel.XValueSelectionRequest, AddressOf Me.SelectXValue
        AddHandler SettingPanel.YValueSelectionRequest, AddressOf Me.SelectYValue
        AddHandler SettingPanel.SubParametersAddedOrRemoved, AddressOf Me.RebindFitParameters

        ' Check if the fit-function is ok with being initialized at the current fit-range
        Dim FitRangeLeft_SuggestedByFitFunction As Double = Me.FitRangeLeft
        Dim FitRangeRight_SuggestedByFitFunction As Double = Me.FitRangeRight
        If Not FitFunction.FitFunctionSuggestsDifferentFitRange(FitRangeLeft_SuggestedByFitFunction, FitRangeRight_SuggestedByFitFunction) Then
            Dim SuggestionResult As DialogResult = MessageBox.Show(My.Resources.rFitBase.FitFunction_InitialFitRangeCheck _
                                                                   .Replace("%l", cUnits.GetPrefix(FitRangeLeft_SuggestedByFitFunction).Value & cUnits.GetPrefix(FitRangeLeft_SuggestedByFitFunction).Key) _
                                                                   .Replace("%h", cUnits.GetPrefix(FitRangeRight_SuggestedByFitFunction).Value & cUnits.GetPrefix(FitRangeRight_SuggestedByFitFunction).Key),
                                                                   My.Resources.rFitBase.FitFunction_InitialFitRangeCheck_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If SuggestionResult = DialogResult.Yes Then
                ' change the fit-range to the suggested values
                Me.FitRangeLeft = FitRangeLeft_SuggestedByFitFunction
                Me.FitRangeRight = FitRangeRight_SuggestedByFitFunction
            End If
        End If

        ' Register the Fit-Range-Changed-Event, and set the fit-range
        AddHandler Me.FitRangeSelectionChanged, AddressOf SettingPanel.FitRangeChanged
        SettingPanel.SetInitializationFitRange(Me.FitRangeLeft, Me.FitRangeRight)

        ' Adapt the width of the settings-panel to the Flow-Layout container:
        SettingPanel.Width = Me.flpFitModels.Width - FLPWidthOffset

        ' Add the Settings-Panel to the Flow-Layout-Stack
        Me.flpFitModels.Controls.Add(SettingPanel)

        '########################################################
        ' Generate individual identifier for the Settings-Panel
        Dim Identifier As String = SettingPanel.FitFunction.FitFunctionName
        Dim i As Integer = 1

        ' Go through all existing setting-panels, and rename them.
        For Each ExistingSettingPanel As cFitSettingPanel In Me.FitFunctionSettings
            If ExistingSettingPanel.FitFunctionName = SettingPanel.FitFunctionName Then

                Dim NewIdentifier As String = Identifier & " #" & i.ToString

                ' Add the same Spectroscopy-Table under the new reference
                If Me.pbPreview.CurrentSpectroscopyTables.ContainsKey(ExistingSettingPanel.Identifier) Then
                    Dim SpectroscopyTable As cSpectroscopyTable = Me.pbPreview.CurrentSpectroscopyTables(ExistingSettingPanel.Identifier)
                    SpectroscopyTable.FullFileName = NewIdentifier
                    Me.pbPreview.RemoveSpectroscopyTable(ExistingSettingPanel.Identifier)
                    Me.pbPreview.AddSpectroscopyTable(SpectroscopyTable)
                End If

                ' Change the FitSettingsPanel identifier itself
                ExistingSettingPanel.Identifier = NewIdentifier

                i += 1
            End If
        Next
        Identifier &= " #" & i.ToString
        SettingPanel.Identifier = Identifier
        ' END: Generate individual identifier for the Settings-Panel
        '#############################################################

        ' Register the Settings-panel in the Settings-Panel list
        With Me.FitFunctionSettings
            .Add(SettingPanel)

            ' If more than one settings-panel exists, lock the YOffset of all
            ' of them, except for the first one! Otherwise the algorithm
            ' will get a problem optimizing the multiple YOffsets.
            If .Count > 1 Then
                .Item(0).LockFitParametersForMultipleFit = False
                For j As Integer = 1 To .Count - 1 Step 1
                    .Item(j).LockFitParametersForMultipleFit = True
                Next
            ElseIf Me.FitFunctionSettings.Count = 1 Then
                .Item(0).LockFitParametersForMultipleFit = False
            End If

        End With

        ' Set the fit-function object
        Me.SetFitFunction()

        ' refresh preview image
        Me.CalculatePreviewImage()

        ' Enable the Fit-Start button
        Me.btnStartFitting.Enabled = True

        Return True
    End Function

    ''' <summary>
    ''' Sets the Me.FitFunction by the given Fit-Function configuration
    ''' </summary>
    Protected Sub SetFitFunction()
        ' Decide whether to use a multiple-peak function
        ' or to perform a direct fit, if only a single FitFunction has been selected
        Select Case Me.FitFunctionSettings.Count
            Case 1
                ' Single-Peak
                '#############

                ' Set to the first and only selected Fit-Function
                Me.FitFunction = Me.FitFunctionSettings(0).FitFunction

                ' Set the multi-threading options
                Me.FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings
            Case Is > 1
                ' Multiple-Peaks
                '################

                ' Summarize all Fit-Functions in a Multiple-Peak-Fit-Function
                Dim MultiplePeakContainer As New cFitFunction_MultiplePeaks

                ' Go through all Fit-Functions
                For Each FitFunctionSetting As cFitSettingPanel In Me.FitFunctionSettings
                    ' Set the multi-threading options
                    FitFunctionSetting.FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings

                    ' Add the Fit-Function to the Multiple-Peak-Fit-Container
                    MultiplePeakContainer.FitFunctions.Add(FitFunctionSetting.FitFunction)
                Next

                ' Initialize the multiple-peak fit-parameters:
                MultiplePeakContainer.ReInitializeFitParameters()

                ' Register the Multiple-Peak-Function
                Me.FitFunction = MultiplePeakContainer
        End Select

        ' Rebind all fit-parameters
        Me.RebindFitParameters()
    End Sub

    ''' <summary>
    ''' Rebinds all fit-parameters to the boxes. Needed to have all Lock-To selections
    ''' </summary>
    Public Sub RebindFitParameters()
        ' Rebind all fit-parameters
        For Each SP As cFitSettingPanel In Me.FitFunctionSettings
            SP.SetFitParameterGroups(Me.FitFunction.FitParametersGroupedWithoutCombinedGroup)
        Next
    End Sub

    ''' <summary>
    ''' Removes the given FitSettingsPanel from the list.
    ''' </summary>
    Private Sub RemoveFitFunction(ByRef FitSettingsPanel As cFitSettingPanel)

        ' Remove from the flow-layout panel
        Me.flpFitModels.Controls.Remove(FitSettingsPanel)

        ' Remove from central list
        Me.FitFunctionSettings.Remove(FitSettingsPanel)

        RemoveHandler FitSettingsPanel.XRangeSelectionRequest, AddressOf Me.SelectXRange
        RemoveHandler FitSettingsPanel.YRangeSelectionRequest, AddressOf Me.SelectYRange
        RemoveHandler FitSettingsPanel.XValueSelectionRequest, AddressOf Me.SelectXValue
        RemoveHandler FitSettingsPanel.YValueSelectionRequest, AddressOf Me.SelectYValue
        RemoveHandler FitSettingsPanel.RequestRemovalOfFitFunction, AddressOf Me.RemoveFitFunction
        RemoveHandler FitSettingsPanel.ParameterChanged, AddressOf Me.CalculatePreviewImage
        RemoveHandler FitSettingsPanel.SubParametersAddedOrRemoved, AddressOf Me.RebindFitParameters

        ' Check again for the lock of the YOffset:
        ' If more than one settings-panel exists, lock the YOffset of all
        ' of them, except for the first one! Otherwise the algorithm
        ' will get a problem optimizing the multiple YOffsets.
        If Me.FitFunctionSettings.Count > 1 Then
            Me.FitFunctionSettings(0).LockFitParametersForMultipleFit = False
            For j As Integer = 1 To Me.FitFunctionSettings.Count - 1 Step 1
                Me.FitFunctionSettings(j).LockFitParametersForMultipleFit = True
            Next
        ElseIf Me.FitFunctionSettings.Count = 1 Then
            Me.FitFunctionSettings(0).LockFitParametersForMultipleFit = False
        End If

        ' Remove from preview-box
        Me.pbPreview.RemoveSpectroscopyTable(FitSettingsPanel.Identifier)

        ' Clear all memory traces.
        FitSettingsPanel.Dispose()

        ' Set the FitFunction object
        Me.SetFitFunction()

        ' refresh preview image
        Me.CalculatePreviewImage()

        ' Disable the Fit-Start button, if no Fit-Model is left
        If Me.FitFunctionSettings.Count <= 0 Then
            Me.btnStartFitting.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' This sub removes all fit-functions simultaneously.
    ''' </summary>
    Private Sub ClearAllFitFunctions()
        Me.FitFunctionSettings.Clear()
        For Each C As Control In Me.flpFitModels.Controls
            C.Dispose()
        Next
        Me.flpFitModels.Controls.Clear()

        ' Refresh preview image
        Me.CalculatePreviewImage()
    End Sub

#End Region

#Region "Preview-Settings"
    Private _PreviewPoints As Integer = 500
    ''' <summary>
    ''' Number of Points used for calculating the preview of the Fit-Data.
    ''' </summary>
    Private Property PreviewPoints As Integer
        Get
            Return _PreviewPoints
        End Get
        Set(value As Integer)
            _PreviewPoints = value
            If nudPreviewPoints.Value <> value Then
                nudPreviewPoints.Value = value
            End If
            My.Settings.Fit_SingleFile_PreviewPoints = value
            My.Settings.Save()
        End Set
    End Property

    ''' <summary>
    ''' Set the current value of the preview-points in the NumericUpDown.
    ''' </summary>
    Private Sub btnSetPreviewPointNumber_Click(sender As Object, e As EventArgs) Handles btnSetPreviewPointNumber.Click
        Me.PreviewPoints = Convert.ToInt32(Me.nudPreviewPoints.Value)
        Me.CalculatePreviewImage()
    End Sub

    ''' <summary>
    ''' Apply the preview-point number by pressing enter.
    ''' This is usually done by the user, instead of clicking the button.
    ''' </summary>
    Private Sub nudPreviewPoints_KeyPress(sender As Object, e As KeyEventArgs) Handles nudPreviewPoints.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.PreviewPoints = Convert.ToInt32(Me.nudPreviewPoints.Value)
            Me.CalculatePreviewImage()
        End If
    End Sub
#End Region

#Region "Preview Drawing"

    ''' <summary>
    ''' Calculates the fit-function-preview and adds the 
    ''' result as a newly created Spectroscopy-Table object to the
    ''' SpectroscopyTableViewer.
    ''' 
    ''' This is done for each Fit-Function.
    ''' </summary>
    Private Sub CalculatePreviewImage()

        If Me.CurrentSpectroscopyTable Is Nothing Then Return

        ' Create equally spaced fit-function-data, if the fit-range is set
        If Me.FitRangeLeft < Me.FitRangeRight Then

            Me.pbPreview.ClearSpectroscopyTables()
            Me.pbPreview.AddSpectroscopyTable(Me.CurrentSpectroscopyTable)

            ' Preview-FitFunction-Array
            Dim PreviewSpectroscopyTables As New List(Of cSpectroscopyTable)

            Dim SpectroscopyTable As cSpectroscopyTable
            Dim xColumn As cSpectroscopyTable.DataColumn
            Dim yColumn As cSpectroscopyTable.DataColumn

            ' Go through the FitFunction array.
            For Each SettingsPanel As cFitSettingPanel In Me.FitFunctionSettings
                SpectroscopyTable = New cSpectroscopyTable

                ' Set the name of the SpectroscopyTable to the FitFunction
                SpectroscopyTable.FullFileName = SettingsPanel.Identifier

                ' Create the columns and set the names to the currently displayed column-names
                xColumn = New cSpectroscopyTable.DataColumn
                yColumn = New cSpectroscopyTable.DataColumn
                xColumn.Name = Me.cbX.SelectedColumnName
                yColumn.Name = Me.cbY.SelectedColumnName

                ' Create the data for the yColumn and the specific fitfunction.
                For n As Integer = 0 To Me._PreviewPoints - 1 Step 1
                    ' Generate Data
                    xColumn.AddValueToColumn(Me.FitRangeLeft + (Me.FitRangeRight - Me.FitRangeLeft) / PreviewPoints * n)
                    yColumn.AddValueToColumn(SettingsPanel.FitFunction.GetY(xColumn.Values.Last, SettingsPanel.FitFunction.FitParametersGrouped))
                Next

                ' Add the columns to the Preview-Box
                SpectroscopyTable.AddNonPersistentColumn(xColumn)
                SpectroscopyTable.AddNonPersistentColumn(yColumn)

                PreviewSpectroscopyTables.Add(SpectroscopyTable)
            Next

            ' Finally add a combined Spectroscopy-Table, if there is more than one fit-function
            If Me.FitFunctionSettings.Count > 1 Then

                ' Create new SpectroscopyTable-Object
                SpectroscopyTable = New cSpectroscopyTable

                ' Set the name of the SpectroscopyTable to the FitFunction
                SpectroscopyTable.FullFileName = My.Resources.rFitFunction_MultiplePeaks.CombinedCurveName

                ' Create a separate YColumn for the summed up data,
                ' to show for multiple fit-functions a combined preview.
                xColumn = New cSpectroscopyTable.DataColumn
                yColumn = New cSpectroscopyTable.DataColumn
                xColumn.Name = Me.cbX.SelectedColumnName
                yColumn.Name = Me.cbY.SelectedColumnName

                ' Set the XColumn to the first PreviewSpectroscopyTable-XColumn
                xColumn = PreviewSpectroscopyTables(0).Column(xColumn.Name)

                Dim zero As Double = 0
                Dim yColumnValues As List(Of Double) = Enumerable.Repeat(zero, Me.PreviewPoints).ToList
                For Each ST As cSpectroscopyTable In PreviewSpectroscopyTables
                    Dim STYColumn As ReadOnlyCollection(Of Double) = ST.Column(yColumn.Name).Values
                    For n As Integer = 0 To Me._PreviewPoints - 1 Step 1
                        yColumnValues(n) += STYColumn(n)
                    Next
                Next
                yColumn.SetValueList(yColumnValues)

                ' Add the columns to the Preview-Box
                SpectroscopyTable.AddNonPersistentColumn(xColumn)
                SpectroscopyTable.AddNonPersistentColumn(yColumn)

                PreviewSpectroscopyTables.Add(SpectroscopyTable)

            Else
                ' Otherwise remove any combined data
                Me.pbPreview.RemoveSpectroscopyTable(My.Resources.rFitBase.CurveTitle_MultipleFit)
            End If

            ' Avoid the restore of the scale, because parameter-selection gets really horrible by this.
            Me.pbPreview.AutomaticallyRestoreScaleAfterRedraw = False

            ' Add the Spectroscopy-Table-List to the Preview-Box
            Me.pbPreview.AddSpectroscopyTables(PreviewSpectroscopyTables)
            'Me.pbPreview.cbX.SelectedColumnName = Me.cbX.SelectedColumnName
            Me.pbPreview.cbY.SelectedColumnName = Me.cbY.SelectedColumnName

            ' Activate the restore of the scale again.
            Me.pbPreview.AutomaticallyRestoreScaleAfterRedraw = True
        End If

    End Sub

#End Region

#Region "Range-selection for easier parameter modification"

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectXRange(Callback As mSpectroscopyTableViewer.XRangeSelectionCallback)
        Me.pbPreview.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview.CallbackXRangeSelected = Callback
        Me.pbPreview.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XRange
    End Sub


    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectYRange(Callback As mSpectroscopyTableViewer.YRangeSelectionCallback)
        Me.pbPreview.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview.CallbackYRangeSelected = Callback
        Me.pbPreview.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.YRange
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectXValue(Callback As mSpectroscopyTableViewer.XValueSelectionCallback)
        Me.pbPreview.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview.CallbackXValueSelected = Callback
        Me.pbPreview.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XValue
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectYValue(Callback As mSpectroscopyTableViewer.YValueSelectionCallback)
        Me.pbPreview.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview.CallbackYValueSelected = Callback
        Me.pbPreview.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.YValue
    End Sub

#End Region

#Region "Handle the stacking of the flow-layout-panel"
    Private FLPWidthOffset As Short = 15
    Private Sub flpFitModels_Resize(sender As Object, e As EventArgs) Handles flpFitModels.Resize
        ' Set the width of all sub-controls to the width of the settings-panel
        For Each C As Control In Me.flpFitModels.Controls
            C.Width = Me.flpFitModels.Width - FLPWidthOffset
        Next
    End Sub
#End Region

#Region "Fit-Range"

    ''' <summary>
    ''' Event gets raised, if the fit-range selection changed.
    ''' </summary>
    Protected Friend Event FitRangeSelectionChanged(LeftValue As Double, RightValue As Double)

    ''' <summary>
    ''' Entered Fit-Range changed
    ''' </summary>
    Private Sub txtFitRange_TextChanged(ByRef Sender As NumericTextbox) Handles txtLeftValue.ValidValueChanged, txtRightValue.ValidValueChanged
        Me.SetFitRange(Me.txtLeftValue.DecimalValue, Me.txtRightValue.DecimalValue)
        Me.CalculatePreviewImage()
    End Sub

    ''' <summary>
    ''' Sets the range in which the Fit-Function is defined
    ''' </summary>
    Public Sub SetFitRange(LeftValue As Double, RightValue As Double)

        ' Set the content of the textboxes
        Me.txtLeftValue.SetValue(LeftValue)
        Me.txtRightValue.SetValue(RightValue)

        My.Settings.Fit_SingleFile_FitRange_LeftValue = LeftValue
        My.Settings.Fit_SingleFile_FitRange_RightValue = RightValue
        My.Settings.Save()

        RaiseEvent FitRangeSelectionChanged(LeftValue, RightValue)

        Me.CalculatePreviewImage()
    End Sub

    ''' <summary>
    ''' Left limit of the fit-range.
    ''' </summary>
    Public Property FitRangeLeft As Double
        Get
            Return Me.txtLeftValue.DecimalValue
        End Get
        Set(value As Double)
            Me.txtLeftValue.SetValue(value)
            My.Settings.Fit_SingleFile_FitRange_LeftValue = value
            My.Settings.Save()
            RaiseEvent FitRangeSelectionChanged(Me.FitRangeLeft, Me.FitRangeRight)
            Me.CalculatePreviewImage()
        End Set
    End Property

    ''' <summary>
    ''' Right limit of the fit-range.
    ''' </summary>
    Public Property FitRangeRight As Double
        Get
            Return Me.txtRightValue.DecimalValue
        End Get
        Set(value As Double)
            Me.txtRightValue.SetValue(value)
            My.Settings.Fit_SingleFile_FitRange_RightValue = value
            My.Settings.Save()
            RaiseEvent FitRangeSelectionChanged(Me.FitRangeLeft, Me.FitRangeRight)
            Me.CalculatePreviewImage()
        End Set
    End Property

    ''' <summary>
    ''' Raise event, that the fit-range selection has been requested.
    ''' </summary>
    Private Sub btnSelectFitRange_Click(sender As Object, e As EventArgs) Handles btnSelectFitRange.Click
        Me.SelectXRange(AddressOf Me.SetFitRange)
    End Sub
#End Region

#Region "Fit-Echo"
    ''' <summary>
    ''' Report-Function for getting the status of the Fit.
    ''' </summary>
    Private Sub ReportFunction(ByVal FitParameters As cFitParameterGroupGroup, Chi2 As Double) Handles FitProcedure.FitStepEcho
        Dim sb As New System.Text.StringBuilder
        sb.Append(cFitParameter.GetShortParameterEcho(FitParameters))
        sb.Append(Now.ToString("hh:mm:ss"))
        sb.Append(" - Calculated Chi2 >> ")
        sb.Append(Chi2.ToString(cFitParameter.NumericFormat))
        sb.Append(" << ")

        ' Preview updaten!
        If Me.ckbUpdatePreviewDuringFit.Checked Then
            Me.WriteBackFitParameters(FitParameters)
        End If

        HandleFitEcho(sb.ToString)
    End Sub

    ''' <summary>
    ''' Fit-Echo function
    ''' </summary>
    Private Delegate Sub _HandleFitEcho(message As String)
    Private Sub HandleFitEcho(Message As String) Handles FitProcedure.FitEcho
        If Me.txtFitEcho.InvokeRequired Then
            Dim _delegate As New _HandleFitEcho(AddressOf HandleFitEcho)
            Invoke(_delegate, Message)
        Else
            Me.txtFitEcho.Text &= Message & vbCrLf
            Me.txtFitEcho.SelectionStart = Me.txtFitEcho.Text.Length
            Me.txtFitEcho.ScrollToCaret()
        End If
    End Sub

    ''' <summary>
    ''' Handle Progress Event
    ''' </summary>
    Private Delegate Sub _HandleProgress(ByVal Item As Integer, ByVal Max As Integer, ByVal Message As String)
    Private Sub HandleProgress(ByVal Item As Integer, ByVal Max As Integer, ByVal Message As String) Handles FitProcedure.CalculationStepProgress
        If Me.InvokeRequired Then
            Dim _delegate As New _HandleProgress(AddressOf HandleProgress)
            Invoke(_delegate, Item, Max, Message)
        Else
            Dim Percent As Double = (Item / Max) * 100
            If Percent > 100 Then Percent = 100
            If Percent < 0 Then Percent = 0

            Me.lblFitProgress.Text = Message & " " & Percent.ToString("N2") & "% (" & Item & "|" & Max & ")"
            Me.pgbFitProgress.Value = CInt(Percent)
        End If
    End Sub
#End Region

#Region "Fit-Initialization"
    ''' <summary>
    ''' Starts the Fit-Procedure
    ''' </summary>
    Public Sub StartFitting(Optional ByVal IgnoreWarnings As Boolean = False) Implements iFitWindow.StartFitting

        ' Clear the FitEcho-Box
        If Me.txtFitEcho.Text.Trim <> String.Empty And Not IgnoreWarnings Then
            'If MessageBox.Show(My.Resources.rFitting.FitEcho_ReallyClearFitEcho,
            '                   My.Resources.rFitting.FitEcho_ReallyClearFitEcho_Title,
            '                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            '    Me.txtFitEcho.Clear()
            'End If
            Me.txtFitEcho.Clear()
        End If

        If Me.FitRangeLeft >= Me.FitRangeRight Then
            HandleFitEcho("ERROR: fit aborted - fit-range is invalid")
            RaiseEvent FitFinishedEvent()
            Return
        End If

        ' Get the fit-data from the SpectroscopyTable
        Dim SourceCol_X As cSpectroscopyTable.DataColumn = Me.CurrentSpectroscopyTable.Column(Me.cbX.SelectedColumnName).GetCopy
        Dim SourceCol_Y As cSpectroscopyTable.DataColumn = Me.CurrentSpectroscopyTable.Column(Me.cbY.SelectedColumnName)

        ' Check, if the Fit-Range is in the range of the fit-data
        ' or otherwise change it to be in there.
        If Me.FitRangeLeft < SourceCol_X.GetMinimumValueOfColumn Then Me.FitRangeLeft = SourceCol_X.GetMinimumValueOfColumn
        If Me.FitRangeRight > SourceCol_X.GetMaximumValueOfColumn Then Me.FitRangeRight = SourceCol_X.GetMaximumValueOfColumn

        ' Cut down the data to the selected fit-range
        SourceCol_X.SetValuesOutsideRangeToNaN(Me.FitRangeLeft, Me.FitRangeRight)
        SourceCol_Y = SourceCol_Y.GetColumnWithoutValuesWhereSourceColumnIsNaN(SourceCol_X)
        'SourceCol_X = SourceCol_X.GetColumnWithoutCroppedData

        ' Create the source array of double-values from the columns
        Dim FitDataPoints()() As Double = New Double()() {SourceCol_X.Values.ToArray, SourceCol_Y.Values.ToArray}

        ' Check for FitFunction Is Nothing
        If Me.FitFunction Is Nothing Then
            HandleFitEcho("ERROR: fit aborted - no fit-models present")
            RaiseEvent FitFinishedEvent()
            Return
        End If

        '' Decide whether to use a multiple-peak function
        '' or to perform a direct fit, if only a single FitFunction has been selected
        'Select Case Me.FitFunctionSettings.Count
        '    Case 1
        '        ' Single-Peak
        '        '#############

        '        ' Set to the first and only selected Fit-Function
        '        Me.FitFunction = Me.FitFunctionSettings(0).FitFunction

        '        ' Set the multi-threading options
        '        Me.FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings
        '    Case Is > 1
        '        ' Multiple-Peaks
        '        '################

        '        ' Summarize all Fit-Functions in a Multiple-Peak-Fit-Function
        '        Dim MultiplePeakContainer As New cFitFunction_MultiplePeaks

        '        ' Go through all Fit-Functions
        '        For Each FitFunctionSetting As cFitSettingPanel In Me.FitFunctionSettings
        '            ' Set the multi-threading options
        '            FitFunctionSetting.FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings

        '            ' Add the Fit-Function to the Multiple-Peak-Fit-Container
        '            MultiplePeakContainer.FitFunctions.Add(FitFunctionSetting.FitFunction)
        '        Next

        '        ' Initialize the multiple-peak fit-parameters:
        '        MultiplePeakContainer.ReInitializeFitParameters()

        '        ' Register the Multiple-Peak-Function
        '        Me.FitFunction = MultiplePeakContainer

        '    Case Else
        '        HandleFitEcho("ERROR: fit aborted - no fit-models present")
        '        RaiseEvent FitFinishedEvent()
        '        Return
        'End Select

        ' Set the range of the fit-function.
        ' This normally does nothing. But some fit-functions need this,
        ' to work properly, e.g. for current integrals.
        Me.FitFunction.ChangeFitRangeX(Me.FitRangeLeft, Me.FitRangeRight)

        ' TODO:
        ' Create a selection-tool for defining weights

        ' Load the Fit-Procedure with the defined Fit-Function
        If Me.FitProcedure Is Nothing Then
            HandleFitEcho("ERROR: fit aborted - no fit-procedure selected")
            RaiseEvent FitFinishedEvent()
            Return
        End If

        ' Initial Echo
        HandleFitEcho("#########################################")
        HandleFitEcho("initialization of " & Me.FitProcedure.Name)
        HandleFitEcho("source data: " & Me.CurrentSpectroscopyTable.FileNameWithoutPath)
        HandleFitEcho("-------- fit procedure settings ---------")
        HandleFitEcho("maximum iteration count: " & Me.FitProcedure.FitProcedureSettings.StopCondition_MaxIterations.ToString("N0"))
        HandleFitEcho("min change in Chi^2:     " & Me.FitProcedure.FitProcedureSettings.StopCondition_MinChi2Change.ToString("E3"))
        HandleFitEcho(Me.FitProcedure.FitProcedureSettings.EchoSettings)
        HandleFitEcho("---------- selected fit models ----------")
        HandleFitEcho("fit models: " & vbNewLine & Me.FitFunction.FitFunctionName)
        HandleFitEcho("fit formula: " & vbNewLine & Me.FitFunction.FitFunctionFormula)
        ' Go through all fit-parameters and echo them
        HandleFitEcho("------- initial set of parameters -------")
        HandleFitEcho(cFitParameter.GetFullParameterEcho(Me.FitFunction.FitParametersGrouped))
        HandleFitEcho("-----------------------------------------")
        HandleFitEcho("3 ... 2 ... 1 ... fit running ... good luck!")

        ' Deactivate the UI-buttons during the fit
        Me.SetButtonsToFitModus(True)

        ' Initialize a UI refresh
        Me.Refresh()

        ' Start Fit async
        Me.FitProcedure.FitAsync(Me.FitFunction,
                                 FitDataPoints,
                                 Nothing)

    End Sub
#End Region

#Region "Fit-Finishing"
    Private Delegate Sub _FitFinished(FitStopReason As Integer,
                                      FinalParameters As cFitParameterGroupGroup,
                                      Chi2 As Double)
    ''' <summary>
    ''' Fit Finished Function
    ''' </summary>
    Private Sub FitFinished(FitStopReason As Integer,
                            FinalParameters As cFitParameterGroupGroup,
                            Chi2 As Double) Handles FitProcedure.FitFinished

        ' Check for required Invoke
        If Me.InvokeRequired Then
            Dim _delegate As New _FitFinished(AddressOf FitFinished)
            Invoke(_delegate, FitStopReason, FinalParameters, Chi2)
        Else
            ' Echo the final set of Parameters
            ' and fit results, depending on the final Fit-Report.
            HandleFitEcho("------------ fit finished ------------")
            HandleFitEcho("stop reason: " & vbNewLine & Me.FitProcedure.ConvertFitStopCodeToMessage(FitStopReason))
            HandleFitEcho("interation count: " & Me.FitProcedure.Iterations.ToString("N0"))
            HandleFitEcho("------------- statistics -------------")
            HandleFitEcho("standard deviation (sigma):   " & Me.FitProcedure.FinalStatistics.StandardDeviation.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("variance (sigma^2):           " & Me.FitProcedure.FinalStatistics.Variance.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("mean squared error (MSE):     " & Me.FitProcedure.FinalStatistics.MeanError.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("coef. of determination (R^2): " & Me.FitProcedure.FinalStatistics.CoefficientOfDetermination_R2.ToString("F6"))
            HandleFitEcho("------- final set of parameters ------")
            HandleFitEcho(cFitParameter.GetFullParameterEcho(FinalParameters))
            HandleFitEcho("")
            HandleFitEcho("Excel compatible output (values separated by a tab, just copy them):")
            HandleFitEcho(cFitParameter.GetExcelCompatibleParameterEcho(FinalParameters))
            HandleFitEcho("--------------------------------------")
            HandleFitEcho("total time consumption: " & Me.FitProcedure.FitDuration.ToString)
            HandleFitEcho("fit performed using SpectraFox ver. " & cProgrammDeployment.GetProgramVersionString)
            HandleFitEcho("#########################################")

            ' Copy the final set of parameters the the SettingPanels
            Me.WriteBackFitParameters(FinalParameters)

            ' reset the interface
            Me.SetButtonsToFitModus(False)

            ' Raise the fit-finished event, to inform other listeners
            RaiseEvent FitFinishedEvent()
        End If
    End Sub

    ''' <summary>
    ''' Writes back the Fit-Parameters to the parameter-boxes and updates the preview image.
    ''' </summary>
    Protected Sub WriteBackFitParameters(FinalParameters As cFitParameterGroupGroup)
        ' Decide whether the fit used a multiple-peak function or a single-fit-function.
        If TypeOf Me.FitFunction Is cFitFunction_MultiplePeaks Then
            ' Multiple Peak Fit Function
            '############################
            Dim MultiplePeakContainer As cFitFunction_MultiplePeaks = DirectCast(Me.FitFunction, cFitFunction_MultiplePeaks)

            ' Go through all Fit-Functions
            For Each FitFunctionSetting As cFitSettingPanel In Me.FitFunctionSettings

                ' Set the individual FitParameter
                FitFunctionSetting.SetFitParameters(FinalParameters)

            Next

        Else
            ' Single Peak Fit Function
            '##########################

            ' Write back the parameters
            Me.FitFunctionSettings(0).SetFitParameters(FinalParameters)

        End If

        ' Calculate the new preview-image, using the new parameters
        Me.CalculatePreviewImage()
    End Sub
#End Region

#Region "Fit-Button action, and interface state"
    ''' <summary>
    ''' Starts the Fit-Procedure, if the Worker is not Running.
    ''' </summary>
    Private Sub StartFitting_Click(sender As System.Object, e As System.EventArgs) Handles btnStartFitting.Click
        If Not Me.FitProcedure Is Nothing Then
            If Not Me.FitProcedure.FitWorker.IsBusy Then
                ' Start:
                Me.StartFitting()
            Else
                ' Abort:
                Me.FitProcedure.AbortAsyncFit()
                Me.btnStartFitting.Enabled = False
                Me.btnStartFitting.Text = My.Resources.rFitting.Fitting_CancellationPending
            End If
        End If
    End Sub

    ''' <summary>
    ''' Enables/disables the Start-Fetch buttons and set's the icons.
    ''' </summary>
    Private Sub SetButtonsToFitModus(ByVal IsFitRunning As Boolean, Optional ByVal UsedForSaving As Boolean = False)
        If Not UsedForSaving Then
            If IsFitRunning Then
                Me.btnStartFitting.Text = My.Resources.rFitting.Fitting_CancelFit
                Me.btnStartFitting.Image = My.Resources.cancel_16
            Else
                Me.btnStartFitting.Text = My.Resources.rFitting.Fitting_StartFit
                Me.btnStartFitting.Image = My.Resources.reload_16
            End If
            Me.btnStartFitting.Enabled = True
        Else
            Me.btnStartFitting.Enabled = Not IsFitRunning
        End If

        Me.btnExportFitModels.Enabled = Not IsFitRunning
        Me.btnImportFitModels.Enabled = Not IsFitRunning
        Me.btnSaveFitToSpectroscopyTable.Enabled = Not IsFitRunning
        Me.btnSaveFitProcedureOutput.Enabled = Not IsFitRunning
        Me.btnAddFitModel.Enabled = Not IsFitRunning
        Me.btnSetPreviewPointNumber.Enabled = Not IsFitRunning
        Me.nudPreviewPoints.Enabled = Not IsFitRunning
        Me.cboFitProcedure.Enabled = Not IsFitRunning
        Me.btnChangeFitProcedureSettings.Enabled = Not IsFitRunning
        Me.btnAddFitToFitQueue.Enabled = Not IsFitRunning
        Me.gbFitRange.Enabled = Not IsFitRunning
        Me.gbSourceDataSelector.Enabled = Not IsFitRunning

        Me.gbProgress.Visible = IsFitRunning

        ' Disable all Fit-Model-settings-panels
        For Each C As Control In Me.flpFitModels.Controls
            C.Enabled = Not IsFitRunning
        Next
    End Sub
#End Region

#Region "Catch Key-Down events (e.g. for cancelling point-selection mode)"

    ''' <summary>
    ''' Catch the Key-Down event, to:
    ''' 1) Cancel the Point-Selection-Mode, if it is active.
    ''' </summary>
    Public Sub KeyDownCatch(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        ' 1) quit point-selection mode
        If e.KeyCode = Keys.Escape Then
            Me.pbPreview.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.None
        End If

    End Sub

#End Region

#Region "Exporting and Importing the Collection of Fit-Models"

#Region "Button to launch the Import/Export procedures"

    ''' <summary>
    ''' Function to get the default export model name.
    ''' </summary>
    Protected Function GetDefaultExportModelName() As String
        Return Me.CurrentSpectroscopyTable.FileNameWithoutPathAndExtension & My.Resources.rFitting.FitModelExport_FileExtension_SingleData
    End Function

    ''' <summary>
    ''' Button to launch the import
    ''' </summary>
    Private Sub btnImportFitModels_Click(sender As Object, e As EventArgs) Handles btnImportFitModels.Click

        ' Displays a OpenFileDialog so the user can load the fit-models
        Dim Dialog As New OpenFileDialog()

        ' Open last path, if it was selected once
        If System.IO.Directory.Exists(My.Settings.Fit_SingleFile_ModelExportPath) Then
            Dialog.InitialDirectory = My.Settings.Fit_SingleFile_ModelExportPath
        End If

        Dialog.Filter = My.Resources.rFitting.FitModelExport_FileExtensionDescription_SingleData & "|*" & My.Resources.rFitting.FitModelExport_FileExtension_SingleData
        Dialog.Title = My.Resources.rFitting.FitModelExport_ImportWindowTitle
        Dialog.FileName = Me.GetDefaultExportModelName
        Dim DialogRes As DialogResult = Dialog.ShowDialog()

        ' If the file name is not an empty string take the path and start the export.
        If DialogRes = DialogResult.OK And Dialog.FileName <> "" Then
            ' Save last path:
            My.Settings.Fit_SingleFile_ModelExportPath = System.IO.Path.GetDirectoryName(Dialog.FileName)
            My.Settings.Save()

            ' Start the Import:
            Me.Import(Dialog.FileName)
        End If

    End Sub

    ''' <summary>
    ''' Button to launch the export
    ''' </summary>
    Private Sub btnExportFitModels_Click(sender As Object, e As EventArgs) Handles btnExportFitModels.Click

        ' Displays a SaveFileDialog so the user can save the fit-models
        Dim Dialog As New SaveFileDialog()

        ' Open last path, if it was selected once
        If System.IO.Directory.Exists(My.Settings.Fit_SingleFile_ModelExportPath) Then
            Dialog.InitialDirectory = My.Settings.Fit_SingleFile_ModelExportPath
        End If

        Dialog.Filter = My.Resources.rFitting.FitModelExport_FileExtensionDescription_SingleData & "|*" & My.Resources.rFitting.FitModelExport_FileExtension_SingleData
        Dialog.Title = My.Resources.rFitting.FitModelExport_ExportWindowTitle
        Dialog.FileName = Me.GetDefaultExportModelName
        Dim DialogRes As DialogResult = Dialog.ShowDialog()

        ' If the file name is not an empty string take the path and start the export.
        If DialogRes = DialogResult.OK And Dialog.FileName <> "" Then
            ' Save last path:
            My.Settings.Fit_SingleFile_ModelExportPath = System.IO.Path.GetDirectoryName(Dialog.FileName)
            My.Settings.Save()

            ' Start the Export:
            Me.Export(Dialog.FileName)
        End If

    End Sub

#End Region

#Region "Export"

    ''' <summary>
    ''' Export-Function:
    ''' For each fit-model the XML-file is generated in a temporary location,
    ''' using the FitFunction-Class-Name as file-name, plus an added individual counter (_x).
    ''' All the files are zipped afterwards and moved to a location determined by the user.
    ''' </summary>
    Public Sub Export(TargetFileName As String)

        Try
            ' Create the File-List
            Dim TmpFileList As New List(Of String)

            Dim TMPPath As String = System.IO.Path.GetTempPath()
            Dim TMPFileName As String
            Dim Counter As Integer = 0

            ' Create the temporary file name
            TMPFileName = Me.FitFunction.GetType.ToString & "." & Counter & ".xml"
            If Me.FitFunction.ExportXML(TMPPath & TMPFileName) Then
                TmpFileList.Add(TMPPath & TMPFileName)
            End If

            If TmpFileList.Count > 0 Then
                ' Now take all the generated files and zip them together
                Dim ZipFileName As String = TMPPath & "FitModels.zip"
                cCompression.CompressFiles(ZipFileName, TmpFileList)

                ' Now remove all the temporary generated XML-files
                For Each FilePlusPathName As String In TmpFileList
                    System.IO.File.Delete(FilePlusPathName)
                Next

                ' Now move the zipped file to the location requested by the user
                System.IO.File.Delete(TargetFileName)
                System.IO.File.Move(ZipFileName, TargetFileName)
            End If

            'MessageBox.Show(My.Resources.rFitting.FitModelExport_Export_Success,
            '                My.Resources.rFitting.FitModelExport_Success_Title,
            '                MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(My.Resources.rFitting.FitModelExport_Error_Export & ex.Message,
                            My.Resources.rFitting.FitModelExport_Error_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


#End Region

#Region "Import"

    ''' <summary>
    ''' Import-Function:
    ''' For each fit-model the XML-file is extracted in a temporary location,
    ''' using the FitFunction-Class-Name as file-name, we create the individual fit-models,
    ''' and save all their parameters.
    ''' </summary>
    Public Sub Import(SourceFileName As String)

        Try
            ' Create the File-List
            Dim TmpFileList As New Dictionary(Of String, IO.MemoryStream)

            Dim TMPPath As String = System.IO.Path.GetTempPath()

            ' Extract the file from the zip-container
            TmpFileList = cCompression.UncompressFilesInMemory(SourceFileName)

            ' Clear all existing Fit-Models
            Me.ClearAllFitFunctions()

            ' Generate a fit-model for each XMLFile in memory (FileName, MemoryStream)
            For Each FileStreamKV As KeyValuePair(Of String, IO.MemoryStream) In TmpFileList
                ' The class of the fit-function is saved in the file-name of the XML,
                ' so extract this information out of the file-name.
                Dim RegPattern As String = "(.*?)\.\d*?\.xml$"

                ' Instantiate the regular expression object.
                Dim Reg As Regex = New Regex(RegPattern, RegexOptions.IgnoreCase)

                ' Match the regular expression pattern against a text string.
                Dim RegMatch As Match = Reg.Match(System.IO.Path.GetFileName(FileStreamKV.Key))
                If RegMatch.Success Then
                    ' There is just one group
                    Dim RegGroup As Group = RegMatch.Groups(1)

                    ' Save the match of this group as class name
                    Dim ClassName As String = RegGroup.ToString()
                    Dim ClassType As Type = AvailablePlugins.GetType(ClassName)

                    ' Get the fit-function type by this.
                    Dim FitFunction As iFitFunction = cFitFunction.GetFitFunctionByType(ClassType)

                    ' Import the FitFunction-Data
                    FitFunction.ImportXML(FileStreamKV.Value)

                    ' Now we have the fit-function with all its parameters.
                    ' Lets add it to the list.

                    ' Treat multiple fit functions different, than single fit functions
                    If FitFunction.GetType Is GetType(cFitFunction_MultiplePeaks) Then
                        ' multiple Fit Functions

                        Dim MultipleFitFunctions As List(Of iFitFunction) = DirectCast(FitFunction, cFitFunction_MultiplePeaks).FitFunctions

                        ' Manage the parameter-locks
                        Dim LockTo As New Dictionary(Of String, String)
                        Dim LockToTMP As Dictionary(Of String, String)
                        ' Extract the lock-Information of the parameters
                        For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                            LockToTMP = MultipleFitFunctions(i).FitParameters.GetLockedParameterInfo()
                            For Each KV As KeyValuePair(Of String, String) In LockToTMP
                                If Not LockTo.ContainsKey(KV.Key) Then LockTo.Add(KV.Key, KV.Value)
                            Next
                        Next
                        LockToTMP = Nothing

                        ' Create the fit-functions
                        For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                            Me.AddFitFunction(MultipleFitFunctions(i))
                        Next

                        ' import the settings
                        For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                            Me.FitFunctionSettings(i).SetFitParameters(MultipleFitFunctions(i).FitParameters)
                        Next

                        ' Write back lock-information
                        For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                            Me.FitFunctionSettings(i).WriteBackLockInformation(LockTo)
                        Next

                    Else
                        ' single fit function --> much easier

                        ' Create the new Settings-Panel
                        If Me.AddFitFunction(FitFunction) Then
                            ' Import the settings to the new settings-panel
                            Me.FitFunctionSettings.Last.SetFitParameters(FitFunction.FitParameters)
                        End If

                    End If

                End If
            Next

            ' Now remove all the temporary XML-files
            For Each FileStreamKV As KeyValuePair(Of String, IO.MemoryStream) In TmpFileList
                FileStreamKV.Value.Close()
                FileStreamKV.Value.Dispose()
            Next
            TmpFileList.Clear()

            'MessageBox.Show(My.Resources.rFitting.FitModelExport_Import_Success,
            '                My.Resources.rFitting.FitModelExport_Success_Title,
            '                MessageBoxButtons.OK, MessageBoxIcon.Information)
#If Not Debug Then
        Catch ex As Exception
            MessageBox.Show(My.Resources.rFitting.FitModelExport_Error_Import & ex.Message,
                            My.Resources.rFitting.FitModelExport_Error_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
#End If
        Finally
        End Try

    End Sub

#End Region

#End Region

#Region "saving of the current fit-data back to the spectroscopy-table"

    Private bGenerateDataForAllSourcePoints As Boolean

    ''' <summary>
    ''' Button to initialize the saving
    ''' </summary>
    Private Sub btnSaveFitToSpectroscopyTable_Click(sender As Object, e As EventArgs) Handles btnSaveFitToSpectroscopyTable.Click
        Me.SaveFitDataAsync()
    End Sub

    ''' <summary>
    ''' Background-Thread to generate the fit-data to save.
    ''' </summary>
    Private WithEvents FitDataSaver As New System.ComponentModel.BackgroundWorker

    ''' <summary>
    ''' Initializes the saving of the Fit-Data
    ''' </summary>
    Private Sub SaveFitDataAsync()
        If FitDataSaver.IsBusy Then Return

        ' Deactivate the buttons
        Me.SetButtonsToFitModus(True, True)

        ' Set the progress bar
        Me.lblSavingProgress.Text = My.Resources.rFitting.dummy
        Me.pgbSavingProgress.Value = 0

        ' Set the properties of data-generation:
        Me.bGenerateDataForAllSourcePoints = Not Me.ckbSaveGenerateDataOnlyInSelectedFitRange.Checked

        ' Start worker
        Me.FitDataSaver.RunWorkerAsync()
    End Sub

    ''' <summary>
    ''' This function saves the fit back to the spectroscopy-table.
    ''' Therefor it generates data using the current set of parameters in the fit-range.
    ''' The rest is set to NaN. Uses a separate thread, since the generation may need some time.
    ''' </summary>
    Private Sub SaveFitData() Handles FitDataSaver.DoWork

        ' Get the source-xcolumn
        Dim xSourceColumn As cSpectroscopyTable.DataColumn = Me.CurrentSpectroscopyTable.Column(Me.cbX.SelectedColumnName)
        Dim xSourceColumnValues As ReadOnlyCollection(Of Double) = xSourceColumn.Values
        Dim ySourceColumn As cSpectroscopyTable.DataColumn = Me.CurrentSpectroscopyTable.Column(Me.cbY.SelectedColumnName)
        Dim yColumns As New List(Of cSpectroscopyTable.DataColumn)
        Dim yColumn As cSpectroscopyTable.DataColumn

        ' Report progress
        Dim Progress As Double = 1
        Dim ProgressProgress As Double = 70 / (Me.FitFunctionSettings.Count + 1)
        Me.FitDataSaver.ReportProgress(CInt(Progress), My.Resources.rFitting.initializing)

        ' Go through the FitSettingsPanel-List and generate the individual fit-data for each settings-panel
        ' and its corresponding fit-function. Also check for additionally generatable data.
        For Each SettingsPanel As cFitSettingPanel In Me.FitFunctionSettings
            Me.FitDataSaver.ReportProgress(CInt(Progress), My.Resources.rFitting.FitSaving_GeneratingIndividualData)
            Progress += ProgressProgress

            ' Create the new column for the main data
            yColumn = New cSpectroscopyTable.DataColumn
            yColumn.Name = SettingsPanel.Identifier
            yColumn.UnitSymbol = ySourceColumn.UnitSymbol
            yColumn.UnitType = ySourceColumn.UnitType
            yColumn.SetValueList(SettingsPanel.FitFunction.GenerateData(xSourceColumn.Values))

            ' Add the columns to list
            yColumns.Add(yColumn)

            ' Ask the fit-function, if it wants to generate additional output-columns
            If SettingsPanel.FitFunction.AdditionalDataGenerationFunctions.Count > 0 Then
                ' Generate the data for each additional output column
                For Each OutputFunctionKV As KeyValuePair(Of String, iFitFunction._GetY) In SettingsPanel.FitFunction.AdditionalDataGenerationFunctions
                    ' Create the new column
                    yColumn = New cSpectroscopyTable.DataColumn
                    yColumn.Name = SettingsPanel.Identifier & " (" & OutputFunctionKV.Key & ")"
                    yColumn.UnitSymbol = ySourceColumn.UnitSymbol
                    yColumn.UnitType = ySourceColumn.UnitType
                    yColumn.SetValueList(SettingsPanel.FitFunction.GenerateData(xSourceColumn.Values, OutputFunctionKV.Value))

                    ' Add the columns to list
                    yColumns.Add(yColumn)
                Next
            End If
        Next

        ' Calculate the combined / summed data, if more than one model was selected
        If Me.FitFunctionSettings.Count > 1 Then
            ProgressProgress = 15 / (xSourceColumn.Values.Count + 1)

            ' Create a separate YColumn for the summed up data
            yColumn = New cSpectroscopyTable.DataColumn
            yColumn.Name = My.Resources.rFitting.CombinedFitColumnName
            yColumn.UnitSymbol = ySourceColumn.UnitSymbol
            yColumn.UnitType = ySourceColumn.UnitType

            Dim zero As Double = 0
            Dim yColumnValues As List(Of Double) = Enumerable.Repeat(zero, xSourceColumn.Values.Count).ToList

            For Each DataCol As cSpectroscopyTable.DataColumn In yColumns
                Dim DataColValues As ReadOnlyCollection(Of Double) = DataCol.Values
                Me.FitDataSaver.ReportProgress(CInt(Progress), My.Resources.rFitting.FitSaving_GeneratingCombinedData)
                Progress += ProgressProgress

                For n As Integer = 0 To DataCol.Values.Count - 1 Step 1
                    If Double.IsNaN(yColumnValues(n)) Then
                        ' Nothing, because is already set to NaN
                    ElseIf Double.IsNaN(DataColValues(n)) Then
                        yColumnValues(n) = Double.NaN
                    Else
                        yColumnValues(n) += DataColValues(n)
                    End If
                Next
            Next
            yColumn.SetValueList(yColumnValues)
            yColumns.Add(yColumn)
        End If

        ' Since the data got generated for the whole X-Source-Column range,
        ' crop it the the desired fit-range. Can be enabled or disabled by the fit.
        If Not Me.bGenerateDataForAllSourcePoints Then
            For i As Integer = 0 To yColumns.Count - 1 Step 1

                ' Set the reference
                yColumn = yColumns(i)

                ' Crop the data, by setting the values outside of the fit-range to Double.NaN
                For n As Integer = 0 To xSourceColumnValues.Count - 1 Step 1
                    If Not Double.IsNaN(xSourceColumnValues(n)) Then
                        If xSourceColumnValues(n) < Me.FitRangeLeft Or xSourceColumnValues(n) >= Me.FitRangeRight Then
                            yColumn.SetValueInColumn(n, Double.NaN)
                        End If
                    End If

                Next
            Next
        End If

        ' Finally save the data to the file-object
        Me.CurrentSpectroscopyTable.BaseFileObject.AddSpectroscopyColumns(yColumns)

        Me.FitDataSaver.ReportProgress(100, My.Resources.rFitting.ExportComplete)
    End Sub

    ''' <summary>
    ''' Activate the Button for saving again.
    ''' </summary>
    Private Sub SaveFitDataFinished() Handles FitDataSaver.RunWorkerCompleted
        ' Set the progress bar
        Me.lblSavingProgress.Text = My.Resources.rFitting.ExportComplete
        Me.pgbSavingProgress.Value = 0

        ' Activate the buttons
        Me.SetButtonsToFitModus(False, True)
    End Sub

    ''' <summary>
    ''' Update the progress of the saving
    ''' </summary>
    Private Sub SaveFitDataProgress(sender As Object, e As ProgressChangedEventArgs) Handles FitDataSaver.ProgressChanged
        Me.lblSavingProgress.Text = e.ProgressPercentage.ToString & " % - " & e.UserState.ToString
        If e.ProgressPercentage >= 0 And e.ProgressPercentage <= 100 Then
            Me.pgbSavingProgress.Value = e.ProgressPercentage
        End If
    End Sub

    ''' <summary>
    ''' Button to save the fit-output to the comment.
    ''' </summary>
    Private Sub btnSaveFitOutputToExtendedComment_Click(sender As Object, e As EventArgs) Handles btnSaveFitProcedureOutput.Click

        ' Save current content of the fit-output to the extended comment section
        If Me.txtFitEcho.Text.Trim <> String.Empty Then
            Me.CurrentSpectroscopyTable.BaseFileObject.AddExtendedComment(Me.txtFitEcho.Text)
        End If

    End Sub

#End Region

#Region "Window-Closing"

    ''' <summary>
    ''' Avoid closing the window during a running fit, or a running saving of data!
    ''' </summary>
    Private Sub wFitBase_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Me.FitDataSaver.IsBusy Then
            MessageBox.Show(My.Resources.rFitting.ExportRunning_PleaseWaitWithWindowClose,
                            My.Resources.rFitting.FitRunning_PleaseWaitWithWindowClose_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Cancel = True
        End If
        If Not Me.FitProcedure Is Nothing Then
            If Me.FitProcedure.FitWorker.IsBusy Then
                MessageBox.Show(My.Resources.rFitting.FitRunning_PleaseWaitWithWindowClose,
                                My.Resources.rFitting.FitRunning_PleaseWaitWithWindowClose_Title,
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Cancel = True
            End If
        End If

        If Not Me.AddressToCallbackOnFormClosing Is Nothing Then e.Cancel = Me.AddressToCallbackOnFormClosing.Invoke

        ' Save the settings
        My.Settings.Save()

    End Sub

#End Region

#Region "Fit-Procedure Changing"
    ''' <summary>
    ''' Set the Fit-Procedure Object on changing the selection
    ''' </summary>
    Private Sub cboFitProcedure_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFitProcedure.SelectedIndexChanged
        Me.FitProcedure.SetFitProcedure(Me.GetSelectedFitProcedure)
        If Not Me.FitProcedure Is Nothing Then
            My.Settings.Fit_SingleFile_Procedure = DirectCast(Me.cboFitProcedure.SelectedItem, KeyValuePair(Of Type, String)).Key.Name
            My.Settings.Save()
        End If
    End Sub
#End Region

#Region "Fit-Queue"

    ''' <summary>
    ''' Button press to add this fit-window to the fit-queue.
    ''' </summary>
    Private Sub btnAddFitToFitQueue_Click(sender As Object, e As EventArgs) Handles btnAddFitToFitQueue.Click

        ' Get the current fit-queue.
        Dim FitQueue As wFitQueue = wFitQueue.GetFitQueueWindow

        ' Change functionality depending on the membership in a fit-queue.
        ' Adds the fit to the fit-queue.
        Dim QueuePosition As Integer = FitQueue.AddFitToFitQueue(Me)
    End Sub

    ''' <summary>
    ''' Saves the new fit-queue position.
    ''' </summary>
    Public Sub FitQueuePositionChanged(ByVal NewPosition As Integer) Implements iFitWindow.FitQueuePositionChanged
        Me.btnAddFitToFitQueue.Text = My.Resources.rFitBase.btnFitQueue_FitPosition.Replace("%", (NewPosition + 1).ToString("N0"))
        Me.btnAddFitToFitQueue.Image = My.Resources.cancel_16
    End Sub

    ''' <summary>
    ''' Registers if the fit is removed from the fit-queue.
    ''' </summary>
    Public Sub FitRemovedFromFitQueue() Implements iFitWindow.FitRemovedFromFitQueue
        Me.btnAddFitToFitQueue.Text = My.Resources.rFitBase.btnFitQueue_AddFitToFitQueue
        Me.btnAddFitToFitQueue.Image = My.Resources.add_16

        ' Enable button again
        Me.btnAddFitToFitQueue.Enabled = True
    End Sub

    ''' <summary>
    ''' Registers the new fit-queue position.
    ''' </summary>
    Public Sub FitAddedToFitQueue(ByVal NewPosition As Integer) Implements iFitWindow.FitAddedToFitQueue
        Me.btnAddFitToFitQueue.Text = My.Resources.rFitBase.btnFitQueue_FitPosition.Replace("%", (NewPosition + 1).ToString("N0"))
        Me.btnAddFitToFitQueue.Image = My.Resources.cancel_16

        ' Change the Interface fit-buttons.
        Me.btnStartFitting.Enabled = False
        Me.btnAddFitToFitQueue.Enabled = False
    End Sub


#End Region

#Region "Parallelization Settings"

    ''' <summary>
    ''' Settings used for parallelization.
    ''' </summary>
    Private MultiThreadingSettings As New System.Threading.Tasks.ParallelOptions

    ''' <summary>
    ''' Change the parallelization settings of the fit.
    ''' </summary>
    Private Sub ChangeFitProcedureParallelizationSettingsWindow(sender As Object, e As EventArgs) Handles btnChangeParallelizationSettings.Click
        Dim SettingsWindow As New wFitParallelizationSettings
        SettingsWindow.ShowDialog()
        Me.MultiThreadingSettings.MaxDegreeOfParallelism = SettingsWindow.MaxThreadCount
        SettingsWindow.Dispose()
    End Sub
#End Region

#Region "Change Fit-Procedure Settings"
    ''' <summary>
    ''' Open the Fit-Procedure Adjustment window
    ''' </summary>
    Private Sub ChangeFitProcedureSettings() Handles btnChangeFitProcedureSettings.Click
        If Not Me.FitProcedure Is Nothing Then
            Dim SettingsWindow As New wFitProcedureSettingsWindow
            With Me.FitProcedure
                .FitProcedureSettings = SettingsWindow.ShowDialog(.ProcedureSettingPanel)
                Me.FitProcedureSettings = .FitProcedureSettings
            End With
        End If
    End Sub
#End Region

#Region "Interface implementations"

    ''' <summary>
    ''' Delegate called, on form closing
    ''' </summary>
    Public Property AddressToCallbackOnFormClosing As iFitWindow._AddressToCallbackOnFormClosing Implements iFitWindow.AddressToCallbackOnFormClosing

    ''' <summary>
    ''' Bring window to front.
    ''' </summary>
    Public Sub BringWindowToFront() Implements iFitWindow.BringToFront
        Me.BringToFront()
    End Sub

    ''' <summary>
    ''' Change window-title.
    ''' </summary>
    Public Property WindowText As String Implements iFitWindow.Text
        Get
            Return Me.Text
        End Get
        Set(value As String)
            Me.Text = value
        End Set
    End Property

#End Region
End Class
