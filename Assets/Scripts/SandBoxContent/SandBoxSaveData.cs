
using System.Collections.Generic;

[System.Serializable]
public class SandBoxSaveData
{
    public List<ItemData> ItemDatas;
    
    public SandBoxSaveData()
    {
        ItemDatas = new List<ItemData>();
    }
}
