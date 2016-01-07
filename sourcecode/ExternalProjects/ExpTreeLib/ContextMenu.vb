Imports System.Runtime.InteropServices
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI

''' <summary>
''' WindowsContextMenu provides the infrastucture for displaying a Windows Context Menu on a Control
''' and for Invoking a Command selected by the user from that Context Menu. Cascaded sub-menus are created,
''' displayed, and responded to as required.
''' The Context Menu applies to all CShItems
''' passed to the ShowMenu Function and all CShItems must be in the same Folder. 
''' </summary>
''' <remarks>Though specifically designed for ListView and TreeView Controls, this Class will work for any Control which
'''          is associated with a single Folder and can provide CShItems from that Folder.</remarks>
Public Class WindowsContextMenu

    Public winMenu As IContextMenu = Nothing
    Public winMenu2 As IContextMenu2 = Nothing
    Public winMenu3 As IContextMenu3 = Nothing
    Public newMenu As IContextMenu = Nothing
    Public newMenu2 As IContextMenu2 = Nothing
    Public newMenu3 As IContextMenu3 = Nothing
    Public newMenuPtr As IntPtr = Nothing
    Private min As Integer = 1
    Private max As Integer = 100000

    ''' <summary>
    ''' If this method returns true then the caller must call ReleaseMenu
    ''' </summary>
    ''' <param name="hwnd">The handle to the control to host the ContextMenu</param>
    ''' <param name="items">The items for which to show the ContextMenu. These items must be in the same folder.</param>
    ''' <param name="pt">The point where the ContextMenu should appear</param>
    ''' <param name="allowrename">Set if the ContextMenu should contain the Rename command where appropriate</param>
    ''' <param name="cmi">The command information for the users selection</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ShowMenu(ByVal hwnd As IntPtr, ByVal items As CShItem(), ByVal pt As Drawing.Point, _
                ByVal allowRename As Boolean, <Out()> ByRef cmi As CMInvokeCommandInfoEx) As Boolean

        Debug.Assert(items.Length > 0)

        Dim comContextMenu As IntPtr = CreatePopupMenu()
        Dim pidls(items.Length - 1) As IntPtr
        Dim i As Integer
        Dim folder As ExpTreeLib.ShellDll.IShellFolder = Nothing

        If items(0) Is CShItem.GetDeskTop Then
            folder = items(0).Folder
        Else
            folder = items(0).Parent.Folder
        End If
        For i = 0 To items.Length - 1
            If Not items(i).CanRename Then allowrename = False
            pidls(i) = CShItem.ILFindLastID(items(i).PIDL)
        Next
        Dim prgf As Integer = 0
        Dim pIcontext As IntPtr
        Dim HR As Integer = folder.GetUIObjectOf(IntPtr.Zero, pidls.Length, pidls, IID_IContextMenu, prgf, pIcontext)

        If Not HR = S_OK Then
#If DEBUG Then
            Marshal.ThrowExceptionForHR(HR)
#End If
            GoTo FAIL
        End If

        winMenu = CType(Marshal.GetObjectForIUnknown(pIcontext), IContextMenu)

        Dim p As IntPtr = Nothing

        Marshal.QueryInterface(pIcontext, IID_IContextMenu2, p)
        If Not p = IntPtr.Zero Then
            winMenu2 = CType(Marshal.GetObjectForIUnknown(p), IContextMenu2)
        End If

        Marshal.QueryInterface(pIcontext, IID_IContextMenu3, p)
        If Not p = IntPtr.Zero Then
            winMenu3 = CType(Marshal.GetObjectForIUnknown(p), IContextMenu3)
        End If

        If Not pIcontext.Equals(IntPtr.Zero) Then
            pIcontext = IntPtr.Zero
        End If
        'Check item count - should always be 0 but check just in case
        Dim startIndex As Integer = GetMenuItemCount(comContextMenu.ToInt32)
        'Fill the context menu
        Dim flags As Integer = CMF.NORMAL Or CMF.EXPLORE
        If allowRename Then flags = flags Or CMF.CANRENAME
        If (Control.ModifierKeys And Keys.Shift) = Keys.Shift Then flags = flags Or CMF.EXTENDEDVERBS
        Dim idCount As Integer = winMenu.QueryContextMenu(comContextMenu, _
                                            startIndex, min, max, flags)
        Dim cmd As Integer = TrackPopupMenuEx(comContextMenu, TPM.RETURNCMD, pt.X, pt.Y, hwnd, IntPtr.Zero)

        If cmd >= min And cmd <= idCount Then
            cmi = New CMInvokeCommandInfoEx
            cmi.cbSize = Marshal.SizeOf(cmi)
            cmi.lpVerb = CType(cmd - min, IntPtr)
            cmi.lpVerbW = CType(cmd - min, IntPtr)
            cmi.nShow = SW.SHOWNORMAL
            cmi.fMask = CMIC.UNICODE Or CMIC.PTINVOKE
            cmi.ptInvoke = New System.Drawing.Point(pt.X, pt.Y)
            ShowMenu = True
        Else
FAIL:
            If winMenu IsNot Nothing Then
                Marshal.ReleaseComObject(winMenu)
                winMenu = Nothing
            End If
            ShowMenu = False
        End If
        If winMenu2 IsNot Nothing Then
            Marshal.ReleaseComObject(winMenu2)
            winMenu2 = Nothing
        End If
        If winMenu3 IsNot Nothing Then
            Marshal.ReleaseComObject(winMenu3)
            winMenu3 = Nothing
        End If
        If Not comContextMenu.Equals(IntPtr.Zero) Then
            Marshal.Release(comContextMenu)
            comContextMenu = Nothing
        End If
    End Function

    Public Sub ReleaseMenu()
        If winMenu IsNot Nothing Then
            Marshal.ReleaseComObject(winMenu)
            winMenu = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Invokes a specific command from an IContextMenu
    ''' </summary>
    ''' <param name="iContextMenu">the IContextMenu containing the item</param>
    ''' <param name="cmd">the index of the command to invoke</param>
    ''' <param name="parentDir">the parent directory from where to invoke</param>
    ''' <param name="ptInvoke">the point (in screen coördinates) from which to invoke</param>
    Public Sub InvokeCommand(ByVal iContextMenu As IContextMenu, ByVal cmd As UInteger, ByVal parentDir As String, ByVal ptInvoke As System.Drawing.Point)
        Dim invoke As ShellAPI.CMInvokeCommandInfoEx = New ShellAPI.CMInvokeCommandInfoEx()
        invoke.cbSize = ShellAPI.cbInvokeCommand
        invoke.lpVerb = CType(cmd, IntPtr)
        invoke.lpDirectory = parentDir
        invoke.lpVerbW = CType(cmd, IntPtr)
        invoke.lpDirectoryW = parentDir
        invoke.fMask = ShellAPI.CMIC.UNICODE Or ShellAPI.CMIC.PTINVOKE
        If (Control.ModifierKeys And Keys.Control) <> 0 Then invoke.fMask = invoke.fMask Or ShellAPI.CMIC.CONTROL_DOWN
        If (Control.ModifierKeys And Keys.Shift) <> 0 Then invoke.fMask = invoke.fMask Or ShellAPI.CMIC.SHIFT_DOWN
        invoke.ptInvoke = New System.Drawing.Point(ptInvoke.X, ptInvoke.Y)
        invoke.nShow = ShellAPI.SW.SHOWNORMAL

        iContextMenu.InvokeCommand(invoke)
    End Sub

    ''' <summary>
    ''' If this method returns true then the caller must call ReleaseNewMenu
    ''' </summary>
    ''' <param name="itm"></param>
    ''' <param name="contextMenu"></param>
    ''' <param name="index"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetUpNewMenu(ByVal itm As CShItem, ByVal contextMenu As IntPtr, ByVal index As Integer) As Boolean

        Dim HR As Integer
        Dim idCount As Integer
        newMenuPtr = Nothing
        HR = CoCreateInstance(CLSID_NewMenu, IntPtr.Zero, CLSCTX.INPROC_SERVER, IID_IContextMenu, newMenuPtr)
        If HR = S_OK Then
            newMenu = CType(Marshal.GetObjectForIUnknown(newMenuPtr), IContextMenu)

            Dim p As IntPtr = IntPtr.Zero
            Marshal.QueryInterface(newMenuPtr, IID_IContextMenu2, p)
            If Not p = IntPtr.Zero Then
                newMenu2 = CType(Marshal.GetObjectForIUnknown(p), IContextMenu2)
            End If

            Marshal.QueryInterface(newMenuPtr, IID_IContextMenu3, p)
            If Not p = IntPtr.Zero Then
                newMenu3 = CType(Marshal.GetObjectForIUnknown(p), IContextMenu3)
            End If

            If Not p.Equals(IntPtr.Zero) Then
                Marshal.Release(p)
                p = IntPtr.Zero
            End If

            Dim iShellExtInitPtr As IntPtr
            HR = (Marshal.QueryInterface(newMenuPtr, IID_IShellExtInit, iShellExtInitPtr))
            If HR = S_OK Then

                Dim shellExtInit As IShellExtInit = _
                    CType(Marshal.GetObjectForIUnknown(iShellExtInitPtr), IShellExtInit)

                shellExtInit.Initialize(itm.PIDL, IntPtr.Zero, IntPtr.Zero)

                Marshal.ReleaseComObject(shellExtInit)
                Marshal.Release(iShellExtInitPtr)
            End If
            If Not newMenuPtr.Equals(IntPtr.Zero) Then
                Marshal.Release(newMenuPtr)
                newMenuPtr = IntPtr.Zero
            End If
        End If

        If Not HR = S_OK Then
            ReleaseNewMenu()
#If DEBUG Then
            Marshal.ThrowExceptionForHR(HR)
#End If
            Return False
        End If

        idCount = newMenu.QueryContextMenu(contextMenu, _
                                             index, min, max, CMF.NORMAL)
        newMenuPtr = GetSubMenu(contextMenu, index)

        Return True

    End Function

    Public Sub ReleaseNewMenu()
        If newMenu IsNot Nothing Then
            Marshal.ReleaseComObject(newMenu)
            newMenu = Nothing
        End If
        If newMenu2 IsNot Nothing Then
            Marshal.ReleaseComObject(newMenu2)
            newMenu2 = Nothing
        End If
        If newMenu3 IsNot Nothing Then
            Marshal.ReleaseComObject(newMenu3)
            newMenu3 = Nothing
        End If
        If Not newMenuPtr.Equals(IntPtr.Zero) Then
            Marshal.Release(newMenuPtr)
            newMenuPtr = Nothing
        End If
    End Sub
End Class
