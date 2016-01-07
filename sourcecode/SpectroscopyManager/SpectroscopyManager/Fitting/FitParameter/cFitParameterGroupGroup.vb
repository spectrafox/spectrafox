Public Class cFitParameterGroupGroup
    Implements IEnumerable(Of cFitParameterGroup)

    ''' <summary>
    ''' Dictionary to store all the fit-parameter-groups
    ''' </summary>
    Private _FitParameterGroups As New Dictionary(Of Guid, cFitParameterGroup)

    ''' <summary>
    ''' Access Fit-Parameter-Groups
    ''' </summary>
    Public Function cFitParameterGroupGroup(ByVal Identifier As Guid) As cFitParameterGroup
        Return Me._FitParameterGroups(Identifier)
    End Function

    ''' <summary>
    ''' Returns the name of the group.
    ''' </summary>
    Public Function Group(ByVal Identifier As Guid) As cFitParameterGroup
        Return Me._FitParameterGroups(Identifier)
    End Function

    ''' <summary>
    ''' Returns the first (or only) existing group.
    ''' Nothing, if no group is stored.
    ''' </summary>
    Public Function FirstGroup() As cFitParameterGroup
        For Each G As cFitParameterGroup In Me._FitParameterGroups.Values
            Return G
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns if a Group exists
    ''' </summary>
    Public Function ContainsKey(ByVal Identifier As Guid) As Boolean
        Return Me._FitParameterGroups.ContainsKey(Identifier)
    End Function

    ''' <summary>
    ''' Returns if a Group exists
    ''' </summary>
    Public Function Exists(ByVal Identifier As Guid) As Boolean
        Return Me._FitParameterGroups.ContainsKey(Identifier)
    End Function

    ''' <summary>
    ''' The number of groups
    ''' </summary>
    Public Function Count() As Integer
        Return Me._FitParameterGroups.Count
    End Function

#Region "Adding and removing FitParameters"

    ''' <summary>
    ''' Wrapper for AddFitParameter.
    ''' </summary>
    Public Function Add(ByRef FitParameterGroup As cFitParameterGroup, ByVal GroupName As String) As Boolean
        Return Me.AddFitParameterGroup(FitParameterGroup, GroupName)
    End Function

    ''' <summary>
    ''' Adds a fit-parameter-group to the group.
    ''' True, if added!
    ''' </summary>
    Public Function AddFitParameterGroup(ByRef FitParameterGroup As cFitParameterGroup, ByVal GroupName As String) As Boolean
        If Me._FitParameterGroups.ContainsKey(FitParameterGroup.Identifier) Then
            Return False
        Else
            FitParameterGroup.GroupGroupName = GroupName
            Me._FitParameterGroups.Add(FitParameterGroup.Identifier, FitParameterGroup)
            Return True
        End If
    End Function

    ''' <summary>
    ''' Add Group of Fit-Parameters
    ''' </summary>
    Public Sub Add(ByRef FitParameterGroups As cFitParameterGroupGroup)
        Me.AddFitParameterGroupGroup(FitParameterGroups)
    End Sub

    ''' <summary>
    ''' Add Group of Fit-Parameters
    ''' </summary>
    Public Sub AddFitParameterGroupGroup(ByRef FitParameterGroups As cFitParameterGroupGroup)
        For Each FPG As cFitParameterGroup In FitParameterGroups
            Me.Add(FPG, FPG.GroupGroupName)
        Next
    End Sub

    ''' <summary>
    ''' Removes a fit-parameter from the group.
    ''' True, if removed!
    ''' </summary>
    Public Function RemoveFitParameter(ByVal Identifier As Guid) As Boolean
        If Me._FitParameterGroups.ContainsKey(Identifier) Then
            Return False
        Else
            Me._FitParameterGroups(Identifier).GroupGroupName = ""
            Me._FitParameterGroups.Remove(Identifier)
            Return True
        End If
    End Function

#End Region

#Region "Enumerator"

    ''' <summary>
    ''' Enumerator for ForEach loops
    ''' </summary>
    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New FitParameterGroupGroupEnum(Me._FitParameterGroups)
    End Function

    Public Function GetEnumerator1() As IEnumerator(Of cFitParameterGroup) Implements IEnumerable(Of cFitParameterGroup).GetEnumerator
        Return New FitParameterGroupGroupEnum(Me._FitParameterGroups)
    End Function

#End Region


End Class

''' <summary>
''' Enumerator Class to implement ForEach.
''' </summary>
Public Class FitParameterGroupGroupEnum
    Implements IEnumerator
    Implements IEnumerator(Of cFitParameterGroup)

    Private Position As Integer = -1
    Private CurrentGUID As Guid
    Private _FitParameterGroups As New Dictionary(Of Guid, cFitParameterGroup)

    Public Sub New(ByRef FitParameterGroupGroup As Dictionary(Of Guid, cFitParameterGroup))
        Me._FitParameterGroups = FitParameterGroupGroup
    End Sub

    Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        Position += 1
        If Position > Me._FitParameterGroups.Count - 1 Then Return False
        CurrentGUID = Me._FitParameterGroups.Keys.ElementAt(Position)

        Return (Position < Me._FitParameterGroups.Count)
    End Function

    Public Sub Reset() Implements IEnumerator.Reset
        Position = -1
    End Sub

    Public ReadOnly Property Current As Object Implements IEnumerator.Current
        Get
            Try
                Return Me._FitParameterGroups(CurrentGUID)
            Catch ex As Exception
                Throw New InvalidOperationException("FitParameterGroupGroup - Entry not found")
            End Try
        End Get
    End Property

    Public ReadOnly Property Current1 As cFitParameterGroup Implements IEnumerator(Of cFitParameterGroup).Current
        Get
            Try
                Return Me._FitParameterGroups(CurrentGUID)
            Catch ex As Exception
                Throw New InvalidOperationException("FitParameterGroupGroup - Entry not found")
            End Try
        End Get
    End Property

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class