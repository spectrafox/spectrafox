Imports System
Imports System.Runtime.InteropServices

Namespace ShellDll
    'Not needed in .Net - use Marshal Class
    <ComImport(), Guid("00000002-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
    Public Interface IMalloc
        ' Allocates a block of memory.
        ' Return value: a pointer to the allocated memory block.
        <PreserveSig()> _
            Function Alloc( _
                    ByVal cb As Integer) As IntPtr ' Size, in bytes, of the memory block to be allocated.

        ' Changes the size of a previously allocated memory block.
        ' Return value:  Reallocated memory block 
        <PreserveSig()> _
            Function Realloc(ByVal pv As IntPtr, _
                             ByVal cb As Integer) As IntPtr

        ' Frees a previously allocated block of memory.
        <PreserveSig()> _
            Sub Free(ByVal pv As IntPtr) ' Pointer to the memory block to be freed.

        ' This method returns the size (in bytes) of a memory block previously allocated with 
        ' IMalloc::Alloc or IMalloc::Realloc.
        ' Return value: The size of the allocated memory block in bytes 
        <PreserveSig()> _
        Function GetSize(ByVal pv As IntPtr) As Integer ' Pointer to the memory block for which the size is requested.

        ' This method determines whether this allocator was used to allocate the specified block of memory.
        ' Return value: 1 - allocated 0 - not allocated by this IMalloc instance. 
        <PreserveSig()> _
            Function DidAlloc( _
                ByVal pv As IntPtr) As Int16 ' Pointer to the memory block

        ' This method minimizes the heap as much as possible by releasing unused memory to the operating system, 
        ' coalescing adjacent free blocks and committing free pages.
        <PreserveSig()> _
            Sub HeapMinimize()
    End Interface
End Namespace
