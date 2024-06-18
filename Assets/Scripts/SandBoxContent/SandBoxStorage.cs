using System.Collections;
using System.Collections.Generic;
using ItemPositionContent;
using UnityEngine;

public class SandBoxStorage : MonoBehaviour
{
    private const string SandBoxSave = "SandBoxSave";

    [SerializeField] private ItemPosition[] _itemPositions;

    private Coroutine _coroutine;

    public void SaveChanges()
    {
        Debug.Log("Сохраняет ");

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SaveDataInfo());
    }

    private IEnumerator SaveDataInfo()
    {
        SandBoxSaveData sandBoxSaveData = new SandBoxSaveData();
        List<ItemData> itemDatas = new List<ItemData>();

        foreach (var itemPosition in _itemPositions)
        {
            if (itemPosition.Item != null)
            {
                ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                itemDatas.Add(itemData);
            }
        }

        sandBoxSaveData.ItemDatas = itemDatas;
        string jsonData = JsonUtility.ToJson(sandBoxSaveData);
        PlayerPrefs.SetString(SandBoxSave, jsonData);
        PlayerPrefs.Save();
        yield return null;
    }

    public void LoadDataInfo()
    {
        SandBoxSaveData sandBoxSaveData = new SandBoxSaveData();

        if (PlayerPrefs.HasKey(SandBoxSave))
        {
            string jsonData = PlayerPrefs.GetString(SandBoxSave);
            sandBoxSaveData = JsonUtility.FromJson<SandBoxSaveData>(jsonData);
        }
        else
        {
            // Debug.Log("нет сохранения");
            return;
        }
    }
}