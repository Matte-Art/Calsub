using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerItem
{
    public int ItemId;
    public Item Item;
    public Rarity Rarity;
    public List<PlayerItemEffect> Effects;

    public PlayerItem(int itemId, Item item, Rarity rarity, List<PlayerItemEffect> effects)
    {
        ItemId = itemId;
        Item = item;
        Rarity = rarity;
        Effects = effects;
    }
}

