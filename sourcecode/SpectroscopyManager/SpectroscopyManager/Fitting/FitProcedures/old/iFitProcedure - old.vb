Imports System.ComponentModel

Public Interface iFitProcedure

#Region "Fit-Procedure-Properties"
    ''' <summary>
    ''' Name of the Fit-Procedure
    ''' </summary>
    ReadOnly Property Name As String

    ''' <summary>
    ''' Returns the number of iterations used for the fit.
    ''' </summary>
    ReadOnly Property Iterations() As Integer

    ''' <summary>
    ''' Returns the parameter set of the current Fit.
    ''' </summary>
    ReadOnly Property FitParameters() As Dictionary(Of Integer, sFitParameter)

    ''' <summary>
    ''' Saves the statistics of the data, that is calculated after the fit.
    ''' </summary>
    ReadOnly Property Statistics As cNumericalMethods.sNumericStatistics

    ''' <summary>
    ''' List with specific settings, specialized for the specific fit procedure.
    ''' </summary>
    Property FitProcedureSettings As iFitProcedureSettings

#End Region

#Region "Threads"
    ''' <summary>
    ''' Single Thread used for Async fitting .
    ''' </summary>
    ReadOnly Property FitThreadWorker As BackgroundWorker
#End Region

#Region "Events"

    ''' <summary>
    ''' Echos the current parameter set, whenever
    ''' the parameters got changed to a minimized Chi2. 
    ''' </summary>
    Event FitStepEcho(ByVal Parameters As Dictionary(Of Integer, sFitParameter),
                      ByVal Chi2 As Double)

    ''' <summary>
    ''' Echos the current parameter set, whenever
    ''' the parameters got changed to a minimized Chi2. 
    ''' </summary>
    Event FitEcho(ByVal Message As String)

    ''' <summary>
    ''' Echoed, if the fit procedure has ended.
    ''' </summary>
    Event FitFinished(ByVal FitStopReason As Integer,
                      ByVal FinalParameters As Dictionary(Of Integer, sFitParameter),
                      ByVal Chi2 As Double)

    ''' <summary>
    ''' An event to listen to, if you want to show a progress bar for single steps.
    ''' </summary>
    Event CalculationStepProgress(ByVal CalcItem As Integer,
                                  ByVal CalcMax As Integer,
                                  ByVal StepDescription As String)

#End Region

#Region "Stop-Reasons"
    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String
#End Region

#Region "Fit-Duration"
    ''' <summary>
    ''' Duration of the currently running fit.
    ''' </summary>
    ReadOnly Property FitDuration As TimeSpan

    ''' <summary>
    ''' Time at which the Fit-Procedure started.
    ''' </summary>
    ReadOnly Property FitStartTime As Date

    ''' <summary>
    ''' Time at which the Fit-Procedure ended
    ''' </summary>
    ReadOnly Property FitEndTime As Date
#End Region

#Region "Fitting and Aborting"

    ''' <summary>
    ''' Aborts the Fit-Procedure, if the Fit is running in a separate Thread.
    ''' Otherwise, if the fit is a direct fit, we can't abort it anyway.
    ''' </summary>
    Sub AbortAsyncFit()

    ''' <summary>
    ''' Starts the fit in a background thread. If used after calling fit(lambda, minDeltaChi2, maxIterations),
    ''' uses those values. The stop condition is fetched from <code>this.stop()</code>.
    ''' Override <code>this.stop()</code> if you want to use another stop condition.
    ''' </summary>
    ''' <param name="ModelFitFunction">The model function to be fitted. The Fit-Parameter in the internal array are used as start-conditions.</param>
    ''' <param name="FitDataPoints">The data points in an array, <code>double[0 = x, 1 = y][point index]</code>. Size must be <code>double[2][N]</code>.</param>
    ''' <param name="Weights">The weights, normally given as: <code>weights[i] = 1 / sigma_i^2</code>. 
    ''' If you have a bad data point, set its weight to zero. If the given array is null,
    ''' a new array is created with all elements set to 1.</param>
    ''' <returns>True if new thread could be started. False, if thread was already running.</returns>
    Function FitAsync(ByVal ModelFitFunction As iFitFunction, ByVal FitDataPoints As Double()(), ByVal Weights As Double()) As Boolean

    ''' <summary>
    ''' The default fit. If used after calling fit(lambda, minDeltaChi2, maxIterations),
    ''' uses those values. The stop condition is fetched from <code>Me.StopCondition</code>.
    ''' Override <code>Me.StopCondition</code> if you want to use another stop condition.
    ''' <param name="ModelFitFunction">The model function to be fitted. The Fit-Parameter in the internal array are used as start-conditions.</param>
    ''' <param name="FitDataPoints">The data points in an array, <code>double[0 = x, 1 = y][point index]</code>. Size must be <code>double[2][N]</code>.</param>
    ''' <param name="Weights">The weights, normally given as: <code>weights[i] = 1 / sigma_i^2</code>. 
    ''' If you have a bad data point, set its weight to zero. If the given array is null,
    ''' a new array is created with all elements set to 1.</param>
    ''' </summary>
    Sub DirectFit(ByVal ModelFitFunction As iFitFunction, ByVal FitDataPoints As Double()(), ByVal Weights As Double())

#End Region

#Region "Fit-Procedure-Panel"
    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    ReadOnly Property ProcedureSettingPanel As cFitProcedureSettingsPanel
#End Region

End Interface
