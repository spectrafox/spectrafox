Imports System.ComponentModel
Imports System.Text.RegularExpressions

''' <summary>
''' Base class for a Data-Fitting window.
''' </summary>
Public Class wFit_MultipleDataSets
    Inherits wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad
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
    Public Sub SetSpectroscopyTable() Handles MyBase.AllFilesFetched

        If Me._SpectroscopyTableList.Count <> 2 Then
            MessageBox.Show(My.Resources.rFitBase.MultipleFit_WrongNumberOfDataFiles.Replace("%n", Me._SpectroscopyTableList.Count.ToString("N0")),
                            My.Resources.rFitBase.MultipleFit_WrongNumberOfDataFiles_Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        ' Initialize the column-selection
        Me.cbX_Set1.InitializeColumns(Me._SpectroscopyTableList(0).GetColumnList, My.Settings.Fit_MultipleFiles_Set1_LastCol_X)
        Me.cbY_Set1.InitializeColumns(Me._SpectroscopyTableList(0).GetColumnList, My.Settings.Fit_MultipleFiles_Set1_LastCol_Y)
        Me.cbX_Set2.InitializeColumns(Me._SpectroscopyTableList(1).GetColumnList, My.Settings.Fit_MultipleFiles_Set2_LastCol_X)
        Me.cbY_Set2.InitializeColumns(Me._SpectroscopyTableList(1).GetColumnList, My.Settings.Fit_MultipleFiles_Set2_LastCol_Y)

        ' Add the Spectroscopy-Table to the preview-display.
        Me.pbPreview_Set1.AddSpectroscopyTable(Me._SpectroscopyTableList(0))
        Me.pbPreview_Set2.AddSpectroscopyTable(Me._SpectroscopyTableList(1))
        Me.SpectroscopyColumnSelectionChangedX_Set1()
        Me.SpectroscopyColumnSelectionChangedY_Set1()
        Me.SpectroscopyColumnSelectionChangedX_Set2()
        Me.SpectroscopyColumnSelectionChangedY_Set2()

        ' Change the title of the window.
        Me.Text &= Me._SpectroscopyTableList(0).FileNameWithoutPath & " & " & Me._SpectroscopyTableList(1).FileNameWithoutPath
    End Sub

    ''' <summary>
    ''' Selected columns change
    ''' </summary>
    Public Sub SpectroscopyColumnSelectionChangedX_Set1() Handles cbX_Set1.SelectedIndexChanged
        Me.CalculatePreviewImage(FitFunctionSets.Set1)
        Me.pbPreview_Set1.cbX.SelectedEntry = Me.cbX_Set1.SelectedColumnName

        ' Save to settings for the next time the window opens
        My.Settings.Fit_MultipleFiles_Set1_LastCol_X = Me.cbX_Set1.SelectedColumnName
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Selected columns change
    ''' </summary>
    Public Sub SpectroscopyColumnSelectionChangedY_Set1() Handles cbY_Set1.SelectedIndexChanged
        Me.CalculatePreviewImage(FitFunctionSets.Set1)
        Me.pbPreview_Set1.cbY.SelectedEntry = Me.cbY_Set1.SelectedColumnName

        ' Save to settings for the next time the window opens
        My.Settings.Fit_MultipleFiles_Set1_LastCol_Y = Me.cbY_Set1.SelectedColumnName
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Selected columns change
    ''' </summary>
    Public Sub SpectroscopyColumnSelectionChangedX_Set2() Handles cbX_Set2.SelectedIndexChanged
        Me.CalculatePreviewImage(FitFunctionSets.Set2)
        Me.pbPreview_Set2.cbX.SelectedEntry = Me.cbX_Set2.SelectedColumnName

        ' Save to settings for the next time the window opens
        My.Settings.Fit_MultipleFiles_Set2_LastCol_X = Me.cbX_Set2.SelectedColumnName
        My.Settings.Save()
    End Sub

    ''' <summary>
    ''' Selected columns change
    ''' </summary>
    Public Sub SpectroscopyColumnSelectionChangedY_Set2() Handles cbY_Set2.SelectedIndexChanged
        Me.CalculatePreviewImage(FitFunctionSets.Set2)
        Me.pbPreview_Set2.cbY.SelectedEntry = Me.cbY_Set2.SelectedColumnName

        ' Save to settings for the next time the window opens
        My.Settings.Fit_MultipleFiles_Set2_LastCol_Y = Me.cbY_Set2.SelectedColumnName
        My.Settings.Save()
    End Sub
#End Region

#Region "Fit-Function"

    ''' <summary>
    ''' Fit-Function Container
    ''' </summary>
    Private FitFunction_Set1 As iFitFunction

    ''' <summary>
    ''' Fit-Function Container
    ''' </summary>
    Private FitFunction_Set2 As iFitFunction

    ''' <summary>
    ''' Different Fit-Functions.
    ''' </summary>
    Private FitFunctionSettings_Set1 As New List(Of cFitSettingPanel)

    ''' <summary>
    ''' Different Fit-Functions.
    ''' </summary>
    Private FitFunctionSettings_Set2 As New List(Of cFitSettingPanel)

    ''' <summary>
    ''' Marker for the data-sets.
    ''' </summary>
    Protected Enum FitFunctionSets
        Set1
        Set2
    End Enum

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
    Private WithEvents FitProcedure As New cFitProcedureMultipleDataFit

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
                If KV.Key.Name = My.Settings.Fit_MultipleFiles_Procedure Then
                    .SelectedIndex = i ' Select last used fit-procedure
                End If
            Next
            If .SelectedIndex < 0 Then .SelectedIndex = 0
        End With

        ' Fit the fit-function combobox
        With Me.cboAddFitModel
            .Items.Clear()

            Dim ShowInternalFitFunction As Boolean = System.IO.File.Exists("C:\spectrafox-internal.txt")

            ' Add all Fit-Functions to the selection
            For Each RegisteredFitFunctionTypeKV As KeyValuePair(Of Type, Boolean) In Me.RegisteredFitFunctions
#If Not Debug Then
                If RegisteredFitFunctionTypeKV.Value Or ShowInternalFitFunction Then
#End If
                .Items.Add(New KeyValuePair(Of Type, String)(RegisteredFitFunctionTypeKV.Key,
                                                             cFitFunction.GetFitFunctionByType(RegisteredFitFunctionTypeKV.Key).FitFunctionName))
#If Not Debug Then
                End If
#End If
            Next
            .ValueMember = "Key"
            .DisplayMember = "Value"
            .Sorted = True
            For i As Integer = 0 To .Items.Count - 1 Step 1
                Dim KV As KeyValuePair(Of Type, String) = DirectCast(.Items(i), KeyValuePair(Of Type, String))
                If KV.Key.Name = My.Settings.Fit_MultipleFiles_Model Then
                    .SelectedIndex = i ' Select last used fit-procedure
                End If
            Next
        End With

        ' Apply last settings.
        Me.SetFitRange_Set1(My.Settings.Fit_MultipleFiles_Set1_FitRange_LeftValue, My.Settings.Fit_MultipleFiles_Set1_FitRange_RightValue)
        Me.SetFitRange_Set2(My.Settings.Fit_MultipleFiles_Set2_FitRange_LeftValue, My.Settings.Fit_MultipleFiles_Set2_FitRange_RightValue)

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
    Private Sub btnAddFitModel_Click(sender As Object, e As EventArgs) Handles btnAddFitModel_Set1.Click, btnAddFitModel_Set2.Click

        ' Save last used to settings
        Dim SelectedFitFunctionType As Type = DirectCast(Me.cboAddFitModel.SelectedItem, KeyValuePair(Of Type, String)).Key
        My.Settings.Fit_MultipleFiles_Model = SelectedFitFunctionType.Name
        My.Settings.Save()

        ' Add Fit-Function to set 1
        If sender Is btnAddFitModel_Set1 Then
            Me.AddFitFunction(Me.GetSelectedFitFunctionToAdd, FitFunctionSets.Set1)
        End If

        ' Add Fit-Function to set 2
        If sender Is btnAddFitModel_Set2 Then
            Me.AddFitFunction(Me.GetSelectedFitFunctionToAdd, FitFunctionSets.Set2)
        End If


    End Sub

    ''' <summary>
    ''' Adds a Fit-Function-Settings-Panel to the list.
    ''' </summary>
    Private Function AddFitFunction(FitFunction As iFitFunction, ToSet As FitFunctionSets) As Boolean

        ' Add a specific SettingPanel
        Dim SettingPanel As cFitSettingPanel = FitFunction.GetFunctionSettingPanel

        ' Check, if Settingspanel got loaded
        If SettingPanel Is Nothing Then Return False

        ' Set the multi-threading options
        FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings

        Dim Set_PreviewBox As mSpectroscopyTableViewer = Nothing
        Dim Set_SettingPanels As List(Of cFitSettingPanel) = Nothing
        Dim Set_FitFunction As iFitFunction = Nothing

        Select Case ToSet

            Case FitFunctionSets.Set1

                ' Set the event-handlers
                AddHandler SettingPanel.XRangeSelectionRequest, AddressOf Me.SelectXRange_Set1
                AddHandler SettingPanel.YRangeSelectionRequest, AddressOf Me.SelectYRange_Set1
                AddHandler SettingPanel.XValueSelectionRequest, AddressOf Me.SelectXValue_Set1
                AddHandler SettingPanel.YValueSelectionRequest, AddressOf Me.SelectYValue_Set1
                AddHandler SettingPanel.RequestRemovalOfFitFunction, AddressOf Me.RemoveFitFunction_Set1
                AddHandler SettingPanel.ParameterChanged, AddressOf Me.CalculatePreviewImage_Set1
                AddHandler SettingPanel.SubParametersAddedOrRemoved, AddressOf Me.RebindFitParameters

                ' Check if the fit-function is ok with being initialized at the current fit-range
                Dim FitRangeLeft_SuggestedByFitFunction As Double = Me.FitRangeLeft_Set1
                Dim FitRangeRight_SuggestedByFitFunction As Double = Me.FitRangeRight_Set1
                If Not FitFunction.FitFunctionSuggestsDifferentFitRange(FitRangeLeft_SuggestedByFitFunction, FitRangeRight_SuggestedByFitFunction) Then
                    Dim SuggestionResult As DialogResult = MessageBox.Show(My.Resources.rFitBase.FitFunction_InitialFitRangeCheck _
                                                                           .Replace("%l", cUnits.GetPrefix(FitRangeLeft_SuggestedByFitFunction).Value & cUnits.GetPrefix(FitRangeLeft_SuggestedByFitFunction).Key) _
                                                                           .Replace("%h", cUnits.GetPrefix(FitRangeRight_SuggestedByFitFunction).Value & cUnits.GetPrefix(FitRangeRight_SuggestedByFitFunction).Key),
                                                                           My.Resources.rFitBase.FitFunction_InitialFitRangeCheck_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If SuggestionResult = DialogResult.Yes Then
                        ' change the fit-range to the suggested values
                        Me.FitRangeLeft_Set1 = FitRangeLeft_SuggestedByFitFunction
                        Me.FitRangeRight_Set1 = FitRangeRight_SuggestedByFitFunction
                    End If
                End If

                ' Register the Fit-Range-Changed-Event, and set the fit-range
                AddHandler Me.FitRangeSelectionChanged, AddressOf SettingPanel.FitRangeChanged
                SettingPanel.SetInitializationFitRange(Me.FitRangeLeft_Set1, Me.FitRangeRight_Set1)

                ' Adapt the width of the settings-panel to the Flow-Layout container:
                SettingPanel.Width = Me.flpFitModels_Set1.Width - FLPWidthOffset

                ' Add the Settings-Panel to the Flow-Layout-Stack
                Me.flpFitModels_Set1.Controls.Add(SettingPanel)

                ' Set References
                Set_SettingPanels = Me.FitFunctionSettings_Set1
                Set_FitFunction = Me.FitFunction_Set1
                Set_PreviewBox = Me.pbPreview_Set1

            Case FitFunctionSets.Set2

                ' Set the event-handlers
                AddHandler SettingPanel.XRangeSelectionRequest, AddressOf Me.SelectXRange_Set2
                AddHandler SettingPanel.YRangeSelectionRequest, AddressOf Me.SelectYRange_Set2
                AddHandler SettingPanel.XValueSelectionRequest, AddressOf Me.SelectXValue_Set2
                AddHandler SettingPanel.YValueSelectionRequest, AddressOf Me.SelectYValue_Set2
                AddHandler SettingPanel.RequestRemovalOfFitFunction, AddressOf Me.RemoveFitFunction_Set2
                AddHandler SettingPanel.ParameterChanged, AddressOf Me.CalculatePreviewImage_Set2
                AddHandler SettingPanel.SubParametersAddedOrRemoved, AddressOf Me.RebindFitParameters

                ' Check if the fit-function is ok with being initialized at the current fit-range
                Dim FitRangeLeft_SuggestedByFitFunction As Double = Me.FitRangeLeft_Set2
                Dim FitRangeRight_SuggestedByFitFunction As Double = Me.FitRangeRight_Set2
                If Not FitFunction.FitFunctionSuggestsDifferentFitRange(FitRangeLeft_SuggestedByFitFunction, FitRangeRight_SuggestedByFitFunction) Then
                    Dim SuggestionResult As DialogResult = MessageBox.Show(My.Resources.rFitBase.FitFunction_InitialFitRangeCheck _
                                                                           .Replace("%l", cUnits.GetPrefix(FitRangeLeft_SuggestedByFitFunction).Value & cUnits.GetPrefix(FitRangeLeft_SuggestedByFitFunction).Key) _
                                                                           .Replace("%h", cUnits.GetPrefix(FitRangeRight_SuggestedByFitFunction).Value & cUnits.GetPrefix(FitRangeRight_SuggestedByFitFunction).Key),
                                                                           My.Resources.rFitBase.FitFunction_InitialFitRangeCheck_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If SuggestionResult = DialogResult.Yes Then
                        ' change the fit-range to the suggested values
                        Me.FitRangeLeft_Set2 = FitRangeLeft_SuggestedByFitFunction
                        Me.FitRangeRight_Set2 = FitRangeRight_SuggestedByFitFunction
                    End If
                End If

                ' Register the Fit-Range-Changed-Event, and set the fit-range
                AddHandler Me.FitRangeSelectionChanged, AddressOf SettingPanel.FitRangeChanged
                SettingPanel.SetInitializationFitRange(Me.FitRangeLeft_Set2, Me.FitRangeRight_Set2)

                ' Adapt the width of the settings-panel to the Flow-Layout container:
                SettingPanel.Width = Me.flpFitModels_Set2.Width - FLPWidthOffset

                ' Add the Settings-Panel to the Flow-Layout-Stack
                Me.flpFitModels_Set2.Controls.Add(SettingPanel)

                ' Set References
                Set_SettingPanels = Me.FitFunctionSettings_Set2
                Set_FitFunction = Me.FitFunction_Set2
                Set_PreviewBox = Me.pbPreview_Set2

            Case Else
                Return False

        End Select

        '########################################################
        ' Generate individual identifier for the Settings-Panel
        Dim Identifier As String = SettingPanel.FitFunction.FitFunctionName
        Dim i As Integer = 1

        ' Go through all existing setting-panels, and rename them.
        For Each ExistingSettingPanel As cFitSettingPanel In Set_SettingPanels
            If ExistingSettingPanel.FitFunctionName = SettingPanel.FitFunctionName Then

                Dim NewIdentifier As String = Identifier & " #" & i.ToString

                ' Add the same Spectroscopy-Table under the new reference
                If Set_PreviewBox.CurrentSpectroscopyTables.ContainsKey(ExistingSettingPanel.Identifier) Then
                    ' Otherwise there is a failure in the plotting
                    Dim SpectroscopyTable As cSpectroscopyTable = Set_PreviewBox.CurrentSpectroscopyTables(ExistingSettingPanel.Identifier)
                    SpectroscopyTable.FullFileName = NewIdentifier
                    Set_PreviewBox.RemoveSpectroscopyTable(ExistingSettingPanel.Identifier)
                    Set_PreviewBox.AddSpectroscopyTable(SpectroscopyTable)
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
        With Set_SettingPanels
            .Add(SettingPanel)

            ' If more than one settings-panel exists, lock the YOffset of all
            ' of them, except for the first one! Otherwise the algorithm
            ' will get a problem optimizing the multiple YOffsets.
            If .Count > 1 Then
                .Item(0).LockFitParametersForMultipleFit = False
                For j As Integer = 1 To .Count - 1 Step 1
                    .Item(j).LockFitParametersForMultipleFit = True
                Next
            ElseIf .Count = 1 Then
                .Item(0).LockFitParametersForMultipleFit = False
            End If
        End With

        ' Set the current Fit-Function object
        Me.SetFitFunction(ToSet)

        ' refresh preview image
        Me.CalculatePreviewImage(ToSet)

        ' Enable the Fit-Start button
        If Me.FitFunctionSettings_Set1.Count > 0 And Me.FitFunctionSettings_Set2.Count > 0 Then
            Me.btnStartFitting.Enabled = True
        End If

        Return True
    End Function

    ''' <summary>
    ''' Sets the Me.FitFunction by the given Fit-Function configuration
    ''' </summary>
    Protected Sub SetFitFunction(ToSet As FitFunctionSets)

        Dim Set_SettingPanels As List(Of cFitSettingPanel) = Nothing
        Dim Set_FitFunction As iFitFunction = Nothing

        Select Case ToSet

            Case FitFunctionSets.Set1

                ' Set References
                Set_SettingPanels = Me.FitFunctionSettings_Set1
                Set_FitFunction = Me.FitFunction_Set1

            Case FitFunctionSets.Set2

                ' Set References
                Set_SettingPanels = Me.FitFunctionSettings_Set2
                Set_FitFunction = Me.FitFunction_Set2

        End Select

        ' Decide whether to use a multiple-peak function
        ' or to perform a direct fit, if only a single FitFunction has been selected
        Select Case Set_SettingPanels.Count
            Case 1
                ' Single-Peak
                '#############

                ' Set to the first and only selected Fit-Function
                Set_FitFunction = Set_SettingPanels(0).FitFunction

                ' Set the multi-threading options
                Set_FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings
            Case Is > 1
                ' Multiple-Peaks
                '################

                ' Summarize all Fit-Functions in a Multiple-Peak-Fit-Function
                Dim MultiplePeakContainer As New cFitFunction_MultiplePeaks

                ' Go through all Fit-Functions
                For Each FitFunctionSetting As cFitSettingPanel In Set_SettingPanels
                    ' Set the multi-threading options
                    FitFunctionSetting.FitFunction.MultiThreadingOptions = Me.MultiThreadingSettings

                    ' Add the Fit-Function to the Multiple-Peak-Fit-Container
                    MultiplePeakContainer.FitFunctions.Add(FitFunctionSetting.FitFunction)
                Next

                ' Initialize the multiple-peak fit-parameters:
                MultiplePeakContainer.ReInitializeFitParameters()

                ' Register the Multiple-Peak-Function
                Set_FitFunction = MultiplePeakContainer
        End Select

        ' Write back the current fit-function
        If ToSet = FitFunctionSets.Set1 Then
            Me.FitFunction_Set1 = Set_FitFunction
        Else
            Me.FitFunction_Set2 = Set_FitFunction
        End If

        ' Rebind all fit-parameters in both sets
        Me.RebindFitParameters()

    End Sub

    ''' <summary>
    ''' Rebinds all fit-parameters to the boxes. Needed to have all Lock-To selections
    ''' </summary>
    Public Sub RebindFitParameters()
        ' Rebind all fit-parameters in both sets
        Dim CombinedFitParameterCollection As New cFitParameterGroupGroup
        If Not Me.FitFunction_Set1 Is Nothing Then CombinedFitParameterCollection.Add(Me.FitFunction_Set1.FitParametersGroupedWithoutCombinedGroup)
        If Not Me.FitFunction_Set2 Is Nothing Then CombinedFitParameterCollection.Add(Me.FitFunction_Set2.FitParametersGroupedWithoutCombinedGroup)
        For Each SP As cFitSettingPanel In Me.FitFunctionSettings_Set1
            SP.SetFitParameterGroups(CombinedFitParameterCollection)
        Next
        For Each SP As cFitSettingPanel In Me.FitFunctionSettings_Set2
            SP.SetFitParameterGroups(CombinedFitParameterCollection)
        Next
    End Sub

    ''' <summary>
    ''' Wrapper.
    ''' </summary>
    Private Sub RemoveFitFunction_Set1(ByRef FitSettingsPanel As cFitSettingPanel)
        Me.RemoveFitFunction(FitSettingsPanel, FitFunctionSets.Set1)
    End Sub

    ''' <summary>
    ''' Wrapper.
    ''' </summary>
    Private Sub RemoveFitFunction_Set2(ByRef FitSettingsPanel As cFitSettingPanel)
        Me.RemoveFitFunction(FitSettingsPanel, FitFunctionSets.Set2)
    End Sub

    ''' <summary>
    ''' Removes the given FitSettingsPanel from the list.
    ''' </summary>
    Private Sub RemoveFitFunction(ByRef FitSettingsPanel As cFitSettingPanel, FromSet As FitFunctionSets)

        Select Case FromSet

            Case FitFunctionSets.Set1
                ' Remove from the flow-layout panel
                Me.flpFitModels_Set1.Controls.Remove(FitSettingsPanel)

                With Me.FitFunctionSettings_Set1
                    ' Remove from central list
                    .Remove(FitSettingsPanel)

                    RemoveHandler FitSettingsPanel.XRangeSelectionRequest, AddressOf Me.SelectXRange_Set1
                    RemoveHandler FitSettingsPanel.YRangeSelectionRequest, AddressOf Me.SelectYRange_Set1
                    RemoveHandler FitSettingsPanel.XValueSelectionRequest, AddressOf Me.SelectXValue_Set1
                    RemoveHandler FitSettingsPanel.YValueSelectionRequest, AddressOf Me.SelectYValue_Set1
                    RemoveHandler FitSettingsPanel.RequestRemovalOfFitFunction, AddressOf Me.RemoveFitFunction_Set1
                    RemoveHandler FitSettingsPanel.ParameterChanged, AddressOf Me.CalculatePreviewImage_Set1
                    RemoveHandler FitSettingsPanel.SubParametersAddedOrRemoved, AddressOf Me.RebindFitParameters

                    ' Check again for the lock of the YOffset:
                    ' If more than one settings-panel exists, lock the YOffset of all
                    ' of them, except for the first one! Otherwise the algorithm
                    ' will get a problem optimizing the multiple YOffsets.
                    If .Count > 1 Then
                        .Item(0).LockFitParametersForMultipleFit = False
                        For j As Integer = 1 To .Count - 1 Step 1
                            .Item(j).LockFitParametersForMultipleFit = True
                        Next
                    ElseIf .Count = 1 Then
                        .Item(0).LockFitParametersForMultipleFit = False
                    End If
                End With

                ' Remove from preview-box
                Me.pbPreview_Set1.RemoveSpectroscopyTable(FitSettingsPanel.Identifier)

            Case FitFunctionSets.Set2
                ' Remove from the flow-layout panel
                Me.flpFitModels_Set2.Controls.Remove(FitSettingsPanel)

                With Me.FitFunctionSettings_Set2
                    ' Remove from central list
                    .Remove(FitSettingsPanel)

                    RemoveHandler FitSettingsPanel.XRangeSelectionRequest, AddressOf Me.SelectXRange_Set2
                    RemoveHandler FitSettingsPanel.YRangeSelectionRequest, AddressOf Me.SelectYRange_Set2
                    RemoveHandler FitSettingsPanel.XValueSelectionRequest, AddressOf Me.SelectXValue_Set2
                    RemoveHandler FitSettingsPanel.YValueSelectionRequest, AddressOf Me.SelectYValue_Set2
                    RemoveHandler FitSettingsPanel.RequestRemovalOfFitFunction, AddressOf Me.RemoveFitFunction_Set2
                    RemoveHandler FitSettingsPanel.ParameterChanged, AddressOf Me.CalculatePreviewImage_Set2
                    RemoveHandler FitSettingsPanel.SubParametersAddedOrRemoved, AddressOf Me.RebindFitParameters

                    ' Check again for the lock of the YOffset:
                    ' If more than one settings-panel exists, lock the YOffset of all
                    ' of them, except for the first one! Otherwise the algorithm
                    ' will get a problem optimizing the multiple YOffsets.
                    If .Count > 1 Then
                        .Item(0).LockFitParametersForMultipleFit = False
                        For j As Integer = 1 To .Count - 1 Step 1
                            .Item(j).LockFitParametersForMultipleFit = True
                        Next
                    ElseIf .Count = 1 Then
                        .Item(0).LockFitParametersForMultipleFit = False
                    End If
                End With

                ' Remove from preview-box
                Me.pbPreview_Set2.RemoveSpectroscopyTable(FitSettingsPanel.Identifier)

        End Select

        ' Clear all memory traces.
        FitSettingsPanel.Dispose()

        ' Set the FitFunction object
        Me.SetFitFunction(FromSet)

        ' refresh preview image
        Me.CalculatePreviewImage(FromSet)

        ' Disable the Fit-Start button, if no Fit-Model is left
        If Me.FitFunctionSettings_Set1.Count <= 0 Or Me.FitFunctionSettings_Set2.Count <= 0 Then
            Me.btnStartFitting.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' This sub removes all fit-functions simultaneously.
    ''' </summary>
    Private Sub ClearAllFitFunctions(FromSet As FitFunctionSets)

        Select Case FromSet
            Case FitFunctionSets.Set1
                Me.FitFunctionSettings_Set1.Clear()
                For Each C As Control In Me.flpFitModels_Set1.Controls
                    C.Dispose()
                Next
                Me.flpFitModels_Set1.Controls.Clear()

            Case FitFunctionSets.Set2
                Me.FitFunctionSettings_Set2.Clear()
                For Each C As Control In Me.flpFitModels_Set2.Controls
                    C.Dispose()
                Next
                Me.flpFitModels_Set2.Controls.Clear()

        End Select

        ' Refresh preview image
        Me.CalculatePreviewImage(FromSet)
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
        Me.CalculatePreviewImage(FitFunctionSets.Set1)
        Me.CalculatePreviewImage(FitFunctionSets.Set2)
    End Sub

    ''' <summary>
    ''' Apply the preview-point number by pressing enter.
    ''' This is usually done by the user, instead of clicking the button.
    ''' </summary>
    Private Sub nudPreviewPoints_KeyPress(sender As Object, e As KeyEventArgs) Handles nudPreviewPoints.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.PreviewPoints = Convert.ToInt32(Me.nudPreviewPoints.Value)
            Me.CalculatePreviewImage(FitFunctionSets.Set1)
            Me.CalculatePreviewImage(FitFunctionSets.Set2)
        End If
    End Sub
#End Region

#Region "Preview Drawing"

    ''' <summary>
    ''' Wrappers for EventHandlers
    ''' </summary>
    Private Sub CalculatePreviewImage_Set1()
        Me.CalculatePreviewImage(FitFunctionSets.Set1)
    End Sub

    ''' <summary>
    ''' Wrappers for EventHandlers
    ''' </summary>
    Private Sub CalculatePreviewImage_Set2()
        Me.CalculatePreviewImage(FitFunctionSets.Set2)
    End Sub

    ''' <summary>
    ''' Calculates the fit-function-preview and adds the 
    ''' result as a newly created Spectroscopy-Table object to the
    ''' SpectroscopyTableViewer.
    ''' 
    ''' This is done for each Fit-Function.
    ''' </summary>
    Private Sub CalculatePreviewImage(ForSet As FitFunctionSets)

        If Me._SpectroscopyTableList.Count <= 0 Then Return

        Select Case ForSet

            Case FitFunctionSets.Set1
                ' Create equally spaced fit-function-data, if the fit-range is set
                If Me.FitRangeLeft_Set1 < Me.FitRangeRight_Set1 Then

                    Me.pbPreview_Set1.ClearSpectroscopyTables()
                    Me.pbPreview_Set1.AddSpectroscopyTable(Me._SpectroscopyTableList(0))

                    ' Preview-FitFunction-Array
                    Dim PreviewSpectroscopyTables As New List(Of cSpectroscopyTable)

                    Dim SpectroscopyTable As cSpectroscopyTable
                    Dim xColumn As cSpectroscopyTable.DataColumn
                    Dim yColumn As cSpectroscopyTable.DataColumn

                    ' Go through the FitFunction array.
                    For Each SettingsPanel As cFitSettingPanel In Me.FitFunctionSettings_Set1
                        SpectroscopyTable = New cSpectroscopyTable

                        ' Set the name of the SpectroscopyTable to the FitFunction
                        SpectroscopyTable.FullFileName = SettingsPanel.Identifier

                        ' Create the columns and set the names to the currently displayed column-names
                        xColumn = New cSpectroscopyTable.DataColumn
                        yColumn = New cSpectroscopyTable.DataColumn
                        xColumn.Name = Me.cbX_Set1.SelectedColumnName
                        yColumn.Name = Me.cbY_Set1.SelectedColumnName

                        ' Create the data for the yColumn and the specific fitfunction.
                        For n As Integer = 0 To Me._PreviewPoints - 1 Step 1
                            ' Generate Data
                            xColumn.AddValueToColumn(Me.FitRangeLeft_Set1 + (Me.FitRangeRight_Set1 - Me.FitRangeLeft_Set1) / PreviewPoints * n)
                            yColumn.AddValueToColumn(SettingsPanel.FitFunction.GetY(xColumn.Values.Last, SettingsPanel.FitFunction.FitParametersGrouped))
                        Next

                        ' Add the columns to the Preview-Box
                        SpectroscopyTable.AddNonPersistentColumn(xColumn)
                        SpectroscopyTable.AddNonPersistentColumn(yColumn)

                        PreviewSpectroscopyTables.Add(SpectroscopyTable)
                    Next

                    ' Finally add a combined Spectroscopy-Table, if there is more than one fit-function
                    If Me.FitFunctionSettings_Set1.Count > 1 Then

                        ' Create new SpectroscopyTable-Object
                        SpectroscopyTable = New cSpectroscopyTable

                        ' Set the name of the SpectroscopyTable to the FitFunction
                        SpectroscopyTable.FullFileName = My.Resources.rFitFunction_MultiplePeaks.CombinedCurveName

                        ' Create a separate YColumn for the summed up data,
                        ' to show for multiple fit-functions a combined preview.
                        xColumn = New cSpectroscopyTable.DataColumn
                        yColumn = New cSpectroscopyTable.DataColumn
                        xColumn.Name = Me.cbX_Set1.SelectedColumnName
                        yColumn.Name = Me.cbY_Set1.SelectedColumnName

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
                        Me.pbPreview_Set1.RemoveSpectroscopyTable(My.Resources.rFitBase.CurveTitle_MultipleFit)
                    End If

                    ' Avoid the restore of the scale, because parameter-selection gets really horrible by this.
                    Me.pbPreview_Set1.AutomaticallyRestoreScaleAfterRedraw = False

                    ' Add the Spectroscopy-Table-List to the Preview-Box
                    Me.pbPreview_Set1.AddSpectroscopyTables(PreviewSpectroscopyTables)
                    'Me.pbPreview.cbX.SelectedColumnName = Me.cbX.SelectedColumnName
                    Me.pbPreview_Set1.cbY.SelectedColumnName = Me.cbY_Set1.SelectedColumnName

                    ' Activate the restore of the scale again.
                    Me.pbPreview_Set1.AutomaticallyRestoreScaleAfterRedraw = True
                End If

            Case FitFunctionSets.Set2
                ' Create equally spaced fit-function-data, if the fit-range is set
                If Me.FitRangeLeft_Set2 < Me.FitRangeRight_Set2 Then

                    Me.pbPreview_Set2.ClearSpectroscopyTables()
                    Me.pbPreview_Set2.AddSpectroscopyTable(Me._SpectroscopyTableList(1))

                    ' Preview-FitFunction-Array
                    Dim PreviewSpectroscopyTables As New List(Of cSpectroscopyTable)

                    Dim SpectroscopyTable As cSpectroscopyTable
                    Dim xColumn As cSpectroscopyTable.DataColumn
                    Dim yColumn As cSpectroscopyTable.DataColumn

                    ' Go through the FitFunction array.
                    For Each SettingsPanel As cFitSettingPanel In Me.FitFunctionSettings_Set2
                        SpectroscopyTable = New cSpectroscopyTable

                        ' Set the name of the SpectroscopyTable to the FitFunction
                        SpectroscopyTable.FullFileName = SettingsPanel.Identifier

                        ' Create the columns and set the names to the currently displayed column-names
                        xColumn = New cSpectroscopyTable.DataColumn
                        yColumn = New cSpectroscopyTable.DataColumn
                        xColumn.Name = Me.cbX_Set2.SelectedColumnName
                        yColumn.Name = Me.cbY_Set2.SelectedColumnName

                        ' Create the data for the yColumn and the specific fitfunction.
                        For n As Integer = 0 To Me._PreviewPoints - 1 Step 1
                            ' Generate Data
                            xColumn.AddValueToColumn(Me.FitRangeLeft_Set2 + (Me.FitRangeRight_Set2 - Me.FitRangeLeft_Set2) / PreviewPoints * n)
                            yColumn.AddValueToColumn(SettingsPanel.FitFunction.GetY(xColumn.Values.Last, SettingsPanel.FitFunction.FitParametersGrouped))
                        Next

                        ' Add the columns to the Preview-Box
                        SpectroscopyTable.AddNonPersistentColumn(xColumn)
                        SpectroscopyTable.AddNonPersistentColumn(yColumn)

                        PreviewSpectroscopyTables.Add(SpectroscopyTable)
                    Next

                    ' Finally add a combined Spectroscopy-Table, if there is more than one fit-function
                    If Me.FitFunctionSettings_Set2.Count > 1 Then

                        ' Create new SpectroscopyTable-Object
                        SpectroscopyTable = New cSpectroscopyTable

                        ' Set the name of the SpectroscopyTable to the FitFunction
                        SpectroscopyTable.FullFileName = My.Resources.rFitFunction_MultiplePeaks.CombinedCurveName

                        ' Create a separate YColumn for the summed up data,
                        ' to show for multiple fit-functions a combined preview.
                        xColumn = New cSpectroscopyTable.DataColumn
                        yColumn = New cSpectroscopyTable.DataColumn
                        xColumn.Name = Me.cbX_Set2.SelectedColumnName
                        yColumn.Name = Me.cbY_Set2.SelectedColumnName

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
                        Me.pbPreview_Set2.RemoveSpectroscopyTable(My.Resources.rFitBase.CurveTitle_MultipleFit)
                    End If

                    ' Avoid the restore of the scale, because parameter-selection gets really horrible by this.
                    Me.pbPreview_Set2.AutomaticallyRestoreScaleAfterRedraw = False

                    ' Add the Spectroscopy-Table-List to the Preview-Box
                    Me.pbPreview_Set2.AddSpectroscopyTables(PreviewSpectroscopyTables)
                    'Me.pbPreview.cbX.SelectedColumnName = Me.cbX.SelectedColumnName
                    Me.pbPreview_Set2.cbY.SelectedColumnName = Me.cbY_Set2.SelectedColumnName

                    ' Activate the restore of the scale again.
                    Me.pbPreview_Set2.AutomaticallyRestoreScaleAfterRedraw = True
                End If

        End Select

    End Sub

#End Region

#Region "Range-selection for easier parameter modification"

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectXRange_Set1(Callback As mSpectroscopyTableViewer.XRangeSelectionCallback)
        Me.pbPreview_Set1.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set1.CallbackXRangeSelected = Callback
        Me.pbPreview_Set1.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XRange
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectYRange_Set1(Callback As mSpectroscopyTableViewer.YRangeSelectionCallback)
        Me.pbPreview_Set1.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set1.CallbackYRangeSelected = Callback
        Me.pbPreview_Set1.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.YRange
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectXValue_Set1(Callback As mSpectroscopyTableViewer.XValueSelectionCallback)
        Me.pbPreview_Set1.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set1.CallbackXValueSelected = Callback
        Me.pbPreview_Set1.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XValue
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectYValue_Set1(Callback As mSpectroscopyTableViewer.YValueSelectionCallback)
        Me.pbPreview_Set1.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set1.CallbackYValueSelected = Callback
        Me.pbPreview_Set1.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.YValue
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectXRange_Set2(Callback As mSpectroscopyTableViewer.XRangeSelectionCallback)
        Me.pbPreview_Set2.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set2.CallbackXRangeSelected = Callback
        Me.pbPreview_Set2.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XRange
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectYRange_Set2(Callback As mSpectroscopyTableViewer.YRangeSelectionCallback)
        Me.pbPreview_Set2.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set2.CallbackYRangeSelected = Callback
        Me.pbPreview_Set2.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.YRange
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectXValue_Set2(Callback As mSpectroscopyTableViewer.XValueSelectionCallback)
        Me.pbPreview_Set2.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set2.CallbackXValueSelected = Callback
        Me.pbPreview_Set2.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XValue
    End Sub

    ''' <summary>
    ''' Processes the event of a settings-panel to select certain values.
    ''' </summary>
    Private Sub SelectYValue_Set2(Callback As mSpectroscopyTableViewer.YValueSelectionCallback)
        Me.pbPreview_Set2.ClearPointSelectionModeAfterSelection = True
        Me.pbPreview_Set2.CallbackYValueSelected = Callback
        Me.pbPreview_Set2.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.YValue
    End Sub

#End Region

#Region "Handle the stacking of the flow-layout-panel"
    Private FLPWidthOffset As Short = 15
    Private Sub flpFitModels_Resize(sender As Object, e As EventArgs) Handles flpFitModels_Set1.Resize
        ' Set the width of all sub-controls to the width of the settings-panel
        For Each C As Control In Me.flpFitModels_Set1.Controls
            C.Width = Me.flpFitModels_Set1.Width - FLPWidthOffset
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
    Private Sub txtFitRange_Set1_TextChanged(ByRef Sender As NumericTextbox) Handles txtFitRange_LeftValue_Set1.ValidValueChanged, txtFitRange_RightValue_Set1.ValidValueChanged
        Me.SetFitRange_Set1(Me.txtFitRange_LeftValue_Set1.DecimalValue, Me.txtFitRange_RightValue_Set1.DecimalValue)
        Me.CalculatePreviewImage(FitFunctionSets.Set1)
    End Sub

    ''' <summary>
    ''' Entered Fit-Range changed
    ''' </summary>
    Private Sub txtFitRange_Set2_TextChanged(ByRef Sender As NumericTextbox) Handles txtFitRange_LeftValue_Set2.ValidValueChanged, txtFitRange_RightValue_Set2.ValidValueChanged
        Me.SetFitRange_Set2(Me.txtFitRange_LeftValue_Set2.DecimalValue, Me.txtFitRange_RightValue_Set2.DecimalValue)
        Me.CalculatePreviewImage(FitFunctionSets.Set2)
    End Sub


    ''' <summary>
    ''' Sets the range in which the Fit-Function is defined
    ''' </summary>
    Public Sub SetFitRange_Set1(LeftValue As Double, RightValue As Double)

        ' Set the content of the textboxes
        Me.txtFitRange_LeftValue_Set1.SetValue(LeftValue)
        Me.txtFitRange_RightValue_Set1.SetValue(RightValue)

        My.Settings.Fit_MultipleFiles_Set1_FitRange_LeftValue = LeftValue
        My.Settings.Fit_MultipleFiles_Set1_FitRange_RightValue = RightValue
        My.Settings.Save()

        RaiseEvent FitRangeSelectionChanged(LeftValue, RightValue)

        Me.CalculatePreviewImage(FitFunctionSets.Set1)
    End Sub

    ''' <summary>
    ''' Sets the range in which the Fit-Function is defined
    ''' </summary>
    Public Sub SetFitRange_Set2(LeftValue As Double, RightValue As Double)

        ' Set the content of the textboxes
        Me.txtFitRange_LeftValue_Set2.SetValue(LeftValue)
        Me.txtFitRange_RightValue_Set2.SetValue(RightValue)

        My.Settings.Fit_MultipleFiles_Set2_FitRange_LeftValue = LeftValue
        My.Settings.Fit_MultipleFiles_Set2_FitRange_RightValue = RightValue
        My.Settings.Save()

        RaiseEvent FitRangeSelectionChanged(LeftValue, RightValue)

        Me.CalculatePreviewImage(FitFunctionSets.Set2)
    End Sub

    ''' <summary>
    ''' Left limit of the fit-range.
    ''' </summary>
    Public Property FitRangeLeft_Set1 As Double
        Get
            Return Me.txtFitRange_LeftValue_Set1.DecimalValue
        End Get
        Set(value As Double)
            Me.txtFitRange_LeftValue_Set1.SetValue(value)
            My.Settings.Fit_MultipleFiles_Set1_FitRange_LeftValue = value
            My.Settings.Save()
            RaiseEvent FitRangeSelectionChanged(Me.FitRangeLeft_Set1, Me.FitRangeRight_Set1)
            Me.CalculatePreviewImage(FitFunctionSets.Set1)
        End Set
    End Property

    ''' <summary>
    ''' Right limit of the fit-range.
    ''' </summary>
    Public Property FitRangeRight_Set1 As Double
        Get
            Return Me.txtFitRange_RightValue_Set1.DecimalValue
        End Get
        Set(value As Double)
            Me.txtFitRange_RightValue_Set1.SetValue(value)
            My.Settings.Fit_MultipleFiles_Set1_FitRange_RightValue = value
            My.Settings.Save()
            RaiseEvent FitRangeSelectionChanged(Me.FitRangeLeft_Set1, Me.FitRangeRight_Set1)
            Me.CalculatePreviewImage(FitFunctionSets.Set1)
        End Set
    End Property

    ''' <summary>
    ''' Left limit of the fit-range.
    ''' </summary>
    Public Property FitRangeLeft_Set2 As Double
        Get
            Return Me.txtFitRange_LeftValue_Set2.DecimalValue
        End Get
        Set(value As Double)
            Me.txtFitRange_LeftValue_Set2.SetValue(value)
            My.Settings.Fit_MultipleFiles_Set2_FitRange_LeftValue = value
            My.Settings.Save()
            RaiseEvent FitRangeSelectionChanged(Me.FitRangeLeft_Set2, Me.FitRangeRight_Set2)
            Me.CalculatePreviewImage(FitFunctionSets.Set2)
        End Set
    End Property

    ''' <summary>
    ''' Right limit of the fit-range.
    ''' </summary>
    Public Property FitRangeRight_Set2 As Double
        Get
            Return Me.txtFitRange_RightValue_Set2.DecimalValue
        End Get
        Set(value As Double)
            Me.txtFitRange_RightValue_Set2.SetValue(value)
            My.Settings.Fit_MultipleFiles_Set2_FitRange_RightValue = value
            My.Settings.Save()
            RaiseEvent FitRangeSelectionChanged(Me.FitRangeLeft_Set2, Me.FitRangeRight_Set2)
            Me.CalculatePreviewImage(FitFunctionSets.Set2)
        End Set
    End Property

    ''' <summary>
    ''' Raise event, that the fit-range selection has been requested.
    ''' </summary>
    Private Sub btnSelectFitRange_Set1_Click(sender As Object, e As EventArgs) Handles btnSelectFitRange_Set1.Click
        Me.SelectXRange_Set1(AddressOf Me.SetFitRange_Set1)
    End Sub

    ''' <summary>
    ''' Raise event, that the fit-range selection has been requested.
    ''' </summary>
    Private Sub btnSelectFitRange_Set2_Click(sender As Object, e As EventArgs) Handles btnSelectFitRange_Set2.Click
        Me.SelectXRange_Set2(AddressOf Me.SetFitRange_Set2)
    End Sub
#End Region

#Region "Fit-Echo"
    ''' <summary>
    ''' Report-Function for getting the status of the Fit.
    ''' </summary>
    Private Sub ReportFunction(ByVal FitParameters As cFitParameterGroupGroup,
                               Chi2_Set1 As Double,
                               Chi2_Set2 As Double) Handles FitProcedure.FitStepEcho
        Dim sb As New System.Text.StringBuilder
        sb.Append(cFitParameter.GetShortParameterEcho(FitParameters))
        sb.Append(Now.ToString("hh:mm:ss"))
        sb.Append(" - Chi2: SET 1 >> ")
        sb.Append(Chi2_Set1.ToString(cFitParameter.NumericFormat))
        sb.Append(" << , SET 2 >> ")
        sb.Append(Chi2_Set2.ToString(cFitParameter.NumericFormat))
        sb.Append(" << , total Chi2 >> ")
        sb.Append((Chi2_Set1 + Chi2_Set2).ToString(cFitParameter.NumericFormat))
        sb.Append(" << ")

        ' Preview updaten!
        If Me.ckbUpdatePreviewDuringFit.Checked Then
            Me.WriteBackFitParameters(FitParameters, FitFunctionSets.Set1)
            Me.WriteBackFitParameters(FitParameters, FitFunctionSets.Set2)
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
            Me.txtFitEcho.Clear()
            'End If
        End If

        If Me.FitRangeLeft_Set1 >= Me.FitRangeRight_Set1 Or Me.FitRangeLeft_Set2 >= Me.FitRangeRight_Set2 Then
            HandleFitEcho("ERROR: fit aborted - fit-range is invalid")
            RaiseEvent FitFinishedEvent()
            Return
        End If

        ' Get the fit-data from the SpectroscopyTable
        Dim SourceCol_X_Set1 As cSpectroscopyTable.DataColumn = Me._SpectroscopyTableList(0).Column(Me.cbX_Set1.SelectedColumnName).GetCopy
        Dim SourceCol_Y_Set1 As cSpectroscopyTable.DataColumn = Me._SpectroscopyTableList(0).Column(Me.cbY_Set1.SelectedColumnName)
        '
        Dim SourceCol_X_Set2 As cSpectroscopyTable.DataColumn = Me._SpectroscopyTableList(1).Column(Me.cbX_Set2.SelectedColumnName).GetCopy
        Dim SourceCol_Y_Set2 As cSpectroscopyTable.DataColumn = Me._SpectroscopyTableList(1).Column(Me.cbY_Set2.SelectedColumnName)

        ' Check, if the Fit-Range is in the range of the fit-data
        ' or otherwise change it to be in there.
        If Me.FitRangeLeft_Set1 < SourceCol_X_Set1.GetMinimumValueOfColumn Then Me.FitRangeLeft_Set1 = SourceCol_X_Set1.GetMinimumValueOfColumn
        If Me.FitRangeRight_Set1 > SourceCol_X_Set1.GetMaximumValueOfColumn Then Me.FitRangeRight_Set1 = SourceCol_X_Set1.GetMaximumValueOfColumn
        '
        If Me.FitRangeLeft_Set2 < SourceCol_X_Set2.GetMinimumValueOfColumn Then Me.FitRangeLeft_Set2 = SourceCol_X_Set2.GetMinimumValueOfColumn
        If Me.FitRangeRight_Set2 > SourceCol_X_Set2.GetMaximumValueOfColumn Then Me.FitRangeRight_Set2 = SourceCol_X_Set2.GetMaximumValueOfColumn

        ' Cut down the data to the selected fit-range
        SourceCol_X_Set1.SetValuesOutsideRangeToNaN(Me.FitRangeLeft_Set1, Me.FitRangeRight_Set1)
        SourceCol_Y_Set1 = SourceCol_Y_Set1.GetColumnWithoutValuesWhereSourceColumnIsNaN(SourceCol_X_Set1)
        'SourceCol_X_Set1 = SourceCol_X_Set1.GetColumnWithoutCroppedData
        '
        SourceCol_X_Set2.SetValuesOutsideRangeToNaN(Me.FitRangeLeft_Set2, Me.FitRangeRight_Set2)
        SourceCol_Y_Set2 = SourceCol_Y_Set2.GetColumnWithoutValuesWhereSourceColumnIsNaN(SourceCol_X_Set2)
        'SourceCol_X_Set2 = SourceCol_X_Set2.GetColumnWithoutCroppedData

        ' Create the source array of double-values from the columns
        Dim FitDataPoints_Set1()() As Double = New Double()() {SourceCol_X_Set1.Values.ToArray, SourceCol_Y_Set1.Values.ToArray}
        Dim FitDataPoints_Set2()() As Double = New Double()() {SourceCol_X_Set2.Values.ToArray, SourceCol_Y_Set2.Values.ToArray}

        ' Check for FitFunction Is Nothing
        If Me.FitFunction_Set1 Is Nothing Then
            HandleFitEcho("ERROR: fit aborted - no fit-models present (SET 1)")
            RaiseEvent FitFinishedEvent()
            Return
        End If
        If Me.FitFunction_Set2 Is Nothing Then
            HandleFitEcho("ERROR: fit aborted - no fit-models present (SET 2)")
            RaiseEvent FitFinishedEvent()
            Return
        End If

        ' Set the range of the fit-function.
        ' This normally does nothing. But some fit-functions need this,
        ' to work properly, e.g. for current integrals.
        Me.FitFunction_Set1.ChangeFitRangeX(Me.FitRangeLeft_Set1, Me.FitRangeRight_Set1)
        Me.FitFunction_Set2.ChangeFitRangeX(Me.FitRangeLeft_Set2, Me.FitRangeRight_Set2)

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
        HandleFitEcho("source data (SET 1): " & Me._SpectroscopyTableList(0).FileNameWithoutPath)
        HandleFitEcho("source data (SET 2): " & Me._SpectroscopyTableList(1).FileNameWithoutPath)
        HandleFitEcho("-------- fit procedure settings ---------")
        HandleFitEcho("maximum iteration count: " & Me.FitProcedure.FitProcedureSettings_Set1.StopCondition_MaxIterations.ToString("N0"))
        HandleFitEcho("min change in Chi^2:     " & Me.FitProcedure.FitProcedureSettings_Set1.StopCondition_MinChi2Change.ToString("E3"))
        HandleFitEcho("")
        HandleFitEcho("---------- selected fit models: SET 1 ----------")
        HandleFitEcho("fit models: " & vbNewLine & Me.FitFunction_Set1.FitFunctionName)
        HandleFitEcho("fit formula: " & vbNewLine & Me.FitFunction_Set1.FitFunctionFormula)
        HandleFitEcho(Me.FitProcedure.FitProcedureSettings_Set1.EchoSettings)
        HandleFitEcho("------- initial set of parameters: SET 1 -------")
        HandleFitEcho(cFitParameter.GetFullParameterEcho(Me.FitFunction_Set1.FitParametersGroupedWithoutCombinedGroup))
        HandleFitEcho("")
        HandleFitEcho("---------- selected fit models: SET 2 ----------")
        HandleFitEcho("fit models: " & vbNewLine & Me.FitFunction_Set2.FitFunctionName)
        HandleFitEcho("fit formula: " & vbNewLine & Me.FitFunction_Set2.FitFunctionFormula)
        HandleFitEcho("------- initial set of parameters: SET 2 -------")
        HandleFitEcho(cFitParameter.GetFullParameterEcho(Me.FitFunction_Set2.FitParametersGroupedWithoutCombinedGroup))
        HandleFitEcho("")
        HandleFitEcho("-----------------------------------------")
        HandleFitEcho("3 ... 2 ... 1 ... fit running ... good luck!")

        ' Deactivate the UI-buttons during the fit
        Me.SetButtonsToFitModus(True)

        ' Initialize a UI refresh
        Me.Refresh()

        ' Start Fit async
        Me.FitProcedure.FitAsync(Me.FitFunction_Set1,
                                 Me.FitFunction_Set2,
                                 FitDataPoints_Set1,
                                 FitDataPoints_Set2,
                                 Nothing,
                                 Nothing)

    End Sub
#End Region

#Region "Fit-Finishing"
    Private Delegate Sub _FitFinished(FitStopReason As Integer,
                                      FinalParameters As cFitParameterGroupGroup,
                                      Chi2_Set1 As Double,
                                      Chi2_Set2 As Double)
    ''' <summary>
    ''' Fit Finished Function
    ''' </summary>
    Private Sub FitFinished(FitStopReason As Integer,
                            FinalParameters As cFitParameterGroupGroup,
                            Chi2_Set1 As Double,
                            Chi2_Set2 As Double) Handles FitProcedure.FitFinished
        ' Check for required Invoke
        If Me.InvokeRequired Then
            Dim _delegate As New _FitFinished(AddressOf FitFinished)
            Invoke(_delegate, FitStopReason, FinalParameters, Chi2_Set1, Chi2_Set2)
        Else
            ' Echo the final set of Parameters
            ' and fit results, depending on the final Fit-Report.
            HandleFitEcho("------------ fit finished ------------")
            HandleFitEcho("stop reason: " & vbNewLine & Me.FitProcedure.ConvertFitStopCodeToMessage(FitStopReason))
            HandleFitEcho("interation count: " & Me.FitProcedure.Iterations.ToString("N0"))
            HandleFitEcho("")
            HandleFitEcho("------------- statistics: SET 1 -------------")
            HandleFitEcho("standard deviation (sigma):   " & Me.FitProcedure.FinalStatistics_Set1.StandardDeviation.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("variance (sigma^2):           " & Me.FitProcedure.FinalStatistics_Set1.Variance.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("mean squared error (MSE):     " & Me.FitProcedure.FinalStatistics_Set1.MeanError.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("coef. of determination (R^2): " & Me.FitProcedure.FinalStatistics_Set1.CoefficientOfDetermination_R2.ToString("F6"))
            HandleFitEcho("------------- statistics: SET 2 -------------")
            HandleFitEcho("standard deviation (sigma):   " & Me.FitProcedure.FinalStatistics_Set2.StandardDeviation.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("variance (sigma^2):           " & Me.FitProcedure.FinalStatistics_Set2.Variance.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("mean squared error (MSE):     " & Me.FitProcedure.FinalStatistics_Set2.MeanError.ToString(cFitParameter.NumericFormat))
            HandleFitEcho("coef. of determination (R^2): " & Me.FitProcedure.FinalStatistics_Set2.CoefficientOfDetermination_R2.ToString("F6"))
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
            Me.WriteBackFitParameters(FinalParameters, FitFunctionSets.Set1)
            Me.WriteBackFitParameters(FinalParameters, FitFunctionSets.Set2)

            ' reset the interface
            Me.SetButtonsToFitModus(False)

            ' Raise the fit-finished event, to inform other listeners
            RaiseEvent FitFinishedEvent()
        End If
    End Sub

    ''' <summary>
    ''' Writes back the Fit-Parameters to the parameter-boxes and updates the preview image.
    ''' </summary>
    Protected Sub WriteBackFitParameters(FinalParameters As cFitParameterGroupGroup, ToSet As FitFunctionSets)

        Dim FitFunctionSettings_Set As List(Of cFitSettingPanel) = Nothing

        Select Case ToSet
            Case FitFunctionSets.Set1
                FitFunctionSettings_Set = Me.FitFunctionSettings_Set1
            Case FitFunctionSets.Set2
                FitFunctionSettings_Set = Me.FitFunctionSettings_Set2
            Case Else
                FitFunctionSettings_Set = Me.FitFunctionSettings_Set1
        End Select

        ' Decide whether the fit used a multiple-peak function or a single-fit-function.
        If TypeOf Me.FitFunction_Set1 Is cFitFunction_MultiplePeaks Then
            ' Multiple Peak Fit Function
            '############################
            Dim MultiplePeakContainer As cFitFunction_MultiplePeaks = DirectCast(Me.FitFunction_Set1, cFitFunction_MultiplePeaks)

            ' Go through all Fit-Functions
            For Each FitFunctionSetting As cFitSettingPanel In FitFunctionSettings_Set

                ' Set the individual FitParameter
                FitFunctionSetting.SetFitParameters(FinalParameters)

            Next

        Else
            ' Single Peak Fit Function
            '##########################

            ' Write back the parameters
            FitFunctionSettings_Set(0).SetFitParameters(FinalParameters)

        End If

        ' Calculate the new preview-image, using the new parameters
        Me.CalculatePreviewImage(ToSet)
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

        Me.gbSaveData.Enabled = Not IsFitRunning
        Me.gbImportExportFitModels.Enabled = Not IsFitRunning
        Me.gbAddFitModels.Enabled = Not IsFitRunning
        Me.btnSetPreviewPointNumber.Enabled = Not IsFitRunning
        Me.nudPreviewPoints.Enabled = Not IsFitRunning
        Me.cboFitProcedure.Enabled = Not IsFitRunning
        Me.btnChangeFitProcedureSettings.Enabled = Not IsFitRunning
        Me.btnAddFitToFitQueue.Enabled = Not IsFitRunning
        Me.gbFitRange_Set1.Enabled = Not IsFitRunning
        Me.gbFitRange_Set2.Enabled = Not IsFitRunning
        Me.gbSourceDataSelector_Set1.Enabled = Not IsFitRunning
        Me.gbSourceDataSelector_Set2.Enabled = Not IsFitRunning

        Me.gbProgress.Visible = IsFitRunning

        ' Disable all Fit-Model-settings-panels
        For Each C As Control In Me.flpFitModels_Set1.Controls
            C.Enabled = Not IsFitRunning
        Next
        For Each C As Control In Me.flpFitModels_Set2.Controls
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
            Me.pbPreview_Set1.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.None
            Me.pbPreview_Set2.PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.None
        End If

    End Sub

#End Region

#Region "Exporting and Importing the Collection of Fit-Models"

#Region "Button to launch the Import/Export procedures"

    ''' <summary>
    ''' Function to get the default export model name.
    ''' </summary>
    Protected Function GetDefaultExportModelName() As String
        Return Me._SpectroscopyTableList(0).FileNameWithoutPathAndExtension & "-" & Me._SpectroscopyTableList(1).FileNameWithoutPathAndExtension & My.Resources.rFitting.FitModelExport_FileExtension_MultipleData
    End Function

    ''' <summary>
    ''' Button to launch the import
    ''' </summary>
    Private Sub btnImportFitModels_Click(sender As Object, e As EventArgs) Handles btnImportFitModels.Click

        ' Displays a OpenFileDialog so the user can load the fit-models
        Dim Dialog As New OpenFileDialog()

        ' Open last path, if it was selected once
        If System.IO.Directory.Exists(My.Settings.Fit_MultipleFiles_ModelExportPath) Then
            Dialog.InitialDirectory = My.Settings.Fit_MultipleFiles_ModelExportPath
        End If

        Dialog.Filter = My.Resources.rFitting.FitModelExport_FileExtensionDescription_MultipleData & "|*" & My.Resources.rFitting.FitModelExport_FileExtension_MultipleData
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

        Dialog.Filter = My.Resources.rFitting.FitModelExport_FileExtensionDescription_MultipleData & "|*" & My.Resources.rFitting.FitModelExport_FileExtension_MultipleData
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
            TMPFileName = Me.FitFunction_Set1.GetType.ToString & ".Set1.xml"
            If Me.FitFunction_Set1.ExportXML(TMPPath & TMPFileName) Then
                TmpFileList.Add(TMPPath & TMPFileName)
            End If

            ' Create the temporary file name
            TMPFileName = Me.FitFunction_Set2.GetType.ToString & ".Set2.xml"
            If Me.FitFunction_Set2.ExportXML(TMPPath & TMPFileName) Then
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
            Me.ClearAllFitFunctions(FitFunctionSets.Set1)
            Me.ClearAllFitFunctions(FitFunctionSets.Set2)

            ' Manage the parameter-locks
            Dim LockTo As New Dictionary(Of String, String)
            Dim LockToTMP As Dictionary(Of String, String)

            ' Generate a fit-model for each XMLFile in memory (FileName, MemoryStream)
            For Each FileStreamKV As KeyValuePair(Of String, IO.MemoryStream) In TmpFileList
                ' The class of the fit-function is saved in the file-name of the XML,
                ' so extract this information out of the file-name.
                Dim RegPattern As String = "(.*?)\.Set(\d).xml$"

                ' Instantiate the regular expression object.
                Dim Reg As Regex = New Regex(RegPattern, RegexOptions.IgnoreCase)

                ' Match the regular expression pattern against a text string.
                Dim RegMatch As Match = Reg.Match(System.IO.Path.GetFileName(FileStreamKV.Key))
                If RegMatch.Success Then
                    ' There is just one group
                    Dim ClassNameMatch As Group = RegMatch.Groups(1)
                    Dim SetNumberMatch As Group = RegMatch.Groups(2)

                    ' Save the match of this group as class name
                    Dim ClassName As String = ClassNameMatch.Value.ToString()
                    'Dim ClassType As Type = Type.GetType(ClassName)
                    Dim ClassType As Type = AvailablePlugins.GetType(ClassName)

                    ' Get the SetNumber
                    Dim SetNumber As Integer = Convert.ToInt32(SetNumberMatch.Value)

                    ' Get the fit-function type by this.
                    Dim FitFunction As iFitFunction = cFitFunction.GetFitFunctionByType(ClassType)

                    ' Import the FitFunction-Data
                    FitFunction.ImportXML(FileStreamKV.Value)

                    ' Treat multiple fit functions different, than single fit functions
                    If FitFunction.GetType Is GetType(cFitFunction_MultiplePeaks) Then
                        ' multiple Fit Functions

                        Dim MultipleFitFunctions As List(Of iFitFunction) = DirectCast(FitFunction, cFitFunction_MultiplePeaks).FitFunctions

                        ' Extract the lock-Information of the parameters
                        For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                            LockToTMP = MultipleFitFunctions(i).FitParameters.GetLockedParameterInfo()
                            For Each KV As KeyValuePair(Of String, String) In LockToTMP
                                If Not LockTo.ContainsKey(KV.Key) Then LockTo.Add(KV.Key, KV.Value)
                            Next
                        Next

                        If SetNumber = 1 Then
                            ' Create the fit-functions
                            For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                                Me.AddFitFunction(MultipleFitFunctions(i), FitFunctionSets.Set1)
                            Next

                            ' import the settings
                            For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                                Me.FitFunctionSettings_Set1(i).SetFitParameters(MultipleFitFunctions(i).FitParameters)
                            Next
                        Else
                            ' Create the fit-functions
                            For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                                Me.AddFitFunction(MultipleFitFunctions(i), FitFunctionSets.Set2)
                            Next

                            ' import the settings
                            For i As Integer = 0 To MultipleFitFunctions.Count - 1 Step 1
                                Me.FitFunctionSettings_Set2(i).SetFitParameters(MultipleFitFunctions(i).FitParameters)
                            Next
                        End If

                    Else
                        ' single fit function --> much easier

                        ' extract lock-information
                        LockToTMP = FitFunction.FitParameters.GetLockedParameterInfo()
                        For Each KV As KeyValuePair(Of String, String) In LockToTMP
                            If Not LockTo.ContainsKey(KV.Key) Then LockTo.Add(KV.Key, KV.Value)
                        Next

                        If SetNumber = 1 Then
                            ' Create the new Settings-Panel
                            If Me.AddFitFunction(FitFunction, FitFunctionSets.Set1) Then
                                ' Import the settings to the new settings-panel
                                Me.FitFunctionSettings_Set1.Last.SetFitParameters(FitFunction.FitParameters)
                            End If
                        Else
                            ' Create the new Settings-Panel
                            If Me.AddFitFunction(FitFunction, FitFunctionSets.Set2) Then
                                ' Import the settings to the new settings-panel
                                Me.FitFunctionSettings_Set2.Last.SetFitParameters(FitFunction.FitParameters)
                            End If
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

            ' Now finally write back the parameter lock-informations
            For i As Integer = 0 To Me.FitFunctionSettings_Set1.Count - 1 Step 1
                Me.FitFunctionSettings_Set1(i).WriteBackLockInformation(LockTo)
            Next
            For i As Integer = 0 To Me.FitFunctionSettings_Set2.Count - 1 Step 1
                Me.FitFunctionSettings_Set2(i).WriteBackLockInformation(LockTo)
            Next

        Catch ex As Exception
            MessageBox.Show(My.Resources.rFitting.FitModelExport_Error_Import & ex.Message,
                            My.Resources.rFitting.FitModelExport_Error_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

#End Region

#End Region

#Region "saving of the current fit-data back to the spectroscopy-table"

    Private bGenerateDataForAllSourcePoints As Boolean
    Private iDataSaverSet As FitFunctionSets

    ''' <summary>
    ''' Button to initialize the saving
    ''' </summary>
    Private Sub btnSaveFitToSpectroscopyTable_Click(sender As Object, e As EventArgs) Handles btnSaveFitToSpectroscopyTable_Set2.Click, btnSaveFitToSpectroscopyTable_Set1.Click
        If sender Is btnSaveFitToSpectroscopyTable_Set1 Then Me.SaveFitDataAsync(FitFunctionSets.Set1)
        If sender Is btnSaveFitToSpectroscopyTable_Set2 Then Me.SaveFitDataAsync(FitFunctionSets.Set2)
    End Sub

    ''' <summary>
    ''' Background-Thread to generate the fit-data to save.
    ''' </summary>
    Private WithEvents FitDataSaver As New System.ComponentModel.BackgroundWorker

    ''' <summary>
    ''' Initializes the saving of the Fit-Data
    ''' </summary>
    Private Sub SaveFitDataAsync(FromSet As FitFunctionSets)
        If FitDataSaver.IsBusy Then Return

        ' Save the set to use
        Me.iDataSaverSet = FromSet

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
        Dim xSourceColumn As cSpectroscopyTable.DataColumn
        Dim ySourceColumn As cSpectroscopyTable.DataColumn
        Dim yColumns As New List(Of cSpectroscopyTable.DataColumn)
        Dim yColumn As cSpectroscopyTable.DataColumn

        ' Set-specific settings
        Dim Set_SettingsPanelList As List(Of cFitSettingPanel) = Nothing
        Dim Set_FitRangeLeft As Double
        Dim Set_FitRangeRight As Double

        Select Case Me.iDataSaverSet

            Case FitFunctionSets.Set1
                xSourceColumn = Me._SpectroscopyTableList(0).Column(Me.cbX_Set1.SelectedColumnName)
                ySourceColumn = Me._SpectroscopyTableList(0).Column(Me.cbY_Set1.SelectedColumnName)
                Set_SettingsPanelList = Me.FitFunctionSettings_Set1
                Set_FitRangeLeft = Me.FitRangeLeft_Set1
                Set_FitRangeRight = Me.FitRangeRight_Set1

            Case FitFunctionSets.Set2
                xSourceColumn = Me._SpectroscopyTableList(1).Column(Me.cbX_Set2.SelectedColumnName)
                ySourceColumn = Me._SpectroscopyTableList(1).Column(Me.cbY_Set2.SelectedColumnName)
                Set_SettingsPanelList = Me.FitFunctionSettings_Set2
                Set_FitRangeLeft = Me.FitRangeLeft_Set2
                Set_FitRangeRight = Me.FitRangeRight_Set2

            Case Else
                Return

        End Select

        Dim xSourceColumnValues As ReadOnlyCollection(Of Double) = xSourceColumn.Values

        ' Report progress
        Dim Progress As Double = 1
        Dim ProgressProgress As Double = 70 / (Set_SettingsPanelList.Count + 1)
        Me.FitDataSaver.ReportProgress(CInt(Progress), My.Resources.rFitting.initializing)

        ' Go through the FitSettingsPanel-List and generate the individual fit-data for each settings-panel
        ' and its corresponding fit-function. Also check for additionally generatable data.
        For Each SettingsPanel As cFitSettingPanel In Set_SettingsPanelList
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
        If Set_SettingsPanelList.Count > 1 Then
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
                        If xSourceColumnValues(n) < Set_FitRangeLeft Or xSourceColumnValues(n) >= Set_FitRangeRight Then
                            yColumn.SetValueInColumn(n, Double.NaN)
                        End If
                    End If

                Next
            Next
        End If

        ' Finally save the data to the file-object
        Me._SpectroscopyTableList(Me.iDataSaverSet).BaseFileObject.AddSpectroscopyColumns(yColumns)

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
    Private Sub btnSaveFitOutputToExtendedComment_Click(sender As Object, e As EventArgs) Handles btnSaveFitProcedureOutput_Set2.Click, btnSaveFitProcedureOutput_Set1.Click

        ' Save current content of the fit-output to the extended comment section
        If Me.txtFitEcho.Text.Trim <> String.Empty Then
            If sender Is btnSaveFitProcedureOutput_Set1 Then Me._SpectroscopyTableList(0).BaseFileObject.AddExtendedComment(Me.txtFitEcho.Text)
            If sender Is btnSaveFitProcedureOutput_Set2 Then Me._SpectroscopyTableList(1).BaseFileObject.AddExtendedComment(Me.txtFitEcho.Text)
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
    End Sub

#End Region

#Region "Fit-Procedure Changing"
    ''' <summary>
    ''' Set the Fit-Procedure Object on changing the selection
    ''' </summary>
    Private Sub cboFitProcedure_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFitProcedure.SelectedIndexChanged
        Me.FitProcedure.SetFitProcedure(Me.GetSelectedFitProcedure, Me.GetSelectedFitProcedure)
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
                .FitProcedureSettings_Set1 = SettingsWindow.ShowDialog(.ProcedureSettingPanel_Set1)
                .FitProcedureSettings_Set2 = .FitProcedureSettings_Set1
                Me.FitProcedureSettings = .FitProcedureSettings_Set1
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
