Imports System.ComponentModel

Public Class wFitTestGauss

    Const i_N As Integer = 450
    Const d_noise As Double = 7.2
    Const d_A1 As Double = 40.0
    Const d_w1 As Double = 70.0
    Const d_xc1 As Double = i_N / 2

    Private WithEvents gf As New cFitGauss

    Private bw As New BackgroundWorker

    Private d_X As Double()
    Private d_Y As Double()
    Private d_YE As Double()

    Private o_x As Double()
    Private o_y As Double()

    Private Sub wFitTest_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        AddHandler bw.DoWork, AddressOf StartFitting
        AddHandler bw.RunWorkerCompleted, AddressOf FitFinished

        ReDim o_x(i_N)
        ReDim o_y(i_N)
        ReDim d_X(i_N)
        ReDim d_Y(i_N)
        ReDim d_YE(i_N)
        ' Werte generieren
        Dim Rand As New Random
        For n As Integer = 0 To i_N - 1 Step 1
            d_YE(n) = 1
            d_X(n) = n
            d_Y(n) = d_A1 * Math.Exp(-0.5 * ((d_X(n) - d_xc1) / d_w1) * ((d_X(n) - d_xc1) / d_w1)) + d_noise * Rand.NextDouble
        Next

        Me.ZedGraphControl1.GraphPane.AddCurve("test", d_X, d_Y, Color.Black, ZedGraph.SymbolType.None)
        Me.ZedGraphControl1.AxisChange()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If Not bw.IsBusy Then
            bw.RunWorkerAsync()
        End If
    End Sub

    Private Sub StartFitting(sender As Object, e As DoWorkEventArgs)
        gf.Predefine(d_X, d_Y, 0, i_N)
        gf.SetActive(True, True, True, True)
        Dim d_RedChiSq As Double = gf.Fit(0.000000000000001, d_X, d_Y, d_YE, 0, i_N)

        For n As Integer = 0 To i_N - 1 Step 1
            o_x(n) = n
            o_y(n) = gf.A * Math.Exp(-0.5 * ((o_x(n) - gf.xc) / gf.w) * ((o_x(n) - gf.xc) / gf.w))
        Next
    End Sub

    Private Sub FitFinished(sender As Object, e As RunWorkerCompletedEventArgs)
        Me.ZedGraphControl2.GraphPane.AddCurve("test", o_x, o_y, Color.Black, ZedGraph.SymbolType.None)
        Me.ZedGraphControl2.AxisChange()
    End Sub

    Private Delegate Sub _HandleFitEcho(ByRef message As String)
    Private Sub HandleFitEcho(ByRef Message As String) Handles gf.FitEcho
        If Me.TextBox1.InvokeRequired Then
            Dim _delegate As New _HandleFitEcho(AddressOf HandleFitEcho)
            Invoke(_delegate, Message)
        Else
            Me.TextBox1.Text &= Message & vbCrLf
            TextBox1.SelectionStart = TextBox1.Text.Length
            TextBox1.ScrollToCaret()
        End If
    End Sub
End Class