Imports System.ComponentModel
Imports System.Threading.Tasks
Imports MathNet.Numerics

Public Class wGridPlotter
    Inherits wFormBase

    Private bReady As Boolean = False

#Region "Properties"

    ''' <summary>
    ''' Reference to the file-list loaded in this window.
    ''' </summary>
    Protected _FileList As cFileImport = Nothing

    ''' <summary>
    ''' Background-Worker for loading the files in the background.
    ''' </summary>
    Protected WithEvents _FileLoader As New BackgroundWorker

    ''' <summary>
    ''' Background-Worker for plotting the grid to the picturebox.
    ''' </summary>
    Protected WithEvents _GridPlotter As New BackgroundWorker

    ''' <summary>
    ''' Contains all file-objects that should be loaded.
    ''' </summary>
    Protected _FileObjectsLoaded_Spectroscopy As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Contains all file-objects that should be loaded.
    ''' </summary>
    Protected _FileObjectsLoaded_ScanImage As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Contains all spectroscopy-tables.
    ''' </summary>
    Protected _SpectroscopyTables As New List(Of cSpectroscopyTable)

    ''' <summary>
    ''' Contains all scan images.
    ''' </summary>
    Protected _ScanImages As New List(Of cScanImage)

    ''' <summary>
    ''' Maximum value that the range selector represents.
    ''' </summary>
    Private _SelectedRange_XMax As Double

    ''' <summary>
    ''' Minimum value that the range selector represents.
    ''' </summary>
    Private _SelectedRange_XMin As Double

    ''' <summary>
    ''' Width of the average window.
    ''' </summary>
    Private _AverageWindowWidth As Double = 0.00005

    ''' <summary>
    ''' Start of the value average window
    ''' </summary>
    Private _AverageWindowStart As Double = 0

    ''' <summary>
    ''' Selected Columns
    ''' </summary>
    Private _SelectedXColumnName As String = String.Empty
    Private _SelectedYColumnName As String = String.Empty

    ''' <summary>
    ''' This array contains the averaged values for each spectrum
    ''' in the <code>Me._SpectroscopyTable</code>-Array
    ''' in the given averaging window.
    ''' </summary>
    Protected _AveragingWindowValues() As Double

    ''' <summary>
    ''' If this variable is set to false, then the grid plotting routine
    ''' will recalculate the average values.
    ''' </summary>
    Protected _AveragingWindowValuesInvalid As Boolean = False

    ''' <summary>
    ''' If this variable is set to true, the value-range selector will update.
    ''' </summary>
    Protected _AveragingWindowValuesWereUpdated As Boolean = False

    ''' <summary>
    ''' Plot point diameter.
    ''' </summary>
    Private _PointDiameter As Double = 0.0000000001

    ''' <summary>
    ''' Show or hide the grid data during the plot.
    ''' </summary>
    Private _ShowGridData As Boolean = True

    ''' <summary>
    ''' Show or hide the topography image during the plot.
    ''' </summary>
    Private _ShowTopography As Boolean = True

    ''' <summary>
    ''' Point-marks to be plotted in the image.
    ''' </summary>
    Private _PointMarkList As New List(Of cScanImagePlot.PointMark)

    ''' <summary>
    ''' Value-range for plotting the grid values.
    ''' </summary>
    Private _GridPlotRangeMax As Double

    ''' <summary>
    ''' Value-range for plotting the grid values.
    ''' </summary>
    Private _GridPlotRangeMin As Double

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Shows the dialog. Hand over the file-buffer to use.
    ''' </summary>
    Public Shadows Function ShowDialog(ByRef FileList As cFileImport) As DialogResult
        Me._FileList = FileList
        Return MyBase.ShowDialog()
    End Function


    ''' <summary>
    ''' Shows the dialog. Hand over the file-buffer to use.
    ''' </summary>
    Public Shadows Sub Show(ByRef FileList As cFileImport)
        Me._FileList = FileList
        MyBase.Show()
    End Sub

    ''' <summary>
    ''' Window constructor.
    ''' </summary>
    Public Sub FormLoaded() Handles MyBase.Load

        ' Set the file-selector contents
        Me.dsSpectroscopyFiles.SetFileList(Me._FileList)
        Me.dsScanImages.SetFileList(Me._FileList)

        ' Setup the background-worker
        Me._FileLoader.WorkerReportsProgress = True
        Me._FileLoader.WorkerSupportsCancellation = True
        Me._GridPlotter.WorkerReportsProgress = True

        ' Set the initial values
        Me.txtPlotSettings_PointDiameter.SetValue(Me._PointDiameter,, False)
        Me.txtValueRangeStart.SetValue(Me._AverageWindowStart,, False)
        Me.txtValueRangeWindow.SetValue(Me._AverageWindowWidth,, False)

        ' Load the last settings
        With My.Settings
            Me._SelectedXColumnName = .GridPlotter_SelectedXColumnName
            Me._SelectedYColumnName = .GridPlotter_SelectedYColumnName
        End With

        ' Set the parents of the pictureboxes for transparency.
        Me.svOutputImage.Parent = Me.gbOutput

        Me.bReady = True

    End Sub

#End Region

#Region "Destructor"

    ''' <summary>
    ''' Function of the Form-Closing Event.
    ''' Abort the Closing, if the File-Buffer is fetching!
    ''' </summary>
    Private Sub CheckBeforeFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me._FileLoader.IsBusy Or Me._GridPlotter.IsBusy Then
            MessageBox.Show(My.Resources.rGridPlotter.WindowClosing_FetchInProgress,
                            My.Resources.rGridPlotter.WindowClosing_FetchInProgress_title,
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            e.Cancel = True
        End If
    End Sub

#End Region

#Region "Selected Files Changed"

    ''' <summary>
    ''' Enable or disable the loading button.
    ''' </summary>
    Private Sub dsSpectroscopyFiles_SelectionChanged(ByRef SelectedFileObjects As Dictionary(Of String, cFileObject)) Handles dsSpectroscopyFiles.SelectionChanged, dsScanImages.SelectionChanged
        If dsSpectroscopyFiles.SelectedEntries.Count > 0 AndAlso dsScanImages.SelectedEntries.Count > 0 Then
            Me.btnLoadData.Enabled = True
        Else
            Me.btnLoadData.Enabled = False
        End If
    End Sub

#End Region

#Region "File loading"

    ''' <summary>
    ''' Load the selected spectroscopy-files.
    ''' </summary>
    Private Sub btnLoadDataSet_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click

        If Me._FileLoader.IsBusy Then Return

        ' Copy the references of the file-objects to the list of file-objects to load.
        Me._FileObjectsLoaded_Spectroscopy = Me.dsSpectroscopyFiles.SelectedEntries()
        Me._FileObjectsLoaded_ScanImage = Me.dsScanImages.SelectedEntries()

        ' Disable buttons.
        ActivateLoadingButton(False)

        ' Start the background-worker.
        Me._FileLoader.RunWorkerAsync()

    End Sub

    ''' <summary>
    ''' Enables of disables the interface to load something.
    ''' </summary>
    Protected Sub ActivateLoadingButton(ByVal Active As Boolean)
        Me.btnLoadData.Enabled = Active
        Me.dsSpectroscopyFiles.Enabled = Active
        Me.dsScanImages.Enabled = Active
        Me.gbDataRange.Enabled = Active
    End Sub

    ''' <summary>
    ''' Loads the FileObjects located in the FileObjectToLoad dictionary.
    ''' </summary>
    Protected Sub LoadSelectedDataSet() Handles _FileLoader.DoWork

        ' Clear the cache.
        Me._ScanImages.Clear()
        Me._SpectroscopyTables.Clear()

        ' Set the length of the list at once, if it is too small.
        If Me._SpectroscopyTables.Capacity < Me._FileObjectsLoaded_Spectroscopy.Count Then
            Me._SpectroscopyTables.Capacity = Me._FileObjectsLoaded_Spectroscopy.Count
        End If
        If Me._ScanImages.Capacity < Me._FileObjectsLoaded_ScanImage.Count Then
            Me._ScanImages.Capacity = Me._FileObjectsLoaded_ScanImage.Count
        End If

        ' Variables for the progress calculation.
        Dim ProgressPercentage As Double = 0
        Dim ProgressPercentageIncrement As Double = 0

        ProgressPercentageIncrement = 90 / (Me._FileObjectsLoaded_Spectroscopy.Count + 1)
        For Each SpectroscopyFOKV As KeyValuePair(Of String, cFileObject) In Me._FileObjectsLoaded_Spectroscopy

            ' Report Progress
            ProgressPercentage += ProgressPercentageIncrement
            Me._FileLoader.ReportProgress(CInt(ProgressPercentage), My.Resources.rGridPlotter.FetchProgress.Replace("%f", SpectroscopyFOKV.Value.FileNameWithoutPath).Replace("%p", ProgressPercentage.ToString("N2")))

            ' Load the Spectroscopy-Table
            Dim SpectroscopyTable As cSpectroscopyTable = Nothing
            cFileImport.GetSpectroscopyFile(SpectroscopyFOKV.Value, SpectroscopyTable, False)

            ' Add the table to the cache.
            If Not SpectroscopyTable Is Nothing Then
                Me._SpectroscopyTables.Add(SpectroscopyTable)
            End If

        Next

        ProgressPercentageIncrement = 99 / (Me._FileObjectsLoaded_ScanImage.Count + 1)
        For Each ScanFOKV As KeyValuePair(Of String, cFileObject) In Me._FileObjectsLoaded_ScanImage

            ' Report Progress
            ProgressPercentage += ProgressPercentageIncrement
            Me._FileLoader.ReportProgress(CInt(ProgressPercentage), My.Resources.rGridPlotter.FetchProgress.Replace("%f", ScanFOKV.Value.FileNameWithoutPath).Replace("%p", ProgressPercentage.ToString("N2")))

            ' Load the ScanImage
            Dim ScanImage As cScanImage = Nothing
            cFileImport.GetScanImageFile(ScanFOKV.Value, ScanImage, False)

            ' Add the table to the cache.
            If Not ScanImage Is Nothing Then
                Me._ScanImages.Add(ScanImage)
            End If

        Next

        ' Hide the progress bars
        Me._FileLoader.ReportProgress(-1, String.Empty)

    End Sub

    Private Delegate Sub SelectedDataSetLoadedDelegate()
    ''' <summary>
    ''' Work finished! So call now the data plotting on the GUI-thread.
    ''' </summary>
    Protected Sub SelectedDataSetLoaded() Handles _FileLoader.RunWorkerCompleted
        If Me.btnLoadData.InvokeRequired Then
            Me.btnLoadData.Invoke(New SelectedDataSetLoadedDelegate(AddressOf SelectedDataSetLoaded))
        Else
            ' Enable buttons.
            ActivateLoadingButton(True)
            Me.DrawDatasetAfterLoading()
        End If
    End Sub

    ''' <summary>
    ''' Draw the loaded data-set to the screen.
    ''' </summary>
    Protected Sub DrawDatasetAfterLoading()

        '####################
        ' SPECTROSCOPY-TABLE
        ' Set the content of the data-selection.
        Me.spDataRangeSelector_DataToDisplay.InitializeColumns(Me._SpectroscopyTables)

        '####################
        ' SCAN-IMAGES
        Me.svOutputImage.SetScanImageObjects(Me._ScanImages)

        ' Plot the averaging window to the graph
        Me.PlotAveragingWindowToSpectroscopyPlot()
    End Sub

    ''' <summary>
    ''' Data to display in the preview box has changed. This is introduced to not plot
    ''' thousands of data in the preview box, but be able to selectively plot some spectra
    ''' for the data selection.
    ''' </summary>
    Private Sub spDataRangeSelector_DataToDisplay_SelectionChanged() Handles spDataRangeSelector_DataToDisplay.SelectedIndexChanged

        ' Add the spectroscopy-tables to the list, after clearing the list so far.
        Me.pbDataRangeSelector.ClearSpectroscopyTables()

        ' Get the selected entries
        Dim SelectedEntries As List(Of String) = Me.spDataRangeSelector_DataToDisplay.SelectedEntries

        Dim SelectedSpectroscopyTables As New List(Of cSpectroscopyTable)(SelectedEntries.Count)
        ' Plot the selected entries.
        For Each Name As String In SelectedEntries
            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1
                If Me._SpectroscopyTables(i).FileNameWithoutPath = Name Then
                    SelectedSpectroscopyTables.Add(Me._SpectroscopyTables(i))
                    Exit For
                End If
            Next
        Next

        ' Plot the selected dables
        Me.bReady = False
        Me.pbDataRangeSelector.AddSpectroscopyTables(SelectedSpectroscopyTables)
        Me.pbDataRangeSelector.cbX.SelectedColumnName = Me._SelectedXColumnName
        Me.pbDataRangeSelector.cbY.SelectedColumnName = Me._SelectedYColumnName
        Me.bReady = True

    End Sub

#End Region

#Region "Data Range Selection"

    ''' <summary>
    ''' Selected Data changed, so change all related settings.
    ''' </summary>
    Private Sub SelectedSpectroscopyDataChanged() Handles pbDataRangeSelector.SelectedIndexChanged
        If Not Me.bReady Then Return

        ' Get the X-Column values.
        Dim XColumnName As String = Me.pbDataRangeSelector.cbX.SelectedColumnName
        Dim YColumnName As String = Me.pbDataRangeSelector.cbY.SelectedColumnName

        ' Check, if we got a correct name, so if we selected a valid data-set.
        If XColumnName = String.Empty Then Return
        If YColumnName = String.Empty Then Return

        ' Get the Max and Min-Values of the XColumn.
        Me._SelectedRange_XMax = Double.MinValue
        Me._SelectedRange_XMin = Double.MaxValue
        For Each S As cSpectroscopyTable In Me._SpectroscopyTables
            If Not S.ColumnExists(XColumnName) Then Return
            Me._SelectedRange_XMax = Math.Max(S.Column(XColumnName).GetMaximumValueOfColumn, Me._SelectedRange_XMax)
            Me._SelectedRange_XMin = Math.Min(S.Column(XColumnName).GetMinimumValueOfColumn, Me._SelectedRange_XMin)
        Next

        Me._SelectedXColumnName = XColumnName
        Me._SelectedYColumnName = YColumnName
        My.Settings.GridPlotter_SelectedXColumnName = Me._SelectedXColumnName
        My.Settings.GridPlotter_SelectedYColumnName = Me._SelectedYColumnName

        ' Set the value-range window to a certain size (100 steps between Max and Min)
        Me.SetAverageWindowWidth((Me._SelectedRange_XMax - Me._SelectedRange_XMin) / 100, False)

        ' Set the average window start to the lower limit
        Me.SetAverageWindowStart(Me._SelectedRange_XMin + (Me._SelectedRange_XMax - Me._SelectedRange_XMin) / 2, True)

    End Sub

    ''' <summary>
    ''' If the average range changes, then also change the trackbar steps.
    ''' </summary>
    Private Sub txtValueRangeWindow_TextChanged(ByRef NT As NumericTextbox) Handles txtValueRangeWindow.ValidValueChanged
        Me.SetAverageWindowWidth(NT.DecimalValue)
    End Sub

    ''' <summary>
    ''' Start-Point of the averaging window changed.
    ''' </summary>
    Private Sub txtValueRangeStart_TextChanged(ByRef NT As NumericTextbox) Handles txtValueRangeStart.ValidValueChanged
        Me.SetAverageWindowStart(NT.DecimalValue)
    End Sub

    ''' <summary>
    ''' Sets the Average-window value, and the value of the textbox.
    ''' </summary>
    Public Sub SetAverageWindowStart(ByVal Value As Double,
                                     Optional ByVal UpdatePlot As Boolean = True)
        ' Get the average window width
        Me._AverageWindowStart = Value
        If Me.txtValueRangeStart.DecimalValue <> Value Then Me.txtValueRangeStart.SetValue(Value,, False)

        ' Plot the highlight range
        If UpdatePlot Then Me.PlotAveragingWindowToSpectroscopyPlot()
    End Sub

    ''' <summary>
    ''' Sets the Average-window value, and the value of the textbox.
    ''' </summary>
    Public Sub SetAverageWindowWidth(ByVal Value As Double,
                                     Optional ByVal UpdatePlot As Boolean = True)
        If Value = 0 Then Return

        ' Get the average window width
        Me._AverageWindowWidth = Value
        If Me.txtValueRangeWindow.DecimalValue <> Value Then Me.txtValueRangeWindow.SetValue(Value,, False)

        ' Plot the highlight range
        If UpdatePlot Then Me.PlotAveragingWindowToSpectroscopyPlot()
    End Sub

    ''' <summary>
    ''' Via the selection area in the graph the averaging window can be repositioned!
    ''' </summary>
    Private Sub AveragingWindowCenterSelected(XValue As Double) Handles pbDataRangeSelector.PointSelectionChanged_XValue
        Me.SetAverageWindowStart(XValue - (Me._AverageWindowWidth / 2))
    End Sub

    ''' <summary>
    ''' Via the selection area in the graph the averaging window can be repositioned!
    ''' </summary>
    Private Sub AveragingWindowWidthSelected(LeftValue As Double, RightValue As Double) Handles pbDataRangeSelector.PointSelectionChanged_XRange
        Me.SetAverageWindowWidth(RightValue - LeftValue, False)
        Me.SetAverageWindowStart(LeftValue, True)
    End Sub

    ''' <summary>
    ''' Plots the average window in the spectroscopy table.
    ''' </summary>
    Private Sub PlotAveragingWindowToSpectroscopyPlot()

        ' Mark the range in the plot
        Me.pbDataRangeSelector.ClearHighlightRanges()
        Me.pbDataRangeSelector.AddHighlightRange(mSpectroscopyTableViewer.SelectionModes.XRange,
                                                 New ZedGraph.PointPair(Me._AverageWindowStart, 0),
                                                 New ZedGraph.PointPair(Me._AverageWindowStart + Me._AverageWindowWidth, 0))

        ' Mark the values as invalid, which allows to recalculate the value-array.
        Me._AveragingWindowValuesInvalid = True

        ' Restart the plotting
        Me.PlotGrid()

    End Sub

    ''' <summary>
    ''' Select the average window range.
    ''' </summary>
    Private Sub btnSelectValueRangeWindow_Click(sender As Object, e As EventArgs) Handles btnSelectValueRangeWindow.Click
        ' Set the X-selection mode.
        With Me.pbDataRangeSelector
            If .PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XRange Then
                .PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.None
            Else
                .PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XRange
            End If
        End With
    End Sub

    ''' <summary>
    ''' Select the average window start.
    ''' </summary>
    Private Sub btnSelectValueRangeStart_Click(sender As Object, e As EventArgs) Handles btnSelectValueRangeStart.Click
        ' Set the X-selection mode.
        With Me.pbDataRangeSelector
            If .PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XValue Then
                .PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.None
            Else
                .PointSelectionMode = mSpectroscopyTableViewer.SelectionModes.XValue
            End If
        End With
    End Sub

    ''' <summary>
    ''' Selection mode changed, so reset the appropriate buttons and their texts.
    ''' </summary>
    Private Sub AveragingWindowSelectionModeChanged(ByVal SelectionMode As mSpectroscopyTableViewer.SelectionModes) Handles pbDataRangeSelector.PointSelectionModeChanged

        With Me.pbDataRangeSelector
            Select Case .PointSelectionMode
                Case mSpectroscopyTableViewer.SelectionModes.XRange
                    Me.btnSelectValueRangeWindow.Text = My.Resources.rGridPlotter.AveragingWindowSelector_CancelSelect
                    Me.btnSelectValueRangeWindow.BackColor = Color.Red
                    Me.btnSelectValueRangeStart.Text = My.Resources.rGridPlotter.AveragingWindowSelector_Select
                    Me.btnSelectValueRangeStart.BackColor = Nothing
                Case mSpectroscopyTableViewer.SelectionModes.XValue
                    Me.btnSelectValueRangeWindow.Text = My.Resources.rGridPlotter.AveragingWindowSelector_Select
                    Me.btnSelectValueRangeWindow.BackColor = Nothing
                    Me.btnSelectValueRangeStart.Text = My.Resources.rGridPlotter.AveragingWindowSelector_CancelSelect
                    Me.btnSelectValueRangeStart.BackColor = Color.Red
                Case mSpectroscopyTableViewer.SelectionModes.None
                    Me.btnSelectValueRangeWindow.Text = My.Resources.rGridPlotter.AveragingWindowSelector_Select
                    Me.btnSelectValueRangeWindow.BackColor = Nothing
                    Me.btnSelectValueRangeStart.Text = My.Resources.rGridPlotter.AveragingWindowSelector_Select
                    Me.btnSelectValueRangeStart.BackColor = Nothing
            End Select
        End With
    End Sub

#End Region

#Region "Averaging Value calculation"

    ''' <summary>
    ''' Fills the array <code>Me._AveragingWindowValues</code> with a <code>Parallel.For</code> loop.
    ''' Sums up all the values in the given averaging window, and stores these values in the array.
    ''' </summary>
    Protected Sub CalculateAveragingWindowValue()

        ' Adjust the length of the value array if necessary,
        ' and if so, then mark the array as invalid.
        If Me._AveragingWindowValues Is Nothing Then
            ReDim Me._AveragingWindowValues(Me._SpectroscopyTables.Count - 1)
            Me._AveragingWindowValuesInvalid = True
        ElseIf Me._SpectroscopyTables.Count <> Me._AveragingWindowValues.Length Then
            ReDim Me._AveragingWindowValues(Me._SpectroscopyTables.Count - 1)
            Me._AveragingWindowValuesInvalid = True
        End If

        ' Sum up all values in the array in parallel.
        If Me._AveragingWindowValuesInvalid Then
            Parallel.For(0, Me._SpectroscopyTables.Count, AddressOf Me.AverageValuesInWindow)
            Me._AveragingWindowValuesWereUpdated = True
        Else
            Me._AveragingWindowValuesWereUpdated = False
        End If

        ' Mark the array as valid
        Me._AveragingWindowValuesInvalid = False

    End Sub

    ''' <summary>
    ''' Function that takes the given Spectroscopy-Table and sums the values
    ''' defined in the averaging window.
    ''' </summary>
    Private Sub AverageValuesInWindow(ByVal SpectroscopyTableIndex As Integer)

        ' Security checks
        If Me._SpectroscopyTables(SpectroscopyTableIndex) Is Nothing Then Return
        If Not Me._SpectroscopyTables(SpectroscopyTableIndex).ColumnExists(Me._SelectedXColumnName) Then Return
        If Not Me._SpectroscopyTables(SpectroscopyTableIndex).ColumnExists(Me._SelectedYColumnName) Then Return

        ' Get references to the value arrays.
        Dim XValues As ReadOnlyCollection(Of Double) = Me._SpectroscopyTables(SpectroscopyTableIndex).Column(Me._SelectedXColumnName).Values
        Dim YValues As ReadOnlyCollection(Of Double) = Me._SpectroscopyTables(SpectroscopyTableIndex).Column(Me._SelectedYColumnName).Values
        Dim AverageWindowEnd As Double = Me._AverageWindowStart + Me._AverageWindowWidth

        Dim Sum As Double = 0
        Dim AveragingCounter As Integer = 0
        For i As Integer = 0 To XValues.Count - 1 Step 1

            If Not Double.IsNaN(XValues(i)) Then

                If XValues(i) >= Me._AverageWindowStart And XValues(i) <= AverageWindowEnd Then

                    Sum += YValues(i)
                    AveragingCounter += 1

                End If

            End If

        Next

        ' Calculate the average value, and store it in the array.
        If AveragingCounter > 0 Then
            Me._AveragingWindowValues(SpectroscopyTableIndex) = Sum / AveragingCounter
        Else
            Me._AveragingWindowValues(SpectroscopyTableIndex) = 0
        End If

    End Sub

#End Region

#Region "Grid plotting"

    ''' <summary>
    ''' Starts the plotting of the grid in an asynchronous way.
    ''' </summary>
    Public Sub PlotGrid()
        If Not Me.bReady Then Return

        ' Check if a plot is still running.
        If Me._GridPlotter.IsBusy Then Return

        '' Deactivate the plot settings box
        'Me.gbPlotSettings.Enabled = False
        'Me.gbExport.Enabled = False

        ' Start the plotting.
        Me._GridPlotter.RunWorkerAsync()

    End Sub

    ''' <summary>
    ''' Plots the grid in the background-worker.
    ''' </summary>
    Private Sub PlotGridAsync() Handles _GridPlotter.DoWork

        Try

            Me._GridPlotter.ReportProgress(5, My.Resources.rGridPlotter.PlotProgress_CalculateAverageValuesOfAveragingWindow)
            ' Start to count up and average all values in the selected range,
            ' if the value array is marked as invalid.
            Me.CalculateAveragingWindowValue()

            ' Get the maximum and minimum-values of the averaging window,
            ' if the averaging window changed.
            If Me._AveragingWindowValuesWereUpdated Then
                Me._GridPlotRangeMax = cNumericalMethods.GetMaximumValue(Me._AveragingWindowValues)
                Me._GridPlotRangeMin = cNumericalMethods.GetMinimumValue(Me._AveragingWindowValues)
            End If

            ' Remove all old point marks
            Me._PointMarkList.Clear()

            ' Get the size of the points.
            Dim PointRadius As Double = Me._PointDiameter / 2

            ' Get the color-scheme colors
            Dim BrushArray As SolidBrush() = Me.cpPlotSettings_ColorCode.GetSelectedColorScheme.BrushArray

            ' Now calculate the settings for all datapoints.
            Dim Progress As Double = 10
            Dim ProgressProgress As Double = (80 / (Me._SpectroscopyTables.Count + 1))
            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1

                ' Report progress
                Me._GridPlotter.ReportProgress(CInt(Progress), My.Resources.rGridPlotter.PlotProgress_PlottingDataPoint.Replace("%f", Me._SpectroscopyTables(i).FileNameWithoutPath))
                Progress += ProgressProgress

                ' Get the color of the point-mark.
                Dim PointColorBrush As SolidBrush = cColorScheme.GetPlotColorFromColorScale(Me._GridPlotRangeMax,
                                                                                            Me._GridPlotRangeMin,
                                                                                            Me._AveragingWindowValues(i),
                                                                                            BrushArray)

                ' Check, if the color exists!
                If PointColorBrush Is Nothing Then Continue For

                ' Create new point-mark.
                Dim NewPointMark As New cScanImagePlot.PointMark(Me._SpectroscopyTables(i).Location_X,
                                                                 Me._SpectroscopyTables(i).Location_Y,
                                                                 Me._SpectroscopyTables(i).Location_Z,
                                                                 PointRadius,
                                                                 PointColorBrush.Color,
                                                                 cScanImagePlot.PointMark.PointMarkShapes.CircleFilled)
                Me._PointMarkList.Add(NewPointMark)

            Next

            '#######################################
            ' !!! OLD !!! JUST DRAWED THE GRID!!!
            '#######################################
            'Me._GridPlotter.ReportProgress(10, My.Resources.rGridPlotter.PlotProgress_GettingGridDimensions)
            '' First we need to calculate the matrix dimensions for the values from
            '' the locations of the spectra.
            'Dim SpectraLocation_XMax As Double = Double.MinValue
            'Dim SpectraLocation_XMin As Double = Double.MaxValue
            'Dim SpectraLocation_YMax As Double = Double.MinValue
            'Dim SpectraLocation_YMin As Double = Double.MaxValue

            '' Go through all spectra, and the the most extremal locations.
            'For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1

            '    SpectraLocation_XMax = Math.Max(Me._SpectroscopyTables(i).Location_X, SpectraLocation_XMax)
            '    SpectraLocation_XMin = Math.Min(Me._SpectroscopyTables(i).Location_X, SpectraLocation_XMin)
            '    SpectraLocation_YMax = Math.Max(Me._SpectroscopyTables(i).Location_Y, SpectraLocation_YMax)
            '    SpectraLocation_YMin = Math.Min(Me._SpectroscopyTables(i).Location_Y, SpectraLocation_YMin)

            'Next

            '' Define the plot properties.
            'Dim PointRadius As Double = Me._PointDiameter / 2

            '' Get the grid dimensions
            'Dim GridDimensionsInScale_Width As Double = (SpectraLocation_XMax - SpectraLocation_XMin) + Me._PointDiameter
            'Dim GridDimensionsInScale_Height As Double = (SpectraLocation_YMax - SpectraLocation_YMin) + Me._PointDiameter

            'Dim ScanImage_XMax As Double = Double.MinValue
            'Dim ScanImage_XMin As Double = Double.MaxValue
            'Dim ScanImage_YMax As Double = Double.MinValue
            'Dim ScanImage_YMin As Double = Double.MaxValue
            '' Now get the topography-image, and compare the dimensions.
            '' Take the LARGER ones to plot the image!
            'For Each ScanImage As cScanImage In Me._ScanImages

            'Next

            '' Now, that we have the grid dimensions, we can create a value matrix initially filled with Double.NaN values,
            '' into which we "paint" the values. This matrix is then plotted.
            'Dim ImageWidth As Integer = Me.pbOutputGrid.Width
            'Dim ImageHeight As Integer = Me.pbOutputGrid.Height
            'If ImageWidth <= 0 Or ImageHeight <= 0 Then Return

            'Dim ScalePerPixel_X As Double = GridDimensionsInScale_Width / ImageWidth
            'Dim ScalePerPixel_Y As Double = GridDimensionsInScale_Height / ImageHeight
            'If ScalePerPixel_X <= 0 Or ScalePerPixel_Y <= 0 Then Return

            '' Get the plot circle radius
            'Dim CircleRadiusInPixels_X As Integer = CInt(PointRadius / ScalePerPixel_X)
            'Dim CircleRadiusInPixels_Y As Integer = CInt(PointRadius / ScalePerPixel_Y)

            '' First we create a matrix for the values at the given plot positions.
            'Dim PlotMatrix As LinearAlgebra.Double.DenseMatrix = LinearAlgebra.Double.DenseMatrix.Create(ImageHeight, ImageWidth, Double.NaN)

            ''Dim MaxValue As Double = Double.MinValue
            ''Dim MinValue As Double = Double.MaxValue

            '' Now fill the matrix at the given spots
            'Dim Progress As Double = 10
            'Dim ProgressProgress As Double = (80 / (Me._SpectroscopyTables.Count + 1))
            'For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1

            '    ' Report progress
            '    Me._GridPlotter.ReportProgress(CInt(Progress), My.Resources.rGridPlotter.PlotProgress_PlottingDataPoint.Replace("%f", Me._SpectroscopyTables(i).FileNameWithoutPath))
            '    Progress += ProgressProgress

            '    Dim PlotPointLocation_X As Integer = -1
            '    Dim PlotPointLocation_Y As Integer = -1

            '    ' Get the center location of the spectrum
            '    PlotPointLocation_X = CInt((Me._SpectroscopyTables(i).Location_X - SpectraLocation_XMin + PointRadius) / ScalePerPixel_X)
            '    PlotPointLocation_Y = CInt((Me._SpectroscopyTables(i).Location_Y - SpectraLocation_YMin + PointRadius) / ScalePerPixel_Y)

            '    If PlotPointLocation_X >= 0 And PlotPointLocation_X < PlotMatrix.ColumnCount And
            '       PlotPointLocation_Y >= 0 And PlotPointLocation_Y < PlotMatrix.RowCount Then

            '        'MaxValue = Math.Max(Me._AveragingWindowValues(i), MaxValue)
            '        'MinValue = Math.Min(Me._AveragingWindowValues(i), MinValue)

            '        ' Change the central location of the matrix, exactly where the spectrum is located.
            '        ' Average with already existing values.
            '        If Double.IsNaN(PlotMatrix(PlotPointLocation_Y, PlotPointLocation_X)) Then
            '            PlotMatrix(PlotPointLocation_Y, PlotPointLocation_X) = Me._AveragingWindowValues(i)
            '        Else
            '            PlotMatrix(PlotPointLocation_Y, PlotPointLocation_X) = (PlotMatrix(PlotPointLocation_Y, PlotPointLocation_X) + Me._AveragingWindowValues(i)) / 2
            '        End If

            '        ' Temporary variables
            '        Dim RadiusFromLocation As Double
            '        Dim CircleCoordinateX As Integer
            '        Dim CircleCoordinateY As Integer

            '        ' Change the values in a circle around the location, by averaging them with already existing values.
            '        For x As Integer = -CircleRadiusInPixels_X To CircleRadiusInPixels_X Step 1
            '            For y As Integer = -CircleRadiusInPixels_Y To CircleRadiusInPixels_Y Step 1

            '                ' Calculate the current point's radius
            '                RadiusFromLocation = Math.Sqrt(x * x * ScalePerPixel_X * ScalePerPixel_X + y * y * ScalePerPixel_Y * ScalePerPixel_Y)

            '                ' Plot a circle, so ignore values outside this radius
            '                If RadiusFromLocation > PointRadius Then Continue For

            '                CircleCoordinateX = PlotPointLocation_X + x
            '                CircleCoordinateY = PlotPointLocation_Y + y

            '                ' Now enter the values in the circle to the matrix.
            '                If CircleCoordinateX >= 0 And CircleCoordinateX < PlotMatrix.ColumnCount And
            '                   CircleCoordinateY >= 0 And CircleCoordinateY < PlotMatrix.RowCount Then

            '                    If Double.IsNaN(PlotMatrix(CircleCoordinateY, CircleCoordinateX)) Then
            '                        PlotMatrix(CircleCoordinateY, CircleCoordinateX) = Me._AveragingWindowValues(i)
            '                    Else
            '                        PlotMatrix(CircleCoordinateY, CircleCoordinateX) = (PlotMatrix(CircleCoordinateY, CircleCoordinateX) + Me._AveragingWindowValues(i)) / 2
            '                    End If

            '                End If

            '            Next
            '        Next

            '    End If

            'Next

            '' Now we create a 2D-Plot of these values.
            'Dim Plot As New cContourPlot(PlotMatrix)

            '' Use the given Color-Scheme
            'Plot.ColorScheme = Me.cpPlotSettings_ColorCode.GetSelectedColorScheme

            '' Save the output image.
            ''Me._OutputImage = Plot.Plot2D(MaxValue, MinValue)
            'Me._OutputImage = Plot.Plot2D(Me.vrsPlotSettings_ColorScaling.SelectedMinValue, Me.vrsPlotSettings_ColorScaling.SelectedMaxValue)


        Catch ex As Exception
            Debug.WriteLine("wGridPlotter->GridPlot()-error: " & ex.Message)
        End Try

        ' Reset the progress
        Me._GridPlotter.ReportProgress(-1, "-")

    End Sub

    ''' <summary>
    ''' The grid has been plotted successfully.
    ''' </summary>
    Private Sub PlotGridAsyncFinished() Handles _GridPlotter.RunWorkerCompleted

        ' Plot the output image in the picturebox.
        Me.SetOutputImageThreadSafe()

        '' Activate the plot settings box
        'Me.gbPlotSettings.Enabled = True
        'Me.gbExport.Enabled = True

        ' Unmark the newly set averaging window values.
        Me._AveragingWindowValuesWereUpdated = False

    End Sub

    Private Delegate Sub SetOutputImageThreadSafeDelegate()
    ''' <summary>
    ''' Sets the output image to the picturebox in a thread-safe way.
    ''' </summary>
    Private Sub SetOutputImageThreadSafe()
        If Me.svOutputImage.InvokeRequired Then
            Me.svOutputImage.Invoke(New SetOutputImageThreadSafeDelegate(AddressOf Me.SetOutputImageThreadSafe))
        Else

            '' Set Point-Marks in the Preview-Window of the ScanImage,
            '' if the selected Image contains the selected Spectroscopy-Files:
            With Me.svOutputImage
                .ClearPointMarkList()
                .AddPointMark(Me._PointMarkList)
                .RecalculateImage()
            End With

            ' Set the value range of the output image to the value range selector
            If Me._AveragingWindowValuesWereUpdated Then
                Me.vrsPlotSettings_ColorScaling.SetValueArray(Me._AveragingWindowValues)
            End If

        End If
    End Sub

#End Region

#Region "Plot Grid Settings"

    ''' <summary>
    ''' Change the plot point diameter.
    ''' </summary>
    Private Sub txtPlotSettings_PointDiameter_TextChanged(ByRef NT As NumericTextbox) Handles txtPlotSettings_PointDiameter.ValidValueChanged
        Me._PointDiameter = Me.txtPlotSettings_PointDiameter.DecimalValue
        Me.PlotGrid()
    End Sub

#End Region

#Region "Manage the status-bar to report progress and messages."

    ''' <summary>
    ''' Report progress.
    ''' </summary>
    Protected Sub WorkerProgress(sender As Object, e As ProgressChangedEventArgs) Handles _FileLoader.ProgressChanged, _GridPlotter.ProgressChanged
        If e.ProgressPercentage <= 100 And e.ProgressPercentage >= 0 Then
            If Not Me.pgProgress.Visible Then Me.pgProgress.Visible = True
            If Not Me.lblProgress.Visible Then Me.lblProgress.Visible = True

            Me.pgProgress.Value = e.ProgressPercentage
            Me.lblProgress.Text = e.UserState.ToString
        Else
            Me.pgProgress.Visible = False
            Me.lblProgress.Visible = False
        End If
    End Sub

#End Region

#Region "Scan Image changed"

    ''' <summary>
    ''' Change the plotted scan image.
    ''' </summary>
    Private Sub scTopographyPlot_ScanChannel_SelectionChanged()
        Me.PlotGrid()
    End Sub

#End Region

#Region "Plot-Range and color-code changed"

    ''' <summary>
    ''' Color-scale of the plotted grid changed by the user.
    ''' </summary>
    Private Sub ColorCodesChanged(NewColorScheme As cColorScheme) Handles cpPlotSettings_ColorCode.SelectedColorSchemaChanged
        Me.PlotGrid()
    End Sub

    ''' <summary>
    ''' Plot-Range of the plotted grid changed by the user.
    ''' </summary>
    Private Sub ColorScalingChanged() Handles vrsPlotSettings_ColorScaling.SelectedRangeChanged
        Me._GridPlotRangeMax = Me.vrsPlotSettings_ColorScaling.SelectedMaxValue
        Me._GridPlotRangeMin = Me.vrsPlotSettings_ColorScaling.SelectedMinValue
        Me.PlotGrid()
    End Sub

#End Region

#Region "Export"

    ''' <summary>
    ''' Export the averaging window to a XYZ-file.
    ''' </summary>
    Private Sub btnExport_SaveAsXYZ_Click(sender As Object, e As EventArgs) Handles btnExport_SaveAsXYZ.Click

        Dim fs As New SaveFileDialog
        With fs
            .InitialDirectory = My.Settings.LastExport_Path
            .Title = My.Resources.Title_SaveImage
            .FileName = "grid.xyz"
            .Filter = "xyz coordinate file|*.xyz"
            If .ShowDialog = DialogResult.OK Then

                Try
                    Dim SB As New Text.StringBuilder
                    Select Case .FilterIndex
                        Case 1
                            ' Write headers
                            SB.Append("x")
                            SB.Append(vbTab)
                            SB.Append("y")
                            SB.Append(vbTab)
                            SB.Append("spectral intensity")
                            SB.Append(vbNewLine)
                            ' Create the XYZ-file from the averaging window values.
                            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1
                                SB.Append(Me._SpectroscopyTables(i).Location_X.ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                                SB.Append(vbTab)
                                SB.Append(Me._SpectroscopyTables(i).Location_Y.ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                                SB.Append(vbTab)
                                SB.Append(Me._AveragingWindowValues(i).ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                                SB.Append(vbNewLine)
                            Next
                    End Select

                    ' Try to save the file!
                    System.IO.File.WriteAllText(.FileName, SB.ToString, System.Text.Encoding.UTF8)

                Catch ex As Exception
                    MessageBox.Show(My.Resources.rGridPlotter.FileExport_Error.Replace("%e", ex.Message),
                                    My.Resources.rGridPlotter.FileExport_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End With

    End Sub

#End Region

#Region "assigned to no region yet"

#End Region

End Class