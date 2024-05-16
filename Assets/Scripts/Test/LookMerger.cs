using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

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
        Debug.Log("CheckCoroutine " );
        SetValue(itemPosition, item);

        if (_currentItem.ItemName == Items.Crane)
        {
            _temporaryItems.Clear();
            _temporaryItems = _minIndexItemSelector.GetTemporaryItems(_currentItemPosition.ItemPositions);
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_currentItemPosition.ItemPositions);
            CheckStartCoroutine(_temporaryItem, itemPosition);
        }
        else
        {
            CheckStartCoroutine(_currentItem, itemPosition);
        }
    }

    private void CheckStartCoroutine(Item item, ItemPosition itemPosition)
    {
        if (!_isTryMerge)
            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
        else
            _coroutine = StartCoroutine(LookMerge(_currentItemPosition, item));
        /*else
        {
            /*if (_coroutines.ContainsKey(itemPosition))
            {
                Debug.Log("Coroutine with key " + itemPosition + " already exists!");
            }
            else
            {
                Debug.Log("Coroutine new ");
                Coroutine coroutine = StartCoroutine(LookMerge(_currentItemPosition, item));
                _coroutines.Add(itemPosition, coroutine);
            }#1#
            
            if (_coroutines.ContainsKey(item))
            {
                Debug.Log("Coroutine Faled " + itemPosition +"   "+ item.name);
            }
            else
            {
                Debug.Log("Coroutine new key " + item + " POS  " + itemPosition );
                Debug.Log("Position " + itemPosition.name + " Item  " + item );
                Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, item));
                _coroutines.Add(item, coroutine);
            }
        }*/
    }

    private void SetValue(ItemPosition itemPosition, Item item)
    {
        StopMoveMatch();

        if (_currentItemPosition != null)
            _currentItemPosition.ClearingPosition();

        Debug.Log("ваыываыа" +  itemPosition.name);
        
        /*if (itemPosition.IsBusy)
        {
            Debug.Log("ваыываыа");
             return;
        }*/
        
        if (!itemPosition.IsBusy && !item.IsLightHouse && itemPosition.IsWater)
            return;

        _currentItemPosition = itemPosition;
        Debug.Log("КАКАЯ Куррент позицияч " + _currentItemPosition);
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
        CheckMatchMerge();
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

    private void CheckMatchMerge()
    {
        Debug.Log("Совпадений " + _matchedItems.Count);
        
        if (_matchedItems.Count >= 2)
        {
            Debug.Log("////// Item Merge " + _currentItem);
            Debug.Log("////// Merge Position" + _currentItemPosition.name);
            _matchedItems.Add(_currentItem);
            _merger.Merge(_currentItemPosition, _positions, _matchedItems);
        }
        else if (_matchedItems.Count < 2 && _temporaryItems.Count > 1)
        {
            SaveNewTemporaryItem();
            _coroutine = StartCoroutine(LookMerge(_currentItemPosition, _temporaryItem));
        }
        else
        {
            StopAllCoroutines();
            NotMerged?.Invoke();
        }
    }

    private void ActivationLookPositions(ItemPosition currentPosition, Item item)
    {
        if (_checkedPositions.Contains(currentPosition))
        {
            return;
        }

        if (!currentPosition.IsSelected)
        {
            _checkedPositions.Add(currentPosition);
            _matchedItems.Add(currentPosition.Item);
            _positions.Add(currentPosition);
        }

        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
            {
                continue;
            }

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

    private void StopAllCoroutines()
    {
        foreach (var coroutine in _coroutines.Values)
        {
            StopCoroutine(coroutine);
        }
        
        _coroutines.Clear();
    }
}