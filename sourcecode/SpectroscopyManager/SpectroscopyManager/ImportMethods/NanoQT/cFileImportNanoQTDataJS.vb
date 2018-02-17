Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportNanoQTDataJS
    Inherits cFileImportNanoQTData

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public Overrides ReadOnly Property FileExtension As String
        Get
            Return ".js"
        End Get
    End Property

End Class
