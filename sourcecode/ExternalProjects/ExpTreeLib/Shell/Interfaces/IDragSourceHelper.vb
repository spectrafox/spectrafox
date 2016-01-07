Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices

Namespace ShellDll
	<ComImport, GuidAttribute("DE5BF786-477A-11d2-839D-00C04FD918D0"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IDragSourceHelper
		' Initializes the drag-image manager for a windowless control
		<PreserveSig> _
		Function InitializeFromBitmap(ByRef pshdi As ShellAPI.SHDRAGIMAGE, ByVal pDataObject As IntPtr) As Int32

		' Initializes the drag-image manager for a control with a window
		<PreserveSig> _
		Function InitializeFromWindow(ByVal hwnd As IntPtr, ByRef ppt As ShellAPI.POINT, ByVal pDataObject As IntPtr) As Int32
	End Interface
End Namespace