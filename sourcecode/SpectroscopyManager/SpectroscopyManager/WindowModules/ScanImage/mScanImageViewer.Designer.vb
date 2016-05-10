<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mScanImageViewer
    Inherits System.Windows.Forms.UserControl

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mScanImageViewer))
        Me.pbScanImage = New System.Windows.Forms.PictureBox()
        Me.cmnuImageContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuCopyImageToClipboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmnuSaveAsImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsTools = New System.Windows.Forms.ToolStrip()
        Me.tsbtnPlotSetup = New System.Windows.Forms.ToolStripButton()
        Me.tssRight = New System.Windows.Forms.ToolStripSeparator()
        Me.btnTool_LineProfile = New System.Windows.Forms.ToolStripButton()
        Me.tslblTools = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnChannelSetup = New System.Windows.Forms.ToolStripButton()
        Me.tssLeft1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tslblInfobar = New System.Windows.Forms.ToolStripLabel()
        Me.dpLeft = New SpectroscopyManager.DockablePanel()
        Me.gbOpenExternal = New System.Windows.Forms.GroupBox()
        Me.btnOpenExternal = New System.Windows.Forms.Button()
        Me.cboExternalViewer = New System.Windows.Forms.ComboBox()
        Me.gbChannel = New System.Windows.Forms.GroupBox()
        Me.cbChannel = New SpectroscopyManager.ucScanChannelSelector()
        Me.btnFilterSettings = New System.Windows.Forms.Button()
        Me.btnRemoveFilter = New System.Windows.Forms.Button()
        Me.btnAddFilter = New System.Windows.Forms.Button()
        Me.cbAddFilter = New System.Windows.Forms.ComboBox()
        Me.lbFilters = New System.Windows.Forms.ListBox()
        Me.lblFilters = New System.Windows.Forms.Label()
        Me.dpRight = New SpectroscopyManager.DockablePanel()
        Me.ckbScaleBarVisible = New System.Windows.Forms.CheckBox()
        Me.ckbUseHighQualityScaling = New System.Windows.Forms.CheckBox()
        Me.vsValueRangeSelector = New SpectroscopyManager.mValueRangeSelector()
        Me.cpColorPicker = New SpectroscopyManager.ucColorPalettePicker()
        Me.lblColorCode = New System.Windows.Forms.Label()
        Me.ttToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.pbScanImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmnuImageContextMenuStrip.SuspendLayout()
        Me.tsTools.SuspendLayout()
        Me.dpLeft.SuspendLayout()
        Me.gbOpenExternal.SuspendLayout()
        Me.gbChannel.SuspendLayout()
        Me.dpRight.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbScanImage
        '
        Me.pbScanImage.ContextMenuStrip = Me.cmnuImageContextMenuStrip
        Me.pbScanImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbScanImage.Location = New System.Drawing.Point(0, 25)
        Me.pbScanImage.Name = "pbScanImage"
        Me.pbScanImage.Size = New System.Drawing.Size(625, 503)
        Me.pbScanImage.TabIndex = 0
        Me.pbScanImage.TabStop = False
        '
        'cmnuImageContextMenuStrip
        '
        Me.cmnuImageContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuCopyImageToClipboard, Me.cmnuSaveAsImage})
        Me.cmnuImageContextMenuStrip.Name = "ImageContextMenuStrip"
        Me.cmnuImageContextMenuStrip.Size = New System.Drawing.Size(168, 48)
        '
        'cmnuCopyImageToClipboard
        '
        Me.cmnuCopyImageToClipboard.Image = Global.SpectroscopyManager.My.Resources.Resources.copy_16
        Me.cmnuCopyImageToClipboard.Name = "cmnuCopyImageToClipboard"
        Me.cmnuCopyImageToClipboard.Size = New System.Drawing.Size(167, 22)
        Me.cmnuCopyImageToClipboard.Text = "copy to clipboard"
        '
        'cmnuSaveAsImage
        '
        Me.cmnuSaveAsImage.Image = Global.SpectroscopyManager.My.Resources.Resources.export_16
        Me.cmnuSaveAsImage.Name = "cmnuSaveAsImage"
        Me.cmnuSaveAsImage.Size = New System.Drawing.Size(167, 22)
        Me.cmnuSaveAsImage.Text = "save image as file"
        '
        'tsTools
        '
        Me.tsTools.BackColor = System.Drawing.Color.Transparent
        Me.tsTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsTools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnPlotSetup, Me.tssRight, Me.btnTool_LineProfile, Me.tslblTools, Me.tsbtnChannelSetup, Me.tssLeft1, Me.tslblInfobar})
        Me.tsTools.Location = New System.Drawing.Point(0, 0)
        Me.tsTools.Name = "tsTools"
        Me.tsTools.Size = New System.Drawing.Size(625, 25)
        Me.tsTools.TabIndex = 19
        Me.tsTools.Text = "tsTools"
        '
        'tsbtnPlotSetup
        '
        Me.tsbtnPlotSetup.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnPlotSetup.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.tsbtnPlotSetup.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnPlotSetup.Name = "tsbtnPlotSetup"
        Me.tsbtnPlotSetup.Size = New System.Drawing.Size(80, 22)
        Me.tsbtnPlotSetup.Text = "plot setup"
        '
        'tssRight
        '
        Me.tssRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tssRight.Name = "tssRight"
        Me.tssRight.Size = New System.Drawing.Size(6, 25)
        '
        'btnTool_LineProfile
        '
        Me.btnTool_LineProfile.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnTool_LineProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnTool_LineProfile.Image = Global.SpectroscopyManager.My.Resources.Resources.curve_16
        Me.btnTool_LineProfile.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnTool_LineProfile.Name = "btnTool_LineProfile"
        Me.btnTool_LineProfile.Size = New System.Drawing.Size(23, 22)
        Me.btnTool_LineProfile.Text = "measure a line profile"
        '
        'tslblTools
        '
        Me.tslblTools.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tslblTools.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslblTools.Name = "tslblTools"
        Me.tslblTools.Size = New System.Drawing.Size(37, 22)
        Me.tslblTools.Text = "tools:"
        '
        'tsbtnChannelSetup
        '
        Me.tsbtnChannelSetup.Image = Global.SpectroscopyManager.My.Resources.Resources.topography_16
        Me.tsbtnChannelSetup.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnChannelSetup.Name = "tsbtnChannelSetup"
        Me.tsbtnChannelSetup.Size = New System.Drawing.Size(69, 22)
        Me.tsbtnChannelSetup.Text = "channel"
        '
        'tssLeft1
        '
        Me.tssLeft1.Name = "tssLeft1"
        Me.tssLeft1.Size = New System.Drawing.Size(6, 25)
        '
        'tslblInfobar
        '
        Me.tslblInfobar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslblInfobar.Name = "tslblInfobar"
        Me.tslblInfobar.Size = New System.Drawing.Size(186, 22)
        Me.tslblInfobar.Text = "please select an image to display"
        '
        'dpLeft
        '
        Me.dpLeft.Controls.Add(Me.gbOpenExternal)
        Me.dpLeft.Controls.Add(Me.gbChannel)
        Me.dpLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.dpLeft.Location = New System.Drawing.Point(0, 25)
        Me.dpLeft.Name = "dpLeft"
        'TODO: Ausnahme "Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression." beim Generieren des Codes für "".
        Me.dpLeft.Size = New System.Drawing.Size(166, 503)
        Me.dpLeft.SlidePixelsPerTimerTick = 25
        Me.dpLeft.SuspendMessageFiltering = False
        Me.dpLeft.TabIndex = 15
        '
        'gbOpenExternal
        '
        Me.gbOpenExternal.Controls.Add(Me.btnOpenExternal)
        Me.gbOpenExternal.Controls.Add(Me.cboExternalViewer)
        Me.gbOpenExternal.Location = New System.Drawing.Point(3, 226)
        Me.gbOpenExternal.Name = "gbOpenExternal"
        Me.gbOpenExternal.Size = New System.Drawing.Size(157, 80)
        Me.gbOpenExternal.TabIndex = 15
        Me.gbOpenExternal.TabStop = False
        Me.gbOpenExternal.Text = "open file in external viewer:"
        '
        'btnOpenExternal
        '
        Me.btnOpenExternal.Location = New System.Drawing.Point(6, 46)
        Me.btnOpenExternal.Name = "btnOpenExternal"
        Me.btnOpenExternal.Size = New System.Drawing.Size(145, 23)
        Me.btnOpenExternal.TabIndex = 15
        Me.btnOpenExternal.Text = "open in external viewer"
        Me.btnOpenExternal.UseVisualStyleBackColor = True
        '
        'cboExternalViewer
        '
        Me.cboExternalViewer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboExternalViewer.FormattingEnabled = True
        Me.cboExternalViewer.Location = New System.Drawing.Point(6, 19)
        Me.cboExternalViewer.Name = "cboExternalViewer"
        Me.cboExternalViewer.Size = New System.Drawing.Size(145, 21)
        Me.cboExternalViewer.TabIndex = 15
        '
        'gbChannel
        '
        Me.gbChannel.Controls.Add(Me.cbChannel)
        Me.gbChannel.Controls.Add(Me.btnFilterSettings)
        Me.gbChannel.Controls.Add(Me.btnRemoveFilter)
        Me.gbChannel.Controls.Add(Me.btnAddFilter)
        Me.gbChannel.Controls.Add(Me.cbAddFilter)
        Me.gbChannel.Controls.Add(Me.lbFilters)
        Me.gbChannel.Controls.Add(Me.lblFilters)
        Me.gbChannel.Location = New System.Drawing.Point(3, 3)
        Me.gbChannel.Name = "gbChannel"
        Me.gbChannel.Size = New System.Drawing.Size(160, 219)
        Me.gbChannel.TabIndex = 11
        Me.gbChannel.TabStop = False
        Me.gbChannel.Text = "channel settings:"
        '
        'cbChannel
        '
        Me.cbChannel.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.cbChannel.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbChannel.Location = New System.Drawing.Point(6, 19)
        Me.cbChannel.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbChannel.Name = "cbChannel"
        Me.cbChannel.SelectedEntries = CType(resources.GetObject("cbChannel.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbChannel.SelectedEntry = ""
        Me.cbChannel.Size = New System.Drawing.Size(148, 27)
        Me.cbChannel.TabIndex = 1
        Me.cbChannel.TurnOnLastFilterSaving = False
        Me.cbChannel.TurnOnLastSelectionSaving = False
        '
        'btnFilterSettings
        '
        Me.btnFilterSettings.Enabled = False
        Me.btnFilterSettings.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.btnFilterSettings.Location = New System.Drawing.Point(131, 70)
        Me.btnFilterSettings.Name = "btnFilterSettings"
        Me.btnFilterSettings.Size = New System.Drawing.Size(25, 23)
        Me.btnFilterSettings.TabIndex = 4
        Me.btnFilterSettings.Text = "-"
        Me.btnFilterSettings.UseVisualStyleBackColor = True
        '
        'btnRemoveFilter
        '
        Me.btnRemoveFilter.Enabled = False
        Me.btnRemoveFilter.Image = Global.SpectroscopyManager.My.Resources.Resources.remove_16
        Me.btnRemoveFilter.Location = New System.Drawing.Point(132, 155)
        Me.btnRemoveFilter.Name = "btnRemoveFilter"
        Me.btnRemoveFilter.Size = New System.Drawing.Size(25, 23)
        Me.btnRemoveFilter.TabIndex = 5
        Me.btnRemoveFilter.Text = "-"
        Me.btnRemoveFilter.UseVisualStyleBackColor = True
        '
        'btnAddFilter
        '
        Me.btnAddFilter.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddFilter.Location = New System.Drawing.Point(131, 184)
        Me.btnAddFilter.Name = "btnAddFilter"
        Me.btnAddFilter.Size = New System.Drawing.Size(25, 23)
        Me.btnAddFilter.TabIndex = 7
        Me.btnAddFilter.Text = "+"
        Me.btnAddFilter.UseVisualStyleBackColor = True
        '
        'cbAddFilter
        '
        Me.cbAddFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAddFilter.FormattingEnabled = True
        Me.cbAddFilter.Location = New System.Drawing.Point(6, 185)
        Me.cbAddFilter.Name = "cbAddFilter"
        Me.cbAddFilter.Size = New System.Drawing.Size(120, 21)
        Me.cbAddFilter.TabIndex = 6
        '
        'lbFilters
        '
        Me.lbFilters.FormattingEnabled = True
        Me.lbFilters.Location = New System.Drawing.Point(6, 70)
        Me.lbFilters.Name = "lbFilters"
        Me.lbFilters.Size = New System.Drawing.Size(120, 108)
        Me.lbFilters.TabIndex = 3
        '
        'lblFilters
        '
        Me.lblFilters.AutoSize = True
        Me.lblFilters.Location = New System.Drawing.Point(3, 54)
        Me.lblFilters.Name = "lblFilters"
        Me.lblFilters.Size = New System.Drawing.Size(75, 13)
        Me.lblFilters.TabIndex = 14
        Me.lblFilters.Text = "channel filters:"
        '
        'dpRight
        '
        Me.dpRight.Controls.Add(Me.ckbScaleBarVisible)
        Me.dpRight.Controls.Add(Me.ckbUseHighQualityScaling)
        Me.dpRight.Controls.Add(Me.vsValueRangeSelector)
        Me.dpRight.Controls.Add(Me.cpColorPicker)
        Me.dpRight.Controls.Add(Me.lblColorCode)
        Me.dpRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.dpRight.Location = New System.Drawing.Point(455, 25)
        Me.dpRight.Name = "dpRight"
        'TODO: Ausnahme "Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression." beim Generieren des Codes für "".
        Me.dpRight.Size = New System.Drawing.Size(170, 503)
        Me.dpRight.SlidePixelsPerTimerTick = 25
        Me.dpRight.SuspendMessageFiltering = False
        Me.dpRight.TabIndex = 14
        '
        'ckbScaleBarVisible
        '
        Me.ckbScaleBarVisible.AutoSize = True
        Me.ckbScaleBarVisible.Checked = Global.SpectroscopyManager.My.MySettings.Default.ScanImageViewer_ShowScaleBar
        Me.ckbScaleBarVisible.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ckbScaleBarVisible.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.SpectroscopyManager.My.MySettings.Default, "ScanImageViewer_ShowScaleBar", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.ckbScaleBarVisible.Location = New System.Drawing.Point(6, 359)
        Me.ckbScaleBarVisible.Name = "ckbScaleBarVisible"
        Me.ckbScaleBarVisible.Size = New System.Drawing.Size(97, 17)
        Me.ckbScaleBarVisible.TabIndex = 10
        Me.ckbScaleBarVisible.Text = "show scale bar"
        Me.ckbScaleBarVisible.UseVisualStyleBackColor = True
        '
        'ckbUseHighQualityScaling
        '
        Me.ckbUseHighQualityScaling.AutoSize = True
        Me.ckbUseHighQualityScaling.Checked = Global.SpectroscopyManager.My.MySettings.Default.ScanImageViewer_HQPlot
        Me.ckbUseHighQualityScaling.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.SpectroscopyManager.My.MySettings.Default, "ScanImageViewer_HQPlot", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.ckbUseHighQualityScaling.Location = New System.Drawing.Point(6, 341)
        Me.ckbUseHighQualityScaling.Name = "ckbUseHighQualityScaling"
        Me.ckbUseHighQualityScaling.Size = New System.Drawing.Size(124, 17)
        Me.ckbUseHighQualityScaling.TabIndex = 9
        Me.ckbUseHighQualityScaling.Text = "high quality rescaling"
        Me.ckbUseHighQualityScaling.UseVisualStyleBackColor = True
        '
        'vsValueRangeSelector
        '
        Me.vsValueRangeSelector.Location = New System.Drawing.Point(2, 73)
        Me.vsValueRangeSelector.Name = "vsValueRangeSelector"
        Me.vsValueRangeSelector.SelectedMaxValue = 0R
        Me.vsValueRangeSelector.SelectedMinValue = 0R
        Me.vsValueRangeSelector.Size = New System.Drawing.Size(163, 264)
        Me.vsValueRangeSelector.TabIndex = 8
        '
        'cpColorPicker
        '
        Me.cpColorPicker.Location = New System.Drawing.Point(6, 22)
        Me.cpColorPicker.Name = "cpColorPicker"
        Me.cpColorPicker.Size = New System.Drawing.Size(148, 45)
        Me.cpColorPicker.TabIndex = 2
        '
        'lblColorCode
        '
        Me.lblColorCode.AutoSize = True
        Me.lblColorCode.Location = New System.Drawing.Point(6, 3)
        Me.lblColorCode.Name = "lblColorCode"
        Me.lblColorCode.Size = New System.Drawing.Size(73, 13)
        Me.lblColorCode.TabIndex = 14
        Me.lblColorCode.Text = "color scheme:"
        '
        'mScanImageViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dpLeft)
        Me.Controls.Add(Me.dpRight)
        Me.Controls.Add(Me.pbScanImage)
        Me.Controls.Add(Me.tsTools)
        Me.Name = "mScanImageViewer"
        Me.Size = New System.Drawing.Size(625, 528)
        CType(Me.pbScanImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmnuImageContextMenuStrip.ResumeLayout(False)
        Me.tsTools.ResumeLayout(False)
        Me.tsTools.PerformLayout()
        Me.dpLeft.ResumeLayout(False)
        Me.gbOpenExternal.ResumeLayout(False)
        Me.gbChannel.ResumeLayout(False)
        Me.gbChannel.PerformLayout()
        Me.dpRight.ResumeLayout(False)
        Me.dpRight.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbScanImage As System.Windows.Forms.PictureBox
    Friend WithEvents gbChannel As System.Windows.Forms.GroupBox
    Friend WithEvents vsValueRangeSelector As SpectroscopyManager.mValueRangeSelector
    Friend WithEvents cpColorPicker As SpectroscopyManager.ucColorPalettePicker
    Friend WithEvents cmnuImageContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuCopyImageToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSaveAsImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblColorCode As System.Windows.Forms.Label
    Friend WithEvents lblFilters As System.Windows.Forms.Label
    Friend WithEvents btnAddFilter As System.Windows.Forms.Button
    Friend WithEvents cbAddFilter As System.Windows.Forms.ComboBox
    Friend WithEvents lbFilters As System.Windows.Forms.ListBox
    Friend WithEvents btnRemoveFilter As System.Windows.Forms.Button
    Friend WithEvents btnFilterSettings As System.Windows.Forms.Button
    Friend WithEvents dpRight As DockablePanel
    Friend WithEvents dpLeft As DockablePanel
    Friend WithEvents cbChannel As ucScanChannelSelector
    Friend WithEvents ckbUseHighQualityScaling As CheckBox
    Friend WithEvents tsTools As ToolStrip
    Friend WithEvents btnTool_LineProfile As ToolStripButton
    Friend WithEvents tslblInfobar As ToolStripLabel
    Friend WithEvents tslblTools As ToolStripLabel
    Friend WithEvents tssLeft1 As ToolStripSeparator
    Friend WithEvents tsbtnChannelSetup As ToolStripButton
    Friend WithEvents tsbtnPlotSetup As ToolStripButton
    Friend WithEvents tssRight As ToolStripSeparator
    Friend WithEvents ckbScaleBarVisible As CheckBox
    Friend WithEvents cboExternalViewer As ComboBox
    Friend WithEvents gbOpenExternal As GroupBox
    Friend WithEvents btnOpenExternal As Button
    Friend WithEvents ttToolTip As ToolTip
End Class
