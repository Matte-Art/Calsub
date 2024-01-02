using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public GameObject[] keyboardButtons;
    public GameObject[] idleButtons;
    public GameObject startObject;
    public GameObject checkResultObject;

    public Button startButton;
    public Button checkResultButton;

    public ExpandableMenu taskTypeMenu;
    public ExpandableMenu difficultiesMenu;
    public ExpandableMenu userMenu;

    private UIController uiController;
    private List<ExpandableMenu> expandableMenus;

    private string playerInputValue;

    public event Action<MathOperationType> OnMathOperationTypeChanged;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiController = GetComponent<UIController>();
        startButton = startObject.GetComponentInChildren<Button>();
        checkResultButton = checkResultObject.GetComponentInChildren<Button>();

        gameManager.OnGameStarting += HandleOnGameStarting;
        gameManager.OnRoundStart += HandleRoundStart;
        gameManager.OnRoundEnd += HandleRoundEnd;
        gameManager.OnIdleEnabled += HandleIdleEnabled;

        startButton.onClick.AddListener(() =>
        {
            HandleStartButtonClick();
        });

        checkResultButton.onClick.AddListener(() =>
        {
            HandleCheckResultButtonClick();
        });

        for (int i = 0; i < keyboardButtons.Length; i++)
        {
            Button button = keyboardButtons[i].GetComponent<Button>();
            string value = button.GetComponentInChildren<TextMeshProUGUI>().text;

            button.onClick.AddListener(() =>
            {
                HandleButtonClick(value.ToString());
                uiController.UpdatePlayerInput(playerInputValue);
            });
        }
        expandableMenus = new List<ExpandableMenu>
        {
            taskTypeMenu,
            difficultiesMenu,
            userMenu
        };

        for (int i = 0; i < taskTypeMenu.buttonList.Length; i++)
        {
            int index = i;

            taskTypeMenu.buttonList[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                OnMathOperationTypeChanged?.Invoke((MathOperationType)index);
            });
        }
    }

    private void Start()
    {
        SetOptionsButtons();
    }

    public void SetOptionsButtons()
    {
        var taskType = gameManager.GetComponent<MathTaskGenerator>().operationType;
        var difficulty = DifficultyManager.Instance.selectedDifficulty;

        switch (taskType)
        {
            case MathOperationType.Addition:
                taskTypeMenu.MakeButtonSelected(taskTypeMenu.animatedChildButtons[0]);
                break;
            case MathOperationType.Subtraction:
                taskTypeMenu.MakeButtonSelected(taskTypeMenu.animatedChildButtons[1]);
                break;
            case MathOperationType.Multiplication:
                taskTypeMenu.MakeButtonSelected(taskTypeMenu.animatedChildButtons[2]);
                break;
            case MathOperationType.Division:
                taskTypeMenu.MakeButtonSelected(taskTypeMenu.animatedChildButtons[3]);
                break;
            default:
                taskTypeMenu.MakeButtonSelected(taskTypeMenu.animatedChildButtons[0]);
                break;
        }

        switch (difficulty)
        {
            case Difficulty.Easy:
                difficultiesMenu.MakeButtonSelected(difficultiesMenu.animatedChildButtons[0]);
                break;
            case Difficulty.Medium:
                difficultiesMenu.MakeButtonSelected(difficultiesMenu.animatedChildButtons[1]);
                break;
            case Difficulty.Hard:
                difficultiesMenu.MakeButtonSelected(difficultiesMenu.animatedChildButtons[2]);
                break;
            case Difficulty.Extreme:
                difficultiesMenu.MakeButtonSelected(difficultiesMenu.animatedChildButtons[3]);
                break;
            default:
                difficultiesMenu.MakeButtonSelected(difficultiesMenu.animatedChildButtons[0]);
                break;
        }
    }

    private void HandleOnGameStarting()
    {
        startObject.SetActive(false);
        foreach (var menu in expandableMenus)
        {
            menu.CollapseMenu();
            menu.HideMenu();
        }
    }

    public void HandleRoundStart(MathTask task)
    {
        checkResultObject.SetActive(true);
        checkResultButton.GetComponent<Button>().interactable = true;

        EnableKeyboardButtons();
        ResetPlayerInput();
        uiController.UpdatePlayerInput(playerInputValue);
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        DisableAllButtons();
    }

    private void HandleIdleEnabled()
    {
        DisableKeyboardButtons();

        foreach (var menu in expandableMenus)
        {
            menu.ShowMenu();
        }

        startObject.SetActive(true);
        checkResultObject.SetActive(false);
    }

    public string GetPlayerInput() => playerInputValue;

    public void ResetPlayerInput() => playerInputValue = string.Empty;

    private void HandleButtonClick(string value)
    {
        if (value == "<")
        {
            if (!string.IsNullOrEmpty(playerInputValue))
            {
                playerInputValue = playerInputValue.Substring(0, playerInputValue.Length - 1);
            }
        }
        else
        {
            if (playerInputValue.Length <= 9)
            {
                playerInputValue += value;
            }  
        }
    }

    public void EnableAllButtons()
    {
        foreach (GameObject button in keyboardButtons)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }

    public void DisableAllButtons()
    {
        foreach (GameObject button in keyboardButtons) 
        { 
            button.GetComponent<Button>().interactable = false;
        }

        checkResultButton.GetComponent<Button>().interactable = false;
    }

    public void EnableKeyboardButtons()
    {
        foreach (GameObject button in keyboardButtons)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }

    public void DisableKeyboardButtons()
    {
        foreach (GameObject button in keyboardButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    private void HandleStartButtonClick()
    {
        gameManager.StartRoundOnClick();
    }

    private void HandleCheckResultButtonClick()
    {
        gameManager.ValidateInputAndStartNewRound();
    }
}