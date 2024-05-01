using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge : MonoBehaviour
{
    [SerializeField] private ItemDragger _itemDragger;
    
    private ItemPosition _currentItemPosition;
    private List<Item> _matchedItems = new List<Item>();

    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _position = new List<ItemPosition>();

    private Item _currentItem;

    public event Action Merging;
    
    private void Start()
    {
    }

    public void SetPosition(ItemPosition itemPosition)
    {
        _currentItemPosition = itemPosition;
        StartCoroutine(Looks());
    }

    private IEnumerator Looks()
    {
        yield return new WaitForSeconds(0.15f);
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _position.Clear();
        TestLookAround(_currentItemPosition);
        ShowMatchedItems();
        Show();
    }

    private void ShowMatchedItems()
    {
        if (_matchedItems.Count == 0)
        {
            Debug.Log("No matched items found");
            return;
        }

        foreach (var item in _matchedItems)
        {
            Debug.Log(item.gameObject.name + " at position " + _matchedItems.Count);
        }
    }

    private void Show()
    {
        if (_position.Count < 3)
            return;

        foreach (var itemPosition in _position)
        {
            itemPosition.Item.gameObject.SetActive(false);
            itemPosition.ClearingItem();
        }

        Item item = Instantiate(_currentItem.NextItem, _currentItemPosition.transform.position, Quaternion.identity);
        item.Activation();
        SetPosition(_currentItemPosition);
        Merging?.Invoke();
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