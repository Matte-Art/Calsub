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
    public ExpandableMenu taskTypeMenu;
    public ExpandableMenu difficultiesMenu;
    public ExpandableMenu userMenu;

    public Button checkResultButton;

    private UIController uiController;
    private List<ExpandableMenu> expandableMenus;

    private string playerInputValue;

    public event Action OnCheckResultButtonClicked;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.OnRoundStart += HandleRoundStart;
        gameManager.OnRoundEnd += HandleRoundEnd;
        gameManager.OnIdleEnabled += HandleIdleEnabled;
        uiController = GetComponent<UIController>();

        checkResultButton.onClick.AddListener(() =>
        {
            OnCheckResultButtonClicked?.Invoke();
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

    public void HandleRoundStart(MathTask task)
    {
        checkResultButton.interactable = true;
        EnableKeyboardButtons();
        ResetPlayerInput();
        uiController.UpdatePlayerInput(playerInputValue);

        foreach (var menu in expandableMenus)
        {
            menu.CollapseMenu();
            menu.HideMenu();
        }
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        DisableAllButtons();
    }

    private void HandleIdleEnabled()
    {
        checkResultButton.interactable = true;
        DisableKeyboardButtons();

        foreach (var menu in expandableMenus)
        {
            menu.ShowMenu();
        }
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
            playerInputValue += value;
        }
    }

    public void EnableAllButtons()
    {
        foreach (GameObject button in keyboardButtons)
        {
            button.GetComponent<Button>().interactable = true;
        }

        checkResultButton.interactable = true;
    }

    public void DisableAllButtons()
    {
        foreach (GameObject button in keyboardButtons) 
        { 
            button.GetComponent<Button>().interactable = false;
        }

        checkResultButton.interactable = false;
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


}
