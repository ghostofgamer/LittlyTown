using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]private Items _itemName;
    
    private bool _isActive;

    public bool IsActive => _isActive;

    public Items ItemName => _itemName;
    
    public void Activation()
    {
        _isActive = true;
    }
}
