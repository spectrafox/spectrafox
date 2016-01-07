Imports System.Threading

''' <summary>
''' Launches a Clipboard in a separate Single Thread Apartment.
''' (Needed for async filling of data into the clipboard.)
''' </summary>
Public Class cClipboardInvoker
    Private InvokeThread As Thread

    Public ClipboardText As String
    Public ClipboardImage As Image
    Public ClipboardObject As Object

    Public Sub New(ClipboardContent As String)
        Me.ClipboardText = ClipboardContent
        InvokeThread = New Thread(New ThreadStart(AddressOf Me.InvokeMethodText))
        InvokeThread.SetApartmentState(ApartmentState.STA)
    End Sub

    Public Sub New(ClipboardContent As Image)
        Me.ClipboardImage = ClipboardContent
        InvokeThread = New Thread(New ThreadStart(AddressOf Me.InvokeMethodImage))
        InvokeThread.SetApartmentState(ApartmentState.STA)
    End Sub

    Public Sub New(ClipboardContent As Object)
        Me.ClipboardObject = ClipboardContent
        InvokeThread = New Thread(New ThreadStart(AddressOf Me.InvokeMethodDataObject))
        InvokeThread.SetApartmentState(ApartmentState.STA)
    End Sub


    Public Function Invoke() As Boolean
        Try
            InvokeThread.Start()
            InvokeThread.Join()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub InvokeMethodText()
        Clipboard.SetText(ClipboardText)
    End Sub
    Private Sub InvokeMethodImage()
        SyncLock ClipboardImage
            Clipboard.SetImage(ClipboardImage)
        End SyncLock
    End Sub
    Private Sub InvokeMethodDataObject()
        Clipboard.SetDataObject(ClipboardObject)
    End Sub
End Class