Imports System.Windows.Forms
Imports System.Runtime.InteropServices

''' <summary>
''' Description résumée de ScrollablePanel.
''' </summary>
Public Class ScrollablePanel
    Inherits System.Windows.Forms.Panel

#Region "Delegates & events"

    Public Event ScrollHorizontal As System.Windows.Forms.ScrollEventHandler
    Public Event ScrollVertical As System.Windows.Forms.ScrollEventHandler
    Public Event ScrollMouseWheel As System.Windows.Forms.MouseEventHandler

#End Region

#Region "Const"

    Private Const SB_LINEUP As Integer = 0
    Private Const SB_LINEDOWN As Integer = 1
    Private Const SB_PAGEUP As Integer = 2
    Private Const SB_PAGEDOWN As Integer = 3
    Private Const SB_THUMBPOSITION As Integer = 4
    Private Const SB_THUMBTRACK As Integer = 5
    Private Const SB_TOP As Integer = 6
    Private Const SB_BOTTOM As Integer = 7
    Private Const SB_ENDSCROLL As Integer = 8

    Private Const WM_HSCROLL As Integer = &H114
    Private Const WM_VSCROLL As Integer = &H115
    Private Const WM_MOUSEWHEEL As Integer = &H20A
    Private Const WM_NCCALCSIZE As Integer = &H83
    Private Const WM_PAINT As Integer = &HF
    Private Const WM_SIZE As Integer = &H5

    Private Const SB_HORZ As UInteger = 0
    Private Const SB_VERT As UInteger = 1
    Private Const SB_CTL As UInteger = 2
    Private Const SB_BOTH As UInteger = 3

    Private Const ESB_DISABLE_BOTH As UInteger = &H3
    Private Const ESB_ENABLE_BOTH As UInteger = &H0

    Private Const MK_LBUTTON As Integer = &H1
    Private Const MK_RBUTTON As Integer = &H2
    Private Const MK_SHIFT As Integer = &H4
    Private Const MK_CONTROL As Integer = &H8
    Private Const MK_MBUTTON As Integer = &H10
    Private Const MK_XBUTTON1 As Integer = &H20
    Private Const MK_XBUTTON2 As Integer = &H40

#End Region

#Region "Vars"

    Private enableAutoHorizontal As Boolean = True
    Private enableAutoVertical As Boolean = True
    Private visibleAutoHorizontal As Boolean = True
    Private visibleAutoVertical As Boolean = True

    Private m_autoScrollHorizontalMinimum As Integer = 0
    Private m_autoScrollHorizontalMaximum As Integer = 100

    Private m_autoScrollVerticalMinimum As Integer = 0
    Private m_autoScrollVerticalMaximum As Integer = 100

#End Region

#Region "Constructor"

    Public Sub New()
        AddHandler Me.Click, New EventHandler(AddressOf ScrollablePanel_Click)
        Me.AutoScroll = True
    End Sub

#End Region

#Region "Properties"

    Public Property AutoScrollHPos() As Integer
        Get
            Return GetScrollPos(Me.Handle, CInt(SB_HORZ))
        End Get
        Set(value As Integer)
            SetScrollPos(Me.Handle, CInt(SB_HORZ), value, True)
        End Set
    End Property

    Public Property AutoScrollVPos() As Integer
        Get
            Return GetScrollPos(Me.Handle, CInt(SB_VERT))
        End Get
        Set(value As Integer)
            SetScrollPos(Me.Handle, CInt(SB_VERT), value, True)
        End Set
    End Property

    Public Property AutoScrollHorizontalMinimum() As Integer
        Get
            Return Me.m_autoScrollHorizontalMinimum
        End Get
        Set(value As Integer)
            Me.m_autoScrollHorizontalMinimum = value
            SetScrollRange(Me.Handle, CInt(SB_HORZ), m_autoScrollHorizontalMinimum, m_autoScrollHorizontalMaximum, True)
        End Set
    End Property

    Public Property AutoScrollHorizontalMaximum() As Integer
        Get
            Return Me.m_autoScrollHorizontalMaximum
        End Get
        Set(value As Integer)
            Me.m_autoScrollHorizontalMaximum = value
            SetScrollRange(Me.Handle, CInt(SB_HORZ), m_autoScrollHorizontalMinimum, m_autoScrollHorizontalMaximum, True)
        End Set
    End Property

    Public Property AutoScrollVerticalMinimum() As Integer
        Get
            Return Me.m_autoScrollVerticalMinimum
        End Get
        Set(value As Integer)
            Me.m_autoScrollVerticalMinimum = value
            SetScrollRange(Me.Handle, CInt(SB_VERT), m_autoScrollHorizontalMinimum, m_autoScrollHorizontalMaximum, True)
        End Set
    End Property

    Public Property AutoScrollVerticalMaximum() As Integer
        Get
            Return Me.m_autoScrollVerticalMaximum
        End Get
        Set(value As Integer)
            Me.m_autoScrollVerticalMaximum = value
            SetScrollRange(Me.Handle, CInt(SB_VERT), m_autoScrollHorizontalMinimum, m_autoScrollHorizontalMaximum, True)
        End Set
    End Property


    Public Property EnableAutoScrollHorizontal() As Boolean
        Get
            Return Me.enableAutoHorizontal
        End Get
        Set(value As Boolean)
            Me.enableAutoHorizontal = value
            If value Then
                EnableScrollBar(Me.Handle, SB_HORZ, ESB_ENABLE_BOTH)
            Else
                EnableScrollBar(Me.Handle, SB_HORZ, ESB_DISABLE_BOTH)
            End If
        End Set
    End Property

    Public Property EnableAutoScrollVertical() As Boolean
        Get
            Return Me.enableAutoVertical
        End Get
        Set(value As Boolean)
            Me.enableAutoVertical = value
            If value Then
                EnableScrollBar(Me.Handle, SB_VERT, ESB_ENABLE_BOTH)
            Else
                EnableScrollBar(Me.Handle, SB_VERT, ESB_DISABLE_BOTH)
            End If
        End Set
    End Property

    Public Property VisibleAutoScrollHorizontal() As Boolean
        Get
            Return Me.visibleAutoHorizontal
        End Get
        Set(value As Boolean)
            Me.visibleAutoHorizontal = value
            ShowScrollBar(Me.Handle, CInt(SB_HORZ), value)
        End Set
    End Property

    Public Property VisibleAutoScrollVertical() As Boolean
        Get
            Return Me.visibleAutoVertical
        End Get
        Set(value As Boolean)
            Me.visibleAutoVertical = value
            ShowScrollBar(Me.Handle, CInt(SB_VERT), value)
        End Set
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
        Select Case LoWord(CInt(wParam))
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

#Region "WndProd override"

    Protected Overrides Sub WndProc(ByRef msg As Message)
        MyBase.WndProc(msg)
        If msg.HWnd <> Me.Handle Then
            Return
        End If
        Select Case msg.Msg
            Case WM_MOUSEWHEEL
                If Not Me.VisibleAutoScrollVertical Then
                    Return
                End If
                Try
                    Dim zDelta As Integer = HIWORD(CInt(msg.WParam))
                    Dim y As Integer = HIWORD(CInt(msg.LParam))
                    Dim x As Integer = LoWord(CInt(msg.LParam))
                    Dim butt As System.Windows.Forms.MouseButtons
                    Select Case LoWord(CInt(msg.WParam))
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
                    Dim arg0 As New System.Windows.Forms.MouseEventArgs(butt, 1, x, y, zDelta)
                    RaiseEvent ScrollMouseWheel(Me, arg0)
                Catch generatedExceptionName As Exception
                End Try

                Exit Select

            Case WM_VSCROLL

                Try
                    Dim type As ScrollEventType = getScrollEventType(msg.WParam)
                    Dim arg As New ScrollEventArgs(type, GetScrollPos(Me.Handle, CInt(SB_VERT)))
                    RaiseEvent ScrollVertical(Me, arg)
                Catch generatedExceptionName As Exception
                End Try

                Exit Select

            Case WM_HSCROLL

                Try
                    Dim type As ScrollEventType = getScrollEventType(msg.WParam)
                    Dim arg As New ScrollEventArgs(type, GetScrollPos(Me.Handle, CInt(SB_HORZ)))
                    RaiseEvent ScrollHorizontal(Me, arg)
                Catch generatedExceptionName As Exception
                End Try

                Exit Select
            Case Else

                Exit Select
        End Select
    End Sub

#End Region

#Region "Perform Manuel scrolling"

    Public Sub performScrollHorizontal(type As ScrollEventType)
        Dim param As Integer = getSBFromScrollEventType(type)
        If param = -1 Then
            Return
        End If
        SendMessage(Me.Handle, CUInt(WM_HSCROLL), CType(param, System.UIntPtr), CType(0, System.IntPtr))
    End Sub

    Public Sub performScrollVertical(type As ScrollEventType)
        Dim param As Integer = getSBFromScrollEventType(type)
        If param = -1 Then
            Return
        End If
        SendMessage(Me.Handle, CUInt(WM_VSCROLL), CType(param, System.UIntPtr), CType(0, System.IntPtr))
    End Sub

#End Region

#Region "Panel Got focus"

    Private Sub ScrollablePanel_Click(sender As Object, e As EventArgs)
        Me.Focus()
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

    <DllImport("user32.dll")> _
    Private Shared Function HIWORD(wParam As System.IntPtr) As Integer
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

#End Region

End Class