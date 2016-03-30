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
    Protected _ActionsByTime As New List(Of KeyValuePair(Of Date, KeyValuePair(Of String, String)))

    ''' <summary>
    ''' List of all actions performed by the user over time.
    ''' Here all spectra are stored, and all the changes of parameters
    ''' from the initial parameter set are stored.
    ''' </summary>
    Public ReadOnly Property ActionsByTime As List(Of KeyValuePair(Of Date, KeyValuePair(Of String, String)))
        Get
            Return Me._ActionsByTime
        End Get
    End Property

    ''' <summary>
    ''' List of all measurements described in the Matrix file.
    ''' </summary>
    Protected _Measurements As New List(Of MeasurementDetails)

    ''' <summary>
    ''' List of all measurements described in the Matrix file.
    ''' </summary>
    Public ReadOnly Property Measurements As List(Of MeasurementDetails)
        Get
            Return Me._Measurements
        End Get
    End Property

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
                            FO._ActionsByTime.Add(New KeyValuePair(Of Date, KeyValuePair(Of String, String))(CurrentBlock.Time, New KeyValuePair(Of String, String)(FileNameDescribed, FileNameDescribed)))

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
                            Dim Key As New KeyValuePair(Of String, String)("EEPA:" & PropertyInstance & "." & PropertyName & "[" & PropertyUnit & "]", PropertyValue)
                            FO._ActionsByTime.Add(New KeyValuePair(Of Date, KeyValuePair(Of String, String))(CurrentBlock.Time, Key))

                        End Using

                    Case "KRAM"
                        ' Marks of the files

                        Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                        Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                            Dim Mark As String = ReadString(BlockReader)
                            FO._ActionsByTime.Add(New KeyValuePair(Of Date, KeyValuePair(Of String, String))(CurrentBlock.Time, New KeyValuePair(Of String, String)(Mark, Mark)))

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
                            Dim Measurement As New MeasurementDetails
                            Measurement.Time = CurrentBlock.Time

                            '#########################

                            ' Jump over the next 4 bytes!
                            BlockReader.BaseStream.Seek(4, SeekOrigin.Current)

                            ' Get storage for the new identifiers.
                            Dim SubLastFourChars As String = String.Empty

                            Dim ScanHeader As New List(Of Int32)

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



                                        Dim CurrentSubBlock As DataBlockWithoutTime = ReadBlockWithoutTime(BlockReader, SubLastFourChars)
                                        Using SubBlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentSubBlock)

                                            While SubBlockReader.BaseStream.Position < SubBlockReader.BaseStream.Length
                                                ScanHeader.Add(SubBlockReader.ReadInt32)
                                            End While

                                        End Using

                                    Case "NACS"
                                        ' Data of the curves.
                                        ' Read the subblock.

                                        Dim ListOfInteger As New List(Of UInt32)

                                        Dim CurrentSubBlock As DataBlockWithoutTime = ReadBlockWithoutTime(BlockReader, SubLastFourChars)
                                        Using SubBlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentSubBlock)

                                            'SubBlockReader.BaseStream.Seek(2, SeekOrigin.Current)

                                            For i As Integer = 0 To 204 Step 1

                                                ListOfInteger.Add(SubBlockReader.ReadUInt32)
                                            Next

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
                                                Dim ChannelNumber As Integer = CInt(SubBlockReader.ReadUInt32)
                                                Dim TransferFunction As String = ReadString(SubBlockReader)
                                                Dim Unit As String = ReadString(SubBlockReader)
                                                Dim PropertyCount As Integer = CInt(SubBlockReader.ReadUInt32)
                                                Dim PropertyName As String
                                                Dim PropertyValue As Double
                                                'Dim Key As String

                                                Dim TFF As New TransferFunction
                                                TFF.ChannelNumber = ChannelNumber
                                                TFF.TransferFunctionType = GetTransferFunctionByName(TransferFunction)
                                                TFF.Unit = Unit

                                                For i As Integer = 0 To PropertyCount - 1 Step 1
                                                    PropertyName = ReadString(SubBlockReader)
                                                    PropertyValue = ReadDouble(SubBlockReader)

                                                    '' Create a general entry for the transferfunction property.
                                                    'Key = "XFER:Channel:" & ChannelNumber.ToString & "::" & PropertyName
                                                    'Measurement.TransferFunctionProperties.Add(Key, PropertyValue)

                                                    ' Create a specific entry in the Transferfunction object.
                                                    Select Case PropertyName
                                                        Case "NeutralFactor"
                                                            TFF.NeutralFactor_2 = PropertyValue
                                                        Case "Offset"
                                                            TFF.Offset = PropertyValue
                                                        Case "PreFactor"
                                                            TFF.Prefactor_2 = PropertyValue
                                                        Case "PreOffset"
                                                            TFF.Preoffset_2 = PropertyValue
                                                        Case "Raw_1"
                                                            TFF.Raw1_2 = PropertyValue
                                                        Case "Factor"
                                                            TFF.Factor_1 = PropertyValue
                                                    End Select
                                                Next

                                                ' perform some additional calculations for the multilinear transferfunction

                                                If TFF.TransferFunctionType = TransferFunctionTypes.MultiLinear1D AndAlso (TFF.NeutralFactor_2 * TFF.Prefactor_2) <> 0 Then
                                                    TFF.Whole_2 = (TFF.Raw1_2 - TFF.Preoffset_2) / (TFF.NeutralFactor_2 * TFF.Prefactor_2)
                                                End If

                                                ' Catch an unknown transferfunction,
                                                ' and replace it by a dummy one.
                                                If TFF.TransferFunctionType = TransferFunctionTypes.Unknown Then
                                                    TFF.TransferFunctionType = TransferFunctionTypes.Linear1D
                                                    TFF.Factor_1 = 1
                                                    TFF.Offset = 0
                                                    TFF.Unit = String.Empty
                                                End If

                                                ' Add the TFF object to the list of Transferfunctions.
                                                Measurement.TransferFunctions.Add(ChannelNumber, TFF)

                                            End While

                                        End Using



                                End Select
                            Loop

                            ' Add the measurement to the list of measurements
                            FO._Measurements.Add(Measurement)

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

#Region "Data Extraction Function"

    ''' <summary>
    ''' Returns the position of the property (e.g. the filename) in the ActionList.
    ''' </summary>
    ''' <return>Returns -1, if not found!</return>
    Public Function GetPositionOfPropertyValueInActionList(ByVal Value As String) As Integer

        Dim SearchLimit As Integer = Me._ActionsByTime.Count - 1
        For i As Integer = 0 To SearchLimit Step 1
            If Value = Me._ActionsByTime(i).Value.Key Then
                Return i
            End If
        Next

        Return -1

    End Function

    ''' <summary>
    ''' Returns the entry being chronologically earlier in the list of actions,
    ''' that starts with a specific value.
    ''' Used, e.g., for finding properties of spectra.
    ''' </summary>
    ''' <return>-1 if not found</return>
    Public Function GetPositionOfPropertyValueBeforePosition(ByVal ValueStartsWith As String,
                                                             ByVal SearchPosition As Integer) As Integer

        If SearchPosition >= 0 Then

            ' Go through earlier entries
            For i As Integer = SearchPosition To 0 Step -1
                If Me._ActionsByTime(i).Value.Key.StartsWith(ValueStartsWith) Then
                    Return i
                End If
            Next

        End If

        Return -1

    End Function

    ''' <summary>
    ''' Searches in the action list for the filename, and returns the
    ''' measurement settings, that are lying earlier in time.
    ''' </summary>
    ''' <return>Nothing, if not found.</return>
    Public Function GetMeasurementSettingsRelatedToFileName(ByVal FileName As String) As MeasurementDetails

        ' First get the file record time.
        Dim FilePositionInActionList As Integer = Me.GetPositionOfPropertyValueInActionList(FileName)

        If FilePositionInActionList >= 0 Then

            ' Now lets get the entry of the file for getting the date.
            Dim FileRecordDate As Date = Me._ActionsByTime(FilePositionInActionList).Key

            ' From the date we now search the measurement earlier in time.
            Dim IndexOfTimeBefore As Integer = -1
            For i As Integer = 0 To Me._Measurements.Count - 1 Step 1
                If Me._Measurements(i).Time <= FileRecordDate AndAlso
                   (IndexOfTimeBefore < 0 OrElse (IndexOfTimeBefore >= 0 AndAlso Me._Measurements(i).Time > Me._Measurements(IndexOfTimeBefore).Time)) Then
                    IndexOfTimeBefore = i
                End If
            Next

            ' If there is a measurement before, then return this measurement.
            If IndexOfTimeBefore >= 0 Then
                Return Me._Measurements(IndexOfTimeBefore)
            End If

        End If

        Return Nothing
    End Function

#End Region

End Class
