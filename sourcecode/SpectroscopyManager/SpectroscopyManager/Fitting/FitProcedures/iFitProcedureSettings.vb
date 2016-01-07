''' <summary>
''' Determines the Setting of a Fit-Procedure
''' </summary>
Public Interface iFitProcedureSettings

    ''' <summary>
    ''' Get a reference to the base-class type.
    ''' </summary>
    ReadOnly Property BaseClass As Type

    ''' <summary>
    ''' Get the output to be shown in the fit-initialization log window.
    ''' </summary>
    Function EchoSettings() As String

    ''' <summary>
    ''' Stop Conditions for the fitting.
    ''' (Always needed.)
    ''' </summary>
    Property StopCondition_MaxIterations As Integer

    ''' <summary>
    ''' Stop Conditions for the fitting.
    ''' (Always needed.)
    ''' </summary>
    Property StopCondition_MinChi2Change As Double

End Interface
