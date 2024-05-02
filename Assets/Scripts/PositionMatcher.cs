using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMatcher : MonoBehaviour
{
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Merge _merge;

    private ItemPosition _currentItemPosition;
    private List<Item> _matchedItems = new List<Item>();

    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _position = new List<ItemPosition>();

    private List<Item> _completeList = new List<Item>();

    private Item _currentItem;

    public event Action Merging;

    public List<Item> MatchedItems => _matchedItems;
    public List<ItemPosition> Positions => _position;

    private Coroutine _coroutine;

    private int _matchItemCount = 0;

    public int matchitemCount => _matchItemCount;

    private void OnEnable()
    {
        _itemDragger.PlaceLooking += SetPosition;
        // _itemDragger.PlaceChanged += Testmerge;
    }

    private void OnDisable()
    {
        _itemDragger.PlaceLooking -= SetPosition;
        // _itemDragger.PlaceChanged += Testmerge;
    }

    public void SetPosition(ItemPosition itemPosition, Item item)
    {
        _matchItemCount = 0;
        StopMoveMatch();
        _currentItemPosition = itemPosition;
        _currentItem = item;
        _completeList.Clear();
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _position.Clear();
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Looks(_currentItemPosition,_currentItem));
    }

    /*private IEnumerator Looks()
    {
        yield return new WaitForSeconds(0.15f);
        /*_completeList.Clear();
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _position.Clear();#1#
        TestLookAroundCycleMover(_currentItemPosition, _currentItem);
        Checker();
        // Show();
        // TestShow();
    }*/

    private IEnumerator Looks(ItemPosition itemPosition,Item item )
    {
        yield return new WaitForSeconds(0.15f);
        /*_completeList.Clear();
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _position.Clear();*/
        TestLookAroundCycleMover(itemPosition, item);
        Checker();
        // Show();
        // TestShow();
    }
    
    private void Show()
    {
        Debug.Log(" SHOW Сколько " + _position.Count);

        // foreach (var position in _position)
        // {
        //     Debug.Log("имя позиций " + position.name);
        // }

        if (_position.Count < 2)
            return;

        foreach (var itemMatch in _matchedItems)
        {
            itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
        }
    }

    private void Checker()
    {
        Debug.Log("CheckerCount " + _matchedItems.Count);
        Debug.Log("ELSE " + _completeList.Count);

        if (_matchedItems.Count >= 2)
        {
            Item item = _matchedItems[0].NextItem;
            foreach (var matchItem in _matchedItems)
            {
                _completeList.Add(matchItem);
            }
            _matchedItems.Clear();
            Debug.Log("Start " + _completeList.Count);
            // Debug.Log("NextItem" + _matchedItems[0].NextItem);
            // TestLookAroundCycleMover(_currentItemPosition, _matchedItems[0].NextItem);
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Looks(_currentItemPosition, item));
        }

        else
        {
            Debug.Log(" SHOW Сколько " + _completeList.Count);

            /*
            if (_position.Count < 2)
                return;
                */
            if (_completeList.Count < 2)
                return;

            Debug.Log(" SHOW После уже ");

            foreach (var itemMatch in _completeList)
            {
                itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
            }

            /*Debug.Log(" SHOW Сколько " + _position.Count);
            
            if (_position.Count < 2)
                return;

            foreach (var itemMatch in _matchedItems)
            {
                itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
            }*/
        }
    }

    private void TestLookAroundCycleMover(ItemPosition currentPosition, Item item)
    {
        Debug.Log(" New Cycle " + item.name);

        if (_checkedPositions.Contains(currentPosition))
        {
            return;
        }

        Debug.Log(" New Cycle 1" + item.name);
        if (!currentPosition.IsSelected)
        {
            _checkedPositions.Add(currentPosition);
            _matchedItems.Add(currentPosition.Item);
            _position.Add(currentPosition);
        }

        Debug.Log(" New Cycle 3" + item.name);
        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
                continue;

            Debug.Log(" New Cycle 4" + item.name);

            if (arroundPosition.Item != null && arroundPosition.Item.ItemName.Equals(item.ItemName))
            {
                Debug.Log(" New Cycle 5" + item.name);
                // _matchedItems.Add(arroundPosition.Item);
                // _matchItemCount++;
                // _position.Add(arroundPosition);
                TestLookAround(arroundPosition);
            }
        }
    }

    private void StopMoveMatch()
    {
        /*if (_matchedItems.Count >= 2)
        {
            foreach (var matchItem in _matchedItems)
            {
                matchItem.GetComponent<ItemMoving>().StopCoroutine();
            }
        }*/
        
        foreach (var matchItem in _completeList)
        {
            matchItem.GetComponent<ItemMoving>().StopCoroutine();
        }
    }

    public void TryMerge(ItemPosition itemPosition)
    {
        StopMoveMatch();
        _currentItemPosition = itemPosition;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(TestMergeCall());
    }

    private IEnumerator TestMergeCall()
    {
        yield return new WaitForSeconds(0.15f);
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _position.Clear();
        TestLookAround(_currentItemPosition);


        _merge.TestMatchMerge(_currentItemPosition);
        // Show();
    }


    private void TestLookAround(ItemPosition currentPosition)
    {
        _currentItem = _currentItemPosition.Item;

        if (_checkedPositions.Contains(currentPosition))
        {
            return;
        }

        _checkedPositions.Add(currentPosition);
        _position.Add(currentPosition);

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
                TestLookAround(arroundPosition);
            }
        }
    }


    /*private void TestLookAround(ItemPosition currentPosition, Item item)
    {
        // _currentItem = _currentItemPosition.Item;

        if (_checkedPositions.Contains(currentPosition))
        {
            return;
        }

        _checkedPositions.Add(currentPosition);
        _position.Add(currentPosition);

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

            if (arroundPosition.Item != null && item.ItemName.Equals(arroundPosition.Item.ItemName))
            {
                TestLookAround(arroundPosition, item);
            }
        }

        // TestShow();
    }*/

    /*private void TestShow()
    {
        if (_position.Count < 3)
            return;

        if (_matchedItems.Count >= 3)
        {
            Debug.Log("3 в ряд!!!");
            TestLookAround(_currentItemPosition,_currentItemPosition.Item.NextItem);
        }

        foreach (var itemMatch in _matchedItems)
        {
            itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
        }
    }*/
}