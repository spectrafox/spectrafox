''' <summary>
''' This is an interface class to implement Functions that a Class
''' has to fullfill, after a SpectroscopyTable has been loaded. It is then
''' passed to the implementing class. 
''' </summary>
Public Interface iSingleSpectroscopyTableLoaded

    ''' <summary>
    ''' Called, when the fetching routine has completed!
    ''' </summary>
    Sub SpectroscopyTableLoaded(ByRef SpectroscopyTable As cSpectroscopyTable)

End Interface
