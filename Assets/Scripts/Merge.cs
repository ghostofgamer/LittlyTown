using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Merge : MonoBehaviour
{
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private PositionMatcher _positionMatcher;

    private ItemPosition _currentItemPosition;
    private List<Item> _matchedItems = new List<Item>();

    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _position = new List<ItemPosition>();

    private Item _currentItem;
    private List<Item> _matchedList = new List<Item>();
    private List<ItemPosition> _matchPos = new List<ItemPosition>();

    public event Action Merging;
    public event Action NotMerging;

    private void OnEnable()
    {
        // _itemDragger.PlaceLooking += SetPosition;
        // _itemDragger.PlaceChanged += Testmerge;
    }

    private void OnDisable()
    {
        // _itemDragger.PlaceLooking -= SetPosition;
        // _itemDragger.PlaceChanged -= Testmerge;
    }

    private void Start()
    {
    }

    public void TestMatchMerge(ItemPosition currentPosition)
    {
        // Debug.Log("!!!!!! " + _positionMatcher.MatchedItems.Count);
        
        if (_positionMatcher.Positions.Count < 3)
        {
            NotMerging?.Invoke();
            return;
        }

        _matchPos = _positionMatcher.Positions;
        _currentItem = currentPosition.Item;
        // _matchedList = _positionMatcher.MatchedItems;
        
        /*foreach (var itemPosition in _matchPos)
        {
            itemPosition.Item.gameObject.SetActive(false);
            itemPosition.Item.Deactivation();
            // itemPosition.Item.GetComponent<ItemMoving>().Move(currentPosition.transform.position);
            // itemPosition.Item.GetOutPosition();
            itemPosition.ClearingItem();
        }*/

        foreach (var itemPosition in _matchPos)
        {
            itemPosition.Item.Deactivation();
            itemPosition.Item.GetComponent<ItemMoving>().Move(currentPosition.transform.position);
        }
        
        StartCoroutine(CorutinaMoveMerge(currentPosition));
        
       
        /*Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        item.Activation();

        _positionMatcher.TryMerge(currentPosition);
        // SetPosition(_currentItemPosition);
        Merging?.Invoke();*/
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

    /*public void TestMatchMerge(ItemPosition currentPosition)
    {

        if (_positionMatcher.Positions.Count < 3)
        {
            NotMerging?.Invoke();
            return;
        }
        
        _matchPos = _positionMatcher.Positions;
        _currentItem = currentPosition.Item;
        // _matchedList = _positionMatcher.MatchedItems;
        
        foreach (var itemPosition in _matchPos)
        {
            // itemPosition.Item.gameObject.SetActive(false);
            itemPosition.Item.GetComponent<ItemMoving>().Move(currentPosition.transform.position);
            itemPosition.Item.GetOutPosition();
            // itemPosition.ClearingItem();
        }

        /*foreach (var itemPosition in _matchPos)
        {
            itemPosition.Item.GetComponent<ItemMoving>().Move(currentPosition.transform.position);
        }
        
        StartCoroutine(CorutinaMoveMerge(currentPosition));#1#
        
Debug.Log("Создаем");
        Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        item.Activation();

        _positionMatcher.TryMerge(currentPosition);
        // SetPosition(_currentItemPosition);
        Merging?.Invoke();
    }*/

    private IEnumerator CorutinaMoveMerge(ItemPosition currentPosition)
    {
        yield return new WaitForSeconds(0.35f);

        foreach (var itemmanch in _matchedList)
        {
            itemmanch.gameObject.SetActive(false);
        }

        foreach (var itemPosition in _matchPos)
        {
            // itemPosition.Item.gameObject.SetActive(false);
            itemPosition.ClearingItem();
        }

        Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        // Debug.Log("Новый " + item.name);
        item.Activation();
        _positionMatcher.TryMerge(currentPosition);
// SetPosition(_currentItemPosition);
        yield return new WaitForSeconds(0.1f);
        Merging?.Invoke();
    }
}