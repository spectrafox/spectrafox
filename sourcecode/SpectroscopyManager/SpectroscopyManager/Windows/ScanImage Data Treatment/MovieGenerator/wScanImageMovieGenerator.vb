Public Class wScanImageMovieGenerator
    Inherits wFormBaseExpectsMultipleScanImagesOnLoad

#Region "Properties"

    ''' <summary>
    ''' Interface-READY Variable.
    ''' </summary>
    Private bReady As Boolean = True

#End Region

#Region "Form Contructor/Desctructor"

    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    ''' <summary>
    ''' Form closing
    ''' </summary>
    Private Sub Form_Close() Handles MyBase.FormClosing
        ' Save settings.
        'cGlobal.SaveSettings()
    End Sub

#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the ScanImages to the display modules.
    ''' </summary>
    Public Sub ScanImagesFetched() Handles MyBase.AllFilesFetched

        'Me._ScanImage1 = Me._ScanImageList.First
        'Me._ScanImage2 = Me._ScanImageList.Last

        '' Set source data selection:
        'Me.scSource1.SetScanImageObjects(Me._ScanImage1)
        'Me.scSource2.SetScanImageObjects(Me._ScanImage2)

        '' Set the names of the files to the boxes
        'Me.gbSource1.Text = Me._ScanImage1.FileNameWithoutPath
        'Me.gbSource2.Text = Me._ScanImage2.FileNameWithoutPath

        '' Load settings used last time.
        'Me.txtXOffset.SetValue(My.Settings.ScanImageMerger_XOffset)
        'Me.txtYOffset.SetValue(My.Settings.ScanImageMerger_YOffset)
        'Me.txtAngleOffset.SetValue(My.Settings.ScanImageMerger_AngleOffset)
        'Me.txtCombinationFactor.SetValue(My.Settings.ScanImageMerger_CombinationFactor)
        'Me.SetCombinationModeSelected(cScanImage.GetCombinationModeFromInt(My.Settings.ScanImageMerger_CombinationMethod))

        '' Activate interface handling.
        'Me.bReady = True

        '' create the initial plot
        'Me.PlotData()
    End Sub


#End Region

End Class