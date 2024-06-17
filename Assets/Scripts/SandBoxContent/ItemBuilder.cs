using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemBuilder : MonoBehaviour
{
    [SerializeField] private Item[] _items;
    [SerializeField] private Transform _container;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private Transform _roadContainer;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private ItemPosition _clearTile;
    [SerializeField] private ItemPosition _tileWater;

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
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("ГГГ");
                return;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy)
                    {
                        Debug.Log("Занят");
                        return;
                    }

                    if (itemPosition.IsRoad || itemPosition.IsTrail || itemPosition.IsWater || itemPosition.IsElevation)
                    {
                        if (_item.ItemName == Items.LightHouse)
                        {
                            itemPosition.DeactivationAll();
                            ItemPosition itemPositionTile = Instantiate(_tileWater, itemPosition.transform.position,
                                Quaternion.identity, _container);
                            itemPosition.SetRoad(itemPositionTile);
                        }
                        else
                        {
                            itemPosition.DeactivationAll();
                            ItemPosition itemPositionTile = Instantiate(_clearTile, itemPosition.transform.position,
                                Quaternion.identity, _container);
                            itemPosition.SetRoad(itemPositionTile);
                        }
                    }

                    Debug.Log("Свободен");
                    Item item = Instantiate(_item, itemPosition.transform.position, Quaternion.identity, _container);
                    item.GetComponent<ItemAnimation>().PositioningAnimation();
                    item.Activation();
                    _roadGenerator.GenerateSandBoxTrail(_itemPositions, _roadContainer);
                }
            }
        }
    }
}