Imports System.ComponentModel

''' <summary>
''' This class is an extension of the FormBase. It modifies the Show and ShowDialog
''' functions to expect a FileObject to load during the opening of the window.
''' </summary>
Public Class wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad
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
    ''' Loaded SpectroscopyTable-Objects
    ''' </summary>
    Protected _SpectroscopyTableList As New List(Of cSpectroscopyTable)

    ''' <summary>
    ''' SpectroscopyTable used for this window.
    ''' </summary>
    Public ReadOnly Property CurrentSpectroscopyTableList As List(Of cSpectroscopyTable)
        Get
            Return Me._SpectroscopyTableList
        End Get
    End Property

    ' Progress-Panel
    Friend WithEvents panProgress As System.Windows.Forms.Panel
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents lblProgressHeader As System.Windows.Forms.Label
    Friend WithEvents pgbProgress As System.Windows.Forms.ProgressBar

    ''' <summary>
    ''' Object for Data Fetching of the Selected Spectroscopy Files
    ''' </summary>
    Protected WithEvents DataFetcher As cSpectroscopyTableFetcher

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
    ''' </summary>
    Public Shadows Sub Show(ByRef WorkingFileObjectList As List(Of cFileObject))
        Me._FileObjectList = WorkingFileObjectList.Where(Function(obj) obj.FileType = cFileObject.FileTypes.SpectroscopyTable).ToList()

        ' Show the progress panel
        Me.panProgress.Visible = True
        Me.panProgress.BringToFront()

        If Me.FetchSpectroscopyTableFiles() Then
            MyBase.Show()
        End If
    End Sub

    ''' <summary>
    ''' Sub shadowing the BaseClass to recieve a working directory
    ''' before opening!
    ''' Filters initially all spectroscopy-table file-objects.
    ''' </summary>
    Public Shadows Sub ShowDialog(ByRef WorkingFileObjectList As List(Of cFileObject))
        Me._FileObjectList = WorkingFileObjectList.Where(Function(obj) obj.FileType = cFileObject.FileTypes.SpectroscopyTable).ToList()

        ' Show the progress panel
        Me.panProgress.Visible = True
        Me.panProgress.BringToFront()

        If Me.FetchSpectroscopyTableFiles() Then
            MyBase.ShowDialog()
        End If
    End Sub
#End Region

#Region "Initialization with SpectroscopyTable File-fetch for each file-object."

    ''' <summary>
    ''' Tracks, which spectroscopy-table has been fetched already.
    ''' </summary>
    Private iSpectroscopyTableLoadingCounter As Integer = 0

    ''' <summary>
    ''' Loads all spectroscopytable-objects for the given FileObject and enables the interface afterwards.
    ''' </summary>
    Private Function FetchSpectroscopyTableFiles() As Boolean

        ' Fetch all files one after another
        If iSpectroscopyTableLoadingCounter < Me._FileObjectList.Count Then
            ' Fetch next file:
            '##################

            ' Show the progress
            Me.pgbProgress.Value = CInt(iSpectroscopyTableLoadingCounter / Me._FileObjectList.Count * 100)
            Me.lblProgress.Text = My.Resources.rFormBaseExpectsFiles.LoadingSpectroscopyFile _
                                                        .Replace("%p", Me.pgbProgress.Value.ToString("N0")) _
                                                        .Replace("%f", Me._FileObjectList(iSpectroscopyTableLoadingCounter).FileNameWithoutPath)

            ' Create a hard-copy of the file-object for the backup storage
            Me._FileObjectCopyOfOriginal.Add(Me._FileObjectList(iSpectroscopyTableLoadingCounter).GetCopy)

            ' Create new DataFetcher Object
            Me.DataFetcher = New cSpectroscopyTableFetcher(Me._FileObjectList(iSpectroscopyTableLoadingCounter))

            ' Load the Spectroscopy-Table-File using Background-Class.
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
    ''' Function that gets called, when the Background-Class finished loading a spectroscopy file.
    ''' </summary>
    Private Sub SpectroscopyTableFetched(sender As Object, e As EventArgs)
        Dim SpectroscopyTable As cSpectroscopyTable = DirectCast(sender, cSpectroscopyTable)

        ' Save the fetched SpectroscopyTable
        Me._SpectroscopyTableList.Add(SpectroscopyTable)

        ' Fetch the next file:
        iSpectroscopyTableLoadingCounter += 1
        Me.FetchSpectroscopyTableFiles()
    End Sub

    ''' <summary>
    ''' Event that gets fired if the SpectroscopyTable got fetched!
    ''' </summary>
    Private eSpectroscopyTableFetchedThreadSafeCall As New EventHandler(AddressOf SpectroscopyTableFetched)

    ''' <summary>
    ''' Call an Invoke to get a thread-save event fired!
    ''' </summary>
    Private Sub SpectroscopyTableFetched_RaiseThreadSafeEvent(ByRef SpectroscopyTable As cSpectroscopyTable) Handles DataFetcher.FileFetchedComplete
        Me.eSpectroscopyTableFetchedThreadSafeCall.RaiseEventAndExecuteItInAnExplicitOrUIThread({SpectroscopyTable}, Nothing)
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
        Me.lblProgressHeader.Size = New System.Drawing.Size(263, 20)
        Me.lblProgressHeader.TabIndex = 2
        Me.lblProgressHeader.Text = "Loading data file ... please wait!"
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
        'wFormBaseExpectsSpectroscopyTableFileObjectOnLoad
        '
        Me.ClientSize = New System.Drawing.Size(467, 291)
        Me.Controls.Add(Me.panProgress)
        Me.Name = "wFormBaseExpectsSpectroscopyTableFileObjectOnLoad"
        Me.panProgress.ResumeLayout(False)
        Me.panProgress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
End Class
