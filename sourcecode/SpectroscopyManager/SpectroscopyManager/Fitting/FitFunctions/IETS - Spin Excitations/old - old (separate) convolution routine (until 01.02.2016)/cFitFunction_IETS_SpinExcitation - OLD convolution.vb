Imports MathNet.Numerics.LinearAlgebra
Imports MathNet.Numerics.LinearAlgebra.Complex
Imports System.Threading.Tasks
Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator
Imports SpectroscopyManager.cNumericFunctions

#Const USEPARALLEL = 1

Public Class cFitFunction_IETS_SpinExcitation
    Inherits cFitFunction

#Region "initialize constants used"

    ''' <summary>
    ''' Cutoff for evaluation of exponential function:
    ''' </summary>
    Public Const MAX_EXP As Double = 100D

    ''' <summary>
    ''' Cutoff for evaluation of exponential function.
    ''' </summary>
    Public Const MIN_EXP As Double = -100D

    ''' <summary>
    ''' Cutoff for integrals, if the value becomes irrelevant small.
    ''' </summary>
    Public Const MIN_DIFFERENTIAL As Double = 0.001

    ''' <summary>
    ''' Number of points to consider for the spline-interpolation
    ''' </summary>
    Public Const PointsToTakeForSplineInterpolation As Integer = 10

#End Region

#Region "Fit range plausibility check"

    ''' <summary>
    ''' Check the fit-range to be not too large (<= 10mV).
    ''' Otherwise the default inititalization will need too much time.
    ''' </summary>
    Public Overrides Function FitFunctionSuggestsDifferentFitRange(ByRef FitRangeLower As Double, ByRef FitRangeUpper As Double) As Boolean
        If FitRangeLower < -0.02 Or FitRangeUpper > 0.02 Then
            FitRangeLower = -0.02
            FitRangeUpper = 0.02
            Return False
        Else
            Return True
        End If
    End Function

#End Region

#Region "Initialize Calculation Parameters"

    ''' <summary>
    ''' Energy sampling size in [eV]
    ''' Used for calculation of the dIdV integral.
    ''' This is the step size between which the dIdV is summed up.
    ''' (irrational for minizing numerical artifacts)
    ''' </summary>
    Public Property IntegrationEnergyStep As Double = 0.00020342123934545349

    ''' <summary>
    ''' dIdV-precalculation interpolation-interval-width:
    ''' is responsible for the number of points that is need to be calculated,
    ''' and therefore the speed of the calculation, and the accuracy.
    ''' </summary>
    Public dEInterpolation_CurrentPrecalculation As Double = 0.00008

    ''' <summary>
    ''' Largest relevant energy value to be considered for integration in the current integral.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property ConvolutionIntegralE_POS As Double = 0.014

    ''' <summary>
    ''' Smallest relevant energy value to be considered for integration in the current integral.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property ConvolutionIntegralE_NEG As Double = -0.014

    ''' <summary>
    ''' Calculate and cache all current values up to this bias.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property CalculateForBiasRangeUpperE As Double = 0.014

    ''' <summary>
    ''' Calculate and cache all current values from this bias.
    ''' Should be set in advance, to minimize the calculation time.
    ''' </summary>
    Public Property CalculateForBiasRangeLowerE As Double = -0.014

    ''' <summary>
    ''' Change the bias range of the calculated current.
    ''' </summary>
    Public Overrides Sub ChangeFitRangeX(LowerValue As Double, HigherValue As Double)
        CalculateForBiasRangeUpperE = HigherValue
        CalculateForBiasRangeLowerE = LowerValue
        ConvolutionIntegralE_POS = HigherValue
        ConvolutionIntegralE_NEG = LowerValue
        MyBase.ChangeFitRangeX(LowerValue, HigherValue)

        Me.ForceNewPrecalculation()
    End Sub

    ''' <summary>
    ''' Forces the precalculation of the values.
    ''' </summary>
    Public Sub ForceNewPrecalculation()
        Me.Precalculate_Current(Me.FitParametersGrouped, True)
    End Sub

#End Region

#Region "Initialize Fit-Parameters"

    ''' <summary>
    ''' Signal type that should be calculated.
    ''' </summary>
    Public Enum SignalTypes
        I
        dIdV
    End Enum

    Protected _SignalType As SignalTypes = SignalTypes.dIdV
    ''' <summary>
    ''' Signal type the routine calculates.
    ''' If dI/dV it performs a numeric derivation of the data.
    ''' </summary>
    Public Property SignalType As SignalTypes
        Get
            Return Me._SignalType
        End Get
        Set(value As SignalTypes)
            Me._SignalType = value

            ' Update the calculation
            Me.ForceNewPrecalculation()
        End Set
    End Property

    ''' <summary>
    ''' Gets a signal type from an input integer (e.g. from loading settings, where the signaltype is stored as integer).
    ''' </summary>
    Public Shared Function GetSignalTypeFromInteger(ByVal Input As Integer) As SignalTypes
        Select Case Input
            Case SignalTypes.I
                Return SignalTypes.I
            Case SignalTypes.dIdV
                Return SignalTypes.dIdV
            Case Else
                Return SignalTypes.I
        End Select
    End Function

    Protected _SpinInOneHalfs As Integer = 4
    ''' <summary>
    ''' Gives the currently spin which is setup.
    ''' </summary>
    Public Property SpinInOneHalfs As Integer
        Get
            Return Me._SpinInOneHalfs
        End Get
        Set(value As Integer)
            Me._SpinInOneHalfs = value

            ' Update the calculation
            Me.ForceNewPrecalculation()
        End Set
    End Property

    ''' <summary>
    ''' Identification Indizes of the Parameters.
    ''' </summary>
    Public Enum FitParameterIdentifier
        Y0
        Amplitude
        XCenter
        LinearSlope
        D
        E
        Temperature
        BField
        BAngleTheta
        BAnglePhi
        g
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Y0.ToString, 0, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_Y0))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.XCenter.ToString, 0, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_XCenter))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Amplitude.ToString, 1, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_Amplitude))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.LinearSlope.ToString, 0, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_LinearSlope))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.D.ToString, -0.00155, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_D))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.E.ToString, 0.00031, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_E))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.Temperature.ToString, 1.2, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_Temperature))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.BField.ToString, 0, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_BField))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.BAngleTheta.ToString, 90, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_BAngleTheta))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.BAnglePhi.ToString, 0, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_BAnglePhi))
        Me.FitParameters.Add(New cFitParameter(FitParameterIdentifier.g.ToString, 2, False, My.Resources.rFitFunction_IETS_SpinExcitation.Parameter_g))
    End Sub

    ' cache variables for the last used parameters, for which the current is cached
    Private LastParameterValues() As Double

#End Region

#Region "Tip and Sample DOS Definition"

    ''' <summary>
    ''' Default is a flat sample-DOS.
    ''' </summary>
    Public Overridable Function SampleDOS(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return 1
    End Function

    ''' <summary>
    ''' Default is a flat tip-DOS.
    ''' </summary>
    Public Overridable Function TipDOS(ByRef E As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
        Return 1
    End Function

#End Region

#Region "FitFunction"

    ''' <summary>
    ''' Cache storage for the current integral.
    ''' </summary>
    Private CurrentIntegralCache_POS As Double()
    Private CurrentIntegralCache_NEG As Double()

    ' Bias cache properties,
    ' giving the bias range of the current values
    ' stored in the arrays.
    Private dVBiasStart_Pos As Double = 0
    Private dVBiasEnd_Pos As Double = 0
    Private dVBiasStart_Neg As Double = 0
    Private dVBiasEnd_Neg As Double = 0

    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' </summary>
    ''' <param name="x">x-value at which the fit function should be evaluated.</param>
    Public Overrides Function GetY(ByRef x As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

        ' Launch the Precalculation
        Me.Precalculate_Current(InputParameters, False)

        ' Finally return the cached FitFunction
        Return Me.FitFunctionPRECALC(x, InputParameters)

    End Function

    ''' <summary>
    ''' Precalculates the current cache.
    ''' </summary>
    Protected Sub Precalculate_Current(ByRef InputParameters As cFitParameterGroupGroup, ByVal Force As Boolean)

        '##############################################################
        ' save the currently used parameter-set as new last used values
        ' Current
        Dim ParameterValues As Double() = InputParameters.Group(Me.UseFitParameterGroupID).GetParameterValues

        ' Check, if the current cache has to be refreshed.
        If Not Force AndAlso
            cArrayHelper.AreArraysTheSame(ParameterValues, LastParameterValues) AndAlso
            LastCache_SpinInOneHalfs = Me._SpinInOneHalfs Then Return

        ' save the last used parameter-set
        If LastParameterValues Is Nothing Then ReDim LastParameterValues(ParameterValues.Length - 1)
        If LastParameterValues.Length <> ParameterValues.Length Then ReDim LastParameterValues(ParameterValues.Length - 1)
        ParameterValues.CopyTo(LastParameterValues, 0)
        '##############################################################

        '########################################################################
        ' Calculate the crystal field hamiltonian and solve the eigenvalue system
        Me.UpdateCachedCrystalFieldHamiltonian(InputParameters, SpinInOneHalfs)
        '########################################################################

        ' Initialize variables
        Dim iCurrentCachePoints_Pos As Integer = 0
        dVBiasStart_Pos = 0
        dVBiasEnd_Pos = 0
        Dim iCurrentCachePoints_Neg As Integer = 0
        dVBiasStart_Neg = 0
        dVBiasEnd_Neg = 0

        ' catch the case where the energy-range is 0
        If Me.CalculateForBiasRangeUpperE <= Me.CalculateForBiasRangeLowerE Then
            Return
        End If

        ' Get the range for which to calculate the current
        ' additionally consider the broadening-width.
        Dim UpperBiasEnergy As Double = Me.CalculateForBiasRangeUpperE  ' Enlarge integration range by 10%
        Dim LowerBiasEnergy As Double = Me.CalculateForBiasRangeLowerE ' Enlarge integration range by 10%

        'If UpperBiasEnergy > 0 Then
        '    UpperBiasEnergy -= 1.1 * (sFitParameter.GetValueForIdentifier(FitParameterIdentifier.D, Identifiers, Values) + sFitParameter.GetValueForIdentifier(FitParameterIdentifier.E, Identifiers, Values))
        'Else
        '    UpperBiasEnergy += 1.1 * (sFitParameter.GetValueForIdentifier(FitParameterIdentifier.D, Identifiers, Values) + sFitParameter.GetValueForIdentifier(FitParameterIdentifier.E, Identifiers, Values))
        'End If
        'If LowerBiasEnergy > 0 Then
        '    LowerBiasEnergy -= 1.1 * (sFitParameter.GetValueForIdentifier(FitParameterIdentifier.D, Identifiers, Values) + sFitParameter.GetValueForIdentifier(FitParameterIdentifier.E, Identifiers, Values))
        'Else
        '    LowerBiasEnergy += 1.1 * (sFitParameter.GetValueForIdentifier(FitParameterIdentifier.D, Identifiers, Values) + sFitParameter.GetValueForIdentifier(FitParameterIdentifier.E, Identifiers, Values))
        'End If

        ' Not calculate the cache of the current: 
        ' for this: calculate the number of points to cache
        ' and the borders to calculate the current in between
        If UpperBiasEnergy > 0 And LowerBiasEnergy > 0 Then
            ' only positive bias values
            '###########################
            iCurrentCachePoints_Pos = CInt((UpperBiasEnergy - LowerBiasEnergy) / dEInterpolation_CurrentPrecalculation)
            dVBiasEnd_Pos = UpperBiasEnergy
            dVBiasStart_Pos = LowerBiasEnergy
            Me.CurrentIntegralCache_NEG = New Double() {}
        ElseIf UpperBiasEnergy < 0 And LowerBiasEnergy < 0 Then
            ' only negative bias values
            '###########################
            iCurrentCachePoints_Neg = -CInt((LowerBiasEnergy - UpperBiasEnergy) / dEInterpolation_CurrentPrecalculation)
            dVBiasEnd_Neg = LowerBiasEnergy
            dVBiasStart_Neg = UpperBiasEnergy
            Me.CurrentIntegralCache_POS = New Double() {}
        Else
            ' both, positive and negative bias values.
            '##########################################
            iCurrentCachePoints_Pos = Math.Abs(CInt(UpperBiasEnergy / dEInterpolation_CurrentPrecalculation))
            iCurrentCachePoints_Neg = Math.Abs(CInt(LowerBiasEnergy / dEInterpolation_CurrentPrecalculation))
            dVBiasStart_Neg = 0
            dVBiasStart_Pos = 0

            dVBiasEnd_Pos = UpperBiasEnergy
            dVBiasEnd_Neg = LowerBiasEnergy
        End If

        ' Redimension the cache arrays
        If Me.CurrentIntegralCache_POS Is Nothing And iCurrentCachePoints_Pos > 0 Then
            ReDim Me.CurrentIntegralCache_POS(iCurrentCachePoints_Pos - 1)
        ElseIf Me.CurrentIntegralCache_POS.Length <> iCurrentCachePoints_Pos And iCurrentCachePoints_Pos > 0 Then
            ReDim Me.CurrentIntegralCache_POS(iCurrentCachePoints_Pos - 1)
        End If
        If Me.CurrentIntegralCache_NEG Is Nothing And iCurrentCachePoints_Neg > 0 Then
            ReDim Me.CurrentIntegralCache_NEG(iCurrentCachePoints_Neg - 1)
        ElseIf Me.CurrentIntegralCache_NEG.Length <> iCurrentCachePoints_Neg And iCurrentCachePoints_Neg > 0 Then
            ReDim Me.CurrentIntegralCache_NEG(iCurrentCachePoints_Neg - 1)
        End If

        ' Create the XArray to calculate the current.
        Dim CurrentIntegralX_POS As Double() = New Double() {}
        Dim CurrentIntegralX_NEG As Double() = New Double() {}
        If iCurrentCachePoints_Pos > 0 Then
            ReDim CurrentIntegralX_POS(iCurrentCachePoints_Pos - 1)
        End If
        If iCurrentCachePoints_Neg > 0 Then
            ReDim CurrentIntegralX_NEG(iCurrentCachePoints_Neg - 1)
        End If

        '// fill the arrays on the CPU
        ' include the 0 in the positive array
        If iCurrentCachePoints_Pos > 0 Then
            Parallel.For(0, iCurrentCachePoints_Pos, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                   CurrentIntegralX_POS(j) = dVBiasStart_Pos + (dEInterpolation_CurrentPrecalculation * j)
                                                                               End Sub)
        End If
        If iCurrentCachePoints_Neg > 0 Then
            Parallel.For(0, iCurrentCachePoints_Neg, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                   CurrentIntegralX_NEG(j) = dVBiasStart_Neg - (dEInterpolation_CurrentPrecalculation * j)
                                                                               End Sub)
        End If

        ' NON CUDA VERSION,
        ' but using .NET parallel library.
        '##################################
#If USEPARALLEL = 1 Then
        Dim Parameters As cFitParameterGroupGroup = InputParameters
        If iCurrentCachePoints_Pos > 0 Then
            Parallel.For(0, iCurrentCachePoints_Pos, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                   CurrentIntegralCache_POS(j) = FuncI(CurrentIntegralX_POS(j), Parameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, IntegrationEnergyStep)
                                                                               End Sub)
        End If
        If iCurrentCachePoints_Neg > 0 Then
            Parallel.For(0, iCurrentCachePoints_Neg, Me.MultiThreadingOptions, Sub(j As Integer)
                                                                                   CurrentIntegralCache_NEG(j) = FuncI(CurrentIntegralX_NEG(j), Parameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, IntegrationEnergyStep)
                                                                               End Sub)
        End If
#Else
        ' calculate the current
        If iCurrentCachePoints_Pos > 0 Then
            For j As Integer = 0 To iCurrentCachePoints_Pos - 1 Step 1
                CurrentIntegralCache_POS(j) = FuncI(CurrentIntegralX_POS(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, IntegrationEnergyStep)
            Next
        End If
        If iCurrentCachePoints_Neg > 0 Then
            For j As Integer = 0 To iCurrentCachePoints_Neg - 1 Step 1
                CurrentIntegralCache_NEG(j) = FuncI(CurrentIntegralX_NEG(j), Identifiers, Values, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, IntegrationEnergyStep)
            Next
        End If
#End If
    End Sub

#End Region

#Region "Crystal Field Hamiltonian, Spin Matrices and Boltzman-Occupation, and the resulting transition probabilities"

    ''' <summary>
    ''' Define the crystal field Hamiltonian.
    ''' See "Molecular Nanomagnets", D. Gatteschi, R. Sessoli, J. Villain, Oxford University Press 2006
    ''' </summary>
    <Cudafy>
    Public Shared Function CrystalFieldHamiltonian(ByVal B As Double,
                                                   ByVal ThetaAsDegree As Double,
                                                   ByVal PhiAsDegree As Double,
                                                   ByVal D As Double,
                                                   ByVal E As Double,
                                                   ByVal g As Double,
                                                   ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Dim SzSq As Matrix(Of Numerics.Complex) = Sz(SpinInOneHalfs)
        SzSq = SzSq.Multiply(SzSq)
        Dim SplusSq As Matrix(Of Numerics.Complex) = Splus(SpinInOneHalfs)
        SplusSq = SplusSq.Multiply(SplusSq)
        Dim SminusSq As Matrix(Of Numerics.Complex) = Sminus(SpinInOneHalfs)
        SminusSq = SminusSq.Multiply(SminusSq)

        'Dim M1 As DenseMatrix = DirectCast(-g * cConstants.muB_eV * B * STheta(SpinInOneHalfs, ThetaAsDegree) + D * (Sz(SpinInOneHalfs) * Sz(SpinInOneHalfs)), DenseMatrix)
        'Dim M1 As DenseMatrix = DirectCast(-g * cConstants.muB_eV * B * SAngle(SpinInOneHalfs, ThetaAsDegree, PhiAsDegree) + D * (Sz(SpinInOneHalfs) * Sz(SpinInOneHalfs)), DenseMatrix)
        'Dim M2 As DenseMatrix = DirectCast(E * 0.5 * (Splus(SpinInOneHalfs) * Splus(SpinInOneHalfs) + Sminus(SpinInOneHalfs) * Sminus(SpinInOneHalfs)), DenseMatrix)
        Dim R As Matrix(Of Numerics.Complex) = SAngle(SpinInOneHalfs, ThetaAsDegree, PhiAsDegree).Multiply(-g * cConstants.muB_eV * B) + SzSq.Multiply(D) + SminusSq.Add(SplusSq).Multiply(E * 0.5)
        Return DirectCast(R, DenseMatrix)
    End Function

    ''' <summary>
    ''' Spin operator for dm = 0.
    ''' </summary>
    <Cudafy>
    Public Shared Function Sz(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Dim M As DenseMatrix = DenseMatrix.Create((SpinInOneHalfs + 1), (SpinInOneHalfs + 1), Function(i As Integer, j As Integer)
                                                                                                  Dim Si As Double = -(SpinInOneHalfs * 0.5) + i * 1
                                                                                                  Dim Sj As Double = -(SpinInOneHalfs * 0.5) + j * 1
                                                                                                  If Si = Sj Then
                                                                                                      Return Si
                                                                                                  Else
                                                                                                      Return 0
                                                                                                  End If
                                                                                              End Function)
        Return M
    End Function

    ''' <summary>
    ''' Spin operator for dm = +1.
    ''' </summary>
    <Cudafy>
    Public Shared Function Splus(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Dim S As Double = SpinInOneHalfs * 0.5
        'Dim M As DenseMatrix = DenseMatrix.Create((SpinInOneHalfs + 1), (SpinInOneHalfs + 1), Function(i As Integer, j As Integer)
        '                                                                                          Dim Si As Double = -S + i * 1
        '                                                                                          Dim Sj As Double = -S + j * 1
        '                                                                                          If Si - 1 = Sj Then
        '                                                                                              Return Math.Sqrt((S + Si) * (S - Si + 1))
        '                                                                                          Else
        '                                                                                              Return 0
        '                                                                                          End If
        '                                                                                      End Function)
        Dim M As DenseMatrix = DenseMatrix.Create((SpinInOneHalfs + 1), (SpinInOneHalfs + 1), Function(i As Integer, j As Integer)
                                                                                                  Dim Si As Double = -S + i * 1
                                                                                                  Dim Sj As Double = -S + j * 1
                                                                                                  If Si = Sj + 1 Then
                                                                                                      Return Math.Sqrt(S * (S + 1) - Si * Sj)
                                                                                                  Else
                                                                                                      Return 0
                                                                                                  End If
                                                                                              End Function)
        Return M
    End Function

    ''' <summary>
    ''' Spin operator for dm = -1.
    ''' </summary>
    <Cudafy>
    Public Shared Function Sminus(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Dim S As Double = SpinInOneHalfs * 0.5
        'Dim M As DenseMatrix = DenseMatrix.Create((SpinInOneHalfs + 1), (SpinInOneHalfs + 1), Function(i As Integer, j As Integer)
        '                                                                                          Dim Si As Double = -S + i * 1
        '                                                                                          Dim Sj As Double = -S + j * 1
        '                                                                                          If Si + 1 = Sj Then
        '                                                                                              Return Math.Sqrt((S + Sj) * (S - Sj + 1))
        '                                                                                          Else
        '                                                                                              Return 0
        '                                                                                          End If
        '                                                                                      End Function)
        Dim M As DenseMatrix = DenseMatrix.Create((SpinInOneHalfs + 1), (SpinInOneHalfs + 1), Function(i As Integer, j As Integer)
                                                                                                  Dim Si As Double = -S + i * 1
                                                                                                  Dim Sj As Double = -S + j * 1
                                                                                                  If Si + 1 = Sj Then
                                                                                                      Return Math.Sqrt(S * (S + 1) - Si * Sj)
                                                                                                  Else
                                                                                                      Return 0
                                                                                                  End If
                                                                                              End Function)
        Return M
    End Function

    ''' <summary>
    ''' Spin matrix Sx.
    ''' </summary>
    <Cudafy>
    Public Shared Function Sx(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Return DirectCast((Splus(SpinInOneHalfs) + Sminus(SpinInOneHalfs)) * 0.5, DenseMatrix)
    End Function

    ''' <summary>
    ''' Spin matrix Sy.
    ''' </summary>
    <Cudafy>
    Public Shared Function Sy(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Return DirectCast((Splus(SpinInOneHalfs) - Sminus(SpinInOneHalfs)) * -0.5 * Numerics.Complex.ImaginaryOne, DenseMatrix)
    End Function

    ''' <summary>
    ''' Angle dependent Spin matrix, with respect to the z-axis. (only in Y-direction).
    ''' </summary>
    <Cudafy>
    Public Shared Function STheta(ByVal SpinInOneHalfs As Integer,
                                  ByVal ThetaAsDegree As Double) As DenseMatrix
        Return Math.Sin(ThetaAsDegree / 180 * Math.PI) * Sz(SpinInOneHalfs) + Math.Cos(ThetaAsDegree / 180 * Math.PI) * Sx(SpinInOneHalfs)
    End Function

    ''' <summary>
    ''' Angle dependent Spin matrix, with respect to the z-axis.
    ''' </summary>
    <Cudafy>
    Public Shared Function SAngle(ByVal SpinInOneHalfs As Integer,
                                  ByVal ThetaAsDegree As Double,
                                  ByVal PhiAsDegree As Double) As DenseMatrix
        Dim X As DenseMatrix = Math.Sin(ThetaAsDegree / 180 * Math.PI) * Math.Cos(PhiAsDegree / 180 * Math.PI) * Sx(SpinInOneHalfs)
        Dim Y As DenseMatrix = Math.Sin(ThetaAsDegree / 180 * Math.PI) * Math.Sin(PhiAsDegree / 180 * Math.PI) * Sy(SpinInOneHalfs)
        Dim Z As DenseMatrix = Math.Cos(ThetaAsDegree / 180 * Math.PI) * Sz(SpinInOneHalfs)
        Return DirectCast(X.Add(Y).Add(Z), DenseMatrix)
    End Function

    ''' <summary>
    ''' Expectation value of an operator.
    ''' </summary>
    Public Shared Function TransitionProbability(ByRef StateInitial As Vector(Of Numerics.Complex),
                                                 ByRef StateFinal As Vector(Of Numerics.Complex),
                                                 ByRef Op As DenseMatrix) As Numerics.Complex
        Return StateFinal.ConjugateDotProduct(Op.Multiply(StateInitial))
    End Function

    ''' <summary>
    ''' Calculate the occupation of the Eigenstates depending on their energy.
    ''' The states are occupied using a Boltzman distribution.
    ''' </summary>
    <Cudafy>
    Public Shared Function Occupation(ByVal EigenValues As DenseVector,
                                      ByVal Temperature As Double) As DenseVector

        ' Calculate initially the smallest eigenvalue of the system,
        ' which is the ground state for the boltzman occupation.
        Dim GroundStateEnergy As Double = Double.MaxValue
        Dim GroundStateDegeneracy As Integer = 0
        For j As Integer = 0 To EigenValues.Count - 1 Step 1
            If EigenValues(j).Real < GroundStateEnergy Then
                GroundStateEnergy = EigenValues(j).Real
                GroundStateDegeneracy = 0
            End If
            If EigenValues(j).Real = GroundStateEnergy Then
                GroundStateDegeneracy += 1
            End If
        Next

        ' Now calculate the total sum of the Boltzman probabilities.
        Dim TotalBoltzmanOccupation As Double = 0
        For j As Integer = 0 To EigenValues.Count - 1 Step 1
            TotalBoltzmanOccupation += BoltzmanDistribution(j, EigenValues, Temperature, GroundStateEnergy, GroundStateDegeneracy)
        Next

        Dim Result As DenseVector
        ' Mathematica code:
        ' occu[eigen_, T_] := (#/Plus @@ #) &@(E^-((eigen - Min[eigen])/(k T)))
        Result = DenseVector.Create(EigenValues.Count, Function(i As Integer)
                                                           Return BoltzmanDistribution(i, EigenValues, Temperature, GroundStateEnergy, GroundStateDegeneracy) / TotalBoltzmanOccupation
                                                       End Function)
        Return Result
    End Function

    ''' <summary>
    ''' Boltzman distributed occupation of state i
    ''' in the list of eigenvalues at temperature T.
    ''' </summary>
    <Cudafy>
    Public Shared Function BoltzmanDistribution(ByVal i As Integer,
                                                ByVal EigenValues As DenseVector,
                                                ByVal Temperature As Double,
                                                ByVal GroundStateEnergy As Double,
                                                ByVal GroundStateDegeneracy As Integer) As Double

        If Temperature <= 0.005 Then
            If EigenValues(i).Real = GroundStateEnergy Then
                Return 1 / GroundStateDegeneracy
            Else
                Return 0
            End If
        Else
            Dim Expo As Double = -(EigenValues(i).Real - GroundStateEnergy) / (cConstants.kB_eV * Temperature)
            Return Math.Exp(Expo)
        End If

        'Return Math.Exp(-(EigenValues(i).Real - EigenValues.Minimum.Real) / (cConstants.kB_eV * Temperature))
    End Function

    ''' <summary>
    ''' Returns the transition probability for a transition from state m to n
    ''' with the given parameters.
    ''' </summary>
    Public Shared Function TransitionProbabilityBetweenStates(ByRef EigenVectors As DenseMatrix,
                                                              ByVal SpinInOneHalfs As Integer,
                                                              ByVal InitialState As Integer,
                                                              ByVal FinalState As Integer) As Double
        Dim ProbPlus As Double = Numerics.Complex.Abs(TransitionProbability(EigenVectors.Column(InitialState), EigenVectors.Column(FinalState), Splus(SpinInOneHalfs)))
        ProbPlus *= ProbPlus
        Dim ProbMinus As Double = Numerics.Complex.Abs(TransitionProbability(EigenVectors.Column(InitialState), EigenVectors.Column(FinalState), Sminus(SpinInOneHalfs)))
        ProbMinus *= ProbMinus
        Dim ProbZero As Double = Numerics.Complex.Abs(TransitionProbability(EigenVectors.Column(InitialState), EigenVectors.Column(FinalState), Sz(SpinInOneHalfs)))
        ProbZero *= (ProbZero * 2)
        Return (ProbPlus + ProbMinus + ProbZero) / 2
    End Function

#End Region

#Region "Cache Crystal Field Hamiltonian"

    Private LastCache_MagneticField As Double = Double.MinValue
    Private LastCache_MagneticFieldAngleTheta As Double = Double.MinValue
    Private LastCache_MagneticFieldAnglePhi As Double = Double.MinValue
    Private LastCache_D As Double = Double.MinValue
    Private LastCache_E As Double = Double.MinValue
    Private LastCache_g As Double = Double.MinValue
    Private LastCache_SpinInOneHalfs As Integer = 0
    Private LastCache_Temperature As Double = Double.MinValue

    Private Cached_HCrystalField As DenseMatrix
    Private Cached_EigenVectors As DenseMatrix
    Private Cached_EigenValues As DenseVector
    Private Cached_Occupation As DenseVector

    ''' <summary>
    ''' Updates the cached crystal field hamiltonian, if necessary.
    ''' </summary>
    Public Sub UpdateCachedCrystalFieldHamiltonian(ByRef InputParameters As cFitParameterGroupGroup,
                                                   ByVal SpinInOneHalfs As Integer)

        Dim MagneticField As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.BField.ToString).Value
        Dim MagneticFieldAngleTheta As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.BAngleTheta.ToString).Value
        Dim MagneticFieldAnglePhi As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.BAnglePhi.ToString).Value
        Dim D As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.D.ToString).Value
        Dim E As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.E.ToString).Value
        Dim g As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.g.ToString).Value
        Dim Temperature As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Temperature.ToString).Value
        Dim EigenSystemChanged As Boolean = False

        ' check for need to update:
        If LastCache_MagneticField <> MagneticField Or
           LastCache_MagneticFieldAngleTheta <> MagneticFieldAngleTheta Or
           LastCache_MagneticFieldAnglePhi <> MagneticFieldAnglePhi Or
           LastCache_D <> D Or
           LastCache_E <> E Or
           LastCache_SpinInOneHalfs <> SpinInOneHalfs Or
           LastCache_g <> g Then

            ' Calculate Hs and solve the eigenvalue system
            Dim HCrystalField As DenseMatrix = CrystalFieldHamiltonian(MagneticField, MagneticFieldAngleTheta, MagneticFieldAnglePhi, D, E, g, SpinInOneHalfs)
            Dim EVDHCrystalField As Factorization.Evd(Of Numerics.Complex) = HCrystalField.Evd

            ' Save the result in the cache
            'Cached_EigenValues = DenseVector.Create(SpinInOneHalfs + 1, Function(i As Integer)
            '                                                                Return EVDHCrystalField.EigenValues(i)
            '                                                            End Function) ' length: 2S+1
            Cached_EigenValues = DirectCast(EVDHCrystalField.EigenValues, DenseVector)
            'Cached_EigenVectors = DirectCast(EVDHCrystalField.EigenVectors.NormalizeColumns(1.0), DenseMatrix)
            Cached_EigenVectors = DirectCast(EVDHCrystalField.EigenVectors, DenseMatrix)

            ' Save cached values
            LastCache_MagneticField = MagneticField
            LastCache_MagneticFieldAngleTheta = MagneticFieldAngleTheta
            LastCache_MagneticFieldAnglePhi = MagneticFieldAnglePhi
            LastCache_D = D
            LastCache_E = E
            LastCache_SpinInOneHalfs = SpinInOneHalfs
            LastCache_g = g
            EigenSystemChanged = True
        End If

        ' calculate occupation, if necessary
        If LastCache_Temperature <> Temperature Or EigenSystemChanged Then
            Cached_Occupation = Occupation(Cached_EigenValues, Temperature)

            ' save last values
            LastCache_Temperature = Temperature
        End If
    End Sub

#End Region

#Region "Fit Function for the precalculation of the dIdV."
    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' PUBLIC VERSION, USING THE MEMBERS OF THE CLASS
    ''' </summary>
    Public Function Current_PRECALC(ByVal VBias As Double) As Double
        ' Return the shared cuda version.
        Return Current_PRECALC_CUDA(VBias,
                                    Me.CurrentIntegralCache_POS, Me.CurrentIntegralCache_NEG,
                                    Me.dVBiasStart_Pos, Me.dVBiasStart_Neg,
                                    dEInterpolation_CurrentPrecalculation)
    End Function

    ''' <summary>
    ''' Access to the precalculated current.
    ''' 
    ''' CUDA VERSION, USING SEPARATE GIVEN ARRAYS
    ''' </summary>
    <Cudafy>
    Protected Shared Function Current_PRECALC_CUDA(ByVal VBias As Double,
                                                   ByVal dIdVIntegralCache_POS As Double(),
                                                   ByVal dIdVIntegralCache_NEG As Double(),
                                                   ByVal dVBiasStart_Pos As Double,
                                                   ByVal dVBiasStart_Neg As Double,
                                                   ByVal dEInterpolation_dIdVPrecalculation As Double) As Double
        ' Reference to the cache array
        Dim dIdVCache As Double()

        ' reference to the next element
        Dim n As Integer
        Dim dPercentOfSlope As Double
        Dim ArrayLength As Integer

        ' depending on the value use different arrays.
        If VBias >= 0 Then
            If dIdVIntegralCache_POS Is Nothing Then Return 0
            dIdVCache = dIdVIntegralCache_POS
            ArrayLength = dIdVIntegralCache_POS.Length

            ' get the index n from the entered bias
            n = CInt((VBias - dVBiasStart_Pos) / dEInterpolation_dIdVPrecalculation)
            dPercentOfSlope = (VBias - dVBiasStart_Pos - n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
        Else
            If dIdVIntegralCache_NEG Is Nothing Then Return 0
            dIdVCache = dIdVIntegralCache_NEG
            ArrayLength = dIdVIntegralCache_NEG.Length

            ' get the index n from the entered bias
            n = CInt(-(VBias - dVBiasStart_Neg) / dEInterpolation_dIdVPrecalculation)
            dPercentOfSlope = -(VBias - dVBiasStart_Neg + n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
        End If

        '  // ** Fallunterscheidung; **
        If ArrayLength = 0 Then
            Return 0
        ElseIf ArrayLength = 1 Or n < 0 Then
            Return dIdVCache(0)
        ElseIf n > ArrayLength - 2 Then
            n = ArrayLength - 2
            If VBias >= 0 Then
                dPercentOfSlope = (VBias - dVBiasStart_Pos - n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
            Else
                dPercentOfSlope = -(VBias - dVBiasStart_Neg + n * dEInterpolation_dIdVPrecalculation) / dEInterpolation_dIdVPrecalculation
            End If
            Return (dPercentOfSlope * (dIdVCache(n + 1) - dIdVCache(n))) + dIdVCache(n)
        Else
            Return (dPercentOfSlope * (dIdVCache(n + 1) - dIdVCache(n))) + dIdVCache(n)
        End If
    End Function

    ''' <summary>
    ''' Fit-Function returning the current signal of a convolved
    ''' tip and sample DOS with spin excited inelastic processes.
    ''' </summary>
    Public Function FitFunctionPRECALC(ByVal VBias As Double,
                                       ByRef InputParameters As cFitParameterGroupGroup) As Double
        Dim VBiasEff As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.XCenter.ToString).Value + VBias

        ' calculate the current
        Dim ReturnVal As Double
        If Me._SignalType = SignalTypes.I Then
            ReturnVal = Current_PRECALC(VBiasEff)
        Else
            ' single sided derivative
            'ReturnVal = -(Current_PRECALC(VBiasEff) - Current_PRECALC(VBiasEff + dEInterpolation_CurrentPrecalculation)) / dEInterpolation_CurrentPrecalculation
            ' double sided derivative
            Dim ILeft As Double = Current_PRECALC((VBiasEff + dEInterpolation_CurrentPrecalculation / 2))
            Dim IRight As Double = Current_PRECALC((VBiasEff - dEInterpolation_CurrentPrecalculation / 2))
            ReturnVal = (ILeft - IRight) / dEInterpolation_CurrentPrecalculation
        End If
        ReturnVal += InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.LinearSlope.ToString).Value * VBiasEff

        ' return the value shifted by the YOffset and stretched by the amplitude
        Return InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Y0.ToString).Value + ReturnVal * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Amplitude.ToString).Value
    End Function

    ''' <summary>
    ''' Fit-Function returning the current signal of a convolved
    ''' tip and sample DOS with spin excited inelastic processes.
    ''' </summary>
    Public Function FitFunctionDirect(ByVal VBias As Double,
                                      ByRef InputParameters As cFitParameterGroupGroup) As Double
        Dim VBiasEff As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.XCenter.ToString).Value + VBias

        ' calculate the current I
        Dim ReturnVal As Double
        If Me._SignalType = SignalTypes.I Then
            ReturnVal = FuncI(VBiasEff, InputParameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, IntegrationEnergyStep)
        Else
            ' single sided derivative
            'ReturnVal = -(Current_PRECALC(VBiasEff) - Current_PRECALC(VBiasEff + dEInterpolation_CurrentPrecalculation)) / dEInterpolation_CurrentPrecalculation
            ' double sided derivative
            Dim ILeft As Double = FuncI((VBiasEff + dEInterpolation_CurrentPrecalculation / 2), InputParameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, IntegrationEnergyStep)
            Dim IRight As Double = FuncI((VBiasEff - dEInterpolation_CurrentPrecalculation / 2), InputParameters, Me.ConvolutionIntegralE_POS, Me.ConvolutionIntegralE_NEG, IntegrationEnergyStep)
            ReturnVal = (ILeft - IRight) / dEInterpolation_CurrentPrecalculation
        End If
        ReturnVal += InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.LinearSlope.ToString).Value * VBiasEff

        ' return the value shifted by the YOffset and stretched by the amplitude
        Return InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Y0.ToString).Value + ReturnVal * InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Amplitude.ToString).Value
    End Function

#End Region

#Region "Convolution Integrand (CPU version)"

    ''' <summary>
    ''' Integrand of the elastic channel.
    ''' </summary>
    ''' <param name="E">Energy of the current integration step.</param>
    ''' <param name="Vb">Bias for which to calculate the integrand.</param>
    ''' <param name="EigenValues">Vector with the Eigenvalues of the system.</param>
    ''' <param name="EigenVectors">Matrix of the Eigenvectors of the system.</param>
    ''' <param name="TransitionProbability">
    ''' Transition probability of the elastic channel.
    ''' It is calculated from the transition probability of the inelastic channel.
    ''' </param>
    ''' <returns>Returns the simple value of the integrand of the elastic channel.</returns>
    Public Function IntegrandEC(ByVal E As Double,
                                ByVal Vb As Double,
                                ByRef InputParameters As cFitParameterGroupGroup,
                                ByVal TransitionProbability As Double) As Double
        Dim Temperature As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Temperature.ToString).Value
        Dim dI As Double = (FermiF_eV(E - Vb, Temperature) - FermiF_eV(E, Temperature)) *
                            TipDOS(E - Vb, InputParameters) * SampleDOS(E, InputParameters) * TransitionProbability
        Return dI
    End Function

    ''' <summary>
    ''' Integrand of the inelastic channel.
    ''' </summary>
    ''' <param name="E">Energy of the current integration step.</param>
    ''' <param name="Vb">Bias for which to calculate the integrand.</param>
    ''' <param name="EigenValues">Vector with the Eigenvalues of the system.</param>
    ''' <param name="EigenVectors">Matrix of the Eigenvectors of the system.</param>
    ''' <param name="SpinInOneHalfs">Current Spin</param>
    ''' <param name="Identifiers">FitParameter-Identifiers</param>
    ''' <param name="Values">FitParameter-Values</param>
    ''' <param name="IntegrandIEC_TransitionProbability">
    ''' Gets filled by the function with the total transition-probability T(m->n) for the
    ''' inelastic process. Is needed for the weighting of the elastic channel.
    ''' </param>
    ''' <returns>The value of the integration step is returned.</returns>
    Public Function IntegrandIEC(ByVal E As Double,
                                 ByVal Vb As Double,
                                 ByRef EigenValues As DenseVector,
                                 ByRef EigenVectors As DenseMatrix,
                                 ByVal SpinInOneHalfs As Integer,
                                 ByRef InputParameters As cFitParameterGroupGroup,
                                 ByRef IntegrandIEC_TransitionProbability As Double) As Double

        ' Reset the IEC transition probability
        IntegrandIEC_TransitionProbability = 0

        ' Convert Spin
        Dim S As Double = SpinInOneHalfs * 0.5
        Dim TwoSPlus1 As Integer = SpinInOneHalfs + 1

        ' Get variables
        Dim Temperature As Double = InputParameters.Group(Me.UseFitParameterGroupID).Parameter(FitParameterIdentifier.Temperature.ToString).Value

        ' Create partial results for forward and backward tunnling
        Dim SumForward As Double = 0
        Dim SumBackward As Double = 0

        ' temporary variable
        Dim tmpOccupation As Double = 0
        Dim tmpDOSTipSampleOverlapFW As Double = 0
        Dim tmpFermiFunctionOverlapFW As Double = 0
        Dim tmpDOSTipSampleOverlapBW As Double = 0
        Dim tmpFermiFunctionOverlapBW As Double = 0
        Dim tmpTransitionProbability As Double = 0
        Dim tmpEigenvalueDifference As Double = 0

        For m As Integer = 0 To TwoSPlus1 - 1 Step 1
            For n As Integer = 0 To TwoSPlus1 - 1 Step 1
                If m = n Then Continue For

                ' calculate the energy difference of the eigenvalues
                tmpEigenvalueDifference = (EigenValues(n).Real - EigenValues(m).Real)

                '########################################################################################
                ' calculate the forward current from all states in the tip to all states in the sample

                ' calculate the overlap of the tip and sample DOS when shifted by the inelastic process
                tmpDOSTipSampleOverlapFW = TipDOS(E - Vb, InputParameters) * SampleDOS(E + tmpEigenvalueDifference, InputParameters)

                ' calculate the Fermi-function overlap shifted by the inelastic process energy
                tmpFermiFunctionOverlapFW = FermiF_eV(E - Vb, Temperature) * (1 - FermiF_eV(E + tmpEigenvalueDifference, Temperature))

                '########################################################################################
                ' calculate the backward current from all states in the sample to all states in the tip

                ' calculate the overlap of the tip and sample DOS when shifted by the inelastic process
                tmpDOSTipSampleOverlapBW = TipDOS(E - Vb + tmpEigenvalueDifference, InputParameters) * SampleDOS(E, InputParameters)

                ' calculate the Fermi-function overlap shifted by the inelastic process energy
                tmpFermiFunctionOverlapBW = (1 - FermiF_eV(E - Vb + tmpEigenvalueDifference, Temperature)) * FermiF_eV(E, Temperature)

                '########################################################################################
                ' calculate the occupation overlap of initial and final states
                'tmpOccupation = Occupation(EigenValues, Temperature)(n).Real * (1 - Occupation(EigenValues, Temperature)(m).Real)
                tmpOccupation = Cached_Occupation(n).Real ' * (1 - Cached_Occupation(m).Real)

                ' calculate the transition probability from state m to n
                tmpTransitionProbability = TransitionProbabilityBetweenStates(EigenVectors, SpinInOneHalfs, m, n)

                '########################################################################################
                ' Add the step to the sum
                SumForward = SumForward + (tmpOccupation * tmpDOSTipSampleOverlapFW * tmpFermiFunctionOverlapFW * tmpTransitionProbability)
                SumBackward = SumBackward + (tmpOccupation * tmpDOSTipSampleOverlapBW * tmpFermiFunctionOverlapBW * tmpTransitionProbability)
                IntegrandIEC_TransitionProbability += 0 * 0.5 * tmpTransitionProbability * tmpOccupation ' / TwoSPlus1
            Next
        Next

        ' Return the forward current, and substract the backward current.
        Return 0.5 * (SumForward - SumBackward) ' / TwoSPlus1
    End Function

#End Region

#Region "Convolution Integral"
    ''' <summary>
    ''' Function to calculate the tunneling current I(Vbias)
    ''' </summary>
    Public Function FuncI(ByVal VBias As Double,
                          ByRef InputParameters As cFitParameterGroupGroup,
                          ByVal MaxEIntegrationRangePos As Double,
                          ByVal MaxEIntegrationRangeNeg As Double,
                          ByVal ConvolutionIntegrationStepSize As Double) As Double

        ' Create the current integration variable
        Dim I As Double = 0.0

        ' Get temporary variables
        Dim E As Double
        Dim IECTransitionProbability As Double = 0.0

        ' Get the maximum number of iterations in the energy-range.
        Dim NmaxPos As Integer = CInt((MaxEIntegrationRangePos * 1.3) / ConvolutionIntegrationStepSize)
        Dim NmaxNeg As Integer = CInt((-MaxEIntegrationRangeNeg * 1.3) / ConvolutionIntegrationStepSize)

        ' Go through all energies for convolution and integrate the individual current contributions.
        For j As Integer = 0 To NmaxPos Step 1
            '######################################
            ' Sum up positive energy contribution
            E = j * ConvolutionIntegrationStepSize

            ' First calculate the IEC, because we need to know its probability
            ' for calculating the remaining probability of the EC.
            I += IntegrandIEC(E, VBias, Cached_EigenValues, Cached_EigenVectors, SpinInOneHalfs, InputParameters, IECTransitionProbability)
            I += IntegrandEC(E, VBias, InputParameters, (1 - IECTransitionProbability))
        Next
        For j As Integer = 0 To NmaxNeg Step 1
            '######################################
            ' Sum up negative energy contribution
            E = -ConvolutionIntegrationStepSize * (j + 1)

            ' First calculate the IEC, because we need to know its probability
            ' for calculating the remaining probability of the EC.
            I += IntegrandIEC(E, VBias, Cached_EigenValues, Cached_EigenVectors, SpinInOneHalfs, InputParameters, IECTransitionProbability)
            I += IntegrandEC(E, VBias, InputParameters, (1 - IECTransitionProbability))
        Next

        ' Return the normalized current
        Return I * ConvolutionIntegrationStepSize
    End Function

#End Region

#Region "Fermi-Function - CUDA compatible"
    ''' <summary>
    ''' // *****************************************************************
    ''' // * Berechnet die Fermiefunktion (in eV Units)                    *
    ''' // *                                                               *
    ''' // *      f(E,T) = 1 / ( exp(E/kT) + 1 )                           *
    ''' // *****************************************************************
    ''' 
    ''' Function supports CUDA!
    ''' </summary>
    <Cudafy>
    Public Shared Function FermiF_eV(ByVal E As Double,
                                     ByVal T As Double) As Double
        '// ** Berechnungsvariablen; **
        Dim f, expo As Double

        '// ** Fallunterscheidung nach T <= 0.0 K oder T > 0.0 K; **
        '// ** (Division durch Null abfangen;) **
        If T <= 0 Then
            If E < 0D Then
                f = 1D
            Else
                f = 0D
            End If
        Else
            '  // ** Berechne Exponent der e-Funktion; **
            expo = E / (cConstants.kB_eV * T) ' * 1000) ' ?????

            '  // ** Berechne Fermi-Funktion; Fange Ueberlauf der e-Funktion ab; **
            If expo < MIN_EXP Or expo > MAX_EXP Then
                If expo < MIN_EXP Then
                    f = 1.0
                Else
                    f = 0.0
                End If
            Else
                f = 1.0 / (Math.Exp(expo) + 1.0)
            End If
        End If

        Return f
    End Function

#End Region

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_IETS_SpinExcitation.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_IETS_SpinExcitation.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_IETS_SpinExcitation.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_IETS_SpinExcitation.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_IETS_SpinExcitation
        End Get
    End Property
#End Region

#Region "Export/Import routine for the additional function parameters (convolution step size, etc.)"

    ''' <summary>
    ''' Write additionally the parameter-settings about enabled or disabled SGPs.
    ''' </summary>
    Protected Overrides Sub Export_WriteAdditionalXMLElements(ByRef XMLWriter As Xml.XmlTextWriter)

        With XMLWriter

            ' Begin the element of the parameter description
            .WriteStartElement("IETSSettings") ' <IETSSettings>

            .WriteStartElement("SpinSystem")
            .WriteAttributeString("SpinInOneHalfs", Me.SpinInOneHalfs.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteEndElement()

            ' Close the Section
            .WriteEndElement() ' <\IETSSettings>

            ' Begin the element of the parameter description
            .WriteStartElement("CurrentIntegralConvolutionSettings") ' <CurrentIntegralConvolutionSettings>
            .WriteAttributeString("CurrentPrecalculationInterpolationStepWidth", Me.dEInterpolation_CurrentPrecalculation.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BiasLimitUpperE", Me.CalculateForBiasRangeUpperE.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("BiasLimitLowerE", Me.CalculateForBiasRangeLowerE.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ConvolutionIntegralEPos", Me.ConvolutionIntegralE_POS.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("ConvolutionIntegralENeg", Me.ConvolutionIntegralE_NEG.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("IntegrationEnergyStep", Me.IntegrationEnergyStep.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteAttributeString("FitDataType", Convert.ToInt32(Me.SignalType).ToString(System.Globalization.CultureInfo.InvariantCulture))
            .WriteEndElement() ' <\CurrentIntegralConvolutionSettings>
        End With

        MyBase.Export_WriteAdditionalXMLElements(XMLWriter)
    End Sub

    ''' <summary>
    ''' Read unknown XML-Elements, if needed by the import.
    ''' </summary>
    Protected Overrides Sub Import_UnknownXMLElementIdentified(ByVal XMLElementName As String, ByRef XMLReader As Xml.XmlReader)
        If XMLElementName = "SpinSystem" Then
            ' Get the IETS settings.
            With XMLReader
                If .AttributeCount > 0 Then
                    While .MoveToNextAttribute
                        Select Case .Name
                            Case "SpinInOneHalfs"
                                Me.SpinInOneHalfs = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                        End Select
                    End While
                End If
            End With
        ElseIf XMLElementName = "CurrentIntegralConvolutionSettings" Then
            With XMLReader
                If .AttributeCount > 0 Then
                    While .MoveToNextAttribute
                        Select Case .Name
                            ' extract all the current integral settings
                            Case "CurrentPrecalculationInterpolationStepWidth"
                                Me.dEInterpolation_CurrentPrecalculation = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "BiasLimitUpperE"
                                Me.CalculateForBiasRangeUpperE = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "BiasLimitLowerE"
                                Me.CalculateForBiasRangeLowerE = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "IntegrationEnergyStep"
                                Me.IntegrationEnergyStep = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "ConvolutionIntegralEPos"
                                Me.ConvolutionIntegralE_POS = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)
                            Case "ConvolutionIntegralENeg"
                                Me.ConvolutionIntegralE_NEG = Convert.ToDouble(.Value, System.Globalization.CultureInfo.InvariantCulture)

                            Case "FitDataType"
                                Me.SignalType = cFitFunction_IETS_SpinExcitation.GetSignalTypeFromInteger(Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture))
                        End Select
                    End While
                End If
            End With
        Else
            ' else pass back to base-class
            MyBase.Import_UnknownXMLElementIdentified(XMLElementName, XMLReader)
        End If
    End Sub

#End Region

End Class
