Public Class MouseBoundPanel
    Inherits Panel

    ''' <summary>
    ''' Check for Mouse-Bound
    ''' </summary>
    Public WithEvents MB As New cMouseBounds(Me)

    ''' <summary>
    ''' Constructor of the base class is called.
    ''' </summary>
    Public Sub New()
        MyBase.New()

        ' Add the MouseBound-Message-Filter
        Application.AddMessageFilter(MB)
    End Sub

    ''' <summary>
    ''' dispose überschreiben, um den Message-Filter für das MouseOver wieder zu entfernen!
    ''' </summary>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Application.RemoveMessageFilter(MB)
        MyBase.Dispose(disposing)
    End Sub

    ''' <summary>
    ''' Returns the parent window of the mouse-bound-control.
    ''' </summary>
    Public Property ParentWindowHandle As IntPtr
        Get
            Return Me.MB.ParentWindowHandle
        End Get
        Set(value As IntPtr)
            Me.MB.ParentWindowHandle = value
        End Set
    End Property

    ''' <summary>
    ''' Determines, if this mouse-control should suspend filtering all the messages!
    ''' </summary>
    Public Property SuspendMessageFiltering As Boolean
        Get
            Return Me.MB.SuspendMessageFiltering
        End Get
        Set(value As Boolean)
            Me.MB.SuspendMessageFiltering = value
        End Set
    End Property

#Region "Handle Mouse-Over or Mouse-Out Events to get the panel to slide out or in"

    ''' <summary>
    ''' Corrected Version of the Mouse-Leave-Event, that considers
    ''' also all Child-Objects!
    ''' </summary>
    Public Event MouseLeave_PanelArea As EventHandler

    ''' <summary>
    ''' Corrected Version of the Mouse-Enter-Event, that considers
    ''' also all Child-Objects!
    ''' </summary>
    Public Event MouseEnter_PanelArea As EventHandler

    ''' <summary>
    ''' Handle a Mouse-Bound-Change
    ''' </summary>
    Public Sub HandleMouseBoundChange(sender As Object, e As EventArgs) Handles MB.MouseBoundsChanged
        If MB.MouseInBounds Then
            RaiseEvent MouseEnter_PanelArea(sender, e)
        Else
            RaiseEvent MouseLeave_PanelArea(sender, e)
        End If
    End Sub

#End Region

#Region "Handle Mouse Move"

    ''' <summary>
    ''' Corrected Version of the Mouse-Move-Event, that considers
    ''' also all Child-Objects!
    ''' </summary>
    Public Event MouseMove_ControlArea As EventHandler

    ''' <summary>
    ''' Handle a Mouse-Move
    ''' </summary>
    Public Sub HandleMouseMove(sender As Object, e As EventArgs) Handles MB.MouseMoved
        RaiseEvent MouseMove_ControlArea(sender, e)
    End Sub

#End Region

#Region "Handle Scroll Events"

    ''' <summary>
    ''' Horizontal scroll
    ''' </summary>
    Public Event ScrollHorizontal As ScrollEventHandler

    ''' <summary>
    ''' Vertcal scroll
    ''' </summary>
    Public Event ScrollVertical As ScrollEventHandler

    ''' <summary>
    ''' Scrolling performed using the mouse-wheel
    ''' </summary>
    Public Event ScrollMouseWheel As MouseEventHandler

    ''' <summary>
    ''' Handle a scrolling
    ''' </summary>
    Public Sub OnScrollHorizontal(sender As Object, e As ScrollEventArgs) Handles MB.ScrollHorizontal
        RaiseEvent ScrollHorizontal(sender, e)
    End Sub

    ''' <summary>
    ''' Handle a scrolling
    ''' </summary>
    Public Sub OnScrollVertical(sender As Object, e As ScrollEventArgs) Handles MB.ScrollVertical
        RaiseEvent ScrollVertical(sender, e)
    End Sub

    ''' <summary>
    ''' Handle a scrolling
    ''' </summary>
    Public Sub OnScrollMouseWheel(sender As Object, e As MouseEventArgs) Handles MB.ScrollMouseWheel
        RaiseEvent ScrollMouseWheel(sender, e)
    End Sub

#End Region

#Region "Handle Click Events"

    ''' <summary>
    ''' Double Click on the Control
    ''' </summary>
    Public Event DoubleClicked As EventHandler

    ''' <summary>
    ''' Handle a double clicking
    ''' </summary>
    Public Sub OnDoubleClicked(sender As Object, e As EventArgs) Handles MB.DoubleClicked
        RaiseEvent DoubleClicked(sender, e)
    End Sub

    ''' <summary>
    ''' Right Click on the Control
    ''' </summary>
    Public Event RightClicked As EventHandler

    ''' <summary>
    ''' Handle a left clicking
    ''' </summary>
    Public Sub OnRightClicked(sender As Object, e As EventArgs) Handles MB.RightClicked
        RaiseEvent RightClicked(sender, e)
    End Sub

    ''' <summary>
    ''' Left Click on the Control
    ''' </summary>
    Public Event LeftClicked As EventHandler

    ''' <summary>
    ''' Handle a Left clicking
    ''' </summary>
    Public Sub OnLeftClicked(sender As Object, e As EventArgs) Handles MB.LeftClicked
        RaiseEvent LeftClicked(sender, e)
    End Sub

    ''' <summary>
    ''' Returns the current mouse-buttons pressed.
    ''' </summary>
    Public ReadOnly Property CurrentMouseButtons As MouseButtons
        Get
            Return Me.MB.CurrentMouseButtons
        End Get
    End Property

    ''' <summary>
    ''' Returns the current modifier-keys pressed.
    ''' </summary>
    Public ReadOnly Property CurrentModifierKeys As Keys
        Get
            Return Me.MB.CurrentModifierKeys
        End Get
    End Property

#End Region

#Region "KeyPressed"
    ''' <summary>
    ''' Raised, if a key is pressed.
    ''' </summary>
    Public Event KeyPressed(KeyCode As Keys)

    ''' <summary>
    ''' Handle a key pressed
    ''' </summary>
    Public Sub OnKeyPressed(KeyCode As Keys) Handles MB.KeyPressed
        RaiseEvent KeyPressed(KeyCode)
    End Sub
#End Region

End Class
