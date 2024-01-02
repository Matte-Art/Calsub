using UnityEngine;

public class CubeStack : MonoBehaviour
{
    public GameObject cubePrefab;
    public int rows;
    public int columns;
    public int layers;
    public float relativeOffset;

    void Start()
    {
        CreateCubeFormation();
    }

    void CreateCubeFormation()
    {
        float offset = cubePrefab.transform.localScale.x * relativeOffset;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                for (int layer = 0; layer < layers; layer++)
                {
                    Vector3 position = new Vector3(col * offset, row * offset, layer * offset);
                    position = position + transform.position;
                    GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
                }
            }
        }
        Quaternion rotationY = Quaternion.Euler(60, 0, 0);
        Quaternion rotationXZ = Quaternion.Euler(0, 0, 30);

        Quaternion finalRotation = rotationY * rotationXZ;

        transform.rotation = finalRotation;

        Vector3 sumVector = Vector3.zero;

        foreach (Transform child in transform)
        {
            sumVector += child.position;
        }

        Vector3 groupCenter = sumVector / transform.childCount;
        transform.position = -groupCenter;


    }
}
