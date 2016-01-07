Imports System.Security
Imports System.Reflection
Imports System.Text
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Threading

''' <summary>
''' Central Class for a WinForms application to catch any unhandled exception,
''' and send a report to the Author of the Program.
''' </summary>
Public Class cBugReporter

#Region "Initialization"
    ''' <summary>
    ''' Initializes the Bug-Reporting Routine,
    ''' by attaching the bug-reporter to the UnhandledException routine.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Initialize()
#If Not Debug Then
        ' Attach UnhandledException-Handler to the whole AppDomain, as last chance
        ' exception handling, before the app gets closed.
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf cBugReporter.UnhandledDomainExceptionHandler
        ' Attach Unhandled Thread Exception-Handler to the Application,
        ' which is fired, if the CLR decides that the app can continue running after
        ' exception. (ONLY FOR WINFORMS!!!)
        AddHandler Application.ThreadException, AddressOf cBugReporter.UnhandledThreadException
#End If
    End Sub
#End Region

#Region "Report Helper Classes"

    ''' <summary>
    ''' Sub-class that represents an Exception Report.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UnhandledExceptionReport
        Public Sub New()
            Title = [GetType]().Name
        End Sub
        Public Property Title As String
        Public Property Application As String
        Public Property ErrorDate As Date
        Public Property UserName As String
        Public Property UserMail As String
        Public Property UserComments As String
        Public Property Exceptions As String
        Public Property Assemblies As String
        Public Property FileAttachments As New List(Of String)
    End Class

#End Region

#Region "Reporting Function"
    ''' <summary>
    ''' Reports an exception a central place. This would most likely
    ''' find a use in Windows smart clients in the 
    ''' AppDomain.CurrentDomain.UnhandledException handler and the
    ''' Application.ThreadException handler
    ''' </summary>
    ''' <param name="exception">The exception</param>
    ''' <param name="reporter">The logic that takes care of actually reporting the error to the place you want it</param>
    ''' <param name="userComments">User comments about the error</param>
    ''' <param name="showForm">True - show error form and ask user to accept report.
    ''' False - dont show form just report</param>
    Public Shared Sub ReportException(exception As Exception,
                                      reporter As iBugReporter(Of UnhandledExceptionReport),
                                      userComments As String,
                                      showForm As Boolean)
        Dim sb As New StringBuilder()
        Dim a As Assembly = Assembly.GetEntryAssembly()
        Dim unhandledExceptionReport As New UnhandledExceptionReport()
        unhandledExceptionReport.Application = a.FullName

        Dim references As AssemblyName() = a.GetReferencedAssemblies()
        For Each reference As AssemblyName In references
            sb.Append(reference.FullName)
            sb.Append(Environment.NewLine)
        Next

        unhandledExceptionReport.Assemblies = sb.ToString()
        unhandledExceptionReport.ErrorDate = DateTime.Now
        unhandledExceptionReport.Exceptions = exception.ToString()
        unhandledExceptionReport.FileAttachments.Add(GetSystemInfo())
        unhandledExceptionReport.UserComments = userComments
        unhandledExceptionReport.UserName = Environment.UserName
        unhandledExceptionReport.UserMail = String.Empty

        If showForm Then
            Dim form As New wUnhandledExceptionReportForm(unhandledExceptionReport)
            If form.ShowDialog() = DialogResult.OK Then
                reporter.Submit(unhandledExceptionReport)
            End If
        Else
            reporter.Submit(unhandledExceptionReport)
        End If
    End Sub


    ''' <summary>
    ''' Creates a System-Information File.
    ''' Calls the MSINFO32.exe and generates a nfo file. The path of the 
    ''' generated file is returned.
    ''' </summary>
    ''' <returns>Path of generated msinfo32 report file</returns>
    Public Shared Function GetSystemInfo() As String
        Try
            ' retrieve the path to MSINFO32.EXE
            Dim key As Microsoft.Win32.RegistryKey
            key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Shared Tools\MSInfo")
            If key IsNot Nothing Then
                Dim exeFile As String = key.GetValue("Path", "").ToString()
                key.Close()

                If exeFile.Length <> 0 Then
                    Dim processStartInfo As New ProcessStartInfo()
                    processStartInfo.FileName = exeFile
                    processStartInfo.Arguments = "/nfo " & Environment.GetEnvironmentVariable("TEMP") & System.IO.Path.DirectorySeparatorChar & "sysinfo.nfo /categories +systemsummary-Resources-Components-SWEnv-InternetSettings-Apps11+SWEnvEnvVars+SWEnvRunningTasks+SWEnvServices"
                    Dim process__1 As Process = Process.Start(processStartInfo)
                    process__1.WaitForExit()
                End If
            End If
            Return Environment.GetEnvironmentVariable("TEMP") & System.IO.Path.DirectorySeparatorChar & "sysinfo.nfo"
        Catch generatedExceptionName As SecurityException
            ' mostlikely due to not having the correct permissions
            ' to access the registry, ignore this exception
            Return Nothing
        End Try
    End Function
#End Region

#Region "Catcher Functions, that forward the actual exception"
    ''' <summary>
    ''' Handler Function of the Bug-Reporter, for catching Unhandled Exceptions of the CLR.
    ''' Note that in .NET 2.0 unless you set <legacyUnhandledExceptionPolicy enabled="1"/>
    ''' in the app.config file your application will exit no matter what you do here anyway. The purpose of this
    ''' eventhandler is not forcing the application to continue but rather logging 
    ''' the exception and enforcing a clean exit where all the finalizers are run (releasing the resources they hold)
    ''' </summary>
    Public Shared Sub UnhandledDomainExceptionHandler(sender As Object, e As System.UnhandledExceptionEventArgs)
        ' Forward to Report Function
        HandleUnhandledExceptions(e.ExceptionObject)

        ' Prepare cooperative async shutdown from another thread
        Dim t As New Thread(Sub()
                                Environment.Exit(1)
                            End Sub)
        t.Start()
        t.Join()
    End Sub

    ''' <summary>
    ''' Handler function of the Bug-Reporter, for catching Exceptions from a Thread different
    ''' of the UI-Thread.
    ''' The application can continue running after this.
    ''' </summary>
    Public Shared Sub UnhandledThreadException(sender As Object, e As System.Threading.ThreadExceptionEventArgs)
        ' Forward to Report Function
        HandleUnhandledExceptions(e.Exception)
    End Sub

    ''' <summary>
    ''' Actual Function that calls the Reporting Function, and takes the exception as an argument.
    ''' </summary>
    Public Shared Sub HandleUnhandledExceptions(exception As Object)
        Dim e As Exception = TryCast(exception, Exception)
        If Not e Is Nothing Then
            ReportException(e,
                            New cBugReport_UploadReporter,
                            String.Empty,
                            True)
        End If
    End Sub
#End Region

End Class