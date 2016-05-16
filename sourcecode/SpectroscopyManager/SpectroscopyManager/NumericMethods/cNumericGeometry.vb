Partial Public Class cNumericalMethods

#Region "Double point definition"

    ''' <summary>
    ''' Represents a 3D-point.
    ''' </summary>
    <DebuggerDisplay("{x}, {y}, {z}")>
    Public Structure Point3D
        Public Sub New(X As Double, Y As Double, Z As Double)
            Me.x = X
            Me.y = Y
            Me.z = Z
        End Sub
        Public x As Double
        Public y As Double
        Public z As Double

        Public Overrides Function ToString() As String
            Return x.ToString & ", " & y.ToString & ", " & z.ToString
        End Function

    End Structure

    ''' <summary>
    ''' Represents a 2D-point.
    ''' </summary>
    <DebuggerDisplay("{x}, {y}")>
    Public Structure Point2D
        Public Sub New(X As Double, Y As Double)
            Me.x = X
            Me.y = Y
        End Sub
        Public x As Double
        Public y As Double

        Public Overrides Function ToString() As String
            Return x.ToString & ", " & y.ToString
        End Function

    End Structure

#End Region

#Region "Point handling"

    ''' <summary>
    ''' Takes two points and orders them so that the first point has (XMin, YMin), and the second (XMax, YMax)
    ''' </summary>
    Public Shared Function GetRectangleCompatiblePoints(ByVal P1 As Point, ByVal P2 As Point) As Tuple(Of Point, Point)
        Dim XMax As Integer = Math.Max(P1.X, P2.X)
        Dim YMax As Integer = Math.Max(P1.Y, P2.Y)
        Dim XMin As Integer = Math.Min(P1.X, P2.X)
        Dim YMin As Integer = Math.Min(P1.Y, P2.Y)

        Return New Tuple(Of Point, Point)(New Point(XMin, YMin), New Point(XMax, YMax))
    End Function

    ''' <summary>
    ''' Takes two points and orders them so that the first point has (XMin, YMin), and the second (XMax, YMax)
    ''' </summary>
    Public Shared Function GetRectangleCompatiblePoints(ByVal P1 As Point2D, ByVal P2 As Point2D) As Tuple(Of Point2D, Point2D)
        Dim XMax As Double = Math.Max(P1.x, P2.x)
        Dim YMax As Double = Math.Max(P1.y, P2.y)
        Dim XMin As Double = Math.Min(P1.x, P2.x)
        Dim YMin As Double = Math.Min(P1.y, P2.y)

        Return New Tuple(Of Point2D, Point2D)(New Point2D(XMin, YMin), New Point2D(XMax, YMax))
    End Function

#End Region

#Region "Coordinate Transformation for Scan Images"

    ''' <summary>
    ''' Converts Degrees to Radian
    ''' </summary>
    Public Shared Function ConvertDegreesToRadians(degrees As Double) As Double
        Dim radians As Double = (Math.PI / 180) * degrees
        Return (radians)
    End Function

    ''' <summary>
    ''' Returns coordinates transformed to the new reference coordinate system.
    ''' </summary>
    Public Shared Function CoordinateTransform(xOld As Double, yOld As Double,
                                               XOldOffset As Double, YOldOffset As Double,
                                               OldRotationAngleInDegree As Double) As Point2D

        ' Convert angle to Radian.
        Dim RotationAngleInRadian As Double = MathNet.Numerics.Trig.DegreeToRadian(OldRotationAngleInDegree)
        Dim CosRotation As Double = MathNet.Numerics.Trig.Cos(RotationAngleInRadian)
        Dim SinRotation As Double = MathNet.Numerics.Trig.Sin(RotationAngleInRadian)

        ' Local location-coordinates in the scan-matrix.
        Dim XLocal_LocationCoordinate As Double = xOld
        Dim YLocal_LocationCoordinate As Double = yOld

        ' Real location-coordinates in the rotated scan-frame WITHOUT OFFSET
        Dim XReal_LocationCoordinate As Double = XLocal_LocationCoordinate * CosRotation + YLocal_LocationCoordinate * SinRotation
        Dim YReal_LocationCoordinate As Double = YLocal_LocationCoordinate * CosRotation - XLocal_LocationCoordinate * SinRotation

        ' Real location-coordinates in the rotated scan-frame WITH OFFSET
        XReal_LocationCoordinate = XOldOffset + XReal_LocationCoordinate
        YReal_LocationCoordinate = YOldOffset - YReal_LocationCoordinate

        Return New Point2D(XReal_LocationCoordinate, YReal_LocationCoordinate)
    End Function

    ''' <summary>
    ''' Returns coordinates transformed from the new reference coordinate system back to the original one.
    ''' </summary>
    Public Shared Function BackCoordinateTransform(xNew As Double, yNew As Double,
                                                   XOldOffset As Double, YOldOffset As Double,
                                                   OldRotationAngleInDegree As Double) As Point2D
        ' Convert angle to Radian.
        Dim RotationAngleInRadian As Double = MathNet.Numerics.Trig.DegreeToRadian(OldRotationAngleInDegree)
        Dim CosRotation As Double = MathNet.Numerics.Trig.Cos(RotationAngleInRadian)
        Dim SinRotation As Double = MathNet.Numerics.Trig.Sin(RotationAngleInRadian)

        ' Real location-coordinates in the rotated scan-frame WITHOUT OFFSET
        Dim XReal_LocationCoordinate As Double = xNew - XOldOffset
        Dim YReal_LocationCoordinate As Double = YOldOffset - yNew

        ' Local location-coordinates in the scan-matrix.
        Dim XLocal_LocationCoordinate As Double = XReal_LocationCoordinate * CosRotation - YReal_LocationCoordinate * SinRotation
        Dim YLocal_LocationCoordinate As Double = YReal_LocationCoordinate * CosRotation + XReal_LocationCoordinate * SinRotation

        ' Local pixel-coordinates in the scan-matrix.
        Dim XLocal_PixelCoordinate As Double = XLocal_LocationCoordinate
        Dim YLocal_PixelCoordinate As Double = YLocal_LocationCoordinate

        Return New Point2D(XLocal_PixelCoordinate, YLocal_PixelCoordinate)
    End Function

#End Region

#Region "General 2D Coordinate transformations"

    ''' <summary>
    ''' Performs a clockwise rotation
    ''' </summary>
    Public Shared Function RotateClockwise(X As Double, Y As Double, AngleInDeg As Double) As Point2D

        ' Convert angle to Radian.
        Dim RotationAngleInRadian As Double = MathNet.Numerics.Trig.DegreeToRadian(AngleInDeg)
        Dim CosRotation As Double = MathNet.Numerics.Trig.Cos(RotationAngleInRadian)
        Dim SinRotation As Double = MathNet.Numerics.Trig.Sin(RotationAngleInRadian)

        ' Local location-coordinates in the scan-matrix.
        Dim XRotated As Double = X * CosRotation - Y * SinRotation
        Dim YRotated As Double = -X * SinRotation - Y * CosRotation

        Return New Point2D(XRotated, YRotated)
    End Function

    ''' <summary>
    ''' Performs a clockwise rotation
    ''' </summary>
    Public Shared Function RotateCounterClockwise(X As Double, Y As Double, AngleInDeg As Double) As Point2D

        ' Convert angle to Radian.
        Dim RotationAngleInRadian As Double = MathNet.Numerics.Trig.DegreeToRadian(AngleInDeg)
        Dim CosRotation As Double = MathNet.Numerics.Trig.Cos(RotationAngleInRadian)
        Dim SinRotation As Double = MathNet.Numerics.Trig.Sin(RotationAngleInRadian)

        ' Local location-coordinates in the scan-matrix.
        Dim XRotated As Double = X * CosRotation + Y * SinRotation
        Dim YRotated As Double = X * SinRotation - Y * CosRotation

        Return New Point2D(XRotated, YRotated)
    End Function

#End Region

End Class
