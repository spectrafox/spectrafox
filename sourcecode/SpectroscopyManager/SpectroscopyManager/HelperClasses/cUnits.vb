Public Class cUnits

    ''' <summary>
    ''' Possible units.
    ''' </summary>
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
        Angle
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
    ''' Gets from the prefix the double factor representing this prefix.
    ''' E.g. µ -> 1E-6
    ''' Input can be any unit string.
    ''' Output returns the unit symbol without the prefix, and the factor.
    ''' </summary>
    Public Shared Function GetFactorFromPrefix(ByVal UnitString As String) As KeyValuePair(Of String, Double)
        ' Check for a valid value range.
        If UnitString.Length < 1 Then Return New KeyValuePair(Of String, Double)(UnitString, 1)

        ' Get a default value.
        Dim PrefixFactor As Double = 1

        ' Extract the first letter of the unit.
        Dim FirstLetterOfUnit As String = UnitString.Substring(0, 1)
        Dim NoValidPrefixFound As Boolean = False

        Select Case FirstLetterOfUnit
            Case "y"
                PrefixFactor = Math.Pow(10, -24)
            Case "z"
                PrefixFactor = Math.Pow(10, -21)
            Case "a"
                PrefixFactor = Math.Pow(10, -18)
            Case "f"
                PrefixFactor = Math.Pow(10, -15)
            Case "p"
                PrefixFactor = Math.Pow(10, -12)
            Case "n"
                PrefixFactor = Math.Pow(10, -9)
            Case "u", "µ"
                PrefixFactor = Math.Pow(10, -6)
            Case "m"
                PrefixFactor = Math.Pow(10, -3)
            Case "c"
                PrefixFactor = Math.Pow(10, -2)
            Case "d"
                PrefixFactor = Math.Pow(10, -1)
            Case "k"
                PrefixFactor = Math.Pow(10, 3)
            Case "M"
                PrefixFactor = Math.Pow(10, 6)
            Case "G"
                PrefixFactor = Math.Pow(10, 9)
            Case "T"
                PrefixFactor = Math.Pow(10, 12)
            Case "P"
                PrefixFactor = Math.Pow(10, 15)
            Case "E"
                PrefixFactor = Math.Pow(10, 18)
            Case "Z"
                PrefixFactor = Math.Pow(10, 21)
            Case Else
                PrefixFactor = 1
                NoValidPrefixFound = True
        End Select

        ' Stripe the unit prefix.
        If Not NoValidPrefixFound Then
            UnitString = UnitString.Substring(1, UnitString.Length - 1)
        End If

        Return New KeyValuePair(Of String, Double)(UnitString, PrefixFactor)
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
            Case UnitType.Angle.ToString
                Return UnitType.Angle
            Case Else
                Return UnitType.Unknown
        End Select
    End Function

    ''' <summary>
    ''' Returns the Unit-Type by a given Unit-Symbol.
    ''' </summary>
    Public Shared Function GetUnitTypeFromSymbol(ByVal Symbol As String) As UnitType
        Select Case Symbol
            Case "V", "Volt"
                Return UnitType.Voltage
            Case "A", "Ampere"
                Return UnitType.Current
            Case "s", "Second"
                Return UnitType.Time
            Case "m", "Meter", "Z", "z", "X", "x", "Y", "y"
                Return UnitType.Length
            Case "Hz", "Hertz"
                Return UnitType.Frequency
            Case "A/V"
                Return UnitType.Conductance
            Case "S", "Siemens"
                Return UnitType.Conductance
            Case "S/V"
                Return UnitType.ConductanceDeriv
            Case "A/V^2"
                Return UnitType.ConductanceDeriv
            Case "Count"
                Return UnitType.Unitary
            Case "Degree", "Deg", "°"
                Return UnitType.Angle
            Case Else
                Return UnitType.Unknown
        End Select
    End Function

    ''' <summary>
    ''' Returns the Unit-Symbol by a given Unit-Type.
    ''' </summary>
    Public Shared Function GetUnitSymbolFromType(ByVal UnitType As UnitType) As String
        Select Case UnitType
            Case UnitType.Voltage
                Return "V"
            Case UnitType.Current
                Return "A"
            Case UnitType.Time
                Return "s"
            Case UnitType.Length
                Return "m"
            Case UnitType.Frequency
                Return "Hz"
            Case UnitType.Conductance
                Return "S"
            Case UnitType.ConductanceDeriv
                Return "S/V"
            Case UnitType.Unitary
                Return "1"
            Case UnitType.Angle
                Return "°"
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

    ''' <summary>
    ''' Gets from a string "µm/V" a tuple of the two units!
    ''' </summary>
    Public Shared Function GetUnitVsUnitFromString(ByVal Value As String) As Tuple(Of String, String)
        Dim Split As String() = Value.Split(CChar("/"))
        If Split.Length = 2 Then
            Return New Tuple(Of String, String)(Split(0), Split(1))
        Else
            Return New Tuple(Of String, String)(String.Empty, String.Empty)
        End If
    End Function

End Class
