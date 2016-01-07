Public Class ProgressPanel
    Inherits Panel

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        If DesignMode Then Me.Visible = False
    End Sub

    End Class
