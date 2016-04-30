''' <summary>
''' This is an interface class to implement Functions that a Class
''' has to fullfill, after a ScanImage has been loaded. It is then
''' passed to the implementing class. 
''' </summary>
Public Interface iSingleScanImageLoaded

    ''' <summary>
    ''' Called, when the fetching routine has completed!
    ''' </summary>
    Sub ScanImageLoaded(ByRef ScanImage As cScanImage)

End Interface
