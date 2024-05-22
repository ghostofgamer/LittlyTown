using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShopItemData
{
    public List<ItemData> ItemsData;
    
    public ShopItemData()
    {
        ItemsData = new List<ItemData>();
    }
}
