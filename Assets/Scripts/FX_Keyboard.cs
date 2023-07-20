using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FX_Keyboard : MonoBehaviour
{
    public GameObject[] buttonList;

    public bool isExpanded = true;
    public bool isHidden = false;

    public AnimationCurve positionCurve;
    public float animationDuration = 0.5f;
    public float delayBetweenAnimations = 0.1f;

    public RectTransform hiddenPosition;

    public List<AnimatedButton> animatedChildButtons;

    void Awake()
    {
        animatedChildButtons = new List<AnimatedButton>();

        foreach (var button in buttonList)
        {
            var shownPosition = button.GetComponent<RectTransform>().anchoredPosition3D;
            var hidePosition = this.hiddenPosition.anchoredPosition3D;

            AnimatedButton animatedButton = new(button, null, shownPosition, hidePosition);
            animatedChildButtons.Add(animatedButton);
        }
    }
    public void ShowKeyboard()
    {
        if (!isExpanded)
        {
            ToggleKeyboard();
        }
    }
    public void CollapseKeyboard()
    {
        if (isExpanded)
        {
            ToggleKeyboard();
        }
    }

    public void ToggleKeyboard()
    {
        StartCoroutine(SuspendAllButtons());
        StartCoroutine(AnimateButtonsCoroutine());
    }

    private IEnumerator AnimateButtonsCoroutine()
    {
        int startIndex = isExpanded ? 0 : animatedChildButtons.Count - 1;
        int endIndex = isExpanded ? animatedChildButtons.Count : -1;
        int step = isExpanded ? 1 : -1;

        for (int i = startIndex; i != endIndex; i += step)
        {
            var animatedButton = animatedChildButtons[i];
            var startPos = isExpanded ? animatedButton.ShownPosition : animatedButton.HiddenPosition;
            var endPos = isExpanded ? animatedButton.HiddenPosition : animatedButton.ShownPosition;

            StopAndRemoveCoroutine(animatedButton);

            if (!isExpanded)
            {
                animatedButton.Button.SetActive(true);
            }

            Coroutine coroutine = StartCoroutine(MoveButtonCoroutine(animatedButton.Button, startPos, endPos));
            animatedButton.AnimationCoroutine = coroutine;

            if (isExpanded)
            {
                StartCoroutine(DeactivateAfterAnimation(animatedButton));
            }

            yield return new WaitForSeconds(delayBetweenAnimations);
        }

        isExpanded = !isExpanded;
    }

    private IEnumerator SuspendAllButtons()
    {
        foreach (var button in animatedChildButtons)
        {
            button.Button.GetComponent<Button>().interactable = false;
        }

        yield return new WaitForSeconds(animationDuration + (buttonList.Length * delayBetweenAnimations));

        foreach (var button in animatedChildButtons)
        {
            button.Button.GetComponent<Button>().interactable = true;
        }
    }

    private IEnumerator DeactivateAfterAnimation(AnimatedButton animatedButton)
    {
        yield return new WaitForSeconds(animationDuration);

        animatedButton.Button.SetActive(false);
    }

    private void StopAndRemoveCoroutine(AnimatedButton animatedButton)
    {
        if (animatedButton.AnimationCoroutine != null)
        {
            StopCoroutine(animatedButton.AnimationCoroutine);
        }

        animatedButton.AnimationCoroutine = null;
    }

    private IEnumerator MoveButtonCoroutine(GameObject button, Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = positionCurve.Evaluate(t); // Próbkowanie AnimationCurve

            Vector3 lerpedPosition = new Vector3(
                Mathf.Lerp(startPos.x, endPos.x, curveValue),
                Mathf.Lerp(startPos.y, endPos.y, curveValue),
                Mathf.Lerp(startPos.z, endPos.z, curveValue)
            );

            button.GetComponent<RectTransform>().anchoredPosition3D = lerpedPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.GetComponent<RectTransform>().anchoredPosition3D = endPos;
    }
}
