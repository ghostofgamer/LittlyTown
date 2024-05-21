using System;
using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using ItemSO;
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
    [SerializeField] private MoveCounter _moveCounter;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Storage _storage;
    [SerializeField]private DropGenerator _dropGenerator;

    private Coroutine _coroutine;
    private Item _selectObject;
    private Item _temporaryObject;
    private ItemDropDataSO _itemDropDataSO;
    private SaveData _saveData;

    public SaveData SaveData => _saveData;
    
    public Item SelectObject => _selectObject;
    
    public event Action<SaveData> SaveCompleted;

    /*private void OnEnable()
    {
        _itemDragger.PlaceChanged += SaveChanges;
        // _itemDragger.BuildItem += SaveChanges;
        _itemDragger.SelectItemReceived += SaveItemDraggerItems;
        _mapGenerator.GenerationCompleted += SaveChanges;
        _replacementPosition.PositionsChanged += SaveChanges;
        _removalItems.Removed += SaveChanges;
        _storage.StoragePlaceChanged += SaveChanges;
    }

    private void OnDisable()
    {
        _itemDragger.PlaceChanged -= SaveChanges;
        // _itemDragger.BuildItem -= SaveChanges;
        _itemDragger.SelectItemReceived -= SaveItemDraggerItems;
        _mapGenerator.GenerationCompleted -= SaveChanges;
        _replacementPosition.PositionsChanged -= SaveChanges;
        _removalItems.Removed -= SaveChanges;
        _storage.StoragePlaceChanged -= SaveChanges;
    }*/
    
    
    
    private void OnEnable()
    {
        _itemDragger.SelectNewItem += SaveChanges;
        
        /*_itemDragger.PlaceChanged += SaveChanges;
        // _itemDragger.BuildItem += SaveChanges;
        _itemDragger.SelectItemReceived += SaveItemDraggerItems;
        _mapGenerator.GenerationCompleted += SaveChanges;
        _replacementPosition.PositionsChanged += SaveChanges;
        _removalItems.Removed += SaveChanges;
        _storage.StoragePlaceChanged += SaveChanges;*/
    }

    private void OnDisable()
    {
        _itemDragger.SelectNewItem -= SaveChanges;
        /*_itemDragger.PlaceChanged -= SaveChanges;
        // _itemDragger.BuildItem -= SaveChanges;
        _itemDragger.SelectItemReceived -= SaveItemDraggerItems;
        _mapGenerator.GenerationCompleted -= SaveChanges;
        _replacementPosition.PositionsChanged -= SaveChanges;
        _removalItems.Removed -= SaveChanges;
        _storage.StoragePlaceChanged -= SaveChanges;*/
    }

    /*public void SaveChanges()
    {
        Debug.Log("Сохраняет ");
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Save());
    }*/
    
    public void CancelMoveSave()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SaveCancelMove());
    }

    public void SaveChanges()
    {
        Debug.Log("Сохраняет ");
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Save());
    }
    
    private IEnumerator Save()
    {
        SaveData saveData = new SaveData();
        _saveData = saveData;
        
        if (_itemDragger.SelectedObject != null)
        {
            _selectObject = _itemDragger.SelectedObject;
            saveData.SelectItemDragger = _selectObject;
            saveData.SelectItemData = new SelectItemData(_selectObject.ItemName, _selectObject.ItemPosition);
        }

        _itemDropDataSO = _dropGenerator.ItemDropData;
        saveData.ItemDropData = _itemDropDataSO;

        yield return new WaitForSeconds(0.165f);

        List<ItemData> itemDatas = new List<ItemData>();

        foreach (var itemPosition in _itemPositions)
        {
            if (itemPosition.Item != null)
            {
                ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                itemDatas.Add(itemData);
            }
        }

        saveData.ItemDatas = itemDatas;
        saveData.BulldozerCount = _bulldozerCounter.PossibilitiesCount;
        saveData.ReplaceCount = _replaceCounter.PossibilitiesCount;
        saveData.GoldValue = _goldWallet.CurrentValue;
        saveData.MoveCount = _moveCounter.MoveCount;
        saveData.ScoreValue = _scoreCounter.CurrentScore;
        saveData.StorageItem = _storage.CurrentItem;

        // saveData.SelectItemDragger = _itemDragger.SelectedObject;
        // saveData.SelectPosition = _itemDragger.SelectedObject.ItemPosition;
        // saveData.TemporaryItemDragger = _itemDragger.TemporaryItem;
        // Debug.Log("Select Item Save " + saveData.SelectItemDragger);
        /*if (_selectObject != null)
        {
            saveData.SelectItemDragger = _itemDragger.SelectedObject;
            saveData.SelectPosition = _itemDragger.SelectedObject.ItemPosition;

            Debug.Log("Select Item Save " + saveData.SelectItemDragger);
        }

        if (_temporaryObject != null)
        {
            saveData.TemporaryItemDragger = _itemDragger.TemporaryItem;

            Debug.Log("Temporaru Item Save " + saveData.TemporaryItemDragger);
        }*/

        
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        yield return null;
        SaveCompleted?.Invoke(saveData);
    }
    
    
    /*private IEnumerator Save()
    {
        SaveData saveData = new SaveData();
        
        if (_itemDragger.SelectedObject != null)
        {
            // _selectObject = new Item();
            _selectObject = _itemDragger.SelectedObject;
            saveData.SelectItemDragger = _selectObject;
            saveData.SelectItemData = new SelectItemData(_selectObject.ItemName, _selectObject.ItemPosition);
Debug.Log("сохраняем SelectItemData " + saveData.SelectItemData.ItemName);
            // Debug.Log("Select Item Save " + saveData.SelectItemDragger);
            // Debug.Log();
        }

        _itemDropDataSO = _dropGenerator.ItemDropData;
        saveData.ItemDropData = _itemDropDataSO;
        // Debug.Log("DROP " + saveData.ItemDropData);
        
        yield return new WaitForSeconds(0.165f);

        List<ItemData> itemDatas = new List<ItemData>();

        foreach (var itemPosition in _itemPositions)
        {
            if (itemPosition.Item != null)
            {
                ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                itemDatas.Add(itemData);
            }
        }

        saveData.ItemDatas = itemDatas;
        saveData.BulldozerCount = _bulldozerCounter.PossibilitiesCount;
        saveData.ReplaceCount = _replaceCounter.PossibilitiesCount;
        saveData.GoldValue = _goldWallet.CurrentValue;
        saveData.MoveCount = _moveCounter.MoveCount;
        saveData.ScoreValue = _scoreCounter.CurrentScore;
        saveData.StorageItem = _storage.CurrentItem;

        // saveData.SelectItemDragger = _itemDragger.SelectedObject;
        // saveData.SelectPosition = _itemDragger.SelectedObject.ItemPosition;
        // saveData.TemporaryItemDragger = _itemDragger.TemporaryItem;
        // Debug.Log("Select Item Save " + saveData.SelectItemDragger);
        /*if (_selectObject != null)
        {
            saveData.SelectItemDragger = _itemDragger.SelectedObject;
            saveData.SelectPosition = _itemDragger.SelectedObject.ItemPosition;

            Debug.Log("Select Item Save " + saveData.SelectItemDragger);
        }

        if (_temporaryObject != null)
        {
            saveData.TemporaryItemDragger = _itemDragger.TemporaryItem;

            Debug.Log("Temporaru Item Save " + saveData.TemporaryItemDragger);
        }#1#

        
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        yield return null;
        SaveCompleted?.Invoke(saveData);
    }*/
    
    
    private IEnumerator SaveCancelMove()
    {
        SaveData saveData = new SaveData();
        
        if (_itemDragger.SelectedObject != null)
        {
            // _selectObject = new Item();
            _selectObject = _itemDragger.SelectedObject;
            saveData.SelectItemDragger = _selectObject;

            // Debug.Log("Select Item Save " + saveData.SelectItemDragger);
            // Debug.Log();
        }

        _itemDropDataSO = _dropGenerator.ItemDropData;
        saveData.ItemDropData = _itemDropDataSO;
        Debug.Log("DROP " + saveData.ItemDropData);
        
        yield return new WaitForSeconds(0.165f);

        List<ItemData> itemDatas = new List<ItemData>();

        foreach (var itemPosition in _itemPositions)
        {
            if (itemPosition.Item != null)
            {
                ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                itemDatas.Add(itemData);
            }
        }

        saveData.ItemDatas = itemDatas;
        saveData.BulldozerCount = _bulldozerCounter.PossibilitiesCount;
        saveData.ReplaceCount = _replaceCounter.PossibilitiesCount;
        saveData.GoldValue = _goldWallet.CurrentValue;
        saveData.MoveCount = _moveCounter.MoveCount;
        saveData.ScoreValue = _scoreCounter.CurrentScore;
        saveData.StorageItem = _storage.CurrentItem;

        // saveData.SelectItemDragger = _itemDragger.SelectedObject;
        // saveData.SelectPosition = _itemDragger.SelectedObject.ItemPosition;
        // saveData.TemporaryItemDragger = _itemDragger.TemporaryItem;
        // Debug.Log("Select Item Save " + saveData.SelectItemDragger);
        /*if (_selectObject != null)
        {
            saveData.SelectItemDragger = _itemDragger.SelectedObject;
            saveData.SelectPosition = _itemDragger.SelectedObject.ItemPosition;

            Debug.Log("Select Item Save " + saveData.SelectItemDragger);
        }

        if (_temporaryObject != null)
        {
            saveData.TemporaryItemDragger = _itemDragger.TemporaryItem;

            Debug.Log("Temporaru Item Save " + saveData.TemporaryItemDragger);
        }*/


        // Debug.Log("StorageItem " + saveData.StorageItem);
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        yield return null;
        // Debug.Log("СОХРАНИЛ " + itemDatas.Count);
    }

    private void SaveItemDraggerItems(Item selectItem,Item temporaryItem)
    {
        /*_selectObject = selectItem;
        _temporaryObject = temporaryItem;
        SaveChanges();*/
    }

    public void Load()
    {
        string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/save.json");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        // Debug.Log("Loading    " + saveData.ItemDatas.Count);

        foreach (var itemData in saveData.ItemDatas)
        {
            if (itemData != null)
            {
                // Debug.Log("Загрузка " + itemData.ItemName);
                Item item = Instantiate(GetItem(itemData.ItemName), itemData.ItemPosition.transform.position,
                    Quaternion.identity, _container);
                item.Init(itemData.ItemPosition);
                item.Activation();
            }
        }

        _replaceCounter.SetValue(saveData.ReplaceCount);
        _bulldozerCounter.SetValue(saveData.BulldozerCount);
        _goldWallet.SetValue(saveData.GoldValue);
        _moveCounter.SetValue(saveData.MoveCount);
        _scoreCounter.SetValue(saveData.ScoreValue);
        
        
        if (saveData.SelectItemData == null)
        {
             Debug.Log("SelectNull");
        }
        else
        {
            Debug.Log("SelectНЕNull" + saveData.SelectItemData.ItemName);
        }
       
        Debug.Log("SelectNAME" + saveData.SelectItemData.ItemName);
        Debug.Log("Selectposition" + saveData.SelectItemData.ItemPosition);
        
        
        
        
        Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
            saveData.SelectItemData.ItemPosition.transform.position,
            Quaternion.identity, _container);
        
        selectItem.gameObject.SetActive(false);
        _itemDragger.SetItem(selectItem, saveData.SelectItemData.ItemPosition);
        /*Item storageItem = Instantiate(saveData.StorageItem, _container);
        storageItem.gameObject.SetActive(false);
        _storage.SetItem(storageItem);*/
        /*Item selectItem = Instantiate(GetItem(saveData.SelectItemDragger.ItemName),
            saveData.SelectItemDragger.ItemPosition.transform.position,
            Quaternion.identity, _container);
        _itemDragger.SetItem(selectItem, selectItem.ItemPosition);

        Item temporaryItem = Instantiate(GetItem(saveData.TemporaryItemDragger.ItemName),
            saveData.TemporaryItemDragger.ItemPosition.transform.position,
            Quaternion.identity, _container);*/
        
        // _itemDragger.SetTemporaryObject(temporaryItem);
        _roadGenerator.OnGeneration();
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