Public Class cLineScanPlot
    Inherits cContourPlot

    ''' <summary>
    ''' List for the SpectroscopyTables to Plot as LineScan
    ''' </summary>
    Private lSpectroscopyTables As List(Of cSpectroscopyTable)

    ''' <summary>
    ''' Saves a list of common Columns that are allowed to be plotted.
    ''' </summary>
    Private ListOfAllowedColumns As List(Of String)

    Private _CurrentNumberOfRowRepetitions As Integer = 0
    ''' <summary>
    ''' Returns the number of Row-Repetitions for the currently drawn image.
    ''' </summary>
    Public ReadOnly Property CurrentNumberOfRowRepetitions As Integer
        Get
            Return Me._CurrentNumberOfRowRepetitions
        End Get
    End Property

    ''' <summary>
    ''' Constructor: Save Reference to the List of Spectroscopy-Tables.
    ''' </summary>
    Public Sub New(ByRef ListOfSpectroscopyTables As List(Of cSpectroscopyTable))
        ' Call Constructor of Base Class
        MyBase.New(New MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(1, 1))

        ' Save Reference to the Spectroscopy-Files:
        Me.lSpectroscopyTables = ListOfSpectroscopyTables

        ' Save Common Columns
        Me.ListOfAllowedColumns = cSpectroscopyTable.GetCommonColumns(Me.lSpectroscopyTables)
    End Sub

    ''' <summary>
    ''' Brush to draw the ScaleBar
    ''' </summary>
    Public Property AxesBrush As Brush = Brushes.Black
    Public Property AxesPen As New Pen(AxesBrush, 2)
    Public Property BackColor As Color = Color.White

    ''' <summary>
    ''' Plotrange in X that was plotted in the image.
    ''' </summary>
    Public Property PlotRangePlottedX_Max As Double

    ''' <summary>
    ''' Plotrange in X that was plotted in the image.
    ''' </summary>
    Public Property PlotRangePlottedX_Min As Double

    ''' <summary>
    ''' Creates an Image of the selected Columns of the SpectroscopyTables.
    ''' </summary>
    Public Shadows Function CreateImage(ByVal TargetWidth As Integer,
                                        ByVal TargetHeight As Integer,
                                        ByVal MaxIntensityCorrespondingToValue As Double,
                                        ByVal MinIntensityCorrespondingToValue As Double,
                                        ByVal ColumnNameX As String,
                                        ByVal ColumnNameValues As String,
                                        Optional ByVal MaxXToPlot As Double = Double.NaN,
                                        Optional ByVal MinXToPlot As Double = Double.NaN,
                                        Optional ByVal PlotLogZ As Boolean = False) As Bitmap

        If TargetHeight <= 0 Then Throw New ArgumentOutOfRangeException("TargetHeight", "The target height of the image has to be > 0")
        If TargetWidth <= 0 Then Throw New ArgumentOutOfRangeException("TargetWidth", "The target width of the image has to be > 0")
        If Me.lSpectroscopyTables.Count <= 0 Then Return New Bitmap(1, 1)

        ' Check, if we have to reverse the plot range:
        If Not Double.IsNaN(MaxXToPlot) AndAlso Not Double.IsNaN(MinXToPlot) Then
            If MaxXToPlot < MinXToPlot Then
                Dim TMP As Double = MaxXToPlot
                MaxXToPlot = MinXToPlot
                MinXToPlot = TMP
            End If
            If MaxXToPlot = MinXToPlot Then
                Throw New ArgumentOutOfRangeException("PlotRange", "Plot range has to be > 0")
            End If
        End If

        ' Create the label dimensions
        Dim LabelFrameBottomHeight As Integer = Convert.ToInt32(0.09 * TargetHeight)

        ' Calculate Value-Matrix-Dimension for the Image to plot
        Me._CurrentNumberOfRowRepetitions = 1
        If Me.lSpectroscopyTables.Count < TargetHeight Then
            Me._CurrentNumberOfRowRepetitions = Convert.ToInt32((TargetHeight - LabelFrameBottomHeight) / Me.lSpectroscopyTables.Count)
        End If

        ' Calculate the dimensions of the line-scan-plot
        Dim RowsOfValueMatrix As Integer = Me.lSpectroscopyTables.Count * Me._CurrentNumberOfRowRepetitions
        Dim ColumnsOfValueMatrix As Integer = TargetWidth
        Me.ValueMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(1, ColumnsOfValueMatrix, Double.NaN)

        ' Initialize the FastImage-Class
        Dim FastOutputImage As New cFastImage(New Bitmap(TargetWidth, RowsOfValueMatrix))
        FastOutputImage.Lock()

        ' Reset the row-counter.
        Dim iPixelRowCounter As Integer = 0

        ' Create Value-Matrix to plot,
        ' but always check, if the datacolumns have correct dimensions.
        For i As Integer = 0 To Me.lSpectroscopyTables.Count - 1 Step 1

            ' Get local identifiers for the current X and Y column
            Dim XColumnCropped As List(Of Double) = Me.lSpectroscopyTables(i).Column(ColumnNameX).GetValuesWithoutNaNValues
            Dim ValueColumnCropped As List(Of Double) = Me.lSpectroscopyTables(i).Column(ColumnNameValues).GetValuesWithoutNaNValues '.GetColumnWithoutValuesWhereSourceColumnIsNaN(Me.lSpectroscopyTables(i).Column(ColumnNameX))

            ' Check, that the length of both columns is the same
            If XColumnCropped.Count <> ValueColumnCropped.Count Then
                Me.oFastImage = New cFastImage(My.Resources.cancel_25)
                Throw New ArgumentException(My.Resources.rLineScanViewer.Error_DifferentRangeInXDetected)
            End If

            ' Get column statistics used later
            Dim XColumnMax As Double = XColumnCropped.Max
            Dim XColumnMin As Double = XColumnCropped.Min

            ' Check, if our plot range was defined from outside.
            ' If not, we will take the first data column, and use the min and max values
            ' for the boundaries.
            If Double.IsNaN(MaxXToPlot) OrElse Double.IsNaN(MinXToPlot) Then
                MaxXToPlot = XColumnMax
                MinXToPlot = XColumnMin
            End If

            ' Store the plotted plotrange:
            Me.PlotRangePlottedX_Max = MaxXToPlot
            Me.PlotRangePlottedX_Min = MinXToPlot

            ' Check for monotonic data: we need strict monotonicity in the XColumn for ALL data!
            Dim Monotonicity As cSpectroscopyTable.DataColumn.Monotonicities = Me.lSpectroscopyTables(i).Column(ColumnNameX).Monotonicity

            ' Ignore column without strict monotonicity in the XColumn
            If Monotonicity <> cSpectroscopyTable.DataColumn.Monotonicities.StrictRising And
               Monotonicity <> cSpectroscopyTable.DataColumn.Monotonicities.StrictFalling Then
                'MessageBox.Show(My.Resources.rLineScanViewer.DataMustBeStrictlyMonotonic,
                '                My.Resources.rLineScanViewer.DataMustBeStrictlyMonotonic_Title,
                '                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.oFastImage = New cFastImage(My.Resources.cancel_25)
                Throw New ArgumentException(My.Resources.rLineScanViewer.DataMustBeStrictlyMonotonic)
            End If

            '######################
            ' Interpolate Data

            ' Get the interpolation range:
            Dim MaxXToInterpolate As Double
            Dim MinXToInterpolate As Double
            If XColumnMax >= MaxXToPlot Then
                MaxXToInterpolate = MaxXToPlot
            Else
                MaxXToInterpolate = XColumnMax
            End If
            If XColumnMin <= MinXToPlot Then
                MinXToInterpolate = MinXToPlot
            Else
                MinXToInterpolate = XColumnMin
            End If

            ' Get the step width for the interpolation.
            ' If we plot a range larger than the value range, we have to reduce the number of points.
            Dim PixelToPlot As Integer
            If MinXToInterpolate > MinXToPlot OrElse MaxXToInterpolate < MaxXToPlot Then
                ' Reduce the number of pixels to plot:
                PixelToPlot = CInt(ColumnsOfValueMatrix * (MaxXToInterpolate - MinXToInterpolate) / (MaxXToPlot - MinXToPlot))
            Else
                PixelToPlot = ColumnsOfValueMatrix
            End If

            Dim XInterpolationStepWidth As Double = (MaxXToInterpolate - MinXToInterpolate) / PixelToPlot

            ' Generate interpolated X-Column
            Dim InterpolatedX As New List(Of Double)(PixelToPlot)
            Dim InterpolationXValue As Double
            For x As Integer = 0 To PixelToPlot - 1 Step 1
                InterpolationXValue = MinXToInterpolate + x * XInterpolationStepWidth
                InterpolatedX.Add(InterpolationXValue)
            Next

            Dim InterpolatedValues(InterpolatedX.Count - 1) As Double
            cNumericalMethods.SplineInterpolationNative(XColumnCropped.ToArray,
                                                        ValueColumnCropped.ToArray,
                                                        InterpolatedX.ToArray,
                                                        InterpolatedValues)
            ' END INTERPOLATE DATA
            '######################

            ' If we plot a range larger than the value range, we have generated a reduced number of points.
            ' In this case, we have to offset the written value matrix to the start of the interpolation.
            Dim jOffset As Integer = 0
            If MinXToInterpolate > MinXToPlot OrElse MaxXToInterpolate < MaxXToPlot Then
                jOffset = CInt(ColumnsOfValueMatrix * (MinXToInterpolate - MinXToPlot) / (MaxXToPlot - MinXToPlot))
            End If

            Dim JPlusOffset As Integer
            If Not PlotLogZ Then
                For j As Integer = 0 To PixelToPlot - 1 Step 1
                    JPlusOffset = jOffset + j
                    If JPlusOffset < 0 OrElse JPlusOffset > ColumnsOfValueMatrix Then Continue For
                    Me.ValueMatrix.Item(0, JPlusOffset) = InterpolatedValues(j)
                Next
            Else
                For j As Integer = 0 To PixelToPlot - 1 Step 1
                    JPlusOffset = jOffset + j
                    If JPlusOffset < 0 OrElse JPlusOffset > ColumnsOfValueMatrix Then Continue For
                    If InterpolatedValues(j) > 0 Then
                        Me.ValueMatrix.Item(0, JPlusOffset) = Math.Log10(InterpolatedValues(j))
                    End If
                Next
            End If

            ' Create Pixel-Line of Image:
            MyBase.Plot2D(MaxIntensityCorrespondingToValue, MinIntensityCorrespondingToValue)

            ' Copy Pixel to the Output Image
            Me.oFastImage.Lock()
            For RepetitionCounter As Integer = 1 To Me._CurrentNumberOfRowRepetitions Step 1
                For p As Integer = 0 To Me.oFastImage.Width - 1 Step 1
                    'OutputImage.SetPixel(p, i, ImageLines(i).GetPixel(p, 0))
                    FastOutputImage.SetPixel(p, iPixelRowCounter, Me.oFastImage.GetPixel(p, 0))
                Next
                iPixelRowCounter += 1
            Next
            Me.oFastImage.Unlock(False)
        Next
        FastOutputImage.Unlock(True)
        FastOutputImage = New cFastImage(cContourPlot.ResizeImage(FastOutputImage.Bitmap, FastOutputImage.Width, TargetHeight - LabelFrameBottomHeight))
        Dim FinalImage As New Bitmap(TargetWidth, TargetHeight)
        Me.oFastImage = New cFastImage(FinalImage)

        ' Clear the image with white, before filling in the rest.
        Me.oFastImage.Lock()
        Me.oFastImage.Clear(Color.White)
        Me.oFastImage.Unlock(True)

        ' Apply additional markers to the image.
        ' Get the Drawing Surface
        Dim OutputImage As Bitmap = Me.oFastImage.Bitmap
        Dim DrawSurface As Graphics = Graphics.FromImage(OutputImage)

        ' Draw the rescaled image to the final output.
        DrawSurface.DrawImageUnscaled(FastOutputImage.Bitmap, 0, 0)

        '###########################################################################
        '###########################################################################
        ' AXES and Labels

        ' Activate Antialiasing
        DrawSurface.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        ' Get the Axis-Range
        Dim AxesRange As Double = MaxXToPlot - MinXToPlot

        ' Format the range and get the ticks from this formatted value
        Dim FormatedRange As Double = cUnits.GetPrefix(AxesRange).Value
        Dim FormatFactor As Double = FormatedRange / AxesRange
        Dim AxesRangePerPixel As Double = FormatedRange / DrawSurface.VisibleClipBounds.Width

        ' Generate X-Axis:
        ' we place it in X direction, so take 100% of the image width as reference frame
        Dim Axes_X_Position As New Point(0, TargetHeight - LabelFrameBottomHeight)
        Dim Axes_X_Width As Integer = ColumnsOfValueMatrix
        Me.AxesPen.Width = Convert.ToInt32(0.003 * TargetHeight)
        Dim AxesPenHalf As Integer = Convert.ToInt32(Me.AxesPen.Width / 2)
        Dim Axes_LimitersHeight_Large As Integer = Convert.ToInt32(4 * Me.AxesPen.Width)
        Dim Axes_LimitersHeight_Small As Integer = Convert.ToInt32(3 * Me.AxesPen.Width)

        ' Generate the Axes-Labels
        Dim AxesLabel_X As String = ColumnNameX & " (" & cUnits.GetPrefix(AxesRange).Key & cUnits.GetUnitSymbolFromType(Me.lSpectroscopyTables(0).Column(ColumnNameX).UnitType) & ")"

        ' Get the size of the label text
        If Axes_LimitersHeight_Large = 0 Then Axes_LimitersHeight_Large = 1
        Dim AxesLabel_Font As Font = New System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif,
                                                                 Axes_LimitersHeight_Small * 3)
        Dim AxesLabel_Font_StringSize As SizeF = DrawSurface.MeasureString(AxesLabel_X, AxesLabel_Font)

        Dim AxesTick_Font As Font = New System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif,
                                                                Axes_LimitersHeight_Small * 2)
        Dim AxesTick_StringSize As SizeF = DrawSurface.MeasureString(AxesLabel_X, AxesTick_Font)

        ' Get the position of the scale-bar-label
        Dim AxesLabel_X_Position As New PointF(Convert.ToSingle(Axes_X_Position.X + Axes_X_Width / 2 - AxesLabel_Font_StringSize.Width / 2),
                                                   Axes_X_Position.Y + AxesTick_StringSize.Height + Axes_LimitersHeight_Small)

        With Axes_X_Position

            '#####################
            ' Draw Axes Line
            Me.AxesPen.Width = AxesPenHalf * 4
            DrawSurface.DrawLine(Me.AxesPen, .X, .Y, .X + Axes_X_Width, .Y)

            ' Draw the axes label
            DrawSurface.DrawString(AxesLabel_X,
                                       AxesLabel_Font,
                                       Me.AxesBrush,
                                       AxesLabel_X_Position)

            '#######################################
            ' Axes Ticks
            Me.AxesPen.Width = AxesPenHalf * 2

            ' Use truncate to check if we use tick-widths of 1
            Dim TickRange As Double = 1
            Dim Ticks As Integer = Convert.ToInt32(Math.Truncate(FormatedRange))

            ' If we would have no ticks, reduce the TickRange by factor of 10,
            ' and calculate again the number of ticks
            While Ticks < 2 And FormatedRange <> 0
                TickRange /= 10
                Ticks = Convert.ToInt32(Math.Truncate(FormatedRange / TickRange))
            End While
            ' If we would have to many ticks, reduce the TickRange by factor of 10,
            ' and calculate again the number of ticks
            While Ticks > 20 And FormatedRange <> 0
                TickRange *= 10
                Ticks = Convert.ToInt32(Math.Truncate(FormatedRange / TickRange))
            End While

            ' Draw Ticks
            Dim DrawTickX As Integer = 0
            Dim XOffset As Integer = Convert.ToInt32((FormatedRange - Ticks * TickRange) / 2)
            Dim TickRangePixel As Double = TickRange / AxesRangePerPixel
            Dim AxesTickLabelValue As Double
            Dim TickLabelPosition As PointF
            For i As Integer = 0 To Ticks Step 1

                ' Draw Tick
                DrawTickX = XOffset + Convert.ToInt32(i * TickRangePixel)

                ' Draw Tick-Label
                AxesTickLabelValue = MinXToPlot * FormatFactor + DrawTickX * AxesRangePerPixel

                ' Cut the decimal places for values larger 100 ... they are then unnecessary!
                Dim TickValueFormatted As String
                If AxesTickLabelValue >= 100 Or CInt(AxesTickLabelValue * 10) Mod 10 = 0 Then
                    TickValueFormatted = Math.Round(AxesTickLabelValue, 0).ToString("F0", Globalization.CultureInfo.InvariantCulture)
                Else
                    TickValueFormatted = Math.Round(AxesTickLabelValue, 1).ToString("F1", Globalization.CultureInfo.InvariantCulture)
                End If

                AxesTick_StringSize = DrawSurface.MeasureString(TickValueFormatted, AxesTick_Font)
                TickLabelPosition = New PointF(Convert.ToSingle(DrawTickX - AxesTick_StringSize.Width / 2),
                                                   Axes_X_Position.Y + Axes_LimitersHeight_Large)

                ' Draw the tick
                DrawSurface.DrawLine(Me.AxesPen, DrawTickX + AxesPenHalf, .Y, DrawTickX + AxesPenHalf, .Y + Axes_LimitersHeight_Large)

                ' draw small ticks in the middle behind and before each tick, if the size between the ticks is large enough
                If TickRangePixel > 5 * Me.AxesPen.Width Then
                    Dim TickRangePixelHalf As Integer = CInt(TickRangePixel * 0.5)
                    DrawSurface.DrawLine(Me.AxesPen, DrawTickX + TickRangePixelHalf, .Y, DrawTickX + TickRangePixelHalf, .Y + Axes_LimitersHeight_Small)
                    'DrawSurface.DrawLine(Me.AxesPen, DrawTickX - TickRangePixelHalf, .Y, DrawTickX - TickRangePixelHalf, .Y + Axes_LimitersHeight_Small)
                End If

                ' draw small ticks in the quarter behind and before each tick, if the size between the ticks is large enough
                If TickRangePixel > 9 * Me.AxesPen.Width Then
                    Dim TickRangePixelQuarter As Integer = CInt(TickRangePixel * 0.25)
                    DrawSurface.DrawLine(Me.AxesPen, DrawTickX + TickRangePixelQuarter, .Y, DrawTickX + TickRangePixelQuarter, .Y + Axes_LimitersHeight_Small)
                    DrawSurface.DrawLine(Me.AxesPen, DrawTickX - TickRangePixelQuarter, .Y, DrawTickX - TickRangePixelQuarter, .Y + Axes_LimitersHeight_Small)
                End If

                ' Drop the tick-label, if it is drawn at 0, or if it would range to negative values,
                ' or the opposite, if it would range larger, than the image is broad.
                If DrawTickX = 0 Or TickLabelPosition.X < 0 Or
                       DrawTickX = DrawSurface.VisibleClipBounds.Width Or TickLabelPosition.X + AxesTick_StringSize.Width > DrawSurface.VisibleClipBounds.Width Then
                    Continue For
                End If

                ' Draw tick-label
                DrawSurface.DrawString(TickValueFormatted,
                                           AxesTick_Font,
                                           Me.AxesBrush,
                                           TickLabelPosition)

            Next

        End With


        Return OutputImage
    End Function

    ''' <summary>
    ''' Exports a LineScan Plot to a WSXM file
    ''' </summary>
    Public Sub ExportToWSXM(ByVal TargetFile As String,
                            ByVal ColumnNameX As String,
                            ByVal ColumnNameValues As String,
                            Optional ByVal RowRepetition As Integer = 1)

        Dim RowsOfValueMatrix As Integer = Me.lSpectroscopyTables.Count * RowRepetition
        Dim ColumnsOfValueMatrix As Integer = Me.lSpectroscopyTables(0).Column(ColumnNameX).Values.Count

        Dim ValueMatrix As New MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(RowsOfValueMatrix, ColumnsOfValueMatrix)

        ' Value statistics:
        Dim MaxValue As Double = Double.MinValue
        Dim MinValue As Double = Double.MaxValue

        Dim RowCounter As Integer = 0
        For i As Integer = 0 To Me.lSpectroscopyTables.Count - 1 Step 1
            Dim HeightValues As ReadOnlyCollection(Of Double) = Me.lSpectroscopyTables(i).Column(ColumnNameValues).Values
            For RepetitionCounter As Integer = 1 To RowRepetition Step 1
                For j As Integer = 0 To ColumnsOfValueMatrix - 1 Step 1
                    ValueMatrix(RowCounter, j) = HeightValues(j)

                    ' Save max and min-value
                    If HeightValues(j) < MinValue Then
                        MinValue = HeightValues(j)
                    End If
                    If HeightValues(j) > MaxValue Then
                        MaxValue = HeightValues(j)
                    End If
                Next
                RowCounter += 1
            Next
        Next

        cExport.Export2DMatrixToWSxM(TargetFile,
                                         ValueMatrix,
                                         Me.lSpectroscopyTables(0).Column(ColumnNameX).GetMaximumValueOfColumn - Me.lSpectroscopyTables(0).Column(ColumnNameX).GetMinimumValueOfColumn, Me.lSpectroscopyTables(0).Column(ColumnNameX).UnitSymbol,
                                         1, ,
                                         MaxValue - MinValue, Me.lSpectroscopyTables(0).Column(ColumnNameValues).UnitSymbol)
    End Sub
End Class
