using UnityEngine;

public class Territory : MonoBehaviour
{
    [SerializeField] private PositionScaller[] _positionScallers;

    public void PositionActivation()
    {
        foreach (var positionSclaScaller in _positionScallers)
        {
            positionSclaScaller.gameObject.SetActive(true);
            positionSclaScaller.ScaleChanged();
        }  
    }
}