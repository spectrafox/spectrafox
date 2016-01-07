Imports System.IO

Public Class wUnhandledExceptionReportForm
    Inherits wFormBase

    Private UnhandledExceptionReport As cBugReporter.UnhandledExceptionReport

    ''' <summary>
    ''' Initialization
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Initialization with and unhandled Exception object.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(unhandledExceptionReport As cBugReporter.UnhandledExceptionReport)
        Me.New()
        Me.UnhandledExceptionReport = unhandledExceptionReport
        If Me.UnhandledExceptionReport IsNot Nothing AndAlso Me.UnhandledExceptionReport.UserComments = String.Empty Then
            Me.UnhandledExceptionReport.UserComments = My.Resources.rBugReporting.BugReport_DefaultDescription
        End If
        If Me.UnhandledExceptionReport IsNot Nothing AndAlso Me.UnhandledExceptionReport.UserMail = String.Empty Then
            Me.UnhandledExceptionReport.UserMail = My.Resources.rBugReporting.BugReport_DefaultMail
        End If

        With Me.UnhandledExceptionReport
            txtTitle.Text = .Title
            txtApplication.Text = .Application
            txtDate.Text = .ErrorDate.ToString()
            txtUsername.DataBindings.Add("Text", Me.UnhandledExceptionReport, "UserName")
            txtEmail.DataBindings.Add("Text", Me.UnhandledExceptionReport, "UserMail")
            txtUserDescription.DataBindings.Add("Text", Me.UnhandledExceptionReport, "UserComments")
            txtException.Text = .Exceptions
            txtReference.Text = .Assemblies

            ' Add the System-File-Report attachment
            Me.AddAttachment(.FileAttachments(0))
        End With
    End Sub

    ''' <summary>
    ''' Open the attachment that the LinkLabel is pointing on.
    ''' </summary>
    Private Sub attachmentLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Dim FilePath As String = DirectCast(sender, LinkLabel).Text
        Try
            ' Try to open the attachment directly with the System-specified opening method.
            Process.Start(FilePath)
        Catch ex As Exception
            ' If the system does not know how to open the file,
            ' just open the explorer in the specified location and select the file
            ' for the user to open it manually.

            If Not File.Exists(FilePath) Then
                Return
            End If
            ' combine the arguments together
            ' it doesn't matter if there is a space after ','
            Dim argument As String = "/select, " & FilePath

            Process.Start("explorer.exe", argument)
        End Try
    End Sub

    ''' <summary>
    ''' Adds an attachment to the BugReport Object, and also to the LinkLabelList.
    ''' </summary>
    Public Sub AddAttachment(FilePath As String)
        Dim bAttachmentAlreadyIncluded As Boolean = False
        ' Check, if the FilePath is already included -> then just add a link
        For i As Integer = 0 To Me.UnhandledExceptionReport.FileAttachments.Count - 1 Step 1
            If Me.UnhandledExceptionReport.FileAttachments(i) = FilePath Then
                bAttachmentAlreadyIncluded = True
            End If
        Next
        If Not bAttachmentAlreadyIncluded Then
            Me.UnhandledExceptionReport.FileAttachments.Add(FilePath)
        End If

        Dim bAttachmentAlreadyShown As Boolean = False
        ' Add the attachment, if it is not already shown
        For Each C As Control In Me.flpAttachments.Controls
            If TypeOf C Is LinkLabel Then
                If DirectCast(C, LinkLabel).Text = FilePath Then
                    bAttachmentAlreadyShown = True
                End If
            End If
        Next

        If Not bAttachmentAlreadyShown Then
            Dim SysLinkLabel As New LinkLabel()
            SysLinkLabel.Text = FilePath
            AddHandler SysLinkLabel.LinkClicked, AddressOf attachmentLinkLabel_LinkClicked
            SysLinkLabel.Width = flpAttachments.Width - 35
            Me.flpAttachments.Controls.Add(SysLinkLabel)
        End If
    End Sub

    ''' <summary>
    ''' Send the Report.
    ''' </summary>
    Private Sub reportButton_Click(sender As Object, e As EventArgs) Handles btnSendReport.Click
        Hide()
        DialogResult = DialogResult.OK
    End Sub

    ''' <summary>
    ''' Close the window and cancel the Report-sending.
    ''' </summary>
    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Hide()
        DialogResult = DialogResult.Cancel
    End Sub

    ''' <summary>
    ''' Add an additional attachment to the Bug-Report.
    ''' </summary>
    Private Sub btnAddAttachment_Click(sender As Object, e As EventArgs) Handles btnAddAttachment.Click
        Dim fd As New OpenFileDialog
        With fd
            .InitialDirectory = My.Settings.LastSelectedPath
            .Title = My.Resources.rBugReporting.BugReport_SelectAdditionalAttachments
            .Filter = My.Resources.rBugReporting.BugReport_AttachmentFileFilter
            .Multiselect = True
            If .ShowDialog = DialogResult.OK Then
                For Each FileName As String In fd.FileNames
                    If File.Exists(FileName) Then
                        Me.AddAttachment(FileName)
                    End If
                Next
            End If
        End With
    End Sub

End Class