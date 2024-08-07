using System;
using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Dragger;
using Enums;
using InitializationContent;
using ItemContent;
using ItemPositionContent;
using PossibilitiesContent;
using Road;
using SaveAndLoad;
using SpawnContent;
using UI;
using UI.Screens;
using UnityEngine;
using Wallets;

namespace Keeper
{
    public class MovesKeeper : MonoBehaviour
    {
        private const string ItemStorageSave = "ItemStorageSave";
        private const string SaveHistoryName = "SaveHistoryName";

        [SerializeField] private Initializator _initializator;
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private GameStorage _gameStorage;
        [SerializeField] private Item[] _items;
        [SerializeField] private PossibilitiesCounter _replaceCounter;
        [SerializeField] private PossibilitiesCounter _bulldozerCounter;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private RoadGenerator _roadGenerator;
        [SerializeField] private Storage _storage;
        [SerializeField] private DropGenerator _dropGenerator;
        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private CompleteScoreScreen _completeScoreScreen;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private ItemThrower _itemThrower;
        [SerializeField] private ReplacementPosition _replacementPosition;
        [SerializeField] private RemovalItems _removalItems;

        private List<SaveData> _savesHistory = new List<SaveData>();
        private int _currentStep = -1;
        private Coroutine _coroutine;
        private SaveData _saveData;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        private float _maxCountSaveHistory = 1;

        public event Action<int> StepChanged;

        public int CurrentStep => _currentStep;

        private void OnEnable()
        {
            _itemThrower.StepCompleted += OnSaveHistory;
            _completeScoreScreen.ScoreCompleted += OnResetSteps;
            _removalItems.Removing += OnSaveHistory;
            _replacementPosition.PositionChanging += OnSaveHistory;
        }

        private void OnDisable()
        {
            _itemThrower.StepCompleted -= OnSaveHistory;
            _completeScoreScreen.ScoreCompleted -= OnResetSteps;
            _removalItems.Removing -= OnSaveHistory;
            _replacementPosition.PositionChanging -= OnSaveHistory;
        }

        public void LoadHistoryData()
        {
            SaveHistoryData saveHistoryData = new SaveHistoryData();

            if (PlayerPrefs.HasKey(SaveHistoryName + _initializator.Index))
            {
                string jsonData = PlayerPrefs.GetString(SaveHistoryName + _initializator.Index);
                saveHistoryData = JsonUtility.FromJson<SaveHistoryData>(jsonData);
                _savesHistory.Clear();
                _savesHistory = saveHistoryData.SavesHistory;
                _currentStep = _savesHistory.Count;
                StepChanged?.Invoke(_currentStep);
            }
            else
            {
                _savesHistory.Clear();
                _currentStep = _savesHistory.Count;
                StepChanged?.Invoke(_currentStep);
            }
        }

        public void CancelLastStep()
        {
            if (_currentStep <= 0)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartCancelling());
        }

        public void ClearList()
        {
            _savesHistory.Clear();
        }

        private void SaveHistoryData()
        {
            SaveHistoryData saveHistoryData = new SaveHistoryData();
            saveHistoryData.SavesHistory = _savesHistory;
            string jsonData = JsonUtility.ToJson(saveHistoryData);
            PlayerPrefs.SetString(SaveHistoryName + _initializator.Index, jsonData);
            PlayerPrefs.Save();
        }

        private void OnSaveHistory()
        {
            if (_savesHistory.Count >= _maxCountSaveHistory)
                _savesHistory.Clear();

            SaveData saveDate = _gameStorage.GetSaveData();
            _savesHistory.Add(saveDate);
            _currentStep = _savesHistory.Count;
            SaveHistoryData();
            StepChanged?.Invoke(_currentStep);
        }

        private void OnSaveHistory(ItemPosition itemPosition)
        {
            if (_savesHistory.Count >= _maxCountSaveHistory)
                _savesHistory.Clear();

            if (PlayerPrefs.HasKey(ItemStorageSave + _initializator.CurrentMap.Index))
            {
                string jsonData = PlayerPrefs.GetString(ItemStorageSave + _initializator.CurrentMap.Index);
                _saveData = JsonUtility.FromJson<SaveData>(jsonData);
            }

            _saveData.SelectItemData.ItemPosition = itemPosition;
            _savesHistory.Add(_saveData);
            _currentStep = _savesHistory.Count;
            StepChanged?.Invoke(_currentStep);
            SaveHistoryData();
        }

        private IEnumerator StartCancelling()
        {
            if (_itemKeeper.SelectedObject != null)
                _itemKeeper.SelectedObject.gameObject.SetActive(false);

            foreach (ItemPosition itemPosition in _initializator.ItemPositions)
            {
                if (itemPosition.Item != null)
                {
                    ClearPositions(itemPosition);
                    yield return _waitForSeconds;
                }

                itemPosition.DisableRoad();
            }

            yield return _waitForSeconds;
            _currentStep--;
            SaveData saveData = _savesHistory[_currentStep];

            foreach (ItemData itemData in saveData.ItemDatas)
            {
                CreateItem(itemData);
                yield return _waitForSeconds;
            }

            InitValues(saveData);
            yield return null;
            _roadGenerator.OnGeneration();
            _currentStep = 0;
            StepChanged?.Invoke(_currentStep);
            _savesHistory.Clear();
            SaveHistoryData();
        }

        private void ClearPositions(ItemPosition itemPosition)
        {
            _audioSource.PlayOneShot(_audioClip);
            itemPosition.Item.gameObject.SetActive(false);
            itemPosition.ClearingPosition();
        }

        private void CreateItem(ItemData itemData)
        {
            Item item = Instantiate(
                GetItem(itemData.ItemName),
                itemData.ItemPosition.transform.position,
                Quaternion.identity,
                _initializator.CurrentMap.ItemsContainer);
            item.Init(itemData.ItemPosition);
            item.Activation();
            _audioSource.PlayOneShot(_audioSource.clip);
        }

        private void InitValues(SaveData saveData)
        {
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

            Item selectItem = Instantiate(
                GetItem(saveData.SelectItemData.ItemName),
                saveData.SelectItemData.ItemPosition.transform.position,
                Quaternion.identity,
                _initializator.CurrentMap.ItemsContainer);

            _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem, saveData.ItemDropData.Icon);
            selectItem.Init(saveData.SelectItemData.ItemPosition);
            _itemKeeper.SetItem(selectItem, selectItem.ItemPosition);
            selectItem.gameObject.SetActive(true);
            _goldWallet.SetValue(saveData.GoldValue);

            foreach (ItemData item in saveData.ItemDatasPrices)
                _shopItems.SetPrice(item.ItemName, item.Price);

            _shopItems.SetPricePossibilitie(
                saveData.PossibilitiesItemsData.PriceBulldozer,
                saveData.PossibilitiesItemsData.PriceReplace);

            if (saveData.StorageItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(
                    GetItem(saveData.StorageItemData.ItemName),
                    _initializator.CurrentMap.ItemsContainer);
                storageItem.gameObject.SetActive(false);
                _storage.SetItem(storageItem);
            }
            else
            {
                _storage.ClearItem();
            }

            _replaceCounter.SetValue(saveData.ReplaceCount);
            _bulldozerCounter.SetValue(saveData.BulldozerCount);
            _goldWallet.SetValue(saveData.GoldValue);
            _moveCounter.SetValue(saveData.MoveCount);
        }

        private Item GetItem(Items itemName)
        {
            foreach (Item item in _items)
            {
                if (item.ItemName == itemName)
                    return item;
            }

            return null;
        }

        private void OnResetSteps()
        {
            _currentStep = 0;
            StepChanged?.Invoke(_currentStep);
        }
    }
}