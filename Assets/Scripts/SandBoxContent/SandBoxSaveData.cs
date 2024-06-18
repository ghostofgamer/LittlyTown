
using System.Collections.Generic;

[System.Serializable]
public class SandBoxSaveData
{
    public List<ItemData> ItemDatas;
    public List<ItemPositionData> ItemPositionDatas;
    
    public SandBoxSaveData()
    {
        ItemDatas = new List<ItemData>();
        ItemPositionDatas = new List<ItemPositionData>();
    }
}
