Imports System.Text

Public Class cExportMethod_Ascii_IGORWave
    Inherits cExportMethod_Ascii

    Public Overrides Function ExportDescription() As String
        Return My.Resources.rExportMethods.ExportTypeDesc_IGORWave
    End Function

    Public Overrides Function FileExtension() As String
        Return ".igor.itx"
    End Function

    Public Sub New(Optional ValuesInInvariantCulture As Boolean = True,
               Optional Use10InsteadOfE As Boolean = False,
               Optional AddAdditionalEmptyHeaderLines As Integer = 0)
        MyBase.New(ValuesInInvariantCulture, Use10InsteadOfE, AddAdditionalEmptyHeaderLines)
    End Sub

    Public Overrides Function GetFormattedValue(Value As Double) As String
        Dim S As String = Value.ToString("0.0000000E+000", System.Globalization.CultureInfo.InvariantCulture)
        S = S.Replace("E", "e")
        Return S
    End Function

    Public Overrides Function GetValueDelimiter() As String
        Return vbTab
    End Function

    Public Overrides Function CustomFormatSettingsAllowed() As Boolean
        Return False
    End Function

    Public Overrides Function ExportName() As String
        Return My.Resources.rExportMethods.ExportTypeName_IGORWave & " (" & Me.FileExtension & ")"
    End Function


    ''' <summary>
    ''' Returns the full header.
    ''' </summary>
    Public Overrides Function GetHeader(ByRef SpectroscopyTable As cSpectroscopyTable) As String
        Dim S As New StringBuilder

        S.AppendLine("IGOR")
        S.Append("WAVES/N=(")
        S.Append(SpectroscopyTable.MeasurementPoints)
        S.Append(",")
        S.Append(SpectroscopyTable.GetColumnList.Count.ToString)
        S.Append(")")
        S.Append(vbTab)
        S.AppendLine(SpectroscopyTable.FileNameWithoutPathAndExtension)
        S.AppendLine("BEGIN")

        Return S.ToString
    End Function

    ''' <summary>
    ''' Returns the footer, containing all the columns.
    ''' </summary>
    Public Overrides Function GetFooter(ByRef SpectroscopyTable As cSpectroscopyTable) As String
        Dim S As New StringBuilder

        S.AppendLine("END")

        Dim ColList As List(Of cSpectroscopyTable.DataColumn) = SpectroscopyTable.GetColumnList.Values.ToList
        Dim ColMaxIndex As Integer = ColList.Count - 1

        ' SET SCALE
        S.Append("X SetScale/P x 0,1,"""", ")
        S.Append(SpectroscopyTable.FileNameWithoutPathAndExtension)
        S.Append("; SetScale/P y 0,1,"""", ")
        S.Append(SpectroscopyTable.FileNameWithoutPathAndExtension)
        S.Append("; SetScale d 0,0,"""", ")
        S.AppendLine(SpectroscopyTable.FileNameWithoutPathAndExtension)

        ' Column-Names
        For i As Integer = 0 To ColMaxIndex Step 1
            ' Create Columns
            S.Append("X SetDimLabel 1, ")
            S.Append(i.ToString)
            S.Append(", '")
            S.Append(ColList(i).Name)
            S.Append("', ")
            S.AppendLine(SpectroscopyTable.FileNameWithoutPathAndExtension)
        Next

        ' COMMENT
        S.Append("X Note ")
        S.Append(SpectroscopyTable.FileNameWithoutPathAndExtension)
        S.Append(", """)
        S.Append(SpectroscopyTable.Comment.Replace(vbNewLine, ";"))
        S.AppendLine("""")

        Return S.ToString
    End Function



End Class
