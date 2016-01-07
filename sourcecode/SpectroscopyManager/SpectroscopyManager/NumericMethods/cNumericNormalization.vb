Partial Public Class cNumericalMethods

    ''' <summary>
    ''' Normalizes the Given Data using a given Value as Reference.
    ''' </summary>
    Public Shared Function Normalize(ByRef InColumn As ICollection(Of Double),
                                     ByVal NormalizationReference As Double) As List(Of Double)

        ' Catch division by 0
        If NormalizationReference = 0 Then Return InColumn.ToList

        Dim OutY As New List(Of Double)(InColumn.Count)

        For i As Integer = 0 To InColumn.Count - 1 Step 1
            If Double.IsNaN(InColumn(i)) Then
                OutY.Add(Double.NaN)
            Else
                OutY.Add(InColumn(i) / NormalizationReference)
            End If
        Next

        Return OutY
    End Function
End Class
