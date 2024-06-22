using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CountersContent;
using Dragger;
using Enums;
using InitializationContent;
using ItemContent;
using ItemPositionContent;
using ItemSO;
using Keeper;
using MapsContent;
using PossibilitiesContent;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Wallets;

public class ItemsStorage : MonoBehaviour
{
    private const string ItemStorageSave = "ItemStorageSave";

    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private ItemKeeper _itemKeeper;
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
    [SerializeField] private Storage _storage1;
    [SerializeField] private Storage _storage2;
    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ShopItems _shopItems;
    [SerializeField] private PossibilitieBulldozer _possibilitieBulldozer;
    [SerializeField] private PossibilitieReplace _possibilitieReplace;
    [SerializeField] private List<SaveData> _saveDatas = new List<SaveData>();
    [SerializeField] private Initializator _initializator;

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
        _itemKeeper.SelectNewItem += SaveChanges;
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
        _itemKeeper.SelectNewItem -= SaveChanges;
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

    /*public void SaveChanges()
    {
        Debug.Log("Сохраняет ");

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Save());
    }*/

    public void SaveChanges()
    {
        Debug.Log("схр");
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SaveDataInfo());
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

    private IEnumerator SaveDataInfo()
    {
        SaveData saveData = new SaveData();
        List<ItemData> itemDatas = new List<ItemData>();
        _saveData = saveData;

        if (_itemKeeper.SelectedObject != null)
        {
            /*_selectObject = _itemDragger.SelectedObject;
            saveData.SelectItemDragger = _selectObject;*/
            saveData.SelectItemData = new SelectItemData(_itemKeeper.SelectedObject.ItemName,
                _itemKeeper.SelectedObject.ItemPosition);
            SelectSaveItem = _itemKeeper.SelectedObject;
            // Debug.Log("сохраняем " + saveData.SelectItemData.ItemName + "   " + saveData.SelectItemData.ItemPosition);
        }

        yield return new WaitForSeconds(0.165f);

        foreach (var itemPosition in _initializator.ItemPositions)
        {
            /*if (itemPosition.Item != null && itemPosition.Item.ItemName != Items.Crane)
            {
                ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                itemDatas.Add(itemData);
            }*/
            if (itemPosition.Item != null)
            {
                ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                // Debug.Log("Сохр " + itemPosition.Item.ItemName);
                itemDatas.Add(itemData);
            }
        }

        if (_itemKeeper.TemporaryItem != null)
        {
            // Debug.Log("Временные айтэм есть " + _itemDragger.TemporaryItem);
            saveData.TemporaryItem =
                new ItemData(_itemKeeper.TemporaryItem.ItemName, null, _itemKeeper.TemporaryItem.Price);
        }
        else
        {
            // Debug.Log("Временные айтэм неть ");
            saveData.TemporaryItem = new ItemData(Items.Empty, null, 0);
        }

        List<ItemData> itemDataPrice = new List<ItemData>();

        foreach (var item in _shopItems.Items)
        {
            ItemData itemData = new ItemData(item.ItemName, null, item.Price);
            itemDataPrice.Add(itemData);
        }

        saveData.PossibilitiesItemsData = new PossibilitiesItemsData(_possibilitieBulldozer, _possibilitieReplace,
            _possibilitieBulldozer.Price, _possibilitieReplace.Price);

        saveData.ItemDatasPrices = itemDataPrice;

        if (_dropGenerator.ItemDropData != null)
        {
            saveData.ItemDropData =
                new ItemDropData(_dropGenerator.ItemDropData.Icon, _dropGenerator.ItemDropData.PrefabItem);
            // Debug.Log("ItemDropData " + saveData.ItemDropData.PrefabItem.ItemName);
        }
        else
            saveData.ItemDropData = null;

        saveData.MoveCount = _moveCounter.MoveCount;
        saveData.ItemDatas = itemDatas;
        saveData.BulldozerCount = _bulldozerCounter.PossibilitiesCount;
        saveData.ReplaceCount = _replaceCounter.PossibilitiesCount;
        saveData.GoldValue = _goldWallet.CurrentValue;
        saveData.ScoreValue = _scoreCounter.CurrentScore;
        saveData.FactorScoreValue = _scoreCounter.Factor;
        // Debug.Log("SCORE Factory Save " + saveData.FactorScoreValue);
        if (_storage.CurrentItem != null)
        {
            // Debug.Log("CurrentStorageSave " + _storage.CurrentItem.ItemName);
            saveData.StorageItemData =
                new StorageItemData(_storage.CurrentItem.ItemName, _storage.CurrentItem.ItemPosition);
        }
        else
        {
            // Debug.Log("CurrentStorageSave NUll!!! ");
            saveData.StorageItemData = new StorageItemData(Items.Empty, null);
        }

        if (_storage1.CurrentItem != null)
        {
            // Debug.Log("  1  CurrentStorageSave " + _storage1.CurrentItem.ItemName);
            saveData.Storage1ItemData =
                new StorageItemData(_storage1.CurrentItem.ItemName, _storage1.CurrentItem.ItemPosition);
        }
        else
        {
            // Debug.Log(" 1  CurrentStorageSave NUll!!! ");
            saveData.Storage1ItemData = new StorageItemData(Items.Empty, null);
        }

        if (_storage2.CurrentItem != null)
        {
            // Debug.Log(" 2  CurrentStorageSave " + _storage2.CurrentItem.ItemName);
            saveData.Storage2ItemData =
                new StorageItemData(_storage2.CurrentItem.ItemName, _storage2.CurrentItem.ItemPosition);
        }
        else
        {
            // Debug.Log(" 2  CurrentStorageSave NUll!!! ");
            saveData.Storage2ItemData = new StorageItemData(Items.Empty, null);
        }

        string jsonData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(ItemStorageSave + _initializator.Index, jsonData);
        PlayerPrefs.Save();
        yield return null;
        // Debug.Log("Сохранение Сцены Мап " + ItemStorageSave + _initializator.Index);
        // SaveCompleted?.Invoke(saveData);
    }


    public void LoadDataInfo()
    {
        SaveData saveData = new SaveData();

        if (PlayerPrefs.HasKey(ItemStorageSave + _initializator.Index))
        {
            string jsonData = PlayerPrefs.GetString(ItemStorageSave + _initializator.Index);
            saveData = JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            // Debug.Log("нет сохранения");
            return;
        }

        Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
            saveData.SelectItemData.ItemPosition.transform.position,
            Quaternion.identity, _initializator.CurrentMap.ItemsContainer);

        selectItem.Init(saveData.SelectItemData.ItemPosition);
        selectItem.gameObject.SetActive(false);
        SelectSaveItem = selectItem;

        /*if (saveData.TemporaryItem != null)
        {
            Debug.Log("TemporaryItemNotNull " + saveData.TemporaryItem.ItemName);
            Item item = Instantiate(GetItem(saveData.TemporaryItem.ItemName), _initializator.CurrentMap.ItemsContainer);
            _itemDragger.SetTemporaryObject(item);
        }*/
        if (saveData.TemporaryItem.ItemName != Items.Empty)
        {
            // Debug.Log("TemporaryItemNotNull " + saveData.TemporaryItem.ItemName);
            Item item = Instantiate(GetItem(saveData.TemporaryItem.ItemName), _initializator.CurrentMap.ItemsContainer);
            _itemKeeper.SetTemporaryObject(item);
        }
        else
        {
            _itemKeeper.SetTemporaryObject(null);
        }

        foreach (var item in saveData.ItemDatasPrices)
        {
            _shopItems.SetPrice(item.ItemName, item.Price);
        }

        _shopItems.SetPricePossibilitie(saveData.PossibilitiesItemsData.PriceBulldozer,
            saveData.PossibilitiesItemsData.PriceReplace);

        _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem, saveData.ItemDropData.Icon);
        _moveCounter.SetValue(saveData.MoveCount);
        _replaceCounter.SetValue(saveData.ReplaceCount);
        _bulldozerCounter.SetValue(saveData.BulldozerCount);
        _goldWallet.SetValue(saveData.GoldValue);
        _scoreCounter.SetValue(saveData.ScoreValue, saveData.FactorScoreValue);
        // Debug.Log("SCORE Load " + saveData.ScoreValue);
        // Debug.Log("SCORE Factory Load " + saveData.FactorScoreValue);
        
        
        if (saveData.StorageItemData.ItemPosition != null || saveData.StorageItemData.ItemName != Items.Empty)
        {
            // Debug.Log("Storage Load : " + saveData.StorageItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.StorageItemData.ItemName), _initializator.CurrentMap.ItemsContainer);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage.SetItem(storageItem);
        }
        else
        {
            // Debug.Log("NULL Storage  : " + saveData.Storage1ItemData.ItemName);
            _storage.SetItem(null);
        }
        
        if (saveData.Storage1ItemData.ItemPosition != null || saveData.Storage1ItemData.ItemName != Items.Empty)
        {
            // Debug.Log("1  Storage Load : " + saveData.Storage1ItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.Storage1ItemData.ItemName), _initializator.CurrentMap.ItemsContainer);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage1.SetItem(storageItem);
        }
        else
        {
            // Debug.Log("NULL Storage 1 : " + saveData.Storage1ItemData.ItemName);
            _storage1.SetItem(null);
        }
        
        if (saveData.Storage2ItemData.ItemPosition != null || saveData.Storage2ItemData.ItemName != Items.Empty)
        {
            // Debug.Log("Storage 2  " + saveData.Storage2ItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.Storage2ItemData.ItemName), _initializator.CurrentMap.ItemsContainer);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage2.SetItem(storageItem);
        }
        else
        {
            // Debug.Log(" NULL Storage 2 : " + saveData.Storage2ItemData.ItemName);
            _storage2.SetItem(null);
        }
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
        SaveData saveData = new SaveData();

        if (PlayerPrefs.HasKey(ItemStorageSave))
        {
            string jsonData = PlayerPrefs.GetString(ItemStorageSave);
            saveData = JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            Debug.Log("нет сохранения");
            return;
        }


        /*string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/save.json");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);*/

        /*_saveDatas.Add(saveData);
        _saveData = saveData;*/


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

        if (saveData.TemporaryItem != null)
        {
            Item item = Instantiate(GetItem(saveData.TemporaryItem.ItemName), _container);
            _itemKeeper.SetTemporaryObject(item);
        }

        _replaceCounter.SetValue(saveData.ReplaceCount);
        _bulldozerCounter.SetValue(saveData.BulldozerCount);
        _goldWallet.SetValue(saveData.GoldValue);
        // Debug.Log("Gold: " + saveData.GoldValue);
        _moveCounter.SetValue(saveData.MoveCount);
        // _scoreCounter.SetValue(saveData.ScoreValue);


        Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
            saveData.SelectItemData.ItemPosition.transform.position,
            Quaternion.identity, _container);

        selectItem.Init(saveData.SelectItemData.ItemPosition);
        selectItem.gameObject.SetActive(false);
        SelectSaveItem = selectItem;
        // _itemDragger.SetItem(selectItem, saveData.SelectItemData.ItemPosition);
        // Item storageItem = Instantiate(saveData.StorageItem, _container);


        if (saveData.StorageItemData.ItemPosition != null || saveData.StorageItemData.ItemName != Items.Empty)
        {
            Debug.Log("Storage Load : " + saveData.StorageItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.StorageItemData.ItemName), _container);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage.SetItem(storageItem);
        }

        if (saveData.Storage1ItemData.ItemPosition != null || saveData.Storage1ItemData.ItemName != Items.Empty)
        {
            Debug.Log("1  Storage Load : " + saveData.Storage1ItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.Storage1ItemData.ItemName), _container);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage1.SetItem(storageItem);
        }

        if (saveData.Storage2ItemData.ItemPosition != null || saveData.Storage2ItemData.ItemName != Items.Empty)
        {
            // Debug.Log("Storage: " + saveData.StorageItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.Storage2ItemData.ItemName), _container);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage2.SetItem(storageItem);
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
        _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem, saveData.ItemDropData.Icon);
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