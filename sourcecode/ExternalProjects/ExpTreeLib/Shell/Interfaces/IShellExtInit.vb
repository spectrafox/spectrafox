Imports System.Runtime.InteropServices

Namespace ShellDll
    <ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
                  GuidAttribute("000214e8-0000-0000-c000-000000000046")> _
    Public Interface IShellExtInit
        <PreserveSig()> _
        Function Initialize(ByVal pidlFolder As IntPtr, _
                            ByVal lpdobj As IntPtr, _
                            ByVal hKeyProgID As IntPtr) As Integer    'Treat all HANDLEs as IntPtr
    End Interface
End Namespace