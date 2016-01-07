<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wShiftColumnValues
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wShiftColumnValues))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.dgvSpectroscopyFiles = New System.Windows.Forms.DataGridView()
        Me.colShow = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colX = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colY = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colShiftX = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colShiftY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colColumnNameX = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSaveX = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colColumnNameY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSaveY = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.zPreview = New ZedGraph.ZedGraphControl()
        Me.HorSplit = New System.Windows.Forms.SplitContainer()
        Me.TopVertSplit = New System.Windows.Forms.SplitContainer()
        Me.btnResetShiftsX = New System.Windows.Forms.Button()
        Me.btnResetShiftsY = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnShowAll = New System.Windows.Forms.Button()
        Me.btnHideAll = New System.Windows.Forms.Button()
        Me.ckbLogY = New System.Windows.Forms.CheckBox()
        Me.ckbLogX = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnSaveYColumns = New System.Windows.Forms.Button()
        Me.btnSaveXColumns = New System.Windows.Forms.Button()
        Me.btnSaveAllColumns = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtShiftAllXLinear = New System.Windows.Forms.TextBox()
        Me.btnShiftAllXLinear = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtShiftAllYLinear = New System.Windows.Forms.TextBox()
        Me.btnShiftAllYLinear = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvSpectroscopyFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HorSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HorSplit.Panel1.SuspendLayout()
        Me.HorSplit.Panel2.SuspendLayout()
        Me.HorSplit.SuspendLayout()
        CType(Me.TopVertSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TopVertSplit.Panel1.SuspendLayout()
        Me.TopVertSplit.Panel2.SuspendLayout()
        Me.TopVertSplit.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dgvSpectroscopyFiles)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1120, 362)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Spectroscopy Files:"
        '
        'dgvSpectroscopyFiles
        '
        Me.dgvSpectroscopyFiles.AllowUserToAddRows = False
        Me.dgvSpectroscopyFiles.AllowUserToDeleteRows = False
        Me.dgvSpectroscopyFiles.AllowUserToResizeRows = False
        Me.dgvSpectroscopyFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSpectroscopyFiles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colShow, Me.colName, Me.colX, Me.colY, Me.colShiftX, Me.colShiftY, Me.colColumnNameX, Me.colSaveX, Me.colColumnNameY, Me.colSaveY})
        Me.dgvSpectroscopyFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSpectroscopyFiles.Location = New System.Drawing.Point(3, 16)
        Me.dgvSpectroscopyFiles.MultiSelect = False
        Me.dgvSpectroscopyFiles.Name = "dgvSpectroscopyFiles"
        Me.dgvSpectroscopyFiles.RowHeadersVisible = False
        Me.dgvSpectroscopyFiles.Size = New System.Drawing.Size(1114, 343)
        Me.dgvSpectroscopyFiles.TabIndex = 0
        '
        'colShow
        '
        Me.colShow.HeaderText = ""
        Me.colShow.Name = "colShow"
        Me.colShow.Width = 30
        '
        'colName
        '
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colName.Width = 120
        '
        'colX
        '
        Me.colX.HeaderText = "X-Column"
        Me.colX.Name = "colX"
        Me.colX.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colX.Width = 130
        '
        'colY
        '
        Me.colY.HeaderText = "Y-Column"
        Me.colY.Name = "colY"
        Me.colY.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colY.Width = 130
        '
        'colShiftX
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle1.Format = "E2"
        DataGridViewCellStyle1.NullValue = Nothing
        Me.colShiftX.DefaultCellStyle = DataGridViewCellStyle1
        Me.colShiftX.HeaderText = "Shift-Value X"
        Me.colShiftX.Name = "colShiftX"
        Me.colShiftX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colShiftX.Width = 120
        '
        'colShiftY
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle2.Format = "E2"
        Me.colShiftY.DefaultCellStyle = DataGridViewCellStyle2
        Me.colShiftY.HeaderText = "Shift-Value Y"
        Me.colShiftY.Name = "colShiftY"
        Me.colShiftY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colShiftY.Width = 120
        '
        'colColumnNameX
        '
        Me.colColumnNameX.HeaderText = "New Name X"
        Me.colColumnNameX.Name = "colColumnNameX"
        Me.colColumnNameX.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colSaveX
        '
        Me.colSaveX.HeaderText = "Save X"
        Me.colSaveX.Name = "colSaveX"
        Me.colSaveX.Text = "Save"
        '
        'colColumnNameY
        '
        Me.colColumnNameY.HeaderText = "New Name Y"
        Me.colColumnNameY.Name = "colColumnNameY"
        Me.colColumnNameY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'colSaveY
        '
        Me.colSaveY.HeaderText = "Save Y"
        Me.colSaveY.Name = "colSaveY"
        Me.colSaveY.Text = "Save"
        '
        'zPreview
        '
        Me.zPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zPreview.Location = New System.Drawing.Point(0, 0)
        Me.zPreview.Name = "zPreview"
        Me.zPreview.ScrollGrace = 0.0R
        Me.zPreview.ScrollMaxX = 0.0R
        Me.zPreview.ScrollMaxY = 0.0R
        Me.zPreview.ScrollMaxY2 = 0.0R
        Me.zPreview.ScrollMinX = 0.0R
        Me.zPreview.ScrollMinY = 0.0R
        Me.zPreview.ScrollMinY2 = 0.0R
        Me.zPreview.Size = New System.Drawing.Size(922, 496)
        Me.zPreview.TabIndex = 7
        '
        'HorSplit
        '
        Me.HorSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HorSplit.Location = New System.Drawing.Point(0, 0)
        Me.HorSplit.Name = "HorSplit"
        Me.HorSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'HorSplit.Panel1
        '
        Me.HorSplit.Panel1.Controls.Add(Me.TopVertSplit)
        '
        'HorSplit.Panel2
        '
        Me.HorSplit.Panel2.Controls.Add(Me.GroupBox1)
        Me.HorSplit.Size = New System.Drawing.Size(1120, 862)
        Me.HorSplit.SplitterDistance = 496
        Me.HorSplit.TabIndex = 8
        '
        'TopVertSplit
        '
        Me.TopVertSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TopVertSplit.Location = New System.Drawing.Point(0, 0)
        Me.TopVertSplit.Name = "TopVertSplit"
        '
        'TopVertSplit.Panel1
        '
        Me.TopVertSplit.Panel1.Controls.Add(Me.zPreview)
        '
        'TopVertSplit.Panel2
        '
        Me.TopVertSplit.Panel2.Controls.Add(Me.GroupBox5)
        Me.TopVertSplit.Panel2.Controls.Add(Me.GroupBox4)
        Me.TopVertSplit.Panel2.Controls.Add(Me.btnResetShiftsX)
        Me.TopVertSplit.Panel2.Controls.Add(Me.btnResetShiftsY)
        Me.TopVertSplit.Panel2.Controls.Add(Me.btnClose)
        Me.TopVertSplit.Panel2.Controls.Add(Me.GroupBox2)
        Me.TopVertSplit.Panel2.Controls.Add(Me.GroupBox3)
        Me.TopVertSplit.Size = New System.Drawing.Size(1120, 496)
        Me.TopVertSplit.SplitterDistance = 922
        Me.TopVertSplit.TabIndex = 8
        '
        'btnResetShiftsX
        '
        Me.btnResetShiftsX.Location = New System.Drawing.Point(21, 226)
        Me.btnResetShiftsX.Name = "btnResetShiftsX"
        Me.btnResetShiftsX.Size = New System.Drawing.Size(141, 20)
        Me.btnResetShiftsX.TabIndex = 14
        Me.btnResetShiftsX.Text = "reset all X shifts to zero"
        Me.btnResetShiftsX.UseVisualStyleBackColor = True
        '
        'btnResetShiftsY
        '
        Me.btnResetShiftsY.Location = New System.Drawing.Point(21, 252)
        Me.btnResetShiftsY.Name = "btnResetShiftsY"
        Me.btnResetShiftsY.Size = New System.Drawing.Size(141, 20)
        Me.btnResetShiftsY.TabIndex = 14
        Me.btnResetShiftsY.Text = "reset all Y shifts to zero"
        Me.btnResetShiftsY.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(4, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(187, 23)
        Me.btnClose.TabIndex = 16
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.btnShowAll)
        Me.GroupBox2.Controls.Add(Me.btnHideAll)
        Me.GroupBox2.Controls.Add(Me.ckbLogY)
        Me.GroupBox2.Controls.Add(Me.ckbLogX)
        Me.GroupBox2.Location = New System.Drawing.Point(4, 290)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(159, 73)
        Me.GroupBox2.TabIndex = 14
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Preview-Settings:"
        '
        'btnShowAll
        '
        Me.btnShowAll.Image = CType(resources.GetObject("btnShowAll.Image"), System.Drawing.Image)
        Me.btnShowAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShowAll.Location = New System.Drawing.Point(79, 40)
        Me.btnShowAll.Name = "btnShowAll"
        Me.btnShowAll.Size = New System.Drawing.Size(71, 26)
        Me.btnShowAll.TabIndex = 14
        Me.btnShowAll.Text = "Show all"
        Me.btnShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShowAll.UseVisualStyleBackColor = True
        '
        'btnHideAll
        '
        Me.btnHideAll.Image = CType(resources.GetObject("btnHideAll.Image"), System.Drawing.Image)
        Me.btnHideAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnHideAll.Location = New System.Drawing.Point(6, 40)
        Me.btnHideAll.Name = "btnHideAll"
        Me.btnHideAll.Size = New System.Drawing.Size(71, 26)
        Me.btnHideAll.TabIndex = 14
        Me.btnHideAll.Text = "Hide all"
        Me.btnHideAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnHideAll.UseVisualStyleBackColor = True
        '
        'ckbLogY
        '
        Me.ckbLogY.AutoSize = True
        Me.ckbLogY.Location = New System.Drawing.Point(91, 17)
        Me.ckbLogY.Name = "ckbLogY"
        Me.ckbLogY.Size = New System.Drawing.Size(50, 17)
        Me.ckbLogY.TabIndex = 11
        Me.ckbLogY.Text = "log Y"
        Me.ckbLogY.UseVisualStyleBackColor = True
        '
        'ckbLogX
        '
        Me.ckbLogX.AutoSize = True
        Me.ckbLogX.Location = New System.Drawing.Point(6, 17)
        Me.ckbLogX.Name = "ckbLogX"
        Me.ckbLogX.Size = New System.Drawing.Size(50, 17)
        Me.ckbLogX.TabIndex = 10
        Me.ckbLogX.Text = "log X"
        Me.ckbLogX.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.btnSaveYColumns)
        Me.GroupBox3.Controls.Add(Me.btnSaveXColumns)
        Me.GroupBox3.Controls.Add(Me.btnSaveAllColumns)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 369)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(184, 124)
        Me.GroupBox3.TabIndex = 15
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Save-Settings:"
        '
        'btnSaveYColumns
        '
        Me.btnSaveYColumns.Image = CType(resources.GetObject("btnSaveYColumns.Image"), System.Drawing.Image)
        Me.btnSaveYColumns.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveYColumns.Location = New System.Drawing.Point(6, 89)
        Me.btnSaveYColumns.Name = "btnSaveYColumns"
        Me.btnSaveYColumns.Size = New System.Drawing.Size(144, 29)
        Me.btnSaveYColumns.TabIndex = 13
        Me.btnSaveYColumns.Text = "Save all Y Columns"
        Me.btnSaveYColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveYColumns.UseVisualStyleBackColor = True
        '
        'btnSaveXColumns
        '
        Me.btnSaveXColumns.Image = CType(resources.GetObject("btnSaveXColumns.Image"), System.Drawing.Image)
        Me.btnSaveXColumns.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveXColumns.Location = New System.Drawing.Point(6, 54)
        Me.btnSaveXColumns.Name = "btnSaveXColumns"
        Me.btnSaveXColumns.Size = New System.Drawing.Size(144, 29)
        Me.btnSaveXColumns.TabIndex = 13
        Me.btnSaveXColumns.Text = "Save all X Columns"
        Me.btnSaveXColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveXColumns.UseVisualStyleBackColor = True
        '
        'btnSaveAllColumns
        '
        Me.btnSaveAllColumns.Image = CType(resources.GetObject("btnSaveAllColumns.Image"), System.Drawing.Image)
        Me.btnSaveAllColumns.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveAllColumns.Location = New System.Drawing.Point(6, 19)
        Me.btnSaveAllColumns.Name = "btnSaveAllColumns"
        Me.btnSaveAllColumns.Size = New System.Drawing.Size(125, 29)
        Me.btnSaveAllColumns.TabIndex = 13
        Me.btnSaveAllColumns.Text = "Save all Columns"
        Me.btnSaveAllColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveAllColumns.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Add value:"
        '
        'txtShiftAllXLinear
        '
        Me.txtShiftAllXLinear.Location = New System.Drawing.Point(10, 37)
        Me.txtShiftAllXLinear.Name = "txtShiftAllXLinear"
        Me.txtShiftAllXLinear.Size = New System.Drawing.Size(110, 20)
        Me.txtShiftAllXLinear.TabIndex = 2
        Me.txtShiftAllXLinear.Text = "0"
        Me.txtShiftAllXLinear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnShiftAllXLinear
        '
        Me.btnShiftAllXLinear.Location = New System.Drawing.Point(126, 37)
        Me.btnShiftAllXLinear.Name = "btnShiftAllXLinear"
        Me.btnShiftAllXLinear.Size = New System.Drawing.Size(27, 20)
        Me.btnShiftAllXLinear.TabIndex = 14
        Me.btnShiftAllXLinear.Text = "ok"
        Me.btnShiftAllXLinear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShiftAllXLinear.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnShiftAllXLinear)
        Me.GroupBox4.Controls.Add(Me.txtShiftAllXLinear)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Location = New System.Drawing.Point(17, 52)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(159, 68)
        Me.GroupBox4.TabIndex = 17
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Modify all X-Shifts:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Add value:"
        '
        'txtShiftAllYLinear
        '
        Me.txtShiftAllYLinear.Location = New System.Drawing.Point(10, 37)
        Me.txtShiftAllYLinear.Name = "txtShiftAllYLinear"
        Me.txtShiftAllYLinear.Size = New System.Drawing.Size(110, 20)
        Me.txtShiftAllYLinear.TabIndex = 2
        Me.txtShiftAllYLinear.Text = "0"
        Me.txtShiftAllYLinear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnShiftAllYLinear
        '
        Me.btnShiftAllYLinear.Location = New System.Drawing.Point(126, 37)
        Me.btnShiftAllYLinear.Name = "btnShiftAllYLinear"
        Me.btnShiftAllYLinear.Size = New System.Drawing.Size(27, 20)
        Me.btnShiftAllYLinear.TabIndex = 14
        Me.btnShiftAllYLinear.Text = "ok"
        Me.btnShiftAllYLinear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShiftAllYLinear.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.btnShiftAllYLinear)
        Me.GroupBox5.Controls.Add(Me.txtShiftAllYLinear)
        Me.GroupBox5.Controls.Add(Me.Label3)
        Me.GroupBox5.Location = New System.Drawing.Point(17, 126)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(159, 69)
        Me.GroupBox5.TabIndex = 17
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Modify all Y-Shifts:"
        '
        'wShiftColumnValues
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1120, 862)
        Me.Controls.Add(Me.HorSplit)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wShiftColumnValues"
        Me.Text = "Shift Column Values"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.dgvSpectroscopyFiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HorSplit.Panel1.ResumeLayout(False)
        Me.HorSplit.Panel2.ResumeLayout(False)
        CType(Me.HorSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HorSplit.ResumeLayout(False)
        Me.TopVertSplit.Panel1.ResumeLayout(False)
        Me.TopVertSplit.Panel2.ResumeLayout(False)
        CType(Me.TopVertSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TopVertSplit.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents dgvSpectroscopyFiles As System.Windows.Forms.DataGridView
    Friend WithEvents zPreview As ZedGraph.ZedGraphControl
    Friend WithEvents HorSplit As System.Windows.Forms.SplitContainer
    Friend WithEvents TopVertSplit As System.Windows.Forms.SplitContainer
    Friend WithEvents btnSaveAllColumns As System.Windows.Forms.Button
    Friend WithEvents ckbLogX As System.Windows.Forms.CheckBox
    Friend WithEvents ckbLogY As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents colShow As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colX As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colY As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colShiftX As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colShiftY As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colColumnNameX As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSaveX As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colColumnNameY As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSaveY As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSaveYColumns As System.Windows.Forms.Button
    Friend WithEvents btnSaveXColumns As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnShowAll As System.Windows.Forms.Button
    Friend WithEvents btnHideAll As System.Windows.Forms.Button
    Friend WithEvents btnResetShiftsY As System.Windows.Forms.Button
    Friend WithEvents btnResetShiftsX As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents btnShiftAllYLinear As System.Windows.Forms.Button
    Friend WithEvents txtShiftAllYLinear As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnShiftAllXLinear As System.Windows.Forms.Button
    Friend WithEvents txtShiftAllXLinear As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
