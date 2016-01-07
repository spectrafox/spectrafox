Public Class GroupBoxCheckable
    Inherits GroupBox

    Private WithEvents m_CheckBox As CheckBox

    ' Add the CheckBox to the control.
    Public Sub New()
        MyBase.New()

        m_CheckBox = New CheckBox
        m_CheckBox.Location = New Point(8, 0)
        Me.Text = "CheckGroup"

        Me.Controls.Add(m_CheckBox)
    End Sub

    ' Keep the CheckBox text synced with our text.
    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal Value As String)
            MyBase.Text = Value
            m_CheckBox.Text = Value

            Dim gr As Graphics = Me.CreateGraphics
            Dim s As SizeF = gr.MeasureString(MyBase.Text, Me.Font)
            m_CheckBox.Size = New Size(CInt(s.Width + 20), CInt(s.Height + 3))
        End Set
    End Property

    ' Delegate to CheckBox.Checked.
    Public Property Checked() As Boolean
        Get
            Return m_CheckBox.Checked
        End Get
        Set(ByVal Value As Boolean)
            m_CheckBox.Checked = Value
        End Set
    End Property

    ' Enable/disable contained controls.
    Private Sub EnableDisableControls()
        For Each ctl As Control In Me.Controls
            If Not (ctl Is m_CheckBox) Then
                Try
                    ctl.Enabled = m_CheckBox.Checked
                Catch ex As Exception
                End Try
            End If
        Next ctl
    End Sub

    ''' <summary>
    ''' Event fired, if the check-state of the groupbox changes.
    ''' </summary>
    Public Event CheckChanged()

    ' Enable/disable contained controls.
    Private Sub m_CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_CheckBox.CheckedChanged
        EnableDisableControls()
        RaiseEvent CheckChanged()
    End Sub

    ' Enable/disable contained controls.
    ' We do this here to set editability
    ' when the control is first loaded.
    Private Sub m_CheckBox_Layout(ByVal sender As Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles m_CheckBox.Layout
        EnableDisableControls()
    End Sub
End Class