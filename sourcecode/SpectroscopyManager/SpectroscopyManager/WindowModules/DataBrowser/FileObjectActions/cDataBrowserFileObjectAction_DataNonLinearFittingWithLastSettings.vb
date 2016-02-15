Public Class cDataBrowserFileObjectAction_DataNonLinearFittingWithLastSettings
    Implements iDataBrowser_FileObjectAction


#Region "Class abilities"
    ''' <summary>
    ''' This class can handle single file-objects.
    ''' </summary>
    Public ReadOnly Property CanHandleMultipleFileObjects As Boolean Implements iDataBrowser_FileObjectAction.CanHandleMultipleFileObjects
        Get
            Return True
        End Get
    End Property

    ''' <summary>
    ''' This class can handle single file-objects.
    ''' </summary>
    Public ReadOnly Property CanHandleSingleFileObjects As Boolean Implements iDataBrowser_FileObjectAction.CanHandleSingleFileObjects
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Handle spectroscopy files only.
    ''' </summary>
    Public ReadOnly Property CanHandleFileObjectType As cFileObject.FileTypes Implements iDataBrowser_FileObjectAction.CanHandleFileObjectType
        Get
            Return cFileObject.FileTypes.SpectroscopyTable
        End Get
    End Property
#End Region

#Region "Style and Settings"

    ''' <summary>
    ''' Return a quick-button with icon.
    ''' </summary>
    Public Function QuickButtonInListEntry() As Button Implements iDataBrowser_FileObjectAction.QuickButtonInListEntry
        Return Nothing
    End Function

    ''' <summary>
    ''' Tooltip description of the quick-button
    ''' </summary>
    Public ReadOnly Property QuickButtonToolTip As String Implements iDataBrowser_FileObjectAction.QuickButtonToolTip
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Set the category, where the menu item for single files will be set.
    ''' </summary>
    Public ReadOnly Property CategoryOfSingleFileActionMenu As mDataBrowserList.APISingleFileToolsMenuCategories Implements iDataBrowser_FileObjectAction.CategoryOfSingleFileActionMenu
        Get
            Return mDataBrowserList.APISingleFileToolsMenuCategories.None
        End Get
    End Property

    ''' <summary>
    ''' Menu button for the single file action context menu.
    ''' </summary>
    Public Function SingleFileActionMenuItem() As ToolStripMenuItem Implements iDataBrowser_FileObjectAction.SingleFileActionMenuItem
        Return Nothing
    End Function

    ''' <summary>
    ''' Set the category, where the menu item for multiple files will be set.
    ''' </summary>
    Public ReadOnly Property CategoryOfMultipleFileActionMenu As mDataBrowserList.APIMultipleFilesToolsMenuCategories Implements iDataBrowser_FileObjectAction.CategoryOfMultipleFileActionMenu
        Get
            Return mDataBrowserList.APIMultipleFilesToolsMenuCategories.SpectroscopyFiles_NumericManipulations
        End Get
    End Property

    ''' <summary>
    ''' Menu button for the multiple file action menu.
    ''' </summary>
    Public Function MultipleFileActionMenuItem() As ToolStripMenuItem Implements iDataBrowser_FileObjectAction.MultipleFileActionMenuItem
        Dim M As New ToolStripMenuItem
        M.Text = My.Resources.rFileObjectActions.MM_DataNonLinearFitWithLastSettings
        M.Image = My.Resources.fit_16
        Return M
    End Function

#End Region

#Region "Multiple file objects"

    ''' <summary>
    ''' Check the settings.
    ''' </summary>
    Public Function MultipleFileActionCheckSettings() As Boolean Implements iDataBrowser_FileObjectAction.MultipleFileActionCheckSettings
        Return True
    End Function

    ''' <summary>
    ''' Maximum number of file-objects to select.
    ''' </summary>
    Public ReadOnly Property MultipleFiles_Count_Max As Integer Implements iDataBrowser_FileObjectAction.MultipleFiles_Count_Max
        Get
            Return Integer.MaxValue
        End Get
    End Property

    ''' <summary>
    ''' Minimum number of file-objects to select.
    ''' </summary>
    Public ReadOnly Property MultipleFiles_Count_Min As Integer Implements iDataBrowser_FileObjectAction.MultipleFiles_Count_Min
        Get
            Return 1
        End Get
    End Property

    ''' <summary>
    ''' Action for multiple file objects.
    ''' Is called async in a thread pool.
    ''' </summary>
    Public Function MultipleFileAction_IndividualAction(ByRef FileObject As cFileObject) As Boolean Implements iDataBrowser_FileObjectAction.MultipleFileAction_IndividualAction
        Return False
    End Function

    ''' <summary>
    ''' All at once action for all file-objects.
    ''' 
    ''' 1) Asks for the fit-model to load.
    ''' 2) Pushes the model to each fit-window, and adds the fit-window to the queue.
    ''' </summary>
    Public Function MultipleFileAction_AllAtOnce(ByRef FileObjectList As List(Of cFileObject)) As Boolean Implements iDataBrowser_FileObjectAction.MultipleFileAction_AllAtOnce

        ' Store the filename of the fitmodel to load.
        Dim FitModelToLoad As String = String.Empty

        Dim FMDialog As New OpenFileDialog()
        With FMDialog
            .CheckFileExists = True
            .CheckPathExists = True
            .AddExtension = True
            .Filter = My.Resources.rFitting.FitModelExport_FileExtensionDescription_SingleData & "|*" & My.Resources.rFitting.FitModelExport_FileExtension_SingleData
            .Title = My.Resources.rFitting.FitMultipleData_SelectFitModelToLoad

        End With

        ' Open the dialog, and get back the result.
        Dim DialogRes As DialogResult = FMDialog.ShowDialog()
        FitModelToLoad = FMDialog.FileName
        FMDialog.Dispose()
        FMDialog = Nothing

        ' If the filename is valid, open a fit-window for all files, and add the window to the fit-queue.
        If DialogRes = DialogResult.OK AndAlso IO.File.Exists(FitModelToLoad) Then

            Dim FitWindow As wFit_SingleDataSet = Nothing
            For Each FO As cFileObject In FileObjectList

                ' Create a new fit-window.
                FitWindow = New wFit_SingleDataSet
                FitWindow.LoadFitModelFileAfterInit = FitModelToLoad
                FitWindow.AddWindowToFitQueueAfterInit = True

                ' Load the file-object.
                FitWindow.Show(FO)

            Next

            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' This function needs to process all the files individually.
    ''' </summary>
    Public ReadOnly Property MultipleFilesProcessedIndividually As Boolean Implements iDataBrowser_FileObjectAction.MultipleFilesProcessedIndividually
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Process multiple files in the thread-pool
    ''' </summary>
    Public ReadOnly Property MultipleFilesProcessASYNC As Boolean Implements iDataBrowser_FileObjectAction.MultipleFilesProcessASYNC
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Single file object"
    ''' <summary>
    ''' Action for a single file object.
    ''' </summary>
    Public Function SingleFileAction(ByRef FileObject As cFileObject) As Boolean Implements iDataBrowser_FileObjectAction.SingleFileAction
        Return False
    End Function
#End Region

End Class
