Imports System.IO
Imports System.Threading

''' <summary>
''' Locks a file for writing.
''' </summary>
Public Class cFileLock

    ''' <summary>
    ''' Extension applied to the filename of the file to lock.
    ''' </summary>
    Protected Const LockExtension As String = ".lock"

    ''' <summary>
    ''' File to lock
    ''' </summary>
    Protected _FileNameInclPath As String

    ''' <summary>
    ''' Returns the path of the lock-file.
    ''' </summary>
    Protected ReadOnly Property LockFile As String
        Get
            Return Me._FileNameInclPath & LockExtension
        End Get
    End Property

    ''' <summary>
    ''' Timeout to be set
    ''' </summary>
    Protected _TimeoutTimeSpan As TimeSpan


#Region "Constructor"

    ''' <summary>
    ''' Constructor for the lock-timer.
    ''' </summary>
    Public Sub New(ByVal FileNameInclPath As String, ByVal LockTimeout As TimeSpan)
        Me._FileNameInclPath = FileNameInclPath
        Me._TimeoutTimeSpan = LockTimeout
    End Sub

#End Region

#Region "Simple file locking"
    ''' <summary>
    ''' Returns true, if the file-lock could be obtained,
    ''' either by a lock-timeout, or by actually initially settings the lock.
    ''' </summary>
    Public Function TryGetFileLock() As Boolean

        ' Check if file is locked
        If File.Exists(LockFile) Then

            ' Check, if we own the lock:
            If File.ReadAllText(LockFile) = Environment.MachineName Then
                Return True
            End If

            ' If not, check for timeout
            If (Date.UtcNow - File.GetLastWriteTime(LockFile)) < Me._TimeoutTimeSpan Then
                Return False
            End If
        End If

        ' Set the lock
        Try
            File.WriteAllText(LockFile, Environment.MachineName)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function
#End Region

#Region "Simple unlocking"

    ''' <summary>
    ''' Releases the locked file.
    ''' </summary>
    Public Sub ReleaseLock()
        ' Check if file is locked
        If File.Exists(LockFile) Then
            File.Delete(LockFile)
        End If
    End Sub

#End Region

#Region "Multithreaded waiting for a file-lock"

    ''' <summary>
    ''' Mutex to wait for a file-lock to be released.
    ''' </summary>
    Protected WaitForLock_CountOfTries As Integer = 0

    ''' <summary>
    ''' Check once per second for a new lock.
    ''' </summary>
    Protected Const WaitForLock_CheckTime As Integer = 1000

    ''' <summary>
    ''' Set the timeout for the lock to 30 sec.
    ''' </summary>
    Protected Const WaitForLock_Timeout As Integer = 30000

    ''' <summary>
    ''' Gets a file-lock, or waits, until it get's it.
    ''' </summary>
    Public Function GetFileLockOrWait() As Boolean

        Do Until Me.TryGetFileLock

            Thread.Sleep(WaitForLock_CheckTime)
            WaitForLock_CountOfTries += 1

            If (WaitForLock_CountOfTries * WaitForLock_CheckTime) > WaitForLock_Timeout Then
                Return False
            End If

        Loop

        Return True
    End Function

#End Region


End Class
