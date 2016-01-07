Imports System.Threading

Public Class cSpectroscopyTableDataAverager
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Object for storing the averaged output column
    ''' </summary>
    Private _AveragedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the current averaged output column
    ''' </summary>
    Public ReadOnly Property AveragedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._AveragedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._AveragedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to average the data in a separate thread.
    ''' </summary>
    Private _FileCallback As New WaitCallback(AddressOf SpectroscopyFileAverager)

    Private _ColumnNamesToBeAveraged As New List(Of String)
    Private _ColumnTargetName As String
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the averaging was successfull.
    ''' </summary>
    Public Event FileAveragingComplete(ByRef SpectroscopyTable As cSpectroscopyTable,
                                       ByRef AveragedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Averaging Function"
    ''' <summary>
    ''' Starts the averaging for the selected file and the selected columns.
    ''' </summary>
    Public Sub AverageColumnsWithoutFetching_ASync(ByVal ColumnNamesToBeAveraged As List(Of String),
                                                   Optional ByVal ColumnTargetName As String = "averaged data")
        Me._ColumnNamesToBeAveraged = ColumnNamesToBeAveraged
        Me._ColumnTargetName = ColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(_FileCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private AveragingWithFetchRunning As Boolean = False
    Public ReadOnly Property AverageProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.AveragingWithFetchRunning
        End Get
    End Property

    ''' <summary>
    ''' Starts the averaging procedure for the selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the averaging.
    ''' </summary>
    Public Sub AverageColumnsWITHAutomaticFetching_ASync(ByVal ColumnNamesToBeAveraged As List(Of String),
                                                         Optional ByVal ColumnTargetName As String = "averaged data")
        Me._ColumnNamesToBeAveraged = ColumnNamesToBeAveraged
        Me._ColumnTargetName = ColumnTargetName

        AveragingWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the averaging
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not AveragingWithFetchRunning Then Return
        AveragingWithFetchRunning = False

        Me.AverageColumnsWithoutFetching_ASync(Me._ColumnNamesToBeAveraged, Me._ColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the averaging procedure for the selected column taken fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' </summary>
    Public Sub AverageColumnsWITHAutomaticFetching_Direct(ByVal ColumnNamesToBeAveraged As List(Of String),
                                                          Optional ByVal ColumnTargetName As String = "averaged data")
        Me._ColumnNamesToBeAveraged = ColumnNamesToBeAveraged
        Me._ColumnTargetName = ColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Sum up
        Me.SpectroscopyFileAverager(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Averages the selected columns in a separate thread, if necessary.
    ''' </summary>
    Private Sub SpectroscopyFileAverager(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Me._ColumnNamesToBeAveraged.Count <= 0 Then
            MessageBox.Show("columns for averaging not found (count 0)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '################################
        ' Start averaging Procedure.

        Me._AveragedColumn = New cSpectroscopyTable.DataColumn

        ' Extract the relevant source columns
        Dim SourceColumns As New List(Of cSpectroscopyTable.DataColumn)

        For i As Integer = 0 To Me._ColumnNamesToBeAveraged.Count - 1 Step 1
            SourceColumns.Add(SpectroscopyTable.Column(Me._ColumnNamesToBeAveraged(i)))
        Next

        ' Average
        Me._AveragedColumn.SetValueList(cNumericalMethods.AverageColumns(SourceColumns))

        ' Set the new name and unit
        Me._AveragedColumn.UnitSymbol = SourceColumns.First.UnitSymbol
        Me._AveragedColumn.UnitType = SourceColumns.First.UnitType
        Me._AveragedColumn.Name = Me._ColumnTargetName

        RaiseEvent FileAveragingComplete(Me.CurrentSpectroscopyTable, Me._AveragedColumn)
    End Sub
#End Region

#Region "Save Averaged Column"

    ''' <summary>
    ''' Saves the current summed column to the file-object
    ''' </summary>
    Public Sub SaveAveragedColumnToFileObject(ByVal ColumnName As String)
        If Me._AveragedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._AveragedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._AveragedColumn)
    End Sub

#End Region

End Class
