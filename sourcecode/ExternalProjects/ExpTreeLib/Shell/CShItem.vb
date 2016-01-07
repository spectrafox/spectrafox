Imports System.Collections.Generic
Imports System.IO
Imports System.IO.FileSystemInfo
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI
Imports ExpTreeLib.CShItemUpdater

''' <summary>
''' CShItem is the <b>Primary Class</b> in ExpTreeLib. It is a superset of the .Net Classes FileInfo/DirectoryInfo. CShItem and its' supporting
''' Classes provide all the functionality of the .Net Classes as well as Change Notification, correct Icons for all Items, support for 
''' non-FileSystem Items, and obtains most information about Items more rapidly than the .Net Classes.
''' </summary>
''' <remarks>Creates and maintains an internal cache of Directories and Files that the calling application has expressed an interest in.
'''          The calling application is responsible for explicitly discarding elements from the cache when it no longer has an interest
'''          in them. Normal usage is to retain all Directory entries but explicitly discard the file entries of Directories that are
'''          no longer of interest. 
''' </remarks>
Public Class CShItem
    Implements IDisposable, IComparable

#Region "   Shared Private Fields"
    'This class has occasion to refer to the TypeName as reported by
    ' SHGetFileInfo. It needs to compare this to the string
    ' (in English) "System Folder"
    'on non-English systems, we do not know, in the general case,
    ' what the equivalent string is to compare against
    'The following variable is set by Sub New() to the string that
    ' corresponds to "System Folder" on the current machine
    ' Sub New() depends on the existance of My Computer(CSIDL.DRIVES),
    ' to determine what the equivalent string is
    Private Shared m_strSystemFolder As String

    'My Computer is also commonly used (though not internally),
    ' so save & expose its name on the current machine
    Private Shared m_strMyComputer As String

    'To get My Documents sorted first, we need to know the Locale 
    'specific name of that folder.
    Private Shared m_strMyDocuments As String

    ' The DesktopBase is set up via Sub New() (one time only) and
    '  disposed of only when DesktopBase is finally disposed of
    Private Shared DesktopBase As CShItem

    ' It is also useful to know if the OS is XP or above.  
    ' Set up in Sub New() to avoid multiple calls to find this info
    Private Shared XPorAbove As Boolean
    ' Likewise if OS is Win2K or Above
    Private Shared Win2KOrAbove As Boolean
    ' Likewise if OS is Vista or Above
    Private Shared VistaOrAbove As Boolean

    ' DragDrop, possibly among others, needs to know the Path of
    ' the DeskTopDirectory in addition to the Desktop itself
    ' Also need the actual CShItem for the DeskTopDirectory, so get it
    Private Shared m_DeskTopDirectory As CShItem

    ''' <summary>
    ''' The CShItem of the Recycle Bin. Set in New() (the Desktop creator)
    ''' Used to prevent UPDATEDIR on this Item from processing.
    ''' Als used to prevent normal UPDATEDIR on Desktop from processing the
    ''' Recycle Bin which would cause an effectively endless loop.
    ''' </summary>
    Private Shared m_Recycle As CShItem            '6/21/2012

    ' Keep the local System Name for IsRemote testing
    Private Shared SystemName As String                              '4/14/2012
    ' Keep list of Drives and their DriveType for IsRemote testing
    Private Shared DriveDict As New Dictionary(Of String, Boolean)   '4/16/2012

    ''' <summary>
    ''' LockObj is used for locking critical updating blocks of code
    ''' that reference the Shared Directory Tree of CShItems.  In the
    ''' normal case, it will not actually lock the block of code since
    ''' Most (all?) updating is done in the main thread. HOWEVER, empirical evidence
    ''' suggests that if multiple code paths of the SAME thread are in play 
    ''' as a byproduct of overriding WndProc for Notification messages, the
    ''' SyncLock LockObj statement will allow pending messages to be processed.
    ''' This effectively causes messages to be processed in reverse order of receipt.
    ''' This is a bit funky, but is at least made predictible by the SyncLock.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared LockObj As New Object

#End Region

#Region "   Instance Private Fields"
    'm_Folder and m_Pidl must be released/freed at Dispose time
    Private m_Folder As IShellFolder    'if item is a folder, contains the Folder interface for this instance
    Private m_Pidl As IntPtr            'The Absolute PIDL for this item (not retained for files)
    Private m_DisplayName As String = ""
    Private m_Path As String
    Private m_TypeName As String
    Private m_Parent As CShItem
    Private m_IconIndexNormal As Integer = -1        'index into the SystemImageListManager list for Normal icon
    Private m_IconIndexOpen As Integer = -1          'index into the SystemImageListManager list for Open icon
    Private m_IconIndexNormalOrig As Integer = -1    'index into the System Image list for Normal icon
    Private m_IconIndexOpenOrig As Integer = -1      'index into the SystemImage list for Open icon
    Private m_IsBrowsable As Boolean
    Private m_IsFileSystem As Boolean
    Private m_IsFolder As Boolean
    Private m_HasSubFolders As Boolean
    Private m_IsLink As Boolean
    Private m_IsDisk As Boolean
    Private m_IsShared As Boolean
    Private m_IsHidden As Boolean
    Private m_IsNetWorkDrive As Boolean
    Private m_IsRemovable As Boolean
    Private m_IsReadOnly As Boolean
    'Properties of interest to Drag Operations
    Private m_CanMove As Boolean
    Private m_CanCopy As Boolean
    Private m_CanDelete As Boolean
    Private m_CanLink As Boolean
    Private m_IsDropTarget As Boolean
    Private m_CanRename As Boolean

    Private m_Directories As CShItemCollection
    Private m_Files As CShItemCollection

    Private m_SFGAO_Attributes As SFGAO     'the original, returned from GetAttributesOf Added 10/09/2011 
    Private m_IsRemote As Boolean           '4/14/2012

    Private m_Tag As Object                 'Added 10/09/2011
    Private m_W32Data As W32Find_Data       '4/24/2012

    Private m_SortFlag As Integer       'Used in comparisons

    'For shell events 
    Private m_updater As CShItemUpdater

    'The following elements are only filled in on demand
    Private m_XtrInfo As Boolean
    Private m_LastWriteTime As DateTime
    Private m_CreationTime As DateTime
    Private m_LastAccessTime As DateTime
    Private m_Length As Long
    Private m_Attributes As FileAttributes  'Added 10/09/2011 'True FileAttributes from FileInfo

    'Indicates whether DisplayName, TypeName, SortFlag have been set up
    Private m_HasDispType As Boolean

    'Indicates whether IsReadOnly has been set up
    Private m_IsReadOnlySetup As Boolean '

    'm_UpdateFolder is True is the IShellFolder (m_Folder) must be refetched
    Private m_UpdateFolder As Boolean

    'Holds a byte() representation of m_PIDL -- filled when needed
    Private m_cPidl As cPidl

    'Flags for Dispose state
    Private m_IsDisposing As Boolean
    Private m_Disposed As Boolean


#End Region

#Region "   Destructor"
    ''' <summary>
    ''' Summary of Dispose.
    ''' </summary>
    ''' 
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        ' Take yourself off of the finalization queue
        ' to prevent finalization code for this object
        ' from executing a second time.
        GC.SuppressFinalize(Me)
    End Sub
    ''' <summary>
    ''' Deallocates CoTaskMem contianing m_Pidl and removes reference to m_Folder
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' 
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
            If Not IsNothing(m_Folder) Then
                Marshal.ReleaseComObject(m_Folder)
                m_Folder = Nothing
            End If
            If Not m_Pidl.Equals(IntPtr.Zero) Then
                Marshal.FreeCoTaskMem(m_Pidl)
                m_Pidl = IntPtr.Zero
            End If
        Else
            Throw New Exception("CShItem Disposed more than once")
        End If
    End Sub

    ' This Finalize method will run only if the 
    ' Dispose method does not get called.
    ' By default, methods are NotOverridable. 
    ' This prevents a derived class from overriding this method.
    ''' <summary>
    ''' Summary of Finalize.
    ''' </summary>
    ''' 
    Protected Overrides Sub Finalize()
        ' Do not re-create Dispose clean-up code here.
        ' Calling Dispose(false) is optimal in terms of
        ' readability and maintainability.
        Dispose(False)
    End Sub

#End Region

#Region "   Constructors"

#Region "   Private Sub New(ByVal pidl As IntPtr, ByVal parent as CShItem)"
    Friend Sub New(ByVal pidl As IntPtr, ByVal parent As CShItem)
        If IsNothing(DesktopBase) Then
            DesktopBase = New CShItem() 'This initializes the Desktop folder
        End If
        m_Parent = parent
        m_Pidl = concatPidls(parent.PIDL, pidl)

        'Get some attributes
        SetUpAttributes(parent.Folder, pidl)

        'Set unfetched value for IconIndex....
        m_IconIndexNormal = -1
        m_IconIndexOpen = -1
        'finally, set up my Folder
        If m_IsFolder Then
            m_Folder = GetFolder(parent, pidl)
            'm_Folder may be returned as Nothing. This is handled in GetContents
        End If
    End Sub

#End Region

    ''' <summary>
    ''' GetFolder returns the IShellFolder interface of the Folder designated by the input Parent and 
    ''' relative PIDL.
    ''' </summary>
    ''' <param name="parent">The CShItem of the Folder containing the folder for which the 
    ''' IShellFolder interface is desired.</param>
    ''' <param name="relPidl">The relative Pidl of the folder for which the interface is desired.</param>
    ''' <returns>The desired interface or Nothing if error.</returns>
    ''' <remarks></remarks>
    ''' 
    Private Shared Function GetFolder(ByVal parent As CShItem, ByVal relPidl As IntPtr) As IShellFolder
        Dim ptr As IntPtr
        Dim rVal As IShellFolder = Nothing
        Dim HR As Integer = parent.Folder.BindToObject(relPidl, IntPtr.Zero, IID_IShellFolder, ptr)
        'If HR = S_OK Then                              'Old code
        If HR >= S_OK AndAlso ptr <> IntPtr.Zero Then   'New code (12/12/09)
            'The ASUS fix is slightly modified from its' original as per a suggestion from Calum 4/8/2010
            Try                                                     'ASUS Fix
                rVal = Marshal.GetTypedObjectForIUnknown(ptr, GetType(IShellFolder))
#If DEBUG Then
            Catch ex As Exception                                   'ASUS Fix - modified 11/13/2013 - was InvalidCastException
                Debug.WriteLine("GetFolder: " & ex.Message)         'ASUS Fix
                Throw ex                                            'ASUS Fix
#End If
            Finally                                                 'ASUS Fix
                Marshal.Release(ptr) 'Must do this in all cases
            End Try                                                 'ASUS Fix
        Else
            If ptr <> IntPtr.Zero Then Marshal.Release(ptr) 'Added Code (12/12/09)
#If DEBUG Then
            DumpPidl(relPidl)
            Marshal.ThrowExceptionForHR(HR)    'Removed 10/22/2011 - restored 11/13/2013
#End If
        End If
        Return rVal
    End Function

#Region "      Private Sub New()"
    ''' <summary>
    ''' Private Constructor. Creates CShItem of the Desktop
    ''' </summary>
    Private Sub New()           'only used when desktopfolder has not been intialized
        If Not IsNothing(DesktopBase) Then
            Throw New Exception("Attempt to initialize CShItem for second time")
        End If

        Dim HR As Integer
        'firstly determine what the local machine calls a "System Folder" and "My Computer"
        Dim tmpPidl As IntPtr
        HR = SHGetSpecialFolderLocation(0, CSIDL.DRIVES, tmpPidl)
        Dim shfi As New SHFILEINFO()
        Dim dwflag As SHGFI = SHGFI.DISPLAYNAME Or _
                                SHGFI.TYPENAME Or _
                                SHGFI.PIDL
        Dim dwAttr As Integer = 0
        SHGetFileInfo(tmpPidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_strSystemFolder = shfi.szTypeName
        m_strMyComputer = shfi.szDisplayName
        Marshal.FreeCoTaskMem(tmpPidl)
        'set OS version info
        XPorAbove = ShellDll.ShellAPI.IsXpOrAbove()
        Win2KOrAbove = ShellDll.ShellAPI.Is2KOrAbove
        VistaOrAbove = ShellDll.ShellAPI.IsVistaOrAbove

        'With That done, now set up Desktop CShItem
        m_Path = "::{" & DesktopGUID.ToString & "}"
        m_IsFolder = True
        m_HasSubFolders = True
        m_IsBrowsable = False
        HR = SHGetDesktopFolder(m_Folder)
        m_Pidl = GetSpecialFolderLocation(IntPtr.Zero, CSIDL.DESKTOP)
        dwflag = SHGFI.DISPLAYNAME Or _
                 SHGFI.TYPENAME Or _
                 SHGFI.SYSICONINDEX Or _
                 SHGFI.PIDL
        dwAttr = 0
        Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_DisplayName = shfi.szDisplayName
        m_TypeName = strSystemFolder   'not returned correctly by SHGetFileInfo
        m_IconIndexNormal = shfi.iIcon
        m_IconIndexOpen = shfi.iIcon
        m_HasDispType = True
        m_IsDropTarget = True
        m_IsReadOnly = False
        m_IsReadOnlySetup = True

        'also get local name for "My Documents"
        Dim pchEaten As Integer
        tmpPidl = IntPtr.Zero
        HR = Me.Folder.ParseDisplayName(Nothing, Nothing, "::{450d8fba-ad25-11d0-98a8-0800361b1103}", _
                 pchEaten, tmpPidl, Nothing)
        shfi = New SHFILEINFO()
        dwflag = SHGFI.DISPLAYNAME Or _
                                SHGFI.TYPENAME Or _
                                SHGFI.PIDL
        dwAttr = 0
        SHGetFileInfo(tmpPidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_strMyDocuments = shfi.szDisplayName
        Marshal.FreeCoTaskMem(tmpPidl)
        'this must be done after getting "My Documents" string
        m_SortFlag = ComputeSortFlag()
        'Set DesktopBase
        DesktopBase = Me
        'Get the SystemName for Remote item testing
        SystemName = Environment.MachineName    '4/14/2012
        ' Get the Path and CShItem of the DesktopDirectory
        m_DeskTopDirectory = GetCShItem(CSIDL.DESKTOPDIRECTORY)
        ' Get the CShItem for the Recycle Bin   6/21/2012
        m_Recycle = GetCShItem(CSIDL.BITBUCKET) '6/21/2012
        ' Start the Notification Process
        m_updater = New CShItemUpdater(Me)
    End Sub
#End Region

#Region "       Utility functions"
    '#Region "       IsPidlEmpty"           'This function no longer used - 5/26/2012
    '    ''' <summary>
    '    ''' Returns True if input PIDL is equal to IntPtr.Zero or if PIDL contains fewer than 2 bytes.
    '    ''' </summary>
    '    ''' <param name="pidl">PIDL to be tested.</param>
    '    ''' <returns>True if input PIDL is equal to IntPtr.Zero or if PIDL contains fewer than 2 bytes.</returns>
    '    ''' <remarks>Will return True when input PIDL is the Desktop.</remarks>
    '    Friend Shared Function IsPidlEmpty(ByVal pidl As IntPtr) As Boolean
    '        If pidl = IntPtr.Zero Then
    '            Return True
    '        End If

    '        Dim bytes As Byte() = New Byte(1) {}
    '        Marshal.Copy(pidl, bytes, 0, 2)
    '        Dim size As Integer = bytes(0) + bytes(1) * 256
    '        Return (size <= 2)
    '    End Function
    '#End Region

#Region "       IsValidPidl"
    '''<summary>It is impossible to validate a PIDL completely since its contents
    ''' are arbitrarily defined by the creating Shell Namespace.  However, it
    ''' is possible to validate the structure of a PIDL.</summary>
    ''' <returns>True if input Byte() contains a valid PIDL structure, False Otherwise</returns>
    Public Shared Function IsValidPidl(ByVal b() As Byte) As Boolean
        IsValidPidl = False     'assume failure
        Dim bMax As Integer = b.Length - 1   'max value that index can have
        If bMax < 1 Then Exit Function 'min size is 2 bytes
        Dim cb As Integer = b(0) + (b(1) * 256)
        Dim indx As Integer = 0
        Do While cb > 0
            If (indx + cb + 1) > bMax Then Exit Function 'an error
            indx += cb
            cb = b(indx) + (b(indx + 1) * 256)
        Loop
        ' on fall thru, it is ok as far as we can check
        IsValidPidl = True
    End Function
#End Region

#Region "       MakeFolderFromBytes"
    ''' <summary>
    ''' Given a Byte() containing a valid PIDL of a Folder, return the IShellFolder of that Folder
    ''' </summary>
    ''' <param name="b">Byte() containing a valid PIDL of a Folder</param>
    ''' <returns>The IShellFolder for the requested PIDL. If Byte() does not contain a valid PIDL of a Folder, return Nothing</returns>
    Public Shared Function MakeFolderFromBytes(ByVal b As Byte()) As ShellDll.IShellFolder
        GetDeskTop()                        'ensure we are initialized
        MakeFolderFromBytes = Nothing       'get rid of VS2005 warning
        If Not IsValidPidl(b) Then Return Nothing
        If b.Length = 2 AndAlso ((b(0) = 0) And (b(1) = 0)) Then 'this is the desktop
            Return DesktopBase.Folder
        ElseIf b.Length = 0 Then   'Also indicates the desktop
            Return DesktopBase.Folder
        Else
            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(b.Length)
            If ptr.Equals(IntPtr.Zero) Then Return Nothing
            Marshal.Copy(b, 0, ptr, b.Length)
            'the next statement assigns a IshellFolder object to the function return, or has an error
            MakeFolderFromBytes = GetFolder(DesktopBase, ptr)
            Marshal.FreeCoTaskMem(ptr)
        End If
    End Function
#End Region

#Region "       GetParentOf"

    '''<summary>Returns both the IShellFolder interface of the parent folder
    '''  and the relative pidl of the input PIDL</summary>
    '''<remarks>Several internal functions need this information and do not have
    ''' it readily available. GetParentOf serves those functions</remarks>
    Private Shared Function GetParentOf(ByVal pidl As IntPtr, ByRef relPidl As IntPtr) As IShellFolder
        GetParentOf = Nothing     'avoid VB2005 warning
        Dim HR As Integer
        Dim itemCnt As Integer = PidlCount(pidl)
        If itemCnt = 1 Then         'parent is desktop
            HR = SHGetDesktopFolder(GetParentOf)
            relPidl = pidl
        Else
            Dim tmpPidl As IntPtr
            tmpPidl = TrimPidl(pidl, relPidl)
            GetParentOf = GetFolder(DesktopBase, tmpPidl)
            Marshal.FreeCoTaskMem(tmpPidl)
        End If
        If Not HR = NOERROR Then Marshal.ThrowExceptionForHR(HR)
    End Function
#End Region

#Region "       SetUpAttributes"
    ''' <summary>Get the base attributes of the folder/file that this CShItem represents</summary>
    ''' <param name="folder">Parent Folder of this Item</param>
    ''' <param name="pidl">Relative Pidl of this Item.</param>
    ''' 
    Private Sub SetUpAttributes(ByVal folder As IShellFolder, ByVal pidl As IntPtr)
        Dim attrFlag As SFGAO
        attrFlag = SFGAO.BROWSABLE                  'D
        attrFlag = attrFlag Or SFGAO.FILESYSTEM     'FD
        'attrFlag = attrFlag Or SFGAO.HASSUBFOLDER   'D  'made into an on-demand attribute
        attrFlag = attrFlag Or SFGAO.FOLDER
        attrFlag = attrFlag Or SFGAO.LINK           'F
        attrFlag = attrFlag Or SFGAO.SHARE          'FD
        attrFlag = attrFlag Or SFGAO.HIDDEN         'FD
        attrFlag = attrFlag Or SFGAO.REMOVABLE
        'attrFlag = attrFlag Or SFGAO.RDONLY   'made into an on-demand attribute
        attrFlag = attrFlag Or SFGAO.CANCOPY
        attrFlag = attrFlag Or SFGAO.CANDELETE
        attrFlag = attrFlag Or SFGAO.CANLINK
        attrFlag = attrFlag Or SFGAO.CANMOVE
        attrFlag = attrFlag Or SFGAO.DROPTARGET
        attrFlag = attrFlag Or SFGAO.CANRENAME      'FD
        attrFlag = attrFlag Or SFGAO.STREAM         'F
        'Note: for GetAttributesOf, we must provide an array, in  all cases with 1 element
        Dim aPidl(0) As IntPtr
        aPidl(0) = pidl
        folder.GetAttributesOf(1, aPidl, attrFlag)
        m_SFGAO_Attributes = attrFlag
        m_IsBrowsable = CBool(attrFlag And SFGAO.BROWSABLE)
        m_IsFileSystem = CBool(attrFlag And SFGAO.FILESYSTEM)
        'm_HasSubFolders = CBool(attrFlag And SFGAO.HASSUBFOLDER)  'made into an on-demand attribute
        m_IsFolder = CBool(attrFlag And SFGAO.FOLDER)
        m_IsLink = CBool(attrFlag And SFGAO.LINK)
        m_IsShared = CBool(attrFlag And SFGAO.SHARE)
        m_IsHidden = CBool(attrFlag And SFGAO.HIDDEN)
        m_IsRemovable = CBool(attrFlag And SFGAO.REMOVABLE)
        'm_IsReadOnly = CBool(attrFlag And SFGAO.RDONLY)      'made into an on-demand attribute
        m_CanCopy = CBool(attrFlag And SFGAO.CANCOPY)
        m_CanDelete = CBool(attrFlag And SFGAO.CANDELETE)
        m_CanLink = CBool(attrFlag And SFGAO.CANLINK)
        m_CanMove = CBool(attrFlag And SFGAO.CANMOVE)
        m_IsDropTarget = CBool(attrFlag And SFGAO.DROPTARGET)
        m_CanRename = CBool(attrFlag And SFGAO.CANRENAME)

        'Get the Path
        SetPath()
        'check for zip file = folder on xp, leave it a file
        If m_IsFolder AndAlso m_IsFileSystem AndAlso XPorAbove Then
            'If (m_Attributes = (m_Attributes And SFGAO.STREAM)) Then
            If CBool(attrFlag And SFGAO.STREAM) Then   'in this case, it is not a Folder, but a .zip or .cab or etc
                m_IsFolder = False
            End If
        End If
        If m_IsFolder AndAlso m_Path.Length = 3 AndAlso m_Path.Substring(1).Equals(":\") Then
            m_IsDisk = True
            Try                 '04/16/2012 Entire Try Block
                Dim disk As New Management.ManagementObject("win32_logicaldisk.deviceid=""" & _
                                               Me.Path.Substring(0, 2) & """")
                m_Length = CType(disk("Size"), UInt64).ToString
                If CType(disk("DriveType"), UInt32).ToString = CStr(4) Then
                    m_IsNetWorkDrive = True
                    m_IsRemote = True
                End If
            Catch ex As Exception
                'Disconnected Network Drives etc. will generate 
                'an error here, just assume that it is a network
                'drive
                m_IsNetWorkDrive = True
                m_IsRemote = True
            Finally
                m_XtrInfo = True
                If Not DriveDict.ContainsKey(m_Path) Then
                    DriveDict.Add(m_Path, m_IsRemote)
                End If
            End Try
        End If

        'Setup IsRemote             '4/14/2012
        'Reworked 5/15/2012 when testing discovered that contrary to the Docs, IO.Path.GetPathRoot(m_Path)
        ' will throw an exception when presented with a long path that GetDisplayNameOf made legal by
        ' using 8.3 names for some of the directories! IO.Path.GetPathRoot is not supposed to do anything to
        ' reference the actual components of the Path. It should be strictly String manipulation!
        ' Error on Path = "C:\Testing\XXXXXA~1\YYYYYY~1\ABCDEF~1\ZZZZZZ~1\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789012345678901234.txt"
        ' which is only 138 chars long.
        If Not (m_IsDisk OrElse m_Path.StartsWith("::")) Then
            If m_Path.StartsWith("\\") Then
                Dim tmp() As String = m_Path.Split(New Char() {"\"}, StringSplitOptions.RemoveEmptyEntries)
                If tmp.Length > 0 AndAlso tmp(0).Equals(SystemName, StringComparison.InvariantCultureIgnoreCase) Then
                    m_IsRemote = False
                Else
                    m_IsRemote = True
                End If
            Else
                If m_Path.Length > 2 AndAlso m_Path.Substring(1, 2).Equals(":\") Then
                    Dim itemroot As String = m_Path.Substring(0, 3)
                    If DriveDict.ContainsKey(itemroot) AndAlso DriveDict(itemroot) Then m_IsRemote = True
                End If
            End If
        End If
    End Sub

#End Region

#Region "       SetPath"
    ''' <summary>
    ''' Sets m_Path to the Full Path of the current Item.
    ''' </summary>
    ''' <remarks>Reworked 11/13/3013 to deal with the case of folder.GetDisplayNameOf returning an error.<br />
    '''          This can occur for incompletely implemented or otherwise corrupt Shell Extension Folders.<br />
    '''          All CShItem constructors will call SetUpAttributes which will call SetPath. Effectively all
    '''          CShItem constructors will be called by GetContents. 
    '''          GetContents will deal with the exceptions that might be thrown here by simply not inserting the
    '''          faulting CShItem into the internal tree. Since the CShItem is not in the tree, no change 
    '''          notification will be called for the Item.<br />
    '''          A Move of a file/folder from a known Folder to a faulty Folder will cause the moved item to 
    '''          disappear from its' original location and not appear anywhere else.
    ''' </remarks>
    Private Sub SetPath()
        'Get the Path
        'Debug.WriteLine("SetPath:" & Me.Parent.DisplayName & " Parent Folder = " & Me.Parent.ToString & " Parent Path = " & Me.Parent.Path)
        Dim folder As IShellFolder = Me.Parent.Folder
        Dim strr As IntPtr = Marshal.AllocCoTaskMem(MAX_PATH * 2 + 4)
        Try
            Dim pidl As IntPtr = ILFindLastID(m_Pidl)
            'If CLng(pidl) - CLng(m_Pidl) < 0 Then
            'Debug.WriteLine("pidl - m_pidl = " & pidl.ToString & " - " & m_Pidl.ToString & " = " & (CLng(pidl) - CLng(m_Pidl)).ToString)
            'End If
            Marshal.WriteInt32(strr, 0, 0)
            Dim buf As New StringBuilder(MAX_PATH)
            Dim itemflags As SHGDN = SHGDN.FORPARSING
            Dim HR As Integer = folder.GetDisplayNameOf(pidl, itemflags, strr)
            If HR = S_OK Then
                HR = StrRetToBuf(strr, pidl, buf, MAX_PATH)
                If HR = NOERROR Then
                    m_Path = buf.ToString
                Else
                    Marshal.ThrowExceptionForHR(HR)
                End If
            Else
                Marshal.ThrowExceptionForHR(HR)
            End If
            'Debug.WriteLine(m_Path)
        Catch ex As Exception
            'Debug.WriteLine("SetPath: Exception")
            'Debug.WriteLine(ex.ToString)
            'Debug.WriteLine("SetPath m_Pidl:")
            'DumpPidl(m_Pidl)
            m_Path = "Unknown"
            Throw ex                '11/14/2013
        Finally
            If strr <> IntPtr.Zero Then Marshal.FreeCoTaskMem(strr)
        End Try
    End Sub
#End Region

#End Region

#Region "       BrowseTo Item"
    ''' <summary>
    ''' BrowseTo locates the desired item and places it in its proper location on the internal tree.
    ''' Any and all sub-directories that need to be populated in the tree in order to properly place
    ''' the desired item, are populated. This is the programatic equivalent of Browsing to a node in <code>ExpTree's</code> TreeView.<br />
    ''' BrowseTo also returns the Parent CShItem. 
    ''' If the desired CShItem does not exist, the returned Parent is the CShItem that would be the
    ''' Immediate ancestor (containing CShItem or Parent) of the desired item should it be created.
    ''' </summary>
    ''' <param name="absPidl">A Absolute PIDL whose CShItem is to be found</param>
    ''' <param name="Parent">Output parameter -- Immediate Ancestor CShItem of the found item OR 
    ''' the CShItem that would contain the item if it existed OR Nothing if NO Immediate ancestor found in the Shell namespace. </param>
    ''' <returns>The desired CShItem or, if not found, Nothing.</returns>
    ''' <remarks>A by-product of this search is that any sub-dirs of the tree along the path will be 
    ''' populated with their sub directories.
    ''' It is logically possible that NO Immediate ancestor can be found.
    ''' For Example: GetCShItem(Path) may be given a string specifying a non-existant directory.
    ''' (eg -- C:\Test\NonExistant\junk.txt). 
    ''' In that case, and that case only, Parent may be returned as Nothing.</remarks>
    Friend Shared Function BrowseTo(ByVal absPidl As IntPtr, <Out()> ByRef Parent As CShItem) As CShItem
        BrowseTo = Nothing     'avoid VB2005 Warning
        Dim BaseItem As CShItem = GetDeskTop()

        Dim CSI As CShItem
        Dim FoundIt As Boolean = False      'True if we found item or an ancestor
        'Dim FirstWithThisBase As Boolean = True     '6/30/2012 Flag to prevent infinite loop
        Do Until FoundIt
            For Each CSI In BaseItem.Directories    '7/2/2012 should use Directories here
                If IsAncestorOf(CSI.PIDL, absPidl) Then
                    If CShItem.IsEqual(CSI.PIDL, absPidl) Then  'we found the desired item
                        Parent = BaseItem
                        Return CSI
                    Else            'Found an ancestor
                        BaseItem = CSI
                        Parent = CSI
                        FoundIt = True
                        Exit For
                    End If
                End If
            Next
            If Not FoundIt Then
                'UPDATE: Check for files in the desktop
                For Each CSI In DesktopBase.Files           'Files will do an UpdateRefresh in case of missing a CREATE
                    If CShItem.IsEqual(CSI.PIDL, absPidl) Then
                        Parent = DesktopBase
                        Return CSI
                    End If
                Next
                'The next block of code is to deal with a rare case of missing a MKDIR - 6/30/2012
                'No longer necessary since BaseItem.Directories above will do an UpdateRefresh
                'If FirstWithThisBase Then
                '    FirstWithThisBase = False
                '    Debug.WriteLine("***Bingo")
                '    BaseItem.UpdateRefresh(False, True)
                '    Continue Do
                'End If
                Parent = Nothing        'didn't find an ancestor
                Return Nothing
            End If
            'The complication is that the desired item may not be a directory
            If Not IsAncestorOf(BaseItem.PIDL, absPidl, True) Then  'Don't have immediate ancestor
                'FirstWithThisBase = True    '6/30/2012
                FoundIt = False     'go around again
            Else
                Parent = BaseItem
                For Each CSI In BaseItem.Directories        '6/6/2012 modified 7/2/2012 Directories needed here
                    If CShItem.IsEqual(CSI.PIDL, absPidl) Then
                        Return CSI
                    End If
                Next
                'Not in Dirs, so look in Files 6/6/2012 fix
                For Each CSI In BaseItem.Files              'Files will do an UpdateRefresh in case of missing a CREATE
                    If CShItem.IsEqual(CSI.PIDL, absPidl) Then
                        Return CSI
                    End If
                Next
                'fall thru here means it doesn't exist or we can't find it because of funny PIDL from SHParseDisplayName
                Return Nothing
            End If
        Loop
    End Function
#End Region

#Region "       GetCShItem --- various signatures of GetCshItem"

    ''' <summary>Given an IntPtr representation of a PIDL,
    ''' GetCshItem finds or creates a CShItem and places any new CShItem into the internal tree.
    ''' The tree is expanded (filled in) as necessary to locate the CShItem or to locate the proper
    ''' placement of a new Item. The assumption is that the Folder system actually contains the item
    ''' that is requested -- File or Directory.Exists equivalent. Returns Nothing on errors such as
    ''' non-existant item.
    ''' </summary>
    ''' <param name="pidl">Absolute (Full) Pidl of item to be Found or Created</param>
    ''' <returns>A CShItem or, in case of error, Nothing</returns>
    Friend Shared Function GetCShItem(ByVal pidl As IntPtr) As CShItem
        Dim Parent As CShItem = Nothing
        GetCShItem = BrowseTo(pidl, Parent)
        If IsNothing(GetCShItem) Then
            If Not IsNothing(Parent) Then
                Try
                    GetCShItem = New CShItem(ILFindLastID(pidl), Parent)
                Catch
                    GetCShItem = Nothing
                End Try
            End If
        End If
    End Function

    ''' <summary>Given a Full Path in a String,
    ''' GetCshItem finds or creates a CShItem and places any new CShItem into the internal tree.
    ''' The tree is expanded (filled in) as necessary to locate the CShItem or to locate the proper
    ''' placement of a new Item. The assumption is that the Folder system actually contains the item
    ''' that is requested -- File or Directory.Exists equivalent. Returns Nothing on errors such as
    ''' non-existant item.
    ''' </summary>
    ''' <param name="path">The Full Path of the desired CShItem</param>
    ''' <returns>A CShItem or, in case of error, Nothing</returns>
    Public Shared Function GetCShItem(ByVal path As String) As CShItem
        GetCShItem = Nothing    'assume failure
        Dim HR As Integer
        Dim tmpPidl As IntPtr
        HR = GetDeskTop.Folder.ParseDisplayName(0, IntPtr.Zero, path, 0, tmpPidl, 0)
        If HR = 0 Then
            GetCShItem = GetCShItem(tmpPidl)
        End If
        If Not tmpPidl.Equals(IntPtr.Zero) Then
            Marshal.FreeCoTaskMem(tmpPidl)
        End If
    End Function

    ''' <summary>Given a CSIDL,
    ''' GetCshItem finds or creates a CShItem and places any new CShItem into the internal tree.
    ''' The tree is expanded (filled in) as necessary to locate the CShItem or to locate the proper
    ''' placement of a new Item. The assumption is that the Folder system actually contains the item
    ''' that is requested -- File or Directory.Exists equivalent. Returns Nothing on errors such as
    ''' non-existant item.
    ''' </summary>
    ''' <param name="ID"></param>
    ''' <returns>A CShItem or, in case of error, Nothing</returns>
    Public Shared Function GetCShItem(ByVal ID As CSIDL) As CShItem
        GetCShItem = Nothing      'avoid VB2005 Warning
        If ID = CSIDL.DESKTOP Then
            Return GetDeskTop()
        End If
        Dim HR As Integer
        Dim tmpPidl As IntPtr  'original code - retain
        'MYDOCUMENTS - the saga continues
        'In Vista and above, My Documents does not live immediately under the Desktop
        ' (is not a member of DesktopBase.Directories)
        'Therefore, without special handling, this rtn will return Nothing as the 
        'CShItem when CSIDL.MYDOCUMENTS is requested.
        'MS Documentation states that in Shell32.dll version 6.0 and above CSIDL_MYDOCUMENTS is 
        'Equivalent to CSIDL_PERSONAL. (6.0 = XP, 6.01 = Vista, 6.1 = Win7)
        'In XP, the PIDLs of PERSONAL and MYDOCUMENTS are Identical. In Vista and Win7, they are not.
        'In all OSes, the PIDL for MYDOCUMENTS has 1 item. In Vista and Win7, the PIDL for PERSONAL is a 
        'two item PIDL, which correctly reflects the location of the corresponding Folder in the directory tree.
        'Because of this, in Vista and above, I must use PERSONAL as the lookup CSIDL to obtain MYDOCUMENTS.

        If ID = CSIDL.MYDOCUMENTS AndAlso VistaOrAbove Then ID = CSIDL.PERSONAL 'added 11/28/2010
        If ID = CSIDL.MYDOCUMENTS Then  'original code - retain
            Dim pchEaten As Integer
            HR = GetDeskTop.Folder.ParseDisplayName(Nothing, Nothing, "::{450d8fba-ad25-11d0-98a8-0800361b1103}", _
                     pchEaten, tmpPidl, Nothing)
        Else
            HR = SHGetSpecialFolderLocation(0, ID, tmpPidl)
        End If
        If HR = NOERROR Then
            GetCShItem = GetCShItem(tmpPidl)
        End If
        If Not tmpPidl.Equals(IntPtr.Zero) Then
            Marshal.FreeCoTaskMem(tmpPidl)
        End If
    End Function

    ''' <summary>Given a Byte() containing the PIDL of a Folder and a Byte() containing the relative PIDL of the desired item,
    ''' GetCshItem finds or creates a CShItem and places any new CShItem into the internal tree.
    ''' The tree is expanded (filled in) as necessary to locate the CShItem or to locate the proper
    ''' placement of a new Item. The assumption is that the Folder system actually contains the item
    ''' that is requested -- File or Directory.Exists equivalent. Returns Nothing on errors such as
    ''' non-existant item.
    ''' </summary>
    ''' <param name="FoldBytes"></param>
    ''' <param name="ItemBytes"></param>
    ''' <returns>A CShItem or, in case of error, Nothing</returns>
    Public Shared Function GetCShItem(ByVal FoldBytes() As Byte, ByVal ItemBytes() As Byte) As CShItem
        GetCShItem = Nothing    'assume failure
        Dim b() As Byte = CPidl.JoinPidlBytes(FoldBytes, ItemBytes)
        If IsNothing(b) Then Exit Function 'can do no more with invalid pidls

        Dim thisPidl As IntPtr = Marshal.AllocCoTaskMem(b.Length)
        If thisPidl.Equals(IntPtr.Zero) Then Return Nothing
        Marshal.Copy(b, 0, thisPidl, b.Length)
        Dim Parent As CShItem = Nothing
        GetCShItem = GetCShItem(thisPidl)
        If Not thisPidl.Equals(IntPtr.Zero) Then Marshal.FreeCoTaskMem(thisPidl)
        If GetCShItem.PIDL.Equals(IntPtr.Zero) Then GetCShItem = Nothing 'last minute failsafe
    End Function

#End Region

#Region "       FindCShItem --- various signatures of FindCShItem"
    ''' <summary>
    ''' FindCShItem attempts to locate a CShItem in the internal tree. It will NOT expand the Tree during the
    ''' search. If the Item identified by the Absolute PIDL parameter is not ALREADY in the internal tree, then
    ''' FindCShItem will return NOTHING.
    ''' </summary>
    ''' <param name="ptr">An Absolute PIDL referencing the item to be Found.</param>
    ''' <returns>The existant CShItem if found, Nothing if not found.</returns>
    ''' <remarks> 5/31/2012 - most code in this function replaced by a call to FindCShItem(BaseItem as CShItem, Abs as IntPtr)</remarks>
    Public Shared Function FindCShItem(ByVal ptr As IntPtr) As CShItem
        Return FindCShItem(GetDeskTop(), ptr)
    End Function

    ''' <summary>
    ''' FindCShItem attempts to locate a CShItem in the internal tree. It will NOT expand the Tree during the
    ''' search. If the Item identified by the Absolute PIDL parameter is not ALREADY in the internal tree, then
    ''' FindCShItem will return NOTHING.
    ''' </summary>
    ''' <param name="Abs">An Absolute PIDL referencing the item to be Found.</param>
    ''' <returns>The existant CShItem if found, Nothing if not found.</returns>
    ''' <remarks> 5/31/2012 -Function added to replace algorithm used in FindCShItem(ptr as IntPtr) which now only calls this routine.</remarks>
    Public Shared Function FindCShItem(ByVal BaseItem As CShItem, ByVal Abs As IntPtr) As CShItem
        FindCShItem = Nothing
        If IsEqual(BaseItem.PIDL, Abs) Then Return BaseItem
        If BaseItem.FilesInitialized AndAlso IsAncestorOf(BaseItem.PIDL, Abs, True) Then
            For Each FItem As CShItem In BaseItem.FileList          '7/2/2012 was BaseItem.Files
                If IsEqual(FItem.PIDL, Abs) Then Return FItem
            Next
        End If
        If BaseItem.FoldersInitialized Then
            For Each DItem As CShItem In BaseItem.DirectoryList     '7/2/2012 was BaseItem.Directories
                If IsEqual(DItem.PIDL, Abs) Then Return DItem
                If IsAncestorOf(DItem.PIDL, Abs) Then
                    Return FindCShItem(DItem, Abs)
                End If
            Next
        End If
    End Function

    ''' <summary>
    ''' FindCShItem attempts to locate a CShItem in the internal tree. It will NOT expand the Tree during the
    ''' search. If the Item identified by the Absolute PIDL parameter is not ALREADY in the internal tree, then
    ''' FindCShItem will return NOTHING.
    ''' </summary>
    ''' <param name="b">A Byte array representation of a Full or Absolute PIDL 
    ''' referencing the item to be Found.</param>
    ''' <returns>The existant CShItem if found, Nothing if not found.</returns>
    ''' <remarks></remarks>
    Public Shared Function FindCShItem(ByVal b() As Byte) As CShItem
        If Not IsValidPidl(b) Then Return Nothing
        Dim thisPidl As IntPtr = Marshal.AllocCoTaskMem(b.Length)
        If thisPidl.Equals(IntPtr.Zero) Then Return Nothing
        Marshal.Copy(b, 0, thisPidl, b.Length)
        FindCShItem = FindCShItem(thisPidl)
        Marshal.FreeCoTaskMem(thisPidl)
    End Function
#End Region

#End Region

#Region "   Icomparable -- for default Sorting"

    ''' <summary>Computes the Sort key of this CShItem, based on its attributes</summary>
    ''' 
    Private Function ComputeSortFlag() As Integer
        Dim rVal As Integer = 0
        If m_IsDisk Then rVal = &H100000
        If m_TypeName.Equals(strSystemFolder) Then
            If Not m_IsBrowsable Then
                rVal = rVal Or &H10000
                If m_strMyDocuments.Equals(m_DisplayName) Then
                    rVal = rVal Or &H1
                End If
            Else
                rVal = rVal Or &H1000
            End If
        End If
        If m_IsFolder Then rVal = rVal Or &H100
        Return rVal
    End Function

    ''' <summary>
    '''  Compares an Object to this instance based on SortFlag. The Object must be a CShItem
    ''' </summary>
    ''' <param name="obj">A CShItem to be Compared to this instance.</param>
    ''' <returns>-1 if this instance less than obj, 0 if equal, 1 if greater.</returns>
    ''' <remarks>The Sort Order from Low to High is:
    ''' <list type="bullet">
    ''' <item><description>Nothing</description></item>
    ''' <item><description>Disks</description></item>
    ''' <item><description>non-browsable System Folders</description></item>
    ''' <item><description>browsable System Folders</description></item>
    ''' <item><description>Directories</description></item>
    ''' <item><description>Files</description></item>
    ''' </list>
    ''' </remarks>
    Public Overridable Overloads Function CompareTo(ByVal obj As Object) As Integer _
            Implements IComparable.CompareTo
        If IsNothing(obj) Then Return 1 'non-existant is always low
        Dim Other As CShItem = TryCast(obj, CShItem)
        ' UPDATE: Error Handling for CShItem.CompareTo
        If Other Is Nothing Then
#If DEBUG Then
            Throw New ArgumentException("Invalid argument for CShItem.CompareTo")
#End If
            Return 0 ' Ignore this in release builds
        End If
        If Not m_HasDispType Then SetDispType()
        Dim cmp As Integer = Other.SortFlag - m_SortFlag 'Note the reversal
        If cmp <> 0 Then
            Return cmp
        Else
            If m_IsDisk Then 'implies that both are
                Return String.Compare(Me.Path, Other.Path)
            Else
                '  Return String.Compare(m_DisplayName, Other.DisplayName)
                Return StringLogicalComparer.CompareStrings(Me.DisplayName, Other.DisplayName)
            End If
        End If
    End Function
#End Region

#Region "   Properties"

#Region "       Shared Properties"
    ''' <summary>
    ''' Contains a String with the Local representation of "My Computer"
    ''' </summary>
    Public Shared ReadOnly Property strMyComputer() As String
        Get
            Return m_strMyComputer
        End Get
    End Property
    ''' <summary>
    ''' Contains a String with the Local representation of "System Folder".
    ''' </summary>
    Public Shared ReadOnly Property strSystemFolder() As String
        Get
            Return m_strSystemFolder
        End Get
    End Property
    ''' <summary>
    ''' Contains a String with the Full Path of the Desktop Directory
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property DesktopDirectoryPath() As String
        Get
            Return m_DeskTopDirectory.Path
        End Get
    End Property
    ''' <summary>
    ''' True if System OS is Vista or above.
    ''' </summary>
    Public Shared ReadOnly Property IsVista() As Boolean
        Get
            Return VistaOrAbove
        End Get
    End Property

#End Region

#Region "       Normal Properties"
    ''' <summary>
    ''' Contains the PIDL for the current instance as an IntPtr
    ''' </summary>
    Public ReadOnly Property PIDL() As IntPtr
        Get
            Return m_Pidl
        End Get
    End Property

    Private Property UpdateFolder() As Boolean
        Get
            Return m_UpdateFolder
        End Get
        Set(ByVal value As Boolean)
            m_UpdateFolder = value
        End Set
    End Property

    ''' <summary>
    ''' Contains the IShellFolder Interface of the instance if it is a Folder.
    ''' </summary>
    ''' <returns>The IShellFolder Interface of the instance if it is a Folder</returns>
    Public ReadOnly Property Folder() As IShellFolder
        Get
            If m_UpdateFolder Then
                If m_Folder IsNot Nothing Then Marshal.ReleaseComObject(m_Folder)
                m_Folder = GetFolder(Me.Parent, ILFindLastID(m_Pidl))
                m_UpdateFolder = False
            End If
            Return m_Folder
        End Get
    End Property

    ''' <summary>
    ''' Contains the Full Path of the instance as obtained from Folder.GetDisplayNameOf
    ''' </summary>
    Public ReadOnly Property Path() As String
        Get
            If m_Path.Equals(String.Empty) Then
                SetPath()
            End If
            Return m_Path
        End Get
    End Property

    ''' <summary>
    ''' Contains the Full Path of the instance as obtained by traversing the internal cache's tree structure.
    ''' </summary>
    ''' <remarks>Useful for items located on certain removable drives not handled well by Folder.GetDisplayNameOf.</remarks>
    Public ReadOnly Property ItemPath() As String
        Get
            Dim item As CShItem = Me
            Dim pathlist As New List(Of CShItem)
            pathlist.Add(item)
            Do While item.Parent IsNot Nothing
                pathlist.Add(item.Parent)
                item = item.Parent
            Loop
            pathlist.Reverse()
            Dim SB As New StringBuilder
            For Each N As CShItem In pathlist
                SB.Append(N.DisplayName) : SB.Append("\")
            Next
            Return SB.ToString
        End Get
    End Property
    ''' <summary>
    ''' For internal use only
    ''' </summary>
    Friend ReadOnly Property FoldersInitialized() As Boolean
        Get
            Return m_Directories IsNot Nothing
        End Get
    End Property
    ''' <summary>
    ''' For internal use only
    ''' </summary>
    Friend ReadOnly Property FilesInitialized() As Boolean
        Get
            Return m_Files IsNot Nothing
        End Get
    End Property
    ''' <summary>
    ''' For internal use only
    ''' </summary>
    Friend ReadOnly Property DirectoryList() As CShItemCollection
        Get
            Return m_Directories
        End Get
    End Property
    ''' <summary>
    ''' Returns an Array of CShItems containing the sub Directories of this instance.
    ''' </summary>
    ''' <returns>Array of CShItems containing the sub Directories of this instance.</returns>
    Public ReadOnly Property Directories() As CShItem()
        Get
            If Not m_IsFolder Then
                Return Array.CreateInstance(GetType(CShItem), 0)    'mod 6/27/09
            ElseIf IsNothing(m_Directories) Then
                m_Directories = GetContents(SHCONTF.FOLDERS Or SHCONTF.INCLUDEHIDDEN)
            Else        '6/30/2012 - Under some circumstances, Windows does not post MKDIR msgs when Folders are created!!! Do a refresh to ensure we are up to date
                Me.UpdateRefresh(False, True)   '6/30/2012 - Note that it is also true that in some circumstances Windows does not post a RMDIR when Folders are removed.
            End If
            Return m_Directories.ToArray
        End Get
    End Property
    ''' <summary>
    ''' Returns the number of Folders currently known to this instance. If not
    ''' initialized, return 0
    ''' </summary>
    ''' <returns>The number of Folders currently known to this instance. If not
    ''' initialized, return 0</returns>
    ''' <remarks>Property added 02/10/2014 to avoid UpdateRefresh</remarks>
    Public ReadOnly Property DirCount() As Integer
        Get
            If Me.FoldersInitialized Then
                Return m_Directories.Count
            Else
                Return 0
            End If
        End Get
    End Property
    ''' <summary>
    ''' Returns the number of Files currently known to this instance. If not
    ''' initialized, return 0
    ''' </summary>
    ''' <returns>The number of Files currently known to this instance. If not
    ''' initialized, return 0</returns>
    ''' <remarks>Property added 02/10/2014 to avoid UpdateRefresh</remarks>
    Public ReadOnly Property FileCount() As Integer
        Get
            If Me.FilesInitialized Then
                Return m_Files.Count
            Else
                Return 0
            End If
        End Get
    End Property

    ''' <summary>
    ''' For internal use only
    ''' </summary>
    Friend ReadOnly Property FileList() As CShItemCollection
        Get
            Return m_Files
        End Get
    End Property

    ''' <summary>
    ''' Returns an Array of CShItems containing the Files contained in this instance.
    ''' </summary>
    ''' <returns>Array of CShItems containing the Files contained in this instance.</returns>
    Public ReadOnly Property Files() As CShItem()
        Get
            If Not m_IsFolder Then
                Return Array.CreateInstance(GetType(CShItem), 0)    'mod 6/27/09
            ElseIf IsNothing(m_Files) Then
                m_Files = GetContents(SHCONTF.NONFOLDERS Or SHCONTF.INCLUDEHIDDEN)
            Else        '6/30/2012 - Under some circumstances, Windows does not post CREATE msgs when Files are created!!! Do a refresh to ensure we are up to date
                Me.UpdateRefresh(True, False)   '6/30/2012 - Note that it is also true that in some circumstances Windows does not post a DELETE when Files are removed.
            End If
            Return m_Files.ToArray
        End Get
    End Property
    ''' <summary>
    ''' Contains the CShItem of this instance's Parent Folder
    ''' </summary>
    ''' <returns>CShItem of this instance's Parent Folder</returns>
    ''' <remarks>Returns Nothing for the Desktop which has no Parent</remarks>
    Public ReadOnly Property Parent() As CShItem
        Get
            Return m_Parent
        End Get
    End Property

    ''' <summary>
    ''' For internal use only
    ''' </summary>
    Friend Sub SetParent(ByVal parent As CShItem)
        m_Parent = parent
    End Sub
    ''' <summary>
    ''' This instance's Shell Attributes as returned by Folder.GetAttributesOf
    ''' </summary>
    ''' <returns>This instance's Shell Attributes as returned by Folder.GetAttributesOf</returns>
    ''' <remarks>Internal use only</remarks>
    Friend ReadOnly Property SFGAO_Attributes() As SFGAO        'Change 10/09/2011
        Get
            Return m_SFGAO_Attributes
        End Get
    End Property
    ''' <summary>
    ''' True if instance is Browsable, False otherwise
    ''' </summary>
    ''' <returns>True if instance is Browsable, False otherwise</returns>
    ''' <remarks>See MSDN for definition of "Browsable"</remarks>
    Public ReadOnly Property IsBrowsable() As Boolean
        Get
            Return m_IsBrowsable
        End Get
    End Property
    ''' <summary>
    ''' True if instance is a File System item
    ''' </summary>
    ''' <returns>True if instance is a File System item</returns>
    ''' <remarks>Numerous Virtual and/or Shell Extension Folders and their content are not members of the File System</remarks>
    Public ReadOnly Property IsFileSystem() As Boolean
        Get
            Return m_IsFileSystem
        End Get
    End Property
    ''' <summary>True if instance is a Folder, False otherwise
    ''' </summary>
    ''' <returns>True if instance is a Folder, False otherwise</returns>
    ''' <remarks>Numerous Virtual and/or Shell Extension Folders are not members of the File System</remarks>
    Public ReadOnly Property IsFolder() As Boolean
        Get
            Return m_IsFolder
        End Get
    End Property

    Private m_HasSubFoldersSetup As Boolean
    ''' <summary>
    ''' True if item is a Folder and has sub-Folders
    ''' </summary>
    ''' <returns>True if item is a Folder and has sub-Folders, False otherwise</returns>
    ''' <remarks>Modified to make this attribute behave (with respect to Remote Folders) like XP, even on Vista/Win7.
    ''' That is, any Remote Folder is reported as HasSubFolders = True. Local Folders are tested with the API call.
    ''' On Vista/Win7, Compressed files (eg - .Zip, .Cab, etc) are considered sub Folders by this Property.
    ''' This behavior is NOT modified to behave like XP.</remarks>
    Public ReadOnly Property HasSubFolders() As Boolean
        Get
            If m_HasSubFoldersSetup Then
                Return m_HasSubFolders
            ElseIf m_IsRemote Then          '4/14/2012
                m_HasSubFolders = True      '4/14/2012
                m_HasSubFoldersSetup = True '4/14/2012
            Else
                Dim shfi As New SHFILEINFO()
                shfi.dwAttributes = SFGAO.HASSUBFOLDER
                Dim dwflag As SHGFI = SHGFI.PIDL Or _
                                        SHGFI.ATTRIBUTES Or _
                                        SHGFI.ATTR_SPECIFIED
                Dim dwAttr As Integer = 0
                Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
                If H.ToInt32 <> NOERROR AndAlso H.ToInt32 <> 1 Then
                    Marshal.ThrowExceptionForHR(H.ToInt32)
                End If
                m_HasSubFolders = CBool(shfi.dwAttributes And SFGAO.HASSUBFOLDER)
                m_SFGAO_Attributes = m_SFGAO_Attributes Or (shfi.dwAttributes And SFGAO.HASSUBFOLDER)
                m_HasSubFoldersSetup = True
            End If
            Return m_HasSubFolders
        End Get
    End Property

    ''' <summary>
    ''' True if this instance is a Disk like device, False otherwise
    ''' </summary>
    ''' <returns>True if this instance is a Disk like device, False otherwise</returns>
    Public ReadOnly Property IsDisk() As Boolean
        Get
            Return m_IsDisk
        End Get
    End Property
    ''' <summary>
    ''' True if this instance is a Link (.lnk or Shortcut), False otherwise
    ''' </summary>
    ''' <returns>True if this instance is a Link (.lnk or Shortcut), False otherwise</returns>
    Public ReadOnly Property IsLink() As Boolean
        Get
            Return m_IsLink
        End Get
    End Property
    ''' <summary>
    ''' True if this instance is Shared, False otherwise
    ''' </summary>
    ''' <returns>True if this instance Shared, False otherwise</returns>
    Public ReadOnly Property IsShared() As Boolean
        Get
            Return m_IsShared
        End Get
    End Property
    ''' <summary>
    ''' True if this instance is Hidden, False otherwise
    ''' </summary>
    ''' <returns>True if this instance Hidden, False otherwise</returns>
    Public ReadOnly Property IsHidden() As Boolean
        Get
            Return m_IsHidden
        End Get
    End Property
    ''' <summary>
    ''' True if this instance is a Removable device, False otherwise
    ''' </summary>
    ''' <returns>True if this instance is a Removable device, False otherwise</returns>
    Public ReadOnly Property IsRemovable() As Boolean
        Get
            Return m_IsRemovable
        End Get
    End Property
    ''' <summary>
    ''' Returns True if this CShItem represents a Folder/File stored on a Remote system
    ''' </summary>
    ''' <returns>Returns True if this CShItem represents a Folder/File stored on a Remote system, False otherwise.</returns>
    ''' <remarks>
    ''' A Remote item is any item whose path is a UNC not referring to the Local system or
    ''' resides on a Mapped (Network) Drive. Set up in SetupAttributes.
    ''' </remarks>
    Public ReadOnly Property IsRemote() As Boolean  '4/14/2012
        Get                                         '4/14/2012
            Return m_IsRemote                       '4/14/2012
        End Get                                     '4/14/2012
    End Property

    ''' <summary>
    ''' True if this instance can be Renamed, False otherwise
    ''' </summary>
    ''' <returns>True if this instance can be Renamed, False otherwise</returns>
    Public ReadOnly Property CanRename() As Boolean
        Get
            Return m_CanRename
        End Get
    End Property

    Private m_size As String = "[]"
    ''' <summary>
    ''' A Formatted String representation of the Item's FileSize
    ''' </summary>
    ''' <returns>A Formatted String representation of the Item's FileSize</returns>
    Public ReadOnly Property Size() As String
        Get
            If m_size = "[]" Then
                GetSize()
            End If
            Return m_size
        End Get
    End Property

    Private Sub GetSize()
        'Split the file size into bytes, kb, MB and GB
        If (Not Me.IsFolder And Me.IsFileSystem) Or Me.IsDisk Then
            If Me.Length >= (1048576 * 1024) Then
                m_size = Format(Me.Length / (1048576 * 1024), "#,###.# GB")
            ElseIf Me.Length >= 1048576 Then
                m_size = Format(Me.Length / 1048576, "#,###.# MB")
            ElseIf Me.Length >= 1024 Then
                m_size = Format(Me.Length / 1024, "#,### KB")
            ElseIf Not (Me.IsRemovable And Me.Length = 0) Then 'Don't show a CD-ROM's size if it doesn't have a disk in it
                m_size = Format(Me.Length, "##0 Bytes")
            Else
                m_size = "" 'Empty CD-ROM
            End If
        Else
            m_size = ""
        End If
    End Sub

    ''' <summary>
    ''' An Object which may used to associate application information with this instance
    ''' </summary>
    ''' <returns>An Object which may used to associate application information with this instance. Nothing if not set by application.</returns>
    ''' <remarks>
    ''' Property may be used for any application defined purpose.
    ''' </remarks>
    Public Property Tag() As Object
        Get
            Return m_Tag
        End Get
        Set(ByVal value As Object)
            m_Tag = value
        End Set
    End Property

    ''' <summary>
    ''' Property used to store information returned by FindFirstFile/FindNextFile API call.
    ''' </summary>
    ''' <returns>The current value or Nothing if not set</returns>
    ''' <remarks>Used to optimize the fetching of information otherwise only easily available from FileInfo/DirectoryInfo.</remarks>
    Public Property W32Data() As W32Find_Data
        Get
            Return m_W32Data
        End Get
        Set(ByVal value As W32Find_Data)
            m_W32Data = value
        End Set
    End Property

#Region "       Drag Ops Properties"

    ''' <summary>
    ''' Returns True if instance may be Moved, False otherwise.
    ''' </summary>
    ''' <returns>True if instance may be Moved, False otherwise.</returns>
    Public ReadOnly Property CanMove() As Boolean
        Get
            Return m_CanMove
        End Get
    End Property

    ''' <summary>
    ''' Returns True if instance can be Copied, False otherwise
    ''' </summary>
    ''' <returns>True if instance can be Copied, False otherwise</returns>
    Public ReadOnly Property CanCopy() As Boolean
        Get
            Return m_CanCopy
        End Get
    End Property

    ''' <summary>
    ''' Returns True if instance can be Deleted, False otherwise
    ''' </summary>
    ''' <returns>True if instance can be Deleted, False otherwise</returns>
    Public ReadOnly Property CanDelete() As Boolean
        Get
            Return m_CanDelete
        End Get
    End Property

    ''' <summary>
    ''' Returns True if instance can be Linked to, False otherwise
    ''' </summary>
    ''' <returns>True if instance can be Linked to, False otherwise</returns>
    Public ReadOnly Property CanLink() As Boolean
        Get
            Return m_CanLink
        End Get
    End Property

    ''' <summary>
    ''' Returns True if instance can be a Drop Target, False otherwise
    ''' </summary>
    ''' <returns>True if instance can be a Drop Target, False otherwise</returns>
    Public ReadOnly Property IsDropTarget() As Boolean
        Get
            Return m_IsDropTarget
        End Get
    End Property
#End Region

#End Region

#Region "       Filled on Demand Properties"

#Region "           Filled based on m_HasDispType"
    ''' <summary>
    ''' Sets DisplayName, TypeName, and SortFlag when actually needed
    ''' </summary>
    ''' 
    Private Sub SetDispType()
        'Get Displayname, TypeName
        Dim shfi As New SHFILEINFO()
        Dim dwflag As SHGFI = SHGFI.DISPLAYNAME Or _
                                SHGFI.TYPENAME Or _
                                SHGFI.PIDL
        Dim dwAttr As Integer = 0
        If m_IsFileSystem And Not m_IsFolder Then
            dwflag = dwflag Or SHGFI.USEFILEATTRIBUTES
            dwAttr = FILE_ATTRIBUTE_NORMAL
        End If
        Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
        m_DisplayName = shfi.szDisplayName
        m_TypeName = shfi.szTypeName
        'fix DisplayName
        If m_DisplayName.Equals("") Then
            m_DisplayName = Me.Path
        End If
        'Fix TypeName
        'If m_IsFolder And m_TypeName.Equals("File") Then
        '    m_TypeName = "File Folder"
        'End If
        m_SortFlag = ComputeSortFlag()
        m_HasDispType = True
    End Sub

    ''' <summary>
    ''' The Name of the File or Directory. If a Special Folder, then the Windows name for that Special Folder
    ''' </summary>
    ''' <returns>The Name of the File or Directory. If a Special Folder, then the Windows name for that Special Folder</returns>
    ''' <remarks>For a link file (xxx.txt.lnk for example) the
    '''  DisplayName property will return xxx.txt</remarks>
    Public ReadOnly Property DisplayName() As String
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_DisplayName
        End Get
    End Property

    ''' <summary>
    ''' An alternate way of obtaining the DisplayName
    ''' </summary>
    ''' <returns>The DisplayName</returns>
    ''' <remarks>For a link file (xxx.txt.lnk for example) the
    '''  DisplayName property will return xxx.txt</remarks>
    Public ReadOnly Property Text() As String
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_DisplayName
        End Get
    End Property

    ''' <summary>
    ''' Name is another way of obtaining the DisplayName
    ''' </summary>
    ''' <returns>The DisplayName of the Item</returns>
    ''' <remarks>For a link file (xxx.txt.lnk for example) the
    '''  DisplayName property will return xxx.txt</remarks>
    Public ReadOnly Property Name() As String
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_DisplayName
        End Get
    End Property
    Private ReadOnly Property SortFlag() As Integer
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_SortFlag
        End Get
    End Property

    ''' <summary>
    ''' The Windows TypeName (eg "Text File")
    ''' </summary>
    ''' <returns>The Windows TypeName</returns>
    Public ReadOnly Property TypeName() As String
        Get
            If Not m_HasDispType Then SetDispType()
            Return m_TypeName
        End Get
    End Property
#End Region

#Region "           IconIndex properties"
    ''' <summary>
    ''' Should not be directly referenced by the application.<br />
    ''' Contains the base IconIndex of the "normal" Icon in the System ImageList 
    ''' as returned by SHGetFileInfo
    ''' </summary>
    ''' <returns>The IconIndex into the System ImageList as returned by SHGetFileInfo</returns>
    ''' <remarks>This is not the IconIndex returned by SystemImageListManager. It is the
    '''          IconIndex that is passed to SystemImageListManager to obtain the true index
    '''          into the per process System Image List. In most, but not all cases, the two
    '''          values are the same.</remarks>
    Friend ReadOnly Property IconIndexNormalOrig() As Integer
        Get
            If m_IconIndexNormalOrig < 0 Then
                If Not m_HasDispType Then SetDispType()
                Dim shfi As New SHFILEINFO()
                Dim dwflag As SHGFI = SHGFI.PIDL Or _
                                        SHGFI.SYSICONINDEX
                Dim dwAttr As Integer = 0
                If m_IsFileSystem And Not m_IsFolder Then
                    dwflag = dwflag Or SHGFI.USEFILEATTRIBUTES
                    dwAttr = FILE_ATTRIBUTE_NORMAL
                End If
                Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
                m_IconIndexNormalOrig = shfi.iIcon
                If m_IconIndexNormal < 0 Then m_IconIndexNormal = SystemImageListManager.GetIconIndex(Me)
            End If
            Return m_IconIndexNormalOrig
        End Get
    End Property

    ''' <summary>
    ''' Should not be directly referenced by the application.<br />
    ''' The base IconIndex of the "Open" image in the System Image List.
    ''' </summary>
    ''' <returns>The base IconIndex of the "Open" image as returned by SHGetFileInfo</returns>
    ''' <remarks>On at least Win7 systems, the "open" Icon is the same as the "normal" Icon.</remarks>
    Friend ReadOnly Property IconIndexOpenOrig() As Integer
        Get
            If m_IconIndexOpenOrig < 0 Then
                If Not m_HasDispType Then SetDispType()
                If Not m_IsDisk And m_IsFileSystem And m_IsFolder Then
                    Dim dwflag As SHGFI = SHGFI.SYSICONINDEX Or SHGFI.PIDL
                    Dim shfi As New SHFILEINFO()
                    Dim H As IntPtr = SHGetFileInfo(m_Pidl, 0, _
                                      shfi, cbFileInfo, _
                                      dwflag Or SHGFI.OPENICON)
                    m_IconIndexOpenOrig = shfi.iIcon
                    If m_IconIndexOpen < 0 Then m_IconIndexOpen = SystemImageListManager.GetIconIndex(Me, True)
                Else
                    m_IconIndexOpenOrig = m_IconIndexNormalOrig
                End If
            End If
            Return m_IconIndexOpenOrig
        End Get
    End Property

    ''' <summary>
    ''' The Index of the "normal" Icon into the list maintained by SystemImageListManager and
    ''' used for the IconIndex in ListViewItems and TreeNodes.
    ''' </summary>
    ''' <value></value>
    ''' <returns>The "normal" IconIndex as used by ListViewItems and TreeNodes</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IconIndexNormal() As Integer
        Get
            If m_IconIndexNormal < 0 Then
                If Not m_HasDispType Then SetDispType()
                m_IconIndexNormal = SystemImageListManager.GetIconIndex(Me)
            End If
            Return m_IconIndexNormal
        End Get
    End Property

    ''' <summary>
    ''' The Index of the "Open" Icon into the list maintained by SystemImageListManager and
    ''' used for the IconIndex in ListViewItems and TreeNodes.
    ''' </summary>
    ''' <value></value>
    ''' <returns>The "Open" IconIndex as used by ListViewItems and TreeNodes</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IconIndexOpen() As Integer
        Get
            If m_IconIndexOpen < 0 Then
                If Not m_HasDispType Then SetDispType()
                If Not m_IsDisk And m_IsFileSystem And m_IsFolder Then
                    m_IconIndexOpen = SystemImageListManager.GetIconIndex(Me, True)
                Else
                    m_IconIndexOpen = m_IconIndexNormal
                End If
            End If
            Return m_IconIndexOpen
        End Get
    End Property

    'Private Shared m_ExtDict As New Dictionary(Of String, Integer)

    '''' <summary>
    '''' The following optimization of IconIndexNormal is a successful but invalid way of optimizing the initial fetch of
    '''' IconIndexNormal. It is successful because it reduces Icon fetch time by 2/3 (2 seconds vs 6 seconds in 3000 file test dir on WHS1)
    '''' but is invalid since all of a file type will have the same Icon - the first one seen - 
    '''' this is really bad for .exe and .dll files and for certain image file types (eg .bmp, .ico, .png).
    '''' These Icons in a normal Win7 (at least) system will actually be a view of the Image which is very handy for most purposes.
    '''' The code avoids the trap of renamed link files, but cannot, without boosting the time and complexity, avoid the Image file
    '''' problem. It is worth noting that .bmp and .png files display, each with a single image using the normal SystemImageListManager
    '''' optimization - though .ico files show each with its' own unique icon - hmmm - probably need a different API call, or at least
    '''' an additional flag bit set. TBD. Note that in .bmp and .png files with normal SystemImageListManager optimization show a 
    '''' unique per type icon that is the old, regular icon.
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public ReadOnly Property IconIndexNormal() As Integer
    '    Get
    '        If m_IconIndexNormal < 0 Then
    '            If Not m_HasDispType Then SetDispType()
    '            Dim shfi As New SHFILEINFO()
    '            Dim dwflag As SHGFI = SHGFI.PIDL Or _
    '                                    SHGFI.SYSICONINDEX
    '            Dim dwAttr As Integer = 0
    '            Dim Ext As String
    '            If m_IsFileSystem And Not m_IsFolder Then
    '                dwflag = dwflag Or SHGFI.USEFILEATTRIBUTES
    '                dwAttr = FILE_ATTRIBUTE_NORMAL
    '                Ext = IO.Path.GetExtension(m_DisplayName)
    '                If m_ExtDict.ContainsKey(Ext) Then
    '                    m_IconIndexNormal = m_ExtDict(Ext)
    '                End If
    '            End If
    '            If m_IconIndexNormal < 0 Then         'it won't be if set above
    '                Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
    '                m_IconIndexNormal = shfi.iIcon
    '                If Ext IsNot Nothing AndAlso Not Me.IsLink AndAlso Ext <> "" Then m_ExtDict.Add(Ext, m_IconIndexNormal) 'Only set if should be in ExtDict, but isn't yet
    '            End If
    '        End If
    '        Return m_IconIndexNormal
    '    End Get
    'End Property

#End Region

#Region "           FileInfo derived Properties"

    ''' <summary>
    ''' Obtains information available from FileInfo. Uses data from W32Data rather than FileInfo/DirectoryInfo if W32Data is present.
    ''' </summary>
    Private Sub FillDemandInfo()
        If m_W32Data IsNot Nothing AndAlso m_IsFileSystem Then  '04/24/2012 - changed to use m_W32Data rather than .Tag
            Dim W_32 As W32Find_Data = m_W32Data
            m_LastWriteTime = W_32.LastWriteTime
            m_LastAccessTime = W_32.LastAccessTime
            m_CreationTime = W_32.CreationTime
            If Not m_IsFolder Then m_Length = W_32.Length
            m_Attributes = W_32.Attributes
            m_W32Data = Nothing      'have what we need. clear for updates
        ElseIf m_IsFileSystem And Not m_IsFolder Then
            'in this case, it's a file
            If File.Exists(Me.Path) Then
                Dim fi As New FileInfo(Me.Path)
                m_LastWriteTime = fi.LastWriteTime
                m_LastAccessTime = fi.LastAccessTime
                m_CreationTime = fi.CreationTime
                m_Length = fi.Length
                m_Attributes = fi.Attributes          'Added 10/09/2011
            End If
        ElseIf m_IsFileSystem And m_IsFolder Then
            If Directory.Exists(Me.Path) Then
                Dim di As New DirectoryInfo(Me.Path)
                m_LastWriteTime = di.LastWriteTime
                m_LastAccessTime = di.LastAccessTime
                m_CreationTime = di.CreationTime
                m_Attributes = di.Attributes          'Added 10/09/2011
            End If
        End If
        m_XtrInfo = True            '05/15/2012 even if there were errors, we have what we can get (long file name problem)
    End Sub

    ''' <summary>
    ''' Contains the LastWriteTime (Last Modified) DateTime of this instance
    ''' </summary>
    ''' <returns>The LastWriteTime (Last Modified) DateTime of this instance</returns>
    ''' <remarks>With other information, Filled by FillDemandInfo on first Get</remarks>
    Public ReadOnly Property LastWriteTime() As DateTime
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_LastWriteTime
        End Get
    End Property

    ''' <summary>
    ''' Contains the LastAccessTime DateTime of this instance
    ''' </summary>
    ''' <returns>The LastAccessTime DateTime of this instance</returns>
    ''' <remarks>With other information, Filled by FillDemandInfo on first Get</remarks>
    Public ReadOnly Property LastAccessTime() As DateTime
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_LastAccessTime
        End Get
    End Property

    ''' <summary>
    ''' Contains the CreationTime DateTime of this instance
    ''' </summary>
    ''' <returns>The CreationTime DateTime of this instance</returns>
    ''' <remarks>With other information, Filled by FillDemandInfo on first Get</remarks>
    Public ReadOnly Property CreationTime() As DateTime
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_CreationTime
        End Get
    End Property

    ''' <summary>
    ''' Contains the FileSize of this instance
    ''' </summary>
    ''' <returns>The FileSize of this instance</returns>
    ''' <remarks>With other information, Filled by FillDemandInfo on first Get</remarks>
    Public ReadOnly Property Length() As Long
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_Length
        End Get
    End Property

    ''' <summary>
    ''' Contains the FileAttributes of this instance
    ''' </summary>
    ''' <returns>The FileAttributes of this instance</returns>
    ''' <remarks>This is the same information, formatted the same way, as found in FileInfo, GetAttr, etc.<br />
    '''          With other information, Filled by FillDemandInfo on first Get</remarks>
    Public ReadOnly Property Attributes() As FileAttributes 'Added 10/09/2011
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_Attributes
        End Get
    End Property

    ''' <summary>
    ''' Returns True if instance is a Mapped (not Local) Drive, False otherwise
    ''' </summary>
    ''' <returns>True if instance is a Mapped (not Local) Drive, False otherwise</returns>
    ''' <remarks>With other information, Filled by FillDemandInfo on first Get</remarks>
    Public ReadOnly Property IsNetworkDrive() As Boolean
        Get
            If Not m_XtrInfo Then
                FillDemandInfo()
            End If
            Return m_IsNetWorkDrive
        End Get
    End Property
#End Region

#Region "           cPidl information"
    ''' <summary>
    ''' The CPidl representation of this instance's PIDL
    ''' </summary>
    ''' <returns>The CPidl representation of this instance's PIDL</returns>
    Public ReadOnly Property clsPidl() As CPidl
        Get
            If IsNothing(m_cPidl) Then
                m_cPidl = New CPidl(m_Pidl)
            End If
            Return m_cPidl
        End Get
    End Property
#End Region

#Region "           IsReadOnly and IsSystem"
    '''<summary>True if instance is ReadOnly, False otherwise</summary>
    ''' <remarks>The IsReadOnly attribute causes an annoying access to any floppy drives
    ''' on the system. To postpone this (or avoid, depending on user action),
    ''' the attribute is only queried when asked for
    ''' </remarks>
    Public ReadOnly Property IsReadOnly() As Boolean
        Get
            If m_IsReadOnlySetup Then
                Return m_IsReadOnly
            Else
                Dim shfi As New SHFILEINFO()
                shfi.dwAttributes = SFGAO.RDONLY
                Dim dwflag As SHGFI = SHGFI.PIDL Or _
                                        SHGFI.ATTRIBUTES Or _
                                        SHGFI.ATTR_SPECIFIED
                Dim dwAttr As Integer = 0
                Dim H As IntPtr = SHGetFileInfo(m_Pidl, dwAttr, shfi, cbFileInfo, dwflag)
                If H.ToInt32 <> NOERROR AndAlso H.ToInt32 <> 1 Then
                    Marshal.ThrowExceptionForHR(H.ToInt32)
                End If
                m_IsReadOnly = CBool(shfi.dwAttributes And SFGAO.RDONLY)
                m_SFGAO_Attributes = m_SFGAO_Attributes Or (shfi.dwAttributes And SFGAO.RDONLY)
                m_IsReadOnlySetup = True
                Return m_IsReadOnly
            End If
        End Get
    End Property
    '''<summary>True if this instance has been marked "System", False otherwise
    '''</summary>
    ''' <returns>True if this instance has been marked "System", False otherwise</returns>
    ''' <remarks>The IsSystem attribute is seldom used, but required by DragDrop operations.
    ''' Since there is no way of getting ONLY the System attribute without getting
    ''' the RO attribute (which forces a reference to the floppy drive), we pay
    ''' the price of calling File.GetAttributes for this purpose alone.</remarks>
    Public ReadOnly Property IsSystem() As Boolean
        Get
            Static HaveSysInfo As Boolean   'true once we have gotten this attr
            Static m_IsSystem As Boolean    'the value of this attr once we have it
            If Not HaveSysInfo Then
                Try
                    m_IsSystem = (File.GetAttributes(Me.Path) And FileAttributes.System) = FileAttributes.System
                    HaveSysInfo = True
                Catch ex As Exception
                    HaveSysInfo = True
                End Try
            End If
            Return m_IsSystem
        End Get
    End Property

#End Region

#End Region

#End Region

#Region "   Public Methods"

#Region "       Shared Public Methods"

#Region "       GetDeskTop"
    ''' <summary>
    ''' If not initialized, then build DesktopBase
    ''' once done, or if initialized already, returns DestopBase
    ''' </summary>
    '''<returns>The DesktopBase CShItem representing the desktop</returns>
    ''' 
    Public Shared Function GetDeskTop() As CShItem
        If IsNothing(DesktopBase) Then
            DesktopBase = New CShItem()
        End If
        Return DesktopBase
    End Function
#End Region

#Region "       IsAncestorOf"
    '''<summary>True if parameter "ancestor" is an ancestor of parameter "current" 
    '''</summary>
    ''' <returns>IsAncestorOf returns True if input CShItem ancestor is an ancestor of input CShItem current</returns>
    ''' <remarks>if OS is Win2K or above, uses the ILIsParent API, otherwise uses the
    ''' cPidl function StartsWith.  This is necessary since ILIsParent in only available
    ''' in Win2K or above systems AND StartsWith fails on some folders on XP systems (most
    ''' obviously some Network Folder Shortcuts, but also Control Panel. Note, StartsWith
    ''' always works on systems prior to XP.<br />
    ''' NOTE: if ancestor and current reference the same Item, both
    ''' methods return True</remarks>
    Public Shared Function IsAncestorOf(ByVal ancestor As CShItem, _
                                        ByVal current As CShItem, _
                                        Optional ByVal fParent As Boolean = False) _
                                        As Boolean
        Return IsAncestorOf(ancestor.PIDL, current.PIDL, fParent)
    End Function
    '''<summary> Compares a candidate Ancestor PIDL with a Child PIDL and
    ''' returns True if Ancestor is an ancestor of the child.
    ''' if fParent is True, then only return True if Ancestor is the immediate
    ''' parent of the Child</summary>
    ''' <param name="AncestorPidl">The Absolute PIDL that is the candidate for being an Ancestor of ChildPidl.</param>
    ''' <param name="ChildPidl">The Absolute PIDL whose ancestory is being searched for.</param>
    ''' <param name="fParent">A flag. If True, then only return True if AncestorPidl is the immediate Parent of ChildPidl.</param>
    ''' <returns>True if AncestorPidl is an ancestor of ChildPidl.
    '''          If fParent is False then will also return True if AncestorPidl and ChildPidl are equal. 
    '''          If fParent is True, <i>only</i> returns True if AncestorPidl is the Parent of ChildPidl</returns>
    Public Shared Function IsAncestorOf(ByVal AncestorPidl As IntPtr, _
                                        ByVal ChildPidl As IntPtr, _
                                        Optional ByVal fParent As Boolean = False) _
                                        As Boolean
        If Is2KOrAbove() Then
            Return ILIsParent(AncestorPidl, ChildPidl, fParent)
        Else
            Dim Child As New CPidl(ChildPidl)
            Dim Ancestor As New CPidl(AncestorPidl)
            IsAncestorOf = Child.StartsWith(Ancestor)
            If Not IsAncestorOf Then Exit Function
            If fParent Then ' check for immediate ancestor, if desired
                Dim oAncBytes() As Object = Ancestor.Decompose
                Dim oChildBytes() As Object = Child.Decompose
                If oAncBytes.Length <> (oChildBytes.Length - 1) Then
                    IsAncestorOf = False
                End If
            End If
        End If
    End Function
#End Region

#Region "      AllFolderWalk"
    '''<summary>The WalkAllCallBack delegate defines the signature of 
    ''' the routine to be passed to AllFolderWalk which returns the CShItem of each
    ''' file and directory in and below an Folder CShItem.
    '''</summary>
    ''' <example>Dim DWalk as New CshItem.WalkAllCallBack(addressof yourroutine)</example>
    Public Delegate Function WalkAllCallBack(ByVal info As CShItem, _
                                             ByVal UserLevel As Integer, _
                                             ByVal Tag As Integer) _
                                             As Boolean
    '''<summary>
    ''' AllFolderWalk recursively walks down directories from cStart, calling its
    '''   callback routine, WalkAllCallBack, for each Directory and File encountered, including those in
    '''   cStart.  UserLevel is incremented by 1 for each level of dirs that DirWalker
    '''  recurses thru.  Tag is an Integer that is simply passed, unmodified to the 
    '''  callback, with each CShItem encountered, both File and Directory CShItems.
    ''' </summary>
    ''' <param name="cStart">The CShItem being examined</param>
    ''' <param name="cback">AddressOf a WalkAllCallBack routine</param>
    ''' <param name="UserLevel">An integer, incremented by 1 for each level of directory and passed to the CallBack routine</param>
    ''' <param name="Tag">An integer passed unmodified to the CallBack routine</param>
    ''' <returns>True to continue Walk, False if Callback said to stop</returns>
    ''' <remarks>It is much more efficient to implement this Function (without CallBack) in the application.</remarks>
    ''' 
    Public Shared Function AllFolderWalk(ByVal cStart As CShItem, _
                                          ByVal cback As WalkAllCallBack, _
                                          ByVal UserLevel As Integer, _
                                          ByVal Tag As Integer) _
                                          As Boolean
        If Not IsNothing(cStart) AndAlso cStart.IsFolder Then
            Dim cItem As CShItem
            'first processes all files in this directory
            For Each cItem In cStart.FileList       '7/2/2012 used Files
                If Not cback(cItem, UserLevel, Tag) Then
                    Return False        'user said stop
                End If
            Next
            'then process all dirs in this directory, recursively
            For Each cItem In cStart.DirectoryList          '7/2/2012 used Directories
                If Not cback(cItem, UserLevel + 1, Tag) Then
                    Return False        'user said stop
                Else
                    If Not AllFolderWalk(cItem, cback, UserLevel + 1, Tag) Then
                        Return False
                    End If
                End If
            Next
            Return True
        Else        'Invalid call
            Throw New ApplicationException("AllFolderWalk -- Invalid Start Directory")
        End If
    End Function
#End Region

#End Region

#Region "       Public Instance Methods"

#Region "           Equals"
    ''' <summary>
    ''' Compares this instance of CShItem to another CShItem. Equality is based on a string comparison of
    ''' their Paths.
    ''' </summary>
    ''' <param name="other">A CShItem to be tested for equality to the current instance.</param>
    ''' <returns>True if both paths are equal.</returns>
    ''' <remarks>An Obsolete method. Since only one copy of a CShItem is allowed, the proper test
    ''' is "If Me Is other".</remarks>
    Public Overloads Function Equals(ByVal other As CShItem) As Boolean
        Equals = Me.Path.Equals(other.Path)
    End Function
#End Region

#Region "           AddItem, RemoveItem"
    ''' <summary>
    ''' For internal use only
    ''' </summary>
    Friend Sub AddItem(ByVal item As CShItem)
        Dim Changed As Boolean = False
        SyncLock LockObj
            Try
                item.m_Parent = Me
                If item.IsFolder Then
                    If Me.FoldersInitialized AndAlso Not m_Directories.Contains(item.PIDL) Then
                        m_Directories.Add(item)
                        Changed = True
                    End If
                ElseIf Me.FilesInitialized AndAlso Not m_Files.Contains(item.PIDL) Then
                    m_Files.Add(item)
                    Changed = True
                End If
            Catch ex As Exception
                Debug.WriteLine("Error in CShItem.AddItem -- " & ex.ToString)
            End Try
        End SyncLock
        If Changed Then
            RaiseEvent CShItemUpdate(Me, New ShellItemUpdateEventArgs(item, CShItemUpdateType.Created))
        End If
    End Sub

    ''' <summary>
    ''' For internal use only
    ''' </summary>
    Friend Sub RemoveItem(ByVal item As CShItem)
        Dim Changed As Boolean = False
        SyncLock LockObj
            Try
                If item.IsFolder Then
                    If Me.FoldersInitialized AndAlso m_Directories.Contains(item) Then
                        'Debug.WriteLine("Removing " & item.Path & " From " & Me.Path)
                        m_Directories.Remove(item)
                        Changed = True
                    End If
                ElseIf Me.FilesInitialized AndAlso m_Files.Contains(item) Then
                    m_Files.Remove(item)
                    Changed = True
                End If
            Catch ex As Exception
                Debug.WriteLine("Error in CShItem.RemoveItem -- " & ex.ToString)
            End Try
        End SyncLock
        If Changed Then
            RaiseEvent CShItemUpdate(Me, New ShellItemUpdateEventArgs(item, CShItemUpdateType.Deleted))
        End If
    End Sub
#End Region

#Region "           ClearItems"
    ''' <summary>
    ''' Clear File and/or Folder items from the CShItem internal cache.
    ''' </summary>
    ''' <param name="ClearFiles">Clear Files</param>
    ''' <param name="ClearDirectories">Clear Folders</param>
    ''' <remarks>Typically used to discard CShItems representing Files that are no longer displayed in 
    ''' the GUI.</remarks>
    Public Sub ClearItems(ByVal ClearFiles As Boolean, Optional ByVal ClearDirectories As Boolean = False)
        SyncLock LockObj
            If ClearFiles AndAlso m_Files IsNot Nothing Then
                m_Files.Clear()
                m_Files = Nothing
            End If
            If ClearDirectories AndAlso m_Directories IsNot Nothing Then
                m_Directories.Clear()
                m_Directories = Nothing
            End If
        End SyncLock
    End Sub

#End Region

#Region "           StopGlobalNotification"
    ''' <summary>
    ''' Stops monitoring of changes to the File System.
    ''' </summary>
    ''' <returns>True if Successful, False otherwise</returns>
    ''' <remarks>Global Change Notification is started by default. Call this function to turn it off.
    '''          Only turn Notification Off under rare, well understood circumstances. If turned off, NO
    '''          changes, including those made by the application will be noticed.</remarks>
    Public Function StopGlobalNotification() As Boolean
        StopGlobalNotification = False        'assume failure
        If Me IsNot DesktopBase Then Exit Function
        If m_updater Is Nothing Then
            StopGlobalNotification = True     'Already stopped
            Exit Function
        End If
        m_updater.Dispose()
        m_updater = Nothing
        StopGlobalNotification = True
    End Function
#End Region

#Region "           StartGlobalNotification"
    ''' <summary>
    ''' Restarts the Dynamic Update listening for Windows Notify messages
    ''' </summary>
    ''' <returns>True if successful, False otherwise</returns>
    ''' <remarks>Resumesthe detection of changes to the FileSystem after a StopGlobalNotification call.
    '''          Changes between that call and a restart will be lost.</remarks>
    Public Function StartGlobalNotification() As Boolean
        StartGlobalNotification = False       'assume failure
        If Me IsNot DesktopBase Then Exit Function
        If m_updater IsNot Nothing Then
            StartGlobalNotification = True        'Already started
            Exit Function
        End If
        m_updater = New CShItemUpdater(Me)
        If m_updater IsNot Nothing Then
            StartGlobalNotification = True
        End If
    End Function
#End Region

#Region "           GetDirectories"
    ''' <summary>
    ''' Returns the sub-directories of the current instance, if the current instance is a
    ''' Folder. Similar to to Property Directories except that it returns the Directories
    ''' as an ArrayList.
    ''' </summary>
    ''' <returns>If the current instance is a Folder, returns its sub-directories as an 
    ''' ArrayList containing the CShItems of its sub-directories. Returns an empty list if
    ''' there are no sub-directories. Returns Nothing if the current instance is not a Folder.</returns>
    ''' <remarks></remarks>
    Public Function GetDirectories() As ArrayList
        Dim D() As CShItem = Me.Directories         '7/2/2012 OK to use Directories in this case
        If D Is Nothing Then Return Nothing
        Dim AL As New ArrayList
        AL.AddRange(D)
        Return AL
    End Function
#End Region

#Region "           GetFiles"
    ''' <summary>
    ''' If the current instance is a Folder then returns an ArrayList of the CShItems of Files 
    ''' contained in the current instance. Otherwise returns Nothing.
    ''' </summary>
    ''' <returns>An ArrayList of the CShItems of the Files in the current instance. If the 
    ''' current instance is not a Folder, returns Nothing. If there are no Files in the 
    ''' current instance, returns an empty ArrayList.</returns>
    ''' <remarks></remarks>
    Public Function GetFiles() As ArrayList
        Dim F() As CShItem = Me.Files
        If F Is Nothing Then Return Nothing
        Dim AF As New ArrayList
        AF.AddRange(F)
        Return AF
    End Function

    ''' <summary>
    ''' Returns the Files of this sub-folder, filtered by a filtering string, as an
    '''   ArrayList of CShitems
    ''' </summary>
    ''' <param name="Filter">A filter string (for example: *.Doc)</param>
    ''' <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
    ''' <remarks>Added 8/22/2012</remarks>
    Public Function GetFiles(ByVal Filter As String) As ArrayList
        GetFiles = New ArrayList()
        If m_IsFolder Then
            Filter = Filter.ToLower
            For Each CSI As CShItem In Me.Files
                If CSI.DisplayName.ToLower Like Filter Then
                    GetFiles.Add(CSI)
                End If
            Next
        End If
    End Function

#End Region

#Region "           GetItems"
    'Added 10/16/2011
    ''' <summary>
    ''' Returns the Directories and Files of this sub-folder as a sorted
    '''   ArrayList of CShitems
    ''' </summary>
    ''' <returns>An ArrayList of CShItems. May return an empty ArrayList if there are none.</returns>
    ''' <remarks>This version is the Optimized version added after any distribution of v2.14</remarks>
    Public Function GetItems() As ArrayList
        Dim rVal As New ArrayList()
        If m_IsFolder Then
            Dim Flags As SHCONTF = SHCONTF.INCLUDEHIDDEN

            SyncLock (LockObj)
                If m_Directories Is Nothing Then Flags = Flags Or SHCONTF.FOLDERS
                If m_Files Is Nothing Then Flags = Flags Or SHCONTF.NONFOLDERS
                If Flags <> SHCONTF.INCLUDEHIDDEN Then 'if already have both already, just report what we have
                    Dim Items As CShItemCollection = GetContents(Flags)
                    GetItems = New ArrayList(Items.Count)       'Actual expected return
                    Dim Dirs As New ArrayList(Items.Count)      'trade space for time - capacity set to max possible
                    Dim Files As New ArrayList(Items.Count)     'trade space for time - capacity set to max possible
                    For Each Item As CShItem In Items
                        If Item.IsFolder Then
                            Dirs.Add(Item)
                        Else : Files.Add(Item)
                        End If
                    Next
                    If m_Directories Is Nothing Then
                        m_Directories = New CShItemCollection(Me)   'First time we even asked
                        m_Directories.AddRange(Dirs)
                    End If
                    If m_Files Is Nothing Then
                        m_Files = New CShItemCollection(Me)         'First time we even asked
                        m_Files.AddRange(Files)
                    End If
                End If
                rVal.AddRange(m_Directories)    '7/14/2012 - trust in SyncLock
                rVal.AddRange(m_Files)          '7/14/2012 - trust in SyncLock
                'rVal.AddRange(Me.Directories)   'use this instead of local list as a last sanity precaution and to prevent race conditions
                'rVal.AddRange(Me.Files)         'use this instead of local list as a last sanity precaution and to prevent race conditions
            End SyncLock                        'should have prevented race conditions, but Windows messages can be funky
            rVal.Sort()
        End If
        Return rVal
    End Function

    'Previous, unoptimized version of GetItems
    'Public Function GetItems() As ArrayList
    '    Dim rVal As New ArrayList()
    '    If m_IsFolder Then
    '        rVal.AddRange(Me.Directories)
    '        rVal.AddRange(Me.Files)
    '        rVal.Sort()
    '        Return rVal
    '    Else
    '        Return rVal
    '    End If
    'End Function

#End Region

#Region "           GetFileName"
    '''<summary>GetFileName returns the Full file name of this item.
    '''  Specifically, for a link file (xxx.txt.lnk for example) the
    '''  DisplayName property will return xxx.txt, this method will
    '''  return xxx.txt.lnk.</summary>
    ''' <returns>The Name of this instance</returns>
    ''' <remarks>In most cases this is equivalent to
    '''  System.IO.Path.GetFileName(m_Path).  However, some m_Paths
    '''  actually are GUIDs.  In that case, this routine returns the
    '''  DisplayName</remarks>
    Public Function GetFileName() As String
        If Me.Path.StartsWith("::{") Then 'Path is really a GUID
            Return Me.DisplayName
        Else
            If m_IsDisk Then
                Return Me.Path.Substring(0, 1)
            Else
                Return IO.Path.GetFileName(Me.Path)
            End If
        End If
    End Function
#End Region

#Region "           ResetIcon"

    ''' <summary>
    ''' Resets the IconIndex to the current value
    ''' </summary>
    ''' <remarks>Certain, seldom occuring, Dynamic Updates will cause the actual Icon and its' IconIndex to change.
    '''          The handlers for these Update Events should Reset the IconIndex to show the new Icon.</remarks>
    Public Sub ResetIconIndex()
        m_IconIndexNormal = -1        'index into the SystemImageListManager list for Normal icon
        m_IconIndexOpen = -1          'index into the SystemImageListManager list for Open icon
        m_IconIndexNormalOrig = -1    'index into the System Image list for Normal icon
        m_IconIndexOpenOrig = -1      'index into the SystemImage list for Open icon
        SystemImageListManager.GetIconIndex(Me, False)
        SystemImageListManager.GetIconIndex(Me, True)
    End Sub

#End Region

#Region "           GetLinkTarget"
    ''' <summary>
    ''' If the current instance (Me) is a Link then return the name of the Target of this link.
    ''' </summary>
    ''' <returns>If this instance is a link, then the name of the link target. If current instance
    ''' is not a link, then returns the empty string.</returns>
    ''' <remarks>Illustrates use of Activator.CreateInstance.</remarks>
    Public Function GetLinkTarget() As String
        Dim pf As IPersistFile
        Dim tShellLink As Type
        Dim m_Link As IShellLink
        tShellLink = Type.GetTypeFromCLSID(CLSID_ShellLink)
        m_Link = CType(Activator.CreateInstance(tShellLink), IShellLink)
        If Me.IsLink Then
            pf = CType(m_Link, IPersistFile)
            Dim HR As Integer = pf.Load(Me.Path, 0)
            If HR = S_OK Then
                Dim wfd As WIN32_FIND_DATA
                Dim SB As StringBuilder = New StringBuilder(MAX_PATH)
                HR = m_Link.GetPath(SB, SB.Capacity, wfd, SLGP.UNCPRIORITY)
                If HR = S_OK Then
                    Return SB.ToString()
                End If
            End If
        End If
        If Not IsNothing(m_Link) Then Marshal.ReleaseComObject(m_Link)
        Return ""
    End Function

#End Region

#Region "           ToString"
    ''' <summary>
    ''' Returns the DisplayName as the normal ToString value
    ''' </summary>
    ''' <returns>The DisplayName</returns>
    Public Overrides Function ToString() As String
        Return m_DisplayName
    End Function
#End Region

#Region "           Debug Dumper"
    ''' <summary>
    ''' Writes some key properties of this CShItem to the Debug console.
    ''' </summary>
    ''' 
    Public Sub DebugDump()
        Debug.WriteLine("DisplayName = " & m_DisplayName)
        Debug.WriteLine("PIDL        = " & m_Pidl.ToString)
        Debug.WriteLine(vbTab & "Path        = " & m_Path)
        Debug.WriteLine(vbTab & "TypeName    = " & Me.TypeName)
        Debug.WriteLine(vbTab & "iIconNormal = " & m_IconIndexNormal)
        Debug.WriteLine(vbTab & "iIconSelect = " & m_IconIndexOpen)
        Debug.WriteLine(vbTab & "IsBrowsable = " & m_IsBrowsable)
        Debug.WriteLine(vbTab & "IsFileSystem= " & m_IsFileSystem)
        Debug.WriteLine(vbTab & "IsFolder    = " & m_IsFolder)
        Debug.WriteLine(vbTab & "IsLink    = " & m_IsLink)
        Debug.WriteLine(vbTab & "IsDropTarget = " & m_IsDropTarget)
        Debug.WriteLine(vbTab & "IsReadOnly   = " & Me.IsReadOnly)
        Debug.WriteLine(vbTab & "CanCopy = " & Me.CanCopy)
        Debug.WriteLine(vbTab & "CanLink = " & Me.CanLink)
        Debug.WriteLine(vbTab & "CanMove = " & Me.CanMove)
        Debug.WriteLine(vbTab & "CanDelete = " & Me.CanDelete)
        If m_IsFolder Then
            If Not IsNothing(m_Directories) Then
                Debug.WriteLine(vbTab & "Directory Count = " & m_Directories.Count)
            Else
                Debug.WriteLine(vbTab & "Directory Count Not yet set")
            End If
        End If
    End Sub
#End Region

#Region "           GetDropTargetOf"
    ''' <summary>
    ''' This method uses the CreateViewObject method of IShellFolder to obtain the IDropTarget of this
    ''' CShItem instance. 
    ''' </summary>
    ''' <param name="tn">The control in which the GUI representation of this CShItem lives.</param>
    ''' <returns>If successful, the IDropTarget interface of the Folder represented by this CShItem.
    ''' If unsuccessful, returns Nothing.</returns>
    ''' <remarks>A similar function exists in the ShellHelper class. GetDropTargetOf is more efficient.</remarks>
    Public Function GetDropTargetOf(ByVal tn As Control) As IDropTarget
        If IsNothing(Me.Folder) Then Return Nothing
        Dim pInterface As IntPtr
        Dim theInterface As IDropTarget = Nothing
        Dim tnH As IntPtr = tn.Handle
        If Me.Folder.CreateViewObject(tnH, ShellDll.ShellAPI.IID_IDropTarget, pInterface) = S_OK Then
            theInterface = Marshal.GetTypedObjectForIUnknown(pInterface, GetType(ShellDll.IDropTarget))
            Return theInterface
        Else
            Return Nothing
        End If
    End Function
#End Region

#End Region

#End Region

#Region "   Private Instance Methods"

#Region "       GetContents"

    '''<summary>
    ''' Returns the requested Items of this Folder as a CShitemCollection
    '''</summary>
    ''' <param name="flags">A set of one or more SHCONTF flags indicating which items to return</param>
    Private Function GetContents(ByVal flags As SHCONTF) As CShItemCollection
        Dim rVal As New CShItemCollection(Me)
        If Me.Folder Is Nothing Then Return rVal 'Added 10/22/2011 to deal with certain Virtual Folders
        Dim ptr As IntPtr
        Dim itm As CShItem
        'Debug.WriteLine("GContent " & Me.Path)
        'Dim StTime As DateTime = Now()
        'Dim content As ArrayList = GetContentPtrs(flags)       '11/09/2013 - should have been commented out originally
        'Debug.WriteLine("GPtrRel " & Now().Subtract(StTime).TotalMilliseconds.ToString & " ms")
        'StTime = Now()
        'For Each ptr In content
        For Each ptr In GetContentPtrs(flags)
            If ptr = IntPtr.Zero Then                                               '11/09/2013 - Investigate other
                Debug.WriteLine("Content=IntPtr.Zero while filling " & Me.Path)     '11/09/2013 - Investigate other
                Marshal.FreeCoTaskMem(ptr)                                          '11/09/2013 - Investigate other
                Continue For                                                        '11/09/2013 - Investigate other
            Else
                Try                                         'ASUS Fix 'mod 06/27/09 First fix added
                    itm = New CShItem(ptr, Me)
                    rVal.Add(itm)
                    'Catch ex As InvalidCastException             'ASUS Fix - superceeded 11/13/2013
                    '    Debug.WriteLine("GetContents - InvCast") 'ASUS Fix
                Catch ex As Exception                                           '11/09/2013 - Investigate other
                    'Debug.WriteLine("GetContents - Exception: " & ex.Message)   '11/09/2013 - Investigate other
                    'Debug.WriteLine("Processing " & Me.Path)                    '11/09/2013 - Investigate other
                    'DumpPidl(ptr)                                               '11/09/2013 - Investigate other
                Finally           'ASUS Fix
                    Marshal.FreeCoTaskMem(ptr)
                End Try           'ASUS Fix
            End If                                                          '11/09/2013 - Investigate other
        Next
        'Debug.WriteLine("BuildItems " & Now().Subtract(StTime).TotalMilliseconds.ToString & " ms")
        Return rVal
    End Function

    ''' <summary>
    ''' Given a relative PIDL (relative to Me.Folder) determine if item is a Folder.
    ''' </summary>
    ''' <param name="ptr">A relative PIDL, relative to Me.Folder</param>
    ''' <returns>True if item is a Folder, False is item is NOT a Folder.</returns>
    ''' <remarks>Container files (such as .zip or .cab) are marked as a "Folder" in WinXP and above, so
    ''' some further testing must be done on XP and above systems. We define such items as non-Folders.</remarks>
    Private Function IsFolderRel(ByVal ptr As IntPtr) As Boolean
        IsFolderRel = False         'assume it is not
        Dim attrFlag As SFGAO = SFGAO.FOLDER Or SFGAO.STREAM
        'Note: for GetAttributesOf, we must provide an array, in all cases with 1 element
        Dim aPidl(0) As IntPtr
        aPidl(0) = ptr
        Me.Folder.GetAttributesOf(1, aPidl, attrFlag)
        If Not XPorAbove Then
            If CBool(attrFlag And SFGAO.FOLDER) Then 'is folder
                IsFolderRel = True
            End If
        Else         'XP or above
            If CBool(attrFlag And SFGAO.FOLDER) AndAlso _
               Not CBool(attrFlag And SFGAO.STREAM) Then   'is folder
                IsFolderRel = True
            End If
        End If

    End Function
    '''<summary>
    ''' Returns the requested Items of this Folder as an ArrayList of relative PIDLs 
    ''' (caller must free the pidls after use).
    '''</summary>
    ''' <param name="flags">A set of one or more SHCONTF flags indicating which items to return</param>
    ''' <returns>On error, returns an empty (count=0) ArrayList. Otherwise, returns the relative PIDLs of
    ''' the requested (via flags param) items in this Folder.</returns>
    Private Function GetContentPtrs(ByVal flags As SHCONTF) As ArrayList
        Dim rVal As New ArrayList
        Dim HR As Integer
        Dim IEnum As IEnumIDList = Nothing
        'UPDATE: Vista and above strictly respect the SHCONTF flags. The "flags" param is now used only to determine what user wants
        HR = Me.Folder.EnumObjects(0, SHCONTF.INCLUDEHIDDEN Or SHCONTF.FOLDERS Or SHCONTF.NONFOLDERS, IEnum)     'new code (12/11/09)
        'HR = Me.Folder.EnumObjects(0, flags, IEnum)    'Old Code
        If HR = NOERROR Then
            Dim ptr As IntPtr = IntPtr.Zero
            Dim itemCnt As Integer
            HR = IEnum.Next(1, ptr, itemCnt)
            Do While HR = NOERROR AndAlso itemCnt > 0 AndAlso Not ptr.Equals(IntPtr.Zero)
                'BEGIN new code (12/11/09)
                Dim ItemIsFolder As Boolean = IsFolderRel(ptr)
                If ItemIsFolder And Not CBool(flags And SHCONTF.FOLDERS) OrElse _
                  (Not ItemIsFolder And Not CBool(flags And SHCONTF.NONFOLDERS)) Then
                    Marshal.FreeCoTaskMem(ptr)
                Else
                    rVal.Add(ptr)
                End If
                'END new code
                ptr = IntPtr.Zero
                itemCnt = 0
                HR = IEnum.Next(1, ptr, itemCnt)
            Loop
            If HR <> 1 Then GoTo HRError '1 means no more
        Else : GoTo HRError
        End If
        'Normal Exit
NORMAL: If Not IsNothing(IEnum) Then Marshal.ReleaseComObject(IEnum)
        Return rVal

        ' Error Exit for all Com errors
HRError:  'not ready disks will return the following error
        'If HR = &HFFFFFFFF800704C7 Then
        '    GoTo NORMAL
        'ElseIf HR = &HFFFFFFFF80070015 Then
        '    GoTo NORMAL
        '    'unavailable net resources will return these
        'ElseIf HR = &HFFFFFFFF80040E96 Or HR = &HFFFFFFFF80040E19 Then
        '    GoTo NORMAL
        'ElseIf HR = &HFFFFFFFF80004001 Then 'Certain "Not Implemented" features will return this
        '    GoTo NORMAL
        ' Sharepoint folders return this at the end of the enum
        If HR = &HFFFFFFFF80004005 Then
            GoTo NORMAL
            'ElseIf HR = &HFFFFFFFF800704C6 Then
            '    GoTo NORMAL
        End If
#If DEBUG Then
        'If Not IsNothing(IEnum) Then Marshal.ReleaseComObject(IEnum)
        'Marshal.ThrowExceptionForHR(HR)
#End If
        rVal = New ArrayList 'sometimes it is a non-fatal error,ignored
        GoTo NORMAL
    End Function

#End Region

#Region "       Really nasty Pidl manipulation"

    ''' <summary>
    ''' Get Size in bytes of the first (possibly only)
    '''  SHItem in an ID list.  Note: the full size of
    '''   the item is the sum of the sizes of all SHItems
    '''   in the list!!
    ''' </summary>
    ''' <param name="pidl">A pointer to a PIDL.</param>
    ''' 
    Private Shared Function ItemIDSize(ByVal pidl As IntPtr) As Integer
        If Not pidl.Equals(IntPtr.Zero) Then
            Dim b(1) As Byte
            Marshal.Copy(pidl, b, 0, 2)
            Return b(1) * 256 + b(0)
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Computes the actual size of the ItemIDList (all SHItems) pointed to by pidl.
    ''' </summary>
    ''' <param name="pidl">The pidl pointing to an ItemIDList</param>
    '''<returns> Returns actual size of the ItemIDList, less the terminating nulnul</returns> 
    Public Shared Function ItemIDListSize(ByVal pidl As IntPtr) As Integer
        If Not pidl.Equals(IntPtr.Zero) Then
            Dim i As Integer = ItemIDSize(pidl)
            Dim b As Integer = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Do While b > 0
                i += b
                b = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Loop
            Return i
        Else : Return 0
        End If
    End Function
    ''' <summary>
    ''' Counts the total number of SHItems in input pidl
    ''' </summary>
    ''' <param name="pidl">The pidl to obtain the count for</param>
    ''' <returns> Returns the count of SHItems pointed to by pidl</returns> 
    Public Shared Function PidlCount(ByVal pidl As IntPtr) As Integer
        If Not pidl.Equals(IntPtr.Zero) Then
            Dim cnt As Integer = 0
            Dim i As Integer = 0
            Dim b As Integer = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Do While b > 0
                cnt += 1
                i += b
                b = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Loop
            Return cnt
        Else : Return 0
        End If
    End Function

    ''' <summary>
    ''' Given a PIDL(Pointer to ID List) as IntPtr, return an Array of PIDL, one for each ID in the List.
    ''' Each PIDL in the returned Array will be a single, well formed and terminated ID.
    ''' </summary>
    ''' <param name="pidl">The PIDL to be Factored</param>
    ''' <returns>An Array of PIDL, each a Single Relative PIDL</returns>
    ''' <remarks>The returned PIDLs must be Released when no longer needed by calling PIDLFree.</remarks>
    Public Shared Function DecomposePIDL(ByVal pidl As IntPtr) As IntPtr()
        Dim lim As Integer = ItemIDListSize(pidl)
        Dim PIDLs(PidlCount(pidl) - 1) As IntPtr
        Dim i As Integer = 0
        Dim curB As Integer
        Dim offSet As Integer = 0
        Do While curB < lim
            Dim thisPtr As IntPtr = New IntPtr(pidl.ToInt64 + curB) '6/8/2012 - ToInt64 works on both 32 & 64 bit systems
            offSet = Marshal.ReadByte(thisPtr) + (Marshal.ReadByte(thisPtr, 1) * 256)
            PIDLs(i) = Marshal.AllocCoTaskMem(offSet + 2)
            Dim b(offSet + 1) As Byte
            Marshal.Copy(thisPtr, b, 0, offSet)
            b(offSet) = 0 : b(offSet + 1) = 0
            Marshal.Copy(b, 0, PIDLs(i), offSet + 2)
            'DumpPidl(PIDLs(i))
            curB += offSet
            i += 1
        Loop
        Return PIDLs
    End Function

    ''' <summary>
    ''' Given a PIDL as IntPtr, Allocate memory for and return a Clone of the input PIDL.
    ''' </summary>
    ''' <param name="pidl">A PIDL to be Cloned</param>
    ''' <returns>A Clone of the input PIDL</returns>
    ''' <remarks>The Clone must be Released when no longer needed by calling PIDLFree</remarks>
    Friend Shared Function PIDLClone(ByVal pidl As IntPtr) As IntPtr
        Dim cb As Integer = ItemIDListSize(pidl)
        Dim b(cb + 1) As Byte
        Marshal.Copy(pidl, b, 0, cb) 'not including terminating nulnul
        b(cb) = 0 : b(cb + 1) = 0 'force to nulnul
        PIDLClone = Marshal.AllocCoTaskMem(cb + 2)
        Marshal.Copy(b, 0, PIDLClone, cb + 2)
    End Function

    ''' <summary>
    ''' Frees a PIDL, releasing its' allocated memory
    ''' </summary>
    ''' <param name="pidl">The PIDL to be Freed</param>
    ''' <remarks></remarks>
    Friend Shared Sub PIDLFree(ByVal pidl As IntPtr)
        If pidl <> IntPtr.Zero Then
            Marshal.FreeCoTaskMem(pidl)
        End If
    End Sub

#Region "       Pidl Equality Methods"
    'TODO: Test IsReallyEqual on Fat32.
    ''' <summary>
    ''' IsReallyEqual compares Pidls using the IsEqual routine. If IsEqual declares them Equal, IsReallyEqual
    ''' checks the Last (or relative) Pidls using a byte by byte comparison. This is necessary because new file
    ''' versions created by File->Save will compare Equal in IsEqual, when we really want to know that a new version
    ''' of a file has been created. Fortunately, the relative Pidl of a new version will differ in a few bytes from
    ''' the relative Pidl of the previous version.
    ''' This Function is no longer used by ExpTreeLib.
    ''' </summary>
    ''' <param name="Pidl1">IntPtr pointing to an ItemIDList.</param>
    ''' <param name="Pidl2">IntPtr pointing to an ItemIDList.</param>
    ''' <returns>True is completely equal, False otherwise.</returns>
    ''' <remarks>At this point, this has been tested on NTFS file systems only.</remarks>
    Friend Shared Function IsReallyEqual(ByVal Pidl1 As IntPtr, ByVal Pidl2 As IntPtr) As Boolean
        'IsReallyEqual = IsEqual(Pidl1, Pidl2)
        'If IsReallyEqual AndAlso Win2KOrAbove Then           'IsEqual says they are -- if Win2KOrAbove, then check the last ItemID
        '    IsReallyEqual = AreBytesEqual(ILFindLastID(Pidl1), ILFindLastID(Pidl2))
        '    'If Not IsReallyEqual Then
        '    '    Debug.WriteLine("IsReallyEqual found mismatch")
        '    '    DumpPidl(Pidl1)
        '    '    DumpPidl(Pidl2)
        '    'End If
        'End If
    End Function

    ''' <summary>
    ''' AreBytesEqual performs a binary comparison of the contents of two ItemIDLists pointed to by two Pidls.
    ''' </summary>
    ''' <param name="Pidl1">IntPtr pointing to an ItemIDList.</param>
    ''' <param name="pidl2">IntPtr pointing to an ItemIDList.</param>
    ''' <returns>True if all bytes are the same, False otherwise.</returns>
    ''' <remarks>A substitute for ILIsEqual on pre-Win2K systems, and used by IsReallyEqual when binary
    ''' comparison is needed on Win2K and above systems.</remarks>
    Public Shared Function AreBytesEqual(ByVal Pidl1 As IntPtr, ByVal pidl2 As IntPtr) As Boolean
        Dim cb1 As Integer, cb2 As Integer
        cb1 = ItemIDListSize(Pidl1)
        cb2 = ItemIDListSize(pidl2)
        If cb1 <> cb2 Then Return False
        Dim lim32 As Integer = cb1 \ 4

        Dim i As Integer
        For i = 0 To lim32 - 1
            If Marshal.ReadInt32(Pidl1, i * 4) <> Marshal.ReadInt32(pidl2, i * 4) Then
                'Debug.WriteLine("Mismatch at Byte " & i * 4 & " (&H" & Hex(i * 4) & ")")
                Return False
            End If
        Next
        Dim limB As Integer = cb1 Mod 4
        Dim offset As Integer = lim32 * 4
        For i = 0 To limB - 1
            If Marshal.ReadByte(Pidl1, offset + i) <> Marshal.ReadByte(pidl2, offset + i) Then
                'Debug.WriteLine("Mismatch at Byte " & i + offset & " (&H" & Hex(i + offset) & ")")
                Return False
            End If
        Next
        Return True 'made it to here, so they are equal
    End Function

    ''' <summary>
    ''' IsEqual compares two ItemIDLists. On Win2K and above systems, it uses the ILIsEqual API, which only
    ''' compares portions of each ItemID. On such systems, the other portions of the ItemID may differ in a 
    ''' few bytes -- typically this is desired behavior, but not in UPDATEDIR cases which do a Byte comparison in addition to IsEqual.
    ''' On Pre-Win2K systems, it performs a binary comparison of the entire content of the ItemIDLists, this
    ''' is OK behavior on such systems.
    ''' </summary>
    ''' <param name="Pidl1">IntPtr pointing to an ItemIDList.</param>
    ''' <param name="Pidl2">IntPtr pointing to an ItemIDList.</param>
    ''' <returns>True if ILIsEqual returns or would return True, False otherwise.</returns>
    ''' <remarks></remarks>
    Public Shared Function IsEqual(ByVal Pidl1 As IntPtr, ByVal Pidl2 As IntPtr) As Boolean
        If Win2KOrAbove Then
            Return ILIsEqual(Pidl1, Pidl2)
        Else 'do hard way, may not work for some files/folders on XP
            Return AreBytesEqual(Pidl1, Pidl2)
        End If
    End Function

    ''' <summary>
    ''' Not currently used. Compares two PIDLs Relative to the instance Folder using the folder.CompareIDs API call.
    ''' </summary>
    ''' <param name="RelPidl1">First Relative PIDL to compare.</param>
    ''' <param name="RelPidl2">Second Relative PIDL to compare.</param>
    ''' <returns>True if Equal, False otherwise.</returns>
    ''' <remarks></remarks>
    Public Function PidlsEqual(ByVal RelPidl1 As IntPtr, ByVal RelPidl2 As IntPtr) As Boolean
        If Me.Folder Is Nothing Then Return IsEqual(RelPidl1, RelPidl2)
        PidlsEqual = False            'assume not equal
        Dim lParam As UInt32 = SHCIDS.CANONICALONLY
        Dim H As Integer
        H = Me.Folder.CompareIDs(lParam, RelPidl1, RelPidl2)
        If H >= 0 Then
            Dim Code As Integer = H And &H7777
            If Code = 0 Then Return True
        Else
            Return IsEqual(RelPidl1, RelPidl2)
        End If
    End Function
#End Region

    ''' <summary>
    ''' Concatenates the contents of two pidls into a new Pidl (ended by 00)
    ''' allocating CoTaskMem to hold the result,
    ''' placing the concatenation (followed by 00) into the
    ''' allocated Memory,
    ''' and returning an IntPtr pointing to the allocated mem
    ''' </summary>
    ''' <param name="pidl1">IntPtr to a well formed SHItemIDList or IntPtr.Zero</param>
    ''' <param name="pidl2">IntPtr to a well formed SHItemIDList or IntPtr.Zero</param>
    ''' <returns>Returns a ptr to an ItemIDList containing the 
    '''   concatenation of the two (followed by the req 2 zeros
    '''   Caller must Free this pidl when done with it</returns>
    ''' <remarks>On Win2k or above systems, will use the API function ILCombine, otherwise performs
    ''' byte array manipulation to accomplish the same thing.
    ''' Caller must free the returned Pidl when no longer needed.</remarks> 
    Public Shared Function concatPidls(ByVal pidl1 As IntPtr, ByVal pidl2 As IntPtr) As IntPtr
        If Win2KOrAbove Then
            Return ILCombine(pidl1, pidl2)
        Else
            Dim cb1 As Integer, cb2 As Integer
            cb1 = ItemIDListSize(pidl1)
            cb2 = ItemIDListSize(pidl2)
            Dim rawCnt As Integer = cb1 + cb2
            If (rawCnt) > 0 Then
                Dim b(rawCnt + 1) As Byte
                If cb1 > 0 Then
                    Marshal.Copy(pidl1, b, 0, cb1)
                End If
                If cb2 > 0 Then
                    Marshal.Copy(pidl2, b, cb1, cb2)
                End If
                Dim rVal As IntPtr = Marshal.AllocCoTaskMem(cb1 + cb2 + 2)
                b(rawCnt) = 0 : b(rawCnt + 1) = 0
                Marshal.Copy(b, 0, rVal, rawCnt + 2)
                Return rVal
            Else
                Return IntPtr.Zero
            End If
        End If
    End Function

    ''' <summary>
    ''' TrimPidl returns an ItemIDList with the last ItemID trimed off.
    '''  It's purpose is to generate an ItemIDList for the Parent of a
    '''  Special Folder which can then be processed with DesktopBase.BindToObject,
    '''  yeilding a Folder for the parent of the Special Folder
    '''  It also creates and passes back a RELATIVE pidl for this item
    ''' </summary>
    ''' <param name="pidl">A pointer to a well formed ItemIDList. The PIDL to trim</param>
    ''' <param name="relPidl">BYREF IntPtr which will point to a new relative pidl
    '''        containing the contents of the last ItemID in the ItemIDList
    '''        terminated by the required 2 nulls.</param>
    ''' <returns> an ItemIDList with the last element removed.</returns>
    '''  <remarks>Caller must Free BOTH the returned, Trimmed PIDL and the 
    ''' returned relPidl.
    '''</remarks>
    Public Shared Function TrimPidl(ByVal pidl As IntPtr, ByRef relPidl As IntPtr) As IntPtr
        Dim cb As Integer = ItemIDListSize(pidl)
        Dim b(cb + 1) As Byte
        Marshal.Copy(pidl, b, 0, cb)
        Dim prev As Integer = 0
        Dim i As Integer = b(0) + (b(1) * 256)
        Do While i > 0 AndAlso i < cb
            prev = i
            i += b(i) + (b(i + 1) * 256)
        Loop
        If (prev + 1) < cb Then
            'first set up the relative pidl
            b(cb) = 0
            b(cb + 1) = 0
            Dim cb1 As Integer = b(prev) + (b(prev + 1) * 256)
            relPidl = Marshal.AllocCoTaskMem(cb1 + 2)
            Marshal.Copy(b, prev, relPidl, cb1 + 2)
            b(prev) = 0 : b(prev + 1) = 0
            Dim rVal As IntPtr = Marshal.AllocCoTaskMem(prev + 2)
            Marshal.Copy(b, 0, rVal, prev + 2)
            Return rVal
        Else
            Return IntPtr.Zero
        End If
    End Function

    '''<summary>ILFindLastID -- returns a pointer to the last ITEMID in a valid
    ''' ITEMIDLIST. Returned pointer SHOULD NOT be released since it
    ''' points to place within the original PIDL</summary>
    '''<returns>IntPtr pointing to last ITEMID in ITEMIDLIST structure,
    ''' Returns IntPtr.Zero if given a null pointer.
    ''' If given a pointer to the Desktop, will return same pointer.</returns>
    '''<remarks>Uses the API ILFindLastID function if Win2k or above, otherwise
    ''' computes the same thing.</remarks>
    Public Shared Function ILFindLastID(ByVal pidl As IntPtr) As IntPtr
        If Win2KOrAbove Then
            Return ShellDll.ShellAPI.ILFindLastID(pidl)
        Else
            Dim prev As Integer = 0
            Dim i As Integer = 0
            Dim b As Integer = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Do While b > 0
                prev = i
                i += b
                b = Marshal.ReadByte(pidl, i) + (Marshal.ReadByte(pidl, i + 1) * 256)
            Loop
            Return New IntPtr(pidl.ToInt64 + prev)  '6/8/2012 - ToInt64 works on both 32 & 64 bit systems (though code is never executed on 64 bit systems)
        End If
    End Function

#Region "   DumpPidl Routines"
    ''' <summary>
    ''' Dumps, to the Debug output, the contents of the mem block pointed to by
    ''' a PIDL. Depends on the internal structure of a PIDL
    ''' </summary>
    ''' <param name="pidl">The IntPtr(a PIDL) pointing to the block to dump</param>
    ''' 
    Public Shared Sub DumpPidl(ByVal pidl As IntPtr)
        Dim cb As Integer = ItemIDListSize(pidl)
        Debug.WriteLine("PIDL " & pidl.ToString & " contains " & cb & " bytes")
        If cb > 0 Then
            Dim b(cb + 1) As Byte
            Marshal.Copy(pidl, b, 0, cb + 1)
            Dim pidlCnt As Integer = 1
            Dim i As Integer = b(0) + (b(1) * 256)
            Dim curB As Integer = 0
            Do While i > 0
                Debug.Write("ItemID #" & pidlCnt & " Length = " & i)
                DumpHex(b, curB, curB + i - 1)
                pidlCnt += 1
                curB += i
                i = b(curB) + (b(curB + 1) * 256)
            Loop
        End If
    End Sub

    '''<summary>Dump a portion or all of a Byte Array to Debug output</summary>
    '''<param name = "b">A single dimension Byte Array</param>
    '''<param name = "sPos">Optional start index of area to dump (default = 0)</param>
    '''<param name = "epos">Optional last index position to dump (default = end of array)</param>
    Public Shared Sub DumpHex(ByVal b() As Byte, _
                            Optional ByVal sPos As Integer = 0, _
                            Optional ByVal ePos As Integer = 0)
        If ePos = 0 Then ePos = b.Length - 1
        Dim j As Integer
        Dim curB As Integer = sPos
        Dim sTmp As String
        Dim ch As Char
        Dim SBH As New StringBuilder()
        Dim SBT As New StringBuilder()
        For j = 0 To ePos - sPos
            If j Mod 16 = 0 Then
                Debug.WriteLine(SBH.ToString & SBT.ToString)
                SBH = New StringBuilder() : SBT = New StringBuilder("          ")
                SBH.Append(HexNum(j + sPos, 4) & "). ")
            End If
            If b(curB) < 16 Then
                sTmp = "0" & Hex(b(curB))
            Else
                sTmp = Hex(b(curB))
            End If
            SBH.Append(sTmp) : SBH.Append(" ")
            ch = Chr(b(curB))
            If Char.IsControl(ch) Then
                SBT.Append(".")
            Else
                SBT.Append(ch)
            End If
            curB += 1
        Next

        Dim fill As Integer = (j) Mod 16
        If fill <> 0 Then
            SBH.Append(" "c, 48 - (3 * ((j) Mod 16)))
        End If
        Debug.WriteLine(SBH.ToString & SBT.ToString)
    End Sub

    ''' <summary>
    ''' Formats an Integer into a String representation of the Hexidecimal representation of that number with
    ''' enough leading zero Chars to fill nrChars number of characters.
    ''' </summary>
    ''' <param name="num">The Integer to Format</param>
    ''' <param name="nrChrs">The desired size of the returned String</param>
    ''' <returns>A String with the Hex representation of the Integer parameter</returns>
    ''' <remarks></remarks>
    Public Shared Function HexNum(ByVal num As Integer, ByVal nrChrs As Integer) As String
        Dim h As String = Hex(num)
        Dim SB As New StringBuilder()
        Dim i As Integer
        For i = 1 To nrChrs - h.Length
            SB.Append("0")
        Next
        SB.Append(h)
        Return SB.ToString
    End Function
#End Region

#End Region

#End Region

#Region "   TagComparer Class"
    '''<summary> It is sometimes useful to sort a list of TreeNodes,
    ''' ListViewItems, or other objects in an order based on CShItems in their Tag.
    ''' TagComparer is a Icomparer Class for that situation. Sorting is based on CShItem.CompareTo
    ''' </summary>
    Public Class TagComparer
        Implements IComparer
        ''' <summary>
        ''' Compares the .Tags of two Objects, which must be CShItems.
        ''' </summary>
        ''' <param name="x">First Object with a CShItem in its' .Tag</param>
        ''' <param name="y">Second Object with a CShItem in its' .Tag</param>
        ''' <returns>-1, 0, or 1 depending on the results of comparing the two CShItems</returns>
        ''' <remarks>See CShItem.CompareTo for discussion of the Comparison of two CShItems</remarks>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
         Implements IComparer.Compare
            Dim xTag As CShItem = x.tag
            Dim yTag As CShItem = y.tag
            Return xTag.CompareTo(y.tag)
        End Function
    End Class
#End Region

#Region "       Update Methods"

    ''' <summary>
    ''' CShItemUpdate is the Event Raised to notify the using application, typically the GUI portion, of changes made to
    ''' Folders and Files that the application has an interest in.<br />
    ''' See <see cref="ExpTreeLib.ShellItemUpdateEventArgs.UpdateType">UpdateType</see> for details.
    ''' </summary>
    ''' <param name="sender">The CShItem of the Folder that has changes in its' content.</param>
    ''' <param name="e">A <see cref="ShellItemUpdateEventArgs">ShellItemUpdateEventArgs</see> which provides information about the change.</param>
    ''' <remarks></remarks>
    Public Shared Event CShItemUpdate(ByVal sender As Object, ByVal e As ShellItemUpdateEventArgs)

    ''' <summary>
    ''' CShItemUpdateType is an Enum of the various types of change that will be reported in a ShellItemUpdateEventArgs.
    ''' </summary>
    ''' <remarks>This Enum is also used by the CShItemUpdater Class to report change types to CShItem.Update which passes it 
    '''          on to the Application.</remarks>
    Public Enum CShItemUpdateType
        Created
        IconChange
        Updated
        UpdateDir
        Renamed
        Deleted
        MediaChange
    End Enum

    Private Sub ResetInfo()
        m_HasDispType = False
        m_IsReadOnlySetup = False
        m_XtrInfo = False
        m_HasSubFoldersSetup = False
        If Me.W32Data IsNot Nothing AndAlso TypeOf Me.W32Data Is W32Find_Data Then Me.W32Data = Nothing
        ResetIconIndex()
    End Sub

    Private Sub ResetChildren()
        'propogate changes to the known children
        If m_Files IsNot Nothing Then
            For Each item As CShItem In m_Files
                item.ResetInfo()
            Next
        End If
        If m_Directories IsNot Nothing Then
            For Each item As CShItem In m_Directories
                item.ResetInfo()
            Next
        End If
    End Sub
    ''' <summary>
    ''' On a Rename operation, we simply modify the existant CShItem to reflect the new PIDL, Path, and
    ''' Folder (if a folder).
    ''' Since in this version of CShItem, m_Pidl is an absolute, fully qualified pidl, it must be updated
    ''' when any of the ancestor Folders is Renamed/Moved. 
    ''' This is also true for both the Path property and the Folder property.
    ''' For Pidls, we actually perform the update here. For Paths, we simply set it to String.Empty and let
    ''' me.Path recreate it as needed.  The latter implies that m_Path should never be read -- use Me.Path instead
    ''' for any _get references.
    ''' For Folders, we set the UpdateFolder property so that the folder interface is re-fetched when needed.
    ''' As with Path, this implies that Me.Folder should always be used rather than m_Folder.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub updateFolderPidlAndPath()
        m_Path = String.Empty             'will update when needed
        Dim newPidl As IntPtr
        newPidl = concatPidls(Me.Parent.PIDL, ILFindLastID(Me.PIDL))
        Marshal.FreeCoTaskMem(m_Pidl)
        m_Pidl = newPidl
        If Me.IsFolder Then
            If m_Files IsNot Nothing Then
                For Each item As CShItem In m_Files
                    item.updateFolderPidlAndPath()
                Next
            End If
            If m_Directories IsNot Nothing Then
                For Each item As CShItem In m_Directories
                    item.UpdateFolder = True
                    item.updateFolderPidlAndPath()
                Next
            End If
        End If
    End Sub

    ''' <summary>For internal use only<br />
    ''' Update is called by the CShItemUpdater Class when that Class receives a WM_Notify message. The purpose of this Class is to
    ''' translate the information passed to it into the appropriate set of actions needed to maintain the internal cache and to,
    ''' directly or indirectly (thru the routines it calls), Raise CShItemUpdate events to notify the using application of changes.
    ''' </summary>
    ''' <param name="newPidl">The absolute PIDL of the affected item. The definition of "affected item" varies with the type of
    '''                       change being reported.</param>
    ''' <param name="changeType">The type of change.</param>
    ''' <remarks>Serves as a bridge between CShItemUpdater and the CShItem that should handle a change.</remarks>
    Friend Sub Update(ByVal newPidl As IntPtr, ByVal changeType As CShItemUpdateType)
        Select Case changeType
            Case CShItemUpdateType.Renamed      'Item has been renamed or moved

                Dim newParent, newPidlRel As IntPtr
                Dim PidlRel, newFolderPtr As IntPtr
                newParent = TrimPidl(newPidl, newPidlRel)
                Dim oldParentItem As CShItem = Me.Parent    'Save in case "renamed" to a new directory
                Dim newParentItem As CShItem = FindCShItem(newParent)
                If newParentItem Is Nothing Then            'renamed to a dir that is not yet in internal tree
                    Me.Parent.RemoveItem(Me)                'no longer in this Folder
                    m_Parent = Nothing                      'and therefore no longer in tree
                Else            'new parent of this item IS in internal tree, fix up and update any files/folders of THIS item
                    If (ShellAPI.SHGetRealIDL(newParentItem.Folder, newPidlRel, PidlRel) = S_OK) Then
                        Marshal.FreeCoTaskMem(m_Pidl)
                        m_Pidl = concatPidls(newParent, PidlRel)  'we use PidlRel because newPidlRel is a "simple" PIDL rather than a regular 1-item SHITEMID
                        If IsFolder Then            'deal with potential "Move" to a new dir
                            If Not newParentItem Is Me.Parent Then
                                Me.Parent.RemoveItem(Me)
                                newParentItem.AddItem(Me)
                            End If
                            ResetInfo()
                            SetPath()
                            If newParentItem.Folder.BindToObject(PidlRel, IntPtr.Zero, ShellAPI.IID_IShellFolder, newFolderPtr) = ShellAPI.S_OK Then
                                Marshal.ReleaseComObject(Folder)
                                m_Folder = CType(Marshal.GetTypedObjectForIUnknown(newFolderPtr, GetType(IShellFolder)), IShellFolder)
                                Marshal.Release(newFolderPtr)
                                If m_Files IsNot Nothing Then
                                    For Each item As CShItem In m_Files
                                        item.updateFolderPidlAndPath()
                                    Next item
                                End If
                                If m_Directories IsNot Nothing Then
                                    For Each item As CShItem In m_Directories
                                        item.updateFolderPidlAndPath()
                                    Next item
                                End If
                            End If
                        Else
                            If Not oldParentItem Is newParentItem Then
                                If oldParentItem.FilesInitialized Then    'deal with potential "Move" to a new dir
                                    oldParentItem.RemoveItem(Me)
                                End If
                                If newParentItem.FilesInitialized Then
                                    newParentItem.AddItem(Me)
                                    ResetInfo()         'new since sent to others
                                    SetPath()           'new since sent to others
                                Else
                                    m_Parent = Nothing
                                    ResetInfo()         'new since sent to others
                                End If
                            Else                    'Added for fix to the fix
                                ResetInfo()         'Added for fix to the fix
                                SetPath()           'Added for fix to the fix
                            End If     'Not oldParentItem Is newParentItem
                            'ResetInfo()         'newly deleted since sent to others
                            'SetPath()           'newly deleted since sent to others
                        End If
                    End If   'SHGetRealIDL = S_OK
                End If       'Check for New ParentDir in internal Tree
                'Note: FreeCoTaskMem will ignore IntPtr.Zero
                Marshal.FreeCoTaskMem(PidlRel)
                Marshal.FreeCoTaskMem(newParent)
                Marshal.FreeCoTaskMem(newPidlRel)
                RaiseEvent CShItemUpdate(oldParentItem, New ShellItemUpdateEventArgs(Me, changeType))
            Case CShItemUpdateType.UpdateDir        'raised when content of a dir changes
                DoUpdateDir(Me)                     'recursively check this Folder and all known sub-Folders for change     '5/21/2012

            Case CShItemUpdateType.Updated      'raised when Attributes (Item or Items under a Folder) change
                'Debug.WriteLine("Updated for " & Me.Path)
UPDATED:        ResetInfo()
                'Previous versions called ResetChildren. Changed to UpdateRefresh - which impacts performance.
                'Decided for now (6/12/2012) to do neither, so commented it out. This message is often closely followed or preceeded
                'by an UPDATEDIR which will, in fact call UpdateRefresh which will also call ResetChildren in many cases.
                'Performance impact is greatly aggravated by the (common on Win7) closely paired UPDATEDIR and UPDATEITEM messages
                'on the same Folder, caused by the same change! Removing this code limits the impact.
                'If Me.IsFolder Then
                '    'Me.ResetChildren()     'Original code
                '    'Me.UpdateRefresh()     '6/3/2012
                'End If
                RaiseEvent CShItemUpdate(Me.Parent, New ShellItemUpdateEventArgs(Me, changeType))
            Case CShItemUpdateType.IconChange
                'Debug.WriteLine("IconChange for " & Me.Path)
                ResetInfo()
                RaiseEvent CShItemUpdate(Me.Parent, New ShellItemUpdateEventArgs(Me, changeType))
            Case CShItemUpdateType.MediaChange          'CD/DVD/External Drive/Etc Added or Removed
                'Debug.WriteLine("MediaChange for " & Me.Path)
                Me.ClearItems(True, True)
                ResetInfo()
                SetPath()
                RaiseEvent CShItemUpdate(Me.Parent, New ShellItemUpdateEventArgs(Me, changeType))
        End Select
    End Sub

#Region "       UpdateRefresh"

    Private Sub DoUpdateDir(ByVal CSI As CShItem)     '5/21/2012
        If CSI Is m_Recycle Then Exit Sub '6/21/2012
        CSI.UpdateRefresh()
        If CSI.m_Directories IsNot Nothing Then
            'The below line has been changed to use CSI.m_Directories rather than just m_Directories which was an error
            For Each FolderItem As CShItem In CSI.m_Directories '02/18/2014 Using Directories here is redundant, causing an extra UpdateRefresh
                DoUpdateDir(FolderItem)
            Next
        End If
    End Sub

    ''' <summary>
    ''' The UpdateRefresh function compares the Current content of the Folder with the
    ''' current state of m_Directories and m_Files, adding/deleting CShItems as appropriate  (thus causing
    ''' appropriate events to be raised for listening clients. 
    ''' Called internally to handle WM_UPDATEDIR messages which map to CShItemUpdateType.UpdateDir. 
    ''' This message indicates that the Contents of this Folder has changed.  Typically, it is fired 
    ''' when multiple items are added/deleted. In practice, several explicit add/delete notification 
    ''' messages are fired followed by WM_UPDATEDIR to indicate that there are more changes. 
    ''' Certain other types of file operations (eg Save) use only WM_UPDATEDIR rather than WM_CREATE.
    ''' </summary>
    ''' <param name="UpdateFiles">True to examine Files of this folder for changes.</param>
    ''' <param name="UpdateFolders">True to examine sub-directories of this folder for changes.</param>
    ''' <returns>True if changes have been made, False otherwise</returns>
    ''' <remarks>If m_Directories or m_Files is Nothing, then no attempt is made to compare with current 
    ''' contents.  That is, if m_files is Nothing then it is not updated, m_Directories is treated the same.
    ''' Note that m_xxxx.Count=0 is not the same thing as m_xxxx is Nothing! m_xxxx = Nothing means
    ''' no one cares about the content.  m_xxxx.Count = 0 means that someone does care, but there were 
    ''' no such items known until (perhaps) now.</remarks>
    Public Function UpdateRefresh(Optional ByVal UpdateFiles As Boolean = True, Optional ByVal UpdateFolders As Boolean = True) As Boolean
        Return False
        UpdateRefresh = False
        If m_IsFolder Then
            SyncLock LockObj
                Dim attrFlag As SHCONTF = SHCONTF.INCLUDEHIDDEN
                If m_Files IsNot Nothing AndAlso UpdateFiles Then attrFlag = attrFlag Or SHCONTF.NONFOLDERS
                If m_Directories IsNot Nothing AndAlso UpdateFolders Then attrFlag = attrFlag Or SHCONTF.FOLDERS
                If attrFlag = SHCONTF.INCLUDEHIDDEN Then Exit Function 'nothing expanded therefore no change

                Dim InvalidItems As New List(Of CShItem)                 'Holds CShItems no longer present
                Dim curPidls As ArrayList = GetContentPtrs(attrFlag)     'Relative PIDLs of current content
                Dim tmpCurrent As New List(Of IntPtr)(curPidls.ToArray(GetType(IntPtr)))  'working list of current content
                If curPidls.Count < 1 Then                               'no items currently in Folder, so mark any previously known as invalid
                    If m_Files IsNot Nothing AndAlso UpdateFiles Then InvalidItems.AddRange(m_Files.ToArray)
                    If m_Directories IsNot Nothing AndAlso UpdateFolders Then InvalidItems.AddRange(m_Directories.ToArray)
                Else            'there are currently some items of interest in Me.Folder
                    Dim tmpItems As New List(Of CShItem)              'working list of old known items
                    If m_Directories IsNot Nothing AndAlso UpdateFolders Then tmpItems.AddRange(m_Directories.ToArray)
                    If m_Files IsNot Nothing AndAlso UpdateFiles Then tmpItems.AddRange(m_Files.ToArray)
                    Dim oldPidls(tmpItems.Count - 1) As IntPtr         'working list of relative pidls of known items
                    For i As Integer = 0 To tmpItems.Count - 1
                        oldPidls(i) = ILFindLastID(tmpItems(i).PIDL)
                    Next

                    Dim InvalidItemsSyncLock As New Object
                    Dim ItemsChecked(tmpCurrent.Count - 1) As Boolean
                    Tasks.Parallel.For(0, oldPidls.Length - 1, Sub(iold As Integer)

                                                                   For icur As Integer = tmpCurrent.Count - 1 To 0 Step -1     '5/21/2012 changed to bottom-up loop
                                                                       If ItemsChecked(icur) Then Continue For
                                                                       ' 5/23/2012 revised the following block of code to also check vs AreBytesEqual
                                                                       If IsEqual(oldPidls(iold), tmpCurrent(icur)) Then   'found the same item
                                                                           ''# EXCLUDED AGAIN 1/04/2016, Mike Ruby !
                                                                           'If Me IsNot m_Recycle AndAlso Not AreBytesEqual(oldPidls(iold), tmpCurrent(icur)) Then  '7/14/2012
                                                                           '    'in this case, some aspect besides name has changed treat as UpdateItem for the old one
                                                                           '    Dim UpdCSI As CShItem = tmpItems(iold)
                                                                           '    'Debug.WriteLine("***Raising Updated based on AreBytesEqual - " & UpdCSI.Name)
                                                                           '    UpdCSI.ResetInfo()
                                                                           '    If UpdCSI.IsFolder Then
                                                                           '        UpdCSI.ResetChildren()
                                                                           '    End If
                                                                           '    RaiseEvent CShItemUpdate(UpdCSI.Parent, New ShellItemUpdateEventArgs(UpdCSI, CShItemUpdateType.Updated)) '6/3/2012
                                                                           '    UpdateRefresh = True        '5/24/2012  
                                                                           'End If
                                                                           'either way, we have found the matching PIDL so continue with the next "old" one (in tree)
                                                                           ItemsChecked(icur) = True
                                                                           'tmpCurrent.RemoveAt(icur) 'Have match, don't look at this one again - and do not add it in the following code
                                                                           Return
                                                                       End If
                                                                       ' 5/23/2012 end of revised code
                                                                   Next
                                                                   'falling thru here means couldn't find iold entry
                                                                   SyncLock InvalidItemsSyncLock
                                                                       InvalidItems.Add(tmpItems(iold))
                                                                   End SyncLock

                                                               End Sub)

                    ' Remove the checked items form the current list, otherwise they are treated as new items!
                    For i As Integer = ItemsChecked.Length - 1 To 0 Step -1
                        If ItemsChecked(i) Then tmpCurrent.RemoveAt(i)
                    Next

                    '                    For iold As Integer = 0 To oldPidls.Length - 1
                    '                        For icur As Integer = tmpCurrent.Count - 1 To 0 Step -1     '5/21/2012 changed to bottom-up loop
                    '                            ' 5/23/2012 revised the following block of code to also check vs AreBytesEqual
                    '                            If IsEqual(oldPidls(iold), tmpCurrent(icur)) Then   'found the same item
                    '                                ''# EXCLUDED AGAIN 1/04/2016, Mike Ruby !
                    '                                'If Me IsNot m_Recycle AndAlso Not AreBytesEqual(oldPidls(iold), tmpCurrent(icur)) Then  '7/14/2012
                    '                                '    'in this case, some aspect besides name has changed treat as UpdateItem for the old one
                    '                                '    Dim UpdCSI As CShItem = tmpItems(iold)
                    '                                '    'Debug.WriteLine("***Raising Updated based on AreBytesEqual - " & UpdCSI.Name)
                    '                                '    UpdCSI.ResetInfo()
                    '                                '    If UpdCSI.IsFolder Then
                    '                                '        UpdCSI.ResetChildren()
                    '                                '    End If
                    '                                '    RaiseEvent CShItemUpdate(UpdCSI.Parent, New ShellItemUpdateEventArgs(UpdCSI, CShItemUpdateType.Updated)) '6/3/2012
                    '                                '    UpdateRefresh = True        '5/24/2012  
                    '                                'End If
                    '                                'either way, we have found the matching PIDL so continue with the next "old" one (in tree)
                    '                                tmpCurrent.RemoveAt(icur) 'Have match, don't look at this one again - and do not add it in the following code
                    '                                GoTo NXTOLD
                    '                            End If
                    '                            ' 5/23/2012 end of revised code
                    '                        Next
                    '                        'falling thru here means couldn't find iold entry
                    '                        InvalidItems.Add(tmpItems(iold))
                    'NXTOLD:             Next

                End If
                'any not found should be removed from my collections (raising event)
                If InvalidItems.Count > 0 Then
                    UpdateRefresh = True
                    Dim csi As CShItem
                    For Each csi In InvalidItems
                        RemoveItem(csi)
                    Next
                End If
                'anything remaining in tmpcurrent is a new entry Add it (raising event)
                If tmpCurrent.Count > 0 Then
                    UpdateRefresh = True
                    For Each iptr As IntPtr In tmpCurrent   'these are relative PIDLs
                        Try                                 'ASUS Fix
                            Dim NewItem As CShItem = New CShItem(iptr, Me)  '11/13/2013
                            AddItem(NewItem)                                '11/13/2013
                        Catch ex As Exception               'ASUS Fix - modified 11/13/2013 was only looking for InvalidCastExcepton
                        End Try                             'ASUS Fix
                    Next
                End If
                'we obtained some new relative PIDLs in curPidls, so free them
                For Each itm As IntPtr In curPidls
                    Marshal.FreeCoTaskMem(itm)
                Next
                '6/18/2012 - If something changed in this Folder, then Raise an Updated Event AFTER all Adds, Deletes, etc have been posted
                '6/18/2012 - One was previously Raised when working down the Tree from Me's Parent, but Adds, Deletes, etc details had not been posted
                '6/18/2012 - at that time. The App did not know HOW this Folder had changed (except for attributes)
                If UpdateRefresh AndAlso Me.IsFolder Then
                    If Me.Parent Is Nothing Then
                        RaiseEvent CShItemUpdate(GetDeskTop, New ShellItemUpdateEventArgs(Me, CShItemUpdateType.Updated))
                    Else
                        RaiseEvent CShItemUpdate(Me.Parent, New ShellItemUpdateEventArgs(Me, CShItemUpdateType.Updated))
                    End If
                End If
            End SyncLock
        End If
    End Function

#End Region

#End Region             'Update region

End Class

''' <summary>
''' ShellItemUpdateEventArgs is used to pass information about a change in the Shell Namespace (that we actually care about) 
'''   to an Event Handler.<br />
''' See <see cref="ExpTreeLib.ShellItemUpdateEventArgs.UpdateType">UpdateType</see> for details.
''' </summary>
''' <remarks>
''' </remarks>
Public Class ShellItemUpdateEventArgs
    Inherits EventArgs
    Private m_Item As CShItem
    Private m_Type As CShItem.CShItemUpdateType

    Public Sub New(ByVal Item As CShItem, ByVal type As CShItem.CShItemUpdateType)
        m_Item = Item
        m_Type = type
    End Sub

    ''' <summary>
    ''' The CShItem that changed.
    ''' </summary>
    ''' <returns>The CShItem changed.</returns>
    ''' <remarks>The precise role of this CShItem in a change depends on the type of change.<br />
    ''' See <see cref="ExpTreeLib.ShellItemUpdateEventArgs.UpdateType">UpdateType</see> for details.
    ''' </remarks>
    Public ReadOnly Property Item() As CShItem
        Get
            Return m_Item
        End Get
    End Property

    ''' <summary>
    ''' The type of change given as one of the CShItemUpdateType Enum values.
    ''' </summary>
    ''' <returns>The type of change given as one of the CShItemUpdateType Enum values.</returns>
    ''' <remarks>The UpdateType has the following meaning:
    ''' <table style="text-align: left" border="3">
    ''' <caption>
    ''' UpdateTypes</caption>  
    '''<tr>  
    ''' <td style="width: 100px">  
    '''            <strong>UpdateType</strong></td>    
    '''                <td style="width: 181px">  
    '''                    <strong>sender</strong></td>  
    '''                <td style="width: 202px">  
    '''                    <strong>Item</strong></td>  
    '''                <td style="width: 295px">  
    '''                    <strong>  
    '''                    Occurs when:</strong></td>  
    '''            </tr>  
    '''            <tr>  
    '''                <td style="width: 100px">  
    '''                    Created
    '''                </td>
    '''                <td style="width: 181px">
    '''                    Folder of Item</td>
    '''                <td style="width: 202px">
    '''                    Newly Created Item</td>
    '''                <td style="width: 295px">
    '''                    Item has been created</td>
    '''            </tr>
    '''            <tr>
    '''                <td style="width: 100px">
    '''                    Deleted
    '''                </td>
    '''                <td style="width: 181px">
    '''                    Folder of Item</td>
    '''                <td style="width: 202px">
    '''                    Newly Deleted Item</td>
    '''                <td style="width: 295px">
    '''                    Item has been Deleted</td>
    '''            </tr>
    '''            <tr>
    '''                <td style="width: 100px">
    '''                    Renamed
    '''                </td>
    '''                <td style="width: 181px">
    '''                    Original Folder of Item</td>
    '''                <td style="width: 202px">
    '''                    Item that has been Renamed</td>
    '''                <td style="width: 295px">
    '''                    Item has been Renamed or Moved<span style="font-size: 8pt; vertical-align: super;
    '''                        font-family: Courier New">1</span></td>
    '''            </tr>
    '''            <tr>
    '''                <td style="width: 100px">
    '''                    Updated
    '''                </td>
    '''                <td style="width: 181px">
    '''                    Folder of Item</td>
    '''                <td style="width: 202px">
    '''                    Item that has changed</td>
    '''                <td style="width: 295px">
    '''                    Attributes of Item have changed</td>
    '''            </tr>
    '''            <tr>
    '''                <td style="width: 100px">
    '''                    UpdateDir
    '''                </td>
    '''                <td style="width: 181px">
    '''                    Folder that has Changed</td>
    '''                <td style="width: 202px">
    '''                    Folder that has Changed</td>
    '''                <td style="width: 295px">
    '''                    A Folder has had Items Added/Deleted<span style="font-size: 8pt; vertical-align: super;
    '''                        font-family: Courier New">2</span></td>
    '''            </tr>
    '''            <tr>
    '''                <td style="width: 100px">
    '''                    MediaChange</td>
    '''                <td style="width: 181px">
    '''                    Folder of Item</td>
    '''                <td style="width: 202px">
    '''                    CShItem of Media</td>
    '''                <td style="width: 295px">
    '''                    When Media has been inserted or removed</td>
    '''            </tr>
    '''            <tr>
    '''                <td style="width: 100px">
    '''                    IconChange</td>
    '''                <td style="width: 181px">  
    '''                    Folder of Item</td>  
    '''                <td style="width: 202px">  
    '''                    Item that has changed</td>  
    '''                <td style="width: 295px">  
    '''                    When Icon has changed</td>  
    '''            </tr>  
    '''        </table> 
    '''        <br />
    '''     <span style="font-size: 8pt; vertical-align: super; font-family: Courier New">1</span>
    '''      In the Renamed case, sender is the Folder of the Item before it
    '''      was Renamed (or Moved). The Item may have moved to a new Folder, in which case,
    '''      the new Folder may be determined by e.Item.Parent.
    '''    <p>
    '''    <span style="font-size: 8pt; vertical-align: super; font-family: Courier New">2</span>
    '''      The UpdateDir UpdateType normally may be ignored since any Add or Deletes of Items
    '''      will have been already reported with previous Created and/or Deleted Events.
    ''' </p>
    ''' </remarks>
    Public ReadOnly Property UpdateType() As CShItem.CShItemUpdateType
        Get
            Return m_Type
        End Get
    End Property
End Class
