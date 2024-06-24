using System;
using Enums;
using ItemPositionContent;
using ItemSO;
using TMPro;
using UI;
using UnityEngine;

namespace ItemContent
{
    [RequireComponent(typeof(AudioSource))]
    public class Item : MonoBehaviour
    {
        [SerializeField] private Items _itemName;
        [SerializeField] private Item _nextItem;
        [SerializeField] private int _reward;
        [SerializeField] private ItemDropDataSO _itemDropDataSo;
        [SerializeField] private bool _isHouse;
        [SerializeField] private bool _isLightHouse;
        [SerializeField] private int _gold;
        [SerializeField] private int _startPrice;
        [SerializeField] private float _priceMultiplier;
        [SerializeField] private FlightScore _flightScore;
        [SerializeField] private bool _isBigHouse;

        public event Action Activated;
        
        public event Action Deactivated;

        public int StartPrice => _startPrice;

        public bool IsBigHouse => _isBigHouse;

        public FlightScore FlightScore => _flightScore;

        public int Reward => _reward;

        public Item NextItem => _nextItem;

        public bool IsActive { get; private set; }

        public Items ItemName => _itemName;

        public bool IsHouse => _isHouse;

        public int Price { get; private set; }

        public bool IsLightHouse => _isLightHouse;

        public int Gold => _gold;

        public ItemDropDataSO ItemDropDataSo => _itemDropDataSo;

        public ItemPosition ItemPosition { get; private set; }

        public void Activation()
        {
            IsActive = true;
            Activated?.Invoke();
        }

        public void Deactivation()
        {
            IsActive = false;
            Deactivated?.Invoke();
        }

        public void Init(ItemPosition itemPosition)
        {
            ItemPosition = itemPosition;
        }

        public void ClearPosition()
        {
            if (ItemPosition != null)
                ItemPosition = null;
        }

        public void SetGold(int gold)
        {
            _gold = gold;
        }

        public void SetPrice(int price)
        {
            Debug.Log("SetPrice");
            Price = price;
        }

        public void IncreasePrice()
        {
            Debug.Log("SetIncreasePrice");
            Price = Mathf.RoundToInt(Price * _priceMultiplier);
        }

        public void SetInitialPrice()
        {
            Debug.Log("SetInitialPrice");
            Price = _startPrice;
        }
    }
}