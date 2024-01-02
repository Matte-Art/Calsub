using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FX_Check_Result : MonoBehaviour
{
    public Image buttonImage;
    public Image buttonFillImage;

    private float correctFXduration = 0.3f;
    private float correctFXmaxScale = 2f;

    private Coroutine correctFXCoroutine;
    private Coroutine wrongFXCoroutine;

    private void Start()
    {
        buttonFillImage.gameObject.SetActive(false);
    }

    public void PlayCorrectResultFX()
    {
        if (correctFXCoroutine != null)
        {
            StopCoroutine(correctFXCoroutine);
        }

        correctFXCoroutine = StartCoroutine(CorrectResultFXCoroutine());
    }

    private System.Collections.IEnumerator CorrectResultFXCoroutine()
    {
        buttonFillImage.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float startScale = 1f;
        float endScale = correctFXmaxScale;
        float startAlpha = 0.7f;
        float endAlpha = 0f;

        while (elapsedTime < correctFXduration)
        {
            float t = elapsedTime / correctFXduration;
            float scale = Mathf.Lerp(startScale, endScale, t);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            buttonFillImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
            buttonFillImage.color = new Color(1f, 1f, 1f, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        buttonFillImage.rectTransform.localScale = new Vector3(startScale, startScale, startScale);
        buttonFillImage.gameObject.SetActive(false);
    }

    public void PlayWrongResultFX()
    {
        if (wrongFXCoroutine != null)
        {
            StopCoroutine(wrongFXCoroutine);
        }

        wrongFXCoroutine = StartCoroutine(WrongResultFXCoroutine());
    }

    private System.Collections.IEnumerator WrongResultFXCoroutine()
    {
        buttonFillImage.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float duration = GameManager.roundDelay;

        Color startColor = new Color(1f, 0.1f, 0.1f, 1f);
        Color endColor = new Color(1f, 0.1f, 0.1f, 0.0f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / correctFXduration;
            Color lerpedColor = Color.Lerp(startColor, endColor, t);

            buttonFillImage.color = lerpedColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        buttonFillImage.color = new Color(1f, 1f, 1f, 0.5f);
        buttonFillImage.gameObject.SetActive(false);
    }
}
