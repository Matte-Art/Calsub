using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public event Action<int> OnScoreChange;
    public event Action<int> OnStreakChange;

    private int score;
    private int streak;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        OnScoreChange?.Invoke(score);
    }

    public void DecreaseScore(int amount)
    {
        score -= amount;
        OnScoreChange?.Invoke(score);
    }

    public void IncreaseStreak(int amount)
    {
        streak += amount;
        OnStreakChange?.Invoke(streak);
    }

    public void ResetStreak()
    {
        streak = 0;
        OnStreakChange?.Invoke(streak);
    }

    public int GetStreak()
    {
        return streak;
    }
}