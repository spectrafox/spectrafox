Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace ShellDll
	<ComImport, GuidAttribute("00000121-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IDropSource
		' Determines whether a drag-and-drop operation should continue
		<PreserveSig> _
		Function QueryContinueDrag(ByVal fEscapePressed As Boolean, ByVal grfKeyState As ShellAPI.MK) As Int32

		' Gives visual feedback to an end user during a drag-and-drop operation
		<PreserveSig> _
		Function GiveFeedback(ByVal dwEffect As DragDropEffects) As Int32
	End Interface
End Namespace