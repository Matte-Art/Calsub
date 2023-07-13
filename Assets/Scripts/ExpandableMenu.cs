using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedButton
{
    public GameObject Button { get; set; }
    public Coroutine AnimationCoroutine { get; set; }
    public Vector3 ShownPosition { get; set; }
    public Vector3 HiddenPosition { get; set; }
    public AnimatedButton(GameObject button, Coroutine coroutine, Vector3 shownPosition, Vector3 hiddenPosition)
    {
        Button = button;
        AnimationCoroutine = coroutine;
        ShownPosition = shownPosition;
        HiddenPosition = hiddenPosition;
    }
}

public class ExpandableMenu : MonoBehaviour
{
    public GameObject menuText;
    public GameObject[] buttonList;

    public bool isExpanded = true;
    public bool isHidden = false;

    public AnimationCurve positionCurve;
    public float animationDuration;
    public float delayBetweenAnimations;

    public RectTransform menuHidenPosition;

    private AnimatedButton animatedMenuButton;
    public List<AnimatedButton> animatedChildButtons;
    private AnimatedButton selectedButton;

    void Awake()
    {
        animatedChildButtons = new List<AnimatedButton>();

        var menuButtonShownPosition = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        var menuButtonHiddenPosition = new Vector3(menuButtonShownPosition.x, menuHidenPosition.anchoredPosition3D.y, 0);
        animatedMenuButton = new(gameObject, null, menuButtonShownPosition, menuButtonHiddenPosition);

        foreach (var button in buttonList)
        {
            var shownPosition = button.GetComponent<RectTransform>().anchoredPosition3D;
            var hiddenPosition = Vector3.zero;

            AnimatedButton animatedButton = new(button, null, shownPosition,  hiddenPosition);
            animatedChildButtons.Add(animatedButton);
            button.GetComponent<Button>().onClick.AddListener(() => MakeButtonSelected(animatedButton));
        }
    }
    private void Start()
    {
        DisableAllAnimatedButtons();
    }

    public void MakeButtonSelected(AnimatedButton button)
    {
        selectedButton = button;
        foreach (var animatedButton in animatedChildButtons)
        {
            if (animatedButton != selectedButton)
            {
                animatedButton.Button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                animatedButton.Button.GetComponent<Image>().color = Color.white;
            }
            else
            {
                animatedButton.Button.GetComponentInChildren<TextMeshProUGUI>().color = Color.cyan;
                animatedButton.Button.GetComponent<Image>().color = Color.cyan;
            }  
        }
    }
    public void ShowMenu()
    {
        if (isHidden)
        {
            StartCoroutine(ToggleMenuCoroutine());
        }
    }

    public void HideMenu()
    {
        if (!isHidden)
        {
            StartCoroutine(ToggleMenuCoroutine());
        }
    }

    private IEnumerator ToggleMenuCoroutine()
    {
        var startPos = isHidden ? animatedMenuButton.HiddenPosition : animatedMenuButton.ShownPosition;
        var endPos = isHidden ? animatedMenuButton.ShownPosition : animatedMenuButton.HiddenPosition;

        StopAndRemoveCoroutine(animatedMenuButton);

        Coroutine coroutine = StartCoroutine(MoveButtonCoroutine(animatedMenuButton.Button, startPos, endPos));
        animatedMenuButton.AnimationCoroutine = coroutine;

        yield return new WaitForSeconds(delayBetweenAnimations);
        isHidden = !isHidden;
    }

    public void CollapseMenu()
    {
        if (isExpanded)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        StartCoroutine(SuspendAllButtons());
        StartCoroutine(AnimateButtonsCoroutine());
    }

    private IEnumerator AnimateButtonsCoroutine()
    {
        gameObject.GetComponent<Button>().interactable = false;

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
        gameObject.GetComponent<Button>().interactable = false;

        foreach (var button in animatedChildButtons)
        {
            button.Button.GetComponent<Button>().interactable = false;
        }

        yield return new WaitForSeconds(animationDuration + (buttonList.Length * delayBetweenAnimations));

        foreach (var button in animatedChildButtons)
        {
            button.Button.GetComponent<Button>().interactable = true;
        }

        gameObject.GetComponent<Button>().interactable = true;
    }

    //private void DeactiveAllAnimatedButtons()
    //{
    //    foreach (var button in animatedButtons) 
    //    {
    //        DeactiveAnimatedButton(button);
    //    }
    //}

    //private void DeactiveAnimatedButton(AnimatedButton button)
    //{
    //    button.Button.GetComponent<Button>().interactable = false;
    //}

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

    private void DisableAllAnimatedButtons()
    {
        foreach (var button in animatedChildButtons)
        {
            button.Button.SetActive(false);
        }
    }
}
