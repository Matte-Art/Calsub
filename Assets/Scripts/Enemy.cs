using UnityEngine;
using System.IO;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int MaxHP;
    public int CurrentHP;
    public int Level;
    public int EnemyImageId;

    private EnemyUI enemyUI;
    private void Awake()
    {
        enemyUI = GetComponent<EnemyUI>();
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.OnRoundEnd += HandleRoundEnd;
    }

    private void Start()
    {
        enemyUI.UpdateHealthBarFill(this);
        enemyUI.UpdateHealthBarText(this);
    }

    private void HandleRoundEnd(RoundEndEventArgs args)
    {
        var damage = args.Task.result;
        TakeDamage((int)damage);

        enemyUI.UpdateHealthBarFill(this);
        enemyUI.UpdateHealthBarText(this);

        if (CurrentHP <= 0)
        {
            IncreaseStatsOnDeath();

            StartCoroutine(UpdateUIAfterDelay(GameManager.roundDelay));
        }
    }

    private IEnumerator UpdateUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        enemyUI.UpdateHealthBarFill(this);
        enemyUI.UpdateHealthBarText(this);
    }

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }
    }

    private void IncreaseStatsOnDeath()
    {
        MaxHP += 10; // Przyk³adowa wartoœæ, mo¿esz dostosowaæ do potrzeb gry
        CurrentHP = MaxHP;
        Level++; // Zwiêkszenie poziomu o 1
        EnemyImageId++; // Zwiêkszenie identyfikatora obrazka przeciwnika o 1
    }

    public void LoadEnemyFromFile(string fileName)
    {
        TextAsset file = Resources.Load(fileName) as TextAsset;

        if (file != null)
        {
            string json = file.ToString();
            EnemyData enemyData = JsonUtility.FromJson<EnemyData>(json);

            MaxHP = enemyData.MaxHP;
            CurrentHP = enemyData.CurrentHP;
            Level = enemyData.Level;
            EnemyImageId = enemyData.EnemyImageId;
        }
        else
        {
            Debug.LogError("Enemy file not found: " + file);
        }
    }

    public void SaveToJson(string filePath)
    {
        EnemyData enemyData = new EnemyData()
        {
            MaxHP = MaxHP,
            CurrentHP = CurrentHP,
            Level = Level,
            EnemyImageId = EnemyImageId
        };

        var jsonData = JsonUtility.ToJson(enemyData);
        File.WriteAllText(filePath, jsonData);
    }
}

[System.Serializable]
public class EnemyData
{
    public int MaxHP;
    public int CurrentHP;
    public int Level;
    public int EnemyImageId;
}
