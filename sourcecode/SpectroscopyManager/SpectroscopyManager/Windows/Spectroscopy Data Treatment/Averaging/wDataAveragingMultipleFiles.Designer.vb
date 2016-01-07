<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wDataAveragingMultipleFiles
    Inherits SpectroscopyManager.wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wDataAveragingMultipleFiles))
        Me.lbCommonColumns = New System.Windows.Forms.ListBox()
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnCloseWindow = New System.Windows.Forms.Button()
        Me.btnApplyAveraging = New System.Windows.Forms.Button()
        Me.txtNewColumnName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lbCommonColumns
        '
        Me.lbCommonColumns.FormattingEnabled = True
        Me.lbCommonColumns.Location = New System.Drawing.Point(15, 57)
        Me.lbCommonColumns.Name = "lbCommonColumns"
        Me.lbCommonColumns.Size = New System.Drawing.Size(211, 108)
        Me.lbCommonColumns.TabIndex = 0
        '
        'lblHeading
        '
        Me.lblHeading.AutoSize = True
        Me.lblHeading.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeading.Location = New System.Drawing.Point(13, 13)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(146, 16)
        Me.lblHeading.TabIndex = 1
        Me.lblHeading.Text = "Averaging %% files:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(213, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Columns the selected files have in common:"
        '
        'btnCloseWindow
        '
        Me.btnCloseWindow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCloseWindow.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCloseWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseWindow.Location = New System.Drawing.Point(16, 281)
        Me.btnCloseWindow.Name = "btnCloseWindow"
        Me.btnCloseWindow.Size = New System.Drawing.Size(210, 29)
        Me.btnCloseWindow.TabIndex = 13
        Me.btnCloseWindow.Text = "Close"
        Me.btnCloseWindow.UseVisualStyleBackColor = True
        '
        'btnApplyAveraging
        '
        Me.btnApplyAveraging.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnApplyAveraging.Enabled = False
        Me.btnApplyAveraging.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApplyAveraging.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_25
        Me.btnApplyAveraging.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApplyAveraging.Location = New System.Drawing.Point(16, 211)
        Me.btnApplyAveraging.Name = "btnApplyAveraging"
        Me.btnApplyAveraging.Size = New System.Drawing.Size(210, 64)
        Me.btnApplyAveraging.TabIndex = 12
        Me.btnApplyAveraging.Text = "Apply Averaging"
        Me.btnApplyAveraging.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApplyAveraging.UseVisualStyleBackColor = True
        '
        'txtNewColumnName
        '
        Me.txtNewColumnName.Location = New System.Drawing.Point(16, 184)
        Me.txtNewColumnName.Name = "txtNewColumnName"
        Me.txtNewColumnName.Size = New System.Drawing.Size(210, 20)
        Me.txtNewColumnName.TabIndex = 15
        Me.txtNewColumnName.Text = "Averaged Data"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 168)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Save as ColumnName:"
        '
        'wDataAveragingMultipleFiles
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(246, 317)
        Me.Controls.Add(Me.txtNewColumnName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCloseWindow)
        Me.Controls.Add(Me.btnApplyAveraging)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblHeading)
        Me.Controls.Add(Me.lbCommonColumns)
        Me.Enabled = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "wDataAveragingMultipleFiles"
        Me.Text = "Average Data in Multiple Files"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbCommonColumns As System.Windows.Forms.ListBox
    Friend WithEvents lblHeading As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCloseWindow As System.Windows.Forms.Button
    Friend WithEvents btnApplyAveraging As System.Windows.Forms.Button
    Friend WithEvents txtNewColumnName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
