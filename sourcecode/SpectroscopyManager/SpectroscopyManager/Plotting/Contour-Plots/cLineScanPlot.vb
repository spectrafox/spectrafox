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

        ' Check Spectroscopy-Files for correct number of values and correct range.


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
    ''' Creates an Image of the selected Columns of the SpectroscopyTables.
    ''' </summary>
    Public Shadows Function CreateImage(ByVal TargetWidth As Integer,
                                        ByVal TargetHeight As Integer,
                                        ByVal MaxIntensityCorrespondingToValue As Single,
                                        ByVal MinIntensityCorrespondingToValue As Single,
                                        ByVal ColumnNameX As String,
                                        ByVal ColumnNameValues As String) As Bitmap

        If TargetHeight <= 0 Then Throw New ArgumentOutOfRangeException("TargetHeight", "The target height of the image has to be > 0")
        If TargetWidth <= 0 Then Throw New ArgumentOutOfRangeException("TargetWidth", "The target width of the image has to be > 0")
        If Me.lSpectroscopyTables.Count <= 0 Then Return New Bitmap(1, 1)

        ' Check, if all SpectroscopyTables have the same amount of data
        Dim ValueCountX As Integer = -1

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
        Me.ValueMatrix = New MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(1, ColumnsOfValueMatrix)

        ' Initialize the FastImage-Class
        Dim FastOutputImage As New cFastImage(New Bitmap(TargetWidth, RowsOfValueMatrix))
        FastOutputImage.Lock()

        ' Reset the row-counter.
        Dim iPixelRowCounter As Integer = 0

        ' Save the first cropped XColumn separately: needed for Axes generation
        Dim FirstXColumnWithoutCroppedValues As List(Of Double) = Nothing
        Dim FirstXColumn As cSpectroscopyTable.DataColumn = Nothing

        ' Create Value-Matrix to Plot,
        ' but allways check, if the datacolumns have correct dimensions.
        For i As Integer = 0 To Me.lSpectroscopyTables.Count - 1 Step 1

            ' Get local identifiers for the current X and Value Column
            Dim CurrentXColumnCropped As List(Of Double) = Me.lSpectroscopyTables(i).Column(ColumnNameX).GetValuesWithoutNaNValues
            Dim CurrentValueColumnCropped As List(Of Double) = Me.lSpectroscopyTables(i).Column(ColumnNameValues).GetValuesWithoutNaNValues '.GetColumnWithoutValuesWhereSourceColumnIsNaN(Me.lSpectroscopyTables(i).Column(ColumnNameX))

            If ValueCountX > 0 Then
                If CurrentXColumnCropped.Count <> ValueCountX Then
                    Me.oFastImage = New cFastImage(My.Resources.cancel_25)
                    Throw New ArgumentOutOfRangeException("ValueCountX", "The individual spectra have a differnt point counts, or a different crop range.")
                End If
                If CurrentValueColumnCropped.Count <> ValueCountX Then
                    Me.oFastImage = New cFastImage(My.Resources.cancel_25)
                    Throw New ArgumentOutOfRangeException("ValueCountX", "The individual spectra have a differnt point counts, or a different crop range.")
                End If
            Else
                ' This is the first Spectroscopy-Table in the list.
                ValueCountX = CurrentXColumnCropped.Count
                FirstXColumnWithoutCroppedValues = CurrentXColumnCropped
                FirstXColumn = Me.lSpectroscopyTables(i).Column(ColumnNameX)
            End If

            ' Create hight-value-array.
            Dim HeightValues As List(Of Double)

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

            ' Check, if Data has to be taken the other way around
            HeightValues = CurrentValueColumnCropped
            If Monotonicity = cSpectroscopyTable.DataColumn.Monotonicities.StrictFalling Then
                HeightValues.Reverse()
            End If

            '######################
            ' Interpolate Data
            ' Generate input X-Column for interpolation
            Dim InputXColumn As New List(Of Double)(HeightValues.Count)
            For x As Integer = 0 To HeightValues.Count - 1 Step 1
                InputXColumn.Add(x)
            Next

            ' Generate interpolated X-Column
            Dim InterpolatedX As New List(Of Double)(ColumnsOfValueMatrix)
            For x As Integer = 0 To ColumnsOfValueMatrix - 1 Step 1
                InterpolatedX.Add(x * InputXColumn.Count / ColumnsOfValueMatrix)
            Next

            Dim InterpolatedValues(InterpolatedX.Count - 1) As Double
            cNumericalMethods.SplineInterpolationNative(InputXColumn.ToArray,
                                                        HeightValues.ToArray,
                                                        InterpolatedX.ToArray,
                                                        InterpolatedValues)
            ' END INTERPOLATE DATA
            '######################
            For j As Integer = 0 To ColumnsOfValueMatrix - 1 Step 1
                Me.ValueMatrix.Item(0, j) = InterpolatedValues(j)
            Next

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

        If Not FirstXColumnWithoutCroppedValues Is Nothing Then
            ' Activate Antialiasing
            DrawSurface.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            ' Get the Axis-Range
            Dim LeftValue As Double = FirstXColumnWithoutCroppedValues.Min
            Dim RightValue As Double = FirstXColumnWithoutCroppedValues.Max
            If LeftValue > RightValue Then
                Dim TmpLeft As Double = LeftValue
                LeftValue = RightValue
                RightValue = TmpLeft
            End If
            Dim AxesRange As Double = RightValue - LeftValue

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
            Dim AxesLabel_X As String = ColumnNameX & " (" & cUnits.GetPrefix(AxesRange).Key & cUnits.GetUnitSymbolFromType(FirstXColumn.UnitType) & ")"

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
                    AxesTickLabelValue = LeftValue * FormatFactor + DrawTickX * AxesRangePerPixel

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

        End If


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
