using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using ItemSO;

[System.Serializable]
public class SaveData
{
    public List<ItemData> ItemDatas;
    public int ReplaceCount;
    public int BulldozerCount;
    public int GoldValue;
    public float MoveCount;
    public int ScoreValue;
    public Item StorageItem;
    public Item SelectItemDragger;
    public Item TemporaryItemDragger;
    public ItemPosition SelectPosition;
    public ItemDropDataSO ItemDropData;
    public SelectItemData SelectItemData;
    public StorageItemData StorageItemData;
    
    public SaveData()
    {
        ItemDatas = new List<ItemData>();
    }
}
