Partial Public Class cNumericalMethods
    ''' <summary>
    ''' Represents Statistical Parameters of a Set of Data.
    ''' </summary>
    Public Structure sNumericStatistics
        Public Mean As Double
        Public StandardDeviation As Double
        Public Variance As Double
        Public MeanError As Double
        Public CoefficientOfDetermination_R2 As Double
        Public ResidualSumOfSquares As Double
        Public TotalSumOfSquares As Double

        ''' <summary>
        ''' Residual Sum of Squares often called RSS or Chi2
        ''' </summary>
        Public ReadOnly Property RSS As Double
            Get
                Return Me.ResidualSumOfSquares
            End Get
        End Property

        ''' <summary>
        ''' Residual Sum of Squares often called RSS or Chi2
        ''' </summary>
        Public ReadOnly Property Chi2 As Double
            Get
                Return Me.ResidualSumOfSquares
            End Get
        End Property

        ''' <summary>
        ''' Total Sum of Squares often called TSS
        ''' </summary>
        Public ReadOnly Property TSS As Double
            Get
                Return Me.TotalSumOfSquares
            End Get
        End Property

        Public Sub New(_Mean As Double,
                       _StandardDeviation As Double,
                       _Variance As Double,
                       _MeanError As Double,
                       _CoefficientOfDetermination_R2 As Double,
                       _ResidualSumOfSquares As Double,
                       _TotalSumOfSquares As Double)
            Me.Mean = _Mean
            Me.StandardDeviation = _StandardDeviation
            Me.Variance = _Variance
            Me.MeanError = _MeanError
            Me.CoefficientOfDetermination_R2 = _CoefficientOfDetermination_R2
            Me.ResidualSumOfSquares = _ResidualSumOfSquares
            Me.TotalSumOfSquares = _TotalSumOfSquares
        End Sub
    End Structure

    ''' <summary>
    ''' #################################################################
    ''' # Applies a simple statistical  analysis to the data set indata #
    ''' # in the intervall [ni, nf] and returns the following values:   #
    ''' #                                                               #
    ''' #  avg :    average y                                           #
    ''' #  sd  :    standard deviation sigma_y                          #
    ''' #  err :    mean error err_y (standard deviation of average)    #
    ''' #                                                               #
    ''' #################################################################
    ''' </summary>
    Public Shared Function Statistics1D(ByRef InColumnY As Double(),
                                        ByRef InColumnYOriginal As Double(),
                                        Optional ByVal ni As Integer = -1,
                                        Optional ByVal nf As Integer = -1) As sNumericStatistics
        ' check initial and final value of the data range to analyze
        If ni < 0 Then ni = 0
        If ni > InColumnY.Count - 1 Then ni = 0
        If nf < 0 Then nf = InColumnY.Count - 1
        If nf > InColumnY.Count - 1 Then ni = InColumnY.Count - 1
        If ni > nf Then
            Dim t As Integer = nf
            nf = ni
            ni = t
        End If

        ' Check integrity of data.
        Dim N As Integer = nf - ni
        If N < 0 Then
            Throw New Exception("Column-Length is zero. Statistical data could not be recieved.")
        End If

        ' Variables for saving the result
        Dim Mean As Double = 0
        Dim StandardDeviation As Double = 0
        Dim Variance As Double = 0
        Dim MeanError As Double = 0
        Dim CoefficientOfDetermination_R2 As Double = 0

        ' Calculate the sum of all Y-values
        ' and the sum of all squared Y-values
        Dim dSumY As Double = 0
        Dim dSumYSq As Double = 0

        ' First Pass
        For i As Integer = ni To nf Step 1
            If Double.IsNaN(InColumnY(i)) Or Double.IsInfinity(InColumnY(i)) Then Continue For
            dSumY += InColumnY(i)
            dSumYSq += InColumnY(i) * InColumnY(i)
        Next

        ' Calculate the average from the sum of all Y
        Mean = dSumY / N

        ' Second Pass, calculate Sum Of Squares
        Dim TotalSumOfSquares As Double = 0
        Dim ResidualSumOfSquares As Double = 0
        For i As Integer = ni To nf Step 1
            If Double.IsNaN(InColumnY(i)) Or Double.IsInfinity(InColumnY(i)) Then Continue For
            TotalSumOfSquares += ((InColumnY(i) - Mean) * (InColumnY(i) - Mean))
            If Not InColumnYOriginal Is Nothing Then
                ResidualSumOfSquares += ((InColumnY(i) - InColumnYOriginal(i)) * (InColumnY(i) - InColumnYOriginal(i)))
            End If
        Next

        ' Save the variance
        Variance = TotalSumOfSquares / N

        ' Save the Coefficient of Determination R^2
        CoefficientOfDetermination_R2 = 1 - ResidualSumOfSquares / TotalSumOfSquares

        ' Calculate the Standard-Deviation by the variance
        ' Allow only positive Standard-Deviation -> otherwise set to zero.
        If Variance > 0 Then
            StandardDeviation = Math.Sqrt(Variance)
        Else
            StandardDeviation = 0D
        End If

        ' Calculate the mean-error, which is the standard-deviation of the average value
        MeanError = Variance / N

        Return New sNumericStatistics(Mean,
                                      StandardDeviation,
                                      Variance,
                                      MeanError,
                                      CoefficientOfDetermination_R2,
                                      ResidualSumOfSquares,
                                      TotalSumOfSquares)
    End Function
    Public Shared Function Statistics1D(ByRef InColumnY As ICollection(Of Double),
                                        Optional ByVal ni As Integer = -1,
                                        Optional ByVal nf As Integer = -1) As sNumericStatistics
        Return Statistics1D(InColumnY.ToArray, Nothing, ni, nf)
    End Function
    Public Shared Function Statistics1D(ByRef InColumnY As ICollection(Of Double),
                                        ByRef InColumnYOriginal As List(Of Double),
                                        Optional ByVal ni As Integer = -1,
                                        Optional ByVal nf As Integer = -1) As sNumericStatistics
        Return Statistics1D(InColumnY.ToArray, InColumnYOriginal.ToArray, ni, nf)
    End Function

    ''' <summary>
    ''' Returns the average values for the column depending on the input columns.
    ''' </summary>
    Public Shared Function AverageDataColumnsFromMultipleFiles(ByVal List As IEnumerable(Of ICollection(Of Double))) As List(Of Double)
        Dim Out As New List(Of Double)
        Dim ValuesPerPoint As Integer = List.Count
        If ValuesPerPoint = 0 Then Return Out
        If ValuesPerPoint = 1 Then Return List(0).ToList

        ' Check, if all Lists have the same length
        For Each DataList As List(Of Double) In List
            If DataList.Count <> List(0).Count Then
                Throw New Exception("Lists do not have the same number of points!")
            End If
        Next

        ' Average Data
        For i As Integer = 0 To List(0).Count - 1 Step 1
            Dim dSum As Double = 0D
            Dim ValueCount As Integer = 0
            For Each DataList As List(Of Double) In List
                If Not Double.IsNaN(DataList(i)) Then
                    ValueCount += 1
                    dSum += DataList(i)
                End If
            Next
            Out.Add(dSum / ValueCount)
        Next

        Return Out
    End Function

    ''' <summary>
    ''' Returns the average Value of a list of Values:
    ''' </summary>
    Public Shared Function AverageValues(ByRef Values As List(Of Double)) As Double
        Dim AverageValue As Double = 0
        For Each Val As Double In Values
            AverageValue += Val
        Next
        If Values.Count > 0 Then
            AverageValue /= Values.Count
        End If
        Return AverageValue
    End Function

    ''' <summary>
    ''' Returns the Maximum Value of a list of Values:
    ''' </summary>
    Public Shared Function GetMaximumValue(ByRef Values As List(Of Double)) As Double
        Dim MaxVal As Double = Double.MinValue
        For Each Val As Double In Values
            If Not Double.IsNaN(Val) Then
                If Val > MaxVal Then MaxVal = Val
            End If
        Next
        Return MaxVal
    End Function

    ''' <summary>
    ''' Returns the Maximum Value of a list of Values:
    ''' </summary>
    Public Shared Function GetMaximumValue(ByRef Values As Double()) As Double
        Dim MaxVal As Double = Double.MinValue
        For Each Val As Double In Values
            If Not Double.IsNaN(Val) Then
                If Val > MaxVal Then MaxVal = Val
            End If
        Next
        Return MaxVal
    End Function

    ''' <summary>
    ''' Returns the Minimum Value of a list of Values:
    ''' </summary>
    Public Shared Function GetMinimumValue(ByRef Values As List(Of Double)) As Double
        Dim MinVal As Double = Double.MaxValue
        For Each Val As Double In Values
            If Not Double.IsNaN(Val) Then
                If Val < MinVal Then MinVal = Val
            End If
        Next
        Return MinVal
    End Function

    ''' <summary>
    ''' Returns the Minimum Value of a list of Values:
    ''' </summary>
    Public Shared Function GetMinimumValue(ByRef Values As Double()) As Double
        Dim MinVal As Double = Double.MaxValue
        For Each Val As Double In Values
            If Not Double.IsNaN(Val) Then
                If Val < MinVal Then MinVal = Val
            End If
        Next
        Return MinVal
    End Function

    ''' <summary>
    ''' Returns the Maximum/Minimum in a double-matrix
    ''' </summary>
    Public Shared Function GetMaximumValue(ByRef Matrix As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix) As Double
        Return GetMaximumValue(Matrix.Values)
        'Dim Value As Double = Double.MinValue
        'For Y As Integer = 0 To Matrix.RowCount - 1 Step 1
        '    For X As Integer = 0 To Matrix.ColumnCount - 1 Step 1
        '        If Not Double.IsNaN(Matrix(Y, X)) Then
        '            If Matrix(Y, X) > Value Then Value = Matrix(Y, X)
        '        End If
        '    Next
        'Next
        'Return Value
    End Function

    ''' <summary>
    ''' Returns the Maximum/Minimum in a double-matrix
    ''' </summary>
    Public Shared Function GetMinimumValue(ByRef Matrix As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix) As Double
        Return GetMinimumValue(Matrix.Values)

        'Dim Value As Double = Double.MaxValue
        'For Y As Integer = 0 To Matrix.RowCount - 1 Step 1
        '    For X As Integer = 0 To Matrix.ColumnCount - 1 Step 1
        '        If Not Double.IsNaN(Matrix(Y, X)) Then
        '            If Matrix(Y, X) < Value Then Value = Matrix(Y, X)
        '        End If
        '    Next
        'Next
        'Return Value
    End Function
End Class
