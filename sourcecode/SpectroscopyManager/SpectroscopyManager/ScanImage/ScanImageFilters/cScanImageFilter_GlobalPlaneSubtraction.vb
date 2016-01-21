Public Class cScanImageFilter_GlobalPlaneSubtraction
    Implements iScanImageFilter

    ''' <summary>
    ''' Applies a Global Plane Subtraction to the Scan-Channel
    ''' </summary>
    Public Function ApplyFilter(ByRef ScanChannel As cScanImage.ScanChannel) As cScanImage.ScanChannel Implements iScanImageFilter.ApplyFilter

        ' Measure the time needed for the filter.
        Dim FilterTimer As Stopwatch = Stopwatch.StartNew()

        '// ********************************************************************
        '// * Applies a global plane subtraction for an STM topological image; *
        '// ********************************************************************

        Dim XCount As Integer = ScanChannel.ScanData.ColumnCount
        Dim YCount As Integer = ScanChannel.ScanData.RowCount

        Dim YStart As Integer = ScanChannel.GetFirstRowWithoutNaN
        Dim YEnd As Integer = ScanChannel.GetLastRowWithoutNaN
        Dim XStart As Integer = ScanChannel.GetFirstColumnWithoutNaN
        Dim XEnd As Integer = ScanChannel.GetLastColumnWithoutNaN

        ' Data checks. We can apply the filter e.g. only, if enough data points are present.
        ' We need at least 4 quadrants, so at least 2x2 pixels!
        If (YEnd - YStart) < 4 OrElse (XEnd - XStart) < 4 Then
            Return ScanChannel
        End If

        '  // ** Calculate global plane gradients in x and y direction; **
        Dim dGradientX As Double = 0
        Dim dGradientY As Double = 0
        Dim dQuadrantAverage As Double(,) = New Double(1, 1) {}
        Dim iNum As Integer(,) = New Integer(1, 1) {}
        Dim QuadrantCenterX As Integer(,) = New Integer(1, 1) {}
        Dim QuadrantCenterY As Integer(,) = New Integer(1, 1) {}
        '    // ** Calculate average of every of the four quadrants of the image; **
        For Xquad As Integer = 0 To 1 Step 1
            '      for( int yps = 0; yps < 2; yps++ )
            For Yquad As Integer = 0 To 1 Step 1
                Dim XMin As Integer = XStart + CInt(Xquad * (XEnd - XStart) / 2)
                Dim XMax As Integer = XStart + CInt((Xquad + 1) * (XEnd - XStart) / 2)
                Dim YMin As Integer = YStart + CInt(Yquad * (YEnd - YStart) / 2)
                Dim YMax As Integer = YStart + CInt((Yquad + 1) * (YEnd - YStart) / 2)
                QuadrantCenterX(Yquad, Xquad) = XMin + CInt((XMax - XMin) / 2)
                QuadrantCenterY(Yquad, Xquad) = YMin + CInt((YMax - YMin) / 2)

                '        d_quadav[yps * 2 + xi] = 0.0;
                '        i_num[yps * 2 + xi] = 0;
                dQuadrantAverage(Yquad, Xquad) = 0
                iNum(Yquad, Xquad) = 0

                '        for( int y = i_ymin; y < i_ymax; y++ )
                For y As Integer = YMin To YMax - 1 Step 1
                    For x As Integer = XMin To XMax - 1 Step 1
                        If Double.IsNaN(ScanChannel.ScanData(y, x)) Then Continue For
                        '            d_quadav[yps * 2 + xi] += p_inData[y * i_inX + x];
                        '            i_num[yps * 2 + xi]++;
                        dQuadrantAverage(Yquad, Xquad) += ScanChannel.ScanData(y, x)
                        iNum(Yquad, Xquad) += 1
                    Next
                Next

                '        d_quadav[yps * 2 + xi] = d_quadav[yps * 2 + xi] / i_num[yps * 2 + xi];
                If iNum(Yquad, Xquad) > 0 Then
                    dQuadrantAverage(Yquad, Xquad) = dQuadrantAverage(Yquad, Xquad) / iNum(Yquad, Xquad)
                Else
                    dQuadrantAverage(Yquad, Xquad) = 0
                End If
            Next
        Next

        '    // ** Calculate global plane gradient in x and y direction; **
        '    d_gx = ( ( d_quadav[1] - d_quadav[0] ) + ( d_quadav[3] - d_quadav[2] ) ) / i_inX;
        '    d_gy = ( ( d_quadav[2] - d_quadav[0] ) + ( d_quadav[3] - d_quadav[1] ) ) / i_inY;
        dGradientX = -(dQuadrantAverage(0, 1) - dQuadrantAverage(0, 0)) + (dQuadrantAverage(1, 1) - dQuadrantAverage(1, 0)) / (QuadrantCenterX(0, 1) - QuadrantCenterX(0, 0)) 'XCount
        dGradientY = -(dQuadrantAverage(1, 0) - dQuadrantAverage(0, 0)) + (dQuadrantAverage(1, 1) - dQuadrantAverage(0, 1)) / (QuadrantCenterY(1, 0) - QuadrantCenterY(0, 0)) 'YCount

        '  // ** Walk the data and apply global plane subtraction; **
        '  for( int x = 0; x < i_inX; x++ )
        '    for( int y = 0; y < i_inY; y++ )
        '      p_outData[y * i_inX + x] = p_inData[y * i_inX + x] - x * d_gx - y * d_gy;
        For x As Integer = 0 To XCount - 1 Step 1
            For y As Integer = 0 To YCount - 1 Step 1
                If Not Double.IsNaN(ScanChannel.ScanData(y, x)) Then
                    ScanChannel.ScanData(y, x) = ScanChannel.ScanData(y, x) - x * dGradientX - y * dGradientY
                Else
                    ScanChannel.ScanData(y, x) = Double.NaN
                End If
            Next
        Next


        FilterTimer.Stop()
        Me.FilterExecutionTime = FilterTimer.Elapsed

        Return ScanChannel
    End Function

    ''' <summary>
    ''' Set to true, because no setup is needed!
    ''' </summary>
    Public Property FilterSetupComplete As Boolean = True Implements iScanImageFilter.FilterSetupComplete

    ''' <summary>
    ''' Returns the time the filter needed for execution.
    ''' </summary>
    Public Property FilterExecutionTime As TimeSpan Implements iScanImageFilter.FilterExecutionTime

    ''' <summary>
    ''' Name of the Filter
    ''' </summary>
    Public ReadOnly Property FilterName As String Implements iScanImageFilter.FilterName
        Get
            Return My.Resources.rScanImageFilters.GlobalPlaneSubstraction_Name
        End Get
    End Property

    ''' <summary>
    ''' Show this filter in the filter list.
    ''' </summary>
    Public ReadOnly Property ShowFilterInFilterMenu As Boolean Implements iScanImageFilter.ShowFilterInFilterMenu
        Get
#If DEBUG Then
            Return False
            'Return True
#Else
            Return False
#End If
        End Get
    End Property

    ''' <summary>
    ''' Does the filter has to be setup in advance?
    ''' </summary>
    Public ReadOnly Property NeedsSetup As Boolean Implements iScanImageFilter.NeedsSetup
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Settings of the filter given as a separate string.
    ''' If set, should use the given settings for the new filter.
    ''' If get, returns the current filter settings as string.
    ''' </summary>
    Public Property FilterSettingsString As String Implements iScanImageFilter.FilterSettingsString


End Class
