Imports System.ComponentModel

Public Class wGridExporter
    Inherits wFormBaseExpectsGridFileOnLoad

    Private bReady As Boolean = False

#Region "Properties"

    ''' <summary>
    ''' Background-Worker for exporting the files.
    ''' </summary>
    Protected WithEvents _ExportWorker As New BackgroundWorker

    ''' <summary>
    ''' List of indices in the GridFile.SpectroscopyTable-list,
    ''' of the files that shall be exported.
    ''' </summary>
    Protected _IndicesSelectedForExport As New List(Of Integer)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Window constructor.
    ''' </summary>
    Public Sub FormLoaded() Handles MyBase.Load

        ' Setup the background-worker
        Me._ExportWorker.WorkerReportsProgress = True
        Me._ExportWorker.WorkerSupportsCancellation = True

        Me.bReady = True

    End Sub

    ''' <summary>
    ''' Function that fills the interface with the GridFile information.
    ''' </summary>
    Public Sub GridFileFetched(ByRef GridFile As cGridFile) Handles MyBase.GridFileFetchedThreadSafeCall

        ' Set the content of the file selector.
        Me.spExportSelector.InitializeColumns(GridFile.SpectroscopyTables)

    End Sub

#End Region

#Region "Destructor"

    ''' <summary>
    ''' Function of the Form-Closing Event.
    ''' Abort the Closing, if the File-Buffer is fetching!
    ''' </summary>
    Private Sub CheckBeforeFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me._ExportWorker.IsBusy Then
            MessageBox.Show(My.Resources.WindowClosing_FetchInProgress,
                            My.Resources.WindowClosing_FetchInProgress_title,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            e.Cancel = True
        End If
    End Sub

    ''' <summary>
    ''' Close the window.
    ''' </summary>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

#End Region


#Region "Export Worker"

    ''' <summary>
    ''' Start the export.
    ''' </summary>
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Me._ExportWorker.IsBusy Then Return

        Me.lblProgressBar.Text = "initializing ..."
        Me.pgProgressBar.Value = 0

        ' Extract the indices which should be exported
        ' Get the selected entries
        Dim SelectedEntries As List(Of String) = Me.spExportSelector.SelectedEntries

        Me._IndicesSelectedForExport.Clear()
        ' Plot the selected entries.
        For Each Name As String In SelectedEntries
            For i As Integer = 0 To Me.GridFile.SpectroscopyTables.Count - 1 Step 1
                If Me.GridFile.SpectroscopyTables(i).FileNameWithoutPath = Name Then
                    Me._IndicesSelectedForExport.Add(i)
                    Exit For
                End If
            Next
        Next

        ' Start the export
        Me._ExportWorker.RunWorkerAsync()
    End Sub

    ''' <summary>
    ''' Export BackgroundThread
    ''' </summary>
    Private Sub ExportSelectedFiles() Handles _ExportWorker.DoWork

        ' The exported files will be stored in the same folder as the grid file.
        Dim ExportFileNameTemplate As String = Me.GridFile.FullFileName & " # Experiment "

        ' Export each experiment.
        Dim Progress As Double = 0
        Dim ProgressProgress As Double = 99 / (Me._IndicesSelectedForExport.Count + 1)
        For Each i As Integer In Me._IndicesSelectedForExport

            ' Report progress
            Progress += ProgressProgress
            Me._ExportWorker.ReportProgress(CInt(Progress), Me.GridFile.SpectroscopyTables(i).FileNameWithoutPathAndExtension)

            ' Export
            cFileImportSpectraFoxSFD.WriteSpectraFoxSFDFile(
                ExportFileNameTemplate & i.ToString("N0"),
                Me.GridFile.SpectroscopyTables(i))

        Next

    End Sub

    ''' <summary>
    ''' Export progress reporter
    ''' </summary>
    Private Sub ExportProgress(sender As Object, e As ProgressChangedEventArgs) Handles _ExportWorker.ProgressChanged
        Dim Progress As Integer = 0
        If e.ProgressPercentage > 100 Then
            Progress = 100
        ElseIf e.ProgressPercentage < 0 Then
            Progress = 0
        Else
            Progress = e.ProgressPercentage
        End If
        Dim ReportString As String = "-"
        If TypeOf e.UserState Is String Then
            ReportString = CType(e.UserState, String)
        End If
        Me.lblProgressBar.Text = Progress.ToString("N1") & " % - " & ReportString
        Me.pgProgressBar.Value = Progress
    End Sub

    ''' <summary>
    ''' Export finished
    ''' </summary>
    Private Sub ExportFinished() Handles _ExportWorker.RunWorkerCompleted

        Me.lblProgressBar.Text = "-"
        Me.pgProgressBar.Value = 100



    End Sub

#End Region

End Class