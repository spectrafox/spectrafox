Imports System.ComponentModel
Imports ImageMagick

Public Class wScanImageMovieGenerator
    Inherits wFormBaseExpectsMultipleScanImagesOnLoad

#Region "Properties"

    ''' <summary>
    ''' Interface-READY Variable.
    ''' </summary>
    Private bReady As Boolean = True

    ''' <summary>
    ''' Backgroundworker for creating the GIF animation.
    ''' </summary>
    Protected WithEvents _GIFAnimationWorker As New BackgroundWorker

    ''' <summary>
    ''' Backgroundworker for plotting the preview.
    ''' </summary>
    Protected WithEvents _PlotWorker As New BackgroundWorker

    Protected _PlottedImages As New List(Of Image)

    Public Property ImageSize As New Size(300, 300)

    Private Property SelectedParameterDisplayKey As String
    Private Property SelectedScanChannel As String
    Private Property SelectedMaxValue As Double
    Private Property SelectedMinValue As Double
    Private Property PropertyPlotPosition As PointF
    Private Property ShowScaleBar As Boolean


#End Region

#Region "Form Contructor/Desctructor"

    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me._GIFAnimationWorker.WorkerReportsProgress = True
        Me._PlotWorker.WorkerReportsProgress = True
    End Sub

    ''' <summary>
    ''' Form closing
    ''' </summary>
    Private Sub Form_Close(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me._GIFAnimationWorker.IsBusy Then e.Cancel = True
        If Me._PlotWorker.IsBusy Then e.Cancel = True
    End Sub

#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the ScanImages to the display modules.
    ''' </summary>
    Public Sub ScanImagesFetched() Handles MyBase.AllFilesFetched

        ' Load the common scan-channel names
        Dim CommonScanChannelNames As List(Of String) = cScanImage.GetCommonScanChannelNames(Me._ScanImageList)
        Dim CommonFileProperties As List(Of String) = cScanImage.GetCommonFilePropertyArrayKeys(Me._ScanImageList)

        ' Set the channel selection
        Dim LastSelectedChannelName As String = String.Empty
        If My.Settings.ScanChannelSelector_LastSelectedChannelNames.Count > 0 Then
            LastSelectedChannelName = My.Settings.ScanChannelSelector_LastSelectedChannelNames(0)
        End If
        Me.scChannel.InitializeColumns(CommonScanChannelNames, LastSelectedChannelName)

        ' Add the common file properties
        With Me.cbParameterDisplay
            .SuspendLayout()
            .Items.Clear()
            .Items.AddRange(CommonFileProperties.ToArray)
            .ResumeLayout()
        End With

        ' Create a scanimage viewer for each scan image in the fetched list.
        Try
            Me.flpImages.SuspendLayout()
            Me.lbOrdering.SuspendLayout()
            Me.flpImages.Controls.Clear()
            Me.lbOrdering.Items.Clear()
            For Each ScanImage As cScanImage In Me._ScanImageList

                ' Create a new scan-viewer
                Dim ScanImageViewer As New PictureBox()

                ' Set the individual scan objects
                With ScanImageViewer
                    '.cbChannel.Enabled = False
                    '.cbChannel.SetSelectedEntry(Me.scChannel.SelectedEntry)
                    '.SetScanImageObjects(ScanImage)
                    .Size = ImageSize
                    .BorderStyle = BorderStyle.Fixed3D
                End With

                ' Add the storage
                Me._PlottedImages.Add(Nothing)

                ' Fill the order box
                Me.lbOrdering.Items.Add(ScanImage.FileNameWithoutPath)

                ' Add the controls
                Me.flpImages.Controls.Add(ScanImageViewer)
                Me.flpImages.SetFlowBreak(ScanImageViewer, True)
            Next
        Catch ex As Exception
            Debug.WriteLine("Error adding a ScanImage.")
        Finally
            Me.flpImages.ResumeLayout()
            Me.lbOrdering.ResumeLayout()
        End Try

        ' Set the animation time to 50ms per picture
        Me.txtAnimationTime.SetValue(0.05 * Me._ScanImageList.Count)

        ' Activate interface handling.
        Me.bReady = True

        ' Call the channel-switched function
        Me.scChannel_Switch()
    End Sub

#End Region

#Region "Replot"

    ''' <summary>
    ''' Enable or disable the global file property display
    ''' </summary>
    Private Sub Replot() Handles cbParameterDisplay.SelectedIndexChanged,
        vrsColorScaling.SelectedRangeChanged,
        cpColorPicker.SelectedColorSchemaChanged,
        tbPlotPositionX.ValueChanged,
        tbPlotPositionY.ValueChanged,
        ckbShowScaleBar.CheckedChanged

        If Not Me.bReady Then Return
        Me.PlotImageAsync()

    End Sub

#End Region

#Region "Plotting"

    ''' <summary>
    ''' ScanChannel switched
    ''' </summary>
    Private Sub scChannel_Switch() Handles scChannel.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' Get a value histogram for all scan channels from the selected channel
        Dim HistogramValues As New List(Of Double)

        For Each ScanImage As cScanImage In Me._ScanImageList
            If ScanImage.ScanChannelExists(Me.scChannel.SelectedEntry) Then
                HistogramValues.AddRange(ScanImage.ScanChannels(Me.scChannel.SelectedEntry).ScanData.Values)
            End If
        Next

        ' Assign data to the value range selector
        If HistogramValues.Count > 0 Then
            Me.vrsColorScaling.SetValueArray(HistogramValues.ToArray)
            Me.vrsColorScaling.SetSelectedRange(HistogramValues.Min, HistogramValues.Max)
        End If

        ' Plot
        Me.PlotImageAsync()

    End Sub

    ''' <summary>
    ''' Starts the plotting of the image.
    ''' </summary>
    Public Sub PlotImageAsync()
        If Me._PlotWorker.IsBusy Then Return

        ' Save properties
        Me.SelectedParameterDisplayKey = Convert.ToString(Me.cbParameterDisplay.SelectedItem)
        Me.SelectedScanChannel = Me.scChannel.SelectedEntry
        Me.SelectedMaxValue = Me.vrsColorScaling.SelectedMaxValue
        Me.SelectedMinValue = Me.vrsColorScaling.SelectedMinValue
        Me.PropertyPlotPosition = New PointF(Convert.ToSingle(Me.tbPlotPositionX.Value / 100 * Me.ImageSize.Width),
                                             Convert.ToSingle(Me.tbPlotPositionY.Value / 100 * Me.ImageSize.Height))
        Me.ShowScaleBar = Me.ckbShowScaleBar.Checked

        Me._PlotWorker.RunWorkerAsync()
    End Sub

    ''' <summary>
    ''' Plots the images in a background thread.
    ''' </summary>
    Private Sub PlotImage() Handles _PlotWorker.DoWork

        ' fill all pictureboxes
        For i As Integer = 0 To Me._ScanImageList.Count - 1 Step 1

            ' Only react on ScanImageViewers
            If Me._PlottedImages.Count <= i Then Return

            ' Create the plotter
            Dim ScanImagePlot As New cScanImagePlot(Me._ScanImageList(i))

            ' Calculate Image:
            With ScanImagePlot
                .ClearPointMarkList()

                ' Create the heading for each picture
                If Me._ScanImageList(i).GeneralPropertyArray.ContainsKey(SelectedParameterDisplayKey) Then
                    Dim PropertyValueToPrint As String = Me._ScanImageList(i).GeneralPropertyArray(Me.SelectedParameterDisplayKey)

                    If IsNumeric(PropertyValueToPrint) Then
                        PropertyValueToPrint = cUnits.GetFormatedValueString(Convert.ToDouble(PropertyValueToPrint))
                    End If

                    Dim PropertyText As New cScanImagePlot.TextObject(Me.PropertyPlotPosition, PropertyValueToPrint)
                    .AddTextObjects({PropertyText}.ToList)
                End If

                .ColorScheme = Me.cpColorPicker.SelectedColorScheme
                .PlotScaleBar = Me.ShowScaleBar
                .CreateImage(Me.SelectedMaxValue, Me.SelectedMinValue,
                             Me.SelectedScanChannel,
                             Me.ImageSize.Width, Me.ImageSize.Height,
                             True)
            End With

            ' Set the individual image objects
            Me._PlottedImages(i) = ScanImagePlot.Image
        Next

    End Sub

    ''' <summary>
    ''' Show all pictures
    ''' </summary>
    Protected Sub PlotFinished() Handles _PlotWorker.RunWorkerCompleted
        For i As Integer = 0 To Me._PlottedImages.Count - 1 Step 1
            If Me.flpImages.Controls.Count <= i Then Return
            If TypeOf Me.flpImages.Controls(i) Is PictureBox Then
                CType(Me.flpImages.Controls(i), PictureBox).Image = Me._PlottedImages(i)
            End If
        Next
    End Sub

#End Region

#Region "Window Resizing"

    ''' <summary>
    ''' Adapt the sizes of the images
    ''' </summary>
    Private Sub flpImages_Resize(sender As Object, e As EventArgs) Handles flpImages.Resize
        '' Rescale all controls:
        'Dim NewHeight As Integer
        'If Me.flpImages.Size.Height < 0 Then
        '    NewHeight = 100
        'ElseIf Me.flpImages.Size.Height < 600 Then
        '    NewHeight = Me.flpImages.Size.Height
        'Else
        '    NewHeight = CInt(Me.flpImages.Size.Height / 2)
        'End If
        'Dim NewWidth As Integer
        'If Me.flpImages.Size.Width > 30 Then
        '    NewWidth = Me.flpImages.Size.Width - 30
        'Else
        '    NewWidth = Me.flpImages.Size.Width
        'End If
        'For i As Integer = 0 To Me.flpImages.Controls.Count - 1 Step 1
        '    Me.flpImages.Controls(i).Size = New Size(NewWidth, NewHeight)
        'Next
    End Sub

#End Region

#Region "GIF animation"

    ''' <summary>
    ''' Filename to store the GIF animation.
    ''' </summary>
    Private _GIFAnimationOutputFileName As String

    ''' <summary>
    ''' Generate the GIF animation file.
    ''' </summary>
    Private Sub btnGenerateGIF_Click(sender As Object, e As EventArgs) Handles btnGenerateGIF.Click
        If Me._GIFAnimationWorker.IsBusy Then Return

        ' Ask for a filename:
        Dim FileBrowser As New SaveFileDialog
        With FileBrowser
            .Title = My.Resources.rGridPlotter.ExportGif_FileDialogTitle
            .Filter = "GIF animation file|*.gif"
            .FileName = "movie.gif"
            .ValidateNames = True
        End With

        ' Check for a valid filename.
        If Not FileBrowser.ShowDialog() = DialogResult.OK Then
            Return
        End If

        ' Store the target filename
        Me._GIFAnimationOutputFileName = FileBrowser.FileName

        Me.ActivateLoadingButton(False)

        ' Start the GIF generation.
        Me._GIFAnimationWorker.RunWorkerAsync()
    End Sub

    ''' <summary>
    ''' Generates the GIF Animation.
    ''' </summary>
    Protected Sub GenerateGIFAnimation() Handles _GIFAnimationWorker.DoWork
        ' Only continue, if we have more than an image
        If Me._ScanImageList.Count <= 0 Then Return

        Dim Steps As Integer = Me.flpImages.Controls.Count
        Dim Progress As Double = 0
        Dim ProgressProgress As Double = 0

        ' Generate the images.
        Dim Images As New List(Of Bitmap)

        Try
            ' Rescale all colors:
            ProgressProgress = 85 / Steps
            For i As Integer = 0 To Me.flpImages.Controls.Count - 1 Step 1

                ' Only react on ScanImageViewers
                If Not TypeOf Me.flpImages.Controls(i) Is PictureBox Then Continue For
                Dim ScanImageViewer As PictureBox = CType(Me.flpImages.Controls(i), PictureBox)


                ' Report progress
                If Me._GIFAnimationWorker.IsBusy Then Me._GIFAnimationWorker.ReportProgress(CInt(Progress), My.Resources.rGridPlotter.ExportGif_Progress_GeneratingImage.Replace("%i", i.ToString).Replace("%max", Steps.ToString))
                Progress += ProgressProgress

                '####################
                ' Generate the image

                ' Store the image:
                Images.Add(New Bitmap(ScanImageViewer.Image))

            Next

            ' Progress
            Progress = 90
            If Me._GIFAnimationWorker.IsBusy Then Me._GIFAnimationWorker.ReportProgress(CInt(Progress), My.Resources.rGridPlotter.ExportGif_Progress_GeneratingFinalGIF)

            ' Create the GIF file
            Using GIFCreator As MagickImageCollection = New MagickImageCollection

                ' Get the time per image.
                Dim AnimationTime As Integer = CInt(Me.txtAnimationTime.DecimalValue / Images.Count)

                For i As Integer = 0 To Images.Count - 1 Step 1
                    GIFCreator.Add(New MagickImage(Images(i)))
                    GIFCreator(i).AnimationDelay = AnimationTime
                    ' For DEBUG:
                    'Images(i).Save(FileBrowser.FileName & "." & i.ToString & ".gif")
                Next

                '' Adjust the settings of the GIF
                'Dim settings As QuantizeSettings = New QuantizeSettings
                'settings.Colors = 256
                'settings.ColorSpace = ColorSpace.RGB
                'GIFCreator.Quantize(settings)

                ' Save gif
                GIFCreator.Write(Me._GIFAnimationOutputFileName)

            End Using

            If Me._GIFAnimationWorker.IsBusy Then Me._GIFAnimationWorker.ReportProgress(-1, String.Empty)
        Catch ex As Exception
            Debug.WriteLine("wScanImageMoviewGenerator:GifCreator--> error " & ex.Message)
        Finally
            ' Dispose all bitmaps!
            For i As Integer = 0 To Images.Count - 1 Step 1
                Images(i).Dispose()
            Next
        End Try

    End Sub

    ''' <summary>
    ''' Called after creating the GIF Animation.
    ''' </summary>
    Protected Sub GenerateGIFAnimationFinished() Handles _GIFAnimationWorker.RunWorkerCompleted

        Me.ActivateLoadingButton(True)

    End Sub

    ''' <summary>
    ''' Enables of disables the interface to load something.
    ''' </summary>
    Protected Sub ActivateLoadingButton(ByVal Active As Boolean)
        Me.gbChannel.Enabled = Active
        Me.gbGenerateGIF.Enabled = Active
        For i As Integer = 0 To Me.flpImages.Controls.Count - 1 Step 1
            Me.flpImages.Controls(i).Enabled = Active
        Next
    End Sub

#End Region

#Region "Drag and drop ordering."

    ''' <summary>
    ''' Down-Click
    ''' </summary>
    Private Sub lbOrdering_MouseDown(sender As Object, e As MouseEventArgs) Handles lbOrdering.MouseDown
        If Me.lbOrdering.SelectedItem Is Nothing Then
            Return
        End If
        Me.lbOrdering.DoDragDrop(Me.lbOrdering.SelectedItem, DragDropEffects.Move)
    End Sub

    ''' <summary>
    ''' Dragging
    ''' </summary>
    Private Sub lbOrdering_DragOver(sender As Object, e As DragEventArgs) Handles lbOrdering.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    ''' <summary>
    ''' Drop
    ''' </summary>
    Private Sub lbOrdering_DragDrop(sender As Object, e As DragEventArgs) Handles lbOrdering.DragDrop

        ' Get the target index of the dragged entry
        Dim point As Point = Me.lbOrdering.PointToClient(New Point(e.X, e.Y))
        Dim index As Integer = Me.lbOrdering.IndexFromPoint(point)
        If index < 0 Then
            index = Me.lbOrdering.Items.Count - 1
        End If

        ' Get the selected entry
        Dim SelectedEntry As Object = Me.lbOrdering.SelectedItem

        ' Resort the _ScanImage-List
        Dim FileNameOfSelectedEntry As String = SelectedEntry.ToString
        Dim SelectedScanImage As cScanImage = Nothing
        For i As Integer = 0 To Me._ScanImageList.Count - 1 Step 1
            If Me._ScanImageList(i).FileNameWithoutPath = FileNameOfSelectedEntry Then
                SelectedScanImage = Me._ScanImageList(i)
                Exit For
            End If
        Next

        ' Abort on error
        If SelectedScanImage Is Nothing Then Return

        Me._ScanImageList.Remove(SelectedScanImage)
        Me._ScanImageList.Insert(index, SelectedScanImage)

        ' Remove it, and put it to the selected position in the list.
        Me.lbOrdering.Items.Remove(SelectedEntry)
        Me.lbOrdering.Items.Insert(index, SelectedEntry)

        ' Replot
        Me.ScanImagesFetched()

    End Sub

#End Region

End Class