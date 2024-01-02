using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FX_Keyboard : BaseAnimatable
{
    public GameObject[] buttonList;

    public bool isHidden = false;

    public AnimationCurve positionCurve;
    public float popAnimationTime = 0.2f;
    public float beetwenAnimationTime;

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

    public override void Toggle()
    {
        beetwenAnimationTime = GameManager.roundDelay / buttonList.Length;

        StartCoroutine(SuspendButtons(popAnimationTime + beetwenAnimationTime, animatedChildButtons));
        StartCoroutine(Animate());
    }

    public override IEnumerator Animate()
    {
        var startSize = isExpanded ? Vector3.one : Vector3.zero;
        var endSize = isExpanded ? Vector3.zero : Vector3.one;

        foreach (var button in animatedChildButtons)
        {
            button.Button.transform.localScale = startSize;
        }
        foreach (var button in animatedChildButtons)
        {
            button.Button.transform.DOScale(endSize, popAnimationTime).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(beetwenAnimationTime);
        }

        isExpanded = !isExpanded;
    }
}
