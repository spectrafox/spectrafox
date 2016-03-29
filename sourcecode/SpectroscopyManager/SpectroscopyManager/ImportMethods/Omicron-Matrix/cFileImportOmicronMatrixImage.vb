Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' Created with the help of Gwyddion Matrix file import.
''' Released under terms of the GPL.
''' </summary>
Public Class cFileImportOmicronMatrixImage
    Inherits cFileImportOmicronMatrix
    Implements iFileImport_ScanImage

    ''' <summary>
    ''' Imports the ScanImage of an Omicron Matrix image file into an ScanImage.
    ''' </summary>
    Public Function ImportSXM(ByRef FullFileNamePlusPath As String,
                              ByVal FetchOnlyFileHeader As Boolean,
                              Optional ByRef ReaderBuffer As String = "",
                              Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing) As cScanImage Implements iFileImport_ScanImage.ImportScanImage


        ' Check, if the ignore list exists
        If FilesToIgnoreAfterThisImport Is Nothing Then FilesToIgnoreAfterThisImport = New List(Of String)

        ' First get the file base name from the individual file name.
        Dim FileName As String = IO.Path.GetFileName(FullFileNamePlusPath)
        Dim FileDirectory As New DirectoryInfo(IO.Path.GetDirectoryName(FullFileNamePlusPath))
        Dim FileListInDirectory As FileInfo() = FileDirectory.GetFiles()

        ' Get the base name of the file.
        Dim BaseFileNameComponents As ScanImageFileName = Me.GetScanImageFileNameComponents(FileName)

        ' Create new ScanImage object.
        Dim oScanImage As New cScanImage
        oScanImage.FullFileName = FullFileNamePlusPath

        ' Omicron Matrix files consist out of a separate file for each trace.
        ' So we collect a list of all files from the folder that match with the basename.
        For Each oFile As FileInfo In FileListInDirectory

            Dim CurrentFileNameComponents As ScanImageFileName = Me.GetScanImageFileNameComponents(oFile.Name)

            '#######################################################
            ' Search for all files of the same dataset.
            ' This is the case, if the basename matches, the curve number, and the unit!
            If CurrentFileNameComponents.BaseName = BaseFileNameComponents.BaseName AndAlso
               CurrentFileNameComponents.CurveNumber = BaseFileNameComponents.CurveNumber Then

                ' Create a new ScanChannel for forward and backward data.
                Dim ChannelFW As New cScanImage.ScanChannel
                Dim ChannelBW As New cScanImage.ScanChannel

                ReaderBuffer = ""

                ' Buffers
                Dim LastFourChars(3) As Char
                Dim Int32Buffer(3) As Byte
                Dim Int64Buffer(7) As Byte

                Dim ExpectedNumber As UInt32
                Dim RecordedNumber As UInt32
                Dim RecordTimeTicks As ULong
                Dim Data As New List(Of Double)

                ' Load StreamReader with the Big Endian Encoding
                Dim fs As New FileStream(oFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)

                ' Now Using BinaryReader to obtain Image-Data
                Dim br As New BinaryReader(fs, System.Text.Encoding.Default)

                Try

                    ' Read the header up to the position of the data, which is announced by "ATAD".
                    Do Until fs.Position = fs.Length Or LastFourChars = "ATAD"

                        ' Move the identifier buffer by one byte.
                        LastFourChars(0) = LastFourChars(1)
                        LastFourChars(1) = LastFourChars(2)
                        LastFourChars(2) = LastFourChars(3)
                        LastFourChars(3) = Convert.ToChar(fs.ReadByte)

                        Select Case LastFourChars

                            Case "ATAD"
                                ' This announces that the data is following.

                                ' Abort reading of the header. From here on the data starts.
                                If FetchOnlyFileHeader Then
                                    Exit Do
                                End If

                                ' Jump over the first 4 bytes!
                                fs.Seek(4, SeekOrigin.Current)

                                ' read the data
                                Do Until fs.Position = fs.Length
                                    fs.Read(Int32Buffer, 0, 4)
                                    Data.Add(BitConverter.ToInt32(Int32Buffer, 0))
                                Loop

                            Case "TLKB"
                                ' This announces the timestamp at which the file has been recorded.

                                ' Jump over the first 4 bytes!
                                fs.Seek(4, SeekOrigin.Current)

                                ' Timestamp of the file. The next 8 bytes.
                                fs.Read(Int64Buffer, 0, 8)
                                RecordTimeTicks = BitConverter.ToUInt64(Int64Buffer, 0)
                                oScanImage.RecordDate = New DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(RecordTimeTicks).ToLocalTime()

                            Case "CSED"
                                ' This announces the number of points in the curve.

                                ' The next 24 bytes are unknown.
                                fs.Seek(24, SeekOrigin.Current)

                                ' The next 4 bytes are UINT32-LE as point number.
                                fs.Read(Int32Buffer, 0, 4)
                                ExpectedNumber = BitConverter.ToUInt32(Int32Buffer, 0)
                                fs.Read(Int32Buffer, 0, 4)
                                RecordedNumber = BitConverter.ToUInt32(Int32Buffer, 0)

                        End Select

                    Loop

                Catch ex As Exception
                    Debug.WriteLine("cFileImportOmicronMatrixImage: Error reading data file: " & ex.Message)
                Finally
                    br.Close()
                fs.Close()
                br.Dispose()
                fs.Dispose()
                End Try

                ' Set some extra columns
                With ChannelFW
                    .IsSpectraFoxGenerated = False
                    .Name = CurrentFileNameComponents.ChannelName
                    .UnitSymbol = "?"
                    .Unit = cUnits.GetUnitTypeFromSymbol("?")
                End With
                With ChannelBW
                    .IsSpectraFoxGenerated = False
                    .Name = CurrentFileNameComponents.ChannelName & " BW"
                    .UnitSymbol = "?"
                    .Unit = cUnits.GetUnitTypeFromSymbol("?")
                End With

                Dim NumberOfPixels As Integer = Convert.ToInt32(Math.Sqrt(ExpectedNumber) / 2)
                oScanImage.ScanPixels_X = NumberOfPixels
                oScanImage.ScanPixels_Y = NumberOfPixels
                oScanImage.ScanRange_X = NumberOfPixels
                oScanImage.ScanRange_Y = NumberOfPixels

                ' Split up the values into a matrix of the size of the expected values.
                ChannelFW.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(NumberOfPixels, NumberOfPixels, 0)
                ChannelBW.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(NumberOfPixels, NumberOfPixels, 0)

                Dim x As Integer = 0
                Dim y As Integer = 0
                For i As Integer = 0 To Data.Count - 1 Step 1

                    ' Check the numbers
                    If x >= 2 * NumberOfPixels OrElse y >= NumberOfPixels Then Continue For

                    ' Fill the forward and backward channels.
                    If x < NumberOfPixels Then
                        ChannelFW.ScanData(y, x) = Data(i)
                    Else
                        ChannelBW.ScanData(y, NumberOfPixels - (x - NumberOfPixels) - 1) = Data(i)
                    End If

                    ' Increase X by one.
                    x += 1
                    If x >= 2 * NumberOfPixels Then
                        x = 0
                        y += 1
                    End If

                Next

                ' Finally add the Channel to the ScanImage
                oScanImage.AddScanChannel(ChannelFW)
                oScanImage.AddScanChannel(ChannelBW)

                ' Ignore this file for further imports, since we already imported it.
                FilesToIgnoreAfterThisImport.Add(oFile.FullName)

            End If
            '#######################################################

            '#######################################################
            ' Also search for the parameter file, if it is present.
            ' It consists out of the "base name"+"_0001.mtrx".
            If oFile.Name = BaseFileNameComponents.BaseName & "_0001.mtrx" Then
                ' Parameter-File found.

                ReaderBuffer = ""

                ' Buffers
                Dim LastFourChars(3) As Char
                Dim Int32Buffer(3) As Byte
                Dim Int64Buffer(7) As Byte

                ' Load StreamReader with the Big Endian Encoding
                Dim fs As New FileStream(oFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)
                ' Now Using BinaryReader to obtain Image-Data
                Dim br As New BinaryReader(fs, System.Text.Encoding.Unicode)

                ' Array with all extracted properties.
                Dim PropertyArray As New Dictionary(Of String, String)

                Try

                    ' Read the whole parameter file.
                    Do Until fs.Position = fs.Length

                        ' Move the identifier buffer by one byte.
                        LastFourChars(0) = LastFourChars(1)
                        LastFourChars(1) = LastFourChars(2)
                        LastFourChars(2) = LastFourChars(3)
                        LastFourChars(3) = Convert.ToChar(fs.ReadByte)

                        Select Case LastFourChars

                            Case "ATEM"
                                ' Some general Matrix related settings.

                                ' Jump over the first 12 bytes!
                                fs.Seek(12, SeekOrigin.Current)
                                ' read the settings
                                PropertyArray.Add("ATEM:SoftwareName", ReadString(br))
                                PropertyArray.Add("ATEM:MatrixVersion", ReadString(br))
                                ' Jump over the next 4 bytes!
                                fs.Seek(4, SeekOrigin.Current)
                                ' read the next settings
                                PropertyArray.Add("ATEM:MatrixProfile", ReadString(br))
                                PropertyArray.Add("ATEM:UserName", ReadString(br))

                            Case "DPXE"
                                ' Project description and files

                                ' Jump over the next 16 bytes!
                                fs.Seek(16, SeekOrigin.Current)

                                ' Read 7 strings
                                For i As Integer = 1 To 7 Step 1
                                    PropertyArray.Add("DPXE:Property" & i.ToString, ReadString(br))
                                Next

                            Case "LNEG"
                                ' Project description and files

                                ' Jump over the next 4 bytes!
                                fs.Seek(4, SeekOrigin.Current)

                                ' Read 3 strings
                                For i As Integer = 1 To 3 Step 1
                                    PropertyArray.Add("LNEG:Property" & i.ToString, ReadString(br))
                                Next

                            Case "TSNI"
                                ' configuration of instances

                                ' Jump over the first 4 bytes!
                                fs.Seek(4, SeekOrigin.Current)

                                ' Get the number of stored parameters.
                                Dim ParameterCount As UInt32 = br.ReadUInt32

                                For i As UInteger = 1 To ParameterCount Step 1

                                    Dim S1 As String = ReadString(br)
                                    Dim S2 As String = ReadString(br)
                                    Dim S3 As String = ReadString(br)

                                    Dim Key As String = "TSNI:" & S1 & "::" & S2 & " (" & S3 & ")."

                                    Dim PropertyCount As Integer = Convert.ToInt32(br.ReadUInt32)

                                    While PropertyCount > 0

                                        Dim t1 As String = ReadString(br)
                                        Dim t2 As String = ReadString(br)
                                        Dim Key2 As String = Key & t1

                                        PropertyArray.Add(Key2, t2)

                                        PropertyCount -= 1
                                    End While

                                Next

                            Case "APEE"
                                ' Configuration of experiment
                                ' altered values are recorded in PMOD
                                ' the most important parts are in XYScanner

                                ' Jump over the first 16 bytes!
                                fs.Seek(16, SeekOrigin.Current)

                                Dim PropertyCount As Integer = Convert.ToInt32(br.ReadUInt32)
                                Dim GroupItemNumber As Integer
                                Dim CheckSub As Boolean
                                Dim Prop As String
                                Dim Unit As String

                                While PropertyCount > 0

                                    ' Get the sub instruction.
                                    Dim Instruction As String = ReadString(br)
                                    If Instruction = "XYScanner" Then
                                        CheckSub = True
                                    Else
                                        CheckSub = False
                                    End If

                                    ' Get the number of group items.
                                    GroupItemNumber = Convert.ToInt32(br.ReadUInt32)
                                    While GroupItemNumber > 0

                                        ' Read the property and the unit
                                        Prop = ReadString(br)
                                        Unit = ReadString(br)

                                        ' Jump over the next 4 bytes!
                                        fs.Seek(4, SeekOrigin.Current)

                                        PropertyArray.Add("EEPA:" & Instruction & "." & Prop & "[" & Unit & "]", ReadObject(br))

                                        GroupItemNumber -= 1
                                    End While



                                    PropertyCount -= 1
                                End While

                        End Select

                    Loop

                Catch ex As Exception
                    Debug.WriteLine("cFileImportOmicronMatrixImage: Error reading parameter file: " & ex.Message)
                Finally
                    br.Close()
                    fs.Close()
                    br.Dispose()
                    fs.Dispose()
                End Try

            End If
            '#######################################################

        Next

        Return oScanImage
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_ScanImage.FileExtension
        Get
            Return "_mtrx"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known Omicron-Matrix spectroscopy image file
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_ScanImage.IdentifyFile

        ' Load StreamReader and read first identifier.
        ' Is the only one needed for identification.
        Dim sr As New StreamReader(FullFileNamePlusPath)
        Dim Buffer(DataFileIdentifier.Length - 1) As Char
        sr.ReadBlock(Buffer, 0, DataFileIdentifier.Length)
        sr.Close()
        sr.Dispose()

        ' All Omicron data files start with the DataFileIdentifier.
        ' If we stumble upon this identifier, we can start loading the file.
        ' For the individual we can then load the parameter file separately.
        If Buffer = DataFileIdentifier Then

            ' Now check, if the file is a SpectroscopyFile.
            ' This is done from the file-extension, which should contain
            ' the curve name, e.g. I(V)_mtrx.
            Dim FileExtension As String = IO.Path.GetExtension(FullFileNamePlusPath)
            If ScanImageFileExtension.IsMatch(FileExtension) Then
                Return True
            End If

        End If

        Return False
    End Function

End Class
