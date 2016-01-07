Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports ExpTreeLib.CShItem
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI

''' <summary>
''' A Class to serve as a broker between a Control that is associated with a single Folder and
'''  a DragTo/DropOn operation initiated From any other DragSource that the Shell supports. The calling application may change 
''' the associated Folder of an instance of ControlDropWrapper as needed - it is not needed or desireable to create new a instance
''' as the associated Folder changes.<br />
'''  This should not be used for TreeView or ListView Controls which require special handling.
''' </summary>
''' <remarks>Originally the targeted use of this Class was for DragSources that provide email items,however it will also work
'''          for Drags from WinExplorer or any application that provides an IDataObject with appropriately formatted data.</remarks>
Public Class ControlDropWrapper
    Implements ShellDll.IDropTarget, IDisposable

#Region "   Private Fields"
    Private m_FullPath As String                'The FullPath to the Dir to Drop On
    Private m_Owner As Control                  'The Control whose IDropTarget this instance is serving as
    Private m_DirCSI As CShItem                 'The CShitem of the Dir associated with this Control
    Private m_Target As ShellDll.IDropTarget    'The DropTarget of the Dir associated with this Control
    Private m_DropHelper As IDropTargetHelper   'A Generic Helper for showing images 
    Private m_Original_Effect As Integer        'Preserved across Dragxxx Events
    Private m_Default_Effect As DragDropEffects 'Default for this control, for this Drag


#End Region

#Region "   Constructor"
    Sub New(ByVal Ctl As Control, ByVal FullPath As String)
        m_DirCSI = GetCShItem(FullPath)
        If m_DirCSI Is Nothing Then
            Throw New ArgumentException(FullPath & " Is not Valid or Reachable")
        End If
        If Not m_DirCSI.IsDropTarget Then
            Throw New ArgumentException(FullPath & " Is not a Valid DropTarget")
        End If

        If TypeOf Ctl Is TreeView OrElse TypeOf Ctl Is ListView Then
            Throw New ArgumentException("Not for use on " & Ctl.GetType().Name)
        End If

        'Ensure FolderList and FileList is initialized 
        If Not m_DirCSI.FoldersInitialized Then m_DirCSI.GetDirectories()
        If Not m_DirCSI.FilesInitialized Then m_DirCSI.GetFiles()

        m_FullPath = FullPath
        m_Owner = Ctl
        If m_Owner.IsHandleCreated Then
            If Application.OleRequired = Threading.ApartmentState.STA Then
                Ctl_HandleCreated(Me, New EventArgs)
            Else
                Throw New ArgumentException("This App or Thread MustBe STA")
            End If
        End If
        AddHandler m_Owner.HandleCreated, AddressOf Ctl_HandleCreated
        AddHandler m_Owner.HandleDestroyed, AddressOf Ctl_HandleDestroyed

        Dim dropHelperPtr As IntPtr     'historical place to accept input from nxt call
        ShellHelper.GetIDropTargetHelper(dropHelperPtr, m_DropHelper)
    End Sub
#End Region

#Region "   Handle Changes"

    Private Sub Ctl_HandleCreated(ByVal sender As Object, ByVal e As EventArgs)
        Dim res As Integer
        res = RegisterDragDrop(m_Owner.Handle, Me)
        If res <> 0 AndAlso res <> -2147221247 Then
            Marshal.ThrowExceptionForHR(res)
        End If
        m_Target = m_DirCSI.GetDropTargetOf(m_Owner)
    End Sub

    Private Sub Ctl_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs)
        If m_Owner IsNot Nothing AndAlso m_Owner.IsHandleCreated Then
            ShellAPI.RevokeDragDrop(m_Owner.Handle)
            ' UPDATE: Added remove handler calls to allow a control
            ' to be shown multiple times in a modal dialog
            RemoveHandler m_Owner.HandleCreated, AddressOf Ctl_HandleCreated
            RemoveHandler m_Owner.HandleDestroyed, AddressOf Ctl_HandleDestroyed
            m_Owner = Nothing
        End If
    End Sub
#End Region

#Region "   Public Properties"
    ''' <summary>
    ''' Contains the Full Path of the Folder associated with this Control
    ''' </summary>
    ''' <returns>The Full Path of the Folder associated with this Control</returns>
    ''' <remarks>Setting this Property to another valid Path will release all references to the previous Folder (if any) and
    '''          associate this instance with the new Folder.</remarks>
    Public Property FullPath() As String
        Get
            Return m_FullPath
        End Get
        Set(ByVal Value As String)
            If Value.Equals(m_FullPath, StringComparison.CurrentCultureIgnoreCase) Then Exit Property
            m_DirCSI.ClearItems(True, False)
            Marshal.ReleaseComObject(m_Target)
            m_DirCSI = GetCShItem(Value)
            m_Target = m_DirCSI.GetDropTargetOf(m_Owner)
            m_FullPath = Value

            'Ensure FolderList and FileList is initialized 
            If Not m_DirCSI.FoldersInitialized Then m_DirCSI.GetDirectories()
            If Not m_DirCSI.FilesInitialized Then m_DirCSI.GetFiles()
        End Set
    End Property

#End Region

#Region "   IDropTarget Implementation"
    ''' <summary>
    ''' For internal use only<br />
    ''' DragEnter is called by the Shell as a drag enters the Control. Its main function is to
    ''' save off the IDataObject of the Drag for use in DragOver processing, since DragOver does
    ''' not receive the IDataObject as a parameter.
    ''' </summary>
    ''' <param name="pDataObj">IDataObject of the Folder of the Item being dragged, containing references to
    ''' the item(s) being Dragged.</param>
    ''' <param name="grfKeyState">State of the Control Keys and Mouse Buttons</param>
    ''' <param name="pt">Location, in screen coordinates, of the mouse.</param>
    ''' <param name="pdwEffect">Permitted Drop actions as set by the DragSource.</param>
    ''' <returns>S_OK</returns>
    Public Function DragEnter(ByVal pDataObj As System.IntPtr, ByVal grfKeyState As ShellDll.ShellAPI.MK, ByVal pt As ShellDll.ShellAPI.POINT, ByRef pdwEffect As System.Windows.Forms.DragDropEffects) As Integer Implements ShellDll.IDropTarget.DragEnter
        m_Original_Effect = pdwEffect
        If m_Target IsNot Nothing Then
            m_Target.DragEnter(pDataObj, grfKeyState, pt, pdwEffect)
            m_Default_Effect = pdwEffect
        Else
            m_Default_Effect = DragDropEffects.None
        End If
        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragEnter(m_Owner.Handle, pDataObj, pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function

    ''' <summary>
    ''' For internal use only
    ''' Entered when a Drag moves over the surface of the associated Control.<br />
    ''' </summary>
    ''' <param name="grfKeyState">State of the Control Keys and Mouse Buttons</param>
    ''' <param name="pt">Location, in screen coordinates, of the mouse.</param>
    ''' <param name="pdwEffect">Permitted Drop actions as set by the DragSource and modified by
    ''' candidate DropTargets.</param>
    ''' <returns>S_OK</returns>
    Public Function DragOver(ByVal grfKeyState As ShellDll.ShellAPI.MK, ByVal pt As ShellDll.ShellAPI.POINT, ByRef pdwEffect As System.Windows.Forms.DragDropEffects) As Integer Implements ShellDll.IDropTarget.DragOver
        m_Target.DragOver(grfKeyState, pt, pdwEffect)
        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragOver(pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function

    ''' <summary>
    ''' DragLeave is raised by the Shell when the Drag is cancelled or otherwise leaves the underlying
    ''' Control.  The handler does whatever cleanup is needed to prepare for another DragEnter.
    ''' </summary>
    ''' <returns>S_OK</returns>
    Public Function DragLeave() As Integer Implements ShellDll.IDropTarget.DragLeave
        m_Original_Effect = DragDropEffects.None
        If m_Target IsNot Nothing Then
            m_Target.DragLeave()
        End If

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragLeave()
        End If
        Return ShellAPI.S_OK
    End Function

    ''' <summary>
    ''' For internal use only<br />
    ''' Entered when a DragDrop has occurred on the associated Control.
    ''' </summary>
    ''' <param name="pDataObj">Pointer to the IDataObject</param>
    ''' <param name="grfKeyState">State of the keyboard Keys and Mouse Buttons</param>
    ''' <param name="pt">Where the Drop occurred on the Control</param>
    ''' <param name="pdwEffect">Result of the Drop - unreliable in case of Move</param>
    ''' <returns>S_OK</returns>
    Public Function DragDrop(ByVal pDataObj As System.IntPtr, ByVal grfKeyState As ShellDll.ShellAPI.MK, ByVal pt As ShellDll.ShellAPI.POINT, ByRef pdwEffect As System.Windows.Forms.DragDropEffects) As Integer Implements ShellDll.IDropTarget.DragDrop
        m_Target.DragDrop(pDataObj, grfKeyState, pt, pdwEffect)
        If m_DropHelper IsNot Nothing Then
            m_DropHelper.Drop(pDataObj, pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function
#End Region

#Region " IDisposable Support "

    Private disposedValue As Boolean = False        ' To detect redundant calls

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' NoneTODO: free other state (managed objects).
            End If
            If m_Target IsNot Nothing Then
                Marshal.ReleaseComObject(m_Target)
            End If
            If Not m_DropHelper Is Nothing Then
                Marshal.ReleaseComObject(m_DropHelper)
            End If
            If m_Owner IsNot Nothing AndAlso m_Owner.Handle <> IntPtr.Zero Then
                ShellAPI.RevokeDragDrop(m_Owner.Handle)
                RemoveHandler m_Owner.HandleCreated, AddressOf Ctl_HandleCreated
                RemoveHandler m_Owner.HandleDestroyed, AddressOf Ctl_HandleDestroyed
                m_Owner = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
