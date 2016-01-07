Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' Summary description for frmFeedback.
''' </summary>
Public Class wPluginDetails
    Inherits System.Windows.Forms.Form

    Private butOk As System.Windows.Forms.Button
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private groupBox2 As System.Windows.Forms.GroupBox
    Private lblPluginAuthor As System.Windows.Forms.Label
    Private lblPluginVersion As System.Windows.Forms.Label
    Private lblPluginName As System.Windows.Forms.Label
    Private lblPluginDesc As System.Windows.Forms.Label
    Private lblFeedback As System.Windows.Forms.Label

    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.Container = Nothing

    Public Sub New()
        MyBase.New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()
        '
        ' TODO: Add any constructor code after InitializeComponent call
        '
    End Sub

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If (Not (components) Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
#Region "Windows Form Designer generated code"

    Private Sub InitializeComponent()
        Me.butOk = New System.Windows.Forms.Button
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.lblPluginDesc = New System.Windows.Forms.Label
        Me.groupBox2 = New System.Windows.Forms.GroupBox
        Me.lblPluginAuthor = New System.Windows.Forms.Label
        Me.lblPluginVersion = New System.Windows.Forms.Label
        Me.lblPluginName = New System.Windows.Forms.Label
        Me.lblFeedback = New System.Windows.Forms.Label
        Me.groupBox1.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' butOk
        ' 
        Me.butOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.butOk.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.butOk.Location = New System.Drawing.Point(128, 200)
        Me.butOk.Name = "butOk"
        Me.butOk.Size = New System.Drawing.Size(64, 24)
        Me.butOk.TabIndex = 0
        Me.butOk.Text = "&OK"
        AddHandler Me.butOk.Click, AddressOf Me.butOk_Click
        ' 
        ' groupBox1
        ' 
        Me.groupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox1.Controls.Add(Me.lblFeedback)
        Me.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupBox1.Location = New System.Drawing.Point(8, 120)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(312, 72)
        Me.groupBox1.TabIndex = 1
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Feedback:"
        ' 
        ' lblPluginDesc
        ' 
        Me.lblPluginDesc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPluginDesc.Location = New System.Drawing.Point(16, 72)
        Me.lblPluginDesc.Name = "lblPluginDesc"
        Me.lblPluginDesc.Size = New System.Drawing.Size(288, 32)
        Me.lblPluginDesc.TabIndex = 0
        ' 
        ' groupBox2
        ' 
        Me.groupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBox2.Controls.Add(Me.lblPluginAuthor)
        Me.groupBox2.Controls.Add(Me.lblPluginVersion)
        Me.groupBox2.Controls.Add(Me.lblPluginName)
        Me.groupBox2.Controls.Add(Me.lblPluginDesc)
        Me.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupBox2.Location = New System.Drawing.Point(8, 8)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(312, 112)
        Me.groupBox2.TabIndex = 2
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Plugin Info:"
        ' 
        ' lblPluginAuthor
        ' 
        Me.lblPluginAuthor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPluginAuthor.Location = New System.Drawing.Point(8, 48)
        Me.lblPluginAuthor.Name = "lblPluginAuthor"
        Me.lblPluginAuthor.Size = New System.Drawing.Size(296, 16)
        Me.lblPluginAuthor.TabIndex = 5
        Me.lblPluginAuthor.Text = "By: <Author\'s Name>"
        ' 
        ' lblPluginVersion
        ' 
        Me.lblPluginVersion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPluginVersion.Location = New System.Drawing.Point(8, 32)
        Me.lblPluginVersion.Name = "lblPluginVersion"
        Me.lblPluginVersion.Size = New System.Drawing.Size(296, 16)
        Me.lblPluginVersion.TabIndex = 4
        Me.lblPluginVersion.Text = "(<Version>)"
        ' 
        ' lblPluginName
        ' 
        Me.lblPluginName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPluginName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPluginName.Location = New System.Drawing.Point(8, 16)
        Me.lblPluginName.Name = "lblPluginName"
        Me.lblPluginName.Size = New System.Drawing.Size(296, 16)
        Me.lblPluginName.TabIndex = 3
        Me.lblPluginName.Text = "<Plugin Name Here>"
        ' 
        ' lblFeedback
        ' 
        Me.lblFeedback.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFeedback.Location = New System.Drawing.Point(8, 16)
        Me.lblFeedback.Name = "lblFeedback"
        Me.lblFeedback.Size = New System.Drawing.Size(296, 48)
        Me.lblFeedback.TabIndex = 0
        ' 
        ' frmFeedback
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(328, 230)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.butOk)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFeedback"
        Me.Text = "Plugin Feedback:"
        AddHandler Load, AddressOf Me.frmFeedback_Load
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
#End Region

    Private Sub frmFeedback_Load(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub butOk_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Public Property PluginAuthor As String
        Get
            Return Me.lblPluginAuthor.Text
        End Get
        Set(value As String)
            Me.lblPluginAuthor.Text = value
        End Set
    End Property

    Public Property PluginDesc As String
        Get
            Return Me.lblPluginDesc.Text
        End Get
        Set(value As String)
            Me.lblPluginDesc.Text = value
        End Set
    End Property

    Public Property PluginName As String
        Get
            Return Me.lblPluginName.Text
        End Get
        Set(value As String)
            Me.lblPluginName.Text = value
        End Set
    End Property

    Public Property PluginVersion As String
        Get
            Return Me.lblPluginVersion.Text
        End Get
        Set(value As String)
            Me.lblPluginVersion.Text = value
        End Set
    End Property

    Public Property Feedback As String
        Get
            Return Me.lblFeedback.Text
        End Get
        Set(value As String)
            Me.lblFeedback.Text = value
        End Set
    End Property
End Class