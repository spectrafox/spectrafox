Public Class cStringCollectionHistory

    ''' <summary>
    ''' Appends the last used filter to the list.
    ''' </summary>
    Public Shared Function AppendToHistory(ByVal History As System.Collections.Specialized.StringCollection,
                                           ByVal StringToAppend As String,
                                           Optional ByVal MaxEntries As Integer = 10) As System.Collections.Specialized.StringCollection

        ' Add the filter to the history of strings,
        ' or move the entry to the top of the list, if it already existed.
        Dim iHistoryIndex As Integer = History.IndexOf(StringToAppend)
        Do While iHistoryIndex >= 0
            History.RemoveAt(iHistoryIndex)
            iHistoryIndex = History.IndexOf(StringToAppend)
        Loop
        History.Add(StringToAppend)

        ' remove more than the max number of entries
        Dim OldHistoryCount = History.Count
        For i As Integer = 0 To OldHistoryCount - MaxEntries - 1 Step 1
            If i < 0 Or i > History.Count - 1 Then Exit For
            History.RemoveAt(i)
        Next

        Return History
    End Function

End Class
