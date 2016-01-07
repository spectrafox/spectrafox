Imports System.Text.RegularExpressions

Public Class NumericTextbox
    Inherits TextBox

#Region "Color Settings"
    Public Colors_EditForeground As Color = Color.Black
    Public Colors_EditBackground As Color = Color.White

    Public Colors_ValueOKForeground As Color = Color.White
    Public Colors_ValueOKBackground As Color = Color.Green

    Public Colors_ValueINVALIDForeground As Color = Color.White
    Public Colors_ValueINVALIDBackground As Color = Color.Red
#End Region

#Region "Tooltip"

    ''' <summary>
    ''' Tooltip to show error-informations.
    ''' </summary>
    Protected _ToolTip As New ToolTip

#End Region


#Region "Settings (Enums)"
    ''' <summary>
    ''' Types of Number-Values allowed.
    ''' </summary>
    Public Enum NumberRanges
        Positive
        Negative
        PositiveAndNegative
    End Enum

    ''' <summary>
    ''' Format of the values
    ''' </summary>
    Public Enum NumberFormatTypes
        DecimalUnits
        ScientificUnits
        PlainNumber
    End Enum

    ''' <summary>
    ''' Types of Data Values
    ''' </summary>
    Public Enum ValueTypes
        IntegerValue
        FloatingPointValue
    End Enum
#End Region

#Region "Properties"

    Private LastValue As Double = Double.NegativeInfinity
    Private CurrentValue As Double

    ''' <summary>
    ''' Contains the system settings for the number Formatting.
    ''' </summary>
    ''' <remarks></remarks>
    Protected NumberFormatInfo As Globalization.NumberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat

    ''' <summary>
    ''' Defines the way numbers are shown in the textbox.
    ''' </summary>
    Public Property NumberFormat As NumberFormatTypes = NumberFormatTypes.DecimalUnits

    ''' <summary>
    ''' Gives the range of Numbers allowed for the Numeric TextBox
    ''' </summary>
    Public Property NumberRange As NumberRanges = NumberRanges.PositiveAndNegative

    Public _ValueType As ValueTypes = ValueTypes.FloatingPointValue
    ''' <summary>
    ''' Type of numbers, that the Textbox should display.
    ''' </summary>
    Public Property ValueType As ValueTypes
        Get
            Return Me._ValueType
        End Get
        Set(value As ValueTypes)
            Me._ValueType = value
            If value = ValueTypes.IntegerValue Then
                Me.FormatDecimalPlaces = 0
            End If
        End Set
    End Property

    ''' <summary>
    ''' Format, that the textbox should use for displaying numbers.
    ''' </summary>
    Public Property FormatDecimalPlaces As Integer = 6

    ''' <summary>
    ''' If False, will not accept values of zero!
    ''' </summary>
    Public Property AllowZero As Boolean = True

#End Region

#Region "Events"
    ''' <summary>
    ''' Event fired, if a valid number has been entered, and the Textbox lost the focus.
    ''' </summary>
    Public Event ValidValueChanged(ByRef SourceTextBox As NumericTextbox)

    ''' <summary>
    ''' Verstecke das ursprüngliche TextChanged Event
    ''' </summary>
    Private Shadows Event TextChanged()
#End Region

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        ' Align the numbers right!
        Me.TextAlign = HorizontalAlignment.Right
        Me.Text = Decimal.Zero.ToString("F" & Me.FormatDecimalPlaces.ToString)

        ' Setup the tooltip style:
        With Me._ToolTip
            .ToolTipIcon = ToolTipIcon.Error
            .ToolTipTitle = My.Resources.NumericTextBox_ToolTip_ErrorTitle
        End With

    End Sub
#End Region

#Region "Check Input during typing"
    ''' <summary>
    ''' Restricts the entry of characters to digits (including hex),
    ''' the negative sign, the e decimal point, and editing keystrokes (backspace).
    ''' </summary>
    Protected Overrides Sub OnKeyPress(ByVal e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)

        Dim DecimalSeparator As String = NumberFormatInfo.NumberDecimalSeparator
        Dim GroupSeparator As String = NumberFormatInfo.NumberGroupSeparator
        Dim NegativeSign As String = NumberFormatInfo.NegativeSign

        Dim keyInput As String = e.KeyChar.ToString()

        If [Char].IsDigit(e.KeyChar) Then
            ' Digits are OK
        ElseIf keyInput.Equals(DecimalSeparator) And Me.ValueType = ValueTypes.FloatingPointValue Then
            ' If ValueType should be Decimal then DecimalSeparator is OK
        ElseIf keyInput.Equals(GroupSeparator) Then
            ' Group-separator
        ElseIf keyInput.Equals(NegativeSign) And Me.NumberRange <> NumberRanges.Positive Then
            ' Negative-Sign is OK
        ElseIf (keyInput.Equals("e") OrElse keyInput.Equals("E")) And Me.ValueType = ValueTypes.FloatingPointValue Then
            ' e,E is OK
        ElseIf (keyInput.Equals("m") OrElse
                keyInput.Equals("u") OrElse
                keyInput.Equals("n") OrElse
                keyInput.Equals("p") OrElse
                keyInput.Equals("f") OrElse
                keyInput.Equals("a") OrElse
                keyInput.Equals("z") OrElse
                keyInput.Equals("y") OrElse
                keyInput.Equals("d") OrElse
                keyInput.Equals("c")) And
               Me.ValueType = ValueTypes.FloatingPointValue Then
            ' SI Units
            ' m,u,n,p,f,a,z,y,d,c is OK
        ElseIf (keyInput.Equals("k") OrElse
                keyInput.Equals("M") OrElse
                keyInput.Equals("G") OrElse
                keyInput.Equals("T") OrElse
                keyInput.Equals("P") OrElse
                keyInput.Equals("E") OrElse
                keyInput.Equals("Z") OrElse
                keyInput.Equals("Y") OrElse
                keyInput.Equals("h") OrElse
                keyInput.Equals("d") OrElse
                keyInput.Equals("a")) And
               Me.ValueType = ValueTypes.FloatingPointValue Then
            ' SI Units
            ' k,M,G,T,P,E,Z,Y is OK
        ElseIf e.KeyChar = vbBack Then
            ' Backspace key is OK
        ElseIf Me.STRGPressed Then
            ' Allow Strg+C and Strg+V
        Else
            ' Swallow this invalid key and beep
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' Is the STRG-key currently pressed?
    ''' </summary>
    Private STRGPressed As Boolean = False

    ''' <summary>
    ''' Sets a flag, if STRG is pressed, to also allow copy paste.
    ''' </summary>
    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        Me.STRGPressed = e.Control
    End Sub

#End Region

#Region "Capture Key-Down event for color adaptions"
    ''' <summary>
    ''' 1) Sets the BackgroundColor of Textbox!
    ''' 2) If Key-Press is "enter", it finishes the editing!
    ''' </summary>
    Protected Sub KeyDownEvent(sender As System.Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Set color to white during TextChange
        Me.SetColor_NEUTRAL()

        ' Jump to next Control on "Enter"
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    ''' <summary>
    ''' 1) Sets the backgroundcolor of Textboxes, that are edited!
    ''' </summary>
    Protected Sub TextChange(sender As System.Object, e As EventArgs) Handles MyBase.TextChanged
        ' Set color to white during TextChange
        Me.SetColor_NEUTRAL()
    End Sub
#End Region

#Region "MAIN FUNCTION: Check the Input and Change the Value"
    ''' <summary>
    ''' If the Textbox looses its focus, the own ValueChanged-Event is fired,
    ''' if the entered value was numeric!
    ''' </summary>
    Protected Sub FocusLost(sender As System.Object, e As EventArgs) Handles Me.LostFocus

        ' Save the last value.
        Me.LastValue = Me.CurrentValue

        ' Temporary Value Storage
        Dim Value As Double
        Dim Handled As Boolean = False

        ' Evaluation Temporary Variables
        Dim SIExponent As Integer = 0
        Dim CurrentText As String = Me.Text

        ' Load some Settings
        Dim DecimalSeparator As String = NumberFormatInfo.NumberDecimalSeparator
        Dim GroupSeparator As String = NumberFormatInfo.NumberGroupSeparator
        Dim NegativeSign As String = NumberFormatInfo.NegativeSign
        Dim PositiveSign As String = NumberFormatInfo.PositiveSign

        ' Capture Exceptions!
        Try
            '###################
            ' Input evaluation

            ' 1) Check, if textbox is empty.
            If CurrentText.Length <= 0 Then
                Throw New Exception("Empty Input.")
            End If

            ' 2) Check, if the Input matches with the regular, or exponential input:
            '    e.g.: +12.42E-12 or just simple 12.42
            Dim RGX_ExponentInput As New Regex("^[" & Regex.Escape(NegativeSign) & Regex.Escape(PositiveSign) & "]?\d*?" & Regex.Escape(GroupSeparator) & "?\d*?" & Regex.Escape(DecimalSeparator) & "?\d*?[Ee]?[" & Regex.Escape(NegativeSign) & Regex.Escape(PositiveSign) & "]?\d+?$")
            If RGX_ExponentInput.IsMatch(CurrentText) Then
                ' Value entered in regular input style:
                '#######################################

                ' This can easily be converted by the Double.Parse structure
                Value = Double.Parse(CurrentText, NumberFormatInfo)
                If Me.ValueType <> ValueTypes.FloatingPointValue Then
                    Value = Convert.ToInt32(Value)
                End If

                Handled = True
            End If

            ' 3) Check, if the input is given in SI Units: (check last character)
            Dim RGX_SIUnitInput As New Regex("^[" & Regex.Escape(NegativeSign) & Regex.Escape(PositiveSign) & "]?\d*?" & Regex.Escape(GroupSeparator) & "?\d*?" & Regex.Escape(DecimalSeparator) & "?\d*?[YZEPTGMkhdcmunpfazy]?a?$")

            If Not Handled And RGX_SIUnitInput.IsMatch(CurrentText) Then
                ' Value entered in SI-Units input style:
                '########################################

                Dim LastCharacter As String = CurrentText.Substring(CurrentText.Length - 1, 1)
                If Not IsNumeric(LastCharacter) Then
                    ' SI Units
                    Select Case LastCharacter
                        Case "Y"
                            SIExponent = 24
                        Case "Z"
                            SIExponent = 21
                        Case "E"
                            SIExponent = 18
                        Case "P"
                            SIExponent = 15
                        Case "T"
                            SIExponent = 12
                        Case "G"
                            SIExponent = 9
                        Case "M"
                            SIExponent = 6
                        Case "k"
                            SIExponent = 3
                        Case "h"
                            SIExponent = 2
                        Case "d"
                            SIExponent = -1
                        Case "c"
                            SIExponent = -2
                        Case "m"
                            SIExponent = -3
                        Case "u"
                            SIExponent = -6
                        Case "n"
                            SIExponent = -9
                        Case "p"
                            SIExponent = -12
                        Case "f"
                            SIExponent = -15
                        Case "a"
                            ' Check for "Deka" -> "da"
                            If CurrentText.Substring(CurrentText.Length - 2, 1) = "d" Then
                                ' Deka
                                SIExponent = 1
                                ' Delete also the d
                                CurrentText = CurrentText.Remove(CurrentText.Length - 1)
                            Else
                                ' Atto
                                SIExponent = -18
                            End If
                        Case "z"
                            SIExponent = -21
                        Case "y"
                            SIExponent = -24
                    End Select

                    CurrentText = CurrentText.Remove(CurrentText.Length - 1)
                End If

                Value = Double.Parse(CurrentText, NumberFormatInfo)
                If Me.ValueType <> ValueTypes.FloatingPointValue Then
                    Value = Convert.ToInt32(Value)
                End If

                ' Apply SI-Units:
                If SIExponent <> 0 Then Value *= Math.Pow(10, SIExponent)

                Handled = True
            End If

            ' End input evaluation
            '######################

            ' If the input has not been handled so far, throw exception
            If Not Handled Then
                Throw New Exception("Input could not be recognized as valid number!")
            End If

            ' Check for correct NumberRange
            If NumberRange = NumberRanges.Negative And Value > 0 Then
                Throw New Exception("No positive values allowed.")
            ElseIf NumberRange = NumberRanges.Positive And Value < 0 Then
                Throw New Exception("No negative values allowed.")
            End If

            ' Check for allowed zero values:
            If Not AllowZero And Value = 0 Then
                Throw New Exception("No zero value allowed.")
            End If

            ' Format the value in a proper way and write it to the textbox.
            Me.SetValue(Value)

            ' Set the color of the textbox to the "valid input"-style
            Me.SetColor_ValueOK()

            ' Save the value to the internal storage
            Me.CurrentValue = Value

            ' Fire the "value-changed"-event, if the value really changed.
            If Me.CurrentValue <> Me.LastValue Then
                RaiseEvent ValidValueChanged(Me)
            End If
        Catch ex As Exception

            ' Show the invalid-input colors!
            Me.SetColor_ValueINVALID()

            ' Show the tooltip and the error message.
            Me._ToolTip.Show(ex.Message, Me, Me.Width, Me.Height)

        End Try
    End Sub
#End Region

#Region "Get Value-Functions"
    ''' <summary>
    ''' Returns the current value as Integer.
    ''' </summary>
    Public ReadOnly Property IntValue() As Integer
        Get
            Return CInt(Me.CurrentValue)
        End Get
    End Property

    ''' <summary>
    ''' Returns the current value as Decimal
    ''' </summary>
    Public ReadOnly Property DecimalValue() As Double
        Get
            Return Me.CurrentValue
        End Get
    End Property
#End Region

#Region "Set Value-Functions"
    ''' <summary>
    ''' Writes a Double-Value to the Textbox,
    ''' to have a unified format in all Textboxes.
    ''' </summary>
    Public Sub SetValue(ByVal Value As Double,
                        Optional ByVal DecimalPlaces As Integer = -1,
                        Optional ByVal RaiseValueChangedEvent As Boolean = False)
        If DecimalPlaces < 0 Then DecimalPlaces = Me.FormatDecimalPlaces

        ' Override CustomFormat, if the box should display integers.
        If Me.ValueType = ValueTypes.IntegerValue Then
            DecimalPlaces = 0
        End If

        ' Format the value according to the preferred way of displaying.

        Select Case Me.NumberFormat
            Case NumberFormatTypes.DecimalUnits
                ' 1) Exponential display:
                ' Check, if we need to display the value in exponential way
                If (Value * Math.Pow(10, DecimalPlaces)) < 1 Or (Value / Math.Pow(10, DecimalPlaces)) > 1000 Then
                    ' Use exponential, if the number of decimal places is not enough
                    ' to display at least two valid digets
                    Me.SetTextThreadSafe(Value.ToString("E" & DecimalPlaces.ToString))
                Else
                    ' Display as simple comma value
                    Me.SetTextThreadSafe(Value.ToString("F" & DecimalPlaces.ToString))
                End If

            Case NumberFormatTypes.ScientificUnits
                ' 2) SI-Units:
                Dim FormatedValue As KeyValuePair(Of String, Double) = cUnits.GetPrefix(Value, True)
                Me.SetTextThreadSafe(FormatedValue.Value.ToString("F" & DecimalPlaces.ToString) & FormatedValue.Key)

            Case NumberFormatTypes.PlainNumber
                ' 3) Plain number without exponent:
                Me.SetTextThreadSafe(Value.ToString("N" & DecimalPlaces.ToString))

        End Select

        Me.SetColor_ValueOK()

        ' Set the value
        Me.CurrentValue = Value

        ' Raise the ValidValueChangedEvent, if it is requested optionally
        If RaiseValueChangedEvent Then
            RaiseEvent ValidValueChanged(Me)
        End If
    End Sub

    ''' <summary>
    ''' Writes a Decimal-Value to the Textbox,
    ''' to have a unified format in all Textboxes.
    ''' </summary>
    Public Sub SetValue(ByVal Value As Decimal,
                        Optional ByVal DecimalPlaces As Integer = -1)
        Me.SetValue(Convert.ToDouble(Value), DecimalPlaces)
    End Sub

    ''' <summary>
    ''' Writes an Integer-Value to the Textbox,
    ''' to have a unified format in all Textboxes.
    ''' </summary>
    Public Sub SetValue(ByVal Value As Integer,
                        Optional ByVal DecimalPlaces As Integer = -1)
        Me.SetValue(Convert.ToDouble(Value), DecimalPlaces)
    End Sub
#End Region

#Region "Set Color-Functions"



    ''' <summary>
    ''' Color of the Textbox for "input is valid"
    ''' </summary>
    Protected Sub SetColor_ValueOK()
        Me.SetBackColorThreadSafe(Me.Colors_ValueOKBackground)
        Me.SetForeColorThreadSafe(Me.Colors_ValueOKForeground)
    End Sub

    ''' <summary>
    ''' Color of the Textbox for "input is INvalid"
    ''' </summary>
    Protected Sub SetColor_ValueINVALID()
        Me.SetBackColorThreadSafe(Me.Colors_ValueINVALIDBackground)
        Me.SetForeColorThreadSafe(Me.Colors_ValueINVALIDForeground)
    End Sub

    ''' <summary>
    ''' Color of the Textbox for "NEUTRAL"
    ''' </summary>
    Protected Sub SetColor_NEUTRAL()
        If Me.BackColor <> Me.Colors_EditBackground Then Me.SetBackColorThreadSafe(Me.Colors_EditBackground)
        If Me.ForeColor <> Me.Colors_EditForeground Then Me.SetForeColorThreadSafe(Me.Colors_EditForeground)
    End Sub

#End Region

#Region "Thread-safe property access"

    ''' <summary>
    ''' Delegate to set the text of the box thread-safe.
    ''' </summary>
    Public Delegate Sub _SetText(ByVal Text As String)

    ''' <summary>
    ''' Sets the .Text property in a thread safe way.
    ''' </summary>
    Public Sub SetTextThreadSafe(ByVal Text As String)
        If Me.InvokeRequired Then
            Dim d As _SetText = AddressOf Me.SetTextThreadSafe
            Me.Invoke(d, Text)
        Else
            Me.Text = Text
        End If
    End Sub

    ''' <summary>
    ''' Delegate to set the color of the box thread-safe.
    ''' </summary>
    Public Delegate Sub _SetColor(ByVal c As Color)

    ''' <summary>
    ''' Sets the .BackColor property in a thread safe way.
    ''' </summary>
    Public Sub SetBackColorThreadSafe(ByVal c As Color)
        If Me.InvokeRequired Then
            Dim d As _SetColor = AddressOf Me.SetBackColorThreadSafe
            Me.Invoke(d, c)
        Else
            Me.BackColor = c
        End If
    End Sub

    ''' <summary>
    ''' Sets the .ForeColor property in a thread safe way.
    ''' </summary>
    Public Sub SetForeColorThreadSafe(ByVal c As Color)
        If Me.InvokeRequired Then
            Dim d As _SetColor = AddressOf Me.SetForeColorThreadSafe
            Me.Invoke(d, c)
        Else
            Me.ForeColor = c
        End If
    End Sub

#End Region

End Class
