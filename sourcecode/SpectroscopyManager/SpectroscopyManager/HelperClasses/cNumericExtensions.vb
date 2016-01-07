''' <summary>
''' An extension class for the between operation
''' name pattern IsBetweenXX where X = I -> Inclusive, X = E -> Exclusive
''' </summary>
Public Module BetweenExtensions

    ''' <summary>
    ''' Between check <![CDATA[min <= value <= max]]> 
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value">the value to check</param>
    ''' <param name="min">Inclusive minimum border</param>
    ''' <param name="max">Inclusive maximum border</param>
    ''' <returns>return true if the value is between the min and max else false</returns>
    <System.Runtime.CompilerServices.Extension>
    Public Function IsBetweenII(Of T As IComparable)(value As T, min As T, max As T) As Boolean
        Return (min.CompareTo(value) <= 0) AndAlso (value.CompareTo(max) <= 0)
    End Function

    ''' <summary>
    ''' Between check <![CDATA[min <= value <= max]]>
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value">the value to check</param>
    ''' <param name="min">Exclusive minimum border</param>
    ''' <param name="max">Inclusive maximum border</param>
    ''' <returns>return true if the value is between the min and max else false</returns>
    <System.Runtime.CompilerServices.Extension>
    Public Function IsBetweenEI(Of T As IComparable)(value As T, min As T, max As T) As Boolean
        Return (min.CompareTo(value) < 0) AndAlso (value.CompareTo(max) <= 0)
    End Function

    ''' <summary>
    ''' between check <![CDATA[min <= value <= max]]>
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value">the value to check</param>
    ''' <param name="min">Inclusive minimum border</param>
    ''' <param name="max">Exclusive maximum border</param>
    ''' <returns>return true if the value is between the min and max else false</returns>
    <System.Runtime.CompilerServices.Extension>
    Public Function IsBetweenIE(Of T As IComparable)(value As T, min As T, max As T) As Boolean
        Return (min.CompareTo(value) <= 0) AndAlso (value.CompareTo(max) < 0)
    End Function

    ''' <summary>
    ''' between check <![CDATA[min <= value <= max]]>
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="value">the value to check</param>
    ''' <param name="min">Exclusive minimum border</param>
    ''' <param name="max">Exclusive maximum border</param>
    ''' <returns>return true if the value is between the min and max else false</returns>

    <System.Runtime.CompilerServices.Extension>
    Public Function IsBetweenEE(Of T As IComparable)(value As T, min As T, max As T) As Boolean
        Return (min.CompareTo(value) < 0) AndAlso (value.CompareTo(max) < 0)
    End Function
End Module

Public Module NumericExtensions

    ''' <summary>
    ''' Exchanges two values with each other.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension>
    Public Sub ExchangeValues(Of T)(ByRef value1 As T, ByRef value2 As T)
        Dim TMP As T = value2
        value2 = value1
        value1 = TMP
    End Sub

End Module