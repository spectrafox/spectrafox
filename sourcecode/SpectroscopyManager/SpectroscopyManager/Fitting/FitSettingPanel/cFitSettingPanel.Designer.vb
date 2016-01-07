<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class cFitSettingPanel
    Inherits UserControl

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
        Me.btnRemoveFitFunction = New System.Windows.Forms.Button()
        Me.lblFitFunctionName = New System.Windows.Forms.Label()
        Me.txtFormula = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.panDescriptionContainer = New System.Windows.Forms.Panel()
        Me.tcFitFunctionDetails = New System.Windows.Forms.TabControl()
        Me.tpFitFunctionDescription = New System.Windows.Forms.TabPage()
        Me.tpFitFunctionFormula = New System.Windows.Forms.TabPage()
        Me.tpFitFunctionAuthors = New System.Windows.Forms.TabPage()
        Me.txtAuthors = New System.Windows.Forms.TextBox()
        Me.ckbUseCUDA = New System.Windows.Forms.CheckBox()
        Me.panDescriptionContainer.SuspendLayout()
        Me.tcFitFunctionDetails.SuspendLayout()
        Me.tpFitFunctionDescription.SuspendLayout()
        Me.tpFitFunctionFormula.SuspendLayout()
        Me.tpFitFunctionAuthors.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRemoveFitFunction
        '
        Me.btnRemoveFitFunction.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnRemoveFitFunction.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveFitFunction.Location = New System.Drawing.Point(6, 3)
        Me.btnRemoveFitFunction.Name = "btnRemoveFitFunction"
        Me.btnRemoveFitFunction.Size = New System.Drawing.Size(24, 22)
        Me.btnRemoveFitFunction.TabIndex = 0
        Me.btnRemoveFitFunction.TabStop = False
        Me.btnRemoveFitFunction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveFitFunction.UseVisualStyleBackColor = True
        '
        'lblFitFunctionName
        '
        Me.lblFitFunctionName.AutoSize = True
        Me.lblFitFunctionName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFitFunctionName.Location = New System.Drawing.Point(32, 5)
        Me.lblFitFunctionName.Name = "lblFitFunctionName"
        Me.lblFitFunctionName.Size = New System.Drawing.Size(116, 15)
        Me.lblFitFunctionName.TabIndex = 1
        Me.lblFitFunctionName.Text = "FitFunctionName"
        '
        'txtFormula
        '
        Me.txtFormula.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFormula.Location = New System.Drawing.Point(3, 3)
        Me.txtFormula.Multiline = True
        Me.txtFormula.Name = "txtFormula"
        Me.txtFormula.ReadOnly = True
        Me.txtFormula.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtFormula.Size = New System.Drawing.Size(233, 72)
        Me.txtFormula.TabIndex = 0
        Me.txtFormula.TabStop = False
        '
        'txtDescription
        '
        Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDescription.Location = New System.Drawing.Point(3, 3)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(243, 72)
        Me.txtDescription.TabIndex = 1
        Me.txtDescription.TabStop = False
        '
        'panDescriptionContainer
        '
        Me.panDescriptionContainer.Controls.Add(Me.tcFitFunctionDetails)
        Me.panDescriptionContainer.Controls.Add(Me.ckbUseCUDA)
        Me.panDescriptionContainer.Controls.Add(Me.btnRemoveFitFunction)
        Me.panDescriptionContainer.Controls.Add(Me.lblFitFunctionName)
        Me.panDescriptionContainer.Dock = System.Windows.Forms.DockStyle.Left
        Me.panDescriptionContainer.Location = New System.Drawing.Point(0, 0)
        Me.panDescriptionContainer.Name = "panDescriptionContainer"
        Me.panDescriptionContainer.Size = New System.Drawing.Size(256, 158)
        Me.panDescriptionContainer.TabIndex = 3
        '
        'tcFitFunctionDetails
        '
        Me.tcFitFunctionDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcFitFunctionDetails.Controls.Add(Me.tpFitFunctionDescription)
        Me.tcFitFunctionDetails.Controls.Add(Me.tpFitFunctionFormula)
        Me.tcFitFunctionDetails.Controls.Add(Me.tpFitFunctionAuthors)
        Me.tcFitFunctionDetails.Location = New System.Drawing.Point(-1, 28)
        Me.tcFitFunctionDetails.Name = "tcFitFunctionDetails"
        Me.tcFitFunctionDetails.SelectedIndex = 0
        Me.tcFitFunctionDetails.Size = New System.Drawing.Size(257, 104)
        Me.tcFitFunctionDetails.TabIndex = 4
        '
        'tpFitFunctionDescription
        '
        Me.tpFitFunctionDescription.Controls.Add(Me.txtDescription)
        Me.tpFitFunctionDescription.Location = New System.Drawing.Point(4, 22)
        Me.tpFitFunctionDescription.Name = "tpFitFunctionDescription"
        Me.tpFitFunctionDescription.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFitFunctionDescription.Size = New System.Drawing.Size(249, 78)
        Me.tpFitFunctionDescription.TabIndex = 1
        Me.tpFitFunctionDescription.Text = "Description"
        Me.tpFitFunctionDescription.UseVisualStyleBackColor = True
        '
        'tpFitFunctionFormula
        '
        Me.tpFitFunctionFormula.Controls.Add(Me.txtFormula)
        Me.tpFitFunctionFormula.Location = New System.Drawing.Point(4, 22)
        Me.tpFitFunctionFormula.Name = "tpFitFunctionFormula"
        Me.tpFitFunctionFormula.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFitFunctionFormula.Size = New System.Drawing.Size(239, 78)
        Me.tpFitFunctionFormula.TabIndex = 0
        Me.tpFitFunctionFormula.Text = "Formula"
        Me.tpFitFunctionFormula.UseVisualStyleBackColor = True
        '
        'tpFitFunctionAuthors
        '
        Me.tpFitFunctionAuthors.Controls.Add(Me.txtAuthors)
        Me.tpFitFunctionAuthors.Location = New System.Drawing.Point(4, 22)
        Me.tpFitFunctionAuthors.Name = "tpFitFunctionAuthors"
        Me.tpFitFunctionAuthors.Size = New System.Drawing.Size(239, 78)
        Me.tpFitFunctionAuthors.TabIndex = 2
        Me.tpFitFunctionAuthors.Text = "References and Authors"
        Me.tpFitFunctionAuthors.UseVisualStyleBackColor = True
        '
        'txtAuthors
        '
        Me.txtAuthors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtAuthors.Location = New System.Drawing.Point(0, 0)
        Me.txtAuthors.Multiline = True
        Me.txtAuthors.Name = "txtAuthors"
        Me.txtAuthors.ReadOnly = True
        Me.txtAuthors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAuthors.Size = New System.Drawing.Size(239, 78)
        Me.txtAuthors.TabIndex = 2
        Me.txtAuthors.TabStop = False
        '
        'ckbUseCUDA
        '
        Me.ckbUseCUDA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ckbUseCUDA.AutoSize = True
        Me.ckbUseCUDA.Location = New System.Drawing.Point(5, 138)
        Me.ckbUseCUDA.Name = "ckbUseCUDA"
        Me.ckbUseCUDA.Size = New System.Drawing.Size(240, 17)
        Me.ckbUseCUDA.TabIndex = 3
        Me.ckbUseCUDA.Text = "use graphic processor acceleration if possible"
        Me.ckbUseCUDA.UseVisualStyleBackColor = True
        '
        'cFitSettingPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.panDescriptionContainer)
        Me.MinimumSize = New System.Drawing.Size(487, 121)
        Me.Name = "cFitSettingPanel"
        Me.Size = New System.Drawing.Size(616, 158)
        Me.panDescriptionContainer.ResumeLayout(False)
        Me.panDescriptionContainer.PerformLayout()
        Me.tcFitFunctionDetails.ResumeLayout(False)
        Me.tpFitFunctionDescription.ResumeLayout(False)
        Me.tpFitFunctionDescription.PerformLayout()
        Me.tpFitFunctionFormula.ResumeLayout(False)
        Me.tpFitFunctionFormula.PerformLayout()
        Me.tpFitFunctionAuthors.ResumeLayout(False)
        Me.tpFitFunctionAuthors.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnRemoveFitFunction As System.Windows.Forms.Button
    Friend WithEvents lblFitFunctionName As System.Windows.Forms.Label
    Friend WithEvents txtFormula As System.Windows.Forms.TextBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents panDescriptionContainer As System.Windows.Forms.Panel
    Friend WithEvents ckbUseCUDA As System.Windows.Forms.CheckBox
    Friend WithEvents tcFitFunctionDetails As System.Windows.Forms.TabControl
    Friend WithEvents tpFitFunctionFormula As System.Windows.Forms.TabPage
    Friend WithEvents tpFitFunctionDescription As System.Windows.Forms.TabPage
    Friend WithEvents tpFitFunctionAuthors As System.Windows.Forms.TabPage
    Friend WithEvents txtAuthors As System.Windows.Forms.TextBox

End Class
