Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace ShellDll
	<ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000122-0000-0000-C000-000000000046")> _
	Public Interface IDropTarget
		' Determines whether a drop can be accepted and its effect if it is accepted
		<PreserveSig> _
		Function DragEnter(ByVal pDataObj As IntPtr, ByVal grfKeyState As ShellAPI.MK, ByVal pt As ShellAPI.POINT, ByRef pdwEffect As DragDropEffects) As Int32

		' Provides target feedback to the user through the DoDragDrop function
		<PreserveSig> _
		Function DragOver(ByVal grfKeyState As ShellAPI.MK, ByVal pt As ShellAPI.POINT, ByRef pdwEffect As DragDropEffects) As Int32

		' Causes the drop target to suspend its feedback actions
		<PreserveSig> _
		Function DragLeave() As Int32

		' Drops the data into the target window
		<PreserveSig> _
		Function DragDrop(ByVal pDataObj As IntPtr, ByVal grfKeyState As ShellAPI.MK, ByVal pt As ShellAPI.POINT, ByRef pdwEffect As DragDropEffects) As Int32
	End Interface
End Namespace