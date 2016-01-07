Imports System.IO
Imports Ionic.Zip

Public Class cCompression

#Region "Helper function for Stream handling"
    ''' <summary>
    ''' Copys a stream into another
    ''' </summary>
    Public Shared Sub CopyStream(ByRef input As System.IO.Stream, ByRef output As System.IO.Stream)
        Dim ReadedNumber As Integer
        Dim Buffer As Byte() = New Byte(2000 - 1) {}
        ReadedNumber = input.Read(Buffer, 0, 2000)
        Do While (ReadedNumber > 0)
            output.Write(Buffer, 0, ReadedNumber)
            ReadedNumber = input.Read(Buffer, 0, 2000)
        Loop
        output.Flush()
    End Sub

    ''' <summary>
    ''' Copys a stream into another
    ''' </summary>
    Public Shared Sub CopyStream(ByRef input As System.IO.MemoryStream, ByRef output As System.IO.Stream)
        Dim ReadedNumber As Integer
        Dim Buffer As Byte() = New Byte(2000 - 1) {}
        ReadedNumber = input.Read(Buffer, 0, 2000)
        Do While (ReadedNumber > 0)
            output.Write(Buffer, 0, ReadedNumber)
            ReadedNumber = input.Read(Buffer, 0, 2000)
        Loop
        output.Flush()
    End Sub

    ''' <summary>
    ''' Copys a stream into a Byte-Array
    ''' </summary>
    Public Shared Function CopyStreamToByte(ByRef InputStream As System.IO.Stream) As Byte()
        Dim OutputStream As New MemoryStream
        Dim ReadedNumber As Integer
        Dim Buffer As Byte() = New Byte(2000 - 1) {}
        Do
            ReadedNumber = InputStream.Read(Buffer, 0, 2000)
            OutputStream.Write(Buffer, 0, ReadedNumber)
        Loop While ReadedNumber > 0

        Return OutputStream.ToArray
    End Function

    ''' <summary>
    ''' Copys a stream into a Byte-Array
    ''' </summary>
    Public Shared Function CopyStreamToByte(ByRef InputStream As System.IO.Compression.DeflateStream) As Byte()
        Dim OutputStream As New MemoryStream
        Dim ReadedNumber As Integer
        Dim Buffer As Byte() = New Byte(2000 - 1) {}
        Do
            ReadedNumber = InputStream.Read(Buffer, 0, 2000)
            OutputStream.Write(Buffer, 0, ReadedNumber)
        Loop While ReadedNumber > 0

        Return OutputStream.ToArray
    End Function
#End Region

#Region "Compression using ZLib"
    ''' <summary>
    ''' Compresses a Byte array
    ''' </summary>
    Public Shared Function ZlibCompress(ByVal InputBytes As Byte()) As Byte()
        Using output As New MemoryStream
            Using outZStream As Stream = New zlib.ZOutputStream(output, zlib.zlibConst.Z_DEFAULT_COMPRESSION)
                Using input As Stream = New MemoryStream(InputBytes)
                    CopyStream(input, outZStream)
                    Return output.ToArray()
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Decompresses a byte Array
    ''' </summary>
    Public Shared Function ZlibDecompress(ByVal InputBytes As Byte(),
                                          Optional ByVal StartIndex As Integer = 0,
                                          Optional ByVal ByteCount As Integer = -1) As Byte()
        If ByteCount <= 0 Then ByteCount = InputBytes.Length - StartIndex

        If ByteCount + StartIndex > InputBytes.Length Then Return New Byte(1) {}

        Dim InputStream As New MemoryStream(InputBytes, StartIndex, ByteCount, False)
        Dim UncompressedStream As Stream = New Ionic.Zlib.ZlibStream(InputStream, Ionic.Zlib.CompressionMode.Decompress, False)
        Return cCompression.CopyStreamToByte(UncompressedStream)
    End Function
#End Region

#Region "Compression using .NET Compress"
    ''' <summary>
    ''' Compresses a Byte array
    ''' </summary>
    Public Shared Function NETCompress(ByVal InputBytes As Byte()) As Byte()
        Dim InputStream As New MemoryStream(InputBytes)
        Dim OutputStream As New MemoryStream
        Dim CompressStream As New IO.Compression.DeflateStream(OutputStream, Compression.CompressionMode.Compress)
        CompressStream.Write(InputBytes, 0, InputBytes.Length)
        Dim OutputBytes As Byte() = OutputStream.ToArray

        InputStream.Close()
        CompressStream.Close()
        OutputStream.Close()
        Return OutputBytes
    End Function

    ''' <summary>
    ''' Decompresses a byte Array
    ''' </summary>
    Public Shared Function NetDecompress(ByVal InputBytes As Byte()) As Byte()
        Dim OutputStream As New MemoryStream
        Dim UnCompressStream As New IO.Compression.DeflateStream(OutputStream, Compression.CompressionMode.Decompress)
        Return cCompression.CopyStreamToByte(UnCompressStream)
    End Function
#End Region

#Region "Compression of a list of files"

    ''' <summary>
    ''' Compresses the given files to the target-file
    ''' </summary>
    Public Shared Sub CompressFiles(TargetFileName As String, ByVal FileList As List(Of String))
        ' Create the Zip-File
        Dim Zip As New ZipFile
        Zip.AddFiles(FileList, "")
        Zip.Save(TargetFileName)
    End Sub

    ''' <summary>
    ''' Uncompresses the given file to a target-directory and return the
    ''' list of uncompressed files.
    ''' </summary>
    Public Shared Function UncompressFiles(ZipFileName As String,
                                           TargetDirectoryWithFinalSlash As String,
                                           Optional ByVal FileExistsAction As ExtractExistingFileAction = ExtractExistingFileAction.OverwriteSilently) As List(Of String)

        Dim FileList As New List(Of String)

        ' Load the Zip-File
        Dim Zip As ZipFile = ZipFile.Read(ZipFileName)
        Dim ExtractFileName As String
        For Each Entry As ZipEntry In Zip
            ExtractFileName = TargetDirectoryWithFinalSlash & Entry.FileName
            ' Extract all files silently
            Entry.Extract(TargetDirectoryWithFinalSlash, FileExistsAction)
            FileList.Add(ExtractFileName)
        Next
        Return FileList
    End Function

    ''' <summary>
    ''' Uncompresses the given file to a target-directory and return the
    ''' list of uncompressed files.
    ''' </summary>
    Public Shared Function UncompressFilesInMemory(ZipFileName As String) As Dictionary(Of String, MemoryStream)

        Dim FileList As New Dictionary(Of String, MemoryStream)

        ' Load the Zip-File
        Dim Zip As ZipFile = ZipFile.Read(ZipFileName)
        Dim CurrentStream As MemoryStream
        For Each Entry As ZipEntry In Zip
            ' Extract all files silently
            CurrentStream = New MemoryStream
            Entry.Extract(CurrentStream)
            CurrentStream.Seek(0, SeekOrigin.Begin)
            FileList.Add(Entry.FileName, CurrentStream)
        Next
        Return FileList
    End Function


#End Region

End Class
