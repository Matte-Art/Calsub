using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform inventoryPanel;
    public Transform equipedPanel;

    public GameObject itemPrefab;

    public Inventory inventory;

    private void Start()
    {
        UpdateInventoryUI();
    }
    private void DrawItem(PlayerItem playerItem, Transform parent, Vector3 position)
    {
        var itemDetails = DataLoader.Instance.GetItemById(playerItem.ItemId);
        Sprite image = ColorManager.Instance.GetItemImageTypeByType(itemDetails.Type);

        var rarityColor = ColorManager.Instance.GetColorByRarity(playerItem.Rarity);

        GameObject newItem = Instantiate(itemPrefab, parent);

        newItem.GetComponent<ItemDetailsUI>().SetPlayerItem(playerItem);
        newItem.transform.localPosition = position;

        var rarityFrame = newItem.transform.Find("RarityFrame");
        var icon = newItem.transform.Find("Icon");
        var iconReflection = newItem.transform;

        if (icon)
        {
            icon.GetComponent<Image>().sprite = image;
        }

        if (rarityFrame)
        {
            rarityFrame.GetComponent<Image>().color = rarityColor;
        }

        if (iconReflection)
        {
            iconReflection.GetComponent<Image>().color = rarityColor;
        }
    }
    public void UpdateInventoryUI()
    {
        ClearInventoryUI();

        for (int i = 0; i < inventory.rows; i++)
        {
            for (int j = 0; j < inventory.columns; j++)
            {
                PlayerItem item = inventory.GetItem(i, j);
                if (item != null)
                {
                    float posX = -120f + j * 60f;
                    float posY = 60f - i * 60f;

                    DrawItem(item, inventoryPanel, new Vector3(posX, posY, 0f));
                }
            }
        }

        foreach (PlayerItem item in inventory.GetEquippedItems())
        {
            var position = new Vector3();
            var itemDetails = DataLoader.Instance.GetItemById(item.ItemId);

            switch (itemDetails.Type)
            {
                case ItemType.Clock:
                    position = new(-110, -20, 0);
                    break;
                case ItemType.Attack:
                    position = new(-36.6666f, -20, 0);
                    break;
                case ItemType.Defense:
                    position = new(36.6666f, -20, 0);
                    break;
                case ItemType.Extra:
                    position = new(110, -20, 0);
                    break;
                default:
                    break;
            }

            DrawItem(item, equipedPanel, position);
        }
    }

    private void ClearInventoryUI()
    {
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
