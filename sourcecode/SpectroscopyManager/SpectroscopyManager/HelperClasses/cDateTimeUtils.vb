Public Module cDateTimeUtils

    ''' <summary>
    ''' <para>Truncates a DateTime to a specified resolution.</para>
    ''' <para>A convenient source for resolution is TimeSpan.TicksPerXXXX constants.</para>
    ''' </summary>
    ''' <param name="date">The DateTime object to truncate</param>
    ''' <param name="resolution">e.g. to round to nearest second, TimeSpan.TicksPerSecond</param>
    ''' <returns>Truncated DateTime</returns>
    <System.Runtime.CompilerServices.Extension>
    Public Function Truncate([date] As DateTime, resolution As Long) As DateTime
        Return New DateTime([date].Ticks - ([date].Ticks Mod resolution), [date].Kind)
    End Function

End Module