Public Class cScanImagePlot
    Inherits cContourPlot

#Region "Properties"

    ''' <summary>
    ''' Object for the ScanImage to Plot
    ''' </summary>
    Private oScanImages As List(Of cScanImage)

    ''' <summary>
    ''' List with all scan-image filters to apply
    ''' </summary>
    Public Property ScanImageFiltersToApplyBeforePlot As List(Of iScanImageFilter)

    ''' <summary>
    ''' Constructor: Save ScanImage Reference.
    ''' </summary>
    Public Sub New(ByRef ScanImage As cScanImage)
        ' Call Constructor of Base Class
        MyBase.New(MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(1, 1, 0))

        ' Save ScanImage-Reference.
        Me.oScanImages = New List(Of cScanImage)
        Me.oScanImages.Add(ScanImage)
    End Sub

    ''' <summary>
    ''' Constructor: Save ScanImage References.
    ''' </summary>
    Public Sub New(ByRef ScanImages As List(Of cScanImage))
        ' Call Constructor of Base Class
        MyBase.New(MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(1, 1, 0))

        ' Save ScanImage-Reference.
        Me.oScanImages = ScanImages
    End Sub

    ''' <summary>
    ''' Scan-Image currently plotted
    ''' </summary>
    Private ScanImage As cScanImage

    ''' <summary>
    ''' Scan-Image that is currently plotted.
    ''' May be a combined one from several images.
    ''' </summary>
    Public ReadOnly Property ScanImagePlotted As cScanImage
        Get
            Return Me.ScanImage
        End Get
    End Property

    ''' <summary>
    ''' Scan-Channel, as plotted in the image.
    ''' </summary>
    Public ScanChannelPlotted As cScanImage.ScanChannel

    ''' <summary>
    ''' Variable determines if a scale bar should be plotted to the image.
    ''' </summary>
    Public Property PlotScaleBar As Boolean = True

    ''' <summary>
    ''' Universal range per pixel used to plot the image.
    ''' Just one value, because we dont want to stretch the image.
    ''' </summary>
    Private _RangePerPixelUsed As Double

    ''' <summary>
    ''' Returns the current scale.
    ''' </summary>
    Public ReadOnly Property RangePerPixelUsed As Double
        Get
            Return Me._RangePerPixelUsed
        End Get
    End Property

    ''' <summary>
    ''' Stores the finally used pixels to plot the scan-image.
    ''' </summary>
    Private PixelsIn_X_ToKeep As Integer

    ''' <summary>
    ''' Stores the finally used pixels to plot the scan-image.
    ''' </summary>
    Private PixelsIn_Y_ToKeep As Integer

#End Region

#Region "Point Marks"
    ''' <summary>
    ''' Saves Points to mark on the Image.
    ''' </summary>
    Private Property ListOfPointMarks As New List(Of PointMark)

    ''' <summary>
    ''' Font with which the labels of points are plotted to the image.
    ''' </summary>
    Public Property PointMarkFont As New Font("Arial", 10.0F, FontStyle.Bold, GraphicsUnit.Point)

    ''' <summary>
    ''' If set to true the labels of point-marks are plotted to the points.
    ''' </summary>
    Public Property PlotPointMarkLabels As Boolean = False

    ''' <summary>
    ''' Structure used to mark points on the scan-image plot.
    ''' </summary>
    Public Class PointMark

        Public X As Double
        Public Y As Double
        Public Z As Double
        Public PlotRadiusInScale As Double

        Public Label As String

        Public Color As Color
        Public Shape As PointMarkShapes

        ''' <summary>
        ''' Coordinates at which the algorithm placed the PointMark for plotting it in the image.
        ''' </summary>
        Public PlotPoint As Point

        ''' <summary>
        ''' Color with which the algorithm decided to plot the point.
        ''' </summary>
        Public PlotColor As Color

        ''' <summary>
        ''' Radius with which the point is plotted.
        ''' </summary>
        Public PlotRadiusInPixel As Integer

        ''' <summary>
        ''' Constructor for the point-mark.
        ''' </summary>
        ''' <param name="X">Location of the point mark in location-coordinates.</param>
        ''' <param name="Y">Location of the point mark in location-coordinates.</param>
        ''' <param name="Z">Location of the point mark in location-coordinates.</param>
        ''' <param name="Color">
        ''' Color of the point mark. If set to nothing,
        ''' the inverted image color will be taken.
        ''' </param>
        ''' <param name="Shape">Shape the point mark is drawn as.</param>
        Public Sub New(ByVal X As Double,
                       ByVal Y As Double,
                       ByVal Z As Double,
                       Optional ByVal PlotRadiusInScale As Double = -1,
                       Optional ByVal Color As Color = Nothing,
                       Optional ByVal Shape As PointMarkShapes = PointMarkShapes.Cross,
                       Optional ByVal Label As String = Nothing)
            Me.X = X
            Me.Y = Y
            Me.Z = Z
            Me.Color = Color
            Me.Shape = Shape
            Me.PlotRadiusInScale = PlotRadiusInScale
            Me.Label = Label
        End Sub

        ''' <summary>
        ''' Shape to plot the point-mark as.
        ''' </summary>
        Public Enum PointMarkShapes
            Cross
            CircleEmpty
            CircleFilled
        End Enum

    End Class

#End Region

#Region "Text Objects"
    ''' <summary>
    ''' Saves TextObjects on the Image.
    ''' </summary>
    Public Property ListOfTextObjects As New List(Of TextObject)

    ''' <summary>
    ''' Adds a TextObjects to the Display-List
    ''' </summary>
    Public Sub AddTextObject(Text As TextObject)
        If Not Me.ListOfTextObjects.Contains(Text) Then
            Me.ListOfTextObjects.Add(Text)
        End If
    End Sub

    ''' <summary>
    ''' Adds a TextObjects to the Display-List
    ''' </summary>
    Public Sub AddTextObjects(PointMarks As List(Of TextObject))
        For Each Text As TextObject In PointMarks
            Me.AddTextObject(Text)
        Next
    End Sub

    ''' <summary>
    ''' Removes all entries from the TextObjects-List
    ''' </summary>
    Public Sub ClearTextObjectList()
        Me.ListOfTextObjects.Clear()
    End Sub

    ''' <summary>
    ''' Structure used to mark points on the scan-image plot.
    ''' </summary>
    Public Class TextObject

        ''' <summary>
        ''' Text to show.
        ''' </summary>
        Public Text As String = String.Empty

        ''' <summary>
        ''' Color to paint the text in.
        ''' </summary>
        Public Color As Color

        ''' <summary>
        ''' Font with which the labels of points are plotted to the image.
        ''' </summary>
        Public Font As New Font("Arial", 10.0F, FontStyle.Bold, GraphicsUnit.Point)

        ''' <summary>
        ''' Coordinates at which the algorithm placed for plotting it in the image.
        ''' </summary>
        Public PlotPoint As PointF

        ''' <summary>
        ''' Determines if the value is given as absolute value or relative.
        ''' </summary>
        Public ValueIsAbsolute As Boolean = True

        ''' <summary>
        ''' Determines if the font size should be given relative or absolute.
        ''' If zero, it uses absolute font size. Given as percent!
        ''' </summary>
        Public UseRelativeFontSize As Integer = 0

        ''' <summary>
        ''' If automatic, it will choose the color from the background.
        ''' </summary>
        Public ChooseColorAutomatic As Boolean = True

        ''' <summary>
        ''' Constructor for the TextObject
        ''' </summary>
        Public Sub New(ByVal PlotPoint As PointF,
                       ByVal Text As String)
            Me.Text = Text
            Me.PlotPoint = PlotPoint
        End Sub

    End Class

#End Region

#Region "Draw Function"

    ''' <summary>
    ''' Creates an image of the scan from the object of the ScanImage
    ''' using the choosen Scan-Channel and direction. Preserves the scale
    ''' of the image by reducing or extending the matrix to the given Bitmap
    ''' size. Filling the empty parts with NaN.
    ''' </summary>
    Public Shadows Function CreateImage(ByVal MaxIntensityCorrespondingToValue As Double,
                                        ByVal MinIntensityCorrespondingToValue As Double,
                                        ByVal ScanChannelName As String,
                                        ByVal TargetWidth As Integer,
                                        ByVal TargetHeight As Integer,
                                        ByVal UseHighQualityImageProcessing As Boolean) As Bitmap

        ' Set the ScanImage-Object to plot.
        ' If we plot just a single image,
        ' we can directly set this image.
        ' If we should plot a list,
        ' we have to create first a combined image.
        If Me.oScanImages.Count <= 0 Then
            Return Nothing
        ElseIf Me.oScanImages.Count = 1 Then
            ScanImage = Me.oScanImages(0)
        Else

            ' Create a combined scan-image from all the input-scan-images.
            ScanImage = cScanImage.CombineScanChannels(Me.oScanImages,
                                                       ScanChannelName,
                                                       TargetWidth, TargetHeight)

        End If

        If Not ScanImage.ScanChannels.ContainsKey(ScanChannelName) Then Return New Bitmap(1, 1)

        ' Getting the scan-channel reference
        Dim ScanChannel As cScanImage.ScanChannel = ScanImage.ScanChannels(ScanChannelName)

        ' Apply now the scan-image filters:
        If Me.ScanImageFiltersToApplyBeforePlot IsNot Nothing Then
            ScanChannel = ScanChannel.GetCopy
            ScanChannel.ClearFilters()

            For i As Integer = 0 To Me.ScanImageFiltersToApplyBeforePlot.Count - 1 Step 1
                Dim Filter As iScanImageFilter = Me.ScanImageFiltersToApplyBeforePlot(i)

                ScanChannel.AddFilter(Filter)
            Next

            ' Apply all filters!
            ScanChannel.FilterChannel_Direct()
        End If

        ' Compress or stretch the image to obtain the correct dimensions of the surface,
        ' since the number of rows an columns has not to represent an equal distance.

        ' Calculate the scaling factor of the scan-matrix
        ' to preserve the dimensions and not stretch of quench
        ' the created image.
        Dim RangePerPixel_X As Double = TargetWidth / ScanImage.ScanRange_X
        Dim RangePerPixel_Y As Double = TargetHeight / ScanImage.ScanRange_Y

        ' The smaller value of both gives the site of the Scan-Matrix which represents a longer range.
        If RangePerPixel_X <= RangePerPixel_Y Then
            Me._RangePerPixelUsed = RangePerPixel_X
        Else
            Me._RangePerPixelUsed = RangePerPixel_Y
        End If

        ' Calculate how many pixels have do be dropped at each site
        ' to fullfill the correct resolution of the Image.
        ' Therefore calculate first the Number of Pixels needed to keep correct dimensions.
        Me.PixelsIn_X_ToKeep = Convert.ToInt32(Me._RangePerPixelUsed * ScanImage.ScanRange_X)
        Me.PixelsIn_Y_ToKeep = Convert.ToInt32(Me._RangePerPixelUsed * ScanImage.ScanRange_Y)

        ' NEW, plot matrix as is, and then rescale it afterwards.
        Me.ValueMatrix = ScanChannel.ScanData

        ' Create Intensity-Image of the selected Scan-Channel:
        MyBase.Plot2D(MaxIntensityCorrespondingToValue, MinIntensityCorrespondingToValue)

        ' NEW: rescale it
        If Me.oFastImage.Width <> PixelsIn_X_ToKeep Or
           Me.oFastImage.Height <> PixelsIn_Y_ToKeep Then
            Me.oFastImage = New cFastImage(ResizeImage(Me.oFastImage.Bitmap, PixelsIn_X_ToKeep, PixelsIn_Y_ToKeep, UseHighQualityImageProcessing))
        End If

        ' Apply additional markers to the image.
        ' Get the Drawing Surface
        Dim DrawSurface As Graphics = Graphics.FromImage(Me.oFastImage.Bitmap)

        '##########################################################################################
        '##########################################################################################
        ' Apply Point-Marks to the Image:
        ' Important: Consider the rotation of the image!!!
        If Me.ListOfPointMarks.Count > 0 Then

            ' Get Pen-Object storage
            Dim PointColorPen As Pen = Nothing

            ' Calculate a size that is suitable for the selected image size!
            ' Use the short side of the image to get an appropriate size.
            Dim PointMarkSizeDefault As Integer
            If PixelsIn_X_ToKeep < PixelsIn_Y_ToKeep Then
                PointMarkSizeDefault = Convert.ToInt32(0.01 * PixelsIn_X_ToKeep)
            Else
                PointMarkSizeDefault = Convert.ToInt32(0.01 * PixelsIn_Y_ToKeep)
            End If
            If PointMarkSizeDefault < 5 Then PointMarkSizeDefault = 5

            ' Go through all Point-Marks, if they exist
            If Me.ListOfPointMarks.Count > 0 Then
                Try
                    ' Lock the image for reading out the pixel-colors.
                    Me.oFastImage.Lock()

                    Try

                        For Each pm As PointMark In Me.ListOfPointMarks
                            ' Get the transformed point in the scan-frame.
                            pm.PlotPoint = ScanImage.GetCoordinateInValueMatrix(pm.X, pm.Y)

                            ' Resized point-location.
                            pm.PlotPoint.X = CInt(pm.PlotPoint.X * (PixelsIn_X_ToKeep / ScanImage.ScanPixels_X))
                            pm.PlotPoint.Y = CInt(pm.PlotPoint.Y * (PixelsIn_Y_ToKeep / ScanImage.ScanPixels_Y))

                            ' Get the color of the point, if it is not specified!
                            If pm.Color = Nothing Then

                                If (pm.PlotPoint.X >= 0 And pm.PlotPoint.X < Me.oFastImage.Width) And
                                   (pm.PlotPoint.Y >= 0 And pm.PlotPoint.Y < Me.oFastImage.Height) Then

                                    pm.PlotColor = cColorHelper.InvertColor(Me.oFastImage.GetPixel(pm.PlotPoint.X, pm.PlotPoint.Y))

                                End If

                            Else
                                ' Use the predefined color.
                                pm.PlotColor = pm.Color
                            End If
                        Next
                    Catch ex As Exception
                        Debug.WriteLine("cScanImagePlot->Error getting point mark color: " & ex.Message)
                    End Try

                    ' Unlock the image for reading out the pixel-colors.
                    Me.oFastImage.Unlock(False)

                        ' Now plot the point marks to the graphics surface.
                        For Each pm As PointMark In Me.ListOfPointMarks

                            ' Get a pen to draw the point.
                            PointColorPen = New Pen(pm.PlotColor, 2)

                            ' Get the size of the point to draw.
                            ' If non is specified (-1) then set it to an appropriate value.
                            If pm.PlotRadiusInScale < 0 Then
                                pm.PlotRadiusInPixel = PointMarkSizeDefault
                            Else
                                pm.PlotRadiusInPixel = CInt(pm.PlotRadiusInScale * Me._RangePerPixelUsed)
                            End If

                            ' Draw a certain shape to the image.
                            Select Case pm.Shape

                                Case PointMark.PointMarkShapes.Cross

                                    ' Plot a cross centered at the point-mark.
                                    DrawSurface.DrawLine(PointColorPen, pm.PlotPoint.X - pm.PlotRadiusInPixel, pm.PlotPoint.Y - pm.PlotRadiusInPixel, pm.PlotPoint.X + pm.PlotRadiusInPixel, pm.PlotPoint.Y + pm.PlotRadiusInPixel)
                                    DrawSurface.DrawLine(PointColorPen, pm.PlotPoint.X - pm.PlotRadiusInPixel, pm.PlotPoint.Y + pm.PlotRadiusInPixel, pm.PlotPoint.X + pm.PlotRadiusInPixel, pm.PlotPoint.Y - pm.PlotRadiusInPixel)

                                Case PointMark.PointMarkShapes.CircleEmpty
                                    DrawSurface.DrawEllipse(PointColorPen, pm.PlotPoint.X - pm.PlotRadiusInPixel, pm.PlotPoint.Y - pm.PlotRadiusInPixel, 2 * pm.PlotRadiusInPixel, 2 * pm.PlotRadiusInPixel)

                                Case PointMark.PointMarkShapes.CircleFilled
                                    DrawSurface.FillEllipse(PointColorPen.Brush, pm.PlotPoint.X - pm.PlotRadiusInPixel, pm.PlotPoint.Y - pm.PlotRadiusInPixel, 2 * pm.PlotRadiusInPixel, 2 * pm.PlotRadiusInPixel)

                            End Select

                            ' Write the label of the point-mark.
                            If Me.PlotPointMarkLabels Then
                                If pm.Label <> Nothing Then
                                    If pm.Label <> String.Empty Then
                                        DrawSurface.DrawString(pm.Label, Me.PointMarkFont, PointColorPen.Brush, pm.PlotPoint.X + pm.PlotRadiusInPixel, pm.PlotPoint.Y + pm.PlotRadiusInPixel)
                                    End If
                                End If
                            End If

                            ' Dispose the pen
                            PointColorPen.Dispose()
                            PointColorPen = Nothing

                        Next

                    Catch ex As Exception
                        Debug.WriteLine("ScanImagePlot->PointMarkPlot: error: " & ex.Message)
                End Try

            End If

        End If
        ' END Point-Mark
        '##########################################################################################
        '##########################################################################################

        '##########################################################################################
        '##########################################################################################
        ' Apply TextObjects
        If Me.ListOfTextObjects IsNot Nothing AndAlso Me.ListOfTextObjects.Count > 0 Then

            For Each Text As TextObject In Me.ListOfTextObjects

                ' Get the absolute position, if it was given relative:
                Dim PlotPosition As PointF
                If Text.ValueIsAbsolute Then
                    PlotPosition = Text.PlotPoint
                Else
                    PlotPosition = New PointF(Me.oFastImage.Width * Text.PlotPoint.X, Me.oFastImage.Height * Text.PlotPoint.Y)
                End If

                ' Get the color to use.
                If Text.ChooseColorAutomatic Then
                    ' Lock the image for reading out the pixel-colors.
                    Me.oFastImage.Lock()

                    If (PlotPosition.X >= 0 And CInt(PlotPosition.X) < Me.oFastImage.Width) And
                       (PlotPosition.Y >= 0 And CInt(PlotPosition.Y) < Me.oFastImage.Height) Then

                        Text.Color = cColorHelper.InvertColor(Me.oFastImage.GetPixel(CInt(PlotPosition.X), CInt(PlotPosition.Y)))

                    End If

                    ' Unlock the image for reading out the pixel-colors.
                    Me.oFastImage.Unlock(False)
                End If

                ' Get the font-size to use.
                If Text.UseRelativeFontSize > 0 Then
                    Text.Font = New Font(Text.Font.FontFamily,
                                         Convert.ToSingle(Math.Min(Me.oFastImage.Width, Me.oFastImage.Height) * Text.UseRelativeFontSize / 100),
                                         FontStyle.Regular)
                End If

                Using P As New Pen(Text.Color)
                    DrawSurface.DrawString(Text.Text, Text.Font, P.Brush, PlotPosition)
                End Using

            Next

        End If
        ' END TextObjects
        '##########################################################################################
        '##########################################################################################

        ' Dispose ressources!
        DrawSurface.Dispose()

        '##########################################################################################
        '##########################################################################################
        ' Draw scale bar to the image

        If Me.PlotScaleBar Then
            MyBase.CreateHorizontalScaleBar(Me.oFastImage, ScanImage.ScanRange_X)
        End If

        ' End draw scale bar.
        '##########################################################################################
        '##########################################################################################

        ' Store the scan-channel as a reference.
        Me.ScanChannelPlotted = ScanChannel

        Return Me.oFastImage.Bitmap
    End Function
#End Region

#Region "PointMark Functions"
    ''' <summary>
    ''' Adds a Point-Mark to the Display-List
    ''' </summary>
    Public Sub AddPointMark(X As Double, Y As Double, Z As Double)
        Me.ListOfPointMarks.Add(New PointMark(X, Y, Z))
    End Sub

    ''' <summary>
    ''' Adds a Point-Mark to the Display-List
    ''' </summary>
    Public Sub AddPointMark(PointMark As PointMark)
        If Not Me.ListOfPointMarks.Contains(PointMark) Then
            Me.ListOfPointMarks.Add(PointMark)
        End If
    End Sub

    ''' <summary>
    ''' Adds a Point-Mark to the Display-List
    ''' </summary>
    Public Sub AddPointMarks(PointMarks As List(Of PointMark))
        For Each PM As PointMark In PointMarks
            Me.AddPointMark(PM)
        Next
    End Sub

    ''' <summary>
    ''' Removes all entries from the Point-Mark-List
    ''' </summary>
    Public Sub ClearPointMarkList()
        Me.ListOfPointMarks.Clear()
    End Sub
#End Region

#Region "Data Point extraction with the plotted scale."

    ''' <summary>
    ''' Returns the location-coordinate of a pixel-coordinate in the scan-image,
    ''' by considering the rotation of the image.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' </summary>
    Public Function GetLocationOfScanData(x As Integer, y As Integer) As cNumericalMethods.Point2D
        If Me.ScanImage Is Nothing Then Return New cNumericalMethods.Point2D(-1, -1)
        Dim XInScanImage As Integer = CInt(x * (Me.ScanImage.ScanPixels_X / Me.PixelsIn_X_ToKeep))
        Dim YInScanImage As Integer = CInt(y * (Me.ScanImage.ScanPixels_Y / Me.PixelsIn_Y_ToKeep))
        Return Me.ScanImage.GetLocationOfScanData(XInScanImage, YInScanImage)
    End Function

    ''' <summary>
    ''' Returns the location-coordinate of a pixel-coordinate in the scan-image,
    ''' by considering the rotation of the image.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' </summary>
    Public Function GetLocationOfScanData(PixelCoordinate As Point) As cNumericalMethods.Point2D
        Return Me.GetLocationOfScanData(PixelCoordinate.X, PixelCoordinate.Y)
    End Function

    ''' <summary>
    ''' Returns the pixel-coordinate of a location-coordinate in the plotted and rescaled picture.
    ''' 
    ''' If Coordinate lies not in the ScanFrame it returns (-1,-1)
    ''' </summary>
    Public Function GetCoordinateInScanFrame(x As Double, y As Double) As Point
        Dim PointInScanMatrix As Point = Me.ScanImage.GetCoordinateInValueMatrix(x, y)
        Dim XInPlottedImage As Integer = CInt(x * Me._RangePerPixelUsed / Me.ScanImage.ScanRange_X)
        Dim YInPlottedImage As Integer = CInt(x * Me._RangePerPixelUsed / Me.ScanImage.ScanRange_Y)
        Return New Point(XInPlottedImage, YInPlottedImage)
    End Function

    ''' <summary>
    ''' Returns the pixel-coordinate of a location-coordinate in the plotted and rescaled picture.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' 
    ''' If Coordinate lies not in the ScanFrame it returns (-1,-1)
    ''' </summary>
    Public Function GetCoordinateInScanFrame(LocationCoordinate As cNumericalMethods.Point2D) As Point
        Return Me.GetCoordinateInScanFrame(LocationCoordinate.x, LocationCoordinate.y)
    End Function

#End Region

End Class
