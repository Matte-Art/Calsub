using System.Collections.Generic;
using UnityEngine;

public class MathTaskGenerator : MonoBehaviour
{
    public int minValue;
    public int maxValue;
    public int operandCount;
    public MathOperationType operationType;

    public MathTask GenerateTask()
    {
        int[] operands = GenerateOperands(operandCount);
        int result = CalculateResult(operationType, operands);

        MathTask task = new MathTask(operands, result, operationType);
        return task;
    }

    public void UpdateParameters(int minValue, int maxValue, int operandCount)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.operandCount = operandCount;
    }

    private int[] GenerateOperands(int count)
    {
        int[] operands = new int[count];

        for (int i = 0; i < count; i++)
        {
            operands[i] = Random.Range(minValue, maxValue + 1);
        }

        return operands;
    }

    private int CalculateResult(MathOperationType operationType, int[] operands)
    {
        int result = 0;

        switch (operationType)
        {
            case MathOperationType.Addition:
                result = CalculateAdditionResult(operands);
                break;
            case MathOperationType.Subtraction:
                result = CalculateSubtractionResult(operands);
                break;
            case MathOperationType.Multiplication:
                result = CalculateMultiplicationResult(operands);
                break;
            case MathOperationType.Division:
                result = CalculateDivisionResult(operands);
                break;
        }

        return result;
    }

    private int CalculateAdditionResult(int[] operands)
    {
        int result = 0;

        for (int i = 0; i < operands.Length; i++)
        {
            result += operands[i];
        }

        return result;
    }

    private int CalculateSubtractionResult(int[] operands)
    {
        int result = operands[0];

        for (int i = 1; i < operands.Length; i++)
        {
            result -= operands[i];
        }

        return result;
    }

    private int CalculateMultiplicationResult(int[] operands)
    {
        int result = 1;

        for (int i = 0; i < operands.Length; i++)
        {
            result *= operands[i];
        }

        return result;
    }

    private int CalculateDivisionResult(int[] operands)
    {
        int result = operands[0];

        for (int i = 1; i < operands.Length; i++)
        {
            if (operands[i] != 0)
            {
                result /= operands[i];
            }
            else
            {
                result = 0;
            }
        }

        return result;
    }

    public void SetOperationType(int operation)
    {
        switch (operation)
        {
            case 0:
                operationType = MathOperationType.Addition; break;
            case 1:
                operationType = MathOperationType.Subtraction; break;
            case 2:
                operationType = MathOperationType.Multiplication; break;
            case 3:
                operationType = MathOperationType.Division; break;
            default:
                break;
        }
    }
}
