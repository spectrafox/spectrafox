Imports System.IO

''' <summary>
''' Imports Omicron Matrix parameter files.
''' 
''' Created for SpectraFox by Michael Ruby.
''' 
''' We acknowledge the Gwyddion source code
''' for the Matrix scan image file import routine,
''' which was released under GPL by the following people:
''' 
''' Copyright (C) 2008, Philipp Rahe, David Necas
''' *  E-mail: hquerquadrat@gmail.com
''' </summary>
Public Class cFileImportOmicronMatrixParameterFile
    Inherits cFileImportOmicronMatrix

#Region "Properties"

    ''' <summary>
    ''' Array with all extracted properties of the Matrix file.
    ''' </summary>
    Protected _PropertyArray As New Dictionary(Of String, String)

    ''' <summary>
    ''' Array with all extracted initial properties of the Matrix file.
    ''' </summary>
    Public ReadOnly Property PropertyArray As Dictionary(Of String, String)
        Get
            Return Me._PropertyArray
        End Get
    End Property

    ''' <summary>
    ''' List of all files, that the parameter has descriptions of.
    ''' </summary>
    Protected _FilesDescribed As New List(Of String)

    ''' <summary>
    ''' List of all files, that the parameter has descriptions of.
    ''' </summary>
    Public ReadOnly Property FilesDescribed As List(Of String)
        Get
            Return Me._FilesDescribed
        End Get
    End Property

    ''' <summary>
    ''' List of all actions performed by the user over time.
    ''' Here all spectra are stored, and all the changes of parameters
    ''' from the initial parameter set are stored.
    ''' </summary>
    Protected _ActionsByTime As New List(Of KeyValuePair(Of Date, String))

    ''' <summary>
    ''' List of all actions performed by the user over time.
    ''' Here all spectra are stored, and all the changes of parameters
    ''' from the initial parameter set are stored.
    ''' </summary>
    Public ReadOnly Property ActionsByTime As List(Of KeyValuePair(Of Date, String))
        Get
            Return Me._ActionsByTime
        End Get
    End Property

    ''' <summary>
    ''' List of all measurements described in the Matrix file.
    ''' </summary>
    Protected _Measurements As New List(Of Measurement)

    ''' <summary>
    ''' List of all measurements described in the Matrix file.
    ''' </summary>
    Public ReadOnly Property Measurements As List(Of Measurement)
        Get
            Return Me._Measurements
        End Get
    End Property

#End Region

#Region "Transferfunction definition"

    ''' <summary>
    ''' Types of possible data transfer functions.
    ''' </summary>
    Public Enum TransferFunctions
        Unknown
        Linear1D
        MultiLinear1D
    End Enum

    ''' <summary>
    ''' Structure that stores the scaling information of the values.
    ''' </summary>
    Public Structure ZScalingStruct
        Public TransferFunction As TransferFunctions
        Public Factor_1 As Double
        Public Offset_1 As Double
        Public NeutralFactor_2 As Double
        Public Offset_2 As Double
        Public Prefactor_2 As Double
        Public Preoffset_2 As Double
        Public Raw1_2 As Double
        Public Whole_2 As Double
        Public ChannelNumber As Integer
        Public ChannelName As String
    End Structure

    ''' <summary>
    ''' Returns the TransferFunction by the name in the file.
    ''' </summary>
    Public Shared Function GetTransferFunctionByName(ByVal Name As String) As TransferFunctions
        Select Case Name
            Case "TFF_Linear1D"
                Return TransferFunctions.Linear1D
            Case "TFF_MultiLinear1D"
                Return TransferFunctions.MultiLinear1D
            Case Else
                Return TransferFunctions.Unknown
        End Select
    End Function

#End Region

#Region "Measurement Details"

    ''' <summary>
    ''' Class that contains all details of a measurement.
    ''' E.g. Data types, channels, etc.
    ''' </summary>
    Public Class Measurement

        ''' <summary>
        ''' Data types of all channels
        ''' </summary>
        Public DataTypeList As New Dictionary(Of String, String)

        ''' <summary>
        ''' Channellist in the format (ChannelID, (Name, Unit)
        ''' </summary>
        Public Channels As New Dictionary(Of Integer, KeyValuePair(Of String, String))

        ''' <summary>
        ''' Block Storage List. Format: (ChannelIndex, StorageString)
        ''' </summary>
        Public BlockStorage As New Dictionary(Of Integer, String)

        ''' <summary>
        ''' Transferfunctions for this measurement.
        ''' </summary>
        Public TransferFunctionProperties As New Dictionary(Of String, Double)

    End Class

#End Region

#Region "Transfer function"

    ''' <summary>
    ''' Calculates from the given Integer value the correct double value,
    ''' using the current transfer function stored in <code>TransferFunction</code>.
    ''' </summary>
    Public Function GetValueByTransferFunction(ByVal Value As Integer, ByVal ZScaling As ZScalingStruct) As Double
        Dim DoubleValue As Double = Convert.ToDouble(Value)
        Select Case ZScaling.TransferFunction
            Case TransferFunctions.Linear1D
                ' // use linear1d: p = (r - n)/f
                Return (DoubleValue - ZScaling.Offset_1) / ZScaling.Factor_1
            Case TransferFunctions.MultiLinear1D
                ' // use multilinear1d:
                ' // p = (r - n)*(r0 - n0)/(fn * f0)
                ' //= (r - n)*s.whole_2
                Return (DoubleValue - ZScaling.Preoffset_2) * ZScaling.Whole_2
            Case TransferFunctions.Unknown
                Return DoubleValue
            Case Else
                Return DoubleValue
        End Select
    End Function

#End Region

#Region "Shared Import Function"

    ''' <summary>
    ''' Reads a parameter file, and returns the object describing the file.
    ''' Returns nothing, if an error occured.
    ''' </summary>
    Public Shared Function ReadParameterFile(ByVal FullFileNameToParameterFile As String) As cFileImportOmicronMatrixParameterFile

        ' Create Parameter File Object
        Dim FO As New cFileImportOmicronMatrixParameterFile

        ' Load StreamReader with the Big Endian Encoding
        Dim fs As New FileStream(FullFileNameToParameterFile, FileMode.Open, FileAccess.Read, FileShare.Read)
        ' Now Using BinaryReader to obtain Image-Data
        Dim br As New BinaryReader(fs, System.Text.Encoding.Unicode)

        ' Buffers
        Dim LastFourChars As String = String.Empty

        Try

            ' Read the whole parameter file.
            Do Until fs.Position = fs.Length

                ' Move the identifier buffer by one byte.
                LastFourChars = GetLastFourCharsByProceedingOneByte(br, LastFourChars)

                Select Case LastFourChars

                    Case "ATEM"
                        ' META information concerning the Matrix system.

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)
                            ' read the settings
                            FO._PropertyArray.Add("META:SoftwareName", ReadString(BlockReader))
                            FO._PropertyArray.Add("META:MatrixVersion", ReadString(BlockReader))
                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)
                            ' read the next settings
                            FO._PropertyArray.Add("META:MatrixProfile", ReadString(BlockReader))
                            FO._PropertyArray.Add("META:UserName", ReadString(BlockReader))
                        End Using

                    Case "DPXE"
                        ' Experiment description and properties.

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)
                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                            ' Read 7 strings
                            For i As Integer = 1 To 7 Step 1
                                FO._PropertyArray.Add("EXPD:Property" & i.ToString, ReadString(BlockReader))
                            Next
                        End Using

                    Case "LNEG"
                        ' Project description and files

                        Dim CurrentBlock As DataBlockWithoutTime = ReadBlockWithoutTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)
                            ' Read 3 strings
                            For i As Integer = 1 To 3 Step 1
                                FO._PropertyArray.Add("GENL:Property" & i.ToString, ReadString(BlockReader))
                            Next
                        End Using

                    Case "TSNI"
                        ' configuration of instances

                        Dim CurrentBlock As DataBlockWithoutTime = ReadBlockWithoutTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                            ' Get the number of stored parameters.
                            Dim ParameterCount As UInt32 = BlockReader.ReadUInt32

                            For i As UInteger = 1 To ParameterCount Step 1

                                Dim S1 As String = ReadString(BlockReader)
                                Dim S2 As String = ReadString(BlockReader)
                                Dim S3 As String = ReadString(BlockReader)

                                Dim Key As String = "INST:" & S1 & "::" & S2 & " (" & S3 & ")."

                                Dim PropertyCount As Integer = Convert.ToInt32(BlockReader.ReadUInt32)

                                While PropertyCount > 0

                                    Dim t1 As String = ReadString(BlockReader)
                                    Dim t2 As String = ReadString(BlockReader)
                                    Dim Key2 As String = Key & t1

                                    FO._PropertyArray.Add(Key2, t2)

                                    PropertyCount -= 1

                                End While

                            Next

                        End Using

                    Case "APEE"
                        ' Configuration of experiment
                        ' altered values are recorded in PMOD
                        ' the most important parts are in XYScanner

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                            Dim PropertyCount As Integer = Convert.ToInt32(BlockReader.ReadUInt32)
                            Dim GroupItemNumber As Integer
                            Dim CheckSub As Boolean
                            Dim Prop As String
                            Dim Unit As String

                            While PropertyCount > 0

                                ' Get the sub instruction.
                                Dim Instruction As String = ReadString(BlockReader)
                                If Instruction = "XYScanner" Then
                                    CheckSub = True
                                Else
                                    CheckSub = False
                                End If

                                ' Get the number of group items.
                                GroupItemNumber = Convert.ToInt32(BlockReader.ReadUInt32)
                                While GroupItemNumber > 0

                                    ' Read the property and the unit
                                    Prop = ReadString(BlockReader)
                                    Unit = ReadString(BlockReader)

                                    ' Jump over the next 4 bytes!
                                    BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                                    FO._PropertyArray.Add("EEPA:" & Instruction & "." & Prop & "[" & Unit & "]", ReadObject(BlockReader))

                                    GroupItemNumber -= 1

                                End While

                                PropertyCount -= 1
                            End While

                        End Using


                    Case "FERB"
                        ' Here comes the data file name,
                        ' and the modified properties of the data file,
                        ' including any marks.

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                            Dim FileNameDescribed As String = ReadString(BlockReader)
                            FO._FilesDescribed.Add(FileNameDescribed)
                            FO._ActionsByTime.Add(New KeyValuePair(Of Date, String)(CurrentBlock.Time, FileNameDescribed))

                        End Using

                    Case "DOMP"
                        ' Here we get the changed parameters.

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                            Dim PropertyInstance As String = ReadString(BlockReader)
                            Dim PropertyName As String = ReadString(BlockReader)
                            Dim PropertyUnit As String = ReadString(BlockReader)

                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                            Dim PropertyValue As String = ReadObject(BlockReader)
                            Dim Key As String = PropertyInstance & ":" & PropertyName & "[" & PropertyUnit & "]" & "=" & PropertyValue
                            FO._ActionsByTime.Add(New KeyValuePair(Of Date, String)(CurrentBlock.Time, Key))

                        End Using

                    Case "KRAM"
                        ' Marks of the files

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                            Dim Mark As String = ReadString(BlockReader)
                            FO._ActionsByTime.Add(New KeyValuePair(Of Date, String)(CurrentBlock.Time, Mark))

                        End Using

                    Case "YSCC"
                        ' Defines a specific measurement.
                        ' This measurement is encapsuled in this block,
                        ' which we interpret separately!
                        ' It has the inner blocks TCID, SCHC, NACS, REFX, FERB.
                        ' These blocks contain the measurement data.

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                            ' Create a new measurement object.
                            Dim Measurement As New Measurement

                            '#########################

                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                            ' Get storage for the new identifiers.
                            Dim SubLastFourChars As String = String.Empty

                            ' Read the whole subset of parameters
                            Do Until BlockReader.BaseStream.Position = BlockReader.BaseStream.Length

                                ' Move the identifier buffer by one byte.
                                SubLastFourChars = GetLastFourCharsByProceedingOneByte(BlockReader, SubLastFourChars)

                                Select Case SubLastFourChars

                                    Case "TCID"
                                        ' // description and internal number of captured channels
                                        ' // has to be linkend to the physical devices 
                                        ' // given in XFER to get the scaling

                                        ' Read the subblock.
                                        Dim CurrentSubBlock As DataBlockWithoutTime = ReadBlockWithoutTime(BlockReader, SubLastFourChars)
                                        Using SubBlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentSubBlock)

                                            ' Jump over the next 8 bytes!
                                            SubBlockReader.BaseStream.Seek(8, SeekOrigin.Current)

                                            ' Get the data types of the channels.
                                            Dim PropertiesCount As Integer = CInt(SubBlockReader.ReadUInt32)

                                            ' Go through the data types list.
                                            For i As Integer = 0 To PropertiesCount - 1 Step 1

                                                ' Jump over the next 16 bytes!
                                                SubBlockReader.BaseStream.Seek(16, SeekOrigin.Current)

                                                ' Add the properties
                                                Measurement.DataTypeList.Add(ReadString(SubBlockReader), ReadString(SubBlockReader))

                                            Next

                                            ' Now get the channel description
                                            Dim ChannelCount As Integer = CInt(SubBlockReader.ReadUInt32)
                                            Dim Name As String
                                            Dim Unit As String
                                            Dim ChannelNumber As Integer

                                            ' Go through the channel list.
                                            For i As Integer = 0 To ChannelCount - 1 Step 1

                                                ' Jump over the next 4 bytes!
                                                SubBlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                                                ChannelNumber = CInt(SubBlockReader.ReadUInt32)

                                                ' Jump over the next 8 bytes!
                                                SubBlockReader.BaseStream.Seek(8, SeekOrigin.Current)

                                                Name = ReadString(SubBlockReader)
                                                Unit = ReadString(SubBlockReader)

                                                ' Add to the channel list
                                                Measurement.Channels.Add(ChannelNumber, New KeyValuePair(Of String, String)(Name, Unit))

                                            Next

                                            ' Get block information
                                            ChannelCount = CInt(SubBlockReader.ReadUInt32)

                                            ' Go through the channel list.
                                            For i As Integer = 0 To ChannelCount - 1 Step 1
                                                ' Jump over the next 16 bytes!
                                                SubBlockReader.BaseStream.Seek(16, SeekOrigin.Current)

                                                ' Read the storage string.
                                                Measurement.BlockStorage.Add(i, ReadString(SubBlockReader))
                                            Next

                                        End Using

                                    Case "SCHC"
                                        ' Header of the curves.
                                        ' Who knows what's in there?

                                        Dim ListOfInteger As New List(Of UInt32)

                                        Dim CurrentSubBlock As DataBlockWithoutTime = ReadBlockWithoutTime(BlockReader, SubLastFourChars)
                                        Using SubBlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentSubBlock)

                                            While SubBlockReader.BaseStream.Position < SubBlockReader.BaseStream.Length
                                                ListOfInteger.Add(SubBlockReader.ReadUInt32)

                                            End While



                                        End Using

                                    Case "NACS"
                                        ' Data of the curves.
                                        ' Read the subblock.
                                        Dim CurrentSubBlock As DataBlockWithoutTime = ReadBlockWithoutTime(BlockReader, SubLastFourChars)
                                        Using SubBlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentSubBlock)

                                        End Using

                                    Case "REFX"
                                        ' Data after the measurement data block.
                                        ' Contains all information for scaling,
                                        ' such as the transferfunction properties.

                                        Dim CurrentSubBlock As DataBlockWithoutTime = ReadBlockWithoutTime(BlockReader, SubLastFourChars)
                                        Using SubBlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentSubBlock)

                                            While SubBlockReader.BaseStream.Position < SubBlockReader.BaseStream.Length

                                                ' Jump over the next 4 bytes!
                                                SubBlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                                                ' Now comes the channel number and channel name
                                                Dim ChannelNumber As UInt32 = SubBlockReader.ReadUInt32
                                                Dim TransferFunction As String = ReadString(SubBlockReader)
                                                Dim Unit As String = ReadString(SubBlockReader)
                                                Dim A As Integer = CInt(SubBlockReader.ReadUInt32)
                                                Dim PropertyName As String
                                                Dim PropertyValue As Double
                                                Dim Key As String

                                                For i As Integer = 0 To A - 1 Step 1
                                                    PropertyName = ReadString(SubBlockReader)
                                                    PropertyValue = ReadDouble(SubBlockReader)
                                                    Key = "XFER:Channel:" & ChannelNumber.ToString & "::" & PropertyName
                                                    Measurement.TransferFunctionProperties.Add(Key, PropertyValue)
                                                Next

                                            End While

                                        End Using
                                End Select
                            Loop

                        End Using



                    Case "WEIV"
                        ' VIEW: Details of the software windows, etc. ...
                        ' unimportant, so jump to the end of the block, by reading the whole block.
                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)

                    Case "CORP"
                        ' PROC: Processors of the scanning windows, etc. ...
                        ' unimportant, so jump to the end of the block, by reading the whole block.
                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)

                    Case "QESF"
                        ' FSEQ: something with file?
                        ' unimportant, so jump to the end of the block, by reading the whole block.
                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)

                    Case "SPXE"
                        ' EXPS: something with experiment?
                        ' unimportant, so jump to the end of the block, by reading the whole block.
                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)

                    Case "DEOE"
                        ' End of file reached
                        Exit Do

                End Select

            Loop

        Catch ex As Exception
            Debug.WriteLine("cFileImportOmicronMatrixParameterFile: Error reading parameter file: " & ex.Message)
            FO = Nothing
        Finally
            br.Close()
            fs.Close()
            br.Dispose()
            fs.Dispose()
        End Try

        Return FO
    End Function

#End Region

End Class
