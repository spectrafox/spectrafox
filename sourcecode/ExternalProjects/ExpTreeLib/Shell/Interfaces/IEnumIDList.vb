Imports System
Imports System.Runtime.InteropServices

Namespace ShellDll
	<ComImportAttribute(), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F2-0000-0000-C000-000000000046")> _
	Public Interface IEnumIDList
		' Retrieves the specified number of item identifiers in the enumeration 
		' sequence and advances the current position by the number of items retrieved
		<PreserveSig()> _
		Function [Next](ByVal celt As Integer, <System.Runtime.InteropServices.Out()> ByRef rgelt As IntPtr, <System.Runtime.InteropServices.Out()> ByRef pceltFetched As Integer) As Int32

		' Skips over the specified number of elements in the enumeration sequence
		<PreserveSig()> _
		Function Skip(ByVal celt As Integer) As Int32

		' Returns to the beginning of the enumeration sequence
		<PreserveSig()> _
		Function Reset() As Int32

		' Creates a new item enumeration object with the same contents and state as the current one
		<PreserveSig()> _
		Function Clone(<System.Runtime.InteropServices.Out()> ByRef ppenum As IEnumIDList) As Int32
	End Interface
End Namespace