Imports System
Imports System.IO
Imports System.Reflection

''' <summary>
''' Summary description for PluginServices.
''' </summary>
Public Class PluginServices
    Implements iPluginHost

    ''' <summary>
    ''' Constructor of the Class
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    Private colAvailablePlugins As AvailablePlugins = New AvailablePlugins

    ''' <summary>
    ''' A Collection of all Plugins Found and Loaded by the FindPlugins() Method
    ''' </summary>
    Public Property AvailablePlugins As AvailablePlugins
        Get
            Return colAvailablePlugins
        End Get
        Set(value As AvailablePlugins)
            colAvailablePlugins = value
        End Set
    End Property

    ''' <summary>
    ''' Searches the Application's Startup Directory for Plugins
    ''' </summary>
    Public Overloads Sub FindPlugins()
        FindPlugins(AppDomain.CurrentDomain.BaseDirectory & Path.DirectorySeparatorChar & "plugins")
    End Sub

    ''' <summary>
    ''' Searches the passed Path for Plugins
    ''' </summary>
    ''' <param name="Path">Directory to search for Plugins in</param>
    Public Overloads Sub FindPlugins(ByVal Path As String)
        ' First empty the collection, we're reloading them all
        colAvailablePlugins.Clear()
        Try
            ' Go through all the files in the plugin directory
            For Each fileOn As String In Directory.GetFiles(Path)
                Dim file As FileInfo = New FileInfo(fileOn)
                'Preliminary check, must be .dll
                If file.Extension.Equals(".dll") Then
                    'Add the 'plugin'
                    Me.AddPlugin(fileOn)
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine("Plugin Load Error: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Unloads and Closes all AvailablePlugins
    ''' </summary>
    Public Sub ClosePlugins()
        For Each pluginOn As AvailablePlugin In colAvailablePlugins
            'Close all plugin instances
            'We call the plugins Dispose sub first incase it has to do 
            'Its own cleanup stuff
            pluginOn.Instance.Dispose()
            'After we give the plugin a chance to tidy up, get rid of it
            pluginOn.Instance = Nothing
        Next
        'Finally, clear our collection of available plugins
        colAvailablePlugins.Clear()
    End Sub

    Private Sub AddPlugin(ByVal FileName As String)
        'Create a new assembly from the plugin file we're adding..
        Dim pluginAssembly As Assembly = Assembly.LoadFrom(FileName)
        'Next we'll loop through all the Types found in the assembly
        For Each pluginType As Type In pluginAssembly.GetTypes
            If pluginType.IsPublic Then
                If Not pluginType.IsAbstract Then
                    'Gets a type object of the interface we need the plugins to match
                    Dim typeInterface As Type = pluginType.GetInterface(GetType(iPluginInterface).ToString, True)
                    'Make sure the interface we want to use actually exists
                    If (Not (typeInterface) Is Nothing) Then
                        'Create a new available plugin since the type implements the IPlugin interface
                        Dim newPlugin As AvailablePlugin = New AvailablePlugin
                        'Set the filename where we found it
                        newPlugin.AssemblyPath = FileName
                        'Create a new instance and store the instance in the collection for later use
                        'We could change this later on to not load an instance.. we have 2 options
                        '1- Make one instance, and use it whenever we need it.. it's always there
                        '2- Don't make an instance, and instead make an instance whenever we use it, then close it
                        'For now we'll just make an instance of all the plugins
                        newPlugin.Instance = CType(Activator.CreateInstance(pluginAssembly.GetType(pluginType.ToString)), iPluginInterface)
                        'Set the Plugin's host to this class which inherited IPluginHost
                        newPlugin.Instance.PluginHost = Me
                        'Call the initialization sub of the plugin
                        newPlugin.Instance.Initialize()
                        'Add the new plugin to our collection here
                        Me.colAvailablePlugins.Add(newPlugin)
                        'cleanup a bit
                        newPlugin = Nothing
                    End If
                    typeInterface = Nothing
                    'Mr. Clean            
                End If
            End If
        Next
        pluginAssembly = Nothing
        'more cleanup
    End Sub

    ''' <summary>
    ''' Displays a feedback dialog from the plugin
    ''' </summary>
    ''' <param name="Feedback">String message for feedback</param>
    ''' <param name="Plugin">The plugin that called the feedback</param>
    Public Sub Feedback(ByVal Feedback As String, ByVal Plugin As iPluginInterface)
        'This sub makes a new feedback form and fills it out
        'With the appropriate information
        'This method can be called from the actual plugin with its Host Property
        Dim newForm As System.Windows.Forms.Form = Nothing
        Dim newFeedbackForm As New wPluginDetails
        'Here we set the frmFeedback's properties that i made custom
        newFeedbackForm.PluginAuthor = ("By: " + Plugin.Author)
        newFeedbackForm.PluginDesc = Plugin.Description
        newFeedbackForm.PluginName = Plugin.Name
        newFeedbackForm.PluginVersion = Plugin.Version
        newFeedbackForm.Feedback = Feedback
        'We also made a Form object to hold the frmFeedback instance
        'If we were to declare if not as  frmFeedback object at first,
        'We wouldn't have access to the properties we need on it
        newForm = newFeedbackForm
        newForm.ShowDialog()
        newFeedbackForm = Nothing
        newForm = Nothing
    End Sub
End Class

''' <summary>
''' Collection for AvailablePlugin Type
''' </summary>
Public Class AvailablePlugins
    Inherits System.Collections.CollectionBase

    'A Simple Home-brew class to hold some info about our Available Plugins
    ''' <summary>
    ''' Add a Plugin to the collection of Available plugins
    ''' </summary>
    ''' <param name="pluginToAdd">The Plugin to Add</param>
    Public Sub Add(ByVal pluginToAdd As AvailablePlugin)
        Me.List.Add(pluginToAdd)
    End Sub

    ''' <summary>
    ''' Remove a Plugin to the collection of Available plugins
    ''' </summary>
    ''' <param name="pluginToRemove">The Plugin to Remove</param>
    Public Sub Remove(ByVal pluginToRemove As AvailablePlugin)
        Me.List.Remove(pluginToRemove)
    End Sub

    ''' <summary>
    ''' Finds a plugin in the available Plugins
    ''' </summary>
    ''' <param name="pluginNameOrPath">The name or File path of the plugin to find</param>
    ''' <returns>Available Plugin, or null if the plugin is not found</returns>
    Public Function Find(ByVal pluginNameOrPath As String) As AvailablePlugin
        Dim toReturn As AvailablePlugin = Nothing
        'Loop through all the plugins
        For Each pluginOn As AvailablePlugin In Me.List
            'Find the one with the matching name or filename
            If (pluginOn.Instance.Name.Equals(pluginNameOrPath) OrElse pluginOn.AssemblyPath.Equals(pluginNameOrPath)) Then
                toReturn = pluginOn
                Exit For
            End If
        Next
        Return toReturn
    End Function

    ''' <summary>
    ''' Tries to get the type of a plugin from the typename as string.
    ''' </summary>
    Public Overloads Shared Function [GetType](typeName As String) As Type
        Dim type__1 = Type.[GetType](typeName)
        If type__1 IsNot Nothing Then
            Return type__1
        End If
        For Each a As Assembly In AppDomain.CurrentDomain.GetAssemblies()
            type__1 = a.[GetType](typeName)
            If type__1 IsNot Nothing Then
                Return type__1
            End If
        Next
        Return Nothing
    End Function
End Class

''' <summary>
''' Data Class for Available Plugin.  Holds and instance of the loaded Plugin, as well as the Plugin's Assembly Path
''' </summary>
Public Class AvailablePlugin

    'This is the actual AvailablePlugin object.. 
    'Holds an instance of the plugin to access
    'ALso holds assembly path... not really necessary
    Private myInstance As iPluginInterface = Nothing

    Private myAssemblyPath As String = ""

    Public Property Instance As iPluginInterface
        Get
            Return myInstance
        End Get
        Set(value As iPluginInterface)
            myInstance = value
        End Set
    End Property

    Public Property AssemblyPath As String
        Get
            Return myAssemblyPath
        End Get
        Set(value As String)
            myAssemblyPath = value
        End Set
    End Property
End Class