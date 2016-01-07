Imports System.Threading.Tasks
Imports Cudafy
Imports Cudafy.Host
Imports Cudafy.Translator

''' <summary>
''' Abstract class implementing the LMAFunction interface
''' </summary>
Public MustInherit Class cFitFunction
    Implements iFitFunction

#Region "General Fit-Description MustOverrides"
    ''' <summary>
    ''' This method should link to a resource and return back
    ''' a description of the Fit-Function.
    ''' </summary>
    Public MustOverride Function FitDescription() As String Implements iFitFunction.FitDescription

    ''' <summary>
    ''' This method should link to a resource and return back
    ''' a name of the Fit-Function.
    ''' </summary>
    Public MustOverride Function FitFunctionName() As String Implements iFitFunction.FitFunctionName

    ''' <summary>
    ''' This method should link to a resource and return back
    ''' the formula that is used as Fit-Function.
    ''' </summary>
    Public MustOverride Function FitFunctionFormula() As String Implements iFitFunction.FitFunctionFormula

    ''' <summary>
    ''' This method should link to a resource and return back
    ''' the Authors and References that were used for the Fit-Function.
    ''' </summary>
    Public MustOverride Function FitFunctionAuthors() As String Implements iFitFunction.FitFunctionAuthors

#End Region

#Region "FitRange-Check overrides"

    ''' <summary>
    ''' When the fit-function gets loaded, the fitrange can be checked, so that the function does not
    ''' run forever, if the range is e.g. too large for convolution integrals that treat data in the mV range.
    ''' 
    ''' The function should modify the input values to the ranges the function accepts by default.
    ''' It should return false, if the values were modified.
    ''' 
    ''' OVERRIDE IT... by default no action
    ''' </summary>
    Public Overridable Function FitFunctionSuggestsDifferentFitRange(ByRef FitRangeLower As Double, ByRef FitRangeUpper As Double) As Boolean Implements iFitFunction.FitFunctionSuggestsDifferentFitRange
        ' by default do nothing
        Return True
    End Function

#End Region

#Region "CUDA"
    ''' <summary>
    ''' This property must be implemented to let the fit-procedure decide
    ''' if to use CUDA to accelerate the fit-procedure or not!
    ''' </summary>
    Protected _FunctionImplementsCUDAVersion As Boolean = False
    ''' <summary>
    ''' This property must be implemented to let the fit-procedure decide
    ''' if to use CUDA to accelerate the fit-procedure or not!
    ''' </summary>
    Public ReadOnly Property FunctionImplementsCUDAVersion As Boolean Implements iFitFunction.FunctionImplementsCUDAVersion
        Get
            Return Me._FunctionImplementsCUDAVersion
        End Get
    End Property

    ''' <summary>
    ''' This property must be implemented as it is written by the fit-procedure to
    ''' tell the fit-function to use it's CUDA implementation.
    ''' </summary>
    Public Property UseCUDAVersion As Boolean = False Implements iFitFunction.UseCUDAVersion

    ''' <summary>
    ''' Is CUDA initialized?
    ''' </summary>
    Protected bCudaInizialized As Boolean = False

    ''' <summary>
    ''' CUDA GPU used.
    ''' </summary>
    Protected CUDAGPU As GPGPU

    ''' <summary>
    ''' Initializes the Cuda-Device and returns the GPGPU, or NOTHING,
    ''' if a fallback to the CPU happened. In this case also the UseCUDAVersion-Variable
    ''' gets set back to false.
    ''' </summary>
    Protected Function InitializeCUDAOrFallBackToCPU(ParamArray CompilationTypes() As Type) As GPGPU

        ' Get the CUDA-GPU if possible.
        CUDAGPU = cGPUComputing.InitializeCUDAOrFallBackToCPU(CompilationTypes)

        If CUDAGPU Is Nothing Then
            Me.UseCUDAVersion = False
            Me.bCudaInizialized = False
        Else
            Me.bCudaInizialized = True
        End If

        Return CUDAGPU
    End Function
#End Region

#Region "Multi-Threading Options"

    ''' <summary>
    ''' Parallelization Options, e.g. regulating the maximum number of threads used!
    ''' Set .MaxDegreeOfParallelism to -1, to set no limit the the number of parallel threads.
    ''' </summary>
    Public Property MultiThreadingOptions As New ParallelOptions Implements iFitFunction.MultiThreadingOptions

#End Region

#Region "Fit-Parameter Section"

    ''' <summary>
    ''' This property is an array of all Fit-Parameters.
    ''' </summary>
    Public Overridable Property FitParameters As New cFitParameterGroup Implements iFitFunction.FitParameters

    ''' <summary>
    ''' Returns the GUID of the FitParametersGroup, that is explicitly used for this FitFunction
    ''' </summary>
    Public ReadOnly Property UseFitParameterGroupID As Guid Implements iFitFunction.UseFitParameterGroupID
        Get
            Return Me.FitParameters.Identifier
        End Get
    End Property

    ' Store once created.
    Protected _FitParameterGroup As cFitParameterGroupGroup
    ''' <summary>
    ''' Returns Me.FitParameters, grouped into a separate group.
    ''' </summary>
    Public Overridable Function FitParametersGrouped() As cFitParameterGroupGroup Implements iFitFunction.FitParametersGrouped
        If Me._FitParameterGroup Is Nothing Then Me._FitParameterGroup = New cFitParameterGroupGroup
        If Not Me._FitParameterGroup.ContainsKey(Me.FitParameters.Identifier) Then
            Me._FitParameterGroup = New cFitParameterGroupGroup
            Me._FitParameterGroup.Add(Me.FitParameters, Me.FitFunctionName)
        End If
        Return Me._FitParameterGroup
    End Function

    ''' <summary>
    ''' Returns Me.FitParameters, grouped into a separate group,
    ''' but without a possibly existing combined group (multiple fits).
    ''' </summary>
    Public Overridable Function FitParametersGroupedWithoutCombinedGroup() As cFitParameterGroupGroup Implements iFitFunction.FitParametersGroupedWithoutCombinedGroup
        If Me._FitParameterGroup Is Nothing Then Me._FitParameterGroup = New cFitParameterGroupGroup
        If Not Me._FitParameterGroup.ContainsKey(Me.FitParameters.Identifier) Then Me._FitParameterGroup.Add(Me.FitParameters, Me.FitFunctionName)
        Return Me._FitParameterGroup
    End Function

    ''' <summary>
    ''' Sub-Method, that initializes all Fit-Parameters of a FitFunction.
    ''' </summary>
    Protected MustOverride Sub InitializeFitParameters() Implements iFitFunction.InitializeFitParameters

    ''' <summary>
    ''' Functions may override this, if they need the properties!
    ''' 
    ''' Changes the range in which the fit-function is defined.
    ''' Needed for convolution integrals and current integrals to estimate
    ''' the range of values to calculate the integral over.
    ''' </summary>
    Public Overridable Sub ChangeFitRangeX(LowerValue As Double, HigherValue As Double) Implements iFitFunction.ChangeFitRangeX
        ' Do nothing normally
    End Sub

#End Region

#Region "Constructor that calls the Initialisation of the FitParameters"
    ''' <summary>
    ''' Contructor of the FitFunction
    ''' </summary>
    Public Sub New()
        Me.InitializeFitParameters()
        Me.RegisterDataGenerationFunction()
    End Sub
#End Region

#Region "Get Y Value Function"
    ''' <summary>
    ''' Returns the y value of the function for
    ''' the given x and vector of parameters
    ''' </summary>
    ''' <param name="x">The <i>x</i>-value for which the <i>y</i>-value is calculated.</param>
    Public MustOverride Function GetY(ByRef x As Double, ByRef Parameters As cFitParameterGroupGroup) As Double Implements iFitFunction.GetY

    ''' <summary>
    ''' Returns the y value of the function for
    ''' the given x and the currently set of fitting parameters.
    ''' </summary>
    ''' <param name="x">The <i>x</i>-value for which the <i>y</i>-value is calculated.</param>
    Public Function GetY(ByRef x As Double) As Double
        Return Me.GetY(x, Me.FitParametersGrouped)
    End Function
#End Region

#Region "Fit-Settings-Panel"

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    MustOverride ReadOnly Property FunctionSettingPanel As cFitSettingPanel

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Function GetFunctionSettingPanel() As cFitSettingPanel Implements iFitFunction.GetFunctionSettingPanel

        Dim SettingPanel As cFitSettingPanel = Me.FunctionSettingPanel

        ' Set the FitFunction
        SettingPanel.InitializeFitFunction(Me)

        Return SettingPanel
    End Function

#End Region

#Region "Data Generation by given X columns"

    ''' <summary>
    ''' List with additional data-generation functions to be used for generating the final
    ''' output of the data, that is saved back to the spectroscopy table.
    ''' </summary>
    Public Property AdditionalDataGenerationFunctions As New Dictionary(Of String, iFitFunction._GetY) Implements iFitFunction.AdditionalDataGenerationFunctions

    ''' <summary>
    ''' Does nothing by default. Is called in the constructor to register
    ''' addition data output generation function-delegates.
    ''' </summary>
    Public Overridable Sub RegisterDataGenerationFunction()
        ' Do nothing by default.
    End Sub

    ''' <summary>
    ''' Returns an array of y(x) values, using the given x values and fitting parameters.
    ''' </summary>
    Public Function GenerateData(ByRef FitParameters As cFitParameterGroupGroup,
                                 ByRef xValues As Double(),
                                 Optional ByRef GetYDelegate As iFitFunction._GetY = Nothing) As Double() Implements iFitFunction.GenerateData

        ' Set the function for data-generation
        If GetYDelegate Is Nothing Then GetYDelegate = New iFitFunction._GetY(AddressOf Me.GetY)

        ' Set the same length of values
        Dim yValues As Double() = New Double(xValues.Length - 1) {}

        ' Get the Y-Value for all X-Values
        For i As Integer = 0 To xValues.Length - 1 Step 1
            If Double.IsNaN(xValues(i)) Then
                yValues(i) = Double.NaN
            Else
                yValues(i) = GetYDelegate(xValues(i), FitParameters)
            End If
        Next

        Return yValues
    End Function

    ''' <summary>
    ''' Returns an array of y(x) values, using the given x values and the
    ''' current internal set of fitting parameters.
    ''' </summary>
    ''' <param name="xValues">x values</param>
    ''' <returns>point values</returns>
    Public Function GenerateData(ByRef xValues As Double(),
                                 Optional ByRef GetYDelegate As iFitFunction._GetY = Nothing) As Double() Implements iFitFunction.GenerateData
        Return Me.GenerateData(Me.FitParametersGrouped, xValues, GetYDelegate)
    End Function

    ''' <summary>
    ''' Returns a list of y(x) values, using the given x values and fitting parameters.
    ''' </summary>
    ''' <param name="xValues">x values</param>
    ''' <param name="FitParameters">fitting parameters</param>
    ''' <returns>point values</returns>
    Public Function GenerateData(ByRef FitParameters As cFitParameterGroupGroup,
                                 ByRef xValues As ICollection(Of Double),
                                 Optional ByRef GetYDelegate As iFitFunction._GetY = Nothing) As List(Of Double) Implements iFitFunction.GenerateData

        ' Set the function for data-generation
        If GetYDelegate Is Nothing Then GetYDelegate = New iFitFunction._GetY(AddressOf Me.GetY)

        Return Me.GenerateData(FitParameters, xValues.ToArray, GetYDelegate).ToList
    End Function

    ''' <summary>
    ''' Returns a list of y(x) values, using the given x values and fitting parameters.
    ''' </summary>
    ''' <param name="xValues">x values</param>
    ''' <param name="FitParameters">fitting parameters</param>
    ''' <returns>point values</returns>
    Public Function GenerateData(ByRef xValues As ICollection(Of Double),
                                  Optional ByRef GetYDelegate As iFitFunction._GetY = Nothing) As List(Of Double) Implements iFitFunction.GenerateData
        ' Set the function for data-generation
        If GetYDelegate Is Nothing Then GetYDelegate = New iFitFunction._GetY(AddressOf Me.GetY)

        Return Me.GenerateData(Me.FitParametersGrouped, xValues.ToArray, GetYDelegate).ToList
    End Function
#End Region

#Region "Export / Import"

    ''' <summary>
    ''' For exporting and importing a bunch of fit-models,
    ''' each settings-panel should have the ability to generate
    ''' and interpret XML data for the individual parameters.
    ''' THIS FUNCTION CAN BE OVERRIDEN BY THE CHILD PANEL
    ''' </summary>
    Public Overridable Function ExportXML(FileName As String) As Boolean Implements iFitFunction.ExportXML

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
                .WriteStartElement("Properties")

                ' First element of fit function
                .WriteStartElement("FitFunction") ' <FitFunction
                .WriteAttributeString("ClassName", Me.GetType.ToString)
                .WriteAttributeString("FitFunctionName", Me.FitFunctionName)
                .WriteEndElement() ' FitFunction /> 

                ' Close the Section
                .WriteEndElement()

                ' Write Fit-Parameters
                .WriteRaw(Me.FitParameters.ExportXML(False))

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
    Public Overridable Function ImportXML(FileStream As IO.Stream) As Boolean Implements iFitFunction.ImportXML

        ' Go to the beginning of the stream, if possible.
        If FileStream.CanSeek Then FileStream.Seek(0, IO.SeekOrigin.Begin)

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
                                    If .AttributeCount > 0 Then
                                        While .MoveToNextAttribute
                                            Select Case .Name
                                                Case "Identifier"
                                                    'Me.Identifier = .Value
                                                Case "ClassName"
                                                    If .Value <> Me.GetType.ToString Then
                                                        Throw New Exception("Fit-model mismatch! The fit-model to import is not of the same type!")
                                                    End If
                                                Case "FitFunctionName"
                                                    'If .Value <> Me.FitFunctionName Then
                                                    '    Throw New Exception("Fit-model mismatch! The fit-model to import is not of the same type!")
                                                    'End If
                                            End Select
                                        End While
                                    End If

                                Case Else

                                    ' Try to call other import-methods from inherited classes
                                    Me.Import_UnknownXMLElementIdentified(.Name, XMLReader)

                            End Select

                    End Select
                Loop
            End With

            ' Import Parameters
            Me.FitParameters.ImportXML(FileStream, AddressOf Me.Import_ParameterIdentified)

#If Not Debug Then
            Catch ex As Exception
            MessageBox.Show("Error importing a Fit-Model:" & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
#End If
        Finally
            ' Release stream resources
            FileStream.Close()
        End Try
        Return True
    End Function

    ''' <summary>
    ''' This function does nothing, if it is not overridden.
    ''' It gets called after each successfull identification of
    ''' a FitParameter during the import-routine!
    ''' By this individual FitParameter-treatment can be handeled,
    ''' without ne need to override the Import-Function!
    ''' </summary>
    Protected Overridable Sub Import_ParameterIdentified(ByVal Identifier As String, ByRef Parameter As cFitParameter)
        ' Do nothing, if not overridden.
    End Sub

    ''' <summary>
    ''' This function does nothing, if it is not overridden.
    ''' It gets called after the import has finished!
    ''' </summary>
    Protected Overridable Sub Import_Finished()
        ' Do nothing, if not overridden.
    End Sub

    ''' <summary>
    ''' This function does nothing. Override it! 
    ''' It gets called, if the import of XML discovered an unknown Element.
    ''' Just read as far as necessary for the import! Document will get closed automatically.
    ''' </summary>
    Protected Overridable Sub Import_UnknownXMLElementIdentified(ByVal XMLElementName As String, ByRef XMLReader As Xml.XmlReader)
        ' Do Nothing!
    End Sub

    ''' <summary>
    ''' This function does nothing. Override it! 
    ''' It gets called in the end of the export. Use it to add additional settings to the fit-model.
    ''' 
    ''' Don't close the writer, or leave open elements. This will break the XML-file.
    ''' </summary>
    Protected Overridable Sub Export_WriteAdditionalXMLElements(ByRef XMLWriter As Xml.XmlTextWriter)
        ' Do Nothing!
    End Sub

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

#End Region

#Region "Fit-Function Plugin Interface"

    ''' <summary>
    ''' Returns a cFitFunction, depending on the identifier, or Nothing, if the
    ''' FitFunction was not recognized.
    ''' </summary>
    Public Shared Function GetFitFunctionByType(FitFunctionType As Type) As iFitFunction
        ' go through all fit-functions, and look for the
        ' right one to load.
        For Each RegisteredType As Type In GetAllLoadableFitFunctions()
            If RegisteredType Is FitFunctionType Then
                Return CType(Activator.CreateInstance(FitFunctionType), iFitFunction)
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns a list with all FitFunction routines implemented in the program or in loaded plugins.
    ''' </summary>
    Public Shared Function GetAllLoadableFitFunctions() As List(Of Type)
        Dim APIList As New List(Of Type)

        Try
            ' fill the list of with the interfaces found.
            With APIList
                Dim APIType = GetType(iFitFunction)
                Dim AllAPIImplementingInterfaces As IEnumerable(Of Type) = AppDomain.CurrentDomain.GetAssemblies() _
                                                                       .SelectMany(Function(s) s.GetTypes()) _
                                                                       .Where(Function(p) APIType.IsAssignableFrom(p) And p.IsClass And Not p.IsAbstract)
                'For Each ImplementingType As Type In AllAPIImplementingInterfaces
                '.Add(DirectCast(System.Activator.CreateInstance(ImplementingType), iFileImport_SpectroscopyTable))
                'Next
                APIList = AllAPIImplementingInterfaces.ToList
            End With
        Catch ex As Exception
            Trace.WriteLine("#ERROR: cFitFunction.GetAllLoadableFitFunctions: Error on loading: " & ex.Message)
        End Try

        Return APIList
    End Function

#End Region

End Class
