using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory Inventory { get; private set; }
    public int Gold { get; set; }


    private void Awake()
    {
        Inventory = GetComponent<Inventory>();
        Gold = 50;
    }
}