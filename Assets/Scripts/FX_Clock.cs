using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FX_Clock : MonoBehaviour
{
    public InputController inputController;

    public Image clockImage;
    public Image clockExtraImage;

    private RectTransform clockPivot;
    private ParticleSystem particleSys;
    private Material particleMaterial;

    public Image correctCircle;
    public float correctFXduration = 0.3f;
    public float correctFXmaxScale = 2f;

    public Image fillCircle;

    private Coroutine circleFullFXCoroutine;
    private Coroutine correctFXCoroutine;
    private Coroutine wrongFXCoroutine;

    private void Awake()
    {
        particleSys = GetComponentInChildren<ParticleSystem>();
        clockPivot = GetComponent<RectTransform>();
        particleMaterial = particleSys.GetComponent<Renderer>().material;


    }
    private void Start()
    {
        correctCircle.gameObject.SetActive(false);
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
    public void UpdateClockCircle(float fillAmount)
    {
        clockExtraImage.fillAmount = fillAmount - 1;
        clockPivot.rotation = Quaternion.Euler(new Vector3(0, 0, -fillAmount * 360));
        clockImage.fillAmount = Mathf.Clamp01(fillAmount);
    }

    public void PlayFillClockAndExtraTime(float extraFill)
    {
        if (circleFullFXCoroutine != null)
        {
            StopCoroutine(circleFullFXCoroutine);
        }

        circleFullFXCoroutine = StartCoroutine(FillClockAndExtraTimeCoroutine(extraFill));
    }

    private IEnumerator FillClockAndExtraTimeCoroutine(float extraFill)
    {
        float duration = GameManager.roundDelay;
        float clockStartFill = clockImage.fillAmount;
        float extraStartFill = clockExtraImage.fillAmount;
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

            clockPivot.rotation = Quaternion.Euler(new Vector3(0, 0, -fillAmount * 360));
            clockImage.fillAmount = fillAmount;
            clockExtraImage.fillAmount = fillAmount - 1;
            yield return null;
        }

        circleFullFXCoroutine = null;
    }


    public void PlayParticleEmission()
    {
        particleSys.Play();
    }

    public void StopParticleEmission()
    {
        particleSys.Stop();
    }

    public void PlayCorrectCircleFX()
    {
        if (correctFXCoroutine != null)
        {
            StopCoroutine(correctFXCoroutine);
        }

        correctFXCoroutine = StartCoroutine(CorrectCircleFXCoroutine());
    }

    private System.Collections.IEnumerator CorrectCircleFXCoroutine()
    {
        correctCircle.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float startScale = 2f;
        float endScale = correctFXmaxScale;
        float startAlpha = 0.7f;
        float endAlpha = 0f;

        while (elapsedTime < correctFXduration)
        {
            float t = elapsedTime / correctFXduration;
            float scale = Mathf.Lerp(startScale, endScale, t);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            correctCircle.rectTransform.localScale = new Vector3(scale, scale, 1f);
            correctCircle.color = new Color(1f, 1f, 1f, alpha);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        correctCircle.gameObject.SetActive(false);
    }

    public void PlayWrongCircleFX()
    {
        if (wrongFXCoroutine != null)
        {
            StopCoroutine(wrongFXCoroutine);
        }

        wrongFXCoroutine = StartCoroutine(WrongCircleFXCoroutine());
    }

    private System.Collections.IEnumerator WrongCircleFXCoroutine()
    {
        float elapsedTime = 0f;
        float duration = GameManager.roundDelay;

        Color startColor = new Color(1f, 0.1f, 0.1f, 1f);
        Color endColor = new Color(1f, 0.1f, 0.1f, 0.0f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / correctFXduration;
            Color lerpedColor = Color.Lerp(startColor, endColor, t);

            fillCircle.color = lerpedColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fillCircle.color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void ChangeExtraClockColor(Color color)
    {
        StartCoroutine(ColorChangeCoroutine(color));
    }

    private System.Collections.IEnumerator ColorChangeCoroutine(Color targetColor)
    {
        float elapsedTime = 0f;
        float duration = 1f;
        var initialColor = clockExtraImage.color;

        while (elapsedTime < duration)
        {
            float normalizedTime = elapsedTime / duration;
            clockExtraImage.color = Color.Lerp(initialColor, targetColor, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        initialColor = targetColor;
        clockExtraImage.color = targetColor;
    }
}
