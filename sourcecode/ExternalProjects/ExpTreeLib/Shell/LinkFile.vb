Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text

''' <summary>
''' A Class for reading and writing .lnk files. It is not used by ExpLib_Demo.
''' </summary>
''' <remarks>
''' <pre>
''' This is a slightly modified version of:
''' Filename:     ShellShortcut.vb
''' Author:       Mattias Sjögren (mattias@mvps.org)
'''               http://www.msjogren.net/dotnet/
'''
''' Description:  Defines a .NET friendly class, ShellShortcut, for reading
'''               and writing shortcuts.
''' </pre>
''' </remarks>
Public Class LinkFile
    Implements IDisposable

    Private m_Link As IShellLink
    Private m_Disposed As Boolean = False
    Private m_LinkPath As String
    Private m_IsValidLink As Boolean = False

    Sub New(ByVal fPath As String)
        Dim pf As IPersistFile
        Dim tShellLink As Type
        tShellLink = Type.GetTypeFromCLSID(CLSID_ShellLink)
        m_Link = CType(Activator.CreateInstance(tShellLink), IShellLink)
        If File.Exists(fPath) Then
            pf = CType(m_Link, IPersistFile)
            Dim HR As Integer = pf.Load(fPath, 0)
            If HR = S_OK Then
                m_IsValidLink = True
#If DEBUG Then
            Else
                Marshal.ThrowExceptionForHR(HR)
#End If
            End If
        End If
        m_LinkPath = fPath
    End Sub

#Region "   Dispose"
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        ' Take yourself off of the finalization queue
        ' to prevent finalization code for this object
        ' from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        ' Allow your Dispose method to be called multiple times,
        ' but throw an exception if the object has been disposed.
        ' Whenever you do something with this class, 
        ' check to see if it has been disposed.
        If Not (m_Disposed) Then
            ' If disposing equals true, dispose all managed 
            ' and unmanaged resources.
            m_Disposed = True
            If (disposing) Then
            End If
            ' Release unmanaged resources. If disposing is false,
            ' only the following code is executed. 
            If m_Link Is Nothing Then Exit Sub
            Marshal.ReleaseComObject(m_Link)
            m_Link = Nothing
        Else
            Throw New Exception("DragLink Disposed more than once")
        End If
    End Sub

    ' This Finalize method will run only if the 
    ' Dispose method does not get called.
    ' By default, methods are NotOverridable. 
    ' This prevents a derived class from overriding this method.
    ''' <summary>
    ''' Calls Dispose(False) to ensure release of the IShellLink object
    ''' </summary>
    Protected Overrides Sub Finalize()
        ' Do not re-create Dispose clean-up code here.
        ' Calling Dispose(false) is optimal in terms of
        ' readability and maintainability.
        Dispose(False)
    End Sub
#End Region

#Region "   Public Properties"
    ''' <summary>
    ''' Returns a String containing the Path of the Link Target
    ''' </summary>
    ''' <returns>String containing the Path of the Link Target</returns>
    Public Property LinkTargetPath() As String
        Get
            Dim wfd As WIN32_FIND_DATA
            Dim SB As StringBuilder = New StringBuilder(MAX_PATH)
            Dim HR As Integer
            HR = m_Link.GetPath(SB, SB.Capacity, wfd, SLGP.UNCPRIORITY)
            If HR = S_OK Then
                Return SB.ToString()
            Else
#If DEBUG Then
                Marshal.ThrowExceptionForHR(HR)
#End If
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            Dim HR As Integer = m_Link.SetPath(Value)
            If HR = S_OK Then
            Else
#If DEBUG Then
                Marshal.ThrowExceptionForHR(HR)
#End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Returns True if the file associated with this instance is a Valid Link
    ''' </summary>
    ''' <returns>True if the file associated with this instance is a Valid Link</returns>
    ''' <remarks>Validity is determined by Windows</remarks>
    Public ReadOnly Property IsValidLink() As Boolean
        Get
            Return m_IsValidLink
        End Get
    End Property
#End Region

#Region "   Public Methods"
    ''' <summary>
    ''' Saves a copy of the instance Link File to a different location within the File System
    ''' </summary>
    ''' <param name="TargetPath">Location to Save the Link File</param>
    ''' <returns>True if successful, False otherwise</returns>
    ''' <remarks>It is normally best to use the System Context Menu for this operation</remarks>
    Public Function SaveAs(ByVal TargetPath As String) As Boolean
        SaveAs = True   'errors change this
        Try
            Dim pf As IPersistFile = CType(m_Link, IPersistFile)
            Dim HR As Integer = pf.Save(TargetPath, True)
            If HR = S_OK Then
                HR = pf.SaveCompleted(m_LinkPath)
                If HR <> S_OK Then
#If DEBUG Then
                    Marshal.ThrowExceptionForHR(HR)
#End If
                End If
            Else
                SaveAs = False
#If DEBUG Then
                Marshal.ThrowExceptionForHR(HR)
#End If
            End If
        Catch ex As Exception
            SaveAs = False
#If DEBUG Then
            MsgBox("Error Saving Link -- " & vbCrLf & ex.Message, MsgBoxStyle.OkOnly, "Error on Link Copy/Move")
#End If
        Finally
        End Try
    End Function

    ''' <summary>
    ''' Saves a copy of the instance Link File with a different name to a different location within the File System
    ''' </summary>
    ''' <param name="TargetPath">Location to Save the Link File with a different name</param>
    ''' <returns>True if successful, False otherwise</returns>
    ''' <remarks>It is normally best to use the System Context Menu for this operation</remarks>
    Public Function SaveCopyAs(ByVal TargetPath As String) As Boolean
        SaveCopyAs = True   'Errors change this
        Try
            Dim pf As IPersistFile = CType(m_Link, IPersistFile)
            Dim HR As Integer = pf.Save(TargetPath, False)
            If HR <> S_OK Then
                SaveCopyAs = False
#If DEBUG Then
                Marshal.ThrowExceptionForHR(HR)
#End If
            End If
        Catch ex As Exception
            SaveCopyAs = False
#If DEBUG Then
            MsgBox("Error in SaveCopyAs Link -- " & vbCrLf & ex.Message, MsgBoxStyle.OkOnly, "Error on Link Copy")
#End If
        Finally
        End Try
    End Function
#End Region
End Class
