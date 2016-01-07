Imports System.ComponentModel

Public Module cEventExtensions

    ' ''' <summary>Raises the event (on the UI thread if available).</summary>
    ' ''' <param name="multicastDelegate">The event to raise.</param>
    ' ''' <param name="sender">The source of the event.</param>
    ' ''' <param name="e">An EventArgs that contains the event data.</param>
    ' ''' <returns>The return value of the event invocation or null if none.</returns>
    '<System.Runtime.CompilerServices.Extension> _
    'Public Function Raise(multicastDelegate As MulticastDelegate, sender As Object, e As EventArgs) As Object
    '    Dim retVal As Object = Nothing

    '    Dim threadSafeMulticastDelegate As MulticastDelegate = multicastDelegate
    '    If threadSafeMulticastDelegate IsNot Nothing Then
    '        For Each d As [Delegate] In threadSafeMulticastDelegate.GetInvocationList()
    '            Dim synchronizeInvoke = TryCast(d.Target, ISynchronizeInvoke)
    '            If (synchronizeInvoke IsNot Nothing) AndAlso synchronizeInvoke.InvokeRequired Then
    '                retVal = synchronizeInvoke.EndInvoke(synchronizeInvoke.BeginInvoke(d, {sender, e}))
    '            Else
    '                retVal = d.DynamicInvoke({sender, e})
    '            End If
    '        Next
    '    End If

    '    Return retVal
    'End Function

    ''' <summary>
    ''' Safely raises any EventHandler event asynchronously.
    ''' </summary>
    ''' <param name="sender">The object raising the event (usually this).</param>
    ''' <param name="e">The EventArgs for this event.</param>
    <System.Runtime.CompilerServices.Extension> _
    Public Sub Raise(thisEvent As MulticastDelegate, sender As Object, e As EventArgs)
        Dim uiMethod As EventHandler
        Dim target As ISynchronizeInvoke
        Dim callback As New AsyncCallback(AddressOf EndAsynchronousEvent)

        For Each d As [Delegate] In thisEvent.GetInvocationList()
            uiMethod = TryCast(d, EventHandler)
            If uiMethod IsNot Nothing Then
                target = TryCast(d.Target, ISynchronizeInvoke)
                If target IsNot Nothing Then
                    target.BeginInvoke(uiMethod, {sender, e})
                Else
                    uiMethod.BeginInvoke(sender, e, callback, uiMethod)
                End If
            End If
        Next
    End Sub

    Private Sub EndAsynchronousEvent(result As IAsyncResult)
        DirectCast(result.AsyncState, EventHandler).EndInvoke(result)
    End Sub

    ''' <summary>
    ''' If ExplicitThread is Nothing, will execute the thread in the UI-Thread.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Sub RaiseEventAndExecuteItInAnExplicitOrUIThread(ByVal _event As System.MulticastDelegate,
                                                            ByVal _ParamArray_args() As Object,
                                                            ByVal ExplicitThreadSynchronizationDispatcher As System.Windows.Threading.Dispatcher)
        If Not _event Is Nothing Then
            If _event.GetInvocationList().Length > 0 Then
                Dim _sync As System.ComponentModel.ISynchronizeInvoke = Nothing
                For Each _delegate As System.MulticastDelegate In _event.GetInvocationList()
                    If ((ExplicitThreadSynchronizationDispatcher Is Nothing) AndAlso (_sync Is Nothing) AndAlso (GetType(System.ComponentModel.ISynchronizeInvoke).IsAssignableFrom(_delegate.Target.GetType())) AndAlso (Not _delegate.Target.GetType().IsAbstract)) Then
                        Try
                            _sync = CType(_delegate.Target, System.ComponentModel.ISynchronizeInvoke)
                        Catch ex As Exception
                            Diagnostics.Debug.WriteLine(ex.ToString())
                            _sync = Nothing
                        End Try
                    End If
                    If Not ExplicitThreadSynchronizationDispatcher Is Nothing Then
                        Try
                            ExplicitThreadSynchronizationDispatcher.Invoke(_delegate, _ParamArray_args)
                        Catch ex As Exception
                            Diagnostics.Debug.WriteLine(ex.ToString())
                        End Try
                    Else
                        If _sync Is Nothing Then
                            Try
                                _delegate.DynamicInvoke(_ParamArray_args)
                            Catch ex As Exception
                                Diagnostics.Debug.WriteLine(ex.ToString())
                            End Try
                        Else
                            Try
                                _sync.Invoke(_delegate, _ParamArray_args)
                            Catch ex As Exception
                                Diagnostics.Debug.WriteLine(ex.ToString())
                            End Try
                        End If
                    End If
                Next
            End If
        End If
    End Sub
End Module
