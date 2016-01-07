Imports Amib.Threading

''' <summary>
''' Extends the Threadpool with some custom features.
''' </summary>
Public Class cSmartThreadPoolExtended
    Inherits SmartThreadPool

    ''' <summary>
    ''' A new work-item has been queued
    ''' </summary>
    Public Event NewWorkQueued()

    ' ''' <summary>
    ' ''' Raises and event additionally to queuing the work-item.
    ' ''' </summary>
    'Public Shadows Function QueueWorkItem(Callback As WorkItemCallback, Priority As WorkItemPriority) As IWorkItemResult
    '    RaiseEvent NewWorkQueued()
    '    Return MyBase.QueueWorkItem(Callback, Priority)
    'End Function

End Class
