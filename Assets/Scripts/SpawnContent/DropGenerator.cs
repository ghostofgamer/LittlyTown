using System.Collections.Generic;
using ItemContent;
using ItemSO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DropGenerator : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<ItemDropDataSO> _itemDropsSO;

    private int _currentLevel;
    private Item _currentItem;
    private Item _nextItem;

    private void Awake()
    {
        _nextItem = _itemDropsSO[0].PrefabItem;
        _image.sprite = _itemDropsSO[0].Icon;
    }

    public Item GetItem()
    {
        _currentItem = _nextItem;
        _nextItem = DropItem();
        return _currentItem;
    }

    public void NextLevel()
    {
        _currentLevel++;
    }

    private Item DropItem()
    {
        float totalChance = 0f;

        foreach (ItemDropDataSO itemDrop in _itemDropsSO)
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
                return _itemDropsSO[i].PrefabItem;
            }
        }

        return _itemDropsSO[0].PrefabItem;
    }

    private float CalculateDropChance(ItemDropDataSO itemDrop, int level)
    {
        float dropChance = itemDrop.BaseDropChance;
        dropChance += level * itemDrop.LevelIncreaseFactor;
        dropChance -= level * itemDrop.LevelDecreaseFactor;
        dropChance = Mathf.Clamp01(dropChance);
        return dropChance;
    }
}