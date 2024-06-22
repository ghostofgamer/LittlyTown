using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using MergeContent;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PositionMatcher : MonoBehaviour
{
    [SerializeField] private ItemPositionLooker _itemPositionLooker;
    [SerializeField] private Merger _merger;
    [SerializeField] private Spawner _spawner;
    [SerializeField] ItemDragger _itemDragger;

    private ItemPosition _currentItemPosition;
    private List<Item> _matchedItems = new List<Item>();
    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _positions = new List<ItemPosition>();
    private List<Item> _completeList = new List<Item>();

    private Item _currentItem;
    
    private Dictionary<ItemPosition, Coroutine> _mergingCoroutines = new Dictionary<ItemPosition, Coroutine>();
    
    public event Action NotMerged;

    public List<ItemPosition> Positions => _positions;

    private Coroutine _coroutine;
    private Coroutine _cyclicСoroutine;

    private void OnEnable()
    {
        /*_itemPositionLooker.PlaceLooking += OnSetPosition;
        _spawner.LooksNeighbors += OnSetPosition;*/
    }

    private void OnDisable()
    {
        /*_itemPositionLooker.PlaceLooking -= OnSetPosition;
        _spawner.LooksNeighbors -= OnSetPosition;*/
    }

    private void OnSetPosition(ItemPosition itemPosition, Item item)
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

        if (_cyclicСoroutine != null)
            StopCoroutine(_cyclicСoroutine);

        if (item.ItemName.Equals(Items.Crane))
        {
            Debug.Log("NEN Crane");
            _currentItem = null;
            foreach (var arroundPosition in _currentItemPosition.ItemPositions)
            {
                if (arroundPosition != null && arroundPosition.Item != null)
                {
                    _coroutine = StartCoroutine(LookPositions(arroundPosition, arroundPosition.Item));
                    break;
                }
            }
        }
        else
        {
            Debug.Log("lse");
            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _currentItem));
        }
        
        
        
        
        // _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _currentItem));
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

    public void StopMoveMatch()
    {
        foreach (var matchItem in _completeList)
            matchItem.GetComponent<ItemMoving>().StopMove();
    }

    public void LookAround(ItemPosition itemPosition)
    {
        // Debug.Log("Look " + itemPosition.name);
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
        
        
        
        /*if (_currentItem != null && _currentItem.ItemName.Equals(Items.Crane))
        {
            _currentItem = null;
            foreach (var arroundPosition in _currentItemPosition.ItemPositions)
            {
                if (arroundPosition != null && arroundPosition.Item != null)
                {
                    _currentItem = arroundPosition.Item;
                    SearchMatches(arroundPosition);
                    break;
                }
            }
        }
        else
        {
            SearchMatches(_currentItemPosition);
        }*/

        
        
        
        
        if (_positions.Count < 3)
        {
            NotMerged?.Invoke();
        }
        else
        {
            // _merger.Merge(_currentItemPosition,_positions);
        }
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
        
        /*if (currentPosition.Item != null)
        {
            if (_currentItem.ItemName.Equals(Items.Crane) || currentPosition.Item.ItemName.Equals(_currentItem.ItemName))
            {
                _matchedItems.Add(currentPosition.Item);
            }
            else if (_currentItem.ItemName != Items.Crane)
            {
                return;
            }
        }*/
        

        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
                continue;

            if (arroundPosition.Item != null && currentPosition.Item.ItemName.Equals(arroundPosition.Item.ItemName))
            {
                SearchMatches(arroundPosition);
            }
            
            /*if (arroundPosition.Item != null && (_currentItem.ItemName.Equals(Items.Crane) || currentPosition.Item.ItemName.Equals(arroundPosition.Item.ItemName)))
            {
                SearchMatches(arroundPosition);
            }*/
        }
    }

    private void ClearLists()
    {
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _positions.Clear();
    }
}