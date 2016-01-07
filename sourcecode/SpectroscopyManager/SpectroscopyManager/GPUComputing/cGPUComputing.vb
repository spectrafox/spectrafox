Imports Cudafy.Host
Imports Cudafy
Imports Cudafy.Translator

Public Class cGPUComputing

    ''' <summary>
    ''' Returns the total number of threads used.
    ''' </summary>
    Public Shared Function GetThreadsPerBlock(ByRef CudaGPU As GPGPU) As Integer
        If CudaGPU Is Nothing Then
            Return 256
        Else
            Return CudaGPU.GetDeviceProperties.MaxThreadsPerBlock
        End If
    End Function

    ''' <summary>
    ''' Cuda-Settings
    ''' </summary>
    Public Shared Function CUDABlocksPerGrid(TotalNumber As Integer, ThreadsPerBlock As Integer) As Integer
        Dim Blocks As Integer = CInt(TotalNumber / ThreadsPerBlock)
        If TotalNumber Mod ThreadsPerBlock > 0 Then Blocks += 1
        Return Blocks
    End Function

    ' ''' <summary>
    ' ''' Cuda-Settings
    ' ''' </summary>
    'Public Shared Function CUDABlocksPerGrid() As Integer
    '    Return 256
    'End Function

    ' ''' <summary>
    ' ''' Returns the total number of threads used.
    ' ''' </summary>
    'Public Shared Function GetThreadsPerBlock(TotalNumber As Integer) As Integer
    '    Dim BlockSize As Integer = CInt(TotalNumber / CUDABlocksPerGrid)
    '    If TotalNumber Mod CUDABlocksPerGrid > 0 Then BlockSize += 1
    '    Return BlockSize
    'End Function

    ''' <summary>
    ''' Initializes the Cuda-Device and returns the GPGPU, or NOTHING,
    ''' if a fallback to the CPU happened. In this case also the UseCUDAVersion-Variable
    ''' gets set back to false.
    ''' </summary>
    Public Shared Function InitializeCUDAOrFallBackToCPU(ParamArray CompilationTypes() As Type) As GPGPU

        ' Initialize the CPU storage
        Dim CudaGPU As GPGPU = Nothing
        Dim CUDAModule As CudafyModule = Nothing

        ' Load settings
        If My.Settings.GPUComputing_GPUDeviceID < 0 Then
            ' Cancel and fallback!
            Debug.WriteLine("GPU computing settings not initialized!")
            MessageBox.Show(My.Resources.rGPUComputing.Settings_Not_Initialized,
                            My.Resources.rGPUComputing.Settings_Not_Initialized_Title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            CudaGPU = Nothing
        Else
            ' Try to start CUDA
            ' Try openCL initialization
            ' check the settings:
            CudafyModes.Target = CType(My.Settings.GPUComputing_GPULanguage, eGPUType)
            CudafyModes.DeviceId = My.Settings.GPUComputing_GPUDeviceID
            Select Case CudafyModes.Target
                Case eGPUType.Cuda
                    CudafyTranslator.Language = eLanguage.Cuda
                Case Else
                    CudafyTranslator.Language = eLanguage.OpenCL
            End Select
            Try
                Dim CudaDeviceCount As Integer = CudafyHost.GetDeviceCount(CudafyModes.Target)
                If CudaDeviceCount > CudafyModes.DeviceId Then
                    CudaGPU = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId)
                End If
            Catch ex As Exception
                Debug.WriteLine("GPU could not be initialized with OPENCL " & ex.Message)
                Debug.WriteLine("Falling back to CPU mode!")
                MessageBox.Show(My.Resources.rGPUComputing.FallBackToCPU & My.Resources.rGPUComputing.FallBackToCPU_Reason_DeviceInitFailed.Replace("%e", ex.Message),
                                My.Resources.rGPUComputing.FallBackToCPU_Title,
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                CudaGPU = Nothing
            End Try
            Try
                If Not CudaGPU Is Nothing Then
                    Select Case CudafyModes.Target
                        Case eGPUType.Cuda
                            CUDAModule = CudafyTranslator.Cudafy(eArchitecture.sm_20, CompilationTypes)
                        Case Else
                            CUDAModule = CudafyTranslator.Cudafy(eArchitecture.OpenCL, CompilationTypes)
                    End Select

                    CudaGPU.LoadModule(CUDAModule)
                End If
            Catch ex As Exception
                If Not CUDAModule Is Nothing Then
                    Debug.WriteLine("GPU code compilation error " & vbNewLine & CUDAModule.CompilerOutput)
                Else
                    Debug.WriteLine("GPU Language compilation error " & vbNewLine & ex.Message)
                End If
                Debug.WriteLine("Falling back to CPU mode!")
                MessageBox.Show(My.Resources.rGPUComputing.FallBackToCPU & My.Resources.rGPUComputing.FallBackToCPU_Reason_CodeCompilationError.Replace("%e", ex.Message),
                                My.Resources.rGPUComputing.FallBackToCPU_Title,
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                'MessageBox.Show(CUDAModule.CompilerOutput)
                CudaGPU = Nothing
            End Try
        End If

        Return CudaGPU
    End Function

    ''' <summary>
    ''' Returns is the settings are Ok to activate CUDA.
    ''' </summary>
    Public Shared Function CanActivateCUDA() As Boolean
        If My.Settings.GPUComputing_GPUDeviceID < 0 Then Return False
        Return True
    End Function

End Class
