''' <summary>
''' Class for hosting global objects, to be accessible from other classes,
''' such as plugin-management, etc.
''' </summary>
Public Class cGlobal

    ''' <summary>
    ''' Plugin-Container
    ''' </summary>
    Public Shared Plugins As New PluginServices

    ''' <summary>
    ''' Global function to be called for saving the settings.
    ''' Catches any errors due to changed settings files.
    ''' </summary>
    Public Shared Sub SaveSettings()

        Try
            My.Settings.Save()
        Catch ex As Exception
            Debug.WriteLine("Error on saving settings: " & ex.Message)
        End Try

    End Sub

End Class
