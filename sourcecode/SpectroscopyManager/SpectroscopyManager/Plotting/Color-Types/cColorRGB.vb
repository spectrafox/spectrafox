Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.Diagnostics
Imports System.Runtime.InteropServices

''' <summary>
''' Represents an ARGB color.
''' http://www.codeproject.com/Articles/17715/Plot-D-surfaces
''' </summary>
<Serializable(), StructLayout(LayoutKind.Sequential), DebuggerDisplay("\{ARGB = ({A}, {R}, {G}, {B})\}")> _
Public Structure cColorRGB
    Implements IEquatable(Of Color)

    <DebuggerStepThrough()> _
    Friend Shared Sub Checkdouble(value As Double, name As String)
        Checkdouble(value, name, 0.0, 1.0)
    End Sub

    <DebuggerStepThrough()> _
    Friend Shared Sub Checkdouble(value As Double, name As String, min As Double, max As Double)
        If (value < min) OrElse (value > max) Then
            Throw New ArgumentException(String.Format("{0},{1},{2},{3}", name, value, min, max))
        End If
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="T:LukeSw.Drawing.ColorRgb"/> structure
    ''' from the specified double values.</summary>
    ''' <param name="alpha">The alpha component value. Valid values are 0 through 1.</param>
    ''' <param name="red">The red component value. Valid values are 0 through 1.</param>
    ''' <param name="green">The green component value. Valid values are 0 through 1.</param>
    ''' <param name="blue">The blue component value. Valid values are 0 through 1.</param>
    Public Sub New(alpha As Double, red As Double, green As Double, blue As Double)
        Checkdouble(alpha, "alpha")
        Checkdouble(red, "red")
        Checkdouble(green, "green")
        Checkdouble(blue, "blue")
        Me.A = alpha
        Me.R = red
        Me.G = green
        Me.B = blue
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="T:LukeSw.Drawing.ColorRgb"/> structure
    ''' from the specified double values. The alpha value is implicitly 1 (fully opaque).</summary>
    ''' <param name="red">The red component value. Valid values are 0 through 1.</param>
    ''' <param name="green">The green component value. Valid values are 0 through 1.</param>
    ''' <param name="blue">The blue component value. Valid values are 0 through 1.</param>
    Public Sub New(red As Double, green As Double, blue As Double)
        Checkdouble(red, "red")
        Checkdouble(green, "green")
        Checkdouble(blue, "blue")
        Me.A = 1.0
        Me.R = red
        Me.G = green
        Me.B = blue
    End Sub

    ''' <summary>Gets the alpha component value.</summary>
    Public ReadOnly A As Double
    ''' <summary>Gets the red component value.</summary>
    Public ReadOnly R As Double
    ''' <summary>Gets the green component value.</summary>
    Public ReadOnly G As Double
    ''' <summary>Gets the blue component value.</summary>
    Public ReadOnly B As Double

    ''' <summary>Converts this <see cref="T:LukeSw.Drawing.ColorRgb" /> structure to a human-readable string.</summary>
    ''' <returns>String that consists of the ARGB component names and their values.</returns>
    Public Overrides Function ToString() As String
        Dim builder As New StringBuilder(&H20)
        builder.Append([GetType]().Name)
        builder.Append(" [")
        builder.Append("A=")
        builder.Append(Me.A)
        builder.Append(", R=")
        builder.Append(Me.R)
        builder.Append(", G=")
        builder.Append(Me.G)
        builder.Append(", B=")
        builder.Append(Me.B)
        builder.Append("]")
        Return builder.ToString()
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return (TypeOf obj Is Color AndAlso ToColor() = CType(obj, Color)) OrElse (TypeOf obj Is cColorRGB AndAlso ToColor() = CType(obj, cColorRGB))
    End Function

    Public Shared Operator =(c1 As cColorRGB, c2 As cColorRGB) As Boolean
        Return c1.A = c2.A AndAlso c1.R = c2.R AndAlso c1.G = c2.G AndAlso c1.B = c2.B
    End Operator

    Public Shared Operator <>(c1 As cColorRGB, c2 As cColorRGB) As Boolean
        Return Not (c1 = c2)
    End Operator

    Public Overloads Function Equals(other As Color) As Boolean
        Return ToColor() = other
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return A.GetHashCode() Xor R.GetHashCode() Xor G.GetHashCode() Xor B.GetHashCode()
    End Function

    Public Shared Function FromColor(color As Color) As cColorRGB
        Return New cColorRGB(color.A / 255.0, color.R / 255.0, color.G / 255.0, color.B / 255.0)
    End Function

    Public Shared Function FromColor(alpha As Double, baseColor As Color) As cColorRGB
        Return New cColorRGB(alpha, baseColor.R / 255.0, baseColor.G / 255.0, baseColor.B / 255.0)
    End Function

    Public Shared Widening Operator CType(color As Color) As cColorRGB
        Return cColorRGB.FromColor(color)
    End Operator

    ''' <summary>Returns a <see cref="T:System.Drawing.Color" /> reprezentation of current instance.</summary>
    ''' <returns>A <see cref="T:System.Drawing.Color" /> reprezentation of current instance</returns>
    Public Function ToColor() As Color
        Return Color.FromArgb(CInt(Math.Truncate(255.0 * A + 0.5)), CInt(Math.Truncate(255.0 * R + 0.5)), CInt(Math.Truncate(255.0 * G + 0.5)), CInt(Math.Truncate(255.0 * B + 0.5)))
    End Function

    Public Shared Narrowing Operator CType(rgb As cColorRGB) As Color
        Return rgb.ToColor()
    End Operator

    Private Shared Function HueToRgb(value1 As Double, value2 As Double, hue As Double) As Double
        If hue < 0.0 Then
            hue += 360.0
        End If
        If hue > 360.0 Then
            hue -= 360.0
        End If
        If hue < 60.0 Then
            Return value1 + (value2 - value1) * hue / 60.0
        End If
        If hue < 180.0 Then
            Return value2
        End If
        If hue < 240.0 Then
            Return value1 + (value2 - value1) * (240.0 - hue) / 60.0
        End If
        Return value1
    End Function

    Public Shared Function FromColor(hsl As cColorHSL) As cColorRGB
        If hsl.S = 0.0 Then
            Return New cColorRGB(hsl.A, hsl.L, hsl.L, hsl.L)
        End If
        Dim value2 As Double = hsl.L * hsl.S
        If hsl.L <= 0.5 Then
            value2 += hsl.L
        Else
            value2 = hsl.L + hsl.S - value2
        End If
        Dim value1 As Double = 2.0F * hsl.L - value2
        Return New cColorRGB(hsl.A, HueToRgb(value1, value2, hsl.H + 120.0), HueToRgb(value1, value2, hsl.H), HueToRgb(value1, value2, hsl.H - 120.0))
    End Function

    Public Shared Widening Operator CType(hsl As cColorHSL) As cColorRGB
        Return cColorRGB.FromColor(hsl)
    End Operator

    Private Function RgbToHue(max As Double, min As Double) As Double
        Dim del As Double = max - min
        If del = 0.0 Then
            Return 0.0
        End If
        If max = R AndAlso G >= B Then
            Return 60.0 * (G - B) / del
        End If
        If max = R Then
            Return 60.0 * (G - B) / del + 360.0
        End If
        If max = G Then
            Return 60.0 * (B - R) / del + 120.0
        End If
        If max = B Then
            Return 60.0 * (R - G) / del + 240.0
        End If
        Return 0.0
    End Function

    ''' <summary>Returns a <see cref="T:LukeSw.Drawing.ColorHsl" /> reprezentation of current instance.</summary>
    ''' <returns>A <see cref="T:LukeSw.Drawing.ColorHsl" /> reprezentation of current instance</returns>
    Public Function ToColorHsl() As cColorHSL
        Dim max As Double = Math.Max(R, Math.Max(G, B))
        Dim min As Double = Math.Min(R, Math.Min(G, B))
        Dim sum As Double = max + min
        Dim l As Double = sum / 2.0F
        If max = min Then
            Return New cColorHSL(A, 0.0, 0.0, l)
        End If
        Dim s As Double = 0.0
        If l <= 0.5 Then
            ' del / sum;
            s = 1.0 - 2.0F * min / sum
        ElseIf l > 0.5 Then
            ' del / (2f - sum);
            s = 1.0 - 2.0 * (1.0 - max) / (2.0 - sum)
        End If
        Return New cColorHSL(A, RgbToHue(max, min), s, l)
    End Function

    Public Shared Widening Operator CType(rgb As cColorRGB) As cColorHSL
        Return rgb.ToColorHsl()
    End Operator

    Public Function Equals1(other As System.Drawing.Color) As Boolean Implements System.IEquatable(Of System.Drawing.Color).Equals
        Return True
    End Function
End Structure
