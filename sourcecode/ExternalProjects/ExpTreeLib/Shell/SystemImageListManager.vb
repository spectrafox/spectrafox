Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Drawing
Imports ExpTreeLib.ShellDll
Imports ExpTreeLib.ShellDll.ShellAPI

''' <summary>
''' Provides an Icon and IConIndex manager between <see cref="ExpTreeLib.CShItem">CShItem</see> and 3 per process System Image Lists,
''' one for Small Icons, one for Large Icons, and one for Extra Large Icons. The IConIndex for a given combination of base Icon and
''' overlays is synchronized such that the same IConIndex will serve for each list. 
''' </summary>
''' <remarks>
''' Correct usage is to obtain a CShItem in any of the normal methods of the CShItem Class. Typically, that CShItem will
''' not have its' IConIndex property assigned.<br />
''' Then call <see cref="ExpTreeLib.SystemImageListManager.GetIconIndex">SystemImageListManager.GetIconIndex</see> to obtain the 
''' true IConIndex into the per process ImageList.<br />
''' GetIconIndex will query CShItem.IconIndexNormal or CShItem.IconIndexOpen to obtain the base IconIndex. This
''' query will force CShItem to do the system call to obtain that icon index (if needed) and set the correct CShItem Property.<br />
''' GetIconIndex will then determine what, if any, Overlays should be applied and, if not already obtained,
''' obtain the Icon and place it in the per process ImageList and save the true IconIndex into the
''' HashTable and return the correct IconIndex to the caller.<br />
''' 
''' Incorporates ExtraLarge and Jumbo Icon code from Jens Madsen as of 5/11/2013 which is a modification of Calum's ExtraLarge code
''' </remarks>
Public Class SystemImageListManager

#Region "       ImageList Related Constants"
    ' For ImageList manipulation
    Private Const LVM_FIRST As Integer = &H1000
    Private Const LVM_SETIMAGELIST As Integer = (LVM_FIRST + 3)

    Private Const LVSIL_NORMAL As Integer = 0
    Private Const LVSIL_SMALL As Integer = 1
    Private Const LVSIL_STATE As Integer = 2
    Private Const LVSIL_GROUPHEADER As Integer = 3

    Private Const TV_FIRST As Integer = &H1100
    Private Const TVM_SETIMAGELIST As Integer = (TV_FIRST + 9)

    Private Const TVSIL_NORMAL As Integer = 0
    Private Const TVSIL_STATE As Integer = 2
#End Region

#Region "   Private Fields"
    Private Shared m_Initialized As Boolean = False
    Private Shared m_smImgList As IntPtr = IntPtr.Zero 'Handle to System Small ImageList
    Private Shared m_lgImgList As IntPtr = IntPtr.Zero 'Handle to System Large ImageList
    'UPDATE: Add m_xlgImgList
    Private Shared m_xlgImgList As IntPtr = IntPtr.Zero 'Handle to System XtraLarge ImageList
    Private Shared m_Table As New Hashtable(128)
    Private Shared m_jumboImgList As IntPtr = IntPtr.Zero 'Handle to System Jumbo ImageList
    'Private Shared m_Mutex As New Mutex()
    Private Shared SILMLock As New Object

    Public Enum LVSIL
        Normal = 0
        Small = 1
        State = 2
        GroupHeader = 3
    End Enum

    Public Enum SHIL
        Small = 1
        Large = 0
        XLarge = 2
        Jumbo = 4
    End Enum

#End Region

#Region "   New"
    ''' <summary>
    ''' Summary of Initializer.
    ''' </summary>
    ''' 
    Private Shared Sub Initializer()
        If m_Initialized Then
            Exit Sub
        End If

        Dim dwFlag As Integer = SHGFI.USEFILEATTRIBUTES Or _
                        SHGFI.SYSICONINDEX Or _
                        SHGFI.SMALLICON
        Dim shfi As New SHFILEINFO()
        m_smImgList = SHGetFileInfo(".txt", _
                           FILE_ATTRIBUTE_NORMAL, _
                           shfi, _
                           cbFileInfo, _
                           dwFlag)
        Debug.Assert((Not m_smImgList.Equals(IntPtr.Zero)), "Failed to create Image Small ImageList")
        If m_smImgList.Equals(IntPtr.Zero) Then
            Throw New Exception("Failed to create Small ImageList")
        End If

        dwFlag = SHGFI.USEFILEATTRIBUTES Or _
                        SHGFI.SYSICONINDEX Or _
                        SHGFI.LARGEICON
        m_lgImgList = SHGetFileInfo(".txt", _
                           FILE_ATTRIBUTE_NORMAL, _
                           shfi, _
                           cbFileInfo, _
                           dwFlag)
        Debug.Assert((Not m_lgImgList.Equals(IntPtr.Zero)), "Failed to create Image Small ImageList")
        If m_lgImgList.Equals(IntPtr.Zero) Then
            Throw New Exception("Failed to create Large ImageList")
        End If
        If IsXpOrAbove() Then   'Lower versions do not support XLarge Icons
            'UPDATE: Get the System IImageList object from the Shell for XLarge Icons:
            Dim iidImageList As New Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")
            SHGetImageListHandle(2, iidImageList, m_xlgImgList)
            Debug.Assert((Not m_xlgImgList.Equals(IntPtr.Zero)), "Failed to create Image XLarge ImageList")
            If m_xlgImgList.Equals(IntPtr.Zero) Then
                Throw New Exception("Failed to create XLarge ImageList")
            End If
            If IsVistaOrAbove() Then
                SHGetImageListHandle(4, iidImageList, m_jumboImgList)
                Debug.Assert((Not m_jumboImgList.Equals(IntPtr.Zero)), "Failed to create Image Jumbo ImageList")
                If m_jumboImgList.Equals(IntPtr.Zero) Then
                    Throw New Exception("Failed to create Jumbo ImageList")
                End If
            End If
        End If
        m_Initialized = True
        'Call here; SHGetIconOverlayIndex requies that the System ImageList is initialized...
        GetOverlayIndices()
    End Sub
#End Region

#Region "   Public Properties"
    ''' <summary>
    ''' The Handle (as IntPtr) of the per process System Image List containing Small Icons.
    ''' </summary>
    Public Shared ReadOnly Property hSmallImageList() As IntPtr
        Get
            Return m_smImgList
        End Get
    End Property
    ''' <summary>
    ''' The Handle (as IntPtr) of the per process System Image List containing Large Icons.
    ''' </summary>
    Public Shared ReadOnly Property hLargeImageList() As IntPtr
        Get
            Return m_lgImgList
        End Get
    End Property
    ''' <summary>
    ''' The Handle (as IntPtr) of the per process System Image List containing Extra Large Icons.
    ''' </summary>
    Public Shared ReadOnly Property hXLargeImageList() As IntPtr
        Get
            Return m_xlgImgList
        End Get
    End Property

    ''' <summary>
    ''' The Handle (as IntPtr) of the per process System Image List containing Jumbo Icons.
    ''' </summary>
    Public Shared ReadOnly Property hJumboImageList() As IntPtr
        Get
            Return m_jumboImgList
        End Get
    End Property
#End Region

#Region "   Public Methods"
#Region "       GetIconIndex"
    Private Shared mCnt As Integer
    Private Shared bCnt As Integer
    ''' <summary>
    ''' Location of the SHIL's overlay icons.
    ''' </summary>
    ''' <remarks>http://msdn.microsoft.com/en-us/library/windows/desktop/bb762183(v=vs.85).aspx </remarks>
    Public Shared ovlShare, ovlLink, ovlSlow, ovlDefault As Integer

    Private Shared Sub GetOverlayIndices()
        ovlLink = SHGetIconOverlayIndex(Nothing, IDO_SHGIOI.IDO_SHGIOI_LINK)
        ovlShare = SHGetIconOverlayIndex(Nothing, IDO_SHGIOI.IDO_SHGIOI_SHARE)
        ovlSlow = SHGetIconOverlayIndex(Nothing, IDO_SHGIOI.IDO_SHGIOI_SLOWFILE)
        ovlDefault = SHGetIconOverlayIndex(Nothing, IDO_SHGIOI.IDO_SHGIOI_DEFAULT)
    End Sub

    ''' <summary>
    ''' Queries the internal Hashtable of IConIndexes and returns the IconIndex for the requested CShItem.
    ''' </summary>
    ''' <param name="item">The CShItem for which the IconIndex is requested</param>
    ''' <param name="GetOpenIcon">True if the "open" IconIndex is requested</param>
    ''' <param name="GetSelectedIcon">True if the "Selected" Icon is requested</param>
    ''' <returns>The true IConIndex into the per process ImageList for the CShItem given as a parameter</returns>
    Public Shared Function GetIconIndex(ByVal item As CShItem, _
                                        Optional ByVal GetOpenIcon As Boolean = False, _
                                        Optional ByVal GetSelectedIcon As Boolean = False _
                                        ) As Integer

        Initializer()
        Dim HasOverlay As Boolean = False  'true if it's an overlay
        Dim rVal As Integer     'The returned Index

        Dim dwflag As SHGFI = SHGFI.SYSICONINDEX Or _
                        SHGFI.PIDL Or SHGFI.ICON
        Dim dwAttr As Integer = 0
        'build Key into HashTable for this Item
        Dim Key As Integer = CInt(IIf(Not GetOpenIcon, item.IconIndexNormalOrig * 256, _
                                                  item.IconIndexOpenOrig * 256))
        With item
            If .IsLink Then
                Key = Key Or 1
                dwflag = dwflag Or SHGFI.LINKOVERLAY
                HasOverlay = True
            End If
            If .IsShared Then
                Key = Key Or 2
                dwflag = dwflag Or SHGFI.ADDOVERLAYS
                HasOverlay = True
            End If
            If GetSelectedIcon Then
                Key = Key Or 4
                dwflag = dwflag Or SHGFI.SELECTED
                HasOverlay = True   'not really an overlay, but handled the same
            End If
            If m_Table.ContainsKey(Key) Then
                rVal = CInt((m_Table(Key)))
                mCnt += 1
            ElseIf Not HasOverlay Then  'for non-overlay icons, we already have
                rVal = Key \ 256        '  the right index -- put in table
                m_Table(Key) = rVal
                bCnt += 1
            Else        'don't have iconindex for an overlay, get it. 
                'This is the tricky part -- add overlaid Icon to systemimagelist
                '  use of SmallImageList from Calum McLellan
                Dim shfi As New SHFILEINFO
                Dim shfi_small As New SHFILEINFO
                Dim HR As IntPtr
                Dim HR_SMALL As IntPtr
                If .IsFileSystem And Not .IsDisk And Not .IsFolder Then
                    dwflag = dwflag Or SHGFI.USEFILEATTRIBUTES
                    dwAttr = FILE_ATTRIBUTE_NORMAL
                End If
                'UPDATE: OpenIcon with overlay
                If GetOpenIcon Then
                    dwflag = dwflag Or SHGFI.OPENICON
                End If
                HR = SHGetFileInfo(.PIDL, dwAttr, shfi, cbFileInfo, dwflag)
                HR_SMALL = SHGetFileInfo(.PIDL, dwAttr, shfi_small, cbFileInfo, dwflag Or SHGFI.SMALLICON)
                'm_Mutex.WaitOne()
                Dim rVal2 As Integer
                SyncLock SILMLock
                    rVal = ImageList_ReplaceIcon(m_smImgList, -1, shfi_small.hIcon)
                    Debug.Assert(rVal > -1, "Failed to add overlaid small icon")
                    rVal2 = ImageList_ReplaceIcon(m_lgImgList, -1, shfi.hIcon)
                    Debug.Assert(rVal2 > -1, "Failed to add overlaid large icon")
                    Debug.Assert(rVal = rVal2, "Small & Large IconIndices are Different")
                    ' Original Code by Calum.
                    'If m_xlgImgList <> IntPtr.Zero Then  'Not set on Windows earlier than XP
                    '    'UPDATE: Get XL Icon
                    '    'There are no XL Overlays so just get the normal Icon and add it 
                    '    'to the list again
                    '    Dim hIcon As IntPtr = IntPtr.Zero
                    '    rVal = GetNonOverlayIndex(item, GetOpenIcon)
                    '    hIcon = ImageList_GetIcon(m_xlgImgList, rVal, 0)
                    '    rVal = ImageList_ReplaceIcon(m_xlgImgList, -1, hIcon)
                    '    Debug.Assert(rVal > -1, "Failed to add overlaid xl icon")
                    '    DestroyIcon(hIcon)
                    '    Debug.Assert(rVal = rVal2, "XL & Large IconIndices are Different")
                    '    'END UPDATE
                    'End If

                    ' Jens' version 

                    If m_xlgImgList <> IntPtr.Zero Then  'Not set on Windows earlier than XP
                        Dim flags As ShellAPI.ILD = ILD.NORMAL '= 0    '5/9/2013 - JDP
                        If item.IsLink Then flags = flags Or INDEXTOOVERLAYMASK(ovlLink)
                        If item.IsShared Then flags = flags Or INDEXTOOVERLAYMASK(ovlShare)
                        Dim hIcon As IntPtr = IntPtr.Zero
                        rVal = GetNonOverlayIndex(item, GetOpenIcon)
                        hIcon = ImageList_GetIcon(m_xlgImgList, rVal, flags)
                        rVal = ImageList_ReplaceIcon(m_xlgImgList, -1, hIcon)
                        Dim rCnt As Integer = ImageList_GetImageCount(m_jumboImgList)
                        Debug.Assert(rVal > -1, "Failed to add overlaid xl icon")
                        DestroyIcon(hIcon)
                        Debug.Assert(rVal = rVal2, "XL & Large Icon Indices are Different")
                    End If
                    ' This fails at rVal = ImageList_ReplaceIcon (incomplete implementation of interface??)
                    If m_jumboImgList <> IntPtr.Zero Then  'Not set on Windows earlier than XP
                        Dim hIcon As IntPtr = IntPtr.Zero
                        rVal = GetNonOverlayIndex(item, GetOpenIcon)

                        Dim flags As ShellAPI.ILD = ILD.NORMAL
                        If item.IsLink Then flags = flags Or INDEXTOOVERLAYMASK(ovlLink)
                        If item.IsShared Then flags = flags Or INDEXTOOVERLAYMASK(ovlShare)
                        hIcon = ImageList_GetIcon(m_jumboImgList, rVal, flags)
                        rVal = ImageList_ReplaceIcon(m_jumboImgList, -1, hIcon)
                        If rVal < 0 Then        '5/11/2013 - JDP
                            rVal = rVal2        '5/11/2013 - JDP
                        End If                  '5/11/2013 - JDP
                        Debug.Assert(rVal > -1, "Failed to add overlaid Jumbo icon")
                        DestroyIcon(hIcon)
                        Debug.Assert(rVal = rVal2, "Jumbo & Large Icon Indices are Different")
                        '   END UPDATE
                    End If

                End SyncLock
                'm_Mutex.ReleaseMutex()
                DestroyIcon(shfi.hIcon)
                DestroyIcon(shfi_small.hIcon)
                If rVal < 0 OrElse rVal <> rVal2 Then
                    Throw New ApplicationException("Failed to add Icon for " & item.DisplayName)
                End If
                m_Table(Key) = rVal
            End If
        End With
        Return rVal
    End Function

    'UPDATE: Add GetNonOverlayIndex
    'Returns the normal non-overlay Icon for XL overlay Icons
    Public Shared Function GetNonOverlayIndex(ByRef item As CShItem, _
                                    Optional ByVal GetOpenIcon As Boolean = False _
                                    ) As Integer

        Initializer()
        Dim rVal As Integer     'The returned Index

        'build Key into HashTable for this Item
        Dim Key As Integer = CInt(IIf(Not GetOpenIcon, item.IconIndexNormalOrig * 256, _
                                                  item.IconIndexOpenOrig * 256))

        If m_Table.ContainsKey(Key) Then
            rVal = CInt(m_Table(Key))
            mCnt += 1
        Else                        '  for non-overlay icons, we already have
            rVal = Key \ 256        '  the right index -- put in table
            m_Table(Key) = rVal
            bCnt += 1
        End If
        Return rVal
    End Function


    ''' <summary>
    ''' Returns the index of the overlay icon in the system image list.
    ''' OBS! The System ImageList must be instantiated for this method to work!
    ''' </summary>
    ''' <param name="pszIconPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the fully qualified path of the file that contains the icon, or NOTHING to retrieve one of then standard overlay icons.</param>
    ''' <param name="iIconIndex">The icon's index in the file pointed to by pszIconPath. To request a standard overlay icon, set pszIconPath to NULL, and iIconIndex to one of the <seealso cref="SystemImageListManager.IDO_SHGIOI "/> flags.</param>
    ''' <returns>Returns the index of the overlay icon in the system image list if successful, or -1 otherwise.</returns>
    ''' <remarks>Icon overlays are part of the system image list. They have two identifiers. The first is a one-based overlay index that identifies the overlay relative to other overlays in the image list. The other is an image index that identifies the actual image. These two indexes are equivalent to the values that you assign to the iOverlay and iImage parameters, respectively, when you add an icon overlay to a private image list with ImageList_SetOverlayImage. SHGetIconOverlayIndex returns the overlay index. To convert an overlay index to its equivalent image index, call <seealso  cref= "INDEXTOOVERLAYMASK " />. 
    ''' Note: After the image has been loaded into the system image list during initialization, it cannot be changed. The file name and index specified by pszIconPath and iIconIndex are used only to identify the icon overlay. SHGetIconOverlayIndex cannot be used to modify the system image list.
    ''' http://msdn.microsoft.com/en-us/library/windows/desktop/bb762183(v=vs.85).aspx </remarks>

    <System.Runtime.InteropServices.DllImportAttribute("Shell32.dll", EntryPoint:="SHGetIconOverlayIndex")> _
    Public Shared Function SHGetIconOverlayIndex(<System.Runtime.InteropServices.InAttribute(), System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPTStr)> ByVal pszIconPath As String, ByVal iIconIndex As Integer) As Integer
    End Function

    'Private Shared Function INDEXTOOVERLAYMASK(ByVal i As Integer) As Integer
    ''' <summary>
    ''' Mockup of Shell Macros.
    ''' </summary>
    ''' <param name="i"></param>
    ''' <returns></returns>
    ''' <remarks>Prepares the index of an overlay mask so that ImageList_GetIcon and ImageList_Draw can use it. </remarks>
    Public Shared Function INDEXTOOVERLAYMASK(ByVal i As Integer) As Integer
        Return ((i) << 8)
    End Function
    Public Shared Function INDEXTOSTATEIMAGEMASK(ByVal i As Integer) As Integer
        Return ((i) << 12)
    End Function

    ''' <summary>
    ''' Used by <see cref="SHGetIconOverlayIndex "/> to request a standard overlay icon: 
    ''' Set pszIconPath to NULL, and iIconIndex to one of the following values:
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum IDO_SHGIOI
        IDO_SHGIOI_SHARE = &HFFFFFFF
        IDO_SHGIOI_LINK = &HFFFFFFE
        IDO_SHGIOI_SLOWFILE = &HFFFFFFFD
        IDO_SHGIOI_DEFAULT = &HFFFFFFFC
    End Enum


    'Private Shared Sub DebugShowImages(ByVal useSmall As Boolean, ByVal iFrom As Integer, ByVal iTo As Integer)
    '    Dim RightIcon As Icon = GetIcon(iFrom, Not useSmall)
    '    Dim rightIndex As Integer = iFrom
    '    Do While iFrom <= iTo
    '        Dim ico As Icon = GetIcon(iFrom, useSmall)
    '        Dim fShow As New frmDebugShowImage(rightIndex, RightIcon, ico, IIf(useSmall, "Small ImageList", "Large ImageList"), iFrom)
    '        fShow.ShowDialog()
    '        fShow.Dispose()
    '        iFrom += 1
    '    Loop
    'End Sub
#End Region

#Region "       GetIcon"
    ''' <summary>
    ''' Returns a GDI+ copy of a Large or Small icon from the ImageList
    ''' at the specified index.</summary>
    ''' <param name="Index">The IconIndex of the desired Icon</param>
    ''' <param name="smallIcon">Optional, default = False. If True, return the
    '''   icon from the Small ImageList rather than the Large.</param>
    ''' <returns>The specified Icon or Nothing</returns>
    Public Shared Function GetIcon(ByVal Index As Integer, _
                            Optional ByVal smallIcon As Boolean = False) _
                            As Icon
        Initializer()
        Dim icon As Icon = Nothing
        Dim hIcon As IntPtr = IntPtr.Zero
        'Customisation to return a small image
        If smallIcon Then
            hIcon = ImageList_GetIcon(m_smImgList, Index, 0)
        Else
            hIcon = ImageList_GetIcon(m_lgImgList, Index, 0)
        End If
        If Not IsNothing(hIcon) Then
            icon = System.Drawing.Icon.FromHandle(hIcon)
        End If
        Return icon
    End Function

    ''' <summary>
    '''Returns a GDI+ copy of an Extra Large Icon from the ImageList 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <returns>The desired Icon or Nothing</returns>
    ''' <remarks></remarks>
    Public Shared Function GetXLIcon(ByVal index As Integer) As Icon
        Initializer()
        Dim icon As Icon = Nothing
        If m_xlgImgList <> IntPtr.Zero Then
            Dim hIcon As IntPtr = IntPtr.Zero
            hIcon = ImageList_GetIcon(m_xlgImgList, index, 0)
            If Not IsNothing(hIcon) Then
                icon = System.Drawing.Icon.FromHandle(hIcon)
            End If
        End If
        Return icon
    End Function

    ''' <summary>
    '''Returns a GDI+ copy of an Jumbo Icon from the ImageList 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <returns>The desired Icon or Nothing</returns>
    ''' <remarks></remarks>
    Public Shared Function GetJumboIcon(ByVal index As Integer) As Icon
        Initializer()
        Dim icon As Icon = Nothing
        If m_jumboImgList <> IntPtr.Zero Then
            Dim hIcon As IntPtr = IntPtr.Zero
            hIcon = ImageList_GetIcon(m_jumboImgList, index, 0)
            If Not IsNothing(hIcon) Then
                icon = System.Drawing.Icon.FromHandle(hIcon)
            End If
        End If
        Return icon
    End Function

#End Region

#Region "       SetListViewImageList"
    '''   <summary>
    '''    Associates a SysImageList with a ListView control
    '''    </summary>
    '''    <param name="listView">ListView control to associate ImageList with</param>
    '''    <param name="forLargeIcons">True=Set Large Icon List
    '''                   False=Set Small Icon List</param>
    '''    <param name="forStateImages">Whether to add ImageList as StateImageList</param>
    Public Shared Sub SetListViewImageList( _
            ByVal listView As ListView, _
            ByVal forLargeIcons As Boolean, _
            ByVal forStateImages As Boolean)

        Initializer()
        Dim wParam As Integer = LVSIL_NORMAL
        Dim HImageList As IntPtr = m_lgImgList
        If Not forLargeIcons Then
            wParam = LVSIL_SMALL
            HImageList = m_smImgList
        End If
        If forStateImages Then
            wParam = LVSIL_STATE
        End If
        SendMessage(listView.Handle, _
                    LVM_SETIMAGELIST, _
                    wParam, _
                    HImageList)
    End Sub

    '''   <summary>
    '''    Associates a SysImageList with a ListView control
    '''    </summary>
    '''    <param name="listView">ListView control to associate ImageList with</param>
    '''    <param name="Usage">State, Group, Normal, Small
    '''                   False=Set Small Icon List</param>
    '''    <param name="IIlSize">Size of Images</param>
    Public Shared Sub SetListViewImageList( _
            ByVal listView As ListView, _
            ByVal Usage As LVSIL, _
            ByVal IIlSize As SHIL)

        Initializer()
        Dim wParam As Integer = CInt(Usage)
        Dim HImageList As IntPtr = m_lgImgList
        If IIlSize = SHIL.Small Then
            HImageList = m_smImgList
        ElseIf IIlSize = SHIL.Jumbo Then
            HImageList = m_jumboImgList
        ElseIf IIlSize = SHIL.XLarge Then
            HImageList = m_xlgImgList
        End If
        SendMessage(listView.Handle, _
                    LVM_SETIMAGELIST, _
                    wParam, _
                    HImageList)
    End Sub
#End Region

#Region "       SetTreeViewImageList"
    ''' <summary>
    ''' Associates a SysImageList with a TreeView control
    ''' </summary>
    ''' <param name="treeView">TreeView control to associate the ImageList with</param>
    ''' <param name="forStateImages">Whether to add ImageList as StateImageList</param>
    Public Shared Sub SetTreeViewImageList( _
        ByVal treeView As TreeView, _
        ByVal forStateImages As Boolean)

        Initializer()
        Dim wParam As Integer = LVSIL_NORMAL
        If forStateImages Then
            wParam = LVSIL_STATE
        End If
        'Dim HR As Integer                      '12/31/2013
        'HR = SendMessage(treeView.Handle, _    '12/31/2013
        SendMessage(treeView.Handle, _
                    TVM_SETIMAGELIST, _
                    wParam, _
                    m_smImgList)
    End Sub

#End Region

#End Region
End Class