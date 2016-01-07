Public Class wExportWizard_SpectroscopyTable
    Inherits wFormBase

#Region "Properties"
    ''' <summary>
    ''' File Exporter Object
    ''' </summary>
    Private WithEvents Exporter As cExportSpectroscopyTable

    Private bReady As Boolean = False

    ''' <summary>
    ''' Container to save all possible export-methods.
    ''' </summary>
    Private ExportMethods As New List(Of iExportMethod_Ascii)

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

        ' Add export-methods to the selection
        ExportMethods.Add(New cExportMethod_Ascii_ORIGIN)
        ExportMethods.Add(New cExportMethod_Mathematica)
        ExportMethods.Add(New cExportMethod_Ascii_CSV)
        ExportMethods.Add(New cExportMethod_Ascii_TAB)

        ' Fill the Combobox with these types
        With Me.cbExportType
            Dim NewIndex As Integer = 0
            For Each ExportMethod As iExportMethod_Ascii In Me.ExportMethods
                NewIndex = .Items.Add(New KeyValuePair(Of iExportMethod_Ascii, String)(ExportMethod, ExportMethod.ExportName))
                If ExportMethod.ExportName = My.Settings.LastExport_Format Then
                    .SelectedIndex = NewIndex
                End If
            Next
            .ValueMember = "Key"
            .DisplayMember = "Value"
            If .Items.Count > 0 And .SelectedIndex <= 0 Then
                .SelectedIndex = 0
            End If
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
    Public Sub SetExportSpectroscopyTableNames(ByVal FileNameList As Dictionary(Of String, cFileObject))
        Me.bReady = False

        Me.Exporter = New cExportSpectroscopyTable(FileNameList)

        Me.lbExportFiles.Items.Clear()
        For Each FileObject As cFileObject In FileNameList.Values
            Me.lbExportFiles.Items.Add(FileObject.FileName)
        Next

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Sets the list of spectroscopy-filenames to export.
    ''' </summary>
    Public Sub SetExportSpectroscopyTableNames(ByVal FileList As List(Of cFileObject))
        Dim FileNameList As New Dictionary(Of String, cFileObject)(FileList.Count)
        For i As Integer = 0 To FileList.Count - 1 Step 1
            FileNameList.Add(FileList(i).FileName, FileList(i))
        Next
        Me.SetExportSpectroscopyTableNames(FileNameList)
    End Sub

    ''' <summary>
    ''' Sets the list of spectroscopy-filenames to export.
    ''' </summary>
    Public Sub SetExportSpectroscopyTableNames(ByVal FileObject As cFileObject)
        Dim FileNameList As New Dictionary(Of String, cFileObject)(1)
        FileNameList.Add(FileObject.FileName, FileObject)
        Me.SetExportSpectroscopyTableNames(FileNameList)
    End Sub
#End Region

#Region "Export Functions"
    ''' <summary>
    ''' Initialize the Export by Clicking the Export-Button.
    ''' </summary>
    Private Sub btnStartExport_Click(sender As System.Object, e As System.EventArgs) Handles btnStartExport.Click
        Me.pbProgress.Visible = True
        Me.lblProgress.Visible = True
        Me.btnStartExport.Enabled = False

        Dim ExportFunctionToUse As iExportMethod_Ascii = Me.GetSelectedExportFunction
        Dim ExportSetting As New cExportMethod_Ascii.Ascii_FormatSetting(Me.rbFormatInvariant.Checked, Me.rbExponents_10.Checked)

        ' Save the export-settings to the export-function
        ExportFunctionToUse.SetCustomFormatSettings(ExportSetting)

        ' Launch the Exporter Class
        Me.Exporter.StartExport(Me.etExportFolder.SelectedItem.Path, ExportFunctionToUse)

        ' Save last export path
        If My.Settings.LastExport_Path <> Me.etExportFolder.SelectedItem.Path Then
            My.Settings.LastExport_Path = Me.etExportFolder.SelectedItem.Path
            My.Settings.LastExport_Format = Me.GetSelectedExportFunction.ExportName
            My.Settings.Save()
        End If
    End Sub


    Delegate Sub _SetExportButtonenableStatus(Enabled As Boolean)
    ''' <summary>
    ''' Enables or disables the export-button thread-safe.
    ''' </summary>
    Friend Sub SetExportButtonEnableStatus(Enabled As Boolean)
        If Me.pbProgress.InvokeRequired Then
            Dim _delegate As New _SetExportButtonenableStatus(AddressOf SetExportButtonEnableStatus)
            Me.Invoke(_delegate, Enabled)
        Else
            Me.btnStartExport.Enabled = Enabled
        End If
    End Sub

    ''' <summary>
    ''' Function, that updates the status, if a file gets exported successfully.
    ''' </summary>
    Private Sub AllFilesExportedSuccessfully() Handles Exporter.AllFilesExportedSuccessfully
        'MessageBox.Show("Export finished!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        SetExportButtonEnableStatus(True)
    End Sub

    ' This will be called from the worker thread to set
    ' the work's progress
    Delegate Sub SetUIRefreshProgressCallback(CurrentFileObject As cFileObject,
                                              TargetFileName As String,
                                              MaxVal As Integer,
                                              CurrentVal As Integer)
    ''' <summary>
    ''' Progress reporting ...
    ''' </summary>
    Friend Sub UIRefreshProgress(CurrentFileObject As cFileObject,
                                 TargetFileName As String,
                                 MaxVal As Integer,
                                 CurrentVal As Integer) Handles Exporter.FileExportedSuccessfully

        If Me.pbProgress.InvokeRequired Then
            Dim _delegate As New SetUIRefreshProgressCallback(AddressOf UIRefreshProgress)
            Me.Invoke(_delegate, CurrentFileObject,
                                 TargetFileName,
                                 MaxVal,
                                 CurrentVal)
        Else
            If CurrentVal >= MaxVal Then
                Me.pbProgress.Visible = False
                Me.lblProgress.Visible = False
            Else
                ' Display the % work done
                Me.lblProgress.Text = "(" & CurrentVal.ToString & "|" & MaxVal.ToString & ") " & CurrentFileObject.FileNameWithoutPath & " exported successfully to " & TargetFileName
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
    Private Function GetSelectedExportFunction() As iExportMethod_Ascii
        Return DirectCast(Me.cbExportType.SelectedItem, KeyValuePair(Of iExportMethod_Ascii, String)).Key
    End Function

    ''' <summary>
    ''' Export-Type changed -> adapt Description and settings
    ''' </summary>
    Private Sub cbExportType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbExportType.SelectedIndexChanged

        ' Description
        Me.txtExportFormatDesciption.Text = Me.GetSelectedExportFunction.ExportDescription

        ' File-Extension
        Me.lblFileExtension.Text = Me.GetSelectedExportFunction.FileExtension

        ' Settings
        If Me.GetSelectedExportFunction.CustomFormatSettingsAllowed Then
            Me.grpExp.Enabled = True
            Me.grpFormat.Enabled = True
        Else
            Me.grpExp.Enabled = False
            Me.grpFormat.Enabled = False
        End If

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