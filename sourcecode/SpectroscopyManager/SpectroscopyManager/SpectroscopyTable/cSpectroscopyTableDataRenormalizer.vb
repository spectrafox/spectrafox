Imports System.Threading

Public Class cSpectroscopyTableDataRenormalizer
    Inherits cSpectroscopyTableDataDeriver

#Region "Properties"

    Protected _Parameter_m As Double = 0.0
    ''' <summary>
    ''' Fitted slope of the renormalization.
    ''' </summary>
    Public ReadOnly Property Parameter_m As Double
        Get
            Return Me._Parameter_m
        End Get
    End Property

    Protected _Parameter_y0 As Double = 0.0
    ''' <summary>
    ''' Fitted y-offset of the renormalization.
    ''' </summary>
    Public ReadOnly Property Parameter_y0 As Double
        Get
            Return Me._Parameter_y0
        End Get
    End Property

    Protected _Boundary_Xmin As Double = 0.0
    ''' <summary>
    ''' This gives the index of the range which should be treated.
    ''' (lowest range of values)
    ''' </summary>
    Public ReadOnly Property Boundary_Xmin As Double
        Get
            Return Me._Boundary_Xmin
        End Get
    End Property

    Protected _Boundary_Xmax As Double = 0.0
    ''' <summary>
    ''' This gives the index of the range which should be treated.
    ''' (largest range of values)
    ''' </summary>
    Public ReadOnly Property Boundary_Xmax As Double
        Get
            Return Me._Boundary_Xmax
        End Get
    End Property

    Protected _UseBoundaries As Boolean = False
    ''' <summary>
    ''' This determines, if the boundaries should be used.
    ''' </summary>
    Public ReadOnly Property UseBoundaries As Boolean
        Get
            Return Me._UseBoundaries
        End Get
    End Property

    ''' <summary>
    ''' Object for storing the renormalized Column
    ''' </summary>
    Private _RenormalizedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the Current Renormalized Column
    ''' </summary>
    Public ReadOnly Property RenormalizedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._RenormalizedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._RenormalizedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to regauge the data in a separate thread.
    ''' </summary>
    Private FileRenormalizerCallback As New WaitCallback(AddressOf SpectroscopyFileRenormalizer)

    Private RenormalizedColumnTargetName As String
    Private RenormalizationColumnNameConsideredAsX As String = ""
    Private RenormalizationColumnNameConsideredAsRenormalizationTarget As String = ""

#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the Renormalization was successfull.
    ''' </summary>
    Public Event FileRenormalizedComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RenormalizedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Renormalization Function"
    ''' <summary>
    ''' Starts the Renormalization for selected Column taken as X and the Derivated Column.
    ''' Expects a present Derivated Column.
    ''' </summary>
    Public Sub RenormalizeColumnWithoutDerivation_Async(ByVal RenormalizationColumnNameConsideredAsX As String,
                                                        ByVal RenormalizationColumnNameConsideredAsrenormalizationTarget As String,
                                                        Optional ByVal RenormalizedColumnTargetName As String = "regauged result")
        Me.RenormalizationColumnNameConsideredAsX = RenormalizationColumnNameConsideredAsX
        Me.RenormalizationColumnNameConsideredAsRenormalizationTarget = RenormalizationColumnNameConsideredAsrenormalizationTarget
        Me.RenormalizedColumnTargetName = RenormalizedColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileRenormalizerCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private RenormalizationWithSmoothingAndDerivationRunning As Boolean = False
    Public ReadOnly Property RenormalizationProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.RenormalizationWithSmoothingAndDerivationRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the Renormalization for selected Column taken as X and a certain Index for the
    ''' Column to derivate and smooth before. This function treats all events of the
    ''' multi-threaded-Derivation and -Smoothing procedure, before initializing the Renormalization.
    ''' </summary>
    Public Sub RenormalizeColumnWITHDerivation_Async(ByVal DerivationColumnNameConsideredAsX As String,
                                                     ByVal DerivationColumnNameConsideredAsTreatmentValues As String,
                                                     ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                                     ByVal SmoothParameter As Integer,
                                                     ByVal RenormalizationColumnNameConsideredAsX As String,
                                                     ByVal RenormalizationColumnNameConsideredAsRenormalizationTarget As String,
                                                     ByVal UseBoundaries As Boolean,
                                                     ByVal Xmin As Double,
                                                     ByVal Xmax As Double,
                                                     Optional ByVal ColumnTargetName As String = "regauged result")
        Me.RenormalizationColumnNameConsideredAsX = RenormalizationColumnNameConsideredAsX
        Me.RenormalizationColumnNameConsideredAsRenormalizationTarget = RenormalizationColumnNameConsideredAsRenormalizationTarget

        Me._Boundary_Xmin = Xmin
        Me._Boundary_Xmax = Xmax
        Me._UseBoundaries = UseBoundaries

        Me.RenormalizedColumnTargetName = ColumnTargetName

        RenormalizationWithSmoothingAndDerivationRunning = True

        ' Send SpectroscopyTable to ThreadPoolQueue of the DataSmoother
        Me.DerivateColumnWITHSmoothing_Async(DerivationColumnNameConsideredAsX,
                                             DerivationColumnNameConsideredAsTreatmentValues,
                                             SmoothProcedure,
                                             SmoothParameter)
    End Sub

    ''' <summary>
    ''' Function that catches the Derivation-Complete-Event and starts the Renormalization
    ''' </summary>
    Private Sub DerivationCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable,
                                          ByRef SmoothedColumn As cSpectroscopyTable.DataColumn) Handles MyBase.FileDerivatedComplete
        If Not RenormalizationWithSmoothingAndDerivationRunning Then Return
        RenormalizationWithSmoothingAndDerivationRunning = False

        ' Start the Renormalization using the Smoothed and derivated Data.
        Me.RenormalizeColumnWithoutDerivation_Async(Me.RenormalizationColumnNameConsideredAsX,
                                                    Me.RenormalizationColumnNameConsideredAsRenormalizationTarget,
                                                    Me.RenormalizedColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the Renormalization for selected Column taken as X and a certain Index for the
    ''' Column to derivate and smooth before. 
    ''' </summary>
    Public Sub RenormalizeColumnWITHDerivation_Direct(ByVal DerivationColumnNameConsideredAsX As String,
                                                      ByVal DerivationColumnNameConsideredAsTreatmentValues As String,
                                                      ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                                      ByVal SmoothParameter As Integer,
                                                      ByVal RenormalizationColumnNameConsideredAsX As String,
                                                      ByVal RenormalizationColumnNameConsideredAsRenormalizationTarget As String,
                                                      ByVal UseBoundaries As Boolean,
                                                      ByVal Xmin As Double,
                                                      ByVal Xmax As Double,
                                                      Optional ByVal ColumnTargetName As String = "regauged result")
        Me.RenormalizationColumnNameConsideredAsX = RenormalizationColumnNameConsideredAsX
        Me.RenormalizationColumnNameConsideredAsRenormalizationTarget = RenormalizationColumnNameConsideredAsRenormalizationTarget

        Me._Boundary_Xmin = Xmin
        Me._Boundary_Xmax = Xmax
        Me._UseBoundaries = UseBoundaries

        Me.RenormalizedColumnTargetName = ColumnTargetName

        ' Fetch, Smooth and Derive column directly.
        Me.DerivateColumnWITHSmoothing_Direct(DerivationColumnNameConsideredAsX,
                                              DerivationColumnNameConsideredAsTreatmentValues,
                                              SmoothProcedure,
                                              SmoothParameter)

        ' Renormalize
        Me.SpectroscopyFileRenormalizer(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' SpectroscopyFileRenormalizer to regauge the data in a separate thread.
    ''' Calculates the derivative of a source column, and fits a target column to this derivative.
    ''' </summary>
    Private Sub SpectroscopyFileRenormalizer(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        Try
            If Not SpectroscopyTable.ColumnExists(Me.RenormalizationColumnNameConsideredAsX) Then
                Throw New ArgumentException("X-axis column for re-gauging procedure not found")
            End If

            If Not SpectroscopyTable.ColumnExists(Me.RenormalizationColumnNameConsideredAsRenormalizationTarget) Then
                Throw New ArgumentException("Y-axis column for re-gauging procedure not found")
            End If

            ' Get the boundaries, if activated
            Dim XMin As Integer = 0
            Dim XMax As Integer = 0

            If Me.UseBoundaries Then
                Dim XDataColumn As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me.RenormalizationColumnNameConsideredAsX)
                If XDataColumn.Monotonicity = cSpectroscopyTable.DataColumn.Monotonicities.Rising Or XDataColumn.Monotonicity = cSpectroscopyTable.DataColumn.Monotonicities.StrictRising Then
                    XMin = XDataColumn.GetFirstIndexAfterWhichValuesAreLargerThan(Me.Boundary_Xmin)
                    XMax = XDataColumn.GetFirstIndexAfterWhichValuesAreLargerThan(Me.Boundary_Xmax, XMin)
                ElseIf XDataColumn.Monotonicity = cSpectroscopyTable.DataColumn.Monotonicities.Falling Or XDataColumn.Monotonicity = cSpectroscopyTable.DataColumn.Monotonicities.StrictFalling Then
                    XMax = XDataColumn.GetFirstIndexAfterWhichValuesAreSmallerThan(Me.Boundary_Xmax)
                    XMin = XDataColumn.GetFirstIndexAfterWhichValuesAreSmallerThan(Me.Boundary_Xmin, XMax)
                Else
                    Throw New ArgumentException("monotonicity check of the data failed: when using value boundaries, the data must be monotonic")
                End If
                If XMax < 0 Then XMax = XDataColumn.Values.Count - 1
                If XMin < 0 Then XMin = 0
            End If

            '################################
            ' Start Renormalization Procedure.

            Me._RenormalizedColumn = New cSpectroscopyTable.DataColumn

            ' Renormalize
            Me._RenormalizedColumn.SetValueList(cNumericalMethods.RenormalizeByDerivativeFitting(SpectroscopyTable.Column(Me.RenormalizationColumnNameConsideredAsX).Values,
                                                                                                 Me.DerivatedColumn.Values,
                                                                                                 SpectroscopyTable.Column(Me.RenormalizationColumnNameConsideredAsX).Values,
                                                                                                 SpectroscopyTable.Column(Me.RenormalizationColumnNameConsideredAsRenormalizationTarget).Values,
                                                                                                 Me._Parameter_y0,
                                                                                                 Me._Parameter_m,
                                                                                                 Me.UseBoundaries,
                                                                                                 XMin,
                                                                                                 XMax))
            Me._RenormalizedColumn.UnitSymbol = DerivatedColumn.UnitSymbol
            Me._RenormalizedColumn.Name = Me.RenormalizedColumnTargetName

            RaiseEvent FileRenormalizedComplete(Me.CurrentSpectroscopyTable, Me._RenormalizedColumn)
        Catch ex As Exception
            MessageBox.Show(ex.Message, My.Resources.title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region

#Region "Save Renormalized Column"
    ''' <summary>
    ''' Saves the Current Renormalized Column to the File-Object
    ''' </summary>
    Public Sub SaveRenormalizedColumnToFileObject(ByVal ColumnName As String)
        If Me._RenormalizedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._RenormalizedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._RenormalizedColumn)
    End Sub
#End Region

End Class
