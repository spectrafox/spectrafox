Public Class wComputingCloudDetails
    Inherits wFormBase

    Private ComputingCloud As cComputingCloud

#Region "Show Dialog Shadow"

    ''' <summary>
    ''' Show-Dialog
    ''' </summary>
    Public Shadows Sub ShowDialog(ByRef ComputingCloud As cComputingCloud)
        Me.ComputingCloud = ComputingCloud
        MyBase.ShowDialog()
    End Sub

    ''' <summary>
    ''' Show-Dialog
    ''' </summary>
    Public Shadows Sub Show(ByRef ComputingCloud As cComputingCloud)
        Me.ComputingCloud = ComputingCloud
        MyBase.Show()
    End Sub

#End Region

#Region "Constructor"
    ''' <summary>
    ''' Constructor of the window.
    ''' </summary>
    Private Sub wComputingCloudSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.dgvCloudMembers.Rows.Clear()
        For Each CM As cComputingCloud.CloudMember In ComputingCloud.CloudMembers.Values
            Dim i As Integer = Me.dgvCloudMembers.Rows.Add
            With Me.dgvCloudMembers.Rows(i)
                .Cells(Me.colClientIP.Index).Value = CM.IP.ToString
                .Cells(Me.colVersion.Index).Value = CM.ClientVersion
                .Cells(Me.colAvailableThreads.Index).Value = CM.ThreadCountAvailable
                .Cells(Me.colCPUName.Index).Value = CM.CPUName
                .Cells(Me.colComputerName.Index).Value = CM.ComputerName
            End With
        Next
    End Sub
#End Region

    ''' <summary>
    ''' actions of click
    ''' </summary>
    Private Sub dgvCloudMembers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCloudMembers.CellContentClick
        ' Get clicked cloud member
        If e.ColumnIndex < 0 Then Return
        If e.RowIndex < 0 Or e.RowIndex > Me.dgvCloudMembers.Rows.Count - 1 Then Return

        ' Get Computing Cloud Member
        Dim CloudMemberIP As String = Convert.ToString(Me.dgvCloudMembers.Rows(e.RowIndex).Cells(Me.colClientIP.Index).Value)

        ' Search Cloud Member
        If Not Me.ComputingCloud.CloudMembers.ContainsKey(CloudMemberIP) Then Return

        Dim SelectedCloudMember As cComputingCloud.CloudMember = Me.ComputingCloud.CloudMembers(CloudMemberIP)

        ' Ask for idle
        cComputingCloud.AreYouIdle(SelectedCloudMember)

    End Sub
End Class