<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mSpectroscopyTableViewer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mSpectroscopyTableViewer))
        Me.zPreview = New ZedGraph.ZedGraphControl()
        Me.tsTools = New System.Windows.Forms.ToolStrip()
        Me.tsbtnColumnSetup = New System.Windows.Forms.ToolStripButton()
        Me.tssRight = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtnTool_Zoom = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnMeasureVertical = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnMeasureHorizontal = New System.Windows.Forms.ToolStripButton()
        Me.tslblTools = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnPlotSetup = New System.Windows.Forms.ToolStripButton()
        Me.tssLeft1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tslblInfobar = New System.Windows.Forms.ToolStripLabel()
        Me.dpLeft = New SpectroscopyManager.DockablePanel()
        Me.lblStyleHeading = New System.Windows.Forms.Label()
        Me.lblStack = New System.Windows.Forms.Label()
        Me.txtStackValue = New SpectroscopyManager.NumericTextbox()
        Me.cbLineColor = New SpectroscopyManager.ColorPickerComboBox()
        Me.cbSymbolTypes = New SpectroscopyManager.ImageComboBox()
        Me.lbLines = New System.Windows.Forms.ListBox()
        Me.nudSymbolSize = New System.Windows.Forms.NumericUpDown()
        Me.nudLineWidth = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbLineStyle = New System.Windows.Forms.ComboBox()
        Me.dpRight = New SpectroscopyManager.DockablePanel()
        Me.spSettings = New System.Windows.Forms.SplitContainer()
        Me.cbX = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.ckbLogX = New System.Windows.Forms.CheckBox()
        Me.lblXAxis = New System.Windows.Forms.Label()
        Me.ckbLogY = New System.Windows.Forms.CheckBox()
        Me.lblYAxis = New System.Windows.Forms.Label()
        Me.cbY = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.tsTools.SuspendLayout()
        Me.dpLeft.SuspendLayout()
        CType(Me.nudSymbolSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLineWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.dpRight.SuspendLayout()
        CType(Me.spSettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.spSettings.Panel1.SuspendLayout()
        Me.spSettings.Panel2.SuspendLayout()
        Me.spSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'zPreview
        '
        Me.zPreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.zPreview.IsShowCopyMessage = False
        Me.zPreview.Location = New System.Drawing.Point(0, 25)
        Me.zPreview.Name = "zPreview"
        Me.zPreview.ScrollGrace = 0R
        Me.zPreview.ScrollMaxX = 0R
        Me.zPreview.ScrollMaxY = 0R
        Me.zPreview.ScrollMaxY2 = 0R
        Me.zPreview.ScrollMinX = 0R
        Me.zPreview.ScrollMinY = 0R
        Me.zPreview.ScrollMinY2 = 0R
        Me.zPreview.Size = New System.Drawing.Size(539, 331)
        Me.zPreview.TabIndex = 6
        '
        'tsTools
        '
        Me.tsTools.BackColor = System.Drawing.Color.Transparent
        Me.tsTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsTools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnColumnSetup, Me.tssRight, Me.tsbtnTool_Zoom, Me.tsbtnMeasureVertical, Me.tsbtnMeasureHorizontal, Me.tslblTools, Me.tsbtnPlotSetup, Me.tssLeft1, Me.tslblInfobar})
        Me.tsTools.Location = New System.Drawing.Point(0, 0)
        Me.tsTools.Name = "tsTools"
        Me.tsTools.Size = New System.Drawing.Size(539, 25)
        Me.tsTools.TabIndex = 20
        Me.tsTools.Text = "tsTools"
        '
        'tsbtnColumnSetup
        '
        Me.tsbtnColumnSetup.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnColumnSetup.Image = Global.SpectroscopyManager.My.Resources.Resources.plot_16
        Me.tsbtnColumnSetup.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnColumnSetup.Name = "tsbtnColumnSetup"
        Me.tsbtnColumnSetup.Size = New System.Drawing.Size(99, 22)
        Me.tsbtnColumnSetup.Text = "data columns"
        '
        'tssRight
        '
        Me.tssRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tssRight.Name = "tssRight"
        Me.tssRight.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtnTool_Zoom
        '
        Me.tsbtnTool_Zoom.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnTool_Zoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnTool_Zoom.Image = Global.SpectroscopyManager.My.Resources.Resources.zoom_out_16
        Me.tsbtnTool_Zoom.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnTool_Zoom.Name = "tsbtnTool_Zoom"
        Me.tsbtnTool_Zoom.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnTool_Zoom.Text = "reset to the default scale"
        '
        'tsbtnMeasureVertical
        '
        Me.tsbtnMeasureVertical.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnMeasureVertical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnMeasureVertical.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_vert_16
        Me.tsbtnMeasureVertical.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnMeasureVertical.Name = "tsbtnMeasureVertical"
        Me.tsbtnMeasureVertical.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnMeasureVertical.Text = "measure vertical distance"
        Me.tsbtnMeasureVertical.ToolTipText = "measure vertical distance"
        '
        'tsbtnMeasureHorizontal
        '
        Me.tsbtnMeasureHorizontal.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnMeasureHorizontal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnMeasureHorizontal.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_16
        Me.tsbtnMeasureHorizontal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnMeasureHorizontal.Name = "tsbtnMeasureHorizontal"
        Me.tsbtnMeasureHorizontal.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnMeasureHorizontal.Text = "measure horizontal distance"
        Me.tsbtnMeasureHorizontal.ToolTipText = "measure horizontal distance"
        '
        'tslblTools
        '
        Me.tslblTools.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tslblTools.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslblTools.Name = "tslblTools"
        Me.tslblTools.Size = New System.Drawing.Size(37, 22)
        Me.tslblTools.Text = "tools:"
        '
        'tsbtnPlotSetup
        '
        Me.tsbtnPlotSetup.Image = Global.SpectroscopyManager.My.Resources.Resources.settings_16
        Me.tsbtnPlotSetup.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnPlotSetup.Name = "tsbtnPlotSetup"
        Me.tsbtnPlotSetup.Size = New System.Drawing.Size(80, 22)
        Me.tsbtnPlotSetup.Text = "plot setup"
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
        Me.tslblInfobar.Size = New System.Drawing.Size(205, 22)
        Me.tslblInfobar.Text = "please select an spectrum to display"
        '
        'dpLeft
        '
        Me.dpLeft.Controls.Add(Me.lblStyleHeading)
        Me.dpLeft.Controls.Add(Me.lblStack)
        Me.dpLeft.Controls.Add(Me.txtStackValue)
        Me.dpLeft.Controls.Add(Me.cbLineColor)
        Me.dpLeft.Controls.Add(Me.cbSymbolTypes)
        Me.dpLeft.Controls.Add(Me.lbLines)
        Me.dpLeft.Controls.Add(Me.nudSymbolSize)
        Me.dpLeft.Controls.Add(Me.nudLineWidth)
        Me.dpLeft.Controls.Add(Me.Label6)
        Me.dpLeft.Controls.Add(Me.Label4)
        Me.dpLeft.Controls.Add(Me.Label5)
        Me.dpLeft.Controls.Add(Me.Label3)
        Me.dpLeft.Controls.Add(Me.cbLineStyle)
        Me.dpLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.dpLeft.Location = New System.Drawing.Point(0, 25)
        Me.dpLeft.Name = "dpLeft"
        'TODO: Ausnahme "Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression." beim Generieren des Codes für "".
        Me.dpLeft.Size = New System.Drawing.Size(134, 331)
        Me.dpLeft.SlidePixelsPerTimerTick = 25
        Me.dpLeft.SuspendMessageFiltering = False
        Me.dpLeft.TabIndex = 14
        '
        'lblStyleHeading
        '
        Me.lblStyleHeading.AutoSize = True
        Me.lblStyleHeading.Location = New System.Drawing.Point(3, 53)
        Me.lblStyleHeading.Name = "lblStyleHeading"
        Me.lblStyleHeading.Size = New System.Drawing.Size(71, 13)
        Me.lblStyleHeading.TabIndex = 7
        Me.lblStyleHeading.Text = "Change style:"
        '
        'lblStack
        '
        Me.lblStack.AutoSize = True
        Me.lblStack.Location = New System.Drawing.Point(2, 5)
        Me.lblStack.Name = "lblStack"
        Me.lblStack.Size = New System.Drawing.Size(73, 13)
        Me.lblStack.TabIndex = 7
        Me.lblStack.Text = "Stack curves:"
        '
        'txtStackValue
        '
        Me.txtStackValue.AllowZero = True
        Me.txtStackValue.BackColor = System.Drawing.Color.White
        Me.txtStackValue.ForeColor = System.Drawing.Color.Black
        Me.txtStackValue.FormatDecimalPlaces = 6
        Me.txtStackValue.Location = New System.Drawing.Point(15, 22)
        Me.txtStackValue.Name = "txtStackValue"
        Me.txtStackValue.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtStackValue.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtStackValue.Size = New System.Drawing.Size(98, 20)
        Me.txtStackValue.TabIndex = 6
        Me.txtStackValue.Text = "0.000000"
        Me.txtStackValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtStackValue.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'cbLineColor
        '
        Me.cbLineColor.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbLineColor.Color = System.Drawing.Color.Black
        Me.cbLineColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbLineColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLineColor.DropDownWidth = 151
        Me.cbLineColor.FormattingEnabled = True
        Me.cbLineColor.Items.AddRange(New Object() {System.Drawing.Color.Magenta, System.Drawing.Color.Black, System.Drawing.Color.Aqua, System.Drawing.Color.Aquamarine, System.Drawing.Color.Blue, System.Drawing.Color.BlueViolet, System.Drawing.Color.Brown, System.Drawing.Color.BurlyWood, System.Drawing.Color.CadetBlue, System.Drawing.Color.Chartreuse, System.Drawing.Color.Chocolate, System.Drawing.Color.Coral, System.Drawing.Color.CornflowerBlue, System.Drawing.Color.Crimson, System.Drawing.Color.Cyan, System.Drawing.Color.DarkBlue, System.Drawing.Color.DarkCyan, System.Drawing.Color.DarkGoldenrod, System.Drawing.Color.DarkGray, System.Drawing.Color.DarkGreen, System.Drawing.Color.DarkKhaki, System.Drawing.Color.DarkMagenta, System.Drawing.Color.DarkOliveGreen, System.Drawing.Color.DarkOrange, System.Drawing.Color.DarkOrchid, System.Drawing.Color.DarkRed, System.Drawing.Color.DarkSalmon, System.Drawing.Color.DarkSeaGreen, System.Drawing.Color.DarkSlateBlue, System.Drawing.Color.DarkSlateGray, System.Drawing.Color.DarkTurquoise, System.Drawing.Color.DarkViolet, System.Drawing.Color.DeepPink, System.Drawing.Color.DeepSkyBlue, System.Drawing.Color.DimGray, System.Drawing.Color.DodgerBlue, System.Drawing.Color.Firebrick, System.Drawing.Color.ForestGreen, System.Drawing.Color.Fuchsia, System.Drawing.Color.Gainsboro, System.Drawing.Color.Gold, System.Drawing.Color.Goldenrod, System.Drawing.Color.Gray, System.Drawing.Color.Green, System.Drawing.Color.GreenYellow, System.Drawing.Color.HotPink, System.Drawing.Color.IndianRed, System.Drawing.Color.Indigo, System.Drawing.Color.Khaki, System.Drawing.Color.LawnGreen, System.Drawing.Color.LightBlue, System.Drawing.Color.LightCoral, System.Drawing.Color.LightGreen, System.Drawing.Color.LightGray, System.Drawing.Color.LightPink, System.Drawing.Color.LightSalmon, System.Drawing.Color.LightSeaGreen, System.Drawing.Color.LightSkyBlue, System.Drawing.Color.LightSlateGray, System.Drawing.Color.LightSteelBlue, System.Drawing.Color.Lime, System.Drawing.Color.LimeGreen, System.Drawing.Color.Maroon, System.Drawing.Color.MediumAquamarine, System.Drawing.Color.MediumBlue, System.Drawing.Color.MediumOrchid, System.Drawing.Color.MediumPurple, System.Drawing.Color.MediumSeaGreen, System.Drawing.Color.MediumSlateBlue, System.Drawing.Color.MediumSpringGreen, System.Drawing.Color.MediumTurquoise, System.Drawing.Color.MediumVioletRed, System.Drawing.Color.MidnightBlue, System.Drawing.Color.NavajoWhite, System.Drawing.Color.Navy, System.Drawing.Color.Olive, System.Drawing.Color.OliveDrab, System.Drawing.Color.Orange, System.Drawing.Color.OrangeRed, System.Drawing.Color.Orchid, System.Drawing.Color.PaleGreen, System.Drawing.Color.PaleTurquoise, System.Drawing.Color.PaleVioletRed, System.Drawing.Color.PeachPuff, System.Drawing.Color.Peru, System.Drawing.Color.Pink, System.Drawing.Color.Plum, System.Drawing.Color.PowderBlue, System.Drawing.Color.Purple, System.Drawing.Color.Red, System.Drawing.Color.RosyBrown, System.Drawing.Color.RoyalBlue, System.Drawing.Color.SaddleBrown, System.Drawing.Color.Salmon, System.Drawing.Color.SandyBrown, System.Drawing.Color.SeaGreen, System.Drawing.Color.Sienna, System.Drawing.Color.Silver, System.Drawing.Color.SkyBlue, System.Drawing.Color.SlateBlue, System.Drawing.Color.SlateGray, System.Drawing.Color.SpringGreen, System.Drawing.Color.SteelBlue, System.Drawing.Color.Tan, System.Drawing.Color.Teal, System.Drawing.Color.Thistle, System.Drawing.Color.Tomato, System.Drawing.Color.Turquoise, System.Drawing.Color.Violet, System.Drawing.Color.Wheat, System.Drawing.Color.Yellow, System.Drawing.Color.YellowGreen})
        Me.cbLineColor.Location = New System.Drawing.Point(6, 240)
        Me.cbLineColor.Name = "cbLineColor"
        Me.cbLineColor.ShowBrightColors = False
        Me.cbLineColor.Size = New System.Drawing.Size(121, 21)
        Me.cbLineColor.TabIndex = 5
        '
        'cbSymbolTypes
        '
        Me.cbSymbolTypes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbSymbolTypes.FormattingEnabled = True
        Me.cbSymbolTypes.Location = New System.Drawing.Point(6, 281)
        Me.cbSymbolTypes.Name = "cbSymbolTypes"
        Me.cbSymbolTypes.SelectedItem = Nothing
        Me.cbSymbolTypes.Size = New System.Drawing.Size(121, 21)
        Me.cbSymbolTypes.TabIndex = 4
        '
        'lbLines
        '
        Me.lbLines.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbLines.FormattingEnabled = True
        Me.lbLines.HorizontalScrollbar = True
        Me.lbLines.Location = New System.Drawing.Point(3, 69)
        Me.lbLines.Name = "lbLines"
        Me.lbLines.Size = New System.Drawing.Size(128, 95)
        Me.lbLines.TabIndex = 3
        '
        'nudSymbolSize
        '
        Me.nudSymbolSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudSymbolSize.Location = New System.Drawing.Point(70, 305)
        Me.nudSymbolSize.Name = "nudSymbolSize"
        Me.nudSymbolSize.Size = New System.Drawing.Size(43, 20)
        Me.nudSymbolSize.TabIndex = 2
        '
        'nudLineWidth
        '
        Me.nudLineWidth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudLineWidth.Location = New System.Drawing.Point(70, 214)
        Me.nudLineWidth.Name = "nudLineWidth"
        Me.nudLineWidth.Size = New System.Drawing.Size(43, 20)
        Me.nudLineWidth.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 307)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Symbol-Size:"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 216)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Line-Width:"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 265)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(44, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Symbol:"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 174)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Line-Style:"
        '
        'cbLineStyle
        '
        Me.cbLineStyle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbLineStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLineStyle.FormattingEnabled = True
        Me.cbLineStyle.Location = New System.Drawing.Point(6, 190)
        Me.cbLineStyle.Name = "cbLineStyle"
        Me.cbLineStyle.Size = New System.Drawing.Size(121, 21)
        Me.cbLineStyle.TabIndex = 0
        '
        'dpRight
        '
        Me.dpRight.Controls.Add(Me.spSettings)
        Me.dpRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.dpRight.Location = New System.Drawing.Point(402, 25)
        Me.dpRight.Name = "dpRight"
        'TODO: Ausnahme "Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression." beim Generieren des Codes für "".
        Me.dpRight.Size = New System.Drawing.Size(137, 331)
        Me.dpRight.SlidePixelsPerTimerTick = 25
        Me.dpRight.SuspendMessageFiltering = False
        Me.dpRight.TabIndex = 12
        '
        'spSettings
        '
        Me.spSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.spSettings.Location = New System.Drawing.Point(0, 0)
        Me.spSettings.Name = "spSettings"
        Me.spSettings.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'spSettings.Panel1
        '
        Me.spSettings.Panel1.Controls.Add(Me.cbX)
        Me.spSettings.Panel1.Controls.Add(Me.ckbLogX)
        Me.spSettings.Panel1.Controls.Add(Me.lblXAxis)
        '
        'spSettings.Panel2
        '
        Me.spSettings.Panel2.Controls.Add(Me.ckbLogY)
        Me.spSettings.Panel2.Controls.Add(Me.lblYAxis)
        Me.spSettings.Panel2.Controls.Add(Me.cbY)
        Me.spSettings.Size = New System.Drawing.Size(137, 331)
        Me.spSettings.SplitterDistance = 165
        Me.spSettings.TabIndex = 11
        '
        'cbX
        '
        Me.cbX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbX.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Listbox
        Me.cbX.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.cbX.Location = New System.Drawing.Point(6, 22)
        Me.cbX.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbX.Name = "cbX"
        Me.cbX.SelectedColumnName = ""
        Me.cbX.SelectedColumnNames = CType(resources.GetObject("cbX.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbX.SelectedEntries = CType(resources.GetObject("cbX.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbX.SelectedEntry = ""
        Me.cbX.Size = New System.Drawing.Size(128, 140)
        Me.cbX.TabIndex = 6
        Me.cbX.TurnOnLastFilterSaving = False
        Me.cbX.TurnOnLastSelectionSaving = False
        '
        'ckbLogX
        '
        Me.ckbLogX.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbLogX.AutoSize = True
        Me.ckbLogX.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ckbLogX.Location = New System.Drawing.Point(96, 4)
        Me.ckbLogX.Name = "ckbLogX"
        Me.ckbLogX.Size = New System.Drawing.Size(38, 17)
        Me.ckbLogX.TabIndex = 7
        Me.ckbLogX.Text = "log"
        Me.ckbLogX.UseVisualStyleBackColor = True
        '
        'lblXAxis
        '
        Me.lblXAxis.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblXAxis.AutoSize = True
        Me.lblXAxis.Location = New System.Drawing.Point(8, 6)
        Me.lblXAxis.Name = "lblXAxis"
        Me.lblXAxis.Size = New System.Drawing.Size(36, 13)
        Me.lblXAxis.TabIndex = 8
        Me.lblXAxis.Text = "x axis:"
        '
        'ckbLogY
        '
        Me.ckbLogY.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbLogY.AutoSize = True
        Me.ckbLogY.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ckbLogY.Location = New System.Drawing.Point(96, 3)
        Me.ckbLogY.Name = "ckbLogY"
        Me.ckbLogY.Size = New System.Drawing.Size(38, 17)
        Me.ckbLogY.TabIndex = 9
        Me.ckbLogY.Text = "log"
        Me.ckbLogY.UseVisualStyleBackColor = True
        '
        'lblYAxis
        '
        Me.lblYAxis.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblYAxis.AutoSize = True
        Me.lblYAxis.Location = New System.Drawing.Point(8, 5)
        Me.lblYAxis.Name = "lblYAxis"
        Me.lblYAxis.Size = New System.Drawing.Size(36, 13)
        Me.lblYAxis.TabIndex = 10
        Me.lblYAxis.Text = "y axis:"
        '
        'cbY
        '
        Me.cbY.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbY.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Listbox
        Me.cbY.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.cbY.Location = New System.Drawing.Point(6, 21)
        Me.cbY.MinimumSize = New System.Drawing.Size(0, 21)
        Me.cbY.Name = "cbY"
        Me.cbY.SelectedColumnName = ""
        Me.cbY.SelectedColumnNames = CType(resources.GetObject("cbY.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.cbY.SelectedEntries = CType(resources.GetObject("cbY.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.cbY.SelectedEntry = ""
        Me.cbY.Size = New System.Drawing.Size(128, 136)
        Me.cbY.TabIndex = 6
        Me.cbY.TurnOnLastFilterSaving = False
        Me.cbY.TurnOnLastSelectionSaving = False
        '
        'mSpectroscopyTableViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dpLeft)
        Me.Controls.Add(Me.dpRight)
        Me.Controls.Add(Me.tsTools)
        Me.Controls.Add(Me.zPreview)
        Me.Name = "mSpectroscopyTableViewer"
        Me.Size = New System.Drawing.Size(539, 356)
        Me.tsTools.ResumeLayout(False)
        Me.tsTools.PerformLayout()
        Me.dpLeft.ResumeLayout(False)
        Me.dpLeft.PerformLayout()
        CType(Me.nudSymbolSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLineWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.dpRight.ResumeLayout(False)
        Me.spSettings.Panel1.ResumeLayout(False)
        Me.spSettings.Panel1.PerformLayout()
        Me.spSettings.Panel2.ResumeLayout(False)
        Me.spSettings.Panel2.PerformLayout()
        CType(Me.spSettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.spSettings.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents zPreview As ZedGraph.ZedGraphControl
    Friend WithEvents cbX As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents cbY As SpectroscopyManager.ucSpectroscopyColumnSelector
    Friend WithEvents ckbLogX As System.Windows.Forms.CheckBox
    Friend WithEvents lblXAxis As System.Windows.Forms.Label
    Friend WithEvents ckbLogY As System.Windows.Forms.CheckBox
    Friend WithEvents lblYAxis As System.Windows.Forms.Label
    Friend WithEvents dpRight As SpectroscopyManager.DockablePanel
    Friend WithEvents spSettings As System.Windows.Forms.SplitContainer
    Friend WithEvents dpLeft As SpectroscopyManager.DockablePanel
    Friend WithEvents nudSymbolSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLineWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbLineStyle As System.Windows.Forms.ComboBox
    Friend WithEvents lbLines As System.Windows.Forms.ListBox
    Friend WithEvents cbSymbolTypes As SpectroscopyManager.ImageComboBox
    Friend WithEvents cbLineColor As SpectroscopyManager.ColorPickerComboBox
    Friend WithEvents lblStyleHeading As Label
    Friend WithEvents lblStack As Label
    Friend WithEvents txtStackValue As NumericTextbox
    Friend WithEvents tsTools As ToolStrip
    Friend WithEvents tsbtnColumnSetup As ToolStripButton
    Friend WithEvents tssRight As ToolStripSeparator
    Friend WithEvents tsbtnTool_Zoom As ToolStripButton
    Friend WithEvents tslblTools As ToolStripLabel
    Friend WithEvents tsbtnPlotSetup As ToolStripButton
    Friend WithEvents tssLeft1 As ToolStripSeparator
    Friend WithEvents tslblInfobar As ToolStripLabel
    Friend WithEvents tsbtnMeasureVertical As ToolStripButton
    Friend WithEvents tsbtnMeasureHorizontal As ToolStripButton
End Class
