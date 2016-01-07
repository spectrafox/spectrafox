Public Class cExportMethod_Ascii_CSV
    Inherits cExportMethod_Ascii

    Public Overrides Function ExportDescription() As String
        Return My.Resources.rExportMethods.ExportTypeDesc_CSV
    End Function

    Public Overrides Function FileExtension() As String
        Return ".export.csv"
    End Function

    Public Sub New(Optional ValuesInInvariantCulture As Boolean = True,
                   Optional Use10InsteadOfE As Boolean = False,
                   Optional AddAdditionalEmptyHeaderLines As Integer = 0)
        MyBase.New(ValuesInInvariantCulture, Use10InsteadOfE, AddAdditionalEmptyHeaderLines)
    End Sub

    Public Overrides Function GetFormattedValue(Value As Double) As String
        Dim S As String
        If Me.ValuesInInvariantCulture Then
            S = Value.ToString("0.0000000E+000", System.Globalization.CultureInfo.InvariantCulture)
        Else
            S = Value.ToString("0.0000000E+000")
        End If
        If Me.Use10InsteadOfE Then
            S = S.Replace("E", "*10^(") & ")"
        End If
        Return S
    End Function

    Public Overrides Function GetValueDelimiter() As String
        Return ", "
    End Function

    Public Overrides Function CustomFormatSettingsAllowed() As Boolean
        Return True
    End Function

    Public Overrides Function ExportName() As String
        Return My.Resources.rExportMethods.ExportTypeName_CSV & " (" & Me.FileExtension & ")"
    End Function
End Class
