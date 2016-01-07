Public Class cFitSettingSubParameter_SubGapPeakPanel
    Inherits cFitSettingSubParameterPanel_4

    ''' <summary>
    ''' Local reference to the SubGapPeak.
    ''' </summary>
    Public oSubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak

    ''' <summary>
    ''' Ready
    ''' </summary>
    Public bReady As Boolean = False

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New(ByRef SubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak,
                   ByRef AllFitParameters As cFitParameterGroupGroup)
        MyBase.New(SubGapPeak.XCenter, SubGapPeak.Amplitude, SubGapPeak.Width, SubGapPeak.PosNegRatio, AllFitParameters)

        ' Save reference to SGP.
        Me.oSubGapPeak = SubGapPeak

        ' Komponenten initialisieren
        Me.InitializeComponent()

        ' Combobox befüllen
        With Me.cbSGPParentDOS
            .DisplayMember = "Value"
            .ValueMember = "Key"
            .Items.Add(New KeyValuePair(Of cFitFunction_TipSampleConvolutionBase.DOSTypes, String)(cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip, cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip.ToString))
            .Items.Add(New KeyValuePair(Of cFitFunction_TipSampleConvolutionBase.DOSTypes, String)(cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample, cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample.ToString))
        End With

        Select Case Me.oSubGapPeak.SubGapPeakParentDOS
            Case cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip
                Me.cbSGPParentDOS.SelectedIndex = 0
            Case cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample
                Me.cbSGPParentDOS.SelectedIndex = 1
        End Select

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Sub that resets the FitParameterGroup to lock parameters together.
    ''' </summary>
    Public Function SetFitParameterGroups(ByRef FitParameterGroups As cFitParameterGroupGroup) As Boolean
        For Each C As Control In Me.Controls
            If C.GetType Is GetType(mFitParameter) Then
                DirectCast(C, mFitParameter).SetFitParameterGroups(FitParameterGroups)
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Changes the DOS to which the SGP is belonging to.
    ''' </summary>
    Private Sub cbSGPParentDOS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSGPParentDOS.SelectedIndexChanged
        If Not Me.bReady Then Return
        Select Case Me.cbSGPParentDOS.SelectedIndex
            Case 0
                Me.oSubGapPeak.SubGapPeakParentDOS = cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip
            Case 1
                Me.oSubGapPeak.SubGapPeakParentDOS = cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample
        End Select
        RaiseEvent ParentDOSChanged(Me.oSubGapPeak, Me.oSubGapPeak.SubGapPeakParentDOS)
    End Sub

    ''' <summary>
    ''' Sets the parent DOS of the SGP.
    ''' </summary>
    Public Sub SetParentDOS(ByVal ParentDOS As cFitFunction_TipSampleConvolutionBase.DOSTypes, Optional ByVal SendParentDOSChangedEvent As Boolean = True)
        If Not SendParentDOSChangedEvent Then Me.bReady = False
        Select Case ParentDOS
            Case cFitFunction_TipSampleConvolutionBase.DOSTypes.Tip
                Me.cbSGPParentDOS.SelectedIndex = 0
            Case cFitFunction_TipSampleConvolutionBase.DOSTypes.Sample
                Me.cbSGPParentDOS.SelectedIndex = 1
        End Select
        If Not SendParentDOSChangedEvent Then Me.bReady = True
        Me.oSubGapPeak.SubGapPeakParentDOS = ParentDOS
    End Sub

    ''' <summary>
    ''' Fired if the Parent DOS changed.
    ''' </summary>
    Public Event ParentDOSChanged(ByRef SubGapPeak As iFitFunction_SubGapPeaks.SubGapPeak, ByVal ParentDOS As cFitFunction_TipSampleConvolutionBase.DOSTypes)

    ''' <summary>
    ''' Change the activation state of the SGP.
    ''' </summary>
    Private Sub ckbSGPActive_CheckedChanged(sender As Object, e As EventArgs) Handles ckbSGPActive.CheckedChanged
        If Not Me.bReady Then Return
        Me.oSubGapPeak.SubGapPeakEnabled = Me.ckbSGPActive.Checked
        RaiseEvent PeakEnabledChanged(Me.oSubGapPeak, Me.ckbSGPActive.Checked)
    End Sub

    ''' <summary>
    ''' Fired if the activation of the SGP changed.
    ''' </summary>
    Public Event PeakEnabledChanged(ByRef Peak As iFitFunction_SubGapPeaks.SubGapPeak, ByVal Enabled As Boolean)

    ''' <summary>
    ''' Sets the enabled-state of the SGP.
    ''' </summary>
    Public Sub SetSGPEnabled(ByVal Enabled As Boolean, Optional ByVal SendEnabledChangedEvent As Boolean = True)
        If Not SendEnabledChangedEvent Then Me.bReady = False
        Me.ckbSGPActive.Checked = Enabled
        If Not SendEnabledChangedEvent Then Me.bReady = True
        Me.oSubGapPeak.SubGapPeakEnabled = Enabled
    End Sub

    '###########
    ' DESIGNER

    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ckbSGPActive As System.Windows.Forms.CheckBox
    Friend WithEvents cbSGPParentDOS As System.Windows.Forms.ComboBox

    Private Sub InitializeComponent()
        Me.cbSGPParentDOS = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ckbSGPActive = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cbSGPParentDOS
        '
        Me.cbSGPParentDOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSGPParentDOS.FormattingEnabled = True
        Me.cbSGPParentDOS.Location = New System.Drawing.Point(399, 64)
        Me.cbSGPParentDOS.Name = "cbSGPParentDOS"
        Me.cbSGPParentDOS.Size = New System.Drawing.Size(121, 21)
        Me.cbSGPParentDOS.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(155, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(238, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "sub gap peak belongs to density of states (DOS):"
        '
        'ckbSGPActive
        '
        Me.ckbSGPActive.AutoSize = True
        Me.ckbSGPActive.Checked = True
        Me.ckbSGPActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ckbSGPActive.Location = New System.Drawing.Point(14, 66)
        Me.ckbSGPActive.Name = "ckbSGPActive"
        Me.ckbSGPActive.Size = New System.Drawing.Size(123, 17)
        Me.ckbSGPActive.TabIndex = 5
        Me.ckbSGPActive.Text = "sub gap peak active"
        Me.ckbSGPActive.UseVisualStyleBackColor = True
        '
        'cFitSettingSubParameter_SubGapPeakPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.ckbSGPActive)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbSGPParentDOS)
        Me.Name = "cFitSettingSubParameter_SubGapPeakPanel"
        Me.Size = New System.Drawing.Size(721, 91)
        Me.Controls.SetChildIndex(Me.cbSGPParentDOS, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.ckbSGPActive, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class
