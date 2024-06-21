using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemContent;
using PossibilitiesContent;
using UnityEngine;

public class ShopItems : MonoBehaviour
{
    [SerializeField] private List<Item> _items;
    [SerializeField] private PossibilitieBulldozer _possibilitieBulldozer;
    [SerializeField] private PossibilitieReplace _possibilitieReplace;
    
    public List<Item> Items => _items;
    
    public void SetPrice(Items itemName ,int price)
    {
        foreach (var itemObject in _items)
        {
            if(itemObject.ItemName == itemName)
                itemObject.SetPrice(price);
        }
    }
    
    public void SetPricePossibilitie(int priceBulldozer,int priceReplace)
    {
        _possibilitieBulldozer.SetCurrentPrice(priceBulldozer);
        _possibilitieReplace.SetCurrentPrice(priceReplace);
    }
}
