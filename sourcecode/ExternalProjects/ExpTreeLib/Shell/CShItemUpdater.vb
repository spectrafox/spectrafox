Imports ExpTreeLib.CShItem
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Text


''' <summary>
''' CShItemUpdater provides the infrastructure that registers for and receives WM_Notify messages for all changes to the FileSystem and
''' Virtual Folders known to the local machine. It has knowledge of the internal CShItem cache. If a change affects that cache, 
''' it calls the appropriate CShItem routines to report these changes.
''' </summary>
''' <remarks>Only changes of interest to the CShItem internal cache are reported. All others are ignored.</remarks>
Friend Class CShItemUpdater
    Inherits Control
    Implements IDisposable

    Private m_notifyId As Integer

    Friend Sub New(ByVal itm As CShItem)
        MyBase.CreateHandle()
        'Subscribe to windows events        
        Dim entry As SHChangeNotifyEntry = New SHChangeNotifyEntry()
        entry.pIdl = itm.PIDL
        entry.Recursively = True
        m_notifyId = SHChangeNotifyRegister( _
               Me.Handle, _
               SHCNRF.InterruptLevel Or SHCNRF.ShellLevel Or SHCNRF.NewDelivery, _
               SHCNE.ALLEVENTS, _
               WM.USER + 200, _
               1, _
               New SHChangeNotifyEntry() {entry})
    End Sub

    Public Shadows Sub Dispose()
        If m_notifyId > 0 Then
            SHChangeNotifyDeregister(m_notifyId)
            MyBase.Dispose()
            GC.SuppressFinalize(Me)
        End If
    End Sub

#If DEBUG Then
    Dim counter As Integer

    Private Function EventDump(ByVal txtID As String, ByVal shNotify As SHNOTIFYSTRUCT, ByVal e As CShItemUpdateEventArgs, ByVal msgID As SHCNE) As Boolean
        EventDump = False
        Dim id As String = " -- Counter = " & e.Tag & " "
        Debug.WriteLine(txtID & id & [Enum].GetName(GetType(SHCNE), msgID))
        Dim csi1, csi2 As CShItem
        Dim parent1 As CShItem
        If shNotify.dwItem1 <> IntPtr.Zero Then     '5/26/2012
            csi1 = FindCShItem(shNotify.dwItem1)
            If csi1 IsNot Nothing Then
                '    If csi1.Path.IndexOf("ntuser.dat", StringComparison.InvariantCultureIgnoreCase) > -1 Then  '6/6/2012 - No longer needed
                '        Return True
                '    End If
                parent1 = csi1.Parent
                Debug.WriteLine(id & "dwItem1: " & " (" & shNotify.dwItem1.ToString & ")" & csi1.ItemPath)
                'DumpPidl(shNotify.dwItem1)
                If parent1 IsNot Nothing Then
                    Debug.WriteLine(id & "parent1: " & parent1.ItemPath)
                End If
            Else
                Debug.WriteLine(id & "dwItem1: " & " (" & shNotify.dwItem1.ToString & ")" & " Not Found")
                'DumpPidl(shNotify.dwItem1)
                If parent1 IsNot Nothing Then
                    Debug.WriteLine(id & "parent1: " & parent1.ItemPath)
                End If
            End If
        Else
            Debug.WriteLine(id & "dwItem1: Is Empty")
        End If
        If shNotify.dwItem2 <> IntPtr.Zero Then     '5/26/2012
            csi2 = FindCShItem(shNotify.dwItem2)    '5/26/2012
            If csi2 IsNot Nothing Then
                Debug.WriteLine(id & "dwItem2: " & " (" & shNotify.dwItem2.ToString & ")" & csi2.ItemPath)
            Else
                Debug.WriteLine(id & "dwItem2: " & " (" & shNotify.dwItem2.ToString & ")" & " Not Found")
            End If
        Else
            Debug.WriteLine(id & "dwItem2: Is Empty")
        End If

    End Function
#End If

    Private Function IsItemNotificationEvent(ByVal lEvent As SHCNE) As Boolean
        Return Not CBool(lEvent And (SHCNE.ASSOCCHANGED Or SHCNE.EXTENDED_EVENT Or SHCNE.FREESPACE Or SHCNE.DRIVEADDGUI Or SHCNE.SERVERDISCONNECT))
    End Function

    ''' <summary>
    ''' CShItemUpdater.WndProc processes WM.SH_NOTIFY messages requested by the SHChangeNotifyRegister 
    ''' API call in the CShItemUpdater constructor.
    ''' Messages are processed as follows:
    ''' 1.Folder/File Create or Delete: If Parent of Item is not in internal tree, ignore message. If
    ''' located, then add or remove the item from the internal tree, which raises an appropriate event to
    ''' notify interested controls.
    ''' 2.Folder/File Rename, Update, UpdateDir, MediaInserted, MediaRemoved: 
    ''' If Item itself is not in the internal tree, ignore message. 
    ''' If located, then call Item.Update for further processing. 
    ''' If appropriate, Item.Update will raise an appropriate event to notify
    ''' interested controls.
    ''' </summary>
    ''' <param name="m">A Windows Message</param>
    ''' <remarks>The use of SHGetRealIDL appears non-essential and wasteful. It is NOT.
    ''' SHGetRealIDL appears specifically designed for use in this situation, returning an 
    ''' Absolute real PIDL in CoTaskMemory. The pidls given in dwItem1 and dwItem2 are owned and
    ''' released by the Message Class. </remarks>
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg <> (WM.USER + 200) Then
            MyBase.WndProc(m)
            Exit Sub
        End If
        Dim ppidl As IntPtr
        Dim msgID As SHCNE
        Dim shNotify As SHNOTIFYSTRUCT
        Dim hLock As IntPtr = SHChangeNotification_Lock(m.WParam, m.LParam, ppidl, msgID)
        If hLock <> IntPtr.Zero AndAlso IsItemNotificationEvent(msgID) Then
            shNotify = CType(Marshal.PtrToStructure(ppidl, shNotify.GetType), SHNOTIFYSTRUCT)

#If DEBUG Then
            Dim UArgs As New CShItemUpdateEventArgs(shNotify, msgID, counter)
            'Debug.WriteLine("Enter WndProc -- Counter = " & UArgs.Tag & " - " & [Enum].GetName(GetType(SHCNE), CType(msgid, SHCNE)))
            'EventDump("Enter WndProc", shNotify, UArgs, msgID)
#End If
            ' In the below test, only UPDATEDIR will ever give me just the Desktop's PIDL - which will appear as an Empty PIDL to IsPidlEmpty
            'If (Not CShItem.IsPidlEmpty(shNotify.dwItem1)) OrElse (msgID = SHCNE.UPDATEDIR AndAlso shNotify.dwItem1 <> IntPtr.Zero) Then '5/21/2012
            If shNotify.dwItem1 <> IntPtr.Zero Then
                Select Case msgID
                    'Item Changes
                    Case SHCNE.CREATE
                        Dim parent, child, realRel As IntPtr
                        parent = CShItem.TrimPidl(shNotify.dwItem1, child)
                        Dim parentItem As CShItem = CShItem.FindCShItem(parent)
                        If Not IsNothing(parentItem) Then
                            If parentItem.FilesInitialized AndAlso Not parentItem.FileList.Contains(shNotify.dwItem1) Then
                                If SHGetRealIDL(parentItem.Folder, child, realRel) = S_OK Then
                                    Dim newItem As New CShItem(realRel, parentItem)
                                    If newItem IsNot Nothing Then parentItem.AddItem(newItem)
                                End If
                                Marshal.FreeCoTaskMem(realRel)
                            Else
                                'Debug.WriteLine(vbTab & "Files not init or already in")
                                'DumpPidl(parent)
                            End If
                        Else
                            'Debug.WriteLine(vbTab & "Parent not found")
                            'DumpPidl(parent)
                        End If
                        Marshal.FreeCoTaskMem(parent)
                        Marshal.FreeCoTaskMem(child)

                    Case SHCNE.DELETE
                        Dim parent, child As IntPtr
                        parent = CShItem.TrimPidl(shNotify.dwItem1, child)
                        Dim parentItem As CShItem
                        parentItem = CShItem.FindCShItem(parent)
                        If Not IsNothing(parentItem) Then
                            If parentItem.FilesInitialized AndAlso parentItem.FileList.Contains(shNotify.dwItem1) Then
                                Dim childItem As CShItem = parentItem.FileList(shNotify.dwItem1)
                                parentItem.RemoveItem(childItem)
                            End If
                        End If
                        Marshal.FreeCoTaskMem(parent)
                        Marshal.FreeCoTaskMem(child)

                    Case ShellAPI.SHCNE.RENAMEITEM
                        If shNotify.dwItem2 <> IntPtr.Zero Then     '5/26/2012
                            Dim item As CShItem = CShItem.FindCShItem(shNotify.dwItem1)
                            If item IsNot Nothing Then
                                item.Update(shNotify.dwItem2, CShItemUpdateType.Renamed)
                            End If
                        End If

                    Case ShellAPI.SHCNE.UPDATEITEM
                        Dim item As CShItem = FindCShItem(shNotify.dwItem1)
                        If Not item Is Nothing Then
                            'Debug.WriteLine("Item: " & item.ToString) 'Change made 9/21/2010
                            item.Update(IntPtr.Zero, CShItemUpdateType.Updated)
                            'item.Update(IntPtr.Zero, CShItemUpdateType.IconChange)
                        End If

                        ' Folder Changes
                    Case ShellAPI.SHCNE.MKDIR, ShellAPI.SHCNE.DRIVEADD
                        ' Make Directory
                        Dim parent, child, realRel As IntPtr
                        parent = TrimPidl(shNotify.dwItem1, child)
                        Dim parentItem As CShItem = FindCShItem(parent)
                        If Not parentItem Is Nothing Then
                            If parentItem.FoldersInitialized AndAlso (Not parentItem.DirectoryList.Contains(shNotify.dwItem1)) Then
                                If SHGetRealIDL(parentItem.Folder, child, realRel) = S_OK Then
                                    Dim newItem As New CShItem(realRel, parentItem)
                                    If newItem IsNot Nothing Then
                                        parentItem.AddItem(newItem)
                                        'Debug.WriteLine("MKDIR: " & newItem.Path)
                                    End If
                                Else
                                    Debug.WriteLine("***MKDIR - Failed on SHGetRealIDL " & parentItem.DisplayName)     '6/30/2012
                                End If
                                Marshal.FreeCoTaskMem(realRel)
                            ElseIf Not IsVistaOrAbove() Then  '6/27/2012 - XP will not send an UPDATEITEM for Parent in this case, so we have to
                                parentItem.Update(IntPtr.Zero, CShItemUpdateType.Updated)
                            End If
                        Else
                            Debug.WriteLine("***MKDIR - Parent Not Found")     '6/30/2012
                        End If
                        Marshal.FreeCoTaskMem(child)
                        Marshal.FreeCoTaskMem(parent)

                    Case ShellAPI.SHCNE.RENAMEFOLDER
                        'Renamed Directory
                        'If Not shNotify.dwItem2 <> IntPtr.Zero Then     '5/26/2012 - Old Code
                        If shNotify.dwItem2 <> IntPtr.Zero Then          '6/11/2012 - New Code
                            Dim item As CShItem = FindCShItem(shNotify.dwItem1)
                            If item IsNot Nothing Then
                                item.Update(shNotify.dwItem2, CShItemUpdateType.Renamed)
                            End If
                        End If

                    Case ShellAPI.SHCNE.RMDIR, ShellAPI.SHCNE.DRIVEREMOVED
                        'Removed Directory
                        Dim parent, child As IntPtr
                        parent = TrimPidl(shNotify.dwItem1, child)

                        Dim parentItem As CShItem = FindCShItem(parent)
                        If parentItem IsNot Nothing Then
                            'From Calum...sometimes when deleting a folder in My Documents 
                            'parentItem.DirectoryList was Nothing...
                            If parentItem.DirectoryList IsNot Nothing Then ' Added code from Calum
                                Dim indx As Integer = parentItem.DirectoryList.IndexOf(shNotify.dwItem1)
                                If indx > -1 Then
                                    parentItem.RemoveItem(parentItem.DirectoryList(indx))   '7/2/2012 - incorrectly used Directories
                                End If
                            ElseIf Not IsVistaOrAbove() Then  '6/27/2012 - XP will not send an UPDATEITEM for Parent in this case, so we have to
                                parentItem.Update(IntPtr.Zero, CShItemUpdateType.Updated)
                            End If
                        End If
                        Marshal.FreeCoTaskMem(child)
                        Marshal.FreeCoTaskMem(parent)

                    Case SHCNE.UPDATEDIR
                        Dim upCSI As CShItem = FindCShItem(shNotify.dwItem1)
                        If upCSI IsNot Nothing Then
                            upCSI.Update(Nothing, CShItemUpdateType.UpdateDir)
                        End If
                    Case SHCNE.MEDIAINSERTED, SHCNE.MEDIAREMOVED
                        Dim mediaCSI As CShItem = FindCShItem(shNotify.dwItem1)
                        If mediaCSI IsNot Nothing Then
                            mediaCSI.Update(Nothing, CShItemUpdateType.MediaChange)
                        End If
                    Case SHCNE.UPDATEIMAGE
                        Dim imgCSI As CShItem = FindCShItem(shNotify.dwItem1)
                        If imgCSI IsNot Nothing Then
                            imgCSI.Update(Nothing, CShItemUpdateType.IconChange)
                        End If
                End Select
            End If
XIT:
#If DEBUG Then
            'Debug.WriteLine("Leave WndProc -- Counter = " & UArgs.Tag)
            'EventDump("Leave WndProc", shNotify, UArgs, m)
#End If
            Dim tst As Boolean = SHChangeNotification_Unlock(hLock)
            If Not tst Then
                Debug.WriteLine("UnLock Failed " & hLock.ToString)
            End If
        End If
        MyBase.WndProc(m)
    End Sub

End Class

#If DEBUG Then
''' <summary>
''' CShItemUpdateEventArgs is only used for development. It provides a container for information used to track the handling of
''' WM_Notify messages.
''' </summary>
''' <remarks></remarks>
Friend Class CShItemUpdateEventArgs
    Inherits EventArgs

    Private m_shNotifyParams As SHNOTIFYSTRUCT

    Public ReadOnly Property NotifyParams() As SHNOTIFYSTRUCT
        Get
            Return m_shNotifyParams
        End Get
    End Property


    Private m_updateType As SHCNE
    Public ReadOnly Property UpdateType() As SHCNE
        Get
            Return m_updateType
        End Get
    End Property

    Private m_Tag As Integer
    Public ReadOnly Property Tag() As Integer
        Get
            Return m_Tag
        End Get
    End Property

    Friend Sub New(ByVal shNotifyParams As SHNOTIFYSTRUCT, ByVal updateType As SHCNE, ByRef tag As Integer)
        m_updateType = updateType
        m_shNotifyParams = shNotifyParams
        tag += 1
        m_Tag = tag
    End Sub
End Class
#End If