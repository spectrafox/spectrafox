Public Interface iNumericSmoothingFunction

    ''' <summary>
    ''' Name of the smoothing method.
    ''' </summary>
    ReadOnly Property Name As String

    ''' <summary>
    ''' Description of the smoothing method.
    ''' </summary>
    ReadOnly Property Description As String

    ''' <summary>
    ''' Smoothes the given data using the given properties of the smoothing function.
    ''' </summary>
    Function Smooth(ByRef InColumn As ICollection(Of Double)) As List(Of Double)

    ''' <summary>
    ''' Returns the settings control.
    ''' </summary>
    Function SmoothingOptions() As UserControl

    ''' <summary>
    ''' Gets or sets a string with the current settings of the routine
    ''' encoded in the string to be storable in the program settings.
    ''' </summary>
    Property CurrentSmoothingSettings As String


End Interface
