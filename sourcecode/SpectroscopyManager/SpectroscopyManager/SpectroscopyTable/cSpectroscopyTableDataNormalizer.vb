Imports System.Threading

Public Class cSpectroscopyTableDataNormalizer
    Inherits cSpectroscopyTableDataSmoother

#Region "Properties"
    ''' <summary>
    ''' Object for storing the normalized Column
    ''' </summary>
    Private _NormalizedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the Current Normalized Column
    ''' </summary>
    Public ReadOnly Property NormalizedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._NormalizedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._NormalizedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to Normalize the Data in a separate Thread.
    ''' </summary>
    Private FileNormalizedCallback As New WaitCallback(AddressOf SpectroscopyFileNormalizer)

    Private ColumnNameConsideredAsX As String = ""
    Private ColumnNameConsideredAsY As String = ""
    Private LeftLimit As Double
    Private RightLimit As Double
    Private NormalizedColumnTargetName As String
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the Normalization was successfull.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event FileNormalizedComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef NormalizedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Normalization Function"
    ''' <summary>
    ''' Starts the Normalization for selected Column taken as X and the Smoothed Column.
    ''' This function does not perform a smoothing on the data in advance. It assumes, that the
    ''' Column in SmoothedColumn is already treated.
    ''' </summary>
    Public Sub NormalizeColumnWithoutSmoothing_ASync(ByVal ColumnNameConsideredAsX As String,
                                                     ByVal ColumnNameToNormalize As String,
                                                     ByVal LeftLimitValueOfAverageInterval As Double,
                                                     ByVal RightLimitValueOfAverageInterval As Double,
                                                     Optional ByVal NormalizedColumnTargetName As String = "Normalized Result")
        Me.ColumnNameConsideredAsX = ColumnNameConsideredAsX
        Me.ColumnNameConsideredAsY = ColumnNameToNormalize
        Me.NormalizedColumnTargetName = NormalizedColumnTargetName
        Me.LeftLimit = LeftLimitValueOfAverageInterval
        Me.RightLimit = RightLimitValueOfAverageInterval

        ' Send SpectroscopyTable to ThreadPoolQueue of the DataDerivator
        ThreadPool.QueueUserWorkItem(FileNormalizedCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private NormalizationWithSmoothingRunning As Boolean = False
    Public ReadOnly Property NormalizationProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.NormalizationWithSmoothingRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the Normalization for selected Column taken as X and a certain Index for the
    ''' Column to smooth before. This function treats all events of the multi-threaded-Smoothing
    ''' procedure, before initializing the derivation.
    ''' </summary>
    Public Sub NormalizeColumnWITHSmoothing_Async(ByVal ColumnNameConsideredAsX As String,
                                                  ByVal ColumnNameToNormalize As String,
                                                  ByVal LeftLimitValueOfAverageInterval As Double,
                                                  ByVal RightLimitValueOfAverageInterval As Double,
                                                  ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                                  ByVal SmoothParameter As Integer,
                                                  Optional ByVal NormalizedColumnTargetName As String = "Normalized Result")
        Me.ColumnNameConsideredAsX = ColumnNameConsideredAsX
        Me.ColumnNameConsideredAsY = ColumnNameToNormalize
        Me.LeftLimit = LeftLimitValueOfAverageInterval
        Me.RightLimit = RightLimitValueOfAverageInterval

        Me.NormalizedColumnTargetName = NormalizedColumnTargetName

        NormalizationWithSmoothingRunning = True

        ' Send SpectroscopyTable to ThreadPoolQueue of the DataSmoother
        Me.SmoothColumnWITHAutomaticFetching_Async(ColumnNameToNormalize,
                                             SmoothProcedure,
                                             SmoothParameter)
    End Sub

    ''' <summary>
    ''' Function that catches the Smoothing-Complete-Event and starts the Normalization
    ''' </summary>
    Private Sub SmoothingCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable,
                                         ByRef SmoothedColumn As cSpectroscopyTable.DataColumn) Handles MyBase.FileSmoothingComplete
        If Not NormalizationWithSmoothingRunning Then Return
        NormalizationWithSmoothingRunning = False

        ' Start the Normalization using the Smoothed Data.
        Me.NormalizeColumnWithoutSmoothing_ASync(Me.ColumnNameConsideredAsX,
                                                 Me.ColumnNameConsideredAsY,
                                                 Me.LeftLimit,
                                                 Me.RightLimit,
                                                 Me.NormalizedColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the Normalization for selected Column taken as X and a certain Index for the
    ''' Column to smooth before.
    ''' </summary>
    Public Sub NormalizeColumnWITHSmoothing_Direct(ByVal ColumnNameConsideredAsX As String,
                                                    ByVal ColumnNameToNormalize As String,
                                                    ByVal LeftLimitValueOfAverageInterval As Double,
                                                    ByVal RightLimitValueOfAverageInterval As Double,
                                                    ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                                    ByVal SmoothParameter As Integer,
                                                    Optional ByVal NormalizedColumnTargetName As String = "Normalized Result")
        Me.ColumnNameConsideredAsX = ColumnNameConsideredAsX
        Me.ColumnNameConsideredAsY = ColumnNameToNormalize
        Me.LeftLimit = LeftLimitValueOfAverageInterval
        Me.RightLimit = RightLimitValueOfAverageInterval

        Me.NormalizedColumnTargetName = NormalizedColumnTargetName

        ' Fetch the file and smooth it.
        Me.SmoothColumnWITHFetching_Direct(ColumnNameToNormalize,
                                           SmoothProcedure,
                                           SmoothParameter)

        ' Normalize
        Me.SpectroscopyFileNormalizer(Me.oSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Normalizer to Normalize the data in a separate Thread.
    ''' </summary>
    Private Sub SpectroscopyFileNormalizer(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me.ColumnNameConsideredAsX) Then
            'MessageBox.Show("X-axis column for normalization not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not SpectroscopyTable.ColumnExists(Me.ColumnNameConsideredAsY) Then
            'MessageBox.Show("Column for normalization not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '################################
        ' Start Normalization Procedure.

        Dim XCol As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me.ColumnNameConsideredAsX)
        Dim XColValues As ReadOnlyCollection(Of Double) = XCol.Values
        Dim ColToNormalize As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me.ColumnNameConsideredAsY)

        ' Save here the data of which the average will be taken as Normalization reference.
        Dim DataToTakeAverageAsReference As New List(Of Double)
        Dim SmoothedValues As ReadOnlyCollection(Of Double) = Me._SmoothedColumn.Values

        ' Extract Data Values in the range selected as Limits for the X-Column:
        For i As Integer = 0 To XCol.Values.Count - 1 Step 1
            If XColValues(i) >= Me.LeftLimit And
               XColValues(i) <= Me.RightLimit And
               Not Double.IsNaN(XColValues(i)) And
               Not Double.IsNaN(SmoothedValues(i)) Then
                DataToTakeAverageAsReference.Add(SmoothedValues(i))
                'DataToTakeAverageAsReference.Add(Me._SmoothedColumn.Values(i))
            End If
        Next


        ' If no data-points were in the range,
        ' it is obviously too small. -> Select nearest point.
        If DataToTakeAverageAsReference.Count = 0 Then
            Dim CurrentDistanceToCoordinate As Double
            Dim NearestPointIndex As Integer = -1
            Dim MinimumDistance As Double = Double.MaxValue
            For i As Integer = 0 To XColValues.Count - 1 Step 1
                If Not Double.IsNaN(XColValues(i)) Then
                    CurrentDistanceToCoordinate = Math.Abs(XColValues(i) - Me.LeftLimit)
                    If CurrentDistanceToCoordinate < MinimumDistance Then
                        NearestPointIndex = i
                        MinimumDistance = CurrentDistanceToCoordinate
                    End If
                End If
            Next
            If NearestPointIndex >= 0 Then
                DataToTakeAverageAsReference.Add(SmoothedValues(NearestPointIndex))
            End If
        End If

        ' If there are still no reference-data, return.
        If DataToTakeAverageAsReference.Count = 0 Then Return

        ' Average selected Background-Data
        Dim ReferenceValue As Double = cNumericalMethods.AverageValues(DataToTakeAverageAsReference)

        ' Normalize the Data
        Me._NormalizedColumn = New cSpectroscopyTable.DataColumn

        ' Depending on selected derivation method, start Derivation
        Me._NormalizedColumn.SetValueList(cNumericalMethods.Normalize(ColToNormalize.Values, ReferenceValue))
        Me._NormalizedColumn.UnitSymbol = ColToNormalize.UnitSymbol
        Me._NormalizedColumn.Name = Me.NormalizedColumnTargetName

        RaiseEvent FileNormalizedComplete(Me.CurrentSpectroscopyTable, Me._NormalizedColumn)
    End Sub
#End Region

#Region "Save Normalized Column"
    ''' <summary>
    ''' Saves the Current Normalized Column to the File-Object
    ''' </summary>
    Public Sub SaveNormalizedColumnToFileObject(ByVal ColumnName As String)
        If Me._NormalizedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._NormalizedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._NormalizedColumn)
    End Sub
#End Region

End Class
