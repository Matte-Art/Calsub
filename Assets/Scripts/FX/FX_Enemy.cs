using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FX_Enemy : MonoBehaviour
{
    Vector3 fightPosition;
    public Vector3 idlePosition;
    Vector2 sizeFight;
    Vector2 sizeIdle;

    public bool isMinimalized;

    public float animationDuration = 0.5f;
    public AnimationCurve positionCurve;
    private void Awake()
    {
        fightPosition = GetComponent<RectTransform>().anchoredPosition3D;


        sizeFight = GetComponent<RectTransform>().sizeDelta;
        sizeIdle = new Vector2(sizeFight.x / 4, sizeFight.y / 4);
    }
    public void ShowEnemy()
    {
        if (!isMinimalized)
        {
            ToggleEnemy();
        }
    }
    public void CollapseEnemy()
    {
        if (isMinimalized)
        {
            ToggleEnemy();
        }
    }

    public void ToggleEnemy()
    {
        Vector2 startSize;
        Vector2 endSize;
        Vector2 startPosition;
        Vector2 endPosition;

        if (!isMinimalized)
        {
            startSize = sizeIdle;
            endSize = sizeFight;
            startPosition = idlePosition;
            endPosition = fightPosition;
        }
        else
        {
            startSize = sizeFight;
            endSize = sizeIdle;
            startPosition = fightPosition;
            endPosition = idlePosition;
        }

        StartCoroutine(MoveAndScaleCoroutine(gameObject, startPosition, endPosition, startSize, endSize));
    }

    private IEnumerator MoveAndScaleCoroutine(GameObject enemy, Vector3 startPos, Vector3 endPos, Vector3 startSize, Vector3 endSize)
    {
        RectTransform enemyRectTransform = enemy.GetComponent<RectTransform>();

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

            enemyRectTransform.anchoredPosition3D = lerpedPosition;

            Vector2 lerpedSize = new Vector2(
                Mathf.Lerp(startSize.x, endSize.x, curveValue),
                Mathf.Lerp(startSize.y, endSize.y, curveValue)
            );

            enemyRectTransform.sizeDelta = lerpedSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyRectTransform.anchoredPosition3D = endPos;
        enemyRectTransform.sizeDelta = endSize;

        isMinimalized = !isMinimalized;
    }
}
