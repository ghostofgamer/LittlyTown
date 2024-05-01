using System;
using Enums;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]private Items _itemName;
    [SerializeField] private Item _nextItem;
    
    private bool _isActive;

    public event Action Activated;
    
    public Item NextItem => _nextItem;
    
    public bool IsActive => _isActive;

    public Items ItemName => _itemName;
    
    public void Activation()
    {
        _isActive = true;
        Activated?.Invoke();
        Debug.Log("Activation");
    }
}
