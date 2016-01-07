
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports ExpTreeLib.CShItem
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI

'''<summary>The CtvDropWrapper class deals with the mechanics of receiving a
''' Drag/Drop operation for a TreeView Control. In effect, it implements the IDropTarget interface
''' for a TreeView. It is designed to handle a TreeView which <b>must</b> have CShItems 
''' in the Tags of the TreeNodes contained in the control.
'''</summary>
'''<remarks>
''' <para>The class recieves the DragEnter, DragLeave, DragOver, and DragDrop events for
''' the associated TreeView, performs the Drag specific processing, and raises corresponding 
''' Events for the associated TreeView to allow the TreeView to do any GUI related processing.</para>
''' The interesting part of this class is that it makes no decisions about the drag
''' nor does any Drop related processing itself. Instead, it acts as a broker between
''' the Drag/Drop operation and the IDropTarget interface of the underlying 
''' Shell Folder.  This allows the Shell Folder, which may be a Shell Extention, to
''' perform whatever action it needs to in order to effect the Drag/Drop.
''' The benefit of this approach is that Drag/Drop targets need not be part of the
''' File System.
''' </remarks>
Public Class CtvDropWrapper
    Implements ShellDll.IDropTarget, IDisposable

#Region "   Private Fields"

    Private m_TreeView As TreeView                  'The Tree if m_treeview is a TreeView, else nothing
    Private m_DataObj As IntPtr                     'The COM interface to IDragData - saved in DragEnter
    Private m_Original_Effect As Integer    'Save it
    Private m_LastTarget As ShellDll.IDropTarget    'IDropTarget of most recent Folder dragged over
    Private m_LastNode As TreeNode                  'Most recent node dragged over
    Private m_DropHelper As IDropTargetHelper       'IDropTargetHelper interface for this control
    Private m_disposed As Boolean = False           'To detect redundant Dispose calls

#End Region

#Region "   Public Events"
    ''' <summary>
    ''' The Event Raised by this Class to inform the TreeView that a Drag has entered the TreeView
    ''' </summary>
    ''' <param name="pDataObj">Pointer to the DataObject being Dragged.</param>
    ''' <param name="grfKeyState">State of the Control Keys and Mouse Buttons</param>
    ''' <param name="pdwEffect">The type of Drop actions permitted by the Drag Source</param>
    Public Event ShDragEnter(ByVal pDataObj As IntPtr, _
                                ByVal grfKeyState As Integer, _
                                ByVal pdwEffect As Integer)

    ''' <summary>
    ''' The Event Raised by this Class to inform the TreeView that a Drag has moved over the TreeView
    ''' </summary>
    ''' <param name="Node">The TreeNode that the Drag is over</param>
    ''' <param name="ClientPoint">Location, in Client coordinates, of the mouse.</param>
    ''' <param name="grfKeyState">State of the Control Keys and Mouse Buttons</param>
    ''' <param name="pdwEffect">The type of Drop actions permitted by the Drag Source</param>
    Public Event ShDragOver(ByVal Node As TreeNode, _
                                ByVal ClientPoint As System.Drawing.Point, _
                                ByVal grfKeyState As Integer, _
                                ByVal pdwEffect As Integer)
    ''' <summary>
    ''' The Event Raised by this Class to inform the TreeView that a Drag has left the TreeView
    ''' </summary>
    Public Event ShDragLeave()

    ''' <summary>
    ''' The Event Raised by this Class to inform the TreeView that a Drop has occured on the TreeView
    ''' </summary>
    ''' <param name="Node">The TreeNode that the Drop occured on</param>
    ''' <param name="grfKeyState"></param>
    ''' <param name="grfKeyState">State of the Control Keys and Mouse Buttons</param>
    Public Event ShDragDrop(ByVal Node As TreeNode, _
                                ByVal grfKeyState As Integer, _
                                ByVal pdwEffect As Integer)

#End Region

#Region "   Public Enum -- KeyStates"
    ''' <summary>
    ''' Bit mask showing the state of Control Keys and Mouse Buttons during a Drag
    ''' </summary>
    <Flags()> _
    Public Enum KeyStates
        LButtonMask = 1
        RButtonMask = 2
        ShiftMask = 4
        CtrlMask = 8
        MButtonMask = 16
        AltMask = 32
    End Enum
#End Region

#Region "   Constructor"
    ''' <summary>
    ''' Initializes a new instance of the Class.
    ''' </summary>
    ''' <param name="ctl">The TreeView for which this instance will Handle Drag/Drop</param>
    ''' <remarks>Registers itself to Handle Drag/Drops for the TreeView.</remarks>
    Public Sub New(ByVal ctl As TreeView)
        m_TreeView = ctl
        ' Correct type of Control, register IDropTarget for it
        If m_TreeView.IsHandleCreated Then
            If Application.OleRequired = Threading.ApartmentState.STA Then
                Dim res As Integer
                res = RegisterDragDrop(m_TreeView.Handle, Me)
                If Not (res = 0) Or (res = -2147221247) Then
                    Marshal.ThrowExceptionForHR(res)
                End If
            Else
                Throw New ThreadStateException("ThreadMustBeSTA")
            End If
        Else
            Throw New ArgumentException(m_TreeView.Name & " Handle has not been created")
        End If
        AddHandler m_TreeView.HandleCreated, AddressOf View_HandleCreated
        AddHandler m_TreeView.HandleDestroyed, AddressOf View_HandleDestroyed

        Dim dropHelperPtr As IntPtr     'historical place to accept input from nxt call
        ShellHelper.GetIDropTargetHelper(dropHelperPtr, m_DropHelper)
    End Sub
#End Region

#Region "   Handle Changes"

    Private Sub View_HandleCreated(ByVal sender As Object, ByVal e As EventArgs)
        Dim res As Integer
        res = RegisterDragDrop(m_TreeView.Handle, Me)
        If Not (res = 0) Or (res = -2147221247) Then
            Marshal.ThrowExceptionForHR(res)
            'Throw New Exception("Failed to Register DragDrop for CDropWrapper on " & ctl.Name)
        End If
    End Sub

    Private Sub View_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs)
        If m_TreeView IsNot Nothing AndAlso m_TreeView.IsHandleCreated Then
            ShellAPI.RevokeDragDrop(m_TreeView.Handle)
            ' UPDATE: Added remove handler calls to allow a treeview
            ' to be shown multiple times in a modal dialog
            RemoveHandler m_TreeView.HandleCreated, AddressOf View_HandleCreated
            RemoveHandler m_TreeView.HandleDestroyed, AddressOf View_HandleDestroyed
            m_TreeView = Nothing
        End If
    End Sub

#End Region

#Region "   ResetPreviousTarget -- a utility/cleanup Method"
    Private Sub ResetPrevTarget()
        If Not IsNothing(m_LastTarget) Then
            Dim hr As Integer = m_LastTarget.DragLeave
            Marshal.ReleaseComObject(m_LastTarget)
            m_LastTarget = Nothing
        End If
        m_LastNode = Nothing
    End Sub
#End Region

#Region "   DragEnter"
    ''' <summary>
    ''' For internal use only<br />
    ''' DragEnter is called by the Shell as a drag enters the TreeView. Its main function is to
    ''' save off the IDataObject of the Drag for use in DragOver processing, since DragOver does
    ''' not receive the IDataObject as a parameter.
    ''' </summary>
    ''' <param name="pDataObj">IDataObject of the Folder of the Item being dragged, containing references to
    ''' the item(s) being Dragged.</param>
    ''' <param name="grfKeyState">State of the Control Keys and Mouse Buttons</param>
    ''' <param name="pt">Location, in screen coordinates, of the mouse.</param>
    ''' <param name="pdwEffect">Permitted Drop actions as set by the DragSource.</param>
    ''' <returns>Always returns S_OK (0)</returns>
    Friend Function DragEnter(ByVal pDataObj As IntPtr, _
                                   ByVal grfKeyState As ShellAPI.MK, _
                                   ByVal pt As ShellAPI.POINT, _
                                   ByRef pdwEffect As DragDropEffects) As Integer _
                           Implements ExpTreeLib.ShellDll.IDropTarget.DragEnter

        ResetPrevTarget()                       'should be redundant, but, just in case ...
        m_Original_Effect = pdwEffect
        m_DataObj = pDataObj

        RaiseEvent ShDragEnter(pDataObj, grfKeyState, pdwEffect)

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragEnter(m_TreeView.Handle, pDataObj, pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function
#End Region

#Region "   DragOver"
    ''' <summary>
    ''' For internal use only
    ''' Entered when a Drag moves over the surface of the associated Control.<br />
    ''' </summary>
    ''' <param name="grfKeyState">State of the Control Keys and Mouse Buttons</param>
    ''' <param name="pt">Location, in screen coordinates, of the mouse.</param>
    ''' <param name="pdwEffect">Permitted Drop actions as set by the DragSource and modified by
    ''' candidate DropTargets.</param>
    ''' <returns>Always returns S_OK (0)</returns>
    Friend Function DragOver(ByVal grfKeyState As ShellAPI.MK, _
                                 ByVal pt As ShellAPI.POINT, _
                                 ByRef pdwEffect As DragDropEffects) As Integer _
                         Implements ExpTreeLib.ShellDll.IDropTarget.DragOver

        Dim tn As TreeNode
        Dim ptClient As System.Drawing.Point = m_TreeView.PointToClient(New System.Drawing.Point(pt.x, pt.y))
        tn = m_TreeView.GetNodeAt(ptClient)
        If IsNothing(tn) Then                   'not over a TreeNode
            ResetPrevTarget()
        Else                                    'currently over Treenode
            If Not IsNothing(m_LastNode) Then   'not the first, check if same as last time
                If tn Is m_LastNode Then
                    If m_DropHelper IsNot Nothing Then m_DropHelper.DragOver(pt, pdwEffect) '7/11/2012
                    Return ShellAPI.S_OK        'all set up anyhow
                Else
                    ResetPrevTarget()
                    m_LastNode = tn
                End If
            Else    'is the first
                ResetPrevTarget()   'just in case
                m_LastNode = tn     'save current node
            End If

            'Drag is now over a new node. Get the IDropTarget of the Folder and interact with it

            Dim CSI As CShItem = tn.Tag
            If CSI.IsDropTarget Then
                m_LastTarget = CSI.GetDropTargetOf(m_TreeView)
                If Not IsNothing(m_LastTarget) Then
                    pdwEffect = m_Original_Effect

                    Dim res As Integer = m_LastTarget.DragEnter(m_DataObj, grfKeyState, pt, pdwEffect)
                    If res = 0 Then
                        res = m_LastTarget.DragOver(grfKeyState, pt, pdwEffect)
                    End If
                    If res <> 0 Then
                        Marshal.ThrowExceptionForHR(res)
                    End If
                Else
                    pdwEffect = DragDropEffects.None 'couldn't get IDropTarget, so report effect None
                End If
            Else
                pdwEffect = DragDropEffects.None  'CSI not a drop target, so report effect None
            End If
            RaiseEvent ShDragOver(tn, ptClient, grfKeyState, pdwEffect)
        End If

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragOver(pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function
#End Region

#Region "   DragLeave"
    ''' <summary>
    ''' DragLeave is raised by the Shell when the Drag is cancelled or otherwise leaves the underlying
    ''' TreeView.  The handler does whatever cleanup is needed to prepare for another DragEnter.
    ''' </summary>
    ''' <returns>Always returns S_OK</returns>
    ''' <remarks></remarks>
    Public Function DragLeave() As Integer Implements ShellDll.IDropTarget.DragLeave
        'Debug.WriteLine("In DragLeave")
        m_Original_Effect = 0
        ResetPrevTarget()
        m_DataObj = IntPtr.Zero
        RaiseEvent ShDragLeave()

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragLeave()
        End If
        Return ShellAPI.S_OK
    End Function
#End Region

#Region "   DragDrop"
    ''' <summary>
    ''' For internal use only
    ''' Entered when a DragDrop has occurred on the associated Control.
    ''' </summary>
    ''' <param name="pDataObj">Pointer to the IDataObject</param>
    ''' <param name="grfKeyState">State of the keyboard Keys and Mouse Buttons</param>
    ''' <param name="pt">Where the Drop occurred on the Control</param>
    ''' <param name="pdwEffect">Result of the Drop - unreliable in case of Move</param>
    ''' <returns>S_OK</returns>
    Public Function DragDrop(ByVal pDataObj As IntPtr, _
                                    ByVal grfKeyState As ShellAPI.MK, _
                                    ByVal pt As ShellAPI.POINT, _
                                    ByRef pdwEffect As DragDropEffects) As Integer _
                                Implements ExpTreeLib.ShellDll.IDropTarget.DragDrop

        'Debug.WriteLine("In DragDrop: Effect = " & pdwEffect & " Keystate = " & grfKeyState)
        Dim res As Integer
        If Not IsNothing(m_LastTarget) Then
            res = m_LastTarget.DragDrop(pDataObj, grfKeyState, pt, pdwEffect)
            'version 21 change 
            If res <> 0 AndAlso res <> 1 Then
                Debug.WriteLine("Error in dropping on DropTarget. res = " & Hex(res))
            End If 'No error on drop
            ' The documented norm for Optimized Moves is pdwEffect=None, so leave it
            RaiseEvent ShDragDrop(m_LastNode, grfKeyState, pdwEffect)
        End If
        ResetPrevTarget()
        m_DataObj = IntPtr.Zero
        m_Original_Effect = 0

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.Drop(pDataObj, pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function
#End Region

#Region "   IDisposable processing"
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not m_disposed Then
            If disposing Then
                DisposeDropWrapper()
            End If
            If m_TreeView IsNot Nothing Then
                ShellAPI.RevokeDragDrop(m_TreeView.Handle)
                m_TreeView = Nothing
            End If
        End If
        m_disposed = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Revokes the View from getting drop messages and releases the IDropTarget
    ''' </summary>
    Private Sub DisposeDropWrapper()
        ReleaseCom()
        If Not m_DropHelper Is Nothing Then
            Marshal.ReleaseComObject(m_DropHelper)
        End If
    End Sub

    ''' <summary>
    ''' Release the IDropTarget and free's the allocated memory
    ''' </summary>
    Private Sub ReleaseCom()
        If Not m_LastTarget Is Nothing Then
            Marshal.ReleaseComObject(m_LastTarget)

            m_LastTarget = Nothing
            'm_dropHelperPtr = IntPtr.Zero
        End If
    End Sub

#End Region

End Class
