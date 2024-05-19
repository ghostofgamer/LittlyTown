using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<ItemData> ItemDatas;
    public int ReplaceCount;
    public int BulldozerCount;
    public int GoldValue;
    public float MoveCount;
    
    public SaveData()
    {
        ItemDatas = new List<ItemData>();
    }
}
