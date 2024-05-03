using System;
using System.Collections;
using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class Merger : MonoBehaviour
{
    [SerializeField] private PositionMatcher _positionMatcher;
    
    private Item _currentItem;
    private List<ItemPosition> _matchPositions = new List<ItemPosition>();

    public event Action Merged;

    public void Merge(ItemPosition currentPosition)
    {
        _matchPositions = _positionMatcher.Positions;
        _currentItem = currentPosition.Item;


        foreach (var itemPosition in _matchPositions)
        {
            itemPosition.Item.Deactivation();
            itemPosition.Item.GetComponent<ItemMoving>().MoveTarget(currentPosition.transform.position);
        }

        StartCoroutine(MergeActivation(currentPosition));
        
        /*foreach (var itemPosition in _matchPositions)
        {
            itemPosition.Item.gameObject.SetActive(false);
            itemPosition.Item.Deactivation();
            itemPosition.ClearingItem();
        }

        Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        item.Activation();
        _positionMatcher.TryMerge(currentPosition);
        Merging?.Invoke();*/
    }

    private IEnumerator MergeActivation(ItemPosition currentPosition)
    {
        yield return new WaitForSeconds(0.35f);

        foreach (var itemPosition in _matchPositions)
        {
            itemPosition.ClearingPosition();
        }

        Item item = Instantiate(_currentItem.NextItem, currentPosition.transform.position, Quaternion.identity);
        item.Activation();
        _positionMatcher.LookAround(currentPosition);
        yield return new WaitForSeconds(0.1f);
        Merged?.Invoke();
    }
}