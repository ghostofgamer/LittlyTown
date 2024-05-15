using System;
using System.Collections;
using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class Merger : MonoBehaviour
{
    [SerializeField] private PositionMatcher _positionMatcher;
    [SerializeField] private LookMerger _lookMerger;

    private Item _currentItem;
    private List<ItemPosition> _matchPositions = new List<ItemPosition>();
    private List<Item> _matchItems = new List<Item>();

    public event Action<int, int, ItemPosition> Merged;

    public event Action Mergered;

    public event Action<Item> ItemMergered;

    public void Merge(ItemPosition currentPosition, List<ItemPosition> matchPositions, List<Item> matchItems)
    {
        _matchPositions = matchPositions;
        _matchItems = matchItems;
        _currentItem = currentPosition.Item;

        /*foreach (var itemPosition in _matchPositions)
        {
            itemPosition.Item.Deactivation();
            itemPosition.Item.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);
        }*/
        
        foreach (var item in _matchItems)
        {
            item.Deactivation();
            item.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);
        }

        StartCoroutine(MergeActivation(currentPosition));
    }

    private IEnumerator MergeActivation(ItemPosition currentPosition)
    {
        yield return new WaitForSeconds(0.35f);

        foreach (var itemPosition in _matchPositions)
        {
            // itemPosition.Item.gameObject.SetActive(false);
            itemPosition.ClearingPosition();
        }

        Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);

        item.transform.forward = currentPosition.transform.forward;
        item.Init(currentPosition);
        item.Activation();
        item.GetComponent<ItemAnimation>().PositioningAnimation();
        // _positionMatcher.LookAround(currentPosition);
        _lookMerger.LookAround(currentPosition, item);
        yield return new WaitForSeconds(0.1f);
        // Debug.Log("MergeActivation");
        Merged?.Invoke(_matchPositions.Count, _currentItem.Reward, currentPosition);
        Mergered?.Invoke();
        ItemMergered?.Invoke(item);
    }


    /*public void Merge(ItemPosition currentPosition)
    {
        _matchPositions = _positionMatcher.Positions;
        _currentItem = currentPosition.Item;

        foreach (var itemPosition in _matchPositions)
        {
            itemPosition.Item.Deactivation();
            itemPosition.Item.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);
        }

        StartCoroutine(MergeActivation(currentPosition));
    }

    private IEnumerator MergeActivation(ItemPosition currentPosition)
    {
        yield return new WaitForSeconds(0.35f);

        foreach (var itemPosition in _matchPositions)
        {
            itemPosition.ClearingPosition();
        }

        Debug.Log("ИМЯ " + _currentItem.ItemName);
        Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        item.transform.forward = currentPosition.transform.forward;
        item.Init(currentPosition);
        item.Activation();
        item.GetComponent<ItemAnimation>().PositioningAnimation();
        // _positionMatcher.LookAround(currentPosition);
        yield return new WaitForSeconds(0.1f);
        // Debug.Log("MergeActivation");
        Merged?.Invoke(_matchPositions.Count, _currentItem.Reward,currentPosition);
        Mergered?.Invoke();
        ItemMergered?.Invoke(item);
    }*/
}