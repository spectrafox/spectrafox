Public Class VirtualVerticalPanel
    Inherits Panel

    ''' <summary>
    ''' Offset of the width of the sub-controls.
    ''' </summary>
    Private Const WidthOffset As Integer = 5

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        Me.DoubleBuffered = True
    End Sub

    ''' <summary>
    ''' Virtual height of the panel that is "emulated" on displaying the list.
    ''' </summary>
    Private _VirtualHeightOfThePanel As Integer = Me.Height

    ''' <summary>
    ''' Virtual height of the panel that is "emulated" on displaying the list.
    ''' </summary>
    Public Property VirtualHeightOfThePanel As Integer
        Get
            Return Me._VirtualHeightOfThePanel
        End Get
        Set(value As Integer)
            Me._VirtualHeightOfThePanel = value
        End Set
    End Property

    ''' <summary>
    ''' Scroll position (offset) on displaying the list.
    ''' </summary>
    Private _ScrollOffset As Integer = 0

    ''' <summary>
    ''' Scroll position (offset) on displaying the list.
    ''' 0 = top most position of the panel!
    ''' </summary>
    Public Property ScrollOffset As Integer
        Get
            Return Me._ScrollOffset
        End Get
        Set(value As Integer)
            Me._ScrollOffset = value

            ' Reposition controls
            Me.RepositionControls()
        End Set
    End Property

    ''' <summary>
    ''' Keeps all virtual controls in the background.
    ''' </summary>
    Private ControlBackgroundContainer As New List(Of VirtualControl)

    ''' <summary>
    ''' Virtual control, that manages the visibility,
    ''' and the location of the control.
    ''' </summary>
    Private Class VirtualControl
        Public Control As Control
        Public IsVisible As Boolean
        Public OriginalLocation As Point
    End Class

    ''' <summary>
    ''' On Control Added.
    ''' </summary>
    Public Sub AddControl(C As Control,
                          Optional ByVal AutomaticallyRepositionControls As Boolean = True)
        ' if it does not exist, create new
        Dim VControl As New VirtualControl
        VControl.Control = C
        VControl.OriginalLocation = C.Location
        VControl.IsVisible = False
        Me.ControlBackgroundContainer.Add(VControl)

        ' Set control properties
        C.Width = Me.Width - WidthOffset

        ' Refresh afterwards
        If AutomaticallyRepositionControls Then Me.RepositionControls()
    End Sub

    ''' <summary>
    ''' On Control removed
    ''' </summary>
    Public Sub RemoveControl(C As Control)
        For i As Integer = 0 To Me.ControlBackgroundContainer.Count - 1 Step 1
            If Me.ControlBackgroundContainer(i).Control Is C Then
                If Me.Controls.Contains(Me.ControlBackgroundContainer(i).Control) Then
                    Me.Controls.Remove(Me.ControlBackgroundContainer(i).Control)
                End If
                Me.ControlBackgroundContainer.RemoveAt(i)
                Me.RepositionControls()
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Removes all controls.
    ''' </summary>
    Public Sub ClearControls()
        Me.Controls.Clear()
        Me.ControlBackgroundContainer.Clear()
    End Sub

    ''' <summary>
    ''' Changes the location of the control.
    ''' </summary>
    Public Sub ChangeControlLocation(C As Control,
                                     ByVal NewLocation As Point,
                                     Optional ByVal AutomaticallyRepositionControls As Boolean = True)
        ' Check, if the control already exists.
        ' If it does, it updates the original position.
        For i As Integer = 0 To Me.ControlBackgroundContainer.Count - 1 Step 1
            If Me.ControlBackgroundContainer(i).Control Is C Then
                Me.ControlBackgroundContainer(i).OriginalLocation = NewLocation
                ' Refresh afterwards
                If AutomaticallyRepositionControls Then Me.RepositionControls()
                Return
            End If
        Next
    End Sub

    ''' <summary>
    ''' Counts all visible controls at the moment.
    ''' </summary>
    Public ControlsVisible As Integer = 0

    ' ''' <summary>
    ' ''' Overrides the OnPaint event, to paint the viewable part of the panel.
    ' ''' </summary>
    'Protected Overrides Sub OnPaint(e As PaintEventArgs)
    '    ' before painting, 
    '    ' shift all the control to the new virtual location!
    '    Dim NewLocation As Point
    '    ControlsVisible = 0
    '    For Each C As VirtualControl In Me.ControlBackgroundContainer
    '        NewLocation = New Point(C.OriginalLocation.X, C.OriginalLocation.Y - Me._ScrollOffset)

    '        ' Is the control visible?
    '        If (NewLocation.Y + C.Control.Height) > 0 And
    '           NewLocation.Y < e.ClipRectangle.Height Then
    '            ' VISIBLE
    '            '#########
    '            C.Control.Location = NewLocation
    '            If Not C.IsVisible Then
    '                'C.Control.ResumeLayout()
    '                Me.Controls.Add(C.Control)
    '                C.IsVisible = True
    '            End If
    '            ControlsVisible += 1
    '        Else
    '            ' HIDDEN
    '            '########
    '            If C.IsVisible Then
    '                Me.Controls.Remove(C.Control)
    '                C.IsVisible = False
    '                'C.Control.SuspendLayout()
    '            End If
    '        End If
    '    Next

    '    ' paint
    '    MyBase.OnPaint(e)
    'End Sub

    ''' <summary>
    ''' Repositions all the control after scrolling.
    ''' </summary>
    Public Sub RepositionControls()
        ' before painting, 
        ' shift all the control to the new virtual location!
        Dim NewLocation As Point
        ControlsVisible = 0
        For Each C As VirtualControl In Me.ControlBackgroundContainer
            NewLocation = New Point(C.OriginalLocation.X, C.OriginalLocation.Y - Me._ScrollOffset)

            ' Is the control visible?
            If (NewLocation.Y + C.Control.Height) > 0 And
               NewLocation.Y < (Me.ClientRectangle.Height + C.Control.Height) Then
                ' VISIBLE
                '#########
                C.Control.Location = NewLocation
                If Not C.IsVisible Then
                    'C.Control.ResumeLayout()
                    Me.Controls.Add(C.Control)
                    C.IsVisible = True
                End If
                ControlsVisible += 1
            Else
                ' HIDDEN
                '########
                If C.IsVisible Then
                    Me.Controls.Remove(C.Control)
                    C.IsVisible = False
                    'C.Control.SuspendLayout()
                End If
            End If
        Next
        'Me.Refresh()
    End Sub

    ''' <summary>
    ''' Resize all client-controls.
    ''' </summary>
    Private Sub VirtualVerticalPanel_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ' Set the width of all controls
        For i As Integer = 0 To Me.ControlBackgroundContainer.Count - 1 Step 1
            Me.ControlBackgroundContainer(i).Control.Width = Me.Width - WidthOffset
        Next

        ' Reposition controls
        Me.RepositionControls()
    End Sub

End Class
