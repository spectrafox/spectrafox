''' <summary>
''' This is an interface class to implement Functions that a Class
''' has to fullfill, after a SpectroscopyTable has been loaded. It is then
''' passed to the implementing class. 
''' </summary>
Public Interface iMultipleSpectroscopyTablesLoaded

    ''' <summary>
    ''' Called, when the fetching routine has completed!
    ''' </summary>
    Sub AllSpectroscopyTablesLoaded(ByRef SpectroscopyTables As List(Of cSpectroscopyTable))

End Interface
