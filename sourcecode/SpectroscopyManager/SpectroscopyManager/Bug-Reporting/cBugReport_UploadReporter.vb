Imports System.IO
Imports System.Net

Public Class cBugReport_UploadReporter
    Implements iBugReporter(Of cBugReporter.UnhandledExceptionReport)

    Private WithEvents SubmitWorker As New System.ComponentModel.BackgroundWorker

    Private BugReport As cBugReporter.UnhandledExceptionReport

    Private ProgressWindow As New wProgress

    ''' <summary>
    ''' Implemented Report function of the Upload-Reporting Unit.
    ''' Opens the status window and starts the Background-Worker to show
    ''' the user the progress of the submission.
    ''' </summary>
    Public Sub Submit(ByRef report As cBugReporter.UnhandledExceptionReport) _
        Implements iBugReporter(Of SpectroscopyManager.cBugReporter.UnhandledExceptionReport).Submit

        ' Save the object
        Me.BugReport = report

        ' Start the backgroundworker
        Me.SubmitWorker.WorkerReportsProgress = True
        Me.SubmitWorker.RunWorkerAsync()

        ' Open the status form
        Me.ProgressWindow.SetTitle(My.Resources.rBugReporting.Submission_ProgressWindowTitle)
        Me.ProgressWindow.ShowDialog()
    End Sub

    ''' <summary>
    ''' States of the Submission
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum SubmissionStates
        ReportStarted
        ReportFinished
        ZipFileStarted
        ZipFileFinished
        NetworkCheck
        FileUploadStarted
        FileUploadFinished
        SubmissionFinished
        ManualSubmissionNeeded
    End Enum

    ''' <summary>
    ''' Implemented Report function of the Upload-Reporting Unit.
    ''' </summary>
    Public Sub SubmitAsync(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles SubmitWorker.DoWork
        ' FileName for Saving the Report
        Dim ZipReportFileName As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) & _
                                          System.IO.Path.DirectorySeparatorChar & _
                                          Application.ProductName.Replace(" ", "").Replace("'", "") & "_BugReport_" & Now.ToString("ddMMyyyyHHmmss") & ".zip"

        ' Encapsule everything in a TRY-CATCH
        Try
            Me.SubmitWorker.ReportProgress(10, SubmissionStates.ReportStarted)
            ' Create Bug-Report File and open the FileStream
            Dim BugReportFileName As String = Environment.GetEnvironmentVariable("TEMP") & System.IO.Path.DirectorySeparatorChar & "bugreport.html"
            Dim FW As New StreamWriter(BugReportFileName, False)

            Dim ContentLine As String = My.Resources.rBugReporting.BugReport_ContentLine
            ' Write Details to the HTML File:
            FW.WriteLine(My.Resources.rBugReporting.BugReport_HTMLHeader)
            With Me.BugReport
                FW.WriteLine(ContentLine.Replace("%t", "Error Title:").Replace("%c", .Title))
                FW.WriteLine(ContentLine.Replace("%t", "Application:").Replace("%c", .Application))
                FW.WriteLine(ContentLine.Replace("%t", "Date of Error:").Replace("%c", .ErrorDate.ToString()))
                FW.WriteLine(ContentLine.Replace("%t", "User Name:").Replace("%c", .UserName & " (" & .UserMail & ")"))
                FW.WriteLine(ContentLine.Replace("%t", "User Description:").Replace("%c", .UserComments.Replace(vbCrLf, "<br />")))
                FW.WriteLine(ContentLine.Replace("%t", "Exceptions Occurred:").Replace("%c", .Exceptions.Replace(vbCrLf, "<br />")))
                FW.WriteLine(ContentLine.Replace("%t", "Assemblies loaded:").Replace("%c", .Assemblies.Replace(vbCrLf, "<br />")))
            End With
            FW.WriteLine(My.Resources.rBugReporting.BugReport_HTMLFooter)
            ' Close the File Stream
            FW.Close()
            Me.BugReport.FileAttachments.Add(BugReportFileName)
            Me.SubmitWorker.ReportProgress(15, SubmissionStates.ReportFinished)

            ' Delete ZipFile, if it was there before.
            If File.Exists(ZipReportFileName) Then
                File.Delete(ZipReportFileName)
            End If

            Me.SubmitWorker.ReportProgress(20, SubmissionStates.ZipFileStarted)
            ' Zip Attachments together.
            ' Create ZipFile
            Dim zf As New Ionic.Zip.ZipFile(ZipReportFileName)
            zf.AddFiles(Me.BugReport.FileAttachments, "/")
            zf.Save()
            Me.SubmitWorker.ReportProgress(60, SubmissionStates.ZipFileFinished)

            ' Delete the Report-File after zipping
            If File.Exists(BugReportFileName) Then
                File.Delete(BugReportFileName)
            End If
        Catch ex As Exception
            ' An error occured in writing the report.
            MessageBox.Show(My.Resources.rBugReporting.Submission_ErrorOccured.Replace("%e", ex.Message),
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Now comes the Report-Transmission part.
        ' If the connection to the webserver is establishable,
        ' upload the file automatically. If not, ask the user to send
        ' an email manually.
        Me.SubmitWorker.ReportProgress(65, SubmissionStates.NetworkCheck)
        Dim ManualSubmissionNeeded As Boolean = True
        Dim ServerAdress As String = "bugreport.mikeruby.net"
        If My.Computer.Network.Ping(ServerAdress, 1000) Then
            ' Server could be pinged!
            '#########################
            Me.SubmitWorker.ReportProgress(66, SubmissionStates.FileUploadStarted)

            Try
                ' Start the upload to the Upload-URL @ http://mikeruby.net
                Dim UploadURL As String = "http://" & ServerAdress & "/bugreport.php"

                '#######################
                ' PHP-Upload routine!!!
                'Dim boundary As String = IO.Path.GetRandomFileName
                'Dim header As New System.Text.StringBuilder()
                'header.AppendLine("--" & boundary)
                'header.Append("Content-Disposition: form-data; name=""uploaded_file"";")
                'header.AppendFormat("filename=""{0}""", IO.Path.GetFileName(ZipReportFileName))
                'header.AppendLine()
                'header.AppendLine("Content-Type: application/octet-stream")
                'header.AppendLine()

                'Dim headerbytes() As Byte = System.Text.Encoding.UTF8.GetBytes(header.ToString)
                'Dim endboundarybytes() As Byte = System.Text.Encoding.ASCII.GetBytes(vbNewLine & "--" & boundary & "--" & vbNewLine)

                'Dim req As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(UploadURL), HttpWebRequest)
                'req.ContentType = "multipart/form-data; boundary=" & boundary
                'req.ContentLength = headerbytes.Length + New IO.FileInfo(ZipReportFileName).Length + endboundarybytes.Length
                'req.Method = "POST"

                'Dim s As IO.Stream = req.GetRequestStream
                's.Write(headerbytes, 0, headerbytes.Length)
                'Dim filebytes() As Byte = My.Computer.FileSystem.ReadAllBytes(ZipReportFileName)
                's.Write(filebytes, 0, filebytes.Length)
                's.Write(endboundarybytes, 0, endboundarybytes.Length)
                's.Close()

                '########################
                ' ASP.NET Upload Method!
                Dim WC As New WebClient
                With WC
                    ' Set Headers of the UploadClient
                    .Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
                    .Headers.Add(System.Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
                    .Headers.Add(System.Net.HttpRequestHeader.ContentLanguage, "en-gb")
                    Dim Respond As Byte() = .UploadFile(UploadURL, ZipReportFileName)
                    Dim RespondString As String = System.Text.ASCIIEncoding.ASCII.GetString(Respond)

                    ' Check for successfull upload
                    If Not RespondString.Contains("Error Code: 0") Then
                        Throw New Exception("Upload Error!" & vbCrLf & RespondString)
                    End If
                End With
                '########################

                Me.SubmitWorker.ReportProgress(96, SubmissionStates.FileUploadFinished)
                ManualSubmissionNeeded = False
                Me.SubmitWorker.ReportProgress(100, SubmissionStates.SubmissionFinished)
            Catch ex As Exception
                ManualSubmissionNeeded = True
            End Try
        End If

        If ManualSubmissionNeeded Then
            ' Ping failed!
            '##############
            Me.SubmitWorker.ReportProgress(100, SubmissionStates.ManualSubmissionNeeded)

            ' Show the file in the Explorer ...
            Dim argument As String = "/select, " & ZipReportFileName
            Process.Start("explorer.exe", argument)
        End If
    End Sub

    ''' <summary>
    ''' Updates the status messages of during the report submission.
    ''' </summary>
    Private Sub SubmitStatus(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles SubmitWorker.ProgressChanged
        ' Show messages depending on the Submission Status.
        Try
            Dim SubmissionStatus As SubmissionStates = DirectCast(e.UserState, SubmissionStates)
            Dim Message As String = ""

            ' Set Message or the final messagebox.
            Select Case SubmissionStatus
                Case SubmissionStates.ReportStarted
                    Message = My.Resources.rBugReporting.Submission_ReportStarted
                Case SubmissionStates.ReportFinished
                    Message = My.Resources.rBugReporting.Submission_ReportFinished
                Case SubmissionStates.ZipFileStarted
                    Message = My.Resources.rBugReporting.Submission_ZipFileStarted
                Case SubmissionStates.ZipFileFinished
                    Message = My.Resources.rBugReporting.Submission_ZipFileFinished
                Case SubmissionStates.NetworkCheck
                    Message = My.Resources.rBugReporting.Submission_NetworkCheck
                Case SubmissionStates.FileUploadStarted
                    Message = My.Resources.rBugReporting.Submission_FileUploadStarted
                Case SubmissionStates.FileUploadFinished
                    Message = My.Resources.rBugReporting.Submission_FileUploadFinished
                Case SubmissionStates.SubmissionFinished
                    Message = My.Resources.rBugReporting.Submission_SubmissionFinished
                    MessageBox.Show(My.Resources.rBugReporting.Submission_SubmissionFinishedMsg,
                                    "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Case SubmissionStates.ManualSubmissionNeeded
                    Message = My.Resources.rBugReporting.Submission_ManualSubmissionNeededMsg
                    MessageBox.Show(My.Resources.rBugReporting.Submission_ManualSubmissionNeededMsg,
                                    "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Select
            Me.ProgressWindow.PostProgress(e.ProgressPercentage, Message)
        Catch ex As Exception
            Me.ProgressWindow.PostProgress(0, "Status Error")
        End Try
    End Sub

    ''' <summary>
    ''' Finalizes the Report submission after everything is finished.
    ''' </summary>
    Private Sub SubmitFinished(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles SubmitWorker.RunWorkerCompleted
        ' Close the progress window, and, by this,
        ' finish the submission procedure 
        Me.ProgressWindow.Close()
    End Sub
End Class
