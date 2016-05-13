Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class cFileImportBruker

    ''' <summary>
    ''' Settings-Entry Regex (e.g. "CreationTime=Jun/09/2014 4:59:48 PM").
    ''' </summary>
    Protected SettingsRegex As New Regex("^(?<name>.*?)\=(?<value>.*?)$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Value-pair Regex (matches values with a unit, e.g. 0.986 Hz).
    ''' </summary>
    Protected ValuePairRegex As New Regex("^(?<value>[+-]?\s?[\d\.]*?)\s(?<unit>\D*?)$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

    ''' <summary>
    ''' Returns a key value pair from a string (e.g. 0.986 Hz).
    ''' </summary>
    Protected Function GetKeyValuePair(ByVal ValueString As String) As KeyValuePair(Of Double, String)

        Dim KV As KeyValuePair(Of Double, String)
        Dim M As Match = ValuePairRegex.Match(ValueString)
        If M.Success Then
            KV = New KeyValuePair(Of Double, String)(Convert.ToDouble(M.Groups.Item("value").Value, CultureInfo.InvariantCulture), M.Groups.Item("unit").Value)
        Else
            KV = New KeyValuePair(Of Double, String)()
        End If

        Return KV
    End Function

End Class
