using UnityEngine;

public class ItemEffectGenerator
{
    public static PlayerItemEffect GeneratePlayerItemEffect(ItemEffect itemEffect, Rarity rarity = Rarity.None)
    {
        float intervalMin = 0f;
        float intervalMax = 0f;

        switch (rarity)
        {
            case Rarity.Standard:
                intervalMin = 0.01f;
                intervalMax = 0.2f;
                break;
            case Rarity.Rare:
                intervalMin = 0.3f;
                intervalMax = 0.8f;
                break;
            case Rarity.Legendary:
                intervalMin = 0.8f;
                intervalMax = 1f;
                break;
        };

        var min = itemEffect.minValue * intervalMin;
        var max = itemEffect.maxValue * intervalMax;

        if (rarity == Rarity.Legendary)
        {
            // Zwiêkszenie szansy na bardzo du¿e bonusy
            float chanceForHighValue = 0.05f; // 5% szansa na bardzo du¿y bonus
            float highValueMultiplier = Mathf.Pow(10f, Random.Range(1f, 3f)); // Losowy mno¿nik dla bardzo du¿ego bonusu

            if (Random.value < chanceForHighValue)
            {
                max *= highValueMultiplier;
            }
        }

        var effectValue = Random.Range(min, max);
        var effect = new PlayerItemEffect(itemEffect.Id, itemEffect, effectValue);

        return effect;
    }
}
public static class ClockEffects
{
    public static ItemEffect RoundTime = DataLoader.Instance.GetEffectByName("Round Time");
    public static ItemEffect ExtraTime = DataLoader.Instance.GetEffectByName("Extra Time");
    public static ItemEffect AnswerSpeed = DataLoader.Instance.GetEffectByName("Answer Speed");
    public static ItemEffect TimeStop = DataLoader.Instance.GetEffectByName("Time Stop");

    public static PlayerItemEffect GetRandomRoundTimeEffect(Rarity rarity)
    {
        var itemEffect = ItemEffectGenerator.GeneratePlayerItemEffect(RoundTime, rarity);
        return itemEffect;
    }

    public static PlayerItemEffect GetRandomExtraTime(Rarity rarity)
    {
        var itemEffect = ItemEffectGenerator.GeneratePlayerItemEffect(ExtraTime, rarity);
        return itemEffect;
    }

    public static PlayerItemEffect GetRandomAnswerSpeed(Rarity rarity)
    {
        var itemEffect = ItemEffectGenerator.GeneratePlayerItemEffect(AnswerSpeed, rarity);
        return itemEffect;
    }

    public static PlayerItemEffect GetRandomTimeStop(Rarity rarity)
    {
        var itemEffect = ItemEffectGenerator.GeneratePlayerItemEffect(TimeStop, rarity);
        return itemEffect;
    }
}