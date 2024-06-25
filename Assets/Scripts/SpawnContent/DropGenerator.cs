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

        public ItemDropDataSo ItemDropData { get; private set; }
        
        public ItemDropDataSo ItemDropDataNew { get; private set; }

        private void Awake()
        {
            _nextItem = _itemDropsSO[0].PrefabItem;
            ItemDropDataNew = _nextItem.ItemDropDataSo;
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
            float totalChance = 0f;

            foreach (ItemDropDataSo itemDrop in _itemDropsSO)
            {
                float dropChance = CalculateDropChance(itemDrop, _currentLevel);
                totalChance += dropChance;
            }

            float randomPoint = Random.value * totalChance;

            for (int i = 0; i < _itemDropsSO.Count; i++)
            {
                randomPoint -= CalculateDropChance(_itemDropsSO[i], _currentLevel);

                if (randomPoint <= 0)
                {
                    _image.sprite = _itemDropsSO[i].Icon;
                    return _itemDropsSO[i];
                }
            }

            return _itemDropsSO[0];
        }

        private float CalculateDropChance(ItemDropDataSo itemDrop, int level)
        {
            float dropChance = itemDrop.BaseDropChance;
            dropChance += level * itemDrop.LevelIncreaseFactor;
            dropChance -= level * itemDrop.LevelDecreaseFactor;
            dropChance = Mathf.Clamp01(dropChance);
            return dropChance;
        }

        private void Reset()
        {
            _nextItem = _itemDropsSO[0].PrefabItem;
            ItemDropDataNew = _nextItem.ItemDropDataSo;
            _image.sprite = _itemDropsSO[0].Icon;
        }
    }
}