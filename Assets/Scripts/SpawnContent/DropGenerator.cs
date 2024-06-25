using System.Collections.Generic;
using ItemContent;
using ItemSO;
using MapsContent;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SpawnContent
{
    public class DropGenerator : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private List<ItemDropDataSo> _itemDropsSO;
        [SerializeField] private StartMap _startMap;

        private int _currentLevel;
        private Item _currentItem;
        private Item _nextItem;
        private float _randomPoint;
        private float _totalChance;
        private float _dropChance;

        public ItemDropDataSo ItemDropData { get; private set; }

        private void Awake()
        {
            _nextItem = _itemDropsSO[0].PrefabItem;
            _image.sprite = _itemDropsSO[0].Icon;
        }

        private void OnEnable()
        {
            _startMap.MapStarted += Reset;
        }

        private void OnDisable()
        {
            _startMap.MapStarted -= Reset;
        }

        public Item GetItem()
        {
            _currentItem = _nextItem;
            ItemDropData = DropItem();
            _nextItem = ItemDropData.PrefabItem;
            return _currentItem;
        }

        public void SetItem(Item itemPrefab, Sprite sprite)
        {
            _nextItem = itemPrefab;
            _image.sprite = sprite;

            foreach (var itemDropData in _itemDropsSO)
            {
                if (itemDropData.PrefabItem.ItemName == itemPrefab.ItemName)
                    ItemDropData = itemDropData;
            }
        }

        public void NextLevel(int value)
        {
            _currentLevel = value;
        }

        public void ResetLevel()
        {
            _currentLevel = 0;
        }

        private ItemDropDataSo DropItem()
        {
            _totalChance = 0f;

            foreach (ItemDropDataSo itemDrop in _itemDropsSO)
            {
                _dropChance = CalculateDropChance(itemDrop, _currentLevel);
                _totalChance += _dropChance;
            }

            _randomPoint = Random.value * _totalChance;

            for (int i = 0; i < _itemDropsSO.Count; i++)
            {
                _randomPoint -= CalculateDropChance(_itemDropsSO[i], _currentLevel);

                if (_randomPoint <= 0)
                {
                    _image.sprite = _itemDropsSO[i].Icon;
                    return _itemDropsSO[i];
                }
            }

            return _itemDropsSO[0];
        }

        private float CalculateDropChance(ItemDropDataSo itemDrop, int level)
        {
            _dropChance = itemDrop.BaseDropChance;
            _dropChance += level * itemDrop.LevelIncreaseFactor;
            _dropChance -= level * itemDrop.LevelDecreaseFactor;
            _dropChance = Mathf.Clamp01(_dropChance);
            return _dropChance;
        }

        private void Reset()
        {
            _nextItem = _itemDropsSO[0].PrefabItem;
            _image.sprite = _itemDropsSO[0].Icon;
        }
    }
}