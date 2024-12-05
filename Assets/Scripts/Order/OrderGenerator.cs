using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class OrderGenerator
{
    const float SILVER_MIN_WEIGHT = 1f;
    const float SILVER_MAX_WEIGHT = 1.3f;
    const float GOLD_MIN_WEIGHT = 0.001f;
    const float GOLD_MAX_WEIGHT = 0.01f;
    const float EXP_MIN_WEIGHT = 0.1f;
    const float EXP_MAX_WEIGHT = 1f;
    const float GOLDEN_ORDER_RATE = 0.05f; //5% golden order rate
    
    public Order Generate(List<Collectible> collectibles, bool setResetTime = false)
    {
        int itemQuantity = Random.Range(1, 4);
        Dictionary<Collectible, Item> requestItems = new Dictionary<Collectible, Item>(itemQuantity);
        int totalCurrencyReward = 0;
        int totalExpReward = 0;
        bool isGoldenOrder = Random.Range(0f, 1f) < GOLDEN_ORDER_RATE;
        
        for (int i = 0; i < itemQuantity; i++)
        {
            Collectible collectible = GetRandomCollectible(collectibles);
            if (requestItems.ContainsKey(collectible)) continue;
            int amount = GetRandomAmount();
            float outputValue = CalcOutputValue(collectible, amount);

            totalExpReward += GenerateRewardValue(outputValue, EXP_MIN_WEIGHT, EXP_MAX_WEIGHT);
            if (isGoldenOrder) totalCurrencyReward += GenerateRewardValue(outputValue, GOLD_MIN_WEIGHT, GOLD_MAX_WEIGHT);
            totalCurrencyReward += GenerateRewardValue(outputValue, SILVER_MIN_WEIGHT, SILVER_MAX_WEIGHT);
            requestItems.Add(collectible, new Item(collectible, amount));
        }

        return setResetTime ?
            new Order(requestItems.Values.ToArray(),
                totalExpReward,
                totalCurrencyReward,
                isGoldenOrder, 
                DateTime.Now + TimeSpan.FromMinutes(15)) :
            new Order(requestItems.Values.ToArray(),
                totalExpReward,
                totalCurrencyReward,
                isGoldenOrder);
    }

    Collectible GetRandomCollectible(List<Collectible> collectibles)
    {
        int index = Random.Range(0, collectibles.Count - 1);
        return collectibles[index];
    }

    int GetRandomAmount()
    {
        return Random.Range(1, LevelSystem.Instance.currentLevel + 10);
    }

    int GenerateRewardValue(float outputValue, float minWeight, float maxWeight)
    {
        int returnValue = (int)(Random.Range(minWeight, maxWeight) * outputValue);
        return returnValue <= 0 ? 1 : returnValue;
    }

    float CalcOutputValue(Collectible collectible, int amount)
    {
        return (from productionOutputData
                    in ResourceManager.Instance.productData.Values
                where collectible.id == productionOutputData.product.id
                select productionOutputData.outputValue).FirstOrDefault() 
               * amount;
    }
    
    
}
