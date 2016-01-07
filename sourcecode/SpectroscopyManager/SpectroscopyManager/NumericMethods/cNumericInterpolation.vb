Imports MathNet.Numerics.Interpolation
Imports Cudafy

Partial Public Class cNumericalMethods

    ''' <summary>
    ''' Numerical Methods to Interpolate a Function.
    ''' </summary>
    Public Enum InterpolationMethods
        AkimaSpline
        BulirschStoerRationalInterpolation
        CubicSplineInterpolation
        LinearSplineInterpolation
        NevillePolynomialInterpolation
    End Enum

    ''' <summary>
    ''' Returns the Selected Interpolation Method.
    ''' </summary>
    Public Shared Function GetInterpolationMethodFromType(ByRef InColumnX As ICollection(Of Double),
                                                          ByRef InColumnY As ICollection(Of Double),
                                                          ByVal Method As InterpolationMethods) As IInterpolation
        Select Case Method
            Case InterpolationMethods.AkimaSpline
                Return MathNet.Numerics.Interpolate.CubicSplineRobust(InColumnX, InColumnY)
            Case InterpolationMethods.BulirschStoerRationalInterpolation
                Return New BulirschStoerRationalInterpolation(InColumnX.ToArray, InColumnY.ToArray)
            Case InterpolationMethods.CubicSplineInterpolation
                Return MathNet.Numerics.Interpolate.CubicSpline(InColumnX, InColumnY)
            Case InterpolationMethods.LinearSplineInterpolation
                Return MathNet.Numerics.Interpolate.Linear(InColumnX, InColumnY)
            Case InterpolationMethods.NevillePolynomialInterpolation
                Return New NevillePolynomialInterpolation(InColumnX.ToArray, InColumnY.ToArray)
            Case Else
                Return MathNet.Numerics.Interpolate.CubicSpline(InColumnX, InColumnY)
        End Select
    End Function

    ''' <summary>
    ''' Returns a Description-Text for the Interpolation-Method.
    ''' </summary>
    Public Shared Function GetInterpolationDescriptionFromType(ByVal Method As InterpolationMethods) As String
        Select Case Method
            Case InterpolationMethods.AkimaSpline
                Return My.Resources.InterpolationDescription_Akima
            Case InterpolationMethods.BulirschStoerRationalInterpolation
            Case InterpolationMethods.CubicSplineInterpolation
            Case InterpolationMethods.LinearSplineInterpolation
            Case InterpolationMethods.NevillePolynomialInterpolation
            Case Else
        End Select
        Return ""
    End Function

#Region "1D Spline Interpolation"
    ''' <summary>
    ''' Interpolates the Data-Columns using a selected Spline-Interpolation
    ''' and returns a list of the interpolation function at the X points given in the EvaluatedAtXColumn list.
    ''' </summary>
    Public Shared Function SplineInterpolation(InColumnX As ICollection(Of Double),
                                               InColumnY As ICollection(Of Double),
                                               ByRef EvaluatedAtXColumn As List(Of Double),
                                               Optional ByVal Method As cNumericalMethods.InterpolationMethods = InterpolationMethods.CubicSplineInterpolation) As List(Of Double)
        Dim oInterpolationMethod As IInterpolation = cNumericalMethods.GetInterpolationMethodFromType(InColumnX, InColumnY, Method)

        ' Create Out-Data-Lists.
        Dim OutX As List(Of Double) = EvaluatedAtXColumn
        Dim OutY As New List(Of Double)(EvaluatedAtXColumn.Count)

        For i As Integer = 1 To OutX.Count Step 1
            OutY.Add(oInterpolationMethod.Interpolate(OutX(i - 1)))
        Next

        Return OutY
    End Function

    ''' <summary>
    ''' Interpolates the Data-Columns using a selected Spline-Interpolation
    ''' and returns a list of the derivatives at the X points of the input column.
    ''' </summary>
    Public Shared Function SplineInterpolation(ByRef InDataColumnX As cSpectroscopyTable.DataColumn,
                                               ByRef InDataColumnY As cSpectroscopyTable.DataColumn,
                                               ByRef EvaluatedAtXColumn As List(Of Double),
                                               Optional ByVal Method As cNumericalMethods.InterpolationMethods = InterpolationMethods.AkimaSpline) As List(Of Double)
        Return cNumericalMethods.SplineInterpolation(InDataColumnX.Values, InDataColumnY.Values, EvaluatedAtXColumn, Method)
    End Function
#End Region

#Region "1D Spline Interpolation - Derivative"
    ''' <summary>
    ''' Interpolates the Data-Columns using a selected Spline-Interpolation
    ''' and returns a list of the derivatives at the X points of the input column.
    ''' </summary>
    Public Shared Function SplineInterpolationDerivative(ByRef InColumnX As ICollection(Of Double),
                                                         ByRef InColumnY As ICollection(Of Double),
                                                         Optional ByVal Method As cNumericalMethods.InterpolationMethods = InterpolationMethods.AkimaSpline) As List(Of Double)
        Dim oInterpolationMethod As IInterpolation = cNumericalMethods.GetInterpolationMethodFromType(InColumnX, InColumnY, Method)

        ' Create Out-Data-Lists.
        Dim OutY As New List(Of Double)(InColumnX.Count)

        For i As Integer = 0 To InColumnX.Count - 1 Step 1
            OutY.Add(oInterpolationMethod.Differentiate(InColumnX(i)))
        Next

        Return OutY
    End Function

    ''' <summary>
    ''' Interpolates the Data-Columns using a selected Spline-Interpolation
    ''' and returns a list of the derivatives at the X points of the input column.
    ''' </summary>
    Public Shared Function SplineInterpolationDerivative(ByRef InDataColumnX As cSpectroscopyTable.DataColumn,
                                                         ByRef InDataColumnY As cSpectroscopyTable.DataColumn,
                                                         Optional ByVal Method As cNumericalMethods.InterpolationMethods = InterpolationMethods.AkimaSpline) As List(Of Double)
        Return cNumericalMethods.SplineInterpolationDerivative(InDataColumnX.Values, InDataColumnY.Values, Method)
    End Function
#End Region

#Region "2D Spline Interpolation"
    ''' <summary>
    ''' This function uses spline interpolation to reduce or extend the data 
    ''' from a source matrix to a new matrix of the given number of points!
    ''' </summary>
    Public Shared Function SplineInterpolation2D(ByRef SourceMatrix As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix,
                                                 ByVal TargetPoints_X As Integer,
                                                 ByVal TargetPoints_Y As Integer) As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix

        If TargetPoints_X <= 0 Then Throw New ArgumentOutOfRangeException("TargetPoints_X", "InterpolateScanData->TargetPoints_X must be > 0!")
        If TargetPoints_Y <= 0 Then Throw New ArgumentOutOfRangeException("TargetPoints_Y", "InterpolateScanData->TargetPoints_Y must be > 0!")
        If SourceMatrix.ColumnCount <= 0 Then Throw New ArgumentOutOfRangeException("ScanData.ColumnCount", "InterpolateScanData->ScanData_X must contain elements!")
        If SourceMatrix.RowCount <= 0 Then Throw New ArgumentOutOfRangeException("ScanData.RowCount", "InterpolateScanData->ScanData_Y must contain elements!")

        ' Create new matrix
        Dim TargetMatrix As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(TargetPoints_Y, TargetPoints_X, Function(x As Integer, y As Integer) As Double
                                                                                                                                                                             Return Double.NaN
                                                                                                                                                                         End Function)

        '####################################
        ' Interpolate data first row by row.
        Dim TMPMatrix As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(SourceMatrix.RowCount, TargetPoints_X, Function(x As Integer, y As Integer) As Double
                                                                                                                                                                                 Return Double.NaN
                                                                                                                                                                             End Function)

        ' Generate input X-Column for interpolation
        Dim InputXColumn As New List(Of Double)(SourceMatrix.ColumnCount)
        For x As Integer = 0 To SourceMatrix.ColumnCount - 1 Step 1
            InputXColumn.Add(x)
        Next

        ' Generate interpolated X-Column
        Dim InterpolatedX As New List(Of Double)(TargetPoints_X)
        For x As Integer = 0 To TargetPoints_X - 1 Step 1
            InterpolatedX.Add(x * InputXColumn.Count / TargetPoints_X)
        Next

        ' Interpolate
        Dim InterpolatedRow As List(Of Double)
        Dim Source As List(Of Double)
        For y As Integer = 0 To SourceMatrix.RowCount - 1 Step 1
            Source = SourceMatrix.Row(y).ToList

            ' Ignore the row, if it contains invalid data
            If Source.Contains(Double.NaN) Then Continue For

            ' Interpolate
            InterpolatedRow = cNumericalMethods.SplineInterpolation(InputXColumn,
                                                                    Source,
                                                                    InterpolatedX,
                                                                    cNumericalMethods.InterpolationMethods.CubicSplineInterpolation)

            ' Set the output data to the tmp-matrix
            For x As Integer = 0 To TMPMatrix.ColumnCount - 1 Step 1
                TMPMatrix(y, x) = InterpolatedRow(x)
            Next
        Next
        ' END OF ROW BY ROW
        '###################################

        ' Now process the columns.

        '#############################################
        ' Generate interpolated data Column by Column

        ' Generate input X-Column for interpolation
        InputXColumn = New List(Of Double)(SourceMatrix.RowCount)
        For x As Integer = 0 To SourceMatrix.RowCount - 1 Step 1
            InputXColumn.Add(x)
        Next

        ' Interpolate
        Dim InterpolatedColumn As List(Of Double)
        Dim IndexNonNaN_Start As Integer
        Dim IndexNonNaN_End As Integer
        Dim IndexNonNan_Range As Integer
        Dim InterpolatedXTargetLength As Integer
        Dim InterpolatedXSumTerm As Double
        Dim InterpolatedX_ToTargetMatrix_YOffset As Integer
        Dim InterpolatedX_ToTargetMatrix_Difference As Integer
        For x As Integer = 0 To TargetPoints_X - 1 Step 1
            Source = TMPMatrix.Column(x).ToList

            If Source.Count <= 0 Then Continue For

            IndexNonNaN_Start = Source.Count - 1
            IndexNonNaN_End = 0

            ' Filter out the values of the source column, which contain no NaN
            For y As Integer = 0 To Source.Count - 1 Step 1
                If Not Double.IsNaN(Source(y)) Then
                    If y < IndexNonNaN_Start Then IndexNonNaN_Start = y
                    If y > IndexNonNaN_End Then IndexNonNaN_End = y
                End If
            Next

            ' Calculate count of values
            IndexNonNan_Range = IndexNonNaN_End - IndexNonNaN_Start

            ' Check, if the range is > 0, so if we have to do an interpolation
            If IndexNonNan_Range <= 0 Then Continue For

            ' Generate interpolated X-Column
            InterpolatedXTargetLength = CInt(TargetPoints_Y * (IndexNonNan_Range + 1) / Source.Count)
            InterpolatedXSumTerm = (InputXColumn(IndexNonNaN_End) - InputXColumn(IndexNonNaN_Start)) / InterpolatedXTargetLength
            InterpolatedX = New List(Of Double)(InterpolatedXTargetLength)
            InterpolatedX_ToTargetMatrix_YOffset = CInt(InputXColumn(IndexNonNaN_Start) * TargetPoints_Y / InputXColumn.Count)
            For y As Integer = 0 To InterpolatedXTargetLength - 1 Step 1
                InterpolatedX.Add(InputXColumn(IndexNonNaN_Start) + y * InterpolatedXSumTerm)
            Next

            ' Interpolate
            InterpolatedColumn = cNumericalMethods.SplineInterpolation(InputXColumn.GetRange(IndexNonNaN_Start, IndexNonNan_Range),
                                                                       Source.GetRange(IndexNonNaN_Start, IndexNonNan_Range),
                                                                       InterpolatedX,
                                                                       cNumericalMethods.InterpolationMethods.CubicSplineInterpolation)
            ' Set the output data to the TargetMatrix
            For y As Integer = 0 To TargetPoints_Y - 1 Step 1
                InterpolatedX_ToTargetMatrix_Difference = y - InterpolatedX_ToTargetMatrix_YOffset
                If InterpolatedX_ToTargetMatrix_Difference >= 0 And InterpolatedX_ToTargetMatrix_Difference < InterpolatedXTargetLength Then
                    TargetMatrix(y, x) = InterpolatedColumn(InterpolatedX_ToTargetMatrix_Difference)
                End If
            Next
        Next

        ' Release the tmp-matrix
        TMPMatrix = Nothing

        Return TargetMatrix
    End Function
#End Region

#Region "Own 2nd Order Polynomial Interpolation (CUDA-compatible)"
    ''' <summary>
    ''' Interpolates between points using a 2nd order polynomial interpolation.
    ''' </summary>
    Public Shared Function SplineInterpolationNative(ByRef InX As Double(),
                                                     ByRef InY As Double(),
                                                     ByVal EvaluatedAtX As Double) As Double
        ' Security check
        If InX.Length <> InY.Length Then Throw New ArgumentException("spline interpolation failed: Number of X points is not matching number of Y points.")

        ' Take care of the outer most points.
        If EvaluatedAtX = InX(0) Then
            Return InX(0)
        End If
        If EvaluatedAtX = InX(InX.Length - 1) Then
            Return InX(InX.Length - 1)
        End If

        ' Count the number of datapoints.
        Dim np As Integer = InX.Length

        If np > 1 Then
            Dim a As Double() = New Double(np - 1) {}
            Dim x1 As Double
            Dim x2 As Double
            Dim y As Double
            Dim h As Double() = New Double(np - 1) {}
            For i As Integer = 1 To np - 1
                h(i) = InX(i) - InX(i - 1)
            Next

            If np > 2 Then
                Dim [sub] As Double() = New Double(np - 2) {}
                Dim diag As Double() = New Double(np - 2) {}
                Dim sup As Double() = New Double(np - 2) {}

                For i As Integer = 1 To np - 2
                    diag(i) = (h(i) + h(i + 1)) / 3
                    sup(i) = h(i + 1) / 6
                    [sub](i) = h(i) / 6
                    a(i) = (InY(i + 1) - InY(i)) / h(i + 1) - (InY(i) - InY(i - 1)) / h(i)
                Next

                ' SolveTridiag is a support function, see Marco Roello's original code
                ' for more information at
                ' http://www.codeproject.com/useritems/SplineInterpolation.asp

                SolveTridiag([sub], diag, sup, a, np - 2)
            End If

            Dim gap As Integer = 0
            Dim previous As Double = Double.MinValue

            ' At the end of this iteration, "gap" will contain the index of the interval
            ' between two known values, which contains the unknown y, and "previous" will
            ' contain the biggest z value among the known samples, left of the unknown z
            For i As Integer = 0 To InX.Count - 1 Step 1
                If InX(i) < EvaluatedAtX AndAlso InX(i) > previous Then
                    previous = InX(i)
                    gap = i + 1
                End If
            Next

            If gap >= h.Length Then gap = h.Length - 1

            x1 = EvaluatedAtX - previous
            x2 = h(gap) - x1

            y = ((-a(gap - 1) / 6 * (x2 + h(gap)) * x1 + InY(gap - 1)) * x2 + (-a(gap) / 6 * (x1 + h(gap)) * x2 + InY(gap)) * x1) / h(gap)
            Return y
        End If

        Return 0
    End Function

    ''' <summary>
    ''' Interpolates between points using a 2nd order polynomial interpolation.
    ''' </summary>
    Public Shared Sub SplineInterpolationNative(ByRef InX As Double(),
                                                ByRef InY As Double(),
                                                ByRef EvaluatedAtX As Double(),
                                                ByRef OutY As Double())

        ' Check length
        If OutY.Length <> EvaluatedAtX.Length Then Throw New ArgumentException("SplineInterpolation: OutY.Length <> EvaluateAtX.Length")

        ' Calculate interpolation for each point.
        For i As Integer = 0 To EvaluatedAtX.Count - 1 Step 1
            OutY(i) = SplineInterpolationNative(InX, InY, EvaluatedAtX(i))
        Next
    End Sub


    ''' <summary>
    ''' Interpolates the Data-Columns using a Selected Spline-Interpolation
    ''' and returns a List of the Interpolation Function at the X Points of the Input Column.
    ''' </summary>
    Public Shared Function Polynomial2ndOrderInterpolation(ByRef InDataColumnX As cSpectroscopyTable.DataColumn,
                                                           ByRef InDataColumnY As cSpectroscopyTable.DataColumn,
                                                           ByRef EvaluatedAtXColumn As List(Of Double)) As Double()
        Dim OutY(EvaluatedAtXColumn.Count - 1) As Double
        SplineInterpolationNative(InDataColumnX.Values.ToArray, InDataColumnY.Values.ToArray, EvaluatedAtXColumn.ToArray, OutY)
        Return OutY
    End Function

    ''' <summary>
    ''' solve linear system with tridiagonal n by n matrix a
    ''' using Gaussian elimination *without* pivoting
    ''' </summary>
    Private Shared Sub SolveTridiag(S As Double(), diag As Double(), sup As Double(), b As Double(), n As Integer)
        ' where   a(i,i-1) = S[i]  for 2<=i<=n
        ' a(i,i)   = diag[i] for 1<=i<=n
        ' a(i,i+1) = sup[i]  for 1<=i<=n-1
        ' (the values S[1], sup[n] are ignored)
        ' right hand side vector b[1:n] is overwritten with solution 
        ' NOTE: 1...n is used in all arrays, 0 is unused 

        Dim i As Integer

        ' factorization and forward substitution 
        For i = 2 To n
            S(i) = S(i) / diag(i - 1)
            diag(i) = diag(i) - S(i) * sup(i - 1)
            b(i) = b(i) - S(i) * b(i - 1)
        Next
        b(n) = b(n) / diag(n)
        For i = n - 1 To 1 Step -1
            b(i) = (b(i) - sup(i) * b(i + 1)) / diag(i)
        Next
    End Sub

#End Region

#Region "OWN 1D Cosine Interpolation"

    ''' <summary>
    ''' Simple cosine interpolation between two points.
    ''' </summary>
    <Cudafy>
    Public Shared Function CosineInterpolationCudafy(y1 As Double,
                                                     y2 As Double,
                                                     EvaluateAtX As Double) As Double
        Dim mu2 As Double = (1 - Math.Cos(EvaluateAtX * cConstants.Pi)) / 2
        Return (y1 * (1 - mu2) + y2 * mu2)
    End Function

#End Region

#Region "OWN 1D Cubic Interpolation"

    ''' <summary>
    ''' Simple Cubic interpolation between four points.
    ''' </summary>
    <Cudafy>
    Public Shared Function CubicInterpolationCudafy(y0 As Double,
                                                    y1 As Double,
                                                    y2 As Double,
                                                    y3 As Double,
                                                    EvaluateAtX As Double) As Double
        Dim a0, a1, a2, a3, mu2 As Double

        mu2 = EvaluateAtX * EvaluateAtX
        a0 = y3 - y2 - y0 + y1
        a1 = y0 - y1 - a0
        a2 = y2 - y0
        a3 = y1

        Return (a0 * EvaluateAtX * mu2 + a1 * mu2 + a2 * EvaluateAtX + a3)
    End Function

#End Region

End Class
