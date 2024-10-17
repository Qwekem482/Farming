using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : SingletonMonoBehavior<LevelSystem>, IGameSystem
{
    int currentExp;
    public int currentLevel;
    int expNeeded;
    
    [SerializeField] Slider expSlider;
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
        EventManager.Instance.AddListener<ExpAddedEvent>(OnExpAdded);
        EventManager.Instance.AddListener<LevelUpEvent>(OnLevelUp);
    }

    public void StartingSystem()
    {

    }

    public void Load(int exp, int level)
    {
        currentExp = exp;
        currentLevel = level;
        expNeeded = ResourceManager.Instance.levelData[currentLevel].expNeeded;
        
        UpdateUI();
    }

    void UpdateUI()
    {
        expSlider.maxValue = expNeeded;
        expSlider.value = currentExp;
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
        expNeeded = ResourceManager.Instance.levelData[eventInfo.nextLv].expNeeded;
        
        //Init a level up windows
        Debug.Log("Level Up");

        CurrencyChangeEvent addSilver = new CurrencyChangeEvent(
            ResourceManager.Instance.levelData[eventInfo.nextLv].silver, CurrencyType.Silver);
        EventManager.Instance.QueueEvent(addSilver);
        
        CurrencyChangeEvent addGold = new CurrencyChangeEvent(
            ResourceManager.Instance.levelData[eventInfo.nextLv].gold, CurrencyType.Gold);
        EventManager.Instance.QueueEvent(addGold);
        
        UpdateUI();
    }
}
