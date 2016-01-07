Public Class mFitParameter

#Region "Fit-Parameter-Object"
    ''' <summary>
    ''' Fit-Parameter controlled.
    ''' </summary>
    Private _FitParameter As cFitParameter

    ''' <summary>
    ''' Fit-Parameter controlled by this usercontrol.
    ''' </summary>
    Public ReadOnly Property FitParameter As cFitParameter
        Get
            Return Me._FitParameter
        End Get
    End Property

    ''' <summary>
    ''' Contains a list of all fit-parameters to lock to a single one.
    ''' </summary>
    Private _FitParameterGroups As cFitParameterGroupGroup
#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        AddHandler cmnUnlock.Click, AddressOf Me.LockToOtherParameterClick
        Me.lblTitle.Text = Me.Name
    End Sub

#End Region

#Region "Setup of the FitParameter"
    ''' <summary>
    ''' This function locks the userControl to a certain fit-parameter.
    ''' </summary>
    Public Sub BindToFitParameter(ByRef FitParameterToControl As cFitParameter,
                                  ByRef AllFitParameters As cFitParameterGroupGroup)
        Me._FitParameter = FitParameterToControl
        Me._FitParameterGroups = AllFitParameters
        Me.UpdateInterface()
    End Sub

    ''' <summary>
    ''' Sets the collection of all fit-parameters (needed for the locking menu)
    ''' </summary>
    Public Sub SetFitParameterGroups(ByRef AllFitParameters As cFitParameterGroupGroup)
        Me._FitParameterGroups = AllFitParameters
    End Sub

    ''' <summary>
    ''' This function locks the userControl to a certain fit-parameter,
    ''' but does not deliver information about other parameters.
    ''' Locking to other parameters is not possible then.
    ''' </summary>
    Public Sub BindToFitParameter(ByRef FitParameterToControl As cFitParameter)
        Me._FitParameter = FitParameterToControl
        Me.UpdateInterface()
    End Sub
#End Region

#Region "Interface initialization according to the fit-parameter values"

    ''' <summary>
    ''' Interface initialization according to the fit-parameter values
    ''' </summary>
    Private Sub UpdateInterface()
        If Me.FitParameter Is Nothing Then Return
        With Me.FitParameter
            Me.lblTitle.Text = .Description
            Me.txtValue.SetValue(.Value)
            Me.GetFixedPicture(.IsFixed)
            Me.GetLockedPicture(.LockedToParameterIdentifier)
            Me.GetBoundaryPicture(.UpperBoundary, .LowerBoundary)
        End With
    End Sub
#End Region

#Region "Fixation of the parameter."

    ''' <summary>
    ''' Returns the image that displays the Fixation-status.
    ''' </summary>
    Private Sub GetFixedPicture(IsFixed As Boolean)
        If IsFixed Then
            Me.pbFixParameter.BackgroundImage = My.Resources.lock_12
        Else
            Me.pbFixParameter.BackgroundImage = My.Resources.lock_disable_12
        End If
    End Sub

    ''' <summary>
    ''' Change the fixation-status of the parameter.
    ''' </summary>
    Private Sub pbFixedParameter_Click(sender As Object, e As EventArgs) Handles pbFixParameter.Click, pbFixParameter.DoubleClick
        With Me.FitParameter
            ' Check, if the parameter is locked to another one -> then do not allow to unfix it
            If .LockedToParameterIdentifier = "" Then
                .IsFixed = Not .IsFixed
                Me.GetFixedPicture(.IsFixed)
            End If
        End With
    End Sub

    ''' <summary>
    ''' Fixation of the parameter
    ''' </summary>
    Public Function IsFixed() As Boolean
        If Me.FitParameter Is Nothing Then Return False
        Return Me.FitParameter.IsFixed
    End Function

    ''' <summary>
    ''' Sets the fixation of the paramter
    ''' </summary>
    Public Sub SetParameterFixation(IsFixed As Boolean)
        If Me.FitParameter Is Nothing Then Return
        With Me.FitParameter
            ' Check, if the parameter is locked to another one -> then do not allow to unfix it
            If .LockedToParameterIdentifier = "" Then
                .IsFixed = IsFixed
                Me.GetFixedPicture(.IsFixed)
            End If
        End With
    End Sub

#End Region

#Region "Locking of the parameter, Context-Menu etc."

    ''' <summary>
    ''' Returns the image that displays the lock-status.
    ''' </summary>
    Private Sub GetLockedPicture(LockedToFitParameterIdentifier As String)
        If LockedToFitParameterIdentifier <> "" Then
            Me.pbLockParameter.BackgroundImage = My.Resources.chain_23
        Else
            Me.pbLockParameter.BackgroundImage = My.Resources.chain_disable_23
        End If
    End Sub

    ''' <summary>
    ''' Change the lock-status of the parameter.
    ''' </summary>
    Private Sub pbLockParameter_Click(sender As Object, e As EventArgs) Handles pbLockParameter.Click, pbLockParameter.DoubleClick
        ' Lock
        '######
        Me.cmLockParameterTo.Show(MousePosition)
    End Sub

    ''' <summary>
    ''' Saves the permanent context-menu entries, that should not be removed.
    ''' </summary>
    Private PermanentContextMenuEntries As Integer = 0

    ''' <summary>
    ''' Opening of the Context-Menu
    ''' </summary>
    Private Sub cmLockParameterTo_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmLockParameterTo.Opening
        With Me.FitParameter
            ' Check, if the all-fitparameter array exists.
            If Me._FitParameterGroups Is Nothing Then Return

            ' Save the number of permanent context-menu entries
            Me.PermanentContextMenuEntries = Me.cmLockParameterTo.Items.Count

            ' Add lock-factor
            Me.cmnuTxtLockFactor.Text = Me.FitParameter.LockedWithFactor.ToString("E3")
            Me.cmnuTxtLockFactor.BackColor = Color.Green
            Me.cmnuTxtLockFactor.ForeColor = Color.White

            ' Add lock-buttons for other parameters
            For Each FPG As cFitParameterGroup In Me._FitParameterGroups

                ' Create Group-Entry
                Dim GroupMenuItem As ToolStripMenuItem = DirectCast(Me.cmLockParameterTo.Items.Add(FPG.GroupGroupName), ToolStripMenuItem)
                GroupMenuItem.Name = FPG.IdentifierString

                ' Go through all Fit-Parameters.
                For Each FP As cFitParameter In FPG

                    ' ignore the parameter itself
                    If Me.FitParameter Is FP Then Continue For

                    ' Create the new parameter Box
                    Dim TSMenuItem As ToolStripMenuItem = DirectCast(GroupMenuItem.DropDownItems.Add(FP.Description), ToolStripMenuItem)
                    TSMenuItem.Name = FP.GetIdentifierForGroupID(FPG.Identifier)
                    AddHandler TSMenuItem.Click, AddressOf Me.LockToOtherParameterClick

                    ' check the parameter
                    If .LockedToParameterIdentifier <> "" Then
                        If TSMenuItem.Name = .LockedToParameterIdentifier Then
                            TSMenuItem.Checked = True
                        End If
                        Me.cmnUnlock.Checked = False
                    Else
                        Me.cmnUnlock.Checked = True
                    End If
                Next
            Next
        End With
    End Sub

    ''' <summary>
    ''' Context-Menu closing --> remove handlers
    ''' </summary>
    Private Sub cmLockParameterTo_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs) Handles cmLockParameterTo.Closing
        ' When hiding the menu, remove all lock-buttons, and remove all handlers
        Dim ItemMaxIndex As Integer = Me.cmLockParameterTo.Items.Count - 1
        For i As Integer = ItemMaxIndex To Me.PermanentContextMenuEntries Step -1
            RemoveHandler Me.cmLockParameterTo.Items(i).Click, AddressOf Me.LockToOtherParameterClick
            Me.cmLockParameterTo.Items.RemoveAt(i)
        Next
    End Sub

    ''' <summary>
    ''' Sets up the lock to the other parameters.
    ''' </summary>
    Private Sub LockToOtherParameterClick(sender As Object, e As EventArgs)
        Dim ParameterIdentifier As String
        Dim ParameterGroup As Guid
        Dim t As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        If t.Name = Me.cmnUnlock.Name Then
            ' UNLOCK
            '########
            ParameterIdentifier = ""
            ParameterGroup = Nothing
        Else
            ' LOCK
            '######
            ' extract the parameter-identifier
            Dim GUIDKV As KeyValuePair(Of Guid, String) = cFitParameter.GetGroupIDFromIdentifier(t.Name)
            ParameterIdentifier = GUIDKV.Value
            ParameterGroup = GUIDKV.Key
        End If
        Me.LockToOtherParameter(ParameterGroup, ParameterIdentifier)
        Me.cmLockParameterTo.Show()
    End Sub

    ''' <summary>
    ''' Actually locks or unlocks the FitParameter
    ''' </summary>
    Public Sub LockToOtherParameter(GIDKV As KeyValuePair(Of Guid, String))
        Me.LockToOtherParameter(GIDKV.Key, GIDKV.Value)
    End Sub

    ''' <summary>
    ''' Actually locks or unlocks the FitParameter
    ''' </summary>
    Public Sub LockToOtherParameter(GroupID As Guid, ParameterIdentifier As String)
        If ParameterIdentifier = "" Then
            ' UNLOCK
            '########

            ' Only check the Unlock-Button
            For Each TM As ToolStripItem In Me.cmLockParameterTo.Items
                If TM.GetType Is GetType(ToolStripMenuItem) Then
                    DirectCast(TM, ToolStripMenuItem).Checked = (TM Is cmnUnlock)
                End If
            Next

            ' Remove old handlers
            Dim OldGIDKV As KeyValuePair(Of Guid, String) = cFitParameter.GetGroupIDFromIdentifier(Me.FitParameter.LockedToParameterIdentifier)
            If Not OldGIDKV.Key = Nothing Then
                If Me._FitParameterGroups.ContainsKey(OldGIDKV.Key) Then
                    With Me._FitParameterGroups.Group(OldGIDKV.Key)
                        RemoveHandler .Parameter(OldGIDKV.Value).ValueChanged, AddressOf LockedToValueChanged
                    End With
                End If
            End If

            ' reset the lock-status
            Me.FitParameter.LockedToParameterIdentifier = ""
            Me.GetLockedPicture(Me.FitParameter.LockedToParameterIdentifier)

            ' unfix the parameter
            Me.FitParameter.IsFixed = False
            Me.txtValue.Enabled = True
            Me.GetFixedPicture(False)
        Else
            ' LOCK
            '######

            ' Lock the parameter
            Me.FitParameter.LockedToParameterIdentifier = cFitParameter.GetIdentifierForGroupID(GroupID, ParameterIdentifier)
            Me.GetLockedPicture(Me.FitParameter.LockedToParameterIdentifier)

            ' check the selected parameter button.
            For Each GM As ToolStripItem In Me.cmLockParameterTo.Items
                If GM.GetType Is GetType(ToolStripMenuItem) Then
                    If DirectCast(GM, ToolStripMenuItem).DropDownItems.Count > 0 Then
                        For Each TM As ToolStripMenuItem In DirectCast(GM, ToolStripMenuItem).DropDownItems
                            TM.Checked = (TM.Name = Me.FitParameter.LockedToParameterIdentifier)
                        Next
                    End If
                End If
            Next

            ' Set the value to the locked property, and the parameter-fixation to true.
            Me.FitParameter.IsFixed = True
            Me.GetFixedPicture(True)
            Me.txtValue.Enabled = False

            If Not GroupID = Nothing Then
                Me.txtValue.SetValue(Me._FitParameterGroups.Group(GroupID).Parameter(ParameterIdentifier).Value * Me.FitParameter.LockedWithFactor, , True)

                ' Add handler to react on value-changes
                If Me._FitParameterGroups.ContainsKey(GroupID) Then
                    AddHandler Me._FitParameterGroups.Group(GroupID).Parameter(ParameterIdentifier).ValueChanged, AddressOf LockedToValueChanged
                End If
            End If

        End If
    End Sub

    ''' <summary>
    ''' Function that adapts the value on changing the locked-status.
    ''' </summary>
    Public Sub LockedToValueChanged(ByVal NewValue As Double)

        ' Before changing the value, apply the factor
        NewValue = Me.FitParameter.LockedWithFactor * NewValue

        ' Change the value
        Me.FitParameter.ChangeValue(NewValue, False)
        Me.txtValue.SetValue(NewValue, , True)

    End Sub

#End Region

#Region "Value Changed"

    ''' <summary>
    ''' Forwards the parameter-changed event.
    ''' </summary>
    Public Event ValidValueChanged(ByRef SourceParameterBox As mFitParameter)

    ''' <summary>
    ''' Adapt value of the fit-parameter on value-change
    ''' </summary>
    Private Sub txtValue_ValidValueChanged(ByRef ParameterTextBox As NumericTextbox) Handles txtValue.ValidValueChanged
        Me.FitParameter.ChangeValue(ParameterTextBox.DecimalValue)
        RaiseEvent ValidValueChanged(Me)
    End Sub

    ''' <summary>
    ''' Forwards the value to the textbox
    ''' and the fit-parameter.
    ''' </summary>
    Public Sub SetValue(Value As Double)
        If Me.FitParameter Is Nothing Then Return
        Me.FitParameter.ChangeValue(Value)
        Me.txtValue.SetValue(Value)
    End Sub

    ''' <summary>
    ''' Value in the textbox
    ''' </summary>
    Public Property DecimalValue As Double
        Get
            Return Me.txtValue.DecimalValue
        End Get
        Set(value As Double)
            Me.SetValue(value)
        End Set
    End Property

    ''' <summary>
    ''' Value in the textbox
    ''' </summary>
    Public Property Value As Double
        Get
            Return Me.txtValue.DecimalValue
        End Get
        Set(value As Double)
            Me.SetValue(value)
        End Set
    End Property

#End Region

#Region "Boundaries of the parameter."

    ''' <summary>
    ''' Returns the image that displays the Fixation-status.
    ''' </summary>
    Private Sub GetBoundaryPicture(UpperBoundary As Double, LowerBoundary As Double)
        If UpperBoundary < Double.MaxValue Or LowerBoundary > Double.MinValue Then
            Me.pbBoundaries.BackgroundImage = My.Resources.boundaries_16
        Else
            Me.pbBoundaries.BackgroundImage = My.Resources.boundaries_disabled_16
        End If
    End Sub

    ''' <summary>
    ''' Change the boundaries of the parameter.
    ''' </summary>
    Private Sub pbBoundaries_Click(sender As Object, e As EventArgs) Handles pbBoundaries.Click, pbBoundaries.DoubleClick
        Dim oBoundarySelector As New wBoundarySelector
        With Me.FitParameter
            If .UpperBoundary < Double.MaxValue Then
                oBoundarySelector.UpperBoundary = .UpperBoundary
            Else
                oBoundarySelector.UpperBoundary = Double.NaN
            End If
            If .LowerBoundary > Double.MinValue Then
                oBoundarySelector.LowerBoundary = .LowerBoundary
            Else
                oBoundarySelector.LowerBoundary = Double.NaN
            End If
            oBoundarySelector.Location = Cursor.Position
            oBoundarySelector.ShowDialog()
            Me.SetParameterBoundaries(oBoundarySelector.UpperBoundary, oBoundarySelector.LowerBoundary)
        End With
    End Sub

    ''' <summary>
    ''' Sets the actual boundaries to the Fit-Parameter
    ''' </summary>
    Public Sub SetParameterBoundaries(UpperBoundary As Double, LowerBoundary As Double)
        With Me.FitParameter
            If Not Double.IsNaN(UpperBoundary) Then
                .UpperBoundary = UpperBoundary
            Else
                .UpperBoundary = Double.MaxValue
            End If
            If Not Double.IsNaN(LowerBoundary) Then
                .LowerBoundary = LowerBoundary
            Else
                .LowerBoundary = Double.MinValue
            End If
            Me.GetBoundaryPicture(.UpperBoundary, .LowerBoundary)
        End With
    End Sub

#End Region

#Region "Lock-Factor"

    ''' <summary>
    ''' Saves the factor used for locking the parameter
    ''' </summary>
    Private Sub SetLockFactor()
        With Me.cmnuTxtLockFactor
            ' Check for numeric entry.
            If Not IsNumeric(.Text) Then
                ' Error
                .BackColor = Color.Red
                .ForeColor = Color.White
                .Select(0, .Text.Length)
            Else
                ' Set the parameter
                .BackColor = Color.Green
                .ForeColor = Color.White
                Me.FitParameter.LockedWithFactor = Convert.ToDouble(.Text)

                ' If locked to parameter, set the new lock
                Dim GUIDKV As KeyValuePair(Of Guid, String) = cFitParameter.GetGroupIDFromIdentifier(Me.FitParameter.LockedToParameterIdentifier)
                If Not GUIDKV.Key = Nothing Then
                    Me.txtValue.SetValue(Me._FitParameterGroups.Group(GUIDKV.Key).Parameter(GUIDKV.Value).Value * Me.FitParameter.LockedWithFactor, , True)
                End If

            End If
        End With
    End Sub

    ''' <summary>
    ''' Validates the lock-parameter.
    ''' </summary>
    Private Sub cmnuTxtLockFactor_KeyEnter(sender As Object, ByVal e As KeyEventArgs) Handles cmnuTxtLockFactor.KeyDown

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            Me.SetLockFactor()
        End If

    End Sub

    ''' <summary>
    ''' Validates the lock-parameter.
    ''' </summary>
    Private Sub cmnuTxtLockFactor_LostFocus(sender As Object, e As EventArgs) Handles cmnuTxtLockFactor.LostFocus
        Me.SetLockFactor()
    End Sub

#End Region

End Class
