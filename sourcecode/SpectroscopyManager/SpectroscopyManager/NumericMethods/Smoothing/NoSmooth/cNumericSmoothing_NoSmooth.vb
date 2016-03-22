Public Class cNumericSmoothing_NoSmooth
    Implements iNumericSmoothingFunction

    ''' <summary>
    ''' Description of the smoothing function.
    ''' </summary>
    Public ReadOnly Property Description As String Implements iNumericSmoothingFunction.Description
            Get
            Return My.Resources.rSmoothing.Description_NoSmooth
        End Get
        End Property

        ''' <summary>
        ''' Public name of the smoothing function.
        ''' </summary>
        Public ReadOnly Property Name As String Implements iNumericSmoothingFunction.Name
            Get
            Return My.Resources.rSmoothing.Name_NoSmooth
        End Get
        End Property

    ''' <summary>
    ''' Gets or sets a string with the current settings of the routine
    ''' encoded in the string to be storable in the program settings.
    ''' </summary>
    Public Property CurrentSmoothingSettings As String Implements iNumericSmoothingFunction.CurrentSmoothingSettings

    ''' <summary>
    ''' Smoothes the data with the Savitzky Golay method.
    ''' </summary>
    Public Function Smooth(ByRef InColumnY As ICollection(Of Double)) As List(Of Double) Implements iNumericSmoothingFunction.Smooth
        Return InColumnY.ToList
    End Function

    ''' <summary>
    ''' Settings-Control.
    ''' </summary>
    Private SmoothingControl As New UserControl

    ''' <summary>
    ''' Returns the settings control for this smoothing method.
    ''' </summary>
    Public Function SmoothingOptions() As UserControl Implements iNumericSmoothingFunction.SmoothingOptions
            Return Me.SmoothingControl
        End Function

    End Class
