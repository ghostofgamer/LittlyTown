using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class ItemBuilder : MonoBehaviour
{
    [SerializeField] private Item[] _items;
    [SerializeField] private Transform _container;

    private Item _item;
    private int _layerMask;
    private int _layer = 3;

    private void Start()
    {
        _layerMask = 1 << _layer;
        _layerMask = ~_layerMask;
    }

    public void SetItems(Items itemName)
    {
        foreach (var item in _items)
        {
            if (item.ItemName.ToString() == itemName.ToString())
            {
                _item = item;
                Debug.Log("Item " + item.name + " " + _item.name);
            }
        }
        Debug.Log("Item ИМЯ " + itemName);
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                {
                    Item item = Instantiate(_item, itemPosition.transform.position, Quaternion.identity, _container);
                    item.Activation();
                }
            }
        }*/
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy)
                    {
                         Debug.Log("Занят"); 
                         return;
                    }
                    
                    Debug.Log("Свободен"); 
                    Item item = Instantiate(_item, itemPosition.transform.position, Quaternion.identity, _container);
                    item.GetComponent<ItemAnimation>().PositioningAnimation();
                    item.Activation();
                }
            }
        }
    }
}