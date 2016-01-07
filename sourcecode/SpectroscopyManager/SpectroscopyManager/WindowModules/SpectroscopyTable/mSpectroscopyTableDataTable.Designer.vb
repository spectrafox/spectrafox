<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mSpectroscopyTableDataTable
    Inherits System.Windows.Forms.UserControl

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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvSpectroscopyTable = New System.Windows.Forms.DataGridView()
        Me.cmMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCopyToClipboard = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.dgvSpectroscopyTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvSpectroscopyTable
        '
        Me.dgvSpectroscopyTable.AllowUserToAddRows = False
        Me.dgvSpectroscopyTable.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.dgvSpectroscopyTable.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvSpectroscopyTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Me.dgvSpectroscopyTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSpectroscopyTable.ContextMenuStrip = Me.cmMenu
        Me.dgvSpectroscopyTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSpectroscopyTable.Location = New System.Drawing.Point(0, 0)
        Me.dgvSpectroscopyTable.Name = "dgvSpectroscopyTable"
        Me.dgvSpectroscopyTable.Size = New System.Drawing.Size(807, 487)
        Me.dgvSpectroscopyTable.TabIndex = 0
        Me.dgvSpectroscopyTable.VirtualMode = True
        '
        'cmMenu
        '
        Me.cmMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopyToClipboard})
        Me.cmMenu.Name = "cmMenu"
        Me.cmMenu.Size = New System.Drawing.Size(218, 26)
        '
        'mnuCopyToClipboard
        '
        Me.mnuCopyToClipboard.Image = Global.SpectroscopyManager.My.Resources.Resources.copy_16
        Me.mnuCopyToClipboard.Name = "mnuCopyToClipboard"
        Me.mnuCopyToClipboard.Size = New System.Drawing.Size(217, 22)
        Me.mnuCopyToClipboard.Text = "copy selection to clipboard"
        '
        'mSpectroscopyTableDataTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgvSpectroscopyTable)
        Me.Name = "mSpectroscopyTableDataTable"
        Me.Size = New System.Drawing.Size(807, 487)
        CType(Me.dgvSpectroscopyTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvSpectroscopyTable As System.Windows.Forms.DataGridView
    Friend WithEvents cmMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCopyToClipboard As System.Windows.Forms.ToolStripMenuItem

End Class
