Partial Public Class cNumericalMethods

    ''' <summary>
    ''' Averages the List of Columns to a single Data-Column.
    ''' </summary>
    Public Shared Function AverageColumns(ByRef ListOfColumns As List(Of ICollection(Of Double))) As List(Of Double)

        Dim ListOfColumnsCount As Integer = ListOfColumns.Count

        ' Check for Empty List.
        If ListOfColumnsCount = 0 Then
            Throw New ArgumentException("Averaging failed: List of Columns is Empty.")
        End If

        ' Check same length of each data-column:
        For i As Integer = 0 To ListOfColumnsCount - 2 Step 1
            If ListOfColumns(i).Count <> ListOfColumns(i + 1).Count Then
                Throw New ArgumentException("Averaging failed: Given columns are not of the same length.")
            End If
        Next

        ' Average Data
        Dim AveragedData As New List(Of Double)
        For j As Integer = 0 To ListOfColumns(0).Count - 1 Step 1
            Dim dValue As Double = 0
            For i As Integer = 0 To ListOfColumnsCount - 1 Step 1
                If Not Double.IsNaN(ListOfColumns(i)(j)) Then
                    dValue += ListOfColumns(i)(j)
                Else
                    dValue = Double.NaN
                    Exit For
                End If
            Next
            AveragedData.Add(dValue / ListOfColumnsCount)
        Next
        Return AveragedData
    End Function

    ''' <summary>
    ''' Averages the List of Columns to a single Data-Column.
    ''' </summary>
    Public Shared Function AverageColumns(ByRef ListOfColumns As List(Of cSpectroscopyTable.DataColumn)) As List(Of Double)
        Dim ColumnDataList As New List(Of ICollection(Of Double))
        For i As Integer = 0 To ListOfColumns.Count - 1 Step 1
            ColumnDataList.Add(ListOfColumns(i).Values)
        Next
        Return AverageColumns(ColumnDataList)
    End Function
End Class
