Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports Amib.Threading
Imports System.Management

''' <summary>
''' Server/Client Socket Class to spread work / such as data-fitting over the network.
''' </summary>
Public Class cComputingCloud

    Public Const UDPBroadcastIP As String = "231.1.2.3"
    Public Const UDPBroadcastPort As Integer = 15641
    Public Const TCPServerPort As Integer = 10247

#Region "Cloud -Member definition and List"

    ''' <summary>
    ''' Keeps track of all cloud-members.
    ''' List gained by sending UDP-Broadcasts.
    ''' </summary>
    Private _CloudMembers As New Dictionary(Of String, CloudMember)

    ''' <summary>
    ''' Keeps track of all cloud-members.
    ''' List gained by sending UDP-Broadcasts.
    ''' </summary>
    Public ReadOnly Property CloudMembers As Dictionary(Of String, CloudMember)
        Get
            Return Me._CloudMembers
        End Get
    End Property

    ''' <summary>
    ''' Represents a member in the computing cloud.
    ''' </summary>
    Public Class CloudMember

        ''' <summary>
        ''' IP-Endpoint of the Cloud-Member
        ''' </summary>
        Public Property IP As IPAddress

        ''' <summary>
        ''' Saves the version of the client.
        ''' </summary>
        Public Property ClientVersion As String

        ''' <summary>
        ''' Count of the available threads for the computing cloud.
        ''' </summary>
        Public Property ThreadCountAvailable As Integer

        ''' <summary>
        ''' Name of the CPU
        ''' </summary>
        Public Property CPUName As String

        ''' <summary>
        ''' Name of the Computer
        ''' </summary>
        Public Property ComputerName As String

    End Class

#End Region

#Region "Thread Pool Definition"

    ''' <summary>
    ''' Create the new Smart-Thread-Pool instance for this data-browser window.
    ''' </summary>
    Private SmartThreadPool As SmartThreadPool

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor for the initial settings.
    ''' </summary>
    Public Sub New()

        ' Calculate the maximum thread-count to use for the computing cloud.
        ' By default, use half of the processor, to allow to continue do other work in parallel.
        Dim MaximumThreadCount As Integer = Environment.ProcessorCount
        If MaximumThreadCount Mod 2 = 0 Then
            MaximumThreadCount = CInt(MaximumThreadCount / 2)
        End If

        ' Initialize the Thread-Pool
        Dim StartInfo As New STPStartInfo
        StartInfo.MaxWorkerThreads = MaximumThreadCount
        Me.SmartThreadPool = New SmartThreadPool(StartInfo)

    End Sub

#End Region

#Region "Communication State-Object"

    ''' <summary>
    ''' State object for reading data asynchronously
    ''' </summary>
    Public Class StateObject
        ''' <summary>
        ''' Partner socket.
        ''' </summary>
        Public workSocket As Socket = Nothing

        ''' <summary>
        ''' Size of receive buffer.
        ''' </summary>
        Public Const BufferSize As Integer = 1024

        ''' <summary>
        ''' Receive buffer.
        ''' </summary>
        Public buffer(BufferSize) As Byte

        ''' <summary>
        ''' Received data string.
        ''' </summary>
        Public sb As New StringBuilder

        ''' <summary>
        ''' Callback called after an async response has been recieved.
        ''' </summary>
        Delegate Sub ResponseRecievedCallback(Response As String)
    End Class

    ''' <summary>
    ''' State object for reading data asynchronously
    ''' </summary>
    Public Class UDPState
        ''' <summary>
        ''' Partner socket.
        ''' </summary>
        Public workSocket As UdpClient = Nothing

        ''' <summary>
        ''' EndPoint.
        ''' </summary>
        Public EndPoint As IPEndPoint = Nothing
    End Class

#End Region

#Region "Start / Stop Computing Cloud"

    ''' <summary>
    ''' Starts the UDP Multicast and the TCP Server.
    ''' </summary>
    Public Function Start() As Boolean
        ' start main servers
        If Not Me.StartBroadcastForSpectraFox() Then Return False
        If Not Me.StartTCPCommunicationServer Then Return False

        ' Start secondary initialization processes
        Me.AskAroundForOtherSpectraFoxes()

        Return True
    End Function

    ''' <summary>
    ''' Stops the UDP Multicast and the TCP Server.
    ''' </summary>
    Public Function [Stop]() As Boolean
        ' stop main servers
        Me.EndBroadcastForSpectraFox()
        Me.StopTCPCommunicationServer()

        ' Start secondary initialization processes
        Me.AskAroundForOtherSpectraFoxes()

        Return True
    End Function

#End Region

#Region "Async TCP-Server to communicate the actual cloud computing"

    ''' <summary>
    ''' TCP-Socket used for listening to computation tasks
    ''' </summary>
    Private TCPServerSocket As Socket

    ''' <summary>
    ''' This server waits for a connection and then uses asychronous operations to
    ''' accept the connection, get data from the connected client.
    ''' It is responsible for launching all higher communication with other clients.
    ''' </summary>
    Private Function StartTCPCommunicationServer() As Boolean

        ' Establish the local endpoint for the socket.
        Dim IPHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
        Dim IPAddress As IPAddress = IPHostInfo.AddressList(0)
        Dim LocalEndPoint As New IPEndPoint(ipAddress, TCPServerPort)

        ' Create a TCP/IP socket.
        TCPServerSocket = New Socket(IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        ' Bind the socket to the local endpoint and listen for incoming connections.
        Try
            TCPServerSocket.Bind(LocalEndPoint)
            TCPServerSocket.Listen(100)

            ' Start an asynchronous socket to listen for connections.
            TCPServerSocket.BeginAccept(New AsyncCallback(AddressOf TCPServerAcceptCallback), TCPServerSocket)

            Return True
        Catch ex As Exception
            MessageBox.Show(My.Resources.rComputingCloud.TCPServer_CouldNotBindToAddress.Replace("%e", ex.Message),
                           My.Resources.rComputingCloud.TCPServer_CouldNotBindToAddress_Title,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Shutdowns the TCP server
    ''' </summary>
    Private Function StopTCPCommunicationServer() As Boolean
        ' Bind the socket to the local endpoint and listen for incoming connections.
        Try
            TCPServerSocket.Close()
            TCPServerSocket.Dispose()
            
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Connection-Acceptance Callback of the TCP-Server.
    ''' </summary>
    Protected Sub TCPServerAcceptCallback(ByVal ar As IAsyncResult)
        Try
            ' Get the socket that handles the client request.
            Dim Listener As Socket = CType(ar.AsyncState, Socket)

            ' End the operation.
            Dim Handler As Socket = Listener.EndAccept(ar)

            ' Create the state object for the async receive.
            Dim State As New StateObject
            State.workSocket = Handler
            Handler.BeginReceive(State.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf TCPServerReadCallback), State)

            ' Start another asynchronous socket to listen for connections again.
            Listener.BeginAccept(New AsyncCallback(AddressOf TCPServerAcceptCallback), Listener)
        Catch ex As ObjectDisposedException
            ' Server Socket already disposed
        End Try
    End Sub

    ''' <summary>
    ''' Data read callback of the TCP-Server.
    ''' </summary>
    Protected Sub TCPServerReadCallback(ByVal ar As IAsyncResult)
        Try
            Dim Content As String = String.Empty

            ' Retrieve the state object and the handler socket
            ' from the asynchronous state object.
            Dim State As StateObject = CType(ar.AsyncState, StateObject)
            Dim Handler As Socket = State.workSocket

            ' Read data from the client socket. 
            Dim BytesRead As Integer = Handler.EndReceive(ar)

            If BytesRead > 0 Then
                ' There  might be more data, so store the data received so far.
                State.sb.Append(Encoding.ASCII.GetString(State.buffer, 0, BytesRead))

                ' Check for end-of-file tag. If it is not there, read 
                ' more data.
                Content = State.sb.ToString()
                If Content.IndexOf("<EOF>") > -1 Then
                    Content = Content.Remove(Content.Length - "<EOF>".Length)

                    '################################################
                    If Content.StartsWith("#AREYOUIDLE#") Then
                        Me.TCPServerSend(Handler, Me.SmartThreadPool.IsIdle.ToString & "<EOF>")
                    End If


                    '################################################
                Else
                    ' Not all data received. Get more.
                    Handler.BeginReceive(State.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf TCPServerReadCallback), State)
                End If
            End If
        Catch ex As ObjectDisposedException
            ' Server Socket already disposed
        End Try
    End Sub

    ''' <summary>
    ''' Send function of the TCP-Server
    ''' </summary>
    Private Sub TCPServerSend(ByVal Handler As Socket, ByVal Data As String)
        ' Convert the string data to byte data using ASCII encoding.
        Dim ByteData As Byte() = Encoding.ASCII.GetBytes(Data)

        ' Begin sending the data to the remote device.
        Handler.BeginSend(ByteData, 0, ByteData.Length, 0, New AsyncCallback(AddressOf TCPServerSendCallback), Handler)
    End Sub 'Send

    ''' <summary>
    ''' Send-Callback of the TCP-Server
    ''' </summary>
    Private Sub TCPServerSendCallback(ByVal ar As IAsyncResult)
        Try
            ' Retrieve the socket from the state object.
            Dim handler As Socket = CType(ar.AsyncState, Socket)

            ' Complete sending the data to the remote device.
            Dim bytesSent As Integer = handler.EndSend(ar)
            Console.WriteLine("Sent {0} bytes to client.", bytesSent)

            'handler.Shutdown(SocketShutdown.Both)
            'handler.Close()
        Catch ex As ObjectDisposedException
            ' Server Socket already disposed
        End Try
    End Sub
#End Region

#Region "Multi-Cast Broadcast for other SpectraFoxes listening"

    ''' <summary>
    ''' UDP Client to recieve Broadcasts!
    ''' </summary>
    Private UDPBroadCastSubScriber As UdpClient

    ''' <summary>
    ''' Starts the UDP Multicast to listen for other Spectrafoxes in the intranet.
    ''' </summary>
    Protected Function StartBroadcastForSpectraFox() As Boolean
        Try
            ' Start the Subscriber
            Me.UDPBroadCastSubScriber = New UdpClient(UDPBroadcastPort, AddressFamily.InterNetwork)
            Dim MulticastAddr As IPAddress = IPAddress.Parse(UDPBroadcastIP)

            ' Join the Broadcast Group
            UDPBroadCastSubScriber.JoinMulticastGroup(MulticastAddr)

            ' Start the recieve of broadcast packages.
            Dim state As New UDPState
            state.workSocket = UDPBroadCastSubScriber
            UDPBroadCastSubScriber.BeginReceive(AddressOf BroadcastCallback, state)
            Return True
        Catch ex As Exception
            MessageBox.Show(My.Resources.rComputingCloud.ComputingCloudCouldNotBindToPort.Replace("%e", ex.Message),
                            My.Resources.rComputingCloud.ComputingCloudCouldNotBindToPort_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Ends the UDP Multicast, if running.
    ''' </summary>
    Protected Sub EndBroadcastForSpectraFox()
        Dim MulticastAddr As IPAddress = IPAddress.Parse(UDPBroadcastIP)
        Me.UDPBroadCastSubScriber.DropMulticastGroup(MulticastAddr)
        Me.UDPBroadCastSubScriber.Close()
        Me.UDPBroadCastSubScriber = Nothing
    End Sub

    ''' <summary>
    ''' Called on recieving a Multicast package by other SpectraFox-Versions.
    ''' </summary>
    Protected Sub BroadcastCallback(ByVal ar As IAsyncResult)
        Try
            ' Get all the contents from the async result.
            Dim state As UDPState = CType(ar.AsyncState, UDPState)
            Dim Socket As UdpClient = state.workSocket

            ' Read data from the client socket. 
            Dim Buffer As String = Encoding.ASCII.GetString(Socket.EndReceive(ar, Nothing))

            ' Start another recieve
            Socket.BeginReceive(AddressOf BroadcastCallback, state)

            '##################
            ' Parse the data.
            ' System Messages to manage the computing cloud.

            ' Ask around for all clients.
            If Buffer.StartsWith("#HELLO#") Then
                '####################################
                ' Answer with local IP and client version and other information
                Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
                Dim ipAddress As IPAddress = ipHostInfo.AddressList(0)
                Dim CPUName As String = Nothing
                Try
                    Dim mos As New ManagementObjectSearcher("select * from Win32_Processor")
                    CPUName = Convert.ToString(mos.Get.Cast(Of ManagementObject).First.Item("Name"))
                Catch ex As Exception
                    CPUName = Nothing
                End Try
                If CPUName Is Nothing Then CPUName = "n/a"
                Dim ComputerName As String = System.Net.Dns.GetHostName()
                '####################################

                SendBroadcast("#PING#" & ipAddress.ToString & _
                              "#" & cProgrammDeployment.GetProgramVersionString & _
                              "#" & Me.SmartThreadPool.STPStartInfo.MaxWorkerThreads.ToString(System.Globalization.CultureInfo.InvariantCulture) & _
                              "#" & CPUName &
                              "#" & ComputerName)

                ' Clear the client-buffer.
                Me._CloudMembers.Clear()
            End If

            ' Tell others that we are here.
            If Buffer.StartsWith("#PING#") Then
                Buffer = Buffer.Remove(0, "#PING#".Length)

                Dim BufferSplit As String() = Buffer.Split(CChar("#"))
                Dim IPString As String = BufferSplit(0)

                ' Parse the client-version and ip.
                Dim ClientVersion As String = BufferSplit(1)
                Dim ClientIP As IPAddress = IPAddress.Parse(IPString)
                Dim MaxThreadsAvailableByClient As Integer = Convert.ToInt32(BufferSplit(2), System.Globalization.CultureInfo.InvariantCulture)
                Dim CPUName As String = BufferSplit(3)
                Dim ComputerName As String = BufferSplit(4)

                ' Ignore the own IP ping.
                Dim LocalIPHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
                Dim LocalIP As IPAddress = LocalIPHostInfo.AddressList(0)
#If Not Debug Then
                If ClientIP.ToString <> LocalIP.ToString Then
#Else
                If True Then
#End If
                    Dim CM As New CloudMember
                    With CM
                        .ClientVersion = ClientVersion
                        .IP = ClientIP
                        .ThreadCountAvailable = MaxThreadsAvailableByClient
                        .CPUName = CPUName
                        .ComputerName = ComputerName
                    End With
                    If Me._CloudMembers.ContainsKey(IPString) Then
                        Me._CloudMembers(IPString) = CM
                    Else
                        Me._CloudMembers.Add(IPString, CM)
                    End If
                End If
            End If
            '##################
        Catch ex As ObjectDisposedException
            ' Socket got closed!
        End Try
    End Sub


    ''' <summary>
    ''' Sends the Broadcast message to all other Spectrafoxes!
    ''' </summary>
    Public Shared Sub SendBroadcast(ByRef Message As String)
        Try
            Dim s As New UdpClient(UDPBroadcastIP, UDPBroadcastPort)
            Dim SendBytes As Byte() = Encoding.ASCII.GetBytes(Message)
            s.BeginSend(SendBytes, SendBytes.Length, Nothing, Nothing)
        Catch ex As Exception
            MessageBox.Show(My.Resources.rComputingCloud.ErrorSendingBroadcast.Replace("%e", ex.Message),
                            My.Resources.rComputingCloud.ErrorSendingBroadcast_Title,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Send a HELLO command, to tell the others that we are a new client, using a certain version.
    ''' All other clients will then answer with their IP-Adresses and Program-Versions.
    ''' </summary>
    Public Sub AskAroundForOtherSpectraFoxes()
        SendBroadcast("#HELLO# Here is SpectraFox! Version " & cProgrammDeployment.GetProgramVersionString & " looking for cloud members!")
    End Sub
#End Region

#Region "Async TCP-Client to connect to other Cloud-Members and ask them to process data"

    ''' <summary>
    ''' Async TCP-Client to connect to other Cloud-Members and ask them to process data
    ''' </summary>
    Public Shared Function ConnectToTCPServer(ByRef TCPServerIP As IPAddress) As Socket
        ' Establish the remote endpoint for the socket.
        Dim ServerEndPoint As New IPEndPoint(TCPServerIP, TCPServerPort)

        ' Create a TCP/IP socket.
        Dim Client As New Socket(TCPServerIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Try
            ' Connect to the remote endpoint.
            Client.BeginConnect(ServerEndPoint, New AsyncCallback(AddressOf TCPClientConnectCallback), Client)
            Return Client
        Catch ex As Exception
            MessageBox.Show(My.Resources.rComputingCloud.TCPClient_CouldNotContactServer.Replace("%e", ex.Message).Replace("%ip", TCPServerIP.ToString),
                           My.Resources.rComputingCloud.TCPClient_CouldNotContactServer_Title,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Connection callback for the TCPClient
    ''' </summary>
    Public Shared Sub TCPClientConnectCallback(ByVal ar As IAsyncResult)
        Try
            ' Retrieve the socket from the state object.
            Dim Client As Socket = CType(ar.AsyncState, Socket)

            ' Complete the connection.
            Client.EndConnect(ar)

            ' Create the state object.
            Dim State As New StateObject
            State.workSocket = Client

            ' Begin receiving the data from the remote device.
            Client.BeginReceive(State.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf TCPClientReceiveCallback), State)
        Catch ex As ObjectDisposedException
            ' Server Socket already disposed
        End Try
    End Sub

    ''' <summary>
    ''' Recieve Callback of the client.
    ''' </summary>
    Public Shared Sub TCPClientReceiveCallback(ByVal ar As IAsyncResult)
        Try
            ' Retrieve the state object and the client socket 
            ' from the asynchronous state object.
            Dim State As StateObject = CType(ar.AsyncState, StateObject)
            Dim Client As Socket = State.workSocket

            ' Read data from the remote device.
            Dim BytesRead As Integer = Client.EndReceive(ar)

            If BytesRead > 0 Then
                ' There might be more data, so store the data received so far.
                State.sb.Append(Encoding.ASCII.GetString(State.buffer, 0, BytesRead))

                ' Get the rest of the data.
                Client.BeginReceive(State.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf TCPClientReceiveCallback), State)
            Else
                ' All the data has arrived; put it in response.
                Dim Response As String = String.Empty
                If State.sb.Length > 1 Then
                    Response = State.sb.ToString()
                End If
            End If
        Catch ex As ObjectDisposedException
            ' Server Socket already disposed
        End Try
    End Sub

    ''' <summary>
    ''' Send function of the TCP-Client
    ''' </summary>
    Public Shared Sub TCPClientSend(ByVal Handler As Socket,
                                    ByVal Data As String)
        ' Convert the string data to byte data using ASCII encoding.
        Dim ByteData As Byte() = Encoding.ASCII.GetBytes(Data)

        ' Begin sending the data to the remote device.
        Handler.BeginSend(ByteData, 0, ByteData.Length, 0, New AsyncCallback(AddressOf TCPClientSendCallback), Handler)
    End Sub

    ''' <summary>
    ''' Send function of the TCP-Client directly
    ''' </summary>
    Public Shared Sub TCPClientSendDirect(ByVal Handler As Socket, ByVal Data As String)
        ' Convert the string data to byte data using ASCII encoding.
        Dim ByteData As Byte() = Encoding.ASCII.GetBytes(Data)

        ' Begin sending the data to the remote device.
        Handler.Send(ByteData, 0, ByteData.Length, 0)
    End Sub

    ''' <summary>
    ''' Send function of the TCP-Client directly and wait for recieve of data
    ''' </summary>
    Public Shared Function TCPClientSendDirectAndWaitForResponse(ByVal Handler As Socket, ByVal Data As String) As String
        ' Convert the string data to byte data using ASCII encoding.
        Dim SendByteData As Byte() = Encoding.ASCII.GetBytes(Data)

        Try
            ' Begin sending the data to the remote device.
            Handler.Send(SendByteData, 0, SendByteData.Length, 0)

            ' Create response buffer
            Dim ResponseBuffer(1024) As Byte
            Dim ResponseSB As New StringBuilder

            Dim BytesRead As Integer
            ' Wait for response
            While True
                BytesRead = Handler.Receive(ResponseBuffer, 0, ResponseBuffer.Length, 0)
                Dim Content As String = Encoding.ASCII.GetString(ResponseBuffer)
                ResponseSB.Append(Content)

                If Content.Contains("<EOF>") Then
                    Exit While
                End If
            End While

            Dim Response As String = ResponseSB.ToString.Trim
            Return Response.Remove(Response.Length - "<EOF>".Length)
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Send-Callback of the TCP-Client
    ''' </summary>
    Public Shared Sub TCPClientSendCallback(ByVal ar As IAsyncResult)
        Try
            ' Retrieve the socket from the state object.
            Dim handler As Socket = CType(ar.AsyncState, Socket)

            ' Complete sending the data to the remote device.
            Dim bytesSent As Integer = handler.EndSend(ar)
            Console.WriteLine("Sent {0} bytes to client.", bytesSent)

            'handler.Shutdown(SocketShutdown.Both)
            'handler.Close()
        Catch ex As ObjectDisposedException
            ' Server Socket already disposed
        End Try
    End Sub

#End Region

#Region "Communication Functions"

    ''' <summary>
    ''' Ask, if the cloud member is idle!
    ''' </summary>
    Public Shared Function AreYouIdle(ByRef CM As CloudMember) As Boolean
        ' Connect to the cloud-member
        Dim Client As Socket = ConnectToTCPServer(CM.IP)
        If Not Client Is Nothing Then
            MsgBox(TCPClientSendDirectAndWaitForResponse(Client, "#AREYOUIDLE# Are you idle?<EOF>"))
        End If
        Client.Close()
        Client.Dispose()

        Return True
    End Function

#End Region

End Class
