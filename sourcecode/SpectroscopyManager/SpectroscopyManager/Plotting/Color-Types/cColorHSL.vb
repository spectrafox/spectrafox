Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Diagnostics

''' <summary>
''' Represents an AHSL color.
''' http://www.codeproject.com/Articles/17715/Plot-D-surfaces
''' </summary>
<Serializable(), StructLayout(LayoutKind.Sequential), DebuggerDisplay("\{AHSL = ({A}, {H}, {S}, {L})\}")> _
Public Structure cColorHSL
    Implements IEquatable(Of Color)

    ''' <summary>Initializes a new instance of the <see cref="T:LukeSw.Drawing.ColorHsl"/> structure
    ''' from the specified double values.</summary>
    ''' <param name="alpha">The alpha component value. Valid values are 0 through 1.</param>
    ''' <param name="hue">The hue component value. Valid values are 0 through 360.</param>
    ''' <param name="saturation">The saturation component value. Valid values are 0 through 1.</param>
    ''' <param name="lightness">The lightness component value. Valid values are 0 through 1.</param>
    Public Sub New(alpha As Double, hue As Double, saturation As Double, lightness As Double)
        cColorRGB.Checkdouble(alpha, "alpha")
        cColorRGB.Checkdouble(hue, "hue", 0.0, 360.0)
        If hue = 360.0 Then
            hue = 0.0
        End If
        cColorRGB.Checkdouble(saturation, "saturation")
        cColorRGB.Checkdouble(lightness, "lightness")
        Me.A = alpha
        Me.H = hue
        Me.S = saturation
        Me.L = lightness
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="T:LukeSw.Drawing.ColorHsl"/> structure
    ''' from the specified double values. The alpha value is implicitly 1 (fully opaque).</summary>
    ''' <param name="hue">The hue component value. Valid values are 0 through 360.</param>
    ''' <param name="saturation">The saturation component value. Valid values are 0 through 1.</param>
    ''' <param name="lightness">The lightness component value. Valid values are 0 through 1.</param>
    Public Sub New(hue As Double, saturation As Double, lightness As Double)
        Me.New(1.0, hue, saturation, lightness)
    End Sub

    ''' <summary>Gets the alpha component value.</summary>
    Public ReadOnly A As Double
    ''' <summary>Gets the hue component value.</summary>
    Public ReadOnly H As Double
    ''' <summary>Gets the saturation component value.</summary>
    Public ReadOnly S As Double
    ''' <summary>Gets the lightness component value.</summary>
    Public ReadOnly L As Double

    ''' <summary>Converts this <see cref="T:LukeSw.Drawing.ColorHsl" /> structure to a human-readable string.</summary>
    ''' <returns>String that consists of the AHSL component names and their values.</returns>
    Public Overrides Function ToString() As String
        Dim builder As New StringBuilder(&H20)
        builder.Append([GetType]().Name)
        builder.Append(" [")
        builder.Append("A=")
        builder.Append(Me.A)
        builder.Append(", H=")
        builder.Append(Me.H)
        builder.Append(", S=")
        builder.Append(Me.S)
        builder.Append(", L=")
        builder.Append(Me.L)
        builder.Append("]")
        Return builder.ToString()
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return ToColorRgb().Equals(obj)
    End Function

    Public Overloads Function Equals(other As Color) As Boolean
        Return ToColorRgb() = cColorRGB.FromColor(other)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return A.GetHashCode() Xor H.GetHashCode() Xor S.GetHashCode() Xor L.GetHashCode()
    End Function
    Public Function ToColor() As Color
        Return cColorRGB.FromColor(Me).ToColor()
    End Function

    Public Function ToColorRgb() As cColorRGB
        Return cColorRGB.FromColor(Me)
    End Function

    Public Shared Function FromColor(color As Color) As cColorHSL
        Return cColorRGB.FromColor(color).ToColorHsl()
    End Function

    Public Function Equals1(other As System.Drawing.Color) As Boolean Implements System.IEquatable(Of System.Drawing.Color).Equals
        Return True
    End Function
End Structure
