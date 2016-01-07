''' <summary>
''' This class extends the listview to avoid flickering due to double-buffering.
''' Additionally it takes care to keep an responsive UI, because the original
''' ListView fires a SelectedIndexChanged event for EACH item. This needs then
''' a huuuuuge calculation runtime.
''' </summary>
Public Class ListView_DoubleBuffered
    Inherits System.Windows.Forms.ListView

    Private m_changeDelayTimer As Timer = Nothing
    Public Sub New()
        MyBase.New()
        ' Set common properties for our listviews
        If Not SystemInformation.TerminalServerSession Then
            DoubleBuffered = True
            SetStyle(ControlStyles.ResizeRedraw, True)
        End If
    End Sub

    ''' <summary>
    ''' Make sure to properly dispose of the timer
    ''' </summary>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso m_changeDelayTimer IsNot Nothing Then
            RemoveHandler m_changeDelayTimer.Tick, AddressOf ChangeDelayTimerTick
            m_changeDelayTimer.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    ''' <summary>
    ''' Hack to avoid lots of unnecessary change events by marshaling with a timer:
    ''' http://stackoverflow.com/questions/86793/how-to-avoid-thousands-of-needless-listview-selectedindexchanged-events
    ''' </summary>
    Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
        If m_changeDelayTimer Is Nothing Then
            m_changeDelayTimer = New Timer()
            AddHandler m_changeDelayTimer.Tick, AddressOf ChangeDelayTimerTick
            m_changeDelayTimer.Interval = 40
        End If
        ' When a new SelectedIndexChanged event arrives, disable, then enable the
        ' timer, effectively resetting it, so that after the last one in a batch
        ' arrives, there is at least 40 ms before we react, plenty of time 
        ' to wait any other selection events in the same batch.
        m_changeDelayTimer.Enabled = False
        m_changeDelayTimer.Enabled = True
    End Sub

    Private Sub ChangeDelayTimerTick(sender As Object, e As EventArgs)
        m_changeDelayTimer.Enabled = False
        MyBase.OnSelectedIndexChanged(New EventArgs())
    End Sub
End Class
