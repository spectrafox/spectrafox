Imports System.Threading

Public Class cSpectroscopyTableAFMForceCurrentDeconvolution
    Inherits cSpectroscopyTableFetcher

#Region "Properties"

    ''' <summary>
    ''' Cutoff for the integral.
    ''' </summary>
    Public Const TMinusZCutoff As Double = 0.00001

    ''' <summary>
    ''' Spring constant of the AFM: N/m
    ''' Default: 1800 N/m = qPlus sensor
    ''' </summary>
    Public Property SpringConstant As Double = 1800

    ''' <summary>
    ''' Oscillation-Amplitude of the tip.
    ''' </summary>
    Public Property OscillationAmplitude As Double = 0

    ''' <summary>
    ''' Resonance-Frequency of the current tip.
    ''' </summary>
    Public Property ResonanceFrequency As Double = 0

    ''' <summary>
    ''' Smoothing algorithm, that is used to smooth the data in advance.
    ''' </summary>
    Public Property DataSmoother As iNumericSmoothingFunction

    ''' <summary>
    ''' Determines, if the deconvolution routine shall remove
    ''' the offset in the frequency shift signal to zero.
    ''' </summary>
    Public Property RemoveFrequencyShiftOffsetToZero As Boolean = True

    ''' <summary>
    ''' Variable set to true, during the deconvolution process.
    ''' </summary>
    Private _DeconvolutionInProgress As Boolean = False

    ''' <summary>
    ''' Returns, if a deconvolution process is currently in progress
    ''' in the ASync background thread.
    ''' </summary>
    Public ReadOnly Property DeconvolutionInProgress As Boolean
        Get
            Return Me._DeconvolutionInProgress
        End Get
    End Property

    ''' <summary>
    ''' Column for storing the deconvoluted force.
    ''' </summary>
    Private _DeconvolutedForce As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the column with the deconvoluted force data AFTER the procedure has been launched.
    ''' </summary>
    Public ReadOnly Property DeconvolutedForce As cSpectroscopyTable.DataColumn
        Get
            If Me._DeconvolutedForce Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._DeconvolutedForce
        End Get
    End Property

    ''' <summary>
    ''' Column for storing the deconvoluted current.
    ''' </summary>
    Private _DeconvolutedCurrent As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the column with the deconvoluted current data AFTER the procedure has been launched.
    ''' </summary>
    Public ReadOnly Property DeconvolutedCurrent As cSpectroscopyTable.DataColumn
        Get
            If Me._DeconvolutedCurrent Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._DeconvolutedCurrent
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function after the Async call of the deconvolution.
    ''' </summary>
    Private AsyncCallback As New WaitCallback(AddressOf PerformDeconvolution)

    ''' <summary>
    ''' Name of the input data column.
    ''' </summary>
    Private _InputColumnName_ZColumn As String = String.Empty

    ''' <summary>
    ''' Name of the input data column.
    ''' </summary>
    Private _InputColumnName_AverageCurrent As String = String.Empty

    ''' <summary>
    ''' Name of the input data column.
    ''' </summary>
    Private _InputColumnName_FrequencyShift As String = String.Empty

    ''' <summary>
    ''' Name out the output data column.
    ''' </summary>
    Public Property OutputColumnName_Force As String = String.Empty

    ''' <summary>
    ''' Name out the output data column.
    ''' </summary>
    Public Property OutputColumnName_Current As String = String.Empty

#End Region

#Region "Events"

    ''' <summary>
    ''' Event that gets fired, when the deconvolution was successfull.
    ''' </summary>
    Public Event CurrentForceDeconvolutionComplete(ByRef SpectroscopyTable As cSpectroscopyTable,
                                                   ByRef ForceColumn As cSpectroscopyTable.DataColumn,
                                                   ByRef CurrentColumn As cSpectroscopyTable.DataColumn)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub

#End Region

#Region "Deconvolution Function"

    ''' <summary>
    ''' Starts the deconvolution procedure in the background.
    ''' Expects the SpectroscopyTableObject to be loaded already from the FileObject.
    ''' </summary>
    Public Sub DeconvoluteWithoutFetch_Async(ByVal InputName_Z As String,
                                             ByVal InputName_FrequencyShift As String,
                                             ByVal InputName_CurrentSignal As String,
                                             ByVal OutputName_Force As String,
                                             ByVal OutputName_Current As String)

        Me._InputColumnName_ZColumn = InputName_Z
        Me._InputColumnName_FrequencyShift = InputName_FrequencyShift
        Me._InputColumnName_AverageCurrent = InputName_CurrentSignal
        Me._OutputColumnName_Force = OutputName_Force
        Me._OutputColumnName_Current = OutputName_Current

        ' Send the SpectroscopyTable to the ThreadPoolQueue für Async processing.
        ThreadPool.QueueUserWorkItem(AsyncCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private FetchRunning As Boolean = False
    Public ReadOnly Property DeconvolutionProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.FetchRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the deconvolution procedure in the background. First of all initializes a FileFetch,
    ''' and waits for the async filefetch to complete.
    ''' </summary>
    Public Sub DeconvoluteWITHFetch_Async(ByVal InputName_Z As String,
                                          ByVal InputName_FrequencyShift As String,
                                          ByVal InputName_CurrentSignal As String,
                                          ByVal OutputName_Force As String,
                                          ByVal OutputName_Current As String)

        Me._InputColumnName_ZColumn = InputName_Z
        Me._InputColumnName_FrequencyShift = InputName_FrequencyShift
        Me._InputColumnName_AverageCurrent = InputName_CurrentSignal
        Me._OutputColumnName_Force = OutputName_Force
        Me._OutputColumnName_Current = OutputName_Current

        FetchRunning = True

        ' Send the fetch to the background class.
        Me.FetchAsync()

    End Sub

    ''' <summary>
    ''' Function that catches the FileFetchEvent and starts the deconvolution.
    ''' </summary>
    Private Sub FileFetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not Me.FetchRunning Then Return
        FetchRunning = False

        ' Start the deconvolution with the freshly fetched file.
        Me.DeconvoluteWithoutFetch_Async(Me._InputColumnName_ZColumn,
                                         Me._InputColumnName_FrequencyShift,
                                         Me._InputColumnName_AverageCurrent,
                                         Me._OutputColumnName_Force,
                                         Me._OutputColumnName_Current)
    End Sub

    ''' <summary>
    ''' Starts the deconvolution procedure in the background.
    ''' Expects the SpectroscopyTableObject to be loaded already from the FileObject.
    ''' </summary>
    Public Sub DeconvoluteWithoutFetch_Direct(ByVal InputName_Z As String,
                                              ByVal InputName_FrequencyShift As String,
                                              ByVal InputName_CurrentSignal As String,
                                              ByVal OutputName_Force As String,
                                              ByVal OutputName_Current As String)

        Me._InputColumnName_ZColumn = InputName_Z
        Me._InputColumnName_FrequencyShift = InputName_FrequencyShift
        Me._InputColumnName_AverageCurrent = InputName_CurrentSignal
        Me._OutputColumnName_Force = OutputName_Force
        Me._OutputColumnName_Current = OutputName_Current

        ' Directly launch the deconvolution.
        Me.PerformDeconvolution(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Starts the deconvolution procedure in the background.
    ''' Loads the FileObject directly in advance to the deconvolution.
    ''' </summary>
    Public Sub DeconvoluteWITHFetch_Direct(ByVal InputName_Z As String,
                                           ByVal InputName_FrequencyShift As String,
                                           ByVal InputName_CurrentSignal As String,
                                           ByVal OutputName_Force As String,
                                           ByVal OutputName_Current As String)

        Me._InputColumnName_ZColumn = InputName_Z
        Me._InputColumnName_FrequencyShift = InputName_FrequencyShift
        Me._InputColumnName_AverageCurrent = InputName_CurrentSignal
        Me._OutputColumnName_Force = OutputName_Force
        Me._OutputColumnName_Current = OutputName_Current

        ' Fetch the file
        Me.FetchDirect()

        ' Directly launch the deconvolution.
        Me.PerformDeconvolution(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Callback function, to be called with the fetched SpectroscopyTable.
    ''' Can be called Async. This function performs the deconvolution of the current and the force signal.
    ''' From the force gradient and the average current of the measurement signal.
    ''' </summary>
    Private Sub PerformDeconvolution(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)
        Me._DeconvolutionInProgress = True

        ' Output file
        Dim OutputSpectroscopyTable As New cSpectroscopyTable

        Try
            '##########################################
            '
            '           Data Check Section
            '
            '##########################################

            ' Check, that the columns exist
            If Not SpectroscopyTable.ColumnExists(Me._InputColumnName_AverageCurrent) Then
                Throw New ArgumentException("input column not found: average current signal")
            End If
            If Not SpectroscopyTable.ColumnExists(Me._InputColumnName_FrequencyShift) Then
                Throw New ArgumentException("input column not found: frequency shift signal")
            End If
            If Not SpectroscopyTable.ColumnExists(Me._InputColumnName_ZColumn) Then
                Throw New ArgumentException("input column not found: z approach signal")
            End If

            ' Get the column-references for quicker access
            Dim Column_Z As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me._InputColumnName_ZColumn)
            Dim Column_AverageCurrent As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me._InputColumnName_AverageCurrent)
            Dim Column_FrequencyShift As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me._InputColumnName_FrequencyShift)

            ' Check the monotonicity of the Z-Column.
            ' Only allow strictly monotonic raising OR falling!
            Dim ZMonotonicity As cSpectroscopyTable.DataColumn.Monotonicities = Column_Z.Monotonicity

            If ZMonotonicity <> cSpectroscopyTable.DataColumn.Monotonicities.StrictFalling AndAlso
               ZMonotonicity <> cSpectroscopyTable.DataColumn.Monotonicities.StrictRising Then
                Throw New ArgumentException("z-approach is not strictly monotonic")
            End If


            '##########################################
            '
            '           Deconvolution Section
            '
            '##########################################

            ' Now get references to the value arrays for faster access
            Dim Column_Z_Values As ReadOnlyCollection(Of Double) = Column_Z.Values

            ' First calculate the numeric derivative of the input-columns:
            Dim Column_FrequencyShiftSmoothed As New cSpectroscopyTable.DataColumn
            Dim Column_AverageCurrentSmoothed As New cSpectroscopyTable.DataColumn

            Dim Column_FrequencyShiftDerivative As New cSpectroscopyTable.DataColumn
            Dim Column_AverageCurrentDerivative As New cSpectroscopyTable.DataColumn

            Dim Column_FrequencyShiftDerivativeSmoothed As New cSpectroscopyTable.DataColumn
            Dim Column_AverageCurrentDerivativeSmoothed As New cSpectroscopyTable.DataColumn

            ' Check, if we have a smoothing algorithm selected.
            ' If not, use by default the raw smoothing algorithm.
            If DataSmoother Is Nothing Then
                DataSmoother = New cNumericSmoothing_NoSmooth
            End If

            Column_FrequencyShiftSmoothed.SetValueList(DataSmoother.Smooth(Column_FrequencyShift.Values))
            Column_AverageCurrentSmoothed.SetValueList(DataSmoother.Smooth(Column_AverageCurrent.Values))
            Column_FrequencyShiftSmoothed.Name = "FreqShift Smoothed"
            Column_AverageCurrentSmoothed.Name = "Current Smoothed"

            ' Simple non-parallel approach
            Dim StartLoop As Integer
            Dim EndLoop As Integer
            Dim StepLoop As Integer
            If ZMonotonicity = cSpectroscopyTable.DataColumn.Monotonicities.StrictRising Then
                StartLoop = Column_Z.CurrentCropInformation.MinIndexIncl
                EndLoop = Column_Z.CurrentCropInformation.MaxIndexIncl
                'StartLoop = 0
                'EndLoop = Column_Z_Values.Count - 1
                StepLoop = 1
            Else
                EndLoop = Column_Z.CurrentCropInformation.MinIndexIncl
                StartLoop = Column_Z.CurrentCropInformation.MaxIndexIncl
                'StartLoop = Column_Z_Values.Count - 1
                'EndLoop = 0
                StepLoop = -1
            End If

            ' Get the rescaling factor for the integrals.
            Dim ZStepSize As Double = (Column_Z_Values(EndLoop) - Column_Z_Values(StartLoop)) / Column_Z_Values.Count

            ' Remove the offset in the frequency shift signal,
            ' if requested. This removes a constant force signal,
            ' which may be considered as background.
            If RemoveFrequencyShiftOffsetToZero Then

                ' Get a reference to the signal with the offset
                Dim FrequencyShiftWithOffset As ReadOnlyCollection(Of Double) = Column_FrequencyShiftSmoothed.Values

                ' take the last values of the frequency-shift signal which are not NaN,
                ' and set the average of this value as the new offset
                Dim AverageOffset As Double = FrequencyShiftWithOffset(EndLoop)
                Dim k As Integer = 1
                While Double.IsNaN(AverageOffset) And ((EndLoop - k) >= 0 And (EndLoop - k) < FrequencyShiftWithOffset.Count)
                    AverageOffset = FrequencyShiftWithOffset(EndLoop - k)
                    k += StepLoop
                End While

                ' Now subtract the offset from the frequency shift signal.
                Dim FrequencyShiftWITHOUTOffset(FrequencyShiftWithOffset.Count - 1) As Double
                For i As Integer = 0 To FrequencyShiftWithOffset.Count - 1 Step 1
                    FrequencyShiftWITHOUTOffset(i) = FrequencyShiftWithOffset(i) - AverageOffset
                Next

                ' Replace the frequency shift signal with the one without offset.
                Column_FrequencyShiftSmoothed.SetValueList(FrequencyShiftWITHOUTOffset.ToList)
            End If

            Column_FrequencyShiftDerivative.SetValueList(cNumericalMethods.NumericalDerivative(Column_Z, Column_FrequencyShiftSmoothed))
            Column_AverageCurrentDerivative.SetValueList(cNumericalMethods.NumericalDerivative(Column_Z, Column_AverageCurrentSmoothed))
            Column_FrequencyShiftDerivative.Name = "FreqShift Derivative"
            Column_AverageCurrentDerivative.Name = "Current Derivative"

            Column_FrequencyShiftDerivativeSmoothed.SetValueList(DataSmoother.Smooth(Column_FrequencyShiftDerivative.Values))
            Column_AverageCurrentDerivativeSmoothed.SetValueList(DataSmoother.Smooth(Column_AverageCurrentDerivative.Values))
            Column_FrequencyShiftDerivativeSmoothed.Name = "FreqShift Derivative Smoothed"
            Column_AverageCurrentDerivativeSmoothed.Name = "Current Derivative Smoothed"

            ' Now get references to the value arrays for faster access
            Dim Column_FrequencyShift_Values As ReadOnlyCollection(Of Double) = Column_FrequencyShiftSmoothed.Values
            Dim Column_AverageCurrent_Values As ReadOnlyCollection(Of Double) = Column_AverageCurrentSmoothed.Values
            Dim Column_FrequencyShiftDerivative_Values As ReadOnlyCollection(Of Double) = Column_FrequencyShiftDerivativeSmoothed.Values
            Dim Column_AverageCurrentDerivative_Values As ReadOnlyCollection(Of Double) = Column_AverageCurrentDerivativeSmoothed.Values

            ' Create the output columns
            Dim ForceValues(Column_Z_Values.Count - 1) As Double
            Dim CurrentValues(Column_Z_Values.Count - 1) As Double

            '#########################################
            ' Second, we perform the numeric integral

            ' Get temporary variables
            Dim ForceValue As Double
            Dim CurrentValue As Double
            Dim zValue As Double
            Dim tValue As Double
            Dim TMinusZ As Double
            Dim Force_Part1 As Double
            Dim Force_Part2 As Double
            Dim Current_Part1 As Double
            Dim Current_Part2 As Double

            Dim ForceAdd As Double
            Dim CurrentAdd As Double

            For zCount As Integer = StartLoop To EndLoop Step StepLoop

                ' Get the current Z-Value
                zValue = Column_Z_Values(zCount)
                ForceValue = 0
                CurrentValue = 0

                For tCount As Integer = zCount To EndLoop Step StepLoop

                    ' Get the current integration z-value
                    tValue = Column_Z_Values(tCount)

                    ' Get a common factor, and check for its size > 0
                    TMinusZ = tValue - zValue

                    '#################
                    ' Force Integral.
                    If TMinusZ > Me.OscillationAmplitude * TMinusZCutoff Then
                        Force_Part1 = 1 + Math.Pow(Me.OscillationAmplitude, 0.5) / (8 * Math.Sqrt(Math.PI * TMinusZ))
                        Force_Part2 = -Math.Pow(Me.OscillationAmplitude, 1.5) / Math.Sqrt(2 * TMinusZ)
                    Else
                        Force_Part1 = 1
                        Force_Part2 = 0
                    End If

                    ForceAdd = (Force_Part1 * Column_FrequencyShift_Values(tCount) + Force_Part2 * Column_FrequencyShiftDerivative_Values(tCount))
                    'ForceValue += (Force_Part1 * Column_FrequencyShift_Values(tCount) + Force_Part2 * Column_FrequencyShiftDerivative_Values(tCount))
                    If Not Double.IsNaN(ForceAdd) Then ForceValue += ForceAdd
                    '
                    '################

                    '################
                    ' Current Integral
                    Current_Part1 = Me.GetValueOfZArrayInterpolated(Column_Z_Values, ZMonotonicity, Column_AverageCurrentDerivative_Values, tValue, StartLoop, EndLoop, StepLoop)
                    Current_Part2 = Me.GetValueOfZArrayInterpolated(Column_Z_Values, ZMonotonicity, Column_AverageCurrentDerivative_Values, tValue + Me.OscillationAmplitude, StartLoop, EndLoop, StepLoop)

                    'If TMinusZ > Me.OscillationAmplitude * TMinusZCutoff Then
                    If TMinusZ <> 0 Then
                        CurrentAdd = Math.Sqrt((2 * Me.OscillationAmplitude) / (TMinusZ)) * (Current_Part1 - Math.Sqrt(2 / Math.PI) * Current_Part2)
                        If Not Double.IsNaN(CurrentAdd) Then
                            CurrentValue += CurrentAdd
                        End If
                    End If
                    '
                    '################

                Next

                ' Finally multiply the prefactors
                ForceValue = ForceValue * 2 * Me.SpringConstant / Me.ResonanceFrequency * ZStepSize
                CurrentValue = Me.GetValueOfZArrayInterpolated(Column_Z_Values, ZMonotonicity, Column_AverageCurrent_Values, zValue + Me.OscillationAmplitude, StartLoop, EndLoop, StepLoop) - CurrentValue * ZStepSize

                ' Add the current and force integral values to the value array.
                ForceValues(zCount) = ForceValue
                CurrentValues(zCount) = CurrentValue

            Next

            '################################
            ' Write the output columns.

            Me._DeconvolutedForce = New cSpectroscopyTable.DataColumn
            Me._DeconvolutedForce.SetValueList(ForceValues.ToList)
            Me._DeconvolutedCurrent = New cSpectroscopyTable.DataColumn
            Me._DeconvolutedCurrent.SetValueList(CurrentValues.ToList)

            ' Change the new names.
            Me._DeconvolutedForce.Name = Me._OutputColumnName_Force
            Me._DeconvolutedForce.UnitSymbol = "N"
            Me._DeconvolutedCurrent.Name = Me._OutputColumnName_Current
            Me._DeconvolutedCurrent.UnitSymbol = "A"

            ' Create the output SpectroscopyTable.
            OutputSpectroscopyTable.AddNonPersistentColumn(Column_Z)
            OutputSpectroscopyTable.AddNonPersistentColumn(Me._DeconvolutedCurrent)
            OutputSpectroscopyTable.AddNonPersistentColumn(Me._DeconvolutedForce)

            ' Add additional checks
            OutputSpectroscopyTable.AddNonPersistentColumn(Column_AverageCurrentSmoothed)
            OutputSpectroscopyTable.AddNonPersistentColumn(Column_FrequencyShiftSmoothed)
            OutputSpectroscopyTable.AddNonPersistentColumn(Column_AverageCurrentDerivative)
            OutputSpectroscopyTable.AddNonPersistentColumn(Column_FrequencyShiftDerivative)
            OutputSpectroscopyTable.AddNonPersistentColumn(Column_AverageCurrentDerivativeSmoothed)
            OutputSpectroscopyTable.AddNonPersistentColumn(Column_FrequencyShiftDerivativeSmoothed)

        Catch ex As Exception
            MessageBox.Show(ex.Message, My.Resources.title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me._DeconvolutionInProgress = False

            ' Raise the event after the deconvolution.
            RaiseEvent CurrentForceDeconvolutionComplete(OutputSpectroscopyTable,
                                                         Me._DeconvolutedForce,
                                                         Me._DeconvolutedCurrent)
        End Try
    End Sub

    ''' <summary>
    ''' Returns from the value arrays (Zrel, Values) the value at the position (Position)
    ''' </summary>
    Private Function GetValueOfZArrayInterpolated(ByRef Zrel As ReadOnlyCollection(Of Double),
                                                  ByVal ZMonotonicity As cSpectroscopyTable.DataColumn.Monotonicities,
                                                  ByRef Values As ReadOnlyCollection(Of Double),
                                                  ByVal Position As Double,
                                                  ByVal StartLoop As Integer,
                                                  ByVal EndLoop As Integer,
                                                  ByVal StepLoop As Integer) As Double

        If Double.IsNaN(Position) Then Return Double.NaN
        If EndLoop = StartLoop Then Return Double.NaN

        Dim ZStepSize As Double = (Zrel(EndLoop) - Zrel(StartLoop)) / Zrel.Count
        Dim ZStartValue As Double = Zrel(StartLoop)

        ' reference to the next element
        Dim n As Double = (Zrel(EndLoop) - Position) / ZStepSize

        ' Save an interger version of the element reference.
        Dim IntN As Integer = CInt(Math.Floor(n))

        ' // Now distinguish between different cases.
        ' /- Arraylength not matching
        ' /- interpolation between requested points, etc.
        If Values.Count <= 1 Then
            ' Some settings are wrong. Just return 0.
            Return 0
        ElseIf n < 0 Then
            ' Interpolate to the left

            ' in the beginning just return the first value
            Return Values(0)

        ElseIf n >= Values.Count - 2 Then
            ' Interpolate to the right

            ' in the beginning just return the last value
            Return Values(Values.Count - 1)
        ElseIf IntN = n Then
            ' If the value matches directly a precalculated value,
            ' then just return the value immediatelly.
            Return Values(IntN)
        Else
            ' the requested value lies between two calculated points,
            ' so lets interpolate between these two points the values.

            '##############################################
            ' USING SPLINE INTERPOLATION NOW.... SEE BELOW
            ' linear interpolation
            Dim val As Double = Values(IntN + 1) - Values(IntN)
            val = val * (n - IntN)
            Return Values(IntN) + val

            '' Cosine Interpolation
            'Dim y1 As Double = CurrentCache(IntN)
            'Dim y2 As Double
            'Dim EvaluateAtX As Double = n - IntN

            'If IntN >= CurrentCache.Length - 2 Then
            '    y2 = CurrentCache(IntN)
            'Else
            '    y2 = CurrentCache(IntN + 1)
            'End If

            'Return cNumericalMethods.CosineInterpolationCudafy(y1, y2, EvaluateAtX)
            ''##############################################

            '' Polynomial Interpolation
            'Dim InterpolationPointsLeft As Integer = PointsToTakeForSplineInterpolation
            'Dim InterpolationPointsRight As Integer = PointsToTakeForSplineInterpolation

            '' Check if we have enough points to the left and right for the interpolation
            'If IntN >= Values.Count - PointsToTakeForSplineInterpolation - 1 Then
            '    InterpolationPointsRight = Values.Count - 1 - IntN
            'End If
            'If IntN < PointsToTakeForSplineInterpolation Then
            '    InterpolationPointsLeft = IntN
            'End If

            '' Calculate the size of the interpolation-arrays
            'Dim InterpolationArrayMaxIndex As Integer = InterpolationPointsRight + InterpolationPointsLeft - 1
            'Dim InterpolationArrayX(InterpolationArrayMaxIndex) As Double
            'Dim InterpolationArrayY(InterpolationArrayMaxIndex) As Double

            '' Fill the interpolation arrays.
            'Dim ArrayCounter As Integer = 0
            'For i As Integer = InterpolationPointsLeft - 1 To 1 Step -1
            '    InterpolationArrayX(ArrayCounter) = ArrayCounter
            '    InterpolationArrayY(ArrayCounter) = Values(IntN - i)

            '    ArrayCounter += 1
            'Next

            '' Fill the point next to the evaluation point
            'Dim NEffective As Double = n - IntN + ArrayCounter
            'InterpolationArrayX(ArrayCounter) = ArrayCounter
            'InterpolationArrayY(ArrayCounter) = Values(IntN)

            '' Fill the interpolation arrays.
            'For i As Integer = 1 To InterpolationPointsRight - 1 Step 1
            '    ArrayCounter += 1

            '    InterpolationArrayX(ArrayCounter) = ArrayCounter
            '    InterpolationArrayY(ArrayCounter) = Values(IntN + i)
            'Next

            '' Take care, that our interpolation is only accessed within the interpolation range
            'If NEffective < 0 Then NEffective = 0.1
            'If NEffective > ArrayCounter - 1 Then NEffective = (ArrayCounter - 1) + 0.1

            'Return cNumericalMethods.SplineInterpolationNative(InterpolationArrayX, InterpolationArrayY, NEffective)
        End If



    End Function

#End Region

#Region "Save Output Columns"

    ''' <summary>
    ''' Saves the output columns to the FileObject.
    ''' </summary>
    Public Sub SaveColumnsToFileObject(ByVal ForceSignalColumnName As String,
                                       ByVal CurrentSignalColumnName As String)

        If Me._DeconvolutedCurrent Is Nothing Then Return
        If Me._DeconvolutedForce Is Nothing Then Return

        ' Set the new names
        Me._DeconvolutedCurrent.Name = CurrentSignalColumnName
        Me._DeconvolutedForce.Name = ForceSignalColumnName

        ' Add to the columns to the data list.
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._DeconvolutedCurrent)
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._DeconvolutedForce)

    End Sub

#End Region

End Class
