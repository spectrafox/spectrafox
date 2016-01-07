<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class wUnhandledExceptionReportForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(wUnhandledExceptionReportForm))
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tlpReportContent = New System.Windows.Forms.TableLayoutPanel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtApplication = New System.Windows.Forms.TextBox()
        Me.txtDate = New System.Windows.Forms.TextBox()
        Me.txtUserDescription = New System.Windows.Forms.TextBox()
        Me.txtException = New System.Windows.Forms.TextBox()
        Me.txtReference = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.flpAttachments = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAddAttachment = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.btnSendReport = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tlpReportContent.SuspendLayout()
        Me.flpAttachments.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbIcon
        '
        Me.pbIcon.Image = Global.SpectroscopyManager.My.Resources.Resources.attention
        Me.pbIcon.Location = New System.Drawing.Point(11, 31)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(104, 105)
        Me.pbIcon.TabIndex = 0
        Me.pbIcon.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(121, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(180, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "What has happened?"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(121, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(172, 20)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "How can we fix this?"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(146, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(475, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "We are sorry! An unexpected error occured! Probably the last action you performed" & _
    " was the reason."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(146, 84)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(450, 52)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = resources.GetString("Label4.Text")
        '
        'tlpReportContent
        '
        Me.tlpReportContent.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpReportContent.ColumnCount = 2
        Me.tlpReportContent.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.92564!))
        Me.tlpReportContent.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.07436!))
        Me.tlpReportContent.Controls.Add(Me.Label5, 0, 0)
        Me.tlpReportContent.Controls.Add(Me.txtTitle, 1, 0)
        Me.tlpReportContent.Controls.Add(Me.Label6, 0, 1)
        Me.tlpReportContent.Controls.Add(Me.txtApplication, 1, 1)
        Me.tlpReportContent.Controls.Add(Me.txtDate, 1, 2)
        Me.tlpReportContent.Controls.Add(Me.txtUserDescription, 1, 5)
        Me.tlpReportContent.Controls.Add(Me.txtException, 1, 6)
        Me.tlpReportContent.Controls.Add(Me.txtReference, 1, 7)
        Me.tlpReportContent.Controls.Add(Me.Label8, 0, 5)
        Me.tlpReportContent.Controls.Add(Me.Label9, 0, 6)
        Me.tlpReportContent.Controls.Add(Me.Label10, 0, 7)
        Me.tlpReportContent.Controls.Add(Me.Label11, 0, 8)
        Me.tlpReportContent.Controls.Add(Me.flpAttachments, 1, 8)
        Me.tlpReportContent.Controls.Add(Me.Label7, 0, 2)
        Me.tlpReportContent.Controls.Add(Me.Label13, 0, 3)
        Me.tlpReportContent.Controls.Add(Me.Label14, 0, 4)
        Me.tlpReportContent.Controls.Add(Me.txtUsername, 1, 3)
        Me.tlpReportContent.Controls.Add(Me.txtEmail, 1, 4)
        Me.tlpReportContent.Location = New System.Drawing.Point(12, 156)
        Me.tlpReportContent.Name = "tlpReportContent"
        Me.tlpReportContent.RowCount = 9
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpReportContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpReportContent.Size = New System.Drawing.Size(659, 418)
        Me.tlpReportContent.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(30, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Title:"
        '
        'txtTitle
        '
        Me.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTitle.Location = New System.Drawing.Point(74, 3)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.ReadOnly = True
        Me.txtTitle.Size = New System.Drawing.Size(582, 20)
        Me.txtTitle.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Application:"
        '
        'txtApplication
        '
        Me.txtApplication.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtApplication.Location = New System.Drawing.Point(74, 27)
        Me.txtApplication.Name = "txtApplication"
        Me.txtApplication.ReadOnly = True
        Me.txtApplication.Size = New System.Drawing.Size(582, 20)
        Me.txtApplication.TabIndex = 2
        '
        'txtDate
        '
        Me.txtDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDate.Location = New System.Drawing.Point(74, 51)
        Me.txtDate.Name = "txtDate"
        Me.txtDate.ReadOnly = True
        Me.txtDate.Size = New System.Drawing.Size(582, 20)
        Me.txtDate.TabIndex = 3
        '
        'txtUserDescription
        '
        Me.txtUserDescription.BackColor = System.Drawing.Color.PaleGreen
        Me.txtUserDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUserDescription.Location = New System.Drawing.Point(74, 123)
        Me.txtUserDescription.Multiline = True
        Me.txtUserDescription.Name = "txtUserDescription"
        Me.txtUserDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtUserDescription.Size = New System.Drawing.Size(582, 68)
        Me.txtUserDescription.TabIndex = 4
        '
        'txtException
        '
        Me.txtException.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtException.Location = New System.Drawing.Point(74, 197)
        Me.txtException.Multiline = True
        Me.txtException.Name = "txtException"
        Me.txtException.ReadOnly = True
        Me.txtException.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtException.Size = New System.Drawing.Size(582, 68)
        Me.txtException.TabIndex = 5
        '
        'txtReference
        '
        Me.txtReference.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtReference.Location = New System.Drawing.Point(74, 271)
        Me.txtReference.Multiline = True
        Me.txtReference.Name = "txtReference"
        Me.txtReference.ReadOnly = True
        Me.txtReference.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtReference.Size = New System.Drawing.Size(582, 68)
        Me.txtReference.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 120)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 52)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Your description of what happened:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 194)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 39)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Details of the exception:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 268)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(65, 26)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Program References:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 342)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Attachment:"
        '
        'flpAttachments
        '
        Me.flpAttachments.AutoScroll = True
        Me.flpAttachments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpAttachments.Controls.Add(Me.btnAddAttachment)
        Me.flpAttachments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAttachments.Location = New System.Drawing.Point(74, 345)
        Me.flpAttachments.Name = "flpAttachments"
        Me.flpAttachments.Size = New System.Drawing.Size(582, 70)
        Me.flpAttachments.TabIndex = 7
        '
        'btnAddAttachment
        '
        Me.btnAddAttachment.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddAttachment.Image = Global.SpectroscopyManager.My.Resources.Resources.add_16
        Me.btnAddAttachment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddAttachment.Location = New System.Drawing.Point(3, 3)
        Me.btnAddAttachment.Name = "btnAddAttachment"
        Me.btnAddAttachment.Size = New System.Drawing.Size(258, 23)
        Me.btnAddAttachment.TabIndex = 0
        Me.btnAddAttachment.Text = "add attachment"
        Me.btnAddAttachment.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 48)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(33, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Date:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(3, 72)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(61, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Your name:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(3, 96)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(59, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Your email:"
        '
        'txtUsername
        '
        Me.txtUsername.BackColor = System.Drawing.Color.PaleGreen
        Me.txtUsername.Location = New System.Drawing.Point(74, 75)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(275, 20)
        Me.txtUsername.TabIndex = 8
        '
        'txtEmail
        '
        Me.txtEmail.BackColor = System.Drawing.Color.PaleGreen
        Me.txtEmail.Location = New System.Drawing.Point(74, 99)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(275, 20)
        Me.txtEmail.TabIndex = 9
        '
        'btnSendReport
        '
        Me.btnSendReport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSendReport.Image = Global.SpectroscopyManager.My.Resources.Resources.ok_16
        Me.btnSendReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSendReport.Location = New System.Drawing.Point(11, 580)
        Me.btnSendReport.Name = "btnSendReport"
        Me.btnSendReport.Size = New System.Drawing.Size(390, 23)
        Me.btnSendReport.TabIndex = 4
        Me.btnSendReport.Text = "Yes, I want to help to improve the program. Send a report!"
        Me.btnSendReport.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Image = Global.SpectroscopyManager.My.Resources.Resources.cancel_16
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(407, 580)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(264, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "I don't want to help. Don't send a report."
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(31, 8)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(66, 20)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Ooops!"
        '
        'wUnhandledExceptionReportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(684, 615)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSendReport)
        Me.Controls.Add(Me.tlpReportContent)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pbIcon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "wUnhandledExceptionReportForm"
        Me.Text = "An unknown error has occurred!"
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tlpReportContent.ResumeLayout(False)
        Me.tlpReportContent.PerformLayout()
        Me.flpAttachments.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbIcon As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tlpReportContent As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtApplication As System.Windows.Forms.TextBox
    Friend WithEvents txtDate As System.Windows.Forms.TextBox
    Friend WithEvents txtUserDescription As System.Windows.Forms.TextBox
    Friend WithEvents txtException As System.Windows.Forms.TextBox
    Friend WithEvents txtReference As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents flpAttachments As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnSendReport As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAddAttachment As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
End Class
