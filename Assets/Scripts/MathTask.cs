public class MathTask
{
    public int[] operands; // Tablica operandów
    public float result; // Wynik
    public MathOperationType operationType; // Typ operacji matematycznej

    public MathTask(int[] operands, int result, MathOperationType operationType)
    {
        this.operands = operands;
        this.result = result;
        this.operationType = operationType;
    }

    public bool CheckResult(float playerResult)
    {
        return playerResult == result;
    }

    public string MathTaskToString()
    {
        string taskString = "";

        for (int i = 0; i < operands.Length; i++)
        {
            taskString += operands[i].ToString();

            if (i < operands.Length - 1)
            {
                switch (operationType)
                {
                    case MathOperationType.Addition:
                        taskString += " + ";
                        break;
                    case MathOperationType.Subtraction:
                        taskString += " - ";
                        break;
                    case MathOperationType.Multiplication:
                        taskString += " × ";
                        break;
                    case MathOperationType.Division:
                        taskString += " ÷ ";
                        break;
                    default:
                        break;
                }
            }
        }
        //taskString += " = ";
        return taskString;
    }
}
