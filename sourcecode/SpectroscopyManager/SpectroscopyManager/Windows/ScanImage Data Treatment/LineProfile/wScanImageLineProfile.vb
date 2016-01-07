Imports ZedGraph

Public Class wScanImageLineProfile

#Region "Properties"

    ''' <summary>
    ''' Reference to the scan-image object to deal with.
    ''' </summary>
    Private _ScanImagePlot As cScanImagePlot = Nothing

    Private P1 As Point
    Private P2 As Point

    Private bReady As Boolean = False

#End Region

#Region "Show and ShowDialog functions"

    ''' <summary>
    ''' Show and Show-Dialog functions which get the profile data handed over.
    ''' </summary>
    Public Shadows Sub Show(ByRef ScanImagePlot As cScanImagePlot,
                            ByVal Point1 As Point,
                            ByVal Point2 As Point)
        Me._ScanImagePlot = ScanImagePlot
        Me.P1 = Point1
        Me.P2 = Point2

        MyBase.Show()
    End Sub

    ''' <summary>
    ''' Show and Show-Dialog functions which get the profile data handed over.
    ''' </summary>
    Public Shadows Function ShowDialog(ByRef ScanImagePlot As cScanImagePlot,
                                       ByVal Point1 As Point,
                                       ByVal Point2 As Point) As DialogResult
        Me._ScanImagePlot = ScanImagePlot
        Me.P1 = Point1
        Me.P2 = Point2

        Return MyBase.ShowDialog
    End Function

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    Private Sub FormLoad() Handles MyBase.Load

        ' Check, if the loaded scan-image is valid!
        If Me._ScanImagePlot Is Nothing Then
            Me.Close()
            Return
        End If

        ' Change the window title
        Me.Text &= Me._ScanImagePlot.ScanImagePlotted.ScanImageName

        ' Start the plot of the data
        Me.PlotLineProfile(Me.P1, Me.P2)

        Me.bReady = True
    End Sub

#End Region

#Region "Plotting of the line-profile"

    Public Sub PlotLineProfile(ByVal Point2 As Point, ByVal Point1 As Point)

        Dim DataPointNo As Integer = CInt(Me.nudDataPoints.Value)

        ' Get real location-coordinates for the points.
        Dim RealP1 As cNumericalMethods.Point2D = Me._ScanImagePlot.GetLocationOfScanData(Point1)
        Dim RealP2 As cNumericalMethods.Point2D = Me._ScanImagePlot.GetLocationOfScanData(Point2)

        ' Define a location vector of the real-coordinates.
        Dim LineVector As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(2)
        LineVector(0) = RealP1.x - RealP2.x
        LineVector(1) = RealP1.y - RealP2.y

        ' Now that we have the location coordinates, we can get the start- and end-points
        ' in the value-matrix, to extract the lines.
        Dim ValueMatrixP1 As Point = Me._ScanImagePlot.ScanImagePlotted.GetCoordinateInValueMatrix(RealP1)
        Dim ValueMatrixP2 As Point = Me._ScanImagePlot.ScanImagePlotted.GetCoordinateInValueMatrix(RealP2)

        ' Get the height values of the value-matrix:
        Dim RealP1_Z As Double = Me._ScanImagePlot.ScanChannelPlotted.ScanData(ValueMatrixP1.Y, ValueMatrixP1.X)
        Dim RealP2_Z As Double = Me._ScanImagePlot.ScanChannelPlotted.ScanData(ValueMatrixP2.Y, ValueMatrixP2.X)

        ' Get the radius between the points
        Dim LineIncrementStep As MathNet.Numerics.LinearAlgebra.Double.DenseVector = LineVector / DataPointNo
        Dim IncrementLength As Double = Math.Sqrt(LineIncrementStep * LineIncrementStep)

        ' Get the initial vector to start the value extraction.
        Dim LineCurrentVector As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(2)
        LineCurrentVector(0) = RealP2.x
        LineCurrentVector(1) = RealP2.y

        ' Also store the initial vector for calculation purposes.
        Dim InitialVectorP1 As New MathNet.Numerics.LinearAlgebra.Double.DenseVector(2)
        InitialVectorP1(0) = RealP2.x
        InitialVectorP1(1) = RealP2.y

        ' Create a new empty spectroscopy-table object.
        Dim SpecTable As New cSpectroscopyTable
        Dim DataColumnX As New cSpectroscopyTable.DataColumn
        Dim DataColumnY As New cSpectroscopyTable.DataColumn

        ' Now extract the data from the plotted value-matrix.
        Dim CurvePointList As New PointPairList()
        Dim R As Double
        Dim DataPoint As Double
        For i As Integer = 0 To DataPointNo - 1 Step 1

            ' Get the new value-matrix position.
            ValueMatrixP1 = Me._ScanImagePlot.ScanImagePlotted.GetCoordinateInValueMatrix(LineCurrentVector)

            ' Ignore, if we can't find a good point
            If ValueMatrixP1.X < 0 Or ValueMatrixP1.Y < 0 Then Continue For

            ' Get the data value and the length of the current vector.
            DataPoint = Me._ScanImagePlot.ScanChannelPlotted.ScanData(ValueMatrixP1.Y, ValueMatrixP1.X)
            R = i * IncrementLength

            ' add the values to the spectroscopy-table-columns
            DataColumnX.AddValueToColumn(R)
            DataColumnY.AddValueToColumn(DataPoint)

            ' Increase the vector by one step.
            LineCurrentVector += LineIncrementStep
        Next

        ' Set some properties:
        With DataColumnX
            .Name = My.Resources.rScanImageLineProfile.LineProfilePlot_AxisLabel_X
            .UnitSymbol = Me._ScanImagePlot.ScanChannelPlotted.UnitSymbol
            .UnitType = Me._ScanImagePlot.ScanChannelPlotted.Unit
        End With
        With DataColumnY
            .Name = My.Resources.rScanImageLineProfile.LineProfilePlot_AxisLabel_Y
            .UnitSymbol = Me._ScanImagePlot.ScanChannelPlotted.UnitSymbol
            .UnitType = Me._ScanImagePlot.ScanChannelPlotted.Unit
        End With

        ' Now add the columns to the SpectroscopyTable.
        SpecTable.AddNonPersistentColumn(DataColumnX)
        SpecTable.AddNonPersistentColumn(DataColumnY)
        SpecTable.MeasurementPoints = DataPointNo

        ' Now add the spectroscopy-table to the plotter.
        Me.svPlot.ClearSpectroscopyTables()
        Me.svPlot.SetSinglePreviewImage(SpecTable, DataColumnX.Name, DataColumnY.Name, False)

        '##############################################
        ' Fill the DataGridView with the plotted data.
        Me.dtProfile.SetSpectroscopyTable(SpecTable)

        '#################################################
        ' Calculate all the distances and fill the boxes!
        Dim DistanceX As Double = RealP1.x - RealP2.x
        Dim DistanceY As Double = RealP1.y - RealP2.y
        Dim DistanceZ As Double = RealP1_Z - RealP2_Z
        Dim DistanceR As Double = Math.Sqrt((DistanceX * DistanceX) + (DistanceY * DistanceY))

        ' Set the labels.
        Me.lblPositionX_Value1.Text = cUnits.GetFormatedValueString(RealP1.x)
        Me.lblPositionY_Value1.Text = cUnits.GetFormatedValueString(RealP1.y)
        Me.lblPositionZ_Value1.Text = cUnits.GetFormatedValueString(RealP1_Z)
        Me.lblPositionX_Value2.Text = cUnits.GetFormatedValueString(RealP2.x)
        Me.lblPositionY_Value2.Text = cUnits.GetFormatedValueString(RealP2.y)
        Me.lblPositionZ_Value2.Text = cUnits.GetFormatedValueString(RealP2_Z)

        Me.lblDistanceX_Value.Text = cUnits.GetFormatedValueString(DistanceX)
        Me.lblDistanceY_Value.Text = cUnits.GetFormatedValueString(DistanceY)
        Me.lblDistanceZ_Value.Text = cUnits.GetFormatedValueString(DistanceZ)
        Me.lblDistanceR_Value.Text = cUnits.GetFormatedValueString(DistanceR)

    End Sub

#End Region

#Region "UI functionality"

    ''' <summary>
    ''' Replot, if the number of data-points changed.
    ''' </summary>
    Private Sub nudDataPoints_ValueChanged(sender As Object, e As EventArgs) Handles nudDataPoints.ValueChanged
        If Not Me.bReady Then Return

        Me.PlotLineProfile(Me.P1, Me.P2)
    End Sub

#End Region

End Class