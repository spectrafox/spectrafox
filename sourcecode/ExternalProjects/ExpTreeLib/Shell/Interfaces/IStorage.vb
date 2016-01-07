Imports System.Runtime.InteropServices

Namespace ShellDll
    <ComImport(), Guid("0000000b-0000-0000-C000-000000000046"), _
       InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Public Interface IStorage
        <PreserveSig()> _
        Function CreateStream(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsName As String, ByVal grfMode As ShellAPI.STGM, ByVal reserved1 As Integer, ByVal reserved2 As Integer, <System.Runtime.InteropServices.Out()> ByRef ppstm As IntPtr) As Int32

        <PreserveSig()> _
        Function OpenStream(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsName As String, ByVal reserved1 As IntPtr, ByVal grfMode As ShellAPI.STGM, ByVal reserved2 As Integer, <System.Runtime.InteropServices.Out()> ByRef ppstm As IntPtr) As Int32

        <PreserveSig()> _
        Function CreateStorage(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsName As String, ByVal grfMode As ShellAPI.STGM, ByVal reserved1 As Integer, ByVal reserved2 As Integer, <System.Runtime.InteropServices.Out()> ByRef ppstg As IntPtr) As Int32

        <PreserveSig()> _
        Function OpenStorage(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsName As String, ByVal pstgPriority As IStorage, ByVal grfMode As ShellAPI.STGM, ByVal snbExclude As IntPtr, ByVal reserved As Integer, <System.Runtime.InteropServices.Out()> ByRef ppstg As IntPtr) As Int32

        <PreserveSig()> _
        Function CopyTo(ByVal ciidExclude As Integer, ByRef rgiidExclude As Guid, ByVal snbExclude As IntPtr, ByVal pstgDest As IStorage) As Int32

        <PreserveSig()> _
        Function MoveElementTo(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsName As String, ByVal pstgDest As IStorage, <MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsNewName As String, ByVal grfFlags As ShellAPI.STGMOVE) As Int32

        <PreserveSig()> _
        Function Commit(ByVal grfCommitFlags As ShellAPI.STGC) As Int32

        <PreserveSig()> _
        Function Revert() As Int32

        <PreserveSig()> _
        Function EnumElements(ByVal reserved1 As Integer, ByVal reserved2 As IntPtr, ByVal reserved3 As Integer, <System.Runtime.InteropServices.Out()> ByRef ppenum As IntPtr) As Int32

        <PreserveSig()> _
        Function DestroyElement(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsName As String) As Int32

        <PreserveSig()> _
        Function RenameElement(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsOldName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsNewName As String) As Int32

        <PreserveSig()> _
        Function SetElementTimes(<MarshalAs(UnmanagedType.LPWStr)> ByVal pwcsName As String, ByVal pctime As ShellAPI.FILETIME, ByVal patime As ShellAPI.FILETIME, ByVal pmtime As ShellAPI.FILETIME) As Int32

        <PreserveSig()> _
        Function SetClass(ByRef clsid As Guid) As Int32

        <PreserveSig()> _
        Function SetStateBits(ByVal grfStateBits As Integer, ByVal grfMask As Integer) As Int32

        <PreserveSig()> _
        Function Stat(<System.Runtime.InteropServices.Out()> ByRef pstatstg As ShellAPI.STATSTG, ByVal grfStatFlag As ShellAPI.STATFLAG) As Int32
    End Interface
End Namespace
