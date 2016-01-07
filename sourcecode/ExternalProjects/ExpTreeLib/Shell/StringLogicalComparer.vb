Imports System
Imports System.Collections

''' <summary>
''' Compares string such that strings containing numeric values will, assuming that non-numeric leading portions
''' are equal, will sort in numeric order. Specifically, the strings: "a1", "a101", "a3" will sort as:
''' "a1", "a3", "a101"
''' </summary>
''' <remarks><list type="bullet">
''' <item><description>Article, code, and forum additions are found at:</description></item>
''' <item><description>http://www.codeproject.com/cs/algorithms/csnsort.asp</description></item>
''' <item><description>Original C# code by Vasian Cepa</description></item>
''' <item><description>Optimized C# code by Richard Deeming</description></item>
''' <item><description>Translated to VB.Net by Mike Cattle</description></item>
''' <item><description>Corrected version of CompareNumbers by Jim Parsells</description></item>
''' </list>
''' </remarks>
Public NotInheritable Class StringLogicalComparer
    Implements IComparer
    Private Shared _default As StringLogicalComparer = New StringLogicalComparer()

    Private Sub New()
    End Sub 'New

    ''' <summary>
    ''' Returns an Instance of StringLogicalComparer
    ''' </summary>
    ''' <returns>an Instance of StringLogicalComparer</returns>
    Public Shared ReadOnly Property [Default]() As IComparer
        Get
            Return _default
        End Get
    End Property

    ''' <summary>
    ''' Compares two Objects which must be Strings. Allows for and Compares properly if one or both Strings are Nothing. 
    ''' <para>When given two initialized Strings, Compares them using 
    ''' <see cref="ExpTreeLib.StringLogicalComparer.CompareStrings">the CompareStrings function of this Class</see></para>
    ''' </summary>
    ''' <param name="x">First String to Compare</param>
    ''' <param name="y">Second String to Compare</param>
    ''' <returns>Negative value if x less than y, 0 if x=y, or a positive value if x greater than y</returns>
    ''' <remarks></remarks>
    Public Function [Compare](ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        If x Is Nothing AndAlso y Is Nothing Then
            Return 0
        End If
        If x Is Nothing Then
            Return -1
        End If
        If y Is Nothing Then
            Return 1
        End If
        If TypeOf x Is String AndAlso TypeOf y Is String Then
            Return CompareStrings(CStr(x), CStr(y))
        End If
        Return Comparer.Default.Compare(x, y)
    End Function 'Compare

    ''' <summary>
    ''' Compares string such that strings containing numeric values will, assuming that non-numeric leading portions
    ''' are equal, will sort in numeric order. Specifically, the strings: "a1", "a101", "a3" will sort as:
    ''' "a1", "a3", "a101"
    ''' </summary>
    ''' <param name="s1">First String to Compare</param>
    ''' <param name="s2">Second String to Compare</param>
    ''' <returns>Negative value if s1 less than s2, 0 if s1=s2, positive value if s1 greater than s2</returns>
    ''' <remarks>Note that negative return values may be other than -1 and that positive return values may be other than 1</remarks>
    Public Shared Function CompareStrings(ByVal s1 As String, ByVal s2 As String) As Integer
        If s1 Is Nothing OrElse s1.Length = 0 Then
            If s2 Is Nothing OrElse s2.Length = 0 Then
                Return 0
            End If
            Return -1
        ElseIf s2 Is Nothing OrElse s2.Length = 0 Then
            Return 1
        End If

        Dim s1Length As Integer = s1.Length
        Dim s2Length As Integer = s2.Length

        Dim sp1 As Boolean = Char.IsLetterOrDigit(s1(0))
        Dim sp2 As Boolean = Char.IsLetterOrDigit(s2(0))

        If sp1 AndAlso Not sp2 Then
            Return 1
        End If
        If Not sp1 AndAlso sp2 Then
            Return -1
        End If
        Dim c1, c2 As Char
        Dim i1 As Integer = 0
        Dim i2 As Integer = 0
        Dim r As Integer = 0
        Dim letter1, letter2 As Boolean

        While True
            c1 = s1(i1)
            c2 = s2(i2)

            sp1 = Char.IsDigit(c1)
            sp2 = Char.IsDigit(c2)

            If Not sp1 AndAlso Not sp2 Then
                If c1 <> c2 Then
                    letter1 = Char.IsLetter(c1)
                    letter2 = Char.IsLetter(c2)

                    If letter1 AndAlso letter2 Then
                        c1 = Char.ToUpper(c1)
                        c2 = Char.ToUpper(c2)

                        r = Asc(c1) - Asc(c2)
                        If 0 <> r Then
                            Return r
                        End If
                    ElseIf Not letter1 AndAlso Not letter2 Then
                        r = Asc(c1) - Asc(c2)
                        If 0 <> r Then
                            Return r
                        End If
                    ElseIf letter1 Then
                        Return 1
                    ElseIf letter2 Then
                        Return -1
                    End If
                End If

            ElseIf sp1 AndAlso sp2 Then
                r = CompareNumbers(s1, s1Length, i1, s2, s2Length, i2)
                If 0 <> r Then
                    Return r
                End If
            ElseIf sp1 Then
                Return -1
            ElseIf sp2 Then
                Return 1
            End If

            i1 += 1
            i2 += 1

            If i1 >= s1Length Then
                If i2 >= s2Length Then
                    Return 0
                End If
                Return -1
            ElseIf i2 >= s2Length Then
                Return 1
            End If
        End While
    End Function 'Compare

    Private Shared Function CompareNumbers(ByVal s1 As String, ByVal s1Length As Integer, ByRef i1 As Integer, ByVal s2 As String, ByVal s2Length As Integer, ByRef i2 As Integer) As Integer
        Dim nzStart1 As Integer = i1, nzStart2 As Integer = i2
        Dim end1 As Integer = i1, end2 As Integer = i2

        ScanNumber(s1, s1Length, i1, nzStart1, end1)
        ScanNumber(s2, s2Length, i2, nzStart2, end2)

        Dim start1 As Integer = i1
        i1 = end1 - 1
        Dim start2 As Integer = i2
        i2 = end2 - 1

        Dim length1 As Integer = end2 - nzStart2
        Dim length2 As Integer = end1 - nzStart1

        If length1 = length2 Then
            Dim r As Integer
            Dim j1 As Integer = nzStart1
            Dim j2 As Integer = nzStart2
            Do While j1 <= i1
                r = Convert.ToInt32(s1.Chars(j1)) - Convert.ToInt32(s2.Chars(j2))
                If 0 <> r Then
                    Return r
                End If
                j1 += 1
                j2 += 1
            Loop

            length1 = end1 - start1
            length2 = end2 - start2

            If length1 = length2 Then
                Return 0
            End If
        End If

        If length1 > length2 Then
            Return -1
        End If
        Return 1
    End Function

    Private Shared Sub ScanNumber(ByVal s As String, ByVal length As Integer, ByVal start As Integer, ByRef nzStart As Integer, ByRef [end] As Integer)
        nzStart = start
        [end] = start

        Dim countZeros As Boolean = True
        Dim c As Char = s([end])

        While True
            If countZeros Then
                If "0"c = c Then
                    nzStart += 1
                Else
                    countZeros = False
                End If
            End If

            [end] += 1
            If [end] >= length Then
                Exit While
            End If
            c = s([end])
            If Not Char.IsDigit(c) Then
                Exit While
            End If
        End While
    End Sub 'ScanNumber

End Class 'StringLogicalComparer