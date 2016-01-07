Imports ExpTreeLib.CShItem
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Text

Namespace ShellDll
    ''' <summary>
    ''' Contains a number of utility routines used by and with ExpTreeLib.
    ''' </summary>
    Public Class ShellHelper

#Region "       Low/High Word"

        ''' <summary>
        ''' Retrieves the High Word of a WParam of a WindowMessage
        ''' </summary>
        ''' <param name="ptr">The pointer to the WParam</param>
        ''' <returns>The unsigned integer for the High Word</returns>
        Public Shared Function HiWord(ByVal ptr As IntPtr) As UInteger
            If (CUInt(ptr) And &H80000000L) = &H80000000L Then
                Return (CUInt(ptr) >> 16)
            Else
                Return CUInt((CUInt(ptr) >> 16) And &HFFFF)
            End If
        End Function

        ''' <summary>
        ''' Retrieves the Low Word of a WParam of a WindowMessage
        ''' </summary>
        ''' <param name="ptr">The pointer to the WParam</param>
        ''' <returns>The unsigned integer for the Low Word</returns>
        Public Shared Function LoWord(ByVal ptr As IntPtr) As UInteger
            Return CUInt(CUInt(ptr) And &HFFFF)
        End Function

#End Region

#Region "       szToString"
        ''' <summary>
        ''' szToString accepts an array of bytes representing an Default Encoded string and
        ''' converts it to a .Net Unicode String.  szToString Truncates the String at the first
        ''' nul (0) byte in the input array.  
        ''' </summary>
        ''' <param name="arb">A Byte() to be translated using the Default codepage</param>
        ''' <param name="iPos">Start index in the Array - Defaults to 0</param>
        ''' <param name="len">Number of Bytes to translate - Defaults to entire Array</param>
        ''' <returns>A .Net String. If errors, returns the empty string ("")</returns>
        ''' <remarks></remarks>
        Public Shared Function szToString(ByVal arb() As Byte, _
                                        Optional ByVal ipos As Integer = 0, _
                                        Optional ByVal len As Integer = 0) As String
            Dim UB As Integer = arb.Length - 1
            If ipos > UB Then
                Return ""
            Else
                If (len = 0) Then len = UB - ipos + 1
                If (ipos + len > (UB + 1)) Then
                    Return ""
                Else
                    Dim i As Integer = ipos
                    Do While i < (ipos + len)
                        If arb(i) = 0 Then
                            len = i - ipos
                            Exit Do
                        End If
                        i += 1
                    Loop
                    Dim uChars As Char() = Encoding.Unicode.GetChars(Encoding.Convert(Encoding.Default, Encoding.Unicode, arb, ipos, len))
                    Return New String(uChars)
                End If
            End If

        End Function

#End Region

#Region "       IStream/IStorage"
        ''' <summary>
        ''' Obtains an IStream Interface for the input CShItem
        ''' </summary>
        ''' <param name="item">The CShItem for whom an IStream Interface is desired.</param>
        ''' <param name="streamPtr"></param>
        ''' <param name="stream">Returned Interface</param>
        ''' <returns>An IStream Interface for the input CShItem</returns>
        ''' <remarks>Not used by ExpTreeLib or its' Demo</remarks>
        Public Shared Function GetIStream(ByVal item As CShItem, <System.Runtime.InteropServices.Out()> ByRef streamPtr As IntPtr, <System.Runtime.InteropServices.Out()> ByRef stream As IStream) As Boolean
            If item.Parent.Folder.BindToStorage(CShItem.ILFindLastID(item.PIDL), IntPtr.Zero, ShellAPI.IID_IStream, streamPtr) = ShellAPI.S_OK Then
                stream = Marshal.GetTypedObjectForIUnknown(streamPtr, GetType(IStream))
                Return True
            Else
                stream = Nothing
                streamPtr = IntPtr.Zero
                Return False
            End If
        End Function
        ''' <summary>
        ''' Obtains an IStorage Interface for the input CShItem
        ''' </summary>
        ''' <param name="item">The CShItem for whom an IStorage Interface is desired.</param>
        ''' <param name="storagePtr"></param>
        ''' <param name="storage">Returned Interface</param>
        ''' <returns>An IStorage Interface for the input CShItem</returns>
        ''' <remarks>Not used by ExpTreeLib or its' Demo</remarks>

        Public Shared Function GetIStorage(ByVal item As CShItem, <System.Runtime.InteropServices.Out()> ByRef storagePtr As IntPtr, <System.Runtime.InteropServices.Out()> ByRef storage As IStorage) As Boolean
            If item.Parent.Folder.BindToStorage(CShItem.ILFindLastID(item.PIDL), IntPtr.Zero, ShellAPI.IID_IStorage, storagePtr) = ShellAPI.S_OK Then
                storage = CType(Marshal.GetTypedObjectForIUnknown(storagePtr, GetType(IStorage)), IStorage)
                Return True
            Else
                storage = Nothing
                storagePtr = IntPtr.Zero
                Return False
            End If
        End Function

#End Region

#Region "       GetIDropTarget"
        ''' <summary>
        ''' This method uses the GetUIObjectOf method of IShellFolder to obtain the IDropTarget of a
        ''' CShItem. 
        ''' </summary>
        ''' <param name="item">The item for which to obtain the IDropTarget</param>
        ''' <param name="dropTarget">The IDropTarget interface of the input Folder</param>
        ''' <returns>True if successful in obtaining the IDropTarget Interface.</returns>
        ''' <remarks>The original FileBrowser version of this returned the IntPtr which points to
        ''' the interface. This is not needed since GetTypedObjectForIUnknown manages that IntPtr.
        ''' For all purposes, the CShItem.GetDropTargetOf routine is more efficient and provides
        ''' the same interface.</remarks>
        Public Shared Function GetIDropTarget(ByVal item As CShItem, <Out()> ByRef dropTarget As ShellDll.IDropTarget) As Boolean
            Dim dropTargetPtr As IntPtr
            Dim parent As CShItem = item.Parent
            If IsNothing(parent) Then parent = item
            Dim folder As IShellFolder
            If item Is CShItem.GetDeskTop Then
                folder = item.Folder
            Else
                folder = item.Parent.Folder
            End If
            Dim relpidl As IntPtr = CShItem.ILFindLastID(item.PIDL)
            If (parent.Folder.GetUIObjectOf(IntPtr.Zero, 1, New IntPtr() {relpidl}, ShellDll.ShellAPI.IID_IDropTarget, _
                    0, dropTargetPtr) = 0) Then
                dropTarget = Marshal.GetTypedObjectForIUnknown(dropTargetPtr, GetType(ShellDll.IDropTarget))
                Return True
            Else
                dropTarget = Nothing
                dropTargetPtr = IntPtr.Zero
                Return False
            End If
        End Function

#End Region

#Region "       GetIDataObject"
        ''' <summary>
        ''' This method will use the GetUIObjectOf method of IShellFolder to obtain the IDataObject of a
        ''' ShellItem. 
        ''' </summary>
        ''' <param name="items">An array of CShItem for which to obtain the IDataObject</param>
        ''' <returns>the IDataObject the ShellItem</returns>
        ''' <remarks>All CShItems in the array are ASSUMED to have the same parent folder.</remarks>
        Public Shared Function GetIDataObject(ByVal items As CShItem()) As IntPtr
            Dim parent As CShItem
            If Not items(0).Parent Is Nothing Then
                parent = items(0).Parent
            Else
                parent = items(0)
            End If

            Dim pidls(items.Length - 1) As IntPtr
            Dim i As Integer = 0
            Do While i < items.Length
                pidls(i) = CShItem.ILFindLastID(items(i).PIDL)
                i += 1
            Loop

            Dim dataObjectPtr As IntPtr
            If parent.Folder.GetUIObjectOf(IntPtr.Zero, CUInt(pidls.Length), pidls, ShellDll.ShellAPI.IID_IDataObject, IntPtr.Zero, dataObjectPtr) = ShellDll.ShellAPI.S_OK Then
                Return dataObjectPtr
            Else
                Return IntPtr.Zero
            End If
        End Function
#End Region

#Region "       GetIDropTargetHelper"
        ''' <summary>
        ''' Obtains an IDropTargetHelper Interface
        ''' </summary>
        ''' <param name="helperPtr">Returns a pointer to the Interface</param>
        ''' <param name="dropHelper">Returns the Interface itself.</param>
        ''' <returns>True if successful, False otherwise.</returns>
        ''' <remarks>This interface is used by drop targets to enable the drag-image manager to display the drag image while the image is over the target window. </remarks>
        Public Shared Function GetIDropTargetHelper(<System.Runtime.InteropServices.Out()> ByRef helperPtr As IntPtr, <System.Runtime.InteropServices.Out()> ByRef dropHelper As IDropTargetHelper) As Boolean
            If ShellAPI.CoCreateInstance(ShellAPI.CLSID_DragDropHelper, IntPtr.Zero, ShellAPI.CLSCTX.INPROC_SERVER, ShellAPI.IID_IDropTargetHelper, helperPtr) = ShellAPI.S_OK Then
                dropHelper = CType(Marshal.GetTypedObjectForIUnknown(helperPtr, GetType(IDropTargetHelper)), IDropTargetHelper)
                Return True
            Else
                dropHelper = Nothing
                helperPtr = IntPtr.Zero
                Return False
            End If
        End Function
#End Region

#Region "       CanDropClipboard"
        ''' <summary>
        '''  It obtains a DragDropEffects flag variable indicating the input CShItem's ability to accept a Paste from the Clipboard.
        ''' </summary>
        ''' <param name="item">The item whose ability to accept a Paste is to be queried.</param>
        ''' <returns>A DragDropEffect indicating what actions the input CShItem is willing to do.</returns>
        ''' <remarks>Used to determine if Paste is a valid menu item.</remarks>
        Public Shared Function CanDropClipboard(ByVal item As CShItem) As DragDropEffects
            Dim dataObject As IntPtr
            ShellAPI.OleGetClipboard(dataObject)

            Dim target As ShellDll.IDropTarget = Nothing

            Dim retVal As DragDropEffects = DragDropEffects.None
            If GetIDropTarget(item, target) Then

                Dim effects As DragDropEffects = DragDropEffects.Copy
                If target.DragEnter(dataObject, ShellAPI.MK.CONTROL, New ShellAPI.POINT(0, 0), effects) = ShellAPI.S_OK Then
                    If effects = DragDropEffects.Copy Then
                        retVal = retVal Or DragDropEffects.Copy
                    End If

                    target.DragLeave()
                End If

                effects = DragDropEffects.Move
                If target.DragEnter(dataObject, ShellAPI.MK.SHIFT, New ShellAPI.POINT(0, 0), effects) = ShellAPI.S_OK Then
                    If effects = DragDropEffects.Move Then
                        retVal = retVal Or DragDropEffects.Move
                    End If

                    target.DragLeave()
                End If

                effects = DragDropEffects.Link
                If target.DragEnter(dataObject, ShellAPI.MK.ALT, New ShellAPI.POINT(0, 0), effects) = ShellAPI.S_OK Then
                    If effects = DragDropEffects.Link Then
                        retVal = retVal Or DragDropEffects.Link
                    End If

                    target.DragLeave()
                End If

                Marshal.ReleaseComObject(target)
            End If

            Return retVal
        End Function
#End Region

#Region "       QueryInfo"
        ''' <summary>
        ''' Obtains an IQueryInfo Interface for the input CShItem.
        ''' </summary>
        ''' <param name="item">The Item to obtain the Interface for.</param>
        ''' <param name="iQueryInfoPtr">The pointer to the obtained Interface</param>
        ''' <param name="iQueryInfo">The actual Interface</param>
        ''' <returns>True if successful, False otherwise.</returns>
        ''' <remarks>Not used by ExpTree or its' Demo.</remarks>
        Public Shared Function GetIQueryInfo(ByVal item As CShItem, _
        <System.Runtime.InteropServices.Out()> ByRef iQueryInfoPtr As IntPtr, _
        <System.Runtime.InteropServices.Out()> ByRef iQueryInfo As IQueryInfo) As Boolean
            Dim parent As CShItem
            If Not item.Parent Is Nothing Then
                parent = item.Parent
            Else
                parent = item
            End If

            If parent.Folder.GetUIObjectOf(IntPtr.Zero, 1, New IntPtr() {CShItem.ILFindLastID(item.PIDL)}, ShellAPI.IID_IQueryInfo, IntPtr.Zero, iQueryInfoPtr) = ShellAPI.S_OK Then
                iQueryInfo = CType(Marshal.GetTypedObjectForIUnknown(iQueryInfoPtr, GetType(IQueryInfo)), IQueryInfo)
                Return True
            Else
                iQueryInfo = Nothing
                iQueryInfoPtr = IntPtr.Zero
                Return False
            End If
        End Function

#End Region

#Region "       Make Shell ID Array (CIDA)"
        '''<summary>
        ''' Shell Folders prefer their IDragData to contain this format which is
        '''  NOT directly supported by .Net.  The underlying structure is the CIDA structure
        '''  which is basically VB, VB.Net, and C# Hostile.
        '''If "Make ShortCut(s) here" is the desired or
        '''  POSSIBLE effect of the drag, then this format is REQUIRED -- otherwise the
        '''  Folder will interpret the DragDropEffects.Link to be "Create Document Shortcut"
        '''  which is NEVER the desired effect in this case
        ''' The normal CIDA contains the Absolute PIDL of the source Folder and 
        '''  Relative PIDLs for each Item in the Drag. 
        '''  I cheat a bit an provide the Absolute PIDL of the Desktop (00, a short)
        '''  and the Absolute PIDLs for the Items (all such Absolute PIDLS ar 
        '''  relative to the Desktop.
        '''</summary>
        ''' <param name="CSIList">An ArrayList of CShItems to be included in the CIDA MemoryStream</param>
        ''' <returns>A MemoryStream which contains a CIDA containing the PIDLs of all Items in CSIList</returns>
        '''<remark>
        '''  <para>The overall concept and much code taken from</para>
        ''' http://www.dotnetmonster.com/Uwe/Forum.aspx/dotnet-interop/3482/Drag-and-Drop
        ''' <para>Dave Anderson's response, translated from C# to VB.Net, was the basis
        ''' of this routine</para>
        ''' <para>An AHA momemnt and a ref to the above url came from</para>
        '''http://www.Planet-Source-Code.com/vb/scripts/ShowCode.asp?txtCodeId=61324%26lngWId=1
        '''</remark>
        Public Shared Function MakeShellIDArray(ByVal CSIList As ArrayList) As System.IO.MemoryStream
            'ensure that we have an arraylist of only CShItems
            Dim AllowedType As Type = GetType(CShItem)
            Dim oCSI As Object
            For Each oCSI In CSIList
                If Not AllowedType.Equals(oCSI.GetType) Then
                    Return Nothing
                End If
            Next
            'ensure at least one item
            If CSIList.Count < 1 Then Return Nothing

            'bArrays is an Array of Byte() each containing the bytes of a PIDL
            Dim bArrays(CSIList.Count - 1) As Object
            Dim CSI As CShItem
            Dim i As Integer = 0
            For Each CSI In CSIList
                bArrays(i) = New CPidl(CSI.PIDL).PidlBytes
                i += 1
            Next

            MakeShellIDArray = New System.IO.MemoryStream()
            Dim BW As New System.IO.BinaryWriter(MakeShellIDArray)

            BW.Write(Convert.ToUInt32(CSIList.Count))   'we don't count the parent (Desktop)
            Dim Desktop As Integer  'we only use the lowval 2 bytes (VB lacks meaninful uint)
            Dim Offset As Integer   'offset into Structure of a PIDL

            ' Calculate and write the offset to each pidl (defined as an array of uint32)
            ' The first pidl is 2 bytes long (0 0) and represents the desktop
            ' The 2 in the statement below is for the offset to the 
            ' folder pidl and the count field in the CIDA structure
            Offset = Marshal.SizeOf(GetType(UInt32)) * (bArrays.Length + 2)
            BW.Write(Convert.ToUInt32(Offset))       'offset to desktop pidl
            Offset += 2 'Marshal.SizeOf(GetType(UInt16)) 'point to the next one
            For i = 0 To bArrays.Length - 1
                BW.Write(Convert.ToUInt32(Offset))
                Offset += CType(bArrays(i), Byte()).Length
            Next
            'done with the array of offsets, write the parent pidl (0 0) = Desktop
            BW.Write(Convert.ToUInt16(Desktop))

            'Write the pidl bytes
            Dim b() As Byte
            For Each b In bArrays
                BW.Write(b)
            Next

            'done, returning the memorystream
            'Debug.WriteLine("Done MakeShellIDArray")
        End Function
#End Region

#Region "       MakeDragListFromPtr "

        '''<summary>Builds a List of the CShItems being dragged from m_StreamCIDA</summary>
        ''' <param name="ptr">IntPtr pointing to a CIDA</param>
        '''<returns>A List of the CShItems being dragged or nothing on failure</returns>
        Friend Shared Function MakeDragListFromPtr(ByVal ptr As IntPtr) As List(Of CShItem)
            Dim streamCIDA As IO.MemoryStream = MakeStreamFromCIDA(ptr)
            Dim BR As New IO.BinaryReader(streamCIDA)
            Dim offsets(BR.ReadInt32 + 1) As Integer   '0=parent, last = total length
            offsets(offsets.Length - 1) = CInt(BR.BaseStream.Length)
            Dim i As Integer
            For i = 0 To offsets.Length - 2
                offsets(i) = BR.ReadInt32
            Next
            Dim bArrays(offsets.Length - 2) As Object   'my objects are byte()
            For i = 0 To bArrays.Length - 1
                Dim thisLen As Integer = offsets(i + 1) - offsets(i)
                bArrays(i) = BR.ReadBytes(thisLen)
            Next
            MakeDragListFromPtr = New List(Of CShItem)
            For i = 1 To bArrays.Length - 1
                Dim isOK As Boolean = True
                Try   'if GetCShitem returns Nothing(it's failure marker) then catch it
                    MakeDragListFromPtr.Add(GetCShItem(bArrays(0), bArrays(i)))
                Catch ex As Exception
                    Debug.Write("Error in making CShiTem from CIDA: " & ex.ToString)
                    isOK = False
                End Try
                If Not isOK Then GoTo ERRXIT
            Next
            'on fall thru, all is done OK
            Exit Function

            'Error cleanup and Exit
ERRXIT:     MakeDragListFromPtr = New List(Of CShItem)
            Debug.WriteLine("MakeDragListFromCIDA failed")
        End Function

        '''<summary>Given an IntPtr pointing to a CIDA,
        ''' copy the CIDA to a new MemoryStream</summary>
        Private Shared Function MakeStreamFromCIDA(ByVal ptr As IntPtr) As IO.MemoryStream
            MakeStreamFromCIDA = Nothing    'assume failure
            If ptr.Equals(IntPtr.Zero) Then Exit Function
            Dim nrItems As Integer = Marshal.ReadInt32(ptr, 0)
            If Not (nrItems > 0) Then Exit Function
            Dim offsets(nrItems) As Integer
            Dim curB As Integer = 4 'already read first 4
            Dim i As Integer
            For i = 0 To nrItems
                offsets(i) = Marshal.ReadInt32(ptr, curB)
                curB += 4
            Next
            Dim pidlLen As Integer
            Dim pidlobjs(nrItems) As Object
            For i = 0 To nrItems
                Dim ipt As New IntPtr(ptr.ToInt32 + offsets(i))
                Dim cp As New CPidl(ipt)
                pidlobjs(i) = cp.PidlBytes
                pidlLen += CType(pidlobjs(i), Byte()).Length
            Next
            MakeStreamFromCIDA = New IO.MemoryStream(pidlLen + (4 * offsets.Length) + 4)
            Dim BW As New IO.BinaryWriter(MakeStreamFromCIDA)
            With BW
                .Write(nrItems)
                For i = 0 To nrItems
                    .Write(offsets(i))
                Next
                For i = 0 To nrItems
                    .Write(CType(pidlobjs(i), Byte()))
                Next
            End With
            ' DumpHex(MakeStreamFromCIDA.ToArray)
            MakeStreamFromCIDA.Seek(0, IO.SeekOrigin.Begin)
        End Function

#End Region

#Region "       DataObjectContainsCShItems "

        ''' <summary>
        ''' Determines if input ShellDll.IDataObject will provide a Shell IDList Array (CIDA).
        ''' </summary>
        ''' <param name="dataObj">The ShellDll.IDataObject to be queried.</param>
        ''' <returns>True if ShellDll.IDataObject will provide a CIDA.</returns>
        ''' <remarks>Normally not needed.</remarks>
        Public Shared Function DataObjectContainsCShItems(ByVal dataObj As ShellDll.IDataObject) As Boolean
            Dim fmtEtc As New FORMATETC
            Dim cf As Integer = RegisterClipboardFormat("Shell IDList Array")
            If cf <> 0 Then
                With fmtEtc
                    .cfFormat = CType(cf, CF)
                    .lindex = -1
                    .dwAspect = DVASPECT.CONTENT
                    .ptd = IntPtr.Zero
                    .Tymd = TYMED.HGLOBAL
                End With

                Dim hr As Integer = dataObj.QueryGetData(fmtEtc)
                If hr = S_OK Then
                    Return True
                End If
            End If
            Return False
        End Function
#End Region

#Region "       GetCShItemsFromDataObject "

        ''' <summary>
        ''' Given an IDataObject, return a list of CShItems corresponding to the PIDLs in
        ''' the Shell IDList Array (CIDA) contained in the IDataObject.
        ''' </summary>
        ''' <param name="dataObj">A well formed ShellDll.IDataObject from which to extract the CShItems.</param>
        ''' <returns>List(Of CShItems) with all CShItems represented by the PIDLs in the CIDA.</returns>
        ''' <remarks>Used by ExplorerControls for standalone ExpList.</remarks>
        Public Shared Function GetCShItemsFromDataObject(ByVal dataObj As ShellDll.IDataObject) As List(Of CShItem)
            Dim items As New List(Of CShItem)
            Dim fmtEtc As FORMATETC
            Dim stg As STGMEDIUM

            Dim cf As Integer = RegisterClipboardFormat("Shell IDList Array")
            If cf <> 0 Then
                With fmtEtc
                    .cfFormat = CType(cf, CF)
                    .lindex = -1
                    .dwAspect = DVASPECT.CONTENT
                    .ptd = IntPtr.Zero
                    .Tymd = TYMED.HGLOBAL
                End With
                With stg
                    .hGlobal = IntPtr.Zero
                    .pUnkForRelease = IntPtr.Zero
                    .tymed = TYMED.HGLOBAL
                End With
                Dim HR As Integer = dataObj.GetData(fmtEtc, stg)
#If DEBUG Then
                If HR < 0 Then Marshal.ThrowExceptionForHR(HR)
#End If
                items = ShellHelper.MakeDragListFromPtr(GlobalLock(stg.hGlobal))
                GlobalUnlock(stg.hGlobal)
                ReleaseStgMedium(stg)       'done with this
                Return items
            End If
            Return Nothing
        End Function
#End Region

    End Class
End Namespace

