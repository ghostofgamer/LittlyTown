using Enums;
using ItemPositionContent;

namespace SaveAndLoad
{
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
}