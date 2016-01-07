Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.IO

Public Class mScanImageViewer
    Implements IDisposable

#Region "Properties"

    ''' <summary>
    ''' Scan-Images currently loading
    ''' </summary>
    Private oScanImages As New List(Of cScanImage)

    ''' <summary>
    ''' Scan-Image currently plotted
    ''' </summary>
    Private oScanImage As cScanImage

    ''' <summary>
    ''' Scan-Image that is currently plotted.
    ''' May be a combined one from several images.
    ''' </summary>
    Public ReadOnly Property ScanImagePlotted As cScanImage
        Get
            Return Me.oScanImage
        End Get
    End Property

    ''' <summary>
    ''' Stores a reference to the plotted scan-channel.
    ''' </summary>
    Private oScanChannelPlotted As cScanImage.ScanChannel

    ''' <summary>
    ''' Determines, if a new scan-image has been fetched.
    ''' </summary>
    Private bPlottedChannelChanged As Boolean = False

    ''' <summary>
    ''' Scan-Image that is pending for loading.
    ''' This object is used as temporary Scan-Image-Object,
    ''' if the Background-Worker is busy while getting the
    ''' task for drawing a new image.
    ''' </summary>
    Private oScanImagesPending As New List(Of cScanImage)

    ''' <summary>
    ''' Scan Image Plot Object used for plotting the current image
    ''' </summary>
    Private oScanImagePlot As cScanImagePlot

    ''' <summary>
    ''' Background-Worker Object to load the Scan-Image in a separate Thread.
    ''' </summary>
    Private WithEvents ImageLoadingBackgroundWorker As New BackgroundWorker

    Private ImageFetcherResult As Bitmap
    Private ImageFetcherTargetWidth As Integer
    Private ImageFetcherTargetHeight As Integer
    Private ImageFetcherChannelName As String
    Private ImageFetcherColorScheme As cColorScheme
    Private ImageFetcherRedrawPending As Boolean = False

    ''' <summary>
    ''' Saves Points to mark on the Image.
    ''' </summary>
    Private Property ListOfPointMarks As New List(Of cScanImagePlot.PointMark)

    ''' <summary>
    ''' Saves the currently selected scan-channel-ID.
    ''' </summary>
    Private _CurrentlySelectedScanChannelName As String = String.Empty

    ''' <summary>
    ''' Save a list of all available scan-image filters.
    ''' </summary>
    Private _ScanImageFiltersAvailable As New List(Of iScanImageFilter)

    ''' <summary>
    ''' Marks the selected indices of the _ScanImageFiltersAvailable list.
    ''' </summary>
    Private _ScanImageFiltersSelected As New List(Of iScanImageFilter)

#End Region

#Region "Module Load Function"
    ''' <summary>
    ''' Constructor: Sets the Adresses for the Scan-Image Background-Worker
    ''' </summary>
    Private Sub mScanImageViewer_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ' Just load the a scan-filter list, if we are not in design-mode!
        If Not Me.DesignMode Then
            _ScanImageFiltersAvailable = cScanImage.ScanChannel.GetAllScanImageFilters
        End If

        ' Set Worker-Settings
        With Me.ImageLoadingBackgroundWorker
            .WorkerReportsProgress = True
            .WorkerSupportsCancellation = True
        End With

        ' Populate Filter list.
        With Me.cbAddFilter
            .DisplayMember = "Key"
            .ValueMember = "Value"
            For Each Filter As iScanImageFilter In Me._ScanImageFiltersAvailable
                If Filter.ShowFilterInFilterMenu Then
                    .Items.Add(New KeyValuePair(Of String, iScanImageFilter)(Filter.FilterName, Filter))
                End If
            Next
            If Me._ScanImageFiltersAvailable.Count > 0 Then .SelectedIndex = 0
        End With

        ' Initially collapse the setting panels
        Me.dpRight.SlideIn(True)
        Me.dpLeft.SlideIn(True)

        ' Locate the panel buttons at the lower corners
        Me.panChannelSettings.Location = New Point(CInt(0) + 3,
                                                   CInt(Me.Height - Me.panChannelSettings.Height) - 3)
        Me.panValueSettings.Location = New Point(CInt(Me.Width - Me.panValueSettings.Width) - 3,
                                                 CInt(Me.Height - Me.panValueSettings.Height) - 3)

    End Sub

    ''' <summary>
    ''' Sets the scan-image to plot for this Module.
    ''' Has to be called for the plotting functionalitys to work.
    ''' IF the setted object is the same as loaded before, a simple replot is performed.
    ''' </summary>
    Public Sub SetScanImageObjects(ByRef ScanImage As cScanImage,
                                   Optional ByVal ScanChannelName As String = "")
        Me.SetScanImageObjects({ScanImage}.ToList, ScanChannelName)
    End Sub

    ''' <summary>
    ''' Sets multiple objects for this module.
    ''' Has to be called for the plotting functionalitys to work.
    ''' IF the setted Object is the same as loaded before, a simple replot is performed.
    ''' </summary>
    Public Sub SetScanImageObjects(ByRef ScanImages As List(Of cScanImage),
                                   Optional ByVal ScanChannelName As String = "")
        ' Save the Image into the Pending Object, if the Worker is busy
        ' and send the Worker the Cancellation Signal
        ' If the Worker is idling, then just set the new Scan-Image:
        If Me.ImageLoadingBackgroundWorker.IsBusy Then
            ' BUSY
            '######
            ' Just ask for a refresh, if the pending list is the same, as the newly set list.
            If Me.oScanImagesPending IsNot Nothing AndAlso Me.oScanImagesPending Is ScanImages Then
                Me.ImageFetcherRedrawPending = True
            Else
                ' set the new list to pending
                Me.oScanImagesPending = ScanImages
            End If
        Else
            ' IDLE
            '######

            ' Reset the selection-mode: TWICE to really reset everything.
            Me.SelectionMode = SelectionModes.None
            Me.SelectionMode = SelectionModes.None

            ' set the new list to plot
            Me.oScanImages = ScanImages
            Me.oScanImagesPending = Nothing

            ' Write Scan-Channels to Combobox and select first:
            Me.cbChannel.InitializeColumns(cScanImage.GetCommonColumns(Me.oScanImages))
            Me.UpdateSelectedScanChannelName()

        End If
    End Sub

    ''' <summary>
    ''' Start Background-Worker for drawing the image.
    ''' </summary>
    Public Sub RecalculateImage() Handles vsValueRangeSelector.SelectedRangeChanged, cpColorPicker.SelectedColorSchemaChanged, pbScanImage.Resize
        If Me.oScanImages.Count <= 0 Then Return

        ' If the Fetcher is Busy, set the "Redraw-Pending" variable to
        ' tell the Fetcher to redraw with the same scan-object, if it finishes.
        If Me.ImageLoadingBackgroundWorker.IsBusy Then
            Me.ImageFetcherRedrawPending = True
            Return
        End If

        ' Set ImageFetcherProperties
        Me.ImageFetcherChannelName = Me.GetSelectedScanChannelName
        Me.ImageFetcherTargetWidth = Me.pbScanImage.Width
        Me.ImageFetcherTargetHeight = Me.pbScanImage.Height
        Me.ImageFetcherColorScheme = Me.cpColorPicker.GetSelectedColorScheme

        Me.tslblInfobar.Text = My.Resources.rScanImageViewer.InfobarTemplate_RedrawInProgress

        Me.ImageLoadingBackgroundWorker.RunWorkerAsync()
    End Sub

#End Region

#Region "Dispose"

    ''' <summary>
    ''' Disposes all the ressources.
    ''' </summary>
    Public Sub OnDispose() Implements IDisposable.Dispose

        ' Dispose managed objects.
        If Disposing Then
            Me._SelectionMode_Font.Dispose()
            Me._SelectionModePen_Text.Dispose()
            Me._SelectionModePen_Selection.Dispose()
            Me._SelectionModePen_CrossHair.Dispose()
        End If

    End Sub

#End Region

#Region "Threaded Image-Load Functions"

    ''' <summary>
    ''' Scan-Image-Fetch-Function
    ''' </summary>
    Private Sub ScanImageFetcher(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles ImageLoadingBackgroundWorker.DoWork

        ' Create new ScanImagePlot from the Data
        Me.oScanImagePlot = New cScanImagePlot(Me.oScanImages)

        ' Setup the plot-engine
        Me.oScanImagePlot.PlotScaleBar = Me.ckbScaleBarVisible.Checked

        ' If we have a new scan-channel that is plotted,
        ' then initially use the full value range to be plotted.
        Dim MaxValueToPlot As Double
        Dim MinValueToPlot As Double
        If Me.bPlottedChannelChanged Then
            MaxValueToPlot = Double.NaN
            MinValueToPlot = Double.NaN
        Else
            MaxValueToPlot = Me.vsValueRangeSelector.SelectedMaxValue
            MinValueToPlot = Me.vsValueRangeSelector.SelectedMinValue
        End If

        ' Calculate Image:
        With Me.oScanImagePlot
            .ClearPointMarkList()
            .AddPointMarks(Me.ListOfPointMarks)
            .ColorScheme = Me.ImageFetcherColorScheme
            .ScanImageFiltersToApplyBeforePlot = Me._ScanImageFiltersSelected
            .CreateImage(MaxValueToPlot,
                         MinValueToPlot,
                         Me.GetSelectedScanChannelName,
                         Me.pbScanImage.Width, Me.pbScanImage.Height,
                         Me.ckbUseHighQualityScaling.Checked)
        End With
        Me.oScanImage = Me.oScanImagePlot.ScanImagePlotted
        Me.oScanChannelPlotted = Me.oScanImagePlot.ScanChannelPlotted

        ' Store the output image.
        Me.ImageFetcherResult = Me.oScanImagePlot.Image
    End Sub

    ''' <summary>
    ''' If the Image is fetched, check, if the progress was canceled
    ''' or, if another image is pending. If so, then do not paint the obtained image
    ''' and simply reload.
    ''' </summary>
    Private Sub ImageFetcher_FetchComplete(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ImageLoadingBackgroundWorker.RunWorkerCompleted
        ' Check, if a Scan-Image is pending, then just reload the Scan-Image
        If Me.oScanImagesPending IsNot Nothing AndAlso Me.oScanImagesPending.Count > 0 Then
            ' Scan Image Pending
            '####################

            ' Restart the fetch of the images.
            Dim TMPScanImages As List(Of cScanImage) = Me.oScanImagesPending
            Me.oScanImagesPending = Nothing
            Me.SetScanImageObjects(TMPScanImages)

        Else

            ' Scan Image not Pending -> Display calculated image, if the worker was not cancelled.
            If Not e.Cancelled Then

                ' Paint rescaled image to draw-area:
                Me.pbScanImage.Image = Me.ImageFetcherResult

                ' Set additional properties
                Dim InfoBarText As String = My.Resources.rScanImageViewer.InfobarTemplate_ScanImageName
                InfoBarText = InfoBarText.Replace("%name", Me.oScanImage.ScanImageName)
                InfoBarText = InfoBarText.Replace("%channel", Me._CurrentlySelectedScanChannelName)
                InfoBarText = InfoBarText.Replace("%wr", cUnits.GetFormatedValueString(Me.oScanImage.ScanRange_X))
                InfoBarText = InfoBarText.Replace("%hr", cUnits.GetFormatedValueString(Me.oScanImage.ScanRange_Y))

                Me.tslblInfobar.Text = InfoBarText

                ' Change the plot.
                If Me.bPlottedChannelChanged Then
                    Me.ScanChannelChanged()
                End If

                ' Unmark a newly plotted scan-channel
                ' (so we dont have to hand over the value-array to the value-range selector).
                Me.bPlottedChannelChanged = False
            End If

            If Me.ImageFetcherRedrawPending Then
                Me.ImageFetcherRedrawPending = False
                Me.RecalculateImage()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Function for reporting the progress of the plot-background-worker.
    ''' </summary>
    Private Sub ImageFetcher_ReportProgress(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles ImageLoadingBackgroundWorker.ProgressChanged
        'lblRedrawing
    End Sub

#End Region

#Region "Scan Channel Selection"
    ''' <summary>
    ''' Returns the selected Scan-Channel of the Image.
    ''' </summary>
    <DebuggerStepThrough>
    Private Function GetSelectedScanChannelName() As String
        Return Me._CurrentlySelectedScanChannelName
    End Function

    ''' <summary>
    ''' Selected Scan-Channel changed:
    ''' Reload Scan-Intensities and Repaint Scan-Image
    ''' </summary>
    Private Sub UpdateSelectedScanChannelName() Handles cbChannel.SelectedIndexChanged
        ' Get the name of the currently selected Scan-Channel
        Me._CurrentlySelectedScanChannelName = Me.cbChannel.SelectedEntry
        Me.bPlottedChannelChanged = True
        Me.RecalculateImage()
    End Sub

    ''' <summary>
    ''' Change of the scan-channel detected. --> Filter, change of the selected ID, etc.
    ''' </summary>
    Public Sub ScanChannelChanged()
        If Me.oScanImage Is Nothing Then Return
        If Me.oScanChannelPlotted Is Nothing Then Return

        ' Get the new scan-channel
        Dim CurrentScanChannel As cScanImage.ScanChannel = Me.oScanChannelPlotted

        ' Get all Filters, and populate the filter-list
        With Me.lbFilters
            .ClearSelected()
            .Items.Clear()
            For i As Integer = 0 To CurrentScanChannel.FilterCount - 1 Step 1
                .Items.Add(CurrentScanChannel.Filter(i).FilterName)
            Next
        End With

        ' Add data to the ValueRangeSelector
        Me.vsValueRangeSelector.SetValueArray(CurrentScanChannel.ScanData.Values,
                                              CurrentScanChannel.UnitSymbol)
    End Sub

#End Region

#Region "Image Filtering"

    ''' <summary>
    ''' Add the selected scan-image filter to the current channel.
    ''' </summary>
    Private Sub btnAddFilter_Click(sender As Object, e As EventArgs) Handles btnAddFilter.Click
        If Me.oScanImage Is Nothing Then Return
        If Me.cbAddFilter.SelectedItem Is Nothing Then Return

        ' Add the selected filter to the end list of the selected filters.
        Try
            Dim SelectedFilterName As String = DirectCast(Me.cbAddFilter.SelectedItem, KeyValuePair(Of String, iScanImageFilter)).Key

            ' Add the filter
            For i As Integer = 0 To Me._ScanImageFiltersAvailable.Count - 1 Step 1
                If Me._ScanImageFiltersAvailable(i).FilterName = SelectedFilterName Then
                    Me._ScanImageFiltersSelected.Add(Me._ScanImageFiltersAvailable(i))
                    Me.lbFilters.Items.Add(Me._ScanImageFiltersAvailable(i).FilterName)
                    Exit For
                End If
            Next

            ' "Change" the scan-channel to produce a redraw of the filtered list.
            Me.bPlottedChannelChanged = True
            Me.RecalculateImage()
        Catch ex As Exception
            Debug.WriteLine("Selected filter could not be extracted: " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Open the settings-window, if the selected filter has options to be set up.
    ''' </summary>
    Private Sub btnFilterSettings_Click(sender As Object, e As EventArgs) Handles btnFilterSettings.Click

    End Sub

    ''' <summary>
    ''' Remove the selected filter.
    ''' </summary>
    Private Sub btnRemoveFilter_Click(sender As Object, e As EventArgs) Handles btnRemoveFilter.Click
        If Me.oScanImage Is Nothing Then Return

        Dim FilterIndex As Integer = Me.lbFilters.SelectedIndex
        If FilterIndex >= 0 And FilterIndex < Me.lbFilters.Items.Count Then
            ' Remove the selected filter.
            Me._ScanImageFiltersSelected.RemoveAt(FilterIndex)
            Me.lbFilters.Items.RemoveAt(FilterIndex)

            ' "Change" the scan-channel to produce a redraw of the filtered list.
            Me.bPlottedChannelChanged = True
            Me.RecalculateImage()
        End If
    End Sub

    ''' <summary>
    ''' Activate or deactivate the buttons on selecting a filter.
    ''' </summary>
    Private Sub lbFilters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbFilters.SelectedIndexChanged
        If Me.oScanImage Is Nothing Then Return

        Dim SelectedIndex As Integer = Me.lbFilters.SelectedIndex
        If SelectedIndex < Me._ScanImageFiltersSelected.Count AndAlso SelectedIndex >= 0 Then
            Me.btnRemoveFilter.Enabled = True

            ' Just enable the settings button, if the filter needs a setup
            If Me._ScanImageFiltersSelected(SelectedIndex).NeedsSetup Then
                Me.btnFilterSettings.Enabled = True
            End If
        Else
            Me.btnRemoveFilter.Enabled = False
            Me.btnFilterSettings.Enabled = False
        End If
    End Sub


#End Region

#Region "Addon Functions (Saving, exporting)"

    ''' <summary>
    ''' Saves the Image to the Clipboard
    ''' </summary>
    Private Sub cmnuCopyImageToClipboard_Click(sender As System.Object, e As System.EventArgs) Handles cmnuCopyImageToClipboard.Click
        Using stream = New MemoryStream()
            Me.pbScanImage.Image.Save(stream, ImageFormat.Png)

            Clipboard.Clear()
            Dim data = New DataObject()
            data.SetData("PNG", True, stream)
            data.SetData(DataFormats.Bitmap, True, Me.pbScanImage.Image)
            Clipboard.SetDataObject(data, True)
        End Using
    End Sub

    ''' <summary>
    ''' Save Image to File:
    ''' </summary>
    Private Sub cmnuSaveAsImage_Click(sender As System.Object, e As System.EventArgs) Handles cmnuSaveAsImage.Click
        Dim fs As New SaveFileDialog
        With fs
            .InitialDirectory = My.Settings.LastExport_Path
            .Title = My.Resources.Title_SaveImage
            .Filter = "Portable Network Graphics|*.png|JPEG Image|*.jpg|Windows Bitmap|*.bmp|Graphics Interchange Format|*.gif|Windows Enhanced Metafile|*.emf|Exchangeable Image File Format|*.exif"
            If .ShowDialog = DialogResult.OK Then
                Select Case .FilterIndex
                    Case 1
                        Me.pbScanImage.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Png)
                    Case 2
                        Me.pbScanImage.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)
                    Case 3
                        Me.pbScanImage.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Bmp)
                    Case 4
                        Me.pbScanImage.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Gif)
                    Case 5
                        Me.pbScanImage.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Emf)
                    Case 6
                        Me.pbScanImage.Image.Save(.FileName, System.Drawing.Imaging.ImageFormat.Exif)
                End Select
            End If
        End With
    End Sub

#End Region

#Region "PointMark Functions"

    ''' <summary>
    ''' Adds a Point-Mark to the Display-List
    ''' </summary>
    Public Sub AddPointMark(PM As cScanImagePlot.PointMark)
        Me.ListOfPointMarks.Add(PM)
    End Sub

    ''' <summary>
    ''' Adds a Point-Marks to the Display-List
    ''' </summary>
    Public Sub AddPointMark(PMs As List(Of cScanImagePlot.PointMark))
        Me.ListOfPointMarks.AddRange(PMs)
    End Sub

    ''' <summary>
    ''' Removes all entries from the Point-Mark-List
    ''' </summary>
    Public Sub ClearPointMarkList()
        Me.ListOfPointMarks.Clear()
    End Sub

#End Region

#Region "Slide-Status changed"

    ''' <summary>
    ''' Slide-Status changed
    ''' </summary>
    Private Sub DockPanel_SlideChanged(CurrentSlideState As DockablePanel.SlideStates) Handles dpRight.SlideChanged, dpLeft.SlideChanged
        ' reset the active input control to nothing
        If CurrentSlideState = DockablePanel.SlideStates.SlidOut Then
            Me.ActiveControl = Nothing
        End If
    End Sub

#End Region

#Region "Settings-Panel RIGHT/LEFT - slide in/out"

    ''' <summary>
    ''' Hide on Mouse-Leave.
    ''' </summary>
    Private Sub dpRight_MouseLeave(sender As Object, e As EventArgs) Handles dpRight.MouseLeave_PanelArea
        Me.dpRight.SlideIn()
        Me.pbScanImage.Focus()
    End Sub

    ''' <summary>
    ''' Show on Mouse-Enter.
    ''' </summary>
    Private Sub dpRight_MouseEnter(sender As Object, e As EventArgs) Handles dpRight.MouseEnter_PanelArea, panValueSettings.MouseEnter, lblDataSettings.MouseEnter, tsbtnValueRange.MouseEnter
        Me.dpRight.SlideOut()
    End Sub

    ''' <summary>
    ''' Hide on Mouse-Leave.
    ''' </summary>
    Private Sub dpLeft_MouseLeave(sender As Object, e As EventArgs) Handles dpLeft.MouseLeave_PanelArea
        Me.dpLeft.SlideIn()
        Me.pbScanImage.Focus()
    End Sub

    ''' <summary>
    ''' Show on Mouse-Enter.
    ''' </summary>
    Private Sub dpLeft_MouseEnter(sender As Object, e As EventArgs) Handles dpLeft.MouseEnter_PanelArea, panChannelSettings.MouseEnter, lblStyleSettings.MouseEnter, tsbtnChannelSetup.MouseEnter
        Me.dpLeft.SlideOut()
    End Sub

#End Region

#Region "UI Functions"

    ''' <summary>
    ''' Desired plot quality changed. So replot.
    ''' </summary>
    Private Sub ckbUseHighQualityScaling_CheckedChanged(sender As Object, e As EventArgs) Handles ckbUseHighQualityScaling.CheckedChanged
        Me.RecalculateImage()
    End Sub

    ''' <summary>
    ''' Change the visibility of the scale-bar.
    ''' </summary>
    Private Sub ckbScaleBarVisible_CheckedChanged(sender As Object, e As EventArgs) Handles ckbScaleBarVisible.CheckedChanged
        Me.RecalculateImage()
    End Sub

#End Region

#Region "Data Selection: Properties, Events, and functions."

    ''' <summary>
    ''' List of selected points given in the pixel coordinate frame.
    ''' </summary>
    Public Property SelectedPoints_PixelCoordinate As New List(Of Point)

    ''' <summary>
    ''' New point selected. Also returns the number of the points selected.
    ''' </summary>
    Public Event PointSelected(ByVal P As Point, ByVal PointNumber As Integer)

    ''' <summary>
    ''' If the selection-mode is <code>SelectionModes.LineSelection</code>,
    ''' then this event is fired after selecting two points.
    ''' </summary>
    Public Event LineSelected(ByVal P1 As Point, ByVal P2 As Point)

    ''' <summary>
    ''' If the selection-mode is <code>SelectionModes.AreaSelection</code>,
    ''' then this event is fired after selecting two points.
    ''' </summary>
    Public Event AreaSelected(ByVal P1 As Point, ByVal P2 As Point)

    ''' <summary>
    ''' Selection modes into which the control can be brought.
    ''' </summary>
    Public Enum SelectionModes

        ''' <summary>
        ''' The selection-mode is inactive.
        ''' If set twice to <code>None</code>, the point list will be cleared.
        ''' </summary>
        None

        ''' <summary>
        ''' A single point is selected, and the selection mode is cancelled afterwards.
        ''' </summary>
        SinglePointSelection

        ''' <summary>
        ''' Multiple points are selected, until the program quits the selection-mode,
        ''' or the user presses ESC.
        ''' </summary>
        MultiplePointSelection

        ''' <summary>
        ''' An area is selected by two points. Afterwards the mode is reset to none.
        ''' The <code>AreaSelected</code>-event is fired.
        ''' </summary>
        SingleAreaSelection

        ''' <summary>
        ''' An area is selected by two points. Afterwards the mode is kept active.
        ''' The <code>AreaSelected</code>-event is fired for each two points that are added to the list.
        ''' </summary>
        MultipleAreaSelection

        ''' <summary>
        ''' The selection mode is reset after selecting two points.
        ''' After this, the <code>LineSelected-Event is thrown!</code>
        ''' </summary>
        LineSelection
    End Enum

    Private _SelectionModeLast As SelectionModes = SelectionModes.None
    Private _SelectionMode As SelectionModes = SelectionModes.None
    ''' <summary>
    ''' Current selection mode state of the control.
    ''' On changing the state to something else than "NONE",
    ''' the selected point list WILL BE CLEARED!
    ''' As well will be done, if the selection-mode is set TWICE to "NONE".
    ''' </summary>
    Public Property SelectionMode As SelectionModes
        Get
            Return Me._SelectionMode
        End Get
        Set(value As SelectionModes)
            ' Clear the selected coordinates on changing the selection mode
            ' to something else than "NONE".
            If value <> SelectionModes.None Or
               (value = SelectionModes.None AndAlso Me._SelectionMode = SelectionModes.None) Then
                Me.SelectedPoints_PixelCoordinate.Clear()
            End If

            ' Store the last selection mode as well, if we set the mode back to none.
            If value = SelectionModes.None Then
                Me._SelectionModeLast = Me._SelectionMode
            Else
                Me._SelectionModeLast = value
            End If

            Me._SelectionMode = value
        End Set
    End Property

    ''' <summary>
    ''' A font to draw additional text to the image.
    ''' </summary>
    Private _SelectionMode_Font As New Font("Arial", 10.0F, FontStyle.Regular)

    ''' <summary>
    ''' Pen that is used to draw the selection.
    ''' </summary>
    Private _SelectionModePen_Text As New Pen(Color.White, 2)

    ''' <summary>
    ''' Pen that is used to draw the selection.
    ''' </summary>
    Private _SelectionModePen_Selection As New Pen(Color.Blue, 2)

    ''' <summary>
    ''' Pen that is used to draw the selection.
    ''' </summary>
    Private _SelectionModePen_CrossHair As New Pen(Color.Red, 2)

    ''' <summary>
    ''' Handle mouse information over the scan-image if a selection mode is active.
    ''' </summary>
    Private Sub pbScanImage_MouseClick(sender As Object, e As MouseEventArgs) Handles pbScanImage.MouseClick

        ' Ignore fully, if no selection mode is activated.
        If Me.SelectionMode = SelectionModes.None Then Return

        ' Mouse point
        Dim MousePoint As Point

        ' Just consider normal clicks.
        Select Case e.Button
            Case MouseButtons.Left
                ' Just continue, this is the normal state for selecting points.
                MousePoint = e.Location
            Case MouseButtons.Middle
                ' Remove the last point in the selection list on clicking with the right mouse button.
                If Me.SelectedPoints_PixelCoordinate.Count > 0 Then
                    Me.SelectedPoints_PixelCoordinate.RemoveAt(Me.SelectedPoints_PixelCoordinate.Count - 1)
                End If
                Return
            Case Else
                ' Ignore for all other cases
                Return
        End Select

        ' Now depending on the selection modes raise different events and also quit the selection modes, if necessary.
        Select Case Me.SelectionMode

            Case SelectionModes.SinglePointSelection
                Me.SelectedPoints_PixelCoordinate.Add(MousePoint)
                RaiseEvent PointSelected(MousePoint, Me.SelectedPoints_PixelCoordinate.Count)
                Me.SelectionMode = SelectionModes.None

            Case SelectionModes.MultiplePointSelection
                Me.SelectedPoints_PixelCoordinate.Add(MousePoint)
                RaiseEvent PointSelected(MousePoint, Me.SelectedPoints_PixelCoordinate.Count)

            Case SelectionModes.LineSelection
                Me.SelectedPoints_PixelCoordinate.Add(MousePoint)
                RaiseEvent PointSelected(MousePoint, Me.SelectedPoints_PixelCoordinate.Count)

                ' After two points, the line is fully defined.
                If Me.SelectedPoints_PixelCoordinate.Count = 2 Then

                    ' Raise the line selection event.
                    RaiseEvent LineSelected(Me.SelectedPoints_PixelCoordinate(0), Me.SelectedPoints_PixelCoordinate(1))

                    ' Clear the selection mode
                    Me.SelectionMode = SelectionModes.None

                End If

            Case SelectionModes.SingleAreaSelection
                Me.SelectedPoints_PixelCoordinate.Add(MousePoint)
                RaiseEvent PointSelected(MousePoint, Me.SelectedPoints_PixelCoordinate.Count)

                ' After two points, the area is fully spanned.
                If Me.SelectedPoints_PixelCoordinate.Count = 2 Then

                    ' Raise the area selection event.
                    RaiseEvent AreaSelected(Me.SelectedPoints_PixelCoordinate(0), Me.SelectedPoints_PixelCoordinate(1))

                    ' Clear the selection mode
                    Me.SelectionMode = SelectionModes.None

                End If

            Case SelectionModes.MultipleAreaSelection
                Me.SelectedPoints_PixelCoordinate.Add(MousePoint)
                RaiseEvent PointSelected(MousePoint, Me.SelectedPoints_PixelCoordinate.Count)

                ' If the point-count is dividable by two, a new area is fully spanned.
                If Me.SelectedPoints_PixelCoordinate.Count Mod 2 = 0 Then

                    ' Raise the area selection event with the two last points.
                    RaiseEvent AreaSelected(Me.SelectedPoints_PixelCoordinate(Me.SelectedPoints_PixelCoordinate.Count - 2), Me.SelectedPoints_PixelCoordinate(Me.SelectedPoints_PixelCoordinate.Count - 1))

                End If

        End Select

    End Sub

    ''' <summary>
    ''' Handle mouse information over the scan-image if a selection mode is active.
    ''' </summary>
    Private Sub pbScanImage_MouseMove(sender As Object, e As MouseEventArgs) Handles pbScanImage.MouseMove

        ' Ignore fully, if no selection mode is activated.
        If Me._SelectionModeLast = SelectionModes.None Then Return

        ' Invalidate the drawing if a selection mode is active.
        Me.pbScanImage.Refresh()

    End Sub

    ''' <summary>
    ''' Paint event handler that draws during an active
    ''' selection-mode the selected areas, points or lines to the screen.
    ''' </summary>
    Public Sub pbScanImage_PaintHandler(sender As Object, e As PaintEventArgs) Handles pbScanImage.Paint

        ' Ignore fully, if no selection mode is activated.
        If Me._SelectionModeLast = SelectionModes.None Then Return

        ' Get the graphics surface to draw onto:
        Dim G As Graphics = e.Graphics

        ' Storage for the hint text to plot.
        Dim HintText As String = String.Empty

        ' Now draw depending on the selection modes the selected areas.
        Select Case Me._SelectionModeLast

            Case SelectionModes.SinglePointSelection
                ' Set the hint-text:
                HintText = My.Resources.rScanImageViewer.SelectionModeInfo_SinglePoint

                ' Draw an eventually selected point.
                Dim P1 As Point = Nothing
                If Me.SelectedPoints_PixelCoordinate.Count = 1 Then
                    P1 = Me.SelectedPoints_PixelCoordinate(0)
                    Me.DrawCrosshair(G, P1, False)
                Else
                    ' Set P1 to the mouse position.
                    If Me._SelectionMode <> SelectionModes.None Then
                        P1 = Me.pbScanImage.PointToClient(Cursor.Position)
                        Me.DrawCrosshair(G, P1)
                    End If
                End If

            Case SelectionModes.MultiplePointSelection
                ' Set the hint-text:
                HintText = My.Resources.rScanImageViewer.SelectionModeInfo_MultiplePoints

                ' Draw all selected points
                For i As Integer = 0 To Me.SelectedPoints_PixelCoordinate.Count - 1 Step 1
                    Dim P As Point = Me.SelectedPoints_PixelCoordinate(i)
                    Me.DrawCrosshair(G, P, False)
                Next

                ' Draw always a cross-hair to the mouse position,
                ' if the selection mode is still active.
                If Me._SelectionMode <> SelectionModes.None Then
                    Dim MousePoint As Point = Me.pbScanImage.PointToClient(Cursor.Position)
                    Me.DrawCrosshair(G, MousePoint)
                End If

            Case SelectionModes.SingleAreaSelection
                ' Set the hint-text:
                HintText = My.Resources.rScanImageViewer.SelectionModeInfo_SingleArea

                ' Get the area coordinates between both points
                ' or between the first point and the mouse pointer.
                Dim P1 As Point = Nothing
                Dim P2 As Point = Nothing
                If Me.SelectedPoints_PixelCoordinate.Count = 1 Then
                    P1 = Me.SelectedPoints_PixelCoordinate(0)
                    P2 = Me.pbScanImage.PointToClient(Cursor.Position)
                    ' Draw cross-hair at P2
                    If Me._SelectionMode <> SelectionModes.None Then
                        Me.DrawCrosshair(G, P2)
                    Else
                        P2 = Nothing
                    End If
                ElseIf Me.SelectedPoints_PixelCoordinate.Count = 2 Then
                    P1 = Me.SelectedPoints_PixelCoordinate(0)
                    P2 = Me.SelectedPoints_PixelCoordinate(1)
                Else
                    ' Set P1 to the mouse position.
                    If Me._SelectionMode <> SelectionModes.None Then
                        P1 = Me.pbScanImage.PointToClient(Cursor.Position)
                    End If
                End If

                ' Paint the area, if both points are defined!
                If P2 = Nothing Then
                    ' Draw cross-hair.
                    If Me._SelectionMode <> SelectionModes.None Then
                        Me.DrawCrosshair(G, P1)
                    End If
                Else
                    ' Draw rectangle for current selection.
                    Dim SortedP1P2 As Tuple(Of Point, Point) = cNumericalMethods.GetRectangleCompatiblePoints(P1, P2)
                    P1 = SortedP1P2.Item1
                    P2 = SortedP1P2.Item2
                    G.DrawRectangle(Me._SelectionModePen_Selection, P1.X, P1.Y, P2.X - P1.X, P2.Y - P1.Y)
                End If

            Case SelectionModes.MultipleAreaSelection
                ' Set the hint-text:
                HintText = My.Resources.rScanImageViewer.SelectionModeInfo_MultipleAreas

                Dim P1 As Point = Nothing
                Dim P2 As Point = Nothing

                ' Plot all selected areas so far.
                ' --> IMPORTANT: STEP = 2, because each area is defined by TWO points.
                For i As Integer = 0 To Me.SelectedPoints_PixelCoordinate.Count - 1 Step 2

                    If Me.SelectedPoints_PixelCoordinate.Count Mod 2 = 1 Then
                        If i = Me.SelectedPoints_PixelCoordinate.Count - 1 Then
                            Exit For
                        End If
                    End If

                    P1 = Me.SelectedPoints_PixelCoordinate(i)
                    P2 = Me.SelectedPoints_PixelCoordinate(i + 1)
                    Dim SortedP1P2 As Tuple(Of Point, Point) = cNumericalMethods.GetRectangleCompatiblePoints(P1, P2)
                    P1 = SortedP1P2.Item1
                    P2 = SortedP1P2.Item2
                    G.DrawRectangle(Me._SelectionModePen_Selection, P1.X, P1.Y, P2.X - P1.X, P2.Y - P1.Y)

                Next

                ' Get the area coordinates between both points
                ' or between the first point and the mouse pointer.
                P1 = Nothing
                P2 = Nothing
                If Me.SelectedPoints_PixelCoordinate.Count Mod 2 = 1 Then
                    P1 = Me.SelectedPoints_PixelCoordinate(Me.SelectedPoints_PixelCoordinate.Count - 1)
                    P2 = Me.pbScanImage.PointToClient(Cursor.Position)
                    ' Draw cross-hair at P2
                    If Me._SelectionMode <> SelectionModes.None Then
                        Me.DrawCrosshair(G, P2)
                    Else
                        P2 = Nothing
                    End If
                Else
                    ' Set P1 to the mouse position.
                    If Me._SelectionMode <> SelectionModes.None Then
                        P1 = Me.pbScanImage.PointToClient(Cursor.Position)
                    End If
                End If

                ' Paint the area, if both points are defined!
                If P2 = Nothing Then
                    ' Draw cross-hair.
                    If Me._SelectionMode <> SelectionModes.None Then
                        Me.DrawCrosshair(G, P1)
                    End If
                Else
                    ' Draw rectangle for current selection.
                    Dim SortedP1P2 As Tuple(Of Point, Point) = cNumericalMethods.GetRectangleCompatiblePoints(P1, P2)
                    P1 = SortedP1P2.Item1
                    P2 = SortedP1P2.Item2
                    G.DrawRectangle(Me._SelectionModePen_Selection, P1.X, P1.Y, P2.X - P1.X, P2.Y - P1.Y)
                End If


            Case SelectionModes.LineSelection
                ' Set the hint-text:
                HintText = My.Resources.rScanImageViewer.SelectionModeInfo_Line

                ' Draw a line, between both points,
                ' or between the first point and the mouse pointer.
                Dim P1 As Point = Nothing
                Dim P2 As Point = Nothing
                If Me.SelectedPoints_PixelCoordinate.Count = 1 Then
                    P1 = Me.SelectedPoints_PixelCoordinate(0)
                    P2 = Me.pbScanImage.PointToClient(Cursor.Position)
                    ' Draw cross-hair at P2
                    If Me._SelectionMode <> SelectionModes.None Then
                        Me.DrawCrosshair(G, P2)
                    Else
                        P2 = Nothing
                    End If
                ElseIf Me.SelectedPoints_PixelCoordinate.Count = 2 Then
                    P1 = Me.SelectedPoints_PixelCoordinate(0)
                    P2 = Me.SelectedPoints_PixelCoordinate(1)
                Else
                    ' Set P1 to the mouse position.
                    If Me._SelectionMode <> SelectionModes.None Then
                        P1 = Me.pbScanImage.PointToClient(Cursor.Position)
                    End If
                End If

                ' Paint the line, if both points are defined!
                If P2 = Nothing Then
                    ' Draw cross-hair.
                    If Me._SelectionMode <> SelectionModes.None Then
                        Me.DrawCrosshair(G, P1)
                    End If
                Else
                    ' Draw line
                    G.DrawLine(Me._SelectionModePen_Selection, P1, P2)
                End If

        End Select

        ' Draw the hint-texts, if the selection mode is still active.
        If Me._SelectionMode <> SelectionModes.None Then
            G.DrawString(HintText,
                         Me._SelectionMode_Font,
                         Me._SelectionModePen_Text.Brush,
                         New PointF(0, 0))
            G.DrawString(My.Resources.rScanImageViewer.SelectionModeInfo_AbortHint,
                         Me._SelectionMode_Font,
                         Me._SelectionModePen_Text.Brush,
                         New PointF(0, 12))
        End If

    End Sub

    ''' <summary>
    ''' On ESC aborts the SelectionMode.
    ''' </summary>
    Private Sub pbScanImage_KeyPress(sender As Object, e As KeyEventArgs) Handles pbScanImage.KeyUp
        If e.KeyCode = Keys.Escape Then
            Me.SelectionMode = SelectionModes.None
        End If
    End Sub

    ''' <summary>
    ''' Used to draw a selection crosshair to the graphics
    ''' surface in the scan-image at the given point. Used for data-selection.
    ''' If not set to ForPointSelection just plots a cross-hair with a size of 5% of the image-width,
    ''' and no coordinate information.
    ''' </summary>
    Private Sub DrawCrosshair(ByRef G As Graphics,
                              ByVal P As Point,
                              Optional ForPointSelection As Boolean = True)
        If Me.oScanImagePlot Is Nothing Then Return

        ' Draw cross-hair.
        If ForPointSelection Then
            G.DrawLine(Me._SelectionModePen_CrossHair, P.X, 0, P.X, Me.pbScanImage.Height)
            G.DrawLine(Me._SelectionModePen_CrossHair, 0, P.Y, Me.pbScanImage.Width, P.Y)

            ' Get local coordinate in picture.
            Dim RealCoordinate As cNumericalMethods.Point2D = Me.oScanImagePlot.GetLocationOfScanData(P)
            Dim RealCoordinateFormatted_X As KeyValuePair(Of String, Double) = cUnits.GetPrefix(RealCoordinate.x)
            Dim RealCoordinateFormatted_Y As KeyValuePair(Of String, Double) = cUnits.GetPrefix(RealCoordinate.y)
            Dim RealCoordinateLabel_X As String = My.Resources.rScanImageViewer.SelectionCrossHair_Label.Replace("%cr", RealCoordinateFormatted_X.Value.ToString("N2") & RealCoordinateFormatted_X.Key).Replace("%cp", P.X.ToString("N0"))
            Dim RealCoordinateLabel_Y As String = My.Resources.rScanImageViewer.SelectionCrossHair_Label.Replace("%cr", RealCoordinateFormatted_Y.Value.ToString("N2") & RealCoordinateFormatted_Y.Key).Replace("%cp", P.Y.ToString("N0"))

            ' Draw cross-hair text.
            G.DrawString(RealCoordinateLabel_X, Me._SelectionMode_Font, Me._SelectionModePen_Text.Brush, New PointF(P.X, P.Y - 16))
            G.DrawString(RealCoordinateLabel_Y, Me._SelectionMode_Font, Me._SelectionModePen_Text.Brush, New PointF(0, P.Y))
        Else
            Dim CrossHairSize As Integer = CInt(0.05 * Me.pbScanImage.Width)
            G.DrawLine(Me._SelectionModePen_Selection, P.X, P.Y - CrossHairSize, P.X, P.Y + CrossHairSize)
            G.DrawLine(Me._SelectionModePen_Selection, P.X - CrossHairSize, P.Y, P.X + CrossHairSize, P.Y)
        End If

    End Sub

#End Region


#Region "Extract data from the graph."

    ''' <summary>
    ''' Start the extraction of a line-profile.
    ''' </summary>
    Private Sub btnTool_LineProfile_Click(sender As Object, e As EventArgs) Handles btnTool_LineProfile.Click

        ' Reset initially, if necessary, the selection mode.
        If Me.SelectionMode <> SelectionModes.None Then
            Me.SelectionMode = SelectionModes.None
            Return
        End If

        ' Start the line selection
        Me.SelectionMode = SelectionModes.LineSelection

    End Sub

    ''' <summary>
    ''' If a line was extracted, open the line-profile window with the selected points.
    ''' </summary>
    Private Sub OnLineSelected(ByVal P1 As Point, ByVal P2 As Point) Handles Me.LineSelected
        Dim wPlot As New wScanImageLineProfile
        wPlot.Show(Me.oScanImagePlot, P1, P2)
    End Sub

#End Region

End Class
