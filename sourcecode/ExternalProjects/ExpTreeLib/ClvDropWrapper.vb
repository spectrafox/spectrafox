Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports ExpTreeLib.CShItem
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI

'''<summary>The ClvDropWrapper class deals with the mechanics of receiving a
''' Drag/Drop operation for a ListView.  In effect, it implements the IDropTarget interface
''' for a ListView.  It is designed to handle a ListView which <b>must</b> have CShItems 
''' in the Tags of the ListViewItems contained in the control. The ListView <b>must also</b> have the 
''' CShItem of the <i>single</i> Parent Folder stored in its' .Tag Property
''' </summary>
''' <remarks>
''' <para>The class receives the DragEnter, DragLeave, DragOver, and DragDrop events for
''' the associated ListView and performs the Drag specific processing. Unlike CtvDropWrapper,
''' this class DOES NOT raise ShDragxxx events for the ListView, and DOES do the
''' GUI related processing.</para>
''' <para>The interesting part of this class is that it makes no decisions about the drag
''' nor does any Drop related processing itself. Instead, it acts as a broker between
''' the Drag/Drop operation and the IDropTarget interface of the underlying 
''' Shell Folder.  This allows the Shell Folder, which may be a Shell Extension, to
''' perform whatever action it needs to in order to effect the Drag/Drop.
''' The benefit of this approach is that Drag/Drop targets need not be part of the
''' File System.</para>
''' ListViews, unlike TreeViews, may be displaying non-folder items and may have
''' substantial areas within the control that are empty of any ListViewItems. This requires
''' different behavior rules for a ListView receiving a Drag.
''' <list type="bullet">
''' <item><description>Upon DragEnter, the "parent" directory of the entire set of Listview items is determined. The "parent" is
'''                    determined from the CShItems contained in the ListViewItem.Tags or if there are no ListViewItems, from
'''                    the CShItem contained in the ListView's .Tag. </description></item>
''' <item><description>The default pdweffect for this control/drag is obtained from the IDropTarget of that "parent"</description></item>
''' <item><description>If there is no common "parent" and if the ListView's Tag does not contain a CShItem,
'''                    the default pdweffect for this control/drag is set to "None"</description></item>
''' <item><description>Upon DragOver: 
''' <br />If over a ListViewItem representing a Directory,Obtain IDropTarget from the Directory, and invoke its DragEnter,DragOver to set pwdeffects.
''' <br />Also set BackGroundColor of that ListView Item to SelectedColor
''' <br />If not over a ListViewItem representing a directory AndAlso a common "parent" can be determined from the ListViewItems or the ListView's
''' .Tag Property then use IDropTarget of parent to accept DragOver and adjust pdweffect.
''' <br />If not over a ListViewItem representing a directory AndAlso if no common "parent" can be determined, then
''' set pwdeffects to "None"</description></item>
''' <item><description>Upon DragLeave, all local vars are reset to "New" state</description></item>
''' <item><description>Upon DragDrop, the IDropTarget.DragDrop of the Folder represented by the current ListViewItem
''' is called and all local vars are reset to "New" state.</description></item>
'''</list></remarks>
Public Class ClvDropWrapper
    Implements ShellDll.IDropTarget, IDisposable

#Region "   Private Fields"

    Private m_ListView As ListView                  'The control
    'Private m_WasNotFullRowSelect As Boolean        'True only if need to switch back to FullRowSelect=False
    Private m_DataObj As IntPtr                     'The COM interface to IDragData - saved in DragEnter
    Private m_Original_Effect As DragDropEffects            'Save it
    Private m_Default_Effect As DragDropEffects     'Default for this control, for this Drag
    Private m_LastTarget As ShellDll.IDropTarget    'IDropTarget of most recent Folder dragged over
    Private m_LastItem As ListViewItem              'Most recent ListViewItem dragged over
    Private m_OriginalColor As Color                'Original BackColor of ListViewItem Dragged Over
    Private m_DropHelper As IDropTargetHelper       'IDropTargetHelper interface for this control
    Private m_ParentItem As CShItem                 'CShItem of Parent dir, if any, otherwise Nothing
    Private m_ParentTarget As ShellDll.IDropTarget  'IDropTarget of the Parent dir of all Items in control, or Nothing
    Private m_disposed As Boolean = False           'To detect redundant Dispose calls

#End Region

#Region "   Constructor"
    ''' <summary>
    ''' Initializes a new instance of the Class.
    ''' </summary>
    ''' <param name="ctl">The ListView for whom this Class will Handle Drag/Drops.</param>
    ''' <remarks>Registers itself to Handle Drag/Drops for the ListView.</remarks>
    Public Sub New(ByVal ctl As ListView)
        m_ListView = ctl
        ' Correct type of Control, register IDropTarget for it
        If m_ListView.IsHandleCreated Then
            If Application.OleRequired = Threading.ApartmentState.STA Then
                Dim res As Integer
                res = RegisterDragDrop(m_ListView.Handle, Me)
                If Not (res = 0) Or (res = -2147221247) Then
                    Marshal.ThrowExceptionForHR(res)
                End If
            Else
                Throw New ThreadStateException("ThreadMustBeSTA")
            End If
        Else
            Throw New ArgumentException(m_ListView.Name & " Handle has not been created")
        End If
        AddHandler m_ListView.HandleCreated, AddressOf View_HandleCreated
        AddHandler m_ListView.HandleDestroyed, AddressOf View_HandleDestroyed

        Dim dropHelperPtr As IntPtr     'historical place to accept input from nxt call
        ShellHelper.GetIDropTargetHelper(dropHelperPtr, m_DropHelper)
    End Sub
#End Region

#Region "   Handle Changes"

    Private Sub View_HandleCreated(ByVal sender As Object, ByVal e As EventArgs)
        Dim res As Integer
        res = RegisterDragDrop(m_ListView.Handle, Me)
        If Not (res = 0) Or (res = -2147221247) Then
            Marshal.ThrowExceptionForHR(res)
            'Throw New Exception("Failed to Register DragDrop for ClvDropWrapper on " & ctl.Name)
        End If
    End Sub

    Private Sub View_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs)
        If m_ListView IsNot Nothing Then
            ShellAPI.RevokeDragDrop(m_ListView.Handle)
            RemoveHandler m_ListView.HandleCreated, AddressOf View_HandleCreated
            RemoveHandler m_ListView.HandleDestroyed, AddressOf View_HandleDestroyed
            m_ListView = Nothing
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
        If m_LastItem IsNot Nothing Then
            m_LastItem.BackColor = Color.Empty
            m_LastItem.ForeColor = Color.Empty
            m_LastItem = Nothing
        End If
    End Sub
#End Region

#Region "       DragEnter"
    ''' <summary>
    ''' For internal use only
    ''' DragEnter is called by the Shell as a drag enters the listview. It determines the default (parent)
    ''' DropTarget and default (parent) pdwEffect for use in those areas of the ListView that do not
    ''' contain eligible DropTargets.
    ''' </summary>
    ''' <param name="pDataObj">IDataObject of the Folder of the Item being dragged, containing references to
    ''' the item(s) being Dragged.</param>
    ''' <param name="grfKeyState">State of the keyboard keys at this moment</param>
    ''' <param name="pt">Location, in screen coordinates, of the mouse.</param>
    ''' <param name="pdwEffect">Permitted Drop actions as set by the DragSource.</param>
    ''' <returns>Always returns S_OK (0)</returns>
    ''' <remarks></remarks>
    Public Function DragEnter(ByVal pDataObj As IntPtr, _
                                   ByVal grfKeyState As ShellAPI.MK, _
                                   ByVal pt As ShellAPI.POINT, _
                                   ByRef pdwEffect As DragDropEffects) As Integer _
                           Implements ExpTreeLib.ShellDll.IDropTarget.DragEnter

        ResetPrevTarget()                       'should be redundant, but, just in case ...
        m_Original_Effect = pdwEffect
        m_DataObj = pDataObj
        m_ParentItem = Nothing
        'Determine Parentage of this set of items -- not necessary if ListView is normal usage, but ...

        For Each lvi As ListViewItem In m_ListView.Items
            Dim csi As CShItem = TryCast(lvi.Tag, CShItem)
            If csi IsNot Nothing Then
                If m_ParentItem Is Nothing Then
                    If csi.Parent IsNot Nothing Then
                        m_ParentItem = csi.Parent
                    Else            'only Desktop lacks a parent
                        m_ParentItem = CShItem.GetDeskTop
                    End If
                Else    ' multiple parents 
                    If m_ParentItem IsNot csi.Parent Then
                        m_ParentItem = Nothing
                        Exit For
                    End If
                End If
            End If
        Next
        '7/5/2012 - The next check deals with the case of an empty ListView (Folder with no Items)
        If m_ListView.Items.Count < 1 AndAlso m_ParentItem Is Nothing AndAlso _
                                            (m_ListView.Tag IsNot Nothing AndAlso TypeOf m_ListView.Tag Is CShItem AndAlso _
                                            DirectCast(m_ListView.Tag, CShItem).IsFolder) Then _
                                            m_ParentItem = m_ListView.Tag
        ' end of 7/5/2012 change
        If m_ParentItem IsNot Nothing Then
            m_ParentTarget = m_ParentItem.GetDropTargetOf(m_ListView)
            If m_ParentTarget IsNot Nothing Then
                m_ParentTarget.DragEnter(pDataObj, grfKeyState, pt, pdwEffect)
                m_Default_Effect = pdwEffect
            Else
                m_Default_Effect = DragDropEffects.None
            End If
        Else
            m_Default_Effect = DragDropEffects.None
        End If

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragEnter(m_ListView.Handle, pDataObj, pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function
#End Region

#Region "   DragOver"
    ''' <summary>
    ''' For internal use only
    ''' Entered when a Drag moves over the surface of the associated Control.<br />
    ''' If the Mouse is over a ListViewItem representing a Folder, sets that item as the 
    ''' most recent potential Drop Target and Changes the colors of that ListViewItem.
    ''' </summary>
    ''' <param name="grfKeyState">The state of certain Keyboard keys</param>
    ''' <param name="pt">The location of the Mouse in the Control.</param>
    ''' <param name="pdwEffect">The actions permitted by the DragSource</param>
    ''' <returns>S_OK</returns>
    Public Function DragOver(ByVal grfKeyState As ShellAPI.MK, ByVal pt As ShellAPI.POINT, ByRef pdwEffect As DragDropEffects) As Integer _
                                   Implements ExpTreeLib.ShellDll.IDropTarget.DragOver
        Dim reset As Boolean = False

        Dim point As System.Drawing.Point = m_ListView.PointToClient(New System.Drawing.Point(pt.x, pt.y))

        Dim hitTest As ListViewHitTestInfo = m_ListView.HitTest(point)
        If hitTest.Item IsNot Nothing Then
            If (hitTest.Item IsNot m_LastItem) Then
                ResetPrevTarget()
                Dim item As CShItem = TryCast(hitTest.Item.Tag, CShItem)
                If item IsNot Nothing AndAlso item.IsFolder Then
                    m_LastItem = hitTest.Item
                    m_OriginalColor = m_LastItem.BackColor
                    m_LastItem.BackColor = SystemColors.Highlight
                    m_LastItem.ForeColor = SystemColors.HighlightText
                    m_LastTarget = item.GetDropTargetOf(m_ListView)
                    reset = True
                End If
            End If
        Else
            ResetPrevTarget()
        End If

        If m_LastTarget IsNot Nothing Then
            If reset Then
                m_LastTarget.DragEnter(m_DataObj, grfKeyState, pt, pdwEffect)
            Else
                m_LastTarget.DragOver(grfKeyState, pt, pdwEffect)
            End If
        ElseIf m_ParentTarget IsNot Nothing Then
            m_ParentTarget.DragOver(grfKeyState, pt, pdwEffect)
        Else
            pdwEffect = m_Default_Effect
        End If

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.DragOver(pt, pdwEffect)
        End If

        Return ShellAPI.S_OK
    End Function

#End Region

#Region "   DragLeave"
    ''' <summary>
    ''' For internal use only
    ''' DragLeave is raised by the Shell when the Drag is cancelled or otherwise leaves the underlying
    ''' listview.  The handler does whatever cleanup is needed to prepare for another DragEnter.
    ''' </summary>
    ''' <returns>Always returns S_OK</returns>
    ''' <remarks></remarks>
    Public Function DragLeave() As Integer Implements ShellDll.IDropTarget.DragLeave
        'Debug.WriteLine("In DragLeave")
        m_Original_Effect = DragDropEffects.None
        ResetPrevTarget()
        m_DataObj = IntPtr.Zero
        If m_ParentTarget IsNot Nothing Then
            m_ParentTarget.DragLeave()
            Marshal.ReleaseComObject(m_ParentTarget)
        End If
        m_ParentItem = Nothing

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
    ''' Normally simply passes the Drop on to the Dropped on Folder which is determined here
    ''' in conjuction with DragEnter.
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
            m_ParentTarget.DragLeave()            'Not dropping on it, so leave it
            'version 21 change 
            If res <> 0 AndAlso res <> 1 Then
                Debug.WriteLine("Error in dropping on DropTarget. res = " & Hex(res))
            End If 'No error on drop
            ' The documented norm for Optimized Moves is pdwEffect=None, so leave it
            ' RaiseEvent ShDragDrop(m_LastItem, grfKeyState, pdwEffect)
        ElseIf m_ParentTarget IsNot Nothing Then
            res = m_ParentTarget.DragDrop(pDataObj, grfKeyState, pt, pdwEffect)
            If res <> 0 AndAlso res <> 1 Then
                Debug.WriteLine("Error in dropping on DropTarget. res = " & Hex(res))
            End If 'No error on drop
        End If
        m_Original_Effect = DragDropEffects.None
        ResetPrevTarget()
        m_DataObj = IntPtr.Zero
        If m_ParentTarget IsNot Nothing Then
            Marshal.ReleaseComObject(m_ParentTarget)
        End If
        m_ParentItem = Nothing

        If m_DropHelper IsNot Nothing Then
            m_DropHelper.Drop(pDataObj, pt, pdwEffect)
        End If
        Return ShellAPI.S_OK
    End Function
#End Region

#Region "IDisposable processing"
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not m_disposed Then
            If disposing Then
                DisposeDropWrapper()
            End If
            If m_ListView IsNot Nothing AndAlso m_ListView.Handle <> IntPtr.Zero Then
                ShellAPI.RevokeDragDrop(m_ListView.Handle)
                m_ListView = Nothing
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
        ResetPrevTarget()
        If m_ParentTarget IsNot Nothing Then
            Marshal.ReleaseComObject(m_ParentTarget)
        End If
        m_ParentItem = Nothing
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
        End If
    End Sub

#End Region

End Class
