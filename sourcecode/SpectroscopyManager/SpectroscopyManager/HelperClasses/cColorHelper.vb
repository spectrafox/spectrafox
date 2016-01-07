Public Class cColorHelper

    ''' <summary>
    ''' Returns a clear visible forecolor (black or white) for a given backcolor to achieve good contrast.
    ''' </summary>
    Public Shared Function ContrastColor(ByRef BaseColor As Color) As Color
        ' http://www.chaho.de/templates/show_article.php?LanguageID=1&ID=54&GetID=2
        If ColorIsBright(BaseColor) Then
            Return Color.Black
        Else
            Return Color.White
        End If
    End Function

    ''' <summary>
    ''' Returns if a color is dark or bright
    ''' </summary>
    Public Shared Function ColorIsBright(ByRef BaseColor As Color, Optional ByVal BrightnessLevel As Integer = 1024) As Boolean
        ' http://www.chaho.de/templates/show_article.php?LanguageID=1&ID=54&GetID=2
        If BaseColor.R * 2 + BaseColor.G * 5 + BaseColor.B > BrightnessLevel Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Returns a clear visible forecolor for a given backcolor to achieve good contrast.
    ''' </summary>
    Public Shared Function ColorIsDark(ByRef BaseColor As Color) As Boolean
        Return Not ColorIsBright(BaseColor)
    End Function

    ''' <summary>
    ''' Inverts a given color
    ''' </summary>
    Public Shared Function InvertColor(ByVal C As Color) As Color
        Return Color.FromArgb(255 - C.R, 255 - C.G, 255 - C.B)
    End Function

End Class
