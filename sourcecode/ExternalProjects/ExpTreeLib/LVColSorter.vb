Imports System.Windows.Forms

''' <summary>
''' LVColSorter is a Class to be used as a ListViewItemSorter. 
''' LVColSorter may be used as the ListViewItemSorter for ListViews populated by any means, but works
''' best when the .Tag properties of the ListViewItems and SubItems are set as described in Remarks.
''' </summary>
''' <remarks>LVColSorter uses the .Tag properties of ListViewItems and SubItems to Sort the contents of a
''' ListView based on the underlying data. If no .Tag properties are set, or if the value of those
''' properties do not implement the IComparable interface, then LVColSorter sorts based on the .Text
''' properties of the SubItems of the ListViewItems.
''' <para>If the .Text properties of SubItems are structured properly for Sorting, then no .Tag information
''' is required for Sorting. Most .Text is not properly structured for Sorting. In that case, 
''' the .Tag property may be set to provide this class with the information needed for a proper Sort.</para>
''' <para>Set the .Tags as follows:
''' <list type="table">
''' <item><term>Each ListViewItem</term><description>The Class instance or DataRow from which the ListViewItem is built.
'''                                                  The instance should support the IComparable Interface,
'''                                                  if not, it is ignored for Sort purposes and may be omitted.</description></item>
''' <item><term>Each SubItem</term><description>If the .Text property will not Sort correctly, then the .Tag should be
'''                                set to the original Value (Date, Double, etc.)</description></item>
''' </list>See the documentation of the Compare Method of this Class for the actual Sort rules.</para>
''' <para>Class Properties
''' or DataRow Fields whose Value is a String will Sort based on that String. String
''' Properties that have been Formatted in a non-Sortable Format in the original data will not Sort correctly. 
''' The application
''' will have to deal with that case separately by setting the SubItem.Tags to a Sortable Value.</para>
''' <para>Each instance of LVColSorter will handle the ListView.ColumnClick event for the associated
''' ListView. The using application <i>should not</i> Handle that Event. When a new ListViewItemSorter
''' is assigned to a ListView, any prior instances of LVColSorter will remove themselves from the 
''' EventHandler list of that ListView. In other words, multiple ListViewItemSorters may be assigned
''' to a ListView without causing prior instances to attempt to handle ColumnClick.</para></remarks>
Public Class LVColSorter
    Implements IComparer

    ''' <summary>
    ''' Compares two ListViewItems from the same ListView in accordance to the Sort rules of the Class.
    ''' </summary>
    ''' <param name="x">First ListViewItem to be Compared.</param>
    ''' <param name="y">Second ListViewItem to be Compared.</param>
    ''' <returns><list type="table">
    ''' <item><term>-1</term><description>If the First item is Less than the Second.</description></item>
    ''' <item><term>0</term><description>If the two items are Equal.</description></item>
    ''' <item><term>1</term><description>If the First item is Greater than the Second.</description></item>
    ''' </list></returns>
    ''' <remarks>Odd numbers of clicks on a column will sort Ascending, even numbers will sort 
    ''' Descending (click 1 Ascending, Click 2 Descending ...). The Sort rules are:
    ''' <list type="number">
    ''' <item><description>If the Clicked column is Column 0 and the ListViewItem's Tag supports the
    '''                    Icomparable Interface, then Compare the ListViewItems Tags.</description></item>
    ''' <item><description>Otherwise, or if the ListViewItems .Tags Compare Equal, then continue with
    '''                    the following rules.</description></item>
    ''' <item><description>If the Clicked column's ListViewItem.SubItem's Tag property supports the
    '''                    IComparable Interface, then use CompareTo to compare the .Tags</description></item>
    ''' <item><description>If the Clicked column's ListViewItem.SubItem's Tag property is Nothing or
    '''                    does not support the Icomparable Interface, Compare the .Text properties.</description></item>
    ''' <item><description>If the items Compare Equal and the Clicked column is not Column 0 then
    '''                    continue the Comparison based on the Column 0 rules above. This has
    '''                    the effect of using the either the source Class instances or the contents of
    '''                    column 0 as a secondary key for the Sort.</description></item>
    ''' <item><description>The result of the comparison is toggled according to if the sort is
    '''                    Ascending or Decending. This sort order is determined by reversing the
    '''                    sort order of the last click on this column.</description></item>
    ''' </list></remarks>
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
                    Implements System.Collections.IComparer.Compare
        'If m_ColOrder(m_Col) = 0 Then Exit Function 'First time thru with no columnclick. Retain original order 6/13/2012 - Allow use as standalone ie Insert with no Col Click
        Compare = 0
        Dim LVX As ListViewItem = DirectCast(x, ListViewItem)
        Dim LVY As ListViewItem = DirectCast(y, ListViewItem)

        If m_Col = 0 AndAlso OKToCompare(LVX.Tag, LVY.Tag) Then
            Compare = LVX.Tag.compareto(LVY.Tag)
        End If
        If Compare = 0 Then
            'Note that in some cases the SubItem Tags may not yet be set up (eg doing set up of some lvi's  in background thread)
            '   in other words, the first lvi may have tags but not all lvi's have tags yet.
            If OKToCompare(LVX.SubItems(m_Col).Tag, LVY.SubItems(m_Col).Tag) Then
                Compare = LVX.SubItems(m_Col).Tag.compareto(LVY.SubItems(m_Col).Tag)
            Else
                Compare = String.Compare(LVX.SubItems(m_Col).Text, LVY.SubItems(m_Col).Text)
            End If
        End If
        If Compare = 0 AndAlso m_Col <> 0 Then      'always use the original ordering as a second key (if not the primary)
            If m_Col = 0 AndAlso OKToCompare(LVX.Tag, LVY.Tag) Then
                Compare = LVX.Tag.compareto(LVY.Tag)
            ElseIf OKToCompare(LVX.SubItems(0).Tag, LVY.SubItems(0).Tag) Then   '6/13/2012 - fixed coding error
                Compare = LVX.SubItems(0).Tag.compareto(LVY.SubItems(0).Tag)
            Else
                Compare = LVX.SubItems(0).Text.CompareTo(LVY.SubItems(0).Text)
            End If
        End If
        If m_ColOrder(m_Col) <> 0 Then Compare = Compare * m_ColOrder(m_Col) '6/13/2012 - Allow use as standalone ie Insert with no Col Click
    End Function

#Region "   Private Fields"
    Private m_View As ListView
    Private m_Col As Integer
    Private m_ColOrder() As Integer

#End Region

#Region "   Constructor"
    ''' <summary>
    ''' Creates a new instance of LVColSorter based on a fully populated
    ''' ListView, with ColumnHeaders defined. Assigns its own Handler for ListView.ColumnClick Events.
    ''' </summary>
    ''' <param name="lv">A fully populated ListView, preferably set up by SetUpListView, which will
    ''' be using this instance as the ListViewItemSorter.</param>
    ''' <remarks></remarks>
    Sub New(ByVal lv As ListView)
        m_View = lv
        ReDim m_ColOrder(lv.Columns.Count - 1)
        For i As Integer = 0 To lv.Columns.Count - 1
            ListViewSortGlyph.SetSortIcon(lv, i, SortOrder.None)
        Next
        lv.ListViewItemSorter = Nothing
        AddHandler lv.ColumnClick, AddressOf ListView_ColumnClick
    End Sub

    Private Function OKToCompare(ByVal X As Object, ByVal Y As Object) As Boolean
        If CompareOK(X) AndAlso CompareOK(Y) Then
            OKToCompare = Object.ReferenceEquals(X.GetType, Y.GetType)
        Else : OKToCompare = False
        End If
    End Function

    Private Function CompareOK(ByVal obj As Object) As Boolean
        CompareOK = False         'assume not OK
        If obj Is Nothing Then Exit Function
        Dim IInfo() As Type = obj.GetType.GetInterfaces
        If IInfo Is Nothing Then Exit Function
        For Each Inter As Type In IInfo
            If Inter.Name.ToLower.StartsWith("icomparable") Then
                Return True
            End If
        Next
    End Function
#End Region

#Region "   Public Properties"
    ''' <summary>
    ''' The order in which the ListView was last sorted.
    ''' </summary>
    ''' <returns>A SortOrder indicating the order in which the ListView was last sorted.</returns>
    ''' <remarks>A return of SortOrder.None indicates that the ListView has never been sorted.
    ''' The Properties OrderOfSort and SortColumn may be used if the application wishes to Draw Sort
    ''' glyphs on the ColumnHeaders.
    ''' </remarks>
    Public ReadOnly Property OrderOfSort() As SortOrder
        Get
            Return m_ColOrder(m_Col)
        End Get
    End Property
    ''' <summary>
    ''' The ListView column on which the ListView was last sorted. Setting this property to a valid
    ''' value will cause the ListView to be sorted on that column in the order based on OrderOfSort rules.
    ''' Specifically, the column will be sorted in reverse of the order it was last sorted.
    ''' </summary>
    ''' <returns>The ListView column on which the ListView was last sorted.</returns>
    ''' <remarks>Unsorted ListViews will return 0 for the SortColumn.
    ''' The Properties OrderOfSort and SortColumn may be used if the application wishes to Draw Sort
    ''' glyphs on the ColumnHeaders.
    ''' </remarks>
    Public Property SortColumn() As Integer
        Get
            Return m_Col
        End Get
        Set(ByVal value As Integer)
            If value > -1 AndAlso value < m_View.Columns.Count Then
                m_Col = value
                ListView_ColumnClick(m_View, New ColumnClickEventArgs(m_Col))
            End If
        End Set
    End Property
#End Region

#Region "   ColumnClick Handler"
    Private Sub ListView_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        Dim LV As ListView = sender   'simplify code a bit -- will throw exception if sender is not a ListView
        'Check that this instance of ListViewColumnSorter is still the operative one
        'if Me is not the operative ListViewColumnSorter, then remove this instance's Handler and exit
        'Debug.WriteLine("LVSorter ColumnClick on " & e.Column)
        If (LV.ListViewItemSorter Is Nothing) _
                 OrElse LV.ListViewItemSorter IsNot Me Then
            RemoveHandler LV.ColumnClick, AddressOf ListView_ColumnClick
            Exit Sub
        End If
        m_Col = e.Column
        If m_ColOrder(m_Col) = 0 Then
            m_ColOrder(m_Col) = 1
        Else
            m_ColOrder(m_Col) *= -1
        End If
        LV.Sort()
        Dim Order As SortOrder
        If m_ColOrder(m_Col) > 0 Then
            Order = SortOrder.Ascending
        Else
            Order = SortOrder.Descending
        End If
        ListViewSortGlyph.SetSortIcon(LV, m_Col, Order)
    End Sub
#End Region
End Class

''' <summary>
''' Set the Sort Glyph on a ListView Column.
''' Obtained from <a href="http://stackoverflow.com/questions/254129/how-to-i-display-a-sort-arrow-in-the-header-of-a-list-view-column-using-c">here</a>
''' and converted to VB.Net by JDP using
''' <a href="http://www.developerfusion.com/tools/convert/csharp-to-vb/">The tools at DeveloperFusion.com</a>
''' JDP also added all XML comments.
''' 
''' The only Public member is the Shared Sub SetIcon.
''' </summary>
''' <remarks>
''' This Class is included here for the use of the LVColSorter Class. However, it may used with any ListViewColumnSorter that calls it.
''' <para>Normally the Caller will set the Glyph to point to the direction that the Column will be Sorted on the NEXT ColumnClick</para></remarks>
<System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
Public NotInheritable Class ListViewSortGlyph
    Private Sub New()
    End Sub
    <System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
    Private Structure LVCOLUMN
        Public mask As Int32
        Public cx As Int32
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)> _
        Public pszText As String
        Public hbm As IntPtr
        Public cchTextMax As Int32
        Public fmt As Int32
        Public iSubItem As Int32
        Public iImage As Int32
        Public iOrder As Int32
    End Structure

    Private Const HDI_FORMAT As Int32 = &H4
    Private Const HDF_SORTUP As Int32 = &H400
    Private Const HDF_SORTDOWN As Int32 = &H200
    Private Const LVM_GETHEADER As Int32 = &H101F
    Private Const HDM_GETITEM As Int32 = &H120B
    Private Const HDM_SETITEM As Int32 = &H120C

    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SendMessage")> _
    Private Shared Function SendMessageLVCOLUMN(ByVal hWnd As IntPtr, ByVal Msg As Int32, ByVal wParam As IntPtr, ByRef lPLVCOLUMN As LVCOLUMN) As IntPtr
    End Function


    '<System.Runtime.CompilerServices.Extension> _ --- This version is Not implemented as an Extension Method
    ''' <summary>
    ''' Set the input ordering Sort Glyph on the input Column of the input ListView, and clears the Sort Glyph from all other Columns.
    ''' </summary>
    ''' <param name="ListViewControl">The ListView Control containing the Column</param>
    ''' <param name="ColumnIndex">The Index of the Column to receive the Sort Glyph</param>
    ''' <param name="Order">The SortOrder designator of the desired Glyph</param>
    ''' <remarks></remarks>
    Public Shared Sub SetSortIcon(ByVal ListViewControl As System.Windows.Forms.ListView, ByVal ColumnIndex As Integer, ByVal Order As System.Windows.Forms.SortOrder)
        Dim ColumnHeader As IntPtr = SendMessage(ListViewControl.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)

        For ColumnNumber As Integer = 0 To ListViewControl.Columns.Count - 1
            Dim ColumnPtr As New IntPtr(ColumnNumber)
            Dim lvColumn As New LVCOLUMN()
            lvColumn.mask = HDI_FORMAT
            SendMessageLVCOLUMN(ColumnHeader, HDM_GETITEM, ColumnPtr, lvColumn)

            If Not (Order = System.Windows.Forms.SortOrder.None) AndAlso ColumnNumber = ColumnIndex Then
                Select Case Order
                    Case System.Windows.Forms.SortOrder.Ascending
                        lvColumn.fmt = lvColumn.fmt And Not HDF_SORTDOWN
                        lvColumn.fmt = lvColumn.fmt Or HDF_SORTUP
                        Exit Select
                    Case System.Windows.Forms.SortOrder.Descending
                        lvColumn.fmt = lvColumn.fmt And Not HDF_SORTUP
                        lvColumn.fmt = lvColumn.fmt Or HDF_SORTDOWN
                        Exit Select
                End Select
            Else
                lvColumn.fmt = lvColumn.fmt And Not HDF_SORTDOWN And Not HDF_SORTUP
            End If

            SendMessageLVCOLUMN(ColumnHeader, HDM_SETITEM, ColumnPtr, lvColumn)
        Next
    End Sub
End Class
