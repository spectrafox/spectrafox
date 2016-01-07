Public Class wFormBase
    Inherits Form

    ''' <summary>
    ''' Sets the basic properties of all windows,
    ''' such as background-color, etc..
    ''' </summary>
    Public Sub New()
        Me.BackColor = Color.LightBlue
        Me.Icon = cProgrammDeployment.ProgramIcon
    End Sub

    ''' <summary>
    ''' Sets/Gets the title of the current window.
    ''' </summary>
    Public Overrides Property Text As String
        Get
            If MyBase.Text.Length > 0 Then
                Return MyBase.Text.Replace(My.Resources._FormBase_WindowTitleExtension.Replace("%v", cProgrammDeployment.GetProgramVersionString), "")
            Else
                Return MyBase.Text
            End If
        End Get
        Set(value As String)
            MyBase.Text = value & My.Resources._FormBase_WindowTitleExtension.Replace("%v", cProgrammDeployment.GetProgramVersionString)
        End Set
    End Property
End Class
