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

public class MovesKeeper : MonoBehaviour
{
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

    private List<SaveData> _savesHistory = new List<SaveData>();
    private int _maxStepSaved = 3;
    private int _currentStep = -1;
    private Coroutine _coroutine;

    public int CurrentStep => _currentStep;

    public event Action<int> StepChanged;

    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private ReplacementPosition _replacementPosition;
    [SerializeField] private RemovalItems _removalItems;

    private void OnEnable()
    {
        _itemDragger.StepCompleted += SaveHistory;

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

    private void RemoveStep()
    {
        _savesHistory.RemoveAt(0);
        _currentStep--;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("CurrentStep  Удаление " + _currentStep);
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
        if (_savesHistory.Count >= _maxStepSaved)
        {
            _savesHistory.RemoveAt(0);
        }

        SaveData saveDate = _itemsStorage.GetSaveData();
        saveDate.SelectItemData.ItemPosition = itemPosition;
        // Debug.Log("SelecItemPosition Save History  " + saveDate.SelectItemData.ItemPosition);

        _savesHistory.Add(saveDate);
        _currentStep = _savesHistory.Count;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("CurrentStep  " + _currentStep);

        /*if (_savesHistory.Count >= _maxStepSaved)
        {
            _savesHistory.RemoveAt(0);
        }

        _savesHistory.Add( _itemsStorage.SaveData);
        _currentStep = _savesHistory.Count - 1;
        StepChanged?.Invoke(_currentStep);
        // Debug.Log("В шагах смотрим " + saveData.SelectItemDragger);*/
    }

    private void SaveHistory()
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
    }

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
        _dropGenerator.SetItem(saveData.ItemDropData);*/


        foreach (var itemData in saveData.ItemDatas)
        {
            Item item = Instantiate(GetItem(itemData.ItemName), itemData.ItemPosition.transform.position,
                Quaternion.identity, _container);
            item.Init(itemData.ItemPosition);
            item.Activation();
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("saveData.SelectItem " + saveData.SelectItemData.ItemName);
        // Debug.Log("saveData.SelectItemPosition " + saveData.SelectItemData.ItemPosition.name);

        Item selectItem = Instantiate(GetItem(saveData.SelectItemData.ItemName),
            saveData.SelectItemData.ItemPosition.transform.position,
            Quaternion.identity, _container);

        selectItem.Init(saveData.SelectItemData.ItemPosition);
        _itemDragger.SetItem(selectItem, selectItem.ItemPosition);
        selectItem.gameObject.SetActive(true);
        _goldWallet.SetValue(saveData.GoldValue);

        foreach (var item in saveData.ItemDatasPrices)
        {
            _shopItems.SetPrice(item.ItemName, item.Price);
        }

        _shopItems.SetPricePossibilitie(saveData.PossibilitiesItemsData.PriceBulldozer,
            saveData.PossibilitiesItemsData.PriceReplace);
        
        
        if (saveData.StorageItemData.ItemPosition != null)
        {
            /*Debug.Log("Storage: ");
            Debug.Log("Storage: " + saveData.StorageItemData.ItemName);*/
            Item storageItem = Instantiate(GetItem(saveData.StorageItemData.ItemName), _container);
            storageItem.gameObject.SetActive(false);
            // Debug.Log("Storage Load: " + storageItem.ItemName);
            _storage.SetItem(storageItem);
        }
        else
        {
            // Debug.Log("clear");
            _storage.ClearItem();
        }
        /*_replaceCounter.SetValue(saveData.ReplaceCount);
        _bulldozerCounter.SetValue(saveData.BulldozerCount);
        _goldWallet.SetValue(saveData.GoldValue);
        _moveCounter.SetValue(saveData.MoveCount);
        _scoreCounter.SetValue(saveData.ScoreValue);*/

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
        Debug.Log("CurrentStepFromClear  " + _currentStep);
    }
}