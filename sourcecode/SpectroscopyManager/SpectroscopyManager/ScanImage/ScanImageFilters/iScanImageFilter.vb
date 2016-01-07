Public Interface iScanImageFilter

    ''' <summary>
    ''' Property to be set, which determines, if the filter has been set up correctly.
    ''' </summary>
    Property FilterSetupComplete As Boolean

    ''' <summary>
    ''' In this property the execution time the filter needs to be calculated
    ''' is stored. Allows the user to decide, whether to dynamically calculate the
    ''' filter each time, or to store it as a separate scan-channel.
    ''' </summary>
    Property FilterExecutionTime As TimeSpan

    ''' <summary>
    ''' Set to determine, if the filter will be shown in the filter list to the user.
    ''' </summary>
    ReadOnly Property ShowFilterInFilterMenu As Boolean

    ''' <summary>
    ''' Name of the filter, as shown in the GUI to the user.
    ''' </summary>
    ReadOnly Property FilterName As String

    ''' <summary>
    ''' Determines, if this filter needs to be setup in advance of using it.
    ''' </summary>
    ReadOnly Property NeedsSetup As Boolean

    ''' <summary>
    ''' Filter Function
    ''' </summary>
    Function ApplyFilter(ByRef ScanChannel As cScanImage.ScanChannel) As cScanImage.ScanChannel

    ''' <summary>
    ''' Settings of the filter given as a separate string.
    ''' If set, should use the given settings for the new filter.
    ''' If get, returns the current filter settings as string.
    ''' </summary>
    Property FilterSettingsString() As String

End Interface
