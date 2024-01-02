using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAnimatable : MonoBehaviour, IAnimatable
{
    protected bool isExpanded = true;

    public void Show()
    {
        if (!isExpanded)
        {
            Toggle();
        }
    }

    public void Collapse()
    {
        if (isExpanded)
        {
            Toggle();
        }
    }

    public virtual void Toggle()
    {
        StartCoroutine(Animate());
    }

    public virtual IEnumerator Animate()
    {
        yield return new WaitForSeconds(0f);
    }

    public IEnumerator SuspendButtons(float time, List<AnimatedButton> animatedButtons)
    {
        foreach (var button in animatedButtons)
        {
            button.Button.GetComponent<Button>().interactable = false;
        }

        yield return new WaitForSeconds(time);

        foreach (var button in animatedButtons)
        {
            button.Button.GetComponent<Button>().interactable = true;
        }
    }
}