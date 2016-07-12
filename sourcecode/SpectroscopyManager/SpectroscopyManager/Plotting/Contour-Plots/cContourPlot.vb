Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports MathNet.Numerics

''' <summary>
''' Class to plot a matrix of float-values using a certain color-scheme.
''' </summary>
Public Class cContourPlot
    Implements IDisposable

#Region "General Properties"

    ''' <summary>
    ''' List, where the Contour-Plot-Values are saved.
    ''' </summary>
    Protected ValueMatrix As LinearAlgebra.Double.DenseMatrix

    ''' <summary>
    ''' Returns the currently plotted value matrix,
    ''' which has only points according to the plot dimensions,
    ''' so which is already interpolated.
    ''' </summary>
    Public ReadOnly Property ValueMatrixPlotted As LinearAlgebra.Double.DenseMatrix
        Get
            Return Me.ValueMatrix
        End Get
    End Property

    ''' <summary>
    ''' Saves a performance tuned version of the image.
    ''' </summary>
    Protected oFastImage As cFastImage

    ''' <summary>
    ''' Returns the current colorized Image.
    ''' </summary>
    Public ReadOnly Property Image As Bitmap
        Get
            If Me.oFastImage Is Nothing Then Return New Bitmap(1, 1)
            Return Me.oFastImage.Bitmap
        End Get
    End Property

    Private _ColorScheme As cColorScheme
    ''' <summary>
    ''' ColorScheme used to display z-values
    ''' </summary>
    Public Property ColorScheme() As cColorScheme
        Get
            Return Me._ColorScheme
        End Get
        Set(value As cColorScheme)
            Me._ColorScheme = value
            Me.DisposeBrushes()
            Me._BrushArray = Me._ColorScheme.BrushArray
        End Set
    End Property

    Private _BrushArray As SolidBrush()
    ''' <summary>
    ''' Brush-Array created from the color-scheme.
    ''' </summary>
    Public ReadOnly Property BrushArray As SolidBrush()
        Get
            Return Me._BrushArray
        End Get
    End Property

    ''' <summary>
    ''' Disposes all brushes!
    ''' </summary>
    Private Sub DisposeBrushes()
        ' Dispose the old brushes.
        If Me._BrushArray IsNot Nothing Then
            For i As Integer = 0 To Me._BrushArray.Length - 1 Step 1
                Me._BrushArray(i).Dispose()
            Next
        End If
    End Sub

#End Region

#Region "Constructors/Destructors"

    ''' <summary>
    ''' Takes the Specified Values into the Matrix
    ''' </summary>
    Public Sub New(ByRef InputValueMatrix As LinearAlgebra.Double.DenseMatrix)

        ' Set the initial color-scheme:
        Me._ColorScheme = cColorScheme.Gray

        ' Store the value-matrix to plot
        Me.ValueMatrix = InputValueMatrix

    End Sub

    ''' <summary>
    ''' Destructor to free memory.
    ''' </summary>
    Public Sub Destructor() Implements IDisposable.Dispose
        Me.DisposeBrushes()
    End Sub

#End Region

#Region "2D Plot-Functions"
    ''' <summary>
    ''' Creates a 2D Contour-Plot of the Value-Matrix.
    ''' Input are the Values, that should be taken as Maximum-Color-Intensity
    ''' and Minimum-Color-Intensity.
    ''' 
    ''' If the plot-ranges are set to Double.NaN we will take the full range as spectrum.
    ''' </summary>
    Public Function Plot2D(ByVal MaxIntensityCorrespondingValue As Double,
                           ByVal MinIntensityCorrespondingValue As Double) As Bitmap

        ' Check the plot ranges
        If Double.IsNaN(MaxIntensityCorrespondingValue) Then
            MaxIntensityCorrespondingValue = cNumericalMethods.GetMaximumValue(Me.ValueMatrix.Values)
        End If
        If Double.IsNaN(MinIntensityCorrespondingValue) Then
            MinIntensityCorrespondingValue = cNumericalMethods.GetMinimumValue(Me.ValueMatrix.Values)
        End If

        ' Create new graphics surface from memory bitmap
        Me.oFastImage = New cFastImage(New Bitmap(Me.ValueMatrix.ColumnCount, Me.ValueMatrix.RowCount))

        Try
            ' Lock the bitmap for editing.
            Me.oFastImage.Lock()

            ' set background color to white so that pixels can be correctly colorized!
            Me.oFastImage.Clear(Color.White)

            ' Check, if we have brushes defined.
            If Me._BrushArray Is Nothing Then Exit Try
            If Me._BrushArray.Length <= 0 Then Exit Try

            ' Exchange Max and Min, if values are in wrong order:
            If MaxIntensityCorrespondingValue < MinIntensityCorrespondingValue Then
                Dim tmp As Double = MinIntensityCorrespondingValue
                MinIntensityCorrespondingValue = MaxIntensityCorrespondingValue
                MaxIntensityCorrespondingValue = tmp
            ElseIf MaxIntensityCorrespondingValue = MinIntensityCorrespondingValue Then
                ' Do nothing!
                Exit Try
            End If

            Dim zz As Double
            ' Draw the pixels from the Brush-Colors, depending on the Value
            ' with respect to the Min and Max to show Value. Ignore the Pixel,
            ' if the Float-Mesh does not contain a value, or if the values
            ' are larger or smaller than the Min and Max Val.
            Dim PlotColorBrush As SolidBrush
            For x As Integer = 0 To Me.ValueMatrix.ColumnCount - 1 Step 1
                For y As Integer = 0 To Me.ValueMatrix.RowCount - 1 Step 1
                    zz = Me.ValueMatrix(y, x) 'meshF(x, y)

                    If Double.IsNaN(zz) Or Double.IsInfinity(zz) Then Continue For
                    If zz > MaxIntensityCorrespondingValue Then Continue For
                    If zz < MinIntensityCorrespondingValue Then Continue For

                    PlotColorBrush = cColorScheme.GetPlotColorFromColorScale(MaxIntensityCorrespondingValue,
                                                                             MinIntensityCorrespondingValue,
                                                                             zz, Me._BrushArray)

                    If Not PlotColorBrush Is Nothing Then
                        Me.oFastImage.SetPixel(x, y, PlotColorBrush.Color)
                    End If
                Next
            Next
        Catch ex As Exception
            Debug.WriteLine("cContourPlot->2DPlot error: " & ex.Message)
        Finally
            ' Always unlock the bitmap and save modified pixels!
            Me.oFastImage.Unlock(True)
        End Try

        Return Me.Image
    End Function

#End Region

#Region "Scale Bars and Axes"

    ''' <summary>
    ''' This function creates a 2D-Scale bar in the contour plot.
    ''' </summary>
    Protected Sub CreateHorizontalScaleBar(ByRef Image As cFastImage,
                                           ByVal XRangeOverWholeWidth As Double)

        ' Get the draw-surface
        Dim DrawSurface As Graphics = Graphics.FromImage(Image.Bitmap)

        ' Activate Antialiasing
        DrawSurface.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        ' DEFAULT POSITION: we place it in X direction, so take 10% of the image width as reference frame
        Dim ScaleBarPosition As New Point(Convert.ToInt32(0.1 * DrawSurface.VisibleClipBounds.Width), Convert.ToInt32(0.95 * DrawSurface.VisibleClipBounds.Height))
        Dim ScaleBarWidth As Integer = Convert.ToInt32(0.2 * DrawSurface.VisibleClipBounds.Width)
        Dim ScaleBarLimitersHeight_Large As Integer = Convert.ToInt32(0.1 * ScaleBarWidth)
        Dim ScaleBarLimitersHeight_Small As Integer = Convert.ToInt32(0.05 * ScaleBarWidth)
        Dim ScaleBarPenWidth = Convert.ToInt32(0.03 * ScaleBarWidth)
        Dim ScaleBarPenHalf As Integer = Convert.ToInt32(ScaleBarPenWidth / 2)

        ' Get the text of the label
        Dim ScaleBarLabelValue As Double = ScaleBarWidth * XRangeOverWholeWidth / DrawSurface.VisibleClipBounds.Width
        Dim FormattedScaleBarLabelValue As KeyValuePair(Of String, Double) = cUnits.GetPrefix(ScaleBarLabelValue)

        ' Cut the decimal places for values larger 100 ... they are then unnecessary!
        Dim ScaleBarValueFormatted As String
        If FormattedScaleBarLabelValue.Value >= 100 Or CInt(FormattedScaleBarLabelValue.Value * 10) Mod 10 = 0 Then
            ScaleBarValueFormatted = Math.Round(FormattedScaleBarLabelValue.Value, 0).ToString("F0", Globalization.CultureInfo.InvariantCulture)
        Else
            ScaleBarValueFormatted = Math.Round(FormattedScaleBarLabelValue.Value, 1).ToString("F1", Globalization.CultureInfo.InvariantCulture)
        End If
        Dim ScaleBarLabelText As String = ScaleBarValueFormatted & " " & FormattedScaleBarLabelValue.Key &
                                          cUnits.GetUnitSymbolFromType(cUnits.UnitType.Length)

        ' Get the size of the label text
        If ScaleBarLimitersHeight_Large = 0 Then ScaleBarLimitersHeight_Large = 1
        Dim ScaleBarLabelFont As Font = New System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif,
                                                                ScaleBarLimitersHeight_Large * 3)
        Dim ScaleBarLabelStringSize As SizeF = DrawSurface.MeasureString(ScaleBarLabelText, ScaleBarLabelFont)

        ' Get the position of the scale-bar-label
        Dim ScaleBarLabelPosition As New PointF(Convert.ToSingle(ScaleBarPosition.X + ScaleBarWidth / 2 - ScaleBarLabelStringSize.Width / 2),
                                                ScaleBarPosition.Y - ScaleBarLabelStringSize.Height)

        ' Get the color of the scale-bar:
        Dim ScaleBarPen As Pen
        Try
            Image.Lock()
            ScaleBarPen = New Pen(cColorHelper.InvertColor(Image.GetPixel(ScaleBarLabelPosition)), 2)
        Catch ex As Exception
            Debug.WriteLine("Failed to get scale bar pen: " & ex.Message)
            ScaleBarPen = New Pen(Color.Black)
        Finally
            Image.Unlock(False)
        End Try

        With ScaleBarPosition
            ' Draw line:
            ScaleBarPen.Width = ScaleBarPenHalf * 4
            DrawSurface.DrawLine(ScaleBarPen, .X, .Y, .X + ScaleBarWidth, .Y)
            ' draw large limiters at the end.
            ScaleBarPen.Width = ScaleBarPenHalf * 2
            DrawSurface.DrawLine(ScaleBarPen, .X + ScaleBarPenHalf, .Y, .X + ScaleBarPenHalf, .Y + ScaleBarLimitersHeight_Large)
            DrawSurface.DrawLine(ScaleBarPen, .X + ScaleBarWidth - ScaleBarPenHalf, .Y, .X + ScaleBarWidth - ScaleBarPenHalf, .Y + ScaleBarLimitersHeight_Large)
            ' draw small limiter in the middle, if size is large enough (2* 2px for the limiters at the end, +2 for the central one +2x2px space)
            If ScaleBarWidth > 5 * ScaleBarPen.Width Then
                DrawSurface.DrawLine(ScaleBarPen, .X + CInt(ScaleBarWidth / 2), .Y, .X + CInt(ScaleBarWidth / 2), .Y + ScaleBarLimitersHeight_Small)
            End If

            ' Draw the scale text
            DrawSurface.DrawString(ScaleBarLabelText,
                                   ScaleBarLabelFont,
                                   ScaleBarPen.Brush,
                                   ScaleBarLabelPosition)

        End With

        ' Dispose the pen and the brush
        ScaleBarPen.Dispose()
        ScaleBarPen = Nothing

        ' Dispose the draw-surface
        DrawSurface.Dispose()
    End Sub

    '''' <summary>
    '''' This function creates an information bar in the upper part of the image.
    '''' </summary>
    'Protected Sub CreateImageInfoBar(ByRef Image As cFastImage,
    '                                 ByVal XRangeOverWholeWidth As Double,
    '                                 ByVal YRangeOverWholeHeight As Double,
    '                                 ByRef ColorScheme As cColorScheme)

    '    ' Get the draw-surface
    '    Dim DrawSurface As Graphics = Graphics.FromImage(Image.Bitmap)

    '    ' Activate Antialiasing
    '    DrawSurface.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

    '    ' DEFAULT POSITION: we place it on the top of the image 1% of the image height.
    '    Dim InfoBarPosition As New Point(0, 0)
    '    Dim InfoBarWidth As Integer = Convert.ToInt32(DrawSurface.VisibleClipBounds.Width)
    '    Dim InfoBarHeight As Integer = Convert.ToInt32(0.01 * DrawSurface.VisibleClipBounds.Height)


    '    ' Get the text of the label
    '    Dim FormattedScaleWidth As KeyValuePair(Of String, Double) = cUnits.GetPrefix(XRangeOverWholeWidth)
    '    Dim FormattedScaleHeight As KeyValuePair(Of String, Double) = cUnits.GetPrefix(YRangeOverWholeHeight)
    '    Dim ScaleInfoText As New System.Text.StringBuilder()
    '    With ScaleInfoText
    '        .Append(FormattedScaleWidth.Value.ToString("F0"))
    '        .Append(" x ")
    '        .Append(FormattedScaleHeight.Value.ToString("F0"))
    '    End With


    '    & " x " & FormattedScaleBarLabelValue.Key &
    '                                      cUnits.GetUnitSymbolFromType(cUnits.UnitType.Length)


    '    Dim ScaleBarLimitersHeight_Large As Integer = Convert.ToInt32(0.1 * ScaleBarWidth)
    '    Dim ScaleBarLimitersHeight_Small As Integer = Convert.ToInt32(0.05 * ScaleBarWidth)
    '    Dim ScaleBarPenWidth = Convert.ToInt32(0.03 * ScaleBarWidth)
    '    Dim ScaleBarPenHalf As Integer = Convert.ToInt32(ScaleBarPenWidth / 2)






    '    ' Get the size of the label text
    '    If ScaleBarLimitersHeight_Large = 0 Then ScaleBarLimitersHeight_Large = 1
    '    Dim ScaleBarLabelFont As Font = New System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif,
    '                                                            ScaleBarLimitersHeight_Large * 3)
    '    Dim ScaleBarLabelStringSize As SizeF = DrawSurface.MeasureString(ScaleBarLabelText, ScaleBarLabelFont)

    '    ' Get the position of the scale-bar-label
    '    Dim ScaleBarLabelPosition As New PointF(Convert.ToSingle(ScaleBarPosition.X + ScaleBarWidth / 2 - ScaleBarLabelStringSize.Width / 2),
    '                                            ScaleBarPosition.Y - ScaleBarLabelStringSize.Height)

    '    ' Get the color of the scale-bar:
    '    Image.Lock()
    '    Dim ScaleBarPen As New Pen(cColorHelper.InvertColor(Image.GetPixel(ScaleBarLabelPosition)), 2)
    '    Image.Unlock(False)

    '    With ScaleBarPosition
    '        ' Draw line:
    '        ScaleBarPen.Width = ScaleBarPenHalf * 4
    '        DrawSurface.DrawLine(ScaleBarPen, .X, .Y, .X + ScaleBarWidth, .Y)
    '        ' draw large limiters at the end.
    '        ScaleBarPen.Width = ScaleBarPenHalf * 2
    '        DrawSurface.DrawLine(ScaleBarPen, .X + ScaleBarPenHalf, .Y, .X + ScaleBarPenHalf, .Y + ScaleBarLimitersHeight_Large)
    '        DrawSurface.DrawLine(ScaleBarPen, .X + ScaleBarWidth - ScaleBarPenHalf, .Y, .X + ScaleBarWidth - ScaleBarPenHalf, .Y + ScaleBarLimitersHeight_Large)
    '        ' draw small limiter in the middle, if size is large enough (2* 2px for the limiters at the end, +2 for the central one +2x2px space)
    '        If ScaleBarWidth > 5 * ScaleBarPen.Width Then
    '            DrawSurface.DrawLine(ScaleBarPen, .X + CInt(ScaleBarWidth / 2), .Y, .X + CInt(ScaleBarWidth / 2), .Y + ScaleBarLimitersHeight_Small)
    '        End If

    '        ' Draw the scale text
    '        DrawSurface.DrawString(ScaleBarLabelText,
    '                               ScaleBarLabelFont,
    '                               ScaleBarPen.Brush,
    '                               ScaleBarLabelPosition)

    '    End With

    '    ' Dispose the pen and the brush
    '    ScaleBarPen.Dispose()
    '    ScaleBarPen = Nothing

    '    ' Dispose the draw-surface
    '    DrawSurface.Dispose()
    'End Sub

#End Region

#Region "3D specific Properties"
    Private _PenColorFor3DPolygonLines As Color = Color.Black
    ''' <summary>
    ''' Returns the Color of the Pen used for the Polygon borders.
    ''' </summary>
    Public Property PenColorFor3DPolygonLines() As Color
        Get
            Return _PenColorFor3DPolygonLines
        End Get
        Set(value As Color)
            _PenColorFor3DPolygonLines = value
        End Set
    End Property

    Private _DrawPolygonBorders As Boolean = True
    ''' <summary>
    ''' Sets, if the Polygon Borders should be drawn.
    ''' </summary>
    Public Property DrawPolygonBorders() As Boolean
        Get
            Return _DrawPolygonBorders
        End Get
        Set(value As Boolean)
            _DrawPolygonBorders = value
        End Set
    End Property

    Private ScreenDistance As Double
    Private sf As Double
    Private cf As Double
    Private st As Double
    Private ct As Double
    Private R As Double
    Private A As Double
    Private B As Double
    Private C As Double
    Private D As Double
#End Region

#Region "3D Plot-Function"
    ''' <summary>
    ''' Initializes a new instance of the 3Dplot class. Calculates transformations coeficients.
    ''' </summary>
    ''' <param name="screenDistance">The screen distance.</param>
    ''' <param name="screenWidthPhys">Width of the screen in meters.</param>
    ''' <param name="screenHeightPhys">Height of the screen in meters.</param>
    Public Sub RecalculateTransformationsCoefficientsFor3D(ObserverXPosition As Double,
                                                           ObserverYPosition As Double,
                                                           ObserverZPosition As Double,
                                                           XCoordinateOfTheScreen As Integer,
                                                           YCoordinateOfTheScreen As Integer,
                                                           ScreenWidth As Integer,
                                                           ScreenHeight As Integer,
                                                           ScreenDistance As Double,
                                                           ScreenWidthPhys As Double,
                                                           ScreenHeightPhys As Double)
        Dim r1 As Double
        Dim a__1 As Double

        ' If screen dimensions are not specified,
        ' take the Screen-Height as Physical-Height
        ' 0.0257 m = 1 inch. Screen has 72 px/inch
        If ScreenWidthPhys <= 0 Then
            ScreenWidthPhys = ScreenWidth * 0.0257 / 72.0
        End If
        If ScreenHeightPhys <= 0 Then
            ScreenHeightPhys = ScreenHeight * 0.0257 / 72.0
        End If

        r1 = ObserverXPosition * ObserverXPosition + ObserverYPosition * ObserverYPosition
        a__1 = Math.Sqrt(r1)

        ' Calctulate the distance in XY plane
        R = Math.Sqrt(r1 + ObserverZPosition * ObserverZPosition)

        ' Calculate the distance from observator to center
        ' and save the rotation matrix coefficients.
        If a__1 <> 0 Then
            ' sin(fi)
            sf = ObserverYPosition / a__1
            ' cos(fi)
            cf = ObserverXPosition / a__1
        Else
            sf = 0
            cf = 1
        End If

        ' sin(theta)
        st = a__1 / R

        ' cos(theta)
        ct = ObserverZPosition / R

        ' Calculate the linear transformation coefficients.
        A = ScreenWidth / ScreenWidthPhys
        B = XCoordinateOfTheScreen + A * ScreenWidthPhys / 2.0
        C = -CDbl(ScreenHeight) / ScreenHeightPhys
        D = YCoordinateOfTheScreen - C * ScreenHeightPhys / 2.0

        Me.ScreenDistance = ScreenDistance
    End Sub

    ''' <summary>
    ''' Performs the projection of a Point to the Drawing Area.
    ''' Calculates screen coordinates for 3D point.
    ''' </summary>
    ''' <param name="x">Point's x coordinate.</param>
    ''' <param name="y">Point's y coordinate.</param>
    ''' <param name="z">Point's z coordinate.</param>
    ''' <returns>Point in 2D space of the screen.</returns>
    Public Function CalculateProjection(x As Double,
                                        y As Double,
                                        z As Double) As PointF

        ' Calculate the point coordinates in computer's frame of reference
        Dim xn As Double
        Dim yn As Double
        Dim zn As Double

        xn = -sf * x + cf * y
        yn = -cf * ct * x - sf * ct * y + st * z
        zn = -cf * st * x - sf * st * y - ct * z + R

        If zn = 0 Then
            zn = 0.01
        End If

        ' Tales' theorem
        Return New PointF(CSng(A * xn * ScreenDistance / zn + B), CSng(C * yn * ScreenDistance / zn + D))
    End Function

    ''' <summary>
    ''' Creates a 3D Contour-Plot of the Value-Matrix.
    ''' Input are the Values, that should be taken as Maximum-Color-Intensity
    ''' and Minimum-Color-Intensity.
    ''' </summary>
    Public Function Plot3D(ByVal MaxIntensityCorrespondingValue As Double,
                           ByVal MinIntensityCorrespondingValue As Double,
                           ByVal RangeX As Double,
                           ByVal RangeY As Double) As Bitmap
        ' Get Array of Brushes from the Selected ColorSchema
        Dim brushes As SolidBrush() = Me._ColorScheme.BrushArray

        ' Create new graphics surface from memory bitmap
        Dim bImage As New Bitmap(Me.ValueMatrix.ColumnCount, Me.ValueMatrix.RowCount)
        Dim DrawSurface As Graphics = Graphics.FromImage(bImage)

        ' Set background color to white so that pixels can be correctly colorized
        DrawSurface.Clear(Color.Transparent)

        ' Create Polygon Object and neighboring value-holders
        Dim z1 As Double
        Dim z2 As Double
        Dim Polygon As PointF() = New PointF(3) {}

        ' Calculate the value that corresponds to a change of one unit in intensity
        ' by the length of the available brushes in the ColorSchema
        Dim IntensityChangeFactor As Double = Convert.ToDouble((MaxIntensityCorrespondingValue - MinIntensityCorrespondingValue) / (brushes.Length - 1.0))
        If IntensityChangeFactor = 0 Then Return bImage

        Dim zz As Double
        Dim meshF As PointF(,) = New PointF(Me.ValueMatrix.ColumnCount - 1, Me.ValueMatrix.RowCount - 1) {}
        ' Add a value to the Float-Mesh, if the Values in the ValueMatrix are not
        ' NaN or Infinity, since these are not paintable.
        Dim xi As Double = RangeX / Me.ValueMatrix.ColumnCount
        Dim yi As Double = RangeY / Me.ValueMatrix.RowCount
        For x As Integer = 0 To Me.ValueMatrix.ColumnCount - 1 Step 1
            For y As Integer = 0 To Me.ValueMatrix.RowCount - 1 Step 1
                zz = Me.ValueMatrix(y, x)
                If Double.IsNaN(zz) Or Double.IsInfinity(zz) Then Continue For
                meshF(x, y) = CalculateProjection(x * xi, y * yi, zz)
            Next
        Next

        ' Draw the pixels from the Brush-Colors, depending on the Value
        ' in respect to the Min and Max to show Value. Ignore the Pixel,
        ' if the Float-Mesh does not contain a value, or if the values
        ' are larger or smaller than the Min and Max Val.
        Using PenForPolygonLines As New Pen(_PenColorFor3DPolygonLines)
            For x As Integer = 0 To Me.ValueMatrix.ColumnCount - 2 Step 1
                For y As Integer = 0 To Me.ValueMatrix.RowCount - 2 Step 1
                    z1 = Me.ValueMatrix(x, y)
                    z2 = Me.ValueMatrix(x, y + 1)

                    If Double.IsNaN(z1) Or Double.IsNaN(z2) Then Continue For
                    If zz > MaxIntensityCorrespondingValue Then Continue For
                    If zz < MinIntensityCorrespondingValue Then Continue For

                    If Double.IsNaN(meshF(x, y).X) Or Double.IsNaN(meshF(x, y).Y) Then Continue For
                    If Double.IsNaN(meshF(x + 1, y).X) Or Double.IsNaN(meshF(x + 1, y).Y) Then Continue For
                    If Double.IsNaN(meshF(x + 1, y + 1).X) Or Double.IsNaN(meshF(x + 1, y + 1).Y) Then Continue For
                    If Double.IsNaN(meshF(x, y + 1).X) Or Double.IsNaN(meshF(x, y + 1).Y) Then Continue For

                    Polygon(0) = meshF(x, y)
                    Polygon(1) = meshF(x, y + 1)
                    Polygon(2) = meshF(x + 1, y + 1)
                    Polygon(3) = meshF(x + 1, y)

                    DrawSurface.SmoothingMode = SmoothingMode.None
                    Dim AverageZValue As Double = (z1 + z2) / 2.0
                    Dim BrushNumber As Integer = CInt(Math.Truncate((AverageZValue - MinIntensityCorrespondingValue) / IntensityChangeFactor))
                    DrawSurface.FillPolygon(brushes(BrushNumber), Polygon)

                    If Me._DrawPolygonBorders Then
                        DrawSurface.SmoothingMode = SmoothingMode.AntiAlias
                        DrawSurface.DrawPolygon(PenForPolygonLines, Polygon)
                    End If
                Next
            Next
        End Using

        Return bImage
    End Function
#End Region

#Region "Image-Rescale Functions"
    ''' <summary>
    ''' Resizes an Image preserving the Scale.
    ''' </summary>
    Public Shared Function ResizeImageByHeight(ByVal ImageObject As System.Drawing.Bitmap,
                                               ByVal TargetHeight As Integer) As System.Drawing.Bitmap
        Dim TargetWidth As Integer = ImageObject.Width
        If TargetHeight <> 0 Then
            TargetWidth = Convert.ToInt32(TargetHeight / ImageObject.Height * TargetWidth)
        End If
        Return cContourPlot.ResizeImage(ImageObject, TargetWidth, TargetHeight)
    End Function

    ''' <summary>
    ''' Resizes an Image preserving the Scale.
    ''' </summary>
    Public Shared Function ResizeImageByWidth(ByVal ImageObject As System.Drawing.Bitmap,
                                              ByVal TargetWidth As Integer) As System.Drawing.Bitmap
        Dim TargetHeight As Integer = ImageObject.Height
        If TargetWidth <> 0 Then
            TargetHeight = Convert.ToInt32(TargetWidth / ImageObject.Width * TargetHeight)
        End If
        Return cContourPlot.ResizeImage(ImageObject, TargetWidth, TargetHeight)
    End Function

    ''' <summary>
    ''' Resizes an image using bicubic interpolation.
    ''' </summary>
    Public Shared Function ResizeImage(ByVal ImageObject As Bitmap,
                                       ByVal TargetWidth As Integer,
                                       ByVal TargetHeight As Integer,
                                       Optional ByVal UseHighQualityImageProcessing As Boolean = True) As Bitmap

        ' Area, where the result is painted in.
        Dim result As New Bitmap(TargetWidth, TargetHeight)

        ' use a graphics object to draw the resized image into the bitmap
        Using gr As Graphics = Graphics.FromImage(result)
            If UseHighQualityImageProcessing Then
                ' set the resize quality modes to high quality
                gr.CompositingQuality = CompositingQuality.HighQuality
                gr.InterpolationMode = InterpolationMode.Bicubic ' InterpolationMode
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality
                gr.SmoothingMode = SmoothingMode.AntiAlias
            Else
                ' set the resize quality modes to high speed quality
                gr.CompositingQuality = CompositingQuality.HighSpeed
                gr.InterpolationMode = InterpolationMode.NearestNeighbor
                gr.PixelOffsetMode = PixelOffsetMode.HighSpeed
                gr.SmoothingMode = SmoothingMode.HighSpeed
            End If

            ' draw the image into the resized target bitmap
            gr.DrawImage(ImageObject, 0, 0, result.Width, result.Height)
        End Using

        ' return the resulting bitmap
        Return result


        'Dim RS As New ResampleImage.ResamplingService
        'RS.Filter = ResampleImage.ResamplingFilters.Lanczos8
        'Dim input As UShort()(,) = ResampleImage.ResamplingService.ConvertBitmapToArray(ImageObject)

        'Dim Output As UShort()(,) = RS.Resample(input, TargetWidth, TargetHeight)

        'Return ResampleImage.ResamplingService.ConvertArrayToBitmap(Output)
    End Function

    ''' <summary>
    ''' Fits an Image into the PictureBox by Resizing it.
    ''' </summary>
    Public Shared Function AutosizeImage(ByVal Image As Bitmap,
                                         ByVal TargetWidth As Integer,
                                         ByVal TargetHeight As Integer,
                                         Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage) As Bitmap
        Dim imgShow As Bitmap
        Dim g As Graphics
        Dim divideBy, divideByH, divideByW As Double

        divideByW = Image.Width / TargetWidth
        divideByH = Image.Height / TargetHeight
        If divideByW > 1 Or divideByH > 1 Then
            If divideByW > divideByH Then
                divideBy = divideByW
            Else
                divideBy = divideByH
            End If

            imgShow = New Bitmap(CInt(CDbl(Image.Width) / divideBy), CInt(CDbl(Image.Height) / divideBy))
            imgShow.SetResolution(Image.HorizontalResolution, Image.VerticalResolution)
            g = Graphics.FromImage(imgShow)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.DrawImage(Image, New Rectangle(0, 0, CInt(CDbl(Image.Width) / divideBy), CInt(CDbl(Image.Height) / divideBy)), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel)
            g.Dispose()
        Else
            imgShow = New Bitmap(Image.Width, Image.Height)
            imgShow.SetResolution(Image.HorizontalResolution, Image.VerticalResolution)
            g = Graphics.FromImage(imgShow)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.DrawImage(Image, New Rectangle(0, 0, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel)
            g.Dispose()
        End If
        Return imgShow
    End Function
#End Region

End Class
