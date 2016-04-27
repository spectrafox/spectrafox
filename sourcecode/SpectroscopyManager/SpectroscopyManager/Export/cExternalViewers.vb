Public Class cExternalViewers

#Region "Structure to store a viewer"

    ''' <summary>
    ''' Structure representing the settings to open a file in a separate viewer.
    ''' </summary>
    Public Structure ExternalViewer

        ''' <summary>
        ''' Identifier that gets replaced by the filename.
        ''' </summary>
        Public Const FileNameReplacement As String = "%f"

        ''' <summary>
        ''' Display name in the software.
        ''' </summary>
        Public DisplayName As String

        ''' <summary>
        ''' Path to the software.
        ''' </summary>
        Public Path As String

        ''' <summary>
        ''' Arguments to start the software.
        ''' </summary>
        Public Arguments As String

        ''' <summary>
        ''' Launches the given file with the selected viewer.
        ''' </summary>
        Public Sub LaunchViewer(ByVal FileName As String)
            Dim Arguments As String = Me.Arguments.Replace(FileNameReplacement, FileName)
            Try
                Process.Start(Me.Path, Arguments)
            Catch ex As Exception
                MessageBox.Show(My.Resources.rExport.ExternalViewer_CouldNotStart.Replace("%e", ex.Message).Replace("%c", Command),
                                My.Resources.rExport.ExternalViewer_CouldNotStart_Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Structure

#End Region

#Region "Handling of the list."

    ''' <summary>
    ''' List to store the external viewers.
    ''' </summary>
    Private _ExternalViewers As New List(Of ExternalViewer)

    ''' <summary>
    ''' List containing references to all external viewers.
    ''' </summary>
    Public ReadOnly Property ExternalViewers As ReadOnlyCollection(Of ExternalViewer)
        Get
            Return New ReadOnlyCollection(Of ExternalViewer)(Me._ExternalViewers)
        End Get
    End Property

    ''' <summary>
    ''' Saves the given list.
    ''' </summary>
    Public Sub SaveList(ViewerList As List(Of ExternalViewer))

        ' Check, if the settings list is not nothing.
        If My.Settings.ExternalViewers_PathList Is Nothing Then My.Settings.ExternalViewers_PathList = New System.Collections.Specialized.StringCollection

        ' Clear the list.
        My.Settings.ExternalViewers_PathList.Clear()
        For Each V As ExternalViewer In ViewerList
            My.Settings.ExternalViewers_PathList.Add(Me.GetStorageString(V))
        Next

        ' Save the settings.
        cGlobal.SaveSettings()

    End Sub

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor loads the current settings.
    ''' </summary>
    Public Sub New()

        ' Check, if the settings list is not nothing.
        If My.Settings.ExternalViewers_PathList Is Nothing Then My.Settings.ExternalViewers_PathList = New System.Collections.Specialized.StringCollection

        ' Clear the list.
        For Each SS As String In My.Settings.ExternalViewers_PathList
            Me._ExternalViewers.Add(Me.GetViewerSettingsFromString(SS))
        Next

    End Sub

#End Region

#Region "Storage handling"

    ''' <summary>
    ''' Storage separator to store the settings in a single string.
    ''' </summary>
    Private Const StorageSeparator As Char = CChar(">")

    ''' <summary>
    ''' Returns the string to store the settings of an external viewer.
    ''' </summary>
    Private Function GetStorageString(Viewer As ExternalViewer) As String
        Return Viewer.DisplayName & StorageSeparator & Viewer.Path & StorageSeparator & Viewer.Arguments
    End Function

    ''' <summary>
    ''' Returns the viewer settings from settings string.
    ''' </summary>
    Private Function GetViewerSettingsFromString(SettingsString As String) As ExternalViewer

        Dim V As New ExternalViewer

        ' Get the settings
        Dim SplittedSettingsString As String() = SettingsString.Split(StorageSeparator)
        If SplittedSettingsString.Length = 3 Then
            V.DisplayName = SplittedSettingsString(0)
            V.Path = SplittedSettingsString(1)
            V.Arguments = SplittedSettingsString(2)
        End If

        Return V
    End Function

#End Region

End Class
