Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportOmicronMatrixCurve
    Inherits cFileImportOmicronMatrix
    Implements iFileImport_SpectroscopyTable

    ''' <summary>
    ''' Imports the spectroscopy-file into a SpectroscopyTable object.
    ''' </summary>
    Public Function ImportBias(ByRef FullFileNamePlusPath As String,
                               ByVal FetchOnlyFileHeader As Boolean,
                               Optional ByRef ReaderBuffer As String = "",
                               Optional ByRef FilesToIgnoreAfterThisImport As List(Of String) = Nothing) As cSpectroscopyTable Implements iFileImport_SpectroscopyTable.ImportSpectroscopyTable

        ' Check, if the ignore list is nothing. If yes, then create a new list.
        If FilesToIgnoreAfterThisImport Is Nothing Then FilesToIgnoreAfterThisImport = New List(Of String)

        ' First get the file base name from the individual file name.
        Dim FileName As String = IO.Path.GetFileName(FullFileNamePlusPath)
        Dim FileDirectory As New DirectoryInfo(IO.Path.GetDirectoryName(FullFileNamePlusPath))
        Dim FileListInDirectory As FileInfo() = FileDirectory.GetFiles()

        ' Get the base name of the file.
        Dim BaseFileNameComponents As SpectroscopyFileName = Me.GetSpectroscopyFileNameComponents(FileName)

        ' Create new SpectroscopyTable object.
        Dim oSpectroscopyTable As New cSpectroscopyTable
        oSpectroscopyTable.FullFileName = FullFileNamePlusPath

        ' Omicron Matrix files consist out of a separate file for each trace.
        ' So we collect a list of all files from the folder that match with the basename.
        For Each oFile As FileInfo In FileListInDirectory

            Dim CurrentFileNameComponents As SpectroscopyFileName = Me.GetSpectroscopyFileNameComponents(oFile.Name)

            '#######################################################
            ' Search for all files of the same dataset.
            ' This is the case, if the basename matches, the curve number, and the unit!
            If CurrentFileNameComponents.BaseName = BaseFileNameComponents.BaseName AndAlso
               CurrentFileNameComponents.CurveNumber = BaseFileNameComponents.CurveNumber AndAlso
               CurrentFileNameComponents.DataUnit = BaseFileNameComponents.DataUnit Then

                ' Create a new DataColumn.
                Dim DataCol As New cSpectroscopyTable.DataColumn

                ' Load StreamReader with the Big Endian Encoding
                Dim fs As New FileStream(oFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)
                ' Now Using BinaryReader to obtain Image-Data
                Dim br As New BinaryReader(fs, System.Text.Encoding.Default)

                ReaderBuffer = ""

                ' Buffers
                Dim LastFourChars(3) As Char
                Dim Int32Buffer(3) As Byte
                Dim Int64Buffer(7) As Byte

                Dim ExpectedNumber As UInt32
                Dim RecordedNumber As UInt32
                Dim RecordTimeTicks As ULong
                Dim Data As New List(Of Double)

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
                            oSpectroscopyTable.RecordDate = New DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(RecordTimeTicks).ToLocalTime()

                        Case "CSED"
                            ' This announces the number of points in the curve.

                            ' The next 24 bytes are unknown.
                            fs.Seek(24, SeekOrigin.Current)

                            ' The next 4 bytes are UINT32-LE as point number.
                            fs.Read(Int32Buffer, 0, 4)
                            ExpectedNumber = BitConverter.ToUInt32(Int32Buffer, 0)
                            fs.Read(Int32Buffer, 0, 4)
                            RecordedNumber = BitConverter.ToUInt32(Int32Buffer, 0)
                            oSpectroscopyTable.MeasurementPoints = Convert.ToInt32(RecordedNumber)

                    End Select

                Loop

                br.Close()
                fs.Close()
                br.Dispose()
                fs.Dispose()

                ' Set some extra columns
                With DataCol
                    .IsSpectraFoxGenerated = False
                    .Name = CurrentFileNameComponents.CurveName
                    .UnitSymbol = CurrentFileNameComponents.DataUnit
                    .UnitType = cUnits.GetUnitTypeFromSymbol(CurrentFileNameComponents.DataUnit)
                    .SetValueList(Data)
                End With

                ' Finally add the datacolumn to the SpectroscopyTable
                oSpectroscopyTable.AddNonPersistentColumn(DataCol)

                ' Ignore this file for further imports, since we already imported it.
                FilesToIgnoreAfterThisImport.Add(oFile.FullName)

            End If
            '#######################################################

            '#######################################################
            ' Also search for the parameter file, if it is present.
            ' It consists out of the "base name"+"_0001.mtrx".
            If oFile.Name = BaseFileNameComponents.BaseName & "_0001.mtrx" Then
                ' Parameter-File found.
                'ParameterFileName = oFile.FullName


            End If
            '#######################################################

        Next

        ' Add a separate Column just for the measurement point number
        Dim PointColumn As New cSpectroscopyTable.DataColumn
        PointColumn.Name = My.Resources.ColumnName_MeasurementPoints
        PointColumn.IsSpectraFoxGenerated = False
        PointColumn.UnitSymbol = "1"
        PointColumn.UnitType = cUnits.UnitType.Unitary
        Dim PointData As New List(Of Double)(oSpectroscopyTable.MeasurementPoints)
        If Not FetchOnlyFileHeader Then
            For i As Integer = 1 To oSpectroscopyTable.MeasurementPoints Step 1
                PointData.Add(i)
            Next
        End If
        PointColumn.SetValueList(PointData)
        oSpectroscopyTable.AddNonPersistentColumn(PointColumn)

        ' File Exists, so set the property.
        oSpectroscopyTable._bFileExists = True

        Return oSpectroscopyTable
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_SpectroscopyTable.FileExtension
        Get
            Return "_mtrx"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given file is a known Omicron-Matrix spectroscopy image file
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile

        ' Load StreamReader and read first identifier.
        ' Is the only one needed for Identification.
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
            If SpectroscopyFileExtension.IsMatch(FileExtension) Then
                Return True
            End If

        End If

        Return False
    End Function

End Class
