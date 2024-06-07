using System;
using Enums;
using ItemPositionContent;
using ItemSO;
using TMPro;
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
        [SerializeField] private TMP_Text _rewardText;
        
        private int _price;
        private bool _isActive;

        public event Action Activated;
        public event Action Deactivated;

        public TMP_Text RewardText => _rewardText;

        public int Reward => _reward;

        public Item NextItem => _nextItem;

        public bool IsActive => _isActive;

        public Items ItemName => _itemName;

        public bool IsHouse => _isHouse;

        public int Price => _price;

        public bool IsLightHouse => _isLightHouse;

        public int Gold => _gold;

        public ItemDropDataSO ItemDropDataSo => _itemDropDataSo;

        public ItemPosition ItemPosition { get; private set; }

        public void Activation()
        {
            // Debug.Log("Act");
            _isActive = true;
            Activated?.Invoke();
        }

        public void Deactivation()
        {
            // Debug.Log("Deact");
            _isActive = false;
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

        public void SetName(Items name)
        {
            _itemName = name;
        }

        public void SetPrice(int price)
        {
            _price = price;
        }

        public void IncreasePrice()
        {
            _price = Mathf.RoundToInt(_price * _priceMultiplier);
            ;
        }

        public void SetInitialPrice()
        {
            // Debug.Log("начальная цена");
            _price = _startPrice;
        }
    }
}