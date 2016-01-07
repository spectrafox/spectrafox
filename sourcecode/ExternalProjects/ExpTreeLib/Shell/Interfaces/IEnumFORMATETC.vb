Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace ShellDll
	<ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000103-0000-0000-C000-000000000046")> _
	Public Interface IEnumFORMATETC
		' Retrieves the specified number of FORMATETC structures in the enumeration 
		' sequence and advances the current position by the number of items retrieved
		<PreserveSig> _
		Function GetNext(ByVal celt As Integer, ByRef rgelt As ShellAPI.FORMATETC, ByRef pceltFetched As Integer) As Integer

		' Skips over the specified number of elements in the enumeration sequence
		<PreserveSig> _
		Function Skip(ByVal celt As Integer) As Integer

		' Returns to the beginning of the enumeration sequence
		<PreserveSig> _
		Function Reset() As Integer

		' Creates a new item enumeration object with the same contents and state as the current one
		<PreserveSig> _
		Function Clone(ByRef ppenum As IEnumFORMATETC) As Integer
	End Interface
End Namespace