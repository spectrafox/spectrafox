<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wMain2
    Inherits SpectroscopyManager.wFormBase

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.tmnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmnuInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmnuShowScanPane = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmnuShowSpectroscopyPane = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmnuShowFolderPane = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmnuReloadFileList = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmnuTest = New System.Windows.Forms.ToolStripMenuItem()
        Me.scMainSplit = New System.Windows.Forms.SplitContainer()
        Me.scSpectroscopyFiles = New System.Windows.Forms.SplitContainer()
        Me.pbPreviewBox = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.slSpectroscopyTableList = New SpectroscopyManager.mSpectroscopyTableFileList()
        Me.scScanImage = New System.Windows.Forms.SplitContainer()
        Me.svScanViewer = New SpectroscopyManager.mScanImageViewer()
        Me.slScanImageList = New SpectroscopyManager.mScanImageList()
        Me.MainStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.pgProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.lblProgressBar = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pSpectroscopyTableProperties = New SpectroscopyManager.DockablePanel()
        Me.gbSpectroscopyTablePropertyGrid = New System.Windows.Forms.GroupBox()
        Me.pgSpectrumProperies = New System.Windows.Forms.PropertyGrid()
        Me.gbPossibleScanImages = New System.Windows.Forms.GroupBox()
        Me.lbPossibleScanImages = New System.Windows.Forms.ListBox()
        Me.gbSpectraComment = New System.Windows.Forms.GroupBox()
        Me.txtSpectraComment = New System.Windows.Forms.TextBox()
        Me.pFileBrowser = New SpectroscopyManager.DockablePanel()
        Me.btnRefreshList = New System.Windows.Forms.Button()
        Me.dbDictionaryBrowser = New ExpTreeLib.ExpTree()
        Me.pScanImageProperties = New SpectroscopyManager.DockablePanel()
        Me.gbScanImagePropertyGrid = New System.Windows.Forms.GroupBox()
        Me.pgImageProperies = New System.Windows.Forms.PropertyGrid()
        Me.gbPossibleSpectra = New System.Windows.Forms.GroupBox()
        Me.lbPossibleSpectra = New System.Windows.Forms.ListBox()
        Me.gbImageComment = New System.Windows.Forms.GroupBox()
        Me.txtImageComment = New System.Windows.Forms.TextBox()
        Me.MainMenu.SuspendLayout()
        CType(Me.scMainSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMainSplit.Panel1.SuspendLayout()
        Me.scMainSplit.Panel2.SuspendLayout()
        Me.scMainSplit.SuspendLayout()
        CType(Me.scSpectroscopyFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scSpectroscopyFiles.Panel1.SuspendLayout()
        Me.scSpectroscopyFiles.Panel2.SuspendLayout()
        Me.scSpectroscopyFiles.SuspendLayout()
        CType(Me.scScanImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scScanImage.Panel1.SuspendLayout()
        Me.scScanImage.Panel2.SuspendLayout()
        Me.scScanImage.SuspendLayout()
        Me.MainStatusStrip.SuspendLayout()
        Me.pSpectroscopyTableProperties.SuspendLayout()
        Me.gbSpectroscopyTablePropertyGrid.SuspendLayout()
        Me.gbPossibleScanImages.SuspendLayout()
        Me.gbSpectraComment.SuspendLayout()
        Me.pFileBrowser.SuspendLayout()
        Me.pScanImageProperties.SuspendLayout()
        Me.gbScanImagePropertyGrid.SuspendLayout()
        Me.gbPossibleSpectra.SuspendLayout()
        Me.gbImageComment.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu
        '
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tmnuExit, Me.tmnuInfo, Me.tmnuShowScanPane, Me.tmnuShowSpectroscopyPane, Me.tmnuShowFolderPane, Me.tmnuReloadFileList, Me.tmnuTest})
        Me.MainMenu.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.Size = New System.Drawing.Size(1584, 24)
        Me.MainMenu.TabIndex = 19
        Me.MainMenu.Text = "MenuStrip1"
        '
        'tmnuExit
        '
        Me.tmnuExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tmnuExit.Image = Global.SpectroscopyManager.My.Resources.Resources.power_16
        Me.tmnuExit.Name = "tmnuExit"
        Me.tmnuExit.Size = New System.Drawing.Size(28, 20)
        Me.tmnuExit.Text = "Program"
        '
        'tmnuInfo
        '
        Me.tmnuInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tmnuInfo.Image = Global.SpectroscopyManager.My.Resources.Resources.info_16
        Me.tmnuInfo.Name = "tmnuInfo"
        Me.tmnuInfo.Size = New System.Drawing.Size(28, 20)
        Me.tmnuInfo.Text = "Info"
        '
        'tmnuShowScanPane
        '
        Me.tmnuShowScanPane.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tmnuShowScanPane.Image = Global.SpectroscopyManager.My.Resources.Resources.expandpanel_16
        Me.tmnuShowScanPane.Name = "tmnuShowScanPane"
        Me.tmnuShowScanPane.Size = New System.Drawing.Size(180, 20)
        Me.tmnuShowScanPane.Text = "Show/Hide Scan-Properties"
        '
        'tmnuShowSpectroscopyPane
        '
        Me.tmnuShowSpectroscopyPane.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tmnuShowSpectroscopyPane.Image = Global.SpectroscopyManager.My.Resources.Resources.expandpanel_16
        Me.tmnuShowSpectroscopyPane.Name = "tmnuShowSpectroscopyPane"
        Me.tmnuShowSpectroscopyPane.Size = New System.Drawing.Size(226, 20)
        Me.tmnuShowSpectroscopyPane.Text = "Show/Hide Spectroscopy-Properties"
        '
        'tmnuShowFolderPane
        '
        Me.tmnuShowFolderPane.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tmnuShowFolderPane.Image = Global.SpectroscopyManager.My.Resources.Resources.expandpanel_16
        Me.tmnuShowFolderPane.Name = "tmnuShowFolderPane"
        Me.tmnuShowFolderPane.Size = New System.Drawing.Size(183, 20)
        Me.tmnuShowFolderPane.Text = "Show/Hide Folder-Selection"
        '
        'tmnuReloadFileList
        '
        Me.tmnuReloadFileList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tmnuReloadFileList.Image = Global.SpectroscopyManager.My.Resources.Resources.reload_16
        Me.tmnuReloadFileList.Name = "tmnuReloadFileList"
        Me.tmnuReloadFileList.Size = New System.Drawing.Size(115, 20)
        Me.tmnuReloadFileList.Text = "Reload File-List"
        '
        'tmnuTest
        '
        Me.tmnuTest.Name = "tmnuTest"
        Me.tmnuTest.Size = New System.Drawing.Size(41, 20)
        Me.tmnuTest.Text = "Test"
        '
        'scMainSplit
        '
        Me.scMainSplit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.scMainSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMainSplit.Location = New System.Drawing.Point(440, 24)
        Me.scMainSplit.Name = "scMainSplit"
        '
        'scMainSplit.Panel1
        '
        Me.scMainSplit.Panel1.Controls.Add(Me.scSpectroscopyFiles)
        '
        'scMainSplit.Panel2
        '
        Me.scMainSplit.Panel2.Controls.Add(Me.scScanImage)
        Me.scMainSplit.Size = New System.Drawing.Size(933, 938)
        Me.scMainSplit.SplitterDistance = 464
        Me.scMainSplit.TabIndex = 20
        '
        'scSpectroscopyFiles
        '
        Me.scSpectroscopyFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scSpectroscopyFiles.Location = New System.Drawing.Point(0, 0)
        Me.scSpectroscopyFiles.Name = "scSpectroscopyFiles"
        Me.scSpectroscopyFiles.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scSpectroscopyFiles.Panel1
        '
        Me.scSpectroscopyFiles.Panel1.Controls.Add(Me.pbPreviewBox)
        '
        'scSpectroscopyFiles.Panel2
        '
        Me.scSpectroscopyFiles.Panel2.Controls.Add(Me.slSpectroscopyTableList)
        Me.scSpectroscopyFiles.Size = New System.Drawing.Size(462, 936)
        Me.scSpectroscopyFiles.SplitterDistance = 360
        Me.scSpectroscopyFiles.TabIndex = 10
        '
        'pbPreviewBox
        '
        Me.pbPreviewBox.AllowAdjustingXColumn = True
        Me.pbPreviewBox.AllowAdjustingYColumn = True
        Me.pbPreviewBox.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbPreviewBox.CallbackXRangeSelected = Nothing
        Me.pbPreviewBox.CallbackXValueSelected = Nothing
        Me.pbPreviewBox.CallbackXYRangeSelected = Nothing
        Me.pbPreviewBox.CallbackYRangeSelected = Nothing
        Me.pbPreviewBox.CallbackYValueSelected = Nothing
        Me.pbPreviewBox.ClearPointSelectionModeAfterSelection = False
        Me.pbPreviewBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbPreviewBox.Location = New System.Drawing.Point(0, 0)
        Me.pbPreviewBox.Name = "pbPreviewBox"
        Me.pbPreviewBox.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbPreviewBox.ShowColumnSelectors = True
        Me.pbPreviewBox.Size = New System.Drawing.Size(462, 360)
        Me.pbPreviewBox.TabIndex = 13
        Me.pbPreviewBox.TotalNumberOfFilesToFetch = 0
        '
        'slSpectroscopyTableList
        '
        Me.slSpectroscopyTableList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.slSpectroscopyTableList.Location = New System.Drawing.Point(0, 0)
        Me.slSpectroscopyTableList.Name = "slSpectroscopyTableList"
        Me.slSpectroscopyTableList.Size = New System.Drawing.Size(462, 572)
        Me.slSpectroscopyTableList.TabIndex = 0
        Me.slSpectroscopyTableList.TotalNumberOfFilesToFetch = 0
        '
        'scScanImage
        '
        Me.scScanImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scScanImage.Location = New System.Drawing.Point(0, 0)
        Me.scScanImage.Name = "scScanImage"
        Me.scScanImage.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scScanImage.Panel1
        '
        Me.scScanImage.Panel1.Controls.Add(Me.svScanViewer)
        '
        'scScanImage.Panel2
        '
        Me.scScanImage.Panel2.Controls.Add(Me.slScanImageList)
        Me.scScanImage.Size = New System.Drawing.Size(463, 936)
        Me.scScanImage.SplitterDistance = 360
        Me.scScanImage.TabIndex = 2
        '
        'svScanViewer
        '
        Me.svScanViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svScanViewer.Location = New System.Drawing.Point(0, 0)
        Me.svScanViewer.Name = "svScanViewer"
        Me.svScanViewer.Size = New System.Drawing.Size(463, 360)
        Me.svScanViewer.TabIndex = 0
        '
        'slScanImageList
        '
        Me.slScanImageList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.slScanImageList.Location = New System.Drawing.Point(0, 0)
        Me.slScanImageList.Name = "slScanImageList"
        Me.slScanImageList.Size = New System.Drawing.Size(463, 572)
        Me.slScanImageList.TabIndex = 0
        '
        'MainStatusStrip
        '
        Me.MainStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.pgProgress, Me.lblProgressBar})
        Me.MainStatusStrip.Location = New System.Drawing.Point(440, 940)
        Me.MainStatusStrip.Name = "MainStatusStrip"
        Me.MainStatusStrip.Size = New System.Drawing.Size(933, 22)
        Me.MainStatusStrip.TabIndex = 22
        Me.MainStatusStrip.Text = "StatusStrip1"
        '
        'pgProgress
        '
        Me.pgProgress.Name = "pgProgress"
        Me.pgProgress.Size = New System.Drawing.Size(400, 16)
        '
        'lblProgressBar
        '
        Me.lblProgressBar.Name = "lblProgressBar"
        Me.lblProgressBar.Size = New System.Drawing.Size(82, 17)
        Me.lblProgressBar.Text = "lblProgressBar"
        '
        'pSpectroscopyTableProperties
        '
        Me.pSpectroscopyTableProperties.Controls.Add(Me.gbSpectroscopyTablePropertyGrid)
        Me.pSpectroscopyTableProperties.Controls.Add(Me.gbPossibleScanImages)
        Me.pSpectroscopyTableProperties.Controls.Add(Me.gbSpectraComment)
        Me.pSpectroscopyTableProperties.Dock = System.Windows.Forms.DockStyle.Left
        Me.pSpectroscopyTableProperties.Location = New System.Drawing.Point(206, 24)
        Me.pSpectroscopyTableProperties.Name = "pSpectroscopyTableProperties"
        Me.pSpectroscopyTableProperties.Size = New System.Drawing.Size(234, 938)
        Me.pSpectroscopyTableProperties.SlidePixelsPerTimerTick = 25
        Me.pSpectroscopyTableProperties.TabIndex = 21
        '
        'gbSpectroscopyTablePropertyGrid
        '
        Me.gbSpectroscopyTablePropertyGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSpectroscopyTablePropertyGrid.Controls.Add(Me.pgSpectrumProperies)
        Me.gbSpectroscopyTablePropertyGrid.Location = New System.Drawing.Point(3, 3)
        Me.gbSpectroscopyTablePropertyGrid.Name = "gbSpectroscopyTablePropertyGrid"
        Me.gbSpectroscopyTablePropertyGrid.Size = New System.Drawing.Size(229, 501)
        Me.gbSpectroscopyTablePropertyGrid.TabIndex = 1
        Me.gbSpectroscopyTablePropertyGrid.TabStop = False
        Me.gbSpectroscopyTablePropertyGrid.Text = "File Properties"
        '
        'pgSpectrumProperies
        '
        Me.pgSpectrumProperies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgSpectrumProperies.HelpVisible = False
        Me.pgSpectrumProperies.Location = New System.Drawing.Point(3, 16)
        Me.pgSpectrumProperies.Name = "pgSpectrumProperies"
        Me.pgSpectrumProperies.Size = New System.Drawing.Size(223, 482)
        Me.pgSpectrumProperies.TabIndex = 12
        Me.pgSpectrumProperies.ToolbarVisible = False
        '
        'gbPossibleScanImages
        '
        Me.gbPossibleScanImages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPossibleScanImages.Controls.Add(Me.lbPossibleScanImages)
        Me.gbPossibleScanImages.Location = New System.Drawing.Point(3, 507)
        Me.gbPossibleScanImages.Name = "gbPossibleScanImages"
        Me.gbPossibleScanImages.Size = New System.Drawing.Size(226, 119)
        Me.gbPossibleScanImages.TabIndex = 10
        Me.gbPossibleScanImages.TabStop = False
        Me.gbPossibleScanImages.Text = "possible Scan-Images"
        '
        'lbPossibleScanImages
        '
        Me.lbPossibleScanImages.DisplayMember = "Value"
        Me.lbPossibleScanImages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbPossibleScanImages.FormattingEnabled = True
        Me.lbPossibleScanImages.Location = New System.Drawing.Point(3, 16)
        Me.lbPossibleScanImages.Name = "lbPossibleScanImages"
        Me.lbPossibleScanImages.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.lbPossibleScanImages.Size = New System.Drawing.Size(220, 100)
        Me.lbPossibleScanImages.TabIndex = 0
        Me.lbPossibleScanImages.ValueMember = "Key"
        '
        'gbSpectraComment
        '
        Me.gbSpectraComment.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSpectraComment.Controls.Add(Me.txtSpectraComment)
        Me.gbSpectraComment.Location = New System.Drawing.Point(3, 632)
        Me.gbSpectraComment.Name = "gbSpectraComment"
        Me.gbSpectraComment.Size = New System.Drawing.Size(229, 305)
        Me.gbSpectraComment.TabIndex = 18
        Me.gbSpectraComment.TabStop = False
        Me.gbSpectraComment.Text = "File-Comment"
        '
        'txtSpectraComment
        '
        Me.txtSpectraComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSpectraComment.Location = New System.Drawing.Point(3, 16)
        Me.txtSpectraComment.Multiline = True
        Me.txtSpectraComment.Name = "txtSpectraComment"
        Me.txtSpectraComment.ReadOnly = True
        Me.txtSpectraComment.Size = New System.Drawing.Size(223, 286)
        Me.txtSpectraComment.TabIndex = 0
        '
        'pFileBrowser
        '
        Me.pFileBrowser.Controls.Add(Me.btnRefreshList)
        Me.pFileBrowser.Controls.Add(Me.dbDictionaryBrowser)
        Me.pFileBrowser.Dock = System.Windows.Forms.DockStyle.Left
        Me.pFileBrowser.Location = New System.Drawing.Point(0, 24)
        Me.pFileBrowser.Name = "pFileBrowser"
        Me.pFileBrowser.Size = New System.Drawing.Size(206, 938)
        Me.pFileBrowser.SlidePixelsPerTimerTick = 25
        Me.pFileBrowser.TabIndex = 1
        '
        'btnRefreshList
        '
        Me.btnRefreshList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefreshList.Image = Global.SpectroscopyManager.My.Resources.Resources.reload_25
        Me.btnRefreshList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefreshList.Location = New System.Drawing.Point(3, 3)
        Me.btnRefreshList.Name = "btnRefreshList"
        Me.btnRefreshList.Size = New System.Drawing.Size(200, 41)
        Me.btnRefreshList.TabIndex = 20
        Me.btnRefreshList.Text = "Reload File-List"
        Me.btnRefreshList.UseVisualStyleBackColor = True
        '
        'dbDictionaryBrowser
        '
        Me.dbDictionaryBrowser.AllowDrop = True
        Me.dbDictionaryBrowser.AllowFolderRename = False
        Me.dbDictionaryBrowser.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dbDictionaryBrowser.Cursor = System.Windows.Forms.Cursors.Default
        Me.dbDictionaryBrowser.Location = New System.Drawing.Point(3, 48)
        Me.dbDictionaryBrowser.Name = "dbDictionaryBrowser"
        Me.dbDictionaryBrowser.ShowHiddenFolders = False
        Me.dbDictionaryBrowser.ShowRootLines = False
        Me.dbDictionaryBrowser.Size = New System.Drawing.Size(200, 886)
        Me.dbDictionaryBrowser.StartUpDirectory = ExpTreeLib.ExpTree.StartDir.MyComputer
        Me.dbDictionaryBrowser.TabIndex = 19
        '
        'pScanImageProperties
        '
        Me.pScanImageProperties.Controls.Add(Me.gbScanImagePropertyGrid)
        Me.pScanImageProperties.Controls.Add(Me.gbPossibleSpectra)
        Me.pScanImageProperties.Controls.Add(Me.gbImageComment)
        Me.pScanImageProperties.Dock = System.Windows.Forms.DockStyle.Right
        Me.pScanImageProperties.Location = New System.Drawing.Point(1373, 24)
        Me.pScanImageProperties.Name = "pScanImageProperties"
        Me.pScanImageProperties.Size = New System.Drawing.Size(211, 938)
        Me.pScanImageProperties.SlidePixelsPerTimerTick = 25
        Me.pScanImageProperties.TabIndex = 21
        '
        'gbScanImagePropertyGrid
        '
        Me.gbScanImagePropertyGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbScanImagePropertyGrid.Controls.Add(Me.pgImageProperies)
        Me.gbScanImagePropertyGrid.Location = New System.Drawing.Point(3, 3)
        Me.gbScanImagePropertyGrid.Name = "gbScanImagePropertyGrid"
        Me.gbScanImagePropertyGrid.Size = New System.Drawing.Size(206, 501)
        Me.gbScanImagePropertyGrid.TabIndex = 1
        Me.gbScanImagePropertyGrid.TabStop = False
        Me.gbScanImagePropertyGrid.Text = "File Properties"
        '
        'pgImageProperies
        '
        Me.pgImageProperies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgImageProperies.HelpVisible = False
        Me.pgImageProperies.Location = New System.Drawing.Point(3, 16)
        Me.pgImageProperies.Name = "pgImageProperies"
        Me.pgImageProperies.Size = New System.Drawing.Size(200, 482)
        Me.pgImageProperies.TabIndex = 12
        Me.pgImageProperies.ToolbarVisible = False
        '
        'gbPossibleSpectra
        '
        Me.gbPossibleSpectra.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPossibleSpectra.Controls.Add(Me.lbPossibleSpectra)
        Me.gbPossibleSpectra.Location = New System.Drawing.Point(3, 507)
        Me.gbPossibleSpectra.Name = "gbPossibleSpectra"
        Me.gbPossibleSpectra.Size = New System.Drawing.Size(203, 119)
        Me.gbPossibleSpectra.TabIndex = 10
        Me.gbPossibleSpectra.TabStop = False
        Me.gbPossibleSpectra.Text = "possible Spectra"
        '
        'lbPossibleSpectra
        '
        Me.lbPossibleSpectra.DisplayMember = "Value"
        Me.lbPossibleSpectra.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbPossibleSpectra.FormattingEnabled = True
        Me.lbPossibleSpectra.Location = New System.Drawing.Point(3, 16)
        Me.lbPossibleSpectra.Name = "lbPossibleSpectra"
        Me.lbPossibleSpectra.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.lbPossibleSpectra.Size = New System.Drawing.Size(197, 100)
        Me.lbPossibleSpectra.TabIndex = 0
        Me.lbPossibleSpectra.ValueMember = "Key"
        '
        'gbImageComment
        '
        Me.gbImageComment.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbImageComment.Controls.Add(Me.txtImageComment)
        Me.gbImageComment.Location = New System.Drawing.Point(3, 632)
        Me.gbImageComment.Name = "gbImageComment"
        Me.gbImageComment.Size = New System.Drawing.Size(206, 305)
        Me.gbImageComment.TabIndex = 18
        Me.gbImageComment.TabStop = False
        Me.gbImageComment.Text = "File-Comment"
        '
        'txtImageComment
        '
        Me.txtImageComment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtImageComment.Location = New System.Drawing.Point(3, 16)
        Me.txtImageComment.Multiline = True
        Me.txtImageComment.Name = "txtImageComment"
        Me.txtImageComment.ReadOnly = True
        Me.txtImageComment.Size = New System.Drawing.Size(200, 286)
        Me.txtImageComment.TabIndex = 0
        '
        'wMain2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1584, 962)
        Me.Controls.Add(Me.MainStatusStrip)
        Me.Controls.Add(Me.scMainSplit)
        Me.Controls.Add(Me.pSpectroscopyTableProperties)
        Me.Controls.Add(Me.pFileBrowser)
        Me.Controls.Add(Me.pScanImageProperties)
        Me.Controls.Add(Me.MainMenu)
        Me.MainMenuStrip = Me.MainMenu
        Me.Name = "wMain2"
        Me.Opacity = 1.0R
        Me.Text = "File List"
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.scMainSplit.Panel1.ResumeLayout(False)
        Me.scMainSplit.Panel2.ResumeLayout(False)
        CType(Me.scMainSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMainSplit.ResumeLayout(False)
        Me.scSpectroscopyFiles.Panel1.ResumeLayout(False)
        Me.scSpectroscopyFiles.Panel2.ResumeLayout(False)
        CType(Me.scSpectroscopyFiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scSpectroscopyFiles.ResumeLayout(False)
        Me.scScanImage.Panel1.ResumeLayout(False)
        Me.scScanImage.Panel2.ResumeLayout(False)
        CType(Me.scScanImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scScanImage.ResumeLayout(False)
        Me.MainStatusStrip.ResumeLayout(False)
        Me.MainStatusStrip.PerformLayout()
        Me.pSpectroscopyTableProperties.ResumeLayout(False)
        Me.gbSpectroscopyTablePropertyGrid.ResumeLayout(False)
        Me.gbPossibleScanImages.ResumeLayout(False)
        Me.gbSpectraComment.ResumeLayout(False)
        Me.gbSpectraComment.PerformLayout()
        Me.pFileBrowser.ResumeLayout(False)
        Me.pScanImageProperties.ResumeLayout(False)
        Me.gbScanImagePropertyGrid.ResumeLayout(False)
        Me.gbPossibleSpectra.ResumeLayout(False)
        Me.gbImageComment.ResumeLayout(False)
        Me.gbImageComment.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pgSpectrumProperies As System.Windows.Forms.PropertyGrid
    Friend WithEvents pbPreviewBox As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents tmnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmnuInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents svScanViewer As SpectroscopyManager.mScanImageViewer
    Friend WithEvents txtSpectraComment As System.Windows.Forms.TextBox
    Friend WithEvents lbPossibleScanImages As System.Windows.Forms.ListBox
    Friend WithEvents scMainSplit As System.Windows.Forms.SplitContainer
    Friend WithEvents pSpectroscopyTableProperties As DockablePanel
    Friend WithEvents MainStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents pgProgress As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents lblProgressBar As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents scSpectroscopyFiles As System.Windows.Forms.SplitContainer
    Friend WithEvents scScanImage As System.Windows.Forms.SplitContainer
    Friend WithEvents gbSpectraComment As System.Windows.Forms.GroupBox
    Friend WithEvents gbSpectroscopyTablePropertyGrid As System.Windows.Forms.GroupBox
    Friend WithEvents dbDictionaryBrowser As ExpTreeLib.ExpTree
    Friend WithEvents btnRefreshList As System.Windows.Forms.Button
    Friend WithEvents gbPossibleScanImages As System.Windows.Forms.GroupBox
    Friend WithEvents slScanImageList As SpectroscopyManager.mScanImageList
    Friend WithEvents slSpectroscopyTableList As SpectroscopyManager.mSpectroscopyTableFileList
    Friend WithEvents pFileBrowser As DockablePanel
    Friend WithEvents pScanImageProperties As DockablePanel
    Friend WithEvents gbScanImagePropertyGrid As System.Windows.Forms.GroupBox
    Friend WithEvents pgImageProperies As System.Windows.Forms.PropertyGrid
    Friend WithEvents gbPossibleSpectra As System.Windows.Forms.GroupBox
    Friend WithEvents lbPossibleSpectra As System.Windows.Forms.ListBox
    Friend WithEvents gbImageComment As System.Windows.Forms.GroupBox
    Friend WithEvents txtImageComment As System.Windows.Forms.TextBox
    Friend WithEvents tmnuShowScanPane As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmnuShowFolderPane As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmnuShowSpectroscopyPane As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmnuReloadFileList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmnuTest As System.Windows.Forms.ToolStripMenuItem

End Class
