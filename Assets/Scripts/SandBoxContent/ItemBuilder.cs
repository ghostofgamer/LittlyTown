using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace SandBoxContent
{
    public class ItemBuilder : Builder
    {
        [SerializeField] private Item[] _items;

        private Item _item;
        private Vector3 _newLocalPosition;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);
        private float _minPositionElevationY = 0.59f;

        public event Action ItemBuilded;

        private void OnEnable()
        {
            StartCoroutine(ActivatedItems());
        }

        public void SetItems(Items itemName)
        {
            foreach (Item item in _items)
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
                return;

            if (_item.ItemName == Items.LightHouse)
            {
                if (itemPosition.IsElevation)
                {
                    _newLocalPosition = new Vector3(
                        itemPosition.transform.localPosition.x,
                        _minPositionElevationY,
                        itemPosition.transform.localPosition.z);
                    itemPosition.transform.localPosition = _newLocalPosition;
                }

                itemPosition.DeactivationAll();
                itemPosition.ActivationWater();
                ItemPosition itemPositionTile = Instantiate(
                    TileWater,
                    itemPosition.transform.position,
                    Quaternion.identity,
                    RoadContainer);
                itemPosition.SetRoad(itemPositionTile);
            }
            else
            {
                if (itemPosition.IsRoad || itemPosition.IsTrail || itemPosition.IsWater)
                {
                    itemPosition.DeactivationAll();
                    ItemPosition itemPositionTile = Instantiate(
                        ClearTile,
                        itemPosition.transform.position,
                        Quaternion.identity,
                        RoadContainer);
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
            yield return _waitForSeconds;
            List<Item> items = new List<Item>();

            for (int i = 0; i < Container.childCount; i++)
                items.Add(Container.GetChild(i).GetComponent<Item>());

            foreach (Item item in items)
                item.Activation();
        }
    }
}