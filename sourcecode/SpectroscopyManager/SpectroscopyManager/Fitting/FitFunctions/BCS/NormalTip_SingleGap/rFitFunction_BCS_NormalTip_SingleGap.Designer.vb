﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.34014
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class rFitFunction_BCS_NormalTip_SingleGap
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("SpectroscopyManager.rFitFunction_BCS_NormalTip_SingleGap", GetType(rFitFunction_BCS_NormalTip_SingleGap).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Authors:
        '''Michael Ruby.
        '''</summary>
        Friend Shared ReadOnly Property Authors() As String
            Get
                Return ResourceManager.GetString("Authors", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Complex BCS deconvolution fit of a sample gap convoluted with a tip gap..
        '''</summary>
        Friend Shared ReadOnly Property Description() As String
            Get
                Return ResourceManager.GetString("Description", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to BCS DOS: rho(E) = (E + i d) / [ (E + i d)^2 - D^2 ] convoluted with a system broadening.
        '''</summary>
        Friend Shared ReadOnly Property Formula() As String
            Get
                Return ResourceManager.GetString("Formula", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Normal Tip + SC Sample (1 gap).
        '''</summary>
        Friend Shared ReadOnly Property Name() As String
            Get
                Return ResourceManager.GetString("Name", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to sample gap.
        '''</summary>
        Friend Shared ReadOnly Property Parameter_Delta_Sample() As String
            Get
                Return ResourceManager.GetString("Parameter_Delta_Sample", resourceCulture)
            End Get
        End Property
    End Class
End Namespace