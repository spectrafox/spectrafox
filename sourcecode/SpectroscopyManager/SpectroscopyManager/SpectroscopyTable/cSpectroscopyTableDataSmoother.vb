Imports System.Threading

''' <summary>
''' Class for Smoothing of a Data
''' </summary>
Public Class cSpectroscopyTableDataSmoother
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Object for storing the Smoothed Column
    ''' </summary>
    Protected _SmoothedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the Current Smoothed Column
    ''' </summary>
    Public ReadOnly Property SmoothedColumn As cSpectroscopyTable.DataColumn
        Get
            Return Me._SmoothedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to Smooth the Data in a separate Thread.
    ''' </summary>
    Private FileSmoothCallback As New WaitCallback(AddressOf SpectroscopyFileSmoother)


    Private SmoothColumnName As String = ""
    Private SmoothMethod As cNumericalMethods.SmoothingMethod
    Private SmoothParameter As Integer
    Private SmoothColumnTargetName As String
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the smoothing was successfull.
    ''' </summary>
    Public Event FileSmoothingComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef SmoothedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Smooth Function"
    ''' <summary>
    ''' Starts the Smooth for the selected File and the selected Columns.
    ''' </summary>
    Public Sub SmoothColumnWithoutFetching_Async(ByVal SmoothColumnName As String,
                                                 ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                                 ByVal SmoothParameter As Integer,
                                                 Optional ByVal SmoothedColumnTargetName As String = "Smoothed Result")
        Me.SmoothColumnName = SmoothColumnName
        Me.SmoothParameter = SmoothParameter
        Me.SmoothMethod = SmoothProcedure
        Me.SmoothColumnTargetName = SmoothedColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileSmoothCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private SmoothingWithFetchRunning As Boolean = False
    Public ReadOnly Property SmoothingProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.SmoothingWithFetchRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the Smoothing Procedure selected Column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the derivation.
    ''' </summary>
    Public Sub SmoothColumnWITHAutomaticFetching_Async(ByVal SmoothColumnName As String,
                                                       ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                                       ByVal SmoothParameter As Integer,
                                                       Optional ByVal SmoothedColumnTargetName As String = "Smoothed Result")
        Me.SmoothColumnName = SmoothColumnName
        Me.SmoothParameter = SmoothParameter
        Me.SmoothMethod = SmoothProcedure
        Me.SmoothColumnTargetName = SmoothedColumnTargetName

        SmoothingWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the Smoothing
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not SmoothingWithFetchRunning Then Return
        SmoothingWithFetchRunning = False

        ' Start the Smoothing using the Fetched Data.
        Me.SmoothColumnWithoutFetching_Async(Me.SmoothColumnName,
                                             Me.SmoothMethod,
                                             Me.SmoothParameter,
                                             Me.SmoothColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the Smoothing Procedure selected Column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function is executed directly.
    ''' </summary>
    Public Sub SmoothColumnWITHFetching_Direct(ByVal SmoothColumnName As String,
                                               ByVal SmoothProcedure As cNumericalMethods.SmoothingMethod,
                                               ByVal SmoothParameter As Integer,
                                               Optional ByVal SmoothedColumnTargetName As String = "Smoothed Result")
        Me.SmoothColumnName = SmoothColumnName
        Me.SmoothParameter = SmoothParameter
        Me.SmoothMethod = SmoothProcedure
        Me.SmoothColumnTargetName = SmoothedColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Smooth the data
        Me.SpectroscopyFileSmoother(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Smoother to Smooth the data in a separate Thread.
    ''' </summary>
    Private Sub SpectroscopyFileSmoother(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me.SmoothColumnName) Then
            'MessageBox.Show("Column for Smoothing not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '################################
        ' Start Smoooooothing Procedure.

        Me._SmoothedColumn = New cSpectroscopyTable.DataColumn

        ' Depending on selected Smoothing Method, start Smoothing
        Select Case Me.SmoothMethod
            Case cNumericalMethods.SmoothingMethod.AdjacentAverageSmooth
                Me._SmoothedColumn.SetValueList(cNumericalMethods.AdjacentAverageSmooth(SpectroscopyTable.Column(Me.SmoothColumnName).Values, Me.SmoothParameter))
            Case cNumericalMethods.SmoothingMethod.SavitzkyGolay
                Me._SmoothedColumn.SetValueList(cNumericalMethods.SavitzkyGolaySmooth(SpectroscopyTable.Column(Me.SmoothColumnName).Values, Me.SmoothParameter))
            Case Else
                Me._SmoothedColumn.SetValueList(SpectroscopyTable.Column(Me.SmoothColumnName).Values.ToList)
        End Select
        Me._SmoothedColumn.UnitSymbol = SpectroscopyTable.Column(Me.SmoothColumnName).UnitSymbol
        Me._SmoothedColumn.Name = Me.SmoothColumnTargetName

        RaiseEvent FileSmoothingComplete(Me.CurrentSpectroscopyTable, Me._SmoothedColumn)
    End Sub
#End Region

#Region "Save Smoothed Column"

    ''' <summary>
    ''' Saves the Current Smoothed Column to the File-Object
    ''' </summary>
    Public Sub SaveSmoothedColumnToFileObject(ByVal ColumnName As String)
        If Me.SmoothedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._SmoothedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._SmoothedColumn)
    End Sub

#End Region

End Class
