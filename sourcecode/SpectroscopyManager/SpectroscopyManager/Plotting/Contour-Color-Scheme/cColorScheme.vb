Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Drawing
Imports System.Text
Imports System.Xml
Imports System.IO

''' <summary>
''' ColorScheme, as defined on CodePlex:
''' http://www.codeproject.com/Articles/17715/Plot-D-surfaces
''' 
''' With additional changes made by Michael Ruby.
''' </summary>
Public Class cColorScheme

    Private _SchemeArray As Color()
    Private _SchemeName As String = ""
    Const DefaultSchemeLength As UInteger = 255

#Region "Properties"
    ''' <summary>
    ''' Returns the Number of Colors present in the Scheme.
    ''' </summary>
    Public ReadOnly Property Length() As Integer
        Get
            Return _SchemeArray.Length
        End Get
    End Property

    ''' <summary>
    ''' Returns a specific Color at given Index from the Scheme
    ''' </summary>
    Default Public ReadOnly Property Item(index As Integer) As Color
        Get
            Return _SchemeArray(index)
        End Get
    End Property

    ''' <summary>
    ''' Returns the Name of the current Scheme.
    ''' </summary>
    Public ReadOnly Property SchemeName As String
        Get
            Return _SchemeName
        End Get
    End Property

    ''' <summary>
    ''' Returns an Array of Brushes created from the Color-Scheme.
    ''' </summary>
    Public ReadOnly Property BrushArray As SolidBrush()
        Get
            Dim Brushes As SolidBrush() = New SolidBrush(_SchemeArray.Length - 1) {}
            For i As Integer = 0 To Brushes.Length - 1
                Brushes(i) = New SolidBrush(_SchemeArray(i))
            Next
            Return Brushes
        End Get
    End Property

    ''' <summary>
    ''' Returns the name of the Scheme.
    ''' </summary>
    Public Overrides Function ToString() As String
        Return Me._SchemeName
    End Function

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Creates an empty scheme.
    ''' </summary>
    Public Sub New()
        Me.New(Color.Black, "default")
    End Sub

    ''' <summary>
    ''' Creates a new Scheme from a given Array of Colors.
    ''' </summary>
    Public Sub New(InputScheme As Color(), Optional ByVal SchemeName As String = "")
        Me._SchemeArray = InputScheme
        Me._SchemeName = SchemeName
    End Sub

    ''' <summary>
    ''' Creates a new Scheme from a given Array of Colors.
    ''' </summary>
    Public Sub New(InputScheme As List(Of Color), Optional ByVal SchemeName As String = "")
        Me._SchemeArray = InputScheme.ToArray
        Me._SchemeName = SchemeName
    End Sub

    ''' <summary>
    ''' Creates a new Scheme from a given Array of 3D-Bytes.
    ''' </summary>
    Protected Sub New(InputScheme As Byte(,), Optional ByVal SchemeName As String = "")
        Me._SchemeArray = New Color(InputScheme.GetLength(0) - 1) {}
        For i As Integer = 0 To Me._SchemeArray.Length - 1
            Me._SchemeArray(i) = Color.FromArgb(InputScheme(i, 0), InputScheme(i, 1), InputScheme(i, 2))
        Next
        Me._SchemeName = SchemeName
    End Sub

    ''' <summary>
    ''' Creates a new Scheme from a given Base-Color
    ''' </summary>
    Public Sub New(BaseColor As Color, Optional ByVal SchemeName As String = "")
        GenerateColorBasedScheme(BaseColor)
        Me._SchemeName = SchemeName
    End Sub

    ''' <summary>
    ''' Creates a new Scheme from a given Base-Hue (Tönung)
    ''' </summary>
    Public Sub New(BaseHue As Double, Optional ByVal SchemeName As String = "")
        GenerateHueBasedScheme(BaseHue)
        Me._SchemeName = SchemeName
    End Sub
#End Region

#Region "Color-Scheme Generation Functions"
    ''' <summary>
    ''' Wrapper for the Hue-Based Scheme creation,
    ''' to add the Default-Scheme length to the constructor.
    ''' </summary>
    Private Sub GenerateHueBasedScheme(InputHue As Double)
        GenerateHueBasedScheme(InputHue, DefaultSchemeLength)
    End Sub

    ''' <summary>
    ''' Function generating the Scheme from the given Hue
    ''' as Scheme of given Array-Length.
    ''' </summary>
    Private Sub GenerateHueBasedScheme(InputHue As Double, SchemeLength As UInteger)
        InputHue = InputHue Mod 360.0
        _SchemeArray = New Color(CInt(SchemeLength - 1)) {}
        For i As Integer = 0 To _SchemeArray.Length - 1
            _SchemeArray(i) = (New cColorHSL(InputHue, 1.0, i / (_SchemeArray.Length - 1.0))).ToColor()
        Next
    End Sub

    ''' <summary>
    ''' Wrapper for the Color-Based Scheme creation,
    ''' to add the Default-Scheme length to the constructor.
    ''' </summary>
    Private Sub GenerateColorBasedScheme(InputColor As Color)
        GenerateColorBasedScheme(InputColor, DefaultSchemeLength)
    End Sub

    ''' <summary>
    ''' Function generating the Schee from the given Color
    ''' by converting the Color to a Hue and then calling the Hue
    ''' based Scheme creation.
    ''' </summary>
    Private Sub GenerateColorBasedScheme(InputColor As Color, SchemeLength As UInteger)
        Dim InputColorHue As Double = InputColor.GetHue()
        GenerateHueBasedScheme(InputColorHue, SchemeLength)
    End Sub

#End Region

#Region "ColorScheme Preview Image"

    ''' <summary>
    ''' Plot direction for the color-gradient of
    ''' the ColorScheme preview image
    ''' </summary>
    Public Enum ColorSchemePreviewDirections
        Vertical
        Horizontal
    End Enum

    ''' <summary>
    ''' Returns a preview image of the color-schemes colors
    ''' </summary>
    Public Function GetPreviewImage(ByVal Height As Integer,
                                    ByVal Width As Integer,
                                    ByVal GradientDirection As ColorSchemePreviewDirections) As Image

        ' Check dimensions
        If Width <= 0 Or Height <= 0 Then Throw New ArgumentException("Previewcreation of ColorScheme: ImageSize is invalid!")
        If Me.Length <= 0 Then Throw New ArgumentException("Previewcreation of ColorScheme: ColorScheme is invalid!")

        ' Create Bitmap
        Dim B As New Bitmap(Width, Height)

        ' Load fast-Image class
        Dim I As New cFastImage(B)

        ' Get the number of color-steps.
        Dim ColorStep As Integer = 0

        I.Lock()
        ' Paint the gradient according to the chosen direction.
        If GradientDirection = ColorSchemePreviewDirections.Horizontal Then

            ' Now paint the color-steps to the preview-image.
            For X As Integer = 0 To Width - 1 Step 1
                ColorStep = CInt(X / Width * (Me.Length - 1))

                For Y As Integer = 0 To Height - 1 Step 1
                    If ColorStep < Me.Length Then
                        I.SetPixel(X, Y, Me.Item(ColorStep))
                    End If
                Next
            Next

        Else

            ' Now paint the color-steps to the preview-image.
            For Y As Integer = 0 To Height - 1 Step 1
                ColorStep = CInt(Y / Height * (Me.Length - 1))

                For X As Integer = 0 To Width - 1 Step 1
                    If ColorStep < Me.Length Then
                        I.SetPixel(X, Y, Me.Item(ColorStep))
                    End If
                Next
            Next

        End If
        I.Unlock(True)

        ' Return Bitmap
        I = Nothing
        Return B
    End Function

#End Region

#Region "Get available color-schemes"
    ''' <summary>
    ''' Returns a list of all available color schemes
    ''' </summary>
    Public Shared Function AvailableColorSchemes() As List(Of cColorScheme)

        ' Add System's default schemes.
        Dim ColorSchemeList As New List(Of cColorScheme)({cColorScheme.Autumn, cColorScheme.Colorcube, cColorScheme.Cool,
                                                          cColorScheme.Copper, cColorScheme.Flag, cColorScheme.Hot,
                                                          cColorScheme.Hsv, cColorScheme.Jet, cColorScheme.Lines,
                                                          cColorScheme.Pink, cColorScheme.Prism, cColorScheme.Spring,
                                                          cColorScheme.Summer, cColorScheme.Winter, cColorScheme.Gray})

        ' Scan plugin directory, and add all schemes in there
        Dim ColorSchemePluginPath As String = Path.GetDirectoryName(Application.ExecutablePath) & Path.DirectorySeparatorChar & "ColorSchemes"
        If Directory.Exists(ColorSchemePluginPath) Then
            Dim ColorFiles As String() = Directory.GetFiles(ColorSchemePluginPath, "*.sfc")
            Dim ReaderBuffer As String = ""
            Dim ImportedColorScheme As cColorScheme
            For i As Integer = 0 To ColorFiles.Length - 1 Step 1
                ImportedColorScheme = cColorScheme.ImportFromCSV(ColorFiles(i), ReaderBuffer)
                If Not ImportedColorScheme Is Nothing Then ColorSchemeList.Add(ImportedColorScheme)
            Next
        End If

        ' Return list
        Return ColorSchemeList

    End Function
#End Region

#Region "XML Import/Export"
    ''' <summary>
    ''' Returns a new color-scheme by an XML-definition.
    ''' </summary>
    Public Shared Function ImportFromXML(XMLString As String) As cColorScheme
        Try
            ' Generate the Color-Array from which to create the scheme.
            Dim ColorArray As New List(Of Color)
            Dim SchemeName As String = "default"

            ' Load the stream-reader.
            Dim StringReader As New IO.StringReader(XMLString)
            Dim XR As New Xml.XmlTextReader(StringReader)

            ' Now read the XML, and import the settings.
            With XR
                ' read up to the end of the file
                Do While .Read
                    ' Check for the type of data
                    Select Case .NodeType
                        Case Xml.XmlNodeType.Element
                            ' An element comes: this is what we are looking for!
                            '####################################################
                            Select Case .Name
                                Case "ColorSchemeInformation"
                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "SchemeName"
                                                    SchemeName = .Value
                                            End Select
                                        End While
                                    End If

                                Case "ColorStep"
                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then
                                        Dim A As Integer = -1
                                        Dim R As Integer = -1
                                        Dim G As Integer = -1
                                        Dim B As Integer = -1

                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "A"
                                                    A = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "R"
                                                    R = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "G"
                                                    G = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                                Case "B"
                                                    B = Convert.ToInt32(.Value, System.Globalization.CultureInfo.InvariantCulture)
                                            End Select
                                        End While

                                        ' Create a valid color!
                                        If A >= 0 And R >= 0 And G >= 0 And B >= 0 Then
                                            Dim oColor As Color = Color.FromArgb(A, R, G, B)
                                            ColorArray.Add(oColor)
                                        End If
                                    End If
                            End Select
                    End Select

                Loop

                ' Close the XML-Reader
                .Close()
                StringReader.Close()
                StringReader.Dispose()
            End With

            Return New cColorScheme(ColorArray, SchemeName)
        Catch ex As Exception
            Debug.WriteLine("ColorScheme could not be loaded from XML." & vbNewLine & ex.Message)
            Return New cColorScheme
        End Try
    End Function

    ''' <summary>
    ''' Writes an XML representation of the ColorScheme to a Stream
    ''' DOES NOT CLOSE THE UNDERLYING STREAM AFTERWARDS.
    ''' </summary>
    Public Function ExportToXML() As String

        ' StringBuilder erstellen
        Dim SB As New StringBuilder

        Try
            ' Create the XmlTextWriter object
            Dim XMLobj As XmlWriter = XmlWriter.Create(SB)

            With XMLobj
                ' create the document header
                '.WriteStartDocument()
                .WriteStartElement("root")

                ' Begin with SpectraFox program properties
                .WriteStartElement("SpectraFox")
                .WriteAttributeString("Version", cProgrammDeployment.GetProgramVersionString)
                .WriteEndElement()

                ' Begin the general description
                .WriteStartElement("ColorSchemeInformation")
                .WriteAttributeString("SchemeName", Me.SchemeName)
                .WriteEndElement()

                ' Begin the section of the columns.
                .WriteStartElement("Colors")
                For i As Integer = 0 To Me._SchemeArray.Length - 1 Step 1
                    ' Write an element for each data-column
                    .WriteStartElement("ColorStep")
                    .WriteAttributeString("A", Me._SchemeArray(i).A.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteAttributeString("R", Me._SchemeArray(i).R.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteAttributeString("G", Me._SchemeArray(i).G.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteAttributeString("B", Me._SchemeArray(i).B.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    .WriteEndElement()
                Next
                .WriteEndElement()

                ' End <root>
                .WriteEndElement()

                ' Close the document
                '.WriteEndDocument()
                .Close()
            End With
        Catch ex As Exception
            Debug.WriteLine("Error writing cColorScheme-XML-stream:" & ex.Message)
        End Try

        Return SB.ToString
    End Function

#End Region

#Region "CSV Import"

    ''' <summary>
    ''' Returns a new color-scheme by an CSV-definition.
    ''' Colors are defined per line, in an RGB fashion.
    ''' E.g.:
    ''' 255,255,255
    ''' 0,0,0
    ''' </summary>
    Public Shared Function ImportFromCSV(ByVal FullFileNamePlusPath As String,
                                         Optional ByRef ReaderBuffer As String = "") As cColorScheme
        Try
            ' Generate the Color-Array from which to create the scheme.
            Dim ColorArray As New List(Of Color)
            Dim SchemeName As String = Path.GetFileNameWithoutExtension(FullFileNamePlusPath)

            ' Load StreamReader
            Dim oStreamReader As New StreamReader(FullFileNamePlusPath)

            ' Read file Line-By-Line. Write the Splitted Values in a String-Array
            ReaderBuffer = ""
            Dim SplittedLine As String()

            ' Read Settings up to the Position of Spectroscopy
            ' Data in the file. Starts with [DATA].
            Do Until oStreamReader.EndOfStream

                ReaderBuffer = oStreamReader.ReadLine

                SplittedLine = Split(ReaderBuffer, ",", 3)
                If SplittedLine.Length <> 3 Then Continue Do

                ' Read String-Values
                Dim sR As String = SplittedLine(0).Trim
                Dim sG As String = SplittedLine(1).Trim
                Dim sB As String = SplittedLine(2).Trim

                ' Convert Values to RGB
                Dim R As Integer = -1
                Dim G As Integer = -1
                Dim B As Integer = -1

                ' Parse strings
                Dim ParseSuccess As Boolean = True
                ParseSuccess = ParseSuccess And Integer.TryParse(sR, R)
                ParseSuccess = ParseSuccess And Integer.TryParse(sG, G)
                ParseSuccess = ParseSuccess And Integer.TryParse(sB, B)

                ' Check, if parsing was successfull
                If ParseSuccess Then
                    ColorArray.Add(Color.FromArgb(R, G, B))
                End If
            Loop

            ' Close Stream
            oStreamReader.Close()
            oStreamReader.Dispose()

            Return New cColorScheme(ColorArray, SchemeName)
        Catch ex As Exception
            Debug.WriteLine("ColorScheme could not be loaded from CSV." & vbNewLine & ex.Message)
            Return New cColorScheme
        End Try
    End Function

#End Region

#Region "Operators"

    ''' <summary>
    ''' Compares two color-schemes by their name and counts!
    ''' </summary>
    Public Shared Operator =(C1 As cColorScheme, C2 As cColorScheme) As Boolean
        If C1.SchemeName = C2.SchemeName And
           C1.Length = C2.Length Then
            Return True
        Else
            Return False
        End If
    End Operator

    ''' <summary>
    ''' Compares two color-schemes by their name and counts!
    ''' </summary>
    Public Shared Operator <>(C1 As cColorScheme, C2 As cColorScheme) As Boolean
        Return Not C1 = C2
    End Operator

#End Region

#Region "Color-Scheme value extraction for contour-plotting"

    ''' <summary>
    ''' Returns the color with which a value between the two extremal values should be plotted
    ''' in the given predefined color-scale. Returns NaN, if the brush could not be found.
    ''' </summary>
    Public Shared Function GetPlotColorFromColorScale(ByVal MaxIntensity As Double,
                                                      ByVal MinIntensity As Double,
                                                      ByVal ValueToEvaluate As Double,
                                                      ByRef BrushArray As SolidBrush()) As SolidBrush

        ' Security checks.
        If Double.IsNaN(ValueToEvaluate) Then Return Nothing

        Dim IntensityChangeFactor As Double = (MaxIntensity - MinIntensity) / (BrushArray.Length - 1.0)
        If IntensityChangeFactor = 0 Then Return Nothing

        Dim BrushNumber As Integer = CInt(Math.Truncate((ValueToEvaluate - MinIntensity) / IntensityChangeFactor))

        If BrushNumber < 0 Or BrushNumber >= BrushArray.Length Then
            Return Nothing
        Else
            Return BrushArray(BrushNumber)
        End If
    End Function

#End Region


#Region "Predefined Colorschemes as shared Functions."
    Public Shared ReadOnly Autumn As New cColorScheme(New Byte(,) {{255, 0, 0}, {255, 4, 0}, {255, 8, 0}, {255, 12, 0}, {255, 16, 0}, {255, 20, 0}, _
     {255, 24, 0}, {255, 28, 0}, {255, 32, 0}, {255, 36, 0}, {255, 40, 0}, {255, 45, 0}, _
     {255, 49, 0}, {255, 53, 0}, {255, 57, 0}, {255, 61, 0}, {255, 65, 0}, {255, 69, 0}, _
     {255, 73, 0}, {255, 77, 0}, {255, 81, 0}, {255, 85, 0}, {255, 89, 0}, {255, 93, 0}, _
     {255, 97, 0}, {255, 101, 0}, {255, 105, 0}, {255, 109, 0}, {255, 113, 0}, {255, 117, 0}, _
     {255, 121, 0}, {255, 125, 0}, {255, 130, 0}, {255, 134, 0}, {255, 138, 0}, {255, 142, 0}, _
     {255, 146, 0}, {255, 150, 0}, {255, 154, 0}, {255, 158, 0}, {255, 162, 0}, {255, 166, 0}, _
     {255, 170, 0}, {255, 174, 0}, {255, 178, 0}, {255, 182, 0}, {255, 186, 0}, {255, 190, 0}, _
     {255, 194, 0}, {255, 198, 0}, {255, 202, 0}, {255, 206, 0}, {255, 210, 0}, {255, 215, 0}, _
     {255, 219, 0}, {255, 223, 0}, {255, 227, 0}, {255, 231, 0}, {255, 235, 0}, {255, 239, 0}, _
     {255, 243, 0}, {255, 247, 0}, {255, 251, 0}, {255, 255, 0}}, "Autumn")

    Public Shared ReadOnly Colorcube As New cColorScheme(New Byte(,) {{85, 85, 0}, {85, 170, 0}, {85, 255, 0}, {170, 85, 0}, {170, 170, 0}, {170, 255, 0}, _
     {255, 85, 0}, {255, 170, 0}, {255, 255, 0}, {0, 85, 128}, {0, 170, 128}, {0, 255, 128}, _
     {85, 0, 128}, {85, 85, 128}, {85, 170, 128}, {85, 255, 128}, {170, 0, 128}, {170, 85, 128}, _
     {170, 170, 128}, {170, 255, 128}, {255, 0, 128}, {255, 85, 128}, {255, 170, 128}, {255, 255, 128}, _
     {0, 85, 255}, {0, 170, 255}, {0, 255, 255}, {85, 0, 255}, {85, 85, 255}, {85, 170, 255}, _
     {85, 255, 255}, {170, 0, 255}, {170, 85, 255}, {170, 170, 255}, {170, 255, 255}, {255, 0, 255}, _
     {255, 85, 255}, {255, 170, 255}, {43, 0, 0}, {85, 0, 0}, {128, 0, 0}, {170, 0, 0}, _
     {213, 0, 0}, {255, 0, 0}, {0, 43, 0}, {0, 85, 0}, {0, 128, 0}, {0, 170, 0}, _
     {0, 213, 0}, {0, 255, 0}, {0, 0, 43}, {0, 0, 85}, {0, 0, 128}, {0, 0, 170}, _
     {0, 0, 213}, {0, 0, 255}, {0, 0, 0}, {36, 36, 36}, {73, 73, 73}, {109, 109, 109}, _
     {146, 146, 146}, {182, 182, 182}, {219, 219, 219}, {255, 255, 255}}, "Colorcube")

    Public Shared ReadOnly Cool As New cColorScheme(New Byte(,) {{0, 255, 255}, {4, 251, 255}, {8, 247, 255}, {12, 243, 255}, {16, 239, 255}, {20, 235, 255}, _
     {24, 231, 255}, {28, 227, 255}, {32, 223, 255}, {36, 219, 255}, {40, 215, 255}, {45, 210, 255}, _
     {49, 206, 255}, {53, 202, 255}, {57, 198, 255}, {61, 194, 255}, {65, 190, 255}, {69, 186, 255}, _
     {73, 182, 255}, {77, 178, 255}, {81, 174, 255}, {85, 170, 255}, {89, 166, 255}, {93, 162, 255}, _
     {97, 158, 255}, {101, 154, 255}, {105, 150, 255}, {109, 146, 255}, {113, 142, 255}, {117, 138, 255}, _
     {121, 134, 255}, {125, 130, 255}, {130, 125, 255}, {134, 121, 255}, {138, 117, 255}, {142, 113, 255}, _
     {146, 109, 255}, {150, 105, 255}, {154, 101, 255}, {158, 97, 255}, {162, 93, 255}, {166, 89, 255}, _
     {170, 85, 255}, {174, 81, 255}, {178, 77, 255}, {182, 73, 255}, {186, 69, 255}, {190, 65, 255}, _
     {194, 61, 255}, {198, 57, 255}, {202, 53, 255}, {206, 49, 255}, {210, 45, 255}, {215, 40, 255}, _
     {219, 36, 255}, {223, 32, 255}, {227, 28, 255}, {231, 24, 255}, {235, 20, 255}, {239, 16, 255}, _
     {243, 12, 255}, {247, 8, 255}, {251, 4, 255}, {255, 0, 255}}, "Cool")

    Public Shared ReadOnly Copper As New cColorScheme(New Byte(,) {{0, 0, 0}, {5, 3, 2}, {10, 6, 4}, {15, 9, 6}, {20, 13, 8}, {25, 16, 10}, _
     {30, 19, 12}, {35, 22, 14}, {40, 25, 16}, {46, 28, 18}, {51, 32, 20}, {56, 35, 22}, _
     {61, 38, 24}, {66, 41, 26}, {71, 44, 28}, {76, 47, 30}, {81, 51, 32}, {86, 54, 34}, _
     {91, 57, 36}, {96, 60, 38}, {101, 63, 40}, {106, 66, 42}, {111, 70, 44}, {116, 73, 46}, _
     {121, 76, 48}, {126, 79, 50}, {132, 82, 52}, {137, 85, 54}, {142, 89, 56}, {147, 92, 58}, _
     {152, 95, 60}, {157, 98, 62}, {162, 101, 64}, {167, 104, 66}, {172, 108, 68}, {177, 111, 70}, _
     {182, 114, 72}, {187, 117, 75}, {192, 120, 77}, {197, 123, 79}, {202, 126, 81}, {207, 130, 83}, _
     {212, 133, 85}, {218, 136, 87}, {223, 139, 89}, {228, 142, 91}, {233, 145, 93}, {238, 149, 95}, _
     {243, 152, 97}, {248, 155, 99}, {253, 158, 101}, {255, 161, 103}, {255, 164, 105}, {255, 168, 107}, _
     {255, 171, 109}, {255, 174, 111}, {255, 177, 113}, {255, 180, 115}, {255, 183, 117}, {255, 187, 119}, _
     {255, 190, 121}, {255, 193, 123}, {255, 196, 125}, {255, 199, 127}}, "Copper")

    Public Shared ReadOnly Flag As New cColorScheme(New Byte(,) {{255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, _
     {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, _
     {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, _
     {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, _
     {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, _
     {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, _
     {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, _
     {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, _
     {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, _
     {0, 0, 255}, {0, 0, 0}, {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}, _
     {255, 0, 0}, {255, 255, 255}, {0, 0, 255}, {0, 0, 0}}, "Flag")

    Public Shared ReadOnly Hot As New cColorScheme(New Byte(,) {{11, 0, 0}, {21, 0, 0}, {32, 0, 0}, {43, 0, 0}, {53, 0, 0}, {64, 0, 0}, _
     {74, 0, 0}, {85, 0, 0}, {96, 0, 0}, {106, 0, 0}, {117, 0, 0}, {128, 0, 0}, _
     {138, 0, 0}, {149, 0, 0}, {159, 0, 0}, {170, 0, 0}, {181, 0, 0}, {191, 0, 0}, _
     {202, 0, 0}, {213, 0, 0}, {223, 0, 0}, {234, 0, 0}, {244, 0, 0}, {255, 0, 0}, _
     {255, 11, 0}, {255, 21, 0}, {255, 32, 0}, {255, 43, 0}, {255, 53, 0}, {255, 64, 0}, _
     {255, 74, 0}, {255, 85, 0}, {255, 96, 0}, {255, 106, 0}, {255, 117, 0}, {255, 128, 0}, _
     {255, 138, 0}, {255, 149, 0}, {255, 159, 0}, {255, 170, 0}, {255, 181, 0}, {255, 191, 0}, _
     {255, 202, 0}, {255, 213, 0}, {255, 223, 0}, {255, 234, 0}, {255, 244, 0}, {255, 255, 0}, _
     {255, 255, 16}, {255, 255, 32}, {255, 255, 48}, {255, 255, 64}, {255, 255, 80}, {255, 255, 96}, _
     {255, 255, 112}, {255, 255, 128}, {255, 255, 143}, {255, 255, 159}, {255, 255, 175}, {255, 255, 191}, _
     {255, 255, 207}, {255, 255, 223}, {255, 255, 239}, {255, 255, 255}}, "Hot")

    Public Shared ReadOnly Hsv As New cColorScheme(New Byte(,) {{255, 0, 0}, {255, 24, 0}, {255, 48, 0}, {255, 72, 0}, {255, 96, 0}, {255, 120, 0}, _
     {255, 143, 0}, {255, 167, 0}, {255, 191, 0}, {255, 215, 0}, {255, 239, 0}, {247, 255, 0}, _
     {223, 255, 0}, {199, 255, 0}, {175, 255, 0}, {151, 255, 0}, {128, 255, 0}, {104, 255, 0}, _
     {80, 255, 0}, {56, 255, 0}, {32, 255, 0}, {8, 255, 0}, {0, 255, 16}, {0, 255, 40}, _
     {0, 255, 64}, {0, 255, 88}, {0, 255, 112}, {0, 255, 135}, {0, 255, 159}, {0, 255, 183}, _
     {0, 255, 207}, {0, 255, 231}, {0, 255, 255}, {0, 231, 255}, {0, 207, 255}, {0, 183, 255}, _
     {0, 159, 255}, {0, 135, 255}, {0, 112, 255}, {0, 88, 255}, {0, 64, 255}, {0, 40, 255}, _
     {0, 16, 255}, {8, 0, 255}, {32, 0, 255}, {56, 0, 255}, {80, 0, 255}, {104, 0, 255}, _
     {128, 0, 255}, {151, 0, 255}, {175, 0, 255}, {199, 0, 255}, {223, 0, 255}, {247, 0, 255}, _
     {255, 0, 239}, {255, 0, 215}, {255, 0, 191}, {255, 0, 167}, {255, 0, 143}, {255, 0, 120}, _
     {255, 0, 96}, {255, 0, 72}, {255, 0, 48}, {255, 0, 24}}, "HSV")

    Public Shared ReadOnly Jet As New cColorScheme(New Byte(,) {{0, 0, 143}, {0, 0, 159}, {0, 0, 175}, {0, 0, 191}, {0, 0, 207}, {0, 0, 223}, _
     {0, 0, 239}, {0, 0, 255}, {0, 16, 255}, {0, 32, 255}, {0, 48, 255}, {0, 64, 255}, _
     {0, 80, 255}, {0, 96, 255}, {0, 112, 255}, {0, 128, 255}, {0, 143, 255}, {0, 159, 255}, _
     {0, 175, 255}, {0, 191, 255}, {0, 207, 255}, {0, 223, 255}, {0, 239, 255}, {0, 255, 255}, _
     {16, 255, 239}, {32, 255, 223}, {48, 255, 207}, {64, 255, 191}, {80, 255, 175}, {96, 255, 159}, _
     {112, 255, 143}, {128, 255, 128}, {143, 255, 112}, {159, 255, 96}, {175, 255, 80}, {191, 255, 64}, _
     {207, 255, 48}, {223, 255, 32}, {239, 255, 16}, {255, 255, 0}, {255, 239, 0}, {255, 223, 0}, _
     {255, 207, 0}, {255, 191, 0}, {255, 175, 0}, {255, 159, 0}, {255, 143, 0}, {255, 128, 0}, _
     {255, 112, 0}, {255, 96, 0}, {255, 80, 0}, {255, 64, 0}, {255, 48, 0}, {255, 32, 0}, _
     {255, 16, 0}, {255, 0, 0}, {239, 0, 0}, {223, 0, 0}, {207, 0, 0}, {191, 0, 0}, _
     {175, 0, 0}, {159, 0, 0}, {143, 0, 0}, {128, 0, 0}}, "Jet")

    Public Shared ReadOnly Lines As New cColorScheme(New Byte(,) {{0, 0, 255}, {0, 128, 0}, {255, 0, 0}, {0, 191, 191}, {191, 0, 191}, {191, 191, 0}, _
     {64, 64, 64}, {0, 0, 255}, {0, 128, 0}, {255, 0, 0}, {0, 191, 191}, {191, 0, 191}, _
     {191, 191, 0}, {64, 64, 64}, {0, 0, 255}, {0, 128, 0}, {255, 0, 0}, {0, 191, 191}, _
     {191, 0, 191}, {191, 191, 0}, {64, 64, 64}, {0, 0, 255}, {0, 128, 0}, {255, 0, 0}, _
     {0, 191, 191}, {191, 0, 191}, {191, 191, 0}, {64, 64, 64}, {0, 0, 255}, {0, 128, 0}, _
     {255, 0, 0}, {0, 191, 191}, {191, 0, 191}, {191, 191, 0}, {64, 64, 64}, {0, 0, 255}, _
     {0, 128, 0}, {255, 0, 0}, {0, 191, 191}, {191, 0, 191}, {191, 191, 0}, {64, 64, 64}, _
     {0, 0, 255}, {0, 128, 0}, {255, 0, 0}, {0, 191, 191}, {191, 0, 191}, {191, 191, 0}, _
     {64, 64, 64}, {0, 0, 255}, {0, 128, 0}, {255, 0, 0}, {0, 191, 191}, {191, 0, 191}, _
     {191, 191, 0}, {64, 64, 64}, {0, 0, 255}, {0, 128, 0}, {255, 0, 0}, {0, 191, 191}, _
     {191, 0, 191}, {191, 191, 0}, {64, 64, 64}, {0, 0, 255}}, "Lines")

    Public Shared ReadOnly Pink As New cColorScheme(New Byte(,) {{30, 0, 0}, {50, 26, 26}, {64, 37, 37}, {75, 45, 45}, {85, 52, 52}, {94, 59, 59}, _
     {102, 64, 64}, {110, 69, 69}, {117, 74, 74}, {123, 79, 79}, {130, 83, 83}, {136, 87, 87}, _
     {141, 91, 91}, {147, 95, 95}, {152, 98, 98}, {157, 102, 102}, {162, 105, 105}, {167, 108, 108}, _
     {172, 111, 111}, {176, 114, 114}, {181, 117, 117}, {185, 120, 120}, {189, 123, 123}, {194, 126, 126}, _
     {195, 132, 129}, {197, 138, 131}, {199, 144, 134}, {201, 149, 136}, {202, 154, 139}, {204, 159, 141}, _
     {206, 164, 144}, {207, 169, 146}, {209, 174, 148}, {211, 178, 151}, {212, 183, 153}, {214, 187, 155}, _
     {216, 191, 157}, {217, 195, 160}, {219, 199, 162}, {220, 203, 164}, {222, 207, 166}, {223, 211, 168}, _
     {225, 215, 170}, {226, 218, 172}, {228, 222, 174}, {229, 225, 176}, {231, 229, 178}, {232, 232, 180}, _
     {234, 234, 185}, {235, 235, 191}, {237, 237, 196}, {238, 238, 201}, {240, 240, 206}, {241, 241, 211}, _
     {243, 243, 216}, {244, 244, 221}, {245, 245, 225}, {247, 247, 230}, {248, 248, 234}, {250, 250, 238}, _
     {251, 251, 243}, {252, 252, 247}, {254, 254, 251}, {255, 255, 255}}, "Pink")

    Public Shared ReadOnly Prism As New cColorScheme(New Byte(,) {{255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}, {0, 0, 255}, {170, 0, 255}, _
     {255, 0, 0}, {255, 128, 0}, {255, 255, 0}, {0, 255, 0}}, "Prism")

    Public Shared ReadOnly Spring As New cColorScheme(New Byte(,) {{255, 0, 255}, {255, 4, 251}, {255, 8, 247}, {255, 12, 243}, {255, 16, 239}, {255, 20, 235}, _
     {255, 24, 231}, {255, 28, 227}, {255, 32, 223}, {255, 36, 219}, {255, 40, 215}, {255, 45, 210}, _
     {255, 49, 206}, {255, 53, 202}, {255, 57, 198}, {255, 61, 194}, {255, 65, 190}, {255, 69, 186}, _
     {255, 73, 182}, {255, 77, 178}, {255, 81, 174}, {255, 85, 170}, {255, 89, 166}, {255, 93, 162}, _
     {255, 97, 158}, {255, 101, 154}, {255, 105, 150}, {255, 109, 146}, {255, 113, 142}, {255, 117, 138}, _
     {255, 121, 134}, {255, 125, 130}, {255, 130, 125}, {255, 134, 121}, {255, 138, 117}, {255, 142, 113}, _
     {255, 146, 109}, {255, 150, 105}, {255, 154, 101}, {255, 158, 97}, {255, 162, 93}, {255, 166, 89}, _
     {255, 170, 85}, {255, 174, 81}, {255, 178, 77}, {255, 182, 73}, {255, 186, 69}, {255, 190, 65}, _
     {255, 194, 61}, {255, 198, 57}, {255, 202, 53}, {255, 206, 49}, {255, 210, 45}, {255, 215, 40}, _
     {255, 219, 36}, {255, 223, 32}, {255, 227, 28}, {255, 231, 24}, {255, 235, 20}, {255, 239, 16}, _
     {255, 243, 12}, {255, 247, 8}, {255, 251, 4}, {255, 255, 0}}, "Spring")

    Public Shared ReadOnly Summer As New cColorScheme(New Byte(,) {{0, 128, 102}, {4, 130, 102}, {8, 132, 102}, {12, 134, 102}, {16, 136, 102}, {20, 138, 102}, _
     {24, 140, 102}, {28, 142, 102}, {32, 144, 102}, {36, 146, 102}, {40, 148, 102}, {45, 150, 102}, _
     {49, 152, 102}, {53, 154, 102}, {57, 156, 102}, {61, 158, 102}, {65, 160, 102}, {69, 162, 102}, _
     {73, 164, 102}, {77, 166, 102}, {81, 168, 102}, {85, 170, 102}, {89, 172, 102}, {93, 174, 102}, _
     {97, 176, 102}, {101, 178, 102}, {105, 180, 102}, {109, 182, 102}, {113, 184, 102}, {117, 186, 102}, _
     {121, 188, 102}, {125, 190, 102}, {130, 192, 102}, {134, 194, 102}, {138, 196, 102}, {142, 198, 102}, _
     {146, 200, 102}, {150, 202, 102}, {154, 204, 102}, {158, 206, 102}, {162, 208, 102}, {166, 210, 102}, _
     {170, 212, 102}, {174, 215, 102}, {178, 217, 102}, {182, 219, 102}, {186, 221, 102}, {190, 223, 102}, _
     {194, 225, 102}, {198, 227, 102}, {202, 229, 102}, {206, 231, 102}, {210, 233, 102}, {215, 235, 102}, _
     {219, 237, 102}, {223, 239, 102}, {227, 241, 102}, {231, 243, 102}, {235, 245, 102}, {239, 247, 102}, _
     {243, 249, 102}, {247, 251, 102}, {251, 253, 102}, {255, 255, 102}}, "Summer")

    Public Shared ReadOnly Winter As New cColorScheme(New Byte(,) {{0, 0, 255}, {0, 4, 253}, {0, 8, 251}, {0, 12, 249}, {0, 16, 247}, {0, 20, 245}, _
     {0, 24, 243}, {0, 28, 241}, {0, 32, 239}, {0, 36, 237}, {0, 40, 235}, {0, 45, 233}, _
     {0, 49, 231}, {0, 53, 229}, {0, 57, 227}, {0, 61, 225}, {0, 65, 223}, {0, 69, 221}, _
     {0, 73, 219}, {0, 77, 217}, {0, 81, 215}, {0, 85, 213}, {0, 89, 210}, {0, 93, 208}, _
     {0, 97, 206}, {0, 101, 204}, {0, 105, 202}, {0, 109, 200}, {0, 113, 198}, {0, 117, 196}, _
     {0, 121, 194}, {0, 125, 192}, {0, 130, 190}, {0, 134, 188}, {0, 138, 186}, {0, 142, 184}, _
     {0, 146, 182}, {0, 150, 180}, {0, 154, 178}, {0, 158, 176}, {0, 162, 174}, {0, 166, 172}, _
     {0, 170, 170}, {0, 174, 168}, {0, 178, 166}, {0, 182, 164}, {0, 186, 162}, {0, 190, 160}, _
     {0, 194, 158}, {0, 198, 156}, {0, 202, 154}, {0, 206, 152}, {0, 210, 150}, {0, 215, 148}, _
     {0, 219, 146}, {0, 223, 144}, {0, 227, 142}, {0, 231, 140}, {0, 235, 138}, {0, 239, 136}, _
     {0, 243, 134}, {0, 247, 132}, {0, 251, 130}, {0, 255, 128}}, "Winter")

    Public Shared ReadOnly Gray As New cColorScheme(New Byte(,) {{0, 0, 0}, {1, 1, 1}, {2, 2, 2}, {3, 3, 3}, {4, 4, 4}, {5, 5, 5},
                                                                 {6, 6, 6}, {7, 7, 7}, {8, 8, 8}, {9, 9, 9}, {10, 10, 10}, {11, 11, 11},
                                                                 {12, 12, 12}, {13, 13, 13}, {14, 14, 14}, {15, 15, 15}, {16, 16, 16},
                                                                 {17, 17, 17}, {18, 18, 18}, {19, 19, 19}, {20, 20, 20}, {21, 21, 21},
                                                                 {22, 22, 22}, {23, 23, 23}, {24, 24, 24}, {25, 25, 25},
                                                                 {26, 26, 26}, {27, 27, 27}, {28, 28, 28}, {29, 29, 29}, {30, 30, 30},
                                                                 {31, 31, 31}, {32, 32, 32}, {33, 33, 33}, {34, 34, 34}, {35, 35, 35},
                                                                 {36, 36, 36}, {37, 37, 37}, {38, 38, 38}, {39, 39, 39}, {40, 40, 40},
                                                                 {41, 41, 41}, {42, 42, 42}, {43, 43, 43}, {44, 44, 44}, {45, 45, 45},
                                                                 {46, 46, 46}, {47, 47, 47}, {48, 48, 48}, {49, 49, 49}, {50, 50, 50},
                                                                 {51, 51, 51}, {52, 52, 52}, {53, 53, 53}, {54, 54, 54}, {55, 55, 55},
                                                                 {56, 56, 56}, {57, 57, 57}, {58, 58, 58}, {59, 59, 59}, {60, 60, 60},
                                                                 {61, 61, 61}, {62, 62, 62}, {63, 63, 63}, {64, 64, 64}, {65, 65, 65},
                                                                 {66, 66, 66}, {67, 67, 67}, {68, 68, 68}, {69, 69, 69}, {70, 70, 70},
                                                                 {71, 71, 71}, {72, 72, 72}, {73, 73, 73}, {74, 74, 74}, {75, 75, 75},
                                                                 {76, 76, 76}, {77, 77, 77}, {78, 78, 78}, {79, 79, 79}, {80, 80, 80},
                                                                 {81, 81, 81}, {82, 82, 82}, {83, 83, 83}, {84, 84, 84}, {85, 85, 85},
                                                                 {86, 86, 86}, {87, 87, 87}, {88, 88, 88}, {89, 89, 89}, {90, 90, 90},
                                                                 {91, 91, 91}, {92, 92, 92}, {93, 93, 93}, {94, 94, 94}, {95, 95, 95},
                                                                 {96, 96, 96}, {97, 97, 97}, {98, 98, 98}, {99, 99, 99}, {100, 100, 100},
                                                                 {101, 101, 101}, {102, 102, 102}, {103, 103, 103}, {104, 104, 104},
                                                                 {105, 105, 105}, {106, 106, 106}, {107, 107, 107}, {108, 108, 108},
                                                                 {109, 109, 109}, {110, 110, 110}, {111, 111, 111}, {112, 112, 112},
                                                                 {113, 113, 113}, {114, 114, 114}, {115, 115, 115}, {116, 116, 116},
                                                                 {117, 117, 117}, {118, 118, 118}, {119, 119, 119}, {120, 120, 120},
                                                                 {121, 121, 121}, {122, 122, 122}, {123, 123, 123}, {124, 124, 124},
                                                                 {125, 125, 125}, {126, 126, 126}, {127, 127, 127}, {128, 128, 128},
                                                                 {129, 129, 129}, {130, 130, 130}, {131, 131, 131}, {132, 132, 132},
                                                                 {133, 133, 133}, {134, 134, 134}, {135, 135, 135}, {136, 136, 136},
                                                                 {137, 137, 137}, {138, 138, 138}, {139, 139, 139}, {140, 140, 140},
                                                                 {141, 141, 141}, {142, 142, 142}, {143, 143, 143}, {144, 144, 144},
                                                                 {145, 145, 145}, {146, 146, 146}, {147, 147, 147}, {148, 148, 148},
                                                                 {149, 149, 149}, {150, 150, 150}, {151, 151, 151}, {152, 152, 152},
                                                                 {153, 153, 153}, {154, 154, 154}, {155, 155, 155}, {156, 156, 156},
                                                                 {157, 157, 157}, {158, 158, 158}, {159, 159, 159}, {160, 160, 160},
                                                                 {161, 161, 161}, {162, 162, 162}, {163, 163, 163}, {164, 164, 164},
                                                                 {165, 165, 165}, {166, 166, 166}, {167, 167, 167}, {168, 168, 168},
                                                                 {169, 169, 169}, {170, 170, 170}, {171, 171, 171}, {172, 172, 172},
                                                                 {173, 173, 173}, {174, 174, 174}, {175, 175, 175}, {176, 176, 176},
                                                                 {177, 177, 177}, {178, 178, 178}, {179, 179, 179}, {180, 180, 180},
                                                                 {181, 181, 181}, {182, 182, 182}, {183, 183, 183}, {184, 184, 184},
                                                                 {185, 185, 185}, {186, 186, 186}, {187, 187, 187}, {188, 188, 188},
                                                                 {189, 189, 189}, {190, 190, 190}, {191, 191, 191}, {192, 192, 192},
                                                                 {193, 193, 193}, {194, 194, 194}, {195, 195, 195}, {196, 196, 196},
                                                                 {197, 197, 197}, {198, 198, 198}, {199, 199, 199}, {200, 200, 200},
                                                                 {201, 201, 201}, {202, 202, 202}, {203, 203, 203}, {204, 204, 204},
                                                                 {205, 205, 205}, {206, 206, 206}, {207, 207, 207}, {208, 208, 208}, {209, 209, 209},
                                                                 {210, 210, 210}, {211, 211, 211}, {212, 212, 212}, {213, 213, 213},
                                                                 {214, 214, 214}, {215, 215, 215}, {216, 216, 216}, {217, 217, 217},
                                                                 {218, 218, 218}, {219, 219, 219}, {220, 220, 220}, {221, 221, 221},
                                                                 {222, 222, 222}, {223, 223, 223}, {224, 224, 224}, {225, 225, 225},
                                                                 {226, 226, 226}, {227, 227, 227}, {228, 228, 228}, {229, 229, 229},
                                                                 {230, 230, 230}, {231, 231, 231}, {232, 232, 232}, {233, 233, 233},
                                                                 {234, 234, 234}, {235, 235, 235}, {236, 236, 236}, {237, 237, 237},
                                                                 {238, 238, 238}, {239, 239, 239}, {240, 240, 240}, {241, 241, 241},
                                                                 {242, 242, 242}, {243, 243, 243}, {244, 244, 244}, {245, 245, 245},
                                                                 {246, 246, 246}, {247, 247, 247}, {248, 248, 248}, {249, 249, 249},
                                                                 {250, 250, 250}, {251, 251, 251}, {252, 252, 252}, {253, 253, 253},
                                                                 {254, 254, 254}, {255, 255, 255}}, "Gray")
#End Region

End Class
