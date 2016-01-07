''' <summary>
''' Structure representing a Fit-Parameter
''' </summary>
<DebuggerDisplay("{Name} = {Value}, fixed = {IsFixed}")>
Public Class cFitParameter

#Region "Properties of this structure"

    ''' <summary>
    ''' Wrapper for the unique name
    ''' </summary>
    Public ReadOnly Property Identifier As String
        Get
            Return Me._Name
        End Get
    End Property

    ''' <summary>
    ''' Changes the identifier
    ''' </summary>
    Public Sub ChangeIdentifier(Identifier As String)
        Me.Name = Identifier
    End Sub

    ''' <summary>
    ''' Name given to the Fit-Parameter
    ''' </summary>
    Public Property Name As String

    ''' <summary>
    ''' Raised, if the value changes.
    ''' </summary>
    Public Event ValueChanged(NewValue As Double)
    Private _Value As Double
    ''' <summary>
    ''' Current value of the fit-parameter.
    ''' </summary>
    Public ReadOnly Property Value As Double
        Get
            Return Me._Value
        End Get
    End Property

    ''' <summary>
    ''' Change the value of the fit-parameter
    ''' </summary>
    Public Sub ChangeValue(Value As Double, Optional ByVal FireValueChangedEvent As Boolean = True)
        Me._Value = Value
        If FireValueChangedEvent Then RaiseEvent ValueChanged(Value)
    End Sub

    ''' <summary>
    ''' Current error of the value of the fit-parameter.
    ''' </summary>
    Public Property StandardDeviation As Double

    ''' <summary>
    ''' Should the parameter be modified in the fit?
    ''' </summary>
    Public Property IsFixed As Boolean

    ''' <summary>
    ''' Short description of the parameter to be shown in the GUI.
    ''' </summary>
    Public Property Description As String

    ''' <summary>
    ''' Identifier of the parameter to which this parameter is locked!
    ''' Only active, if this parameter is fixed, and the other is non-fixed.
    ''' -1 is non-active.
    ''' </summary>
    Public Property LockedToParameterIdentifier As String

    ''' <summary>
    ''' Factor used to lock to another parameter. Usually 1.
    ''' </summary>
    Public Property LockedWithFactor As Double

    ''' <summary>
    ''' Upper Boundary of the Parameter (incl)
    ''' </summary>
    Public Property UpperBoundary As Double

    ''' <summary>
    ''' Upper Boundary of the Parameter (incl)
    ''' </summary>
    Public Property LowerBoundary As Double

    ''' <summary>
    ''' Contructor for a new FitParameter
    ''' </summary>
    ''' <param name="ParameterName">Unique Parameter Name among all other fit parameters</param>
    Public Sub New(ByVal ParameterName As String,
                   ByVal InitialValue As Double,
                   ByVal IsValueFixed As Boolean,
                   Optional ByVal ParameterDescription As String = "",
                   Optional ByVal LockedToIdentifier As String = "",
                   Optional ByVal UpperValueBoundaryIncl As Double = Double.MaxValue,
                   Optional ByVal LowerValueBoundaryIncl As Double = Double.MinValue,
                   Optional ByVal StandardDeviationOfValue As Double = 0,
                   Optional ByVal _LockedWithFactor As Double = 1)
        Me.Name = ParameterName
        Me._Value = InitialValue
        Me.IsFixed = IsValueFixed
        Me.Description = ParameterDescription
        Me.LockedToParameterIdentifier = LockedToIdentifier
        Me.UpperBoundary = UpperValueBoundaryIncl
        Me.LowerBoundary = LowerValueBoundaryIncl
        Me.StandardDeviation = StandardDeviationOfValue
        Me.LockedWithFactor = _LockedWithFactor
    End Sub
#End Region

#Region "Function to get an array of fittable Parameter values"

    ''' <summary>
    ''' Numeric-Format of all Parameter-Output of the functions in this class.
    ''' </summary>
    Public Shared NumericFormat As String = "E6"

    ''' <summary>
    ''' Returns a full, detailed string of all current parameters in the list.
    ''' </summary>
    Public Shared Function GetFullParameterEcho(ByRef FitParameters As cFitParameterGroupGroup) As String
        Dim SB As New System.Text.StringBuilder()
        Dim MaxLengthName As Integer = 0

        ' For Formatting, extract the longest name of the fit-parameters
        For Each FPG As cFitParameterGroup In FitParameters
            For Each P As cFitParameter In FPG
                If P.Description.Length > MaxLengthName Then MaxLengthName = P.Description.Length
            Next
        Next

        ' Add one additional Space
        MaxLengthName += 1

        ' Go through all Parameters and echo the current settings
        Dim i As Integer = 0
        Dim j As Integer = 0
        For Each FPG As cFitParameterGroup In FitParameters
            i = 0
            SB.AppendLine(FPG.GroupGroupName)
            For Each Parameter As cFitParameter In FPG
                If Parameter.IsFixed Then
                    SB.Append("(fixed)  ")
                Else
                    SB.Append("(fitted) ")
                End If
                SB.Append(Parameter.Description)
                SB.Append(New String(Chr(32), (MaxLengthName - Parameter.Description.Length)))
                SB.Append("= ")
                SB.Append(Parameter.Value.ToString(NumericFormat))
                If Not Parameter.IsFixed Then
                    SB.Append(" +- ")
                    SB.Append(Parameter.StandardDeviation.ToString(NumericFormat))
                End If
                i += 1
                If i < FPG.Count Then
                    SB.Append(vbNewLine)
                End If
            Next
            j += 1
            If j < FitParameters.Count Then
                SB.Append(vbNewLine)
            End If
        Next

        Return SB.ToString
    End Function

    ''' <summary>
    ''' Returns an excel parameter echo, detailed string of all current parameters in the list.
    ''' Parameters separated by TABs.
    ''' </summary>
    Public Shared Function GetExcelCompatibleParameterEcho(ByRef FitParameters As cFitParameterGroup) As String
        Dim SBHeader As New System.Text.StringBuilder()
        Dim SBParameter As New System.Text.StringBuilder()

        ' Go through all Parameters and echo the current settings
        Dim i As Integer = 0
        For Each Parameter As cFitParameter In FitParameters
            SBHeader.Append(Parameter.Description)
            SBParameter.Append(Parameter.Value.ToString(NumericFormat))
            i += 1
            If i < FitParameters.Count Then
                SBHeader.Append(vbTab)
                SBParameter.Append(vbTab)
            End If
        Next

        ' Merge both strings
        SBHeader.Append(vbNewLine)
        SBHeader.Append(SBParameter)
        Return SBHeader.ToString
    End Function

    ''' <summary>
    ''' Returns an excel parameter echo, detailed string of all current parameters in the list.
    ''' Parameters separated by TABs.
    ''' </summary>
    Public Shared Function GetExcelCompatibleParameterEcho(ByRef FitParameterGroups As cFitParameterGroupGroup) As String
        Dim SB As New System.Text.StringBuilder()
        Dim i As Integer = 0
        For Each FPG As cFitParameterGroup In FitParameterGroups
            SB.AppendLine(FPG.GroupGroupName)
            SB.Append(GetExcelCompatibleParameterEcho(FPG))
            i += 1
            If i < FitParameterGroups.Count Then
                SB.Append(vbNewLine)
            End If
        Next
        Return SB.ToString
    End Function

    ''' <summary>
    ''' Returns a short Two-line-string of all parameters.
    ''' </summary>
    Public Shared Function GetShortParameterEcho(ByRef FitParameters As cFitParameterGroupGroup) As String
        Dim SB As New System.Text.StringBuilder()
        Dim MaxLength As Integer = 0

        ' For Formatting, extract the longest printout
        For Each FPG As cFitParameterGroup In FitParameters
            For Each Parameter As cFitParameter In FPG
                If Parameter.Description.Length > MaxLength Then MaxLength = Parameter.Description.Length
                If Parameter.Value.ToString(NumericFormat).Length > MaxLength Then MaxLength = Parameter.Value.ToString(NumericFormat).Length
            Next
        Next

        ' Go through all Parameters and echo the current settings
        For Each FPG As cFitParameterGroup In FitParameters
            SB.AppendLine(FPG.GroupGroupName)
            SB.Append("||")
            For Each Parameter As cFitParameter In FPG
                SB.Append(" ")
                SB.Append(Parameter.Description)
                SB.Append(New String(Chr(32), (MaxLength - Parameter.Description.Length)))
                SB.Append(" |")
            Next
            SB.Append("|")
            SB.Append(vbCrLf)
            SB.Append("||")
            Dim ParameterValue As String
            For Each Parameter As cFitParameter In FPG
                SB.Append(" ")
                ParameterValue = Parameter.Value.ToString(NumericFormat)
                SB.Append(ParameterValue)
                SB.Append(New String(Chr(32), (MaxLength - ParameterValue.Length)))
                SB.Append(" |")
            Next
            SB.Append("|")
            SB.Append(vbCrLf)
        Next

        Return SB.ToString
    End Function
#End Region

#Region "Get/Extract Group-Identifier"

    ''' <summary>
    ''' Returns the Identifier in the current Fitparameter for the given GroupID
    ''' </summary>
    Public Function GetIdentifierForGroupID(ByVal GroupID As Guid) As String
        Return GetIdentifierForGroupID(GroupID, Me.Name)
    End Function

    ''' <summary>
    ''' Returns the Identifier in the current Fitparameter for the given GroupID
    ''' </summary>
    Public Shared Function GetIdentifierForGroupID(ByVal GroupID As Guid, ByVal ParameterName As String) As String
        Return GroupID.ToString("N") & "#" & ParameterName
    End Function

    ''' <summary>
    ''' Returns the Group-Identifier in the current Fitparameter for the given Identifier
    ''' Returns KeyValuePair(GUID, FitParameter-Identifier)
    ''' </summary>
    Public Shared Function GetGroupIDFromIdentifier(ByVal Identifier As String) As KeyValuePair(Of Guid, String)
        Dim ParameterIdentifier() As String = Identifier.Split(CChar("#"))
        If ParameterIdentifier.Length <> 2 Then
            Return New KeyValuePair(Of Guid, String)(Nothing, "")
        End If

        Return New KeyValuePair(Of Guid, String)(New Guid(ParameterIdentifier(0)), ParameterIdentifier(1))
    End Function

#End Region

End Class
