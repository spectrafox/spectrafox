﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.34003
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
    Friend Class rBCSFit_SubGapPeak
        
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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("SpectroscopyManager.rBCSFit_SubGapPeak", GetType(rBCSFit_SubGapPeak).Assembly)
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
        '''  Looks up a localized string similar to SGP amplitude.
        '''</summary>
        Friend Shared ReadOnly Property Amplitude() As String
            Get
                Return ResourceManager.GetString("Amplitude", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Disabled sub-gap-peaks were discovered.
        '''Up to now, these peaks will not get saved in the export-file.
        '''If you want to keep them, please activate them first..
        '''</summary>
        Friend Shared ReadOnly Property DisabledSGPsDiscoveredExport() As String
            Get
                Return ResourceManager.GetString("DisabledSGPsDiscoveredExport", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to disabled sub-gap-peaks discovered.
        '''</summary>
        Friend Shared ReadOnly Property DisabledSGPsDiscoveredExport_Title() As String
            Get
                Return ResourceManager.GetString("DisabledSGPsDiscoveredExport_Title", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SGP +/- ratio.
        '''</summary>
        Friend Shared ReadOnly Property PosNegRatio() As String
            Get
                Return ResourceManager.GetString("PosNegRatio", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Gaussian Peaks.
        '''</summary>
        Friend Shared ReadOnly Property SubGapPeakType_Gauss() As String
            Get
                Return ResourceManager.GetString("SubGapPeakType_Gauss", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Lorentzian Peaks.
        '''</summary>
        Friend Shared ReadOnly Property SubGapPeakType_Lorentz() As String
            Get
                Return ResourceManager.GetString("SubGapPeakType_Lorentz", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SGP width.
        '''</summary>
        Friend Shared ReadOnly Property Width() As String
            Get
                Return ResourceManager.GetString("Width", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SGP X center.
        '''</summary>
        Friend Shared ReadOnly Property XCenter() As String
            Get
                Return ResourceManager.GetString("XCenter", resourceCulture)
            End Get
        End Property
    End Class
End Namespace