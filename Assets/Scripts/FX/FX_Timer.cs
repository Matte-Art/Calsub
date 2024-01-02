using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FX_Timer : BaseAnimatable
{
    public InputController inputController;

    public Image timerImage;
    public Image timerExtraImage;

    public RectTransform timerPivot;

    private ParticleSystem particleSys;
    private Material particleMaterial;

    private Coroutine timerFullFXCoroutine;

    private void Awake()
    {
        particleSys = GetComponentInChildren<ParticleSystem>();
        particleMaterial = particleSys.GetComponent<Renderer>().material;
    }

    public void SetClockParticleType(MathOperationType type)
    {
        switch (type)
        {
            case MathOperationType.Addition:
                particleMaterial.SetTexture("_MainTex", ColorManager.Instance.additionImage);
                break;
            case MathOperationType.Subtraction:
                particleMaterial.SetTexture("_MainTex", ColorManager.Instance.subtractionImage);
                break;
            case MathOperationType.Multiplication:
                particleMaterial.SetTexture("_MainTex", ColorManager.Instance.multiplicationImage);
                break;
            case MathOperationType.Division:
                particleMaterial.SetTexture("_MainTex", ColorManager.Instance.divisionImage);
                break;
        }
    }
    public void UpdateTimer(float fillAmount)
    {
        timerExtraImage.fillAmount = fillAmount - 1;
        timerImage.fillAmount = Mathf.Clamp01(fillAmount);

        var timerWidth = timerImage.GetComponent<RectTransform>().rect.x;
        var currentPositionX = Mathf.Lerp(timerWidth, -timerWidth, fillAmount % 1);
        var staticPosition = timerPivot.transform.localPosition;

        timerPivot.transform.localPosition = new Vector3(currentPositionX, staticPosition.y, staticPosition.z);
    }

    public void PlayFillTimerAndExtraTimer(float extraFill)
    {
        if (timerFullFXCoroutine != null)
        {
            StopCoroutine(timerFullFXCoroutine);
        }

        timerFullFXCoroutine = StartCoroutine(FillTimerAndExtraTimerCoroutine(extraFill));
    }

    private IEnumerator FillTimerAndExtraTimerCoroutine(float extraFill)
    {
        float duration = GameManager.roundDelay;
        float clockStartFill = timerImage.fillAmount;
        float extraStartFill = timerExtraImage.fillAmount;
        float extraEndFill = 1.0f + extraFill;
        
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float fillAmount;
            if (extraFill > 0 && clockStartFill >= 1)
            {
                fillAmount = Mathf.Lerp(1+extraStartFill, extraEndFill, t);
            }
            else
            {
                fillAmount = Mathf.Lerp(clockStartFill, extraEndFill, t);
            }

            timerPivot.rotation = Quaternion.Euler(new Vector3(0, 0, -fillAmount * 360));
            timerImage.fillAmount = fillAmount;
            timerExtraImage.fillAmount = fillAmount - 1;
            yield return null;
        }

        timerFullFXCoroutine = null;
    }


    public void PlayParticleEmission()
    {
        particleSys.Play();
    }

    public void StopParticleEmission()
    {
        particleSys.Stop();
    }

    public void ChangeExtraClockColor(Color color)
    {
        StartCoroutine(ColorChangeCoroutine(color));
    }

    private System.Collections.IEnumerator ColorChangeCoroutine(Color targetColor)
    {
        float elapsedTime = 0f;
        float duration = 1f;
        var initialColor = timerExtraImage.color;

        while (elapsedTime < duration)
        {
            float normalizedTime = elapsedTime / duration;
            timerExtraImage.color = Color.Lerp(initialColor, targetColor, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        initialColor = targetColor;
        timerExtraImage.color = targetColor;
    }

    public override IEnumerator Animate()
    {
        var startSize = isExpanded ? Vector3.one : Vector3.zero;
        var endSize = isExpanded ? Vector3.zero : Vector3.one;

        timerExtraImage.transform.localScale = startSize;
        timerImage.transform.localScale = startSize;

        timerExtraImage.transform.DOScale(endSize, GameManager.roundDelay).SetEase(Ease.OutExpo);
        timerImage.transform.DOScale(endSize, GameManager.roundDelay).SetEase(Ease.OutExpo);

        yield return new WaitForSeconds(GameManager.roundDelay);

        isExpanded = !isExpanded;
    }
}
