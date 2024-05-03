using System;
using Enums;
using UnityEngine;

namespace ItemContent
{
    public class Item : MonoBehaviour
    {
        [SerializeField]private Items _itemName;
        [SerializeField] private Item _nextItem;

        private bool _isActive;

        public event Action Activated;
        public event Action Deactivated;
    
        public Item NextItem => _nextItem;
    
        public bool IsActive => _isActive;

        public Items ItemName => _itemName;
    
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
    }
}
