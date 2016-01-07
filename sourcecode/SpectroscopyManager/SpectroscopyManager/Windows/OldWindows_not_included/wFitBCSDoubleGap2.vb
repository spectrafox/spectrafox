Imports System.ComponentModel

''' <summary>
''' All units are given in [meV and pA]
''' </summary>
''' <remarks></remarks>
Public Class wFitBCSDoubleGap2
    Inherits wFormBase
    Implements iSingleSpectroscopyTableLoaded

#Region "Interface Functions"
    Private Delegate Sub _ShowDialog()
    Public Sub SpectroscopyTableLoaded(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded
        Me.SetSpectroscopyTable(SpectroscopyTable)

        If zgSampleDOS.InvokeRequired Then
            zgSampleDOS.Invoke(New _ShowDialog(AddressOf Me.Show))
            'Me.ShowDialog()
        End If
    End Sub
#End Region

    Private TextBoxFormat As Integer = 6

#Region "Objects used to fit, such as Fit-Class and SpectroscopyTable"
    Private bReady As Boolean = False

    ''' <summary>
    ''' SpectroscopyTable with the data to fit.
    ''' </summary>
    Private oSpectroscopyTable As cSpectroscopyTable

    ''' <summary>
    ''' Fit-Class
    ''' </summary>
    Private WithEvents Fit As cLMAFit

    Private f As New cFitFunction_BCSDoubleGap()
#End Region

#Region "Fit-Parameters"
    Private _YCorrection As Boolean = False
    ''' <summary>
    ''' Gap-Width of the Tip.
    ''' </summary>
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
    Private Property BroadeningWidth As Double
        Get
            Return _BroadeningWidth
        End Get
        Set(value As Double)
            _BroadeningWidth = value
            Me.txtBroadeningWidth.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _BroadeningType As cFitFunction_BCSBase.Broadening = cFitFunction_BCSBase.Broadening.ImaginaryDamping
    ''' <summary>
    ''' Type of Broadening applied to the BCS function!
    ''' </summary>
    Private Property BroadeningType As cFitFunction_BCSBase.Broadening
        Get
            Return _BroadeningType
        End Get
        Set(value As cFitFunction_BCSBase.Broadening)
            _BroadeningType = value
            Dim SelectedBroadening As cFitFunction_BCSBase.Broadening = DirectCast(Me.cboBroadeningType.SelectedItem, KeyValuePair(Of String, cFitFunction_BCSBase.Broadening)).Value
            If SelectedBroadening <> value Then
                For i As Integer = 0 To Me.cboBroadeningType.Items.Count - 1 Step 1
                    Dim t As KeyValuePair(Of String, cFitFunction_BCSBase.Broadening) = DirectCast(Me.cboBroadeningType.Items(i), KeyValuePair(Of String, cFitFunction_BCSBase.Broadening))
                    If t.Value = value Then
                        Me.cboBroadeningType.SelectedIndex = i
                    End If
                Next
            End If

            ' Turn on or off imaginary damping, depending on the selected
            ' Broadening method:
            If value <> cFitFunction_BCSBase.Broadening.None Then
                If value <> cFitFunction_BCSBase.Broadening.ImaginaryDamping Then
                    Me.txtBroadeningWidth.Enabled = True
                Else
                    Me.txtBroadeningWidth.Enabled = False
                End If
            Else
                Me.txtBroadeningWidth.Enabled = False
            End If
        End Set
    End Property

    Private _ImaginaryDamping As Double = 0.015
    ''' <summary>
    ''' Broadening width of the broadening applied to the BCS function.
    ''' </summary>
    Private Property ImaginaryDamping As Double
        Get
            Return _ImaginaryDamping
        End Get
        Set(value As Double)
            _ImaginaryDamping = value
            Me.txtImaginaryDamping.SetValue(value, TextBoxFormat)
        End Set
    End Property

    Private _FitDataType As cFitFunction_BCSBase.FitFunctionType = cFitFunction_BCSBase.FitFunctionType.dIdV
    ''' <summary>
    ''' Type of data that should be fitted.
    ''' </summary>
    Private Property FitDataType As cFitFunction_BCSBase.FitFunctionType
        Get
            Return _FitDataType
        End Get
        Set(value As cFitFunction_BCSBase.FitFunctionType)
            _FitDataType = value
            With Me.cboFitDataType
                Dim SelectedFitFunction As cFitFunction_BCSBase.FitFunctionType = DirectCast(.SelectedItem, KeyValuePair(Of String, cFitFunction_BCSBase.FitFunctionType)).Value
                If SelectedFitFunction <> value Then
                    For i As Integer = 0 To .Items.Count - 1 Step 1
                        If DirectCast(.Items(i), KeyValuePair(Of String, cFitFunction_BCSBase.FitFunctionType)).Value = value Then
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
    Private Sub wFitTest_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Fill the Comboboxes
        With Me.cboBroadeningType
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, cFitFunction_BCSBase.Broadening)(My.Resources.rFitFunction_BCSDoubleGap.BCSFitting_Broad_None, cFitFunction_BCSBase.Broadening.None))
            .Items.Add(New KeyValuePair(Of String, cFitFunction_BCSBase.Broadening)(My.Resources.rFitFunction_BCSDoubleGap.BCSFitting_Broad_ImaginaryDamping, cFitFunction_BCSBase.Broadening.ImaginaryDamping))
            .Items.Add(New KeyValuePair(Of String, cFitFunction_BCSBase.Broadening)(My.Resources.rFitFunction_BCSDoubleGap.BCSFitting_Broad_Gauss, cFitFunction_BCSBase.Broadening.Gauss))
            .Items.Add(New KeyValuePair(Of String, cFitFunction_BCSBase.Broadening)(My.Resources.rFitFunction_BCSDoubleGap.BCSFitting_Broad_Lorentz, cFitFunction_BCSBase.Broadening.Lorentz))
            .ValueMember = "Value"
            .DisplayMember = "Key"
            .SelectedIndex = 1
        End With

        With Me.cboFitDataType
            .Items.Clear()
            .Items.Add(New KeyValuePair(Of String, cFitFunction_BCSBase.FitFunctionType)(My.Resources.rFitFunction_BCSDoubleGap.BCSFitting_FitFunctionType_I, cFitFunction_BCSBase.FitFunctionType.I))
            .Items.Add(New KeyValuePair(Of String, cFitFunction_BCSBase.FitFunctionType)(My.Resources.rFitFunction_BCSDoubleGap.BCSFitting_FitFunctionType_dIdV, cFitFunction_BCSBase.FitFunctionType.dIdV))
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
            Me.BroadeningType = DirectCast(.LastBCSDoubleGapFit_BroadeningType, cFitFunction_BCSBase.Broadening)
            Me.FitDataType = DirectCast(.LastBCSDoubleGapFit_FitDataType, cFitFunction_BCSBase.FitFunctionType)
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

#Region "Preview DOS Painting"
    ''' <summary>
    ''' Set the Fit-Data, if the all Columns are selected.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FitColumnsSelected() Handles cbX.SelectedIndexChanged, cbY.SelectedIndexChanged
        If Not Me.bReady Then Return

        If Me.cbX.SelectedColumnName = String.Empty Or
            Me.cbY.SelectedColumnName = String.Empty Then Return

        ' Set Number of Fit-Points
        i_N_Fit = Me.oSpectroscopyTable.MeasurementPoints

        ' Set min and max E, expected: eV -> convert to meV
        Me.dMinE = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnName).GetMinimumValueOfColumn * 1000
        Me.dMaxE = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnName).GetMaximumValueOfColumn * 1000

        Me.gbParameters.Enabled = True
        Me.btnStartFitting.Enabled = True

        ' Set Fit-Range:
        SetFitRangeAndFitData()
    End Sub

    ''' <summary>
    ''' Extracts the Fit-Data from the SpectroscopyTable in the
    ''' specified energy-limits, and saves it in the Fit-Array.
    ''' </summary>
    Private Sub SetFitRangeAndFitData()
        Dim xValues As cSpectroscopyTable.DataColumn = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnName)
        Dim yValues As cSpectroscopyTable.DataColumn = Me.oSpectroscopyTable.Column(Me.cbY.SelectedColumnName)
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

        For n As Integer = 0 To lXIndicesToConsider.Count - 1 Step 1
            ' Fit Data is given in A(I) and eV
            ' Convert it to nA(I) and meV
            d_X_FitData(n) = 1000 * xValues.Values(lXIndicesToConsider(n))
            d_Y_FitData(n) = yValues.Values(lXIndicesToConsider(n))
            If YCorrection Then
                If Me.FitDataType = cFitFunction_BCSBase.FitFunctionType.I Then
                    d_Y_FitData(n) *= 10000000000.0
                ElseIf Me.FitDataType = cFitFunction_BCSBase.FitFunctionType.dIdV Then
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
        If Me.cbX.SelectedColumnName = String.Empty Or
            Me.cbY.SelectedColumnName = String.Empty Then Return

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
            d_Y_SampleDOS(n) = _RatioSample1ToSample2 * f.BCSFuncPrecalc(d_X_SampleDOS(n), 1, cFitFunction_BCSDoubleGap.PrecalcDOSType.Sample1) +
                               f.BCSFuncPrecalc(d_X_SampleDOS(n), 1, cFitFunction_BCSDoubleGap.PrecalcDOSType.Sample2)
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
            d_Y_TipDOS(n) = f.BCSFuncPrecalc(d_X_TipDOS(n), 1, cFitFunction_BCSDoubleGap.PrecalcDOSType.Tip)
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
#End Region

#Region "Fit-Button action, and interface state"
    ''' <summary>
    ''' Starts the Fit-Procedure, if the Worker is not Running.
    ''' </summary>
    Private Sub StartFitting_Click(sender As System.Object, e As System.EventArgs) Handles btnStartFitting.Click
        If Fit Is Nothing Then
            ' Start Fit:
            Me.SetButtonsToFitModus(True)
            StartFitting()
        Else
            ' Abort:
            Fit.AbortAsyncFit()
            Me.btnStartFitting.Enabled = False
            Me.btnStartFitting.Text = "Cancellation pending ..."
            'Me.SetButtonsToFitModus(False)
        End If
    End Sub

    ''' <summary>
    ''' Enables/disables the Start-Fetch buttons and set's the icons.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetButtonsToFitModus(ByVal IsFitRunning As Boolean)
        If IsFitRunning Then
            Me.btnStartFitting.Text = My.Resources.rFitting.Fitting_CancelFit
            Me.btnStartFitting.Image = My.Resources.cancel_16
            Me.gbParameters.Enabled = False
            Me.gbTipParameters.Enabled = False
            Me.gbSaveFit.Enabled = False
            Me.gbSourceDataSelector.Enabled = False
            Me.gbSettings.Enabled = False
        Else
            Me.btnStartFitting.Text = My.Resources.rFitting.Fitting_StartFit
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
    ''' Report-Function for getting the status of the Fit.
    ''' </summary>
    Private Sub ReportFunction(ByVal FitParameters As Dictionary(Of Integer, sFitParameter), Chi2 As Double) Handles Fit.FitStepEcho
        Dim sb As New System.Text.StringBuilder
        sb.Append(sFitParameter.GetShortParameterEcho(FitParameters))
        sb.Append("Calculated Chi2 >> ")
        sb.Append(Chi2.ToString("N8"))
        sb.Append(" << ")

        HandleFitEcho(sb.ToString)
    End Sub

    ''' <summary>
    ''' Starts the Fit-Procedure
    ''' </summary>
    Private Sub StartFitting()
        f.maxE = dMaxE
        f.minE = dMinE

        Dim StartTime As Date = Now

        ' Initial Echo
        HandleFitEcho("Fit procedure started")
        HandleFitEcho("------------------")
        HandleFitEcho("Initial Set of Parameters: ")
        HandleFitEcho("Fit Data Type:    " & FitDataType.ToString)
        HandleFitEcho("Temperature:      " & T_sample.ToString("N" & TextBoxFormat))
        HandleFitEcho("Delta Tip:        " & Delta_Tip.ToString("N" & TextBoxFormat))
        HandleFitEcho("Delta Sample 1:   " & Delta_Sample1.ToString("N" & TextBoxFormat))
        HandleFitEcho("Delta Sample 2:   " & Delta_Sample2.ToString("N" & TextBoxFormat))
        HandleFitEcho("Broadening Width: " & BroadeningWidth.ToString("N" & TextBoxFormat))
        HandleFitEcho("Broadening Type:  " & BroadeningType.ToString)
        HandleFitEcho("ImaginaryDamping: " & ImaginaryDamping.ToString("N" & TextBoxFormat))
        HandleFitEcho("Delta1 / Delta2:  " & RatioSample1ToSample2.ToString("N" & TextBoxFormat))
        HandleFitEcho("Global X Offset:  " & GlobalXOffset.ToString("N" & TextBoxFormat))
        HandleFitEcho("Global Y Offset:  " & GlobalYOffset.ToString("N" & TextBoxFormat))
        HandleFitEcho("Global Y Stretch: " & GlobalYStretch.ToString("N" & TextBoxFormat))
        HandleFitEcho("------------------")
        HandleFitEcho("Fit running ... good luck!")

        Dim DataPoints()() As Double = New Double()() {d_X_FitData, d_Y_FitData}

        ' Parameters
        Dim FitParameters As Dictionary(Of Integer, sFitParameter) = f.FitParameters

        Dim epsf As Double = 0
        ' Smallest step size -> accuracy of the result.
        Dim epsx As Double = 0.000000000001
        Dim diffstep As Double = 0.0001

        f.BroadeningType = Me.BroadeningType
        ' Initialize first DOS-Set
        f.PrecalcBCSDOS(Delta_Tip,
                        FitParameters(0).Value,
                        FitParameters(1).Value,
                        FitParameters(2).Value,
                        Me.BroadeningType,
                        FitParameters(7).Value)

        ' Create Fit-Object
        Me.Fit = New cLMAFit(f,
                             DataPoints,
                             Nothing,
                             epsx,
                             1000,
                             diffstep)

        ' Start Fit async
        Me.Fit.FitAsync()

    End Sub

    Private Delegate Sub _FitFinished(FitStopReason As cLMAFit.FitStopReason,
                                      FinalParameters As Dictionary(Of Integer, sFitParameter),
                                      Chi2 As Double)
    ''' <summary>
    ''' Fit Finished Function
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FitFinished(FitStopReason As Integer,
                            FinalParameters As Dictionary(Of Integer, sFitParameter),
                            Chi2 As Double) Handles Fit.FitFinished
        ' Check for required Invoke
        If Me.InvokeRequired Then
            Dim _delegate As New _FitFinished(AddressOf FitFinished)
            Invoke(_delegate, FitStopReason, FinalParameters, Chi2)
        Else
            ' Echo the final set of Parameters
            ' and fit results, depending on the final Fit-Report.
            HandleFitEcho("Fit finished: ")
            Select Case FitStopReason
                Case cLMAFit.FitStopReason.FitConverged
                    HandleFitEcho("Fit-Stop-Reason: relative function improvement is no more than: " & Me.Fit.MinDeltaChi2)
                Case cLMAFit.FitStopReason.MaxIterationsReached
                    HandleFitEcho("Fit-Stop-Reason: Maximum iterations were taken: " & Me.Fit.MaximumIterations.ToString("N0"))
                Case cLMAFit.FitStopReason.UserAborted
                    HandleFitEcho("Fit-Stop-Reason: The fit procedure was aborted!")
            End Select
            HandleFitEcho("Interation Count: " & Me.Fit.Iterations.ToString("N0"))
            'HandleFitEcho("Average Error: " & .ToString(TextBoxFormat))
            'HandleFitEcho("RMS Error: " & .ToString(TextBoxFormat))
            'HandleFitEcho("Max Error: " & .ToString(TextBoxFormat))
            HandleFitEcho("------------------")
            HandleFitEcho("Final Set of Parameters: ")
            HandleFitEcho("Fit Data Type:    " & FitDataType.ToString)
            HandleFitEcho("Temperature:      " & T_sample.ToString("N" & TextBoxFormat))
            HandleFitEcho("Delta Tip:        " & Delta_Tip.ToString("N" & TextBoxFormat))
            HandleFitEcho("Delta Sample 1:   " & FinalParameters(0).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("Delta Sample 2:   " & FinalParameters(1).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("Broadening Width: " & FinalParameters(2).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("Broadening Type:  " & BroadeningType.ToString)
            HandleFitEcho("ImaginaryDamping: " & FinalParameters(7).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("Delta1 / Delta2:  " & FinalParameters(3).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("Global X Offset:  " & FinalParameters(4).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("Global Y Offset:  " & FinalParameters(5).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("Global Y Stretch: " & FinalParameters(6).Value.ToString("N" & TextBoxFormat))
            HandleFitEcho("------------------")
            HandleFitEcho("Fit procedure ended after " & Me.Fit.FitDuration.TotalMinutes.ToString("N0") & " minutes!")

            ' Initialize final DOS-Set
            f.PrecalcBCSDOS(Delta_Tip,
                            FinalParameters(0).Value,
                            FinalParameters(1).Value,
                            FinalParameters(2).Value,
                            Me.BroadeningType,
                            FinalParameters(7).Value)

            ' Now use the fitted set of parameters to create the output
            ' Data, that is shown afterwards in the Fit-Result-Box.
            d_Output_X = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnName).Values.ToArray
            ReDim d_Output_Y_FitData(d_Output_X.Length - 1)
            ReDim d_Output_Y_SampleDOS(d_Output_X.Length - 1)
            ReDim d_Output_Y_TipDOS(d_Output_X.Length - 1)
            ReDim d_Output_Y_FermiFunction(d_Output_X.Length - 1)
            Dim xmeV As Double
            For n As Integer = 0 To d_Output_X.Length - 1 Step 1
                ' Save current X in meV, since our fit-results were obtained in meV!
                xmeV = d_Output_X(n) * 1000
                d_Output_Y_FitData(n) = f.FitFunction(xmeV,
                                                      Delta_Tip,
                                                      FinalParameters(0).Value,
                                                      FinalParameters(1).Value,
                                                      T_tip,
                                                      T_sample,
                                                      FinalParameters(2).Value,
                                                      Me.BroadeningType,
                                                      FinalParameters(7).Value,
                                                      FinalParameters(3).Value,
                                                      FinalParameters(4).Value,
                                                      FinalParameters(5).Value,
                                                      FinalParameters(6).Value,
                                                      Me.FitDataType,
                                                      dIdVDerivationStepWidth)
                d_Output_Y_SampleDOS(n) = _RatioSample1ToSample2 * f.BCSFuncPrecalc(xmeV, 1, cFitFunction_BCSDoubleGap.PrecalcDOSType.Sample1) + f.BCSFuncPrecalc(xmeV, 1, cFitFunction_BCSDoubleGap.PrecalcDOSType.Sample2)
                d_Output_Y_TipDOS(n) = f.BCSFuncPrecalc(xmeV, 1, cFitFunction_BCSDoubleGap.PrecalcDOSType.Tip)
                d_Output_Y_FermiFunction(n) = f.FermiF_eV(xmeV, _T_sample)

                ' Convert current fit back from pA to A as base unit
                If YCorrection Then
                    If FitDataType = cFitFunction_BCSBase.FitFunctionType.I Then
                        d_Output_Y_FitData(n) /= 10000000000.0
                    ElseIf FitDataType = cFitFunction_BCSBase.FitFunctionType.dIdV Then
                        d_Output_Y_FitData(n) /= 1000000.0
                    End If
                End If
            Next

            ' Set current fit parameter to the final fitted set of parameters
            _Delta_Sample1 = FinalParameters(0).Value
            _Delta_Sample2 = FinalParameters(1).Value
            _BroadeningWidth = FinalParameters(2).Value
            _RatioSample1ToSample2 = FinalParameters(3).Value
            _GlobalXOffset = FinalParameters(4).Value
            _GlobalYOffset = FinalParameters(5).Value
            _GlobalYStretch = FinalParameters(6).Value
            _ImaginaryDamping = FinalParameters(7).Value

            SetButtonsToFitModus(False)

            ' If the Fit-Operation was successfull, show final set of data!
            If FitStopReason <> cLMAFit.FitStopReason.UserAborted Then
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
                    LI = .GraphPane.AddCurve("", d_Output_X, Me.oSpectroscopyTable.Column(Me.cbY.SelectedColumnName).Values.ToArray, Color.Blue, ZedGraph.SymbolType.Circle)
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

            Me.Fit = Nothing
        End If
    End Sub
#End Region

#Region "Fit-Echo"
    ''' <summary>
    ''' Fit-Echo function
    ''' </summary>
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

    ''' <summary>
    ''' Handle Progress Event
    ''' </summary>
    Private Delegate Sub _HandleProgress(ByVal Item As Integer, ByVal Max As Integer, ByVal Message As String)
    Private Sub HandleProgress(ByVal Item As Integer, ByVal Max As Integer, ByVal Message As String) Handles Fit.CalculationStepProgress
        If Me.InvokeRequired Then
            Dim _delegate As New _HandleProgress(AddressOf HandleProgress)
            Invoke(_delegate, Item, Max, Message)
        Else
            Dim Percent As Double = (Item / Max) * 100
            If Percent > 100 Then Percent = 100
            If Percent < 0 Then Percent = 0

            Me.lblProgress.Text = Message & " " & Percent.ToString("N2") & "% (" & Item & "|" & Max & ")"
            Me.pgbProgress.Value = CInt(Percent)
        End If
    End Sub
#End Region

#Region "Input Check and Value Definition"
    ''' <summary>
    ''' Sets the values of the variables from the textbox,
    ''' if a valid value was entered into one.
    ''' </summary>
    Private Sub NumericTextbox_ValidValueChanged(ByRef TB As NumericTextbox) _
        Handles txtSampleGap1.ValidValueChanged, txtSampleGap2.ValidValueChanged, txtGapRatio2vs1.ValidValueChanged,
                txtXOffset.ValidValueChanged, txtYOffset.ValidValueChanged, txtYStretch.ValidValueChanged,
                txtFitRangeMax.ValidValueChanged, txtFitRangeMin.ValidValueChanged, txtBroadeningWidth.ValidValueChanged,
                txtTemperature.ValidValueChanged, txtTipGap.ValidValueChanged, txtImaginaryDamping.ValidValueChanged
        Dim Value As Double = TB.DecimalValue
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
    Private Sub cboBroadeningType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboBroadeningType.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.BroadeningType = DirectCast(Me.cboBroadeningType.SelectedItem, KeyValuePair(Of String, cFitFunction_BCSBase.Broadening)).Value
        PaintSimulatedTipDOS()
        PaintSimulatedSampleDOSAndFitFunction()
    End Sub

    ''' <summary>
    ''' Combobox for Fit-Data-Type changed.
    ''' </summary>
    Private Sub cboFitDataType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboFitDataType.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.FitDataType = DirectCast(Me.cboFitDataType.SelectedItem, KeyValuePair(Of String, cFitFunction_BCSBase.FitFunctionType)).Value
        SetFitRangeAndFitData()
    End Sub

    ''' <summary>
    ''' Set the number of Preview-Points by the numeric Up-Down.
    ''' </summary>
    Private Sub btnSetPreviewPointNumber_Click(sender As System.Object, e As System.EventArgs) Handles btnSetPreviewPointNumber.Click
        If Not Me.bReady Then Return
        Me.i_N_Preview = Convert.ToInt32(Me.nudPreviewPoints.Value)
        PaintSimulatedTipDOS()
        PaintSimulatedSampleDOSAndFitFunction()
    End Sub

    ''' <summary>
    ''' Turn on Y correction of current and dIdV
    ''' </summary>
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
    Private Sub btnSaveColumns_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveColumns.Click
        ' Take all the Output-Data and write them to the SpectroscopyTable,
        ' if the SpectroscopyTable-Columns have the same number of Points!
        Dim iDataCount As Integer = Me.oSpectroscopyTable.Column(Me.cbX.SelectedColumnName).Values.Count
        If d_Output_X.Length <> iDataCount Or
            d_Output_Y_FermiFunction.Length <> iDataCount Or
            d_Output_Y_FitData.Length <> iDataCount Or
            d_Output_Y_SampleDOS.Length <> iDataCount Or
            d_Output_Y_TipDOS.Length <> iDataCount Then Return

        ' Create new DataColumns
        Dim FitData As New cSpectroscopyTable.DataColumn
        FitData.Values.AddRange(d_Output_Y_FitData)
        FitData.Name = Me.txtColNameFitResult.Text
        If FitDataType = cFitFunction_BCSBase.FitFunctionType.I Then
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
    Private Sub FormIsClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Cancel Closing, if Fit is running!
        If Not Me.Fit Is Nothing Then
            If Me.Fit.FitIsRunning Then
                MessageBox.Show(My.Resources.rFitting.Fitting_FitRunningCloseWindowNotPossible,
                                My.Resources.rFitting.Fitting_FitRunning, MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Cancel = True
            End If
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
            If Me.cbX.SelectedColumnName <> String.Empty Then
                .LastBCSDoubleGapFit_ColumnNameX = Me.cbX.SelectedColumnName
            End If
            If Me.cbY.SelectedColumnName <> String.Empty Then
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