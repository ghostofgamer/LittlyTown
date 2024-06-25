using Enums;
using ItemPositionContent;

namespace SaveAndLoad
{
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
}