using ItemPositionContent;
using UnityEngine;

public class Territory : MonoBehaviour
{
    [SerializeField] private PositionScaller[] _positionScallers;
    [SerializeField] private ItemPosition[] _itemPositions;

    public ItemPosition[] ItemPositions => _itemPositions;

    public void PositionActivation()
    {
        foreach (var positionScaller in _positionScallers)
        {
            positionScaller.gameObject.SetActive(true);
            positionScaller.ScaleChanged();
        }  
    }
    
    public void ShowPositions()
    {
        foreach (var positionScaller in _positionScallers)
        {
            positionScaller.gameObject.SetActive(true);
        }  
    }
    
    public void PositionDeactivation()
    {
        foreach (var positionScaller in _positionScallers)
        {
            positionScaller.gameObject.SetActive(false);
        }  
    }
}