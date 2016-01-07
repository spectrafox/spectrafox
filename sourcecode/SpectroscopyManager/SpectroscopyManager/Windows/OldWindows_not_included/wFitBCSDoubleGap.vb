Imports System.ComponentModel
Imports alglib

''' <summary>
''' All units are given in [meV and pA]
''' </summary>
''' <remarks></remarks>
Public Class wFitBCSDoubleGap
    Inherits wFormBase

    Private TextBoxFormat As String = "N6"

#Region "Objects used to fit, such as Fit-Class and SpectroscopyTable"
    Private bReady As Boolean = False

    ''' <summary>
    ''' SpectroscopyTable with the data to fit.
    ''' </summary>
    ''' <remarks></remarks>
    Private oSpectroscopyTable As cSpectroscopyTable

    ''' <summary>
    ''' Fit-Class
    ''' </summary>
    ''' <remarks></remarks>
    Private f As New cFitBCSDoubleGap

    ''' <summary>
    ''' Background-Worker Thread for Fitting.
    ''' </summary>
    ''' <remarks></remarks>
    Private BW As New BackgroundWorker

    ''' <summary>
    ''' Fit-Exception that should be thrown,
    ''' if the user requests a cancellation of the fit!
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CancellationPendingException
        Inherits Exception
    End Class
#End Region

#Region "Fit-Parameters"

    Private _YCorrection As Boolean = False
    ''' <summary>
    ''' Gap-Width of the Tip.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property YCorrection As Boolean
        Get
            Return _YCorrection
        End Get
        Set(value As Boolean)
            _YCorrection = value
            Me.ckbYCorrection.Checked = value
        End Set
    End Property

    Private _Delta_Tip As Double = 1.34
    ''' <summary>
    ''' Gap-Width of the Tip.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Delta_Tip As Double
        Get
            Return _Delta_Tip
        End Get
        Set(value As Double)
            _Delta_Tip = value
            Me.txtTipGap.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _Delta_Sample1 As Double = 1.42
    ''' <summary>
    ''' First gap of the Sample.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Delta_Sample1 As Double
        Get
            Return _Delta_Sample1
        End Get
        Set(value As Double)
            _Delta_Sample1 = value
            Me.txtSampleGap1.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _Delta_Sample2 As Double = 1.32
    ''' <summary>
    ''' Second gap of the Sample.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Delta_Sample2 As Double
        Get
            Return _Delta_Sample2
        End Get
        Set(value As Double)
            _Delta_Sample2 = value
            Me.txtSampleGap2.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _RatioSample1ToSample2 As Double = 0.1
    ''' <summary>
    ''' Hight-Ratio between Sample-Gap 1 and Sample-Gap 2.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property RatioSample1ToSample2 As Double
        Get
            Return _RatioSample1ToSample2
        End Get
        Set(value As Double)
            _RatioSample1ToSample2 = value
            Me.txtGapRatio2vs1.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _GlobalXOffset As Double = 0
    ''' <summary>
    ''' Energy shift in X due to a non-perfect bias-gauge.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property GlobalXOffset As Double
        Get
            Return _GlobalXOffset
        End Get
        Set(value As Double)
            _GlobalXOffset = value
            Me.txtXOffset.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _GlobalYOffset As Double = 0
    ''' <summary>
    ''' Shift in Y.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property GlobalYOffset As Double
        Get
            Return _GlobalYOffset
        End Get
        Set(value As Double)
            _GlobalYOffset = value
            Me.txtYOffset.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _GlobalYStretch As Double = 1
    ''' <summary>
    ''' Strech-Factor of the data to fit the height of the sample.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property GlobalYStretch As Double
        Get
            Return _GlobalYStretch
        End Get
        Set(value As Double)
            _GlobalYStretch = value
            Me.txtYStretch.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _T_tip As Double = 1.19
    ''' <summary>
    ''' Effective Temperature of the tip.
    ''' Always set equal to the temperature of the sample!
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property T_tip As Double
        Get
            Return _T_tip
        End Get
        Set(value As Double)
            _T_tip = value
            _T_sample = value
            Me.txtTemperature.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _T_sample As Double = 1.19
    ''' <summary>
    ''' Effective Temperature of the sample
    ''' Always set equal to the temperature of the tip!
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property T_sample As Double
        Get
            Return _T_sample
        End Get
        Set(value As Double)
            _T_sample = value
            _T_tip = value
            Me.txtTemperature.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _BroadeningWidth As Double = 0.05
    ''' <summary>
    ''' Broadening width of the broadening applied to the BCS function.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property BroadeningWidth As Double
        Get
            Return _BroadeningWidth
        End Get
        Set(value As Double)
            _BroadeningWidth = value
            Me.txtBroadeningWidth.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _BroadeningType As cFitBCS.Broadening = cFitBCS.Broadening.ImaginaryDamping
    ''' <summary>
    ''' Type of Broadening applied to the BCS function!
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property BroadeningType As cFitBCS.Broadening
        Get
            Return _BroadeningType
        End Get
        Set(value As cFitBCS.Broadening)
            _BroadeningType = value
            With Me.cboBroadeningType
                Dim SelectedBroadening As cFitBCS.Broadening = DirectCast(.SelectedItem, KeyValuePair(Of String, cFitBCS.Broadening)).Value
                If SelectedBroadening <> value Then
                    For i As Integer = 0 To .Items.Count - 1 Step 1
                        If DirectCast(.Items(i), KeyValuePair(Of String, cFitBCS.Broadening)).Value = value Then
                            .SelectionStart = i
                        End If
                    Next
                End If

                ' Turn on or off imaginary damping, depending on the selected
                ' Broadening method:
                If SelectedBroadening <> cFitBCS.Broadening.None Then
                    If SelectedBroadening <> cFitBCS.Broadening.ImaginaryDamping Then
                        Me.txtBroadeningWidth.Enabled = True
                    Else
                        Me.txtBroadeningWidth.Enabled = False
                    End If
                Else
                    Me.txtBroadeningWidth.Enabled = False
                End If
            End With
        End Set
    End Property

    Private _ImaginaryDamping As Double = 0.015
    ''' <summary>
    ''' Broadening width of the broadening applied to the BCS function.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property ImaginaryDamping As Double
        Get
            Return _ImaginaryDamping
        End Get
        Set(value As Double)
            _ImaginaryDamping = value
            Me.txtImaginaryDamping.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _FitDataType As cFitBCSDoubleGap.FitFunctionType = cFitBCSDoubleGap.FitFunctionType.dIdV
    ''' <summary>
    ''' Type of data that should be fitted.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property FitDataType As cFitBCSDoubleGap.FitFunctionType
        Get
            Return _FitDataType
        End Get
        Set(value As cFitBCSDoubleGap.FitFunctionType)
            _FitDataType = value
            With Me.cboFitDataType
                Dim SelectedFitFunction As cFitBCSDoubleGap.FitFunctionType = DirectCast(.SelectedItem, KeyValuePair(Of String, cFitBCSDoubleGap.FitFunctionType)).Value
                If SelectedFitFunction <> value Then
                    For i As Integer = 0 To .Items.Count - 1 Step 1
                        If DirectCast(.Items(i), KeyValuePair(Of String, cFitBCSDoubleGap.FitFunctionType)).Value = value Then
                            .SelectionStart = i
                        End If
                    Next
                End If
            End With
        End Set
    End Property

    Private _dIdVDerivationStepWidth As Double = 0.01
    ''' <summary>
    ''' Energy-sampling size, that should be used for the numerical derivation
    ''' of the Current-Function, to get to dIdV.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property dIdVDerivationStepWidth As Double
        Get
            Return _dIdVDerivationStepWidth
        End Get
        Set(value As Double)
            _dIdVDerivationStepWidth = value
        End Set
    End Property

    Private _dMaxE As Double = 5
    ''' <summary>
    ''' Minimal Energy to consider for fitting.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property dMaxE As Double
        Get
            Return _dMaxE
        End Get
        Set(value As Double)
            _dMaxE = value
            f.maxE = value
            Me.txtFitRangeMax.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _dMinE As Double = -5
    ''' <summary>
    ''' Maximal Energy to consider for fitting.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property dMinE As Double
        Get
            Return _dMinE
        End Get
        Set(value As Double)
            _dMinE = value
            f.minE = value
            Me.txtFitRangeMin.SetValue(value, TextBoxFormat)
        End Set
    End Property
#End Region

#Region "Preview-Settings"
    '#####################
    Private _i_N_Preview As Integer = 500
    ''' <summary>
    ''' Number of Points used for calculating the preview of the Fit-Data.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property i_N_Preview As Integer
        Get
            Return _i_N_Preview
        End Get
        Set(value As Integer)
            _i_N_Preview = value
            If nudPreviewPoints.Value <> value Then
                nudPreviewPoints.Value = value
            End If
        End Set
    End Property

    Private i_N_Preview_Tip As Integer = 500
    Private i_N_Fit As Integer = 500
#End Region

#Region "Data-Arrays for saving the Preview-, Fit, and Output-Data"
    ' Tip Preview-Data
    Private d_X_TipDOS As Double()
    Private d_Y_TipDOS As Double()
    Private d_Y_FermiFunctionTip As Double()

    Private d_X_SampleDOS As Double()
    Private d_Y_SampleDOS As Double()
    Private d_Y_FermiFunctionSample As Double()

    Private d_X_SampleFitFunction As Double()
    Private d_Y_SampleFitFunction As Double()

    Private d_X_FitData As Double()
    Private d_X2_FitData As Double(,)
    Private d_Y_FitData As Double()

    Private d_Output_X As Double()
    Private d_Output_Y_FitData As Double()
    Private d_Output_Y_SampleDOS As Double()
    Private d_Output_Y_TipDOS As Double()
    Private d_Output_Y_FermiFunction As Double()
#End Region

#Region "Initialization of the Spectroscopy-Table, and applying the last settings"
    ''' <summary>
    ''' Sets the used Spectroscopy-Table and enables the Interface.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetSpectroscopyTable(ByRef SpectroscopyTable As cSpectroscopyTable)
        Me.Enabled = True

        Me.oSpectroscopyTable = SpectroscopyTable

        ' Set Preview-Image:
        'Me.pbExperimentalData.SetPreviewImage(Me.oSpectroscopyTable)
        Me.cbX.InitializeColumns(Me.oSpectroscopyTable.GetColumnList)
        Me.cbY.InitializeColumns(Me.oSpectroscopyTable.GetColumnList)
    End Sub

    ''' <summary>
    ''' Window Initialization
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub wFitTest_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        BW.WorkerSupportsCancellation = True
        AddHandler BW.DoWork, AddressOf StartFitting
        AddHandler BW.RunWorkerCompleted, AddressOf FitFinished

        ' Fill the Comboboxes
        With Me.cboBroadeningType
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, cFitBCS.Broadening)(My.Resources.BCSFitting_Broad_None, cFitBCS.Broadening.None))
            .Items.Add(New KeyValuePair(Of String, cFitBCS.Broadening)(My.Resources.BCSFitting_Broad_ImaginaryDamping, cFitBCS.Broadening.ImaginaryDamping))
            .Items.Add(New KeyValuePair(Of String, cFitBCS.Broadening)(My.Resources.BCSFitting_Broad_Gauss, cFitBCS.Broadening.Gauss))
            .Items.Add(New KeyValuePair(Of String, cFitBCS.Broadening)(My.Resources.BCSFitting_Broad_Lorentz, cFitBCS.Broadening.Lorentz))
            .ValueMember = "Value"
            .DisplayMember = "Key"
            .SelectedIndex = 1
        End With

        With Me.cboFitDataType
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, cFitBCSDoubleGap.FitFunctionType)(My.Resources.BCSFitting_FitFunctionType_I, cFitBCSDoubleGap.FitFunctionType.I))
            .Items.Add(New KeyValuePair(Of String, cFitBCSDoubleGap.FitFunctionType)(My.Resources.BCSFitting_FitFunctionType_dIdV, cFitBCSDoubleGap.FitFunctionType.dIdV))
            .ValueMember = "Value"
            .DisplayMember = "Key"
            .SelectedIndex = 1
        End With

        ' Set tip and sample temperature to the same
        Me.T_sample = Me.T_tip

        ' Set Initial Values
        ' Load Properties From Settings if possible!
        With My.Settings
            Me.Delta_Tip = .LastBCSDoubleGapFit_Delta_Tip
            Me.BroadeningWidth = .LastBCSDoubleGapFit_BroadeningWidth
            Me.ImaginaryDamping = .LastBCSDoubleGapFit_ImaginaryDampingFactor
            Me.Delta_Sample1 = .LastBCSDoubleGapFit_Delta_Sample1
            Me.Delta_Sample2 = .LastBCSDoubleGapFit_Delta_Sample2
            Me.RatioSample1ToSample2 = .LastBCSDoubleGapFit_RatioSample1ToSample2
            Me.GlobalXOffset = .LastBCSDoubleGapFit_GlobalXOffset
            Me.GlobalYOffset = .LastBCSDoubleGapFit_GlobalYOffset
            Me.GlobalYStretch = .LastBCSDoubleGapFit_GlobalYStretch
            Me.dMaxE = .LastBCSDoubleGapFit_MaxE
            Me.dMinE = .LastBCSDoubleGapFit_MinE
            Me.BroadeningType = DirectCast(.LastBCSDoubleGapFit_BroadeningType, cFitBCS.Broadening)
            Me.FitDataType = DirectCast(.LastBCSDoubleGapFit_FitDataType, cFitBCSDoubleGap.FitFunctionType)
            Me.i_N_Preview = .LastBCSDoubleGapFit_LastPreviewPoints
            Me.YCorrection = .LastBCSDoubleGapFit_YCorrectionActive

            Me.cbX.SelectedColumnName = .LastBCSDoubleGapFit_ColumnNameX
            Me.cbY.SelectedColumnName = .LastBCSDoubleGapFit_ColumnNameY
            Me.txtColNameFitResult.Text = .LastBCSDoubleGapFit_SaveColFitResult
            Me.txtColNameSampleDOS.Text = .LastBCSDoubleGapFit_SaveColSampleDOS
            Me.txtColNameTipDOS.Text = .LastBCSDoubleGapFit_SaveColTipDOS
            Me.txtColNameFermiFunction.Text = .LastBCSDoubleGapFit_SaveColFermiFunction
        End With

        Me.bReady = True

        ' Paint TIP-DOS:
        PaintSimulatedTipDOS()
        FitColumnsSelected()
    End Sub
#End Region

    ''' <summary>
    ''' Set the Fit-Data, if the all Columns are selected.
    ''' </summary>
    Private Sub FitColumnsSelected() Handles cbX.SelectedIndexChanged, cbY.SelectedIndexChanged
        If Not Me.bReady Then Return

        If Me.cbX.SelectedColumnIndex = -1 Or
            Me.cbY.SelectedColumnIndex = -1 Then Return

        ' Set Number of Fit-Points
        i_N_Fit = Me.oSpectroscopyTable.MeasurementPoints

        ' Set min and max E, expected: eV -> convert to meV
        Me.dMinE = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnIndex).GetMinimumValueOfColumn * 1000
        Me.dMaxE = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnIndex).GetMaximumValueOfColumn * 1000

        Me.gbParameters.Enabled = True
        Me.btnStartFitting.Enabled = True

        ' Set Fit-Range:
        SetFitRangeAndFitData()
    End Sub

    ''' <summary>
    ''' Extracts the Fit-Data from the SpectroscopyTable in the
    ''' specified energy-limits, and saves it in the Fit-Array.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetFitRangeAndFitData()
        Dim xValues As cSpectroscopyTable.DataColumn = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnIndex)
        Dim yValues As cSpectroscopyTable.DataColumn = Me.oSpectroscopyTable.Column(Me.cbY.SelectedColumnIndex)
        ' Set data in the Fit-Range to the Fit-Arrays
        Dim lXIndicesToConsider As New List(Of Integer)
        For i As Integer = 0 To xValues.Values.Count - 1 Step 1
            If xValues.Values(i) >= _dMinE And xValues.Values(i) <= _dMaxE Then
                lXIndicesToConsider.Add(i)
            End If
        Next

        ' Resize Fit-Arrays
        ReDim d_X_FitData(lXIndicesToConsider.Count - 1)
        ReDim d_Y_FitData(d_X_FitData.Length - 1)
        ReDim d_X2_FitData(d_X_FitData.Length - 1, 0)

        For n As Integer = 0 To lXIndicesToConsider.Count - 1 Step 1
            ' Fit Data is given in A(I) and eV
            ' Convert it to nA(I) and meV
            d_X_FitData(n) = 1000 * xValues.Values(lXIndicesToConsider(n))
            d_X2_FitData(n, 0) = d_X_FitData(n)
            d_Y_FitData(n) = yValues.Values(lXIndicesToConsider(n))
            If YCorrection Then
                If Me.FitDataType = cFitBCSDoubleGap.FitFunctionType.I Then
                    d_Y_FitData(n) *= 10000000000.0
                ElseIf Me.FitDataType = cFitBCSDoubleGap.FitFunctionType.dIdV Then
                    d_Y_FitData(n) *= 1000000.0
                End If
            End If
        Next

        PaintSimulatedSampleDOSAndFitFunction()
    End Sub

    ''' <summary>
    ''' Paints the Sample-DOS using the current parameter-set,
    ''' and overlays the experimental data.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PaintSimulatedSampleDOSAndFitFunction()
        If Me.cbX.SelectedColumnIndex = -1 Or
            Me.cbY.SelectedColumnIndex = -1 Then Return

        ReDim d_X_SampleFitFunction(_i_N_Preview)
        ReDim d_Y_SampleFitFunction(_i_N_Preview)
        ReDim d_X_SampleDOS(_i_N_Preview)
        ReDim d_Y_SampleDOS(_i_N_Preview)
        ReDim d_Y_FermiFunctionSample(_i_N_Preview)

        f.minE = _dMinE
        f.maxE = _dMaxE

        ' Initialize first Precalc-DOS:
        f.PrecalcBCSDOS(_Delta_Tip, _Delta_Sample1, _Delta_Sample2, _BroadeningWidth, _BroadeningType, _ImaginaryDamping)

        For n As Integer = 0 To _i_N_Preview - 1 Step 1
            d_X_SampleFitFunction(n) = _dMinE + (_dMaxE - _dMinE) / _i_N_Preview * n
            d_Y_SampleFitFunction(n) = f.FitFunction(d_X_SampleFitFunction(n),
                                          _Delta_Tip,
                                          _Delta_Sample1,
                                          _Delta_Sample2,
                                          _T_tip,
                                          _T_sample,
                                          _BroadeningWidth,
                                          _BroadeningType,
                                          _ImaginaryDamping,
                                          _RatioSample1ToSample2,
                                          _GlobalXOffset,
                                          _GlobalYOffset,
                                          _GlobalYStretch,
                                          _FitDataType,
                                          _dIdVDerivationStepWidth)
            d_X_SampleDOS(n) = _dMinE + (_dMaxE - _dMinE) / _i_N_Preview * n
            d_Y_SampleDOS(n) = _RatioSample1ToSample2 * f.BCSFuncPrecalc(d_X_SampleDOS(n), 1, cFitBCSDoubleGap.PrecalcDOSType.Sample1) +
                               f.BCSFuncPrecalc(d_X_SampleDOS(n), 1, cFitBCSDoubleGap.PrecalcDOSType.Sample2)
            d_Y_FermiFunctionSample(n) = f.FermiF_eV(d_X_SampleDOS(n), _T_sample)
        Next

        With zgFitResult
            .GraphPane.CurveList.Clear()
            .GraphPane.Title.Text = "Fit Data Preview from Current Parameter Set"
            .GraphPane.XAxis.Title.Text = "Energy (meV)"
            .GraphPane.YAxis.Title.Text = Me.FitDataType.ToString
            ' Simulated Sample
            Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", d_X_SampleFitFunction, d_Y_SampleFitFunction, Color.Black, ZedGraph.SymbolType.Circle)
            LI.Line.IsVisible = False
            LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
            ' Real Data
            LI = .GraphPane.AddCurve("", d_X_FitData, d_Y_FitData, Color.Blue, ZedGraph.SymbolType.Circle)
            LI.Line.IsVisible = False
            LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
            .AxisChange()
            .Refresh()
        End With

        With zgSampleDOS
            .GraphPane.CurveList.Clear()
            .GraphPane.Title.Text = "Sample Density of States & Fermi Distribution Preview"
            .GraphPane.XAxis.Title.Text = "Energy (meV)"
            .GraphPane.YAxis.Title.Text = "DOS (N0)"
            ' Simulated Sample
            Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", d_X_SampleDOS, d_Y_SampleDOS, Color.Black, ZedGraph.SymbolType.Circle)
            LI.Line.IsVisible = False
            LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
            ' Real Data
            LI = .GraphPane.AddCurve("", d_X_SampleDOS, d_Y_FermiFunctionSample, Color.Blue, ZedGraph.SymbolType.Circle)
            LI.Line.IsVisible = False
            LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
            .AxisChange()
            .Refresh()
        End With
    End Sub

    ''' <summary>
    ''' Paints the TIP-DOS using the current parameter-set into the Preview-area.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PaintSimulatedTipDOS()
        ReDim d_X_TipDOS(i_N_Preview_Tip)
        ReDim d_Y_TipDOS(i_N_Preview_Tip)
        ReDim d_Y_FermiFunctionTip(i_N_Preview_Tip)

        f.minE = _dMinE
        f.maxE = _dMaxE

        ' Initialize first Precalc-DOS:
        f.PrecalcBCSDOS(_Delta_Tip, _Delta_Sample1, _Delta_Sample2, _BroadeningWidth, _BroadeningType, _ImaginaryDamping)

        For n As Integer = 0 To i_N_Preview_Tip - 1 Step 1
            d_X_TipDOS(n) = _dMinE + (_dMaxE - _dMinE) / i_N_Preview_Tip * n
            d_Y_TipDOS(n) = f.BCSFuncPrecalc(d_X_TipDOS(n), 1, cFitBCSDoubleGap.PrecalcDOSType.Tip)
            d_Y_FermiFunctionTip(n) = f.FermiF_eV(d_X_TipDOS(n), _T_tip)
        Next

        With zgTipDOS
            .GraphPane.CurveList.Clear()
            .GraphPane.Title.Text = "Tip Density of States & Fermi Distribution Preview"
            .GraphPane.XAxis.Title.Text = "Energy (meV)"
            .GraphPane.YAxis.Title.Text = "Tip DOS (N0)"
            Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", d_X_TipDOS, d_Y_TipDOS, Color.Black, ZedGraph.SymbolType.Circle)
            LI.Line.IsVisible = False
            LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
            LI = .GraphPane.AddCurve("", d_X_TipDOS, d_Y_FermiFunctionTip, Color.Blue, ZedGraph.SymbolType.Circle)
            LI.Line.IsVisible = False
            LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
            .AxisChange()
            .Refresh()
        End With
    End Sub

#Region "Fit-Button action, and interface state"
    ''' <summary>
    ''' Starts the Fit-Procedure, if the Worker is not Running.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub StartFitting_Click(sender As System.Object, e As System.EventArgs) Handles btnStartFitting.Click
        If Not BW.IsBusy Then
            ' Start Fit:
            Me.SetButtonsToFitModus(True)
            BW.RunWorkerAsync()
        Else
            ' Abort:
            BW.CancelAsync()
            'Me.SetButtonsToFitModus(False)
        End If
    End Sub

    ''' <summary>
    ''' Enables/disables the Start-Fetch buttons and set's the icons.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetButtonsToFitModus(ByVal IsFitRunning As Boolean)
        If IsFitRunning Then
            Me.btnStartFitting.Text = My.Resources.Fitting_CancelFit
            Me.btnStartFitting.Image = My.Resources.cancel_16
            Me.gbParameters.Enabled = False
            Me.gbTipParameters.Enabled = False
            Me.gbSaveFit.Enabled = False
            Me.gbSourceDataSelector.Enabled = False
            Me.gbSettings.Enabled = False
        Else
            Me.btnStartFitting.Text = My.Resources.Fitting_StartFit
            Me.btnStartFitting.Image = My.Resources.reload_16
            Me.gbParameters.Enabled = True
            Me.gbTipParameters.Enabled = True
            Me.gbSaveFit.Enabled = True
            Me.gbSourceDataSelector.Enabled = True
            Me.gbSettings.Enabled = True
        End If
    End Sub
#End Region

#Region "Fit Procedure"
    ''' <summary>
    ''' Defines the Fit-Function for the Fitting-Procedure
    ''' </summary>
    Public Sub function_I(c As Double(), x As Double(), ByRef func As Double, obj As Object)
        ' Check, if the user requested a cancellation!
        If BW.CancellationPending Then
            Throw New CancellationPendingException
        End If

        ' Check, if BCSDOS needs to be recalculated - speeds up everything
        f.PrecalcBCSDOS(_Delta_Tip, c(0), c(1), c(2), _BroadeningType, c(7))

        ' Calculate the function value.
        func = f.FitFunction(x(0), _Delta_Tip, c(0), c(1), _T_tip, _T_sample, c(2), _BroadeningType, c(7), c(3), c(4), c(5), c(6), _FitDataType, _dIdVDerivationStepWidth)
    End Sub

    ''' <summary>
    ''' Report-Function for getting the status of the Fit.
    ''' </summary>
    Private Sub ReportFunction(ByVal arg As Double(), func As Double, obj As Object)
        Dim sb As New System.Text.StringBuilder
        sb.Append("|")
        For i As Integer = 0 To arg.Length - 1 Step 1
            sb.Append("| ")
            sb.Append(arg(i).ToString("N5"))
            If i < arg.Length - 1 Then
                sb.Append(" ")
            Else
                sb.Append(" ||")
            End If
        Next
        sb.Append(" >> ")
        sb.Append(func.ToString("N8"))
        sb.Append(" << ")

        HandleFitEcho(sb.ToString)
    End Sub

    ''' <summary>
    ''' Starts the Fit-Procedure
    ''' </summary>
    Private Sub StartFitting(sender As Object, e As DoWorkEventArgs)
        f.maxE = dMaxE
        f.minE = dMinE

        Dim StartTime As Date = Now

        ' Initial Echo
        HandleFitEcho("Fit procedure started")
        HandleFitEcho("------------------")
        HandleFitEcho("Initial Set of Parameters: ")
        HandleFitEcho("Fit Data Type:    " & FitDataType.ToString)
        HandleFitEcho("Temperature:      " & T_sample.ToString(TextBoxFormat))
        HandleFitEcho("Delta Tip:        " & Delta_Tip.ToString(TextBoxFormat))
        HandleFitEcho("Delta Sample 1:   " & Delta_Sample1.ToString(TextBoxFormat))
        HandleFitEcho("Delta Sample 2:   " & Delta_Sample2.ToString(TextBoxFormat))
        HandleFitEcho("Broadening Width: " & BroadeningWidth.ToString(TextBoxFormat))
        HandleFitEcho("Broadening Type:  " & BroadeningType.ToString)
        HandleFitEcho("ImaginaryDamping: " & ImaginaryDamping.ToString(TextBoxFormat))
        HandleFitEcho("Delta1 / Delta2:  " & RatioSample1ToSample2.ToString(TextBoxFormat))
        HandleFitEcho("Global X Offset:  " & GlobalXOffset.ToString(TextBoxFormat))
        HandleFitEcho("Global Y Offset:  " & GlobalYOffset.ToString(TextBoxFormat))
        HandleFitEcho("Global Y Stretch: " & GlobalYStretch.ToString(TextBoxFormat))
        HandleFitEcho("------------------")
        HandleFitEcho("Fit running ... good luck!")

        Dim x(,) As Double = d_X2_FitData
        Dim y() As Double = d_Y_FitData
        ' Parameters
        Dim c() As Double = New Double() {Delta_Sample1,
                                          Delta_Sample2,
                                          BroadeningWidth,
                                          RatioSample1ToSample2,
                                          GlobalXOffset,
                                          GlobalYOffset,
                                          GlobalYStretch,
                                          ImaginaryDamping}
        Dim epsf As Double = 0
        ' Smallest step size -> accuracy of the result.
        Dim epsx As Double = 0.000001
        Dim maxits As Integer = 10000
        Dim info As Integer
        Dim state As lsfitstate = New alglib.lsfitstate()
        Dim rep As lsfitreport = New alglib.lsfitreport()
        Dim diffstep As Double = 0.0001

        ' Initialize first DOS-Set
        f.PrecalcBCSDOS(Delta_Tip, c(0), c(1), c(2), Me.BroadeningType, c(7))

        ' Start Fitting:
        ' set up a Try-Environment to be able to abort the Fit-Procedure by throwing an
        ' exception in the fit-function-function, that is pemanently called.
        ' -> Only way to abort the Fit.
        Try
            alglib.lsfitcreatef(x, y, c, diffstep, state)
            'alglib.lsfitsetbc(state, bndl, bndu)
            alglib.lsfitsetcond(state, epsf, epsx, maxits)
            alglib.lsfitsetxrep(state, True)
            alglib.lsfitfit(state, AddressOf function_I, AddressOf ReportFunction, e)
            alglib.lsfitresults(state, info, c, rep)

            ' Echo the final set of Parameters
            ' and fit results, depending on the final Fit-Report.
            HandleFitEcho("Fit finished: ")
            Select Case info
                Case -7
                    HandleFitEcho("Fit-Stop-Reason: gradient verification failed. See LSFitSetGradientCheck() for more information.")
                Case 1
                    HandleFitEcho("Fit-Stop-Reason: relative function improvement is no more than EpsF: " & epsf)
                Case 2
                    HandleFitEcho("Fit-Stop-Reason: relative step is no more than EpsX: " & epsx)
                Case 4
                    HandleFitEcho("Fit-Stop-Reason: gradient norm is no more than EpsG.")
                Case 5
                    HandleFitEcho("Fit-Stop-Reason: MaxIts steps was taken: " & maxits.ToString("N0"))
                Case 7
                    HandleFitEcho("Fit-Stop-Reason: stopping conditions are too stringent. Further improvement is impossible.")
            End Select
            HandleFitEcho("Interation Count: " & rep.iterationscount.ToString("N0"))
            HandleFitEcho("Average Error: " & rep.avgerror.ToString(TextBoxFormat))
            HandleFitEcho("RMS Error: " & rep.rmserror.ToString(TextBoxFormat))
            HandleFitEcho("Max Error: " & rep.maxerror.ToString(TextBoxFormat))
            HandleFitEcho("------------------")
            HandleFitEcho("Final Set of Parameters: ")
            HandleFitEcho("Fit Data Type:    " & FitDataType.ToString)
            HandleFitEcho("Temperature:      " & T_sample.ToString(TextBoxFormat))
            HandleFitEcho("Delta Tip:        " & Delta_Tip.ToString(TextBoxFormat))
            HandleFitEcho("Delta Sample 1:   " & c(0).ToString(TextBoxFormat))
            HandleFitEcho("Delta Sample 2:   " & c(1).ToString(TextBoxFormat))
            HandleFitEcho("Broadening Width: " & c(2).ToString(TextBoxFormat))
            HandleFitEcho("Broadening Type:  " & BroadeningType.ToString)
            HandleFitEcho("ImaginaryDamping: " & c(7).ToString(TextBoxFormat))
            HandleFitEcho("Delta1 / Delta2:  " & c(3).ToString(TextBoxFormat))
            HandleFitEcho("Global X Offset:  " & c(4).ToString(TextBoxFormat))
            HandleFitEcho("Global Y Offset:  " & c(5).ToString(TextBoxFormat))
            HandleFitEcho("Global Y Stretch: " & c(6).ToString(TextBoxFormat))
            HandleFitEcho("------------------")
            HandleFitEcho("Fit procedure ended after " & (Now - StartTime).TotalMinutes.ToString("N0") & " minutes!")

            ' Initialize final DOS-Set
            f.PrecalcBCSDOS(Delta_Tip, c(0), c(1), c(2), Me.BroadeningType, c(7))

            ' Now use the fitted set of parameters to create the output
            ' Data, that is shown afterwards in the Fit-Result-Box.
            d_Output_X = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnIndex).Values.ToArray
            ReDim d_Output_Y_FitData(d_Output_X.Length - 1)
            ReDim d_Output_Y_SampleDOS(d_Output_X.Length - 1)
            ReDim d_Output_Y_TipDOS(d_Output_X.Length - 1)
            ReDim d_Output_Y_FermiFunction(d_Output_X.Length - 1)
            Dim xmeV As Double
            For n As Integer = 0 To d_Output_X.Length - 1 Step 1
                ' Save current X in meV, since our fit-results were obtained in meV!
                xmeV = d_Output_X(n) * 1000
                d_Output_Y_FitData(n) = f.FitFunction(xmeV, Delta_Tip, c(0), c(1), T_tip, T_sample, c(2), Me.BroadeningType, c(7), c(3), c(4), c(5), c(6), Me.FitDataType, dIdVDerivationStepWidth)
                d_Output_Y_SampleDOS(n) = _RatioSample1ToSample2 * f.BCSFuncPrecalc(xmeV, 1, cFitBCSDoubleGap.PrecalcDOSType.Sample1) + f.BCSFuncPrecalc(xmeV, 1, cFitBCSDoubleGap.PrecalcDOSType.Sample2)
                d_Output_Y_TipDOS(n) = f.BCSFuncPrecalc(xmeV, 1, cFitBCSDoubleGap.PrecalcDOSType.Tip)
                d_Output_Y_FermiFunction(n) = f.FermiF_eV(xmeV, _T_sample)

                ' Convert current fit back from pA to A as base unit
                If YCorrection Then
                    If FitDataType = cFitBCS.FitFunctionType.I Then
                        d_Output_Y_FitData(n) /= 10000000000.0
                    ElseIf FitDataType = cFitBCS.FitFunctionType.dIdV Then
                        d_Output_Y_FitData(n) /= 1000000.0
                    End If
                End If
            Next

            ' Set current fit parameter to the final fitted set of parameters
            _Delta_Sample1 = c(0)
            _Delta_Sample2 = c(1)
            _BroadeningWidth = c(2)
            _RatioSample1ToSample2 = c(3)
            _GlobalXOffset = c(4)
            _GlobalYOffset = c(5)
            _GlobalYStretch = c(6)
            _ImaginaryDamping = c(7)
        Catch ex As alglibexception
            MsgBox("An error occured in the Fit-Procedure: " & ex.Message)
            e.Cancel = True
        Catch ex As CancellationPendingException
            ' This exception was just thrown, to be able to abort the fit-Procedure
            e.Cancel = True
        End Try

    End Sub

    ''' <summary>
    ''' Fit Finished Function
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FitFinished(sender As Object, e As RunWorkerCompletedEventArgs)
        SetButtonsToFitModus(False)

        ' If the Fit-Operation was successfull, show final set of data!
        If Not e.Cancelled Then
            ' Reset the final set of parameters to write the values into the textboxes.
            Me.Delta_Sample1 = _Delta_Sample1
            Me.Delta_Sample2 = _Delta_Sample2
            Me.BroadeningWidth = _BroadeningWidth
            Me.RatioSample1ToSample2 = _RatioSample1ToSample2
            Me.GlobalXOffset = _GlobalXOffset
            Me.GlobalYOffset = _GlobalYOffset
            Me.GlobalYStretch = _GlobalYStretch
            Me.ImaginaryDamping = _ImaginaryDamping

            ' Enable the save-button, since data is now present!
            Me.btnSaveColumns.Enabled = True

            ' Plot the Fit-Result with the Source-Data
            With Me.zgFitResult
                .GraphPane.CurveList.Clear()
                .GraphPane.Title.Text = "Fit with Experimental Data"
                .GraphPane.XAxis.Title.Text = "Energy (eV)"
                .GraphPane.YAxis.Title.Text = Me.FitDataType.ToString
                Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", d_Output_X, d_Output_Y_FitData, Color.Black, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
                ' Real Data
                LI = .GraphPane.AddCurve("", d_Output_X, Me.oSpectroscopyTable.Column(Me.cbY.SelectedColumnIndex).Values.ToArray, Color.Blue, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
                .AxisChange()
                .Refresh()
            End With

            With zgSampleDOS
                .GraphPane.CurveList.Clear()
                .GraphPane.Title.Text = "Sample Density of States & Fermi Distribution"
                .GraphPane.XAxis.Title.Text = "Energy (meV)"
                .GraphPane.YAxis.Title.Text = "DOS (N0)"
                ' Simulated Sample
                Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", d_Output_X, d_Output_Y_SampleDOS, Color.Black, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
                ' Real Data
                LI = .GraphPane.AddCurve("", d_Output_X, d_Output_Y_FermiFunction, Color.Blue, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
                .AxisChange()
                .Refresh()
            End With

            With zgTipDOS
                .GraphPane.CurveList.Clear()
                .GraphPane.Title.Text = "Tip Density of States & Fermi Distribution"
                .GraphPane.XAxis.Title.Text = "Energy (meV)"
                .GraphPane.YAxis.Title.Text = "DOS (N0)"
                ' Simulated Sample
                Dim LI As ZedGraph.LineItem = .GraphPane.AddCurve("", d_Output_X, d_Output_Y_TipDOS, Color.Black, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Black)
                ' Real Data
                LI = .GraphPane.AddCurve("", d_Output_X, d_Output_Y_FermiFunction, Color.Blue, ZedGraph.SymbolType.Circle)
                LI.Line.IsVisible = False
                LI.Symbol.Fill = New ZedGraph.Fill(Brushes.Blue)
                .AxisChange()
                .Refresh()
            End With
        End If
    End Sub
#End Region

#Region "Fit-Echo"
    ''' <summary>
    ''' Fit-Echo function
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Private Delegate Sub _HandleFitEcho(ByRef message As String)
    Private Sub HandleFitEcho(ByRef Message As String)
        If Me.FitEcho.InvokeRequired Then
            Dim _delegate As New _HandleFitEcho(AddressOf HandleFitEcho)
            Invoke(_delegate, Message)
        Else
            Me.FitEcho.Text &= Now.ToString("HH:mm:ss") & ": " & Message & vbCrLf
            FitEcho.SelectionStart = FitEcho.Text.Length
            FitEcho.ScrollToCaret()
        End If
    End Sub
#End Region

#Region "Input Check and Value Definition"
    ''' <summary>
    ''' Sets the values of the variables from the textbox,
    ''' if a valid value was entered into one.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub NumericTextbox_ValidValueChanged(ByRef TB As NumericTextbox, ByVal Value As Decimal) _
        Handles txtSampleGap1.ValidValueChanged, txtSampleGap2.ValidValueChanged, txtGapRatio2vs1.ValidValueChanged,
                txtXOffset.ValidValueChanged, txtYOffset.ValidValueChanged, txtYStretch.ValidValueChanged,
                txtFitRangeMax.ValidValueChanged, txtFitRangeMin.ValidValueChanged, txtBroadeningWidth.ValidValueChanged,
                txtTemperature.ValidValueChanged, txtTipGap.ValidValueChanged, txtImaginaryDamping.ValidValueChanged
        Select Case TB.Name
            Case txtSampleGap1.Name
                Delta_Sample1 = Value
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtSampleGap2.Name
                Delta_Sample2 = Value
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtGapRatio2vs1.Name
                RatioSample1ToSample2 = Value
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtXOffset.Name
                GlobalXOffset = Value
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtYOffset.Name
                GlobalYOffset = Value
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtYStretch.Name
                GlobalYStretch = Value
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtFitRangeMax.Name
                dMaxE = Value
                SetFitRangeAndFitData()
            Case txtFitRangeMin.Name
                dMinE = Value
                SetFitRangeAndFitData()
            Case txtBroadeningWidth.Name
                BroadeningWidth = Value
                PaintSimulatedTipDOS()
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtImaginaryDamping.Name
                ImaginaryDamping = Value
                PaintSimulatedTipDOS()
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtTemperature.Name
                T_sample = Value
                T_tip = Value
                PaintSimulatedTipDOS()
                PaintSimulatedSampleDOSAndFitFunction()
            Case txtTipGap.Name
                Delta_Tip = Value
                PaintSimulatedTipDOS()
                PaintSimulatedSampleDOSAndFitFunction()
        End Select
    End Sub

    ''' <summary>
    ''' Combobox for Broadening Type changed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboBroadeningType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboBroadeningType.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.BroadeningType = DirectCast(Me.cboBroadeningType.SelectedItem, KeyValuePair(Of String, cFitBCS.Broadening)).Value
        PaintSimulatedTipDOS()
        PaintSimulatedSampleDOSAndFitFunction()
    End Sub

    ''' <summary>
    ''' Combobox for Fit-Data-Type changed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboFitDataType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboFitDataType.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.FitDataType = DirectCast(Me.cboFitDataType.SelectedItem, KeyValuePair(Of String, cFitBCSDoubleGap.FitFunctionType)).Value
        SetFitRangeAndFitData()
    End Sub

    ''' <summary>
    ''' Set the number of Preview-Points by the numeric Up-Down.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSetPreviewPointNumber_Click(sender As System.Object, e As System.EventArgs) Handles btnSetPreviewPointNumber.Click
        If Not Me.bReady Then Return
        Me.i_N_Preview = Convert.ToInt32(Me.nudPreviewPoints.Value)
        PaintSimulatedTipDOS()
        PaintSimulatedSampleDOSAndFitFunction()
    End Sub

    ''' <summary>
    ''' Turn on Y correction of current and dIdV
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ckbYCorrection_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ckbYCorrection.CheckedChanged
        If Not Me.bReady Then Return
        Me.YCorrection = Me.ckbYCorrection.Checked
        PaintSimulatedTipDOS()
        PaintSimulatedSampleDOSAndFitFunction()
    End Sub

    ''' <summary>
    ''' Starts to create the final data, that is afterwards
    ''' saved to the SpectrosopyTable. For this purpose,
    ''' it has to have the same number of points.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSaveColumns_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumns.Click
        ' Take all the Output-Data and write them to the SpectroscopyTable,
        ' if the SpectroscopyTable-Columns have the same number of Points!
        Dim iDataCount As Integer = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnIndex).Values.Count
        If d_Output_X.Length <> iDataCount Or
            d_Output_Y_FermiFunction.Length <> iDataCount Or
            d_Output_Y_FitData.Length <> iDataCount Or
            d_Output_Y_SampleDOS.Length <> iDataCount Or
            d_Output_Y_TipDOS.Length <> iDataCount Then Return

        ' Create new DataColumns
        Dim FitData As New cSpectroscopyTable.DataColumn
        FitData.Values.AddRange(d_Output_Y_FitData)
        FitData.Name = Me.txtColNameFitResult.Text
        If FitDataType = cFitBCS.FitFunctionType.I Then
            FitData.UnitType = cUnits.UnitType.Current
            FitData.UnitSymbol = cUnits.GetUnitSymbolFromType(FitData.UnitType)
        Else
            FitData.UnitType = cUnits.UnitType.Conductance
            FitData.UnitSymbol = cUnits.GetUnitSymbolFromType(FitData.UnitType)
        End If
        Dim SampleDOS As New cSpectroscopyTable.DataColumn
        SampleDOS.Values.AddRange(d_Output_Y_SampleDOS)
        SampleDOS.Name = Me.txtColNameSampleDOS.Text
        SampleDOS.UnitSymbol = "N0"
        Dim TipDOS As New cSpectroscopyTable.DataColumn
        TipDOS.Values.AddRange(d_Output_Y_TipDOS)
        TipDOS.Name = Me.txtColNameTipDOS.Text
        TipDOS.UnitSymbol = "N0"
        Dim FermiFunction As New cSpectroscopyTable.DataColumn
        FermiFunction.Values.AddRange(d_Output_Y_FermiFunction)
        FermiFunction.Name = Me.txtColNameFermiFunction.Text
        FermiFunction.UnitSymbol = "1"
        FermiFunction.UnitType = cUnits.UnitType.Unitary

        ' Save Fit-Result to the FileObject.
        With Me.oSpectroscopyTable.BaseFileObject
            .SpectroscopyColumnsAdded.AddRange({FitData, SampleDOS, TipDOS, FermiFunction})
        End With
    End Sub
#End Region

#Region "Window Closing"
    ''' <summary>
    ''' Function that runs when the form is closed... saves Settings.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FormIsClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Cancel Closing, if Fit is running!
        If Me.BW.IsBusy Then
            MessageBox.Show(My.Resources.Fitting_FitRunningCloseWindowNotPossible,
                            My.Resources.Fitting_FitRunning, MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Cancel = True
        End If

        ' Saves the Chosen Parameters to the Settings.
        With My.Settings
            .LastBCSDoubleGapFit_YCorrectionActive = Me.YCorrection
            .LastBCSDoubleGapFit_Delta_Tip = Me.Delta_Tip
            .LastBCSDoubleGapFit_BroadeningWidth = Me.BroadeningWidth
            .LastBCSDoubleGapFit_ImaginaryDampingFactor = Me.ImaginaryDamping
            .LastBCSDoubleGapFit_Delta_Sample1 = Me.Delta_Sample1
            .LastBCSDoubleGapFit_Delta_Sample2 = Me.Delta_Sample2
            .LastBCSDoubleGapFit_RatioSample1ToSample2 = Me.RatioSample1ToSample2
            .LastBCSDoubleGapFit_GlobalXOffset = Me.GlobalXOffset
            .LastBCSDoubleGapFit_GlobalYOffset = Me.GlobalYOffset
            .LastBCSDoubleGapFit_GlobalYStretch = Me.GlobalYStretch
            .LastBCSDoubleGapFit_MaxE = Me.dMaxE
            .LastBCSDoubleGapFit_MinE = Me.dMinE
            .LastBCSDoubleGapFit_BroadeningType = Convert.ToInt32(Me.BroadeningType)
            .LastBCSDoubleGapFit_FitDataType = Convert.ToInt32(Me.FitDataType)
            .LastBCSDoubleGapFit_LastPreviewPoints = Me.i_N_Preview
            If Me.cbX.SelectedColumnIndex <> -1 Then
                .LastBCSDoubleGapFit_ColumnNameX = Me.cbX.SelectedColumnName
            End If
            If Me.cbY.SelectedColumnIndex <> -1 Then
                .LastBCSDoubleGapFit_ColumnNameY = Me.cbY.SelectedColumnName
            End If
            If Me.txtColNameFitResult.Text.Trim <> "" Then
                .LastBCSDoubleGapFit_SaveColFitResult = Me.txtColNameFitResult.Text.Trim
            End If
            If Me.txtColNameTipDOS.Text.Trim <> "" Then
                .LastBCSDoubleGapFit_SaveColTipDOS = Me.txtColNameTipDOS.Text.Trim
            End If
            If Me.txtColNameSampleDOS.Text.Trim <> "" Then
                .LastBCSDoubleGapFit_SaveColSampleDOS = Me.txtColNameSampleDOS.Text.Trim
            End If
            If Me.txtColNameFermiFunction.Text.Trim <> "" Then
                .LastBCSDoubleGapFit_SaveColFermiFunction = Me.txtColNameFermiFunction.Text.Trim
            End If
            .Save()
        End With
    End Sub
#End Region
End Class