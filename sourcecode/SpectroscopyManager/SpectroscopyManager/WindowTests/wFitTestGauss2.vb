Imports System.ComponentModel

Public Class wFitTestGauss2
    Inherits wFormBase

    Const i_N As Integer = 400
    Const d_noise As Double = 56.2
    Const d_A1 As Double = 40.0
    Const d_w1 As Double = 70.0
    Const d_xc1 As Double = i_N / 2

    Private WithEvents gf As cLMAFit
    Private ff As New cFitFunction_Gauss

    Private d_X As Double()
    Private d_Y As Double()
    Private d_YE As Double()

    Private o_x As Double()
    Private o_y As Double()

    Private Sub wFitTest_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Y0) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Y0).ChangeValue(0)
        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.XCenter) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.XCenter).ChangeValue(d_xc1)
        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Width) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Width).ChangeValue(d_w1)
        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Stretch) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Stretch).ChangeValue(d_A1)

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
            d_Y(n) = ff.GetY(d_X(n), ff.FitParameters) + d_noise * Rand.NextDouble
        Next

        Me.ZedGraphControl1.GraphPane.AddCurve("Sample Data", d_X, d_Y, Color.Black, ZedGraph.SymbolType.None)
        Me.ZedGraphControl1.AxisChange()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim StartTime As Date = Now

        Dim Rand As New Random

        Dim Y0 As Double = 3 + d_noise * Rand.NextDouble
        Dim A As Double = 20 + d_A1 + d_noise * Rand.NextDouble
        Dim W As Double = 30 + d_w1 + d_noise * Rand.NextDouble
        Dim XC As Double = 60 + d_xc1 + d_noise * Rand.NextDouble

        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Y0) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Y0).ChangeValue(Y0)
        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.XCenter) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.XCenter).ChangeValue(XC)
        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Width) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Width).ChangeValue(W)
        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Stretch) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Stretch).ChangeValue(A)


        ' Initial Echo
        HandleFitEcho("Fit procedure started")
        HandleFitEcho("------------------")
        HandleFitEcho("Initial Set of Parameters: ")
        HandleFitEcho("Y-Offset:         " & Y0)
        HandleFitEcho("Stretch:          " & A)
        HandleFitEcho("Width:            " & W)
        HandleFitEcho("XCenter:          " & XC)
        HandleFitEcho("------------------")
        HandleFitEcho("Fit running ... good luck!")

        Dim DataPoints()() As Double = New Double()() {d_X, d_Y}

        Dim epsf As Double = 0
        ' Smallest step size -> accuracy of the result.
        Dim epsx As Double = 0.000001

        ' Create Fit-Object
        Me.gf = New cLMAFit(ff,
                            DataPoints,
                            Nothing,
                            ,
                            1000)

        ' Start Fit async
        Me.gf.FitAsync()
    End Sub

    ''' <summary>
    ''' Fit Finished Function
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FitFinished(FitStopReason As cLMAFit.FitStopReason,
                            FinalParameters As Dictionary(Of Integer, sFitParameter),
                            Chi2 As Double) Handles gf.FitFinished

        Dim Y0 As Double = FinalParameters(0).Value
        Dim A As Double = FinalParameters(1).Value
        Dim W As Double = FinalParameters(2).Value
        Dim XC As Double = FinalParameters(3).Value

        ' Echo the final set of Parameters
        ' and fit results, depending on the final Fit-Report.
        HandleFitEcho("Fit finished: ")
        Select Case FitStopReason
            Case cLMAFit.FitStopReason.FitConverged
                HandleFitEcho("Fit-Stop-Reason: relative function improvement is no more than: " & Me.gf.MinimumDeltaChi2)
            Case cLMAFit.FitStopReason.MaxIterationsReached
                HandleFitEcho("Fit-Stop-Reason: Maximum iterations were taken: " & Me.gf.MaximumIterations.ToString("N0"))
            Case cLMAFit.FitStopReason.UserAborted
                HandleFitEcho("Fit-Stop-Reason: The fit procedure was aborted!")
        End Select
        HandleFitEcho("Interation Count: " & Me.gf.Iterations.ToString("N0"))
        'HandleFitEcho("Average Error: " & .ToString(TextBoxFormat))
        'HandleFitEcho("RMS Error: " & .ToString(TextBoxFormat))
        'HandleFitEcho("Max Error: " & .ToString(TextBoxFormat))
        HandleFitEcho("------------------")
        HandleFitEcho("Final Set of Parameters: ")
        HandleFitEcho("------------------")
        HandleFitEcho("Initial Set of Parameters: ")
        HandleFitEcho("Y-Offset:         " & Y0 & " (real: " & ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Y0).Value & ")")
        HandleFitEcho("Stretch:          " & A & " (real: " & ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Stretch).Value & ")")
        HandleFitEcho("Width:            " & W & " (real: " & ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Width).Value & ")")
        HandleFitEcho("XCenter:          " & XC & " (real: " & ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.XCenter).Value & ")")
        HandleFitEcho("------------------")
        HandleFitEcho("Fit procedure ended after " & Me.gf.FitDuration.TotalMinutes.ToString("N0") & " minutes!")

        For n As Integer = 0 To i_N - 1 Step 1
            d_YE(n) = 1
            o_x(n) = n
            o_y(n) = ff.FitFunction(o_x(n), Y0, A, W, XC)
        Next

        ' If the Fit-Operation was successfull, show final set of data!
        If FitStopReason <> cLMAFit.FitStopReason.UserAborted Then
            With Me.ZedGraphControl2
                .GraphPane.CurveList.Clear()
                .GraphPane.Title.Text = "FitResult"
                Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", o_x, o_y, Color.Black, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
                ' Real Data
                LI = .GraphPane.AddCurve("", d_X, d_Y, Color.Blue, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
                .AxisChange()
            End With
        End If

        Me.gf = Nothing
    End Sub

    Private Delegate Sub _HandleFitEcho(Message As String, FitParameters As Dictionary(Of Integer, sFitParameter))
    Private Sub HandleFitEcho(Message As String, FitParameters As Dictionary(Of Integer, sFitParameter))
        If Me.TextBox1.InvokeRequired Then
            Dim _delegate As New _HandleFitEcho(AddressOf HandleFitEcho)
            Invoke(_delegate, Message, FitParameters)
        Else
            If Not FitParameters Is Nothing Then

                Dim Y0 As Double = FitParameters(0).Value
                Dim A As Double = FitParameters(1).Value
                Dim W As Double = FitParameters(2).Value
                Dim XC As Double = FitParameters(3).Value

                For n As Integer = 0 To i_N - 1 Step 1
                    d_YE(n) = 1
                    o_x(n) = n
                    o_y(n) = ff.FitFunction(o_x(n), Y0, A, W, XC)
                Next

                ' If the Fit-Operation was successfull, show final set of data!
                With Me.ZedGraphControl2
                    .GraphPane.CurveList.Clear()
                    .GraphPane.Title.Text = "FitResult"
                    Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", o_x, o_y, Color.Black, ZedGraph.SymbolType.Circle)
                    LI.Line.IsVisible = False
                    LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
                    ' Real Data
                    LI = .GraphPane.AddCurve("", d_X, d_Y, Color.Blue, ZedGraph.SymbolType.Circle)
                    LI.Line.IsVisible = False
                    LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
                    .AxisChange()
                    .Refresh()
                End With
            End If

            Me.TextBox1.Text &= Message & vbCrLf
            TextBox1.SelectionStart = TextBox1.Text.Length
            TextBox1.ScrollToCaret()
        End If
    End Sub

    Private Sub HandleFitEcho(Message As String)
        If Me.TextBox1.InvokeRequired Then
            Dim _delegate As New _HandleFitEcho(AddressOf HandleFitEcho)
            Invoke(_delegate, Message, Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Report-Function for getting the status of the Fit.
    ''' </summary>
    Private Sub ReportFunction(ByVal FitParameters As Dictionary(Of Integer, sFitParameter), Chi2 As Double) Handles gf.FitStepEcho
        Dim sb As New System.Text.StringBuilder
        sb.Append(sFitParameter.GetShortParameterEcho(FitParameters))
        sb.Append("Calculated Chi2 >> ")
        sb.Append(Chi2.ToString("N8"))
        sb.Append(" << ")

        HandleFitEcho(sb.ToString, FitParameters)
    End Sub

    Private Sub ckbFixWidth_CheckedChanged(sender As Object, e As EventArgs) Handles ckbFixWidth.CheckedChanged
        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Width) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Width).ChangeFixed(ckbFixWidth.Checked)
    End Sub

    Private Sub ckbFixHeight_CheckedChanged(sender As Object, e As EventArgs) Handles ckbFixHeight.CheckedChanged

        ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Stretch) = ff.FitParameters(cFitFunction_Gauss.FitParameterIdentifier.Stretch).ChangeFixed(ckbFixHeight.Checked)

    End Sub

    Private Sub NumericTextbox1_TextChanged(ByRef TB As NumericTextbox) Handles NumericTextbox1.ValidValueChanged
        MessageBox.Show(TB.DecimalValue.ToString)
    End Sub

End Class