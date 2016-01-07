Public Interface iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Extension, by which the file is identified.
    ''' </summary>
    ReadOnly Property FileExtension As String

    ''' <summary>
    ''' This function checks the file for characteristic content.
    ''' If the file can be imported, using this API, it should return true.
    ''' Else False.
    ''' </summary>
    Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                          Optional ByRef ReaderBuffer As String = "") As Boolean

    ''' <summary>
    ''' Import routine
    ''' </summary>
    Function ImportSpectroscopyTable(ByRef FullFileNamePlusPath As String,
                                     ByVal FetchOnlyFileHeader As Boolean,
                                     Optional ByRef ReaderBuffer As String = "") As cSpectroscopyTable

End Interface
