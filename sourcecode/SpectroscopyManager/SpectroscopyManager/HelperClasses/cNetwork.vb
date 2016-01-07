Imports System.Net

Public Class cNetwork

    ''' <summary>
    ''' Returns the fully qualified domain name.
    ''' </summary>
    Public Shared Function GetFQDN() As String

        Dim domainName As String = NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName
        Dim hostName As String = Dns.GetHostName()
        Dim fqdn As String = ""
        If Not hostName.Contains(domainName) Then
            fqdn = hostName + "." + domainName
        Else
            fqdn = hostName
        End If

        Return fqdn
    End Function

End Class
