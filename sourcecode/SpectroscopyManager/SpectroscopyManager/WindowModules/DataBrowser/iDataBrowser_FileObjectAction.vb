
''' <summary>
''' This interface can be implemented, if the class
''' offers the possibility to perform some actions
''' with file-objects.
''' </summary>
Public Interface iDataBrowser_FileObjectAction

    ''' <summary>
    ''' Tells the API which file-type can be handled.
    ''' </summary>
    ReadOnly Property CanHandleFileObjectType As cFileObject.FileTypes

#Region "Single file actions"

    ''' <summary>
    ''' Can the class handle e.g. several spectroscopy files?
    ''' Or just single?
    ''' </summary>
    ReadOnly Property CanHandleSingleFileObjects As Boolean

    ''' <summary>
    ''' How many files does the procedure accept maximum
    ''' Usually Integer.Max
    ''' </summary>
    ReadOnly Property MultipleFiles_Count_Max As Integer

    ''' <summary>
    ''' How many files does the procedure accept minimum
    ''' Usually 0.
    ''' </summary>
    ReadOnly Property MultipleFiles_Count_Min As Integer

    ''' <summary>
    ''' Implement single file-action.
    ''' </summary>
    Function SingleFileAction(ByRef FileObject As cFileObject) As Boolean

    ''' <summary>
    ''' Return a button, if the class should provide quick access to the
    ''' single-file-object action.
    ''' </summary>
    Function QuickButtonInListEntry() As Button

    ''' <summary>
    ''' If a quick-button is available, please give a description for the tool-tip shown.
    ''' </summary>
    ReadOnly Property QuickButtonToolTip As String

    ''' <summary>
    ''' If this file implements a menu-item for single files,
    ''' the category where this item should appear
    ''' in the menu has to be set.
    ''' </summary>
    ReadOnly Property CategoryOfSingleFileActionMenu As mDataBrowserList.APISingleFileToolsMenuCategories

    ''' <summary>
    ''' Returns the menu item shown in the single-file action menu of the data browser.
    ''' Is shown in the given category by <code>CategoryOfSingleFileActionMenu</code>.
    ''' </summary>
    Function SingleFileActionMenuItem() As ToolStripMenuItem

#End Region

#Region "Multiple file actions"

    ''' <summary>
    ''' Can the class handle e.g. several spectroscopy files?
    ''' Or just single?
    ''' </summary>
    ReadOnly Property CanHandleMultipleFileObjects As Boolean

    ''' <summary>
    ''' Implement actions on multiple files.
    ''' This function is called for each file in a selection of multiple files.
    ''' The data is processed async in a thread-pool, to provide quickest processing.
    ''' </summary>
    Function MultipleFileAction_IndividualAction(ByRef FileObject As cFileObject) As Boolean

    ''' <summary>
    ''' Implement actions on multiple files.
    ''' This function is called with a list of all file-objects.
    ''' It is needed e.g. for line-scan plotting, or in general for processing
    ''' data, which depends on each other.
    ''' The data is processed async in a background-thread, to provide quickest processing.
    ''' </summary>
    Function MultipleFileAction_AllAtOnce(ByRef FileObjectList As List(Of cFileObject)) As Boolean

    ''' <summary>
    ''' Tells the executing function whether to execute the 
    ''' <code>MultipleFileAction_AllAtOnce</code> (on false) or the
    ''' <code>MultipleFileAction_IndividualAction</code> (on true)
    ''' functions.
    ''' </summary>
    ReadOnly Property MultipleFilesProcessedIndividually As Boolean

    ''' <summary>
    ''' If all files are processed at once, determine, whether to use the thread-pool
    ''' for execution, or not. E.g. if a window has to be shown, or not.
    ''' </summary>
    ReadOnly Property MultipleFilesProcessASYNC As Boolean

    ''' <summary>
    ''' This function should check, if all settings for multiple-file actions are set.
    ''' If not, then return false, else true!
    ''' </summary>
    Function MultipleFileActionCheckSettings() As Boolean

    ''' <summary>
    ''' If this file implements a menu-item for multiple files,
    ''' the category where this item should appear
    ''' in the menu has to be set.
    ''' </summary>
    ReadOnly Property CategoryOfMultipleFileActionMenu As mDataBrowserList.APIMultipleFilesToolsMenuCategories

    ''' <summary>
    ''' Returns the menu item shown in the multiple-file action menu of the data browser.
    ''' Is shown in the given category by <code>CategoryOfMultipleFileActionMenu</code>.
    ''' </summary>
    Function MultipleFileActionMenuItem() As ToolStripMenuItem

#End Region


End Interface
