Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

''' <summary>
''' Imports Omicron Matrix image files.
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
Public Class cFileImportOmicronMatrixImage
    Inherits cFileImportOmicronMatrix
    Implements iFileImport_ScanImage

#Region "Import routine"

    ''' <summary>
    ''' Imports the ScanImage of an Omicron Matrix image file into an ScanImage.
    ''' </summary>
    Public Function ImportSXM(ByRef FullFileNamePlusPath As String,
                              ByVal FetchOnlyFileHeader As Boolean,
                              Optional ByRef ReaderBuffer As String = "",
                              Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing,
                              Optional ByRef ParameterFilesImportedOnce As List(Of iFileImport_ParameterFileToBeImportedOnce) = Nothing) As cScanImage Implements iFileImport_ScanImage.ImportScanImage

        ' Check, if the ignore list exists
        If FilesToIgnoreAfterThisImport Is Nothing Then FilesToIgnoreAfterThisImport = New List(Of String)
        If ParameterFilesImportedOnce Is Nothing Then ParameterFilesImportedOnce = New List(Of iFileImport_ParameterFileToBeImportedOnce)

        ' First get the file base name from the individual file name.
        Dim FileName As String = IO.Path.GetFileName(FullFileNamePlusPath)
        Dim FileDirectory As New DirectoryInfo(IO.Path.GetDirectoryName(FullFileNamePlusPath))
        Dim FileListInDirectory As FileInfo() = FileDirectory.GetFiles()

        ' Get the base name of the file.
        Dim BaseFileNameComponents As ScanImageFileName = Me.GetScanImageFileNameComponents(FileName)

        '################################################################
        ' First of all, search for the parameter file, if it is present.
        ' It consists out of the "base name"+"_0001.mtrx".
        Dim ParameterFile As cFileImportOmicronMatrixParameterFile = Nothing
        For Each oFile As FileInfo In FileListInDirectory
            If oFile.Name = BaseFileNameComponents.BaseName & "_0001.mtrx" Then

                ' Check, if the parameter file is already imported:
                Dim ParameterFileFoundInCache As Boolean = False
                For i As Integer = 0 To ParameterFilesImportedOnce.Count - 1 Step 1
                    If ParameterFilesImportedOnce(i).ParameterFileName = oFile.FullName AndAlso
                       ParameterFilesImportedOnce(i).ParameterFileType = GetType(cFileImportOmicronMatrixParameterFile) Then
                        ParameterFile = DirectCast(ParameterFilesImportedOnce(i), cFileImportOmicronMatrixParameterFile)
                        ParameterFileFoundInCache = True
                        Exit For
                    End If
                Next

                ' Get the parameter file, if we did not get it from the cache.
                If Not ParameterFileFoundInCache Then
                    ParameterFile = cFileImportOmicronMatrixParameterFile.ReadParameterFile(oFile.FullName)
                    ParameterFilesImportedOnce.Add(ParameterFile)
                End If

                ' Exit the loop, since we have found the file.
                Exit For
                End If
        Next
        '#######################################################

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

                ' Get the XYScanner-properties for the file
                Dim XYScanner As XYScannerProperties = GetXYScannerPropertiesFromPropertyArray(oFile.Name, ParameterFile)

                ' Get the measurement configuration for the data.
                ' This is necessary for getting the correct units and transferfunctions.
                Dim MeasurementConfig As cFileImportOmicronMatrixParameterFile.MeasurementDetails
                Dim TFF As TransferFunction
                Dim ChannelID As Integer = -1
                Dim ChannelUnit As String = ""

                If ParameterFile IsNot Nothing Then
                    MeasurementConfig = ParameterFile.GetMeasurementSettingsRelatedToFileName(oFile.Name)
                    ChannelID = MeasurementConfig.GetChannelIDFromMeasurement(CurrentFileNameComponents.ChannelName)
                    If ChannelID >= 0 Then
                        TFF = MeasurementConfig.TransferFunctions(ChannelID)
                        ChannelUnit = MeasurementConfig.Channels(ChannelID).Value
                    Else
                        TFF = New TransferFunction
                    End If
                Else
                    TFF = New TransferFunction
                End If


                ' Set the scan image properties from the XYScanner
                With oScanImage
                    .ScanAngle = XYScanner.Angle
                    .ScanOffset_X = XYScanner.XOffset
                    .ScanOffset_Y = XYScanner.YOffset
                    .ScanPixels_X = XYScanner.XPoints
                    .ScanPixels_Y = XYScanner.YPoints
                    .ScanRange_X = XYScanner.Width
                    .ScanRange_Y = XYScanner.Height
                    .ZControllerProportionalGain = XYScanner.ProportionalGain
                    .ZControllerSetpoint = XYScanner.Setpoint
                    .ZControllerSetpointUnit = cUnits.GetUnitSymbolFromType(cUnits.GetUnitTypeFromSymbol(XYScanner.SetpointUnit))
                    .Bias = XYScanner.BiasVoltage
                    .Current = XYScanner.Setpoint
                    .ZControllerOn = XYScanner.FeedbackOn
                    .ACQ_Time = XYScanner.RasterPeriodTime

                    ' Correct the position of the image.
                    ' The coordinate of the image is in the center of the image.
                    ' --> So perform a coordinate transformation to the top left corner of the image.
                    With oScanImage
                        Dim NewScanLocation As cNumericalMethods.Point2D = cNumericalMethods.BackCoordinateTransform(.ScanOffset_X - .ScanRange_X / 2,
                                                                                                                     .ScanOffset_Y - .ScanRange_Y / 2,
                                                                                                                     .ScanOffset_X, .ScanOffset_Y,
                                                                                                                     -oScanImage.ScanAngle)
                        .ScanOffset_X += NewScanLocation.x
                        .ScanOffset_Y += NewScanLocation.y
                    End With
                End With

                Dim ExpectedNumber As UInt32
                Dim RecordedNumber As UInt32
                Dim RecordTimeTicks As ULong
                Dim Data As New List(Of Integer)

                ' Load StreamReader with the Big Endian Encoding
                Dim fs As New FileStream(oFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)

                ' Now Using BinaryReader to obtain Image-Data
                Dim br As New BinaryReader(fs, System.Text.Encoding.Default)

                Try

                    ' Buffers
                    Dim LastFourChars As String = String.Empty
                    ReaderBuffer = ""

                    ' Read the header up to the position of the data, which is announced by "ATAD".
                    Do Until fs.Position = fs.Length Or LastFourChars = "ATAD"

                        ' Move the identifier buffer by one byte.
                        LastFourChars = GetLastFourCharsByProceedingOneByte(br, LastFourChars)

                        Select Case LastFourChars

                            Case "TLKB"
                                ' BKLT: This announces the timestamp at which the file has been recorded.

                                ' BLOCK READING NOT WORKING HERE???
                                'Dim CurrentBlock As DataBlockWithTime = ReadBlockWithTime(br, LastFourChars)
                                'Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                                '    ' Store the time as record time.
                                '    oSpectroscopyTable.RecordDate = CurrentBlock.Time

                                'End Using

                                ' Jump over the first 4 bytes!
                                fs.Seek(4, SeekOrigin.Current)

                                ' Timestamp of the file. The next 8 bytes.
                                RecordTimeTicks = br.ReadUInt64
                                oScanImage.RecordDate = New DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(RecordTimeTicks).ToLocalTime()

                            Case "CSED"
                                ' DESC: This is the descriptor of the file.
                                ' E.g. it announces the number of points in the curve.

                                Dim CurrentBlock As DataBlockWithoutTime = ReadBlockWithoutTime(br, LastFourChars)
                                Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                                    ' The next 20 bytes are unknown.
                                    BlockReader.BaseStream.Seek(20, SeekOrigin.Current)

                                    ' The next 4 bytes are UINT32-LE as point number.
                                    ExpectedNumber = BlockReader.ReadUInt32
                                    RecordedNumber = BlockReader.ReadUInt32

                                End Using

                            Case "ATAD"
                                ' DATA: This announces that the data is following.

                                ' Abort reading of the header. From here on the data starts.
                                If FetchOnlyFileHeader Then
                                    Exit Do
                                End If

                                Dim CurrentBlock As DataBlockWithoutTime = ReadBlockWithoutTime(br, LastFourChars)
                                Using BlockReader As BinaryReader = GetBinaryReaderForBlockContent(CurrentBlock)

                                    ' Create the data storage, using the length information obtained before
                                    Data = New List(Of Integer)(CInt(RecordedNumber))

                                    ' read the data
                                    Dim Value As Int32
                                    While BlockReader.BaseStream.Position < BlockReader.BaseStream.Length
                                        Value = BlockReader.ReadInt32
                                        Data.Add(Value)
                                    End While

                                End Using

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

                ' Create an emergency catch of the data.
                Dim NumberOfPixels As Integer = Convert.ToInt32(Math.Sqrt(ExpectedNumber) / 2)
                If oScanImage.ScanPixels_X <= 0 Then oScanImage.ScanPixels_X = NumberOfPixels
                If oScanImage.ScanPixels_Y <= 0 Then oScanImage.ScanPixels_Y = NumberOfPixels
                If oScanImage.ScanRange_X <= 0 Then oScanImage.ScanRange_X = NumberOfPixels
                If oScanImage.ScanRange_Y <= 0 Then oScanImage.ScanRange_Y = NumberOfPixels

                Dim DataPoint As Integer = 0

                ' Create ScanChannels according to the GridMode.
                ' Gridmode = 0: Up, Down, ReUp, ReDown.
                Select Case XYScanner.GridMode
                    Case 0
                        ' No constraint.
                        Dim ChannelFWUp As New cScanImage.ScanChannel
                        Dim ChannelBWUp As New cScanImage.ScanChannel

                        ' First do the up channels.
                        With ChannelFWUp
                            .IsSpectraFoxGenerated = False
                            .Name = CurrentFileNameComponents.ChannelName & " FW Up"
                            .UnitSymbol = ChannelUnit
                            .Unit = cUnits.GetUnitTypeFromSymbol(.UnitSymbol)
                        End With
                        With ChannelBWUp
                            .IsSpectraFoxGenerated = False
                            .Name = CurrentFileNameComponents.ChannelName & " BW Up"
                            .UnitSymbol = ChannelUnit
                            .Unit = cUnits.GetUnitTypeFromSymbol(.UnitSymbol)
                        End With

                        If Not FetchOnlyFileHeader Then

                            ChannelFWUp.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Double.NaN)
                            ChannelBWUp.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Double.NaN)

                            For y As Integer = 0 To oScanImage.ScanPixels_Y - 1 Step 1
                                If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For
                                For x As Integer = 0 To oScanImage.ScanPixels_X - 1 Step 1
                                    If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For

                                    ' // Trace Forward Up
                                    ChannelFWUp.ScanData(y, x) = GetValueByTransferFunction(Data(DataPoint), TFF)

                                    DataPoint += 1
                                Next

                                For x As Integer = oScanImage.ScanPixels_X - 1 To 0 Step -1
                                    If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For

                                    ' // Trace Backward Up
                                    ChannelBWUp.ScanData(y, x) = GetValueByTransferFunction(Data(DataPoint), TFF)

                                    DataPoint += 1
                                Next
                            Next

                        End If

                        oScanImage.AddScanChannel(ChannelFWUp)
                        oScanImage.AddScanChannel(ChannelBWUp)

                        ' Now do the down channels, if there are datapoints left.

                        If DataPoint < Data.Count Then

                            Dim ChannelFWDown As New cScanImage.ScanChannel
                            Dim ChannelBWDown As New cScanImage.ScanChannel

                            With ChannelFWDown
                                .IsSpectraFoxGenerated = False
                                .Name = CurrentFileNameComponents.ChannelName & " FW Down"
                                .UnitSymbol = ChannelUnit
                                .Unit = cUnits.GetUnitTypeFromSymbol(.UnitSymbol)
                            End With
                            With ChannelBWDown
                                .IsSpectraFoxGenerated = False
                                .Name = CurrentFileNameComponents.ChannelName & " BW Down"
                                .UnitSymbol = ChannelUnit
                                .Unit = cUnits.GetUnitTypeFromSymbol(.UnitSymbol)
                            End With

                            If Not FetchOnlyFileHeader Then

                                ChannelFWDown.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Double.NaN)
                                ChannelBWDown.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Double.NaN)

                                For y As Integer = oScanImage.ScanPixels_Y - 1 To 0 Step -1
                                    If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For
                                    For x As Integer = 0 To oScanImage.ScanPixels_X - 1 Step 1
                                        If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For

                                        ' // Trace Forward Down
                                        ChannelFWUp.ScanData(y, x) = GetValueByTransferFunction(Data(DataPoint), TFF)

                                        DataPoint += 1
                                    Next

                                    For x As Integer = oScanImage.ScanPixels_X - 1 To 0 Step -1
                                        If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For

                                        ' // Trace Backward Down
                                        ChannelBWUp.ScanData(y, x) = GetValueByTransferFunction(Data(DataPoint), TFF)

                                        DataPoint += 1
                                    Next
                                Next
                            End If

                            oScanImage.AddScanChannel(ChannelFWDown)
                            oScanImage.AddScanChannel(ChannelBWDown)

                        End If

                    Case 1
                        ' Constraint Line
                        Dim ChannelFWUp As New cScanImage.ScanChannel
                        Dim ChannelBWUp As New cScanImage.ScanChannel

                        ' Set some extra columns
                        With ChannelFWUp
                            .IsSpectraFoxGenerated = False
                            .Name = CurrentFileNameComponents.ChannelName & " FW"
                            .UnitSymbol = ChannelUnit
                            .Unit = cUnits.GetUnitTypeFromSymbol(.UnitSymbol)
                        End With
                        With ChannelBWUp
                            .IsSpectraFoxGenerated = False
                            .Name = CurrentFileNameComponents.ChannelName & " BW"
                            .UnitSymbol = ChannelUnit
                            .Unit = cUnits.GetUnitTypeFromSymbol(.UnitSymbol)
                        End With

                        If Not FetchOnlyFileHeader Then

                            ChannelFWUp.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Double.NaN)
                            ChannelBWUp.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Double.NaN)

                            For y As Integer = 0 To oScanImage.ScanPixels_Y - 1 Step 1
                                If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For
                                For x As Integer = 0 To oScanImage.ScanPixels_X - 1 Step 1
                                    If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For

                                    ' // Trace Forward Up
                                    ChannelFWUp.ScanData(y, x) = GetValueByTransferFunction(Data(DataPoint), TFF)

                                    DataPoint += 1
                                Next

                                For x As Integer = oScanImage.ScanPixels_X - 1 To 0 Step -1
                                    If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For

                                    ' // Trace Backward Up
                                    ChannelBWUp.ScanData(y, x) = GetValueByTransferFunction(Data(DataPoint), TFF)

                                    DataPoint += 1
                                Next
                            Next
                        End If

                        oScanImage.AddScanChannel(ChannelFWUp)
                        oScanImage.AddScanChannel(ChannelBWUp)

                    Case 2
                        ' Constraint Point: Only one trace.
                        Dim ChannelFWUp As New cScanImage.ScanChannel

                        ' Set some extra columns
                        With ChannelFWUp
                            .IsSpectraFoxGenerated = False
                            .Name = CurrentFileNameComponents.ChannelName
                            .UnitSymbol = ChannelUnit
                            .Unit = cUnits.GetUnitTypeFromSymbol(.UnitSymbol)
                        End With

                        If Not FetchOnlyFileHeader Then

                            ChannelFWUp.ScanData = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Create(oScanImage.ScanPixels_Y, oScanImage.ScanPixels_X, Double.NaN)

                            For y As Integer = 0 To oScanImage.ScanPixels_Y - 1 Step 1
                                If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For
                                For x As Integer = 0 To oScanImage.ScanPixels_X - 1 Step 1
                                    If DataPoint >= RecordedNumber OrElse DataPoint >= Data.Count Then Exit For

                                    ' // Trace Up
                                    ChannelFWUp.ScanData(y, x) = GetValueByTransferFunction(Data(DataPoint), TFF)

                                    DataPoint += 1
                                Next
                            Next
                        End If

                        oScanImage.AddScanChannel(ChannelFWUp)

                End Select

                ' Ignore this file for further imports, since we already imported it.
                FilesToIgnoreAfterThisImport.Add(oFile.FullName)

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

#End Region

End Class
