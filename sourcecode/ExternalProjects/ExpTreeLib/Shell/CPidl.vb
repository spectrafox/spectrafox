Imports ExpTreeLib.CShItem
Imports ExpTreeLib.ShellDll
Imports System.Runtime.InteropServices
Imports System.Text


'''<summary>cPidl class contains a Byte() representation of a PIDL and
''' certain Methods and Properties for comparing one cPidl to another</summary>
Public Class CPidl
    Implements IEnumerable

#Region "       Private Fields"
    Private m_bytes() As Byte   'The local copy of the PIDL
    Private m_ItemCount As Integer      'the # of ItemIDs in this ItemIDList (PIDL)
    Private m_OffsetToRelative As Integer 'the index of the start of the last itemID in m_bytes
#End Region

#Region "       Constructor"
    ''' <summary>
    ''' Given an IntPtr pointing to a valid PIDL, copy the bytes of that PIDL to a Byte()
    ''' </summary>
    ''' <param name="Pidl">IntPtr pointing to a valid PIDL</param>
    Sub New(ByVal Pidl As IntPtr)
        Dim cb As Integer = ItemIDListSize(Pidl)
        If cb > 0 Then
            ReDim m_bytes(cb + 1)
            Marshal.Copy(Pidl, m_bytes, 0, cb)
            'DumpPidl(pidl)
        Else
            ReDim m_bytes(1)  'This is the DeskTop (we hope)
        End If
        'ensure nulnul
        m_bytes(m_bytes.Length - 2) = 0 : m_bytes(m_bytes.Length - 1) = 0
        m_ItemCount = PidlCount(Pidl)
    End Sub
#End Region

#Region "       Public Properties"
    ''' <summary>
    ''' Returns this cPIDL's Byte() containing the Bytes of the original PIDL
    ''' </summary>
    ''' <returns>This cPIDL's Byte() containing the Bytes of the original PIDL</returns>
    Public ReadOnly Property PidlBytes() As Byte()
        Get
            Return m_bytes
        End Get
    End Property

    ''' <summary>
    ''' Returns the number of Bytes in this cPidl
    ''' </summary>
    ''' <returns>The number of Bytes in this cPidl</returns>
    Public ReadOnly Property Length() As Integer
        Get
            Return m_bytes.Length
        End Get
    End Property

    ''' <summary>
    ''' Returns the number of Item IDs in this instance
    ''' </summary>
    ''' <returns>The number of Item IDs in this cPidl</returns>
    Public ReadOnly Property ItemCount() As Integer
        Get
            Return m_ItemCount
        End Get
    End Property

#End Region

#Region "       Public Instance Methods -- ToPIDL, Decompose, and IsEqual"

    '''<summary> Copy the contents of a byte() containing a PIDL to
    ''' CoTaskMemory, returning an IntPtr that points to that mem block
    ''' Assumes that this cPidl is properly terminated, as all New 
    ''' cPidls are.
    '''</summary>
    ''' <returns>The newly created PIDL</returns>
    ''' <remarks> Caller must Free the returned IntPtr when done with the returned PIDL.</remarks>
    Public Function ToPIDL() As IntPtr
        ToPIDL = BytesToPidl(m_bytes)
    End Function

    ''' <summary>
    ''' Returns an object containing a byte() for each of this cPidl's
    ''' ITEMIDs (individual PIDLS), in order such that obj(0) is
    ''' a byte() containing the bytes of the first ITEMID, etc.
    ''' Each ITEMID is properly terminated with a nulnul    ''' </summary>
    ''' <returns>An Object containing a Byte() for each ID in the PIDL</returns>
    ''' <remarks></remarks>
    Public Function Decompose() As Object()
        Dim bArrays(Me.ItemCount - 1) As Object
        Dim eByte As ICPidlEnumerator = CType(Me.GetEnumerator(), ICPidlEnumerator)
        Dim i As Integer
        Do While eByte.MoveNext
            bArrays(i) = eByte.Current
            i += 1
        Loop
        Return bArrays
    End Function

    ''' <summary>
    ''' Returns True if input cPidl's content exactly match the 
    ''' contents of this instance, False otherwise
    ''' </summary>
    ''' <param name="other">The CPidl to compare to this instance</param>
    ''' <returns>True if "other" is Equal to this instance, False otherwise</returns>
    Public Function IsEqual(ByVal other As CPidl) As Boolean
        IsEqual = False     'assume not
        If other.Length <> Me.Length Then Exit Function
        Dim ob() As Byte = other.PidlBytes
        Dim i As Integer
        For i = 0 To Me.Length - 1  'note: we look at nulnul also
            If ob(i) <> m_bytes(i) Then Exit Function
        Next
        Return True         'all equal on fall thru
    End Function
#End Region

#Region "       Public Shared Methods"

#Region "           JoinPidlBytes"
    '''<summary> Join two byte arrays containing PIDLS. 
    ''' Returns NOTHING if error
    ''' </summary>
    ''' <returns>A Byte() containing the resultant ITEMIDLIST.</returns>
    ''' <remarks>Both Byte() must be properly terminated (nulnul)</remarks>
    Public Shared Function JoinPidlBytes(ByVal b1() As Byte, ByVal b2() As Byte) As Byte()
        If IsValidPidl(b1) And IsValidPidl(b2) Then
            Dim b(b1.Length + b2.Length - 3) As Byte 'allow for leaving off first nulnul
            Array.Copy(b1, b, b1.Length - 2)
            Array.Copy(b2, 0, b, b1.Length - 2, b2.Length)
            If IsValidPidl(b) Then
                Return b
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "           BytesToPidl"
    ''' <summary>
    ''' Copy the contents of a byte() containing a pidl to
    '''  CoTaskMemory, returning an IntPtr that points to that mem block
    ''' Caller must free the IntPtr when done with it
    ''' </summary>
    ''' <param name="b">A Byte() containing a valid PIDL</param>
    ''' <returns>An IntPtr pointing to the newly allocated and created PIDL</returns>
    ''' <remarks>Caller is responsible for Freeing the PIDL when no longer required</remarks>
    Public Shared Function BytesToPidl(ByVal b() As Byte) As IntPtr
        BytesToPidl = IntPtr.Zero       'assume failure
        If IsValidPidl(b) Then
            Dim bLen As Integer = b.Length
            BytesToPidl = Marshal.AllocCoTaskMem(bLen)
            If BytesToPidl.Equals(IntPtr.Zero) Then Exit Function 'another bad error
            Marshal.Copy(b, 0, BytesToPidl, bLen)
        End If
    End Function
#End Region

#Region "           StartsWith"
    '''<summary>returns True if the beginning of pidlA matches PidlB exactly for pidlB's entire length</summary>
    ''' <returns>True if the beginning of pidlA matches PidlB exactly for pidlB's entire length</returns>
    Public Shared Function StartsWith(ByVal pidlA As IntPtr, ByVal pidlB As IntPtr) As Boolean
        Return CPidl.StartsWith(New CPidl(pidlA), New CPidl(pidlB))
    End Function

    '''<summary>returns True if the beginning of A matches B exactly for B's entire length</summary>
    ''' <returns>True if the beginning of A matches B exactly for pidlB's entire length</returns>
    Public Shared Function StartsWith(ByVal A As CPidl, ByVal B As CPidl) As Boolean
        Return A.StartsWith(B)
    End Function

    '''<summary>Returns true if the CPidl input parameter exactly matches the
    ''' beginning of this instance of CPidl</summary>
    ''' <returns>True if the CPidl input parameter exactly matches the
    ''' beginning of this instance of CPidl</returns>
    Public Function StartsWith(ByVal cp As CPidl) As Boolean
        Dim b() As Byte = cp.PidlBytes
        If b.Length > m_bytes.Length Then Return False 'input is longer
        Dim i As Integer
        For i = 0 To b.Length - 3 'allow for nulnul at end of cp.PidlBytes
            If b(i) <> m_bytes(i) Then Return False
        Next
        Return True
    End Function
#End Region

#End Region

#Region "       GetEnumerator"
    ''' <summary>
    ''' Obtains a new Enumerator for this cPidl
    ''' </summary>
    ''' <returns>a new Enumerator for this cPidl</returns>
    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return New ICPidlEnumerator(m_bytes)
    End Function
#End Region

#Region "       CPIDL enumerator Class"
    Private Class ICPidlEnumerator
        Implements IEnumerator

        Private m_sPos As Integer   'the first index in the current PIDL
        Private m_ePos As Integer   'the last index in the current PIDL
        Private m_bytes() As Byte   'the local copy of the PIDL
        Private m_NotEmpty As Boolean = False 'the desktop PIDL is zero length

        ''' <summary>
        ''' Creates a New instance of ICPidlEnumerator
        ''' </summary>
        ''' <param name="b">A Byte() containing a valid PIDL</param>
        Sub New(ByVal b() As Byte)
            m_bytes = b
            If b.Length > 0 Then m_NotEmpty = True
            m_sPos = -1 : m_ePos = -1
        End Sub

        ''' <summary>
        ''' Returns the Byte() containing the Current Item ID
        ''' </summary>
        ''' <returns>Current ID as Byte()</returns>
        Public ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
            Get
                If m_sPos < 0 Then Throw New InvalidOperationException("ICPidlEnumerator --- attempt to get Current with invalidated list")
                Dim b((m_ePos - m_sPos) + 2) As Byte    'room for nulnul
                Array.Copy(m_bytes, m_sPos, b, 0, b.Length - 2)
                b(b.Length - 2) = 0 : b(b.Length - 1) = 0 'add nulnul
                Return b
            End Get
        End Property

        ''' <summary>
        ''' Moves the Current pointer to the Next Item ID in this cPidl
        ''' </summary>
        ''' <returns>True if successful, False if there is no Next Item ID</returns>
        ''' <remarks></remarks>
        Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
            If m_NotEmpty Then
                If m_sPos < 0 Then
                    m_sPos = 0 : m_ePos = -1
                Else
                    m_sPos = m_ePos + 1
                End If
                If m_bytes.Length < m_sPos + 1 Then Throw New InvalidCastException("Malformed PIDL")
                Dim cb As Integer = m_bytes(m_sPos) + m_bytes(m_sPos + 1) * 256
                If cb = 0 Then
                    Return False 'have passed all back
                Else
                    m_ePos += cb
                End If
            Else
                m_sPos = 0 : m_ePos = 0
                Return False        'in this case, we have exhausted the list of 0 ITEMIDs
            End If
            Return True
        End Function

        ''' <summary>
        ''' Resets the Current pointer to the beginning of this cPidl
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Reset() Implements System.Collections.IEnumerator.Reset
            m_sPos = -1 : m_ePos = -1
        End Sub
    End Class
#End Region

End Class