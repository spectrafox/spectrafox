Public Class cFitParameterGroup
    Implements IEnumerable(Of cFitParameter)

#Region "Properties"

    ''' <summary>
    ''' Unique identifier of this fit-parameter group.
    ''' Created when creating the fit-group.
    ''' </summary>
    Private Property _Identifier As Guid

    ''' <summary>
    ''' Only use, if fit-parameters are not bound to a function.
    ''' </summary>
    Protected Sub ChangeIdentifier(ByVal Identifier As Guid)
        Me._Identifier = Identifier
    End Sub

    ''' <summary>
    ''' Only use, if fit-parameters are not bound to a function.
    ''' </summary>
    Public Sub ChangeIdentifier(ByVal Identifier As String)
        Me._Identifier = New Guid(Identifier)
    End Sub

    ''' <summary>
    ''' Constructor generating a new GUID
    ''' </summary>
    Public Sub New()
        Me._Identifier = Guid.NewGuid()
    End Sub

    ''' <summary>
    ''' Constructor, using the given GUID
    ''' </summary>
    Public Sub New(ByVal Identifier As Guid)
        Me._Identifier = Identifier
    End Sub

    ''' <summary>
    ''' Constructor, using the given GUID
    ''' </summary>
    Public Sub New(ByVal Identifier As String)
        Me._Identifier = New Guid(Identifier)
    End Sub

    ''' <summary>
    ''' Identifier for this fit-parameter group.
    ''' Should be unique among other groups.
    ''' </summary>
    Public ReadOnly Property Identifier As Guid
        Get
            Return Me._Identifier
        End Get
    End Property

    ''' <summary>
    ''' Identifier for this fit-parameter group.
    ''' Should be unique among other groups.
    ''' </summary>
    Public ReadOnly Property IdentifierString As String
        Get
            Return Me._Identifier.ToString("N")
        End Get
    End Property

    ''' <summary>
    ''' Gives the GUID-String
    ''' </summary>
    Public Overrides Function ToString() As String
        Return Me.IdentifierString
    End Function

    ''' <summary>
    ''' List to store all the fit-parameters for a fit-model to which this group belongs to.
    ''' </summary>
    Private _FitParameters As New List(Of cFitParameter)

    ''' <summary>
    ''' Dictionary to store all locations of the fit-parameters for a fit-model to which this group belongs to.
    ''' (Identifier, Index)
    ''' </summary>
    Private _FitParameterIdentifiersToIndex As New Dictionary(Of String, Integer)

    ''' <summary>
    ''' Access Fit-Parameters.
    ''' </summary>
    Public Function cFitParameterGroup(ByVal Identifier As String) As cFitParameter
        Return Me.Parameter(Identifier)
    End Function

    ''' <summary>
    ''' Returns the Parameter chosen.
    ''' </summary>
    Public Function Parameter(ByVal Identifier As String) As cFitParameter
        Return Me._FitParameters(Me._FitParameterIdentifiersToIndex(Identifier))
    End Function

    ''' <summary>
    ''' Returns the Parameter chosen.
    ''' </summary>
    Public Function ParameterByIndex(ByVal Index As Integer) As cFitParameter
        Return Me._FitParameters(Index)
    End Function

    ''' <summary>
    ''' Returns if a Group exists
    ''' </summary>
    Public Function ContainsKey(ByVal Identifier As String) As Boolean
        Return Me._FitParameterIdentifiersToIndex.ContainsKey(Identifier)
    End Function

    ''' <summary>
    ''' Returns if a Group exists
    ''' </summary>
    Public Function Exists(ByVal Identifier As String) As Boolean
        Return Me._FitParameterIdentifiersToIndex.ContainsKey(Identifier)
    End Function

    ''' <summary>
    ''' Counts all FitParameters
    ''' </summary>
    Public Function Count() As Integer
        Return Me._FitParameters.Count
    End Function

    ''' <summary>
    ''' Counts all non-fixed FitParameters
    ''' </summary>
    Public Function CountNonFixed() As Integer
        Dim Count As Integer = 0
        For Each FP As cFitParameter In Me._FitParameters
            If Not FP.IsFixed Then
                Count += 1
            End If
        Next
        Return Count
    End Function

    ''' <summary>
    ''' Counts all fixed FitParameters
    ''' </summary>
    Public Function CountFixed() As Integer
        Dim Count As Integer = 0
        For Each FP As cFitParameter In Me._FitParameters
            If FP.IsFixed Then
                Count += 1
            End If
        Next
        Return Count
    End Function

    ''' <summary>
    ''' Get all Identifiers for non-fixed FitParameters, but DONT give the parameter identifier,
    ''' but the internal identifier, just valid in this group.
    ''' </summary>
    Public Function GetNonFixedInternalIdentifiers() As String()
        Dim Identifiers As New List(Of String)
        For Each FPKV As KeyValuePair(Of String, Integer) In Me._FitParameterIdentifiersToIndex
            If Not Me._FitParameters(FPKV.Value).IsFixed Then
                Identifiers.Add(FPKV.Key)
            End If
        Next
        Return Identifiers.ToArray
    End Function

    ''' <summary>
    ''' Get all Identifiers for non-fixed FitParameters, saved in the parameters themselves.
    ''' </summary>
    Public Function GetNonFixedIdentifiers() As String()
        Dim Identifiers As New List(Of String)
        For Each FP As cFitParameter In Me._FitParameters
            If Not FP.IsFixed Then
                Identifiers.Add(FP.Identifier)
            End If
        Next
        Return Identifiers.ToArray
    End Function

    ''' <summary>
    ''' Clears all FitParameters
    ''' </summary>
    Public Sub Clear()
        Me._FitParameters.Clear()
    End Sub

    ''' <summary>
    ''' Groups this fit-parameter group into a new group.
    ''' </summary>
    Public Function ToGroupGroup() As cFitParameterGroupGroup
        Dim G As New cFitParameterGroupGroup
        G.Add(Me, "")
        Return G
    End Function

    ''' <summary>
    ''' Gets all information about locked parameters.
    ''' Returns Dictionary (ParameterIdentifier that is locked, Parameter identifier of the lock-target)
    ''' </summary>
    Public Function GetLockedParameterInfo() As Dictionary(Of String, String)
        Dim LockTo As New Dictionary(Of String, String)
        For Each FP As cFitParameter In Me._FitParameters
            If FP.LockedToParameterIdentifier <> "" Then
                LockTo.Add(FP.GetIdentifierForGroupID(Me.Identifier), FP.LockedToParameterIdentifier)
            End If
        Next
        Return LockTo
    End Function

    ''' <summary>
    ''' Get the values of all parameters as array.
    ''' </summary>
    Public Function GetParameterValues() As Double()
        Dim Values(Me._FitParameters.Count - 1) As Double
        For i As Integer = 0 To Me._FitParameters.Count - 1 Step 1
            Values(i) = Me._FitParameters(i).Value
        Next
        Return Values
    End Function

    ''' <summary>
    ''' Groupname, given to, when added to a fit-parameter-group group.
    ''' </summary>
    Public Property GroupGroupName As String

#End Region

#Region "Adding and removing FitParameters"

    ''' <summary>
    ''' Wrapper for AddFitParameter, uses the group-internal Identifier from the FitParameter
    ''' </summary>
    Public Function Add(ByRef FitParameter As cFitParameter) As Boolean
        Return Me.AddFitParameter(FitParameter.Identifier, FitParameter)
    End Function

    ''' <summary>
    ''' Wrapper for AddFitParameter.
    ''' </summary>
    Public Function Add(ByVal Identifier As String, ByRef FitParameter As cFitParameter) As Boolean
        Return Me.AddFitParameter(Identifier, FitParameter)
    End Function

    ''' <summary>
    ''' Adds a fit-parameter to the group.
    ''' True, if added!
    ''' </summary>
    Public Function AddFitParameter(ByVal Identifier As String, ByRef FitParameter As cFitParameter) As Boolean
        If Me._FitParameterIdentifiersToIndex.ContainsKey(Identifier) Then
            Return False
        Else
            ' Get the new index, where the parameter is stored in the list.
            Dim NewIndex As Integer = Me._FitParameters.Count

            ' Add to the FitParameter-List
            Me._FitParameters.Add(FitParameter)

            ' Add to the registry dictionary
            Me._FitParameterIdentifiersToIndex.Add(Identifier, NewIndex)

            Return True
        End If
    End Function

    ''' <summary>
    ''' Removes a fit-parameter from the group.
    ''' True, if added!
    ''' </summary>
    Public Function RemoveFitParameter(ByVal Identifier As String) As Boolean
        If Not Me._FitParameterIdentifiersToIndex.ContainsKey(Identifier) Then
            Return False
        Else
            ' Get the new index, where the parameter is stored in the list.
            Dim Index As Integer = Me._FitParameterIdentifiersToIndex(Identifier)

            ' Add to the FitParameter-List
            Me._FitParameters.RemoveAt(Index)

            ' Remove the parameter from the registry dictionary
            Me._FitParameterIdentifiersToIndex.Remove(Identifier)

            ' Rewrite registry with new parameters
            For i As Integer = Index + 1 To Me._FitParameters.Count Step 1
                For Each FP As cFitParameter In Me._FitParameters
                    If Me._FitParameterIdentifiersToIndex(FP.Identifier) = i Then
                        Me._FitParameterIdentifiersToIndex(FP.Identifier) = i - 1
                        Exit For
                    End If
                Next
            Next

            Return True
        End If
    End Function

#End Region

#Region "Enumerator"
    Public Function GetEnumerator() As IEnumerator(Of cFitParameter) Implements IEnumerable(Of cFitParameter).GetEnumerator
        Return New FitParameterGroupEnum(Me._FitParameters)
    End Function

    Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New FitParameterGroupEnum(Me._FitParameters)
    End Function
#End Region

#Region "Export / Import"

    ''' <summary>
    ''' Exporting fit-parameters as XML
    ''' </summary>
    Public Overridable Function ExportXML(ByVal IncludeRootElement As Boolean) As String

        ' Create the XmlTextWriter object
        Dim SB As New System.IO.StringWriter
        Dim XMLobj As New Xml.XmlTextWriter(SB)

        With XMLobj
            ' Set the proper formatting
            .Formatting = Xml.Formatting.Indented
            .Indentation = 4

            ' create the document header
            If IncludeRootElement Then
                .WriteStartDocument()
                .WriteStartElement("root") ' <root>
            End If

            ' First element of fit parameter group
            .WriteStartElement("FitParameterGroup") ' <FitParameterGroup>
            .WriteAttributeString("Identifier", Me.IdentifierString)

            ' Begin the element of the parameter description
            .WriteStartElement("FitParameters") ' <FitParameters>

            ' Write an element for each Fit-Parameter
            For Each FP As cFitParameter In Me._FitParameters
                ' check, if the fit-parameter should be ignored
                ' -> the function can be overridden by a child class to return true instead of false.
                If Me.Export_IgnoreFitParameter(FP.Identifier) Then Continue For

                ' write the parameter-settings
                .WriteStartElement("FitParameter") ' <FitParameter
                .WriteAttributeString("Identifier", FP.Identifier.ToString)
                .WriteAttributeString("Name", FP.Name.ToString)
                .WriteAttributeString("Value", FP.Value.ToString(Globalization.CultureInfo.InvariantCulture))
                .WriteAttributeString("IsFixed", FP.IsFixed.ToString(Globalization.CultureInfo.InvariantCulture))
                .WriteAttributeString("LockedTo", FP.LockedToParameterIdentifier.ToString(Globalization.CultureInfo.InvariantCulture))
                .WriteAttributeString("LockedWithFactor", FP.LockedWithFactor.ToString(Globalization.CultureInfo.InvariantCulture))
                If FP.UpperBoundary < Double.MaxValue Then
                    .WriteAttributeString("UpperBoundary", FP.UpperBoundary.ToString(Globalization.CultureInfo.InvariantCulture))
                Else
                    .WriteAttributeString("UpperBoundary", Double.NaN.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                If FP.LowerBoundary > Double.MinValue Then
                    .WriteAttributeString("LowerBoundary", FP.LowerBoundary.ToString(Globalization.CultureInfo.InvariantCulture))
                Else
                    .WriteAttributeString("LowerBoundary", Double.NaN.ToString(Globalization.CultureInfo.InvariantCulture))
                End If
                .WriteEndElement() ' FitParameter /> 
            Next

            ' Close the Section
            .WriteEndElement() ' <\FitParameters>
            .WriteEndElement() ' <\FitParameterGroup> 

            ' Close the document
            If IncludeRootElement Then
                .WriteEndElement() ' <\root>
                .WriteEndDocument()
            End If

            ' Close the XML-Document
            .Close() ' Document 
        End With

        Return SB.ToString
    End Function

    ''' <summary>
    ''' Import XML from a given Stream
    ''' </summary>
    Public Overridable Function ImportXML(FileStream As IO.Stream, ByRef ParameterIdentifiedCallback As iFitFunction.Import_ParameterIdentified) As Boolean

        ' Go to the beginning of the stream, if possible.
        If FileStream.CanSeek Then FileStream.Seek(0, IO.SeekOrigin.Begin)

        ' Open the XML-reader object for the specified file
        Dim XMLReader As Xml.XmlReader = New Xml.XmlTextReader(FileStream)

        ' Import
        Dim Result As Boolean = Me.ImportXML(XMLReader, ParameterIdentifiedCallback)

        ' Release ressources
        FileStream.Close()

        Return Result
    End Function

    ''' <summary>
    ''' Import XML from a given XMLReader
    ''' </summary>
    Public Overridable Function ImportXML(XMLReader As Xml.XmlReader, ByRef ParameterIdentifiedCallback As iFitFunction.Import_ParameterIdentified) As Boolean

        Try
            ' Not read the XML-file, and import the settings.
            With XMLReader
                ' read up to the end of the file
                Do While .Read
                    ' Check for the type of data
                    Select Case .NodeType

                        Case Xml.XmlNodeType.Element
                            ' An element comes: this is what we are looking for!
                            '####################################################

                            Select Case .Name
                                Case "FitParameterGroup"
                                    ' get and check the properties:
                                    '###############################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Identifier"
                                                    Me.ChangeIdentifier(.Value)
                                            End Select
                                        End While
                                    End If

                                Case "FitParameter"
                                    ' get all parameters:
                                    '#####################
                                    ' go through all attributes
                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Identifier"

                                                    ' Fit-Parameter found! Extract settings
                                                    Dim ParameterIdentifier As String = .Value
                                                    Dim Value As Double = 0D
                                                    Dim IsFixed As Boolean = False
                                                    Dim ParameterName As String = ""
                                                    Dim LockedTo As String = ""
                                                    Dim LockedWithFactor As Double = 1
                                                    Dim UpperBoundary As Double = Double.MaxValue
                                                    Dim LowerBoundary As Double = Double.MinValue

                                                    While .MoveToNextAttribute
                                                        Select Case .Name
                                                            Case "Name"
                                                                ParameterName = .Value
                                                            Case "Value"
                                                                Value = Convert.ToDouble(.Value, Globalization.CultureInfo.InvariantCulture)
                                                            Case "IsFixed"
                                                                IsFixed = Convert.ToBoolean(.Value, Globalization.CultureInfo.InvariantCulture)
                                                            Case "LockedTo"
                                                                LockedTo = Convert.ToString(.Value, Globalization.CultureInfo.InvariantCulture)
                                                            Case "LockedWithFactor"
                                                                LockedWithFactor = Convert.ToDouble(.Value, Globalization.CultureInfo.InvariantCulture)
                                                            Case "UpperBoundary"
                                                                UpperBoundary = Convert.ToDouble(.Value, Globalization.CultureInfo.InvariantCulture)
                                                            Case "LowerBoundary"
                                                                LowerBoundary = Convert.ToDouble(.Value, Globalization.CultureInfo.InvariantCulture)
                                                        End Select
                                                    End While

                                                    ' Call a possible constructor
                                                    Dim Parameter As New cFitParameter(ParameterName, Value, IsFixed, , LockedTo, UpperBoundary, LowerBoundary, , LockedWithFactor)
                                                    ParameterIdentifiedCallback(ParameterIdentifier, Parameter)

                                                    ' Save values
                                                    Me.Parameter(ParameterIdentifier).IsFixed = IsFixed
                                                    Me.Parameter(ParameterIdentifier).LockedToParameterIdentifier = LockedTo
                                                    Me.Parameter(ParameterIdentifier).LockedWithFactor = LockedWithFactor
                                                    Me.Parameter(ParameterIdentifier).UpperBoundary = UpperBoundary
                                                    Me.Parameter(ParameterIdentifier).LowerBoundary = LowerBoundary
                                                    Me.Parameter(ParameterIdentifier).ChangeValue(Value, False)

                                            End Select
                                        End While
                                    End If
                            End Select
                    End Select
                Loop
            End With
#If Not Debug Then
            Catch ex As Exception
            MessageBox.Show("Error importing a Fit-Model:" & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
#End If
        Finally
        End Try
        Return True
    End Function

#End Region

    ''' <summary>
    ''' This function returns always false, if not overridden.
    ''' It is called during the export of the individual fit-parameters.
    ''' Use it to exclude certain parameters from being exported.
    ''' If RETURN=TRUE, parameter will not be exported.
    ''' </summary>
    Protected Overridable Function Export_IgnoreFitParameter(ByVal IdentifierOfParameterToIgnore As String) As Boolean
        ' Does nothing
        Return False
    End Function

End Class

Public Class FitParameterGroupEnum
    Implements IEnumerator(Of cFitParameter)

    Private Position As Integer = -1
    Private _FitParameters As List(Of cFitParameter)

    Public Sub New(ByRef FitParameters As List(Of cFitParameter))
        Me._FitParameters = FitParameters
    End Sub

    Public ReadOnly Property Current As cFitParameter Implements IEnumerator(Of cFitParameter).Current
        Get
            Try
                Return Me._FitParameters(Position)
            Catch ex As Exception
                Throw New InvalidOperationException("FitParameterGroup - Entry not found")
            End Try
        End Get
    End Property

    Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
        Get
            Try
                Return Me._FitParameters(Position)
            Catch ex As Exception
                Throw New InvalidOperationException("FitParameterGroup - Entry not found")
            End Try
        End Get
    End Property

    Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        Position += 1

        Return (Position < Me._FitParameters.Count)
    End Function

    Public Sub Reset() Implements IEnumerator.Reset
        Position = -1
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class