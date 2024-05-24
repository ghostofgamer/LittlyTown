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

    // public ItemDropDataSO ItemDropData;

    public ItemDropData ItemDropData;
    public SelectItemData SelectItemData;
    public StorageItemData StorageItemData;
    public ShopItemData ShopItemData;
    public List<ItemData> ItemDatasPrices;
    public PossibilitiesItemsData PossibilitiesItemsData;
    public ItemData TemporaryItem;
    
    public SaveData()
    {
        ItemDatas = new List<ItemData>();
        ItemDatasPrices = new List<ItemData>();
    }
}
