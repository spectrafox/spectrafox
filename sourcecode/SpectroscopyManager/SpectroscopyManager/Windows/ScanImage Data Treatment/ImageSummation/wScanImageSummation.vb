Public Class wScanImageSummation
    Inherits wFormBaseExpectsMultipleScanImagesOnLoad

#Region "Properties"

    ''' <summary>
    ''' Interface-READY Variable.
    ''' </summary>
    Private bReady As Boolean = True

    ''' <summary>
    ''' ScanImage 1, to be used as first.
    ''' </summary>
    Protected _ScanImage1 As cScanImage

    ''' <summary>
    ''' ScanImage 2, to be used as second image.
    ''' </summary>
    Protected _ScanImage2 As cScanImage

#End Region

#Region "Form Contructor/Desctructor"

    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set Window Title
        Me.Text &= MyBase._FileObjectList.First.FileName & " + " & MyBase._FileObjectList.Last.FileName

    End Sub

    ''' <summary>
    ''' Form closing
    ''' </summary>
    Private Sub Form_Close() Handles MyBase.FormClosing
        ' Load settings used last time.
        My.Settings.ScanImageMerger_XOffset = Me.txtXOffset.DecimalValue
        My.Settings.ScanImageMerger_YOffset = Me.txtYOffset.DecimalValue
        My.Settings.ScanImageMerger_CombinationFactor = Me.txtCombinationFactor.DecimalValue
        My.Settings.ScanImageMerger_CombinationMethod = Me.GetCombinationModeSelected
        My.Settings.ScanImageMerger_AngleOffset = Me.txtAngleOffset.DecimalValue
        cGlobal.SaveSettings()
    End Sub

#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the ScanImages to the display modules.
    ''' </summary>
    Public Sub ScanImagesFetched() Handles MyBase.AllFilesFetched

        Me._ScanImage1 = Me._ScanImageList.First
        Me._ScanImage2 = Me._ScanImageList.Last

        ' Set source data selection:
        Me.scSource1.SetScanImageObjects(Me._ScanImage1)
        Me.scSource2.SetScanImageObjects(Me._ScanImage2)

        ' Set the names of the files to the boxes
        Me.gbSource1.Text = Me._ScanImage1.FileNameWithoutPath
        Me.gbSource2.Text = Me._ScanImage2.FileNameWithoutPath

        ' Load settings used last time.
        Me.txtXOffset.SetValue(My.Settings.ScanImageMerger_XOffset)
        Me.txtYOffset.SetValue(My.Settings.ScanImageMerger_YOffset)
        Me.txtAngleOffset.SetValue(My.Settings.ScanImageMerger_AngleOffset)
        Me.txtCombinationFactor.SetValue(My.Settings.ScanImageMerger_CombinationFactor)
        Me.SetCombinationModeSelected(cScanImage.GetCombinationModeFromInt(My.Settings.ScanImageMerger_CombinationMethod))

        ' Activate interface handling.
        Me.bReady = True

        ' create the initial plot
        Me.PlotData()
    End Sub

#End Region

#Region "Summation process"

    ''' <summary>
    ''' Changes the offset and plots the data.
    ''' </summary>
    Public Sub PlotData()

        ' Only proceed, if we have two scan images.
        If Not Me.bReady Then Return
        If Me._ScanImage1 Is Nothing OrElse Me._ScanImage2 Is Nothing Then Return

        ' Get the currently plotted scan channels in 1 and 2
        Dim ChannelName1 As String = Me.scSource1.cbChannel.SelectedEntry
        Dim ChannelName2 As String = Me.scSource2.cbChannel.SelectedEntry

        ' Abort, if the channels do not exist.
        If Not Me._ScanImage1.ScanChannelExists(ChannelName1) OrElse Not Me._ScanImage2.ScanChannelExists(ChannelName2) Then
            Return
        End If

        ' Get the channels:
        Dim ScanChannel1 As cScanImage.ScanChannel = Me._ScanImage1.ScanChannels(ChannelName1).GetCopy
        Dim ScanChannel2 As cScanImage.ScanChannel = Me._ScanImage2.ScanChannels(ChannelName2).GetCopy

        ' Create a combined channel name.
        ' Use the same name as the first selection, if both names in the images are the same.
        Dim OutputName As String
        If ChannelName1 = ChannelName2 Then
            OutputName = ChannelName1
        Else
            OutputName = "merged image"
        End If

        ScanChannel1.Name = OutputName
        ScanChannel2.Name = OutputName

        ' Create dummy images, that are shifted by the offset.
        Dim SourceImage1 As New cScanImage
        With SourceImage1
            .ScanRange_X = Me._ScanImage1.ScanRange_X
            .ScanRange_Y = Me._ScanImage1.ScanRange_Y
            .ScanPixels_X = Me._ScanImage1.ScanPixels_X
            .ScanPixels_Y = Me._ScanImage1.ScanPixels_Y
            .ScanOffset_X = Me._ScanImage1.ScanOffset_X
            .ScanOffset_Y = Me._ScanImage1.ScanOffset_Y
            .ScanAngle = Me._ScanImage1.ScanAngle + Me.txtAngleOffset.DecimalValue
        End With

        Dim SourceImage2 As New cScanImage
        With SourceImage2
            .ScanRange_X = Me._ScanImage2.ScanRange_X
            .ScanRange_Y = Me._ScanImage2.ScanRange_Y
            .ScanPixels_X = Me._ScanImage2.ScanPixels_X
            .ScanPixels_Y = Me._ScanImage2.ScanPixels_Y
            .ScanOffset_X = Me._ScanImage2.ScanOffset_X + Me.txtXOffset.DecimalValue
            .ScanOffset_Y = Me._ScanImage2.ScanOffset_Y + Me.txtYOffset.DecimalValue
            .ScanAngle = Me._ScanImage2.ScanAngle + Me.txtAngleOffset.DecimalValue
        End With

        ' Add the source channels to the dummy images
        SourceImage1.AddScanChannel(ScanChannel1)
        SourceImage2.AddScanChannel(ScanChannel2)

        ' Get the combination mode
        Dim CombinationFactor As Double = Me.txtCombinationFactor.DecimalValue
        Dim CombinationMode As cScanImage.ScanChannelCombinationMode = Me.GetCombinationModeSelected

        ' Create new scan image with both channels summed up.
        ' Create a combined scan-image from all the input-scan-images.
        Dim OutputScanImage = cScanImage.CombineScanChannels({SourceImage1, SourceImage2}.ToList,
                                                             OutputName,
                                                             Me.scSourceOutput.Width,
                                                             Me.scSourceOutput.Height,
                                                             CombinationMode,
                                                             {1, CombinationFactor}.ToList)

        ' Set the output to the combined image:
        Me.scOutput.SetScanImageObjects(OutputScanImage, OutputName)

    End Sub

#End Region

#Region "Interface functions"

    ''' <summary>
    ''' Returns the currently selected combination mode.
    ''' </summary>
    Public Function GetCombinationModeSelected() As cScanImage.ScanChannelCombinationMode

        Dim CombinationMode As cScanImage.ScanChannelCombinationMode

        If Me.rdbMergeOperation_Summation.Checked Then
            CombinationMode = cScanImage.ScanChannelCombinationMode.Summation
        ElseIf Me.rdbMergeOperation_Multiplication.Checked Then
            CombinationMode = cScanImage.ScanChannelCombinationMode.Multiplication
        ElseIf Me.rdbMergeOperation_Division.Checked Then
            CombinationMode = cScanImage.ScanChannelCombinationMode.Division
        ElseIf Me.rdbMergeOperation_Override.Checked Then
            CombinationMode = cScanImage.ScanChannelCombinationMode.Override
        Else
            CombinationMode = cScanImage.ScanChannelCombinationMode.Override
        End If

        Return CombinationMode
    End Function

    ''' <summary>
    ''' Sets the currently selected combination mode.
    ''' </summary>
    Public Sub SetCombinationModeSelected(CombinationMode As cScanImage.ScanChannelCombinationMode)

        Select Case CombinationMode
            Case cScanImage.ScanChannelCombinationMode.Summation
                Me.rdbMergeOperation_Summation.Checked = True
            Case cScanImage.ScanChannelCombinationMode.Multiplication
                Me.rdbMergeOperation_Multiplication.Checked = True
            Case cScanImage.ScanChannelCombinationMode.Division
                Me.rdbMergeOperation_Division.Checked = True
            Case cScanImage.ScanChannelCombinationMode.Override
                Me.rdbMergeOperation_Override.Checked = True
        End Select

    End Sub

#End Region

#Region "Replot functions"

    ''' <summary>
    ''' Plot the data due to a change in a numeric textbox.
    ''' </summary>
    Private Sub ReplotDueToTextboxChange(ByRef NT As NumericTextbox) Handles txtXOffset.ValidValueChanged, txtYOffset.ValidValueChanged, txtCombinationFactor.ValidValueChanged, txtAngleOffset.ValidValueChanged
        If Not Me.bReady Then Return
        Me.PlotData()
    End Sub

    ''' <summary>
    ''' Replot on merge operation change
    ''' </summary>
    Private Sub rdbMergeOperationChanged(sender As Object, e As EventArgs) Handles rdbMergeOperation_Summation.CheckedChanged, rdbMergeOperation_Multiplication.CheckedChanged, rdbMergeOperation_Division.CheckedChanged, rdbMergeOperation_Override.CheckedChanged
        If Not Me.bReady Then Return
        Me.PlotData()
    End Sub

    ''' <summary>
    ''' Replot after scan channel change.
    ''' </summary>
    Private Sub ReplotDueToScanChannelChange(ChannelName As String) Handles scSource1.SelectedScanChannelChanged, scSource2.SelectedScanChannelChanged
        If Not Me.bReady Then Return
        Me.PlotData()
    End Sub

    ''' <summary>
    ''' Exchanges the files and replots.
    ''' </summary>
    Private Sub btnExchange1and2_Click(sender As Object, e As EventArgs) Handles btnExchange1and2.Click
        If Not Me.bReady Then Return

        ' Exchange the files
        Me._ScanImageList.Reverse()
        Me.ScanImagesFetched()

        ' replot
        Me.PlotData()

    End Sub

#End Region


End Class