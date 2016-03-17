Imports System.Threading

Public Class cSpectroscopyTableDataRegaugeByFitParameters
    Inherits cSpectroscopyTableFetcher

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

    ''' <summary>
    ''' Object for storing the regauged column
    ''' </summary>
    Private _RegaugedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the current regauged column
    ''' </summary>
    Public ReadOnly Property RegaugedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._RegaugedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._RegaugedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to regauge the data in a separate thread.
    ''' </summary>
    Private FileRegaugeByGivenParametersCallback As New WaitCallback(AddressOf SpectroscopyFileRegaugerByGivenParameters)

    Private _RegaugeColumnNameConsideredAsGaugeTarget As String = ""
    Private _RegaugeColumnTargetName As String
    Private RegaugeWithFetchRunning As Boolean = False
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the regauge-procedure was successfull.
    ''' </summary>
    Public Event FileRegaugeComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RegaugedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Regauging by given parameters"
    ''' <summary>
    ''' Starts the regauge-procedure for selected column with the given input parameters y0 and m,
    ''' as slope and y-offset.
    ''' </summary>
    Public Sub RegaugeColumnByParameters_Direct(ByVal RegaugeColumnNameConsideredAsGaugeTarget As String,
                                                ByVal in_Parameter_y0 As Double,
                                                ByVal in_Parameter_m As Double,
                                                Optional ByVal ColumnTargetName As String = "regauged result")

        ' save the column-name to use
        Me._RegaugeColumnNameConsideredAsGaugeTarget = RegaugeColumnNameConsideredAsGaugeTarget
        Me._RegaugeColumnTargetName = ColumnTargetName
        Me._Parameter_m = in_Parameter_m
        Me._Parameter_y0 = in_Parameter_y0

        ' Fetch the spectroscopy-file.
        Me.FetchDirect()

        ' Renormalize
        Me.SpectroscopyFileRegaugerByGivenParameters(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Starts the regauge-procedure by given parameters. This function treats all events of the
    ''' multi-threaded file fetching before initializing the actual procedure.
    ''' </summary>
    Public Sub RenormalizeColumnByParameters_Async(ByVal RegaugeColumnNameConsideredAsGaugeTarget As String,
                                                   ByVal in_Parameter_y0 As Double,
                                                   ByVal in_Parameter_m As Double,
                                                   Optional ByVal ColumnTargetName As String = "regauged result")
        ' save the column-name to use
        Me._RegaugeColumnNameConsideredAsGaugeTarget = RegaugeColumnNameConsideredAsGaugeTarget
        Me._RegaugeColumnTargetName = ColumnTargetName
        Me._Parameter_m = in_Parameter_m
        Me._Parameter_y0 = in_Parameter_y0

        RegaugeWithFetchRunning = True

        ' Send SpectroscopyTable to ThreadPoolQueue of the DataSmoother
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the fetch-complete event and starts the regauge procedure.
    ''' </summary>
    Private Sub FetchCompleteHandlerToStartParametrization(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not RegaugeWithFetchRunning Then Return
        RegaugeWithFetchRunning = False

        ' Start the renormalization using the fetched data.
        Me.RegaugeColumnByParametersWithoutFetch_Async(Me._RegaugeColumnNameConsideredAsGaugeTarget,
                                                       Me._Parameter_y0,
                                                       Me._Parameter_m,
                                                       Me._RegaugeColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the regauge-procedure, expects an already fetched file-object.
    ''' </summary>
    Public Sub RegaugeColumnByParametersWithoutFetch_Async(ByVal RegaugeColumnNameConsideredAsGaugeTarget As String,
                                                           ByVal in_Parameter_y0 As Double,
                                                           ByVal in_Parameter_m As Double,
                                                           Optional ByVal ColumnTargetName As String = "regauged result")
        Me._RegaugeColumnNameConsideredAsGaugeTarget = RegaugeColumnNameConsideredAsGaugeTarget
        Me._RegaugeColumnTargetName = ColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileRegaugeByGivenParametersCallback, Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Regauges the data in a separate thread.
    ''' Uses the given input parameters for the regauge procedure.
    ''' Does not check, if these parameters are valid.
    ''' </summary>
    Private Sub SpectroscopyFileRegaugerByGivenParameters(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        Try
            If Not SpectroscopyTable.ColumnExists(Me._RegaugeColumnNameConsideredAsGaugeTarget) Then
                Throw New ArgumentException("Y-axis column for re-gauging procedure not found")
            End If

            '############################
            ' Start regauge procedure.
            Me._RegaugedColumn = New cSpectroscopyTable.DataColumn

            ' Renormalize
            Me._RegaugedColumn.SetValueList(cNumericalMethods.RenormalizeByGivenParameters(SpectroscopyTable.Column(Me._RegaugeColumnNameConsideredAsGaugeTarget).Values,
                                                                                           Me.Parameter_y0,
                                                                                           Me.Parameter_m))
            Me._RegaugedColumn.Name = Me._RegaugeColumnTargetName

            RaiseEvent FileRegaugeComplete(Me.CurrentSpectroscopyTable, Me._RegaugedColumn)
        Catch ex As Exception
            MessageBox.Show(ex.Message, My.Resources.title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

#End Region

#Region "save the regauged column"
    ''' <summary>
    ''' Saves the current regauged column to the File-Object
    ''' </summary>
    Public Sub SaveRegaugedColumnToFileObject(ByVal ColumnName As String)
        If Me._RegaugedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._RegaugedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._RegaugedColumn)
    End Sub
#End Region

End Class
