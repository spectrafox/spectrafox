Imports System.Text

Public Class cExportMethod_Ascii_ORIGIN
    Inherits cExportMethod_Ascii

    Public Overrides Function ExportDescription() As String
        Return My.Resources.rExportMethods.ExportTypeDesc_Origin
    End Function

    Public Overrides Function FileExtension() As String
        Return ".origin.txt"
    End Function

    Public Sub New(Optional AddAdditionalEmptyHeaderLines As Integer = 0)
        MyBase.New(True, False, AddAdditionalEmptyHeaderLines)
    End Sub

    Public Overrides Function GetFormattedValue(Value As Double) As String
        Return Value.ToString("0.0000000E+000", System.Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Overrides Function GetValueDelimiter() As String
        Return vbTab
    End Function

    ''' <summary>
    ''' Returns the full header placed in the beginning of the file!
    ''' Default Method places all "Column-Name (Unit)" in the first row.
    ''' </summary>
    Public Overrides Function GetHeader(ByRef SpectroscopyTable As cSpectroscopyTable) As String
        Dim S As New StringBuilder

        Dim ColList As List(Of cSpectroscopyTable.DataColumn) = SpectroscopyTable.GetColumnList.Values.ToList
        Dim ColMaxIndex As Integer = ColList.Count - 1

        ' HEADER-Names
        For i As Integer = 0 To ColMaxIndex Step 1
            ' Create Columns
            S.Append("""")
            S.Append(ColList(i).Name)
            S.Append("""")

            If i < ColMaxIndex Then
                S.Append(Me.GetValueDelimiter)
            End If
        Next

        ' Close the first header line
        S.Append(vbCrLf)

        ' HEADER-Units
        For i As Integer = 0 To ColMaxIndex Step 1
            ' Create Columns
            S.Append(ColList(i).UnitSymbol)

            If i < ColMaxIndex Then
                S.Append(Me.GetValueDelimiter)
            End If
        Next

        ' Close the last header line
        S.Append(vbCrLf)

        Return S.ToString
    End Function

    Public Overrides Function CustomFormatSettingsAllowed() As Boolean
        Return False
    End Function

    Public Overrides Function ExportName() As String
        Return My.Resources.rExportMethods.ExportTypeName_Origin & " (" & Me.FileExtension & ")"
    End Function
End Class
