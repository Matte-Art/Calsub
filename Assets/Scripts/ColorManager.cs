using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color NoneColor;
    public Color StandardColor;
    public Color RareColor;
    public Color LegendaryColor;

    public Sprite clockImage;
    public Sprite attackImage;
    public Sprite defenseImage;
    public Sprite extraImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetItemImageTypeByType(ItemType itemType)
    {
        return itemType switch 
        { 
            ItemType.Clock => clockImage, 
            ItemType.Attack => attackImage, 
            ItemType.Defense => defenseImage, 
            ItemType.Extra => extraImage, 
            _ => null
        };

    }

    public Color GetColorByRarity(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.None => NoneColor,
            Rarity.Standard => StandardColor,
            Rarity.Rare => RareColor,
            Rarity.Legendary => LegendaryColor,
            _ => NoneColor,
        };
    }
}
