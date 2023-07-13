using System.Collections.Generic;

[System.Serializable]
public class PlayerItemJSON
{
    public int ItemId;
    public Rarity Rarity;
    public List<PlayerItemEffect> Effects;
}