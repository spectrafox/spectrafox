Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.IO
Imports ExpTreeLib.ShellDll.ShellAPI

Namespace ShellDll
	<ComImport, Guid("0000000c-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Public Interface IStream
		' Reads a specified number of bytes from the stream object into memory 
		' starting at the current seek pointer
		<PreserveSig> _
		Function Read(<MarshalAs(UnmanagedType.LPArray)> ByVal pv As Byte(), ByVal cb As Integer, ByVal pcbRead As IntPtr) As Int32

		' Writes a specified number of bytes into the stream object starting at 
		' the current seek pointer
		<PreserveSig> _
		Function Write(<MarshalAs(UnmanagedType.LPArray)> ByVal pv As Byte(), ByVal cb As Integer, ByVal pcbWritten As IntPtr) As Int32

		' Changes the seek pointer to a new location relative to the beginning 
		' of the stream, the end of the stream, or the current seek pointer
		<PreserveSig> _
		Function Seek(ByVal dlibMove As Long, ByVal dwOrigin As SeekOrigin, ByVal plibNewPosition As IntPtr) As Int32

		' Changes the size of the stream object
		<PreserveSig> _
		Function SetSize(ByVal libNewSize As Long) As Int32

		' Copies a specified number of bytes from the current seek pointer in 
		' the stream to the current seek pointer in another stream
		<PreserveSig> _
		Function CopyTo(ByVal pstm As IStream, ByVal cb As Long, ByVal pcbRead As IntPtr, ByVal pcbWritten As IntPtr) As Int32

		' Ensures that any changes made to a stream object open in transacted 
		' mode are reflected in the parent storage object
		<PreserveSig> _
		Function Commit(ByVal grfCommitFlags As ShellAPI.STGC) As Int32

		' Discards all changes that have been made to a transacted stream since 
		' the last call to IStream::Commit
		<PreserveSig> _
		Function Revert() As Int32

		' Restricts access to a specified range of bytes in the stream. Supporting 
		' this functionality is optional since some file systems do not provide it
		<PreserveSig> _
		Function LockRegion(ByVal libOffset As Long, ByVal cb As Long, ByVal dwLockType As ShellAPI.LOCKTYPE) As Int32

		' Removes the access restriction on a range of bytes previously restricted 
		' with IStream::LockRegion
		<PreserveSig> _
		Function UnlockRegion(ByVal libOffset As Long, ByVal cb As Long, ByVal dwLockType As ShellAPI.LOCKTYPE) As Int32

		' Retrieves the STATSTG structure for this stream
		<PreserveSig> _
		Function Stat(<System.Runtime.InteropServices.Out()> ByRef pstatstg As ShellAPI.STATSTG, ByVal grfStatFlag As ShellAPI.STATFLAG) As Int32

		' Creates a new stream object that references the same bytes as the original 
		' stream but provides a separate seek pointer to those bytes
		<PreserveSig> _
		Function Clone(<System.Runtime.InteropServices.Out()> ByRef ppstm As IntPtr) As Int32
	End Interface
End Namespace