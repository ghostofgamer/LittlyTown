using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMatcher : MonoBehaviour
{
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

    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Merge _merge;

    private ItemPosition _currentItemPosition;
    private List<Item> _matchedItems = new List<Item>();

    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _position = new List<ItemPosition>();

    private Item _currentItem;

    public event Action Merging;

    public List<Item> MatchedItems => _matchedItems;
    public List<ItemPosition> Positions => _position;

    private Coroutine _coroutine;

    public void SetPosition(ItemPosition itemPosition)
    {
        StopMoveMatch();
        _currentItemPosition = itemPosition;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Looks());
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

    private void StopMoveMatch()
    {
        if (_matchedItems.Count >= 3)
        {
            foreach (var matchItem in _matchedItems)
            {
                matchItem.GetComponent<ItemMoving>().StopCoroutine();
            }
        }
    }

    private IEnumerator Looks()
    {
        yield return new WaitForSeconds(0.15f);
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _position.Clear();
        TestLookAround(_currentItemPosition);
        Show();
    }


    private void Show()
    {
        if (_position.Count < 3)
            return;

        
        
        foreach (var itemMatch in _matchedItems)
        {
            itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
        }
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
}