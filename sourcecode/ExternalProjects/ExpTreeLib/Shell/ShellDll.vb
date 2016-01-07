Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32.SafeHandles

Namespace ShellDll
    ''' <summary>
    ''' ShellAPI contains many declarations of Shell API functions, Constants, Structures, Enums used by ExpTreeLib.
    ''' Certain other declarations of Shell API components are declared outside of this Class, typically in those Classes that
    ''' are the only place that such declarations are needed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ShellAPI

#Region "   Constants"
        Public Const MAX_PATH As Integer = 260
        Public Const FILE_ATTRIBUTE_READONLY As Integer = &H1
        Public Const FILE_ATTRIBUTE_HIDDEN As Integer = &H2
        Public Const FILE_ATTRIBUTE_SYSTEM As Integer = &H4
        Public Const FILE_ATTRIBUTE_DIRECTORY As Integer = &H10
        Public Const FILE_ATTRIBUTE_ARCHIVE As Integer = &H20
        Public Const FILE_ATTRIBUTE_NORMAL As Integer = &H80
        Public Const FILE_ATTRIBUTE_TEMPORARY As Integer = &H100
        Public Const FILE_ATTRIBUTE_COMPRESSED As Integer = &H800

        Public Const NOERROR As Integer = 0
        Public Const S_OK As Integer = 0
        Public Const S_FALSE As Integer = 1

        Public Const DRAGDROP_S_DROP As Integer = &H40100
        Public Const DRAGDROP_S_CANCEL As Integer = &H40101
        Public Const DRAGDROP_S_USEDEFAULTCURSORS As Integer = &H40102

        Public Shared cbFileInfo As Integer = Marshal.SizeOf(GetType(SHFILEINFO))
        Public Shared cbMenuItemInfo As Integer = Marshal.SizeOf(GetType(MENUITEMINFO))
        'Public Const cbTpmParams As Integer = Marshal.SizeOf(GetType(TPMPARAMS))
        Public Shared cbInvokeCommand As Integer = Marshal.SizeOf(GetType(CMInvokeCommandInfoEx))

        ' ListView Message Constants
        Friend Const LVM_FIRST As Integer = &H1000
        Friend Const LVM_SETITEMSTATE As Integer = (LVM_FIRST + 43)
        Friend Const LVM_SETBKIMAGE As Integer = (LVM_FIRST + 68)
        Friend Const LVM_SETTEXTBKCOLOR As Integer = (LVM_FIRST + 38)
        Friend Const LVM_ENABLEGROUPVIEW As Integer = (LVM_FIRST + 157)
        Friend Const LVM_INSERTGROUP As Integer = (LVM_FIRST + 145)
        Friend Const LVM_REMOVEALLGROUPS As Integer = (LVM_FIRST + 160)
        Friend Const LVM_SETITEM As Integer = (LVM_FIRST + 6)
        Friend Const LVM_SETSELECTEDCOLUMN As Integer = (LVM_FIRST + 140)
        Friend Const LVM_GETHEADER As Integer = 4127
        Friend Const LVM_SETCOLUMN As Integer = 4122
        Friend Const LVM_SETEXTENDEDLISTVIEWSTYLE As Integer = LVM_FIRST + 54

        ''For ListItem State
        Friend Const LVIF_STATE As Integer = &H8
        Friend Const LVIS_SELECTED As Integer = &H2
        Friend Const LVIS_FOCUSED As Integer = &H1
        Friend Const LVIS_CUT As Integer = &H4

        ' For BackgroundImage
        Friend Const LVBKIF_SOURCE_NONE As Integer = &H0
        Friend Const LVBKIF_SOURCE_URL As Integer = &H2
        Friend Const LVBKIF_STYLE_TILE As Integer = &H10
        Friend Const LVBKIF_STYLE_NORMAL As Integer = &H0

        ' For ColumnHeader Images
        Friend Const HDM_SETIMAGELIST As Integer = &H1208
        Friend Const LVCF_FMT As Integer = &H1
        Friend Const LVCF_IMAGE As Integer = &H10
        Friend Const LVCFMT_IMAGE As Integer = &H800
        Friend Const LVCF_BITMAP_ON_RIGHT As Integer = &H1000
        Friend Const LVCF_STRING As Integer = &H4000

        ' For ToolTips
        Friend Const LVS_EX_LABELTIP As Integer = &H4000

        ' For ImageList_Draw
        Friend Const ILD_NORMAL As Integer = &H0
        Friend Const ILD_TRANSPARENT As Integer = &H1
        Friend Const ILD_BLEND25 As Integer = &H2
        Friend Const ILD_SELECTED As Integer = &H4
        Friend Const ILD_MASK As Integer = &H10
        Friend Const ILD_IMAGE As Integer = &H20

        ''Other...
        Friend Const CLR_NONE As Integer = -&H1

#End Region

#Region "   Shell GUIDs"
        Public Shared ReadOnly IID_IMalloc As New Guid("{00000002-0000-0000-C000-000000000046}")
        Public Shared ReadOnly IID_IShellFolder As New Guid("{000214E6-0000-0000-C000-000000000046}")
        Public Shared ReadOnly IID_IFolderFilterSite As New Guid("{C0A651F5-B48B-11d2-B5ED-006097C686F6}")
        Public Shared ReadOnly IID_IFolderFilter As New Guid("{9CC22886-DC8E-11d2-B1D0-00C04F8EEB3E}")
        Public Shared ReadOnly DesktopGUID As New Guid("{00021400-0000-0000-C000-000000000046}")

        Public Shared ReadOnly IID_IDropTarget As New Guid("{00000122-0000-0000-C000-000000000046}")
        Public Shared ReadOnly IID_IDataObject As New Guid("{0000010e-0000-0000-C000-000000000046}")

        Public Shared ReadOnly IID_IContextMenu As New Guid("{000214e4-0000-0000-c000-000000000046}")
        Public Shared ReadOnly IID_IContextMenu2 As New Guid("{000214f4-0000-0000-c000-000000000046}")
        Public Shared ReadOnly IID_IContextMenu3 As New Guid("{bcfce0a0-ec17-11d0-8d10-00a0c90f2719}")

        Public Shared ReadOnly IID_IExtractImage As New Guid("{BB2E617C-0920-11d1-9A0B-00C04FC2D6C1}")

        Public Shared ReadOnly IID_IQueryInfo As New Guid("{00021500-0000-0000-C000-000000000046}")
        Public Shared ReadOnly IID_IPersistFile As New Guid("{0000010b-0000-0000-C000-000000000046}")

        Public Shared ReadOnly CLSID_DragDropHelper As New Guid("{4657278A-411B-11d2-839A-00C04FD918D0}")
        Public Shared ReadOnly CLSID_NewMenu As New Guid("{D969A300-E7FF-11d0-A93B-00A0C90F2719}")
        Public Shared ReadOnly IID_IDragSourceHelper As New Guid("{DE5BF786-477A-11d2-839D-00C04FD918D0}")
        Public Shared ReadOnly IID_IDropTargetHelper As New Guid("{4657278B-411B-11d2-839A-00C04FD918D0}")

        Public Shared ReadOnly IID_IShellExtInit As New Guid("{000214e8-0000-0000-c000-000000000046}")
        Public Shared ReadOnly IID_IStream As New Guid("{0000000c-0000-0000-c000-000000000046}")
        Public Shared ReadOnly IID_IStorage As New Guid("{0000000B-0000-0000-C000-000000000046}")

        Public Shared ReadOnly CLSID_ShellLink As New Guid("{00021401-0000-0000-C000-000000000046}")
        Public Shared ReadOnly CLSID_InternetShortcut As New Guid("{FBF23B40-E3F0-101B-8488-00AA003E56F8}")

#End Region

#Region "   Shell Structures"

#Region "       SHFILEINFO"
        '///<summary>
        ' SHFILEINFO structure for VB.Net
        '///</summary>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
        Public Structure SHFILEINFO
            Public hIcon As IntPtr
            Public iIcon As Integer
            Public dwAttributes As SFGAO
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)> _
            Public szDisplayName As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)> _
            Public szTypeName As String
        End Structure

#End Region

#Region "       STRRET Structures"
        'both of these formats work in main thread, neither in worker thread
        '<StructLayout(LayoutKind.Sequential)> _
        'Public Structure STRRET
        '    Public uType As Integer
        '    Public pOle As IntPtr
        'End Structure
        <StructLayout(LayoutKind.Explicit)> _
        Public Structure STRRET
            <FieldOffset(0)> _
            Public uType As Integer       ' One of the STRRET_* values
            <FieldOffset(4)> _
              Public pOleStr As Integer ' must be freed by caller of GetDisplayNameOf
            <FieldOffset(4)> _
              Public uOffset As Integer ' Offset into SHITEMID
            <FieldOffset(4)> _
             Public pStr As Integer    ' NOT USED
        End Structure
#End Region

#Region "       W32Find_Data"
        ''' <summary>
        ''' W32Find_Data is a Class representation of the Win32_Find_Data Structure. It should be an exact replacement for
        ''' that structure, but, for some reason, which I do not care to explore, is not.
        ''' There are some references to Win32_Find_Data in the ShellDll Namespace which will simply cause the app to quit
        ''' if given a W32Find_Data. I suspect it has to do with the problem API calls not having the necessary attributes on 
        ''' the parameter.
        ''' The references related to FindFirstFile and FindNextFile work just fine when called with this class rather than
        ''' Win32_Find_Data. I do not care to pursue this, so I define both versions here.
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayoutAttribute(LayoutKind.Sequential, _
         CharSet:=CharSet.Auto), BestFitMapping(False)> _
         Public Class W32Find_Data
            Public dwFileAttributes As Integer
            Public ftCreationTimeLow As UInt32
            Public ftCreationTimeHigh As UInt32
            Public ftLastAccessTimeLow As UInt32
            Public ftLastAccessTimeHigh As UInt32
            Public ftLastWriteTimeLow As UInt32
            Public ftLastWriteTimeHigh As UInt32
            Public nFileSizeHigh As Integer
            Public nFileSizeLow As Integer
            Public dwReserved0 As Integer
            Public dwReserved1 As Integer
            <MarshalAs(UnmanagedType.ByValTStr, _
                       SizeConst:=MAX_PATH)> _
            Public cFileName As String
            <MarshalAs(UnmanagedType.ByValTStr, _
                       SizeConst:=14)> _
            Public cAlternateFileName As String

            Private m_directoryname As String

            Sub New(ByVal DirectoryName As String)
                m_directoryname = DirectoryName
            End Sub

#Region "   Public Properties"
            Public Property Attributes() As FileAttribute
                Get
                    Return Me.dwFileAttributes
                End Get
                Set(ByVal value As FileAttribute)
                    Me.dwFileAttributes = value
                End Set
            End Property

            Public ReadOnly Property IsCompressed() As Boolean
                Get
                    Return (Me.dwFileAttributes And FileAttributes.Compressed) = FileAttributes.Compressed
                End Get
            End Property

            Public ReadOnly Property IsEncrypted() As Boolean
                Get
                    Return (Me.dwFileAttributes And FileAttributes.Encrypted) = FileAttributes.Encrypted
                End Get
            End Property

            Public ReadOnly Property Length() As Long
                Get
                    Return (CLng(nFileSizeHigh) << &H20) Or (nFileSizeLow And CLng(&HFFFFFFFF))
                End Get
            End Property

            Public ReadOnly Property CreationTimeUTC() As DateTime
                Get
                    Dim filetime As Long = ((CLng(ftCreationTimeHigh) << &H20) Or ftCreationTimeLow)
                    Return DateTime.FromFileTimeUtc(filetime)
                End Get
            End Property

            Public ReadOnly Property CreationTime() As DateTime
                Get
                    Return Me.CreationTimeUTC.ToLocalTime
                End Get
            End Property

            Public ReadOnly Property LastWriteTimeUTC() As DateTime
                Get
                    Dim filetime As Long = ((CLng(ftLastWriteTimeHigh) << &H20) Or ftLastWriteTimeLow)
                    Return DateTime.FromFileTimeUtc(filetime)
                End Get
            End Property

            Public ReadOnly Property LastWriteTime() As DateTime
                Get
                    Return Me.LastWriteTimeUTC.ToLocalTime
                End Get
            End Property

            Public ReadOnly Property LastAccessTimeUTC() As DateTime
                Get
                    Dim filetime As Long = ((CLng(ftLastAccessTimeHigh) << &H20) Or ftLastAccessTimeLow)
                    Return DateTime.FromFileTimeUtc(filetime)
                End Get
            End Property

            Public ReadOnly Property LastAccessTime() As DateTime
                Get
                    Return Me.LastAccessTimeUTC.ToLocalTime
                End Get
            End Property

            Public ReadOnly Property Name() As String
                Get
                    Return Me.cFileName
                End Get
            End Property

            Public Property DirectoryName() As String
                Get
                    If m_directoryname Is Nothing OrElse m_directoryname = String.Empty Then
                        Return ""
                    Else
                        Return m_directoryname
                    End If
                End Get
                Set(ByVal value As String)
                    m_directoryname = value
                End Set
            End Property

            Public ReadOnly Property FullName() As String
                Get
                    If Me.DirectoryName.Equals("") Then
                        Return Me.Name
                    Else
                        Return Me.DirectoryName & Path.DirectorySeparatorChar & Me.Name
                    End If
                End Get
            End Property
#End Region

        End Class

#End Region

#Region "   FindFirstFile and related declarations"
        <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function FindFirstFile(ByVal fileName As String, _
                                             <[In](), Out()> ByVal data As W32Find_Data) As SafeFindHandle
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function FindNextFile(ByVal hndFindFile As SafeFindHandle, <[In](), Out(), MarshalAs(UnmanagedType.LPStruct)> ByVal lpFindFileData As W32Find_Data) As Boolean
        End Function

        <DllImport("kernel32.dll")> _
        Public Shared Function FindClose(ByVal handle As IntPtr) As Boolean
        End Function

        ''' <summary>
        ''' Provides a <see cref="SafeHandleZeroOrMinusOneIsInvalid">SafeHandleZeroOrMinusOneIsInvalid</see> to FindFirstFile and
        ''' FindNextFile, preset that the Handle will be reliably released.
        ''' </summary>
        Public NotInheritable Class SafeFindHandle
            Inherits SafeHandleZeroOrMinusOneIsInvalid
            Public Sub New()
                MyBase.New(True)
            End Sub

            ''' <summary>
            ''' Releases this Handle
            ''' </summary>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Protected Overrides Function ReleaseHandle() As Boolean
                Return FindClose(MyBase.handle)
            End Function
        End Class
#End Region

#Region "       W32_FIND_DATA"
        <StructLayoutAttribute(LayoutKind.Sequential, _
         CharSet:=CharSet.Auto)> _
         Public Structure WIN32_FIND_DATA
            Public dwFileAttributes As Integer
            Public ftCreationTime As ComTypes.FILETIME
            Public ftLastAccessTime As ComTypes.FILETIME
            Public ftLastWriteTime As ComTypes.FILETIME
            Public nFileSizeHigh As Integer
            Public nFileSizeLow As Integer
            Public dwReserved0 As Integer
            Public dwReserved1 As Integer
            <MarshalAs(UnmanagedType.ByValTStr, _
                       SizeConst:=MAX_PATH)> _
            Public cFileName As String
            <MarshalAs(UnmanagedType.ByValTStr, _
                       SizeConst:=14)> _
            Public cAlternateFileName As String
        End Structure
#End Region

        ' Contains the information needed to create a drag image
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure SHDRAGIMAGE
            Public sizeDragImage As Size
            Public ptOffset As POINT
            Public hbmpDragImage As IntPtr
            Public crColorKey As Color
        End Structure

        ' Represents the number of 100-nanosecond intervals since January 1, 1601
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure FILETIME
            Public dwLowDateTime As Integer
            Public dwHighDateTime As Integer
        End Structure


        ' Contains statistical data about an open storage, stream, or byte-array object
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure STATSTG
            <MarshalAs(UnmanagedType.LPWStr)> _
            Public pwcsName As String
            Public type As STGTY
            Public cbSize As Long
            Public mtime As ShellAPI.FILETIME
            Public ctime As ShellAPI.FILETIME
            Public atime As ShellAPI.FILETIME
            Public grfMode As STGM
            Public grfLocksSupported As LOCKTYPE
            Public clsid As Guid
            Public grfStateBits As Integer
            Public reserved As Integer
        End Structure

#Region "       ImageList Structures"
        <StructLayout(LayoutKind.Sequential)> _
        Private Structure RECT
            Dim left As Integer
            Dim top As Integer
            Dim right As Integer
            Dim bottom As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
         Public Structure POINT
            Sub New(ByVal xValue As Integer, ByVal yValue As Integer)
                x = xValue
                y = yValue
            End Sub
            Dim x As Integer
            Dim y As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Friend Structure LVBKIMAGE
            Friend ulFlags As Int32
            Friend hbm As IntPtr
            Friend pszImage As String
            Friend cchImageMax As Int32
            Friend xOffsetPercent As Int32
            Friend yOffsetPercent As Int32
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Friend Structure LVITEM
            Friend mask As Int32
            Friend iItem As Int32
            Friend iSubItem As Int32
            Friend state As Int32
            Friend stateMask As Int32
            <MarshalAs(UnmanagedType.LPWStr)> _
            Friend pszText As String
            Friend cchTextMax As String
            Friend iImage As Int32
            Friend lParam As Int32
            Friend iIndent As Int32
            Friend iGroupId As Int32
            Friend cColumns As Int32
            Friend puColumns As Int32
        End Structure

        <StructLayout(LayoutKind.Sequential, pack:=8, CharSet:=CharSet.Auto)> _
        Friend Structure LVCOLUMN
            Dim mask As Integer
            Dim fmt As Integer
            Dim cx As Integer
            Dim pszText As IntPtr
            Dim cchTextMax As Integer
            Dim iSubItem As Integer
            Dim iImage As Integer
            Dim iOrder As Integer
        End Structure
#End Region

#End Region

#Region "       shell32 Dll Declarations"

#Region "       DragQueryFiles"
        Declare Auto Function DragQueryFile Lib "shell32.dll" ( _
                            ByVal hDrop As IntPtr, _
                            ByVal iFile As Integer, _
                       <MarshalAs(UnmanagedType.LPTStr)> _
                            ByVal lpszFile As StringBuilder, _
                            ByVal cch As Integer) As Integer
#End Region

#Region "       IL functions"
        Declare Auto Function ILIsEqual Lib "shell32" Alias "#21" ( _
                            ByVal pidl1 As IntPtr, _
                            ByVal pidl2 As IntPtr) As Boolean

        Declare Auto Function ILIsParent Lib "shell32" Alias "#23" ( _
                                            ByVal pidlParent As IntPtr, _
                                            ByVal pidlBelow As IntPtr, _
                                            ByVal fImmediate As Boolean) _
                                                        As Boolean
        Declare Auto Function ILCombine Lib "shell32" Alias "#25" ( _
                                            ByVal pidl1 As IntPtr, _
                                            ByVal pidl2 As IntPtr) As IntPtr
        Declare Auto Function ILFindLastID Lib "shell32" Alias "#16" ( _
                                            ByVal pidl As IntPtr) As IntPtr
        Declare Auto Function ILRemoveLastID Lib "shell32" Alias "#17" ( _
                                         <[In]()> <Out()> ByRef pidl As IntPtr) As Boolean
        Declare Auto Function ILGetNext Lib "shell32" (ByVal pidl As IntPtr) As IntPtr

#End Region

#Region "       Notification Declarations"

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
Public Structure SHChangeNotifyEntry
            Public pIdl As IntPtr
            Public Recursively As Boolean
        End Structure

        'Contains two PIDLs concerning the notify message
        <StructLayout(LayoutKind.Sequential)> _
         Public Structure SHNOTIFYSTRUCT
            Public dwItem1 As IntPtr
            Public dwItem2 As IntPtr
        End Structure

        'Registers a window that receives notifications from the file system or shell
        <DllImport("shell32", EntryPoint:="#2", CharSet:=CharSet.Auto)> _
        Public Shared Function SHChangeNotifyRegister( _
                ByVal hwnd As IntPtr, _
                ByVal fSources As SHCNRF, _
                ByVal fEvents As SHCNE, _
                ByVal wMsg As WM, _
                ByVal cEntries As Integer, _
                <MarshalAs(UnmanagedType.LPArray)> ByVal _
                pfsne As SHChangeNotifyEntry()) As Integer
        End Function

        'Unregisters the client's window process from receiving SHChangeNotify
        <DllImport("shell32", EntryPoint:="#4", CharSet:=CharSet.Auto)> _
        Public Shared Function SHChangeNotifyDeregister( _
                ByVal hNotify As Integer) As Boolean
        End Function


        <DllImport("shell32", CharSet:=CharSet.Auto)> _
        Public Shared Function SHChangeNotification_Lock(ByVal hChange As IntPtr, _
                                                         ByVal dwProcId As UInt32, _
                                                         ByRef pppidl As IntPtr, _
                                                         ByRef plEvent As SHCNE) As IntPtr
        End Function

        <DllImport("shell32", CharSet:=CharSet.Auto)> _
        Public Shared Function SHChangeNotification_Unlock(ByVal hLock As IntPtr) As Int32
        End Function

        Declare Auto Sub SHChangeNotify Lib "shell32" (ByVal wEventId As Integer, _
                            ByVal uFlags As Integer, _
                            ByVal dwItem1 As IntPtr, _
                            ByVal dwItem2 As IntPtr)

#End Region

#Region "       SHGetDesktopFolder"
        '<summary>
        ' Retrieves the IShellFolder interface for the desktop folder, 
        '    which is the root of the Shell's namespace. 
        '<param>
        '  ppshf -- Recieves the IShellFolder interface for the desktop folder
        '</param>
        Declare Auto Function SHGetDesktopFolder Lib "shell32" ( _
                    ByRef ppshf As IShellFolder) As Integer
#End Region

#Region "       SHGetFileInfo"
        'SHGetFileInfo
        'Retrieves information about an object in the file system,
        ' such as a file, a folder, a directory, or a drive root.

        ' <summary>
        '  SHGetFileInfo  - for a given Path as a string
        ' </summary>
        Declare Auto Function SHGetFileInfo Lib "shell32" ( _
             ByVal pszPath As String, _
             ByVal dwFileAttributes As Integer, _
             ByRef sfi As SHFILEINFO, _
             ByVal cbsfi As Integer, _
             ByVal uFlags As Integer) As IntPtr
        ' <summary>
        '  SHGetFileInfo  - for a given ItemIDList as IntPtr
        ' </summary>
        Declare Auto Function SHGetFileInfo Lib "shell32" ( _
                 ByVal ppidl As IntPtr, _
                 ByVal dwFileAttributes As Integer, _
                 ByRef sfi As SHFILEINFO, _
                 ByVal cbsfi As Integer, _
                 ByVal uFlags As SHGFI) As IntPtr
#End Region

#Region "       ShGetImageListHandle"
        'UPDATE: Add SHGetImageListHandle
        '''<summary>
        '''SHGetImageList is not exported correctly in XP.  See KB316931
        '''http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q316931
        '''Apparently (and hopefully) ordinal 727 isn't going to change.
        '''</summary>
        <DllImport("shell32.dll", EntryPoint:="#727")> _
        Friend Shared Function SHGetImageListHandle( _
        ByVal iImageList As Integer, ByRef riid As Guid, ByRef handle As IntPtr) As Integer

        End Function
#End Region

#Region "       SHGetMalloc"
        '<summary>
        '  Get an Imalloc Interface
        ' Not required for .Net apps, use Marshal class
        '</summary>
        Declare Auto Function SHGetMalloc Lib "shell32" ( _
                ByRef pMalloc As IMalloc) As Integer
#End Region

#Region "       SHGetNewLinkInfo"
        '''<summary>Despite its name, the API returns a filename
        ''' for a link to be copied/created in a Target directory,
        ''' with a specific LinkTarget. It will create a unique name
        ''' unless instructed otherwise (SHGLNI_NOUNIQUE).  And add
        ''' the ".lnk" extension, unless instructed otherwise(SHGLNI.NOLNK)
        '''</summary>
        Declare Ansi Function SHGetNewLinkInfo Lib "shell32" Alias "SHGetNewLinkInfoA" ( _
                ByVal pszLinkTo As String, _
                ByVal pszDir As String, _
                <Out(), MarshalAs(UnmanagedType.LPStr)> ByVal pszName As StringBuilder, _
                ByRef pfMustCopy As Boolean, _
                ByVal uFlags As SHGNLI) As Integer
        '''<summary> Same function using a PIDL as the pszLinkTo.
        '''  SHGNLI.PIDL must be set.
        '''</summary>
        Declare Ansi Function SHGetNewLinkInfo Lib "shell32" Alias "SHGetNewLinkInfoA" ( _
            ByVal pszLinkTo As IntPtr, _
            ByVal pszDir As String, _
            <Out(), MarshalAs(UnmanagedType.LPStr)> ByVal pszName As StringBuilder, _
            ByRef pfMustCopy As Boolean, _
            ByVal uFlags As SHGNLI) As Integer

#End Region

#Region "       SHGetPathFromIDList"
        Declare Auto Function SHGetPathFromIDList Lib "shell32" ( _
                            ByVal pidl As IntPtr, _
                            ByVal Path As StringBuilder) As Boolean

#End Region

#Region "       SHGetRealIDL"
        ' SHGetRealIDL converts a simple PIDL to a full PIDL
        ' Note that Win2K and before do not export SHGetRealIDL, though support it at Ordinal 98
        Declare Auto Function SHGetRealIDL Lib "shell32" Alias "#98" ( _
                                    ByVal psf As IShellFolder, _
                                    ByVal pidlSimple As IntPtr, _
                                    <Out()> ByRef ppidlReal As IntPtr) As Integer
#End Region

#Region "       SHGetSpecialFolderLocation"

        Declare Function SHGetSpecialFolderLocation Lib "shell32" ( _
            ByVal hWndOwner As Integer, _
            ByVal csidl As Integer, _
            ByRef ppidl As IntPtr) As Integer
#End Region

#End Region

#Region "       shlwapi Dll Declarations"

#Region "           STRRETtoSomeString"
        ' Accepts a STRRET structure returned by IShellFolder::GetDisplayNameOf that contains or points to a 
        ' string, and then returns that string as a BSTR.
        ' <param>
        '       Pointer to a STRRET structure.
        '       Pointer to an ITEMIDLIST uniquely identifying a file object or subfolder relative
        '       Pointer to a variable of type BSTR that contains the converted string.
        '</param>
        Declare Auto Function StrRetToBSTR Lib "shlwapi.dll" ( _
                    ByRef pstr As STRRET, _
                    ByVal pidl As IntPtr, _
                    <MarshalAs(UnmanagedType.BStr)> _
                    ByRef pbstr As String) As Integer

        '<summary>
        ' Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf, 
        ' converts it to a string, and 
        ' places the result in a buffer. 
        ' <param>
        '       Pointer to a STRRET structure.
        '       Pointer to an ITEMIDLIST uniquely identifying a file object or subfolder relative
        '       Pointer to a Buffer to hold the display name. It will be returned as a null-terminated
        '                   string. If cchBuf is too small, 
        '                   the name will be truncated to fit. 
        '       Size of pszBuf, in characters. 
        '</param>
        '</summary>
        Declare Auto Function StrRetToBuf Lib "shlwapi.dll" ( _
                            ByVal pstr As IntPtr, _
                            ByVal pidl As IntPtr, _
                            ByVal pszBuf As StringBuilder, _
                            <MarshalAs(UnmanagedType.U4)> _
                            ByVal cchBuf As Integer) As Integer
#End Region

#End Region

#Region "       user32 Dll Declarations"

#Region "           SendMessage"
        '<summary>
        '   Sends a message to some Window
        '</summary>
        Declare Auto Function SendMessage Lib "user32" ( _
                ByVal hWnd As IntPtr, _
                ByVal wMsg As WM, _
                ByVal wParam As Integer, _
                ByVal lParam As IntPtr) As Integer

        Friend Declare Auto Function SendMessage Lib "User32.dll" (ByVal hWnd As IntPtr, ByVal Msg As Integer, _
           ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr

        Friend Declare Auto Function SendMessage Lib "User32.dll" (ByVal hWnd As IntPtr, ByVal Msg As Integer, _
           ByVal wParam As Integer, ByVal lParam As IntPtr) As IntPtr

        Friend Declare Auto Function SendMessage Lib "User32.dll" (ByVal hWnd As IntPtr, ByVal Msg As Integer, _
              ByVal wParam As Integer, ByRef lParam As LVCOLUMN) As IntPtr

        Friend Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal wMsg As Int32, _
            ByVal wParam As Int32, ByRef lParam As LVBKIMAGE) As Boolean

        Friend Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal wMsg As Int32, _
           ByVal wParam As Int32, ByRef lParam As LVITEM) As Boolean

#End Region

#Region "           DestroyIcon"
        Declare Function DestroyIcon Lib "user32.dll" ( _
                    ByVal hIcon As IntPtr) As Boolean
#End Region

#Region "           Menu related"
        Declare Auto Function AppendMenu Lib "user32" (ByVal hMenu As IntPtr, ByVal uFlags As Integer, _
            ByVal uIDNewItem As Integer, <MarshalAs(UnmanagedType.LPTStr)> ByVal lpNewItem As String) As Boolean

        Declare Function CreatePopupMenu Lib "user32.dll" () As IntPtr

        Declare Function GetMenuItemCount Lib "user32.dll" (ByVal hMenu As Integer) As Integer

        Declare Function GetSubMenu Lib "user32" (ByVal hMenu As IntPtr, ByVal nPos As Integer) As IntPtr

        Declare Auto Function InsertMenuItem Lib "user32" (ByVal hMenu As IntPtr, ByVal uItem As Integer, _
                ByVal fByPosition As Boolean, ByRef lpmii As MENUITEMINFO) As Boolean

        Declare Function TrackPopupMenuEx Lib "user32.dll" (ByVal hMenu As IntPtr, _
           ByVal uFlags As Integer, ByVal x As Integer, ByVal y As Integer, _
           ByVal hWnd As IntPtr, ByVal lptpm As IntPtr) As Integer
#End Region

#Region "           RegisterClipboardFormat"

        Declare Auto Function RegisterClipboardFormat Lib "User32" (ByVal lpszFormat As String) As Integer

#End Region

#End Region

#Region "       comctl32 Dll Declarations"

#Region "       ImageList_GetIconSize"
        '<summary>
        '   Gets an IconSize from a ImagelistHandle
        '</summary>
        Declare Function ImageList_GetIconSize Lib "comctl32" ( _
                   ByVal himl As IntPtr, _
                   ByRef cx As Integer, _
                   ByRef cy As Integer) As Integer
#End Region

#Region "       ImageList_ReplaceIcon"
        Declare Auto Function ImageList_ReplaceIcon Lib "comctl32" _
                                        (ByVal hImageList As IntPtr, _
                                        ByVal IconIndex As Integer, _
                                        ByVal hIcon As IntPtr) _
                                        As Integer
        Declare Auto Function ImageList_GetImageCount Lib "comctl32" _
                    (ByVal hImageList As IntPtr) As Integer
#End Region

#Region "       ImageList_GetIcon"
        Declare Function ImageList_GetIcon Lib "comctl32" ( _
                    ByVal himl As IntPtr, _
                    ByVal i As Integer, _
                    ByVal flags As ILD) As IntPtr
#End Region

#Region "       ImageList_Draw"
        Declare Function ImageList_Draw Lib "comctl32" ( _
            ByVal hIml As IntPtr, _
            ByVal indx As Integer, _
            ByVal hdcDst As IntPtr, _
            ByVal x As Integer, _
            ByVal y As Integer, _
            ByVal fStyle As Integer) As Integer
#End Region

#Region "       ImageList_DrawEx"
        'Used for hidden folders in ExpCombo
        Friend Declare Function ImageList_DrawEx Lib "comctl32" (ByVal hIml As IntPtr, ByVal i As Integer, ByVal hdcDst As IntPtr, _
           ByVal x As Integer, ByVal y As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal rgbBk As Integer, _
           ByVal rgbFg As Integer, ByVal fStyle As Integer) As Integer
#End Region

#End Region

#Region "       Ole32 Dll Declarations"

        <DllImport("ole32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
Public Shared Function RegisterDragDrop(ByVal hWnd As IntPtr, ByVal IdropTgt As ShellDll.IDropTarget) As Integer
        End Function

        ' Revokes the registration of the specified application window as a potential target for 
        ' OLE drag-and-drop operations
        <DllImport("ole32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function RevokeDragDrop(ByVal hWnd As IntPtr) As Integer
        End Function

        ' This function frees the specified storage medium
        <DllImport("ole32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Sub ReleaseStgMedium(ByRef pmedium As STGMEDIUM)
        End Sub

        ' Carries out an OLE drag and drop operation
        <DllImport("ole32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function DoDragDrop(ByVal pDataObject As IntPtr, <MarshalAs(UnmanagedType.Interface)> ByVal pDropSource As IDropSource, ByVal dwOKEffect As DragDropEffects, <System.Runtime.InteropServices.Out()> ByRef pdwEffect As DragDropEffects) As Integer
        End Function

        ' Retrieves a drag/drop helper interface for drawing the drag/drop images
        <DllImport("ole32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function CoCreateInstance(ByRef rclsid As Guid, _
                               ByVal pUnkOuter As IntPtr, _
                               ByVal dwClsContext As CLSCTX, _
                               ByRef riid As Guid, _
                               <Out()> ByRef ppv As IntPtr) As Integer
        End Function

        ' Retrieves a data object that you can use to access the contents of the clipboard
        <DllImport("ole32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function OleGetClipboard(<System.Runtime.InteropServices.Out()> ByRef ppDataObj As IntPtr) As Integer
        End Function

#End Region

#Region "       kernel32 Declarations"
        ''' <summary>
        ''' Locks a Global memory Handle. Used for referencing stg.hGlobal in some CIDA related cases
        ''' of ExploreControls. Returns a pointer to the actual data block, dealing with intra and inter
        ''' application Drag ops.
        ''' </summary>
        ''' <param name="handle">A Global memory handle.</param>
        ''' <returns>Pointer to actual data.</returns>
        ''' <remarks>Needed when actually implementing IDropTarget type processing.</remarks>
        <DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True)> _
        Public Shared Function GlobalLock(ByVal handle As IntPtr) As IntPtr
        End Function
        ''' <summary>
        ''' Releases a handle by decrementing a reference counter kept with it.
        ''' </summary>
        ''' <param name="handle">A previously GlobalLock locked Global memory handle.</param>
        ''' <returns>True if locks remain, False if none.</returns>
        ''' <remarks>Just unlocks a previous lock.</remarks>
        <DllImport("kernel32.dll", ExactSpelling:=True, SetLastError:=True)> _
        Public Shared Function GlobalUnlock(ByVal handle As IntPtr) As Boolean
        End Function

#End Region

#Region "       gdi32 Dll Declarations"

        <DllImport("gdi32", CharSet:=CharSet.Auto)> _
        Public Shared Function DeleteObject(ByVal hObject As IntPtr) As Integer
        End Function

#End Region

#Region "       Context Menu Related"

#Region " Structures "

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
         Public Structure MENUITEMINFO

            Public Sub New(ByVal text As String)
                cbSize = Marshal.SizeOf(Me)
                dwTypeData = text
                cch = text.Length
                fMask = 0
                fType = 0
                fState = 0
                wID = 0
                hSubMenu = IntPtr.Zero
                hbmpChecked = IntPtr.Zero
                hbmpUnchecked = IntPtr.Zero
                dwItemData = IntPtr.Zero
                hbmpItem = IntPtr.Zero
            End Sub

            Public cbSize As Integer
            Public fMask As Integer
            Public fType As Integer
            Public fState As Integer
            Public wID As Integer
            Public hSubMenu As IntPtr
            Public hbmpChecked As IntPtr
            Public hbmpUnchecked As IntPtr
            Public dwItemData As IntPtr
            <MarshalAs(UnmanagedType.LPTStr)> _
            Public dwTypeData As String
            Public cch As Integer
            Public hbmpItem As IntPtr
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure CMInvokeCommandInfoEx
            Public cbSize As Integer
            Public fMask As Integer
            Public hwnd As IntPtr
            Public lpVerb As IntPtr
            <MarshalAs(UnmanagedType.LPStr)> Public lpParameters As String
            <MarshalAs(UnmanagedType.LPStr)> Public lpDirectory As String
            Public nShow As Integer
            Public dwHotKey As Integer
            Public hIcon As IntPtr
            <MarshalAs(UnmanagedType.LPStr)> Public lpTitle As String
            Public lpVerbW As IntPtr
            <MarshalAs(UnmanagedType.LPWStr)> Public lpParametersW As String
            <MarshalAs(UnmanagedType.LPWStr)> Public lpDirectoryW As String
            <MarshalAs(UnmanagedType.LPWStr)> Public lpTitleW As String
            Public ptInvoke As System.Drawing.Point
        End Structure

#End Region

#End Region

#Region "       Drag/Drop Stuctures"

#Region "           FORMATETC Structure"
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Public Structure FORMATETC
            Public cfFormat As CF
            Public ptd As IntPtr
            Public dwAspect As DVASPECT
            Public lindex As Integer
            Public Tymd As TYMED 'ShellDll.ShellAPI.TYMED
        End Structure
#End Region

#Region "           STGMEDIUM Structure"
        <StructLayout(LayoutKind.Sequential)> _
    Public Structure STGMEDIUM
            Public tymed As Integer
            Public hGlobal As IntPtr
            Public pUnkForRelease As IntPtr
        End Structure
#End Region

#Region "           DROPFILES Structure"
        <StructLayout(LayoutKind.Sequential)> _
          Public Structure DROPFILES
            Public pFiles As Integer
            Public pt As POINT
            Public fNC As Boolean
            Public fWide As Boolean
        End Structure
#End Region

#End Region

#Region "   Public Shared Methods"

#Region "       GetSpecialFolderPath"
        Public Shared Function GetSpecialFolderPath(ByVal hWnd As IntPtr, ByVal csidl As Integer) As String
            Dim res As IntPtr
            Dim ppidl As IntPtr
            ppidl = GetSpecialFolderLocation(hWnd, csidl)
            Dim shfi As New SHFILEINFO()
            Dim uFlags As SHGFI = SHGFI.PIDL Or SHGFI.DISPLAYNAME Or SHGFI.TYPENAME
            'uFlags = uFlags Or SHGFI.SYSICONINDEX
            Dim dwAttr As Integer = 0
            res = SHGetFileInfo(ppidl, dwAttr, shfi, cbFileInfo, uFlags)
            Marshal.FreeCoTaskMem(ppidl)
            Return shfi.szDisplayName & "  (" & shfi.szTypeName & ")"
        End Function
#End Region

#Region "       GetSpecialFolderLocation"
        ''' <summary>
        ''' Returns an IntPtr referencing the PIDL of the requested Special Folder.
        ''' </summary>
        ''' <param name="hWnd">Unused</param>
        ''' <param name="csidl">The integer equivalent of the CSIDL Enum Value for the desired Special Folder.</param>
        ''' <returns>An IntPtr referencing the PIDL of the requested Special Folder.</returns>
        ''' <remarks></remarks>
        Public Shared Function GetSpecialFolderLocation(ByVal hWnd As IntPtr, ByVal csidl As Integer) As IntPtr
            Dim rVal As IntPtr
            Dim res As Integer
            res = SHGetSpecialFolderLocation(0, csidl, rVal)
            Return rVal
        End Function
#End Region

#Region "       IsXpOrAbove and Is2KOrAbove"
        ''' <summary>
        ''' Determines is the current OS is Windows XP or newer.
        ''' </summary>
        ''' <returns>True if current OS is Windows XP or newer. Returns False otherwise.</returns>
        ''' <remarks></remarks>
        Public Shared Function IsXpOrAbove() As Boolean
            Dim rVal As Boolean = False
            If Environment.OSVersion.Version.Major > 5 Then
                rVal = True
            ElseIf Environment.OSVersion.Version.Major = 5 AndAlso _
                   Environment.OSVersion.Version.Minor >= 1 Then
                rVal = True
            End If
            'if none of the above tests succeed, then return false
            Return rVal
        End Function

        ''' <summary>
        ''' Determines is the current OS is Windows 2000 or newer.
        ''' </summary>
        ''' <returns>True if current OS is Windows XP or newer. Returns False otherwise.</returns>
        ''' <remarks></remarks>
        Public Shared Function Is2KOrAbove() As Boolean
            If Environment.OSVersion.Version.Major >= 5 Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Determines is the current OS is Windows Vista or newer.
        ''' </summary>
        ''' <returns>True if current OS is Windows Vista or newer. Returns False otherwise.</returns>
        ''' <remarks></remarks>
        Public Shared Function IsVistaOrAbove() As Boolean
            If Environment.OSVersion.Version.Major >= 6 Then
                Return True
            Else
                Return False
            End If
        End Function
#End Region

#End Region

    End Class
End Namespace
