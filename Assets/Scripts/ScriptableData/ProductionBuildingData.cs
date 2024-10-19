using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductionBuildingData", menuName = "CustomObject/ProductionBuildingData")]
public class ProductionBuildingData : BuildingData
{
    public ProductionBuildingType type;
    public List<ProductionOutputData> productData;
    
    protected override void OnValidate()
    {
        area.size = new Vector3Int(area.size.x, area.size.y, 1);
        if (char.IsDigit(id[0])) id = "F" + id;
    }
}
public enum ProductionBuildingType
{
    Grinder,
    Steamer,
    Field,
    SugarCooker,
}
