Public Class cFitProcedureBase

    ''' <summary>
    ''' General FitProcedure Stop Reasons
    ''' Values SMALLER 0!!!!
    ''' </summary>
    Public Enum FitStopReasons
        None = -1
        MaxIterationsReached = -2
        UserAborted = -3
        FitConverged = -4
    End Enum

    ''' <summary>
    ''' This function should be called after each iteration.
    ''' It takes care of adapting the locked parameters to each other.
    ''' </summary>
    Public Shared Sub LockParametersTogether(ByRef FitParameterGroups As cFitParameterGroupGroup)

        For Each FPG As cFitParameterGroup In FitParameterGroups
            For Each FP As cFitParameter In FPG

                ' If locked, find the locked parameter,
                ' and set the values equal, using the given factor.
                If FP.LockedToParameterIdentifier <> "" Then

                    ' Extract the information from the lock-string.
                    Dim GIDKV As KeyValuePair(Of Guid, String) = cFitParameter.GetGroupIDFromIdentifier(FP.LockedToParameterIdentifier)

                    ' Check, if the GID exists
                    If FitParameterGroups.ContainsKey(GIDKV.Key) Then
                        FP.ChangeValue(FitParameterGroups.Group(GIDKV.Key).Parameter(GIDKV.Value).Value * FP.LockedWithFactor, False)
                    End If

                End If

            Next
        Next

    End Sub


#Region "Chi2 calculation"

    ''' <summary>
    ''' High penalty value of Chi2,
    ''' if parameters are out of their boundaries.
    ''' </summary>
    Public Const Chi2PenaltyForOutOfBoundaryParameters As Double = Double.MaxValue

    ''' <summary>
    ''' Calculates value of the function for given parameter array.
    ''' Also checks the boundaries of the parameters, and adds
    ''' a penalty factor, if the parameter value is outside the boundary.
    ''' </summary>
    ''' <returns>value of the function</returns>
    Public Shared Function CalculateChi2(ByRef FitFunction As iFitFunction,
                                         ByRef FitDataPoints As Double()(),
                                         ByRef Weights As Double(),
                                         ByRef InputParameters As cFitParameterGroupGroup) As Double

        ' Check parameter boundaries in advance.
        ' If one parameter is out of range,
        ' add a high penalty factor, and abort the rest of the calculation.
        For Each FPG As cFitParameterGroup In InputParameters
            For Each FP As cFitParameter In FPG
                If FP.Value > FP.UpperBoundary Or FP.Value < FP.LowerBoundary Then
                    Return Chi2PenaltyForOutOfBoundaryParameters
                End If
            Next
        Next

        ' Reduce the FitEcho to integer percentage values
        Dim FitEchoValue As Integer = CInt(FitDataPoints(0).Length / 50)

        Dim Chi2 As Double = 0
        Dim dy As Double
        For i As Integer = 0 To FitDataPoints(0).Length - 1
            ' Calculation
            dy = FitDataPoints(1)(i) - FitFunction.GetY(FitDataPoints(0)(i), InputParameters)
            Chi2 += Weights(i) * dy * dy
        Next

        Return Chi2
    End Function

    ''' <summary>
    ''' Calculates value of the function for given parameter array.
    ''' Also checks the boundaries of the parameters, and adds
    ''' a penalty factor, if the parameter value is outside the boundary.
    ''' 
    ''' Use this for already calculated function values.
    ''' </summary>
    ''' <returns>value of the function</returns>
    Public Shared Function CalculateChi2(ByRef TestFunctionValueVector As Double(),
                                         ByRef FitDataPoints As Double()(),
                                         ByRef Weights As Double(),
                                         ByRef InputParameters As cFitParameterGroupGroup) As Double


        ' Check parameter boundaries in advance.
        ' If one parameter is out of range,
        ' add a high penalty factor, and abort the rest of the calculation.
        For Each FPG As cFitParameterGroup In InputParameters
            For Each FP As cFitParameter In FPG
                If FP.Value > FP.UpperBoundary Or FP.Value < FP.LowerBoundary Then
                    Return Chi2PenaltyForOutOfBoundaryParameters
                End If
            Next
        Next

        ' Reduce the FitEcho to integer percentage values
        Dim FitEchoValue As Integer = CInt(FitDataPoints(0).Length / 50)

        Dim Chi2 As Double = 0
        Dim dy As Double
        For i As Integer = 0 To FitDataPoints(0).Length - 1
            ' Calculation
            dy = FitDataPoints(1)(i) - TestFunctionValueVector(i)
            Chi2 += Weights(i) * dy * dy
        Next

        Return Chi2
    End Function
#End Region

#Region "Jacobian Matrix"

    ''' <summary>
    ''' Delta between the partial derivatives used for calculating the jacobian
    ''' </summary>
    Public Shared PartialDerivativeDelta As Double = 0.000001 ' 0.000000001

    ''' <summary>
    ''' Create Jacobian Matrix of all partial derivatives of the Fit-Function.
    ''' <code>UseCenterDifferentiationForJacobian</code> decides, whether center
    ''' differences will be used. They are more accurate, but require more function evaluations,
    ''' since for the forward differences the function evaluations already performed before are used.
    ''' 
    ''' ONLY CHECKS IF THE JACOBIAN EXISTS (IsNothing). DOES NOT CHECK THE LENGTH OF AN EXISTING ONE!!!
    ''' </summary>
    ''' <returns>Boolean, True, if successfull, false, if Jacobian contains NaN</returns>
    Public Shared Function CalculateJacobianMatrix(ByRef FitFunction As iFitFunction,
                                                   ByRef DataPoints As Double()(),
                                                   ByRef InputParameters As cFitParameterGroupGroup,
                                                   ByVal UseCenterDifferentiationForJacobian As Boolean,
                                                   ByRef FunctionValueVector As Double(),
                                                   ByRef JacobianMatrix As Double(,)) As Boolean

        ' Register Boolean result to return
        Dim ReturnResult As Boolean = True

        ' Copy the fit-parameter value set
        Dim FPG As cFitParameterGroup = InputParameters.Group(FitFunction.UseFitParameterGroupID)
        Dim FPGNonFixedCount As Integer = FPG.CountNonFixed

        ' Check the length of the Output matrix, or set it, if it does not fit.
        If JacobianMatrix Is Nothing Then
            JacobianMatrix = New Double(FPGNonFixedCount - 1, DataPoints(0).Length - 1) {}
        End If

        ' Tmp-Variable
        Dim OldValue As Double
        Dim JacobianRow As Integer = 0

        ' Decide weather to use centered evaluation, or
        ' forward numeric derivation.
        If UseCenterDifferentiationForJacobian Then
            '
            '    Centered Differences
            ' (more accurate than forward)
            '
            '##############################

            ' Generate the buffer arrays for the positive change in the elements
            Dim PartialDerivatives_Pos(DataPoints(0).Length - 1) As Double
            Dim PartialDerivatives_Neg(DataPoints(0).Length - 1) As Double

            ' Create variable shortcut the currently used parameter index
            For Each FP As cFitParameter In FPG

                ' Just consider non-fixed parameters
                If FP.IsFixed Then Continue For

                ' store the old value
                OldValue = FP.Value

                ' Calculate the positive elements
                FP.ChangeValue(OldValue + PartialDerivativeDelta, False)
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    PartialDerivatives_Pos(i) = FitFunction.GetY(DataPoints(0)(i), InputParameters)
                Next

                ' Calculate the negative elements
                FP.ChangeValue(OldValue - PartialDerivativeDelta, False)
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    PartialDerivatives_Neg(i) = FitFunction.GetY(DataPoints(0)(i), InputParameters)
                Next

                ' Calculate the final partial derivatives and write them to the cache:
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    JacobianMatrix(JacobianRow, i) = (PartialDerivatives_Pos(i) - PartialDerivatives_Neg(i)) / (2 * PartialDerivativeDelta)
                    If Double.IsNaN(JacobianMatrix(JacobianRow, i)) Then
                        ReturnResult = False
                    End If
                Next

                ' Write back the old value
                FP.ChangeValue(OldValue, False)

                ' Increment the row used in the jacobian
                JacobianRow += 1
            Next
        Else
            '
            '         Forward Differences
            ' (needs half the function evaluations)
            '
            '#######################################

            ' Create variable shortcut the currently used parameter index
            For Each FP As cFitParameter In FPG

                ' Just consider non-fixed parameters
                If FP.IsFixed Then Continue For

                ' store the old value
                OldValue = FP.Value

                ' Calculate the positive elements
                FP.ChangeValue(OldValue + PartialDerivativeDelta, False)
                For i As Integer = 0 To DataPoints(0).Length - 1 Step 1
                    JacobianMatrix(JacobianRow, i) = (FitFunction.GetY(DataPoints(0)(i), InputParameters) _
                                                                - FunctionValueVector(i)) / PartialDerivativeDelta
                    If Double.IsNaN(JacobianMatrix(JacobianRow, i)) Then
                        ReturnResult = False
                    End If
                Next

                ' Write back the old value
                FP.ChangeValue(OldValue, False)

                ' Increment the row used in the jacobian
                JacobianRow += 1
            Next

        End If
        Return ReturnResult
    End Function

#End Region

End Class
