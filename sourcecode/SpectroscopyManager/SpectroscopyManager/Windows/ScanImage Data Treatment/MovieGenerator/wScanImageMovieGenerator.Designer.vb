<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class wScanImageMovieGenerator
    Inherits wFormBaseExpectsMultipleScanImagesOnLoad

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wScanImageMovieGenerator))
        Me.flpImages = New System.Windows.Forms.FlowLayoutPanel()
        Me.vrsColorScaling = New SpectroscopyManager.mValueRangeSelector()
        Me.gbGenerateGIF = New System.Windows.Forms.GroupBox()
        Me.lblAnimationTime = New System.Windows.Forms.Label()
        Me.txtAnimationTime = New SpectroscopyManager.NumericTextbox()
        Me.btnGenerateGIF = New System.Windows.Forms.Button()
        Me.gbChannel = New System.Windows.Forms.GroupBox()
        Me.scChannel = New SpectroscopyManager.ucScanChannelSelector()
        Me.gbOrdering = New System.Windows.Forms.GroupBox()
        Me.lbOrdering = New System.Windows.Forms.ListBox()
        Me.cbParameterDisplay = New System.Windows.Forms.ComboBox()
        Me.lblFileProperty = New System.Windows.Forms.Label()
        Me.cpColorPicker = New SpectroscopyManager.ucColorPalettePicker()
        Me.tbPlotPositionX = New System.Windows.Forms.TrackBar()
        Me.tbPlotPositionY = New System.Windows.Forms.TrackBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ckbShowScaleBar = New System.Windows.Forms.CheckBox()
        Me.gbGenerateGIF.SuspendLayout()
        Me.gbChannel.SuspendLayout()
        Me.gbOrdering.SuspendLayout()
        CType(Me.tbPlotPositionX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tbPlotPositionY, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'flpImages
        '
        Me.flpImages.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpImages.AutoScroll = True
        Me.flpImages.Location = New System.Drawing.Point(0, 4)
        Me.flpImages.Name = "flpImages"
        Me.flpImages.Size = New System.Drawing.Size(420, 712)
        Me.flpImages.TabIndex = 1
        '
        'vrsColorScaling
        '
        Me.vrsColorScaling.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vrsColorScaling.Location = New System.Drawing.Point(426, 187)
        Me.vrsColorScaling.Name = "vrsColorScaling"
        Me.vrsColorScaling.Size = New System.Drawing.Size(202, 189)
        Me.vrsColorScaling.TabIndex = 76
        '
        'gbGenerateGIF
        '
        Me.gbGenerateGIF.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbGenerateGIF.Controls.Add(Me.lblAnimationTime)
        Me.gbGenerateGIF.Controls.Add(Me.txtAnimationTime)
        Me.gbGenerateGIF.Controls.Add(Me.btnGenerateGIF)
        Me.gbGenerateGIF.Location = New System.Drawing.Point(426, 56)
        Me.gbGenerateGIF.Name = "gbGenerateGIF"
        Me.gbGenerateGIF.Size = New System.Drawing.Size(202, 74)
        Me.gbGenerateGIF.TabIndex = 78
        Me.gbGenerateGIF.TabStop = False
        Me.gbGenerateGIF.Text = "generate animated GIF:"
        '
        'lblAnimationTime
        '
        Me.lblAnimationTime.AutoSize = True
        Me.lblAnimationTime.Location = New System.Drawing.Point(11, 20)
        Me.lblAnimationTime.Name = "lblAnimationTime"
        Me.lblAnimationTime.Size = New System.Drawing.Size(74, 13)
        Me.lblAnimationTime.TabIndex = 10
        Me.lblAnimationTime.Text = "total time (ms):"
        '
        'txtAnimationTime
        '
        Me.txtAnimationTime.AllowZero = True
        Me.txtAnimationTime.BackColor = System.Drawing.Color.White
        Me.txtAnimationTime.ForeColor = System.Drawing.Color.Black
        Me.txtAnimationTime.FormatDecimalPlaces = 3
        Me.txtAnimationTime.Location = New System.Drawing.Point(94, 17)
        Me.txtAnimationTime.Name = "txtAnimationTime"
        Me.txtAnimationTime.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtAnimationTime.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtAnimationTime.Size = New System.Drawing.Size(96, 20)
        Me.txtAnimationTime.TabIndex = 90
        Me.txtAnimationTime.Text = "0.000000"
        Me.txtAnimationTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAnimationTime.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'btnGenerateGIF
        '
        Me.btnGenerateGIF.Image = Global.SpectroscopyManager.My.Resources.Resources.export_16
        Me.btnGenerateGIF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerateGIF.Location = New System.Drawing.Point(8, 43)
        Me.btnGenerateGIF.Name = "btnGenerateGIF"
        Me.btnGenerateGIF.Size = New System.Drawing.Size(186, 23)
        Me.btnGenerateGIF.TabIndex = 93
        Me.btnGenerateGIF.Text = "generate GIF"
        Me.btnGenerateGIF.UseVisualStyleBackColor = True
        '
        'gbChannel
        '
        Me.gbChannel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbChannel.Controls.Add(Me.scChannel)
        Me.gbChannel.Location = New System.Drawing.Point(426, 4)
        Me.gbChannel.Name = "gbChannel"
        Me.gbChannel.Size = New System.Drawing.Size(202, 46)
        Me.gbChannel.TabIndex = 79
        Me.gbChannel.TabStop = False
        Me.gbChannel.Text = "select channel"
        '
        'scChannel
        '
        Me.scChannel.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.scChannel.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.scChannel.Location = New System.Drawing.Point(4, 19)
        Me.scChannel.MinimumSize = New System.Drawing.Size(0, 21)
        Me.scChannel.Name = "scChannel"
        Me.scChannel.SelectedEntries = CType(resources.GetObject("scChannel.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.scChannel.SelectedEntry = ""
        Me.scChannel.Size = New System.Drawing.Size(190, 21)
        Me.scChannel.TabIndex = 0
        Me.scChannel.TurnOnLastFilterSaving = False
        Me.scChannel.TurnOnLastSelectionSaving = False
        '
        'gbOrdering
        '
        Me.gbOrdering.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOrdering.Controls.Add(Me.lbOrdering)
        Me.gbOrdering.Location = New System.Drawing.Point(426, 521)
        Me.gbOrdering.Name = "gbOrdering"
        Me.gbOrdering.Size = New System.Drawing.Size(202, 195)
        Me.gbOrdering.TabIndex = 80
        Me.gbOrdering.TabStop = False
        Me.gbOrdering.Text = "image order (move by drag and drop)"
        '
        'lbOrdering
        '
        Me.lbOrdering.AllowDrop = True
        Me.lbOrdering.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbOrdering.FormattingEnabled = True
        Me.lbOrdering.Location = New System.Drawing.Point(3, 16)
        Me.lbOrdering.Name = "lbOrdering"
        Me.lbOrdering.Size = New System.Drawing.Size(196, 176)
        Me.lbOrdering.TabIndex = 0
        '
        'cbParameterDisplay
        '
        Me.cbParameterDisplay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbParameterDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbParameterDisplay.FormattingEnabled = True
        Me.cbParameterDisplay.Location = New System.Drawing.Point(434, 395)
        Me.cbParameterDisplay.Name = "cbParameterDisplay"
        Me.cbParameterDisplay.Size = New System.Drawing.Size(186, 21)
        Me.cbParameterDisplay.TabIndex = 17
        '
        'lblFileProperty
        '
        Me.lblFileProperty.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFileProperty.AutoSize = True
        Me.lblFileProperty.Location = New System.Drawing.Point(431, 379)
        Me.lblFileProperty.Name = "lblFileProperty"
        Me.lblFileProperty.Size = New System.Drawing.Size(101, 13)
        Me.lblFileProperty.TabIndex = 81
        Me.lblFileProperty.Text = "overlay file property:"
        '
        'cpColorPicker
        '
        Me.cpColorPicker.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cpColorPicker.Location = New System.Drawing.Point(429, 136)
        Me.cpColorPicker.Name = "cpColorPicker"
        Me.cpColorPicker.Size = New System.Drawing.Size(198, 45)
        Me.cpColorPicker.TabIndex = 82
        Me.cpColorPicker.TabStop = False
        '
        'tbPlotPositionX
        '
        Me.tbPlotPositionX.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPlotPositionX.Location = New System.Drawing.Point(463, 422)
        Me.tbPlotPositionX.Maximum = 100
        Me.tbPlotPositionX.Name = "tbPlotPositionX"
        Me.tbPlotPositionX.Size = New System.Drawing.Size(162, 45)
        Me.tbPlotPositionX.TabIndex = 83
        Me.tbPlotPositionX.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'tbPlotPositionY
        '
        Me.tbPlotPositionY.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPlotPositionY.Location = New System.Drawing.Point(463, 464)
        Me.tbPlotPositionY.Maximum = 100
        Me.tbPlotPositionY.Name = "tbPlotPositionY"
        Me.tbPlotPositionY.Size = New System.Drawing.Size(162, 45)
        Me.tbPlotPositionY.TabIndex = 83
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(431, 435)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 81
        Me.Label1.Text = "X (%):"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(431, 470)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 81
        Me.Label2.Text = "Y (%):"
        '
        'ckbShowScaleBar
        '
        Me.ckbShowScaleBar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbShowScaleBar.AutoSize = True
        Me.ckbShowScaleBar.Location = New System.Drawing.Point(440, 498)
        Me.ckbShowScaleBar.Name = "ckbShowScaleBar"
        Me.ckbShowScaleBar.Size = New System.Drawing.Size(94, 17)
        Me.ckbShowScaleBar.TabIndex = 84
        Me.ckbShowScaleBar.Text = "show scalebar"
        Me.ckbShowScaleBar.UseVisualStyleBackColor = True
        '
        'wScanImageMovieGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 715)
        Me.Controls.Add(Me.ckbShowScaleBar)
        Me.Controls.Add(Me.tbPlotPositionY)
        Me.Controls.Add(Me.tbPlotPositionX)
        Me.Controls.Add(Me.cpColorPicker)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblFileProperty)
        Me.Controls.Add(Me.vrsColorScaling)
        Me.Controls.Add(Me.cbParameterDisplay)
        Me.Controls.Add(Me.gbOrdering)
        Me.Controls.Add(Me.gbChannel)
        Me.Controls.Add(Me.gbGenerateGIF)
        Me.Controls.Add(Me.flpImages)
        Me.MinimumSize = New System.Drawing.Size(300, 300)
        Me.Name = "wScanImageMovieGenerator"
        Me.Text = "create animated GIF"
        Me.Controls.SetChildIndex(Me.flpImages, 0)
        Me.Controls.SetChildIndex(Me.gbGenerateGIF, 0)
        Me.Controls.SetChildIndex(Me.gbChannel, 0)
        Me.Controls.SetChildIndex(Me.gbOrdering, 0)
        Me.Controls.SetChildIndex(Me.cbParameterDisplay, 0)
        Me.Controls.SetChildIndex(Me.vrsColorScaling, 0)
        Me.Controls.SetChildIndex(Me.lblFileProperty, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.cpColorPicker, 0)
        Me.Controls.SetChildIndex(Me.tbPlotPositionX, 0)
        Me.Controls.SetChildIndex(Me.tbPlotPositionY, 0)
        Me.Controls.SetChildIndex(Me.ckbShowScaleBar, 0)
        Me.gbGenerateGIF.ResumeLayout(False)
        Me.gbGenerateGIF.PerformLayout()
        Me.gbChannel.ResumeLayout(False)
        Me.gbOrdering.ResumeLayout(False)
        CType(Me.tbPlotPositionX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tbPlotPositionY, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents flpImages As FlowLayoutPanel
    Friend WithEvents vrsColorScaling As mValueRangeSelector
    Friend WithEvents gbGenerateGIF As GroupBox
    Friend WithEvents lblAnimationTime As Label
    Friend WithEvents txtAnimationTime As NumericTextbox
    Friend WithEvents btnGenerateGIF As Button
    Friend WithEvents gbChannel As GroupBox
    Friend WithEvents scChannel As ucScanChannelSelector
    Friend WithEvents gbOrdering As GroupBox
    Friend WithEvents lbOrdering As ListBox
    Friend WithEvents cbParameterDisplay As ComboBox
    Friend WithEvents lblFileProperty As Label
    Friend WithEvents cpColorPicker As ucColorPalettePicker
    Friend WithEvents tbPlotPositionX As TrackBar
    Friend WithEvents tbPlotPositionY As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ckbShowScaleBar As CheckBox
End Class
