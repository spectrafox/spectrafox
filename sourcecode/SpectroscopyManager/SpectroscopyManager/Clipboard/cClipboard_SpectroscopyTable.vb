Public Class cClipboard_SpectroscopyTable
    Implements iSingleSpectroscopyTableLoaded

    ''' <summary>
    ''' Event to signalize the calling window to collect the clip-board
    ''' content and copy it the the actual clipboard.
    ''' </summary>
    Public Event ClipboardContentReady(ByVal ClipBoardContent As String)

    ''' <summary>
    ''' Saves the export-method to use to convert the spectroscopy-table to an Ascii-string.
    ''' </summary>
    Public Property ExportMethod As iExportMethod_Ascii

    ''' <summary>
    ''' Set the clip-board-content with the ascii gained by using the certain export-method.
    ''' </summary>
    Public Sub SpectroscopyTableLoaded(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded
        If ExportMethod Is Nothing Then Return

        RaiseEvent ClipboardContentReady(ExportMethod.GetExportOutput(SpectroscopyTable))
    End Sub
End Class
