Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportNanoQTDataJSON
    Inherits cFileImportNanoQTData

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public Overrides ReadOnly Property FileExtension As String
        Get
            Return ".json"
        End Get
    End Property

End Class
