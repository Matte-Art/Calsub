using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<PlayerItem> equippedItems;
    public PlayerItem[,] playerItems;

    public int rows = 3;
    public int columns = 5;

    private ListWrapperJSON<PlayerItemJSON> playerItemList;

    private void Awake()
    {
        equippedItems = new List<PlayerItem>();
        playerItems = new PlayerItem[rows, columns];
    }

    private void Start()
    {
        FillInventoryWithPlayerItems(playerItemList);
    }

    public void HandleItemClick(PlayerItem playerItem)
    {
        ToggleItemEquip(playerItem);
    }

    private void FillInventoryWithPlayerItems(ListWrapperJSON<PlayerItemJSON> playerItems)
    {
        int index = 0;

        foreach (var playerItemJSON in playerItems.list)
        {
            int i = index / columns;
            int j = index % columns;

            var ItemId = playerItemJSON.ItemId;
            var Rarity = playerItemJSON.Rarity;
            var Effects = playerItemJSON.Effects;

            foreach (var effect in Effects)
            {
                effect.ItemEffect = DataLoader.Instance.GetEffectById(effect.EffectId);
            }

            var Item = DataLoader.Instance.GetItemById(ItemId);

            this.playerItems[i, j] = new PlayerItem(ItemId, Item, Rarity, Effects);

            index++;
        }
    }



    //private void SavePlayerItemsToFile()
    //{
    //    string json = JsonUtility.ToJson();
    //    //string filePath = Path.Combine(Application.persistentDataPath, PlayerItemsFileName);

    //    //File.WriteAllText(filePath, json);
    //}



    public bool AddItem(PlayerItem item)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (playerItems[i, j] == null)
                {
                    playerItems[i, j] = item;
                    return true;
                }
            }
        }

        return false;
    }

    public bool RemoveItem(PlayerItem item)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (playerItems[i, j] == item)
                {
                    playerItems[i, j] = null;
                    return true;
                }
            }
        }

        return false;
    }

    public void ToggleItemEquip(PlayerItem item)
    {
        if (equippedItems.Contains(item))
        {
            UnequipItem(item);
        }
        else if (!equippedItems.Contains(item))
        {
            EquipItem(item);
        }
    }

    public bool EquipItem(PlayerItem item)
    {
        if (equippedItems.Contains(item))
        {
            Debug.LogWarning("Przedmiot jest ju¿ za³o¿ony!");
            return false;
        }

        if (IsTypeEquiped(item.Item.Type))
        {
            Debug.LogWarning("Przedmiot tego typu jest ju¿ za³o¿ony!");
            return false;
        }
        
        switch (item.Item.Type)
        {
            case ItemType.Clock:
                equippedItems.Add(item);
                return true;
            case ItemType.Attack:
                equippedItems.Add(item);
                return true;
            case ItemType.Defense:
                equippedItems.Add(item);
                return true;
            case ItemType.Extra:
                equippedItems.Add(item);
                return true;
            default:
                Debug.LogWarning("Nieobs³ugiwany typ przedmiotu!");
                return false;
        }
    }

    public bool UnequipItem(PlayerItem item)
    {
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
            return true;
        }
        else
        {
            Debug.LogWarning("Przedmiot nie jest za³o¿ony!");
            return false;
        }
    }

    public PlayerItem GetItemByRowColumn(int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns)
        {
            return playerItems[row, column];
        }
        else
        {
            Debug.LogWarning("Nieprawid³owe wspó³rzêdne komórki ekwipunku!");
            return null;
        }
    }

    public bool IsItemEquiped(PlayerItem item)
    {
        return equippedItems.Contains(item);
    }

    public bool IsTypeEquiped(ItemType type)
    {
        foreach (var item in equippedItems)
        {
            if (item.Item.Type == type)
            {
                return true;
            }
        }
        return false;
    }

    public List<PlayerItem> GetEquippedItems()
    {
        return equippedItems;
    }

    public PlayerItem[,] GetPlayerItems()
    {
        return playerItems;
    }

    public void LoadPlayerItemsFromFile(string fileName)
    {
        TextAsset file = Resources.Load(fileName) as TextAsset;

        if (file != null)
        {
            string json = file.ToString();
            playerItemList = JsonUtility.FromJson<ListWrapperJSON<PlayerItemJSON>>(json);
        }
        else
        {
            Debug.LogError("Player items file not found: " + file);
        }
    }
}
