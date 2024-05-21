using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemPositionContent;
using UnityEngine;

[System.Serializable]
public class SelectItemData 
{
    public Items ItemName;
    public ItemPosition ItemPosition;

    public SelectItemData(Items itemName, ItemPosition itemPosition)
    {
        ItemName = itemName;
        ItemPosition = itemPosition;
    }
}
