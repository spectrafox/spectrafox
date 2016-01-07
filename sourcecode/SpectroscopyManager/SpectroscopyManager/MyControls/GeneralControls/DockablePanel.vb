''' <summary>
''' This class is an inheritance of a normal panel,
''' that has the ability to slide out or in at the docked side.
''' </summary>
Public Class DockablePanel
    Inherits MouseBoundPanel

    ''' <summary>
    ''' Determines the sliding direction.
    ''' </summary>
    Private iDirection As Integer = 0

    ''' <summary>
    ''' Creates the Timer, that controlls the sliding.
    ''' </summary>
    Private clsTimer As System.Timers.Timer

    ''' <summary>
    ''' Saves the original size of the panel (Width or Height).
    ''' </summary>
    Private iOriginalSize As Integer = 0

    ''' <summary>
    ''' Returns the current Slide-State.
    ''' </summary>
    Private _CurrentSlideState As SlideStates = SlideStates.SlidOut

    ''' <summary>
    ''' Sets the number of pixels, that the width of the panel is modified per Timer-Tick.
    ''' (Determines the speed of the sliding-animation.)
    ''' </summary>
    Public Property SlidePixelsPerTimerTick As Integer = 25

    ''' <summary>
    ''' Event fired, when the slide-status changed.
    ''' </summary>
    Public Event SlideChanged(CurrentSlideState As SlideStates)

    ''' <summary>
    ''' This function initializes the SlideOut-Procedure.
    ''' </summary>
    Public Sub SlideOut(Optional ByVal WithoutAnimation As Boolean = False)
        If Me.SlideState = SlideStates.SlidOut Then Return
        If Not WithoutAnimation Then
            DoSlide(+1)
        Else
            Me.SetSize(iOriginalSize)
            Me._CurrentSlideState = SlideStates.SlidOut
        End If

        RaiseEvent SlideChanged(Me._CurrentSlideState)
    End Sub

    ''' <summary>
    ''' This function collapses the panel again.
    ''' </summary>
    Public Sub SlideIn(Optional ByVal WithoutAnimation As Boolean = False)
        If Me.SlideState = SlideStates.SlidIn Then Return
        iOriginalSize = Me.GetSize()
        If Not WithoutAnimation Then
            DoSlide(-1)
        Else
            Me.SetSize(0)
            Me._CurrentSlideState = SlideStates.SlidIn
        End If

        RaiseEvent SlideChanged(Me._CurrentSlideState)
    End Sub

    ''' <summary>
    ''' This function returns the current Slide-State of the Panel.
    ''' </summary>
    Public Function SlideState() As SlideStates
        Return Me._CurrentSlideState
    End Function

    ''' <summary>
    ''' This function returns, if the Panel is slidout.
    ''' </summary>
    Public Function IsPanelSlidOut() As Boolean
        If Me._CurrentSlideState = SlideStates.SlidOut Then Return True
        Return False
    End Function

    ''' <summary>
    ''' Calls the timer to start the slide.
    ''' </summary>
    Private Sub DoSlide(ByVal Direction As Integer)
        ' instantiate timer if not already done...
        If clsTimer Is Nothing Then
            clsTimer = New System.Timers.Timer
            clsTimer.Interval = 5
            clsTimer.AutoReset = True
            clsTimer.SynchronizingObject = Me
            AddHandler clsTimer.Elapsed, AddressOf Timer_Tick
        Else
            ' only process one request at a time...
            If clsTimer.Enabled Then Exit Sub
        End If

        ' set up and kick off slide...
        iDirection = Direction
        clsTimer.Enabled = True
    End Sub

    ''' <summary>
    ''' Increases or decreases the size of the panel with each timer-tick.
    ''' </summary>
    Private Sub Timer_Tick(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs)
        If clsTimer.Enabled = False Then Return

        Dim iNewSize As Integer = Me.GetSize + (iDirection * SlidePixelsPerTimerTick)
        If iDirection > 0 And iNewSize > iOriginalSize Then
            iNewSize = iOriginalSize : clsTimer.Enabled = False
            ' Set the new SlideState.
            Me._CurrentSlideState = SlideStates.SlidOut
        ElseIf iDirection < 0 And iNewSize < 0 Then
            iNewSize = 0 : clsTimer.Enabled = False
            ' Set the new SlideState.
            Me._CurrentSlideState = SlideStates.SlidIn
        End If

        ' Check, if the docking location is valid,
        ' by checking for GetSize = -1.
        If Me.GetSize >= 0 Then
            Me.SetSize(iNewSize)
        Else
            ' can't slide...
            clsTimer.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Sets the size of the panel, depending on the current docking state.
    ''' </summary>
    Private Sub SetSize(ByVal iPixels As Integer)
        Select Case Me.Dock
            Case DockStyle.Bottom, DockStyle.Top
                Me.Height = iPixels
            Case DockStyle.Left, DockStyle.Right
                Me.Width = iPixels
        End Select
    End Sub

    ''' <summary>
    ''' Returns the size (width or height) of the
    ''' panel, depending on the docking location.
    ''' </summary>
    Private Function GetSize() As Integer
        Select Case Me.Dock
            Case DockStyle.Bottom, DockStyle.Top
                Return Me.Height
            Case DockStyle.Left, DockStyle.Right
                Return Me.Width
            Case Else
                Return -1
        End Select
    End Function

    ''' <summary>
    ''' Represents the current slide-state.
    ''' </summary>
    Public Enum SlideStates
        SlidIn
        SlidOut
    End Enum
End Class
