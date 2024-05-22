using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemContent;
using UnityEngine;

public class ShopItems : MonoBehaviour
{
    [SerializeField] private List<Item> _items;

    public List<Item> Items => _items;
    
    public void SetPrice(Items itemName ,int price)
    {
        foreach (var itemObject in _items)
        {
            if(itemObject.ItemName == itemName)
                itemObject.SetPrice(price);
        }
    }
}
