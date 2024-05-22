using Enums;
using ItemPositionContent;

[System.Serializable]
public class ItemData
{
    public Items ItemName;
    public ItemPosition ItemPosition;
    public int Price;

    public ItemData(Items itemName, ItemPosition itemPosition = null, int price = 0)
    {
        ItemName = itemName;
        ItemPosition = itemPosition;
        Price = price;
    }
}