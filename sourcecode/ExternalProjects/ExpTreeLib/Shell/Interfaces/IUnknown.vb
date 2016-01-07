Imports System
Imports System.Runtime.InteropServices

Namespace ShellDll

    'Not needed in .Net - use Marshal Class
    <ComImport(), Guid("00000000-0000-0000-C000-000000000046"), _
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
      Public Interface IUnknown
        <PreserveSig()> _
        Function QueryInterface(ByRef riid As Guid, ByRef pVoid As IntPtr) As Integer
        <PreserveSig()> _
         Function AddRef() As UInt32
        <PreserveSig()> _
       Function Release() As UInt32
    End Interface
End Namespace
