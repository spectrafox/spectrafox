Imports ExpTreeLib.ShellDll.ShellAPI
Imports System.Runtime.InteropServices
Namespace ShellDll

    <ComImport(), _
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
     GuidAttribute("000214e4-0000-0000-c000-000000000046")> _
    Public Interface IContextMenu
        ' IContextMenu methods

        <PreserveSig()> _
        Function QueryContextMenu(ByVal hmenu As IntPtr, _
        ByVal iMenu As Integer, _
        ByVal idCmdFirst As Integer, _
        ByVal idCmdLast As Integer, _
        ByVal uFlags As Integer) As Integer

        <PreserveSig()> _
        Function InvokeCommand(ByRef pici As CMInvokeCommandInfoEx) As Integer

        <PreserveSig()> _
       Function GetCommandString(ByVal idcmd As Integer, _
        ByVal uflags As Integer, _
        ByVal reserved As Integer, _
        <MarshalAs(UnmanagedType.LPArray)> ByVal commandstring As Byte(), _
        ByVal cch As Integer) As Integer

    End Interface
End Namespace