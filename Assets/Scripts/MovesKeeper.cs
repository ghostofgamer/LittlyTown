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

    private List<SaveData> _savesHistory = new List<SaveData>();
    private int _maxStepSaved = 3;
    private int _currentStep = -1;
    private Coroutine _coroutine;

    public int CurrentStep => _currentStep;

    public event Action<int> StepChanged;

    private void OnEnable()
    {
        _itemsStorage.SaveCompleted += SaveHistory;
    }

    private void OnDisable()
    {
        _itemsStorage.SaveCompleted -= SaveHistory;
    }

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

    private void SaveHistory(SaveData saveData)
    {
        if (_savesHistory.Count >= _maxStepSaved)
        {
            _savesHistory.RemoveAt(0);
        }

        _savesHistory.Add(saveData);
        _currentStep = _savesHistory.Count - 1;
        StepChanged?.Invoke(_currentStep);
    }


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
        SaveData newSaveData = _savesHistory[_currentStep];
        Debug.Log("ITEMDROPPP " + newSaveData.ItemDropData);
        _dropGenerator.SetItem(newSaveData.ItemDropData);

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
            Quaternion.identity, _container);*/

        Item selectItem = Instantiate(GetItem(_itemsStorage.SelectObject.ItemName),
            _itemsStorage.SelectObject.ItemPosition.transform.position,
            Quaternion.identity, _container);

        _itemDragger.SetItem(selectItem, _itemsStorage.SelectObject.ItemPosition);
        selectItem.gameObject.SetActive(true);
        // Debug.Log("ШАГ НАЗАД " + selectItem.name);

        /*Item temporaryItem = Instantiate(GetItem(saveData.TemporaryItemDragger.ItemName),saveData.TemporaryItemDragger.ItemPosition.transform.position,
            Quaternion.identity, _container);
        _itemDragger.SetTemporaryObject(temporaryItem);*/


        yield return null;
        _roadGenerator.OnGeneration();
        ClearHistory();
        yield return new WaitForSeconds(0.1f);
        // _itemsStorage.SaveChanges();
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
}