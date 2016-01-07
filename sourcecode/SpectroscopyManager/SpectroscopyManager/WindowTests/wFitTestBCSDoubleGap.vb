Imports System.ComponentModel
Imports alglib

Public Class wFitTestBCSDoubleGap

    Private bReady As Boolean = False

    Const i_N As Integer = 500

    Private WithEvents f As New cFitBCSDoubleGap

    Private bw As New BackgroundWorker

    Private d_X As Double()
    Private d_X2 As Double(,)
    Private d_Y As Double()
    Private d_YE As Double()

    Private o_x As Double()
    Private o_y As Double()

    Dim Delta_Tip As Double = 1.34
    Dim Delta_Sample1 As Double = 1.42
    Dim Delta_Sample2 As Double = 1.32
    Dim RatioSample1ToSample2 As Double = 0.1
    Dim GlobalXOffset As Double = 0.1
    Dim GlobalYOffset As Double = 0.1
    Dim GlobalYStretch As Double = 1
    Dim T_tip As Double = 1.19
    Dim T_sample As Double = 1.19
    Dim wBroad As Double = 0.005
    ' Werte generieren
    Dim d_noise As Double = 0.5
    Dim dMaxE As Integer = 5

    Private Sub wFitTest_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        AddHandler bw.DoWork, AddressOf StartFitting
        AddHandler bw.RunWorkerCompleted, AddressOf FitFinished

        ReDim o_x(i_N)
        ReDim o_y(i_N)
        ReDim d_X(i_N)
        ReDim d_X2(i_N, 1)
        ReDim d_Y(i_N)
        ReDim d_YE(i_N)

        f.maxE = dMaxE
        f.minE = -dMaxE

        ' Check, if BCSDOS needs to be recalculated - speeds up everything
        f.PrecalcBCSDOS(Delta_Tip, Delta_Sample1, Delta_Sample2, wBroad, cFitBCSDoubleGap.Broadening.Gauss, 0.015)

        Dim Rand As New Random
        For n As Integer = 0 To i_N - 1 Step 1
            d_YE(n) = 1
            d_X(n) = -dMaxE + dMaxE / i_N * 2 * n
            d_X2(n, 0) = d_X(n)
            d_Y(n) = f.FitFunction(d_X(n), Delta_Tip, Delta_Sample1, Delta_Sample2, T_tip, T_sample, wBroad, cFitBCSDoubleGap.Broadening.Gauss, 0.015, RatioSample1ToSample2, GlobalXOffset, GlobalYOffset, GlobalYStretch, cFitBCSDoubleGap.FitFunctionType.I, 0.01)
            ' + d_noise * (Rand.NextDouble - Rand.NextDouble)
        Next

        Me.zgMeasuredData.GraphPane.AddCurve("test DOS", d_X, d_Y, Color.Black, ZedGraph.SymbolType.None)
        Me.zgMeasuredData.AxisChange()

        Me.bReady = True
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnStartFitting.Click
        If Not bw.IsBusy Then
            bw.RunWorkerAsync()
        End If
    End Sub

    Public Sub function_I(c As Double(), x As Double(), ByRef func As Double, obj As Object)
        ' Check, if BCSDOS needs to be recalculated - speeds up everything
        f.PrecalcBCSDOS(Delta_Tip, c(0), c(1), c(2), cFitBCSDoubleGap.Broadening.Gauss, 0.015)

        func = f.FitFunction(x(0), Delta_Tip, c(0), c(1), T_tip, T_sample, c(2), cFitBCSDoubleGap.Broadening.Gauss, 0.015, c(3), c(4), c(5), c(6), cFitBCSDoubleGap.FitFunctionType.dIdV, 0.1)
    End Sub

    Private Sub ReportFunction(ByVal arg As Double(), func As Double, obj As Object)
        HandleFitEcho(alglib.ap.format(arg, 5) & " - f(x)=" & func.ToString)
    End Sub

    Private Sub StartFitting(sender As Object, e As DoWorkEventArgs)
        'f.Predefine(d_X, d_Y, 0, i_N)
        'f.SetActive(True, True, True, True)
        'Dim d_RedChiSq As Double = f.Fit(0.000000000000001, d_X, d_Y, d_YE, 0, i_N)

        HandleFitEcho("Fit Started: " & Now.ToString("dd.MM.yyyy HH:mm:ss"))

        Dim x(,) As Double = d_X2
        Dim y() As Double = d_Y
        Dim c() As Double = New Double() {Delta_Sample1 - 0.00523, Delta_Sample2 + 0.0024, wBroad, RatioSample1ToSample2 + 0.00123, GlobalXOffset + 0.001, GlobalYOffset + 0.00232, GlobalYStretch + 0.02}
        Dim bndl() As Double = New Double() {0.0}
        Dim bndu() As Double = New Double() {1.0}
        Dim epsf As Double = 0
        Dim epsx As Double = 0.000001
        Dim maxits As Integer = 0
        Dim info As Integer
        Dim state As lsfitstate = New alglib.lsfitstate() ' initializer can be dropped, but compiler will issue warning
        Dim rep As lsfitreport = New alglib.lsfitreport() ' initializer can be dropped, but compiler will issue warning
        Dim diffstep As Double = 0.0001

        ' Check, if BCSDOS needs to be recalculated - speeds up everything
        f.PrecalcBCSDOS(Delta_Tip, c(0), c(1), c(2), cFitBCSDoubleGap.Broadening.Gauss, 0.015)

        alglib.lsfitcreatef(x, y, c, diffstep, state)
        'alglib.lsfitsetbc(state, bndl, bndu)
        alglib.lsfitsetcond(state, epsf, epsx, maxits)
        alglib.lsfitsetxrep(state, True)
        alglib.lsfitfit(state, AddressOf function_I, AddressOf ReportFunction, Nothing)
        alglib.lsfitresults(state, info, c, rep)

        HandleFitEcho("Fit finished: " & alglib.ap.format(c, 2))
        HandleFitEcho("Fit finished: " & alglib.ap.format({Delta_Sample1, Delta_Sample2, wBroad, RatioSample1ToSample2, GlobalXOffset, GlobalYOffset, GlobalYStretch}, 2))
        HandleFitEcho("Fit finished: " & Now.ToString("dd.MM.yyyy HH:mm:ss"))

        o_x = d_X
        For n As Integer = 0 To i_N - 1 Step 1
            o_y(n) = f.FitFunction(d_X(n), Delta_Tip, c(0), c(1), T_tip, T_sample, c(2), cFitBCSDoubleGap.Broadening.Gauss, 0.015, c(3), c(4), c(5), c(6), cFitBCSDoubleGap.FitFunctionType.dIdV, 0.01)
        Next

        Return
    End Sub

    Private Sub FitFinished(sender As Object, e As RunWorkerCompletedEventArgs)
        Me.zgMeasuredData.GraphPane.AddCurve("Fit-Result", o_x, o_y, Color.Blue, ZedGraph.SymbolType.Plus)
        MsgBox("Finished")
        Me.zgMeasuredData.RestoreScale(Me.zgMeasuredData.GraphPane)
        Me.zgMeasuredData.AxisChange()
        Me.zgMeasuredData.Refresh()
    End Sub

    Private Delegate Sub _HandleFitEcho(ByRef message As String)
    Private Sub HandleFitEcho(ByRef Message As String)
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