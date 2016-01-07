<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wComputingCloudDetails
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvCloudMembers = New System.Windows.Forms.DataGridView()
        Me.colClientIP = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colComputerName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colVersion = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCPUName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAvailableThreads = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.gbCloudMembers = New System.Windows.Forms.GroupBox()
        CType(Me.dgvCloudMembers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCloudMembers.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvCloudMembers
        '
        Me.dgvCloudMembers.AllowUserToAddRows = False
        Me.dgvCloudMembers.AllowUserToDeleteRows = False
        Me.dgvCloudMembers.AllowUserToOrderColumns = True
        Me.dgvCloudMembers.AllowUserToResizeRows = False
        Me.dgvCloudMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCloudMembers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colClientIP, Me.colComputerName, Me.colVersion, Me.colCPUName, Me.colAvailableThreads})
        Me.dgvCloudMembers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCloudMembers.Location = New System.Drawing.Point(3, 16)
        Me.dgvCloudMembers.Name = "dgvCloudMembers"
        Me.dgvCloudMembers.ReadOnly = True
        Me.dgvCloudMembers.RowHeadersVisible = False
        Me.dgvCloudMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCloudMembers.Size = New System.Drawing.Size(797, 386)
        Me.dgvCloudMembers.TabIndex = 0
        '
        'colClientIP
        '
        Me.colClientIP.HeaderText = "Client IP"
        Me.colClientIP.Name = "colClientIP"
        Me.colClientIP.ReadOnly = True
        Me.colClientIP.Width = 200
        '
        'colComputerName
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.colComputerName.DefaultCellStyle = DataGridViewCellStyle4
        Me.colComputerName.HeaderText = "Computer-Name"
        Me.colComputerName.Name = "colComputerName"
        Me.colComputerName.ReadOnly = True
        '
        'colVersion
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colVersion.DefaultCellStyle = DataGridViewCellStyle5
        Me.colVersion.HeaderText = "Client Version"
        Me.colVersion.Name = "colVersion"
        Me.colVersion.ReadOnly = True
        Me.colVersion.Width = 150
        '
        'colCPUName
        '
        Me.colCPUName.HeaderText = "CPU"
        Me.colCPUName.Name = "colCPUName"
        Me.colCPUName.ReadOnly = True
        Me.colCPUName.Width = 250
        '
        'colAvailableThreads
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.Format = "N0"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.colAvailableThreads.DefaultCellStyle = DataGridViewCellStyle6
        Me.colAvailableThreads.HeaderText = "Available Threads"
        Me.colAvailableThreads.Name = "colAvailableThreads"
        Me.colAvailableThreads.ReadOnly = True
        Me.colAvailableThreads.Width = 70
        '
        'gbCloudMembers
        '
        Me.gbCloudMembers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbCloudMembers.Controls.Add(Me.dgvCloudMembers)
        Me.gbCloudMembers.Location = New System.Drawing.Point(13, 13)
        Me.gbCloudMembers.Name = "gbCloudMembers"
        Me.gbCloudMembers.Size = New System.Drawing.Size(803, 405)
        Me.gbCloudMembers.TabIndex = 1
        Me.gbCloudMembers.TabStop = False
        Me.gbCloudMembers.Text = "Computing Cloud Members"
        '
        'wComputingCloudDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(828, 430)
        Me.Controls.Add(Me.gbCloudMembers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "wComputingCloudDetails"
        Me.Text = "Computing Cloud Details"
        CType(Me.dgvCloudMembers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbCloudMembers.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvCloudMembers As System.Windows.Forms.DataGridView
    Friend WithEvents gbCloudMembers As System.Windows.Forms.GroupBox
    Friend WithEvents colClientIP As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colComputerName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colVersion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCPUName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAvailableThreads As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
