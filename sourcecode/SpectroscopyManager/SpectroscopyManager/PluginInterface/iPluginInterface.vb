Imports System

Public Interface iPluginInterface

    ''' <summary>
    ''' The plugin's host object.
    ''' </summary>
    Property PluginHost As iPluginHost

    ''' <summary>
    ''' Enter the name of the plug-in as listed in the plug-in window.
    ''' </summary>
    ReadOnly Property Name As String

    ''' <summary>
    ''' Retrun a description of you plugin.
    ''' </summary>
    ReadOnly Property Description As String

    ''' <summary>
    ''' Return the plugin's author's name.
    ''' </summary>
    ReadOnly Property Author As String

    ''' <summary>
    ''' Return the version of you plugin.
    ''' </summary>
    ReadOnly Property Version As String

    ''' <summary>
    ''' Implement a constructor.
    ''' </summary>
    Sub Initialize()

    ''' <summary>
    ''' Implement a destructor.
    ''' </summary>
    Sub Dispose()

End Interface

Public Interface iPluginHost

End Interface