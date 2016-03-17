Partial Public Class cNumericalMethods

    ''' <summary>
    ''' Re-gauge dIdV-data by using the usually known data-acuisition-parameters of the lock-in amplifier.
    ''' The modulation amplitude is given in RMS. So it is multiplied by Sqrt[2].
    ''' 
    ''' RenormalizeByNumbers[dIdVSignalFromLockIn_, Vmod_, SensitivityRange_, AmplifierGain_] := dIdVSignalFromLockIn*SensitivityRange/Vmod/10^(AmplifierGain);
    ''' </summary>
    Public Shared Function RenormalizeByDataAcuisitionParameters(ByRef InColumn As ICollection(Of Double),
                                                                 ByVal BiasModulationAmplitude As Double,
                                                                 ByVal LockInSensitivity As Double,
                                                                 ByVal AmplifierGain As Integer) As List(Of Double)
        Dim OutColumn As New List(Of Double)(InColumn.Count)
        Dim Sqrt2 As Double = Math.Sqrt(2)

        For i As Integer = 0 To InColumn.Count - 1 Step 1
            If Double.IsNaN(InColumn(i)) OrElse BiasModulationAmplitude = 0 Then
                OutColumn.Add(Double.NaN)
            Else
                OutColumn.Add(InColumn(i) * LockInSensitivity / (BiasModulationAmplitude * Sqrt2) / Math.Pow(10, AmplifierGain))
            End If
        Next

        'For i As Integer = 0 To InColumn.Count - 1 Step 1
        '    If Double.IsNaN(InColumn(i)) Or BiasModulationAmplitude = 0 Then
        '        OutColumn.Add(Double.NaN)
        '    Else
        '        OutColumn.Add(InColumn(i) * LockInSensitivity / BiasModulationAmplitude / Math.Pow(10, AmplifierGain))
        '    End If
        'Next

        Return OutColumn
    End Function

    ''' <summary>
    ''' Renormalizes using a given parameter for the linear stretch (m), and the offset (y0).
    ''' target = y0 + m * source
    ''' </summary>
    Public Shared Function RenormalizeByGivenParameters(ByVal InColumnY As ICollection(Of Double),
                                                        ByVal in_y0 As Double,
                                                        ByVal in_m As Double) As List(Of Double)

        ' Multiply Y-Target Values with Fit-Parameters.
        Dim ColumnYTarget As New List(Of Double)(InColumnY.Count)
        For i As Integer = 0 To InColumnY.Count - 1 Step 1
            ColumnYTarget.Add(in_y0 + in_m * InColumnY(i))
        Next

        Return ColumnYTarget
    End Function

    ''' <summary>
    ''' Renormalizes the Values of Target to Source,
    ''' by trying a Fit of Target in Source,
    ''' using a Squeez-Factor, and an Offset.
    ''' </summary>
    Public Shared Function RenormalizeByDerivativeFitting(ByRef InColumnXSource As ICollection(Of Double),
                                                          ByRef InColumnYSource As ICollection(Of Double),
                                                          ByVal InColumnXTarget As ICollection(Of Double),
                                                          ByVal InColumnYTarget As ICollection(Of Double),
                                                          ByRef out_y0 As Double,
                                                          ByRef out_m As Double,
                                                          ByVal UseBoundaries As Boolean,
                                                          Optional ByVal XMin As Integer = -1,
                                                          Optional ByVal XMax As Integer = -1) As List(Of Double)

        ' Set the boundaries of the fit.
        If UseBoundaries Then
            If XMin < 0 Then XMin = 0
            If XMax < 0 Then XMax = InColumnXSource.Count - 1
            If XMin > XMax Then
                ' turn around, if xmin > xmax
                Dim t As Integer = XMax
                XMax = XMin
                XMin = t
            End If
            If XMax - XMin <= 0 Then
                Throw New ArgumentException("the given boundaries were incorrect: " & XMin & " -> " & XMax)
            End If
        Else
            ' Set boundaries to the maximum size of the input array.
            XMax = InColumnXSource.Count - 1
            XMin = 0
        End If

        out_m = 0D
        out_y0 = 0D
        cNumericalMethods.FitDataToFunction(InColumnXSource,
                                            InColumnYSource,
                                            InColumnXTarget,
                                            InColumnYTarget,
                                            out_m,
                                            out_y0,
                                            XMin,
                                            XMax)

        'Dim oFitResult As FitResult = cNumericalMethods.FitDataToFunction(InColumnXSource,
        '                                                                  InColumnYSource,
        '                                                                  InColumnXTarget,
        '                                                                  InColumnYTarget,
        '                                                                  InterpolationMethod)
        ' Multiply Y-Target Values with Fit-Parameters.
        Dim ColumnYTarget As New List(Of Double)
        For i As Integer = 0 To InColumnYTarget.Count - 1 Step 1
            ColumnYTarget.Add(out_y0 + out_m * InColumnYTarget(i))
        Next

        Return ColumnYTarget
    End Function

    ''' <summary>
    ''' Returns the Parameters for a Fit of the Target Data
    ''' to an interpolated Version of the Source-Data using the Fit-Function:
    ''' 
    ''' Source = y0 + m * Target
    ''' </summary>
    Public Shared Function FitDataToFunction(ByRef InColumnXSource As ICollection(Of Double),
                                             ByRef InColumnYSource As ICollection(Of Double),
                                             ByRef InColumnXTarget As ICollection(Of Double),
                                             ByRef InColumnYTarget As ICollection(Of Double),
                                             ByRef m As Double,
                                             ByRef y0 As Double,
                                             ByVal XMin As Integer,
                                             ByVal XMax As Integer) As Boolean
        ' Gunnars Fit-Function

        '// ** Start minimum bracketing methode (see Numerical Recipes); **
        'double ml, mr;
        'double fl, fr, f;
        '{
        '  // ** Precalculate expected scaling value; **
        '  double mEx;
        '  {
        '    double sdNum, sdLock;
        '    Statistics1D( numdata, ni, nf, NULL, &sdNum, NULL );
        '    Statistics1D( lockdata, ni, nf, NULL, &sdLock, NULL );

        '    if( ( sdNum == 0.0 ) || ( sdLock == 0.0 ) )
        '      return false;

        '    mEx = sdNum / sdLock;
        '}

        ' Estimation-Values as Start-Values for Fitting:
        Dim SourceStat As cNumericalMethods.sNumericStatistics = cNumericalMethods.Statistics1D(InColumnYSource, XMin, XMax)
        Dim TargetStat As cNumericalMethods.sNumericStatistics = cNumericalMethods.Statistics1D(InColumnYTarget, XMin, XMax)

        Dim ml As Double
        Dim mr As Double
        Dim fl As Double
        Dim fr As Double
        Dim f As Double
        Dim mEx As Double ' Expected m

        If SourceStat.StandardDeviation = 0 Or TargetStat.StandardDeviation = 0 Then Return False

        mEx = SourceStat.StandardDeviation / TargetStat.StandardDeviation

        '  // ** Start with expectation value as right and 80% of it as left start value; Intermediate value inbetween; **
        '  mr = mEx;
        '  ml = 0.8 * mr;
        '  *m = 0.9 * mr;

        mr = mEx
        ml = 0.8 * mr
        m = 0.9 * mr

        '  // ** Find minimum bracketing interval itteratively; **
        '  double stepSize = 0.2 * mr;
        '  fl = F6ChiSq( numdata, lockdata, ni, nf, ml, y0 );
        '  f  = F6ChiSq( numdata, lockdata, ni, nf, *m, y0 );
        '  fr = F6ChiSq( numdata, lockdata, ni, nf, mr, y0 );
        '  while( !( ( f < fl ) && ( f < fr ) ) )
        '  {
        '              If (fl < fr) Then
        '    {
        '      *m = ml;
        '      f = fl;
        '      ml = *m - stepSize;
        '      fl = F6ChiSq( numdata, lockdata, ni, nf, ml, y0 );
        '    }
        '              Else
        '    {
        '      *m = mr;
        '      f = fr;
        '      mr = *m + stepSize;
        '      fr = F6ChiSq( numdata, lockdata, ni, nf, mr, y0 );
        '    }

        '    // ** Breakoff for pathologic cases; **
        '                  If (mr - ml > 40.0 * mEx) Then
        '      return false;
        '  }
        '}

        Dim StepSize As Double = 0.2 * mr
        Dim YSourceError As ICollection(Of Double) = Enumerable.Repeat(1.0, InColumnYSource.Count).ToList
        Dim YTargetError As ICollection(Of Double) = Enumerable.Repeat(1.0, InColumnYSource.Count).ToList

        fl = cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, ml, y0, XMin, XMax)
        f = cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, m, y0, XMin, XMax)
        fr = cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, mr, y0, XMin, XMax)

        While Not (f < fl And f < fr)
            If fl < fr Then
                m = ml
                f = fl
                ml = m - StepSize
                fl = cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, ml, y0, XMin, XMax)
            Else
                m = mr
                f = fr
                mr = m + StepSize
                fr = cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, mr, y0, XMin, XMax)
            End If

            If mr - ml > 40 * mEx Then
                Return False
            End If
        End While

        '// ** Start itterative bracketing minimum finding algorithm; **
        'const double ONEOVERPHI = 1.0 / phi; // = 0.618...
        'const double epsSqrt = 3.0e-8;
        'bool geom = true;
        'double mtest, ftest;

        'Const ONEOVERPHI As Double = 1 / MathNet.Numerics.Constants.GoldenRatio
        Const epsSqrt As Double = 0.00000003
        Dim geom As Boolean = True

        Dim mtest As Double
        Dim ftest As Double

        'While (mr - ml > epsSqrt * (fabs(mr) + fabs(ml)))
        '{
        '  If (geom) Then
        '  {
        '    mtest = mr - ONEOVERPHI * ( mr - *m );
        '    ftest = F6ChiSq( numdata, lockdata, ni, nf, mtest, y0 );

        '    If (ftest < f) Then
        '    {
        '      ml = *m;
        '      *m = mtest;
        '      f = ftest;
        '    }
        '     Else
        '    {
        '      mr = mtest;
        '      geom = false;
        '    }
        '  }
        '  else
        '  {
        '    mtest = ml + ONEOVERPHI * ( *m - ml );
        '    ftest = F6ChiSq( numdata, lockdata, ni, nf, mtest, y0 );

        '     If (ftest < f) Then
        '    {
        '      mr = *m;
        '      *m = mtest;
        '      f = ftest;
        '    }
        '   Else
        '    {
        '      ml = mtest;
        '      geom = true;
        '    }
        '  }
        '}

        While mr - ml > epsSqrt * (Math.Abs(mr) + Math.Abs(ml))
            If geom Then
                mtest = mr - (mr - m) / MathNet.Numerics.Constants.GoldenRatio
                ftest = cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, mtest, y0, XMin, XMax)

                If ftest < f Then
                    ml = m
                    m = mtest
                    f = ftest
                Else
                    mr = mtest
                    geom = False
                End If

            Else
                mtest = ml + (m - ml) / MathNet.Numerics.Constants.GoldenRatio
                ftest = cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, mtest, y0, XMin, XMax)

                If ftest < f Then
                    mr = m
                    m = mtest
                    f = ftest
                Else
                    ml = mtest
                    geom = True
                End If
            End If
        End While

        '// ** Start Function F6ChiSq() once more with optimal m for also getting the optimal y0; **
        'F6ChiSq( numdata, lockdata, ni, nf, *m, y0 );
        cNumericalMethods.FitTwoDatasetsChiSq(InColumnYSource, YSourceError, InColumnYTarget, YTargetError, m, y0, XMin, XMax)
        Return True
    End Function

    ''' <summary>
    ''' // *****************************************************************
    ''' // * Function ChiSquare();  The chi^2 of  two mea-                 *
    ''' // * surement curves  shiftet and  scaled ( v1 = m V2 - c )  looks *
    ''' // * like                                                          *
    ''' // *                                                               *
    ''' // *                   ( v1_i - ( m * v2_i + c ) )^2               *
    ''' // *   chi^2 = sum_i ( ----------------------------- )             *
    ''' // *                       sig1^2 + (m sig2)^2                     *
    ''' // *                                                               *
    ''' // * with the meas. values v1_i = num. deriv., v2_i = lockin der.  *
    ''' // * and the standard deviations sig1, sig2;                       *
    ''' // *****************************************************************
    ''' </summary>
    Public Shared Function FitTwoDatasetsChiSq(ByRef InColumnYSource As ICollection(Of Double),
                                               ByRef InColumnYSourceError As ICollection(Of Double),
                                               ByRef InColumnYTarget As ICollection(Of Double),
                                               ByRef InColumnYTargetError As ICollection(Of Double),
                                               ByRef m As Double,
                                               ByRef y0 As Double,
                                               ByVal XMin As Integer,
                                               ByVal XMax As Integer) As Double
        Dim zs1 As Double
        Dim zs2 As Double

        '###############
        ' Calculate y0
        'double zs1, zs2;

        '// ** Calculate y0; **
        '{
        '  double S = 0.0;
        '  double COUNT = 0.0;

        '  for( unsigned int i = ni; i <= nf; i++ )
        '  {
        '    zs1 = numdata.y[i] - m * lockdata.y[i];
        '    zs2 = numdata.ey[i] * numdata.ey[i] + m * m * lockdata.ey[i] * lockdata.ey[i];

        '    COUNT += zs1 / zs2;
        '    S += 1.0 / zs2;
        '  }

        '  *y0 = COUNT / S;
        '}
        Dim S As Double = 0D
        Dim Count As Double = 0D
        For i As Integer = XMin To XMax Step 1
            If Double.IsNaN(InColumnYSource(i)) Or Double.IsInfinity(InColumnYSource(i)) Then Continue For
            zs1 = InColumnYSource(i) - m * InColumnYTarget(i)
            zs2 = InColumnYSourceError(i) * InColumnYSourceError(i) - m * m * InColumnYTargetError(i) * InColumnYTargetError(i)
            Count += zs1 / zs2
            S += 1.0 / zs2
        Next

        y0 = Count / S

        '#################
        ' Calculate Chi^2 
        '// ** Calculate Chi^2; **
        'double chiSq = 0.0;
        'for( unsigned int i = ni; i <= nf; i++ )
        '{
        '  zs1 = numdata.y[i] - m * lockdata.y[i] - *y0;
        '  chiSq += ( zs1 * zs1 ) /
        '           ( numdata.ey[i] * numdata.ey[i] + m * m * lockdata.ey[i] * lockdata.ey[i] );
        '}
        Dim ChiSq As Double = 0D
        For i As Integer = XMin To XMax Step 1
            If Double.IsNaN(InColumnYSource(i)) Or Double.IsInfinity(InColumnYSource(i)) Then Continue For
            zs1 = InColumnYSource(i) - m * InColumnYTarget(i) - y0
            ChiSq += (zs1 * zs1) / (InColumnYSourceError(i) * InColumnYSourceError(i) - m * m * InColumnYTargetError(i) * InColumnYTargetError(i))
        Next

        Return ChiSq
    End Function
End Class
