Public Class wFitQueue
    Inherits wFormBase

    Protected _CurrentyRunningFitWindow As iFitWindow = Nothing

    ''' <summary>
    ''' Queue of fit-windows to start after the currently running fit-window.
    ''' </summary>
    Protected _FitWindowsInQueue As New List(Of iFitWindow)

    Private bReady As Boolean = False

#Region "Window opening and closing"
    ''' <summary>
    ''' Window constructor
    ''' </summary>
    Private Sub wFitQueue_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Window closing. Cancel, if a fit is running, or if the fit-queue is not empty.
    ''' </summary>
    Private Sub wFitQueue_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _FitWindowsInQueue.Count > 0 Or _CurrentyRunningFitWindow IsNot Nothing Then
            MessageBox.Show(My.Resources.rFitQueue.FormClosing_NotEmptyOrFitRunning,
                            My.Resources.rFitQueue.FormClosing_NotEmptyOrFitRunning_Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Cancel = True
        End If
    End Sub

#End Region

#Region "Add and remove fits from the fit-queue"

    ''' <summary>
    ''' Event fired, if a fit-window got added.
    ''' </summary>
    Public Event FitAddedToFitQueue(ByRef FitWindow As iFitWindow, FitQueuePosition As Integer)

    ''' <summary>
    ''' Event that informs all members in the fit-queue, if the fit-queue has changed.
    ''' </summary>
    Public Event FitQueueChanged()

    ''' <summary>
    ''' Adds the fit to the queue and returns the number in the queue.
    ''' </summary>
    Public Function AddFitToFitQueue(ByRef Fit As iFitWindow) As Integer
        If Me._FitWindowsInQueue.Contains(Fit) Then
            Return Me._FitWindowsInQueue.IndexOf(Fit)
        End If

        ' Add the fit
        Me._FitWindowsInQueue.Add(Fit)

        Dim NewIndex As Integer = Me._FitWindowsInQueue.Count - 1

        ' Register the new fit-queue member everywhere.
        Fit.FitAddedToFitQueue(NewIndex)
        RaiseEvent FitAddedToFitQueue(Fit, NewIndex)
        RaiseEvent FitQueueChanged()

        ' Add the fit-window closing event
        Fit.AddressToCallbackOnFormClosing = AddressOf Me.wFitQueueMemberWindow_Closing

        Return NewIndex
    End Function

    ''' <summary>
    ''' Removes the fit from the queue.
    ''' </summary>
    Public Function RemoveFitFromFitQueue(ByRef Fit As iFitWindow) As Boolean
        If Me._FitWindowsInQueue.Contains(Fit) Then
            ' Remove the fit-window closing event
            Fit.AddressToCallbackOnFormClosing = Nothing
            
            ' Call the remove-function in the fit.
            Fit.FitRemovedFromFitQueue()

            ' remove the fit from the list.
            Me._FitWindowsInQueue.Remove(Fit)

            ' tell the others, that their positions might have changed.
            RaiseEvent FitQueueChanged()
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Removes the fit from the queue.
    ''' </summary>
    Public Function RemoveFitFromFitQueue(Index As Integer) As Boolean
        If Me._FitWindowsInQueue.Count > Index Then
            Dim Fit As iFitWindow = Me._FitWindowsInQueue(Index)

            ' Remove the fit-window closing event
            Fit.AddressToCallbackOnFormClosing = Nothing

            ' Call the remove-function in the fit.
            Fit.FitRemovedFromFitQueue()

            ' remove the fit from the list.
            Me._FitWindowsInQueue.RemoveAt(Index)

            ' tell the others, that their positions might have changed.
            RaiseEvent FitQueueChanged()
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Removes all fits from he queue.
    ''' </summary>
    Public Sub ClearFitQueue()
        ' Go through all fit-windows an inform them about their position.
        For i As Integer = 0 To Me._FitWindowsInQueue.Count - 1 Step 1
            Me._FitWindowsInQueue(i).FitRemovedFromFitQueue()
            ' Remove the fit-window closing event
            Me._FitWindowsInQueue(i).AddressToCallbackOnFormClosing = Nothing
        Next
        Me._FitWindowsInQueue.Clear()

        ' tell the others, that their positions might have changed.
        RaiseEvent FitQueueChanged()
    End Sub

#End Region

#Region "Fit queue member window closing"

    ''' <summary>
    ''' Window closing. Cancel, if a fit is running, or if the fit-queue is not empty.
    ''' </summary>
    Private Function wFitQueueMemberWindow_Closing() As Boolean
        MessageBox.Show(My.Resources.rFitQueue.FitWindowFromQueue_FormClosing,
                        My.Resources.rFitQueue.FitWindowFromQueue_FormClosing_Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return True
    End Function

#End Region

#Region "Fit Queue changed"

    ''' <summary>
    ''' Fit Queue changed, so inform all queue members about their current position.
    ''' </summary>
    Protected Sub FitQueueChangedHandler() Handles Me.FitQueueChanged

        ' Inform the waiting queue-members.
        Me.FitQueueChanged_InformQueueMembers()

        ' check, if a fit is running. If not, start the fit for the first queue position.
        If Me._CurrentyRunningFitWindow Is Nothing And Me._FitWindowsInQueue.Count > 0 And Not Me.PauseFit Then

            ' Get the fit-object
            Dim Fit As iFitWindow = Me._FitWindowsInQueue(0)

            ' Set the Fit to the currently active fit.
            Me._CurrentyRunningFitWindow = Fit

            ' Set the handler to catch if the fit has finished, 
            ' so that we are able to start the next fit in the queue.
            AddHandler Me._CurrentyRunningFitWindow.FitFinishedEvent, AddressOf RunningFitFinished

            ' Remove the first member from the fit-queue
            Me.RemoveFitFromFitQueue(Fit)

            ' Start the fit!
            Fit.StartFitting(True)
        End If

        ' update the interface
        Me.FitQueueChanged_UpdateInterface()

    End Sub

    ''' <summary>
    ''' Fit Queue changed, so inform all queue members about their current position.
    ''' </summary>
    Protected Sub FitQueueChanged_InformQueueMembers()
        ' Go through all fit-windows an inform them about their position.
        For i As Integer = 0 To Me._FitWindowsInQueue.Count - 1 Step 1
            Me._FitWindowsInQueue(i).FitQueuePositionChanged(i)
        Next
    End Sub

    ''' <summary>
    ''' Fit Queue changed, update the interface
    ''' </summary>
    Protected Sub FitQueueChanged_UpdateInterface()

        ' Update the interface
        If Me._CurrentyRunningFitWindow Is Nothing Then
            Me.lblCurrentFit.Text = "-"
        Else
            Me.lblCurrentFit.Text = Me._CurrentyRunningFitWindow.Text
        End If

        ' Update the listbox.
        Me.bReady = False
        Me.lbQueue.Items.Clear()
        For i As Integer = 0 To Me._FitWindowsInQueue.Count - 1 Step 1
            Me.lbQueue.Items.Add(Me._FitWindowsInQueue(i).Text)
        Next
        Me.bReady = True
    End Sub

#End Region

#Region "currently running fit finished"
    ''' <summary>
    ''' currently running fit finished
    ''' </summary>
    Protected Sub RunningFitFinished()
        ' Remove this event handler
        RemoveHandler Me._CurrentyRunningFitWindow.FitFinishedEvent, AddressOf RunningFitFinished

        ' Remove the current fit-object.
        Me._CurrentyRunningFitWindow = Nothing

        ' Call the fit-queue changed -function, to start the next fit!
        RaiseEvent FitQueueChanged()
    End Sub
#End Region

#Region "gets the fit-queue window, if one is open"
    ''' <summary>
    ''' Gets the current fit-queue window open, or opens a new one,
    ''' if none exists before.
    ''' </summary>
    Public Shared Function GetFitQueueWindow() As wFitQueue
        ' Go through all forms and show them in the list.
        For i As Integer = 0 To Application.OpenForms.Count - 1 Step 1
            If TypeOf Application.OpenForms.Item(i) Is wFitQueue Then
                Return CType(Application.OpenForms.Item(i), wFitQueue)
            End If
        Next

        ' If the program gets to this point, no open fit-queue was found.
        ' So create a new one.
        Dim FQ As New wFitQueue
        FQ.Show()
        Return FQ
    End Function
#End Region

#Region "Play / Pause fit queue"

    ''' <summary>
    ''' Decides whether to pass new fits form the queue.
    ''' </summary>
    Protected PauseFit As Boolean = True

    ''' <summary>
    ''' Start of restart the fitting of the queue.
    ''' </summary>
    Private Sub btnPlayPause_Click(sender As Object, e As EventArgs) Handles btnPlayPause.Click
        If PauseFit Then
            Me.PauseFit = False

            'Me.btnPlayPause.Text = My.Resources.rFitQueue.FitQueue_Pause
            Me.btnPlayPause.Image = My.Resources.pause_16

            ' Raise the queue-changed event to relaunch the fit.
            RaiseEvent FitQueueChanged()
        Else
            Me.PauseFit = True

            'Me.btnPlayPause.Text = My.Resources.rFitQueue.FitQueue_Play
            Me.btnPlayPause.Image = My.Resources.play_16
        End If
    End Sub

#End Region

#Region "Goto fit-window buttons"

    ''' <summary>
    ''' Goto current fit-window.
    ''' </summary>
    Private Sub btnGoToRunningFitWindow_Click(sender As Object, e As EventArgs) Handles btnGoToRunningFitWindow.Click
        If Not Me._CurrentyRunningFitWindow Is Nothing Then
            Me._CurrentyRunningFitWindow.BringToFront()
        End If
    End Sub

    ''' <summary>
    ''' Bring to front the fit-window one double clicks on.
    ''' </summary>
    Private Sub lbQueue_SelectedIndexChanged(sender As Object, e As MouseEventArgs) Handles lbQueue.MouseDoubleClick
        Dim index As Integer = Me.lbQueue.IndexFromPoint(e.Location)
        If index <> System.Windows.Forms.ListBox.NoMatches Then
            Me._FitWindowsInQueue(index).BringToFront()
        End If
    End Sub
#End Region

#Region "Remove from queue buttons"
    ''' <summary>
    ''' Clears the fit-queue
    ''' </summary>
    Private Sub btnClearFitQueue_Click(sender As Object, e As EventArgs) Handles btnClearFitQueue.Click
        Me.ClearFitQueue()
    End Sub

    ''' <summary>
    ''' Removes a single fit from the queue.
    ''' </summary>
    Private Sub btnRemoveFromFitQueue_Click(sender As Object, e As EventArgs) Handles btnRemoveFromQueue.Click
        If Not Me.bReady Then Return

        Dim FitIndex As Integer = Me.lbQueue.SelectedIndex
        If FitIndex >= 0 Then
            If Me._FitWindowsInQueue.Count > FitIndex Then
                If Not Me._FitWindowsInQueue(FitIndex) Is Nothing Then
                    Me.RemoveFitFromFitQueue(FitIndex)
                End If
            End If
        End If
    End Sub
#End Region


End Class