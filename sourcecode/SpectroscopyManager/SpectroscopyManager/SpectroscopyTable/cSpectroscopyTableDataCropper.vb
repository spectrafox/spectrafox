Imports System.Threading

''' <summary>
''' Class for Cropping Data our of a Spectroscopy-Table
''' </summary>
Public Class cSpectroscopyTableDataCropper
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Callback-Function to Smooth the Data in a separate Thread.
    ''' </summary>
    Private FileCropCallback As New WaitCallback(AddressOf SpectroscopyFileCropper)

    Private ColumnNameOfColumnToCrop As String = ""
    Private MinIndexIncl As Integer
    Private MaxIndexIncl As Integer

    Private _CropInformationStorage As cSpectroscopyTable.DataColumn.CropInformation
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the cropping was successfull.
    ''' </summary>
    Public Event FileCroppingComplete(ByRef CroppedSpectroscopyTable As cSpectroscopyTable)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Crop Function"
    ''' <summary>
    ''' Starts the Crop for the selected File.
    ''' </summary>
    Public Sub CropColumnWithoutFetching_ASync(ByVal ColumnName As String,
                                               ByVal CropUpToLowerValueIncl As Integer,
                                               ByVal CropFromUpperValueIncl As Integer)
        Me.MinIndexIncl = CropUpToLowerValueIncl
        Me.MaxIndexIncl = CropFromUpperValueIncl
        Me.ColumnNameOfColumnToCrop = ColumnName

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileCropCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private CropWithFetchRunning As Boolean = False
    Public ReadOnly Property CroppingProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.CropWithFetchRunning
        End Get
    End Property

    ''' <summary>
    ''' Starts the Crop Procedure and automatically fetches
    ''' the SpectroscopyTable in advance from the file.
    ''' This function treats all events of the multi-threaded-file-fetching
    ''' procedure, before initializing the derivation.
    ''' </summary>
    Public Sub CropColumnWITHAutomaticFetching_Async(ByVal ColumnName As String,
                                                     ByVal CropUpToLowerValueIncl As Integer,
                                                     ByVal CropFromUpperValueIncl As Integer)
        Me.MinIndexIncl = CropUpToLowerValueIncl
        Me.MaxIndexIncl = CropFromUpperValueIncl
        Me.ColumnNameOfColumnToCrop = ColumnName

        CropWithFetchRunning = True

        ' Start the Spectroscopy-TableFetcher
        Me.FetchAsync()
    End Sub

    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the Cropping
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not CropWithFetchRunning Then Return
        CropWithFetchRunning = False

        ' Start the Cropping using the Fetched Data.
        Me.CropColumnWithoutFetching_ASync(Me.ColumnNameOfColumnToCrop, Me.MinIndexIncl, Me.MaxIndexIncl)
    End Sub

    ''' <summary>
    ''' Starts the Crop for the selected File.
    ''' </summary>
    Public Sub CropColumnWithFetching_Direct(ByVal ColumnName As String,
                                             ByVal CropUpToLowerValueIncl As Integer,
                                             ByVal CropFromUpperValueIncl As Integer)
        Me.MinIndexIncl = CropUpToLowerValueIncl
        Me.MaxIndexIncl = CropFromUpperValueIncl

        ' Start the Spectroscopy-TableFetcher
        Me.FetchDirect()

        ' Crop the data
        Me.SpectroscopyFileCropper(Me.CurrentSpectroscopyTable)
    End Sub

    ''' <summary>
    ''' Spectroscopy File Cropper the data in a separate thread.
    ''' </summary>
    Private Sub SpectroscopyFileCropper(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        '##########################################
        ' Start Cropping of all Columns Procedure.

        ' Depending on selected Column, crop the data
        If SpectroscopyTable.ColumnExists(Me.ColumnNameOfColumnToCrop) Then

            ' Crop all columns
            Me._CropInformationStorage = New cSpectroscopyTable.DataColumn.CropInformation(Me.MinIndexIncl, Me.MaxIndexIncl)
            SpectroscopyTable.SetCropInformation(Me._CropInformationStorage)

        End If

        RaiseEvent FileCroppingComplete(Me.CurrentSpectroscopyTable)
    End Sub
#End Region

#Region "Save - Write Columns back to FileObject"
    ''' <summary>
    ''' Saves the Cropped Data back to the FileObject, that other modules can handle the shortened data
    ''' </summary>
    Public Sub SaveBackToFileObject()
        With Me.CurrentFileObject
            ' Save all Crop-Informations
            .Set_SpectroscopyTable_CropInformation(Me._CropInformationStorage)
        End With
    End Sub
#End Region
End Class

