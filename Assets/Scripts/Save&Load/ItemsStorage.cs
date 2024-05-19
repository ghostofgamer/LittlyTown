using System;
using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Wallets;

public class ItemsStorage : MonoBehaviour
{
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Item[] _items;
    [SerializeField] private Transform _container;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private PossibilitiesCounter _replaceCounter;
    [SerializeField] private PossibilitiesCounter _bulldozerCounter;
    [SerializeField] private ReplacementPosition _replacementPosition;
    [SerializeField] private RemovalItems _removalItems;
    [SerializeField] private GoldWallet _goldWallet;
    [SerializeField]private MoveCounter _moveCounter;

    private Coroutine _coroutine;

    public event Action<SaveData> SaveCompleted;

    private void OnEnable()
    {
        _itemDragger.PlaceChanged += SaveChanges;
        _mapGenerator.GenerationCompleted += SaveChanges;
        _replacementPosition.PositionsChanged += SaveChanges;
        _removalItems.Removed+= SaveChanges;
    }

    private void OnDisable()
    {
        _itemDragger.PlaceChanged -= SaveChanges;
        _mapGenerator.GenerationCompleted -= SaveChanges;
        _replacementPosition.PositionsChanged -= SaveChanges; 
        _removalItems.Removed-= SaveChanges;
    }

    public void SaveChanges()
    {
        if(_coroutine!=null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Save());
    }

    private IEnumerator Save()
    {
        yield return new WaitForSeconds(0.15f);

        List<ItemData> itemDatas = new List<ItemData>();

        foreach (var itemPosition in _itemPositions)
        {
            if (itemPosition.Item != null)
            {
                ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                itemDatas.Add(itemData);
            }
        }

        SaveData saveData = new SaveData();
        saveData.ItemDatas = itemDatas;
        saveData.BulldozerCount = _bulldozerCounter.PossibilitiesCount;
        saveData.ReplaceCount = _replaceCounter.PossibilitiesCount;
        saveData.GoldValue = _goldWallet.CurrentValue;
        saveData.MoveCount = _moveCounter.MoveCount;
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        yield return null;
        SaveCompleted?.Invoke(saveData);
        Debug.Log("СОХРАНИЛ " + itemDatas.Count);
    }

    public void Load()
    {
        string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/save.json");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        
        Debug.Log("Loading    "+saveData.ItemDatas.Count);

        foreach (var itemData in saveData.ItemDatas)
        {
            if (itemData != null)
            {
                Debug.Log("Загрузка " + itemData.ItemName);
                Item item = Instantiate(GetItem(itemData.ItemName), itemData.ItemPosition.transform.position,
                    Quaternion.identity, _container);
                item.Init(itemData.ItemPosition);
            }
        }
    }

    private Item GetItem(Items itemName)
    {
        foreach (var item in _items)
        {
            if (item.ItemName == itemName)
                return item;
        }

        return null;
    }
}