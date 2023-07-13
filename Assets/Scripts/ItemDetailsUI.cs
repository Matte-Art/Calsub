using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Samsung.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDetailsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Image iconDetails;
    public Image iconItemType;
    public Image rarityTriangle;
    public Image background;
    public Image dotsBackground;
    public List<Image> bonusBackgrounds;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI itemTypeText;
    public TextMeshProUGUI rarityText;
    public List<TextMeshProUGUI> bonusesText;

    private GameObject detailsWindow;

    private PlayerItem playerItem;
    private void Awake()
    {
        detailsWindow = gameObject.transform.Find("Item_Details").gameObject;
        detailsWindow.SetActive(false);
        
    }
    public void SetPlayerItem(PlayerItem item)
    {
        this.playerItem = item;
    }
    public void UpdateItemDetailsWindow()
    {
        detailsWindow.transform.position = new Vector3(0, detailsWindow.transform.position.y, detailsWindow.transform.position.z);

        var itemDetails = DataLoader.Instance.GetItemById(playerItem.ItemId);

        var rarityColor = ColorManager.Instance.GetColorByRarity(playerItem.Rarity);
        var itemTypeIcon = ColorManager.Instance.GetItemImageTypeByType(itemDetails.Type);

        iconItemType.sprite = itemTypeIcon;
        iconDetails.sprite = icon.sprite;

        background.color = rarityColor;
        dotsBackground.color = rarityColor;
        rarityTriangle.color = rarityColor;
        nameText.color = rarityColor;
        iconDetails.color = rarityColor;

        int effectsCount = playerItem.Effects.Count;

        // Wy��cz wszystkie elementy bonusBackgrounds
        for (int i = 0; i < bonusBackgrounds.Count; i++)
        {
            bonusBackgrounds[i].gameObject.SetActive(false);
            bonusesText[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < effectsCount; i++)
        {
            float t = (i + 1) / (float)effectsCount;
            bonusBackgrounds[i].color = Color.Lerp(Color.white, rarityColor, t);
            bonusBackgrounds[i].gameObject.SetActive(true);
            bonusesText[i].gameObject.SetActive(true);
        }

        nameText.text = itemDetails.Name;
        descriptionText.text = itemDetails.Description;
        itemTypeText.text = itemDetails.Type.ToString();
        rarityText.text = playerItem.Rarity.ToString();

        if (effectsCount > 0)
        {
            for (int i = 0; i < effectsCount; i++)
            {
                var effect = playerItem.Effects[i];
                var effectName = effect.ItemEffect.effectName;
                var effectDescription = effect.ItemEffect.description;
                var effectValue = effect.EffectValue;

                bonusesText[i].text = $"Bonus {i + 1}: {effectName}: {effectValue} {effectDescription}";
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateItemDetailsWindow();
        detailsWindow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailsWindow.SetActive(false);
    }
}
