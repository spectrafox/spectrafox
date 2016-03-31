Public Class wFitParallelizationSettings
    Inherits wFormBase

    Private bReady As Boolean = True

    Private _MaxThreadCount As Integer = My.Settings.Fitting_MaxParallelization
    ''' <summary>
    ''' Current settings of the max number of threads.
    ''' </summary>
    Public Property MaxThreadCount As Integer
        Get
            Return _MaxThreadCount
        End Get
        Set(value As Integer)
            Me._MaxThreadCount = value
            My.Settings.Fitting_MaxParallelization = value
            cGlobal.SaveSettings()
        End Set
    End Property

    ''' <summary>
    ''' Fill the combobox.
    ''' </summary>
    Private Sub wFitParallelizationSettings_Load(sender As Object, e As EventArgs) Handles Me.Load

        With Me.cboThreadCount
            With .Items
                .Add(My.Resources.rFitBase.Parallelization_UseAllProcessors)
                For i As Integer = 1 To Environment.ProcessorCount Step 1
                    .Add(String.Format(My.Resources.rFitBase.Parallelization_Processor, i.ToString("N0")))
                Next
            End With
            If Me.MaxThreadCount >= .Items.Count Then
                .SelectedIndex = 0
                Me.MaxThreadCount = -1
            ElseIf Me.MaxThreadCount < 0 Then
                .SelectedIndex = 0
            Else
                .SelectedIndex = Me.MaxThreadCount
            End If
        End With

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Change the settings
    ''' </summary>
    Private Sub cboThreadCount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboThreadCount.SelectedIndexChanged
        If Not Me.bReady Then Return

        If Me.cboThreadCount.SelectedIndex > 0 Then
            Me.MaxThreadCount = Me.cboThreadCount.SelectedIndex
        Else
            Me.MaxThreadCount = -1
        End If
    End Sub

    ''' <summary>
    ''' Close window
    ''' </summary>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class