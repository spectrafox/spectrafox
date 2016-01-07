Imports System.Threading

Public Class cSpectroscopyTableDataRenormalizerByParameter
    Inherits cSpectroscopyTableFetcher

#Region "Properties"
    ''' <summary>
    ''' Object for storing the renormalized Column
    ''' </summary>
    Private _RenormalizedColumn As cSpectroscopyTable.DataColumn

    ''' <summary>
    ''' Returns the current Renormalized Column
    ''' </summary>
    Public ReadOnly Property RenormalizedColumn As cSpectroscopyTable.DataColumn
        Get
            If Me._RenormalizedColumn Is Nothing Then Return New cSpectroscopyTable.DataColumn
            Return Me._RenormalizedColumn
        End Get
    End Property

    ''' <summary>
    ''' Callback-Function to Renormalize the Data in a separate Thread.
    ''' </summary>
    Private FileRenormalizerCallback As New WaitCallback(AddressOf SpectroscopyFileRenormalizer)

    Private _RenormalizationParameter_BiasModulation As Double
    Private _RenormalizationParameter_LockInSensitivity As Double
    Private _RenormalizationParameter_AmplifierGain As Integer

    Private _RenormalizedColumnTargetName As String
    Private _ColumnNameToRenormalize As String
#End Region

#Region "Events"
    ''' <summary>
    ''' Event that gets fired, when the Renormalization was successfull.
    ''' </summary>
    Public Event FileRenormalizedComplete(ByRef SpectroscopyTable As cSpectroscopyTable, ByRef RenormalizedDataColumn As cSpectroscopyTable.DataColumn)
#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor takes the selected SpectroscopyTable-FileObject.
    ''' </summary>
    Public Sub New(ByRef SpectroscopyFile As cFileObject)
        MyBase.New(SpectroscopyFile)
    End Sub
#End Region

#Region "Renormalization Function"
    ''' <summary>
    ''' Starts the renormalization with the given parameters for the selected column.
    ''' </summary>
    Public Sub RenormalizeColumnWithoutFetch(ByVal ColumnNameToRenormalize As String,
                                             ByVal BiasModulation As Double,
                                             ByVal LockInSensitivityRange As Double,
                                             ByVal CurrentAmplifierGain As Integer,
                                             Optional ByVal RenormalizedColumnTargetName As String = "Renormalized Result")
        Me._ColumnNameToRenormalize = ColumnNameToRenormalize

        Me._RenormalizedColumnTargetName = RenormalizedColumnTargetName

        Me._RenormalizationParameter_AmplifierGain = CurrentAmplifierGain
        Me._RenormalizationParameter_LockInSensitivity = LockInSensitivityRange
        Me._RenormalizationParameter_BiasModulation = BiasModulation

        ' Send FileObject to ThreadPoolQueue
        ThreadPool.QueueUserWorkItem(FileRenormalizerCallback, Me.CurrentSpectroscopyTable)
    End Sub

    Private RenormalizationWithFetchRunning As Boolean = False
    Public ReadOnly Property RenormalizationProcedureCreatesFetchEvent As Boolean
        Get
            Return Me.RenormalizationWithFetchRunning
        End Get
    End Property
    
    ''' <summary>
    ''' Starts the Renormalization for selected Column-Name, and starting the Spectroscopy-Table-Fetch before initializing the Renormalization.
    ''' </summary>
    Public Sub RenormalizeColumnWITHFetch(ByVal ColumnNameToRenormalize As String,
                                          ByVal BiasModulation As Double,
                                          ByVal LockInSensitivityRange As Double,
                                          ByVal CurrentAmplifierGain As Integer,
                                          Optional ByVal ColumnTargetName As String = "Renormalized Result")
        Me._ColumnNameToRenormalize = ColumnNameToRenormalize

        Me._RenormalizedColumnTargetName = ColumnTargetName

        Me._RenormalizationParameter_AmplifierGain = CurrentAmplifierGain
        Me._RenormalizationParameter_LockInSensitivity = LockInSensitivityRange
        Me._RenormalizationParameter_BiasModulation = BiasModulation

        RenormalizationWithFetchRunning = True

        ' Start the SpectroscopyTableFetcher
        Me.FetchAsync()
    End Sub


    ''' <summary>
    ''' Function that catches the Fetched-Complete-Event and starts the renormalization.
    ''' </summary>
    Private Sub FetchCompleteHandler(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.FileFetchedComplete
        If Not RenormalizationWithFetchRunning Then Return
        RenormalizationWithFetchRunning = False

        ' Start the renormalization using the Fetched Data.
        ' Start the Renormalization using the Smoothed and derivated Data.
        Me.RenormalizeColumnWithoutFetch(Me._ColumnNameToRenormalize,
                                         Me._RenormalizationParameter_BiasModulation,
                                         Me._RenormalizationParameter_LockInSensitivity,
                                         Me._RenormalizationParameter_AmplifierGain,
                                         Me._RenormalizedColumnTargetName)

    End Sub

    ''' <summary>
    ''' Spectroscopy File Renormalizer to Renormalize the data.
    ''' </summary>
    Private Sub SpectroscopyFileRenormalizer(SpectroscopyTableObject As Object)
        Dim SpectroscopyTable As cSpectroscopyTable = CType(SpectroscopyTableObject, cSpectroscopyTable)

        If Not SpectroscopyTable.ColumnExists(Me._ColumnNameToRenormalize) Then
            MessageBox.Show("Target column for re-gauging procedure not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        '################################
        ' Start Renormalization Procedure.

        Dim SourceColumn As cSpectroscopyTable.DataColumn = SpectroscopyTable.Column(Me._ColumnNameToRenormalize)
        Me._RenormalizedColumn = New cSpectroscopyTable.DataColumn

        ' Renormalize
        Me._RenormalizedColumn.SetValueList(cNumericalMethods.RenormalizeByDataAcuisitionParameters(SourceColumn.Values,
                                                                                                    Me._RenormalizationParameter_BiasModulation,
                                                                                                    Me._RenormalizationParameter_LockInSensitivity,
                                                                                                    Me._RenormalizationParameter_AmplifierGain))
        Me._RenormalizedColumn.UnitSymbol = "A/V"
        Me._RenormalizedColumn.UnitType = cUnits.UnitType.Conductance
        Me._RenormalizedColumn.Name = Me._RenormalizedColumnTargetName

        RaiseEvent FileRenormalizedComplete(Me.CurrentSpectroscopyTable, Me._RenormalizedColumn)
    End Sub
#End Region

#Region "Save Renormalized Column"
    ''' <summary>
    ''' Saves the Current Renormalized Column to the File-Object
    ''' </summary>
    Public Sub SaveRenormalizedColumnToFileObject(ByVal ColumnName As String)
        If Me._RenormalizedColumn Is Nothing Then Return

        ' Add to the Extension List
        Me._RenormalizedColumn.Name = ColumnName
        Me.CurrentFileObject.AddSpectroscopyColumn(Me._RenormalizedColumn)
    End Sub
#End Region

End Class
