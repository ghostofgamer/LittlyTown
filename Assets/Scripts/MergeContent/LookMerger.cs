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
        // StopMoveMatch();
        if (itemPosition.IsWater || itemPosition.IsBusy)
        {
         
            StopMoveMatch();
            _completeList.Clear();
            return;
        }
            
   
        _isTryMerge = false;
        _targetPosition.Clear();
        CheckCoroutine(itemPosition, item);
    }

    public void LookAround(ItemPosition itemPosition, Item item)
    {
        // Debug.Log("Look Around " + itemPosition.name + " / " + item.name);
        _isTryMerge = true;
        CheckCoroutine(itemPosition, item);
    }

    private void CheckCoroutine(ItemPosition itemPosition, Item item)
    {
        SetValue(itemPosition, item);

        if (itemPosition.IsWater)
            return;
        
        if (!_targetPosition.ContainsKey(itemPosition))
        {
            _targetPosition.Add(itemPosition, itemPosition);
            // Debug.Log("Колличество TArgetPosition " + _targetPosition.Count);
        }

        _targetPosition[itemPosition].SetSelected();


        if (_currentItem.ItemName == Items.Crane)
        {
            _temporaryItems.Clear();
            _temporaryItems = _minIndexItemSelector.GetTemporaryItems(_currentItemPosition.ItemPositions);
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_currentItemPosition.ItemPositions);

            if (_temporaryItem != null)
            {
                // Debug.Log("Не NULL Crane " + _temporaryItem.ItemName);
                CheckStartCoroutine(_temporaryItem, itemPosition);
            }

            else if (_isTryMerge && _temporaryItem == null)
            {
                // Debug.Log("убирайся ");
                _currentItem.GetComponent<CraneDestroyer>().Destroy();
            }

            /*else
            {
                Debug.Log("убирайся ");
                                         _currentItem.GetComponent<CraneDestroyer>().Destroy();
            }*/
        }
        else
        {
            // CheckStartCoroutine(_currentItem, itemPosition);
            CheckStartCoroutine(_currentItem, _targetPosition[itemPosition]);
        }
    }

    private void CheckStartCoroutine(Item item, ItemPosition itemPosition)
    {
        if (!_isTryMerge)
        {
            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
        }
        /*else
            _coroutine = StartCoroutine(LookMerge(_currentItemPosition, item));*/
        else
        {
            // Debug.Log("Ошибка корутины " + _coroutines.Count);
            // Debug.Log("Ошибка  имени " + item.name);

            if (_coroutines.ContainsKey(item))
            {
                // Debug.Log("StopCoroutine");
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
            }
            else if (!_coroutines.ContainsKey(item))
            {
                if (!_newMatchedItems.ContainsKey(item))
                {
                    _newMatchedItems.Add(item, new List<Item>());
                }

                // Debug.Log("НОВАЯ КОРУТИНА 1 " + item.name + " POS  " + itemPosition.name);
                Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, item));
                _coroutines.Add(item, coroutine);
            }
            else
            {
                if (!_newMatchedItems.ContainsKey(item))
                {
                    _newMatchedItems.Add(item, new List<Item>());
                }

                // Debug.Log("НОВАЯ КОРУТИНА 3 " + item.name + " POS  " + itemPosition.name);
                Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, item));
                _coroutines.Add(item, coroutine);
            }

            // Debug.Log("Сколько корутин активных на старте " + _coroutines.Count);
        }
    }

    private void SetValue(ItemPosition itemPosition, Item item)
    {
        StopMoveMatch();

        if (_currentItemPosition != null && !_currentItemPosition.IsReplaceSelected)
        {
            // Debug.Log("SEKK " + _currentItemPosition.name);
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
        yield return new WaitForSeconds(0.15f);
        ActivationLookPositions(itemPosition, item);
        // Debug.Log("Mergeeeeee");
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

            if (_targetPosition.ContainsKey(_currentItemPosition))
            {
                _targetPosition.Remove(_currentItemPosition);
                // Debug.Log("Колличество TARGET---MORETARGETAMOUNT = " + _targetPosition.Count);
            }


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

            if (_targetPosition.ContainsKey(_currentItemPosition))
            {
                _targetPosition.Remove(_currentItemPosition);
                // Debug.Log("Колличество TARGET---ELSE = " + _targetPosition.Count);
            }

            foreach (var itemMatch in _completeList)
                itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
            
            Debug.Log("ТУТ");
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
        // Debug.Log("temporary колличество  " + _temporaryItems.Count);
        // Debug.Log(" колличество  " + _newMatchedItems[item].Count);

        if (_newMatchedItems[item].Count >= 2)
        {
            // Debug.Log("1");
            foreach (var _newMatchedItems in _newMatchedItems[item])
            {
                // Debug.Log("ПЕРЕДАЧА В МЕРДЖ ТУТ " + _newMatchedItems.name + "///" + _newMatchedItems.ItemPosition);
            }

            _newMatchedItems[item].Add(item);
            _merger.Merge(itemPosition, _positions, _newMatchedItems[item], item);

            if (_coroutines.ContainsKey(item))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
                // Debug.Log("Колличество корутин MERGE!!! => " + _coroutines.Count);
            }

            if (_targetPosition.ContainsKey(itemPosition))
            {
                _targetPosition.Remove(itemPosition);
                // Debug.Log("Колличество TARGET---POSITION MERGE = " + _targetPosition.Count);
            }

            if (_temporaryItems.Count > 0)
            {
                _temporaryItems.Clear();
            }

            if (_newMatchedItems.ContainsKey(item))
            {
                _newMatchedItems[item].Clear();
                _newMatchedItems.Remove(item);
            }
        }
        else if (_newMatchedItems[item].Count < 2 && _temporaryItems.Count > 1)
        {
            // Debug.Log("3");


            if (_coroutines.ContainsKey(_temporaryItem))
            {
                // Debug.Log("стоп корутайн кран 1 " + _temporaryItem);
                StopCoroutine(_coroutines[_temporaryItem]);
                _coroutines.Remove(_temporaryItem);
            }

            if (_newMatchedItems.ContainsKey(item))
            {
                // Debug.Log("стоп newMatchitems " + item);
                _newMatchedItems[item].Clear();
                _newMatchedItems.Remove(item);
            }

            SaveNewTemporaryItem();
            // Debug.Log("СтопимКранМЕньше " + _temporaryItem.name);

            if (_coroutines.ContainsKey(_temporaryItem))
            {
                // Debug.Log("стоп корутайн кран 3 " + item);
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
            }

            // Debug.Log("Новая корутина крана" + _temporaryItem);
            Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, _temporaryItem));
            _coroutines.Add(_temporaryItem, coroutine);
            // _coroutine = StartCoroutine(LookMerge(itemPosition, _temporaryItem));
        }
        /*else if (_matchedItems.Count < 2 && _temporaryItems.Count > 1)
        {
            
            Debug.Log("ЗАБЕГАЕТ ");
            SaveNewTemporaryItem();
            _coroutine = StartCoroutine(LookMerge(itemPosition, _temporaryItem));
        }*/
        else
        {
            // Debug.Log("5");
            if (itemPosition.IsReplaceSelected)
            {
                itemPosition.DeactivationSelected();
                itemPosition.ReplaceSelectedDeactivate();
            }

            if (_currentItem.ItemName == Items.Crane)
            {
                // Debug.Log("Да это кран Merge");
                // _currentItem.gameObject.SetActive(false);
                _currentItem.GetComponent<CraneDestroyer>().Destroy();
            }


            if (_newMatchedItems.ContainsKey(item))
            {
                // Debug.Log("Удаляешь тут список?  ");
                _newMatchedItems[item].Clear();
                _newMatchedItems.Remove(item);
            }

            NotMerged?.Invoke();

            if (_targetPosition.ContainsKey(itemPosition))
            {
                _targetPosition.Remove(itemPosition);
                // Debug.Log("Колличество TARGET---POSITION NOTMERGE = " + _targetPosition.Count);
            }

            // Debug.Log("ПЕРЕВ ВЫКЛЮЧЕНИЕМ КОУРТИНЫ " + item.name);

            if (_coroutines.ContainsKey(item))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
                // Debug.Log("Колличество корутин NOTMERGE = " + _coroutines.Count);
            }
        }

        // Debug.Log("совпадений колличество " + _newMatchedItems[item].Count);
    }

    private void ActivationLookPositions(ItemPosition currentPosition, Item item)
    {
        if (!_newMatchedItems.ContainsKey(item))
        {
            _newMatchedItems.Add(item, new List<Item>());
        }

        if (_checkedPositions.Contains(currentPosition))
        {
            return;
        }

        if (!currentPosition.IsSelected)
        {
            _checkedPositions.Add(currentPosition);
            _matchedItems.Add(currentPosition.Item);
            _positions.Add(currentPosition);

            if (_isTryMerge)
            {
                // Debug.Log("И что тут за item " + item );
                _newMatchedItems[item].Add(currentPosition.Item);
            }
        }

        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
            {
                continue;
            }

            if (arroundPosition.Item != null && arroundPosition.Item.NextItem != null &&
                arroundPosition.Item.ItemName.Equals(item.ItemName))
            {
                ActivationLookPositions(arroundPosition, item);
            }
        }
    }

    public void StopMoveMatch()
    {
        foreach (var matchItem in _completeList)
        {
            matchItem.GetComponent<ItemMoving>().StopMove();
// Debug.Log("СТОП");
            /*if (matchItem.ItemPosition != null)
                matchItem.transform.position = matchItem.ItemPosition.transform.position;*/
        }
    }

    private void ClearLists()
    {
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _positions.Clear();
    }

    private void StopAllCoroutines(Item item)
    {
        StopCoroutine(_coroutines[item]);
        _coroutines.Remove(item);
        _newMatchedItems[item].Clear();
    }

    private void Stop()
    {
        List<Item> itemsToRemove = new List<Item>();

        foreach (var pair in _coroutines)
        {
            Item item = pair.Key;
            Coroutine routine = pair.Value;

            StopCoroutine(routine);
            itemsToRemove.Add(item);
        }

        foreach (var item in itemsToRemove)
        {
            _coroutines.Remove(item);
        }

        _coroutines.Clear();
        _newMatchedItems.Clear();
    }
}