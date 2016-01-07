'Imports Wolfram.NETLink

Public Class cMathematica

    ''' <summary>
    ''' Represents a Structure, that Creates a Table in Mathematica
    ''' </summary>
    Class MathematicaTable
        ''' <summary>
        ''' Returns the Code for the Generation of the Table in Mathematica.
        ''' </summary>
        Public Shared Function GetMathematicaCode(Columns As IEnumerable(Of ICollection(Of Double)),
                                                  Optional ByVal TableName As String = "") As String
            ' Check, if Columns are Present.
            If Columns.Count <= 0 Then Return ""

            ' Check, if all columns have the same length.
            Dim iColMaxSize As Integer = Columns(0).Count
            For Each col As ReadOnlyCollection(Of Double) In Columns
                If col.Count > iColMaxSize Then
                    iColMaxSize = col.Count
                End If
            Next

            For Each col As ReadOnlyCollection(Of Double) In Columns
                If col.Count <> iColMaxSize Then
                    Throw New ArgumentException("GetMathematicaCode: Input columns not of the same length.")
                End If
            Next

            ' Create Mathematica Code
            Dim iValueNumber As Integer = Columns(0).Count
            Dim iColumnNumber As Integer = Columns.Count
            Dim sEvaluate As String = ""

            ' Use Table-Name if given.
            If TableName <> "" Then sEvaluate &= TableName & " = "

            sEvaluate &= "{"
            For ValueCounter As Integer = 0 To iValueNumber - 1 Step 1
                sEvaluate &= "{"
                For ColumnCounter As Integer = 0 To iColumnNumber - 1 Step 1
                    sEvaluate &= Columns(ColumnCounter)(ValueCounter).ToString("0.###E00", Globalization.CultureInfo.InvariantCulture)
                    If ColumnCounter < iColumnNumber - 1 Then sEvaluate &= ", "
                Next
                sEvaluate &= "}"
                If ValueCounter < iValueNumber - 1 Then sEvaluate &= ", "
            Next
            sEvaluate &= "}; "
            sEvaluate = sEvaluate.Replace("E", "*10^")
            Return sEvaluate
        End Function

        ''' <summary>
        ''' Same Function but with Numbered Collections.
        ''' </summary>
        Public Shared Function GetMathematicaCode(ByRef Columns As List(Of Dictionary(Of Integer, Double)),
                                                  Optional ByVal TableName As String = "") As String
            Dim lNewColumns As New List(Of List(Of Double))(Columns.Count)
            For Each Col As Dictionary(Of Integer, Double) In Columns
                Dim lList As New List(Of Double)(Col.Count)
                For Each Value As Double In Col.Values
                    lList.Add(Value)
                Next
                lNewColumns.Add(lList)
            Next
            Return cMathematica.MathematicaTable.GetMathematicaCode(lNewColumns, TableName)
        End Function

        ''' <summary>
        ''' Same Function but with SpectroscopyTable.
        ''' </summary>
        Public Shared Function GetMathematicaCode(ByRef SpectroscopyTable As cSpectroscopyTable,
                                                  Optional ByVal TableName As String = "") As String
            Dim lNewColumns As New List(Of ReadOnlyCollection(Of Double))
            For Each Col As cSpectroscopyTable.DataColumn In SpectroscopyTable.GetColumnList.Values
                lNewColumns.Add(Col.Values)
            Next
            Return cMathematica.MathematicaTable.GetMathematicaCode(lNewColumns, TableName)
        End Function
    End Class

    ' ''' <summary>
    ' ''' Initializes a MathLink-Connection
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function CreateMathLink() As IKernelLink
    '    Dim MathLink As IKernelLink
    '    MathLink = MathLinkFactory.CreateKernelLink()
    '    ' Discard the initial InputNamePacket the kernel will send when launched.
    '    MathLink.WaitAndDiscardAnswer()
    '    Return MathLink
    'End Function

    ' ''' <summary>
    ' ''' Ends a MathLink-Connection
    ' ''' </summary>
    ' ''' <param name="MathLink"></param>
    ' ''' <remarks></remarks>
    'Public Shared Sub DestroyMathLink(ByRef MathLink As IKernelLink)
    '    MathLink.Close()
    'End Sub
End Class
