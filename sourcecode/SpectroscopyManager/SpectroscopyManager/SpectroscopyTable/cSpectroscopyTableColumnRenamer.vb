Imports System.Threading

Public Class cSpectroscopyTableColumnRenamer
    Inherits cSpectroscopyTableFetcher

#Region "Properties"

    ''' <summary>
    ''' Stores the current rename-rules.
    ''' </summary>
    Public Property RenameRules As New System.Collections.Specialized.StringCollection

#End Region

#Region "Rule definition"

    ''' <summary>
    ''' Structure to store the properties of the rule
    ''' </summary>
    Public Structure ReplaceRule
        Public SearchFor As String
        Public ReplaceBy As String
        Public DeleteInsteadOfReplace As Boolean

        Public Sub New(SearchFor As String, ReplaceBy As String, DeleteInsteadOfReplace As Boolean)
            Me.SearchFor = SearchFor
            Me.ReplaceBy = ReplaceBy
            Me.DeleteInsteadOfReplace = DeleteInsteadOfReplace
        End Sub
    End Structure

#End Region

#Region "Rename rules: store in settings or get from settings"

    Private Const RenameRuleSplit As String = "#SPLIT#"

    ''' <summary>
    ''' Returns the rename-rules stored in the settings as a dictionary of "Search", "Replace" values.
    ''' </summary>
    Public Shared Function GetRenameRulesFromSettings() As List(Of ReplaceRule)

        Dim RenameRules As System.Collections.Specialized.StringCollection = My.Settings.LastRenameColumnRules
        Dim RuleCollection As New List(Of ReplaceRule)

        Dim SplitRule As String()
        For Each Rule As String In RenameRules
            SplitRule = System.Text.RegularExpressions.Regex.Split(Rule, RenameRuleSplit)
            If SplitRule.Length = 2 Then
                ' old rules which only allow renaming
                RuleCollection.Add(New ReplaceRule(SplitRule(0), SplitRule(1), False))
            ElseIf SplitRule.Length = 3 Then
                ' new rule which allow deleting a row
                Dim DeleteInsteadOfReplace As Boolean
                If Not Boolean.TryParse(SplitRule(2), DeleteInsteadOfReplace) Then
                    DeleteInsteadOfReplace = False
                End If
                RuleCollection.Add(New ReplaceRule(SplitRule(0), SplitRule(1), DeleteInsteadOfReplace))
            End If
        Next

        Return RuleCollection
    End Function

    ''' <summary>
    ''' Returns the rename-rules stored in the settings as a dictionary of "Search", "Replace" values.
    ''' </summary>
    Public Shared Sub SaveRenameRulesToSettings(ByRef RenameRules As List(Of ReplaceRule))

        Dim SettingsRuleCollection As New System.Collections.Specialized.StringCollection
        For Each Rule As ReplaceRule In RenameRules
            SettingsRuleCollection.Add(Rule.SearchFor & RenameRuleSplit & Rule.ReplaceBy & RenameRuleSplit & Rule.DeleteInsteadOfReplace.ToString)
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
    Public Sub RenameColumns(ByRef RenameRules As List(Of ReplaceRule))
        If Me.oSpectroscopyTable Is Nothing Then Me.FetchDirect()

        Dim bColumnsChanged As Boolean = False

        ' Go through all columns, and search for a match with the rule
        For Each RenameRule As ReplaceRule In RenameRules

            ' Check, if the column exists in the file:
            If Me.oSpectroscopyTable.Columns.ContainsKey(RenameRule.SearchFor) Then

                Dim Col As cSpectroscopyTable.DataColumn = Me.oSpectroscopyTable.Columns(RenameRule.SearchFor)

                ' Just modify it, if we are allowed to!
                If Col.IsSpectraFoxGenerated Then

                    ' Rename the column or delete the column, depending on the selection
                    If Not RenameRule.DeleteInsteadOfReplace Then
                        ' RENAME
                        If Col.Name = RenameRule.SearchFor Then
                            Col.Name = RenameRule.ReplaceBy
                            bColumnsChanged = True

                            Me.oSpectroscopyTable.Columns.Remove(RenameRule.SearchFor)
                            ' Change also the key in the dictionary.
                            Me.oSpectroscopyTable.Columns.Add(RenameRule.ReplaceBy, Col)
                        End If
                    Else
                        ' JUST DELETE
                        Me.oSpectroscopyTable.Columns.Remove(RenameRule.SearchFor)
                        bColumnsChanged = True
                    End If

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
