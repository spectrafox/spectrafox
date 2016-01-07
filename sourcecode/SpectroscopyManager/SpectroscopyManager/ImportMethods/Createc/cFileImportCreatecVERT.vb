Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class cFileImportCreatecVERT
    Implements iFileImport_SpectroscopyTable

    Private Enum STMAFM_Version
        Version3above
        Version2
        Version1
    End Enum

    ''' <summary>
    ''' Imports the Bias-Spectroscopy-File into a Spectroscopy Table
    ''' </summary>
    Public Function ImportBias(ByRef FullFileNamePlusPath As String,
                               ByVal FetchOnlyFileHeader As Boolean,
                               Optional ByRef ReaderBuffer As String = "") As cSpectroscopyTable Implements iFileImport_SpectroscopyTable.ImportSpectroscopyTable
        ' STMAFM-specific Parameters:
        Dim STMAFMVersion As STMAFM_Version
        Dim DACBits As Integer = 0

        Dim GainX As Integer = 0
        Dim GainY As Integer = 0
        Dim GainZ As Integer = 0
        Dim GainPreamplifier As Integer = 0

        Dim XPiezoConst As Double = 0
        Dim YPiezoConst As Double = 0
        Dim ZPiezoConst As Double = 0

        Dim NumberOfCols As Integer = 0
        Dim ChannelListEncoding As Integer = 0
        Dim DACToXYConversionFactor As Double = 0
        Dim DACToZConversionFactor As Double = 0
        Dim IVConversionfactorADCToV As Double = 0
        Dim IConversionfactorADCToV As Double = 0

        Dim ScanOffset_X As Double = 0
        Dim ScanOffset_Y As Double = 0
        Dim ScanRange_X As Double = 0
        Dim ScanRange_Y As Double = 0

        ' Create New SpectroscopyTable
        Dim oSpectroscopyTable As New cSpectroscopyTable
        oSpectroscopyTable.FullFileName = FullFileNamePlusPath

        ' Load StreamReader
        Dim sr As New StreamReader(FullFileNamePlusPath)

        ' Read File Line-By-Line and Export Settings
        ' and Data. Write the Splitted Values in a String-Array
        ReaderBuffer = ""
        'Dim ReaderBuffer As String = ""
        Dim SplittedLine As String()

        ' Read first line to get the STMAFM-Version:
        ReaderBuffer = sr.ReadLine

        ' VERT files contain in first row "[ParVERT30]" in Version 3 and
        ' "[Parameter]" in older Versions
        If ReaderBuffer.Contains("[ParVERT30]") Then
            STMAFMVersion = STMAFM_Version.Version3above
        ElseIf ReaderBuffer.Contains("[Parameter]") Then
            STMAFMVersion = STMAFM_Version.Version2
        Else
            ' If none of these is found return the empty Table
            Return oSpectroscopyTable
        End If

        ' Read Settings up to the 128x128 bytes
        ' which in the end includes the "DATA" tag, that says the next lines coming
        ' are spectroscopy-data.
        Do Until sr.EndOfStream Or sr.BaseStream.Position >= 16384
            ReaderBuffer = sr.ReadLine

            ' Before Splitting the parameters at "="
            ' check, if the line contains comments:
            If ReaderBuffer.StartsWith("memo:") Then
                oSpectroscopyTable.Comment &= ReaderBuffer.Substring(5, ReaderBuffer.Length - 5) & vbCrLf
                Continue Do
            End If

            SplittedLine = Split(ReaderBuffer, "=", 2)
            If SplittedLine.Length <> 2 Then Continue Do
            Dim sPropertyName As String = SplittedLine(0).Trim
            Dim sPropertyValue As String = SplittedLine(1).Trim

            With oSpectroscopyTable
                ' Other Properties
                Select Case sPropertyName
                    Case "DAC-Type"
                        ' Remove "bit" at end of value
                        sPropertyValue = sPropertyValue.Remove(sPropertyValue.Length - 3, 3)
                        DACBits = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "GainX / GainX"
                        GainX = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "GainY / GainY"
                        GainY = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "GainZ / GainZ"
                        GainZ = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Scanrotoffx / OffsetX"
                        ScanOffset_X = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Scanrotoffy / OffsetY"
                        ScanOffset_Y = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Biasvolt[mV]"
                        .FeedbackOpenBias_V = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Current[A]"
                        .FeedbackOpenCurrent_A = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Vertmangain"
                        GainPreamplifier = Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Vertmandelay"
                        ' ** Time interval between two vert man actions in 20 musec units; **
                        .SettlingTime_s = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture) * 20 * Math.Pow(10, -6)
                    Case "XPiezoconst"
                        XPiezoConst = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "YPiezoconst"
                        YPiezoConst = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "ZPiezoconst"
                        ZPiezoConst = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Dacto[A]xy"
                        DACToXYConversionFactor = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Dacto[A]z"
                        DACToZConversionFactor = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "VertSpecBack"
                        .Backward_Sweep = Convert.ToBoolean(Integer.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture))
                    Case "Length x[A]"
                        ScanRange_X = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "Length y[A]"
                        ScanRange_Y = Double.Parse(sPropertyValue, Globalization.CultureInfo.InvariantCulture)
                    Case "memo"
                        .Comment &= sPropertyValue & vbCrLf
                End Select
            End With
        Loop

        ' After the Header ended (128x128 bytes), check one last time, if the last line contained the "DATA" tag.
        Do Until sr.EndOfStream Or ReaderBuffer.Contains("DATA")
            ReaderBuffer = sr.ReadLine
        Loop

        ' In the STMAFM-VERT files the first row has 3 numbers (+ 1 above Vers3)
        ' with statistical data of the spectrum:
        ' #1 Number of Points taken during Vertical Manipulation
        ' #2 X Position of the Spectrum
        ' #3 Y Position of the Spectrum
        ' (above Vers3: #4 Number of custom DataColumns)
        ReaderBuffer = sr.ReadLine

        ' Using Regular-Expression to get Version-Specific Parameters
        Dim oRegExVers3 As New Regex("(?<Param1>[-\d]*)\s+(?<Param2>[-\d]+)\s+(?<Param3>[-\d]+)\s+(?<Param4>[-\d]+)", RegexOptions.Compiled)
        Dim oMatchVers3 As Match = oRegExVers3.Match(ReaderBuffer.Trim)
        Dim oRegExVers2 As New Regex("(?<Param1>[-\d]*)\s+(?<Param2>[-\d]+)\s+(?<Param3>[-\d]+)", RegexOptions.Compiled)
        Dim oMatchVers2 As Match = oRegExVers2.Match(ReaderBuffer.Trim)

        Dim DataPoints As Integer = 0
        ' Extract Version 2 Settings
        If oMatchVers3.Success Then
            If STMAFMVersion = STMAFM_Version.Version2 Then If STMAFMVersion = STMAFM_Version.Version3above Then Throw New Exception("Statistics-Row in VERT-File is from Version 3, but file seemed to be Version <= 2. Perhaps a corrupt file? (File: " & oSpectroscopyTable.FullFileName & ")")
            DataPoints = Integer.Parse(oMatchVers3.Groups("Param1").Value, Globalization.CultureInfo.InvariantCulture)
            ' Save first in DAC-Units
            ' Recalculation NEEDED!!!
            oSpectroscopyTable.Location_X = Integer.Parse(oMatchVers3.Groups("Param2").Value, Globalization.CultureInfo.InvariantCulture)
            oSpectroscopyTable.Location_Y = Integer.Parse(oMatchVers3.Groups("Param3").Value, Globalization.CultureInfo.InvariantCulture)
            ChannelListEncoding = Integer.Parse(oMatchVers3.Groups("Param4").Value, Globalization.CultureInfo.InvariantCulture)
        ElseIf oMatchVers2.Success Then
            If STMAFMVersion = STMAFM_Version.Version3above Then Throw New Exception("Statistics-Row in VERT-File is from Version 2, but file seemed to be Version >= 3. Perhaps a corrupt file? (File: " & oSpectroscopyTable.FullFileName & ")")
            DataPoints = Integer.Parse(oMatchVers2.Groups("Param1").Value, Globalization.CultureInfo.InvariantCulture)
            ' Save first in DAC-Units
            ' Recalculation NEEDED!!!
            oSpectroscopyTable.Location_X = Integer.Parse(oMatchVers2.Groups("Param2").Value, Globalization.CultureInfo.InvariantCulture)
            oSpectroscopyTable.Location_Y = Integer.Parse(oMatchVers2.Groups("Param3").Value, Globalization.CultureInfo.InvariantCulture)
        Else
            Throw New Exception("Statistic Column of File " & oSpectroscopyTable.FullFileName & " could not be identified!")
        End If

        ' Convert X and Y position to real X and Y:
        ScanOffset_X *= DACToXYConversionFactor * GainX * 0.0000000001
        ScanOffset_Y *= DACToXYConversionFactor * GainY * 0.0000000001
        oSpectroscopyTable.Location_X *= DACToXYConversionFactor * GainX * 0.0000000001 ' -DACTOXY... (minus) because of image-coordinate markers in the scan-image
        oSpectroscopyTable.Location_Y *= -DACToXYConversionFactor * GainY * 0.0000000001
        oSpectroscopyTable.Location_X += ScanOffset_X
        oSpectroscopyTable.Location_Y += ScanOffset_Y

        '  // ** Get number of columns and variables to describe the version
        '  // ** dependent position (= column number) of each data type; **
        '  // ** (-1 means: Data type does not exist; )**
        Dim posI As Integer = -1
        Dim posV As Integer = -1
        Dim posInternalIV As Integer = -1
        Dim posIV As Integer = -1
        Dim posIIVV As Integer = -1
        Dim posADC3 As Integer = -1
        Dim posDF As Integer = -1
        Dim posEXC As Integer = -1
        Dim posAMP As Integer = -1
        Dim posZ As Integer = -1
        Dim posTopo As Integer = -1

        ' Read next row for real Column-Count:
        ReaderBuffer = sr.ReadLine
        SplittedLine = Split(ReaderBuffer.Trim, vbTab)

        ' Depending on Version determine Column-Positions
        Select Case STMAFMVersion
            Case STMAFM_Version.Version3above
                ' Set permanent Channel-Positions
                posV = 1
                posZ = 2

                NumberOfCols = 3

                ' In Version 3, the Channel-List has to be decoded:
                ' Current = 1
                ' dI/dV   = 2
                ' d2I/dV2 = 4
                ' ADC0    = 8
                ' ADC1    = 16
                ' ADC2    = 32
                ' ADC3    = 64
                '         = 128
                '         = 256
                '         = 512
                ' di_q    = 1024
                ' di2_q   = 2048
                ' Top DAC0= 4096

                ' Compare if the Channel exist by a Bitwise And-Coupling
                Dim ChannelExist(13) As Boolean
                Dim PowerOf2 As Integer = 1
                For b As Integer = 0 To 12 Step 1
                    If (ChannelListEncoding And PowerOf2) > 0 Then
                        ChannelExist(b) = True
                        NumberOfCols += 1
                    Else
                        ChannelExist(b) = False
                    End If

                    PowerOf2 *= 2
                Next

                '// ** Set I channel position; **
                If ChannelExist(0) Then posI = 3

                '// ** Set internal IV (dI/dV) channel position; **
                If ChannelExist(1) Then
                    posInternalIV = 3
                    If ChannelExist(0) Then posInternalIV += 1
                End If

                '// ** Set IV (ADC 1) channel position; **
                If ChannelExist(4) Then
                    posIV = 3
                    For b As Integer = 0 To 3 Step 1
                        If ChannelExist(b) Then posIV += 1
                    Next
                End If

                '// ** Set IIVV (ADC 2) channel position; **
                If ChannelExist(5) Then
                    posIIVV = 3
                    For b As Integer = 0 To 4 Step 1
                        If ChannelExist(b) Then posIIVV += 1
                    Next
                End If

                '// ** Set ADC3 (ADC 3) channel position; **
                If ChannelExist(6) Then
                    posADC3 = 3
                    For b As Integer = 0 To 5 Step 1
                        If ChannelExist(b) Then posADC3 += 1
                    Next
                End If

                '// ** Set DF channel position; **
                If ChannelExist(7) Then
                    posDF = 3
                    For b As Integer = 0 To 6 Step 1
                        If ChannelExist(b) Then posDF += 1
                    Next
                End If

                '// ** Set EXC channel position; **
                If ChannelExist(8) Then
                    posEXC = 3
                    For b As Integer = 0 To 7 Step 1
                        If ChannelExist(b) Then posEXC += 1
                    Next
                End If

                '// ** Set AMP channel position; **
                If ChannelExist(9) Then
                    posAMP = 3
                    For b As Integer = 0 To 8 Step 1
                        If ChannelExist(b) Then posAMP += 1
                    Next
                End If

                '// ** Set Topography (DAC 0) channel position; **
                If ChannelExist(12) Then
                    posTopo = 3
                    For b As Integer = 0 To 11 Step 1
                        If ChannelExist(b) Then posTopo += 1
                    Next
                End If

            Case STMAFM_Version.Version2
                '// ** Set the permanent channel positions; **
                posI = 1
                posIV = 2
                posV = 3
                posZ = 4
                posIIVV = 5

                '// ** Testwise read the first line to detect how many arguments are in there; **
                ' We did this already -> Count Columns
                NumberOfCols = SplittedLine.Length

                ' Detect specific STMAFM Version
                If NumberOfCols = 8 Then
                    STMAFMVersion = STMAFM_Version.Version1
                ElseIf NumberOfCols = 13 Then
                    STMAFMVersion = STMAFM_Version.Version2
                End If

                '// ** Set Topography (DAC 0) channel position; **
                If STMAFMVersion = STMAFM_Version.Version2 Then posTopo = 12
        End Select

        '################
        ' Create Columns
        '################
        Dim lSortedColumnList As New SortedDictionary(Of Integer, cSpectroscopyTable.DataColumn)

        If oSpectroscopyTable.Backward_Sweep Then
            oSpectroscopyTable.MeasurementPoints = Convert.ToInt32(DataPoints / 2)
        Else
            oSpectroscopyTable.MeasurementPoints = DataPoints
        End If

        If posI <> -1 Then
            Dim ColI As New cSpectroscopyTable.DataColumn
            With ColI
                ' Save Column-Name
                .Name = "Current"
                ' Save Column-Unit
                .UnitSymbol = "A"
                .UnitType = cUnits.UnitType.Current
            End With
            lSortedColumnList.Add(posI, ColI)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColIBW As New cSpectroscopyTable.DataColumn
                With ColIBW
                    ' Save Column-Name
                    .Name = "Current BW"
                    ' Save Column-Unit
                    .UnitSymbol = "A"
                    .UnitType = cUnits.UnitType.Current
                End With
                lSortedColumnList.Add(posI + 100, ColIBW)
            End If
        End If

        If posInternalIV <> -1 Then
            Dim ColIV As New cSpectroscopyTable.DataColumn
            With ColIV
                ' Save Column-Name
                .Name = "internal dI/dV"
                ' Save Column-Unit
                .UnitSymbol = "S"
                .UnitType = cUnits.UnitType.Conductance
            End With
            lSortedColumnList.Add(posInternalIV, ColIV)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColIVBW As New cSpectroscopyTable.DataColumn
                With ColIVBW
                    ' Save Column-Name
                    .Name = "internal dI/dV BW"
                    ' Save Column-Unit
                    .UnitSymbol = "S"
                    .UnitType = cUnits.UnitType.Conductance
                End With
                lSortedColumnList.Add(posInternalIV + 100, ColIVBW)
            End If
        End If

        If posIV <> -1 Then
            Dim ColIV As New cSpectroscopyTable.DataColumn
            With ColIV
                ' Save Column-Name
                .Name = "dI/dV"
                ' Save Column-Unit
                .UnitSymbol = "S"
                .UnitType = cUnits.UnitType.Conductance
            End With
            lSortedColumnList.Add(posIV, ColIV)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColIVBW As New cSpectroscopyTable.DataColumn
                With ColIVBW
                    ' Save Column-Name
                    .Name = "dI/dV BW"
                    ' Save Column-Unit
                    .UnitSymbol = "S"
                    .UnitType = cUnits.UnitType.Conductance
                End With
                lSortedColumnList.Add(posIV + 100, ColIVBW)
            End If
        End If

        If posV <> -1 Then
            Dim ColV As New cSpectroscopyTable.DataColumn
            With ColV
                ' Save Column-Name
                .Name = "Bias"
                ' Save Column-Unit
                .UnitSymbol = "V"
                .UnitType = cUnits.UnitType.Voltage
            End With
            lSortedColumnList.Add(posV, ColV)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColVBW As New cSpectroscopyTable.DataColumn
                With ColVBW
                    ' Save Column-Name
                    .Name = "Bias BW"
                    ' Save Column-Unit
                    .UnitSymbol = "V"
                    .UnitType = cUnits.UnitType.Voltage
                End With
                lSortedColumnList.Add(posV + 100, ColVBW)
            End If
        End If

        If posZ <> -1 Then
            Dim ColZ As New cSpectroscopyTable.DataColumn
            With ColZ
                ' Save Column-Name
                .Name = "Z-Position"
                ' Save Column-Unit
                .UnitSymbol = "m"
                .UnitType = cUnits.UnitType.Length
            End With
            lSortedColumnList.Add(posZ, ColZ)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColZBW As New cSpectroscopyTable.DataColumn
                With ColZBW
                    ' Save Column-Name
                    .Name = "Z-Position BW"
                    ' Save Column-Unit
                    .UnitSymbol = "m"
                    .UnitType = cUnits.UnitType.Length
                End With
                lSortedColumnList.Add(posZ + 100, ColZBW)
            End If
        End If

        If posIIVV <> -1 Then
            Dim ColIIVV As New cSpectroscopyTable.DataColumn
            With ColIIVV
                ' Save Column-Name
                .Name = "d/dV dI/dV"
                ' Save Column-Unit
                .UnitSymbol = "S/V"
                .UnitType = cUnits.UnitType.ConductanceDeriv
            End With
            lSortedColumnList.Add(posIIVV, ColIIVV)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColIIVVBW As New cSpectroscopyTable.DataColumn
                With ColIIVVBW
                    ' Save Column-Name
                    .Name = "d/dV dI/dV BW"
                    ' Save Column-Unit
                    .UnitSymbol = "S/V"
                    .UnitType = cUnits.UnitType.ConductanceDeriv
                End With
                lSortedColumnList.Add(posIIVV + 100, ColIIVVBW)
            End If
        End If

        If posADC3 <> -1 Then
            Dim ColADC3 As New cSpectroscopyTable.DataColumn
            With ColADC3
                ' Save Column-Name
                .Name = "ADC 3"
                ' Save Column-Unit
                .UnitSymbol = "S/V"
                .UnitType = cUnits.UnitType.ConductanceDeriv
            End With
            lSortedColumnList.Add(posADC3, ColADC3)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColADC3BW As New cSpectroscopyTable.DataColumn
                With ColADC3BW
                    ' Save Column-Name
                    .Name = "ADC 3 BW"
                    ' Save Column-Unit
                    .UnitSymbol = "S/V"
                    .UnitType = cUnits.UnitType.ConductanceDeriv
                End With
                lSortedColumnList.Add(posADC3 + 100, ColADC3BW)
            End If
        End If

        If posDF <> -1 Then
            Dim ColDF As New cSpectroscopyTable.DataColumn
            With ColDF
                ' Save Column-Name
                .Name = "DF"
                ' Save Column-Unit
                .UnitSymbol = "Hz"
                .UnitType = cUnits.UnitType.Frequency
            End With
            lSortedColumnList.Add(posDF, ColDF)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColDFBW As New cSpectroscopyTable.DataColumn
                With ColDFBW
                    ' Save Column-Name
                    .Name = "DF BW"
                    ' Save Column-Unit
                    .UnitSymbol = "Hz"
                    .UnitType = cUnits.UnitType.Frequency
                End With
                lSortedColumnList.Add(posDF + 100, ColDFBW)
            End If
        End If

        If posEXC <> -1 Then
            Dim ColEXC As New cSpectroscopyTable.DataColumn
            With ColEXC
                ' Save Column-Name
                .Name = "EXC"
                ' Save Column-Unit
                .UnitSymbol = "V"
                .UnitType = cUnits.UnitType.Voltage
            End With
            lSortedColumnList.Add(posEXC, ColEXC)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColEXCBW As New cSpectroscopyTable.DataColumn
                With ColEXCBW
                    ' Save Column-Name
                    .Name = "EXC BW"
                    ' Save Column-Unit
                    .UnitSymbol = "V"
                    .UnitType = cUnits.UnitType.Voltage
                End With
                lSortedColumnList.Add(posEXC + 100, ColEXCBW)
            End If
        End If

        If posAMP <> -1 Then
            Dim ColAMP As New cSpectroscopyTable.DataColumn
            With ColAMP
                ' Save Column-Name
                .Name = "AMP"
                ' Save Column-Unit
                .UnitSymbol = "V"
                .UnitType = cUnits.UnitType.Voltage
            End With
            lSortedColumnList.Add(posAMP, ColAMP)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColAMPBW As New cSpectroscopyTable.DataColumn
                With ColAMPBW
                    ' Save Column-Name
                    .Name = "AMP BW"
                    ' Save Column-Unit
                    .UnitSymbol = "V"
                    .UnitType = cUnits.UnitType.Voltage
                End With
                lSortedColumnList.Add(posAMP + 100, ColAMPBW)
            End If
        End If

        If posTopo <> -1 Then
            Dim ColTopo As New cSpectroscopyTable.DataColumn
            With ColTopo
                ' Save Column-Name
                .Name = "Topography"
                ' Save Column-Unit
                .UnitSymbol = "m"
                .UnitType = cUnits.UnitType.Length
            End With
            lSortedColumnList.Add(posTopo, ColTopo)

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColTopoBW As New cSpectroscopyTable.DataColumn
                With ColTopoBW
                    ' Save Column-Name
                    .Name = "Topography BW"
                    ' Save Column-Unit
                    .UnitSymbol = "m"
                    .UnitType = cUnits.UnitType.Length
                End With
                lSortedColumnList.Add(posTopo + 100, ColTopoBW)
            End If
        End If

        Dim ColRowNumber As New cSpectroscopyTable.DataColumn
        With ColRowNumber
            ' Save Column-Name
            .Name = My.Resources.ColumnName_MeasurementPoints
            ' Save Column-Unit
            .UnitSymbol = "1"
            .UnitType = cUnits.UnitType.Unitary
        End With
        lSortedColumnList.Add(NumberOfCols + 1, ColRowNumber)

        '// ** Calculate conversion factor facIV from ADC0-units to V; **
        '// ** facIV = C_ADC0, see file "STMAFM_Data_Conversion.ppt"; **
        ' 20 V divided by the 2^DAC-Bit-Number -Channels
        IVConversionfactorADCToV = 20.0 / Math.Pow(2, DACBits)

        '// ** Calculate conversion factor facI from ADC0-units to current I in Amperes; **
        '// ** facI = C_ADC0 / 10^Gain, see file "STMAFM_Data_Conversion.ptt"; **
        IConversionfactorADCToV = 20.0 / Math.Pow(2, DACBits) / Math.Pow(10, GainPreamplifier - 12) * Math.Pow(10, -12)

        ' Read Spectroscopy-Data
        If Not FetchOnlyFileHeader Then
            Dim iRowCounter As Integer = 1
            Dim Value As Double
            Do
                If iRowCounter <> 1 Then
                    ReaderBuffer = sr.ReadLine
                    SplittedLine = Split(ReaderBuffer, vbTab)
                End If

                For j As Integer = 0 To SplittedLine.Length - 1 Step 1
                    ' Save Spectroscopy-Values.
                    If Not Double.TryParse(SplittedLine(j), NumberStyles.Float, CultureInfo.InvariantCulture, Value) Then
                        Value = 0
                    End If

                    ' This code has been too slow
                    'Dim Value As Double = 0
                    'If IsNumeric(SplittedLine(j)) Then
                    '    Value = Double.Parse(SplittedLine(j), Globalization.CultureInfo.InvariantCulture)
                    'End If

                    Select Case j
                        Case posI
                            Value *= IConversionfactorADCToV
                        Case posInternalIV
                            Value *= IVConversionfactorADCToV
                        Case posIV
                            Value *= IVConversionfactorADCToV
                        Case posIIVV
                            Value *= IVConversionfactorADCToV
                        Case posAMP
                            Value *= IVConversionfactorADCToV
                        Case posEXC
                            Value *= IVConversionfactorADCToV
                        Case posADC3
                            Value *= IVConversionfactorADCToV
                        Case posV
                            ' Since value is given in mV
                            Value *= 0.001
                        Case posZ
                            Value *= ZPiezoConst / 1000 * 0.0000000001
                        Case posTopo
                            ' If a topography column is present, the 
                            ' Feedback was switched on during Manipulation.
                            ' Therefore replace Z-Values.
                            Value *= -DACToZConversionFactor * GainZ * 0.0000000001
                    End Select

                    Dim b As Integer = j
                    'If b = posTopo Then
                    '    ' If a topography column is present, the 
                    '    ' Feedback was switched on during Manipulation.
                    '    ' Therefore replace Z-Values.
                    '    b = posZ
                    'End If

                    ' Sort Backward-Sweep in separate Column.
                    If oSpectroscopyTable.Backward_Sweep And iRowCounter > oSpectroscopyTable.MeasurementPoints Then
                        b += 100
                    End If

                    If lSortedColumnList.ContainsKey(b) Then
                        lSortedColumnList(b).AddValueToColumn(Value)
                    End If
                Next

                ' Add the row number to the separate column
                ' if we are not in the backward sweep!
                If iRowCounter <= oSpectroscopyTable.MeasurementPoints Then
                    lSortedColumnList(NumberOfCols + 1).AddValueToColumn(iRowCounter)
                End If

                iRowCounter += 1
            Loop Until sr.EndOfStream
        End If

        sr.Close()
        sr.Dispose()

        If posI <> -1 And posV <> -1 Then

            '#################
            ' Forward channel
            Dim ColG As New cSpectroscopyTable.DataColumn
            With ColG
                ' Save Column-Name
                .Name = "Conductivity in G0-Units"
                ' Save Column-Unit
                .UnitSymbol = "S"
                .UnitType = cUnits.UnitType.Conductance
            End With
            lSortedColumnList.Add(NumberOfCols, ColG)

            Dim ValuesI As ObjectModel.ReadOnlyCollection(Of Double) = lSortedColumnList(posI).Values(True)
            Dim ValuesV As ObjectModel.ReadOnlyCollection(Of Double) = lSortedColumnList(posV).Values(True)
            Dim OutputValues As New List(Of Double)(ValuesI.Count)

            For j As Integer = 0 To ValuesI.Count - 1 Step 1
                OutputValues.Add(ValuesI(j) / ValuesV(j) / cConstants.G0_SI)
            Next
            'lSortedColumnList(NumberOfCols).SetValueList(OutputValues)
            ColG.SetValueList(OutputValues)

            '##################
            ' Backward channel

            If oSpectroscopyTable.Backward_Sweep Then
                Dim ColGBW As New cSpectroscopyTable.DataColumn
                With ColGBW
                    ' Save Column-Name
                    .Name = "Conductivity in G0-Units - BW"
                    ' Save Column-Unit
                    .UnitSymbol = "S"
                    .UnitType = cUnits.UnitType.Conductance
                End With
                lSortedColumnList.Add(NumberOfCols + 100, ColGBW)

                ValuesI = lSortedColumnList(posI + 100).Values(True)
                ValuesV = lSortedColumnList(posV + 100).Values(True)
                OutputValues.Clear()

                For j As Integer = 0 To ValuesI.Count - 1 Step 1
                    OutputValues.Add(ValuesI(j) / ValuesV(j) / cConstants.G0_SI)
                Next

                ColGBW.SetValueList(OutputValues)
                'lSortedColumnList(NumberOfCols + 100).SetValueList(OutputValuesBW)
            End If
        End If

        ' Finish writing Properties
        If posV <> -1 Then
            If lSortedColumnList(posV).Values.Count > 0 Then
                oSpectroscopyTable.BiasSpec_SweepStart_V = lSortedColumnList(posV).Values(True).Item(0)
                oSpectroscopyTable.BiasSpec_SweepEnd_V = lSortedColumnList(posV).Values(True).Item(oSpectroscopyTable.MeasurementPoints - 1)
            End If
        End If
        If posTopo <> -1 Then
            oSpectroscopyTable.FeedbackOff = False
        Else
            oSpectroscopyTable.FeedbackOff = True
        End If
        oSpectroscopyTable.Bias_Calibration_V_V = 1
        oSpectroscopyTable.NumberOfSweeps = 1
        oSpectroscopyTable.RecordDate = IO.File.GetLastWriteTime(FullFileNamePlusPath)

        ' Finally add all Columns from the Temporary List to the Spectroscopy-Table
        Dim i As Integer = 0
        For Each oColumn As cSpectroscopyTable.DataColumn In lSortedColumnList.Values
            oColumn.IsSpectraFoxGenerated = False
            oSpectroscopyTable.AddNonPersistentColumn(oColumn)
            i += 1
        Next

        ' File Exists, so Set the Property.
        oSpectroscopyTable._bFileExists = True

        Return oSpectroscopyTable
    End Function

    ''' <summary>
    ''' File-Extension
    ''' </summary>
    Public ReadOnly Property FileExtension As String Implements iFileImport_SpectroscopyTable.FileExtension
        Get
            Return ".vert"
        End Get
    End Property

    ''' <summary>
    ''' Checks, if the given File is a known Createc VERT-File-Type
    ''' </summary>
    Public Function IdentifyFile(ByRef FullFileNamePlusPath As String,
                                 Optional ByRef ReaderBuffer As String = "") As Boolean Implements iFileImport_SpectroscopyTable.IdentifyFile
        ' Load StreamReader and Read first line.
        ' Is the only one needed for Identification.
        Dim sr As New StreamReader(FullFileNamePlusPath)
        ReaderBuffer = sr.ReadLine
        sr.Close()
        sr.Dispose()

        ' VERT files contain in first row "[ParVERT30]" in Version 3 and
        ' "[Parameter]" in older Versions
        If ReaderBuffer.Contains("[ParVERT30]") Or
           ReaderBuffer.Contains("[Parameter]") Then
            Return True
        End If

        Return False
    End Function

End Class
