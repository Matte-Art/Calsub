using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEndEventArgs : EventArgs
{
    public MathTask Task;
    public float ExtraTime;
    public bool IsTaskCorrect;
    public RoundEndEventArgs(MathTask task, float extraTime, bool isCorrect)
    {
        Task = task;
        ExtraTime = extraTime;
        IsTaskCorrect = isCorrect;
    }
}

public class GameManager : MonoBehaviour
{
    public float roundTimeLimit = 10f;
    public float extraTime = 0f;

    public static float roundDelay = 1f;

    private static float initialExtraTime;

    private MathTaskGenerator taskGenerator;
    private ScoreManager scoreManager;
    private UIController uiController;
    private InputController inputController;
    private MathTask currentTask;
    private FXManager fxManager;
    private float timer;
    public bool isRunning = false;
    private bool answeredInTime = false;

    public event Action OnGameStarting;
    public event Action<MathTask> OnRoundStart;
    public event Action<RoundEndEventArgs> OnRoundEnd;
    public event Action OnIdleEnabled;

    void Awake()
    {
        taskGenerator = GetComponent<MathTaskGenerator>();
        uiController = GetComponent<UIController>();
        inputController = GetComponent<InputController>();
        scoreManager = GetComponent<ScoreManager>();
        fxManager = GetComponent<FXManager>();
    }
    void Start()
    {
        EnableIdleMode();

        
        for (int i = 0; i < 10; i++)
        {
            float max = 50;
            float chanceForHighValue = 1f; // 5% szansa na bardzo du�y bonus
            float highValueMultiplier = Mathf.Pow(2f, UnityEngine.Random.Range(1f, 3f)); // Losowy mno�nik dla bardzo du�ego bonusu

            if (UnityEngine.Random.value < chanceForHighValue)
            {
                max *= highValueMultiplier;
            }
        }
    }

    private void EnableIdleMode()
    {
        isRunning = false;
        OnIdleEnabled?.Invoke();
    }
    private void UpdateGeneratorDifficulty()
    {
        var operationType = taskGenerator.operationType;
        var currentStreak = ScoreManager.Instance.GetStreak();

        var parameters = DifficultyManager.Instance.GetDifficultyParameters(operationType, currentStreak);

        int minValue = parameters[0];
        int maxValue = parameters[1];
        int operandCount = parameters[2];

        taskGenerator.UpdateParameters(minValue, maxValue, operandCount);
    }

    public float? GetPlayerInput()
    {
        string playerInput = inputController.GetPlayerInput();

        if (string.IsNullOrEmpty(playerInput))
        {
            return null;
        }

        if (float.TryParse(playerInput, out float result))
        {
            return result;
        }

        return null;
    }

    public void NewRound()
    {
        bool taskCorrect = EndRound();

        fxManager.fxTimer.PlayFillTimerAndExtraTimer(extraTime / 10f);

        if (taskCorrect)
        {
            Invoke(nameof(StartRound), roundDelay);
        }
        else
        {
            Invoke(nameof(EnableIdleMode), roundDelay);
        }
    }

    public void ValidateInputAndStartNewRound()
    {
        if (inputController.GetPlayerInput() != string.Empty)
        {
            NewRound();
        }
    }

    public void StartRoundOnClick()
    {
        OnGameStarting?.Invoke();

        if (!isRunning)
        {
            Invoke(nameof(StartRound), roundDelay);
        }
    }
    private void StartRound()
    {
        UpdateGeneratorDifficulty();
        currentTask = taskGenerator.GenerateTask();

        StartTimer();

        isRunning = true;
        answeredInTime = true;
        initialExtraTime = extraTime;

        OnRoundStart?.Invoke(currentTask);
    }
    private void StartTimer()
    {
        timer = roundTimeLimit + extraTime;
    }

    public bool EndRound()
    {
        isRunning = false;
        float? playerInput = GetPlayerInput();
        bool taskCorrect = currentTask.CheckResult(playerInput.GetValueOrDefault());

        if (taskCorrect)
        {
            fxManager.fxCheckResult.PlayCorrectResultFX();
            scoreManager.IncreaseScore(1);
            scoreManager.IncreaseStreak(1);

            if (answeredInTime)
            {
                float timePassed = roundTimeLimit - timer;
                float t = Mathf.Clamp01(timePassed / 5f);

                float nextExtraTime = Mathf.Lerp(5f, 0f, t);
                extraTime += nextExtraTime;
                extraTime = Mathf.Clamp(extraTime, 0f, 10f);
            }
        }
        else if (playerInput.HasValue == false)
        {
            scoreManager.DecreaseScore(1);
            scoreManager.ResetStreak();
            fxManager.fxCheckResult.PlayWrongResultFX();
            extraTime = 0f;
        }
        else
        {
            scoreManager.DecreaseScore(1);
            scoreManager.ResetStreak();
            fxManager.fxCheckResult.PlayWrongResultFX();
            extraTime = 0f;
        }

        OnRoundEnd?.Invoke(new RoundEndEventArgs(currentTask, extraTime, taskCorrect));
        return taskCorrect;
    }

    void Update()
    {
        if (isRunning && timer > 0f)
        {
            timer -= Time.deltaTime;
            extraTime -= extraTime > 0f ? Time.deltaTime : 0f;
            answeredInTime = timer >= roundTimeLimit + initialExtraTime - 5f;

            float time = timer / roundTimeLimit;

            fxManager.fxTimer.UpdateTimer(time);
            //uiController.ChangeColorByTime(time);

            if (timer <= 0f)
            {
                NewRound();
            }
        }
    }
}
