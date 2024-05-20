using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Wallets;

public class Merger : MonoBehaviour
{
    [SerializeField] private PositionMatcher _positionMatcher;
    [SerializeField] private LookMerger _lookMerger;
    [SerializeField] private GoldWallet _goldWallet;

    private Item _currentItem;
    private List<ItemPosition> _matchPositions = new List<ItemPosition>();
    private List<Item> _matchItems = new List<Item>();

    private Dictionary<ItemPosition, List<ItemPosition>> _newMatchedPositions =
        new Dictionary<ItemPosition, List<ItemPosition>>();

    private Dictionary<ItemPosition, List<Item>> _newMatchedItems =
        new Dictionary<ItemPosition, List<Item>>();

    private Dictionary<ItemPosition, Item> _newItem =
        new Dictionary<ItemPosition, Item>();
    
    private Dictionary<Item, Item> _targetItem =
        new Dictionary<Item, Item>();
    
    private Dictionary<ItemPosition, Coroutine> _coroutines = new Dictionary<ItemPosition, Coroutine>();


    public event Action<int, int, ItemPosition> Merged;

    public event Action Mergered;

    public event Action<Item> ItemMergered;

    public void Merge(ItemPosition currentPosition, List<ItemPosition> matchPositions, List<Item> matchItems,Item targetItem)
    {
        if (!_newMatchedItems.ContainsKey(currentPosition))
        {
            _newMatchedItems.Add(currentPosition, new List<Item>());
            // Debug.Log("NEW");
        }

        Item newItem = currentPosition.Item;
        if (!_newItem.ContainsKey(currentPosition))
        {
            // _newItem.Add(currentPosition, newItem);
            _newItem.Add(currentPosition, targetItem);
            // Debug.Log("NEW");
        }
        
        if (!_targetItem.ContainsKey(targetItem))
        {
            // _newItem.Add(currentPosition, newItem);
            _targetItem.Add(targetItem, targetItem);
            // Debug.Log("NEW");
        }
        
        _matchPositions = matchPositions;
        _matchItems = matchItems;
        _currentItem = currentPosition.Item;
        // Debug.Log("Создаем  " + _currentItem);
        
        foreach (var item in _matchItems)
        {
            item.Deactivation();
            item.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);
        }
        _currentItem.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);

        // Debug.Log("Создаем 1 " + _currentItem);
        StartCoroutine(MergeActivation(currentPosition,targetItem));
        
        
        
        /*
        foreach (var item in _matchItems)
        {
            item.Deactivation();
            item.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);
        }
        

        Debug.Log("Создаем 1 " + _currentItem);
        StartCoroutine(MergeActivation(currentPosition));


        if (_coroutines.ContainsKey(currentPosition))
        {
            Debug.Log("Coroutine Faled " + currentPosition);
        }
        else
        {
            Debug.Log("Position " + currentPosition.name);
            Coroutine coroutine = StartCoroutine(MergeActivation(currentPosition));
            _coroutines.Add(currentPosition, coroutine);
        }*/
    }

    private IEnumerator MergeActivation(ItemPosition currentPosition , Item targetItem)
    {
        yield return new WaitForSeconds(0.35f);

        foreach (var itemPosition in _matchPositions)
        {
            itemPosition.ClearingPosition();
        }

        if (_currentItem.ItemName == Items.Crane)
        {
            _currentItem = _targetItem[targetItem];
        }

        // Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        // Item item = Instantiate(_newItem[currentPosition].NextItem, currentPosition.transform.position, Quaternion.identity);
        Debug.Log("Instantiate " + _targetItem[targetItem].name);
        Item item = Instantiate(_targetItem[targetItem].NextItem, currentPosition.transform.position, Quaternion.identity);

        if (item.TryGetComponent(out TreasureChest treasureChest))
            treasureChest.Init(_goldWallet);
        
        // Debug.Log("Instantiate " + _newItem[currentPosition].name);
        item.transform.forward = currentPosition.transform.forward;
        item.Init(currentPosition);
        item.Activation();
        item.GetComponent<ItemAnimation>().PositioningAnimation();
        // Debug.Log("_matchPositions.Count " + _matchItems.Count);
        // Merged?.Invoke(_matchItems.Count, _currentItem.Reward, currentPosition);
        Merged?.Invoke(_matchPositions.Count, _currentItem.Reward, currentPosition);
        // Debug.Log("повторный Мердж" + item.name);
        _lookMerger.LookAround(currentPosition, item);
        // Debug.Log("_matchPositions.Count " + _matchItems.Count);
        yield return new WaitForSeconds(0.1f);
        // Merged?.Invoke(_matchPositions.Count, _currentItem.Reward, currentPosition);
        Mergered?.Invoke();
        ItemMergered?.Invoke(item);
        // StopCoroutine(_coroutines[currentPosition]);
    }


    /*public void Merge(ItemPosition currentPosition, List<ItemPosition> matchPositions, List<Item> matchItems)
    {
        _matchPositions = matchPositions;
        _matchItems = matchItems;
        _currentItem = currentPosition.Item;
        Debug.Log("Создаем  " + _currentItem);
        foreach (var item in _matchItems)
        {
            item.Deactivation();
            item.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);
        }

        Debug.Log("Создаем 1 " + _currentItem);
        StartCoroutine(MergeActivation(currentPosition));
    }

    private IEnumerator MergeActivation(ItemPosition currentPosition)
    {
        yield return new WaitForSeconds(0.35f);

        foreach (var itemPosition in _matchPositions)
        {
            itemPosition.ClearingPosition();
        }

        if (_currentItem.ItemName == Items.Crane)
        {
            _currentItem = _matchItems[0];
        }

        Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        item.transform.forward = currentPosition.transform.forward;
        item.Init(currentPosition);
        item.Activation();
        item.GetComponent<ItemAnimation>().PositioningAnimation();
        // Debug.Log("_matchPositions.Count " + _matchItems.Count);
        Merged?.Invoke(_matchItems.Count, _currentItem.Reward, currentPosition);
        Debug.Log("повторный Мердж" + item.name);
        _lookMerger.LookAround(currentPosition, item);
        // Debug.Log("_matchPositions.Count " + _matchItems.Count);
        yield return new WaitForSeconds(0.1f);
        // Merged?.Invoke(_matchPositions.Count, _currentItem.Reward, currentPosition);
        Mergered?.Invoke();
        ItemMergered?.Invoke(item);
    }*/
}