Imports ExpTreeLib.CShItem
''' <summary>
''' Provides a Synchronized wrapper for a Strongly Typed Arraylist of CShItems. 
''' </summary>
''' <remarks></remarks>
Friend Class CShItemCollection
    Implements IEnumerable
    Implements ICollection

    Private m_items As ArrayList
    Private m_item As CShItem

    Friend Sub New(ByVal item As CShItem)
        m_item = item
        Dim m_tmp As New ArrayList()
        m_items = ArrayList.Synchronized(m_tmp)
    End Sub

    Public ReadOnly Property CShItem() As CShItem
        Get
            Return m_item
        End Get
    End Property

    Public ReadOnly Property Syncroot() As Object Implements ICollection.SyncRoot
        Get
            Return m_items.SyncRoot
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements ICollection.IsSynchronized
        Get
            Return m_items.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property Count() As Integer Implements ICollection.Count
        Get
            Return m_items.Count
        End Get
    End Property

    Public Sub Sort()
        m_items.Sort()
    End Sub

    Friend Function Add(ByVal value As CShItem) As Integer
        If value.Parent Is Nothing Then
            value.SetParent(m_item)
        End If
        Return m_items.Add(value)
    End Function

    Friend Sub AddRange(ByVal value As ICollection)
        m_items.AddRange(value)
    End Sub

    Friend Sub Clear()
        m_items.Clear()
    End Sub

    Public Function Contains(ByVal value As CShItem) As Boolean
        Return m_items.Contains(value)
    End Function

    Public Function Contains(ByVal name As String) As Boolean
        Dim itm As CShItem
        For Each itm In Me
            If String.Compare(itm.GetFileName, name, True) = 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function Contains(ByVal pidl As IntPtr) As Boolean
        Dim itm As CShItem
        'DumpPidl(pidl)
        For Each itm In Me
            If (IsEqual(itm.PIDL, pidl)) Then
                Return True
            Else
                'DumpPidl(itm.PIDL)
            End If
        Next
        Return False
    End Function

    Public Function IndexOf(ByVal value As CShItem) As Integer
        Return m_items.IndexOf(value)
    End Function

    Public Function IndexOf(ByVal name As String) As Integer
        Dim i As Integer
        For i = 0 To m_items.Count - 1
            If String.Compare(DirectCast(m_items(i), CShItem).GetFileName, _
                name, True) = 0 Then
                Return i
            End If
        Next
        Return -1
    End Function

    Public Function IndexOf(ByVal pidl As IntPtr) As Integer
        Dim i As Integer
        For i = 0 To m_items.Count - 1
            If IsEqual(DirectCast(m_items(i), CShItem).PIDL, pidl) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Friend Sub Insert(ByVal index As Integer, ByVal value As CShItem)
        m_items.Insert(index, value)
    End Sub

    Friend Sub Remove(ByVal value As CShItem)
        m_items.Remove(value)
    End Sub

    Friend Sub Remove(ByVal name As String)
        Dim index As Integer = IndexOf(name)

        If index > -1 Then
            RemoveAt(index)
        End If
    End Sub

    Friend Sub RemoveAt(ByVal index As Integer)
        m_items.RemoveAt(index)
    End Sub

    Default Public ReadOnly Property Item(ByVal index As Integer) As CShItem
        Get
            Return DirectCast(m_items(index), CShItem)
        End Get
    End Property

    Default Public Property Item(ByVal name As String) As CShItem
        Get
            Dim index As Integer = IndexOf(name)
            If index > -1 Then
                Return DirectCast(m_items(index), CShItem)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As CShItem)
            Dim index As Integer = IndexOf(name)
            If index > -1 Then
                m_items(index) = value
            End If
        End Set
    End Property

    Default Public Property Item(ByVal pidl As IntPtr) As CShItem
        Get
            Dim index As Integer = IndexOf(pidl)
            If index > -1 Then
                Return DirectCast(m_items(index), CShItem)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As CShItem)
            Dim index As Integer = IndexOf(pidl)
            If index > -1 Then
                m_items(index) = value
            End If
        End Set
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return m_items.GetEnumerator
    End Function
    ''' <summary>
    ''' Copys all CShItems contained in this instance to an Array (of CShItems), starting at the supplied
    ''' index into the Array.
    ''' </summary>
    ''' <param name="array">CShItem Array to copy to.</param>
    ''' <param name="index">Index into array to copy the first instance of CShItem.</param>
    ''' <remarks>Is Thread save.</remarks>
    Public Sub CopyTo(ByVal array As Array, ByVal index As Integer) Implements ICollection.CopyTo
        SyncLock m_items.SyncRoot
            m_items.CopyTo(array, index)
        End SyncLock
    End Sub
    ''' <summary>
    ''' Returns all CShItems contained in this instance.
    ''' </summary>
    ''' <returns>An Array of CShItems</returns>
    ''' <remarks>Is Thread safe.</remarks>
    Public Function ToArray() As CShItem()
        SyncLock m_items.SyncRoot
            Return m_items.ToArray(GetType(CShItem))
        End SyncLock
    End Function
End Class