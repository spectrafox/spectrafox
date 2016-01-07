Public Class wDataExplorer_SpectroscopyTable
    Inherits wFormBaseExpectsIndividualSpectroscopyTableFileObjectOnLoad

    Private bReady As Boolean = False

    Private ValueSeparator As String = " "

#Region "Form Contructor"
    ''' <summary>
    ''' Form load
    ''' </summary>
    Private Sub wDataExplorer_SpectroscopyTable_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Set Window Title
        Me.Text &= MyBase._FileObject.FileName

        ' load Settings
        Me.ckbDataCollector_ShowXPoints.Checked = My.Settings.DataBrowser_DataCollectorOutput_ShowXCoord
        Me.ckbDataCollector_ShowYPoints.Checked = My.Settings.DataBrowser_DataCollectorOutput_ShowYCoord
        Me.rdbDataCollector_ShowHorizonally.Checked = My.Settings.DataBrowser_DataCollectorOutput_PrintHorizontally
        Me.rdbDataCollector_ShowVertically.Checked = Not My.Settings.DataBrowser_DataCollectorOutput_PrintHorizontally
        ValueSeparator = My.Settings.DataBrowser_DataCollectorOutput_ValueSeparator
        Me.txtDataSelector_ValueSeparator.Text = ValueSeparator
        If ValueSeparator = vbTab Then
            Me.ckbDataCollector_UseTab.Checked = True
            Me.txtDataSelector_ValueSeparator.Enabled = False
        End If

        bReady = True
    End Sub
#End Region

#Region "Sub called after successfull file-fetch process"

    ''' <summary>
    ''' Set the Spectroscopy-File to the display.
    ''' </summary>
    Public Sub SpectroscopyTableFetched(ByRef SpectroscopyTable As cSpectroscopyTable) Handles MyBase.SpectroscopyTableFetchedThreadSafeCall
        Me.SetViewerThreadSafe(SpectroscopyTable)
        Me.SetSpectrumPropertiesThreadSafe(SpectroscopyTable)
        Me.SetSpectraCommentThreadSafe(SpectroscopyTable.Comment)
        Me.SetAdditionalCommentThreadSafe(MyBase._FileObject.ExtendedComment)
        Me.SetDataTableThreadSafe(SpectroscopyTable)

        ' Non-Thread-Safe
        'Me.svDataViewer.SetSinglePreviewImage(SpectroscopyTable)
        'Me.pgSpectrumProperies.SelectedObject = SpectroscopyTable
        'Me.txtSpectraComment.Text = SpectroscopyTable.Comment
        'Me.dtSpectroscopyTable.SetSpectroscopyTable(SpectroscopyTable)

        'Me.txtAdditionalComment.Text = MyBase.FileObject.ExtendedComment

        Me.svDataViewer.cbX.SelectedEntry = InitialColumnNameX
        Me.svDataViewer.cbY.SelectedEntry = InitialColumnNameY
    End Sub

#End Region

#Region "Thread-Safe-Setters"
    Private Delegate Sub StringDelegate(Text As String)

    Public Sub SetViewerThreadSafe(ByRef SpectroscopyTable As cSpectroscopyTable)
        With Me.svDataViewer
            If .InvokeRequired Then
                .Invoke(New _ThreadSafeSpectroscopyTableDelegate(AddressOf SetViewerThreadSafe), SpectroscopyTable)
            Else
                .SetSinglePreviewImage(SpectroscopyTable)
            End If
        End With
    End Sub

    Public Sub SetSpectrumPropertiesThreadSafe(ByRef SpectroscopyTable As cSpectroscopyTable)
        With Me.pgSpectrumProperies
            If .InvokeRequired Then
                .Invoke(New _ThreadSafeSpectroscopyTableDelegate(AddressOf SetSpectrumPropertiesThreadSafe), SpectroscopyTable)
            Else
                .SelectedObject = SpectroscopyTable
            End If
        End With
    End Sub

    Public Sub SetSpectraCommentThreadSafe(Text As String)
        With Me.txtSpectraComment
            If .InvokeRequired Then
                .Invoke(New StringDelegate(AddressOf SetSpectraCommentThreadSafe), Text)
            Else
                .Text = Text
            End If
        End With
    End Sub

    Public Sub SetAdditionalCommentThreadSafe(Text As String)
        With Me.txtAdditionalComment
            If .InvokeRequired Then
                .Invoke(New StringDelegate(AddressOf SetAdditionalCommentThreadSafe), Text)
            Else
                .Text = Text
            End If
        End With
    End Sub

    Public Sub SetDataTableThreadSafe(ByRef SpectroscopyTable As cSpectroscopyTable)
        With Me.dtSpectroscopyTable
            If .InvokeRequired Then
                .Invoke(New _ThreadSafeSpectroscopyTableDelegate(AddressOf SetDataTableThreadSafe), SpectroscopyTable)
            Else
                .SetSpectroscopyTable(SpectroscopyTable)
            End If
        End With
    End Sub
#End Region

#Region "Selected Columns"

    Private InitialColumnNameX As String
    Private InitialColumnNameY As String

    ''' <summary>
    ''' Sets the initial column-names that get displayed in the preview-image.
    ''' </summary>
    Public Sub SetInitialColumnSelection(ByVal ColumnNameX As String,
                                         ByVal ColumnNameY As String)
        Me.InitialColumnNameX = ColumnNameX
        Me.InitialColumnNameY = ColumnNameY
    End Sub

#End Region

#Region "Save additional comment"

    ''' <summary>
    ''' A click saves the additional comment from the textbox.
    ''' </summary>
    Private Sub btnSaveAdditionalComment_Click(sender As Object, e As EventArgs) Handles btnSaveAdditionalComment.Click
        MyBase._FileObject.SetExtendedComment(Me.txtAdditionalComment.Text)
    End Sub

#End Region

#Region "Tool - Coordinate Collection"

    ''' <summary>
    ''' Collection used to store the point-information.
    ''' </summary>
    Private lDataCollectorPointList As New List(Of ZedGraph.PointPair)

    ''' <summary>
    ''' Collection used to store the labels
    ''' </summary>
    Private lDataCollectorPointListLabel As New List(Of String)

    ''' <summary>
    ''' Function, that prints the collected data points.
    ''' </summary>
    Private Sub PrintDataCollection()

        ' Create string-builder
        Dim SB As New System.Text.StringBuilder

        ' Add entries
        For i As Integer = 0 To Me.lDataCollectorPointList.Count - 1 Step 1
            If Me.ckbDataCollector_ShowLabels.Checked Then
                SB.Append(Me.lDataCollectorPointListLabel(i))
                SB.Append(ValueSeparator)
            End If
            If Me.ckbDataCollector_ShowXPoints.Checked Then
                SB.Append(Me.lDataCollectorPointList(i).X.ToString("E3"))
                SB.Append(ValueSeparator)
            End If
            If Me.ckbDataCollector_ShowYPoints.Checked Then
                SB.Append(Me.lDataCollectorPointList(i).Y.ToString("E3"))
                SB.Append(ValueSeparator)
            End If
            If Not Me.rdbDataCollector_ShowHorizonally.Checked Then SB.Append(vbNewLine)
        Next

        ' Write output
        Me.txtDataCollector_Output.Text = SB.ToString

    End Sub

    ''' <summary>
    ''' Saves a new data-point
    ''' </summary>
    Private Sub DataPlotPointClicked(XValue As Double, YValue As Double, CurveLabel As String) Handles svDataViewer.PointSelectionChanged_DataPoint
        Me.lDataCollectorPointList.Add(New ZedGraph.PointPair(XValue, YValue))
        Me.lDataCollectorPointListLabel.Add(CurveLabel)
        Me.PrintDataCollection()
    End Sub

    ''' <summary>
    ''' Save settings, and update the point-list
    ''' </summary>
    Private Sub ckbDataCollector_ShowXPoints_CheckedChanged(sender As Object, e As EventArgs) Handles ckbDataCollector_ShowXPoints.CheckedChanged, ckbDataCollector_ShowYPoints.CheckedChanged, ckbDataCollector_ShowLabels.CheckedChanged
        If Not Me.bReady Then Return

        My.Settings.DataBrowser_DataCollectorOutput_ShowXCoord = Me.ckbDataCollector_ShowXPoints.Checked
        My.Settings.DataBrowser_DataCollectorOutput_ShowYCoord = Me.ckbDataCollector_ShowYPoints.Checked
        My.Settings.DataBrowser_DataCollectorOutput_ShowLabels = Me.ckbDataCollector_ShowLabels.Checked
        Me.PrintDataCollection()
    End Sub

    ''' <summary>
    ''' Save settings, and update the point-list
    ''' </summary>
    Private Sub rdbDataCollector_ShowHorizonally_CheckedChanged(sender As Object, e As EventArgs) Handles rdbDataCollector_ShowHorizonally.CheckedChanged, rdbDataCollector_ShowVertically.CheckedChanged
        My.Settings.DataBrowser_DataCollectorOutput_PrintHorizontally = Me.rdbDataCollector_ShowHorizonally.Checked
        Me.PrintDataCollection()
    End Sub

    ''' <summary>
    ''' Clear points
    ''' </summary>
    Private Sub btnDataSelector_ClearPoints_Click(sender As Object, e As EventArgs) Handles btnDataSelector_ClearPoints.Click
        Me.lDataCollectorPointList.Clear()
        Me.lDataCollectorPointListLabel.Clear()
        Me.PrintDataCollection()
    End Sub

    ''' <summary>
    ''' Select all.
    ''' </summary>
    Private Sub SelectAllDataCollector_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtDataCollector_Output.KeyDown
        If e.Control AndAlso e.KeyValue = 65 Then
            Me.txtDataCollector_Output.SelectAll()
        End If
    End Sub

    ''' <summary>
    ''' Change the value-separator
    ''' </summary>
    Private Sub txtDataSelector_ValueSeparator_TextChanged(sender As Object, e As EventArgs) Handles txtDataSelector_ValueSeparator.TextChanged
        If Not Me.bReady Then Return
        ValueSeparator = Me.txtDataSelector_ValueSeparator.Text
        My.Settings.DataBrowser_DataCollectorOutput_ValueSeparator = ValueSeparator
        Me.PrintDataCollection()
    End Sub

    ''' <summary>
    ''' Decide to use a tab as value-separator.
    ''' </summary>
    Private Sub ckbDataCollector_UseTab_CheckedChanged(sender As Object, e As EventArgs) Handles ckbDataCollector_UseTab.CheckedChanged
        If Not bReady Then Return
        If Me.ckbDataCollector_UseTab.Checked Then
            ValueSeparator = vbTab
            My.Settings.DataBrowser_DataCollectorOutput_ValueSeparator = vbTab
            Me.txtDataSelector_ValueSeparator.Enabled = False
        Else
            Me.txtDataSelector_ValueSeparator.Enabled = True
        End If
        Me.PrintDataCollection()
    End Sub

#End Region

End Class