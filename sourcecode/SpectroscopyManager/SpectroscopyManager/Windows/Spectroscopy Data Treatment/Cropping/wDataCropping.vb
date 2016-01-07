Public Class wDataCropping
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

#Region "Properties"

    ''' <summary>
    ''' Limits for the reference of the crop data.
    ''' </summary>
    Private MinIndexIncl As Integer = My.Settings.LastCropping_LeftPoint
    Private MaxIndexIncl As Integer = My.Settings.LastCropping_RightPoint

    ''' <summary>
    ''' Returns the current limits as crop information object.
    ''' </summary>
    Private ReadOnly Property CI As cSpectroscopyTable.DataColumn.CropInformation
        Get
            Return New cSpectroscopyTable.DataColumn.CropInformation(MinIndexIncl, MaxIndexIncl)
        End Get
    End Property

    ''' <summary>
    ''' Callback for thread save interface changes.
    ''' </summary>
    Delegate Sub ActivateInterfaceButtonCallback(ByVal Enabled As Boolean)

#End Region

#Region "Form Contructor"
    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set Window Title
        Me.Text &= MyBase._FileObject.FileName
    End Sub
#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the Spectroscopy-File to the display.
    ''' </summary>
    Public Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall

        ' Plot the data
        Me.ReplotData()

        ' Load Properties from Settings if possible!
        With My.Settings
            Me.txtMinIndexIncl.SetValue(.LastCropping_LeftPoint)
            If .LastCropping_RightPoint < 0 Then
                .LastCropping_RightPoint = SpectroscopyTable.MeasurementPoints
            End If
            Me.txtMaxIndexIncl.SetValue(.LastCropping_RightPoint)
        End With

    End Sub

    ''' <summary>
    ''' Plots the current SpectroscopyTable Object.
    ''' </summary>
    Protected Sub ReplotData()
        ' Set Preview-Images:
        Me.ReplotCropRanges()
        With Me.pbBefore
            .SetSinglePreviewImage(Me.CurrentSpectroscopyTable)
            .AllowAdjustingXColumn = False
            .cbX.SelectedColumnName = My.Resources.ColumnName_MeasurementPoints
            .cbY.SelectedColumnName = My.Settings.LastCropping_ColumnY
        End With
    End Sub

    ''' <summary>
    ''' Updates the crop ranges.
    ''' </summary>
    Protected Sub ReplotCropRanges()

        With Me.pbBefore
            ' Set the crop markers to display
            .ClearHighlightRanges()
            .AddHighlightRange(mSpectroscopyTableViewer.SelectionModes.XRange,
                               New ZedGraph.PointPair(MinIndexIncl, 0),
                               New ZedGraph.PointPair(MaxIndexIncl, 0))

        End With
    End Sub

#End Region

#Region "Data Cropping"
    ''' <summary>
    ''' Applies the selected crop
    ''' </summary>
    Public Sub btnCrop_Click(sender As System.Object, e As System.EventArgs) Handles btnCrop.Click

        ' Save the selected Column
        My.Settings.LastCropping_ColumnY = Me.pbBefore.cbY.SelectedColumnName

        ' Set the crop information for the file-object
        Me.CurrentSpectroscopyTable.SetCropInformation(CI)

        Me.SetSaveButton(True)

        ' Plot the data
        Me.ReplotData()

    End Sub

#End Region

#Region "Reset the data"

    ''' <summary>
    ''' Resets all Changes by resetting the FileObject
    ''' </summary>
    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click

        ' Remove the existing crop information.
        'Me._FileObject = Me._FileObjectCopyOfOriginal.GetCopy
        Me.SpectroscopyTable.SetCropInformation(New cSpectroscopyTable.DataColumn.CropInformation)
        Me._FileObject.Remove_SpectroscopyTable_CropInformation(True)

        ' Reset the save-button so disabled.
        Me.SetSaveButton(False)

        ' Plot the data
        Me.ReplotData()

    End Sub

#End Region

#Region "Activate Interface"

    ''' <summary>
    ''' This will be called from the worker thread to activate or deactivate the ColumnSave buttons
    ''' </summary>
    Friend Sub SetSaveButton(ByVal Enabled As Boolean)
        If Me.btnSave.InvokeRequired Then
            Dim _delegate As New ActivateInterfaceButtonCallback(AddressOf SetSaveButton)
            Me.Invoke(_delegate, Enabled)
        Else
            Me.btnSave.Enabled = Enabled
        End If
    End Sub

#End Region

#Region "Column Saving"

    ''' <summary>
    ''' Saves the Column Gained from the Smoothing to the Source SpectroscopyTable
    ''' </summary>
    Public Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        ' remove all preview-images from the cache
        Me._FileObject.ClearAllPreviewImages()
        Me._FileObject.Set_SpectroscopyTable_CropInformation(CI, True)
    End Sub

#End Region

#Region "Point-Selection of the Crop-Range"
    ''' <summary>
    ''' Save the left and right border.
    ''' </summary>
    Private Sub txtAbsoluteValue_TextChanged(ByRef SourceTextBox As NumericTextbox) Handles txtMinIndexIncl.ValidValueChanged, txtMaxIndexIncl.ValidValueChanged
        If SourceTextBox Is Me.txtMaxIndexIncl Then
            Me.MaxIndexIncl = SourceTextBox.IntValue
        ElseIf SourceTextBox Is Me.txtMinIndexIncl Then
            Me.MinIndexIncl = SourceTextBox.IntValue
        End If

        If Me.MaxIndexIncl < Me.MinIndexIncl Then
            Dim TMP As Integer = Me.MaxIndexIncl
            Me.MaxIndexIncl = Me.MinIndexIncl
            Me.MinIndexIncl = TMP

            Me.txtMinIndexIncl.SetValue(Me.MinIndexIncl,, False)
            Me.txtMaxIndexIncl.SetValue(Me.MaxIndexIncl,, False)
        End If

        Me.ReplotCropRanges()
    End Sub

    ''' <summary>
    ''' Calculate from the entered absolute value the nearest point number.
    ''' </summary>
    Private Sub PointSelectionChanged(ByVal LeftValue As Double,
                                      ByVal RightValue As Double) Handles pbBefore.PointSelectionChanged_XRange
        Me.MinIndexIncl = CInt(LeftValue)
        Me.MaxIndexIncl = CInt(RightValue)

        Me.txtMinIndexIncl.SetValue(Me.MinIndexIncl,, False)
        Me.txtMaxIndexIncl.SetValue(Me.MaxIndexIncl,, False)

        Me.ReplotCropRanges()
    End Sub
#End Region

#Region "Window Closing"
    ''' <summary>
    ''' Close Window
    ''' </summary>
    Private Sub btnCloseWindow_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseWindow.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Function that runs when the form is closed... saves Settings.
    ''' </summary>
    Private Sub FormIsClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Saves the Chosen Parameters to the Settings.
        With My.Settings
            .LastCropping_LeftPoint = Me.MinIndexIncl
            .LastCropping_RightPoint = Me.MaxIndexIncl
            .LastCropping_ColumnX = Me.pbBefore.cbX.SelectedColumnName
            .LastCropping_ColumnY = Me.pbBefore.cbY.SelectedColumnName
            .Save()
        End With
    End Sub

#End Region

End Class