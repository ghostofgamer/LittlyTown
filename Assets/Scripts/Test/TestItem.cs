using System;
using Enums;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    [SerializeField]private Items _itemName;
    [SerializeField] private TestItem _nextItem;
    [SerializeField] private ItemPosition _itemPosition;
    
    private bool _isActive;

    public event Action Activated;
    
    // public Item NextItem => _nextItem;
    
    public bool IsActive => _isActive;

    public Items ItemName => _itemName;
    
    public void Activation()
    {
        _isActive = true;
        Activated?.Invoke();
        // Debug.Log("Activation");
    }
    public void Deactivation()
    {
        _isActive = false;
      
        // Debug.Log("Activation");
    }
}
