Public Class wExportWizard
    Inherits wFormBase

#Region "Properties"
    ''' <summary>
    ''' File Exporter Object
    ''' </summary>
    ''' <remarks></remarks>
    Private WithEvents Exporter As cFileExportSpectroscopyTable

    Private bReady As Boolean = False
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor of the Window.
    ''' Fill Combobox.
    ''' </summary>
    Private Sub wExportWizard_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.pbProgress.Visible = False
        Me.lblProgress.Visible = False

        Me.etExportFolder.Refresh()

        ' Fill the Combobox.
        With Me.cbExportType
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Origin compatible file (.txt)", cFileExport.FileExportTypes.Origin))
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Mathematica Notebook (.nb)", cFileExport.FileExportTypes.MathematicaList))
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Excel CSV, local region settings (.csv)", cFileExport.FileExportTypes.CSV_current))
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Excel CSV, english format (.csv)", cFileExport.FileExportTypes.CSV_english))
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Tab separated ASCII, exponential, local region settings (.txt)", cFileExport.FileExportTypes.Ascii_current_culture))
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Tab separated ASCII, *10^, local region settings (.txt)", cFileExport.FileExportTypes.Ascii_current_culture_10))
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Tab separated ASCII, exponential, english format (.txt)", cFileExport.FileExportTypes.Ascii_english))
            .Items.Add(New KeyValuePair(Of String, cFileExport.FileExportTypes)("Tab separated ASCII, *10^, english format (.txt)", cFileExport.FileExportTypes.Ascii_english_10))
            .ValueMember = "Value"
            .DisplayMember = "Key"
            .SelectedIndex = 0
        End With

        ' Load StartupPath
        Dim ExportPath As String = My.Settings.LastExport_Path

        ' Try Several alternative Paths, if the Path from the setting does not exist!
        If Not IO.Directory.Exists(ExportPath) Then
            ExportPath = My.Settings.LastSelectedPath
        End If
        If Not IO.Directory.Exists(ExportPath) Then
            ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        End If
        If Not IO.Directory.Exists(ExportPath) Then
            ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
        End If
        If Not IO.Directory.Exists(ExportPath) Then
            ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
        If Not IO.Directory.Exists(ExportPath) Then
            ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        End If
        If Not IO.Directory.Exists(ExportPath) Then
            ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
        End If

        Dim ExportPathItem As ExpTreeLib.CShItem = ExpTreeLib.CShItem.GetCShItem(ExportPath)
        If Not ExportPathItem Is Nothing Then
            Me.etExportFolder.ExpandANode(ExportPathItem, True)
        End If
    End Sub

    ''' <summary>
    ''' Sets the list of spectroscopy-filenames to export.
    ''' </summary>
    Public Sub SetExportSpectroscopyTableNames(ByVal FileNameList As Dictionary(Of String, cFileImport.FileObject))
        Me.bReady = True

        Me.Exporter = New cFileExportSpectroscopyTable(FileNameList)

        Me.lbExportFiles.Items.Clear()
        For Each FileObject As cFileImport.FileObject In FileNameList.Values
            Me.lbExportFiles.Items.Add(FileObject.FileName)
        Next

        Me.bReady = True
    End Sub
#End Region

#Region "Export Functions"
    ''' <summary>
    ''' Initialize the Export by Clicking the Export-Button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnStartExport_Click(sender As System.Object, e As System.EventArgs) Handles btnStartExport.Click
        Me.pbProgress.Visible = True
        Me.lblProgress.Visible = True

        ' Launch the Exporter Class
        Me.Exporter.StartExport(Me.etExportFolder.SelectedItem.Path, Me.GetSelectedExportType)

        ' Save last export path
        If My.Settings.LastExport_Path <> Me.etExportFolder.SelectedItem.Path Then
            My.Settings.LastExport_Path = Me.etExportFolder.SelectedItem.Path
            My.Settings.LastExport_Format = Me.GetSelectedExportType
            My.Settings.Save()
        End If
    End Sub

    ''' <summary>
    ''' Function, that updates the status, if a file gets exported successfully.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AllFilesExportedSuccessfully() Handles Exporter.AllFileExportedSuccessfully
        Me.UIRefreshProgress(1, 1)
        MessageBox.Show("Export finished!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' This will be called from the worker thread to set
    ' the work's progress
    Delegate Sub SetUIRefreshProgressCallback(ByVal MaxVal As Integer, CurrentVal As Integer)
    Friend Sub UIRefreshProgress(MaxVal As Integer, CurrentVal As Integer)
        If Me.pbProgress.InvokeRequired Then
            Dim _delegate As New SetUIRefreshProgressCallback(AddressOf UIRefreshProgress)
            Me.Invoke(_delegate, MaxVal, CurrentVal)
        Else
            If CurrentVal >= MaxVal Then
                Me.pbProgress.Visible = False
                Me.lblProgress.Visible = False
            Else
                ' Display the % work done
                Me.lblProgress.Text = String.Format("({0}|{1}) files exported ...")
                If MaxVal > 0 Then
                    Me.pbProgress.Value = CInt(Math.Truncate(CurrentVal / MaxVal * 100))
                End If
            End If
        End If
    End Sub
#End Region

#Region "Window Functions"
    ''' <summary>
    ''' Determines the wanted Export-Type.
    ''' </summary>
    Private Function GetSelectedExportType() As cFileExport.FileExportTypes
        Return DirectCast(Me.cbExportType.SelectedItem, KeyValuePair(Of String, cFileExport.FileExportTypes)).Value
    End Function

    ''' <summary>
    ''' Export-Type changed -> adapt Description
    ''' </summary>
    Private Sub cbExportType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbExportType.SelectedIndexChanged
        Me.txtExportFormatDesciption.Text = cFileExport.GetExportTypeDescription(Me.GetSelectedExportType)
    End Sub

    ''' <summary>
    ''' Selects the current working directory in the Folder-Selector.
    ''' </summary>
    Private Sub btnGoToCurrentWorkingDirectory_Click(sender As System.Object, e As System.EventArgs) Handles btnGoToCurrentWorkingDirectory.Click
        If My.Settings.LastSelectedPath.Trim <> "" And IO.Directory.Exists(My.Settings.LastSelectedPath) Then
            Me.etExportFolder.ExpandANode(ExpTreeLib.CShItem.GetCShItem(My.Settings.LastSelectedPath))
        End If
    End Sub

    ''' <summary>
    ''' CLose Window
    ''' </summary>
    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
#End Region


End Class