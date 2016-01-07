<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wGPUComputingSettings
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
        Me.txtCudaList = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cboGPUComputingDevices = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnConfirmComputingDevices = New System.Windows.Forms.Button()
        Me.txtInfo = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtCudaList
        '
        Me.txtCudaList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtCudaList.Location = New System.Drawing.Point(3, 16)
        Me.txtCudaList.Multiline = True
        Me.txtCudaList.Name = "txtCudaList"
        Me.txtCudaList.ReadOnly = True
        Me.txtCudaList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCudaList.Size = New System.Drawing.Size(366, 459)
        Me.txtCudaList.TabIndex = 0
        Me.txtCudaList.WordWrap = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.txtCudaList)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(372, 478)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Detected GPU Computing Devices and Capabilities"
        '
        'cboGPUComputingDevices
        '
        Me.cboGPUComputingDevices.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboGPUComputingDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGPUComputingDevices.FormattingEnabled = True
        Me.cboGPUComputingDevices.Location = New System.Drawing.Point(397, 424)
        Me.cboGPUComputingDevices.Name = "cboGPUComputingDevices"
        Me.cboGPUComputingDevices.Size = New System.Drawing.Size(327, 21)
        Me.cboGPUComputingDevices.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(394, 408)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(168, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Selected GPU Computing Device:"
        '
        'btnConfirmComputingDevices
        '
        Me.btnConfirmComputingDevices.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConfirmComputingDevices.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_16
        Me.btnConfirmComputingDevices.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnConfirmComputingDevices.Location = New System.Drawing.Point(664, 459)
        Me.btnConfirmComputingDevices.Name = "btnConfirmComputingDevices"
        Me.btnConfirmComputingDevices.Size = New System.Drawing.Size(64, 23)
        Me.btnConfirmComputingDevices.TabIndex = 4
        Me.btnConfirmComputingDevices.Text = "close"
        Me.btnConfirmComputingDevices.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnConfirmComputingDevices.UseVisualStyleBackColor = True
        '
        'txtInfo
        '
        Me.txtInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtInfo.Location = New System.Drawing.Point(386, 12)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ReadOnly = True
        Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtInfo.Size = New System.Drawing.Size(346, 382)
        Me.txtInfo.TabIndex = 1
        Me.txtInfo.Text = "----- INFO -----" & Global.Microsoft.VisualBasic.ChrW(13)
        '
        'wGPUComputingSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(740, 494)
        Me.Controls.Add(Me.txtInfo)
        Me.Controls.Add(Me.btnConfirmComputingDevices)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboGPUComputingDevices)
        Me.Controls.Add(Me.GroupBox1)
        Me.MinimumSize = New System.Drawing.Size(679, 533)
        Me.Name = "wGPUComputingSettings"
        Me.Text = "GPU Computing Settings"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCudaList As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cboGPUComputingDevices As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnConfirmComputingDevices As System.Windows.Forms.Button
    Friend WithEvents txtInfo As System.Windows.Forms.TextBox
End Class
