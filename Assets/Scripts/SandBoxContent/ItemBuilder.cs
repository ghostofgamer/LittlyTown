using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class ItemBuilder : Builder
{
    [SerializeField] private Item[] _items;

    private Item _item;

    public event Action ItemBuilded;
    
    private void OnEnable()
    {
        StartCoroutine(ActivatedItems());
    }

    public void SetItems(Items itemName)
    {
        foreach (var item in _items)
        {
            if (item.ItemName.ToString() == itemName.ToString())
                _item = item;
        }
    }

    protected override void FirstChoose()
    {
        SetItems(Items.Bush);
    }

    protected override void TakeAction(ItemPosition itemPosition)
    {
        if (itemPosition.IsBusy)
        {
            Debug.Log("Занят");
            return;
        }

        if (_item.ItemName == Items.LightHouse)
        {
            if (itemPosition.IsElevation)
            {
                Vector3 newLocalPosition = new Vector3(itemPosition.transform.localPosition.x, 0.59f,
                    itemPosition.transform.localPosition.z);
                itemPosition.transform.localPosition = newLocalPosition;
            }

            itemPosition.DeactivationAll();
            itemPosition.ActivationWater();
            ItemPosition itemPositionTile = Instantiate(TileWater, itemPosition.transform.position,
                Quaternion.identity, RoadContainer);
            itemPosition.SetRoad(itemPositionTile);
        }
        else
        {
            if (itemPosition.IsRoad || itemPosition.IsTrail || itemPosition.IsWater)
            {
                itemPosition.DeactivationAll();
                ItemPosition itemPositionTile = Instantiate(ClearTile, itemPosition.transform.position,
                    Quaternion.identity, RoadContainer);
                itemPosition.SetRoad(itemPositionTile);
            }
        }

        itemPosition.OnBusy();
        Item item = Instantiate(_item, itemPosition.transform.position, Container.transform.rotation, Container);
        item.GetComponent<ItemAnimation>().PositioningAnimation();
        item.Activation();
        StartRoadGeneration();
        ItemBuilded?.Invoke();
    }

    private IEnumerator ActivatedItems()
    {
        yield return new WaitForSeconds(0.5f);

        List<Item> items = new List<Item>();

        for (int i = 0; i < Container.childCount; i++)
            items.Add(Container.GetChild(i).GetComponent<Item>());

        foreach (var item in items)
            item.Activation();
    }
}