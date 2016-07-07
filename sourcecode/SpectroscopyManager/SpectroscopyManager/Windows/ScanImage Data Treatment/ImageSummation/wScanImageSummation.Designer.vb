<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wScanImageSummation
    Inherits wFormBaseExpectsMultipleScanImagesOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wScanImageSummation))
        Me.scSource1 = New SpectroscopyManager.mScanImageViewer()
        Me.gbSource1 = New System.Windows.Forms.GroupBox()
        Me.scSourceData = New System.Windows.Forms.SplitContainer()
        Me.gbSource2 = New System.Windows.Forms.GroupBox()
        Me.scSource2 = New SpectroscopyManager.mScanImageViewer()
        Me.scSourceOutput = New System.Windows.Forms.SplitContainer()
        Me.btnExchange1and2 = New System.Windows.Forms.Button()
        Me.gbMergeOperation = New System.Windows.Forms.GroupBox()
        Me.txtCombinationFactor = New SpectroscopyManager.NumericTextbox()
        Me.lblOperationFactor = New System.Windows.Forms.Label()
        Me.rdbMergeOperation_Division = New System.Windows.Forms.RadioButton()
        Me.rdbMergeOperation_Override = New System.Windows.Forms.RadioButton()
        Me.rdbMergeOperation_Multiplication = New System.Windows.Forms.RadioButton()
        Me.rdbMergeOperation_Summation = New System.Windows.Forms.RadioButton()
        Me.gbOffsetAdjustment = New System.Windows.Forms.GroupBox()
        Me.txtAngleOffset = New SpectroscopyManager.NumericTextbox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtYOffset = New SpectroscopyManager.NumericTextbox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtXOffset = New SpectroscopyManager.NumericTextbox()
        Me.lblXOffset = New System.Windows.Forms.Label()
        Me.gbOutput = New System.Windows.Forms.GroupBox()
        Me.scOutput = New SpectroscopyManager.mScanImageViewer()
        Me.gbSource1.SuspendLayout()
        CType(Me.scSourceData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scSourceData.Panel1.SuspendLayout()
        Me.scSourceData.Panel2.SuspendLayout()
        Me.scSourceData.SuspendLayout()
        Me.gbSource2.SuspendLayout()
        CType(Me.scSourceOutput, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scSourceOutput.Panel1.SuspendLayout()
        Me.scSourceOutput.Panel2.SuspendLayout()
        Me.scSourceOutput.SuspendLayout()
        Me.gbMergeOperation.SuspendLayout()
        Me.gbOffsetAdjustment.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        Me.SuspendLayout()
        '
        'scSource1
        '
        Me.scSource1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scSource1.Location = New System.Drawing.Point(3, 16)
        Me.scSource1.Name = "scSource1"
        Me.scSource1.SelectedPoints_PixelCoordinate = CType(resources.GetObject("scSource1.SelectedPoints_PixelCoordinate"), System.Collections.Generic.List(Of System.Drawing.Point))
        Me.scSource1.SelectionMode = SpectroscopyManager.mScanImageViewer.SelectionModes.None
        Me.scSource1.Size = New System.Drawing.Size(410, 371)
        Me.scSource1.TabIndex = 1
        '
        'gbSource1
        '
        Me.gbSource1.Controls.Add(Me.scSource1)
        Me.gbSource1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSource1.Location = New System.Drawing.Point(0, 0)
        Me.gbSource1.Name = "gbSource1"
        Me.gbSource1.Size = New System.Drawing.Size(416, 390)
        Me.gbSource1.TabIndex = 2
        Me.gbSource1.TabStop = False
        Me.gbSource1.Text = "FILENAME"
        '
        'scSourceData
        '
        Me.scSourceData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scSourceData.Location = New System.Drawing.Point(0, 0)
        Me.scSourceData.Name = "scSourceData"
        '
        'scSourceData.Panel1
        '
        Me.scSourceData.Panel1.Controls.Add(Me.gbSource1)
        '
        'scSourceData.Panel2
        '
        Me.scSourceData.Panel2.Controls.Add(Me.gbSource2)
        Me.scSourceData.Size = New System.Drawing.Size(832, 390)
        Me.scSourceData.SplitterDistance = 416
        Me.scSourceData.TabIndex = 3
        '
        'gbSource2
        '
        Me.gbSource2.Controls.Add(Me.scSource2)
        Me.gbSource2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSource2.Location = New System.Drawing.Point(0, 0)
        Me.gbSource2.Name = "gbSource2"
        Me.gbSource2.Size = New System.Drawing.Size(412, 390)
        Me.gbSource2.TabIndex = 3
        Me.gbSource2.TabStop = False
        Me.gbSource2.Text = "FILENAME"
        '
        'scSource2
        '
        Me.scSource2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scSource2.Location = New System.Drawing.Point(3, 16)
        Me.scSource2.Name = "scSource2"
        Me.scSource2.SelectedPoints_PixelCoordinate = CType(resources.GetObject("scSource2.SelectedPoints_PixelCoordinate"), System.Collections.Generic.List(Of System.Drawing.Point))
        Me.scSource2.SelectionMode = SpectroscopyManager.mScanImageViewer.SelectionModes.None
        Me.scSource2.Size = New System.Drawing.Size(406, 371)
        Me.scSource2.TabIndex = 1
        '
        'scSourceOutput
        '
        Me.scSourceOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scSourceOutput.Location = New System.Drawing.Point(0, 0)
        Me.scSourceOutput.Name = "scSourceOutput"
        Me.scSourceOutput.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scSourceOutput.Panel1
        '
        Me.scSourceOutput.Panel1.Controls.Add(Me.scSourceData)
        '
        'scSourceOutput.Panel2
        '
        Me.scSourceOutput.Panel2.Controls.Add(Me.btnExchange1and2)
        Me.scSourceOutput.Panel2.Controls.Add(Me.gbMergeOperation)
        Me.scSourceOutput.Panel2.Controls.Add(Me.gbOffsetAdjustment)
        Me.scSourceOutput.Panel2.Controls.Add(Me.gbOutput)
        Me.scSourceOutput.Size = New System.Drawing.Size(832, 911)
        Me.scSourceOutput.SplitterDistance = 390
        Me.scSourceOutput.TabIndex = 4
        '
        'btnExchange1and2
        '
        Me.btnExchange1and2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExchange1and2.Image = Global.SpectroscopyManager.My.Resources.Resources.rangeselection_12
        Me.btnExchange1and2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExchange1and2.Location = New System.Drawing.Point(630, 14)
        Me.btnExchange1and2.Name = "btnExchange1and2"
        Me.btnExchange1and2.Size = New System.Drawing.Size(199, 23)
        Me.btnExchange1and2.TabIndex = 100
        Me.btnExchange1and2.Text = "exchange first and second file"
        Me.btnExchange1and2.UseVisualStyleBackColor = True
        '
        'gbMergeOperation
        '
        Me.gbMergeOperation.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbMergeOperation.Controls.Add(Me.txtCombinationFactor)
        Me.gbMergeOperation.Controls.Add(Me.lblOperationFactor)
        Me.gbMergeOperation.Controls.Add(Me.rdbMergeOperation_Division)
        Me.gbMergeOperation.Controls.Add(Me.rdbMergeOperation_Override)
        Me.gbMergeOperation.Controls.Add(Me.rdbMergeOperation_Multiplication)
        Me.gbMergeOperation.Controls.Add(Me.rdbMergeOperation_Summation)
        Me.gbMergeOperation.Location = New System.Drawing.Point(629, 43)
        Me.gbMergeOperation.Name = "gbMergeOperation"
        Me.gbMergeOperation.Size = New System.Drawing.Size(200, 109)
        Me.gbMergeOperation.TabIndex = 8
        Me.gbMergeOperation.TabStop = False
        Me.gbMergeOperation.Text = "merge operation"
        '
        'txtCombinationFactor
        '
        Me.txtCombinationFactor.AllowZero = True
        Me.txtCombinationFactor.BackColor = System.Drawing.Color.White
        Me.txtCombinationFactor.ForeColor = System.Drawing.Color.Black
        Me.txtCombinationFactor.FormatDecimalPlaces = 6
        Me.txtCombinationFactor.Location = New System.Drawing.Point(57, 81)
        Me.txtCombinationFactor.Name = "txtCombinationFactor"
        Me.txtCombinationFactor.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.DecimalUnits
        Me.txtCombinationFactor.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtCombinationFactor.Size = New System.Drawing.Size(133, 20)
        Me.txtCombinationFactor.TabIndex = 4
        Me.txtCombinationFactor.Text = "0.000000"
        Me.txtCombinationFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtCombinationFactor.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblOperationFactor
        '
        Me.lblOperationFactor.AutoSize = True
        Me.lblOperationFactor.Location = New System.Drawing.Point(14, 84)
        Me.lblOperationFactor.Name = "lblOperationFactor"
        Me.lblOperationFactor.Size = New System.Drawing.Size(37, 13)
        Me.lblOperationFactor.TabIndex = 1
        Me.lblOperationFactor.Text = "factor:"
        '
        'rdbMergeOperation_Division
        '
        Me.rdbMergeOperation_Division.AutoSize = True
        Me.rdbMergeOperation_Division.Location = New System.Drawing.Point(104, 41)
        Me.rdbMergeOperation_Division.Name = "rdbMergeOperation_Division"
        Me.rdbMergeOperation_Division.Size = New System.Drawing.Size(60, 17)
        Me.rdbMergeOperation_Division.TabIndex = 2
        Me.rdbMergeOperation_Division.Text = "division"
        Me.rdbMergeOperation_Division.UseVisualStyleBackColor = True
        '
        'rdbMergeOperation_Override
        '
        Me.rdbMergeOperation_Override.AutoSize = True
        Me.rdbMergeOperation_Override.Location = New System.Drawing.Point(61, 61)
        Me.rdbMergeOperation_Override.Name = "rdbMergeOperation_Override"
        Me.rdbMergeOperation_Override.Size = New System.Drawing.Size(63, 17)
        Me.rdbMergeOperation_Override.TabIndex = 3
        Me.rdbMergeOperation_Override.Text = "override"
        Me.rdbMergeOperation_Override.UseVisualStyleBackColor = True
        '
        'rdbMergeOperation_Multiplication
        '
        Me.rdbMergeOperation_Multiplication.AutoSize = True
        Me.rdbMergeOperation_Multiplication.Location = New System.Drawing.Point(14, 41)
        Me.rdbMergeOperation_Multiplication.Name = "rdbMergeOperation_Multiplication"
        Me.rdbMergeOperation_Multiplication.Size = New System.Drawing.Size(85, 17)
        Me.rdbMergeOperation_Multiplication.TabIndex = 1
        Me.rdbMergeOperation_Multiplication.Text = "multiplication"
        Me.rdbMergeOperation_Multiplication.UseVisualStyleBackColor = True
        '
        'rdbMergeOperation_Summation
        '
        Me.rdbMergeOperation_Summation.AutoSize = True
        Me.rdbMergeOperation_Summation.Checked = True
        Me.rdbMergeOperation_Summation.Location = New System.Drawing.Point(14, 21)
        Me.rdbMergeOperation_Summation.Name = "rdbMergeOperation_Summation"
        Me.rdbMergeOperation_Summation.Size = New System.Drawing.Size(132, 17)
        Me.rdbMergeOperation_Summation.TabIndex = 0
        Me.rdbMergeOperation_Summation.TabStop = True
        Me.rdbMergeOperation_Summation.Text = "summation/subtraction"
        Me.rdbMergeOperation_Summation.UseVisualStyleBackColor = True
        '
        'gbOffsetAdjustment
        '
        Me.gbOffsetAdjustment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOffsetAdjustment.Controls.Add(Me.txtAngleOffset)
        Me.gbOffsetAdjustment.Controls.Add(Me.Label2)
        Me.gbOffsetAdjustment.Controls.Add(Me.txtYOffset)
        Me.gbOffsetAdjustment.Controls.Add(Me.Label1)
        Me.gbOffsetAdjustment.Controls.Add(Me.txtXOffset)
        Me.gbOffsetAdjustment.Controls.Add(Me.lblXOffset)
        Me.gbOffsetAdjustment.Location = New System.Drawing.Point(629, 158)
        Me.gbOffsetAdjustment.Name = "gbOffsetAdjustment"
        Me.gbOffsetAdjustment.Size = New System.Drawing.Size(200, 104)
        Me.gbOffsetAdjustment.TabIndex = 7
        Me.gbOffsetAdjustment.TabStop = False
        Me.gbOffsetAdjustment.Text = "offsets:"
        '
        'txtAngleOffset
        '
        Me.txtAngleOffset.AllowZero = True
        Me.txtAngleOffset.BackColor = System.Drawing.Color.White
        Me.txtAngleOffset.ForeColor = System.Drawing.Color.Black
        Me.txtAngleOffset.FormatDecimalPlaces = 2
        Me.txtAngleOffset.Location = New System.Drawing.Point(132, 71)
        Me.txtAngleOffset.Name = "txtAngleOffset"
        Me.txtAngleOffset.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtAngleOffset.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtAngleOffset.Size = New System.Drawing.Size(58, 20)
        Me.txtAngleOffset.TabIndex = 13
        Me.txtAngleOffset.Text = "0.000000"
        Me.txtAngleOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAngleOffset.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "global rotation angle:"
        '
        'txtYOffset
        '
        Me.txtYOffset.AllowZero = True
        Me.txtYOffset.BackColor = System.Drawing.Color.White
        Me.txtYOffset.ForeColor = System.Drawing.Color.Black
        Me.txtYOffset.FormatDecimalPlaces = 6
        Me.txtYOffset.Location = New System.Drawing.Point(73, 45)
        Me.txtYOffset.Name = "txtYOffset"
        Me.txtYOffset.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtYOffset.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtYOffset.Size = New System.Drawing.Size(117, 20)
        Me.txtYOffset.TabIndex = 12
        Me.txtYOffset.Text = "0.000000"
        Me.txtYOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtYOffset.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Y offset:"
        '
        'txtXOffset
        '
        Me.txtXOffset.AllowZero = True
        Me.txtXOffset.BackColor = System.Drawing.Color.White
        Me.txtXOffset.ForeColor = System.Drawing.Color.Black
        Me.txtXOffset.FormatDecimalPlaces = 6
        Me.txtXOffset.Location = New System.Drawing.Point(73, 19)
        Me.txtXOffset.Name = "txtXOffset"
        Me.txtXOffset.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtXOffset.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.PositiveAndNegative
        Me.txtXOffset.Size = New System.Drawing.Size(117, 20)
        Me.txtXOffset.TabIndex = 11
        Me.txtXOffset.Text = "0.000000"
        Me.txtXOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtXOffset.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblXOffset
        '
        Me.lblXOffset.AutoSize = True
        Me.lblXOffset.Location = New System.Drawing.Point(21, 22)
        Me.lblXOffset.Name = "lblXOffset"
        Me.lblXOffset.Size = New System.Drawing.Size(46, 13)
        Me.lblXOffset.TabIndex = 6
        Me.lblXOffset.Text = "X offset:"
        '
        'gbOutput
        '
        Me.gbOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOutput.Controls.Add(Me.scOutput)
        Me.gbOutput.Location = New System.Drawing.Point(3, 3)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Size = New System.Drawing.Size(620, 506)
        Me.gbOutput.TabIndex = 3
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "merged output"
        '
        'scOutput
        '
        Me.scOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scOutput.Location = New System.Drawing.Point(3, 16)
        Me.scOutput.Name = "scOutput"
        Me.scOutput.SelectedPoints_PixelCoordinate = CType(resources.GetObject("scOutput.SelectedPoints_PixelCoordinate"), System.Collections.Generic.List(Of System.Drawing.Point))
        Me.scOutput.SelectionMode = SpectroscopyManager.mScanImageViewer.SelectionModes.None
        Me.scOutput.Size = New System.Drawing.Size(614, 487)
        Me.scOutput.TabIndex = 1
        '
        'wScanImageSummation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(832, 911)
        Me.Controls.Add(Me.scSourceOutput)
        Me.Name = "wScanImageSummation"
        Me.Controls.SetChildIndex(Me.scSourceOutput, 0)
        Me.gbSource1.ResumeLayout(False)
        Me.scSourceData.Panel1.ResumeLayout(False)
        Me.scSourceData.Panel2.ResumeLayout(False)
        CType(Me.scSourceData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scSourceData.ResumeLayout(False)
        Me.gbSource2.ResumeLayout(False)
        Me.scSourceOutput.Panel1.ResumeLayout(False)
        Me.scSourceOutput.Panel2.ResumeLayout(False)
        CType(Me.scSourceOutput, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scSourceOutput.ResumeLayout(False)
        Me.gbMergeOperation.ResumeLayout(False)
        Me.gbMergeOperation.PerformLayout()
        Me.gbOffsetAdjustment.ResumeLayout(False)
        Me.gbOffsetAdjustment.PerformLayout()
        Me.gbOutput.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents scSource1 As mScanImageViewer
    Friend WithEvents gbSource1 As GroupBox
    Friend WithEvents scSourceData As SplitContainer
    Friend WithEvents gbSource2 As GroupBox
    Friend WithEvents scSource2 As mScanImageViewer
    Friend WithEvents scSourceOutput As SplitContainer
    Friend WithEvents gbOutput As GroupBox
    Friend WithEvents scOutput As mScanImageViewer
    Friend WithEvents gbOffsetAdjustment As GroupBox
    Friend WithEvents txtYOffset As NumericTextbox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtXOffset As NumericTextbox
    Friend WithEvents lblXOffset As Label
    Friend WithEvents gbMergeOperation As GroupBox
    Friend WithEvents txtCombinationFactor As NumericTextbox
    Friend WithEvents lblOperationFactor As Label
    Friend WithEvents rdbMergeOperation_Multiplication As RadioButton
    Friend WithEvents rdbMergeOperation_Summation As RadioButton
    Friend WithEvents rdbMergeOperation_Division As RadioButton
    Friend WithEvents rdbMergeOperation_Override As RadioButton
    Friend WithEvents btnExchange1and2 As Button
    Friend WithEvents txtAngleOffset As NumericTextbox
    Friend WithEvents Label2 As Label
End Class
