Imports System.ComponentModel
Public Class mValueRangeSelector
    Implements IDisposable

#Region "Properties"

    ' Set Start-Parameters for Max and Min Values, to be filled with the values from the data-set.
    Private MinValue As Double = Double.MaxValue
    Private MaxValue As Double = Double.MinValue
    Private DataStatistics As cNumericalMethods.sNumericStatistics

    ''' <summary>
    ''' Selected Maximum Value
    ''' </summary>
    Public Property SelectedMaxValue As Double

    ''' <summary>
    ''' Selected Minimum Value
    ''' </summary>
    Public Property SelectedMinValue As Double

    ''' <summary>
    ''' Saves the values to analyze.
    ''' </summary>
    Private ValueList As Double()

    ''' <summary>
    ''' Saves the number of pixels to display in the selection area.
    ''' </summary>
    Private PixelStackCounts As New List(Of Integer)

    ''' <summary>
    ''' Saves the calculation parameter for the value corresponding to one pixel
    ''' </summary>
    Private ValueRangePerPixelRow As Double = 0

    ''' <summary>
    ''' Create pen for drawing the value statistics
    ''' </summary>
    Private PenStatisticData As New Pen(Color.Yellow, 1)

    ''' <summary>
    ''' Brush to use for the selection.
    ''' </summary>
    Private BrushSelectionArea As Brush = Brushes.Green

#End Region

#Region "Events"

    ''' <summary>
    ''' Event that is fired, if the selected value-range changed.
    ''' </summary>
    Public Event SelectedRangeChanged()

#End Region

#Region "Constructor/Destructor"

    ''' <summary>
    ''' Constructor, which activates Double-Buffering.
    ''' </summary>
    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        ' Set the values from the settings.
        Me.txtSigma.SetValue(My.Settings.ValueRangePlotter_SigmaValue,, False)

    End Sub

    ''' <summary>
    ''' Dispose-Method for the control.
    ''' Clean up brushes and pens.
    ''' </summary>
    Private Sub Destructor() Implements IDisposable.Dispose
        If Disposing Then
            ' TODO: dispose managed state (managed objects).
            Me.PenStatisticData.Dispose()
            Me.BrushSelectionArea.Dispose()
        End If

        ' TODO: free unmanaged resources (unmanaged objects).
    End Sub

#End Region

#Region "Value-Array Setters"

    ''' <summary>
    ''' Sets the Array of Values that should be shown in the Value-Selector.
    ''' Counts the individual values by dividing the paint area into different regions.
    ''' </summary>
    Public Sub SetValueArray(ByRef InputValueList As Double(),
                             Optional ByVal UnitSymbol As String = "")
        Me.ValueList = InputValueList

        ' Get some data statistics on setting new data
        Me.DataStatistics = cNumericalMethods.Statistics1D(Me.ValueList.ToList)
        Me.MinValue = cNumericalMethods.GetMinimumValue(InputValueList)
        Me.MaxValue = cNumericalMethods.GetMaximumValue(InputValueList)

        ' Reset selected Min and Max Values:
        Me.SelectedMaxValue = Me.MaxValue
        Me.SelectedMinValue = Me.MinValue

        ' Calculate Paint-Parameters from Data
        Me.CalculatePaintParameters()

        ' Raise the event
        RaiseEvent SelectedRangeChanged()
    End Sub

#End Region

    ''' <summary>
    ''' Calculates the Paint-Area Curve.
    ''' Function is called on each resize.
    ''' </summary>
    Private Sub CalculatePaintParameters() Handles MyBase.Resize
        If ValueList Is Nothing Then Return
        If Me.pPaintArea.Height <= 0 Then Return

        ' Calculate factors to divide paint-area corresponding to value properties.
        Me.ValueRangePerPixelRow = (Me.MaxValue - Me.MinValue) / Me.pPaintArea.Height

        ' Clear Value-Stack-Count-Buffer:
        Me.PixelStackCounts.Clear()
        For i As Integer = 0 To Me.pPaintArea.Height Step 1
            Me.PixelStackCounts.Add(0)
        Next

        ' Fill Count-Buffer with Values:
        If Me.ValueRangePerPixelRow > 0 Then
            For i As Integer = 0 To Me.ValueList.Length - 1 Step 1
                If Double.IsNaN(Me.ValueList(i)) Then Continue For
                Dim CorrespondingStackNumber As Integer = Convert.ToInt32((Me.ValueList(i) - Me.MinValue) / ValueRangePerPixelRow)
                If CorrespondingStackNumber <= Me.pPaintArea.Height Then
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
            PixelsPerCount = (Me.pPaintArea.Width - 4) / MaximumCountNumber
        End If
        For i As Integer = 0 To Me.PixelStackCounts.Count - 1 Step 1
            Me.PixelStackCounts(i) = Convert.ToInt32(Me.PixelStackCounts(i) * PixelsPerCount)
        Next

        ' Initialize Repaint.
        Me.Refresh()
    End Sub

#Region "Paint-Procedure"

    ''' <summary>
    ''' Paints the calculated value distribution to the image.
    ''' </summary>
    Private Sub pPaintArea_Paint(sender As System.Object, e As PaintEventArgs) Handles pPaintArea.Paint
        ' Initialize Repaint-Event:
        Dim gPaintArea As Graphics = e.Graphics

        Dim SelectedPixelMin As Integer
        Dim SelectedPixelMax As Integer
        If Me.ValueRangePerPixelRow <> 0 Then
            SelectedPixelMin = CInt(Math.Round((Me.SelectedMinValue - Me.MinValue) / Me.ValueRangePerPixelRow, 0))
            SelectedPixelMax = CInt(Math.Round((Me.SelectedMaxValue - Me.MinValue) / Me.ValueRangePerPixelRow, 0))
        Else
            SelectedPixelMin = 0
            SelectedPixelMax = Me.pPaintArea.Height
        End If

        ' Draw Selection-Area
        gPaintArea.FillRectangle(Me.BrushSelectionArea, 0, SelectedPixelMin, Me.pPaintArea.Width, SelectedPixelMax - SelectedPixelMin)

        ' Draw Statistics-Data:
        For i As Integer = 0 To Me.PixelStackCounts.Count - 1 Step 1
            gPaintArea.DrawLine(PenStatisticData, 0, i, Me.PixelStackCounts(i), i)
        Next

    End Sub

#End Region

#Region "Value-Selection"

    ''' <summary>
    ''' Selects the Maximum and Minimum Point, that is selected.
    ''' Left-Click sets the maximum point, 
    ''' Right-Click sets the minimum point.
    ''' </summary>
    Private Sub PaintArea_MouseClick(sender As Object, e As MouseEventArgs) Handles pPaintArea.MouseClick, pPaintArea.MouseMove
        If e.Button = MouseButtons.None Then Return

        Dim bValuesChanged As Boolean = False
        Dim NewSelectedValue As Double = e.Y * Me.ValueRangePerPixelRow + Me.MinValue
        Select Case e.Button
            Case MouseButtons.Left
                ' Left-Mouse-Button = set selected maximum value
                '################################################
                ' Allow only plausible borders, that are not larger than the corresponding counterparts.
                If NewSelectedValue > Me.SelectedMinValue And NewSelectedValue <> Me.SelectedMaxValue Then
                    Me.SelectedMaxValue = NewSelectedValue
                    bValuesChanged = True
                End If
            Case MouseButtons.Right
                ' Right-Mouse-Button = set selected minimum value
                '################################################
                ' Allow only plausible borders, that are not larger than the corresponding counterparts.
                If NewSelectedValue < Me.SelectedMaxValue And NewSelectedValue <> Me.SelectedMinValue Then
                    Me.SelectedMinValue = NewSelectedValue
                    bValuesChanged = True
                End If
        End Select

        ' Just repaint, if the values have been changed
        If bValuesChanged Then
            ' Raise the event
            RaiseEvent SelectedRangeChanged()

            ' Initialize Repaint-Process
            Me.pPaintArea.Refresh()
        End If
    End Sub

#End Region

#Region "Range selection by Textboxes"

    ''' <summary>
    ''' Fills the TextBoxes with the correct values:
    ''' </summary>
    Private Sub FillTextBoxes() Handles Me.SelectedRangeChanged
        Me.txtMinValue.SetValue(Me.SelectedMinValue,, False)
        Me.txtMaxValue.SetValue(Me.SelectedMaxValue,, False)
    End Sub

    ''' <summary>
    ''' Set Markers again to cover the full value-range:
    ''' </summary>
    Private Sub btnFullScale_Click(sender As System.Object, e As System.EventArgs) Handles btnFullScale.Click
        Me.SelectedMinValue = Me.MinValue
        Me.SelectedMaxValue = Me.MaxValue

        RaiseEvent SelectedRangeChanged()

        Me.Refresh()
    End Sub

    ''' <summary>
    ''' Manually change the selected value range.
    ''' </summary>
    Private Sub txtManualRangeSelection_TextChanged(ByRef NT As NumericTextbox) Handles txtMinValue.ValidValueChanged, txtMaxValue.ValidValueChanged

        ' Set the new values for the plot.
        If Me.txtMaxValue.DecimalValue < Me.txtMinValue.DecimalValue Then
            Me.SelectedMinValue = Me.txtMaxValue.DecimalValue
            Me.SelectedMaxValue = Me.txtMinValue.DecimalValue
        Else
            Me.SelectedMaxValue = Me.txtMaxValue.DecimalValue
            Me.SelectedMinValue = Me.txtMinValue.DecimalValue
        End If

        ' Raise the event
        RaiseEvent SelectedRangeChanged()

        ' Initialize Repaint-Process
        Me.pPaintArea.Refresh()
    End Sub

#End Region

#Region "Sigma Value Range plotting"

    ''' <summary>
    ''' On changing the sigma parameter, calculate the new value range
    ''' from the standard-deviation.
    ''' </summary>
    Private Sub txtSigma_TextChanged(ByRef NT As NumericTextbox) Handles txtSigma.ValidValueChanged

        ' Go through the value list and calculate the std. deviation.
        Me.SelectedMinValue = Me.DataStatistics.Mean - Me.txtSigma.DecimalValue * Me.DataStatistics.StandardDeviation
        Me.SelectedMaxValue = Me.DataStatistics.Mean + Me.txtSigma.DecimalValue * Me.DataStatistics.StandardDeviation

        ' Save as settings.
        My.Settings.ValueRangePlotter_SigmaValue = Me.txtSigma.DecimalValue

        ' Raise the event
        RaiseEvent SelectedRangeChanged()

        ' Initialize Repaint-Process
        Me.pPaintArea.Refresh()

    End Sub

    ''' <summary>
    ''' Activate the sigma plotting.
    ''' </summary>
    Private Sub btnUseSigma_Click(sender As Object, e As EventArgs) Handles btnUseSigma.Click
        Me.txtSigma_TextChanged(Me.txtSigma)
    End Sub

#End Region

End Class
