Imports System.Threading

Public Class cSpectroscopyTableColumnRenamer
    Inherits cSpectroscopyTableFetcher

#Region "Properties"

    ''' <summary>
    ''' Stores the current rename-rules.
    ''' </summary>
    Public Property RenameRules As New System.Collections.Specialized.StringCollection

#End Region

#Region "Rename rules: store in settings or get from settings"

    Private Const RenameRuleSplit As String = "#SPLIT#"

    ''' <summary>
    ''' Returns the rename-rules stored in the settings as a dictionary of "Search", "Replace" values.
    ''' </summary>
    Public Shared Function GetRenameRulesFromSettings() As Dictionary(Of String, String)

        Dim RenameRules As System.Collections.Specialized.StringCollection = My.Settings.LastRenameColumnRules
        Dim RuleCollection As New Dictionary(Of String, String)

        Dim SplitRule As String()
        For Each Rule As String In RenameRules
            SplitRule = System.Text.RegularExpressions.Regex.Split(Rule, RenameRuleSplit)
            If SplitRule.Length = 2 Then
                RuleCollection.Add(SplitRule(0), SplitRule(1))
            End If
        Next

        Return RuleCollection
    End Function

    ''' <summary>
    ''' Returns the rename-rules stored in the settings as a dictionary of "Search", "Replace" values.
    ''' </summary>
    Public Shared Sub SaveRenameRulesToSettings(ByRef RenameRules As Dictionary(Of String, String))

        Dim SettingsRuleCollection As New System.Collections.Specialized.StringCollection
        For Each Rule As KeyValuePair(Of String, String) In RenameRules
            SettingsRuleCollection.Add(Rule.Key & RenameRuleSplit & Rule.Value)
        Next

        My.Settings.LastRenameColumnRules = SettingsRuleCollection
        cGlobal.SaveSettings()
    End Sub

#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the rename was successfull.
    ''' </summary>
    Public Event ColumnRenameComplete(ByRef SpectroscopyTable As cSpectroscopyTable)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Rename Function"

    ''' <summary>
    ''' Function that renames all the columns in a SpectroscopyTable by the given rules,
    ''' and stores the FileObject.
    ''' </summary>
    Public Sub RenameColumns(ByRef RenameRules As Dictionary(Of String, String))
        If Me.oSpectroscopyTable Is Nothing Then Me.FetchDirect()

        Dim bColumnsChanged As Boolean = False

        For Each RenameRule As KeyValuePair(Of String, String) In RenameRules

            ' Check, if the column exists in the file:
            If Me.oSpectroscopyTable.Columns.ContainsKey(RenameRule.Key) Then

                Dim Col As cSpectroscopyTable.DataColumn = Me.oSpectroscopyTable.Columns(RenameRule.Key)

                ' Just modify it, if we are allowed to!
                If Col.IsSpectraFoxGenerated Then

                    ' Rename the column
                    If Col.Name = RenameRule.Key Then
                        Col.Name = RenameRule.Value
                        bColumnsChanged = True
                    End If

                    ' Change also the key in the dictionary.
                    Me.oSpectroscopyTable.Columns.Remove(RenameRule.Key)
                    Me.oSpectroscopyTable.Columns.Add(RenameRule.Value, Col)

                End If

            End If

        Next

        ' Store the changes
        If bColumnsChanged Then
            Me.oSpectroscopyTable.BaseFileObject.SaveChangesAsFile(True)
        End If

    End Sub

#End Region

End Class
