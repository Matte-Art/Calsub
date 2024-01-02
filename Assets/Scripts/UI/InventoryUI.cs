using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform inventoryPanel;
    public Transform equipedPanel;

    public GameObject itemPrefab;

    public Inventory inventory;

    private List<ItemUI> inventoryItemUIs;
    private List<ItemUI> equipedItemUIs;

    private void Start()
    {
        UpdateInventoryUI();
    }

    private void HandleItemClick(PlayerItem playerItem)
    {
        inventory.HandleItemClick(playerItem);
        UpdateInventoryUI();
    }

    private void DrawItem(PlayerItem playerItem, Transform parent, Vector3 position)
    {
        Sprite image = ColorManager.Instance.GetItemImageTypeByType(playerItem.Item.Type);

        var rarityColor = ColorManager.Instance.GetColorByRarity(playerItem.Rarity);

        GameObject newItem = Instantiate(itemPrefab, parent);
        
        var itemUI = newItem.GetComponent<ItemUI>();

        itemUI.SetPlayerItem(playerItem);
        itemUI.OnItemClick += HandleItemClick;

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

        if (parent == inventoryPanel)
        {
            inventoryItemUIs.Add(itemUI);
        }

        if (parent == equipedPanel)
        {
            equipedItemUIs.Add(itemUI);
        }
        
    }
    public void UpdateInventoryUI()
    {
        inventoryItemUIs = new List<ItemUI>();
        equipedItemUIs = new List<ItemUI>();
        ClearInventoryUI();

        // Draw Inventory Items
        for (int i = 0; i < inventory.rows; i++)
        {
            for (int j = 0; j < inventory.columns; j++)
            {
                PlayerItem item = inventory.GetItemByRowColumn(i, j);
                if (item != null)
                {
                    float posX = -120f + j * 60f;
                    float posY = 60f - i * 60f;

                    DrawItem(item, inventoryPanel, new Vector3(posX, posY, 0f));
                }
            }
        }

        // Draw Equiped Items
        foreach (PlayerItem item in inventory.GetEquippedItems())
        {
            var position = new Vector3();
            var itemDetails = item.Item;

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

        // Disable Equiped Items in inventory
        foreach (var item in inventoryItemUIs)
        {
            if (inventory.IsItemEquiped(item.GetPlayerItem()))
            {
                item.DisableInteraction();
            }
        }

        foreach (var item in equipedItemUIs)
        {
            item.GetComponent<Button>().interactable = true;
        }
    }

    private void ClearInventoryUI()
    {
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in equipedPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
