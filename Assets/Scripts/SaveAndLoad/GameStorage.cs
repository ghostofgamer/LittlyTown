using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Enums;
using InitializationContent;
using ItemContent;
using Keeper;
using PossibilitiesContent;
using SpawnContent;
using UI;
using UnityEngine;
using Wallets;

namespace SaveAndLoad
{
    public class GameStorage : MonoBehaviour
    {
        private const string ItemStorageSave = "ItemStorageSave";

        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Item[] _items;
        [SerializeField] private PossibilitiesCounter _replaceCounter;
        [SerializeField] private PossibilitiesCounter _bulldozerCounter;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Storage _firstStorage;
        [SerializeField] private Storage _secondStorage;
        [SerializeField] private Storage _thirdStorage;
        [SerializeField] private DropGenerator _dropGenerator;
        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private PossibilitieBulldozer _possibilitieBulldozer;
        [SerializeField] private PossibilitieReplace _possibilitieReplace;
        [SerializeField] private Initializator _initializator;

        private Coroutine _coroutine;
        private SaveData _saveData;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.165f);

        public Item SelectSaveItem { get; private set; }

        private void OnEnable()
        {
            _itemKeeper.SelectNewItem += OnSaveChanges;
            _goldWallet.ValueChangedCompleted += OnSaveChanges;
        }

        private void OnDisable()
        {
            _itemKeeper.SelectNewItem -= OnSaveChanges;
            _goldWallet.ValueChangedCompleted -= OnSaveChanges;
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

            Item selectItem = Instantiate(
                GetItem(saveData.SelectItemData.ItemName),
                saveData.SelectItemData.ItemPosition.transform.position,
                Quaternion.identity,
                _initializator.CurrentMap.ItemsContainer);
            selectItem.Init(saveData.SelectItemData.ItemPosition);
            selectItem.gameObject.SetActive(false);
            SelectSaveItem = selectItem;

            if (saveData.TemporaryItem.ItemName != Items.Empty)
            {
                Item item = Instantiate(
                    GetItem(saveData.TemporaryItem.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                _itemKeeper.SetTemporaryObject(item);
            }
            else
            {
                _itemKeeper.SetTemporaryObject(null);
            }

            foreach (var item in saveData.ItemDatasPrices)
                _shopItems.SetPrice(item.ItemName, item.Price);

            _shopItems.SetPricePossibilitie(
                saveData.PossibilitiesItemsData.PriceBulldozer,
                saveData.PossibilitiesItemsData.PriceReplace);
            _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem, saveData.ItemDropData.Icon);
            _moveCounter.SetValue(saveData.MoveCount);
            _replaceCounter.SetValue(saveData.ReplaceCount);
            _bulldozerCounter.SetValue(saveData.BulldozerCount);
            _goldWallet.SetValue(saveData.GoldValue);
            _scoreCounter.SetValue(saveData.ScoreValue, saveData.FactorScoreValue);

            if (saveData.StorageItemData.ItemPosition != null || saveData.StorageItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(
                    GetItem(saveData.StorageItemData.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                storageItem.gameObject.SetActive(false);
                _firstStorage.SetItem(storageItem);
            }
            else
            {
                _firstStorage.SetItem(null);
            }

            if (saveData.Storage1ItemData.ItemPosition != null || saveData.Storage1ItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(
                    GetItem(saveData.Storage1ItemData.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                storageItem.gameObject.SetActive(false);
                _secondStorage.SetItem(storageItem);
            }
            else
            {
                _secondStorage.SetItem(null);
            }

            if (saveData.Storage2ItemData.ItemPosition != null || saveData.Storage2ItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(
                    GetItem(saveData.Storage2ItemData.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                storageItem.gameObject.SetActive(false);
                _thirdStorage.SetItem(storageItem);
            }
            else
            {
                _thirdStorage.SetItem(null);
            }
        }

        public SaveData GetSaveData()
        {
            return _saveData;
        }

        private void OnSaveChanges()
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
                saveData.SelectItemData = new SelectItemData(
                    _itemKeeper.SelectedObject.ItemName,
                    _itemKeeper.SelectedObject.ItemPosition);
                SelectSaveItem = _itemKeeper.SelectedObject;
            }

            yield return _waitForSeconds;

            foreach (var itemPosition in _initializator.ItemPositions)
            {
                if (itemPosition.Item != null)
                {
                    ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                    itemDatas.Add(itemData);
                }
            }

            saveData.TemporaryItem = _itemKeeper.TemporaryItem != null
                ? new ItemData(_itemKeeper.TemporaryItem.ItemName, null, _itemKeeper.TemporaryItem.Price)
                : new ItemData(Items.Empty, null, 0);
            List<ItemData> itemDataPrice = new List<ItemData>();

            foreach (var item in _shopItems.Items)
            {
                ItemData itemData = new ItemData(item.ItemName, null, item.Price);
                itemDataPrice.Add(itemData);
            }

            saveData.PossibilitiesItemsData =
                new PossibilitiesItemsData(_possibilitieBulldozer.Price, _possibilitieReplace.Price);
            saveData.ItemDatasPrices = itemDataPrice;
            saveData.ItemDropData = _dropGenerator.ItemDropData != null
                ? new ItemDropData(_dropGenerator.ItemDropData.Icon, _dropGenerator.ItemDropData.PrefabItem)
                : null;
            saveData.MoveCount = _moveCounter.MoveCount;
            saveData.ItemDatas = itemDatas;
            saveData.BulldozerCount = _bulldozerCounter.PossibilitiesCount;
            saveData.ReplaceCount = _replaceCounter.PossibilitiesCount;
            saveData.GoldValue = _goldWallet.CurrentValue;
            saveData.ScoreValue = _scoreCounter.CurrentScore;
            saveData.FactorScoreValue = _scoreCounter.Factor;
            saveData.StorageItemData = _firstStorage.CurrentItem != null
                ? new StorageItemData(_firstStorage.CurrentItem.ItemName, _firstStorage.CurrentItem.ItemPosition)
                : new StorageItemData(Items.Empty, null);
            saveData.Storage1ItemData = _secondStorage.CurrentItem != null
                ? new StorageItemData(_secondStorage.CurrentItem.ItemName, _secondStorage.CurrentItem.ItemPosition)
                : new StorageItemData(Items.Empty, null);
            saveData.Storage2ItemData = _thirdStorage.CurrentItem != null
                ? new StorageItemData(_thirdStorage.CurrentItem.ItemName, _thirdStorage.CurrentItem.ItemPosition)
                : new StorageItemData(Items.Empty, null);

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