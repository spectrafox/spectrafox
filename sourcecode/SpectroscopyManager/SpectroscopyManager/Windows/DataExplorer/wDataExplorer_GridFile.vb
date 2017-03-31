Imports System.ComponentModel

Public Class wDataExplorer_GridFile
    Inherits wFormBaseExpectsGridFileOnLoad

    Private bReady As Boolean = False

#Region "Properties"

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Window constructor.
    ''' </summary>
    Public Sub FormLoaded() Handles MyBase.Load

        ' Set Window Title
        Me.Text &= MyBase.FileObject.FileName

        Me.bReady = True

    End Sub

    ''' <summary>
    ''' Function that fills the interface with the GridFile information.
    ''' </summary>
    Public Sub GridFileFetched(ByRef GridFile As cGridFile) Handles MyBase.GridFileFetchedThreadSafeCall

        ' Set the content of the file selector.
        Me.spSpectraSelector.InitializeColumns(GridFile.SpectroscopyTables)

    End Sub

#End Region

#Region "Destructor"

    ''' <summary>
    ''' Function of the Form-Closing Event.
    ''' Abort the Closing, if the File-Buffer is fetching!
    ''' </summary>
    Private Sub CheckBeforeFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

    End Sub

#End Region

#Region "Spectra Plotting"

    ''' <summary>
    ''' Data to display in the preview box has changed. This is introduced to not plot
    ''' thousands of data in the preview box, but be able to selectively plot some spectra
    ''' for the data selection.
    ''' </summary>
    Private Sub spSpectraSelector_SelectionChanged() Handles spSpectraSelector.SelectedIndexChanged

        ' Save the last selection
        Dim LastSelectionX As String = Me.pbSpectrumViewer.cbX.SelectedColumnName
        Dim LastSelectionY As List(Of String) = Me.pbSpectrumViewer.cbY.SelectedColumnNames.ToArray.ToList

        ' Add the spectroscopy-tables to the list, after clearing the list so far.
        Me.pbSpectrumViewer.ClearSpectroscopyTables()

        ' Get the selected entries
        Dim SelectedEntries As List(Of String) = Me.spSpectraSelector.SelectedEntries

        Dim SelectedSpectroscopyTables As New List(Of cSpectroscopyTable)(SelectedEntries.Count)
        ' Plot the selected entries.
        For Each Name As String In SelectedEntries
            For i As Integer = 0 To Me.GridFile.SpectroscopyTables.Count - 1 Step 1
                If Me.GridFile.SpectroscopyTables(i).FileNameWithoutPath = Name Then
                    SelectedSpectroscopyTables.Add(Me.GridFile.SpectroscopyTables(i))
                    Exit For
                End If
            Next
        Next

        ' Plot the selected dables
        Me.bReady = False
        Me.pbSpectrumViewer.AddSpectroscopyTables(SelectedSpectroscopyTables)
        Me.pbSpectrumViewer.cbX.SelectedColumnName = LastSelectionX
        Me.pbSpectrumViewer.cbY.SelectedColumnNames = LastSelectionY
        Me.bReady = True

    End Sub

#End Region

End Class