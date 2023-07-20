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

    public Color AdditionColorTheme;
    public Color SubtractionColorTheme;
    public Color MultiplicationColorTheme;
    public Color DivisionColorTheme;

    public Sprite clockImage;
    public Sprite attackImage;
    public Sprite defenseImage;
    public Sprite extraImage;

    public Texture additionImage;
    public Texture subtractionImage;
    public Texture multiplicationImage;
    public Texture divisionImage;

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

    public Color GetColorByMathOperationType(MathOperationType mathOperationType)
    {
        Color color;
        switch (mathOperationType)
        {
            case MathOperationType.Addition:
                color = AdditionColorTheme;
                break;
            case MathOperationType.Subtraction:
                color = SubtractionColorTheme;
                break;
            case MathOperationType.Multiplication:
                color = MultiplicationColorTheme;
                break;
            case MathOperationType.Division:
                color = DivisionColorTheme;
                break;
            default:
                color = AdditionColorTheme;
                break;                
        }

        return color;
    }
}
