using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : SingletonMonoBehavior<LevelSystem>
{
    int currentExp;
    int currentLevel;
    int expNeeded;
    LevelList levelList;

    [SerializeField] TextAsset levelAsset;
    [SerializeField] Slider expSlider;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] TextMeshProUGUI levelText;
    
    /*"levels":
     * {
            "levelCount": 0,
            "expNeeded": 10,
            "itemsUnlocked": [0, 1, 2],
            "rewards": {
                "gold": 0,
                "silver": 0
            }
        }
     */
    

    protected override void Awake()
    {
        base.Awake();
        levelList ??= GetLevels();
        
        currentExp = 0; //read from player's save
        currentLevel = 0;  //read from player's save
        if (levelList != null) expNeeded = levelList.levels[currentLevel].expNeeded;
    }

    void Start()
    {
        EventManager.Instance.AddListener<ExpAddedEvent>(OnExpAdded);
        EventManager.Instance.AddListener<LevelUpEvent>(OnLevelUp);
        UpdateUI();
    }

    LevelList GetLevels()
    {
        return JsonUtility.FromJson<LevelList>(levelAsset.text);
    }

    void UpdateUI()
    {
        expSlider.maxValue = expNeeded;
        expSlider.value = currentExp;
        expText.text = currentExp + "/" + expNeeded;
        levelText.text = currentLevel.ToString();
    }

    void OnExpAdded(ExpAddedEvent eventInfo)
    {
        currentExp += eventInfo.amount;

        if (currentExp >= expNeeded) {
            currentLevel++;
            LevelUpEvent levelUpEvent = new LevelUpEvent(currentLevel);
            EventManager.Instance.QueueEvent(levelUpEvent);
        }
        
        UpdateUI();
    }

    void OnLevelUp(LevelUpEvent eventInfo)
    {
        currentExp -= expNeeded;
        expNeeded = levelList.levels[eventInfo.nextLv].expNeeded;
        
        //Init a level up windows
        Debug.Log("Level Up");

        CurrencyChangeEvent addSilver = new CurrencyChangeEvent(
            levelList.levels[eventInfo.nextLv].rewards.silver, CurrencyType.Silver);
        EventManager.Instance.QueueEvent(addSilver);
        
        CurrencyChangeEvent addGold = new CurrencyChangeEvent(
            levelList.levels[eventInfo.nextLv].rewards.gold, CurrencyType.Gold);
        EventManager.Instance.QueueEvent(addGold);
        
        UpdateUI();
    }
}

[Serializable]
public class Level
{
    public int levelCount;
    public int expNeeded;
    public int[] itemsUnlocked;
    public Reward rewards;
}

[Serializable]
public class Reward
{
    public int gold;
    public int silver;
}

[Serializable]
public class LevelList
{
    public Level[] levels;
}
