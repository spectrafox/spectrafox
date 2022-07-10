Imports System.Runtime.InteropServices

''' <summary>
''' Since there is a bug in the MouseLeave-Event for sub-controls of panels,
''' we have to use a workaround to capture the real mouse-leave events.
''' </summary>
Public Class cMouseBounds
    Implements IMessageFilter

#Region "Events"

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
    ''' Mouse bounds changed!
    ''' </summary>
    Public Event MouseBoundsChanged As EventHandler

    ''' <summary>
    ''' Control area got double-clicked!
    ''' </summary>
    Public Event DoubleClicked As EventHandler

    ''' <summary>
    ''' Control area got right-clicked
    ''' </summary>
    Public Event RightClicked As EventHandler

    ''' <summary>
    ''' Control area got left-clicked
    ''' </summary>
    Public Event LeftClicked As EventHandler

    ''' <summary>
    ''' Mouse moved in the control
    ''' </summary>
    Public Event MouseMoved As EventHandler

    ''' <summary>
    ''' Raised, if a key is pressed.
    ''' </summary>
    Public Event KeyPressed(KeyCode As Keys)

    ''' <summary>
    ''' Mouse buttons pressed during the last action
    ''' </summary>
    Public Property CurrentMouseButtons As MouseButtons = MouseButtons.None

    ''' <summary>
    ''' Keys pressed during the last action
    ''' </summary>
    Public Property CurrentModifierKeys As Keys = Keys.None

    ''' <summary>
    ''' Determines, if this mouse-control should suspend filtering all the messages!
    ''' </summary>
    Public Property SuspendMessageFiltering As Boolean = False


#End Region

#Region "Constructor"

    ''' <summary>
    ''' Control to watch.
    ''' </summary>
    Private _Control As Control

    ''' <summary>
    ''' Reference to the window, where the control is located in,
    ''' to compare the window-handles of the message.
    ''' </summary>
    Public Property ParentWindowHandle As IntPtr = Nothing

    Public Sub New(ByRef ControlToWatch As Control)
        Me._Control = ControlToWatch
    End Sub

#End Region

#Region "Const values to interpret the message queue"

    Private Const WM_KEYDOWN As Integer = &H100

    Private Const WM_NCMOUSEMOVE As Integer = &HA0
    Private Const WM_MOUSEMOVE As Integer = &H200
    Private Const WM_NCMOUSELEAVE As Integer = &H2A2
    Private Const WM_MOUSELEAVE As Integer = &H2A3

    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_NCLBUTTONDOWN As Integer = &H2A1
    Private Const WM_LBUTTONUP As Integer = &H202
    Private Const WM_NCLBUTTONUP As Integer = &H2A2
    Private Const WM_LBUTTONDBLCLK As Integer = &H203
    Private Const WM_NCLBUTTONDBLCLK As Integer = &H2A3
    Private Const WM_RBUTTONDOWN As Integer = &H204
    Private Const WM_NCRBUTTONDOWN As Integer = &H2A4
    Private Const WM_RBUTTONUP As Integer = &H205
    Private Const WM_NCRBUTTONUP As Integer = &H2A5
    Private Const WM_RBUTTONDBLCLK As Integer = &H206
    Private Const WM_NCRBUTTONDBLCLK As Integer = &H2A6

    Private Const WM_MOUSEWHEEL As Integer = &H20A

    Private Const SB_LINEUP As Integer = 0
    Private Const SB_LINEDOWN As Integer = 1
    Private Const SB_PAGEUP As Integer = 2
    Private Const SB_PAGEDOWN As Integer = 3
    Private Const SB_THUMBPOSITION As Integer = 4
    Private Const SB_THUMBTRACK As Integer = 5
    Private Const SB_TOP As Integer = 6
    Private Const SB_BOTTOM As Integer = 7
    Private Const SB_ENDSCROLL As Integer = 8

    Private Const MK_LBUTTON As Integer = &H1
    Private Const MK_RBUTTON As Integer = &H2
    Private Const MK_SHIFT As Integer = &H4
    Private Const MK_CONTROL As Integer = &H8
    Private Const MK_MBUTTON As Integer = &H10
    Private Const MK_XBUTTON1 As Integer = &H20
    Private Const MK_XBUTTON2 As Integer = &H40

    Private Const WM_HSCROLL As Integer = &H114
    Private Const WM_VSCROLL As Integer = &H115
    Private Const WM_NCCALCSIZE As Integer = &H83
    Private Const WM_PAINT As Integer = &HF
    Private Const WM_SIZE As Integer = &H5

    Private Const SB_HORZ As UInteger = 0
    Private Const SB_VERT As UInteger = 1
    Private Const SB_CTL As UInteger = 2
    Private Const SB_BOTH As UInteger = 3

#End Region

#Region "Message Filter"

    ''' <summary>
    ''' Message-Filter function.
    ''' </summary>
    Public Function WndProc(ByRef msg As Message) As Boolean Implements IMessageFilter.PreFilterMessage

        ' check, if this control should filter messages, or not
        If Me.SuspendMessageFiltering Then Return False

        ' Get the message recieving control.
        ' If we can not get it, it's just a general message.
        Dim oControl As Control = Control.FromHandle(msg.HWnd)
        If oControl Is Nothing Then Return False

        ' Get the parent form of the recieving control.
        ' If it is the correct target, handle the filter.
        Dim ParentForm As Control = oControl.TopMostParent ' FindForm
        If ParentForm Is Nothing Then Return False

        ' If we finally have a parent window:
        ' check, that this is the correct window of our control.
        If Not Me.ParentWindowHandle = Nothing Then
            If Me.ParentWindowHandle <> ParentForm.Handle Then
                Return False
            End If
        End If


        Select Case msg.Msg

            Case WM_MOUSEMOVE, WM_NCMOUSEMOVE
                ' Mouse enter and leave!
                CheckMouseBounds(True)

                If MouseInBounds Then
                    Dim butt As System.Windows.Forms.MouseButtons = Me.GetMouseButton(msg.WParam)
                    Me.CurrentMouseButtons = butt
                    Dim ModKey As Keys = Me.GetModifierKeys(msg.WParam)
                    Me.CurrentModifierKeys = ModKey
                    RaiseEvent MouseMoved(Me._Control, EventArgs.Empty)
                End If
                Exit Select

            Case WM_NCMOUSELEAVE, WM_MOUSELEAVE

                If MouseInBounds Then
                    Dim butt As System.Windows.Forms.MouseButtons = Me.GetMouseButton(msg.WParam)
                    Me.CurrentMouseButtons = butt
                    Dim ModKey As Keys = Me.GetModifierKeys(msg.WParam)
                    Me.CurrentModifierKeys = ModKey
                End If

                ' Mouse enter and leave!
                CheckMouseBounds(False)

                Exit Select


            Case WM_LBUTTONDBLCLK
                ' Double-Click on the control
                If MouseInBounds Then
                    RaiseEvent DoubleClicked(Me._Control, EventArgs.Empty)
                End If
                Exit Select

            Case WM_LBUTTONUP
                ' Right-Click on the control
                CheckMouseBounds(False)
                If MouseInBounds Then
                    Dim butt As System.Windows.Forms.MouseButtons = Me.GetMouseButton(msg.WParam)
                    Me.CurrentMouseButtons = butt
                    Dim ModKey As Keys = Me.GetModifierKeys(msg.WParam)
                    Me.CurrentModifierKeys = ModKey
                    RaiseEvent LeftClicked(Me._Control, EventArgs.Empty)
                End If
                Exit Select

            Case WM_RBUTTONUP
                ' Right-Click on the control
                CheckMouseBounds(False)
                If MouseInBounds Then
                    Dim butt As System.Windows.Forms.MouseButtons = Me.GetMouseButton(msg.WParam)
                    Me.CurrentMouseButtons = butt
                    Dim ModKey As Keys = Me.GetModifierKeys(msg.WParam)
                    Me.CurrentModifierKeys = ModKey
                    RaiseEvent RightClicked(Me._Control, EventArgs.Empty)
                End If
                Exit Select


            Case WM_MOUSEWHEEL
                Try
                    If MouseInBounds Then
                        Dim zDelta As Integer = HiWord(GetInt(msg.WParam))
                        Dim y As Integer = HiWord(GetInt(msg.LParam))
                        Dim x As Integer = LoWord(GetInt(msg.LParam))
                        Dim butt As System.Windows.Forms.MouseButtons = Me.GetMouseButton(msg.WParam)
                        Dim arg0 As New System.Windows.Forms.MouseEventArgs(butt, 1, x, y, zDelta)
                        Dim ModKey As Keys = Me.GetModifierKeys(msg.WParam)
                        Me.CurrentModifierKeys = ModKey
                        RaiseEvent ScrollMouseWheel(Me._Control, arg0)
                    End If
                Catch generatedExceptionName As Exception
                End Try

                Exit Select

            Case WM_VSCROLL
                ' Vertical scroll
                Try
                    Dim type As ScrollEventType = getScrollEventType(msg.WParam)
                    Dim arg As New ScrollEventArgs(type, GetScrollPos(Me._Control.Handle, CInt(SB_VERT)))
                    RaiseEvent ScrollVertical(Me._Control, arg)
                Catch generatedExceptionName As Exception
                End Try

                Exit Select

            Case WM_HSCROLL
                ' Horizontal scroll
                Try
                    Dim type As ScrollEventType = getScrollEventType(msg.WParam)
                    Dim arg As New ScrollEventArgs(type, GetScrollPos(Me._Control.Handle, CInt(SB_HORZ)))
                    RaiseEvent ScrollHorizontal(Me._Control, arg)
                Catch generatedExceptionName As Exception
                End Try

                Exit Select

            Case WM_KEYDOWN
                Try
                    If MouseInBounds Then
                        Dim keyCode As Keys = CType(msg.WParam, Keys) And Keys.KeyCode
                        RaiseEvent KeyPressed(keyCode)
                    End If
                Catch ex As Exception

                End Try

            Case Else

                Exit Select
        End Select

        Return False
        ' dont actually filter the message
    End Function

    ''' <summary>
    ''' Gets the mouse-button pressed by the WParam of a message.
    ''' </summary>
    Private Function GetMouseButton(WParam As IntPtr) As System.Windows.Forms.MouseButtons
        Dim butt As System.Windows.Forms.MouseButtons
        Select Case LoWord(GetInt(WParam))
            Case MK_LBUTTON
                butt = System.Windows.Forms.MouseButtons.Left
                Exit Select
            Case MK_MBUTTON
                butt = System.Windows.Forms.MouseButtons.Middle
                Exit Select
            Case MK_RBUTTON
                butt = System.Windows.Forms.MouseButtons.Right
                Exit Select
            Case MK_XBUTTON1
                butt = System.Windows.Forms.MouseButtons.XButton1
                Exit Select
            Case MK_XBUTTON2
                butt = System.Windows.Forms.MouseButtons.XButton2
                Exit Select
            Case Else
                butt = System.Windows.Forms.MouseButtons.None
                Exit Select
        End Select
        Return butt
    End Function

    ''' <summary>
    ''' Gets the modifier keys pressed by the WParam of a message.
    ''' </summary>
    Private Function GetModifierKeys(WParam As IntPtr) As Keys
        Dim butt As Keys = Keys.None
        Select Case LoWord(GetInt(WParam))
            Case MK_CONTROL
                butt = Keys.Control
                Exit Select
            Case MK_SHIFT
                butt = Keys.Shift
                Exit Select
        End Select
        Return butt
    End Function

#End Region

#Region "Check if the mouse is in the bounds of the control."

    Private _MouseInBounds As Boolean = False

    ''' <summary>
    ''' Checks if the current cursor position is contained within the bounds of the
    ''' form and sets MouseInBounds which in turn fires event MouseBoundsChanged
    ''' </summary>
    Private Sub CheckMouseBounds(MouseMove As Boolean)
        ' Already know the mouse is in the bounds, so we dont
        ' care that the mouse just moved (saves unnecessary checks
        ' on the form bounds and OnMouseBoundsChanged calls)
        If MouseInBounds AndAlso MouseMove Then Return

        ' Get the current control
        Dim c As Control = Me._Control
        ' Return, if no parent could be found.
        ' This control is then not shown, and
        ' therefore could not be the one having the focus
        If c.Parent Is Nothing Then
            SetMouseBounds(False)
            Return
        End If

        ' Get the rectangle to check
        Dim RectangleToCheck As Rectangle = _Control.RectangleToScreen(c.ClientRectangle)
        While True
            RectangleToCheck = Rectangle.Intersect(RectangleToCheck, c.RectangleToScreen(c.ClientRectangle))
            If c.Parent Is Nothing Then Exit While
            c = c.Parent
        End While

        ' if the cursor is in the bounds of the current panel 
        ' set the bounds to true, else set the bounds to false           
        SetMouseBounds(RectangleToCheck.Contains(Cursor.Position))
    End Sub

    Public Sub SetMouseBounds(MouseInBounds As Boolean)
        ' prevent setting the bounds status to the same as it was
        ' already set (shouldn't happen anyway, so this is just a
        ' sanity check)
        If MouseInBounds <> _MouseInBounds Then
            Me._MouseInBounds = MouseInBounds
            OnMouseBoundsChanged(EventArgs.Empty)
        End If
    End Sub

    ''' <summary>
    ''' Raise Event
    ''' </summary>
    Private Sub OnMouseBoundsChanged(e As EventArgs)
        RaiseEvent MouseBoundsChanged(Me._Control, e)
    End Sub

    ''' <summary>
    ''' Returns, if the mouse is currently in the bound of the control.
    ''' </summary>
    Public ReadOnly Property MouseInBounds() As Boolean
        Get
            Return Me._MouseInBounds
        End Get
    End Property

#End Region

#Region "ScrollEventType to SB_* messages and SB_* messages to ScrollEventType"

    Private Function getSBFromScrollEventType(type As ScrollEventType) As Integer
        Dim res As Integer = -1
        Select Case type
            Case ScrollEventType.SmallDecrement
                res = SB_LINEUP
                Exit Select
            Case ScrollEventType.SmallIncrement
                res = SB_LINEDOWN
                Exit Select
            Case ScrollEventType.LargeDecrement
                res = SB_PAGEUP
                Exit Select
            Case ScrollEventType.LargeIncrement
                res = SB_PAGEDOWN
                Exit Select
            Case ScrollEventType.ThumbTrack
                res = SB_THUMBTRACK
                Exit Select
            Case ScrollEventType.First
                res = SB_TOP
                Exit Select
            Case ScrollEventType.Last
                res = SB_BOTTOM
                Exit Select
            Case ScrollEventType.ThumbPosition
                res = SB_THUMBPOSITION
                Exit Select
            Case ScrollEventType.EndScroll
                res = SB_ENDSCROLL
                Exit Select
            Case Else
                Exit Select
        End Select
        Return res
    End Function

    Private Function getScrollEventType(wParam As System.IntPtr) As ScrollEventType
        Dim res As ScrollEventType = 0
        Select Case LoWord(GetInt(wParam))
            Case SB_LINEUP
                res = ScrollEventType.SmallDecrement
                Exit Select
            Case SB_LINEDOWN
                res = ScrollEventType.SmallIncrement
                Exit Select
            Case SB_PAGEUP
                res = ScrollEventType.LargeDecrement
                Exit Select
            Case SB_PAGEDOWN
                res = ScrollEventType.LargeIncrement
                Exit Select
            Case SB_THUMBTRACK
                res = ScrollEventType.ThumbTrack
                Exit Select
            Case SB_TOP
                res = ScrollEventType.First
                Exit Select
            Case SB_BOTTOM
                res = ScrollEventType.Last
                Exit Select
            Case SB_THUMBPOSITION
                res = ScrollEventType.ThumbPosition
                Exit Select
            Case SB_ENDSCROLL
                res = ScrollEventType.EndScroll
                Exit Select
            Case Else
                res = ScrollEventType.EndScroll
                Exit Select
        End Select
        Return res
    End Function

#End Region

#Region "Perform Manual scrolling"

    Public Sub ManualScrollHorizontal(type As ScrollEventType)
        Dim param As Integer = getSBFromScrollEventType(type)
        If param = -1 Then
            Return
        End If
        SendMessage(Me._Control.Handle, CUInt(WM_HSCROLL), CType(param, System.UIntPtr), CType(0, System.IntPtr))
    End Sub

    Public Sub ManualScrollVertical(type As ScrollEventType)
        Dim param As Integer = getSBFromScrollEventType(type)
        If param = -1 Then
            Return
        End If
        SendMessage(Me._Control.Handle, CUInt(WM_VSCROLL), CType(param, System.UIntPtr), CType(0, System.IntPtr))
    End Sub

#End Region

#Region "API32 functions"

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Public Shared Function GetSystemMetrics(code As Integer) As Integer
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function EnableScrollBar(hWnd As System.IntPtr, wSBflags As UInteger, wArrows As UInteger) As Boolean
    End Function

    '[DllImport("user32.dll")]
    'static public extern bool GetScrollInfo(System.IntPtr hwnd, int fnBar, LPSCROLLINFO lpsi);

    <DllImport("user32.dll")> _
    Public Shared Function SetScrollRange(hWnd As System.IntPtr, nBar As Integer, nMinPos As Integer, nMaxPos As Integer, bRedraw As Boolean) As Integer
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function SetScrollPos(hWnd As System.IntPtr, nBar As Integer, nPos As Integer, bRedraw As Boolean) As Integer
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function GetScrollPos(hWnd As System.IntPtr, nBar As Integer) As Integer
    End Function

    '
    '		public struct LPSCROLLINFO 
    '		{ 
    '			uint cbSize; 
    '			uint fMask; 
    '			int  nMin; 
    '			int  nMax; 
    '			uint nPage; 
    '			int  nPos; 
    '			int  nTrackPos; 
    '		}
    '		


    <DllImport("user32.dll")> _
    Public Shared Function ShowScrollBar(hWnd As System.IntPtr, wBar As Integer, bShow As Boolean) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As UInteger, wParam As UIntPtr, lParam As IntPtr) As IntPtr
    End Function

    Private Shared Function MakeLong(LoWord As Integer, HiWord As Integer) As Integer
        Return (HiWord << 16) Or (LoWord And &HFFFF)
    End Function

    Private Shared Function MakeLParam(LoWord As Integer, HiWord As Integer) As IntPtr
        Return CType((HiWord << 16) Or (LoWord And &HFFFF), IntPtr)
    End Function

    Private Shared Function HiWord(number As Integer) As Integer
        If (number And &H80000000UI) = &H80000000UI Then
            Return (number >> 16)
        Else
            Return (number >> 16) And &HFFFF
        End If
    End Function

    Private Shared Function LoWord(number As Integer) As Integer
        Return number And &HFFFF
    End Function

    Private Shared Function GetInt(ptr As IntPtr) As Integer
        If IntPtr.Size = 8 Then
            Return CInt(ptr.ToInt64())
        Else
            Return ptr.ToInt32()
        End If
    End Function

#End Region

End Class
