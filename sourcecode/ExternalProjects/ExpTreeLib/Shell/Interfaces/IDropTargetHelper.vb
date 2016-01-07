Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace ShellDll
	<ComImport, GuidAttribute("4657278B-411B-11d2-839A-00C04FD918D0"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IDropTargetHelper
		' Notifies the drag-image manager that the drop target's IDropTarget::DragEnter method has been called
		<PreserveSig> _
		Function DragEnter(ByVal hwndTarget As IntPtr, ByVal pDataObject As IntPtr, ByRef ppt As ShellAPI.POINT, ByVal dwEffect As DragDropEffects) As Int32

		' Notifies the drag-image manager that the drop target's IDropTarget::DragLeave method has been called
		<PreserveSig> _
		Function DragLeave() As Int32

		' Notifies the drag-image manager that the drop target's IDropTarget::DragOver method has been called
		<PreserveSig> _
		Function DragOver(ByRef ppt As ShellAPI.POINT, ByVal dwEffect As DragDropEffects) As Int32

		' Notifies the drag-image manager that the drop target's IDropTarget::Drop method has been called
		<PreserveSig> _
		Function Drop(ByVal pDataObject As IntPtr, ByRef ppt As ShellAPI.POINT, ByVal dwEffect As DragDropEffects) As Int32

		' Notifies the drag-image manager to show or hide the drag image
		<PreserveSig> _
		Function Show(ByVal fShow As Boolean) As Int32
	End Interface
End Namespace