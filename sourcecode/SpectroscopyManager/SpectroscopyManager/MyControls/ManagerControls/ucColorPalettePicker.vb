Public Class ucColorPalettePicker

    Private bReady As Boolean = False

    ''' <summary>
    ''' Event that is fired, if the selected color-scheme changed.
    ''' </summary>
    Public Event SelectedColorSchemaChanged(NewColorSchema As cColorScheme)

    Private _SelectedColorScheme As cColorScheme = cColorScheme.Gray
    ''' <summary>
    ''' Currently selected colorscheme.
    ''' Thread-safe! Does not access the interface.
    ''' </summary>
    Public ReadOnly Property SelectedColorScheme As cColorScheme
        Get
            Return Me._SelectedColorScheme
        End Get
    End Property

    ''' <summary>
    ''' Add Colors to the Combobox
    ''' </summary>
    Private Sub ucLoad(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ' Last ColorScheme saved in the settings.
        Dim LastSettingsColorScheme As cColorScheme = Nothing
        If My.Settings.LastColorScheme <> String.Empty Then
            LastSettingsColorScheme = cColorScheme.ImportFromXML(My.Settings.LastColorScheme)
        End If

        ' Load all available color-schemes.
        Dim ColorSchemes As List(Of cColorScheme) = cColorScheme.AvailableColorSchemes

        ' Populate Colorschema Combobox.
        With Me.cbColorPicker
            ' Add Images
            .Items.Clear()
            .DisplayMember = "SchemeName"
            .Items.AddRange(ColorSchemes.ToArray)
            If LastSettingsColorScheme Is Nothing Then
                .SelectedIndex = 0
            Else
                Dim iLastColorScheme As Integer = ColorSchemes.IndexOf(LastSettingsColorScheme)
                If iLastColorScheme > 0 Then
                    .SelectedIndex = iLastColorScheme
                Else
                    .Items.Add(LastSettingsColorScheme)
                    .SelectedIndex = .Items.Count - 1
                End If
            End If
        End With

        Me._UpdateSelectedColorScheme()

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Returns the selected ColorScheme.
    ''' Depracted: Use <code>.SelectedColorScheme</code> instead.
    ''' </summary>
    Public Function GetSelectedColorScheme() As cColorScheme
        Return Me._SelectedColorScheme
    End Function

    ''' <summary>
    ''' Reads the combobox, and extracts the selected color-scheme if possible.
    ''' </summary>
    Private Sub _UpdateSelectedColorScheme()

        Dim Color As cColorScheme = Nothing

        ' Try to get a color-scheme
        If Me.cbColorPicker IsNot Nothing AndAlso Me.cbColorPicker.SelectedIndex > 0 Then
            Color = TryCast(Me.cbColorPicker.SelectedItem, cColorScheme)
        End If

        ' Set to default, if not color could be loaded.
        If Color Is Nothing Then
            Color = cColorScheme.Gray
        End If

        ' Set the new selection.
        Me._SelectedColorScheme = Color

        ' Now create the preview image plot.
        If Me.pbSchemePreview.Height > 0 AndAlso Me.pbSchemePreview.Width > 0 Then
            Me.pbSchemePreview.Image = Me._SelectedColorScheme.GetPreviewImage(Me.pbSchemePreview.Height, Me.pbSchemePreview.Width, cColorScheme.ColorSchemePreviewDirections.Horizontal)
        End If

    End Sub

    ''' <summary>
    ''' Change the selected colorscheme if the selection changes.
    ''' </summary>
    Private Sub cbColorPalettes_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbColorPicker.SelectionChangeCommitted

        If Not Me.bReady Then Return

        ' Set the new color-scheme.
        Me._UpdateSelectedColorScheme()

        ' save the selected colorscheme as XML to the settings.
        My.Settings.LastColorScheme = Me._SelectedColorScheme.ExportToXML()

        ' Raise the event, that the selected ColorScheme changed.
        RaiseEvent SelectedColorSchemaChanged(Me._SelectedColorScheme)

    End Sub

#Region "Copy ColorScheme preview to the clipboard."

    ''' <summary>
    ''' Copys the image to the clipboard.
    ''' </summary>
    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        Clipboard.SetImage(Me.pbSchemePreview.Image)
    End Sub

#End Region
End Class
