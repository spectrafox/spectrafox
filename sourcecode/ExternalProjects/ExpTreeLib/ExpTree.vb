Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports ExpTreeLib.CShItem
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI
Imports ExpTreeLib.ShellDll.ShellHelper
Imports ExpTreeLib.SystemImageListManager

''' <summary>
''' ExpTree is a UserControl encapsulating a TreeView which will display all or part of the Windows Shell
''' Namespace.  The Shell Namespace is a superset of the Windows file system. It is the Tree commonly shown
''' by Windows Explorer, in Classic View. That is, it is a Tree rooted in the Desktop.
''' ExpTree supports Drag and Drop and standard Windows Context Menus.
''' </summary>
''' <remarks>ExpTree raises one major Event, ExpTreeNodeSelected. That event is raised whenever the 
''' Selected TreeNode changes because of User Action (i.e. -- clicking on the node)</remarks>
<DefaultProperty("StartUpDirectory"), DefaultEvent("StartUpDirectoryChanged")> _
Public Class ExpTree
    Inherits System.Windows.Forms.UserControl

    Private Root As TreeNode

    ''' <summary>
    ''' StartUpDirectoryChanged is raised when the root of the TreeView is changed via StartUpDirectory
    ''' Property. 
    ''' </summary>
    ''' <param name="newVal">One of the StartDir Enum values that represent the possible Start Up Directories.</param>
    ''' <remarks>Seldom listened for since, in typical use, the Method which set the StartUpDirectory value
    ''' is the only Method which is interested. It is also true that a by-product of setting the StartUpDirectory 
    ''' value is the Selection of the new root node.  That change in SelectedNode will cause an ExpTreeNodeSelected
    ''' Event to be raised.</remarks>
    Public Event StartUpDirectoryChanged(ByVal newVal As StartDir)

    ''' <summary>
    ''' ExpTreeNodeSelected is raised when a Node in the TreeView is Selected.
    ''' </summary>
    ''' <param name="SelPath">The Path of the CShItem represented by the TreeNode, and stored in the
    ''' TreeNode's Tag.</param>
    ''' <param name="Item">The CShItem represented by the TreeNode, and stored in the
    ''' TreeNode's Tag.</param>
    ''' <remarks></remarks>
    Public Event ExpTreeNodeSelected(ByVal SelPath As String, ByVal Item As CShItem)

    Private EnableEventPost As Boolean = True 'flag to supress ExpTreeNodeSelected raising during refresh and 

    Private WithEvents DropHandler As CtvDropWrapper

    Private DragHandler As CDragWrapper

    Private m_showHiddenFolders As Boolean = True

    Private m_windowsContextMenu As WindowsContextMenu = New WindowsContextMenu

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call


        'setting the imagelist here allows many good things to happen, but
        ' also one bad thing -- the "tooltip" like display of selectednode.text
        ' is made invisible.  This remains a problem to be solved.
        SystemImageListManager.SetTreeViewImageList(tv1, False)

        AddHandler StartUpDirectoryChanged, AddressOf OnStartUpDirectoryChanged

        AddHandler CShItemUpdate, AddressOf OnItemUpdate            '7/1/2012

        'RaiseEvent StartUpDirectoryChanged(StartDir.Desktop)        '11/08/2013 -- Removed 01/08/2014

    End Sub
    'ExpTree overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            RemoveHandler CShItemUpdate, AddressOf OnItemUpdate     '7/1/2012
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents tv1 As System.Windows.Forms.TreeView

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tv1 = New System.Windows.Forms.TreeView
        Me.SuspendLayout()
        '
        'tv1
        '
        Me.tv1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tv1.HideSelection = False
        Me.tv1.HotTracking = True
        Me.tv1.Location = New System.Drawing.Point(0, 0)
        Me.tv1.Name = "tv1"
        Me.tv1.ShowRootLines = False
        Me.tv1.Size = New System.Drawing.Size(200, 264)
        Me.tv1.TabIndex = 1
        '
        'ExpTree
        '
        Me.Controls.Add(Me.tv1)
        Me.Name = "ExpTree"
        Me.Size = New System.Drawing.Size(200, 264)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "   Initialization/Destruction Events"

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)> _
    Private Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal pszSubAppName As String, ByVal pszSubIdList As String) As Integer
    End Function

    Private Sub tv1_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles tv1.HandleCreated
        DragHandler = New CDragWrapper(Me.tv1)
        If m_AllowDrop Then DropHandler = New CtvDropWrapper(Me.tv1) '7/11/2012
        SetWindowTheme(tv1.Handle, "explorer", Nothing)
        ' Update: Check against nothing as when the treeview is used in a modal 
        ' dialog and shown more than once a duplicate call causes a horizontal 
        ' scrollBar to appear in the control???
        'If tv1.TreeViewNodeSorter Is Nothing Then tv1.TreeViewNodeSorter = New TagComparer  '5/9/2012 Removed - not needed and can cause extreme delays
    End Sub

    Private Sub tv1_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs) Handles tv1.HandleDestroyed

    End Sub

#End Region

#Region "   Public Properties"

#Region "       AllowDrop - Property added 7/11/2012"

    Private m_AllowDrop As Boolean = False

    ''' <summary>
    ''' Turns this ExpTree Control's ability to accept Drops on or Off.<br />
    ''' True - Enables the ExpTree Control to accept Drops.<br />
    ''' False - Disables the ExpTree Control acceptance of  Drops.
    ''' </summary>
    ''' <returns>True or False</returns>
    ''' <remarks>Works by assigning or  removing an instance of CtvDropWrapper to the Local variable DropHandler.</remarks>
    Public Overrides Property AllowDrop() As Boolean
        Get
            Return DropHandler IsNot Nothing
        End Get
        Set(ByVal value As Boolean)
            m_AllowDrop = value
            If value Then
                If tv1.IsHandleCreated Then
                    If DropHandler Is Nothing Then      'otherwise, already running
                        DropHandler = New CtvDropWrapper(tv1)
                    End If
                End If
            Else
                If DropHandler IsNot Nothing Then
                    DropHandler.Dispose()
                    DropHandler = Nothing
                End If
            End If
        End Set
    End Property

#End Region

#Region "       RootItem"
    '''<summary>
    ''' RootItem is a Run-Time only Property. Setting this Item via an External call results in
    '''  re-setting the entire tree to be rooted in the input CShItem.
    ''' The new CShItem must be a valid CShItem of some kind of Folder (File Folder or System Folder).
    ''' Attempts to set it using a non-Folder CShItem are ignored.
    '''</summary>
    <Browsable(False)> _
    Public Property RootItem() As CShItem
        Get
            If Root Is Nothing OrElse Root.Tag Is Nothing Then  '11/05/2013
                Return CShItem.GetDeskTop                       '11/05/2013
            Else                                                '11/05/2013
                Return Root.Tag
            End If                                              '11/05/2013
        End Get
        Set(ByVal Value As CShItem)
            If Value.IsFolder Then
                tv1.BeginUpdate()
                ClearTree()
                Dim CSI() As CShItem = Value.Directories
                Root = New TreeNode(Value.DisplayName)
                BuildTree(CSI)
                Root.ImageIndex = SystemImageListManager.GetIconIndex(Value, False)
                Root.SelectedImageIndex = Root.ImageIndex
                Root.Tag = Value
                tv1.Nodes.Add(Root)
                Root.Expand()
                tv1.SelectedNode = Root
                tv1.EndUpdate()
            End If
        End Set
    End Property
#End Region

#Region "       SelectedItem"
    ''' <summary>
    ''' Run-time only Property which returns the CShItem underlying the SelectedNode of the TreeView.
    ''' </summary>
    ''' <returns>The underlying CShItem of the TreeView.SelectedNode. If none Selected, returns Nothing.</returns>
    <Browsable(False)> _
    Public ReadOnly Property SelectedItem() As CShItem
        Get
            If Not IsNothing(tv1.SelectedNode) Then
                Return tv1.SelectedNode.Tag
            Else
                Return Nothing
            End If
        End Get
    End Property
#End Region

#Region "       ShowHidden"
    ''' <summary>
    ''' ShowHiddenFolders sets or gets a Boolean indicating whether or not to Display Folders with the Hidden Attribute.
    ''' </summary>
    ''' <value></value>
    ''' <returns>True if ExpTree is Displaying Hidden Folders, False if not.</returns>
    ''' <remarks>Hidden Folders may be Displayed or not Displayed at run-time.</remarks>
    <Browsable(True), Category("Options"), _
    Description("Show Hidden Directories."), _
    DefaultValue(True)> _
   Public Property ShowHiddenFolders() As Boolean
        Get
            Return m_showHiddenFolders
        End Get
        Set(ByVal Value As Boolean)
            If m_showHiddenFolders Xor Value Then
                m_showHiddenFolders = Value
                If Root IsNot Nothing Then RefreshTree() 'Fix 2/5/2012
            End If
        End Set
    End Property
#End Region

#Region "       ShowRootLines"
    ''' <summary>
    ''' Exposes the normal TreeView ShowRootLines property.
    ''' </summary>
    ''' <value></value>
    ''' <returns>The state of the underlying TreeView property.</returns>
    ''' <remarks></remarks>
    <Category("Options"), _
  Description("Allow Collapse of Root Item."), _
  Browsable(True)> _
Public Property ShowRootLines() As Boolean
        Get
            Return tv1.ShowRootLines
        End Get
        Set(ByVal Value As Boolean)
            If Not (Value = tv1.ShowRootLines) Then
                tv1.ShowRootLines = Value
                tv1.Refresh()
            End If
        End Set
    End Property
#End Region

#Region "       StartupDir"
    ''' <summary>
    ''' The values representing the System's Special Folders.
    ''' </summary>
    ''' <remarks>Certain Special Folders are disallowed since they may not exist, or may cause program failure
    ''' on certain versions of Windows (primarily the older, unsupported versions).</remarks>
    Public Enum StartDir As Integer
        Desktop = &H0
        Programs = &H2
        Controls = &H3
        Printers = &H4
        Personal = &H5
        Favorites = &H6
        Startup = &H7
        Recent = &H8
        SendTo = &H9
        StartMenu = &HB
        MyDocuments = &HC
        'MyMusic = &HD
        'MyVideo = &HE
        DesktopDirectory = &H10
        MyComputer = &H11
        My_Network_Places = &H12
        'NETHOOD = &H13
        'FONTS = &H14
        ApplicatationData = &H1A
        'PRINTHOOD = &H1B
        Internet_Cache = &H20
        Cookies = &H21
        History = &H22
        Windows = &H24
        System = &H25
        Program_Files = &H26
        MyPictures = &H27
        Profile = &H28
        Systemx86 = &H29
        AdminTools = &H30
    End Enum

    Private m_StartUpDirectory As StartDir = StartDir.Desktop

    ' 11/04/2012 Removed DefaultValue Property from this declaration.
    ''' <summary>
    ''' Sets the initial Root directory of ExpTree.
    ''' </summary>
    ''' <value>Must be one of the StartDir Enum values.</value>
    ''' <returns>Current StartDir value.</returns>
    ''' <remarks></remarks>
    <Category("Options"), _
     Description("Sets the Initial Directory of the Tree"), _
     Browsable(True)> _
    Public Property StartUpDirectory() As StartDir
        Get
            Return m_StartUpDirectory
        End Get
        Set(ByVal Value As StartDir)
            If Array.IndexOf([Enum].GetValues(Value.GetType), Value) >= 0 Then
                m_StartUpDirectory = Value
                RaiseEvent StartUpDirectoryChanged(Value)
            Else
                Throw New ApplicationException("Invalid Initial StartUpDirectory")
            End If
        End Set
    End Property
#End Region

#End Region

#Region "   Public Methods"

#Region "       ExpandANode"
    ''' <summary>
    ''' Expands TreeNodes from the tree root through the input Path. All intermediate nodes between the
    ''' Tree Root and the input Path are Expanded. If the Optional Property SelectExpandedNode is True (the Default),
    ''' the Expanded Node will be Selected, Raising a ExpNodeSelected Event. If False, the current Selected Node is unchanged
    ''' and no Event is Raised.
    ''' </summary>
    ''' <param name="newPath">The FileSystem path of the Node node to be Expanded.</param>
    ''' <param name="SelectExpandedNode">If True(the Default) then Select the Expanded Node.<br />
    '''                                  If False, Do Not Select the Expanded Node.</param>
    ''' <returns>True if Successful, False otherwise.</returns>
    ''' <remarks>The preferred method is to use:
    ''' <pre lang="vbnet">Public Function ExpandANode(ByVal newItem As CShItem) As Boolean</pre> 
    ''' If the item defined by the input Path does not exist, False is returned.<br />
    ''' Calling with SelectExpandedNode = False is useful when it is not desired to Raise an
    ''' ExpTreeNodeSelected Event as a result of ExpandaNode.</remarks>
    Public Function ExpandANode(ByVal newPath As String, Optional ByVal SelectExpandedNode As Boolean = True) As Boolean  '7/13/2012
        ExpandANode = False     'assume failure
        Dim newItem As CShItem
        Try
            newItem = GetCShItem(newPath)
            If newItem Is Nothing Then Exit Function
            If Not newItem.IsFolder Then Exit Function
        Catch
            Exit Function
        End Try
        Return ExpandANode(newItem, SelectExpandedNode) '7/13/2012
    End Function
    ''' <summary>
    ''' Expands TreeNodes from the tree root through the input CShItem. All intermediate nodes between the
    ''' Tree Root and the input CShItem are Expanded. If the Optional Property SelectExpandedNode is True (the Default),
    ''' the Expanded Node will be Selected, Raising a ExpNodeSelected Event. If False, the current Selected Node is unchanged
    ''' and no Event is Raised.
    ''' </summary>
    ''' <param name="newItem">The CShItem representing the Shell Namespace object whose TreeNode is to
    ''' be expanded.</param>
    ''' <param name="SelectExpandedNode">If True(the Default) then Select the Expanded Node.<br />
    '''                                  If False, Do Not Select the Expanded Node.</param>
    ''' <returns>True if Successful, False otherwise.</returns>
    ''' <remarks>This is the preferred method of ExpandANode.<br />
    ''' Calling with SelectExpandedNode = False is useful when it is not desired to Raise an
    ''' ExpTreeNodeSelected Event as a result of ExpandaNode.</remarks>
    Public Function ExpandANode(ByVal newItem As CShItem, Optional ByVal SelectExpandedNode As Boolean = True) As Boolean   '7/13/2012
        ExpandANode = False     'assume failure
        Dim baseNode As TreeNode = Root
        tv1.BeginUpdate()
        baseNode.Expand() 'Ensure base is filled in
        'do the drill down -- Node to expand must be included in tree
        Dim testNode As TreeNode
        Dim lim As Integer = CShItem.PidlCount(newItem.PIDL) - CShItem.PidlCount(baseNode.Tag.pidl)
        'TODO: Test ExpandARow again on XP to ensure that the CP problem is fixed
        Do While lim > 0
            For Each testNode In baseNode.Nodes
                If CShItem.IsAncestorOf(testNode.Tag, newItem, False) Then
                    baseNode = testNode
                    'RefreshNode(baseNode)   'ensure up-to-date
                    baseNode.Expand()
                    lim -= 1
                    Continue Do
                End If
            Next
            GoTo XIT     'on falling thru For, we can't find it, so get out
NEXLEV: Loop
        'after falling thru here, we have found & expanded the node
        Me.tv1.HideSelection = False
        Me.Select()
        If SelectExpandedNode Then Me.tv1.SelectedNode = baseNode '7/13/2012
        ExpandANode = True
XIT:    tv1.EndUpdate()
        baseNode.EnsureVisible()       '12/18/13
    End Function
#End Region

#End Region

#Region "   Initial Dir Set Handler"

    Private Sub OnStartUpDirectoryChanged(ByVal newVal As StartDir)
        tv1.BeginUpdate()
        ClearTree()
        Dim special As CShItem
        special = GetCShItem(CType(Val(m_StartUpDirectory), ShellDll.ShellAPI.CSIDL))
        Root = New TreeNode(special.DisplayName)
        Root.ImageIndex = SystemImageListManager.GetIconIndex(special, False)
        Root.SelectedImageIndex = Root.ImageIndex
        Root.Tag = special
        BuildTree(special.Directories)
        tv1.Nodes.Add(Root)
        Root.Expand()
        tv1.EndUpdate()
    End Sub

    Private Sub BuildTree(ByVal L1 As CShItem())
        Array.Sort(L1)
        Dim CSI As CShItem
        For Each CSI In L1
            If Not (CSI.IsHidden And Not m_showHiddenFolders) Then
                Root.Nodes.Add(MakeNode(CSI))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Creates a TreeNode whose .Text is the DisplayName of the CShItem.<br />
    ''' Sets the IconIndexes for that TreeNode from the CShItem.<br />
    ''' Sets the Tag of the TreeNode to the CShItem<br />
    ''' If the CShItem (a Folder) has or may have sub-Folders (see Remarks), adds a Dummy node to
    '''   the TreeNode's .Nodes collection. This is always done if the input CShItem represents a Removable device. Checking
    '''   further on such devices may cause unacceptable delays.
    ''' Returns the complete TreeNode.
    ''' </summary>
    ''' <param name="item">The CShItem to make a TreeNode to represent.</param>
    ''' <returns>A TreeNode set up to represent the CShItem.</returns>
    ''' <remarks>
    ''' This routine will not be called if the CShItem (a Folder) is Hidden and ExpTree's ShowHidden Property is False.<br />
    ''' If the Folder is Hidden and ShowHidden is True, then this routine will be called.<br />
    ''' If the Folder is Hidden and it only contains Hidden Folders (files are not considered here), then, 
    ''' the HasSubFolders attribute may be returned False even though Hidden Folders exist. In that case, we 
    ''' must make an extra check to ensure that the TreeNode is expandable.<br />
    ''' 
    ''' There are additional complication with HasSubFolders. 
    ''' <ul>
    ''' <li>
    ''' On XP and earlier systems, HasSubFolders was always
    ''' returned True if the Folder was on a Remote system. On Vista and above, the OS would check and return an 
    ''' accurate value. This extra check can take a long time on Remote systems - approximately the same amount of time as checking
    ''' item.GetDirectories.Count. Versions 2.12 and above of ExpTreeLib have a modified HasSubFolders Property which will always
    ''' return True if the Folder is on a Remote system, restoring XP behavior.</li>
    ''' <li>
    ''' On XP and earlier systems, compressed files (.zip, .cab, etc) were treated as files. On Vista and above, they are treated
    ''' as Folders. ExpTreeLib continues to treat such files as files. The HasSubFolder attribute will report a Folder which
    ''' contains only compressed files as True. In MakeNode, I simply accept the Vista and above interpretation, setting a dummy
    ''' node in such a Folder. An attempt to expand such a TreeNode will just turn off the expansion marker.
    ''' </li>
    ''' </ul>
    ''' </remarks>
    Private Function MakeNode(ByVal item As CShItem) As TreeNode
        Dim newNode As New TreeNode(item.DisplayName)
        newNode.Tag = item
        newNode.ImageIndex = SystemImageListManager.GetIconIndex(item, False)
        newNode.SelectedImageIndex = SystemImageListManager.GetIconIndex(item, True)
        If item.IsRemovable Then
            newNode.Nodes.Add(New TreeNode(" : "))
        ElseIf item.HasSubFolders Then
            newNode.Nodes.Add(New TreeNode(" : "))
        ElseIf item.IsHidden Then
            If item.DirCount > 0 Then           '02/12/2014
                newNode.Nodes.Add(New TreeNode(" : "))
            End If
        End If
        Return newNode
    End Function

    Private Sub ClearTree()
        tv1.Nodes.Clear()
        Root = Nothing
    End Sub
#End Region

#Region "   RefreshTree"
    '''<summary>RefreshTree Method thanks to Calum McLellan</summary>
    <Description("Refresh the Tree and all nodes through the currently selected item")> _
    Private Sub RefreshTree(Optional ByVal rootCSI As CShItem = Nothing)
        'Modified to use ExpandANode(CShItem) rather than ExpandANode(path)
        'Set refresh variable for BeforeExpand method
        EnableEventPost = False
        'Begin Calum's change -- With some modification
        Dim Selnode As TreeNode
        If IsNothing(Me.tv1.SelectedNode) Then
            Selnode = Me.Root
        Else
            Selnode = Me.tv1.SelectedNode
        End If
        'End Calum's change
        Try
            Me.tv1.BeginUpdate()
            Dim SelCSI As CShItem = Selnode.Tag
            'Set root node
            If IsNothing(rootCSI) Then
                Me.RootItem = Me.RootItem
            Else
                Me.RootItem = rootCSI
            End If
            'Try to expand the node
            If Not Me.ExpandANode(SelCSI) Then
                Dim nodeList As New ArrayList()
                While Not IsNothing(Selnode.Parent)
                    nodeList.Add(Selnode.Parent)
                    Selnode = Selnode.Parent
                End While

                For Each Selnode In nodeList
                    If Me.ExpandANode(CType(Selnode.Tag, CShItem)) Then Exit For
                Next
            End If
            'Reset refresh variable for BeforeExpand method
        Finally
            Me.tv1.EndUpdate()
        End Try
        EnableEventPost = True
        'We suppressed EventPosting during refresh, so give it one now
        tv1_AfterSelect(Me, New TreeViewEventArgs(tv1.SelectedNode))
    End Sub
#End Region

#Region "   TreeView BeforeExpand Event"

    Private Sub tv1_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tv1.BeforeExpand
        Dim oldCursor As Cursor = Cursor
        Cursor = Cursors.WaitCursor
        If e.Node.Nodes.Count = 1 AndAlso e.Node.Nodes(0).Text.Equals(" : ") Then
            PopulateNode(e.Node)            '8/26/2012
        End If
        e.Node.ImageIndex = DirectCast(e.Node.Tag, CShItem).IconIndexOpen
        Cursor = oldCursor
    End Sub

    ''' <summary>
    ''' Called to Populate the TreeNodes of a TreeNode that only contains a Dummy Node.
    ''' </summary>
    ''' <param name="NodeToFill">The unexpanded TreeNode to Fill</param>
    ''' <remarks>Should only be called to populate a TreeNode which only has a Dummy Node.<br />
    ''' Refactored code added 8/26/2012 so that this functionality could be used from more than one method.</remarks>
    Private Sub PopulateNode(ByVal NodeToFill As TreeNode)          '8/26/2012
        Dim CSI As CShItem = NodeToFill.Tag
        '02/12/2014 - Setting of D changed at suggestion of Michael Ruby
        Dim D As ArrayList
        If CSI.DirectoryList Is Nothing Then
            D = New ArrayList(CSI.Directories)
        Else
            D = New ArrayList(CSI.DirectoryList)
        End If
        If D.Count > 0 Then
            D.Sort()    'uses the class comparer
            NodeToFill.Nodes.Clear()    '11/03/2012 DO NOT Clear out the dummy prior to calling .Directories which forces a UpdateRefresh!
            For Each Item As CShItem In D
                If Not (Item.IsHidden And Not m_showHiddenFolders) Then
                    NodeToFill.Nodes.Add(MakeNode(Item))
                End If
            Next
        Else        '11/03/2012 BUT DO get rid of any unnessesary Dummy
            NodeToFill.Nodes.Clear()
        End If
    End Sub
#End Region

#Region "   TreeView AfterSelect Event"
    Private Sub tv1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tv1.AfterSelect
        Dim CSI As CShItem = e.Node.Tag
        If EnableEventPost Then 'turned off during RefreshTree
            If CSI.Path.StartsWith(":") Then
                RaiseEvent ExpTreeNodeSelected(CSI.DisplayName, CSI)
            Else
                RaiseEvent ExpTreeNodeSelected(CSI.Path, CSI)
            End If
        End If
    End Sub
#End Region

#Region "   TreeView VisibleChanged Event"
    '''<summary>When a form containing this control is Hidden and then re-Shown,
    ''' the association to the SystemImageList is lost.  Also lost is the
    ''' Expanded state of the various TreeNodes. 
    ''' The VisibleChanged Event occurs when the form is re-shown (and other times
    '''  as well).  
    ''' We re-establish the SystemImageList as the ImageList for the TreeView and
    ''' restore at least some of the Expansion.</summary> 
    Private Sub tv1_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tv1.VisibleChanged
        If tv1.Visible Then
            SystemImageListManager.SetTreeViewImageList(tv1, False)
            If Not Root Is Nothing Then
                Root.Expand()
                If Not IsNothing(tv1.SelectedNode) Then
                    tv1.SelectedNode.Expand()
                Else
                    tv1.SelectedNode = Me.Root
                End If
            End If
        End If
    End Sub
#End Region

#Region "   TreeView BeforeCollapse Event"
    '''<summary>Should never occur since if the condition tested for is True,
    ''' the user should never be able to Collapse the node. However, it is
    ''' theoretically possible for the code to request a collapse of this node
    ''' If it occurs, cancel it</summary>
    Private Sub tv1_BeforeCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tv1.BeforeCollapse
        If Not tv1.ShowRootLines AndAlso e.Node Is Root Then
            e.Cancel = True
        Else
            e.Node.ImageIndex = DirectCast(e.Node.Tag, CShItem).IconIndexNormal
        End If
    End Sub
#End Region

#Region "   CtvDropWrapper Event Handling"

    ' dropNode is the TreeNode that most recently was DraggedOver or
    '    Dropped onto.  
    Private dropNode As TreeNode

    Private NodePoint As System.Drawing.Point

    'expandNodeTimer is used to expand a node that is hovered over, with a delay
    Private WithEvents expandNodeTimer As New System.Windows.Forms.Timer()

#Region "       expandNodeTimer_Tick"
    Private Sub expandNodeTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) _
       Handles expandNodeTimer.Tick
        expandNodeTimer.Stop()
        If Not IsNothing(dropNode) Then
            RemoveHandler DropHandler.ShDragOver, AddressOf DragWrapper_ShDragOver
            Try
                tv1.BeginUpdate()
                dropNode.Expand()
                '7/12/2012 - The following block of code relocated from ShDragOver
                Dim delta As Integer = tv1.Height - NodePoint.Y
                If delta < tv1.Height / 2 And delta > 0 Then
                    If Not IsNothing(dropNode) AndAlso Not (dropNode.NextVisibleNode Is Nothing) Then
                        dropNode.NextVisibleNode.EnsureVisible()
                    End If
                End If
                If delta > tv1.Height / 2 And delta < tv1.Height Then
                    If Not IsNothing(dropNode) AndAlso Not (dropNode.PrevVisibleNode Is Nothing) Then
                        dropNode.PrevVisibleNode.EnsureVisible()
                    End If
                End If
                '7/12/2012 - end of relocated block
                dropNode.EnsureVisible()
            Finally
                tv1.EndUpdate()
                AddHandler DropHandler.ShDragOver, AddressOf DragWrapper_ShDragOver
            End Try
        End If
    End Sub
#End Region

    '''<summary>ShDragEnter does nothing. It is here for debug tracking</summary>
    Private Sub DragWrapper_ShDragEnter(ByVal pDataObj As IntPtr, _
                                        ByVal grfKeyState As Integer, _
                                        ByVal pdwEffect As Integer) _
                                Handles DropHandler.ShDragEnter
        'Debug.WriteLine("Enter ExpTree ShDragEnter. PdwEffect = " & pdwEffect)
    End Sub

    '''<summary>Drag has left the control. Cleanup what we have to</summary>
    Private Sub DragWrapper_ShDragLeave() Handles DropHandler.ShDragLeave
        expandNodeTimer.Stop()    'shut off the dragging over nodes timer
        'Debug.WriteLine("Enter ExpTree ShDragLeave")
        If Not IsNothing(dropNode) Then
            ResetTreeviewNodeColor(dropNode)
        End If
        dropNode = Nothing
    End Sub

    '''<summary>ShDragOver manages the appearance of the TreeView.  Management of
    ''' the underlying FolderItem is done in CDragWrapper
    ''' Credit to Cory Smith for TreeView colorizing technique and code,
    ''' at http://addressof.com/blog/archive/2004/10/01/955.aspx
    ''' Node expansion based on expandNodeTimer added by me.
    '''</summary>
    Private Sub DragWrapper_ShDragOver(ByVal Node As TreeNode, _
                                ByVal pt As System.Drawing.Point, _
                                ByVal grfKeyState As Integer, _
                                ByVal pdwEffect As Integer) _
                                Handles DropHandler.ShDragOver
        'Debug.WriteLine("Enter ExpTree ShDragOver. PdwEffect = " & pdwEffect)
        'Debug.WriteLine(vbTab & "Over node: " & CType(Node, TreeNode).Text)

        If IsNothing(Node) Then  'clean up node stuff & fix color. Leave Draginfo alone-cleaned up on DragLeave
            expandNodeTimer.Stop()
            If Not dropNode Is Nothing Then
                ResetTreeviewNodeColor(dropNode)
                dropNode = Nothing
            End If
        Else  'Drag is Over a node - fix color & DragDropEffects
            If Node Is dropNode Then
                Exit Sub    'we've already done it all
            End If

            expandNodeTimer.Stop() 'not over previous node anymore
            Try
                tv1.BeginUpdate()
                '7/12/2012 - the following block relocated to expandNodeTime.Tick
                'Dim delta As Integer = tv1.Height - pt.Y
                'If delta < tv1.Height / 2 And delta > 0 Then
                '    If Not IsNothing(Node) AndAlso Not (Node.NextVisibleNode Is Nothing) Then
                '        Node.NextVisibleNode.EnsureVisible()
                '    End If
                'End If
                'If delta > tv1.Height / 2 And delta < tv1.Height Then
                '    If Not IsNothing(Node) AndAlso Not (Node.PrevVisibleNode Is Nothing) Then
                '        Node.PrevVisibleNode.EnsureVisible()
                '    End If
                'End If
                '7/12/2012 - end of relocated block
                If Not Node.BackColor.Equals(SystemColors.Highlight) Then
                    ResetTreeviewNodeColor(tv1.Nodes(0))
                    Node.BackColor = SystemColors.Highlight
                    Node.ForeColor = SystemColors.HighlightText
                End If
            Finally
                tv1.EndUpdate()
            End Try
            dropNode = Node     'dropNode is the Saved Global version of Node
            NodePoint = pt      '7/12/2012 NodePoint is the Saved, Form Global Mouse Location (in client coordinates)
            If Not dropNode.IsExpanded Then
                expandNodeTimer.Interval = 500  '7/12/2012 - reduced from 1200
                expandNodeTimer.Start()
            End If
        End If
    End Sub

    Private Sub DragWrapper_ShDragDrop(ByVal Node As TreeNode, _
                                ByVal grfKeyState As Integer, _
                                ByVal pdwEffect As Integer) Handles DropHandler.ShDragDrop
        expandNodeTimer.Stop()
        'Debug.WriteLine("Enter ExpTree ShDragDrop. PdwEffect = " & pdwEffect)
        'Debug.WriteLine(vbTab & "Over node: " & CType(Node, TreeNode).Text)

        If Not IsNothing(dropNode) Then
            ResetTreeviewNodeColor(dropNode)
        Else
            ResetTreeviewNodeColor(tv1.Nodes(0))
        End If
        dropNode = Nothing
        'Debug.WriteLine("Leaving ExpTree ShDragDrop")
    End Sub

    Private Sub ResetTreeviewNodeColor(ByVal node As TreeNode)
        If Not node.BackColor.Equals(Color.Empty) Then
            node.BackColor = Color.Empty
            node.ForeColor = Color.Empty
        End If
        If Not node.FirstNode Is Nothing AndAlso node.IsExpanded Then
            Dim child As TreeNode
            For Each child In node.Nodes
                If Not child.BackColor.Equals(Color.Empty) Then
                    child.BackColor = Color.Empty
                    child.ForeColor = Color.Empty
                End If
                If Not child.FirstNode Is Nothing AndAlso child.IsExpanded Then
                    ResetTreeviewNodeColor(child)
                End If
            Next
        End If
    End Sub
#End Region

#Region "   LabelEdit "

    'V2.14: Added LabelEdit region -- Credit Calum

    Private m_allowFolderRename As Boolean
    ''' <summary>
    ''' Allow renaming of folders using LabelEdit
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Category("Behavior"), Description("Allow renaming of folders using LabelEdit")> _
    Public Property AllowFolderRename() As Boolean
        Get
            Return m_allowFolderRename
        End Get
        Set(ByVal value As Boolean)
            m_allowFolderRename = value
            tv1.LabelEdit = value
        End Set
    End Property

    'Newest code from Calum for Before and After LabelEdit. His remarks are:
    'I also made some changes to ExpTree, I added a check for a dummy node as after renaming a folder 
    'that hadn't been expanded I would receive an error as a BeforeLabelEdit event was fired for the 
    'dummyx node. This only happened with SharePoint folders 
    '(Note that SharePoint folder ALWAYS return true for HasSubFolders and this was happening on 
    'folders without subfolders...) I also replaced the IsFileSystem check (always false for SharePoint) 
    'with a check for a special folder path - this seemed to cover everthing that CanRename didn't cover.
    'I removed the character check in AfterLabelEdit as SetNameOf shows the user a message with illegal 
    'characters if there are any. 

    Private Sub tv1_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tv1.BeforeLabelEdit
        If e.Node.Text = " : " Then
            e.CancelEdit = True
            Exit Sub
        End If
        Dim item As CShItem = DirectCast(e.Node.Tag, CShItem)
        'If item.Path.StartsWith("::") Or item.IsDisk Or (Not m_allowFolderRename) Or _
        '    item.Path = CShItem.GetCShItem(CSIDL.MYDOCUMENTS).Path Or _
        '    Not (item.CanRename) Then
        ' Changed 11/28/2010
        If item.Path.StartsWith("::") OrElse item.IsDisk OrElse (Not m_allowFolderRename) OrElse _
            item.Path = CShItem.GetCShItem(CSIDL.MYDOCUMENTS).Path OrElse _
            Not (item.CanRename) Then
            System.Media.SystemSounds.Beep.Play()
            e.CancelEdit = True
        End If
    End Sub

    Private Sub tv1_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tv1.AfterLabelEdit
        Dim item As CShItem = DirectCast(e.Node.Tag, CShItem)
        Dim NewName As String

        Try
            NewName = e.Label.Trim
        Catch ex As Exception
            e.CancelEdit = True
            System.Media.SystemSounds.Beep.Play()
            Exit Sub
        End Try

        Dim newPidl As IntPtr = IntPtr.Zero
        If item.Parent.Folder.SetNameOf(tv1.Handle, CShItem.ILFindLastID(item.PIDL), NewName, SHGDN.NORMAL, newPidl) = S_OK Then
            'the following line is not needed since use of SetNameOf will cause a renamed WM_Notify msg 
            'which will be handled thru normal change notification processes
            'item.Update(newPidl, CShItemUpdater.CShItemUpdateType.Renamed)
        Else
            System.Media.SystemSounds.Beep.Play()
            e.CancelEdit = True
        End If
    End Sub

#End Region

#Region "   Context Menu Methods"
    ' Credit Calum 

    Private m_useWindowsContextMenu As Boolean = True
    ''' <summary>
    ''' Sets whether or not the control should use Windows System context menu for TreeNode items.
    ''' </summary>
    ''' <returns>The current setting (True or False).</returns>
    ''' <remarks>Setting this Property to False prevents the display and processing of Windows Context Menus on a Right-Click on a TreeNode.</remarks>
    <Category("Behavior"), Description("Whether the control should use windows context menus."), _
    DefaultValue(True)> _
    Public Property UseWindowsContextMenu() As Boolean
        Get
            Return m_useWindowsContextMenu
        End Get
        Set(ByVal value As Boolean)
            m_useWindowsContextMenu = value
        End Set
    End Property


    Private Sub ExpTree_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tv1.MouseUp
        If e.Button = MouseButtons.Right Then
            Dim tn As TreeNode
            Dim pt As System.Drawing.Point = PointToClient(MousePosition)
            tn = tv1.GetNodeAt(pt)
            If m_useWindowsContextMenu And Not IsNothing(tn) Then
                Dim itms(0) As CShItem
                itms(0) = DirectCast(tn.Tag, CShItem)
                Dim cmi As CMInvokeCommandInfoEx = Nothing
                If m_windowsContextMenu.ShowMenu(Me.Handle, itms, MousePosition, m_allowFolderRename, cmi) Then
                    'Check for rename
                    Dim cmdBytes(256) As Byte
                    m_windowsContextMenu.winMenu.GetCommandString(cmi.lpVerb.ToInt32, GCS.VERBA, 0, cmdBytes, 256)

                    Dim cmdName As String = szToString(cmdBytes).ToLower
                    If cmdName.Equals("rename") Then
                        tv1.LabelEdit = True
                        tn.BeginEdit()
                    Else
                        m_windowsContextMenu.InvokeCommand(m_windowsContextMenu.winMenu, cmi.lpVerb, itms(0).Parent.Path, pt)
                    End If
                    'Marshal.ReleaseComObject(m_windowsContextMenu.winMenu)
                    m_windowsContextMenu.ReleaseMenu()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Windows Message Handler for receiving Messages associated with a System Menu. 
    ''' This is what causes Cascading menus to Display
    ''' </summary>
    ''' <param name="m">A Windows Message</param>
    ''' <remarks>Only Handles Messages relating to Windows Context Menus</remarks>
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        'For send to menu in the explorer context menu
        Dim hr As Integer = 0
        If m.Msg = WM.INITMENUPOPUP Or m.Msg = WM.MEASUREITEM Or m.Msg = WM.DRAWITEM Then
            If Not m_windowsContextMenu.winMenu2 Is Nothing Then
                hr = m_windowsContextMenu.winMenu2.HandleMenuMsg(m.Msg, m.WParam, m.LParam)
                If hr = 0 Then
                    Return
                End If
            End If
        ElseIf m.Msg = WM.MENUCHAR Then
            If Not m_windowsContextMenu.winMenu3 Is Nothing Then
                hr = m_windowsContextMenu.winMenu3.HandleMenuMsg2(m.Msg, m.WParam, m.LParam, IntPtr.Zero)
                If hr = 0 Then
                    Return
                End If
            End If
        End If
        MyBase.WndProc(m)
    End Sub
#End Region

#Region "   Dynamic Update Handler"

    'Private WithEvents DeskTopItem As CShItem = CShItem.GetDeskTop     '7/1/2012

    Private Sub OnItemUpdate(ByVal sender As Object, ByVal e As ShellItemUpdateEventArgs) 'Handles DeskTopItem.CShItemUpdate  '7/1/2012 removed Handles clause
        'Debug.WriteLine("Enter ExpTree OnItemUpdate -- " & e.Item.DisplayName & " - " & e.UpdateType.ToString)
        If e.Item IsNot Nothing AndAlso e.Item.IsFolder Then  'no interest in non-folder events (or UpdateDir)
            Dim Parent As CShItem = DirectCast(sender, CShItem)
            Dim pNode As TreeNode
            If GetTreeNode(Parent, pNode) Then
                'Debug.WriteLine("Located Parent Node " & pNode.Text & " of Item " & e.Item.Path)
                Try
                    tv1.BeginUpdate()
                    Select Case e.UpdateType
                        Case CShItemUpdateType.Created  'A new Dir has been created under Parent/pNode
                            Dim Node As TreeNode = MakeNode(e.Item)
                            'Debug.WriteLine("Adding Node " & NodePath(Node))
                            InsertNode(Node, pNode) '6/25/2012
                            'pNode.Nodes.Add(Node)  '6/25/2012
                            'tv1.Invalidate()   '6/18/2012 - Trust tv1 to do right thing on an Add
                            Exit Select
                        Case CShItemUpdateType.Deleted  'An old Dir has been deleted from Parent/pNode
                            For Each Node As TreeNode In pNode.Nodes
                                If Node.Tag IsNot Nothing AndAlso Node.Tag Is e.Item Then
                                    'Debug.WriteLine("Removing Node " & NodePath(Node))
                                    pNode.Nodes.Remove(Node)
                                    'tv1.Invalidate()   '6/18/2012 - Trust tv1 to do right thing on a Delete
                                    Exit Select
                                End If
                            Next
                            'In the Renamed case, pnode is the Parent CShitem Before the rename,
                            'get the current Parent CShItem from the renamed CShItem(e.Item)
                        Case CShItemUpdateType.Renamed  'A directory has been renamed under Parent/pNode
                            For Each Node As TreeNode In pNode.Nodes
                                If Node.Tag IsNot Nothing AndAlso Node.Tag Is e.Item Then
                                    Dim wasSelected As Boolean = tv1.SelectedNode Is Node
                                    Node.Text = e.Item.DisplayName
                                    pNode.Nodes.Remove(Node)
                                    Dim curPNode As TreeNode
                                    If GetTreeNode(e.Item.Parent, curPNode) Then
                                        InsertNode(Node, curPNode) '6/25/2012
                                        'curPNode.Nodes.Add(Node)  '6/25/2012
                                        If wasSelected Then     '6/25/2012
                                            tv1.SelectedNode = Node
                                            Node.EnsureVisible()
                                        End If
                                    End If
                                    'tv1.Invalidate()   '6/18/2012 - Trust tv1 to do right thing on an Add or Delete
                                    Exit Select
                                End If
                            Next
                        Case CShItemUpdateType.MediaChange  'Media has been added/removed
                            For indx As Integer = 0 To pNode.Nodes.Count - 1
                                If pNode.Tag IsNot Nothing AndAlso pNode.Nodes(indx).Tag Is e.Item Then
                                    Dim node As TreeNode = pNode.Nodes(indx)
                                    Dim item As CShItem = node.Tag
                                    Dim wasExpanded As Boolean = node.IsExpanded
                                    If wasExpanded Then
                                        node.ImageIndex = item.IconIndexOpen
                                    Else
                                        node.ImageIndex = item.IconIndexNormal
                                    End If
                                    node.Collapse(False)
                                    node.Nodes.Clear()
                                    If item.HasSubFolders Then
                                        node.Nodes.Add(New TreeNode(" : "))
                                    End If
                                    If wasExpanded Then node.Expand()
                                    tv1.Invalidate()
                                    If node Is tv1.SelectedNode Then
                                        If e.Item.Path.StartsWith(":") Then
                                            RaiseEvent ExpTreeNodeSelected(e.Item.DisplayName, e.Item)
                                        Else
                                            RaiseEvent ExpTreeNodeSelected(e.Item.Path, e.Item)
                                        End If
                                    End If
                                    Exit Select
                                End If
                            Next
                        Case CShItemUpdateType.Updated  '5/24/2012 - In this case, it is the Item that had some change. Check if Expandability has changed
                            Dim UNode As TreeNode
                            If GetTreeNode(e.Item, UNode) Then    'otherwise don't care
                                'If UNode.IsExpanded Then        'Earlier msgs will update the nodes
                                '    SortNodes(UNode)
                                'Else    '6/5/2012 - check Expandable - in case a Folder added or Deleted which may happen without another message (Async ops)
                                If UNode.Nodes.Count = 0 Then     'Was not Expandable, should it be? (Folder may have been added)
                                    If e.Item.IsRemovable Then
                                        UNode.Nodes.Add(New TreeNode(" : "))
                                    ElseIf e.Item.HasSubFolders Then
                                        UNode.Nodes.Add(New TreeNode(" : "))
                                    ElseIf e.Item.IsHidden Then
                                        If e.Item.DirCount > 0 Then             '02/12/2014
                                            UNode.Nodes.Add(New TreeNode(" : "))
                                        End If
                                    End If
                                    UNode.Collapse(False)   '02/12/2014 can only have 0 or 1 (dummy) node - collapse to avoid showing dummy
                                    ' 02/12/2014 ElseIf Block recast and now uses DirCount rather than Directories
                                ElseIf UNode.Nodes.Count = 1 AndAlso UNode.Nodes(0).Text.Equals(" : ") Then 'Should it still have dummy? (Folder may have been Deleted)
                                    If Not e.Item.IsRemovable AndAlso Not e.Item.HasSubFolders AndAlso _
                                       (e.Item.IsHidden AndAlso e.Item.DirCount = 0) Then
                                        UNode.Nodes.Clear()
                                    End If
                                End If
                                'End If
                            End If
                        Case Else
                            'Don't care about any other type of change
                    End Select
                Catch ex As Exception
                    Debug.WriteLine("ExpTree Update Error -- " & ex.ToString)
                Finally
                    tv1.EndUpdate()
                End Try
            Else        'no find means that node not expanded and therefore of no interest
            End If
        End If
    End Sub

    Private Function GetTreeNode(ByVal shellItem As CShItem, ByRef treeNode As TreeNode) As Boolean
        Dim pathList As New List(Of CShItem)
        If shellItem Is Nothing Then shellItem = GetDeskTop() '6/18/2012

        Do While Not shellItem.Parent Is Nothing
            pathList.Add(shellItem)
            shellItem = shellItem.Parent
        Loop
        pathList.Add(shellItem)

        pathList.Reverse()

        If tv1.Nodes.Count < 1 Then Return False '11/05/2012
        treeNode = tv1.Nodes(0)
        Dim i As Integer = 0
        'since pathList starts from Desktop and the tree may start somewhere below that, first locate
        'the tree base in the path
        Dim found As Boolean = False
        Do While i < pathList.Count
            If pathList(i) Is treeNode.Tag Then
                found = True
                Exit Do
            End If
            i += 1
        Loop
        If Not found Then           'failed to find match between top of tree and top of pathlist -- so exit
            treeNode = Nothing
            Return False
        End If
        'have top of tree and pathList(i) as equal -- find actual node, down from top of tree
        i += 1
        Do While i < pathList.Count
            found = False         'reset for next loop
            For Each node As TreeNode In treeNode.Nodes
                If Not node.Tag Is Nothing AndAlso node.Tag Is pathList(i) Then
                    treeNode = node
                    found = True
                    Exit For
                End If
            Next node

            If (Not found) Then
                treeNode = Nothing
                Return False
            End If
            i += 1
        Loop
        Return True
    End Function

    ''' <summary>
    ''' Insert a TreeNode into its' Parent's Nodes list in its' proper location
    ''' in its' Parent Node's Nodes list.
    ''' </summary>
    ''' <param name="Node">The Node to be inserted</param>
    ''' <param name="ParentNode">The Parent Node of the Node to be inserted.</param>
    ''' <remarks>Only called from Dynamic update code when the Parent Node is known (ie displayed).</remarks>
    Private Sub InsertNode(ByVal Node As TreeNode, ByVal ParentNode As TreeNode)    '6/25/2012
        Dim Item As CShItem = Node.Tag
        'It is possible that the ParentNode has only a dummy node. Since we are adding a Node,
        'it is necessary to remove that dummy, beforehand. Note that this case cannot occur if all
        'prior references to the ParentNode occur only within ExpTree. In that case, ParentNode.Tag.Directories will not have
        'been Initialized so no Create or Rename messages will be passed to ExpTree - thus no InsertNode call.
        If ParentNode.Nodes.Count = 1 AndAlso ParentNode.Nodes(0).Text.Equals(" : ") Then
            PopulateNode(ParentNode)        '8/26/2012 - PopulateNode will insert the node correctly
        Else                                '8/26/2012 - Otherwise Insert Node in correct location
            For i As Integer = 0 To ParentNode.Nodes.Count - 1
                If DirectCast(ParentNode.Nodes(i).Tag, CShItem).CompareTo(Item) > 0 Then
                    ParentNode.Nodes.Insert(i, Node)
                    Exit Sub
                End If
            Next
            'on fall thru, did not find a spot to insert, so it goes at the end
            ParentNode.Nodes.Add(Node)
        End If                              '8/26/2012
    End Sub

    ''' <summary>
    ''' Sorts the Nodes of the input TreeNode
    ''' </summary>
    ''' <param name="N">The Node whose Nodes.Collection is to be sorted</param>
    ''' <remarks></remarks>
    Private Sub SortNodes(ByVal N As TreeNode)
        If N.Nodes.Count > 1 Then     'if not, why sort
            Dim tmp(N.Nodes.Count - 1) As TreeNode
            N.Nodes.CopyTo(tmp, 0)
            Array.Sort(tmp, New TagComparer)
            'tv1.BeginUpdate()      '6/18/2012 - not needed already in BeginUpdate when this rtn called
            N.Nodes.Clear()
            N.Nodes.AddRange(tmp)
            'tv1.EndUpdate()        '6/18/2012 - not needed already in BeginUpdate when this rtn called
        End If
    End Sub
#End Region

    ''' <summary>
    ''' NodePath returns the Text version of the full path of a TreeNode.
    ''' </summary>
    ''' <param name="node">The TreeNode to return the full path for.</param>
    ''' <returns>The full path to the input node within a tree</returns>
    ''' <remarks>Used only for some Debug.WriteLine statements.</remarks>
    Private Function NodePath(ByVal node As TreeNode) As String
        Dim pathlist As New List(Of TreeNode)
        pathlist.Add(node)
        Do While node.Parent IsNot Nothing
            pathlist.Add(node.Parent)
            node = node.Parent
        Loop
        pathlist.Reverse()
        Dim SB As New StringBuilder
        For Each N As TreeNode In pathlist
            SB.Append(N.Text) : SB.Append("\")
        Next
        Return SB.ToString
    End Function

    Public Event TreeKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

    Private Sub tv1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tv1.KeyDown
        '    Debug.WriteLine("KeyDown in ExpTree Char = " & e.KeyData.ToString)
        If e.KeyData = Keys.Escape Then e.Handled = True
        RaiseEvent TreeKeyDown(sender, e)
    End Sub
End Class
