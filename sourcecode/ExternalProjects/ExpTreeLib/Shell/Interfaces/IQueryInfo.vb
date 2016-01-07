Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace ShellDll
    <ComImport(), Guid("00021500-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
   Public Interface IQueryInfo
        <PreserveSig()> _
         Function GetInfoTip(ByVal dwFlags As ShellAPI.QITIPF, <System.Runtime.InteropServices.Out(), MarshalAs(UnmanagedType.LPWStr)> ByRef ppwszTip As String) As Int32

        <PreserveSig()> _
         Function GetInfoFlags(<System.Runtime.InteropServices.Out()> ByRef pdwFlags As IntPtr) As Int32
    End Interface
End Namespace