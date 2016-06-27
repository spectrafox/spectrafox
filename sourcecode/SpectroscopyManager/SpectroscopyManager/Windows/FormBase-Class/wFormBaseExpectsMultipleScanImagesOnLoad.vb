''' <summary>
''' This class is an extension of the FormBase. It modifies the Show and ShowDialog
''' functions to expect multiple FileObjects to load during the opening of the window.
''' </summary>
Public Class wFormBaseExpectsMultipleScanImagesOnLoad
    Inherits wFormBase

    ''' <summary>
    ''' File objects to load and use by this window.
    ''' </summary>
    Protected _FileObjectList As List(Of cFileObject)

    ''' <summary>
    ''' File-Object list loaded in this window.
    ''' </summary>
    Public ReadOnly Property CurrentFileObjectList As List(Of cFileObject)
        Get
            Return Me._FileObjectList
        End Get
    End Property

    ''' <summary>
    ''' HardCopy of the initial File-Object-List, to be able to revert changes.
    ''' </summary>
    Protected _FileObjectCopyOfOriginal As New List(Of cFileObject)

    ''' <summary>
    ''' Original File-Object used for this window.
    ''' </summary>
    Public ReadOnly Property CurrentFileObjectCopyOfOriginal As List(Of cFileObject)
        Get
            Return Me._FileObjectCopyOfOriginal
        End Get
    End Property

    ''' <summary>
    ''' Loaded ScanImage-Objects
    ''' </summary>
    Protected _ScanImageList As New List(Of cScanImage)

    ''' <summary>
    ''' ScanImages used for this window.
    ''' </summary>
    Public ReadOnly Property CurrentScanImageList As List(Of cScanImage)
        Get
            Return Me._ScanImageList
        End Get
    End Property

    ' Progress-Panel
    Friend WithEvents panProgress As System.Windows.Forms.Panel
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents lblProgressHeader As System.Windows.Forms.Label
    Friend WithEvents pgbProgress As System.Windows.Forms.ProgressBar

    ''' <summary>
    ''' Object for Data Fetching of the Selected ScanImage Files
    ''' </summary>
    Protected WithEvents DataFetcher As cScanImageFetcher

    ''' <summary>
    ''' Event that gets raised if all files were successfully fetched!
    ''' </summary>
    Public Event AllFilesFetched()

#Region "Constructor and Close-Dialog Window"

    ''' <summary>
    ''' Constructor!
    ''' </summary>
    Public Sub New()
        Me.InitializeComponent()

        If Me.DesignMode Then
            ' Hide the loading panel in design mode:
            Me.panProgress.Visible = False
        End If
    End Sub

    Private FilesFetched As Boolean = False

    ''' <summary>
    ''' If the fetching is in progress, abort the closing of the window!
    ''' </summary>
    Public Sub FormCloseCatcher(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not FilesFetched Then
            MessageBox.Show(My.Resources.WindowClosing_FetchInProgress,
                            My.Resources.WindowClosing_FetchInProgress_title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Cancel = True
        End If
    End Sub


#End Region

#Region "Show and ShowDialog-Shadows"
    ''' <summary>
    ''' Sub shadowing the BaseClass to recieve a working directory
    ''' before opening!
    ''' 
    ''' Filters initially all ScanImage-objects.
    ''' </summary>
    Public Shadows Sub Show(ByRef WorkingFileObjectList As List(Of cFileObject))
        Me._FileObjectList = WorkingFileObjectList.Where(Function(obj) obj.FileType = cFileObject.FileTypes.ScanImage).ToList()

        ' Show the progress panel
        Me.panProgress.Visible = True
        Me.panProgress.BringToFront()

        If Me.FetchScanImageFiles() Then
            MyBase.Show()
        End If
    End Sub

    ''' <summary>
    ''' Sub shadowing the BaseClass to recieve a working directory
    ''' before opening!
    ''' 
    ''' Filters initially all ScanImage-objects.
    ''' </summary>
    Public Shadows Sub ShowDialog(ByRef WorkingFileObjectList As List(Of cFileObject))
        Me._FileObjectList = WorkingFileObjectList.Where(Function(obj) obj.FileType = cFileObject.FileTypes.ScanImage).ToList()

        ' Show the progress panel
        Me.panProgress.Visible = True
        Me.panProgress.BringToFront()

        If Me.FetchScanImageFiles() Then
            MyBase.ShowDialog()
        End If
    End Sub
#End Region

#Region "Initialization with ScanImage File-fetch"

    ''' <summary>
    ''' Tracks, which ScanImageFiles have been fetched already.
    ''' </summary>
    Private iScanImageLoadingCounter As Integer = 0

    ''' <summary>
    ''' Loads all ScanImages for the given FileObject and enables the interface afterwards.
    ''' </summary>
    Private Function FetchScanImageFiles() As Boolean

        ' Fetch all files one after another
        If iScanImageLoadingCounter < Me._FileObjectList.Count Then
            ' Fetch next file:
            '##################

            ' Show the progress
            Me.pgbProgress.Value = CInt(iScanImageLoadingCounter / Me._FileObjectList.Count * 100)
            Me.lblProgress.Text = My.Resources.rFormBaseExpectsFiles.LoadingScanImageFile _
                                                        .Replace("%p", Me.pgbProgress.Value.ToString("N0")) _
                                                        .Replace("%f", Me._FileObjectList(iScanImageLoadingCounter).FileNameWithoutPath)

            ' Create a hard-copy of the file-object for the backup storage
            Me._FileObjectCopyOfOriginal.Add(Me._FileObjectList(iScanImageLoadingCounter).GetCopy)

            ' Create new DataFetcher Object
            Me.DataFetcher = New cScanImageFetcher(Me._FileObjectList(iScanImageLoadingCounter))

            ' Load the ScanImage-File using Background-Class.
            Me.DataFetcher.FetchAsync()
        Else
            ' All files fetched!
            ' Call finalizer, and show the interface.
            '#########################################

            ' Activate the window, and hide the progress panel
            'Me.ShowHideLoadingPanel(False)
            Me.panProgress.Visible = False
            Me.panProgress.SendToBack()

            Me.FilesFetched = True

            RaiseEvent AllFilesFetched()
        End If


        Return True
    End Function

    ''' <summary>
    ''' Function that gets called, when the Background-Class finished loading a scan-image file.
    ''' </summary>
    Private Sub ScanImageFetched(ByRef ScanImage As cScanImage) Handles Me.ScanImageFetchedThreadSafeCall

        ' Save ScanImage
        Me._ScanImageList.Add(ScanImage)

        ' Fetch the next file:
        iScanImageLoadingCounter += 1
        Me.FetchScanImageFiles()

    End Sub

    ''' <summary>
    ''' Event that gets fired if the SpectroscopyTable got fetched!
    ''' </summary>
    Public Event ScanImageFetchedThreadSafeCall(ByRef ScanImage As cScanImage)

    Public Delegate Sub ThreadSafeScanImageDelegate(ByRef ScanImage As cScanImage)
    ''' <summary>
    ''' Call an Invoke to get a thread-save event fired!
    ''' </summary>
    Private Sub ScanImageFetched_RaiseThreadSafeEvent(ByRef ScanImage As cScanImage) Handles DataFetcher.FileFetchedComplete
        If Me.InvokeRequired Then
            Me.Invoke(New ThreadSafeScanImageDelegate(AddressOf Me.ScanImageFetched_RaiseThreadSafeEvent), ScanImage)
        Else
            RaiseEvent ScanImageFetchedThreadSafeCall(ScanImage)
        End If
    End Sub

#End Region


    Private Sub InitializeComponent()
        Me.panProgress = New ProgressPanel()
        Me.lblProgressHeader = New System.Windows.Forms.Label()
        Me.pgbProgress = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.panProgress.SuspendLayout()
        Me.SuspendLayout()
        '
        'panProgress
        '
        Me.panProgress.Controls.Add(Me.lblProgressHeader)
        Me.panProgress.Controls.Add(Me.pgbProgress)
        Me.panProgress.Controls.Add(Me.lblProgress)
        Me.panProgress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panProgress.Location = New System.Drawing.Point(0, 0)
        Me.panProgress.Name = "panProgress"
        Me.panProgress.Size = New System.Drawing.Size(467, 291)
        Me.panProgress.TabIndex = 0
        '
        'lblProgressHeader
        '
        Me.lblProgressHeader.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblProgressHeader.AutoSize = True
        Me.lblProgressHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgressHeader.Location = New System.Drawing.Point(7, 106)
        Me.lblProgressHeader.Name = "lblProgressHeader"
        Me.lblProgressHeader.Size = New System.Drawing.Size(275, 20)
        Me.lblProgressHeader.TabIndex = 2
        Me.lblProgressHeader.Text = "Loading image file ... please wait!"
        '
        'pgbProgress
        '
        Me.pgbProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbProgress.Location = New System.Drawing.Point(11, 129)
        Me.pgbProgress.Name = "pgbProgress"
        Me.pgbProgress.Size = New System.Drawing.Size(444, 23)
        Me.pgbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pgbProgress.TabIndex = 1
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(11, 160)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(228, 13)
        Me.lblProgress.TabIndex = 0
        Me.lblProgress.Text = "Waiting for CPU time to process the request ... "
        '
        'wFormBaseExpectsScanImageFileObjectOnLoad
        '
        Me.ClientSize = New System.Drawing.Size(467, 291)
        Me.Controls.Add(Me.panProgress)
        Me.Name = "wFormBaseExpectsScanImageFileObjectOnLoad"
        Me.panProgress.ResumeLayout(False)
        Me.panProgress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
End Class
