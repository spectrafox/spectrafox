Imports System.Threading.Tasks
Imports System.Threading
Imports System.Collections.Concurrent

Public Class cFitFunction_MultiplePeaks
    Inherits cFitFunction

#Region "Properties, such as arrays of all Fit-Functions used!"

    ''' <summary>
    ''' Array with all Fit-Functions used.
    ''' Add or remove them, before running the fit,
    ''' and call afterwards "InitializeFitParameters", to
    ''' create the array of all FitParameters used.
    ''' </summary>
    Public FitFunctions As New List(Of iFitFunction)

    ''' <summary>
    ''' Dictionary that contains the location of the individual
    ''' Fit-Parameters of the individual Fit-Function
    ''' in the total list of Fit-Parameters.
    ''' (FitFunctionIndex, (ParameterIndex, Relocated ParameterIndex))
    ''' </summary>
    Protected FitParameterIndexLocations As New Dictionary(Of Integer, Dictionary(Of Integer, Integer))

    ''' <summary>
    ''' Returns all Fit-Parameters, grouped together.
    ''' </summary>
    Public Overrides Function FitParametersGrouped() As cFitParameterGroupGroup
        Dim G As cFitParameterGroupGroup = Me.FitParametersGroupedWithoutCombinedGroup

        ' Add a group of all parameters as a wrapper group
        G.Add(Me.FitParameters, My.Resources.rFitFunction_MultiplePeaks.CombinedCurveName)

        Return G
    End Function

    ''' <summary>
    ''' Returns all Fit-Parameters, grouped together.
    ''' </summary>
    Public Overrides Function FitParametersGroupedWithoutCombinedGroup() As cFitParameterGroupGroup
        Dim G As New cFitParameterGroupGroup

        ' Add the real groups
        For Each FF As iFitFunction In Me.FitFunctions
            G.Add(FF.FitParameters, FF.FitFunctionName)
        Next

        Return G
    End Function

#End Region

#Region "Initialize and return Fit-Parameter-array."

    ''' <summary>
    ''' Method that sets all Initial FitParameters from the individual FitFunctions
    ''' in this multiple-function-container
    ''' </summary>
    Public Sub ReInitializeFitParameters()
        Me.FitParameters.Clear()

        ' Create the Fit-Parameter array, depending on the Fit-Function
        ' Position in the List of Fit-Functions.
        For i As Integer = 0 To Me.FitFunctions.Count - 1 Step 1
            Me.FitParameterIndexLocations.Add(i, New Dictionary(Of Integer, Integer))

            Dim NewParameterIdentifier As String = "FF_"
            For Each FP As cFitParameter In Me.FitFunctions(i).FitParameters
                With FP
                    Me.FitParameters.Add(NewParameterIdentifier & i & FP.Identifier, FP)
                End With
            Next
        Next

    End Sub

    ''' <summary>
    ''' Method that sets all Initial FitParameters
    ''' </summary>
    Protected Overrides Sub InitializeFitParameters()
        ' Base-Class Call ---- NOTHING
    End Sub

#End Region

#Region "FitFunction"
    ''' <summary>
    ''' Returns the actual FitFunction-Value at a given X
    ''' </summary>
    ''' <param name="x">x-value at which the fit function should be evaluated.</param>
    Public Overrides Function GetY(ByRef x As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

        Dim Result As Double = 0

        ' Go through all Fit-Functions and sum up the function value
        ' Usually each Fit-Function should already have an "offset"
        ' and a "stretch" factor
        For i As Integer = 0 To (Me.FitFunctions.Count - 1) Step 1
            Result += Me.FitFunctions(i).GetY(x, InputParameters)
        Next

        Return Result
    End Function
#End Region

#Region "Fit-Description and Formula"
    ''' <summary>
    ''' Formula used the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionFormula() As String
        Dim FitFormula As New System.Text.StringBuilder

        For i As Integer = 0 To Me.FitFunctions.Count - 1 Step 1
            FitFormula.Append(Me.FitFunctions(i).FitFunctionFormula)
            If i < Me.FitFunctions.Count - 1 Then
                FitFormula.Append(" + ")
                FitFormula.Append(vbNewLine)
            End If
        Next

        Return FitFormula.ToString
    End Function

    ''' <summary>
    ''' Name of the fit-function.
    ''' </summary>
    Public Overrides Function FitFunctionName() As String
        Dim ReturnName As String
        Dim Names As New Dictionary(Of String, Integer)

        If Me.FitFunctions.Count > 0 Then

            ' Count the names of all Fit-Functions and give a summarized name
            For i As Integer = 0 To Me.FitFunctions.Count - 1 Step 1
                If Not Names.ContainsKey(Me.FitFunctions(i).FitFunctionName) Then
                    Names.Add(Me.FitFunctions(i).FitFunctionName, 1)
                Else
                    Names(Me.FitFunctions(i).FitFunctionName) += 1
                End If
            Next

            Dim ReturnNameBuilder As New System.Text.StringBuilder
            ' Create the Return-String
            For Each NamesKV As KeyValuePair(Of String, Integer) In Names
                If NamesKV.Value > 1 Then
                    ReturnNameBuilder.Append(NamesKV.Value)
                    ReturnNameBuilder.Append("x ")
                End If
                ReturnNameBuilder.Append(NamesKV.Key)
                ReturnNameBuilder.Append(" + ")
            Next
            ' Remove the last plus
            ReturnNameBuilder.Remove(ReturnNameBuilder.Length - 3, 3)

            ' Copy it to the string
            ReturnName = ReturnNameBuilder.ToString
        Else
            ReturnName = My.Resources.rFitFunction_MultiplePeaks.Name
        End If

        Return ReturnName
    End Function

    ''' <summary>
    ''' Description of the fit that is performed.
    ''' </summary>
    Public Overrides Function FitDescription() As String
        Return My.Resources.rFitFunction_MultiplePeaks.Description
    End Function

    ''' <summary>
    ''' Authors of the fit function.
    ''' </summary>
    Public Overrides Function FitFunctionAuthors() As String
        Return My.Resources.rFitFunction_MultiplePeaks.Authors
    End Function
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel.
    ''' Dummy! Since multiple peaks are not fittable!
    ''' </summary>
    Overrides ReadOnly Property FunctionSettingPanel As cFitSettingPanel
        Get
            Return New cFitSettingPanel
        End Get
    End Property
#End Region

#Region "Export / Import"

    ''' <summary>
    ''' For exporting and importing a bunch of fit-models,
    ''' each settings-panel should have the ability to generate
    ''' and interpret XML data for the individual parameters.
    ''' THIS FUNCTION CAN BE OVERRIDEN BY THE CHILD PANEL
    ''' </summary>
    Public Overrides Function ExportXML(FileName As String) As Boolean

        Try
            ' Select the File-Encoding
            Dim enc As New System.Text.UnicodeEncoding

            ' Create the XmlTextWriter object
            Dim XMLobj As New Xml.XmlTextWriter(FileName, enc)

            With XMLobj
                ' Set the proper formatting
                .Formatting = Xml.Formatting.Indented
                .Indentation = 4

                ' create the document header
                .WriteStartDocument()
                .WriteStartElement("root") ' <root>

                ' Begin the element fit-function description
                .WriteStartElement("FitFunctions")

                ' Write each fit-function settings
                For Each FF As iFitFunction In Me.FitFunctions

                    ' First element of fit function
                    .WriteStartElement("FitFunction") ' <FitFunction
                    .WriteAttributeString("ClassName", FF.GetType.ToString)
                    .WriteAttributeString("FitFunctionName", FF.FitFunctionName)

                    ' Write Fit-Parameters for each Fit-Function
                    .WriteRaw(FF.FitParameters.ExportXML(False))

                    .WriteEndElement() ' FitFunction />

                Next

                ' Close the Section
                .WriteEndElement()

                ' Write additional properties, if child-classes override the function
                Me.Export_WriteAdditionalXMLElements(XMLobj)

                ' Close the document
                .WriteEndElement() ' <\root>
                .WriteEndDocument()

                ' Close the XML-Document
                .Close() ' Document 

            End With

            Return True
        Catch ex As Exception
            MessageBox.Show("Error exporting Fit-Model:" & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Import Parameters for this FitModel using XML.
    ''' </summary>
    Public Overrides Function ImportXML(FileStream As IO.Stream) As Boolean

        ' Go to the beginning of the stream, if possible.
        If FileStream.CanSeek Then FileStream.Seek(0, IO.SeekOrigin.Begin)

        ' Delete all old fit-functions
        Me.FitFunctions.Clear()

        Try
            ' Open the XML-reader object for the specified file
            Dim XMLReader As Xml.XmlReader = New Xml.XmlTextReader(FileStream)

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
                                Case "FitFunction"
                                    ' get and check the properties:
                                    '###############################
                                    ' go through all attributes

                                    ' Create Fit-Function of saved type
                                    Dim ClassType As Type = Nothing

                                    ' Get the fit-function type by this.
                                    Dim FitFunction As iFitFunction = Nothing

                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "ClassName"

                                                    ' Create Fit-Function
                                                    ClassType = AvailablePlugins.GetType(.Value)
                                                    FitFunction = cFitFunction.GetFitFunctionByType(ClassType)

                                            End Select
                                        End While

                                        If Not FitFunction Is Nothing Then

                                            ' Go back to element
                                            .MoveToElement()

                                            ' Now fill in the parameters from the sub-elements
                                            FitFunction.FitParameters.ImportXML(.ReadSubtree, AddressOf Me.Import_ParameterIdentified)

                                            ' Add FitFunction to the Array
                                            Me.FitFunctions.Add(FitFunction)
                                        End If

                                    End If

                            End Select
                    End Select
                Loop
            End With

            ' Reinitialize the Fit-Parameters
            Me.ReInitializeFitParameters()

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

End Class
