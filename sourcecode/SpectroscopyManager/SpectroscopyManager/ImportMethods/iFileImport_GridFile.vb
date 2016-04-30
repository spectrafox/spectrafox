Public Interface iFileImport_GridFile

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
    ''' Import routine.
    ''' </summary>
    Function ImportGridFile(ByRef FullFileNamePlusPath As String,
                            ByVal FetchOnlyFileHeader As Boolean,
                            Optional ByRef ReaderBuffer As String = "",
                            Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                            Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cGridFile


End Interface
