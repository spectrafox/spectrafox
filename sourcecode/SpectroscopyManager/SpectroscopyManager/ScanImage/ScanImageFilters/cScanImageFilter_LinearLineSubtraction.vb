Public Class cScanImageFilter_LinearLineSubtraction
    Implements iScanImageFilter

    ''' <summary>
    ''' Applies a line-wise linear regression and
    ''' substracts it from each line of the scan-channel
    ''' </summary>
    Public Function ApplyFilter(ByRef ScanChannel As cScanImage.ScanChannel) As cScanImage.ScanChannel Implements iScanImageFilter.ApplyFilter

        ' Measure the time needed for the filter.
        Dim FilterTimer As Stopwatch = Stopwatch.StartNew()

        Dim XCount As Integer = ScanChannel.ScanData.ColumnCount
        Dim YCount As Integer = ScanChannel.ScanData.RowCount

        Dim XStart As Integer = ScanChannel.GetFirstColumnWithoutNaN
        Dim XEnd As Integer = ScanChannel.GetLastColumnWithoutNaN

        ' calculate the coefficients y(x) = a + b * x for each line
        Dim a As Double ' Offset
        Dim b As Double ' Slope
        Dim n As Integer ' Value Counter
        Dim SSxy As Double ' empirical covariance
        Dim SSxx As Double ' empirical variance
        Dim XMean As Double = (XEnd - XStart) / 2 ' mean value of x (in general)
        Dim YMean As Double ' mean value of y per row

        For y As Integer = 0 To YCount - 1 Step 1
            a = 0
            b = 0
            n = 0
            SSxy = 0
            SSxx = 0
            YMean = 0

            ' calculate coefficient a and b -> see linear regression in wikipedia
            ' http://de.wikipedia.org/wiki/Lineare_Regression
            For x As Integer = 0 To XCount - 1 Step 1
                If Not Double.IsNaN(ScanChannel.ScanData(y, x)) Then
                    YMean += ScanChannel.ScanData(y, x)
                    n += 1
                End If
            Next

            If n <= 0 Then Continue For

            YMean /= n
            For x As Integer = 0 To XCount - 1 Step 1
                If Not Double.IsNaN(ScanChannel.ScanData(y, x)) Then
                    SSxy += (x - XMean) * (ScanChannel.ScanData(y, x) - YMean) / n
                    SSxx += (x - XMean) * (x - XMean) / n
                End If
            Next

            If SSxx = 0 Then Continue For

            b = SSxy / SSxx
            a = YMean - b * XMean

            ' Substract the line from each row.
            For x As Integer = 0 To XCount - 1 Step 1
                If Not Double.IsNaN(ScanChannel.ScanData(y, x)) Then
                    ScanChannel.ScanData(y, x) = ScanChannel.ScanData(y, x) - x * b - a
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
            Return My.Resources.rScanImageFilters.LinearLineSubstraction_Name
        End Get
    End Property

    ''' <summary>
    ''' Show this filter in the filter list.
    ''' </summary>
    Public ReadOnly Property ShowFilterInFilterMenu As Boolean Implements iScanImageFilter.ShowFilterInFilterMenu
        Get
            Return True
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
