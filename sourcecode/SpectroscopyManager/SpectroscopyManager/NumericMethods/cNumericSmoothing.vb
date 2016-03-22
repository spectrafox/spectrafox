Partial Public Class cNumericalMethods

    ''' <summary>
    ''' Returns a list with all smoothing routines implemented in the program or in loaded plugins.
    ''' </summary>
    Public Shared Function GetAllLoadableSmoothingMethods() As List(Of Type)
        Dim APIList As New List(Of Type)

        Try
            ' fill the list of with the interfaces found.
            With APIList
                Dim APIType = GetType(iNumericSmoothingFunction)
                Dim AllAPIImplementingInterfaces As IEnumerable(Of Type) = AppDomain.CurrentDomain.GetAssemblies() _
                                                                       .SelectMany(Function(s) s.GetTypes()) _
                                                                       .Where(Function(p) APIType.IsAssignableFrom(p) And p.IsClass And Not p.IsAbstract)
                'For Each ImplementingType As Type In AllAPIImplementingInterfaces
                '.Add(DirectCast(System.Activator.CreateInstance(ImplementingType), iFileImport_SpectroscopyTable))
                'Next
                APIList = AllAPIImplementingInterfaces.ToList
            End With
        Catch ex As Exception
            Trace.WriteLine("#ERROR: cNumericalMethods.GetAllLoadableSmoothingMethods: Error on loading: " & ex.Message)
        End Try

        Return APIList
    End Function

    ''' <summary>
    ''' Returns a SmoothingMethod from the given type.
    ''' </summary>
    Public Shared Function GetSmoothingMethodByType(SmoothingMethodType As Type) As iNumericSmoothingFunction
        Return CType(Activator.CreateInstance(SmoothingMethodType), iNumericSmoothingFunction)
    End Function

    ''' <summary>
    ''' Returns a SmoothingMethod from the given type-string.
    ''' Returns nothing, if the type could not be found.
    ''' </summary>
    Public Shared Function GetSmoothingMethodFromString(SmoothingMethodType As String) As Type
        Dim SMTypes As List(Of Type) = GetAllLoadableSmoothingMethods()
        For i As Integer = 0 To SMTypes.Count - 1 Step 1
            If SMTypes(i).ToString = SmoothingMethodType Then
                Return SMTypes(i)
            End If
        Next
        Return Nothing
    End Function

End Class