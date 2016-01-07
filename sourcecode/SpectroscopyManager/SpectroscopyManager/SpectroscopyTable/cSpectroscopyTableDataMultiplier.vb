Imports System.Threading

Public Class cSpectroscopyTableDataMultiplier
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Object for storing the Multiplied Column
    ''' </summary>
    Private _MultipliedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the current multiplied column
    ''' </summary>
    Public ReadOnly Property MultipliedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._MultipliedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._MultipliedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to multiply the data in a separate thread.
    ''' </summary>
    Private FileCallback As New WaitCallback(AddressOf SpectroscopyFileMultiplier)

    ''' <summary>
    ''' Describes the multiplication mode.
    ''' E.g. use a constant factor or a column to devide by.
    ''' </summary>
    Public Enum MultiplicationMode
        Factor
        OtherColumnMultiply
        OtherColumnDivide
    End Enum

    Private MultiplicationMethod As MultiplicationMode
    Private MultiplicationFactor As Double
    Private MultiplicationColumnName As String = ""
    Private ColumnNameToBeMultipliedWith As String = ""

    Private ColumnTargetName As String
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the multiplication was successfull.
    ''' </summary>
    Public Event FileMultiplicationComplete(ByRef SpectroscopyTable As cSpectroscopyTable,
                                            ByRef MultipliedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Multiplication Function"
    ''' <summary>
    ''' Starts the multiplication for the selected file and the selected columns
    ''' with the selected method. Constant factor, or given by another column.
    ''' </summary>
    Public Sub MultiplyColumnWithoutFetching_ASync(ByVal ColumnNameToBeMultiplied As String,
                                                   ByVal MultiplicationFactor As Double,
                                                   Optional ByVal MultipliedColumnTargetName As String = "Multiplied Result")
        Me.MultiplicationColumnName = ColumnNameToBeMultiplied
        Me.MultiplicationMethod = MultiplicationMode.Factor
        Me.MultiplicationFactor = MultiplicationFactor
        Me.ColumnTargetName = MultipliedColumnTargetName


        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileCallback, Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Starts the multiplication for the selected file and the selected columns
    ''' with the selected method. Constant factor, or given by another column.
    ''' </summary>
    Public Sub MultiplyColumnWithoutFetching_ASync(ByVal ColumnNameToBeMultiplied As String,
                                                   ByVal MultMode As MultiplicationMode,
                                                   ByVal MultiplicationColumnName As String,
                                                   Optional ByVal MultipliedColumnTargetName As String = "Multiplied Result")
        Me.MultiplicationColumnName = ColumnNameToBeMultiplied
        Me.MultiplicationMethod = MultMode
        Me.ColumnNameToBeMultipliedWith = MultiplicationColumnName
        Me.ColumnTargetName = MultipliedColumnTargetName


        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private MultiplicationWithFetchRunning As Boolean = False
    Public ReadOnly Property MultiplicationProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.MultiplicationWithFetchRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the multiplication procedure for the selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the multiplication.
    ''' </summary>
    Public Sub MultiplyColumnWITHAutomaticFetching_ASync(ByVal ColumnNameToBeMultiplied As String,
                                                         ByVal MultiplicationFactor As Double,
                                                         Optional ByVal MultipliedColumnTargetName As String = "Multiplied Result")
        Me.MultiplicationColumnName = ColumnNameToBeMultiplied
        Me.MultiplicationMethod = MultiplicationMode.Factor
        Me.MultiplicationFactor = MultiplicationFactor
        Me.ColumnTargetName = MultipliedColumnTargetName

        MultiplicationWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Starts the multiplication procedure for the selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the multiplication.
    ''' </summary>
    Public Sub MultiplyColumnWITHAutomaticFetching_ASync(ByVal ColumnNameToBeMultiplied As String,
                                                         ByVal MultMode As MultiplicationMode,
                                                         ByVal ColumnNameToMultiplyWith As String,
                                                         Optional ByVal MultipliedColumnTargetName As String = "Multiplied Result")
        Me.MultiplicationColumnName = ColumnNameToBeMultiplied
        Me.MultiplicationMethod = MultMode
        Me.ColumnNameToBeMultipliedWith = ColumnNameToMultiplyWith
        Me.ColumnTargetName = MultipliedColumnTargetName

        MultiplicationWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the multiplication
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not MultiplicationWithFetchRunning Then Return
        MultiplicationWithFetchRunning = False

        ' Start the multiplication using the Fetched Data.
        If Me.MultiplicationMethod = MultiplicationMode.Factor Then
            Me.MultiplyColumnWithoutFetching_ASync(Me.MultiplicationColumnName,
                                                   Me.MultiplicationFactor,
                                                   Me.ColumnTargetName)
        Else
            Me.MultiplyColumnWithoutFetching_ASync(Me.MultiplicationColumnName,
                                                   Me.MultiplicationMethod,
                                                   Me.ColumnNameToBeMultipliedWith,
                                                   Me.ColumnTargetName)
        End If

    End Sub

    ''' <summary>
    ''' Starts the multiplication procedure for the selected column taken fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' </summary>
    Public Sub MultiplyColumnWITHAutomaticFetching_Direct(ByVal ColumnNameToBeMultiplied As String,
                                                          ByVal MultiplicationFactor As Double,
                                                          Optional ByVal MultipliedColumnTargetName As String = "Multiplied Result")
        Me.MultiplicationColumnName = ColumnNameToBeMultiplied
        Me.MultiplicationMethod = MultiplicationMode.Factor
        Me.MultiplicationFactor = MultiplicationFactor
        Me.ColumnTargetName = MultipliedColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Multiply
        Me.SpectroscopyFileMultiplier(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Starts the multiplication procedure for the selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the multiplication.
    ''' </summary>
    Public Sub MultiplyColumnWITHAutomaticFetching_Direct(ByVal ColumnNameToBeMultiplied As String,
                                                          ByVal MultMode As MultiplicationMode,
                                                          ByVal ColumnNameToMultiplyWith As String,
                                                          Optional ByVal MultipliedColumnTargetName As String = "Multiplied Result")
        Me.MultiplicationColumnName = ColumnNameToBeMultiplied
        Me.MultiplicationMethod = MultMode
        Me.ColumnNameToBeMultipliedWith = ColumnNameToMultiplyWith
        Me.ColumnTargetName = MultipliedColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Multiply
        Me.SpectroscopyFileMultiplier(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File multiplied to multiply the data in a separate thread.
    ''' </summary>
    Private Sub SpectroscopyFileMultiplier(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me.MultiplicationColumnName) Then
            MessageBox.Show("Column for multiplication not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Me.MultiplicationMethod <> MultiplicationMode.Factor Then
            If Not SpectroscopyTable.ColumnExists(Me.ColumnNameToBeMultipliedWith) Then
                MessageBox.Show("Reference column for multiplication not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If SpectroscopyTable.Column(Me.MultiplicationColumnName).Values.Count <> SpectroscopyTable.Column(Me.ColumnNameToBeMultipliedWith).Values.Count Then
                MessageBox.Show("reference column and column to be multiplied are not of same length", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        End If

        '################################
        ' Start Multiplication Procedure.

        Me._MultipliedColumn = New cSpectroscopyTable.DataColumn
        Dim SourceColumn As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me.MultiplicationColumnName)
        Dim SourceColumnValues As ReadOnlyCollection(Of Double) = SourceColumn.Values

        ' Depending on selected multiplication method, start multiplication
        Select Case Me.MultiplicationMethod
            Case MultiplicationMode.Factor
                ' Multiply by fixed factor
                For i As Integer = 0 To SourceColumnValues.Count - 1 Step 1
                    If Double.IsNaN(SourceColumnValues(i)) Or Double.IsInfinity(SourceColumnValues(i)) Then
                        Me._MultipliedColumn.AddValueToColumn(SourceColumnValues(i))
                    Else
                        Me._MultipliedColumn.AddValueToColumn(SourceColumnValues(i) * Me.MultiplicationFactor)
                    End If
                Next
                Me._MultipliedColumn.UnitSymbol = SourceColumn.UnitSymbol
            Case MultiplicationMode.OtherColumnMultiply
                Dim OtherColumnValues As ReadOnlyCollection(Of Double) = SpectroscopyTable.Column(Me.ColumnNameToBeMultipliedWith).Values

                For i As Integer = 0 To SourceColumnValues.Count - 1 Step 1
                    If Double.IsNaN(SourceColumnValues(i)) Or Double.IsInfinity(SourceColumnValues(i)) Then
                        Me._MultipliedColumn.AddValueToColumn(SourceColumnValues(i))
                    ElseIf Double.IsNaN(OtherColumnValues(i)) Or Double.IsInfinity(OtherColumnValues(i)) Then
                        Me._MultipliedColumn.AddValueToColumn(Double.NaN)
                    Else
                        Me._MultipliedColumn.AddValueToColumn(SourceColumnValues(i) * OtherColumnValues(i))
                    End If
                Next
                Me._MultipliedColumn.UnitSymbol = SourceColumn.UnitSymbol & SpectroscopyTable.Column(Me.ColumnNameToBeMultipliedWith).UnitSymbol
            Case MultiplicationMode.OtherColumnDivide
                Dim OtherColumnValues As ReadOnlyCollection(Of Double) = SpectroscopyTable.Column(Me.ColumnNameToBeMultipliedWith).Values

                For i As Integer = 0 To SourceColumnValues.Count - 1 Step 1
                    If Double.IsNaN(SourceColumnValues(i)) Or Double.IsInfinity(SourceColumnValues(i)) Then
                        Me._MultipliedColumn.AddValueToColumn(SourceColumnValues(i))
                    ElseIf Double.IsNaN(OtherColumnValues(i)) Or Double.IsInfinity(OtherColumnValues(i)) Then
                        Me._MultipliedColumn.AddValueToColumn(Double.NaN)
                    Else
                        Me._MultipliedColumn.AddValueToColumn(SourceColumnValues(i) / OtherColumnValues(i))
                    End If
                Next
                Me._MultipliedColumn.UnitSymbol = SourceColumn.UnitSymbol & "/" & SpectroscopyTable.Column(Me.ColumnNameToBeMultipliedWith).UnitSymbol
        End Select
        Me._MultipliedColumn.Name = Me.ColumnTargetName

        RaiseEvent FileMultiplicationComplete(Me.CurrentSpectroscopyTable, Me._MultipliedColumn)
    End Sub
#End Region

#Region "Save Multiplied Column"

    ''' <summary>
    ''' Saves the current multiplied column to the file-object
    ''' </summary>
    Public Sub SaveMultipliedColumnToFileObject(ByVal ColumnName As String)
        If Me._MultipliedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._MultipliedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._MultipliedColumn)
    End Sub

#End Region

End Class
