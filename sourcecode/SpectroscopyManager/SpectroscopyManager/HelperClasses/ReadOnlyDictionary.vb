Public Interface IReadOnlyDictionary(Of TKey, TValue)
    Inherits IEnumerable

    Function ContainsKey(key As TKey) As Boolean
    ReadOnly Property Keys() As ICollection(Of TKey)
    ReadOnly Property Values() As ICollection(Of TValue)
    ReadOnly Property Count() As Integer
    Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean
    Default ReadOnly Property Item(key As TKey) As TValue
    Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean
    Sub CopyTo(array As KeyValuePair(Of TKey, TValue)(), arrayIndex As Integer)
    Shadows Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue))
End Interface

Public Class ReadOnlyDictionary(Of TKey, TValue)
    Implements IReadOnlyDictionary(Of TKey, TValue)
    ReadOnly _dictionary As IDictionary(Of TKey, TValue)
    Public Sub New(dictionary As IDictionary(Of TKey, TValue))
        _dictionary = dictionary
    End Sub
    Public Function ContainsKey(key As TKey) As Boolean Implements IReadOnlyDictionary(Of TKey, TValue).ContainsKey
        Return _dictionary.ContainsKey(key)
    End Function
    Public ReadOnly Property Keys() As ICollection(Of TKey) Implements IReadOnlyDictionary(Of TKey, TValue).Keys
        Get
            Return _dictionary.Keys
        End Get
    End Property
    Public Function TryGetValue(key As TKey, ByRef value As TValue) As Boolean Implements IReadOnlyDictionary(Of TKey, TValue).TryGetValue
        Return _dictionary.TryGetValue(key, value)
    End Function
    Public ReadOnly Property Values() As ICollection(Of TValue) Implements IReadOnlyDictionary(Of TKey, TValue).Values
        Get
            Return _dictionary.Values
        End Get
    End Property
    Default Public ReadOnly Property Item(key As TKey) As TValue Implements IReadOnlyDictionary(Of TKey, TValue).Item
        Get
            Return _dictionary(key)
        End Get
    End Property
    Public Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements IReadOnlyDictionary(Of TKey, TValue).Contains
        Return _dictionary.Contains(item)
    End Function
    Public Sub CopyTo(array As KeyValuePair(Of TKey, TValue)(), arrayIndex As Integer) Implements IReadOnlyDictionary(Of TKey, TValue).CopyTo
        _dictionary.CopyTo(array, arrayIndex)
    End Sub
    Public ReadOnly Property Count() As Integer Implements IReadOnlyDictionary(Of TKey, TValue).Count
        Get
            Return _dictionary.Count
        End Get
    End Property
    Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IReadOnlyDictionary(Of TKey, TValue).GetEnumerator
        Return _dictionary.GetEnumerator()
    End Function
    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return _dictionary.GetEnumerator()
    End Function
End Class