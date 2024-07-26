using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Dictionary<ItemType, List<ShopItem>> shopItems;
    public List<Collectible> allCollectibles;
}
