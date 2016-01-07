Imports System
Imports System.Runtime.InteropServices

Namespace ShellDll
    <ComImportAttribute(), _
      InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown), _
      Guid("0000010B-0000-0000-C000-000000000046")> _
  Public Interface IPersistFile

        'Inheirited from Ipersist
        Sub GetClassID( _
          <Out()> ByRef pClassID As Guid)

        'IPersistFile Interfaces
        <PreserveSig()> _
        Function IsDirty() As Integer

        Function Load( _
          <MarshalAs(UnmanagedType.LPWStr)> ByVal pszFileName As String, _
          ByVal dwMode As Integer) As Integer

        Function Save( _
          <MarshalAs(UnmanagedType.LPWStr)> ByVal pszFileName As String, _
          <MarshalAs(UnmanagedType.Bool)> ByVal fRemember As Boolean) As Integer

        Function SaveCompleted( _
          <MarshalAs(UnmanagedType.LPWStr)> ByVal pszFileName As String) As Integer

        Function GetCurFile( _
          <Out(), MarshalAs(UnmanagedType.LPWStr)> ByRef ppszFileName As String) As Integer

    End Interface
End Namespace