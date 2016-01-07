Public Class wDataExplorer_ScanImage
    Inherits wFormBaseExpectsScanImageFileObjectOnLoad

#Region "Form Contructor"
    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub wDataExplorer_ScanImage_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set Window Title
        Me.Text &= MyBase.FileObject.FileName

    End Sub
#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the ScanImage-File to the display.
    ''' </summary>
    Public Sub ScanImageFetched(ByRef ScanImage As cScanImage) Handles MyBase.ScanImageFetchedThreadSafeCall
        Me.SetScanImageViewerThreadSafe(ScanImage)
        Me.SetSpectrumPropertiesThreadSafe(ScanImage)
        Me.SetSpectraCommentThreadSafe(ScanImage.Comment)
        Me.SetAdditionalCommentThreadSafe(MyBase.FileObject.ExtendedComment)

        Me.svScanImage.cbChannel.SelectedEntry = InitialChannelName
    End Sub

#End Region

#Region "Thread-Safe-Setters"
    Private Delegate Sub StringDelegate(Text As String)

    Public Sub SetScanImageViewerThreadSafe(ByRef ScanImage As cScanImage)
        With Me.svScanImage
            If .InvokeRequired Then
                .Invoke(New ThreadSafeScanImageDelegate(AddressOf SetScanImageViewerThreadSafe), ScanImage)
            Else
                .SetScanImageObjects(ScanImage)
            End If
        End With
    End Sub

    Public Sub SetSpectrumPropertiesThreadSafe(ByRef ScanImage As cScanImage)
        With Me.pgSpectrumProperies
            If .InvokeRequired Then
                .Invoke(New ThreadSafeScanImageDelegate(AddressOf SetSpectrumPropertiesThreadSafe), ScanImage)
            Else
                .SelectedObject = ScanImage
            End If
        End With
    End Sub

    Public Sub SetSpectraCommentThreadSafe(Text As String)
        With Me.txtSpectraComment
            If .InvokeRequired Then
                .Invoke(New StringDelegate(AddressOf SetSpectraCommentThreadSafe), Text)
            Else
                .Text = Text
            End If
        End With
    End Sub

    Public Sub SetAdditionalCommentThreadSafe(Text As String)
        With Me.txtAdditionalComment
            If .InvokeRequired Then
                .Invoke(New StringDelegate(AddressOf SetAdditionalCommentThreadSafe), Text)
            Else
                .Text = Text
            End If
        End With
    End Sub
#End Region

#Region "Selected Channel"

    Private InitialChannelName As String

    ''' <summary>
    ''' Sets the initial channel-name that get displayed in the preview-image.
    ''' </summary>
    Public Sub SetInitialChannelSelection(ByVal ChannelName As String)
        Me.InitialChannelName = ChannelName
    End Sub

#End Region

#Region "Save additional comment"

    ''' <summary>
    ''' A click saves the additional comment from the textbox.
    ''' </summary>
    Private Sub btnSaveAdditionalComment_Click(sender As Object, e As EventArgs) Handles btnSaveAdditionalComment.Click
        MyBase.FileObject.SetExtendedComment(Me.txtAdditionalComment.Text)
    End Sub

#End Region

End Class