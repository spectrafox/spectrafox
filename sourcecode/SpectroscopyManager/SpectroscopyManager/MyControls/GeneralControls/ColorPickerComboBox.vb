Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Reflection

''' <summary>
''' Inherits from existing ComboBox
''' </summary>
Public Class ColorPickerComboBox
    Inherits ComboBox

    ''' <summary>
    ''' All colors available in the combobox.
    ''' </summary>
    Public ColorsAvailable As New List(Of Color)

    Private _ShowBrightColors As Boolean = True
    ''' <summary>
    ''' sets if the combobox should only show bright colors
    ''' </summary>
    Public Property ShowBrightColors As Boolean
        Get
            Return Me._ShowBrightColors
        End Get
        Set(value As Boolean)
            Me._ShowBrightColors = value
            Me.FillColors()
        End Set
    End Property

    Public Sub New()
        FillColors()

        ' Change DrawMode for custom drawing
        Me.DrawMode = DrawMode.OwnerDrawFixed
        Me.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub FillColors()
        Me.Items.Clear()
        Me.ColorsAvailable.Clear()

        ' Fill Colors using Reflection
        For Each _c As Color In GetType(Color).GetProperties(BindingFlags.[Static] Or BindingFlags.[Public]).Where(Function(c) c.PropertyType = GetType(Color)).[Select](Function(c) CType(c.GetValue(c, Nothing), Color))
            If Not ShowBrightColors Then If cColorHelper.ColorIsBright(_c, 1800) Then Continue For
            Me.Items.Add(_c)
            Me.ColorsAvailable.Add(_c)
        Next
    End Sub

    ''' <summary>
    ''' Override Draw Method
    ''' </summary>
    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If e.Index >= 0 Then
            Dim _Color As Color = CType(Me.Items(e.Index), Color)
            Dim ColorBrush As New SolidBrush(_Color)
            Dim ForeColorBrushBright As New SolidBrush(Drawing.Color.Black)
            Dim ForeColorBrushDark As New SolidBrush(Drawing.Color.White)

            Dim nextX As Integer = 0

            e.Graphics.FillRectangle(ColorBrush, e.Bounds)
            'DrawColor(e, _Color, ColorBrush, ForeColorBrush, nextX)
            If cColorHelper.ColorIsBright(_Color) Then
                DrawText(e, _Color, ColorBrush, ForeColorBrushBright, nextX)
            Else
                DrawText(e, _Color, ColorBrush, ForeColorBrushDark, nextX)
            End If

            ColorBrush.Dispose()
            ForeColorBrushBright.Dispose()
            ForeColorBrushDark.Dispose()
        Else
            MyBase.OnDrawItem(e)
        End If
    End Sub

    ''' <summary>
    ''' Draw the Color rectangle filled with item color
    ''' </summary>
    Private Sub DrawColor(ByRef e As DrawItemEventArgs,
                          ByRef _Color As Color,
                          ByRef ColorBrush As SolidBrush,
                          ByRef ForeColorBrush As SolidBrush,
                          ByRef nextX As Integer)
        Dim width As Integer = e.Bounds.Height * 2 - 8
        Dim rectangle As New Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, e.Bounds.Height - 6)
        e.Graphics.FillRectangle(ColorBrush, rectangle)

        nextX = width + 8
    End Sub

    ''' <summary>
    ''' Draw the color name next to the color rectangle
    ''' </summary>
    Private Sub DrawText(ByRef e As DrawItemEventArgs,
                         ByRef _Color As Color,
                         ByRef ColorBrush As SolidBrush,
                         ByRef ForeColorBrush As SolidBrush,
                         ByRef nextX As Integer)
        e.Graphics.DrawString(_Color.Name, e.Font, ForeColorBrush, New PointF(nextX, e.Bounds.Y + (e.Bounds.Height - e.Font.Height) \ 2))
    End Sub

    ''' <summary>
    ''' Gets/sets the selected color of ComboBox
    ''' (Default color is Black)
    ''' </summary>
    Public Property Color() As Color
        Get
            If Me.SelectedItem IsNot Nothing Then
                Return CType(Me.SelectedItem, Color)
            End If

            Return Color.Black
        End Get
        Set(value As Color)
            Dim ix As Integer = Me.Items.IndexOf(value)
            If ix >= 0 Then
                Me.SelectedIndex = ix
            End If
        End Set
    End Property
End Class
