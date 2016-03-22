Public Class cNumericSmoothing_SavitzkyGolay
    Implements iNumericSmoothingFunction

    ''' <summary>
    ''' Description of the smoothing function.
    ''' </summary>
    Public ReadOnly Property Description As String Implements iNumericSmoothingFunction.Description
        Get
            Return My.Resources.rSmoothing.Description_SavitzkyGolay
        End Get
    End Property

    ''' <summary>
    ''' Public name of the smoothing function.
    ''' </summary>
    Public ReadOnly Property Name As String Implements iNumericSmoothingFunction.Name
        Get
            Return My.Resources.rSmoothing.Name_SavitzkyGolay
        End Get
    End Property

    ''' <summary>
    ''' Sets the smoothing window of this filter.
    ''' </summary>
    Public Property NearestNeighborWindow() As Integer
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
    ''' Smoothes the data with the Savitzky Golay method.
    ''' </summary>
    Public Function Smooth(ByRef InColumnY As ICollection(Of Double)) As List(Of Double) Implements iNumericSmoothingFunction.Smooth
        ' Create the matrix of the Savitzky-Golay Coefficients
        Dim SGCoef As MathNet.Numerics.LinearAlgebra.Single.DenseMatrix = MathNet.Numerics.LinearAlgebra.Single.DenseMatrix.Create(13, 14, Function(x As Integer, y As Integer) As Single
                                                                                                                                               Return 0
                                                                                                                                           End Function)
        ' Set the smoothing Coefficients for Savitzky-Golay
        ' The zeroth value is the normalization factor
        ' SGCoef(Neighbors)
        SGCoef(2, 1) = 17
        SGCoef(2, 2) = 12
        SGCoef(2, 3) = -3
        SGCoef(2, 0) = 35

        SGCoef(3, 1) = 7
        SGCoef(3, 2) = 6
        SGCoef(3, 3) = 3
        SGCoef(3, 4) = -2
        SGCoef(3, 0) = 21

        SGCoef(4, 1) = 59
        SGCoef(4, 2) = 54
        SGCoef(4, 3) = 39
        SGCoef(4, 4) = 14
        SGCoef(4, 5) = -21
        SGCoef(4, 0) = 231

        SGCoef(5, 1) = 89
        SGCoef(5, 2) = 84
        SGCoef(5, 3) = 69
        SGCoef(5, 4) = 44
        SGCoef(5, 5) = 9
        SGCoef(5, 6) = -36
        SGCoef(5, 0) = 429

        SGCoef(6, 1) = 25
        SGCoef(6, 2) = 24
        SGCoef(6, 3) = 21
        SGCoef(6, 4) = 16
        SGCoef(6, 5) = 9
        SGCoef(6, 6) = 0
        SGCoef(6, 7) = -11
        SGCoef(6, 0) = 143

        SGCoef(7, 1) = 167
        SGCoef(7, 2) = 162
        SGCoef(7, 3) = 147
        SGCoef(7, 4) = 122
        SGCoef(7, 5) = 87
        SGCoef(7, 6) = 42
        SGCoef(7, 7) = -13
        SGCoef(7, 8) = -78
        SGCoef(7, 0) = 1105

        SGCoef(8, 1) = 43
        SGCoef(8, 2) = 42
        SGCoef(8, 3) = 39
        SGCoef(8, 4) = 34
        SGCoef(8, 5) = 27
        SGCoef(8, 6) = 18
        SGCoef(8, 7) = 7
        SGCoef(8, 8) = -6
        SGCoef(8, 9) = -21
        SGCoef(8, 0) = 323

        SGCoef(9, 1) = 269
        SGCoef(9, 2) = 264
        SGCoef(9, 3) = 249
        SGCoef(9, 4) = 224
        SGCoef(9, 5) = 189
        SGCoef(9, 6) = 144
        SGCoef(9, 7) = 89
        SGCoef(9, 8) = 24
        SGCoef(9, 9) = -51
        SGCoef(9, 10) = -136
        SGCoef(9, 0) = 2261

        SGCoef(10, 1) = 329
        SGCoef(10, 2) = 324
        SGCoef(10, 3) = 309
        SGCoef(10, 4) = 284
        SGCoef(10, 5) = 249
        SGCoef(10, 6) = 204
        SGCoef(10, 7) = 149
        SGCoef(10, 8) = 84
        SGCoef(10, 9) = 9
        SGCoef(10, 10) = -76
        SGCoef(10, 11) = -171
        SGCoef(10, 0) = 3059

        SGCoef(11, 1) = 79
        SGCoef(11, 2) = 78
        SGCoef(11, 3) = 75
        SGCoef(11, 4) = 70
        SGCoef(11, 5) = 63
        SGCoef(11, 6) = 54
        SGCoef(11, 7) = 43
        SGCoef(11, 8) = 30
        SGCoef(11, 9) = 15
        SGCoef(11, 10) = -2
        SGCoef(11, 11) = -21
        SGCoef(11, 12) = -42
        SGCoef(11, 0) = 806

        SGCoef(12, 1) = 467
        SGCoef(12, 2) = 462
        SGCoef(12, 3) = 447
        SGCoef(12, 4) = 422
        SGCoef(12, 5) = 387
        SGCoef(12, 6) = 322
        SGCoef(12, 7) = 287
        SGCoef(12, 8) = 222
        SGCoef(12, 9) = 147
        SGCoef(12, 10) = 62
        SGCoef(12, 11) = -33
        SGCoef(12, 12) = -138
        SGCoef(12, 13) = -253
        SGCoef(12, 0) = 5175

        ' This method of the Savitzky-Golay smoothing algorithm fits the data window
        ' given by a certain number of neighbors to a second order polynomial.
        ' This method assumes a fixed spacing of the data points.
        Dim NumberOfPoints As Integer = InColumnY.Count

        Dim SmoothedY As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(NumberOfPoints)

        Dim TempSum As Double = 0
        Dim LargerVal As Double
        Dim SmallerVal As Double
        Dim iMinusj As Integer
        Dim iPlusj As Integer

        For i As Integer = 0 To NumberOfPoints - 1 Step 1
            TempSum = InColumnY(i) * SGCoef(Me.NearestNeighborWindow, 1)
            For j As Integer = 1 To Me.NearestNeighborWindow Step 1
                iPlusj = (i + j)
                iMinusj = (i - j)

                If iPlusj < 0 Then
                    LargerVal = InColumnY(0)
                ElseIf iPlusj > NumberOfPoints - 1 Then
                    LargerVal = InColumnY(NumberOfPoints - 1)
                Else
                    LargerVal = InColumnY(i + j)
                End If

                If iMinusj < 0 Then
                    SmallerVal = InColumnY(0)
                ElseIf iMinusj > NumberOfPoints - 1 Then
                    SmallerVal = InColumnY(NumberOfPoints - 1)
                Else
                    SmallerVal = InColumnY(i - j)
                End If

                TempSum += SmallerVal * (SGCoef(Me.NearestNeighborWindow, j + 1))
                TempSum += LargerVal * (SGCoef(Me.NearestNeighborWindow, j + 1))
            Next
            SmoothedY(i) = TempSum / SGCoef(Me.NearestNeighborWindow, 0)
        Next

        Return New List(Of Double)(SmoothedY.ToArray)
    End Function

    ''' <summary>
    ''' Settings-Control.
    ''' </summary>
    Private SmoothingControl As New cNumericSmoothingPanel_SavitzkyGolay

    ''' <summary>
    ''' Returns the settings control for this smoothing method.
    ''' </summary>
    Public Function SmoothingOptions() As UserControl Implements iNumericSmoothingFunction.SmoothingOptions
        Return Me.SmoothingControl
    End Function

End Class
