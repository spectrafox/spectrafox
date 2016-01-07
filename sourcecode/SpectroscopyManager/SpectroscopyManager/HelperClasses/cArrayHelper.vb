Public Class cArrayHelper

    ''' <summary>
    ''' Compares two arrays of Double.
    ''' </summary>
    Public Shared Function AreArraysTheSame(ByRef Array1 As Double(), ByRef Array2 As Double()) As Boolean
        If Array1 Is Nothing Or Array2 Is Nothing Then Return False
        If Array1.Length <> Array2.Length Then Return False
        For i As Integer = 0 To Array1.Length - 1 Step 1
            If Array1(i) <> Array2(i) Then Return False
        Next
        Return True
    End Function

    ''' <summary>
    ''' Compares two arrays of Integer.
    ''' </summary>
    Public Shared Function AreArraysTheSame(ByRef Array1 As Integer(), ByRef Array2 As Integer()) As Boolean
        If Array1 Is Nothing Or Array2 Is Nothing Then Return False
        If Array1.Length <> Array2.Length Then Return False
        For i As Integer = 0 To Array1.Length - 1 Step 1
            If Array1(i) <> Array2(i) Then Return False
        Next
        Return True
    End Function

End Class
