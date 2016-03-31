Imports ExpTreeLib
Imports ExpTreeLib.CShItem
Imports ExpTreeLib.SystemImageListManager
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI
Imports ExpTreeLib.ShellDll.ShellHelper
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.IO
Imports System.Text
Imports System.Threading

Public Class wFolderBrowser
    Inherits wFormBase

    ''' <summary>
    ''' Saves the last selected FolderBrowser Item
    ''' </summary>
    Private LastSelectedCSI As CShItem

    'avoid Globalization problem-- an empty timevalue
    Private EmptyTimeValue As New DateTime(1, 1, 1, 0, 0, 0)

    'Flag for NewMenu processing of "New" item
    Private m_CreateNew As Boolean = False

    ''' <summary>
    ''' Saves the current working Directory.
    ''' </summary>
    Private sWorkingDirectoryPath As String

    ''' <summary>
    ''' Window-Ready variable
    ''' </summary>
    Private bReady As Boolean = False

    ''' <summary>
    ''' Setup a tool-tip for additional information.
    ''' </summary>
    Private ttToolTip As New ToolTip

#Region "Public Properties"
    ''' <summary>
    ''' InitialLoadLimit is a the number of lv1.Items whose IconIndex will we fetched on initial load
    ''' the balance will be fetched AFTER lv1 shows its initial display
    ''' </summary>
    <Browsable(True), Category("Misc"), _
    Description("Maximum # of Items to build in GUI Thread for initial display"), _
    DefaultValue(32)> _
    Public Property InitialLoadLimit() As Integer
        Get
            Return _InitialLoadLimit
        End Get
        Set(ByVal value As Integer)
            _InitialLoadLimit = value
        End Set
    End Property
    Private _InitialLoadLimit As Integer = 32

    ''' <summary>
    ''' WorkUpdateInterval is the Maximum # of Items to build in each 
    ''' BackGroundWorker Progress Interval before reporting them back to the GUI.
    ''' </summary>
    ''' <remarks>If there are 200 items to show, the first InitialLoadLimit will be built and
    '''          displayed in the GUI thread. The balance will be built in the BackgroundWorker
    '''          thread and reported back to the GUI in chunks of WorkUpdateInterval Items.</remarks>
    <Browsable(True), Category("Misc"), _
    Description("Maximum # of Items in each Background update interval"), _
    DefaultValue(100)> _
    Public Property WorkUpdateInterval() As Integer
        Get
            Return _WorkUpdateInterval
        End Get
        Set(ByVal value As Integer)
            _WorkUpdateInterval = value
        End Set
    End Property
    Private _WorkUpdateInterval As Integer = 100
#End Region

#Region "Form Load and Close"
    ''' <summary>
    ''' On Load: Select the last used path.
    ''' </summary>
    Private Sub wFolderSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize FileTreeList
        Me.dbDictionaryBrowser.RootItem = ExpTreeLib.CShItem.GetDeskTop

        '### Load Last Settings, if they are still valid.

        ' Load StartupPath
        Dim StartupPath As String = My.Settings.LastSelectedPath

        ' Try Several alternative Paths, if the Path from the setting does not exist!
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        End If
        If Not IO.Directory.Exists(StartupPath) Then
            StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
        End If

        Dim StartupPathItem As ExpTreeLib.CShItem = ExpTreeLib.CShItem.GetCShItem(StartupPath)
        If Not StartupPathItem Is Nothing Then
            Me.dbDictionaryBrowser.ExpandANode(StartupPathItem)
            Me.sWorkingDirectoryPath = StartupPath
        End If

        ' Save Settings.
        My.Settings.LastSelectedPath = StartupPath
        'cGlobal.SaveSettings()

        ' Add Folder-Update Handler
        AddHandler CShItemUpdate, AddressOf UpdateInvoke        '7/1/2012

        ' Set the button label
        Me.btnBrowseFolder.Text = My.Resources.rFolderBrowser.OpenButton_CurrentFolder

        ' Set tool-tip
        With Me.ttToolTip
            .SetToolTip(Me.btnBrowseFolder, My.Resources.rFolderBrowser.ToolTip_OpenButton)
        End With

        ' Create the folder-history menu.
        Me.RefreshFolderHistory()

        Me.bReady = True

#If DEBUG Then
        ' open automatically the data-browser in debug-mode, to save some time.
        Me.btnBrowseFolder_Click()
#End If
    End Sub

    Private Sub wFolderSelector_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler CShItemUpdate, AddressOf UpdateInvoke '7/1/2012
    End Sub
#End Region

#Region "Set Working Directory!"

    ''' <summary>
    ''' Sets the given path as current working directory, and
    ''' saves the value in the settings.
    ''' </summary>
    Private Sub SetWorkingDirectory(Path As String)
        If Me.bReady Then
            Me.sWorkingDirectoryPath = Path
            ' Save Settings.
            My.Settings.LastSelectedPath = Path
            cGlobal.SaveSettings()
        End If
    End Sub
#End Region

#Region "   Dynamic Update Handler"
    ''' <summary>
    ''' Refresh the view on click!
    ''' </summary>
    Private Sub mnuRefreshView_Click(sender As Object, e As EventArgs) Handles mnuRefreshView.Click
        If Me.dbDictionaryBrowser.SelectedItem Is Nothing Then Return
        UpdateInvoke(Me.dbDictionaryBrowser.SelectedItem, New ShellItemUpdateEventArgs(Me.dbDictionaryBrowser.SelectedItem, CShItemUpdateType.Updated))
    End Sub

    ''' <summary>
    ''' To receive notification of changes to the FileSystem which may affect the GUI display, declare
    ''' DeskTopItem WithEvents. Changes to CShItem's internal tree which are caused by notification of 
    ''' FileSystem changes or by a refresh of the contents of the internal tree raise CShItemUpdate
    ''' events.  For possible future changes, we check to see if an Invoke is required or not.
    ''' </summary>
    ''' <remarks></remarks>
    Private Delegate Sub InvokeUpdate(ByVal sender As Object, ByVal e As ShellItemUpdateEventArgs)

    'Private WithEvents DeskTopItem As CShItem = CShItem.GetDeskTop '7/1/2012

    Private m_InvokeUpdate As New InvokeUpdate(AddressOf DoItemUpdate)

    ''' <summary>
    ''' Returns the last CShItem Selected.
    ''' </summary>
    Public ReadOnly Property SelectedItem() As CShItem
        Get
            Return LastSelectedCSI
        End Get
    End Property

    ''' <summary>
    ''' Determines if DoItemUpdate should be called directly or via Invoke, and then calls it.
    ''' </summary>
    ''' <param name="sender">The CShItem of the Folder of the changed item.</param>
    ''' <param name="e">Contains information about the type of change and items affected.</param>
    ''' <remarks>Responds to events raised by either WM_Notify messages or FileWatch.</remarks>
    Private Sub UpdateInvoke(ByVal sender As Object, ByVal e As ShellItemUpdateEventArgs) '7/1/2012 removed Handles clause
        Return ' DISABLED!!!
        If Me.InvokeRequired Then
            Invoke(m_InvokeUpdate, sender, e)
        Else
            DoItemUpdate(sender, e)
        End If
    End Sub
    ''' <summary>
    ''' Makes changes in lv1 GUI in response to updating events raised by CShItem.
    ''' </summary>
    ''' <param name="sender">The CShItem of the Folder of the changed item.</param>
    ''' <param name="e">Contains information about the type of change and items affected.</param>
    ''' <remarks>Responds to events raised by WM_Notify messages. </remarks>
    Private Sub DoItemUpdate(ByVal sender As Object, ByVal e As ShellItemUpdateEventArgs)
        Dim Parent As CShItem = DirectCast(sender, CShItem)
        If Parent Is LastSelectedCSI Then ' 6/11/2012 - OrElse (e.Item Is LastSelectedCSI AndAlso e.UpdateType = CShItemUpdateType.Updated) Then   'If not, then of no interest to us
            Try
                lv1.BeginUpdate()
                Select Case e.UpdateType
                    Case CShItem.CShItemUpdateType.Created
                        Dim lvi As ListViewItem = MakeSparceLVItem(e.Item)
                        lvi.ImageIndex = e.Item.IconIndexNormal
                        RefreshLvi(lvi)
                        InsertLvi(lvi, lv1)     '6/11/2012
                        If m_CreateNew Then
                            m_CreateNew = False
                            lvi.BeginEdit()
                        End If
                    Case CShItemUpdateType.Deleted
                        Dim lvi As ListViewItem = FindLVItem(e.Item)
                        If lvi IsNot Nothing Then
                            lv1.Items.Remove(lvi)
                        End If
                    Case CShItemUpdateType.Renamed
                        Dim lvi As ListViewItem = FindLVItem(e.Item)
                        If lvi IsNot Nothing Then
                            If e.Item.Parent IsNot LastSelectedCSI Then     'if true = item renamed to different directory
                                lv1.Items.Remove(lvi)
                            Else
                                RefreshLvi(lvi)
                                e.Item.ResetIconIndex()                       'may have changed
                                lvi.ImageIndex = e.Item.IconIndexNormal
                                lv1.Items.Remove(lvi)   '6/11/2012
                                InsertLvi(lvi, lv1)     '6/11/2012
                            End If
                        End If
                    Case CShItemUpdateType.UpdateDir  'in this case Parent/sender is the item of interest
                        ' CShItemUpdater, etc. will do the appropriate Adds and Removes, generating
                        ' Created/Deleted events that will occur before an UpdateDir event. There is
                        ' no need to do anything here.
                        'lv1.BeginUpdate()
                        'lv1.Sort()
                        'lv1.EndUpdate()

                    Case CShItemUpdateType.Updated
                        Dim lvi As ListViewItem = FindLVItem(e.Item)
                        If lvi IsNot Nothing Then
                            Dim indx As Integer = lv1.Items.IndexOf(lvi)
                            Dim newLVI As ListViewItem = MakeSparceLVItem(e.Item)
                            RefreshLvi(newLVI)
                            e.Item.ResetIconIndex()                       'may have changed
                            newLVI.ImageIndex = e.Item.IconIndexNormal
                            lv1.Items.RemoveAt(indx)
                            lv1.Items.Insert(indx, newLVI)
                        End If
                    Case CShItemUpdateType.IconChange
                        Dim lvi As ListViewItem = FindLVItem(e.Item)
                        If lvi IsNot Nothing Then
                            e.Item.ResetIconIndex()
                            lvi.ImageIndex = e.Item.IconIndexNormal
                        End If
                    Case CShItemUpdateType.MediaChange
                        Dim lvi As ListViewItem = FindLVItem(e.Item)
                        If lvi IsNot Nothing Then
                            RefreshLvi(lvi)
                            e.Item.ResetIconIndex()
                            lvi.ImageIndex = e.Item.IconIndexNormal
                        End If
                End Select

            Catch ex As Exception
                Debug.WriteLine("Error in frmThread -- lv1 updater -- " & ex.ToString)
            Finally
                lv1.EndUpdate()
            End Try
            ShowCounts()
        End If      'end of Parent Is LastSelectedCSI test
    End Sub

    Private Function FindLVItem(ByVal item As CShItem) As ListViewItem
        For Each lvi As ListViewItem In lv1.Items
            If lvi.Tag Is item Then
                Return lvi
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Given a ListViewItem with a  CShItem in its' Tag, and a ListView whose Items all have a CShItem in
    ''' their Tags, Insert the ListViewItem in its' proper place in the ListView.
    ''' </summary>
    ''' <param name="lvi">The ListViewItem to be inserted.</param>
    ''' <param name="LV">The ListView into which the ListViewItem is to be inserted.</param>
    ''' <remarks>6/11/2012 - better than a Sort when the list is in order.<br />
    '''          Will honor any prior Column Sorts.</remarks>
    Private Sub InsertLvi(ByVal lvi As ListViewItem, ByVal LV As ListView)
        Dim Item As CShItem = DirectCast(lvi.Tag, CShItem)
        If lv1.ListViewItemSorter IsNot Nothing AndAlso TypeOf lv1.ListViewItemSorter Is LVColSorter Then
            Dim CSorter As LVColSorter = DirectCast(lv1.ListViewItemSorter, LVColSorter)
            For i As Integer = 0 To LV.Items.Count - 1
                If CSorter.Compare(LV.Items(i), lvi) > 0 Then
                    LV.Items.Insert(i, lvi)
                    lvi.EnsureVisible()
                    Exit Sub
                End If
            Next
        Else
            For i As Integer = 0 To LV.Items.Count - 1
                If DirectCast(LV.Items(i).Tag, CShItem).CompareTo(Item) > 0 Then
                    LV.Items.Insert(i, lvi)
                    lvi.EnsureVisible()
                    Exit Sub
                End If
            Next
        End If
        'on fall thru, did not find a spot to insert, so it goes at then end
        LV.Items.Add(lvi)
        lvi.EnsureVisible()
    End Sub
#End Region

#Region "ExplorerTree Event Handling -- AfterNodeSelect"
    Private Sub AfterNodeSelect(ByVal pathName As String, ByVal CSI As CShItem) _
           Handles dbDictionaryBrowser.ExpTreeNodeSelected

        If LastSelectedCSI IsNot Nothing AndAlso LastSelectedCSI Is CSI Then Exit Sub
        Cursor = Cursors.WaitCursor
        tsslMiddle.Text = pathName
        Me.Text = My.Resources.rFolderBrowser.WindowTitle_Addon & pathName
        tsslLeft.Text = My.Resources.rFolderBrowser.UpdateNode_Status

        If BGW2 IsNot Nothing Then
            BGW2.CancelAsync()
            Event2.WaitOne()
        End If
        Dim TotalItems As Integer
        Dim combList As ArrayList = CSI.GetItems

        TotalItems = combList.Count
        If TotalItems > 0 Then
            'Build the ListViewItems & add to lv1
            lv1.BeginUpdate()
            lv1.Items.Clear()
            If LastSelectedCSI IsNot Nothing AndAlso LastSelectedCSI IsNot CSI Then
                LastSelectedCSI.ClearItems(True)
            End If
            lv1.Refresh()

            Dim InitialFillLim As Integer = Math.Min(combList.Count, InitialLoadLimit)
            Dim FirstLoad As New List(Of ListViewItem)(InitialFillLim)
            For i As Integer = 0 To InitialFillLim - 1

                '## Added 12/11/2014: hide SpectraFox generated file-extensions!
                If DirectCast(combList(i), CShItem).Text.EndsWith(".sfx") Then
                    Continue For
                End If
                '##########

                Dim lvi As ListViewItem = MakeSparceLVItem(DirectCast(combList(i), CShItem))
                RefreshLvi(lvi)
                lvi.ImageIndex = SystemImageListManager.GetIconIndex(DirectCast(combList(i), CShItem), False)
                FirstLoad.Add(lvi)
            Next
            lv1.Items.AddRange(FirstLoad.ToArray)

            'Fill the ListView with the remaining items without FileInfo or ICon
            Dim SparseLoad As New List(Of ListViewItem)(combList.Count - InitialFillLim)
            If combList.Count > InitialFillLim Then
                For i As Integer = InitialFillLim To combList.Count - 1

                    '## Added 12/11/2014: hide SpectraFox generated file-extensions!
                    If DirectCast(combList(i), CShItem).Text.EndsWith(".sfx") Then
                        Continue For
                    End If
                    '##########

                    Dim lvi As ListViewItem = MakeSparceLVItem(DirectCast(combList(i), CShItem))
                    RefreshLvi(lvi, True)
                    SparseLoad.Add(lvi)
                Next
                lv1.Items.AddRange(SparseLoad.ToArray)
            End If
            lv1.EndUpdate()

            If combList.Count > InitialLoadLimit Then
                LoadLV1(SparseLoad)
            End If
        Else
            lv1.Items.Clear()
            If LastSelectedCSI IsNot Nothing AndAlso LastSelectedCSI IsNot CSI Then
                LastSelectedCSI.ClearItems(True)
            End If
            tsslRight.Text = My.Resources.rFolderBrowser.FolderCount_FolderEmpty
        End If
        LastSelectedCSI = CSI
        lv1.Tag = LastSelectedCSI           '7/5/2012   For ClvDropWapper

        'Now that lv.ListViewItems has been set up (and MakeLvItem does attach the appropriate tags
        ' to both the ListViewItem and the appropriate SubItems), set the ListViewItemSorter
        lv1.ListViewItemSorter = New LVColSorter(lv1)
        tsslLeft.Text = My.Resources.rFolderBrowser.Status_Ready
        ShowCounts()
        Cursor = Cursors.Default

        SetWorkingDirectory(CSI.Path)
    End Sub

    ''' <summary>
    ''' Counts files and folders in the current directory.
    ''' </summary>
    Private Sub ShowCounts()
        If lv1.Items.Count > 0 Then
            Dim Dirs As Integer
            Dim Files As Integer
            For Each lvi As ListViewItem In lv1.Items
                Dim Item As CShItem = DirectCast(lvi.Tag, CShItem)
                If Item.IsFolder Then
                    Dirs += 1
                Else : Files += 1
                End If
            Next
            tsslRight.Text = My.Resources.rFolderBrowser.FolderCount_Status.Replace("%d", Dirs.ToString("N0")).Replace("%f", Files.ToString("N0"))
        Else
            tsslRight.Text = My.Resources.rFolderBrowser.FolderCount_FolderEmpty
        End If
    End Sub

#End Region

#Region "Visible Changed and lv1 HandleCreated"
    Private Sub lv1_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) _
                Handles lv1.HandleCreated
        SystemImageListManager.SetListViewImageList(lv1, False, False)
        SystemImageListManager.SetListViewImageList(lv1, True, False)
    End Sub

    Private Sub frmThread_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        SystemImageListManager.SetListViewImageList(lv1, False, False)
        SystemImageListManager.SetListViewImageList(lv1, True, False)
    End Sub
#End Region

#Region "   Various MakeLV routines"

    ''' <summary>
    ''' Creates a minimal ListViewItem from the input CShItem
    ''' </summary>
    ''' <param name="item">The CShItem that this ListViewItem represents</param>
    ''' <returns>A ListViewItem with .Tag and .Text filled, with empty SubItems for the balance of the SubItems required by the ListView.</returns>
    ''' <remarks>When it is timely to fill the remaining SubItems with data (after BGW2 has completed), call RefreshLvi.</remarks>
    Private Function MakeSparceLVItem(ByVal item As CShItem) As ListViewItem
        Dim lvi As New ListViewItem(item.DisplayName)
        With lvi
            .Tag = item
            For i As Integer = 1 To lv1.Columns.Count - 1
                .SubItems.Add("")
            Next
        End With
        Return lvi
    End Function

    ''' <summary>
    ''' Loads all of a ListViewItem's SubItems with values from the associated CShItem - obtained from the ListViewItem's .Tag.
    ''' Note that the CShItem's time sensitive values will be set by CShItem using a W32_FindData structure if it finds one in the
    ''' CShItem's .Tag - as set in this Form by a BackgroundWorker.
    ''' </summary>
    ''' <param name="lvi">The ListViewItem to be refreshed</param>
    ''' <param name="DeferSet">If True, Defer the filling of Length and Date information until later (in the BackgroundWorker).
    '''                        If False (the default) fill Length and Date information in this call.</param>
    ''' <remarks>For optimization, depends on BGW2 having filled CSI.W32Data with a W32Find_Data structure which CShItem will 
    '''          use for Length, Attributes, and Date information.</remarks>
    Private Sub RefreshLvi(ByVal lvi As ListViewItem, Optional ByVal DeferSet As Boolean = False)
        Dim CSI As CShItem = DirectCast(lvi.Tag, CShItem)
        'Set the Items that must come from a CShItem
        lvi.Text = CSI.Name
        lvi.SubItems(3).Text = CSI.TypeName

        If Not DeferSet Then    'Set the SubItems that may come from an W32Find_Data
            With lvi
                'Set Length
                If CSI.IsDisk OrElse (CSI.IsFileSystem And Not CSI.IsFolder) Then      'Not CSI.IsDisk And
                    If CSI.Length > 1024 Then
                        .SubItems(1).Text = (Format(CSI.Length / 1024, "#,### KB"))
                    Else
                        .SubItems(1).Text = (Format(CSI.Length, "##0 Bytes"))
                    End If
                    .SubItems(1).Tag = CSI.Length
                Else
                    '.SubItems(1) already has been correctly set to blank entry
                    'But, to make LVColSorter work correctly, then we have to Set the .Tag to 0 (really)
                    .SubItems(1).Tag = 0L
                End If
                'Set LastWriteTime
                If CSI.IsDisk OrElse CSI.LastWriteTime = EmptyTimeValue Then '"#1/1/0001 12:00:00 AM#" is empty
                    '.SubItems(2) already has been correctly set to blank entry
                    'But, to make LVColSorter work correctly, then we have to Set the .Tag to EmptyTimeValue
                    ' (Not really in this case, but it is good to do in the general case)
                    .SubItems(2).Tag = EmptyTimeValue
                Else
                    .SubItems(2).Text = CSI.LastWriteTime.ToString("MM/dd/yyyy HH:mm:ss")
                    .SubItems(2).Tag = CSI.LastWriteTime
                End If
                'Set Attributes
                If Not CSI.IsDisk And CSI.IsFileSystem Then
                    Dim SB As New StringBuilder()
                    Try
                        Dim attr As FileAttributes = CSI.Attributes
                        If (attr And FileAttributes.System) = FileAttributes.System Then SB.Append("S")
                        If (attr And FileAttributes.Hidden) = FileAttributes.Hidden Then SB.Append("H")
                        If (attr And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then SB.Append("R")
                        If (attr And FileAttributes.Archive) = FileAttributes.Archive Then SB.Append("A")
                    Catch
                    End Try
                    .SubItems(4).Text = SB.ToString
                Else
                    '.SubItems(4) already has been correctly set to blank entry
                    'But, to make LVColSorter work correctly, then we have to Set the .Tag to EmptyTimeValue
                    ' (Not really in this case, but it is good to do in the general case)
                    .SubItems(4).Tag = EmptyTimeValue
                End If
                'Set CreationTime
                If CSI.IsDisk OrElse CSI.CreationTime = EmptyTimeValue Then '"#1/1/0001 12:00:00 AM#" is empty
                    '.SubItems(5) already has been correctly set to blank entry
                    'But, to make LVColSorter work correctly, then we have to Set the .Tag to EmptyTimeValue
                    ' (Not really in this case, but it is good to do in the general case)
                    .SubItems(5).Tag = EmptyTimeValue
                Else
                    .SubItems(5).Text = CSI.CreationTime.ToString("MM/dd/yyyy HH:mm:ss")
                    .SubItems(5).Tag = CSI.CreationTime
                End If
            End With
        End If
    End Sub

#End Region

#Region "   The Background worker"
    Private WithEvents BGW2 As BackgroundWorker
    Private Event2 As New ManualResetEvent(True)
    Private InBkground As Integer = 0
    Private ItemInfo As Dictionary(Of String, W32Find_Data)
    Private Sub LoadLV1(ByVal ListToDo As List(Of ListViewItem))
        If ListToDo IsNot Nothing AndAlso ListToDo.Count > 0 Then
            Event2.Reset()
            BGW2 = New BackgroundWorker
            tsslLeft.Text = "Loading Info"
            With BGW2
                .WorkerReportsProgress = True
                .WorkerSupportsCancellation = True
                InBkground = 0
                .RunWorkerAsync(ListToDo)
            End With
        End If
    End Sub

    '7/8/2012 - Routine modified to correct a potential deadlock
    Private Sub BGW2_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BGW2.DoWork
        Dim ThisWorker As BackgroundWorker = DirectCast(sender, BackgroundWorker)
        Dim WorkList As List(Of ListViewItem) = DirectCast(e.Argument, List(Of ListViewItem))
        Dim Results As New List(Of ListViewItem)(WorkUpdateInterval)
        Dim Item As CShItem = DirectCast(WorkList(0).Tag, CShItem).Parent
        If Item.IsFileSystem AndAlso WorkList.Count > 0 Then
            GetItemDatas(Item, WorkList)
        End If
        For i As Integer = 0 To WorkList.Count - 1
            If ThisWorker.CancellationPending Then
                e.Cancel = True : Results = Nothing
                Exit For
            End If
            Item = DirectCast(WorkList(i).Tag, CShItem)
            'Force fetch of IconIndex 
            Dim tmp As Integer = Item.IconIndexNormal
            Results.Add(WorkList(i))
            If Results.Count = WorkUpdateInterval Then
                If ThisWorker.CancellationPending Then
                    e.Cancel = True : Results = Nothing
                    Exit For
                End If
                ThisWorker.ReportProgress(i, Results)
                Results = New List(Of ListViewItem)(WorkUpdateInterval)
            End If
        Next
        Event2.Set()
        e.Result = Results
    End Sub

    Private Sub BGW2_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BGW2.ProgressChanged
        Dim Items As List(Of ListViewItem) = DirectCast(e.UserState, List(Of ListViewItem))
        For Each Lvi As ListViewItem In Items
            SetLvi(Lvi)
            InBkground += 1
        Next
    End Sub

    Private Sub BGW2_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BGW2.RunWorkerCompleted
        If Not e.Cancelled AndAlso e.Error Is Nothing Then
            If e.Result IsNot Nothing Then
                Dim Items As List(Of ListViewItem) = DirectCast(e.Result, List(Of ListViewItem))
                For Each Lvi As ListViewItem In Items
                    SetLvi(Lvi)
                    InBkground += 1
                Next
            End If
        End If
        Event2.Set()
        BGW2.Dispose()          '7/8/2012
        BGW2 = Nothing
    End Sub

    Private Sub GetItemDatas(ByVal BaseCSI As CShItem, ByVal LVIList As List(Of ListViewItem))
        ItemInfo = New Dictionary(Of String, W32Find_Data)(LVIList.Count)
        Try
            GetInfos(BaseCSI)
        Catch ex As ApplicationException    'only occurs on Invalid Handle - we have nothing to do
            Exit Sub
        End Try
        For Each Lvi As ListViewItem In LVIList
            Dim CSI As CShItem = DirectCast(Lvi.Tag, CShItem)
            Dim csiName As String = IO.Path.GetFileName(CSI.Path)
            If CSI.IsFolder Then
                csiName = csiName & "\"
            End If
            If ItemInfo.ContainsKey(csiName) Then
                CSI.W32Data = ItemInfo(csiName)
            Else
#If DEBUG Then
                Throw New ArgumentException("No ItemData for " & CSI.Path)
#End If
            End If
        Next
    End Sub

    Private Sub GetInfos(ByVal CSI As CShItem)

        Dim DirName As String = CSI.Path
        Dim Data As New W32Find_Data(DirName)

        Dim Handle As SafeFindHandle = Nothing

        Handle = FindFirstFile(DirName & "\*", Data)
        If Handle.IsInvalid Then
            Debug.WriteLine("Invalid Handle for " & CSI.Path)
            Throw New ApplicationException("Invalid FindFileHandle returned for " & DirName)
        End If
        Dim HR As Boolean = True
        While (HR)
            If (Data.dwFileAttributes And FileAttributes.Directory) <> 0 Then
                If Not Data.cFileName.StartsWith(".") Then
                    ItemInfo.Add(Data.Name & "\", Data)
                End If
            Else
                ItemInfo.Add(Data.Name, Data)
            End If
            Data = New W32Find_Data(DirName)
            HR = FindNextFile(Handle, Data)
        End While
        Handle.Close()
    End Sub

    Private Sub SetLvi(ByVal Lvi As ListViewItem)
        Dim CSI As CShItem = DirectCast(Lvi.Tag, CShItem)
        Lvi.ImageIndex = CSI.IconIndexNormal
        RefreshLvi(Lvi)
    End Sub

#End Region

#Region "   lv1_DoubleClick"

    ''' <summary>
    ''' Open the selected folder in the foldertree with a double click.
    ''' </summary>
    Private Sub lv1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv1.DoubleClick
        Dim csi As CShItem = DirectCast(lv1.SelectedItems(0).Tag, CShItem)
        If csi.IsFolder Then
            Me.dbDictionaryBrowser.ExpandANode(csi)
            SetWorkingDirectory(csi.Path)
        Else
            Try
                Process.Start(csi.Path)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Error in starting application")
            End Try
        End If
    End Sub

#End Region

#Region "   LabelEdit Handlers (Item Rename) From Calum"
    ''' <summary>
    ''' Handles Before Item Rename for lv1
    ''' </summary>
    ''' <remarks>Modified version of Calum McLellan's code from ExpList.</remarks>
    Private Sub lv1_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles lv1.BeforeLabelEdit
        Dim item As CShItem = DirectCast(lv1.Items(e.Item).Tag, CShItem)
        If (Not item.IsFileSystem) Or item.IsDisk Or _
            item.Path = CShItem.GetCShItem(CSIDL.MYDOCUMENTS).Path Or _
            Not (item.CanRename) Then
            System.Media.SystemSounds.Beep.Play()
            e.CancelEdit = True
        End If
    End Sub

    ''' <summary>
    ''' Handles After Item Rename for lv1
    ''' </summary>
    ''' <remarks>Modified version of Calum McLellan's code from ExpList.</remarks>
    Private Sub lv1_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles lv1.AfterLabelEdit
        Dim item As CShItem = DirectCast(lv1.Items(e.Item).Tag, CShItem)
        Dim NewName As String
        Dim index As Integer
        Dim path As String
        If e.Label Is Nothing OrElse e.Label = String.Empty Then Exit Sub '6/11/2012
        Try
            NewName = e.Label.Trim

            If NewName.Length < 1 OrElse NewName.IndexOfAny(System.IO.Path.GetInvalidPathChars) <> -1 Then
                e.CancelEdit = True
                System.Media.SystemSounds.Beep.Play()
                Exit Sub
            End If

            path = item.Path

            index = path.LastIndexOf("\"c)
            If index = -1 Then
                e.CancelEdit = True
                System.Media.SystemSounds.Beep.Play()
                Exit Sub
            End If

            Dim newPidl As IntPtr = IntPtr.Zero
            If item.Parent.Folder.SetNameOf(CInt(lv1.Handle), CShItem.ILFindLastID(item.PIDL), NewName, SHGDN.NORMAL, newPidl) = S_OK Then
            Else
                System.Media.SystemSounds.Beep.Play()
                e.CancelEdit = True
            End If
        Catch ex As Exception
            e.CancelEdit = True
            System.Media.SystemSounds.Beep.Play()
            Exit Sub
        End Try
    End Sub
#End Region

#Region "   Context Menu Handlers"
    Private m_WindowsContextMenu As WindowsContextMenu = New WindowsContextMenu

    Private Function IsWithin(ByVal Ctl As Control, ByVal e As MouseEventArgs) As Boolean
        IsWithin = False            'default to Not Within
        If e.X < 0 OrElse e.Y < 0 Then Exit Function
        Dim CR As Rectangle = Ctl.ClientRectangle
        If e.X > CR.Width OrElse e.Y > CR.Height Then Exit Function
        IsWithin = True
    End Function
    ''' <summary>
    ''' Sort the ListViewItems based on the CShItems stored in the .Tag of each ListViewItem.
    ''' </summary>
    ''' <remarks>Cannot use LVColSorter for this since we do not know current state
    ''' </remarks>
    Private Sub SortLVItems()
        With lv1
            If .Items.Count < 2 Then Exit Sub 'no point in sorting 0 or 1 items
            .BeginUpdate()
            Dim tmp(.Items.Count - 1) As ListViewItem
            .Items.CopyTo(tmp, 0)
            Array.Sort(tmp, New TagComparer)
            .Items.Clear()
            .Items.AddRange(tmp)
            .EndUpdate()
        End With
    End Sub

    ''' <summary>
    ''' m_OutOfRange is set to True on lv1.MouseLeave (which happens under many circumstances) to prevent
    ''' the non-ListViewItem specific menu from firing. See Remarks
    ''' m_OutOfRange is set to False (allowing ContextMenus in lv1), only on lv1.MouseDown when the Right
    ''' button is pressed. MouseDown only occurs when the Mouse is really over lv1.
    ''' </summary>
    ''' <remarks>
    '''If you hold down the right mouse button, then leave lv1,
    ''' then let go of the mouse button, the MouseUp event is fired upon
    ''' re-entering the lv1 - meaning that the Windows ContextMenu will
    ''' be shown if we don't use this flag (from Calum)
    '''</remarks>
    Private m_OutOfRange As Boolean

    Private Sub lv1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv1.MouseLeave
        m_OutOfRange = True
    End Sub

    Private Sub lv1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lv1.MouseDown
        If e.Button = MouseButtons.Right Then
            m_OutOfRange = False
        End If
    End Sub

    ''' <summary>
    ''' Handles RightButton Click
    ''' </summary>
    ''' <remarks>Modified version of Calum McLellan's code from ExpList.</remarks>
    Private Sub lv1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lv1.MouseUp
        If e.Button = MouseButtons.Right Then
            If Not IsWithin(lv1, e) Then Exit Sub
            If m_OutOfRange Then Exit Sub
            Dim lvi As ListViewItem
            Dim pt As New System.Drawing.Point(e.X, e.Y)
            lvi = lv1.GetItemAt(e.X, e.Y)
            If Not IsNothing(lvi) AndAlso lv1.SelectedItems.Count > 0 Then
                Dim itms(lv1.SelectedItems.Count - 1) As CShItem
                For i As Integer = 0 To lv1.SelectedItems.Count - 1
                    itms(i) = DirectCast(lv1.SelectedItems(i).Tag, CShItem)
                Next
                Dim cmi As CMInvokeCommandInfoEx = Nothing
                Dim allowRename As Boolean = True          'Don't allow rename of more than 1 item
                If lv1.SelectedItems.Count > 1 Then allowRename = False
                If m_WindowsContextMenu.ShowMenu(Me.Handle, itms, MousePosition, allowRename, cmi) Then
                    'Check for rename
                    Dim cmdBytes(256) As Byte
                    m_WindowsContextMenu.winMenu.GetCommandString(cmi.lpVerb.ToInt32, GCS.VERBA, 0, cmdBytes, 256)

                    Dim cmdName As String = szToString(cmdBytes).ToLower
                    If cmdName.Equals("rename") Then
                        lv1.LabelEdit = True
                        lvi.BeginEdit()
                    Else
                        m_WindowsContextMenu.InvokeCommand(m_WindowsContextMenu.winMenu, CUInt(cmi.lpVerb), itms(0).Parent.Path, pt)
                    End If
                    Marshal.ReleaseComObject(m_WindowsContextMenu.winMenu)
                End If
            Else
                GetFolderMenu(MousePosition)
            End If
        End If
    End Sub

#Region "           Windows Folder ContextMenu "

    Private Sub GetFolderMenu(ByVal pt As Drawing.Point)
        Dim HR As Integer
        Dim min As Integer = 1
        Dim max As Integer = 100000
        Dim cmi As New CMInvokeCommandInfoEx
        Dim comContextMenu As IntPtr = CreatePopupMenu()
        Dim viewSubMenu As IntPtr = CreatePopupMenu()

        'Check item count - should always be 0 but check just in case
        Dim startIndex As Integer = GetMenuItemCount(comContextMenu.ToInt32)
        'Fill the context menu
        Dim itemInfo As New MENUITEMINFO("View")
        itemInfo.fMask = MIIM.SUBMENU Or MIIM.STRING
        itemInfo.hSubMenu = viewSubMenu
        InsertMenuItem(comContextMenu, 0, True, itemInfo)
        Dim checked As Integer = MFT.BYCOMMAND
        If lv1.View = View.Tile Then checked = MFT.RADIOCHECK Or MFT.CHECKED
        AppendMenu(viewSubMenu, checked, CMD.TILES, "Tiles")
        checked = MFT.BYCOMMAND
        If lv1.View = View.LargeIcon Then checked = MFT.RADIOCHECK Or MFT.CHECKED
        AppendMenu(viewSubMenu, checked, CMD.LARGEICON, "Large Icons")
        checked = MFT.BYCOMMAND
        If lv1.View = View.List Then checked = MFT.RADIOCHECK Or MFT.CHECKED
        AppendMenu(viewSubMenu, checked, CMD.LIST, "List")
        checked = MFT.BYCOMMAND
        If lv1.View = View.Details Then checked = MFT.RADIOCHECK Or MFT.CHECKED
        AppendMenu(viewSubMenu, checked, CMD.DETAILS, "Details")
        checked = MFT.BYCOMMAND

        AppendMenu(comContextMenu, MFT.SEPARATOR, 0, String.Empty)
        AppendMenu(comContextMenu, MFT.BYCOMMAND, CMD.REFRESH, "Refresh")
        AppendMenu(comContextMenu, MFT.SEPARATOR, 0, String.Empty)

        Dim enabled As Integer = MFT.GRAYED
        Dim effects As DragDropEffects
        If LastSelectedCSI Is Nothing Then
            enabled = MFT.BYCOMMAND
        Else
            effects = ShellHelper.CanDropClipboard(LastSelectedCSI)
            If ((effects And DragDropEffects.Copy) = DragDropEffects.Copy) Or _
                    ((effects And DragDropEffects.Move) = DragDropEffects.Move) Then ' Enable paste for stand-alone ExpList
                enabled = MFT.BYCOMMAND
            End If
        End If
        AppendMenu(comContextMenu, enabled, CMD.PASTE, "Paste")

        If LastSelectedCSI IsNot Nothing Then
            enabled = MFT.GRAYED
            If ((effects And DragDropEffects.Link) = DragDropEffects.Link) Then
                enabled = MFT.BYCOMMAND
            End If

            AppendMenu(comContextMenu, enabled, CMD.PASTELINK, _
                    "Paste Link")
            AppendMenu(comContextMenu, MFT.SEPARATOR, 0, String.Empty)

            ' Add the 'New' menu
            If LastSelectedCSI.IsFolder And _
                ((Not LastSelectedCSI.Path.StartsWith("::")) Or (LastSelectedCSI Is CShItem.GetDeskTop)) Then
                Dim xIndex As Integer = GetMenuItemCount(CInt(comContextMenu))
                m_WindowsContextMenu.SetUpNewMenu(LastSelectedCSI, comContextMenu, xIndex) ' 6) ' 7)
                AppendMenu(comContextMenu, MFT.SEPARATOR, 0, String.Empty)
            End If
            AppendMenu(comContextMenu, MFT.BYCOMMAND, CMD.PROPERTIES, _
                    "Properties")
        End If

        Dim cmdID As Integer = _
            TrackPopupMenuEx(comContextMenu, TPM.RETURNCMD, _
            pt.X, pt.Y, Me.Handle, IntPtr.Zero)


        If cmdID >= min Then
            cmi = New CMInvokeCommandInfoEx
            cmi.cbSize = Marshal.SizeOf(cmi)
            cmi.nShow = SW.SHOWNORMAL
            cmi.fMask = CMIC.UNICODE Or CMIC.PTINVOKE
            cmi.ptInvoke = New Drawing.Point(pt.X, pt.Y)

            Select Case cmdID
                Case CMD.TILES
                    lv1.View = View.Tile
                    GoTo CLEANUP
                Case CMD.LARGEICON
                    lv1.View = View.LargeIcon
                    GoTo CLEANUP
                Case CMD.LIST
                    lv1.View = View.List
                    GoTo CLEANUP
                Case CMD.DETAILS
                    lv1.View = View.Details
                    GoTo CLEANUP
                Case CMD.REFRESH
                    If LastSelectedCSI IsNot Nothing Then
                        LastSelectedCSI.UpdateRefresh()
                    End If
                    SortLVItems()
                    GoTo CLEANUP
                Case CMD.PASTE
                    If LastSelectedCSI IsNot Nothing Then
                        cmi.lpVerb = Marshal.StringToHGlobalAnsi("paste")
                        cmi.lpVerbW = Marshal.StringToHGlobalUni("paste")
                    Else
                        GoTo CLEANUP
                    End If
                Case CMD.PASTELINK
                    cmi.lpVerb = Marshal.StringToHGlobalAnsi("pastelink")
                    cmi.lpVerbW = Marshal.StringToHGlobalUni("pastelink")
                Case CMD.PROPERTIES
                    cmi.lpVerb = Marshal.StringToHGlobalAnsi("properties")
                    cmi.lpVerbW = Marshal.StringToHGlobalUni("properties")
                Case Else
                    If CShItem.IsVista Then cmdID -= 1 '12/15/2010 Change
                    cmi.lpVerb = CType(cmdID, IntPtr)
                    cmi.lpVerbW = CType(cmdID, IntPtr)
                    m_CreateNew = True
                    HR = m_WindowsContextMenu.newMenu.InvokeCommand(cmi)
#If DEBUG Then
                    If HR <> S_OK Then
                        Marshal.ThrowExceptionForHR(HR)
                    End If
#End If

                    GoTo CLEANUP
            End Select

            ' Invoke the Paste, Paste Shortcut or Properties command
            If LastSelectedCSI IsNot Nothing Then
                Dim prgf As Integer = 0
                Dim iunk As IntPtr = IntPtr.Zero
                Dim folder As ShellDll.IShellFolder = Nothing
                If LastSelectedCSI Is CShItem.GetDeskTop Then
                    folder = LastSelectedCSI.Folder
                Else
                    folder = LastSelectedCSI.Parent.Folder
                End If

                Dim relPidl As IntPtr = CShItem.ILFindLastID(LastSelectedCSI.PIDL)
                HR = folder.GetUIObjectOf(IntPtr.Zero, 1, New IntPtr() {relPidl}, IID_IContextMenu, prgf, iunk)
#If DEBUG Then
                If Not HR = S_OK Then
                    Marshal.ThrowExceptionForHR(HR)
                End If
#End If

                m_WindowsContextMenu.winMenu = CType(Marshal.GetObjectForIUnknown(iunk), IContextMenu)
                HR = m_WindowsContextMenu.winMenu.InvokeCommand(cmi)
                m_WindowsContextMenu.ReleaseMenu()

#If DEBUG Then
                If Not HR = S_OK Then
                    Marshal.ThrowExceptionForHR(HR)
                End If
#End If
            End If
        End If      '12/15/2010 change
CLEANUP:
        m_WindowsContextMenu.ReleaseNewMenu()

        Marshal.Release(comContextMenu)
        comContextMenu = IntPtr.Zero
        Marshal.Release(viewSubMenu)
        viewSubMenu = IntPtr.Zero

    End Sub

#End Region

    ''' <summary>
    ''' Handles Windows Messages having to do with the display of Cascading menus of the Context Menu.
    ''' </summary>
    ''' <param name="m">The Windows Message</param>
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        'For send to menu in the ListView context menu
        Dim hr As Integer = 0
        If m.Msg = WM.INITMENUPOPUP Or m.Msg = WM.MEASUREITEM Or m.Msg = WM.DRAWITEM Then
            If Not m_WindowsContextMenu.winMenu2 Is Nothing Then
                hr = m_WindowsContextMenu.winMenu2.HandleMenuMsg(m.Msg, m.WParam, m.LParam)
                If hr = 0 Then
                    Return
                End If
            ElseIf (m.Msg = WM.INITMENUPOPUP And m.WParam = m_WindowsContextMenu.newMenuPtr) _
                    Or m.Msg = WM.MEASUREITEM Or m.Msg = WM.DRAWITEM Then
                If Not m_WindowsContextMenu.newMenu2 Is Nothing Then
                    hr = m_WindowsContextMenu.newMenu2.HandleMenuMsg(m.Msg, m.WParam, m.LParam)
                    If hr = 0 Then
                        Return
                    End If
                End If
            End If
        ElseIf m.Msg = WM.MENUCHAR Then
            If Not m_WindowsContextMenu.winMenu3 Is Nothing Then
                hr = m_WindowsContextMenu.winMenu3.HandleMenuMsg2(m.Msg, m.WParam, m.LParam, IntPtr.Zero)
                If hr = 0 Then
                    Return
                End If
            End If
        End If
        MyBase.WndProc(m)
    End Sub
#End Region

#Region "Open the Folder Browser for the selected path"
    ''' <summary>
    ''' Open the Folder Browser for the selected path or the selected files
    ''' </summary>
    Private Sub btnBrowseFolder_Click() Handles btnBrowseFolder.Click

        ' Get a list of files to filter.
        Dim FilterFileList As New List(Of String)
        For i As Integer = 0 To Me.lv1.SelectedItems.Count - 1 Step 1
            'If Not DirectCast(Me.lv1.SelectedItems(i).Tag, CShItem).IsFolder Then
            FilterFileList.Add(Me.lv1.SelectedItems(i).Text)
            'End If
        Next
        If FilterFileList.Count = 0 Then FilterFileList = Nothing

        ' make sure the folder-history array exists
        If My.Settings.LastBrowsedFolders Is Nothing Then My.Settings.LastBrowsedFolders = New Specialized.StringCollection()

        My.Settings.LastBrowsedFolders = cStringCollectionHistory.AppendToHistory(My.Settings.LastBrowsedFolders,
                                                                                  Me.sWorkingDirectoryPath,
                                                                                  My.Settings.LastBrowsedFolders_Max)

        '# REMOVED DUE TO CENTRALIZED FUNCTION cStringCollectionHistory
        ' Add the folder to load to the history of opened folders,
        ' or move the entry to the top of the list, if it already existed.
        'Dim iHistoryIndex As Integer = My.Settings.LastBrowsedFolders.IndexOf(Me.sWorkingDirectoryPath)
        'Do While iHistoryIndex >= 0
        '    My.Settings.LastBrowsedFolders.RemoveAt(iHistoryIndex)
        '    iHistoryIndex = My.Settings.LastBrowsedFolders.IndexOf(Me.sWorkingDirectoryPath)
        'Loop
        'My.Settings.LastBrowsedFolders.Add(Me.sWorkingDirectoryPath)

        '' check the max length of the folder history
        'If My.Settings.LastBrowsedFolders.Count >= My.Settings.LastBrowsedFolders_Max Then
        '    My.Settings.LastBrowsedFolders.RemoveAt(My.Settings.LastBrowsedFolders.Count - 1)
        'End If
        cGlobal.SaveSettings()

        ' Refresh the folder-history
        Me.RefreshFolderHistory()

        ' Create and show the window-object
        Dim oDataBrowser As New wDataBrowserModular
        oDataBrowser.Show(Me.sWorkingDirectoryPath, FilterFileList)

        ' Set the location right next to this window,
        ' and adapt the width to fit the screen.
        ' ****** CHANGED (COMMENTED OUT) 15.05.2014: not good on small screens, and on multiple screens.

        'oDataBrowser.Location = New Drawing.Point(Me.Location.X + Me.Width, Me.Location.Y)
        'oDataBrowser.Height = Me.Height
        'Dim BrowserScreen As Screen = Screen.FromControl(Me)
        'oDataBrowser.Width = BrowserScreen.WorkingArea.Width - Me.Width - Me.Location.X

    End Sub
#End Region

#Region "Selection of ListView Changed"

    ''' <summary>
    ''' Counter for the selected files.
    ''' </summary>
    Private SelectedFileCount As Integer = 0

    ''' <summary>
    ''' Called on selection of files in the Folder-Browser.
    ''' </summary>
    Private Sub lv1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lv1.SelectedIndexChanged
        Dim FilesCount As Integer = Me.GetSelectedFilesCount 'Me.lv1.SelectedItems.Count '
        If FilesCount = 0 Then
            Me.btnBrowseFolder.Text = My.Resources.rFolderBrowser.OpenButton_CurrentFolder
        Else
            Me.btnBrowseFolder.Text = My.Resources.rFolderBrowser.OpenButton_CurrentSelection.Replace("%c", FilesCount.ToString("N0"))
        End If
    End Sub

    ''' <summary>
    ''' Counts selected files in the folder-content-view
    ''' </summary>
    Private Function GetSelectedFilesCount() As Integer
        Dim FilesCount As Integer = 0

        '# 10.08.2015: catch an empty path
        If Me.sWorkingDirectoryPath.Trim <> String.Empty Then

            Try
                Dim oDirectoryInfo As New DirectoryInfo(Me.sWorkingDirectoryPath)
                Dim diDirectoryList As DirectoryInfo() = oDirectoryInfo.GetDirectories()
                Dim bFolderFound As Boolean = False
                For i As Integer = 0 To Me.lv1.SelectedItems.Count - 1 Step 1
                    bFolderFound = False
                    For j As Integer = 0 To diDirectoryList.Length - 1 Step 1
                        If diDirectoryList(j).Name = Me.lv1.SelectedItems(i).Text Then
                            bFolderFound = True
                            Exit For
                        End If
                        'If Not DirectCast(Me.lv1.SelectedItems(i).Tag, CShItem).IsFolder Then
                        'FilesCount += 1
                        'End If
                    Next
                    If Not bFolderFound Then FilesCount += 1
                Next
                diDirectoryList = Nothing
                oDirectoryInfo = Nothing
            Catch ex As Exception
                Debug.WriteLine("wFolderBrowser.GetSelectedFilesCount: " & ex.Message)
                FilesCount = 0
            End Try

        End If

        Return FilesCount
    End Function
#End Region


#Region "Folder-History"

    ''' <summary>
    ''' Refreshes the sub-menu with the folder-history,
    ''' and throws out unused values.
    ''' </summary>
    Private Sub RefreshFolderHistory()
        With Me.mnuFolderHistory

            ' Remove old drop-down-items
            For i As Integer = 0 To .DropDownItems.Count - 1 Step 1
                RemoveHandler .DropDownItems(i).Click, AddressOf FolderHistory_Click
            Next

            ' Clear the drop-down
            .DropDownItems.Clear()

            If My.Settings.LastBrowsedFolders.Count > 0 Then
                .Enabled = True
                ' Add the stored history
                For i As Integer = My.Settings.LastBrowsedFolders.Count - 1 To 0 Step -1
                    .DropDownItems.Add(My.Settings.LastBrowsedFolders(i), My.Resources.folder_16)
                    AddHandler .DropDownItems(My.Settings.LastBrowsedFolders.Count - 1 - i).Click, AddressOf FolderHistory_Click
                Next
            Else
                .Enabled = False
            End If


        End With
    End Sub

    ''' <summary>
    ''' Opens the folder from the folder-history.
    ''' </summary>
    Private Sub FolderHistory_Click(sender As Object, e As EventArgs)

        ' Load the path from the button-text.
        Dim sPath As String = DirectCast(sender, ToolStripDropDownItem).Text

        ' load the path, if the directory exists.
        If System.IO.Directory.Exists(sPath) Then
            Dim Path As ExpTreeLib.CShItem = ExpTreeLib.CShItem.GetCShItem(sPath)
            If Not Path Is Nothing Then
                Me.dbDictionaryBrowser.ExpandANode(Path)
                Me.sWorkingDirectoryPath = sPath
            End If
        Else
            ' if the directory does not exist,
            ' remove it from the folder-history in the settings
            Dim iHistoryIndex As Integer = My.Settings.LastBrowsedFolders.IndexOf(sPath)
            Do While iHistoryIndex >= 0
                My.Settings.LastBrowsedFolders.RemoveAt(iHistoryIndex)
                iHistoryIndex = My.Settings.LastBrowsedFolders.IndexOf(sPath)
            Loop
            ' Refresh the folder-history
            Me.RefreshFolderHistory()
        End If
    End Sub

#End Region

#Region "React on KeyDown event for navigation with the keyboard."

    ''' <summary>
    ''' React on KeyDown event for navigation with the keyboard.
    ''' </summary>
    Private Sub dbDictionaryBrowser_TreeKeyDown(sender As Object, e As KeyEventArgs) Handles dbDictionaryBrowser.TreeKeyDown

        Select Case e.KeyCode
            Case Keys.Return
                ' Open the currently selected folder in the DataBrowser.
                Me.btnBrowseFolder_Click()

        End Select

    End Sub

    ''' <summary>
    ''' React on KeyDown event for navigation with the keyboard.
    ''' </summary>
    Private Sub lv1_KeyDown(sender As Object, e As KeyEventArgs) Handles lv1.KeyDown

        Select Case e.KeyCode
            Case Keys.Return
                ' Open the folder, if it is a folder that is selected.
                If Me.lv1.SelectedItems.Count = 1 Then
                    Dim csi As CShItem = DirectCast(lv1.SelectedItems(0).Tag, CShItem)
                    If csi.IsFolder Then
                        Me.dbDictionaryBrowser.ExpandANode(csi)
                        SetWorkingDirectory(csi.Path)
                    End If
                ElseIf Me.lv1.SelectedItems.Count = 0 Then
                    ' Open the currently selected folder in the DataBrowser.
                    Me.btnBrowseFolder_Click()
                End If

        End Select

    End Sub

#End Region

End Class