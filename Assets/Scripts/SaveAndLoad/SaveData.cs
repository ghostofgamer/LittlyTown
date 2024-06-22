using System.Collections.Generic;

namespace SaveAndLoad
{
    [System.Serializable]
    public class SaveData
    {
        public List<ItemData> ItemDatas;
        public int ReplaceCount;
        public int BulldozerCount;
        public int GoldValue;
        public float MoveCount;
        public int ScoreValue;
        public int FactorScoreValue;
        public ItemDropData ItemDropData;
        public SelectItemData SelectItemData;
        public StorageItemData StorageItemData;
        public StorageItemData Storage1ItemData;
        public StorageItemData Storage2ItemData;
        public List<ItemData> ItemDatasPrices;
        public PossibilitiesItemsData PossibilitiesItemsData;
        public ItemData TemporaryItem;
    
        public SaveData()
        {
            ItemDatas = new List<ItemData>();
            ItemDatasPrices = new List<ItemData>();
        }
    }
}
