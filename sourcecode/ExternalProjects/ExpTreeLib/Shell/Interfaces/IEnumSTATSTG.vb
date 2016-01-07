Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace ShellDll
	<ComImport, Guid("0000000d-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IEnumSTATSTG
		' The user needs to allocate an STATSTG array whose size is celt.
		<PreserveSig> _
		Function [Next](ByVal celt As UInteger, <System.Runtime.InteropServices.Out(), MarshalAs(UnmanagedType.LPArray)> ByRef rgelt As ShellAPI.STATSTG(), <System.Runtime.InteropServices.Out()> ByRef pceltFetched As UInteger) As UInteger

		<PreserveSig> _
		Sub Skip(ByVal celt As UInteger)

		<PreserveSig> _
		Sub Reset()

		<PreserveSig> _
		Function Clone() As IEnumSTATSTG
	End Interface
End Namespace