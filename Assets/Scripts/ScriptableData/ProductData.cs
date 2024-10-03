using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Product", menuName = "CustomObject/Product")]
public class ProductData : ProductionOutputData
{
    public Item[] materials;
    
#if UNITY_EDITOR
    
    protected override void OnValidate()
    {
        FixInput();
        FixID();
        outputValue = CalcOutputValue();
    }

    protected override void FixID()
    {
        if (char.IsDigit(productID[0])) productID = "P" + productID;
    }

    void FixInput()
    {
        if (materials.Length <= 4) return;
        Array.Resize(ref materials, 4);
        Debug.LogError("Material array is limited to 4 elements");
    }

    int CalcOutputValue()
    {
        int output = 0;
        string[] guids = AssetDatabase.FindAssets
            ("t:ProductionOutputData", new[] { "Assets/Data/Products" });
        
        foreach(string guid in guids)
        {
            ProductionOutputData data = AssetDatabase.LoadAssetAtPath<ProductionOutputData>
                (AssetDatabase.GUIDToAssetPath(guid));
            if(data == null) continue;
            foreach(Item material in materials)
            {
                if (data.product != material.collectible) continue;
                output += data.outputValue * material.amount;
                break;
            }
        }
        
        return output + (int)processingTime.ToSecond() / 60 + level;
    }
    
#endif
    
}
