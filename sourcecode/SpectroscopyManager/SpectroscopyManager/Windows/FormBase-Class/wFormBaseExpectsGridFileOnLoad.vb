''' <summary>
''' This class is an extension of the FormBase. It modifies the Show and ShowDialog
''' functions to expect a FileObject to load during the opening of the window.
''' </summary>
Public Class wFormBaseExpectsGridFileOnLoad
    Inherits wFormBase

    ''' <summary>
    ''' File object to load and use by this window.
    ''' </summary>
    Protected FileObject As cFileObject

    ''' <summary>
    ''' HardCopy of the initial File-Object, to revert changes.
    ''' </summary>
    Protected FileObjectOriginal As cFileObject

    ''' <summary>
    ''' Loaded GridFile-Object
    ''' </summary>
    Protected GridFile As cGridFile

    ' Progress-Panel
    Friend WithEvents panProgress As System.Windows.Forms.Panel
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents lblProgressHeader As System.Windows.Forms.Label
    Friend WithEvents pgbProgress As System.Windows.Forms.ProgressBar

    ''' <summary>
    ''' Object for data fetching of the selected GridFiles
    ''' </summary>
    Protected WithEvents DataFetcher As cGridFileFetcher

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

    Private FileFetched As Boolean = False

    ''' <summary>
    ''' If the fetching is in progress, abort the closing of the window!
    ''' </summary>
    Public Sub FormCloseCatcher(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not FileFetched Then
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
    ''' </summary>
    Public Shadows Sub Show(ByRef WorkingFileObject As cFileObject)
        Me.FileObject = WorkingFileObject
        If Me.SetGridFile(Me.FileObject) Then
            MyBase.Show()
        End If
    End Sub

    ''' <summary>
    ''' Sub shadowing the BaseClass to recieve a working directory
    ''' before opening!
    ''' </summary>
    Public Shadows Sub ShowDialog(ByRef WorkingFileObject As cFileObject)
        Me.FileObject = WorkingFileObject

        If Me.SetGridFile(Me.FileObject) Then
            MyBase.ShowDialog()
        End If
    End Sub
#End Region

#Region "Initialization with GridFile File-fetch"
    ''' <summary>
    ''' Sets the used GridFile-Object and enables the Interface.
    ''' </summary>
    Private Function SetGridFile(ByRef GridFileObject As cFileObject) As Boolean
        ' Check, if the File-Object is a GridFile
        If GridFileObject.FileType <> cFileObject.FileTypes.GridFile Then Return False

        Me.FileObjectOriginal = GridFileObject.GetCopy

        ' Create new DataFetcher Object
        Me.DataFetcher = New cGridFileFetcher(GridFileObject)

        ' Show the progress
        Me.panProgress.Visible = True
        Me.lblProgressHeader.Text &= Me.FileObject.FileNameWithoutPath
        Me.pgbProgress.Value = 50
        Me.panProgress.BringToFront()

        ' Load the GridFile using the Background-Class.
        Me.DataFetcher.FetchAsync()

        Return True
    End Function

    ''' <summary>
    ''' Function that gets called, when the Background-Class finished loading a GridFile file.
    ''' </summary>
    Private Sub GridFileFetched(ByRef GridFile As cGridFile) Handles Me.GridFileFetchedThreadSafeCall

        ' Save the GridFile
        Me.GridFile = GridFile

        ' Activate the window, and hide the progress panel
        'Me.ShowHideLoadingPanel(False)
        Me.panProgress.Visible = False

        Me.FileFetched = True

    End Sub

    ''' <summary>
    ''' Event that gets fired if the GridFile got fetched!
    ''' </summary>
    Public Event GridFileFetchedThreadSafeCall(ByRef GridFile As cGridFile)

    Public Delegate Sub _ThreadSafeGridFileDelegate(ByRef GridFile As cGridFile)
    ''' <summary>
    ''' Call an Invoke to get a thread-save event fired!
    ''' </summary>
    Private Sub GridFileFetched_RaiseThreadSafeEvent(ByRef GridFile As cGridFile) Handles DataFetcher.FileFetchedComplete
        If Me.InvokeRequired Then
            Me.Invoke(New _ThreadSafeGridFileDelegate(AddressOf Me.GridFileFetched_RaiseThreadSafeEvent), GridFile)
        Else
            RaiseEvent GridFileFetchedThreadSafeCall(GridFile)
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

