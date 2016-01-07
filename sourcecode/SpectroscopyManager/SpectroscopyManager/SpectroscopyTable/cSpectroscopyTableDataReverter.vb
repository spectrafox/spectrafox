Imports System.Threading

''' <summary>
''' Class for Smoothing of a Data
''' </summary>
Public Class cSpectroscopyTableDataReverter
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Object for storing the reversed column
    ''' </summary>
    Protected _ReversedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the current reversed column
    ''' </summary>
    Public ReadOnly Property ReversedColumn As cSpectroscopyTable.DataColumn
        Get
            Return Me._ReversedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to invert the data in a separate thread.
    ''' </summary>
    Private FileInvertCallback As New WaitCallback(AddressOf SpectroscopyFileReverser)

    Private _ReverseColumnName As String = ""
    Private _ReversedColumnTargetName As String

    Private _ReversionWithFetchRunning As Boolean = False
    Public ReadOnly Property ReversionProcedureCreatesFetchEvent As Boolean
        Get
            Return Me._ReversionWithFetchRunning
        End Get
    End Property
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the inversion was successfull.
    ''' </summary>
    Public Event FileReversionComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef InvertedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Revert Function"
    ''' <summary>
    ''' Starts the reversion for the selected File and the selected columns.
    ''' </summary>
    Public Sub ReverseColumnWithoutFetching_Async(ByVal ReverseColumnName As String,
                                                  Optional ByVal ReversedColumnTargetName As String = "reversed data")
        Me._ReverseColumnName = ReverseColumnName
        Me._ReversedColumnTargetName = ReversedColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileInvertCallback, Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Starts the reversion procedure selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the reversion.
    ''' </summary>
    Public Sub ReverseColumnWITHAutomaticFetching_Async(ByVal ReverseColumnName As String,
                                                        Optional ByVal ReverseColumnTargetName As String = "reversed data")
        Me._ReverseColumnName = ReverseColumnName
        Me._ReversedColumnTargetName = ReverseColumnTargetName

        _ReversionWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the Smoothing
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not _ReversionWithFetchRunning Then Return
        _ReversionWithFetchRunning = False

        ' Start the reversion using the fetched data.
        Me.ReverseColumnWithoutFetching_Async(Me._ReverseColumnName,
                                              Me._ReversedColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the reversion procedure selected column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function is executed directly.
    ''' </summary>
    Public Sub ReverseColumnWITHFetching_Direct(ByVal ReverseColumnName As String,
                                                Optional ByVal ReverseColumnTargetName As String = "reversed data")
        Me._ReverseColumnName = ReverseColumnName
        Me._ReversedColumnTargetName = ReverseColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Smooth the data
        Me.SpectroscopyFileReverser(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File inverter to invert the data in a separate thread.
    ''' </summary>
    Private Sub SpectroscopyFileReverser(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me._ReverseColumnName) Then
            'MessageBox.Show("Column for reversion not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '################################
        ' Start inversion procedure.

        Me._ReversedColumn = SpectroscopyTable.Column(Me._ReverseColumnName).GetCopy
        Me._ReversedColumn.SetValueList(Me._ReversedColumn.Values(True).Reverse.ToList)
        Me._ReversedColumn.UnitSymbol = SpectroscopyTable.Column(Me._ReverseColumnName).UnitSymbol
        Me._ReversedColumn.Name = Me._ReversedColumnTargetName

        RaiseEvent FileReversionComplete(Me.CurrentSpectroscopyTable, Me._ReversedColumn)
    End Sub
#End Region

#Region "Save inverted column"

    ''' <summary>
    ''' Saves the current reversed column to the file-object
    ''' </summary>
    Public Sub SaveReversedColumnToFileObject(ByVal ColumnName As String)
        If Me.ReversedColumn Is Nothing Then Return

        ' Save the column
        Me._ReversedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._ReversedColumn)
    End Sub

#End Region

End Class
