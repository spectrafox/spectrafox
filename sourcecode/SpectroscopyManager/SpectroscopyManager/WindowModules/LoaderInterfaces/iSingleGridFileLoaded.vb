''' <summary>
''' This is an interface class to implement functions that a class
''' has to fullfill, after a GridFile has been loaded. It is then
''' passed to the implementing class. 
''' </summary>
Public Interface iSingleGridFileLoaded

    ''' <summary>
    ''' Called, when the fetching routine has completed!
    ''' </summary>
    Sub GridFileLoaded(ByRef GridFile As cGridFile)

End Interface
