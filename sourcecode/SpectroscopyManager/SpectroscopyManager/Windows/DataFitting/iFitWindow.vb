Public Interface iFitWindow

    ''' <summary>
    ''' Event fired, when the fit is finished!
    ''' </summary>
    Event FitFinishedEvent()

    ''' <summary>
    ''' Delegate called, on form closing
    ''' </summary>
    Delegate Function _AddressToCallbackOnFormClosing() As Boolean

    ''' <summary>
    ''' Delegate called, on form closing
    ''' </summary>
    Property AddressToCallbackOnFormClosing As _AddressToCallbackOnFormClosing

    ''' <summary>
    ''' Starts the fitting.
    ''' </summary>
    Sub StartFitting(Optional ByVal IgnoreWarning As Boolean = False)

    ''' <summary>
    ''' Called, if a fit is added to a fit-queue.
    ''' </summary>
    ''' <param name="QueueIndex">Index, at which place the fit is added</param>
    Sub FitAddedToFitQueue(QueuePosition As Integer)

    ''' <summary>
    ''' Called, if a fit is removed from the fit-queue
    ''' </summary>
    Sub FitRemovedFromFitQueue()

    ''' <summary>
    ''' Called, if the position in the fit-queue is changed
    ''' </summary>
    Sub FitQueuePositionChanged(NewPosition As Integer)

    ''' <summary>
    ''' Property, that represents the text of the window.
    ''' </summary>
    Property Text As String

    ''' <summary>
    ''' Brings the window to the front of the z-order.
    ''' </summary>
    Sub BringToFront()

End Interface
