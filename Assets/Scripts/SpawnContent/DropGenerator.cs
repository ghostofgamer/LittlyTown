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

    public ItemDropDataSO ItemDropData { get; private set; }
    public ItemDropDataSO ItemDropDataNew { get; private set; }

    private void Awake()
    {
        _nextItem = _itemDropsSO[0].PrefabItem;
        ItemDropDataNew = _nextItem.ItemDropDataSo;
        _image.sprite = _itemDropsSO[0].Icon;
    }

    /*public Item GetItem()
    {
        Debug.Log("GETITEM   " + _nextItem.ItemName);
        _currentItem = _nextItem;
        _nextItem = DropItem();
        return _currentItem;
    }*/

    public Item GetItem()
    {
        // Debug.Log("GETITEM   " + _nextItem.ItemName);
        _currentItem = _nextItem;
        ItemDropData = DropItem();
        // Debug.Log("ITEMDROPGENERATION " + ItemDropData);
        _nextItem = ItemDropData.PrefabItem;
        
        return _currentItem;
    }

    /*public void SetItem()
    {
        // Debug.Log("SETITEM   "+ itemDropData.PrefabItem.ItemName );
        // _currentItem = itemDropData.PrefabItem;
        _nextItem= itemDropData.PrefabItem;
        _image.sprite = itemDropData.Icon;
        ItemDropData = itemDropData;
        // Debug.Log("SETSO " + ItemDropData);
    }*/
    
    public void SetItem(Item itemPrefab,Sprite sprite)
    {
        _nextItem= itemPrefab;
        _image.sprite = sprite;

        foreach (var itemDropData in _itemDropsSO)
        {
            if (itemDropData.PrefabItem.ItemName == itemPrefab.ItemName)
                ItemDropData = itemDropData;
        }
    }
    
    public void NextLevel()
    {
        _currentLevel++;
    }

    public void ResetLevel()
    {
        _currentLevel = 0;
    }

    private ItemDropDataSO DropItem()
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
                // ItemDropData = _itemDropsSO[i];
                _image.sprite = _itemDropsSO[i].Icon;
                return _itemDropsSO[i];
            }
        }

        return _itemDropsSO[0];
    }
    
    /*private Item DropItem()
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
                ItemDropData = _itemDropsSO[i];
                Debug.Log("ITEMDROPGENERATION " + ItemDropData);
                _image.sprite = _itemDropsSO[i].Icon;
                return _itemDropsSO[i].PrefabItem;
            }
        }

        return _itemDropsSO[0].PrefabItem;
    }*/

    private float CalculateDropChance(ItemDropDataSO itemDrop, int level)
    {
        float dropChance = itemDrop.BaseDropChance;
        dropChance += level * itemDrop.LevelIncreaseFactor;
        dropChance -= level * itemDrop.LevelDecreaseFactor;
        dropChance = Mathf.Clamp01(dropChance);
        return dropChance;
    }
}