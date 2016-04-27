<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wSettings_Program
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
        Me.dgvExternalViewers = New System.Windows.Forms.DataGridView()
        Me.tcSettings = New System.Windows.Forms.TabControl()
        Me.tcExternalViewers = New System.Windows.Forms.TabPage()
        Me.lblExternalViewers_Description = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.dgvExternalViewers_colDisplayName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvExternalViewers_colPath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvExternalViewers_colArguments = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvExternalViewers_colBrowse = New System.Windows.Forms.DataGridViewButtonColumn()
        CType(Me.dgvExternalViewers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcSettings.SuspendLayout()
        Me.tcExternalViewers.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvExternalViewers
        '
        Me.dgvExternalViewers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvExternalViewers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvExternalViewers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvExternalViewers_colDisplayName, Me.dgvExternalViewers_colPath, Me.dgvExternalViewers_colArguments, Me.dgvExternalViewers_colBrowse})
        Me.dgvExternalViewers.Location = New System.Drawing.Point(8, 26)
        Me.dgvExternalViewers.Name = "dgvExternalViewers"
        Me.dgvExternalViewers.Size = New System.Drawing.Size(918, 282)
        Me.dgvExternalViewers.TabIndex = 0
        '
        'tcSettings
        '
        Me.tcSettings.Controls.Add(Me.tcExternalViewers)
        Me.tcSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcSettings.Location = New System.Drawing.Point(0, 0)
        Me.tcSettings.Name = "tcSettings"
        Me.tcSettings.SelectedIndex = 0
        Me.tcSettings.Size = New System.Drawing.Size(942, 371)
        Me.tcSettings.TabIndex = 1
        '
        'tcExternalViewers
        '
        Me.tcExternalViewers.Controls.Add(Me.lblExternalViewers_Description)
        Me.tcExternalViewers.Controls.Add(Me.btnSave)
        Me.tcExternalViewers.Controls.Add(Me.dgvExternalViewers)
        Me.tcExternalViewers.Location = New System.Drawing.Point(4, 22)
        Me.tcExternalViewers.Name = "tcExternalViewers"
        Me.tcExternalViewers.Padding = New System.Windows.Forms.Padding(3)
        Me.tcExternalViewers.Size = New System.Drawing.Size(934, 345)
        Me.tcExternalViewers.TabIndex = 0
        Me.tcExternalViewers.Text = "external scan image viewers"
        Me.tcExternalViewers.UseVisualStyleBackColor = True
        '
        'lblExternalViewers_Description
        '
        Me.lblExternalViewers_Description.AutoSize = True
        Me.lblExternalViewers_Description.Location = New System.Drawing.Point(6, 7)
        Me.lblExternalViewers_Description.Name = "lblExternalViewers_Description"
        Me.lblExternalViewers_Description.Size = New System.Drawing.Size(681, 13)
        Me.lblExternalViewers_Description.TabIndex = 2
        Me.lblExternalViewers_Description.Text = "Add viewers to the list. The display name is used in the program to identify the " &
    "viewer. In the path, the ""%f"" template is replaced by the filename."
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Image = Global.SpectroscopyManager.My.Resources.Resources.save_16
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(823, 314)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(103, 23)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "save viewers"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'dgvExternalViewers_colDisplayName
        '
        Me.dgvExternalViewers_colDisplayName.HeaderText = "display name"
        Me.dgvExternalViewers_colDisplayName.Name = "dgvExternalViewers_colDisplayName"
        '
        'dgvExternalViewers_colPath
        '
        Me.dgvExternalViewers_colPath.HeaderText = "path to the viewer including options"
        Me.dgvExternalViewers_colPath.Name = "dgvExternalViewers_colPath"
        Me.dgvExternalViewers_colPath.Width = 450
        '
        'dgvExternalViewers_colArguments
        '
        Me.dgvExternalViewers_colArguments.HeaderText = "startup arguments"
        Me.dgvExternalViewers_colArguments.Name = "dgvExternalViewers_colArguments"
        Me.dgvExternalViewers_colArguments.Width = 250
        '
        'dgvExternalViewers_colBrowse
        '
        Me.dgvExternalViewers_colBrowse.HeaderText = "browse"
        Me.dgvExternalViewers_colBrowse.Name = "dgvExternalViewers_colBrowse"
        Me.dgvExternalViewers_colBrowse.Width = 50
        '
        'wSettings_Program
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(942, 371)
        Me.Controls.Add(Me.tcSettings)
        Me.Name = "wSettings_Program"
        Me.Text = "program settings"
        CType(Me.dgvExternalViewers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcSettings.ResumeLayout(False)
        Me.tcExternalViewers.ResumeLayout(False)
        Me.tcExternalViewers.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvExternalViewers As DataGridView
    Friend WithEvents tcSettings As TabControl
    Friend WithEvents tcExternalViewers As TabPage
    Friend WithEvents btnSave As Button
    Friend WithEvents lblExternalViewers_Description As Label
    Friend WithEvents dgvExternalViewers_colDisplayName As DataGridViewTextBoxColumn
    Friend WithEvents dgvExternalViewers_colPath As DataGridViewTextBoxColumn
    Friend WithEvents dgvExternalViewers_colArguments As DataGridViewTextBoxColumn
    Friend WithEvents dgvExternalViewers_colBrowse As DataGridViewButtonColumn
End Class
