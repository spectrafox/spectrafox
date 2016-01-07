<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class wDataCropping
    Inherits SpectroscopyManager.wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataCropping))
        Me.btnCrop = New System.Windows.Forms.Button()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.txtMaxIndexIncl = New SpectroscopyManager.NumericTextbox()
        Me.txtMinIndexIncl = New SpectroscopyManager.NumericTextbox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.gbPreview = New System.Windows.Forms.GroupBox()
        Me.pbBefore = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.GroupBox3.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPreview.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCrop
        '
        Me.btnCrop.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCrop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCrop.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnCrop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCrop.Location = New System.Drawing.Point(733, 148)
        Me.btnCrop.Name = "btnCrop"
        Me.btnCrop.Size = New System.Drawing.Size(287, 48)
        Me.btnCrop.TabIndex = 9
        Me.btnCrop.Text = "Crop"
        Me.btnCrop.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(733, 443)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(287, 26)
        Me.btnCloseWindow.TabIndex = 11
        Me.btnCloseWindow.Text = "Close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.PictureBox1)
        Me.GroupBox3.Controls.Add(Me.txtMaxIndexIncl)
        Me.GroupBox3.Controls.Add(Me.txtMinIndexIncl)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Location = New System.Drawing.Point(733, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(287, 136)
        Me.GroupBox3.TabIndex = 15
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Select Area to take as Reference:"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_25
        Me.PictureBox1.Location = New System.Drawing.Point(13, 90)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(28, 28)
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'txtMaxIndexIncl
        '
        Me.txtMaxIndexIncl.AllowZero = True
        Me.txtMaxIndexIncl.BackColor = System.Drawing.Color.White
        Me.txtMaxIndexIncl.ForeColor = System.Drawing.Color.Black
        Me.txtMaxIndexIncl.FormatDecimalPlaces = 0
        Me.txtMaxIndexIncl.Location = New System.Drawing.Point(117, 48)
        Me.txtMaxIndexIncl.Name = "txtMaxIndexIncl"
        Me.txtMaxIndexIncl.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.PlainNumber
        Me.txtMaxIndexIncl.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtMaxIndexIncl.Size = New System.Drawing.Size(130, 20)
        Me.txtMaxIndexIncl.TabIndex = 6
        Me.txtMaxIndexIncl.Text = "0.000000"
        Me.txtMaxIndexIncl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMaxIndexIncl.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.IntegerValue
        '
        'txtMinIndexIncl
        '
        Me.txtMinIndexIncl.AllowZero = True
        Me.txtMinIndexIncl.BackColor = System.Drawing.Color.White
        Me.txtMinIndexIncl.ForeColor = System.Drawing.Color.Black
        Me.txtMinIndexIncl.FormatDecimalPlaces = 0
        Me.txtMinIndexIncl.Location = New System.Drawing.Point(117, 18)
        Me.txtMinIndexIncl.Name = "txtMinIndexIncl"
        Me.txtMinIndexIncl.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.PlainNumber
        Me.txtMinIndexIncl.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtMinIndexIncl.Size = New System.Drawing.Size(130, 20)
        Me.txtMinIndexIncl.TabIndex = 5
        Me.txtMinIndexIncl.Text = "0.000000"
        Me.txtMinIndexIncl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtMinIndexIncl.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.IntegerValue
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(48, 82)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(222, 39)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Note: Crop-range selection is possible" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "by using the mouse to draw a frame in" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "th" &
    "e upper preview graph on the right."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 51)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(93, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Crop above index:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Crop below index:"
        '
        'btnReset
        '
        Me.btnReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReset.Image = Global.SpectroscopyManager.My.Resources.Resources.reload_25
        Me.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReset.Location = New System.Drawing.Point(733, 202)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(150, 40)
        Me.btnReset.TabIndex = 9
        Me.btnReset.Text = "reset crop range"
        Me.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'gbPreview
        '
        Me.gbPreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPreview.Controls.Add(Me.pbBefore)
        Me.gbPreview.Location = New System.Drawing.Point(0, 3)
        Me.gbPreview.Name = "gbPreview"
        Me.gbPreview.Size = New System.Drawing.Size(718, 469)
        Me.gbPreview.TabIndex = 12
        Me.gbPreview.TabStop = False
        Me.gbPreview.Text = "select data:"
        '
        'pbBefore
        '
        Me.pbBefore.AllowAdjustingXColumn = True
        Me.pbBefore.AllowAdjustingYColumn = True
        Me.pbBefore.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbBefore.CallbackDataPointSelected = Nothing
        Me.pbBefore.CallbackXRangeSelected = Nothing
        Me.pbBefore.CallbackXValueSelected = Nothing
        Me.pbBefore.CallbackXYRangeSelected = Nothing
        Me.pbBefore.CallbackXYValueSelected = Nothing
        Me.pbBefore.CallbackYRangeSelected = Nothing
        Me.pbBefore.CallbackYValueSelected = Nothing
        Me.pbBefore.ClearPointSelectionModeAfterSelection = False
        Me.pbBefore.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbBefore.Location = New System.Drawing.Point(3, 16)
        Me.pbBefore.MultipleSpectraStackOffset = 0R
        Me.pbBefore.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.pbBefore.Name = "pbBefore"
        Me.pbBefore.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.XRange
        Me.pbBefore.ShowColumnSelectors = True
        Me.pbBefore.Size = New System.Drawing.Size(712, 450)
        Me.pbBefore.TabIndex = 6
        Me.pbBefore.TurnOnLastFilterSaving_Y = True
        Me.pbBefore.TurnOnLastSelectionSaving_Y = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Enabled = False
        Me.btnSave.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(889, 202)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(131, 40)
        Me.btnSave.TabIndex = 16
        Me.btnSave.Text = "save crop range"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'wDataCropping
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1032, 475)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.gbPreview)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.btnCrop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataCropping"
        Me.Text = "Data Cropping - "
        Me.Controls.SetChildIndex(Me.btnCrop, 0)
        Me.Controls.SetChildIndex(Me.btnReset, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.GroupBox3, 0)
        Me.Controls.SetChildIndex(Me.gbPreview, 0)
        Me.Controls.SetChildIndex(Me.btnSave, 0)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPreview.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCrop As System.Windows.Forms.Button
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents gbPreview As System.Windows.Forms.GroupBox
    Friend WithEvents pbBefore As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents txtMaxIndexIncl As SpectroscopyManager.NumericTextbox
    Friend WithEvents txtMinIndexIncl As SpectroscopyManager.NumericTextbox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnSave As Button
End Class
