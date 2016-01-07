Imports System.Threading

''' <summary>
''' Launches a dialog in a separate Single Thread Apartment.
''' (Needed for async launching of an Open- or SaveFileDialog.
''' </summary>
Public Class cDialogInvoker
    Public InvokeDialog As CommonDialog
    Private InvokeThread As Thread
    Private InvokeResult As DialogResult

    Public Sub New(dialog As CommonDialog)
        InvokeDialog = dialog
        InvokeThread = New Thread(New ThreadStart(AddressOf Me.InvokeMethod))
        InvokeThread.SetApartmentState(ApartmentState.STA)
        InvokeResult = DialogResult.None
    End Sub

    Public Function Invoke() As DialogResult
        InvokeThread.Start()
        InvokeThread.Join()
        Return InvokeResult
    End Function

    Private Sub InvokeMethod()
        InvokeResult = InvokeDialog.ShowDialog()
    End Sub
End Class
