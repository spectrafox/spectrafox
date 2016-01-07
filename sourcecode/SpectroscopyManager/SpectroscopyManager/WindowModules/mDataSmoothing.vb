Public Class mDataSmoothing

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        With Me.cbMethods
            .Items.Clear()
            .ValueMember = "Value"
            .DisplayMember = "Key"
            .Items.Add(New KeyValuePair(Of String, cNumericalMethods.SmoothingMethod)(My.Resources.SmoothingMethod_SavitzkyGolay, cNumericalMethods.SmoothingMethod.SavitzkyGolay))
            .Items.Add(New KeyValuePair(Of String, cNumericalMethods.SmoothingMethod)(My.Resources.SmoothingMethod_AdjacentAverage, cNumericalMethods.SmoothingMethod.AdjacentAverageSmooth))
            .SelectedIndex = 0
        End With
    End Sub

    ''' <summary>
    ''' Constructor, that writes the Smoothing Methods and Start-Values into the Interface.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mDataSmoothing_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
       
    End Sub

    ''' <summary>
    ''' Change the Description of the Smoothing Method.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cbMethods_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbMethods.SelectedIndexChanged
        If Me.cbMethods.SelectedItem Is Nothing Then Return
        
        ' Show Description of Smoothing Method:
        Me.txtDescription.Text = cNumericalMethods.GetSmoothingDescriptionFromType(Me.SelectedSmoothingMethod)

        ' Write Property-Name and set Maximum Values for the Smoothing Property:
        Select Case Me.SelectedSmoothingMethod
            Case cNumericalMethods.SmoothingMethod.AdjacentAverageSmooth
                Me.lblPropertyName.Text = My.Resources.SmoothingPropertyName_AdjacentAverage
                Me.udSmoothProperties.Maximum = 500
                Me.udSmoothProperties.Minimum = 1
                Me.udSmoothProperties.Value = 3
            Case cNumericalMethods.SmoothingMethod.SavitzkyGolay
                Me.lblPropertyName.Text = My.Resources.SmoothingPropertyName_SavitzkyGolay
                Me.udSmoothProperties.Maximum = 12
                Me.udSmoothProperties.Minimum = 2
                Me.udSmoothProperties.Value = 5
        End Select
    End Sub

    ''' <summary>
    ''' Set/Get selected Smoothing Method
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectedSmoothingMethod() As cNumericalMethods.SmoothingMethod
        Get
            If Me.cbMethods.SelectedItem Is Nothing Then Return cNumericalMethods.SmoothingMethod.AdjacentAverageSmooth
            Return DirectCast(Me.cbMethods.SelectedItem, KeyValuePair(Of String, cNumericalMethods.SmoothingMethod)).Value
        End Get
        Set(value As cNumericalMethods.SmoothingMethod)
            For Each Method As KeyValuePair(Of String, cNumericalMethods.SmoothingMethod) In Me.cbMethods.Items
                If Method.Value = value Then
                    Me.cbMethods.SelectedItem = Method
                End If
            Next
        End Set
    End Property

    ''' <summary>
    ''' Set/Get selected NeighborNumber
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SmoothingParameter() As Integer
        Get
            Return Convert.ToInt32(Me.udSmoothProperties.Value)
        End Get
        Set(value As Integer)
            Me.udSmoothProperties.Value = value
        End Set
    End Property

#Region "Obsolete Direct Smoothing Functions"
    ''' <summary>
    ''' Returns the given Data Smoothed by the Selected Method.
    ''' </summary>
    Public Function GetSmoothedData(ByRef InData As ICollection(Of Double)) As List(Of Double)
        ' Return the Smoothed Data
        Select Case Me.SelectedSmoothingMethod
            Case cNumericalMethods.SmoothingMethod.AdjacentAverageSmooth
                Return cNumericalMethods.AdjacentAverageSmooth(InData, Convert.ToInt32(Me.udSmoothProperties.Value))
            Case cNumericalMethods.SmoothingMethod.SavitzkyGolay
                Return cNumericalMethods.SavitzkyGolaySmooth(InData, Convert.ToInt32(Me.udSmoothProperties.Value))
            Case Else
                Return InData.ToList
        End Select
    End Function

    ''' <summary>
    ''' Returns the given Data Smoothed by the Selected Method.
    ''' </summary>
    Public Function GetSmoothedData(ByRef InDataColumn As cSpectroscopyTable.DataColumn) As List(Of Double)
        Return Me.GetSmoothedData(InDataColumn.Values)
    End Function
#End Region

End Class
