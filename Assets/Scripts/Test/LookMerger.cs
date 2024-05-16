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
        SetValue(itemPosition, item);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_currentItem.ItemName == Items.Crane)
        {
            _temporaryItems.Clear();
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_currentItemPosition.ItemPositions);
            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _temporaryItem));
        }
        else
        {
            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _currentItem));
        }
    }

    public void LookAround(ItemPosition itemPosition, Item item)
    {
        SetValue(itemPosition, item);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_currentItem.ItemName == Items.Crane)
        {
            _temporaryItems.Clear();
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_currentItemPosition.ItemPositions);
            _coroutine = StartCoroutine(LookMerge(_currentItemPosition, _temporaryItem));
        }
        else
        {
            _coroutine = StartCoroutine(LookMerge(_currentItemPosition, _currentItem));
        }
    }

    private void SetValue(ItemPosition itemPosition, Item item)
    {
        StopMoveMatch();

        if (_currentItemPosition != null)
        {
            _currentItemPosition.ClearingPosition();
        }

        if (itemPosition.IsBusy)
            return;

        _currentItemPosition = itemPosition;
        _currentItemPosition.SetSelected();
        _currentItem = item;
        _completeList.Clear();
        ClearLists();
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
            _temporaryItems.Remove(_temporaryItem);
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_temporaryItems);

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

    private void CheckMatchMerge()
    {
        if (_matchedItems.Count >= 2)
        {
            _matchedItems.Add(_currentItem);
            _merger.Merge(_currentItemPosition, _positions, _matchedItems);
        }
        else
        {
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
}