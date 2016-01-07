''' <summary>
''' Class that speeds up drawing of bitmaps that get manipulated pixel by pixel using SetPixel.
''' GDI is slow, since it locks and unlocks the whole bitmap before setting the value of each pixel.
''' </summary>
Public Class cFastImage

#Region "Properties"
    ''' <summary>
    ''' Values stored as RGB
    ''' </summary>
    Private RGBValues() As Byte

    ''' <summary>
    ''' Details of the Bitmap.
    ''' </summary>
    Private BitmapData As Imaging.BitmapData

    ''' <summary>
    ''' Pointer to the Bitmap in the memory (Scan0)
    ''' </summary>
    Private BitmapPointer As IntPtr
    Private BitmapLocked As Boolean = False

    Private _BaseBitmap As Bitmap
    Private _IsAlphaImage As Boolean = False
    Private _ImageWidth As Integer
    Private _ImageHeight As Integer

    ''' <summary>
    ''' Returns the width of the bitmap.
    ''' </summary>
    Public ReadOnly Property Width() As Integer
        Get
            Return Me._ImageWidth
        End Get
    End Property

    ''' <summary>
    ''' Returns the height of the bitmap.
    ''' </summary>
    Public ReadOnly Property Height() As Integer
        Get
            Return Me._ImageHeight
        End Get
    End Property

    ''' <summary>
    ''' Returns, if the bitmap uses alpha blending.
    ''' </summary>
    Public ReadOnly Property IsAlphaBitmap() As Boolean
        Get
            Return Me._IsAlphaImage
        End Get
    End Property

    ''' <summary>
    ''' Returns the bitmap object.
    ''' </summary>
    Public ReadOnly Property Bitmap() As Bitmap
        Get
            Return Me._BaseBitmap
        End Get
    End Property
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor to create a new FastImage-Object from an existing bitmap.
    ''' </summary>
    Public Sub New(ByVal bitmap As Bitmap)
        If (bitmap.PixelFormat = (bitmap.PixelFormat Or Imaging.PixelFormat.Indexed)) Then
            Throw New Exception("Cannot lock an Indexed image.")
            Return
        End If
        Me._BaseBitmap = bitmap
        Me._IsAlphaImage = (Me.Bitmap.PixelFormat = (Me.Bitmap.PixelFormat Or Imaging.PixelFormat.Alpha))
        Me._ImageWidth = bitmap.Width
        Me._ImageHeight = bitmap.Height
    End Sub
#End Region

#Region "Locking / Unlocking"
    ''' <summary>
    ''' Locks the processing of an image to set the pixel in the background-buffer.
    ''' </summary>
    Public Sub Lock()
        If Me.BitmapLocked Then
            Throw New Exception("Bitmap already locked.")
            Return
        End If

        Dim rect As New Rectangle(0, 0, Me.Width, Me.Height)
        Me.BitmapData = Me.Bitmap.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, Me.Bitmap.PixelFormat)
        Me.BitmapPointer = Me.BitmapData.Scan0

        If Me.IsAlphaBitmap Then
            Dim bytes As Integer = (Me.Width * Me.Height) * 4
            ReDim Me.RGBValues(bytes - 1)
            System.Runtime.InteropServices.Marshal.Copy(Me.BitmapPointer, RGBValues, 0, Me.RGBValues.Length)
        Else
            Dim bytes As Integer = (Me.Width * Me.Height) * 3
            ReDim Me.RGBValues(bytes - 1)
            System.Runtime.InteropServices.Marshal.Copy(Me.BitmapPointer, RGBValues, 0, Me.RGBValues.Length)
        End If

        Me.BitmapLocked = True
    End Sub

    ''' <summary>
    ''' Unlocks the Bitmap and draws the pixel that were manipulated.
    ''' </summary>
    ''' <param name="setPixels">If true, writes the manipulated pixels to the bitmap.</param>
    Public Sub Unlock(ByVal setPixels As Boolean)
        If Not Me.BitmapLocked Then
            Throw New Exception("Bitmap not locked.")
            Return
        End If
        ' Copy the RGB values back to the bitmap
        If setPixels Then System.Runtime.InteropServices.Marshal.Copy(Me.RGBValues, 0, Me.BitmapPointer, Me.RGBValues.Length)
        ' Unlock the bits.
        Me.Bitmap.UnlockBits(BitmapData)
        Me.BitmapLocked = False
    End Sub
#End Region

#Region "Clear Image"
    ''' <summary>
    ''' Clears the whole image by filling each pixel with a certain color.
    ''' </summary>
    Public Sub Clear(ByVal colour As Color)
        If Not Me.BitmapLocked Then
            Throw New Exception("Bitmap not locked.")
            Return
        End If

        If Me.IsAlphaBitmap Then
            For index As Integer = 0 To Me.RGBValues.Length - 1 Step 4
                Me.RGBValues(index) = colour.B
                Me.RGBValues(index + 1) = colour.G
                Me.RGBValues(index + 2) = colour.R
                Me.RGBValues(index + 3) = colour.A
            Next index
        Else
            For index As Integer = 0 To Me.RGBValues.Length - 1 Step 3
                Me.RGBValues(index) = colour.B
                Me.RGBValues(index + 1) = colour.G
                Me.RGBValues(index + 2) = colour.R
            Next index
        End If
    End Sub
#End Region

#Region "Set Pixel"
    ''' <summary>
    ''' Manipulates a pixel value.
    ''' </summary>
    Public Sub SetPixel(ByVal location As Point, ByVal colour As Color)
        Me.SetPixel(location.X, location.Y, colour)
    End Sub

    ''' <summary>
    ''' Manipulates a pixel value.
    ''' </summary>
    Public Sub SetPixel(ByVal x As Integer, ByVal y As Integer, ByVal colour As Color)
        If Not Me.BitmapLocked Then
            Throw New Exception("Bitmap not locked.")
            Return
        End If

        If Me.IsAlphaBitmap Then
            Dim index As Integer = ((y * Me.Width + x) * 4)
            Me.RGBValues(index) = colour.B
            Me.RGBValues(index + 1) = colour.G
            Me.RGBValues(index + 2) = colour.R
            Me.RGBValues(index + 3) = colour.A
        Else
            Dim index As Integer = ((y * Me.Width + x) * 3)
            Me.RGBValues(index) = colour.B
            Me.RGBValues(index + 1) = colour.G
            Me.RGBValues(index + 2) = colour.R
        End If
    End Sub
#End Region

#Region "Get Pixel"
    ''' <summary>
    ''' Returns the value of a pixel.
    ''' </summary>
    Public Function GetPixel(ByVal location As Point) As Color
        Return Me.GetPixel(location.X, location.Y)
    End Function

    ''' <summary>
    ''' Returns the value of a pixel.
    ''' </summary>
    Public Function GetPixel(ByVal location As PointF) As Color
        Return Me.GetPixel(CInt(location.X), CInt(location.Y))
    End Function

    ''' <summary>
    ''' Returns the value of a pixel.
    ''' </summary>
    Public Function GetPixel(ByVal x As Integer, ByVal y As Integer) As Color
        If Not Me.BitmapLocked Then
            Throw New Exception("Bitmap not locked.")
            Return Nothing
        End If

        If Me.IsAlphaBitmap Then
            Dim index As Integer = ((y * Me.Width + x) * 4)
            Dim b As Integer = Me.RGBValues(index)
            Dim g As Integer = Me.RGBValues(index + 1)
            Dim r As Integer = Me.RGBValues(index + 2)
            Dim a As Integer = Me.RGBValues(index + 3)
            Return Color.FromArgb(a, r, g, b)
        Else
            Dim index As Integer = ((y * Me.Width + x) * 3)
            Dim b As Integer = Me.RGBValues(index)
            Dim g As Integer = Me.RGBValues(index + 1)
            Dim r As Integer = Me.RGBValues(index + 2)
            Return Color.FromArgb(r, g, b)
        End If
    End Function
#End Region
End Class
