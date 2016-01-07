Imports System.ComponentModel

Public Class ImageComboBox
    Inherits ComboBox

#Region " Fields "

    ''' <summary>
    ''' Boolean value indicating whether the specified image for each ImageComboBoxItem of the ImageComboBox is displayed beside the item text.
    ''' </summary>
    Private _ShowImages As Boolean

    ''' <summary>
    ''' Collection that stores all the ImageComboBoxItems for the ImageComboBox.
    ''' </summary>
    Private _Items As ImageComboBoxItemCollection

    ''' <summary>
    ''' Enumeration value corresponding to the selected alignment to align the image of each ImageComboList item in the ImageComboBox.
    ''' </summary>
    Private _ImageAlignment As ImageAlignment = ImageAlignment.Left

#End Region

#Region " Enumerations "

    ''' <summary>
    ''' Represents an enumeration of valid text alignments for items in the control.
    ''' </summary>
    Public Enum ImageAlignment
        Left = 0
        Right = 1
    End Enum

#End Region

#Region " Constructors "

    ''' <summary>
    ''' Creates an instance of the ImageComboBox.
    ''' </summary>
    Public Sub New()
        Me.DrawMode = DrawMode.OwnerDrawFixed
        Me.DropDownStyle = ComboBoxStyle.DropDownList
        _ShowImages = True
        _ImageAlignment = ImageAlignment.Left
        _Items = New ImageComboBoxItemCollection(Me)
    End Sub

#End Region

#Region " Properties "

    ''' <summary>
    ''' Returns the items in the ImageComboBoxItemCollection to be used in the ImageComboBox.
    ''' </summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Overloads ReadOnly Property Items() As ImageComboBoxItemCollection
        Get
            Return _Items
        End Get
    End Property

    ''' <summary>
    ''' The original items from the ImageComboBox that will never been seen.
    ''' </summary>
    Private ReadOnly Property baseItems() As ObjectCollection
        Get
            Return MyBase.Items
        End Get
    End Property

    ''' <summary>
    ''' Returns the currently selected item from the ImageComboBox.
    ''' </summary>
    Public Overloads Property SelectedItem() As ImageComboBoxItem
        Get
            Return DirectCast(MyBase.SelectedItem, ImageComboBoxItem)
        End Get
        Set(ByVal value As ImageComboBoxItem)
            MyBase.SelectedItem = value
        End Set
    End Property

    ''' <summary>
    ''' Sets the DrawMode of the ImageComboBox.
    ''' </summary>
    ''' <value>DrawMode.OwnerDrawFixed</value>
    ''' <returns>DrawMode.OwnerDrawFixed</returns>
    ''' <remarks>Hidden property. Needs to stay 'DrawMode.OwnerDrawFixed' to properly draw items.</remarks>
    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overloads Property DrawMode() As DrawMode
        Get
            Return DrawMode.OwnerDrawFixed
        End Get
        Set(ByVal value As DrawMode)
            MyBase.DrawMode = DrawMode.OwnerDrawFixed
        End Set
    End Property

    ''' <summary>
    ''' Sets the ComboBoxStyle of the ImageComboBox
    ''' </summary>
    ''' <value>ComboBoxStyle.DropDownList</value>
    ''' <returns>ComboBoxStyle.DropDownList</returns>
    ''' <remarks>Hidden Property. Needs to stay 'ComboBoxStyle.DropDownList' so that the user can't enter text into a ComboBox that has this property set to 'ComboBoxStyle.DropDown'.</remarks>
    <Browsable(False)> _
    <EditorBrowsable(EditorBrowsableState.Never)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    <DefaultValue(ComboBoxStyle.DropDownList)> _
    Public Overloads Property DropDownStyle() As ComboBoxStyle
        Get
            Return ComboBoxStyle.DropDownList
        End Get
        Set(ByVal value As ComboBoxStyle)
            MyBase.DropDownStyle = ComboBoxStyle.DropDownList
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the specified image for each ImageComboBoxItem of the ImageComboBox is displayed beside the item text.
    ''' </summary>
    ''' <value>True/False</value>
    ''' <returns>True or False</returns>
    <Description("Indicates whether the specified image for each ImageComboBoxItem of the ImageComboBox is displayed beside the item text.")> _
    <DefaultValue(True)> _
    Public Property ShowImages() As Boolean
        Get
            Return _ShowImages
        End Get
        Set(ByVal value As Boolean)
            If _ShowImages <> value Then
                _ShowImages = value
                Me.Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating the alignment of the image for each item in the ImageComboBox.
    ''' </summary>
    ''' <value>Left/Right</value>
    ''' <returns>Enumeration value corresponding to the selected alignment.</returns>
    <Description("Gets or sets a value indicating the alignment of the image for each item in the ImageComboBox.")> _
    <DefaultValue(GetType(ImageAlignment), "Left")> _
    Public Property ImageAlign() As ImageAlignment
        Get
            Return _ImageAlignment
        End Get
        Set(ByVal value As ImageAlignment)
            If [Enum].IsDefined(GetType(ImageAlignment), value) Then
                _ImageAlignment = value
                Me.Invalidate()
            End If
        End Set
    End Property

#End Region

#Region " Methods "

    ''' <summary>
    ''' Draws each ImageComboBoxItem in ImageComboBoxItemCollection in the ImageComboBox.
    ''' </summary>
    Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
        e.DrawBackground()
        e.DrawFocusRectangle()

        Dim bounds As Rectangle = e.Bounds

        If e.Index >= 0 AndAlso e.Index < Me.Items.Count Then
            Dim item As ImageComboBoxItem = Me.Items(e.Index)
            If item IsNot Nothing Then
                e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                If Me.ShowImages AndAlso item.Image IsNot Nothing Then
                    Dim textSize As SizeF = e.Graphics.MeasureString(item.Text, e.Font)
                    Dim w As Single = textSize.Width
                    Dim h As Single = textSize.Height
                    Select Case Me.ImageAlign
                        Case ImageAlignment.Left
                            e.Graphics.DrawImage(item.Image, e.Bounds.Left, e.Bounds.Top, Me.ItemHeight, Me.ItemHeight)
                            e.Graphics.DrawString(item.Text, e.Font, New SolidBrush(e.ForeColor), bounds.Left + Me.ItemHeight, bounds.Top + (bounds.Height - h) / 2)
                            Exit Select
                        Case ImageAlignment.Right
                            e.Graphics.DrawString(item.Text, e.Font, New SolidBrush(e.ForeColor), bounds.Left, bounds.Top + (bounds.Height - h) / 2)
                            e.Graphics.DrawImage(item.Image, e.Bounds.Right - Me.ItemHeight, e.Bounds.Top, Me.ItemHeight, Me.ItemHeight)
                            Exit Select
                        Case Else
                            e.Graphics.DrawString(item.Text, e.Font, New SolidBrush(e.ForeColor), e.Bounds.Left, e.Bounds.Top)
                            Exit Select
                    End Select
                Else
                    e.Graphics.DrawString(item.Text, e.Font, New SolidBrush(e.ForeColor), Bounds.Left, Bounds.Top)
                End If
            End If
        End If

        MyBase.OnDrawItem(e)
    End Sub

#End Region

#Region " Nested classes "

    ''' <summary>
    ''' A collection of ImageComboBoxItems.
    ''' </summary>
    Public Class ImageComboBoxItemCollection
        Inherits System.Collections.ObjectModel.Collection(Of ImageComboBoxItem)

#Region " Fields "

        ''' <summary>
        ''' Keeps a reference to the ImageComboBox so we can update its baseItems list.
        ''' </summary>
        Private _comboBox As ImageComboBox

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Creates an instance of the ImageComboBoxCollection.
        ''' </summary>
        Public Sub New(ByVal comboBox As ImageComboBox)
            _comboBox = comboBox
        End Sub

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds an ImageComboBoxItem to the ImageComboBoxCollection.
        ''' </summary>
        Public Overloads Function Add() As ImageComboBoxItem
            Return Me.Add("New Item", Nothing)
        End Function

        ''' <summary>
        ''' Adds an ImageComboBoxItem to the ImageComboBoxCollection.
        ''' </summary>
        ''' <param name="text">Text to be displayed in the ImageComboBox.</param>
        Public Overloads Function Add(ByVal text As String) As ImageComboBoxItem
            Return Me.Add(text, Nothing)
        End Function

        ''' <summary>
        ''' Adds an ImageComboBoxItem to the ImageComboBoxCollection.
        ''' </summary>
        ''' <param name="text">Text to be displayed in the ImageComboBox for the ImageComboBoxItem.</param>
        ''' <param name="image">Image to be displayed in the ImageComboBox for the ImageComboBoxItem.</param>
        Public Overloads Function Add(ByVal text As String, ByVal image As Image) As ImageComboBoxItem
            Dim item As New ImageComboBoxItem(text, image)
            Me.InsertItem(Me.Items.Count, item)
            Return item
        End Function

        ''' <summary>
        ''' Clears all the ImageComboBoxItems from both the ImageComboBoxCollection and the baseItems list of the ImageComboBox.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub ClearItems()
            MyBase.ClearItems()
            _comboBox.baseItems.Clear()
        End Sub

        ''' <summary>
        ''' Inserts an ImageComboBoxItem at the specified index into the ImageComboBoxCollection and the baseItems list of the ImageComboBox.
        ''' </summary>
        ''' <param name="index">Index at which to insert the ImageComboBoxItem.</param>
        ''' <param name="item">The ImageComboBoxItem to be inserted.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As ImageComboBoxItem)
            MyBase.InsertItem(index, item)
            _comboBox.baseItems.Insert(index, item)
        End Sub

        ''' <summary>
        ''' Removes an ImageComboBoxItem at the specified index from the ImageComboBoxCollection and the baseItems list of the ImageComboBox.
        ''' </summary>
        ''' <param name="index">Index of the ImageComboBoxItem to be removed.</param>
        ''' <remarks></remarks>
        Protected Overrides Sub RemoveItem(ByVal index As Integer)
            MyBase.RemoveItem(index)
            _comboBox.baseItems.RemoveAt(index)
        End Sub

        ''' <summary>
        ''' Replaces an ImageComboBoxItem at the specified index with another ImageComboBoxItem in the ImageComboBoxCollection and the baseItems list of the ImageComboBox.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub SetItem(ByVal index As Integer, ByVal item As ImageComboBoxItem)
            MyBase.SetItem(index, item)
            _comboBox.baseItems(index) = item
        End Sub

        ''' <summary>
        ''' Adds two or more ImageComboBoxItems to the ImageComboBoxCollection.
        ''' </summary>
        ''' <param name="items">The list of ImageComboBoxItems to be inserted.</param>
        ''' <remarks></remarks>
        Public Sub AddRange(ByVal items As IEnumerable(Of ImageComboBoxItem))
            For Each item As ImageComboBoxItem In items
                Me.InsertItem(Me.Items.Count, item)
            Next
        End Sub

#End Region

    End Class

#End Region

End Class

''' <summary>
''' An item that is added to the ImageComboBox
''' </summary>
Public Class ImageComboBoxItem

#Region " Fields "

    ''' <summary>
    ''' String value that holds the text to be displayed for the ImageComboBoxItem.
    ''' </summary>
    Private _text As String

    ''' <summary>
    ''' Image to be displayed for the ImageComboBoxItem.
    ''' </summary>
    Private _image As Image

    ''' <summary>
    ''' Object that holds any additional data you may need for the ImageComboBoxItem. 
    ''' </summary>
    Private _tag As Object

#End Region

#Region " Constructors "

    ''' <summary>
    ''' Creates an instance of the ImageComboBoxItem.
    ''' </summary>
    Public Sub New()
        Me.New("New Item", Nothing)
    End Sub

    ''' <summary>
    ''' Creates an instance of the ImageComboBoxItem.
    ''' </summary>
    ''' <param name="text">String value to be displayed for the ImageComboBoxItem.</param>
    Public Sub New(ByVal text As String)
        Me.New(text, Nothing)
    End Sub

    ''' <summary>
    ''' Creates an instance of the ImageComboBoxItem.
    ''' </summary>
    ''' <param name="text">String value to be displayed for the ImageComboBoxItem.</param>
    ''' <param name="image">Image to be displayed for the ImageComboBoxItem.</param>
    Public Sub New(ByVal text As String, ByVal image As Image)
        Me.Text = text
        Me.Image = image
    End Sub

#End Region

#Region " Properties "

    ''' <summary>
    ''' String value that holds the text to be displayed for the ImageComboBoxItem.
    ''' </summary>
    ''' <returns>String value for the ImageComboBoxItem.</returns>
    Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    ''' <summary>
    ''' Image to be displayed for the ImageComboBoxItem.
    ''' </summary>
    ''' <returns>Image object to be displayed for the ImageComboBoxItem.</returns>
    Property Image() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Image)
            _image = value
        End Set
    End Property

    ''' <summary>
    ''' Object that holds any additional data you may need for the ImageComboBoxItem. 
    ''' </summary>
    ''' <returns>Object that holds any additional data you may need for the ImageComboBoxItem. </returns>
    Public Property Tag() As Object
        Get
            Return _tag
        End Get
        Set(ByVal value As Object)
            _tag = value
        End Set
    End Property

#End Region

End Class
