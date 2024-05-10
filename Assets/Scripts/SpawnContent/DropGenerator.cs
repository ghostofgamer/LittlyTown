using System;
using System.Collections;
using System.Collections.Generic;
using ItemContent;
using SpawnContent;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DropGenerator : MonoBehaviour
{
    [SerializeField ] private Image _image;
    
    public List<ItemDrop> itemDrops;
    public int currentLevel;
    private Item _currentItem;
    private Item _nextItem;

    private void Awake()
    {
        _nextItem = itemDrops[0].PrefabItem;
        _image.sprite = itemDrops[0].Icon;
    }

    public Item GetItem()
    {
        _currentItem = _nextItem;
        _nextItem = DropItem().PrefabItem;
        return _currentItem;
    }

    public ItemDrop DropItem()
    {
        float totalChance = 0f;
        foreach (ItemDrop itemDrop in itemDrops)
        {
            float dropChance = CalculateDropChance(itemDrop, currentLevel);
            totalChance += dropChance;
        }

        float randomPoint = Random.value * totalChance;

        for (int i = 0; i < itemDrops.Count; i++)
        {
            randomPoint -= CalculateDropChance(itemDrops[i], currentLevel);
            if (randomPoint <= 0)
            {
                Debug.Log("Дропнулся предмет: " + itemDrops[i].ItemName);
                // Дропнуть предмет itemDrops[i].itemName
                _image.sprite = itemDrops[i].Icon;
                return itemDrops[i];
            }
        }

        return itemDrops[0];
    }

    float CalculateDropChance(ItemDrop itemDrop, int level)
    {
        float dropChance = itemDrop.BaseDropChance;
        dropChance += level * itemDrop.LevelDecreaseFactor;
        dropChance -= level * itemDrop.LevelDecreaseFactor;
        dropChance = Mathf.Clamp01(dropChance); // Ограничить значение между 0 и 1
        return dropChance;
    }
}
