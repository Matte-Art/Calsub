using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FX_Shooting : MonoBehaviour
{
    public GameObject currentBullet;

    public GameObject bulletPrefabAddition;
    public GameObject bulletPrefabSubtraction;
    public GameObject bulletPrefabMultiplication;
    public GameObject bulletPrefabDivision;

    public Transform enemyPosition;
    public float sA, eA;
    private float shootingDuration = 0.5f;
    private float minRotationSpeed = 10f;
    private float maxRotationSpeed = 20f;
    private float maxCurveAmount = 400.0f;
    private float circleRadius = 100f;

    private void Start()
    {
        StartShooting(10);
    }

    public void StartShooting(int bulletsAmount)
    {
        StartCoroutine(ShootBullets(bulletsAmount));
    }

    private IEnumerator ShootBullets(int bulletsAmount)
    {
        for (int i = 0; i < bulletsAmount; i++)
        {
            ShootBullet();
            yield return new WaitForSeconds(shootingDuration / bulletsAmount);
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(currentBullet, GetRandomCirclePosition(), Quaternion.identity, gameObject.transform);
        StartCoroutine(MoveBullet(bullet));
    }

    public void ChangeBulletType(MathOperationType operationType)
    {
        switch (operationType)
        {
            case MathOperationType.Addition:
                currentBullet = bulletPrefabAddition;
                break;
            case MathOperationType.Subtraction:
                currentBullet = bulletPrefabSubtraction;
                break;
            case MathOperationType.Division:
                currentBullet = bulletPrefabDivision;
                break;
            case MathOperationType.Multiplication:
                currentBullet = bulletPrefabMultiplication;
                break;
        }
    }

    public void ChangeBulletColor(Color color)
    {
        currentBullet.GetComponent<Image>().color = color;
    }

    private Vector3 GetRandomCirclePosition()
    {
        float angle = Random.Range(sA, eA);
        float radianAngle = angle * Mathf.Deg2Rad;
        float x = circleRadius * Mathf.Cos(radianAngle);
        float y = circleRadius * Mathf.Sin(radianAngle);
        Vector3 circlePosition = new Vector3(x, y, 0f);
        return circlePosition;
    }

    private IEnumerator MoveBullet(GameObject bullet)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = GetRandomCirclePosition();
        Vector3 targetPosition = enemyPosition.GetComponent<RectTransform>().anchoredPosition3D;

        // Random rotation speed for the bullet
        float rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        while (elapsedTime < shootingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / shootingDuration);

            // Calculate angle from startPosition to targetPosition
            float angle = Vector3.SignedAngle(Vector3.right, targetPosition - startPosition, Vector3.forward);
            // Calculate normalized distance from central part of the arc (90 degrees)
            float normalizedDistanceFromCenter = Mathf.Abs(angle - 90f) / 90f;
            // Use normalized distance to scale the curve amount
            float curveAmount = Mathf.Lerp(maxCurveAmount, -maxCurveAmount, normalizedDistanceFromCenter);

            // Use quadratic curve for smooth trajectory
            float curve = curveAmount * (t - t * t);

            Vector3 direction = Vector3.Lerp(startPosition, targetPosition, t);
            if (angle < 90)
            {
                direction.x -= curve;
            }
            else if (angle > 90)
            {
                direction.x += curve;
            }

            bullet.GetComponent<RectTransform>().anchoredPosition3D = direction;

            bullet.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(bullet);
    }
}
