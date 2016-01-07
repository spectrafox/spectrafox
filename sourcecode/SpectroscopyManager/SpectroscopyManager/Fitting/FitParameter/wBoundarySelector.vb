Public Class wBoundarySelector
    Inherits wFormBase

    Private _UpperBoundary As Double
    Private _LowerBoundary As Double

    Public Property UpperBoundary As Double
        Get
            Return _UpperBoundary
        End Get
        Set(value As Double)
            If Double.IsNaN(value) Then
                Me.txtUpperBoundary.Enabled = False
                Me.ckbActivateUpper.Checked = False
                Me.txtUpperBoundary.SetValue(0)
            Else
                Me.ckbActivateUpper.Checked = True
                Me.txtUpperBoundary.Enabled = True
                Me.txtUpperBoundary.SetValue(value)
            End If
            Me._UpperBoundary = value
        End Set
    End Property

    Public Property LowerBoundary As Double
        Get
            Return _LowerBoundary
        End Get
        Set(value As Double)
            If Double.IsNaN(value) Then
                Me.txtLowerBoundary.Enabled = False
                Me.ckbActivateLower.Checked = False
                Me.txtLowerBoundary.SetValue(0)
            Else
                Me.ckbActivateLower.Checked = True
                Me.txtLowerBoundary.Enabled = True
                Me.txtLowerBoundary.SetValue(value)
            End If
            Me._LowerBoundary = value
        End Set
    End Property

    ''' <summary>
    ''' Reset
    ''' </summary>
    Private Sub btnResetLowerBoundary_Click(sender As Object, e As EventArgs)
        Me.LowerBoundary = Double.MinValue
    End Sub

    ''' <summary>
    ''' Reset
    ''' </summary>
    Private Sub btnResetUpperBoundary_Click(sender As Object, e As EventArgs)
        Me.UpperBoundary = Double.MaxValue
    End Sub

    Private Sub ckbActivateUpper_CheckedChanged(sender As Object, e As EventArgs) Handles ckbActivateUpper.CheckedChanged
        If Me.ckbActivateUpper.Checked Then
            Me.UpperBoundary = 0
        Else
            Me.UpperBoundary = Double.NaN
        End If
    End Sub

    Private Sub ckbActivateLower_CheckedChanged(sender As Object, e As EventArgs) Handles ckbActivateLower.CheckedChanged
        If Me.ckbActivateLower.Checked Then
            Me.LowerBoundary = 0
        Else
            Me.LowerBoundary = Double.NaN
        End If
    End Sub

    Private Sub txtUpperBoundary_TextChanged(ByRef T As NumericTextbox) Handles txtUpperBoundary.ValidValueChanged
        Me._UpperBoundary = T.DecimalValue
    End Sub

    Private Sub txtLowerBoundary_TextChanged(ByRef T As NumericTextbox) Handles txtLowerBoundary.ValidValueChanged
        Me._LowerBoundary = T.DecimalValue
    End Sub
End Class