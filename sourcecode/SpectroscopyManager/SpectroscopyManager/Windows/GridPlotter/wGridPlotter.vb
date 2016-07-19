Imports System.ComponentModel
Imports System.Threading.Tasks
Imports ImageMagick
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
    ''' Backgroundworker for creating the GIF animation.
    ''' </summary>
    Protected WithEvents _GIFAnimationWorker As New BackgroundWorker

    ''' <summary>
    ''' Is any worker busy?
    ''' </summary>
    Public Function IsAnyWorkerBusy() As Boolean
        If Me._GIFAnimationWorker.IsBusy Then Return True
        If Me._FileLoader.IsBusy Then Return True
        If Me._GridPlotter.IsBusy Then Return True
        Return False
    End Function

    ''' <summary>
    ''' Contains all file-objects that should be loaded.
    ''' </summary>
    Protected _FileObjectsLoaded_Spectroscopy As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Contains all file-objects that should be loaded.
    ''' </summary>
    Protected _FileObjectsLoaded_ScanImage As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Contains all file-objects that should be loaded.
    ''' </summary>
    Protected _FileObjectsLoaded_GridFile As New Dictionary(Of String, cFileObject)

    ''' <summary>
    ''' Contains all spectroscopy-tables.
    ''' </summary>
    Protected _SpectroscopyTables As New List(Of cSpectroscopyTable)

    ''' <summary>
    ''' Contains all scan images.
    ''' </summary>
    Protected _ScanImages As New List(Of cScanImage)

    ''' <summary>
    ''' Contains all grid files.
    ''' </summary>
    Protected _GridFiles As New List(Of cGridFile)

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
    ''' Keep grid value range constant, and don't adapt it to the full range automatically.
    ''' </summary>
    Protected _KeepGridValueRangeConstant As Boolean = False

    ''' <summary>
    ''' Plot point diameter.
    ''' </summary>
    Private _PointDiameter As Double = 0.0000000001

    ''' <summary>
    ''' Defines the position of the bias indicator.
    ''' </summary>
    Private BiasIndicatorPosition As New PointF(0.01, 0.01)

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
        Me.dsGridFiles.SetFileList(Me._FileList)
        Me.dsScanImages.SetFileList(Me._FileList)

        ' Setup the background-worker
        Me._FileLoader.WorkerReportsProgress = True
        Me._FileLoader.WorkerSupportsCancellation = True
        Me._GridPlotter.WorkerReportsProgress = True
        Me._GIFAnimationWorker.WorkerReportsProgress = True

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

        ' Set some default values
        Me.txtAnimationTime.SetValue(5000)
        Me.txtPlotSettings_BiasIndicatorSize.SetValue(7)
        Me.ckbPlotSettings_BiasIndicatorSize.Checked = True
        Me.txtCreepCorrectionX.SetValue(0)
        Me.txtCreepCorrectionY.SetValue(0)

        Me.bReady = True

    End Sub

#End Region

#Region "Destructor"

    ''' <summary>
    ''' Function of the Form-Closing Event.
    ''' Abort the Closing, if the File-Buffer is fetching!
    ''' </summary>
    Private Sub CheckBeforeFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me.IsAnyWorkerBusy Then
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
    Private Sub dsSpectroscopyFiles_SelectionChanged(ByRef SelectedFileObjects As Dictionary(Of String, cFileObject)) Handles dsSpectroscopyFiles.SelectionChanged, dsScanImages.SelectionChanged, dsGridFiles.SelectionChanged
        If ((dsSpectroscopyFiles.SelectedEntries.Count > 0 AndAlso Me.tcGridFilesSelector.SelectedTab Is Me.tpIndividualSpectroscopyFiles) OrElse
            (dsGridFiles.SelectedEntries.Count > 0 AndAlso Me.tcGridFilesSelector.SelectedTab Is Me.tpGridFiles)) AndAlso
           dsScanImages.SelectedEntries.Count >= 0 Then
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

        If Me.IsAnyWorkerBusy Then Return

        ' Copy the references of the file-objects to the list of file-objects to load.
        Me._FileObjectsLoaded_Spectroscopy = Me.dsSpectroscopyFiles.SelectedEntries()
        Me._FileObjectsLoaded_ScanImage = Me.dsScanImages.SelectedEntries()
        Me._FileObjectsLoaded_GridFile = Me.dsGridFiles.SelectedEntries()

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
        Me.tcGridFilesSelector.Enabled = Active
        Me.dsScanImages.Enabled = Active
        Me.gbDataRange.Enabled = Active
        Me.gbExport.Enabled = Active
        Me.gbGenerateGIF.Enabled = Active
    End Sub

    ''' <summary>
    ''' Loads the FileObjects located in the FileObjectToLoad dictionary.
    ''' </summary>
    Protected Sub LoadSelectedDataSet() Handles _FileLoader.DoWork

        ' Clear the cache.
        Me._ScanImages.Clear()
        Me._SpectroscopyTables.Clear()
        Me._GridFiles.Clear()

        ' Set the length of the list at once, if it is too small.
        If Me._SpectroscopyTables.Capacity < Me._FileObjectsLoaded_Spectroscopy.Count Then
            Me._SpectroscopyTables.Capacity = Me._FileObjectsLoaded_Spectroscopy.Count
        End If
        If Me._ScanImages.Capacity < Me._FileObjectsLoaded_ScanImage.Count Then
            Me._ScanImages.Capacity = Me._FileObjectsLoaded_ScanImage.Count
        End If
        If Me._GridFiles.Capacity < Me._FileObjectsLoaded_GridFile.Count Then
            Me._GridFiles.Capacity = Me._FileObjectsLoaded_GridFile.Count
        End If

        ' Variables for the progress calculation.
        Dim ProgressPercentage As Double = 0
        Dim ProgressPercentageIncrement As Double = 0

        ProgressPercentageIncrement = 70 / (Me._FileObjectsLoaded_Spectroscopy.Count + 1)
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

        ProgressPercentageIncrement = 10 / (Me._FileObjectsLoaded_ScanImage.Count + 1)
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

        ProgressPercentageIncrement = 15 / (Me._FileObjectsLoaded_GridFile.Count + 1)
        For Each GridFOKV As KeyValuePair(Of String, cFileObject) In Me._FileObjectsLoaded_GridFile

            ' Report Progress
            ProgressPercentage += ProgressPercentageIncrement
            Me._FileLoader.ReportProgress(CInt(ProgressPercentage), My.Resources.rGridPlotter.FetchProgress.Replace("%f", GridFOKV.Value.FileNameWithoutPath).Replace("%p", ProgressPercentage.ToString("N2")))

            ' Load the GridFile
            Dim GridFile As cGridFile = Nothing
            cFileImport.GetGridFile(GridFOKV.Value, GridFile, False)

            ' Add the table to the cache.
            If Not GridFile Is Nothing Then
                Me._GridFiles.Add(GridFile)
            End If

        Next

        ' If we loaded a grid file. Store the spectroscopytables of the grid file
        ' in the spectroscopy table list.
        If Me._GridFiles.Count > 0 Then
            Me._SpectroscopyTables = Me._GridFiles(0).SpectroscopyTables
        End If

        '##################################################
        ' Handling of pure grid data without a topography.
        ' In this case we generate a dummy topography file.
        '##################################################

        If Me._ScanImages.Count = 0 Then

            ' Create new scan image.
            Dim ScanImage As New cScanImage
            Dim ScanChannel As New cScanImage.ScanChannel

            ' Get the dimensions of the grid.
            Dim X_Max As Double = Double.MinValue
            Dim X_Min As Double = Double.MaxValue
            Dim Y_Max As Double = Double.MinValue
            Dim Y_Min As Double = Double.MaxValue

            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1

                X_Max = Math.Max(Me._SpectroscopyTables(i).Location_X, X_Max)
                X_Min = Math.Min(Me._SpectroscopyTables(i).Location_X, X_Min)

                Y_Max = Math.Max(Me._SpectroscopyTables(i).Location_Y, Y_Max)
                Y_Min = Math.Min(Me._SpectroscopyTables(i).Location_Y, Y_Min)

            Next

            Dim XRange As Double = X_Max - X_Min
            Dim YRange As Double = Y_Max - Y_Min

            With ScanImage
                .ScanOffset_X = X_Min - 0.02 * XRange
                .ScanOffset_Y = Y_Max + 0.02 * YRange
                .ScanRange_X = XRange * 1.04
                .ScanRange_Y = YRange * 1.04
                .ScanAngle = 0
                .ScanPixels_X = 500
                .ScanPixels_Y = 500

                ScanChannel.ScanData = LinearAlgebra.Double.DenseMatrix.Create(500, 500, Double.NaN)
                .AddScanChannel(ScanChannel)
            End With

            Me._ScanImages.Add(ScanImage)
        End If

        ' 
        '##################################################


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

        ' Load the selected data
        Me.SelectedSpectroscopyDataChanged()

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

        ' Set the GIF-export to the max and min values
        Me.txtGIFStartValue.SetValue(Me._SelectedRange_XMin)
        Me.txtGIFEndValue.SetValue(Me._SelectedRange_XMax)

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
        If Me.IsAnyWorkerBusy Then Return

        '' Deactivate the plot settings box
        'Me.gbPlotSettings.Enabled = False
        'Me.gbExport.Enabled = False

        ' Start the plotting.
        Me._GridPlotter.RunWorkerAsync()

    End Sub

    ''' <summary>
    ''' Plots the grid in the background-worker.
    ''' </summary>
    Private Sub CreateGridPointMarks() Handles _GridPlotter.DoWork

        Try

            If Me._GridPlotter.IsBusy Then Me._GridPlotter.ReportProgress(5, My.Resources.rGridPlotter.PlotProgress_CalculateAverageValuesOfAveragingWindow)
            ' Start to count up and average all values in the selected range,
            ' if the value array is marked as invalid.
            Me.CalculateAveragingWindowValue()

            ' Get the maximum and minimum-values of the averaging window,
            ' if the averaging window changed.
            If Me._AveragingWindowValuesWereUpdated AndAlso Not Me._KeepGridValueRangeConstant Then
                Me._GridPlotRangeMax = cNumericalMethods.GetMaximumValue(Me._AveragingWindowValues)
                Me._GridPlotRangeMin = cNumericalMethods.GetMinimumValue(Me._AveragingWindowValues)
            End If

            ' Remove all old point marks
            Me._PointMarkList.Clear()

            ' Get the size of the points.
            Dim PointRadius As Double = Me._PointDiameter / 2

            ' Get the color-scheme colors
            Dim BrushArray As SolidBrush() = Me.cpPlotSettings_ColorCode.GetSelectedColorScheme.BrushArray

            ' Get the chronological first spectrum in the list
            Dim EarliestSpectrumTime As Date = Date.MaxValue
            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1
                If Me._SpectroscopyTables(i).RecordDate < EarliestSpectrumTime Then
                    EarliestSpectrumTime = Me._SpectroscopyTables(i).RecordDate
                End If
            Next

            ' Coordinate Buffers
            Dim PointX As Double
            Dim PointY As Double
            Dim SecondsSinceFirstSpectrum As Double

            ' Now calculate the settings for all datapoints.
            Dim Progress As Double = 10
            Dim ProgressProgress As Double = (80 / (Me._SpectroscopyTables.Count + 1))
            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1

                ' Report progress
                If Me._GridPlotter.IsBusy Then Me._GridPlotter.ReportProgress(CInt(Progress), My.Resources.rGridPlotter.PlotProgress_PlottingDataPoint.Replace("%f", Me._SpectroscopyTables(i).FileNameWithoutPath))
                Progress += ProgressProgress

                ' Get the color of the point-mark.
                Dim PointColorBrush As SolidBrush = cColorScheme.GetPlotColorFromColorScale(Me._GridPlotRangeMax,
                                                                                            Me._GridPlotRangeMin,
                                                                                            Me._AveragingWindowValues(i),
                                                                                            BrushArray)

                ' Check, if the color exists!
                If PointColorBrush Is Nothing Then Continue For

                ' Get the coordinates to plot:
                PointX = Me._SpectroscopyTables(i).Location_X
                PointY = Me._SpectroscopyTables(i).Location_Y

                ' Correct them by a creep along X and Y over time
                SecondsSinceFirstSpectrum = (Me._SpectroscopyTables(i).RecordDate - EarliestSpectrumTime).TotalSeconds
                PointX += Me.txtCreepCorrectionX.DecimalValue * SecondsSinceFirstSpectrum
                PointY += Me.txtCreepCorrectionY.DecimalValue * SecondsSinceFirstSpectrum

                ' Create new point-mark.
                Dim NewPointMark As New cScanImagePlot.PointMark(PointX,
                                                                 PointY,
                                                                 Me._SpectroscopyTables(i).Location_Z,
                                                                 PointRadius,
                                                                 PointColorBrush.Color,
                                                                 cScanImagePlot.PointMark.PointMarkShapes.CircleFilled,
                                                                 Me._SpectroscopyTables(i).FileNameWithoutPath,
                                                                 i.ToString(Globalization.CultureInfo.InvariantCulture))
                Me._PointMarkList.Add(NewPointMark)

            Next

        Catch ex As Exception
            Debug.WriteLine("wGridPlotter->GridPlot()-error: " & ex.Message)
        End Try

        ' Reset the progress
        If Me._GridPlotter.IsBusy Then Me._GridPlotter.ReportProgress(-1, "-")

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

                ' Add the grid's point marks
                .ClearPointMarkList()
                .AddPointMark(Me._PointMarkList)

                ' Add the bias value as Text-Object
                .ClearTextObjectList()
                If Me.ckbPlotSettings_BiasIndicatorSize.Checked AndAlso Me.txtPlotSettings_BiasIndicatorSize.IntValue > 0 Then
                    Dim Text As New cScanImagePlot.TextObject(BiasIndicatorPosition, cUnits.GetFormatedValueString(Me._AverageWindowStart))
                    With Text
                        .ValueIsAbsolute = False
                        .UseRelativeFontSize = Me.txtPlotSettings_BiasIndicatorSize.IntValue
                    End With
                    .AddTextObject(Text)
                End If

                .RecalculateImageAsync()
            End With

            ' Set the value range of the output image to the value range selector
            If Me._AveragingWindowValuesWereUpdated AndAlso Me._AveragingWindowValues.Count > 0 Then
                Me.vrsPlotSettings_ColorScaling.SetValueArray(Me._AveragingWindowValues,,
                                                              False,
                                                              New Tuple(Of Double, Double)(Me._GridPlotRangeMin, Me._GridPlotRangeMax),
                                                              False)
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

    ''' <summary>
    ''' Plot settings changed
    ''' </summary>
    Private Sub txtPlotSettingsChanged(ByRef NT As NumericTextbox) Handles txtCreepCorrectionX.ValidValueChanged, txtCreepCorrectionY.ValidValueChanged, txtPlotSettings_BiasIndicatorSize.ValidValueChanged
        Me.PlotGrid()
    End Sub

#End Region

#Region "Manage the status-bar to report progress and messages."

    ''' <summary>
    ''' Report progress.
    ''' </summary>
    Protected Sub WorkerProgress(sender As Object, e As ProgressChangedEventArgs) Handles _FileLoader.ProgressChanged, _GridPlotter.ProgressChanged, _GIFAnimationWorker.ProgressChanged
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

                            ' Get the chronological first spectrum in the list
                            Dim EarliestSpectrumTime As Date = Date.MaxValue
                            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1
                                If Me._SpectroscopyTables(i).RecordDate < EarliestSpectrumTime Then
                                    EarliestSpectrumTime = Me._SpectroscopyTables(i).RecordDate
                                End If
                            Next

                            ' Coordinate Buffers
                            Dim PointX As Double
                            Dim PointY As Double
                            Dim SecondsSinceFirstSpectrum As Double

                            ' Create the XYZ-file from the averaging window values.
                            For i As Integer = 0 To Me._SpectroscopyTables.Count - 1 Step 1

                                ' Get the coordinates to plot:
                                PointX = Me._SpectroscopyTables(i).Location_X
                                PointY = Me._SpectroscopyTables(i).Location_Y

                                ' Correct them by a creep along X and Y over time
                                SecondsSinceFirstSpectrum = (Me._SpectroscopyTables(i).RecordDate - EarliestSpectrumTime).TotalSeconds
                                PointX += Me.txtCreepCorrectionX.DecimalValue * SecondsSinceFirstSpectrum
                                PointY += Me.txtCreepCorrectionY.DecimalValue * SecondsSinceFirstSpectrum

                                SB.Append(PointX.ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
                                SB.Append(vbTab)
                                SB.Append(PointY.ToString("E6", System.Globalization.CultureInfo.InvariantCulture))
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

#Region "GIF animation"

    ''' <summary>
    ''' Filename to store the GIF animation.
    ''' </summary>
    Private _GIFAnimationOutputFileName As String

    ''' <summary>
    ''' Generate the GIF animation file.
    ''' </summary>
    Private Sub btnGenerateGIF_Click(sender As Object, e As EventArgs) Handles btnGenerateGIF.Click
        If Me.IsAnyWorkerBusy Then Return

        ' Ask for a filename:
        Dim FileBrowser As New SaveFileDialog
        With FileBrowser
            .Title = My.Resources.rGridPlotter.ExportGif_FileDialogTitle
            .Filter = "GIF animation file|*.gif"
            .FileName = "grid.gif"
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
        ' Only continue, if the averaging window is not zero.
        If Me._AverageWindowWidth <= 0 Then Return

        ' Store the initial parameters
        Dim OldAveragingWindowStart As Double = Me._AverageWindowStart

        ' Get the number of steps.
        Dim StartValue As Double = Me.txtGIFStartValue.DecimalValue
        Dim EndValue As Double = Me.txtGIFEndValue.DecimalValue
        Dim Steps As Integer = CInt((EndValue - StartValue) / Me._AverageWindowWidth)
        If Steps <= 0 Then Return

        Dim Progress As Double = 0
        Dim ProgressProgress As Double = 0

        ' Generate the images.
        Dim Images As New List(Of Bitmap)

        Try

            ' Sweep though the range and copy each image.
            Dim CurrentValue As Double
            ProgressProgress = 85 / Steps
            For i As Integer = 0 To Steps - 1 Step 1

                CurrentValue = StartValue + i * Me._AverageWindowWidth

                ' Report progress
                If Me._GIFAnimationWorker.IsBusy Then Me._GIFAnimationWorker.ReportProgress(CInt(Progress), My.Resources.rGridPlotter.ExportGif_Progress_GeneratingImage.Replace("%i", i.ToString).Replace("%max", Steps.ToString))
                Progress += ProgressProgress

                '####################
                ' Generate the image

                ' First: set the value range.
                Me.SetAverageWindowStart(CurrentValue, False)

                ' Mark the values as invalid, which allows to recalculate the value-array.
                Me._AveragingWindowValuesInvalid = True

                ' Second: generate the GridPointMarks.
                Me.CreateGridPointMarks()

                ' Third: use a ScanImagePlotter to generate the image to be saved.
                ' Create new ScanImagePlot from the Data
                Dim oScanImagePlot As New cScanImagePlot(Me._ScanImages)

                ' Setup the plot-engine
                oScanImagePlot.PlotScaleBar = Me.svOutputImage.ckbScaleBarVisible.Checked

                ' If we have a new scan-channel that is plotted,
                ' then initially use the full value range to be plotted.
                Dim MaxValueToPlot As Double
                Dim MinValueToPlot As Double
                MaxValueToPlot = Me.svOutputImage.vsValueRangeSelector.SelectedMaxValue
                MinValueToPlot = Me.svOutputImage.vsValueRangeSelector.SelectedMinValue

                ' Calculate Image:
                With oScanImagePlot
                    .ClearPointMarkList()

                    ' Add the grid points
                    .AddPointMarks(Me._PointMarkList)

                    ' Add the bias value as Text-Object
                    If Me.txtPlotSettings_BiasIndicatorSize.IntValue > 0 Then
                        Dim Text As New cScanImagePlot.TextObject(BiasIndicatorPosition, cUnits.GetFormatedValueString(CurrentValue))
                        With Text
                            .ValueIsAbsolute = False
                            .UseRelativeFontSize = Me.txtPlotSettings_BiasIndicatorSize.IntValue
                        End With
                        .AddTextObject(Text)
                    End If

                    ' Draw the image
                    .ColorScheme = Me.svOutputImage.cpColorPicker.SelectedColorScheme
                    .ScanImageFiltersToApplyBeforePlot = Me.svOutputImage.ScanImageFiltersSelected
                    .CreateImage(MaxValueToPlot,
                                 MinValueToPlot,
                                 Me.svOutputImage.GetSelectedScanChannelName,
                                 Me.svOutputImage.Width, Me.svOutputImage.Height,
                                 Me.svOutputImage.ckbUseHighQualityScaling.Checked)
                End With
                '####################

                ' Store the image:
                Images.Add(New Bitmap(oScanImagePlot.Image))

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
                    Images(i).Dispose()
                    GIFCreator(i).AnimationDelay = AnimationTime
                    ' For DEBUG:
                    'Images(i).Save(FileBrowser.FileName & "." & i.ToString & ".gif")
                Next

                ' Adjust the settings of the GIF
                Dim settings As QuantizeSettings = New QuantizeSettings
                settings.Colors = 256
                GIFCreator.Quantize(settings)

                ' Optionally optimize the images (images should have the same size).
                'GIFCreator.Optimize()

                ' Save gif
                GIFCreator.Write(Me._GIFAnimationOutputFileName)

            End Using

            ' last but not least, set back the old value.
            Me.SetAverageWindowStart(OldAveragingWindowStart, False)

            If Me._GIFAnimationWorker.IsBusy Then Me._GIFAnimationWorker.ReportProgress(-1, String.Empty)
        Catch ex As Exception
            Debug.WriteLine("wGridPlotter:GifCreator--> error " & ex.Message)
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

#End Region

#Region "Interface stuff"

    ''' <summary>
    ''' Switch the bias indicator on or off.
    ''' </summary>
    Private Sub ckbPlotSettings_BiasIndicatorSize_CheckedChanged(sender As Object, e As EventArgs) Handles ckbPlotSettings_BiasIndicatorSize.CheckedChanged
        If Not Me.bReady Then Return
        Me.txtPlotSettings_BiasIndicatorSize.Enabled = Me.ckbPlotSettings_BiasIndicatorSize.Checked
        Me.PlotGrid()
    End Sub

    ''' <summary>
    ''' Changes the automatic adjustment of the grid value range.
    ''' </summary>
    Private Sub ckbGIFKeepValueRangeConstant_CheckedChanged(sender As Object, e As EventArgs) Handles ckbGIFKeepValueRangeConstant.CheckedChanged
        Me._KeepGridValueRangeConstant = Me.ckbGIFKeepValueRangeConstant.Checked
    End Sub

#End Region

#Region "Event handling for selection in the preview image viewers"

    ''' <summary>
    ''' If a point mark is selected in the scan image viewer,
    ''' then try to select the spectroscopy file in the spectroscopy list.
    ''' </summary>
    Public Sub PointMarkSelectedInScanImage(PointMark As cScanImagePlot.PointMark) Handles svOutputImage.PointMarkClicked

        ' The tag contains always the index in the loaded spectroscopy list.
        Dim SpectrumIndex As Integer = -1
        Integer.TryParse(PointMark.Tag, SpectrumIndex)

        If SpectrumIndex >= 0 AndAlso Me._SpectroscopyTables.Count > SpectrumIndex Then

            ' Select the spectroscopy table with the index.
            Me.spDataRangeSelector_DataToDisplay.SetSelectedEntry(Me._SpectroscopyTables(SpectrumIndex).FileNameWithoutPath)

        End If


    End Sub

#End Region

End Class