using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class LookMerger : MonoBehaviour
{
    [SerializeField] private TestMerger _testMerger;
    [SerializeField] private ItemPositionLooker _itemPositionLooker;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Merger _merger;

    private List<Item> _matchedItems = new List<Item>();
    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _positions = new List<ItemPosition>();
    private List<Item> _completeList = new List<Item>();
    private ItemPosition _currentItemPosition;
    private Item _currentItem;
    private Coroutine _coroutine;

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

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _currentItem));
    }

    public void LookAround(ItemPosition itemPosition, Item item)
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

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(LookMerge(_currentItemPosition, _currentItem));
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
            Item item = _matchedItems[0].NextItem;

            foreach (var matchItem in _matchedItems)
                _completeList.Add(matchItem);

            _matchedItems.Clear();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
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
            Debug.Log("Merege" + _matchedItems.Count);
            _merger.Merge(_currentItemPosition,_positions,_matchedItems);
        }
        else
        {
            Debug.Log("NotMerge" + _positions.Count);
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
                // SearchMatches(arroundPosition);
                ActivationLookPositions(arroundPosition, item);
            }
        }
    }

    public void StopMoveMatch()
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