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
        [SerializeField] private ItemsStorage _itemsStorage;
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

        public List<SaveData> _savesHistory = new List<SaveData>();
        private int _currentStep = -1;
        private Coroutine _coroutine;
        private SaveData _saveData;

        public event Action<int> StepChanged;

        public int CurrentStep => _currentStep;


        private void OnEnable()
        {
            _itemThrower.StepCompleted += SaveHistory;
            _completeScoreScreen.ScoreCompleted += ResetSteps;
            _removalItems.Removing += SaveHistory;
            _replacementPosition.PositionChanging += SaveHistory;
        }

        private void OnDisable()
        {
            _itemThrower.StepCompleted -= SaveHistory;
            _completeScoreScreen.ScoreCompleted -= ResetSteps;
            _removalItems.Removing -= SaveHistory;
            _replacementPosition.PositionChanging -= SaveHistory;
        }

        private void SaveHistoryData()
        {
            SaveHistoryData saveHistoryData = new SaveHistoryData();
            saveHistoryData.savesHistory = _savesHistory;
            string jsonData = JsonUtility.ToJson(saveHistoryData);
            PlayerPrefs.SetString(SaveHistoryName + _initializator.Index, jsonData);
            PlayerPrefs.Save();
        }

        public void ClearList()
        {
            _savesHistory.Clear();
        }

        public void LoadHistoryData()
        {
            SaveHistoryData saveHistoryData = new SaveHistoryData();

            if (PlayerPrefs.HasKey(SaveHistoryName + _initializator.Index))
            {
                string jsonData = PlayerPrefs.GetString(SaveHistoryName + _initializator.Index);
                saveHistoryData = JsonUtility.FromJson<SaveHistoryData>(jsonData);
                _savesHistory.Clear();
                _savesHistory = saveHistoryData.savesHistory;
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

        private void SaveHistory()
        {
            if (_savesHistory.Count >= 1)
                _savesHistory.Clear();

            SaveData saveDate = _itemsStorage.GetSaveData();
            _savesHistory.Add(saveDate);
            _currentStep = _savesHistory.Count;
            SaveHistoryData();
            StepChanged?.Invoke(_currentStep);
        }

        private void SaveHistory(ItemPosition itemPosition)
        {
            if (_savesHistory.Count >= 1)
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

        public void CancelLastStep()
        {
            if (_currentStep <= 0)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartCancelling());
        }

        private IEnumerator StartCancelling()
        {
            if (_itemKeeper.SelectedObject != null)
                _itemKeeper.SelectedObject.gameObject.SetActive(false);

            foreach (var itemPosition in _initializator.ItemPositions)
            {
                if (itemPosition.Item != null)
                {
                    _audioSource.PlayOneShot(_audioClip);
                    itemPosition.Item.gameObject.SetActive(false);
                    itemPosition.ClearingPosition();
                    yield return new WaitForSeconds(0.1f);
                }

                itemPosition.DisableRoad();
            }

            yield return new WaitForSeconds(0.1f);
            _currentStep--;
            SaveData saveData = _savesHistory[_currentStep];

            foreach (var itemData in saveData.ItemDatas)
            {
                Item item = Instantiate(GetItem(itemData.ItemName), itemData.ItemPosition.transform.position,
                    Quaternion.identity, _initializator.CurrentMap.ItemsContainer);
                item.Init(itemData.ItemPosition);
                item.Activation();
                _audioSource.PlayOneShot(_audioSource.clip);
                yield return new WaitForSeconds(0.1f);
            }

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

            Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
                saveData.SelectItemData.ItemPosition.transform.position,
                Quaternion.identity, _initializator.CurrentMap.ItemsContainer);

            _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem, saveData.ItemDropData.Icon);
            selectItem.Init(saveData.SelectItemData.ItemPosition);
            _itemKeeper.SetItem(selectItem, selectItem.ItemPosition);
            selectItem.gameObject.SetActive(true);
            _goldWallet.SetValue(saveData.GoldValue);

            foreach (var item in saveData.ItemDatasPrices)
                _shopItems.SetPrice(item.ItemName, item.Price);


            _shopItems.SetPricePossibilitie(saveData.PossibilitiesItemsData.PriceBulldozer,
                saveData.PossibilitiesItemsData.PriceReplace);


            if (saveData.StorageItemData.ItemName != Items.Empty)
            {
                Item storageItem = Instantiate(GetItem(saveData.StorageItemData.ItemName),
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
            yield return null;
            _roadGenerator.OnGeneration();
            _currentStep = 0;
            StepChanged?.Invoke(_currentStep);
            _savesHistory.Clear();
            SaveHistoryData();
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

        private void ResetSteps()
        {
            _currentStep = 0;
            StepChanged?.Invoke(_currentStep);
        }
    }
}