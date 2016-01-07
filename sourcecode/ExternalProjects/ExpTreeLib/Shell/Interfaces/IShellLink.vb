Imports ExpTreeLib.ShellDll.ShellAPI
Imports System
Imports System.Runtime.InteropServices
Imports System.Text

Namespace ShellDll
    'We define the Ansi version since all Win OSs (95 thru XP) support it
    <ComImportAttribute(), _
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
    Guid("000214EE-0000-0000-C000-000000000046")> _
Public Interface IShellLink

        Function GetPath( _
          <Out(), MarshalAs(UnmanagedType.LPStr)> ByVal pszFile As StringBuilder, _
          ByVal cchMaxPath As Integer, _
          <Out()> ByRef pfd As WIN32_FIND_DATA, _
          ByVal fFlags As SLGP) As Integer

        Function GetIDList( _
          ByRef ppidl As IntPtr) As Integer

        Function SetIDList( _
          ByVal pidl As IntPtr) As Integer

        Function GetDescription( _
          <Out(), MarshalAs(UnmanagedType.LPStr)> ByVal pszName As StringBuilder, _
          ByVal cchMaxName As Integer) As Integer

        Function SetDescription( _
          <MarshalAs(UnmanagedType.LPStr)> ByVal pszName As String) As Integer

        Function GetWorkingDirectory( _
          <Out(), MarshalAs(UnmanagedType.LPStr)> ByVal pszDir As StringBuilder, _
          ByVal cchMaxPath As Integer) As Integer

        Function SetWorkingDirectory( _
          <MarshalAs(UnmanagedType.LPStr)> ByVal pszDir As String) As Integer

        Function GetArguments( _
          <Out(), MarshalAs(UnmanagedType.LPStr)> ByVal pszArgs As StringBuilder, _
          ByVal cchMaxPath As Integer) As Integer

        Function SetArguments( _
          <MarshalAs(UnmanagedType.LPStr)> ByVal pszArgs As String) As Integer

        Function GetHotkey( _
          ByRef pwHotkey As Short) As Integer

        Function SetHotkey( _
          ByVal wHotkey As Short) As Integer

        Function GetShowCmd( _
          ByRef piShowCmd As Integer) As Integer

        Function SetShowCmd( _
          ByVal iShowCmd As Integer) As Integer

        Function GetIconLocation( _
          <Out(), MarshalAs(UnmanagedType.LPStr)> _
          ByVal pszIconPath As StringBuilder, _
          ByVal cchIconPath As Integer, _
          ByRef piIcon As Integer) As Integer

        Function SetIconLocation( _
          <MarshalAs(UnmanagedType.LPStr)> _
          ByVal pszIconPath As String, _
          ByVal iIcon As Integer) As Integer

        Function SetRelativePath( _
          <MarshalAs(UnmanagedType.LPStr)> _
          ByVal pszPathRel As String, _
          ByVal dwReserved As Integer) As Integer

        Function Resolve( _
          ByVal hwnd As IntPtr, _
          ByVal fFlags As SLR) As Integer

        Function SetPath( _
          <MarshalAs(UnmanagedType.LPStr)> _
          ByVal pszFile As String) As Integer

    End Interface
End Namespace