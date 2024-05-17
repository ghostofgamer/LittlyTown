using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using UnityEngine.UIElements;

public class LookMerger : MonoBehaviour
{
    [SerializeField] private ItemPositionLooker _itemPositionLooker;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Merger _merger;
    [SerializeField] private MinIndexItemSelector _minIndexItemSelector;

    private ItemPosition _currentItemPosition;
    private Item _currentItem;
    private Coroutine _coroutine;
    private List<Item> _matchedItems = new List<Item>();
    private List<Item> _completeList = new List<Item>();
    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _positions = new List<ItemPosition>();

    private List<Item> _temporaryItems = new List<Item>();
    private Item _temporaryItem;
    private bool _isTryMerge;

    // private Dictionary<ItemPosition, Coroutine> _coroutines = new Dictionary<ItemPosition, Coroutine>();
    private Dictionary<Item, Coroutine> _coroutines = new Dictionary<Item, Coroutine>();
    private Dictionary<Item, List<Item>> _newMatchedItems = new Dictionary<Item, List<Item>>();
    private Dictionary<ItemPosition, ItemPosition> _targetPosition = new Dictionary<ItemPosition, ItemPosition>();

    public event Action NotMerged;

    private void OnEnable()
    {
        _itemPositionLooker.PlaceLooking += CheckMatches;
        _spawner.LooksNeighbors += CheckMatches;
    }

    private void OnDisable()
    {
        _itemPositionLooker.PlaceLooking -= CheckMatches;
        _spawner.LooksNeighbors -= CheckMatches;
    }

    private void CheckMatches(ItemPosition itemPosition, Item item)
    {
        _isTryMerge = false;
        CheckCoroutine(itemPosition, item);
    }

    public void LookAround(ItemPosition itemPosition, Item item)
    {
        _isTryMerge = true;
        CheckCoroutine(itemPosition, item);
    }

    private void CheckCoroutine(ItemPosition itemPosition, Item item)
    {
        // Debug.Log("CheckCoroutine " );
        SetValue(itemPosition, item);

        if (!_targetPosition.ContainsKey(itemPosition))
        {
            _targetPosition.Add(itemPosition, itemPosition);
            // Debug.Log("NEW KEY " + item.name);
        }

        _targetPosition[itemPosition].SetSelected();

        Debug.Log("ИМЯ " + itemPosition.name);

        if (_currentItem.ItemName == Items.Crane)
        {
            _temporaryItems.Clear();
            _temporaryItems = _minIndexItemSelector.GetTemporaryItems(_currentItemPosition.ItemPositions);
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_currentItemPosition.ItemPositions);
            // Debug.Log("isTryMerge " + _isTryMerge);
            CheckStartCoroutine(_temporaryItem, itemPosition);
        }
        else
        {
            // CheckStartCoroutine(_currentItem, itemPosition);
            CheckStartCoroutine(_currentItem, _targetPosition[itemPosition]);
        }
    }

    private void CheckStartCoroutine(Item item, ItemPosition itemPosition)
    {
        // Debug.Log("ПЕРЕДАЕМИ Сюда Item " + item.name);

        if (!_isTryMerge)
        {
            // Debug.Log("NOTMERGE");
            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
        }
        /*else
            _coroutine = StartCoroutine(LookMerge(_currentItemPosition, item));*/
        else
        {
            if (_coroutines.ContainsKey(item))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
                Debug.Log("Сколкь окорутин активных " + _coroutines.Count);
                Debug.Log("Coroutine Faled " + itemPosition + "   " + item.name);
            }
            else if (!_coroutines.ContainsKey(item))
            {
                if (!_newMatchedItems.ContainsKey(item))
                {
                    _newMatchedItems.Add(item, new List<Item>());
                    // Debug.Log("NEW KEY " + item.name);
                }

                Debug.Log("Coroutine NEWWW" + item + " POS  " + itemPosition);

                Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, item));
                _coroutines.Add(item, coroutine);
                // Debug.Log("Сколкь окорутин активных " + _coroutines.Count);
            }
            else
            {
                if (!_newMatchedItems.ContainsKey(item))
                {
                    _newMatchedItems.Add(item, new List<Item>());
                    // Debug.Log("NEW KEY " + item.name);
                }

                Debug.Log("Coroutine new key " + item + " POS  " + itemPosition);

                Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, item));
                _coroutines.Add(item, coroutine);
                // Debug.Log("Сколкь окорутин активных " + _coroutines.Count);
            }

            Debug.Log("Сколкь окорутин активных " + _coroutines.Count);
        }
    }

    private void SetValue(ItemPosition itemPosition, Item item)
    {
        StopMoveMatch();

        if (_currentItemPosition != null && !_currentItemPosition.IsReplaceSelected)
        {
            Debug.Log("SEKK " + _currentItemPosition.name);
            _currentItemPosition.ClearingPosition();
        }


        // Debug.Log("ваыываыа" +  itemPosition.name);

        /*if (itemPosition.IsBusy)
        {
            Debug.Log("ваыываыа");
             return;
        }*/

        if (!itemPosition.IsBusy && !item.IsLightHouse && itemPosition.IsWater)
            return;

        _currentItemPosition = itemPosition;
        // Debug.Log("КАКАЯ Куррент позицияч " + _currentItemPosition);
        _currentItemPosition.SetSelected();

        _currentItem = item;
        _completeList.Clear();
        ClearLists();

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator LookPositions(ItemPosition itemPosition, Item item)
    {
        yield return new WaitForSeconds(0.15f);
        ActivationLookPositions(itemPosition, item);
        CheckNextLevel();
    }

    private IEnumerator LookMerge(ItemPosition itemPosition, Item item)
    {
        Debug.Log("Coroutine START 1  " + itemPosition.name);
        yield return new WaitForSeconds(0.15f);
        Debug.Log("Coroutine START 0  " + itemPosition.name);
        ActivationLookPositions(itemPosition, item);
        // yield return null;
        // yield return new WaitForEndOfFrame();
        Debug.Log("После Coroutine   " + itemPosition.name);
        CheckMatchMerge(itemPosition, item);
    }

    private void CheckNextLevel()
    {
        if (_matchedItems.Count >= 2)
        {
            if (_currentItem.ItemName == Items.Crane)
                _temporaryItems.Clear();

            Item item = _matchedItems[0].NextItem;

            foreach (var matchItem in _matchedItems)
                _completeList.Add(matchItem);

            _matchedItems.Clear();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
        }

        else if (_matchedItems.Count < 2 && _temporaryItems.Count > 1)
        {
            SaveNewTemporaryItem();
            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _temporaryItem));
        }

        else
        {
            if (_completeList.Count < 2)
                return;

            foreach (var itemMatch in _completeList)
                itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
        }
    }

    private void SaveNewTemporaryItem()
    {
        _matchedItems.Clear();
        _temporaryItems.Remove(_temporaryItem);
        _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_temporaryItems);
    }

    private void CheckMatchMerge(ItemPosition itemPosition, Item item)
    {
        // Debug.Log("ПЕРЕДМЕРДЖЕМ  " + item);
        // Debug.Log("Совпадений " + _matchedItems.Count);
        // Debug.Log("СОВА " + _newMatchedItems[item].Count + "   KEY   " + item.name);

        if (_newMatchedItems[item].Count >= 2)
        {
            foreach (var _newMatchedItems in _newMatchedItems[item])
            {
                Debug.Log("ПЕРЕДАЧА В МЕРДЖ ТУТ " + _newMatchedItems.name + "///" + _newMatchedItems.ItemPosition);
            }

            Debug.Log("//////// ");
            // Debug.Log("////// Item Merge " + _currentItem);
            // Debug.Log("////// Merge Position" + _currentItemPosition.name);
            _newMatchedItems[item].Add(item);
            _merger.Merge(itemPosition, _positions, _newMatchedItems[item], item);
            StopCoroutine(_coroutines[item]);
            _coroutines.Remove(item);
        }
        /*if (_matchedItems.Count >= 2)
        {
            Debug.Log("////// Item Merge " + _currentItem);
            Debug.Log("////// Merge Position" + _currentItemPosition.name);
            _matchedItems.Add(_currentItem);
            _merger.Merge(_currentItemPosition, _positions, _matchedItems);
        }*/
        else if (_matchedItems.Count < 2 && _temporaryItems.Count > 1)
        {
            Debug.Log("Здесь бываешь? " + item);
            SaveNewTemporaryItem();
            _coroutine = StartCoroutine(LookMerge(itemPosition, item));
        }
        else
        {
            // Debug.Log("ITEM " + item);

            /*if (_isTryMerge)
            {
                Stop();
                // StopAllCoroutines(item);
            }*/
            Debug.Log("itemPosition" + itemPosition+ itemPosition.IsReplaceSelected);

            if (itemPosition.IsReplaceSelected)
            {
                Debug.Log("itemPosition   REPLACED" + itemPosition+ itemPosition.IsReplaceSelected);
                itemPosition.DeactivationSelected();
                itemPosition.ReplaceSelectedDeactivate();
            }

            

            NotMerged?.Invoke();

            if (_coroutines.ContainsKey(item))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
                Debug.Log("Сколкь окорутин активных ВКОНЦЕ " + _coroutines.Count);
            }
        }
    }

    private void ActivationLookPositions(ItemPosition currentPosition, Item item)
    {
        Debug.Log("Проверяем позицию " + currentPosition.name);

        if (_checkedPositions.Contains(currentPosition))
        {
            Debug.Log("БЫЛ Return" + currentPosition.name);
            return;
        }

        if (!currentPosition.IsSelected)
        {
            Debug.Log("NOT SELECTED " + currentPosition.name);
            _checkedPositions.Add(currentPosition);
            _matchedItems.Add(currentPosition.Item);
            _positions.Add(currentPosition);

            if (_isTryMerge)
                _newMatchedItems[item].Add(currentPosition.Item);
            // Debug.Log("Позиция добавления в список " + currentPosition + " ITEM " + item);
        }

        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
            {
                continue;
            }

            Debug.Log("FOREASCH " + arroundPosition.name);

            if (arroundPosition.Item != null && arroundPosition.Item.ItemName.Equals(item.ItemName))
            {
                ActivationLookPositions(arroundPosition, item);
            }
        }
    }

    private void StopMoveMatch()
    {
        foreach (var matchItem in _completeList)
            matchItem.GetComponent<ItemMoving>().StopCoroutine();
    }

    private void ClearLists()
    {
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _positions.Clear();
    }

    private void StopAllCoroutines(Item item)
    {
        // Debug.Log("STOP " + item);

        StopCoroutine(_coroutines[item]);

        _coroutines.Remove(item);

        _newMatchedItems[item].Clear();
        /*foreach (var coroutine in _coroutines.Values)
        {
            StopCoroutine(coroutine);
        }
        
        _coroutines.Clear();*/
    }

    private void Stop()
    {
        // Debug.Log("Сколько корутин " + _coroutines.Count);

        List<Item> itemsToRemove = new List<Item>();

        foreach (var pair in _coroutines)
        {
            Item item = pair.Key;
            Coroutine routine = pair.Value;

            // Останавливаем корутину
            StopCoroutine(routine);

            // Добавляем ключ в список для удаления
            itemsToRemove.Add(item);
        }

// Удаляем элементы из словаря
        foreach (var item in itemsToRemove)
        {
            _coroutines.Remove(item);
        }

// Очищаем словарь (опционально)
        _coroutines.Clear();
        _newMatchedItems.Clear();
    }
}