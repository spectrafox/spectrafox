Imports System.Threading

Public Class cSpectroscopyTableDataDeriver
    Inherits cSpectroscopyTableDataSmoother

#Region "Properties"
    ''' <summary>
    ''' Object for storing the derivated Column
    ''' </summary>
    Private _DerivatedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the Current Derivated Column
    ''' </summary>
    Public ReadOnly Property DerivatedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._DerivatedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._DerivatedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to Derivate the Data in a separate Thread.
    ''' </summary>
    Private FileDerivatedCallback As New WaitCallback(AddressOf SpectroscopyFileDerivator)

    Private _SourceData As cSpectroscopyTable.DataColumn
    Private DerivatedColumnTargetName As String

    Private DerivationColumnNameConsideredAsX As String = ""
    Private _DerivationColumnName As String = ""

    ''' <summary>
    ''' Counter to count the performed derivations.
    ''' </summary>
    Private iHigherOrderDerivativeCounter As Integer
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the derivation was successfull.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event FileDerivatedComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef DerivatedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Derivation Function"
    ''' <summary>
    ''' Starts the Derivation for selected Column taken as X and the Smoothed Column.
    ''' This function does not perform a smoothing on the data in advance. It assumes, that the
    ''' Column in SmoothedColumn is already treated.
    ''' </summary>
    Public Sub DerivateColumnWithoutSmoothing_Async(ByVal DerivationColumnNameConsideredAsX As String,
                                                    ByVal SourceData As cSpectroscopyTable.DataColumn,
                                                    Optional ByVal DerivatedColumnTargetName As String = "Derivation Result")
        Me.DerivationColumnNameConsideredAsX = DerivationColumnNameConsideredAsX
        Me.DerivatedColumnTargetName = DerivatedColumnTargetName
        Me._SourceData = SourceData

        ' Send SpectroscopyTable to ThreadPoolQueue of the DataDerivator
        ThreadPool.QueueUserWorkItem(FileDerivatedCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private DerivationWithSmoothingRunning As Boolean = False
    Public ReadOnly Property DerivationProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.DerivationWithSmoothingRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the Derivation for selected Column taken as X and a certain Index for the
    ''' Column to smooth before. This function treats all events of the multi-threaded-Smoothing
    ''' procedure, before initializing the derivation.
    ''' </summary>
    Public Sub DerivateColumnWITHSmoothing_Async(ByVal DerivationColumnNameConsideredAsX As String,
                                               ByVal ColumnNameConsideredAsTreatmentValues As String,
                                               ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                               ByVal SmoothParameter As Integer,
                                               Optional ByVal DerivatedColumnTargetName As String = "Derivative Result",
                                               Optional ByVal DerivativeOrder As Integer = 1)
        Me.DerivationColumnNameConsideredAsX = DerivationColumnNameConsideredAsX
        Me._DerivationColumnName = ColumnNameConsideredAsTreatmentValues
        Me.DerivatedColumnTargetName = DerivatedColumnTargetName
        Me.iHigherOrderDerivativeCounter = DerivativeOrder

        DerivationWithSmoothingRunning = True

        ' Send SpectroscopyTable to ThreadPoolQueue of the DataSmoother
        Me.SmoothColumnWITHAutomaticFetching_Async(ColumnNameConsideredAsTreatmentValues,
                                             SmoothProcedure,
                                             SmoothParameter)
    End Sub

    ''' <summary>
    ''' Function that catches the Smoothing-Complete-Event and starts the Derivation
    ''' </summary>
    Private Sub SmoothingCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable,
                                         ByRef SmoothedColumn As cSpectroscopyTable.DataColumn) Handles MyBase.FileSmoothingComplete
        If Not DerivationWithSmoothingRunning Then Return
        DerivationWithSmoothingRunning = False

        ' Start the Derivation using the Smoothed Data.
        Me.DerivateColumnWithoutSmoothing_Async(Me.DerivationColumnNameConsideredAsX,
                                                Me.SmoothedColumn,
                                                Me.DerivatedColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the Derivation for selected Column taken as X and a certain Index for the
    ''' Column to smooth before.
    ''' </summary>
    Public Sub DerivateColumnWITHSmoothing_Direct(ByVal DerivationColumnNameConsideredAsX As String,
                                                  ByVal ColumnNameConsideredAsTreatmentValues As String,
                                                  ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                                  ByVal SmoothParameter As Integer,
                                                  Optional ByVal DerivatedColumnTargetName As String = "Derivative Result",
                                                  Optional ByVal DerivativeOrder As Integer = 1)
        Me.DerivationColumnNameConsideredAsX = DerivationColumnNameConsideredAsX
        Me._DerivationColumnName = ColumnNameConsideredAsTreatmentValues
        Me.DerivatedColumnTargetName = DerivatedColumnTargetName
        Me.iHigherOrderDerivativeCounter = DerivativeOrder

        ' Fetch and Smooth the file in advance
        Me.SmoothColumnWITHFetching_Direct(ColumnNameConsideredAsTreatmentValues,
                                           SmoothProcedure,
                                           SmoothParameter)

        Me._SourceData = Me.SmoothedColumn

        ' Calculate the numeric derivative
        Me.SpectroscopyFileDerivator(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Derivator to Derivate the data in a separate Thread.
    ''' </summary>
    Private Sub SpectroscopyFileDerivator(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me.DerivationColumnNameConsideredAsX) Then
            MessageBox.Show("X-Axis Column for the Derivation not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '################################
        ' Start Derivation Procedure.

        Me._DerivatedColumn = New cSpectroscopyTable.DataColumn
        Me._DerivatedColumn.SetValueList(Me._SourceData.Values.ToList)
        Me._DerivatedColumn.UnitSymbol = Me._SourceData.UnitSymbol
        ' Depending on selected Derivation Method, start Derivation
        For i As Integer = 1 To Me.iHigherOrderDerivativeCounter Step 1
            Me._DerivatedColumn.SetValueList(cNumericalMethods.NumericalDerivative(SpectroscopyTable.Column(Me.DerivationColumnNameConsideredAsX),
                                                                                   Me._DerivatedColumn))
            Me._DerivatedColumn.UnitSymbol &= "/" & SpectroscopyTable.Column(DerivationColumnNameConsideredAsX).UnitSymbol
        Next
        Me._DerivatedColumn.Name = Me.DerivatedColumnTargetName

        RaiseEvent FileDerivatedComplete(Me.CurrentSpectroscopyTable, Me._DerivatedColumn)
    End Sub
#End Region

#Region "Save Derived Column"
    ''' <summary>
    ''' Saves the Current Derivated Column to the File-Object
    ''' </summary>
    Public Sub SaveDerivatedColumnToFileObject(ByVal ColumnName As String)
        If Me._DerivatedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._DerivatedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._DerivatedColumn)
    End Sub
#End Region

End Class
