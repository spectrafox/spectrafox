Imports System.ComponentModel
Imports ZedGraph

Public Class mSpectroscopyTableViewer
    Implements iSingleSpectroscopyTableLoaded
    Implements IDisposable

#Region "Properties"
    Private bReady As Boolean = False

    ''' <summary>
    ''' Uses the ColorScheme Class to create an array of Colors to
    ''' display different SpectroscopyTables in separate colors!
    ''' </summary>
    Private ColorArray As New List(Of Color)
    'Private ColorArray As SolidBrush() = cColorScheme.Colorcube.BrushArray

    ''' <summary>
    ''' Returns the next color in the array.
    ''' Usefull for plots of multiple data.
    ''' </summary>
    ''' <param name="CurrentColor"></param>
    ''' <param name="NextIndex">Give the n'th next type.</param>
    Public Function GetNextColor(ByVal CurrentColor As Color,
                                 Optional ByVal NextIndex As Integer = 1) As Color
        For i As Integer = 0 To ColorArray.Count - 1 Step 1
            If ColorArray(i) = CurrentColor Then
                Dim ReturnIndex As Integer = i + NextIndex

                If ReturnIndex = ColorArray.Count - 1 Then
                    ReturnIndex -= i
                End If

                Return ColorArray(ReturnIndex)
            End If
        Next

        ' If nothing worked, return current symbol type
        Return CurrentColor
    End Function

    ''' <summary>
    ''' Create an array of Symbols to
    ''' display different SpectroscopyTables as separate Symbols!
    ''' </summary>
    Private SymbolArray() As SymbolType = New SymbolType() {SymbolType.Circle,
                                                            SymbolType.Square,
                                                            SymbolType.Triangle,
                                                            SymbolType.TriangleDown,
                                                            SymbolType.Diamond,
                                                            SymbolType.Star,
                                                            SymbolType.XCross,
                                                            SymbolType.Plus,
                                                            SymbolType.HDash,
                                                            SymbolType.VDash}

    ''' <summary>
    ''' Returns the next symbol-type in the array.
    ''' Usefull for plots of multiple data of the same file.
    ''' </summary>
    ''' <param name="CurrentSymbolType"></param>
    ''' <param name="NextIndex">Give the n'th next type.</param>
    Public Function GetNextSymbolType(ByVal CurrentSymbolType As SymbolType,
                                      Optional ByVal NextIndex As Integer = 1) As SymbolType
        For i As Integer = 0 To SymbolArray.Length - 1 Step 1
            If SymbolArray(i) = CurrentSymbolType Then
                Dim ReturnIndex As Integer = i + NextIndex

                If ReturnIndex = SymbolArray.Length - 1 Then
                    ReturnIndex -= i
                End If

                Return SymbolArray(ReturnIndex)
            End If
        Next

        ' If nothing worked, return current symbol type
        Return CurrentSymbolType
    End Function

    ''' <summary>
    ''' Create an array of line-styles to
    ''' display different SpectroscopyTables as separate styles!
    ''' </summary>
    Private LineStyleArray() As Drawing2D.DashStyle = New Drawing2D.DashStyle() {Drawing2D.DashStyle.Solid,
                                                                                 Drawing2D.DashStyle.Dash,
                                                                                 Drawing2D.DashStyle.DashDot,
                                                                                 Drawing2D.DashStyle.DashDotDot,
                                                                                 Drawing2D.DashStyle.Dot}

    ''' <summary>
    ''' Random-Generator, to create random colors, if more spectroscopy-tables
    ''' were added, than the predefined brush-array can manage.
    ''' </summary>
    Private RandomGenerator As New System.Random

    ''' <summary>
    ''' This property determines, if the scale of the graph
    ''' is restored, after a redraw of the image.
    ''' </summary>
    Public Property AutomaticallyRestoreScaleAfterRedraw As Boolean = True

    ''' <summary>
    ''' Selection Mode for the Y Column
    ''' </summary>
    Public Property MultipleYColumnSelectionMode As System.Windows.Forms.SelectionMode
        Get
            Return Me.cbY.IfAppearanceListBox_MultiSelectionMode
        End Get
        Set(value As System.Windows.Forms.SelectionMode)
            Me.cbY.IfAppearanceListBox_MultiSelectionMode = value
        End Set
    End Property

    ''' <summary>
    ''' Last Filter Saving for the column-selectors
    ''' </summary>
    Public Property TurnOnLastFilterSaving_Y As Boolean
        Get
            Return Me.cbY.TurnOnLastFilterSaving
        End Get
        Set(value As Boolean)
            Me.cbY.TurnOnLastFilterSaving = value
        End Set
    End Property

    ''' <summary>
    ''' Last Selection Saving for the column-selectors
    ''' </summary>
    Public Property TurnOnLastSelectionSaving_Y As Boolean
        Get
            Return Me.cbY.TurnOnLastSelectionSaving
        End Get
        Set(value As Boolean)
            Me.cbY.TurnOnLastSelectionSaving = value
        End Set
    End Property

    Public _MultipleSpectraStackOffset As Double = 0
    ''' <summary>
    ''' Values used when plotting multiple values.
    ''' Stacks the graphs on top of each other.
    ''' </summary>
    Public Property MultipleSpectraStackOffset As Double
        Get
            Return Me._MultipleSpectraStackOffset
        End Get
        Set(value As Double)
            Me._MultipleSpectraStackOffset = value
            My.Settings.SpectroscopyTableViewer_StackOffset_Last = value
            Me.txtStackValue.SetValue(value)
            Me.PaintPreviewImage()
        End Set
    End Property

#End Region

#Region "Spectroscopy-Table Objects, Properties, Colors, and Symbol-Types"
    Private SpectroscopyTables As New Dictionary(Of String, cSpectroscopyTable)
    ''' <summary>
    ''' Returns the currently saved Dictionary of Spectroscopy-Tables
    ''' (FullFileName, SpectroscopyTable)
    ''' </summary>
    Public ReadOnly Property CurrentSpectroscopyTables As Dictionary(Of String, cSpectroscopyTable)
        Get
            Return Me.SpectroscopyTables
        End Get
    End Property

    ''' <summary>
    ''' Adds or updates a single SpectroscopyTable in the internal display list.
    ''' </summary>
    Public Sub AddSpectroscopyTable(ByRef SpectroscopyTable As cSpectroscopyTable) Implements iSingleSpectroscopyTableLoaded.SpectroscopyTableLoaded
        If SpectroscopyTable Is Nothing Then Return

        Me.bReady = False

        ' Get the index of the spectroscopy-table
        Dim SpectroscopyTableIndex As Integer = 0
        For Each SpectroscopyTableName As String In Me.SpectroscopyTables.Keys
            If SpectroscopyTableName = SpectroscopyTable.FullFileName Then
                Exit For
            End If
            SpectroscopyTableIndex += 1
        Next

        ' Get a predefined color for the SpectroscopyTable
        ' or... if more spectroscopy-tables should be drawn than predefined colors exist,
        ' then generate a random color.
        Dim PaintColor As Color
        If SpectroscopyTableIndex < ColorArray.Count Then
            ' Select a predefined color from the color-selector-box.
            PaintColor = ColorArray(SpectroscopyTableIndex)
        Else
            ' Generate random color
            PaintColor = Color.FromArgb(RandomGenerator.Next(0, 255),
                                        RandomGenerator.Next(0, 255),
                                        RandomGenerator.Next(0, 255),
                                        RandomGenerator.Next(0, 255))
        End If

        ' Get a predefined symbol for the SpectroscopyTable
        ' or... if more spectroscopy-tables should be drawn than predefined symbols exist,
        ' then repeat from the beginning... the color should be different.
        Dim Symbol As SymbolType
        If SpectroscopyTableIndex < Me.SymbolArray.Length Then
            ' Select a predefined Symbol
            Symbol = Me.SymbolArray(SpectroscopyTableIndex)
        Else
            ' Start over
            Symbol = Me.SymbolArray(SpectroscopyTableIndex Mod Me.SymbolArray.Length)
        End If

        ' Change or add the data to the plot
        If Not Me.SpectroscopyTables.ContainsKey(SpectroscopyTable.FullFileName) Then
            Me.SpectroscopyTables.Add(SpectroscopyTable.FullFileName, SpectroscopyTable)

            ' Set Color and other Default Settings
            Me._PaintColors.Add(SpectroscopyTable.FullFileName, PaintColor)
            Me._SymbolTypes.Add(SpectroscopyTable.FullFileName, Symbol)
            Me._SymbolSizes.Add(SpectroscopyTable.FullFileName, My.Settings.Plot2D_LastSymbolSize)
            Me._LineWidths.Add(SpectroscopyTable.FullFileName, My.Settings.Plot2D_LastLineThickness)
            Me._LineStyles.Add(SpectroscopyTable.FullFileName, My.Settings.Plot2D_LastDashStyle)
        Else
            Me.SpectroscopyTables(SpectroscopyTable.FullFileName) = SpectroscopyTable

            ' Set Color and other Default Settings
            Me._PaintColors(SpectroscopyTable.FullFileName) = PaintColor
            Me._SymbolTypes(SpectroscopyTable.FullFileName) = Symbol
            Me._SymbolSizes(SpectroscopyTable.FullFileName) = My.Settings.Plot2D_LastSymbolSize
            Me._LineWidths(SpectroscopyTable.FullFileName) = My.Settings.Plot2D_LastLineThickness
            Me._LineStyles(SpectroscopyTable.FullFileName) = My.Settings.Plot2D_LastDashStyle
        End If

        ' Update the style-settings-list
        UpdateStyleSpectroscopyTableList()

        ' Update ColumnList
        Me.cbX.InitializeColumns(SpectroscopyTable.GetColumnNameList, My.Settings.LastSpectroscopyPlot_SelectedColumnNameX, False)
        Me.cbY.InitializeColumns(SpectroscopyTable.GetColumnNameList, My.Settings.LastSpectroscopyPlot_SelectedColumnNames, Me.TurnOnLastFilterSaving_Y)

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Delegate to call a sub without parameters.
    ''' </summary>
    Private Delegate Sub VoidSub()

    ''' <summary>
    ''' Call the filling of the spectroscopy-table-list in the style-panel thread-safe.
    ''' </summary>
    Private Sub UpdateStyleSpectroscopyTableList()
        If Me.lbLines.InvokeRequired Then
            Me.lbLines.Invoke(New VoidSub(AddressOf UpdateStyleSpectroscopyTableList_ThreadSafe))
        Else
            UpdateStyleSpectroscopyTableList_ThreadSafe()
        End If
    End Sub

    ''' <summary>
    ''' Fill the list-box with all spectroscopy-tables
    ''' </summary>
    Private Sub UpdateStyleSpectroscopyTableList_ThreadSafe()

        '# Added 10.08.2015: check, if the listbox is already disposed... was reported as bug.
        If Me.lbLines.IsDisposed Then Return

        With Me.lbLines
            .Items.Clear()
            For Each s As cSpectroscopyTable In Me.SpectroscopyTables.Values
                .Items.Add(s.FileNameWithoutPathAndExtension)
            Next
        End With
    End Sub

    ''' <summary>
    ''' Called, when the fetching routine has completed!
    ''' </summary>
    Public Sub AllSpectroscopyTablesLoaded()
        ' Update ColumnList
        Me.bReady = False
        Me.cbX.InitializeColumns(Me.SpectroscopyTables, My.Settings.LastSpectroscopyPlot_SelectedColumnNameX, False)
        Me.bReady = True
        Me.cbY.InitializeColumns(Me.SpectroscopyTables, My.Settings.LastSpectroscopyPlot_SelectedColumnNames, Me.TurnOnLastFilterSaving_Y)
    End Sub

    ''' <summary>
    ''' Adds or updates multiple Spectroscopy-Tables to the Preview-List.
    ''' </summary>
    Public Sub AddSpectroscopyTables(ByRef AddSpectroscopyTables As List(Of cSpectroscopyTable))
        For Each SpectroscopyTable As cSpectroscopyTable In AddSpectroscopyTables
            Me.AddSpectroscopyTable(SpectroscopyTable)
        Next

        ' Update ColumnList
        Me.bReady = False
        Me.cbX.InitializeColumns(Me.SpectroscopyTables, My.Settings.LastSpectroscopyPlot_SelectedColumnNameX, False)
        Me.bReady = True
        Me.cbY.InitializeColumns(Me.SpectroscopyTables, My.Settings.LastSpectroscopyPlot_SelectedColumnNames, Me.TurnOnLastFilterSaving_Y)
    End Sub

    ''' <summary>
    ''' Removes a Spectroscopy-Table from the Preview-List.
    ''' </summary>
    Public Sub RemoveSpectroscopyTable(ByVal FullFileName As String)
        If Me.SpectroscopyTables Is Nothing Then Return
        If Me.SpectroscopyTables.ContainsKey(FullFileName) Then
            Me.SpectroscopyTables.Remove(FullFileName)
            Me._PaintColors.Remove(FullFileName)
            Me._SymbolTypes.Remove(FullFileName)
            Me._LineWidths.Remove(FullFileName)
            Me._SymbolSizes.Remove(FullFileName)
            Me._LineStyles.Remove(FullFileName)
        End If
    End Sub

    ''' <summary>
    ''' Deletes all displayed SpectroscopyTables.
    ''' </summary>
    Public Sub ClearSpectroscopyTables()
        Me.SpectroscopyTables.Clear()
        Me._PaintColors.Clear()
        Me._SymbolTypes.Clear()
        Me._LineWidths.Clear()
        Me._SymbolSizes.Clear()
        Me._LineStyles.Clear()
    End Sub


    ''' <summary>
    ''' Color-Array for the colors of the individual SpectroscopyTable-Data.
    ''' </summary>
    Private _PaintColors As New Dictionary(Of String, Color)

    ''' <summary>
    ''' Changes the Color of the SpectroscopyTable.
    ''' </summary>
    Public Sub ChangeColor(ByVal FullFileName As String, ByVal NewColor As Color)
        If Me._PaintColors.ContainsKey(FullFileName) Then
            Me._PaintColors(FullFileName) = NewColor
        End If
    End Sub

    ''' <summary>
    ''' SymbolType-Array for the display style of the individual SpectroscopyTable-Data.
    ''' </summary>
    Private _SymbolTypes As New Dictionary(Of String, ZedGraph.SymbolType)

    ''' <summary>
    ''' Changes the SymbolType of the SpectroscopyTable.
    ''' </summary>
    Public Sub ChangeSymbolType(ByVal FullFileName As String, ByVal NewSymbolType As ZedGraph.SymbolType)
        If Me._SymbolTypes.ContainsKey(FullFileName) Then
            Me._SymbolTypes(FullFileName) = NewSymbolType
        End If
    End Sub

    ''' <summary>
    ''' SymbolSize-Array for the display style of the individual SpectroscopyTable-Data.
    ''' </summary>
    Private _SymbolSizes As New Dictionary(Of String, Decimal)

    ''' <summary>
    ''' Changes the SymbolType of the SpectroscopyTable.
    ''' </summary>
    Public Sub ChangeSymbolSize(ByVal FullFileName As String, ByVal NewSymbolSize As Decimal)
        If Me._SymbolSizes.ContainsKey(FullFileName) Then
            Me._SymbolSizes(FullFileName) = NewSymbolSize
            My.Settings.Plot2D_LastSymbolSize = NewSymbolSize
            My.Settings.Save()
        End If
    End Sub

    ''' <summary>
    ''' LineStyles-Array for the display style of the individual SpectroscopyTable-Data.
    ''' </summary>
    Private _LineStyles As New Dictionary(Of String, Drawing2D.DashStyle)

    ''' <summary>
    ''' Changes the LineStyle of the SpectroscopyTable.
    ''' </summary>
    Public Sub ChangeLineStyle(ByVal FullFileName As String, ByVal NewLineType As Drawing2D.DashStyle)
        If Me._LineStyles.ContainsKey(FullFileName) Then
            Me._LineStyles(FullFileName) = NewLineType
            My.Settings.Plot2D_LastDashStyle = NewLineType
            My.Settings.Save()
        End If
    End Sub

    ''' <summary>
    ''' LineWidths-Array for the display style of the individual SpectroscopyTable-Data.
    ''' </summary>
    Private _LineWidths As New Dictionary(Of String, Decimal)

    ''' <summary>
    ''' Changes the LineWidths of the SpectroscopyTable.
    ''' </summary>
    Public Sub ChangeLineWidth(ByVal FullFileName As String, ByVal LineWidth As Decimal)
        If Me._LineWidths.ContainsKey(FullFileName) Then
            Me._LineWidths(FullFileName) = LineWidth
            My.Settings.Plot2D_LastLineThickness = LineWidth
            My.Settings.Save()
        End If
    End Sub


#End Region

#Region "Column-Selection"
    Private _ShowColumnSelectors As Boolean = True
    ''' <summary>
    ''' Determines, if the User is allowed to change the visible Column.
    ''' </summary>
    <DescriptionAttribute("Determines, if the User is allowed to change the visible Column."), _
     CategoryAttribute("PB Settings")>
    Public Property ShowColumnSelectors As Boolean
        Get
            Return Me._ShowColumnSelectors
        End Get
        Set(value As Boolean)
            Me._ShowColumnSelectors = value
            Me.panSettings.Visible = value
            If Not value Then
                Me.zPreview.Dock = DockStyle.Fill
            Else
                Me.zPreview.Dock = DockStyle.None
            End If
        End Set
    End Property

    Private _AllowAdjustingXColumn As Boolean = True
    ''' <summary>
    ''' Determines, if the Groupbox around is shown
    ''' </summary>
    <DescriptionAttribute("Determines, if the User can Adjust the X Column."), _
     CategoryAttribute("PB Settings")>
    Public Property AllowAdjustingXColumn As Boolean
        Get
            Return Me._AllowAdjustingXColumn
        End Get
        Set(value As Boolean)
            Me._AllowAdjustingXColumn = value
            Me.cbX.Enabled = value
        End Set
    End Property

    Private _AllowAdjustingYColumn As Boolean = True
    ''' <summary>
    ''' Determines, if the Groupbox around is shown
    ''' </summary>
    <DescriptionAttribute("Determines, if the User can Adjust the Y Column."), _
     CategoryAttribute("PB Settings")>
    Public Property AllowAdjustingYColumn As Boolean
        Get
            Return Me._AllowAdjustingYColumn
        End Get
        Set(value As Boolean)
            Me._AllowAdjustingYColumn = value
            Me.cbY.Enabled = value
        End Set
    End Property
#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' add all available colors
        Me.ColorArray = Me.cbLineColor.ColorsAvailable
        With Me.ColorArray
            ' Define some often used standard colors
            Dim StandardColors As Color() = {Color.Black,
                                             Color.Blue,
                                             Color.Red,
                                             Color.Orange,
                                             Color.Yellow,
                                             Color.DarkGreen}
            ' remove the standard-colors from the initial list,
            ' to avoid doublings
            Me.ColorArray.RemoveAll(Function(C As Color) As Boolean
                                        Return StandardColors.Contains(C)
                                    End Function)

            ' add standard colors at the beginning
            Me.ColorArray.InsertRange(0, StandardColors)
        End With

        ' Default Pane Settings
        With Me.zPreview
            .GraphPane.Title.Text = ""

            ' by default show the point values
            .IsShowPointValues = True
        End With

        ' Set last selected Axis-Scale type.
        Me.ckbLogX.Checked = My.Settings.PreviewBox_LogX
        Me.ckbLogY.Checked = My.Settings.PreviewBox_LogY

        ' Set the last stacking property
        Me.MultipleSpectraStackOffset = My.Settings.SpectroscopyTableViewer_StackOffset_Last

        ' Position Settings-Panel at lower left/right corner:
        With Me.zPreview.GraphPane
            Me.panSettings.Location = New Point(CInt(.Rect.Location.X + .Rect.Width - Me.panSettings.Width) + 3,
                                                CInt(.Rect.Location.Y + .Rect.Height - Me.panSettings.Height) - 3)
            Me.panStyle.Location = New Point(CInt(.Rect.Location.X + .Margin.Left),
                                             CInt(.Rect.Location.Y + .Rect.Height - Me.panStyle.Height) - 3)
        End With

        ' Initially collapse the setting panels
        Me.dpRight.SlideIn(True)
        Me.dpLeft.SlideIn(True)

        ' Fill the symbol-list, and the line-style-list.
        With cbSymbolTypes
            Dim s As New ZedGraph.Symbol
            For i As Integer = 0 To Me.SymbolArray.Length - 1 Step 1
                ' Set the type of the current symbol
                s.Type = Me.SymbolArray(i)

                ' Create new image
                Dim img As New Bitmap(20, 20)
                Dim g As Graphics = Graphics.FromImage(img)

                ' Draw the symbol with the reference to the preview-pane
                s.DrawSymbol(g, Me.zPreview.GraphPane, 10, 10, 1, False, Nothing)

                ' Save the symbol in the selection
                .Items.Add(Me.SymbolArray(i).ToString, img)
            Next
        End With

        ' Fill the line-style-list, and the line-style-list.
        With cbLineStyle
            For i As Integer = 0 To Me.LineStyleArray.Length - 1 Step 1
                ' Save the symbol in the selection
                .Items.Add(Me.LineStyleArray(i).ToString)
            Next
        End With

        ' Disable style settings initially
        Me.nudLineWidth.Enabled = False
        Me.nudSymbolSize.Enabled = False
        Me.cbSymbolTypes.Enabled = False
        Me.cbLineStyle.Enabled = False
        Me.cbLineColor.Enabled = False

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Paints the selected Column from the Spectroscopy-Table
    ''' as the only SpectroscopyTable in the Control.
    ''' (This function exists for compatibility to older versions!!!)
    ''' </summary>
    Public Sub SetSinglePreviewImage(ByVal SpectroscopyTable As cSpectroscopyTable,
                                     Optional ByVal SelectedColumnNameX As String = "",
                                     Optional ByVal SelectedColumnNamesY As List(Of String) = Nothing,
                                     Optional ByVal PreferLastSelectedColumnNames As Boolean = False)
        If SpectroscopyTable Is Nothing Then Return

        Me.ClearSpectroscopyTables()
        Me.AddSpectroscopyTable(SpectroscopyTable)

        ' Fill the Column-Selectors.
        Me.bReady = False

        Me.cbX.InitializeColumns(SpectroscopyTable.GetColumnList)
        Me.cbY.InitializeColumns(SpectroscopyTable.GetColumnList,, Me.TurnOnLastFilterSaving_Y)

        Dim YColumnsToSelect As List(Of String)
        If (PreferLastSelectedColumnNames) AndAlso
           (My.Settings.LastSpectroscopyPlot_SelectedColumnNames IsNot Nothing AndAlso My.Settings.LastSpectroscopyPlot_SelectedColumnNames.Count > 0) Then

            ' Add the list from the settings.
            YColumnsToSelect = New List(Of String)(My.Settings.LastSpectroscopyPlot_SelectedColumnNames.Count)
            For Each C As String In My.Settings.LastSpectroscopyPlot_SelectedColumnNames
                YColumnsToSelect.Add(C)
            Next

        Else

            ' add the given list
            YColumnsToSelect = SelectedColumnNamesY
        End If

        ' Select the given entries:
        Me.SelectColumns(SelectedColumnNameX, YColumnsToSelect)

        Me.bReady = True
        Me.PaintPreviewImage()
    End Sub

    ''' <summary>
    ''' Paints the selected Column from the Spectroscopy-Table
    ''' as the only SpectroscopyTable in the Control.
    ''' (This function exists for compatibility to older versions!!!)
    ''' </summary>
    Public Sub SetSinglePreviewImage(ByVal SpectroscopyTable As cSpectroscopyTable,
                                     ByVal SelectedColumnNameX As String,
                                     ByVal SelectedColumnNameY As String,
                                     Optional ByVal PreferLastSelectedColumnNames As Boolean = False)
        Me.SetSinglePreviewImage(SpectroscopyTable, SelectedColumnNameX, New List(Of String)({SelectedColumnNameY}), PreferLastSelectedColumnNames)
    End Sub

    ''' <summary>
    ''' Selects the given columns to display them.
    ''' </summary>
    Public Sub SelectColumns(ByVal XColumn As String, ByVal YColumns As List(Of String))
        ' Fill the Column-Selectors.
        Me.bReady = False
        Me.cbX.SetSelectedEntry(XColumn)
        Me.bReady = True
        Me.cbY.SetSelectedColumnNames(YColumns)
    End Sub

    ''' <summary>
    ''' Selects the given columns to display them.
    ''' </summary>
    Public Sub SelectColumns(ByVal XColumn As String, ByVal YColumn As String)
        ' Fill the Column-Selectors.
        Me.bReady = False
        Me.cbX.SetSelectedEntry(XColumn)
        Me.bReady = True
        Me.cbY.SetSelectedEntry(YColumn)
    End Sub

#End Region

#Region "Paint-Function"

    ' Delegate for thread-safe call
    Private Delegate Sub _PaintPreviewImage()

    ' Delegate for thread-safe call
    Private Sub PaintPreviewImage()
        If Me.zPreview.InvokeRequired Then
            Me.zPreview.Invoke(New _PaintPreviewImage(AddressOf PaintPreviewImageThreadSafe))
        Else
            Me.PaintPreviewImageThreadSafe()
        End If
    End Sub

    ''' <summary>
    ''' Draws the Preview-Image.
    ''' </summary>
    Private Sub PaintPreviewImageThreadSafe()
        If Not Me.bReady Then Return

        ' Check, if a Column is selected:
        If Me.cbX.SelectedColumnName = String.Empty Or Me.cbY.SelectedColumnName = String.Empty Then Return

        ' Clear GraphPane
        Me.zPreview.GraphPane.CurveList.Clear()
        Me.zPreview.GraphPane.GraphObjList.Clear()

        Dim ColumnNameX As String = Me.cbX.SelectedColumnName
        Dim ColumnNamesY As New List(Of String)

        Dim SpectroscopyTablePlotIndex As Integer = 0

        ' Plot all SpectroscopyTables
        For Each SpectroscopyTable As cSpectroscopyTable In Me.SpectroscopyTables.Values

            ColumnNamesY.Clear()
            For Each ColumnName As String In Me.cbY.SelectedColumnNames
                If SpectroscopyTable.ColumnExists(ColumnName) Then
                    ColumnNamesY.Add(ColumnName)
                End If
            Next

            ' Check, if columns exist in the spectroscopy-file: If not, proceed with the next file.
            If Not SpectroscopyTable.ColumnExists(ColumnNameX) Or ColumnNamesY.Count <= 0 Then Continue For
            Dim XValues As ReadOnlyCollection(Of Double) = SpectroscopyTable.Column(ColumnNameX).Values

            ' now add each Y column to the graph
            For CurrentColumnIndex As Integer = 0 To ColumnNamesY.Count - 1 Step 1

                ' Get the column-ID
                Dim ColumnNameY As String = ColumnNamesY(CurrentColumnIndex)

                ' Check for valid ColumnID (>= 0)
                Dim YValues As ReadOnlyCollection(Of Double) = SpectroscopyTable.Column(ColumnNameY).Values

                Try
                    ' Add data to the Plot-Point-List
                    Dim list As New ZedGraph.PointPairList
                    For i As Integer = 0 To SpectroscopyTable.Column(ColumnNameY).Values.Count - 1 Step 1

                        ' Check for NaN
                        If Double.IsNaN(XValues(i)) Or
                           Double.IsNaN(YValues(i)) Then Continue For

                        ' Add Values to the Point-List
                        list.Add(XValues(i), YValues(i) + SpectroscopyTablePlotIndex * MultipleSpectraStackOffset)
                    Next

                    ' Paint Graph
                    With Me.zPreview.GraphPane

                        ' If only a single Graph if plotted, add as title the file-name of the SpectroscopyTable.
                        ' Else, add a legend, if not too many files were shown.
                        If Me.SpectroscopyTables.Count = 1 And ColumnNamesY.Count = 1 Then
                            .Title.IsVisible = True
                            .Title.Text = IO.Path.GetFileName(SpectroscopyTable.FullFileName)

                            .Legend.IsVisible = False
                        Else
                            .Title.IsVisible = False
                            .Title.Text = ""

                            ' Check, if more than 5 files are shown in the graph.
                            ' If this is the case, hide the legend, to not squeez the graph too much.
                            .Legend.IsVisible = (Me.SpectroscopyTables.Count <= 5)
                        End If

                        ' Axis-Titles and Types
                        .XAxis.Title.Text = SpectroscopyTable.Column(ColumnNameX).AxisTitle
                        If Me.ckbLogX.Checked Then
                            .XAxis.Type = AxisType.Log
                        Else
                            .XAxis.Type = AxisType.Linear
                        End If
                        .YAxis.Title.Text = SpectroscopyTable.Column(ColumnNameY).AxisTitle
                        If Me.ckbLogY.Checked Then
                            .YAxis.Type = AxisType.Log
                        Else
                            .YAxis.Type = AxisType.Linear
                        End If

                        ' Adapt for multiple columns in the same file.
                        Dim CurveLabel As String
                        If Me.SpectroscopyTables.Count > 1 And ColumnNamesY.Count > 1 Then
                            CurveLabel = SpectroscopyTable.FileNameWithoutPath & " - " & SpectroscopyTable.Column(ColumnNameY).AxisTitle
                        ElseIf Me.SpectroscopyTables.Count = 1 And ColumnNamesY.Count > 1 Then
                            CurveLabel = SpectroscopyTable.Column(ColumnNameY).AxisTitle
                        Else
                            CurveLabel = SpectroscopyTable.FileNameWithoutPath
                        End If

                        ' Determine SymbolType
                        Dim Symbol As SymbolType = Me.GetNextSymbolType(Me._SymbolTypes(SpectroscopyTable.FullFileName), CurrentColumnIndex)

                        ' Determine the graph's color
                        ' If we have just one graph, but multiple plots, also vary the color between the two plots.
                        Dim Color As Color
                        If Me.SpectroscopyTables.Count = 1 Then
                            Color = Me.GetNextColor(Me._PaintColors(SpectroscopyTable.FullFileName), CurrentColumnIndex)
                        Else
                            Color = Me._PaintColors(SpectroscopyTable.FullFileName)
                        End If

                        Dim oLine As ZedGraph.LineItem = .AddCurve(CurveLabel,
                                                                   list,
                                                                   Color,
                                                                   Symbol)

                        ' Graph-Settings
                        With oLine
                            ' Set the symbol of the line
                            .Symbol.Fill = New ZedGraph.Fill(Color)
                            .Symbol.Type = Symbol
                            If Me._SymbolSizes(SpectroscopyTable.FullFileName) <= 0 Then
                                .Symbol.IsVisible = False
                            Else
                                .Symbol.Size = Me._SymbolSizes(SpectroscopyTable.FullFileName)
                            End If

                            ' Set the properties of the line itself
                            .Line.Color = Color
                            .Line.Style = Me._LineStyles(SpectroscopyTable.FullFileName)
                            If Me._LineWidths(SpectroscopyTable.FullFileName) <= 0 Then
                                .Line.IsVisible = False
                            Else
                                .Line.Width = Me._LineWidths(SpectroscopyTable.FullFileName)
                            End If
                        End With

                    End With
                Catch ex As Exception
                    MessageBox.Show("Plotting of the data failed!" & vbCrLf & ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next

            SpectroscopyTablePlotIndex += 1
        Next

        ' Plot a possible stacking offset
        If Me.MultipleSpectraStackOffset <> 0 And SpectroscopyTablePlotIndex > 1 Then
            Dim FormatedStackingOffset As KeyValuePair(Of String, Double) = cUnits.GetPrefix(Me.MultipleSpectraStackOffset)
            Dim StackingString As String = FormatedStackingOffset.Value & " " & FormatedStackingOffset.Key
            Dim StackInformation As New TextObj(My.Resources.rSpectroscopyTableViewer.StackOffsetGraphInformation.Replace("%v", StackingString), 0, 0, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
            Me.zPreview.GraphPane.GraphObjList.Add(StackInformation)
        End If

        Me.zPreview.AxisChange()
        Me.zPreview.Invalidate()
        Me.zPreview.Refresh()

        ' Restore scale, if the property is set
        If Me.AutomaticallyRestoreScaleAfterRedraw Then
            Me.RestoreScale()
        End If
    End Sub

    ''' <summary>
    ''' Restores the scale of the image.
    ''' </summary>
    Public Sub RestoreScale()
        Me.zPreview.RestoreScale(Me.zPreview.GraphPane)
    End Sub

    ''' <summary>
    ''' Repaints the Image
    ''' </summary>
    Public Sub RepaintImage()
        If Not Me.bReady Then Return
        Me.PaintPreviewImage()
    End Sub
#End Region

#Region "Selected Columns Changed, or Column-Display Type Changed (Log, Lin)"

    ''' <summary>
    ''' Simple SelectedIndexChanged Event
    ''' </summary>
    Public Event SelectedIndexChanged()

    ''' <summary>
    ''' Repaint, if X column changes, and save the selected column name.
    ''' </summary>
    Private Sub ColumnSelectorX_SelectedIndexChanged() Handles cbX.SelectedIndexChanged
        If Not Me.bReady Then Return
        My.Settings.LastSpectroscopyPlot_SelectedColumnNameX = Me.cbX.SelectedColumnName
        My.Settings.Save()
        Me.PaintPreviewImage()
        RaiseEvent SelectedIndexChanged()
    End Sub

    ''' <summary>
    ''' Repaint, if Columns changed.
    ''' </summary>
    Private Sub ColumnSelectorY_SelectedIndexChanged() Handles cbY.SelectedIndexChanged
        If Not Me.bReady Then Return
        Me.PaintPreviewImage()
        RaiseEvent SelectedIndexChanged()
    End Sub

    ''' <summary>
    ''' Changes the Scale of the Plot.
    ''' </summary>
    Private Sub ckbLog_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ckbLogX.CheckedChanged, ckbLogY.CheckedChanged
        My.Settings.PreviewBox_LogX = Me.ckbLogX.Checked
        My.Settings.PreviewBox_LogY = Me.ckbLogY.Checked
        My.Settings.Save()
        Me.RepaintImage()
    End Sub

#End Region

#Region "Style-Panel: Plot-Style changes"

    ''' <summary>
    ''' Saves the currently selected line-object to set the style from.
    ''' </summary>
    Private CurrentlySelectedFileName As String = Nothing

    ''' <summary>
    ''' Change the currently selected line to plot.
    ''' </summary>
    Private Sub lbLines_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbLines.SelectedIndexChanged
        If Not Me.bReady Then Return
        If Me.lbLines.SelectedIndex < 0 Then
            Me.CurrentlySelectedFileName = Nothing
            Me.nudSymbolSize.Enabled = False
            Me.nudLineWidth.Enabled = False
            Me.cbSymbolTypes.Enabled = False
            Me.cbLineStyle.Enabled = False
            Me.cbLineColor.Enabled = False
            Return
        Else
            Me.nudSymbolSize.Enabled = True
            Me.nudLineWidth.Enabled = True
            Me.cbSymbolTypes.Enabled = True
            Me.cbLineStyle.Enabled = True
            Me.cbLineColor.Enabled = True
        End If

        ' Turn off change of the curve-parameters.
        Me.bReady = False

        ' Get the current file-name as index
        For Each SpectroscopyTableKV As KeyValuePair(Of String, cSpectroscopyTable) In Me.SpectroscopyTables
            If SpectroscopyTableKV.Value.FileNameWithoutPathAndExtension = Convert.ToString(Me.lbLines.SelectedItem) Then
                Me.CurrentlySelectedFileName = SpectroscopyTableKV.Key
            End If
        Next

        ' set the current-settings
        Me.nudSymbolSize.Value = Me._SymbolSizes(Me.CurrentlySelectedFileName)
        Me.nudLineWidth.Value = Me._LineWidths(Me.CurrentlySelectedFileName)
        For i As Integer = 0 To Me.SymbolArray.Length - 1 Step 1
            If Me.SymbolArray(i) = Me._SymbolTypes(Me.CurrentlySelectedFileName) Then Me.cbSymbolTypes.SelectedIndex = i
        Next
        For i As Integer = 0 To Me.LineStyleArray.Length - 1 Step 1
            If Me.LineStyleArray(i) = Me._LineStyles(Me.CurrentlySelectedFileName) Then Me.cbLineStyle.SelectedIndex = i
        Next
        ' color found
        For i As Integer = 0 To Me.cbLineColor.Items.Count - 1 Step 1
            If CType(Me.cbLineColor.Items(i), Color) = Me._PaintColors(Me.CurrentlySelectedFileName) Then Me.cbLineColor.SelectedIndex = i
        Next

        Me.bReady = True
    End Sub

    ''' <summary>
    ''' Change the size of the selected line symbols.
    ''' </summary>
    Private Sub nudSymbolSize_ValueChanged(sender As Object, e As EventArgs) Handles nudSymbolSize.ValueChanged
        If Not Me.bReady Then Return
        If Me.lbLines.SelectedIndex < 0 Then Return
        If Me.CurrentlySelectedFileName = Nothing Then Return

        ' Set the Symbol-Sizes:
        Me.ChangeSymbolSize(Me.CurrentlySelectedFileName, Me.nudSymbolSize.Value)
        'Me._SymbolSizes(Me.CurrentlySelectedFileName) = Me.nudSymbolSize.Value

        Me.AutomaticallyRestoreScaleAfterRedraw = False
        Me.PaintPreviewImage()
        Me.AutomaticallyRestoreScaleAfterRedraw = True
    End Sub

    ''' <summary>
    ''' Change the width of the line plotted for the curve.
    ''' </summary>
    Private Sub nudLineWidth_ValueChanged(sender As Object, e As EventArgs) Handles nudLineWidth.ValueChanged
        If Not Me.bReady Then Return
        If Me.lbLines.SelectedIndex < 0 Then Return
        If Me.CurrentlySelectedFileName = Nothing Then Return

        ' Set the Line-Width:
        Me.ChangeLineWidth(Me.CurrentlySelectedFileName, Me.nudLineWidth.Value)
        'Me._LineWidths(Me.CurrentlySelectedFileName) = Me.nudLineWidth.Value

        Me.AutomaticallyRestoreScaleAfterRedraw = False
        Me.PaintPreviewImage()
        Me.AutomaticallyRestoreScaleAfterRedraw = True
    End Sub

    ''' <summary>
    ''' Change the style of the line.
    ''' </summary>
    Private Sub cbLineStyle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbLineStyle.SelectedIndexChanged
        If Not Me.bReady Then Return
        If Me.cbLineStyle.SelectedIndex < 0 And Me.cbLineStyle.SelectedIndex >= Me.cbLineStyle.Items.Count Then Return
        If Me.lbLines.SelectedIndex < 0 Then Return
        If Me.CurrentlySelectedFileName = Nothing Then Return

        ' Set the line-style:
        Me.ChangeLineStyle(Me.CurrentlySelectedFileName, Me.LineStyleArray(Me.cbLineStyle.SelectedIndex))
        'Me._LineStyles(Me.CurrentlySelectedFileName) = Me.LineStyleArray(Me.cbLineStyle.SelectedIndex)

        Me.AutomaticallyRestoreScaleAfterRedraw = False
        Me.PaintPreviewImage()
        Me.AutomaticallyRestoreScaleAfterRedraw = True
    End Sub

    ''' <summary>
    ''' Change the color of the line.
    ''' </summary>
    Private Sub cbLineColor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbLineColor.SelectedIndexChanged
        If Not Me.bReady Then Return
        If Me.cbLineColor.SelectedIndex < 0 And Me.cbLineColor.SelectedIndex >= Me.cbLineColor.Items.Count Then Return
        If Me.lbLines.SelectedIndex < 0 Then Return
        If Me.CurrentlySelectedFileName = Nothing Then Return

        ' Set the line-color and symbol-color:
        Me.ChangeColor(Me.CurrentlySelectedFileName, CType(Me.cbLineColor.Items(Me.cbLineColor.SelectedIndex), Color))
        'Me._PaintColors(Me.CurrentlySelectedFileName) = CType(Me.cbLineColor.Items(Me.cbLineColor.SelectedIndex), Color)

        Me.AutomaticallyRestoreScaleAfterRedraw = False
        Me.PaintPreviewImage()
        Me.AutomaticallyRestoreScaleAfterRedraw = True
    End Sub

    ''' <summary>
    ''' Change the symbol-type of the curve.
    ''' </summary>
    Private Sub cbSymbolType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSymbolTypes.SelectedIndexChanged
        If Not Me.bReady Then Return
        If Me.cbSymbolTypes.SelectedIndex < 0 And Me.cbSymbolTypes.SelectedIndex >= Me.cbSymbolTypes.Items.Count Then Return
        If Me.lbLines.SelectedIndex < 0 Then Return
        If Me.CurrentlySelectedFileName = Nothing Then Return

        ' Set the symbol-type:
        Me.ChangeSymbolType(Me.CurrentlySelectedFileName, Me.SymbolArray(Me.cbSymbolTypes.SelectedIndex))
        'Me._SymbolTypes(Me.CurrentlySelectedFileName) = Me.SymbolArray(Me.cbSymbolTypes.SelectedIndex)

        Me.AutomaticallyRestoreScaleAfterRedraw = False
        Me.PaintPreviewImage()
        Me.AutomaticallyRestoreScaleAfterRedraw = True
    End Sub

#End Region

#Region "Point-Selection by the user"

#Region "Delegate Callback-Functions to call specific functions after selection"

    ''' <summary>
    ''' Delegate function to treat the result of a selected range.
    ''' </summary>
    Public Delegate Sub XRangeSelectionCallback(LeftValue As Double, RightValue As Double)

    ''' <summary>
    ''' Callback-Function: If set, the function will be called,
    ''' after the specific range has been selected.
    ''' </summary>
    Public Property CallbackXRangeSelected As XRangeSelectionCallback

    ''' <summary>
    ''' Delegate function to treat the result of a selected range.
    ''' </summary>
    Public Delegate Sub YRangeSelectionCallback(UpperValue As Double, LowerValue As Double)

    ''' <summary>
    ''' Callback-Function: If set, the function will be called,
    ''' after the specific range has been selected.
    ''' </summary>
    Public Property CallbackYRangeSelected As YRangeSelectionCallback

    ''' <summary>
    ''' Delegate function to treat the result of a selected range.
    ''' </summary>
    Public Delegate Sub XValueSelectionCallback(XValue As Double)

    ''' <summary>
    ''' Callback-Function: If set, the function will be called,
    ''' after the specific range has been selected.
    ''' </summary>
    Public Property CallbackXValueSelected As XValueSelectionCallback

    ''' <summary>
    ''' Delegate function to treat the result of a selected range.
    ''' </summary>
    Public Delegate Sub YValueSelectionCallback(YValue As Double)

    ''' <summary>
    ''' Callback-Function: If set, the function will be called,
    ''' after the specific range has been selected.
    ''' </summary>
    Public Property CallbackYValueSelected As YValueSelectionCallback

    ''' <summary>
    ''' Delegate function to treat the result of a selected range.
    ''' </summary>
    Public Delegate Sub XYRangeSelectionCallback(Point_Left As PointPair, Point_Right As PointPair)

    ''' <summary>
    ''' Callback-Function: If set, the function will be called,
    ''' after the specific range has been selected.
    ''' </summary>
    Public Property CallbackXYRangeSelected As XYRangeSelectionCallback

    ''' <summary>
    ''' Delegate function to treat the result of a selected value-pair
    ''' </summary>
    Public Delegate Sub XYValueSelectionCallback(XValue As Double, YValue As Double)

    ''' <summary>
    ''' Callback-Function: If set, the function will be called,
    ''' after the specific range has been selected.
    ''' </summary>
    Public Property CallbackXYValueSelected As XYValueSelectionCallback

    ''' <summary>
    ''' Delegate function to treat the result of a selected data-point
    ''' </summary>
    Public Delegate Sub DataPointSelectionCallback(XValue As Double, YValue As Double, CurveLabel As String)

    ''' <summary>
    ''' Callback-Function: If set, the function will be called,
    ''' after the specific range has been selected.
    ''' </summary>
    Public Property CallbackDataPointSelected As DataPointSelectionCallback


#End Region

#Region "Properties"
    ''' <summary>
    ''' Different Types of Modes that a Mouse-Down-Event
    ''' will initiate. None = Normal Zoom Function.
    ''' </summary>
    Public Enum SelectionModes
        None
        XRange
        YRange
        XYRange
        XValue
        YValue
        XYValue
    End Enum

    Private _PointSelectionMode As SelectionModes = SelectionModes.None

    ''' <summary>
    ''' Keeps Track of all selected Points!
    ''' </summary>
    Private SelectedPoints As New PointPairList
    Private PointSelection_1st As New PointPair
    Private PointSelection_2nd As New PointPair
    Private PointSelection_ClosestDataPoint As New PointPair
    Private PointSelection_ClosestDataPoint_CurveLabel As String

    Private IsMouseDown As Boolean = False
    Private PreviewDrawingSurface As Graphics

    ' Create pen and brush for point selection.
    Private SelectionPen As New Pen(Color.Red, 2) ' Red
    Private SelectionSemiTransparentBrush As New SolidBrush(Color.FromArgb(100, 255, 0, 0)) ' light red

    ' Create pen and brush for highlighting.
    Private HighlightBorderColor As Color = Color.Blue ' blue
    Private HighlightFillColor As Color = Color.FromArgb(100, 0, 0, 255) ' light blue
    Private SpotMarkerWidth As Integer = 6

    ''' <summary>
    ''' Determines, if the Zoom-Function is active, or, if the Zoom-Function
    ''' is replaced by a point-selection-tool.
    ''' </summary>
    <DescriptionAttribute("Determines, if the Zoom-Function is active, or, if the Zoom-Function is replaced by a point-selection-tool."), CategoryAttribute("PB Settings")>
    Public Property PointSelectionMode As SelectionModes
        Get
            Return Me._PointSelectionMode
        End Get
        Set(value As SelectionModes)
            Me._PointSelectionMode = value
            Me.Refresh()
            RaiseEvent PointSelectionModeChanged(Me._PointSelectionMode)
        End Set
    End Property

    ''' <summary>
    ''' Determines, if the PointSelectionMode should be set to "None" after the selection.
    ''' This allows only a single selection procedure.
    ''' </summary>
    <DescriptionAttribute("Determines, if the only a single point-selection procedure should be performed, so that the point-selection-mode is set to None afterwards."), _
     CategoryAttribute("PB Settings")>
    Public Property ClearPointSelectionModeAfterSelection As Boolean = False

#End Region

#Region "Events"

    ''' <summary>
    ''' Event that returns the XY-Range selected
    ''' </summary>
    Public Event PointSelectionChanged_XYRange(Point_Left As PointPair, Point_Right As PointPair)

    ''' <summary>
    ''' Event that returns the X-Range selected
    ''' </summary>
    Public Event PointSelectionChanged_XRange(LeftValue As Double, RightValue As Double)

    ''' <summary>
    ''' Event that returns the Y-Range selected
    ''' </summary>
    Public Event PointSelectionChanged_YRange(HighValue As Double, LowValue As Double)

    ''' <summary>
    ''' Event that returns a single selected XValue.
    ''' </summary>
    Public Event PointSelectionChanged_XValue(XValue As Double)

    ''' <summary>
    ''' Event that returns a single selected YValue.
    ''' </summary>
    Public Event PointSelectionChanged_YValue(YValue As Double)

    ''' <summary>
    ''' Event that returns a single selected XYValue.
    ''' </summary>
    Public Event PointSelectionChanged_XYValue(XValue As Double, YValue As Double)

    ''' <summary>
    ''' Event that returns a single selected DataPoint.
    ''' </summary>
    Public Event PointSelectionChanged_DataPoint(XValue As Double, YValue As Double, CurveLabel As String)

    ''' <summary>
    ''' Event that gets fired, if the selection mode of this control changes.
    ''' </summary>
    Public Event PointSelectionModeChanged(ByVal PointSelectionMode As SelectionModes)

#End Region

#Region "Paint-Event of the Graph, to treat further information in the image"
    ''' <summary>
    ''' add additional information to the zedgraph, before the paint happens.
    ''' </summary>
    Private Sub GraphPaint(sender As Object, e As PaintEventArgs) Handles zPreview.Paint

        ' Clear all graph-objects, additionally added to the graph
        zPreview.MasterPane.GraphObjList.Clear()
        zPreview.GraphPane.GraphObjList.Clear()

        '#############################################################################
        ' Add an information text to the graph, if the point-selection mode is active
        If Me.PointSelectionMode <> SelectionModes.None Then
            ' Create a Text-Object
            Dim Text As TextObj = New TextObj

            ' Set the Text-Content
            Select Case PointSelectionMode
                Case SelectionModes.XRange
                    Text.Text = My.Resources.rSpectroscopyTableViewer.SelectionMode_XRange
                Case SelectionModes.XValue
                    Text.Text = My.Resources.rSpectroscopyTableViewer.SelectionMode_XValue
                Case SelectionModes.XYRange
                    Text.Text = My.Resources.rSpectroscopyTableViewer.SelectionMode_XYRange
                Case SelectionModes.YRange
                    Text.Text = My.Resources.rSpectroscopyTableViewer.SelectionMode_YRange
                Case SelectionModes.YValue
                    Text.Text = My.Resources.rSpectroscopyTableViewer.SelectionMode_YValue
                Case SelectionModes.XYValue
                    Text.Text = My.Resources.rSpectroscopyTableViewer.SelectionMode_XYValue
                Case Else
                    Text.Text = My.Resources.rSpectroscopyTableViewer.SelectionMode_Else
            End Select

            ' rotate the text 
            Text.FontSpec.Angle = 0
            ' Text will be red, bold, and 16 point
            Text.FontSpec.FontColor = Color.White
            Text.FontSpec.IsBold = True
            Text.FontSpec.Size = 16
            ' Disable the border and background fill options for the text
            Text.FontSpec.Border.IsVisible = False
            Text.FontSpec.Fill.IsVisible = True
            Text.FontSpec.Fill.Color = Color.Red
            ' Align the text such the the Left-Top corner is at the specified coordinates
            Text.Location.AlignH = AlignH.Center
            Text.Location.AlignV = AlignV.Top
            Text.Location.CoordinateFrame = CoordType.ChartFraction
            Text.Location.X = 0.5
            Text.Location.Y = 0
            zPreview.GraphPane.GraphObjList.Add(Text)
        End If
        ' END of adding point-selection information text.
        '############################################################################

        '#######################################
        ' Add selected ranges by boxes

        For i As Integer = 0 To Me._HighlightRanges.Count - 1 Step 1

            ' Create storage for the graph-objects.
            Dim GraphObject As GraphObj = Nothing

            Dim FillColor As Color = Me.HighlightBorderColor
            Dim BorderColor As Color = Me.HighlightFillColor
            If Not Me._HighlightRanges(i).BorderColor = Nothing Then BorderColor = Me._HighlightRanges(i).BorderColor
            If Not Me._HighlightRanges(i).FillColor = Nothing Then BorderColor = Me._HighlightRanges(i).FillColor

            Select Case Me._HighlightRanges(i).SelectionMode
                Case SelectionModes.XYRange
                    ' Create a filled box.
                    Dim Box As New BoxObj(Me._HighlightRanges(i).Point_1st.X,
                                          Me._HighlightRanges(i).Point_1st.Y,
                                          Me._HighlightRanges(i).Point_2nd.X - Me._HighlightRanges(i).Point_1st.X,
                                          Me._HighlightRanges(i).Point_2nd.Y - Me._HighlightRanges(i).Point_1st.Y,
                                          BorderColor,
                                          FillColor)
                    Box.Location.CoordinateFrame = CoordType.AxisXYScale
                    GraphObject = Box


                Case SelectionModes.XRange
                    ' Create a filled box.
                    Dim Box As New BoxObj(Me._HighlightRanges(i).Point_1st.X,
                                          0,
                                          Me._HighlightRanges(i).Point_2nd.X - Me._HighlightRanges(i).Point_1st.X,
                                          1,
                                          BorderColor,
                                          FillColor)
                    Box.Location.CoordinateFrame = CoordType.XScaleYChartFraction
                    GraphObject = Box

                Case SelectionModes.YRange
                    ' Create a filled box.
                    Dim Box As New BoxObj(0,
                                          Me._HighlightRanges(i).Point_1st.Y,
                                          1,
                                          Me._HighlightRanges(i).Point_2nd.Y - Me._HighlightRanges(i).Point_1st.Y,
                                          BorderColor,
                                          FillColor)
                    Box.Location.CoordinateFrame = CoordType.XScaleYChartFraction
                    GraphObject = Box

                Case SelectionModes.XValue
                    ' Create a marker line.
                    Dim Line As New LineObj(BorderColor,
                                            Me._HighlightRanges(i).Point_1st.X, 0,
                                            Me._HighlightRanges(i).Point_1st.X, 1)
                    Line.Location.CoordinateFrame = CoordType.XScaleYChartFraction
                    GraphObject = Line

                Case SelectionModes.YValue
                    ' Create a marker line.
                    Dim Line As New LineObj(BorderColor,
                                            0, Me._HighlightRanges(i).Point_1st.X,
                                            100, Me._HighlightRanges(i).Point_1st.X)
                    Line.Location.CoordinateFrame = CoordType.XChartFractionYScale
                    GraphObject = Line

                Case SelectionModes.XYValue
                    ' Create a marker spot.
                    Dim Circle As New EllipseObj(Me._HighlightRanges(i).Point_1st.X - CInt(SpotMarkerWidth / 2),
                                                 Me._HighlightRanges(i).Point_1st.Y - CInt(SpotMarkerWidth / 2),
                                                 SpotMarkerWidth,
                                                 SpotMarkerWidth,
                                                 BorderColor,
                                                 FillColor)
                    Circle.Location.CoordinateFrame = CoordType.AxisXYScale
                    GraphObject = Circle

            End Select

            ' Add the highlighter to the master-pane
            With GraphObject
                .Location.AlignH = AlignH.Left
                .Location.AlignV = AlignV.Top
                .ZOrder = ZOrder.E_BehindCurves
            End With
            If Not GraphObject Is Nothing Then zPreview.GraphPane.GraphObjList.Add(GraphObject)
        Next

        ' END of highlighting selected ranges.
        '#######################################

    End Sub
#End Region

#Region "Function to mark the selected region, and finally return the selected range"

    ''' <summary>
    ''' If the Point-Selection-Mode is enabled, catch the Mouse-Down event, to set the start-point.
    ''' </summary>
    Private Function zPreview_MouseDown(sender As ZedGraphControl, e As MouseEventArgs) As Boolean Handles zPreview.MouseDownEvent
        If Me._PointSelectionMode = SelectionModes.None Then Return False

        Me.IsMouseDown = True

        Dim MousePoint As New PointF(e.X, e.Y)
        Dim GPane As GraphPane = sender.MasterPane.FindChartRect(MousePoint)

        If Not GPane Is Nothing Then
            Dim x As Double
            Dim y As Double

            ' Get Coordinates from Graph
            GPane.ReverseTransform(MousePoint, x, y)
            ' Save Points
            Me.PointSelection_1st.X = x
            Me.PointSelection_1st.Y = y
        Else
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' If the Point-Selection-Mode is enabled, and the Mouse is down, catch the Mouse-Move event,
    ''' to draw the selection-frame (bars or rectangle).
    ''' </summary>
    Private Function zPreview_MouseMove(sender As ZedGraphControl, e As MouseEventArgs) As Boolean Handles zPreview.MouseMoveEvent
        If Me._PointSelectionMode = SelectionModes.None Or Not Me.IsMouseDown Then Return False

        Dim MousePoint As New PointF(e.X, e.Y)
        Dim GPane As GraphPane = sender.MasterPane.FindChartRect(MousePoint)

        If Not GPane Is Nothing Then

            ' Get Drawing Surface
            If Me.PreviewDrawingSurface Is Nothing Then
                Me.PreviewDrawingSurface = zPreview.CreateGraphics
            Else
                Me.PreviewDrawingSurface.Dispose()
                Me.PreviewDrawingSurface = zPreview.CreateGraphics
            End If

            ' From initial point, get screen coordinate from graph coordinate:
            Dim StartPointScreenCoordinate As PointF

            ' Possible storage for the nearest curve
            Dim NearestCurve As CurveItem = Nothing
            Dim NearestPointIndex As Integer = -1

            ' Depending on the selection mode, paint a rectangle or a whole region
            ' of a selection box.
            Select Case Me.PointSelectionMode

                Case SelectionModes.XYRange, SelectionModes.XYValue
                    ' For XYRange paint a real rectangle
                    StartPointScreenCoordinate = GPane.GeneralTransform(Me.PointSelection_1st.X, Me.PointSelection_1st.Y, CoordType.AxisXYScale)

                Case SelectionModes.XRange
                    ' For XRange paint a range
                    StartPointScreenCoordinate = GPane.GeneralTransform(Me.PointSelection_1st.X, 0, CoordType.XScaleYChartFraction)

                Case SelectionModes.YRange
                    ' For YRange paint a range
                    StartPointScreenCoordinate = GPane.GeneralTransform(0, Me.PointSelection_1st.Y, CoordType.XChartFractionYScale)

                Case SelectionModes.XValue
                    ' For XValue paint a line
                    StartPointScreenCoordinate = GPane.GeneralTransform(MousePoint.X, 0, CoordType.XScaleYChartFraction)

                Case SelectionModes.YValue
                    ' For XValue paint a line
                    StartPointScreenCoordinate = GPane.GeneralTransform(0, MousePoint.Y, CoordType.XChartFractionYScale)

            End Select

            ' Create location and size of rectangle.
            Dim width As Integer
            Dim height As Integer

            ' Depending on the selection mode, determine the width and height of the selection rectangle
            Try
                Select Case Me.PointSelectionMode
                    Case SelectionModes.XYRange
                        height = Convert.ToInt32(MousePoint.Y - StartPointScreenCoordinate.Y)
                        width = Convert.ToInt32(MousePoint.X - StartPointScreenCoordinate.X)
                    Case SelectionModes.XRange
                        height = CInt(GPane.Chart.Rect.Height)
                        width = Convert.ToInt32(MousePoint.X - StartPointScreenCoordinate.X)
                    Case SelectionModes.YRange
                        height = Convert.ToInt32(MousePoint.Y - StartPointScreenCoordinate.Y)
                        width = CInt(GPane.Chart.Rect.Width)
                    Case SelectionModes.XValue
                        height = CInt(GPane.Chart.Rect.Height)
                        width = 0
                    Case SelectionModes.YValue
                        height = 0
                        width = CInt(GPane.Chart.Rect.Width)
                End Select
            Catch ex As Exception
                Debug.WriteLine("#ERROR: mSpectroscopyTableViewer: point selection error: " & ex.Message)
                Return False
            End Try

            Dim TopLeft As New Point

            ' Catch wrong drawing directions
            If width >= 0 Then
                TopLeft.X = Convert.ToInt32(StartPointScreenCoordinate.X)
            Else
                TopLeft.X = Convert.ToInt32(MousePoint.X)
                width = -width
            End If
            If height >= 0 Then
                TopLeft.Y = Convert.ToInt32(StartPointScreenCoordinate.Y)
            Else
                TopLeft.Y = Convert.ToInt32(MousePoint.Y)
                height = -height
            End If

            ' Refresh the rest of the GraphPane
            Me.zPreview.Refresh()


            ' Draw the selection borders to the screen
            Select Case Me.PointSelectionMode

                Case SelectionModes.XYRange
                    ' Draw the frame of a rectangle
                    Me.PreviewDrawingSurface.DrawRectangle(Me.SelectionPen, TopLeft.X, TopLeft.Y, width, height)
                    ' Fill the selection rectangle
                    Me.PreviewDrawingSurface.FillRectangle(Me.SelectionSemiTransparentBrush, TopLeft.X, TopLeft.Y, width, height)

                Case SelectionModes.XRange
                    ' draw lines to mark the selection area
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X, TopLeft.Y, TopLeft.X, TopLeft.Y + height)
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X + width, TopLeft.Y, TopLeft.X + width, TopLeft.Y + height)
                    ' Fill the selection rectangle
                    Me.PreviewDrawingSurface.FillRectangle(Me.SelectionSemiTransparentBrush, TopLeft.X, TopLeft.Y, width, height)

                Case SelectionModes.YRange
                    ' draw lines to mark the selection area
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X, TopLeft.Y, TopLeft.X + width, TopLeft.Y)
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X, TopLeft.Y + height, TopLeft.X + width, TopLeft.Y + height)
                    ' Fill the selection rectangle
                    Me.PreviewDrawingSurface.FillRectangle(Me.SelectionSemiTransparentBrush, TopLeft.X, TopLeft.Y, width, height)

                Case SelectionModes.XValue
                    ' draw just a single line
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X, TopLeft.Y, TopLeft.X, TopLeft.Y + height)

                Case SelectionModes.YValue
                    ' draw just a single line
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X, TopLeft.Y, TopLeft.X + width, TopLeft.Y)

                Case SelectionModes.XYValue
                    ' draw a cross hair
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X, TopLeft.Y, TopLeft.X, TopLeft.Y + height) ' X-line
                    Me.PreviewDrawingSurface.DrawLine(Me.SelectionPen, TopLeft.X, TopLeft.Y, TopLeft.X + width, TopLeft.Y) ' Y-line

            End Select

        Else
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' If the Point-Selection-Mode is enabled, catch the Mouse-Up event, to set the end-point,
    ''' and raise the events for a successfull selection, depending on the selection mode.
    ''' </summary>
    Private Function zPreview_MouseUp(sender As ZedGraphControl, e As MouseEventArgs) As Boolean Handles zPreview.MouseUpEvent
        If Me._PointSelectionMode = SelectionModes.None Then Return False

        Me.IsMouseDown = False
        If Not Me.PreviewDrawingSurface Is Nothing Then
            Me.PreviewDrawingSurface.Dispose()
            Me.zPreview.Refresh()
        End If

        Dim MousePoint As New PointF(e.X, e.Y)
        Dim GPane As GraphPane = sender.MasterPane.FindChartRect(MousePoint)

        If Not GPane Is Nothing Then
            Dim x As Double
            Dim y As Double

            ' Get Coordinates from Graph
            GPane.ReverseTransform(MousePoint, x, y)

            ' Save Points
            Me.PointSelection_2nd.X = x
            Me.PointSelection_2nd.Y = y

        Else
            Return False
        End If

        ' Further range processing
        Me.RangeSelected()

        Return True
    End Function

#End Region

#Region "Fire events, depending on the selection modes"

    ''' <summary>
    ''' Depending on the selection mode, this function fires the events
    ''' of a successfull selection.
    ''' </summary>
    Protected Sub RangeSelected()

        ' Raise Events depending on the selection mode
        Select Case Me._PointSelectionMode

            Case SelectionModes.XYRange
                If Me.PointSelection_1st.X < Me.PointSelection_2nd.X Then
                    RaiseEvent PointSelectionChanged_XYRange(Me.PointSelection_1st, Me.PointSelection_2nd)
                    If Not CallbackXYRangeSelected Is Nothing Then Me.Invoke(CallbackXYRangeSelected, Me.PointSelection_1st, Me.PointSelection_2nd)
                Else
                    RaiseEvent PointSelectionChanged_XYRange(Me.PointSelection_2nd, Me.PointSelection_1st)
                    If Not CallbackXYRangeSelected Is Nothing Then Me.Invoke(CallbackXYRangeSelected, Me.PointSelection_2nd, Me.PointSelection_1st)
                End If

            Case SelectionModes.XRange
                If Me.PointSelection_1st.X < Me.PointSelection_2nd.X Then
                    RaiseEvent PointSelectionChanged_XRange(Me.PointSelection_1st.X, Me.PointSelection_2nd.X)
                    If Not CallbackXRangeSelected Is Nothing Then Me.Invoke(CallbackXRangeSelected, Me.PointSelection_1st.X, Me.PointSelection_2nd.X)
                Else
                    RaiseEvent PointSelectionChanged_XRange(Me.PointSelection_2nd.X, Me.PointSelection_1st.X)
                    If Not CallbackXRangeSelected Is Nothing Then Me.Invoke(CallbackXRangeSelected, Me.PointSelection_2nd.X, Me.PointSelection_1st.X)
                End If

            Case SelectionModes.YRange
                If Me.PointSelection_1st.Y > Me.PointSelection_2nd.Y Then
                    RaiseEvent PointSelectionChanged_YRange(Me.PointSelection_1st.Y, Me.PointSelection_2nd.Y)
                    If Not CallbackYRangeSelected Is Nothing Then Me.Invoke(CallbackYRangeSelected, Me.PointSelection_1st.Y, Me.PointSelection_2nd.Y)
                Else
                    RaiseEvent PointSelectionChanged_YRange(Me.PointSelection_2nd.Y, Me.PointSelection_1st.Y)
                    If Not CallbackYRangeSelected Is Nothing Then Me.Invoke(CallbackYRangeSelected, Me.PointSelection_2nd.Y, Me.PointSelection_1st.Y)
                End If

            Case SelectionModes.XValue
                RaiseEvent PointSelectionChanged_XValue(Me.PointSelection_2nd.X)
                If Not CallbackXValueSelected Is Nothing Then Me.Invoke(CallbackXValueSelected, Me.PointSelection_2nd.X)

            Case SelectionModes.YValue
                RaiseEvent PointSelectionChanged_YValue(Me.PointSelection_2nd.Y)
                If Not CallbackYValueSelected Is Nothing Then Me.Invoke(CallbackYValueSelected, Me.PointSelection_2nd.Y)

            Case SelectionModes.XYValue
                RaiseEvent PointSelectionChanged_XYValue(Me.PointSelection_2nd.X, Me.PointSelection_2nd.Y)
                If Not CallbackXYValueSelected Is Nothing Then Me.Invoke(CallbackXYValueSelected, Me.PointSelection_2nd.X, Me.PointSelection_2nd.Y)

        End Select

        If Me.ClearPointSelectionModeAfterSelection Then
            CallbackXRangeSelected = Nothing
            CallbackXValueSelected = Nothing
            CallbackXYRangeSelected = Nothing
            CallbackYRangeSelected = Nothing
            CallbackYValueSelected = Nothing
            CallbackXYValueSelected = Nothing

            Me.PointSelectionMode = SelectionModes.None
        End If

    End Sub

#End Region

    ''' <summary>
    ''' Get the closest point on double-click
    ''' </summary>
    Private Sub zPreview_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles zPreview.MouseDoubleClick

        If sender.GetType Is GetType(ZedGraphControl) Then

            Dim MousePoint As New PointF(e.X, e.Y)
            Dim GPane As GraphPane = DirectCast(sender, ZedGraphControl).MasterPane.FindChartRect(e.Location)

            If Not GPane Is Nothing Then
                Dim NearestCurve As CurveItem = Nothing
                Dim NearestPointIndex As Integer = -1
                GPane.FindNearestPoint(MousePoint, GPane.CurveList, NearestCurve, NearestPointIndex)

                If NearestPointIndex >= 0 And Not NearestCurve Is Nothing Then
                    Me.PointSelection_ClosestDataPoint = NearestCurve(NearestPointIndex)
                    Me.PointSelection_ClosestDataPoint_CurveLabel = NearestCurve.Label.Text
                End If

                RaiseEvent PointSelectionChanged_DataPoint(Me.PointSelection_ClosestDataPoint.X, Me.PointSelection_ClosestDataPoint.Y, Me.PointSelection_ClosestDataPoint_CurveLabel)
                If Not CallbackDataPointSelected Is Nothing Then Me.Invoke(CallbackDataPointSelected, Me.PointSelection_ClosestDataPoint.X, Me.PointSelection_ClosestDataPoint.Y, Me.PointSelection_ClosestDataPoint_CurveLabel)

            End If

        End If
    End Sub

#End Region

#Region "Highlight a certain range"

    ''' <summary>
    ''' Highlight Range
    ''' </summary>
    Private _HighlightRanges As New List(Of HighlightRange)

    ''' <summary>
    ''' Stores information about highlighted ranges.
    ''' </summary>
    Public Structure HighlightRange
        Public SelectionMode As SelectionModes
        Public Point_1st As PointPair
        Public Point_2nd As PointPair
        Public BorderColor As Color
        Public FillColor As Color
        Public Sub New(ByVal SelectionMode As SelectionModes,
                       ByVal Point_1st As PointPair,
                       ByVal Point_2nd As PointPair,
                       Optional ByVal BorderColor As Color = Nothing,
                       Optional ByVal FillColor As Color = Nothing)
            Me.Point_1st = Point_1st
            Me.Point_2nd = Point_2nd
            Me.SelectionMode = SelectionMode
            Me.BorderColor = BorderColor
            Me.FillColor = FillColor
        End Sub
    End Structure

    ''' <summary>
    ''' Show the selection frame to mark a certain area on the image.
    ''' </summary>
    Public Sub AddHighlightRange(ByVal SelectionMode As SelectionModes,
                                 ByVal Point_1st As PointPair,
                                 ByVal Point_2nd As PointPair,
                                 Optional ByVal BorderColor As Color = Nothing,
                                 Optional ByVal FillColor As Color = Nothing,
                                 Optional ByVal RefreshGraph As Boolean = True)

        Select Case SelectionMode
            Case SelectionModes.XRange, SelectionModes.XValue
                Point_1st.Y = 0
                Point_2nd.Y = 1
                If Point_1st.X > Point_2nd.X Then
                    NumericExtensions.ExchangeValues(Point_1st.X, Point_2nd.X)
                End If
            Case SelectionModes.YRange, SelectionModes.YValue
                Point_1st.X = 0
                Point_2nd.X = 1
                If Point_1st.Y > Point_2nd.Y Then
                    NumericExtensions.ExchangeValues(Point_1st.Y, Point_2nd.Y)
                End If
            Case SelectionModes.XYRange, SelectionModes.XYValue
                If Point_1st.X > Point_2nd.X Then
                    NumericExtensions.ExchangeValues(Point_1st.X, Point_2nd.X)
                End If
                If Point_1st.Y > Point_2nd.Y Then
                    NumericExtensions.ExchangeValues(Point_1st.Y, Point_2nd.Y)
                End If

            Case Else
                Return
        End Select

        ' Reformat the coordinates!
        Me._HighlightRanges.Add(New HighlightRange(SelectionMode,
                                                   Point_1st,
                                                   Point_2nd,
                                                   BorderColor,
                                                   FillColor))
        If RefreshGraph Then Me.zPreview.Refresh()
    End Sub

    ''' <summary>
    ''' Clears all highlight-ranges for the next paint.
    ''' </summary>
    Public Sub ClearHighlightRanges()
        Me._HighlightRanges.Clear()
        Me.zPreview.Refresh()
    End Sub

#End Region

#Region "Settings-Panel RIGHT/LEFT - slide in/out"

    ''' <summary>
    ''' Hide on Mouse-Leave.
    ''' </summary>
    Private Sub dpRight_MouseLeave(sender As Object, e As EventArgs) Handles dpRight.MouseLeave_PanelArea
        Me.dpRight.SlideIn()
    End Sub

    ''' <summary>
    ''' Show on Mouse-Enter.
    ''' </summary>
    Private Sub dpRight_MouseEnter(sender As Object, e As EventArgs) Handles dpRight.MouseEnter_PanelArea, panSettings.MouseEnter, lblDataSettings.MouseEnter
        Me.dpRight.SlideOut()
    End Sub

    ''' <summary>
    ''' Hide on Mouse-Leave.
    ''' </summary>
    Private Sub dpLeft_MouseLeave(sender As Object, e As EventArgs) Handles dpLeft.MouseLeave_PanelArea
        Me.dpLeft.SlideIn()
    End Sub

    ''' <summary>
    ''' Show on Mouse-Enter.
    ''' </summary>
    Private Sub dpLeft_MouseEnter(sender As Object, e As EventArgs) Handles dpLeft.MouseEnter_PanelArea, panStyle.MouseEnter, lblStyleSettings.MouseEnter
        Me.dpLeft.SlideOut()
    End Sub

#End Region

#Region "Formatting of the tooltip that is shown if the point values were shown."
    ''' <summary>
    ''' Show formatted value
    ''' </summary>
    Private Function zPreview_PointValueEvent(sender As ZedGraphControl, pane As GraphPane, curve As CurveItem, iPt As Integer) As String Handles zPreview.PointValueEvent
        Dim Point As PointPair = curve(iPt)
        Dim TTString As String = curve.Label.Text & vbCrLf &
                                 "X: " & cUnits.GetPrefix(Point.X).Value.ToString("N3") & cUnits.GetPrefix(Point.X).Key & vbCrLf &
                                 "Y: " & cUnits.GetPrefix(Point.Y).Value.ToString("N3") & cUnits.GetPrefix(Point.Y).Key
        Return TTString
    End Function
#End Region

#Region "Slide-Status changed"

    ''' <summary>
    ''' Slide-Status changed
    ''' </summary>
    Private Sub DockPanel_SlideChanged(CurrentSlideState As DockablePanel.SlideStates) Handles dpRight.SlideChanged, dpLeft.SlideChanged
        ' reset the active input control to nothing
        If CurrentSlideState = DockablePanel.SlideStates.SlidOut Then
            Me.ActiveControl = Nothing
        End If
    End Sub

#End Region

#Region "Stacking of graphs"

    ''' <summary>
    ''' Changes the stacking of multiple graphs.
    ''' </summary>
    Private Sub txtStackValue_TextChanged(ByRef NT As NumericTextbox) Handles txtStackValue.ValidValueChanged
        If Not Me.bReady Then Return
        Me.MultipleSpectraStackOffset = Me.txtStackValue.DecimalValue
    End Sub

#End Region

#Region "Dispose"

    ''' <summary>
    ''' Dispose function, that clears up the mempry
    ''' </summary>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        MyBase.Dispose(Disposing)
        If Disposing Then
            ' TODO: dispose managed state (managed objects).
        End If

        ' TODO: free unmanaged resources (unmanaged objects).
        Me.SelectionPen.Dispose()
        Me.SelectionSemiTransparentBrush.Dispose()
        Me.PreviewDrawingSurface.Dispose()
    End Sub

#End Region

End Class
