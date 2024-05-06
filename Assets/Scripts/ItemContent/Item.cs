using System;
using Enums;
using ItemPositionContent;
using UnityEngine;

namespace ItemContent
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Items _itemName;
        [SerializeField] private Item _nextItem;
        [SerializeField] private int _reward;

        private bool _isActive;

        public event Action Activated;
        public event Action Deactivated;

        public int Reward => _reward;

        public Item NextItem => _nextItem;

        public bool IsActive => _isActive;

        public Items ItemName => _itemName;

        public ItemPosition ItemPosition { get; private set; }

        public void Activation()
        {
            _isActive = true;
            Activated?.Invoke();
        }

        public void Deactivation()
        {
            _isActive = false;
            Deactivated?.Invoke();
        }

        public void Init(ItemPosition itemPosition)
        {
            ItemPosition = itemPosition;
        }
    }
}