using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PositionMatcher : MonoBehaviour
{
    [SerializeField] private ItemPositionLooker _itemPositionLooker;
    [SerializeField] private Merger _merger;
    [SerializeField]private Spawner _spawner;

    private ItemPosition _currentItemPosition;
    private List<Item> _matchedItems = new List<Item>();
    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _positions = new List<ItemPosition>();
    private List<Item> _completeList = new List<Item>();

    private Item _currentItem;

    public event Action NotMerged;

    public List<ItemPosition> Positions => _positions;

    private Coroutine _coroutine;


    private void OnEnable()
    {
        _itemPositionLooker.PlaceLooking += OnSetPosition;
        _spawner.LooksNeighbors += OnSetPosition;
    }

    private void OnDisable()
    {
        _itemPositionLooker.PlaceLooking -= OnSetPosition;
        _spawner.LooksNeighbors -= OnSetPosition;
    }

    private void OnSetPosition(ItemPosition itemPosition, Item item)
    {
        StopMoveMatch();
        
        if (_currentItemPosition != null)
        {
            _currentItemPosition.ClearingPosition();
        }
        
        _currentItemPosition = itemPosition;
        _currentItemPosition.SetSelected();
        _currentItem = item;
        _completeList.Clear();
        ClearLists();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _currentItem));
    }

    private IEnumerator LookPositions(ItemPosition itemPosition, Item item)
    {
        yield return new WaitForSeconds(0.15f);
        ActivationLookPositions(itemPosition, item);
        CheckNextLevel();
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

    private void StopMoveMatch()
    {
        foreach (var matchItem in _completeList)
            matchItem.GetComponent<ItemMoving>().StopCoroutine();
    }

    public void LookAround(ItemPosition itemPosition)
    {
        StopMoveMatch();
        _currentItemPosition = itemPosition;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SearchMatchAround());
    }

    private IEnumerator SearchMatchAround()
    {
        yield return new WaitForSeconds(0.15f);
        ClearLists();
        SearchMatches(_currentItemPosition);
        // Debug.Log("LookAround " + _positions.Count);
        if (_positions.Count < 3)
            NotMerged?.Invoke();

        else
            _merger.Merge(_currentItemPosition);
    }


    private void SearchMatches(ItemPosition currentPosition)
    {
        _currentItem = _currentItemPosition.Item;

        if (_checkedPositions.Contains(currentPosition))
        {
            return;
        }

        _checkedPositions.Add(currentPosition);
        _positions.Add(currentPosition);

        if (currentPosition.Item == null)
        {
            return;
        }

        if (currentPosition.Item != null)
        {
            _matchedItems.Add(currentPosition.Item);
        }

        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
                continue;

            if (arroundPosition.Item != null && currentPosition.Item.ItemName.Equals(arroundPosition.Item.ItemName))
            {
                SearchMatches(arroundPosition);
            }
        }
    }

    private void ClearLists()
    {
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _positions.Clear();
    }
}