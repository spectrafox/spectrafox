<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wFitTestBCSDoubleGap
    Inherits System.Windows.Forms.Form

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
        Me.zgMeasuredData = New ZedGraph.ZedGraphControl()
        Me.zgTipDOS = New ZedGraph.ZedGraphControl()
        Me.btnStartFitting = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbExperimentaldIdV = New SpectroscopyManager.mPreviewBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.scFitOutput = New System.Windows.Forms.SplitContainer()
        Me.btnSaveParameters = New System.Windows.Forms.Button()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.gbParameters = New System.Windows.Forms.GroupBox()
        Me.txtFitRangeMax = New System.Windows.Forms.TextBox()
        Me.txtFitRangeMin = New System.Windows.Forms.TextBox()
        Me.txtYStretch = New System.Windows.Forms.TextBox()
        Me.txtYOffset = New System.Windows.Forms.TextBox()
        Me.txtXOffset = New System.Windows.Forms.TextBox()
        Me.txtGapRatio2vs1 = New System.Windows.Forms.TextBox()
        Me.txtSampleGap2 = New System.Windows.Forms.TextBox()
        Me.txtSampleGap1 = New System.Windows.Forms.TextBox()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTemperature = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTipGap = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBroadeningWidth = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.scParameters = New System.Windows.Forms.SplitContainer()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cboBroadeningType = New System.Windows.Forms.ComboBox()
        Me.cboFitDataType = New System.Windows.Forms.ComboBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.scFitOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scFitOutput.Panel1.SuspendLayout()
        Me.scFitOutput.Panel2.SuspendLayout()
        Me.scFitOutput.SuspendLayout()
        Me.gbParameters.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.scParameters, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scParameters.Panel1.SuspendLayout()
        Me.scParameters.Panel2.SuspendLayout()
        Me.scParameters.SuspendLayout()
        Me.SuspendLayout()
        '
        'zgMeasuredData
        '
        Me.zgMeasuredData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgMeasuredData.Location = New System.Drawing.Point(3, 16)
        Me.zgMeasuredData.Name = "zgMeasuredData"
        Me.zgMeasuredData.ScrollGrace = 0.0R
        Me.zgMeasuredData.ScrollMaxX = 0.0R
        Me.zgMeasuredData.ScrollMaxY = 0.0R
        Me.zgMeasuredData.ScrollMaxY2 = 0.0R
        Me.zgMeasuredData.ScrollMinX = 0.0R
        Me.zgMeasuredData.ScrollMinY = 0.0R
        Me.zgMeasuredData.ScrollMinY2 = 0.0R
        Me.zgMeasuredData.Size = New System.Drawing.Size(607, 338)
        Me.zgMeasuredData.TabIndex = 0
        '
        'zgTipDOS
        '
        Me.zgTipDOS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgTipDOS.Location = New System.Drawing.Point(3, 16)
        Me.zgTipDOS.Name = "zgTipDOS"
        Me.zgTipDOS.ScrollGrace = 0.0R
        Me.zgTipDOS.ScrollMaxX = 0.0R
        Me.zgTipDOS.ScrollMaxY = 0.0R
        Me.zgTipDOS.ScrollMaxY2 = 0.0R
        Me.zgTipDOS.ScrollMinX = 0.0R
        Me.zgTipDOS.ScrollMinY = 0.0R
        Me.zgTipDOS.ScrollMinY2 = 0.0R
        Me.zgTipDOS.Size = New System.Drawing.Size(409, 298)
        Me.zgTipDOS.TabIndex = 0
        '
        'btnStartFitting
        '
        Me.btnStartFitting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartFitting.Image = Global.SpectroscopyManager.My.Resources.ok_16
        Me.btnStartFitting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnStartFitting.Location = New System.Drawing.Point(599, 378)
        Me.btnStartFitting.Name = "btnStartFitting"
        Me.btnStartFitting.Size = New System.Drawing.Size(232, 39)
        Me.btnStartFitting.TabIndex = 1
        Me.btnStartFitting.Text = "Start Fitting"
        Me.btnStartFitting.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(3, 16)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(620, 398)
        Me.TextBox1.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.pbExperimentaldIdV)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(587, 412)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Experimental Data - dI/dV"
        '
        'pbExperimentaldIdV
        '
        Me.pbExperimentaldIdV.AllowAdjustingXColumn = True
        Me.pbExperimentaldIdV.AllowAdjustingYColumn = True
        Me.pbExperimentaldIdV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbExperimentaldIdV.Location = New System.Drawing.Point(3, 16)
        Me.pbExperimentaldIdV.Name = "pbExperimentaldIdV"
        Me.pbExperimentaldIdV.PointSelectionMode = False
        Me.pbExperimentaldIdV.ShowColumnSelectors = True
        Me.pbExperimentaldIdV.Size = New System.Drawing.Size(581, 393)
        Me.pbExperimentaldIdV.TabIndex = 6
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.zgTipDOS)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(415, 317)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Tip Density of States"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TextBox1)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(626, 417)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Fit Output:"
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.zgMeasuredData)
        Me.GroupBox4.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(613, 357)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Sample Density of States"
        '
        'scFitOutput
        '
        Me.scFitOutput.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.scFitOutput.Location = New System.Drawing.Point(0, 428)
        Me.scFitOutput.Name = "scFitOutput"
        '
        'scFitOutput.Panel1
        '
        Me.scFitOutput.Panel1.Controls.Add(Me.GroupBox3)
        '
        'scFitOutput.Panel2
        '
        Me.scFitOutput.Panel2.Controls.Add(Me.btnSaveParameters)
        Me.scFitOutput.Panel2.Controls.Add(Me.btnSaveColumn)
        Me.scFitOutput.Panel2.Controls.Add(Me.txtNewColumnName)
        Me.scFitOutput.Panel2.Controls.Add(Me.Label12)
        Me.scFitOutput.Panel2.Controls.Add(Me.GroupBox4)
        Me.scFitOutput.Size = New System.Drawing.Size(1258, 423)
        Me.scFitOutput.SplitterDistance = 638
        Me.scFitOutput.TabIndex = 7
        '
        'btnSaveParameters
        '
        Me.btnSaveParameters.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveParameters.Enabled = False
        Me.btnSaveParameters.Image = Global.SpectroscopyManager.My.Resources.save_16
        Me.btnSaveParameters.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveParameters.Location = New System.Drawing.Point(320, 386)
        Me.btnSaveParameters.Name = "btnSaveParameters"
        Me.btnSaveParameters.Size = New System.Drawing.Size(284, 29)
        Me.btnSaveParameters.TabIndex = 13
        Me.btnSaveParameters.Text = "Save Parameters to Comment"
        Me.btnSaveParameters.UseVisualStyleBackColor = True
        '
        'btnSaveColumn
        '
        Me.btnSaveColumn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveColumn.Enabled = False
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.save_16
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(20, 386)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(284, 29)
        Me.btnSaveColumn.TabIndex = 13
        Me.btnSaveColumn.Text = "Save Column"
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewColumnName.Location = New System.Drawing.Point(138, 361)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(166, 20)
        Me.txtNewColumnName.TabIndex = 12
        Me.txtNewColumnName.Text = "Fitted Data"
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(17, 364)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(115, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Save as ColumnName:"
        '
        'gbParameters
        '
        Me.gbParameters.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbParameters.Controls.Add(Me.cboFitDataType)
        Me.gbParameters.Controls.Add(Me.cboBroadeningType)
        Me.gbParameters.Controls.Add(Me.txtFitRangeMax)
        Me.gbParameters.Controls.Add(Me.txtFitRangeMin)
        Me.gbParameters.Controls.Add(Me.txtBroadeningWidth)
        Me.gbParameters.Controls.Add(Me.txtYStretch)
        Me.gbParameters.Controls.Add(Me.Label7)
        Me.gbParameters.Controls.Add(Me.Label6)
        Me.gbParameters.Controls.Add(Me.txtYOffset)
        Me.gbParameters.Controls.Add(Me.txtXOffset)
        Me.gbParameters.Controls.Add(Me.txtGapRatio2vs1)
        Me.gbParameters.Controls.Add(Me.txtSampleGap2)
        Me.gbParameters.Controls.Add(Me.txtSampleGap1)
        Me.gbParameters.Controls.Add(Me.Button12)
        Me.gbParameters.Controls.Add(Me.Label13)
        Me.gbParameters.Controls.Add(Me.Label11)
        Me.gbParameters.Controls.Add(Me.Button10)
        Me.gbParameters.Controls.Add(Me.Label10)
        Me.gbParameters.Controls.Add(Me.Button9)
        Me.gbParameters.Controls.Add(Me.Label9)
        Me.gbParameters.Controls.Add(Me.Button8)
        Me.gbParameters.Controls.Add(Me.Label8)
        Me.gbParameters.Controls.Add(Me.Button5)
        Me.gbParameters.Controls.Add(Me.Label5)
        Me.gbParameters.Controls.Add(Me.Button4)
        Me.gbParameters.Controls.Add(Me.Label4)
        Me.gbParameters.Controls.Add(Me.Button3)
        Me.gbParameters.Controls.Add(Me.Label3)
        Me.gbParameters.Location = New System.Drawing.Point(599, 8)
        Me.gbParameters.Name = "gbParameters"
        Me.gbParameters.Size = New System.Drawing.Size(232, 355)
        Me.gbParameters.TabIndex = 8
        Me.gbParameters.TabStop = False
        Me.gbParameters.Text = "define initial set of parameters"
        '
        'txtFitRangeMax
        '
        Me.txtFitRangeMax.Location = New System.Drawing.Point(112, 217)
        Me.txtFitRangeMax.Name = "txtFitRangeMax"
        Me.txtFitRangeMax.Size = New System.Drawing.Size(63, 20)
        Me.txtFitRangeMax.TabIndex = 2
        Me.txtFitRangeMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFitRangeMin
        '
        Me.txtFitRangeMin.Location = New System.Drawing.Point(112, 192)
        Me.txtFitRangeMin.Name = "txtFitRangeMin"
        Me.txtFitRangeMin.Size = New System.Drawing.Size(63, 20)
        Me.txtFitRangeMin.TabIndex = 2
        Me.txtFitRangeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtYStretch
        '
        Me.txtYStretch.Location = New System.Drawing.Point(112, 163)
        Me.txtYStretch.Name = "txtYStretch"
        Me.txtYStretch.Size = New System.Drawing.Size(63, 20)
        Me.txtYStretch.TabIndex = 2
        Me.txtYStretch.Text = "1"
        Me.txtYStretch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtYOffset
        '
        Me.txtYOffset.Location = New System.Drawing.Point(112, 134)
        Me.txtYOffset.Name = "txtYOffset"
        Me.txtYOffset.Size = New System.Drawing.Size(63, 20)
        Me.txtYOffset.TabIndex = 2
        Me.txtYOffset.Text = "0"
        Me.txtYOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtXOffset
        '
        Me.txtXOffset.Location = New System.Drawing.Point(112, 105)
        Me.txtXOffset.Name = "txtXOffset"
        Me.txtXOffset.Size = New System.Drawing.Size(63, 20)
        Me.txtXOffset.TabIndex = 2
        Me.txtXOffset.Text = "0"
        Me.txtXOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGapRatio2vs1
        '
        Me.txtGapRatio2vs1.Location = New System.Drawing.Point(112, 76)
        Me.txtGapRatio2vs1.Name = "txtGapRatio2vs1"
        Me.txtGapRatio2vs1.Size = New System.Drawing.Size(63, 20)
        Me.txtGapRatio2vs1.TabIndex = 2
        Me.txtGapRatio2vs1.Text = "0.1"
        Me.txtGapRatio2vs1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSampleGap2
        '
        Me.txtSampleGap2.Location = New System.Drawing.Point(112, 47)
        Me.txtSampleGap2.Name = "txtSampleGap2"
        Me.txtSampleGap2.Size = New System.Drawing.Size(63, 20)
        Me.txtSampleGap2.TabIndex = 2
        Me.txtSampleGap2.Text = "1.42"
        Me.txtSampleGap2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSampleGap1
        '
        Me.txtSampleGap1.Location = New System.Drawing.Point(112, 18)
        Me.txtSampleGap1.Name = "txtSampleGap1"
        Me.txtSampleGap1.Size = New System.Drawing.Size(63, 20)
        Me.txtSampleGap1.TabIndex = 2
        Me.txtSampleGap1.Text = "1.32"
        Me.txtSampleGap1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button12
        '
        Me.Button12.Location = New System.Drawing.Point(181, 203)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(45, 23)
        Me.Button12.TabIndex = 1
        Me.Button12.Text = "select"
        Me.Button12.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 247)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(74, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Fit Data Type:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 195)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(78, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Fit-Range [eV]:"
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(181, 161)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(45, 23)
        Me.Button10.TabIndex = 1
        Me.Button10.Text = "select"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 166)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Y Stretch Factor:"
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(181, 132)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(45, 23)
        Me.Button9.TabIndex = 1
        Me.Button9.Text = "select"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 137)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Global Y Offset [eV]"
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(181, 103)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(45, 23)
        Me.Button8.TabIndex = 1
        Me.Button8.Text = "select"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 108)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(103, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Global X Offset [eV]:"
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(181, 74)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(45, 23)
        Me.Button5.TabIndex = 1
        Me.Button5.Text = "select"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 79)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(101, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Gap Ratio Gap 2/1:"
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(181, 45)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(45, 23)
        Me.Button4.TabIndex = 1
        Me.Button4.Text = "select"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Sample Gap 2 [eV]:"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(181, 16)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(45, 23)
        Me.Button3.TabIndex = 1
        Me.Button3.Text = "select"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Sample Gap 1 [eV]:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Temperature [K]:"
        '
        'txtTemperature
        '
        Me.txtTemperature.Location = New System.Drawing.Point(120, 24)
        Me.txtTemperature.Name = "txtTemperature"
        Me.txtTemperature.Size = New System.Drawing.Size(63, 20)
        Me.txtTemperature.TabIndex = 2
        Me.txtTemperature.Text = "1.19"
        Me.txtTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(206, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tip Gap [eV]:"
        '
        'txtTipGap
        '
        Me.txtTipGap.Location = New System.Drawing.Point(312, 24)
        Me.txtTipGap.Name = "txtTipGap"
        Me.txtTipGap.Size = New System.Drawing.Size(63, 20)
        Me.txtTipGap.TabIndex = 2
        Me.txtTipGap.Text = "1.34"
        Me.txtTipGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 301)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(94, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Broad. Width [eV]:"
        '
        'txtBroadeningWidth
        '
        Me.txtBroadeningWidth.Location = New System.Drawing.Point(136, 298)
        Me.txtBroadeningWidth.Name = "txtBroadeningWidth"
        Me.txtBroadeningWidth.Size = New System.Drawing.Size(63, 20)
        Me.txtBroadeningWidth.TabIndex = 2
        Me.txtBroadeningWidth.Text = "0.5"
        Me.txtBroadeningWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.Controls.Add(Me.Label1)
        Me.GroupBox5.Controls.Add(Me.Label2)
        Me.GroupBox5.Controls.Add(Me.txtTemperature)
        Me.GroupBox5.Controls.Add(Me.txtTipGap)
        Me.GroupBox5.Location = New System.Drawing.Point(7, 323)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(405, 58)
        Me.GroupBox5.TabIndex = 9
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Define Tip Parameters:"
        '
        'scParameters
        '
        Me.scParameters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scParameters.Location = New System.Drawing.Point(0, 0)
        Me.scParameters.Name = "scParameters"
        '
        'scParameters.Panel1
        '
        Me.scParameters.Panel1.Controls.Add(Me.gbParameters)
        Me.scParameters.Panel1.Controls.Add(Me.GroupBox1)
        Me.scParameters.Panel1.Controls.Add(Me.btnStartFitting)
        '
        'scParameters.Panel2
        '
        Me.scParameters.Panel2.Controls.Add(Me.GroupBox5)
        Me.scParameters.Panel2.Controls.Add(Me.GroupBox2)
        Me.scParameters.Size = New System.Drawing.Size(1258, 428)
        Me.scParameters.SplitterDistance = 839
        Me.scParameters.TabIndex = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 274)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Broad. Type:"
        '
        'cboBroadeningType
        '
        Me.cboBroadeningType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBroadeningType.FormattingEnabled = True
        Me.cboBroadeningType.Location = New System.Drawing.Point(112, 271)
        Me.cboBroadeningType.Name = "cboBroadeningType"
        Me.cboBroadeningType.Size = New System.Drawing.Size(114, 21)
        Me.cboBroadeningType.TabIndex = 4
        '
        'cboFitDataType
        '
        Me.cboFitDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFitDataType.FormattingEnabled = True
        Me.cboFitDataType.Location = New System.Drawing.Point(112, 244)
        Me.cboFitDataType.Name = "cboFitDataType"
        Me.cboFitDataType.Size = New System.Drawing.Size(114, 21)
        Me.cboFitDataType.TabIndex = 5
        '
        'wFitTestBCSDoubleGap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1258, 851)
        Me.Controls.Add(Me.scParameters)
        Me.Controls.Add(Me.scFitOutput)
        Me.Name = "wFitTestBCSDoubleGap"
        Me.Text = "Double Gap Fitting"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.scFitOutput.Panel1.ResumeLayout(False)
        Me.scFitOutput.Panel2.ResumeLayout(False)
        Me.scFitOutput.Panel2.PerformLayout()
        CType(Me.scFitOutput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scFitOutput.ResumeLayout(False)
        Me.gbParameters.ResumeLayout(False)
        Me.gbParameters.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.scParameters.Panel1.ResumeLayout(False)
        Me.scParameters.Panel2.ResumeLayout(False)
        CType(Me.scParameters, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scParameters.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents zgMeasuredData As ZedGraph.ZedGraphControl
    Friend WithEvents zgTipDOS As ZedGraph.ZedGraphControl
    Friend WithEvents btnStartFitting As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents pbExperimentaldIdV As SpectroscopyManager.mPreviewBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents scFitOutput As System.Windows.Forms.SplitContainer
    Friend WithEvents gbParameters As System.Windows.Forms.GroupBox
    Friend WithEvents txtTemperature As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFitRangeMax As System.Windows.Forms.TextBox
    Friend WithEvents txtFitRangeMin As System.Windows.Forms.TextBox
    Friend WithEvents txtYStretch As System.Windows.Forms.TextBox
    Friend WithEvents txtYOffset As System.Windows.Forms.TextBox
    Friend WithEvents txtXOffset As System.Windows.Forms.TextBox
    Friend WithEvents txtBroadeningWidth As System.Windows.Forms.TextBox
    Friend WithEvents txtGapRatio2vs1 As System.Windows.Forms.TextBox
    Friend WithEvents txtSampleGap2 As System.Windows.Forms.TextBox
    Friend WithEvents txtSampleGap1 As System.Windows.Forms.TextBox
    Friend WithEvents txtTipGap As System.Windows.Forms.TextBox
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents scParameters As System.Windows.Forms.SplitContainer
    Friend WithEvents btnSaveParameters As System.Windows.Forms.Button
    Friend WithEvents btnSaveColumn As System.Windows.Forms.Button
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cboFitDataType As System.Windows.Forms.ComboBox
    Friend WithEvents cboBroadeningType As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
