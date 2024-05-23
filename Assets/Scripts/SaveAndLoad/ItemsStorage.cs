using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CountersContent;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using ItemSO;
using PossibilitiesContent;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ShopItems _shopItems;
    [SerializeField] private PossibilitieBulldozer _possibilitieBulldozer;
    [SerializeField] private PossibilitieReplace _possibilitieReplace;
    [SerializeField] private List<SaveData> _saveDatas = new List<SaveData>();

    private Coroutine _coroutine;
    private Coroutine _coroutineSignal;
    private Item _selectObject;
    private Item _temporaryObject;
    private ItemDropDataSO _itemDropDataSO;
    private SaveData _saveData;

    public SaveData SaveData => _saveData;

    public Item SelectObject => _selectObject;


    public Item SelectSaveItem { get; private set; }

    public event Action<SaveData> SaveCompleted;

    public event Action StepChanged;

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
        // _itemDragger.StepCompleted += SaveSteps;
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
        // _itemDragger.StepCompleted -= SaveSteps;
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

    public void SaveChanges()
    {
        Debug.Log("Сохраняет ");

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Save());
    }

    /*
    private void SaveSteps()
    {
        Debug.Log("Сохраняет ");
        if (_coroutineSignal != null)
            StopCoroutine(_coroutineSignal);

        _coroutineSignal = StartCoroutine(SignalsaveStep());
    }

    private IEnumerator SignalsaveStep()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Save());

        Debug.Log("1");
        yield return _coroutine;
        Debug.Log("3");
        StepChanged?.Invoke();
    }*/

    private IEnumerator Save()
    {
        SaveData saveData = new SaveData();
        _saveData = saveData;

        if (_itemDragger.SelectedObject != null)
        {
            /*_selectObject = _itemDragger.SelectedObject;
            saveData.SelectItemDragger = _selectObject;*/
            saveData.SelectItemData = new SelectItemData(_itemDragger.SelectedObject.ItemName,
                _itemDragger.SelectedObject.ItemPosition);

            Debug.Log("сохраняем " + saveData.SelectItemData.ItemName + "   " + saveData.SelectItemData.ItemPosition);
        }

        /*_itemDropDataSO = _dropGenerator.ItemDropData;
        saveData.ItemDropData = _itemDropDataSO;*/


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

        List<ItemData> itemDataPrice = new List<ItemData>();

        foreach (var item in _shopItems.Items)
        {
            ItemData itemData = new ItemData(item.ItemName, null, item.Price);
            itemDataPrice.Add(itemData);
        }

        // Debug.Log("ShopItemData " + itemDataPrice.Count);

        saveData.PossibilitiesItemsData = new PossibilitiesItemsData(_possibilitieBulldozer, _possibilitieReplace,
            _possibilitieBulldozer.Price, _possibilitieReplace.Price);
        // Debug.Log("SavePossibilitie " + saveData.PossibilitiesItemsData.PossibilitieBulldozer.Price);


        // saveData.ShopItemData.ItemsData = itemDataPrice;
        saveData.ItemDatasPrices = itemDataPrice;
        // Debug.Log("Цены " + saveData.ItemDatasPrices.Count);
        if (_dropGenerator.ItemDropData != null)
        {
            saveData.ItemDropData =
                new ItemDropData(_dropGenerator.ItemDropData.Icon, _dropGenerator.ItemDropData.PrefabItem);
            // Debug.Log("ItemDropData " + saveData.ItemDropData.PrefabItem.ItemName);
        }
        else
            saveData.ItemDropData = null;


        saveData.ItemDatas = itemDatas;
        saveData.BulldozerCount = _bulldozerCounter.PossibilitiesCount;
        saveData.ReplaceCount = _replaceCounter.PossibilitiesCount;
        saveData.GoldValue = _goldWallet.CurrentValue;
        // Debug.Log("Gold: " + saveData.GoldValue);
        saveData.MoveCount = _moveCounter.MoveCount;
        saveData.ScoreValue = _scoreCounter.CurrentScore;
        // saveData.StorageItem = _storage.CurrentItem;
        if (_storage.CurrentItem != null)
        {
            // Debug.Log("CurrentStorage " + _storage.CurrentItem.ItemName);
            saveData.StorageItemData =
                new StorageItemData(_storage.CurrentItem.ItemName, _storage.CurrentItem.ItemPosition);
        }
        else
        {
            saveData.StorageItemData = new StorageItemData(Items.Apartment,null);
        }

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
        // Agava.YandexGames.Utility.PlayerPrefs.SetString("save.json 1", json);
        // Agava.YandexGames.Utility.PlayerPrefs.Save();
        yield return null;
        SaveCompleted?.Invoke(saveData);
    }

    private void SaveItemDraggerItems(Item selectItem, Item temporaryItem)
    {
        /*_selectObject = selectItem;
        _temporaryObject = temporaryItem;
        SaveChanges();*/
    }

    public SaveData GetSaveData()
    {
        return _saveData;
    }

    public void Load()
    {
        // Agava.YandexGames.Utility.PlayerPrefs.HasKey()
        // string json = Agava.YandexGames.Utility.PlayerPrefs.GetString("save.json 1");
        // Agava.YandexGames.Utility.PlayerPrefs.Load(onSuccessCallback:);
        // Agava.YandexGames.Utility.PlayerPrefs.Save();
        string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/save.json");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        _saveDatas.Add(saveData);
        _saveData = saveData;
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
        // Debug.Log("Gold: " + saveData.GoldValue);
        _moveCounter.SetValue(saveData.MoveCount);
        _scoreCounter.SetValue(saveData.ScoreValue);


        Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
            saveData.SelectItemData.ItemPosition.transform.position,
            Quaternion.identity, _container);

        selectItem.Init(saveData.SelectItemData.ItemPosition);
        selectItem.gameObject.SetActive(false);
        SelectSaveItem = selectItem;
        // _itemDragger.SetItem(selectItem, saveData.SelectItemData.ItemPosition);
        // Item storageItem = Instantiate(saveData.StorageItem, _container);


        if (saveData.StorageItemData.ItemName != null)
        {
            // Debug.Log("Storage: " + saveData.StorageItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.StorageItemData.ItemName), _container);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage.SetItem(storageItem);
        }

        /*foreach (var item in saveData.ShopItemData.ItemsData)
        {
            _shopItems.SetPrice(item.ItemName,item.Price);
        }*/

        foreach (var item in saveData.ItemDatasPrices)
        {
            _shopItems.SetPrice(item.ItemName, item.Price);
        }

        // Debug.Log("СМОТРИМ ЦЕНУ " + saveData.PossibilitiesItemsData.PriceBulldozer);
        _shopItems.SetPricePossibilitie(saveData.PossibilitiesItemsData.PriceBulldozer,
            saveData.PossibilitiesItemsData.PriceReplace);

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