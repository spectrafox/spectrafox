Imports System.Threading

Public Class cSpectroscopyTableDataSummerTwoSets
    Inherits cSpectroscopyTableFetcherMultiple

#Region "Settings"

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
    ''' Factor to use for scaling of the source column before summation/substration
    ''' </summary>
    Public Property dScalingFactor As Double = 1D
    Private _ColumnTargetName As String

    ''' <summary>
    ''' Callback-Function to sum the data in a separate thread.
    ''' </summary>
    Private FileCallback As New WaitCallback(AddressOf SpectroscopyFileSummer)

    ' ASYNC storage

    Private SummationWithFetchRunning As Boolean = False
    Public ReadOnly Property SummationProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.SummationWithFetchRunning
        End Get
    End Property

    Private sName_ColumnToSum As String = ""
    Private sName_ColumnToSumWith As String = ""

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
    Public Sub New(ByRef SpectroscopyFileToSum As cFileObject,
                   ByRef SpectroscopyFileToTakeSummationColumnFrom As cFileObject)
        MyBase.New({SpectroscopyFileToSum, SpectroscopyFileToTakeSummationColumnFrom})
    End Sub
#End Region

#Region "Start the action"

    ''' <summary>
    ''' Starts the Summation for the selected file and the selected columns
    ''' with the selected method.
    ''' </summary>
    Public Sub SumColumnWithoutFetching_ASync(ByVal Name_ColumnToSum As String,
                                              ByVal Name_ColumnToSumWith As String,
                                              ByVal ScalingFactor As Double,
                                              Optional ByVal ColumnTargetName As String = "Summation Result")
        Me.sName_ColumnToSum = Name_ColumnToSum
        Me.sName_ColumnToSumWith = Name_ColumnToSumWith
        Me.dScalingFactor = ScalingFactor
        Me._ColumnTargetName = ColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileCallback, Me.lSpectroscopyTableList.Values.ToArray)
    End Sub

    ''' <summary>
    ''' Starts the summation procedure for the selected column and automatically fetches
    ''' the SpectroscopyTables in advance from the files.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the summation.
    ''' </summary>
    Public Sub SumColumnWITHAutomaticFetching_ASync(ByVal Name_ColumnToSum As String,
                                                    ByVal Name_ColumnToSumWith As String,
                                                    ByVal ScalingFactor As Double,
                                                    Optional ByVal ColumnTargetName As String = "Summation Result")
        Me.sName_ColumnToSum = Name_ColumnToSum
        Me.sName_ColumnToSumWith = Name_ColumnToSumWith
        Me.dScalingFactor = ScalingFactor
        Me._ColumnTargetName = ColumnTargetName

        SummationWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the summation
    ''' </summary>
    Private Sub FetchCompleteHandler() Handles MyBase.FileFetchedComplete
        If Not SummationWithFetchRunning Then Return
        SummationWithFetchRunning = False


        Me.SumColumnWithoutFetching_ASync(Me.sName_ColumnToSum,
                                          Me.sName_ColumnToSumWith,
                                          Me.dScalingFactor,
                                          Me._ColumnTargetName)


    End Sub

    ''' <summary>
    ''' Starts the summation procedure for the selected column and fetches
    ''' the SpectroscopyTables in advance from the file.
    ''' </summary>
    Public Sub SumColumnWITHAutomaticFetching_Direct(ByVal Name_ColumnToSum As String,
                                                     ByVal Name_ColumnToSumWith As String,
                                                     ByVal ScalingFactor As Double,
                                                     Optional ByVal ColumnTargetName As String = "Summation Result")
        Me.sName_ColumnToSum = Name_ColumnToSum
        Me.sName_ColumnToSumWith = Name_ColumnToSumWith
        Me.dScalingFactor = ScalingFactor
        Me._ColumnTargetName = ColumnTargetName

        ' Start the SpectroscopyTableFetcher
        Me.FetchDirect()

        ' Do it
        Me.SpectroscopyFileSummer(Me.lSpectroscopyTableList.Values.ToArray)
    End Sub

#End Region

#Region "Summation"

    ''' <summary>
    ''' Spectroscopy File to sum the data in a separate thread.
    ''' </summary>
    Private Sub SpectroscopyFileSummer(SpectroscopyFiles As Object)
        Dim lSpectroscopyFiles As cSpectroscopyTable() = CType(SpectroscopyFiles, cSpectroscopyTable())
        Dim SpectroscopyTableToSum As cSpectroscopyTable = lSpectroscopyFiles.First
        Dim SpectroscopyTableToTakeSummationColumnFrom As cSpectroscopyTable = lSpectroscopyFiles.Last

        If Not SpectroscopyTableToSum.ColumnExists(Me.sName_ColumnToSum) Then
            MessageBox.Show("column for summation not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not SpectroscopyTableToTakeSummationColumnFrom.ColumnExists(Me.sName_ColumnToSumWith) Then
            MessageBox.Show("reference column for summation not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Get shorter local references to the columns
        Dim ColumnToSum As cSpectroscopyTable.DataColumn = SpectroscopyTableToSum.Column(Me.sName_ColumnToSum)
        Dim ColumnToSumValues As ReadOnlyCollection(Of Double) = ColumnToSum.Values
        Dim ColumnToSumWith As cSpectroscopyTable.DataColumn = SpectroscopyTableToTakeSummationColumnFrom.Column(Me.sName_ColumnToSumWith)
        Dim ColumnToSumWithValues As ReadOnlyCollection(Of Double) = ColumnToSumWith.Values

        If ColumnToSumValues.Count <> ColumnToSumWithValues.Count Then
            MessageBox.Show("reference column and column to be summed are not of same length",
                            "error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '################################
        ' Start summation Procedure.

        Me._SummedColumn = New cSpectroscopyTable.DataColumn


        For i As Integer = 0 To ColumnToSumValues.Count - 1 Step 1

            ' Only sum values, if there are values set!
            If Double.IsNaN(ColumnToSumValues(i)) Or
                Double.IsInfinity(ColumnToSumValues(i)) Or
                Double.IsNaN(ColumnToSumWithValues(i)) Or
                Double.IsInfinity(ColumnToSumWithValues(i)) Then

                Me._SummedColumn.AddValueToColumn(Double.NaN)

            Else

                Me._SummedColumn.AddValueToColumn(ColumnToSumValues(i) + Me.dScalingFactor * ColumnToSumWithValues(i))

            End If
        Next

        Me._SummedColumn.UnitSymbol = ColumnToSum.UnitSymbol
        Me._SummedColumn.Name = Me._ColumnTargetName

        RaiseEvent FileSummationComplete(Me.lSpectroscopyTableList.First.Value, Me._SummedColumn)
    End Sub


#End Region

#Region "Save Calculated Column"

    ''' <summary>
    ''' Saves the summed column to the file-object
    ''' </summary>
    Public Sub SaveSummedColumnToFileObject(ByVal ColumnName As String)
        If Me._SummedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._SummedColumn.Name = ColumnName
        Me.lSpectroscopyTableList.First.Key.AddSpectroscopyColumn(Me._SummedColumn)
    End Sub

#End Region

End Class
