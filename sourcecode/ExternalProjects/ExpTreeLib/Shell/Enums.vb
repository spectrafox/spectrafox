Namespace ShellDll
    ' This file has been split from the ShellDll.vb file simply to reduce its size. Otherwise it is
    ' just an extension of ShellAPI. It contains various Enums used in referencing the Windows APIs.
    Partial Public Class ShellAPI

#Region "   Windows Messages"
        ''' <summary>
        ''' Windows Message Numbers
        ''' </summary>
        ''' <remarks></remarks>
        <Flags()> _
        Public Enum WM As UInt32
            ACTIVATE = 6
            ACTIVATEAPP = 28
            AFXFIRST = 864
            AFXLAST = 895
            APP = 32768
            ASKCBFORMATNAME = 780
            CANCELJOURNAL = 75
            CANCELMODE = 31
            CAPTURECHANGED = 533
            CHANGECBCHAIN = 781
            [CHAR] = 258
            CHARTOITEM = 47
            CHILDACTIVATE = 34
            CLEAR = 771
            CLOSE = 16
            COMMAND = 273
            COMPACTING = 65
            COMPAREITEM = 57
            CONTEXTMENU = 123
            COPY = 769
            COPYDATA = 74
            CREATE = 1
            CTLCOLORBTN = 309
            CTLCOLORDLG = 310
            CTLCOLOREDIT = 307
            CTLCOLORLISTBOX = 308
            CTLCOLORMSGBOX = 306
            CTLCOLORSCROLLBAR = 311
            CTLCOLORSTATIC = 312
            CUT = 768
            DEADCHAR = 259
            DELETEITEM = 45
            DESTROY = 2
            DESTROYCLIPBOARD = 775
            DEVICECHANGE = 537
            DEVMODECHANGE = 27
            DISPLAYCHANGE = 126
            DRAWCLIPBOARD = 776
            DRAWITEM = 43
            DROPFILES = 563
            ENABLE = 10
            ENDSESSION = 22
            ENTERIDLE = 289
            ENTERMENULOOP = 529
            ENTERSIZEMOVE = 561
            ERASEBKGND = 20
            EXITMENULOOP = 530
            EXITSIZEMOVE = 562
            FONTCHANGE = 29
            GETDLGCODE = 135
            GETFONT = 49
            GETHOTKEY = 51
            GETICON = 127
            GETMINMAXINFO = 36
            GETOBJECT = 61
            GETSYSMENU = 787
            GETTEXT = 13
            GETTEXTLENGTH = 14
            HANDHELDFIRST = 856
            HANDHELDLAST = 863
            HELP = 83
            HOTKEY = 786
            HSCROLL = 276
            HSCROLLCLIPBOARD = 782
            ICONERASEBKGND = 39
            IME_CHAR = 646
            IME_COMPOSITION = 271
            IME_COMPOSITIONFULL = 644
            IME_CONTROL = 643
            IME_ENDCOMPOSITION = 270
            IME_KEYDOWN = 656
            IME_KEYLAST = 271
            IME_KEYUP = 657
            IME_NOTIFY = 642
            IME_REQUEST = 648
            IME_SELECT = 645
            IME_SETCONTEXT = 641
            IME_STARTCOMPOSITION = 269
            INITDIALOG = 272
            INITMENU = 278
            INITMENUPOPUP = 279
            INPUTLANGCHANGE = 81
            INPUTLANGCHANGEREQUEST = 80
            KEYDOWN = 256
            KEYFIRST = 256
            KEYLAST = 264
            KEYUP = 257
            KILLFOCUS = 8
            LBUTTONDBLCLK = 515
            LBUTTONDOWN = 513
            LBUTTONUP = 514
            LVM_GETEDITCONTROL = 4120
            LVM_SETIMAGELIST = 4099
            MBUTTONDBLCLK = 521
            MBUTTONDOWN = 519
            MBUTTONUP = 520
            MDIACTIVATE = 546
            MDICASCADE = 551
            MDICREATE = 544
            MDIDESTROY = 545
            MDIGETACTIVE = 553
            MDIICONARRANGE = 552
            MDIMAXIMIZE = 549
            MDINEXT = 548
            MDIREFRESHMENU = 564
            MDIRESTORE = 547
            MDISETMENU = 560
            MDITILE = 550
            MEASUREITEM = 44
            MENUCHAR = 288
            MENUCOMMAND = 294
            MENUDRAG = 291
            MENUGETOBJECT = 292
            MENURBUTTONUP = 290
            MENUSELECT = 287
            MOUSEACTIVATE = 33
            MOUSEFIRST = 512
            MOUSEHOVER = 673
            MOUSELAST = 522
            MOUSELEAVE = 675
            MOUSEMOVE = 512
            MOUSEWHEEL = 522
            MOVE = 3
            MOVING = 534
            NCACTIVATE = 134
            NCCALCSIZE = 131
            NCCREATE = 129
            NCDESTROY = 130
            NCHITTEST = 132
            NCLBUTTONDBLCLK = 163
            NCLBUTTONDOWN = 161
            NCLBUTTONUP = 162
            NCMBUTTONDBLCLK = 169
            NCMBUTTONDOWN = 167
            NCMBUTTONUP = 168
            NCMOUSEHOVER = 672
            NCMOUSELEAVE = 674
            NCMOUSEMOVE = 160
            NCPAINT = 133
            NCRBUTTONDBLCLK = 166
            NCRBUTTONDOWN = 164
            NCRBUTTONUP = 165
            NEXTDLGCTL = 40
            NEXTMENU = 531
            NOTIFY = 78
            NOTIFYFORMAT = 85
            [NULL] = 0
            PAINT = 15
            PAINTCLIPBOARD = 777
            PAINTICON = 38
            PALETTECHANGED = 785
            PALETTEISCHANGING = 784
            PARENTNOTIFY = 528
            PASTE = 770
            PENWINFIRST = 896
            PENWINLAST = 911
            POWER = 72
            PRINT = 791
            PRINTCLIENT = 792
            QUERYDRAGICON = 55
            QUERYENDSESSION = 17
            QUERYNEWPALETTE = 783
            QUERYOPEN = 19
            QUEUESYNC = 35
            QUIT = 18
            RBUTTONDBLCLK = 518
            RBUTTONDOWN = 516
            RBUTTONUP = 517
            RENDERALLFORMATS = 774
            RENDERFORMAT = 773
            SETCURSOR = 32
            SETFOCUS = 7
            SETFONT = 48
            SETHOTKEY = 50
            SETICON = 128
            SETMARGINS = 211
            SETREDRAW = 11
            SETTEXT = 12
            SETTINGCHANGE = 26
            SH_NOTIFY = 1025
            SHOWWINDOW = 24
            SIZE = 5
            SIZECLIPBOARD = 779
            SIZING = 532
            SPOOLERSTATUS = 42
            STYLECHANGED = 125
            STYLECHANGING = 124
            SYNCPAINT = 136
            SYSCHAR = 262
            SYSCOLORCHANGE = 21
            SYSCOMMAND = 274
            SYSDEADCHAR = 263
            SYSKEYDOWN = 260
            SYSKEYUP = 261
            TCARD = 82
            TIMECHANGE = 30
            TIMER = 275
            TVM_GETEDITCONTROL = 4367
            TVM_SETIMAGELIST = 4361
            UNDO = 772
            UNINITMENUPOPUP = 293
            USER = 1024
            USERCHANGED = 84
            VKEYTOITEM = 46
            VSCROLL = 277
            VSCROLLCLIPBOARD = 778
            WINDOWPOSCHANGED = 71
            WINDOWPOSCHANGING = 70
            WININICHANGE = 26
        End Enum

#End Region

#Region "   Shell Enumerations"

#Region "   CSIDL"
        Public Enum CSIDL As Integer
            DESKTOP = &H0
            INTERNET = &H1
            PROGRAMS = &H2
            CONTROLS = &H3
            PRINTERS = &H4
            PERSONAL = &H5
            FAVORITES = &H6
            STARTUP = &H7
            RECENT = &H8
            SENDTO = &H9
            BITBUCKET = &HA
            STARTMENU = &HB
            MYDOCUMENTS = &HC
            MYMUSIC = &HD
            MYVIDEO = &HE
            DESKTOPDIRECTORY = &H10
            DRIVES = &H11
            NETWORK = &H12
            NETHOOD = &H13
            FONTS = &H14
            TEMPLATES = &H15
            COMMON_STARTMENU = &H16
            COMMON_PROGRAMS = &H17
            COMMON_STARTUP = &H18
            COMMON_DESKTOPDIRECTORY = &H19
            APPDATA = &H1A
            PRINTHOOD = &H1B
            LOCAL_APPDATA = &H1C
            ALTSTARTUP = &H1D
            COMMON_ALTSTARTUP = &H1E
            COMMON_FAVORITES = &H1F
            INTERNET_CACHE = &H20
            COOKIES = &H21
            HISTORY = &H22
            COMMON_APPDATA = &H23
            WINDOWS = &H24
            SYSTEM = &H25
            PROGRAM_FILES = &H26
            MYPICTURES = &H27
            PROFILE = &H28
            SYSTEMX86 = &H29
            PROGRAM_FILESX86 = &H2A
            PROGRAM_FILES_COMMON = &H2B
            PROGRAM_FILES_COMMONX86 = &H2C
            COMMON_TEMPLATES = &H2D
            COMMON_DOCUMENTS = &H2E
            COMMON_ADMINTOOLS = &H2F
            ADMINTOOLS = &H30
            CONNECTIONS = &H31
            COMMON_MUSIC = &H35
            COMMON_PICTURES = &H36
            COMMON_VIDEO = &H37
            RESOURCES = &H38
            RESOURCES_LOCALIZED = &H39
            COMMON_OEM_LINKS = &H3A
            CDBURN_AREA = &H3B
            COMPUTERSNEARME = &H3D
            FLAG_PER_USER_INIT = &H800
            FLAG_NO_ALIAS = &H1000
            FLAG_DONT_VERIFY = &H4000
            FLAG_CREATE = &H8000
            FLAG_MASK = &HFF00
        End Enum
#End Region

#Region "   E_STRRET"

        <Flags()> _
        Private Enum E_STRRET
            WSTR = &H0          ' Use STRRET.pOleStr
            OFFSET = &H1        ' Use STRRET.uOffset to Ansi
            C_STR = &H2         ' Use STRRET.cStr
        End Enum
#End Region

#Region "   SHCONTF"
        <Flags()> _
        Public Enum SHCONTF
            EMPTY = 0                      ' used to zero a SHCONTF variable
            FOLDERS = &H20                 ' only want folders enumerated (FOLDER)
            NONFOLDERS = &H40              ' include non folders
            INCLUDEHIDDEN = &H80           ' show items normally hidden
            INIT_ON_FIRST_NEXT = &H100     ' allow EnumObject() to return before validating enum
            NETPRINTERSRCH = &H200         ' hint that client is looking for printers
            SHAREABLE = &H400              ' hint that client is looking sharable resources (remote shares)
            STORAGE = &H800                ' include all items with accessible storage and their ancestors
        End Enum
#End Region

#Region "   SHCIDS"
        <Flags()> _
        Public Enum SHCIDS
            ALLFIELDS = &H80000000
            CANONICALONLY = &H10000000
            BITMASK = &HFFFF0000
            COLUMNMASK = &HFFFF
        End Enum
#End Region

#Region "   SFGAO"
        <Flags()> _
        Public Enum SFGAO
            CANCOPY = &H1                    ' Objects can be copied    
            CANMOVE = &H2                    ' Objects can be moved     
            CANLINK = &H4                    ' Objects can be linked    
            STORAGE = &H8                    ' supports BindToObject(IID_IStorage)
            CANRENAME = &H10                 ' Objects can be renamed
            CANDELETE = &H20                 ' Objects can be deleted
            HASPROPSHEET = &H40              ' Objects have property sheets
            DROPTARGET = &H100               ' Objects are drop target
            CAPABILITYMASK = &H177           ' This flag is a mask for the capability flags.
            ENCRYPTED = &H2000               ' object is encrypted (use alt color)
            ISSLOW = &H4000                  ' 'slow' object
            GHOSTED = &H8000                 ' ghosted icon
            LINK = &H10000                   ' Shortcut (link)
            SHARE = &H20000                  ' shared
            RDONLY = &H40000               ' read-only
            HIDDEN = &H80000                 ' hidden object
            DISPLAYATTRMASK = &HFC000        ' This flag is a mask for the display attributes.
            FILESYSANCESTOR = &H10000000     ' may contain children with FILESYSTEM
            FOLDER = &H20000000              ' support BindToObject(IID_IShellFolder)
            FILESYSTEM = &H40000000          ' is a win32 file system object (file/folder/root)
            HASSUBFOLDER = &H80000000        ' may contain children with FOLDER
            CONTENTSMASK = &H80000000        ' This flag is a mask for the contents attributes.
            VALIDATE = &H1000000             ' invalidate cached information
            REMOVABLE = &H2000000            ' is this removeable media?
            COMPRESSED = &H4000000           ' Object is compressed (use alt color)
            BROWSABLE = &H8000000            ' supports IShellFolder but only implements CreateViewObject() (non-folder view)
            NONENUMERATED = &H100000         ' is a non-enumerated object
            NEWCONTENT = &H200000            ' should show bold in explorer tree
            CANMONIKER = &H400000            ' defunct
            HASSTORAGE = &H400000            ' defunct
            STREAM = &H400000                ' supports BindToObject(IID_IStream)
            STORAGEANCESTOR = &H800000       ' may contain children with STORAGE or STREAM
            STORAGECAPMASK = &H70C50008      ' for determining storage capabilities ie for open/save semantics
        End Enum
#End Region

#Region "   SHGFI"
        <Flags()> _
        Public Enum SHGFI
            ICON = &H100                ' get icon 
            DISPLAYNAME = &H200         ' get display name 
            TYPENAME = &H400            ' get type name 
            ATTRIBUTES = &H800          ' get attributes 
            ICONLOCATION = &H1000       ' get icon location 
            EXETYPE = &H2000            ' return exe type 
            SYSICONINDEX = &H4000       ' get system icon index 
            LINKOVERLAY = &H8000        ' put a link overlay on icon 
            SELECTED = &H10000          ' show icon in selected state 
            ATTR_SPECIFIED = &H20000    ' get only specified attributes 
            LARGEICON = &H0             ' get large icon 
            SMALLICON = &H1             ' get small icon 
            OPENICON = &H2              ' get open icon 
            SHELLICONSIZE = &H4         ' get shell size icon 
            PIDL = &H8                  ' pszPath is a pidl 
            USEFILEATTRIBUTES = &H10    ' use passed dwFileAttribute 
            ADDOVERLAYS = &H20          ' apply the appropriate overlays
            OVERLAYINDEX = &H40         ' Get the index of the overlay
        End Enum
#End Region

#Region "   SHGDN"
        <Flags()> _
        Public Enum SHGDN
            NORMAL = 0
            INFOLDER = 1
            FORADDRESSBAR = 16384
            FORPARSING = 32768
        End Enum
#End Region

#Region "   ILD --- Flags controlling how the Image List item is drawn"
        '/// <summary>
        '/// Flags controlling how the Image List item is 
        '/// drawn
        '/// </summary>
        '[Flags]	
        '   Public Enum ImageListDrawItemConstants : int
        '{
        '	/// <summary>
        '	/// Draw item normally.
        '	/// </summary>
        '	ILD_NORMAL = 0x0,
        '	/// <summary>
        '	/// Draw item transparently.
        '	/// </summary>
        '	ILD_TRANSPARENT = 0x1,
        '	/// <summary>
        '	/// Draw item blended with 25% of the specified foreground colour
        '	/// or the Highlight colour if no foreground colour specified.
        '	/// </summary>
        '	ILD_BLEND25 = 0x2,
        '	/// <summary>
        '	/// Draw item blended with 50% of the specified foreground colour
        '	/// or the Highlight colour if no foreground colour specified.
        '	/// </summary>
        '	ILD_SELECTED = 0x4,
        '	/// <summary>
        '	/// Draw the icon's mask
        '	/// </summary>
        '	ILD_MASK = 0x10,
        '	/// <summary>
        '	/// Draw the icon image without using the mask
        '	/// </summary>
        '	ILD_IMAGE = 0x20,
        '	/// <summary>
        '	/// Draw the icon using the ROP specified.
        '	/// </summary>
        '	ILD_ROP = 0x40,
        '	/// <summary>
        '	/// Preserves the alpha channel in dest. XP only.
        '	/// </summary>
        '	ILD_PRESERVEALPHA = 0x1000,
        '	/// <summary>
        '	/// Scale the image to cx, cy instead of clipping it.  XP only.
        '	/// </summary>
        '	ILD_SCALE = 0x2000,
        '	/// <summary>
        '	/// Scale the image to the current DPI of the display. XP only.
        '	/// </summary>
        '	ILD_DPISCALE = 0x4000
        '/// <summary>
        '/// Flags controlling how the Image List item is 
        '/// drawn
        '/// </summary>
        <Flags()> _
        Public Enum ILD
            NORMAL = &H0
            TRANSPARENT = &H1
            BLEND25 = &H2
            SELECTED = &H4
            MASK = &H10
            IMAGE = &H20
            ROP = &H40
            PRESERVEALPHA = &H1000
            SCALE = &H2000
            DPISCALE = &H4000
        End Enum
#End Region

#Region "   ILS --- XP ImageList Draw State options"
        '   /// <summary>
        '/// Enumeration containing XP ImageList Draw State options
        '/// </summary>
        Public Enum ILS
            NORMAL = (&H0)      'The image state is not modified.
            GLOW = (&H1)        'The color for the glow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
            SHADOW = (&H2)      'The color for the drop shadow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
            SATURATE = (&H4)    'The amount to increase is indicated by the frame member in the IMAGELISTDRAWPARAMS method. 
            ALPHA = (&H8)       'The value of the alpha channel is indicated by the frame member in the IMAGELISTDRAWPARAMS method. The alpha channel can be from 0 to 255 with 0 being completely transparent and 255 being completely opaque. 
        End Enum
#End Region

#Region "   SLR --- IShellLink.Resolve Flags"
        <Flags()> _
        Public Enum SLR
            NO_UI = &H1
            ANY_MATCH = &H2
            UPDATE = &H4
            NOUPDATE = &H8
            NOSEARCH = &H10
            NOTRACK = &H20
            NOLINKINFO = &H40
            INVOKE_MSI = &H80
            NO_UI_WITH_MSG_PUMP = &H101
        End Enum
#End Region

#Region "   SLGP --- IShellLink.GetPath Flags"
        <Flags()> _
        Public Enum SLGP
            SHORTPATH = &H1
            UNCPRIORITY = &H2
            RAWPATH = &H4
        End Enum
#End Region

#Region "   SHGNLI -- SHGetNewLinkInfo flags"
        <Flags()> _
    Public Enum SHGNLI
            PIDL = 1        'pszLinkTo is a pidl
            PREFIXNAME = 2  'Make name "Shortcut to xxx"
            NOUNIQUE = 4    'don't do the unique name generation
            NOLNK = 8       'don't add ".lnk" extension (Win2k or higher,IE5 or higher)
        End Enum

#End Region

        ' Indicate whether the method should try to return a name in the pwcsName member of the STATSTG structure
        <Flags()> _
        Public Enum STATFLAG
            [DEFAULT] = 0
            NONAME = 1
            NOOPEN = 2
        End Enum

        ' Indicate the type of locking requested for the specified range of bytes
        <Flags()> _
        Public Enum LOCKTYPE
            WRITE = 1
            EXCLUSIVE = 2
            ONLYONCE = 4
        End Enum

        ' Used in the type member of the STATSTG structure to indicate the type of the storage element
        Public Enum STGTY
            STORAGE = 1
            STREAM = 2
            LOCKBYTES = 3
            [PROPERTY] = 4
        End Enum

        ' Indicate conditions for creating and deleting the object and access modes for the object
        <Flags()> _
        Public Enum STGM
            DIRECT = &H0
            TRANSACTED = &H10000
            SIMPLE = &H8000000
            READ = &H0
            WRITE = &H1
            READWRITE = &H2
            SHARE_DENY_NONE = &H40
            SHARE_DENY_READ = &H30
            SHARE_DENY_WRITE = &H20
            SHARE_EXCLUSIVE = &H10
            PRIORITY = &H40000
            DELETEONRELEASE = &H4000000
            NOSCRATCH = &H100000
            CREATE = &H1000
            CONVERT = &H20000
            FAILIFTHERE = &H0
            NOSNAPSHOT = &H200000
            DIRECT_SWMR = &H400000
        End Enum

        ' Indicate whether a storage element is to be moved or copied
        Public Enum STGMOVE
            MOVE = 0
            COPY = 1
            SHALLOWCOPY = 2
        End Enum

        ' Specify the conditions for performing the commit operation in the IStorage::Commit and IStream::Commit methods
        <Flags()> _
        Public Enum STGC
            [DEFAULT] = 0
            OVERWRITE = 1
            ONLYIFCURRENT = 2
            DANGEROUSLYCOMMITMERELYTODISKCACHE = 4
            CONSOLIDATE = 8
        End Enum

        ' Directing the handling of the item from which you're retrieving the info tip text
        <Flags()> _
        Public Enum QITIPF
            [DEFAULT] = &H0
            USENAME = &H1
            LINKNOTARGET = &H2
            LINKUSETARGET = &H4
            USESLOWTIP = &H8
        End Enum

#End Region

#Region "   Context Menu Related Enums "

        <Flags()> _
        Public Enum CLSCTX As UInt32
            ' Fields
            ALL = 23
            DISABLE_AAA = 32768
            ENABLE_AAA = 65536
            ENABLE_CODE_DOWNLOAD = 8192
            FROM_DEFAULT_CONTEXT = 131072
            INPROC = 3
            INPROC_HANDLER = 2
            INPROC_HANDLER16 = 32
            INPROC_SERVER = 1
            INPROC_SERVER16 = 8
            LOCAL_SERVER = 4
            NO_CODE_DOWNLOAD = 1024
            NO_CUSTOM_MARSHAL = 4096
            NO_FAILURE_LOG = 16384
            REMOTE_SERVER = 16
            RESERVED1 = 64
            RESERVED2 = 128
            RESERVED3 = 256
            RESERVED4 = 512
            RESERVED5 = 2048
            SERVER = 21
        End Enum

        Public Enum CMD
            TILES = 100001
            LARGEICON = 100002
            LIST = 100003
            DETAILS = 100004
            THUMBNAILS = 100005
            REFRESH = 100006
            PASTE = 100007
            PASTELINK = 100008
            PROPERTIES = 100009
            ARRANGEICONS = 100010
        End Enum

        Public Enum MK
            LBUTTON = &H1
            RBUTTON = &H2
            SHIFT = &H4
            CONTROL = &H8
            MBUTTON = &H10
            ALT = &H20
        End Enum

        Public Enum CMIC
            HOTKEY = &H20
            ICON = &H10
            FLAG_NO_UI = &H400
            UNICODE = &H4000
            NO_CONSOLE = &H8000
            ASYNCOK = &H100000
            NOZONECHECKS = &H800000
            SHIFT_DOWN = &H10000000
            CONTROL_DOWN = &H40000000
            FLAG_LOG_USAGE = &H4000000
            PTINVOKE = &H20000000
        End Enum

        Public Enum SW
            HIDE = 0
            SHOWNORMAL = 1
            SHOW = 5
            SHOWDEFAULT = 10
            SHOWMAXIMIZED = 3
            SHOWMINIMIZED = 2
            SHOWMINNOACTIVE = 7
            SHOWNOACTIVATE = 4
        End Enum

        Public Enum TPM
            CENTERALIGN = &H4
            LEFTALIGN = &H0
            RIGHTALIGN = &H8
            BOTTOMALIGN = &H20
            TOPALIGN = &H0
            VCENTERALIGN = &H10
            NONOTIFY = &H80
            RETURNCMD = &H100
            LEFTBUTTON = &H0
            RIGHTBUTTON = &H2
        End Enum

        Public Enum CMF
            NORMAL = &H0
            DEFAULTONLY = &H1
            VERBSONLY = &H2
            EXPLORE = &H4
            NOVERBS = &H8
            CANRENAME = &H10
            NODEFAULT = &H20
            INCLUDESTATIC = &H40
            EXTENDEDVERBS = &H100
            RESERVED = &HFFFF0000
        End Enum

        Public Enum GCS
            VERBA = 0
            HELPTEXTA = 1
            VALIDATEA = 2
            VERBW = 4
            HELPTEXTW = 5
            VALIDATEW = 6
        End Enum

        Public Enum MFT
            GRAYED = &H3
            DISABLED = &H3
            CHECKED = &H8
            SEPARATOR = &H800
            RADIOCHECK = &H200
            BITMAP = &H4
            OWNERDRAW = &H100
            MENUBARBREAK = &H20
            MENUBREAK = &H40
            RIGHTORDER = &H2000
            BYCOMMAND = &H0
            BYPOSITION = &H400
            POPUP = &H10
        End Enum

        Public Enum MIIM
            BITMAP = &H80
            CHECKMARKS = &H8
            DATA = &H20
            FTYPE = &H100
            ID = &H2
            STATE = &H1
            [STRING] = &H40
            SUBMENU = &H4
            TYPE = &H10
        End Enum

        <Flags()> _
        Public Enum SHCNE As Int32
            RENAMEITEM = &H1
            CREATE = &H2
            DELETE = &H4
            MKDIR = &H8
            RMDIR = &H10
            MEDIAINSERTED = &H20
            MEDIAREMOVED = &H40
            DRIVEREMOVED = &H80
            DRIVEADD = &H100
            NETSHARE = &H200
            NETUNSHARE = &H400
            ATTRIBUTES = &H800
            UPDATEDIR = &H1000
            UPDATEITEM = &H2000
            SERVERDISCONNECT = &H4000
            UPDATEIMAGE = &H8000
            DRIVEADDGUI = &H10000
            RENAMEFOLDER = &H20000
            FREESPACE = &H40000
            EXTENDED_EVENT = &H4000000
            ASSOCCHANGED = &H8000000
            DISKEVENTS = &H2381F
            GLOBALEVENTS = &HC0581E0
            ALLEVENTS = &H7FFFFFFF
            INTERRUPT = &H80000000
        End Enum

        Public Enum SHCNF
            IDLIST = &H0
            FLUSH = &H1000
        End Enum

        <Flags()> _
        Public Enum SHCNRF
            InterruptLevel = &H1
            ShellLevel = &H2
            RecursiveInterrupt = &H1000
            NewDelivery = &H8000
        End Enum

#End Region

#Region "   Drag/Drop Related Enums"

#Region "           CLIPFORMAT Enum"
        Public Enum CF
            TEXT = 1
            BITMAP = 2
            METAFILEPICT = 3
            SYLK = 4
            DIF = 5
            TIFF = 6
            OEMTEXT = 7
            DIB = 8
            PALETTE = 9
            PENDATA = 10
            RIFF = 11
            WAVE = 12
            UNICODETEXT = 13
            ENHMETAFILE = 14
            HDROP = 15
            LOCALE = 16
            MAX = 17
            OWNERDISPLAY = &H80
            DSPTEXT = &H81
            DSPBITMAP = &H82
            DSPMETAFILEPICT = &H83
            DSPENHMETAFILE = &H8E
            PRIVATEFIRST = &H200
            PRIVATELAST = &H2FF
            GDIOBJFIRST = &H300
            GDIOBJLAST = &H3FF
        End Enum
#End Region

#Region "           ADVF Enum"
        <Flags()> _
        Public Enum ADVF
            NODATA = 1
            PRIMEFIRST = 2
            ONLYONCE = 4
            DATAONSTOP = 64
            CACHE_NOHANDLER = 8
            CACHE_FORCEBUILTIN = 16
            CACHE_ONSAVE = 32
        End Enum
#End Region

#Region "           DVASPECT Enum"
        <Flags()> _
        Public Enum DVASPECT
            CONTENT = 1
            THUMBNAIL = 2
            ICON = 4
            DOCPRINT = 8
        End Enum
#End Region

#Region "           TYMED Enum"
        <Flags()> _
        Public Enum TYMED
            HGLOBAL = 1
            FILE = 2
            ISTREAM = 4
            ISTORAGE = 8
            GDI = 16
            MFPICT = 32
            ENHMF = 64
            NULL = 0
        End Enum
#End Region

#End Region

#Region "   Thumbnail Related Enums"
        <Flags()> _
        Public Enum IEIFLAG
            ASYNC = &H1     ' ask the extractor if it supports ASYNC extract (free threaded)
            CACHE = &H2      'returned from the extractor if it does NOT cache the thumbnail
            ASPECT = &H4      ' passed to the extractor to beg it to render to the aspect ratio of the supplied rect
            OFFLINE = &H8     ' if the extractor shouldn't hit the net to get any content needed for the rendering
            GLEAM = &H10     'does the image have a gleam ? this will be returned if it does
            SCREEN = &H20      ' render as if for the screen  (this is exlusive with IEIFLAG_ASPECT )
            ORIGSIZE = &H40      ' render to the approx size passed, but crop if neccessary
            NOSTAMP = &H80      ' returned from the extractor if it does NOT want an icon stamp on the thumbnail
            NOBORDER = &H100      'returned from the extractor if it does NOT want an a border around the thumbnail
            QUALITY = &H200      ' passed to the Extract method to indicate that a slower, higher quality image is desired, re-compute the thumbnail
        End Enum
#End Region

    End Class
End Namespace