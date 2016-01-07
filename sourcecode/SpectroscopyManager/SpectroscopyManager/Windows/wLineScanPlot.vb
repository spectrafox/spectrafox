Public Class wLineScanPlot
    Inherits wFormBaseExpectsMultipleSpectroscopyTableFileObjectsOnLoad

    ''' <summary>
    ''' Object SpectroscopyTables to Average.
    ''' </summary>
    Private ListOfSpectroscopyTables As New List(Of cSpectroscopyTable)

    ''' <summary>
    ''' If all files are loaded, set the input of the list-scan-viewer.
    ''' </summary>
    Public Sub SetSpectroscopyTables() Handles Me.AllFilesFetched
        Me.lvLineScanViewer.SetLineScanList(Me._SpectroscopyTableList)
    End Sub

    ''' <summary>
    ''' Close the window.
    ''' </summary>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class