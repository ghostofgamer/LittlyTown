using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Enums;
using InitializationContent;
using ItemContent;
using ItemSO;
using Keeper;
using PossibilitiesContent;
using SpawnContent;
using UI;
using UnityEngine;
using Wallets;

namespace SaveAndLoad
{
    public class ItemsStorage : MonoBehaviour
    {
        private const string ItemStorageSave = "ItemStorageSave";

        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Item[] _items;
        [SerializeField] private PossibilitiesCounter _replaceCounter;
        [SerializeField] private PossibilitiesCounter _bulldozerCounter;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Storage _storage;
        [SerializeField] private Storage _storage1;
        [SerializeField] private Storage _storage2;
        [SerializeField] private DropGenerator _dropGenerator;
        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private PossibilitieBulldozer _possibilitieBulldozer;
        [SerializeField] private PossibilitieReplace _possibilitieReplace;
        [SerializeField] private Initializator _initializator;

        private Coroutine _coroutine;
        private Coroutine _coroutineSignal;
        private Item _selectObject;
        private Item _temporaryObject;
        private ItemDropDataSo _itemDropDataSO;
        private SaveData _saveData;

        public Item SelectSaveItem { get; private set; }

        private void OnEnable()
        {
            _itemKeeper.SelectNewItem += SaveChanges;
        }

        private void OnDisable()
        {
            _itemKeeper.SelectNewItem -= SaveChanges;
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
                return;
            }

            Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
                saveData.SelectItemData.ItemPosition.transform.position,
                Quaternion.identity, _initializator.CurrentMap.ItemsContainer);
            selectItem.Init(saveData.SelectItemData.ItemPosition);
            selectItem.gameObject.SetActive(false);
            SelectSaveItem = selectItem;

            if (saveData.TemporaryItem.ItemName != Items.Empty)
            {
                Item item = Instantiate(GetItem(saveData.TemporaryItem.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                _itemKeeper.SetTemporaryObject(item);
            }
            else
            {
                _itemKeeper.SetTemporaryObject(null);
            }

            Debug.Log("load");

            foreach (var item in saveData.ItemDatasPrices)
                _shopItems.SetPrice(item.ItemName, item.Price);

            _shopItems.SetPricePossibilitie(saveData.PossibilitiesItemsData.PriceBulldozer,
                saveData.PossibilitiesItemsData.PriceReplace);
            _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem, saveData.ItemDropData.Icon);
            _moveCounter.SetValue(saveData.MoveCount);
            _replaceCounter.SetValue(saveData.ReplaceCount);
            _bulldozerCounter.SetValue(saveData.BulldozerCount);
            _goldWallet.SetValue(saveData.GoldValue);
            _scoreCounter.SetValue(saveData.ScoreValue, saveData.FactorScoreValue);

            if (saveData.StorageItemData.ItemPosition != null || saveData.StorageItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(GetItem(saveData.StorageItemData.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                storageItem.gameObject.SetActive(false);
                _storage.SetItem(storageItem);
            }
            else
            {
                _storage.SetItem(null);
            }

            if (saveData.Storage1ItemData.ItemPosition != null || saveData.Storage1ItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(GetItem(saveData.Storage1ItemData.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                storageItem.gameObject.SetActive(false);
                _storage1.SetItem(storageItem);
            }
            else
            {
                _storage1.SetItem(null);
            }

            if (saveData.Storage2ItemData.ItemPosition != null || saveData.Storage2ItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(GetItem(saveData.Storage2ItemData.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                storageItem.gameObject.SetActive(false);
                _storage2.SetItem(storageItem);
            }
            else
            {
                _storage2.SetItem(null);
            }
        }

        public SaveData GetSaveData()
        {
            return _saveData;
        }

        private void SaveChanges()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(SaveDataInfo());
        }

        private IEnumerator SaveDataInfo()
        {
            SaveData saveData = new SaveData();
            List<ItemData> itemDatas = new List<ItemData>();
            _saveData = saveData;

            if (_itemKeeper.SelectedObject != null)
            {
                saveData.SelectItemData = new SelectItemData(_itemKeeper.SelectedObject.ItemName,
                    _itemKeeper.SelectedObject.ItemPosition);
                SelectSaveItem = _itemKeeper.SelectedObject;
            }

            yield return new WaitForSeconds(0.165f);

            foreach (var itemPosition in _initializator.ItemPositions)
            {
                if (itemPosition.Item != null)
                {
                    ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                    itemDatas.Add(itemData);
                }
            }

            if (_itemKeeper.TemporaryItem != null)
            {
                saveData.TemporaryItem =
                    new ItemData(_itemKeeper.TemporaryItem.ItemName, null, _itemKeeper.TemporaryItem.Price);
            }
            else
            {
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

            if (_storage.CurrentItem != null)
            {
                saveData.StorageItemData =
                    new StorageItemData(_storage.CurrentItem.ItemName, _storage.CurrentItem.ItemPosition);
            }
            else
            {
                saveData.StorageItemData = new StorageItemData(Items.Empty, null);
            }

            if (_storage1.CurrentItem != null)
            {
                saveData.Storage1ItemData =
                    new StorageItemData(_storage1.CurrentItem.ItemName, _storage1.CurrentItem.ItemPosition);
            }
            else
            {
                saveData.Storage1ItemData = new StorageItemData(Items.Empty, null);
            }

            if (_storage2.CurrentItem != null)
            {
                saveData.Storage2ItemData =
                    new StorageItemData(_storage2.CurrentItem.ItemName, _storage2.CurrentItem.ItemPosition);
            }
            else
            {
                saveData.Storage2ItemData = new StorageItemData(Items.Empty, null);
            }

            string jsonData = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(ItemStorageSave + _initializator.Index, jsonData);
            PlayerPrefs.Save();
            yield return null;
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
}