using System.Collections.Generic;

namespace SaveAndLoad
{
    [System.Serializable]
    public class ShopItemData
    {
        public List<ItemData> ItemsData;
    
        public ShopItemData()
        {
            ItemsData = new List<ItemData>();
        }
    }
}
