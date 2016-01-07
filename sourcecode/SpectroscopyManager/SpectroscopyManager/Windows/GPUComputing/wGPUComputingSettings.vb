Imports Cudafy.Host
Imports Cudafy

Public Class wGPUComputingSettings
    Inherits wFormBase

    Private GPUPropertyList As New Dictionary(Of eGPUType, List(Of GPGPUProperties))

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Private Sub wGPUComputingSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim CudafyTargets As New List(Of eGPUType)
        CudafyTargets.Add(eGPUType.Cuda)
        CudafyTargets.Add(eGPUType.OpenCL)

        ' List all GPUs
        Dim i As Integer = 0
        Dim SB As New System.Text.StringBuilder

        ' Go through all possible GPU-Types
        For Each T As eGPUType In CudafyTargets
            Me.GPUPropertyList.Add(T, New List(Of GPGPUProperties))

            Try
                SB.AppendLine("Scanning for GPU-Hardware using " & T.ToString & " ...")
                For Each prop As GPGPUProperties In CudafyHost.GetDeviceProperties(T)
                    SB.AppendLine("   --- General Information for device " & i & " ---")
                    SB.AppendLine("Name:  " & prop.Name)
                    SB.AppendLine("Platform Name:  " & prop.PlatformName)
                    SB.AppendLine("Device Id:  " & prop.DeviceId)
                    SB.AppendLine("Compute capability:  " & prop.Capability.Major & "." & prop.Capability.Minor)
                    SB.AppendLine("Clock rate: " & prop.ClockRate)
                    SB.AppendLine("Simulated: " & prop.IsSimulated)
                    SB.AppendLine("   --- Memory Information for device " & i & " ---")
                    SB.AppendLine("Total global mem:  " & prop.TotalMemory)
                    SB.AppendLine("Total constant Mem:  " & prop.TotalConstantMemory)
                    SB.AppendLine("Max mem pitch:  " & prop.MemoryPitch)
                    SB.AppendLine("Texture Alignment:  " & prop.TextureAlignment)
                    SB.AppendLine("   --- MP Information for device " & i & " ---")
                    SB.AppendLine("Shared mem per mp: " & prop.SharedMemoryPerBlock)
                    SB.AppendLine("Registers per mp:  " & prop.RegistersPerBlock)
                    SB.AppendLine("Threads in warp:  " & prop.WarpSize)
                    SB.AppendLine("Max threads per block:  " & prop.MaxThreadsPerBlock)
                    SB.AppendLine("Max thread dimensions:  (" & prop.MaxThreadsSize.x & ", " & prop.MaxThreadsSize.y & ", " & prop.MaxThreadsSize.z & ")")
                    SB.AppendLine("Max grid dimensions:  (" & prop.MaxGridSize.x & ", " & prop.MaxGridSize.y & ", " & prop.MaxGridSize.z & ")")

                    SB.AppendLine()

                    ' Add to the list
                    Me.GPUPropertyList(T).Add(prop)

                    ' Add the name to the Combobox
                    With Me.cboGPUComputingDevices
                        .Items.Add(New KeyValuePair(Of KeyValuePair(Of Integer, eGPUType), String)(New KeyValuePair(Of Integer, eGPUType)(prop.DeviceId, T), " (Language: " & T.ToString & ") " & prop.Name))
                        If My.Settings.GPUComputing_GPUDeviceID = prop.DeviceId And My.Settings.GPUComputing_GPULanguage = CInt(T) Then
                            .SelectedIndex = .Items.Count - 1
                        End If
                    End With

                    i += 1
                Next

            Catch ex As Exception
                SB.AppendLine("Error reading the GPU-Hardware details for GPU-Type " & T.ToString & "!")
                SB.AppendLine("=> " & ex.Message)
                SB.AppendLine()
            End Try
        Next

        ' disable the combobox
        If Me.cboGPUComputingDevices.Items.Count = 0 Then
            Me.cboGPUComputingDevices.Enabled = False
        End If

        ' Set the combobox properties.
        Me.cboGPUComputingDevices.ValueMember = "Key"
        Me.cboGPUComputingDevices.DisplayMember = "Value"



        ' Write the info to the window
        Me.txtCudaList.Text = SB.ToString
        Me.txtInfo.Text = My.Resources.GPUProcessing_INFO

    End Sub

    ''' <summary>
    ''' Save the device ID and the language to use in the settings.
    ''' </summary>
    Private Sub cboGPUComputingDevices_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboGPUComputingDevices.SelectedIndexChanged
        Dim SelectedDeviceID As Integer
        Dim SelectedLanguage As eGPUType
        If Not Me.cboGPUComputingDevices.SelectedItem Is Nothing Then
            ' Get the selected DeviceID
            SelectedDeviceID = DirectCast(Me.cboGPUComputingDevices.SelectedItem, KeyValuePair(Of KeyValuePair(Of Integer, eGPUType), String)).Key.Key
            SelectedLanguage = DirectCast(Me.cboGPUComputingDevices.SelectedItem, KeyValuePair(Of KeyValuePair(Of Integer, eGPUType), String)).Key.Value
        Else
            SelectedDeviceID = -1
            SelectedLanguage = eGPUType.OpenCL
        End If
        
        ' Save the GPU settings.
        With My.Settings
            .GPUComputing_GPUDeviceID = SelectedDeviceID
            .GPUComputing_GPULanguage = SelectedLanguage
        End With
    End Sub

    ''' <summary>
    ''' Close the window.
    ''' </summary>
    Private Sub btnConfirmComputingDevices_Click(sender As Object, e As EventArgs) Handles btnConfirmComputingDevices.Click
        Me.Close()
    End Sub
End Class