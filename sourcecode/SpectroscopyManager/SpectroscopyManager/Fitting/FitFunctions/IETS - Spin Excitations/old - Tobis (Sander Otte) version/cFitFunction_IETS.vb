Imports MathNet.Numerics.LinearAlgebra
Imports MathNet.Numerics.LinearAlgebra.Double

Public Class cFitFunction_IETS
    Inherits cFitFunction

#Region "Initialize Fit-Parameters"

    ''' <summary>
    ''' Gives the currently spin which is setup.
    ''' </summary>
    Public SpinInOneHalfs As Integer = 2

    Public Shared eV As Double = 1.602E-19 '1.602E-22
    Public Shared mu As Double = 9.274E-24 '0.000057883761 ' in eV/T ' SI: 9.274E-24 J/T
    Public Shared k As Double = 1.38E-23 ' 0.000086173324 ' in eV/K ' SI: 1.38E-23 J/K

    Public Shared IntegrationEnergyStep As Double = 0.000022342123934545346

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
        BAngle
        g
    End Enum

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        Me.FitParameters.Add(FitParameterIdentifier.Y0, New sFitParameter("Y0", 0, False, My.Resources.rFitFunction_IETS.Parameter_Y0))
        Me.FitParameters.Add(FitParameterIdentifier.XCenter, New sFitParameter("Xc", 0, False, My.Resources.rFitFunction_IETS.Parameter_XCenter))
        Me.FitParameters.Add(FitParameterIdentifier.Amplitude, New sFitParameter("A", 1, False, My.Resources.rFitFunction_IETS.Parameter_Amplitude))
        Me.FitParameters.Add(FitParameterIdentifier.LinearSlope, New sFitParameter("slope", 0, False, My.Resources.rFitFunction_IETS.Parameter_LinearSlope))
        Me.FitParameters.Add(FitParameterIdentifier.D, New sFitParameter("D", 0.003, False, My.Resources.rFitFunction_IETS.Parameter_D))
        Me.FitParameters.Add(FitParameterIdentifier.E, New sFitParameter("E", 0.001, False, My.Resources.rFitFunction_IETS.Parameter_E))
        Me.FitParameters.Add(FitParameterIdentifier.Temperature, New sFitParameter("temperature", 1.2, False, My.Resources.rFitFunction_IETS.Parameter_Temperature))
        Me.FitParameters.Add(FitParameterIdentifier.BField, New sFitParameter("BField", 0, False, My.Resources.rFitFunction_IETS.Parameter_BField))
        Me.FitParameters.Add(FitParameterIdentifier.BAngle, New sFitParameter("BAngle", 90, False, My.Resources.rFitFunction_IETS.Parameter_BAngle))
        Me.FitParameters.Add(FitParameterIdentifier.g, New sFitParameter("g", 2, False, My.Resources.rFitFunction_IETS.Parameter_g))
    End Sub
#End Region

#Region "FitFunction"

    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' </summary>
    ''' <param name="x">x-value at which the fit function should be evaluated.</param>
    Public Overrides Function GetY(ByRef x As Double, ByRef Identifiers As Integer(), ByRef Values As Double()) As Double

        ' Check the cached values
        ' Calculate Hs and solve the eigenvalue system
        Me.UpdateCachedCrystalFieldHamiltonianIfNecessary(sFitParameter.GetValueForIdentifier(FitParameterIdentifier.BField, Identifiers, Values),
                                                          sFitParameter.GetValueForIdentifier(FitParameterIdentifier.BAngle, Identifiers, Values),
                                                          sFitParameter.GetValueForIdentifier(FitParameterIdentifier.D, Identifiers, Values),
                                                          sFitParameter.GetValueForIdentifier(FitParameterIdentifier.E, Identifiers, Values),
                                                          sFitParameter.GetValueForIdentifier(FitParameterIdentifier.g, Identifiers, Values),
                                                          SpinInOneHalfs)

        ' Calculate the function value.
        Return FitFunction(x,
                           sFitParameter.GetValueForIdentifier(FitParameterIdentifier.Y0, Identifiers, Values),
                           sFitParameter.GetValueForIdentifier(FitParameterIdentifier.Amplitude, Identifiers, Values),
                           sFitParameter.GetValueForIdentifier(FitParameterIdentifier.XCenter, Identifiers, Values),
                           sFitParameter.GetValueForIdentifier(FitParameterIdentifier.LinearSlope, Identifiers, Values),
                           sFitParameter.GetValueForIdentifier(FitParameterIdentifier.Temperature, Identifiers, Values),
                           Me.Cached_EigenValues,
                           Me.Cached_EigenVectors,
                           SpinInOneHalfs)
    End Function

    ''' <summary>
    ''' Fit-Function, Lorentzian-Distribution
    ''' ' calculate the IETS value
    ''' '###################
    ''' ' Mathematica Code:
    ''' 'Sign[V]*
    ''' 'NIntegrate[
    ''' ' Sum[
    ''' '  (
    ''' '    occu[eigen90, T][[m]] (1 - occu[eigen90, T][[n]])
    ''' '    )
    ''' '   *(
    ''' '    1/2*Abs[eigenV90[[n]]\[Conjugate].Sp[S].eigenV90[[m]]]^2
    ''' '     + 1/2*Abs[eigenV90[[n]]\[Conjugate].Sm[S].eigenV90[[m]]]^2
    ''' '     + Abs[eigenV90[[n]]\[Conjugate].Sz[S].eigenV90[[m]]]^2
    ''' '    )
    ''' '   *(
    ''' '    Sech[(eigen90[[m]]/meV - eigen90[[n]]/meV + x)/(k/meV*T)]^2
    ''' '    ),
    ''' '  {m, 1, (2 S + 1)},
    ''' '  {n, m + 1, (2 S + 1)}],
    ''' ' {x, -V, V}]]
    ''' '##################
    ''' </summary>
    Public Shared Function FitFunction(ByVal V As Double,
                                       ByVal YOffset As Double,
                                       ByVal Amplitude As Double,
                                       ByVal XCenter As Double,
                                       ByVal LinearSlope As Double,
                                       ByVal Temperature As Double,
                                       ByRef EigenValues As DenseVector,
                                       ByRef EigenVectors As DenseMatrix,
                                       ByVal SpinInOneHalfs As Integer) As Double

        ' Set offsets
        Dim EffectiveBias As Double = V - XCenter

        ' Convert Spin
        Dim S As Double = SpinInOneHalfs * 0.5
        Dim TwoSPlus1 As Integer = SpinInOneHalfs + 1

        Dim IETS As Double = 0

        ' Integrate numerically from -V to V
        Dim dV As Double = -Math.Abs(EffectiveBias)
        Dim Vmax As Double = Math.Abs(EffectiveBias)
        Dim SumStep As Double = 0
        While (dV < Vmax)

            Dim Sum As Double = 0
            ' Evaluate the double sum over n and m
            For m As Integer = 0 To TwoSPlus1 - 1 Step 1
                For n As Integer = m + 1 To TwoSPlus1 - 1 Step 1

                    ' calculate the state occupation
                    SumStep = Occupation(EigenValues, Temperature)(m) * (1 - Occupation(EigenValues, Temperature)(n))

                    Dim Res1 As Double = Math.Abs(EigenVectors.Row(n).Conjugate * Splus(SpinInOneHalfs) * EigenVectors.Row(m))
                    Res1 = Res1 * Res1 / 2
                    Dim Res2 As Double = Math.Abs(EigenVectors.Row(n).Conjugate * Sminus(SpinInOneHalfs) * EigenVectors.Row(m))
                    Res2 = Res2 * Res2 / 2
                    Dim Res3 As Double = Math.Abs(EigenVectors.Row(n).Conjugate * Sz(SpinInOneHalfs) * EigenVectors.Row(m))
                    Res3 = Res3 * Res3

                    SumStep *= (Res1 + Res2 + Res3)

                    Dim Res4 As Double = (EigenValues(m) / eV - EigenValues(n) / eV + dV) / (k / eV * Temperature)
                    Res4 = 1 / Math.Cosh(Res4)
                    Res4 = Res4 * Res4

                    SumStep *= Res4

                    ' Add the step to the sum
                    Sum += SumStep
                Next
            Next

            IETS += Sum

            ' Count up the integration step.
            dV += IntegrationEnergyStep
        End While
        Return (IntegrationEnergyStep * 20000) * IETS * Amplitude + LinearSlope * EffectiveBias + YOffset
    End Function


    Private LastCache_MagneticField As Double = Double.MinValue
    Private LastCache_MagneticFieldAngle As Double = Double.MinValue
    Private LastCache_D As Double = Double.MinValue
    Private LastCache_E As Double = Double.MinValue
    Private LastCache_g As Double = Double.MinValue
    Private LastCache_SpinInOneHalfs As Integer = 0

    Private Cached_HCrystalField As DenseMatrix
    Private Cached_EigenVectors As DenseMatrix
    Private Cached_EigenValues As DenseVector

    ''' <summary>
    ''' Updates the cached crystal field hamiltonian, if necessary.
    ''' </summary>
    Public Sub UpdateCachedCrystalFieldHamiltonianIfNecessary(ByVal MagneticField As Double,
                                                              ByVal MagneticFieldAngle As Double,
                                                              ByVal D As Double,
                                                              ByVal E As Double,
                                                              ByVal g As Double,
                                                              ByVal SpinInOneHalfs As Integer)

        ' check for need to update:
        If LastCache_MagneticField <> MagneticField Or
           LastCache_MagneticFieldAngle <> MagneticFieldAngle Or
           LastCache_D <> D Or
           LastCache_E <> E Or
           LastCache_SpinInOneHalfs <> SpinInOneHalfs Or
           LastCache_g <> g Then

            ' Calculate Hs and solve the eigenvalue system
            Dim HCrystalField As DenseMatrix = CrystalFieldHamiltonian(MagneticField, MagneticFieldAngle, D, E, g, SpinInOneHalfs)
            Dim EVDHCrystalField As Factorization.Evd(Of Double) = HCrystalField.Evd

            ' Save the result in the cache
            Cached_EigenValues = DenseVector.Create(SpinInOneHalfs + 1, Function(i As Integer)
                                                                            Return EVDHCrystalField.EigenValues(i).Real
                                                                        End Function) ' length: 2S+1
            Cached_EigenVectors = DirectCast(EVDHCrystalField.EigenVectors.NormalizeColumns(1.0), DenseMatrix)

            ' Save cached values
            LastCache_MagneticField = MagneticField
            LastCache_MagneticFieldAngle = MagneticFieldAngle
            LastCache_D = D
            LastCache_E = E
            LastCache_SpinInOneHalfs = SpinInOneHalfs
            LastCache_g = g
        End If
    End Sub


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

    Public Shared Function Sminus(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Dim S As Double = SpinInOneHalfs * 0.5
        Dim M As DenseMatrix = DenseMatrix.Create((SpinInOneHalfs + 1), (SpinInOneHalfs + 1), Function(i As Integer, j As Integer)
                                                                                                  Dim Si As Double = -S + i * 1
                                                                                                  Dim Sj As Double = -S + j * 1
                                                                                                  If Si + 1 = Sj Then
                                                                                                      Return Math.Sqrt((S + Sj) * (S - Sj + 1))
                                                                                                  Else
                                                                                                      Return 0
                                                                                                  End If
                                                                                              End Function)
        Return M
    End Function

    Public Shared Function Splus(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Dim S As Double = SpinInOneHalfs * 0.5
        Dim M As DenseMatrix = DenseMatrix.Create((SpinInOneHalfs + 1), (SpinInOneHalfs + 1), Function(i As Integer, j As Integer)
                                                                                                  Dim Si As Double = -S + i * 1
                                                                                                  Dim Sj As Double = -S + j * 1
                                                                                                  If Si - 1 = Sj Then
                                                                                                      Return Math.Sqrt((S + Si) * (S - Si + 1))
                                                                                                  Else
                                                                                                      Return 0
                                                                                                  End If
                                                                                              End Function)
        Return M
    End Function

    'Public Shared Function Sy(ByVal SpinInOneHalfs As Integer) As DenseMatrix
    '    Return DirectCast((Sp(SpinInOneHalfs) - Sm(SpinInOneHalfs)).Divide(2 * Numerics.Complex.ImaginaryOne), DenseMatrix)
    'End Function

    Public Shared Function Sx(ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Return DirectCast((Splus(SpinInOneHalfs) + Sminus(SpinInOneHalfs)) * 0.5, DenseMatrix)
    End Function

    Public Shared Function STheta(ByVal SpinInOneHalfs As Integer,
                                  ByVal ThetaAsDegree As Double) As DenseMatrix
        Return Math.Sin(ThetaAsDegree / 180 * Math.PI) * Sz(SpinInOneHalfs) + Math.Cos(ThetaAsDegree / 180 * Math.PI) * Sx(SpinInOneHalfs)
    End Function

    Public Shared Function CrystalFieldHamiltonian(ByVal B As Double,
                                                   ByVal ThetaAsDegree As Double,
                                                   ByVal D As Double,
                                                   ByVal E As Double,
                                                   ByVal g As Double,
                                                   ByVal SpinInOneHalfs As Integer) As DenseMatrix
        Dim M1 As DenseMatrix = DirectCast(-g * mu * B * STheta(SpinInOneHalfs, ThetaAsDegree) + D * eV * (Sz(SpinInOneHalfs) * Sz(SpinInOneHalfs)), DenseMatrix)
        Dim M2 As DenseMatrix = DirectCast(E * 0.5 * eV * (Splus(SpinInOneHalfs) * Splus(SpinInOneHalfs) + Sminus(SpinInOneHalfs) * Sminus(SpinInOneHalfs)), DenseMatrix)
        Return M1 + M2
    End Function

    Public Shared Function ExpectactionValue(ByRef EigenVectors As DenseMatrix,
                                             ByRef Op As DenseMatrix) As DenseMatrix
        Return DirectCast(EigenVectors.Conjugate * Op * EigenVectors, DenseMatrix)
    End Function

    Public Shared Function Occupation(ByVal EigenValues As DenseVector,
                                      ByVal Temperature As Double) As DenseVector
        ' Mathematica code:
        ' occu[eigen_, T_] := (#/Plus @@ #) &@(E^-((eigen - Min[eigen])/(k T)))
        Dim Result As DenseVector = DenseVector.Create(EigenValues.Count, Function(i As Integer)
                                                                              Dim R As Double = 0
                                                                              For j As Integer = 0 To EigenValues.Count - 1 Step 1
                                                                                  R += OccupationExponent(j, EigenValues, Temperature)
                                                                              Next
                                                                              R = OccupationExponent(i, EigenValues, Temperature) / R
                                                                              Return R
                                                                          End Function)
        Return Result
    End Function

    Public Shared Function OccupationExponent(ByVal i As Integer,
                                              ByRef EigenValues As DenseVector,
                                              ByRef Temperature As Double) As Double
        Return Math.Exp(-(EigenValues(i) - EigenValues.Minimum) / (k / eV * Temperature))
    End Function

#End Region

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Return My.Resources.rFitFunction_IETS.Formula
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Return My.Resources.rFitFunction_IETS.Name
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_IETS.Description
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel_IETS
        End Get
    End Property
#End Region

End Class
