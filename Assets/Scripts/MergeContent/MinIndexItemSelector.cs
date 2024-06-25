using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace MergeContent
{
    public class MinIndexItemSelector : MonoBehaviour
    {
        private List<Item> _temporaryItems = new List<Item>();
        private Item _temporaryItem;

        public List<Item> GetTemporaryItems(ItemPosition[] itemPositions)
        {
            _temporaryItems.Clear();

            foreach (var arroundPosition in itemPositions)
            {
                if (arroundPosition == null)
                    continue;

                if (arroundPosition.Item != null)
                    _temporaryItems.Add(arroundPosition.Item);
            }

            return _temporaryItems;
        }

        public Item GetItemMinIndex(ItemPosition[] itemPositions)
        {
            return GetItemMinIndex(GetTemporaryItems(itemPositions));
        }

        public Item GetItemMinIndex(List<Item> items)
        {
            int minIndex = int.MaxValue;

            foreach (Item temporaryItem in items)
            {
                int index = (int) temporaryItem.ItemName;

                if (minIndex > index)
                {
                    minIndex = index;
                    _temporaryItem = temporaryItem;
                }
            }

            return _temporaryItem;
        }
    }
}