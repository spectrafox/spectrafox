''' <summary>
''' Not abstract, since we want to use the Designer to style the settings.
''' </summary>
Public Class cFitSettingPanel

#Region "Events & properties: Removal of FitFunction, Parameter-Change"

    ''' <summary>
    ''' Event to remove the fit-function out of the selection of the base window.
    ''' </summary>
    Public Event RequestRemovalOfFitFunction(ByRef PanelToRemove As cFitSettingPanel)

    ''' <summary>
    ''' Event gets raised, if a parameter of a fit-function gets changed,
    ''' so that a refresh of the preview can be processed.
    ''' </summary>
    Protected Friend Event ParameterChanged()

    ''' <summary>
    ''' If set to true, a modification of the parameters will not
    ''' lead to a raise of the ParameterChanged-Event.
    ''' Use this to avoid Event-Spamming,
    ''' if you set the parameters programmatically.
    ''' </summary>
    Public Property DontRaiseParameterChangeEvent As Boolean = False

    ''' <summary>
    ''' Sub to raise the parameter-changed event from a child-class.
    ''' </summary>
    Public Sub RaiseParameterChangedEvent()
        If Not DontRaiseParameterChangeEvent Then
            RaiseEvent ParameterChanged()
        End If
    End Sub

    ''' <summary>
    ''' Event raised by the fit-function defining subclass.
    ''' </summary>
    Public Event FitFunctionInitialized()

    ''' <summary>
    ''' Raises the FitFunctionInitializedEvent
    ''' </summary>
    Public Sub OnFitFunctionInitialized()
        Me._FitFunction.ChangeFitRangeX(Me.InitialFitRangeLeft, Me.InitialFitRangeRight)
        RaiseEvent FitFunctionInitialized()
    End Sub

    ''' <summary>
    ''' Raised, if SuBParameters were added or removed
    ''' </summary>
    Public Event SubParametersAddedOrRemoved()

    ''' <summary>
    ''' Raised, if SuBParameters were added or removed
    ''' </summary>
    Public Sub RaiseSubParametersAddedOrRemoved()
        RaiseEvent SubParametersAddedOrRemoved()
    End Sub

#End Region

#Region "Events: Range-Selection"
    ''' <summary>
    ''' Event to request a selection of a certain range.
    ''' </summary>
    Public Event XRangeSelectionRequest(Callback As mSpectroscopyTableViewer.XRangeSelectionCallback)

    ''' <summary>
    ''' Sub to raise the range-selection request event from a child-class.
    ''' </summary>
    Public Sub RaiseXRangeSelectionRequest(Callback As mSpectroscopyTableViewer.XRangeSelectionCallback)
        RaiseEvent XRangeSelectionRequest(Callback)
    End Sub

    ''' <summary>
    ''' Event to request a selection of a certain range.
    ''' </summary>
    Public Event YRangeSelectionRequest(Callback As mSpectroscopyTableViewer.YRangeSelectionCallback)

    ''' <summary>
    ''' Sub to raise the range-selection request event from a child-class.
    ''' </summary>
    Public Sub RaiseYRangeSelectionRequest(Callback As mSpectroscopyTableViewer.YRangeSelectionCallback)
        RaiseEvent YRangeSelectionRequest(Callback)
    End Sub

    ''' <summary>
    ''' Event to request a selection of the a certain value.
    ''' </summary>
    Public Event XValueSelectionRequest(Callback As mSpectroscopyTableViewer.XValueSelectionCallback)

    ''' <summary>
    ''' Sub to raise the XValue-selection request event from a child-class.
    ''' </summary>
    Public Sub RaiseXValueSelectionRequest(Callback As mSpectroscopyTableViewer.XValueSelectionCallback)
        RaiseEvent XValueSelectionRequest(Callback)
    End Sub

    ''' <summary>
    ''' Event to request a selection of the a certain value.
    ''' </summary>
    Public Event YValueSelectionRequest(Callback As mSpectroscopyTableViewer.YValueSelectionCallback)

    ''' <summary>
    ''' Sub to raise the XValue-selection request event from a child-class.
    ''' </summary>
    Public Sub RaiseYValueSelectionRequest(Callback As mSpectroscopyTableViewer.YValueSelectionCallback)
        RaiseEvent YValueSelectionRequest(Callback)
    End Sub

#End Region

#Region "Fit-Range-Changed"

    ''' <summary>
    ''' Sub that modifies the Fit-Range of the Fit-Function.
    ''' Is usually called from the fit-window, if the user changed the range.
    ''' </summary>
    Public Sub FitRangeChanged(LeftValue As Double, RightValue As Double)
        If Me.FitFunction Is Nothing Then Return
        Me.FitFunction.ChangeFitRangeX(LeftValue, RightValue)
    End Sub

    ''' <summary>
    ''' Initial value of the fit-range that is transmitted to the fit-function
    ''' after it get's constructed and initialized.
    ''' </summary>
    Private InitialFitRangeLeft As Double = 0

    ''' <summary>
    ''' Initial value of the fit-range that is transmitted to the fit-function
    ''' after it get's constructed and initialized.
    ''' </summary>
    Private InitialFitRangeRight As Double = 0

    ''' <summary>
    ''' On construction of the fit-function these values will be transmitted
    ''' to the fit-function to initialize.
    ''' </summary>
    Public Sub SetInitializationFitRange(LeftValue As Double, RightValue As Double)
        InitialFitRangeLeft = LeftValue
        InitialFitRangeRight = RightValue
    End Sub

#End Region

#Region "Constructor"

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    ''' <summary>
    ''' Initialization of the Panel.
    ''' Has to be called from the child-class.
    ''' </summary>
    Protected Sub Initialize() Handles Me.FitFunctionInitialized
        If Me.DesignMode Then Return

        ' Set the Name of the Container to the Fit-Function-Name
        Me.lblFitFunctionName.Text = Me.FitFunctionName

        ' Set the Description and the Formula of the Fit:
        Me.txtFormula.Text = Me.FitFunctionFormula
        Me.txtDescription.Text = Me.FitDescription
        Me.txtAuthors.Text = Me.FitAuthors

        ' Enable or disable the CUDA-Support.
        Me.ckbUseCUDA.Enabled = Me.FitFunctionSupportsCUDA
    End Sub
#End Region

#Region "Panel-Settings"
    ''' <summary>
    ''' Identifier of the Fit-Function.
    ''' Should be individual between different fit-functions.
    ''' </summary>
    Public Property Identifier As String

    ''' <summary>
    ''' Name of the Fit-Function to display.
    ''' </summary>
    Public Overridable ReadOnly Property FitFunctionName As String
        Get
            If Me.FitFunction Is Nothing Then
                Return "Fit-Function-Name"
            Else
                Return Me.FitFunction.FitFunctionName
            End If
        End Get
    End Property

    ''' <summary>
    ''' Formula of the Fit-Function to display.
    ''' </summary>
    Public Overridable ReadOnly Property FitFunctionFormula As String
        Get
            If Me.FitFunction Is Nothing Then
                Return "f(x) = x^2"
            Else
                Return Me.FitFunction.FitFunctionFormula
            End If
        End Get
    End Property

    ''' <summary>
    ''' Description of the Fit-Function to display.
    ''' </summary>
    Public Overridable ReadOnly Property FitDescription As String
        Get
            If Me.FitFunction Is Nothing Then
                Return "This is a cool fit-function."
            Else
                Return Me.FitFunction.FitDescription
            End If
        End Get
    End Property

    ''' <summary>
    ''' Authors of the Fit-Function to display.
    ''' </summary>
    Public Overridable ReadOnly Property FitAuthors As String
        Get
            If Me.FitFunction Is Nothing Then
                Return "Author: A. Nonym"
            Else
                Return Me.FitFunction.FitFunctionAuthors
            End If
        End Get
    End Property

    ''' <summary>
    ''' Fit functions supports CUDA.
    ''' </summary>
    Public Overridable ReadOnly Property FitFunctionSupportsCUDA As Boolean
        Get
            If Me.FitFunction Is Nothing Then
                Return False
            Else
                Return Me.FitFunction.FunctionImplementsCUDAVersion
            End If
        End Get
    End Property
#End Region

#Region "Lock certain parameters, if the fit-settings-panel is used together with other fits"

    Private _LockFitParametersForMultipleFit As Boolean = False
    ''' <summary>
    ''' This property can be overridden!
    ''' For multiple-function fit, we have to deactivate
    ''' the ability to fit also some parameters, such as the YOffset
    ''' for ALL fit-functions, except the first one.
    ''' Otherwise the algorithm will optimize them, by pushing
    ''' e.g. one to the negative, to match another one, which is pushed
    ''' to the positive offset.
    ''' </summary>
    Public Overridable Property LockFitParametersForMultipleFit As Boolean
        Get
            Return Me._LockFitParametersForMultipleFit
        End Get
        Set(value As Boolean)
            Me._LockFitParametersForMultipleFit = value

            ' Check go through all registered Parameter-Textboxes and Checkboxes
            ' and set the default value, or the fixation of the parameter
            For Each IdentifierKV As KeyValuePair(Of String, Double) In Me.LockedFitParameterOnMultipleFit
                For Each TextboxKV As KeyValuePair(Of mFitParameter, String) In Me.FitParameterTextBoxes
                    If TextboxKV.Value = IdentifierKV.Key Then
                        ' Enable or disable the textbox
                        TextboxKV.Key.Enabled = Not Me._LockFitParametersForMultipleFit

                        ' If locking, set the given default value, and lock the parameter.
                        '# 10.08.2015: If unlocking, don't unlock the parameter. Otherwise during import
                        ' the parameter will be unlocked all the time.
                        If Me._LockFitParametersForMultipleFit Then
                            TextboxKV.Key.SetValue(IdentifierKV.Value)
                            TextboxKV.Key.SetParameterFixation(Me._LockFitParametersForMultipleFit)
                        End If
                        Exit For
                    End If
                Next
            Next
        End Set
    End Property

    ''' <summary>
    ''' Dictionary with all parameter identifiers and their default values,
    ''' that should be set on locking them during the multiple-function-fit.
    ''' </summary>
    Private LockedFitParameterOnMultipleFit As New Dictionary(Of String, Double)

    ''' <summary>
    ''' Registers a parameter numeric textbox with all its events.
    ''' </summary>
    Protected Sub RegisterLockedFitParameterOnMultipleFit(ByVal Identifier As String, ByVal DefaultValue As Double)
        If Me.LockedFitParameterOnMultipleFit.ContainsKey(Identifier) Then
            Me.LockedFitParameterOnMultipleFit(Identifier) = DefaultValue
        Else
            Me.LockedFitParameterOnMultipleFit.Add(Identifier, DefaultValue)
        End If
    End Sub

#End Region

#Region "Button Clicks"
    ''' <summary>
    ''' Raise the event to remove the fit-function.
    ''' </summary>
    Private Sub btnRemoveFitFunction_Click(sender As Object, e As EventArgs) Handles btnRemoveFitFunction.Click
        RaiseEvent RequestRemovalOfFitFunction(Me)
    End Sub
#End Region

#Region "Interface Functions for initialization"
    ''' <summary>
    ''' Container for the Fit-Function
    ''' </summary>
    Protected _FitFunction As iFitFunction

    ''' <summary>
    ''' Returns the FitFunction with the current applied settings.
    ''' </summary>
    Public Overridable ReadOnly Property FitFunction As iFitFunction
        Get
            Return Me._FitFunction
        End Get
    End Property

#End Region

#Region "Write FitParameters back to the Settings-Panel, e.g. used after a successful fit"

    ''' <summary>
    ''' This function has to be overridden, to allow a
    ''' programmatical change of the Fit-Parameters from outside the panel.
    ''' </summary>
    Public Overridable Sub SetFitParameters(ByRef FitParameters As cFitParameterGroupGroup)

        ' But watch out: we have to stops the spamming of the ParameterChanged-Event during this process
        Me.DontRaiseParameterChangeEvent = True

        ' Write back the parameters
        Me.WriteBackFitParameters(FitParameters)

        ' Activate the ParameterChanged-Event
        Me.DontRaiseParameterChangeEvent = False

        ' Raise the event only one time
        Me.RaiseParameterChangedEvent()

    End Sub

    ''' <summary>
    ''' This function has to be overridden, to allow a
    ''' programmatical change of the Fit-Parameters from outside the panel.
    ''' </summary>
    Public Overridable Sub SetFitParameters(ByRef FitParameters As cFitParameterGroup)

        ' But watch out: we have to stops the spamming of the ParameterChanged-Event during this process
        Me.DontRaiseParameterChangeEvent = True

        ' Write back the parameters
        Me.WriteBackFitParameters(FitParameters)

        ' Activate the ParameterChanged-Event
        Me.DontRaiseParameterChangeEvent = False

        ' Raise the event only one time
        Me.RaiseParameterChangedEvent()

    End Sub

    ''' <summary>
    ''' This function can be overridden, to allow an individual
    ''' programmatical change of the Fit-Parameters from outside the panel.
    ''' The default setting is to use all registered Parameter-Textboxes to
    ''' write the FitParameterSettings
    ''' </summary>
    Protected Overridable Sub WriteBackFitParameters(ByRef FitParameterGroups As cFitParameterGroupGroup)
        ' Set the Textbox-Content
        Dim nTextBox As mFitParameter = Nothing

        ' Write back all fit-parameters
        For Each FPG As cFitParameterGroup In FitParameterGroups
            ' Check, if the GUID matches, then write back the parameters.
            If Me._FitFunction.UseFitParameterGroupID = FPG.Identifier Then
                Me.WriteBackFitParameters(FPG)
                Return
            End If
        Next

    End Sub

    ''' <summary>
    ''' This function can be overridden, to allow an individual
    ''' programmatical change of the Fit-Parameters from outside the panel.
    ''' The default setting is to use all registered Parameter-Textboxes to
    ''' write the FitParameterSettings
    ''' </summary>
    Protected Overridable Sub WriteBackFitParameters(ByRef FitParameters As cFitParameterGroup)
        ' Set the Textbox-Content
        Dim nTextBox As mFitParameter = Nothing

        For Each FP As cFitParameter In FitParameters

            ' Get the FitParameter-Textbox from the registered list:
            If Me.FitParameterTextBoxes.ContainsValue(FP.Identifier) Then
                For Each TextBoxKV As KeyValuePair(Of mFitParameter, String) In Me.FitParameterTextBoxes
                    If TextBoxKV.Value = FP.Identifier Then
                        nTextBox = TextBoxKV.Key
                        Exit For
                    End If
                Next
                If Not nTextBox Is Nothing Then
                    ' Set the value, if the textbox could be found
                    nTextBox.SetValue(FP.Value)
                End If
            End If

        Next

    End Sub

    ''' <summary>
    ''' This function can be used to lock parameters together, by handing over a dictionary
    ''' with lock-information. 
    ''' Dictionary (ParameterIdentifier that is locked, Parameter identifier of the lock-target)
    ''' </summary>
    Public Overridable Sub WriteBackLockInformation(ByRef LockInformation As Dictionary(Of String, String))
        ' Set the Textbox-Content
        Dim nTextBox As mFitParameter = Nothing
        Dim GID_Source As KeyValuePair(Of Guid, String) = Nothing
        Dim GID_Target As KeyValuePair(Of Guid, String) = Nothing

        For Each LockKV As KeyValuePair(Of String, String) In LockInformation

            ' Extract Lock-Information
            GID_Source = cFitParameter.GetGroupIDFromIdentifier(LockKV.Key)

            ' Check if out Group-ID matches
            If Not Me.FitFunction.UseFitParameterGroupID = GID_Source.Key Then Continue For

            ' Get target-Lock
            GID_Target = cFitParameter.GetGroupIDFromIdentifier(LockKV.Value)

            ' Get the FitParameter-Textbox from the registered list:
            If Me.FitParameterTextBoxes.ContainsValue(GID_Source.Value) Then
                For Each TextBoxKV As KeyValuePair(Of mFitParameter, String) In Me.FitParameterTextBoxes
                    If TextBoxKV.Value = GID_Source.Value Then
                        nTextBox = TextBoxKV.Key
                        Exit For
                    End If
                Next
                If Not nTextBox Is Nothing Then
                    ' Set the value, if the textbox could be found
                    nTextBox.LockToOtherParameter(GID_Target)
                End If
            End If

        Next


    End Sub

#End Region

#Region "Register Parameter Numeric Textboxes and handle their clicks"

    ''' <summary>
    ''' Sub that has to be overriden, which binds all fit-parameters
    ''' to the interface. This function is necessary to handle inheritances correctly.
    ''' </summary>
    Protected Overridable Function BindFitParameters() As Boolean
        Return False
    End Function

    ''' <summary>
    ''' Initializes the Fit-Function
    ''' </summary>
    Public Overridable Sub InitializeFitFunction(ByVal FitFunction As iFitFunction)
        ' Set the Fit-Function of the BaseClass
        Me._FitFunction = FitFunction

        Me.OnFitFunctionInitialized()

        ' Initialize the Layout
        Me.Initialize()

        ' bind the fit-parameters to the GUI
        Me.BindFitParameters()
    End Sub

    ''' <summary>
    ''' Sub that resets the FitParameterGroup to lock parameters together.
    ''' </summary>
    Public Overridable Function SetFitParameterGroups(ByRef FitParameterGroups As cFitParameterGroupGroup) As Boolean
        Me.SetFitParameterGroupsRecursive(Me, FitParameterGroups)
        Return True
    End Function

    ''' <summary>
    ''' Recursive loop to set the parameters for all container controls containing FitParameters
    ''' </summary>
    Private Sub SetFitParameterGroupsRecursive(ByRef Container As Control, ByRef FitParameterGroups As cFitParameterGroupGroup)
        For Each C As Control In Container.Controls
            If C.GetType Is GetType(mFitParameter) Then
                DirectCast(C, mFitParameter).SetFitParameterGroups(FitParameterGroups)
            Else
                SetFitParameterGroupsRecursive(C, FitParameterGroups)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Dictionary with all parameter textboxes and their registration
    ''' to a certain fit-parameter-identifiers.
    ''' </summary>
    Private FitParameterTextBoxes As New Dictionary(Of mFitParameter, String)

    ''' <summary>
    ''' Read-only dictionary with all fit-parameter-panels
    ''' </summary>
    Public ReadOnly Property CurrentFitParameterTextBoxes As ReadOnlyDictionary(Of mFitParameter, String)
        Get
            Return New ReadOnlyDictionary(Of mFitParameter, String)(Me.FitParameterTextBoxes)
        End Get
    End Property

    ''' <summary>
    ''' Registers a parameter numeric textbox with all its events.
    ''' </summary>
    Protected Sub RegisterFitParameterSettingsBox(ByVal Identifier As String, ByRef ParameterBox As mFitParameter)
        If Me.FitParameterTextBoxes.ContainsKey(ParameterBox) Then
            Me.FitParameterTextBoxes(ParameterBox) = Identifier
        Else
            Me.FitParameterTextBoxes.Add(ParameterBox, Identifier)
        End If

        ' Register the FitParameter-ValidValueChanged-Event
        AddHandler ParameterBox.ValidValueChanged, AddressOf Me.ParameterValidValueChanged
    End Sub

    ''' <summary>
    ''' This function is called, if the numeric textbox changes its values.
    ''' Therefore the parameter in the fit-function is changed, too!
    ''' </summary>
    Private Sub ParameterValidValueChanged(ByRef ParameterTextBox As mFitParameter)
        ' Get the registered FitParameterIdentifier:
        Dim FitParameterIdentifier As String = Me.FitParameterTextBoxes(ParameterTextBox)

        ' Change the value of the Fit-Parameter
        With Me._FitFunction
            If Not .FitParameters.ContainsKey(FitParameterIdentifier) Then Return

            ' Raise the base-class event, that this happened.
            Me.RaiseParameterChangedEvent()
        End With
    End Sub

#End Region

#Region "Enable / disable CUDA for the fit-function, if possible"
    ''' <summary>
    ''' Enable / disable CUDA for the fit-function, if possible
    ''' </summary>
    Private Sub ckbUseCUDA_CheckedChanged(sender As Object, e As EventArgs) Handles ckbUseCUDA.CheckedChanged
        If Me.FitFunction Is Nothing Then Return
        If Me.FitFunction.FunctionImplementsCUDAVersion Then
            If Me.ckbUseCUDA.Checked Then
                If Not cGPUComputing.CanActivateCUDA Then
                    Me.ckbUseCUDA.Checked = False
                    MessageBox.Show(My.Resources.rFitSettingPanel.GPUActivationNotPossible,
                                    My.Resources.rFitSettingPanel.GPUActivationNotPossible_Title,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
            ' Fit function supports CUDA
            Me.FitFunction.UseCUDAVersion = Me.ckbUseCUDA.Checked
        Else
            Me.ckbUseCUDA.Checked = False
        End If
    End Sub
#End Region

End Class
