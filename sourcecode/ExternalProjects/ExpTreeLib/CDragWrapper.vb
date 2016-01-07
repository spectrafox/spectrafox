Imports System
Imports System.Collections.Generic
Imports System.Text
Imports ExpTreeLib.ShellDll
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing

''' <summary>
''' This class completely handles all drag from operation for its' associated ListView or TreeView control. Each TreeNode in the
''' associated TreeView must have a valid CShItem in its' .Tag. Each ListViewItem in the associated ListView must have a valid CShItem in
''' its' Tag. All ListViewItems and the CShItems in their .Tags must be direct children of the same Folder.
''' </summary>
Public Class CDragWrapper
    Implements IDropSource, IDisposable

    ' The Control which is our client
    Private m_Client As Control

    ' The pointer to the IDataObject being dragged
    Private dataObjectPtr As IntPtr

    ' If true then working for TreeView, False then working for ListView
    Private isTreeView As Boolean

    ' The mouseButtons state when a drag starts
    Private startButton As MouseButtons

    ' A bool to indicate whether this class has been disposed
    Private disposed As Boolean = False

    ' The events that will be raised when a drag starts and when it ends
    ''' <summary>
    ''' Event Raised when a Drag is started from the associated Control
    ''' </summary>
    Public Event DragStart As DragStartEventHandler
    ''' <summary>
    ''' Event Raised when a Drag from the associated Control is complete (Dropped)
    ''' </summary>
    Public Event DragEnd As EventHandler

    ''' <summary>
    ''' Creates and registers this instance to receive events when an item is being dragged
    ''' </summary>
    ''' <param name="Ctl">The ListView or TreeView for which to support the drag</param>
    Public Sub New(ByVal Ctl As Control)
        If TypeOf Ctl Is ListView Then
            AddHandler CType(Ctl, ListView).ItemDrag, AddressOf ItemDrag
            isTreeView = False
        ElseIf TypeOf Ctl Is TreeView Then
            AddHandler CType(Ctl, TreeView).ItemDrag, AddressOf ItemDrag
            isTreeView = True
        Else
            Throw New ArgumentException("CDragWrapper -- Does not support drags from " & Ctl.GetType.Name)
        End If
        m_Client = Ctl
    End Sub 'New

    ''' <summary>
    ''' This method initialises the dragging of a TreeNode or 1 or more ListViewItems
    ''' </summary>
    Sub ItemDrag(ByVal sender As Object, ByVal e As ItemDragEventArgs)
        ReleaseCom()

        startButton = e.Button
        Dim item As CShItem

        If isTreeView Then      'Can only drag 1 Item
            item = TryCast(e.Item.tag, CShItem)
            If item Is Nothing Then Throw New ArgumentException("CDragWrapper -- Invalid item to drag -- No CShItem in Tag")
            dataObjectPtr = ShellHelper.GetIDataObject(New CShItem() {item})
        Else        'ListView may have more than one item to drag
            'All items to drag MUST be in the same Folder!
            Dim ctl As ListView = DirectCast(m_Client, ListView)
            Dim items(ctl.SelectedItems.Count - 1) As CShItem
            Dim parent As CShItem = DirectCast(ctl.SelectedItems(0).Tag, CShItem).Parent
            Dim i As Integer
            For i = 0 To ctl.SelectedItems.Count - 1
                If Not parent Is DirectCast(ctl.SelectedItems(i).Tag, CShItem).Parent Then Exit Sub
                items(i) = DirectCast(ctl.SelectedItems(i).Tag, CShItem)
            Next i
            item = items(0)
            dataObjectPtr = ShellHelper.GetIDataObject(items)
        End If

        If dataObjectPtr <> IntPtr.Zero Then
            Dim AllowedEffects As DragDropEffects
            Dim effects As DragDropEffects
            Dim parent As CShItem
            If item.Parent IsNot Nothing Then
                parent = item.Parent
            Else
                parent = item
            End If
            If TypeOf m_Client Is TreeView Then
                AllowedEffects = DragDropEffects.Copy Or DragDropEffects.Move
            Else        'must be ListView
                AllowedEffects = DragDropEffects.Copy Or DragDropEffects.Move Or DragDropEffects.Link
            End If

            RaiseEvent DragStart(sender, New DragStartEventArgs(parent, m_Client))
            ShellAPI.DoDragDrop(dataObjectPtr, Me, AllowedEffects, effects)
            RaiseEvent DragEnd(m_Client, New EventArgs())
        End If
    End Sub 'ItemDrag

    ''' <summary>
    ''' Provides a minimal implementation of IDropSource.QueryContinueDrag
    ''' </summary>
    ''' <param name="fEscapePressed">True if the Escape Key is pressed</param>
    ''' <param name="grfKeyState">Which Button is pressed</param>
    ''' <returns>S_CANCEL if Escape Key is pressed, S_OK otherwise</returns>
    Public Function QueryContinueDrag(ByVal fEscapePressed As Boolean, ByVal grfKeyState As ShellAPI.MK) As Integer _
            Implements ShellDll.IDropSource.QueryContinueDrag
        If fEscapePressed Then
            Return ShellAPI.DRAGDROP_S_CANCEL
        Else
            If (startButton And MouseButtons.Left) <> 0 And (grfKeyState And ShellAPI.MK.LBUTTON) = 0 Then
                Return ShellAPI.DRAGDROP_S_DROP
            Else
                If (startButton And MouseButtons.Right) <> 0 And (grfKeyState And ShellAPI.MK.RBUTTON) = 0 Then
                    Return ShellAPI.DRAGDROP_S_DROP
                Else
                    Return ShellAPI.S_OK
                End If
            End If
        End If
    End Function 'QueryContinueDrag

    ''' <summary>
    ''' Used to provide a minimal implementation of IDropSource.GiveFeedback
    ''' </summary>
    ''' <param name="dwEffect">Unused</param>
    ''' <returns>Always returns DRAGDROP_S_USEDEFAULTCURSORS</returns>
    Public Function GiveFeedback(ByVal dwEffect As DragDropEffects) As Integer Implements ShellDll.IDropSource.GiveFeedback
        Return ShellAPI.DRAGDROP_S_USEDEFAULTCURSORS
    End Function 'GiveFeedback


    ''' <summary>
    ''' If not disposed, dispose the class
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        If Not disposed Then
            ReleaseCom()
            GC.SuppressFinalize(Me)

            disposed = True
        End If
    End Sub 'Dispose

    ''' <summary>
    ''' Release the IDataObject and free's the allocated memory
    ''' </summary>
    Private Sub ReleaseCom()
        If dataObjectPtr <> IntPtr.Zero Then
            Marshal.Release(dataObjectPtr)
            dataObjectPtr = IntPtr.Zero
        End If
    End Sub 'ReleaseCom

End Class 'CDragWrapper

''' <summary>
''' The Delegate defining the signature of an Event Handler to Handle the Event Raised when
''' a Drag is started from a Control associated with an instance of <see cref="ExpTreeLib.CDragWrapper">CDragWrapper</see>.
''' </summary>
''' <param name="sender">The Control from which the Drag originates</param>
''' <param name="e">A <see cref="ExpTreeLib.DragStartEventArgs">DragStartEventArgs</see>DragStartEventArgs constructed by CDragWrapper</param>
Public Delegate Sub DragStartEventHandler(ByVal sender As Object, ByVal e As DragStartEventArgs)

''' <summary>
''' An EventArgs which provides information about a Drag started within a Control managed by an instance of <see cref="ExpTreeLib.CDragWrapper">CDragWrapper</see>.
''' </summary>
''' <remarks>The information is the CShItem of the Folder being Dragged or the Parent of the Items being Dragged and the Control in which the Drag originated.</remarks>
Public Class DragStartEventArgs
    Inherits EventArgs
    Private m_parent As CShItem
    Private m_DragStartControl As Control

    ''' <summary>
    ''' Contructs a new Instance of this Class
    ''' </summary>
    ''' <param name="parent">The Folder being Dragged or the Parent Folder of the Items being Dragged</param>
    ''' <param name="dragStartControl">Control in which the Drag originated</param>
    Public Sub New(ByVal parent As CShItem, ByVal dragStartControl As Control)
        m_parent = parent
        m_DragStartControl = dragStartControl
    End Sub 'New

    ''' <summary>
    ''' The Folder being Dragged or the Parent Folder of the Items being Dragged
    ''' </summary>
    ''' <returns>The Folder being Dragged or the Parent Folder of the Items being Dragged</returns>
    ''' <remarks>If Drag is a single Folder then the CShItem of that Folder. If from a ListView, then the Folder containing all
    '''          Item(s) being Dragged</remarks>
    Public ReadOnly Property Parent() As CShItem
        Get
            Return m_parent
        End Get
    End Property

    ''' <summary>
    ''' The Control in which the Drag originated
    ''' </summary>
    ''' <returns>The Control in which the Drag originated</returns>
    Public ReadOnly Property DragStartControl() As Control
        Get
            Return m_DragStartControl
        End Get
    End Property
End Class 'DragEnterEventArgs '

