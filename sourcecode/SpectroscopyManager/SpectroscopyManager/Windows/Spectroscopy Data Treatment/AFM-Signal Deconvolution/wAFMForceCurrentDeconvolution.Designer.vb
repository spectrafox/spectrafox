<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class wAFMForceCurrentDeconvolution
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wAFMForceCurrentDeconvolution))
        Me.gbSettings = New System.Windows.Forms.GroupBox()
        Me.lblUnitSpringConst = New System.Windows.Forms.Label()
        Me.lblUnitOscAmp = New System.Windows.Forms.Label()
        Me.lblUnitResFreq = New System.Windows.Forms.Label()
        Me.txtAFMSetupSpringConst = New SpectroscopyManager.NumericTextbox()
        Me.lblSpringConstant = New System.Windows.Forms.Label()
        Me.txtAFMSetupOscAmp = New SpectroscopyManager.NumericTextbox()
        Me.lblOscillationAmplitude = New System.Windows.Forms.Label()
        Me.txtAFMSetupResFreq = New SpectroscopyManager.NumericTextbox()
        Me.lblResonanceFrequency = New System.Windows.Forms.Label()
        Me.lblToolDescription = New System.Windows.Forms.Label()
        Me.gbDataSelection = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblColZRel = New System.Windows.Forms.Label()
        Me.lblColForceGradient = New System.Windows.Forms.Label()
        Me.csColumn_CurrentSignal = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.csColumn_ZRel = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.csColumn_ForceGradient = New SpectroscopyManager.ucSpectroscopyColumnSelector()
        Me.gbBefore = New System.Windows.Forms.GroupBox()
        Me.pbBefore = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.btnDoForceDeconvolution = New System.Windows.Forms.Button()
        Me.gbAfter = New System.Windows.Forms.GroupBox()
        Me.pbAfter = New SpectroscopyManager.mSpectroscopyTableViewer()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.btnSaveColumn = New System.Windows.Forms.Button()
        Me.dsDataSmoother = New SpectroscopyManager.mDataSmoothing()
        Me.gbSmoothingSettings = New System.Windows.Forms.GroupBox()
        Me.lblSmoothingDescription = New System.Windows.Forms.Label()
        Me.gbOutput = New System.Windows.Forms.GroupBox()
        Me.txtOutputName_Current = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtOutputName_Force = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.scPreview = New System.Windows.Forms.SplitContainer()
        Me.gbAdditionalSettings = New System.Windows.Forms.GroupBox()
        Me.ckbSettings_RemoveFrequencyShiftOffset = New System.Windows.Forms.CheckBox()
        Me.gbSettings.SuspendLayout()
        Me.gbDataSelection.SuspendLayout()
        Me.gbBefore.SuspendLayout()
        Me.gbAfter.SuspendLayout()
        Me.gbSmoothingSettings.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        CType(Me.scPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scPreview.Panel1.SuspendLayout()
        Me.scPreview.Panel2.SuspendLayout()
        Me.scPreview.SuspendLayout()
        Me.gbAdditionalSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbSettings
        '
        Me.gbSettings.Controls.Add(Me.lblUnitSpringConst)
        Me.gbSettings.Controls.Add(Me.lblUnitOscAmp)
        Me.gbSettings.Controls.Add(Me.lblUnitResFreq)
        Me.gbSettings.Controls.Add(Me.txtAFMSetupSpringConst)
        Me.gbSettings.Controls.Add(Me.lblSpringConstant)
        Me.gbSettings.Controls.Add(Me.txtAFMSetupOscAmp)
        Me.gbSettings.Controls.Add(Me.lblOscillationAmplitude)
        Me.gbSettings.Controls.Add(Me.txtAFMSetupResFreq)
        Me.gbSettings.Controls.Add(Me.lblResonanceFrequency)
        Me.gbSettings.Location = New System.Drawing.Point(478, 2)
        Me.gbSettings.Name = "gbSettings"
        Me.gbSettings.Size = New System.Drawing.Size(276, 104)
        Me.gbSettings.TabIndex = 1
        Me.gbSettings.TabStop = False
        Me.gbSettings.Text = "oscillator settings:"
        '
        'lblUnitSpringConst
        '
        Me.lblUnitSpringConst.AutoSize = True
        Me.lblUnitSpringConst.Location = New System.Drawing.Point(236, 74)
        Me.lblUnitSpringConst.Name = "lblUnitSpringConst"
        Me.lblUnitSpringConst.Size = New System.Drawing.Size(28, 13)
        Me.lblUnitSpringConst.TabIndex = 2
        Me.lblUnitSpringConst.Text = "N/m"
        '
        'lblUnitOscAmp
        '
        Me.lblUnitOscAmp.AutoSize = True
        Me.lblUnitOscAmp.Location = New System.Drawing.Point(236, 48)
        Me.lblUnitOscAmp.Name = "lblUnitOscAmp"
        Me.lblUnitOscAmp.Size = New System.Drawing.Size(15, 13)
        Me.lblUnitOscAmp.TabIndex = 2
        Me.lblUnitOscAmp.Text = "m"
        '
        'lblUnitResFreq
        '
        Me.lblUnitResFreq.AutoSize = True
        Me.lblUnitResFreq.Location = New System.Drawing.Point(236, 22)
        Me.lblUnitResFreq.Name = "lblUnitResFreq"
        Me.lblUnitResFreq.Size = New System.Drawing.Size(20, 13)
        Me.lblUnitResFreq.TabIndex = 2
        Me.lblUnitResFreq.Text = "Hz"
        '
        'txtAFMSetupSpringConst
        '
        Me.txtAFMSetupSpringConst.AllowZero = True
        Me.txtAFMSetupSpringConst.BackColor = System.Drawing.Color.Green
        Me.txtAFMSetupSpringConst.ForeColor = System.Drawing.Color.White
        Me.txtAFMSetupSpringConst.FormatDecimalPlaces = 6
        Me.txtAFMSetupSpringConst.Location = New System.Drawing.Point(133, 71)
        Me.txtAFMSetupSpringConst.Name = "txtAFMSetupSpringConst"
        Me.txtAFMSetupSpringConst.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtAFMSetupSpringConst.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtAFMSetupSpringConst.Size = New System.Drawing.Size(100, 20)
        Me.txtAFMSetupSpringConst.TabIndex = 1
        Me.txtAFMSetupSpringConst.Text = "0.000000"
        Me.txtAFMSetupSpringConst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAFMSetupSpringConst.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblSpringConstant
        '
        Me.lblSpringConstant.AutoSize = True
        Me.lblSpringConstant.Location = New System.Drawing.Point(17, 74)
        Me.lblSpringConstant.Name = "lblSpringConstant"
        Me.lblSpringConstant.Size = New System.Drawing.Size(82, 13)
        Me.lblSpringConstant.TabIndex = 0
        Me.lblSpringConstant.Text = "spring constant:"
        '
        'txtAFMSetupOscAmp
        '
        Me.txtAFMSetupOscAmp.AllowZero = True
        Me.txtAFMSetupOscAmp.BackColor = System.Drawing.Color.White
        Me.txtAFMSetupOscAmp.ForeColor = System.Drawing.Color.Black
        Me.txtAFMSetupOscAmp.FormatDecimalPlaces = 6
        Me.txtAFMSetupOscAmp.Location = New System.Drawing.Point(133, 45)
        Me.txtAFMSetupOscAmp.Name = "txtAFMSetupOscAmp"
        Me.txtAFMSetupOscAmp.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtAFMSetupOscAmp.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtAFMSetupOscAmp.Size = New System.Drawing.Size(100, 20)
        Me.txtAFMSetupOscAmp.TabIndex = 1
        Me.txtAFMSetupOscAmp.Text = "0.000000"
        Me.txtAFMSetupOscAmp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAFMSetupOscAmp.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblOscillationAmplitude
        '
        Me.lblOscillationAmplitude.AutoSize = True
        Me.lblOscillationAmplitude.Location = New System.Drawing.Point(17, 48)
        Me.lblOscillationAmplitude.Name = "lblOscillationAmplitude"
        Me.lblOscillationAmplitude.Size = New System.Drawing.Size(104, 13)
        Me.lblOscillationAmplitude.TabIndex = 0
        Me.lblOscillationAmplitude.Text = "oscillation amplitude:"
        '
        'txtAFMSetupResFreq
        '
        Me.txtAFMSetupResFreq.AllowZero = True
        Me.txtAFMSetupResFreq.BackColor = System.Drawing.Color.White
        Me.txtAFMSetupResFreq.ForeColor = System.Drawing.Color.Black
        Me.txtAFMSetupResFreq.FormatDecimalPlaces = 6
        Me.txtAFMSetupResFreq.Location = New System.Drawing.Point(133, 19)
        Me.txtAFMSetupResFreq.Name = "txtAFMSetupResFreq"
        Me.txtAFMSetupResFreq.NumberFormat = SpectroscopyManager.NumericTextbox.NumberFormatTypes.ScientificUnits
        Me.txtAFMSetupResFreq.NumberRange = SpectroscopyManager.NumericTextbox.NumberRanges.Positive
        Me.txtAFMSetupResFreq.Size = New System.Drawing.Size(100, 20)
        Me.txtAFMSetupResFreq.TabIndex = 1
        Me.txtAFMSetupResFreq.Text = "0.000000"
        Me.txtAFMSetupResFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAFMSetupResFreq.ValueType = SpectroscopyManager.NumericTextbox.ValueTypes.FloatingPointValue
        '
        'lblResonanceFrequency
        '
        Me.lblResonanceFrequency.AutoSize = True
        Me.lblResonanceFrequency.Location = New System.Drawing.Point(17, 22)
        Me.lblResonanceFrequency.Name = "lblResonanceFrequency"
        Me.lblResonanceFrequency.Size = New System.Drawing.Size(110, 13)
        Me.lblResonanceFrequency.TabIndex = 0
        Me.lblResonanceFrequency.Text = "resonance frequency:"
        '
        'lblToolDescription
        '
        Me.lblToolDescription.AutoSize = True
        Me.lblToolDescription.Location = New System.Drawing.Point(12, 13)
        Me.lblToolDescription.Name = "lblToolDescription"
        Me.lblToolDescription.Size = New System.Drawing.Size(458, 78)
        Me.lblToolDescription.TabIndex = 2
        Me.lblToolDescription.Text = resources.GetString("lblToolDescription.Text")
        '
        'gbDataSelection
        '
        Me.gbDataSelection.Controls.Add(Me.Label1)
        Me.gbDataSelection.Controls.Add(Me.lblColZRel)
        Me.gbDataSelection.Controls.Add(Me.lblColForceGradient)
        Me.gbDataSelection.Controls.Add(Me.csColumn_CurrentSignal)
        Me.gbDataSelection.Controls.Add(Me.csColumn_ZRel)
        Me.gbDataSelection.Controls.Add(Me.csColumn_ForceGradient)
        Me.gbDataSelection.Location = New System.Drawing.Point(760, 2)
        Me.gbDataSelection.Name = "gbDataSelection"
        Me.gbDataSelection.Size = New System.Drawing.Size(328, 104)
        Me.gbDataSelection.TabIndex = 3
        Me.gbDataSelection.TabStop = False
        Me.gbDataSelection.Text = "data selection:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 75)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "average current column:"
        '
        'lblColZRel
        '
        Me.lblColZRel.AutoSize = True
        Me.lblColZRel.Location = New System.Drawing.Point(17, 22)
        Me.lblColZRel.Name = "lblColZRel"
        Me.lblColZRel.Size = New System.Drawing.Size(107, 13)
        Me.lblColZRel.TabIndex = 4
        Me.lblColZRel.Text = "Z coordinate column:"
        '
        'lblColForceGradient
        '
        Me.lblColForceGradient.AutoSize = True
        Me.lblColForceGradient.Location = New System.Drawing.Point(17, 48)
        Me.lblColForceGradient.Name = "lblColForceGradient"
        Me.lblColForceGradient.Size = New System.Drawing.Size(112, 13)
        Me.lblColForceGradient.TabIndex = 4
        Me.lblColForceGradient.Text = "force gradient column:"
        '
        'csColumn_CurrentSignal
        '
        Me.csColumn_CurrentSignal.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumn_CurrentSignal.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumn_CurrentSignal.Location = New System.Drawing.Point(145, 71)
        Me.csColumn_CurrentSignal.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumn_CurrentSignal.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumn_CurrentSignal.Name = "csColumn_CurrentSignal"
        Me.csColumn_CurrentSignal.SelectedColumnName = ""
        Me.csColumn_CurrentSignal.SelectedColumnNames = CType(resources.GetObject("csColumn_CurrentSignal.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumn_CurrentSignal.SelectedEntries = CType(resources.GetObject("csColumn_CurrentSignal.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumn_CurrentSignal.SelectedEntry = ""
        Me.csColumn_CurrentSignal.Size = New System.Drawing.Size(164, 21)
        Me.csColumn_CurrentSignal.TabIndex = 3
        Me.csColumn_CurrentSignal.TurnOnLastFilterSaving = False
        Me.csColumn_CurrentSignal.TurnOnLastSelectionSaving = False
        '
        'csColumn_ZRel
        '
        Me.csColumn_ZRel.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumn_ZRel.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumn_ZRel.Location = New System.Drawing.Point(145, 18)
        Me.csColumn_ZRel.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumn_ZRel.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumn_ZRel.Name = "csColumn_ZRel"
        Me.csColumn_ZRel.SelectedColumnName = ""
        Me.csColumn_ZRel.SelectedColumnNames = CType(resources.GetObject("csColumn_ZRel.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumn_ZRel.SelectedEntries = CType(resources.GetObject("csColumn_ZRel.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumn_ZRel.SelectedEntry = ""
        Me.csColumn_ZRel.Size = New System.Drawing.Size(164, 21)
        Me.csColumn_ZRel.TabIndex = 3
        Me.csColumn_ZRel.TurnOnLastFilterSaving = False
        Me.csColumn_ZRel.TurnOnLastSelectionSaving = False
        '
        'csColumn_ForceGradient
        '
        Me.csColumn_ForceGradient.AppereanceType = SpectroscopyManager.ucFilteredListComboBoxSelector.SelectorType.Combobox
        Me.csColumn_ForceGradient.IfAppearanceListBox_MultiSelectionMode = System.Windows.Forms.SelectionMode.One
        Me.csColumn_ForceGradient.Location = New System.Drawing.Point(145, 44)
        Me.csColumn_ForceGradient.MaximumSize = New System.Drawing.Size(10000, 21)
        Me.csColumn_ForceGradient.MinimumSize = New System.Drawing.Size(0, 21)
        Me.csColumn_ForceGradient.Name = "csColumn_ForceGradient"
        Me.csColumn_ForceGradient.SelectedColumnName = ""
        Me.csColumn_ForceGradient.SelectedColumnNames = CType(resources.GetObject("csColumn_ForceGradient.SelectedColumnNames"), System.Collections.Generic.List(Of String))
        Me.csColumn_ForceGradient.SelectedEntries = CType(resources.GetObject("csColumn_ForceGradient.SelectedEntries"), System.Collections.Generic.List(Of String))
        Me.csColumn_ForceGradient.SelectedEntry = ""
        Me.csColumn_ForceGradient.Size = New System.Drawing.Size(164, 21)
        Me.csColumn_ForceGradient.TabIndex = 3
        Me.csColumn_ForceGradient.TurnOnLastFilterSaving = False
        Me.csColumn_ForceGradient.TurnOnLastSelectionSaving = False
        '
        'gbBefore
        '
        Me.gbBefore.Controls.Add(Me.pbBefore)
        Me.gbBefore.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbBefore.Location = New System.Drawing.Point(0, 0)
        Me.gbBefore.Name = "gbBefore"
        Me.gbBefore.Size = New System.Drawing.Size(574, 428)
        Me.gbBefore.TabIndex = 13
        Me.gbBefore.TabStop = False
        Me.gbBefore.Text = "before deconvolution:"
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
        Me.pbBefore.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbBefore.ShowColumnSelectors = True
        Me.pbBefore.Size = New System.Drawing.Size(568, 409)
        Me.pbBefore.TabIndex = 20
        Me.pbBefore.TurnOnLastFilterSaving_Y = True
        Me.pbBefore.TurnOnLastSelectionSaving_Y = False
        '
        'btnDoForceDeconvolution
        '
        Me.btnDoForceDeconvolution.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDoForceDeconvolution.Image = Global.SpectroscopyManager.My.Resources.Resources.run_25
        Me.btnDoForceDeconvolution.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDoForceDeconvolution.Location = New System.Drawing.Point(898, 548)
        Me.btnDoForceDeconvolution.Name = "btnDoForceDeconvolution"
        Me.btnDoForceDeconvolution.Size = New System.Drawing.Size(286, 43)
        Me.btnDoForceDeconvolution.TabIndex = 14
        Me.btnDoForceDeconvolution.Text = "perform force and" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "current deconvolution"
        Me.btnDoForceDeconvolution.UseVisualStyleBackColor = True
        '
        'gbAfter
        '
        Me.gbAfter.Controls.Add(Me.pbAfter)
        Me.gbAfter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAfter.Location = New System.Drawing.Point(0, 0)
        Me.gbAfter.Name = "gbAfter"
        Me.gbAfter.Size = New System.Drawing.Size(605, 428)
        Me.gbAfter.TabIndex = 15
        Me.gbAfter.TabStop = False
        Me.gbAfter.Text = "after deconvolution:"
        '
        'pbAfter
        '
        Me.pbAfter.AllowAdjustingXColumn = True
        Me.pbAfter.AllowAdjustingYColumn = True
        Me.pbAfter.AutomaticallyRestoreScaleAfterRedraw = True
        Me.pbAfter.CallbackDataPointSelected = Nothing
        Me.pbAfter.CallbackXRangeSelected = Nothing
        Me.pbAfter.CallbackXValueSelected = Nothing
        Me.pbAfter.CallbackXYRangeSelected = Nothing
        Me.pbAfter.CallbackXYValueSelected = Nothing
        Me.pbAfter.CallbackYRangeSelected = Nothing
        Me.pbAfter.CallbackYValueSelected = Nothing
        Me.pbAfter.ClearPointSelectionModeAfterSelection = False
        Me.pbAfter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbAfter.Location = New System.Drawing.Point(3, 16)
        Me.pbAfter.MultipleSpectraStackOffset = 0R
        Me.pbAfter.MultipleYColumnSelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.pbAfter.Name = "pbAfter"
        Me.pbAfter.PointSelectionMode = SpectroscopyManager.mSpectroscopyTableViewer.SelectionModes.None
        Me.pbAfter.ShowColumnSelectors = True
        Me.pbAfter.Size = New System.Drawing.Size(599, 409)
        Me.pbAfter.TabIndex = 20
        Me.pbAfter.TurnOnLastFilterSaving_Y = True
        Me.pbAfter.TurnOnLastSelectionSaving_Y = False
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(1089, 772)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(95, 29)
        Me.btnCloseWindow.TabIndex = 17
        Me.btnCloseWindow.Text = "close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'btnSaveColumn
        '
        Me.btnSaveColumn.Image = Global.SpectroscopyManager.My.Resources.Resources.save_16
        Me.btnSaveColumn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveColumn.Location = New System.Drawing.Point(27, 116)
        Me.btnSaveColumn.Name = "btnSaveColumn"
        Me.btnSaveColumn.Size = New System.Drawing.Size(238, 23)
        Me.btnSaveColumn.TabIndex = 18
        Me.btnSaveColumn.Text = "save columns"
        Me.btnSaveColumn.UseVisualStyleBackColor = True
        '
        'dsDataSmoother
        '
        Me.dsDataSmoother.Location = New System.Drawing.Point(256, 16)
        Me.dsDataSmoother.Name = "dsDataSmoother"
        Me.dsDataSmoother.SelectedSmoothingMethodType = Nothing
        Me.dsDataSmoother.Size = New System.Drawing.Size(266, 224)
        Me.dsDataSmoother.TabIndex = 19
        '
        'gbSmoothingSettings
        '
        Me.gbSmoothingSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbSmoothingSettings.Controls.Add(Me.lblSmoothingDescription)
        Me.gbSmoothingSettings.Controls.Add(Me.dsDataSmoother)
        Me.gbSmoothingSettings.Location = New System.Drawing.Point(4, 544)
        Me.gbSmoothingSettings.Name = "gbSmoothingSettings"
        Me.gbSmoothingSettings.Size = New System.Drawing.Size(528, 247)
        Me.gbSmoothingSettings.TabIndex = 20
        Me.gbSmoothingSettings.TabStop = False
        Me.gbSmoothingSettings.Text = "smoothing settings:"
        '
        'lblSmoothingDescription
        '
        Me.lblSmoothingDescription.AutoSize = True
        Me.lblSmoothingDescription.Location = New System.Drawing.Point(8, 19)
        Me.lblSmoothingDescription.Name = "lblSmoothingDescription"
        Me.lblSmoothingDescription.Size = New System.Drawing.Size(250, 65)
        Me.lblSmoothingDescription.TabIndex = 0
        Me.lblSmoothingDescription.Text = resources.GetString("lblSmoothingDescription.Text")
        '
        'gbOutput
        '
        Me.gbOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOutput.Controls.Add(Me.txtOutputName_Current)
        Me.gbOutput.Controls.Add(Me.Label3)
        Me.gbOutput.Controls.Add(Me.txtOutputName_Force)
        Me.gbOutput.Controls.Add(Me.Label2)
        Me.gbOutput.Controls.Add(Me.btnSaveColumn)
        Me.gbOutput.Location = New System.Drawing.Point(898, 593)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Size = New System.Drawing.Size(286, 156)
        Me.gbOutput.TabIndex = 21
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "output settings:"
        '
        'txtOutputName_Current
        '
        Me.txtOutputName_Current.Location = New System.Drawing.Point(27, 80)
        Me.txtOutputName_Current.Name = "txtOutputName_Current"
        Me.txtOutputName_Current.Size = New System.Drawing.Size(238, 20)
        Me.txtOutputName_Current.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(210, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "column name for the deconvoluted current:"
        '
        'txtOutputName_Force
        '
        Me.txtOutputName_Force.Location = New System.Drawing.Point(27, 38)
        Me.txtOutputName_Force.Name = "txtOutputName_Force"
        Me.txtOutputName_Force.Size = New System.Drawing.Size(238, 20)
        Me.txtOutputName_Force.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(201, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "column name for the deconvoluted force:"
        '
        'scPreview
        '
        Me.scPreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scPreview.Location = New System.Drawing.Point(1, 110)
        Me.scPreview.Name = "scPreview"
        '
        'scPreview.Panel1
        '
        Me.scPreview.Panel1.Controls.Add(Me.gbBefore)
        '
        'scPreview.Panel2
        '
        Me.scPreview.Panel2.Controls.Add(Me.gbAfter)
        Me.scPreview.Size = New System.Drawing.Size(1183, 428)
        Me.scPreview.SplitterDistance = 574
        Me.scPreview.TabIndex = 22
        '
        'gbAdditionalSettings
        '
        Me.gbAdditionalSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAdditionalSettings.Controls.Add(Me.ckbSettings_RemoveFrequencyShiftOffset)
        Me.gbAdditionalSettings.Location = New System.Drawing.Point(582, 544)
        Me.gbAdditionalSettings.Name = "gbAdditionalSettings"
        Me.gbAdditionalSettings.Size = New System.Drawing.Size(308, 43)
        Me.gbAdditionalSettings.TabIndex = 23
        Me.gbAdditionalSettings.TabStop = False
        Me.gbAdditionalSettings.Text = "deconvolution settings:"
        '
        'ckbSettings_RemoveFrequencyShiftOffset
        '
        Me.ckbSettings_RemoveFrequencyShiftOffset.AutoSize = True
        Me.ckbSettings_RemoveFrequencyShiftOffset.Location = New System.Drawing.Point(17, 19)
        Me.ckbSettings_RemoveFrequencyShiftOffset.Name = "ckbSettings_RemoveFrequencyShiftOffset"
        Me.ckbSettings_RemoveFrequencyShiftOffset.Size = New System.Drawing.Size(162, 17)
        Me.ckbSettings_RemoveFrequencyShiftOffset.TabIndex = 0
        Me.ckbSettings_RemoveFrequencyShiftOffset.Text = "remove frequency shift offset"
        Me.ckbSettings_RemoveFrequencyShiftOffset.UseVisualStyleBackColor = True
        '
        'wAFMForceCurrentDeconvolution
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1186, 803)
        Me.Controls.Add(Me.gbAdditionalSettings)
        Me.Controls.Add(Me.gbOutput)
        Me.Controls.Add(Me.gbSmoothingSettings)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnDoForceDeconvolution)
        Me.Controls.Add(Me.gbDataSelection)
        Me.Controls.Add(Me.lblToolDescription)
        Me.Controls.Add(Me.gbSettings)
        Me.Controls.Add(Me.scPreview)
        Me.Name = "wAFMForceCurrentDeconvolution"
        Me.Text = "AFM force and current deconvolution - "
        Me.Controls.SetChildIndex(Me.scPreview, 0)
        Me.Controls.SetChildIndex(Me.gbSettings, 0)
        Me.Controls.SetChildIndex(Me.lblToolDescription, 0)
        Me.Controls.SetChildIndex(Me.gbDataSelection, 0)
        Me.Controls.SetChildIndex(Me.btnDoForceDeconvolution, 0)
        Me.Controls.SetChildIndex(Me.btnCloseWindow, 0)
        Me.Controls.SetChildIndex(Me.gbSmoothingSettings, 0)
        Me.Controls.SetChildIndex(Me.gbOutput, 0)
        Me.Controls.SetChildIndex(Me.gbAdditionalSettings, 0)
        Me.gbSettings.ResumeLayout(False)
        Me.gbSettings.PerformLayout()
        Me.gbDataSelection.ResumeLayout(False)
        Me.gbDataSelection.PerformLayout()
        Me.gbBefore.ResumeLayout(False)
        Me.gbAfter.ResumeLayout(False)
        Me.gbSmoothingSettings.ResumeLayout(False)
        Me.gbSmoothingSettings.PerformLayout()
        Me.gbOutput.ResumeLayout(False)
        Me.gbOutput.PerformLayout()
        Me.scPreview.Panel1.ResumeLayout(False)
        Me.scPreview.Panel2.ResumeLayout(False)
        CType(Me.scPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scPreview.ResumeLayout(False)
        Me.gbAdditionalSettings.ResumeLayout(False)
        Me.gbAdditionalSettings.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents gbSettings As GroupBox
    Friend WithEvents txtAFMSetupSpringConst As NumericTextbox
    Friend WithEvents lblSpringConstant As Label
    Friend WithEvents txtAFMSetupOscAmp As NumericTextbox
    Friend WithEvents lblOscillationAmplitude As Label
    Friend WithEvents txtAFMSetupResFreq As NumericTextbox
    Friend WithEvents lblResonanceFrequency As Label
    Friend WithEvents lblToolDescription As Label
    Friend WithEvents gbDataSelection As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblColForceGradient As Label
    Friend WithEvents csColumn_CurrentSignal As ucSpectroscopyColumnSelector
    Friend WithEvents csColumn_ForceGradient As ucSpectroscopyColumnSelector
    Friend WithEvents gbBefore As GroupBox
    Friend WithEvents pbBefore As mSpectroscopyTableViewer
    Friend WithEvents btnDoForceDeconvolution As Button
    Friend WithEvents gbAfter As GroupBox
    Friend WithEvents pbAfter As mSpectroscopyTableViewer
    Friend WithEvents lblColZRel As Label
    Friend WithEvents csColumn_ZRel As ucSpectroscopyColumnSelector
    Friend WithEvents btnCloseWindow As Button
    Friend WithEvents btnSaveColumn As Button
    Friend WithEvents lblUnitSpringConst As Label
    Friend WithEvents lblUnitOscAmp As Label
    Friend WithEvents lblUnitResFreq As Label
    Friend WithEvents dsDataSmoother As mDataSmoothing
    Friend WithEvents gbSmoothingSettings As GroupBox
    Friend WithEvents lblSmoothingDescription As Label
    Friend WithEvents gbOutput As GroupBox
    Friend WithEvents txtOutputName_Current As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtOutputName_Force As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents scPreview As SplitContainer
    Friend WithEvents gbAdditionalSettings As GroupBox
    Friend WithEvents ckbSettings_RemoveFrequencyShiftOffset As CheckBox
End Class
