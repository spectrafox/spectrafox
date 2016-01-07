''' <summary>
''' Interface to be implemented for Bug-Reporting.
''' </summary>
Public Interface iBugReporter(Of T)
    ''' <summary>
    ''' Reports some information to a specific location
    ''' </summary>
    Sub Submit(ByRef report As T)
End Interface
