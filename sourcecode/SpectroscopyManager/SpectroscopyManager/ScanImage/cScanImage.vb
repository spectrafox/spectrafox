Imports System.ComponentModel
Imports MathNet.Numerics.LinearAlgebra.Double

''' <summary>
''' Class that represents 2D-data recorded with SPMs.
''' Each file can contain several channels.
''' </summary>
Public Class cScanImage
    Implements IDisposable

#Region "Properties"

    ''' <summary>
    ''' Variable that saves the FileObject-Reference from which the ScanImage was created.
    ''' </summary>
    Public BaseFileObject As cFileObject

    ''' <summary>
    ''' Variable that stores a custom given name for that scan-image.
    ''' For normal images it is empty.
    ''' </summary>
    Protected _DisplayName As String = String.Empty

    ''' <summary>
    ''' Returns a name to show it in plots, etc.
    ''' Usually it returns the FileNameWithoutPath, but if
    ''' the <code>_DisplayName</code> is set, it will display this variable.
    ''' </summary>
    Public Property DisplayName As String
        Get
            If Me._DisplayName <> String.Empty Then
                Return Me._DisplayName
            Else
                Return Me.FileNameWithoutPath
            End If
        End Get
        Set(value As String)
            Me._DisplayName = value.Trim
        End Set
    End Property

    <DescriptionAttribute("Shows the full path of the scan image file."),
        CategoryAttribute("File Informations"),
        ReadOnlyAttribute(True)>
    Public Property FullFileName As String

    <DescriptionAttribute("Shows the extracted filename of the scan image file."),
        CategoryAttribute("File Informations"),
        ReadOnlyAttribute(True)>
    Public ReadOnly Property FileNameWithoutPath As String
        Get
            Return System.IO.Path.GetFileName(Me.FullFileName)
        End Get
    End Property

    <DescriptionAttribute("Shows the date the image was recorded."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property RecordDate As Date

    <DescriptionAttribute("Shows the number of pixels in the image in X direction."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ScanPixels_X As Integer

    <DescriptionAttribute("Shows the number of pixels in the image in Y direction."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ScanPixels_Y As Integer

    <DescriptionAttribute("Shows the acquisition time for the image."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ACQ_Time As Double

    <DescriptionAttribute("Shows the range scanned in X direction."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ScanRange_X As Double

    <DescriptionAttribute("Shows the range scanned in Y direction."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ScanRange_Y As Double

    <DescriptionAttribute("Shows the offset of the scan position in X direction orienting at the center of the frame."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ScanOffset_X As Double

    <DescriptionAttribute("Shows the offset of the scan position in Y direction orienting at the center of the frame."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ScanOffset_Y As Double

    <DescriptionAttribute("Shows the angle the image is scanned."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property ScanAngle As Double

    <DescriptionAttribute("Shows the bias voltage at which the image is acquired."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property Bias As Double

    <DescriptionAttribute("Shows the current at which the image is acquired."),
        CategoryAttribute("General Image Properties"),
        ReadOnlyAttribute(True)>
    Public Property Current As Double

    <DescriptionAttribute("Shows the properties of the Z-controller."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerSettingsRawString As String

    <DescriptionAttribute("Shows the name of the Z-controller."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerName As String

    <DescriptionAttribute("Shows the setpoint of the Z-controller."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerSetpoint As Double

    <DescriptionAttribute("Shows the unit of the Z-controller."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerSetpointUnit As String

    <DescriptionAttribute("Shows if the Z-controller has been switched on."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerOn As Boolean

    <DescriptionAttribute("Shows the proportional gain settings (unit: m)."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerProportionalGain As Double

    <DescriptionAttribute("Shows the integral gain settings (m/s)."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerIntegralGain As Double

    <DescriptionAttribute("Shows the time constant of the integral gain (s)."),
        CategoryAttribute("Z-Controller"),
        ReadOnlyAttribute(True)>
    Public Property ZControllerTimeConstant As Double

    <DescriptionAttribute("Shows the multi-pass configuration."),
        CategoryAttribute("multi pass configuration"),
        ReadOnlyAttribute(True)>
    Public Property MultiPassConfig As String


    <DescriptionAttribute("Comment of the user."),
        CategoryAttribute("User Input"),
        ReadOnlyAttribute(True)>
    Public Property Comment As String

    <DescriptionAttribute("Channels recorded in the image."),
        CategoryAttribute("Channels"),
        ReadOnlyAttribute(True)>
    Public ReadOnly Property ScanChannels As Dictionary(Of String, ScanChannel)
        Get
            Return Me._ScanChannels
        End Get
    End Property

    ''' <summary>
    ''' Scan Channel list for this file
    ''' </summary>
    Protected _ScanChannels As New Dictionary(Of String, ScanChannel)

    ''' <summary>
    ''' Checks, if a scan channel with the given name exists.
    ''' </summary>
    Public Function ScanChannelExists(ByVal ChannelName As String) As Boolean
        Return Me._ScanChannels.ContainsKey(ChannelName)
    End Function

    ''' <summary>
    ''' This array stores all properties, that are informative for the user,
    ''' but unimportant for the software to work.
    ''' </summary>
    Protected _GeneralPropertyArray As New Dictionary(Of String, String)

    ''' <summary>
    ''' This array stores all properties, that are informative for the user,
    ''' but unimportant for the software to work.
    ''' </summary>
    <DescriptionAttribute("Properties extracted from the file."),
        CategoryAttribute("General Properties"),
        ReadOnlyAttribute(True)>
    Public ReadOnly Property GeneralPropertyArray As Dictionary(Of String, String)
        Get
            Return Me._GeneralPropertyArray
        End Get
    End Property

    ''' <summary>
    ''' Adds a general property attribute to the property array.
    ''' </summary>
    Public Sub AddGeneralProperty(ByVal Key As String, ByVal Value As String)
        If Me._GeneralPropertyArray.ContainsKey(Key) Then
            ' Replace value:
            Me._GeneralPropertyArray(Key) = Value
        Else
            ' Add a value.
            Me._GeneralPropertyArray.Add(Key, Value)
        End If
    End Sub

#End Region

#Region "ScanImage-Functions"

    ''' <summary>
    ''' Adds a scan-channel to the list of channels.
    ''' </summary>
    Public Sub AddScanChannel(ByRef Channel As ScanChannel)

        ' Check, if the Name already exists, if yes, then add a number in the end.
        Dim iCounter As Integer = 2
        Dim sNameWithoutCounter As String = Channel.Name
        Do While CheckIfChannelNameAlreadyExists(Me._ScanChannels, Channel.Name)
            Channel.Name = sNameWithoutCounter & " (" & iCounter & ")"
            iCounter += 1
        Loop

        ' Add the Column to the Dictionary.
        Me._ScanChannels.Add(Channel.Name, Channel)

        ' Add the event handlers.
        AddHandler Me._ScanChannels(Channel.Name).FiltersModified, AddressOf Me.FilterInChannelChangedEvent
    End Sub

    ''' <summary>
    ''' Returns the saved ScanChannelName-List of Names.
    ''' </summary>
    Public Function GetChannelNameList() As List(Of String)
        Return Me.ScanChannels.Keys.ToList
    End Function

    ''' <summary>
    ''' Returns the saved ScanChannelName-List of Names.
    ''' </summary>
    Public Function GetChannelList() As Dictionary(Of String, ScanChannel)
        Return Me.ScanChannels
    End Function


    ''' <summary>
    ''' Checks, if the ChannelName is just present the given number of times in the ChannelList
    ''' </summary>
    Public Shared Function CheckIfChannelNameAlreadyExists(ByRef ListOfChannels As Dictionary(Of String, ScanChannel),
                                                           ByVal Name As String) As Boolean
        Return ListOfChannels.Keys.Contains(Name)
    End Function

    ''' <summary>
    ''' Takes an Input Coordinate and checks, if the Position lies in the Scan-Range of the Image.
    ''' </summary>
    Public Function CheckIfCoordinateLiesInImage(ByVal XCoordinateToSearch As Double,
                                                 ByVal YCoordinateToSearch As Double) As Boolean
        Return cScanImage.CheckIfCoordinateLiesInImage(XCoordinateToSearch, YCoordinateToSearch,
                                                       Me.ScanOffset_X, Me.ScanOffset_Y,
                                                       Me.ScanRange_X, Me.ScanRange_Y)
    End Function

    ''' <summary>
    ''' Takes an Input Coordinate and Image-Parameters and checks, if the Position lies in the Scan-Range of the Image.
    ''' </summary>
    Public Shared Function CheckIfCoordinateLiesInImage(ByVal XCoordinateToSearch As Double,
                                                        ByVal YCoordinateToSearch As Double,
                                                        ByVal XCoordinateOfImage As Double,
                                                        ByVal YCoordinateOfImage As Double,
                                                        ByVal XRangeOfImage As Double,
                                                        ByVal YRangeOfImage As Double) As Boolean
        If XCoordinateToSearch >= XCoordinateOfImage And XCoordinateToSearch <= XCoordinateOfImage + XRangeOfImage Then
            If YCoordinateToSearch >= YCoordinateOfImage And YCoordinateToSearch <= YCoordinateOfImage + YRangeOfImage Then
                Return True
            End If
        End If
        Return False
    End Function
#End Region

#Region "Event handling"

    ''' <summary>
    ''' Function called, if a filter of a scanchannel has changed.
    ''' </summary>
    Private Sub FilterInChannelChangedEvent()

        ' Raise the fileobject changed event.
        If Not Me.BaseFileObject Is Nothing Then
            Me.BaseFileObject.SaveChangesAsFile(False)
        End If

    End Sub


#End Region

#Region "Disposing"

    ''' <summary>
    ''' Dispose the object.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose

        ' Remove the handlers from all the channels
        For Each SC As ScanChannel In Me._ScanChannels.Values

            ' Remove the filter changed event.
            RemoveHandler SC.FiltersModified, AddressOf Me.FilterInChannelChangedEvent

        Next

    End Sub

#End Region

#Region "ScanChannel Class"
    Public Class ScanChannel

#Region "Scan-Channel properties and data-storage"
        '################################
        ' Properties of the scan-channel 

        ''' <summary>
        ''' Name of the channel. Must be unique per ScanImage.
        ''' </summary>
        Public Property Name As String = "n/a"

        ''' <summary>
        ''' Unit symbol for the unit the values of this channel are given in.
        ''' </summary>
        Public Property UnitSymbol As String = "n/a"

        ''' <summary>
        ''' Unit of the values.
        ''' </summary>
        Public Property Unit As cUnits.UnitType = cUnits.UnitType.Unknown
        Public Property Calibration As Double = 0
        Public Property Offset As Double = 0
        Public Property Bias As Double = 0

        ''' <summary>
        ''' Is this scan channel originally contained in the data file,
        ''' or generated by SpectraFox.
        ''' </summary>
        Public Property IsSpectraFoxGenerated As Boolean = True

        Public Property ScanDirection As ScanDirections = ScanDirections.Forward
        ''' <summary>
        ''' Directions the image is recorded into.
        ''' </summary>
        Public Enum ScanDirections
            Forward
            Backward
        End Enum

        ''' <summary>
        ''' Matrix to store the scan-data.
        ''' </summary>
        Public ScanData As DenseMatrix
#End Region

#Region "Filters and access to filters"
        ''' <summary>
        ''' Storage for the applied filters.
        ''' </summary>
        Protected lFilters As New List(Of iScanImageFilter)

        ''' <summary>
        ''' Get a filter from the list.
        ''' </summary>
        Public Function Filter(Index As Integer) As iScanImageFilter
            If Me.lFilters.Count <= Index Then Return Nothing
            Return Me.lFilters(Index)
        End Function

        ''' <summary>
        ''' Event that gets raised, if a filter is added/removed/modified to the scan-channel.
        ''' </summary>
        Public Event FiltersModified()

        ''' <summary>
        ''' Adds a filter to the list of filters for this scan-channel.
        ''' All the settings have to be applied in advance.
        ''' </summary>
        Public Sub AddFilter(ByRef Filter As iScanImageFilter)
            If Filter.FilterSetupComplete Then
                Me.lFilters.Add(Filter)
                RaiseEvent FiltersModified()
            End If
        End Sub

        ''' <summary>
        ''' Clear all filters.
        ''' </summary>
        Public Sub ClearFilters()
            Me.lFilters.Clear()
            RaiseEvent FiltersModified()
        End Sub

        ''' <summary>
        ''' Adds a filter to the list of filters for this scan-channel.
        ''' All the settings have to be applied in advance.
        ''' </summary>
        Public Sub RemoveFilter(ByRef Filter As iScanImageFilter)
            If Me.lFilters.Contains(Filter) Then
                Me.lFilters.Remove(Filter)
                RaiseEvent FiltersModified()
            End If
        End Sub

        ''' <summary>
        ''' Get the number of filters applied to the filtered version of the image.
        ''' </summary>
        Public Function FilterCount() As Integer
            Return lFilters.Count
        End Function

        ''' <summary>
        ''' Gets a filtered version of the channel (a separate copy, independent of the original channel)
        ''' </summary>
        Public Function FilterChannel_CreateCopy() As ScanChannel
            ' Create a copy of the original Channel
            Dim FilteredChannel As ScanChannel = Me.GetCopy

            For i As Integer = 0 To Me.lFilters.Count - 1 Step 1
                ' apply the filter to the channel
                FilteredChannel = Me.lFilters(i).ApplyFilter(FilteredChannel)
            Next

            ' Return the new channel
            Return FilteredChannel
        End Function

        ''' <summary>
        ''' Gets a filtered version of the channel.
        ''' (Filters directly this channel. So no access to the original data is possible
        ''' in the currently loaded object.)
        ''' </summary>
        Public Function FilterChannel_Direct() As ScanChannel
            For i As Integer = 0 To Me.lFilters.Count - 1 Step 1
                ' apply the filter to the channel
                Me.lFilters(i).ApplyFilter(Me)
            Next
            Return Me
        End Function

        ''' <summary>
        ''' Returns a list with all filters implemented in the program.
        ''' </summary>
        Public Shared Function GetAllScanImageFilters() As List(Of iScanImageFilter)
            Dim APIList As New List(Of iScanImageFilter)

            Try
                ' fill the list of with the interfaces found.
                With APIList
                    Dim APIType = GetType(iScanImageFilter)
                    Dim AllAPIImplementingInterfaces As IEnumerable(Of Type) = AppDomain.CurrentDomain.GetAssemblies() _
                                                                           .SelectMany(Function(s) s.GetTypes()) _
                                                                           .Where(Function(p) APIType.IsAssignableFrom(p) And p.IsClass And Not p.IsAbstract)
                    For Each ImplementingType As Type In AllAPIImplementingInterfaces
                        .Add(DirectCast(System.Activator.CreateInstance(ImplementingType), iScanImageFilter))
                    Next
                End With
            Catch ex As Exception
                Trace.WriteLine("#ERROR: cScanImage.GetAllScanImageFilters: Error on loading all scanimage filters: " & ex.Message)
            End Try

            Return APIList
        End Function
#End Region

#Region "Addon functions (Statistics & Co)"
        ''' <summary>
        ''' Returns the Maximum/Minimum in the Scan-Data
        ''' </summary>
        <DebuggerStepThrough>
        Public Function GetMaximumValue() As Double
            Return cNumericalMethods.GetMaximumValue(Me.ScanData)
        End Function

        ''' <summary>
        ''' Returns the Maximum/Minimum In the Scan-Data
        ''' </summary>
        <DebuggerStepThrough>
        Public Function GetMinimumValue() As Double
            Return cNumericalMethods.GetMinimumValue(Me.ScanData)
        End Function

        ''' <summary>
        ''' Returns the row-index of the first row without NaN content in it.
        ''' </summary>
        Public Function GetFirstRowWithoutNaN() As Integer
            Dim YStart As Integer = 0
            For Y As Integer = 0 To Me.ScanData.RowCount - 1 Step 1
                If Not Me.ScanData.Row(Y).Contains(Double.NaN) Then
                    YStart = Y
                    Exit For
                End If
            Next
            Return YStart
        End Function

        ''' <summary>
        ''' Returns the row-index of the last row without NaN content in it.
        ''' </summary>
        Public Function GetLastRowWithoutNaN() As Integer
            Dim YEnd As Integer = Me.ScanData.RowCount - 1
            For Y As Integer = Me.ScanData.RowCount - 1 To 0 Step -1
                If Not Me.ScanData.Row(Y).Contains(Double.NaN) Then
                    YEnd = Y
                    Exit For
                End If
            Next
            Return YEnd
        End Function

        ''' <summary>
        ''' Returns the column-index of the first column without NaN content in it.
        ''' </summary>
        Public Function GetFirstColumnWithoutNaN() As Integer
            Dim XStart As Integer = 0
            For X As Integer = 0 To Me.ScanData.ColumnCount - 1 Step 1
                If Not Me.ScanData.Column(X).Contains(Double.NaN) Then
                    XStart = X
                    Exit For
                End If
            Next
            Return XStart
        End Function

        ''' <summary>
        ''' Returns the column-index of the last column without NaN content in it.
        ''' </summary>
        Public Function GetLastColumnWithoutNaN() As Integer
            Dim XEnd As Integer = Me.ScanData.ColumnCount - 1
            For X As Integer = Me.ScanData.ColumnCount - 1 To 0 Step -1
                If Not Me.ScanData.Column(X).Contains(Double.NaN) Then
                    XEnd = X
                    Exit For
                End If
            Next
            Return XEnd
        End Function

        ''' <summary>
        ''' Gets the scan-direction
        ''' </summary>
        Public Shared Function GetScanDirectionFromString(ScanDirectionString As String) As ScanDirections
            Select Case ScanDirectionString
                Case ScanDirections.Forward.ToString
                    Return ScanDirections.Forward
                Case ScanDirections.Backward.ToString
                    Return ScanDirections.Backward
                Case Else
                    If ScanDirectionString.ToLower.Contains("back") Then
                        Return ScanDirections.Backward
                    ElseIf ScanDirectionString.ToLower.Contains("for") Then
                        Return ScanDirections.Forward
                    End If
            End Select
            Return ScanDirections.Forward
        End Function
#End Region

#Region "Copy the whole channel"
        ''' <summary>
        ''' Returns a direct hard copy of the ScanChannel
        ''' </summary>
        Public Function GetCopy() As ScanChannel
            Dim ScanChannel As New ScanChannel
            With ScanChannel
                .Calibration = Me.Calibration
                .Name = Me.Name
                .Offset = Me.Offset
                .Unit = Me.Unit
                .UnitSymbol = Me.UnitSymbol
                .ScanDirection = Me.ScanDirection
                .IsSpectraFoxGenerated = Me.IsSpectraFoxGenerated
                If Not Me.ScanData Is Nothing Then
                    .ScanData = DenseMatrix.OfMatrix(Me.ScanData)
                End If
            End With
            Return ScanChannel
        End Function
#End Region

#Region "Mathematical Procedures on the scan-data."
        ''' <summary>
        ''' This function uses spline interpolation to reduce or extend the scan-data
        ''' to a new matrix of the given number of points!
        ''' </summary>
        Public Function InterpolateScanData(ByVal TargetPoints_X As Integer,
                                            ByVal TargetPoints_Y As Integer) As DenseMatrix
            Return cNumericalMethods.SplineInterpolation2D(Me.ScanData, TargetPoints_X, TargetPoints_Y)
        End Function
#End Region

    End Class
#End Region

#Region "Coordinates of the Scan-Data, and values to get by coordinates."

    ''' <summary>
    ''' Returns the location-coordinate of a pixel-coordinate in the scan-image,
    ''' by considering the rotation of the image.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' </summary>
    Public Function GetLocationOfScanData(x As Integer, y As Integer) As cNumericalMethods.Point2D
        Return cNumericalMethods.CoordinateTransform(x * (Me.ScanRange_X / Me.ScanPixels_X),
                                                     y * (Me.ScanRange_Y / Me.ScanPixels_Y),
                                                     Me.ScanOffset_X,
                                                     Me.ScanOffset_Y,
                                                     -Me.ScanAngle)
    End Function

    ''' <summary>
    ''' Returns the location-coordinate of a pixel-coordinate in the scan-image,
    ''' by considering the rotation of the image.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' </summary>
    Public Function GetLocationOfScanData(PixelCoordinate As Point) As cNumericalMethods.Point2D
        Return Me.GetLocationOfScanData(PixelCoordinate.X, PixelCoordinate.Y)
    End Function

    ''' <summary>
    ''' Returns the pixel-coordinate of a location-coordinate in the value-matrix.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' 
    ''' If Coordinate lies not in the ScanFrame it returns (-1,-1)
    ''' </summary>
    Public Function GetCoordinateInValueMatrix(x As Double, y As Double) As Point
        Dim P As cNumericalMethods.Point2D = cNumericalMethods.BackCoordinateTransform(x,
                                                                                       y,
                                                                                       Me.ScanOffset_X,
                                                                                       Me.ScanOffset_Y,
                                                                                       -Me.ScanAngle)

        P.x *= (Me.ScanPixels_X / Me.ScanRange_X)
        P.y *= (Me.ScanPixels_Y / Me.ScanRange_Y)

        If (P.x >= 0 AndAlso P.x < Me.ScanPixels_X) AndAlso
           (P.y >= 0 AndAlso P.y < Me.ScanPixels_Y) Then
            Return New Point(CInt(P.x), CInt(P.y))
        Else
            Return New Point(-1, -1)
        End If

    End Function

    ''' <summary>
    ''' Returns the pixel-coordinate of a location-coordinate in the value-matrix.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' 
    ''' If Coordinate lies not in the ScanFrame it returns (-1,-1)
    ''' </summary>
    Public Function GetCoordinateInValueMatrix(LocationCoordinate As cNumericalMethods.Point2D) As Point
        Return Me.GetCoordinateInValueMatrix(LocationCoordinate.x, LocationCoordinate.y)
    End Function

    ''' <summary>
    ''' Returns the pixel-coordinate of a location-coordinate in the value-matrix.
    ''' Rotation is always given with respect to the upper left corner of the picture.
    ''' 
    ''' If Coordinate lies not in the ScanFrame it returns (-1,-1)
    ''' </summary>
    Public Function GetCoordinateInValueMatrix(LocationCoordinate As MathNet.Numerics.LinearAlgebra.Double.DenseVector) As Point
        Return Me.GetCoordinateInValueMatrix(LocationCoordinate(0), LocationCoordinate(1))
    End Function

#End Region

    '##################################################################################
    '
    '                           SHARED FUNCTIONS
    '
    '##################################################################################

#Region "Combine several scan channels to one big scan-channel image!"

    ''' <summary>
    ''' Combination modes of the scan channel combination.
    ''' </summary>
    Public Enum ScanChannelCombinationMode
        Override = 0
        Summation = 1
        Multiplication = 3
        Division = 4
    End Enum

    ''' <summary>
    ''' Returns the combination mode from the integer defined by the ENUM.
    ''' </summary>
    Public Shared Function GetCombinationModeFromInt(ByVal CombinationMode As Integer) As ScanChannelCombinationMode
        Select Case CombinationMode
            Case ScanChannelCombinationMode.Override
                Return ScanChannelCombinationMode.Override
            Case ScanChannelCombinationMode.Summation
                Return ScanChannelCombinationMode.Summation
            Case ScanChannelCombinationMode.Multiplication
                Return ScanChannelCombinationMode.Multiplication
            Case ScanChannelCombinationMode.Division
                Return ScanChannelCombinationMode.Division
            Case Else
                Return ScanChannelCombinationMode.Override
        End Select
    End Function

    ''' <summary>
    ''' Takes a list of scan-channels and creates a combined scan-channel with a
    ''' big matrix containing the data of each scan-channel at the defined location.
    ''' The rest of the data is filled with Double.NaN.
    ''' 
    ''' Combination of multiple images by default occurs in Override-Mode.
    ''' If another mode is choosen, then there has to be a factor for each scan image given.
    ''' </summary>
    Public Shared Function CombineScanChannels(ByRef ScanImages As List(Of cScanImage),
                                               ByVal ChannelNameToPlot As String,
                                               ByVal Width As Integer,
                                               ByVal Height As Integer,
                                               Optional ByVal Mode As ScanChannelCombinationMode = ScanChannelCombinationMode.Override,
                                               Optional ByVal CombinationFactors As List(Of Double) = Nothing) As cScanImage

        If Mode <> ScanChannelCombinationMode.Override Then
            If CombinationFactors Is Nothing OrElse CombinationFactors.Count <> ScanImages.Count Then
                Throw New ArgumentException("Combination of images is only possible with given factors for each image.")
            End If
        End If

        ' Create output channel.
        Dim OutputScanImage As New cScanImage
        Dim OutputScanChannel As New cScanImage.ScanChannel

        ' Get the dimensions of the new scan-channel,
        ' by going through all the scan-channels, and getting the extremal values.
        Dim ScanImage_XMax As Double = Double.MinValue
        Dim ScanImage_XMin As Double = Double.MaxValue
        Dim ScanImage_YMax As Double = Double.MinValue
        Dim ScanImage_YMin As Double = Double.MaxValue

        Dim UnrotatedLocation As cNumericalMethods.Point2D
        For i As Integer = 0 To ScanImages.Count - 1 Step 1

            For x As Integer = 0 To ScanImages(i).ScanPixels_X - 1 Step 1
                For y As Integer = 0 To ScanImages(i).ScanPixels_Y - 1 Step 1

                    ' Get the unrotated values.
                    UnrotatedLocation = ScanImages(i).GetLocationOfScanData(x, y)

                    ' Compare the location.
                    ScanImage_XMax = Math.Max(ScanImage_XMax, UnrotatedLocation.x)
                    ScanImage_XMin = Math.Min(ScanImage_XMin, UnrotatedLocation.x)
                    ScanImage_YMax = Math.Max(ScanImage_YMax, UnrotatedLocation.y)
                    ScanImage_YMin = Math.Min(ScanImage_YMin, UnrotatedLocation.y)

                Next
            Next

        Next

        ' Get the new channel dimensions
        Dim ChannelDimensionsInScale_Width As Double = (ScanImage_XMax - ScanImage_XMin)
        Dim ChannelDimensionsInScale_Height As Double = (ScanImage_YMax - ScanImage_YMin)

        ' Fill the new scan-image with the offset and pixel-dimension information
        With OutputScanImage
            .ScanAngle = 0
            .ScanOffset_X = ScanImage_XMin
            .ScanOffset_Y = ScanImage_YMax
            .ScanRange_X = ChannelDimensionsInScale_Width
            .ScanRange_Y = ChannelDimensionsInScale_Height
            .ScanPixels_X = Width
            .ScanPixels_Y = Height
        End With

        ' Create the new value-matrix
        Dim ValueMatrix As DenseMatrix = DenseMatrix.Create(Height, Width, Double.NaN)
        ' Create the new matrix, which stores, if a value has been plotted already.
        Dim FilledMatrix(Height - 1, Width - 1) As Boolean
        ' Create the new matrix, which stores, if a value has been plotted already.
        Dim CombinationMatrix(Height - 1, Width - 1, ScanImages.Count) As Boolean

        ' define some temporary variables needed in the loop.
        Dim DataPointPixelCoordinateInValueMatrix As Point
        Dim DataPointLocationCoordinateInSource As cNumericalMethods.Point2D
        Dim C As cScanImage.ScanChannel
        Dim RangePerPixel_XNew As Double = OutputScanImage.ScanRange_X / OutputScanImage.ScanPixels_X
        Dim RangePerPixel_YNew As Double = OutputScanImage.ScanRange_Y / OutputScanImage.ScanPixels_Y
        Dim RangePerPixel_XSource As Double
        Dim RangePerPixel_YSource As Double
        Dim XPlotCoordinate As Integer
        Dim YPlotCoordinate As Integer
        Dim PixelMultiplierX As Integer
        Dim PixelMultiplierY As Integer

        ' If we are in an image combination mode other than override,
        ' we only plot common pixels in all images. For this we use the FilledMatrix
        ' to obtain only pixels that are filled by all images.
        If Mode <> ScanChannelCombinationMode.Override Then

            For i As Integer = 0 To ScanImages.Count - 1 Step 1

                ' Get the range per pixel, to plot more than one pixel in the 
                ' target matrix, if we have to perform an upscaling.
                RangePerPixel_XSource = ScanImages(i).ScanRange_X / ScanImages(i).ScanPixels_X
                RangePerPixel_YSource = ScanImages(i).ScanRange_Y / ScanImages(i).ScanPixels_Y

                ' Now if we do an upscaling place also the values in a range around the new point.
                ' This we do by a for loop, that modifies the coordinates in a square around the point.
                PixelMultiplierX = CInt(Math.Ceiling(RangePerPixel_XSource / RangePerPixel_XNew - 1))
                If PixelMultiplierX < 0 Then PixelMultiplierX = 0
                PixelMultiplierY = CInt(Math.Ceiling(RangePerPixel_YSource / RangePerPixel_YNew - 1))
                If PixelMultiplierY < 0 Then PixelMultiplierY = 0

                For x As Integer = 0 To ScanImages(i).ScanPixels_X - 1 Step 1
                    For y As Integer = 0 To ScanImages(i).ScanPixels_Y - 1 Step 1

                        ' Get the plot-location of the point.
                        DataPointLocationCoordinateInSource = ScanImages(i).GetLocationOfScanData(x, y)
                        DataPointPixelCoordinateInValueMatrix = OutputScanImage.GetCoordinateInValueMatrix(DataPointLocationCoordinateInSource)

                        For yArea As Integer = -PixelMultiplierY To PixelMultiplierY Step 1
                            For xArea As Integer = -PixelMultiplierX To PixelMultiplierX Step 1

                                XPlotCoordinate = DataPointPixelCoordinateInValueMatrix.X + xArea
                                YPlotCoordinate = DataPointPixelCoordinateInValueMatrix.Y + yArea

                                ' Increase the counter of each pixel in the big value-matrix, if the coordinates are valid.
                                If XPlotCoordinate >= 0 AndAlso YPlotCoordinate >= 0 AndAlso
                                   XPlotCoordinate < Width AndAlso YPlotCoordinate < Height Then

                                    If Not CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) Then
                                        CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) = True
                                    End If

                                End If

                            Next
                        Next

                    Next
                Next
            Next

            Dim PresentInAll As Boolean
            For x As Integer = 0 To Width - 1 Step 1
                For y As Integer = 0 To Height - 1 Step 1
                    PresentInAll = True

                    ' Search for TRUE in all.
                    For i As Integer = 0 To ScanImages.Count - 1 Step 1
                        If Not CombinationMatrix(y, x, i) Then
                            PresentInAll = False
                            Exit For
                        End If
                    Next

                    ' If not TRUE in all, set all to false
                    If Not PresentInAll Then
                        For i As Integer = 0 To ScanImages.Count - 1 Step 1
                            CombinationMatrix(y, x, i) = False
                        Next
                    End If
                Next
            Next

        End If

            ' Ok, now go through all the scan-images and plot their data to the new value-matrix.
            For i As Integer = 0 To ScanImages.Count - 1 Step 1

            ' Get a reference to the scan-channel to deal with
            C = ScanImages(i).ScanChannels(ChannelNameToPlot)

            ' Get the range per pixel, to plot more than one pixel in the 
            ' target matrix, if we have to perform an upscaling.
            RangePerPixel_XSource = ScanImages(i).ScanRange_X / ScanImages(i).ScanPixels_X
            RangePerPixel_YSource = ScanImages(i).ScanRange_Y / ScanImages(i).ScanPixels_Y

            ' Now if we do an upscaling place also the values in a range around the new point.
            ' This we do by a for loop, that modifies the coordinates in a square around the point.
            PixelMultiplierX = CInt(Math.Ceiling(RangePerPixel_XSource / RangePerPixel_XNew - 1))
            If PixelMultiplierX < 0 Then PixelMultiplierX = 0
            PixelMultiplierY = CInt(Math.Ceiling(RangePerPixel_YSource / RangePerPixel_YNew - 1))
            If PixelMultiplierY < 0 Then PixelMultiplierY = 0

            For x As Integer = 0 To ScanImages(i).ScanPixels_X - 1 Step 1
                For y As Integer = 0 To ScanImages(i).ScanPixels_Y - 1 Step 1

                    ' Get the plot-location of the point.
                    DataPointLocationCoordinateInSource = ScanImages(i).GetLocationOfScanData(x, y)
                    DataPointPixelCoordinateInValueMatrix = OutputScanImage.GetCoordinateInValueMatrix(DataPointLocationCoordinateInSource)

                    ' Only place the values, if the value is valid and not NaN.
                    If Not Double.IsNaN(C.ScanData(y, x)) Then

                        For yArea As Integer = -PixelMultiplierY To PixelMultiplierY Step 1
                            For xArea As Integer = -PixelMultiplierX To PixelMultiplierX Step 1

                                XPlotCoordinate = DataPointPixelCoordinateInValueMatrix.X + xArea
                                YPlotCoordinate = DataPointPixelCoordinateInValueMatrix.Y + yArea

                                ' Place the value into the big value-matrix, if the coordinates are valid.
                                If XPlotCoordinate >= 0 AndAlso YPlotCoordinate >= 0 AndAlso
                                   XPlotCoordinate < Width AndAlso YPlotCoordinate < Height Then

                                    ' Add the data to the value-matrix, depending on the combination mode.
                                    Select Case Mode
                                        Case ScanChannelCombinationMode.Override

                                            ' Ignore points already filled once
                                            If FilledMatrix(YPlotCoordinate, XPlotCoordinate) Then
                                                Continue For
                                            Else
                                                FilledMatrix(YPlotCoordinate, XPlotCoordinate) = True
                                            End If

                                            ValueMatrix(YPlotCoordinate, XPlotCoordinate) = C.ScanData(y, x)

                                        Case ScanChannelCombinationMode.Summation

                                            If CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) Then
                                                If Not Double.IsNaN(ValueMatrix(YPlotCoordinate, XPlotCoordinate)) Then
                                                    ValueMatrix(YPlotCoordinate, XPlotCoordinate) += CombinationFactors(i) * C.ScanData(y, x)
                                                Else
                                                    ValueMatrix(YPlotCoordinate, XPlotCoordinate) = CombinationFactors(i) * C.ScanData(y, x)
                                                End If
                                                CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) = False
                                            End If

                                        Case ScanChannelCombinationMode.Multiplication

                                            If CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) Then
                                                If Not Double.IsNaN(ValueMatrix(YPlotCoordinate, XPlotCoordinate)) Then
                                                    ValueMatrix(YPlotCoordinate, XPlotCoordinate) *= CombinationFactors(i) * C.ScanData(y, x)
                                                Else
                                                    ValueMatrix(YPlotCoordinate, XPlotCoordinate) = CombinationFactors(i) * C.ScanData(y, x)
                                                End If
                                                CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) = False
                                            End If

                                        Case ScanChannelCombinationMode.Division

                                            If CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) Then
                                                If Not Double.IsNaN(ValueMatrix(YPlotCoordinate, XPlotCoordinate)) Then
                                                    Dim DivisionValue As Double = CombinationFactors(i) * C.ScanData(y, x)
                                                    If DivisionValue <> 0 Then
                                                        ValueMatrix(YPlotCoordinate, XPlotCoordinate) /= DivisionValue
                                                    Else
                                                        ValueMatrix(YPlotCoordinate, XPlotCoordinate) = Double.NaN
                                                    End If
                                                Else
                                                    ValueMatrix(YPlotCoordinate, XPlotCoordinate) = CombinationFactors(i) * C.ScanData(y, x)
                                                End If
                                                CombinationMatrix(YPlotCoordinate, XPlotCoordinate, i) = False
                                            End If

                                    End Select

                                End If

                            Next
                        Next
                    End If

                Next
            Next

        Next

        ' Finally set the properties to the output channel.
        If ScanImages.Count > 0 Then
            With OutputScanChannel

                .Name = ChannelNameToPlot
                .IsSpectraFoxGenerated = True
                .ScanDirection = ScanChannel.ScanDirections.Forward

                ' Set the value-matrix
                .ScanData = ValueMatrix

                ' Copy some values from the first scan-channel to plot.
                C = ScanImages(0).ScanChannels(ChannelNameToPlot)
                .Unit = C.Unit
                .UnitSymbol = C.UnitSymbol
                .Bias = C.Bias
                .Calibration = C.Calibration
                .Offset = C.Offset

            End With

        End If

        With OutputScanImage
            .AddScanChannel(OutputScanChannel)
            .DisplayName = My.Resources.rScanImage.CombinedScanImage.Replace("%n", ScanImages.Count.ToString("N0"))
        End With
        Return OutputScanImage
    End Function

#End Region

#Region "Get common features between different ScanImages"
    ''' <summary>
    ''' Takes several scan-images and return the channel-names that the files have in common.
    ''' </summary>
    Public Shared Function GetCommonColumns(ByRef ScanImageList As List(Of cScanImage)) As List(Of String)
        Dim ListOfCommonChannelNames As New List(Of String)

        ' List to count the channels by name to determine all common channels
        Dim ListOfCommonChannels As New Dictionary(Of String, Integer)

        ' Run through all Spectroscopy-Tables and increase
        ' the presence-counter for each Column in the Spectroscopy-File
        For i As Integer = 0 To ScanImageList.Count - 1 Step 1
            For Each ChannelName As String In ScanImageList(i).GetChannelNameList
                ' Save Column in the Counter-List
                If ListOfCommonChannels.ContainsKey(ChannelName) Then
                    ListOfCommonChannels(ChannelName) += 1
                Else
                    ListOfCommonChannels.Add(ChannelName, 1)
                End If
            Next
        Next

        ' Now extract all Column-Names into the Combobox, that exist in each Spectroscopy-Table
        For Each Col As KeyValuePair(Of String, Integer) In ListOfCommonChannels
            If Col.Value = ScanImageList.Count Then
                ListOfCommonChannelNames.Add(Col.Key)
            End If
        Next

        Return ListOfCommonChannelNames
    End Function
#End Region


End Class
