Imports System.IO

Public Class cImageConverter

    ''' <summary>
    ''' Returns the JPG version of the given image as byte-array.
    ''' </summary>
    Public Shared Function ImageToByteArray(ByRef ImageIn As Image) As Byte()
        Dim ms As New MemoryStream()
        ImageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        Return ms.ToArray()
    End Function

    ''' <summary>
    ''' Returns the JPG version of the given byte-array of an image.
    ''' </summary>
    Public Shared Function ByteArrayToImage(ByRef ByteIn As Byte()) As Image
        Dim ms As New MemoryStream(ByteIn)
        Try
            Return Image.FromStream(ms)
        Catch ex As Exception
            Return My.Resources.cancel_16
        End Try
    End Function

End Class
