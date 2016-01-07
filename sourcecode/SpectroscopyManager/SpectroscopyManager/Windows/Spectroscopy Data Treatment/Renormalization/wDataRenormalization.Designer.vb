<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataRenormalization
    Inherits SpectroscopyManager.wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataRenormalization))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pbSourceData = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.pbTargetData = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.pbOutput = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.btnSaveColumns = New System.Windows.Forms.Button()
        Me.btnApplyRenormalization = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDerivedDataColumnName = New System.Windows.Forms.TextBox()
        Me.txtRenormalizedDataColumnName = New System.Windows.Forms.TextBox()
        Me.txtSmoothedDataColumnName = New System.Windows.Forms.TextBox()
        Me.ckbSaveDerivedData = New System.Windows.Forms.CheckBox()
        Me.ckbSaveSmoothedData = New System.Windows.Forms.CheckBox()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.SplitContainerMain = New System.Windows.Forms.SplitContainer()
        Me.SplitContainerLeft = New System.Windows.Forms.SplitContainer()
        Me.dsDataSmoothing = New SpectroscopyManager.mDataSmoothing()
        Me.SplitContainerRight = New System.Windows.Forms.SplitContainer()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.SplitContainerMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerMain.Panel1.SuspendLayout()
        Me.SplitContainerMain.Panel2.SuspendLayout()
        Me.SplitContainerMain.SuspendLayout()
        CType(Me.SplitContainerLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerLeft.Panel1.SuspendLayout()
        Me.SplitContainerLeft.Panel2.SuspendLayout()
        Me.SplitContainerLeft.SuspendLayout()
        CType(Me.SplitContainerRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerRight.Panel1.SuspendLayout()
        Me.SplitContainerRight.Panel2.SuspendLayout()
        Me.SplitContainerRight.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pbSourceData)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(539, 360)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Reference data of which the numeric derivative will be calculated"
        '
        'pbSourceData
        '
        Me.pbSourceData.AllowAdjustingXColumn = True
        Me.pbSourceData.AllowAdjustingYColumn = True
        Me.pbSourceData.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbSourceData.CallbackXRangeSelected = Nothing
        Me.pbSourceData.CallbackXValueSelected = Nothing
        Me.pbSourceData.CallbackXYRangeSelected = Nothing
        Me.pbSourceData.CallbackYRangeSelected = Nothing
        Me.pbSourceData.CallbackYValueSelected = Nothing
        Me.pbSourceData.ClearPointSelectionModeAfterSelection = False
        Me.pbSourceData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbSourceData.Location = New System.Drawing.Point(3, 16)
        Me.pbSourceData.Name = "pbSourceData"
        Me.pbSourceData.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbSourceData.ShowColumnSelectors = True
        Me.pbSourceData.Size = New System.Drawing.Size(533, 341)
        Me.pbSourceData.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbTargetData)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(625, 328)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Data to re-gauge by fitting it to the reference data on the left"
        '
        'pbTargetData
        '
        Me.pbTargetData.AllowAdjustingXColumn = True
        Me.pbTargetData.AllowAdjustingYColumn = True
        Me.pbTargetData.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbTargetData.CallbackXRangeSelected = Nothing
        Me.pbTargetData.CallbackXValueSelected = Nothing
        Me.pbTargetData.CallbackXYRangeSelected = Nothing
        Me.pbTargetData.CallbackYRangeSelected = Nothing
        Me.pbTargetData.CallbackYValueSelected = Nothing
        Me.pbTargetData.ClearPointSelectionModeAfterSelection = False
        Me.pbTargetData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbTargetData.Location = New System.Drawing.Point(3, 16)
        Me.pbTargetData.Name = "pbTargetData"
        Me.pbTargetData.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbTargetData.ShowColumnSelectors = True
        Me.pbTargetData.Size = New System.Drawing.Size(619, 309)
        Me.pbTargetData.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.pbOutput)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(625, 352)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Re-gauged result"
        '
        'pbOutput
        '
        Me.pbOutput.AllowAdjustingXColumn = True
        Me.pbOutput.AllowAdjustingYColumn = True
        Me.pbOutput.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbOutput.CallbackXRangeSelected = Nothing
        Me.pbOutput.CallbackXValueSelected = Nothing
        Me.pbOutput.CallbackXYRangeSelected = Nothing
        Me.pbOutput.CallbackYRangeSelected = Nothing
        Me.pbOutput.CallbackYValueSelected = Nothing
        Me.pbOutput.ClearPointSelectionModeAfterSelection = False
        Me.pbOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbOutput.Location = New System.Drawing.Point(3, 16)
        Me.pbOutput.Name = "pbOutput"
        Me.pbOutput.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbOutput.ShowColumnSelectors = True
        Me.pbOutput.Size = New System.Drawing.Size(619, 333)
        Me.pbOutput.TabIndex = 0
        '
        'btnSaveColumns
        '
        Me.btnSaveColumns.Enabled = False
        Me.btnSaveColumns.Image = Global.SpectroscopyManager.My.Resources.Resources.save_25
        Me.btnSaveColumns.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumns.Location = New System.Drawing.Point(6, 172)
        Me.btnSaveColumns.Name = "btnSaveColumns"
        Me.btnSaveColumns.Size = New System.Drawing.Size(167, 43)
        Me.btnSaveColumns.TabIndex = 8
        Me.btnSaveColumns.Text = "Save Columns"
        Me.btnSaveColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveColumns.UseVisualStyleBackColor = True
        '
        'btnApplyRenormalization
        '
        Me.btnApplyRenormalization.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyRenormalization.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyRenormalization.Location = New System.Drawing.Point(261, 9)
        Me.btnApplyRenormalization.Name = "btnApplyRenormalization"
        Me.btnApplyRenormalization.Size = New System.Drawing.Size(179, 53)
        Me.btnApplyRenormalization.TabIndex = 8
        Me.btnApplyRenormalization.Text = "gauge data now"
        Me.btnApplyRenormalization.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyRenormalization.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.txtRenormalizedDataColumnName)
        Me.GroupBox4.Controls.Add(Me.txtSmoothedDataColumnName)
        Me.GroupBox4.Controls.Add(Me.ckbSaveDerivedData)
        Me.GroupBox4.Controls.Add(Me.ckbSaveSmoothedData)
        Me.GroupBox4.Controls.Add(Me.btnSaveColumns)
        Me.GroupBox4.Controls.Add(Me.txtDerivedDataColumnName)
        Me.GroupBox4.Location = New System.Drawing.Point(261, 68)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(179, 219)
        Me.GroupBox4.TabIndex = 9
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Data Saving"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Re-gauged Column Name:"
        '
        'txtDerivedDataColumnName
        '
        Me.txtDerivedDataColumnName.Enabled = False
        Me.txtDerivedDataColumnName.Location = New System.Drawing.Point(6, 146)
        Me.txtDerivedDataColumnName.Name = "txtDerivedDataColumnName"
        Me.txtDerivedDataColumnName.Size = New System.Drawing.Size(167, 20)
        Me.txtDerivedDataColumnName.TabIndex = 10
        Me.txtDerivedDataColumnName.Text = "Derivative of Data"
        '
        'txtRenormalizedDataColumnName
        '
        Me.txtRenormalizedDataColumnName.Location = New System.Drawing.Point(6, 35)
        Me.txtRenormalizedDataColumnName.Name = "txtRenormalizedDataColumnName"
        Me.txtRenormalizedDataColumnName.Size = New System.Drawing.Size(167, 20)
        Me.txtRenormalizedDataColumnName.TabIndex = 10
        Me.txtRenormalizedDataColumnName.Text = "Regauged Data"
        '
        'txtSmoothedDataColumnName
        '
        Me.txtSmoothedDataColumnName.Enabled = False
        Me.txtSmoothedDataColumnName.Location = New System.Drawing.Point(6, 94)
        Me.txtSmoothedDataColumnName.Name = "txtSmoothedDataColumnName"
        Me.txtSmoothedDataColumnName.Size = New System.Drawing.Size(167, 20)
        Me.txtSmoothedDataColumnName.TabIndex = 10
        Me.txtSmoothedDataColumnName.Text = "Smoothed Data"
        '
        'ckbSaveDerivedData
        '
        Me.ckbSaveDerivedData.AutoSize = True
        Me.ckbSaveDerivedData.Location = New System.Drawing.Point(6, 127)
        Me.ckbSaveDerivedData.Name = "ckbSaveDerivedData"
        Me.ckbSaveDerivedData.Size = New System.Drawing.Size(140, 17)
        Me.ckbSaveDerivedData.TabIndex = 9
        Me.ckbSaveDerivedData.Text = "saved derivative of data"
        Me.ckbSaveDerivedData.UseVisualStyleBackColor = True
        '
        'ckbSaveSmoothedData
        '
        Me.ckbSaveSmoothedData.AutoSize = True
        Me.ckbSaveSmoothedData.Location = New System.Drawing.Point(6, 73)
        Me.ckbSaveSmoothedData.Name = "ckbSaveSmoothedData"
        Me.ckbSaveSmoothedData.Size = New System.Drawing.Size(122, 17)
        Me.ckbSaveSmoothedData.TabIndex = 9
        Me.ckbSaveSmoothedData.Text = "save smoothed data"
        Me.ckbSaveSmoothedData.UseVisualStyleBackColor = True
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(992, 752)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(182, 29)
        Me.btnCloseWindow.TabIndex = 8
        Me.btnCloseWindow.Text = "Close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'SplitContainerMain
        '
        Me.SplitContainerMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainerMain.Location = New System.Drawing.Point(2, 62)
        Me.SplitContainerMain.Name = "SplitContainerMain"
        '
        'SplitContainerMain.Panel1
        '
        Me.SplitContainerMain.Panel1.Controls.Add(Me.SplitContainerLeft)
        '
        'SplitContainerMain.Panel2
        '
        Me.SplitContainerMain.Panel2.Controls.Add(Me.SplitContainerRight)
        Me.SplitContainerMain.Size = New System.Drawing.Size(1172, 688)
        Me.SplitContainerMain.SplitterDistance = 541
        Me.SplitContainerMain.TabIndex = 10
        '
        'SplitContainerLeft
        '
        Me.SplitContainerLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerLeft.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerLeft.Name = "SplitContainerLeft"
        Me.SplitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerLeft.Panel1
        '
        Me.SplitContainerLeft.Panel1.Controls.Add(Me.GroupBox1)
        '
        'SplitContainerLeft.Panel2
        '
        Me.SplitContainerLeft.Panel2.Controls.Add(Me.GroupBox4)
        Me.SplitContainerLeft.Panel2.Controls.Add(Me.dsDataSmoothing)
        Me.SplitContainerLeft.Panel2.Controls.Add(Me.btnApplyRenormalization)
        Me.SplitContainerLeft.Size = New System.Drawing.Size(541, 688)
        Me.SplitContainerLeft.SplitterDistance = 362
        Me.SplitContainerLeft.TabIndex = 0
        '
        'dsDataSmoothing
        '
        Me.dsDataSmoothing.Location = New System.Drawing.Point(1, 3)
        Me.dsDataSmoothing.Name = "dsDataSmoothing"
        Me.dsDataSmoothing.SelectedSmoothingMethod = SpectroscopyManager.cNumericalMethods.SmoothingMethod.SavitzkyGolay
        Me.dsDataSmoothing.Size = New System.Drawing.Size(260, 284)
        Me.dsDataSmoothing.SmoothingParameter = 5
        Me.dsDataSmoothing.TabIndex = 7
        '
        'SplitContainerRight
        '
        Me.SplitContainerRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerRight.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerRight.Name = "SplitContainerRight"
        Me.SplitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerRight.Panel1
        '
        Me.SplitContainerRight.Panel1.Controls.Add(Me.GroupBox2)
        '
        'SplitContainerRight.Panel2
        '
        Me.SplitContainerRight.Panel2.Controls.Add(Me.GroupBox3)
        Me.SplitContainerRight.Size = New System.Drawing.Size(627, 688)
        Me.SplitContainerRight.SplitterDistance = 330
        Me.SplitContainerRight.TabIndex = 0
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(6, 11)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(553, 39)
        Me.lblDescription.TabIndex = 11
        Me.lblDescription.Text = resources.GetString("lblDescription.Text")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(574, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(545, 39)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = resources.GetString("Label2.Text")
        '
        'wDataRenormalization
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1177, 784)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.SplitContainerMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataRenormalization"
        Me.Text = "Gauge Data to Numeric Derivative"
        Me.Controls.SetChildIndex(Me.SplitContainerMain, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.lblDescription, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.SplitContainerMain.Panel1.ResumeLayout(False)
        Me.SplitContainerMain.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerMain.ResumeLayout(False)
        Me.SplitContainerLeft.Panel1.ResumeLayout(False)
        Me.SplitContainerLeft.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerLeft.ResumeLayout(False)
        Me.SplitContainerRight.Panel1.ResumeLayout(False)
        Me.SplitContainerRight.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerRight.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents pbSourceData As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents pbTargetData As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents dsDataSmoothing As SpectroscopyManager.mDataSmoothing
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents pbOutput As SpectroscopyManager.mSpectroscopyTableViewer
    Friend WithEvents btnSaveColumns As System.Windows.Forms.Button
    Friend WithEvents btnApplyRenormalization As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents txtRenormalizedDataColumnName As System.Windows.Forms.TextBox
    Friend WithEvents txtSmoothedDataColumnName As System.Windows.Forms.TextBox
    Friend WithEvents ckbSaveDerivedData As System.Windows.Forms.CheckBox
    Friend WithEvents ckbSaveSmoothedData As System.Windows.Forms.CheckBox
    Friend WithEvents txtDerivedDataColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SplitContainerMain As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerLeft As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerRight As System.Windows.Forms.SplitContainer
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
