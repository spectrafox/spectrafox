Public Class wTest

    Private Const ControlCount As Integer = 50000

    Private Sub wTest_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim Height As Integer = 0

        For i As Integer = 0 To ControlCount - 1 Step 1
            Dim l As New Label
            l.Text = "control - " & i.ToString
            l.Location = New Point(3, Height)
            Height += l.Height
            Me.vpList.AddControl(l)
        Next

        Me.vpList.VirtualHeightOfThePanel = Height
        Me.vListScroll.Maximum = Height
    End Sub

    ''' <summary>
    ''' Scrolling of the list via the scroll-bar.
    ''' </summary>
    Private Sub vListScroll_Scroll(sender As Object, e As ScrollEventArgs) Handles vListScroll.Scroll
        If e.ScrollOrientation = ScrollOrientation.VerticalScroll Then
            Me.vpList.ScrollOffset = Me.vListScroll.Value
        End If
    End Sub

    Private Sub vpList_Paint(sender As Object, e As PaintEventArgs) Handles vpList.Paint
        Me.lblVisibleControls.Text = Me.vpList.ControlsVisible.ToString
    End Sub
End Class