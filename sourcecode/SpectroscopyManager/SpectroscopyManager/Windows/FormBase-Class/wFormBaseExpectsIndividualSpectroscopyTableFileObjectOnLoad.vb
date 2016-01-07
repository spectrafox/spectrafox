Imports System.ComponentModel

''' <summary>
''' This class is an extension of the FormBase. It modifies the Show and ShowDialog
''' functions to expect a FileObject to load during the opening of the window.
''' </summary>
Public Class wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad
    Inherits wFormBase

    ''' <summary>
    ''' File object to load and use by this window.
    ''' </summary>
    Protected _FileObject As cFileObject

    ''' <summary>
    ''' File-Object used for this window.
    ''' </summary>
    Public ReadOnly Property CurrentFileObject As cFileObject
        Get
            Return Me._FileObject
        End Get
    End Property

    ''' <summary>
    ''' HardCopy of the initial File-Object, to revert changes.
    ''' </summary>
    Protected _FileObjectCopyOfOriginal As cFileObject

    ''' <summary>
    ''' Original File-Object used for this window.
    ''' </summary>
    Public ReadOnly Property CurrentFileObjectCopyOfOriginal As cFileObject
        Get
            Return Me._FileObjectCopyOfOriginal
        End Get
    End Property

    ''' <summary>
    ''' Loaded SpectroscopyTable-Object
    ''' </summary>
    Protected SpectroscopyTable As cSpectroscopyTable

    ''' <summary>
    ''' SpectroscopyTable used for this window.
    ''' </summary>
    Public ReadOnly Property CurrentSpectroscopyTable As cSpectroscopyTable
        Get
            Return Me.SpectroscopyTable
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

#Region "Constructor and Close-Dialog Window"

    ''' <summary>
    ''' Constructor!
    ''' </summary>
    Public Sub New()
        Me.InitializeComponent()

        If Not Me.DesignMode Then
            ' Hide the loading panel in design mode:
            Me.panProgress.Dock = DockStyle.Fill
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
        Me._FileObject = WorkingFileObject
        If Me.SetSpectroscopyTableFile(Me._FileObject) Then
            MyBase.Show()
        End If
    End Sub

    ''' <summary>
    ''' Sub shadowing the BaseClass to recieve a working directory
    ''' before opening!
    ''' </summary>
    Public Shadows Sub ShowDialog(ByRef WorkingFileObject As cFileObject)
        Me._FileObject = WorkingFileObject

        If Me.SetSpectroscopyTableFile(Me._FileObject) Then
            MyBase.ShowDialog()
        End If
    End Sub
#End Region

#Region "Initialization with SpectroscopyTable File-fetch"
    ''' <summary>
    ''' Sets the used Spectroscopy-Table-FileObject and enables the Interface.
    ''' </summary>
    Private Function SetSpectroscopyTableFile(ByRef SpectroscopyTableFileObject As cFileObject) As Boolean
        ' Check, if the File-Object is a spectroscopy table
        If SpectroscopyTableFileObject.FileType <> cFileObject.FileTypes.SpectroscopyTable Then Return False

        Me._FileObjectCopyOfOriginal = SpectroscopyTableFileObject.GetCopy

        ' Create new DataFetcher Object
        Me.DataFetcher = New cSpectroscopyTableFetcher(SpectroscopyTableFileObject)

        ' Show the progress
        Me.panProgress.Visible = True
        Me.lblProgressHeader.Text &= Me._FileObject.FileNameWithoutPath
        Me.pgbProgress.Value = 50
        Me.panProgress.BringToFront()

        ' Load the Spectroscopy-Table-File using Background-Class.
        Me.DataFetcher.FetchAsync()

        Return True
    End Function

    ''' <summary>
    ''' Function that gets called, when the Background-Class finished loading a spectroscopy file.
    ''' </summary>
    Private Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles Me.SpectroscopyTableFetchedThreadSafeCall

        ' Save SpectroscopyTable
        Me.SpectroscopyTable = SpectroscopyTable

        ' Activate the window, and hide the progress panel
        'Me.ShowHideLoadingPanel(False)
        Me.panProgress.Visible = False
        Me.panProgress.SendToBack()

        Me.FileFetched = True
    End Sub

    ''' <summary>
    ''' Event that gets fired if the SpectroscopyTable got fetched!
    ''' </summary>
    Public Event SpectroscopyTableFetchedThreadSafeCall(ByRef SpectroscopyTable As cSpectroscopyTable)

    Public Delegate Sub _ThreadSafeSpectroscopyTableDelegate(ByRef SpectroscopyTable As cSpectroscopyTable)
    ''' <summary>
    ''' Call an Invoke to get a thread-save event fired!
    ''' </summary>
    Private Sub SpectroscopyTableFetched_RaiseThreadSafeEvent(ByRef SpectroscopyTable As cSpectroscopyTable) Handles DataFetcher.FileFetchedComplete
        If Me.InvokeRequired Then
            Me.Invoke(New _ThreadSafeSpectroscopyTableDelegate(AddressOf Me.SpectroscopyTableFetched_RaiseThreadSafeEvent), SpectroscopyTable)
        Else
            RaiseEvent SpectroscopyTableFetchedThreadSafeCall(SpectroscopyTable)
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
        Me.panProgress.Location = New System.Drawing.Point(0, 0)
        Me.panProgress.Name = "panProgress"
        Me.panProgress.Size = New System.Drawing.Size(455, 89)
        Me.panProgress.TabIndex = 0
        '
        'lblProgressHeader
        '
        Me.lblProgressHeader.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblProgressHeader.AutoSize = True
        Me.lblProgressHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgressHeader.Location = New System.Drawing.Point(7, 5)
        Me.lblProgressHeader.Name = "lblProgressHeader"
        Me.lblProgressHeader.Size = New System.Drawing.Size(272, 20)
        Me.lblProgressHeader.TabIndex = 2
        Me.lblProgressHeader.Text = "Loading data files ... please wait!"
        '
        'pgbProgress
        '
        Me.pgbProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgbProgress.Location = New System.Drawing.Point(11, 28)
        Me.pgbProgress.Name = "pgbProgress"
        Me.pgbProgress.Size = New System.Drawing.Size(432, 23)
        Me.pgbProgress.TabIndex = 1
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(11, 59)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(228, 13)
        Me.lblProgress.TabIndex = 0
        Me.lblProgress.Text = "Waiting for CPU time to process the request ... "
        '
        'wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad
        '
        Me.ClientSize = New System.Drawing.Size(467, 291)
        Me.Controls.Add(Me.panProgress)
        Me.Name = "wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad"
        Me.panProgress.ResumeLayout(False)
        Me.panProgress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
End Class
