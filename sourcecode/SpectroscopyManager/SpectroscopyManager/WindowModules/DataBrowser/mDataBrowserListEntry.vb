Imports Amib.Threading

Public Class mDataBrowserListEntry
    Inherits MouseBoundControl
    Implements IDisposable

    ''' <summary>
    ''' Has the component's content been fetched before the init of the UI?
    ''' Than we have to call the fetch-complete event afterwards.
    ''' </summary>
    Private bFetchBeforeInitComplete As Boolean = False

    ''' <summary>
    ''' Fileobject displayed in this list-entry.
    ''' </summary>
    Private WithEvents _FileObject As cFileObject

    ''' <summary>
    ''' Marker, if the fileobject has changed for the current fetch.
    ''' </summary>
    Private FileObjectChanged As Boolean = False

    ''' <summary>
    ''' Returns the currently displayed file-object.
    ''' </summary>
    Public ReadOnly Property CurrentFileObject As cFileObject
        Get
            Return Me._FileObject
        End Get
    End Property

    ''' <summary>
    ''' List-entry to save the preview-data.
    ''' </summary>
    Private _ListEntry As ListEntry

    ''' <summary>
    ''' Threadpool used for all kind of actions for this list-entry.
    ''' </summary>
    Private _ThreadPool As cSmartThreadPoolExtended

    ''' <summary>
    ''' Priority for fetching a list-entry.
    ''' </summary>
    Public Property ListEntryFetchPriority As WorkItemPriority = WorkItemPriority.BelowNormal

    ''' <summary>
    ''' Callback-Function to load the ListEntry in the thread-pool.
    ''' </summary>
    Private ListEntryFetcherCallback As New WorkItemCallback(AddressOf ListEntryFetcher)

    ''' <summary>
    ''' Settings used to draw the preview-image.
    ''' </summary>
    Public Property PreviewImageSettings As mDataBrowserList.PreviewImageSettings

    ''' <summary>
    ''' Tooltip help for the buttons.
    ''' </summary>
    Private ttToolTip As New ToolTip

    ''' <summary>
    ''' Dictionary to get quick-button-clicks access to the base function.
    ''' </summary>
    Private FileActionAPIsToQuickButtons As New Dictionary(Of Button, iDataBrowser_FileObjectAction)

    ''' <summary>
    ''' Quick-buttons for spectroscopy-files.
    ''' </summary>
    Private lQuickButtonStorage_Spec As New List(Of Button)

    ''' <summary>
    ''' Quick-buttons for scan-images.
    ''' </summary>
    Private lQuickButtonStorage_Scan As New List(Of Button)

    ''' <summary>
    ''' Quick-buttons for grid-files.
    ''' </summary>
    Private lQuickButtonStorage_Grid As New List(Of Button)

    ''' <summary>
    ''' ListEntry Background Color, if not selected!
    ''' </summary>
    Private DefaultBackgroundColor As Color = Color.LightGray

    ''' <summary>
    ''' ListEntry Background Color, if it IS selected!
    ''' </summary>
    Private SelectionBackgroundColor As Color = Color.DarkBlue

    ''' <summary>
    ''' Selection Brush to frame the list-entry on selection.
    ''' </summary>
    Private SelectionBrush As Brush = New SolidBrush(SelectionBackgroundColor)

    ''' <summary>
    ''' Selection Pen to frame the list-entry on selection.
    ''' </summary>
    Private SelectionPen As Pen = New Pen(SelectionBrush, 5)

#Region "Events"

    ''' <summary>
    ''' Fired, if the selection of the current list-entry changed.
    ''' </summary>
    Public Event ListEntryClicked(ByRef sender As mDataBrowserListEntry,
                                  ByVal ModifierKeys As Keys)


    ''' <summary>
    ''' This event is used to tell the list-entry hosting container
    ''' that the list-entry has been fetched. So the container can
    ''' continue data processing.
    ''' </summary>
    Public Event ListEntryFetched(ByVal FileType As cFileObject.FileTypes,
                                  ByRef ListEntry As ListEntry,
                                  ByVal ReloadDueToChangeInFileObject As Boolean)

    ''' <summary>
    ''' This event get's raised, if the user right-clicks on the panel.
    ''' </summary>
    Public Event ListEntryRightClicked(ByRef FileObject As cFileObject)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New(ByRef ThreadPool As cSmartThreadPoolExtended,
                   ByRef FileActionAPIs As List(Of iDataBrowser_FileObjectAction),
                   ByRef PreviewSettings As mDataBrowserList.PreviewImageSettings,
                   ByRef ParentWindow As IntPtr)

        ' Initialize the GUI
        Me.InitializeComponent()

        ' Save the thread-pool reference to be
        ' used for loading the data
        ' and other general settings
        Me.ParentWindow = ParentWindow
        Me._ThreadPool = ThreadPool
        Me.PreviewImageSettings = PreviewSettings

        ' Setup all the quick buttons for the file-action-APIs
        For i As Integer = 0 To FileActionAPIs.Count - 1 Step 1

            ' only consider single-file-object actions.
            If FileActionAPIs(i).CanHandleSingleFileObjects Then

                ' Filter certain file-types the Quick-Button
                Select Case FileActionAPIs(i).CanHandleFileObjectType
                    Case cFileObject.FileTypes.SpectroscopyTable
                    Case cFileObject.FileTypes.ScanImage
                    Case cFileObject.FileTypes.GridFile
                    Case Else : Continue For
                End Select

                ' Setup quickbutton (style) and add it to the flow-layout panel.
                If Not FileActionAPIs(i).QuickButtonInListEntry Is Nothing Then
                    Dim QuickButton As Button = FileActionAPIs(i).QuickButtonInListEntry
                    QuickButton.Width = 29
                    QuickButton.Height = 25
                    QuickButton.Margin = New Padding(0, 0, 0, 0)
                    QuickButton.Padding = New Padding(0, 0, 0, 0)
                    Me.flpTools.Controls.Add(QuickButton)

                    ' link the button to the tooltip
                    If FileActionAPIs(i).QuickButtonToolTip <> String.Empty Then Me.ttToolTip.SetToolTip(QuickButton, FileActionAPIs(i).QuickButtonToolTip)

                    ' Add Quick-Button storage
                    Select Case FileActionAPIs(i).CanHandleFileObjectType
                        Case cFileObject.FileTypes.SpectroscopyTable : lQuickButtonStorage_Spec.Add(QuickButton)
                        Case cFileObject.FileTypes.ScanImage : lQuickButtonStorage_Scan.Add(QuickButton)
                        Case cFileObject.FileTypes.GridFile : lQuickButtonStorage_Grid.Add(QuickButton)
                    End Select

                    ' Add the click-action
                    Me.FileActionAPIsToQuickButtons.Add(QuickButton, FileActionAPIs(i))
                    AddHandler QuickButton.Click, AddressOf QuickButtonClick
                End If

            End If
        Next

        ' Set Tooltip messages
        With Me.ttToolTip
            .SetToolTip(Me.panSelection, My.Resources.rDataBrowser.TT_SelectListEntry)
            .SetToolTip(Me.lblFileName, My.Resources.rDataBrowser.TT_FileName)
            .SetToolTip(Me.lblRecordTime, My.Resources.rDataBrowser.TT_FileRecordTime)
            .SetToolTip(Me.lblFileTime, My.Resources.rDataBrowser.TT_FileChangeTime)
            .SetToolTip(Me.txtComment, My.Resources.rDataBrowser.TT_Comment)
            .SetToolTip(Me.lbDataColumns, My.Resources.rDataBrowser.TT_DataColumns)
            .SetToolTip(Me.pbPreview, My.Resources.rDataBrowser.TT_PreviewImage)
        End With
    End Sub

    ''' <summary>
    ''' Destructor of the list-entry.
    ''' </summary>
    Protected Overrides Sub Dispose(disposing As Boolean)

        Try
            ' Remove the quick-button handlers
            For Each QuickButton As Button In Me.FileActionAPIsToQuickButtons.Keys
                RemoveHandler QuickButton.Click, AddressOf QuickButtonClick
            Next

            ' Dispose all the quick-buttons
            Me.FileActionAPIsToQuickButtons.Clear()
            For i As Integer = 0 To Me.lQuickButtonStorage_Spec.Count - 1 Step 1
                Me.lQuickButtonStorage_Spec(i).Dispose()
            Next
            Me.lQuickButtonStorage_Spec.Clear()
            For i As Integer = 0 To Me.lQuickButtonStorage_Scan.Count - 1 Step 1
                Me.lQuickButtonStorage_Scan(i).Dispose()
            Next
            Me.lQuickButtonStorage_Scan.Clear()

            ' Dispose other stuff
            Me._ThreadPool = Nothing
            Me._PreviewImageSettings = Nothing
            Me._FileObject = Nothing
            Me._ListEntry.Dispose()
            Me._ListEntry = Nothing
            Me.ParentWindow = Nothing
            Me.eListEntryFetchComplete = Nothing
            Me.ListEntryFetcherCallback = Nothing

            ' Dispose the tooltip
            Me.ttToolTip.Dispose()

            ' Dispose the GDI-Objects:
            Me.SelectionBrush.Dispose()
            Me.SelectionPen.Dispose()

            ' Disposing of components
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    ''' <summary>
    ''' Run GUI filling, if fetch was performed before the handle-creation.
    ''' </summary>
    Private Sub mDataBrowserListEntry_HandleCreated(sender As Object, e As EventArgs) Handles Me.HandleCreated
        ' If fetch has completed before the init of the UI
        If Me.bFetchBeforeInitComplete Then
            eListEntryFetchComplete.Raise(Me, EventArgs.Empty)
            Me.bFetchBeforeInitComplete = False
        End If
        Me.Refresh()
    End Sub

#End Region

#Region "Set the file-object."
    ''' <summary>
    ''' Initialize by handing over the file-object to show,
    ''' and the Threadpool to use for all kinds of data handling.
    ''' </summary>
    Public Sub SetFileObject(ByRef FileObject As cFileObject)

        ' Save the file-object
        Me._FileObject = FileObject

        ' Set some initial design-styles
        Me.lblFileName.Text = Me._FileObject.FileName
        Me.lblFileTime.Text = Me._FileObject.LastFileChange.ToString()
        Me.pbPreview.Image = My.Resources.loading_16
        Me.panLoading.BringToFront()

        ' Set the quick-buttons
        ' Setup all the quick buttons for the file-action-APIs
        Me.flpTools.SuspendLayout()
        Try
            Me.flpTools.Controls.Clear()
            Select Case Me._FileObject.FileType
                Case cFileObject.FileTypes.SpectroscopyTable : Me.flpTools.Controls.AddRange(Me.lQuickButtonStorage_Spec.ToArray)
                Case cFileObject.FileTypes.ScanImage : Me.flpTools.Controls.AddRange(Me.lQuickButtonStorage_Scan.ToArray)
            End Select
        Catch ex As Exception
        Finally
            ' always resume the layout of the quick-button panel.
            Me.flpTools.ResumeLayout()
        End Try

        ' Fetch the file to show more details.
        Me.FetchListEntry()
    End Sub

#End Region

#Region "ListEntry Structure and Fetching via the Thread-Pool"

    '###############
    Private tmpFetch_PreviewImageSize As Size
    '###############

    Public Delegate Sub _ShowHidePanLoading(ByVal Show As Boolean)

    ''' <summary>
    ''' Shows or hides the loading panel.
    ''' </summary>
    Public Sub ShowHidePanLoading(ByVal Show As Boolean)
        If Me.panLoading.InvokeRequired Then
            Me.panLoading.Invoke(New _ShowHidePanLoading(AddressOf ShowHidePanLoading), Show)
        Else
            Me.panLoading.Visible = Show
        End If
    End Sub

    ''' <summary>
    ''' Initializes the list-entry-fetch.
    ''' Manually, or on any change of the file-object.
    ''' </summary>
    Public Sub FetchListEntry()

        Try
            '# added 07/19/2016: crash during list refreshs
            ' Check, if our interface is disposed already.
            If Me Is Nothing Then Return
            If Me.IsDisposed Then Return
            If Me.panLoading.IsDisposed Then Return

            ' Show loading screen
            ShowHidePanLoading(True)

            ' Set the loading background color.
            '# added 07/29/2016: crash still occured at this line here
            Me.BackColor = Color.LightGray

            ' Save list-entry fetch properties.
            tmpFetch_PreviewImageSize = Me.pbPreview.Size

            ' Initialize the fetch of the file-object:
            Me._ThreadPool.QueueWorkItem(ListEntryFetcherCallback, Me.ListEntryFetchPriority)

        Catch ex As Exception
            Debug.WriteLine("FetchListEntry failed: " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Initializes the list-entry-fetch.
    ''' Due to a change of the file-object.
    ''' </summary>
    Private Sub FetchListEntryDueToChangeOfTheObject() Handles _FileObject.FileObjectChanged
        Me.FileObjectChanged = True
        Me.FetchListEntry()
    End Sub

    ''' <summary>
    ''' Represents a List-Entry of a Spectroscopy-File.
    ''' </summary>
    Public Structure ListEntry
        Implements IDisposable

        Public Property ColumnNames As List(Of String)
        Public Property Comment As String
        Public Property DisplayName As String
        Public Property FileName As String
        Public Property FullFileName As String
        Public Property MeasurementPoints As String
        Public Property PreviewImage As Image
        Public Property RecordDate As Date
        Public Property BackColor As Color

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not Me.PreviewImage Is Nothing Then
                '# 15.10.2015: Removed due to images that were disposed, before saving them in the cache files.
                'Me.PreviewImage.Dispose()
            End If
            Me.BackColor = Nothing
            If Not Me.ColumnNames Is Nothing Then
                Me.ColumnNames.Clear()
            End If
        End Sub
    End Structure

    ''' <summary>
    ''' Event that gets fired, when a ListEntry was successfully created from a given FileObject.
    ''' Is called thread-safe in the GUI-Thread.
    ''' </summary>
    Public eListEntryFetchComplete As New EventHandler(AddressOf ListEntryFetcher_FetchComplete)

    ''' <summary>
    ''' Writes all the fetched data to the panel
    ''' </summary>
    Private Sub ListEntryFetcher_FetchComplete()

        ' show the data
        Try
            With Me._ListEntry

                ' hide the loading-panel
                Me.panLoading.Visible = False

                ' Check for a valid list-entry.
                If Me._ListEntry.PreviewImage Is Nothing Then Return

                ' Add preview-image
                Me.pbPreview.Image = .PreviewImage

                ' Add column-list
                Me.lbDataColumns.Items.Clear()
                If .ColumnNames IsNot Nothing Then Me.lbDataColumns.Items.AddRange(.ColumnNames.ToArray)

                ' Displayname
                Me.lblFileName.Text = .DisplayName

                ' Add comment
                Me.txtComment.Text = .Comment

                ' Datapoints
                Me.lblPoints.Text = .MeasurementPoints

                ' Record date
                Me.lblRecordTime.Text = .RecordDate.ToString

                ' Back-Color
                Me.DefaultBackgroundColor = .BackColor
                Me.BackColor = Me.DefaultBackgroundColor
                Me.panSelection.BackColor = Me.DefaultBackgroundColor

            End With

            ' RaiseEvent to report back the fetched list-entry details.
            RaiseEvent ListEntryFetched(Me._FileObject.FileType, Me._ListEntry, Me.FileObjectChanged)
            Me.FileObjectChanged = False
            'Me.Refresh()
        Catch ex As Exception
            Debug.WriteLine("ERROR IN mDataBrowserListEntry.ListEntryFetcher_FetchComplete: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' File Fetcher to load the SpectroscopyTable-Files displayed in the list asynchronously.
    ''' </summary>
    Private Function ListEntryFetcher() As Object

        Try

            If Me._FileObject.FileType = cFileObject.FileTypes.SpectroscopyTable Then
                '############################
                ' Fetch a Spectroscopy-Table
                '############################

                ' Load the Preview-Image, if the columns could be found in the Table.

                ' Send Abort-Image, if the Columns were not found.
                Dim PreviewImage As Image
                With Me.PreviewImageSettings
                    Dim XCol As String = .GetFirstExistingColumnName_X(Me._FileObject.SpectroscopyTable.GetColumnNameList)
                    Dim YCol As String = .GetFirstExistingColumnName_Y(Me._FileObject.SpectroscopyTable.GetColumnNameList)
                    If Me._FileObject.SpectroscopyTable.Columns.ContainsKey(XCol) AndAlso
                        Me._FileObject.SpectroscopyTable.Columns.ContainsKey(YCol) Then
                        PreviewImage = Me._FileObject.GetSpectroscopyTablePreviewImage(XCol,
                                                                                       YCol,
                                                                                       Me.tmpFetch_PreviewImageSize.Width,
                                                                                       Me.tmpFetch_PreviewImageSize.Height,
                                                                                       .SpectroscopyTable_LogX,
                                                                                       .SpectroscopyTable_LogY,
                                                                                       .SpectroscopyTable_EnablePointReduction)
                    Else
                        PreviewImage = My.Resources.columns_do_not_exist
                    End If
                End With

                ' Create new ListEntry
                Me._ListEntry = New ListEntry
                With Me._FileObject
                    Me._ListEntry.FullFileName = .FullFileNameInclPath
                    Me._ListEntry.FileName = .FileNameWithoutPath
                    Me._ListEntry.DisplayName = .DisplayName
                    ' Show the source file comment, if it is not empty.
                    ' Else, show the extended comment.
                    If .SourceFileComment <> String.Empty Then
                        Me._ListEntry.Comment = .SourceFileComment
                    Else
                        Me._ListEntry.Comment = .ExtendedComment
                    End If
                    Me._ListEntry.ColumnNames = .GetColumnNameList
                    Me._ListEntry.RecordDate = .RecordDate
                    Me._ListEntry.PreviewImage = PreviewImage
                    Me._ListEntry.MeasurementPoints = .MeasurementDimensions
                    Me._ListEntry.BackColor = Color.LightGray
                End With


            ElseIf Me._FileObject.FileType = cFileObject.FileTypes.ScanImage Then
                '##########################
                ' Fetch a ScanImage-Table
                '##########################

                ' Load the Preview-Image, with the first Channel
                ' Send Abort-Image, if the Columns were not found.
                Dim PreviewImage As Image
                If Me._FileObject.GetScanChannelList.Count > 0 Then

                    ' Check in the ScanImage, if there are
                    ' Channels with the names to be displayed as Preview-Image.
                    If Me._FileObject.GetScanChannelNameList.Contains(Me.PreviewImageSettings.ScanImage_Channel) Then
                        PreviewImage = Me._FileObject.GetScanChannelPreviewImage(Me.PreviewImageSettings.ScanImage_Channel,
                                                                                 Me.tmpFetch_PreviewImageSize.Width,
                                                                                 Me.tmpFetch_PreviewImageSize.Height)
                    Else
                        PreviewImage = My.Resources.channel_does_not_exist
                    End If
                Else
                    PreviewImage = My.Resources.cancel
                End If

                ' Create new ListEntry
                Me._ListEntry = New ListEntry
                With Me._FileObject
                    Me._ListEntry.FullFileName = .FullFileNameInclPath
                    Me._ListEntry.FileName = .FileNameWithoutPath
                    Me._ListEntry.DisplayName = .DisplayName
                    ' Show the source file comment, if it is not empty.
                    ' Else, show the extended comment.
                    If .SourceFileComment <> String.Empty Then
                        Me._ListEntry.Comment = .SourceFileComment
                    Else
                        Me._ListEntry.Comment = .ExtendedComment
                    End If
                    Me._ListEntry.ColumnNames = .GetScanChannelNameList
                    Me._ListEntry.RecordDate = .RecordDate
                    Me._ListEntry.PreviewImage = PreviewImage
                    Me._ListEntry.MeasurementPoints = .MeasurementDimensions
                    Me._ListEntry.BackColor = Color.DimGray
                End With

            ElseIf Me._FileObject.FileType = cFileObject.FileTypes.GridFile Then
                '##########################
                ' Fetch a GridFile-Table
                '##########################

                ' Load the Preview-Image, with the first Channel
                ' Send Abort-Image, if the Columns were not found.
                Dim PreviewImage As Image = My.Resources.gridfile_preview
                'If Me._FileObject.GetGridSpectroscopyTableList.Count > 0 Then

                '    '' Check in the ScanImage, if there are
                '    '' Channels with the names to be displayed as Preview-Image.
                '    'If Me._FileObject.GetScanChannelNameList.Contains(Me.PreviewImageSettings.ScanImage_Channel) Then
                '    '    PreviewImage = Me._FileObject.GetScanChannelPreviewImage(Me.PreviewImageSettings.ScanImage_Channel,
                '    '                                                             Me.tmpFetch_PreviewImageSize.Width,
                '    '                                                             Me.tmpFetch_PreviewImageSize.Height)
                '    'Else
                '    '    PreviewImage = My.Resources.channel_does_not_exist
                '    'End If
                'Else
                '    PreviewImage = My.Resources.cancel
                'End If

                ' Create new ListEntry
                Me._ListEntry = New ListEntry
                With Me._FileObject
                    Me._ListEntry.FullFileName = .FullFileNameInclPath
                    Me._ListEntry.FileName = .FileNameWithoutPath
                    Me._ListEntry.DisplayName = .DisplayName
                    ' Show the source file comment, if it is not empty.
                    ' Else, show the extended comment.
                    If .SourceFileComment <> String.Empty Then
                        Me._ListEntry.Comment = .SourceFileComment
                    Else
                        Me._ListEntry.Comment = .ExtendedComment
                    End If
                    Me._ListEntry.ColumnNames = .GetGridSpectroscopyTableNameList
                    Me._ListEntry.RecordDate = .RecordDate
                    Me._ListEntry.PreviewImage = PreviewImage
                    Me._ListEntry.MeasurementPoints = .MeasurementDimensions
                    Me._ListEntry.BackColor = Color.Gray
                End With

            End If

            ' Fetch complete
            If Me.IsHandleCreated Then
                eListEntryFetchComplete.Raise(Me, EventArgs.Empty)
            Else
                Me.bFetchBeforeInitComplete = True
            End If

        Catch ex As Exception
            Debug.WriteLine("ERROR IN mDataBrowserListEntry.ListEntryFetcher: " & ex.Message)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Changes the shown progress of the loading-procedure,
    ''' if panLoading is visible.
    ''' </summary>
    Private Sub ProgressChange(ByVal ProgressPercent As Integer,
                               ByVal Message As String)
        Me.lblLoadingProgress.Text = Message
    End Sub
#End Region

#Region "Paint / Resize event"

    ''' <summary>
    ''' Override for a custom paint event.
    ''' </summary>
    Private Sub mDataBrowserListEntry_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        ' Paint border?
        If Me._ListEntrySelected Then
            e.Graphics.DrawRectangle(Me.SelectionPen, Me.ClientRectangle)
            If Me.panSelection.BackColor <> SelectionBackgroundColor Then
                Me.panSelection.BackColor = SelectionBackgroundColor
            End If
        End If
    End Sub

    ''' <summary>
    ''' On-Resize:
    ''' 1) hide the quick-tool-panel, if the total width is too narrow.
    ''' </summary>
    Private Sub mDataBrowserListEntry_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        ' Manually perform the "anchor" of the group-boxes.
        Dim HeadingOffset As Integer = Me.lblFileName.Location.Y + Me.lblFileName.Height + 5
        Dim GroupboxHeight As Integer = Me.Height - HeadingOffset - 5
        Dim EffectiveWidth As Integer = Me.Width - 5

        ' 1) ##########
        If Me.Size.Width < 700 Then
            Me.gbTools.Visible = False
            Me.gbColumns.Visible = False
        ElseIf Me.Size.Width < 900 Then
            Me.gbTools.Visible = False
            Me.gbColumns.Visible = True
        Else
            Me.gbTools.Visible = True
            Me.gbColumns.Visible = True
        End If

        With Me.gbFileProperties ' Property box pinned to the left side
            If .Visible Then
                .Location = New Point(.Location.X, HeadingOffset)
                .Height = GroupboxHeight
            End If
        End With
        With Me.gbTools ' TOOLBOX pinned to the right side
            If .Visible Then
                .Location = New Point(EffectiveWidth - .Width, HeadingOffset)
                .Height = GroupboxHeight
            End If
        End With
        With Me.gbColumns ' ColumnList pinned to the right side, next to the tool-box
            If Me.gbTools.Visible Then
                .Location = New Point(EffectiveWidth - .Width - Me.gbTools.Width, HeadingOffset)
            Else
                .Location = New Point(EffectiveWidth - .Width, HeadingOffset)
            End If
            .Height = GroupboxHeight
        End With
        With Me.gbComment ' Comment adapting the width to the right
            .Location = New Point(Me.gbFileProperties.Location.X + Me.gbFileProperties.Width, HeadingOffset)
            .Height = GroupboxHeight
            If Me.gbColumns.Visible Then
                .Width = Me.gbColumns.Location.X - .Location.X
            Else
                .Width = EffectiveWidth - .Location.X
            End If
        End With
    End Sub

#End Region

#Region "Selection of the list-entry"

    Private _ListEntrySelected As Boolean = False
    ''' <summary>
    ''' Is the list-entry selected?
    ''' </summary>
    Public Property ListEntrySelected As Boolean
        Get
            Return Me._ListEntrySelected
        End Get
        Set(value As Boolean)
            If value = Me._ListEntrySelected Then Return
            If value Then
                Me.panSelection.BackgroundImage = My.Resources.selected_w
                Me.panSelection.BackColor = SelectionBackgroundColor
            Else
                Me.panSelection.BackgroundImage = Nothing
                Me.panSelection.BackColor = Me.DefaultBackgroundColor
            End If
            Me._ListEntrySelected = value
            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Selection of the list-entry.
    ''' </summary>
    Private Sub mDataBrowserListEntry_MouseDown(sender As Object, e As EventArgs) Handles Me.LeftClicked
        If sender Is Me Then
            Me.OnClickListEntry(Me.CurrentModifierKeys)
        End If
    End Sub

    ' ''' <summary>
    ' ''' Mouse-Move with pressed mouse button to change selection
    ' ''' </summary>
    'Private Sub mDataBrowserListEntry_MouseEnter(sender As Object, e As EventArgs) Handles Me.MouseEnter_ControlArea
    '    If MyBase.MB.CurrentMouseButtons = Windows.Forms.MouseButtons.Left Then
    '        Me.OnClickListEntry(Me.CurrentModifierKeys)
    '    End If
    'End Sub

    ''' <summary>
    ''' Swap the current selection state, and raise the event.
    ''' </summary>
    Public Sub OnClickListEntry(ModKeys As Keys)
        RaiseEvent ListEntryClicked(Me, ModKeys)
    End Sub
#End Region

#Region "Double click on the list-entry's file-name or image and open the file in a separate window."

    ''' <summary>
    ''' Open the selected list-entry on double clicking the control.
    ''' </summary>
    Public Sub ListEntry_DoubleClicked(sender As Object, e As EventArgs) Handles lblFileName.DoubleClick, pbPreview.DoubleClick

        ' Open detail window for the file-object.
        Select Case Me._FileObject.FileType
            Case cFileObject.FileTypes.SpectroscopyTable
                ' Show Spectroscopy-Table Details
                Dim DataExplorer As New wDataExplorer_SpectroscopyTable
                DataExplorer.Show(Me._FileObject)
                DataExplorer.SetInitialColumnSelection(Me.PreviewImageSettings.GetFirstExistingColumnName_X(Me._FileObject.GetColumnNameList),
                                                       Me.PreviewImageSettings.GetFirstExistingColumnName_Y(Me._FileObject.GetColumnNameList))

            Case cFileObject.FileTypes.ScanImage
                ' Show ScanImage Details
                Dim DataExplorer As New wDataExplorer_ScanImage
                DataExplorer.Show(Me._FileObject)
                DataExplorer.SetInitialChannelSelection(Me.PreviewImageSettings.ScanImage_Channel)
        End Select
    End Sub

#End Region

#Region "Right click on the list-entry will ask to bring up the context-menu."

    ''' <summary>
    ''' Open the selected list-entry's context menu on right clicking the control.
    ''' </summary>
    Public Sub ListEntry_RightClicked(sender As Object, e As EventArgs) Handles MB.RightClicked
        RaiseEvent ListEntryRightClicked(Me._FileObject)
    End Sub

#End Region

#Region "Quick-Button Click"
    ''' <summary>
    ''' Handles the click on a quick-button.
    ''' It searches for the quick-button API implementation,
    ''' and launches the action.
    ''' </summary>
    Private Sub QuickButtonClick(sender As Object, e As EventArgs)
        Dim B As Button = TryCast(sender, Button)
        If B Is Nothing Then Return

        ' Launch single-file action.
        If Me.FileActionAPIsToQuickButtons.ContainsKey(B) Then
            Me.FileActionAPIsToQuickButtons(B).SingleFileAction(Me._FileObject)
        End If
    End Sub

#End Region

End Class
