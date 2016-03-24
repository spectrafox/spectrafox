Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' Class with support functions for the Nanotec import.
''' </summary>
Public Class cFileImportNanotec

    ''' <summary>
    ''' Settings-Entry Regex (e.g. "    Acquisition time: 10/02/2016, 12:19:38.569").
    ''' </summary>
    Protected SettingsRegex As New Regex("^\s*(?<name>.*?)\: (?<value>.*?)$", RegexOptions.Compiled Or RegexOptions.IgnoreCase)

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
