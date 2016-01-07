Public Module cFormHelper
    ''' <summary>
    ''' Returns the parent form of a control (the top most parent)
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function TopMostParent(C As Control) As Control
        Dim Parent As Control = C
        While True
            If Parent.Parent Is Nothing Then Exit While
            Parent = Parent.Parent
        End While
        Return Parent
    End Function
End Module

