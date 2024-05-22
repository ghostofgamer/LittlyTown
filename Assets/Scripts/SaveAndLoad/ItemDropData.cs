using System.Collections;
using System.Collections.Generic;
using ItemContent;
using UnityEngine;

[System.Serializable]
public class ItemDropData 
{
    public Sprite Icon;
    public Item PrefabItem;
    
    public ItemDropData(Sprite icon, Item prefabItem)
    {
        Icon = icon;
        PrefabItem = prefabItem;
    }
}
