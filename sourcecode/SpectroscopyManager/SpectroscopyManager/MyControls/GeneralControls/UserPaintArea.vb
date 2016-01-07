Public Class UserPaintArea
    Inherits System.Windows.Forms.Panel

    Public Sub New()
        MyBase.New()

        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.UserPaint Or
                    ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.ResizeRedraw Or
                    ControlStyles.ContainerControl Or
                    ControlStyles.OptimizedDoubleBuffer Or
                    ControlStyles.SupportsTransparentBackColor, True)
    End Sub
End Class
