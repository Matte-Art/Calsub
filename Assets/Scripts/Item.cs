using UnityEngine.UI;

public enum Rarity
{
    None,
    Standard,
    Rare,
    Legendary
}

public enum ItemType
{
    Clock,
    Attack,
    Defense,
    Extra
}

[System.Serializable]
public class Item
{
    public int Id;
    public string Name;
    public string Description;
    public ItemType Type;
    public ItemEffect Effect;
    public Image Image;
}