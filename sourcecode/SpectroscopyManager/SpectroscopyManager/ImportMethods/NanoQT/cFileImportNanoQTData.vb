Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public MustInherit Class cFileImportNanoQTData
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Imports the Bias-Spectroscopy-File into a Spectroscopy Table
    ''' </summary>
    Public Function ImportBias(ByRef FullFileNamePlusPath As String,
                               ByVal FetchOnlyFileHeader As Boolean,
                               Optional ByRef ReaderBuffer As String = "",
                               Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                               Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cSpectroscopyTable Implements iFileImport_SpectroscopyTable.ImportSpectroscopyTable

        ' Create new SpectroscopyTable
        Dim oSpectroscopyTable As New cSpectroscopyTable
        oSpectroscopyTable.FullFileName = FullFileNamePlusPath

        ' Load StreamReader
        Using sr As New StreamReader(FullFileNamePlusPath)

            ' Load JSON-Reader
            Using JSR As New Newtonsoft.Json.JsonTextReader(sr)

                Dim CurrentPropertyName As String = String.Empty
                Dim CurrentGroupNames As New List(Of String)
                Dim CurrentPropertyValueString As String = String.Empty
                Dim CurrentPropertyValueDouble As Double = 0
                Dim CurrentPropertyValueInteger As Double = 0
                Dim CurrentColumnX As cSpectroscopyTable.DataColumn = Nothing
                Dim CurrentColumnY As cSpectroscopyTable.DataColumn = Nothing
                Dim DataForXColumn As Boolean = True
                Dim XData As Double
                Dim YData As Double
                Dim DataCount As Integer = 0


                While JSR.Read

                    Select Case JSR.TokenType
                        Case Newtonsoft.Json.JsonToken.PropertyName
                            CurrentPropertyName = Convert.ToString(JSR.Value)
                        Case Newtonsoft.Json.JsonToken.String
                            CurrentPropertyValueString = Convert.ToString(JSR.Value)
                        Case Newtonsoft.Json.JsonToken.Integer
                            CurrentPropertyValueInteger = Convert.ToInt32(JSR.Value)
                        Case Newtonsoft.Json.JsonToken.Float
                            CurrentPropertyValueDouble = Convert.ToDouble(JSR.Value)
                        Case Newtonsoft.Json.JsonToken.StartArray
                            CurrentGroupNames.Add(CurrentPropertyName)
                        Case Newtonsoft.Json.JsonToken.EndArray
                            CurrentGroupNames.RemoveAt(CurrentGroupNames.Count - 1)
                        Case Newtonsoft.Json.JsonToken.StartObject
                            CurrentGroupNames.Add(CurrentPropertyName)
                        Case Newtonsoft.Json.JsonToken.EndObject
                            CurrentGroupNames.RemoveAt(CurrentGroupNames.Count - 1)
                        Case Newtonsoft.Json.JsonToken.StartConstructor
                            CurrentGroupNames.Add(CurrentPropertyName)
                        Case Newtonsoft.Json.JsonToken.EndConstructor
                            CurrentGroupNames.RemoveAt(CurrentGroupNames.Count - 1)
                    End Select

                    If JSR.TokenType = Newtonsoft.Json.JsonToken.EndArray AndAlso
                       CurrentGroupNames.Count > 2 AndAlso
                       CurrentPropertyName = "data" AndAlso
                       CurrentGroupNames(CurrentGroupNames.Count - 1) = "measures" Then
                        oSpectroscopyTable.AddNonPersistentColumn(CurrentColumnX)
                        oSpectroscopyTable.AddNonPersistentColumn(CurrentColumnY)
                    ElseIf JSR.TokenType <> Newtonsoft.Json.JsonToken.PropertyName Then

                        Select Case CurrentGroupNames.Count
                            Case 0
                            Case 1
                                ' Data in the first level
                                Select Case CurrentPropertyName
                                    Case "Name"
                                        oSpectroscopyTable.Comment += "Name: (" + CurrentPropertyValueString + ")" & vbNewLine
                                    Case "Type"
                                        oSpectroscopyTable.Comment += "Type: (" + CurrentPropertyValueString + ")" & vbNewLine
                                    Case "Sample"
                                        oSpectroscopyTable.Comment += "Sample: (" + CurrentPropertyValueString + ")" & vbNewLine
                                    Case "Junction"
                                        oSpectroscopyTable.Comment += "Junction: (" + CurrentPropertyValueString + ")" & vbNewLine

                                End Select
                            Case Else

                                If CurrentPropertyName = "start" Then
                                    Date.TryParse(CurrentPropertyValueString, oSpectroscopyTable.RecordDate)
                                End If

                                If CurrentGroupNames(CurrentGroupNames.Count - 1) = "columns" Then
                                    If JSR.TokenType = Newtonsoft.Json.JsonToken.StartArray Then
                                        CurrentColumnX = New cSpectroscopyTable.DataColumn()
                                        CurrentColumnX.Name = "X"
                                        CurrentColumnY = New cSpectroscopyTable.DataColumn()
                                        CurrentColumnY.Name = "Y"
                                    ElseIf JSR.TokenType = Newtonsoft.Json.JsonToken.String Then
                                        If DataForXColumn Then
                                            CurrentColumnX.Name = CurrentPropertyValueString
                                        Else
                                            CurrentColumnY.Name = CurrentPropertyValueString
                                        End If
                                        DataForXColumn = Not DataForXColumn
                                    End If

                                End If

                                If CurrentColumnX IsNot Nothing AndAlso CurrentColumnY IsNot Nothing Then
                                    If CurrentGroupNames(CurrentGroupNames.Count - 2) = "data" And
                                       CurrentGroupNames(CurrentGroupNames.Count - 1) = "data" Then
                                        If JSR.TokenType = Newtonsoft.Json.JsonToken.StartArray Then
                                            If DataCount = 2 Then
                                                CurrentColumnX.AddValueToColumn(XData)
                                                CurrentColumnY.AddValueToColumn(YData)
                                            End If
                                            DataCount = 0
                                        ElseIf JSR.TokenType = Newtonsoft.Json.JsonToken.Float Then
                                            If DataCount = 0 Then
                                                XData = CurrentPropertyValueDouble
                                            ElseIf DataCount = 1 Then
                                                YData = CurrentPropertyValueDouble
                                            End If
                                            DataCount += 1
                                        End If

                                    End If
                                End If

                        End Select
                    End If

                End While


            End Using

            ' Create a Temporary List of DataColumns
            Dim lColumns As New List(Of cSpectroscopyTable.DataColumn)

            ' Finally add all Columns from the Temporary List to the Spectroscopy-Table
            For Each oColumn As cSpectroscopyTable.DataColumn In lColumns
                oColumn.IsSpectraFoxGenerated = False
                oSpectroscopyTable.AddNonPersistentColumn(oColumn)
            Next

        End Using

        ' File Exists, so Set the Property.
        oSpectroscopyTable._bFileExists = True

        Return oSpectroscopyTable
    End Function

    Public MustOverride ReadOnly Property FileExtension As String Implements iFileImport_SpectroscopyTable.FileExtension

    ''' <summary>
    ''' Checks, if the given File is a known Nanonis File-Type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Using sr As New StreamReader(FullFileNamePlusPath)
            ' The identifier is in the second JSON line
            sr.ReadLine()
            ReaderBuffer = sr.ReadLine()
        End Using
        If ReaderBuffer.Contains("NanoQt JSON data file") Then
            Return True
        End If

        Return False
    End Function

End Class
