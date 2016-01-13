Partial Public Class cNumericalMethods

    ''' <summary>
    ''' Methods for Smoothing Data.
    ''' </summary>
    Public Enum SmoothingMethod
        AdjacentAverageSmooth
        SavitzkyGolay
    End Enum

    ''' <summary>
    ''' Returns a description-text for the smoothing-method.
    ''' </summary>
    Public Shared Function GetSmoothingDescriptionFromType(ByVal Method As SmoothingMethod) As String
        Select Case Method
            Case SmoothingMethod.AdjacentAverageSmooth
                Return My.Resources.SmoothingDescription_AdjacentAverage
            Case SmoothingMethod.SavitzkyGolay
                Return My.Resources.SmoothingDescription_SavitzkyGolay
        End Select
        Return ""
    End Function

    ''' <summary>
    ''' Smoothes the given data using the given number of next neighbors.
    ''' </summary>
    Public Shared Function AdjacentAverageSmooth(ByRef InColumn As ICollection(Of Double),
                                                 Optional ByVal Neighbors As Integer = 1) As List(Of Double)
        Dim OutY As New List(Of Double)

        Dim SumY As Double = 0D
        Dim iii As Integer = 0
        Dim iif As Integer = 0
        Dim ni As Integer = 0
        Dim nf As Integer = InColumn.Count - 1

        For n As Integer = ni To nf Step 1
            SumY = 0D

            If Neighbors < n - ni Then
                iii = n - ni - Neighbors
            Else
                iii = 0
            End If

            If Neighbors > nf - n Then
                iif = nf - ni
            Else
                iif = n - ni + Neighbors
            End If

            For i As Integer = iii To iif Step 1
                SumY += InColumn(i)
            Next
            OutY.Add(SumY / (iif - iii + 1))
        Next
        Return OutY
    End Function

    ''' <summary>
    ''' Smoothes the given data using the given number of next neighbors.
    ''' </summary>
    Public Shared Function AdjacentAverageSmooth(ByRef InDataColumn As cSpectroscopyTable.DataColumn,
                                                 Optional ByVal Neighbors As Integer = 1) As List(Of Double)
        Return cNumericalMethods.AdjacentAverageSmooth(InDataColumn.Values, Neighbors)
    End Function

    ''' <summary>
    ''' Smoothes a given dataset using the SavitzkyGolay polynomial fitting method.
    ''' It uses second order polynomials, and a moving average window of size NP.
    ''' </summary>
    Public Shared Function SavitzkyGolaySmooth(ByRef InColumnY As ICollection(Of Double),
                                               ByVal Neighbors As Integer) As List(Of Double)

        ' Create the matrix of the Savitzky-Golay Coefficients
        Dim SGCoef As MathNet.Numerics.LinearAlgebra.Single.DenseMatrix = MathNet.Numerics.LinearAlgebra.Single.DenseMatrix.Create(13, 14, Function(x As Integer, y As Integer) As Single
                                                                                                                                               Return 0
                                                                                                                                           End Function)
        ' Set the smoothing Coefficients for Savitzky-Golay
        ' The zeroth value is the normalization factor
        ' SGCoef(Neighbors)
        SGCoef(2, 1) = 17
        SGCoef(2, 2) = 12
        SGCoef(2, 3) = -3
        SGCoef(2, 0) = 35

        SGCoef(3, 1) = 7
        SGCoef(3, 2) = 6
        SGCoef(3, 3) = 3
        SGCoef(3, 4) = -2
        SGCoef(3, 0) = 21

        SGCoef(4, 1) = 59
        SGCoef(4, 2) = 54
        SGCoef(4, 3) = 39
        SGCoef(4, 4) = 14
        SGCoef(4, 5) = -21
        SGCoef(4, 0) = 231

        SGCoef(5, 1) = 89
        SGCoef(5, 2) = 84
        SGCoef(5, 3) = 69
        SGCoef(5, 4) = 44
        SGCoef(5, 5) = 9
        SGCoef(5, 6) = -36
        SGCoef(5, 0) = 429

        SGCoef(6, 1) = 25
        SGCoef(6, 2) = 24
        SGCoef(6, 3) = 21
        SGCoef(6, 4) = 16
        SGCoef(6, 5) = 9
        SGCoef(6, 6) = 0
        SGCoef(6, 7) = -11
        SGCoef(6, 0) = 143

        SGCoef(7, 1) = 167
        SGCoef(7, 2) = 162
        SGCoef(7, 3) = 147
        SGCoef(7, 4) = 122
        SGCoef(7, 5) = 87
        SGCoef(7, 6) = 42
        SGCoef(7, 7) = -13
        SGCoef(7, 8) = -78
        SGCoef(7, 0) = 1105

        SGCoef(8, 1) = 43
        SGCoef(8, 2) = 42
        SGCoef(8, 3) = 39
        SGCoef(8, 4) = 34
        SGCoef(8, 5) = 27
        SGCoef(8, 6) = 18
        SGCoef(8, 7) = 7
        SGCoef(8, 8) = -6
        SGCoef(8, 9) = -21
        SGCoef(8, 0) = 323

        SGCoef(9, 1) = 269
        SGCoef(9, 2) = 264
        SGCoef(9, 3) = 249
        SGCoef(9, 4) = 224
        SGCoef(9, 5) = 189
        SGCoef(9, 6) = 144
        SGCoef(9, 7) = 89
        SGCoef(9, 8) = 24
        SGCoef(9, 9) = -51
        SGCoef(9, 10) = -136
        SGCoef(9, 0) = 2261

        SGCoef(10, 1) = 329
        SGCoef(10, 2) = 324
        SGCoef(10, 3) = 309
        SGCoef(10, 4) = 284
        SGCoef(10, 5) = 249
        SGCoef(10, 6) = 204
        SGCoef(10, 7) = 149
        SGCoef(10, 8) = 84
        SGCoef(10, 9) = 9
        SGCoef(10, 10) = -76
        SGCoef(10, 11) = -171
        SGCoef(10, 0) = 3059

        SGCoef(11, 1) = 79
        SGCoef(11, 2) = 78
        SGCoef(11, 3) = 75
        SGCoef(11, 4) = 70
        SGCoef(11, 5) = 63
        SGCoef(11, 6) = 54
        SGCoef(11, 7) = 43
        SGCoef(11, 8) = 30
        SGCoef(11, 9) = 15
        SGCoef(11, 10) = -2
        SGCoef(11, 11) = -21
        SGCoef(11, 12) = -42
        SGCoef(11, 0) = 806

        SGCoef(12, 1) = 467
        SGCoef(12, 2) = 462
        SGCoef(12, 3) = 447
        SGCoef(12, 4) = 422
        SGCoef(12, 5) = 387
        SGCoef(12, 6) = 322
        SGCoef(12, 7) = 287
        SGCoef(12, 8) = 222
        SGCoef(12, 9) = 147
        SGCoef(12, 10) = 62
        SGCoef(12, 11) = -33
        SGCoef(12, 12) = -138
        SGCoef(12, 13) = -253
        SGCoef(12, 0) = 5175

        ' This method of the Savitzky-Golay smoothing algorithm fits the data window
        ' given by a certain number of neighbors to a second order polynomial.
        ' This method assumes a fixed spacing of the data points.

        Dim NumberOfPoints As Integer = InColumnY.Count

        Dim SmoothedY As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(NumberOfPoints)

        'we cannot smooth too close to the data bounds
        For i As Integer = 0 To Neighbors Step 1
            SmoothedY(i) = InColumnY(i)
        Next
        For i As Integer = NumberOfPoints - Neighbors - 1 To NumberOfPoints - 1 Step 1
            SmoothedY(i) = InColumnY(i)
        Next

        Dim TempSum As Double = 0

        For i As Integer = Neighbors + 1 To NumberOfPoints - Neighbors - 2 Step 1
            TempSum = InColumnY(i) * SGCoef(Neighbors, 1)
            For j As Integer = 1 To Neighbors Step 1
                TempSum += InColumnY(i - j) * (SGCoef(Neighbors, j + 1))
                TempSum += InColumnY(i + j) * (SGCoef(Neighbors, j + 1))
            Next
            SmoothedY(i) = TempSum / SGCoef(Neighbors, 0)
        Next

        Return New List(Of Double)(SmoothedY.ToArray)

        '######################################
        ' GUNNAR's Method

        'Dim YError As New List(Of Double)
        'For Each Elem In InColumnY
        '    YError.Add(1D)
        'Next

        'Dim ni As Integer = 0
        'Dim nf As Integer = InColumnY.Count - 1

        ''// ** Order of Savitzky-Golay polynoms; **
        ''// ** Do not go higher than 5 or signal will get very noisy; **
        ''// ** (Maybe another alorithm than solving the normal equation can help;) **
        ''// ** Do not go below 3, cause then the 2nd derivative cannot be built any more; **
        ''const int maxM = 4;
        'Const maxM As Integer = 4

        ''// ** Apply Savitzky-Golay; **
        ''{
        ''  unsigned int iii, iif;
        ''  unsigned int n, i;
        ''  double* A;
        ''  double* a;
        ''  double* b;
        ''  double x;
        ''  int m;

        'Dim iii As Integer
        'Dim iif As Integer

        'Dim x As Double
        'Dim m As Integer

        ''  // ** Walk through every point; **
        ''  for( n = ni; n <= nf; n++ )
        ''  {
        'For n As Integer = ni To nf Step 1
        '    '    // ** Get fitting interval [iii, iif]; **
        '    '    if( I != NULL ) {
        '    If Not i = Nothing Then
        '        '      if( n < ni + *I )
        '        '        iii = ni;
        '        '              Else
        '        '      {
        '        '        if( n + *I > nf )
        '        '          iii = nf - 2 * *I;
        '        '                  Else
        '        '          iii = n - *I;
        '        '      }

        '        '      if( n + *I > nf )
        '        '        iif = nf;
        '        '                      Else
        '        '      {
        '        '        if( n < ni + *I )
        '        '          iif = ni + 2 * *I;
        '        '                          Else
        '        '          iif = n + *I;
        '        '      }

        '        If n < ni + i Then
        '            iii = ni
        '        Else
        '            If n + i > nf Then
        '                iii = nf - 2 * i
        '            Else
        '                iii = n - i
        '            End If
        '        End If

        '        If n + i > nf Then
        '            iif = nf
        '        Else
        '            If n < ni + i Then
        '                iif = ni + 2 * i
        '            Else
        '                iif = n + i
        '            End If
        '        End If

        '        '      // ** Set polynomial order for this section; **
        '        '      // ** Reduce order on the borders down to 3 to avoid overswinging; **
        '        '    If ((IIf() - n) < (n - iii)) Then
        '        '        m = ( ( iif - n ) / *I ) * ( maxM - 3 ) + 3;
        '        '                              Else
        '        '        m = ( ( n - iii ) / *I ) * ( maxM - 3 ) + 3;
        '        '    }
        '        If (iif - n) < (n - iii) Then
        '            m = Convert.ToInt32((iif - n) / i * (maxM - 3) + 3)
        '        Else
        '            m = Convert.ToInt32((n - iii) / i * (maxM - 3) + 3)
        '        End If
        '    Else
        '        '      // ** First: Push upper limit upwards; **
        '        '      iif = n;
        '        '      double xMax = source.x[n];
        '        '      double xMin = source.x[n];
        '        iif = n
        '        Dim xMax As Double = InColumnX(n)
        '        Dim xMin As Double = InColumnX(n)

        '        '    While (IIf() < nf)
        '        '      {
        '        '        iif++;

        '        '        if( source.x[iif] > xMax )
        '        '          xMax = source.x[iif];

        '        '        if( source.x[iif] < xMin )
        '        '          xMin = source.x[iif];

        '        '        if( xMax - xMin >= *delX )
        '        '          break;
        '        '      }
        '        While iif < nf
        '            iif += 1

        '            If InColumnX(iif) > xMax Then xMax = InColumnX(iif)

        '            If InColumnX(iif) < xMin Then xMin = InColumnX(iif)

        '            If xMax - xMin >= delX Then Exit While

        '        End While

        '        '      // ** Second: Push lower limit downwards; **
        '        '      iii = n;
        '        '      While (iii > ni)
        '        '      {
        '        '        iii--;

        '        '        if( source.x[iii] > xMax )
        '        '          xMax = source.x[iii];

        '        '        if( source.x[iii] < xMin )
        '        '          xMin = source.x[iii];

        '        '        if( xMax - xMin >= 2 * *delX )
        '        '          break;
        '        '      }
        '        iii = n
        '        While iii > ni
        '            iii -= 1

        '            If InColumnX(iii) > xMax Then xMax = InColumnX(iii)

        '            If InColumnX(iii) < xMin Then xMin = InColumnX(iii)

        '            If xMax - xMin >= 2 * delX Then Exit While

        '        End While

        '        '      // ** Third: Try to push upper limit even further, (if lower limit was blocked by bounds); **
        '        '      if( xMax - xMin < 2 * *delX )
        '        '        While (IIf() < nf)
        '        '        {
        '        '          iif++;

        '        '          if( source.x[iif] > xMax )
        '        '            xMax = source.x[iif];

        '        '          if( source.x[iif] < xMin )
        '        '            xMin = source.x[iif];

        '        '          if( xMax - xMin >= 2 * *delX )
        '        '            break;
        '        '        }
        '        If xMax - xMin < 2 * delX Then
        '            While iif < nf
        '                iif += 1

        '                If InColumnX(iif) > xMax Then xMax = InColumnX(iif)

        '                If InColumnX(iif) < xMin Then xMin = InColumnX(iif)

        '                If xMax - xMin >= 2 * delX Then Exit While

        '            End While
        '        End If

        '        '      // ** Set polynomial order for this section; **
        '        '      // ** Reduce order on the borders down to 3 to avoid overswinging; **
        '        '      if( ( iii == ni ) || ( iif == nf ) )
        '        '      {
        '        '        if( fabs( source.x[iif] - source.x[n] ) < fabs( source.x[iii] - source.x[n] ) )
        '        '          m = ( fabs( source.x[iif] - source.x[n] ) / *delX ) * ( maxM - 3 ) + 3;
        '        '        Else
        '        '          m = ( fabs( source.x[iii] - source.x[n] ) / *delX ) * ( maxM - 3 ) + 3;
        '        '      }
        '        '      else
        '        '        m = maxM;
        '        '    }

        '        If iii = ni Or iif = nf Then
        '            If Math.Abs(InColumnX(iif) - InColumnX(n)) < Math.Abs(InColumnX(iii) - InColumnX(n)) Then
        '                m = Convert.ToInt32(Math.Abs(InColumnX(iif) - InColumnX(n)) / delX * (maxM - 3)) + 3
        '            Else
        '                m = Convert.ToInt32(Math.Abs(InColumnX(iii) - InColumnX(n)) / delX * (maxM - 3)) + 3
        '            End If
        '        Else
        '            m = maxM
        '        End If
        '    End If


        '    '    // ** Allocate memory for the matrices A, a and b; **
        '    '    A = (double*) malloc( sizeof( double ) * m * m );
        '    '    a = (double*) malloc( sizeof( double ) * m * 3 );
        '    '    b = (double*) malloc( sizeof( double ) * m * 3 );
        '    Dim A As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(m * m)
        '    Dim aa As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(m * 3)
        '    Dim b As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(m * 3)

        '    '    // ** Calculate matrix A's and vector b's elements of the normal equation A a = b; **
        '    '    // ** Store vector b in the first column of the matrix b; **
        '    '    for( unsigned int p = 0; p < m; p++ )
        '    For p As Integer = 0 To m - 1 Step 1
        '        '      b[p * 3 + 0] = 0.0;
        '        '      b[p * 3 + 1] = 0.0;
        '        '      b[p * 3 + 2] = 0.0;
        '        b.Item(p * 3 + 0) = 0
        '        b.Item(p * 3 + 1) = 0
        '        b.Item(p * 3 + 2) = 0

        '        '      for( unsigned int q = p; q < m; q++ )
        '        For q As Integer = p To m - 1 Step 1
        '            '        A[p * m + q] = 0.0;
        '            A(p * m + q) = 0

        '            '        for( i = iii; i <= iif; i++ )
        '            For j As Integer = iii To iif Step 1
        '                '          x = source.x[i] - source.x[n];
        '                x = InColumnX(j) - InColumnX(n)

        '                '          A[p * m + q] += pow( x, p ) * pow( x, q ) / ( source.ey[i] * source.ey[i] );
        '                A(p * m + q) += Math.Pow(x, p) * Math.Pow(x, q) / Math.Pow(YError(i), 2)

        '                '          if( q == p )
        '                '            b[p * 3 + 0] += source.y[i] * pow( x, p ) / ( source.ey[i] * source.ey[i] );
        '                If q = p Then b(p * 3 + 0) += InColumnY(i) * Math.Pow(x, p) / Math.Pow(YError(i), 2)
        '            Next
        '            '        A[q * m + p] = A[p * m + q];
        '            A(q * m + p) = A(p * m + q)
        '        Next
        '    Next

        '    '    // ** Prepare the matrix b's second and third column with ( 0, 1, 0, 0, ... ) and **
        '    '    // ** ( 0, 0, 1, 0, ... ) to get the second and third diagonal element C_11 and C_22 of the inverse **
        '    '    // ** matrix C = A^-1; These elements give the standard error of the corresponding element of a **
        '    '    // ** (see Numerical Recipes Ch. 15.4 ); **
        '    '    b[1 * 3 + 1] = 1.0;
        '    '    b[2 * 3 + 2] = 1.0;
        '    b(1 * 3 + 1) = 1.0
        '    b(2 * 3 + 2) = 1.0

        '    '    // ** Solve normal equation (use advantage, that A is symmetric and positiv definite); **
        '    '    if( !DiagCholesky( &A[0 * m + 0], &a[0 * 3 + 0], &b[0 * 3 + 0], m, 3 ) )
        '    '      return false;


        '    '    // ** Save first derivative component and its error = sqrt( C_11 ); **
        '    '    if( outder1 != NULL )
        '    '    {
        '    '      outder1->y[n] = a[1 * 3 + 0];

        '    '      if( a[1 * 3 + 1] > 0.0 )
        '    '        outder1->ey[n] = sqrt( a[1 * 3 + 1] );
        '    '      Else
        '    '       If (n > ni) Then
        '    '          outder1->ey[n] = outder1->ey[n - 1];
        '    '       Else
        '    '          outder1->ey[n] = 1.0;
        '    '    }

        '    '    // ** Save second derivative component its error = sqrt( C_22 ); **
        '    '    if( outder2 != NULL )
        '    '    {
        '    '      outder2->y[n] = 2.0 * a[2 * 3 + 0];

        '    '      if( a[2 * 3 + 2] > 0.0 )
        '    '        outder2->ey[n] = 2.0 * sqrt( a[2 * 3 + 2] );
        '    '      Else
        '    '        If (n > ni) Then
        '    '          outder2->ey[n] = outder2->ey[n - 1];
        '    '        Else
        '    '          outder2->ey[n] = 1.0;
        '    '    }

        'Next
    End Function

End Class