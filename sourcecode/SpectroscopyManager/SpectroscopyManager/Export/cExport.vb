Imports System.Text

''' <summary>
''' Class for exporting several different files.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class cExport

#Region "Properties"
    ' File Export Status
    Protected MaxFileExports As Integer = 0
    Protected CurrentFileExports As Integer = 0
#End Region

#Region "Events"
    ''' <summary>
    ''' Event raised, if a file gets successfully exported.
    ''' </summary>
    Public Event FileExportedSuccessfully(ByVal SourceFileObject As cFileObject,
                                          ByVal TargetFileName As String,
                                          ByVal TotalNumberOfFiles As Integer,
                                          ByVal CurrentFileNumber As Integer)

    ''' <summary>
    ''' Event raised, if a file gets successfully exported.
    ''' </summary>
    Protected Event _FileExportedSuccessfully(ByVal SourceFileObject As cFileObject,
                                              ByVal TargetFileName As String)

    ''' <summary>
    ''' Function to raise the event from an inherited class.
    ''' </summary>
    Protected Sub RaiseEventFileExportedSuccessfully(ByVal SourceFileObject As cFileObject,
                                                     ByVal TargetFileName As String)
        RaiseEvent _FileExportedSuccessfully(SourceFileObject, TargetFileName)
    End Sub

    ''' <summary>
    ''' Event raised, if the export finished with all files.
    ''' </summary>
    Public Event AllFilesExportedSuccessfully()
#End Region

#Region "Functions"

    ''' <summary>
    ''' Increases the internal file counter, to send after all files the AllFileExportedSuccessfully Event.
    ''' </summary>
    Private Sub FileExportedSuccessfullyCounter(ByVal SourceFileObject As cFileObject, ByVal TargetFileName As String) Handles Me._FileExportedSuccessfully
        If Me.MaxFileExports > 0 Then
            Me.CurrentFileExports += 1

            RaiseEvent FileExportedSuccessfully(SourceFileObject,
                                                TargetFileName,
                                                Me.MaxFileExports,
                                                Me.CurrentFileExports)

            If Me.CurrentFileExports >= Me.MaxFileExports Then
                RaiseEvent AllFilesExportedSuccessfully()
            End If
        End If
    End Sub

#End Region

#Region "Shared File-Export Functions"
    ''' <summary>
    ''' Saves the a Spectroscopy-Table to Ascii.
    ''' </summary>
    Public Shared Function ExportAscii(ByRef SpectroscopyTable As cSpectroscopyTable,
                                       ByRef ExportMethod As iExportMethod_Ascii) As String

        Return ExportMethod.GetExportOutput(SpectroscopyTable)
    End Function

    ''' <summary>
    ''' Exports a 2D-Data-Matrix to a WSxM-Binary File (.stp) - e.g. for Line-Scans
    ''' </summary>
    Public Shared Sub Export2DMatrixToWSxM(ByVal TargetFile As String,
                                           ByRef ValueMatrix As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix,
                                           Optional ByVal XValueRange As Double = 1,
                                           Optional ByVal XUnitSymbol As String = "V",
                                           Optional ByVal YValueRange As Double = 1,
                                           Optional ByVal YUnitSymbol As String = "V",
                                           Optional ByVal ZValueRange As Double = 1,
                                           Optional ByVal ZUnitSymbol As String = "V")

        Try
            '  // ** Open file; **
            Dim fsA As New System.IO.FileStream(TargetFile, IO.FileMode.Create, IO.FileAccess.Write)

            ' Open StreamWriter in Ascii-Mode
            Dim sw As New System.IO.StreamWriter(fsA, System.Text.Encoding.Default)
            Dim sHeaderBuilder As New StringBuilder

            '  // ** Starting full header with preheader; **
            sHeaderBuilder.AppendLine("WSxM file copyright Nanotec Electronica")
            sHeaderBuilder.AppendLine("SxM Image file")

            sHeaderBuilder.AppendLine("Image header size: %HeaderSize%")

            '  // ** Building postheader; **
            '  strcpy( s_postheader, "\n\n[Control]\n\n" );
            sHeaderBuilder.AppendLine("")
            sHeaderBuilder.AppendLine("[Control]")
            sHeaderBuilder.AppendLine("")
            '  strcat( s_postheader, " Signal Gain: 1\n" );
            sHeaderBuilder.AppendLine(" Signal Gain: 1")
            '  strcat( s_postheader, " Set Point: " );
            '  strcat( s_postheader, p_stmobject->GetTechData( DRQ_FEEDBACKCURRENT, &d_values ) );
            '  strcat( s_postheader, " A\n" );
            sHeaderBuilder.AppendLine(" Set Point: 0 A")
            '  strcat( s_postheader, " Topography Bias: " );
            '  strcat( s_postheader, p_stmobject->GetTechData( DRQ_BIASVOLT, &i_values ) );
            '  strcat( s_postheader, " mV\n" );
            sHeaderBuilder.AppendLine(" Topography Bias: 0 mV")
            '  strcat( s_postheader, " X Amplitude: " );
            '  strcat( s_postheader, p_stmobject->GetTechData( DRQ_DISTX, &d_values ) );
            '  strcat( s_postheader, " Å\n" );
            sHeaderBuilder.AppendLine(" X Amplitude: " & XValueRange & " " & XUnitSymbol)
            '  strcat( s_postheader, " Y Amplitude: " );
            '  strcat( s_postheader, p_stmobject->GetTechData( DRQ_DISTY, &d_values ) );
            '  strcat( s_postheader, " Å\n" );
            sHeaderBuilder.AppendLine(" Y Amplitude: " & YValueRange & " " & YUnitSymbol)
            '  strcat( s_postheader, " Z Gain: 1\n\n" );
            sHeaderBuilder.AppendLine(" Z Gain: 1")
            sHeaderBuilder.AppendLine("")
            '  strcat( s_postheader, "[General Info]\n\n" );
            sHeaderBuilder.AppendLine("[General Info]")
            sHeaderBuilder.AppendLine("")
            '  strcat( s_postheader, " Head type: STM\n" );
            sHeaderBuilder.AppendLine(" Head type: STM")
            '  strcat( s_postheader, " Image Data Type: double\n" );
            sHeaderBuilder.AppendLine(" Image Data Type: double")
            '  strcat( s_postheader, " Number of columns: " );
            '  strcat( s_postheader, p_stmobject->GetTechData( DRQ_PIXELSX, &i_values ) );
            sHeaderBuilder.AppendLine(" Number of columns: " & ValueMatrix.ColumnCount)
            '  strcat( s_postheader, "\n Number of rows: " );
            '  strcat( s_postheader, p_stmobject->GetTechData( DRQ_PIXELSY, &i_values ) );
            sHeaderBuilder.AppendLine(" Number of rows: " & ValueMatrix.RowCount)
            '  strcat( s_postheader, "\n Z Amplitude: " );
            '  strcat( s_postheader, p_stmobject->GetTechData( DRQ_DISTZ, &d_values ) );
            '  strcat( s_postheader, " Å\n\n" );
            sHeaderBuilder.AppendLine(" Z Amplitude: " & ZValueRange & " " & ZUnitSymbol)
            sHeaderBuilder.AppendLine("")
            '  strcat( s_postheader, "[Head Settings]\n\n" );
            sHeaderBuilder.AppendLine("[Head Settings]")
            sHeaderBuilder.AppendLine("")
            '  strcat( s_postheader, " Preamp Gain: " );
            '  strcat( s_postheader, i_gain );
            '  strcat( s_postheader, " mV/nA\n" );
            sHeaderBuilder.AppendLine(" Preamp Gain: 0 mV/nA")
            '  strcat( s_postheader, " X Calibration: 1 Å/V\n" );
            sHeaderBuilder.AppendLine(" X Calibration: 1 Å/V")
            '  strcat( s_postheader, " Z Calibration: 1 Å/V\n\n" );
            sHeaderBuilder.AppendLine(" Z Calibration: 1 Å/V")
            sHeaderBuilder.AppendLine("")
            '  strcat( s_postheader, "[Header end]\n" );
            sHeaderBuilder.AppendLine("[Header end]")

            ' Replace Header-Length
            sHeaderBuilder.Replace("%HeaderSize%", (sHeaderBuilder.Length - 12 + (sHeaderBuilder.Length - 12).ToString.Length).ToString)

            ' Write Header
            sw.Write(sHeaderBuilder)

            ' Close the file in ASCII Mode and reopen it in Binary-Mode:
            sw.Close()
            sw.Dispose()
            fsA.Dispose()

            Dim fsB As New System.IO.FileStream(TargetFile, IO.FileMode.Append, IO.FileAccess.Write)
            Dim bw As New System.IO.BinaryWriter(fsB, System.Text.Encoding.Default)

            ' WRITE DATA AS BINARY Values
            Dim WriteBuffer As Byte()

            For Y As Integer = ValueMatrix.RowCount - 1 To 0 Step -1
                For X As Integer = 0 To ValueMatrix.ColumnCount - 1 Step 1
                    WriteBuffer = BitConverter.GetBytes(ValueMatrix(Y, X))
                    bw.Write(WriteBuffer)
                Next
            Next

            ' Close Binary-Stream and File-Stream
            bw.Close()
            bw.Dispose()
            fsB.Dispose()
        Catch ex As Exception
            MessageBox.Show("WSxM 2D Matrix Export failed: " & vbCrLf & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

End Class
