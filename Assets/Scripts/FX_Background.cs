using UnityEngine;
using UnityEngine.UI;

public class FX_Background : MonoBehaviour
{
    public Image background;
    public ParticleSystem particleSys;
    public Material particleMaterial;
    public float duration = 1f;

    private Color initialColor;
    private float timer;

    private void Start()
    {
        initialColor = background.color;
    }

    public void ChangeBackgroundColor(Color color)
    {
        StartCoroutine(ColorChangeCoroutine(color));
    }

    private System.Collections.IEnumerator ColorChangeCoroutine(Color targetColor)
    {
        timer = 0f;

        while (timer < duration)
        {
            float normalizedTime = timer / duration;
            background.color = Color.Lerp(initialColor, targetColor, normalizedTime);

            timer += Time.deltaTime;
            yield return null;
        }
        initialColor = targetColor;
        background.color = targetColor;
    }

    public void SetBackgroundParticleType(MathOperationType type)
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
}