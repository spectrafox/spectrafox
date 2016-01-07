Imports System.Threading

Public Class cSpectroscopyTableDataSummer
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Object for storing the summed Column
    ''' </summary>
    Private _SummedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the current summed column
    ''' </summary>
    Public ReadOnly Property SummedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._SummedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._SummedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to sum the data in a separate thread.
    ''' </summary>
    Private FileCallback As New WaitCallback(AddressOf SpectroscopyFileSummer)

    ''' <summary>
    ''' Describes the summation mode.
    ''' E.g. use a constant factor or a column to sum with.
    ''' </summary>
    Public Enum SummationMode
        ByValue
        OtherColumnSum
        OtherColumnSubstract
    End Enum

    Private _ColumnNameToSum As String
    Private SummationMethod As SummationMode
    Private SummationFactor As Double
    Private _SummationColumnName As String

    Private ColumnTargetName As String
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the summation was successfull.
    ''' </summary>
    Public Event FileSummationComplete(ByRef SpectroscopyTable As cSpectroscopyTable,
                                       ByRef SummedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Summation Function"
    ''' <summary>
    ''' Starts the Summation for the selected file and the selected columns
    ''' with the selected method. Constant factor, or given by another column.
    ''' </summary>
    Public Sub SumColumnWithoutFetching_ASync(ByVal ColumnNameToSum As String,
                                              ByVal SummationFactor As Double,
                                              Optional ByVal ColumnTargetName As String = "Summation Result")
        Me._ColumnNameToSum = ColumnNameToSum
        Me.SummationMethod = SummationMode.ByValue
        Me.SummationFactor = SummationFactor
        Me.ColumnTargetName = ColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileCallback, Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Starts the summation for the selected file and the selected columns
    ''' with the selected method. Constant factor, or given by another column.
    ''' </summary>
    Public Sub SumColumnWithoutFetching_ASync(ByVal ColumnNameToSum As String,
                                              ByVal SummationMethod As SummationMode,
                                              ByVal SummationColumnName As String,
                                              Optional ByVal ColumnTargetName As String = "Summation Result")
        Me._ColumnNameToSum = ColumnNameToSum
        Me.SummationMethod = SummationMethod
        Me._SummationColumnName = SummationColumnName
        Me.ColumnTargetName = ColumnTargetName


        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private SummationWithFetchRunning As Boolean = False
    Public ReadOnly Property SummationProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.SummationWithFetchRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the summation procedure for the selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the summation.
    ''' </summary>
    Public Sub SumColumnWITHAutomaticFetching_ASync(ByVal ColumnNameToBeSummed As String,
                                                    ByVal SummationFactor As Double,
                                                    Optional ByVal ColumnTargetName As String = "Summation Result")
        Me._ColumnNameToSum = ColumnNameToBeSummed
        Me.SummationMethod = SummationMode.ByValue
        Me.SummationFactor = SummationFactor
        Me.ColumnTargetName = ColumnTargetName

        SummationWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Starts the summation procedure for the selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the summation.
    ''' </summary>
    Public Sub SumColumnWITHAutomaticFetching_ASync(ByVal ColumnNameToBeSummed As String,
                                                    ByVal SummationMode As SummationMode,
                                                    ByVal ColumnNameToSumWith As String,
                                                    Optional ByVal ColumnTargetName As String = "Summation Result")
        Me._ColumnNameToSum = ColumnNameToBeSummed
        Me.SummationMethod = SummationMode
        Me._SummationColumnName = ColumnNameToSumWith
        Me.ColumnTargetName = ColumnTargetName

        SummationWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the summation
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not SummationWithFetchRunning Then Return
        SummationWithFetchRunning = False

        ' Start the summation using the Fetched Data.
        If Me.SummationMethod = SummationMode.ByValue Then
            Me.SumColumnWithoutFetching_ASync(Me._ColumnNameToSum,
                                              Me.SummationFactor,
                                              Me.ColumnTargetName)
        Else
            Me.SumColumnWithoutFetching_ASync(Me._ColumnNameToSum,
                                              Me.SummationMethod,
                                              Me._SummationColumnName,
                                              Me.ColumnTargetName)
        End If

    End Sub

    ''' <summary>
    ''' Starts the summation procedure for the selected column taken fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' </summary>
    Public Sub SumColumnWITHAutomaticFetching_Direct(ByVal ColumnNameToBeSummed As String,
                                                     ByVal SummationFactor As Double,
                                                     Optional ByVal dColumnTargetName As String = "Summation Result")
        Me._SummationColumnName = ColumnNameToBeSummed
        Me.SummationMethod = SummationMode.ByValue
        Me.SummationFactor = SummationFactor
        Me.ColumnTargetName = dColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Sum up
        Me.SpectroscopyFileSummer(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Starts the summation procedure for the selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the summation.
    ''' </summary>
    Public Sub SumColumnWITHAutomaticFetching_Direct(ByVal ColumnNameToBeSummed As String,
                                                     ByVal SumMode As SummationMode,
                                                     ByVal ColumnNameToSumWith As String,
                                                     Optional ByVal ColumnTargetName As String = "Summation Result")
        Me._ColumnNameToSum = ColumnNameToBeSummed
        Me._SummationColumnName = ColumnNameToSumWith
        Me.SummationMethod = SumMode
        Me.ColumnTargetName = ColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Just do it
        Me.SpectroscopyFileSummer(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File to sum the data in a separate thread.
    ''' </summary>
    Private Sub SpectroscopyFileSummer(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me._ColumnNameToSum) Then
            MessageBox.Show("Column for summation not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Me.SummationMethod <> SummationMode.ByValue Then

            If SpectroscopyTable.ColumnExists(Me._SummationColumnName) Then
                MessageBox.Show("Reference column for summation not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If SpectroscopyTable.Column(Me._SummationColumnName).Values.Count <> SpectroscopyTable.Column(Me._ColumnNameToSum).Values.Count Then
                MessageBox.Show("reference column and column to be summed are not of same length",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        '################################
        ' Start summation Procedure.

        Me._SummedColumn = New cSpectroscopyTable.DataColumn
        Dim SourceColumn As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me._ColumnNameToSum)
        Dim SourceColumnValues As ReadOnlyCollection(Of Double) = SourceColumn.Values

        ' Depending on selected summation method, start summation
        Select Case Me.SummationMethod
            Case SummationMode.ByValue
                ' Sum by fixed factor
                For i As Integer = 0 To SourceColumnValues.Count - 1 Step 1
                    If Double.IsNaN(SourceColumnValues(i)) Or Double.IsInfinity(SourceColumnValues(i)) Then
                        Me._SummedColumn.AddValueToColumn(SourceColumnValues(i))
                    Else
                        Me._SummedColumn.AddValueToColumn(SourceColumnValues(i) + Me.SummationFactor)
                    End If
                Next
                Me._SummedColumn.UnitSymbol = SourceColumn.UnitSymbol
            Case SummationMode.OtherColumnSum
                Dim OtherColumnValues As ReadOnlyCollection(Of Double) = SpectroscopyTable.Column(Me._SummationColumnName).Values

                For i As Integer = 0 To SourceColumnValues.Count - 1 Step 1
                    If Double.IsNaN(SourceColumnValues(i)) Or Double.IsInfinity(SourceColumnValues(i)) Then
                        Me._SummedColumn.AddValueToColumn(SourceColumnValues(i))
                    ElseIf Double.IsNaN(OtherColumnValues(i)) Or Double.IsInfinity(OtherColumnValues(i)) Then
                        Me._SummedColumn.AddValueToColumn(Double.NaN)
                    Else
                        Me._SummedColumn.AddValueToColumn(SourceColumnValues(i) + OtherColumnValues(i))
                    End If
                Next
                If SourceColumn.UnitSymbol <> SpectroscopyTable.Column(Me._SummationColumnName).UnitSymbol Then
                    Me._SummedColumn.UnitSymbol = SourceColumn.UnitSymbol & "+" & SpectroscopyTable.Column(Me._SummationColumnName).UnitSymbol
                Else
                    Me._SummedColumn.UnitSymbol = SourceColumn.UnitSymbol
                End If
            Case SummationMode.OtherColumnSubstract
                Dim OtherColumnValues As ReadOnlyCollection(Of Double) = SpectroscopyTable.Column(Me._SummationColumnName).Values

                For i As Integer = 0 To SourceColumnValues.Count - 1 Step 1
                    If Double.IsNaN(SourceColumnValues(i)) Or Double.IsInfinity(SourceColumnValues(i)) Then
                        Me._SummedColumn.AddValueToColumn(SourceColumnValues(i))
                    ElseIf Double.IsNaN(OtherColumnValues(i)) Or Double.IsInfinity(OtherColumnValues(i)) Then
                        Me._SummedColumn.AddValueToColumn(Double.NaN)
                    Else
                        Me._SummedColumn.AddValueToColumn(SourceColumnValues(i) - OtherColumnValues(i))
                    End If
                Next
                If SourceColumn.UnitSymbol <> SpectroscopyTable.Column(Me._SummationColumnName).UnitSymbol Then
                    Me._SummedColumn.UnitSymbol = SourceColumn.UnitSymbol & "+" & SpectroscopyTable.Column(Me._SummationColumnName).UnitSymbol
                Else
                    Me._SummedColumn.UnitSymbol = SourceColumn.UnitSymbol
                End If
        End Select
        Me._SummedColumn.Name = Me.ColumnTargetName

        RaiseEvent FileSummationComplete(Me.CurrentSpectroscopyTable, Me._SummedColumn)
    End Sub
#End Region

#Region "Save newly generated column"

    ''' <summary>
    ''' Saves the current summed column to the file-object
    ''' </summary>
    Public Sub SaveSummedColumnToFileObject(ByVal ColumnName As String)
        If Me._SummedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._SummedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._SummedColumn)
    End Sub

#End Region

End Class
