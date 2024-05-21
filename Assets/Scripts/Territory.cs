using UnityEngine;

public class Territory : MonoBehaviour
{
    [SerializeField] private PositionScaller[] _positionScallers;

    public void PositionActivation()
    {
        foreach (var positionScaller in _positionScallers)
        {
            positionScaller.gameObject.SetActive(true);
            positionScaller.ScaleChanged();
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