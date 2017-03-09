Imports System.Text

''' <summary>
''' Base-Class for exporting a SpectroscopyTable to Mathematica.
''' </summary>
Public Class cExportMethod_Mathematica
    Implements iExportMethod_Ascii

    ''' <summary>
    ''' Child-Class should implement a description of the export-method.
    ''' </summary>
    Public Function ExportDescription() As String Implements iExportMethod_Ascii.ExportDescription
        Return My.Resources.rExportMethods.ExportTypeDesc_Mathematica
    End Function

    ''' <summary>
    ''' Calls and returns the output of the child-class.
    ''' </summary>
    Public Function GetExportOutput(ByRef SpectroscopyTable As cSpectroscopyTable) As String Implements iExportMethod_Ascii.GetExportOutput
        ' Create new StringBuilder with the header as new initial value.
        Dim S As New StringBuilder(Me.GetHeader(SpectroscopyTable))

        ' Write Mathematica-Header
        S.Append(My.Resources.rExportMethods.Mathematica_Header)

        ' For Mathematica Convert Using Mathematica-Class
        S.Append(cMathematica.MathematicaTable.GetMathematicaCode(SpectroscopyTable))

        ' Write Mathematica-Footer
        S.Append(My.Resources.rExportMethods.Mathematica_Footer)

        Return S.ToString
    End Function

    ''' <summary>
    ''' Formatting of the value.
    ''' </summary>
    Public Function GetFormattedValue(Value As Double) As String Implements iExportMethod_Ascii.GetFormattedValue
        Return Value.ToString("0.0000000E+000", System.Globalization.CultureInfo.InvariantCulture).Replace("E+", "*10^(") & ")"
    End Function

    ''' <summary>
    ''' Mathematica File-Extension is ".nb"
    ''' </summary>
    Public Function FileExtension() As String Implements iExportMethod_Ascii.FileExtension
        Return ".nb"
    End Function

    ''' <summary>
    ''' Returns the Delimiter used for separating the values.
    ''' </summary>
    Public Function GetValueDelimiter() As String Implements iExportMethod_Ascii.GetValueDelimiter
        Return String.Empty
    End Function

    ''' <summary>
    ''' Returns an empty header
    ''' </summary>
    Public Overridable Function GetHeader(ByRef SpectroscopyTable As cSpectroscopyTable) As String Implements iExportMethod_Ascii.GetHeader
        Return String.Empty
    End Function

    ''' <summary>
    ''' Returns an empty footer.
    ''' </summary>
    Public Overridable Function GetFooter(ByRef SpectroscopyTable As cSpectroscopyTable) As String Implements iExportMethod_Ascii.GetFooter
        Return String.Empty
    End Function

    ''' <summary>
    ''' No custom formatting of the values allowed!
    ''' </summary>
    Public Function CustomFormatSettingsAllowed() As Boolean Implements iExportMethod_Ascii.CustomFormatSettingsAllowed
        Return False
    End Function

    ''' <summary>
    ''' Descriptive name of the export method.
    ''' </summary>
    Public Function ExportName() As String Implements iExportMethod_Ascii.ExportName
        Return My.Resources.rExportMethods.ExportTypeName_Mathematica & " (" & Me.FileExtension & ")"
    End Function

    ''' <summary>
    ''' Empty, since no formatting is allowed here!
    ''' </summary>
    Public Sub SetCustomFormatSettings(CustomFormatSetting As iExport_CustomFormatSetting) Implements iExportMethod_Ascii.SetCustomFormatSettings
        Return
    End Sub
End Class
