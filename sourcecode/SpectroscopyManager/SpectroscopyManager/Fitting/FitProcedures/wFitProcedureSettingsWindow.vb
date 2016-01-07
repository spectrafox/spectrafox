Public Class wFitProcedureSettingsWindow
    Inherits wFormBase

    ''' <summary>
    ''' Keeps the reference to the Fit-Procedure Settings-Panel
    ''' </summary>
    Private _FitProcedureSettingsPanel As cFitProcedureSettingsPanel

    ''' <summary>
    ''' To show this setting-window, we have to hand over a settings-panel
    ''' of the fit-procedure, which will be shown in the top left corner.
    ''' </summary>
    Public Shadows Function ShowDialog(FitProcedureSettingsPanel As cFitProcedureSettingsPanel) As iFitProcedureSettings
        Me._FitProcedureSettingsPanel = FitProcedureSettingsPanel
        MyBase.ShowDialog()
        Return Me._FitProcedureSettingsPanel.SelectedFitSettings
    End Function

    ''' <summary>
    ''' USE SHOWDIALOG INSTEAD!!!
    ''' </summary>
    Public Shadows Function Show(FitProcedureSettingsPanel As cFitProcedureSettingsPanel) As iFitProcedureSettings
        Me._FitProcedureSettingsPanel = FitProcedureSettingsPanel
        MyBase.ShowDialog()
        Return Me._FitProcedureSettingsPanel.SelectedFitSettings
    End Function

    ''' <summary>
    ''' Show the settings-panel
    ''' </summary>
    Private Sub wFitProcedureSettingsWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Add(Me._FitProcedureSettingsPanel)
        Me._FitProcedureSettingsPanel.Location = New Point(0, 0)

        ' Set the width of the window to keep symmetric space around the ok-button
        Me.Width = Me._FitProcedureSettingsPanel.Width + (Me.Width - Me.btnClose.Location.X) + (Me.Width - Me.btnClose.Location.X - Me.btnClose.Width)
        ' Set the height to the settings-panel.
        Me.Height = Me._FitProcedureSettingsPanel.Height + 45

    End Sub

    ''' <summary>
    ''' Close the settings-window
    ''' </summary>
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class