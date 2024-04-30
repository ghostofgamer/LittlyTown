using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Merge : MonoBehaviour
{
    private ItemPosition _currentItemPosition;
    private List<Item> _matchedItems = new List<Item>();

    /*private List<ItemPosition> _currentCheckedPositions = new List<ItemPosition>();
    private List<ItemPosition> _previousCheckedPositions = new List<ItemPosition>();*/
    /*private int _recursionDepth = 0;
    private int _maxRecursionDepth = 10;*/
    private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
    private List<ItemPosition> _position = new List<ItemPosition>();

    public void SetPosition(ItemPosition itemPosition)
    {
        _currentItemPosition = itemPosition;
        StartCoroutine(Looks());
        // LookAround();
    }
    
    private void LookAround(ItemPosition currentPosition)
    {
        if (_checkedPositions.Contains(currentPosition))
        {
            return;
        }

        _checkedPositions.Add(currentPosition);

        // _recursionDepth++;

        // if (_recursionDepth > _maxRecursionDepth)
        // {
        //     Debug.Log("Max recursion depth reached!");
        //     _recursionDepth--;
        //     return;
        // }

        // if (_currentCheckedPositions.Contains(currentPosition))
        // {
        //     _recursionDepth--;
        //     return;
        // }

        // _previousCheckedPositions.AddRange(_currentCheckedPositions);
        // _currentCheckedPositions.Clear();
        // _currentCheckedPositions.Add(currentPosition);

        if (currentPosition.Item == null)
        {
            Debug.Log("NULL Currint");
            // _recursionDepth--;
            return;
        }
        // Debug.Log("Current " + currentPosition.Item);

        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
                continue;

            Debug.Log("ПОЗИЦИЯ   ===   " + arroundPosition.name);
            
            /*if (_checkedPositions.Contains(arroundPosition))
            {
                return;
            }*/

            // _checkedPositions.Add(arroundPosition);
            
            Debug.Log("пров");
            /*if (currentPosition.Item == null)
            {
                Debug.Log("NULL Currint");
                continue;
            }*/

            /*if (arroundPosition.Item == null)
            {
                // Debug.Log("NULL тут ");
                // Debug.Log("NullItem arroundPosition " + arroundPosition.name);
                continue;
            }*/

            if (arroundPosition.Item != null && currentPosition.Item.ItemName.Equals(arroundPosition.Item.ItemName))
            {
                Debug.Log("Попал   " + arroundPosition.name);
                _position.Add(arroundPosition);
                _matchedItems.Add(arroundPosition.Item);
                LookAround(arroundPosition);
                _checkedPositions.Add(arroundPosition);
            }

            if (_checkedPositions.Contains(arroundPosition))
            {
                return;
            }

            _checkedPositions.Add(arroundPosition);
        }

        /*_currentCheckedPositions.Clear();
        _currentCheckedPositions.AddRange(_previousCheckedPositions);
        _recursionDepth--;*/
        /*foreach (var matchedItem in _matchedItems)
        {
            Debug.Log(matchedItem.name);
        }
        Debug.Log(_matchedItems.Count);*/
    }

    private IEnumerator Looks()
    {
        yield return new WaitForSeconds(0.15f);
        _matchedItems.Clear();
        _checkedPositions.Clear();
        _position.Clear();
        Debug.Log("Start   " + _matchedItems.Count);
        // _position.Add(_currentItemPosition);
        // _matchedItems.Add(_currentItemPosition.Item);
        // LookAround(_currentItemPosition);
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
        if (_position.Count > 0)
        {
            foreach (var item in _position)
            {
                Debug.Log(item.gameObject.name + " at position " + item.Item.name);
            }
        }

        if (_position.Count >= 3)
        {
            foreach (var item in _position)
            {
                Debug.Log("ТУТАТЭ " + _position.Count);
                Debug.Log("Name " + this.name);
                Debug.Log("ItemName " + item.name);
                item.Item.gameObject.SetActive(false);
                Debug.Log("3 ");
                item.ClearingItem();
                Debug.Log("5 ");
            }
        }
    }

    private void TestLookAround(ItemPosition currentPosition)
    {
        Debug.Log("Проверяем позицию " + currentPosition);
        
        if (_checkedPositions.Contains(currentPosition))
        {
            Debug.Log("Return ---------------------------------");
            return;
        }

        _checkedPositions.Add(currentPosition);
        _position.Add(currentPosition);

        if (currentPosition.Item == null)
        {
            Debug.Log("NULL Currint");
            // _recursionDepth--;
            return;
        }

        if (currentPosition.Item != null)
        {
            Debug.Log("Поднял");
            _matchedItems.Add(currentPosition.Item);
        }

        foreach (var arroundPosition in currentPosition.ItemPositions)
        {
            if (arroundPosition == null)
                continue;

            Debug.Log("ПОЗИЦИЯ   ===   " + arroundPosition.name);
            
            Debug.Log("пров");

            if (arroundPosition.Item != null && currentPosition.Item.ItemName.Equals(arroundPosition.Item.ItemName))
            {
                Debug.Log("Попал   " + arroundPosition.name);
                // _position.Add(arroundPosition);
                // _matchedItems.Add(arroundPosition.Item);
                TestLookAround(arroundPosition);
                // _checkedPositions.Add(arroundPosition);
            }

            // if (_checkedPositions.Contains(arroundPosition))
            // {
            //     return;
            // }
            //
            // _checkedPositions.Add(arroundPosition);
        }
    }
}