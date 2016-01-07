Imports System.Text

''' <summary>
''' Base-Class for Ascii-Export-Functions.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class cExportMethod_Ascii
    Implements iExportMethod_Ascii

    ''' <summary>
    ''' Change to TRUE, to write the values in invariant culture.
    ''' Else, the current culture is used!
    ''' </summary>
    Public Property ValuesInInvariantCulture As Boolean = True

    ''' <summary>
    ''' If set to true, the values will use "*10^" instead of "E+"
    ''' </summary>
    Public Property Use10InsteadOfE As Boolean = False

    ''' <summary>
    ''' Additional empty lines to place after the header section.
    ''' </summary>
    Public Property AdditionalHeaderSpacingLines As Integer = 0

    ''' <summary>
    ''' Initial settings for the formatting of the values.
    ''' </summary>
    Public Sub New(ValuesInInvariantCulture As Boolean,
                   Use10InsteadOfE As Boolean,
                   Optional AddAdditionalHeaderSpacingLines As Integer = 0)
        Me._ValuesInInvariantCulture = ValuesInInvariantCulture
        Me._Use10InsteadOfE = Use10InsteadOfE
        Me._AdditionalHeaderSpacingLines = AddAdditionalHeaderSpacingLines
    End Sub

    ''' <summary>
    ''' Child-Class should implement a description of the export-method.
    ''' </summary>
    Public MustOverride Function ExportDescription() As String Implements iExportMethod_Ascii.ExportDescription

    ''' <summary>
    ''' Calls and returns the output of the child-class.
    ''' </summary>
    Public Function GetExportOutput(ByRef SpectroscopyTable As cSpectroscopyTable) As String Implements iExportMethod_Ascii.GetExportOutput

        ' Create new StringBuilder with the header as new initial value.
        Dim S As New StringBuilder(Me.GetHeader(SpectroscopyTable))

        ' Add additional empty header lines
        For i As Integer = 1 To Me.AdditionalHeaderSpacingLines Step 1
            S.AppendLine()
        Next

        Dim ColListCols As List(Of cSpectroscopyTable.DataColumn) = SpectroscopyTable.GetColumnList.Values.ToList
        Dim ColList As New List(Of ObjectModel.ReadOnlyCollection(Of Double))(ColListCols.Count)
        For Each Col As cSpectroscopyTable.DataColumn In ColListCols
            ColList.Add(Col.Values)
        Next

        Dim ColMaxIndex As Integer = ColList.Count - 1

        ' Get the longest Column
        Dim RowMaxIndex As Integer = 0
        For i As Integer = 0 To ColList.Count - 1 Step 1
            If RowMaxIndex < ColList(i).Count Then
                RowMaxIndex = ColList(i).Count
            End If
        Next
        RowMaxIndex -= 1

        ' Write the data row by row
        Dim RowOnlyContainedNaN As Boolean
        For Row As Integer = 0 To RowMaxIndex Step 1
            RowOnlyContainedNaN = True
            For Col As Integer = 0 To ColMaxIndex Step 1
                If Not Double.IsNaN(ColList(Col)(Row)) And RowOnlyContainedNaN Then
                    RowOnlyContainedNaN = False
                End If
            Next

            If Not RowOnlyContainedNaN Then
                For Col As Integer = 0 To ColMaxIndex Step 1
                    ' Check and write Data
                    If Row >= ColList(Col).Count Then
                        S.Append("")
                    ElseIf Double.IsNaN(ColList(Col)(Row)) Then
                        S.Append("")
                    Else
                        S.Append(Me.GetFormattedValue(ColList(Col)(Row)))
                    End If

                    ' Close line or separate value
                    If Col < ColMaxIndex Then
                        S.Append(Me.GetValueDelimiter)
                    Else
                        S.Append(vbCrLf)
                    End If
                Next
            End If
        Next

        Return S.ToString
    End Function

    ''' <summary>
    ''' The child-class must specify a value.
    ''' </summary>
    Public MustOverride Function GetFormattedValue(Value As Double) As String Implements iExportMethod_Ascii.GetFormattedValue

    ''' <summary>
    ''' The child-class must specify a file-extension.
    ''' </summary>
    Public MustOverride Function FileExtension() As String Implements iExportMethod_Ascii.FileExtension

    ''' <summary>
    ''' Returns the Delimiter used for separating the values.
    ''' </summary>
    Public MustOverride Function GetValueDelimiter() As String Implements iExportMethod_Ascii.GetValueDelimiter

    ''' <summary>
    ''' Returns the full header placed in the beginning of the file!
    ''' Default Method places all "Column-Name (Unit)" in the first row.
    ''' </summary>
    Public Overridable Function GetHeader(ByRef SpectroscopyTable As cSpectroscopyTable) As String Implements iExportMethod_Ascii.GetHeader
        Dim S As New StringBuilder

        Dim ColList As List(Of cSpectroscopyTable.DataColumn) = SpectroscopyTable.GetColumnList.Values.ToList
        Dim ColMaxIndex As Integer = ColList.Count - 1

        For i As Integer = 0 To ColMaxIndex Step 1
            ' Create Columns
            S.Append("""")
            S.Append(ColList(i).Name)
            S.Append(" (")
            S.Append(ColList(i).UnitSymbol)
            S.Append(")")
            S.Append("""")

            If i < ColMaxIndex Then
                S.Append(Me.GetValueDelimiter)
            End If
        Next

        ' Close the last header line
        S.Append(vbCrLf)

        Return S.ToString
    End Function

    ''' <summary>
    ''' The child-classes have to define, if custom formats are allowed.
    ''' </summary>
    Public MustOverride Function CustomFormatSettingsAllowed() As Boolean Implements iExportMethod_Ascii.CustomFormatSettingsAllowed

    ''' <summary>
    ''' Child-Class must override and give a name to the specific function.
    ''' </summary>
    Public MustOverride Function ExportName() As String Implements iExportMethod_Ascii.ExportName

    ''' <summary>
    ''' Write current settings.
    ''' </summary>
    ''' <param name="CustomFormatSetting">Expects an object of Ascii_FormatSetting</param>
    Public Sub SetCustomFormatSettings(CustomFormatSetting As iExport_CustomFormatSetting) Implements iExportMethod_Ascii.SetCustomFormatSettings
        Dim FormatSetting As Ascii_FormatSetting = TryCast(CustomFormatSetting, Ascii_FormatSetting)
        If Not FormatSetting Is Nothing Then
            Me.Use10InsteadOfE = FormatSetting.Use10InsteadOfE
            Me.ValuesInInvariantCulture = FormatSetting.ValuesInInvariantCulture
            Me.AdditionalHeaderSpacingLines = FormatSetting.AdditionalHeaderSpacingLines
        End If
    End Sub

    ''' <summary>
    ''' Format-Setting to use for all child-classes to write or set the settings, such as:
    ''' exponent-type and culture-invariant formatting.
    ''' </summary>
    Public Class Ascii_FormatSetting
        Implements iExport_CustomFormatSetting

        Public Property FormatSettingName As String = "Ascii Export Settings" Implements iExport_CustomFormatSetting.FormatSettingName

        ''' <summary>
        ''' Initial settings for the formatting of the values.
        ''' </summary>
        Public Sub New(ValuesInInvariantCulture As Boolean,
                       Use10InsteadOfE As Boolean,
                       Optional AddAdditionalHeaderSpaceingLines As Integer = 0)
            Me._ValuesInInvariantCulture = ValuesInInvariantCulture
            Me._Use10InsteadOfE = Use10InsteadOfE
            Me._AdditionalHeaderSpacingLines = AddAdditionalHeaderSpaceingLines
        End Sub

        ''' <summary>
        ''' Additional empty lines to place after the header section.
        ''' </summary>
        Public Property AdditionalHeaderSpacingLines As Integer = 0

        ''' <summary>
        ''' Change to TRUE, to write the values in invariant culture.
        ''' Else, the current culture is used!
        ''' </summary>
        Public Property ValuesInInvariantCulture As Boolean = True

        ''' <summary>
        ''' If set to true, the values will use "*10^" instead of "E+"
        ''' </summary>
        Public Property Use10InsteadOfE As Boolean = False
    End Class
End Class
