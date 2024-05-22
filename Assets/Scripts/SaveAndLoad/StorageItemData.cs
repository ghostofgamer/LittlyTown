using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemPositionContent;
using UnityEngine;
[System.Serializable]
public class StorageItemData 
{
    public Items ItemName;
    public ItemPosition ItemPosition;

    public StorageItemData(Items itemName, ItemPosition itemPosition)
    {
        ItemName = itemName;
        ItemPosition = itemPosition;
    }
}
