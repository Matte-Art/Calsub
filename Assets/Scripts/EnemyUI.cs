using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image enemyImage;
    public TextMeshProUGUI healthBarText;
    public Image healthBarImage;

    public List<Sprite> enemyImages;

    private void Awake()
    {

    }

    public void SetImageById(int id)
    {
        enemyImage.sprite = enemyImages[id];
    }

    public void UpdateHealthBarText(Enemy enemy)
    {
        string text = $"{enemy.CurrentHP} / {enemy.MaxHP} ";
        healthBarText.text = text;
    }

    public void UpdateHealthBarFill(Enemy enemy)
    {
        float fillAmount = (float)enemy.CurrentHP / (float)enemy.MaxHP;
        healthBarImage.fillAmount = fillAmount;
    }
}
