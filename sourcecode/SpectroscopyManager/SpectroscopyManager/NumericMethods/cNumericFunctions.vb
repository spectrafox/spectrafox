''' <summary>
''' Class that contains shared functions of different type.
''' </summary>
Public Class cNumericFunctions

#Region "Error Function (sigmoidal)"
    ''' <summary>
    ''' A nummeric approximations to the ErrorFunction centered around zero.
    ''' Maximal error of the approximation is 1.2*10-7.
    ''' Source: Numerical Recipes in Fortran 77: The Art of Scientific Computing (ISBN 0-521-43064-X), 1992, page 214, Cambridge University Press.
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function ErrorFunction(ByVal x As Double,
                                         ByVal XOffset As Double,
                                         ByVal YOffset As Double) As Double
        x = x - XOffset ' We can do this, because x is given ByVal
        Dim T As Double = 1 / (1 + 0.5 * MathAbs(x))
        Dim Tau As Double = T * Math.Exp(-(x * x) - _
                                         1.26551223 + _
                                         1.00002368 * T + _
                                         0.37409196 * Math.Pow(T, 2) + _
                                         0.09678418 * Math.Pow(T, 3) - _
                                         0.18628806 * Math.Pow(T, 4) + _
                                         0.27886807 * Math.Pow(T, 5) - _
                                         1.13520398 * Math.Pow(T, 6) + _
                                         1.48851587 * Math.Pow(T, 7) - _
                                         0.82215223 * Math.Pow(T, 8) + _
                                         0.17087277 * Math.Pow(T, 9))

        ' Return the error function value
        If x >= 0 Then
            Return YOffset + 1 - Tau
        Else
            Return YOffset + Tau - 1
        End If
    End Function
#End Region

#Region "Sigmoidal Boltzmann Function"
    ''' <summary>
    ''' A Sigmoidal Boltzmann Function for a broadened step between A1 and A2, with width dx around x0.
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function SigmoidalBoltzmann(ByVal x As Double,
                                              ByVal XOffset As Double,
                                              ByVal A1 As Double,
                                              ByVal A2 As Double,
                                              ByVal Width As Double) As Double
        ' Check for too small Width
        If Width <= 0.00001 Then
            If x > XOffset Then
                Return A2
            Else
                Return A1
            End If
        End If
        Return A2 + (A1 - A2) / (1 + Math.Exp((x - XOffset) / Width))
    End Function
#End Region

#Region "Gaussian Peak"
    ''' <summary>
    ''' Amplitude Definition of a Gaussian Peak
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function GaussPeak_Amplitude(ByVal x As Double,
                                               ByVal XCenter As Double,
                                               ByVal Width As Double,
                                               ByVal YOffset As Double,
                                               ByVal Amplitude As Double) As Double
        If Width = 0 Then Return 0
        Dim d_zs As Double = (x - XCenter) / Width
        Return YOffset + Amplitude * Math.Exp(-0.5 * d_zs * d_zs)
    End Function

    ''' <summary>
    ''' Area Definition of a Gaussian Peak
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function GaussPeak_Area(ByVal x As Double,
                                          ByVal XCenter As Double,
                                          ByVal Width As Double,
                                          ByVal YOffset As Double,
                                          ByVal Area As Double) As Double
        If Width = 0 Then Return 0
        Dim d_zs As Double = (x - XCenter) / Width
        Return YOffset + Area / (Width * cConstants.SqrtPiHalf) * Math.Exp(-2 * d_zs * d_zs)
    End Function

    ''' <summary>
    ''' Gaussian Peak normalized to Amplitude = 1
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function GaussPeak_Amplitude_Normalized(ByVal x As Double,
                                                          ByVal XCenter As Double,
                                                          ByVal Width As Double) As Double
        Return GaussPeak_Amplitude(x, XCenter, Width, 0, 1)
    End Function

    ''' <summary>
    ''' Gaussian Peak normalized to Area = 1
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function GaussPeak_Area_Normalized(ByVal x As Double,
                                                     ByVal XCenter As Double,
                                                     ByVal Width As Double) As Double
        Return GaussPeak_Area(x, XCenter, Width, 0, 1)
    End Function
#End Region

#Region "Lorentzian Peak"

    ''' <summary>
    ''' Fit-Function, Lorentzian-Distribution
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function LorentzPeak(ByVal x As Double,
                                       ByVal XCenter As Double,
                                       ByVal Width As Double,
                                       ByVal YOffset As Double,
                                       ByVal Area As Double) As Double
        If Width = 0 Then Return 0
        Return YOffset + 2 * Area * Width * MathNet.Numerics.Constants.InvPi / (4 * (x - XCenter) * (x - XCenter) + Width * Width)
    End Function

    ''' <summary>
    ''' Lorentzian Peak normalized to Area = 1
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function LorentzPeak_Normalized(ByVal x As Double,
                                                  ByVal XCenter As Double,
                                                  ByVal Width As Double) As Double
        Return LorentzPeak(x, XCenter, Width, 0, 1)
    End Function

#End Region

#Region "Custom absolute function"

    ''' <summary>
    ''' Math.Abs written manually for CUDA, since it has some conflict somehow in there.
    ''' </summary>
    <Cudafy.Cudafy>
    Public Shared Function MathAbs(ByVal d As Double) As Double
        If d >= 0 Then
            Return d
        Else
            Return -d
        End If
    End Function

#End Region

End Class
