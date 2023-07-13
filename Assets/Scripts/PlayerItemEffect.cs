[System.Serializable]
public class PlayerItemEffect
{
    public int EffectId;
    public ItemEffect ItemEffect;
    public float EffectValue;

    public PlayerItemEffect(int effectId, ItemEffect itemEffect, float effectValue)
    {
        EffectId = effectId;
        ItemEffect = itemEffect;
        EffectValue = effectValue;
    }
}