<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wScanImageLineProfile
    Inherits wFormBase

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
        Me.lblPositionX_Header1 = New System.Windows.Forms.Label()
        Me.gbPoint1 = New System.Windows.Forms.GroupBox()
        Me.lblPositionZ_Value1 = New System.Windows.Forms.Label()
        Me.lblPositionZ_Header1 = New System.Windows.Forms.Label()
        Me.lblPositionY_Value1 = New System.Windows.Forms.Label()
        Me.lblPositionY_Header1 = New System.Windows.Forms.Label()
        Me.lblPositionX_Value1 = New System.Windows.Forms.Label()
        Me.gbPoint2 = New System.Windows.Forms.GroupBox()
        Me.lblPositionZ_Value2 = New System.Windows.Forms.Label()
        Me.lblPositionZ_Header2 = New System.Windows.Forms.Label()
        Me.lblPositionY_Value2 = New System.Windows.Forms.Label()
        Me.lblPositionY_Header2 = New System.Windows.Forms.Label()
        Me.lblPositionX_Value2 = New System.Windows.Forms.Label()
        Me.lblPositionX_Header2 = New System.Windows.Forms.Label()
        Me.lblDistanceX_Header = New System.Windows.Forms.Label()
        Me.lblDistanceX_Value = New System.Windows.Forms.Label()
        Me.lblDistanceY_Header = New System.Windows.Forms.Label()
        Me.lblDistanceR_Header = New System.Windows.Forms.Label()
        Me.lblDistanceY_Value = New System.Windows.Forms.Label()
        Me.lblDistanceR_Value = New System.Windows.Forms.Label()
        Me.lblDistanceZ_Header = New System.Windows.Forms.Label()
        Me.lblDistanceZ_Value = New System.Windows.Forms.Label()
        Me.gbDistance = New System.Windows.Forms.GroupBox()
        Me.nudDataPoints = New System.Windows.Forms.NumericUpDown()
        Me.lblDataPoints = New System.Windows.Forms.Label()
        Me.gbSettings = New System.Windows.Forms.GroupBox()
        Me.tcDataContainer = New System.Windows.Forms.TabControl()
        Me.tpPlot = New System.Windows.Forms.TabPage()
        Me.svPlot = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.tpDataTable = New System.Windows.Forms.TabPage()
        Me.dtProfile = New SpectroscopyManager.mSpectroscopyTableDataTable()
        Me.gbPoint1.SuspendLayout()
        Me.gbPoint2.SuspendLayout()
        Me.gbDistance.SuspendLayout()
        CType(Me.nudDataPoints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSettings.SuspendLayout()
        Me.tcDataContainer.SuspendLayout()
        Me.tpPlot.SuspendLayout()
        Me.tpDataTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblPositionX_Header1
        '
        Me.lblPositionX_Header1.AutoSize = True
        Me.lblPositionX_Header1.Location = New System.Drawing.Point(10, 17)
        Me.lblPositionX_Header1.Name = "lblPositionX_Header1"
        Me.lblPositionX_Header1.Size = New System.Drawing.Size(17, 13)
        Me.lblPositionX_Header1.TabIndex = 1
        Me.lblPositionX_Header1.Text = "X:"
        '
        'gbPoint1
        '
        Me.gbPoint1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPoint1.Controls.Add(Me.lblPositionZ_Value1)
        Me.gbPoint1.Controls.Add(Me.lblPositionZ_Header1)
        Me.gbPoint1.Controls.Add(Me.lblPositionY_Value1)
        Me.gbPoint1.Controls.Add(Me.lblPositionY_Header1)
        Me.gbPoint1.Controls.Add(Me.lblPositionX_Value1)
        Me.gbPoint1.Controls.Add(Me.lblPositionX_Header1)
        Me.gbPoint1.Location = New System.Drawing.Point(719, 7)
        Me.gbPoint1.Name = "gbPoint1"
        Me.gbPoint1.Size = New System.Drawing.Size(152, 64)
        Me.gbPoint1.TabIndex = 2
        Me.gbPoint1.TabStop = False
        Me.gbPoint1.Text = "point 1"
        '
        'lblPositionZ_Value1
        '
        Me.lblPositionZ_Value1.AutoSize = True
        Me.lblPositionZ_Value1.Location = New System.Drawing.Point(33, 43)
        Me.lblPositionZ_Value1.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblPositionZ_Value1.Name = "lblPositionZ_Value1"
        Me.lblPositionZ_Value1.Size = New System.Drawing.Size(50, 13)
        Me.lblPositionZ_Value1.TabIndex = 1
        Me.lblPositionZ_Value1.Text = "value"
        Me.lblPositionZ_Value1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPositionZ_Header1
        '
        Me.lblPositionZ_Header1.AutoSize = True
        Me.lblPositionZ_Header1.Location = New System.Drawing.Point(10, 43)
        Me.lblPositionZ_Header1.Name = "lblPositionZ_Header1"
        Me.lblPositionZ_Header1.Size = New System.Drawing.Size(17, 13)
        Me.lblPositionZ_Header1.TabIndex = 1
        Me.lblPositionZ_Header1.Text = "Z:"
        '
        'lblPositionY_Value1
        '
        Me.lblPositionY_Value1.AutoSize = True
        Me.lblPositionY_Value1.Location = New System.Drawing.Point(33, 30)
        Me.lblPositionY_Value1.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblPositionY_Value1.Name = "lblPositionY_Value1"
        Me.lblPositionY_Value1.Size = New System.Drawing.Size(50, 13)
        Me.lblPositionY_Value1.TabIndex = 1
        Me.lblPositionY_Value1.Text = "value"
        Me.lblPositionY_Value1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPositionY_Header1
        '
        Me.lblPositionY_Header1.AutoSize = True
        Me.lblPositionY_Header1.Location = New System.Drawing.Point(10, 30)
        Me.lblPositionY_Header1.Name = "lblPositionY_Header1"
        Me.lblPositionY_Header1.Size = New System.Drawing.Size(17, 13)
        Me.lblPositionY_Header1.TabIndex = 1
        Me.lblPositionY_Header1.Text = "Y:"
        '
        'lblPositionX_Value1
        '
        Me.lblPositionX_Value1.AutoSize = True
        Me.lblPositionX_Value1.Location = New System.Drawing.Point(33, 17)
        Me.lblPositionX_Value1.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblPositionX_Value1.Name = "lblPositionX_Value1"
        Me.lblPositionX_Value1.Size = New System.Drawing.Size(50, 13)
        Me.lblPositionX_Value1.TabIndex = 1
        Me.lblPositionX_Value1.Text = "value"
        Me.lblPositionX_Value1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'gbPoint2
        '
        Me.gbPoint2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPoint2.Controls.Add(Me.lblPositionZ_Value2)
        Me.gbPoint2.Controls.Add(Me.lblPositionZ_Header2)
        Me.gbPoint2.Controls.Add(Me.lblPositionY_Value2)
        Me.gbPoint2.Controls.Add(Me.lblPositionY_Header2)
        Me.gbPoint2.Controls.Add(Me.lblPositionX_Value2)
        Me.gbPoint2.Controls.Add(Me.lblPositionX_Header2)
        Me.gbPoint2.Location = New System.Drawing.Point(719, 77)
        Me.gbPoint2.Name = "gbPoint2"
        Me.gbPoint2.Size = New System.Drawing.Size(152, 64)
        Me.gbPoint2.TabIndex = 2
        Me.gbPoint2.TabStop = False
        Me.gbPoint2.Text = "point 2"
        '
        'lblPositionZ_Value2
        '
        Me.lblPositionZ_Value2.AutoSize = True
        Me.lblPositionZ_Value2.Location = New System.Drawing.Point(33, 43)
        Me.lblPositionZ_Value2.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblPositionZ_Value2.Name = "lblPositionZ_Value2"
        Me.lblPositionZ_Value2.Size = New System.Drawing.Size(50, 13)
        Me.lblPositionZ_Value2.TabIndex = 1
        Me.lblPositionZ_Value2.Text = "value"
        Me.lblPositionZ_Value2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPositionZ_Header2
        '
        Me.lblPositionZ_Header2.AutoSize = True
        Me.lblPositionZ_Header2.Location = New System.Drawing.Point(10, 43)
        Me.lblPositionZ_Header2.Name = "lblPositionZ_Header2"
        Me.lblPositionZ_Header2.Size = New System.Drawing.Size(17, 13)
        Me.lblPositionZ_Header2.TabIndex = 1
        Me.lblPositionZ_Header2.Text = "Z:"
        '
        'lblPositionY_Value2
        '
        Me.lblPositionY_Value2.AutoSize = True
        Me.lblPositionY_Value2.Location = New System.Drawing.Point(33, 30)
        Me.lblPositionY_Value2.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblPositionY_Value2.Name = "lblPositionY_Value2"
        Me.lblPositionY_Value2.Size = New System.Drawing.Size(50, 13)
        Me.lblPositionY_Value2.TabIndex = 1
        Me.lblPositionY_Value2.Text = "value"
        Me.lblPositionY_Value2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPositionY_Header2
        '
        Me.lblPositionY_Header2.AutoSize = True
        Me.lblPositionY_Header2.Location = New System.Drawing.Point(10, 30)
        Me.lblPositionY_Header2.Name = "lblPositionY_Header2"
        Me.lblPositionY_Header2.Size = New System.Drawing.Size(17, 13)
        Me.lblPositionY_Header2.TabIndex = 1
        Me.lblPositionY_Header2.Text = "Y:"
        '
        'lblPositionX_Value2
        '
        Me.lblPositionX_Value2.AutoSize = True
        Me.lblPositionX_Value2.Location = New System.Drawing.Point(33, 17)
        Me.lblPositionX_Value2.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblPositionX_Value2.Name = "lblPositionX_Value2"
        Me.lblPositionX_Value2.Size = New System.Drawing.Size(50, 13)
        Me.lblPositionX_Value2.TabIndex = 1
        Me.lblPositionX_Value2.Text = "value"
        Me.lblPositionX_Value2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPositionX_Header2
        '
        Me.lblPositionX_Header2.AutoSize = True
        Me.lblPositionX_Header2.Location = New System.Drawing.Point(10, 17)
        Me.lblPositionX_Header2.Name = "lblPositionX_Header2"
        Me.lblPositionX_Header2.Size = New System.Drawing.Size(17, 13)
        Me.lblPositionX_Header2.TabIndex = 1
        Me.lblPositionX_Header2.Text = "X:"
        '
        'lblDistanceX_Header
        '
        Me.lblDistanceX_Header.AutoSize = True
        Me.lblDistanceX_Header.Location = New System.Drawing.Point(10, 20)
        Me.lblDistanceX_Header.Name = "lblDistanceX_Header"
        Me.lblDistanceX_Header.Size = New System.Drawing.Size(24, 13)
        Me.lblDistanceX_Header.TabIndex = 1
        Me.lblDistanceX_Header.Text = "∆X:"
        '
        'lblDistanceX_Value
        '
        Me.lblDistanceX_Value.AutoSize = True
        Me.lblDistanceX_Value.Location = New System.Drawing.Point(33, 20)
        Me.lblDistanceX_Value.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblDistanceX_Value.Name = "lblDistanceX_Value"
        Me.lblDistanceX_Value.Size = New System.Drawing.Size(50, 13)
        Me.lblDistanceX_Value.TabIndex = 1
        Me.lblDistanceX_Value.Text = "value"
        Me.lblDistanceX_Value.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDistanceY_Header
        '
        Me.lblDistanceY_Header.AutoSize = True
        Me.lblDistanceY_Header.Location = New System.Drawing.Point(10, 33)
        Me.lblDistanceY_Header.Name = "lblDistanceY_Header"
        Me.lblDistanceY_Header.Size = New System.Drawing.Size(24, 13)
        Me.lblDistanceY_Header.TabIndex = 1
        Me.lblDistanceY_Header.Text = "∆Y:"
        '
        'lblDistanceR_Header
        '
        Me.lblDistanceR_Header.AutoSize = True
        Me.lblDistanceR_Header.Location = New System.Drawing.Point(10, 46)
        Me.lblDistanceR_Header.Name = "lblDistanceR_Header"
        Me.lblDistanceR_Header.Size = New System.Drawing.Size(25, 13)
        Me.lblDistanceR_Header.TabIndex = 1
        Me.lblDistanceR_Header.Text = "∆R:"
        '
        'lblDistanceY_Value
        '
        Me.lblDistanceY_Value.AutoSize = True
        Me.lblDistanceY_Value.Location = New System.Drawing.Point(33, 33)
        Me.lblDistanceY_Value.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblDistanceY_Value.Name = "lblDistanceY_Value"
        Me.lblDistanceY_Value.Size = New System.Drawing.Size(50, 13)
        Me.lblDistanceY_Value.TabIndex = 1
        Me.lblDistanceY_Value.Text = "value"
        Me.lblDistanceY_Value.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDistanceR_Value
        '
        Me.lblDistanceR_Value.AutoSize = True
        Me.lblDistanceR_Value.Location = New System.Drawing.Point(33, 46)
        Me.lblDistanceR_Value.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblDistanceR_Value.Name = "lblDistanceR_Value"
        Me.lblDistanceR_Value.Size = New System.Drawing.Size(50, 13)
        Me.lblDistanceR_Value.TabIndex = 1
        Me.lblDistanceR_Value.Text = "value"
        Me.lblDistanceR_Value.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDistanceZ_Header
        '
        Me.lblDistanceZ_Header.AutoSize = True
        Me.lblDistanceZ_Header.Location = New System.Drawing.Point(10, 59)
        Me.lblDistanceZ_Header.Name = "lblDistanceZ_Header"
        Me.lblDistanceZ_Header.Size = New System.Drawing.Size(24, 13)
        Me.lblDistanceZ_Header.TabIndex = 1
        Me.lblDistanceZ_Header.Text = "∆Z:"
        '
        'lblDistanceZ_Value
        '
        Me.lblDistanceZ_Value.AutoSize = True
        Me.lblDistanceZ_Value.Location = New System.Drawing.Point(33, 59)
        Me.lblDistanceZ_Value.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblDistanceZ_Value.Name = "lblDistanceZ_Value"
        Me.lblDistanceZ_Value.Size = New System.Drawing.Size(50, 13)
        Me.lblDistanceZ_Value.TabIndex = 1
        Me.lblDistanceZ_Value.Text = "value"
        Me.lblDistanceZ_Value.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'gbDistance
        '
        Me.gbDistance.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDistance.Controls.Add(Me.lblDistanceY_Header)
        Me.gbDistance.Controls.Add(Me.lblDistanceX_Header)
        Me.gbDistance.Controls.Add(Me.lblDistanceR_Header)
        Me.gbDistance.Controls.Add(Me.lblDistanceX_Value)
        Me.gbDistance.Controls.Add(Me.lblDistanceZ_Value)
        Me.gbDistance.Controls.Add(Me.lblDistanceZ_Header)
        Me.gbDistance.Controls.Add(Me.lblDistanceR_Value)
        Me.gbDistance.Controls.Add(Me.lblDistanceY_Value)
        Me.gbDistance.Location = New System.Drawing.Point(719, 148)
        Me.gbDistance.Name = "gbDistance"
        Me.gbDistance.Size = New System.Drawing.Size(152, 82)
        Me.gbDistance.TabIndex = 3
        Me.gbDistance.TabStop = False
        Me.gbDistance.Text = "distances"
        '
        'nudDataPoints
        '
        Me.nudDataPoints.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.SpectroscopyManager.My.MySettings.Default, "LineProfilePlot_DataPointNumber", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.nudDataPoints.Location = New System.Drawing.Point(86, 21)
        Me.nudDataPoints.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudDataPoints.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudDataPoints.Name = "nudDataPoints"
        Me.nudDataPoints.Size = New System.Drawing.Size(57, 20)
        Me.nudDataPoints.TabIndex = 4
        Me.nudDataPoints.Value = Global.SpectroscopyManager.My.MySettings.Default.LineProfilePlot_DataPointNumber
        '
        'lblDataPoints
        '
        Me.lblDataPoints.AutoSize = True
        Me.lblDataPoints.Location = New System.Drawing.Point(10, 24)
        Me.lblDataPoints.Name = "lblDataPoints"
        Me.lblDataPoints.Size = New System.Drawing.Size(70, 13)
        Me.lblDataPoints.TabIndex = 5
        Me.lblDataPoints.Text = "No. of points:"
        '
        'gbSettings
        '
        Me.gbSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSettings.Controls.Add(Me.nudDataPoints)
        Me.gbSettings.Controls.Add(Me.lblDataPoints)
        Me.gbSettings.Location = New System.Drawing.Point(719, 251)
        Me.gbSettings.Name = "gbSettings"
        Me.gbSettings.Size = New System.Drawing.Size(152, 54)
        Me.gbSettings.TabIndex = 6
        Me.gbSettings.TabStop = False
        Me.gbSettings.Text = "settings"
        '
        'tcDataContainer
        '
        Me.tcDataContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcDataContainer.Controls.Add(Me.tpPlot)
        Me.tcDataContainer.Controls.Add(Me.tpDataTable)
        Me.tcDataContainer.Location = New System.Drawing.Point(2, 0)
        Me.tcDataContainer.Name = "tcDataContainer"
        Me.tcDataContainer.SelectedIndex = 0
        Me.tcDataContainer.Size = New System.Drawing.Size(711, 502)
        Me.tcDataContainer.TabIndex = 7
        '
        'tpPlot
        '
        Me.tpPlot.Controls.Add(Me.svPlot)
        Me.tpPlot.Location = New System.Drawing.Point(4, 22)
        Me.tpPlot.Name = "tpPlot"
        Me.tpPlot.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPlot.Size = New System.Drawing.Size(703, 476)
        Me.tpPlot.TabIndex = 0
        Me.tpPlot.Text = "profile plot"
        Me.tpPlot.UseVisualStyleBackColor = True
        '
        'svPlot
        '
        Me.svPlot.AllowAdjustingXColumn = True
        Me.svPlot.AllowAdjustingYColumn = True
        Me.svPlot.AutomaticallyRestoreScaleAfterRedraw = True
        Me.svPlot.CallbackDataPointSelected = Nothing
        Me.svPlot.CallbackXRangeSelected = Nothing
        Me.svPlot.CallbackXValueSelected = Nothing
        Me.svPlot.CallbackXYRangeSelected = Nothing
        Me.svPlot.CallbackXYValueSelected = Nothing
        Me.svPlot.CallbackYRangeSelected = Nothing
        Me.svPlot.CallbackYValueSelected = Nothing
        Me.svPlot.ClearPointSelectionModeAfterSelection = False
        Me.svPlot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.svPlot.Location = New System.Drawing.Point(3, 3)
        Me.svPlot.MultipleSpectraStackOffset = 0R
        Me.svPlot.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.svPlot.Name = "svPlot"
        Me.svPlot.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.svPlot.ShowColumnSelectors = False
        Me.svPlot.Size = New System.Drawing.Size(697, 470)
        Me.svPlot.TabIndex = 0
        Me.svPlot.TurnOnLastFilterSaving_Y = False
        Me.svPlot.TurnOnLastSelectionSaving_Y = False
        '
        'tpDataTable
        '
        Me.tpDataTable.Controls.Add(Me.dtProfile)
        Me.tpDataTable.Location = New System.Drawing.Point(4, 22)
        Me.tpDataTable.Name = "tpDataTable"
        Me.tpDataTable.Padding = New System.Windows.Forms.Padding(3)
        Me.tpDataTable.Size = New System.Drawing.Size(703, 476)
        Me.tpDataTable.TabIndex = 1
        Me.tpDataTable.Text = "data table"
        Me.tpDataTable.UseVisualStyleBackColor = True
        '
        'dtProfile
        '
        Me.dtProfile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtProfile.Location = New System.Drawing.Point(3, 3)
        Me.dtProfile.Name = "dtProfile"
        Me.dtProfile.Size = New System.Drawing.Size(697, 470)
        Me.dtProfile.TabIndex = 1
        '
        'wScanImageLineProfile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(874, 501)
        Me.Controls.Add(Me.tcDataContainer)
        Me.Controls.Add(Me.gbSettings)
        Me.Controls.Add(Me.gbDistance)
        Me.Controls.Add(Me.gbPoint2)
        Me.Controls.Add(Me.gbPoint1)
        Me.Name = "wScanImageLineProfile"
        Me.Text = "Line Profile of "
        Me.gbPoint1.ResumeLayout(False)
        Me.gbPoint1.PerformLayout()
        Me.gbPoint2.ResumeLayout(False)
        Me.gbPoint2.PerformLayout()
        Me.gbDistance.ResumeLayout(False)
        Me.gbDistance.PerformLayout()
        CType(Me.nudDataPoints, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSettings.ResumeLayout(False)
        Me.gbSettings.PerformLayout()
        Me.tcDataContainer.ResumeLayout(False)
        Me.tpPlot.ResumeLayout(False)
        Me.tpDataTable.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblPositionX_Header1 As Label
    Friend WithEvents gbPoint1 As GroupBox
    Friend WithEvents lblPositionZ_Value1 As Label
    Friend WithEvents lblPositionZ_Header1 As Label
    Friend WithEvents lblPositionY_Value1 As Label
    Friend WithEvents lblPositionY_Header1 As Label
    Friend WithEvents lblPositionX_Value1 As Label
    Friend WithEvents gbPoint2 As GroupBox
    Friend WithEvents lblPositionZ_Value2 As Label
    Friend WithEvents lblPositionZ_Header2 As Label
    Friend WithEvents lblPositionY_Value2 As Label
    Friend WithEvents lblPositionY_Header2 As Label
    Friend WithEvents lblPositionX_Value2 As Label
    Friend WithEvents lblPositionX_Header2 As Label
    Friend WithEvents lblDistanceX_Header As Label
    Friend WithEvents lblDistanceX_Value As Label
    Friend WithEvents lblDistanceY_Header As Label
    Friend WithEvents lblDistanceR_Header As Label
    Friend WithEvents lblDistanceY_Value As Label
    Friend WithEvents lblDistanceR_Value As Label
    Friend WithEvents lblDistanceZ_Header As Label
    Friend WithEvents lblDistanceZ_Value As Label
    Friend WithEvents gbDistance As GroupBox
    Friend WithEvents nudDataPoints As NumericUpDown
    Friend WithEvents lblDataPoints As Label
    Friend WithEvents gbSettings As GroupBox
    Friend WithEvents tcDataContainer As TabControl
    Friend WithEvents tpPlot As TabPage
    Friend WithEvents tpDataTable As TabPage
    Friend WithEvents dtProfile As mSpectroscopyTableDataTable
    Friend WithEvents svPlot As mSpectroscopyTableViewer
End Class
