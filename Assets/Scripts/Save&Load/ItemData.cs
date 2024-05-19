using Enums;
using ItemPositionContent;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public Items ItemName;
    public ItemPosition ItemPosition;

    public ItemData(Items itemName, ItemPosition itemPosition)
    {
        ItemName = itemName;
        ItemPosition = itemPosition;
    }
}
