using System;
using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Wallets;

public class MovesKeeper : MonoBehaviour
{
    [SerializeField] private ItemsStorage _itemsStorage;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Transform _container;
    [SerializeField] private Item[] _items;
    [SerializeField] private PossibilitiesCounter _replaceCounter;
    [SerializeField] private PossibilitiesCounter _bulldozerCounter;
    [SerializeField] private GoldWallet _goldWallet;
    [SerializeField]private MoveCounter _moveCounter;
    
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
        _currentStep = _savesHistory.Count - 1;
        StepChanged?.Invoke(_currentStep);
        Debug.Log("CurrentStep  Удаление " + _currentStep);
    }

    private void SaveHistory(SaveData saveData)
    {
        if (_currentStep >= _maxStepSaved)
        {
            RemoveStep();
        }

        _savesHistory.Add(saveData);
        _currentStep = _savesHistory.Count - 1;
        StepChanged?.Invoke(_currentStep);
        Debug.Log("CurrentStep   " + _currentStep);
    }

    private void LoadLastStep()
    {
    }

    public void CancelLastStep()
    {
        Debug.Log("CurrentStep   " + _currentStep);
        if (_currentStep <= 0)
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(StartCancelling());
    }

    private IEnumerator StartCancelling()
    {
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
        
        yield return null;
        ClearHistory();
        _itemsStorage.SaveChanges();
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
        _savesHistory.Clear();
        _currentStep = _savesHistory.Count - 1;
        StepChanged?.Invoke(_currentStep);
        Debug.Log("CurrentStep   Очистка" + _currentStep);
    }
}