Partial Public Class cNumericalMethods
    ''' <summary>
    ''' Calculates the Numerical Derivative of the given X and Y Column.
    ''' At each position of the X Column.
    ''' </summary>
    Public Shared Function NumericalDerivative(ByVal InColumnX As ICollection(Of Double),
                                               ByVal InColumnY As ICollection(Of Double)) As List(Of Double)
        ' Determine the Length of the DataColumns:
        Dim LastIndex As Integer = InColumnY.Count - 1

        ' Create Out-Data-Lists.
        Dim OutY As New List(Of Double)(InColumnX.Count)

        ' For the First and Last Point: only one-sided derivative
        ' First Element: One-Sided Derivative
        OutY.Add((InColumnY(1) - InColumnY(0)) / (InColumnX(1) - InColumnX(0)))

        ' Calculate Two-Sided derivative for all other Points
        ' using the new Parallelization For-Loop, that uses all Threads
        ' of the running Computer.
        Dim dicParallelOutYList As New Dictionary(Of Integer, Double)
        'Parallel.For(1, LastIndex, Sub(i As Integer)
        '                               dicParallelOutYList.Add(i,
        '                                                       0.5 * (
        '                                                            (InColumnY(i + 1) - InColumnY(i)) / (InColumnX(i + 1) - InColumnX(i)) +
        '                                                            (InColumnY(i) - InColumnY(i - 1)) / (InColumnX(i) - InColumnX(i - 1)))
        '                                                              )
        '                           End Sub)
        For i As Integer = 1 To LastIndex - 1 Step 1
            dicParallelOutYList.Add(i,
                                0.5 * (
                                     (InColumnY(i + 1) - InColumnY(i)) / (InColumnX(i + 1) - InColumnX(i)) +
                                     (InColumnY(i) - InColumnY(i - 1)) / (InColumnX(i) - InColumnX(i - 1)))
                                       )
        Next

        ' Copy Elements from Calculated Dictionary to Out-List
        For i As Integer = 1 To LastIndex - 1 Step 1
            OutY.Add(dicParallelOutYList(i))
        Next

        ' Last Element: One-Sided Derivative
        OutY.Add((InColumnY(LastIndex) - InColumnY(LastIndex - 1)) / (InColumnX(LastIndex) - InColumnX(LastIndex - 1)))

        ' Return Data
        Return OutY
    End Function

    ''' <summary>
    ''' Calculates the Numerical Derivative of the given X and Y Column.
    ''' Returns a KeyValuePair with Key=X, Value=Y.
    ''' </summary>
    Public Shared Function NumericalDerivative(ByRef InDataColumnX As cSpectroscopyTable.DataColumn,
                                               ByRef InDataColumnY As cSpectroscopyTable.DataColumn) As List(Of Double)
        Return cNumericalMethods.NumericalDerivative(InDataColumnX.Values, InDataColumnY.Values)
    End Function

End Class
