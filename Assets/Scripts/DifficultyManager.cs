using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Extreme
}

[System.Serializable]
public class DifficultyLevelList
{
    public List<DifficultyLevel> additionDifficulties;
    public List<DifficultyLevel> subtractionDifficulties;
    public List<DifficultyLevel> multiplicationDifficulties;
    public List<DifficultyLevel> divisionDifficulties;
}

[System.Serializable]
public class DifficultyLevel
{
    public int streak;
    public int minValue;
    public int maxValue;
    public int operandCount;
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    public Difficulty selectedDifficulty;

    public List<DifficultyLevel> additionDifficulties = new();
    public List<DifficultyLevel> subtractionDifficulties = new();
    public List<DifficultyLevel> multiplicationDifficulties = new();
    public List<DifficultyLevel> divisionDifficulties = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(int difficulty)
    {
        selectedDifficulty = (Difficulty)difficulty;
    }

    public List<int> GetDifficultyParameters(MathOperationType operationType, int currentStreak)
    {
        Dictionary<MathOperationType, List<DifficultyLevel>> difficultyLevels = new()
        {
            { MathOperationType.Addition, additionDifficulties },
            { MathOperationType.Subtraction, subtractionDifficulties },
            { MathOperationType.Multiplication, multiplicationDifficulties },
            { MathOperationType.Division, divisionDifficulties }
        };

        Dictionary<Difficulty, int> extraDifficulties = new()
        {
            { Difficulty.Easy, 0 },
            { Difficulty.Medium, 10 },
            { Difficulty.Hard, 20 },
            { Difficulty.Extreme, 30 },
        };

        if (!difficultyLevels.TryGetValue(operationType, out List<DifficultyLevel> difficulty))
        {
            Debug.LogError("Difficulty levels not found for operation type: " + operationType);
            return new List<int>();
        }

        if (!extraDifficulties.TryGetValue(selectedDifficulty, out int extraStreak))
        {
            Debug.LogError("Extra difficulty not found for selected difficulty: " + selectedDifficulty);
            return new List<int>();
        }
        int minValue = 0, maxValue = 0, operandCount = 0;

        foreach (DifficultyLevel level in difficulty)
        {
            int modifiedStreak = extraStreak + currentStreak;

            if (modifiedStreak >= level.streak)
            {
                minValue = level.minValue;
                maxValue = level.maxValue;
                operandCount = level.operandCount;
            }
        }

        return new List<int>() { minValue , maxValue, operandCount};
    }

    public void LoadDifficultyLevelsFromFile(string fileName)
    {
        TextAsset file = Resources.Load(fileName) as TextAsset;

        if (file != null)
        {
            string json = file.ToString();
            DifficultyLevelList difficultyLevelList = JsonUtility.FromJson<DifficultyLevelList>(json);

            additionDifficulties = difficultyLevelList.additionDifficulties;
            subtractionDifficulties = difficultyLevelList.subtractionDifficulties;
            multiplicationDifficulties = difficultyLevelList.multiplicationDifficulties;
            divisionDifficulties = difficultyLevelList.divisionDifficulties;
        }
        else
        {
            Debug.LogError("Difficulty levels file not found: " + file);
        }
    }
}
