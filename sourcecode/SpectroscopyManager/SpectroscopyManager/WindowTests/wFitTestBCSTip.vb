Imports System.ComponentModel
Imports System.Threading
Imports alglib

Public Class wFitTestBCSTip

    Const i_N As Integer = 500

    Private WithEvents f As New cFitBCSTip


    Private d_X As Double()
    Private d_Y As Double()
    Private d_YE As Double()

    Private o_x As Double()
    Private o_y As Double()

    Private oFitBCSTip As New cFitBCSTip
    Private d_X2 As Double(,)

    Public Sub function_tip_func(c As Double(), x As Double(), ByRef func As Double, obj As Object)
        func = Me.oFitBCSTip.FuncF(x(0), c(0), c(1), cFitBaseParameter.Broadening.Gauss)
        '
        ' this callback calculates f(c,x)=c[0]*(1+c[1]*(pow(x[0]-1999,c[2])-1))
        '
        'func = c(0) * (1 + c(1) * (System.Math.Pow(x(0) - 1999, c(2)) - 1))
    End Sub

    Private Sub wFitTest_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Me.ZedGraphControl1.PointSelectionMode = mZedGraph.SelectionModes.SinglePoint


        ReDim o_x(i_N)
        ReDim o_y(i_N)
        ReDim d_X(i_N)
        ReDim d_X2(i_N, 1)
        ReDim d_Y(i_N)
        ReDim d_YE(i_N)

        Dim d_noise As Double = 0.5
        Dim gap As Double = 150
        ' Werte generieren
        Dim Rand As New Random
        For n As Integer = 0 To i_N - 1 Step 1
            d_YE(n) = 1
            d_X(n) = n - i_N / 2
            d_X2(n, 0) = n - i_N / 2
            d_Y(n) = f.FuncF(d_X(n), gap, 2, cFitBaseParameter.Broadening.None) + d_noise * Rand.NextDouble
            'd_Y(n) = Math.Abs(((d_X(n) + dBroad) / (System.Numerics.Complex.Sqrt((d_X(n) + dBroad) * (d_X(n) + dBroad) - dGap * dGap))).Real) + d_noise * Rand.NextDouble
        Next

        f.dM.fFit = False
        f.dV0.fFit = False
        f.dY0.fFit = False
        f.SrcData = cFitBaseParameter.DataType.IV
        f.FitData = cFitBaseParameter.DataType.IV
        f.minE = -i_N / 2
        f.maxE = i_N / 2
        f.dWt.dVal = 2.54
        f.dDt.dVal = gap - d_noise * Rand.NextDouble

        Me.ZedGraphControl1.GraphPane.AddCurve("test", d_X, d_Y, Color.Black, ZedGraph.SymbolType.None)
        Me.ZedGraphControl1.AxisChange()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    End Sub


  
End Class