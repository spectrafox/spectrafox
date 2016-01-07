Imports System
Imports System.Runtime.InteropServices
Imports ExpTreeLib.ShellDll.ShellAPI

Namespace ShellDll
    <ComImportAttribute(), _
     InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown), _
     Guid("000214E6-0000-0000-C000-000000000046")> _
 Public Interface IShellFolder
        <PreserveSig()> _
        Function ParseDisplayName( _
            ByVal hwndOwner As Integer, _
            ByVal pbcReserved As IntPtr, _
            <MarshalAs(UnmanagedType.LPWStr)> _
            ByVal lpszDisplayName As String, _
            ByRef pchEaten As Integer, _
            ByRef ppidl As IntPtr, _
            ByRef pdwAttributes As Integer) As Integer

        <PreserveSig()> _
        Function EnumObjects( _
            ByVal hwndOwner As Integer, _
            <MarshalAs(UnmanagedType.U4)> ByVal _
            grfFlags As SHCONTF, _
            ByRef ppenumIDList As IEnumIDList) As Integer

        <PreserveSig()> _
        Function BindToObject( _
            ByVal pidl As IntPtr, _
            ByVal pbcReserved As IntPtr, _
            ByRef riid As Guid, _
            ByRef ppvOut As IntPtr) As Integer
        'IShellFolder) As Integer

        <PreserveSig()> _
        Function BindToStorage( _
            ByVal pidl As IntPtr, _
            ByVal pbcReserved As IntPtr, _
            ByRef riid As Guid, _
            ByVal ppvObj As IntPtr) As Integer

        <PreserveSig()> _
        Function CompareIDs( _
            ByVal lParam As UInt32, _
            ByVal pidl1 As IntPtr, _
            ByVal pidl2 As IntPtr) As Integer

        <PreserveSig()> _
        Function CreateViewObject( _
            ByVal hwndOwner As IntPtr, _
            ByRef riid As Guid, _
            ByRef ppvOut As IntPtr) As Integer
        'IUnknown) As Integer

        <PreserveSig()> _
        Function GetAttributesOf( _
            ByVal cidl As Integer, _
            <MarshalAs(UnmanagedType.LPArray, sizeparamindex:=0)> _
            ByVal apidl() As IntPtr, _
            ByRef rgfInOut As SFGAO) As Integer

        <PreserveSig()> _
        Function GetUIObjectOf( _
            ByVal hwndOwner As IntPtr, _
            ByVal cidl As Integer, _
            <MarshalAs(UnmanagedType.LPArray, sizeparamindex:=0)> _
            ByVal apidl() As IntPtr, _
            ByRef riid As Guid, _
            ByRef prgfInOut As Integer, _
            ByRef ppvOut As IntPtr) As Integer
        'ByRef ppvOut As IUnknown) As Integer
        'ByRef ppvOut As IDropTarget) As Integer

        <PreserveSig()> _
        Function GetDisplayNameOf( _
            ByVal pidl As IntPtr, _
            <MarshalAs(UnmanagedType.U4)> _
            ByVal uFlags As SHGDN, _
            ByVal lpName As IntPtr) As Integer

        <PreserveSig()> _
        Function SetNameOf( _
            ByVal hwndOwner As Integer, _
            ByVal pidl As IntPtr, _
            <MarshalAs(UnmanagedType.LPWStr)> ByVal _
            lpszName As String, _
            <MarshalAs(UnmanagedType.U4)> ByVal _
            uFlags As SHGDN, _
            ByRef ppidlOut As IntPtr) As Integer

    End Interface
End Namespace
