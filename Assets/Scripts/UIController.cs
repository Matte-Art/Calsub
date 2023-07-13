using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject task;
    public GameObject playerInput;
    public GameObject score;
    public GameObject streak;
    public GameObject uiInventory;
    //public GameObject time;

    public Image timeCircle;

    private TextMeshProUGUI taskText;
    private TextMeshProUGUI playerInputText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI streakText;
    //private TextMeshProUGUI timeText;

    void Awake()
    {
        
        taskText = task.GetComponent<TextMeshProUGUI>();
        playerInputText = playerInput.GetComponent<TextMeshProUGUI>();
        scoreText = score.GetComponent<TextMeshProUGUI>();
        streakText = streak.GetComponent<TextMeshProUGUI>();
        //timeText = time.GetComponent<TextMeshProUGUI>();

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.OnRoundStart += HandleRoundStart;
        gameManager.OnRoundEnd += HandleRoundEnd;
        gameManager.OnIdleEnabled += HandleIdleEnabled;
    }

    private void Start()
    {
        ScoreManager.Instance.OnScoreChange += UpdateScoreText;
        ScoreManager.Instance.OnStreakChange += UpdateStreakText;
    }

    private void HandleRoundStart(MathTask task)
    {
        UpdateTaskText(task.MathTaskToString());
        UpdateTaskFontSize(task.operands.Length);
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        UpdateTaskText(args.Task.result.ToString());
        UpdateTaskFontSize(2);
    }

    private void HandleIdleEnabled()
    {
        UpdatePlayerInput(string.Empty);
    }

    public void UpdateTaskText(string text)
    {
        taskText.text = text;  
    }
    public void UpdateTaskFontSize(int fontSize) 
    {
        switch (fontSize)
        {
            case 2:
                taskText.fontSize = 36;
                break;
            case 3:
                taskText.fontSize = 24;
                break;
            case 4:
                taskText.fontSize = 18;
                break;
            default:
                taskText.fontSize = 36;
                break;
        }
    }
    public void UpdatePlayerInput(string text)
    {
        playerInputText.text = text;
    }

    private void UpdateScoreText(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    private void UpdateStreakText(int newStreak)
    {
        streakText.text = newStreak.ToString();
    }

    public void UpdateTimeText(string text)
    {
        //timeText.text = text;
    }

    public void ChangeColorByTime(float timePart)
    {
        Color startColor = Color.white;
        Color endColor = Color.cyan;

        Color lerpedColor = Color.Lerp(endColor, startColor, timePart);

        timeCircle.color = lerpedColor;
    }

    public string GenerateRandomString(int length)
    {
        string characters = "-+×÷1234567890";
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            int index = UnityEngine.Random.Range(0, characters.Length);
            sb.Append(characters[index]);
        }

        return sb.ToString();
    }

    public void ShowInventoryUI()
    {
        uiInventory.gameObject.SetActive(true);
    }
}