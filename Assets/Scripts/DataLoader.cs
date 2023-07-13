using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DataLoader : MonoBehaviour
{
    public static DataLoader Instance { get; private set; }

    public List<Item> ItemDatabase;
    public List<ItemEffect> ItemEffectDatabase;
    public DifficultyLevelList DifficultyLevelDatabase;

    private Inventory inventory;// nie wiem czy usun¹æ to

    private const string DifficultyLevelsFileName = "TaskDifficulties";
    private const string ItemEffectsFileName = "ItemEffectsDatabase";
    private const string ItemDatabaseFileName = "ItemDatabase";
    private const string PlayerItemsFileName = "PlayerItems";

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

        inventory = GameObject.Find("Player").GetComponent<Inventory>();

        LoadDifficultyLevelsFromFile(DifficultyLevelsFileName);
        LoadItemEffectsFromFile(ItemEffectsFileName);
        LoadItemsFromFile(ItemDatabaseFileName);
        inventory.LoadPlayerItemsFromFile(PlayerItemsFileName);
    }

    public void LoadDifficultyLevelsFromFile(string fileName)
    {
        TextAsset file = Resources.Load(fileName) as TextAsset;

        if (file != null)
        {
            string json = file.ToString();
            DifficultyLevelList difficultyLevelList = JsonUtility.FromJson<DifficultyLevelList>(json);

            DifficultyLevelDatabase = difficultyLevelList;
        }
        else
        {
            Debug.LogError("Difficulty levels file not found: " + file);
        }
    }

    public void LoadItemsFromFile(string fileName)
    {
        TextAsset file = Resources.Load(fileName) as TextAsset;
        if (file != null)
        {
            string json = file.ToString();

            ListWrapperJSON<Item> itemsList = JsonUtility.FromJson<ListWrapperJSON<Item>>(json);
            ItemDatabase = itemsList.list;
        }
        else
        {
            Debug.LogError("Items Database file not found: " + file);
        }
    }

    public void LoadItemEffectsFromFile(string fileName)
    {
        TextAsset file = Resources.Load(fileName) as TextAsset;
        if (file != null)
        {
            string json = file.ToString();
            ListWrapperJSON<ItemEffect> itemEffectsList = JsonUtility.FromJson<ListWrapperJSON<ItemEffect>>(json);

            ItemEffectDatabase = itemEffectsList.list;
        }
        else
        {
            Debug.LogError("Item Effect Database file not found: " + file);
        }
    }

    public Item GetItemById(int itemId)
    {
        return ItemDatabase.Find(item => item.Id == itemId);
    }

    public ItemEffect GetEffectById(int effectId)
    {
        return ItemEffectDatabase.Find(effect => effect.Id == effectId);
    }

    public ItemEffect GetEffectByName(string effectName) 
    {
        return ItemEffectDatabase.Find(effect => effect.effectName == effectName);
    }
}
