Imports System.Threading

Public Class cSpectroscopyTableFFT
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Object for storing the FFT Column
    ''' </summary>
    Protected _FFTColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the Current Smoothed Column
    ''' </summary>
    Public ReadOnly Property FFTColumn As cSpectroscopyTable.DataColumn
        Get
            Return Me._FFTColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to FFT the Data in a separate Thread.
    ''' </summary>
    Private FileFFTCallback As New WaitCallback(AddressOf SpectroscopyFileFFT)


    Private FFTColumnName As String = ""
    Private FFTColumnTargetName As String
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the FFT was successfull.
    ''' </summary>
    Public Event FileFFTComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef FFTDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "FFT Function"
    ''' <summary>
    ''' Starts the FFT for the selected File and the selected Columns.
    ''' </summary>
    Public Sub FFTColumnWithoutFetching_Async(ByVal FFTColumnName As String,
                                              Optional ByVal FFTColumnTargetName As String = "FFT Result")
        Me.FFTColumnName = FFTColumnName
        Me.FFTColumnTargetName = FFTColumnTargetName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileFFTCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private FFTWithFetchRunning As Boolean = False
    Public ReadOnly Property FFTProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.FFTWithFetchRunning
        End Get
    End Property
    ''' <summary>
    ''' Starts the FFT Procedure selected Column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the derivation.
    ''' </summary>
    Public Sub FFTColumnWITHAutomaticFetching_Async(ByVal FFTColumnName As String,
                                                    Optional ByVal FFTColumnTargetName As String = "FFT Result")
        Me.FFTColumnName = FFTColumnName
        Me.FFTColumnTargetName = FFTColumnTargetName

        FFTWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the Smoothing
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not FFTWithFetchRunning Then Return
        FFTWithFetchRunning = False

        ' Start the Smoothing using the Fetched Data.
        Me.FFTColumnWithoutFetching_Async(Me.FFTColumnName,
                                          Me.FFTColumnTargetName)
    End Sub

    ''' <summary>
    ''' Starts the FFT Procedure selected Column taken and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function is executed directly.
    ''' </summary>
    Public Sub FFTColumnWITHFetching_Direct(ByVal FFTColumnName As String,
                                            Optional ByVal FFTColumnTargetName As String = "FFT Result")
        Me.FFTColumnName = FFTColumnName
        Me.FFTColumnTargetName = FFTColumnTargetName

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Smooth the data
        Me.SpectroscopyFileFFT(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Smoother to Smooth the data in a separate Thread.
    ''' </summary>
    Private Sub SpectroscopyFileFFT(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me.FFTColumnName) Then
            Return
        End If

        '################################
        ' Start FFT Procedure.
        Me._FFTColumn = New cSpectroscopyTable.DataColumn

        Dim ArrayOfComplex(SpectroscopyTable.Column(Me.FFTColumnName).Values.Count - 1) As Numerics.Complex
        Dim i As Integer = 0
        For Each Value As Double In SpectroscopyTable.Column(Me.FFTColumnName).Values
            ArrayOfComplex(i) = New Numerics.Complex(Value, 0)
            i += 1
        Next

        ' start FFT
        MathNet.Numerics.IntegralTransforms.Fourier.Forward(ArrayOfComplex)
        Dim OutputList As New List(Of Double)

        For Each Value As Numerics.Complex In ArrayOfComplex
            OutputList.Add(Value.Real)
        Next

        Me._FFTColumn.SetValueList(OutputList)
        Me._FFTColumn.Name = Me.FFTColumnTargetName

        RaiseEvent FileFFTComplete(Me.CurrentSpectroscopyTable, Me._FFTColumn)
    End Sub
#End Region

#Region "Save Smoothed Column"

    ''' <summary>
    ''' Saves the current FFTColumn to the File-Object
    ''' </summary>
    Public Sub SaveColumnToFileObject(ByVal ColumnName As String)
        If Me.FFTColumn IsNot Nothing Then
            ' Add to the Extension List
            Me._FFTColumn.Name = ColumnName
            Me.CurrentFileObject.AddSpectroscopyColumn(Me._FFTColumn)
        End If
    End Sub

#End Region

End Class
