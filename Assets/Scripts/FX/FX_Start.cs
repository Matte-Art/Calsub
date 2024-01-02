using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FX_Start : MonoBehaviour
{
    private GameManager gameManager;
    private UIController uiController;

    private readonly float idleAnimationDelay = 0.5f;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiController = gameManager.GetComponent<UIController>();
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

    public void PlayRandomTextFX()
    {
        StartCoroutine(IdleTextAnimationCoroutine());
    }
    
    private IEnumerator IdleTextAnimationCoroutine()
    {
        while (!gameManager.isRunning)
        {
            uiController.UpdateRandomNumbersText(GenerateRandomString(6));
            yield return new WaitForSeconds(idleAnimationDelay);
        }
    }
}
