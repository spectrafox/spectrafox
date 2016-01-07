Imports System.IO
Imports ZedGraph
Imports System.Threading.Tasks


Public Class wSXMTest
    Inherits Form

    Public ScanImage As cScanImage
    Dim sr As cContourPlot

    'Private Sub button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    sr = New cContourPlot(Me.ScanImage.ScanChannels(0).ScanData)
    '    sr.ColorScheme = cColorScheme.Autumn
    '    ResizeRedraw = True
    '    DoubleBuffered = True
    '    Me.Repaint()
    'End Sub

    'Private Sub Repaint()
    '    Me.Refresh()
    '    sr.RecalculateTransformationsCoefficientsFor3D(0, 0, 0, 0, 0, 300, 300, 1, 0, 0)
    '    sr.Plot3D(Me.ScanImage.ScanChannels(0).GetMaximumValue(),
    '              Me.ScanImage.ScanChannels(0).GetMinimumValue(), Me.ScanImage.ScanRange_X, Me.ScanImage.ScanRange_Y)
    '    Me.BackgroundImage = sr.Image
    'End Sub


    'Private Sub TrackBar1_Scroll(sender As System.Object, e As System.EventArgs) Handles TrackBar1.Scroll
    '    Me.Repaint()
    'End Sub
End Class


