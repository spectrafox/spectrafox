''' <summary>
''' This is an interface class to implement Functions that a Class
''' has to fullfill, after a ScanImage has been loaded. It is then
''' passed to the implementing class. 
''' </summary>
Public Interface iMultipleScanImagesLoaded

    ''' <summary>
    ''' Called, when the fetching routine has completed!
    ''' </summary>
    Sub AllScanImagesLoaded(ByRef ScanImageList As List(Of cScanImage))

End Interface
