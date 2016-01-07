Public Class cUnits

    Public Enum UnitType
        Unknown
        Length
        Voltage
        Current
        Frequency
        Time
        Conductance
        ConductanceDeriv
        Unitary
    End Enum

    ''' <summary>
    ''' Gets the Prefix of the Unit as Key-String
    ''' and the recalculated Value as Value-Double.
    ''' </summary>
    ''' <param name="UseCompatibleOutputFormat">If true uses "u" instead of "µ" as prefix!</param>
    Public Shared Function GetPrefix(ByVal Value As Double,
                                     Optional ByVal UseCompatibleOutputFormat As Boolean = False) As KeyValuePair(Of String, Double)
        Dim sReturnString As String = ""
        Dim dReturnDouble As Double = 1
        Select Case Math.Abs(Value)
            Case 0
            Case Is < Math.Pow(10, -21)
                sReturnString = "y"
                dReturnDouble = Math.Pow(10, 24)
            Case Is < Math.Pow(10, -18)
                sReturnString = "z"
                dReturnDouble = Math.Pow(10, 21)
            Case Is < Math.Pow(10, -15)
                sReturnString = "a"
                dReturnDouble = Math.Pow(10, 18)
            Case Is < Math.Pow(10, -12)
                sReturnString = "f"
                dReturnDouble = Math.Pow(10, 15)
            Case Is < Math.Pow(10, -9)
                sReturnString = "p"
                dReturnDouble = Math.Pow(10, 12)
            Case Is < Math.Pow(10, -6)
                sReturnString = "n"
                dReturnDouble = Math.Pow(10, 9)
            Case Is < Math.Pow(10, -3)
                If UseCompatibleOutputFormat Then
                    sReturnString = "u"
                Else
                    sReturnString = "µ"
                End If
                dReturnDouble = Math.Pow(10, 6)
            Case Is < Math.Pow(10, 0)
                'Case Is < Math.Pow(10, -2)
                sReturnString = "m"
                dReturnDouble = Math.Pow(10, 3)
                'Case Is < Math.Pow(10, -1)
                '    sReturnString = "c"
                '    dReturnDouble = Math.Pow(10, 2)
                'Case Is < Math.Pow(10, 0)
                '    sReturnString = "d"
                '    dReturnDouble = Math.Pow(10, 1)
            Case Is < Math.Pow(10, 3)
                sReturnString = ""
                dReturnDouble = 1
            Case Is < Math.Pow(10, 6)
                sReturnString = "k"
                dReturnDouble = Math.Pow(10, -3)
            Case Is < Math.Pow(10, 9)
                sReturnString = "M"
                dReturnDouble = Math.Pow(10, -6)
            Case Is < Math.Pow(10, 12)
                sReturnString = "G"
                dReturnDouble = Math.Pow(10, -9)
            Case Is < Math.Pow(10, 15)
                sReturnString = "T"
                dReturnDouble = Math.Pow(10, -12)
            Case Is < Math.Pow(10, 18)
                sReturnString = "P"
                dReturnDouble = Math.Pow(10, -15)
            Case Is < Math.Pow(10, 21)
                sReturnString = "E"
                dReturnDouble = Math.Pow(10, -18)
            Case Is < Math.Pow(10, 24)
                sReturnString = "Z"
                dReturnDouble = Math.Pow(10, -21)
            Case Else
                sReturnString = "Y"
                dReturnDouble = Math.Pow(10, -24)
        End Select
        Return New KeyValuePair(Of String, Double)(sReturnString, dReturnDouble * Value)
    End Function

    ''' <summary>
    ''' Returns the Unit-Type by a given Unit-Type-String.
    ''' </summary>
    Public Shared Function GetUnitTypeFromTypeString(ByVal Symbol As String) As UnitType
        Select Case Symbol
            Case UnitType.Voltage.ToString
                Return UnitType.Voltage
            Case UnitType.Current.ToString
                Return UnitType.Current
            Case UnitType.Time.ToString
                Return UnitType.Time
            Case UnitType.Length.ToString
                Return UnitType.Length
            Case UnitType.Frequency.ToString
                Return UnitType.Frequency
            Case UnitType.Conductance.ToString
                Return UnitType.Conductance
            Case UnitType.Conductance.ToString
                Return UnitType.Conductance
            Case UnitType.ConductanceDeriv.ToString
                Return UnitType.ConductanceDeriv
            Case UnitType.ConductanceDeriv.ToString
                Return UnitType.ConductanceDeriv
            Case Else
                Return UnitType.Unknown
        End Select
    End Function

    ''' <summary>
    ''' Returns the Unit-Type by a given Unit-Symbol.
    ''' </summary>
    Public Shared Function GetUnitTypeFromSymbol(ByVal Symbol As String) As UnitType
        Select Case Symbol
            Case "V"
                Return UnitType.Voltage
            Case "A"
                Return UnitType.Current
            Case "s"
                Return UnitType.Time
            Case "m"
                Return UnitType.Length
            Case "Hz"
                Return UnitType.Frequency
            Case "A/V"
                Return UnitType.Conductance
            Case "S"
                Return UnitType.Conductance
            Case "S/V"
                Return UnitType.ConductanceDeriv
            Case "A/V^2"
                Return UnitType.ConductanceDeriv
            Case Else
                Return UnitType.Unknown
        End Select
    End Function

    ''' <summary>
    ''' Returns the Unit-Symbol by a given Unit-Type.
    ''' </summary>
    Public Shared Function GetUnitSymbolFromType(ByVal UnitType As UnitType) As String
        Select Case UnitType
            Case cUnits.UnitType.Voltage
                Return "V"
            Case cUnits.UnitType.Current
                Return "A"
            Case cUnits.UnitType.Time
                Return "s"
            Case cUnits.UnitType.Length
                Return "m"
            Case cUnits.UnitType.Frequency
                Return "Hz"
            Case cUnits.UnitType.Conductance
                Return "S"
            Case cUnits.UnitType.ConductanceDeriv
                Return "S/V"
            Case cUnits.UnitType.Unitary
                Return ""
            Case Else
                Return "-"
        End Select
    End Function

    ''' <summary>
    ''' Uses <code>GetPrefix</code> to return a formated string.
    ''' </summary>
    Public Shared Function GetFormatedValueString(ByVal Value As Double,
                                                  Optional ByVal Digits As Integer = 2) As String
        Dim VK As KeyValuePair(Of String, Double) = GetPrefix(Value)
        Return VK.Value.ToString("N" & Digits) & VK.Key
    End Function
End Class
