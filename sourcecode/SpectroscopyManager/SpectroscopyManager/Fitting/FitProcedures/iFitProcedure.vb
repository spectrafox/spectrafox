Imports System.ComponentModel

Public Interface iFitProcedure

#Region "Fit-Procedure-Properties"

    ''' <summary>
    ''' Name of the Fit-Procedure
    ''' </summary>
    ReadOnly Property Name As String

    ''' <summary>
    ''' List with specific settings, specialized for the specific fit procedure.
    ''' </summary>
    Property FitProcedureSettings As iFitProcedureSettings

#End Region

#Region "Stop-Reasons"

    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String

#End Region

#Region "Fitting and Aborting"

    ''' <summary>
    ''' Initializes the FitProcedure with all its parameters.
    ''' The initial parameters are given in the iFitFunction.FitParameters
    ''' </summary>
    Sub FitInitializer(ByRef ModelFitFunction As iFitFunction,
                       ByRef FitDataPoints As Double()(),
                       ByRef Weights As Double())

    ''' <summary>
    ''' Step of the Fit-Procedure.
    ''' </summary>
    Function FitStep(ByRef FitDataPoints As Double()(),
                     ByRef Weights As Double(),
                     ByRef FitParameterGroups As cFitParameterGroupGroup,
                     ByRef Iteration As Integer,
                     ByRef StopReason As Integer) As Double

    ''' <summary>
    ''' Finalizes the FitProcedure with all its parameters.
    ''' </summary>
    Function FitFinalizer(ByRef FitDataPoints As Double()(),
                          ByRef Weights As Double(),
                          ByRef FitParameterGroups As cFitParameterGroupGroup) As Boolean

#End Region

#Region "Fit-Procedure-Panel"

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    ReadOnly Property ProcedureSettingPanel As cFitProcedureSettingsPanel

#End Region

End Interface
