Public Class cFileImportGenericCSV
    Inherits cFileImportGeneric
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public Overloads ReadOnly Property FileExtension As String Implements iFileImport_SpectroscopyTable.FileExtension
        Get
            Return ".csv"
        End Get
    End Property
End Class
