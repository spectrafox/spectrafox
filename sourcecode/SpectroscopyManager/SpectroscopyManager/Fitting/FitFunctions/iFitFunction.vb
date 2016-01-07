Imports System.Threading.Tasks

''' <summary>
''' Interface to be implemented by a fit-function!
''' This interface is called by the fit-procedures, such as the
''' Levenberg-Marquardt algorithm.
''' </summary>
Public Interface iFitFunction

#Region "technical Fit-Function properties"

    ''' <summary>
    ''' This property must be implemented to let the fit-procedure decide
    ''' if to use CUDA to accelerate the fit-procedure or not!
    ''' </summary>
    ReadOnly Property FunctionImplementsCUDAVersion As Boolean

    ''' <summary>
    ''' This property must be implemented as it is written by the fit-procedure to
    ''' tell the fit-function to use it's CUDA implementation.
    ''' </summary>
    Property UseCUDAVersion As Boolean

    ''' <summary>
    ''' ParallelOptions used by the Multi-Threading Tasks
    ''' implemented in this library. Use this to
    ''' limit the number of max concurrent threads.
    ''' Set .MaxDegreeOfParallelism to -1, to set no limit the the number of parallel threads.
    ''' </summary>
    Property MultiThreadingOptions As ParallelOptions

#End Region

#Region "plausibility check of initialization parameters"

    ''' <summary>
    ''' When the fit-function gets loaded, the fitrange can be checked, so that the function does not
    ''' run forever, if the range is e.g. too large for convolution integrals that treat data in the mV range.
    ''' 
    ''' The function should modify the input values to the ranges the function accepts by default.
    ''' It should return false, if the values were modified.
    ''' </summary>
    Function FitFunctionSuggestsDifferentFitRange(ByRef FitRangeLower As Double, ByRef FitRangeUpper As Double) As Boolean

#End Region

#Region "Fit-Parameters"
    ''' <summary>
    ''' This Property is an array of all Fit-Parameters.
    ''' </summary>
    Property FitParameters() As cFitParameterGroup

    ''' <summary>
    ''' This Property is an array of all Fit-Parameter-Groups in this fit-function.
    ''' If this is not a single fit function, then the group will contain more than one
    ''' FitParameterGroup, otherwise it contains just one.
    ''' </summary>
    Function FitParametersGrouped() As cFitParameterGroupGroup

    ''' <summary>
    ''' Returns Me.FitParameters, grouped into a separate group,
    ''' but without a possibly existing combined group (multiple fits).
    ''' </summary>
    Function FitParametersGroupedWithoutCombinedGroup() As cFitParameterGroupGroup

    ''' <summary>
    ''' Returns the GUID of the FitParametersGroup, that is explicitly used for this FitFunction
    ''' </summary>
    ReadOnly Property UseFitParameterGroupID As Guid

    ''' <summary>
    ''' Sub-Method, that initializes all Fit-Parameters of a FitFunction.
    ''' Should be called in the Constructor.
    ''' </summary>
    Sub InitializeFitParameters()

    ''' <summary>
    ''' Changes the range in which the fit-function is defined.
    ''' Needed for convolution integrals and current integrals to estimate
    ''' the range of values to calculate the integral over.
    ''' </summary>
    Sub ChangeFitRangeX(ByVal LowerValue As Double, ByVal HigherValue As Double)
#End Region

#Region "Fit-Function methods"
    ''' <summary>
    ''' Returns the y value of the function for
    ''' the given x and vector of parameters
    ''' </summary>
    ''' <param name="x">The <i>x</i>-value for which the <i>y</i>-value is calculated.</param>
    Function GetY(ByRef x As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double

    ''' <summary>
    ''' Delegate function representing a function expecting a fit-parameter array.
    ''' </summary>
    Delegate Function _GetY(ByRef x As Double, ByRef InputParameters As cFitParameterGroupGroup) As Double
#End Region

#Region "Fit-Description"
    ''' <summary>
    ''' This method should link to a resource and return back
    ''' a name of the Fit-Function.
    ''' </summary>
    Function FitFunctionName() As String

    ''' <summary>
    ''' This method should link to a resource and return back
    ''' a description of the Fit-Function.
    ''' </summary>
    Function FitDescription() As String

    ''' <summary>
    ''' This method should link to a resource and return back
    ''' the formula that is used as Fit-Function.
    ''' </summary>
    Function FitFunctionFormula() As String

    ''' <summary>
    ''' This method should link to a resource and return back
    ''' the Authors and References that were used for the Fit-Function.
    ''' </summary>
    Function FitFunctionAuthors() As String
#End Region

#Region "Fit-Settings-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Function GetFunctionSettingPanel() As cFitSettingPanel
#End Region

#Region "Generate Data using Fit-Function"

    ''' <summary>
    ''' List with additional data-generation functions to be used for generating the final
    ''' output of the data, that is saved back to the spectroscopy table.
    ''' </summary>
    Property AdditionalDataGenerationFunctions As Dictionary(Of String, _GetY)

    ''' <summary>
    ''' Returns an array of y(x) values, using the given x values and the
    ''' current internal set of fitting parameters.
    ''' </summary>
    ''' <param name="xValues">x values</param>
    ''' <returns>point values</returns>
    Function GenerateData(ByRef xValues As Double(),
                          Optional ByRef GetYDelegate As _GetY = Nothing) As Double()

    ''' <summary>
    ''' Returns an array of y(x) values, using the given x values and fitting parameters.
    ''' </summary>
    ''' <param name="xValues">x values</param>
    ''' <param name="FitParameters">fitting parameters</param>
    ''' <returns>point values</returns>
    Function GenerateData(ByRef FitParameters As cFitParameterGroupGroup,
                          ByRef xValues As Double(),
                          Optional ByRef GetYDelegate As _GetY = Nothing) As Double()

    ''' <summary>
    ''' Returns a list of y(x) values, using the given x values and fitting parameters.
    ''' </summary>
    ''' <param name="xValues">x values</param>
    ''' <param name="FitParameters">fitting parameters</param>
    ''' <returns>point values</returns>
    Function GenerateData(ByRef FitParameters As cFitParameterGroupGroup,
                          ByRef xValues As ICollection(Of Double),
                          Optional ByRef GetYDelegate As _GetY = Nothing) As List(Of Double)

    ''' <summary>
    ''' Returns a List of y(x) values, using the given x values and the
    ''' current internal set of fitting parameters.
    ''' </summary>
    ''' <param name="xValues">x values</param>
    ''' <returns>point values</returns>
    Function GenerateData(ByRef xValues As ICollection(Of Double),
                          Optional ByRef GetYDelegate As _GetY = Nothing) As List(Of Double)

#End Region

#Region "Import-Export"

    ''' <summary>
    ''' Import Parameters for this FitModel using XML.
    ''' </summary>
    Function ImportXML(FileStream As IO.Stream) As Boolean

    ''' <summary>
    ''' This function does nothing, if it is not overridden.
    ''' It gets called after each successfull identification of
    ''' a FitParameter during the import-routine!
    ''' By this individual FitParameter-treatment can be handeled,
    ''' without ne need to override the Import-Function!
    ''' </summary>
    Delegate Sub Import_ParameterIdentified(ByVal Identifier As String, ByRef Parameter As cFitParameter)

    ''' <summary>
    ''' Export Parameters from this FitModel as XML
    ''' </summary>
    Function ExportXML(FileName As String) As Boolean

#End Region

End Interface
