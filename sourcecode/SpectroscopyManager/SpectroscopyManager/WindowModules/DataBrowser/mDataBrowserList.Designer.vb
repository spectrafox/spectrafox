<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mDataBrowserList
    Inherits UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.mnu = New System.Windows.Forms.MenuStrip()
        Me.mnuRefreshList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSorting = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSort_ByFileName = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSort_ByFileDate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSort_ByRecordDate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSort_BySelection = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSort_ASC = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSort_DESC = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilter_ShowDataTableFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilter_ShowGridFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilter_ShowScanImageFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFilter_Title = New System.Windows.Forms.ToolStripLabel()
        Me.mnuFilter_FilterText = New System.Windows.Forms.ToolStripTextBox()
        Me.mnuFilter_AddToHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilter_FilterHistory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilter_ClearFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuFilter_SearchTitle = New System.Windows.Forms.ToolStripLabel()
        Me.mnuFilter_SearchText = New System.Windows.Forms.ToolStripTextBox()
        Me.mnuFilter_SearchNext = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFilter_SearchClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPreviewSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPreview_XTitle = New System.Windows.Forms.ToolStripLabel()
        Me.mnuPreview_cbSpectroscopyColumnX_1st = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuPreview_cbSpectroscopyColumnX_2nd = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuPreview_cbSpectroscopyColumnX_3rd = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuPreview_Spectroscopy_LogX = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPreview_YTitle = New System.Windows.Forms.ToolStripLabel()
        Me.mnuPreview_cbSpectroscopyColumnY_1st = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuPreview_cbSpectroscopyColumnY_2nd = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuPreview_cbSpectroscopyColumnY_3rd = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuPreview_Spectroscopy_LogY = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPreview_Spectroscopy_PointReduction = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPreview_ChannelTitle = New System.Windows.Forms.ToolStripLabel()
        Me.mnuPreview_cbScanImageChannel = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuMultipleSpectroscopyFileActions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_OpenSpectroscopyFilesSeparately = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTools_ExportWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTools_PlotSpectraTogether = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_PlotSpectraTogetherAutomatic = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTools_Visualization = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_DataManipulations = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_NumericManipulations = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_AtomicForceMicroscopyTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTools_SpectroscopyDataCache = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMultipleScanImageFileActions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_OpenScanImagesSeparately = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTools_ScanImageDataCache = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools_OpenMovieGenerator = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGridTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGridTools_ExportGridSpectra = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSpecialTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSpecialTools_GridViewer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ssStatus = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblTimerStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.mnuTimerConfig = New System.Windows.Forms.ToolStripSplitButton()
        Me.smnuTimerOnOff = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTimerConfig_TimeHeader = New System.Windows.Forms.ToolStripLabel()
        Me.mnuTimerConfig_RefreshTime = New System.Windows.Forms.ToolStripTextBox()
        Me.mnuTimerConfig_ApplyTimer = New System.Windows.Forms.ToolStripMenuItem()
        Me.vListScroll = New System.Windows.Forms.VScrollBar()
        Me.ttToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmSpectroscopyFile = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuSpectroscopy_Header = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuSpectroscopy_SpectroscopyTableShowDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSpectroscopy_ShowNearestScanImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSpectroscopy_OpenExportWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuSpectroscopy_CopyDataToClipboardOriginCompatible = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSpectroscopy_CopyDataToClipboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuSpectroscopy_DataManipulations = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSpectroscopy_NumericManipulations = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSpectroscopy_AtomicForceMicroscopyTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmScanImageFile = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuScanImage_Header = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuScanImage_OpenInSeparateWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmGridFile = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuGridFile_Header = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmnuGridFile_OpenInSeparateWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuGridFile_ExportGridSpectra = New System.Windows.Forms.ToolStripMenuItem()
        Me.panBrowserListContainer = New SpectroscopyManager.MouseBoundPanel()
        Me.panProgress = New System.Windows.Forms.Panel()
        Me.lblProgressHeading = New System.Windows.Forms.Label()
        Me.pbProgress = New System.Windows.Forms.ProgressBar()
        Me.panBrowserList = New SpectroscopyManager.VirtualVerticalPanel()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnu.SuspendLayout()
        Me.ssStatus.SuspendLayout()
        Me.cmSpectroscopyFile.SuspendLayout()
        Me.cmScanImageFile.SuspendLayout()
        Me.cmGridFile.SuspendLayout()
        Me.panBrowserListContainer.SuspendLayout()
        Me.panProgress.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnu
        '
        Me.mnu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRefreshList, Me.mnuSorting, Me.mnuFilter, Me.mnuPreviewSettings, Me.mnuMultipleSpectroscopyFileActions, Me.mnuMultipleScanImageFileActions, Me.mnuGridTools, Me.mnuSpecialTools})
        Me.mnu.Location = New System.Drawing.Point(0, 0)
        Me.mnu.Name = "mnu"
        Me.mnu.Size = New System.Drawing.Size(851, 24)
        Me.mnu.TabIndex = 0
        Me.mnu.Text = "MenuStrip1"
        '
        'mnuRefreshList
        '
        Me.mnuRefreshList.Image = Global.SpectroscopyManager.My.Resources.Resources.reload_16
        Me.mnuRefreshList.Name = "mnuRefreshList"
        Me.mnuRefreshList.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.mnuRefreshList.Size = New System.Drawing.Size(28, 20)
        Me.mnuRefreshList.ToolTipText = "scan the project folder for now or changed files"
        '
        'mnuSorting
        '
        Me.mnuSorting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSort_ByFileName, Me.mnuSort_ByFileDate, Me.mnuSort_ByRecordDate, Me.mnuSort_BySelection, Me.ToolStripSeparator1, Me.mnuSort_ASC, Me.mnuSort_DESC})
        Me.mnuSorting.Image = Global.SpectroscopyManager.My.Resources.Resources.sort_16
        Me.mnuSorting.Name = "mnuSorting"
        Me.mnuSorting.Size = New System.Drawing.Size(28, 20)
        Me.mnuSorting.ToolTipText = "change the sorting of the list"
        '
        'mnuSort_ByFileName
        '
        Me.mnuSort_ByFileName.Name = "mnuSort_ByFileName"
        Me.mnuSort_ByFileName.Size = New System.Drawing.Size(134, 22)
        Me.mnuSort_ByFileName.Text = "file name"
        '
        'mnuSort_ByFileDate
        '
        Me.mnuSort_ByFileDate.Checked = True
        Me.mnuSort_ByFileDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuSort_ByFileDate.Name = "mnuSort_ByFileDate"
        Me.mnuSort_ByFileDate.Size = New System.Drawing.Size(134, 22)
        Me.mnuSort_ByFileDate.Text = "file date"
        '
        'mnuSort_ByRecordDate
        '
        Me.mnuSort_ByRecordDate.Name = "mnuSort_ByRecordDate"
        Me.mnuSort_ByRecordDate.Size = New System.Drawing.Size(134, 22)
        Me.mnuSort_ByRecordDate.Text = "record date"
        '
        'mnuSort_BySelection
        '
        Me.mnuSort_BySelection.Name = "mnuSort_BySelection"
        Me.mnuSort_BySelection.Size = New System.Drawing.Size(134, 22)
        Me.mnuSort_BySelection.Text = "selection"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(131, 6)
        '
        'mnuSort_ASC
        '
        Me.mnuSort_ASC.Checked = True
        Me.mnuSort_ASC.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuSort_ASC.Name = "mnuSort_ASC"
        Me.mnuSort_ASC.Size = New System.Drawing.Size(134, 22)
        Me.mnuSort_ASC.Text = "ascending"
        '
        'mnuSort_DESC
        '
        Me.mnuSort_DESC.Name = "mnuSort_DESC"
        Me.mnuSort_DESC.Size = New System.Drawing.Size(134, 22)
        Me.mnuSort_DESC.Text = "decending"
        '
        'mnuFilter
        '
        Me.mnuFilter.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFilter_ShowDataTableFiles, Me.mnuFilter_ShowGridFiles, Me.mnuFilter_ShowScanImageFiles, Me.ToolStripSeparator10, Me.mnuFilter_Title, Me.mnuFilter_FilterText, Me.mnuFilter_AddToHistory, Me.mnuFilter_FilterHistory, Me.mnuFilter_ClearFilter, Me.ToolStripSeparator12, Me.mnuFilter_SearchTitle, Me.mnuFilter_SearchText, Me.mnuFilter_SearchNext, Me.mnuFilter_SearchClear})
        Me.mnuFilter.Image = Global.SpectroscopyManager.My.Resources.Resources.filter_16
        Me.mnuFilter.Name = "mnuFilter"
        Me.mnuFilter.Size = New System.Drawing.Size(28, 20)
        Me.mnuFilter.ToolTipText = "search entries, or apply filters to the list"
        '
        'mnuFilter_ShowDataTableFiles
        '
        Me.mnuFilter_ShowDataTableFiles.Checked = True
        Me.mnuFilter_ShowDataTableFiles.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuFilter_ShowDataTableFiles.Name = "mnuFilter_ShowDataTableFiles"
        Me.mnuFilter_ShowDataTableFiles.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.mnuFilter_ShowDataTableFiles.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_ShowDataTableFiles.Text = "show data table files"
        '
        'mnuFilter_ShowGridFiles
        '
        Me.mnuFilter_ShowGridFiles.Checked = True
        Me.mnuFilter_ShowGridFiles.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuFilter_ShowGridFiles.Name = "mnuFilter_ShowGridFiles"
        Me.mnuFilter_ShowGridFiles.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_ShowGridFiles.Text = "show grid spectroscopy files"
        '
        'mnuFilter_ShowScanImageFiles
        '
        Me.mnuFilter_ShowScanImageFiles.Checked = True
        Me.mnuFilter_ShowScanImageFiles.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuFilter_ShowScanImageFiles.Name = "mnuFilter_ShowScanImageFiles"
        Me.mnuFilter_ShowScanImageFiles.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.mnuFilter_ShowScanImageFiles.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_ShowScanImageFiles.Text = "show scan image files"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(277, 6)
        '
        'mnuFilter_Title
        '
        Me.mnuFilter_Title.Name = "mnuFilter_Title"
        Me.mnuFilter_Title.Size = New System.Drawing.Size(215, 15)
        Me.mnuFilter_Title.Text = "apply name filter: (use * as placeholder)"
        '
        'mnuFilter_FilterText
        '
        Me.mnuFilter_FilterText.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuFilter_FilterText.Name = "mnuFilter_FilterText"
        Me.mnuFilter_FilterText.Size = New System.Drawing.Size(220, 23)
        '
        'mnuFilter_AddToHistory
        '
        Me.mnuFilter_AddToHistory.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.mnuFilter_AddToHistory.Name = "mnuFilter_AddToHistory"
        Me.mnuFilter_AddToHistory.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_AddToHistory.Text = "add current filter to history"
        '
        'mnuFilter_FilterHistory
        '
        Me.mnuFilter_FilterHistory.Image = Global.SpectroscopyManager.My.Resources.Resources.filter_16
        Me.mnuFilter_FilterHistory.Name = "mnuFilter_FilterHistory"
        Me.mnuFilter_FilterHistory.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_FilterHistory.Text = "last used filters"
        '
        'mnuFilter_ClearFilter
        '
        Me.mnuFilter_ClearFilter.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.mnuFilter_ClearFilter.Name = "mnuFilter_ClearFilter"
        Me.mnuFilter_ClearFilter.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.mnuFilter_ClearFilter.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_ClearFilter.Text = "clear filter"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(277, 6)
        '
        'mnuFilter_SearchTitle
        '
        Me.mnuFilter_SearchTitle.Name = "mnuFilter_SearchTitle"
        Me.mnuFilter_SearchTitle.Size = New System.Drawing.Size(193, 15)
        Me.mnuFilter_SearchTitle.Text = "search name: (use * as placeholder)"
        '
        'mnuFilter_SearchText
        '
        Me.mnuFilter_SearchText.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuFilter_SearchText.Name = "mnuFilter_SearchText"
        Me.mnuFilter_SearchText.Size = New System.Drawing.Size(220, 23)
        '
        'mnuFilter_SearchNext
        '
        Me.mnuFilter_SearchNext.Image = Global.SpectroscopyManager.My.Resources.Resources.right_16
        Me.mnuFilter_SearchNext.Name = "mnuFilter_SearchNext"
        Me.mnuFilter_SearchNext.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.mnuFilter_SearchNext.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_SearchNext.Text = "search next entry"
        '
        'mnuFilter_SearchClear
        '
        Me.mnuFilter_SearchClear.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.mnuFilter_SearchClear.Name = "mnuFilter_SearchClear"
        Me.mnuFilter_SearchClear.ShortcutKeys = System.Windows.Forms.Keys.F6
        Me.mnuFilter_SearchClear.Size = New System.Drawing.Size(280, 22)
        Me.mnuFilter_SearchClear.Text = "clear search string"
        '
        'mnuPreviewSettings
        '
        Me.mnuPreviewSettings.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPreview_XTitle, Me.mnuPreview_cbSpectroscopyColumnX_1st, Me.mnuPreview_cbSpectroscopyColumnX_2nd, Me.mnuPreview_cbSpectroscopyColumnX_3rd, Me.mnuPreview_Spectroscopy_LogX, Me.ToolStripSeparator14, Me.mnuPreview_YTitle, Me.mnuPreview_cbSpectroscopyColumnY_1st, Me.mnuPreview_cbSpectroscopyColumnY_2nd, Me.mnuPreview_cbSpectroscopyColumnY_3rd, Me.mnuPreview_Spectroscopy_LogY, Me.ToolStripSeparator15, Me.mnuPreview_Spectroscopy_PointReduction, Me.ToolStripSeparator9, Me.mnuPreview_ChannelTitle, Me.mnuPreview_cbScanImageChannel})
        Me.mnuPreviewSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.mnuPreviewSettings.Name = "mnuPreviewSettings"
        Me.mnuPreviewSettings.Size = New System.Drawing.Size(28, 20)
        Me.mnuPreviewSettings.ToolTipText = "adjust the preview image settings"
        '
        'mnuPreview_XTitle
        '
        Me.mnuPreview_XTitle.Image = Global.SpectroscopyManager.My.Resources.Resources.x_16
        Me.mnuPreview_XTitle.Name = "mnuPreview_XTitle"
        Me.mnuPreview_XTitle.Size = New System.Drawing.Size(224, 16)
        Me.mnuPreview_XTitle.Text = "preview column (1st, 2nd, 3rd choice):"
        '
        'mnuPreview_cbSpectroscopyColumnX_1st
        '
        Me.mnuPreview_cbSpectroscopyColumnX_1st.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuPreview_cbSpectroscopyColumnX_1st.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mnuPreview_cbSpectroscopyColumnX_1st.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mnuPreview_cbSpectroscopyColumnX_1st.Name = "mnuPreview_cbSpectroscopyColumnX_1st"
        Me.mnuPreview_cbSpectroscopyColumnX_1st.Size = New System.Drawing.Size(171, 23)
        Me.mnuPreview_cbSpectroscopyColumnX_1st.ToolTipText = "primary choice for the column to be used in the preview image"
        '
        'mnuPreview_cbSpectroscopyColumnX_2nd
        '
        Me.mnuPreview_cbSpectroscopyColumnX_2nd.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuPreview_cbSpectroscopyColumnX_2nd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mnuPreview_cbSpectroscopyColumnX_2nd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mnuPreview_cbSpectroscopyColumnX_2nd.Name = "mnuPreview_cbSpectroscopyColumnX_2nd"
        Me.mnuPreview_cbSpectroscopyColumnX_2nd.Size = New System.Drawing.Size(171, 23)
        Me.mnuPreview_cbSpectroscopyColumnX_2nd.ToolTipText = "second choice for the column to be used in the preview image" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(used only, if the " &
    "primary choice is not found in the data file)"
        '
        'mnuPreview_cbSpectroscopyColumnX_3rd
        '
        Me.mnuPreview_cbSpectroscopyColumnX_3rd.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuPreview_cbSpectroscopyColumnX_3rd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mnuPreview_cbSpectroscopyColumnX_3rd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mnuPreview_cbSpectroscopyColumnX_3rd.Name = "mnuPreview_cbSpectroscopyColumnX_3rd"
        Me.mnuPreview_cbSpectroscopyColumnX_3rd.Size = New System.Drawing.Size(171, 23)
        Me.mnuPreview_cbSpectroscopyColumnX_3rd.ToolTipText = "third choice for the column to be used in the preview image" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(used only, if the p" &
    "rimary and the secondary choices are not found in the data file)"
        '
        'mnuPreview_Spectroscopy_LogX
        '
        Me.mnuPreview_Spectroscopy_LogX.CheckOnClick = True
        Me.mnuPreview_Spectroscopy_LogX.Name = "mnuPreview_Spectroscopy_LogX"
        Me.mnuPreview_Spectroscopy_LogX.Size = New System.Drawing.Size(284, 22)
        Me.mnuPreview_Spectroscopy_LogX.Text = "logarithmic x scale"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(281, 6)
        '
        'mnuPreview_YTitle
        '
        Me.mnuPreview_YTitle.Image = Global.SpectroscopyManager.My.Resources.Resources.y_16
        Me.mnuPreview_YTitle.Name = "mnuPreview_YTitle"
        Me.mnuPreview_YTitle.Size = New System.Drawing.Size(224, 16)
        Me.mnuPreview_YTitle.Text = "preview column (1st, 2nd, 3rd choice):"
        '
        'mnuPreview_cbSpectroscopyColumnY_1st
        '
        Me.mnuPreview_cbSpectroscopyColumnY_1st.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuPreview_cbSpectroscopyColumnY_1st.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mnuPreview_cbSpectroscopyColumnY_1st.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mnuPreview_cbSpectroscopyColumnY_1st.Name = "mnuPreview_cbSpectroscopyColumnY_1st"
        Me.mnuPreview_cbSpectroscopyColumnY_1st.Size = New System.Drawing.Size(171, 23)
        Me.mnuPreview_cbSpectroscopyColumnY_1st.ToolTipText = "primary choice for the column to be used in the preview image" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'mnuPreview_cbSpectroscopyColumnY_2nd
        '
        Me.mnuPreview_cbSpectroscopyColumnY_2nd.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuPreview_cbSpectroscopyColumnY_2nd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mnuPreview_cbSpectroscopyColumnY_2nd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mnuPreview_cbSpectroscopyColumnY_2nd.Name = "mnuPreview_cbSpectroscopyColumnY_2nd"
        Me.mnuPreview_cbSpectroscopyColumnY_2nd.Size = New System.Drawing.Size(171, 23)
        Me.mnuPreview_cbSpectroscopyColumnY_2nd.ToolTipText = "second choice for the column to be used in the preview image" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(used only, if the " &
    "primary choice is not found in the data file)"
        '
        'mnuPreview_cbSpectroscopyColumnY_3rd
        '
        Me.mnuPreview_cbSpectroscopyColumnY_3rd.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuPreview_cbSpectroscopyColumnY_3rd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mnuPreview_cbSpectroscopyColumnY_3rd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mnuPreview_cbSpectroscopyColumnY_3rd.Name = "mnuPreview_cbSpectroscopyColumnY_3rd"
        Me.mnuPreview_cbSpectroscopyColumnY_3rd.Size = New System.Drawing.Size(171, 23)
        Me.mnuPreview_cbSpectroscopyColumnY_3rd.ToolTipText = "third choice for the column to be used in the preview image" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(used only, if the p" &
    "rimary and the secondary choices are not found in the data file)"
        '
        'mnuPreview_Spectroscopy_LogY
        '
        Me.mnuPreview_Spectroscopy_LogY.CheckOnClick = True
        Me.mnuPreview_Spectroscopy_LogY.Name = "mnuPreview_Spectroscopy_LogY"
        Me.mnuPreview_Spectroscopy_LogY.Size = New System.Drawing.Size(284, 22)
        Me.mnuPreview_Spectroscopy_LogY.Text = "logarithmic y scale"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(281, 6)
        '
        'mnuPreview_Spectroscopy_PointReduction
        '
        Me.mnuPreview_Spectroscopy_PointReduction.CheckOnClick = True
        Me.mnuPreview_Spectroscopy_PointReduction.Name = "mnuPreview_Spectroscopy_PointReduction"
        Me.mnuPreview_Spectroscopy_PointReduction.Size = New System.Drawing.Size(284, 22)
        Me.mnuPreview_Spectroscopy_PointReduction.Text = "point reduction enabled"
        Me.mnuPreview_Spectroscopy_PointReduction.ToolTipText = "dynamically reduces the number of points that are plotted in the preview images" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
    "(use, if you experience a bad performance during the preview image creation)"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(281, 6)
        '
        'mnuPreview_ChannelTitle
        '
        Me.mnuPreview_ChannelTitle.Image = Global.SpectroscopyManager.My.Resources.Resources.z_16
        Me.mnuPreview_ChannelTitle.Name = "mnuPreview_ChannelTitle"
        Me.mnuPreview_ChannelTitle.Size = New System.Drawing.Size(148, 16)
        Me.mnuPreview_ChannelTitle.Text = "preview image channel:"
        '
        'mnuPreview_cbScanImageChannel
        '
        Me.mnuPreview_cbScanImageChannel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.mnuPreview_cbScanImageChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.mnuPreview_cbScanImageChannel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.mnuPreview_cbScanImageChannel.Name = "mnuPreview_cbScanImageChannel"
        Me.mnuPreview_cbScanImageChannel.Size = New System.Drawing.Size(171, 23)
        Me.mnuPreview_cbScanImageChannel.ToolTipText = "channel to be plotted in preview images of 2D data"
        '
        'mnuMultipleSpectroscopyFileActions
        '
        Me.mnuMultipleSpectroscopyFileActions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTools_OpenSpectroscopyFilesSeparately, Me.ToolStripSeparator4, Me.mnuTools_ExportWizard, Me.ToolStripSeparator13, Me.mnuTools_PlotSpectraTogether, Me.mnuTools_PlotSpectraTogetherAutomatic, Me.ToolStripSeparator2, Me.mnuTools_Visualization, Me.mnuTools_DataManipulations, Me.mnuTools_NumericManipulations, Me.mnuTools_AtomicForceMicroscopyTools, Me.ToolStripSeparator3, Me.mnuTools_SpectroscopyDataCache})
        Me.mnuMultipleSpectroscopyFileActions.Enabled = False
        Me.mnuMultipleSpectroscopyFileActions.Image = Global.SpectroscopyManager.My.Resources.Resources.plot_16
        Me.mnuMultipleSpectroscopyFileActions.Name = "mnuMultipleSpectroscopyFileActions"
        Me.mnuMultipleSpectroscopyFileActions.Size = New System.Drawing.Size(105, 20)
        Me.mnuMultipleSpectroscopyFileActions.Text = "spectroscopy"
        Me.mnuMultipleSpectroscopyFileActions.ToolTipText = "these tools apply to the current selection of spectroscopy data files"
        '
        'mnuTools_OpenSpectroscopyFilesSeparately
        '
        Me.mnuTools_OpenSpectroscopyFilesSeparately.Image = Global.SpectroscopyManager.My.Resources.Resources.plot_16
        Me.mnuTools_OpenSpectroscopyFilesSeparately.Name = "mnuTools_OpenSpectroscopyFilesSeparately"
        Me.mnuTools_OpenSpectroscopyFilesSeparately.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_OpenSpectroscopyFilesSeparately.Text = "open data files separately"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(269, 6)
        '
        'mnuTools_ExportWizard
        '
        Me.mnuTools_ExportWizard.Image = Global.SpectroscopyManager.My.Resources.Resources.export_16
        Me.mnuTools_ExportWizard.Name = "mnuTools_ExportWizard"
        Me.mnuTools_ExportWizard.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_ExportWizard.Text = "export files ..."
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(269, 6)
        '
        'mnuTools_PlotSpectraTogether
        '
        Me.mnuTools_PlotSpectraTogether.Image = Global.SpectroscopyManager.My.Resources.Resources.smooth_16
        Me.mnuTools_PlotSpectraTogether.Name = "mnuTools_PlotSpectraTogether"
        Me.mnuTools_PlotSpectraTogether.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_PlotSpectraTogether.Text = "plot selected files together"
        '
        'mnuTools_PlotSpectraTogetherAutomatic
        '
        Me.mnuTools_PlotSpectraTogetherAutomatic.CheckOnClick = True
        Me.mnuTools_PlotSpectraTogetherAutomatic.Name = "mnuTools_PlotSpectraTogetherAutomatic"
        Me.mnuTools_PlotSpectraTogetherAutomatic.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_PlotSpectraTogetherAutomatic.Text = "always plot files together on selection"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(269, 6)
        '
        'mnuTools_Visualization
        '
        Me.mnuTools_Visualization.Name = "mnuTools_Visualization"
        Me.mnuTools_Visualization.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_Visualization.Text = "visualization tools"
        '
        'mnuTools_DataManipulations
        '
        Me.mnuTools_DataManipulations.Name = "mnuTools_DataManipulations"
        Me.mnuTools_DataManipulations.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_DataManipulations.Text = "data manipulations"
        '
        'mnuTools_NumericManipulations
        '
        Me.mnuTools_NumericManipulations.Name = "mnuTools_NumericManipulations"
        Me.mnuTools_NumericManipulations.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_NumericManipulations.Text = "numeric manipulations"
        '
        'mnuTools_AtomicForceMicroscopyTools
        '
        Me.mnuTools_AtomicForceMicroscopyTools.Name = "mnuTools_AtomicForceMicroscopyTools"
        Me.mnuTools_AtomicForceMicroscopyTools.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_AtomicForceMicroscopyTools.Text = "atomic force microscopy tools"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(269, 6)
        '
        'mnuTools_SpectroscopyDataCache
        '
        Me.mnuTools_SpectroscopyDataCache.Image = Global.SpectroscopyManager.My.Resources.Resources.save_16
        Me.mnuTools_SpectroscopyDataCache.Name = "mnuTools_SpectroscopyDataCache"
        Me.mnuTools_SpectroscopyDataCache.Size = New System.Drawing.Size(272, 22)
        Me.mnuTools_SpectroscopyDataCache.Text = "persistent data storage"
        '
        'mnuMultipleScanImageFileActions
        '
        Me.mnuMultipleScanImageFileActions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTools_OpenScanImagesSeparately, Me.ToolStripSeparator5, Me.mnuTools_ScanImageDataCache, Me.ToolStripSeparator17, Me.mnuTools_OpenMovieGenerator, Me.ToolStripSeparator18})
        Me.mnuMultipleScanImageFileActions.Enabled = False
        Me.mnuMultipleScanImageFileActions.Image = Global.SpectroscopyManager.My.Resources.Resources.topography_16
        Me.mnuMultipleScanImageFileActions.Name = "mnuMultipleScanImageFileActions"
        Me.mnuMultipleScanImageFileActions.Size = New System.Drawing.Size(73, 20)
        Me.mnuMultipleScanImageFileActions.Text = "images"
        Me.mnuMultipleScanImageFileActions.ToolTipText = "these tools apply to the current selection of image data files"
        '
        'mnuTools_OpenScanImagesSeparately
        '
        Me.mnuTools_OpenScanImagesSeparately.Image = Global.SpectroscopyManager.My.Resources.Resources.topography_16
        Me.mnuTools_OpenScanImagesSeparately.Name = "mnuTools_OpenScanImagesSeparately"
        Me.mnuTools_OpenScanImagesSeparately.Size = New System.Drawing.Size(246, 22)
        Me.mnuTools_OpenScanImagesSeparately.Text = "open image files separately"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(243, 6)
        '
        'mnuTools_ScanImageDataCache
        '
        Me.mnuTools_ScanImageDataCache.Image = Global.SpectroscopyManager.My.Resources.Resources.save_16
        Me.mnuTools_ScanImageDataCache.Name = "mnuTools_ScanImageDataCache"
        Me.mnuTools_ScanImageDataCache.Size = New System.Drawing.Size(246, 22)
        Me.mnuTools_ScanImageDataCache.Text = "persistent data storage"
        '
        'mnuTools_OpenMovieGenerator
        '
        Me.mnuTools_OpenMovieGenerator.Image = Global.SpectroscopyManager.My.Resources.Resources.movie_16
        Me.mnuTools_OpenMovieGenerator.Name = "mnuTools_OpenMovieGenerator"
        Me.mnuTools_OpenMovieGenerator.Size = New System.Drawing.Size(246, 22)
        Me.mnuTools_OpenMovieGenerator.Text = "create animated gif from images"
        '
        'mnuGridTools
        '
        Me.mnuGridTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGridTools_ExportGridSpectra})
        Me.mnuGridTools.Enabled = False
        Me.mnuGridTools.Image = Global.SpectroscopyManager.My.Resources.Resources.gridplotter_16
        Me.mnuGridTools.Name = "mnuGridTools"
        Me.mnuGridTools.Size = New System.Drawing.Size(61, 20)
        Me.mnuGridTools.Text = "grids"
        '
        'mnuGridTools_ExportGridSpectra
        '
        Me.mnuGridTools_ExportGridSpectra.Image = Global.SpectroscopyManager.My.Resources.Resources.export_16
        Me.mnuGridTools_ExportGridSpectra.Name = "mnuGridTools_ExportGridSpectra"
        Me.mnuGridTools_ExportGridSpectra.Size = New System.Drawing.Size(201, 22)
        Me.mnuGridTools_ExportGridSpectra.Text = "export spectra from grid"
        '
        'mnuSpecialTools
        '
        Me.mnuSpecialTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSpecialTools_GridViewer})
        Me.mnuSpecialTools.Enabled = False
        Me.mnuSpecialTools.Image = Global.SpectroscopyManager.My.Resources.Resources.tools_special_16
        Me.mnuSpecialTools.Name = "mnuSpecialTools"
        Me.mnuSpecialTools.Size = New System.Drawing.Size(61, 20)
        Me.mnuSpecialTools.Text = "tools"
        Me.mnuSpecialTools.ToolTipText = "special tools which do not apply to the current selection," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "but handle the files " &
    "in the project folder itself"
        '
        'mnuSpecialTools_GridViewer
        '
        Me.mnuSpecialTools_GridViewer.Image = Global.SpectroscopyManager.My.Resources.Resources.gridplotter_16
        Me.mnuSpecialTools_GridViewer.Name = "mnuSpecialTools_GridViewer"
        Me.mnuSpecialTools_GridViewer.Size = New System.Drawing.Size(278, 22)
        Me.mnuSpecialTools_GridViewer.Text = "plot grid data as spectral intensity map"
        '
        'ssStatus
        '
        Me.ssStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.lblTimerStatus, Me.mnuTimerConfig})
        Me.ssStatus.Location = New System.Drawing.Point(0, 862)
        Me.ssStatus.Name = "ssStatus"
        Me.ssStatus.Size = New System.Drawing.Size(851, 26)
        Me.ssStatus.SizingGrip = False
        Me.ssStatus.TabIndex = 2
        Me.ssStatus.Text = "StatusStrip1"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(36, 21)
        Me.lblStatus.Text = "ready"
        '
        'lblTimerStatus
        '
        Me.lblTimerStatus.Name = "lblTimerStatus"
        Me.lblTimerStatus.Size = New System.Drawing.Size(768, 21)
        Me.lblTimerStatus.Spring = True
        Me.lblTimerStatus.Text = "time to refresh: 00:00:00"
        Me.lblTimerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'mnuTimerConfig
        '
        Me.mnuTimerConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.mnuTimerConfig.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.smnuTimerOnOff, Me.mnuTimerConfig_TimeHeader, Me.mnuTimerConfig_RefreshTime, Me.mnuTimerConfig_ApplyTimer})
        Me.mnuTimerConfig.Image = Global.SpectroscopyManager.My.Resources.Resources.timer_16
        Me.mnuTimerConfig.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.mnuTimerConfig.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuTimerConfig.Name = "mnuTimerConfig"
        Me.mnuTimerConfig.Size = New System.Drawing.Size(32, 24)
        Me.mnuTimerConfig.Text = "ToolStripSplitButton1"
        '
        'smnuTimerOnOff
        '
        Me.smnuTimerOnOff.Checked = True
        Me.smnuTimerOnOff.CheckState = System.Windows.Forms.CheckState.Checked
        Me.smnuTimerOnOff.Name = "smnuTimerOnOff"
        Me.smnuTimerOnOff.Size = New System.Drawing.Size(184, 22)
        Me.smnuTimerOnOff.Text = "automatic refresh on"
        '
        'mnuTimerConfig_TimeHeader
        '
        Me.mnuTimerConfig_TimeHeader.Image = Global.SpectroscopyManager.My.Resources.Resources.timer_16
        Me.mnuTimerConfig_TimeHeader.Name = "mnuTimerConfig_TimeHeader"
        Me.mnuTimerConfig_TimeHeader.Size = New System.Drawing.Size(115, 16)
        Me.mnuTimerConfig_TimeHeader.Text = "refresh period (s):"
        '
        'mnuTimerConfig_RefreshTime
        '
        Me.mnuTimerConfig_RefreshTime.Name = "mnuTimerConfig_RefreshTime"
        Me.mnuTimerConfig_RefreshTime.Size = New System.Drawing.Size(100, 23)
        Me.mnuTimerConfig_RefreshTime.Text = "60"
        Me.mnuTimerConfig_RefreshTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'mnuTimerConfig_ApplyTimer
        '
        Me.mnuTimerConfig_ApplyTimer.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_16
        Me.mnuTimerConfig_ApplyTimer.Name = "mnuTimerConfig_ApplyTimer"
        Me.mnuTimerConfig_ApplyTimer.Size = New System.Drawing.Size(184, 22)
        Me.mnuTimerConfig_ApplyTimer.Text = "apply settings"
        '
        'vListScroll
        '
        Me.vListScroll.Dock = System.Windows.Forms.DockStyle.Right
        Me.vListScroll.LargeChange = 101
        Me.vListScroll.Location = New System.Drawing.Point(834, 24)
        Me.vListScroll.Name = "vListScroll"
        Me.vListScroll.Size = New System.Drawing.Size(17, 838)
        Me.vListScroll.SmallChange = 50
        Me.vListScroll.TabIndex = 3
        '
        'ttToolTip
        '
        Me.ttToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        '
        'cmSpectroscopyFile
        '
        Me.cmSpectroscopyFile.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuSpectroscopy_Header, Me.ToolStripSeparator7, Me.cmnuSpectroscopy_SpectroscopyTableShowDetails, Me.cmnuSpectroscopy_ShowNearestScanImage, Me.cmnuSpectroscopy_OpenExportWizard, Me.ToolStripSeparator11, Me.cmnuSpectroscopy_CopyDataToClipboardOriginCompatible, Me.cmnuSpectroscopy_CopyDataToClipboard, Me.ToolStripSeparator6, Me.cmnuSpectroscopy_DataManipulations, Me.cmnuSpectroscopy_NumericManipulations, Me.cmnuSpectroscopy_AtomicForceMicroscopyTools})
        Me.cmSpectroscopyFile.Name = "dgvContextMenu"
        Me.cmSpectroscopyFile.Size = New System.Drawing.Size(282, 216)
        '
        'cmnuSpectroscopy_Header
        '
        Me.cmnuSpectroscopy_Header.Name = "cmnuSpectroscopy_Header"
        Me.cmnuSpectroscopy_Header.Size = New System.Drawing.Size(211, 15)
        Me.cmnuSpectroscopy_Header.Text = "#selected spectroscopy table file name"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(278, 6)
        '
        'cmnuSpectroscopy_SpectroscopyTableShowDetails
        '
        Me.cmnuSpectroscopy_SpectroscopyTableShowDetails.Image = Global.SpectroscopyManager.My.Resources.Resources.openfolder_16
        Me.cmnuSpectroscopy_SpectroscopyTableShowDetails.Name = "cmnuSpectroscopy_SpectroscopyTableShowDetails"
        Me.cmnuSpectroscopy_SpectroscopyTableShowDetails.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_SpectroscopyTableShowDetails.Text = "open in separate window"
        '
        'cmnuSpectroscopy_ShowNearestScanImage
        '
        Me.cmnuSpectroscopy_ShowNearestScanImage.Image = Global.SpectroscopyManager.My.Resources.Resources.topography_16
        Me.cmnuSpectroscopy_ShowNearestScanImage.Name = "cmnuSpectroscopy_ShowNearestScanImage"
        Me.cmnuSpectroscopy_ShowNearestScanImage.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_ShowNearestScanImage.Text = "show nearest image file (in time)"
        '
        'cmnuSpectroscopy_OpenExportWizard
        '
        Me.cmnuSpectroscopy_OpenExportWizard.Image = Global.SpectroscopyManager.My.Resources.Resources.export_16
        Me.cmnuSpectroscopy_OpenExportWizard.Name = "cmnuSpectroscopy_OpenExportWizard"
        Me.cmnuSpectroscopy_OpenExportWizard.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_OpenExportWizard.Text = "export file ..."
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(278, 6)
        '
        'cmnuSpectroscopy_CopyDataToClipboardOriginCompatible
        '
        Me.cmnuSpectroscopy_CopyDataToClipboardOriginCompatible.Name = "cmnuSpectroscopy_CopyDataToClipboardOriginCompatible"
        Me.cmnuSpectroscopy_CopyDataToClipboardOriginCompatible.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_CopyDataToClipboardOriginCompatible.Text = "copy to clipboard (origin compatible)"
        '
        'cmnuSpectroscopy_CopyDataToClipboard
        '
        Me.cmnuSpectroscopy_CopyDataToClipboard.Name = "cmnuSpectroscopy_CopyDataToClipboard"
        Me.cmnuSpectroscopy_CopyDataToClipboard.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_CopyDataToClipboard.Text = "copy to clipboard (english data format)"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(278, 6)
        '
        'cmnuSpectroscopy_DataManipulations
        '
        Me.cmnuSpectroscopy_DataManipulations.Name = "cmnuSpectroscopy_DataManipulations"
        Me.cmnuSpectroscopy_DataManipulations.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_DataManipulations.Text = "data manipulations"
        '
        'cmnuSpectroscopy_NumericManipulations
        '
        Me.cmnuSpectroscopy_NumericManipulations.Name = "cmnuSpectroscopy_NumericManipulations"
        Me.cmnuSpectroscopy_NumericManipulations.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_NumericManipulations.Text = "numeric manipulations"
        '
        'cmnuSpectroscopy_AtomicForceMicroscopyTools
        '
        Me.cmnuSpectroscopy_AtomicForceMicroscopyTools.Name = "cmnuSpectroscopy_AtomicForceMicroscopyTools"
        Me.cmnuSpectroscopy_AtomicForceMicroscopyTools.Size = New System.Drawing.Size(281, 22)
        Me.cmnuSpectroscopy_AtomicForceMicroscopyTools.Text = "atomic force microscopy tools"
        '
        'cmScanImageFile
        '
        Me.cmScanImageFile.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuScanImage_Header, Me.ToolStripSeparator8, Me.cmnuScanImage_OpenInSeparateWindow})
        Me.cmScanImageFile.Name = "cmScanImageFile"
        Me.cmScanImageFile.Size = New System.Drawing.Size(233, 50)
        '
        'cmnuScanImage_Header
        '
        Me.cmnuScanImage_Header.Name = "cmnuScanImage_Header"
        Me.cmnuScanImage_Header.Size = New System.Drawing.Size(172, 15)
        Me.cmnuScanImage_Header.Text = "#selected scan image file name"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(229, 6)
        '
        'cmnuScanImage_OpenInSeparateWindow
        '
        Me.cmnuScanImage_OpenInSeparateWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.openfolder_16
        Me.cmnuScanImage_OpenInSeparateWindow.Name = "cmnuScanImage_OpenInSeparateWindow"
        Me.cmnuScanImage_OpenInSeparateWindow.Size = New System.Drawing.Size(232, 22)
        Me.cmnuScanImage_OpenInSeparateWindow.Text = "open in separate window"
        '
        'cmGridFile
        '
        Me.cmGridFile.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuGridFile_Header, Me.ToolStripSeparator16, Me.cmnuGridFile_OpenInSeparateWindow, Me.cmnuGridFile_ExportGridSpectra})
        Me.cmGridFile.Name = "cmScanImageFile"
        Me.cmGridFile.Size = New System.Drawing.Size(207, 72)
        '
        'cmnuGridFile_Header
        '
        Me.cmnuGridFile_Header.Name = "cmnuGridFile_Header"
        Me.cmnuGridFile_Header.Size = New System.Drawing.Size(133, 15)
        Me.cmnuGridFile_Header.Text = "#selected grid file name"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(203, 6)
        '
        'cmnuGridFile_OpenInSeparateWindow
        '
        Me.cmnuGridFile_OpenInSeparateWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.openfolder_16
        Me.cmnuGridFile_OpenInSeparateWindow.Name = "cmnuGridFile_OpenInSeparateWindow"
        Me.cmnuGridFile_OpenInSeparateWindow.Size = New System.Drawing.Size(206, 22)
        Me.cmnuGridFile_OpenInSeparateWindow.Text = "open in separate window"
        '
        'cmnuGridFile_ExportGridSpectra
        '
        Me.cmnuGridFile_ExportGridSpectra.Image = Global.SpectroscopyManager.My.Resources.Resources.export_16
        Me.cmnuGridFile_ExportGridSpectra.Name = "cmnuGridFile_ExportGridSpectra"
        Me.cmnuGridFile_ExportGridSpectra.Size = New System.Drawing.Size(206, 22)
        Me.cmnuGridFile_ExportGridSpectra.Text = "export spectra from grid"
        '
        'panBrowserListContainer
        '
        Me.panBrowserListContainer.Controls.Add(Me.panProgress)
        Me.panBrowserListContainer.Controls.Add(Me.panBrowserList)
        Me.panBrowserListContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panBrowserListContainer.Location = New System.Drawing.Point(0, 24)
        Me.panBrowserListContainer.Name = "panBrowserListContainer"
        'TODO: Ausnahme "Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression." beim Generieren des Codes für "".
        Me.panBrowserListContainer.Size = New System.Drawing.Size(834, 838)
        Me.panBrowserListContainer.SuspendMessageFiltering = False
        Me.panBrowserListContainer.TabIndex = 1
        '
        'panProgress
        '
        Me.panProgress.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.panProgress.Controls.Add(Me.lblProgressHeading)
        Me.panProgress.Controls.Add(Me.pbProgress)
        Me.panProgress.Location = New System.Drawing.Point(0, 764)
        Me.panProgress.Name = "panProgress"
        Me.panProgress.Size = New System.Drawing.Size(834, 74)
        Me.panProgress.TabIndex = 2
        '
        'lblProgressHeading
        '
        Me.lblProgressHeading.AutoSize = True
        Me.lblProgressHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgressHeading.Location = New System.Drawing.Point(11, 14)
        Me.lblProgressHeading.Name = "lblProgressHeading"
        Me.lblProgressHeading.Size = New System.Drawing.Size(388, 20)
        Me.lblProgressHeading.TabIndex = 1
        Me.lblProgressHeading.Text = "Your request is being processed ... please wait!"
        '
        'pbProgress
        '
        Me.pbProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbProgress.Location = New System.Drawing.Point(15, 39)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(804, 23)
        Me.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pbProgress.TabIndex = 0
        '
        'panBrowserList
        '
        Me.panBrowserList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panBrowserList.Location = New System.Drawing.Point(3, 3)
        Me.panBrowserList.Name = "panBrowserList"
        Me.panBrowserList.ScrollOffset = 0
        Me.panBrowserList.Size = New System.Drawing.Size(814, 157)
        Me.panBrowserList.TabIndex = 1
        Me.panBrowserList.VirtualHeightOfThePanel = 100
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(243, 6)
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(243, 6)
        '
        'mDataBrowserList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.panBrowserListContainer)
        Me.Controls.Add(Me.vListScroll)
        Me.Controls.Add(Me.mnu)
        Me.Controls.Add(Me.ssStatus)
        Me.Name = "mDataBrowserList"
        Me.Size = New System.Drawing.Size(851, 888)
        Me.mnu.ResumeLayout(False)
        Me.mnu.PerformLayout()
        Me.ssStatus.ResumeLayout(False)
        Me.ssStatus.PerformLayout()
        Me.cmSpectroscopyFile.ResumeLayout(False)
        Me.cmScanImageFile.ResumeLayout(False)
        Me.cmGridFile.ResumeLayout(False)
        Me.panBrowserListContainer.ResumeLayout(False)
        Me.panProgress.ResumeLayout(False)
        Me.panProgress.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnu As System.Windows.Forms.MenuStrip
    Friend WithEvents panBrowserListContainer As MouseBoundPanel
    Friend WithEvents mnuRefreshList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ssStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblTimerStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents mnuTimerConfig As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents smnuTimerOnOff As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPreviewSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents panBrowserList As VirtualVerticalPanel
    Friend WithEvents vListScroll As System.Windows.Forms.VScrollBar
    Friend WithEvents mnuSorting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSort_ByFileName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSort_ByFileDate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSort_ByRecordDate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilter_ShowDataTableFiles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilter_ShowScanImageFiles As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSort_ASC As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSort_DESC As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMultipleSpectroscopyFileActions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools_ExportWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuTools_Visualization As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools_DataManipulations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools_NumericManipulations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuTools_OpenSpectroscopyFilesSeparately As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuTools_SpectroscopyDataCache As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSort_BySelection As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ttToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents panProgress As System.Windows.Forms.Panel
    Friend WithEvents lblProgressHeading As System.Windows.Forms.Label
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents mnuMultipleScanImageFileActions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools_OpenScanImagesSeparately As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuTools_ScanImageDataCache As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmSpectroscopyFile As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuSpectroscopy_ShowNearestScanImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuSpectroscopy_OpenExportWizard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSpectroscopy_CopyDataToClipboardOriginCompatible As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSpectroscopy_CopyDataToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuSpectroscopy_SpectroscopyTableShowDetails As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSpectroscopy_DataManipulations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSpectroscopy_NumericManipulations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTools_PlotSpectraTogether As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSpectroscopy_Header As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmScanImageFile As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuScanImage_OpenInSeparateWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuScanImage_Header As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPreview_XTitle As System.Windows.Forms.ToolStripLabel
    Friend WithEvents mnuPreview_cbSpectroscopyColumnX_1st As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents mnuPreview_cbSpectroscopyColumnY_1st As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents mnuPreview_Spectroscopy_LogY As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuPreview_Spectroscopy_LogX As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPreview_ChannelTitle As System.Windows.Forms.ToolStripLabel
    Friend WithEvents mnuPreview_cbScanImageChannel As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents mnuPreview_YTitle As System.Windows.Forms.ToolStripLabel
    Friend WithEvents mnuPreview_Spectroscopy_PointReduction As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFilter_Title As System.Windows.Forms.ToolStripLabel
    Friend WithEvents mnuFilter_FilterText As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents mnuFilter_AddToHistory As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilter_FilterHistory As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilter_ClearFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilter_SearchTitle As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFilter_SearchText As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents mnuFilter_SearchNext As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilter_SearchClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSpecialTools As ToolStripMenuItem
    Friend WithEvents mnuSpecialTools_GridViewer As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents mnuTools_PlotSpectraTogetherAutomatic As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents mnuPreview_cbSpectroscopyColumnX_2nd As ToolStripComboBox
    Friend WithEvents mnuPreview_cbSpectroscopyColumnX_3rd As ToolStripComboBox
    Friend WithEvents mnuPreview_cbSpectroscopyColumnY_2nd As ToolStripComboBox
    Friend WithEvents mnuPreview_cbSpectroscopyColumnY_3rd As ToolStripComboBox
    Friend WithEvents mnuTools_AtomicForceMicroscopyTools As ToolStripMenuItem
    Friend WithEvents cmnuSpectroscopy_AtomicForceMicroscopyTools As ToolStripMenuItem
    Friend WithEvents cmGridFile As ContextMenuStrip
    Friend WithEvents cmnuGridFile_Header As ToolStripLabel
    Friend WithEvents ToolStripSeparator16 As ToolStripSeparator
    Friend WithEvents cmnuGridFile_OpenInSeparateWindow As ToolStripMenuItem
    Friend WithEvents mnuFilter_ShowGridFiles As ToolStripMenuItem
    Friend WithEvents mnuTimerConfig_RefreshTime As ToolStripTextBox
    Friend WithEvents mnuTimerConfig_TimeHeader As ToolStripLabel
    Friend WithEvents mnuTimerConfig_ApplyTimer As ToolStripMenuItem
    Friend WithEvents mnuGridTools As ToolStripMenuItem
    Friend WithEvents mnuGridTools_ExportGridSpectra As ToolStripMenuItem
    Friend WithEvents cmnuGridFile_ExportGridSpectra As ToolStripMenuItem
    Friend WithEvents mnuTools_OpenMovieGenerator As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
End Class
