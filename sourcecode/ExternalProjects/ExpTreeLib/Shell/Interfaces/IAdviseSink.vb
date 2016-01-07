Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace ShellDll
	<ComImport, Guid("0000010f-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IAdviseSink
		' Advises that data has changed
		<PreserveSig> _
		Sub OnDataChange(ByRef pformatetcIn As ShellAPI.FORMATETC, ByRef pmedium As ShellAPI.STGMEDIUM)

		' Advises that view of object has changed
		<PreserveSig> _
		Sub OnViewChange(ByVal dwAspect As Integer, ByVal lindex As Integer)

		' Advises that name of object has changed
		<PreserveSig> _
		Sub OnRename(ByVal pmk As IntPtr)

		' Advises that object has been saved to disk
		<PreserveSig> _
		Sub OnSave()

		' Advises that object has been closed
		<PreserveSig> _
		Sub OnClose()
	End Interface
End Namespace