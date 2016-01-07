Public Class mValueRangeSelector2

    ' Set Start-Parameters for Max and Min Values, to be filled with the values from the data-set.
    Private MinValue As Double = Double.MaxValue
    Private MaxValue As Double = Double.MinValue

    ''' <summary>
    ''' Selected Maximum Value
    ''' </summary>
    Public ReadOnly Property SelectedMaxValue As Double
        Get
            If Me.SelectedPixelMax = Nothing Then
                Return Me.MaxValue
            End If
            Return Me.SelectedPixelMax * Me.ValueRangePerPixelRow + Me.MinValue
        End Get
    End Property

    ''' <summary>
    ''' Selected Minimum Value
    ''' </summary>
    Public ReadOnly Property SelectedMinValue As Double
        Get
            Return Me.SelectedPixelMin * Me.ValueRangePerPixelRow + Me.MinValue
        End Get
    End Property

    ' Saves the selected Max and Min pixels to paint the marker lines.
    Private SelectedPixelMin As Integer
    Private SelectedPixelMax As Integer

    ' Saves the values to analyze:
    Private ValueList As Double()

    ' Saves the number of pixels to display in the selection area.
    Private PixelStackCounts As New List(Of Integer)

    ' Saves the calculation parameter for the value corresponding to one pixel
    Private ValueRangePerPixelRow As Double = 0

    ' Event that tells the program, that the selection has changed.
    Public Event SelectedRangeChanged()

    ''' <summary>
    ''' Constructor, which activates Double-Buffering.
    ''' </summary>
    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.DoubleBuffered = True
    End Sub

    ''' <summary>
    ''' Sets the Array of Values that should be shown in the Value-Selector.
    ''' Counts the individual values by dividing the paint area into different regions.
    ''' </summary>
    Public Sub SetValueArray(ByRef InputValueList As Double(),
                             Optional ByVal UnitSymbol As String = "")
        Me.ValueList = InputValueList

        Me.MinValue = cNumericalMethods.GetMinimumValue(InputValueList)
        Me.MaxValue = cNumericalMethods.GetMaximumValue(InputValueList)

        ' Reset selected Min and Max Values:
        Me.SelectedPixelMin = 0
        Me.SelectedPixelMax = Nothing

        ' Calculate Paint-Parameters from Data
        Me.CalculatePaintParameters()
    End Sub

    ''' <summary>
    ''' Calculates the Paint-Area Curve.
    ''' Function is called on each resize.
    ''' </summary>
    Private Sub CalculatePaintParameters() Handles MyBase.Resize
        If ValueList Is Nothing Then Return

        ' Calculate Factors to divide Paint-Area corresponding to value properties.
        Me.ValueRangePerPixelRow = (Me.MaxValue - Me.MinValue) / Me.pValueHistogram.Height

        ' Clear Value-Stack-Count-Buffer:
        Me.PixelStackCounts.Clear()
        For i As Integer = 0 To Me.pValueHistogram.Height Step 1
            Me.PixelStackCounts.Add(0)
        Next

        ' Fill Count-Buffer with Values:
        If Me.ValueRangePerPixelRow > 0 Then
            For i As Integer = 0 To Me.ValueList.Length - 1 Step 1
                If Double.IsNaN(Me.ValueList(i)) Then Continue For
                Dim CorrespondingStackNumber As Integer = Convert.ToInt32((Me.ValueList(i) - Me.MinValue) / ValueRangePerPixelRow)
                If CorrespondingStackNumber <= Me.pValueHistogram.Height Then
                    Me.PixelStackCounts(CorrespondingStackNumber) += 1
                End If
            Next
        End If

        ' Search maximum Count Number to get a proper scale for the image:
        Dim MaximumCountNumber As Integer = 0
        For Each i As Integer In Me.PixelStackCounts
            If i > MaximumCountNumber Then MaximumCountNumber = i
        Next

        ' Recalculate Pixel-Stack-Counts to actual Pixel-Widths in the Image:
        Dim PixelsPerCount As Double = 0
        If MaximumCountNumber > 0 Then
            PixelsPerCount = (Me.pValueHistogram.Width - 4) / MaximumCountNumber
        End If
        For i As Integer = 0 To Me.PixelStackCounts.Count - 1 Step 1
            Me.PixelStackCounts(i) = Convert.ToInt32(Me.PixelStackCounts(i) * PixelsPerCount)
        Next

        ' Initialize Repaint.
        Me.Refresh()
    End Sub

    ''' <summary>
    ''' Selects the Maximum and Minimum Point, that is selected.
    ''' Left-Click sets the maximum point, 
    ''' Right-Click sets the minimum point.
    ''' </summary>
    Private Sub PaintArea_MouseClick(sender As Object, e As MouseEventArgs) Handles pValueHistogram.MouseClick, pValueHistogram.MouseMove, pColorScale.MouseMove, pColorScale.MouseClick
        Dim bValuesChanged As Boolean = False
        Select Case e.Button
            Case MouseButtons.Left
                ' Left-Mouse-Button = set selected maximum value
                '################################################
                ' Allow only plausible borders, that are not larger than the corresponding counterparts.
                If e.Y > Me.SelectedPixelMin Then
                    Me.SelectedPixelMax = e.Y
                    bValuesChanged = True
                End If
            Case MouseButtons.Right
                ' Right-Mouse-Button = set selected minimum value
                '################################################
                ' Allow only plausible borders, that are not larger than the corresponding counterparts.
                If e.Y < Me.SelectedPixelMax Then
                    Me.SelectedPixelMin = e.Y
                    bValuesChanged = True
                End If
        End Select

        If bValuesChanged Then
            RaiseEvent SelectedRangeChanged()

            ' Initialize Repaint-Process
            Me.Refresh()
        End If
    End Sub

    ''' <summary>
    ''' Fills the TextBoxes with the correct values:
    ''' </summary>
    Private Sub FillTextBoxes()
        'Me.txtMax.Text = Me.SelectedMaxValue.ToString("E3")
        'Me.txtMin.Text = Me.SelectedMinValue.ToString("E3")
    End Sub

    ''' <summary>
    ''' Set Markers again to cover the full value-range:
    ''' </summary>
    Private Sub btnFullScale_Click(sender As System.Object, e As System.EventArgs) Handles btnScale_FullScale.Click, btnScale_Sigma.Click, btnScale_Weighted.Click
        Me.SelectedPixelMax = Nothing
        Me.SelectedPixelMin = 0

        RaiseEvent SelectedRangeChanged()

        Me.Refresh()
    End Sub

    ''' <summary>
    ''' Paints the Calculated Data to the Image.
    ''' </summary>
    Private Sub pPaintArea_Paint(sender As System.Object, e As PaintEventArgs) Handles pValueHistogram.Paint, pColorScale.Paint
        ' Initialize Repaint-Event:
        Dim gPaintArea As Graphics = e.Graphics

        ' Create pen for Drawing the Pixels:
        Dim Pen_StatisticData As New Pen(Color.YellowGreen, 1)
        Dim Brush_SelectionArea As Brush = Brushes.Green

        ' Draw Selection-Area
        If Me.SelectedPixelMax = Nothing Then Me.SelectedPixelMax = Me.pValueHistogram.Height
        gPaintArea.FillRectangle(Brush_SelectionArea, 0, Me.SelectedPixelMin, Me.pValueHistogram.Width, Me.SelectedPixelMax - Me.SelectedPixelMin)

        ' Draw Statistics-Data:
        For i As Integer = 0 To Me.PixelStackCounts.Count - 1 Step 1
            gPaintArea.DrawLine(Pen_StatisticData, 0, i, Me.PixelStackCounts(i), i)
        Next

        ' Dispose all pens again.
        Pen_StatisticData.Dispose()
        Brush_SelectionArea.Dispose()

        ' Fill TextBoxes with the correct values:
        Me.FillTextBoxes()
    End Sub
End Class
