''' <summary>
''' Interface, that describes a method to export SpectroscopyTable-Files
''' </summary>
Public Interface iExportMethod_Ascii

#Region "Descriptive Information"

    ''' <summary>
    ''' This method should link to a resource and return back
    ''' a description of the Export-Function.
    ''' </summary>
    Function ExportDescription() As String

    ''' <summary>
    ''' Function that returns the file-extension of the exported file.
    ''' </summary>
    Function FileExtension() As String

    ''' <summary>
    ''' Function that returns a short description of the file-type.
    ''' To be used in comboboxes as selection
    ''' </summary>
    Function ExportName() As String

#End Region

#Region "Custom Formatting"
    ''' <summary>
    ''' Tells the calling functions, if customization of the format is possible.
    ''' </summary>
    Function CustomFormatSettingsAllowed() As Boolean

    ''' <summary>
    ''' Sets the custom format settings.
    ''' </summary>
    Sub SetCustomFormatSettings(CustomFormatSetting As iExport_CustomFormatSetting)
#End Region

#Region "Format settings"
    ''' <summary>
    ''' Returns a Double-Value in a specific way, used for formatting the exported values.
    ''' </summary>
    Function GetFormattedValue(ByVal Value As Double) As String

    ''' <summary>
    ''' Returns the Delimiter used for separating the values.
    ''' </summary>
    Function GetValueDelimiter() As String
#End Region

#Region "Export Output Creation Functions"
    ''' <summary>
    ''' Function that returns the actual output of the exported files.
    ''' </summary>
    Function GetExportOutput(ByRef SpectroscopyTable As cSpectroscopyTable) As String

    ''' <summary>
    ''' Creates the ASCII header written to the exported File!
    ''' </summary>
    Function GetHeader(ByRef SpectroscopyTable As cSpectroscopyTable) As String
#End Region


End Interface
