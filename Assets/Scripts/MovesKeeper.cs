using System;
using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;
using Wallets;

public class MovesKeeper : MonoBehaviour
{
    private const string ItemStorageSave = "ItemStorageSave";
    private const string SaveHistoryName = "SaveHistoryName";
  
    [SerializeField] private Initializator _initializator;
    
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private ItemsStorage _itemsStorage;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Transform _container;
    [SerializeField] private Item[] _items;
    [SerializeField] private PossibilitiesCounter _replaceCounter;
    [SerializeField] private PossibilitiesCounter _bulldozerCounter;
    [SerializeField] private GoldWallet _goldWallet;
    [SerializeField] private MoveCounter _moveCounter;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Storage _storage;
    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ShopItems _shopItems;
    [SerializeField] private CompleteScoreScreen _completeScoreScreen;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    public List<SaveData> _savesHistory = new List<SaveData>();
    private int _maxStepSaved = 3;
    private int _currentStep = -1;
    private Coroutine _coroutine;

    public int CurrentStep => _currentStep;

    public event Action<int> StepChanged;

    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private ReplacementPosition _replacementPosition;
    [SerializeField] private RemovalItems _removalItems;

    private SaveData _saveData;
    
    private void OnEnable()
    {
        _itemDragger.StepCompleted += SaveHistory;
        _completeScoreScreen.ScoreCompleted += ResetSteps;
        // _replacementPosition.PositionsChanged += SaveHistory;
        _removalItems.Removing += SaveHistory;

        /*_itemDragger.PlaceChanged += SaveHistory;
        // _itemDragger.BuildItem += SaveChanges;
        // _itemDragger.SelectItemReceived += SaveHistory;
        _mapGenerator.GenerationCompleted += SaveHistory;
        _replacementPosition.PositionsChanged += SaveHistory;
        _removalItems.Removed += SaveHistory;
        _storage.StoragePlaceChanged += SaveHistory;*/
    }

    private void OnDisable()
    {
        _itemDragger.StepCompleted -= SaveHistory;
        _completeScoreScreen.ScoreCompleted -= ResetSteps;
        // _replacementPosition.PositionsChanged -= SaveHistory;
        _removalItems.Removing -= SaveHistory;

        /*_itemDragger.PlaceChanged -= SaveHistory;
        // _itemDragger.BuildItem -= SaveChanges;
        // _itemDragger.SelectItemReceived -= SaveHistory;
        _mapGenerator.GenerationCompleted -= SaveHistory;
        _replacementPosition.PositionsChanged -= SaveHistory;
        _removalItems.Removed -= SaveHistory;
        _storage.StoragePlaceChanged -= SaveHistory;*/
    }

    /*private void OnEnable()
    {
        _itemsStorage.SaveCompleted += SaveHistory;
    }

    private void OnDisable()
    {
        _itemsStorage.SaveCompleted -= SaveHistory;
    }*/

    public void SaveHistoryData()
    {
        SaveHistoryData saveHistoryData = new SaveHistoryData();
        saveHistoryData.savesHistory = _savesHistory;
        
        string jsonData = JsonUtility.ToJson(saveHistoryData);
        PlayerPrefs.SetString(SaveHistoryName + _initializator.Index, jsonData);
        PlayerPrefs.Save();
        
        Debug.Log("сохраняем " + saveHistoryData.savesHistory.Count);
    }

    public void LoadHistoryData()
    {
        SaveHistoryData saveHistoryData = new SaveHistoryData();
        
        if (PlayerPrefs.HasKey(SaveHistoryName+_initializator.Index))
        {
            string jsonData = PlayerPrefs.GetString(SaveHistoryName+_initializator.Index);
            saveHistoryData = JsonUtility.FromJson<SaveHistoryData>(jsonData);
            // _savesHistory.Clear();
            _savesHistory = new List<SaveData>();
            _savesHistory = saveHistoryData.savesHistory;
            _currentStep = _savesHistory.Count;
            StepChanged?.Invoke(_currentStep);
            Debug.Log("Есть сохранение " + _savesHistory.Count);
        }
        else
        {
            _savesHistory.Clear();
            _currentStep = _savesHistory.Count;
            StepChanged?.Invoke(_currentStep);
            Debug.Log("нет сохранения " + _savesHistory.Count);
            return;
        }
    }
    
    private void RemoveStep()
    {
        _savesHistory.RemoveAt(0);
        _currentStep--;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("CurrentStep  Удаление " + _currentStep);
    }

    private void SaveHistory()
    {
        if (_savesHistory.Count >= 1)
        {
            _savesHistory.Clear();
        }

        SaveData saveDate = _itemsStorage.GetSaveData();
        _savesHistory.Add(saveDate);
        _currentStep = _savesHistory.Count;
        SaveHistoryData();
        StepChanged?.Invoke(_currentStep);
    }

    /*private void SaveHistory(SaveData saveData)
    {
        if (_currentStep >= _maxStepSaved)
        {
            RemoveStep();
        }

        _savesHistory.Add(saveData);
        Debug.Log("сохраняем " + _savesHistory.Count);
        _currentStep = _savesHistory.Count - 1;

        StepChanged?.Invoke(_currentStep);
        Debug.Log("CurrentStep   " + _currentStep);
    }*/

    private void SaveHistory(ItemPosition itemPosition)
    {
        /*if (_savesHistory.Count >= _maxStepSaved)
        {
            _savesHistory.RemoveAt(0);
        }*/
        if (_savesHistory.Count >= 1)
        {
            // _savesHistory.RemoveAt(0);
            _savesHistory.Clear();
        }
        
        if (PlayerPrefs.HasKey(ItemStorageSave + _initializator.CurrentMap.Index))
        {
            string jsonData = PlayerPrefs.GetString(ItemStorageSave + _initializator.CurrentMap.Index);
            _saveData  = JsonUtility.FromJson<SaveData>(jsonData);
        }
        // SaveData saveDate = _itemsStorage.GetSaveData();
        
        
        
        // Debug.Log("ПОЛОЖИЛИ " + saveDate.ItemDropData.PrefabItem.ItemName);
        _saveData.SelectItemData.ItemPosition = itemPosition;
        // Debug.Log("SelecItemPosition Save History  " + saveDate.SelectItemData.ItemPosition);

        _savesHistory.Add(_saveData);
        _currentStep = _savesHistory.Count;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("CurrentStep  " + _currentStep);
        SaveHistoryData();



        /*if (_savesHistory.Count >= _maxStepSaved)
        {
            _savesHistory.RemoveAt(0);
        }

        _savesHistory.Add( _itemsStorage.SaveData);
        _currentStep = _savesHistory.Count - 1;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("В шагах смотрим " + saveData.SelectItemDragger);*/
    }

    /*private void SaveHistory()
    {
        if (_savesHistory.Count >= _maxStepSaved)
        {
            _savesHistory.RemoveAt(0);
        }

        SaveData saveDate = _itemsStorage.GetSaveData();
        // Debug.Log("SelecItemPosition Save History  " + saveDate.SelectItemData.ItemPosition);

        _savesHistory.Add(saveDate);
        _currentStep = _savesHistory.Count;
        StepChanged?.Invoke(_currentStep);
    }*/

    /*private void SaveHistory(SaveData saveData)
    {
        if (_savesHistory.Count >= _maxStepSaved)
        {
            _savesHistory.RemoveAt(0);
        }

        _savesHistory.Add(saveData);
        _currentStep = _savesHistory.Count - 1;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("В шагах смотрим " + saveData.SelectItemDragger);
    }*/


    public void CancelLastStep()
    {
        // Debug.Log("CurrentStep   " + _currentStep);
        if (_currentStep <= 0)
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(StartCancelling());
    }

    private IEnumerator StartCancelling()
    {
        // _currentStep--;
        // SaveData newSaveData = _savesHistory[_currentStep];
        // Debug.Log("ITEMDROPPP " + newSaveData.ItemDropData);
        // _dropGenerator.SetItem(newSaveData.ItemDropData.PrefabItem, newSaveData.ItemDropData.Icon);
        
        if (_itemDragger.SelectedObject != null)
            _itemDragger.SelectedObject.gameObject.SetActive(false);
        
        // Debug.Log(_itemsStorage.SelectObject.ItemName);
        /*foreach (var itemPosition in _itemPositions)
        {
            if (itemPosition.Item != null)
            {
                _audioSource.PlayOneShot(_audioClip);
                Debug.Log("удаляем");
                itemPosition.Item.gameObject.SetActive(false);
                itemPosition.ClearingPosition();
                yield return new WaitForSeconds(0.1f);
            }
        }*/
        
        foreach (var itemPosition in _initializator.ItemPositions)
        {
            if (itemPosition.Item != null)
            {
                _audioSource.PlayOneShot(_audioClip);
                Debug.Log("удаляем");
                itemPosition.Item.gameObject.SetActive(false);
                itemPosition.ClearingPosition();
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.1f);

        _currentStep--;
        // Debug.Log("CurrentStepIndex = " + _currentStep);
        SaveData saveData = _savesHistory[_currentStep];
        // Debug.Log("какую index истории вызываем " + _currentStep);
        /*Debug.Log("ITEMDROPPP " + saveData.ItemDropData);
        _dropGenerator.SetItem(saveData.ItemDropData);*/


        foreach (var itemData in saveData.ItemDatas)
        {
            Item item = Instantiate(GetItem(itemData.ItemName), itemData.ItemPosition.transform.position,
                Quaternion.identity, _initializator.CurrentMap.ItemsContainer);
            item.Init(itemData.ItemPosition);
            item.Activation();
            _audioSource.PlayOneShot(_audioSource.clip);
            yield return new WaitForSeconds(0.1f);
        }

        /*if (saveData.TemporaryItem != null)
        {
            Item item = Instantiate(GetItem(saveData.TemporaryItem.ItemName), _container);
            _itemDragger.SetTemporaryObject(item);
        }*/
        
        if (saveData.TemporaryItem.ItemName != Items.Empty)
        {
            // Debug.Log("TemporaryItemNotNull " + saveData.TemporaryItem.ItemName);
            Item item = Instantiate(GetItem(saveData.TemporaryItem.ItemName), _initializator.CurrentMap.ItemsContainer);
            _itemDragger.SetTemporaryObject(item);
        }
        else
        {
            _itemDragger.SetTemporaryObject(null);
        }

        // Debug.Log("saveData.SelectItem " + saveData.SelectItemData.ItemName);
        // Debug.Log("saveData.SelectItemPosition " + saveData.SelectItemData.ItemPosition.name);

        Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
            saveData.SelectItemData.ItemPosition.transform.position,
            Quaternion.identity, _container);

        // Debug.Log("CANCELING DROP Item " + saveData.ItemDropData.PrefabItem.ItemName);
        _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem, saveData.ItemDropData.Icon);
        // _dropGenerator.SetItem(saveData.ItemDropData.PrefabItem,saveData.ItemDropData.Icon);
        selectItem.Init(saveData.SelectItemData.ItemPosition);
        // Debug.Log("CANCELING Select Item " + selectItem.ItemName);
        _itemDragger.SetItem(selectItem, selectItem.ItemPosition);
        selectItem.gameObject.SetActive(true);
        _goldWallet.SetValue(saveData.GoldValue);

        foreach (var item in saveData.ItemDatasPrices)
        {
            _shopItems.SetPrice(item.ItemName, item.Price);
        }

        _shopItems.SetPricePossibilitie(saveData.PossibilitiesItemsData.PriceBulldozer,
            saveData.PossibilitiesItemsData.PriceReplace);


        if (saveData.StorageItemData.ItemName != Items.Empty)
        {
            // Debug.Log("Storage: ");
            // Debug.Log("Storage: CancelLoad " + saveData.StorageItemData.ItemName);
            Item storageItem = Instantiate(GetItem(saveData.StorageItemData.ItemName), _initializator.CurrentMap.ItemsContainer);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage.SetItem(storageItem);
        }
        else
        {
            // Debug.Log("clear Storage ");
            _storage.ClearItem();
        }

        // Debug.Log("ReplaceCount " + saveData.ReplaceCount);
        _replaceCounter.SetValue(saveData.ReplaceCount);
        _bulldozerCounter.SetValue(saveData.BulldozerCount);
        _goldWallet.SetValue(saveData.GoldValue);
        _moveCounter.SetValue(saveData.MoveCount);
        // _scoreCounter.SetValue(saveData.ScoreValue);
        Debug.Log("Score Value Canceling" + saveData.ScoreValue);

        yield return null;
        _roadGenerator.OnGeneration();
        // ClearAllHistory();
        _currentStep = 0;
        StepChanged?.Invoke(_currentStep);
        
        
        
        
        _savesHistory.Clear();
        SaveHistoryData();
        /*_replaceCounter.SetValue(saveData.ReplaceCount);
        _bulldozerCounter.SetValue(saveData.BulldozerCount);
        _goldWallet.SetValue(saveData.GoldValue);
        _moveCounter.SetValue(saveData.MoveCount);
        _scoreCounter.SetValue(saveData.ScoreValue);
        _storage.SetItem(saveData.StorageItem);


        /*Item selectItem = Instantiate(GetItem(saveData.SelectItemDragger.ItemName),saveData.SelectItemDragger.ItemPosition.transform.position,
            Quaternion.identity, _container);#1#

        Item selectItem = Instantiate(GetItem(_itemsStorage.SelectObject.ItemName),
            _itemsStorage.SelectObject.ItemPosition.transform.position,
            Quaternion.identity, _container);

        _itemDragger.SetItem(selectItem, _itemsStorage.SelectObject.ItemPosition);
        selectItem.gameObject.SetActive(true);

        /*Item temporaryItem = Instantiate(GetItem(saveData.TemporaryItemDragger.ItemName),saveData.TemporaryItemDragger.ItemPosition.transform.position,
            Quaternion.identity, _container);
        _itemDragger.SetTemporaryObject(temporaryItem);#1#


        yield return null;
        _roadGenerator.OnGeneration();
        ClearHistory();
        yield return new WaitForSeconds(0.1f);
        // _itemsStorage.SaveChanges();*/
    }


    /*private IEnumerator StartCancelling()
    {
        SaveData newSaveData = _savesHistory[_currentStep];
        // Debug.Log("ITEMDROPPP " + newSaveData.ItemDropData);
        _dropGenerator.SetItem(newSaveData.ItemDropData.PrefabItem, newSaveData.ItemDropData.Icon);

        _itemDragger.SelectedObject.gameObject.SetActive(false);
        // Debug.Log(_itemsStorage.SelectObject.ItemName);
        foreach (var itemPosition in _itemPositions)
        {
            if (itemPosition.Item != null)
            {
                itemPosition.Item.gameObject.SetActive(false);
                itemPosition.ClearingPosition();
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.1f);

        _currentStep--;
        SaveData saveData = _savesHistory[_currentStep];
        // Debug.Log("какую index истории вызываем " + _currentStep);
        /*Debug.Log("ITEMDROPPP " + saveData.ItemDropData);
        _dropGenerator.SetItem(saveData.ItemDropData);#1#

        foreach (var itemData in saveData.ItemDatas)
        {
            Item item = Instantiate(GetItem(itemData.ItemName), itemData.ItemPosition.transform.position,
                Quaternion.identity, _container);
            item.Init(itemData.ItemPosition);
            item.Activation();
            yield return new WaitForSeconds(0.1f);
        }

        _replaceCounter.SetValue(saveData.ReplaceCount);
        _bulldozerCounter.SetValue(saveData.BulldozerCount);
        _goldWallet.SetValue(saveData.GoldValue);
        _moveCounter.SetValue(saveData.MoveCount);
        _scoreCounter.SetValue(saveData.ScoreValue);
        _storage.SetItem(saveData.StorageItem);
        // Debug.Log("ШАГ Вперед " + saveData.SelectItemDragger.ItemName);
        // Debug.Log("ШАГ !!! " + _itemsStorage.SelectObject.ItemName);
        // Debug.Log("ШАГ Середина " + _itemsStorage.SelectObject.ItemPosition);

        /*Item selectItem = Instantiate(GetItem(saveData.SelectItemDragger.ItemName),saveData.SelectItemDragger.ItemPosition.transform.position,
            Quaternion.identity, _container);#1#

        Item selectItem = Instantiate(GetItem(_itemsStorage.SelectObject.ItemName),
            _itemsStorage.SelectObject.ItemPosition.transform.position,
            Quaternion.identity, _container);

        _itemDragger.SetItem(selectItem, _itemsStorage.SelectObject.ItemPosition);
        selectItem.gameObject.SetActive(true);
        // Debug.Log("ШАГ НАЗАД " + selectItem.name);

        /*Item temporaryItem = Instantiate(GetItem(saveData.TemporaryItemDragger.ItemName),saveData.TemporaryItemDragger.ItemPosition.transform.position,
            Quaternion.identity, _container);
        _itemDragger.SetTemporaryObject(temporaryItem);#1#


        yield return null;
        _roadGenerator.OnGeneration();
        ClearHistory();
        yield return new WaitForSeconds(0.1f);
        // _itemsStorage.SaveChanges();
    }*/

    private Item GetItem(Items itemName)
    {
        foreach (var item in _items)
        {
            if (item.ItemName == itemName)
                return item;
        }

        return null;
    }

    private void ClearHistory()
    {
        /*int countToRemove = _savesHistory.Count - 1;
        if (countToRemove > 0)
        {
            _savesHistory.RemoveRange(0, countToRemove);
        }*/

        // _savesHistory.Clear();
        SaveData newSaveData = _savesHistory[_savesHistory.Count - 2];
        _savesHistory.Clear();
        _savesHistory.Add(newSaveData);
        _currentStep = 0;
        // Debug.Log("история " + _savesHistory.Count);
        StepChanged?.Invoke(_currentStep);
        // _itemsStorage.SaveChanges();
        // _itemsStorage.CancelMoveSave();
        // Debug.Log("CurrentStep   Очистка" + _currentStep);
    }

    public void ClearAllHistory()
    {
        _savesHistory.Clear();
        _currentStep = _savesHistory.Count;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("CurrentStepFromClear  " + _currentStep);
    }

    private void ResetSteps()
    {
        _currentStep = 0;
        StepChanged?.Invoke(_currentStep);
    }
}