Imports SpectroscopyManager

Public Class cNumericSmoothing_AdjacentAverageSmooth
    Implements iNumericSmoothingFunction

    ''' <summary>
    ''' Description of the smoothing function.
    ''' </summary>
    Public ReadOnly Property Description As String Implements iNumericSmoothingFunction.Description
        Get
            Return My.Resources.rSmoothing.Description_AdjacentAverage
        End Get
    End Property

    ''' <summary>
    ''' Public name of the smoothing function.
    ''' </summary>
    Public ReadOnly Property Name As String Implements iNumericSmoothingFunction.Name
        Get
            Return My.Resources.rSmoothing.Name_AdjacentAverage
        End Get
    End Property

    ''' <summary>
    ''' Sets the smoothing window of this filter.
    ''' </summary>
    Public Property NearestNeighborWindow As Integer
        Get
            Return CInt(Me.SmoothingControl.nudNeighbors.Value)
        End Get
        Set(value As Integer)
            Me.SmoothingControl.nudNeighbors.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a string with the current settings of the routine
    ''' encoded in the string to be storable in the program settings.
    ''' </summary>
    Public Property CurrentSmoothingSettings As String Implements iNumericSmoothingFunction.CurrentSmoothingSettings
        Get
            Dim SB As New Text.StringBuilder
            SB.Append("NearestNeighborWindow=")
            SB.Append(Me.NearestNeighborWindow.ToString)
            Return SB.ToString
        End Get
        Set(value As String)
            Dim SettingsLines As String() = value.Split(CChar("#"))
            For Each SettingsLine As String In SettingsLines
                Dim Settings As String() = SettingsLine.Split(CChar("="))
                If Settings.Length = 2 Then
                    If Settings(0) = "NearestNeighborWindow" Then
                        Me.NearestNeighborWindow = Convert.ToInt32(Settings(1))
                    End If
                End If
            Next
        End Set
    End Property

    ''' <summary>
    ''' Returns the smoothed data.
    ''' </summary>
    Public Function Smooth(ByRef InColumn As ICollection(Of Double)) As List(Of Double) Implements iNumericSmoothingFunction.Smooth

        Dim OutY As New List(Of Double)

        Dim SumY As Double = 0D
        Dim iii As Integer = 0
        Dim iif As Integer = 0
        Dim ni As Integer = 0
        Dim nf As Integer = InColumn.Count - 1

        For n As Integer = ni To nf Step 1
            SumY = 0D

            If NearestNeighborWindow < n - ni Then
                iii = n - ni - NearestNeighborWindow
            Else
                iii = 0
            End If

            If NearestNeighborWindow > nf - n Then
                iif = nf - ni
            Else
                iif = n - ni + NearestNeighborWindow
            End If

            For i As Integer = iii To iif Step 1
                SumY += InColumn(i)
            Next
            OutY.Add(SumY / (iif - iii + 1))
        Next
        Return OutY

    End Function

    ''' <summary>
    ''' Settings-Control.
    ''' </summary>
    Private SmoothingControl As New cNumericSmoothingPanel_AdjacentAverageSmooth

    ''' <summary>
    ''' Returns the settings control for this smoothing method.
    ''' </summary>
    Public Function SmoothingOptions() As UserControl Implements iNumericSmoothingFunction.SmoothingOptions
        Return Me.SmoothingControl
    End Function

End Class
