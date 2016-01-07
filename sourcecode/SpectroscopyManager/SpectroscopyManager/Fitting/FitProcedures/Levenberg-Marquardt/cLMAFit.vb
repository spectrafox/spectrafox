Imports System.Diagnostics
Imports System.Threading
Imports System.ComponentModel

Imports MathNet.Numerics.LinearAlgebra.Double

Public Class cLMAFit
    Implements iFitProcedure

    ' default end conditions
    Public Const LambdaConst As Double = 0.0001

    ' default end conditions
    Public Const LambdaConstMax As Double = 10

#Region "Custom Fit-Procedure Setting-Class"

    ''' <summary>
    ''' Custom implementation of the settings-class used for this fit-procedure.
    ''' </summary>
    Public Class cFitProcedureSettings
        Implements iFitProcedureSettings

        Private Const DefaultMinDeltaChi2 As Double = 0.0000000001
        Private Const DefaultMaxIterations As Integer = 300
        Private Const DefaultDifferentiationStep As Double = 0.00001
        Private Const DefaultUseCenterDifferentiationJacobianCalculation As Boolean = True

        ''' <summary>
        ''' Returns the type of the fit-procedure class.
        ''' </summary>
        Public ReadOnly Property BaseClass As Type Implements iFitProcedureSettings.BaseClass
            Get
                Return GetType(cLMAFit)
            End Get
        End Property

        Private _MinDeltaChi2 As Double = DefaultMinDeltaChi2
        ''' <summary>
        ''' Sets the minimum change in Chi^2 for which to abort the fit
        ''' </summary>
        Public Property MinDeltaChi2 As Double Implements iFitProcedureSettings.StopCondition_MinChi2Change
            Get
                Return _MinDeltaChi2
            End Get
            Set(value As Double)
                _MinDeltaChi2 = value
                My.Settings.LMAFit_MinChi2 = value
                My.Settings.Save()
            End Set
        End Property

        Private _MaxIterations As Integer = DefaultMaxIterations
        ''' <summary>
        ''' Sets the maximum number of iterations, after which to abort the fit.
        ''' </summary>
        Public Property MaxIteration As Integer Implements iFitProcedureSettings.StopCondition_MaxIterations
            Get
                Return _MaxIterations
            End Get
            Set(value As Integer)
                _MaxIterations = value
                My.Settings.LMAFit_MaxIterations = value
                My.Settings.Save()
            End Set
        End Property

        Private _DifferentiationStep As Double = DefaultDifferentiationStep
        ''' <summary>
        ''' Sets the smallest step between numeric differentiations
        ''' </summary>
        Public Property DifferentiationStep As Double
            Get
                Return _DifferentiationStep
            End Get
            Set(value As Double)
                _DifferentiationStep = value
                My.Settings.LMAFit_DerivativeDelta = value
                My.Settings.Save()
            End Set
        End Property

        Private _UseCenterDifferentiationJacobianCalculation As Boolean = DefaultUseCenterDifferentiationJacobianCalculation
        ''' <summary>
        ''' <code>UseCenterDifferentiationForJacobian</code> decides, whether center
        ''' differences will be used. They are more accurate, but require more function evaluations,
        ''' since for the forward differences the function evaluations already performed before are used.
        ''' </summary>
        Public Property UseCenterDifferentiationJacobianCalculation As Boolean
            Get
                Return _UseCenterDifferentiationJacobianCalculation
            End Get
            Set(value As Boolean)
                _UseCenterDifferentiationJacobianCalculation = value
                My.Settings.LMAFit_UseCenterDifferentiation = value
                My.Settings.Save()
            End Set
        End Property

        ''' <summary>
        ''' Echo the Settings
        ''' </summary>
        Public Function EchoSettings() As String Implements iFitProcedureSettings.EchoSettings
            Dim SB As New System.Text.StringBuilder
            SB.AppendLine("use center differences:   " & Me.UseCenterDifferentiationJacobianCalculation)
            SB.Append("numeric derivative delta: " & Me.DifferentiationStep.ToString("E3"))
            Return SB.ToString
        End Function

        ''' <summary>
        ''' Load settings, if present!
        ''' </summary>
        Public Sub New()
            If My.Settings.LMAFit_MaxIterations > 0 Then Me.MaxIteration = My.Settings.LMAFit_MaxIterations
            If My.Settings.LMAFit_MinChi2 > 0 Then Me.MinDeltaChi2 = My.Settings.LMAFit_MinChi2
            If My.Settings.LMAFit_DerivativeDelta > 0 Then Me.DifferentiationStep = My.Settings.LMAFit_DerivativeDelta
        End Sub
    End Class

#End Region

#Region "Fit-Procedure-Settings"

    ''' <summary>
    ''' Fit-Procedure-Settings
    ''' </summary>
    Private ThisFitProcedureSettings As New cFitProcedureSettings

    ''' <summary>
    ''' Property to save the procedure-specific fit-settings.
    ''' </summary>
    Public Property FitProcedureSettings As iFitProcedureSettings Implements iFitProcedure.FitProcedureSettings
        Get
            Return ThisFitProcedureSettings
        End Get
        Set(value As iFitProcedureSettings)
            Me.ThisFitProcedureSettings = CType(value, cFitProcedureSettings)
        End Set
    End Property

    ''' <summary>
    ''' Returns the Settings-Panel
    ''' </summary>
    Public ReadOnly Property ProcedureSettingPanel As cFitProcedureSettingsPanel Implements iFitProcedure.ProcedureSettingPanel
        Get
            Dim SP As New cFitProcedureSettingsPanel_LMA
            SP.FitProcedureSettings = Me.ThisFitProcedureSettings
            Return SP
        End Get
    End Property

#End Region

#Region "Stop-Reasons"
    ''' <summary>
    ''' Reasons for the Fit-Procedure to end.
    ''' </summary>
    Public Enum FitStopReason
        MatrixSingular
        AlphaElemNaN
    End Enum

    ''' <summary>
    ''' Converts a FitStopReason-Code to a message to display to the user
    ''' </summary>
    Public Function ConvertFitStopCodeToMessage(FitStopCode As Integer) As String Implements iFitProcedure.ConvertFitStopCodeToMessage
        Select Case FitStopCode
            Case cLMAFit.FitStopReason.MatrixSingular
                Return My.Resources.rLMAFit.FitStopReason_MatrixSingular
            Case cLMAFit.FitStopReason.AlphaElemNaN
                Return My.Resources.rLMAFit.FitStopReason_AlphaElemNaN
            Case Else
                Return "unknown fit stop reason"
        End Select
    End Function
#End Region

#Region "Properties"
    ''' <summary>
    ''' Fit-Procedure-Name
    ''' </summary>
    Public ReadOnly Property Name As String Implements iFitProcedure.Name
        Get
            Return My.Resources.rLMAFit.LMA_Name
        End Get
    End Property
#End Region

#Region "Fit Procedure"

    ''' <summary>
    ''' Hessian Matrix
    ''' </summary>
    Private alphaMat As MathNet.Numerics.LinearAlgebra.Double.DenseMatrix

    ''' <summary>
    ''' Gradient vector
    ''' </summary>
    Private betaVec As MathNet.Numerics.LinearAlgebra.Double.DenseVector

    ''' <summary>
    ''' FitParameter Step increment values
    ''' </summary>
    Private daVec As MathNet.Numerics.LinearAlgebra.Vector(Of Double)

    ''' <summary>
    ''' Damping parameter lambda.
    ''' </summary>
    Private lambda As Double

    ''' <summary>
    ''' Storage for the incremented Chi2
    ''' </summary>
    Private IncrementedChi2 As Double

    ' Fit Function used for the fit
    Protected FitFunction As iFitFunction

    ' storage for the non-fixed parameter info
    Protected ParameterCount As Integer
    Protected NonFixedFitParameterCount As Integer
    Protected NonFixedFitParameterIndices As String()

    ' Create temporary arrays used 
    Protected OldValues As Double()
    Protected CalculatedDataPoints As Double()
    Protected TestDataPoints As Double()

    ' Get the storage for the jacobi-matrix
    Protected JacobianMatrix As Double(,) = Nothing

    ''' <summary>
    ''' FitInizializer, defines the initial set of variables,
    ''' e.g. adapts the length of the arrays to the datapoints, etc.
    ''' </summary>
    Protected Sub FitInitializer(ByRef ModelFitFunction As iFitFunction,
                                 ByRef DataPoints As Double()(),
                                 ByRef Weights As Double()) Implements iFitProcedure.FitInitializer

        ' Save the fit-function
        Me.FitFunction = ModelFitFunction

        '// number of active parameters
        Me.NonFixedFitParameterIndices = ModelFitFunction.FitParameters.GetNonFixedInternalIdentifiers
        Me.NonFixedFitParameterCount = Me.NonFixedFitParameterIndices.Length
        Me.ParameterCount = ModelFitFunction.FitParameters.Count

        ' Set the given Hessian Matrix
        Me.alphaMat = DenseMatrix.Create(NonFixedFitParameterCount, NonFixedFitParameterCount, 0)
        ' Set the Vectors used for calculation of the LMA-equation
        Me.betaVec = DenseVector.Create(NonFixedFitParameterCount, 0)
       
        ' Create temporary arrays used
        ReDim OldValues(Me.ParameterCount - 1)
        ReDim TestDataPoints(DataPoints(0).Length - 1)

        ' Get a clear storage for the jacobi-matrix
        JacobianMatrix = Nothing

        ' set the initial damping parameter lambda
        lambda = LambdaConst

        Debug.WriteLine("Starting the Fit-Process ...")
        Debug.Indent()

        ' Calculating the datapoints for the current parameter-set
        Debug.WriteLine("Calculating the function values for the current parameter-set ...")
        CalculatedDataPoints = Me.FitFunction.GenerateData(ModelFitFunction.FitParametersGrouped, DataPoints(0))

    End Sub

    ''' <summary>
    ''' FitStep. This functions gets called in the fit-loop,
    ''' until the fit converged! Has to return the calculated Chi2.
    ''' </summary>
    Protected Function FitStep(ByRef FitDataPoints As Double()(),
                               ByRef Weights As Double(),
                               ByRef InputParameters As cFitParameterGroupGroup,
                               ByRef Iteration As Integer,
                               ByRef StopReason As Integer) As Double Implements iFitProcedure.FitStep

        ' Secure the old values
        Debug.WriteLine("Securing unincremented values ...")
        For i As Integer = 0 To Me.ParameterCount - 1 Step 1
            OldValues(i) = InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).ParameterByIndex(i).Value
        Next

        ' Calculates the new Chi2
        Debug.WriteLine("Calculating Chi2 ...")
        Dim _TestChi2 As Double = cFitProcedureBase.CalculateChi2(CalculatedDataPoints, FitDataPoints, Weights, InputParameters)

        Debug.WriteLine("Calculating Jacobian Matrix ...")
        ' Calculate the Jacobian-Matrix
        If Not cFitProcedureBase.CalculateJacobianMatrix(Me.FitFunction,
                                                         FitDataPoints,
                                                         InputParameters,
                                                         Me.ThisFitProcedureSettings.UseCenterDifferentiationJacobianCalculation,
                                                         CalculatedDataPoints,
                                                         JacobianMatrix) Then
            ' Singular Matrix recognized
            StopReason = FitStopReason.MatrixSingular
            Return _TestChi2
        End If

        '###############################
        ' Calculating the alpha matrix elements
        Debug.WriteLine("Calculating the Alpha-Matrix ...")
        Dim j As Integer = 0
        For Row As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            For Col As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                ' Progress
                j += 1
                'If Not FitThreadWorker Is Nothing Then RaiseEvent_CalculationStepProgress(j, Me.NonFixedFitParameterCount * Me.NonFixedFitParameterCount, "Calculating Alpha Matrix ... ")

                '# FIXED 16/01/2015: Reset alpha to 0
                Me.alphaMat(Row, Col) = 0
                For i As Integer = 0 To FitDataPoints(0).Length - 1
                    Me.alphaMat(Row, Col) = Me.alphaMat(Row, Col) + (Weights(i) * JacobianMatrix(Row, i) * JacobianMatrix(Col, i))
                Next

                ' add a damping factor to the diagonal elements
                If Row = Col Then
                    Me.alphaMat(Row, Col) = Me.alphaMat(Row, Col) * (1 + lambda)
                End If

                If Double.IsNaN(Me.alphaMat(Row, Col)) Or Double.IsInfinity(Me.alphaMat(Row, Col)) Then
                    Debug.WriteLine("Alpha Element is NaN: " & Row.ToString & " | " & Col.ToString)
                    StopReason = FitStopReason.AlphaElemNaN
                    Return _TestChi2
                End If
            Next
        Next
        '###############################

        '###############################
        ' calculating the beta-vector
        Debug.WriteLine("Calculating the Beta-Vector ...")
        For Row As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
            ' Progress
            'If Not FitThreadWorker Is Nothing Then RaiseEvent_CalculationStepProgress(Row, Me.NonFixedFitParameterCount, "Calculating Beta Vector ... ")
            For i As Integer = 0 To FitDataPoints(0).Length - 1
                Me.betaVec(Row) = Me.betaVec(Row) + (Weights(i) * (FitDataPoints(1)(i) - CalculatedDataPoints(i)) * JacobianMatrix(Row, i)) ' FitFunction.GetY(DataPoints(0)(i), InIdentifiers, InValues)) *
            Next
        Next
        '###############################

        ' Encapsule the Increment-Solving-Section
        ' in a Try-Catch, to catch errors from singular matrices!
        Try
            Debug.WriteLine("Solving the linear equation system da = Beta Alpha^-1 ...")
            Me.daVec = alphaMat.Cholesky.Solve(betaVec)
            For i As Integer = 0 To Me.NonFixedFitParameterCount - 1 Step 1
                InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).Parameter(Me.NonFixedFitParameterIndices(i)).ChangeValue(OldValues(i) + Me.daVec(i), False)
            Next
            cFitProcedureBase.LockParametersTogether(InputParameters)
        Catch ex As Exception
            ' Singular Matrix recognized
            StopReason = FitStopReason.MatrixSingular
            Return _TestChi2
        End Try
        Debug.WriteLine("Calculating the new Chi2, using the parameter-set incremented by da ...")
        TestDataPoints = Me.FitFunction.GenerateData(InputParameters, FitDataPoints(0))
        IncrementedChi2 = cFitProcedureBase.CalculateChi2(TestDataPoints, FitDataPoints, Weights, InputParameters)

        ' Fit improvement
        ' The guess results to worse chi2 - make the step smaller
        If IncrementedChi2 >= _TestChi2 Then
            Debug.WriteLine("The incremented Chi2 is changed to the worse ... -> increasing the damping parameter lambda")

            ' Reset, if lambda gets too large
            lambda *= 10

            ' Reset the parameters to the old values
            For i As Integer = 0 To Me.ParameterCount - 1 Step 1
                InputParameters.Group(Me.FitFunction.UseFitParameterGroupID).ParameterByIndex(i).ChangeValue(OldValues(i), False)
            Next

            Return _TestChi2
        Else
            ' The guess results to better chi2 - move and make the step larger
            Debug.WriteLine("The incremented Chi2 is better than the original one ... -> lowering the damping parameter lambda")
            lambda /= 10

            ' Save the new test-point-set.
            TestDataPoints.CopyTo(CalculatedDataPoints, 0)

            ' Raise event to echo the step
            'Me._FitParameters = sFitParameter.GetFitParameterDictionaryFromArrays(Me._FitParameters, InIdentifiers, InValues)
            'RaiseEvent_FitStepEcho(Me._FitParameters, Chi2)

            Return IncrementedChi2
        End If

    End Function

    ''' <summary>
    ''' FitFinalizer, called, after the fit finished.
    ''' Releases resources, etc.
    ''' </summary>
    Protected Function FitFinalizer(ByRef FitDataPoints As Double()(),
                                    ByRef Weights As Double(),
                                    ByRef InputParameters As cFitParameterGroupGroup) As Boolean Implements iFitProcedure.FitFinalizer

        ' Clear Resources
        Me.alphaMat = Nothing
        Me.betaVec = Nothing
        Me.daVec = Nothing
        Me.FitFunction = Nothing
        Me.NonFixedFitParameterIndices = Nothing
        Me.OldValues = Nothing
        Me.CalculatedDataPoints = Nothing
        Me.TestDataPoints = Nothing
        Me.JacobianMatrix = Nothing

        Return True
    End Function

#End Region

End Class
