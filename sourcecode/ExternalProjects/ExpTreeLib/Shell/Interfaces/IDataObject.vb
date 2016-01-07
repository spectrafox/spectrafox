Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace ShellDll
	<ComImport, Guid("0000010e-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IDataObject
		' Renders the data described in a FORMATETC structure 
		' and transfers it through the STGMEDIUM structure
		<PreserveSig> _
		Function GetData(ByRef pformatetcIn As ShellAPI.FORMATETC, ByRef pmedium As ShellAPI.STGMEDIUM) As Int32

		' Renders the data described in a FORMATETC structure 
		' and transfers it through the STGMEDIUM structure allocated by the caller
		<PreserveSig> _
		Function GetDataHere(ByRef pformatetcIn As ShellAPI.FORMATETC, ByRef pmedium As ShellAPI.STGMEDIUM) As Int32

		' Determines whether the data object is capable of 
		' rendering the data described in the FORMATETC structure
		<PreserveSig> _
		Function QueryGetData(ByRef pformatetc As ShellAPI.FORMATETC) As Int32

		' Provides a potentially different but logically equivalent FORMATETC structure
		<PreserveSig> _
		Function GetCanonicalFormatEtc(ByRef pformatetc As ShellAPI.FORMATETC, ByRef pformatetcout As ShellAPI.FORMATETC) As Int32

		' Provides the source data object with data described by a 
		' FORMATETC structure and an STGMEDIUM structure
		<PreserveSig> _
		Function SetData(ByRef pformatetcIn As ShellAPI.FORMATETC, ByRef pmedium As ShellAPI.STGMEDIUM, ByVal frelease As Boolean) As Int32

		' Creates and returns a pointer to an object to enumerate the 
		' FORMATETC supported by the data object
		<PreserveSig> _
		Function EnumFormatEtc(ByVal dwDirection As Integer, ByRef ppenumFormatEtc As IEnumFORMATETC) As Int32

		' Creates a connection between a data object and an advise sink so 
		' the advise sink can receive notifications of changes in the data object
		<PreserveSig> _
		Function DAdvise(ByRef pformatetc As ShellAPI.FORMATETC, ByRef advf As ShellAPI.ADVF, ByRef pAdvSink As IAdviseSink, ByRef pdwConnection As Integer) As Int32

		' Destroys a notification previously set up with the DAdvise method
		<PreserveSig> _
		Function DUnadvise(ByVal dwConnection As Integer) As Int32

		' Creates and returns a pointer to an object to enumerate the current advisory connections
		<PreserveSig> _
		Function EnumDAdvise(ByRef ppenumAdvise As Object) As Int32
	End Interface
End Namespace